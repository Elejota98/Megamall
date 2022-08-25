using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlockAndPass.PPMWinform
{
    public partial class CopiaFacturaM : Form
    {
        string _NumFact = string.Empty;
        string _Modulo = string.Empty;

        public CopiaFacturaM(string NUM, string MOD)
        {
            _NumFact = NUM;
            _Modulo = MOD;
            InitializeComponent();
        }

        private void btn_Ok_Click(object sender, EventArgs e)
        {
            string NUMFACT = tbnumerofactura.Text;
            string MODULO = cboModulo.SelectedItem.ToString();
            this.dataTable2TableAdapter1.Fill(this.dataSetCopia.DataTable2, NUMFACT, MODULO);
            //this.DATA

            this.reportViewer1.RefreshReport();
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void CopiaFacturaM_Load(object sender, EventArgs e)
        {
            tbnumerofactura.Text = _NumFact;
            cboModulo.SelectedItem = _Modulo;

        }
    }
}
