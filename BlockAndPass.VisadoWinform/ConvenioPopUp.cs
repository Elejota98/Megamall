using EGlobalT.Device.SmartCard;
using EGlobalT.Device.SmartCardReaders;
using EGlobalT.Device.SmartCardReaders.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace BlockAndPass.VisadoWinform
{
    public partial class ConvenioPopUp : Form
    {
        Convenios misConvenios = new Convenios();

        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "convenioFile.dat");

        public ConvenioPopUp()
        {
            InitializeComponent();

            if (File.Exists(path))
            {
                misConvenios = DeSerializeObject<Convenios>(path);
                if (misConvenios != null)
                {
                    this.cbConvenio.DataSource = misConvenios.ListConvenio;
                    this.cbConvenio.DisplayMember = "Nombre";
                    this.cbConvenio.ValueMember = "Identificador";
                }
                else
                {
                    MessageBox.Show("No encontro convenios", "Visado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("No encontro convenios", "Visado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void btn_Ok_Click(object sender, EventArgs e)
        {
            Convenio oConvenio = (Convenio)cbConvenio.SelectedItem;
            bool bContinuar = true;
            if (oConvenio.Tipo == TipoConvenio.Bolsa)
            {
                if (oConvenio.CantidadHorasTotal > oConvenio.CantidadHorasUsadas)
                {
                    bContinuar = true;
                }
                else
                {
                    bContinuar = false;
                    MessageBox.Show("No cuenta con horas disponibles para aplicar este convenio.", "Visado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            if (bContinuar)
            {
                string clave = System.Configuration.ConfigurationManager.AppSettings["ClaveTarjeta"];
                if (clave != string.Empty)
                {
                    CardResponse oCardResponse = AplicarConvenio(clave, oConvenio.Identificador.ToString());
                    if (!oCardResponse.error)
                    {
                        MessageBox.Show("Convenio aplicado exitosamente, quedan disponibles " + (oConvenio.CantidadHorasTotal - oConvenio.CantidadHorasUsadas - 1).ToString() + " aplicaciones del convenio.", "Visado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        if (oConvenio.Tipo == TipoConvenio.Bolsa)
                        {
                            foreach (var item in misConvenios.ListConvenio)
                            {
                                if (item.Identificador == oConvenio.Identificador)
                                {
                                    item.CantidadHorasUsadas++;
                                    break;
                                }
                            }

                            SerializeObject<Convenios>(misConvenios, path);

                        }
                    }
                    else
                    {
                        MessageBox.Show(oCardResponse.errorMessage, "Visado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }     
                }
                else
                {
                    MessageBox.Show("No se encontro parametro claveTarjeta.", "Error Aplicar Convenio", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cbConvenio_SelectedIndexChanged(object sender, EventArgs e)
        {
            Convenio oConvenio = (Convenio)cbConvenio.SelectedItem;
            tbDescripcion.Text = oConvenio.Descripcion;
        }

        /// <summary>
        /// Serializes an object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serializableObject"></param>
        /// <param name="fileName"></param>
        public void SerializeObject<T>(T serializableObject, string fileName)
        {
            if (serializableObject == null) { return; }

            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                XmlSerializer serializer = new XmlSerializer(serializableObject.GetType());
                using (MemoryStream stream = new MemoryStream())
                {
                    serializer.Serialize(stream, serializableObject);
                    stream.Position = 0;
                    xmlDocument.Load(stream);
                    xmlDocument.Save(fileName);
                    stream.Close();
                }
            }
            catch (Exception ex)
            {
                //Log exception here
            }
        }

        /// <summary>
        /// Deserializes an xml file into an object list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public T DeSerializeObject<T>(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) { return default(T); }

            T objectOut = default(T);

            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(fileName);
                string xmlString = xmlDocument.OuterXml;

                using (StringReader read = new StringReader(xmlString))
                {
                    Type outType = typeof(T);

                    XmlSerializer serializer = new XmlSerializer(outType);
                    using (XmlReader reader = new XmlTextReader(read))
                    {
                        objectOut = (T)serializer.Deserialize(reader);
                        reader.Close();
                    }

                    read.Close();
                }
            }
            catch (Exception ex)
            {
                //Log exception here
            }

            return objectOut;
        }

        #region Tarjetas
        public CardResponse AplicarConvenio(string password, string idConvenio)
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

                                if (myTarjeta.CodeAgreement1 == null || myTarjeta.CodeAgreement1 == 0)
                                {
                                    myTarjeta.CodeAgreement1 = Convert.ToInt32(idConvenio);
                                }
                                else if (myTarjeta.CodeAgreement2 == null || myTarjeta.CodeAgreement2 == 0)
                                {
                                    myTarjeta.CodeAgreement2 = Convert.ToInt32(idConvenio);
                                }
                                else if (myTarjeta.CodeAgreement3 == null || myTarjeta.CodeAgreement3 == 0)
                                {
                                    myTarjeta.CodeAgreement3 = Convert.ToInt32(idConvenio);
                                }
                                else
                                {
                                    myTarjeta.CodeAgreement1 = Convert.ToInt32(idConvenio);
                                }
                                
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
        #endregion

        private void btn_Leer_Click(object sender, EventArgs e)
        {
            string clave = System.Configuration.ConfigurationManager.AppSettings["ClaveTarjeta"];
            if (clave != string.Empty)
            {
                CardResponse oCardResponse = GetCardInfo(clave);

                if (!oCardResponse.error)
                {
                    tbFechaIngreso.Text = ((DateTime)oCardResponse.fechEntrada).ToString("dd/MM/yyyy HH:mm:ss");
                    tbTipoVehiculo.Text = oCardResponse.tipoVehiculo;
                }
                else
                {
                    MessageBox.Show(oCardResponse.errorMessage, "Visado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("No se encontro parametro claveTarjeta.", "Error Leer Tarjeta", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
