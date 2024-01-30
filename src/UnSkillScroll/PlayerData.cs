using System;
using Eco.Shared.Serialization;

namespace Village.Eco.Mods.Core
{
    public partial class PlayerData
    {
        [Serialized] public double LastUnspecializingDay { get; set; } = 0;
    }
}