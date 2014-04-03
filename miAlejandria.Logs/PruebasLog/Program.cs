using FeedBackManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebasLog
{
    class Program
    {
        static void Main(string[] args)
        {
            Logs.initLog();
            Logs.WriteText("titulo", "prueba");
        }
    }
}
