using BlockAndPass.PPMWinform.ByPServices;
using BlockAndPass.PPMWinform.Tickets;
using EGlobalT.Device.SmartCard;
using EGlobalT.Device.SmartCardReaders;
using EGlobalT.Device.SmartCardReaders.Entities;
using LectorDevice;
using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace BlockAndPass.PPMWinform
{
    public partial class PPM : Form, IMessageFilter
    {
        Lectora oLectora = new Lectora();
        

        #region MoveWindow
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        public const int WM_LBUTTONDOWN = 0x0201;
        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        private HashSet<Control> controlsToMove = new HashSet<Control>();
        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg == WM_LBUTTONDOWN &&
                 controlsToMove.Contains(Control.FromHandle(m.HWnd)))
            {
                ReleaseCapture();
                SendMessage(this.Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
                return true;
            }
            return false;
        }
        #endregion

        #region Definiciones
        public string NombreConvenio = string.Empty;
        public string Id_Convenio = string.Empty;
        public string _FechaEntrda = string.Empty;
        private string _DocumentoUsuario = string.Empty;
        public string DocumentoUsuario
        {
            get { return _DocumentoUsuario; }
            set { _DocumentoUsuario = value; }
        }
        ServicesByP cliente = new ServicesByP();
        LiquidacionService liquidacion = new LiquidacionService();
        System.Windows.Forms.Timer tmrHora = new System.Windows.Forms.Timer();
        System.Windows.Forms.Timer tmrTimeOutPago = new System.Windows.Forms.Timer();
        int cnt = 0;
        #endregion

        #region Eventos
        void tmrTimeOutPago_Tick(object sender, EventArgs e)
        {
            //cnt++;

            ////int valorfinal = Convert.toInlblTiempoFuera.Text.Replace("Usted dispone de ","").Replace(" segundos para pagar.","");

            //lblTiempoFuera.Text = "Usted dispone de " + (40 - cnt) + " segundos para pagar.";

            //if (cnt >= 40)
            //{
            //    tmrTimeOutPago.Stop();
            //    RestablecerPPM();
            //    panelPagar.Enabled = false;
            //}
        }
        void tmrHora_Tick(object sender, EventArgs e)
        {
            lblHoraPago.Text = "Hora Actual: " + GetTimestamp(DateTime.Now);
        }
        private void tbRecibido_TextChanged(object sender, EventArgs e)
        {
            //MessageBox.Show(tbCambio.Text);
            Int64 pagar = Convert.ToInt64(tbValorPagar.Text.Replace("$", "").Replace(".", ""));
            //Int64 recibido = Convert.ToInt64(tbRecibido.Text.Replace("$", ""));
            Int64 cambio = Convert.ToInt64(tbCambio.Text.Replace("$", "").Replace(".", ""));

            Int64 recibido = 0;
            try
            {
                if (Int64.TryParse(tbRecibido.Text.Replace("$", "").Replace(".", ""), out recibido))
                {
                    tbRecibido.Text = "$" + string.Format("{0:#,##0.##}", recibido);

                    if (recibido > pagar)
                    {
                        //MessageBox.Show(recibido + " " + pagar);
                        tbCambio.Text = "$" + string.Format("{0:#,##0.##}", (recibido - pagar));
                    }
                    else
                    {
                        tbCambio.Text = "$0";
                    }
                }
                else
                {
                    tbRecibido.Text = string.Empty;
                }
            }
            catch (Exception exe)
            {
                //MessageBox.Show(exe.InnerException.ToString() + " " + exe.Message + " " + );
            }

            tbRecibido.SelectionStart = tbRecibido.Text.Length; // add some logic if length is 0
            tbRecibido.SelectionLength = 0;
        }
        private void btn_Limpiar_Click(object sender, EventArgs e)
        {
            tmrTimeOutPago.Stop();
            RestablecerPPM();
        }
        private void btn_Pagar_Click(object sender, EventArgs e)
        {
            string valor = string.Empty;
            if (Id_Convenio != "0" && Id_Convenio != null && Id_Convenio != string.Empty && Id_Convenio != "32")
            {
                valor = cliente.InsertaIdConvTrans(tbIdTransaccion.Text, Id_Convenio);
            }

            if (Convert.ToInt64(tbValorPagar.Text.Replace("$", "").Replace(".", "")) > 0)
            {
                cnt = 0;
                tmrTimeOutPago.Stop();
                Cargando(true);
                string clave = cliente.ObtenerValorParametroxNombre("ClaveTarjetasCRT", cbEstacionamiento.SelectedValue.ToString());
                if (clave != string.Empty)
                {
                    #region Estacionamiento
                    if (chbEstacionamiento.Checked)
                    {
                        CardResponse oCardResponse = new CardResponse();
                        oCardResponse = PayCard(clave, tbIdTarjeta.Text, cbPPM.SelectedValue.ToString(), DateTime.ParseExact(tbHoraPago.Text, "dd'/'MM'/'yyyy HH':'mm':'ss", CultureInfo.CurrentCulture));
                        if (!oCardResponse.error)
                        {
                            string pagosFinal = "";
                            double sumTotalPagar = 0;
                            foreach (DatosLiquidacionService item in liquidacion.LstLiquidacion)
                            {
                                if (pagosFinal == string.Empty)
                                {
                                }
                                else
                                {
                                    pagosFinal += ',';
                                }
                                sumTotalPagar += item.Total;
                                pagosFinal += item.Tipo + "-" + item.SubTotal + "-" + item.Iva + "-" + item.Total;
                            }

                            InfoPagoNormalService pagoNormal = cliente.PagarClienteParticular(pagosFinal, cbEstacionamiento.SelectedValue.ToString(), tbIdTransaccion.Text, cbPPM.SelectedValue.ToString(), oCardResponse.fechaPago, sumTotalPagar.ToString());

                            if (pagoNormal.Exito)
                            {   
                                oLectora.BorrarReposicion(clave);
                                ImprimirPagoNormal(tbIdTransaccion.Text);
                            }
                            else
                            {
                                Cargando(false);
                                MessageBox.Show(pagoNormal.ErrorMessage, "Error Pagar PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            Cargando(false);
                            MessageBox.Show(oCardResponse.errorMessage, "Error Pagar PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    #endregion
                    #region Mensualidad
                    else
                    {
                        //Mensualidad
                        string pagosFinal = "";
                        double sumTotalPagar = 0;
                        foreach (DatosLiquidacionService item in liquidacion.LstLiquidacion)
                        {
                            sumTotalPagar += item.Total;
                            pagosFinal += item.Tipo + "-" + item.SubTotal + "-" + item.Iva + "-" + item.Total;
                        }

                        InfoPagoMensualidadService pagoNormal = cliente.PagarMensualidad(pagosFinal, cbEstacionamiento.SelectedValue.ToString(), cbPPM.SelectedValue.ToString(), DateTime.Now.ToString(), sumTotalPagar.ToString(), tbIdTarjeta.Text);

                        if (pagoNormal.Exito)
                        {
                            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();
                            
                            oLectora.BorrarReposicion(clave);

                           if(oResultadoOperacion.oEstado == TipoRespuesta.Exito)
                           {
                                ImprimirPagoMensualidad(pagoNormal.IdTranaccion, pagoNormal.IdAutorizacion);
                            }
                            else if (ckMensualidadDocumento.Checked == true && txtPlaca.Text != null)
                            {
                                

                                ImprimirPagoMensualidad(pagoNormal.IdTranaccion, pagoNormal.IdAutorizacion);
                            }
                        }
                        else
                        {
                            Cargando(false);
                            MessageBox.Show(pagoNormal.ErrorMessage, "Error Pagar Mensualidad PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    #endregion
                }
            }
            else
            {
                if (NombreConvenio != string.Empty && valor.Equals("NO VALIDADO"))
                {
                    ImprimirValidacion();
                    //MessageBox.Show("!Tiene 10 minutos para saslir!", "PPM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {

                    //DialogResult result3 = MessageBox.Show("Valor a pagar = 0 ¿Desea crear la salida para la transaccion: " + tbIdTransaccion.Text, "Crear Salida", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                    //if (result3 == DialogResult.Yes)
                    //{
                    //    CreaSalidaResponse resp = cliente.CrearSalida3(cbEstacionamiento.SelectedValue.ToString(), tbIdTransaccion.Text, cbPPM.SelectedValue.ToString());

                    //    if (resp.Exito)
                    //    {
                    //        MessageBox.Show("Salida registrada exitosamente.", "Pagar PPM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //    }
                    //    else
                    //    {
                    //        MessageBox.Show(resp.ErrorMessage, "Error Crear Salida PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //    }
                    //}
                    //else
                    //{
                    //    MessageBox.Show("Valor a Pagar = 0, no se registro la salida", "Pagar PPM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //}

                }
                Cargando(false);

                RestablecerPPM();
            }
        }
        private void btn_Cerrar_Click(object sender, EventArgs e)
        {
            this.Close();
            Application.Exit();
        }
        private void btn_Leer_Click(object sender, EventArgs e)
        {
            Tarjeta oTarjeta = new Tarjeta();
            string idcard = string.Empty;
            //liquidacion = cliente.ConsultarValorPagar(true, true, 1, "0", "0EEEC6CB");
            cnt = 0;
            Cargando(true);
            string clave = cliente.ObtenerValorParametroxNombre("ClaveTarjetasCRT", cbEstacionamiento.SelectedValue.ToString());
            if (clave != string.Empty)
            {
                ResultadoOperacion oResultadoOperacion1 = new ResultadoOperacion();

                oResultadoOperacion1 = oLectora.ObtenerID();

                if (oResultadoOperacion1.oEstado == TipoRespuesta.Exito)
                {

                    idcard = oResultadoOperacion1.EntidadDatos.ToString();

                    bool bContinuarLiquidacion = true;

                    oResultadoOperacion1 = oLectora.LeerTI(clave);
                    oTarjeta = (Tarjeta)oResultadoOperacion1.EntidadDatos;


                    ValidarIdTarjetaAutorizadoResponse tarjetAutorizado = cliente.ValidarTarjetaAutorizado(idcard);

                    #region Estacionamiento
                    if (!tarjetAutorizado.Exito)
                    {
                        #region CrearSalidaOld
                        //if (oTarjeta.Ty == "AUTHORIZED_PARKING")
                        //{
                        //    DialogResult result3 = MessageBox.Show("¿Desea crear una salida de autorizado? \n PRESIONE NO PARA CONTINUAR CON LA LIQUIDACION.", "Crear Salida", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                        //    if (result3 == DialogResult.Yes)
                        //    {

                        //        DialogResult result4 = MessageBox.Show("¿Esta seguro que desea crear la salida para la placa: " + oCardResponse.placa, "Crear Salida", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                        //        if (result4 == DialogResult.Yes)
                        //        {
                        //            bContinuarLiquidacion = false;
                        //            CarrilxIdModuloResponse oCarrilxIdModuloResponse = cliente.ObtenerCarrilxIdModulo(cbEstacionamiento.SelectedValue.ToString(), oCardResponse.moduloEntrada);
                        //            if (oCarrilxIdModuloResponse.Exito)
                        //            {
                        //                string sIdTransaccion = Convert.ToDateTime(oCardResponse.fechEntrada).ToString("yyyyMMddHHmmss") + oCarrilxIdModuloResponse.Carril + cbEstacionamiento.SelectedValue.ToString();

                        //                CardResponse oCardResponseExit = new CardResponse();
                        //                oCardResponseExit = ExitCardAutho(clave, oCardResponse.idCard);
                        //                if (!oCardResponseExit.error)
                        //                {
                        //                    CreaSalidaResponse resp = cliente.CrearSalida2(cbEstacionamiento.SelectedValue.ToString(), oCardResponse.placa, sIdTransaccion, oCarrilxIdModuloResponse.Carril.ToString(), oCardResponse.moduloEntrada, oCardResponse.idCard);

                        //                    if (resp.Exito)
                        //                    {
                        //                        MessageBox.Show("Salida creada con EXITO", "Crear Salida PPM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //                    }
                        //                    else
                        //                    {
                        //                        this.DialogResult = DialogResult.None;
                        //                        MessageBox.Show(resp.ErrorMessage, "Error Crear Salida PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //                    }
                        //                }
                        //                else
                        //                {
                        //                    Cargando(false);
                        //                    MessageBox.Show(oCardResponseExit.errorMessage, "Error Crear Salida PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //                }

                        //            }
                        //            else
                        //            {
                        //                MessageBox.Show("No fue posible encontrar el carril asociado al modulo.", "Error Crear Salida PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //            }
                        //        }
                        //    }
                        //}
                        #endregion

                        if (bContinuarLiquidacion)
                        {
                            CarrilxIdModuloResponse oCarrilxIdModuloResponse = cliente.ObtenerCarrilxIdModulo(cbEstacionamiento.SelectedValue.ToString(), oTarjeta.EntranceModule);
                            if (oCarrilxIdModuloResponse.Exito)
                            {
                                //yyyyMMddHHmmssce

                                oResultadoOperacion1 = oLectora.ObtenerID();

                                if (oResultadoOperacion1.oEstado == TipoRespuesta.Exito)
                                {
                                    tbIdTarjeta.Text = oResultadoOperacion1.EntidadDatos.ToString();
                                }
                                //string sIdTransaccion = Convert.ToDateTime(oTarjeta.DateTimeEntrance).ToString("yyyyMMddHHmmss") + oCarrilxIdModuloResponse.Carril + cbEstacionamiento.SelectedValue.ToString();
                                InfoTransaccionService oInfoTransaccionService1 = cliente.ConsultarInfoTransaccion(cbEstacionamiento.SelectedValue.ToString(), tbIdTarjeta.Text, oTarjeta.EntranceModule);

                                string sIdTransaccion = oInfoTransaccionService1.IdTransaccion;

                                InfoTransaccionService oInfoTransaccionService = cliente.ConsultarInfoTransaccionxId(sIdTransaccion);
                                InfoTransaccionService lstInfo = cliente.ConsultarCascosxId(sIdTransaccion);

                                NombreConvenio = cliente.ObtenerNombreConvenio(oTarjeta.CodeAgreement1.ToString());
                                Id_Convenio = oTarjeta.CodeAgreement1.ToString();

                                //InfoTransaccionService oInfoTransaccionService = cliente.ConsultarInfoTransaccion(cbEstacionamiento.SelectedValue.ToString(), oCardResponse.idCard, oCardResponse.moduloEntrada);
                                if (oInfoTransaccionService.Exito)
                                {
                                    if (oInfoTransaccionService.IdTransaccion != string.Empty)
                                    {
                                        tbIdTransaccion.Text = oInfoTransaccionService.IdTransaccion;

                                        tbPlaca.Text = oTarjeta.EntrancePlate;
                                        textBox1.Text = NombreConvenio;

                                        if (oTarjeta.CodeAgreement1 != 0)
                                        {
                                            cliente.AplicarConvenios(oInfoTransaccionService.IdTransaccion, Convert.ToInt32(oTarjeta.CodeAgreement1), Convert.ToInt32(oTarjeta.CodeAgreement2), Convert.ToInt32(oTarjeta.CodeAgreement3));
                                        }

                                        DateTime dtDespues = DateTime.Now;
                                        DateTime dtAntes = new DateTime();

                                        //MessageBox.Show(oInfoTransaccionService.HoraTransaccion);

                                        oInfoTransaccionService.HoraTransaccion = oInfoTransaccionService.HoraTransaccion.Replace("a. m.", "a.m.");
                                        oInfoTransaccionService.HoraTransaccion = oInfoTransaccionService.HoraTransaccion.Replace("p. m.", "p.m.");

                                        //try
                                        //{
                                        //    if (!DateTime.TryParseExact(oInfoTransaccionService.HoraTransaccion, "dd'/'MM'/'yyyy hh':'mm':'ss tt", CultureInfo.CurrentCulture, DateTimeStyles.None, out dtAntes))
                                        //    {
                                        //        if (!DateTime.TryParseExact(oInfoTransaccionService.HoraTransaccion, "d'/'MM'/'yyyy hh':'mm':'ss tt", CultureInfo.CurrentCulture, DateTimeStyles.None, out dtAntes))
                                        //        {
                                        //            if (!DateTime.TryParseExact(oInfoTransaccionService.HoraTransaccion, "dd'/'MM'/'yyyy h':'mm':'ss tt", CultureInfo.CurrentCulture, DateTimeStyles.None, out dtAntes))
                                        //            {
                                        //                if (!DateTime.TryParseExact(oInfoTransaccionService.HoraTransaccion, "d'/'MM'/'yyyy h':'mm':'ss tt", CultureInfo.CurrentCulture, DateTimeStyles.None, out dtAntes))
                                        //                {
                                        //                    if (!DateTime.TryParseExact(oInfoTransaccionService.HoraTransaccion, "dd'/'MM'/'yyyy H':'mm':'ss", CultureInfo.CurrentCulture, DateTimeStyles.None, out dtAntes))
                                        //                    {
                                        //                        dtAntes = DateTime.ParseExact(oInfoTransaccionService.HoraTransaccion, "dd'/'MM'/'yyyy HH':'mm':'ss", CultureInfo.CurrentCulture);
                                        //                    }
                                        //                }
                                        //            }
                                        //        }
                                        //    }
                                        //}
                                        //catch (Exception exe)
                                        //{
                                        //    MessageBox.Show("Tiempo de permanencia no disponible, continue con el pago e informe al desarrollador->" + exe.Message + " / " + oInfoTransaccionService.HoraTransaccion, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        //}

                                        DateTime? ntes = oTarjeta.DateTimeEntrance;
                                        dtAntes = Convert.ToDateTime(ntes);

                                        tbHoraIngreso.Text = dtAntes.ToString("dd'/'MM'/'yyyy HH':'mm':'ss");
                                        tbHoraPago.Text = dtDespues.ToString("dd'/'MM'/'yyyy HH':'mm':'ss");

                                        if (lstInfo.LstTransac.Length == 1)
                                        {
                                            tbCasillero.Text = lstInfo.LstTransac[0].Casillero;
                                        }
                                        else if (lstInfo.LstTransac.Length == 2)
                                        {
                                            tbCasillero.Text = lstInfo.LstTransac[0].Casillero + " y " + lstInfo.LstTransac[1].Casillero;
                                        }
                                        else
                                        {
                                            tbCasillero.Text = string.Empty;
                                        }

                                        TimeSpan ts = dtDespues - dtAntes;
                                        tbTiempo.Text = Convert.ToInt32(ts.TotalMinutes).ToString() + " minutos";

                                        int tipoVehiculo = 0;
                                        if (oTarjeta.TypeVehicle == 1)
                                        {
                                            tipoVehiculo = 1;
                                        }
                                        else if (oTarjeta.TypeVehicle == 2)
                                        {
                                            tipoVehiculo = 2;
                                        }
                                        else if (oTarjeta.TypeVehicle == 3)
                                        {
                                            tipoVehiculo = 3;
                                        }

                                        liquidacion = cliente.ConsultarValorPagar(false, Convert.ToBoolean(oTarjeta.Replacement), tipoVehiculo, oInfoTransaccionService.IdTransaccion, tbIdTarjeta.Text);
                                        double sumTotalPagar = 0;
                                        #region Estacionamiento
                                        if (liquidacion.Exito)
                                        {
                                            if (liquidacion.LstLiquidacion.Length > 0)
                                            {
                                                foreach (DatosLiquidacionService item in liquidacion.LstLiquidacion)
                                                {
                                                    sumTotalPagar += item.Total;
                                                }

                                                tbValorPagar.Text = "$" + string.Format("{0:#,##0.##}", sumTotalPagar);
                                                tbCambio.Text = "$0";
                                                tbRecibido.Text = "0";


                                                Cargando(false);
                                                panelPagar.Enabled = true;
                                                tmrTimeOutPago.Start();

                                                chbEstacionamiento.Checked = true;
                                                chbMensualidad.Checked = false;

                                                lblTiempoFuera.Text = "Usted dispone de 40 segundos para pagar.";

                                            }
                                            else
                                            {
                                                //MessageBox.Show("No obtiene valor a pagar idTransaccion = " + oInfoTransaccionService.IdTransaccion, "Error Leer PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                liquidacion = cliente.ConsultarValorPagar(true, Convert.ToBoolean(oTarjeta.Replacement), 1, "0", tbIdTarjeta.Text);
                                                sumTotalPagar = 0;
                                                if (liquidacion.Exito)
                                                {
                                                    foreach (DatosLiquidacionService item in liquidacion.LstLiquidacion)
                                                    {
                                                        sumTotalPagar += item.Total;
                                                    }

                                                    tbValorPagar.Text = "$" + string.Format("{0:#,##0.##}", sumTotalPagar);
                                                    tbCambio.Text = "$0";
                                                    tbRecibido.Text = "0";

                                                    Cargando(false);
                                                    panelPagar.Enabled = true;
                                                    tmrTimeOutPago.Start();

                                                    chbEstacionamiento.Checked = false;
                                                    chbMensualidad.Checked = true;

                                                    lblTiempoFuera.Text = "Usted dispone de 40 segundos para pagar.";
                                                }
                                                else
                                                {
                                                    Cargando(false);
                                                    MessageBox.Show("No obtiene valor a pagar mensualidad", "Error Leer PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                }
                                            }
                                        }
                                        #endregion
                                        #region Mensualidad
                                        else
                                        {
                                            //MessageBox.Show("No obtiene valor a pagar idTransaccion = " + oInfoTransaccionService.IdTransaccion, "Error Leer PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            liquidacion = cliente.ConsultarValorPagar(true, Convert.ToBoolean(oTarjeta.Replacement), 1, "0", tbIdTarjeta.Text);
                                            sumTotalPagar = 0;
                                            if (liquidacion.Exito)
                                            {
                                                foreach (DatosLiquidacionService item in liquidacion.LstLiquidacion)
                                                {
                                                    sumTotalPagar += item.Total;
                                                }

                                                if (sumTotalPagar > 0)
                                                {
                                                    tbValorPagar.Text = "$" + string.Format("{0:#,##0.##}", sumTotalPagar);
                                                    tbCambio.Text = "$0";
                                                    tbRecibido.Text = "0";

                                                    Cargando(false);
                                                    panelPagar.Enabled = true;
                                                    tmrTimeOutPago.Start();

                                                    chbEstacionamiento.Checked = false;
                                                    chbMensualidad.Checked = true;

                                                    lblTiempoFuera.Text = "Usted dispone de 40 segundos para pagar.";
                                                }
                                                else
                                                {
                                                    DialogResult result5 = MessageBox.Show("¿El valor a pagar es = $0, desea renovar la mensualidad?", "Renovar Mensualidad", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                                                    if (result5 == DialogResult.Yes)
                                                    {
                                                        ActualizaVigenciaAutorizadoResponse resp = cliente.ActualizarVigenciaAutorizado(tbIdTarjeta.Text);

                                                        if (resp.Exito)
                                                        {
                                                            MessageBox.Show("Renovacion exitosa.", "Renovar Mensualidad", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                                        }
                                                        else
                                                        {
                                                            this.DialogResult = DialogResult.None;
                                                            MessageBox.Show(resp.ErrorMessage, "Error Renovar Mensualidad", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        Cargando(false);
                                                    }
                                                    Cargando(false);
                                                }
                                            }
                                            else
                                            {
                                                Cargando(false);
                                                MessageBox.Show("No obtiene valor a pagar mensualidad", "Error Leer PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            }
                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        Cargando(false);
                                        MessageBox.Show("No obtiene infromacion de la transaccion", "Error Leer PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }
                                else
                                {
                                    Cargando(false);
                                    MessageBox.Show(oInfoTransaccionService.ErrorMessage + ": " + sIdTransaccion, "Error Leer PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            else
                            {
                                Cargando(false);
                                MessageBox.Show("No obtiene carril apartir del modulo", "Error Leer PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            Cargando(false);
                            //Camino de salida autorizado
                        }
                    }
                    #endregion

                    #region Mensualidad
                    else
                    {
                        oResultadoOperacion1 = oLectora.ObtenerID();

                        if (oResultadoOperacion1.oEstado == TipoRespuesta.Exito)
                        {
                            tbIdTarjeta.Text = oResultadoOperacion1.EntidadDatos.ToString();
                        }
                        tbIdTarjeta.Text = tbIdTarjeta.Text;
                        //liquidacion = cliente.ConsultarValorPagar(true, oCardResponse.reposicion, 1, "0", "0EEEC6CB");
                        liquidacion = cliente.ConsultarValorPagar(true, Convert.ToBoolean(oTarjeta.Replacement), 1, tbIdTarjeta.Text, tbIdTarjeta.Text);
                        double sumTotalPagar = 0;
                        if (liquidacion.Exito)
                        {
                            foreach (DatosLiquidacionService item in liquidacion.LstLiquidacion)
                            {
                                sumTotalPagar += item.Total;
                            }

                            if (sumTotalPagar > 0)
                            {
                                tbValorPagar.Text = "$" + string.Format("{0:#,##0.##}", sumTotalPagar);
                                tbCambio.Text = "$0";
                                tbRecibido.Text = "0";

                                Cargando(false);
                                panelPagar.Enabled = true;
                                tmrTimeOutPago.Start();

                                chbEstacionamiento.Checked = false;
                                chbMensualidad.Checked = true;

                                lblTiempoFuera.Text = "Usted dispone de 40 segundos para pagar.";
                            }
                            else
                            {
                                DialogResult result5 = MessageBox.Show("¿El valor a pagar es = $0, desea renovar la mensualidad?", "Renovar Mensualidad", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                                if (result5 == DialogResult.Yes)
                                {
                                    ActualizaVigenciaAutorizadoResponse resp = cliente.ActualizarVigenciaAutorizado(tbIdTarjeta.Text);

                                    if (resp.Exito)
                                    {
                                        MessageBox.Show("Renovacion exitosa.", "Renovar Mensualidad", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                    else
                                    {
                                        this.DialogResult = DialogResult.None;
                                        MessageBox.Show(resp.ErrorMessage, "Error Renovar Mensualidad", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                    Cargando(false);
                                }
                                else
                                {
                                    Cargando(false);
                                }
                            }
                        }
                        else
                        {
                            Cargando(false);
                            MessageBox.Show("No obtiene valor a pagar mensualidad", "Error Leer PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    #endregion

                }
                else
                {
                    if (ckMensualidadDocumento.Checked == true)
                    {
                        AutorizadoxPlacaResponse IdTarjeta = cliente.BuscarAutorizadoxPlaca(txtPlaca.Text);
                        if (IdTarjeta.Exito)
                        {
                            tbIdTarjeta.Text = IdTarjeta.IdTarjeta;

                            liquidacion = cliente.ConsultarValorPagar(true, Convert.ToBoolean(oTarjeta.Replacement), 1, tbIdTarjeta.Text, tbIdTarjeta.Text);
                            double sumTotalPagar = 0;
                            if (liquidacion.Exito)
                            {
                                foreach (DatosLiquidacionService item in liquidacion.LstLiquidacion)
                                {
                                    sumTotalPagar += item.Total;
                                }

                                if (sumTotalPagar > 0)
                                {
                                    tbValorPagar.Text = "$" + string.Format("{0:#,##0.##}", sumTotalPagar);
                                    tbCambio.Text = "$0";
                                    tbRecibido.Text = "0";

                                    Cargando(false);
                                    panelPagar.Enabled = true;
                                    tmrTimeOutPago.Start();

                                    chbEstacionamiento.Checked = false;
                                    chbMensualidad.Checked = true;

                                    lblTiempoFuera.Text = "Usted dispone de 40 segundos para pagar.";
                                }
                                else
                                {
                                    //DialogResult result5 = MessageBox.Show("¿El valor a pagar es = $0, desea renovar la mensualidad?", "Renovar Mensualidad", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                                    //if (result5 == DialogResult.Yes)
                                    //{
                                    //    ActualizaVigenciaAutorizadoResponse resp = cliente.ActualizarVigenciaAutorizado(oCardResponse.idCard);

                                    //    if (resp.Exito)
                                    //    {
                                    //        MessageBox.Show("Renovacion exitosa.", "Renovar Mensualidad", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    //    }
                                    //    else
                                    //    {
                                    //        this.DialogResult = DialogResult.None;
                                    //        MessageBox.Show(resp.ErrorMessage, "Error Renovar Mensualidad", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    //    }
                                    //    Cargando(false);
                                    //}
                                    //else
                                    //{
                                    //    Cargando(false);
                                    //}

                                    Cargando(false);
                                    MessageBox.Show("Consulte vencimiento de mensualidad", "Error Leer PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }

                            else
                            {
                                Cargando(false);
                                MessageBox.Show("No fue posible obtener el valor de la mensualidad", "Error Leer PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            Cargando(false);
                            MessageBox.Show("La placa no pertenece a un autorizado", "Error Leer PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else if (ckMensualidadDocumento.Checked == false)
                    {
                        tbIdTarjeta.Text = tbIdTarjeta.Text;
                        //liquidacion = cliente.ConsultarValorPagar(true, oCardResponse.reposicion, 1, "0", "0EEEC6CB");
                        liquidacion = cliente.ConsultarValorPagar(true, Convert.ToBoolean(oTarjeta.Replacement), 1, tbIdTarjeta.Text, tbIdTarjeta.Text);
                        double sumTotalPagar = 0;
                        if (liquidacion.Exito)
                        {
                            foreach (DatosLiquidacionService item in liquidacion.LstLiquidacion)
                            {
                                sumTotalPagar += item.Total;
                            }

                            if (sumTotalPagar > 0)
                            {
                                tbValorPagar.Text = "$" + string.Format("{0:#,##0.##}", sumTotalPagar);
                                tbCambio.Text = "$0";
                                tbRecibido.Text = "0";

                                Cargando(false);
                                panelPagar.Enabled = true;
                                tmrTimeOutPago.Start();

                                chbEstacionamiento.Checked = false;
                                chbMensualidad.Checked = true;

                                lblTiempoFuera.Text = "Usted dispone de 40 segundos para pagar.";
                            }
                            else
                            {
                                DialogResult result5 = MessageBox.Show("¿El valor a pagar es = $0, desea renovar la mensualidad?", "Renovar Mensualidad", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                                if (result5 == DialogResult.Yes)
                                {
                                    ActualizaVigenciaAutorizadoResponse resp = cliente.ActualizarVigenciaAutorizado(tbIdTarjeta.Text);

                                    if (resp.Exito)
                                    {
                                        MessageBox.Show("Renovacion exitosa.", "Renovar Mensualidad", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                    else
                                    {
                                        this.DialogResult = DialogResult.None;
                                        MessageBox.Show(resp.ErrorMessage, "Error Renovar Mensualidad", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                    Cargando(false);
                                }
                                else
                                {
                                    Cargando(false);
                                }
                            }
                        }
                        else
                        {
                            Cargando(false);
                            MessageBox.Show("No obtiene valor a pagar mensualidad", "Error Leer PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        DialogResult result3 = MessageBox.Show("No fue posible leer la tarjeta, ¿Desea registrar una salida SIN tarjeta?", "Leer PPM", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                        if (result3 == DialogResult.Yes)
                        {
                            CrearSalida popup = new CrearSalida(cbEstacionamiento.SelectedValue.ToString());
                            popup.ShowDialog();
                            if (popup.DialogResult == DialogResult.OK)
                            {
                                Cargando(false);
                                MessageBox.Show("Salida creada con EXITO", "Crear Salida PPM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else if (popup.DialogResult == System.Windows.Forms.DialogResult.Cancel)
                            {
                                Cargando(false);
                                MessageBox.Show("Operacion cancelada por el usuario", "Crear Salida PPM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                Cargando(false);
                                MessageBox.Show("Error al procesar ventana crear salida", "Crear Salida PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            Cargando(false);
                            //MessageBox.Show(oCardResponse.errorMessage, "Error Leer PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }

            else
            {
                Cargando(false);
                MessageBox.Show("No se encontro parametro claveTarjeta para el estacionamiento = " + cbEstacionamiento.SelectedValue.ToString(), "Error Leer PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btn_Moto_Click(object sender, EventArgs e)
        {
             DialogResult oDialogResult = MessageBox.Show("¿Esta seguro que desea cambiar a MOTO?", "ETIQUETA MOTO", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
             if (oDialogResult == DialogResult.Yes)
             {
                 Cargando(true);
                 string clave = cliente.ObtenerValorParametroxNombre("claveTarjeta", cbEstacionamiento.SelectedValue.ToString());
                 if (clave != string.Empty)
                 {
                     CardResponse oCardResponse = GetCardInfo(clave);
                     if (!oCardResponse.error)
                     {
                         AplicarMotoResponse oAplicarMotoResponse = cliente.AplicarEtiquetaMoto(cbEstacionamiento.SelectedValue.ToString(), oCardResponse.idCard, oCardResponse.moduloEntrada);
                         if (oAplicarMotoResponse.Exito)
                         {
                             MessageBox.Show("Etiqueta moto aplicada exitosamente a = " + cbEstacionamiento.SelectedValue.ToString() + "/" + oCardResponse.idCard + "/" + oCardResponse.moduloEntrada, "Aplicar Moto PPM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                             Cargando(false);
                         }
                         else
                         {
                             Cargando(false);
                             MessageBox.Show(oAplicarMotoResponse.ErrorMessage, "Error Aplicar Moto PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                         }
                     }
                     else
                     {
                         Cargando(false);
                         MessageBox.Show(oCardResponse.errorMessage, "Error Aplicar Moto PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                     }
                 }
                 else
                 {
                     Cargando(false);
                     MessageBox.Show("No se encontro parametro claveTarjeta para el estacionamiento = " + cbEstacionamiento.SelectedValue.ToString(), "Error Aplicar Moto PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                 }
             }
        }
        private void btn_Convenio_Click(object sender, EventArgs e)
        {
            ConvenioPopUp popup = new ConvenioPopUp(cbEstacionamiento.SelectedValue.ToString(), _DocumentoUsuario);
            popup.ShowDialog();
            if (popup.DialogResult == DialogResult.OK)
            {
                Cargando(true);
                string clave = cliente.ObtenerValorParametroxNombre("ClaveTarjetasCRT", cbEstacionamiento.SelectedValue.ToString());
                if (clave != string.Empty)
                {
                    CardResponse oCardResponse = AplicarConvenio(clave, popup.Convenio.ToString());
                    if (!oCardResponse.error)
                    {
                        SaveConveniosResponse oInfo = new SaveConveniosResponse();
                        oInfo = cliente.SaveConvenio(cbEstacionamiento.SelectedValue.ToString(), Convert.ToInt64(popup.Convenio), popup.NameConvenio.ToString());

                        MessageBox.Show("Convenio aplicado exitosamente", "Aplicar Convenio PPM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Cargando(false);
                    }
                    else
                    {
                        Cargando(false);
                        MessageBox.Show(oCardResponse.errorMessage, "Error Aplicar Convenio PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    Cargando(false);
                    MessageBox.Show("No se encontro parametro claveTarjeta para el estacionamiento = " + cbEstacionamiento.SelectedValue.ToString(), "Error Aplicar Convenio PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (popup.DialogResult == System.Windows.Forms.DialogResult.Cancel)
            {
                Cargando(false);
                MessageBox.Show("Operacion cancelada por el usuario", "Aplicar Convenio PPM", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                Cargando(false);
                MessageBox.Show("Error al procesar ventana convenio", "Aplicar Convenio PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btn_Cortesia_Click(object sender, EventArgs e)
        {
            Tarjeta oTarjeta = new Tarjeta();
            CortesiaPopUp popup = new CortesiaPopUp(cbEstacionamiento.SelectedValue.ToString());
            popup.ShowDialog();
            if (popup.DialogResult == DialogResult.OK)
            {
                Cargando(true);
                string clave = cliente.ObtenerValorParametroxNombre("claveTarjetasCRT", cbEstacionamiento.SelectedValue.ToString());
                if (clave != string.Empty)
                {
                    ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();

                    oResultadoOperacion = oLectora.LeerTI(clave);
                    oTarjeta = (Tarjeta)oResultadoOperacion.EntidadDatos;

                    if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
                    {

                        oResultadoOperacion = oLectora.EscribirCortesia(clave);

                        if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
                        {
                            CarrilxIdModuloResponse oCarrilxIdModuloResponse = cliente.ObtenerCarrilxIdModulo(cbEstacionamiento.SelectedValue.ToString(), oTarjeta.EntranceModule);
                            if (oCarrilxIdModuloResponse.Exito)
                            {
                                //yyyyMMddHHmmssce

                                string sIdTransaccion = Convert.ToDateTime(oTarjeta.DateTimeEntrance).ToString("yyyyMMddHHmmss") + oCarrilxIdModuloResponse.Carril + cbEstacionamiento.SelectedValue.ToString();
                                oResultadoOperacion = oLectora.ObtenerID();
                                tbIdTarjeta.Text = oResultadoOperacion.EntidadDatos.ToString();
                                InfoTransaccionService oInfoTransaccionService = cliente.ConsultarInfoTransaccionxId(sIdTransaccion);
                                //InfoTransaccionService oInfoTransaccionService = cliente.ConsultarInfoTransaccion(cbEstacionamiento.SelectedValue.ToString(), oCardResponse.idCard, oCardResponse.moduloEntrada);
                                if (oInfoTransaccionService.Exito)
                                {
                                    AplicarCortesiaResponse oAplicarCortesiaResponse = cliente.AplicarLaCortesia(cbEstacionamiento.SelectedValue.ToString(), popup.Observacion, popup.Motivo.ToString(), oInfoTransaccionService.IdTransaccion, _DocumentoUsuario);
                                    if (oAplicarCortesiaResponse.Exito)
                                    {
                                        oResultadoOperacion = oLectora.EscribirCortesia(clave);
                                        AplicaCascoResponse oInfo = cliente.LiberarCasco(sIdTransaccion);
                                        //if (oInfo.Exito)
                                        //{
                                        if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
                                        {

                                            MessageBox.Show("Cortesia aplicada exitosamente", "Aplicar Cortesia PPM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            Cargando(false);

                                        }
                                        else
                                        {
                                            Cargando(false);
                                            MessageBox.Show("No se pudo aplicar la cortesia", "Error Aplicar Cortesia PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        }
                                        //}
                                        //else 
                                        //{
                                        //    Cargando(false);
                                        //    MessageBox.Show(oCardResponse.errorMessage, "Error al liberar casillero PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        //}
                                    }
                                    else
                                    {
                                        Cargando(false);
                                        MessageBox.Show(oAplicarCortesiaResponse.ErrorMessage, "Error Aplicar Cortesia PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }
                                else
                                {
                                    Cargando(false);
                                    MessageBox.Show(oInfoTransaccionService.ErrorMessage, "Error Aplicar Cortesia PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            else
                            {
                                Cargando(false);
                                MessageBox.Show("No obtiene carril apartir del modulo", "Error Leer PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            Cargando(false);
                            MessageBox.Show("No se pudo aplicar la cortesia", "Error Aplicar Cortesia PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        Cargando(false);
                        MessageBox.Show("Error al leer la tarjeta", "Error Aplicar Cortesia PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    Cargando(false);
                    MessageBox.Show("No se encontro parametro claveTarjeta para el estacionamiento = " + cbEstacionamiento.SelectedValue.ToString(), "Error Aplicar Cortesia PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                    
            }
            else if (popup.DialogResult == System.Windows.Forms.DialogResult.Cancel)
            {
                Cargando(false);
                MessageBox.Show("Operacion cancelada por el usuario", "Aplicar Cortesia PPM", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                Cargando(false);
                MessageBox.Show("Error al procesar ventana cortesia", "Aplicar Cortesia PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btn_Carga_Click(object sender, EventArgs e)
        {
            RegistrarCargaResponse oRegistro = cliente.RegistrarLaCarga(cbEstacionamiento.SelectedValue.ToString(), cbPPM.SelectedValue.ToString(), _DocumentoUsuario);
            if (oRegistro.Exito)
            {
                CargaPopUp popup = new CargaPopUp(oRegistro.IdCarga.ToString());
                popup.ShowDialog();
                if (popup.DialogResult == DialogResult.OK)
                {
                    ConfirmarCargaResponse confCarga = cliente.ConfirmarLaCarga(cbEstacionamiento.SelectedValue.ToString(), cbPPM.SelectedValue.ToString(), oRegistro.IdCarga.ToString(), popup.Valor.ToString());
                    if (confCarga.Exito)
                    {
                        ImprimirCarga(oRegistro.IdCarga.ToString());
                    }
                    else
                    {
                        MessageBox.Show(confCarga.ErrorMessage, "Error Carga PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else if (popup.DialogResult == System.Windows.Forms.DialogResult.Cancel)
                {
                    MessageBox.Show("Operacion cancelada por el usuario", "Carga PPM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Error al procesar ventana carga", "Carga PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show(oRegistro.ErrorMessage, "Error Carga PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btn_Arqueo_Click(object sender, EventArgs e)
        {
             DialogResult oDialogResult = MessageBox.Show("¿Esta seguro que desea realizar el arqueo?", "Arqueo PPM", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
             if (oDialogResult == DialogResult.Yes)
             {
                 RegistrarArqueoResponse rgis = cliente.RegistrarElArqueo(cbEstacionamiento.SelectedValue.ToString(), cbPPM.SelectedValue.ToString(), _DocumentoUsuario);
                 if (rgis.Exito)
                 {
                     ArqueoPopUp popup = new ArqueoPopUp(rgis.IdArqueo.ToString());
                     popup.ShowDialog();
                     if (popup.DialogResult == DialogResult.OK)
                     {
                         ConfirmarArqueoResponse confirmacionArqueo = cliente.ConfirmarElArqueo(cbEstacionamiento.SelectedValue.ToString(), cbPPM.SelectedValue.ToString(), rgis.IdArqueo.ToString(), popup.Valor.ToString());
                         if (confirmacionArqueo.Exito)
                         {
                             ImprimirArqueo(rgis.IdArqueo.ToString());
                         }
                         else
                         {
                             MessageBox.Show(confirmacionArqueo.ErrorMessage, "Arqueo PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);

                             StringBuilder sb = new StringBuilder();
                             sb.Append(confirmacionArqueo.ErrorMessage);
                             File.AppendAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"log.txt"), sb.ToString());
                             sb.Clear();
                         }
                     }
                     else if (popup.DialogResult == System.Windows.Forms.DialogResult.Cancel)
                     {
                         MessageBox.Show("Operacion cancelada por el usuario", "Arqueo PPM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                     }
                     else
                     {
                         MessageBox.Show("Error al procesar ventana carga", "Arqueo PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                     }
                 }
                 else
                 {
                     MessageBox.Show(rgis.ErrorMessage, "Arqueo PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                 }
             }
        }
        private void btn_Eventos_Click(object sender, EventArgs e)
        {
            EventoPopUp popup = new EventoPopUp(cbEstacionamiento.SelectedValue.ToString(), _DocumentoUsuario);
            popup.ShowDialog();
            if (popup.DialogResult == DialogResult.OK)
            {
                Cargando(true);
                string clave = cliente.ObtenerValorParametroxNombre("claveTarjeta", cbEstacionamiento.SelectedValue.ToString());
                if (clave != string.Empty)
                {
                    CardResponse oCardResponse = GetCardInfo(clave);
                    if (!oCardResponse.error)
                    {
                        CarrilxIdModuloResponse oCarrilxIdModuloResponse = cliente.ObtenerCarrilxIdModulo(cbEstacionamiento.SelectedValue.ToString(), oCardResponse.moduloEntrada);
                        if (oCarrilxIdModuloResponse.Exito)
                        {
                            //yyyyMMddHHmmssce

                            string sIdTransaccion = Convert.ToDateTime(oCardResponse.fechEntrada).ToString("yyyyMMddHHmmss") + oCarrilxIdModuloResponse.Carril + cbEstacionamiento.SelectedValue.ToString();

                            InfoTransaccionService oInfoTransaccionService = cliente.ConsultarInfoTransaccionxId(sIdTransaccion);

                            if (oInfoTransaccionService.Exito)
                            {
                                AplicarEventoResponse oAplicarEventoResponse = cliente.AplicarElEvento(cbEstacionamiento.SelectedValue.ToString(), sIdTransaccion, _DocumentoUsuario, oCardResponse.idCard, popup.Evento.ToString());
                                if (oAplicarEventoResponse.Exito)
                                {
                                    MessageBox.Show("Evento aplicado exitosamente", "Aplicar Evento PPM", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    Cargando(false);
                                }
                                else
                                {
                                    Cargando(false);
                                    MessageBox.Show(oAplicarEventoResponse.ErrorMessage, "Error Aplicar Evento PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                            else
                            {
                                Cargando(false);
                                MessageBox.Show(oInfoTransaccionService.ErrorMessage, "Error Aplicar Evento PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            Cargando(false);
                            MessageBox.Show("No obtiene carril apartir del modulo", "Error Leer PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        Cargando(false);
                        MessageBox.Show(oCardResponse.errorMessage, "Error Aplicar Evento PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    Cargando(false);
                    MessageBox.Show("No se encontro parametro claveTarjeta para el estacionamiento = " + cbEstacionamiento.SelectedValue.ToString(), "Error Aplicar Evento PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (popup.DialogResult == System.Windows.Forms.DialogResult.Cancel)
            {
                Cargando(false);
                MessageBox.Show("Operacion cancelada por el usuario", "Aplicar Evento PPM", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                Cargando(false);
                MessageBox.Show("Error al procesar ventana convenio", "Aplicar Evento PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btn_Entrada_Click(object sender, EventArgs e)
        {
            Cargando(true);
            CrearIngreso popup = new CrearIngreso(cbSede.SelectedValue.ToString(), cbEstacionamiento.SelectedValue.ToString());
            popup.ShowDialog();
            if (popup.DialogResult == DialogResult.OK)
            {
                Cargando(false);
                MessageBox.Show("Entrada creada con EXITO", "Crear Entrada PPM", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (popup.DialogResult == System.Windows.Forms.DialogResult.Cancel)
            {
                Cargando(false);
                MessageBox.Show("Operacion cancelada por el usuario", "Crear Entrada PPM", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                Cargando(false);
                MessageBox.Show("Error al procesar ventana crear entrada", "Crear Entrada PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btn_Casco_Click(object sender, EventArgs e)
        {

            //CODIGO FUNCIONAL 
            Cargando(true);
            CasilleroCasco popup = new CasilleroCasco(cbEstacionamiento.SelectedValue.ToString());
            popup.ShowDialog();
            if (popup.DialogResult == DialogResult.OK)
            {
                Cargando(false);
                MessageBox.Show("Tarifa casco creada con EXITO", "Crear tarifa casco PPM", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else if (popup.DialogResult == System.Windows.Forms.DialogResult.Cancel)
            {
                Cargando(false);
                MessageBox.Show("Operacion cancelada por el usuario", "Crear tarifa casco PPM", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                Cargando(false);
                MessageBox.Show("Error al procesar ventana crear tarifa", "Crear tarifa casco PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


            //Cargando(true);
            //string clave = cliente.ObtenerValorParametroxNombre("claveTarjeta", cbEstacionamiento.SelectedValue.ToString());
            //if (clave != string.Empty)
            //{
            //    CardResponse oCardResponse = GetCardInfo(clave);
            //    if (!oCardResponse.error)
            //    {
            //        if (oCardResponse.tipoVehiculo == "MOTORCYCLE")
            //        {
            //            CasilleroCasco popup = new CasilleroCasco(cbEstacionamiento.SelectedValue.ToString());
            //            popup.ShowDialog();
            //            if (popup.DialogResult == DialogResult.OK)
            //            {
            //                Cargando(false);
            //                MessageBox.Show("Tarifa casco creada con EXITO", "Crear tarifa casco PPM", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //            }
            //            else if (popup.DialogResult == System.Windows.Forms.DialogResult.Cancel)
            //            {
            //                Cargando(false);
            //                MessageBox.Show("Operacion cancelada por el usuario", "Crear tarifa casco PPM", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //            }
            //            else
            //            {
            //                Cargando(false);
            //                MessageBox.Show("Error al procesar ventana crear tarifa", "Crear tarifa casco PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //            }
            //        }
            //        else {
            //            Cargando(false);
            //            MessageBox.Show("NO PUEDE APLICAR CASCOS A UN VEHICULO", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        }
            //    }
            //    else
            //    {
            //        Cargando(false);
            //        MessageBox.Show(oCardResponse.errorMessage, "Error Aplicar Moto PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    }
            //}
            //else
            //{
            //    Cargando(false);
            //    MessageBox.Show("No se encontro parametro claveTarjeta para el estacionamiento = " + cbEstacionamiento.SelectedValue.ToString(), "Error Aplicar Moto PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}

                      
        }
        private void btn_Copia_Click(object sender, EventArgs e)
        {
            Cargando(true);
            CopiaFactura popup = new CopiaFactura();
            popup.ShowDialog();
            if (popup.DialogResult == DialogResult.OK)
            {
                Cargando(false);
            }
            else if (popup.DialogResult == System.Windows.Forms.DialogResult.Cancel)
            {
                Cargando(false);
                MessageBox.Show("Operacion cancelada por el usuario", "Copia de factura PPM", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                Cargando(false);
                MessageBox.Show("Error al procesar ventana copia de factura", "Copia de factura PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }
        #endregion

        #region Formulario
        public PPM(string sDocumento, InfoPPMService oInfoPPMService)
        {
            //ImprimirPagoNormal("2017051813485412");
            

            _DocumentoUsuario = sDocumento;
            Application.AddMessageFilter(this);

            controlsToMove.Add(this);
            //controlsToMove.Add(this);//Add whatever controls here you want to move the form when it is clicked and dragged

            InitializeComponent();
            tmrTimeOutPago.Interval = 1000;
            tmrTimeOutPago.Tick += tmrTimeOutPago_Tick;
            //tmrTimeOutPago.Start();
            tmrHora.Interval = 1000;
            tmrHora.Tick += tmrHora_Tick;
            tmrHora.Start();
            panelPagar.Enabled = false;
            panelTodo.Enabled = false;
            lblTiempoFuera.Text = string.Empty;

            Cargando(true);

            CargarUsuario();

            
            var dataSource = new List<Object>();
            dataSource.Add(new { Name = oInfoPPMService.Sede, Value = oInfoPPMService.IdSede });

            //Setup data binding
            this.cbSede.DataSource = dataSource;
            this.cbSede.DisplayMember = "Name";
            this.cbSede.ValueMember = "Value";

            var dataSource2 = new List<Object>();
            dataSource2.Add(new { Name = oInfoPPMService.Estacionamiento, Value = oInfoPPMService.IdEstacionamiento });

            //Setup data binding
            this.cbEstacionamiento.DataSource = dataSource2;
            this.cbEstacionamiento.DisplayMember = "Name";
            this.cbEstacionamiento.ValueMember = "Value";

            var dataSource3 = new List<Object>();
            dataSource3.Add(new { Name = oInfoPPMService.Modulo, Value = oInfoPPMService.Modulo });

            //Setup data binding
            this.cbPPM.DataSource = dataSource3;
            this.cbPPM.DisplayMember = "Name";
            this.cbPPM.ValueMember = "Value";
            Cargando(false);
            

        }
        private void Cargando(bool bCarga)
        {
            panelTodo.Enabled = !bCarga;
            progressBar1.Visible = bCarga;
        }
        private void RestablecerPPM()
        {
            tbHoraIngreso.Text = string.Empty;
            liquidacion = new LiquidacionService();
            tmrTimeOutPago.Stop();
            panelPagar.Enabled = false;
            chbEstacionamiento.Checked = false;
            chbMensualidad.Checked = false;
            tbCambio.Text = "$0";
            tbIdTarjeta.Text = string.Empty;
            tbIdTransaccion.Text = string.Empty;
            tbPlaca.Text = string.Empty;
            tbRecibido.Text = "0";
            tbValorPagar.Text = string.Empty;
            lblTiempoFuera.Text = string.Empty;
            tbTiempo.Text = string.Empty;
            tbHoraPago.Text = string.Empty;
            tbCasillero.Text = string.Empty;
            textBox1.Text = string.Empty;

            NombreConvenio = string.Empty;
           _FechaEntrda = string.Empty;
        }
        private void CargarUsuario()
        {
            InfoUsuarioResponse response = cliente.ObtenerInformacionUsuario(_DocumentoUsuario);
            if (response.Exito)
            {
                tbUsuario.Text = response.Usuario;
            }
            else
            {
                tbUsuario.Text = "Usuario Desconocido";
            }
        }
        #endregion

        #region Tarjetas
        public CardResponse AplicarCortesia(string password)
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
                                myTarjeta.Courtesy = true;
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
        public CardResponse PayCard(string password, string idTarjeta, string moduloPago, DateTime dtFechaPago)
        {
            CardResponse oCardResponse = new CardResponse();
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();
            //Lectora_ACR122U lectora = new Lectora_ACR122U();

            //Rspsta_Conexion_LECTOR res = lectora.Conectar(false);
            //if (res.Conectado)
            //{
            //Thread.Sleep(200);
            //Rspsta_DetectarTarjeta_LECTOR re = lectora.DetectarTarjeta();
            //if (re.TarjetaDetectada)
            //{
            //Thread.Sleep(200);
            //Rspsta_CodigoTarjeta_LECTOR resp = lectora.ObtenerIDTarjeta();

            oResultadoOperacion = oLectora.ObtenerID();

            //if (resp.CodigoTarjeta != null && resp.CodigoTarjeta != string.Empty)
            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                //Thread.Sleep(200);
                //Rspsta_EstablecerClave_LECTOR resp1 = lectora.EstablecerClaveLECTOR(password);
                //if (resp1.ClaveEstablecida)
                //{
                //Thread.Sleep(200);
                //Rspsta_Leer_Tarjeta_LECTOR resp2 = lectora.LeerTarjeta(TYPE_STRUCTURE_SMARTCARD.SMARTCARD_PARKING_V1, false);
                //oResultadoOperacion = oLectora.LeerTI();
                //    //if (resp2.TarjetaLeida)
                //if(oResultadoOperacion.oEstado == TipoRespuesta.Exito)
                //{
                //Thread.Sleep(2000);
                //SMARTCARD_PARKING_V1 myTarjeta = (SMARTCARD_PARKING_V1)resp2.Tarjeta;
                //myTarjeta.PaymentModule = moduloPago;
                //myTarjeta.PaymentDateTime = dtFechaPago;
                //myTarjeta.CodeCard = idTarjeta;
                //myTarjeta.Replacement = false;
                //Rspsta_Escribir_Tarjeta_LECTOR resp4 = lectora.EscribirTarjeta(myTarjeta, false, false);
                //if (resp4.TarjetaEscrita)
                //{
                //Thread.Sleep(2000);
                //Rspsta_Leer_Tarjeta_LECTOR respFinal = lectora.LeerTarjeta(TYPE_STRUCTURE_SMARTCARD.SMARTCARD_PARKING_V1, false);
                string clave = cliente.ObtenerValorParametroxNombre("ClaveTarjetasCRT", cbEstacionamiento.SelectedValue.ToString());
                oResultadoOperacion = oLectora.LeerTI(clave);
                //if (respFinal.TarjetaLeida)
                if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
                {
                    oCardResponse.error = false;
                    //SMARTCARD_PARKING_V1 myTarjetaFinal = (SMARTCARD_PARKING_V1)respFinal.Tarjeta;
                    //oCardResponse.cicloActivo = myTarjetaFinal.ActiveCycle != null ? (bool)myTarjetaFinal.ActiveCycle : false;
                    //oCardResponse.codeAutorizacion1 = myTarjetaFinal.CodeAgreement1 != null ? (int)myTarjetaFinal.CodeAgreement1 : 0;
                    //oCardResponse.codeAutorizacion2 = myTarjetaFinal.CodeAgreement2 != null ? (int)myTarjetaFinal.CodeAgreement2 : 0;
                    //oCardResponse.codeAutorizacion3 = myTarjetaFinal.CodeAgreement3 != null ? (int)myTarjetaFinal.CodeAgreement3 : 0;
                    //oCardResponse.cortesia = myTarjetaFinal.Courtesy != null ? (bool)myTarjetaFinal.Courtesy : false;
                    //oCardResponse.fechEntrada = myTarjetaFinal.DateTimeEntrance;
                    //oCardResponse.moduloEntrada = myTarjetaFinal.EntranceModule;
                    //oCardResponse.placa = myTarjetaFinal.EntrancePlate;
                    //oCardResponse.fechaPago = myTarjetaFinal.PaymentDateTime.ToString();
                    //oCardResponse.moduloPago = myTarjetaFinal.PaymentModule;
                    //oCardResponse.reposicion = myTarjetaFinal.Replacement != null ? (bool)myTarjetaFinal.Replacement : false;
                    //oCardResponse.tipoTarjeta = myTarjetaFinal.TypeCard.ToString();
                    //oCardResponse.tipoVehiculo = myTarjetaFinal.TypeVehicle.ToString();
                    //oCardResponse.valet = myTarjetaFinal.ValetParking != null ? (bool)myTarjetaFinal.ValetParking : false;

                    Tarjeta myTarjetaFinal = new Tarjeta();
                    myTarjetaFinal = (Tarjeta)oResultadoOperacion.EntidadDatos;

                    oCardResponse.cicloActivo = myTarjetaFinal.ActiveCycle != null ? (bool)myTarjetaFinal.ActiveCycle : false;
                    oCardResponse.codeAutorizacion1 = myTarjetaFinal.CodeAgreement1 != null ? (int)myTarjetaFinal.CodeAgreement1 : 0;
                    oCardResponse.codeAutorizacion2 = myTarjetaFinal.CodeAgreement2 != null ? (int)myTarjetaFinal.CodeAgreement2 : 0;
                    oCardResponse.codeAutorizacion3 = myTarjetaFinal.CodeAgreement3 != null ? (int)myTarjetaFinal.CodeAgreement3 : 0;
                    oCardResponse.cortesia = myTarjetaFinal.Courtesy != null ? (bool)myTarjetaFinal.Courtesy : false;
                    oCardResponse.fechEntrada = myTarjetaFinal.DateTimeEntrance;
                    oCardResponse.moduloEntrada = myTarjetaFinal.EntranceModule;
                    oCardResponse.placa = myTarjetaFinal.EntrancePlate;
                    //oCardResponse.fechaPago = myTarjetaFinal.PaymentDateTime.ToString();
                    oCardResponse.fechaPago = DateTime.Now.ToString("yyyy/dd/MM HH:mm:ss");
                    oCardResponse.moduloPago = myTarjetaFinal.PaymentModule;
                    oCardResponse.reposicion = myTarjetaFinal.Replacement != null ? (bool)myTarjetaFinal.Replacement : false;
                    oCardResponse.tipoTarjeta = myTarjetaFinal.TypeCard.ToString();
                    oCardResponse.tipoVehiculo = myTarjetaFinal.TypeVehicle.ToString();
                    oCardResponse.valet = myTarjetaFinal.ValetParking != null ? (bool)myTarjetaFinal.ValetParking : false;
                }
                else
                {
                    oCardResponse.error = true;
                    oCardResponse.errorMessage = "No lee tarjeta.";
                }
                //}
                //else
                //{
                //    oCardResponse.error = true;
                //    oCardResponse.errorMessage = "No escribe tarjeta.";
                //}
                //}
                //else
                //{
                //    oCardResponse.error = true;
                //    oCardResponse.errorMessage = "No lee tarjeta.";
                //}
                //}
                //else
                //{
                //    oCardResponse.error = true;
                //    oCardResponse.errorMessage = "No establece clave tarjeta.";
                //}
            }
            else
            {
                oCardResponse.error = true;
                oCardResponse.errorMessage = "No obtiene id tarjeta.";
            }
            //}
            //else
            //{
            //    oCardResponse.error = true;
            //    oCardResponse.errorMessage = "No detecta tarjeta.";
            //}
            //}
            //else
            //{
            //    oCardResponse.error = true;
            //    oCardResponse.errorMessage = "No conecta con lectora de tarjetas.";
            //}


            return oCardResponse;
        }
        public CardResponse ExitCardAutho(string password, string idTarjeta)
        {
            CardResponse oCardResponse = new CardResponse();
            Lectora_ACR122U lectora = new Lectora_ACR122U();

            Rspsta_Conexion_LECTOR res = lectora.Conectar(false);
            if (res.Conectado)
            {
                //Thread.Sleep(200);
                Rspsta_DetectarTarjeta_LECTOR re = lectora.DetectarTarjeta();
                if (re.TarjetaDetectada)
                {
                    //Thread.Sleep(200);
                    Rspsta_CodigoTarjeta_LECTOR resp = lectora.ObtenerIDTarjeta();
                    if (resp.CodigoTarjeta != null && resp.CodigoTarjeta != string.Empty)
                    {
                        //Thread.Sleep(200);
                        Rspsta_EstablecerClave_LECTOR resp1 = lectora.EstablecerClaveLECTOR(password);
                        if (resp1.ClaveEstablecida)
                        {
                            //Thread.Sleep(200);
                            Rspsta_Leer_Tarjeta_LECTOR resp2 = lectora.LeerTarjeta(TYPE_STRUCTURE_SMARTCARD.SMARTCARD_PARKING_V1, false);
                            if (resp2.TarjetaLeida)
                            {
                                Thread.Sleep(2000);
                                SMARTCARD_PARKING_V1 myTarjeta = (SMARTCARD_PARKING_V1)resp2.Tarjeta;
                                myTarjeta.Replacement = false;
                                myTarjeta.ActiveCycle = false;
                                myTarjeta.DateTimeEntrance = null;
                                myTarjeta.EntranceModule = string.Empty;
                                myTarjeta.EntrancePlate = string.Empty;
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
        public CardResponse AplicarConvenio(string password, string idConvenio)
        {
            CardResponse oCardResponse = new CardResponse();
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();
            //Lectora_ACR122U lectora = new Lectora_ACR122U();
            //Rspsta_Conexion_LECTOR res = lectora.Conectar(false);
            //if (res.Conectado)
            //{
            //Rspsta_DetectarTarjeta_LECTOR re = lectora.DetectarTarjeta();
            //if (re.TarjetaDetectada)
            //{
            //Rspsta_CodigoTarjeta_LECTOR resp = lectora.ObtenerIDTarjeta();
            //if (resp.CodigoTarjeta != null && resp.CodigoTarjeta != string.Empty)
            //{
            //Rspsta_EstablecerClave_LECTOR resp1 = lectora.EstablecerClaveLECTOR(password);
            //if (resp1.ClaveEstablecida)
            //{
            //Rspsta_Leer_Tarjeta_LECTOR resp2 = lectora.LeerTarjeta(TYPE_STRUCTURE_SMARTCARD.SMARTCARD_PARKING_V1, false);
            //if (resp2.TarjetaLeida)
            //{
            //SMARTCARD_PARKING_V1 myTarjeta = (SMARTCARD_PARKING_V1)resp2.Tarjeta;
            //myTarjeta.CodeAgreement1 = Convert.ToInt32(idConvenio);
            //Rspsta_Escribir_Tarjeta_LECTOR resp4 = lectora.EscribirTarjeta(myTarjeta, false, false);
            oResultadoOperacion = oLectora.EscribirConvenio(Convert.ToInt32(idConvenio));
            //if (resp4.TarjetaEscrita)
            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                oCardResponse.error = false;
            }
            else
            {
                oCardResponse.error = true;
                oCardResponse.errorMessage = "No escribe tarjeta.";
            }
            //}
            //else
            //{
            //    oCardResponse.error = true;
            //    oCardResponse.errorMessage = "No lee tarjeta.";
            //}
            //}
            //else
            //{
            //    oCardResponse.error = true;
            //    oCardResponse.errorMessage = "No establece clave tarjeta.";
            //}
            //}
            //else
            //{
            //    oCardResponse.error = true;
            //    oCardResponse.errorMessage = "No obtiene id tarjeta.";
            //}
            //}
            //else
            //{
            //    oCardResponse.error = true;
            //    oCardResponse.errorMessage = "No detecta tarjeta.";
            //}
            //}
            //else
            //{
            //    oCardResponse.error = true;
            //    oCardResponse.errorMessage = "No conecta con lectora de tarjetas.";
            //}


            return oCardResponse;
        }
        public CardResponse LimpiarReposicion(string password)
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
                                myTarjeta.Replacement = false;
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
        #endregion

        #region Auxiliares
        private static String GetTimestamp(DateTime value)
        {
            return value.ToString("HH:mm:ss");
        }
        private string GetLocalMACAddress()
        {
            return GetMACAddress();
        }
        private string GetMACAddress()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            String sMacAddress = string.Empty;
            foreach (NetworkInterface adapter in nics)
            {
                if (sMacAddress == String.Empty)// only return MAC Address from first card  
                {
                    IPInterfaceProperties properties = adapter.GetIPProperties();
                    sMacAddress = adapter.GetPhysicalAddress().ToString();
                }
            }
            //return "DC4A3E507594";
            return sMacAddress;
        }
        #endregion

        #region Impresion
        private void ImprimirPagoNormal(string idTransaccion)
        {
            InfoFacturaResponse oInfoFacturaResponse = cliente.ObtenerDatosFactura(idTransaccion);
            if (oInfoFacturaResponse.Exito)
            {
                bool resultado = PrintTicket(oInfoFacturaResponse.LstItems.ToList());
                if (!resultado)
                {
                    MessageBox.Show("No fue posible imprimir ticket", "Error Imprimir PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                RestablecerPPM();
                Cargando(false);
            }
            else
            {
                RestablecerPPM();
                Cargando(false);
                MessageBox.Show(oInfoFacturaResponse.ErrorMessage, "Error Imprimir PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private bool ImprimirValidacion() 
        {
            bool bPrint = false;

            try
            {
            ReportDataSource datasource = new ReportDataSource();
            LocalReport oLocalReport = new LocalReport();

            datasource = new ReportDataSource("DataSetValidacion", (DataTable)GenerarTicketValidacion().Tables[0]);
            oLocalReport.ReportPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format(@"Tickets\{0}.rdlc", "TicketValidacion"));


            oLocalReport.DataSources.Add(datasource);
            oLocalReport.Refresh();

            ReportPrintDocument ore = new ReportPrintDocument(oLocalReport);
            ore.PrintController = new StandardPrintController();
            ore.Print();

            oLocalReport.Dispose();
            oLocalReport = null;
            ore.Dispose();
            ore = null;


            bPrint = true;
            }
            catch (Exception e)
            {
                bPrint = false;
            }
            return bPrint;

        }


        public bool PrintTicket(List<InfoItemsFacturaResponse> datos)
        {
            bool bPrint = false;

            try
            {

                List<List<InfoItemsFacturaResponse>> facturas = new List<List<InfoItemsFacturaResponse>>();
                foreach (InfoItemsFacturaResponse item in datos)
                {
                    bool find = false;
                    if (facturas.Count > 0)
                    {
                        foreach (List<InfoItemsFacturaResponse> item2 in facturas)
                        {
                            if (item2[0].NumeroFactura == item.NumeroFactura)
                            {
                                find = true;
                                item2.Add(item);
                            }
                        }

                        if (!find)
                        {
                            List<InfoItemsFacturaResponse> otraFactura = new List<InfoItemsFacturaResponse>();
                            otraFactura.Add(item);
                            facturas.Add(otraFactura);
                        }
                        find = false;
                    }
                    else
                    {
                        List<InfoItemsFacturaResponse> primeraFactura = new List<InfoItemsFacturaResponse>();
                        primeraFactura.Add(item);
                        facturas.Add(primeraFactura);
                    }
                }



                if (facturas.Count > 0)
                {
                    foreach (var item in facturas)
                    {
                        ReportDataSource datasource = new ReportDataSource();
                        LocalReport oLocalReport = new LocalReport();

                        datasource = new ReportDataSource("DataSetTicketPago", (DataTable)GenerarTicketPago(item).Tables[0]);
                        oLocalReport.ReportPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format(@"Tickets\{0}.rdlc", "ticketPago"));


                        oLocalReport.DataSources.Add(datasource);
                        oLocalReport.Refresh();

                        ReportPrintDocument ore = new ReportPrintDocument(oLocalReport);
                        ore.PrintController = new StandardPrintController();
                        ore.Print();

                        oLocalReport.Dispose();
                        oLocalReport = null;
                        ore.Dispose();
                        ore = null;
                    }
                }





                bPrint = true;
            }
            catch (Exception e)
            {
                bPrint = false;
            }
            return bPrint;
        }
        private DataSetTicketPago GenerarTicketPago(List<InfoItemsFacturaResponse> infoTicket)
        {
            DataSetTicketPago facturacion = new DataSetTicketPago();

            double total = 0;
            foreach (var item in infoTicket)
            {
                total += Convert.ToDouble(item.Total);
            }

            foreach (var item in infoTicket)
            {
                DataSetTicketPago.TablaTicketPagoRow rowDatosFactura = facturacion.TablaTicketPago.NewTablaTicketPagoRow();

                rowDatosFactura.Cambio = Convert.ToDouble(item.Cambio);
                rowDatosFactura.Direccion = item.Direccion;
                rowDatosFactura.Fecha = item.Fecha;
                rowDatosFactura.IdTransaccion = item.IdTransaccion;
                rowDatosFactura.Informacion = "Esta infromacion esta quemada en el codigo, deberia obtenerse de algun lugar";
                rowDatosFactura.Modulo = item.Modulo;
                rowDatosFactura.Nombre = item.Nombre;
                rowDatosFactura.NumeroFactura = item.NumeroFactura;
                rowDatosFactura.Placa = item.Placa;
                rowDatosFactura.Recibido = Convert.ToDouble(item.ValorRecibido);
                //string[] data = item.NumeroResolucion.Split('\n');
                //rowDatosFactura.Resolucion = data[0] + " \n" + data[1] + " " + data[2] + " " + data[3];
                rowDatosFactura.Resolucion = item.NumeroResolucion;
                ////
                rowDatosFactura.Rut = "NIT 900.250.796-0";
                rowDatosFactura.Telefono = item.Telefono;
                rowDatosFactura.TotalFinal = total;
                rowDatosFactura.Total = Convert.ToDouble(item.Total);
                rowDatosFactura.Subtotal = Convert.ToDouble(item.Subtotal);
                rowDatosFactura.Iva = Convert.ToDouble(item.Iva);
                rowDatosFactura.TipoPago = item.Tipo;
                rowDatosFactura.Fecha2 = item.FechaEntrada;
                rowDatosFactura.Vehiculo = item.TipoVehiculo;
                rowDatosFactura.VigenciaFactura = item.Vigencia.Trim('\n'); ;

                rowDatosFactura.Convenio = NombreConvenio;

                facturacion.TablaTicketPago.AddTablaTicketPagoRow(rowDatosFactura);
            }

            return facturacion;
        }

        public DataSetValidacion GenerarTicketValidacion()
        {
            DataSetValidacion facturacion = new DataSetValidacion();

            DataSetValidacion.DataTable1Row rowDatosFactura = facturacion.DataTable1.NewDataTable1Row();

            rowDatosFactura.Convenio = NombreConvenio;
          //   rowDatosFactura.FechaEntrada = _FechaEntrda
            rowDatosFactura.FechaEntrada = tbHoraIngreso.Text;
            rowDatosFactura.FechaValidacion = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt");
      //      rowDatosFactura.FechaValidacion = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

            facturacion.DataTable1.AddDataTable1Row(rowDatosFactura);


            return facturacion;
        }

        private void ImprimirPagoMensualidad(string idTransaccion, string idAutorizacion)
        {
            InfoFacturaResponse oInfoFacturaResponse = cliente.ObtenerDatosFacturaMensualidad(idTransaccion, idAutorizacion);
            if (oInfoFacturaResponse.Exito)
            {
                bool resultado = PrintTicketMensualidad(oInfoFacturaResponse.LstItemsMensualidad.ToList());
                if (!resultado)
                {
                    MessageBox.Show("No fue posible imprimir ticket Mensualidad", "Error Imprimir PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                RestablecerPPM();
                Cargando(false);
            }
            else
            {
                RestablecerPPM();
                Cargando(false);
                MessageBox.Show(oInfoFacturaResponse.ErrorMessage, "Error Imprimir PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public bool PrintTicketMensualidad(List<InfoItemsFacturaMensualidadResponse> datos)
        {
            bool bPrint = false;

            try
            {

                ReportDataSource datasource = new ReportDataSource();
                LocalReport oLocalReport = new LocalReport();

                datasource = new ReportDataSource("DataSetTicketPago", (DataTable)GenerarTicketPagoMensualidad(datos).Tables[0]);
                oLocalReport.ReportPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format(@"Tickets\{0}.rdlc", "ticketPagoM"));


                oLocalReport.DataSources.Add(datasource);
                oLocalReport.Refresh();

                ReportPrintDocument ore = new ReportPrintDocument(oLocalReport);
                ore.PrintController = new StandardPrintController();
                ore.Print();

                oLocalReport.Dispose();
                oLocalReport = null;
                ore.Dispose();
                ore = null;

                bPrint = true;
            }
            catch (Exception e)
            {
                bPrint = false;
            }
            return bPrint;
        }
        private DataSetTicketPagoMensualidad GenerarTicketPagoMensualidad(List<InfoItemsFacturaMensualidadResponse> infoTicket)
        {
            DataSetTicketPagoMensualidad facturacion = new DataSetTicketPagoMensualidad();

            double total = 0;
            foreach (var item in infoTicket)
            {
                total += Convert.ToDouble(item.Total);
            }

            foreach (var item in infoTicket)
            {
                DataSetTicketPagoMensualidad.TablaTicketPagoRow rowDatosFactura = facturacion.TablaTicketPago.NewTablaTicketPagoRow();

                rowDatosFactura.Direccion = item.Direccion;
                rowDatosFactura.Fecha = item.Fecha;
                rowDatosFactura.IdTransaccion = item.IdTransaccion;
                rowDatosFactura.Informacion = "Esta infromacion esta quemada en el codigo, deberia obtenerse de algun lugar";
                rowDatosFactura.Modulo = item.Modulo;
                rowDatosFactura.Nombre = item.Nombre;
                rowDatosFactura.NumeroFactura = item.NumeroFactura;
                //string[] data = item.NumeroResolucion.Split('\n');
                //int co = data.Count();
                //switch (co)
                //{
                //    case 3:
                //        rowDatosFactura.Resolucion = data[0] + " \n" + data[1] + " " + data[2] + " ";
                //        break;
                //    case 4:
                //        rowDatosFactura.Resolucion = data[0] + " \n" + data[1] + " " + data[2] + " " + data[3];
                //        break;
                //}
                rowDatosFactura.Resolucion = item.NumeroResolucion;
               //
                rowDatosFactura.Rut = "NIT 900.250.796-0"; 
                rowDatosFactura.Telefono = item.Telefono;
                rowDatosFactura.TotalFinal = total;
                rowDatosFactura.Total = Convert.ToDouble(item.Total);
                rowDatosFactura.Subtotal = Convert.ToDouble(item.Subtotal);
                rowDatosFactura.Iva = Convert.ToDouble(item.Iva);
                rowDatosFactura.TipoPago = item.Tipo;
                rowDatosFactura.NombreAutorizacion = item.NombreAutorizacion;
                rowDatosFactura.Documento = item.Documento;
                rowDatosFactura.VigenciaFactura = item.Vigencia.Trim('\n');

                //NEW FIELDS
                rowDatosFactura.Nit = item.NombreEmpresa;
                rowDatosFactura.NombreEmpresa = item.Nit;
                rowDatosFactura.NombreAutorizado =item.NombreAutorizado ;
                //

                facturacion.TablaTicketPago.AddTablaTicketPagoRow(rowDatosFactura);
            }

            return facturacion;
        }
        private void ImprimirArqueo(string idArqueo)
        {
            InfoArqueoResponse oInfoFacturaResponse = cliente.ObtenerDatosComprobanteArqueo(idArqueo);
            if (oInfoFacturaResponse.Exito)
            {
                bool resultado = PrintTicketArqueo(oInfoFacturaResponse.LstInfoArqueos.ToList());
                if (!resultado)
                {
                    MessageBox.Show("No fue posible imprimir ticket", "Error Imprimir PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show(oInfoFacturaResponse.ErrorMessage, "Error Imprimir PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private bool PrintTicketArqueo(List<InfoItemsArqueoResponse> datos)
        {
            bool bPrint = false;

            try
            {
                foreach (InfoItemsArqueoResponse item in datos)
                {
                    ReportDataSource datasource = new ReportDataSource();
                    LocalReport oLocalReport = new LocalReport();

                    datasource = new ReportDataSource("DataSetTicketArqueo", (DataTable)GenerarTicketArqueo(item).Tables[0]);
                    oLocalReport.ReportPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format(@"Tickets\{0}.rdlc", "ticketArqueo"));


                    oLocalReport.DataSources.Add(datasource);
                    oLocalReport.Refresh();

                    ReportPrintDocument ore = new ReportPrintDocument(oLocalReport);
                    ore.PrintController = new StandardPrintController();
                    ore.Print();

                    oLocalReport.Dispose();
                    oLocalReport = null;
                    ore.Dispose();
                    ore = null;

                    bPrint = true;
                }
            }
            catch (Exception e)
            {
                bPrint = false;
            }
            return bPrint;
        }
        private DataSetTicketArqueo GenerarTicketArqueo(InfoItemsArqueoResponse infoTicket)
        {
            DataSetTicketArqueo facturacion = new DataSetTicketArqueo();

            DataSetTicketArqueo.TablaTicketArqueoRow rowDatosFactura = facturacion.TablaTicketArqueo.NewTablaTicketArqueoRow();

            rowDatosFactura.Valor = infoTicket.Valor != "" ? Convert.ToDouble(infoTicket.Valor) : 0;
            rowDatosFactura.Producido = Convert.ToDouble(infoTicket.Producido);
            rowDatosFactura.Direccion = infoTicket.Direccion;
            rowDatosFactura.Fecha = infoTicket.Fecha;
            rowDatosFactura.IdArqueo = infoTicket.IdArqueo;
            rowDatosFactura.Modulo = infoTicket.Modulo;
            rowDatosFactura.Nombre = infoTicket.Nombre;
            rowDatosFactura.Telefono = infoTicket.Telefono;
            rowDatosFactura.Cantidad = infoTicket.CantTransacciones;
            rowDatosFactura.IdUsuario = infoTicket.IdUsuario;
            rowDatosFactura.Conteo = Convert.ToDouble(infoTicket.Conteo);


            facturacion.TablaTicketArqueo.AddTablaTicketArqueoRow(rowDatosFactura);

            return facturacion;
        }
        private void ImprimirCarga(string idCarga)
        {
            InfoCargaResponse oInfoFacturaResponse = cliente.ObtenerDatosComprobanteCarga(idCarga);
            if (oInfoFacturaResponse.Exito)
            {
                bool resultado = PrintTicketCarga(oInfoFacturaResponse.LstInfoCargas.ToList());
                if (!resultado)
                {
                    MessageBox.Show("No fue posible imprimir ticket", "Error Imprimir PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show(oInfoFacturaResponse.ErrorMessage, "Error Imprimir PPM", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private bool PrintTicketCarga(List<InfoItemsCargaResponse> datos)
        {
            bool bPrint = false;

            try
            {
                foreach (InfoItemsCargaResponse item in datos)
                {
                    ReportDataSource datasource = new ReportDataSource();
                    LocalReport oLocalReport = new LocalReport();

                    datasource = new ReportDataSource("DataSetTicketCarga", (DataTable)GenerarTicketCarga(item).Tables[0]);
                    oLocalReport.ReportPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format(@"Tickets\{0}.rdlc", "ticketCarga"));


                    oLocalReport.DataSources.Add(datasource);
                    oLocalReport.Refresh();

                    ReportPrintDocument ore = new ReportPrintDocument(oLocalReport);
                    ore.PrintController = new StandardPrintController();
                    ore.Print();

                    oLocalReport.Dispose();
                    oLocalReport = null;
                    ore.Dispose();
                    ore = null;

                    bPrint = true;
                }
            }
            catch (Exception e)
            {
                bPrint = false;
            }
            return bPrint;
        }
        private DataSetTicketCarga GenerarTicketCarga(InfoItemsCargaResponse infoTicket)
        {
            DataSetTicketCarga facturacion = new DataSetTicketCarga();

            DataSetTicketCarga.TablaTicketCargaRow rowDatosFactura = facturacion.TablaTicketCarga.NewTablaTicketCargaRow();

            rowDatosFactura.Valor = Convert.ToDouble(infoTicket.Valor);
            rowDatosFactura.Direccion = infoTicket.Direccion;
            rowDatosFactura.Fecha = infoTicket.Fecha;
            rowDatosFactura.IdCarga = infoTicket.IdCarga;
            rowDatosFactura.Modulo = infoTicket.Modulo;
            rowDatosFactura.Nombre = infoTicket.Nombre;
            rowDatosFactura.Telefono = infoTicket.Telefono;
            rowDatosFactura.IdEstacionamiento = infoTicket.IdEstacionamiento;
            rowDatosFactura.IdUsuario = infoTicket.IdUsuario;


            facturacion.TablaTicketCarga.AddTablaTicketCargaRow(rowDatosFactura);

            return facturacion;
        }
        #endregion

        private void panelTodo_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tbIdTransaccion_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

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
