using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace BlockAndPass.PPMWinform
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Login myLoginForm = new Login();
            myLoginForm.Show();
            Application.Run();
        }
    }
}
