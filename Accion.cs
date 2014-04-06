using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FeedBackManager;

namespace FeedBackManager
{
    public class Accion
    {
        public string Titulo { get; set; }
        public string Mensaje { get; set; }
        public string TipoDeAccion { get; set; }
        public string PrioridadAccion { get; set; }
        public DateTime FechaHora{ get; set; }
        
        public Accion()
        {            
        }

        public Accion(string _titulo, string _Mensaje, DateTime _fechaHora, string _tipo, string _prioridad)
        {
            Titulo = _titulo;
            Mensaje = _Mensaje;
            TipoDeAccion = _tipo;
            PrioridadAccion = _prioridad;        
            FechaHora = _fechaHora;
        }

        public string toXML()
        {
            string msg = "<Accion ";
            msg += "Titulo='" + Titulo + "' ";
            msg += "Fecha='" + FechaHora.ToShortDateString() + "' ";
            msg += "Hora='" + FechaHora.ToLongTimeString() + "' ";
            msg += "Prioridad='" + PrioridadAccion + "' ";
            msg += "TipoAccion='" + TipoDeAccion + "' ";
            msg += ">" + Mensaje + "";
            return msg + "</Accion>";
        }
    }
}
