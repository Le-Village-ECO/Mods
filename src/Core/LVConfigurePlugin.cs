// Le Village - Plugin de configuration des mods

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
        public ThreadSafeAction<object, string> ParamChanged { get; set; } = new ThreadSafeAction<object, string>();

        static LVConfigurePlugin() => config = new PluginConfig<LVConfigureConfig>("LVConfigure");
        public object GetEditObject() => config.Config;
        public void OnEditObjectChanged(object o, string param) => this.SaveConfig();
        public static void Save() => config.SaveAsync().Wait();

        public string GetCategory() => Localizer.DoStr("LeVillageMods");
        public override string ToString() => Localizer.DoStr("Configuration");
        public string GetStatus() => Localizer.DoStr("Active");
    }
}