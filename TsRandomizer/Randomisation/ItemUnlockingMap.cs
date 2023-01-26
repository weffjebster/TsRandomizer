﻿using System;
using System.Collections.Generic;
using System.Linq;
using Timespinner.GameAbstractions.Gameplay;
using Timespinner.GameAbstractions.Inventory;
using Timespinner.GameAbstractions.Saving;
using TsRandomizer.Extensions;
using TsRandomizer.IntermediateObjects;
using TsRandomizer.IntermediateObjects.CustomItems;
using TsRandomizer.Randomisation.ItemPlacers;
using R = TsRandomizer.Randomisation.Requirement;

namespace TsRandomizer.Randomisation
{
	class DefaultItemUnlockingMap : ItemUnlockingMap
	{
		public DefaultItemUnlockingMap(Seed seed) : base(seed)
		{
			if (!seed.Options.UnchainedKeys)
				SetTeleporterPickupAction(seed);
			else
				SetUnchainedKeyPickupActions(seed);
		}

		void SetTeleporterPickupAction(Seed seed)
		{
			IEnumerable<TeleporterGate> teleporterGates = PresentTeleporterGates;

			if (!seed.Options.Inverted)
			{
				IEnumerable<TeleporterGate> pastTeleporterGates = PastTeleporterGates;

				if (seed.FloodFlags.Maw)
					pastTeleporterGates = pastTeleporterGates.Where(g => g.Gate != R.GateMaw);

				teleporterGates = teleporterGates.Union(pastTeleporterGates);
			}

			var selectedGate = teleporterGates.SelectRandom(Random);

			var pyramidUnlockingSpecification =
				new UnlockingSpecification(new ItemIdentifier(EInventoryRelicType.PyramidsKey), R.None, R.Teleport);

			pyramidUnlockingSpecification.OnPickup = level => {
				UnlockRoom(level, selectedGate.LevelId, selectedGate.RoomId);

				if (seed.Options.EnterSandman)
				{
					UnlockFirstPyramidPortal(level);
					pyramidUnlockingSpecification.AdditionalUnlocks = PyramidTeleporterGates[1].Gate;
				}

			};

			pyramidUnlockingSpecification.Unlocks = selectedGate.Gate;

			UnlockingSpecifications.Add(pyramidUnlockingSpecification);
		}

		void SetUnchainedKeyPickupActions(Seed seed)
		{
			IEnumerable<TeleporterGate> pastTeleporterGates = PastTeleporterGates;

			if (seed.FloodFlags.Maw)
				pastTeleporterGates = pastTeleporterGates.Where(g => g.Gate != R.GateMaw);

			SetUnchainedKeysUnlock(Random, new TimewornWarpBeacon(), R.PastWarp, pastTeleporterGates.ToArray());
			SetUnchainedKeysUnlock(Random, new ModernWarpBeacon(), R.PresentWarp, PresentTeleporterGates);

			if (seed.Options.EnterSandman)
				SetUnchainedKeysUnlock(Random, new MysteriousWarpBeacon(), R.PyramidWarp, PyramidTeleporterGates);
		}

		protected void SetUnchainedKeysUnlock(Random random, CustomItem item, R unlock, TeleporterGate[] gates)
		{
			var pyramidWarpUnlockingSpecification = new UnlockingSpecification(item.Identifier, unlock);
			var pyramidGate = gates.SelectRandom(random);

			item.SetDescription($"You feel the twin pyramid key attune to: {pyramidGate.Name}", "Twin Pyramid Key");

			pyramidWarpUnlockingSpecification.OnPickup = level => UnlockRoom(level, pyramidGate.LevelId, pyramidGate.RoomId);
			pyramidWarpUnlockingSpecification.Unlocks = pyramidGate.Gate;

			UnlockingSpecifications.Add(pyramidWarpUnlockingSpecification);
		}
	}

	class ArchipelagoUnlockingMap : ItemUnlockingMap
	{
		public ArchipelagoUnlockingMap(Seed seed, GameSave saveGame) : base(seed)
		{
			var allTeleporterGates = PresentTeleporterGates
				.Union(PastTeleporterGates)
				.Union(PyramidTeleporterGates)
				.ToArray();

			if (!seed.Options.UnchainedKeys)
			{
				saveGame.DataKeyStrings
					.TryParsePyramidKeysUnlock(ArchipelagoItemLocationRandomizer.GameSavePyramidsKeysUnlock, out var pyramidsGate);

				SetTeleporterPickupAction(seed, allTeleporterGates, pyramidsGate, new ItemIdentifier(EInventoryRelicType.PyramidsKey));
			}
			else
			{
				saveGame.DataKeyStrings.TryParsePyramidKeysUnlock(
					ArchipelagoItemLocationRandomizer.GameSavePastPyramidsKeysUnlock, out var pastGate);
				SetTeleporterPickupAction(
					seed, allTeleporterGates, pastGate, CustomItem.GetIdentifier(CustomItemType.TimewornWarpBeacon));

				saveGame.DataKeyStrings.TryParsePyramidKeysUnlock(
					ArchipelagoItemLocationRandomizer.GameSavePresentPyramidsKeysUnlock, out var presentGate);
				SetTeleporterPickupAction(
					seed, allTeleporterGates, presentGate, CustomItem.GetIdentifier(CustomItemType.ModernWarpBeacon));

				saveGame.DataKeyStrings.TryParsePyramidKeysUnlock(
					ArchipelagoItemLocationRandomizer.GameSaveTimePyramidsKeysUnlock, out var timeGate);
				SetTeleporterPickupAction(
					seed, allTeleporterGates, timeGate, CustomItem.GetIdentifier(CustomItemType.MysteriousWarpBeacon));
			}
		}
		
		void SetTeleporterPickupAction(Seed seed, TeleporterGate[] allGates, R gateToUnlock, ItemIdentifier item)
		{
			var selectedGate = allGates.First(g => g.Gate == gateToUnlock);

			var unlockingSpecification = new UnlockingSpecification(item, R.None, R.Teleport);

			unlockingSpecification.OnPickup = level => {
				UnlockRoom(level, selectedGate.LevelId, selectedGate.RoomId);

				if (seed.Options.EnterSandman)
				{
					UnlockFirstPyramidPortal(level);

					unlockingSpecification.AdditionalUnlocks = PyramidTeleporterGates[1].Gate;
				}
			};

			unlockingSpecification.Unlocks = selectedGate.Gate;

			UnlockingSpecifications.Add(unlockingSpecification);
		}
	}

	abstract class ItemUnlockingMap
	{
		protected static readonly TeleporterGate[] PresentTeleporterGates =
		{
			new TeleporterGate{Gate = R.GateKittyBoss, LevelId = 2, RoomId = 55, Name = "Sewers"},
			new TeleporterGate{Gate = R.GateLeftLibrary, LevelId = 2, RoomId = 54, Name = "Library"},
			new TeleporterGate{Gate = R.GateMilitaryGate, LevelId = 10, RoomId = 12, Name = "Military Hangar"},
			new TeleporterGate{Gate = R.GateSealedCaves, LevelId = 9, RoomId = 50, Name = "Xarion's Cave Entrance"},
			//new TeleporterGate{Gate = R.GateXarion, LevelId = 9, RoomId = 49}, //dont want to spawn infront of xarion
			new TeleporterGate{Gate = R.GateSealedSirensCave, LevelId = 9, RoomId = 51, Name = "Sirens' Cave"},
			new TeleporterGate{Gate = R.GateLakeDesolation, LevelId = 1, RoomId = 25, Name = "Lake Desolation"}
		};

		protected static readonly TeleporterGate[] PastTeleporterGates =
		{
			//new TeleporterGate{Gate = Requirement.GateLakeSirineLeft, LevelId = 7, RoomId = 30}, //you dont want to spawn with a boss in your face
			new TeleporterGate{Gate = R.GateLakeSereneRight, LevelId = 7, RoomId = 31, Name = "East Lake Serene"},
			new TeleporterGate{Gate = R.GateAccessToPast, LevelId = 8, RoomId = 51, Name = "Upper Caves of Banishment"},
			//new TeleporterGate{Gate = Requirement.GateAccessToPast, LevelId = 3, RoomId = 6}, //Refugee Camp, Somehow doesnt work ¯\_(ツ)_/¯
			new TeleporterGate{Gate = R.GateCastleRamparts, LevelId = 4, RoomId = 23, Name = "Castle Ramparts"},
			new TeleporterGate{Gate = R.GateCastleKeep, LevelId = 5, RoomId = 24, Name = "Castle Keep"},
			new TeleporterGate{Gate = R.GateRoyalTowers, LevelId = 6, RoomId = 0, Name = "Royal Towers"},
			new TeleporterGate{Gate = R.GateMaw, LevelId = 8, RoomId = 49, Name = "Maw's Lair"},
			new TeleporterGate{Gate = R.GateCavesOfBanishment, LevelId = 8, RoomId = 50, Name = "Maw's Cave Entrance"}
		};

		protected static readonly TeleporterGate[] PyramidTeleporterGates =
		{
			new TeleporterGate{Gate = R.GateGyre, LevelId = 14, RoomId = 1, Name = "Temporal Gyre Entrance"},
			new TeleporterGate{Gate = R.GateLeftPyramid, LevelId = 16, RoomId = 12, Name = "Ancient Pyramid Entrance"},
			new TeleporterGate{Gate = R.GateRightPyramid, LevelId = 16, RoomId = 19, Name = "Inner Ancient Pyramid"}
		};

		protected readonly LookupDictionary<ItemIdentifier, UnlockingSpecification> UnlockingSpecifications;

		public IEnumerable<ItemIdentifier> AllProgressionItems => UnlockingSpecifications.Select(us => us.Item);
		public R PyramidKeysUnlock => UnlockingSpecifications[new ItemIdentifier(EInventoryRelicType.PyramidsKey)].Unlocks;


		protected Random Random;

		protected ItemUnlockingMap(Seed seed)
		{
			Random = new Random((int)seed.Id);

			UnlockingSpecifications = new LookupDictionary<ItemIdentifier, UnlockingSpecification>(29, s => s.Item)
			{
				new UnlockingSpecification(new ItemIdentifier(EInventoryRelicType.TimespinnerWheel), R.TimespinnerWheel, R.TimeStop),
				new UnlockingSpecification(new ItemIdentifier(EInventoryRelicType.DoubleJump), R.DoubleJump, R.TimeStop),
				new UnlockingSpecification(new ItemIdentifier(EInventoryRelicType.Dash), R.ForwardDash),
				new UnlockingSpecification(new ItemIdentifier(EInventoryOrbType.Flame, EOrbSlot.Passive), R.AntiWeed),
				new UnlockingSpecification(new ItemIdentifier(EInventoryOrbType.Flame, EOrbSlot.Melee), R.AntiWeed),
				new UnlockingSpecification(new ItemIdentifier(EInventoryOrbType.Flame, EOrbSlot.Spell), R.AntiWeed),
				new UnlockingSpecification(new ItemIdentifier(EInventoryOrbType.Book, EOrbSlot.Spell), R.AntiWeed),
				new UnlockingSpecification(new ItemIdentifier(EInventoryRelicType.ScienceKeycardA), R.CardA, R.CardB | R.CardC | R.CardD),
				new UnlockingSpecification(new ItemIdentifier(EInventoryRelicType.ScienceKeycardB), R.CardB, R.CardC | R.CardD),
				new UnlockingSpecification(new ItemIdentifier(EInventoryRelicType.ScienceKeycardC), R.CardC, R.CardD),
				new UnlockingSpecification(new ItemIdentifier(EInventoryRelicType.ScienceKeycardD), R.CardD),
				new UnlockingSpecification(new ItemIdentifier(EInventoryRelicType.ElevatorKeycard), R.CardE),
				new UnlockingSpecification(new ItemIdentifier(EInventoryRelicType.ScienceKeycardV), R.CardV),
				new UnlockingSpecification(new ItemIdentifier(EInventoryRelicType.WaterMask), R.Swimming),
				new UnlockingSpecification(new ItemIdentifier(EInventoryRelicType.TimespinnerSpindle), R.TimespinnerSpindle),
				new UnlockingSpecification(new ItemIdentifier(EInventoryRelicType.TimespinnerGear1), R.TimespinnerPiece1),
				new UnlockingSpecification(new ItemIdentifier(EInventoryRelicType.TimespinnerGear2), R.TimespinnerPiece2),
				new UnlockingSpecification(new ItemIdentifier(EInventoryRelicType.TimespinnerGear3), R.TimespinnerPiece3),
				new UnlockingSpecification(new ItemIdentifier(EInventoryRelicType.EssenceOfSpace), R.UpwardDash, R.DoubleJump | R.TimeStop),
				new UnlockingSpecification(new ItemIdentifier(EInventoryOrbType.Barrier, EOrbSlot.Spell), R.UpwardDash, R.DoubleJump | R.TimeStop),
				new UnlockingSpecification(new ItemIdentifier(EInventoryOrbType.Pink, EOrbSlot.Melee), R.PinkOrb),
				new UnlockingSpecification(new ItemIdentifier(EInventoryOrbType.Pink, EOrbSlot.Spell), R.PinkOrb),
				new UnlockingSpecification(new ItemIdentifier(EInventoryOrbType.Pink, EOrbSlot.Passive), R.PinkOrb),
				new UnlockingSpecification(new ItemIdentifier(EInventoryRelicType.AirMask), R.GasMask),
				new UnlockingSpecification(new ItemIdentifier(EInventoryRelicType.Tablet), R.Tablet),
				new UnlockingSpecification(new ItemIdentifier(EInventoryOrbType.Eye, EOrbSlot.Passive), R.OculusRift),
				new UnlockingSpecification(new ItemIdentifier(EInventoryFamiliarType.Kobo), R.Kobo),
				new UnlockingSpecification(new ItemIdentifier(EInventoryFamiliarType.MerchantCrow), R.MerchantCrow)
			};

			if (seed.Options.SpecificKeys)
				MakeKeyCardUnlocksCardSpecific();
		}

		void MakeKeyCardUnlocksCardSpecific()
		{
			UnlockingSpecifications[new ItemIdentifier(EInventoryRelicType.ScienceKeycardA)].AdditionalUnlocks = R.None;
			UnlockingSpecifications[new ItemIdentifier(EInventoryRelicType.ScienceKeycardB)].AdditionalUnlocks = R.None;
			UnlockingSpecifications[new ItemIdentifier(EInventoryRelicType.ScienceKeycardC)].AdditionalUnlocks = R.None;
			UnlockingSpecifications[new ItemIdentifier(EInventoryRelicType.ScienceKeycardD)].AdditionalUnlocks = R.None;
		}

		public R GetAllUnlock(ItemIdentifier identifier) =>
			UnlockingSpecifications.TryGetValue(identifier, out var value)
				? value.AllUnlocks
				: R.None;

		public Action<Level> GetPickupAction(ItemIdentifier identifier) =>
			UnlockingSpecifications.TryGetValue(identifier, out var value)
				? value.OnPickup
				: null;

		protected static void UnlockRoom(Level level, int levelId, int roomId)
		{
			var minimapRoom = level.Minimap.Areas[levelId].Rooms[roomId];

			minimapRoom.SetKnown(true);
			minimapRoom.SetVisited(true);
		}

		protected static void UnlockFirstPyramidPortal(Level level) =>
			UnlockRoom(level, 16, 12);

		protected class UnlockingSpecification
		{
			public ItemIdentifier Item { get; }
			public R Unlocks { get; internal set; }
			public R AdditionalUnlocks { get; internal set; }
			public R AllUnlocks => Unlocks | AdditionalUnlocks;
			public Action<Level> OnPickup { get; internal set; }

			public UnlockingSpecification(ItemIdentifier item, R unlocks, R? additionalUnlocks = null)
			{
				Item = item;
				Unlocks = unlocks;
				AdditionalUnlocks = additionalUnlocks ?? R.None;
			}
		}

		protected class TeleporterGate
		{
			public R Gate { get; internal set; }
			public int LevelId { get; internal set; }
			public int RoomId { get; internal set; }
			public string Name { get; internal set; }
		}
	}
}
