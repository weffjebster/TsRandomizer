﻿using Timespinner.GameObjects.BaseClasses;
using TsRandomizer.IntermediateObjects;
using TsRandomizer.Screens;
using TsRandomizer.Settings;

namespace TsRandomizer.LevelObjects.Other
{
	[TimeSpinnerType("Timespinner.GameObjects.Events.EnvironmentPrefabs.EnvPrefabCurtainWinch")]
	// ReSharper disable once UnusedMember.Global
	class EnvPrefabCurtainWinch : LevelObject
	{
		public EnvPrefabCurtainWinch(Mobile typedObject, GameplayScreen gameplayScreen) : base(typedObject, gameplayScreen)
		{
		}

		protected override void Initialize(Seed seed, SettingCollection settings)
		{
			Dynamic._isDrawbridgeUp = !LevelReflected.GetLevelSaveBool("HasWinchBeenUsed") 
				? false 
				: LevelReflected.GetLevelSaveBool("IsDrawbridgeRaised");
			Dynamic._isRotatingClockwise = Dynamic._isDrawbridgeUp;
		}
	}
}
