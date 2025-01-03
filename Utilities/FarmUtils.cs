using System;
using System.Collections.Generic;
using System.Linq;

namespace ResidentsFarmWithYou.Utilities
{
    public static class FarmUtils
    {
        public static List<Point> GetNeighborPoints(Point origin, bool includeDiagonals = true)
        {
            List<Point> neighbors = new List<Point>();
            Point tempPoint = new Point();

            // Iterate through all possible positions within a 2-tile radius
            for (int i = origin.x - 2; i <= origin.x + 2; i++)
            {
                for (int j = origin.z - 2; j <= origin.z + 2; j++)
                {
                    // Skip diagonal points if diagonals are not included
                    if (!includeDiagonals && Math.Abs(value: i - origin.x) + Math.Abs(value: j - origin.z) > 1)
                    {
                        continue;
                    }

                    tempPoint.Set(_x: i, _z: j);
                    if (tempPoint.IsValid && (i != origin.x || j != origin.z)) // Exclude the origin point
                    {
                        neighbors.Add(item: new Point(p: tempPoint));
                    }
                }
            }

            return neighbors;
        }
        
        public static bool CanGrow(Cell cell)
        {
            if (cell.growth?.source?.id == 0)
            {
                return false;
            }
            if (cell.HasBlock == true && 
                cell.sourceBlock?.tileType?.IsFence == false)
            {
                return false;
            }
            if (cell.growth?.NeedSunlight == true)
            {
                if (cell.HasRoof == true ||
                    EClass._map?.IsIndoor == true)
                {
                    return false;
                }
            }
            return true;
        }
    }
}