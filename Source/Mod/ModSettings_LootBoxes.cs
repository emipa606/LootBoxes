using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Verse;

namespace Lanilor.LootBoxes.Mod
{
    [UsedImplicitly]
    public class ModSettingsLootBoxes : ModSettings
    {
        public const float GapHeight = 10f;

        public static List<int> HashArchive = new List<int>();

        private static bool _defaultBonusLootChance = true;

        public static bool BonusLootChance = _defaultBonusLootChance;

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
            Scribe_Collections.Look(ref HashArchive, "hashArchive", LookMode.Value);
            Scribe_Values.Look(ref BonusLootChance, "bonusLootChance", _defaultBonusLootChance);
        }
    }
}