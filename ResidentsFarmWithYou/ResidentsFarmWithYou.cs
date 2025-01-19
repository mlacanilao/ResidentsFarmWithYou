using BepInEx;
using HarmonyLib;

namespace ResidentsFarmWithYou
{
    internal static class ModInfo
    {
        internal const string Guid = "omegaplatinum.elin.residentsfarmwithyou";
        internal const string Name = "Residents Farm with You";
        internal const string Version = "1.6.7.0";
    }

    [BepInPlugin(GUID: ModInfo.Guid, Name: ModInfo.Name, Version: ModInfo.Version)]
    internal class ResidentsFarmWithYou : BaseUnityPlugin
    {
        internal static ResidentsFarmWithYou Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
            
            ResidentsFarmWithYouConfig.LoadConfig(config: Config);
        }

        private void Start()
        {
            Harmony.CreateAndPatchAll(type: typeof(Patcher), harmonyInstanceId: ModInfo.Guid);
        }
        
        internal static void Log(object payload)
        {
            Instance?.Logger.LogInfo(data: payload);
        }
    }
}