using System.Reflection;
using Harmony;
using Verse;

namespace LootBoxes
{

    [StaticConstructorOnStartup]
    public class Harmony
    {

        static Harmony()
        {
            HarmonyInstance harmony = HarmonyInstance.Create("rimworld.lanilor.lootboxes");
            harmony.PatchAll(Assembly.GetExecutingAssembly());
        }

    }

}
