using System;
using System.Collections.Generic;

namespace ResidentsFarmWithYou
{
    public class AI_WaterOmega : TaskPoint
    {
        public override int LeftHand
        {
            get
            {
                return -1;
            }
        }
        
        public override int RightHand
        {
            get
            {
                return 1105;
            }
        }
        
        public override bool HasProgress
        {
            get
            {
                return true;
            }
        }
        
        public override bool CancelWhenMoved
        {
            get
            {
                return false;
            }
        }
        
        public override void OnStart()
        {
        }
        
        public override void OnProgress()
        {
            this.owner?.SetTempHand(right: 1106, left: -1);
            this.owner?.PlaySound(id: "Material/mud", v: 1f, spatial: true);
        }
        
        public override void OnProgressComplete()
        {
            this.owner?.ModExp(ele: 286, a: 15);
            
            this.owner?.PlaySound(id: "water_farm", v: 1f, spatial: true);
            EClass._map?.SetLiquid(x: this.pos.x, z: this.pos.z, id: 1, value: 2);
            this.pos.cell.isWatered = true;

            int radius = this.PreviousWork.destThing.trait.radius;
            
            Point newFarmPoint = null;

            for (int r = 1; r <= radius; r++)
            {
                for (int i = this.pos.x - r; i <= this.pos.x + r; i++)
                {
                    for (int j = this.pos.z - r; j <= this.pos.z + r; j++)
                    {
                        Point tempPoint = new Point();
                        tempPoint.Set(_x: i, _z: j);
                        
                        if (tempPoint.IsValid &&
                            this.owner?.CanMoveTo(p: tempPoint) == true &&
                            tempPoint.HasChara == false &&
                            tempPoint.IsFarmField &&
                            tempPoint.cell?.isWatered == false &&
                            tempPoint.cell?.HasBlock == false
                            )
                        {
                            newFarmPoint = tempPoint;
                            break;
                        }
                    }

                    if (newFarmPoint != null) break;
                }

                if (newFarmPoint != null) break;
            }

            if (newFarmPoint != null)
            {
                this.owner?.SetAIImmediate(g: new AI_FarmOmega
                {
                    pos = newFarmPoint,
                    PreviousWork = this.PreviousWork
                });
            }
        }
        
        public AIWork_FarmOmega PreviousWork;
    }
}