using System.Reflection;
using Verse;

namespace Lanilor.LootBoxes.Harmony
{
    [StaticConstructorOnStartup]
    public class HarmonyPatchLoad
    {
        static HarmonyPatchLoad()
        {
            var harmony = new HarmonyLib.Harmony("rimworld.lanilor.lootboxes");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}