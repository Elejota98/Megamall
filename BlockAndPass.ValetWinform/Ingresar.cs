using BlockAndPass.ValetWinform.ByPServices;
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

namespace BlockAndPass.ValetWinform
{
    public partial class Ingresar : Form
    {

        ServicesByP cliente = new ServicesByP();

        private string _IdEstacionamiento = string.Empty;
        public string IdEstacionamiento
        {
            get { return _IdEstacionamiento; }
            set { _IdEstacionamiento = value; }
        }

        private string _Error = string.Empty;
        public string Error
        {
            get { return _Error; }
            set { _Error = value; }
        }

        public Ingresar(string sIdEstacionamiento)
        {
            InitializeComponent();
            _IdEstacionamiento = sIdEstacionamiento;
            
        }

        public CardResponse GetCardInfo(string sPass)
        {
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
                        oCardResponse.idCard = resp.CodigoTarjeta;
                        Rspsta_EstablecerClave_LECTOR resp1 = lectora.EstablecerClaveLECTOR(sPass);
                        if (resp1.ClaveEstablecida)
                        {
                            Rspsta_Leer_Tarjeta_LECTOR resp2 = lectora.LeerTarjeta(TYPE_STRUCTURE_SMARTCARD.SMARTCARD_PARKING_V1, false);
                            if (resp2.TarjetaLeida)
                            {
                                oCardResponse.error = false;
                                SMARTCARD_PARKING_V1 myTarjeta = (SMARTCARD_PARKING_V1)resp2.Tarjeta;
                                oCardResponse.cicloActivo = myTarjeta.ActiveCycle != null ? (bool)myTarjeta.ActiveCycle : false;
                                oCardResponse.codeAutorizacion1 = myTarjeta.CodeAgreement1 != null ? (int)myTarjeta.CodeAgreement1 : 0;
                                oCardResponse.codeAutorizacion2 = myTarjeta.CodeAgreement2 != null ? (int)myTarjeta.CodeAgreement2 : 0;
                                oCardResponse.codeAutorizacion3 = myTarjeta.CodeAgreement3 != null ? (int)myTarjeta.CodeAgreement3 : 0;
                                oCardResponse.cortesia = myTarjeta.Courtesy != null ? (bool)myTarjeta.Courtesy : false;
                                oCardResponse.fechEntrada = myTarjeta.DateTimeEntrance;
                                oCardResponse.moduloEntrada = myTarjeta.EntranceModule;
                                oCardResponse.placa = myTarjeta.EntrancePlate;
                                oCardResponse.fechaPago = myTarjeta.PaymentDateTime.ToString();
                                oCardResponse.moduloPago = myTarjeta.PaymentModule;
                                oCardResponse.reposicion = myTarjeta.Replacement != null ? (bool)myTarjeta.Replacement : false;
                                oCardResponse.tipoTarjeta = myTarjeta.TypeCard.ToString();
                                oCardResponse.tipoVehiculo = myTarjeta.TypeVehicle.ToString();
                                oCardResponse.valet = myTarjeta.ValetParking != null ? (bool)myTarjeta.ValetParking : false;
                                //myTarjeta.

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
        public CardResponse AplicarValet(string password)
        {
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
                                myTarjeta.ValetParking = true;
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
        private void btn_Ok_Click(object sender, EventArgs e)
        {
            string clave = cliente.ObtenerValorParametroxNombre("claveTarjeta", _IdEstacionamiento);

            CardResponse oCardResponse = GetCardInfo(clave);
            if (oCardResponse.error != true)
            {
                CarrilxIdModuloResponse oCarrilxIdModuloResponse = cliente.ObtenerCarrilxIdModulo(_IdEstacionamiento, oCardResponse.moduloEntrada);
                if (oCarrilxIdModuloResponse.Exito)
                {
                    string sIdTransaccion = Convert.ToDateTime(oCardResponse.fechEntrada).ToString("yyyyMMddHHmmss") + oCarrilxIdModuloResponse.Carril + _IdEstacionamiento;

                    VehiculosEnValetResponse oVehiculosEnValetResponse = cliente.InsertarVehiculoValet(sIdTransaccion, tbPlaca.Text, tbColor.Text, tbMarca.Text, string.Empty, _IdEstacionamiento);

                    if (oVehiculosEnValetResponse.Exito)
                    {
                        //Crear entrada en tarjeta
                        oCardResponse = AplicarValet(clave);
                        if (oCardResponse.error != true)
                        {
                            this.DialogResult = this.DialogResult = DialogResult.OK;
                            this.Close();
                        }
                        else
                        {
                            _Error = oCardResponse.errorMessage;
                            this.DialogResult = DialogResult.Cancel;
                            this.Close();
                        }
                    }
                    else
                    {
                        _Error = oVehiculosEnValetResponse.ErrorMessage;
                        this.DialogResult = DialogResult.Cancel;
                        this.Close();
                    }
                }
                else
                {
                    _Error = oCarrilxIdModuloResponse.ErrorMessage;
                    this.DialogResult = DialogResult.Cancel;
                    this.Close();
                }
            }
            else
            {
                _Error = oCardResponse.errorMessage;
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }

    #region ClasesAuxiliares
    public class CardResponse
    {
        public bool error { set; get; }
        public string errorMessage { set; get; }
        public string idCard { set; get; }

        public bool cicloActivo { get; set; }
        public bool cortesia { get; set; }
        public bool reposicion { get; set; }
        public bool valet { get; set; }

        public int codeAutorizacion1 { get; set; }
        public int codeAutorizacion2 { get; set; }
        public int codeAutorizacion3 { get; set; }

        public DateTime? fechEntrada { get; set; }
        public string fechaPago { get; set; }

        public string moduloEntrada { get; set; }
        public string moduloPago { get; set; }
        public string placa { get; set; }
        public string tipoTarjeta { get; set; }
        public string tipoVehiculo { set; get; }
    }
    #endregion
}
