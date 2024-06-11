using Eco.Core.Utils;
using Eco.Gameplay.Components;
using Eco.Gameplay.Components.Storage;
using Eco.Gameplay.Objects;
using Eco.Gameplay.Property;
using Eco.Gameplay.Rooms;
using Eco.Gameplay.Systems.Messaging.Notifications;
using Eco.Shared.IoC;
using Eco.Shared.Localization;
using Eco.Shared.Serialization;
using Eco.Shared.Utils;
using Eco.Simulation.Settings;
using Eco.Simulation.Time;
using System;

namespace Village.Eco.Mods.Laboratory
{
    [Serialized]
    [RequireComponent(typeof(StatusComponent))]
    public class ResearchComponent : WorldObjectComponent
    {
        //Initialise le component
        public void Initialize(int var1)
        {
            // Initialise le WorldObjectComponent
            base.Initialize();
        }
        public override void Tick() => Tick(ServiceHolder<IWorldObjectManager>.Obj.TickDeltaTime * EcoDef.Obj.TimeMult);

        public void Tick(float deltaTime)
        {
            
        }
    }
}
