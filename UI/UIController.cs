using System.IO;
using System.Reflection;
using BepInEx;
using EvilMask.Elin.ModOptions;
using EvilMask.Elin.ModOptions.UI;

namespace ResidentsFarmWithYou
{
    public class UIController
    {
        public static void RegisterUI()
        {
            foreach (var obj in ModManager.ListPluginObject)
            {
                if (obj is BaseUnityPlugin plugin && plugin.Info.Metadata.GUID == ModInfo.ModOptionsGuid)
                {
                    var controller = ModOptionController.Register(guid: ModInfo.Guid, tooptipId: "mod.tooltip");
                    
                    var assemblyLocation = Path.GetDirectoryName(path: Assembly.GetExecutingAssembly().Location);
                    var xmlPath = Path.Combine(path1: assemblyLocation, path2: "ResidentsFarmWithYouConfig.xml");
                    ResidentsFarmWithYouConfig.InitializeXmlPath(xmlPath: xmlPath);
            
                    var xlsxPath = Path.Combine(path1: assemblyLocation, path2: "translations.xlsx");
                    ResidentsFarmWithYouConfig.InitializeTranslationXlsxPath(xlsxPath: xlsxPath);
                    
                    if (File.Exists(path: ResidentsFarmWithYouConfig.XmlPath))
                    {
                        using (StreamReader sr = new StreamReader(path: ResidentsFarmWithYouConfig.XmlPath))
                            controller.SetPreBuildWithXml(xml: sr.ReadToEnd());
                    }
                    
                    if (File.Exists(path: ResidentsFarmWithYouConfig.TranslationXlsxPath))
                    {
                        controller.SetTranslationsFromXslx(path: ResidentsFarmWithYouConfig.TranslationXlsxPath);
                    }
                    
                    RegisterEvents(controller: controller);
                }
            }
        }

        private static void RegisterEvents(ModOptionController controller)
        {
            controller.OnBuildUI += builder =>
            {
                var enableFertilizerToggle = builder.GetPreBuild<OptToggle>(id: "enableFertilizerToggle");
                enableFertilizerToggle.Checked = ResidentsFarmWithYouConfig.EnableFertilizer.Value;
                enableFertilizerToggle.OnValueChanged += isChecked =>
                {
                    ResidentsFarmWithYouConfig.EnableFertilizer.Value = isChecked;
                };
                
                var enableRequireFertilizerToggle = builder.GetPreBuild<OptToggle>(id: "enableRequireFertilizerToggle");
                enableRequireFertilizerToggle.Checked = ResidentsFarmWithYouConfig.EnableRequireFertilizer.Value;
                enableRequireFertilizerToggle.OnValueChanged += isChecked =>
                {
                    ResidentsFarmWithYouConfig.EnableRequireFertilizer.Value = isChecked;
                };
                
                var enableEqualizePlantsToggle = builder.GetPreBuild<OptToggle>(id: "enableEqualizePlantsToggle");
                enableEqualizePlantsToggle.Checked = ResidentsFarmWithYouConfig.EnableEqualizePlants.Value;
                enableEqualizePlantsToggle.OnValueChanged += isChecked =>
                {
                    ResidentsFarmWithYouConfig.EnableEqualizePlants.Value = isChecked;
                };
            };
        }
    }
}