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
            
            bool enableFarmingLevelLimit = ResidentsFarmWithYouConfig.EnableFarmingLevelLimit?.Value ?? false;

            if (enableFarmingLevelLimit == true)
            {
                int farmingLevel = EClass.pc?.Evalue(ele: 286) ?? 0;
                if (encLv > farmingLevel)
                {
                    encLv = farmingLevel;
                }
            }
            
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
            return false;
        }
    }
}