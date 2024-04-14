// Le Village Core - Outils pour alimenter la log des mods du Village
// TODO - Principe d'activation on/off paramétrable global et/ou par mod ?
// TODO - Solution plus industrielle sur le paramétre "mod"
// TODO - gestion des messages d'erreur donc avec exception

using Eco.Core.Utils.Logging;
using System.Reflection;

namespace Village.Eco.Mods.Core
{
    public enum Level 
    {
        Info,
        Warning,
        //Error,  //TODO
        Debug
    }
    public class Logger
    {
        public static void SendLog(Level level,string mod, string msg) 
        {
            var log = NLogManager.GetLogWriter("LeVillageMods");

            //TODO - pour ajouter le mod dans le texte
            //Assembly? assembly = Assembly.GetCallingAssembly();
            
            switch (level) 
            {
                case Level.Info:
                    log.Write($"[{mod}] - {msg}");
                    break;
                case Level.Warning:
                    log.WriteWarning($"[{mod}] - {msg}");
                    break;
                case Level.Debug:
                    log.Debug($"[{mod}] - {msg}");
                    break;
            }
        }
    }
}
