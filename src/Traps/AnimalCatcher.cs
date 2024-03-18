// Le Village
// Le catcher permet de définir une liste d'animaux qui peuvent être pris par un piège

using System;
using System.Collections.Generic;
using Eco.Core.Utils;
using Eco.Gameplay.Objects;
using Eco.Gameplay.Players;
using Eco.Mods.Organisms.SpeciesCatchers;
using Eco.Mods.TechTree;
using Eco.Shared.Utils;

namespace Village.Eco.Mods.Traps
{
    public class SmallAnimalCatcher : TrapCatcher
    {
        public override ThreadSafeList<string> DefaultTargetSpecies => new(AnimalCatchTarget.SmallAnimalTarget);
        public override TimeSpan NextCatchDelay => TimeSpan.FromMinutes(RandomUtil.Range(5, 10));

        public SmallAnimalCatcher(User user, WorldObject obj) : base(user, obj) { }
        public SmallAnimalCatcher() { }
    }
    public class FoxCatcher : TrapCatcher
    {
        public override ThreadSafeList<string> DefaultTargetSpecies => new(AnimalCatchTarget.FoxTarget);
        public override TimeSpan NextCatchDelay => TimeSpan.FromMinutes(RandomUtil.Range(5, 10));

        public FoxCatcher(User user, WorldObject obj) : base(user, obj) { }
        public FoxCatcher() { }
    }

    public static class AnimalCatchTarget
    {
        // Liste des petits animaux : agouti, lièvre 
        public static readonly List<string> SmallAnimalTarget = new()
        {
            nameof(Agouti),
            nameof(Hare),
        };

        // Seulement le renard
        public static readonly List<string> FoxTarget = new() { nameof(Fox) };
    }
}
