using ResidentsFarmWithYou.Patches;
using HarmonyLib;

namespace ResidentsFarmWithYou
{
    public class Patcher
    {
        [HarmonyPrefix]
        [HarmonyPatch(declaringType: typeof(ElementContainer), methodName: nameof(ElementContainer.ModExp))]
        public static void ElementContainerModExp(ElementContainer __instance, int ele, int a, bool chain)
        {
            ElementContainerPatch.ModExpPrefix(__instance: __instance, ele: ele, a: a, chain: chain);
        }
        
        [HarmonyPrefix]
        [HarmonyPatch(declaringType: typeof(AI_Farm), methodName: nameof(AI_Farm.OnProgress))]
        public static bool AI_FarmOnProgressComplete(AI_Farm __instance)
        {
            return AI_FarmPatch.OnProgressPrefix(__instance: __instance);
        }
        
        [HarmonyPostfix]
        [HarmonyPatch(declaringType: typeof(AIWork), methodName: nameof(AIWork.SetDestPos))]
        public static void AIWorkSetDestPos(AIWork __instance)
        {
            AIWorkPatch.SetDestPosPostfix(__instance: __instance);
        }
        
        [HarmonyPostfix]
        [HarmonyPatch(declaringType: typeof(AI_Water), methodName: nameof(AI_Water.OnProgressComplete))]
        public static void AI_WaterOnProgressComplete(AI_Water __instance)
        {
            AI_WaterPatch.OnProgressCompletePostfix(__instance: __instance);
        }
    }
}