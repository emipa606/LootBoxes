using System;
using System.Collections.Generic;
using System.Linq;
using Lanilor.LootBoxes.DefOfs;
using RimWorld;
using Verse;

namespace Lanilor.LootBoxes.Things;

public class CompUseEffectLootBoxPandora : CompUseEffectLootBox
{
    private static readonly Dictionary<List<IncidentCategoryDef>, IncidentGroupData> IncidentGroups =
        new Dictionary<List<IncidentCategoryDef>, IncidentGroupData>
        {
            {
                [
                    LootboxDefOf.ShipChunkDrop,
                    LootboxDefOf.OrbitalVisitor,
                    LootboxDefOf.FactionArrival,
                    IncidentCategoryDefOf.Misc
                ],
                new IncidentGroupData(0.3f, 1, 3, 6)
            },
            {
                [
                    LootboxDefOf.ShipChunkDrop,
                    LootboxDefOf.OrbitalVisitor,
                    IncidentCategoryDefOf.ThreatSmall,
                    IncidentCategoryDefOf.Misc,
                    IncidentCategoryDefOf.DiseaseHuman,
                    LootboxDefOf.DiseaseAnimal
                ],
                new IncidentGroupData(0.2f, 2, 5, 9)
            },
            {
                [
                    LootboxDefOf.ShipChunkDrop,
                    IncidentCategoryDefOf.ThreatBig,
                    IncidentCategoryDefOf.ThreatSmall,
                    IncidentCategoryDefOf.Misc,
                    IncidentCategoryDefOf.DiseaseHuman,
                    LootboxDefOf.DiseaseAnimal
                ],
                new IncidentGroupData(0.5f, 1, 3, 7)
            }
        };

    protected override void OpenBox(Faction faction)
    {
        var map = parent.Map;
        Rand.PushState(parent.HashOffset());
        var hostileActivity = GenHostility.AnyHostileActiveThreatToPlayer(map);
        var selectedGroup = IncidentGroups.Where(k => !hostileActivity || !k.Key.Contains(LootboxDefOf.FactionArrival))
            .RandomElementByWeight(kvp => kvp.Value.Chance);
        var num = Rand.RangeInclusive(selectedGroup.Value.CountMin,
            Rand.RangeInclusive(selectedGroup.Value.CountStep, selectedGroup.Value.CountMax));
        var source = (from def in DefDatabase<IncidentDef>.AllDefs
            where selectedGroup.Key.Contains(def.category) && def.TargetAllowed(map)
            select def
            into incident
            let parameters = StorytellerUtility.DefaultParmsNow(incident.category, map)
            where incident.Worker.CanFireNow(parameters)
            select incident).ToList();
        while (num > 0)
        {
            var incidentDef = source.RandomElementByWeight(IncidentChanceFinal);
            if (incidentDef != null)
            {
                var incidentParms = StorytellerUtility.DefaultParmsNow(incidentDef.category, map);
                if (incidentDef.pointsScaleable)
                {
                    var storytellerComp = Find.Storyteller.storytellerComps.FirstOrDefault(x =>
                        x is StorytellerComp_OnOffCycle || x is StorytellerComp_RandomMain);
                    if (storytellerComp != null)
                    {
                        incidentParms = storytellerComp.GenerateParms(incidentDef.category, incidentParms.target);
                    }
                }

                incidentDef.Worker.TryExecute(incidentParms);
            }

            num--;
        }

        Rand.PopState();
    }

    private static float IncidentChanceFinal(IncidentDef def)
    {
        var baseChanceThisGame = def.Worker.BaseChanceThisGame;
        baseChanceThisGame *= IncidentChanceFactor_CurrentPopulation(def);
        return Math.Max(0f, baseChanceThisGame);
    }

    private static float IncidentChanceFactor_CurrentPopulation(IncidentDef def)
    {
        if (def.chanceFactorByPopulationCurve == null)
        {
            return 1f;
        }

        var count = PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_Colonists.Count;
        return def.chanceFactorByPopulationCurve.Evaluate(count);
    }

    private readonly struct IncidentGroupData(float chance, int countMin, int countStep, int countMax)
    {
        public float Chance { get; } = chance;

        public int CountMin { get; } = countMin;

        public int CountStep { get; } = countStep;

        public int CountMax { get; } = countMax;
    }
}