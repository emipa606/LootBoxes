using System;
using System.Collections.Generic;
using System.Reflection;
using JetBrains.Annotations;
using Lanilor.LootBoxes.Things;
using UnityEngine;
using Verse;
#if V10
using Harmony;
#else
using HarmonyLib;
#endif

namespace Lanilor.LootBoxes.Mod
{
    [UsedImplicitly]
    public class ModLootBoxes : Verse.Mod
    {
        public static ModSettingsLootBoxes Settings;

        public ModLootBoxes(ModContentPack content) : base(content)
        {
            Settings = GetSettings<ModSettingsLootBoxes>();

#if V10
            var harmony = HarmonyInstance.Create("rimworld.lanilor.lootboxes");
#else
            var harmony = new Harmony("rimworld.lanilor.lootboxes");
#endif
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        [NotNull]
        public override string SettingsCategory()
        {
            return "LootBoxes_SettingsCategory".Translate();
        }

        private LootBoxType m_Selected = LootBoxType.Treasure;

        private Vector2 m_Scroll = Vector2.zero;

        public override void DoSettingsWindowContents(Rect inRect)
        {
            inRect.yMin += 15f;
            inRect.yMax -= 15f;

            var columnWidth = inRect.width - 50f;
            var ls = new Listing_Standard
            {
                ColumnWidth = columnWidth
            };

            var r = new Rect(0f, 0f, inRect.width - 16f, inRect.height * 2f);
            Widgets.BeginScrollView(inRect, ref m_Scroll, r);
            ls.Begin(r);

            ls.Gap();

            ls.CheckboxLabeled("LootBoxes_SettingsUseIngameRewardsGenerator".Translate(),
                ref Settings.UseIngameRewardsGenerator, "LootBoxes_SettingsUseIngameRewardsGeneratorDesc".Translate());
            ls.Gap();

            ls.CheckboxLabeled("LootBoxes_SettingsUseBonusLootChance".Translate(), ref Settings.UseBonusLootChance,
                "LootBoxes_SettingsUseBonusLootChanceDesc".Translate());
            ls.Gap();

#if !V10
            ls.CheckboxLabeled("LootBoxes_SettingsAllowPsychicAmplifierSpawn".Translate(), ref Settings.AllowPsychicAmplifierSpawn,
                "LootBoxes_SettingsAllowPsychicAmplifierSpawnDesc".Translate());
            ls.Gap();
#endif

            ls.Label("LootBoxes_SettingsBonusLootChance".Translate() + Settings.BonusLootChance + "x");
            ls.Gap();
            Settings.BonusLootChance = Widgets.HorizontalSlider(ls.GetRect(20f), Settings.BonusLootChance, 1, 5, false,
                null, "1x", "5x", 0.1f);
            ls.GapLine();

            ls.Label("LootBoxes_ChancesExplanation".Translate());
            ls.Gap();
            ls.Label("LootBoxes_SettingsChanceForTreasureBox".Translate() + Settings.ChanceForTreasure + "%");
            ls.Gap();
            Settings.ChanceForTreasure = Widgets.HorizontalSlider(ls.GetRect(20f), Settings.ChanceForTreasure, 0, 100,
                false, null, "0%", "100%", 1);
            ls.Gap();
            ls.Label("LootBoxes_SettingsChanceForSilverSmallBox".Translate() + Settings.ChanceForSilverS + "%");
            ls.Gap();
            Settings.ChanceForSilverS = Widgets.HorizontalSlider(ls.GetRect(20f), Settings.ChanceForSilverS, 0, 100,
                false, null, "0%", "100%", 1);
            ls.Gap();
            ls.Label("LootBoxes_SettingsChanceForSilverLargeBox".Translate() + Settings.ChanceForSilverL + "%");
            ls.Gap();
            Settings.ChanceForSilverL = Widgets.HorizontalSlider(ls.GetRect(20f), Settings.ChanceForSilverL, 0, 100,
                false, null, "0%", "100%", 1);
            ls.Gap();
            ls.Label("LootBoxes_SettingsChanceForGoldSmallBox".Translate() + Settings.ChanceForGoldS + "%");
            ls.Gap();
            Settings.ChanceForGoldS = Widgets.HorizontalSlider(ls.GetRect(20f), Settings.ChanceForGoldS, 0, 100, false,
                null, "0%", "100%", 1);
            ls.Gap();
            ls.Label("LootBoxes_SettingsChanceForGoldLargeBox".Translate() + Settings.ChanceForGoldL + "%");
            ls.Gap();
            Settings.ChanceForGoldL = Widgets.HorizontalSlider(ls.GetRect(20f), Settings.ChanceForGoldL, 0, 100, false,
                null, "0%", "100%", 1);
            ls.Gap();
            ls.Label("LootBoxes_SettingsChanceForPandoraBox".Translate() + Settings.ChanceForPandora + "%");
            ls.Gap();
            Settings.ChanceForPandora = Widgets.HorizontalSlider(ls.GetRect(20f), Settings.ChanceForPandora, 0, 100,
                false, null, "0%", "100%", 1);
            ls.Gap();

            ls.GapLine();

            if (ls.ButtonText($"LootBoxes_Settings{m_Selected}ButtonLabel".Translate(), $"LootBoxes_Settings{m_Selected}Button".Translate()))
            {
                var window = new FloatMenu(new List<FloatMenuOption>
                {
                    new FloatMenuOption("LootBoxes_SettingsTreasureButtonLabel".Translate(),
                        () => m_Selected = LootBoxType.Treasure),
                    new FloatMenuOption("LootBoxes_SettingsSilverSButtonLabel".Translate(),
                        () => m_Selected = LootBoxType.SilverS),
                    new FloatMenuOption("LootBoxes_SettingsSilverLButtonLabel".Translate(),
                        () => m_Selected = LootBoxType.SilverL),
                    new FloatMenuOption("LootBoxes_SettingsGoldSButtonLabel".Translate(),
                        () => m_Selected = LootBoxType.GoldS),
                    new FloatMenuOption("LootBoxes_SettingsGoldLButtonLabel".Translate(),
                        () => m_Selected = LootBoxType.GoldL)
                })
                {
                    vanishIfMouseDistant = true
                };
                Find.WindowStack.Add(window);
            }

            ls.Gap();

            var min = 1;
            var max = 1;
            var value = 10f;
            var multiplier = 0.25f;
            switch (m_Selected)
            {
                case LootBoxType.Treasure:
                    min = Settings.SetMinTreasure;
                    max = Settings.SetMaxTreasure;
                    value = Settings.TreasureRewardValue;
                    multiplier = Settings.TreasureLootboxChanceMultiplier;
                    break;
                case LootBoxType.SilverS:
                    min = Settings.SetMinSilverS;
                    max = Settings.SetMaxSilverS;
                    value = Settings.SilverSRewardValue;
                    multiplier = Settings.SilverSLootboxChanceMultiplier;
                    break;
                case LootBoxType.SilverL:
                    min = Settings.SetMinSilverL;
                    max = Settings.SetMaxSilverL;
                    value = Settings.SilverLRewardValue;
                    multiplier = Settings.SilverLLootboxChanceMultiplier;
                    break;
                case LootBoxType.GoldS:
                    min = Settings.SetMinGoldS;
                    max = Settings.SetMaxGoldS;
                    value = Settings.GoldSRewardValue;
                    multiplier = Settings.GoldSLootboxChanceMultiplier;
                    break;
                case LootBoxType.GoldL:
                    min = Settings.SetMinGoldL;
                    max = Settings.SetMaxGoldL;
                    value = Settings.GoldLRewardValue;
                    multiplier = Settings.GoldLLootboxChanceMultiplier;
                    break;
            }

            ls.Label($"LootBoxes_Settings{m_Selected}Button".Translate());
            ls.Gap();

            ls.Label("LootBoxes_SettingsMinSet".Translate() + min, -1, "LootBoxes_MinSetTooltip".Translate());
            ls.Gap();
            min = Math.Min(max, (int)Widgets.HorizontalSlider(ls.GetRect(20f), min, 1, 50, false, null, "1", "50", 1f));
            ls.Gap();

            ls.Label("LootBoxes_SettingsMaxSet".Translate() + max, -1, "LootBoxes_MaxSetTooltip".Translate());
            ls.Gap();
            max = Math.Max(min, (int)Widgets.HorizontalSlider(ls.GetRect(20f), max, 1, 50, false, null, "1", "50", 1f));
            ls.Gap();

            ls.Label("LootBoxes_SettingsRewardValue".Translate() + value, -1, "LootBoxes_RewardValueTooltip".Translate());
            ls.Gap();
            value = (int)Widgets.HorizontalSlider(ls.GetRect(20f), value, 10, 5000, false, null, "10", "5000", 10f);
            ls.Gap();

            ls.Label("LootBoxes_SettingsLootboxChanceMult".Translate() + multiplier + "x", -1, "LootBoxes_LootboxChanceMultTooltip".Translate());
            ls.Gap();
            multiplier = Widgets.HorizontalSlider(ls.GetRect(20f), multiplier, 0, 10, false, null, "0x", "10x", 0.01f);
            ls.GapLine();

            switch (m_Selected)
            {
                case LootBoxType.Treasure:
                    Settings.SetMinTreasure = min;
                    Settings.SetMaxTreasure = max;
                    Settings.TreasureRewardValue = value;
                    Settings.TreasureLootboxChanceMultiplier = multiplier;
                    break;
                case LootBoxType.SilverS:
                    Settings.SetMinSilverS = min;
                    Settings.SetMaxSilverS = max;
                    Settings.SilverSRewardValue = value;
                    Settings.SilverSLootboxChanceMultiplier = multiplier;
                    break;
                case LootBoxType.SilverL:
                    Settings.SetMinSilverL = min;
                    Settings.SetMaxSilverL = max;
                    Settings.SilverLRewardValue = value;
                    Settings.SilverLLootboxChanceMultiplier = multiplier;
                    break;
                case LootBoxType.GoldS:
                    Settings.SetMinGoldS = min;
                    Settings.SetMaxGoldS = max;
                    Settings.GoldSRewardValue = value;
                    Settings.GoldSLootboxChanceMultiplier = multiplier;
                    break;
                case LootBoxType.GoldL:
                    Settings.SetMinGoldL = min;
                    Settings.SetMaxGoldL = max;
                    Settings.GoldLRewardValue = value;
                    Settings.GoldLLootboxChanceMultiplier = multiplier;
                    break;
            }

            var reset = ls.ButtonText("LootBoxes_SettingsResetButtonLabel".Translate(),
                "LootBoxes_SettingsResetButton".Translate());

            if (reset)
                Settings.Reset();

            ls.End();
            Widgets.EndScrollView();

            Settings.Write();
        }
    }
}