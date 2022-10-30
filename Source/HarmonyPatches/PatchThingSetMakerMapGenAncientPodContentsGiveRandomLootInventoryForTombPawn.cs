using JetBrains.Annotations;
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
    [UsedImplicitly]
    public class PatchThingSetMakerMapGenAncientPodContentsGiveRandomLootInventoryForTombPawn
    {
        [HarmonyPostfix]
        [UsedImplicitly]
        public static void Postfix(Pawn p)
        {
            var random = Rand.Value;
            var lootToAdd = random switch
            {
                < 0.10f => LootboxDefOf.LootBoxTreasure,
                < 0.35f => LootboxDefOf.LootBoxSilverSmall,
                < 0.40f => LootboxDefOf.LootBoxGoldSmall,
                < 0.50f => LootboxDefOf.LootBoxPandora,
                _ => null
            };

            // Create and add loot if needed
            if (lootToAdd == null) return;

            var thing = ThingMaker.MakeThing(lootToAdd);
            thing.stackCount = 1;
            p.inventory.innerContainer.TryAdd(thing);
        }
    }
}