using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
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

            return lootList;
        }

        [NotNull]
        public override string CompInspectStringExtra()
        {
            return " \n" + parent.DescriptionFlavor;
        }
    }
}