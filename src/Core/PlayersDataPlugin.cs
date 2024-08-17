using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Eco.Core.Plugins;
using Eco.Core.Plugins.Interfaces;
using Eco.Core.Utils;
using Eco.Gameplay.Players;
using Eco.Shared.Localization;
using Eco.Shared.Serialization;

namespace Village.Eco.Mods.Core
{
    public class PlayersDataPlugin : IShutdownablePlugin, IModKitPlugin
    {
        private readonly PluginConfig<PlayersDataConfig> config;
        public PlayersDataPlugin() => config = new PluginConfig<PlayersDataConfig>("PlayersData");

        public Task ShutdownAsync() => config.SaveAsync();
        public string GetCategory() => Localizer.DoStr("LeVillageMods");
        public override string ToString() => Localizer.DoStr("Players Data");
        public string GetStatus() => Localizer.DoStr("Active");

        public static string GetUserId(User user)
            => user.SteamId?.IfEmpty(user.StrangeId) ?? user.StrangeId;
        public static string GetPlayerId(Player player) => GetUserId(player.User);
        
        public PlayerData GetUserDataOrDefault(User user)
            => config.Config.PlayerDataPerId.GetValueOrDefault(GetUserId(user)) ?? new();
        public PlayerData GetPlayerDataOrDefault(Player player) => GetUserDataOrDefault(player.User);

        public void AddOrSetUserData(User user, PlayerData data)
        {
            if (config.Config.PlayerDataPerId.TryAdd(GetUserId(user), data) is false)
                config.Config.PlayerDataPerId[GetUserId(user)] = data;

            Task.Run(() => config.SaveAsync());
        }
        public void AddOrSetPlayerData(Player player, PlayerData data) => AddOrSetUserData(player.User, data);
    }

    public class PlayersDataConfig
    {
        [LocDescription("Data per player Id")]
        public Dictionary<string, PlayerData> PlayerDataPerId { get; set; } = new();
    }

    public partial class PlayerData
    {
        [Serialized] public double LastUnspecializingDay { get; set; } = 0; //Unskill mod
        [Serialized] public double LastDailyBoost { get; set; } = 0; //Exhaustion mod
        [Serialized] public bool BoostWE { get; set; } = false; //Boost Week-end
    }
}