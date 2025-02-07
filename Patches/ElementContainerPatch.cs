namespace ResidentsFarmWithYou.Patches
{
    public static class ElementContainerPatch
    {
        public static void ModExpPrefix(ElementContainer __instance, int ele, int a, bool chain)
        {
            if (EClass.core?.IsGameStarted == false ||
                EClass._zone?.IsPCFaction == false ||
                __instance.Chara?.IsPC == false ||
                (ele != 286 && ele != 250) ||
                EClass._zone?.branch is null)
            {
                return;
            }
            
            foreach (Chara chara in EClass._zone?.branch?.members)
            {
                if (chara.IsPC == true)
                {
                    continue;
                }
                
                bool hasFarmingWork = false;
                bool hasFarmingHobby = false;
                
                foreach (Hobby w in chara.ListWorks())
                {
                    if (w.source.alias != "Farm" && 
                        w.source.alias != "Gardening")
                    {
                        continue;
                    }
                    hasFarmingWork = true;
                }
                
                foreach (Hobby h in chara.ListHobbies())
                {
                    if (h.source.alias != "Farm" && 
                        h.source.alias != "Gardening")
                    {
                        continue;
                    }
                    hasFarmingHobby = true;
                }

                if (hasFarmingWork == true &&
                    !(chara.ai is AIWork_Farm || chara.ai is AI_Farm || chara.ai is AI_Water || chara.ai is AI_Eat))
                {
                    Goal goal = chara.GetGoalWork();
                    if (goal is GoalWork goalWork)
                    {
                        goalWork.FindWork(c: chara);
                    }
                }

                if (hasFarmingHobby == true &&
                    !(chara.ai is AIWork_Farm || chara.ai is AI_Farm || chara.ai is AI_Water || chara.ai is AI_Eat))
                {
                    Goal goal = chara.GetGoalHobby();
                    if (goal is GoalHobby goalHobby)
                    {
                        goalHobby.FindWork(c: chara);
                    }
                }
            }
        }
    }
}