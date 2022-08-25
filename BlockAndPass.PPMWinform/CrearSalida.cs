using BlockAndPass.PPMWinform.ByPServices;
using EGlobalT.Device.SmartCard;
using EGlobalT.Device.SmartCardReaders;
using EGlobalT.Device.SmartCardReaders.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlockAndPass.PPMWinform
{
    public partial class CrearSalida : Form
    {
        private int _IdEstacionamiento = 0;
        public int IdEstacionamiento
        {
            get { return _IdEstacionamiento; }
            set { _IdEstacionamiento = value; }
        }

        ServicesByP cliente = new ServicesByP();

        public CrearSalida(string iIdEstacionamiento)
        {
            InitializeComponent();

            _IdEstacionamiento = Convert.ToInt32(iIdEstacionamiento);
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btn_Ok_Click(object sender, EventArgs e)
        {
            if (tbPlaca.Text != string.Empty)
            {
                if (EsAutorizado(tbPlaca.Text))
                {
                    DialogResult result3 = MessageBox.Show("¿Esta seguro que desea crear la salida para la placa: " + tbPlaca.Text, "Crear Salida", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                    if (result3 == DialogResult.Yes)
                    {
                        CreaSalidaResponse resp = cliente.CrearSalida(_IdEstacionamiento.ToString(), tbPlaca.Text);

                        if (resp.Exito)
                        {
                            this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                        else
                        {
                            this.DialogResult = DialogResult.None;
                            MessageBox.Show(resp.ErrorMessage, "Error Crear Salida PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            this.Close();
                        }
                    }
                    else
                    {
                        this.DialogResult = DialogResult.Cancel;
                        this.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Solo es posible Crear Salida sin tarjeta de Autorizados", "Error Crear Salida", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                }
            }
            else
            {
                MessageBox.Show("Debe escribir una placa valida.", "Error Crear Salida", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }

            //this.Close();
        }

        private void tbPlaca_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                btn_Ok_Click(btn_Ok, EventArgs.Empty);
            }
        }

        private bool EsAutorizado(string sPlaca)
        {
            bool bResultado = false;

            AutorizadoxPlacaResponse resp = cliente.BuscarAutorizadoxPlaca(tbPlaca.Text);

            if (resp.Exito)
            {
                bResultado = true;
            }

            return bResultado;
        }
    }
}
