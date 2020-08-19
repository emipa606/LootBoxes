using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Lanilor.LootBoxes.Things
{
    public class LootboxReward
    {
        public float Weight;

        public List<Reward> Rewards;

        public LootboxReward(float weight, [NotNull] params Reward[] rewards)
        {
            Weight = weight;
            Rewards = rewards.ToList();
        }
    }
}