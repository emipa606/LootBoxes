using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using RimWorld;
using Verse;

namespace Lanilor.LootBoxes.Things;

[UsedImplicitly]
public class CompUseEffectLootBoxPandora : CompUseEffectLootBox
{
    private static readonly Dictionary<List<IncidentCategoryDef>, IncidentGroupData> IncidentGroups =
        new()
        {
            {
                new List<IncidentCategoryDef>
                {
                    IncidentCategoryDefOf.ShipChunkDrop, IncidentCategoryDefOf.OrbitalVisitor,
                    IncidentCategoryDefOf.FactionArrival, IncidentCategoryDefOf.Misc
                },
                new IncidentGroupData(0.30f, 1, 3, 6)
            }, // Good and neutral
            {
                new List<IncidentCategoryDef>
                {
                    IncidentCategoryDefOf.ShipChunkDrop, IncidentCategoryDefOf.OrbitalVisitor,
                    IncidentCategoryDefOf.ThreatSmall, IncidentCategoryDefOf.Misc,
                    IncidentCategoryDefOf.DiseaseHuman, IncidentCategoryDefOf.DiseaseAnimal
                },
                new IncidentGroupData(0.20f, 2, 5, 9)
            }, // Neutral to slightly bad
            {
                new List<IncidentCategoryDef>
                {
                    IncidentCategoryDefOf.ShipChunkDrop, IncidentCategoryDefOf.ThreatBig,
                    IncidentCategoryDefOf.ThreatSmall, IncidentCategoryDefOf.Misc,
                    IncidentCategoryDefOf.DiseaseHuman, IncidentCategoryDefOf.DiseaseAnimal
                },
                new IncidentGroupData(0.50f, 1, 3, 7)
            } // Bad and neutral
        };

    protected override void OpenBox(Pawn usedBy)
    {
        var map = usedBy.Map;
        Rand.PushState(parent.HashOffset());

        var hostileActivity = GenHostility.AnyHostileActiveThreatToPlayer(map);

        var selectedGroup = IncidentGroups
            .Where(k => !hostileActivity || !k.Key.Contains(IncidentCategoryDefOf.FactionArrival))
            .RandomElementByWeight(kvp => kvp.Value.Chance);

        var numberToFire = Rand.RangeInclusive(selectedGroup.Value.CountMin,
            Rand.RangeInclusive(selectedGroup.Value.CountStep, selectedGroup.Value.CountMax));

        var validIncidents =
            (from incident in from def in DefDatabase<IncidentDef>.AllDefs
                    where selectedGroup.Key.Contains(def.category) && def.TargetAllowed(map)
                    select def
                let parameters = StorytellerUtility.DefaultParmsNow(incident.category, map)
                where incident.Worker.CanFireNow(parameters)
                select incident).ToList();

        for (; numberToFire > 0; numberToFire--)
        {
            var selectedIncident =
                validIncidents.RandomElementByWeight(IncidentChanceFinal);
            if (selectedIncident == null) continue;

            var storyTellerParams = StorytellerUtility.DefaultParmsNow(selectedIncident.category, map);
            if (selectedIncident.pointsScaleable)
            {
                var storytellerComp = Find.Storyteller.storytellerComps.FirstOrDefault(x =>
                    x is StorytellerComp_OnOffCycle or StorytellerComp_RandomMain);
                if (storytellerComp != null)
                    storyTellerParams =
                        storytellerComp.GenerateParms(selectedIncident.category, storyTellerParams.target);
            }

            selectedIncident.Worker.TryExecute(storyTellerParams);
        }

        Rand.PopState();
    }

    private static float IncidentChanceFinal([NotNull] IncidentDef def)
    {
#if V10
        var chance = def.Worker.AdjustedChance;
#else
        var chance = def.Worker.BaseChanceThisGame;
#endif
        chance *= IncidentChanceFactor_CurrentPopulation(def);
        return Math.Max(0f, chance);
    }

    private static float IncidentChanceFactor_CurrentPopulation([NotNull] IncidentDef def)
    {
        if (def.chanceFactorByPopulationCurve == null) return 1f;

        var pawns = PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_Colonists;
#if V10
        var pawnCount = pawns.Count();
#else
        var pawnCount = pawns.Count;
#endif
        return def.chanceFactorByPopulationCurve.Evaluate(pawnCount);
    }

    private readonly struct IncidentGroupData
    {
        public float Chance { get; }
        public int CountMin { get; }
        public int CountStep { get; }
        public int CountMax { get; }

        public IncidentGroupData(float chance, int countMin, int countStep, int countMax)
        {
            Chance = chance;
            CountMin = countMin;
            CountStep = countStep;
            CountMax = countMax;
        }
    }
}