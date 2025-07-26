using System.Collections.Generic;
using Lanilor.LootBoxes.Mod;

namespace Lanilor.LootBoxes.Things;

public class CompUseEffectLootBoxTreasure : CompUseEffectLootBox
{
    protected override LootBoxType LootBoxType => LootBoxType.Treasure;

    protected override int SetMinimum => ModLootBoxes.Settings?.SetMinTreasure ?? 1;

    protected override int SetMaximum => ModLootBoxes.Settings?.SetMaxTreasure ?? 3;

    protected override float MaximumMarketRewardValue => ModLootBoxes.Settings?.TreasureRewardValue ?? 150f;

    protected override IEnumerable<LootboxReward> RewardTable
    {
        get
        {
            var list = new List<LootboxReward>
            {
                new(7f, new Reward("Gold", 3, 6, 17)),
                new(5f, new Reward("Gold", 7, 15, 35)),
                new(4f, new Reward("Silver", 50, 90, 600)),
                new(4f, new Reward("Jade", 7, 13, 30)),
                new(4f, new Reward("LootBoxSilverSmall")),
                new(3f, new Reward("ElephantTusk", 1, 1, 3)),
                new(3f, new Reward("Gold", 5, 10, 25), new Reward("Jade", 5, 10, 25)),
                new(3f, new Reward("Hyperweave", 4, 7, 20)),
                new(2f, new Reward("LootBoxTreasure")),
                new(2f, new Reward("Silver", 40, 80, 220), new Reward("Gold", 8, 15, 40)),
                new(2f, new Reward("LootBoxPandora")),
                new(1f, new Reward("Gold", 10, 20, 140)),
                new(1f, new Reward("LootBoxGoldSmall")),
                new(1f, new Reward("ThrumboHorn"))
            };
            return list;
        }
    }
}