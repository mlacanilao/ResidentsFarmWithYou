using System;
using System.Collections.Generic;
using UnityEngine;

namespace ResidentsFarmWithYou
{
    public static class FarmUtils
    {
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
        
        public static void PopHarvest(Chara c, GrowSystem growSystem, PlantData plantData, Thing t = null, int num = -1)
        {
            if (t == null)
            {
                t = ((!growSystem.idHarvestThing.StartsWith(value: "#")) ? ThingGen.Create(id: growSystem.idHarvestThing.IsEmpty(defaultStr: "apple")) : ThingGen.CreateFromCategory(idCat: growSystem.idHarvestThing.Replace(oldValue: "#", newValue: "")));
            }
            
            growSystem.ApplySeed(t: t);
            
            if (plantData != null && plantData.size > 0)
            {
                t.c_weight = t.SelfWeight * (80 + plantData.size * plantData.size * 100) / 100;
                t.SetBool(id: 115, enable: true);
                t.isWeightChanged = true;
            }
            t.SetBlessedState(s: BlessedState.Normal);
            if (growSystem.source?._growth?.Length > 4)
            {
                int num2 = EClass.rnd(a: growSystem.source._growth[4].ToInt()) + 1;
                int soilCost = EClass._zone.GetSoilCost();
                int maxSoil = EClass._zone.MaxSoil;
                if (soilCost > maxSoil && EClass.player.stats.days >= 5)
                {
                    num2 -= EClass.rnd(a: 2 + (soilCost - maxSoil) / 20);
                }
                if (num2 <= 0)
                {
                    return;
                }
                t.SetNum(a: num2);
            }
            else
            {
                ResidentsFarmWithYou.Log(payload: "harvest count not set:" + growSystem.source?.id + "/" + growSystem.source?.alias);
            }
            if (num > 0)
            {
                t.SetNum(a: num);
            }
            
            TryAddThingToSharedContainer(thing: t);
        }
        
        public static void PopMineObj(GrowSystem growSystem, PlantData plantData, Chara c = null)
        {
            switch (growSystem)
            {
                case GrowSystemBerry berrySystem:
                    HandleBerrySystem(berrySystem: berrySystem, plantData: plantData, chara: c);
                    break;
                case GrowSystemCactus cactusSystem:
                    HandleCactusSystem(cactusSystem: cactusSystem, plantData: plantData, chara: c);
                    break;
                case GrowSystemPasture pastureSystem:
                    HandlePastureSystem(pastureSystem: pastureSystem, plantData: plantData, chara: c);
                    break;
                case GrowSystemWheat wheatSystem:
                    HandleWheatSystem(wheatSystem: wheatSystem, plantData: plantData, chara: c);
                    break;
                case GrowSystemCrop cropSystem:
                    HandleCropSystem(cropSystem: cropSystem, plantData: plantData, chara: c);
                    break;
                case GrowSystemKinoko kinokoSystem:
                    HandleKinokoSystem(kinokoSystem: kinokoSystem, plantData: plantData, chara: c);
                    break;
                case GrowSystemFlower flowerSystem:
                    HandleFlowerSystem(flowerSystem: flowerSystem, plantData: plantData, chara: c);
                    break;
                case GrowSystemHerb herbSystem:
                    HandleHerbSystem(herbSystem: herbSystem, plantData: plantData, chara: c);
                    break;
                case GrowSystemPlant plantSystem:
                    HandlePlantSystem(plantSystem: plantSystem, plantData: plantData, chara: c);
                    break;
                case GrowSystemTree treeSystem:
                    treeSystem.OnMineObj();
                    break;
                case GrowSystemWeed weedSystem:
                    HandleWeedSystem(weedSystem: weedSystem, plantData: plantData, chara: c);
                    break;
                default:
                    HandleOnMineObj(growSystem: growSystem, chara: c);
                    break;
            }
            
            // Try pop seed
            Thing thing = TraitSeed.MakeSeed(obj: growSystem.source, plant: plantData);
            Thing duplicate = thing.Duplicate(num: 1);
                        
            // Try move seed
            TryAddThingToSharedContainer(thing: duplicate);
        }

        private static void HandleCropSystem(GrowSystemCrop cropSystem, PlantData plantData, Chara chara)
        {
            if (cropSystem.source.alias == "grape" && (cropSystem.stage.idx == 2 || cropSystem.stage.idx == 3))
            {
                Thing vineThing = ThingGen.Create(id: "pasture", idMat: -1, lv: -1);
                TryAddThingToSharedContainer(thing: vineThing);
            }
            HandleOnMineObj(growSystem: cropSystem, chara: chara);
        }

        private static void HandleOnMineObj(GrowSystem growSystem, Chara chara)
        {
            if (!growSystem.IsWithered())
            {
                int num = GrowSystem.cell.sourceObj.components.Length - 1;
                Thing t = ThingGen.Create(id: GrowSystem.cell.sourceObj.components[num].Split(separator: '/', options: StringSplitOptions.None)[0], idMat: GrowSystem.cell.matObj_fixed.alias);
                TryAddThingToSharedContainer(thing: t);
            }
        }

        private static void HandleWheatSystem(GrowSystemWheat wheatSystem, PlantData plantData, Chara chara)
        {
            if (wheatSystem.IsWithered() || wheatSystem.stage.idx == wheatSystem.HarvestStage)
            {
                Thing wheatThing = ThingGen.Create(id: "grass", idMat: "straw");
                TryAddThingToSharedContainer(thing: wheatThing);
                return;
            }

            HandleOnMineObj(growSystem: wheatSystem, chara: chara);
        }

        private static void HandlePastureSystem(GrowSystemPasture pastureSystem, PlantData plantData, Chara chara)
        {
            if (pastureSystem.IsWithered() || pastureSystem.stage.idx == 0)
            {
                HandleOnMineObj(growSystem: pastureSystem, chara: chara);
                return;
            }
            int num = 1 + EClass.rnd(a: pastureSystem.stage.idx);
            Thing thing = (plantData != null) ? plantData.seed : null;
            if (thing != null && thing.encLV > 1 && !EClass._zone.IsUserZone)
            {
                num += EClass.rndHalf(a: (int)Mathf.Sqrt(f: (float)thing.encLV) + 1);
            }
            Thing pastureThing = ThingGen.Create(id: "pasture", idMat: -1, lv: -1);
            PopHarvest(c: chara, growSystem: pastureSystem, plantData: plantData, t: pastureThing, num: num);
        }

        private static void HandleHerbSystem(GrowSystemHerb herbSystem, PlantData plantData, Chara chara)
        {
            int idMat = EClass.sources.materials.alias[key: "grass"].id;
            Thing thing = ThingGen.Create(id: "grass", idMat: idMat, lv: -1);
            TryAddThingToSharedContainer(thing: thing);
            if (herbSystem.Grown || herbSystem.Mature)
            {
                Thing flowerThing = ThingGen.Create(id: herbSystem.idHarvestThing?.IsEmpty(defaultStr: "flower"), idMat: -1, lv: -1);
                PopHarvest(c: chara ?? EClass.pc, growSystem: herbSystem, plantData: plantData, t: flowerThing, num: -1);
            }
        }

        private static void HandleFlowerSystem(GrowSystemFlower flowerSystem, PlantData plantData, Chara chara)
        {
            int idMat = EClass.sources.materials.alias[key: "grass"].id;
            Thing thing = ThingGen.Create(id: "grass", idMat: idMat, lv: -1);
            TryAddThingToSharedContainer(thing: thing);
            if (flowerSystem.Grown || flowerSystem.Mature)
            {
                Thing flowerThing = ThingGen.Create(id: flowerSystem.idHarvestThing?.IsEmpty(defaultStr: "flower"), idMat: -1, lv: -1);
                PopHarvest(c: chara ?? EClass.pc, growSystem: flowerSystem, plantData: plantData, t: flowerThing, num: -1);
            }
        }

        private static void HandleCactusSystem(GrowSystemCactus cactusSystem, PlantData plantData, Chara chara)
        {
            Thing thing = ThingGen.Create(id: "needle", idMat: -1, lv: -1);
            TryAddThingToSharedContainer(thing: thing);
        }

        private static void HandleBerrySystem(GrowSystemBerry berrySystem, PlantData plantData, Chara chara)
        {
            int idMat = EClass.sources.materials.alias[key: "grass_forest"].id;
            Thing thing = ThingGen.Create(id: "grass", idMat: idMat, lv: -1);
            TryAddThingToSharedContainer(thing: thing);
            HandleOnMineObj(growSystem: berrySystem, chara: chara);
        }

        private static void HandleWeedSystem(GrowSystemWeed weedSystem, PlantData plantData, Chara chara)
        {
            int idMat = EClass.sources.materials.alias[key: "grass"].id;
            Thing thing = ThingGen.Create(id: "grass", idMat: idMat, lv: -1);
            TryAddThingToSharedContainer(thing: thing);
            HandleOnMineObj(growSystem: weedSystem, chara: chara);
        }

        private static void HandlePlantSystem(GrowSystemPlant plantSystem, PlantData plantData, Chara chara)
        {
            int idMat = EClass.sources.materials.alias[key: "grass"].id;
            Thing thing = ThingGen.Create(id: "grass", idMat: idMat, lv: -1);
            TryAddThingToSharedContainer(thing: thing);
            HandleOnMineObj(growSystem: plantSystem, chara: chara);
        }

        private static void HandleKinokoSystem(GrowSystemKinoko kinokoSystem, PlantData plantData, Chara chara)
        {
            Thing thing = ThingGen.CreateFromCategory(idCat: "mushroom", lv: -1);
            PopHarvest(c: chara ?? EClass.pc, growSystem: kinokoSystem, plantData: plantData, t: thing, num: -1);
        }

        internal static void TryAddThingToSharedContainer(Thing thing)
        {
            if (EClass._zone?.TryAddThingInSharedContainer(
                    t: thing, 
                    containers: null, 
                    add: true, 
                    msg: false, 
                    chara: null, 
                    sharedOnly: true) == false)
            {
                EClass.game?.cards?.container_shipping?.AddCard(c: thing);
            }
        }
    }
}