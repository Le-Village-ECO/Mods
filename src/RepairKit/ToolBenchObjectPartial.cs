// Le village
// Ajoute un stockage dans l'établi d'outillage (5 emplacements de 5 kits de réparation)
// Erreurs normales car manque le fichier ToolBenchObject.cs dans le projet.

using Eco.Gameplay.Components;
using Eco.Gameplay.Components.Storage;
using Eco.Gameplay.Items;
using Eco.Gameplay.Objects;
using Eco.Shared.Localization;

namespace Eco.Mods.TechTree
{
    [RequireComponent(typeof(PublicStorageComponent))]  // Ajoute un stockage
    public partial class ToolBenchObject
    {
        partial void ModsPreInitialize()
        {
            this.GetComponent<MinimapComponent>().SetCategory(Localizer.DoStr("Crafting"));
            
            // Ajout d'un stockage pour les kits de réparation
            var storage = this.GetComponent<PublicStorageComponent>();
            storage.Initialize(5);                                              // 5 emplacements
            storage.Storage.AddInvRestriction(new StackLimitRestriction(5));    // 5 objets par emplacement
            storage.Storage.AddInvRestriction(new TagRestriction("RepairKit")); // Uniquement les Kits de réparation
        }
    }
}