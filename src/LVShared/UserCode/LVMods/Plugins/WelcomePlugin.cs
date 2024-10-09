// Le Village - Génération d'une fenêtre d'accueil pour les joueurs arrivant sur le serveur
// TODO - Pouvoir alimenter les news à partir d'un fichier déposé directement sur le serveur

using Eco.Core.Plugins.Interfaces;
using Eco.Core.Utils;
using Eco.Gameplay.Civics.GameValues;
using Eco.Gameplay.Civics.GameValues.Values;
using Eco.Gameplay.Players;
using Eco.Gameplay.Systems.Messaging.Chat.Commands;
using Eco.Shared.Localization;
using Eco.Shared.Utils;
using Eco.Simulation.Time;
using Village.Eco.Mods.Core;

namespace LVShared.UserCode.LVMods.Plugins
{
    [LocDisplayName("Welcome Plugin")]
    [ChatCommandHandler]
    public class WelcomePlugin : IInitializablePlugin, IModKitPlugin
    {
        public void Initialize(TimedTask timer)
        {
            UserManager.NewUserJoinedEvent.Add(NewUserJoinedEvent);
            UserManager.OnUserLoggedIn.Add(OnUserLoggedIn);
        }

        public static void NewUserJoinedEvent(User user)
        {
            //Note : L'utilisateur est créé mais pas encore connecté réellement donc pas possible d'envoyer des messages
        }

        public static void OnUserLoggedIn(User user)
        {
            Welcome(user);

            //Voir pour enchainer plusieurs popup
            //Task.Run(() => OnUsedAsync(player, itemStack)).continuewith();
        }

        public static void Welcome(User user)
        {
            var title = new LocStringBuilder();
            var text = new LocStringBuilder();
            var button = new LocStringBuilder();

            //Affichage du titre de la popup
            title.Append(TextLoc.ColorLocStr(Color.Yellow, "Bienvenue sur le serveur du Village !"));

            //Affichage du contenu de la popup
            text.AppendLine(1);
            text.AppendLine(TextLoc.BoldLocStr("Rappel des règles du serveur :")).AppendLine(1);
            text.AppendLine(Localizer.DoStr("Toutes formes de non-respect, de harcèlement ou d’insulte envers un autre joueur ne sont absolument pas tolérées et peuvent conduire, après des avertissements et en dernier recours, à un bannissement du serveur.")).AppendLine(1);
            text.AppendLine(Localizer.DoStr("L’utilisation de plusieurs comptes de jeu est interdite.")).AppendLine(1);
            text.AppendLine(Localizer.DoStr("Il est demandé à tous les joueurs d’avoir le même pseudo sur Discord et dans ECO. Cela peut se faire en changeant son pseudo spécifiquement sur le Discord du Village.")).AppendLine(1);
            text.AppendLine(Localizer.DoStr($"Rejoindre ce Discord implique que vous ayez lu et accepté les présentes règles. Pour rappel : {TextLoc.BoldLocStr("Dura lex sed lex")}")).AppendLine(1);
            text.AppendLine(Localizer.DoStr("Rejoindre le serveur de jeu ECO implique que vous ayez pris connaissance des paramétrages et modifications mises en place ainsi que leur fonctionnement. Ne pas hésiter à poser des questions dans les channels dédiés si besoin.")).AppendLine(1);

            // Voir pour ajouter un lien vers Discord - pour l'instant ça fait de la merde...
            //https://www.w3schools.com/tags/tag_a.asp
            //string DiscordURL = @"<a href=""https://www.play.eco"">Visit W3Schools.com!</a>";
            //text.AppendLine(Localizer.DoStr(DiscordURL));

            //Affichage du bouton de la popup
            button.Append($"Lu et approuvé.");
            
            user.Player.LargeInfoBox(title.ToLocString(), text.ToLocString(), button.ToLocString());
        }

        public static void LastNews(User user) 
        {
            var title = new LocStringBuilder();
            var text = new LocStringBuilder();
            var button = new LocStringBuilder();

            int WorldDay = (int)WorldTime.Day+1;
            int AllUsers = PlayerUtils.AllUsers.Count();
            int OnlineUsers = PlayerUtils.OnlineUsers.Count();
            int ExhaustUsers = PlayerUtils.ExhaustedUsers.Count();
            string exhaustText = ExhaustUsers == 0 ? "aucun" : ExhaustUsers.ToString();

            //Affichage du titre de la popup
            title.Append(TextLoc.BoldLocStr("Le Village Hebdo"));

            //Affichage du contenu de la popup
            text.AppendLine(1);
            text.AppendLine(Localizer.DoStr($"Bonjour cher lecteur/chère lectrice du meilleur journal sur ECO !")).AppendLine(1);
            text.AppendLine(Localizer.DoStr($"Aujourd'hui, nous sommes le jour {TextLoc.ColorLocStr(Color.Yellow, $"{WorldDay}")}."));
            text.AppendLine(Localizer.DoStr($"Il y a actuellement {TextLoc.ColorLocStr(Color.Yellow, $"{OnlineUsers}")} joueur(s) connecté(s) sur {TextLoc.ColorLocStr(Color.Yellow, $"{AllUsers}")}."));
            text.AppendLine(Localizer.DoStr($"Également, {TextLoc.ColorLocStr(Color.Yellow, $"{exhaustText}")} joueurs ne sont épuisés.")).AppendLine(2);
            text.AppendLine(TextLoc.BoldLocStr("Voici les dernières nouvelles :")).AppendLine(1);
            text.AppendLine(TextLoc.IndentLocStr("Aucune nouvelle.")).AppendLine(1);

            //Affichage du bouton de la popup
            button.Append($"Pour ne rien rater, je m'abonne.");

            user.Player.LargeInfoBox(title.ToLocString(), text.ToLocString(), button.ToLocString());
        }

        [ChatCommand("Welcome message & rules", ChatAuthorizationLevel.User)]
        public static void LVRules(User user) 
        {
            Welcome(user);
        }

        [ChatCommand("Welcome message & rules", ChatAuthorizationLevel.User)]
        public static void LVNews(User user)
        {
            LastNews(user);
        }

        public string GetCategory() => "LeVillageMods";
        public override string ToString() => Localizer.DoStr("Welcome Plugin");
        public string GetStatus() => "Active";
    }
}