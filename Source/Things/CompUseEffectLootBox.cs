using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using JetBrains.Annotations;
using Lanilor.LootBoxes.DefOfs;
using Lanilor.LootBoxes.Mod;
using RimWorld;
using Verse;
using Verse.Sound;

namespace Lanilor.LootBoxes.Things
{
    public abstract class CompUseEffectLootBox : CompUseEffect
    {
        protected virtual LootBoxType LootBoxType => LootBoxType.Treasure;

        protected virtual int SetMinimum => 0;

        protected virtual int SetMaximum => 0;

        protected virtual float MaximumMarketRewardValue => 0;

        public override void DoEffect([NotNull] Pawn usedBy)
        {
            base.DoEffect(usedBy);
            SoundDefOf.PsychicPulseGlobal.PlayOneShotOnCamera(usedBy.MapHeld);
            usedBy.records.Increment(LootboxRecordDefOf.LootBoxesOpened);
            OpenBox(usedBy);
        }

        protected virtual void OpenBox([NotNull] Pawn usedBy)
        {
            var c = parent.Position;
            var map = parent.Map;

            Rand.PushState(parent.HashOffset());

            var spawnCount = Rand.RangeInclusive(SetMinimum, Rand.RangeInclusive(SetMinimum, SetMaximum));
            if (ModLootBoxes.Settings.UseBonusLootChance)
                spawnCount = GenMath.RoundRandom(spawnCount * ModLootBoxes.Settings.BonusLootChance);
            spawnCount = Math.Max(1, spawnCount);

#if V10
            var makerParams = default(ThingSetMakerParams);

            makerParams.qualityGenerator = QualityGenerator.Reward;
            makerParams.techLevel = usedBy.Faction.def.techLevel;
            makerParams.totalMarketValueRange = new FloatRange(0.7f, 1.3f) * MaximumMarketRewardValue;
#else
            var generatorParams = default(RewardsGeneratorParams);

            generatorParams.rewardValue = MaximumMarketRewardValue * Find.Storyteller.difficulty.questRewardValueFactor;
            generatorParams.minGeneratedRewardValue = 250f;
            generatorParams.giverFaction = usedBy.Faction;
            generatorParams.populationIntent =
 QuestTuning.PopIncreasingRewardWeightByPopIntentCurve.Evaluate(StorytellerUtilityPopulation.PopulationIntentForQuest);
            generatorParams.allowGoodwill = false;
            generatorParams.allowRoyalFavor = false;
            generatorParams.giveToCaravan = false;
            generatorParams.thingRewardItemsOnly = true;
#endif

            var used = 0;
            do
            {
                var taken = spawnCount - used;
#if V10
                var generatedItems = ThingSetMakerDefOf.Reward_TradeRequest.root.Generate(makerParams);
#else
                var generatedItems =
 RewardsGenerator.Generate(generatorParams).Cast<Reward_Items>().SelectMany(k => k.items);
#endif
                var chosenItems = generatedItems.InRandomOrder().Take(spawnCount - used).ToList();

                foreach (var reward in chosenItems)
                {
                    if (IsLootBox(reward.def, out var type) && !ShouldDrop(LootBoxType, type))
                    {
                        taken--;
                        continue;
                    }

                    GenPlace.TryPlaceThing(reward, c, map, ThingPlaceMode.Near);
                }

                used += Math.Min(taken, chosenItems.Count);
            } while (used < spawnCount);

            Rand.PushState();

            map.wealthWatcher.ForceRecount();
        }

        private static bool ShouldDrop(LootBoxType source, LootBoxType type)
        {
            var multiplier = ModLootBoxes.Settings.TreasureLootboxChanceMultiplier;
            var chance = ModLootBoxes.Settings.ChanceForTreasure;

            switch (source)
            {
                case LootBoxType.SilverS:
                    multiplier = ModLootBoxes.Settings.SilverSLootboxChanceMultiplier;
                    break;
                case LootBoxType.SilverL:
                    multiplier = ModLootBoxes.Settings.SilverLLootboxChanceMultiplier;
                    break;
                case LootBoxType.GoldS:
                    multiplier = ModLootBoxes.Settings.GoldSLootboxChanceMultiplier;
                    break;
                case LootBoxType.GoldL:
                    multiplier = ModLootBoxes.Settings.GoldLLootboxChanceMultiplier;
                    break;
            }

            switch (type)
            {
                case LootBoxType.SilverS:
                    chance = ModLootBoxes.Settings.ChanceForSilverS;
                    break;
                case LootBoxType.SilverL:
                    chance = ModLootBoxes.Settings.ChanceForSilverL;
                    break;
                case LootBoxType.GoldS:
                    chance = ModLootBoxes.Settings.ChanceForGoldS;
                    break;
                case LootBoxType.GoldL:
                    chance = ModLootBoxes.Settings.ChanceForGoldL;
                    break;
                case LootBoxType.Pandora:
                    chance = ModLootBoxes.Settings.ChanceForPandora;
                    break;
            }

            return Rand.Value <= chance * multiplier / 100;
        }

        private static bool IsLootBox(ThingDef def, out LootBoxType type)
        {
            type = LootBoxType.Treasure;

            if (def == LootboxDefOf.LootBoxTreasure)
                return true;

            if (def == LootboxDefOf.LootBoxSilverSmall)
            {
                type = LootBoxType.SilverS;
                return true;
            }

            if (def == LootboxDefOf.LootBoxSilverLarge)
            {
                type = LootBoxType.SilverL;
                return true;
            }

            if (def == LootboxDefOf.LootBoxGoldSmall)
            {
                type = LootBoxType.GoldS;
                return true;
            }

            if (def == LootboxDefOf.LootBoxGoldLarge)
            {
                type = LootBoxType.GoldL;
                return true;
            }

            if (def != LootboxDefOf.LootBoxPandora) return false;

            type = LootBoxType.Pandora;
            return true;
        }

        [NotNull]
        public override string CompInspectStringExtra()
        {
            return " \n" + parent.DescriptionFlavor;
        }
    }
}