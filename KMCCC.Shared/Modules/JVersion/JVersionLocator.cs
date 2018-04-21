﻿namespace KMCCC.Modules.JVersion
{
	#region

	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using Launcher;
	using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Tools;

	#endregion

	/// <summary>
	///     默认的版本定位器
	/// </summary>
	public class JVersionLocator : IVersionLocator
	{
		private readonly HashSet<string> _locatingVersion;

		private readonly Dictionary<string, Version> _versions;

		public JVersionLocator()
		{
			_versions = new Dictionary<string, Version>();
			_locatingVersion = new HashSet<string>();
		}

		public string GameRootPath { get; set; }

		private LauncherCore _core;

		public LauncherCore Core
		{
			set
			{
				GameRootPath = value.GameRootPath;
				_core = value;
			}
		}

		public Version Locate(string id)
		{
			lock (_locatingVersion)
			{
				return GetVersionInternal(id);
			}
		}

		public IEnumerable<Version> GetAllVersions()
		{
			try
			{
				lock (_locatingVersion)
				{
					return new DirectoryInfo(GameRootPath + @"\versions").EnumerateDirectories()
						.Select(dir => GetVersionInternal(dir.Name)).Where(item => item != null);
				}
			}
			catch
			{
				return new Version[0];
			}
		}

		/// <summary>
		///     获取Version信息，当出现错误时会返回null
		/// </summary>
		/// <param name="id">版本id</param>
		/// <returns>Version的信息</returns>
		internal Version GetVersionInternal(string id)
		{
			try
			{
				if (_locatingVersion.Contains(id))
				{
					return null;
				}
				_locatingVersion.Add(id);

				Version version;
				if (_versions.TryGetValue(id, out version))
				{
					return version;
				}

				var jver = LoadVersion(_core.GetVersionJsonPath(id));
				if (jver == null)
				{
					return null;
				}

				version = new Version();
				if (string.IsNullOrWhiteSpace(jver.Id))
				{
					return null;
				}
                if (jver.arguments == null && string.IsNullOrWhiteSpace(jver.MinecraftArguments))
                {
                    return null;
                }
				if (string.IsNullOrWhiteSpace(jver.MainClass))
				{
					return null;
				}
				if (string.IsNullOrWhiteSpace(jver.Assets))
				{
					jver.Assets = "legacy";
				}
				if (jver.Libraries == null)
				{
					return null;
				}
				version.Id = jver.Id;
                version.Type = jver.Type;
                version.MinecraftArguments = jver.MinecraftArguments ?? UsefulTools.PrintfArray(jver.arguments.game);
                version.Assets = jver.Assets;
				version.MainClass = jver.MainClass;
				version.JarId = jver.JarId;
                version.ClientJarUrl = jver.Downloads?.Client?.Url;
                version.ClientJarSHA1 = jver.Downloads?.Client?.SHA1;
                version.ServerJarUrl = jver.Downloads?.Server?.Url;
                version.ServerJarSHA1 = jver.Downloads?.Server?.SHA1;
				version.Libraries = new List<Library>();
				version.Natives = new List<Native>();
				foreach (var lib in jver.Libraries)
				{
					if (string.IsNullOrWhiteSpace(lib.Name))
					{
						continue;
					}
					var names = lib.Name.Split(':');
					if (names.Length != 3)
					{
						continue;
					}
					if (lib.Natives == null)
					{
						if (!IfAllowed(lib.Rules))
						{
							continue;
						}
                        Library tmp = new Library
                        {
                            NS = names[0],
                            Name = names[1],
                            Version = names[2]
                        };
                        if (lib.DownloadsInfo != null)
                        {
                            tmp.Url = lib.DownloadsInfo.Artifact?.Url;
                            tmp.SHA1 = lib.DownloadsInfo.Artifact?.SHA1;
                        }
                        else
                        {
                            tmp.Url = lib.Url;
                            tmp.checksums = lib.checksums;
                        }
                        version.Libraries.Add(tmp);
					}
					else
					{
						if (!IfAllowed(lib.Rules))
						{
							continue;
						}
                        var native = new Native
                        {
                            NS = names[0],
                            Name = names[1],
                            Version = names[2],
                            NativeSuffix = lib.Natives["windows"].Replace("${arch}", SystemTools.GetArch())
						};
                        if (lib.DownloadsInfo != null)
                        {
                            native.Url = lib.DownloadsInfo?.Classifiers?.Windows?.Url;
                            native.checksum = lib.DownloadsInfo?.Classifiers?.Windows?.SHA1;
                        }
                        else
                        {
                            native.Url = lib.Url;
                        }
						version.Natives.Add(native);
						if (lib.Extract != null)
						{
							native.Options = new UnzipOptions {Exclude = lib.Extract.Exculde};
						}
					}
				}
				if (jver.InheritsVersion != null)
				{
					var target = GetVersionInternal(jver.InheritsVersion);
					if (target == null)
					{
						return null;
					}
					else
					{
                        if (version.Assets == "legacy")
                            version.Assets = null;
                        version.Assets = version.Assets ?? target.Assets;
						version.JarId = version.JarId ?? target.JarId;
						version.MainClass = version.MainClass ?? target.MainClass;
						version.MinecraftArguments = version.MinecraftArguments ?? target.MinecraftArguments;
						version.Natives.AddRange(target.Natives);
						version.Libraries.AddRange(target.Libraries);
					}
				}

                //assets
                string json = File.ReadAllText(_core.GetAssetIndexPath(version));
                JObject rootObj = JObject.Parse(json)["objects"].ToObject<JObject>();
                foreach(JObject obj in rootObj.Values<JObject>())
                {
                    string hash = obj["hash"].ToString();
                    int size;
                    int.TryParse(obj["size"].ToString(), out size);

                    if (string.IsNullOrWhiteSpace(hash))
                    {
                        continue;
                    }

                    Asset asset = new Asset
                    {
                        Hash = hash,
                        Size = size
                    };
                    version.AssetsList.Add(asset);
                }

				version.JarId = version.JarId ?? version.Id;
				_versions.Add(version.Id, version);
				return version;
			}
			catch
			{
				return null;
			}
			finally
			{
				_locatingVersion.Remove(id);
			}
		}

		public JVersion LoadVersion(string jsonPath)
		{
			try
			{
				return JsonConvert.DeserializeObject<JVersion>(File.ReadAllText(jsonPath));
			}
			catch
			{
				return null;
			}
		}

		/// <summary>
		///     判断一系列规则后是否启用
		/// </summary>
		/// <param name="rules">规则们</param>
		/// <returns>是否启用</returns>
		public bool IfAllowed(List<JRule> rules)
		{
			if (rules == null)
			{
				return true;
			}
			if (rules.Count == 0)
			{
				return true;
			}
			var allowed = false;
			foreach (var rule in rules)
			{
				if (rule.OS == null)
				{
					allowed = rule.Action == "allow";
					continue;
				}
				if (rule.OS.Name == "windows")
				{
					allowed = rule.Action == "allow";
				}
			}
			return allowed;
		}
	}
}