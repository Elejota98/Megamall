using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockAndPass.Utilidades
{
    public class Generales
    {
        /// <summary>
        /// Verifica si el archivo está en uso
        /// </summary>
        /// <param name="sRuta"></param>
        /// <param name="sNombreArchivo"></param>
        /// <returns></returns>
        public static bool VerificaArchivoEnUso(string sRuta, string sNombreArchivo)
        {
            try
            {
                using (var stream = new FileStream(sRuta + "\\" + sNombreArchivo, FileMode.Open, FileAccess.Read)) { }
            }
            catch (IOException)
            {
                return true;
            }

            return false;
        }
    }
}
