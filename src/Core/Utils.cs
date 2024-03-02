using Eco.Core.Utils.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Village.Eco.Mods.Core
{
    public static class Utils
    {
        // Permet de log la pile d'appels pour analyser et debuguer
        // Il suffit d'appeler LogWithStackTrace
        public static void LogWithStackTrace() 
        {
            var stack = Environment.StackTrace;
            var log = NLogManager.GetLogWriter("LeVillageMods");
            log.Write($"Pile d'appels {stack}");
        }
    }
}
