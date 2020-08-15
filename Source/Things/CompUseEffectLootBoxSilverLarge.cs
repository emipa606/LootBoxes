using Lanilor.LootBoxes.Mod;

namespace Lanilor.LootBoxes.Things
{
    public class CompUseEffectLootBoxSilverLarge : CompUseEffectLootBox
    {
        protected override LootBoxType LootBoxType => LootBoxType.SilverL;
        protected override int SetMinimum => ModLootBoxes.Settings?.SetMinSilverL ?? 1;
        protected override int SetMaximum => ModLootBoxes.Settings?.SetMaxSilverL ?? 12;
        protected override float MaximumMarketRewardValue => ModLootBoxes.Settings?.SilverLRewardValue ?? 600;
    }
}