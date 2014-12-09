﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LitJson;

namespace KMCCC.Version
{
	/// <summary>
	/// 用来Json的实体类
	/// </summary>
	public class JVersion
	{
		[JsonPropertyName("id")]
		public string Id { get; set; }

		[JsonPropertyName("time")]
		public DateTime Time { get; set; }

		[JsonPropertyName("releaseTime")]
		public DateTime ReleaseTime { get; set; }

		[JsonPropertyName("type")]
		public string Type { get; set; }

		[JsonPropertyName("minecraftArguments")]
		public string MinecraftArguments { get; set; }

		[JsonPropertyName("minimumLauncherVersion")]
		public int MinimumLauncherVersion { get; set; }

		[JsonPropertyName("libraries")]
		public List<JLibrary> Libraries { get; set; }

		[JsonPropertyName("mainClass")]
		public string MainClass { get; set; }

		[JsonPropertyName("assets")]
		public string Assets { get; set; }
	}

	public class JLibrary
	{
		[JsonPropertyName("name")]
		public string Name { get; set; }

		[JsonPropertyName("natives")]
		public Dictionary<string, string> Natives { get; set; }

		[JsonPropertyName("rules")]
		public List<JRule> Rules { get; set; }

		[JsonPropertyName("extract")]
		public JExtract Extract { get; set; }
	}

	public class JRule
	{
		[JsonPropertyName("action")]
		public string Action { get; set; }

		[JsonPropertyName("os")]
		public JOS OS { get; set; }
	}

	public class JOS
	{
		[JsonPropertyName("name")]
		public string Name { get; set; }
	}

	public class JExtract
	{
		[JsonPropertyName("exclude")]
		public List<string> Exculde { get; set; }
	}
}
