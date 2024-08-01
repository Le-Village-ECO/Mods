using Eco.Core.Items;
using Eco.Gameplay.Interactions.Interactors;
using Eco.Gameplay.Items;
using Eco.Gameplay.Players;
using Eco.Gameplay.Systems.Messaging.Notifications;
using Eco.Gameplay.Systems.NewTooltip.TooltipLibraryFiles;
using Eco.Shared.Localization;
using Eco.Shared.Math;
using Eco.Shared.Serialization;
using Eco.Shared.SharedTypes;
using Eco.Shared.Utils;
using Eco.World;
using Eco.World.Blocks;
using System;
using System.ComponentModel;
using System.Text;

namespace Village.Eco.Mods.MiningSpecialty
{
    [Serialized]
    [LocDisplayName("Gold Detector")]
    [LocDescription("Indique la proximité de filon d'or (chaud-froid)")]
    [Category("Tools")]
    [Tag("Tool")]
    [Ecopedia("Items", "Tools", createAsSubPage: true)]
    [Weight(1000)]
    public class GoldDetector : ToolItem, IInteractor
    {
        [Interaction(InteractionTrigger.LeftClick, "Rechercher de l'or")]
        public bool SearchGold(Player player, InteractionTriggerInfo triggerInfo, InteractionTarget target)
        {
            if (target.IsBlock) 
            {
                int range = GetGoldOreDistance(target.BlockPosition.Value);

                NotificationManager.ServerMessageToAllLoc($"Range = {range} / Temperature = {LocDistance(range)}");

                return true;
            }

            return false;
        }
        public static int GetGoldOreDistance(Vector3i position)
        {
            int x = position.X;
            int y = position.Y;
            int z = position.Z;
            int rangeMax = 3;

            NotificationManager.ServerMessageToAllLoc($"X-Y-Z = {x}-{y}-{z}");

            for (int i = x - rangeMax; i <= x + rangeMax; i++) 
            {
                for (int j = y - rangeMax; j <= y + rangeMax; j++) 
                {
                    for (int k = z - rangeMax; k <= z + rangeMax; k++) 
                    {
                        //Calcul de la distance euclidienne entre (x,y,z) et (i,j,k)
                        double distance = Math.Sqrt(Math.Pow(i - x, 2) + Math.Pow(j - y, 2) + Math.Pow(k - z, 2));
                        if (distance <= rangeMax) 
                        {
                            NotificationManager.ServerMessageToAllLoc($"I-J-K = {i}-{j}-{k}");
                            if (IsGoldFound(position)) return (int)distance;
                        }
                    }
                }
            };
            WorldRange range = WorldRange.SurroundingSpace(position.Value, 30);
            return rangeMax+1;
        }
        public static bool IsGoldFound(Vector3i position) 
        {
            //Inspiration de DrillItem.cs

            // Unpack block from position, block position won't be null
            var block = World.GetBlock(position);

            // Get resulting item from block
            var typeId = -1;
            if (block is IRepresentsItem item) typeId = Item.Get(item)?.TypeID ?? -1;
            if (typeId == -1) typeId = BlockItem.CreatingItem(block.GetType())?.TypeID ?? -1;
            if (World.GetBlock(position).Is<Impenetrable>() || position.y <= 0) typeId = -2;


            NotificationManager.ServerMessageToAllLoc($"typeId = {typeId} / block = {block}");
            if (block is IRepresentsItem item2)
            {
                NotificationManager.ServerMessageToAllLoc($"Name = {Item.Get(item2).Name}");
                if (Item.Get(item2).Name == "SandstoneItem") return true;
            }   

            return false;
        }
        public static LocString LocDistance(int distance) => distance switch
        {
            0 => TextLoc.ColorLocStr(Color.Red, "Very hot!!"),
            1 => TextLoc.ColorLocStr(Color.Red, "Very hot!!"),
            2 => TextLoc.ColorLocStr(Color.LightRed, "Hot!"),
            3 => TextLoc.ColorLocStr(Color.Grey, "Lukewarm."),
            4 => TextLoc.ColorLocStr(Color.BlueGrey, "Cold..."),
            5 => TextLoc.ColorLocStr(Color.Blue, "Really cold..."),
            _ => throw new NotImplementedException(), //Ajout Visual Studio
        };
    }
}
