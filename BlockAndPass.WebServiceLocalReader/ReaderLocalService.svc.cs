using BlockAndPass.WebServiceLocalReader.Tickets;
using EGlobalT.Device.SmartCard;
using EGlobalT.Device.SmartCardReaders;
using EGlobalT.Device.SmartCardReaders.Entities;
using LectorDevice;
using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.Data;
//using System.Collections.Generic;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Helpers;

namespace BlockAndPass.WebServiceLocalReader
{
    public class ReaderLocalService : IReaderLocalService
    {
        Lectora oLectora = new Lectora();

        public CardResponse GetIdCard()
        {
            CardResponse oCardResponse = new CardResponse();
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();
            ////Lectora_ACR122U lectora = new Lectora_ACR122U();
            //Lectora oLectora = new Lectora();
            ////Rspsta_Conexion_LECTOR res = lectora.Conectar(false);
            oResultadoOperacion = oLectora.LoadKey();

            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
            //    //if (res.Conectado)
            //    //{
            //    //Rspsta_DetectarTarjeta_LECTOR re = lectora.DetectarTarjeta();
            //    //if (re.TarjetaDetectada)
            //    //{
                oResultadoOperacion = oLectora.ObtenerID();

                if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
                {
                    oCardResponse.error = false;
                    oCardResponse.idCard = oResultadoOperacion.EntidadDatos.ToString();
                    oResultadoOperacion = oLectora.DesconectarCRT();
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
            //    oCardResponse.errorMessage = " / No conecta con lectora de tarjetas.";
            }

           
            return oCardResponse;
        }

        public CardResponse ReplaceCard()
        {
            CardResponse oCardResponse = new CardResponse();
            //string password = HttpContext.Current.Request.Params["password"];
            string password = "ADDAADDAADDA";
            string plate = HttpContext.Current.Request.Params["plate"];
            string modulo = HttpContext.Current.Request.Params["modulo"];

            string dia = HttpContext.Current.Request.Params["dia"];
            string mes = HttpContext.Current.Request.Params["mes"];
            string anho = HttpContext.Current.Request.Params["anho"];

            string hora = HttpContext.Current.Request.Params["hora"];
            string min = HttpContext.Current.Request.Params["min"];
            string seg = HttpContext.Current.Request.Params["seg"];

            string ampm = HttpContext.Current.Request.Params["ampm"];

            string tipov = HttpContext.Current.Request.Params["tipov"];

            int horaFinal = 0;
            if (ampm == "p.m." || ampm == "p.")
            {
                horaFinal = Convert.ToInt32(hora) + 12;
            }
            else
            {
                horaFinal = Convert.ToInt32(hora);
            }

            //Lectora_ACR122U lectora = new Lectora_ACR122U();
            Lectora oLectora = new Lectora();
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();
            //Rspsta_Conexion_LECTOR res = lectora.Conectar(false);
            //if (res.Conectado)
            //{
            oResultadoOperacion = oLectora.LoadKey();

            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                //Rspsta_DetectarTarjeta_LECTOR re = lectora.DetectarTarjeta();
                //if (re.TarjetaDetectada)
                //{
                oResultadoOperacion = oLectora.ObtenerID();

                if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
                {

                    oCardResponse.idCard = oResultadoOperacion.EntidadDatos.ToString();
                    //Rspsta_EstablecerClave_LECTOR resp1 = lectora.EstablecerClaveLECTOR(password);
                    //if (resp1.ClaveEstablecida)
                    //{

                    //Rspsta_BorrarTarjeta_LECTOR respBorrar = lectora.BorrarTarjetaLECTORA(TYPE_STRUCTURE_SMARTCARD.SMARTCARD_PARKING_V1, false);
                    oResultadoOperacion = oLectora.BorrarTI(password);
                    if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
                    {
                         DateTime fecha = new DateTime(Convert.ToInt32(anho), Convert.ToInt32(mes), Convert.ToInt32(dia), Convert.ToInt32(hora), Convert.ToInt32(min), Convert.ToInt32(seg));
                         Tarjeta oTarjeta = new LectorDevice.Tarjeta();
                         oTarjeta.ActiveCycle = true;
                         oTarjeta.DateTimeEntrance = fecha;
                         oTarjeta.TypeCard = 1;
                         oTarjeta.EntranceModule = modulo;
                         if (oTarjeta.EntranceModule != "ENTR03")
                         {
                           oTarjeta.TypeVehicle = 1;
                          }
                          else
                           {
                             oTarjeta.TypeVehicle = 2;
                            }

                           oResultadoOperacion = oLectora.EscribirTI(oTarjeta);

                           if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
                           {
                               oResultadoOperacion = oLectora.EscribirReposicion(password);


                               if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
                               {
                                   oCardResponse.error = false;
                               }
                               else
                               {
                                   oCardResponse.error = true;
                                   oCardResponse.errorMessage = "Error el momento de aplicar la reposición";

                               }
                           }
                           else
                           {
                               oCardResponse.error = true;
                               oCardResponse.errorMessage = "Error al escribir en la tarjeta";
                           }

                        //SMARTCARD_PARKING_V1 oTarjeta = new SMARTCARD_PARKING_V1();
                        //oTarjeta.TypeCard = TYPE_TARJETAPARKING_V1.VISITOR;
                        //if (tipov == "1")
                        //{
                        //    oTarjeta.TypeVehicle = TYPEVEHICLE_TARJETAPARKING_V1.AUTOMOBILE;
                        //}
                        //else if (tipov == "2")
                        //{
                        //    oTarjeta.TypeVehicle = TYPEVEHICLE_TARJETAPARKING_V1.MOTORCYCLE;
                        //}
                        //oTarjeta.Replacement = true;
                        //oTarjeta.CodeCard = resp.CodigoTarjeta;
                        //oTarjeta.ActiveCycle = true;

                        //DateTime fecha = new DateTime();

                        //try
                        //{
                            //fecha = new DateTime(Convert.ToInt32(anho), Convert.ToInt32(mes), Convert.ToInt32(dia), Convert.ToInt32(hora), Convert.ToInt32(min), Convert.ToInt32(seg));

                        //    //       fecha = new DateTime(Convert.ToInt32(anho), Convert.ToInt32(mes), Convert.ToInt32(dia), horaFinal, Convert.ToInt32(min), Convert.ToInt32(seg));
                        //}
                        //catch (Exception fex)
                        //{
                        //    //       fecha = new DateTime(Convert.ToInt32(anho), Convert.ToInt32(dia), Convert.ToInt32(mes), horaFinal, Convert.ToInt32(min), Convert.ToInt32(seg));
                        //    fecha = new DateTime(Convert.ToInt32(anho), Convert.ToInt32(dia), Convert.ToInt32(mes), Convert.ToInt32(hora), Convert.ToInt32(min), Convert.ToInt32(seg));
                        //}



                        //oTarjeta.DateTimeEntrance = fecha;



                        //oTarjeta.EntranceModule = modulo;
                        //oTarjeta.EntrancePlate = plate;
                        //Rspsta_Escribir_Tarjeta_LECTOR resp4 = lectora.EscribirTarjeta(oTarjeta, false, false);
                        //if (resp4.TarjetaEscrita)
                        //{
                       
                        //oCardResponse.idCard = resp.CodigoTarjeta;
                        //}
                        //else
                        //{
                        //    oCardResponse.error = true;
                        //    oCardResponse.errorMessage = "No escribe tarjeta.";
                        //}
                    }
                    else
                    {
                        oCardResponse.error = true;
                        oCardResponse.errorMessage = "No borra tarjeta.";
                    }
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
            }
            else
            {
                oCardResponse.error = true;
                oCardResponse.errorMessage = "No conecta con lectora de tarjetas.";
            }


            return oCardResponse;
        }

        public CardResponse CreateCard()
        {
            CardResponse oCardResponse = new CardResponse();
            string password1 = HttpContext.Current.Request.Params["passwordIni"];
            string password2 = HttpContext.Current.Request.Params["passwordFin"];

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
                        Rspsta_EstablecerClave_LECTOR resp1 = lectora.EstablecerClaveLECTOR(password1);
                        if (resp1.ClaveEstablecida)
                        {

                            Rspsta_BorrarTarjeta_LECTOR respBorrar = lectora.BorrarTarjetaLECTORA(TYPE_STRUCTURE_SMARTCARD.SMARTCARD_PARKING_V1, false);

                            if (respBorrar.TarjetaBorrada)
                            {
                                Rspsta_CambiarClaveTarjeta_LECTOR respCambio = lectora.CambiarClaveTarjetaLECTORA(TYPE_STRUCTURE_SMARTCARD.SMARTCARD_PARKING_V1, password1, password2, false);
                                if (respCambio.ClaveCambiada)
                                {
                                    oCardResponse.error = false;
                                }
                                else
                                {
                                    oCardResponse.error = true;
                                    oCardResponse.errorMessage = "No cambia clave tarjeta.";
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

        public CardResponse CreateAuthCard()
        {
            string password = "ADDAADDAADDA";
            CardResponse oCardResponse = new CardResponse();
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();
            //Lectora_ACR122U lectora = new Lectora_ACR122U();
            Lectora oLectora = new Lectora();
            //Rspsta_Conexion_LECTOR res = lectora.Conectar(false);
            oResultadoOperacion = oLectora.LoadKey();

            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                //if (res.Conectado)
                //{
                //Rspsta_DetectarTarjeta_LECTOR re = lectora.DetectarTarjeta();
                //if (re.TarjetaDetectada)
                //{
                oResultadoOperacion = oLectora.ObtenerID();

                if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
                {
                    oCardResponse.idCard = oResultadoOperacion.EntidadDatos.ToString();

                    oResultadoOperacion = oLectora.BorrarTI(password);

                    if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
                    {
                        oResultadoOperacion = oLectora.DesconectarCRT();
                        oCardResponse.error = false;
                        
                    }
                    else
                    {
                        oCardResponse.error = true;
                        oCardResponse.errorMessage = "No fue posible el borrado de la tarjeta";
                    }
                    
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
            }
            else
            {
                oCardResponse.error = true;
                oCardResponse.errorMessage = " / No conecta con lectora de tarjetas.";
            }

            return oCardResponse;

            //CardResponse oCardResponse = new CardResponse();
            //string password = HttpContext.Current.Request.Params["password"];
            //string idAutho = HttpContext.Current.Request.Params["idAutho"];

            //Lectora_ACR122U lectora = new Lectora_ACR122U();
            //Rspsta_Conexion_LECTOR res = lectora.Conectar(false);
            //if (res.Conectado)
            //{
            //    Rspsta_DetectarTarjeta_LECTOR re = lectora.DetectarTarjeta();
            //    if (re.TarjetaDetectada)
            //    {
            //        Rspsta_CodigoTarjeta_LECTOR resp = lectora.ObtenerIDTarjeta();
            //        if (resp.CodigoTarjeta != null && resp.CodigoTarjeta != string.Empty)
            //        {
            //            oCardResponse.idCard = resp.CodigoTarjeta;
            //            Rspsta_EstablecerClave_LECTOR resp1 = lectora.EstablecerClaveLECTOR(password);
            //            if (resp1.ClaveEstablecida)
            //            {

            //                Rspsta_BorrarTarjeta_LECTOR respBorrar = lectora.BorrarTarjetaLECTORA(TYPE_STRUCTURE_SMARTCARD.SMARTCARD_PARKING_V1, false);

            //                if (respBorrar.TarjetaBorrada)
            //                {
            //                    SMARTCARD_PARKING_V1 oTarjeta = new SMARTCARD_PARKING_V1();
            //                    oTarjeta.TypeCard = TYPE_TARJETAPARKING_V1.AUTHORIZED_PARKING;
            //                    oTarjeta.CodeCard = resp.CodigoTarjeta;
            //                    oTarjeta.ActiveCycle = false;
            //                    //oTarjeta.CodeAgreement1 = Convert.ToInt32(idAutho);

            //                    Rspsta_Escribir_Tarjeta_LECTOR resp4 = lectora.EscribirTarjeta(oTarjeta, false, false);
            //                    if (resp4.TarjetaEscrita)
            //                    {
            //                        oCardResponse.error = false;
            //                    }
            //                    else
            //                    {
            //                        oCardResponse.error = true;
            //                        oCardResponse.errorMessage = "No escribe tarjeta.";
            //                    }
            //                }
            //                else
            //                {
            //                    oCardResponse.error = true;
            //                    oCardResponse.errorMessage = "No borra tarjeta.";
            //                }
            //            }
            //            else
            //            {
            //                oCardResponse.error = true;
            //                oCardResponse.errorMessage = "No establece clave tarjeta.";
            //            }
            //        }
            //        else
            //        {
            //            oCardResponse.error = true;
            //            oCardResponse.errorMessage = "No obtiene id tarjeta.";
            //        }
            //    }
            //    else
            //    {
            //        oCardResponse.error = true;
            //        oCardResponse.errorMessage = "No detecta tarjeta.";
            //    }
            //}
            //else
            //{
            //    oCardResponse.error = true;
            //    oCardResponse.errorMessage = "No conecta con lectora de tarjetas.";
            //}


            
        }

        public CardResponse CreateAuthCardRepo()
        {   
            string password = "ADDAADDAADDA";
            CardResponse oCardResponse = new CardResponse();
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();
            //Lectora_ACR122U lectora = new Lectora_ACR122U();
            Lectora oLectora = new Lectora();
            //Rspsta_Conexion_LECTOR res = lectora.Conectar(false);
            oResultadoOperacion = oLectora.LoadKey();

            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                //if (res.Conectado)
                //{
                //Rspsta_DetectarTarjeta_LECTOR re = lectora.DetectarTarjeta();
                //if (re.TarjetaDetectada)
                //{
                oResultadoOperacion = oLectora.ObtenerID();

                if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
                {

                    oCardResponse.idCard = oResultadoOperacion.EntidadDatos.ToString();

                    //Tarjeta oTarjeta = new LectorDevice.Tarjeta();
                    //DateTime fechan = Convert.ToDateTime(DateTime.Now);
                    //oTarjeta.ActiveCycle = false;
                    //oTarjeta.DateTimeEntrance = fechan;
                    //oTarjeta.TypeVehicle = 1;
                    //oTarjeta.TypeCard = 1;
                    //oTarjeta.EntranceModule = "ENTR01";
                    //oTarjeta.Replacement = true;

                    //oResultadoOperacion = oLectora.EscribirTI(oTarjeta);

                    //if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
                    //{
                        oResultadoOperacion = oLectora.EscribirReposicion(password);

                        if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
                        {
                            oCardResponse.error = false;
                        }
                        else
                        {
                            oCardResponse.error = true;
                            oCardResponse.errorMessage = "No escribe reposición de autorizado";
                        }
                    //}
                    //else
                    //{
                    //    oCardResponse.error = true;
                    //    oCardResponse.errorMessage = "No escribe tarjeta de autorizado";
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
            }
            else
            {
                oCardResponse.error = true;
                oCardResponse.errorMessage = " / No conecta con lectora de tarjetas.";
            }

            return oCardResponse;

            //CardResponse oCardResponse = new CardResponse();
            //string password = HttpContext.Current.Request.Params["password"];
            //string idAutho = HttpContext.Current.Request.Params["idAutho"];

            //Lectora_ACR122U lectora = new Lectora_ACR122U();
            //Rspsta_Conexion_LECTOR res = lectora.Conectar(false);
            //if (res.Conectado)
            //{
            //    Rspsta_DetectarTarjeta_LECTOR re = lectora.DetectarTarjeta();
            //    if (re.TarjetaDetectada)
            //    {
            //        Rspsta_CodigoTarjeta_LECTOR resp = lectora.ObtenerIDTarjeta();
            //        if (resp.CodigoTarjeta != null && resp.CodigoTarjeta != string.Empty)
            //        {
            //            oCardResponse.idCard = resp.CodigoTarjeta;
            //            Rspsta_EstablecerClave_LECTOR resp1 = lectora.EstablecerClaveLECTOR(password);
            //            if (resp1.ClaveEstablecida)
            //            {

            //                Rspsta_BorrarTarjeta_LECTOR respBorrar = lectora.BorrarTarjetaLECTORA(TYPE_STRUCTURE_SMARTCARD.SMARTCARD_PARKING_V1, false);

            //                if (respBorrar.TarjetaBorrada)
            //                {
            //                    SMARTCARD_PARKING_V1 oTarjeta = new SMARTCARD_PARKING_V1();
            //                    oTarjeta.TypeCard = TYPE_TARJETAPARKING_V1.AUTHORIZED_PARKING;
            //                    oTarjeta.CodeCard = resp.CodigoTarjeta;
            //                    oTarjeta.ActiveCycle = false;
            //                    oTarjeta.Replacement = true;
            //                    //oTarjeta.CodeAgreement1 = Convert.ToInt32(idAutho);

            //                    Rspsta_Escribir_Tarjeta_LECTOR resp4 = lectora.EscribirTarjeta(oTarjeta, false, false);
            //                    if (resp4.TarjetaEscrita)
            //                    {
            //                        oCardResponse.error = false;
            //                    }
            //                    else
            //                    {
            //                        oCardResponse.error = true;
            //                        oCardResponse.errorMessage = "No escribe tarjeta.";
            //                    }
            //                }
            //                else
            //                {
            //                    oCardResponse.error = true;
            //                    oCardResponse.errorMessage = "No borra tarjeta.";
            //                }
            //            }
            //            else
            //            {
            //                oCardResponse.error = true;
            //                oCardResponse.errorMessage = "No establece clave tarjeta.";
            //            }
            //        }
            //        else
            //        {
            //            oCardResponse.error = true;
            //            oCardResponse.errorMessage = "No obtiene id tarjeta.";
            //        }
            //    }
            //    else
            //    {
            //        oCardResponse.error = true;
            //        oCardResponse.errorMessage = "No detecta tarjeta.";
            //    }
            //}
            //else
            //{
            //    oCardResponse.error = true;
            //    oCardResponse.errorMessage = "No conecta con lectora de tarjetas.";
            //}


            //return oCardResponse;
        }

        //public CardResponse NewEntryCard()
        //{
        //    CardResponse oCardResponse = new CardResponse();
        //    string password = HttpContext.Current.Request.Params["password"];

        //    string plate = HttpContext.Current.Request.Params["plate"];
        //    string modulo = HttpContext.Current.Request.Params["modulo"];

        //    string dia = HttpContext.Current.Request.Params["dia"];
        //    string mes = HttpContext.Current.Request.Params["mes"];
        //    string anho = HttpContext.Current.Request.Params["anho"];

        //    string hora = HttpContext.Current.Request.Params["hora"];
        //    string min = HttpContext.Current.Request.Params["min"];

        //    string ampm = HttpContext.Current.Request.Params["ampm"];

        //    string tipov = HttpContext.Current.Request.Params["tipov"];

        //    int horaFinal = 0;
        //    if (ampm == "p.m.")
        //    {
        //        horaFinal = Convert.ToInt32(hora) + 12;
        //    }
        //    else
        //    {
        //        horaFinal = Convert.ToInt32(hora);
        //    }

        //    Lectora_ACR122U lectora = new Lectora_ACR122U();
        //    Rspsta_Conexion_LECTOR res = lectora.Conectar(false);
        //    if (res.Conectado)
        //    {
        //        Rspsta_DetectarTarjeta_LECTOR re = lectora.DetectarTarjeta();
        //        if (re.TarjetaDetectada)
        //        {
        //            Rspsta_CodigoTarjeta_LECTOR resp = lectora.ObtenerIDTarjeta();
        //            if (resp.CodigoTarjeta != null && resp.CodigoTarjeta != string.Empty)
        //            {
        //                Rspsta_EstablecerClave_LECTOR resp1 = lectora.EstablecerClaveLECTOR(password);
        //                if (resp1.ClaveEstablecida)
        //                {

        //                    Rspsta_BorrarTarjeta_LECTOR respBorrar = lectora.BorrarTarjetaLECTORA(TYPE_STRUCTURE_SMARTCARD.SMARTCARD_PARKING_V1, false);

        //                    if (respBorrar.TarjetaBorrada)
        //                    {
        //                        SMARTCARD_PARKING_V1 oTarjeta = new SMARTCARD_PARKING_V1();
        //                        oTarjeta.TypeCard = TYPE_TARJETAPARKING_V1.VISITOR;
        //                        if (tipov == "1")
        //                        {
        //                            oTarjeta.TypeVehicle = TYPEVEHICLE_TARJETAPARKING_V1.AUTOMOBILE;
        //                        }
        //                        else if (tipov == "2")
        //                        {
        //                            oTarjeta.TypeVehicle = TYPEVEHICLE_TARJETAPARKING_V1.MOTORCYCLE;
        //                        }

        //                        oTarjeta.CodeCard = resp.CodigoTarjeta;
        //                        oTarjeta.ActiveCycle = true;
        //                        oTarjeta.DateTimeEntrance = new DateTime(Convert.ToInt32(anho), Convert.ToInt32(mes), Convert.ToInt32(dia), horaFinal, Convert.ToInt32(min), 0);
        //                        oTarjeta.EntranceModule = modulo;
        //                        oTarjeta.EntrancePlate = plate;
        //                        Rspsta_Escribir_Tarjeta_LECTOR resp4 = lectora.EscribirTarjeta(oTarjeta, false, false);
        //                        if (resp4.TarjetaEscrita)
        //                        {
        //                            oCardResponse.error = false;
        //                        }
        //                        else
        //                        {
        //                            oCardResponse.error = true;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        oCardResponse.error = true;
        //                    }
        //                }
        //                else
        //                {
        //                    oCardResponse.error = true;
        //                }
        //            }
        //            else
        //            {
        //                oCardResponse.error = true;
        //            }
        //        }
        //        else
        //        {
        //            oCardResponse.error = true;
        //        }
        //    }
        //    else
        //    {
        //        oCardResponse.error = true;
        //    }


        //    return oCardResponse;
        //}

        public CardResponse GetCardInfo()
        {
            CardResponse oCardResponse = new CardResponse();
            string password = HttpContext.Current.Request.Params["password"];
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();
            Lectora oLectora = new Lectora();
            oResultadoOperacion = oLectora.LoadKey();

            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                //Rspsta_DetectarTarjeta_LECTOR re = lectora.DetectarTarjeta();
                //if (re.TarjetaDetectada)
                //{
                oResultadoOperacion = oLectora.ObtenerID();

                if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
                {
                    oCardResponse.idCard = oResultadoOperacion.EntidadDatos.ToString();
                    //Rspsta_EstablecerClave_LECTOR resp1 = lectora.EstablecerClaveLECTOR(password);
                    //if (resp1.ClaveEstablecida)
                    //{
                    oResultadoOperacion = oLectora.LeerTI(password);
                    if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
                    {
                        //oCardResponse.error = false;
                        //SMARTCARD_PARKING_V1 myTarjeta = (SMARTCARD_PARKING_V1)resp2.Tarjeta;
                        //oCardResponse.cicloActivo = myTarjeta.ActiveCycle != null ? (bool)myTarjeta.ActiveCycle : false;
                        //oCardResponse.codeAutorizacion1 = myTarjeta.CodeAgreement1 != null ? (int)myTarjeta.CodeAgreement1 : 0;
                        //oCardResponse.codeAutorizacion2 = myTarjeta.CodeAgreement2 != null ? (int)myTarjeta.CodeAgreement2 : 0;
                        //oCardResponse.codeAutorizacion3 = myTarjeta.CodeAgreement3 != null ? (int)myTarjeta.CodeAgreement3 : 0;
                        //oCardResponse.cortesia = myTarjeta.Courtesy != null ? (bool)myTarjeta.Courtesy : false;
                        //oCardResponse.fechEntrada = myTarjeta.DateTimeEntrance.ToString();
                        //oCardResponse.moduloEntrada = myTarjeta.EntranceModule;
                        //oCardResponse.placa = myTarjeta.EntrancePlate;
                        //oCardResponse.fechaPago = myTarjeta.PaymentDateTime.ToString();
                        //oCardResponse.moduloPago = myTarjeta.PaymentModule;
                        //oCardResponse.reposicion = myTarjeta.Replacement != null ? (bool)myTarjeta.Replacement : false;
                        //oCardResponse.tipoTarjeta = myTarjeta.TypeCard.ToString();
                        //oCardResponse.tipoVehiculo = myTarjeta.TypeVehicle.ToString();
                        //oCardResponse.valet = myTarjeta.ValetParking != null ? (bool)myTarjeta.ValetParking : false;

                        Tarjeta myTarjetaFinal = new Tarjeta();
                        myTarjetaFinal = (Tarjeta)oResultadoOperacion.EntidadDatos;

                        oCardResponse.cicloActivo = myTarjetaFinal.ActiveCycle != null ? (bool)myTarjetaFinal.ActiveCycle : false;
                        oCardResponse.codeAutorizacion1 = myTarjetaFinal.CodeAgreement1 != null ? (int)myTarjetaFinal.CodeAgreement1 : 0;
                        oCardResponse.codeAutorizacion2 = myTarjetaFinal.CodeAgreement2 != null ? (int)myTarjetaFinal.CodeAgreement2 : 0;
                        oCardResponse.codeAutorizacion3 = myTarjetaFinal.CodeAgreement3 != null ? (int)myTarjetaFinal.CodeAgreement3 : 0;
                        oCardResponse.cortesia = myTarjetaFinal.Courtesy != null ? (bool)myTarjetaFinal.Courtesy : false;
                        oCardResponse.fechEntrada = myTarjetaFinal.DateTimeEntrance.ToString();
                        oCardResponse.moduloEntrada = myTarjetaFinal.EntranceModule;
                        oCardResponse.placa = myTarjetaFinal.EntrancePlate;
                        //oCardResponse.fechaPago = myTarjetaFinal.PaymentDateTime.ToString();
                        oCardResponse.fechaPago = DateTime.Now.ToString("yyyy/dd/MM HH:mm:ss");
                        oCardResponse.moduloPago = myTarjetaFinal.PaymentModule;
                        oCardResponse.reposicion = myTarjetaFinal.Replacement != null ? (bool)myTarjetaFinal.Replacement : false;
                        oCardResponse.tipoTarjeta = myTarjetaFinal.TypeCard.ToString();
                        oCardResponse.tipoVehiculo = myTarjetaFinal.TypeVehicle.ToString();
                        oCardResponse.valet = myTarjetaFinal.ValetParking != null ? (bool)myTarjetaFinal.ValetParking : false;

                        oResultadoOperacion = oLectora.DesconectarCRT();

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
            }
            else
            {
                oCardResponse.error = true;
                oCardResponse.errorMessage = "No conecta con lectora de tarjetas.";
            }

            return oCardResponse;
        }

        public CardResponse CreateEntry()
        {
            CardResponse oCardResponse = new CardResponse();
            string password = "ADDAADDAADDA";
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();
            string plate = HttpContext.Current.Request.Params["plate"];
            string modulo = HttpContext.Current.Request.Params["modulo"];

            string dia = HttpContext.Current.Request.Params["dia"];
            string mes = HttpContext.Current.Request.Params["mes"];
            string anho = HttpContext.Current.Request.Params["anho"];

            string hora = HttpContext.Current.Request.Params["hora"];
            string min = HttpContext.Current.Request.Params["min"];

            string tipov = HttpContext.Current.Request.Params["tipov"];
            string idCard = HttpContext.Current.Request.Params["idCard"];


            Lectora oLectora = new Lectora();
            oResultadoOperacion = oLectora.LoadKey();

            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                //Rspsta_DetectarTarjeta_LECTOR re = lectora.DetectarTarjeta();
                //if (re.TarjetaDetectada)
                //{
            oResultadoOperacion = oLectora.ObtenerID();

            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                //Rspsta_EstablecerClave_LECTOR resp1 = lectora.EstablecerClaveLECTOR(password);
                //if (resp1.ClaveEstablecida)
                //{

                oResultadoOperacion = oLectora.BorrarTI(password);

                if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
                {
                    Tarjeta oTarjeta = new Tarjeta();
                    oTarjeta.TypeCard = 1;
                    if (tipov == "1")
                    {
                        oTarjeta.TypeVehicle = 1;
                    }
                    else if (tipov == "2")
                    {
                        oTarjeta.TypeVehicle = 2;
                    }
                    oTarjeta.Replacement = false;
                    oTarjeta.CodeCard = idCard;
                    oTarjeta.ActiveCycle = true;
                    oTarjeta.DateTimeEntrance = new DateTime(Convert.ToInt32(anho), Convert.ToInt32(mes), Convert.ToInt32(dia), Convert.ToInt32(hora), Convert.ToInt32(min), 0);
                    oTarjeta.EntranceModule = modulo;
                    oTarjeta.EntrancePlate = plate;


                    oResultadoOperacion = oLectora.EscribirTI(oTarjeta);
                    if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
                    {
                        oCardResponse.error = false;
                        oLectora.DesconectarCRT();
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
                //}
                //    else
                //    {
                //        oCardResponse.error = true;
                //        oCardResponse.errorMessage = "No detecta tarjeta.";
                //    }
                //}
                //else
                //{
                //    oCardResponse.error = true;
                //    oCardResponse.errorMessage = "No conecta con lectora de tarjetas.";
                //}

            
            return oCardResponse;
        }

        public CardResponse CleanCard()
        {
            CardResponse oCardResponse = new CardResponse();
            string password = HttpContext.Current.Request.Params["password"];
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();
            Lectora oLectora = new Lectora();
            oResultadoOperacion = oLectora.LoadKey();

            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                //Rspsta_DetectarTarjeta_LECTOR re = lectora.DetectarTarjeta();
                //if (re.TarjetaDetectada)
                //{
                oResultadoOperacion = oLectora.ObtenerID();

                if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
                {
                    //Rspsta_EstablecerClave_LECTOR resp1 = lectora.EstablecerClaveLECTOR(password);
                    //if (resp1.ClaveEstablecida)
                    //{

                    oResultadoOperacion = oLectora.BorrarTI(password);

                    if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
                    {
                        oCardResponse.error = false;
                        oCardResponse.errorMessage = "TARJETA BORRADA";
                    }
                    else
                    {
                        oCardResponse.error = true;
                        oCardResponse.errorMessage = "No borra tarjeta.";
                    }
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
            }
            else
            {
                oCardResponse.error = true;
                oCardResponse.errorMessage = "No conecta con lectora de tarjetas.";
            }


            return oCardResponse;
        }

        public CardResponse CleanRepo()
        {
            CardResponse oCardResponse = new CardResponse();
            string password = HttpContext.Current.Request.Params["password"];

              ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();
            Lectora oLectora = new Lectora();
            oResultadoOperacion = oLectora.LoadKey();

            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
            //Lectora_ACR122U lectora = new Lectora_ACR122U();
            //Rspsta_Conexion_LECTOR res = lectora.Conectar(false);
            //if (res.Conectado)
            //{
                 oResultadoOperacion = oLectora.ObtenerID();

                 if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
                 {
                     //Rspsta_DetectarTarjeta_LECTOR re = lectora.DetectarTarjeta();
                     //if (re.TarjetaDetectada)
                     //{
                     //Rspsta_CodigoTarjeta_LECTOR resp = lectora.ObtenerIDTarjeta();
                     //if (resp.CodigoTarjeta != null && resp.CodigoTarjeta != string.Empty)
                     //{

                     oResultadoOperacion = oLectora.BorrarReposicion(password);

                     if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
                     {
                         oCardResponse.error = false;

                     }

                     else
                     {
                         oCardResponse.error = true;
                         oCardResponse.errorMessage = "Ocurrio un error al borrar la reposición";
                     }
                 }

                 else
                 {
                     oCardResponse.error = true;
                     oCardResponse.errorMessage = "No obtiene id tarjeta.";
                 }
                        //Rspsta_EstablecerClave_LECTOR resp1 = lectora.EstablecerClaveLECTOR(password);
                        //if (resp1.ClaveEstablecida)
                        //{
                        //    oCardResponse = LimpiarReposicion(password);
                        //}
                      
                   
                }
                else
                {
                    oCardResponse.error = true;
                    oCardResponse.errorMessage = "No detecta tarjeta.";
                }
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

        public CardResponse AplicarCortesia()
        {
            CardResponse oCardResponse = new CardResponse();
            string password = HttpContext.Current.Request.Params["password"];

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

        public CardResponse AplicarConvenio()
        {
            CardResponse oCardResponse = new CardResponse();
            string password = HttpContext.Current.Request.Params["password"];
            string idConvenio = HttpContext.Current.Request.Params["idConvenio"];
            ResultadoOperacion oResultadoOperacion = new ResultadoOperacion();


            Lectora oLectora = new Lectora();
            oResultadoOperacion = oLectora.LoadKey();

            if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
            {
                //Rspsta_DetectarTarjeta_LECTOR re = lectora.DetectarTarjeta();
                //if (re.TarjetaDetectada)
                //{
                oResultadoOperacion = oLectora.ObtenerID();

                if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
                {
                    //Rspsta_EstablecerClave_LECTOR resp1 = lectora.EstablecerClaveLECTOR(password);
                    //if (resp1.ClaveEstablecida)
                    //{
                    oResultadoOperacion = oLectora.LeerTI(password);
                    if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
                    {
                        Tarjeta myTarjetaFinal = new Tarjeta();
                        myTarjetaFinal = (Tarjeta)oResultadoOperacion.EntidadDatos;
                        myTarjetaFinal.CodeAgreement1 = Convert.ToInt32(idConvenio);

                        oResultadoOperacion = oLectora.EscribirConvenio(Convert.ToInt32(myTarjetaFinal.CodeAgreement1));
                        if (oResultadoOperacion.oEstado == TipoRespuesta.Exito)
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
            }
            else
            {
                oCardResponse.error = true;
                oCardResponse.errorMessage = "No conecta con lectora de tarjetas.";
            }


            return oCardResponse;
        }

        public CardResponse PayCard()
        {
            CardResponse oCardResponse = new CardResponse();
            string password = HttpContext.Current.Request.Params["password"];
            string idTarjeta = HttpContext.Current.Request.Params["idTarjeta"];
            string moduloPago = HttpContext.Current.Request.Params["moduloPago"];

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
                                myTarjeta.PaymentModule = moduloPago;
                                myTarjeta.PaymentDateTime = DateTime.Now;
                                myTarjeta.CodeCard = idTarjeta;
                                Rspsta_Escribir_Tarjeta_LECTOR resp4 = lectora.EscribirTarjeta(myTarjeta, false, false);
                                if (resp4.TarjetaEscrita)
                                {
                                    Thread.Sleep(2000);
                                    Rspsta_Leer_Tarjeta_LECTOR respFinal = lectora.LeerTarjeta(TYPE_STRUCTURE_SMARTCARD.SMARTCARD_PARKING_V1, false);
                                    if (respFinal.TarjetaLeida)
                                    {
                                        oCardResponse.error = false;
                                        SMARTCARD_PARKING_V1 myTarjetaFinal = (SMARTCARD_PARKING_V1)respFinal.Tarjeta;
                                        oCardResponse.cicloActivo = myTarjetaFinal.ActiveCycle != null ? (bool)myTarjetaFinal.ActiveCycle : false;
                                        oCardResponse.codeAutorizacion1 = myTarjetaFinal.CodeAgreement1 != null ? (int)myTarjetaFinal.CodeAgreement1 : 0;
                                        oCardResponse.codeAutorizacion2 = myTarjetaFinal.CodeAgreement2 != null ? (int)myTarjetaFinal.CodeAgreement2 : 0;
                                        oCardResponse.codeAutorizacion3 = myTarjetaFinal.CodeAgreement3 != null ? (int)myTarjetaFinal.CodeAgreement3 : 0;
                                        oCardResponse.cortesia = myTarjetaFinal.Courtesy != null ? (bool)myTarjetaFinal.Courtesy : false;
                                        oCardResponse.fechEntrada = myTarjetaFinal.DateTimeEntrance.ToString();
                                        oCardResponse.moduloEntrada = myTarjetaFinal.EntranceModule;
                                        oCardResponse.placa = myTarjetaFinal.EntrancePlate;
                                        oCardResponse.fechaPago = myTarjetaFinal.PaymentDateTime.ToString();
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

        public string GetLocalMACAddress()
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
            //return "";
            return sMacAddress;
        }

        class temporalTicketPago
        {
            public double Cambio = 0;
            public string Direccion = string.Empty;
            public string Fecha = string.Empty;
            public string IdTransaccion = string.Empty;
            public string Informacion = string.Empty;
            public string Modulo = string.Empty;
            public string Nombre = string.Empty;
            public string NumeroFactura = string.Empty;
            public string Placa = string.Empty;
            public double Recibido = 0;
            public string Resolucion = string.Empty;
            public string Rut = string.Empty;
            public string Telefono = string.Empty;
            //public string TotalFinal = string.Empty;
            public double Total = 0;
            public double Subtotal = 0;
            public double Iva = 0;
            public string TipoPago = string.Empty;
            public string Fecha2 = string.Empty;
            public string Vehiculo = string.Empty;
        }


   
        public PrintTicketResponse PrintTicket()
        {
            PrintTicketResponse oPrintTicketResponse = new PrintTicketResponse();
            string datos = HttpContext.Current.Request.Params["datos"];
            dynamic stuff;
            try
            {
                List<temporalTicketPago> lstTemporal1 = new List<temporalTicketPago>();

                stuff = Json.Decode(datos);

                dynamic antes = stuff;
                foreach (var item in antes.Resultado)
                {
                    temporalTicketPago otemporalTicketPago = new temporalTicketPago();

                    otemporalTicketPago.Cambio = Convert.ToDouble(item.Cambio);
                    otemporalTicketPago.Direccion = item.Direccion;
                    otemporalTicketPago.Fecha = item.Fecha;
                    otemporalTicketPago.IdTransaccion = item.IdTransaccion;
                    otemporalTicketPago.Informacion = "Esta infromacion esta quemada en el codigo, deberia obtenerse de algun lugar";
                    otemporalTicketPago.Modulo = item.Modulo;
                    otemporalTicketPago.Nombre = item.Nombre;
                    otemporalTicketPago.NumeroFactura = item.NumeroFactura;
                    otemporalTicketPago.Placa = item.Placa;
                    otemporalTicketPago.Recibido = Convert.ToDouble(item.ValorRecibido);
                    otemporalTicketPago.Resolucion = item.NumeroResolucion;
                    otemporalTicketPago.Rut = "NIT 900.003.663-1";
                    otemporalTicketPago.Telefono = item.Telefono;
                    //otemporalTicketPago.TotalFinal = total;
                    otemporalTicketPago.Total = Convert.ToDouble(item.Total);
                    otemporalTicketPago.Subtotal = Convert.ToDouble(item.Subtotal);
                    otemporalTicketPago.Iva = Convert.ToDouble(item.Iva);
                    otemporalTicketPago.TipoPago = item.Tipo;
                    otemporalTicketPago.Fecha2 = item.FechaEntrada;
                    otemporalTicketPago.Vehiculo = Convert.ToString(item.TipoVehiculo).ToUpper();


                    lstTemporal1.Add(otemporalTicketPago);
                }

                List<string> numFacturas = new List<string>();
                foreach (temporalTicketPago item in lstTemporal1)
                {
                    bool find = false;
                    if (numFacturas.Count > 0)
                    {
                        foreach (string item2 in numFacturas)
                        {
                            if (item2 == item.NumeroFactura)
                            {
                                find = true;
                            }
                        }

                        if (!find)
                        {
                            numFacturas.Add(item.NumeroFactura);
                        }
                        find = false;
                    }
                    else
                    {
                        numFacturas.Add(item.NumeroFactura);
                    }
                }

                List<temporalTicketPago> finalFiltrada = new List<temporalTicketPago>();
                foreach (temporalTicketPago item in lstTemporal1)
                {
                    if (item.NumeroFactura == numFacturas[numFacturas.Count - 1])
                    {
                        finalFiltrada.Add(item);
                    }
                }



                ReportDataSource datasource = new ReportDataSource();
                LocalReport oLocalReport = new LocalReport();

                datasource = new ReportDataSource("DataSetTicketPago", (DataTable)GenerarTicketPago(finalFiltrada).Tables[0]);
                oLocalReport.ReportPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format(@"bin\Tickets\{0}.rdlc", "ticketPago"));


                oLocalReport.DataSources.Add(datasource);
                oLocalReport.Refresh();




                ///
                Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;
                string filenameExtension;
                byte[] bytes;
                bytes = oLocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);
                oLocalReport.Dispose();
                oLocalReport = null;
                oPrintTicketResponse.impresion = Convert.ToBase64String(bytes);

            }
            catch (Exception e)
            {
                oPrintTicketResponse.error = true;
                oPrintTicketResponse.errorMessage = e.InnerException + " " + e.Message;
            }
            return oPrintTicketResponse;
        }

       
        public PrintTicketResponse PrintTicketM()
        {
            PrintTicketResponse oPrintTicketResponse = new PrintTicketResponse();
            string datos = HttpContext.Current.Request.Params["datos"];
            dynamic stuff;
            try
            {
                stuff = Json.Decode(datos);




                ReportDataSource datasource = new ReportDataSource();
                LocalReport oLocalReport = new LocalReport();

                datasource = new ReportDataSource("DataSetTicketPago", (DataTable)GenerarTicketPagoM(stuff).Tables[0]);
                oLocalReport.ReportPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format(@"bin\Tickets\{0}.rdlc", "ticketPagoM"));


                oLocalReport.DataSources.Add(datasource);
                oLocalReport.Refresh();




                ///
                Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;
                string filenameExtension;
                byte[] bytes;
                bytes = oLocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);
                oLocalReport.Dispose();
                oLocalReport = null;
                oPrintTicketResponse.impresion = Convert.ToBase64String(bytes);

            }
            catch (Exception e)
            {
                oPrintTicketResponse.error = true;
                oPrintTicketResponse.errorMessage = e.InnerException + " " + e.Message;
            }
            return oPrintTicketResponse;
        }

        public PrintTicketResponse PrintTicketArqueo()
        {
            PrintTicketResponse oPrintTicketResponse = new PrintTicketResponse();
            string datos = HttpContext.Current.Request.Params["datos"];

            dynamic stuff = Json.Decode(datos);

            try
            {
                foreach (var item in stuff.Resultado)
                {
                    ReportDataSource datasource = new ReportDataSource();
                    LocalReport oLocalReport = new LocalReport();

                    datasource = new ReportDataSource("DataSetTicketArqueo", (DataTable)GenerarTicketArqueo(item).Tables[0]);
                    oLocalReport.ReportPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format(@"bin\Tickets\{0}.rdlc", "ticketArqueo"));


                    oLocalReport.DataSources.Add(datasource);
                    oLocalReport.Refresh();

                    ///
                    Warning[] warnings;
                    string[] streamids;
                    string mimeType;
                    string encoding;
                    string filenameExtension;
                    byte[] bytes;
                    bytes = oLocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);
                    oLocalReport.Dispose();
                    oLocalReport = null;
                    oPrintTicketResponse.impresion = Convert.ToBase64String(bytes);

                }
                oPrintTicketResponse.error = false;

            }
            catch (Exception e)
            {
                oPrintTicketResponse.error = true;
                oPrintTicketResponse.errorMessage = e.InnerException + " " + e.Message;
            }

            return oPrintTicketResponse;
        }

        public PrintTicketResponse PrintTicketCarga()
        {
            PrintTicketResponse oPrintTicketResponse = new PrintTicketResponse();
            string datos = HttpContext.Current.Request.Params["datos"];

            dynamic stuff = Json.Decode(datos);

            try
            {
                foreach (var item in stuff.Resultado)
                {

                    ReportDataSource datasource = new ReportDataSource();
                    LocalReport oLocalReport = new LocalReport();

                    datasource = new ReportDataSource("DataSetTicketCarga", (DataTable)GenerarTicketCarga(item).Tables[0]);
                    oLocalReport.ReportPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, string.Format(@"bin\Tickets\{0}.rdlc", "ticketCarga"));


                    oLocalReport.DataSources.Add(datasource);
                    oLocalReport.Refresh();

                    ///
                    Warning[] warnings;
                    string[] streamids;
                    string mimeType;
                    string encoding;
                    string filenameExtension;
                    byte[] bytes;
                    bytes = oLocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);
                    oLocalReport.Dispose();
                    oLocalReport = null;
                    oPrintTicketResponse.impresion = Convert.ToBase64String(bytes);
                }
            }
            catch (Exception e)
            {
                oPrintTicketResponse.error = true;
                oPrintTicketResponse.errorMessage = e.InnerException + " " + e.Message;
            }
            return oPrintTicketResponse;
        }

        private DataSetTicketPago GenerarTicketPago(List<temporalTicketPago> infoTicket)
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

                rowDatosFactura.Cambio = item.Cambio;
                rowDatosFactura.Direccion = item.Direccion;
                rowDatosFactura.Fecha = item.Fecha;
                rowDatosFactura.IdTransaccion = item.IdTransaccion;
                rowDatosFactura.Informacion = "Esta infromacion esta quemada en el codigo, deberia obtenerse de algun lugar";
                rowDatosFactura.Modulo = item.Modulo;
                rowDatosFactura.Nombre = item.Nombre;
                rowDatosFactura.NumeroFactura = item.NumeroFactura;
                rowDatosFactura.Placa = item.Placa;
                rowDatosFactura.Recibido = item.Recibido;
                rowDatosFactura.Resolucion = item.Resolucion;
                rowDatosFactura.Rut = "NIT 900.003.663-1";
                rowDatosFactura.Telefono = item.Telefono;
                rowDatosFactura.TotalFinal = total;
                rowDatosFactura.Total = item.Total;
                rowDatosFactura.Subtotal = item.Subtotal;
                rowDatosFactura.Iva = item.Iva;
                rowDatosFactura.TipoPago = item.TipoPago;
                rowDatosFactura.Fecha2 = item.Fecha2;
                rowDatosFactura.Vehiculo = item.Vehiculo;


                facturacion.TablaTicketPago.AddTablaTicketPagoRow(rowDatosFactura);
            }

            return facturacion;
        }

        private DataSetTicketArqueo GenerarTicketArqueo(dynamic infoTicket)
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


            facturacion.TablaTicketArqueo.AddTablaTicketArqueoRow(rowDatosFactura);

            return facturacion;
        }

        private DataSetTicketCarga GenerarTicketCarga(dynamic infoTicket)
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

        private DataSetTicketPagoM GenerarTicketPagoM(dynamic infoTicket)
        {
            DataSetTicketPagoM facturacion = new DataSetTicketPagoM();

            double total = 0;
            foreach (var item in infoTicket.Resultado)
            {
                total += Convert.ToDouble(item.Total);
            }

            foreach (var item in infoTicket.Resultado)
            {
                DataSetTicketPagoM.TablaTicketPagoRow rowDatosFactura = facturacion.TablaTicketPago.NewTablaTicketPagoRow();

                rowDatosFactura.Direccion = item.Direccion;
                rowDatosFactura.Fecha = item.Fecha;
                rowDatosFactura.IdTransaccion = item.IdTransaccion;
                rowDatosFactura.Informacion = "Esta infromacion esta quemada en el codigo, deberia obtenerse de algun lugar";
                rowDatosFactura.Modulo = item.Modulo;
                rowDatosFactura.Nombre = item.Nombre;
                rowDatosFactura.NumeroFactura = item.NumeroFactura;
                rowDatosFactura.Resolucion = item.NumeroResolucion;
                rowDatosFactura.Rut = "NIT 900554696-8";
                rowDatosFactura.Telefono = item.Telefono;
                rowDatosFactura.TotalFinal = total;
                rowDatosFactura.Total = Convert.ToDouble(item.Total);
                rowDatosFactura.Subtotal = Convert.ToDouble(item.Subtotal);
                rowDatosFactura.Iva = Convert.ToDouble(item.Iva);
                rowDatosFactura.TipoPago = item.Tipo;
                rowDatosFactura.NombreAutorizacion = item.NombreAutorizacion;
                rowDatosFactura.Documento = item.Documento;


                facturacion.TablaTicketPago.AddTablaTicketPagoRow(rowDatosFactura);
            }

            return facturacion;
        }
    }

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

        public string fechEntrada { get; set; }
        public string fechaPago { get; set; }

        public string moduloEntrada { get; set; }
        public string moduloPago { get; set; }
        public string placa { get; set; }
        public string tipoTarjeta { get; set; }
        public string tipoVehiculo { set; get; }
    }

    public class PrintTicketResponse
    {
        public bool error { set; get; }
        public string errorMessage { set; get; }
        public string impresion { set; get; }
    }
}
