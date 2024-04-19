// Le Village
// Ajout d'une restriction qui empêche les carcasses (FoodItem) d'aller dans les glacières,frigo et frigo industriel

namespace Eco.Mods.TechTree
{
    using System.Collections.Generic;
    using System.Linq;
    using Eco.Gameplay.Components.Storage;
    using Eco.Gameplay.Items;
    using Eco.Gameplay.Objects;
    using Eco.Shared.Localization;
    using Eco.Shared.Utils;

    public partial class IceboxObject : WorldObject
    {
        protected override void PostInitialize()
        {
            base.PostInitialize();
            this.GetComponent<PublicStorageComponent>().Storage.AddInvRestriction(new FoodStorageRestriction());
            this.GetComponent<PublicStorageComponent>().Storage.AddInvRestriction(new DisallowedTagRestriction("Carcasse")); //LV pas de tag "carcasse" autorisé
        }
    }

    public partial class RefrigeratorObject : WorldObject
    {
        protected override void PostInitialize()
        {
            base.PostInitialize();
            this.GetComponent<PublicStorageComponent>().Storage.AddInvRestriction(new FoodStorageRestriction());
            this.GetComponent<PublicStorageComponent>().Storage.AddInvRestriction(new DisallowedTagRestriction("Carcasse")); //LV pas de tag "carcasse" autorisé
        }
    }

    public partial class IndustrialRefrigeratorObject : WorldObject
    {
        protected override void PostInitialize()
        {
            base.PostInitialize();
            this.GetComponent<PublicStorageComponent>().Storage.AddInvRestriction(new FoodStorageRestriction());
            this.GetComponent<PublicStorageComponent>().Storage.AddInvRestriction(new DisallowedTagRestriction("Carcasse")); //LV pas de tag "carcasse" autorisé
        }
    }

    /// <summary>Restricts the inventory to only accept items that match the given tags.</summary>
    public class DisallowedTagRestriction : InventoryRestriction
    {
        private readonly List<string> disallowedTags;

        public DisallowedTagRestriction(params string[] disallowedTags) => this.disallowedTags = new List<string>(disallowedTags);
        public DisallowedTagRestriction(IEnumerable<Tag> disallowedTags) => this.disallowedTags = new List<string>(disallowedTags.Select(tag => tag.Name));

        public override LocString Message => Localizer.Do($"Inventory doesn't accept {this.disallowedTags.Select(x => TagManager.Tag(x).MarkedUpName).CommaList()}.");
        public override int MaxAccepted(Item item, int currentQuantity) => item.Tags().Any(x => this.disallowedTags.Contains(x.Name)) ? 0 : -1;
    }
}
