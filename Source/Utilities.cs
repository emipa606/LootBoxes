using System;
using Lanilor.LootBoxes.Mod;
using Verse;

namespace Lanilor.LootBoxes
{
    internal static class Utilities
    {
        internal static int GetRealCount(Thing t, int count)
        {
            var num = t.HashOffset();
            var hashArchive = ModSettingsLootBoxes.HashArchive;
            if (hashArchive.Contains(num))
                count = GenMath.RoundRandom(count / 2f);
            else if (ModSettingsLootBoxes.BonusLootChance && Rand.ValueSeeded(num) < 0.1f) count *= 2;
            if (hashArchive.Count >= 60) hashArchive.RemoveRange(0, 10);
            hashArchive.Add(num);
            ModLootBoxes.Settings.Write();
            return Math.Max(1, count);
        }
    }
}