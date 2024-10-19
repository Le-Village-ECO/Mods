using Eco.Gameplay.Components;
using Eco.Gameplay.Components.Auth;
using Eco.Gameplay.Interactions.Interactors;
using Eco.Gameplay.Items;
using Eco.Gameplay.Items.Recipes;
using Eco.Gameplay.Objects;
using Eco.Gameplay.Occupancy;
using Eco.Gameplay.Players;
using Eco.Gameplay.Skills;
using Eco.Mods.TechTree;
using Eco.Shared.Items;
using Eco.Shared.Localization;
using Eco.Shared.Math;
using Eco.Shared.Serialization;
using Eco.Shared.SharedTypes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Eco.Mods.RoadObjects
{
    [Serialized]
    [RequireComponent(typeof(StandaloneAuthComponent))]
    [RequireComponent(typeof(PowerGridComponent))]
    [RequireComponent(typeof(PowerConsumptionComponent))]
    [RequireComponent(typeof(OnOffComponent))]
    public partial class TrafficLightObject : WorldObject, IRepresentsItem
    {
        public virtual Type RepresentedItemType => typeof(TrafficLightItem);
        public override LocString DisplayName => Localizer.DoStr("Traffic Signal");
        public override TableTextureMode TableTexture => TableTextureMode.Wood;
        public override bool HasTier => true;
        public override int Tier => 3;

        private int _state = 0;

        [Serialized] private int _startMode = 0;
        private bool _isError = false;
        private bool _isOff = false;

        private List<LightState> _lightStates = new List<LightState>();

        private Dictionary<string, List<LightState>> _modes = new() {
            {
                "Default A",
                new List<LightState>()
                {
                    new LightState() { Name = "Stop", StateLength = TimeSpan.FromSeconds(10), RedLight = true, AmberLight = false, GreenLight = false },
                    new LightState() { Name = "Prepare", StateLength = TimeSpan.FromSeconds(3), RedLight = true, AmberLight = true, GreenLight = false},
                    new LightState() { Name = "Go", StateLength = TimeSpan.FromSeconds(10), RedLight = false, AmberLight = false, GreenLight = true },
                    new LightState() { Name = "Caution", StateLength = TimeSpan.FromSeconds(3), RedLight = false, AmberLight = true, GreenLight = false },
                }
            },
            {
                "Default B",
                new List<LightState>()
                {
                    new LightState() { Name = "Go", StateLength = TimeSpan.FromSeconds(10), RedLight = false, AmberLight = false, GreenLight = true },
                    new LightState() { Name = "Caution", StateLength = TimeSpan.FromSeconds(3), RedLight = false, AmberLight = true, GreenLight = false },
                    new LightState() { Name = "Stop", StateLength = TimeSpan.FromSeconds(10), RedLight = true, AmberLight = false, GreenLight = false },
                    new LightState() { Name = "Prepare", StateLength = TimeSpan.FromSeconds(3), RedLight = true, AmberLight = true, GreenLight = false},
                }
            },
            {
                "Flash Yellow",
                new List<LightState>()
                {
                    new LightState() { Name = "On", StateLength = TimeSpan.FromSeconds(1), RedLight = false, AmberLight = true, GreenLight = false },
                    new LightState() { Name = "Off", StateLength = TimeSpan.FromSeconds(1), RedLight = false, AmberLight = false, GreenLight = false},
                }
            },
            {
                "Flash Red",
                new List<LightState>()
                {
                    new LightState() { Name = "On", StateLength = TimeSpan.FromSeconds(1), RedLight = true, AmberLight = false, GreenLight = false },
                    new LightState() { Name = "Off", StateLength = TimeSpan.FromSeconds(1), RedLight = false, AmberLight = false, GreenLight = false},
                }
            }
        };
        
        protected override void Initialize()
        {
            this.GetComponent<PowerGridComponent>().Initialize(10, new ElectricPower());
            this.GetComponent<PowerConsumptionComponent>().Initialize(20f);
        }
        protected override void PostInitialize()
        {
            var initMode = _modes.ElementAtOrDefault(_startMode);

            if (initMode.IsDefault())
            {
                _isError = true;
            }
            else
            {
                _lightStates = _modes.ElementAt(_startMode).Value;
            }

            if (_startMode > _modes.Count)
                _startMode = 0;

            base.PostInitialize();
        }

        static TrafficLightObject()
        {
            var BlockOccupancyList = new List<BlockOccupancy>
            {
            new BlockOccupancy(new Vector3i(0, 0, 0)),
            };
            AddOccupancy<TrafficLightObject>(BlockOccupancyList);
        }

        /*
        public override InteractResult OnActLeft(InteractionContext context)
        {
            return base.OnActLeft(context);
        }

        public override InteractResult OnActRight(InteractionContext context)
        {            
            var maxState = _modes.Count;

            if (++_startMode >= maxState)
            {
                _startMode = 0;
            }

            var selectedMode = _modes.ElementAt(_startMode);
            _lightStates = selectedMode.Value;
            return InteractResult.SuccessWithReason(Localizer.DoStr("Set Start Mode to " + selectedMode.Key));
        }
        */
        [Interaction(InteractionTrigger.RightClick, "Switch mode", priority: -2, authRequired: AccessType.ConsumerAccess)]
        public void OnActSwitch(Player player, InteractionTriggerInfo triggerInfo, InteractionTarget target) 
        {
            var maxState = _modes.Count;

            if (++_startMode >= maxState)
            {
                _startMode = 0;
            }

            var selectedMode = _modes.ElementAt(_startMode);
            _lightStates = selectedMode.Value;
            player.MsgLocStr($"Set Start Mode to {selectedMode.Key}");
        } 

        public override void Tick()
        {
            base.Tick();

            if (this.Operating && !_isError)
            {
                _isOff = false;

                var now = DateTime.Now.Ticks;
                var totalStateLength = _lightStates.Sum(x => x.StateLength.Ticks);

                var relativeTick = now % totalStateLength;

                var expectedState = GetExpectedState(relativeTick);

                if (_state != expectedState)
                {
                    _state = expectedState;
                    SetLightFromState();
                }
            }
            
            if (!this.Operating && !_isOff)
            {
                //Log.Debug("Not operating, and state is not set as off.");
                _isOff = true;
                SetLightFromState();
            }
        }

        private int GetExpectedState(long relativeTick)
        {
            long start = 0;

            for (int i = 0; i < _lightStates.Count; i++)
            {
                var state = _lightStates[i];

                var end = start + state.StateLength.Ticks;

                if (relativeTick >= start && relativeTick < end)
                {
                    return i;
                }

                start = end;
            }

            _isError = true;
            return -1;
        }

        private void SetLightFromState()
        {
            if (_isError)
            {
                SetAnimatedState("LightRed", true);
                SetAnimatedState("LightAmber", true);
                SetAnimatedState("LightGreen", true);
            }
            else if(_isOff)
            { 

                SetAnimatedState("LightRed", false);
                SetAnimatedState("LightAmber", false);
                SetAnimatedState("LightGreen", false);
            }
            else
            {
                var state = _lightStates.ElementAtOrDefault(_state);

                if (state is null)
                {
                    //Log.Debug($"Traffic Light did not find state with Id {_state}");
                    _isError = true;
                    SetLightFromState();
                }
                else
                {
                    SetAnimatedState("LightRed", state.RedLight);
                    SetAnimatedState("LightAmber", state.AmberLight);
                    SetAnimatedState("LightGreen", state.GreenLight);
                }
            }
        }
    }

    [Serialized]
    [LocDisplayName("Traffic Light")]
    [LocDescription("Should I stay or should i go")]
    [Weight(1000)]
    [MaxStackSize(1)]
    public class TrafficLightItem : WorldObjectItem<TrafficLightObject>
    {
        protected override OccupancyContext GetOccupancyContext => new SideAttachedContext(0 | DirectionAxisFlags.Down, WorldObject.GetOccupancyInfo(this.WorldObjectType));

    }

    [RequiresSkill(typeof(BasicEngineeringSkill), 3)]
    public partial class TrafficLightRecipe : RecipeFamily
    {
        public TrafficLightRecipe()
        {
            var recipe = new Recipe();
            recipe.Init(
                name: "TrafficLight",
                displayName: Localizer.DoStr("Traffic Light"),
                ingredients: new List<IngredientElement>()
                {
                    new IngredientElement(typeof(LightBulbItem), 3, true),
                    new IngredientElement(typeof(BasicCircuitItem), 2, true),
                    new IngredientElement(typeof(PlasticItem), 3, false),
                },
                items: new List<CraftingElement>
                {
                    new CraftingElement<TrafficLightItem>(1)
                });

            this.Recipes = new List<Recipe> { recipe };
            this.ExperienceOnCraft = 0.5f;

            this.LaborInCalories = CreateLaborInCaloriesValue(200, typeof(BasicEngineeringSkill));
            this.CraftMinutes = CreateCraftTimeValue(beneficiary: typeof(TrafficLightRecipe), start: 1.5f, skillType: typeof(BasicEngineeringSkill));

            this.ModsPreInitialize();
            this.Initialize(displayText: Localizer.DoStr("Traffic Light"), recipeType: typeof(TrafficLightRecipe));
            this.ModsPostInitialize();

            CraftingComponent.AddRecipe(tableType: typeof(ElectronicsAssemblyObject), recipe: this);
        }

        partial void ModsPreInitialize();

        partial void ModsPostInitialize();
    }

    public class LightState
    {
        public string Name { get; init; }

        public TimeSpan StateLength { get; init; }

        public bool RedLight { get; init; } = false;
        public bool AmberLight { get; init; } = false;
        public bool GreenLight { get; init; } = false;

    }
}

public static class Extensions
{
    public static bool IsDefault<T>(this T value) where T : struct
    {
        bool isDefault = value.Equals(default(T));

        return isDefault;
    }
}