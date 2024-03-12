using Eco.Core.Plugins;
using Eco.Core.Plugins.Interfaces;
using Eco.Core.Utils;
using Eco.Shared.Localization;

namespace Village.Eco.Mods.Core
{
    [LocDisplayName("LV Configure Plugin")]
    public class LVConfigurePlugin : IModKitPlugin, IConfigurablePlugin
    {
        private static readonly PluginConfig<LVConfigureConfig> config;
        public IPluginConfig PluginConfig => config;
        public static LVConfigureConfig Config => config.Config;
        static LVConfigurePlugin() => config = new PluginConfig<LVConfigureConfig>("LVConfigure");
        public ThreadSafeAction<object, string> ParamChanged { get; set; } = new ThreadSafeAction<object, string>();
        public object GetEditObject() => config.Config;
        public void OnEditObjectChanged(object o, string param) => this.SaveConfig();

        public string GetCategory() => Localizer.DoStr("LeVillageMods");
        public string GetStatus() => Localizer.DoStr("Active");

    }
}
