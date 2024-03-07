using System.Linq;
using System.Reflection;
using Eco.Gameplay.Players;
using Eco.Shared.Utils;

namespace ExhaustionMod
{
    //Le village
    public static class ExhaustionMonitorExtensions
    {
        public static void AddEnergy(this ExhaustionMonitor monitor, float hours)
        {
            var exhaustionMonitorFields = typeof(ExhaustionMonitor).GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
            var extraEnergySecondsFieldInfo = exhaustionMonitorFields.First(p => p.Name == "ChangeSets");
            var extraEnergySecondsFieldValue = (double)extraEnergySecondsFieldInfo.GetValue(monitor)!;

            monitor.Energize(hours);

            var extendedExtraEnergySeconds = extraEnergySecondsFieldValue + TimeUtil.HoursToSeconds(hours);
            extraEnergySecondsFieldInfo.SetValue(monitor, extendedExtraEnergySeconds);

            monitor.Tick();
        }
    }
}