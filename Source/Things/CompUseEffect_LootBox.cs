using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using RimWorld;
using Verse;
using Verse.Sound;
using RecordDefOf = Lanilor.LootBoxes.DefOf.RecordDefOf;

namespace Lanilor.LootBoxes.Things
{
    public abstract class CompUseEffectLootBox : CompUseEffect
    {
        [NotNull] protected virtual string LootTable => string.Empty;

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

        // This entire section should be optimized. Old version used a janky hack to obfuscate the spawn tables (why??)
        [NotNull]
        private IEnumerable<Thing> CreateLoot()
        {
            var lootEntries = new Dictionary<string, float>();
            var arr = LootTable.Split('|');
            var setsMin = int.Parse(arr[0]);
            var setsMax = int.Parse(arr[1]);
            var setsCount = Utilities.GetRealCount(parent,
                Rand.RangeInclusive(setsMin, Rand.RangeInclusive(setsMin, setsMax)));
            for (int i = 2, iLen = arr.Length; i < iLen; i++)
            {
                var str = arr[i];
                var pos = str.FirstIndexOf(c => c == ';');
                lootEntries.Add(str.Substring(pos + 1), float.Parse(str.Substring(0, pos)));
            }

            var lootList = new List<Thing>();
            for (var i = 0; i < setsCount; i++)
            {
                var chosenSet = lootEntries.RandomElementByWeight(kvp => kvp.Value * kvp.Value).Key;
                arr = chosenSet.Split(';');
                for (int j = 0, jLen = arr.Length; j < jLen; j++)
                {
                    var inner = arr[j].Split(',');
                    var min = inner.Length <= 1 ? 1 : int.Parse(inner[1]);
                    var max = inner.Length <= 2 ? min : int.Parse(inner[2]);
                    if (inner.Length >= 4) max = Rand.RangeInclusive(max, int.Parse(inner[3]));
                    var def = DefDatabase<ThingDef>.GetNamed(inner[0]);
                    if (def == null) continue;

                    var countToDo = Rand.RangeInclusive(min, max);
                    while (countToDo > 0)
                    {
                        var count = Math.Min(countToDo, def.stackLimit);
                        countToDo -= count;
                        var thing = ThingMaker.MakeThing(def, GenStuff.RandomStuffFor(def));
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