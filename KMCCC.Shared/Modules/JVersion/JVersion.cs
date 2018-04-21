namespace KMCCC.Modules.JVersion
{
    using Newtonsoft.Json;
    #region

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

		[JsonProperty("inheritsFrom")]
		public string InheritsVersion { get; set; }

		[JsonProperty("jar")]
		public string JarId { get; set; }

        [JsonProperty("downloads")]
        public JVersionDownloadsContent Downloads { get; set; }

        [JsonProperty("assetIndex")]
        public JAssetsIndexDownloadInfo AssetIndex;
    }

    public class JVersionDownloadsContent
    {
        [JsonProperty("client")]
        public JDownloadInfo Client;

        [JsonProperty("server")]
        public JDownloadInfo Server;
    }

    public class JAssetsIndexDownloadInfo : JDownloadInfo
    {
        [JsonProperty("id")]
        public string Id;

        [JsonProperty("totalSize")]
        public int TotalSize;
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
        public JLibraryDownloadsContent DownloadsInfo { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        public string[] checksums { get; set; }
    }

    public class JLibraryDownloadsContent
    {
        [JsonProperty("artifact")]
        public JDownloadInfo Artifact;

        [JsonProperty("classifiers")]
        public JClassifiers Classifiers;
    }

    public class JClassifiers
    {
        [JsonProperty("natives-linux")]
        public JDownloadInfo Linux;

        [JsonProperty("natives-osx")]
        public JDownloadInfo OSX;

        [JsonProperty("natives-windows")]
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