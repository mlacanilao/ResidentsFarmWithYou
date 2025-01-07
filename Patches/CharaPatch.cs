namespace ResidentsFarmWithYou.Patches
{
    public static class CharaPatch
    {
        public static bool PickPrefix(Chara __instance, Thing t)
        {
            if (ResidentsFarmWithYouConfig.EnableAutoPlaceFarmingItems?.Value == false ||
                EClass.core?.IsGameStarted == false ||
                EClass._zone?.IsPCFaction == false ||
                __instance == null ||
                __instance.IsPC == false ||
                t.isWeightChanged == false ||
                t.isCrafted == false ||
                t.pos?.IsInBounds == true ||
                __instance.ai is AI_Eat || 
                __instance.ai is AI_UseCrafter)
            {
                return true;
            }
            
            if (EClass._zone?.TryAddThingInSharedContainer(t: t, containers: null, add: true, msg: false, chara: null, sharedOnly: true) == false)
            {
                EClass._map?.TrySmoothPick(cell: t.pos?.cell, t: t, c: EClass.pc);
            }

            return false;
        }
    }
}