using Lanilor.LootBoxes.Mod;

namespace Lanilor.LootBoxes.Things
{
    public class CompUseEffectLootBoxGoldSmall : CompUseEffectLootBox
    {
        protected override LootBoxType LootBoxType => LootBoxType.GoldS;
        protected override int SetMinimum => ModLootBoxes.Settings?.SetMinGoldS ?? 1;
        protected override int SetMaximum => ModLootBoxes.Settings?.SetMaxGoldS ?? 4;
        protected override float MaximumMarketRewardValue => ModLootBoxes.Settings?.GoldSRewardValue ?? 650;
    }
}