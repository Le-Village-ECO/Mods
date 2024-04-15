// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.

namespace Eco.Gameplay.Items
{
    using System;
    using Eco.Core.Items;
    using Eco.Gameplay.Players;
    using Eco.Shared.Localization;
    using Eco.Shared.Serialization;
    using Eco.Shared.Networking;
    using Eco.Core.Controller;
    using Eco.Gameplay.Interactions.Interactors;
    using Eco.Shared.Items;
    using Eco.Shared.SharedTypes;

    [Serialized]
    [Compostable]
    [ItemGroup("Carcass")]
    [Tag("Carcass")]
    public abstract partial class CarcassItem : IInteractor
    {
        

        public override LocString Label => Localizer.DoStr("Freshness"); // Override the label to display in the store

        public bool CanStack(Item stackingOntoItem) => true;
        
    }
}
