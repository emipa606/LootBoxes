using Lanilor.LootBoxes.DefOfs;
using RimWorld;
using Verse;
#if V10
using Harmony;

#else
using HarmonyLib;
#endif

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
                lootToAdd = LootboxDefOf.LootBoxTreasure;
            else if (random < 0.35f)
                lootToAdd = LootboxDefOf.LootBoxSilverSmall;
            else if (random < 0.40f)
                lootToAdd = LootboxDefOf.LootBoxGoldSmall;
            else if (random < 0.50f)
                lootToAdd = LootboxDefOf.LootBoxPandora;

            // Create and add loot if needed
            if (lootToAdd == null) return;

            var thing = ThingMaker.MakeThing(lootToAdd);
            thing.stackCount = 1;
            p.inventory.innerContainer.TryAdd(thing);
        }
    }
}