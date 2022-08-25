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
    public partial class CasilleroCasco : Form
    {
        ServicesByP cliente = new ServicesByP();
        private int _IdEstacionamiento = 0;
        public int IdEstacionamiento
        {
            get { return _IdEstacionamiento; }
            set { _IdEstacionamiento = value; }
        }

        public CasilleroCasco(string iIdEstacionamiento)
        {
            InitializeComponent();
            _IdEstacionamiento = Convert.ToInt32(iIdEstacionamiento);
        }

        private void btn_Ok_Click(object sender, EventArgs e)
        {
            string clave = cliente.ObtenerValorParametroxNombre("claveTarjeta", _IdEstacionamiento.ToString());
            if (clave != string.Empty)
            {
                CardResponse oCardResponse = GetCardInfo(clave);

                if (!oCardResponse.error)
                {
                    if (oCardResponse.cicloActivo)
                    {
                        CarrilxIdModuloResponse oCarrilxIdModuloResponse = cliente.ObtenerCarrilxIdModulo(_IdEstacionamiento.ToString(), oCardResponse.moduloEntrada);
                        if (oCarrilxIdModuloResponse.Exito)
                        {
                            string sIdTransaccion = Convert.ToDateTime(oCardResponse.fechEntrada).ToString("yyyyMMddHHmmss") + oCarrilxIdModuloResponse.Carril + _IdEstacionamiento.ToString();


                            //VALIDAR CASILLERO DISPONIBLE
                            bool bLIBRE = false;
                            InfoTransaccionService lstInfo = cliente.ObtenerCasillero(_IdEstacionamiento.ToString());

                            for (int i = 0; i < lstInfo.LstTransac.Length; i++)
                            {
                                if (lstInfo.LstTransac[i].Casillero == tbCasillero.Text)
                                {
                                    bLIBRE = true;
                                    break;
                                }
                                if (lstInfo.LstTransac[i].Casillero == tbCasillero2.Text)
                                {
                                    bLIBRE = true;
                                    break;
                                }
                            }

                            if (!bLIBRE)
                            {

                                DialogResult result3 = MessageBox.Show("¿Desea adicionar la tarifa de casco?", "Aplicar tarifa casco", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                                if (result3 == DialogResult.Yes)
                                {
                                    AplicaCascoResponse oInfo = new AplicaCascoResponse();

                                    oInfo = cliente.AplicarCasco(sIdTransaccion, _IdEstacionamiento.ToString(), tbCasillero.Text);

                                    if (tbCasillero2.Text != string.Empty)
                                    {
                                        oInfo = cliente.AplicarCasco(sIdTransaccion, _IdEstacionamiento.ToString(), tbCasillero2.Text);
                                    }

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
                                    MessageBox.Show("Operacion cancelada.", "PPM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    this.Close();
                                }
                            }
                            else 
                            {
                                this.DialogResult = DialogResult.None;
                                MessageBox.Show("El casillero seleccionado se encuentra ocupado.", "PPM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                this.Close();
                            }
                        }
                        else
                        {
                            this.DialogResult = DialogResult.None;
                            MessageBox.Show("Operacion cancelada.", "PPM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Close();
                        }
                    }
                    else
                    {
                        this.DialogResult = DialogResult.None;
                        MessageBox.Show("Tarjeta sin registro de entrada.", "PPM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }
                }
                else
                {
                    this.DialogResult = DialogResult.None;
                    MessageBox.Show("Error al leer la tarjeta.", "PPM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
            }

            this.Close();
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
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
    }
}
