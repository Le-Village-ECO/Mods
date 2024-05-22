// Le village
// Ajoute un stockage dans l'établi d'outillage (5 emplacements de 5 kits de réparation)
// Erreurs normales car manque le fichier ToolBenchObject.cs dans le projet.
// TODO - Pb à régler (est-ce possible ???) pour que la réparation utilise le stock de la table

using Eco.Gameplay.Components;
using Eco.Gameplay.Components.Storage;
using Eco.Gameplay.Items;
using Eco.Gameplay.Objects;
using Eco.Shared.Localization;

namespace Eco.Mods.TechTree
{
    [RequireComponent(typeof(PublicStorageComponent))]  // Ajoute un stockage
    [RequireComponent(typeof(LinkComponent))]  //Tentative de régler le pb (voir TODO) sans succès
    //[RequireComponent(typeof(InOutLinkedInventoriesComponent))] // a été testé mais finalement inutile car déjà présent avec le Crafting Component
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