using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace BlockAndPass.VisadoWinform
{
    public partial class WelcomeVisado : Form
    {
        public WelcomeVisado()
        {
            InitializeComponent();
        }

        private void btn_Next_Click(object sender, EventArgs e)
        {
            if (this.ValidateChildren(ValidationConstraints.Enabled))
            {

                //Todo es correcto, guardamos los datos
                UsuarioCollection usuarios = null;
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "loginFile.mp78");
                bool bFind = false;
                bool bWork = false;

                if (File.Exists(path))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(UsuarioCollection));

                    StreamReader reader = new StreamReader(path);
                    usuarios = (UsuarioCollection)serializer.Deserialize(reader);
                    reader.Close();


                    if (usuarios.Usuario.Length > 0)
                    {
                        foreach (var item in usuarios.Usuario)
                        {
                            if (item.Login == tbUsuario.Text)
                            {
                                bFind = true;
                                if (item.Clave == tbClave.Text)
                                {
                                    AdministrarVisado popup = new AdministrarVisado();
                                    popup.ShowDialog();
                                    bWork = true;
                                    break;
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("No es posible administrar ya que no se encontraron usuarios.", "Visado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("No es posible administrar ya que no existe el archivo de usuarios.", "Visado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                if (bFind)
                {
                    if (!bWork)
                    {
                        MessageBox.Show("La clave no corresponde al usuario ingresado.", "Visado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("No fue posible encontrar el usuario.", "Visado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                tbClave.Text = string.Empty;
                tbUsuario.Text = string.Empty;
            }
            //else
            //{
            //    MessageBox.Show("Faltan algunos campos por rellenar");
            //}
        }

        private void tbUsuario_Validating(object sender, CancelEventArgs e)
        {
            if (tbUsuario.Text == "")
            {
                e.Cancel = true;
                tbUsuario.Select(0, tbUsuario.Text.Length);
                errorProvider1.SetError(tbUsuario, "Debe introducir el Login");
            }
        }

        private void tbUsuario_Validated(object sender, EventArgs e)
        {
            errorProvider1.SetError(tbUsuario, "");
        }

        private void tbClave_Validating(object sender, CancelEventArgs e)
        {
            if (tbClave.Text == "")
            {
                e.Cancel = true;
                tbClave.Select(0, tbClave.Text.Length);
                errorProvider2.SetError(tbClave, "Debe introducir la Clave");
            }
        }

        private void tbClave_Validated(object sender, EventArgs e)
        {
            errorProvider2.SetError(tbClave, "");
        }

        private void btn_Convenio_Click(object sender, EventArgs e)
        {
            ConvenioPopUp popup = new ConvenioPopUp();
            popup.ShowDialog();
        }
    }

    [Serializable()]
    public class Usuario
    {
        [System.Xml.Serialization.XmlElement("Login")]
        public string Login { get; set; }

        [System.Xml.Serialization.XmlElement("Clave")]
        public string Clave { get; set; }
    }

    [Serializable()]
    [System.Xml.Serialization.XmlRoot("UsuarioCollection")]
    public class UsuarioCollection
    {
        [XmlArray("Usuarios")]
        [XmlArrayItem("Usuario", typeof(Usuario))]
        public Usuario[] Usuario { get; set; }
    }
}
