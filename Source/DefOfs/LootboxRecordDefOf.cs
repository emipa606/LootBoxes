using JetBrains.Annotations;
using RimWorld;

namespace Lanilor.LootBoxes.DefOfs
{
    [DefOf]
    public static class LootboxRecordDefOf
    {
        static LootboxRecordDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(LootboxRecordDefOf));
        }

        [UsedImplicitly] public static RecordDef LootBoxesOpened;
    }
}