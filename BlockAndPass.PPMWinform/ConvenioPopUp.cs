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
    public partial class ConvenioPopUp : Form
    {
        ServicesByP cliente = new ServicesByP();

        private string _NameConvenio = string.Empty;

        public string NameConvenio
        {
            get { return _NameConvenio; }
            set { _NameConvenio = value; }
        }
        private int _Convenio = 0;
        public int Convenio
        {
            get { return _Convenio; }
            set { _Convenio = value; }
        }

        public ConvenioPopUp(string idEstacionamiento, string sUsuario)
        {
            InitializeComponent();

            ConveniosResponse oInfo = cliente.ObtenerListaConveniosXEstacionamientoXUsuario(idEstacionamiento, sUsuario);
            if (!oInfo.Exito)
            {
                MessageBox.Show("No encontro convenios asociados al estacionamiento id = " + idEstacionamiento, "Error Aplicar Convenio PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.None;
                this.Close();
            }
            else
            {
                var dataSource = new List<Object>();
                foreach (var item in oInfo.LstInfoConvenios)
                {
                    dataSource.Add(new { Name = item.Display, Value = item.Value });
                }

                //Setup data binding
                this.cbConvenio.DataSource = dataSource;
                this.cbConvenio.DisplayMember = "Name";
                this.cbConvenio.ValueMember = "Value";

                DescripcionConvenioResponse oDes = cliente.ConsultarDescripcionConvenio(cbConvenio.SelectedValue.ToString());
                if (oDes.Exito)
                {
                    tbDescripcion.Text = oDes.Descripcion;
                }
                else
                {
                    tbDescripcion.Text = oDes.ErrorMessage;
                }
            }
        }

        private void btn_Ok_Click(object sender, EventArgs e)
        {
            this.Convenio = Convert.ToInt32(cbConvenio.SelectedValue);
            
            string GG = cbConvenio.Text;
            this.NameConvenio = GG;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void cbConvenio_SelectedIndexChanged(object sender, EventArgs e)
        {
            DescripcionConvenioResponse oDes = cliente.ConsultarDescripcionConvenio(cbConvenio.SelectedValue.ToString());
            if (oDes.Exito)
            {
                tbDescripcion.Text = oDes.Descripcion;
            }
            else
            {
                tbDescripcion.Text = oDes.ErrorMessage;
            }
        }
    }
}
