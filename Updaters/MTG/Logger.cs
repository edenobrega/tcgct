using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTG
{
	internal static class Logger
	{
        public static bool ShouldLog { get; set; }
        public static void Log(string type, string message)
		{
            if (!ShouldLog)
            {
                return;
            }
            Console.WriteLine($"{DateTime.Now.Hour}:{DateTime.Now.Minute}:{DateTime.Now.Second}:{DateTime.Now.Millisecond} | {type} | {message}");
        }
    }
}
