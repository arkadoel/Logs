using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;
using System.Xml;

namespace FeedBackManager
{
    public class Logs
    {
        public static string LOG_FILEPATH { get; set; }
        public static string LOG_DIR { get; set; }

        /// <summary>
        /// Inicia directorios y fichero LOG para hoy.
        /// Si existe un archivo de dias anteriores, lo envia para su evaluacion.
        /// </summary>
        public static void initLog()
        {
            //Directorio de logs
            LOG_DIR = Path.Combine(Directory.GetCurrentDirectory(), "Logs");

            if (Directory.Exists(LOG_DIR) == false)
            {
                Directory.CreateDirectory(LOG_DIR);
            }

            //Fichero diario de log
            LOG_FILEPATH = Path.Combine(LOG_DIR, dailyLogName());

            if (File.Exists(LOG_FILEPATH) == false)
            {
                StreamWriter fich = new StreamWriter(LOG_FILEPATH);
                fich.WriteLine("<?xml version=\"1.0\"?>");
                fich.WriteLine("<Acciones>");
                fich.WriteLine("</Acciones>");
                fich.Close();
            }

            //Evaluar si hay ficheros anteriores y enviar si procede.
           /* new System.Threading.Thread(delegate()
            {
                //RevisarLogsAntiguosParaEnviar();
                AplicacionEsUsada();
            }).Start();
            */
        }

        /// <summary>
        /// Permite registrar cuando la aplicacion es usada por un ordenador
        /// </summary>
        private static void AplicacionEsUsada()
        {
            string mensaje = GetDatosOrdenador();
            /*
             * Parte en la que poner codigo para enviar por email
             */
        }

        /// <summary>
        /// Conseguir datos estadisticos del ordenador
        /// </summary>
        /// <returns></returns>
        private static string GetDatosOrdenador()
        {
            string mensaje = "Aplicacion usada";
            mensaje += "\r\nDominio: " + Environment.UserDomainName.ToString();
            mensaje += "\r\nNombre maquina: " + Environment.MachineName.ToString();
            mensaje += "\r\nUsuario: " + Environment.UserName.ToString();
            mensaje += "\r\nSistema Operativo: " + GetOSVersion();
            mensaje += "\r\nPlataforma: " + Environment.OSVersion.Platform.ToString();
            mensaje += "\r\nService Pack: " + Environment.OSVersion.ServicePack.ToString();
            mensaje += "\r\nVersion sistema Operativo: " + Environment.OSVersion.VersionString.ToString();
            mensaje += "\r\nSistema de 64bits: " + Environment.Is64BitOperatingSystem.ToString();
            return mensaje;
        }

        /// <summary>
        /// Excribir en el XML los datos estadisticos del ordenador
        /// </summary>
        public static void DatosOrdenador()
        {
            XElement e = new XElement("DatosPC",
                new XElement("Dominio",  Environment.UserDomainName.ToString()),
                new XElement("NombreMaquina", Environment.MachineName.ToString()),
                new XElement("Usuario", Environment.UserName.ToString()),
                new XElement("SistemaOperativo", GetOSVersion()),
                new XElement("Plataforma", Environment.OSVersion.Platform.ToString()),
                new XElement("ServicePack", Environment.OSVersion.ServicePack.ToString()),
                new XElement("VersionOS", Environment.OSVersion.VersionString.ToString()),
                new XElement("SistemaDe64bits", Environment.Is64BitOperatingSystem.ToString())
                );

            XDocument doc = XDocument.Load(LOG_FILEPATH);

            try
            {
                //escribir xml
                doc.Element("Acciones").Add(e);
                doc.Save(LOG_FILEPATH, SaveOptions.None);

            }
            catch { /*..Ignorar errores..*/  }
            finally
            {
                doc = null;
                GC.Collect();
            }
        }


        /// <summary>
        /// Saber version del sistema operativo
        /// </summary>
        /// <returns></returns>
        public static string GetOSVersion()
        {
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Win32S:
                    return "Win 3.1";
                case PlatformID.Win32Windows:
                    switch (Environment.OSVersion.Version.Minor)
                    {
                        case 0:
                            return "Win95";
                        case 10:
                            return "Win98";
                        case 90:
                            return "WinME";
                    }
                    break;

                case PlatformID.Win32NT:
                    switch (Environment.OSVersion.Version.Major)
                    {
                        case 3:
                            return "NT 3.51";
                        case 4:
                            return "NT 4.0";
                        case 5:
                            switch (Environment.OSVersion.Version.Minor)
                            {
                                case 0:
                                    return "Windows 2000";
                                case 1:
                                    return "Windows XP";
                                case 2:
                                    return "Windows Server 2003";
                            }
                            break;

                        case 6:
                            switch (Environment.OSVersion.Version.Minor)
                            {
                                case 0:
                                    return "Windows Vista";
                                case 1:
                                    return "Windows 7";
                                case 2:
                                    return "Windows 8";
                                case 3:
                                    return "Windows 8.1";
                            }
                            break;
                    }
                    break;

                case PlatformID.WinCE:
                    return "Win CE";
            }

            return "Desconocido";
        }

        /// <summary>
        /// Revisa si hay algun archivo de log anterior y lo prepara para
        /// enviarlo automaticamente en segundo plano.
        /// </summary>
        [Obsolete("Este metodo esta en revision")]
        private static void RevisarLogsAntiguosParaEnviar()
        {
            DirectoryInfo dir = new DirectoryInfo(LOG_DIR);
            FileInfo[] filesInDir = dir.GetFiles();
            var logFiles = from u in dir.GetFiles()
                           where u.Extension == ".xml"
                           select u;

            if (logFiles.Count() > 0)
            {
                foreach (FileInfo file in logFiles)
                {
                    FileInfo fileInfo = file;

                    if (file.FullName == LOG_FILEPATH)
                    {
                        string tempPath = System.IO.Path.GetTempPath();
                        tempPath = Path.Combine(tempPath, file.Name);

                        System.IO.File.Copy(file.FullName, tempPath);
                        fileInfo = new FileInfo(tempPath);
                    }
                        //enviar log de dia anterior
                        string mensaje = "" + GetDatosOrdenador();
                        mensaje += "\r\nFecha: " + DateTime.Today.ToShortDateString();
                        mensaje += " " + DateTime.Now.ToShortTimeString();
                        mensaje += "\r\n\r\n";

                        //finalizamos el archivo XML
                        StreamWriter fichero = new StreamWriter(fileInfo.FullName, true);
                        fichero.WriteLine("</Acciones>");
                        fichero.Close();

                        //Procedemos a enviar el archivo
                        new System.Threading.Thread(delegate()
                        {
                            Boolean estaEnviado = true;
                            string rutaFichero = fileInfo.FullName;

                            estaEnviado = false; // añadir metodo que mande emails
                            if (estaEnviado)
                            {
                                //Como el mensaje esta enviado correctamente, se elimina el fichero
                                try { File.Delete(rutaFichero); }
                                catch { }
                            }
                        }).Start();
                    
                }
            }
        }

        /// <summary>
        /// Escribe un mensaje en el log con prioridad nula y de tipo informacion
        /// </summary>
        /// <param name="titulo"></param>
        /// <param name="mensaje"></param>
        public static void WriteText(string titulo, string mensaje)
        {
            WriteText(titulo, mensaje, Constantes.TipoAccion.Info, Constantes.Prioridad.Nula);      
        }

        /// <summary>
        /// Escribe un mensaje en el log especificando todos los datos.
        /// </summary>
        /// <param name="titulo"></param>
        /// <param name="mensaje"></param>
        /// <param name="tipo"></param>
        /// <param name="prio"></param>
        public static void WriteText(string titulo, string mensaje, string tipo, string prio )
        {
            Accion acc = new Accion();
            acc.Mensaje = mensaje;
            acc.TipoDeAccion = tipo;
            acc.PrioridadAccion = prio;
            acc.Titulo = titulo;
            acc.FechaHora = DateTime.Now;
            Guardar( acc );
        }

        /// <summary>
        /// Permite registrar un error producido en el archivo de log.
        /// </summary>
        /// <param name="titulo"></param>
        /// <param name="ex"></param>
        public static void WriteError(string titulo, Exception ex)
        {
            Accion acc = new Accion();
            acc.MensajeXML = new XElement("ErrorMensaje");

            acc.MensajeXML.SetValue(ex.Message);           

           // acc.Mensaje = "<ErrorMensaje>" + ex.Message + "</ErrorMensaje>";
            if (ex.InnerException != null)
            {
                /*
                acc.Mensaje += "<InnerException><Source>" +
                    ex.InnerException.Source +
                    "</Source><InnerText>" + ex.InnerException.Message + "</InnerText></InnerException>";
                */
                XElement innerException = new XElement("InnerException",
                    new XElement("Source", ex.InnerException.Source),
                    new XElement("InnerText", ex.InnerException.Message)
                    );
                acc.MensajeXML.Add(innerException);
            }

            //acc.Mensaje += "<StackTrace>" + ex.StackTrace.ToString() + "</StackTrace>";
            acc.MensajeXML.Add(new XElement("StackTrace", ex.StackTrace.ToString()));

            acc.TipoDeAccion = Constantes.TipoAccion.Error;
            acc.PrioridadAccion = Constantes.Prioridad.Alta;
            acc.Titulo = titulo;
            acc.FechaHora = DateTime.Now;
            Guardar(acc);
        }

        /// <summary>
        /// Guarda una accion en el archivo de log.
        /// </summary>
        /// <param name="a"></param>
        [Obsolete("Metodo antiguo, usar 'Guardar(Accion)'")]
        public static void GuardarAccion(Accion a)
        {
            StreamWriter fich = null;
            try
            {
                fich = new StreamWriter(LOG_FILEPATH, true);
                fich.WriteLine("\t" + a.toXML());
            }
            catch { /*...*/  }
            finally
            {
               if(fich!=null) fich.Close();
            }
        }

        /// <summary>
        /// Guarda una accion en el archivo de log en formato XML.
        /// </summary>
        /// <param name="a">Objeto Accion a guardar</param>
        public static void Guardar(Accion a)
        {
            XDocument doc = XDocument.Load(LOG_FILEPATH);
            try
            {
                //escribir xml
                XElement elemento = new XElement("Accion");
                elemento.SetAttributeValue("Titulo", a.Titulo);
                elemento.SetAttributeValue("Fecha", a.FechaHora.ToShortDateString());
                elemento.SetAttributeValue("Hora", a.FechaHora.ToLongTimeString());
                elemento.SetAttributeValue("Prioridad", a.PrioridadAccion);
                elemento.SetAttributeValue("TipoAccion", a.TipoDeAccion);

                if (a.MensajeXML == null)
                {
                    elemento.SetValue(a.Mensaje);
                }
                else
                {
                    elemento.Add(a.MensajeXML);
                }

                doc.Element("Acciones").Add(elemento);
                doc.Save(LOG_FILEPATH,SaveOptions.None);

            }
            catch { /*..Ignorar errores..*/  }
            finally
            {
                doc = null;
                GC.Collect();
            }
        }

        /// <summary>
        /// Devuelve el nombre del archivo, el cual varia dependiendo
        /// del dia en el que se este.
        /// </summary>
        /// <returns></returns>
        public static string dailyLogName()
        {
            int dia = DateTime.Today.Day;
            int mes = DateTime.Today.Month;
            int anyo = DateTime.Today.Year;
            string strDia, strMes, strAnyo;

            if (dia < 10) strDia = "0" + dia.ToString();
            else strDia = dia.ToString();

            if (mes < 10) strMes = "0" + mes.ToString();
            else strMes = mes.ToString();

            if (anyo < 10) strAnyo = "0" + anyo.ToString();
            else strAnyo = anyo.ToString();

            return strAnyo + strMes + strDia + ".xml";
        }
    }
}
