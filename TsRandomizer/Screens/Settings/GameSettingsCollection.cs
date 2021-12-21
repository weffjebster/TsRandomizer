﻿using System;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using TsRandomizer.Extensions;
using TsRandomizer.Screens.Settings.GameSettingObjects;

namespace TsRandomizer.Screens.Settings
{
	public class GameSettingsCollection
	{
		[JsonIgnore()]
		public string Path { get; private set; }
		public OnOffGameSetting DamageRando { get; set; }
		public SpecificValuesGameSetting ShopFill { get; set; }
		public NumberGameSetting ShopMultiplier { get; set; }
		public OnOffGameSetting ShopWarpShards { get; set; }

		public GameSettingsCollection()
		{
			string dir = "settings/";
			if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
			var settingsFiles = Directory.GetFiles(dir).ToList().Where(f => f.EndsWith(".json"));
			if (settingsFiles.Count() > 0)
			{
				var defaultFile = settingsFiles.First();
				Path = defaultFile;
			}
			else
			{
				Path = dir + "settings.json";
				DamageRando = new OnOffGameSetting("Damage Randomizer", "Adds a high chance to make orb damage very low, and a low chance to make orb damage very, very high", false, false);
				string[] shopSettings = { "Default", "Random", "Vanilla", "Empty" };
				ShopFill = new SpecificValuesGameSetting("Shop Inventory", "Sets the items for sale in Merchant Crow's shops. Options: [Default,Random,Vanilla,Empty]", "Default", shopSettings, false);
				ShopMultiplier = new NumberGameSetting("Shop Price Multiplier", "Multiplier for the cost of items in the shop. Set to 0 for free shops", 1, 0, 10, 1, true, true);
				ShopWarpShards = new OnOffGameSetting("Always Sell Warp Shards", "Shops always sell warp shards (when keys possessed), ignoring fill setting.", true, true);
				WriteSettings(); //write settings file with default values
			}
		}

		public void LoadSettingsFromFile()
		{
			try
			{
				if (File.Exists(Path))
				{
					string settingsString = File.ReadAllText(Path);
					var settings = JsonConvert.DeserializeObject<GameSettingsCollection>(settingsString);
					DamageRando = settings.DamageRando;
					ShopFill = settings.ShopFill;
					ShopMultiplier = settings.ShopMultiplier;
					ShopWarpShards = settings.ShopWarpShards;
				}
				else
					Console.WriteLine("Settings file not found: " + Path);
			}
			catch
			{
				Console.WriteLine("Error loading settings from " + Path);
			}
		}

		public bool WriteSettings()
		{
			try
			{
				string jsonSettings = JsonConvert.SerializeObject(this, Formatting.Indented);
				File.WriteAllText(Path, jsonSettings);
				Console.WriteLine($"Writing settings for: {this}");
				Console.WriteLine($"Writing settings as: {jsonSettings}");
				return true;
			}
			catch (Exception exc)
			{
				Console.WriteLine($"Error writing settings: {exc}");
				return false;
			}
		}
	}
}
