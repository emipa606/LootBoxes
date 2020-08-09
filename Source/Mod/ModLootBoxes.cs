using System.Reflection;
using JetBrains.Annotations;
using UnityEngine;
using Verse;

namespace Lanilor.LootBoxes.Mod
{
    public class ModLootBoxes : Verse.Mod
    {
        public static ModSettingsLootBoxes Settings;

        public ModLootBoxes(ModContentPack content) : base(content)
        {
            Settings = GetSettings<ModSettingsLootBoxes>();

            var harmony = new HarmonyLib.Harmony("rimworld.lanilor.lootboxes");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

        [NotNull]
        public override string SettingsCategory()
        {
            return "LootBoxes_SettingsCategory".Translate();
        }

        public override void DoSettingsWindowContents(Rect rect)
        {
            var ls = new Listing_Standard();
            var gap = Listing.DefaultGap;

            ls.Begin(rect);
            ls.Gap(gap);

            ls.CheckboxLabeled("LootBoxes_SettingsUseBonusLootChance".Translate(), ref Settings.UseBonusLootChance,
                "LootBoxes_SettingsUseBonusLootChanceDesc".Translate());
            ls.Gap(gap);

            ls.Label("LootBoxes_SettingsBonusLootChance".Translate() + Settings.BonusLootChance);
            Settings.BonusLootChance = Widgets.HorizontalSlider(ls.GetRect(20f), Settings.BonusLootChance, 1, 5, roundTo: 0.1f);
            ls.GapLine(gap);

            ls.End();

            Settings.Write();
        }
    }
}