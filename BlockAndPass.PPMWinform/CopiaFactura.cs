using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlockAndPass.PPMWinform
{
    public partial class CopiaFactura : Form
    {
        public static SqlConnection conexionSQL = new SqlConnection();
        public static SqlCommand comandoSQL = new SqlCommand();
        public static string sSerial
        {
            get
            {
                string sIdeModulo = ConfigurationManager.AppSettings["serial"];
                if (!string.IsNullOrEmpty(sIdeModulo))
                {
                    return sIdeModulo;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public CopiaFactura()
        {
            InitializeComponent();
        }

        private void CopiaFactura_Load(object sender, EventArgs e)
        {

            
        }

        private void btn_Ok_Click(object sender, EventArgs e)
        {

            string NUMFACT = tbnumerofactura.Text;

            string data = @"" + sSerial + "";
            conexionSQL.ConnectionString = data;

            string TIPO = string.Empty;

            conexionSQL.Open();

            //Formar la sentencia SQL, un SELECT en este caso
            SqlDataReader myReader1 = null;
            string strCadSQL1 = "SELECT IDTIPOPAGO FROM  T_PAGOS WHERE  NUMEROFACTURA = '" + NUMFACT + "'";
            SqlCommand myCommand1 = new SqlCommand(strCadSQL1, conexionSQL);

            //Ejecutar el comando SQL
            myReader1 = myCommand1.ExecuteReader();

            while (myReader1.Read())
            {
                TIPO = myReader1["IDTIPOPAGO"].ToString();
            }

            conexionSQL.Close();

            if (TIPO == "1")
            {

                this.dataTable1TableAdapter1.Fill(this.dataSetCopia.DataTable1, NUMFACT);
                //this.DATA

                this.reportViewer1.RefreshReport();
            }
            else 
            {
                CopiaFacturaM popup = new CopiaFacturaM(tbnumerofactura.Text);
                popup.ShowDialog();
                if (popup.DialogResult == DialogResult.OK)
                {
                    this.Close();
                }
                else if (popup.DialogResult == System.Windows.Forms.DialogResult.Cancel)
                {
                    this.DialogResult = DialogResult.Cancel;
                    this.Close();
                }
            }
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
