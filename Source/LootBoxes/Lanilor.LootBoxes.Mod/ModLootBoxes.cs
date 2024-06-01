using System;
using System.Reflection;
using HarmonyLib;
using Lanilor.LootBoxes.Things;
using Mlie;
using UnityEngine;
using Verse;

namespace Lanilor.LootBoxes.Mod;

public class ModLootBoxes : Verse.Mod
{
    public static ModSettingsLootBoxes Settings;

    private static string currentVersion;

    private Vector2 m_Scroll = Vector2.zero;

    private LootBoxType m_Selected;

    public ModLootBoxes(ModContentPack content)
        : base(content)
    {
        Settings = GetSettings<ModSettingsLootBoxes>();
        new Harmony("rimworld.lanilor.lootboxes").PatchAll(Assembly.GetExecutingAssembly());
        currentVersion = VersionFromManifest.GetVersionFromModMetaData(content.ModMetaData);
    }

    public override string SettingsCategory()
    {
        return "LootBoxes_SettingsCategory".Translate();
    }

    public override void DoSettingsWindowContents(Rect inRect)
    {
        inRect.yMin += 15f;
        inRect.yMax -= 15f;
        var columnWidth = inRect.width - 50f;
        var listing_Standard = new Listing_Standard
        {
            ColumnWidth = columnWidth
        };
        var rect = new Rect(0f, 0f, inRect.width - 16f, inRect.height * 2f);
        Widgets.BeginScrollView(inRect, ref m_Scroll, rect);
        listing_Standard.Begin(rect);
        listing_Standard.Gap();
        if (listing_Standard.ButtonText("LootBoxes_SettingsResetButtonLabel".Translate(),
                "LootBoxes_SettingsResetButton".Translate()))
        {
            Settings.Reset();
        }

        listing_Standard.CheckboxLabeled("LootBoxes_SettingsUseIngameRewardsGenerator".Translate(),
            ref Settings.UseIngameRewardsGenerator, "LootBoxes_SettingsUseIngameRewardsGeneratorDesc".Translate());
        listing_Standard.Gap();
        listing_Standard.CheckboxLabeled("LootBoxes_SettingsUseBonusLootChance".Translate(),
            ref Settings.UseBonusLootChance, "LootBoxes_SettingsUseBonusLootChanceDesc".Translate());
        listing_Standard.Gap();
        listing_Standard.CheckboxLabeled("LootBoxes_SettingsAllowPsychicAmplifierSpawn".Translate(),
            ref Settings.AllowPsychicAmplifierSpawn, "LootBoxes_SettingsAllowPsychicAmplifierSpawnDesc".Translate());
        listing_Standard.Gap();
        listing_Standard.Label(string.Concat("LootBoxes_SettingsBonusLootChance".Translate(),
            Settings.BonusLootChance.ToString(), "x"));
        listing_Standard.Gap();
        Settings.BonusLootChance = Widgets.HorizontalSlider(listing_Standard.GetRect(20f), Settings.BonusLootChance, 1f,
            5f, false, null, "1x", "5x", 0.1f);
        listing_Standard.GapLine();
        listing_Standard.Label("LootBoxes_ChancesExplanation".Translate());
        listing_Standard.Gap();
        listing_Standard.Label(string.Concat("LootBoxes_SettingsChanceForTreasureBox".Translate(),
            Settings.ChanceForTreasure.ToString(), "%"));
        listing_Standard.Gap();
        Settings.ChanceForTreasure = Widgets.HorizontalSlider(listing_Standard.GetRect(20f), Settings.ChanceForTreasure,
            0f, 100f, false, null, "0%", "100%", 1f);
        listing_Standard.Gap();
        listing_Standard.Label(string.Concat("LootBoxes_SettingsChanceForSilverSmallBox".Translate(),
            Settings.ChanceForSilverS.ToString(), "%"));
        listing_Standard.Gap();
        Settings.ChanceForSilverS = Widgets.HorizontalSlider(listing_Standard.GetRect(20f), Settings.ChanceForSilverS,
            0f, 100f, false, null, "0%", "100%", 1f);
        listing_Standard.Gap();
        listing_Standard.Label(string.Concat("LootBoxes_SettingsChanceForSilverLargeBox".Translate(),
            Settings.ChanceForSilverL.ToString(), "%"));
        listing_Standard.Gap();
        Settings.ChanceForSilverL = Widgets.HorizontalSlider(listing_Standard.GetRect(20f), Settings.ChanceForSilverL,
            0f, 100f, false, null, "0%", "100%", 1f);
        listing_Standard.Gap();
        listing_Standard.Label(string.Concat("LootBoxes_SettingsChanceForGoldSmallBox".Translate(),
            Settings.ChanceForGoldS.ToString(), "%"));
        listing_Standard.Gap();
        Settings.ChanceForGoldS = Widgets.HorizontalSlider(listing_Standard.GetRect(20f), Settings.ChanceForGoldS, 0f,
            100f, false, null, "0%", "100%", 1f);
        listing_Standard.Gap();
        listing_Standard.Label(string.Concat("LootBoxes_SettingsChanceForGoldLargeBox".Translate(),
            Settings.ChanceForGoldL.ToString(), "%"));
        listing_Standard.Gap();
        Settings.ChanceForGoldL = Widgets.HorizontalSlider(listing_Standard.GetRect(20f), Settings.ChanceForGoldL, 0f,
            100f, false, null, "0%", "100%", 1f);
        listing_Standard.Gap();
        listing_Standard.Label(string.Concat("LootBoxes_SettingsChanceForPandoraBox".Translate(),
            Settings.ChanceForPandora.ToString(), "%"));
        listing_Standard.Gap();
        Settings.ChanceForPandora = Widgets.HorizontalSlider(listing_Standard.GetRect(20f), Settings.ChanceForPandora,
            0f, 100f, false, null, "0%", "100%", 1f);
        listing_Standard.Gap();
        listing_Standard.GapLine();
        if (listing_Standard.ButtonText($"LootBoxes_Settings{m_Selected}ButtonLabel".Translate(),
                $"LootBoxes_Settings{m_Selected}Button".Translate()))
        {
            var window = new FloatMenu([
                new FloatMenuOption("LootBoxes_SettingsTreasureButtonLabel".Translate(),
                    delegate { m_Selected = LootBoxType.Treasure; }),

                new FloatMenuOption("LootBoxes_SettingsSilverSButtonLabel".Translate(),
                    delegate { m_Selected = LootBoxType.SilverS; }),

                new FloatMenuOption("LootBoxes_SettingsSilverLButtonLabel".Translate(),
                    delegate { m_Selected = LootBoxType.SilverL; }),

                new FloatMenuOption("LootBoxes_SettingsGoldSButtonLabel".Translate(),
                    delegate { m_Selected = LootBoxType.GoldS; }),

                new FloatMenuOption("LootBoxes_SettingsGoldLButtonLabel".Translate(),
                    delegate { m_Selected = LootBoxType.GoldL; })
            ])
            {
                vanishIfMouseDistant = true
            };
            Find.WindowStack.Add(window);
        }

        listing_Standard.Gap();
        var num = 1;
        var num2 = 1;
        var value = 10f;
        var value2 = 0.25f;
        switch (m_Selected)
        {
            case LootBoxType.Treasure:
                num = Settings.SetMinTreasure;
                num2 = Settings.SetMaxTreasure;
                value = Settings.TreasureRewardValue;
                value2 = Settings.TreasureLootboxChanceMultiplier;
                break;
            case LootBoxType.SilverS:
                num = Settings.SetMinSilverS;
                num2 = Settings.SetMaxSilverS;
                value = Settings.SilverSRewardValue;
                value2 = Settings.SilverSLootboxChanceMultiplier;
                break;
            case LootBoxType.SilverL:
                num = Settings.SetMinSilverL;
                num2 = Settings.SetMaxSilverL;
                value = Settings.SilverLRewardValue;
                value2 = Settings.SilverLLootboxChanceMultiplier;
                break;
            case LootBoxType.GoldS:
                num = Settings.SetMinGoldS;
                num2 = Settings.SetMaxGoldS;
                value = Settings.GoldSRewardValue;
                value2 = Settings.GoldSLootboxChanceMultiplier;
                break;
            case LootBoxType.GoldL:
                num = Settings.SetMinGoldL;
                num2 = Settings.SetMaxGoldL;
                value = Settings.GoldLRewardValue;
                value2 = Settings.GoldLLootboxChanceMultiplier;
                break;
        }

        listing_Standard.Label($"LootBoxes_Settings{m_Selected}Button".Translate());
        listing_Standard.Gap();
        listing_Standard.Label(string.Concat("LootBoxes_SettingsMinSet".Translate(), num.ToString()), -1f,
            "LootBoxes_MinSetTooltip".Translate());
        listing_Standard.Gap();
        num = Math.Min(num2,
            (int)Widgets.HorizontalSlider(listing_Standard.GetRect(20f), num, 1f, 50f, false, null, "1", "50", 1f));
        listing_Standard.Gap();
        listing_Standard.Label(string.Concat("LootBoxes_SettingsMaxSet".Translate(), num2.ToString()), -1f,
            "LootBoxes_MaxSetTooltip".Translate());
        listing_Standard.Gap();
        num2 = Math.Max(num,
            (int)Widgets.HorizontalSlider(listing_Standard.GetRect(20f), num2, 1f, 50f, false, null, "1", "50", 1f));
        listing_Standard.Gap();
        listing_Standard.Label(string.Concat("LootBoxes_SettingsRewardValue".Translate(), value.ToString()), -1f,
            "LootBoxes_RewardValueTooltip".Translate());
        listing_Standard.Gap();
        value = (int)Widgets.HorizontalSlider(listing_Standard.GetRect(20f), value, 10f, 5000f, false, null, "10",
            "5000", 10f);
        listing_Standard.Gap();
        listing_Standard.Label(string.Concat("LootBoxes_SettingsLootboxChanceMult".Translate(), value2.ToString(), "x"),
            -1f, "LootBoxes_LootboxChanceMultTooltip".Translate());
        listing_Standard.Gap();
        value2 = Widgets.HorizontalSlider(listing_Standard.GetRect(20f), value2, 0f, 10f, false, null, "0x", "10x",
            0.01f);
        listing_Standard.GapLine();
        switch (m_Selected)
        {
            case LootBoxType.Treasure:
                Settings.SetMinTreasure = num;
                Settings.SetMaxTreasure = num2;
                Settings.TreasureRewardValue = value;
                Settings.TreasureLootboxChanceMultiplier = value2;
                break;
            case LootBoxType.SilverS:
                Settings.SetMinSilverS = num;
                Settings.SetMaxSilverS = num2;
                Settings.SilverSRewardValue = value;
                Settings.SilverSLootboxChanceMultiplier = value2;
                break;
            case LootBoxType.SilverL:
                Settings.SetMinSilverL = num;
                Settings.SetMaxSilverL = num2;
                Settings.SilverLRewardValue = value;
                Settings.SilverLLootboxChanceMultiplier = value2;
                break;
            case LootBoxType.GoldS:
                Settings.SetMinGoldS = num;
                Settings.SetMaxGoldS = num2;
                Settings.GoldSRewardValue = value;
                Settings.GoldSLootboxChanceMultiplier = value2;
                break;
            case LootBoxType.GoldL:
                Settings.SetMinGoldL = num;
                Settings.SetMaxGoldL = num2;
                Settings.GoldLRewardValue = value;
                Settings.GoldLLootboxChanceMultiplier = value2;
                break;
        }

        if (currentVersion != null)
        {
            listing_Standard.Gap();
            GUI.contentColor = Color.gray;
            listing_Standard.Label("LootBoxes_SettingsModVersion".Translate(currentVersion));
            GUI.contentColor = Color.white;
        }

        listing_Standard.End();
        Widgets.EndScrollView();
        Settings.Write();
    }
}