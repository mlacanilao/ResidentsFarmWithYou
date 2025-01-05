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
        
        [HarmonyPrefix]
        [HarmonyPatch(declaringType: typeof(AI_Water), methodName: nameof(AI_Water.OnProgressComplete))]
        public static bool AI_WaterOnProgressComplete(AI_Water __instance)
        {
            return AI_WaterPatch.OnProgressCompletePrefix(__instance: __instance);
        }
        
        [HarmonyPrefix]
        [HarmonyPatch(declaringType: typeof(GrowSystem), methodName: nameof(GrowSystem.TryPick), argumentTypes: new[] { typeof(Cell), typeof(Thing), typeof(Chara), typeof(bool) })]
        public static bool GrowSystemTryPick(GrowSystem __instance, Thing t, Chara c, bool applySeed)
        {
            return GrowSystemPatch.TryPickPrefix(__instance: __instance, t: t, c:c, applySeed: applySeed);
        }
        
        [HarmonyPrefix]
        [HarmonyPatch(declaringType: typeof(GrowSystem), methodName: nameof(GrowSystem.ApplySeed))]
        public static bool GrowSystemApplySeed(ref Thing t)
        {
            return GrowSystemPatch.ApplySeedPrefix(t: ref t);
        }
        
        [HarmonyPrefix]
        [HarmonyPatch(declaringType: typeof(Chara), methodName: nameof(Chara.Pick))]
        public static bool CharaPick(Chara __instance, Thing t)
        {
            return CharaPatch.PickPrefix(__instance: __instance, t: t);
        }
    }
}