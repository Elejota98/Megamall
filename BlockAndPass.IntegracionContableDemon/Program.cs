using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlockAndPass.IntegracionContableDemon
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {

                if (System.Diagnostics.Process.GetProcessesByName("BlockAndPass.IntegracionContableDemon").Length > 1)
                {
                    MessageBox.Show("Ya existe una Instancia en Ejecución..");
                    return;
                }

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new frmIntegracionContableDemon());
            }
            catch (Exception e)
            {
                MessageBox.Show(e.InnerException + " " + e.Source + " " + e.Message);
            }
        }
    }
    
    
}
