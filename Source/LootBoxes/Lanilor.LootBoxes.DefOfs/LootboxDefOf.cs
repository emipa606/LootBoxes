using RimWorld;
using Verse;

namespace Lanilor.LootBoxes.DefOfs;

[DefOf]
public static class LootboxDefOf
{
    public static ThingDef LootBoxTreasure;
    public static ThingDef LootBoxSilverSmall;
    public static ThingDef LootBoxSilverLarge;
    public static ThingDef LootBoxGoldSmall;
    public static ThingDef LootBoxGoldLarge;
    public static ThingDef LootBoxPandora;
    public static RecordDef LootBoxesOpened;
    public static IncidentCategoryDef ShipChunkDrop;
    public static IncidentCategoryDef OrbitalVisitor;
    public static IncidentCategoryDef FactionArrival;
    public static IncidentCategoryDef DiseaseAnimal;

    static LootboxDefOf()
    {
        DefOfHelper.EnsureInitializedInCtor(typeof(LootboxDefOf));
    }
}