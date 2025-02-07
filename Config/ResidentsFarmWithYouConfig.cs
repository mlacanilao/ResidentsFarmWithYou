using System.IO;
using BepInEx.Configuration;

namespace ResidentsFarmWithYou
{
    internal static class ResidentsFarmWithYouConfig
    {
        internal static ConfigEntry<bool> EnableFertilizer;
        internal static ConfigEntry<bool> EnableRequireFertilizer;
        internal static ConfigEntry<bool> EnableEqualizePlants;

        
        internal static string XmlPath { get; private set; }
        internal static string TranslationXlsxPath { get; private set; }
        
        internal static void LoadConfig(ConfigFile config)
        {
            EnableFertilizer = config.Bind(
                section: ModInfo.Name,
                key: "Enable Fertilizer",
                defaultValue: true,
                description: "Enable or disable the use of fertilizer.\n" +
                             "Set to 'true' to enable fertilizer, or 'false' to disable it.\n" +
                             "肥料の使用を有効または無効にします。\n" +
                             "'true' に設定すると肥料が有効になり、'false' に設定すると無効になります。\n" +
                             "启用或禁用肥料的使用。\n" +
                             "设置为 'true' 启用肥料，设置为 'false' 禁用肥料。"
            );
            
            EnableRequireFertilizer = config.Bind(
                section: ModInfo.Name,
                key: "Enable Require Fertilizer",
                defaultValue: false,
                description: "Enable or disable requiring residents to use the player's fertilizer for farming.\n" +
                             "Set to 'true' to require fertilizer, or 'false' to allow farming without it.\n" +
                             "住人が農作物を育てる際にプレイヤーの肥料を使用するかどうかを設定します。\n" +
                             "'true' に設定すると肥料が必要になり、'false' に設定すると肥料がなくても育てられます。\n" +
                             "启用或禁用要求居民使用玩家的肥料才能种植农作物。\n" +
                             "设置为 'true' 以需要肥料，或设置为 'false' 允许无肥料种植。"
            );
            
            EnableEqualizePlants = config.Bind(
                section: ModInfo.Name,
                key: "Enable Equalize Plants",
                defaultValue: false,
                description: "Enable or disable the equalization of plants in neighboring tiles.\n" +
                             "Equalization means that plants in neighboring tiles will share the same growth level and seed type if they match certain conditions.\n" +
                             "Set to 'true' to enable equalization, or 'false' to disable it.\n" +
                             "隣接するタイルの植物を均等化するかどうかを有効または無効にします。\n" +
                             "均等化とは、特定の条件を満たす場合、隣接するタイルの植物が同じ成長レベルや種子の種類を共有することを意味します。\n" +
                             "'true' に設定すると均等化が有効になり、'false' に設定すると無効になります。\n" +
                             "启用或禁用邻近地块植物的均衡化。\n" +
                             "均衡化意味着如果满足某些条件，邻近地块的植物将共享相同的生长水平和种子类型。\n" +
                             "设置为 'true' 启用均衡化，设置为 'false' 禁用均衡化。\n"
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