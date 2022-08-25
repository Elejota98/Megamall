using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockAndPass.Utilidades
{
    public class Globales
    {
        /// <summary>
        /// Establece si está habilitado el logging de la aplicación
        /// </summary>
        public static bool bEnabledTracking
        {
            get
            {
                string sEnabledTracking = ConfigurationManager.AppSettings["EnabledTracking"];
                if (!string.IsNullOrEmpty(sEnabledTracking))
                {
                    return Convert.ToBoolean(sEnabledTracking);
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
