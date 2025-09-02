using System.IO;
using BepInEx.Configuration;

namespace ResidentsFarmWithYou
{
    internal static class ResidentsFarmWithYouConfig
    {
        internal static ConfigEntry<bool> EnableFertilizer;
        internal static ConfigEntry<bool> EnableRequireFertilizer;
        internal static ConfigEntry<bool> EnableSeedLevelMatch;
        
        internal static string XmlPath { get; private set; }
        internal static string TranslationXlsxPath { get; private set; }
        
        internal static void LoadConfig(ConfigFile config)
        {
            EnableFertilizer = config.Bind(
                section: ModInfo.Name,
                key: "Enable Fertilizer",
                defaultValue: true,
                description:
                    "Enable or disable residents applying fertilizer to crops.\n" +
                    "Set to 'true' to allow residents to apply fertilizer during farming, or 'false' to prevent them from using fertilizer.\n" +
                    "住人が作物に肥料を施すかどうかを設定します。\n" +
                    "'true' に設定すると住人が農作業中に肥料を施し、'false' では施しません。\n" +
                    "启用或禁用居民为作物施肥。\n" +
                    "设置为 'true' 时，居民会在农作时施肥；设置为 'false' 时，他们不会施肥。"
            );
            
            EnableRequireFertilizer = config.Bind(
                section: ModInfo.Name,
                key: "Enable Require Fertilizer",
                defaultValue: false,
                description: 
                    "Enable or disable requiring residents to use the player's fertilizer for farming.\n" +
                    "Set to 'true' to require fertilizer, or 'false' to allow farming without it.\n" +
                    "住人が農作物を育てる際にプレイヤーの肥料を使用するかどうかを設定します。\n" +
                    "'true' に設定すると肥料が必要になり、'false' に設定すると肥料がなくても育てられます。\n" +
                    "启用或禁用要求居民使用玩家的肥料才能种植农作物。\n" +
                    "设置为 'true' 以需要肥料，或设置为 'false' 允许无肥料种植。"
            );
            
            EnableSeedLevelMatch = config.Bind(
                section: ModInfo.Name,
                key: "Enable Seed Level Match",
                defaultValue: true,
                description:
                    "Enable or disable residents setting the planted seed level to match the player's farming skill level.\n" +
                    "Set to 'true' to have residents adjust seeds to the player's farming skill level when planting, or 'false' to leave the seed level unchanged.\n" +
                    "住人が植え付け時に種のレベルをプレイヤーの農業スキルレベルに合わせるかどうかを設定します。\n" +
                    "'true' に設定すると、住人が種を植える際にプレイヤーの農業スキルレベルに設定されます。'false' では変更しません。\n" +
                    "启用或禁用居民在播种时将种子等级匹配为玩家的农业技能等级。\n" +
                    "设置为 'true' 时，居民在播种时会将种子等级设为玩家的农业技能等级；设置为 'false' 则不更改。"
            );
        }
        
        internal static void InitializeXmlPath(string xmlPath)
        {
            if (File.Exists(path: xmlPath))
            {
                XmlPath = xmlPath;
            }
            else
            {
                XmlPath = string.Empty;
            }
        }
        
        internal static void InitializeTranslationXlsxPath(string xlsxPath)
        {
            if (File.Exists(path: xlsxPath))
            {
                TranslationXlsxPath = xlsxPath;
            }
            else
            {
                TranslationXlsxPath = string.Empty;
            }
        }
    }
}