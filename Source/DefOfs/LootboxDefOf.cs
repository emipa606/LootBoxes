using JetBrains.Annotations;
using RimWorld;
using Verse;

namespace Lanilor.LootBoxes.DefOfs;

[DefOf]
public static class LootboxDefOf
{
    static LootboxDefOf()
    {
        DefOfHelper.EnsureInitializedInCtor(typeof(LootboxDefOf));
    }

    [UsedImplicitly] public static ThingDef LootBoxTreasure;
    [UsedImplicitly] public static ThingDef LootBoxSilverSmall;
    [UsedImplicitly] public static ThingDef LootBoxSilverLarge;
    [UsedImplicitly] public static ThingDef LootBoxGoldSmall;
    [UsedImplicitly] public static ThingDef LootBoxGoldLarge;
    [UsedImplicitly] public static ThingDef LootBoxPandora;
}