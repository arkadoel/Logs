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
        #region "Clases de prueba"
        public class MyAppException : ApplicationException
        {
            public MyAppException(String message)
                : base(message)
            { }
            public MyAppException(String message, Exception inner) : base(message, inner) { }
        }
        public class ExceptExample
        {
            public void ThrowInner()
            {
                throw new MyAppException("ExceptExample inner exception");
            }
            public void CatchInner()
            {
                try
                {
                    this.ThrowInner();
                }
                catch (Exception e)
                {
                    throw new MyAppException("Error caused by trying ThrowInner.", e);
                }
            }
        }
        #endregion

        static void Main(string[] args)
        {
            Logs.initLog();
            Logs.WriteText("titulo", "prueba");
            Logs.DatosOrdenador();
            
            ExceptExample testInstance = new ExceptExample();
            try 
            {
            testInstance.CatchInner();
            }
            catch(Exception e) 
            {
                Logs.WriteError("Producido un error", e);
            }
      
        }
    }
}
