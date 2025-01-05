namespace ResidentsFarmWithYou.Patches
{
    public static class GrowSystemPatch
    {
        public static void ApplySeedPrefix(ref Thing t)
        {
            if (ResidentsFarmWithYouConfig.EnableAutoPlaceFarmingItems?.Value == false ||
                EClass.core?.IsGameStarted == false ||
                EClass._zone?.IsPCFaction == false ||
                t == null)
            {
                return; 
            }
            
            t.isWeightChanged = true;
        }

        public static bool TryPickPrefix(GrowSystem __instance, Thing t, Chara c, bool applySeed)
        {
            if (ResidentsFarmWithYouConfig.EnableAutoPlaceFarmingItems?.Value == false ||
                EClass.core?.IsGameStarted == false ||
                EClass._zone?.IsPCFaction == false ||
                t?.pos?.IsInBounds == true ||
                c?.ai is AI_Eat)
            {
                return true;
            }
            
            if (applySeed)
            {
                __instance.ApplySeed(t);
            }
            
            if (EClass._zone?.TryAddThingInSharedContainer(t: t, containers: null, add: true, msg: false, chara: null, sharedOnly: true) == false)
            {
                EClass._map?.TrySmoothPick(cell: t.pos?.cell, t: t, c: EClass.pc);
            }

            return false;
        }
    }
}