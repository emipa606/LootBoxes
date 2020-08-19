using Lanilor.LootBoxes.Mod;
using System.Collections.Generic;

namespace Lanilor.LootBoxes.Things
{
    public class CompUseEffectLootBoxTreasure : CompUseEffectLootBox
    {
        protected override LootBoxType LootBoxType => LootBoxType.Treasure;
        protected override int SetMinimum => ModLootBoxes.Settings?.SetMinTreasure ?? 1;
        protected override int SetMaximum => ModLootBoxes.Settings?.SetMaxTreasure ?? 3;
        protected override float MaximumMarketRewardValue => ModLootBoxes.Settings?.TreasureRewardValue ?? 150;

        protected override IEnumerable<LootboxReward> RewardTable => new List<LootboxReward>
        {
            new LootboxReward(7, new Reward("Gold", 3, 6, 17)),
            new LootboxReward(5, new Reward("Gold", 7, 15, 35)),
            new LootboxReward(4, new Reward("Silver", 50, 90, 600)),
            new LootboxReward(4, new Reward("Jade", 7, 13, 30)),
            new LootboxReward(4, new Reward("LootBoxSilverSmall")),
            new LootboxReward(3, new Reward("ElephantTusk", rand: 3)),
            new LootboxReward(3, new Reward("Gold", 5, 10, 25), new Reward("Jade", 5, 10, 25)),
            new LootboxReward(3, new Reward("Hyperweave", 4, 7, 20)),
            new LootboxReward(2, new Reward("LootBoxTreasure")),
            new LootboxReward(2, new Reward("Silver", 40, 80, 220), new Reward("Gold", 8, 15, 40)),
            new LootboxReward(2, new Reward("LootBoxPandora")),
            new LootboxReward(1, new Reward("Gold", 10, 20, 140)),
            new LootboxReward(1, new Reward("LootBoxGoldSmall")),
            new LootboxReward(1, new Reward("ThrumboHorn"))
        };
    }
}