using Eco.Core.Plugins;
using Eco.Core.Plugins.Interfaces;
using Eco.Shared.Localization;
using Eco.Shared.Utils;

using System.Diagnostics.CodeAnalysis;

namespace TradeAssistant
{
    internal class TradeAssistantPlugin : Singleton<TradeAssistantPlugin>, IModKitPlugin, ISaveablePlugin
    {
        [NotNull] private readonly TradeAssistantData data;
        public TradeAssistantPlugin()
        {
            try { data = StorageManager.LoadOrCreate<TradeAssistantData>("TradeAssistant"); }
            catch
            {
                foreach (var fileName in StorageManager.GetFiles("TradeAssistant"))
                    StorageManager.Delete(fileName);

                data = StorageManager.LoadOrCreate<TradeAssistantData>("TradeAssistant");
            }
        }
        public string GetCategory() => Localizer.DoStr("Mods");
        public string GetStatus() => string.Empty;

        public void SaveAll()
        {
            StorageManager.Obj.MarkDirty(data);
        }
    }
}