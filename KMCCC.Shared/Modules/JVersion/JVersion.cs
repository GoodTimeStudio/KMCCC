namespace KMCCC.Modules.JVersion
{
    #region
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Newtonsoft.Json.Serialization;
    using System;
	using System.Collections.Generic;

    #endregion

    /// <summary>
    ///     用来Json的实体类
    /// </summary>
    public class JVersion
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("time")]
        public DateTime Time { get; set; }

        [JsonProperty("releaseTime")]
        public DateTime ReleaseTime { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("downloads")]
        public JDownloads Downloads { get; set; }

        [JsonProperty("minecraftArguments")]
        public string MinecraftArguments { get; set; }

        [JsonProperty("arguments")]
        public JArguments arguments { get; set; }

        [JsonProperty("minimumLauncherVersion")]
        public int MinimumLauncherVersion { get; set; }

        [JsonProperty("libraries")]
        public List<JLibrary> Libraries { get; set; }

        [JsonProperty("mainClass")]
        public string MainClass { get; set; }

        [JsonProperty("assets")]
        public string Assets { get; set; }

        [JsonProperty("assetIndex")]
        public JFileInfo AssetsIndex { get; set; }

        [JsonProperty("inheritsFrom")]
		public string InheritsVersion { get; set; }

        [JsonProperty("jar")]
        public string JarId { get; set; }

    }

    public class JFileInfo
    {
        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("sha1")]
        public string SHA1 { get; set; }

        [JsonProperty("size")]
        public int Size { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("totalSize")]
        public int TotalSize { get; set; }
    }

    public class JDownloads
    {
        [JsonProperty("client")]
        public JFileInfo Client { get; set; }

        [JsonProperty("server")]
        public JFileInfo Server { get; set; }
    }

    public class JLibrary
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("natives")]
        public Dictionary<string, string> Natives { get; set; }

        [JsonProperty("rules")]
        public List<JRule> Rules { get; set; }

        [JsonProperty("extract")]
        public JExtract Extract { get; set; }

        [JsonProperty("downloads")]
        public JLibraryDownloadsInfo DownloadsInfo { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        public string[] checksums { get; set; }
    }

    public class JLibraryDownloadsInfo
    {
        [JsonProperty("artifact")]
        public JDownloadInfo Artifact;

        [JsonProperty("classifiers")]
        public JObject ClassifiersInternal;

        [JsonIgnore]
        public JClassifiers Classifiers;
    }

    public class JClassifiers
    {
        public JDownloadInfo Linux;

        public JDownloadInfo OSX;

        public JDownloadInfo Windows;
    }

    public class JDownloadInfo
    {
        [JsonProperty("size")]
        public int Size { get; set; }

        [JsonProperty("sha1")]
        public string SHA1 { get; set; }

        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }

    public class JArguments
    {
        [JsonProperty("game")]
        public object[] game { get; set; }

        [JsonProperty("jvm")]
        public object[] jvm { get; set; }
    }

    public class JRule
	{
		[JsonProperty("action")]
		public string Action { get; set; }

		[JsonProperty("os")]
		public JOperatingSystem OS { get; set; }
	}

	public class JOperatingSystem
	{
		[JsonProperty("name")]
		public string Name { get; set; }
	}

	public class JExtract
	{
		[JsonProperty("exclude")]
		public List<string> Exculde { get; set; }
	}
}