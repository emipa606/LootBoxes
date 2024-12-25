using System;
using System.Collections.Generic;
using System.Linq;
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

    protected virtual float MaximumMarketRewardValue => 0f;

    protected virtual IEnumerable<LootboxReward> RewardTable => new List<LootboxReward>();

    public override void DoEffect(Pawn usedBy)
    {
        base.DoEffect(usedBy);
        LootboxDefOf.LootBoxOpening.PlayOneShotOnCamera(usedBy.MapHeld);
        usedBy.records.Increment(LootboxDefOf.LootBoxesOpened);
        OpenBox(usedBy);
    }

    public override IEnumerable<Gizmo> CompGetGizmosExtra()
    {
        foreach (var gizmo in base.CompGetGizmosExtra())
        {
            yield return gizmo;
        }

        if (!Prefs.DevMode)
        {
            yield break;
        }

        yield return new Command_Action
        {
            defaultLabel = "Dev: Open",
            action = delegate
            {
                OpenBox(Faction.OfPlayer);
                parent.Destroy();
            }
        };
    }

    protected virtual void OpenBox(Pawn usedBy)
    {
        OpenBox(usedBy.Faction);
    }

    protected virtual void OpenBox(Faction faction)
    {
        var position = parent.Position;
        var map = parent.Map;
        if (ModLootBoxes.Settings.UseIngameRewardsGenerator)
        {
            Rand.PushState(parent.HashOffset());
            var num = Rand.RangeInclusive(SetMinimum, Rand.RangeInclusive(SetMinimum, SetMaximum));
            if (ModLootBoxes.Settings.UseBonusLootChance)
            {
                num = GenMath.RoundRandom(num * ModLootBoxes.Settings.BonusLootChance);
            }

            num = Math.Max(1, num);
            var rewardsGeneratorParams = default(RewardsGeneratorParams);
            rewardsGeneratorParams.rewardValue =
                MaximumMarketRewardValue * Find.Storyteller.difficulty.questRewardValueFactor;
            rewardsGeneratorParams.minGeneratedRewardValue = 250f;
            rewardsGeneratorParams.giverFaction = faction;
            rewardsGeneratorParams.populationIntent =
                QuestTuning.PopIncreasingRewardWeightByPopIntentCurve.Evaluate(StorytellerUtilityPopulation
                    .PopulationIntent);
            rewardsGeneratorParams.allowGoodwill = false;
            rewardsGeneratorParams.allowRoyalFavor = false;
            rewardsGeneratorParams.giveToCaravan = false;
            rewardsGeneratorParams.thingRewardItemsOnly = true;
            var parms = rewardsGeneratorParams;
            var num2 = 0;
            do
            {
                var num3 = num - num2;
                var list = new List<Thing>();
                var array = RewardsGenerator.Generate(parms).Cast<Reward_Items>().SelectMany(k => k.items)
                    .ToArray();
                for (var i = 0; i < array.Length; i++)
                {
                    var thing = array[i];
                    if (thing.def == ThingDefOf.PsychicAmplifier &&
                        (!ModLootBoxes.Settings.AllowPsychicAmplifierSpawn || list.Exists(k => k.def == thing.def)))
                    {
                        parms.disallowedThingDefs = [ThingDefOf.PsychicAmplifier];
                        array = RewardsGenerator.Generate(parms).Cast<Reward_Items>().SelectMany(k => k.items)
                            .ToArray();
                        i = 0;
                    }
                    else
                    {
                        list.Add(thing);
                    }
                }

                var list2 = list.InRandomOrder().Take(num - num2).ToList();
                foreach (var item in list2)
                {
                    if (IsLootBox(item.def, out var type) && !ShouldDrop(LootBoxType, type))
                    {
                        num3--;
                        continue;
                    }

                    GenPlace.TryPlaceThing(item, position, map, ThingPlaceMode.Near);
                    if (item.def == ThingDefOf.PsychicAmplifier)
                    {
                        Find.History.Notify_PsylinkAvailable();
                    }
                }

                num2 += Math.Min(num3, list2.Count);
            } while (num2 < num);

            Rand.PopState();
        }
        else
        {
            foreach (var item2 in CreateLoot())
            {
                GenPlace.TryPlaceThing(item2, position, map, ThingPlaceMode.Near);
            }
        }

        map.wealthWatcher.ForceRecount();
    }


    private IEnumerable<Thing> CreateLoot()
    {
        var list = new List<Thing>();
        var num = Rand.RangeInclusive(SetMinimum, Rand.RangeInclusive(SetMinimum, SetMaximum));
        if (ModLootBoxes.Settings.UseBonusLootChance)
        {
            num = GenMath.RoundRandom(num * ModLootBoxes.Settings.BonusLootChance);
        }

        num = Math.Max(1, num);
        while (num > 0)
        {
            num--;
            foreach (var item in (from k in RewardTable.RandomElementByWeight(k => k.Weight * k.Weight).Rewards
                         group k by DefDatabase<ThingDef>.GetNamed(k.ItemDefName)
                         into k
                         where k.Key != null
                         select k).ToList())
            {
                var key = item.Key;
                foreach (var item2 in item)
                {
                    var num2 = Math.Max(item2.MinimumDropCount, 1);
                    var num3 = Math.Max(item2.MaximumDropCount, num2);
                    if (item2.RandomizeDropCountUpTo > num3)
                    {
                        num3 = Rand.RangeInclusive(num3, item2.RandomizeDropCountUpTo);
                    }

                    var num4 = Rand.RangeInclusive(num2, num3);
                    while (num4 > 0)
                    {
                        var num5 = Math.Min(num4, key.stackLimit);
                        num4 -= num5;
                        var thing = ThingMaker.MakeThing(key, GenStuff.RandomStuffFor(key));
                        thing.stackCount = num5;
                        var compQuality = thing.TryGetComp<CompQuality>();
                        if (compQuality != null)
                        {
                            var qualityCategory =
                                QualityUtility.GenerateQualityCreatedByPawn(Rand.RangeInclusive(0, 19), false);
                            compQuality.SetQuality(qualityCategory, ArtGenerationContext.Outsider);
                        }

                        thing.TryGetComp<CompArt>()?.InitializeArt(ArtGenerationContext.Outsider);
                        list.Add(thing);
                    }
                }
            }
        }

        return list;
    }

    private static bool ShouldDrop(LootBoxType source, LootBoxType type)
    {
        var num = source switch
        {
            LootBoxType.SilverS => ModLootBoxes.Settings.SilverSLootboxChanceMultiplier,
            LootBoxType.SilverL => ModLootBoxes.Settings.SilverLLootboxChanceMultiplier,
            LootBoxType.GoldS => ModLootBoxes.Settings.GoldSLootboxChanceMultiplier,
            LootBoxType.GoldL => ModLootBoxes.Settings.GoldLLootboxChanceMultiplier,
            _ => ModLootBoxes.Settings.TreasureLootboxChanceMultiplier
        };
        var num2 = type switch
        {
            LootBoxType.SilverS => ModLootBoxes.Settings.ChanceForSilverS,
            LootBoxType.SilverL => ModLootBoxes.Settings.ChanceForSilverL,
            LootBoxType.GoldS => ModLootBoxes.Settings.ChanceForGoldS,
            LootBoxType.GoldL => ModLootBoxes.Settings.ChanceForGoldL,
            LootBoxType.Pandora => ModLootBoxes.Settings.ChanceForPandora,
            _ => ModLootBoxes.Settings.ChanceForTreasure
        };
        return Rand.Value <= num2 * num / 100f;
    }

    private static bool IsLootBox(ThingDef def, out LootBoxType type)
    {
        type = LootBoxType.Treasure;
        if (def == LootboxDefOf.LootBoxTreasure)
        {
            return true;
        }

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

        if (def != LootboxDefOf.LootBoxPandora)
        {
            return false;
        }

        type = LootBoxType.Pandora;
        return true;
    }


    public override string CompInspectStringExtra()
    {
        return $" \n{parent.DescriptionFlavor}";
    }
}