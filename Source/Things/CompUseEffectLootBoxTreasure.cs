using Lanilor.LootBoxes.Mod;

namespace Lanilor.LootBoxes.Things
{
    public class CompUseEffectLootBoxTreasure : CompUseEffectLootBox
    {
        protected override LootBoxType LootBoxType => LootBoxType.Treasure;
        protected override int SetMinimum => ModLootBoxes.Settings?.SetMinTreasure ?? 1;
        protected override int SetMaximum => ModLootBoxes.Settings?.SetMaxTreasure ?? 3;
        protected override float MaximumMarketRewardValue => ModLootBoxes.Settings?.TreasureRewardValue ?? 150;
    }
}