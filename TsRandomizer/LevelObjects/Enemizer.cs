﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Timespinner.Core.Specifications;
using Timespinner.GameAbstractions.Gameplay;
using Timespinner.GameObjects.BaseClasses;
using TsRandomizer.Extensions;
using TsRandomizer.IntermediateObjects;
using TsRandomizer.Randomisation;
using E = TsRandomizer.LevelObjects.EnemyType;

namespace TsRandomizer.LevelObjects
{
	class Enemizer
	{
		static readonly Type BossType = TimeSpinnerType.Get("Timespinner.GameObjects.BaseClasses.BossClass");

		static readonly E[] LargeGroundedEnemies = {
			E.JumpingCheveuxTank,
			E.SealDog,
			E.WormFlower,
			E.WormFlowerWalker,
			E.RoombaCat,
			E.ForestBabyCheveux,
			E.ForestWormFlower,
			E.PastMushroomTower,
			E.PresentMushroomTower,
			E.PastLargeSoldier,
			E.PresentLargeSoldier,
			E.ChargingCheveux,
			E.IceMage,
			E.LargeCheveux
		};

		static readonly E[] NormalGroundedEnemies = {
			E.CheveuxTank,
			E.PastBomber,
			E.PresentBomber,
			E.PastSiren,
			E.PresentSiren,
			E.PastShieldKnight,
			E.PresentShieldKnight,
			E.PastArcher,
			E.PresentArcher,
			E.Granadier,
			E.Rat,
			E.FireMage,
			E.PastSlime,
			E.PresentSlime
		};

		static readonly E[] SmallGroundedEnemies = {
			E.Spider,
			E.Egg
		};

		static readonly E[] GroundedEnemies = 
			LargeGroundedEnemies
			.Concat(NormalGroundedEnemies)
			.Concat(SmallGroundedEnemies)
			.ToArray();

		static readonly E[] FlyingEnemies = {
			E.Fly,
			E.PastDemon,
			E.PresentDemon,
			E.PastLargeDemon,
			E.PresentLargeDemon,
			E.PastMoth,
			E.PresentMoth,
			E.HelicopterCheveux,
			E.ScyteDemon,
			E.Ryshia,
			E.MeteorSparrow,
			E.PlasmaPod
		};

		static readonly E[] CeilingEnemies = {
			E.CeilingStar,
			E.Bat,
			E.PastCeilingTentacle,
			E.PresentCeilingTentacle,
			E.PastWaterDrake,
			E.PresentWaterDrake
		};

		static readonly E[] OtherEnemies = {
			E.PastSnail, //to large for timestop
			E.PresentSnail, //to large for timestop
			E.HellSpider, //path blocking lazer, timestop immunity
			E.Conviction, //timestop immunity
			E.Zeal, //timestop immunity
			E.Justice, //timestop immunity
			//E.Nethershade, // invisable, timestop immunity
			E.Turret, //timestop immunity
			E.LabDemon, //timestop immunity
			E.FlyingIceMage, // Flying buy to hard to controll 
			E.TomeOrbGuy // Flying buy to hard to controll 
		};

		static readonly E[] Enemies = GroundedEnemies
			.Concat(FlyingEnemies)
			.Concat(CeilingEnemies)
			.Concat(OtherEnemies)
			.ToArray();

		static readonly E[] UnderwaterEnemies = Enemies.Concat(new[] {
			E.Eel,
			E.PastAnemone,  //Flies to top of screen
			E.PresentAnemone  //Flies to top of screen
		}).ToArray();

		static readonly LookupDictionary<Roomkey, RoomSpecificEnemies> HardcodedEnemies
			= new LookupDictionary<Roomkey, RoomSpecificEnemies>(k => k.RoomKey) {
				new RoomSpecificEnemies(1, 10, E.JumpingCheveuxTank, //lake desolation bridge
					GroundedEnemies.Concat(FlyingEnemies)), 
				new RoomSpecificEnemies(9, 19, 608, 928, E.PresentMushroomTower, //Sealed caves mushroom tower jump
					LargeGroundedEnemies.Concat(FlyingEnemies)), 
                new RoomSpecificEnemies(1, 3, 312, 320, E.CheveuxTank, //lake desolation ledge near save
	                NormalGroundedEnemies.Concat(FlyingEnemies)), 
				new RoomSpecificEnemies(7, 3, 312, 320, E.ForestBabyCheveux, //lake serene ledge near save
					NormalGroundedEnemies.Concat(FlyingEnemies)), 
				new RoomSpecificEnemies(1, 2, 936, 656, E.JumpingCheveuxTank, // lake desolation T chest
					FlyingEnemies.Concat(new [] {
						E.JumpingCheveuxTank,
						E.ForestBabyCheveux,
						E.PastLargeSoldier,
						E.PresentLargeSoldier,
						E.LargeCheveux,
						E.IceMage
					})),
				new RoomSpecificEnemies(3, 4, 776, 112, E.PastMoth, //forest green bridge jump
					FlyingEnemies), 
				new RoomSpecificEnemies(4, 1, new []{ new Point(368, 160), new Point(432, 160) }, E.PastBomber, //castle boomers
					E.PastBomber), 
				new RoomSpecificEnemies(10, 3, E.PresentBomber, //militairy fortress boomers
					E.PresentBomber), 
				new RoomSpecificEnemies(7, 5, E.ChargingCheveux, //fluffy bird pre cantoran
					E.ChargingCheveux), 
				new RoomSpecificEnemies(3, 15, E.Bat, //double bat cave jump
					FlyingEnemies.Concat(E.ForestBabyCheveux)), 
				new RoomSpecificEnemies(4, 3, 192, 144, E.PastBomber, //castle scare the engineer
					FlyingEnemies.Concat(new [] {
						E.JumpingCheveuxTank,
						E.WormFlowerWalker,
						E.RoombaCat,
						E.ForestBabyCheveux,
						E.PastLargeSoldier,
						E.PresentLargeSoldier,
						E.IceMage,
						E.FireMage,
						E.LargeCheveux,
						E.CheveuxTank,
						E.PastBomber,
						E.PresentBomber,
						E.Granadier,
						E.Rat,
						E.PastSlime,
						E.PresentSlime,
						E.Spider
					})),
				new RoomSpecificEnemies(5, 20, 584, 192, E.FireMage, // fire mage before twins
					GroundedEnemies.Concat(FlyingEnemies)),
				new RoomSpecificEnemies(6, 2, E.PastDemon, // small demon in mid royal towers
					FlyingEnemies.Concat(E.ForestBabyCheveux)),
				new RoomSpecificEnemies(6, 1, E.IceMage, // ice mage courtyard jump
					FlyingEnemies.Concat(new [] {
						E.JumpingCheveuxTank,
						E.WormFlowerWalker,
						E.RoombaCat,
						E.ForestBabyCheveux,
						E.PastLargeSoldier,
						E.PresentLargeSoldier,
						E.IceMage,
						E.FireMage,
						E.LargeCheveux,
						E.CheveuxTank,
						E.Granadier,
						E.Rat,
						E.PastBomber,
						E.PresentBomber,
						E.PastSlime,
						E.PresentSlime,
						E.Spider
					})),
				new RoomSpecificEnemies(6, 17, E.PastLargeDemon, // top struggle juggle
					FlyingEnemies), 
				new RoomSpecificEnemies(6, 10, E.PastLargeDemon, // lower struggle juggle
					FlyingEnemies.Concat(new [] { 
						E.ForestBabyCheveux, 
						E.IceMage,
					})),
				new RoomSpecificEnemies(12, 21, E.PresentDemon, // emperors left tower
					FlyingEnemies),
				new RoomSpecificEnemies(12, 9, new []{ new Point(280, 225), new Point(278, 223), new Point(278, 224) }, E.PresentDemon, // emperors lower right tower
					FlyingEnemies),
				new RoomSpecificEnemies(12, 17, E.PresentDemon, // emperors upper right tower
					FlyingEnemies),
				new RoomSpecificEnemies(10, 6, E.PresentBomber, //bombers before lab near gun-orb
					E.PresentBomber),
				new RoomSpecificEnemies(9, 7, E.XarionBossHand, //Xarions hand
					E.XarionBossHand) 
			};

/*TODO
Plantbat breaks on floor
Lab turret faces wrong way
*/

		public static void RandomizeEnemies(
			Level level, dynamic levelReflected, int levelId, int roomId, IEnumerable<Monster> enemies, Seed seed)
		{
			var random = new Random((int)(seed.Id + (levelId * 100) + roomId));

			foreach (var enemy in enemies)
			{
				if (enemy.EnemyType == EEnemyTileType.JunkSpawner || enemy.EnemyType == EEnemyTileType.LabAdult)
					continue;
				
				var type = enemy.GetType();

				if (type.IsSubclassOf(BossType))
					continue;

				E newEnemyType;
				if (HardcodedEnemies.TryGetValue(new Roomkey(levelId, roomId), out var hardcodedEnemy) 
					&& Enemy.Get[hardcodedEnemy.EnemyTypeToReplace].Class == type
					&& (hardcodedEnemy.EnemyPositions == null || hardcodedEnemy.EnemyPositions.Contains(enemy.Position)))
				{
					if (hardcodedEnemy.Enemies.Length == 1 && hardcodedEnemy.EnemyTypeToReplace == hardcodedEnemy.Enemies[0])
						continue;

					newEnemyType = hardcodedEnemy.Enemies.SelectRandom(random);
				}
				else
				{
					newEnemyType = (enemy.IsInWater)
						? UnderwaterEnemies.SelectRandom(random)
						: Enemies.SelectRandom(random);
				}

				var newEnemy = enemy.ReplaceWith(level, Enemy.Get[newEnemyType]);

				levelReflected.RequestAddObject(newEnemy);
			}
		}
	}
	
	class RoomSpecificEnemies
	{
		public Roomkey RoomKey;
		public E EnemyTypeToReplace;
		public Point[] EnemyPositions;
		public E[] Enemies;

		public RoomSpecificEnemies(int levelId, int roomId, E enemyType, IEnumerable<E> validEnemies)
			: this(levelId, roomId, null, enemyType, validEnemies.ToArray())
		{
		}

		public RoomSpecificEnemies(int levelId, int roomId, int x, int y, E enemyType, IEnumerable<E> validEnemies)
			: this(levelId, roomId, new []{ new Point(x, y) }, enemyType, validEnemies.ToArray())
		{
		}

		public RoomSpecificEnemies(int levelId, int roomId, Point[] positions, E enemyType, IEnumerable<E> validEnemies)
			: this(levelId, roomId, positions, enemyType, validEnemies.ToArray())
		{
		}

		public RoomSpecificEnemies(int levelId, int roomId, E enemyType, params E[] validEnemies)
			: this(levelId, roomId, null, enemyType, validEnemies)
		{
		}

		public RoomSpecificEnemies(int levelId, int roomId, int x, int y, E enemyType, params E[] validEnemies)
			: this(levelId, roomId, new[] { new Point(x, y) }, enemyType, validEnemies)
		{
		}

		public RoomSpecificEnemies(int levelId, int roomId, Point[] positions, E enemyType, params E[] validEnemies)
		{
			if (validEnemies.Length == 0)
				throw new InvalidOperationException(
					$"RoomSpecificEnemies for level: {levelId}, room: {roomId}, for enemyType: {enemyType}, does not have any validEnemies specified");

			RoomKey = new Roomkey(levelId, roomId);
			EnemyTypeToReplace = enemyType;
			EnemyPositions = positions;
			Enemies = validEnemies;
		}
	}
}
