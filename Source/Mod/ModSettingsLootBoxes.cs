using UnityEngine;
using Verse;

namespace Lanilor.LootBoxes.Mod
{
    public class ModSettingsLootBoxes : ModSettings
    {
        public const float GapHeight = 10f;

        private const bool DefaultBonusLootChance = true;

        public static bool BonusLootChance = DefaultBonusLootChance;

        public void DoWindowContents(Rect rect)
        {
            var ls = new Listing_Standard();
            ls.Begin(rect);
            ls.Gap(GapHeight);

            ls.CheckboxLabeled("LootBoxes_SettingsBonusLootChance".Translate(), ref BonusLootChance,
                "LootBoxes_SettingsBonusLootChanceDesc".Translate());
            ls.Gap(GapHeight);

            ls.End();
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref BonusLootChance, "bonusLootChance", DefaultBonusLootChance);
        }
    }
}