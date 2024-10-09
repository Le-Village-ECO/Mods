// Le Village Core - Outils pour alimenter la log des mods du Village
// TODO - Principe d'activation on/off paramétrable global et/ou par mod ?
// TODO - Solution plus industrielle sur le paramétre "mod"

using Eco.Core.Utils.Logging;
using System.Reflection;

namespace Village.Eco.Mods.Core
{
    public enum Criticity
    {
        Info,
        Warning
    }
    public class Logger
    {
        public static void SendLog(Criticity level,string mod, string msg) 
        {
            var log = NLogManager.GetLogWriter("LeVillageMods");

            //TODO - pour ajouter le mod dans le texte
            //Assembly? assembly = Assembly.GetCallingAssembly();
            
            switch (level) 
            {
                case Criticity.Info:
                    log.Write($"[{mod}] - {msg}");
                    break;
                case Criticity.Warning:
                    log.WriteWarning($"[{mod}] - {msg}");
                    break;
            }
        }
    }
}
