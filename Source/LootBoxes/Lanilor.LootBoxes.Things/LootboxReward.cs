using System.Collections.Generic;
using System.Linq;

namespace Lanilor.LootBoxes.Things;

public class LootboxReward(float weight, params Reward[] rewards)
{
    public readonly List<Reward> Rewards = rewards.ToList();
    public readonly float Weight = weight;
}