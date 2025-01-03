namespace ResidentsFarmWithYou.Patches
{
    public static class ElementContainerPatch
    {
        public static void ModExpPrefix(ElementContainer __instance, int ele, int a, bool chain)
        {
            if (EClass.core?.IsGameStarted == false ||
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
                    if (w.Name.Contains(value: "Farming") == false &&
                        w.Name.Contains(value: "Gardening") == false)
                    {
                        continue;
                    }
                    hasFarmingWork = true;
                }
                
                foreach (Hobby h in chara.ListHobbies())
                {
                    if (h.Name.Contains(value: "Farming") == false &&
                        h.Name.Contains(value: "Gardening") == false)
                    {
                        continue;
                    }
                    hasFarmingHobby = true;
                }

                if (hasFarmingWork == true &&
                    (chara.ai is AIWork_Farm == false || chara.ai is AI_Farm == false || chara.ai is AI_Water == false))
                {
                    Goal goal = chara.GetGoalWork();
                    if (goal is GoalWork goalWork)
                    {
                        goalWork.FindWork(c: chara);
                    }
                }

                if (hasFarmingHobby == true &&
                    (chara.ai is AIWork_Farm == false || chara.ai is AI_Farm == false || chara.ai is AI_Water == false))
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