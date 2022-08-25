using BlockAndPass.ValetWinform.ByPServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlockAndPass.ValetWinform
{
    public partial class Valet : Form
    {
        ServicesByP cliente = new ServicesByP();

        private string _DocumentoUsuario = string.Empty;
        public string DocumentoUsuario
        {
            get { return _DocumentoUsuario; }
            set { _DocumentoUsuario = value; }
        }

        bool entryComboSede = false;
        bool entryComboEsta = false;

        Timer timerGrillaIngresados = new Timer();

        public Valet(string sDocumento, string sNombreUsuario)
        {
            InitializeComponent();

            _DocumentoUsuario = sDocumento;

            SedesResponse oSedesResponse = cliente.ObtenerListaSedes(_DocumentoUsuario);
            
            //Setup data binding
            this.cbSede.DataSource = oSedesResponse.LstInfoSedes;
            this.cbSede.DisplayMember = "Display";
            this.cbSede.ValueMember = "Value";

            EstacionamientosResponse oEstacionamientosResponse = cliente.ObtenerListaEstacionamientoXSede(_DocumentoUsuario, cbSede.SelectedValue.ToString());

            //Setup data binding
            this.cbEstacionamiento.DataSource = oEstacionamientosResponse.LstInfoEstacionamientos;
            this.cbEstacionamiento.DisplayMember = "Display";
            this.cbEstacionamiento.ValueMember = "Value";

            lblUsuario.Text = "Usuario: " + sNombreUsuario;
            lblDocumento.Text = "Documento: " + sDocumento;

            UpdateGrillaIngresados();
            UpdateGrillaSaliendo();

            timerGrillaIngresados.Interval = (10 * 1000); // 10 secs
            timerGrillaIngresados.Tick += new EventHandler(timertimerGrillaIngresados_Tick);
            timerGrillaIngresados.Start();
        }

        private void UpdateGrillaIngresados()
        {
            VehiculosEnValetResponse response = cliente.ObtenerListaVehiculosEnValet(cbEstacionamiento.SelectedValue.ToString(), _DocumentoUsuario);

            //381
            //Setup data binding
            this.grvIngresados.DataSource = response.LstInfoVehiculosEnValet;
            this.grvIngresados.Columns["IdTransaccion"].Visible = false;
            this.grvIngresados.Columns["Estado"].Visible = false;
            this.grvIngresados.Columns["IdEstacionamiento"].Visible = false;

            this.grvIngresados.Columns["Placa"].Width = 81;
            this.grvIngresados.Columns["Color"].Width = 100;
            this.grvIngresados.Columns["Marca"].Width = 100;
            this.grvIngresados.Columns["Ubicacion"].Width = 100;
        }

        private void UpdateGrillaSaliendo()
        {
            VehiculosEnValetResponse response = cliente.ObtenerListaVehiculosSaliendo(cbEstacionamiento.SelectedValue.ToString(), _DocumentoUsuario);

            //381
            //Setup data binding
            this.grvSaliendo.DataSource = response.LstInfoVehiculosEnValet;
            this.grvSaliendo.Columns["IdTransaccion"].Visible = false;
            this.grvSaliendo.Columns["Estado"].Visible = false;
            this.grvSaliendo.Columns["IdEstacionamiento"].Visible = false;

            this.grvSaliendo.Columns["Placa"].Width = 81;
            this.grvSaliendo.Columns["Color"].Width = 100;
            this.grvSaliendo.Columns["Marca"].Width = 100;
            this.grvSaliendo.Columns["Ubicacion"].Width = 100;
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            Ingresar popup = new Ingresar(cbEstacionamiento.SelectedValue.ToString());
            popup.ShowDialog();
            if (popup.DialogResult == DialogResult.OK)
            {
                UpdateGrillaIngresados();
                UpdateGrillaIngresados();
                MessageBox.Show("Error: " + popup.Error, "Valet", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Ocurrio un error al ingresar a Valet", "Valet", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void timertimerGrillaIngresados_Tick(object sender, EventArgs e)
        {
            UpdateGrillaIngresados();
            UpdateGrillaSaliendo();
        }

        private void grvIngresados_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            grvIngresados.ClearSelection();
        }

        private void grvIngresados_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            grvIngresados.ClearSelection();
        }

        private void grvSaliendo_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            grvSaliendo.ClearSelection();
        }

        private void grvSaliendo_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            grvSaliendo.ClearSelection();
        }

        private void cbSede_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (entryComboSede)
            {
                EstacionamientosResponse oEstacionamientosResponse = cliente.ObtenerListaEstacionamientoXSede(_DocumentoUsuario, cbSede.SelectedValue.ToString());

                //Setup data binding
                this.cbEstacionamiento.DataSource = oEstacionamientosResponse.LstInfoEstacionamientos;
                this.cbEstacionamiento.DisplayMember = "Display";
                this.cbEstacionamiento.ValueMember = "Value";
            }
            else
            {
                entryComboSede = true;
            }
        }

        private void cbEstacionamiento_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (entryComboEsta)
            {
                UpdateGrillaIngresados();
                UpdateGrillaSaliendo();
            }
            else
            {
                entryComboEsta = true;
            }
        }
    }
}
