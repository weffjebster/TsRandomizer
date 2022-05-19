﻿using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Timespinner.Core;
using Timespinner.Core.Specifications;
using Timespinner.GameAbstractions.Gameplay;
using Timespinner.GameAbstractions.Inventory;
using TsRandomizer.Extensions;
using TsRandomizer.IntermediateObjects;
using TsRandomizer.Settings;

namespace TsRandomizer.Randomisation
{
	struct BossAttributes
	{
		public int Index;
		public string VisibleName;
		public string SaveName;
		public RoomItemKey RoomKey;
		public Point Position;
		public int HP;
		public int XP;
		public int TouchDamage;
		public Timespinner.GameAbstractions.EBGM Song;
		public SpriteSheet Sprite;
		public Type BossType;
		public int TileId;

	}

	static class BestiaryManager
	{
		public static BossAttributes GetBossAttributes(Level level, int bossId)
		{
			switch (bossId)
			{
				case 65:
					return new BossAttributes
					{
						Index = bossId,
						VisibleName = "Feline Sentry",
						SaveName = "IsBossDead_RoboKitty",
						RoomKey = new RoomItemKey(1, 5),
						Position = new Point(200, 200),
						HP = 475,
						XP = 50,
						TouchDamage = 17,
						Song = Timespinner.GameAbstractions.EBGM.Boss01,
						Sprite = level.GCM.SpRoboKitty,
						BossType = TimeSpinnerType.Get("Timespinner.GameObjects.Bosses.RoboKitty.RoboKittyBoss"),
						TileId = (int)EEnemyTileType.RoboKittyBoss
					};
				case 66:
					return new BossAttributes
					{
						Index = bossId,
						VisibleName = "Varndagroth",
						SaveName = "IsBossDead_Varndagroth",
						RoomKey = new RoomItemKey(2, 29),
						Position = new Point(200, 200),
						HP = 800,
						XP = 100,
						TouchDamage = 25,
						Song = Timespinner.GameAbstractions.EBGM.Boss02,
						Sprite = level.GCM.SpVarndagroth,
						BossType = TimeSpinnerType.Get("Timespinner.GameObjects.Bosses.Varndagroth.VarndagrothBoss"),
						TileId = (int)EEnemyTileType.VarndagrothBoss
					};
				case 67:
					return new BossAttributes
					{
						Index = bossId,
						VisibleName = "Azure Queen",
						SaveName = "IsBossDead_Bird",
						RoomKey = new RoomItemKey(7, 0),
						Position = new Point(200, 200),
						HP = 1600,
						XP = 200,
						TouchDamage = 40,
						Song = Timespinner.GameAbstractions.EBGM.Boss07,
						Sprite = level.GCM.SpBirdBoss,
						BossType = TimeSpinnerType.Get("Timespinner.GameObjects.Bosses.Bird.GodBirdBoss"),
						TileId = (int)EEnemyTileType.BirdBoss
					};
				case 68:
					return new BossAttributes
					{
						Index = bossId,
						VisibleName = "Golden Idol",
						SaveName = "IsBossDead_Demon",
						RoomKey = new RoomItemKey(5, 5),
						Position = new Point(200, 200),
						HP = 2000,
						XP = 250,
						TouchDamage = 46,
						Song = Timespinner.GameAbstractions.EBGM.Boss05A,
						Sprite = level.GCM.SpDemonBoss,
						BossType = TimeSpinnerType.Get("Timespinner.GameObjects.Bosses.DemonBoss"),
						TileId = (int)EEnemyTileType.IncubusBoss
					};
				case 69:
					return new BossAttributes
					{
						Index = bossId,
						VisibleName = "Aelana",
						SaveName = "IsBossDead_Sorceress",
						RoomKey = new RoomItemKey(6, 15),
						Position = new Point(200, 200),
						HP = 2250,
						XP = 300,
						TouchDamage = 48,
						Song = Timespinner.GameAbstractions.EBGM.Boss06,
						Sprite = level.GCM.SpAelana,
						BossType = TimeSpinnerType.Get("Timespinner.GameObjects.Bosses.AelanaBoss"),
						TileId = (int)EEnemyTileType.AelanaBoss
					};
				case 70:
					return new BossAttributes
					{
						Index = bossId,
						VisibleName = "The Maw",
						SaveName = "IsBossDead_Maw",
						// Actually 8, 7, but 8, 13 is where Lunais is spit out
						RoomKey = new RoomItemKey(8, 13),
						Position = new Point(200, 200),
						HP = 2250,
						XP = 366,
						TouchDamage = 52,
						Song = Timespinner.GameAbstractions.EBGM.Boss08,
						Sprite = level.GCM.SpMawBoss,
						BossType = TimeSpinnerType.Get("Timespinner.GameObjects.Bosses.MawBoss"),
						TileId = (int)EEnemyTileType.MawBoss
					};
				case 71:
					return new BossAttributes
					{
						Index = bossId,
						VisibleName = "Cantoran",
						SaveName = "IsBossDead_Cantoran",
						RoomKey = new RoomItemKey(7, 5),
						Position = new Point(200, 200),
						HP = 2250,
						XP = 300,
						TouchDamage = 54,
						Song = Timespinner.GameAbstractions.EBGM.Boss07,
						Sprite = level.GCM.SpCantoranBoss,
						BossType = TimeSpinnerType.Get("Timespinner.GameObjects.Bosses.CantoranBoss"),
						TileId = (int)EEnemyTileType.CantoranBoss
					};
				case 72:
					return new BossAttributes
					{
						Index = bossId,
						VisibleName = "Genza",
						SaveName = "IsBossDead_Shapeshift",
						RoomKey = new RoomItemKey(11, 21),
						Position = new Point(200, 200),
						HP = 3000,
						XP = 500,
						TouchDamage = 60,
						Song = Timespinner.GameAbstractions.EBGM.Boss11,
						Sprite = level.GCM.SpShapeshifter,
						BossType = TimeSpinnerType.Get("Timespinner.GameObjects.Bosses.ShapeshifterBoss"),
						TileId = (int)EEnemyTileType.ShapeshiftBoss
					};
				case 73:
					return new BossAttributes
					{
						Index = bossId,
						VisibleName = "Emperor Nuvius",
						SaveName = "IsBossDead_Emperor",
						RoomKey = new RoomItemKey(12, 20),
						Position = new Point(200, 200),
						HP = 3500,
						XP = 666,
						TouchDamage = 80,
						Song = Timespinner.GameAbstractions.EBGM.Boss12,
						Sprite = level.GCM.SpEmperor,
						BossType = TimeSpinnerType.Get("Timespinner.GameObjects.Bosses.Emperor.EmperorBoss"),
						TileId = (int)EEnemyTileType.EmperorBoss
					};
				case 74:
					return new BossAttributes
					{
						Index = bossId,
						VisibleName = "Emperor Vol Terrilis",
						SaveName = "IsTerrilisDead",
						RoomKey = new RoomItemKey(13, 1),
						Position = new Point(200, 200),
						HP = 4000,
						XP = 777,
						TouchDamage = 85,
						Song = Timespinner.GameAbstractions.EBGM.Boss12,
						Sprite = level.GCM.SpEmperor,
						BossType = TimeSpinnerType.Get("Timespinner.GameObjects.Bosses.Emperor.EmperorBoss"),
						TileId = (int)EEnemyTileType.EmperorBoss
					};
				case 75:
					return new BossAttributes
					{
						Index = bossId,
						VisibleName = "Prince Nuvius",
						SaveName = "IsPrinceDead",
						RoomKey = new RoomItemKey(13, 0),
						Position = new Point(200, 200),
						HP = 2500,
						XP = 350,
						TouchDamage = 70,
						Song = Timespinner.GameAbstractions.EBGM.Boss12,
						Sprite = level.GCM.SpEmperor,
						BossType = TimeSpinnerType.Get("Timespinner.GameObjects.Bosses.Emperor.EmperorBoss"),
						TileId = (int)EEnemyTileType.EmperorBoss
					};
				case 76:
					return new BossAttributes
					{
						Index = bossId,
						VisibleName = "Xarion",
						SaveName = "IsBossDead_Xarion",
						RoomKey = new RoomItemKey(9, 7),
						Position = new Point(500, 200),
						HP = 3500,
						XP = 550,
						TouchDamage = 75,
						Song = Timespinner.GameAbstractions.EBGM.Boss13,
						Sprite = level.GCM.SpXarionBoss,
						BossType = TimeSpinnerType.Get("Timespinner.GameAbstractions.GameObjects.XarionBoss"),
						TileId = (int)EEnemyTileType.XarionBoss
					};
				case 77:
					return new BossAttributes
					{
						Index = bossId,
						VisibleName = "Ravenlord",
						SaveName = "IsBossDead_Raven",
						RoomKey = new RoomItemKey(14, 4),
						Position = new Point(200, 200),
						HP = 5000,
						XP = 680,
						TouchDamage = 95,
						Song = Timespinner.GameAbstractions.EBGM.Boss13,
						Sprite = level.GCM.SpRavenBoss,
						BossType = TimeSpinnerType.Get("Timespinner.GameObjects.Bosses.Z_Raven.RavenBoss"),
						TileId = (int)EEnemyTileType.RavenBoss
					};
				case 78:
					return new BossAttributes
					{
						Index = bossId,
						VisibleName = "Ifrit",
						SaveName = "IsBossDead_Zel",
						RoomKey = new RoomItemKey(14, 5),
						Position = new Point(200, 200),
						HP = 5000,
						XP = 700,
						TouchDamage = 95,
						Song = Timespinner.GameAbstractions.EBGM.Boss12,
						Sprite = level.GCM.SpZelBoss,
						BossType = TimeSpinnerType.Get("Timespinner.GameAbstractions.GameObjects.ZelBoss"),
						TileId = (int)EEnemyTileType.ZelBoss
					};
				case 79: 
					return new BossAttributes
					{
						Index = bossId,
						VisibleName = "Sandman",
						SaveName = "IsBossDead_Sandman",
						// Actual room is 16, 4, but trigger final Nightmare instead as return
						// RoomKey = new RoomItemKey(16, 4),
						RoomKey = new RoomItemKey(16, 26),
						Position = new Point(200, 200),
						HP = 5000,
						XP = 800,
						TouchDamage = 90,
						Song = Timespinner.GameAbstractions.EBGM.Boss15,
						Sprite = level.GCM.SpSandmanBoss,
						BossType = TimeSpinnerType.Get("Timespinner.GameAbstractions.GameObjects.SandmanBoss"),
						TileId = (int)EEnemyTileType.SandmanBoss
					};
				case 80:
					return new BossAttributes
					{
						Index = bossId,
						VisibleName = "Nightmare",
						SaveName = "IsBossDead_Nightmare",
						// Actual room is 16, 26, but trigger ! instead
						// RoomKey = new RoomItemKey(16, 26),
						RoomKey = new RoomItemKey(16, 27),
						Position = new Point(500, 200),
						HP = 6666,
						XP = 0,
						TouchDamage = 111,
						Song = Timespinner.GameAbstractions.EBGM.Boss16,
						Sprite = level.GCM.SpNightmareBoss,
						BossType = TimeSpinnerType.Get("Timespinner.GameObjects.Bosses.OtherBosses.NightmareBoss"),
						TileId = (int)EEnemyTileType.NightmareBoss
					};
				default: 
					return new BossAttributes
					{ 
						Index = 10,
						VisibleName = "Baby Cheveur",
						SaveName = "KILL_LakeCheveux",
						RoomKey = new RoomItemKey(7, 5),
						Position = new Point(200, 200),
						HP = 50,
						XP = 9,
						TouchDamage = 25,
						Song = Timespinner.GameAbstractions.EBGM.Boss01,
						Sprite = level.GCM.SpLakeBirdEgg,
						BossType = TimeSpinnerType.Get("Timespinner.GameObjects.LakeBirdEgg"),
						TileId = (int)EEnemyTileType.LakeBirdEgg
					};
			}
		}

		public static BossAttributes GetReplacedBoss(Level level, SeedOptions seedOptions, int vanillaBossId)
		{
			// Add 72 when ShapeshifterBoss merged into BossEnemy
			int[] validBosses = new int[] { 65, 66, 67, 68, 69, 70, 71, 73, 74, 75, 76, 77, 78, 79, 80};

			Random random = new Random(seedOptions.GetHashCode());
			int[] replacedBosses = validBosses.OrderBy(x => random.Next()).ToArray();

			int bossIndex = Array.IndexOf(validBosses, vanillaBossId, 0);

			int replacedBossId = replacedBosses[bossIndex];

			BossAttributes replacedBossInfo = GetBossAttributes(level, replacedBossId);

			return replacedBossInfo;
		}

		public static void SetBossKillSave(Level level, int vanillaBossId)
		{
			BossAttributes vanillaBossInfo = GetBossAttributes(level, vanillaBossId);
			level.GameSave.SetValue($"TSRando_{vanillaBossInfo.SaveName}", true);
			RefreshBossSaveFlags(level);
		}


		public static void RefreshBossSaveFlags(Level level)
		{
			// Iterate through all bosses and set their kill flag to reflect boss location, not actual boss
			// Add 72 when ShapeshifterBoss merged into BossEnemy
			int[] validBosses = new int[] { 65, 66, 67, 68, 69, 70, 71, 73, 74, 75, 76, 77, 78, 79, 80 };
			foreach (int bossIndex in validBosses)
			{
				BossAttributes bossInfo = GetBossAttributes(level, bossIndex);
				bool isBossDead = level.GameSave.GetSaveBool($"TSRando_{bossInfo.SaveName}");
				level.GameSave.SetValue(bossInfo.SaveName, isBossDead);
			}
		}

		public static BossAttributes GetVanillaBoss(Level level, SeedOptions seedOptions, int replacedBossId)
		{
			// Add 72 when ShapeshifterBoss merged into BossEnemy
			int[] validBosses = new int[] { 65, 66, 67, 68, 69, 70, 71, 73, 74, 75, 76, 77, 78, 79, 80 };

			Random random = new Random(seedOptions.GetHashCode());
			int[] replacedBosses = validBosses.OrderBy(x => random.Next()).ToArray();

			int bossIndex = Array.IndexOf(replacedBosses, replacedBossId, 0);

			int vanillaBossId = validBosses[bossIndex];

			BossAttributes vanillaBossInfo = GetBossAttributes(level, vanillaBossId);

			return vanillaBossInfo;
		}

		public static void UpdateBestiary(Level level, SeedOptions seedOptions, SettingCollection gameSettings)
		{
			TimeSpinnerGame.Localizer.OverrideKey("inv_use_PlaceHolderItem1", "Nothing");
			TimeSpinnerGame.Localizer.OverrideKey("inv_use_PlaceHolderItem1_desc", "You thought you picked something up, but it turned out to be nothing.");

			var bestiary = level.GCM.Bestiary;
			Random random = new Random(seedOptions.GetHashCode());
			foreach (var bestiaryEntry in bestiary.BestiaryEntries)
			{
				if (gameSettings.ShowBestiary.Value)
				{
					level.GameSave.SetValue(string.Format(bestiaryEntry.Key.Replace("Enemy_", "KILL_")), 1);
				}
				// TODO: Add 72 when fixing ShapeshifterBoss
				if (bestiaryEntry.Index >= 65 && bestiaryEntry.Index <= 80 && bestiaryEntry.Index != 72)
				{
					BossAttributes replacedBossInfo = GetBossAttributes(level, bestiaryEntry.Index);
					BossAttributes vanillaBossInfo = GetVanillaBoss(level, seedOptions, bestiaryEntry.Index);
					bestiaryEntry.VisibleName = $"{replacedBossInfo.VisibleName} as {vanillaBossInfo.VisibleName}";
					bestiaryEntry.HP = vanillaBossInfo.HP;
					bestiaryEntry.TouchDamage = vanillaBossInfo.TouchDamage;
					bestiaryEntry.Exp = vanillaBossInfo.XP;
				}

				int dropSlot = 0;
				foreach (var loot in bestiaryEntry.LootTable)
				{
					if (gameSettings.ShowDrops.Value)
					{
						level.GameSave.SetValue(string.Format(bestiaryEntry.Key.Replace("Enemy_", "DROP_" + dropSlot + "_")), true);
					}
					if (gameSettings.LootPool.Value == "Random")
					{
						var item = Helper.GetAllLoot().SelectRandom(random);
						loot.Item = item.ItemId;
						if (item.LootType == LootType.Equipment)
							loot.Category = (int)EInventoryCategoryType.Equipment;
						else
							loot.Category = (int)EInventoryCategoryType.UseItem;
					}
					else if (gameSettings.LootPool.Value == "Empty")
					{
						loot.DropRate = 0;
						loot.Item = (int)EInventoryUseItemType.PlaceHolderItem1;
						loot.Category = (int)EInventoryCategoryType.UseItem;
					}
					dropSlot++;
				}
			}
		}
	}
}
