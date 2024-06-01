using HarmonyLib;
using Lanilor.LootBoxes.DefOfs;
using RimWorld;
using Verse;

namespace Lanilor.LootBoxes.HarmonyPatches;

[HarmonyPatch(typeof(ThingSetMaker_MapGen_AncientPodContents), "GiveRandomLootInventoryForTombPawn")]
public class ThingSetMaker_MapGen_AncientPodContents_GiveRandomLootInventoryForTombPawn
{
    public static void Postfix(Pawn p)
    {
        var value = Rand.Value;
        ThingDef thingDef;
        switch (value)
        {
            case < 0.4f and < 0.1f:
                thingDef = LootboxDefOf.LootBoxTreasure;
                break;
            case < 0.4f and < 0.35f:
                thingDef = LootboxDefOf.LootBoxSilverSmall;
                break;
            case < 0.4f:
                thingDef = LootboxDefOf.LootBoxGoldSmall;
                break;
            case < 0.5f:
                thingDef = LootboxDefOf.LootBoxPandora;
                break;
            default:
                thingDef = null;
                break;
        }

        if (thingDef == null)
        {
            return;
        }

        var thing = ThingMaker.MakeThing(thingDef);
        thing.stackCount = 1;
        p.inventory.innerContainer.TryAdd(thing);
    }
}