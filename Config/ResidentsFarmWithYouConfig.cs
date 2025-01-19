using BepInEx.Configuration;

namespace ResidentsFarmWithYou
{
    internal static class ResidentsFarmWithYouConfig
    {
        internal static ConfigEntry<bool> EnableFertilizer;
        internal static ConfigEntry<bool> EnableRequireFertilizer;

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
        }
    }
}