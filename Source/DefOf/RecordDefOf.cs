using RimWorld;

namespace Lanilor.LootBoxes.DefOf
{
    [RimWorld.DefOf]
    public static class RecordDefOf
    {
        static RecordDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(RecordDefOf));
        }

        public static RecordDef LootBoxesOpened;
    }
}