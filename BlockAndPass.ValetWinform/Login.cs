using BlockAndPass.ValetWinform.ByPServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlockAndPass.ValetWinform
{
    public partial class Login : Form
    {
        ServicesByP cliente = new ServicesByP();

        public Login()
        {
            InitializeComponent();
        }

        private void btn_Ok_Click(object sender, EventArgs e)
        {
            LoginResponse oLogin = cliente.Loguearse(tbUser.Text);
            if (oLogin.Exito)
            {
                if (Decrypt(oLogin.Clave) == tbClave.Text)
                {

                    Valet df = new Valet(oLogin.Documento, tbUser.Text);

                    df.Show();

                    this.Close();

                }
                else
                {
                    MessageBox.Show("Clave incorrecta", "Error Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    Application.Exit();
                }
            }
            else
            {
                MessageBox.Show(oLogin.ErrorMessage, "Error Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                Application.Exit();
            }
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
            Application.Exit();
        }

        private string Decrypt(string cipherText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

        private void tbClave_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                btn_Ok_Click(btn_Ok, EventArgs.Empty);
            }
        }
    }
}
