
using BlockAndPass.WebService.LiquidacionWR;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Web;
using System.Web.Services;

namespace BlockAndPass.WebService
{
    /// <summary>
    /// Descripción breve de Service1
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    // [System.Web.Script.Services.ScriptService]
    public class ServicesByP : System.Web.Services.WebService
    {

        public ServicesByP()
        {

        }


        [WebMethod]
        public InfoUsuarioResponse ObtenerInformacionUsuario(string sUser)
        {
            ArrayList array = new ArrayList();
            InfoUsuarioResponse oInfoUsuarioResponse = new InfoUsuarioResponse();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;

            query = "select Documento, Nombres, Apellidos, Usuario, Cargo from t_usuarios where documento='" + sUser + "'";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Check is the reader has any rows at all before starting to read.
                        if (reader.HasRows)
                        {
                            // Read advances to the next row.
                            while (reader.Read())
                            {

                                oInfoUsuarioResponse.Documento = reader[0].ToString();
                                oInfoUsuarioResponse.Nombres = reader[1].ToString() + " " + reader[2].ToString();
                                oInfoUsuarioResponse.Usuario = reader[3].ToString();
                                oInfoUsuarioResponse.Cargo = reader[4].ToString();

                            }
                        }
                    }
                }
            }

            if (oInfoUsuarioResponse.Documento != string.Empty)
            {
                oInfoUsuarioResponse.Exito = true;
            }
            else
            {
                oInfoUsuarioResponse.Exito = false;
                oInfoUsuarioResponse.ErrorMessage = "No se encontraron cuentas asociadas al usuario.";
            }

            return oInfoUsuarioResponse;
        }

        [WebMethod]
        public LoginResponse Loguearse(string a)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            LoginResponse response = new LoginResponse();
            string query = string.Empty;

            query = "select documento, contraseña from t_usuarios where usuario='" + a + "' and Estado = 'true'";

            string documento = string.Empty;
            string clave = string.Empty;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Check is the reader has any rows at all before starting to read.
                        if (reader.HasRows)
                        {
                            // Read advances to the next row.
                            while (reader.Read())
                            {
                                response.Documento = reader["documento"].ToString();
                                response.Clave = reader["contraseña"].ToString();
                            }
                        }
                    }
                }
            }

            if (response.Documento != string.Empty)
            {
                response.Exito = true;
            }
            else
            {
                response.Exito = false;
                response.ErrorMessage = "No se encontraron cuentas asociadas al usuario.";
            }

            return response;
        }

        [WebMethod]
        public InfoPPMService ObtenerDatosPPMxMac(string sMac)
        {
            InfoPPMService oInfoPPMService = new InfoPPMService();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;

            query = "select top (1) IdModulo as NombreModulo, e.Nombre as NombreEstacionamiento, e.IdEstacionamiento, s.nombre as NombreSede, s.IdSede"
                    + " from T_Configuracion as c"
                    + " inner join T_Estacionamientos as e"
                    + " on c.IdEstacionamiento=e.IdEstacionamiento "
                    + " inner join T_Sedes as s"
                    + " on s.IdSede=e.IdSede"
                    + " where mac='" + sMac + "'";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Check is the reader has any rows at all before starting to read.
                        if (reader.HasRows)
                        {
                            // Read advances to the next row.
                            while (reader.Read())
                            {
                                oInfoPPMService.Modulo = reader[0].ToString();
                                oInfoPPMService.Estacionamiento = reader[1].ToString();
                                oInfoPPMService.IdEstacionamiento = reader[2].ToString();
                                oInfoPPMService.Sede = reader[3].ToString();
                                oInfoPPMService.IdSede = reader[4].ToString();
                            }
                        }
                        else
                        {
                            oInfoPPMService.Exito = false;
                            oInfoPPMService.ErrorMessage = "No encontro registro.";
                        }
                    }
                }
            }

            return oInfoPPMService;
        }

        [WebMethod]
        public string ObtenerValorParametroxNombre(string sNombre, string sIdEstacionamiento)
        {
            string valor = string.Empty;
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            string query = string.Empty;

            query = "select valor from T_Parametros where Codigo='" + sNombre + "' and idEstacionamiento='" + sIdEstacionamiento + "'";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Check is the reader has any rows at all before starting to read.
                        if (reader.HasRows)
                        {
                            // Read advances to the next row.
                            while (reader.Read())
                            {
                                valor = reader[0].ToString();
                            }
                        }
                    }
                }
            }

            return valor;
        }

        [WebMethod]
        public InfoTransaccionService ConsultarInfoTransaccion(string idEstacionamiento, string idTarjeta, string moduloEntrada)
        {
            InfoTransaccionService oInfoTransaccionService = new InfoTransaccionService();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;

            query = "select top (1) IdTransaccion, FechaEntrada from T_Transacciones where ModuloEntrada='" + moduloEntrada + "' and idestacionamiento='" + idEstacionamiento + "' and idtarjeta='" + idTarjeta + "' order by FechaEntrada desc";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Check is the reader has any rows at all before starting to read.
                        if (reader.HasRows)
                        {
                            // Read advances to the next row.
                            while (reader.Read())
                            {
                                oInfoTransaccionService.IdTransaccion = reader[0].ToString();
                                oInfoTransaccionService.HoraTransaccion = reader[1].ToString();
                            }
                        }
                        else
                        {
                            oInfoTransaccionService.Exito = false;
                            oInfoTransaccionService.ErrorMessage = "No encontro registro.";
                        }
                    }
                }
            }

            return oInfoTransaccionService;
        }

        [WebMethod]
        public InfoTransaccionService ConsultarCascosxId(string idTransaccion)
        {
            List<InfoItemsTransaccionService> lstItemsTransac = new List<InfoItemsTransaccionService>();
            InfoTransaccionService oInfoTransaccionService = new InfoTransaccionService();

            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;

            query = "select dbo.T_Transacciones.IdTransaccion, dbo.T_Transacciones.FechaEntrada, dbo.T_Cascos.Casillero  FROM dbo.T_Transacciones INNER JOIN  dbo.T_Cascos ON dbo.T_Transacciones.IdTransaccion = dbo.T_Cascos.IdTransaccion where dbo.T_Transacciones.IdTransaccion='" + idTransaccion + "'";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Check is the reader has any rows at all before starting to read.
                        if (reader.HasRows)
                        {
                            // Read advances to the next row.
                            while (reader.Read())
                            {
                                InfoItemsTransaccionService oInfoItemsTransaccionService = new InfoItemsTransaccionService();
                                oInfoItemsTransaccionService.Casillero = reader[2].ToString();
                                lstItemsTransac.Add(oInfoItemsTransaccionService);
                            }
                        }
                        else
                        {
                            oInfoTransaccionService.Exito = false;
                            oInfoTransaccionService.ErrorMessage = "No encontro registro.";
                        }
                    }
                }
            }

            if (lstItemsTransac.Count > 0)
            {
                oInfoTransaccionService.LstTransac = lstItemsTransac;
            }
            else
            {
                oInfoTransaccionService.Exito = false;
                oInfoTransaccionService.ErrorMessage = "No encuentra informacion facturacion.";
            }

            return oInfoTransaccionService;
        }

        [WebMethod]
        public InfoTransaccionService ConsultarInfoTransaccionxId(string idTransaccion)
        {
            InfoTransaccionService oInfoTransaccionService = new InfoTransaccionService();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;

            query = "select top (1) IdTransaccion, FechaEntrada from T_Transacciones where IdTransaccion='" + idTransaccion + "'";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Check is the reader has any rows at all before starting to read.
                        if (reader.HasRows)
                        {
                            // Read advances to the next row.
                            while (reader.Read())
                            {
                                oInfoTransaccionService.IdTransaccion = reader[0].ToString();
                                oInfoTransaccionService.HoraTransaccion = reader[1].ToString();
                            }
                        }
                        else
                        {
                            oInfoTransaccionService.Exito = false;
                            oInfoTransaccionService.ErrorMessage = "No encontro registro.";
                        }
                    }
                }
            }

            //string path = "c://log.txt" ;
            //string createText = oInfoTransaccionService.IdTransaccion + Environment.NewLine + oInfoTransaccionService.HoraTransaccion;
            //File.WriteAllText(path, createText);

            //// Open the file to read from.
            //string readText = File.ReadAllText(path);

            return oInfoTransaccionService;
        }

        [WebMethod]
        public LiquidacionService ConsultarValorPagar(bool mensualidad, bool repo, int tipoVehiculo, string idTransaccion, string idTarjeta)
        {
            LiquidacionService oLiquidacionService = new LiquidacionService();
            List<DatosLiquidacionService> lstLiquidacion = new List<DatosLiquidacionService>();

            try
            {

                string url = ConfigurationManager.AppSettings["URLLiquidar"].ToString();

                LiquidacionServiceClient miCLiente = new LiquidacionServiceClient();
                miCLiente.Endpoint.Address = new EndpointAddress(url);


                Liquidacion_Request request = new Liquidacion_Request();
                request.bMensualidad = mensualidad;
                request.bReposicion = repo;
                request.iTipoVehiculo = Convert.ToInt32(tipoVehiculo);
                request.sSecuencia = idTransaccion;
                request.sIdtarjeta = idTarjeta;

                Liquidacion_Response responseWS = miCLiente.getDatosLiquidacion(request);
                if (responseWS.olstDtoLiquidacion != null)
                {
                    foreach (ServiceDtoDatosLiquidacion item in responseWS.olstDtoLiquidacion)
                    {

                        DatosLiquidacionService oDatosLiquidacionService = new DatosLiquidacionService();
                        oDatosLiquidacionService.Tipo = item.Tipo;
                        oDatosLiquidacionService.SubTotal = item.SubTotal;
                        oDatosLiquidacionService.Iva = item.Iva;
                        oDatosLiquidacionService.Total = item.Total;

                        lstLiquidacion.Add(oDatosLiquidacionService);
                    }

                    oLiquidacionService.Exito = true;
                    oLiquidacionService.LstLiquidacion = lstLiquidacion;
                }
                else
                {
                    oLiquidacionService.Exito = false;
                    oLiquidacionService.ErrorMessage = "No se encontraron items de liquidacion";
                }
            }
            catch (Exception e)
            {
                oLiquidacionService.Exito = false;
                oLiquidacionService.ErrorMessage = e.InnerException + " " + e.Message;
            }


            return oLiquidacionService;
        }

        [WebMethod]
        public InfoPagoNormalService PagarClienteParticular(string pagosstring, string idEstacionamiento, string idTransaccion, string idModulo, string fecha, string total)
        {
            ArrayList pagosFinal = new ArrayList();

            if (fecha.Split(' ').Length > 2)
            {
                if (fecha.Split(' ')[2][0] == 'p')
                {
                    int horaFinal = Convert.ToInt32(fecha.Split(' ')[1].Split(':')[0]);
                    if (Convert.ToInt32(fecha.Split(' ')[1].Split(':')[0]) != 12)
                    {
                        horaFinal = Convert.ToInt32(fecha.Split(' ')[1].Split(':')[0]) + 12;
                    }

                    fecha = fecha.Split(' ')[0] + " " + horaFinal + ":" + fecha.Split(' ')[1].Split(':')[1] + ":" + fecha.Split(' ')[1].Split(':')[2];
                }
                else
                {
                    int horaFinal = Convert.ToInt32(fecha.Split(' ')[1].Split(':')[0]);
                    if (Convert.ToInt32(fecha.Split(' ')[1].Split(':')[0]) != 12)
                    {
                        horaFinal = Convert.ToInt32(fecha.Split(' ')[1].Split(':')[0]) - 12;
                    }
                    fecha = fecha.Split(' ')[0] + " " + fecha.Split(' ')[1];
                }
            }
            else
            {
                fecha = fecha.Split(' ')[0] + " " + fecha.Split(' ')[1];
            }


            string[] pagos = pagosstring.Split(',');

            InfoPagoNormalService oInfoPagoService = new InfoPagoNormalService();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;

            query = "select top (1) IdFacturacion, FacturaActual from T_Facturacion where IdModulo = '" + idModulo + "' and IdEstacionamiento='" + idEstacionamiento + "'";

            string facturaActual = string.Empty;
            string idFacturacion = string.Empty;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Check is the reader has any rows at all before starting to read.
                        if (reader.HasRows)
                        {
                            // Read advances to the next row.
                            while (reader.Read())
                            {
                                idFacturacion = reader[0].ToString();
                                facturaActual = reader[1].ToString();
                            }
                        }
                    }
                }
            }

            if (idFacturacion != string.Empty && facturaActual != string.Empty)
            {
                for (int i = 0; i < pagos.Length; i++)
                {
                    string[] temporalPagos = pagos[i].Split('-');
                    pagosFinal.Add(new string[] { temporalPagos[0], temporalPagos[1], temporalPagos[2], temporalPagos[3], (Convert.ToInt32(facturaActual)).ToString() });
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand command = connection.CreateCommand();
                    SqlTransaction transaction;

                    // Start a local transaction.
                    transaction = connection.BeginTransaction("PayTransaction");

                    // Must assign both transaction object and connection 
                    // to Command object for a pending local transaction
                    command.Connection = connection;
                    command.Transaction = transaction;

                    try
                    {
                        command.CommandText =
                            "update T_Facturacion set facturaActual = " + Convert.ToInt32(Convert.ToInt32(facturaActual) + 1) + " where idfacturacion = " + idFacturacion;
                        command.ExecuteNonQuery();

                        command.CommandText =
                            "update T_Cascos set Estado = 0 where IdTransaccion = '" + idTransaccion + "'";
                        command.ExecuteNonQuery();


                        foreach (string[] item in pagosFinal)
                        {
                            command.CommandText =
                                "Insert into T_Pagos (IdTransaccion, IdEstacionamiento, IdModulo, IdFacturacion, IdTipoPago, FechaPago, Subtotal, Iva, Total, NumeroFactura) VALUES "
                                + "('" + idTransaccion + "', '" + idEstacionamiento + "', '" + idModulo + "', '" + idFacturacion + "', '" + item[0] + "', convert(datetime,'" + fecha + "',103), '" + item[1] + "', '" + item[2] + "', '" + item[3] + "', '" + item[4] + "')";
                            command.ExecuteNonQuery();
                        }

                        command.CommandText =
                            "Insert into T_Movimientos (IdTransaccion, IdEstacionamiento, IdModulo, Parte, Accion, Denominacion, Cantidad, Valor, FechaMovimiento) VALUES "
                            + "('" + idTransaccion + "','" + idEstacionamiento + "','" + idModulo + "','CM','Entrada','" + total + "','1','" + total + "',GETDATE())";
                        command.ExecuteNonQuery();

                        command.CommandText =
                            "update T_Partes set DineroActual=(select DineroActual from T_Partes where IdModulo='" + idModulo + "' and IdEstacionamiento='" + idEstacionamiento + "' and NombreParte='CM')+" + total + ", sincronizacion = 0 where IdModulo='" + idModulo + "' and IdEstacionamiento='" + idEstacionamiento + "' and NombreParte='CM'";
                        command.ExecuteNonQuery();

                        command.CommandText =
                            "update T_Transacciones set ValorRecibido = '" + total + "',Cambio=0 where idTransaccion='" + idTransaccion + "'";
                        command.ExecuteNonQuery();

                        // Attempt to commit the transaction.
                        transaction.Commit();

                    }
                    catch (Exception ex)
                    {
                        oInfoPagoService.Exito = false;
                        oInfoPagoService.ErrorMessage = "EXCEPCION BD No guarda modificaciones en BD: " + ex.Data + "//////////" + ex.Message + " - " + idTransaccion + "////" + ex.StackTrace;//+ " - " + fechaAntes;
                        // Attempt to roll back the transaction. 
                        try
                        {
                            transaction.Rollback();
                        }
                        catch (Exception ex2)
                        {
                            //No rollback
                        }
                    }
                }
            }
            else
            {
                oInfoPagoService.Exito = false;
                oInfoPagoService.ErrorMessage = "No encontro facturas disponibles." + query;
            }

            //string path = "c://log.txt";
            //string createText = query;
            //File.WriteAllText(path, createText);

            //// Open the file to read from.
            //string readText = File.ReadAllText(path);

            return oInfoPagoService;
        }

        [WebMethod]
        public InfoFacturaResponse ObtenerDatosFactura(string idTransaccion)
        {
            List<InfoItemsFacturaResponse> lstItemsFactura = new List<InfoItemsFacturaResponse>();
            InfoFacturaResponse oInfoFacturaResponse = new InfoFacturaResponse();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;

            query = "select f.Prefijo + '-' + p.NumeroFactura, e.Nombre, e.TelefonoContacto, e.Direccion, c.IdModulo, p.FechaPago, p.IdTransaccion, t.PlacaEntrada, tp.TipoPago, p.Total, t.ValorRecibido, t.Cambio, p.Subtotal, p.Iva, f.NumeroResolucion + ' ' + f.FechaResolucion + ' DEL ' + f.FacturaInicial + ' AL ' + f.FacturaFinal , t.FechaEntrada, tv.TipoVehiculo,"
                    + " (select count(cantidad) from (select count(*) as cantidad"
                                + " from T_Pagos"
                                + " where IdTransaccion='" + idTransaccion + "'"
                                + " group by(NumeroFactura)) as myTable), f.FechaFinResolucion"
                    + " from T_Pagos as p"
                    + " inner join T_Estacionamientos as e"
                    + " on p.IdEstacionamiento=e.IdEstacionamiento"
                    + " inner join T_Configuracion as c"
                    + " on p.IdModulo=c.IdModulo"
                    + " inner join T_Transacciones as t"
                    + " on p.IdTransaccion = t.IdTransaccion"
                    + " inner join T_TipoPago as tp"
                    + " on p.IdTipoPago=tp.IdTipoPago"
                    + " inner join T_Facturacion as f"
                    + " on p.IdFacturacion = f.IdFacturacion"
                    + " inner join T_TipoVehiculo as tv"
                    + " on tv.IdTipoVehiculo = t.IdTipoVehiculo"
                    + " where p.IdTransaccion='" + idTransaccion + "'"
                    + " order by p.NumeroFactura asc";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Check is the reader has any rows at all before starting to read.
                        if (reader.HasRows)
                        {
                            // Read advances to the next row.
                            while (reader.Read())
                            {
                                InfoItemsFacturaResponse oInfoItemsFacturaResponse = new InfoItemsFacturaResponse();
                                oInfoItemsFacturaResponse.NumeroFactura = reader[0].ToString();
                                oInfoItemsFacturaResponse.Nombre = reader[1].ToString();
                                oInfoItemsFacturaResponse.Telefono = reader[2].ToString();
                                oInfoItemsFacturaResponse.Direccion = reader[3].ToString();
                                oInfoItemsFacturaResponse.Modulo = reader[4].ToString();
                                oInfoItemsFacturaResponse.Fecha = reader[5].ToString();
                                oInfoItemsFacturaResponse.IdTransaccion = reader[6].ToString();
                                oInfoItemsFacturaResponse.Placa = reader[7].ToString();
                                oInfoItemsFacturaResponse.Tipo = reader[8].ToString();
                                oInfoItemsFacturaResponse.Total = reader[9].ToString();
                                oInfoItemsFacturaResponse.ValorRecibido = reader[10].ToString();
                                oInfoItemsFacturaResponse.Cambio = reader[11].ToString();
                                oInfoItemsFacturaResponse.Subtotal = reader[12].ToString();
                                oInfoItemsFacturaResponse.Iva = reader[13].ToString();
                                oInfoItemsFacturaResponse.NumeroResolucion = reader[14].ToString();
                                oInfoItemsFacturaResponse.FechaEntrada = reader[15].ToString();
                                oInfoItemsFacturaResponse.TipoVehiculo = reader[16].ToString();
                                oInfoItemsFacturaResponse.Cantidad = reader[17].ToString();
                                oInfoItemsFacturaResponse.Vigencia = reader[18].ToString();

                                lstItemsFactura.Add(oInfoItemsFacturaResponse);
                            }
                        }
                    }
                }
            }

            if (lstItemsFactura.Count > 0)
            {
                oInfoFacturaResponse.LstItems = lstItemsFactura;
            }
            else
            {
                oInfoFacturaResponse.Exito = false;
                oInfoFacturaResponse.ErrorMessage = "No encuentra informacion facturacion.";
            }

            return oInfoFacturaResponse;
        }

        [WebMethod]
        public InfoPagoMensualidadService PagarMensualidad(string pagosstring, string idEstacionamiento, string idModulo, string fecha, string total, string idTarjeta)
        {

            ArrayList pagosFinal = new ArrayList();

            fecha = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");


            string[] pagos = pagosstring.Split(',');

            string response = string.Empty;
            InfoPagoMensualidadService oInfoPagoMensualidadService = new InfoPagoMensualidadService();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;

            query = "select top (1) IdFacturacion, FacturaActual from T_Facturacion where IdModulo = '" + idModulo + "' and IdEstacionamiento='" + idEstacionamiento + "'";

            string facturaActual = string.Empty;
            string idFacturacion = string.Empty;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Check is the reader has any rows at all before starting to read.
                        if (reader.HasRows)
                        {
                            // Read advances to the next row.
                            while (reader.Read())
                            {
                                idFacturacion = reader[0].ToString();
                                facturaActual = reader[1].ToString();
                            }
                        }
                    }
                }
            }

            query = "select top (1) Documento, IdAutorizacion from T_PersonasAutorizadas where IdTarjeta='" + idTarjeta + "'";

            string documentoAutorizado = string.Empty;
            string idAutorizacion = string.Empty;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Check is the reader has any rows at all before starting to read.
                        if (reader.HasRows)
                        {
                            // Read advances to the next row.
                            while (reader.Read())
                            {
                                documentoAutorizado = reader[0].ToString();
                                idAutorizacion = reader[1].ToString();
                            }
                        }
                    }
                }
            }

            if (idFacturacion != string.Empty && facturaActual != string.Empty && documentoAutorizado != string.Empty && idAutorizacion != string.Empty)
            {
                for (int i = 0; i < pagos.Length; i++)
                {
                    string[] temporalPagos = pagos[i].Split('-');
                    pagosFinal.Add(new string[] { temporalPagos[0], temporalPagos[1], temporalPagos[2], temporalPagos[3], (Convert.ToInt32(facturaActual)).ToString() });
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    SqlCommand command = connection.CreateCommand();
                    SqlTransaction transaction;

                    // Start a local transaction.
                    transaction = connection.BeginTransaction("PayTransaction");

                    // Must assign both transaction object and connection 
                    // to Command object for a pending local transaction
                    command.Connection = connection;
                    command.Transaction = transaction;

                    try
                    {
                        command.CommandText =
                            "update T_Facturacion set facturaActual = " + Convert.ToInt32(Convert.ToInt32(facturaActual) + 1) + " where idfacturacion = " + idFacturacion;
                        command.ExecuteNonQuery();



                        foreach (string[] item in pagosFinal)
                        {
                            command.CommandText =
                                "Insert into T_Pagos (IdTransaccion, IdEstacionamiento, IdModulo, IdFacturacion, IdTipoPago, FechaPago, Subtotal, Iva, Total, NumeroFactura, IdAutorizado) VALUES "
                                + "('" + documentoAutorizado + "', '" + idEstacionamiento + "', '" + idModulo + "', '" + idFacturacion + "', '" + item[0] + "', getdate(), '" + item[1] + "', '" + item[2] + "', '" + item[3] + "', '" + item[4] + "','" + idAutorizacion + "')";
                            command.ExecuteNonQuery();
                        }

                        command.CommandText =
                            "Insert into T_Movimientos (IdTransaccion, IdEstacionamiento, IdModulo, Parte, Accion, Denominacion, Cantidad, Valor, FechaMovimiento) VALUES "
                            + "('" + documentoAutorizado + "','" + idEstacionamiento + "','" + idModulo + "','CM','Entrada','" + total + "','1','" + total + "',GETDATE())";
                        command.ExecuteNonQuery();

                        command.CommandText =
                            "update T_Partes set DineroActual=(select DineroActual from T_Partes where IdModulo='" + idModulo + "' and IdEstacionamiento='" + idEstacionamiento + "' and NombreParte='CM')+" + total + ", sincronizacion = 0  where IdModulo='" + idModulo + "' and IdEstacionamiento='" + idEstacionamiento + "' and NombreParte='CM'";
                        command.ExecuteNonQuery();

                        // Attempt to commit the transaction.
                        transaction.Commit();

                    }
                    catch (Exception ex)
                    {
                        oInfoPagoMensualidadService.Exito = false;
                        oInfoPagoMensualidadService.ErrorMessage = "EXCEPCION BD No guarda modificaciones en BD: " + ex.InnerException + "//////////" + ex.Message + " - " + documentoAutorizado + "////" + ex.Source;
                        // Attempt to roll back the transaction. 
                        try
                        {
                            transaction.Rollback();
                        }
                        catch (Exception ex2)
                        {
                            //No rollback
                        }
                    }
                }

            }
            else
            {
                oInfoPagoMensualidadService.Exito = false;
                oInfoPagoMensualidadService.ErrorMessage = "No encontro facturas disponibles o autorizaciones asociadas a ese idTarjeta";
            }

            oInfoPagoMensualidadService.IdTranaccion = documentoAutorizado;
            oInfoPagoMensualidadService.IdAutorizacion = idAutorizacion;

            return oInfoPagoMensualidadService;
        }

        [WebMethod]
        public InfoFacturaResponse ObtenerDatosFacturaMensualidad(string idTransaccion, string idAutorizacion)
        {
            List<InfoItemsFacturaMensualidadResponse> lstItemsFactura = new List<InfoItemsFacturaMensualidadResponse>();
            InfoFacturaResponse oInfoFacturaResponse = new InfoFacturaResponse();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;
            try
            {
                query = "select top(1) f.Prefijo + '-' + p.NumeroFactura, e.Nombre, e.TelefonoContacto, e.Direccion, c.IdModulo, p.FechaPago, p.IdTransaccion, tp.TipoPago, p.Total, p.Subtotal, p.Iva, f.NumeroResolucion + ' ' + f.FechaResolucion + ' DEL ' + f.FacturaInicial + ' AL ' + f.FacturaFinal, au.NombreAutorizacion, pa.Documento, f.FechaFinResolucion , pa.Nit , pa.NombreEmpresa"
                         + " from T_Pagos as p"
                         + " inner join T_Estacionamientos as e"
                         + " on p.IdEstacionamiento=e.IdEstacionamiento"
                         + " inner join T_Configuracion as c"
                         + " on p.IdModulo=c.IdModulo"
                         + " inner join T_TipoPago as tp"
                         + " on p.IdTipoPago=tp.IdTipoPago"
                         + " inner join T_Facturacion as f"
                         + " on p.IdFacturacion = f.IdFacturacion"
                         + " inner join T_Autorizaciones as au"
                         + " on au.IdAutorizacion=p.IdAutorizado"
                         + " inner join T_PersonasAutorizadas as pa"
                         + " on pa.Documento=p.IdTransaccion"
                         + " where p.IdTransaccion='" + idTransaccion + "' and p.IdAutorizado='" + idAutorizacion + "'"
                         + " order by FechaPago desc";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Check is the reader has any rows at all before starting to read.
                            if (reader.HasRows)
                            {
                                // Read advances to the next row.
                                while (reader.Read())
                                {
                                    InfoItemsFacturaMensualidadResponse oInfoItemsFacturaMensualidadResponse = new InfoItemsFacturaMensualidadResponse();

                                    oInfoItemsFacturaMensualidadResponse.NumeroFactura = reader[0].ToString();
                                    oInfoItemsFacturaMensualidadResponse.Nombre = reader[1].ToString();
                                    oInfoItemsFacturaMensualidadResponse.Telefono = reader[2].ToString();
                                    oInfoItemsFacturaMensualidadResponse.Direccion = reader[3].ToString();
                                    oInfoItemsFacturaMensualidadResponse.Modulo = reader[4].ToString();
                                    oInfoItemsFacturaMensualidadResponse.Fecha = reader[5].ToString();
                                    oInfoItemsFacturaMensualidadResponse.IdTransaccion = reader[6].ToString();
                                    oInfoItemsFacturaMensualidadResponse.Tipo = reader[7].ToString();
                                    oInfoItemsFacturaMensualidadResponse.Total = reader[8].ToString();
                                    oInfoItemsFacturaMensualidadResponse.Subtotal = reader[9].ToString();
                                    oInfoItemsFacturaMensualidadResponse.Iva = reader[10].ToString();
                                    oInfoItemsFacturaMensualidadResponse.NumeroResolucion = reader[11].ToString();
                                    oInfoItemsFacturaMensualidadResponse.NombreAutorizacion = reader[12].ToString();
                                    oInfoItemsFacturaMensualidadResponse.Documento = reader[13].ToString();
                                    oInfoItemsFacturaMensualidadResponse.Vigencia = reader[14].ToString();

                                    //   new fields
                                    oInfoItemsFacturaMensualidadResponse.NombreEmpresa = reader[15].ToString();
                                    oInfoItemsFacturaMensualidadResponse.Nit = reader[16].ToString();

                                    lstItemsFactura.Add(oInfoItemsFacturaMensualidadResponse);
                                }
                            }
                        }
                    }
                }

                if (lstItemsFactura.Count > 0)
                {
                    oInfoFacturaResponse.LstItemsMensualidad = lstItemsFactura;
                }
                else
                {
                    oInfoFacturaResponse.Exito = false;
                    oInfoFacturaResponse.ErrorMessage = "No encuentra informacion facturacion.; " + query;
                }
            }
            catch (Exception e)
            {
                oInfoFacturaResponse.Exito = false;
                oInfoFacturaResponse.ErrorMessage = "Exception; .; " + e.InnerException + " " + e.Message;
            }

            return oInfoFacturaResponse;
        }

        [WebMethod]
        public AplicarMotoResponse AplicarEtiquetaMoto(string idEstacionamiento, string idTarjeta, string moduloEntrada)
        {
            string response = string.Empty;
            AplicarMotoResponse oAplicarMotoResponse = new AplicarMotoResponse();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;

            query = "select top (1) IdTransaccion from T_Transacciones where ModuloEntrada='" + moduloEntrada + "' and idestacionamiento='" + idEstacionamiento + "' and idtarjeta='" + idTarjeta + "' order by FechaEntrada desc";

            string idTransaccion = string.Empty;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Check is the reader has any rows at all before starting to read.
                        if (reader.HasRows)
                        {
                            // Read advances to the next row.
                            while (reader.Read())
                            {
                                idTransaccion = reader[0].ToString();
                            }
                        }
                        else
                        {
                            oAplicarMotoResponse.Exito = false;
                            oAplicarMotoResponse.ErrorMessage = "No encontro registro.";
                        }
                    }
                }
            }

            if (idTransaccion != string.Empty)
            {
                query = "update T_Transacciones set IdTipoVehiculo=2"
                    + " where IdTransaccion='" + idTransaccion + "'";


                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        int respuesta = cmd.ExecuteNonQuery();

                        if (respuesta <= 0)
                        {
                            oAplicarMotoResponse.Exito = false;
                            oAplicarMotoResponse.ErrorMessage = "No fue posible actualizar el registro.";
                        }
                    }
                }
            }

            return oAplicarMotoResponse;
        }

        [WebMethod]
        public AplicarConvenioResponse AplicarConvenios(string idTransaccion, int idConvenio1, int idConvenio2, int idConvenio3)
        {
            string response = string.Empty;
            AplicarConvenioResponse oAplicarConvenioResponse = new AplicarConvenioResponse();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;


            query = "update T_Transacciones set IdConvenio1=" + idConvenio1
                 + ", IdConvenio2=" + idConvenio2
                 + ", IdConvenio3=" + idConvenio3
                 + " where IdTransaccion='" + idTransaccion + "'";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    int respuesta = cmd.ExecuteNonQuery();

                    if (respuesta <= 0)
                    {
                        oAplicarConvenioResponse.Exito = false;
                        oAplicarConvenioResponse.ErrorMessage = "No fue posible actualizar el registro.";
                    }
                }
            }


            return oAplicarConvenioResponse;
        }

        [WebMethod]
        public AplicaCascoResponse AplicarCasco(string idTransaccion, string idEstacionamiento, string Casillero)
        {
            string response = string.Empty;
            AplicaCascoResponse oAplicarCascoResponse = new AplicaCascoResponse();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;


            query = "insert into T_Cascos values('" + idTransaccion + "','" + idEstacionamiento + "','" + Casillero + "',GETDATE(),'false','true')";



            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    int result = cmd.ExecuteNonQuery();
                    if (result <= 0)
                    {
                        oAplicarCascoResponse.Exito = false;
                        oAplicarCascoResponse.ErrorMessage = "No fue posible insertar el registro.";
                    }
                }
            }


            return oAplicarCascoResponse;
        }

        [WebMethod]
        public SaveConveniosResponse SaveConvenio(string idEstacionamiento, long IdConvenio, string NombreConvenio)
        {
            string response = string.Empty;
            SaveConveniosResponse oSaveConvenioResponse = new SaveConveniosResponse();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;


            query = "insert into T_DetalleConvenios values('" + idEstacionamiento + "','" + NombreConvenio + "'," + IdConvenio + ",GETDATE())";



            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    int result = cmd.ExecuteNonQuery();
                    if (result <= 0)
                    {
                        oSaveConvenioResponse.Exito = false;
                        oSaveConvenioResponse.ErrorMessage = "No fue posible insertar el registro.";
                    }
                }
            }


            return oSaveConvenioResponse;
        }

        [WebMethod]
        public AplicaCascoResponse LiberarCasco(string idTransaccion)
        {
            string response = string.Empty;
            AplicaCascoResponse oAplicarCascoResponse = new AplicaCascoResponse();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;


            query = "update T_Cascos set Estado = 0 where IdTransaccion = '" + idTransaccion + "'";



            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    int result = cmd.ExecuteNonQuery();
                    if (result <= 0)
                    {
                        oAplicarCascoResponse.Exito = false;
                        oAplicarCascoResponse.ErrorMessage = "No fue posible insertar el registro.";
                    }
                }
            }


            return oAplicarCascoResponse;
        }

        [WebMethod]
        public InfoTransaccionService ObtenerCasillero(string idEstacionamiento)
        {
            List<InfoItemsTransaccionService> lstItemsTransac = new List<InfoItemsTransaccionService>();
            InfoTransaccionService oInfoTransaccionService = new InfoTransaccionService();

            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;

            query = "select Casillero from T_Cascos WHERE Estado = 1 and IdEstacionamiento = " + idEstacionamiento;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Check is the reader has any rows at all before starting to read.
                        if (reader.HasRows)
                        {
                            // Read advances to the next row.
                            while (reader.Read())
                            {
                                InfoItemsTransaccionService oInfoItemsTransaccionService = new InfoItemsTransaccionService();
                                oInfoItemsTransaccionService.Casillero = reader[0].ToString();
                                lstItemsTransac.Add(oInfoItemsTransaccionService);
                            }
                        }
                        else
                        {
                            oInfoTransaccionService.Exito = false;
                            oInfoTransaccionService.ErrorMessage = "No encontro registro.";
                        }
                    }
                }
            }

            if (lstItemsTransac.Count > 0)
            {
                oInfoTransaccionService.LstTransac = lstItemsTransac;
            }
            else
            {
                oInfoTransaccionService.Exito = false;
                oInfoTransaccionService.ErrorMessage = "No encuentra informacion facturacion.";
            }

            return oInfoTransaccionService;
        }

        [WebMethod]
        public AplicarCortesiaResponse AplicarLaCortesia(string idEstacionamiento, string observacion, string idMotivo, string idTransaccion, string sUser)
        {
            AplicarCortesiaResponse oAplicarCortesiaResponse = new AplicarCortesiaResponse();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;

            query = "insert into t_cortesias values('" + idTransaccion + "','" + idEstacionamiento + "',GETDATE(),'" + sUser + "','" + idMotivo + "', '" + observacion + "','false')";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    int result = cmd.ExecuteNonQuery();
                    if (result <= 0)
                    {
                        oAplicarCortesiaResponse.Exito = false;
                        oAplicarCortesiaResponse.ErrorMessage = "No fue posible insertar el registro.";
                    }
                }
            }

            return oAplicarCortesiaResponse;
        }

        [WebMethod]
        public AplicarEventoResponse AplicarElEvento(string idEstacionamiento, string idTransaccion, string sUser, string idTarjeta, string idEvento)
        {
            AplicarEventoResponse oAplicarCortesiaResponse = new AplicarEventoResponse();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;

            query = "insert into T_TransaccionEvento values('" + idTransaccion + "','" + idTarjeta + "','" + idEstacionamiento + "','" + idEvento + "', '" + idEvento + "',GetDate())";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    int result = cmd.ExecuteNonQuery();
                    if (result <= 0)
                    {
                        oAplicarCortesiaResponse.Exito = false;
                        oAplicarCortesiaResponse.ErrorMessage = "No fue posible insertar el registro.";
                    }
                }
            }

            return oAplicarCortesiaResponse;
        }

        [WebMethod]
        public MotivosCortesiaResponse ObtenerListaMotivosCortesiaXEstacionamiento(string idEstacionamiento)
        {
            MotivosCortesiaResponse oMotivosCortesiaResponse = new MotivosCortesiaResponse();
            List<InfoMotivosCortesiaResponse> lstMotivos = new List<InfoMotivosCortesiaResponse>();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;

            string user = HttpContext.Current.User.Identity.Name.ToString();

            query = "select IdMotivo, Motivo from t_motivocortesia where idestacionamiento = '" + idEstacionamiento + "'";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Check is the reader has any rows at all before starting to read.
                        if (reader.HasRows)
                        {
                            // Read advances to the next row.
                            while (reader.Read())
                            {
                                InfoMotivosCortesiaResponse oInfoMotivosCortesiaResponse = new InfoMotivosCortesiaResponse();
                                oInfoMotivosCortesiaResponse.Value = Convert.ToInt32(reader["IdMotivo"]);
                                oInfoMotivosCortesiaResponse.Display = reader["Motivo"].ToString();

                                lstMotivos.Add(oInfoMotivosCortesiaResponse);
                            }
                        }
                    }
                }
            }

            if (lstMotivos.Count > 0)
            {
                oMotivosCortesiaResponse.Exito = true;
                oMotivosCortesiaResponse.LstMotivosCortesia = lstMotivos;
            }
            else
            {
                oMotivosCortesiaResponse.Exito = false;
                oMotivosCortesiaResponse.ErrorMessage = "No encuentra Motivos de cortesia";
            }


            return oMotivosCortesiaResponse;
        }

        [WebMethod]
        public CarrilxIdModuloResponse ObtenerCarrilxIdModulo(string idEstacionamiento, string idModulo)
        {
            CarrilxIdModuloResponse oCarrilxIdModuloResponse = new CarrilxIdModuloResponse();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            int carril = 0;

            string query = string.Empty;

            string user = HttpContext.Current.User.Identity.Name.ToString();

            query = "select carril from t_configuracion where idestacionamiento = '" + idEstacionamiento + "' and idmodulo='" + idModulo + "'";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Check is the reader has any rows at all before starting to read.
                        if (reader.HasRows)
                        {
                            // Read advances to the next row.
                            while (reader.Read())
                            {
                                carril = Convert.ToInt32(reader["carril"]);
                            }
                        }
                    }
                }
            }

            if (carril > 0)
            {
                oCarrilxIdModuloResponse.Exito = true;
                oCarrilxIdModuloResponse.Carril = carril;
            }
            else
            {
                oCarrilxIdModuloResponse.Exito = false;
                oCarrilxIdModuloResponse.ErrorMessage = "No encuentra carriles para el modulo";
            }


            return oCarrilxIdModuloResponse;
        }

        [WebMethod]
        public ConveniosResponse ObtenerListaConveniosXEstacionamientoXUsuario(string idEstacionamiento, string user)
        {
            ConveniosResponse array = new ConveniosResponse();
            List<InfoConveniosResponse> lstConvenios = new List<InfoConveniosResponse>();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;

            query = "select c.idConvenio, Nombre"
                    + " from T_Convenios as c"
                    + " inner join T_PermisosConveniosUsuarios as pcu"
                    + " on pcu.IdConvenio = c.idconvenio"
                    + " where pcu.DocumentoUsuario = '" + user + "' and idestacionamiento = '" + idEstacionamiento + "'";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Check is the reader has any rows at all before starting to read.
                        if (reader.HasRows)
                        {
                            // Read advances to the next row.
                            while (reader.Read())
                            {
                                InfoConveniosResponse oInfoConveniosResponse = new InfoConveniosResponse();
                                oInfoConveniosResponse.Value = Convert.ToInt32(reader["IdConvenio"]);
                                oInfoConveniosResponse.Display = reader["Nombre"].ToString();
                                lstConvenios.Add(oInfoConveniosResponse);
                            }
                        }
                    }
                }
            }

            if (lstConvenios.Count > 0)
            {
                array.Exito = true;
                array.LstInfoConvenios = lstConvenios;
            }
            else
            {
                array.Exito = false;
                array.ErrorMessage = "No encuentra convenios";
            }

            return array;
        }

        [WebMethod]
        public EventosResponse ObtenerListaEventosXEstacionamientoXUsuario(string idEstacionamiento, string user)
        {
            EventosResponse array = new EventosResponse();
            List<InfoEventosResponse> lstEventos = new List<InfoEventosResponse>();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;

            query = "select e.idEvento, Evento"
                    + " from T_Eventos as e"
                    + " where idestacionamiento = '" + idEstacionamiento + "'";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Check is the reader has any rows at all before starting to read.
                        if (reader.HasRows)
                        {
                            // Read advances to the next row.
                            while (reader.Read())
                            {
                                InfoEventosResponse oInfoEventosResponse = new InfoEventosResponse();
                                oInfoEventosResponse.Value = Convert.ToInt32(reader["IdEvento"]);
                                oInfoEventosResponse.Display = reader["Evento"].ToString();
                                lstEventos.Add(oInfoEventosResponse);
                            }
                        }
                    }
                }
            }

            if (lstEventos.Count > 0)
            {
                array.Exito = true;
                array.LstInfoEventos = lstEventos;
            }
            else
            {
                array.Exito = false;
                array.ErrorMessage = "No encuentra eventos";
            }

            return array;
        }

        [WebMethod]
        public DescripcionConvenioResponse ConsultarDescripcionConvenio(string idConvenio)
        {
            DescripcionConvenioResponse oDescripcionConvenioResponse = new DescripcionConvenioResponse();
            string valor = string.Empty;
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            string query = string.Empty;

            query = "select descripcion from t_convenios where idconvenio='" + idConvenio + "'";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Check is the reader has any rows at all before starting to read.
                        if (reader.HasRows)
                        {
                            // Read advances to the next row.
                            while (reader.Read())
                            {
                                valor = reader[0].ToString();
                            }
                        }
                    }
                }
            }

            if (valor != string.Empty)
            {
                oDescripcionConvenioResponse.Exito = true;
                oDescripcionConvenioResponse.Descripcion = valor;
            }
            else
            {
                oDescripcionConvenioResponse.Exito = false;
                oDescripcionConvenioResponse.ErrorMessage = "No se encontro descripcion de convenio.";
            }

            return oDescripcionConvenioResponse;
        }

        [WebMethod]
        public RegistrarArqueoResponse RegistrarElArqueo(string idEstacionamiento, string idModulo, string user)
        {
            string response = string.Empty;
            RegistrarArqueoResponse oRegistrarArqueoResponse = new RegistrarArqueoResponse();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;

            query = "insert into T_Arqueos(Fechainicio,IdUsuario,IdModulo, IdEstacionamiento,Tipo) " +
                    "output INSERTED.IdArqueo " +
                    "values (getdate()," + user + ",'" + idModulo + "'," + idEstacionamiento + ",'T')";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();

                    int result2 = Convert.ToInt32(cmd.ExecuteScalar());

                    if (result2 != 0)
                    {
                        oRegistrarArqueoResponse.Exito = true;
                        oRegistrarArqueoResponse.IdArqueo = result2;
                    }
                    else
                    {
                        oRegistrarArqueoResponse.Exito = false;
                        oRegistrarArqueoResponse.ErrorMessage = "No se puede insertar arqueo";
                    }
                }
            }

            return oRegistrarArqueoResponse;
        }

        [WebMethod]
        public ConfirmarArqueoResponse ConfirmarElArqueo(string idEstacionamiento, string idModulo, string idArqueo, string manual)
        {
            ConfirmarArqueoResponse oConfirmarArqueoResponse = new ConfirmarArqueoResponse();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;

            string FechaAntes = string.Empty;
            string FechaDespues = string.Empty;

            query = "select max(FechaFin),getdate() " +
                    "from T_Arqueos " +
                    "where  IdModulo = '" + idModulo + "' and Valor != 0 and IdEstacionamiento='" + idEstacionamiento + "'";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Check is the reader has any rows at all before starting to read.
                        if (reader.HasRows)
                        {
                            // Read advances to the next row.
                            while (reader.Read())
                            {
                                FechaAntes = reader[0].ToString();
                                FechaDespues = reader[1].ToString();
                            }
                        }
                    }
                }
            }

            string queryCantidad = "(select (case when COUNT(IdTipoPago) is null then 0 else COUNT(IdTipoPago) end) " +
                                  "from T_Pagos as P inner join T_Transacciones as T on CAST(T.IdTransaccion as varchar) = P.IdTransaccion " +
                                  "where  P.IdModulo = '" + idModulo + "' and T.IdEstacionamiento='" + idEstacionamiento + "'" +
                                  "and P.FechaPago between (select max(FechaFin) from T_Arqueos where  IdModulo = '" + idModulo + "' and Valor != 0 and IdEstacionamiento='" + idEstacionamiento + "') and getdate())";

            string queryValor = "(select (case when SUM(P.Total) is null then 0 else SUM(P.Total) end)  " +
                                  "from T_Pagos as P inner join T_Transacciones as T on CAST(T.IdTransaccion as varchar) = P.IdTransaccion " +
                                  "where  P.IdModulo = '" + idModulo + "' and T.IdEstacionamiento='" + idEstacionamiento + "'" +
                                  "and P.FechaPago between (select max(FechaFin) from T_Arqueos where  IdModulo = '" + idModulo + "' and Valor != 0 and IdEstacionamiento='" + idEstacionamiento + "') and getdate())";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;

                // Start a local transaction.
                transaction = connection.BeginTransaction("ArqueoTransaction");

                // Must assign both transaction object and connection 
                // to Command object for a pending local transaction
                command.Connection = connection;
                command.Transaction = transaction;

                try
                {
                    command.CommandText =
                            "Insert into T_Movimientos (IdArqueo, IdEstacionamiento, IdModulo, Parte, Accion, Denominacion, Cantidad, Valor, FechaMovimiento) VALUES "
                            + "('" + idArqueo + "','" + idEstacionamiento + "','" + idModulo + "','CM','Salida',1,'1',(select DineroActual from T_Partes where IdModulo='" + idModulo + "' and IdEstacionamiento='" + idEstacionamiento + "' and NombreParte='CM'),GETDATE())";
                    command.ExecuteNonQuery();

                    command.CommandText =
                            "update T_Arqueos set FechaFin=GetDate(), Valor=(select DineroActual from T_Partes where IdModulo='" + idModulo + "' and IdEstacionamiento='" + idEstacionamiento + "' and NombreParte='CM'), CantTransacciones=" + queryCantidad + " , Producido=" + queryValor + ", Conteo = " + manual + ", Sincronizacion = 'false' where IdModulo='" + idModulo + "' and IdEstacionamiento='" + idEstacionamiento + "' and idArqueo=" + idArqueo;
                    command.ExecuteNonQuery();

                    command.CommandText =
                            "update T_Partes set DineroActual=0 , sincronizacion = 0 where IdModulo='" + idModulo + "' and IdEstacionamiento='" + idEstacionamiento + "' and NombreParte='CM'";
                    command.ExecuteNonQuery();


                    // Attempt to commit the transaction.
                    transaction.Commit();

                }
                catch (Exception ex)
                {
                    oConfirmarArqueoResponse.Exito = false;
                    oConfirmarArqueoResponse.ErrorMessage = "No guarda modificaciones en BD.";
                    // Attempt to roll back the transaction. 
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception ex2)
                    {
                        //No rollback
                    }
                }
            }

            return oConfirmarArqueoResponse;
        }

        [WebMethod]
        public InfoArqueoResponse ObtenerDatosComprobanteArqueo(string idArqueo)
        {
            List<InfoItemsArqueoResponse> lista = new List<InfoItemsArqueoResponse>();

            InfoArqueoResponse oInfoArqueoResponse = new InfoArqueoResponse();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;

            query = "select e.Nombre, e.TelefonoContacto, e.Direccion, c.IdModulo, a.FechaFin, a.IdArqueo, a.Producido, a.CantTransacciones, a.Valor, a.Conteo, a.IdUsuario"
                    + " from T_Arqueos as a"
                    + " inner join T_Estacionamientos as e"
                    + " on a.IdEstacionamiento=e.IdEstacionamiento"
                    + " inner join T_Configuracion as c"
                    + " on a.IdModulo=c.IdModulo"
                    + " where a.IdArqueo='" + idArqueo + "'";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Check is the reader has any rows at all before starting to read.
                        if (reader.HasRows)
                        {
                            // Read advances to the next row.
                            while (reader.Read())
                            {
                                InfoItemsArqueoResponse item = new InfoItemsArqueoResponse();

                                item.Nombre = reader[0].ToString();
                                item.Telefono = reader[1].ToString();
                                item.Direccion = reader[2].ToString();
                                item.Modulo = reader[3].ToString();
                                item.Fecha = reader[4].ToString();
                                item.IdArqueo = reader[5].ToString();
                                item.Producido = reader[6].ToString();
                                item.CantTransacciones = reader[7].ToString();
                                item.Valor = reader[8].ToString();
                                item.Conteo = reader[9].ToString();
                                item.IdUsuario = reader[10].ToString();

                                lista.Add(item);
                            }
                        }
                    }
                }
            }

            if (lista.Count > 0)
            {
                oInfoArqueoResponse.LstInfoArqueos = lista;
            }
            else
            {
                oInfoArqueoResponse.Exito = false;
                oInfoArqueoResponse.ErrorMessage = "No encuentra informacion ticket arquero.";
            }

            return oInfoArqueoResponse;
        }

        [WebMethod]
        public InfoCargaResponse ObtenerDatosComprobanteCarga(string idCarga)
        {
            List<InfoItemsCargaResponse> array = new List<InfoItemsCargaResponse>();
            string response = string.Empty;
            InfoCargaResponse oInfoCargaResponse = new InfoCargaResponse();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;

            query = "select e.Nombre, e.TelefonoContacto, e.Direccion, c.IdModulo, car.FechaFin, car.IdCarga, car.Valor, car.IdUsuario, car.IdEstacionamiento"
                    + " from T_Carga as car"
                    + " inner join T_Estacionamientos as e"
                    + " on car.IdEstacionamiento=e.IdEstacionamiento"
                    + " inner join T_Configuracion as c"
                    + " on car.IdModulo=c.IdModulo"
                    + " where car.IdCarga='" + idCarga + "'";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Check is the reader has any rows at all before starting to read.
                        if (reader.HasRows)
                        {
                            // Read advances to the next row.
                            while (reader.Read())
                            {
                                InfoItemsCargaResponse oInfoItemsCargaResponse = new InfoItemsCargaResponse();

                                oInfoItemsCargaResponse.Nombre = reader[0].ToString();
                                oInfoItemsCargaResponse.Telefono = reader[1].ToString();
                                oInfoItemsCargaResponse.Direccion = reader[2].ToString();
                                oInfoItemsCargaResponse.Modulo = reader[3].ToString();
                                oInfoItemsCargaResponse.Fecha = reader[4].ToString();
                                oInfoItemsCargaResponse.IdCarga = reader[5].ToString();
                                oInfoItemsCargaResponse.Valor = reader[6].ToString();
                                oInfoItemsCargaResponse.IdUsuario = reader[7].ToString();
                                oInfoItemsCargaResponse.IdEstacionamiento = reader[8].ToString();

                                array.Add(oInfoItemsCargaResponse);
                            }
                        }
                    }
                }
            }

            if (array.Count > 0)
            {
                oInfoCargaResponse.LstInfoCargas = array;
            }
            else
            {
                oInfoCargaResponse.Exito = false;
                oInfoCargaResponse.ErrorMessage = "No encuentra informacion ticket carga.";
            }

            return oInfoCargaResponse;
        }

        [WebMethod]
        public RegistrarCargaResponse RegistrarLaCarga(string idEstacionamiento, string idModulo, string user)
        {
            string response = string.Empty;
            RegistrarCargaResponse oRegistrarCargaResponse = new RegistrarCargaResponse();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;

            query = "insert into T_Carga (Fechainicio,IdUsuario, IdModulo, IdEstacionamiento) " +
                    "output INSERTED.IdCarga " +
                    "values (getdate()," + user + ",'" + idModulo + "'," + idEstacionamiento + ")";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();

                    int result2 = Convert.ToInt32(cmd.ExecuteScalar());

                    if (result2 != 0)
                    {
                        oRegistrarCargaResponse.IdCarga = result2;
                    }
                    else
                    {
                        oRegistrarCargaResponse.Exito = false;
                        oRegistrarCargaResponse.ErrorMessage = "No se puede insertar carga";
                    }
                }
            }

            return oRegistrarCargaResponse;
        }

        [WebMethod]
        public ConfirmarCargaResponse ConfirmarLaCarga(string idEstacionamiento, string idModulo, string idCarga, string valor)
        {
            ConfirmarCargaResponse oConfirmarCargaResponse = new ConfirmarCargaResponse();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;

                // Start a local transaction.
                transaction = connection.BeginTransaction("CargaTransaction");

                // Must assign both transaction object and connection 
                // to Command object for a pending local transaction
                command.Connection = connection;
                command.Transaction = transaction;

                try
                {

                    command.CommandText =
                            "Insert into T_Movimientos (IdCarga, IdEstacionamiento, IdModulo, Parte, Accion, Denominacion, Cantidad, Valor, FechaMovimiento) VALUES "
                            + "('" + idCarga + "','" + idEstacionamiento + "','" + idModulo + "','CM','Entrada',1,'1'," + valor + ",GETDATE())";
                    command.ExecuteNonQuery();

                    command.CommandText =
                            "update T_Carga set FechaFin=GetDate(), Valor=" + valor + " where IdModulo='" + idModulo + "' and IdEstacionamiento='" + idEstacionamiento + "' and idCarga=" + idCarga;
                    command.ExecuteNonQuery();

                    command.CommandText =
                            "update T_Partes set DineroActual=(select DineroActual from t_partes where NombreParte='cm' and IdEstacionamiento='" + idEstacionamiento + "' and IdModulo='" + idModulo + "')+" + valor + ", sincronizacion = 0 where IdModulo='" + idModulo + "' and IdEstacionamiento='" + idEstacionamiento + "' and NombreParte='CM'";
                    command.ExecuteNonQuery();


                    // Attempt to commit the transaction.
                    transaction.Commit();

                }
                catch (Exception ex)
                {
                    oConfirmarCargaResponse.Exito = false;
                    oConfirmarCargaResponse.ErrorMessage = "No guarda modificaciones en BD.";
                    // Attempt to roll back the transaction. 
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception ex2)
                    {
                        //No rollback
                    }
                }
            }

            return oConfirmarCargaResponse;
        }

        [WebMethod]
        public CarrilEntradaXEntradaResponse ObtenerListaCarrilEntradaxEstacionamiento(int idSede, int idEstacionamiento)
        {

            CarrilEntradaXEntradaResponse array = new CarrilEntradaXEntradaResponse();
            List<CarrilesEntrada> lstCarriles = new List<CarrilesEntrada>();

            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;

            if ((idEstacionamiento == null || idEstacionamiento == 0) && (idSede == null || idSede == 0))
            {
                query = "select IdModulo, Carril from T_Configuracion where IdTipoModulo=1";
            }
            else if ((idEstacionamiento == null || idEstacionamiento == 0) && (idSede != null || idSede != 0))
            {
                query = "select IdModulo, Carril from T_Configuracion as c inner join t_sedes as s on c.IdEstacionamiento=s.IdSede where IdTipoModulo=1 and IdSede =" + idSede;
            }
            else
            {
                query = "select IdModulo, Carril from T_Configuracion where IdTipoModulo=1 and IdEstacionamiento = " + idEstacionamiento;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Check is the reader has any rows at all before starting to read.
                        if (reader.HasRows)
                        {
                            // Read advances to the next row.
                            while (reader.Read())
                            {
                                CarrilesEntrada oCarrilesEntrada = new CarrilesEntrada();
                                oCarrilesEntrada.Value = Convert.ToInt32(reader["Carril"]);
                                oCarrilesEntrada.Display = reader["IdModulo"].ToString();
                                lstCarriles.Add(oCarrilesEntrada);
                            }
                        }
                    }
                }
            }

            if (lstCarriles.Count > 0)
            {
                array.Exito = true;
                array.LstCarrillesEntrada = lstCarriles;
            }
            else
            {
                array.Exito = false;
                array.ErrorMessage = "No encuentra carriles";
            }

            return array;
        }

        [WebMethod]
        public CreaEntradaResponse CrearEntrada(string idEstacionamiento, string idTarjeta, string carril, string placa, DateTime fecha, string tipov, string _IdAutorizacion)
        {
            string modulo = string.Empty;

            string user = HttpContext.Current.User.Identity.Name.ToString();

            string dia = string.Empty;
            string mes = string.Empty;
            string anho = string.Empty;
            string hora = string.Empty;
            string min = string.Empty;
            string seg = string.Empty;

            string response = string.Empty;
            CreaEntradaResponse oCrearEntradaResponse = new CreaEntradaResponse();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;

            try
            {
                dia = fecha.ToString("dd");
                if (dia.Length == 1)
                {
                    dia = "0" + dia;
                }
                mes = fecha.ToString("MM");
                if (mes.Length == 1)
                {
                    mes = "0" + mes;
                }
                anho = fecha.ToString("yyyy");
                hora = fecha.ToString("HH");
                if (hora.Length == 1)
                {
                    hora = "0" + hora;
                }
                min = fecha.ToString("mm");
                if (min.Length == 1)
                {
                    min = "0" + min;
                }
                seg = fecha.ToString("ss");
                if (seg.Length == 1)
                {
                    seg = "0" + seg;
                }




                query = "select carril from t_configuracion where idEstacionamiento='" + idEstacionamiento + "' and idmodulo='" + carril + "'";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Check is the reader has any rows at all before starting to read.
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    modulo = reader["carril"].ToString();
                                }
                            }
                        }
                    }
                }

                if (modulo != string.Empty)
                {
                    if (_IdAutorizacion != string.Empty)
                    {
                        query = "insert into t_transacciones values ('" + anho + mes + dia + hora + min + seg + modulo + idEstacionamiento + "','" + modulo + "','" + carril + "','" + idEstacionamiento + "','" + idTarjeta + "','" + placa.ToUpper() + "'," + "convert(datetime,'" + fecha.ToString("dd/MM/yyyy HH:mm:ss") + "',103),NULL,NULL,NULL,NULL,'" + tipov + "',NULL, " + _IdAutorizacion + ", NULL, NULL, NULL, NULL, NULL, 'false', 'false', 'false')";
                    }
                    else
                    {
                        query = "insert into t_transacciones values ('" + anho + mes + dia + hora + min + seg + modulo + idEstacionamiento + "','" + modulo + "','" + carril + "','" + idEstacionamiento + "','" + idTarjeta + "','" + placa.ToUpper() + "'," + "convert(datetime,'" + fecha.ToString("dd/MM/yyyy HH:mm:ss") + "',103),NULL,NULL,NULL,NULL,'" + tipov + "',NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'false', 'false', 'false')";
                    }



                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand(query, connection))
                        {
                            connection.Open();
                            int resultado = cmd.ExecuteNonQuery();
                            if (resultado <= 0)
                            {
                                oCrearEntradaResponse.Exito = false;
                            }
                            else
                            {

                            }
                        }
                    }

                }
                else
                {
                    oCrearEntradaResponse.Exito = false;
                    oCrearEntradaResponse.ErrorMessage = "No se encontro modulo para el carril seleccionado.";
                }

            }
            catch (Exception e)
            {
                oCrearEntradaResponse.Exito = false;
                oCrearEntradaResponse.ErrorMessage = e.InnerException + " " + e.Message + " / " + query;
            }

            return oCrearEntradaResponse;
        }

        [WebMethod]
        public CreaSalidaResponse CrearSalida(string idEstacionamiento, string placa)
        {

            string user = HttpContext.Current.User.Identity.Name.ToString();

            string response = string.Empty;
            CreaSalidaResponse oCreaSalidaResponse = new CreaSalidaResponse();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            string idsalida = string.Empty;

            string query = string.Empty;


            try
            {
                query = "select t.CarrilEntrada, t.ModuloEntrada, t.IdTransaccion from T_Transacciones as t where t.PlacaEntrada ='" + placa + "' and t.FechaSalida is null order by FechaEntrada desc";

                string carril = string.Empty;
                string modulo = string.Empty;
                string idTransaccion = string.Empty;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Check is the reader has any rows at all before starting to read.
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    carril = reader["CarrilEntrada"].ToString();
                                    modulo = reader["ModuloEntrada"].ToString();
                                    idTransaccion = reader["IdTransaccion"].ToString();
                                }
                            }
                        }
                    }
                }



                if (carril != string.Empty)
                {
                    try
                    {
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand("P_RegistrarSalida", connection))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.Add("@IdTransaccion", SqlDbType.BigInt).Value = idTransaccion;
                                cmd.Parameters.Add("@CarrilSalida", SqlDbType.Int).Value = carril;
                                cmd.Parameters.Add("@ModuloSalida", SqlDbType.VarChar).Value = modulo;
                                cmd.Parameters.Add("@IdEstacionamiento", SqlDbType.BigInt).Value = Convert.ToInt32(idEstacionamiento);
                                cmd.Parameters.Add("@IdTarjeta", SqlDbType.VarChar).Value = "NULL";
                                cmd.Parameters.Add("@PlacaSalida", SqlDbType.VarChar).Value = placa;

                                connection.Open();
                                using (SqlDataReader reader = cmd.ExecuteReader())
                                {
                                    if (reader.HasRows)
                                    {
                                        while (reader.Read())
                                        {
                                            idsalida = reader[0].ToString();
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        oCreaSalidaResponse.Exito = false;
                        oCreaSalidaResponse.ErrorMessage = ex.InnerException + " " + ex.Message;
                    }
                }
                else
                {
                    oCreaSalidaResponse.Exito = false;
                    oCreaSalidaResponse.ErrorMessage = "No se entrada abierta asociada a la placa.";
                }
            }
            catch (Exception e)
            {
                oCreaSalidaResponse.Exito = false;
                oCreaSalidaResponse.ErrorMessage = e.InnerException + " " + e.Message;
            }

            if (idsalida == string.Empty)
            {
                oCreaSalidaResponse.Exito = false;
                oCreaSalidaResponse.ErrorMessage = "No fue posible registrar la salida.";
            }

            return oCreaSalidaResponse;
        }

        [WebMethod]
        public CreaSalidaResponse CrearSalida2(string idEstacionamiento, string placa, string idTransaccion, string carril, string modulo, string idtarjeta)
        {

            string user = HttpContext.Current.User.Identity.Name.ToString();

            string response = string.Empty;
            CreaSalidaResponse oCreaSalidaResponse = new CreaSalidaResponse();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            string idsalida = string.Empty;

            string query = string.Empty;



            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("P_RegistrarSalida", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@IdTransaccion", SqlDbType.BigInt).Value = idTransaccion;
                        cmd.Parameters.Add("@CarrilSalida", SqlDbType.Int).Value = carril;
                        cmd.Parameters.Add("@ModuloSalida", SqlDbType.VarChar).Value = modulo;
                        cmd.Parameters.Add("@IdEstacionamiento", SqlDbType.BigInt).Value = Convert.ToInt32(idEstacionamiento);
                        cmd.Parameters.Add("@IdTarjeta", SqlDbType.VarChar).Value = idtarjeta;
                        cmd.Parameters.Add("@PlacaSalida", SqlDbType.VarChar).Value = placa;

                        connection.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    idsalida = reader[0].ToString();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                oCreaSalidaResponse.Exito = false;
                oCreaSalidaResponse.ErrorMessage = ex.InnerException + " " + ex.Message;
            }

            if (idsalida == string.Empty)
            {
                oCreaSalidaResponse.Exito = false;
                oCreaSalidaResponse.ErrorMessage = "No fue posible registrar la salida.";
            }

            return oCreaSalidaResponse;
        }

        [WebMethod]
        public CreaSalidaResponse CrearSalida3(string idEstacionamiento, string idTransaccion, string idModulo)
        {

            string user = HttpContext.Current.User.Identity.Name.ToString();

            string response = string.Empty;
            CreaSalidaResponse oCreaSalidaResponse = new CreaSalidaResponse();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            string idsalida = string.Empty;

            string query = string.Empty;


            try
            {
                query = "select t.CarrilEntrada, t.ModuloEntrada, t.IdTransaccion, t.PlacaEntrada from T_Transacciones as t where t.IdTransaccion ='" + idTransaccion + "' and t.FechaSalida is null order by FechaEntrada desc";

                string carril = string.Empty;
                string modulo = string.Empty;
                string placa = string.Empty;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Check is the reader has any rows at all before starting to read.
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    carril = reader["CarrilEntrada"].ToString();
                                    modulo = reader["ModuloEntrada"].ToString();
                                    idTransaccion = reader["IdTransaccion"].ToString();
                                    placa = reader["PlacaEntrada"].ToString();
                                }
                            }
                        }
                    }
                }



                if (carril != string.Empty)
                {
                    try
                    {
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand("P_RegistrarSalida", connection))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;

                                cmd.Parameters.Add("@IdTransaccion", SqlDbType.BigInt).Value = idTransaccion;
                                cmd.Parameters.Add("@CarrilSalida", SqlDbType.Int).Value = carril;
                                cmd.Parameters.Add("@ModuloSalida", SqlDbType.VarChar).Value = modulo;
                                cmd.Parameters.Add("@IdEstacionamiento", SqlDbType.BigInt).Value = Convert.ToInt32(idEstacionamiento);
                                cmd.Parameters.Add("@IdTarjeta", SqlDbType.VarChar).Value = "NULL";
                                cmd.Parameters.Add("@PlacaSalida", SqlDbType.VarChar).Value = placa;

                                connection.Open();
                                using (SqlDataReader reader = cmd.ExecuteReader())
                                {
                                    if (reader.HasRows)
                                    {
                                        while (reader.Read())
                                        {
                                            idsalida = reader[0].ToString();
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        oCreaSalidaResponse.Exito = false;
                        oCreaSalidaResponse.ErrorMessage = ex.InnerException + " " + ex.Message;
                    }
                }
                else
                {
                    oCreaSalidaResponse.Exito = false;
                    oCreaSalidaResponse.ErrorMessage = "No se entrada abierta asociada a la placa.";
                }
            }
            catch (Exception e)
            {
                oCreaSalidaResponse.Exito = false;
                oCreaSalidaResponse.ErrorMessage = e.InnerException + " " + e.Message;
            }

            if (idsalida == string.Empty)
            {
                oCreaSalidaResponse.Exito = false;
                oCreaSalidaResponse.ErrorMessage = "No fue posible registrar la salida.";
            }

            return oCreaSalidaResponse;
        }     

        [WebMethod]
        public VerificaTransaccionAbiertaAutorizadoResponse VerificarTransaccionAbiertaAutorizado(string idTarjeta)
        {

            VerificaTransaccionAbiertaAutorizadoResponse array = new VerificaTransaccionAbiertaAutorizadoResponse();

            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;
            string valor = string.Empty;


            query = "select * from T_Transacciones where IdTarjeta = '" + idTarjeta + "' and FechaSalida is Null";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Check is the reader has any rows at all before starting to read.
                        if (reader.HasRows)
                        {
                            // Read advances to the next row.
                            while (reader.Read())
                            {
                                valor = reader[0].ToString();
                            }
                        }
                    }
                }
            }

            if (valor != string.Empty)
            {
                array.Exito = true;
            }
            else
            {
                array.Exito = false;
                array.ErrorMessage = "No se encontro transaccion abierta autorizado.";
            }

            return array;
        }

        [WebMethod]
        public VerificaVigenciaAutorizadoResponse VerificarVigenciaAutorizado(string idTarjeta)
        {

            VerificaVigenciaAutorizadoResponse array = new VerificaVigenciaAutorizadoResponse();

            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;
            string valor = string.Empty;


            query = "if GETDATE() between (select FechaInicio " +
                    "from T_PersonasAutorizadas " +
                    "where IdTarjeta = '" + idTarjeta + "') " +
                    "and (select FechaFin " +
                    "from T_PersonasAutorizadas " +
                    "where IdTarjeta = '" + idTarjeta + "') " +
                    "begin " +
                    "select 'OK' " +
                    "end " +
                    "else " +
                    "begin " +
                    "select 'ERROR' " +
                    "end";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Check is the reader has any rows at all before starting to read.
                        if (reader.HasRows)
                        {
                            // Read advances to the next row.
                            while (reader.Read())
                            {
                                valor = reader[0].ToString();
                            }
                        }
                    }
                }
            }

            if (valor != string.Empty && valor == "OK")
            {
                array.Exito = true;
            }
            else
            {
                array.Exito = false;
                array.ErrorMessage = "Usuario con vigencia vencida.";
            }

            return array;
        }

        [WebMethod]
        public ActualizaVigenciaAutorizadoResponse ActualizarVigenciaAutorizado(string idTarjeta)
        {

            ActualizaVigenciaAutorizadoResponse array = new ActualizaVigenciaAutorizadoResponse();

            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            string idsalida = string.Empty;

            string query = string.Empty;



            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("P_ActualizaMensual", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@IdTarjeta", SqlDbType.VarChar).Value = idTarjeta;

                        connection.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    idsalida = reader[0].ToString();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                array.Exito = false;
                array.ErrorMessage = "Error con IdTarjeta = " + idTarjeta + " - Excepcion: " + ex.InnerException + " " + ex.Message;
                return array;
            }

            if (idsalida != string.Empty && idsalida == "OK")
            {
                array.Exito = true;
            }
            else
            {
                array.Exito = false;
                array.ErrorMessage = "Error con IdTarjeta = " + idTarjeta + " - No fue posible actualizar vigencia. Respuesta = " + idsalida;
            }

            return array;
        }

        [WebMethod]
        public VehiculosEnValetResponse ObtenerListaVehiculosEnValet(string idEstacionamiento, string user)
        {
            VehiculosEnValetResponse array = new VehiculosEnValetResponse();
            List<InfoVehiculosEnValetResponse> lstVehiculos = new List<InfoVehiculosEnValetResponse>();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;

            query = "select *"
                    + " from T_VehiculosEnValet where Estado = 'Activo'";

            if (idEstacionamiento != null && idEstacionamiento != string.Empty)
            {
                query += " and idestacionamiento = '" + idEstacionamiento + "'";
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Check is the reader has any rows at all before starting to read.
                        if (reader.HasRows)
                        {
                            // Read advances to the next row.
                            while (reader.Read())
                            {
                                InfoVehiculosEnValetResponse oInfoVehiculosResponse = new InfoVehiculosEnValetResponse();
                                oInfoVehiculosResponse.Color = reader["Color"].ToString();
                                oInfoVehiculosResponse.Estado = reader["Estado"].ToString();
                                oInfoVehiculosResponse.IdTransaccion = reader["IdTransaccion"].ToString();
                                oInfoVehiculosResponse.Marca = reader["Marca"].ToString();
                                oInfoVehiculosResponse.Placa = reader["Placa"].ToString();
                                oInfoVehiculosResponse.Ubicacion = reader["Ubicacion"].ToString();
                                oInfoVehiculosResponse.IdEstacionamiento = reader["IdEstacionamiento"].ToString();
                                lstVehiculos.Add(oInfoVehiculosResponse);
                            }
                        }
                    }
                }
            }

            if (lstVehiculos.Count > 0)
            {
                array.Exito = true;
                array.LstInfoVehiculosEnValet = lstVehiculos;
            }
            else
            {
                array.Exito = false;
                array.ErrorMessage = "No encuentra vehiculos";
            }

            return array;
        }

        [WebMethod]
        public VehiculosEnValetResponse ObtenerListaVehiculosSaliendo(string idEstacionamiento, string user)
        {
            VehiculosEnValetResponse array = new VehiculosEnValetResponse();
            List<InfoVehiculosEnValetResponse> lstVehiculos = new List<InfoVehiculosEnValetResponse>();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;

            query = "select *"
                    + " from T_VehiculosEnValet where Estado = 'Saliendo'";

            if (idEstacionamiento != null && idEstacionamiento != string.Empty)
            {
                query += " and idestacionamiento = '" + idEstacionamiento + "'";
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Check is the reader has any rows at all before starting to read.
                        if (reader.HasRows)
                        {
                            // Read advances to the next row.
                            while (reader.Read())
                            {
                                InfoVehiculosEnValetResponse oInfoVehiculosResponse = new InfoVehiculosEnValetResponse();
                                oInfoVehiculosResponse.Color = reader["Color"].ToString();
                                oInfoVehiculosResponse.Estado = reader["Estado"].ToString();
                                oInfoVehiculosResponse.IdTransaccion = reader["IdTransaccion"].ToString();
                                oInfoVehiculosResponse.Marca = reader["Marca"].ToString();
                                oInfoVehiculosResponse.Placa = reader["Placa"].ToString();
                                oInfoVehiculosResponse.Ubicacion = reader["Ubicacion"].ToString();
                                oInfoVehiculosResponse.IdEstacionamiento = reader["IdEstacionamiento"].ToString();
                                lstVehiculos.Add(oInfoVehiculosResponse);
                            }
                        }
                    }
                }
            }

            if (lstVehiculos.Count > 0)
            {
                array.Exito = true;
                array.LstInfoVehiculosEnValet = lstVehiculos;
            }
            else
            {
                array.Exito = false;
                array.ErrorMessage = "No encuentra vehiculos";
            }

            return array;
        }

        [WebMethod]
        public VehiculosEnValetResponse InsertarVehiculoValet(string idTransaccion, string sPlaca, string sColor, string sMarca, string sUbicacion, string idEstacionamiento)
        {

            string user = HttpContext.Current.User.Identity.Name.ToString();

            string response = string.Empty;
            VehiculosEnValetResponse oVehiculosEnValetResponse = new VehiculosEnValetResponse();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            string idsalida = string.Empty;

            string query = string.Empty;

            query = "insert into t_vehiculosenvalet values('" + idTransaccion + "','" + sPlaca + "','" + sColor + "','" + sMarca + "','ACTIVO', '" + sUbicacion + "','" + idEstacionamiento + "')";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    int result = cmd.ExecuteNonQuery();
                    if (result <= 0)
                    {
                        oVehiculosEnValetResponse.Exito = false;
                        oVehiculosEnValetResponse.ErrorMessage = "No fue posible insertar el registro.";
                    }
                }
            }

            return oVehiculosEnValetResponse;
        }

        [WebMethod]
        public SedesResponse ObtenerListaSedes(string user)
        {
            SedesResponse array = new SedesResponse();
            List<InfoSedesResponse> lstSedes = new List<InfoSedesResponse>();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;

            query = "select s.IdSede, Nombre"
                    + " from T_Sedes as s";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Check is the reader has any rows at all before starting to read.
                        if (reader.HasRows)
                        {
                            // Read advances to the next row.
                            while (reader.Read())
                            {
                                InfoSedesResponse oInfoSedesResponse = new InfoSedesResponse();
                                oInfoSedesResponse.Value = Convert.ToInt32(reader["IdSede"]);
                                oInfoSedesResponse.Display = reader["Nombre"].ToString();
                                lstSedes.Add(oInfoSedesResponse);
                            }
                        }
                    }
                }
            }

            if (lstSedes.Count > 0)
            {
                array.Exito = true;
                array.LstInfoSedes = lstSedes;
            }
            else
            {
                array.Exito = false;
                array.ErrorMessage = "No encuentra sedes";
            }

            return array;
        }

        [WebMethod]
        public EstacionamientosResponse ObtenerListaEstacionamientoXSede(string user, string idSede)
        {
            EstacionamientosResponse array = new EstacionamientosResponse();
            List<InfoEstacionamientosResponse> lstEstacionamientos = new List<InfoEstacionamientosResponse>();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;

            query = "select e.IdEstacionamiento, Nombre"
                    + " from T_Estacionamientos as e"
                    + " where e.IdSede = " + idSede;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Check is the reader has any rows at all before starting to read.
                        if (reader.HasRows)
                        {
                            // Read advances to the next row.
                            while (reader.Read())
                            {
                                InfoEstacionamientosResponse oInfoEstacionamientosResponse = new InfoEstacionamientosResponse();
                                oInfoEstacionamientosResponse.Value = Convert.ToInt32(reader["IdEstacionamiento"]);
                                oInfoEstacionamientosResponse.Display = reader["Nombre"].ToString();
                                lstEstacionamientos.Add(oInfoEstacionamientosResponse);
                            }
                        }
                    }
                }
            }

            if (lstEstacionamientos.Count > 0)
            {
                array.Exito = true;
                array.LstInfoEstacionamientos = lstEstacionamientos;
            }
            else
            {
                array.Exito = false;
                array.ErrorMessage = "No encuentra estacionamientos";
            }

            return array;
        }

        [WebMethod]
        public string ObtenerNombreConvenio(string IdConvenio)
        {
            string valor = string.Empty;
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            string query = string.Empty;

            query = "select Nombre from T_Convenios where IdConvenio=" + IdConvenio;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Check is the reader has any rows at all before starting to read.
                        if (reader.HasRows)
                        {
                            // Read advances to the next row.
                            while (reader.Read())
                            {
                                valor = reader[0].ToString();
                            }
                        }
                    }
                }
            }

            return valor;
        }

        [WebMethod]
        public string InsertaIdConvTrans(string IdTransaccion, string IdConvenio)
        {
            string valor = string.Empty;
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            string query = string.Empty;

            query = "DECLARE @VALOR VARCHAR(50) \n" +
            "DECLARE @RSPTA VARCHAR(50) \n" +
            "SET @RSPTA = null \n" +
            "SET @RSPTA = 'VALIDADO' \n" +
            "SET @VALOR = null \n" +
            "SET @VALOR = (SELECT top (1) IdConvenio FROM T_ConveniosAplicados WHERE IdTransaccion= '" + IdTransaccion + "' and IdConvenio IS NOT NULL ORDER BY FechaAplicacion DESC) \n" +
            "IF NOT EXISTS (SELECT top (1) IdConvenio FROM T_ConveniosAplicados WHERE IdTransaccion= '" + IdTransaccion + "' and IdConvenio IS NOT NULL) \n" + "BEGIN \n"
            + "insert into T_ConveniosAplicados values('" + IdTransaccion + "','" + IdConvenio + "',GETDATE()) \n" +
            "SET @RSPTA = 'NO VALIDADO' \n" +
            "END \n" +
            "IF @VALOR <> '" + IdConvenio + "' \n" +
            "BEGIN \n" +
            "insert into T_ConveniosAplicados values('" + IdTransaccion + "','" + IdConvenio + "',GETDATE()) \n" +
            "SET @RSPTA = 'NO VALIDADO' \n" +
            "END \n" +
            "SELECT @RSPTA";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Check is the reader has any rows at all before starting to read.
                        if (reader.HasRows)
                        {
                            // Read advances to the next row.
                            while (reader.Read())
                            {
                                valor = reader[0].ToString();
                            }
                        }
                    }
                }
            }


            return valor;
        }

        [WebMethod]
        public AutorizadoxPlacaResponse BuscarAutorizadoxPlaca(string sPlaca)
        {

            AutorizadoxPlacaResponse array = new AutorizadoxPlacaResponse();

            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;
            string valor = string.Empty;
            string valor2 = string.Empty;


            query = "select documento, idAutorizacion, IdEstacionamiento, IdTarjeta from T_PersonasAutorizadas where Placa1 = '" + sPlaca + "' or Placa2 = '" + sPlaca + "' or Placa3 = '" + sPlaca + "' or Placa4 = '" + sPlaca + "' or Placa5 = '" + sPlaca + "'";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Check is the reader has any rows at all before starting to read.
                        if (reader.HasRows)
                        {
                            // Read advances to the next row.
                            while (reader.Read())
                            {

                                valor = reader["IdTarjeta"].ToString();
                                valor2 = reader["IdAutorizacion"].ToString();

                            }
                        }
                    }
                }
            }

            if (valor != string.Empty)
            {
                array.Exito = true;
                array.IdTarjeta = valor;
                array.IdAutorizacion = valor2;
            }
            else
            {
                array.Exito = false;
                array.ErrorMessage = "No se encontro autorizado.";
            }

            return array;
        }

    }

    public class EstacionamientosResponse
    {
        private bool _Exito = true;

        public bool Exito
        {
            get { return _Exito; }
            set { _Exito = value; }
        }

        private string _ErrorMessage = string.Empty;

        public string ErrorMessage
        {
            get { return _ErrorMessage; }
            set { _ErrorMessage = value; }
        }

        private List<InfoEstacionamientosResponse> _lstInfoEstacionamientosResponse = new List<InfoEstacionamientosResponse>();

        public List<InfoEstacionamientosResponse> LstInfoEstacionamientos
        {
            get { return _lstInfoEstacionamientosResponse; }
            set { _lstInfoEstacionamientosResponse = value; }
        }
    }

    public class InfoEstacionamientosResponse
    {
        private int _Value = 0;

        public int Value
        {
            get { return _Value; }
            set { _Value = value; }
        }
        private string _Display = string.Empty;

        public string Display
        {
            get { return _Display; }
            set { _Display = value; }
        }
    }

    public class SedesResponse
    {
        private bool _Exito = true;

        public bool Exito
        {
            get { return _Exito; }
            set { _Exito = value; }
        }

        private string _ErrorMessage = string.Empty;

        public string ErrorMessage
        {
            get { return _ErrorMessage; }
            set { _ErrorMessage = value; }
        }

        private List<InfoSedesResponse> _lstInfoSedesResponse = new List<InfoSedesResponse>();

        public List<InfoSedesResponse> LstInfoSedes
        {
            get { return _lstInfoSedesResponse; }
            set { _lstInfoSedesResponse = value; }
        }
    }

    public class InfoSedesResponse
    {
        private int _Value = 0;

        public int Value
        {
            get { return _Value; }
            set { _Value = value; }
        }
        private string _Display = string.Empty;

        public string Display
        {
            get { return _Display; }
            set { _Display = value; }
        }
    }

    public class VehiculosEnValetResponse
    {
        private bool _Exito = true;

        public bool Exito
        {
            get { return _Exito; }
            set { _Exito = value; }
        }

        private string _ErrorMessage = string.Empty;

        public string ErrorMessage
        {
            get { return _ErrorMessage; }
            set { _ErrorMessage = value; }
        }

        private List<InfoVehiculosEnValetResponse> _lstInfoVehiculosEnValet = new List<InfoVehiculosEnValetResponse>();

        public List<InfoVehiculosEnValetResponse> LstInfoVehiculosEnValet
        {
            get { return _lstInfoVehiculosEnValet; }
            set { _lstInfoVehiculosEnValet = value; }
        }
    }

    public class InfoVehiculosEnValetResponse
    {
        private string _IdTransaccion = string.Empty;

        public string IdTransaccion
        {
            get { return _IdTransaccion; }
            set { _IdTransaccion = value; }
        }
        private string _Placa = string.Empty;

        public string Placa
        {
            get { return _Placa; }
            set { _Placa = value; }
        }
        private string _Color = string.Empty;

        public string Color
        {
            get { return _Color; }
            set { _Color = value; }
        }
        private string _Marca = string.Empty;

        public string Marca
        {
            get { return _Marca; }
            set { _Marca = value; }
        }
        private string _Estado = string.Empty;

        public string Estado
        {
            get { return _Estado; }
            set { _Estado = value; }
        }
        private string _Ubicacion = string.Empty;

        public string Ubicacion
        {
            get { return _Ubicacion; }
            set { _Ubicacion = value; }
        }

        private string _IdEstacionamiento = string.Empty;

        public string IdEstacionamiento
        {
            get { return _IdEstacionamiento; }
            set { _IdEstacionamiento = value; }
        }
    }

    public class ActualizaVigenciaAutorizadoResponse
    {
        private bool _Exito = true;

        public bool Exito
        {
            get { return _Exito; }
            set { _Exito = value; }
        }

        private string _ErrorMessage = string.Empty;

        public string ErrorMessage
        {
            get { return _ErrorMessage; }
            set { _ErrorMessage = value; }
        }
    }

    public class CreaSalidaResponse
    {
        private bool _Exito = true;

        public bool Exito
        {
            get { return _Exito; }
            set { _Exito = value; }
        }

        private string _ErrorMessage = string.Empty;

        public string ErrorMessage
        {
            get { return _ErrorMessage; }
            set { _ErrorMessage = value; }
        }
    }

    public class VerificaVigenciaAutorizadoResponse
    {
        private bool _Exito = true;

        public bool Exito
        {
            get { return _Exito; }
            set { _Exito = value; }
        }

        private string _ErrorMessage = string.Empty;

        public string ErrorMessage
        {
            get { return _ErrorMessage; }
            set { _ErrorMessage = value; }
        }
    }

    public class VerificaTransaccionAbiertaAutorizadoResponse
    {
        private bool _Exito = true;

        public bool Exito
        {
            get { return _Exito; }
            set { _Exito = value; }
        }

        private string _ErrorMessage = string.Empty;

        public string ErrorMessage
        {
            get { return _ErrorMessage; }
            set { _ErrorMessage = value; }
        }
    }

    public class AutorizadoxPlacaResponse
    {
        private bool _Exito = true;

        public bool Exito
        {
            get { return _Exito; }
            set { _Exito = value; }
        }

        private string _ErrorMessage = string.Empty;

        public string ErrorMessage
        {
            get { return _ErrorMessage; }
            set { _ErrorMessage = value; }
        }

        private string _IdTarjeta = string.Empty;

        public string IdTarjeta
        {
            get { return _IdTarjeta; }
            set { _IdTarjeta = value; }
        }

        private string _IdAutorizacion = string.Empty;

        public string IdAutorizacion
        {
            get { return _IdAutorizacion; }
            set { _IdAutorizacion = value; }
        }
        //private string _IdEstacionamiento = string.Empty;
    }

    public class CreaEntradaResponse
    {
        private bool _Exito = true;

        public bool Exito
        {
            get { return _Exito; }
            set { _Exito = value; }
        }

        private string _ErrorMessage = string.Empty;

        public string ErrorMessage
        {
            get { return _ErrorMessage; }
            set { _ErrorMessage = value; }
        }
    }

    public class CarrilesEntrada
    {
        private int _Value = 0;

        public int Value
        {
            get { return _Value; }
            set { _Value = value; }
        }
        private string _Display = string.Empty;

        public string Display
        {
            get { return _Display; }
            set { _Display = value; }
        }
    }

    public class CarrilEntradaXEntradaResponse
    {
        private bool _Exito = true;

        public bool Exito
        {
            get { return _Exito; }
            set { _Exito = value; }
        }

        private string _ErrorMessage = string.Empty;

        public string ErrorMessage
        {
            get { return _ErrorMessage; }
            set { _ErrorMessage = value; }
        }

        private List<CarrilesEntrada> _LstCarrillesEntrada = new List<CarrilesEntrada>();

        public List<CarrilesEntrada> LstCarrillesEntrada
        {
            get { return _LstCarrillesEntrada; }
            set { _LstCarrillesEntrada = value; }
        }
    }

    public class ConfirmarCargaResponse
    {
        private bool _Exito = true;

        public bool Exito
        {
            get { return _Exito; }
            set { _Exito = value; }
        }

        private string _ErrorMessage = string.Empty;

        public string ErrorMessage
        {
            get { return _ErrorMessage; }
            set { _ErrorMessage = value; }
        }
    }

    public class RegistrarCargaResponse
    {
        private bool _Exito = true;

        public bool Exito
        {
            get { return _Exito; }
            set { _Exito = value; }
        }

        private string _ErrorMessage = string.Empty;

        public string ErrorMessage
        {
            get { return _ErrorMessage; }
            set { _ErrorMessage = value; }
        }

        private int _IdCarga = 0;

        public int IdCarga
        {
            get { return _IdCarga; }
            set { _IdCarga = value; }
        }
    }

    public class InfoItemsCargaResponse
    {
        private string _Nombre = string.Empty;

        public string Nombre
        {
            get { return _Nombre; }
            set { _Nombre = value; }
        }
        private string _Telefono = string.Empty;

        public string Telefono
        {
            get { return _Telefono; }
            set { _Telefono = value; }
        }
        private string _Direccion = string.Empty;

        public string Direccion
        {
            get { return _Direccion; }
            set { _Direccion = value; }
        }
        private string _Modulo = string.Empty;

        public string Modulo
        {
            get { return _Modulo; }
            set { _Modulo = value; }
        }
        private string _Fecha = string.Empty;

        public string Fecha
        {
            get { return _Fecha; }
            set { _Fecha = value; }
        }
        private string _IdCarga = string.Empty;

        public string IdCarga
        {
            get { return _IdCarga; }
            set { _IdCarga = value; }
        }
        private string _Valor = string.Empty;

        public string Valor
        {
            get { return _Valor; }
            set { _Valor = value; }
        }
        private string _IdUsuario = string.Empty;

        public string IdUsuario
        {
            get { return _IdUsuario; }
            set { _IdUsuario = value; }
        }
        private string _IdEstacionamiento = string.Empty;

        public string IdEstacionamiento
        {
            get { return _IdEstacionamiento; }
            set { _IdEstacionamiento = value; }
        }


    }

    public class InfoCargaResponse
    {
        private bool _Exito = true;

        public bool Exito
        {
            get { return _Exito; }
            set { _Exito = value; }
        }

        private string _ErrorMessage = string.Empty;

        public string ErrorMessage
        {
            get { return _ErrorMessage; }
            set { _ErrorMessage = value; }
        }

        private List<InfoItemsCargaResponse> _lstInfoCargas = new List<InfoItemsCargaResponse>();

        public List<InfoItemsCargaResponse> LstInfoCargas
        {
            get { return _lstInfoCargas; }
            set { _lstInfoCargas = value; }
        }

    }

    public class InfoItemsArqueoResponse
    {
        private string _Nombre = string.Empty;

        public string Nombre
        {
            get { return _Nombre; }
            set { _Nombre = value; }
        }
        private string _Telefono = string.Empty;

        public string Telefono
        {
            get { return _Telefono; }
            set { _Telefono = value; }
        }
        private string _Direccion = string.Empty;

        public string Direccion
        {
            get { return _Direccion; }
            set { _Direccion = value; }
        }
        private string _Modulo = string.Empty;

        public string Modulo
        {
            get { return _Modulo; }
            set { _Modulo = value; }
        }
        private string _Fecha = string.Empty;

        public string Fecha
        {
            get { return _Fecha; }
            set { _Fecha = value; }
        }
        private string _IdArqueo = string.Empty;

        public string IdArqueo
        {
            get { return _IdArqueo; }
            set { _IdArqueo = value; }
        }
        private string _Producido = string.Empty;

        public string Producido
        {
            get { return _Producido; }
            set { _Producido = value; }
        }
        private string _CantTransacciones = string.Empty;

        public string CantTransacciones
        {
            get { return _CantTransacciones; }
            set { _CantTransacciones = value; }
        }
        private string _Valor = string.Empty;

        public string Valor
        {
            get { return _Valor; }
            set { _Valor = value; }
        }

        private string _Conteo = string.Empty;
        public string Conteo
        {
            get { return _Conteo; }
            set { _Conteo = value; }
        }

        private string _IdUsuario = string.Empty;
        public string IdUsuario
        {
            get { return _IdUsuario; }
            set { _IdUsuario = value; }
        }
 
    }

    public class InfoArqueoResponse
    {
        private bool _Exito = true;

        public bool Exito
        {
            get { return _Exito; }
            set { _Exito = value; }
        }

        private string _ErrorMessage = string.Empty;

        public string ErrorMessage
        {
            get { return _ErrorMessage; }
            set { _ErrorMessage = value; }
        }

        private List<InfoItemsArqueoResponse> _lstInfoArqueos = new List<InfoItemsArqueoResponse>();

        public List<InfoItemsArqueoResponse> LstInfoArqueos
        {
            get { return _lstInfoArqueos; }
            set { _lstInfoArqueos = value; }
        }
        
    }

    public class ConfirmarArqueoResponse
    {
        private bool _Exito = true;

        public bool Exito
        {
            get { return _Exito; }
            set { _Exito = value; }
        }

        private string _ErrorMessage = string.Empty;

        public string ErrorMessage
        {
            get { return _ErrorMessage; }
            set { _ErrorMessage = value; }
        }
    }

    public class RegistrarArqueoResponse
    {
        private bool _Exito = true;

        public bool Exito
        {
            get { return _Exito; }
            set { _Exito = value; }
        }

        private string _ErrorMessage = string.Empty;

        public string ErrorMessage
        {
            get { return _ErrorMessage; }
            set { _ErrorMessage = value; }
        }

        private int _IdArqueo = 0;

        public int IdArqueo
        {
            get { return _IdArqueo; }
            set { _IdArqueo = value; }
        }
    }

    public class DescripcionConvenioResponse
    {
        private bool _Exito = true;

        public bool Exito
        {
            get { return _Exito; }
            set { _Exito = value; }
        }

        private string _ErrorMessage = string.Empty;

        public string ErrorMessage
        {
            get { return _ErrorMessage; }
            set { _ErrorMessage = value; }
        }

        private string _Descripcion = string.Empty;

        public string Descripcion
        {
            get { return _Descripcion; }
            set { _Descripcion = value; }
        }
    }

    public class InfoUsuarioResponse
    {
        private bool _Exito = true;

        public bool Exito
        {
            get { return _Exito; }
            set { _Exito = value; }
        }

        private string _ErrorMessage = string.Empty;

        public string ErrorMessage
        {
            get { return _ErrorMessage; }
            set { _ErrorMessage = value; }
        }

        private string _Documento = string.Empty;

        public string Documento
        {
            get { return _Documento; }
            set { _Documento = value; }
        }
        private string _Nombres = string.Empty;

        public string Nombres
        {
            get { return _Nombres; }
            set { _Nombres = value; }
        }
        private string _Usuario = string.Empty;

        public string Usuario
        {
            get { return _Usuario; }
            set { _Usuario = value; }
        }
        private string _Cargo = string.Empty;

        public string Cargo
        {
            get { return _Cargo; }
            set { _Cargo = value; }
        }

    }

    public class LoginResponse
    {
        private bool _Exito = true;

        public bool Exito
        {
            get { return _Exito; }
            set { _Exito = value; }
        }

        private string _ErrorMessage = string.Empty;

        public string ErrorMessage
        {
            get { return _ErrorMessage; }
            set { _ErrorMessage = value; }
        }

        private string _Documento = string.Empty;

        public string Documento
        {
            get { return _Documento; }
            set { _Documento = value; }
        }

        private string _Clave = string.Empty;

        public string Clave
        {
            get { return _Clave; }
            set { _Clave = value; }
        }
    }

    public class EventosResponse
    {
        private bool _Exito = true;

        public bool Exito
        {
            get { return _Exito; }
            set { _Exito = value; }
        }

        private string _ErrorMessage = string.Empty;

        public string ErrorMessage
        {
            get { return _ErrorMessage; }
            set { _ErrorMessage = value; }
        }

        private List<InfoEventosResponse> _lstInfoEventos = new List<InfoEventosResponse>();

        public List<InfoEventosResponse> LstInfoEventos
        {
            get { return _lstInfoEventos; }
            set { _lstInfoEventos = value; }
        }
    }

    public class ConveniosResponse
    {
        private bool _Exito = true;

        public bool Exito
        {
            get { return _Exito; }
            set { _Exito = value; }
        }

        private string _ErrorMessage = string.Empty;

        public string ErrorMessage
        {
            get { return _ErrorMessage; }
            set { _ErrorMessage = value; }
        }

        private List<InfoConveniosResponse> _lstInfoConvenios = new List<InfoConveniosResponse>();

        public List<InfoConveniosResponse> LstInfoConvenios
        {
            get { return _lstInfoConvenios; }
            set { _lstInfoConvenios = value; }
        }
    }

    public class InfoEventosResponse
    {
        private int _Value = 0;

        public int Value
        {
            get { return _Value; }
            set { _Value = value; }
        }
        private string _Display = string.Empty;

        public string Display
        {
            get { return _Display; }
            set { _Display = value; }
        }
    }

    public class InfoConveniosResponse
    {
        private int _Value = 0;

        public int Value
        {
            get { return _Value; }
            set { _Value = value; }
        }
        private string _Display = string.Empty;

        public string Display
        {
            get { return _Display; }
            set { _Display = value; }
        }
    }

    public class InfoMotivosCortesiaResponse
    {
        private int _Value = 0;

        public int Value
        {
            get { return _Value; }
            set { _Value = value; }
        }
        private string _Display = string.Empty;

        public string Display
        {
            get { return _Display; }
            set { _Display = value; }
        }
    }

    public class CarrilxIdModuloResponse
    {
        private bool _Exito = true;

        public bool Exito
        {
            get { return _Exito; }
            set { _Exito = value; }
        }

        private string _ErrorMessage = string.Empty;

        public string ErrorMessage
        {
            get { return _ErrorMessage; }
            set { _ErrorMessage = value; }
        }

        private int _Carril = 0;

        public int Carril
        {
            get { return _Carril; }
            set { _Carril = value; }
        }
    }

    public class MotivosCortesiaResponse
    {
        private bool _Exito = true;

        public bool Exito
        {
            get { return _Exito; }
            set { _Exito = value; }
        }

        private string _ErrorMessage = string.Empty;

        public string ErrorMessage
        {
            get { return _ErrorMessage; }
            set { _ErrorMessage = value; }
        }

        private List<InfoMotivosCortesiaResponse> _lstMotivosCortesia = new List<InfoMotivosCortesiaResponse>();

        public List<InfoMotivosCortesiaResponse> LstMotivosCortesia
        {
            get { return _lstMotivosCortesia; }
            set { _lstMotivosCortesia = value; }
        }
    }

    public class AplicarEventoResponse
    {
        private bool _Exito = true;

        public bool Exito
        {
            get { return _Exito; }
            set { _Exito = value; }
        }

        private string _ErrorMessage = string.Empty;

        public string ErrorMessage
        {
            get { return _ErrorMessage; }
            set { _ErrorMessage = value; }
        }
    }

    public class AplicarCortesiaResponse
    {
        private bool _Exito = true;

        public bool Exito
        {
            get { return _Exito; }
            set { _Exito = value; }
        }

        private string _ErrorMessage = string.Empty;

        public string ErrorMessage
        {
            get { return _ErrorMessage; }
            set { _ErrorMessage = value; }
        }
    }

    public class AplicarConvenioResponse
    {
        private bool _Exito = true;

        public bool Exito
        {
            get { return _Exito; }
            set { _Exito = value; }
        }

        private string _ErrorMessage = string.Empty;

        public string ErrorMessage
        {
            get { return _ErrorMessage; }
            set { _ErrorMessage = value; }
        }
    }

    public class AplicaCascoResponse
    {
        private bool _Exito = true;

        public bool Exito
        {
            get { return _Exito; }
            set { _Exito = value; }
        }

        private string _ErrorMessage = string.Empty;

        public string ErrorMessage
        {
            get { return _ErrorMessage; }
            set { _ErrorMessage = value; }
        }
    }

    public class SaveConveniosResponse
    {
        private bool _Exito = true;

        public bool Exito
        {
            get { return _Exito; }
            set { _Exito = value; }
        }

        private string _ErrorMessage = string.Empty;

        public string ErrorMessage
        {
            get { return _ErrorMessage; }
            set { _ErrorMessage = value; }
        }
    }

    public class AplicarMotoResponse
    {
        private bool _Exito = true;

        public bool Exito
        {
            get { return _Exito; }
            set { _Exito = value; }
        }

        private string _ErrorMessage = string.Empty;

        public string ErrorMessage
        {
            get { return _ErrorMessage; }
            set { _ErrorMessage = value; }
        }
    }

    public class InfoItemsFacturaMensualidadResponse
    {
        private string _NumeroFactura = string.Empty;

        public string NumeroFactura
        {
            get { return _NumeroFactura; }
            set { _NumeroFactura = value; }
        }
        private string _Nombre = string.Empty;

        public string Nombre
        {
            get { return _Nombre; }
            set { _Nombre = value; }
        }
        private string _Telefono = string.Empty;

        public string Telefono
        {
            get { return _Telefono; }
            set { _Telefono = value; }
        }
        private string _Direccion = string.Empty;

        public string Direccion
        {
            get { return _Direccion; }
            set { _Direccion = value; }
        }
        private string _Modulo = string.Empty;

        public string Modulo
        {
            get { return _Modulo; }
            set { _Modulo = value; }
        }
        private string _Fecha = string.Empty;

        public string Fecha
        {
            get { return _Fecha; }
            set { _Fecha = value; }
        }
        private string _IdTransaccion = string.Empty;

        public string IdTransaccion
        {
            get { return _IdTransaccion; }
            set { _IdTransaccion = value; }
        }
        private string _Tipo = string.Empty;

        public string Tipo
        {
            get { return _Tipo; }
            set { _Tipo = value; }
        }
        private string _Total = string.Empty;

        public string Total
        {
            get { return _Total; }
            set { _Total = value; }
        }
        private string _Subtotal = string.Empty;

        public string Subtotal
        {
            get { return _Subtotal; }
            set { _Subtotal = value; }
        }
        private string _Iva = string.Empty;

        public string Iva
        {
            get { return _Iva; }
            set { _Iva = value; }
        }
        private string _NumeroResolucion = string.Empty;

        public string NumeroResolucion
        {
            get { return _NumeroResolucion; }
            set { _NumeroResolucion = value; }
        }
        private string _NombreAutorizacion = string.Empty;

        public string NombreAutorizacion
        {
            get { return _NombreAutorizacion; }
            set { _NombreAutorizacion = value; }
        }
        private string _Documento = string.Empty;

        public string Documento
        {
            get { return _Documento; }
            set { _Documento = value; }
        }


        private string _Nit = string.Empty;

        public string Nit
        {
            get { return _Nit; }
            set { _Nit = value; }
        }

        private string _NombreEmpresa = string.Empty;

        public string NombreEmpresa
        {
            get { return _NombreEmpresa; }
            set { _NombreEmpresa = value; }
        }

        private string _NombreAutorizado = string.Empty;

        public string NombreAutorizado
        {
            get { return _NombreAutorizado; }
            set { _NombreAutorizado = value; }
        }

        private string _Vigencia = string.Empty;

        public string Vigencia
        {
            get { return _Vigencia; }
            set { _Vigencia = value; }
        }
    }

    public class InfoPagoMensualidadService
    {
        private bool _Exito = true;

        public bool Exito
        {
            get { return _Exito; }
            set { _Exito = value; }
        }

        private string _ErrorMessage = string.Empty;

        public string ErrorMessage
        {
            get { return _ErrorMessage; }
            set { _ErrorMessage = value; }
        }

        private string _IdTranaccion = string.Empty;

        public string IdTranaccion
        {
            get { return _IdTranaccion; }
            set { _IdTranaccion = value; }
        }
        private string _IdAutorizacion = string.Empty;

        public string IdAutorizacion
        {
            get { return _IdAutorizacion; }
            set { _IdAutorizacion = value; }
        }

        

    }

    public class LiquidacionService
    {
        private bool _Exito = true;

        public bool Exito
        {
            get { return _Exito; }
            set { _Exito = value; }
        }

        private string _ErrorMessage = string.Empty;

        public string ErrorMessage
        {
            get { return _ErrorMessage; }
            set { _ErrorMessage = value; }
        }

        private List<DatosLiquidacionService> _lstLiquidacion = new List<DatosLiquidacionService>();

        public List<DatosLiquidacionService> LstLiquidacion
        {
            get { return _lstLiquidacion; }
            set { _lstLiquidacion = value; }
        }
    }

    public class InfoItemsFacturaResponse
    {
        private string _NumeroFactura = string.Empty;

        public string NumeroFactura
        {
            get { return _NumeroFactura; }
            set { _NumeroFactura = value; }
        }

        private string _Nombre = string.Empty;

        public string Nombre
        {
            get { return _Nombre; }
            set { _Nombre = value; }
        }

        private string _Telefono = string.Empty;

        public string Telefono
        {
            get { return _Telefono; }
            set { _Telefono = value; }
        }

        private string _Direccion = string.Empty;

        public string Direccion
        {
            get { return _Direccion; }
            set { _Direccion = value; }
        }

        private string _Modulo = string.Empty;

        public string Modulo
        {
            get { return _Modulo; }
            set { _Modulo = value; }
        }

        private string _Fecha = string.Empty;

        public string Fecha
        {
            get { return _Fecha; }
            set { _Fecha = value; }
        }

        private string _IdTransaccion = string.Empty;

        public string IdTransaccion
        {
            get { return _IdTransaccion; }
            set { _IdTransaccion = value; }
        }

        private string _Placa = string.Empty;

        public string Placa
        {
            get { return _Placa; }
            set { _Placa = value; }
        }

        private string _Tipo = string.Empty;

        public string Tipo
        {
            get { return _Tipo; }
            set { _Tipo = value; }
        }

        private string _Total = string.Empty;

        public string Total
        {
            get { return _Total; }
            set { _Total = value; }
        }

        private string _ValorRecibido = string.Empty;

        public string ValorRecibido
        {
            get { return _ValorRecibido; }
            set { _ValorRecibido = value; }
        }

        private string _Cambio = string.Empty;

        public string Cambio
        {
            get { return _Cambio; }
            set { _Cambio = value; }
        }

        private string _Subtotal = string.Empty;

        public string Subtotal
        {
            get { return _Subtotal; }
            set { _Subtotal = value; }
        }

        private string _Iva = string.Empty;

        public string Iva
        {
            get { return _Iva; }
            set { _Iva = value; }
        }

        private string _NumeroResolucion = string.Empty;

        public string NumeroResolucion
        {
            get { return _NumeroResolucion; }
            set { _NumeroResolucion = value; }
        }

        private string _FechaEntrada = string.Empty;

        public string FechaEntrada
        {
            get { return _FechaEntrada; }
            set { _FechaEntrada = value; }
        }

        private string _TipoVehiculo = string.Empty;

        public string TipoVehiculo
        {
            get { return _TipoVehiculo; }
            set { _TipoVehiculo = value; }
        }

        private string _Cantidad = string.Empty;

        public string Cantidad
        {
            get { return _Cantidad; }
            set { _Cantidad = value; }
        }

        private string _Vigencia = string.Empty;

        public string Vigencia
        {
            get { return _Vigencia; }
            set { _Vigencia = value; }
        }
    }

    public class InfoFacturaResponse
    {
        private bool _Exito = true;

        public bool Exito
        {
            get { return _Exito; }
            set { _Exito = value; }
        }

        private string _ErrorMessage = string.Empty;

        public string ErrorMessage
        {
            get { return _ErrorMessage; }
            set { _ErrorMessage = value; }
        }

        private List<InfoItemsFacturaResponse> _lstItems = new List<InfoItemsFacturaResponse>();

        public List<InfoItemsFacturaResponse> LstItems
        {
            get { return _lstItems; }
            set { _lstItems = value; }
        }

        private List<InfoItemsFacturaMensualidadResponse> _lstItemsMensualidad = new List<InfoItemsFacturaMensualidadResponse>();

        public List<InfoItemsFacturaMensualidadResponse> LstItemsMensualidad
        {
            get { return _lstItemsMensualidad; }
            set { _lstItemsMensualidad = value; }
        }
    }

    public class DatosLiquidacionService
    {
        private int _Tipo = 0;

        public int Tipo
        {
            get { return _Tipo; }
            set { _Tipo = value; }
        }
        private double _SubTotal = 0;

        public double SubTotal
        {
            get { return _SubTotal; }
            set { _SubTotal = value; }
        }
        private double _Iva = 0;

        public double Iva
        {
            get { return _Iva; }
            set { _Iva = value; }
        }
        private double _Total = 0;

        public double Total
        {
            get { return _Total; }
            set { _Total = value; }
        }
    }

    public class InfoPagoNormalService
    {
        private bool _Exito = true;

        public bool Exito
        {
            get { return _Exito; }
            set { _Exito = value; }
        }

        private string _ErrorMessage = string.Empty;

        public string ErrorMessage
        {
            get { return _ErrorMessage; }
            set { _ErrorMessage = value; }
        }
    }

    public class InfoPPMService
    {
        private bool _Exito = true;

        public bool Exito
        {
            get { return _Exito; }
            set { _Exito = value; }
        }

        private string _ErrorMessage = string.Empty;

        public string ErrorMessage
        {
            get { return _ErrorMessage; }
            set { _ErrorMessage = value; }
        }

        private string _Modulo = string.Empty;

        public string Modulo
        {
            get { return _Modulo; }
            set { _Modulo = value; }
        }

        private string _Estacionamiento = string.Empty;

        public string Estacionamiento
        {
            get { return _Estacionamiento; }
            set { _Estacionamiento = value; }
        }

        private string _Sede = string.Empty;

        public string Sede
        {
            get { return _Sede; }
            set { _Sede = value; }
        }

        private string _IdEstacionamiento = string.Empty;

        public string IdEstacionamiento
        {
            get { return _IdEstacionamiento; }
            set { _IdEstacionamiento = value; }
        }

        private string _IdSede = string.Empty;

        public string IdSede
        {
            get { return _IdSede; }
            set { _IdSede = value; }
        }
    }

    public class InfoItemsTransaccionService
    {
        private string _Casillero = string.Empty;

        public string Casillero
        {
            get { return _Casillero; }
            set { _Casillero = value; }
        }
    }
    public class InfoTransaccionService
    {
        private bool _Exito = true;

        public bool Exito
        {
            get { return _Exito; }
            set { _Exito = value; }
        }

        private string _ErrorMessage = string.Empty;

        public string ErrorMessage
        {
            get { return _ErrorMessage; }
            set { _ErrorMessage = value; }
        }

        private string _IdTransaccion = string.Empty;

        public string IdTransaccion
        {
            get { return _IdTransaccion; }
            set { _IdTransaccion = value; }
        }

        private string _Casillero = string.Empty;

        public string Casillero
        {
            get { return _Casillero; }
            set { _Casillero = value; }
        }

        private string _HoraTransaccion = string.Empty;

        public string HoraTransaccion
        {
            get { return _HoraTransaccion; }
            set { _HoraTransaccion = value; }
        }

        private List<InfoItemsTransaccionService> _lstTransac = new List<InfoItemsTransaccionService>();

        public List<InfoItemsTransaccionService> LstTransac
        {
            get { return _lstTransac; }
            set { _lstTransac = value; }
        }

        public class AutorizadoxPlacaResponse
        {
            private bool _Exito = true;

            public bool Exito
            {
                get { return _Exito; }
                set { _Exito = value; }
            }

            private string _ErrorMessage = string.Empty;

            public string ErrorMessage
            {
                get { return _ErrorMessage; }
                set { _ErrorMessage = value; }
            }

            private string _IdTarjeta = string.Empty;

            public string IdTarjeta
            {
                get { return _IdTarjeta; }
                set { _IdTarjeta = value; }
            }

            private string _IdAutorizacion = string.Empty;

            public string IdAutorizacion
            {
                get { return _IdAutorizacion; }
                set { _IdAutorizacion = value; }
            }
            //private string _IdEstacionamiento = string.Empty;
        }
    }
}