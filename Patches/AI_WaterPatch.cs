using System.Collections.Generic;
using ResidentsFarmWithYou.Utilities;

namespace ResidentsFarmWithYou.Patches
{
    public static class AI_WaterPatch
    {
        public static void OnProgressCompletePostfix(AI_Water __instance)
        {
            Point newFarmPoint = null;

            List<Point> neighbors = FarmUtils.GetNeighborPoints(origin: __instance.pos);

            foreach (Point p in neighbors)
            {
                if (p.IsFarmField && p.cell?.isWatered == false && p.cell?.HasBlock == false)
                {
                    newFarmPoint = p;
                    break;
                }
            }

            if (newFarmPoint != null)
            {
                __instance.owner?.SetAIImmediate(g: new AI_Farm
                {
                    pos = newFarmPoint
                });
            }
        }
    }
}