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
using System.Text.RegularExpressions;
using LectorDevice;

namespace BlockAndPass.PPMWinform
{
    public partial class CrearIngreso : Form
    {
        Lectora oLectora = new Lectora();
        private int _IdSede = 0;
        public int IdSede
        {
            get { return _IdSede; }
            set { _IdSede = value; }
        }
        private int _IdEstacionamiento = 0;
        public int IdEstacionamiento
        {
            get { return _IdEstacionamiento; }
            set { _IdEstacionamiento = value; }
        }

        private string _IdTarjeta = string.Empty;
        private string _IdAutorizacion = string.Empty;

        ServicesByP cliente = new ServicesByP();

        public CrearIngreso(string iIdSede, string iIdEstacionamiento)
        {
            InitializeComponent();

            _IdSede = Convert.ToInt32(iIdSede);
            _IdEstacionamiento = Convert.ToInt32(iIdEstacionamiento);

            CarrilEntradaXEntradaResponse oInfo = cliente.ObtenerListaCarrilEntradaxEstacionamiento(IdSede, _IdEstacionamiento);
            if (!oInfo.Exito)
            {
                this.DialogResult = DialogResult.None;
                MessageBox.Show("No encontro entradas asociadas al estacionamiento id = " + _IdEstacionamiento, "Error Crear Entrada PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
            else
            {
                var dataSource = new List<Object>();
                foreach (var item in oInfo.LstCarrillesEntrada)
                {
                    dataSource.Add(new { Name = item.Display, Value = item.Value });
                }

                //Setup data binding
                this.cbEntrada.DataSource = dataSource;
                this.cbEntrada.DisplayMember = "Name";
                this.cbEntrada.ValueMember = "Value";
            }

            List<KeyValuePair<int, string>> miLista = new List<KeyValuePair<int, string>>();
            miLista.Add(new KeyValuePair<int, string>(1, "Carro"));
            miLista.Add(new KeyValuePair<int, string>(2, "Moto"));
            
           
            

            cbTipoVehiculo.DataSource = miLista;
            cbTipoVehiculo.DisplayMember = "Value";
            cbTipoVehiculo.ValueMember = "Key";
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                btn_Continuar_Click(btn_Continuar, EventArgs.Empty);
            }
        }

        private void btn_Continuar_Click(object sender, EventArgs e)
        {
            //  Regex pattern = new Regex("[^A-Za-z0-9.]");
            Regex pattern = new Regex("^[A-Za-z0-9]{6}\\.{0,1}$");
            if (tbPlaca.Text != string.Empty)
            {
                if (!pattern.IsMatch(tbPlaca.Text))
                {
                    MessageBox.Show("Recuerde digitar solo Letras y números ", "Error Crear Entrada PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    btn_Continuar.Enabled = false;

                    if (EsAutorizado(tbPlaca.Text))
                    {
                        if (!TieneVigencia(tbPlaca.Text))
                        {
                            this.DialogResult = DialogResult.None;
                            MessageBox.Show("El autorizado tiene la vigencia vencida.", "Error Crear Entrada PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            this.Close();
                        }
                        else
                        {
                            if (TieneTransaccionAbierta(tbPlaca.Text))
                            {
                                this.DialogResult = DialogResult.None;
                                MessageBox.Show("El autorizado tiene una transaccion pendiente de salida.", "Error Crear Entrada PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                this.Close();
                            }
                            else
                            {
                                chbAutorizado.Checked = true;
                                chbAutorizado.Visible = true;
                            }
                        }
                    }
                    else
                    {
                        chbAutorizado.Checked = false;
                        chbAutorizado.Visible = false;
                    }

                    panelCreacion.Visible = true;
                    btn_Ok.Visible = true;
                }
            }
        }

        private bool EsAutorizado(string sPlaca)
        {
            bool bResultado = false;

            AutorizadoxPlacaResponse resp = cliente.BuscarAutorizadoxPlaca(tbPlaca.Text);

            if (resp.Exito)
            {
                _IdTarjeta = resp.IdTarjeta;
                _IdAutorizacion = resp.IdAutorizacion;
                bResultado = true;
            }

            return bResultado;
        }

        private bool TieneTransaccionAbierta(string sPlaca)
        {
            bool bResultado = false;

            VerificaTransaccionAbiertaAutorizadoResponse resp = cliente.VerificarTransaccionAbiertaAutorizado(_IdTarjeta);
            if (resp.Exito)
            {
                bResultado = true;
            }

            return bResultado;
        }

        private bool TieneVigencia(string sPlaca)
        {
            bool bResultado = false;

            VerificaVigenciaAutorizadoResponse resp = cliente.VerificarVigenciaAutorizado(_IdTarjeta);
            if (resp.Exito)
            {
                bResultado = true;
            }

            return bResultado;
        }

        private void btn_Ok_Click(object sender, EventArgs e)
        {
            if (chbAutorizado.Checked)
            {
                DialogResult result3 = MessageBox.Show("¿Desea crear una entrada de autorizado SIN tarjeta? \n TENGA EN CUENTA QUE NO PODRA CAMBIAR ESTA SELECCION", "Crear Entrada", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (result3 == DialogResult.Yes)
                {
                    CreaEntradaResponse oInfo = cliente.CrearEntrada(_IdEstacionamiento.ToString(), _IdTarjeta, cbEntrada.Text, tbPlaca.Text, dtpFechaIngreso.Value, cbTipoVehiculo.SelectedValue.ToString(), _IdAutorizacion);

                    if (oInfo.Exito)
                    {
                        this.DialogResult = DialogResult.OK;
                    }
                    else
                    {
                        this.DialogResult = DialogResult.None;
                        MessageBox.Show(oInfo.ErrorMessage, "Error Crear Entrada PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Coloque la tarjeta en el lector y presione continuar.", "Crear Entrada", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);

                    string clave = cliente.ObtenerValorParametroxNombre("ClaveTarjeta", _IdEstacionamiento.ToString());

                    CardResponse oCardResponse = CreateAuthEntry(clave, tbPlaca.Text, cbEntrada.Text, dtpFechaIngreso.Value, cbTipoVehiculo.SelectedValue.ToString(), _IdTarjeta);

                    if (!oCardResponse.error)
                    {
                        CreaEntradaResponse oInfo = cliente.CrearEntrada(_IdEstacionamiento.ToString(), _IdTarjeta, cbEntrada.Text, tbPlaca.Text, dtpFechaIngreso.Value, cbTipoVehiculo.SelectedValue.ToString(), _IdAutorizacion);

                        if (oInfo.Exito)
                        {
                            this.DialogResult = DialogResult.OK;
                        }
                        else
                        {
                            this.DialogResult = DialogResult.None;
                            MessageBox.Show(oInfo.ErrorMessage, "Error Crear Entrada PPM Clienete Normal", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        this.DialogResult = DialogResult.None;
                        MessageBox.Show(oCardResponse.errorMessage, "Error Crear Entrada PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Coloque la tarjeta en el lector y presione continuar.", "Crear Entrada", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);

                string clave = cliente.ObtenerValorParametroxNombre("ClaveTarjetasCRT", _IdEstacionamiento.ToString());

                //CardResponse oCardResponse = CreateEntry(clave, tbPlaca.Text, cbEntrada.Text, dtpFechaIngreso.Value, cbTipoVehiculo.SelectedValue.ToString());

                ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

                if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
                {
                    //textBox2.Text = oResultadoOperacion.EntidadDatos.ToString();
                    oResultadoOperacion = oLectora.ObtenerID();
                    if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
                    {
                        oResultadoOperacion = oLectora.LeerTI(clave);

                        if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
                        {
                            Tarjeta oTarjeta = new Tarjeta();

                            oTarjeta = (Tarjeta)oResultadoOperacion.EntidadDatos;

                            DateTime fechan = Convert.ToDateTime(dtpFechaIngreso.Text);
                            oTarjeta.ActiveCycle = true;
                            oTarjeta.DateTimeEntrance = fechan;
                            if (cbTipoVehiculo.SelectedValue.ToString() == "1")
                            {
                                oTarjeta.TypeVehicle = 1;
                            }
                            else
                            {
                                oTarjeta.TypeVehicle = 2;
                            }
                            //oTarjeta.TypeVehicle = 1;
                            oTarjeta.TypeCard = 1;
                            if (oTarjeta.TypeVehicle == 1)
                            {
                                oTarjeta.EntranceModule = "ENTR01";
                            }
                            else
                            {
                                oTarjeta.EntranceModule = "ENTR03";
                            }
                            oTarjeta.CodeAgreement1 = 0;

                            oResultadoOperacion = oLectora.EscribirTI(oTarjeta);

                            //if (!oCardResponse.error)
                            //{
                            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
                            {
                                oResultadoOperacion = oLectora.ObtenerID();
                                CreaEntradaResponse oInfo = cliente.CrearEntrada(_IdEstacionamiento.ToString(), oResultadoOperacion.EntidadDatos.ToString(), oTarjeta.EntranceModule, tbPlaca.Text, dtpFechaIngreso.Value, cbTipoVehiculo.SelectedValue.ToString(), string.Empty);

                                oLectora.DesconectarCRT();


                                if (oInfo.Exito)
                                {
                                    this.DialogResult = DialogResult.OK;
                                }
                                else
                                {
                                    this.DialogResult = DialogResult.None;
                                    MessageBox.Show(oInfo.ErrorMessage, "Error Crear Entrada PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            else
                            {
                                this.DialogResult = DialogResult.None;
                                MessageBox.Show("Error al crear la entrada, intente con otra tarjeta", "Error Crear Entrada PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            this.DialogResult = DialogResult.None;
                            MessageBox.Show("No se pudo crear la entrada", "Error Crear Entrada PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        this.DialogResult = DialogResult.None;
                        MessageBox.Show("No se pudo crear la entrada", "Error Crear Entrada PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            this.Close();
        }

        public CardResponse CreateEntry(string password, string plate, string modulo, DateTime dtFecha, string tipov)
        {
            dtFecha = new DateTime(dtFecha.Year, dtFecha.Month, dtFecha.Day, dtFecha.Hour, dtFecha.Minute, dtFecha.Second);
            CardResponse oCardResponse = new CardResponse();


            Lectora_ACR122U lectora = new Lectora_ACR122U();
            Rspsta_Conexion_LECTOR res = lectora.Conectar(false);
            if (res.Conectado)
            {
                Rspsta_DetectarTarjeta_LECTOR re = lectora.DetectarTarjeta();
                if (re.TarjetaDetectada)
                {
                    Rspsta_CodigoTarjeta_LECTOR resp = lectora.ObtenerIDTarjeta();
                    if (resp.CodigoTarjeta != null && resp.CodigoTarjeta != string.Empty)
                    {
                        Rspsta_EstablecerClave_LECTOR resp1 = lectora.EstablecerClaveLECTOR(password);
                        if (resp1.ClaveEstablecida)
                        {

                            Rspsta_BorrarTarjeta_LECTOR respBorrar = lectora.BorrarTarjetaLECTORA(TYPE_STRUCTURE_SMARTCARD.SMARTCARD_PARKING_V1, false);

                            if (respBorrar.TarjetaBorrada)
                            {
                                SMARTCARD_PARKING_V1 oTarjeta = new SMARTCARD_PARKING_V1();
                                oTarjeta.TypeCard = TYPE_TARJETAPARKING_V1.VISITOR;
                                if (tipov == "1")
                                {
                                    oTarjeta.TypeVehicle = TYPEVEHICLE_TARJETAPARKING_V1.AUTOMOBILE;
                                }
                                else if (tipov == "2")
                                {
                                    oTarjeta.TypeVehicle = TYPEVEHICLE_TARJETAPARKING_V1.MOTORCYCLE;
                                }
                                oTarjeta.Replacement = false;
                                oTarjeta.CodeCard = resp.CodigoTarjeta;
                                oTarjeta.ActiveCycle = true;
                                oTarjeta.DateTimeEntrance = dtFecha;
                                oTarjeta.EntranceModule = modulo;
                                oTarjeta.EntrancePlate = plate;
                                Rspsta_Escribir_Tarjeta_LECTOR resp4 = lectora.EscribirTarjeta(oTarjeta, false, false);
                                if (resp4.TarjetaEscrita)
                                {
                                    oCardResponse.idCard = resp.CodigoTarjeta;
                                    oCardResponse.error = false;
                                }
                                else
                                {
                                    oCardResponse.error = true;
                                    oCardResponse.errorMessage = "No escribe tarjeta.";
                                }
                            }
                            else
                            {
                                oCardResponse.error = true;
                                oCardResponse.errorMessage = "No borra tarjeta.";
                            }
                        }
                        else
                        {
                            oCardResponse.error = true;
                            oCardResponse.errorMessage = "No establece clave tarjeta.";
                        }
                    }
                    else
                    {
                        oCardResponse.error = true;
                        oCardResponse.errorMessage = "No obtiene id tarjeta.";
                    }
                }
                else
                {
                    oCardResponse.error = true;
                    oCardResponse.errorMessage = "No detecta tarjeta.";
                }
            }
            else
            {
                oCardResponse.error = true;
                oCardResponse.errorMessage = "No conecta con lectora de tarjetas.";
            }


            return oCardResponse;
        }

        public CardResponse CreateAuthEntry(string password, string plate, string modulo, DateTime dtFecha, string tipov, string sIdTarjeta)
        {
            dtFecha = new DateTime(dtFecha.Year, dtFecha.Month, dtFecha.Day, dtFecha.Hour, dtFecha.Minute, dtFecha.Second);
            CardResponse oCardResponse = new CardResponse();


            Lectora_ACR122U lectora = new Lectora_ACR122U();
            Rspsta_Conexion_LECTOR res = lectora.Conectar(false);
            if (res.Conectado)
            {
                Rspsta_DetectarTarjeta_LECTOR re = lectora.DetectarTarjeta();
                if (re.TarjetaDetectada)
                {
                    Rspsta_CodigoTarjeta_LECTOR resp = lectora.ObtenerIDTarjeta();
                    if (resp.CodigoTarjeta != null && resp.CodigoTarjeta != string.Empty)
                    {
                        Rspsta_EstablecerClave_LECTOR resp1 = lectora.EstablecerClaveLECTOR(password);
                        if (resp1.ClaveEstablecida)
                        {
                            Rspsta_Leer_Tarjeta_LECTOR resp2 = lectora.LeerTarjeta(TYPE_STRUCTURE_SMARTCARD.SMARTCARD_PARKING_V1, false);
                            if (resp2.TarjetaLeida)
                            {
                                SMARTCARD_PARKING_V1 myTarjeta = (SMARTCARD_PARKING_V1)resp2.Tarjeta;

                                if (!Convert.ToBoolean(myTarjeta.ActiveCycle))
                                {

                                    if (tipov == "1")
                                    {
                                        myTarjeta.TypeVehicle = TYPEVEHICLE_TARJETAPARKING_V1.AUTOMOBILE;
                                    }
                                    else if (tipov == "2")
                                    {
                                        myTarjeta.TypeVehicle = TYPEVEHICLE_TARJETAPARKING_V1.MOTORCYCLE;
                                    }
                                    myTarjeta.Replacement = false;
                                    myTarjeta.CodeCard = sIdTarjeta;
                                    myTarjeta.ActiveCycle = true;
                                    myTarjeta.DateTimeEntrance = dtFecha;
                                    myTarjeta.EntranceModule = modulo;
                                    myTarjeta.EntrancePlate = plate;

                                    Rspsta_Escribir_Tarjeta_LECTOR resp4 = lectora.EscribirTarjeta(myTarjeta, false, false);
                                    if (resp4.TarjetaEscrita)
                                    {
                                        oCardResponse.error = false;
                                    }
                                    else
                                    {
                                        oCardResponse.error = true;
                                        oCardResponse.errorMessage = "No escribe tarjeta.";
                                    }
                                }
                                else
                                {
                                    oCardResponse.error = true;
                                    oCardResponse.errorMessage = "Tarjeta con entrada registrada.";
                                }
                            }
                            else
                            {
                                oCardResponse.error = true;
                                oCardResponse.errorMessage = "No lee tarjeta.";
                            }
                        }
                        else
                        {
                            oCardResponse.error = true;
                            oCardResponse.errorMessage = "No establece clave tarjeta.";
                        }
                    }
                    else
                    {
                        oCardResponse.error = true;
                        oCardResponse.errorMessage = "No obtiene id tarjeta.";
                    }
                }
                else
                {
                    oCardResponse.error = true;
                    oCardResponse.errorMessage = "No detecta tarjeta.";
                }
            }
            else
            {
                oCardResponse.error = true;
                oCardResponse.errorMessage = "No conecta con lectora de tarjetas.";
            }


            return oCardResponse;
        }

        private void dtpFechaIngreso_ValueChanged(object sender, EventArgs e)
        {

        }

        private void cbTipoVehiculo_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void chbAutorizado_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void tbPlaca_TextChanged(object sender, EventArgs e)
        {
        }

    }
}
