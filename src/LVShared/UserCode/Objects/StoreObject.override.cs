// Copyright (c) Strange Loop Games. All rights reserved.
// See LICENSE file in the project root for full license information.
// Le Village - Ajout de l'extension StoreExtComponent - MISE EN COMMENTAIRE EN ATTENTE SOLUTION POUR INTEGRER TRADE ASSISTANT

namespace Eco.Mods.TechTree
{
    using Eco.Core.Controller;
    using Eco.Core.Utils;
    using Eco.Gameplay.Aliases;
    using Eco.Gameplay.Property;
    using Eco.Gameplay.Components;
    using Eco.Gameplay.Components.Auth;
    using Eco.Gameplay.Economy;
    using Eco.Gameplay.GameActions;
    using Eco.Gameplay.Objects;
    using Eco.Shared.Networking;
    using Eco.Gameplay.Components.Store;
    using LVShared.UserCode.LVMods.Utils;

    //Attributes must remain in this order: (SharedLinkComponent, StoreComponent) to avoid double Update calls for notification messages
    [RequireComponent(typeof(SharedLinkComponent))]
    [RequireComponent(typeof(StoreComponent))]
    //[RequireComponent(typeof(StoreExtComponent))]  //Le Village
    public partial class StoreObject : WorldObject, INullCurrencyAllowed
    {
        protected override void OnCreatePostInitialize()
        {
            base.OnCreatePostInitialize();
            this.GetComponent<PropertyAuthComponent>().SetPublic(); // so everyone can acess store by default
        }
    }

    /// <summary> This represents wood shop cart. It's a special shop which you can move by pulling it. Implements IFreezable, so when nobody pull it, it won't move anyway. </summary>
    [RequireComponent(typeof(StoreComponent))]
    public partial class WoodShopCartObject : PhysicsWorldObject, INullCurrencyAllowed, IFreezable, ICanOverrideAuth
    {
#region IFreezable
        public NetPhysicsEntity NetEntity => (NetPhysicsEntity)this.netEntity;
        public float GroundDistance { get; set; } //Used if ground is goes away from store so it can wake up and fall.
        #endregion
        protected override void PostInitialize()
        {
            base.PostInitialize();
            this.GetComponent<VehicleComponent>().SetDrivableFunc(this.Drivable);
            this.GetComponent<OnOffComponent>().Subscribe(nameof(OnOffComponent.On), this.NotifyDrivingChange);
        }

        //Store is drivable when it's in deactivated mode. 
        bool Drivable() => this.GetComponent<OnOffComponent>().On == false;
        void NotifyDrivingChange() => this.GetComponent<VehicleComponent>().Changed(nameof(VehicleComponent.Drivable));
        protected override void OnCreatePostInitialize()
        {
            base.OnCreatePostInitialize();
            this.GetComponent<OnOffComponent>().SetOnOff(null, false); //Shop truck starts in mode vehicle, user must to explicitly put it as store
        }

        //Allow everybody to access shop cart. Only costumers will be able to drive or access storage.
        public LazyResult ShouldOverrideAuth(IAlias alias, IOwned property, GameAction action)
        {
            //This is done this way because for now it's impossible to have freedom in auth setuping. Customer by default means access to storage, to vehicle etc. 
            //So before auth refactoring is done, it allows to buy to anybody, so you dont have to put people as a customer giving them too much access. TODO: rework after auth refactoring https://github.com/StrangeLoopGames/Eco/pull/10620
            if (action is QueryAction || action is OpenAction || action is TradeAction) 
                return LazyResult.Succeeded;
            return LazyResult.FailedNoMessage;
        }
    }
}
