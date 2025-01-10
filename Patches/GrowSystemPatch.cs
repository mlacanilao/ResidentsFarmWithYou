namespace ResidentsFarmWithYou.Patches
{
    public static class GrowSystemPatch
    {
        public static bool ApplySeedPrefix(ref Thing t)
        {
            if (t == null ||
                EClass.core?.IsGameStarted == false ||
                EClass._zone?.IsPCFaction == false)
            {
                return true;
            }
            
            PlantData plantData = EClass._map?.TryGetPlant(c: GrowSystem.cell);

            Thing thing = plantData?.seed;

            if (thing == null)
            {
                return true;
            }

            int encLv = thing.encLV + (thing.encLV > 0 ? 1 : 0);
            bool flag = t.IsFood || t.Evalue(ele: 10) > 0 || t.id == "grass";
            foreach (Element element in thing.elements?.dict?.Values)
            {
                if ((!element.IsFoodTrait || flag) && (element.IsFoodTrait || element.id == 2))
                {
                    t.elements.ModBase(ele: element.id, v: element.Value / 10 * 10);
                }
            }
            t.SetEncLv(a: encLv);
            t.c_refText = t.c_refText;
            t.isCrafted = true;

            bool enableAutoPlaceFarmingItems = ResidentsFarmWithYouConfig.EnableAutoPlaceFarmingItems?.Value ?? false;
            
            if (enableAutoPlaceFarmingItems == false)
            {
                return false; 
            }
            
            t.isWeightChanged = true;

            return false;
        }

        public static bool TryPickPrefix(GrowSystem __instance, Thing t, Chara c, bool applySeed)
        {
            bool enableAutoPlaceFarmingItems = ResidentsFarmWithYouConfig.EnableAutoPlaceFarmingItems?.Value ?? false;
            
            if (enableAutoPlaceFarmingItems == false ||
                EClass.core?.IsGameStarted == false ||
                EClass._zone?.IsPCFaction == false ||
                t?.pos?.IsInBounds == true ||
                c?.ai is AI_Eat)
            {
                return true;
            }
            
            if (applySeed)
            {
                __instance.ApplySeed(t: t);
            }
            
            if (EClass._zone?.TryAddThingInSharedContainer(t: t, containers: null, add: true, msg: false, chara: null, sharedOnly: true) == false)
            {
                EClass._map?.TrySmoothPick(cell: t.pos?.cell, t: t, c: EClass.pc);
            }

            return false;
        }
    }
}