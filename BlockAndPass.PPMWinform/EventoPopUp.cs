using BlockAndPass.PPMWinform.ByPServices;
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
    public partial class EventoPopUp : Form
    {
        ServicesByP cliente = new ServicesByP();

        private int _Evento = 0;

        public int Evento
        {
            get { return _Evento; }
            set { _Evento = value; }
        }

        public EventoPopUp(string idEstacionamiento, string sUsuario)
        {
            InitializeComponent();

            EventosResponse oInfo = cliente.ObtenerListaEventosXEstacionamientoXUsuario(idEstacionamiento, sUsuario);
            if (!oInfo.Exito)
            {
                this.DialogResult = DialogResult.None;
                MessageBox.Show("No encontro eventos asociados al estacionamiento id = " + idEstacionamiento, "Error Aplicar Evento PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
            else
            {
                var dataSource = new List<Object>();
                foreach (var item in oInfo.LstInfoEventos)
                {
                    dataSource.Add(new { Name = item.Display, Value = item.Value });
                }

                //Setup data binding
                this.cbEvento.DataSource = dataSource;
                this.cbEvento.DisplayMember = "Name";
                this.cbEvento.ValueMember = "Value";
            }
        }

        private void btn_Ok_Click(object sender, EventArgs e)
        {
            this.Evento = Convert.ToInt32(cbEvento.SelectedValue);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
