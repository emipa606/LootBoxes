using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Lanilor.LootBoxes.DefOfs;
using Lanilor.LootBoxes.Mod;
using RimWorld;
using Verse;
using Verse.Sound;

namespace Lanilor.LootBoxes.Things;

public abstract class CompUseEffectLootBox : CompUseEffect
{
    protected virtual LootBoxType LootBoxType => LootBoxType.Treasure;

    protected virtual int SetMinimum => 0;

    protected virtual int SetMaximum => 0;

    protected virtual float MaximumMarketRewardValue => 0;

    [NotNull] protected virtual IEnumerable<LootboxReward> RewardTable => new List<LootboxReward>();

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

        if (ModLootBoxes.Settings.UseIngameRewardsGenerator)
        {
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
            var generatorParams = new RewardsGeneratorParams
            {
                rewardValue = MaximumMarketRewardValue * Find.Storyteller.difficulty.questRewardValueFactor,
                minGeneratedRewardValue = 250f,
                giverFaction = usedBy.Faction,
                populationIntent =
                    QuestTuning.PopIncreasingRewardWeightByPopIntentCurve.Evaluate(StorytellerUtilityPopulation.PopulationIntent),
                allowGoodwill = false,
                allowRoyalFavor = false,
                giveToCaravan = false,
                thingRewardItemsOnly = true
            };
#endif

            var used = 0;
            do
            {
                var taken = spawnCount - used;
#if V10
                var generatedItems = ThingSetMakerDefOf.Reward_TradeRequest.root.Generate(makerParams);
#else
                var generatedItems = new List<Thing>();
                var firstGen =
                    RewardsGenerator.Generate(generatorParams).Cast<Reward_Items>().SelectMany(k => k.items).ToArray();

                for (var i = 0; i < firstGen.Length; i++)
                {
                    var thing = firstGen[i];
                    if (thing.def == ThingDefOf.PsychicAmplifier && (!ModLootBoxes.Settings.AllowPsychicAmplifierSpawn || generatedItems.Exists(k => k.def == thing.def)))
                    {
                        generatorParams.disallowedThingDefs = new List<ThingDef> { ThingDefOf.PsychicAmplifier };
                        firstGen =
                            RewardsGenerator.Generate(generatorParams).Cast<Reward_Items>().SelectMany(k => k.items).ToArray();
                        i = 0;
                        continue;
                    }

                    generatedItems.Add(thing);
                }
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
#if !V10
                    if (reward.def == ThingDefOf.PsychicAmplifier)
                        Find.History.Notify_PsylinkAvailable();
#endif
                }

                used += Math.Min(taken, chosenItems.Count);
            } while (used < spawnCount);

            Rand.PopState();
        }
        else
        {
            foreach (var t in CreateLoot())
                GenPlace.TryPlaceThing(t, c, map, ThingPlaceMode.Near);
        }

        map.wealthWatcher.ForceRecount();
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

    private static bool ShouldDrop(LootBoxType source, LootBoxType type)
    {
        var multiplier = source switch
        {
            LootBoxType.SilverS => ModLootBoxes.Settings.SilverSLootboxChanceMultiplier,
            LootBoxType.SilverL => ModLootBoxes.Settings.SilverLLootboxChanceMultiplier,
            LootBoxType.GoldS => ModLootBoxes.Settings.GoldSLootboxChanceMultiplier,
            LootBoxType.GoldL => ModLootBoxes.Settings.GoldLLootboxChanceMultiplier,
            _ => ModLootBoxes.Settings.TreasureLootboxChanceMultiplier
        };

        var chance = type switch
        {
            LootBoxType.SilverS => ModLootBoxes.Settings.ChanceForSilverS,
            LootBoxType.SilverL => ModLootBoxes.Settings.ChanceForSilverL,
            LootBoxType.GoldS => ModLootBoxes.Settings.ChanceForGoldS,
            LootBoxType.GoldL => ModLootBoxes.Settings.ChanceForGoldL,
            LootBoxType.Pandora => ModLootBoxes.Settings.ChanceForPandora,
            _ => ModLootBoxes.Settings.ChanceForTreasure
        };

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