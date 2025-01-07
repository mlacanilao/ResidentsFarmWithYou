using BepInEx.Configuration;

namespace ResidentsFarmWithYou
{
    internal static class ResidentsFarmWithYouConfig
    {
        internal static ConfigEntry<bool> EnableAutoPlaceFarmingItems;
        internal static ConfigEntry<bool> EnableFertilizer;

        internal static void LoadConfig(ConfigFile config)
        {
            EnableAutoPlaceFarmingItems = config.Bind(
                section: ModInfo.Name,
                key: "Enable Auto Place Farming Items",
                defaultValue: false,
                description: "Enable or disable automatically placing farming items into shared containers.\n" +
                             "Set to 'true' to enable automatic placement, or 'false' to disable it.\n" +
                             "農作物アイテムを共有コンテナに自動的に配置する機能を有効または無効にします。\n" +
                             "'true' に設定すると自動配置が有効になり、'false' に設定すると無効になります。\n" +
                             "启用或禁用自动将农作物物品放入共享容器。\n" +
                             "设置为 'true' 以启用自动放置，或设置为 'false' 禁用此功能。"
            );
            
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
        }
    }
}