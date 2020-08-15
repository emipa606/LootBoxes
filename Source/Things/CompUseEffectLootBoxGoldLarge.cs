using Lanilor.LootBoxes.Mod;

namespace Lanilor.LootBoxes.Things
{
    public class CompUseEffectLootBoxGoldLarge : CompUseEffectLootBox
    {
        protected override LootBoxType LootBoxType => LootBoxType.GoldL;
        protected override int SetMinimum => ModLootBoxes.Settings?.SetMinGoldL ?? 3;
        protected override int SetMaximum => ModLootBoxes.Settings?.SetMaxGoldL ?? 9;
        protected override float MaximumMarketRewardValue => ModLootBoxes.Settings?.GoldLRewardValue ?? 1000;
    }
}