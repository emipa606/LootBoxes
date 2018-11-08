using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace LootBoxes
{

    public class CompUseEffect_LootBox_Pandora : CompUseEffect_LootBox
    {

        protected override void OpenBox(Pawn usedBy)
        {
            Map map = usedBy.Map;
            List<IncidentDef> validIncidents = new List<IncidentDef>();
            foreach (IncidentDef incident in from def in DefDatabase<IncidentDef>.AllDefs where def.TargetAllowed(map) select def)
			{
				IncidentParms parms = StorytellerUtility.DefaultParmsNow(incident.category, map);
				if (incident.Worker.CanFireNow(parms, false))
				{
					validIncidents.Add(incident);
				}
            }
            IncidentDef selectedIncident = validIncidents.RandomElementByWeight(new Func<IncidentDef, float>(IncidentChanceFinal));
            if (selectedIncident != null)
            {
                IncidentParms parms = StorytellerUtility.DefaultParmsNow(selectedIncident.category, map);
                if (selectedIncident.pointsScaleable)
				{
					StorytellerComp storytellerComp = Find.Storyteller.storytellerComps.First((StorytellerComp x) => x is StorytellerComp_OnOffCycle || x is StorytellerComp_RandomMain);
                    if (storytellerComp != null)
                    {
                        parms = storytellerComp.GenerateParms(selectedIncident.category, parms.target);
                    }
				}
				selectedIncident.Worker.TryExecute(parms);
            }
        }

        private float IncidentChanceFinal(IncidentDef def)
        {
            float chance = def.Worker.AdjustedChance;
            chance *= IncidentChanceFactor_CurrentPopulation(def);
            return Math.Max(0f, chance);
        }

        private float IncidentChanceFactor_CurrentPopulation(IncidentDef def)
        {
            if (def.chanceFactorByPopulationCurve == null)
            {
                return 1f;
            }
            int pawnCount = PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_Colonists.Count();
            return def.chanceFactorByPopulationCurve.Evaluate(pawnCount);
        }


    }

}
