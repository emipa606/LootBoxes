using UnityEngine;
using Verse;

namespace Lanilor.LootBoxes.Mod
{
    public class ModSettingsLootBoxes : ModSettings
    {

        public bool UseBonusLootChance = true;

        public float BonusLootChance = 1.5f;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref BonusLootChance, "BonusLootChance", 1.5f, true);
            Scribe_Values.Look(ref UseBonusLootChance, "UseBonusLootChance", true, true);
        }
    }
}