using RimWorld;
using Verse;

namespace Lanilor.LootBoxes.DefOfs
{
    [DefOf]
    public static class LootboxDefOf
    {
        static LootboxDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(LootboxDefOf));
        }

        public static ThingDef LootBoxTreasure;
        public static ThingDef LootBoxSilverSmall;
        public static ThingDef LootBoxSilverLarge;
        public static ThingDef LootBoxGoldSmall;
        public static ThingDef LootBoxGoldLarge;
        public static ThingDef LootBoxPandora;
    }
}