using HarmonyLib;
using RimWorld;
using Verse;
using ThingDefOf = Lanilor.LootBoxes.DefOfs.ThingDefOf;

namespace Lanilor.LootBoxes.HarmonyPatches
{
    [HarmonyPatch(typeof(ThingSetMaker_MapGen_AncientPodContents))]
    [HarmonyPatch("GiveRandomLootInventoryForTombPawn")]
    public class PatchThingSetMakerMapGenAncientPodContentsGiveRandomLootInventoryForTombPawn
    {
        [HarmonyPostfix]
        public static void Postfix(Pawn p)
        {
            ThingDef lootToAdd = null;
            var random = Rand.Value;
            if (random < 0.10f)
                lootToAdd = ThingDefOf.LootBoxTreasure;
            else if (random < 0.35f)
                lootToAdd = ThingDefOf.LootBoxSilverSmall;
            else if (random < 0.40f)
                lootToAdd = ThingDefOf.LootBoxGoldSmall;
            else if (random < 0.50f) lootToAdd = ThingDefOf.LootBoxPandora;

            // Create and add loot if needed
            if (lootToAdd == null) return;

            var thing = ThingMaker.MakeThing(lootToAdd);
            thing.stackCount = 1;
            p.inventory.innerContainer.TryAdd(thing);
        }
    }
}