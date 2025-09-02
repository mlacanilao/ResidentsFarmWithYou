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
            
            if (plantData != null)
            {
                bool enableFertilizer = ResidentsFarmWithYouConfig.EnableFertilizer?.Value ?? true;
                bool enableRequireFertilizer = ResidentsFarmWithYouConfig.EnableRequireFertilizer?.Value ?? false;
                bool enableSeedLevelMatch = ResidentsFarmWithYouConfig.EnableSeedLevelMatch?.Value ?? true;

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

                if (enableSeedLevelMatch == true)
                {
                    plantData.seed.encLV = EClass.pc?.Evalue(ele: 286) ?? plantData.seed.encLV;
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