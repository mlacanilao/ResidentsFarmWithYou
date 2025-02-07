using System;

namespace ResidentsFarmWithYou
{
    internal class GoalWorkPatch
    {
        internal static bool TryWorkPrefix(GoalWork __instance, BaseArea destArea, Hobby h, bool setAI,
            ref bool __result)
        {
            if (EClass.core?.IsGameStarted == false ||
                EClass._zone?.IsPCFaction == false ||
                h.source == null ||
                (h.source?.alias != "Farm" && h.source?.alias != "Gardening"))
            {
                return true;
            }
            
            AIWork ai = new AIWork_FarmOmega();
            ai.owner = __instance.owner;
            ai.sourceWork = h.source;
            ai.destArea = destArea;
            ai.destThing = EClass._map?.FindThing(type: Type.GetType(typeName: "TraitSpotFarm, Elin"), c: __instance.owner);
            
            if (ai.destThing != null)
            {
                if (setAI)
                {
                    __instance.owner.SetAI(g: ai);
                }

                __result = true;
                return false;
            }
            __result = false;
            return false;
        }
    }
}