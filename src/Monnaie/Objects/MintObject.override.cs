// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.

namespace Eco.Mods.TechTree
{
    using Eco.Gameplay.Components;
    using Eco.Gameplay.Economy;
    using Eco.Gameplay.Items;
    using Eco.Gameplay.Objects;
    using Eco.Gameplay.Systems.TextLinks;
    using Eco.Shared.Serialization;
    using Eco.Gameplay.Items.PersistentData;
    using Village.Eco.Mods.Monnaie;

    [RequireComponent(typeof(MintComponent))]
    public partial class MintObject : WorldObject { }

    [MaxStackSize(1)]
    public partial class MintItem : WorldObjectItem<MintObject>
    {
    }
}
