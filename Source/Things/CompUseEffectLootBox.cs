using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Lanilor.LootBoxes.Mod;
using RimWorld;
using Verse;
using Verse.Sound;
using RecordDefOf = Lanilor.LootBoxes.DefOfs.RecordDefOf;

namespace Lanilor.LootBoxes.Things
{
    public abstract class CompUseEffectLootBox : CompUseEffect
    {
        [NotNull] public virtual IEnumerable<LootboxReward> RewardTable => new List<LootboxReward>();

        public virtual int SetMinimum => 0;

        public virtual int SetMaximum => 0;

        public override void DoEffect([NotNull] Pawn usedBy)
        {
            base.DoEffect(usedBy);
            SoundDefOf.PsychicPulseGlobal.PlayOneShotOnCamera(usedBy.MapHeld);
            usedBy.records.Increment(RecordDefOf.LootBoxesOpened);
            OpenBox(usedBy);
        }

        protected virtual void OpenBox(Pawn usedBy)
        {
            var c = parent.Position;
            var map = parent.Map;
            map.wealthWatcher.ForceRecount();

            var lootList = CreateLoot();
            foreach (var t in lootList) GenPlace.TryPlaceThing(t, c, map, ThingPlaceMode.Near);
        }

        [NotNull]
        private IEnumerable<Thing> CreateLoot()
        {
            var lootList = new List<Thing>();
            // RNG Here will be biased towards the SetMinimum, so it'll be more likely to get closer to SetMinimum than getting to SetMaximum.
            var spawnCount = Rand.RangeInclusive(SetMinimum, Rand.RangeInclusive(SetMinimum, SetMaximum));

            /*
             * Bonus Loot chance by default was multiplied by 2 the reward count, but it also would reduce
             * the chances by a factor of 2 if a bonus was already applied to the exact same box with the same
             * hash ID (so if you were to reload the world without saving, the same box would be there and chances
             * would be reduced instead of increased). This code just says screw that and always increases it as 
             * long as the config option is enabled.
             */
            if (ModLootBoxes.Settings.UseBonusLootChance)
                spawnCount = GenMath.RoundRandom(spawnCount * ModLootBoxes.Settings.BonusLootChance);

            spawnCount = Math.Max(1, spawnCount);

            while (spawnCount > 0)
            {
                spawnCount--;
                var chosenReward = RewardTable.RandomElementByWeight(k => k.Weight * k.Weight);
                var groupedReward = chosenReward.Rewards.GroupBy(k => DefDatabase<ThingDef>.GetNamed(k.ItemDefName))
                    .Where(k => k.Key != null).ToList();

                foreach (var group in groupedReward)
                {
                    var targetDef = group.Key;
                    foreach (var reward in group)
                    {
                        var min = Math.Max(reward.MinimumDropCount, 1);
                        var max = Math.Max(reward.MaximumDropCount, min);
                        if (reward.RandomizeDropCountUpTo > max)
                            max = Rand.RangeInclusive(max, reward.RandomizeDropCountUpTo);

                        var countToDo = Rand.RangeInclusive(min, max);
                        while (countToDo > 0)
                        {
                            var count = Math.Min(countToDo, targetDef.stackLimit);
                            countToDo -= count;

                            var thing = ThingMaker.MakeThing(targetDef, GenStuff.RandomStuffFor(targetDef));
                            thing.stackCount = count;

                            var compQuality = thing.TryGetComp<CompQuality>();
                            if (compQuality != null)
                            {
                                var q = QualityUtility.GenerateQualityCreatedByPawn(Rand.RangeInclusive(0, 19), false);
                                compQuality.SetQuality(q, ArtGenerationContext.Outsider);
                            }

                            var compArt = thing.TryGetComp<CompArt>();
                            compArt?.InitializeArt(ArtGenerationContext.Outsider);
                            lootList.Add(thing);
                        }
                    }
                }
            }

            return lootList;
        }

        [NotNull]
        public override string CompInspectStringExtra()
        {
            return " \n" + parent.DescriptionFlavor;
        }
    }
}