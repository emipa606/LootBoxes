using Lanilor.LootBoxes.Mod;

namespace Lanilor.LootBoxes.Things
{
    public class CompUseEffectLootBoxSilverSmall : CompUseEffectLootBox
    {
        protected override LootBoxType LootBoxType => LootBoxType.SilverS;
        protected override int SetMinimum => ModLootBoxes.Settings?.SetMinSilverS ?? 2;
        protected override int SetMaximum => ModLootBoxes.Settings?.SetMaxSilverS ?? 3;
        protected override float MaximumMarketRewardValue => ModLootBoxes.Settings?.SilverSRewardValue ?? 300;
    }
}