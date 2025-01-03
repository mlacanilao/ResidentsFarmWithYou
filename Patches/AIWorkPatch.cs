using System;

namespace ResidentsFarmWithYou.Patches
{
    public static class AIWorkPatch
    {
        public static void SetDestPosPostfix(AIWork __instance)
        {
            if (__instance is AIWork_Farm == false)
            {
                return;
            }

            if (__instance.destPos?.cell?.isWatered == false)
            {
                return;
            }
            
            const int maxAttempts = 25;
            int attempts = 0;

            while (__instance.destPos?.cell?.isWatered == true)
            {
                attempts++;
        
                if (attempts >= maxAttempts)
                {
                    return;
                }

                if (__instance.destThing != null && __instance.destThing?.ExistsOnMap == true)
                {
                    __instance.destPos = __instance.destThing?.trait?.GetRandomPoint(func: new Func<Point, bool>(__instance._FuncWorkPoint), accessChara: null);
                }
            }
        }
    }
}