﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO.Compression;
using System.IO;
using System.IO.Packaging;
using System.Reflection;
using System.Collections;
using System.Threading.Tasks;

namespace KMCCC.Tools
{
	/// <summary>
	/// 操蛋的通过反射调用Zip解压
	/// Notice: 文件名只支持ASCII
	/// </summary>
	public static class ZipTools
	{
		static ZipTools()
		{
			try
			{
				var windowsBase = typeof(System.IO.Packaging.Package).Assembly;
				ZipArchive = windowsBase.GetType("MS.Internal.IO.Zip.ZipArchive");
				ZipArchive_OpenOnFile = ZipArchive.GetMethod("OpenOnFile", BindingFlags.NonPublic | BindingFlags.Static);
				ZipArchive_GetFiles = ZipArchive.GetMethod("GetFiles", BindingFlags.NonPublic | BindingFlags.Instance);
				ZipArchive_Close = ZipArchive.GetMethod("Close", BindingFlags.NonPublic | BindingFlags.Instance);

				ZipFileInfo = windowsBase.GetType("MS.Internal.IO.Zip.ZipFileInfo");
				ZipFileInfo_GetStream = ZipFileInfo.GetMethod("GetStream", BindingFlags.NonPublic | BindingFlags.Instance);
				ZipFileInfo_Name = ZipFileInfo.GetProperty("Name", BindingFlags.NonPublic | BindingFlags.Instance);
				ZipFileInfo_FolderFlag = ZipFileInfo.GetProperty("FolderFlag", BindingFlags.NonPublic | BindingFlags.Instance);
				Enabled = true;
			}
			catch { Enabled = false; }
		}

		public static readonly Boolean Enabled;

		public static readonly Type ZipArchive;

		public static readonly MethodInfo ZipArchive_OpenOnFile;

		public static readonly MethodInfo ZipArchive_GetFiles;

		public static readonly MethodInfo ZipArchive_Close;

		public static readonly Type ZipFileInfo;

		public static readonly MethodInfo ZipFileInfo_GetStream;

		public static readonly PropertyInfo ZipFileInfo_Name;

		public static readonly PropertyInfo ZipFileInfo_FolderFlag;

		public static bool Unzip(string zipFile, string outputDirectory, UnzipOptions options)
		{
			if (options == null) { return false; }
			try
			{
				var root = new DirectoryInfo(outputDirectory);
				root.Create();
				var rootPath = root.FullName + "/";
				var zip = ZipArchive_OpenOnFile.Invoke(null, new object[] { zipFile, FileMode.Open, FileAccess.Read, FileShare.Read, false });
				IEnumerable files = (IEnumerable)ZipArchive_GetFiles.Invoke(zip, new object[] { });
				IEnumerable<string> exclude = (options.Exclude == null ? new List<string>() : options.Exclude);
				if (exclude.Count() > 1000) { exclude = exclude.AsParallel(); }
				foreach (var item in files)
				{
					string name = (string)ZipFileInfo_Name.GetValue(item, null);
					if (exclude.Any(ex => name.StartsWith(ex)))
					{
						continue;
					}
					if ((bool)ZipFileInfo_FolderFlag.GetValue(item, null))
					{
						Directory.CreateDirectory(rootPath + name);
						continue;
					}
					using (Stream stream = (Stream)ZipFileInfo_GetStream.Invoke(item, new object[] { FileMode.Open, FileAccess.Read }))
					{
						string filePath = rootPath + name;
						new FileInfo(filePath).Directory.Create();
						using (var fs = new FileStream(filePath, FileMode.Create))
						{
							stream.CopyTo(fs);
						}
					}
				}
				ZipArchive_Close.Invoke(zip, new object[] { });
				return true;
			}
			catch
			{
				return false;
			}
		}

		private static string CreateFilenameFromUri(Uri uri)
		{
			char[] invalidChars = Path.GetInvalidFileNameChars();
			StringBuilder sb = new StringBuilder(uri.OriginalString.Length);
			foreach (char c in uri.OriginalString)
			{
				sb.Append(Array.IndexOf(invalidChars, c) < 0 ? c : '_');
			}
			return sb.ToString();

		}

	}

	public class UnzipOptions
	{
		public List<string> Exclude { get; set; }
	}

}