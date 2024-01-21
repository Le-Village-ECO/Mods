using Eco.Core.Controller;
using Eco.Gameplay.Components;
using Eco.Gameplay.Components.Storage;
using Eco.Gameplay.Objects;
using Eco.Gameplay.Players;
using Eco.Mods.TechTree;
using Eco.Shared.Serialization;
using System.Collections.Generic;
using System.Linq;

namespace VehicleModifiers
{
    [NoIcon]
    [RequireComponent(typeof(VehicleComponent), null)]
    [RequireComponent(typeof(FuelSupplyComponent), null)]
    [RequireComponent(typeof(FuelConsumptionComponent), null)]
    [Serialized]
    public class VehicleModifiersComponent : WorldObjectComponent
    {
        private float RegularMaxSpeed { get; set; }
        private float EfficiencyMultiplier { get; set; }
        private int Seats { get; set; }
        private VehicleComponent Vehicle { get; set; }
        private FuelSupplyComponent FuelSupply { get; set; }
        private FuelConsumptionComponent FuelConsumption { get; set; }
        private float? RegularJoulesPerSecond { get; set; }

        private float PreviousSpeedMultiplier { get; set; }

        public void Initialize(float maxSpeed, float efficiencyMultiplier, int seats = 1, string controlHints = null, bool isDrivenUnderwater = false)
        {
            PreviousSpeedMultiplier = 1f;
            RegularMaxSpeed = maxSpeed;
            EfficiencyMultiplier = efficiencyMultiplier;
            Seats = seats;
            Vehicle = Parent.GetComponent<VehicleComponent>();
            FuelSupply = Parent.GetComponent<FuelSupplyComponent>();
            FuelConsumption = Parent.GetComponent<FuelConsumptionComponent>();

            Vehicle.Initialize(maxSpeed, efficiencyMultiplier, seats, controlHints, isDrivenUnderwater);
            FuelSupply.Inventory.OnChanged.Add(OnInventoryChanged);
            UpdateSpeedAndFuelConsumption();
        }

        private void OnInventoryChanged(User _user)
        {
            UpdateSpeedAndFuelConsumption();
        }

        private void UpdateSpeedAndFuelConsumption()
        {
            try
            {
                float speedMultiplier = GetSpeedMultiplier();
                if (PreviousSpeedMultiplier != speedMultiplier)
                {
                    PreviousSpeedMultiplier = speedMultiplier;

                    RegularJoulesPerSecond ??= FuelConsumption.JoulesPerSecond;
                    List<(Player, int)> mounts = Vehicle.Mounts.MountedPlayers.Select(player => (player, Vehicle.Mounts.GetPlayerSeat(player))).ToList();
                    Vehicle.Initialize(RegularMaxSpeed * speedMultiplier, EfficiencyMultiplier, Seats, Vehicle.ControlHints, Vehicle.IsDrivenUnderwater);
                    foreach ((Player player, int seatIndex) in mounts)
                    {
                        Vehicle.Mounts.MountSeat(seatIndex, player);
                    }
                    FuelConsumption.JoulesPerSecond = RegularJoulesPerSecond.Value * speedMultiplier;
                }
            }
            finally { }
        }

        private float GetSpeedMultiplier()
        {
            if (FuelSupply.CurrentFuel is BiodieselItem biodiesel)
            {
                return biodiesel.VehicleSpeedMultiplier;
            }
            else if (FuelSupply.CurrentFuel is CharcoalItem charcoal)
            {
                return charcoal.VehicleSpeedMultiplier;
            }
            else
            {
                return 1f;
            }
        }
    }
}
