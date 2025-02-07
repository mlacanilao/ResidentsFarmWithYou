using System;

namespace ResidentsFarmWithYou
{
    public class AI_FarmOmega : TaskPoint
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
                return 1006;
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
            this.owner?.ShowEmo(_emo: Emo.farm, duration: 0f, skipSame: true);
        }
        
        public override void OnProgress()
        {
            this.owner.PlaySound(id: "Material/mud", v: 1f, spatial: true);
        }
        
        public override void OnProgressComplete()
        {
            this.owner?.ModExp(ele: 286, a: 15);
            
            PlantData plantData = EClass._map?.TryGetPlant(c: this.pos?.cell);
            
            if (plantData != null &&
                FarmUtils.CanGrow(cell: this.pos?.cell) == true)
            {
                int mtp = 100;
                int num = this.pos.growth.Step * mtp * (1);
                int num2 = (int)(this.pos?.cell?.objVal / 30);
                int num3 = ((int)this.pos?.cell?.objVal + num) / 30;
                
                if (num2 != num3)
                {
                    if (num2 == this.pos?.growth?.HarvestStage && 
                        this.pos?.growth?.CanHarvest() == true)
                    {
                        try
                        {
                            FarmUtils.PopHarvest(c: null, growSystem: this.pos?.growth, plantData: plantData);
                        }
                        catch (Exception ex)
                        {
                            ResidentsFarmWithYou.Log(payload: $"Error during PopHarvest: {ex.Message}");
                        }
                    }
                    
                    if (num2 >= this.pos?.growth?.AutoMineStage)
                    {
                        bool enableEqualizePlants = ResidentsFarmWithYouConfig.EnableEqualizePlants?.Value ?? false;
                        if (enableEqualizePlants == true)
                        {
                            Point point = GrowSystem.cell.GetPoint();
                            this.pos?.growth?.EqualizePlants(point);
                        }
                        
                        // Try pop seed
                        Thing thing = TraitSeed.MakeSeed(obj: this.pos?.growth?.source, plant: plantData);
                        
                        FarmUtils.PopMineObj(growSystem: this.pos?.growth, plantData: plantData, c: null);
                        
                        if (thing != null)
                        {
                            this.pos?.SetObj(id: 0, value: 1, dir: 0);
                            EClass._zone?.AddCard(t: thing, point: this.pos).Install();
                        }
                    }
                    else if (num2 == this.pos?.growth?.StageLength - 1)
                    {
                        bool enableEqualizePlants = ResidentsFarmWithYouConfig.EnableEqualizePlants?.Value ?? false;
                        if (enableEqualizePlants == true)
                        {
                            Point point = GrowSystem.cell.GetPoint();
                            this.pos?.growth?.EqualizePlants(point);
                        }
                        
                        // Try pop seed
                        Thing thing2 = TraitSeed.MakeSeed(obj: this.pos?.growth?.source, plant: plantData);
                        
                        if (thing2 != null)
                        {
                            this.pos?.SetObj(id: 0, value: 1, dir: 0);
                            EClass._zone?.AddCard(t: thing2, point: this.pos).Install();
                        }
                    }
                    else
                    {
                        this.pos?.growth?.OnReachNextStage();
                    }
                }
                else
                {
                    Cell cell = this.pos?.cell;
                    cell.objVal += (byte)num;
                }
                this.pos?.cell.Refresh();
                EClass._map?.RefreshFOV(x: (int)this.owner?.pos?.cell.x, z: (int)this.owner?.pos?.cell.z, radius: 6, recalculate: false);

                bool enableFertilizer = ResidentsFarmWithYouConfig.EnableFertilizer?.Value ?? true;
                bool enableRequireFertilizer = ResidentsFarmWithYouConfig.EnableRequireFertilizer?.Value ?? false;

                if (enableFertilizer == true)
                {
                    if (enableRequireFertilizer == true)
                    {
                        Thing fertilizerShared = EClass._map?.Stocked?.Find(id: "fertilizer", idMat: -1, refVal: -1, shared: true);
                        
                        if (fertilizerShared != null)
                        {
                            // Fertilizer
                            Thing fertilizer = ThingGen.Create(id: "fertilizer");
                            EClass._zone?.AddCard(t: fertilizer, point: this.pos).Install();
                            
                            fertilizerShared.ModNum(a: -1, notify: true);
                            if (fertilizerShared.isDestroyed || fertilizerShared.Num == 0)
                            {
                                fertilizerShared = null;
                            }
                        }
                    }
                    else
                    {
                        // Fertilizer
                        Thing fertilizer = ThingGen.Create(id: "fertilizer");
                        EClass._zone?.AddCard(t: fertilizer, point: this.pos).Install();
                    }
                }
            }
            
            this.owner?.SetAI(g: new AI_WaterOmega
            {
                pos = this.pos,
                PreviousWork = this.PreviousWork
            });
        }
        
        public AIWork_FarmOmega PreviousWork;
    }
}