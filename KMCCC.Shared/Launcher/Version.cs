﻿namespace KMCCC.Launcher
{
	#region

	using System;
	using System.Collections.Generic;
	using Tools;

	#endregion

	/// <summary>
	///     版本定位器接口
	/// </summary>
	public interface IVersionLocator
	{
		/// <summary>
		///     设置定位器基于的核心
		/// </summary>
		LauncherCore Core { set; }

		/// <summary>
		///     获取对应Id的Version，若不存在应返回null
		/// </summary>
		/// <param name="versionId">对应的Id</param>
		/// <returns>对应的Version</returns>
		Version Locate(string versionId);

		/// <summary>
		///     获取所有可找到Version
		/// </summary>
		/// <returns>所有Version</returns>
		IEnumerable<Version> GetAllVersions();
	}

	/// <summary>
	///     表示版本
	/// </summary>
	public sealed class Version
	{
		/// <summary>
		///     ID
		/// </summary>
		public string Id { get; set; }

        /// <summary>
        /// 版本类型
        /// </summary>
        public string Type { get; set; }

		/// <summary>
		///     主启动参数
		/// </summary>
		public string MinecraftArguments { get; set; }

        /// <summary>
        ///     主启动参数（新版本）
        /// </summary>
        public arguments NewMinecraftArguments { get; set; }

        /// <summary>
        ///     资源名
        /// </summary>
        public string Assets { get; set; }

        public AssetIndex AssetIndexInfo { get; set; }

		/// <summary>
		///     主类
		/// </summary>
		public string MainClass { get; set; }

		/// <summary>
		///     库列表
		/// </summary>
		public List<Library> Libraries { get; set; }

		/// <summary>
		///     本地实现表
		/// </summary>
		public List<Native> Natives { get; set; }

		/// <summary>
		///     Jar文件（Id）
		/// </summary>
		public string JarId { get; set; }

        /// <summary>
        /// 客户端Jar下载Url
        /// </summary>
        public string ClientJarUrl { get; set; }

        public string ClientJarSHA1 { get; set; }

        /// <summary>
        /// 服务端Jar下载Url
        /// </summary>
        public string ServerJarUrl { get; set; }

        public string ServerJarSHA1 { get; set; }
	}

	/// <summary>
	///     表示库
	/// </summary>
	public class Library
	{
		/// <summary>
		///     NS
		/// </summary>
		public string NS { get; set; }

		/// <summary>
		///     Name
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		///     Version
		/// </summary>
		public string Version { get; set; }

        /// <summary>
        ///     Url
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        ///     checksums
        /// </summary>
        public string[] checksums { get; set; }

        public string SHA1 { get; set; }

        /// <summary>
        ///     serverreq
        /// </summary>
        public bool serverreq { get; set; } = true;

        public bool clientreq { get; set; } = true;
    }

    /// <summary>
    ///     表示启动参数（新格式）
    /// </summary>
    public class arguments
    {
        public List<_game> game { get; set; }
        public _jvm jvm { get; set; }
    }

    /// <summary>
    ///     表示启动参数游戏部分
    /// </summary>
    public class _game
    {

    }

    /// <summary>
    ///     表示启动参数jvm虚拟机部分
    /// </summary>
    public class _jvm
    {

    }

    /// <summary>
    ///     表示本机实现
    /// </summary>
    public class Native
	{
		/// <summary>
		///     NS
		/// </summary>
		public string NS { get; set; }

		/// <summary>
		///     Name
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		///     Version
		/// </summary>
		public string Version { get; set; }

		/// <summary>
		///     本机实现后缀
		/// </summary>
		public string NativeSuffix { get; set; }

        /// <summary>
        /// Jar下载url
        /// </summary>
        public string Url { get; set; }

        public string checksum { get; set; }

		/// <summary>
		///     解压参数
		/// </summary>
		public UnzipOptions Options { get; set; }
	}

    public class AssetIndex
    {
        /// <summary>
        /// 资源名
        /// </summary>
        public string Id { get; set; }

        public string SHA1 { get; set; }

        /// <summary>
        /// 下载Url
        /// </summary>
        public string Url { get; set; }
    }

	/// <summary>
	///     找Item，自己看我不加注释了
	/// </summary>
	public static class LauncherCoreItemResolverExtensions
	{

		public static string GetVersionRootPath(this LauncherCore core, Version version)
		{
			return GetVersionRootPath(core, version.Id);
		}

		public static string GetVersionRootPath(this LauncherCore core, string versionId)
		{
			return String.Format(@"{0}\versions\{1}\", core.GameRootPath, versionId);
		}

		public static string GetVersionJarPath(this LauncherCore core, Version version)
		{
			return GetVersionJarPath(core, version.Id);
		}

		public static string GetVersionJarPath(this LauncherCore core, string versionId)
		{
			return String.Format(@"{0}\versions\{1}\{1}.jar", core.GameRootPath, versionId);
		}

		public static string GetVersionJsonPath(this LauncherCore core, Version version)
		{
			return GetVersionJsonPath(core, version.Id);
		}

		public static string GetVersionJsonPath(this LauncherCore core, string versionId)
		{
			return String.Format(@"{0}\versions\{1}\{1}.json", core.GameRootPath, versionId);
		}

        public static string GetVersionOptions(this LauncherCore core, Version version)
        {
            return GetVersionOptions(core, version.Id);
        }

        public static string GetVersionOptions(this LauncherCore core, string versionId)
        {
            return String.Format(@"{0}\versions\{1}\options.txt", core.GameRootPath, versionId);
        }

        public static string GetLibPath(this LauncherCore core, Library lib)
		{
			return String.Format(@"{0}\libraries\{1}\{2}\{3}\{2}-{3}.jar", core.GameRootPath, lib.NS.Replace(".", "\\"), lib.Name, lib.Version);
		}

		public static string GetNativePath(this LauncherCore core, Native native)
		{
			return String.Format(@"{0}\libraries\{1}\{2}\{3}\{2}-{3}-{4}.jar", core.GameRootPath, native.NS.Replace(".", "\\"), native.Name, native.Version,
				native.NativeSuffix);
		}
	}
}