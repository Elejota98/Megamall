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
    public partial class CortesiaPopUp : Form
    {
        ServicesByP cliente = new ServicesByP();

        private string _Observacion = string.Empty;

        public string Observacion
        {
            get { return _Observacion; }
            set { _Observacion = value; }
        }

        private int _Motivo = 0;

        public int Motivo
        {
            get { return _Motivo; }
            set { _Motivo = value; }
        }

        public CortesiaPopUp(string idEstacionamiento)
        {
            InitializeComponent();

            MotivosCortesiaResponse oInfo = cliente.ObtenerListaMotivosCortesiaXEstacionamiento(idEstacionamiento);
            if (!oInfo.Exito)
            {
                this.DialogResult = DialogResult.None;
                MessageBox.Show("No encontro motivos asociados al estacionamiento id = " + idEstacionamiento, "Error Aplicar Cortesia PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
            else
            {
                var dataSource = new List<Object>();
                foreach (var item in oInfo.LstMotivosCortesia)
                {
                    dataSource.Add(new { Name = item.Display, Value = item.Value });
                }

                //Setup data binding
                this.cbMotivo.DataSource = dataSource;
                this.cbMotivo.DisplayMember = "Name";
                this.cbMotivo.ValueMember = "Value";
            }
        }

        private void btn_Ok_Click(object sender, EventArgs e)
        {
            this.Motivo = Convert.ToInt32(cbMotivo.SelectedValue);
            this.Observacion = tbObservacion.Text;
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
