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
    public partial class ArqueoPopUp : Form
    {
        ServicesByP cliente = new ServicesByP();

        private Int64 _Valor = 0;

        public Int64 Valor
        {
            get { return _Valor; }
            set { _Valor = value; }
        }

        public ArqueoPopUp(string idArqueo)
        {
            InitializeComponent();

            tbIdArqueo.Text = idArqueo;
            
        }

        private void btn_Ok_Click(object sender, EventArgs e)
        {
            this.Valor = Convert.ToInt64(tbValorConteoManual.Text.Replace("$", "").Replace(".", ""));
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void tbValor_TextChanged(object sender, EventArgs e)
        {
            Int64 recibido = 0;
            if (Int64.TryParse(tbValorConteoManual.Text.Replace("$", "").Replace(".", ""), out recibido))
            {
                tbValorConteoManual.Text = "$" + string.Format("{0:#,##0.##}", recibido);
            }
            else
            {
                tbValorConteoManual.Text = string.Empty;
            }

            tbValorConteoManual.SelectionStart = tbValorConteoManual.Text.Length; // add some logic if length is 0
            tbValorConteoManual.SelectionLength = 0;
        }

    }
}
