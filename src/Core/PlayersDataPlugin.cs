using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Eco.Core.Plugins;
using Eco.Core.Plugins.Interfaces;
using Eco.Core.Utils;
using Eco.Gameplay.Players;
using Eco.Shared.Localization;

namespace Village.Eco.Mods.Core
{
    public class PlayersDataPlugin : IShutdownablePlugin, IModKitPlugin
    {
        private PluginConfig<PlayersDataConfig> config;
        public PlayersDataPlugin() => config = new PluginConfig<PlayersDataConfig>("PlayersData");

        public Task ShutdownAsync() => config.SaveAsync();
        public string GetCategory() => Localizer.DoStr("PlayersDataConfig");
        public string GetStatus() => Localizer.DoStr("PlayersDataConfig");

        public static string GetPlayerId(Player player)
            => player.User.SteamId?.IfEmpty(player.User.SlgId) ?? player.User.SlgId;

        public PlayerData GetPlayerDataOrDefault(Player player)
            => config.Config.PlayerDataPerId.GetValueOrDefault(GetPlayerId(player)) ?? new();

        public void AddOrSetPlayerData(Player player, PlayerData data)
        {
            if (config.Config.PlayerDataPerId.TryAdd(GetPlayerId(player), data) is false)
                config.Config.PlayerDataPerId[GetPlayerId(player)] = data;
        }
    }

    public class PlayersDataConfig
    {
        [LocDescription("Data per player Id")]
        public Dictionary<string, PlayerData> PlayerDataPerId { get; set; } = new();
    }

    public partial class PlayerData { }
}