﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using KMCCC.Tools;
using LitJson;

namespace KMCCC.Launcher
{
	/// <summary>
	/// 表示版本
	/// </summary>
	public class Version
	{
		/// <summary>
		/// ID
		/// </summary>
		public string Id { get; set; }

		/// <summary>
		/// 主启动参数
		/// </summary>
		public string MinecraftArguments { get; set; }

		/// <summary>
		/// 资源名
		/// </summary>
		public string Assets { get; set; }

		/// <summary>
		/// 主类
		/// </summary>
		public string MainClass { get; set; }

		/// <summary>
		/// 库列表
		/// </summary>
		public List<Library> Libraries { get; set; }

		/// <summary>
		/// 本地实现表
		/// </summary>
		public List<Native> Natives { get; set; }
	}

	/// <summary>
	/// 表示库
	/// </summary>
	public class Library
	{
		/// <summary>
		/// NS
		/// </summary>
		public string NS { get; set; }

		/// <summary>
		/// Name
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Version
		/// </summary>
		public string Version { get; set; }
	}

	/// <summary>
	/// 表示本机实现
	/// </summary>
	public class Native
	{
		/// <summary>
		/// NS
		/// </summary>
		public string NS { get; set; }

		/// <summary>
		/// Name
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Version
		/// </summary>
		public string Version { get; set; }

		/// <summary>
		/// 本机实现后缀
		/// </summary>
		public string NativeSuffix { get; set; }

		/// <summary>
		/// 解压参数
		/// </summary>
		public UnzipOptions Options { get; set; }
	}

	/// <summary>
	/// 找Item，自己看我不加注释了
	/// </summary>
	public static class LauncherCoreItemResolverExtensions
	{
		public static string GetVersionRootPath(this LauncherCore core, Version version)
		{
			return String.Format(@"{0}\versions\{1}\", core.GameRootPath, version.Id);
		}

		public static string GetVersionJarPath(this LauncherCore core, Version version)
		{
			return String.Format(@"{0}\versions\{1}\{1}.jar", core.GameRootPath, version.Id);
		}

		public static string GetVersionJsonPath(this LauncherCore core, Version version)
		{
			return String.Format(@"{0}\versions\{1}\{1}.json", core.GameRootPath, version.Id);
		}

		public static string GetLibPath(this LauncherCore core, Library lib)
		{
			return String.Format(@"{0}\libraries\{1}\{2}\{3}\{2}-{3}.jar", core.GameRootPath, lib.NS.Replace(".", "\\"), lib.Name, lib.Version);
		}

		public static string GetNativePath(this LauncherCore core, Native native)
		{
			return String.Format(@"{0}\libraries\{1}\{2}\{3}\{2}-{3}-{4}.jar", core.GameRootPath, native.NS.Replace(".", "\\"), native.Name, native.Version, native.NativeSuffix);
		}
	}
}
