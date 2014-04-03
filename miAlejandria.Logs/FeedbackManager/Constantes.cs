using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FeedBackManager
{
    public class Constantes
    {
        public const string APP_NAME = "Nombre app";
        public const string APP_VERSION = "0.1.2";

        public static String DirectorioActual { get; set; }

        public class TipoAccion
        {
            public const string Error = "Error";
            public const string Info = "Info";
        }
        public class Prioridad
        {
            public const string Extrema = "Extrema";
            public const string Grave = "Grave";
            public const string Alta = "Alta";
            public const string Normal = "Normal";
            public const string Baja = "Baja";
            public const string Nula = "Nula";
        }

        public static void CargarParametrosIniciales()
        {
            //cargar directorio actual
            DirectorioActual = System.IO.Directory.GetCurrentDirectory();

            FeedBackManager.Logs.initLog();

        }
    }
}
