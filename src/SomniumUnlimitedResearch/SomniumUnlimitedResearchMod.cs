using HarmonyLib;
using ModLoader;
using System;

namespace SomniumUnlimitedResearch
{
    public class SomniumUnlimitedResearchMod : IMod
    {
        Harmony HarmonyInstance;
        IModHelper Helper;

        public void ModEntry(IModHelper helper)
        {
            Helper = helper;
            HarmonyInstance = new Harmony("SomniumTD.SomniumUnlimitedResearch");
            HarmonyInstance.PatchAll();

            /* This way would be cleaner, since it uses the developer mode to override research points instead of a Harmony override.
             * However, the amount of points granted by the developer mode is not enough to unlock all research.
             * Index for the override in the developerOverrides array is from the "TinyZoo.DeveloperOverrides" enum

            int[] overrides = Reflection.GetStaticFieldValue<int[]>("Z_DebugFlags", "developerOverrides");
            overrides[1] = 1;
            */

            // Using AccessTools and manual patching instead of Harmony annotations because the "Unlocks" class has visibility "internal"
            var researchType = Type.GetType("TinyZoo.PlayerDir.Unlocks, LetsBuildAZoo");
            HarmonyInstance.Patch(
                    original: AccessTools.DeclaredPropertyGetter(researchType, "ResearchPoints"),
                    postfix: new HarmonyMethod(GetType(), nameof(ResearchPointOverride))
                    );
        }
        public static void ResearchPointOverride(ref int __result)
        {
            __result = 999; // Enough points to unlock even the most expensive research project.
        }
    }
}
