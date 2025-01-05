using System;
using ResidentsFarmWithYou.Utilities;

namespace ResidentsFarmWithYou.Patches
{
    public static class AI_FarmPatch
    {
        public static bool OnProgressPrefix(AI_Farm __instance)
        {
            if (EClass.core?.IsGameStarted == false ||
                EClass._zone?.IsPCFaction == false ||
                __instance == null || 
                __instance.owner?.IsPC == true)
            {
                return true;
            }
            
            __instance.owner?.PlaySound(id: "Material/mud", v: 1f, spatial: true);
            
            PlantData plantData = EClass._map?.TryGetPlant(c: __instance.owner?.pos?.cell);
            
            if (FarmUtils.CanGrow(cell: __instance.pos?.cell) == true &&
                plantData != null)
            {
                int mtp = 100;
                int num = __instance.pos.growth.Step * mtp * (1);
                int num2 = (int)(__instance.owner?.pos?.cell?.objVal / 30);
                int num3 = ((int)__instance.owner?.pos?.cell?.objVal + num) / 30;
                
                if (num2 != num3)
                {
                    if (num2 == __instance.pos?.growth?.HarvestStage && 
                        __instance.pos?.growth?.CanHarvest() == true)
                    {
                        try
                        {
                            __instance.pos?.growth?.PopHarvest(c: null, t: null as Thing, num: -1);
                        }
                        catch (Exception ex)
                        {
                            ResidentsFarmWithYou.Log(payload: $"Error during PopHarvest: {ex.Message}");
                        }
                    }
                    
                    if (num2 >= __instance.pos?.growth?.AutoMineStage)
                    {
                        __instance.pos?.growth?.EqualizePlants(pos: __instance.pos);
                        
                        // Try pop seed
                        Thing thing = TraitSeed.MakeSeed(obj: __instance.pos?.growth?.source, plant: plantData);
                        
                        __instance.pos?.growth?.ApplySeed(t: thing);
                        
                        // Try move seed
                        if (EClass._zone?.TryAddThingInSharedContainer(t: thing, containers: null, add: true, msg: false, chara: null, sharedOnly: true) == false)
                        {
                            EClass._map?.TrySmoothPick(cell: thing.pos?.cell, t: thing, c: EClass.pc);
                        }
                        
                        __instance.pos?.growth?.PopMineObj(c: null);
                        
                        if (thing != null)
                        {
                            __instance.pos?.SetObj(id: 0, value: 1, dir: 0);
                            EClass._zone?.AddCard(t: thing, point: __instance.pos).Install();
                        }
                    }
                    else if (num2 == __instance.pos?.growth?.StageLength - 1)
                    {
                        __instance.pos?.growth?.EqualizePlants(pos: __instance.pos);
                        
                        // Try pop seed
                        Thing thing2 = TraitSeed.MakeSeed(obj: __instance.pos?.growth?.source, plant: plantData);
                        
                        __instance.pos?.growth?.ApplySeed(t: thing2);
                        
                        // Try move seed
                        if (EClass._zone?.TryAddThingInSharedContainer(t: thing2, containers: null, add: true, msg: false, chara: null, sharedOnly: true) == false)
                        {
                            EClass._map?.TrySmoothPick(cell: thing2.pos?.cell, t: thing2, c: EClass.pc);
                        }
                        
                        if (thing2 != null)
                        {
                            __instance.pos?.SetObj(id: 0, value: 1, dir: 0);
                            EClass._zone?.AddCard(t: thing2, point: __instance.pos).Install();
                        }
                    }
                    else
                    {
                        __instance.pos?.growth?.OnReachNextStage();
                    }
                }
                else
                {
                    Cell cell = __instance.pos?.cell;
                    cell.objVal += (byte)num;
                }
                __instance.pos?.cell.Refresh();
                EClass._map?.RefreshFOV(x: (int)__instance.owner?.pos?.cell.x, z: (int)__instance.owner?.pos?.cell.z, radius: 6, recalculate: false);
                
                // Move extra items to shared container
                foreach (Card card in __instance.pos?.ListCards())
                {
                    if (card == __instance.owner)
                    {
                        continue;
                    }
                    
                    if (card.isThing &&
                        card.placeState == PlaceState.roaming &&
                        card.ignoreAutoPick == false)
                    {
                        if (EClass._zone?.TryAddThingInSharedContainer(t: card.Thing, containers: null, add: true, msg: false,
                                chara: null, sharedOnly: true) == false)
                        {
                            EClass._map?.TrySmoothPick(cell: card.pos.cell, t: card.Thing, c: EClass.pc);
                        }
                    }
                }
                
                // Fertilizer
                Thing fertilizer = ThingGen.Create(id: "fertilizer");
                EClass._zone?.AddCard(t: fertilizer, point: __instance.pos).Install();
            }
            
            __instance.owner?.SetAI(g: new AI_Water
            {
                pos = __instance.pos
            });

            return false;
        }
    }
}