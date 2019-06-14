namespace KMCCC.Tools
{
    #region

    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.IO.Compression;
    using System.Linq;
    using System.Text;

    #endregion

    public static class ZipTools
	{

		public static bool Unzip(string zipFile, string outputDirectory, UnzipOptions options)
		{
			return UnzipFile(zipFile, outputDirectory, options) == null;
		}

		public static Exception UnzipFile(string zipFile, string outputDirectory, UnzipOptions options)
		{
			options = options ?? new UnzipOptions();
			try
			{
				var root = new DirectoryInfo(outputDirectory);
				root.Create();
				var rootPath = root.FullName + "/";
                
				using (var zip = ZipFile.Open(zipFile, ZipArchiveMode.Read, options.Encoding ?? Encoding.Default))
				{
                    var files = zip.Entries;
					IEnumerable<string> exclude = (options.Exclude ?? new List<string>());
					if (exclude.Count() > 1000)
					{
						exclude = exclude.AsParallel();
					}

					foreach (var item in files)
					{
                        var name = item.FullName;
						if (exclude.Any(name.StartsWith))
						{
							continue;
						}

                        // Determine whether the entry is a directory
                        if (name.Last() == '/')
                        {
                            Directory.CreateDirectory(rootPath + name);
                            continue;
                        }

                        var filePath = rootPath + name;
                        item.ExtractToFile(filePath, true);
					}
				}
				return null;
			}
			catch (Exception exp)
			{
				return exp;
			}
		}

		#region 编码大法

		public class WarpedEncoding : ASCIIEncoding
		{
			private readonly Encoding _innerEncoding;

			public WarpedEncoding(Encoding encoding)
			{
				_innerEncoding = encoding ?? Default;
			}

			public override bool Equals(object value)
			{
				return _innerEncoding.Equals(value);
			}

			public override int GetByteCount(char[] chars, int index, int count)
			{
				return _innerEncoding.GetByteCount(chars, index, count);
			}

			public override int GetByteCount(string chars)
			{
				return _innerEncoding.GetByteCount(chars);
			}

			public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
			{
				return _innerEncoding.GetBytes(chars, charIndex, charCount, bytes, byteIndex);
			}

			public override int GetBytes(string s, int charIndex, int charCount, byte[] bytes, int byteIndex)
			{
				return _innerEncoding.GetBytes(s, charIndex, charCount, bytes, byteIndex);
			}

			public override int GetCharCount(byte[] bytes, int index, int count)
			{
				return _innerEncoding.GetCharCount(bytes, index, count);
			}

			public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
			{
				return _innerEncoding.GetChars(bytes, byteIndex, byteCount, chars, charIndex);
			}

			public override Decoder GetDecoder()
			{
				return _innerEncoding.GetDecoder();
			}

			public override Encoder GetEncoder()
			{
				return _innerEncoding.GetEncoder();
			}

			public override int GetHashCode()
			{
				return _innerEncoding.GetHashCode();
			}

			public override int GetMaxByteCount(int charCount)
			{
				return _innerEncoding.GetMaxByteCount(charCount);
			}

			public override int GetMaxCharCount(int byteCount)
			{
				return _innerEncoding.GetMaxCharCount(byteCount);
			}

			public override byte[] GetPreamble()
			{
				return _innerEncoding.GetPreamble();
			}

			public override string GetString(byte[] bytes, int index, int count)
			{
				return _innerEncoding.GetString(bytes, index, count);
			}
		}

		#endregion
	}


	/// <summary>
	///     解压选项
	/// </summary>
	public class UnzipOptions
	{
		/// <summary>
		///     排除的文件（夹）
		/// </summary>
		public List<string> Exclude { get; set; }

		/// <summary>
		///     文件（夹）名使用的编码
		/// </summary>
		public Encoding Encoding { get; set; }
	}
}