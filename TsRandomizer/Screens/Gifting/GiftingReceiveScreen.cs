﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Timespinner.GameAbstractions;
using Timespinner.GameAbstractions.Inventory;
using Timespinner.GameStateManagement.ScreenManager;
using TsRandomizer.Archipelago.Gifting;
using TsRandomizer.Extensions;
using TsRandomizer.IntermediateObjects;
using TsRandomizer.Randomisation;
using TsRandomizer.Screens.Menu;

namespace TsRandomizer.Screens.Gifting
{
	class GiftingReceiveScreen : GiftingScreen
	{
		const int NumberOfTraitsToDisplay = 7;

		static readonly Type StatEntryType =
			TimeSpinnerType.Get("Timespinner.GameStateManagement.Screens.BaseClasses.Menu.StatEntry");
		static readonly Type StatEntryDisplayEnumType =
			TimeSpinnerType.Get("Timespinner.GameStateManagement.Screens.BaseClasses.Menu.StatEntry+EStatDisplayType");
		static readonly Type MenuUseItemInventoryType =
			TimeSpinnerType.Get("Timespinner.GameStateManagement.Screens.BaseClasses.Menu.MenuUseItemInventory");
		static readonly Type MenuRelicInventoryType =
			TimeSpinnerType.Get("Timespinner.GameStateManagement.Screens.BaseClasses.Menu.MenuRelicInventory");

		static readonly Dictionary<Trait, string> Descriptions = new Dictionary<Trait, string> {
			{ Trait.Armor, "A piece of armor (body or hat)" },
			{ Trait.Egg, "Gilded Egg's" },
			{ Trait.Flower, "Chaos status's" },
			{ Trait.Drink, "Potions" },
			{ Trait.Food, "Solid food that restores health" },
			{ Trait.Fruit, "Healthy fruit that restores health or mana" },
			{ Trait.Vegetable, "Solid food that restores health containing vegetables" },
			{ Trait.Meat, "Solid food that restores health containing meat" },
			{ Trait.Fish, "Solid food that restores health containing fish" },
			{ Trait.Heal, "Healing items" },
			{ Trait.Mana, "Mana items" },
			{ Trait.Cure, "Items that cure status ailments" },
			{ Trait.Speed, "Warp Shards" },
			{ Trait.Consumable, "Use items" },
			{ Trait.Resource, "Sand bottles" },
		};

		static readonly Dictionary<Trait, object> Icons = new Dictionary<Trait, object> {
			{ Trait.Armor,  EInventoryItemIconType.GetEnumValue("AdvisorRobe") },
			{ Trait.Egg, EInventoryItemIconType.GetEnumValue("FamiliarEgg") },
			{ Trait.Flower, EInventoryItemIconType.GetEnumValue("ChaosHeal") },
			{ Trait.Drink, EInventoryItemIconType.GetEnumValue("FiligreeTea") },
			{ Trait.Food, EInventoryItemIconType.GetEnumValue("Spaghetti") },
			{ Trait.Fruit, EInventoryItemIconType.GetEnumValue("LachiemiSun") },
			{ Trait.Vegetable, EInventoryItemIconType.GetEnumValue("Casserole") },
			{ Trait.Meat, EInventoryItemIconType.GetEnumValue("FriedCheveux") },
			{ Trait.Fish, EInventoryItemIconType.GetEnumValue("UnagiRoll") },
			{ Trait.Heal, EInventoryItemIconType.GetEnumValue("Potion") },
			{ Trait.Mana, EInventoryItemIconType.GetEnumValue("Ether") },
			{ Trait.Cure, EInventoryItemIconType.GetEnumValue("Antidote") },
			{ Trait.Speed, EInventoryItemIconType.GetEnumValue("WarpCard") },
			{ Trait.Consumable, EInventoryItemIconType.GetEnumValue("FuturePotion") },
			{ Trait.Resource, EInventoryItemIconType.GetEnumValue("SandBottle") },
		};

		//InventoryItem selectedItem;
		//AcceptedTraits selectedPlayer;

		object traitSelectionMenu;
		TraitsInventoryCollection traitsCollection;

		bool shouldUpdateMotherbox;

		static string GetTraitNameKey(Trait trait) => $"TSR_trait_{trait}";
		static string GetTraitDescriptionKey(Trait trait) => GetTraitNameKey(trait) + "_desc";

		static GiftingReceiveScreen()
		{
			foreach (var trait in (Trait[])Enum.GetValues(typeof(Trait)))
			{
				TimeSpinnerGame.Localizer.OverrideKey(GetTraitNameKey(trait), trait.ToString());
				TimeSpinnerGame.Localizer.OverrideKey(GetTraitDescriptionKey(trait), Descriptions[trait]);
			}
		}

		public GiftingReceiveScreen(ScreenManager screenManager, GameScreen gameScreen) : base(screenManager, gameScreen)
		{
		}

		public override void Initialize(ItemLocationMap itemLocationMap, GCM gcm)
		{
			base.Initialize(itemLocationMap, gcm);

			Dynamic._menuTitle = "Gifting - Receive";

			//acceptedTraitsPerSlot = GiftingService.GetAcceptedTraits();
		}

		protected override void PopulateMainMenu(IList menuEntriesList, IList subMenuCollections)
		{
			traitSelectionMenu = CreateMenuForTraits();
			if (traitSelectionMenu != null)
			{
				subMenuCollections.Add(traitSelectionMenu);

				var selectTraitsMenu = MenuEntry.Create("Choose wanted types", () => { Dynamic.ChangeMenuCollection(traitSelectionMenu, true); });
				selectTraitsMenu.IsCenterAligned = false;
				selectTraitsMenu.DoesDrawLargeShadow = false;
				selectTraitsMenu.ColumnWidth = 144;
				menuEntriesList.Add(selectTraitsMenu.AsTimeSpinnerMenuEntry());
			}
			
			var giftReceiveMenu = MenuEntry.Create("Open Gifts", () => { });
			giftReceiveMenu.IsCenterAligned = false;
			giftReceiveMenu.DoesDrawLargeShadow = false;
			giftReceiveMenu.ColumnWidth = 144;
			menuEntriesList.Add(giftReceiveMenu.AsTimeSpinnerMenuEntry());
		}

		object CreateMenuForTraits()
		{
			void OnTraitSelected(Trait trait, InventoryRelic relicMenuItem)
			{
				relicMenuItem.IsActive = !relicMenuItem.IsActive;

				shouldUpdateMotherbox = true;
			}

			traitsCollection = new TraitsInventoryCollection(OnTraitSelected);

			foreach (var trait in (Trait[])Enum.GetValues(typeof(Trait)))
			{
				traitsCollection.AddItem(trait);

				var inventoryRelic = traitsCollection.Inventory[traitsCollection.Inventory.Count - 1];

				inventoryRelic.IsActive = false;
			}

			traitsCollection.RefreshItemNameAndDescriptions();

			var inventoryMenu = MenuRelicInventoryType.CreateInstance(false, traitsCollection, 
				(Action<InventoryRelic>)traitsCollection.OnRelicSelected, GameContentManager.SpPauseMenu).AsDynamic();
			inventoryMenu.Font = GameContentManager.ActiveFont;

			return ~inventoryMenu;
		}

		
		protected override void OnGiftItemAccept(object obj, EventArgs args)
		{
			
		}

		protected override void OnGiftItemCancel(object obj, EventArgs args) => Dynamic.OnCancel(obj, args);

		public override void Update(GameTime gameTime, InputState input)
		{
			base.Update(gameTime, input);

			var selectedIndex = ((object)Dynamic._primaryMenuCollection).AsDynamic().SelectedIndex;
			if (selectedIndex != 0)
				return;

			if (Dynamic._selectedMenuCollection == traitSelectionMenu)
			{
				var menuCollection = traitSelectionMenu.AsDynamic();
				var entries = (IList)menuCollection.Entries;
				var entryIndex = (int)menuCollection.SelectedIndex;

				if (entryIndex < entries.Count)
				{
					var traitName = (string)entries[entryIndex].AsDynamic().Text;

					var highlightedTrait = (Trait)typeof(Trait).GetEnumValue(traitName);
					Dynamic.ChangeDescription(Descriptions[highlightedTrait], Icons[highlightedTrait]);
				}
			}
			else
			{
				UpdateMotherbox();
			}

			RefreshGiftInfo(GameContentManager.ActiveFont);
		}

		void RefreshGiftInfo(SpriteFont menuFont)
		{
			/*var selectedIndex = ((object)Dynamic._primaryMenuCollection).AsDynamic().SelectedIndex;
			if (selectedIndex >= acceptedTraitsPerSlot.Count)
				return;

			AcceptedTraits selectedPlayerTraits = acceptedTraitsPerSlot[selectedIndex];

			var entries = (IList)PlayerInfoCollection.Entries;
			entries.Clear();

			var gameEntry = StatEntryType.CreateInstance().AsDynamic();
			gameEntry.Type = StatEntryDisplayEnumType.GetEnumValue("ColoredText");
			gameEntry.Title = "Game:";
			gameEntry.Text = selectedPlayerTraits.Game;
			gameEntry.TextColor = StatEntryColor;
			gameEntry.Initialize(menuFont);
			gameEntry._drawStringWidth = (int)(menuFont.MeasureString(gameEntry._drawString).X - 24);
			gameEntry._titleTextWidthReduction = gameEntry._drawStringWidth + 2;

			entries.Add(~gameEntry);

			if (selectedPlayerTraits.AcceptsAnyTrait)
			{
				var allTraitsEntry = StatEntryType.CreateInstance().AsDynamic();
				allTraitsEntry.Type = StatEntryDisplayEnumType.GetEnumValue("ColoredText");
				allTraitsEntry.Title = "Wants:";
				allTraitsEntry.Text = "All";
				allTraitsEntry.TextColor = StatEntryColor;
				allTraitsEntry.Initialize(menuFont);
				allTraitsEntry._drawStringWidth = (int)(menuFont.MeasureString(allTraitsEntry._drawString).X - 24);
				allTraitsEntry._titleTextWidthReduction = allTraitsEntry._drawStringWidth + 2;

				entries.Add(~allTraitsEntry);
			}
			else
			{
				var traits = new List<string>(NumberOfTraitsToDisplay + 1) {
					"Wants:"
				};

				for (var i = 0; i < NumberOfTraitsToDisplay; i++)
				{
					if (i == NumberOfTraitsToDisplay - 1)
					{
						if (selectedPlayerTraits.DesiredTraits.Length == NumberOfTraitsToDisplay)
							traits.Add(selectedPlayerTraits.DesiredTraits[i - 1].ToString());
						else if (selectedPlayerTraits.DesiredTraits.Length < NumberOfTraitsToDisplay)
							traits.Add("");
						else
							traits.Add("More...");

					}
					else
					{
						if (i < selectedPlayerTraits.DesiredTraits.Length)
							traits.Add(selectedPlayerTraits.DesiredTraits[i].ToString());
						else
							traits.Add("");
					}
				}

				for (var i = 0; i < NumberOfTraitsToDisplay; i += 2)
				{
					var statEntry = StatEntryType.CreateInstance().AsDynamic();
					statEntry.Type = StatEntryDisplayEnumType.GetEnumValue("ColoredText");
					statEntry.Title = traits[i];
					statEntry.Text = traits[i + 1];
					statEntry.TextColor = StatEntryColor;
					statEntry.Initialize(menuFont);
					statEntry._drawStringWidth = (int)(menuFont.MeasureString(statEntry._drawString).X - 24);
					statEntry._titleTextWidthReduction = statEntry._drawStringWidth + 2;

					entries.Add(~statEntry);
				}
			}*/
		}

		public override void Unload() => UpdateMotherbox();

		void UpdateMotherbox()
		{
			if (!shouldUpdateMotherbox || traitsCollection == null)
				return;

			shouldUpdateMotherbox = false;

			var enabledTraits = traitsCollection.Inventory.Values
				.Where(r => r.IsActive)
				.Select(r => (Trait)r.Key)
				.ToArray();

			GiftingService.SetAcceptedGifts(enabledTraits);
		}

		class TraitsInventoryCollection : InventoryRelicCollection
		{
			readonly Action<Trait, InventoryRelic> onTraitSelected;

			public TraitsInventoryCollection(Action<Trait, InventoryRelic> onTraitSelected)
			{
				this.onTraitSelected = onTraitSelected;
			}

			public void AddItem(Trait item) => AddItem((int)item);

			public override void RefreshItemNameAndDescriptions()
			{
				// ReSharper disable once SuggestVarOrType_SimpleTypes
				foreach (InventoryRelic relic in Inventory.Values)
				{
					var trait = (Trait)relic.Key;

					var dynamicInventoryItem = relic.AsDynamic();
					dynamicInventoryItem.NameKey = GetTraitNameKey(trait);
					dynamicInventoryItem.DescriptionKey = GetTraitDescriptionKey(trait);
				}

				base.RefreshItemNameAndDescriptions();
			}

			public void OnRelicSelected(InventoryRelic relic) =>
				onTraitSelected((Trait)relic.Key, relic);
		}
	}
}
