using System.Collections.Generic;
using ResidentsFarmWithYou.Utilities;

namespace ResidentsFarmWithYou.Patches
{
    public static class AI_WaterPatch
    {
        public static bool OnProgressCompletePrefix(AI_Water __instance)
        {
            __instance.owner.PlaySound(id: "water_farm", v: 1f, spatial: true);
            EClass._map?.SetLiquid(x: __instance.pos.x, z: __instance.pos.z, id: 1, value: 2);
            __instance.pos.cell.isWatered = true;
            
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

            return false;
        }
    }
}