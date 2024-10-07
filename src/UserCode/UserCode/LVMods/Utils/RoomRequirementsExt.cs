// Le village - Attribut pour WorldObject pour contrôler la valeur de la pièce.
// Dans l'objet, il faut ajouter cet attribut : [RequireRoomValue(1.0f)] (exemple)
//
// TODO : Tenter de finir le check sur la category de la piece
// Note sur ce TODO : il semble devoir recopier un gros bout de code du jeu car il n'y a pas de valeur facilement accessible au niveau de la Room
// De plus, il est toujours possible d'utiliser le HousingComponent qui contrôle la cohérence de l'objet avec le type de la pièce.

using Eco.Gameplay.Housing;
using Eco.Gameplay.Housing.PropertyValues;
using Eco.Gameplay.Players;
using Eco.Gameplay.Property;
using Eco.Gameplay.Rooms;
using Eco.Shared.Localization;
using Eco.Shared.Utils;
using Eco.Stats;
using System.Linq;

namespace Village.Eco.Mods.Core
{
    public class RequireRoomValueAttribute : RoomRequirementAttribute
    {
        public float Value = 0.0f;
        public RequireRoomValueAttribute(float value) 
        {
            Value = value;
        }
        public override bool IsMet(Room room, User owner)
        {
            return room.RoomValue != null ? room.RoomValue.Value >= Value : false;
        }
        public override LocString Describe()
        {
            return Localizer.Do($"{Value} value minimum in room");
        }
        public override LocString Describe(Room room, User owner)
        {
            float roomValue = room.RoomValue != null ? room.RoomValue.Value : 0.0f;
            return Localizer.Do($"Minimum room value (furniture): You have {Text.Info(roomValue)} out of needed {Text.Info(Value)}");
        }
    }
    //TODO - a finir
    public class RoomTypeRequirementAttribute : RoomRequirementAttribute
    {
        public RoomCategory RoomCategory;
        
        public RoomTypeRequirementAttribute(RoomCategory roomCategory)
        {
            RoomCategory = roomCategory;
        }
        public override bool IsMet(Room room, User owner)
        {
            return false;
        }
        public override LocString Describe()
        {
            return Localizer.Do($"{RoomCategory} as room category");
        }
        public override LocString Describe(Room room, User owner)
        {
            RoomCategory actualRoomCat = RoomCategory.LivingRoom;
            return Localizer.Do($"Room category: It is {Text.Info(actualRoomCat)} instead of {Text.Info(RoomCategory)}");
        }
        public RoomCategory GetRoomCategory(Room room)
        {
            //Retrieve room stats
            var roomStats = room.RoomStats;
            //Retrieve all applicable components
            var allComponents = roomStats.ContainedComponents<HousingComponent>().Where(x => x.HomeValue != null).ToList();

            //Voir logique dans StandardFurnishedRoomValue.cs, ligne 58, GetRoomCategoryAndFurnishings

            return RoomCategory;
        }
    }
}
