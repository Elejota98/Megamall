using BlockAndPass.AdminWeb.LiquidacionByPReference;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

namespace BlockAndPass.AdminWeb
{
    /// <summary>
    /// Descripción breve de DataService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    [System.Web.Script.Services.ScriptService]
    public class DataService : System.Web.Services.WebService
    {
        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string CerrarArqueo()
        {

            string idEstacionamiento = HttpContext.Current.Request.Params["idEstacionamiento"];
            string idArqueo = HttpContext.Current.Request.Params["idArqueo"];
            string valorCierre = HttpContext.Current.Request.Params["valorCierre"];

            string response = string.Empty;
            DataBaseResponse oDataBaseResponse = new DataBaseResponse();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;

            query = "update t_arqueos set Conteo='" + valorCierre + "' where idarqueo=" + idArqueo + " and idestacionamiento=" + idEstacionamiento;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();

                    int result2 = Convert.ToInt32(cmd.ExecuteNonQuery());

                    if (result2 > 0)
                    {
                        oDataBaseResponse.Resultado = result2;
                        RegistrarAuditoria(TipoAccion.update, "t_arqueos", query);
                    }
                    else
                    {
                        oDataBaseResponse.Exito = false;
                        oDataBaseResponse.ErrorMessage = "No se puede cerrar el arqueo";
                    }
                }
            }

            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            response = javaScriptSerializer.Serialize(oDataBaseResponse);

            return response;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string RegistrarEventoDispositivo()
        {
            string idEstacionamiento = HttpContext.Current.Request.Params["idEstacionamiento"];
            string idModulo = HttpContext.Current.Request.Params["idModulo"];
            string observacion = HttpContext.Current.Request.Params["observacion"];

            string response = string.Empty;
            DataBaseResponse oDataBaseResponse = new DataBaseResponse();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string user = HttpContext.Current.User.Identity.Name.ToString();

            string query = string.Empty;

            query = "insert into T_EventoDispositivo (IdEstacionamiento, IdModulo, Usuario, Observacion, FechaHora) " +
                    "values ('" + idEstacionamiento + "','" + idModulo + "','" + user + "', '" + observacion + "',getdate())";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    int result = cmd.ExecuteNonQuery();
                    if (result <= 0)
                    {
                        oDataBaseResponse.Exito = false;
                        oDataBaseResponse.ErrorMessage = "No fue posible insertar el registro.";
                    }
                    else
                    {
                        RegistrarAuditoria(TipoAccion.insert, "T_EventoDispositivo", query);
                    }
                }
            }


            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            response = javaScriptSerializer.Serialize(oDataBaseResponse);

            return response;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string RegistrarCarga()
        {
            string idEstacionamiento = HttpContext.Current.Request.Params["idEstacionamiento"];
            string idModulo = HttpContext.Current.Request.Params["idModulo"];

            string response = string.Empty;
            DataBaseResponse oDataBaseResponse = new DataBaseResponse();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string user = HttpContext.Current.User.Identity.Name.ToString();

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
                        oDataBaseResponse.Resultado = result2;
                        RegistrarAuditoria(TipoAccion.insert, "t_carga", query);
                    }
                    else
                    {
                        oDataBaseResponse.Exito = false;
                        oDataBaseResponse.ErrorMessage = "No se puede insertar carga";
                    }
                }
            }


            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            response = javaScriptSerializer.Serialize(oDataBaseResponse);

            return response;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string ConfirmarCarga()
        {

            string idEstacionamiento = HttpContext.Current.Request.Params["idEstacionamiento"];
            string idModulo = HttpContext.Current.Request.Params["idModulo"];
            string idCarga = HttpContext.Current.Request.Params["idCarga"];
            string valor = HttpContext.Current.Request.Params["valor"];

            string response = string.Empty;
            DataBaseResponse oDataBaseResponse = new DataBaseResponse();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string sQuery1 = string.Empty;
            string sQuery2 = string.Empty;
            string sQuery3 = string.Empty;

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

                    sQuery1 = "Insert into T_Movimientos (IdCarga, IdEstacionamiento, IdModulo, Parte, Accion, Denominacion, Cantidad, Valor, FechaMovimiento) VALUES "
                            + "('" + idCarga + "','" + idEstacionamiento + "','" + idModulo + "','CM','Entrada',1,'1'," + valor + ",GETDATE())"; ;
                    sQuery2 = "update T_Carga set FechaFin=GetDate(), Valor=" + valor + " where IdModulo='" + idModulo + "' and IdEstacionamiento='" + idEstacionamiento + "' and idCarga=" + idCarga;
                    sQuery3 = "update T_Partes set DineroActual=(select DineroActual from t_partes where NombreParte='cm' and IdEstacionamiento='" + idEstacionamiento + "' and IdModulo='" + idModulo + "')+" + valor + "  where IdModulo='" + idModulo + "' and IdEstacionamiento='" + idEstacionamiento + "' and NombreParte='CM'";

                    command.CommandText = sQuery1;

                    command.ExecuteNonQuery();

                    command.CommandText = sQuery2;

                    command.ExecuteNonQuery();

                    command.CommandText = sQuery3;

                    command.ExecuteNonQuery();


                    // Attempt to commit the transaction.
                    transaction.Commit();

                }
                catch (Exception ex)
                {
                    oDataBaseResponse.Exito = false;
                    oDataBaseResponse.ErrorMessage = "No guarda modificaciones en BD.";
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

            if (oDataBaseResponse.Exito)
            {
                RegistrarAuditoria(TipoAccion.insert, "t_movimientos", sQuery1);
                RegistrarAuditoria(TipoAccion.update, "t_carga", sQuery2);
                RegistrarAuditoria(TipoAccion.update, "t_partes", sQuery3);
            }

            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            response = javaScriptSerializer.Serialize(oDataBaseResponse);

            return response;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string RegistrarArqueo()
        {
            string idEstacionamiento = HttpContext.Current.Request.Params["idEstacionamiento"];
            string idModulo = HttpContext.Current.Request.Params["idModulo"];

            string response = string.Empty;
            DataBaseResponse oDataBaseResponse = new DataBaseResponse();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string user = HttpContext.Current.User.Identity.Name.ToString();

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
                        oDataBaseResponse.Resultado = result2;
                        RegistrarAuditoria(TipoAccion.insert, "t_arqueos", query);
                    }
                    else
                    {
                        oDataBaseResponse.Exito = false;
                        oDataBaseResponse.ErrorMessage = "No se puede insertar arqueo";
                    }
                }
            }


            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            response = javaScriptSerializer.Serialize(oDataBaseResponse);

            return response;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string ConfirmarArqueo()
        {

            string idEstacionamiento = HttpContext.Current.Request.Params["idEstacionamiento"];
            string idModulo = HttpContext.Current.Request.Params["idModulo"];
            string idArqueo = HttpContext.Current.Request.Params["idArqueo"];

            string response = string.Empty;
            DataBaseResponse oDataBaseResponse = new DataBaseResponse();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;

            string FechaAntes = string.Empty;
            string FechaDespues = string.Empty;

            query = "select max(FechaInicio),getdate() " +
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
                            RegistrarAuditoria(TipoAccion.select, "t_arqueos", query);
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

            string sQuery1 = string.Empty;
            string sQuery2 = string.Empty;
            string sQuery3 = string.Empty;

            string queryCantidad = "(select (case when COUNT(IdTipoPago) is null then 0 else COUNT(IdTipoPago) end) " +
                                  "from T_Pagos as P inner join T_Transacciones as T on T.IdTransaccion = P.IdTransaccion " +
                                  "where  P.IdModulo = '" + idModulo + "' and T.IdEstacionamiento='" + idEstacionamiento + "'" +
                                  "and P.FechaPago between GetDate() and (select max(FechaInicio) from T_Arqueos where IdModulo = '" + idModulo + "' and Valor != 0 and IdEstacionamiento='" + idEstacionamiento + "' ))";

            string queryValor = "(select (case when SUM(P.Total) is null then 0 else SUM(P.Total) end)  " +
                                  "from T_Pagos as P inner join T_Transacciones as T on T.IdTransaccion = P.IdTransaccion " +
                                  "where  P.IdModulo = '" + idModulo + "' and T.IdEstacionamiento='" + idEstacionamiento + "'" +
                                  "and P.FechaPago between GetDate() and (select max(FechaInicio) from T_Arqueos where IdModulo = '" + idModulo + "' and Valor != 0 and IdEstacionamiento='" + idEstacionamiento + "' ))";

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
                    sQuery1 = "Insert into T_Movimientos (IdArqueo, IdEstacionamiento, IdModulo, Parte, Accion, Denominacion, Cantidad, Valor, FechaMovimiento) VALUES "
                            + "('" + idArqueo + "','" + idEstacionamiento + "','" + idModulo + "','CM','Salida',1,'1',(select DineroActual from T_Partes where IdModulo='" + idModulo + "' and IdEstacionamiento='" + idEstacionamiento + "' and NombreParte='CM'),GETDATE())";
                    sQuery2 = "update T_Arqueos set FechaFin=GetDate(), Valor=(select DineroActual from T_Partes where IdModulo='" + idModulo + "' and IdEstacionamiento='" + idEstacionamiento + "' and NombreParte='CM'), CantTransacciones=" + queryCantidad + " , Producido=" + queryValor + " where IdModulo='" + idModulo + "' and IdEstacionamiento='" + idEstacionamiento + "' and idArqueo=" + idArqueo;
                    sQuery3 = "update T_Partes set DineroActual=0 where IdModulo='" + idModulo + "' and IdEstacionamiento='" + idEstacionamiento + "' and NombreParte='CM'";

                    command.CommandText = sQuery1;

                    command.ExecuteNonQuery();

                    command.CommandText = sQuery2;

                    command.ExecuteNonQuery();

                    command.CommandText = sQuery3;

                    command.ExecuteNonQuery();


                    // Attempt to commit the transaction.
                    transaction.Commit();

                }
                catch (Exception ex)
                {
                    oDataBaseResponse.Exito = false;
                    oDataBaseResponse.ErrorMessage = "No guarda modificaciones en BD.";
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

            if (oDataBaseResponse.Exito)
            {
                RegistrarAuditoria(TipoAccion.insert, "t_movimientos", sQuery1);
                RegistrarAuditoria(TipoAccion.update, "t_arqueos", sQuery2);
                RegistrarAuditoria(TipoAccion.update, "t_partes", sQuery3);
            }


            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            response = javaScriptSerializer.Serialize(oDataBaseResponse);

            return response;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string ConsultarReporte()
        {
            string idReporte = HttpContext.Current.Request.Params["idReporte"];

            string response = string.Empty;
            DataBaseResponse oDataBaseResponse = new DataBaseResponse();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;

            query = "select top (1) * from T_Reportes where IdReporte='" + idReporte + "'";

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
                            RegistrarAuditoria(TipoAccion.select, "t_reportes", query);
                            // Read advances to the next row.
                            while (reader.Read())
                            {
                                oDataBaseResponse.Resultado = new
                                {
                                    IdReporte = reader[0].ToString(),
                                    Descripcion = reader[1].ToString(),
                                    ColumnasConsulta = reader[2].ToString(),
                                    ColumnasTabla = reader[3].ToString(),
                                    FromTabla = reader[4].ToString(),
                                    Columna0 = reader[5].ToString(),
                                    Nombre = reader[6].ToString(),
                                    Group = reader[7].ToString()
                                };
                            }
                        }
                        else
                        {
                            oDataBaseResponse.Exito = false;
                            oDataBaseResponse.ErrorMessage = "No encontro registro.";
                        }
                    }
                }
            }

            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            response = javaScriptSerializer.Serialize(oDataBaseResponse);

            return response;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string ConsultarReporte2()
        {
            string idReporte = HttpContext.Current.Request.Params["idReporte"];

            string response = string.Empty;
            DataBaseResponse oDataBaseResponse = new DataBaseResponse();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;

            query = "select top (1) * from T_Reportes where IdReporte='" + idReporte + "'";

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
                                RegistrarAuditoria(TipoAccion.select, "t_reportes", query);
                                oDataBaseResponse.Resultado = new
                                {
                                    IdReporte = reader[0].ToString(),
                                    Descripcion = reader[1].ToString(),
                                    ColumnasConsulta = reader[2].ToString(),
                                    ColumnasTabla = reader[3].ToString(),
                                    FromTabla = reader[4].ToString(),
                                    Columna0 = reader[5].ToString(),
                                    Nombre = reader[6].ToString(),
                                    //Group = reader[7].ToString(),
                                    Url = reader[7].ToString()
                                };
                            }
                        }
                        else
                        {
                            oDataBaseResponse.Exito = false;
                            oDataBaseResponse.ErrorMessage = "No encontro registro.";
                        }
                    }
                }
            }

            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            response = javaScriptSerializer.Serialize(oDataBaseResponse);

            return response;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string PagarMensualidad()
        {
            string pagosstring = HttpContext.Current.Request.Params["pagos"];

            ArrayList pagosFinal = new ArrayList();

            string idEstacionamiento = HttpContext.Current.Request.Params["idEstacionamiento"];
            string idModulo = HttpContext.Current.Request.Params["idModulo"];
            string fecha = HttpContext.Current.Request.Params["fecha"];
            string total = HttpContext.Current.Request.Params["total"];
            string idTarjeta = HttpContext.Current.Request.Params["idTarjeta"];

            //if (fecha.Split(' ').Length > 2)
            //{
            //    if (fecha.Split(' ')[2][0] == 'p')
            //    {
            //        int horaFinal = Convert.ToInt32(fecha.Split(' ')[1].Split(':')[0]);
            //        if (Convert.ToInt32(fecha.Split(' ')[1].Split(':')[0]) != 12)
            //        {
            //            horaFinal = Convert.ToInt32(fecha.Split(' ')[1].Split(':')[0]) + 12;
            //        }

            //        fecha = fecha.Split(' ')[0] + " " + horaFinal + ":" + fecha.Split(' ')[1].Split(':')[1] + ":" + fecha.Split(' ')[1].Split(':')[2];
            //    }
            //    else
            //    {
            //        int horaFinal = Convert.ToInt32(fecha.Split(' ')[1].Split(':')[0]);
            //        if (Convert.ToInt32(fecha.Split(' ')[1].Split(':')[0]) != 12)
            //        {
            //            horaFinal = Convert.ToInt32(fecha.Split(' ')[1].Split(':')[0]) - 12;
            //        }
            //        fecha = fecha.Split(' ')[0] + " " + fecha.Split(' ')[1];
            //    }
            //}
            //else
            //{
            //    fecha = fecha.Split(' ')[0] + " " + fecha.Split(' ')[1];
            //}

            fecha = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");


            string[] pagos = pagosstring.Split(',');

            string response = string.Empty;
            DataBaseResponse oDataBaseResponse = new DataBaseResponse();
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
                            RegistrarAuditoria(TipoAccion.select, "t_facturacion", query);
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
                            RegistrarAuditoria(TipoAccion.select, "t_personasautorizadas", query);
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

            string sQuery1 = string.Empty;
            ArrayList sQuery2 = new ArrayList(); ;
            string sQuery3 = string.Empty;
            string sQuery4 = string.Empty;

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
                        sQuery1 = "update T_Facturacion set facturaActual = " + Convert.ToInt32(Convert.ToInt32(facturaActual) + 1) + " where idfacturacion = " + idFacturacion;
                        sQuery3 = "Insert into T_Movimientos (IdTransaccion, IdEstacionamiento, IdModulo, Parte, Accion, Denominacion, Cantidad, Valor, FechaMovimiento) VALUES "
                            + "('" + documentoAutorizado + "','" + idEstacionamiento + "','" + idModulo + "','CM','Entrada','" + total + "','1','" + total + "',GETDATE())";
                        sQuery4 = "update T_Partes set DineroActual=(select DineroActual from T_Partes where IdModulo='" + idModulo + "' and IdEstacionamiento='" + idEstacionamiento + "' and NombreParte='CM')+" + total + " where IdModulo='" + idModulo + "' and IdEstacionamiento='" + idEstacionamiento + "' and NombreParte='CM'";


                        command.CommandText = sQuery1;

                        command.ExecuteNonQuery();

                        int i = 0;

                        foreach (string[] item in pagosFinal)
                        {
                            sQuery2.Add("Insert into T_Pagos (IdTransaccion, IdEstacionamiento, IdModulo, IdFacturacion, IdTipoPago, FechaPago, Subtotal, Iva, Total, NumeroFactura, IdAutorizado) VALUES "
                                + "('" + documentoAutorizado + "', '" + idEstacionamiento + "', '" + idModulo + "', '" + idFacturacion + "', '" + item[0] + "', convert(datetime,'" + fecha + "',103), '" + item[1] + "', '" + item[2] + "', '" + item[3] + "', '" + item[4] + "','" + idAutorizacion + "')");
                            command.CommandText = "Insert into T_Pagos (IdTransaccion, IdEstacionamiento, IdModulo, IdFacturacion, IdTipoPago, FechaPago, Subtotal, Iva, Total, NumeroFactura, IdAutorizado) VALUES "
                                + "('" + documentoAutorizado + "', '" + idEstacionamiento + "', '" + idModulo + "', '" + idFacturacion + "', '" + item[0] + "', convert(datetime,'" + fecha + "',103), '" + item[1] + "', '" + item[2] + "', '" + item[3] + "', '" + item[4] + "','" + idAutorizacion + "')";
                            command.ExecuteNonQuery();
                            i++;
                        }

                        command.CommandText = sQuery3;

                        command.ExecuteNonQuery();

                        command.CommandText = sQuery4;

                        command.ExecuteNonQuery();

                        // Attempt to commit the transaction.
                        transaction.Commit();

                    }
                    catch (Exception ex)
                    {
                        oDataBaseResponse.Exito = false;
                        oDataBaseResponse.ErrorMessage = "EXCEPCION BD No guarda modificaciones en BD: " + ex.InnerException + "//////////" + ex.Message + " - " + fecha;
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
                oDataBaseResponse.Exito = false;
                oDataBaseResponse.ErrorMessage = "No encontro facturas disponibles o autorizaciones asociadas a ese idTarjeta";
            }

            if (oDataBaseResponse.Exito)
            {
                RegistrarAuditoria(TipoAccion.update, "t_facturacion", sQuery1);
                foreach (string item in sQuery2)
                {
                    RegistrarAuditoria(TipoAccion.insert, "t_pagos", item);
                }
                RegistrarAuditoria(TipoAccion.insert, "t_movimientos", sQuery3);
                RegistrarAuditoria(TipoAccion.update, "t_partes", sQuery4);
            }

            oDataBaseResponse.Resultado = new { IdTranaccion = documentoAutorizado, IdAutorizacion = idAutorizacion };

            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            response = javaScriptSerializer.Serialize(oDataBaseResponse);

            return response;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string PagarClienteParticular()
        {
            //Registrar T_Pagos
            //Registrar T_Movimientos
            //Actualizar T_Facturacion
            //Actualizar T_Transaccion
            //Actualizar Partes

            string pagosstring = HttpContext.Current.Request.Params["pagos"];

            ArrayList pagosFinal = new ArrayList();

            string idEstacionamiento = HttpContext.Current.Request.Params["idEstacionamiento"];
            string idTransaccion = HttpContext.Current.Request.Params["idTransaccion"];
            string idModulo = HttpContext.Current.Request.Params["idModulo"];
            string fecha = HttpContext.Current.Request.Params["fecha"];
            string total = HttpContext.Current.Request.Params["total"];
            //string fechaAntes = fecha;

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

            string response = string.Empty;
            DataBaseResponse oDataBaseResponse = new DataBaseResponse();
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
                            RegistrarAuditoria(TipoAccion.select, "t_facturacion", query);
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


            string sQuery1 = string.Empty;
            ArrayList sQuery2 = new ArrayList();
            string sQuery3 = string.Empty;
            string sQuery4 = string.Empty;
            string sQuery5 = string.Empty;

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
                        sQuery1 = "update T_Facturacion set facturaActual = " + Convert.ToInt32(Convert.ToInt32(facturaActual) + 1) + " where idfacturacion = " + idFacturacion;
                        sQuery3 = "Insert into T_Movimientos (IdTransaccion, IdEstacionamiento, IdModulo, Parte, Accion, Denominacion, Cantidad, Valor, FechaMovimiento) VALUES "
                            + "('" + idTransaccion + "','" + idEstacionamiento + "','" + idModulo + "','CM','Entrada','" + total + "','1','" + total + "',GETDATE())";
                        sQuery4 = "update T_Partes set DineroActual=(select DineroActual from T_Partes where IdModulo='" + idModulo + "' and IdEstacionamiento='" + idEstacionamiento + "' and NombreParte='CM')+" + total + " where IdModulo='" + idModulo + "' and IdEstacionamiento='" + idEstacionamiento + "' and NombreParte='CM'";
                        sQuery5 = "update T_Transacciones set ValorRecibido = '" + total + "',Cambio=0 where idTransaccion=" + idTransaccion; ;

                        command.CommandText = sQuery1;

                        command.ExecuteNonQuery();



                        foreach (string[] item in pagosFinal)
                        {
                            sQuery2.Add("Insert into T_Pagos (IdTransaccion, IdEstacionamiento, IdModulo, IdFacturacion, IdTipoPago, FechaPago, Subtotal, Iva, Total, NumeroFactura) VALUES "
                                + "('" + idTransaccion + "', '" + idEstacionamiento + "', '" + idModulo + "', '" + idFacturacion + "', '" + item[0] + "', convert(datetime,'" + fecha + "',103), '" + item[1] + "', '" + item[2] + "', '" + item[3] + "', '" + item[4] + "')");
                            command.CommandText =
                                "Insert into T_Pagos (IdTransaccion, IdEstacionamiento, IdModulo, IdFacturacion, IdTipoPago, FechaPago, Subtotal, Iva, Total, NumeroFactura) VALUES "
                                + "('" + idTransaccion + "', '" + idEstacionamiento + "', '" + idModulo + "', '" + idFacturacion + "', '" + item[0] + "', convert(datetime,'" + fecha + "',103), '" + item[1] + "', '" + item[2] + "', '" + item[3] + "', '" + item[4] + "')";
                            command.ExecuteNonQuery();
                        }

                        command.CommandText = sQuery3;

                        command.ExecuteNonQuery();

                        command.CommandText = sQuery4;

                        command.ExecuteNonQuery();

                        command.CommandText = sQuery5;

                        command.ExecuteNonQuery();

                        // Attempt to commit the transaction.
                        transaction.Commit();

                    }
                    catch (Exception ex)
                    {
                        oDataBaseResponse.Exito = false;
                        oDataBaseResponse.ErrorMessage = "EXCEPCION BD No guarda modificaciones en BD: " + ex.InnerException + "//////////" + ex.Message + " - " + fecha;//+ " - " + fechaAntes;
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
                oDataBaseResponse.Exito = false;
                oDataBaseResponse.ErrorMessage = "No encontro facturas disponibles.";
            }

            if (oDataBaseResponse.Exito)
            {
                RegistrarAuditoria(TipoAccion.update, "t_facturacion", sQuery1);
                foreach (string item in sQuery2)
                {
                    RegistrarAuditoria(TipoAccion.insert, "t_pagos", item);
                }
                RegistrarAuditoria(TipoAccion.insert, "t_movimientos", sQuery3);
                RegistrarAuditoria(TipoAccion.update, "t_partes", sQuery4);
                RegistrarAuditoria(TipoAccion.update, "t_transacciones", sQuery5);
            }

            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            response = javaScriptSerializer.Serialize(oDataBaseResponse);

            return response;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string ConsultarInfoTransaccion()
        {
            string idEstacionamiento = HttpContext.Current.Request.Params["idEstacionamiento"];
            string idTarjeta = HttpContext.Current.Request.Params["idTarjeta"];
            string moduloEntrada = HttpContext.Current.Request.Params["moduloEntrada"];

            string response = string.Empty;
            DataBaseResponse oDataBaseResponse = new DataBaseResponse();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;

            query = "select top (1) IdTransaccion from T_Transacciones where ModuloEntrada='" + moduloEntrada + "' and idestacionamiento='" + idEstacionamiento + "' and idtarjeta='" + idTarjeta + "' order by FechaEntrada desc";

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
                            RegistrarAuditoria(TipoAccion.select, "t_transacciones", query);
                            // Read advances to the next row.
                            while (reader.Read())
                            {
                                oDataBaseResponse.Resultado = new { IdTransaccion = reader[0].ToString() };
                            }
                        }
                        else
                        {
                            oDataBaseResponse.Exito = false;
                            oDataBaseResponse.ErrorMessage = "No encontro registro.";
                        }
                    }
                }
            }

            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            response = javaScriptSerializer.Serialize(oDataBaseResponse);

            return response;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string AplicarEtiquetaMoto()
        {
            string idEstacionamiento = HttpContext.Current.Request.Params["idEstacionamiento"];
            string idTarjeta = HttpContext.Current.Request.Params["idTarjeta"];
            string moduloEntrada = HttpContext.Current.Request.Params["moduloEntrada"];

            string response = string.Empty;
            DataBaseResponse oDataBaseResponse = new DataBaseResponse();
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
                            RegistrarAuditoria(TipoAccion.select, "t_transacciones", query);
                            // Read advances to the next row.
                            while (reader.Read())
                            {
                                idTransaccion = reader[0].ToString();
                            }
                        }
                        else
                        {
                            oDataBaseResponse.Exito = false;
                            oDataBaseResponse.ErrorMessage = "No encontro registro.";
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
                            oDataBaseResponse.Exito = false;
                            oDataBaseResponse.ErrorMessage = "No fue posible actualizar el registro.";
                        }
                        else
                        {
                            RegistrarAuditoria(TipoAccion.update, "t_transacciones", query);
                        }
                    }
                }
            }

            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            response = javaScriptSerializer.Serialize(oDataBaseResponse);

            return response;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string ConsultarDescripcionConvenio()
        {
            string idConvenio = HttpContext.Current.Request.Params["idConvenio"];

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
                            RegistrarAuditoria(TipoAccion.select, "t_convenios", query);
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
        [ScriptMethod(UseHttpGet = true)]
        public string ConsultarInfoInterfazContable()
        {
            string idEstacionamiento = HttpContext.Current.Request.Params["idEstacionamiento"];
            string fecha = HttpContext.Current.Request.Params["fecha"];

            string response = string.Empty;
            DataBaseResponse oDataBaseResponse = new DataBaseResponse();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            string query = string.Empty;

            DataTable dtRespuesta = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("P_ObtenerInformacionInterfazContableAdmin", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@IdEstacionamientoConsulta", SqlDbType.BigInt).Value = Convert.ToInt32(idEstacionamiento);
                        cmd.Parameters.Add("@FechaCosultaAntes", SqlDbType.VarChar).Value = fecha;

                        connection.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            dtRespuesta.Load(reader);
                        }
                    }
                }

                List<DatosInterfazContable> _LstDatosInterfaz = new List<DatosInterfazContable>();

                oDataBaseResponse.Exito = true;

                foreach (DataRow row in dtRespuesta.Rows)
                {
                    DatosInterfazContable oDatosInterfazContable = new DatosInterfazContable();
                    oDatosInterfazContable.empresa = row[0].ToString();
                    oDatosInterfazContable.documento = row[1].ToString();
                    oDatosInterfazContable.numero = row[2].ToString();
                    oDatosInterfazContable.fecha = row[3].ToString();
                    oDatosInterfazContable.item = row[4].ToString();
                    oDatosInterfazContable.concepto = row[5].ToString();
                    oDatosInterfazContable.tercero = row[6].ToString();
                    oDatosInterfazContable.terceroconsecutivo = row[7].ToString();
                    oDatosInterfazContable.cuentalocal = row[8].ToString();
                    oDatosInterfazContable.proyecto = row[9].ToString();
                    oDatosInterfazContable.naturaleza = row[10].ToString();
                    oDatosInterfazContable.banco = row[11].ToString();
                    oDatosInterfazContable.cuentabancaria = row[12].ToString();
                    oDatosInterfazContable.centro = row[13].ToString();
                    oDatosInterfazContable.valor = row[14].ToString();
                    oDatosInterfazContable.referencia = row[15].ToString();
                    _LstDatosInterfaz.Add(oDatosInterfazContable);
                }

                oDataBaseResponse.Resultado = _LstDatosInterfaz;

            }
            catch (Exception ex)
            {
                oDataBaseResponse.Exito = false;
                oDataBaseResponse.ErrorMessage = "No fue posible obtener informaion interfaz contable.";
                dtRespuesta = null;
            }

            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            response = javaScriptSerializer.Serialize(oDataBaseResponse);

            return response;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string ObtenerDescripcionReporte()
        {
            string idReporte = HttpContext.Current.Request.Params["idReporte"];

            string valor = string.Empty;
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            string query = string.Empty;

            query = "select descripcion from t_reportes where idreporte='" + idReporte + "'";

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
                            RegistrarAuditoria(TipoAccion.select, "t_reportes", query);
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
        [ScriptMethod(UseHttpGet = true)]
        public string ObtenerIdCajeroxMAC()
        {
            string mac = HttpContext.Current.Request.Params["mac"];

            string response = string.Empty;
            DataBaseResponse oDataBaseResponse = new DataBaseResponse();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;

            query = "select top (1) IdModulo, IdEstacionamiento from T_Configuracion where mac='" + mac + "'";

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
                            RegistrarAuditoria(TipoAccion.select, "t_configuracion", query);
                            // Read advances to the next row.
                            while (reader.Read())
                            {
                                oDataBaseResponse.Resultado = new { IdModulo = reader[0].ToString(), IdEstacionamiento = reader[1].ToString() };
                            }
                        }
                        else
                        {
                            oDataBaseResponse.Exito = false;
                            oDataBaseResponse.ErrorMessage = "No encontro registro.";
                        }
                    }
                }
            }

            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            response = javaScriptSerializer.Serialize(oDataBaseResponse);

            return response;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public int ObtenerIdReglaAutorizacionxCondiciones()
        {
            string horaIni = HttpContext.Current.Request.Params["horaIni"];
            string horaFin = HttpContext.Current.Request.Params["horaFin"];
            string dias = HttpContext.Current.Request.Params["dias"];



            Dictionary<string, bool> list = new Dictionary<string, bool>
            {
                {"Lunes", false},
                {"Martes", false},
                {"Miercoles", false},
                {"Jueves", false},
                {"Viernes", false},
                {"Sabado", false},
                {"Domingo", false},
                {"Festivos", false}
            };

            string[] todosLosDias = { "Lunes", "Martes", "Miercoles", "Jueves", "Viernes", "Sabado", "Domingo", "Festivos" };
            string[] todosLosDiasSeleccionados = dias.Split(',');
            List<string> finalFalsos = new List<string>();

            foreach (string com in todosLosDias)
            {
                if (!todosLosDiasSeleccionados.Contains(com))
                {
                    finalFalsos.Add(com);
                }
            }

            foreach (string item in todosLosDiasSeleccionados.ToList())
            {
                list[item] = true;
            }

            int regla = 0;
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            string query = string.Empty;

            query = "select top (1) IdReglaAutorizacion from T_ReglasAutorizacion where estado='True'";

            for (int i = 0; i < todosLosDiasSeleccionados.Length; i++)
            {
                query += " and " + todosLosDiasSeleccionados[i] + "='True'";
            }

            foreach (var item in finalFalsos)
            {
                query += " and " + item + "='False'";
            }

            query += " and HoraInicial = '" + horaIni + "' and HoraFinal = '" + horaFin + "'";

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
                            RegistrarAuditoria(TipoAccion.select, "t_reglasautorizacion", query);
                            // Read advances to the next row.
                            while (reader.Read())
                            {
                                regla = Convert.ToInt32(reader[0]);
                            }
                        }
                    }
                }
            }

            if (regla == 0)
            {

                query = "select max(IdReglaAutorizacion) from T_ReglasAutorizacion";

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
                                RegistrarAuditoria(TipoAccion.select, "t_reglasautorizacion", query);
                                // Read advances to the next row.
                                while (reader.Read())
                                {
                                    regla = Convert.ToInt32(reader[0]);
                                }
                            }
                        }
                    }
                }

                regla = regla + 1;
                query = "insert into T_ReglasAutorizacion values (" + regla + ",'" + horaIni + "','" + horaFin + "','" + list["Lunes"] + "','" + list["Martes"] + "','" + list["Miercoles"] + "','" + list["Jueves"] + "','" + list["Viernes"] + "','" + list["Sabado"] + "','" + list["Domingo"] + "','" + list["Festivos"] + "','True', 'False')";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        int respuesta = (int)cmd.ExecuteNonQuery();

                        if (respuesta <= 0)
                        {
                            regla = 0;
                        }
                        else
                        {
                            RegistrarAuditoria(TipoAccion.insert, "t_reglasautorizacion", query);
                        }
                    }
                }
            }

            return regla;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string ObtenerValorParametroxNombre()
        {
            string nombre = "ClaveTarjetasCRT";
            string idEstacionamiento = HttpContext.Current.Request.Params["idEstacionamiento"];

            string valor = string.Empty;
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            string query = string.Empty;

            query = "select valor from T_Parametros where Codigo='" + nombre + "' and idEstacionamiento='" + idEstacionamiento + "'";

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
                            RegistrarAuditoria(TipoAccion.select, "t_parametros", query);
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
        [ScriptMethod(UseHttpGet = true)]
        public ArrayList ObtenerReglaAutorizacion()
        {
            string idRegla = HttpContext.Current.Request.Params["idRegla"];

            ArrayList array = new ArrayList();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            string query = "SELECT * FROM T_ReglasAutorizacion where IdReglaAutorizacion=" + idRegla;

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
                            RegistrarAuditoria(TipoAccion.select, "t_reglasautorizacion", query);
                            // Read advances to the next row.
                            while (reader.Read())
                            {
                                array.Add(new { IdRegla = reader[0], HoraI = reader[1].ToString(), HoraF = reader[2].ToString(), Lunes = reader[3], Martes = reader[4], Miercoles = reader[5], Jueves = reader[6], Viernes = reader[7], Sabado = reader[8], Domingo = reader[9], Festivo = reader[10] });
                            }
                        }
                    }
                }
            }
            return array;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public ArrayList ObtenerListaAlarmasCategoria()
        {
            ArrayList array = new ArrayList();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;

            query = "select 'Total Alarmas' as Descripcion, count(*) as Cantidad from T_Alarmas where FechaAtencion is null "
                     + "union all "
                     + "select 'Nivel1' as Descripcion, count(*) as Cantidad from T_Alarmas where FechaAtencion is null and NivelError = 1 "
                     + "union all "
                     + "select 'Nivel2' as Descripcion, count(*) as Cantidad from T_Alarmas where FechaAtencion is null and NivelError = 2 "
                     + "union all "
                     + "select 'Nivel3' as Descripcion, count(*) as Cantidad from T_Alarmas where FechaAtencion is null and NivelError = 3";


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
                            RegistrarAuditoria(TipoAccion.select, "t_alarmas", query);
                            // Read advances to the next row.
                            while (reader.Read())
                            {
                                array.Add(new { Value = reader["Descripcion"], Display = reader["Cantidad"] });
                            }
                        }
                    }
                }
            }
            return array;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string ObtenerImagenesxTransaccion()
        {
            //2017032806320012
            string idTransaccion = HttpContext.Current.Request.Params["idTransaccion"];

            string response = string.Empty;
            DataBaseResponse oDataBaseResponse = new DataBaseResponse();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;

            query = "select e.Nombre, DATEPART(yy,FechaEntrada),DATEPART(MM,FechaEntrada),DATEPART(dd,FechaEntrada), t.ModuloEntrada, p.NombreParte" +
                    " from T_Transacciones as t" +
                    " inner join t_estacionamientos as e" +
                    " on t.idestacionamiento=e.IdEstacionamiento" +
                    " inner join T_Partes as p" +
                    " on p.idModulo=t.ModuloEntrada" +
                    " where p.TipoParte = 'CAMARA' and t.IdTransaccion=" + idTransaccion +
                    " union" +
                    " select e.Nombre, DATEPART(yy,FechaEntrada),DATEPART(MM,FechaEntrada),DATEPART(dd,FechaEntrada), t.ModuloSalida, p.NombreParte" +
                    " from T_Transacciones as t" +
                    " inner join t_estacionamientos as e" +
                    " on t.idestacionamiento=e.IdEstacionamiento" +
                    " inner join T_Partes as p" +
                    " on p.idModulo=t.ModuloSalida" +
                    " where p.TipoParte = 'CAMARA' and t.IdTransaccion=" + idTransaccion;

            ArrayList temporal = new ArrayList();

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
                            RegistrarAuditoria(TipoAccion.select, "t_transacciones", query);
                            // Read advances to the next row.
                            while (reader.Read())
                            {
                                temporal.Add(new { Estacionamiento = reader[0], Anho = reader[1], Mes = reader[2], Dia = reader[3], Modulo = reader[4], Parte = reader[5] });
                            }
                        }
                    }
                }
            }

            string rutaFotos = ConfigurationManager.AppSettings["RutaFotos"].ToString();

            ArrayList array = new ArrayList();

            foreach (var item in temporal)
            {
                dynamic itemDinamico = item;
                try
                {
                    string filaName = rutaFotos + Convert.ToString(itemDinamico.Estacionamiento) + @"\" + Convert.ToString(itemDinamico.Anho) + "-" + Convert.ToString(itemDinamico.Mes) + "-" + Convert.ToString(itemDinamico.Dia) + @"\" + Convert.ToString(itemDinamico.Modulo) + @"\" + idTransaccion + "_" + Convert.ToString(itemDinamico.Modulo) + "_" + Convert.ToString(itemDinamico.Parte) + "_" + Convert.ToString(itemDinamico.Anho) + "_" + Convert.ToString(itemDinamico.Mes) + "_" + Convert.ToString(itemDinamico.Dia) + "_" + 1 + ".jpg";
                    if (File.Exists(filaName))
                    {
                        Image img = Image.FromFile(filaName);

                        byte[] arr;
                        using (MemoryStream ms = new MemoryStream())
                        {
                            img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                            arr = ms.ToArray();
                        }
                        array.Add(new { Value = Convert.ToBase64String(arr) });
                    }
                }
                catch (Exception e)
                {

                }
            }


            oDataBaseResponse.Resultado = array;

            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            response = javaScriptSerializer.Serialize(oDataBaseResponse);

            return response;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public ArrayList ObtenerListaCarrilPagoxEstacionamiento()
        {
            string idEstacionamiento = HttpContext.Current.Request.Params["idEstacionamiento"];
            string idSede = HttpContext.Current.Request.Params["idSede"];


            ArrayList array = new ArrayList();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;

            if ((idEstacionamiento == null || idEstacionamiento == "0") && (idSede == null || idSede == "0"))
            {
                query = "select IdModulo, Carril from T_Configuracion where IdTipoModulo=3";
            }
            else if ((idEstacionamiento == null || idEstacionamiento == "0") && (idSede != null || idSede != "0"))
            {
                query = "select IdModulo, Carril from T_Configuracion as c inner join t_estacionamientos as e on c.IdEstacionamiento=e.idestacionamiento  where IdTipoModulo=3 and IdSede =" + idSede;
            }
            else
            {
                query = "select IdModulo, Carril from T_Configuracion where IdTipoModulo=3 and IdEstacionamiento = " + idEstacionamiento;
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
                            RegistrarAuditoria(TipoAccion.select, "t_configuracion", query);
                            // Read advances to the next row.
                            while (reader.Read())
                            {
                                array.Add(new { Value = reader["IdModulo"], Display = reader["Carril"] });
                            }
                        }
                    }
                }
            }
            return array;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public ArrayList ObtenerListaCarrilEntradaxEstacionamiento()
        {
            string idEstacionamiento = HttpContext.Current.Request.Params["idEstacionamiento"];
            string idSede = HttpContext.Current.Request.Params["idSede"];


            ArrayList array = new ArrayList();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;

            if ((idEstacionamiento == null || idEstacionamiento == "0") && (idSede == null || idSede == "0"))
            {
                query = "select IdModulo, Carril from T_Configuracion where IdTipoModulo=1";
            }
            else if ((idEstacionamiento == null || idEstacionamiento == "0") && (idSede != null || idSede != "0"))
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
                            RegistrarAuditoria(TipoAccion.select, "t_configuracion", query);
                            // Read advances to the next row.
                            while (reader.Read())
                            {
                                array.Add(new { Value = reader["IdModulo"], Display = reader["Carril"] });
                            }
                        }
                    }
                }
            }
            return array;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public ArrayList ObtenerListaMotivosCortesiaXEstacionamiento()
        {
            string idEstacionamiento = HttpContext.Current.Request.Params["idEstacionamiento"];

            ArrayList array = new ArrayList();
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
                            RegistrarAuditoria(TipoAccion.select, "t_motivocortesia", query);
                            // Read advances to the next row.
                            while (reader.Read())
                            {
                                array.Add(new { Value = reader["IdMotivo"], Display = reader["Motivo"] });
                            }
                        }
                    }
                }
            }
            return array;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public ArrayList ObtenerListaMotivosCortesia()
        {
            ArrayList array = new ArrayList();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;

            string user = HttpContext.Current.User.Identity.Name.ToString();

            query = "select IdMotivo, Motivo from t_motivocortesia";

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
                            RegistrarAuditoria(TipoAccion.select, "t_motivocortesia", query);
                            // Read advances to the next row.
                            while (reader.Read())
                            {
                                array.Add(new { Value = reader["IdMotivo"], Display = reader["Motivo"] });
                            }
                        }
                    }
                }
            }
            return array;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public ArrayList ObtenerListaConveniosXEstacionamientoXUsuario()
        {
            string idEstacionamiento = HttpContext.Current.Request.Params["idEstacionamiento"];

            ArrayList array = new ArrayList();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;

            string user = HttpContext.Current.User.Identity.Name.ToString();

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
                            RegistrarAuditoria(TipoAccion.select, "t_convenios", query);
                            // Read advances to the next row.
                            while (reader.Read())
                            {
                                array.Add(new { Value = reader["IdConvenio"], Display = reader["Nombre"] });
                            }
                        }
                    }
                }
            }
            return array;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public ArrayList ObtenerListaEstacionamientos()
        {
            string idSede = HttpContext.Current.Request.Params["idSede"];

            ArrayList array = new ArrayList();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;

            string user = HttpContext.Current.User.Identity.Name.ToString();

            query = "select e.IdEstacionamiento, Nombre"
                    + " from T_Estacionamientos as e"
                    + " inner join"
                    + " T_PermisosEstacionamientoUsuarios as peu"
                    + " on e.IdEstacionamiento = peu.IdEstacionamiento"
                    + " where peu.DocumentoUsuario='" + user + "'";

            if (idSede != null && idSede != "0")
            {
                query += " and idsede=" + idSede;
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
                            RegistrarAuditoria(TipoAccion.select, "t_estacionamientos", query);
                            // Read advances to the next row.
                            while (reader.Read())
                            {
                                array.Add(new { Value = reader["IdEstacionamiento"], Display = reader["Nombre"] });
                            }
                        }
                    }
                }
            }
            return array;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public ArrayList ObtenerListaUsuarios()
        {
            ArrayList array = new ArrayList();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;

            string user = HttpContext.Current.User.Identity.Name.ToString();

            query = "select documento, usuario from t_usuarios";

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
                            RegistrarAuditoria(TipoAccion.select, "t_usuarios", query);
                            // Read advances to the next row.
                            while (reader.Read())
                            {
                                array.Add(new { Value = reader["documento"], Display = reader["usuario"] });
                            }
                        }
                    }
                }
            }
            return array;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public ArrayList ObtenerListaModulos()
        {
            //string idSede = HttpContext.Current.Request.Params["idSede"];

            ArrayList array = new ArrayList();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;

            string user = HttpContext.Current.User.Identity.Name.ToString();

            query = "select idmodulo from t_configuracion";

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
                            RegistrarAuditoria(TipoAccion.select, "t_configuracion", query);
                            // Read advances to the next row.
                            while (reader.Read())
                            {
                                array.Add(new { Value = reader["idmodulo"], Display = reader["idmodulo"] });
                            }
                        }
                    }
                }
            }
            return array;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public ArrayList ObtenerListaModulosPP()
        {
            string idSede = HttpContext.Current.Request.Params["idSede"];
            string idEstacionamiento = HttpContext.Current.Request.Params["idEstacionamiento"];

            ArrayList array = new ArrayList();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;

            string user = HttpContext.Current.User.Identity.Name.ToString();

            if ((idEstacionamiento == null || idEstacionamiento == "0") && (idSede == null || idSede == "0"))
            {
                query = "select IdModulo from T_Configuracion where IdTipoModulo=3";
            }
            else if ((idEstacionamiento == null || idEstacionamiento == "0") && (idSede != null || idSede != "0"))
            {
                query = "select IdModulo from T_Configuracion as c inner join t_estacionamientos as e on c.IdEstacionamiento=e.idestacionamiento  where IdTipoModulo=3 and IdSede =" + idSede;
            }
            else
            {
                query = "select IdModulo from T_Configuracion where IdTipoModulo=3 and IdEstacionamiento = " + idEstacionamiento;
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
                            RegistrarAuditoria(TipoAccion.select, "t_configuracion", query);
                            // Read advances to the next row.
                            while (reader.Read())
                            {
                                array.Add(new { Value = reader["idmodulo"], Display = reader["idmodulo"] });
                            }
                        }
                    }
                }
            }
            return array;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public ArrayList ObtenerListaModulosES()
        {
            string idSede = HttpContext.Current.Request.Params["idSede"];
            string idEstacionamiento = HttpContext.Current.Request.Params["idEstacionamiento"];

            ArrayList array = new ArrayList();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;

            string user = HttpContext.Current.User.Identity.Name.ToString();

            if ((idEstacionamiento == null || idEstacionamiento == "0") && (idSede == null || idSede == "0"))
            {
                query = "select IdModulo from T_Configuracion where IdTipoModulo in (1,2)";
            }
            else if ((idEstacionamiento == null || idEstacionamiento == "0") && (idSede != null || idSede != "0"))
            {
                query = "select IdModulo from T_Configuracion as c inner join t_estacionamientos as e on c.IdEstacionamiento=e.idestacionamiento  where IdTipoModulo in (1,2) and IdSede =" + idSede;
            }
            else
            {
                query = "select IdModulo from T_Configuracion where IdTipoModulo in (1,2) and IdEstacionamiento = " + idEstacionamiento;
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
                            RegistrarAuditoria(TipoAccion.select, "t_configuracion", query);
                            // Read advances to the next row.
                            while (reader.Read())
                            {
                                array.Add(new { Value = reader["idmodulo"], Display = reader["idmodulo"] });
                            }
                        }
                    }
                }
            }
            return array;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public ArrayList ObtenerListaAutorizaciones()
        {
            ArrayList array = new ArrayList();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = "SELECT idAutorizacion, NombreAutorizacion FROM t_autorizaciones WHERE ESTADO = 1";

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
                            RegistrarAuditoria(TipoAccion.select, "t_autorizaciones", query);
                            // Read advances to the next row.
                            while (reader.Read())
                            {
                                array.Add(new { Value = reader["idAutorizacion"], Display = reader["NombreAutorizacion"] });
                            }
                        }
                    }
                }
            }
            return array;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public ArrayList ObtenerListaConvenios()
        {
            ArrayList array = new ArrayList();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = "SELECT idconvenio, nombre FROM t_convenios";

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
                            RegistrarAuditoria(TipoAccion.select, "t_convenios", query);
                            // Read advances to the next row.
                            while (reader.Read())
                            {
                                array.Add(new { Value = reader["idconvenio"], Display = reader["nombre"] });
                            }
                        }
                    }
                }
            }
            return array;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public ArrayList ObtenerListaEventos()
        {
            ArrayList array = new ArrayList();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = "SELECT idevento, evento FROM t_eventos";

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
                            RegistrarAuditoria(TipoAccion.select, "t_eventos", query);
                            // Read advances to the next row.
                            while (reader.Read())
                            {
                                array.Add(new { Value = reader["idevento"], Display = reader["evento"] });
                            }
                        }
                    }
                }
            }
            return array;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public ArrayList ObtenerListaTiposPago()
        {
            ArrayList array = new ArrayList();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = "SELECT idtipopago, tipopago FROM t_tipopago";

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
                            RegistrarAuditoria(TipoAccion.select, "t_tipopago", query);
                            // Read advances to the next row.
                            while (reader.Read())
                            {
                                array.Add(new { Value = reader["idtipopago"], Display = reader["tipopago"] });
                            }
                        }
                    }
                }
            }
            return array;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public ArrayList ObtenerListaSedesCombos()
        {
            ArrayList array = new ArrayList();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;

            string user = HttpContext.Current.User.Identity.Name.ToString();

            query = "SELECT IdSede, Nombre FROM T_Sedes";

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
                            RegistrarAuditoria(TipoAccion.select, "t_sedes", query);
                            // Read advances to the next row.
                            while (reader.Read())
                            {
                                array.Add(new { Value = reader["IdSede"], Display = reader["Nombre"] });
                            }
                        }
                    }
                }
            }
            return array;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public ArrayList ObtenerListaSedes()
        {
            ArrayList array = new ArrayList();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;

            string user = HttpContext.Current.User.Identity.Name.ToString();





            query = "SELECT distinct(s.IdSede), s.Nombre"
                    + " FROM T_Sedes as s"
                    + " inner join"
                    + " T_Estacionamientos as e"
                    + " on s.IdSede=e.IdSede"
                    + " inner join"
                    + " T_PermisosEstacionamientoUsuarios as peu"
                    + " on peu.IdEstacionamiento=e.IdEstacionamiento"
                    + " where peu.DocumentoUsuario='" + user + "'";

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
                            RegistrarAuditoria(TipoAccion.select, "t_sedes", query);
                            // Read advances to the next row.
                            while (reader.Read())
                            {
                                array.Add(new { Value = reader["IdSede"], Display = reader["Nombre"] });
                            }
                        }
                    }
                }
            }

            return array;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public ArrayList ObtenerListaReportes()
        {
            string idTipo = HttpContext.Current.Request.Params["idTipo"];

            ArrayList array = new ArrayList();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;

            string user = HttpContext.Current.User.Identity.Name.ToString();

            query = "SELECT IdReporte, Nombre from T_Reportes where tiporeporte=" + idTipo;

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
                            RegistrarAuditoria(TipoAccion.select, "t_reportes", query);
                            // Read advances to the next row.
                            while (reader.Read())
                            {
                                array.Add(new { Value = reader["IdReporte"], Display = reader["Nombre"] });
                            }
                        }
                    }
                }
            }
            return array;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public ArrayList ObtenerListaTipoModulo()
        {
            ArrayList array = new ArrayList();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = "SELECT IdTipoModulo, TipoModulo FROM T_TipoModulo";

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
                            RegistrarAuditoria(TipoAccion.select, "t_tipomodulo", query);
                            // Read advances to the next row.
                            while (reader.Read())
                            {
                                array.Add(new { Value = reader["IdTipoModulo"], Display = reader["TipoModulo"] });
                            }
                        }
                    }
                }
            }
            return array;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public ArrayList ObtenerListaTipoVehiculo()
        {
            ArrayList array = new ArrayList();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = "SELECT IdTipoVehiculo, TipoVehiculo FROM T_TipoVehiculo";

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
                            RegistrarAuditoria(TipoAccion.select, "t_tipovehiculo", query);
                            // Read advances to the next row.
                            while (reader.Read())
                            {
                                array.Add(new { Value = reader["IdTipoVehiculo"], Display = reader["TipoVehiculo"] });
                            }
                        }
                    }
                }
            }
            return array;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public ArrayList ObtenerListaTipoDeposito()
        {
            ArrayList array = new ArrayList();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = "SELECT IdDeposito, Consignacion FROM T_TipoConsignacion";

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
                            RegistrarAuditoria(TipoAccion.select, "T_TipoConsignacion", query);
                            // Read advances to the next row.
                            while (reader.Read())
                            {
                                array.Add(new { Value = reader["IdDeposito"], Display = reader["Consignacion"] });
                            }
                        }
                    }
                }
            }
            return array;
        }


        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public ArrayList ObtenerFiltrosReporte()
        {
            string idReporte = HttpContext.Current.Request.Params["idReporte"];

            ArrayList array = new ArrayList();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = "SELECT tfr.IdTipoFiltro, NombreControl FROM T_FiltroReporte as fr inner join T_TipoFiltroReportes as tfr on fr.idtipofiltro=tfr.idtipofiltro where idreporte=" + idReporte;

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
                            RegistrarAuditoria(TipoAccion.select, "t_filtroreporte", query);
                            // Read advances to the next row.
                            while (reader.Read())
                            {
                                array.Add(new { Value = reader["IdTipoFiltro"], Display = reader["NombreControl"] });
                            }
                        }
                    }
                }
            }
            return array;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string GetItemsUsuarios()
        {
            int sEcho = ToInt(HttpContext.Current.Request.Params["sEcho"]);
            int iDisplayLength = ToInt(HttpContext.Current.Request.Params["iDisplayLength"]);
            int iDisplayStart = ToInt(HttpContext.Current.Request.Params["iDisplayStart"]);
            //string rawSearch = HttpContext.Current.Request.Params["sSearch"];

            //string participant = HttpContext.Current.Request.Params["iIdEstacionamiento"];
            //string sede = HttpContext.Current.Request.Params["iIdSede"];

            var sb = new StringBuilder();

            var whereClause = string.Empty;
            //if (participant.Length > 0 && participant != "0")
            //{
            //    sb.Append(" Where u.documento like ");
            //    sb.Append("'%" + participant + "%'");
            //    whereClause = sb.ToString();
            //}
            //sb.Clear();
            //if (sede.Length > 0 && sede != "0")
            //{
            //    if (whereClause == string.Empty)
            //    {
            //        sb.Append(" Where s.idsede = '" + sede + "'");
            //    }
            //    else
            //    {
            //        sb.Append(" and s.idsede = '" + sede + "'");
            //    }
            //    whereClause += sb.ToString();
            //}
            //sb.Clear();


            var filteredWhere = string.Empty;

            //var wrappedSearch = "'%" + rawSearch + "%'";

            //if (rawSearch.Length > 0)
            //{
            //    sb.Append(" WHERE Nombre LIKE ");
            //    sb.Append(wrappedSearch);
            //    sb.Append(" OR t.ModuloEntrada LIKE ");
            //    sb.Append(wrappedSearch);
            //    sb.Append(" OR t.FechaEntrada LIKE ");
            //    sb.Append(wrappedSearch);
            //    sb.Append(" OR t.PlacaEntrada LIKE ");
            //    sb.Append(wrappedSearch);
            //    sb.Append(" OR t.ModuloSalida LIKE ");
            //    sb.Append(wrappedSearch);
            //    sb.Append(" OR t.FechaSalida LIKE ");
            //    sb.Append(wrappedSearch);
            //    sb.Append(" OR t.PlacaSalida LIKE ");
            //    sb.Append(wrappedSearch);

            //    filteredWhere = sb.ToString();
            //}


            //ORDERING

            sb.Clear();

            string orderByClause = string.Empty;
            sb.Append(ToInt(HttpContext.Current.Request.Params["iSortCol_0"]));

            sb.Append(" ");

            sb.Append(HttpContext.Current.Request.Params["sSortDir_0"]);

            orderByClause = sb.ToString();

            if (!String.IsNullOrEmpty(orderByClause))
            {
                orderByClause = orderByClause.Replace("0", ", documento ");
                //orderByClause = orderByClause.Replace("1", ", idestacionamiento ");
                orderByClause = orderByClause.Replace("1", ", nombres ");
                orderByClause = orderByClause.Replace("2", ", apellidos ");
                orderByClause = orderByClause.Replace("3", ", usuario ");
                orderByClause = orderByClause.Replace("4", ", contraseña ");
                orderByClause = orderByClause.Replace("5", ", cargo ");
                orderByClause = orderByClause.Replace("6", ", usuariocreador ");
                orderByClause = orderByClause.Replace("7", ", fechacreacion ");
                orderByClause = orderByClause.Replace("8", ", idtarjeta ");
                orderByClause = orderByClause.Replace("9", ", estado ");
                //orderByClause = orderByClause.Replace("11", ", nombresede ");
                //orderByClause = orderByClause.Replace("12", ", nombreestacionamiento ");

                orderByClause = orderByClause.Remove(0, 1);
            }
            else
            {
                orderByClause = "Codigo ASC";
            }
            orderByClause = "ORDER BY " + orderByClause;

            sb.Clear();

            var numberOfRowsToReturn = "";
            numberOfRowsToReturn = iDisplayLength == -1 ? "TotalRows" : (iDisplayStart + iDisplayLength).ToString();

            string query = @"               
                            SELECT *
                            FROM
	                            (SELECT row_number() OVER ({0}) AS RowNumber
		                              , *
	                             FROM
		                             (SELECT (SELECT count(u.documento)
				                              FROM
					                              t_usuarios as u {4}) AS TotalRows
			                               , ( SELECT  count( u.documento) FROM t_usuarios as u {1} {4}) AS TotalDisplayRows
	                                       , u.documento
	                                       , u.nombres
	                                       , u.apellidos
	                                       , u.usuario
	                                       , u.contraseña
	                                       , u.cargo
	                                       , u.usuariocreador
	                                       , u.fechacreacion
	                                       , u.idtarjeta
	                                       , u.estado
		                              FROM
			                              t_usuarios as u {1} {4} ) RawResults) Results
                            WHERE
	                            RowNumber BETWEEN {2} AND {3}";


            query = String.Format(query, orderByClause, filteredWhere, iDisplayStart + 1, numberOfRowsToReturn, whereClause);

            //var connectionString = "Data Source=CECHECE-OFFLAP\\MSSQLSERVER2012;Initial Catalog=Parking;;User ID=sa;Password=3GL0B4LT12+";
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);

            try
            {
                conn.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            var DB = new SqlCommand();
            DB.Connection = conn;
            DB.CommandText = query;
            var data = DB.ExecuteReader();
            RegistrarAuditoria(TipoAccion.select, "t_usuarios", query);

            var totalDisplayRecords = "";
            var totalRecords = "";
            string outputJson = string.Empty;

            var rowClass = "";
            var count = 0;

            while (data.Read())
            {

                if (totalRecords.Length == 0)
                {
                    totalRecords = data["TotalRows"].ToString();
                    totalDisplayRecords = data["TotalDisplayRows"].ToString();
                }
                sb.Append("{");
                sb.AppendFormat(@"""DT_RowId"": ""{0}""", count++);
                sb.Append(",");
                sb.AppendFormat(@"""DT_RowClass"": ""{0}""", rowClass);
                sb.Append(",");
                sb.AppendFormat(@"""0"": ""{0}""", data["Documento"]);
                sb.Append(",");
                sb.AppendFormat(@"""1"": ""{0}""", data["Nombres"]);
                sb.Append(",");
                sb.AppendFormat(@"""2"": ""{0}""", data["Apellidos"]);
                sb.Append(",");
                sb.AppendFormat(@"""3"": ""{0}""", data["Usuario"]);
                sb.Append(",");
                sb.AppendFormat(@"""4"": ""{0}""", data["Contraseña"]);
                sb.Append(",");
                sb.AppendFormat(@"""5"": ""{0}""", data["Cargo"]);
                sb.Append(",");
                sb.AppendFormat(@"""6"": ""{0}""", data["UsuarioCreador"]);
                sb.Append(",");
                sb.AppendFormat(@"""7"": ""{0}""", data["FechaCreacion"]);
                sb.Append(",");
                sb.AppendFormat(@"""8"": ""{0}""", data["IdTarjeta"]);
                sb.Append(",");
                sb.AppendFormat(@"""9"": ""{0}""", data["Estado"]);
                sb.Append("},");
            }

            // handles zero records
            if (totalRecords.Length == 0)
            {
                sb.Append("{");
                sb.Append(@"""sEcho"": ");
                sb.AppendFormat(@"""{0}""", sEcho);
                sb.Append(",");
                sb.Append(@"""iTotalRecords"": 0");
                sb.Append(",");
                sb.Append(@"""iTotalDisplayRecords"": 0");
                sb.Append(", ");
                sb.Append(@"""aaData"": [ ");
                sb.Append("]}");
                outputJson = sb.ToString();

                return outputJson;
            }
            outputJson = sb.Remove(sb.Length - 1, 1).ToString();
            sb.Clear();

            sb.Append("{");
            sb.Append(@"""sEcho"": ");
            sb.AppendFormat(@"""{0}""", sEcho);
            sb.Append(",");
            sb.Append(@"""iTotalRecords"": ");
            sb.Append(totalRecords);
            sb.Append(",");
            sb.Append(@"""iTotalDisplayRecords"": ");
            sb.Append(totalDisplayRecords);
            sb.Append(", ");
            sb.Append(@"""aaData"": [ ");
            sb.Append(outputJson);
            sb.Append("]}");
            outputJson = sb.ToString();

            return outputJson;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string GetItemsParametros()
        {
            int sEcho = ToInt(HttpContext.Current.Request.Params["sEcho"]);
            int iDisplayLength = ToInt(HttpContext.Current.Request.Params["iDisplayLength"]);
            int iDisplayStart = ToInt(HttpContext.Current.Request.Params["iDisplayStart"]);
            //string rawSearch = HttpContext.Current.Request.Params["sSearch"];

            string participant = HttpContext.Current.Request.Params["iIdEstacionamiento"];
            string sede = HttpContext.Current.Request.Params["iIdSede"];

            var sb = new StringBuilder();

            var whereClause = string.Empty;
            if (participant.Length > 0 && participant != "0")
            {
                sb.Append(" Where p.IdEstacionamiento like ");
                sb.Append("'%" + participant + "%'");
                whereClause = sb.ToString();
            }
            sb.Clear();
            if (sede.Length > 0 && sede != "0")
            {
                if (whereClause == string.Empty)
                {
                    sb.Append(" Where s.idsede = '" + sede + "'");
                }
                else
                {
                    sb.Append(" and s.idsede = '" + sede + "'");
                }
                whereClause += sb.ToString();
            }
            sb.Clear();


            var filteredWhere = string.Empty;

            //var wrappedSearch = "'%" + rawSearch + "%'";

            //if (rawSearch.Length > 0)
            //{
            //    sb.Append(" WHERE Nombre LIKE ");
            //    sb.Append(wrappedSearch);
            //    sb.Append(" OR t.ModuloEntrada LIKE ");
            //    sb.Append(wrappedSearch);
            //    sb.Append(" OR t.FechaEntrada LIKE ");
            //    sb.Append(wrappedSearch);
            //    sb.Append(" OR t.PlacaEntrada LIKE ");
            //    sb.Append(wrappedSearch);
            //    sb.Append(" OR t.ModuloSalida LIKE ");
            //    sb.Append(wrappedSearch);
            //    sb.Append(" OR t.FechaSalida LIKE ");
            //    sb.Append(wrappedSearch);
            //    sb.Append(" OR t.PlacaSalida LIKE ");
            //    sb.Append(wrappedSearch);

            //    filteredWhere = sb.ToString();
            //}


            //ORDERING

            sb.Clear();

            string orderByClause = string.Empty;
            sb.Append(ToInt(HttpContext.Current.Request.Params["iSortCol_0"]));

            sb.Append(" ");

            sb.Append(HttpContext.Current.Request.Params["sSortDir_0"]);

            orderByClause = sb.ToString();

            if (!String.IsNullOrEmpty(orderByClause))
            {
                orderByClause = orderByClause.Replace("0", ", nombresede ");
                orderByClause = orderByClause.Replace("1", ", nombreestacionamiento ");
                orderByClause = orderByClause.Replace("2", ", IdEstacionamiento ");
                orderByClause = orderByClause.Replace("3", ", Codigo ");
                orderByClause = orderByClause.Replace("4", ", Valor ");
                orderByClause = orderByClause.Replace("5", ", Descripcion ");
                orderByClause = orderByClause.Replace("6", ", Estado ");

                orderByClause = orderByClause.Remove(0, 1);
            }
            else
            {
                orderByClause = "Codigo ASC";
            }
            orderByClause = "ORDER BY " + orderByClause;

            sb.Clear();

            var numberOfRowsToReturn = "";
            numberOfRowsToReturn = iDisplayLength == -1 ? "TotalRows" : (iDisplayStart + iDisplayLength).ToString();

            string query = @"               
                            SELECT *
                            FROM
	                            (SELECT row_number() OVER ({0}) AS RowNumber
		                              , *
	                             FROM
		                             (SELECT (SELECT count(p.Codigo)
				                              FROM
					                              t_parametros as p inner join T_Estacionamientos as e on p.IdEstacionamiento = e.IdEstacionamiento inner join T_Sedes as s on s.IdSede=e.IdSede {4}) AS TotalRows
			                               , ( SELECT  count( e.Nombre) FROM t_parametros as p inner join T_Estacionamientos as e on p.IdEstacionamiento = e.IdEstacionamiento inner join T_Sedes as s on s.IdSede=e.IdSede {1} {4}) AS TotalDisplayRows	
                                           , s.Nombre as NombreSede
                                           , e.Nombre as NombreEstacionamiento   
			                               , p.IdEstacionamiento 
                                           , p.Codigo
                                           , p.Valor
                                           , p.Descripcion
                                           , p.Estado  
		                              FROM
			                              t_parametros as p inner join T_Estacionamientos as e on p.IdEstacionamiento = e.IdEstacionamiento inner join T_Sedes as s on s.IdSede=e.IdSede {1} {4} ) RawResults) Results
                            WHERE
	                            RowNumber BETWEEN {2} AND {3}";


            query = String.Format(query, orderByClause, filteredWhere, iDisplayStart + 1, numberOfRowsToReturn, whereClause);

            //var connectionString = "Data Source=CECHECE-OFFLAP\\MSSQLSERVER2012;Initial Catalog=Parking;;User ID=sa;Password=3GL0B4LT12+";
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);

            try
            {
                conn.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            var DB = new SqlCommand();
            DB.Connection = conn;
            DB.CommandText = query;
            var data = DB.ExecuteReader();
            RegistrarAuditoria(TipoAccion.select, "t_parametros", query);

            var totalDisplayRecords = "";
            var totalRecords = "";
            string outputJson = string.Empty;

            var rowClass = "";
            var count = 0;

            while (data.Read())
            {

                if (totalRecords.Length == 0)
                {
                    totalRecords = data["TotalRows"].ToString();
                    totalDisplayRecords = data["TotalDisplayRows"].ToString();
                }
                sb.Append("{");
                sb.AppendFormat(@"""DT_RowId"": ""{0}""", count++);
                sb.Append(",");
                sb.AppendFormat(@"""DT_RowClass"": ""{0}""", rowClass);
                sb.Append(",");
                sb.AppendFormat(@"""0"": ""{0}""", data["NombreSede"]);
                sb.Append(",");
                sb.AppendFormat(@"""1"": ""{0}""", data["NombreEstacionamiento"]);
                sb.Append(",");
                sb.AppendFormat(@"""2"": ""{0}""", data["IdEstacionamiento"]);
                sb.Append(",");
                sb.AppendFormat(@"""3"": ""{0}""", data["Codigo"]);
                sb.Append(",");
                sb.AppendFormat(@"""4"": ""{0}""", data["Valor"]);
                sb.Append(",");
                sb.AppendFormat(@"""5"": ""{0}""", data["Descripcion"]);
                sb.Append(",");
                sb.AppendFormat(@"""6"": ""{0}""", data["Estado"]);
                sb.Append("},");
            }

            // handles zero records
            if (totalRecords.Length == 0)
            {
                sb.Append("{");
                sb.Append(@"""sEcho"": ");
                sb.AppendFormat(@"""{0}""", sEcho);
                sb.Append(",");
                sb.Append(@"""iTotalRecords"": 0");
                sb.Append(",");
                sb.Append(@"""iTotalDisplayRecords"": 0");
                sb.Append(", ");
                sb.Append(@"""aaData"": [ ");
                sb.Append("]}");
                outputJson = sb.ToString();

                return outputJson;
            }
            outputJson = sb.Remove(sb.Length - 1, 1).ToString();
            sb.Clear();

            sb.Append("{");
            sb.Append(@"""sEcho"": ");
            sb.AppendFormat(@"""{0}""", sEcho);
            sb.Append(",");
            sb.Append(@"""iTotalRecords"": ");
            sb.Append(totalRecords);
            sb.Append(",");
            sb.Append(@"""iTotalDisplayRecords"": ");
            sb.Append(totalDisplayRecords);
            sb.Append(", ");
            sb.Append(@"""aaData"": [ ");
            sb.Append(outputJson);
            sb.Append("]}");
            outputJson = sb.ToString();

            return outputJson;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string GetItemsFacturas()
        {
            int sEcho = ToInt(HttpContext.Current.Request.Params["sEcho"]);
            int iDisplayLength = ToInt(HttpContext.Current.Request.Params["iDisplayLength"]);
            int iDisplayStart = ToInt(HttpContext.Current.Request.Params["iDisplayStart"]);
            //string rawSearch = HttpContext.Current.Request.Params["sSearch"];

            string participant = HttpContext.Current.Request.Params["iIdEstacionamiento"];
            string sede = HttpContext.Current.Request.Params["iIdSede"];
            string modulo = HttpContext.Current.Request.Params["iIdModulo"];

            var sb = new StringBuilder();

            var whereClause = string.Empty;
            if (participant.Length > 0 && participant != "0")
            {
                sb.Append(" Where f.IdEstacionamiento like ");
                sb.Append("'%" + participant + "%'");
                whereClause = sb.ToString();
            }
            sb.Clear();
            if (sede.Length > 0 && sede != "0")
            {
                if (whereClause == string.Empty)
                {
                    sb.Append(" Where s.idsede = '" + sede + "'");
                }
                else
                {
                    sb.Append(" and s.idsede = '" + sede + "'");
                }
                whereClause += sb.ToString();
            }
            sb.Clear();
            if (modulo.Length > 0 && modulo != "0")
            {
                if (whereClause == string.Empty)
                {
                    sb.Append(" Where f.Idmodulo = '" + modulo + "'");
                }
                else
                {
                    sb.Append(" and f.Idmodulo = '" + modulo + "'");
                }
                whereClause += sb.ToString();
            }
            sb.Clear();


            var filteredWhere = string.Empty;

            //var wrappedSearch = "'%" + rawSearch + "%'";

            //if (rawSearch.Length > 0)
            //{
            //    sb.Append(" WHERE Nombre LIKE ");
            //    sb.Append(wrappedSearch);
            //    sb.Append(" OR t.ModuloEntrada LIKE ");
            //    sb.Append(wrappedSearch);
            //    sb.Append(" OR t.FechaEntrada LIKE ");
            //    sb.Append(wrappedSearch);
            //    sb.Append(" OR t.PlacaEntrada LIKE ");
            //    sb.Append(wrappedSearch);
            //    sb.Append(" OR t.ModuloSalida LIKE ");
            //    sb.Append(wrappedSearch);
            //    sb.Append(" OR t.FechaSalida LIKE ");
            //    sb.Append(wrappedSearch);
            //    sb.Append(" OR t.PlacaSalida LIKE ");
            //    sb.Append(wrappedSearch);

            //    filteredWhere = sb.ToString();
            //}


            //ORDERING

            sb.Clear();

            string orderByClause = string.Empty;
            sb.Append(ToInt(HttpContext.Current.Request.Params["iSortCol_0"]));

            sb.Append(" ");

            sb.Append(HttpContext.Current.Request.Params["sSortDir_0"]);

            orderByClause = sb.ToString();

            if (!String.IsNullOrEmpty(orderByClause))
            {
                orderByClause = orderByClause.Replace("0", ", IdFacturacion");
                orderByClause = orderByClause.Replace("1", ", IdModulo");
                orderByClause = orderByClause.Replace("2", ", IdEstacionamiento");
                orderByClause = orderByClause.Replace("3", ", Prefijo");
                orderByClause = orderByClause.Replace("4", ", FacturaInicial");
                orderByClause = orderByClause.Replace("5", ", FacturaFinal");
                orderByClause = orderByClause.Replace("6", ", FacturaActual");
                orderByClause = orderByClause.Replace("7", ", NumeroResolucion");
                orderByClause = orderByClause.Replace("8", ", FechaResolucion");
                orderByClause = orderByClause.Replace("9", ", Estado");
                orderByClause = orderByClause.Replace("10", ", FechaFinResolucion");
                orderByClause = orderByClause.Replace("11", ", IdSede");
                orderByClause = orderByClause.Replace("12", ", NombreSede");
                orderByClause = orderByClause.Replace("13", ", NombreEstacionamiento");

                orderByClause = orderByClause.Remove(0, 1);
            }
            else
            {
                orderByClause = "IdFacturacion ASC";
            }
            orderByClause = "ORDER BY " + orderByClause;

            sb.Clear();

            var numberOfRowsToReturn = "";
            numberOfRowsToReturn = iDisplayLength == -1 ? "TotalRows" : (iDisplayStart + iDisplayLength).ToString();

            string query = @"               
                            SELECT *
                            FROM
	                            (SELECT row_number() OVER ({0}) AS RowNumber
		                              , *
	                             FROM
		                             (SELECT (SELECT count(f.IdFacturacion)
				                              FROM
					                              t_facturacion as f inner join T_Estacionamientos as e on f.IdEstacionamiento = e.IdEstacionamiento inner join T_Sedes as s on s.IdSede=e.IdSede {4}) AS TotalRows
			                               , ( SELECT  count( e.Nombre) FROM t_facturacion as f inner join T_Estacionamientos as e on f.IdEstacionamiento = e.IdEstacionamiento inner join T_Sedes as s on s.IdSede=e.IdSede {1} {4}) AS TotalDisplayRows	
                                           , f.Idfacturacion
                                           , f.IdModulo
                                           , f.IdEstacionamiento
                                           , f.Prefijo
                                           , f.FacturaInicial
                                           , f.FacturaFinal
                                           , f.FacturaActual
                                           , f.NumeroResolucion
                                           , f.FechaResolucion
                                           , f.Estado
                                           , f.FechaFinResolucion
                                           , s.IdSede   
                                           , s.Nombre as NombreSede
                                           , e.Nombre as NombreEstacionamiento
		                              FROM
			                              t_facturacion as f inner join T_Estacionamientos as e on f.IdEstacionamiento = e.IdEstacionamiento inner join T_Sedes as s on s.IdSede=e.IdSede {1} {4} ) RawResults) Results
                            WHERE
	                            RowNumber BETWEEN {2} AND {3}";


            query = String.Format(query, orderByClause, filteredWhere, iDisplayStart + 1, numberOfRowsToReturn, whereClause);

            //var connectionString = "Data Source=CECHECE-OFFLAP\\MSSQLSERVER2012;Initial Catalog=Parking;;User ID=sa;Password=3GL0B4LT12+";
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);

            try
            {
                conn.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            var DB = new SqlCommand();
            DB.Connection = conn;
            DB.CommandText = query;
            var data = DB.ExecuteReader();
            RegistrarAuditoria(TipoAccion.select, "t_facturacion", query);

            var totalDisplayRecords = "";
            var totalRecords = "";
            string outputJson = string.Empty;

            var rowClass = "";
            var count = 0;

            while (data.Read())
            {

                if (totalRecords.Length == 0)
                {
                    totalRecords = data["TotalRows"].ToString();
                    totalDisplayRecords = data["TotalDisplayRows"].ToString();
                }
                sb.Append("{");
                sb.AppendFormat(@"""DT_RowId"": ""{0}""", count++);
                sb.Append(",");
                sb.AppendFormat(@"""DT_RowClass"": ""{0}""", rowClass);
                sb.Append(",");
                sb.AppendFormat(@"""0"": ""{0}""", data["Idfacturacion"]);
                sb.Append(",");
                sb.AppendFormat(@"""1"": ""{0}""", data["IdModulo"]);
                sb.Append(",");
                sb.AppendFormat(@"""2"": ""{0}""", data["IdEstacionamiento"]);
                sb.Append(",");
                sb.AppendFormat(@"""3"": ""{0}""", data["Prefijo"]);
                sb.Append(",");
                sb.AppendFormat(@"""4"": ""{0}""", data["FacturaInicial"]);
                sb.Append(",");
                sb.AppendFormat(@"""5"": ""{0}""", data["FacturaFinal"]);
                sb.Append(",");
                sb.AppendFormat(@"""6"": ""{0}""", data["FacturaActual"]);
                sb.Append(",");
                sb.AppendFormat(@"""7"": ""{0}""", data["NumeroResolucion"]);
                sb.Append(",");
                sb.AppendFormat(@"""8"": ""{0}""", data["FechaResolucion"]);
                sb.Append(",");
                sb.AppendFormat(@"""9"": ""{0}""", data["Estado"]);
                sb.Append(",");
                sb.AppendFormat(@"""10"": ""{0}""", data["FechaFinResolucion"]);
                sb.Append(",");
                sb.AppendFormat(@"""11"": ""{0}""", data["IdSede"]);
                sb.Append(",");
                sb.AppendFormat(@"""12"": ""{0}""", data["NombreSede"]);
                sb.Append(",");
                sb.AppendFormat(@"""13"": ""{0}""", data["NombreEstacionamiento"]);
                sb.Append("},");
            }

            // handles zero records
            if (totalRecords.Length == 0)
            {
                sb.Append("{");
                sb.Append(@"""sEcho"": ");
                sb.AppendFormat(@"""{0}""", sEcho);
                sb.Append(",");
                sb.Append(@"""iTotalRecords"": 0");
                sb.Append(",");
                sb.Append(@"""iTotalDisplayRecords"": 0");
                sb.Append(", ");
                sb.Append(@"""aaData"": [ ");
                sb.Append("]}");
                outputJson = sb.ToString();

                return outputJson;
            }
            outputJson = sb.Remove(sb.Length - 1, 1).ToString();
            sb.Clear();

            sb.Append("{");
            sb.Append(@"""sEcho"": ");
            sb.AppendFormat(@"""{0}""", sEcho);
            sb.Append(",");
            sb.Append(@"""iTotalRecords"": ");
            sb.Append(totalRecords);
            sb.Append(",");
            sb.Append(@"""iTotalDisplayRecords"": ");
            sb.Append(totalDisplayRecords);
            sb.Append(", ");
            sb.Append(@"""aaData"": [ ");
            sb.Append(outputJson);
            sb.Append("]}");
            outputJson = sb.ToString();

            return outputJson;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string GetItems()
        {
            int sEcho = ToInt(HttpContext.Current.Request.Params["sEcho"]);
            int iDisplayLength = ToInt(HttpContext.Current.Request.Params["iDisplayLength"]);
            int iDisplayStart = ToInt(HttpContext.Current.Request.Params["iDisplayStart"]);
            //string rawSearch = HttpContext.Current.Request.Params["sSearch"];

            string participant = HttpContext.Current.Request.Params["iIdEstacionamiento"];
            string fechaAntes = HttpContext.Current.Request.Params["sFechaAntes"];
            string fechaDespues = HttpContext.Current.Request.Params["sFechaDespues"];
            string tipoBusqueda = HttpContext.Current.Request.Params["sTipoBusquedaFecha"];
            string carril = HttpContext.Current.Request.Params["sCarril"];
            string placa = HttpContext.Current.Request.Params["sPlaca"];
            string tarjeta = HttpContext.Current.Request.Params["sIdTarjeta"];
            string sede = HttpContext.Current.Request.Params["iIdSede"];
            string tipoVehiculo = HttpContext.Current.Request.Params["iIdTipoVehiculo"];


            var sb = new StringBuilder();

            var whereClause = string.Empty;
            if (participant.Length > 0 && participant != "0")
            {
                sb.Append(" Where e.IdEstacionamiento like ");
                sb.Append("'%" + participant + "%'");
                whereClause = sb.ToString();
            }
            sb.Clear();
            if (fechaAntes.Length > 0 && fechaDespues.Length > 0)
            {
                if (whereClause == string.Empty)
                {
                    switch (tipoBusqueda)
                    {
                        case "1":
                            sb.Append(" Where t.FechaEntrada between ");
                            break;
                        case "2":
                            sb.Append(" Where t.FechaSalida between ");
                            break;
                        case "3":

                            break;
                    }
                }
                else
                {
                    switch (tipoBusqueda)
                    {
                        case "1":
                            sb.Append(" and t.FechaEntrada between ");
                            break;
                        case "2":
                            sb.Append(" and t.FechaSalida between ");
                            break;
                        case "3":

                            break;
                    }
                }
                sb.Append("convert(datetime,'" + fechaAntes + "',103) and convert(datetime,'" + fechaDespues + "',103)");
                whereClause += sb.ToString();
            }
            sb.Clear();
            if (carril.Length > 0 && carril != "0")
            {
                if (whereClause == string.Empty)
                {
                    sb.Append(" Where t.ModuloEntrada = '" + carril + "'");
                }
                else
                {
                    sb.Append(" and t.ModuloEntrada = '" + carril + "'");
                }
                whereClause += sb.ToString();
            }
            sb.Clear();
            if (placa.Length > 0 && placa != string.Empty)
            {
                if (whereClause == string.Empty)
                {
                    sb.Append(" Where t.PlacaEntrada like ");
                    sb.Append("'%" + placa + "%'");
                }
                else
                {
                    sb.Append(" and t.PlacaEntrada like ");
                    sb.Append("'%" + placa + "%'");
                }
                whereClause += sb.ToString();
            }
            sb.Clear();
            if (tarjeta.Length > 0 && tarjeta != string.Empty)
            {
                if (whereClause == string.Empty)
                {
                    sb.Append(" Where t.IdTarjeta = '" + tarjeta + "'");
                }
                else
                {
                    sb.Append(" and t.IdTarjeta = '" + tarjeta + "'");
                }
                whereClause += sb.ToString();
            }
            sb.Clear();
            if (sede.Length > 0 && sede != "0")
            {
                if (whereClause == string.Empty)
                {
                    sb.Append(" Where s.idsede = '" + sede + "'");
                }
                else
                {
                    sb.Append(" and s.idsede = '" + sede + "'");
                }
                whereClause += sb.ToString();
            }
            sb.Clear();
            if (tipoVehiculo.Length > 0 && tipoVehiculo != "0")
            {
                if (whereClause == string.Empty)
                {
                    sb.Append(" Where t.idtipovehiculo = '" + tipoVehiculo + "'");
                }
                else
                {
                    sb.Append(" and t.idtipovehiculo = '" + tipoVehiculo + "'");
                }
                whereClause += sb.ToString();
            }
            sb.Clear();

            string user = HttpContext.Current.User.Identity.Name.ToString();


            if (whereClause == string.Empty)
            {
                sb.Append(" Where peu.DocumentoUsuario= '" + user + "'");
            }
            else
            {
                sb.Append(" and peu.DocumentoUsuario= '" + user + "'");
            }
            whereClause += sb.ToString();
            sb.Clear();

            var filteredWhere = string.Empty;

            //var wrappedSearch = "'%" + rawSearch + "%'";

            //if (rawSearch.Length > 0)
            //{
            //    sb.Append(" WHERE Nombre LIKE ");
            //    sb.Append(wrappedSearch);
            //    sb.Append(" OR t.ModuloEntrada LIKE ");
            //    sb.Append(wrappedSearch);
            //    sb.Append(" OR t.FechaEntrada LIKE ");
            //    sb.Append(wrappedSearch);
            //    sb.Append(" OR t.PlacaEntrada LIKE ");
            //    sb.Append(wrappedSearch);
            //    sb.Append(" OR t.ModuloSalida LIKE ");
            //    sb.Append(wrappedSearch);
            //    sb.Append(" OR t.FechaSalida LIKE ");
            //    sb.Append(wrappedSearch);
            //    sb.Append(" OR t.PlacaSalida LIKE ");
            //    sb.Append(wrappedSearch);

            //    filteredWhere = sb.ToString();
            //}


            //ORDERING

            sb.Clear();

            string orderByClause = string.Empty;
            sb.Append(ToInt(HttpContext.Current.Request.Params["iSortCol_0"]));

            sb.Append(" ");

            sb.Append(HttpContext.Current.Request.Params["sSortDir_0"]);

            orderByClause = sb.ToString();

            if (!String.IsNullOrEmpty(orderByClause))
            {

                orderByClause = orderByClause.Replace("0", ", Nombre ");
                orderByClause = orderByClause.Replace("1", ", ModuloEntrada ");
                orderByClause = orderByClause.Replace("2", ", FechaEntrada ");
                orderByClause = orderByClause.Replace("3", ", PlacaEntrada ");
                orderByClause = orderByClause.Replace("4", ", ModuloSalida ");
                orderByClause = orderByClause.Replace("5", ", FechaSalida ");
                orderByClause = orderByClause.Replace("6", ", PlacaSalida ");
                orderByClause = orderByClause.Replace("7", ", IdTransaccion ");
                orderByClause = orderByClause.Replace("8", ", IdConvenio1 ");
                orderByClause = orderByClause.Replace("9", ", IdConvenio2 ");
                orderByClause = orderByClause.Replace("10", ", IdConvenio3 ");
                orderByClause = orderByClause.Replace("11", ", IdTipoVehiculo ");
                orderByClause = orderByClause.Replace("12", ", IdEstacionamiento ");

                orderByClause = orderByClause.Replace("13", ", IdTarjeta ");



                orderByClause = orderByClause.Remove(0, 1);
            }
            else
            {
                orderByClause = "FechaEntrada ASC";
            }
            orderByClause = "ORDER BY " + orderByClause;

            sb.Clear();

            var numberOfRowsToReturn = "";
            numberOfRowsToReturn = iDisplayLength == -1 ? "TotalRows" : (iDisplayStart + iDisplayLength).ToString();

            string query = @"               
                            SELECT *
                            FROM
	                            (SELECT row_number() OVER ({0}) AS RowNumber
		                              , *
	                             FROM
		                             (SELECT (SELECT count(e.Nombre)
				                              FROM
					                              T_Transacciones as t inner join T_Estacionamientos as e on t.IdEstacionamiento=e.IdEstacionamiento inner join T_Configuracion as c on c.IdModulo=t.ModuloEntrada inner join T_Sedes as s on s.IdSede=e.IdSede inner join T_PermisosEstacionamientoUsuarios as peu on peu.IdEstacionamiento=t.IdEstacionamiento {4}) AS TotalRows
			                               , ( SELECT  count( e.Nombre) FROM T_Transacciones as t inner join T_Estacionamientos as e on t.IdEstacionamiento=e.IdEstacionamiento inner join T_Configuracion as c on c.IdModulo=t.ModuloEntrada inner join T_Sedes as s on s.IdSede=e.IdSede inner join T_PermisosEstacionamientoUsuarios as peu on peu.IdEstacionamiento=t.IdEstacionamiento {1} {4}) AS TotalDisplayRows			   
			                               , e.Nombre 
                                           , t.ModuloEntrada
                                           , t.FechaEntrada
                                           , t.PlacaEntrada
                                           , t.ModuloSalida
                                           , t.FechaSalida
                                           , t.PlacaSalida    
                                           , t.IdTransaccion   
                                           , t.IdConvenio1
                                           , t.IdConvenio2
                                           , t.IdConvenio3
                                           , t.IdTipoVehiculo 
                                           , t.IdEstacionamiento
                                           , t.IdTarjeta                                                             
		                              FROM
			                              T_Transacciones as t inner join T_Estacionamientos as e on t.IdEstacionamiento=e.IdEstacionamiento inner join T_Configuracion as c on c.IdModulo=t.ModuloEntrada inner join T_Sedes as s on s.IdSede=e.IdSede inner join T_PermisosEstacionamientoUsuarios as peu on peu.IdEstacionamiento=t.IdEstacionamiento {1} {4} ) RawResults) Results
                            WHERE
	                            RowNumber BETWEEN {2} AND {3}";


            query = String.Format(query, orderByClause, filteredWhere, iDisplayStart + 1, numberOfRowsToReturn, whereClause);

            //var connectionString = "Data Source=CECHECE-OFFLAP\\MSSQLSERVER2012;Initial Catalog=Parking;;User ID=sa;Password=3GL0B4LT12+";
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);

            try
            {
                conn.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            var DB = new SqlCommand();
            DB.Connection = conn;
            DB.CommandText = query;
            var data = DB.ExecuteReader();
            RegistrarAuditoria(TipoAccion.select, "t_transacciones", query);

            var totalDisplayRecords = "";
            var totalRecords = "";
            string outputJson = string.Empty;

            var rowClass = "";
            var count = 0;

            while (data.Read())
            {

                if (totalRecords.Length == 0)
                {
                    totalRecords = data["TotalRows"].ToString();
                    totalDisplayRecords = data["TotalDisplayRows"].ToString();
                }
                sb.Append("{");
                sb.AppendFormat(@"""DT_RowId"": ""{0}""", count++);
                sb.Append(",");
                sb.AppendFormat(@"""DT_RowClass"": ""{0}""", rowClass);
                sb.Append(",");
                sb.AppendFormat(@"""0"": ""{0}""", data["Nombre"]);
                sb.Append(",");
                sb.AppendFormat(@"""1"": ""{0}""", data["ModuloEntrada"]);
                sb.Append(",");
                sb.AppendFormat(@"""2"": ""{0}""", data["FechaEntrada"]);
                sb.Append(",");
                sb.AppendFormat(@"""3"": ""{0}""", data["PlacaEntrada"]);
                sb.Append(",");
                sb.AppendFormat(@"""4"": ""{0}""", data["ModuloSalida"]);
                sb.Append(",");
                sb.AppendFormat(@"""5"": ""{0}""", data["FechaSalida"]);
                sb.Append(",");
                sb.AppendFormat(@"""6"": ""{0}""", data["PlacaSalida"]);
                sb.Append(",");
                sb.AppendFormat(@"""7"": ""{0}""", data["IdTransaccion"]);
                sb.Append(",");
                sb.AppendFormat(@"""8"": ""{0}""", data["IdConvenio1"]);
                sb.Append(",");
                sb.AppendFormat(@"""9"": ""{0}""", data["IdConvenio2"]);
                sb.Append(",");
                sb.AppendFormat(@"""10"": ""{0}""", data["IdConvenio3"]);
                sb.Append(",");
                sb.AppendFormat(@"""11"": ""{0}""", data["IdTipoVehiculo"]);
                sb.Append(",");
                sb.AppendFormat(@"""12"": ""{0}""", data["IdEstacionamiento"]);

                sb.Append(",");
                sb.AppendFormat(@"""13"": ""{0}""", data["IdTarjeta"]);

                sb.Append("},");
            }

            // handles zero records
            if (totalRecords.Length == 0)
            {
                sb.Append("{");
                sb.Append(@"""sEcho"": ");
                sb.AppendFormat(@"""{0}""", sEcho);
                sb.Append(",");
                sb.Append(@"""iTotalRecords"": 0");
                sb.Append(",");
                sb.Append(@"""iTotalDisplayRecords"": 0");
                sb.Append(", ");
                sb.Append(@"""aaData"": [ ");
                sb.Append("]}");
                outputJson = sb.ToString();

                return outputJson;
            }
            outputJson = sb.Remove(sb.Length - 1, 1).ToString();
            sb.Clear();

            sb.Append("{");
            sb.Append(@"""sEcho"": ");
            sb.AppendFormat(@"""{0}""", sEcho);
            sb.Append(",");
            sb.Append(@"""iTotalRecords"": ");
            sb.Append(totalRecords);
            sb.Append(",");
            sb.Append(@"""iTotalDisplayRecords"": ");
            sb.Append(totalDisplayRecords);
            sb.Append(", ");
            sb.Append(@"""aaData"": [ ");
            sb.Append(outputJson);
            sb.Append("]}");
            outputJson = sb.ToString();

            return outputJson;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public IEnumerable GetItemsMovimientosPagos()
        {
            string idTransaccion = HttpContext.Current.Request.Params["idTransaccion"];

            DataTable dt = new DataTable();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            // string query = "SELECT IdMovimiento, IdTransaccion, IdEstacionamiento, IdCancelacion, IdModulo, Parte, Accion, Denominacion, Cantidad, Valor, FechaMovimiento FROM T_Movimientos where IdTransaccion = " + idTransaccion;

            string query = "SELECT IdMovimiento, IdTransaccion, IdEstacionamiento, IdCancelacion, IdModulo, Parte, Accion, Denominacion, Cantidad, Valor, FechaMovimiento FROM T_Movimientos  where IdTransaccion='" + idTransaccion + "'";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        RegistrarAuditoria(TipoAccion.select, "t_movimientos", query);
                        dt.Load(reader);
                    }
                }
            }

            var feeds =
                    from myRow in dt.AsEnumerable()
                    select new
                    {
                        IdMovimiento = myRow["IdMovimiento"],
                        IdTransaccion = myRow["IdTransaccion"],
                        IdEstacionamiento = myRow["IdEstacionamiento"],
                        IdCancelacion = myRow["IdCancelacion"],
                        IdModulo = myRow["IdModulo"],
                        Parte = myRow["Parte"],
                        Accion = myRow["Accion"],
                        Denominacion = myRow["Denominacion"],
                        Cantidad = myRow["Cantidad"],
                        Valor = myRow["Valor"],
                        FechaMovimiento = myRow["FechaMovimiento"],
                    };
            return feeds;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string GetItemsCargas()
        {
            int sEcho = ToInt(HttpContext.Current.Request.Params["sEcho"]);
            int iDisplayLength = ToInt(HttpContext.Current.Request.Params["iDisplayLength"]);
            int iDisplayStart = ToInt(HttpContext.Current.Request.Params["iDisplayStart"]);
            //string rawSearch = HttpContext.Current.Request.Params["sSearch"];

            string participant = HttpContext.Current.Request.Params["iIdEstacionamiento"];
            string fechaAntes = HttpContext.Current.Request.Params["sFechaAntes"];
            string fechaDespues = HttpContext.Current.Request.Params["sFechaDespues"];
            string carril = HttpContext.Current.Request.Params["sCarril"];
            string sede = HttpContext.Current.Request.Params["iIdSede"];

            var sb = new StringBuilder();

            var whereClause = string.Empty;
            if (participant.Length > 0 && participant != "0")
            {
                sb.Append(" Where c.IdEstacionamiento like ");
                sb.Append("'%" + participant + "%'");
                whereClause = sb.ToString();
            }
            sb.Clear();
            if (fechaAntes.Length > 0 && fechaDespues.Length > 0)
            {
                if (whereClause == string.Empty)
                {
                    sb.Append(" Where c.FechaInicio between ");
                }
                else
                {
                    sb.Append(" and c.FechaInicio between ");
                }
                sb.Append("convert(datetime,'" + fechaAntes + "',103) and convert(datetime,'" + fechaDespues + "',103)");
                //sb.Append("'" + fechaAntes + "' and '" + fechaDespues + "'");
                whereClause += sb.ToString();
            }
            sb.Clear();
            if (carril.Length > 0 && carril != "0")
            {
                if (whereClause == string.Empty)
                {
                    sb.Append(" Where c.IdModulo = '" + carril + "'");
                }
                else
                {
                    sb.Append(" and c.IdModulo = '" + carril + "'");
                }
                whereClause += sb.ToString();
            }
            sb.Clear();
            if (sede.Length > 0 && sede != "0")
            {
                if (whereClause == string.Empty)
                {
                    sb.Append(" Where s.idsede = '" + sede + "'");
                }
                else
                {
                    sb.Append(" and s.idsede = '" + sede + "'");
                }
                whereClause += sb.ToString();
            }
            sb.Clear();

            string user = HttpContext.Current.User.Identity.Name.ToString();


            if (whereClause == string.Empty)
            {
                sb.Append(" Where peu.DocumentoUsuario= '" + user + "'");
            }
            else
            {
                sb.Append(" and peu.DocumentoUsuario= '" + user + "'");
            }
            whereClause += sb.ToString();
            sb.Clear();

            var filteredWhere = string.Empty;

            //var wrappedSearch = "'%" + rawSearch + "%'";

            //if (rawSearch.Length > 0)
            //{
            //    sb.Append(" WHERE Nombre LIKE ");
            //    sb.Append(wrappedSearch);
            //    sb.Append(" OR t.ModuloEntrada LIKE ");
            //    sb.Append(wrappedSearch);
            //    sb.Append(" OR t.FechaEntrada LIKE ");
            //    sb.Append(wrappedSearch);
            //    sb.Append(" OR t.PlacaEntrada LIKE ");
            //    sb.Append(wrappedSearch);
            //    sb.Append(" OR t.ModuloSalida LIKE ");
            //    sb.Append(wrappedSearch);
            //    sb.Append(" OR t.FechaSalida LIKE ");
            //    sb.Append(wrappedSearch);
            //    sb.Append(" OR t.PlacaSalida LIKE ");
            //    sb.Append(wrappedSearch);

            //    filteredWhere = sb.ToString();
            //}


            //ORDERING

            sb.Clear();

            string orderByClause = string.Empty;
            sb.Append(ToInt(HttpContext.Current.Request.Params["iSortCol_0"]));

            sb.Append(" ");

            sb.Append(HttpContext.Current.Request.Params["sSortDir_0"]);

            orderByClause = sb.ToString();

            if (!String.IsNullOrEmpty(orderByClause))
            {
                if (orderByClause == "7 asc" || orderByClause == "7 desc")
                {
                    orderByClause = "FechaInicio ASC";
                }
                else
                {

                    orderByClause = orderByClause.Replace("0", ", IdCarga ");
                    orderByClause = orderByClause.Replace("1", ", FechaInicio ");
                    orderByClause = orderByClause.Replace("2", ", FechaFin ");
                    orderByClause = orderByClause.Replace("3", ", Valor ");
                    orderByClause = orderByClause.Replace("4", ", IdUsuario ");
                    orderByClause = orderByClause.Replace("5", ", IdModulo ");
                    orderByClause = orderByClause.Replace("6", ", IdEstacionamiento ");

                    orderByClause = orderByClause.Remove(0, 1);
                }
            }
            else
            {
                orderByClause = "FechaInicio ASC";
            }
            orderByClause = "ORDER BY " + orderByClause;

            sb.Clear();

            var numberOfRowsToReturn = "";
            numberOfRowsToReturn = iDisplayLength == -1 ? "TotalRows" : (iDisplayStart + iDisplayLength).ToString();

            string query = @"               
                            SELECT *
                            FROM
	                            (SELECT row_number() OVER ({0}) AS RowNumber
		                              , *
	                             FROM
		                             (SELECT (SELECT count(IdCarga)
				                              FROM
					                              T_Carga as c inner join T_Estacionamientos as e on c.IdEstacionamiento=e.IdEstacionamiento inner join T_Sedes as s on s.IdSede=e.IdSede inner join T_PermisosEstacionamientoUsuarios as peu on peu.IdEstacionamiento=c.IdEstacionamiento {4}) AS TotalRows
			                               , ( SELECT  count( IdCarga) FROM T_Carga as c inner join T_Estacionamientos as e on c.IdEstacionamiento=e.IdEstacionamiento inner join T_Sedes as s on s.IdSede=e.IdSede inner join T_PermisosEstacionamientoUsuarios as peu on peu.IdEstacionamiento=c.IdEstacionamiento {1} {4}) AS TotalDisplayRows			   
			                               , c.IdCarga 
                                           , c.FechaInicio
                                           , c.FechaFin
                                           , c.Valor
                                           , c.IdUsuario
                                           , c.IdModulo
                                           , c.IdEstacionamiento       
		                              FROM
			                             T_Carga as c inner join T_Estacionamientos as e on c.IdEstacionamiento=e.IdEstacionamiento inner join T_Sedes as s on s.IdSede=e.IdSede inner join T_PermisosEstacionamientoUsuarios as peu on peu.IdEstacionamiento=c.IdEstacionamiento {1} {4} ) RawResults) Results
                            WHERE
	                            RowNumber BETWEEN {2} AND {3}";


            query = String.Format(query, orderByClause, filteredWhere, iDisplayStart + 1, numberOfRowsToReturn, whereClause);

            //var connectionString = "Data Source=CECHECE-OFFLAP\\MSSQLSERVER2012;Initial Catalog=Parking;;User ID=sa;Password=3GL0B4LT12+";
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);

            try
            {
                conn.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            var DB = new SqlCommand();
            DB.Connection = conn;
            DB.CommandText = query;
            var data = DB.ExecuteReader();
            RegistrarAuditoria(TipoAccion.select, "t_carga", query);

            var totalDisplayRecords = "";
            var totalRecords = "";
            string outputJson = string.Empty;

            var rowClass = "";
            var count = 0;

            while (data.Read())
            {

                if (totalRecords.Length == 0)
                {
                    totalRecords = data["TotalRows"].ToString();
                    totalDisplayRecords = data["TotalDisplayRows"].ToString();
                }
                sb.Append("{");
                sb.AppendFormat(@"""DT_RowId"": ""{0}""", count++);
                sb.Append(",");
                sb.AppendFormat(@"""DT_RowClass"": ""{0}""", rowClass);
                sb.Append(",");
                sb.AppendFormat(@"""0"": ""{0}""", data["IdCarga"]);
                sb.Append(",");
                sb.AppendFormat(@"""1"": ""{0}""", data["FechaInicio"]);
                sb.Append(",");
                sb.AppendFormat(@"""2"": ""{0}""", data["FechaFin"]);
                sb.Append(",");
                sb.AppendFormat(@"""3"": ""{0}""", data["Valor"]);
                sb.Append(",");
                sb.AppendFormat(@"""4"": ""{0}""", data["IdUsuario"]);
                sb.Append(",");
                sb.AppendFormat(@"""5"": ""{0}""", data["IdModulo"]);
                sb.Append(",");
                sb.AppendFormat(@"""6"": ""{0}""", data["IdEstacionamiento"]);
                sb.Append("},");
            }

            // handles zero records
            if (totalRecords.Length == 0)
            {
                sb.Append("{");
                sb.Append(@"""sEcho"": ");
                sb.AppendFormat(@"""{0}""", sEcho);
                sb.Append(",");
                sb.Append(@"""iTotalRecords"": 0");
                sb.Append(",");
                sb.Append(@"""iTotalDisplayRecords"": 0");
                sb.Append(", ");
                sb.Append(@"""aaData"": [ ");
                sb.Append("]}");
                outputJson = sb.ToString();

                return outputJson;
            }
            outputJson = sb.Remove(sb.Length - 1, 1).ToString();
            sb.Clear();

            sb.Append("{");
            sb.Append(@"""sEcho"": ");
            sb.AppendFormat(@"""{0}""", sEcho);
            sb.Append(",");
            sb.Append(@"""iTotalRecords"": ");
            sb.Append(totalRecords);
            sb.Append(",");
            sb.Append(@"""iTotalDisplayRecords"": ");
            sb.Append(totalDisplayRecords);
            sb.Append(", ");
            sb.Append(@"""aaData"": [ ");
            sb.Append(outputJson);
            sb.Append("]}");
            outputJson = sb.ToString();

            return outputJson;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public IEnumerable GetItemsMovimientosCargas()
        {
            string idCarga = HttpContext.Current.Request.Params["idCarga"];

            DataTable dt = new DataTable();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            string query = "SELECT IdMovimiento, IdEstacionamiento, IdCancelacion, IdCarga, IdModulo, Parte, Accion, Denominacion, Cantidad, Valor, FechaMovimiento FROM T_Movimientos where IdCarga = " + idCarga;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        RegistrarAuditoria(TipoAccion.select, "t_movimientos", query);
                        dt.Load(reader);
                    }
                }
            }

            var feeds =
                    from myRow in dt.AsEnumerable()
                    select new
                    {
                        IdMovimiento = myRow["IdMovimiento"],
                        IdEstacionamiento = myRow["IdEstacionamiento"],
                        IdCancelacion = myRow["IdCancelacion"],
                        IdTransaccion = myRow["IdCarga"],
                        IdModulo = myRow["IdModulo"],
                        Parte = myRow["Parte"],
                        Accion = myRow["Accion"],
                        Denominacion = myRow["Denominacion"],
                        Cantidad = myRow["Cantidad"],
                        Valor = myRow["Valor"],
                        FechaMovimiento = myRow["FechaMovimiento"],
                    };
            return feeds;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string GetItemsArqueos()
        {
            int sEcho = ToInt(HttpContext.Current.Request.Params["sEcho"]);
            int iDisplayLength = ToInt(HttpContext.Current.Request.Params["iDisplayLength"]);
            int iDisplayStart = ToInt(HttpContext.Current.Request.Params["iDisplayStart"]);
            //string rawSearch = HttpContext.Current.Request.Params["sSearch"];

            string participant = HttpContext.Current.Request.Params["iIdEstacionamiento"];
            string fechaAntes = HttpContext.Current.Request.Params["sFechaAntes"];
            string fechaDespues = HttpContext.Current.Request.Params["sFechaDespues"];
            string carril = HttpContext.Current.Request.Params["sCarril"];
            string sede = HttpContext.Current.Request.Params["iIdSede"];

            var sb = new StringBuilder();

            var whereClause = string.Empty;
            if (participant.Length > 0 && participant != "0")
            {
                sb.Append(" Where a.IdEstacionamiento like ");
                sb.Append("'%" + participant + "%'");
                whereClause = sb.ToString();
            }
            sb.Clear();
            if (fechaAntes.Length > 0 && fechaDespues.Length > 0)
            {
                if (whereClause == string.Empty)
                {
                    sb.Append(" Where a.FechaInicio between ");
                }
                else
                {
                    sb.Append(" and a.FechaInicio between ");
                }
                sb.Append("convert(datetime,'" + fechaAntes + "',103) and convert(datetime,'" + fechaDespues + "',103)");
                //sb.Append("'" + fechaAntes + "' and '" + fechaDespues + "'");
                whereClause += sb.ToString();
            }
            sb.Clear();
            if (carril.Length > 0 && carril != "0")
            {
                if (whereClause == string.Empty)
                {
                    sb.Append(" Where a.IdModulo = '" + carril + "'");
                }
                else
                {
                    sb.Append(" and a.IdModulo = '" + carril + "'");
                }
                whereClause += sb.ToString();
            }
            sb.Clear();
            if (sede.Length > 0 && sede != "0")
            {
                if (whereClause == string.Empty)
                {
                    sb.Append(" Where s.idsede = '" + sede + "'");
                }
                else
                {
                    sb.Append(" and s.idsede = '" + sede + "'");
                }
                whereClause += sb.ToString();
            }
            sb.Clear();


            string user = HttpContext.Current.User.Identity.Name.ToString();


            if (whereClause == string.Empty)
            {
                sb.Append(" Where peu.DocumentoUsuario= '" + user + "'");
            }
            else
            {
                sb.Append(" and peu.DocumentoUsuario= '" + user + "'");
            }
            whereClause += sb.ToString();
            sb.Clear();

            var filteredWhere = string.Empty;

            //var wrappedSearch = "'%" + rawSearch + "%'";

            //if (rawSearch.Length > 0)
            //{
            //    sb.Append(" WHERE Nombre LIKE ");
            //    sb.Append(wrappedSearch);
            //    sb.Append(" OR t.ModuloEntrada LIKE ");
            //    sb.Append(wrappedSearch);
            //    sb.Append(" OR t.FechaEntrada LIKE ");
            //    sb.Append(wrappedSearch);
            //    sb.Append(" OR t.PlacaEntrada LIKE ");
            //    sb.Append(wrappedSearch);
            //    sb.Append(" OR t.ModuloSalida LIKE ");
            //    sb.Append(wrappedSearch);
            //    sb.Append(" OR t.FechaSalida LIKE ");
            //    sb.Append(wrappedSearch);
            //    sb.Append(" OR t.PlacaSalida LIKE ");
            //    sb.Append(wrappedSearch);

            //    filteredWhere = sb.ToString();
            //}


            //ORDERING

            sb.Clear();

            string orderByClause = string.Empty;
            sb.Append(ToInt(HttpContext.Current.Request.Params["iSortCol_0"]));

            sb.Append(" ");

            sb.Append(HttpContext.Current.Request.Params["sSortDir_0"]);

            orderByClause = sb.ToString();

            if (!String.IsNullOrEmpty(orderByClause))
            {
                if (orderByClause == "10 asc" || orderByClause == "10 desc")
                {
                    orderByClause = "FechaInicio ASC";
                }
                else
                {

                    orderByClause = orderByClause.Replace("0", ", IdArqueo ");
                    orderByClause = orderByClause.Replace("1", ", FechaInicio ");
                    orderByClause = orderByClause.Replace("2", ", FechaFin ");
                    orderByClause = orderByClause.Replace("3", ", Valor ");
                    orderByClause = orderByClause.Replace("4", ", IdUsuario ");
                    orderByClause = orderByClause.Replace("5", ", IdModulo ");
                    orderByClause = orderByClause.Replace("6", ", IdEstacionamiento ");
                    orderByClause = orderByClause.Replace("7", ", CantTransacciones ");
                    orderByClause = orderByClause.Replace("8", ", Producido ");
                    orderByClause = orderByClause.Replace("9", ", Tipo ");
                    orderByClause = orderByClause.Replace("10", ", Conteo ");

                    orderByClause = orderByClause.Remove(0, 1);
                }
            }
            else
            {
                orderByClause = "FechaInicio ASC";
            }
            orderByClause = "ORDER BY " + orderByClause;

            sb.Clear();

            var numberOfRowsToReturn = "";
            numberOfRowsToReturn = iDisplayLength == -1 ? "TotalRows" : (iDisplayStart + iDisplayLength).ToString();

            string query = @"               
                            SELECT *
                            FROM
	                            (SELECT row_number() OVER ({0}) AS RowNumber
		                              , *
	                             FROM
		                             (SELECT (SELECT count(IdArqueo)
				                              FROM
					                              T_Arqueos as a inner join T_Estacionamientos as e on a.IdEstacionamiento=e.IdEstacionamiento inner join T_Sedes as s on s.IdSede=e.IdSede inner join T_PermisosEstacionamientoUsuarios as peu on peu.IdEstacionamiento=a.IdEstacionamiento {4}) AS TotalRows
			                               , ( SELECT  count( IdArqueo) FROM T_Arqueos as a inner join T_Estacionamientos as e on a.IdEstacionamiento=e.IdEstacionamiento inner join T_Sedes as s on s.IdSede=e.IdSede inner join T_PermisosEstacionamientoUsuarios as peu on peu.IdEstacionamiento=a.IdEstacionamiento {1} {4}) AS TotalDisplayRows			   
			                               , a.IdArqueo 
                                           , a.FechaInicio
                                           , a.FechaFin
                                           , a.Valor
                                           , a.IdUsuario
                                           , a.IdModulo
                                           , a.IdEstacionamiento       
                                           , a.CantTransacciones    
                                           , a.Producido    
                                           , a.Tipo
                                           , a.Conteo
		                              FROM
			                             T_Arqueos as a inner join T_Estacionamientos as e on a.IdEstacionamiento=e.IdEstacionamiento inner join T_Sedes as s on s.IdSede=e.IdSede inner join T_PermisosEstacionamientoUsuarios as peu on peu.IdEstacionamiento=a.IdEstacionamiento {1} {4} ) RawResults) Results
                            WHERE
	                            RowNumber BETWEEN {2} AND {3}";


            query = String.Format(query, orderByClause, filteredWhere, iDisplayStart + 1, numberOfRowsToReturn, whereClause);

            //var connectionString = "Data Source=CECHECE-OFFLAP\\MSSQLSERVER2012;Initial Catalog=Parking;;User ID=sa;Password=3GL0B4LT12+";
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);

            try
            {
                conn.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            var DB = new SqlCommand();
            DB.Connection = conn;
            DB.CommandText = query;
            var data = DB.ExecuteReader();
            RegistrarAuditoria(TipoAccion.select, "t_arqueos", query);

            var totalDisplayRecords = "";
            var totalRecords = "";
            string outputJson = string.Empty;

            var rowClass = "";
            var count = 0;

            while (data.Read())
            {

                if (totalRecords.Length == 0)
                {
                    totalRecords = data["TotalRows"].ToString();
                    totalDisplayRecords = data["TotalDisplayRows"].ToString();
                }
                sb.Append("{");
                sb.AppendFormat(@"""DT_RowId"": ""{0}""", count++);
                sb.Append(",");
                sb.AppendFormat(@"""DT_RowClass"": ""{0}""", rowClass);
                sb.Append(",");
                sb.AppendFormat(@"""0"": ""{0}""", data["IdArqueo"]);
                sb.Append(",");
                sb.AppendFormat(@"""1"": ""{0}""", data["FechaInicio"]);
                sb.Append(",");
                sb.AppendFormat(@"""2"": ""{0}""", data["FechaFin"]);
                sb.Append(",");
                sb.AppendFormat(@"""3"": ""{0}""", data["Valor"]);
                sb.Append(",");
                sb.AppendFormat(@"""4"": ""{0}""", data["IdUsuario"]);
                sb.Append(",");
                sb.AppendFormat(@"""5"": ""{0}""", data["IdModulo"]);
                sb.Append(",");
                sb.AppendFormat(@"""6"": ""{0}""", data["IdEstacionamiento"]);
                sb.Append(",");
                sb.AppendFormat(@"""7"": ""{0}""", data["CantTransacciones"]);
                sb.Append(",");
                sb.AppendFormat(@"""8"": ""{0}""", data["Producido"]);
                sb.Append(",");
                sb.AppendFormat(@"""9"": ""{0}""", data["Tipo"]);
                sb.Append(",");
                sb.AppendFormat(@"""10"": ""{0}""", data["Conteo"]);
                sb.Append("},");
            }

            // handles zero records
            if (totalRecords.Length == 0)
            {
                sb.Append("{");
                sb.Append(@"""sEcho"": ");
                sb.AppendFormat(@"""{0}""", sEcho);
                sb.Append(",");
                sb.Append(@"""iTotalRecords"": 0");
                sb.Append(",");
                sb.Append(@"""iTotalDisplayRecords"": 0");
                sb.Append(", ");
                sb.Append(@"""aaData"": [ ");
                sb.Append("]}");
                outputJson = sb.ToString();

                return outputJson;
            }
            outputJson = sb.Remove(sb.Length - 1, 1).ToString();
            sb.Clear();

            sb.Append("{");
            sb.Append(@"""sEcho"": ");
            sb.AppendFormat(@"""{0}""", sEcho);
            sb.Append(",");
            sb.Append(@"""iTotalRecords"": ");
            sb.Append(totalRecords);
            sb.Append(",");
            sb.Append(@"""iTotalDisplayRecords"": ");
            sb.Append(totalDisplayRecords);
            sb.Append(", ");
            sb.Append(@"""aaData"": [ ");
            sb.Append(outputJson);
            sb.Append("]}");
            outputJson = sb.ToString();

            return outputJson;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public IEnumerable GetItemsMovimientosArqueos()
        {
            string idArqueo = HttpContext.Current.Request.Params["idArqueo"];

            DataTable dt = new DataTable();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            string query = "SELECT IdMovimiento, IdEstacionamiento, IdCancelacion, IdArqueo, IdModulo, Parte, Accion, Denominacion, Cantidad, Valor, FechaMovimiento FROM T_Movimientos where IdArqueo = " + idArqueo;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        RegistrarAuditoria(TipoAccion.select, "t_movimientos", query);
                        dt.Load(reader);
                    }
                }
            }

            var feeds =
                    from myRow in dt.AsEnumerable()
                    select new
                    {
                        IdMovimiento = myRow["IdMovimiento"],
                        IdEstacionamiento = myRow["IdEstacionamiento"],
                        IdCancelacion = myRow["IdCancelacion"],
                        IdTransaccion = myRow["IdArqueo"],
                        IdModulo = myRow["IdModulo"],
                        Parte = myRow["Parte"],
                        Accion = myRow["Accion"],
                        Denominacion = myRow["Denominacion"],
                        Cantidad = myRow["Cantidad"],
                        Valor = myRow["Valor"],
                        FechaMovimiento = myRow["FechaMovimiento"],
                    };
            return feeds;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string GetItemsAlarmasSinAtender()
        {
            int sEcho = ToInt(HttpContext.Current.Request.Params["sEcho"]);
            int iDisplayLength = ToInt(HttpContext.Current.Request.Params["iDisplayLength"]);
            int iDisplayStart = ToInt(HttpContext.Current.Request.Params["iDisplayStart"]);
            //string rawSearch = HttpContext.Current.Request.Params["sSearch"];

            string participant = HttpContext.Current.Request.Params["iIdEstacionamiento"];
            //string fechaAntes = HttpContext.Current.Request.Params["sFechaAntes"];
            //string fechaDespues = HttpContext.Current.Request.Params["sFechaDespues"];
            //string carril = HttpContext.Current.Request.Params["sCarril"];

            var sb = new StringBuilder();

            var whereClause = string.Empty;
            if (participant.Length > 0 && participant != "0")
            {
                sb.Append(" and a.IdEstacionamiento like ");
                sb.Append("'%" + participant + "%'");
                whereClause = sb.ToString();
            }
            sb.Clear();
            //if (fechaAntes.Length > 0 && fechaDespues.Length > 0)
            //{
            //    if (whereClause == string.Empty)
            //    {
            //        sb.Append(" Where a.FechaInicio between ");
            //    }
            //    else
            //    {
            //        sb.Append(" and a.FechaInicio between ");
            //    }
            //    sb.Append("'" + fechaAntes + "' and '" + fechaDespues + "'");
            //    whereClause += sb.ToString();
            //}
            //sb.Clear();
            //if (carril.Length > 0 && carril != "0")
            //{
            //    if (whereClause == string.Empty)
            //    {
            //        sb.Append(" Where a.IdModulo = '" + carril + "'");
            //    }
            //    else
            //    {
            //        sb.Append(" and a.IdModulo = '" + carril + "'");
            //    }
            //    whereClause += sb.ToString();
            //}
            //sb.Clear();


            var filteredWhere = string.Empty;

            //var wrappedSearch = "'%" + rawSearch + "%'";

            //if (rawSearch.Length > 0)
            //{
            //    sb.Append(" WHERE Nombre LIKE ");
            //    sb.Append(wrappedSearch);
            //    sb.Append(" OR t.ModuloEntrada LIKE ");
            //    sb.Append(wrappedSearch);
            //    sb.Append(" OR t.FechaEntrada LIKE ");
            //    sb.Append(wrappedSearch);
            //    sb.Append(" OR t.PlacaEntrada LIKE ");
            //    sb.Append(wrappedSearch);
            //    sb.Append(" OR t.ModuloSalida LIKE ");
            //    sb.Append(wrappedSearch);
            //    sb.Append(" OR t.FechaSalida LIKE ");
            //    sb.Append(wrappedSearch);
            //    sb.Append(" OR t.PlacaSalida LIKE ");
            //    sb.Append(wrappedSearch);

            //    filteredWhere = sb.ToString();
            //}


            //ORDERING

            sb.Clear();

            string orderByClause = string.Empty;
            sb.Append(ToInt(HttpContext.Current.Request.Params["iSortCol_0"]));

            sb.Append(" ");

            sb.Append(HttpContext.Current.Request.Params["sSortDir_0"]);

            orderByClause = sb.ToString();

            if (!String.IsNullOrEmpty(orderByClause))
            {


                orderByClause = orderByClause.Replace("1", ", IdAlarma ");
                orderByClause = orderByClause.Replace("2", ", IdModulo ");
                orderByClause = orderByClause.Replace("3", ", IdEstacionamiento ");
                orderByClause = orderByClause.Replace("4", ", TipoError ");
                orderByClause = orderByClause.Replace("5", ", Parte ");
                orderByClause = orderByClause.Replace("6", ", Descripcion ");
                orderByClause = orderByClause.Replace("7", ", NivelError ");
                orderByClause = orderByClause.Replace("8", ", FechaRegistro ");
                orderByClause = orderByClause.Replace("9", ", Observacion ");
                orderByClause = orderByClause.Replace("10", ", FechaAtencion ");
                orderByClause = orderByClause.Replace("11", ", UsuarioObservacion ");
                orderByClause = orderByClause.Replace("12", ", FechaSolucion ");

                orderByClause = orderByClause.Remove(0, 1);

            }
            else
            {
                orderByClause = "FechaInicio ASC";
            }
            orderByClause = "ORDER BY " + orderByClause;

            sb.Clear();

            var numberOfRowsToReturn = "";
            numberOfRowsToReturn = iDisplayLength == -1 ? "TotalRows" : (iDisplayStart + iDisplayLength).ToString();

            string query = @"               
                            SELECT *
                            FROM
	                            (SELECT row_number() OVER ({0}) AS RowNumber
		                              , *
	                             FROM
		                             (SELECT (SELECT count(IdAlarma)
				                              FROM
					                              T_Alarmas as a where FechaAtencion is null and FechaSolucion is null) AS TotalRows
			                               , ( SELECT  count( IdAlarma) FROM T_Alarmas as a {1} where FechaAtencion is null and FechaSolucion is null) AS TotalDisplayRows			   
			                               , a.IdAlarma 
                                           , a.IdModulo
                                           , a.IdEstacionamiento
                                           , a.TipoError
                                           , a.Parte
                                           , a.Descripcion
                                           , a.NivelError       
                                           , a.FechaRegistro    
                                           , a.Observacion    
                                           , a.FechaAtencion
                                           , a.UsuarioObservacion
                                           , a.FechaSolucion
		                              FROM
			                             T_Alarmas as a {1} where FechaAtencion is null and FechaSolucion is null {4} ) RawResults) Results
                            WHERE
	                            RowNumber BETWEEN {2} AND {3}";


            query = String.Format(query, orderByClause, filteredWhere, iDisplayStart + 1, numberOfRowsToReturn, whereClause);

            //var connectionString = "Data Source=CECHECE-OFFLAP\\MSSQLSERVER2012;Initial Catalog=Parking;;User ID=sa;Password=3GL0B4LT12+";
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);

            try
            {
                conn.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            var DB = new SqlCommand();
            DB.Connection = conn;
            DB.CommandText = query;
            var data = DB.ExecuteReader();
            RegistrarAuditoria(TipoAccion.select, "t_alarmas", query);

            var totalDisplayRecords = "";
            var totalRecords = "";
            string outputJson = string.Empty;

            var rowClass = "";
            var count = 0;

            while (data.Read())
            {

                if (totalRecords.Length == 0)
                {
                    totalRecords = data["TotalRows"].ToString();
                    totalDisplayRecords = data["TotalDisplayRows"].ToString();
                }
                sb.Append("{");
                sb.AppendFormat(@"""DT_RowId"": ""{0}""", count++);
                sb.Append(",");
                sb.AppendFormat(@"""DT_RowClass"": ""{0}""", rowClass);
                sb.Append(",");
                sb.AppendFormat(@"""1"": ""{0}""", data["IdAlarma"]);
                sb.Append(",");
                sb.AppendFormat(@"""2"": ""{0}""", data["IdModulo"]);
                sb.Append(",");
                sb.AppendFormat(@"""3"": ""{0}""", data["IdEstacionamiento"]);
                sb.Append(",");
                sb.AppendFormat(@"""4"": ""{0}""", data["TipoError"]);
                sb.Append(",");
                sb.AppendFormat(@"""5"": ""{0}""", data["Parte"]);
                sb.Append(",");
                sb.AppendFormat(@"""6"": ""{0}""", data["Descripcion"]);
                sb.Append(",");
                sb.AppendFormat(@"""7"": ""{0}""", data["NivelError"]);
                sb.Append(",");
                sb.AppendFormat(@"""8"": ""{0}""", data["FechaRegistro"]);
                sb.Append(",");
                sb.AppendFormat(@"""9"": ""{0}""", data["Observacion"]);
                sb.Append(",");
                sb.AppendFormat(@"""10"": ""{0}""", data["FechaAtencion"]);
                sb.Append(",");
                sb.AppendFormat(@"""11"": ""{0}""", data["UsuarioObservacion"]);
                sb.Append(",");
                sb.AppendFormat(@"""12"": ""{0}""", data["FechaSolucion"]);
                sb.Append("},");
            }

            // handles zero records
            if (totalRecords.Length == 0)
            {
                sb.Append("{");
                sb.Append(@"""sEcho"": ");
                sb.AppendFormat(@"""{0}""", sEcho);
                sb.Append(",");
                sb.Append(@"""iTotalRecords"": 0");
                sb.Append(",");
                sb.Append(@"""iTotalDisplayRecords"": 0");
                sb.Append(", ");
                sb.Append(@"""aaData"": [ ");
                sb.Append("]}");
                outputJson = sb.ToString();

                return outputJson;
            }
            outputJson = sb.Remove(sb.Length - 1, 1).ToString();
            sb.Clear();

            sb.Append("{");
            sb.Append(@"""sEcho"": ");
            sb.AppendFormat(@"""{0}""", sEcho);
            sb.Append(",");
            sb.Append(@"""iTotalRecords"": ");
            sb.Append(totalRecords);
            sb.Append(",");
            sb.Append(@"""iTotalDisplayRecords"": ");
            sb.Append(totalDisplayRecords);
            sb.Append(", ");
            sb.Append(@"""aaData"": [ ");
            sb.Append(outputJson);
            sb.Append("]}");
            outputJson = sb.ToString();

            return outputJson;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string GetItemsAlarmasSinSolucionar()
        {
            int sEcho = ToInt(HttpContext.Current.Request.Params["sEcho"]);
            int iDisplayLength = ToInt(HttpContext.Current.Request.Params["iDisplayLength"]);
            int iDisplayStart = ToInt(HttpContext.Current.Request.Params["iDisplayStart"]);
            //string rawSearch = HttpContext.Current.Request.Params["sSearch"];

            string participant = HttpContext.Current.Request.Params["iIdEstacionamiento"];
            //string fechaAntes = HttpContext.Current.Request.Params["sFechaAntes"];
            //string fechaDespues = HttpContext.Current.Request.Params["sFechaDespues"];
            //string carril = HttpContext.Current.Request.Params["sCarril"];

            var sb = new StringBuilder();

            var whereClause = string.Empty;
            if (participant.Length > 0 && participant != "0")
            {
                sb.Append(" and a.IdEstacionamiento like ");
                sb.Append("'%" + participant + "%'");
                whereClause = sb.ToString();
            }
            sb.Clear();
            //if (fechaAntes.Length > 0 && fechaDespues.Length > 0)
            //{
            //    if (whereClause == string.Empty)
            //    {
            //        sb.Append(" Where a.FechaInicio between ");
            //    }
            //    else
            //    {
            //        sb.Append(" and a.FechaInicio between ");
            //    }
            //    sb.Append("'" + fechaAntes + "' and '" + fechaDespues + "'");
            //    whereClause += sb.ToString();
            //}
            //sb.Clear();
            //if (carril.Length > 0 && carril != "0")
            //{
            //    if (whereClause == string.Empty)
            //    {
            //        sb.Append(" Where a.IdModulo = '" + carril + "'");
            //    }
            //    else
            //    {
            //        sb.Append(" and a.IdModulo = '" + carril + "'");
            //    }
            //    whereClause += sb.ToString();
            //}
            //sb.Clear();


            var filteredWhere = string.Empty;

            //var wrappedSearch = "'%" + rawSearch + "%'";

            //if (rawSearch.Length > 0)
            //{
            //    sb.Append(" WHERE Nombre LIKE ");
            //    sb.Append(wrappedSearch);
            //    sb.Append(" OR t.ModuloEntrada LIKE ");
            //    sb.Append(wrappedSearch);
            //    sb.Append(" OR t.FechaEntrada LIKE ");
            //    sb.Append(wrappedSearch);
            //    sb.Append(" OR t.PlacaEntrada LIKE ");
            //    sb.Append(wrappedSearch);
            //    sb.Append(" OR t.ModuloSalida LIKE ");
            //    sb.Append(wrappedSearch);
            //    sb.Append(" OR t.FechaSalida LIKE ");
            //    sb.Append(wrappedSearch);
            //    sb.Append(" OR t.PlacaSalida LIKE ");
            //    sb.Append(wrappedSearch);

            //    filteredWhere = sb.ToString();
            //}


            //ORDERING

            sb.Clear();

            string orderByClause = string.Empty;
            sb.Append(ToInt(HttpContext.Current.Request.Params["iSortCol_0"]));

            sb.Append(" ");

            sb.Append(HttpContext.Current.Request.Params["sSortDir_0"]);

            orderByClause = sb.ToString();

            if (!String.IsNullOrEmpty(orderByClause))
            {


                orderByClause = orderByClause.Replace("1", ", IdAlarma ");
                orderByClause = orderByClause.Replace("2", ", IdModulo ");
                orderByClause = orderByClause.Replace("3", ", IdEstacionamiento ");
                orderByClause = orderByClause.Replace("4", ", TipoError ");
                orderByClause = orderByClause.Replace("5", ", Parte ");
                orderByClause = orderByClause.Replace("6", ", Descripcion ");
                orderByClause = orderByClause.Replace("7", ", NivelError ");
                orderByClause = orderByClause.Replace("8", ", FechaRegistro ");
                orderByClause = orderByClause.Replace("9", ", Observacion ");
                orderByClause = orderByClause.Replace("10", ", FechaAtencion ");
                orderByClause = orderByClause.Replace("11", ", UsuarioObservacion ");
                orderByClause = orderByClause.Replace("12", ", FechaSolucion ");

                orderByClause = orderByClause.Remove(0, 1);

            }
            else
            {
                orderByClause = "FechaInicio ASC";
            }
            orderByClause = "ORDER BY " + orderByClause;

            sb.Clear();

            var numberOfRowsToReturn = "";
            numberOfRowsToReturn = iDisplayLength == -1 ? "TotalRows" : (iDisplayStart + iDisplayLength).ToString();

            string query = @"               
                            SELECT *
                            FROM
	                            (SELECT row_number() OVER ({0}) AS RowNumber
		                              , *
	                             FROM
		                             (SELECT (SELECT count(IdAlarma)
				                              FROM
					                              T_Alarmas as a where FechaSolucion is null and FechaAtencion is not null) AS TotalRows
			                               , ( SELECT  count( IdAlarma) FROM T_Alarmas as a {1} where FechaSolucion is null and FechaAtencion is not null) AS TotalDisplayRows			   
			                               , a.IdAlarma 
                                           , a.IdModulo
                                           , a.IdEstacionamiento
                                           , a.TipoError
                                           , a.Parte
                                           , a.Descripcion
                                           , a.NivelError       
                                           , a.FechaRegistro    
                                           , a.Observacion    
                                           , a.FechaAtencion
                                           , a.UsuarioObservacion
                                           , a.FechaSolucion
		                              FROM
			                             T_Alarmas as a {1} where FechaSolucion is null and FechaAtencion is not null {4} ) RawResults) Results
                            WHERE
	                            RowNumber BETWEEN {2} AND {3}";


            query = String.Format(query, orderByClause, filteredWhere, iDisplayStart + 1, numberOfRowsToReturn, whereClause);

            //var connectionString = "Data Source=CECHECE-OFFLAP\\MSSQLSERVER2012;Initial Catalog=Parking;;User ID=sa;Password=3GL0B4LT12+";
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);

            try
            {
                conn.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            var DB = new SqlCommand();
            DB.Connection = conn;
            DB.CommandText = query;
            var data = DB.ExecuteReader();

            RegistrarAuditoria(TipoAccion.select, "t_alarmas", query);

            var totalDisplayRecords = "";
            var totalRecords = "";
            string outputJson = string.Empty;

            var rowClass = "";
            var count = 0;

            while (data.Read())
            {

                if (totalRecords.Length == 0)
                {
                    totalRecords = data["TotalRows"].ToString();
                    totalDisplayRecords = data["TotalDisplayRows"].ToString();
                }
                sb.Append("{");
                sb.AppendFormat(@"""DT_RowId"": ""{0}""", count++);
                sb.Append(",");
                sb.AppendFormat(@"""DT_RowClass"": ""{0}""", rowClass);
                sb.Append(",");
                sb.AppendFormat(@"""1"": ""{0}""", data["IdAlarma"]);
                sb.Append(",");
                sb.AppendFormat(@"""2"": ""{0}""", data["IdModulo"]);
                sb.Append(",");
                sb.AppendFormat(@"""3"": ""{0}""", data["IdEstacionamiento"]);
                sb.Append(",");
                sb.AppendFormat(@"""4"": ""{0}""", data["TipoError"]);
                sb.Append(",");
                sb.AppendFormat(@"""5"": ""{0}""", data["Parte"]);
                sb.Append(",");
                sb.AppendFormat(@"""6"": ""{0}""", data["Descripcion"]);
                sb.Append(",");
                sb.AppendFormat(@"""7"": ""{0}""", data["NivelError"]);
                sb.Append(",");
                sb.AppendFormat(@"""8"": ""{0}""", data["FechaRegistro"]);
                sb.Append(",");
                sb.AppendFormat(@"""9"": ""{0}""", data["Observacion"]);
                sb.Append(",");
                sb.AppendFormat(@"""10"": ""{0}""", data["FechaAtencion"]);
                sb.Append(",");
                sb.AppendFormat(@"""11"": ""{0}""", data["UsuarioObservacion"]);
                sb.Append(",");
                sb.AppendFormat(@"""12"": ""{0}""", data["FechaSolucion"]);
                sb.Append("},");
            }

            // handles zero records
            if (totalRecords.Length == 0)
            {
                sb.Append("{");
                sb.Append(@"""sEcho"": ");
                sb.AppendFormat(@"""{0}""", sEcho);
                sb.Append(",");
                sb.Append(@"""iTotalRecords"": 0");
                sb.Append(",");
                sb.Append(@"""iTotalDisplayRecords"": 0");
                sb.Append(", ");
                sb.Append(@"""aaData"": [ ");
                sb.Append("]}");
                outputJson = sb.ToString();

                return outputJson;
            }
            outputJson = sb.Remove(sb.Length - 1, 1).ToString();
            sb.Clear();

            sb.Append("{");
            sb.Append(@"""sEcho"": ");
            sb.AppendFormat(@"""{0}""", sEcho);
            sb.Append(",");
            sb.Append(@"""iTotalRecords"": ");
            sb.Append(totalRecords);
            sb.Append(",");
            sb.Append(@"""iTotalDisplayRecords"": ");
            sb.Append(totalDisplayRecords);
            sb.Append(", ");
            sb.Append(@"""aaData"": [ ");
            sb.Append(outputJson);
            sb.Append("]}");
            outputJson = sb.ToString();

            return outputJson;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string GetItemsAlarmasSolucionadas()
        {
            int sEcho = ToInt(HttpContext.Current.Request.Params["sEcho"]);
            int iDisplayLength = ToInt(HttpContext.Current.Request.Params["iDisplayLength"]);
            int iDisplayStart = ToInt(HttpContext.Current.Request.Params["iDisplayStart"]);
            //string rawSearch = HttpContext.Current.Request.Params["sSearch"];

            string participant = HttpContext.Current.Request.Params["iIdEstacionamiento"];
            //string fechaAntes = HttpContext.Current.Request.Params["sFechaAntes"];
            //string fechaDespues = HttpContext.Current.Request.Params["sFechaDespues"];
            //string carril = HttpContext.Current.Request.Params["sCarril"];

            var sb = new StringBuilder();

            var whereClause = string.Empty;
            if (participant.Length > 0 && participant != "0")
            {
                sb.Append(" and a.IdEstacionamiento like ");
                sb.Append("'%" + participant + "%'");
                whereClause = sb.ToString();
            }
            sb.Clear();
            //if (fechaAntes.Length > 0 && fechaDespues.Length > 0)
            //{
            //    if (whereClause == string.Empty)
            //    {
            //        sb.Append(" Where a.FechaInicio between ");
            //    }
            //    else
            //    {
            //        sb.Append(" and a.FechaInicio between ");
            //    }
            //    sb.Append("'" + fechaAntes + "' and '" + fechaDespues + "'");
            //    whereClause += sb.ToString();
            //}
            //sb.Clear();
            //if (carril.Length > 0 && carril != "0")
            //{
            //    if (whereClause == string.Empty)
            //    {
            //        sb.Append(" Where a.IdModulo = '" + carril + "'");
            //    }
            //    else
            //    {
            //        sb.Append(" and a.IdModulo = '" + carril + "'");
            //    }
            //    whereClause += sb.ToString();
            //}
            //sb.Clear();


            var filteredWhere = string.Empty;

            //var wrappedSearch = "'%" + rawSearch + "%'";

            //if (rawSearch.Length > 0)
            //{
            //    sb.Append(" WHERE Nombre LIKE ");
            //    sb.Append(wrappedSearch);
            //    sb.Append(" OR t.ModuloEntrada LIKE ");
            //    sb.Append(wrappedSearch);
            //    sb.Append(" OR t.FechaEntrada LIKE ");
            //    sb.Append(wrappedSearch);
            //    sb.Append(" OR t.PlacaEntrada LIKE ");
            //    sb.Append(wrappedSearch);
            //    sb.Append(" OR t.ModuloSalida LIKE ");
            //    sb.Append(wrappedSearch);
            //    sb.Append(" OR t.FechaSalida LIKE ");
            //    sb.Append(wrappedSearch);
            //    sb.Append(" OR t.PlacaSalida LIKE ");
            //    sb.Append(wrappedSearch);

            //    filteredWhere = sb.ToString();
            //}


            //ORDERING

            sb.Clear();

            string orderByClause = string.Empty;
            sb.Append(ToInt(HttpContext.Current.Request.Params["iSortCol_0"]));

            sb.Append(" ");

            sb.Append(HttpContext.Current.Request.Params["sSortDir_0"]);

            orderByClause = sb.ToString();

            if (!String.IsNullOrEmpty(orderByClause))
            {


                orderByClause = orderByClause.Replace("1", ", IdAlarma ");
                orderByClause = orderByClause.Replace("2", ", IdModulo ");
                orderByClause = orderByClause.Replace("3", ", IdEstacionamiento ");
                orderByClause = orderByClause.Replace("4", ", TipoError ");
                orderByClause = orderByClause.Replace("5", ", Parte ");
                orderByClause = orderByClause.Replace("6", ", Descripcion ");
                orderByClause = orderByClause.Replace("7", ", NivelError ");
                orderByClause = orderByClause.Replace("8", ", FechaRegistro ");
                orderByClause = orderByClause.Replace("9", ", Observacion ");
                orderByClause = orderByClause.Replace("10", ", FechaAtencion ");
                orderByClause = orderByClause.Replace("11", ", UsuarioObservacion ");
                orderByClause = orderByClause.Replace("12", ", FechaSolucion ");

                orderByClause = orderByClause.Remove(0, 1);

            }
            else
            {
                orderByClause = "FechaInicio ASC";
            }
            orderByClause = "ORDER BY " + orderByClause;

            sb.Clear();

            var numberOfRowsToReturn = "";
            numberOfRowsToReturn = iDisplayLength == -1 ? "TotalRows" : (iDisplayStart + iDisplayLength).ToString();

            string query = @"               
                            SELECT *
                            FROM
	                            (SELECT row_number() OVER ({0}) AS RowNumber
		                              , *
	                             FROM
		                             (SELECT (SELECT count(IdAlarma)
				                              FROM
					                              T_Alarmas as a where FechaSolucion is not null and FechaAtencion is not null) AS TotalRows
			                               , ( SELECT  count( IdAlarma) FROM T_Alarmas as a {1} where FechaSolucion is not null and FechaAtencion is not null) AS TotalDisplayRows			   
			                               , a.IdAlarma 
                                           , a.IdModulo
                                           , a.IdEstacionamiento
                                           , a.TipoError
                                           , a.Parte
                                           , a.Descripcion
                                           , a.NivelError       
                                           , a.FechaRegistro    
                                           , a.Observacion    
                                           , a.FechaAtencion
                                           , a.UsuarioObservacion
                                           , a.FechaSolucion
		                              FROM
			                             T_Alarmas as a {1} where FechaSolucion is not null and FechaAtencion is not null {4} ) RawResults) Results
                            WHERE
	                            RowNumber BETWEEN {2} AND {3}";


            query = String.Format(query, orderByClause, filteredWhere, iDisplayStart + 1, numberOfRowsToReturn, whereClause);

            //var connectionString = "Data Source=CECHECE-OFFLAP\\MSSQLSERVER2012;Initial Catalog=Parking;;User ID=sa;Password=3GL0B4LT12+";
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);

            try
            {
                conn.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            var DB = new SqlCommand();
            DB.Connection = conn;
            DB.CommandText = query;
            var data = DB.ExecuteReader();

            RegistrarAuditoria(TipoAccion.select, "t_alarmas", query);

            var totalDisplayRecords = "";
            var totalRecords = "";
            string outputJson = string.Empty;

            var rowClass = "";
            var count = 0;

            while (data.Read())
            {

                if (totalRecords.Length == 0)
                {
                    totalRecords = data["TotalRows"].ToString();
                    totalDisplayRecords = data["TotalDisplayRows"].ToString();
                }
                sb.Append("{");
                sb.AppendFormat(@"""DT_RowId"": ""{0}""", count++);
                sb.Append(",");
                sb.AppendFormat(@"""DT_RowClass"": ""{0}""", rowClass);
                sb.Append(",");
                sb.AppendFormat(@"""1"": ""{0}""", data["IdAlarma"]);
                sb.Append(",");
                sb.AppendFormat(@"""2"": ""{0}""", data["IdModulo"]);
                sb.Append(",");
                sb.AppendFormat(@"""3"": ""{0}""", data["IdEstacionamiento"]);
                sb.Append(",");
                sb.AppendFormat(@"""4"": ""{0}""", data["TipoError"]);
                sb.Append(",");
                sb.AppendFormat(@"""5"": ""{0}""", data["Parte"]);
                sb.Append(",");
                sb.AppendFormat(@"""6"": ""{0}""", data["Descripcion"]);
                sb.Append(",");
                sb.AppendFormat(@"""7"": ""{0}""", data["NivelError"]);
                sb.Append(",");
                sb.AppendFormat(@"""8"": ""{0}""", data["FechaRegistro"]);
                sb.Append(",");
                sb.AppendFormat(@"""9"": ""{0}""", data["Observacion"]);
                sb.Append(",");
                sb.AppendFormat(@"""10"": ""{0}""", data["FechaAtencion"]);
                sb.Append(",");
                sb.AppendFormat(@"""11"": ""{0}""", data["UsuarioObservacion"]);
                sb.Append(",");
                sb.AppendFormat(@"""12"": ""{0}""", data["FechaSolucion"]);
                sb.Append("},");
            }

            // handles zero records
            if (totalRecords.Length == 0)
            {
                sb.Append("{");
                sb.Append(@"""sEcho"": ");
                sb.AppendFormat(@"""{0}""", sEcho);
                sb.Append(",");
                sb.Append(@"""iTotalRecords"": 0");
                sb.Append(",");
                sb.Append(@"""iTotalDisplayRecords"": 0");
                sb.Append(", ");
                sb.Append(@"""aaData"": [ ");
                sb.Append("]}");
                outputJson = sb.ToString();

                return outputJson;
            }
            outputJson = sb.Remove(sb.Length - 1, 1).ToString();
            sb.Clear();

            sb.Append("{");
            sb.Append(@"""sEcho"": ");
            sb.AppendFormat(@"""{0}""", sEcho);
            sb.Append(",");
            sb.Append(@"""iTotalRecords"": ");
            sb.Append(totalRecords);
            sb.Append(",");
            sb.Append(@"""iTotalDisplayRecords"": ");
            sb.Append(totalDisplayRecords);
            sb.Append(", ");
            sb.Append(@"""aaData"": [ ");
            sb.Append(outputJson);
            sb.Append("]}");
            outputJson = sb.ToString();

            return outputJson;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public IEnumerable GetItemsSedes()
        {

            DataTable dt = new DataTable();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;



            string user = HttpContext.Current.User.Identity.Name.ToString();
            string query = "SELECT Cargo FROM T_Usuarios where Documento='" + user + "'";
            string CARGO = string.Empty;
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
                                CARGO = reader["Cargo"].ToString();
                            }
                        }
                    }
                }
            }
            if (CARGO == "CONTROL INTERNO")
            {


                query = "SELECT IdSede, Nombre, Ciudad, Departamento, Direccion, UsuarioAsigando, TelefonoContacto, Estado FROM T_Sedes";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            RegistrarAuditoria(TipoAccion.select, "t_sedes", query);
                            dt.Load(reader);
                        }
                    }
                }
            }

            var feeds =
                    from myRow in dt.AsEnumerable()
                    select new
                    {
                        IdSede = myRow["IdSede"],
                        Nombre = myRow["Nombre"],
                        Ciudad = myRow["Ciudad"],
                        Departamento = myRow["Departamento"],
                        Direccion = myRow["Direccion"],
                        UsuarioAsigando = myRow["UsuarioAsigando"],
                        TelefonoContacto = myRow["TelefonoContacto"],
                        Estado = myRow["Estado"],
                    };
            return feeds;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public IEnumerable GetItemsEstacionamientos()
        {
            DataTable dt = new DataTable();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            string query = "SELECT IdEstacionamiento, Nombre, Direccion, TelefonoContacto, PersonaContacto,  Email, FechaContratacion, FechaVencimientoContrato, PaqueteContratado, NumeroContrato, EjecutivoCuenta, IdSede, Estado FROM T_Estacionamientos";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        RegistrarAuditoria(TipoAccion.select, "t_estacionamientos", query);
                        dt.Load(reader);
                    }
                }
            }

            var feeds =
                    from myRow in dt.AsEnumerable()
                    select new
                    {
                        IdEstacionamiento = myRow["IdEstacionamiento"],
                        Nombre = myRow["Nombre"],
                        Direccion = myRow["Direccion"],
                        TelefonoContacto = myRow["TelefonoContacto"],
                        PersonaContacto = myRow["PersonaContacto"],
                        Email = myRow["Email"],
                        FechaContratacion = myRow["FechaContratacion"],
                        FechaVencimiento = myRow["FechaVencimientoContrato"],
                        PaqueteContratado = myRow["PaqueteContratado"],
                        NumeroContrato = myRow["NumeroContrato"],
                        EjecutivoCuenta = myRow["EjecutivoCuenta"],
                        IdSede = myRow["IdSede"],
                        Estado = myRow["Estado"],
                    };
            return feeds;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public IEnumerable GetItemsAutorizados()
        {
            DataTable dt = new DataTable();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            string query = "SELECT Documento, IdAutorizacion, e.idEstacionamiento, NombreApellidos, IdTarjeta, NombreEmpresa, Nit, p.Congelar, FechaCreacion, FechaFin, DocumentoUsuarioCreacion, p.Estado, Nombre, p.Telefono, p.Email, p.Placa1, p.Placa2, p.Placa3, p.Placa4, p.Placa5 FROM T_PersonasAutorizadas as p inner join t_estacionamientos as e on p.idestacionamiento=e.idestacionamiento";

            //   string query = "SELECT Documento, IdAutorizacion, e.idEstacionamiento, NombreApellidos, IdTarjeta, FechaCreacion, FechaFin, DocumentoUsuarioCreacion, p.Estado, Nombre, p.Telefono, p.Email, p.Placa1, p.Placa2, p.Placa3, p.Placa4, p.Placa5 FROM T_PersonasAutorizadas as p inner join t_estacionamientos as e on p.idestacionamiento=e.idestacionamiento";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        RegistrarAuditoria(TipoAccion.select, "t_personasautorizadas", query);
                        dt.Load(reader);
                    }
                }
            }

            var feeds =
                    from myRow in dt.AsEnumerable()
                    select new
                    {
                        Documento = myRow["Documento"],
                        IdAutorizacion = myRow["IdAutorizacion"],
                        idEstacionamiento = myRow["idEstacionamiento"],
                        NombreApellidos = myRow["NombreApellidos"],
                        IdTarjeta = myRow["IdTarjeta"],
                        NombreEmpresa = myRow["NombreEmpresa"],
                        Nit = myRow["Nit"],
                        Congelar = myRow["Congelar"],
                        FechaCreacion = myRow["FechaCreacion"].ToString(),
                        FechaFin = myRow["FechaFin"].ToString(),
                        DocumentoUsuarioCreacion = myRow["DocumentoUsuarioCreacion"],
                        Estado = myRow["Estado"],
                        Nombre = myRow["Nombre"],
                        Telefono = myRow["Telefono"],
                        Email = myRow["Email"],
                        Placa1 = myRow["Placa1"],
                        Placa2 = myRow["Placa2"],
                        Placa3 = myRow["Placa3"],
                        Placa4 = myRow["Placa4"],
                        Placa5 = myRow["Placa5"],



                    };
            return feeds;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public IEnumerable GetItemsAutorizaciones()
        {
            DataTable dt = new DataTable();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            string query = "SELECT IdAutorizacion, a.idEstacionamiento, IdReglaAutorizacion, NombreAutorizacion, FechaInicial, FechaFinal, a.Estado, Nombre, a.IdTipo FROM T_Autorizaciones as a inner join t_estacionamientos as e on a.idestacionamiento=e.idestacionamiento";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        RegistrarAuditoria(TipoAccion.select, "t_autorizaciones", query);
                        dt.Load(reader);
                    }
                }
            }

            var feeds =
                    from myRow in dt.AsEnumerable()
                    select new
                    {
                        IdAutorizacion = myRow["IdAutorizacion"],
                        idEstacionamiento = myRow["idEstacionamiento"],
                        IdReglaAutorizacion = myRow["IdReglaAutorizacion"],
                        NombreAutorizacion = myRow["NombreAutorizacion"],
                        FechaInicial = myRow["FechaInicial"].ToString(),
                        FechaFinal = myRow["FechaFinal"].ToString(),
                        Estado = myRow["Estado"],
                        Nombre = myRow["Nombre"],
                        IdTipo = myRow["IdTipo"],
                    };
            return feeds;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public IEnumerable GetItemsTarifas()
        {
            DataTable dt = new DataTable();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            string query = "select t.IdTarifa, t.IdEstacionamiento, t.IdTipoPago, t.IdTipoVehiculo, t.IdAutorizacion, t.Valor, t.TipoCobro, t.Estado , tv.TipoVehiculo, e.Nombre, e.IdSede, ev.IdEvento, c.IdConvenio, ev.Evento, c.Nombre"
                            + " from t_tarifas as t"
                            + " inner join"
                            + " T_TipoVehiculo as tv"
                            + " on tv.IdTipoVehiculo = t.IdTipoVehiculo"
                            + " left join"
                            + " T_Estacionamientos as e"
                            + " on t.idestacionamiento=e.idestacionamiento"
                            + " left join"
                            + " T_convenios as c"
                            + " on c.idconvenio=t.idconvenio"
                            + " left join"
                            + " t_eventos as ev"
                            + " on ev.idevento=t.idevento";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        RegistrarAuditoria(TipoAccion.select, "t_tarifas", query);
                        dt.Load(reader);
                    }
                }
            }

            var feeds =
                    from myRow in dt.AsEnumerable()
                    select new
                    {
                        IdTarifa = myRow["IdTarifa"],
                        IdEstacionamiento = myRow["IdEstacionamiento"],
                        IdTipoPago = myRow["IdTipoPago"],
                        IdTipoVehiculo = myRow["IdTipoVehiculo"],
                        IdAutorizacion = myRow["IdAutorizacion"].ToString(),
                        Valor = myRow["Valor"],
                        TipoCobro = myRow["TipoCobro"],
                        Estado = myRow["Estado"],
                        TipoVehiculo = myRow["TipoVehiculo"],
                        Nombre = myRow["Nombre"].ToString(),
                        IdSede = myRow["IdSede"].ToString(),
                        IdEvento = myRow["IdEvento"].ToString(),
                        IdConvenio = myRow["IdConvenio"].ToString(),
                        Evento = myRow["Evento"].ToString(),
                        NombreConvenio = myRow["Nombre"].ToString()
                    };
            return feeds;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public IEnumerable GetItemsConvenios()
        {
            DataTable dt = new DataTable();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            string query = "select c.idconvenio, c.nombre, c.descripcion, c.idestacionamiento, e.nombre as nombreestacionamiento, c.fechainicial, c.fechafinal, c.minuto, c.estado, e.idsede"
                            + " from t_convenios as c"
                            + " inner join"
                            + " T_Estacionamientos as e"
                            + " on c.idestacionamiento=e.idestacionamiento";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        RegistrarAuditoria(TipoAccion.select, "t_tarifas", query);
                        dt.Load(reader);
                    }
                }
            }

            var feeds =
                    from myRow in dt.AsEnumerable()
                    select new
                    {
                        idconvenio = myRow["idconvenio"],
                        nombre = myRow["nombre"],
                        descripcion = myRow["descripcion"],
                        idestacionamiento = myRow["idestacionamiento"],
                        nombreEstacionamiento = myRow["nombreestacionamiento"].ToString(),
                        fechainicial = myRow["fechainicial"].ToString(),
                        fechafinal = myRow["fechafinal"].ToString(),
                        minuto = myRow["minuto"],
                        estado = myRow["estado"].ToString(),
                        idsede = myRow["idsede"].ToString()
                    };
            return feeds;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public IEnumerable GetItemsEventos()
        {
            DataTable dt = new DataTable();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            string query = "select e.idevento, e.evento, e.idestacionamiento, es.nombre as nombreestacionamiento, e.documentousuario, e.estado, es.idsede, e.horasevento"
                            + " from t_eventos as e"
                            + " inner join"
                            + " T_Estacionamientos as es"
                            + " on e.idestacionamiento=es.idestacionamiento";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        RegistrarAuditoria(TipoAccion.select, "t_eventos", query);
                        dt.Load(reader);
                    }
                }
            }

            var feeds =
                    from myRow in dt.AsEnumerable()
                    select new
                    {
                        idevento = myRow["idevento"],
                        evento = myRow["evento"],
                        idestacionamiento = myRow["idestacionamiento"],
                        nombreestacionamiento = myRow["nombreestacionamiento"],
                        documentousuario = myRow["documentousuario"].ToString(),
                        estado = myRow["estado"].ToString(),
                        idsede = myRow["idsede"].ToString(),
                        horasevento = myRow["horasevento"].ToString()
                    };
            return feeds;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public IEnumerable GetItemsModulos()
        {
            DataTable dt = new DataTable();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            string query = "SELECT IdModulo, c.IdEstacionamiento, IP, Carril, c.IdTipoModulo, Extension, c.Estado, t.TipoModulo, e.Nombre, c.Mac FROM T_Configuracion as c inner join t_TipoModulo as t on t.IdTipoModulo=c.IdTipoModulo inner join T_Estacionamientos as e on e.IdEstacionamiento=c.IdEstacionamiento";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        RegistrarAuditoria(TipoAccion.select, "t_configuracion", query);
                        dt.Load(reader);
                    }
                }
            }

            var feeds =
                    from myRow in dt.AsEnumerable()
                    select new
                    {
                        IdModulo = myRow["IdModulo"],
                        IdEstacionamiento = myRow["IdEstacionamiento"],
                        IP = myRow["IP"],
                        Carril = myRow["Carril"],
                        IdTipoModulo = myRow["IdTipoModulo"],
                        Extension = myRow["Extension"],
                        Estado = myRow["Estado"],
                        TipoModulo = myRow["TipoModulo"],
                        Nombre = myRow["Nombre"],
                        Mac = myRow["mac"]
                    };
            return feeds;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public IEnumerable GetItemsTarjetas()
        {
            DataTable dt = new DataTable();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
           // string query = "SELECT top (500) e.Nombre, e.IdEstacionamiento, IdTarjeta, FechaRegistro, DocumentoUsuarioRegistro, t.Estado FROM T_Tarjetas as t inner join t_Estacionamientos as e on e.IdEstacionamiento=t.IdEstacionamiento order by FechaRegistro desc";
            string query = "SELECT e.Nombre, e.IdEstacionamiento, IdTarjeta, FechaRegistro, DocumentoUsuarioRegistro, t.Estado FROM T_Tarjetas as t inner join t_Estacionamientos as e on e.IdEstacionamiento=t.IdEstacionamiento order by t.Estado";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        RegistrarAuditoria(TipoAccion.select, "t_tarjetas", query);
                        dt.Load(reader);
                    }
                }
            }

            var feeds =
                    from myRow in dt.AsEnumerable()
                    select new
                    {
                        NombreEstacionamiento = myRow["Nombre"],
                        IdEstacionamiento = myRow["IdEstacionamiento"],
                        IdTarjeta = myRow["IdTarjeta"],
                        FechaRegistro = myRow["FechaRegistro"].ToString(),
                        DocumentoUsuarioRegistro = myRow["DocumentoUsuarioRegistro"],
                        Estado = myRow["Estado"],
                    };
            return feeds;
        }


        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public IEnumerable GetItemsPermisosUsuarioEstacionamiento()
        {
            string documentoUsuario = HttpContext.Current.Request.Params["documento"];

            DataTable dt = new DataTable();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = "select t.IdEstacionamiento, Nombre, 'true' as Estado"
                            + " from T_PermisosEstacionamientoUsuarios as eu"
                            + " inner join"
                            + " T_Estacionamientos as t"
                            + " on eu.IdEstacionamiento = t.IdEstacionamiento"
                            + " where eu.DocumentoUsuario = '" + documentoUsuario + "'"
                            + " union"
                            + " select t2.IdEstacionamiento, t2.Nombre, 'false' as Estado"
                            + " from"
                            + " (select t.IdEstacionamiento"
                            + " from T_PermisosEstacionamientoUsuarios as eu"
                            + " inner join T_Estacionamientos as t"
                            + " on eu.IdEstacionamiento = t.IdEstacionamiento"
                            + " where eu.DocumentoUsuario = '" + documentoUsuario + "') as t1"
                            + " right join"
                            + " (select IdEstacionamiento, Nombre"
                            + " from T_Estacionamientos) as t2"
                            + " on t1.IdEstacionamiento=t2.idestacionamiento"
                            + " where t1.IdEstacionamiento is null";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        RegistrarAuditoria(TipoAccion.select, "t_permisosestacionamientousuarios", query);
                        dt.Load(reader);
                    }
                }
            }

            var feeds =
                    from myRow in dt.AsEnumerable()
                    select new
                    {
                        IdEstacionamiento = myRow["IdEstacionamiento"],
                        Nombre = myRow["Nombre"],
                        Estado = myRow["Estado"],
                    };
            return feeds;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public IEnumerable GetItemsPermisosAutorizacionEstacionamiento()
        {
            string idAutorizacion = HttpContext.Current.Request.Params["idAutorizacion"];
            string idEstacionamiento = HttpContext.Current.Request.Params["idEstacionamiento"];

            DataTable dt = new DataTable();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = "select t.IdEstacionamiento, Nombre, 'true' as Estado"
                            + " from T_PermisosEstacionamientoAutorizacion as ea"
                            + " inner join"
                            + " T_Estacionamientos as t"
                            + " on ea.IdEstacionamientoPermiso = t.IdEstacionamiento"
                            + " where ea.IdAutorizacion = '" + idAutorizacion + "' and ea.IdEstacionamiento='" + idEstacionamiento + "'"
                            + " union"
                            + " select t2.IdEstacionamiento, t2.Nombre, 'false' as Estado"
                            + " from"
                            + " (select t.IdEstacionamiento"
                            + " from T_PermisosEstacionamientoAutorizacion as ea"
                            + " inner join T_Estacionamientos as t"
                            + " on ea.IdEstacionamientoPermiso = t.IdEstacionamiento"
                            + " where ea.idAutorizacion = '" + idAutorizacion + "' and ea.IdEstacionamiento='" + idEstacionamiento + "') as t1"
                            + " right join"
                            + " (select IdEstacionamiento, Nombre"
                            + " from T_Estacionamientos) as t2"
                            + " on t1.IdEstacionamiento=t2.idestacionamiento"
                            + " where t1.IdEstacionamiento is null";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        RegistrarAuditoria(TipoAccion.select, "t_permisosestacionamientautorizacion", query);
                        dt.Load(reader);
                    }
                }
            }

            var feeds =
                    from myRow in dt.AsEnumerable()
                    select new
                    {
                        IdEstacionamiento = myRow["IdEstacionamiento"],
                        Nombre = myRow["Nombre"],
                        Estado = myRow["Estado"],
                    };
            return feeds;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public IEnumerable GetItemsPermisosAutorizadoEstacionamiento()
        {
            string documento = HttpContext.Current.Request.Params["documento"];

            DataTable dt = new DataTable();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = "select t.IdEstacionamiento, Nombre, 'true' as Estado"
                            + " from T_PermisosEstacionamientoPersonas as ea"
                            + " inner join"
                            + " T_Estacionamientos as t"
                            + " on ea.IdEstacionamientoPermiso = t.IdEstacionamiento"
                            + " where ea.Documento = '" + documento
                            + "' union"
                            + " select t2.IdEstacionamiento, t2.Nombre, 'false' as Estado"
                            + " from"
                            + " (select t.IdEstacionamiento"
                            + " from T_PermisosEstacionamientoPersonas as ea"
                            + " inner join T_Estacionamientos as t"
                            + " on ea.IdEstacionamientoPermiso = t.IdEstacionamiento"
                            + " where ea.Documento = '" + documento + "') as t1"
                            + " right join"
                            + " (select IdEstacionamiento, Nombre"
                            + " from T_Estacionamientos) as t2"
                            + " on t1.IdEstacionamiento=t2.idestacionamiento"
                            + " where t1.IdEstacionamiento is null";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        RegistrarAuditoria(TipoAccion.select, "T_PermisosEstacionamientoPersonas", query);
                        dt.Load(reader);
                    }
                }
            }

            var feeds =
                    from myRow in dt.AsEnumerable()
                    select new
                    {
                        IdEstacionamiento = myRow["IdEstacionamiento"],
                        Nombre = myRow["Nombre"],
                        Estado = myRow["Estado"],
                    };
            return feeds;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public IEnumerable GetItemsPermisosUsuarioConvenio()
        {
            string documento = HttpContext.Current.Request.Params["documento"];

            DataTable dt = new DataTable();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = "select t.idconvenio, Nombre, 'true' as Estado"
                            + " from T_PermisosConveniosUsuarios as pcu"
                            + " inner join"
                            + " T_Convenios as t"
                            + " on pcu.idconvenio = t.idconvenio"
                            + " where pcu.documentousuario = '" + documento + "'"
                            + " union"
                            + " select t2.idconvenio, t2.Nombre, 'false' as Estado"
                            + " from"
                            + " (select t.idconvenio"
                            + " from T_PermisosConveniosUsuarios as pcu"
                            + " inner join T_Convenios as t"
                            + " on pcu.idconvenio = t.idconvenio"
                            + " where pcu.documentousuario = '" + documento + "') as t1"
                            + " right join"
                            + " (select idconvenio, Nombre"
                            + " from T_Convenios) as t2"
                            + " on t1.idconvenio=t2.idconvenio"
                            + " where t1.idconvenio is null";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        RegistrarAuditoria(TipoAccion.select, "t_permisosconveniosusuarios", query);
                        dt.Load(reader);
                    }
                }
            }

            var feeds =
                    from myRow in dt.AsEnumerable()
                    select new
                    {
                        IdConvenio = myRow["IdConvenio"],
                        Nombre = myRow["Nombre"],
                        Estado = myRow["Estado"],
                    };
            return feeds;
        }

        public static int ToInt(string toParse)
        {
            int result;
            if (int.TryParse(toParse, out result)) return result;

            return result;
        }

        //Update

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string AtenderAlarma()
        {
            string idAlarma = HttpContext.Current.Request.Params["idAlarma"];
            string descripcion = HttpContext.Current.Request.Params["descripcion"];

            string user = HttpContext.Current.User.Identity.Name.ToString();

            string response = string.Empty;
            DataBaseResponse oDataBaseResponse = new DataBaseResponse();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;

            query = "update t_alarmas set fechaatencion = GETDATE(), Observacion = '" + descripcion + "', UsuarioObservacion='" + user + "' where idalarma = " + idAlarma;


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    int respuesta = cmd.ExecuteNonQuery();

                    if (respuesta <= 0)
                    {
                        oDataBaseResponse.Exito = false;
                        oDataBaseResponse.ErrorMessage = "No fue posible actualizar el registro.";
                    }
                    else
                    {
                        RegistrarAuditoria(TipoAccion.update, "t_alarmas", query);
                    }
                }
            }

            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            response = javaScriptSerializer.Serialize(oDataBaseResponse);

            return response;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string ActualizarSede()
        {
            string idSede = HttpContext.Current.Request.Params["idSede"];
            string nombre = HttpContext.Current.Request.Params["nombre"];
            string ciudad = HttpContext.Current.Request.Params["ciudad"];
            string departamento = HttpContext.Current.Request.Params["departamento"];
            string direccion = HttpContext.Current.Request.Params["direccion"];
            string usuario = HttpContext.Current.Request.Params["usuario"];
            string telefono = HttpContext.Current.Request.Params["telefono"];
            string estado = HttpContext.Current.Request.Params["estado"];

            string response = string.Empty;
            DataBaseResponse oDataBaseResponse = new DataBaseResponse();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;

            string user = HttpContext.Current.User.Identity.Name.ToString();
            query = "SELECT Cargo FROM T_Usuarios where Documento='" + user + "'";

            string CARGO = string.Empty;
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
                                CARGO = reader["Cargo"].ToString();
                            }
                        }
                    }
                }
            }


            if (CARGO == "CONTROL INTERNO")
            {

                query = "update t_sedes set nombre='" + nombre + "', ciudad='" + ciudad + "', departamento='" + telefono + "', direccion='" + direccion + "', UsuarioAsigando='" + usuario + "', telefonocontacto='" + telefono + "', estado='" + estado + "' , Sincronizacion='False' where idsede=" + idSede;


                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        int respuesta = cmd.ExecuteNonQuery();

                        if (respuesta <= 0)
                        {
                            oDataBaseResponse.Exito = false;
                            oDataBaseResponse.ErrorMessage = "No fue posible actualizar el registro.";
                        }
                        else
                        {
                            RegistrarAuditoria(TipoAccion.update, "t_sedes", query);
                        }
                    }
                }
            }
            else
            {
                oDataBaseResponse.Exito = false;
                oDataBaseResponse.ErrorMessage = "No tiene permiso para realizar esta operacion.";
            }

            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            response = javaScriptSerializer.Serialize(oDataBaseResponse);

            return response;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string ActualizarEstacionamiento()
        {
            string idEstacionamiento = HttpContext.Current.Request.Params["idEstacionamiento"];
            string nombre = HttpContext.Current.Request.Params["nombre"];
            string direccion = HttpContext.Current.Request.Params["direccion"];
            string telefono = HttpContext.Current.Request.Params["telefono"];
            string persona = HttpContext.Current.Request.Params["persona"];
            string ciudad = HttpContext.Current.Request.Params["ciudad"];
            string departamento = HttpContext.Current.Request.Params["departamento"];
            string email = HttpContext.Current.Request.Params["email"];
            string fechac = HttpContext.Current.Request.Params["fechac"];
            string fechav = HttpContext.Current.Request.Params["fechav"];
            string paquete = HttpContext.Current.Request.Params["paquete"];
            string numero = HttpContext.Current.Request.Params["numero"];
            string ejecutivo = HttpContext.Current.Request.Params["ejecutivo"];
            string idSede = HttpContext.Current.Request.Params["idSede"];
            string estado = HttpContext.Current.Request.Params["estado"];

            string response = string.Empty;
            DataBaseResponse oDataBaseResponse = new DataBaseResponse();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;

            query = "update t_estacionamientos set nombre='" + nombre
                    + "', direccion='" + direccion
                    + "', telefonocontacto='" + telefono
                    + "', personacontacto='" + persona
                    + "', email='" + email
                    + "', fechacontratacion='" + fechac
                    + "', fechavencimientocontrato='" + fechav
                    + "', paquetecontratado='" + paquete
                    + "', numerocontrato='" + numero
                    + "', ejecutivocuenta='" + ejecutivo
                    + "', idsede='" + idSede
                    + "', estado='" + estado
                    + "', Sincronizacion='False'"
                    + " where idestacionamiento=" + idEstacionamiento;


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    int respuesta = cmd.ExecuteNonQuery();

                    if (respuesta <= 0)
                    {
                        oDataBaseResponse.Exito = false;
                        oDataBaseResponse.ErrorMessage = "No fue posible actualizar el registro.";
                    }
                    else
                    {
                        RegistrarAuditoria(TipoAccion.update, "t_estacionamientos", query);
                    }
                }
            }

            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            response = javaScriptSerializer.Serialize(oDataBaseResponse);

            return response;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string ActualizarModulo()
        {
            string idEstacionamiento = HttpContext.Current.Request.Params["idEstacionamiento"];
            string idModulo = HttpContext.Current.Request.Params["idModulo"];
            string ip = HttpContext.Current.Request.Params["ip"];
            string carril = HttpContext.Current.Request.Params["carril"];
            string idTipoModulo = HttpContext.Current.Request.Params["idTipoModulo"];
            string extension = HttpContext.Current.Request.Params["extension"];
            string estado = HttpContext.Current.Request.Params["estado"];
            string mac = HttpContext.Current.Request.Params["mac"];

            string response = string.Empty;
            DataBaseResponse oDataBaseResponse = new DataBaseResponse();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;

            query = "update t_configuracion set idEstacionamiento='" + idEstacionamiento
                    + "', IP='" + ip
                    + "', Carril='" + carril
                    + "', IdTipoModulo='" + idTipoModulo
                    + "', extension='" + extension
                    + "', estado='" + estado
                    + "', mac='" + mac
                    + "', Sincronizacion='False'"
                    + " where idModulo='" + idModulo + "'";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    int respuesta = cmd.ExecuteNonQuery();

                    if (respuesta <= 0)
                    {
                        oDataBaseResponse.Exito = false;
                        oDataBaseResponse.ErrorMessage = "No fue posible actualizar el registro.";
                    }
                    else
                    {
                        RegistrarAuditoria(TipoAccion.update, "t_configuracion", query);
                    }
                }
            }

            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            response = javaScriptSerializer.Serialize(oDataBaseResponse);

            return response;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string ActualizarTarjeta()
        {
            string idEstacionamiento = HttpContext.Current.Request.Params["idEstacionamiento"];
            string idTarjeta = HttpContext.Current.Request.Params["idTarjeta"];
            string estado = HttpContext.Current.Request.Params["estado"];

            string response = string.Empty;
            DataBaseResponse oDataBaseResponse = new DataBaseResponse();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;

            query = "update t_tarjetas set idEstacionamiento='" + idEstacionamiento
                    + "', estado='" + estado
                    + "' where idTarjeta='" + idTarjeta + "'";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    int respuesta = cmd.ExecuteNonQuery();

                    if (respuesta <= 0)
                    {
                        oDataBaseResponse.Exito = false;
                        oDataBaseResponse.ErrorMessage = "No fue posible actualizar el registro.";
                    }
                    else
                    {
                        RegistrarAuditoria(TipoAccion.update, "t_tarjetas", query);
                    }
                }
            }

            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            response = javaScriptSerializer.Serialize(oDataBaseResponse);

            return response;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string ActualizarFactura()
        {
            string idFacturacion = HttpContext.Current.Request.Params["idFacturacion"];
            string idModulo = HttpContext.Current.Request.Params["idModulo"];
            string idEstacionamiento = HttpContext.Current.Request.Params["idEstacionamiento"];
            string prefijo = HttpContext.Current.Request.Params["prefijo"];
            string inicial = HttpContext.Current.Request.Params["inicial"];
            string final = HttpContext.Current.Request.Params["final"];
            string numero = HttpContext.Current.Request.Params["numero"];
            string fecha = HttpContext.Current.Request.Params["fecha"];
            string estado = HttpContext.Current.Request.Params["estado"];
            string fin = HttpContext.Current.Request.Params["fin"];

            string response = string.Empty;
            DataBaseResponse oDataBaseResponse = new DataBaseResponse();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;

            query = "update t_facturacion set idmodulo='" + idModulo
                    + "', idestacionamiento='" + idEstacionamiento
                    + "', prefijo='" + prefijo
                    + "', facturainicial='" + inicial
                    + "', facturafinal='" + final
                    + "', numeroresolucion='" + numero
                    + "', fecharesolucion='" + fecha
                    + "', estado='" + estado
                    + "', fechafinresolucion='" + fin
                    + "', Sincronizacion='False'"
                    + " where idfacturacion='" + idFacturacion + "'";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    int respuesta = cmd.ExecuteNonQuery();

                    if (respuesta <= 0)
                    {
                        oDataBaseResponse.Exito = false;
                        oDataBaseResponse.ErrorMessage = "No fue posible actualizar el registro.";
                    }
                    else
                    {
                        RegistrarAuditoria(TipoAccion.update, "t_facturacion", query);
                    }
                }
            }

            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            response = javaScriptSerializer.Serialize(oDataBaseResponse);

            return response;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string ActualizarTarifa()
        {
            string idTarifa = HttpContext.Current.Request.Params["idTarifa"];
            string idEstacionamiento = HttpContext.Current.Request.Params["idEstacionamiento"];
            string idTipoPago = HttpContext.Current.Request.Params["idTipoPago"];
            string idTipoVehiculo = HttpContext.Current.Request.Params["idTipoVehiculo"];
            string idAutorizacion = HttpContext.Current.Request.Params["idAutorizacion"];
            string valor = HttpContext.Current.Request.Params["valor"];
            string tipoCobro = HttpContext.Current.Request.Params["tipoCobro"];
            string estado = HttpContext.Current.Request.Params["estado"];
            string idConvenio = HttpContext.Current.Request.Params["idConvenio"];
            string idEvento = HttpContext.Current.Request.Params["idEvento"];


            string response = string.Empty;
            DataBaseResponse oDataBaseResponse = new DataBaseResponse();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;

            query = "update t_tarifas set idEstacionamiento='" + idEstacionamiento
                    + "', idTipoPago='" + idTipoPago
                    + "', idTipoVehiculo='" + idTipoVehiculo
                    + "', idAutorizacion=" + (Convert.ToInt32(idAutorizacion) > 0 ? idAutorizacion : "NULL")
                    + ", idConvenio=" + (Convert.ToInt32(idConvenio) > 0 ? idConvenio : "NULL")
                    + ", idEvento=" + (Convert.ToInt32(idEvento) > 0 ? idEvento : "NULL")
                    + ", valor='" + valor
                    + "', tipoCobro='" + tipoCobro
                    + "', estado='" + estado
                    + "', Sincronizacion='False'"
                    + " where idTarifa='" + idTarifa + "'";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    int respuesta = cmd.ExecuteNonQuery();

                    if (respuesta <= 0)
                    {
                        oDataBaseResponse.Exito = false;
                        oDataBaseResponse.ErrorMessage = "No fue posible actualizar el registro.";
                    }
                    else
                    {
                        RegistrarAuditoria(TipoAccion.update, "t_tarifas", query);
                    }
                }
            }

            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            response = javaScriptSerializer.Serialize(oDataBaseResponse);

            return response;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string ActualizarConvenio()
        {
            string idConvenio = HttpContext.Current.Request.Params["idConvenio"];
            string idEstacionamiento = HttpContext.Current.Request.Params["idEstacionamiento"];
            string nombre = HttpContext.Current.Request.Params["nombre"];
            string descripcion = HttpContext.Current.Request.Params["descripcion"];
            string fechaInicial = HttpContext.Current.Request.Params["fechaInicial"];
            string fechaFinal = HttpContext.Current.Request.Params["fechaFinal"];
            string minuto = HttpContext.Current.Request.Params["minuto"];
            string estado = HttpContext.Current.Request.Params["estado"];


            string response = string.Empty;
            DataBaseResponse oDataBaseResponse = new DataBaseResponse();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;

            //query = "update t_convenios set idEstacionamiento='" + idEstacionamiento
            //        + "', nombre='" + nombre
            //        + "', descripcion='" + descripcion
            //        + "', fechainicial=convert(datetime,'" + fechaInicial + "',103)"
            //        + ", fechafinal=convert(datetime,'" + fechaFinal + "',103)"
            //        + ", minuto='" + minuto
            //        + "', estado='" + estado
            //        +"' , Sincronizacion='False'"
            //        + " where idconvenio='" + idConvenio + "'";

            query = "update t_convenios set idEstacionamiento='" + idEstacionamiento
                  + "', nombre='" + nombre
                  + "', descripcion='" + descripcion
                  + "', fechainicial=(select convert(varchar,'" + fechaInicial + "', 108))"
                  + ", fechafinal=(select convert(varchar,'" + fechaFinal + "', 108))"
                  + ", minuto='" + minuto
                  + "', estado='" + estado
                  + "' , Sincronizacion='False'"
                  + " where idconvenio='" + idConvenio + "'";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    int respuesta = cmd.ExecuteNonQuery();

                    if (respuesta <= 0)
                    {
                        oDataBaseResponse.Exito = false;
                        oDataBaseResponse.ErrorMessage = "No fue posible actualizar el registro.";
                    }
                    else
                    {
                        RegistrarAuditoria(TipoAccion.update, "t_convenios", query);
                    }
                }
            }

            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            response = javaScriptSerializer.Serialize(oDataBaseResponse);

            return response;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string ActualizarEvento()
        {
            string idEvento = HttpContext.Current.Request.Params["idEvento"];
            string idEstacionamiento = HttpContext.Current.Request.Params["idEstacionamiento"];
            string evento = HttpContext.Current.Request.Params["evento"];
            string horas = HttpContext.Current.Request.Params["horas"];
            string estado = HttpContext.Current.Request.Params["estado"];


            string response = string.Empty;
            DataBaseResponse oDataBaseResponse = new DataBaseResponse();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;

            query = "update t_eventos set idEstacionamiento='" + idEstacionamiento
                    + "', evento='" + evento
                    + "', horasevento='" + horas
                    + "', estado='" + estado
                    + "'"
                    + " where idevento='" + idEvento + "'";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    int respuesta = cmd.ExecuteNonQuery();

                    if (respuesta <= 0)
                    {
                        oDataBaseResponse.Exito = false;
                        oDataBaseResponse.ErrorMessage = "No fue posible actualizar el registro.";
                    }
                    else
                    {
                        RegistrarAuditoria(TipoAccion.update, "t_convenios", query);
                    }
                }
            }

            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            response = javaScriptSerializer.Serialize(oDataBaseResponse);

            return response;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string ActualizarParametro()
        {
            string idEstacionamiento = HttpContext.Current.Request.Params["idEstacionamiento"];
            string codigo = HttpContext.Current.Request.Params["codigo"];
            string valor = HttpContext.Current.Request.Params["valor"];
            string descripcion = HttpContext.Current.Request.Params["descripcion"];
            string estado = HttpContext.Current.Request.Params["estado"];

            string response = string.Empty;
            DataBaseResponse oDataBaseResponse = new DataBaseResponse();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;

            query = "update t_parametros set Valor='" + valor
                    + "', Descripcion='" + descripcion
                    + "', Estado='" + estado
                    + "' , Sincronizacion='False'"
                    + " where idEstacionamiento='" + idEstacionamiento + "' and Codigo = '" + codigo + "'";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    int respuesta = cmd.ExecuteNonQuery();

                    if (respuesta <= 0)
                    {
                        oDataBaseResponse.Exito = false;
                        oDataBaseResponse.ErrorMessage = "No fue posible actualizar el registro.";
                    }
                    else
                    {
                        RegistrarAuditoria(TipoAccion.update, "t_parametros", query);
                    }
                }
            }

            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            response = javaScriptSerializer.Serialize(oDataBaseResponse);

            return response;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string ActualizarUsuario()
        {
            string documento = HttpContext.Current.Request.Params["documento"];
            string nombres = HttpContext.Current.Request.Params["nombres"];
            string apellidos = HttpContext.Current.Request.Params["apellidos"];
            string usuario = HttpContext.Current.Request.Params["usuario"];
            string cargo = HttpContext.Current.Request.Params["cargo"];
            string estado = HttpContext.Current.Request.Params["estado"];

            string response = string.Empty;
            DataBaseResponse oDataBaseResponse = new DataBaseResponse();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;

            query = "update t_usuarios set nombres='" + nombres
                    + "', apellidos='" + apellidos
                    + "', usuario='" + usuario
                    + "', cargo='" + cargo
                    + "', estado='" + estado
                    + "', Sincronizacion='False'"
                    + " where documento = '" + documento + "'";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    int respuesta = cmd.ExecuteNonQuery();

                    if (respuesta <= 0)
                    {
                        oDataBaseResponse.Exito = false;
                        oDataBaseResponse.ErrorMessage = "No fue posible actualizar el registro.";
                    }
                    else
                    {
                        RegistrarAuditoria(TipoAccion.update, "t_usuarios", query);
                    }
                }
            }

            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            response = javaScriptSerializer.Serialize(oDataBaseResponse);

            return response;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string ActualizarAutorizado()
        {
            string documento = HttpContext.Current.Request.Params["documento"].Trim();
            string nombre = HttpContext.Current.Request.Params["nombre"];
            string estado = HttpContext.Current.Request.Params["estado"];
            string idAutorizacion = HttpContext.Current.Request.Params["idAutorizacion"];
            string idEStacionamiento = HttpContext.Current.Request.Params["idEStacionamiento"];
            string NombreEmpresa = HttpContext.Current.Request.Params["NombreEmpresa"];
            string Nit = HttpContext.Current.Request.Params["Nit"];
            string telefono = HttpContext.Current.Request.Params["telefono"];
            string email = HttpContext.Current.Request.Params["email"];
            string placa1 = HttpContext.Current.Request.Params["placa1"].Trim();
            string placa2 = HttpContext.Current.Request.Params["placa2"].Trim();
            string placa3 = HttpContext.Current.Request.Params["placa3"].Trim();
            string placa4 = HttpContext.Current.Request.Params["placa4"].Trim();
            string placa5 = HttpContext.Current.Request.Params["placa5"].Trim();
            string valorBolsa = HttpContext.Current.Request.Params["valorBolsa"].Trim();
            //    string FechaFin = HttpContext.Current.Request.Params["FechaFin"];
            string Congelar = HttpContext.Current.Request.Params["Congelar"].Trim();
          //  string Fechai = HttpContext.Current.Request.Params["Fecha"].Trim();

            int cong = 0;

            if (Congelar != string.Empty)
            {
                cong = Convert.ToInt32(Congelar);
            }
            string response = string.Empty;
            DataBaseResponse oDataBaseResponse = new DataBaseResponse();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;

            string user = HttpContext.Current.User.Identity.Name.ToString();

            query = "SELECT top (1) Cargo FROM T_Usuarios where Documento='" + user + "'";

            string CARGO = string.Empty;

            string FechaInicio = string.Empty;
            string FechaFin = string.Empty;
            

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
                                CARGO = reader["Cargo"].ToString();
                            }
                        }
                    }
                }
            }

            if (cong >= 0)
            {
                query = "SELECT top (1) FechaInicio,FechaFin FROM T_PersonasAutorizadas where Documento='" + documento + "'";

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
                                    FechaInicio = reader["FechaInicio"].ToString();
                                    FechaFin = reader["FechaFin"].ToString();
                                }
                            }
                        }
                    }
                }
            }


            if ((CARGO == "CONTROL INTERNO" || CARGO == "COORDINADORA DE OPERACIONES") && cong >= 0)
            {

                if (FechaInicio != string.Empty && FechaFin != string.Empty)
                {
                    DateTime fecha1 = Convert.ToDateTime(FechaInicio);
                    DateTime fecha2 = Convert.ToDateTime(FechaFin);
               //     DateTime fecha3 = Convert.ToDateTime(Fechai);

                    int AÑO_ = fecha1.Year;
                    int MES_ = fecha1.Month;
                    int DIA_ = fecha1.Day;
                    int HORA_ = fecha1.Hour;
                    int MINUTO_ = fecha1.Minute;
                    int SEG_ = fecha1.Second;

                    int AÑO__ = fecha2.Year;
                    int MES__ = fecha2.Month;
                    int DIA__ = fecha2.Day;
                    int HORA__ = fecha2.Hour;
                    int MINUTO__ = fecha2.Minute;
                    int SEG__ = fecha2.Second;

                    //int AÑO___ = fecha3.Year;
                    //int MES___ = fecha3.Month;
                    //int DIA___ = fecha3.Day;
                    //int HORA___ = fecha3.Hour;
                    //int MINUTO___ = fecha3.Minute;
                    //int SEG___ = fecha3.Second;

                    //string fecha_inicio = AÑO_.ToString() + "-" + MES_.ToString() + "-" + DIA_.ToString() + " " + HORA_.ToString() + ":" + MINUTO_.ToString() + ":" + SEG_.ToString();
                    //string fecha_fin = AÑO__.ToString() + "-" + MES__.ToString() + "-" + DIA__.ToString() + " " + HORA__.ToString() + ":" + MINUTO__.ToString() + ":" + SEG__.ToString();

                    string fecha_inicio = AÑO_.ToString() + "-" + DIA_.ToString() + "-" + MES_.ToString() + " " + HORA_.ToString() + ":" + MINUTO_.ToString() + ":" + SEG_.ToString();
                    string fecha_fin = AÑO__.ToString() + "-" + DIA__.ToString() + "-" + MES__.ToString() + " " + HORA__.ToString() + ":" + MINUTO__.ToString() + ":" + SEG__.ToString();

               //     string fecha_inicio1 = AÑO___.ToString() + "-" + DIA___.ToString() + "-" + MES___.ToString() + " " + HORA___.ToString() + ":" + MINUTO___.ToString() + ":" + SEG___.ToString();

                    query = "update T_PersonasAutorizadas set IdAutorizacion='" + idAutorizacion
                          + "', NombreApellidos='" + nombre
                          + "', Email='" + email
                          + "', Telefono='" + telefono
                          + "', Estado='" + estado
                          + "', Placa1='" + placa1
                          + "', Placa2='" + placa2
                          + "', Placa3='" + placa3
                          + "', Placa4='" + placa4
                          + "', Placa5='" + placa5
                          + "', FechaInicio=(" + "SELECT DATEADD(DAY," + cong + ",'" + fecha_inicio + "'))"
                          + ", FechaFin=(" + "SELECT DATEADD(DAY," + cong + ",'" + fecha_fin + "'))"
                          + ",NombreEmpresa='" + NombreEmpresa
                          + "', Nit='" + Nit
                          + "', Congelar=0"
                          + ", ValorBolsa='" + valorBolsa
                          + "', Sincronizacion='False'"
                          + " where documento = '" + documento + "' and idEStacionamiento='" + idEStacionamiento + "'";

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand(query, connection))
                        {
                            connection.Open();
                            int respuesta = cmd.ExecuteNonQuery();

                            if (respuesta <= 0)
                            {
                                oDataBaseResponse.Exito = false;
                                oDataBaseResponse.ErrorMessage = "No fue posible actualizar el registro.";
                            }
                            else
                            {
                                RegistrarAuditoria(TipoAccion.update, "t_personasautorizadas", query);
                            }
                        }
                    }
                }
                else
                {
                    query = "update T_PersonasAutorizadas set IdAutorizacion='" + idAutorizacion
                             + "', NombreApellidos='" + nombre
                             + "', Email='" + email
                             + "', Telefono='" + telefono
                             + "', Estado='" + estado
                             + "', Placa1='" + placa1
                             + "', Placa2='" + placa2
                             + "', Placa3='" + placa3
                             + "', Placa4='" + placa4
                             + "', Placa5='" + placa5
                             + "',NombreEmpresa='" + NombreEmpresa
                             + "', Nit='" + Nit
                             + "', Congelar=0"
                             + ", ValorBolsa='" + valorBolsa
                             + "', Sincronizacion='False'"
                             + " where documento = '" + documento + "' and idEStacionamiento='" + idEStacionamiento + "'";

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand(query, connection))
                        {
                            connection.Open();
                            int respuesta = cmd.ExecuteNonQuery();

                            if (respuesta <= 0)
                            {
                                oDataBaseResponse.Exito = false;
                                oDataBaseResponse.ErrorMessage = "No fue posible actualizar el registro.";
                            }
                            else
                            {
                                RegistrarAuditoria(TipoAccion.update, "t_personasautorizadas", query);
                            }
                        }
                    }
                }


            }
            else if ((CARGO == "CONTROL INTERNO" || CARGO == "COORDINADORA DE OPERACIONES") && Congelar == string.Empty)
            {
                query = "update T_PersonasAutorizadas set IdAutorizacion='" + idAutorizacion
                       + "', NombreApellidos='" + nombre
                       + "', Email='" + email
                       + "', Telefono='" + telefono
                       + "', Estado='" + estado
                       + "', Placa1='" + placa1
                       + "', Placa2='" + placa2
                       + "', Placa3='" + placa3
                       + "', Placa4='" + placa4
                       + "', Placa5='" + placa5
                       + "', NombreEmpresa='" + NombreEmpresa
                       + "', Nit='" + Nit
                       + "', Congelar='" + Congelar
                       + "', ValorBolsa='" + valorBolsa
                       + "', Sincronizacion='False'"
                       + " where documento = '" + documento + "' and idEStacionamiento='" + idEStacionamiento + "'";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        int respuesta = cmd.ExecuteNonQuery();

                        if (respuesta <= 0)
                        {
                            oDataBaseResponse.Exito = false;
                            oDataBaseResponse.ErrorMessage = "No fue posible actualizar el registro.";
                        }
                        else
                        {
                            RegistrarAuditoria(TipoAccion.update, "t_personasautorizadas", query);
                        }
                    }
                }
            }
            else if (CARGO == "VIGILANTE" || CARGO == "COORDINADOR" || CARGO == "SUPERVISOR" || CARGO == "CAJERO")
            {
                query = "update T_PersonasAutorizadas set IdAutorizacion='" + idAutorizacion
                       + "', NombreApellidos='" + nombre
                       + "', Email='" + email
                       + "', Telefono='" + telefono
                       + "', Estado='" + estado
                       + "', Placa1='" + placa1
                       + "', Placa2='" + placa2
                       + "', Placa3='" + placa3
                       + "', Placa4='" + placa4
                       + "', Placa5='" + placa5
                       + "', NombreEmpresa='" + NombreEmpresa
                       + "', Nit='" + Nit
                       + "', Congelar=0"
                       + ", ValorBolsa='" + valorBolsa
                       + "', Sincronizacion='False'"
                       + " where documento = '" + documento + "' and idEStacionamiento='" + idEStacionamiento + "'";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        int respuesta = cmd.ExecuteNonQuery();

                        if (respuesta <= 0)
                        {
                            oDataBaseResponse.Exito = false;
                            oDataBaseResponse.ErrorMessage = "No fue posible actualizar el registro.";
                        }
                        else
                        {
                            RegistrarAuditoria(TipoAccion.update, "t_personasautorizadas", query);
                        }
                    }
                }

            }
            else
            {
                oDataBaseResponse.Exito = false;
                oDataBaseResponse.ErrorMessage = "No tiene permiso para realizar esta operacion.";
            }

            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            response = javaScriptSerializer.Serialize(oDataBaseResponse);

            return response;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string UpdatePermisosUsuarioEstacionamiento()
        {
            string documento = HttpContext.Current.Request.Params["documento"];
            string permisos = HttpContext.Current.Request.Params["permisos"];
            string[] totalpermisos = permisos.Split(',');

            string response = string.Empty;
            DataBaseResponse oDataBaseResponse = new DataBaseResponse();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;

            query = "delete T_PermisosEstacionamientoUsuarios where documentousuario='" + documento + "'";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    int resultDelete = cmd.ExecuteNonQuery();
                    if (resultDelete > 0)
                    {
                        RegistrarAuditoria(TipoAccion.delete, "T_permisosestacionamientousuarios", query);
                    }
                }
            }

            if (totalpermisos.Length > 0)
            {
                using (SqlConnection connection2 = new SqlConnection(connectionString))
                {
                    connection2.Open();
                    foreach (string item in totalpermisos)
                    {
                        if (item != "null")
                        {
                            query = "insert into T_PermisosEstacionamientoUsuarios values ('" + documento + "','" + item + "',0)";

                            using (SqlCommand cmd2 = new SqlCommand(query, connection2))
                            {
                                int result2 = cmd2.ExecuteNonQuery();

                                if (result2 <= 0)
                                {
                                    oDataBaseResponse.Exito = false;
                                    oDataBaseResponse.ErrorMessage = "No fue posible actualizar el registro.";
                                }
                                else
                                {
                                    RegistrarAuditoria(TipoAccion.insert, "T_permisosestacionamientousuarios", query);
                                }
                            }
                        }
                    }
                }
            }

            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            response = javaScriptSerializer.Serialize(oDataBaseResponse);

            return response;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string UpdatePermisosUsuarioConvenios()
        {
            string documento = HttpContext.Current.Request.Params["documento"];
            string permisos = HttpContext.Current.Request.Params["permisos"];
            string[] totalpermisos = permisos.Split(',');

            string response = string.Empty;
            DataBaseResponse oDataBaseResponse = new DataBaseResponse();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;

            query = "delete T_PermisosConveniosUsuarios where documentousuario='" + documento + "'";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    int resultDelete = cmd.ExecuteNonQuery();
                    if (resultDelete > 0)
                    {
                        RegistrarAuditoria(TipoAccion.delete, "T_permisosconveniosusuarios", query);
                    }
                }
            }

            if (totalpermisos.Length > 0)
            {
                using (SqlConnection connection2 = new SqlConnection(connectionString))
                {
                    connection2.Open();
                    foreach (string item in totalpermisos)
                    {
                        if (item != "null")
                        {
                            query = "insert into T_PermisosConveniosUsuarios values ('" + documento + "','" + item + "','False')";

                            using (SqlCommand cmd2 = new SqlCommand(query, connection2))
                            {
                                int result2 = cmd2.ExecuteNonQuery();

                                if (result2 <= 0)
                                {
                                    oDataBaseResponse.Exito = false;
                                    oDataBaseResponse.ErrorMessage = "No fue posible actualizar el registro.";
                                }
                                else
                                {
                                    RegistrarAuditoria(TipoAccion.insert, "T_permisosconveniosusuarios", query);
                                }
                            }
                        }
                    }
                }
            }

            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            response = javaScriptSerializer.Serialize(oDataBaseResponse);

            return response;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string UpdatePermisosAutorizacionEstacionamiento()
        {
            string idAutorizacion = HttpContext.Current.Request.Params["idAutorizacion"];
            string idEstacionamiento = HttpContext.Current.Request.Params["idEstacionamiento"];
            string permisos = HttpContext.Current.Request.Params["permisos"];
            string[] totalpermisos = permisos.Split(',');

            string response = string.Empty;
            DataBaseResponse oDataBaseResponse = new DataBaseResponse();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;

            query = "delete T_PermisosEstacionamientoAutorizacion where IdAutorizacion='" + idAutorizacion + "' and IdEstacionamiento='" + idEstacionamiento + "'";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    int resultDelete = cmd.ExecuteNonQuery();
                    if (resultDelete > 0)
                    {
                        RegistrarAuditoria(TipoAccion.delete, "T_permisosestacionamientoautorizacion", query);
                    }
                }
            }

            if (totalpermisos.Length > 0)
            {
                using (SqlConnection connection2 = new SqlConnection(connectionString))
                {
                    connection2.Open();
                    foreach (string item in totalpermisos)
                    {
                        if (item != "null")
                        {
                            query = "insert into T_PermisosEstacionamientoAutorizacion values ('" + idAutorizacion + "','" + idEstacionamiento + "','" + item + "',0)";

                            using (SqlCommand cmd2 = new SqlCommand(query, connection2))
                            {
                                int result2 = cmd2.ExecuteNonQuery();

                                if (result2 <= 0)
                                {
                                    oDataBaseResponse.Exito = false;
                                    oDataBaseResponse.ErrorMessage = "No fue posible actualizar el registro.";
                                }
                                else
                                {
                                    RegistrarAuditoria(TipoAccion.insert, "T_permisosestacionamientoautorizacion", query);
                                }
                            }
                        }
                    }
                }
            }

            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            response = javaScriptSerializer.Serialize(oDataBaseResponse);

            return response;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string UpdatePermisosAutorizadoEstacionamiento()
        {
            string documento = HttpContext.Current.Request.Params["documento"];
            string permisos = HttpContext.Current.Request.Params["permisos"];
            string[] totalpermisos = permisos.Split(',');

            string response = string.Empty;
            DataBaseResponse oDataBaseResponse = new DataBaseResponse();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;

            query = "delete T_PermisosEstacionamientoPersonas where Documento='" + documento + "'";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    int resultDelete = cmd.ExecuteNonQuery();
                    if (resultDelete > 0)
                    {
                        RegistrarAuditoria(TipoAccion.delete, "T_PermisosEstacionamientoPersonas", query);
                    }
                }
            }

            if (totalpermisos.Length > 0)
            {
                using (SqlConnection connection2 = new SqlConnection(connectionString))
                {
                    connection2.Open();
                    foreach (string item in totalpermisos)
                    {
                        if (item != "null")
                        {
                            query = "insert into T_PermisosEstacionamientoPersonas values ('" + documento + "','" + item + "',0)";

                            using (SqlCommand cmd2 = new SqlCommand(query, connection2))
                            {
                                int result2 = cmd2.ExecuteNonQuery();

                                if (result2 <= 0)
                                {
                                    oDataBaseResponse.Exito = false;
                                    oDataBaseResponse.ErrorMessage = "No fue posible actualizar el registro.";
                                }
                                else
                                {
                                    RegistrarAuditoria(TipoAccion.insert, "T_PermisosEstacionamientoPersonas", query);
                                }
                            }
                        }
                    }
                }
            }

            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            response = javaScriptSerializer.Serialize(oDataBaseResponse);

            return response;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public string UpdatePermisos()
        {
            string documento = HttpContext.Current.Request.Params["documento"];
            string usuario = HttpContext.Current.Request.Params["usuario"];
            string permisos = HttpContext.Current.Request.Params["permisos"];
            string[] totalpermisos = permisos.Split(',');

            string response = string.Empty;
            DataBaseResponse oDataBaseResponse = new DataBaseResponse();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;

            query = "delete t_permisos where DocumentoUsuario='" + documento + "'";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    int resultDelete = cmd.ExecuteNonQuery();
                    if (resultDelete > 0)
                    {
                        RegistrarAuditoria(TipoAccion.delete, "t_permisos", query);
                    }
                }
            }


            using (SqlConnection connection2 = new SqlConnection(connectionString))
            {
                connection2.Open();
                foreach (string item in totalpermisos)
                {
                    query = "insert into t_permisos values ('" + documento + "','" + item + "')";

                    using (SqlCommand cmd2 = new SqlCommand(query, connection2))
                    {
                        int result2 = cmd2.ExecuteNonQuery();

                        if (result2 <= 0)
                        {
                            oDataBaseResponse.Exito = false;
                            oDataBaseResponse.ErrorMessage = "No fue posible actualizar el registro.";
                        }
                        else
                        {
                            RegistrarAuditoria(TipoAccion.insert, "t_permisos", query);
                        }
                    }
                }
            }

            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            response = javaScriptSerializer.Serialize(oDataBaseResponse);

            return response;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
        public string UpdateClaveUsuario()
        {

            string b = HttpContext.Current.Request.Params["claveVieja"];
            string c = HttpContext.Current.Request.Params["claveNueva"];

            string response = string.Empty;
            DataBaseResponse oDataBaseResponse = new DataBaseResponse();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string user = HttpContext.Current.User.Identity.Name.ToString();

            string query = string.Empty;

            query = "select documento, contraseña from t_usuarios where documento='" + user + "'";

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
                            RegistrarAuditoria(TipoAccion.select, "t_usuarios", query);
                            // Read advances to the next row.
                            while (reader.Read())
                            {
                                documento = reader["documento"].ToString();
                                clave = reader["contraseña"].ToString();
                            }
                        }
                    }
                }
            }

            if (documento != string.Empty)
            {
                if (Decrypt(clave) == b)
                {
                    query = "update t_usuarios set contraseña= '" + Encrypt(c) + "' where documento='" + user + "'";

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand(query, connection))
                        {
                            connection.Open();
                            int result = cmd.ExecuteNonQuery();
                            if (result <= 0)
                            {
                                oDataBaseResponse.Exito = false;
                                oDataBaseResponse.ErrorMessage = "No fue posible actualizar el registro.";
                            }
                            else
                            {
                                RegistrarAuditoria(TipoAccion.update, "t_usuarios", query);
                            }

                        }
                    }
                }
                else
                {
                    oDataBaseResponse.Exito = false;
                    oDataBaseResponse.ErrorMessage = "Clave antigua invalida.";
                }
            }
            else
            {
                oDataBaseResponse.Exito = false;
                oDataBaseResponse.ErrorMessage = "Usuario invalido.";
            }

            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            response = javaScriptSerializer.Serialize(oDataBaseResponse);

            return response;
        }


        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string ActualizarAutorizacion()
        {
            string idAutorizacion = HttpContext.Current.Request.Params["idAutorizacion"];
            string nombre = HttpContext.Current.Request.Params["nombre"];
            string fechaInicio = HttpContext.Current.Request.Params["fechaInicio"];
            string fechaFin = HttpContext.Current.Request.Params["fechaFin"];
            string estado = HttpContext.Current.Request.Params["estado"];
            string idRegla = HttpContext.Current.Request.Params["idRegla"];
            string idEStacionamiento = HttpContext.Current.Request.Params["idEStacionamiento"];
            string tipo = HttpContext.Current.Request.Params["tipo"];

            string response = string.Empty;
            DataBaseResponse oDataBaseResponse = new DataBaseResponse();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;

            query = "update T_Autorizaciones set IdReglaAutorizacion='" + idRegla
                    + "', NombreAutorizacion='" + nombre
                    + "', FechaInicial=convert(datetime,'" + fechaInicio + "',103)"
                    + ", FechaFinal=convert(datetime,'" + fechaFin + "',103)"
                    + ", estado='" + estado
                    + "', idtipo=" + tipo
                    + " where IdAutorizacion='" + idAutorizacion + "' and IdEstacionamiento = '" + idEStacionamiento + "'";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    int respuesta = cmd.ExecuteNonQuery();

                    if (respuesta <= 0)
                    {
                        oDataBaseResponse.Exito = false;
                        oDataBaseResponse.ErrorMessage = "No fue posible actualizar el registro.";
                    }
                    else
                    {
                        RegistrarAuditoria(TipoAccion.update, "t_autorizaciones", query);
                    }
                }
            }

            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            response = javaScriptSerializer.Serialize(oDataBaseResponse);

            return response;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string InactivarTarjeta()
        {
            string idTransaccion = HttpContext.Current.Request.Params["idTransaccion"];
            string idTarjeta = HttpContext.Current.Request.Params["idTarjeta"];
            string idTarjetaFinal = string.Empty;


            string response = string.Empty;
            DataBaseResponse oDataBaseResponse = new DataBaseResponse();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;

            query = "select IdTarjeta from T_Transacciones where IdTransaccion='" + idTransaccion + "'";


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
                            RegistrarAuditoria(TipoAccion.select, "t_transacciones", query);
                            // Read advances to the next row.
                            while (reader.Read())
                            {
                                idTarjetaFinal = reader[0].ToString();
                            }
                        }
                        else
                        {
                            oDataBaseResponse.Exito = false;
                            oDataBaseResponse.ErrorMessage = "No encontro registro.";
                        }
                    }
                }
            }

            if (idTarjetaFinal != string.Empty)
            {
                query = "update T_Tarjetas set Estado='false'"
                        + " where IdTarjeta='" + idTarjetaFinal + "'";


                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        int respuesta = cmd.ExecuteNonQuery();

                        if (respuesta <= 0)
                        {
                            oDataBaseResponse.Exito = false;
                            oDataBaseResponse.ErrorMessage = "No fue posible actualizar el registro.";
                        }
                        else
                        {
                            RegistrarAuditoria(TipoAccion.update, "t_tarjetas", query);
                        }
                    }
                }

                query = "update T_Transacciones set IdTarjeta='" + idTarjeta + "'"
                        + " where IdTransaccion='" + idTransaccion + "'";


                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        int respuesta = cmd.ExecuteNonQuery();

                        if (respuesta <= 0)
                        {
                            oDataBaseResponse.Exito = false;
                            oDataBaseResponse.ErrorMessage = "No fue posible actualizar el registro.";
                        }
                        else
                        {
                            RegistrarAuditoria(TipoAccion.update, "t_transacciones", query);
                        }
                    }
                }
            }

            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            response = javaScriptSerializer.Serialize(oDataBaseResponse);

            return response;
        }

        //Delete

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string EliminarParametro()
        {
            string idEstacionamiento = HttpContext.Current.Request.Params["idEstacionamiento"];
            string codigo = HttpContext.Current.Request.Params["codigo"];

            string response = string.Empty;
            DataBaseResponse oDataBaseResponse = new DataBaseResponse();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;

            query = "delete from t_parametros where IdEstacionamiento = '" + idEstacionamiento + "' and Codigo = '" + codigo + "'";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();

                    int result = cmd.ExecuteNonQuery();
                    if (result <= 0)
                    {
                        oDataBaseResponse.Exito = false;
                        oDataBaseResponse.ErrorMessage = "No fue posible actualizar el registro.";
                    }
                    else
                    {
                        RegistrarAuditoria(TipoAccion.delete, "t_parametros", query);
                    }
                }
            }

            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            response = javaScriptSerializer.Serialize(oDataBaseResponse);

            return response;
        }

        //Crear

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string AplicarCortesia()
        {
            string idEstacionamiento = HttpContext.Current.Request.Params["idEstacionamiento"];
            string observacion = HttpContext.Current.Request.Params["observacion"];
            string idMotivo = HttpContext.Current.Request.Params["idMotivo"];
            string idTransaccion = HttpContext.Current.Request.Params["idTransaccion"];

            string response = string.Empty;
            DataBaseResponse oDataBaseResponse = new DataBaseResponse();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string user = HttpContext.Current.User.Identity.Name.ToString();

            string query = string.Empty;

            query = "insert into t_cortesias values('" + idTransaccion + "','" + idEstacionamiento + "',GETDATE(),'" + user + "','" + idMotivo + "', '" + observacion + "','false')";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    int result = cmd.ExecuteNonQuery();
                    if (result <= 0)
                    {
                        oDataBaseResponse.Exito = false;
                        oDataBaseResponse.ErrorMessage = "No fue posible insertar el registro.";
                    }
                    else
                    {
                        RegistrarAuditoria(TipoAccion.insert, "t_cortesias", query);
                    }
                }
            }

            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            response = javaScriptSerializer.Serialize(oDataBaseResponse);

            return response;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string CrearSede()
        {
            string nombre = HttpContext.Current.Request.Params["nombre"];
            string ciudad = HttpContext.Current.Request.Params["ciudad"];
            string departamento = HttpContext.Current.Request.Params["departamento"];
            string direccion = HttpContext.Current.Request.Params["direccion"];
            string usuario = HttpContext.Current.Request.Params["usuario"];
            string telefono = HttpContext.Current.Request.Params["telefono"];
            string estado = HttpContext.Current.Request.Params["estado"];

            string response = string.Empty;
            DataBaseResponse oDataBaseResponse = new DataBaseResponse();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;



            string user = HttpContext.Current.User.Identity.Name.ToString();
            query = "SELECT Cargo FROM T_Usuarios where Documento='" + user + "'";

            string CARGO = string.Empty;
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
                                CARGO = reader["Cargo"].ToString();
                            }
                        }
                    }
                }
            }


            if (CARGO == "CONTROL INTERNO")
            {

                query = "insert into t_sedes values('" + nombre + "','" + ciudad + "','" + departamento + "','" + direccion + "','" + usuario + "', '" + telefono + "','" + estado + "')";


                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        int result = cmd.ExecuteNonQuery();
                        if (result <= 0)
                        {
                            oDataBaseResponse.Exito = false;
                            oDataBaseResponse.ErrorMessage = "No fue posible insertar el registro.";
                        }
                        else
                        {
                            RegistrarAuditoria(TipoAccion.insert, "t_sedes", query);
                        }
                    }
                }

            }
            else
            {
                oDataBaseResponse.Exito = false;
                oDataBaseResponse.ErrorMessage = "No tiene permiso para realizar esta operacion.";
            }


            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            response = javaScriptSerializer.Serialize(oDataBaseResponse);

            return response;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string CrearEstacionamiento()
        {
            string nombre = HttpContext.Current.Request.Params["nombre"];
            string direccion = HttpContext.Current.Request.Params["direccion"];
            string telefono = HttpContext.Current.Request.Params["telefono"];
            string persona = HttpContext.Current.Request.Params["persona"];
            string ciudad = HttpContext.Current.Request.Params["ciudad"];
            string departamento = HttpContext.Current.Request.Params["departamento"];
            string email = HttpContext.Current.Request.Params["email"];
            string fechac = HttpContext.Current.Request.Params["fechac"];
            string fechav = HttpContext.Current.Request.Params["fechav"];
            string paquete = HttpContext.Current.Request.Params["paquete"];
            string numero = HttpContext.Current.Request.Params["numero"];
            string ejecutivo = HttpContext.Current.Request.Params["ejecutivo"];
            string idSede = HttpContext.Current.Request.Params["idSede"];
            string estado = HttpContext.Current.Request.Params["estado"];

            string response = string.Empty;
            DataBaseResponse oDataBaseResponse = new DataBaseResponse();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;

            query = "insert into t_estacionamientos values('" + nombre + "','" + direccion + "','" + telefono + "','" + persona + "','" + email + "','" + fechac + "','" + fechav + "','" + paquete + "','" + numero + "','" + ejecutivo + "','" + idSede + "','" + estado + "')";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    int result = cmd.ExecuteNonQuery();
                    if (result <= 0)
                    {
                        oDataBaseResponse.Exito = false;
                        oDataBaseResponse.ErrorMessage = "No fue posible insertar el registro.";
                    }
                    else
                    {
                        RegistrarAuditoria(TipoAccion.insert, "t_estacionamientos", query);
                    }
                }
            }

            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            response = javaScriptSerializer.Serialize(oDataBaseResponse);

            return response;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string CrearFacturaManual()
        {
            string idEstacionamiento = HttpContext.Current.Request.Params["idEstacionamiento"];
            string idModulo = HttpContext.Current.Request.Params["idModulo"];
            string fecha = HttpContext.Current.Request.Params["fecha"];
            string total = HttpContext.Current.Request.Params["total"];
            string subTotal = HttpContext.Current.Request.Params["subTotal"];
            string iva = HttpContext.Current.Request.Params["iva"];
            string prefijo = HttpContext.Current.Request.Params["prefijo"];
            string numFactura = HttpContext.Current.Request.Params["numFactura"];
            string TipoV = HttpContext.Current.Request.Params["TipoV"];

            string user = HttpContext.Current.User.Identity.Name.ToString();

            string response = string.Empty;
            DataBaseResponse oDataBaseResponse = new DataBaseResponse();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;


            query = "select IdEstacionamiento from T_Configuracion where IdModulo='" + idModulo + "'";


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
                            RegistrarAuditoria(TipoAccion.select, "t_configuracion", query);
                            // Read advances to the next row.
                            while (reader.Read())
                            {
                                idEstacionamiento = reader[0].ToString();
                            }
                        }
                        else
                        {
                            oDataBaseResponse.Exito = false;
                            oDataBaseResponse.ErrorMessage = "No encontro registro.";
                        }
                    }
                }
            }

            if (idEstacionamiento != "0")
            {
                query = "insert into t_facturasmanuales values('" + idModulo + "','" + idEstacionamiento + "',convert(datetime,'" + fecha + "',103)" + ",'" + subTotal + "','" + iva + "','" + total + "','" + prefijo + "', '" + numFactura + "','" + TipoV + "','" + user + "', 'false')";


                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        int result = cmd.ExecuteNonQuery();
                        if (result <= 0)
                        {
                            oDataBaseResponse.Exito = false;
                            oDataBaseResponse.ErrorMessage = "No fue posible insertar el registro.";
                        }
                        else
                        {
                            RegistrarAuditoria(TipoAccion.insert, "t_facturasmanuales", query);
                        }
                    }
                }
            }

            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            response = javaScriptSerializer.Serialize(oDataBaseResponse);

            return response;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string CrearConsignacion()
        {
            string idEstacionamiento = HttpContext.Current.Request.Params["idEstacionamiento"];
            string fecha = HttpContext.Current.Request.Params["fecha"];
            string total = HttpContext.Current.Request.Params["total"];
            string referencia = HttpContext.Current.Request.Params["referencia"];
            string IdTipoDeposito = HttpContext.Current.Request.Params["TipoD"];


            string user = HttpContext.Current.User.Identity.Name.ToString();

            string response = string.Empty;
            DataBaseResponse oDataBaseResponse = new DataBaseResponse();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;



            query = "insert into T_Consignacion values('" + idEstacionamiento + "',convert(datetime,'" + fecha + "',103)" + ",'" + total + "','" + referencia + "','" + user + "','" + IdTipoDeposito + "', 'false')";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    int result = cmd.ExecuteNonQuery();
                    if (result <= 0)
                    {
                        oDataBaseResponse.Exito = false;
                        oDataBaseResponse.ErrorMessage = "No fue posible insertar el registro.";
                    }
                    else
                    {
                        RegistrarAuditoria(TipoAccion.insert, "t_facturasmanuales", query);
                    }
                }
            }


            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            response = javaScriptSerializer.Serialize(oDataBaseResponse);

            return response;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string CrearModulo()
        {
            string idEstacionamiento = HttpContext.Current.Request.Params["idEstacionamiento"];
            string idModulo = HttpContext.Current.Request.Params["idModulo"];
            string ip = HttpContext.Current.Request.Params["ip"];
            string carril = HttpContext.Current.Request.Params["carril"];
            string idTipoModulo = HttpContext.Current.Request.Params["idTipoModulo"];
            string extension = HttpContext.Current.Request.Params["extension"];
            string estado = HttpContext.Current.Request.Params["estado"];
            string mac = HttpContext.Current.Request.Params["mac"];

            string response = string.Empty;
            DataBaseResponse oDataBaseResponse = new DataBaseResponse();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;

            query = "insert into t_configuracion values('" + idModulo + "','" + idEstacionamiento + "','" + ip + "','" + mac + "','" + carril + "','" + idTipoModulo + "','" + extension + "', '" + estado + "')";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    int result = cmd.ExecuteNonQuery();
                    if (result <= 0)
                    {
                        oDataBaseResponse.Exito = false;
                        oDataBaseResponse.ErrorMessage = "No fue posible insertar el registro.";
                    }
                    else
                    {
                        RegistrarAuditoria(TipoAccion.insert, "t_configuracion", query);
                    }
                }
            }

            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            response = javaScriptSerializer.Serialize(oDataBaseResponse);

            return response;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string CrearParametro()
        {
            string idEstacionamiento = HttpContext.Current.Request.Params["idEstacionamiento"];
            string codigo = HttpContext.Current.Request.Params["codigo"];
            string valor = HttpContext.Current.Request.Params["valor"];
            string descripcion = HttpContext.Current.Request.Params["descripcion"];
            string estado = HttpContext.Current.Request.Params["estado"];


            string response = string.Empty;
            DataBaseResponse oDataBaseResponse = new DataBaseResponse();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;

            query = "insert into t_parametros values('" + idEstacionamiento + "','" + codigo + "','" + valor + "','" + descripcion + "','" + estado + "',0)";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    int result = cmd.ExecuteNonQuery();
                    if (result <= 0)
                    {
                        oDataBaseResponse.Exito = false;
                        oDataBaseResponse.ErrorMessage = "No fue posible insertar el registro.";
                    }
                    else
                    {
                        RegistrarAuditoria(TipoAccion.insert, "t_parametros", query);
                    }
                }
            }

            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            response = javaScriptSerializer.Serialize(oDataBaseResponse);

            return response;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string CrearFactura()
        {
            string idModulo = HttpContext.Current.Request.Params["idModulo"];
            string idEstacionamiento = HttpContext.Current.Request.Params["idEstacionamiento"];
            string prefijo = HttpContext.Current.Request.Params["prefijo"];
            string inicial = HttpContext.Current.Request.Params["inicial"];
            string final = HttpContext.Current.Request.Params["final"];
            string numero = HttpContext.Current.Request.Params["numero"];
            string fecha = HttpContext.Current.Request.Params["fecha"];
            string estado = HttpContext.Current.Request.Params["estado"];
            string fin = HttpContext.Current.Request.Params["fin"];

            string response = string.Empty;
            DataBaseResponse oDataBaseResponse = new DataBaseResponse();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;

            query = "insert into t_facturacion values ('"
                    + idModulo + "','"
                    + idEstacionamiento + "','"
                    + prefijo + "','"
                    + inicial + "','"
                    + final + "','"
                    + inicial + "','"
                    + numero + "','"
                    + fecha + "','"
                    + estado + "','"
                    + fin + "',0)";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    int respuesta = cmd.ExecuteNonQuery();

                    if (respuesta <= 0)
                    {
                        oDataBaseResponse.Exito = false;
                        oDataBaseResponse.ErrorMessage = "No fue posible insertar el registro.";
                    }
                    else
                    {
                        RegistrarAuditoria(TipoAccion.insert, "t_facturacion", query);
                    }
                }
            }

            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            response = javaScriptSerializer.Serialize(oDataBaseResponse);

            return response;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string CrearUsuario()
        {
            //string idEstacionamiento = HttpContext.Current.Request.Params["idEstacionamiento"];
            string documento = HttpContext.Current.Request.Params["documento"];
            string nombres = HttpContext.Current.Request.Params["nombres"];
            string apellidos = HttpContext.Current.Request.Params["apellidos"];
            string usuario = HttpContext.Current.Request.Params["usuario"];
            string cargo = HttpContext.Current.Request.Params["cargo"];
            string estado = HttpContext.Current.Request.Params["estado"];

            string response = string.Empty;
            DataBaseResponse oDataBaseResponse = new DataBaseResponse();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string user = HttpContext.Current.User.Identity.Name.ToString();

            string query = string.Empty;

            query = "insert into t_usuarios values('" + documento + "','" + nombres + "','" + apellidos + "','" + usuario + "','" + Encrypt("123") + "','" + cargo + "','" + user + "',GETDATE(),NULL,'" + estado + "',0)";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    int result = cmd.ExecuteNonQuery();
                    if (result <= 0)
                    {
                        oDataBaseResponse.Exito = false;
                        oDataBaseResponse.ErrorMessage = "No fue posible insertar el registro.";
                    }
                    else
                    {
                        RegistrarAuditoria(TipoAccion.insert, "t_usuarios", query);
                    }
                }
            }

            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            response = javaScriptSerializer.Serialize(oDataBaseResponse);

            return response;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string CrearTarjeta()
        {
            string idEstacionamiento = HttpContext.Current.Request.Params["idEstacionamiento"];
            string idTarjeta = HttpContext.Current.Request.Params["idTarjeta"];
            string estado = HttpContext.Current.Request.Params["estado"];

            string user = HttpContext.Current.User.Identity.Name.ToString();

            string response = string.Empty;
            DataBaseResponse oDataBaseResponse = new DataBaseResponse();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;

            //query = "insert into t_tarjetas values ('"+idEstacionamiento+"','" + idTarjeta + "',GETDATE(),'"+user+"','" + estado + "')"; 

            //NEW QUERY VERIFICA SI YA ESTA EN LA DB LA TARJETA//
            query = "IF NOT EXISTS (SELECT top (1) idTarjeta FROM t_tarjetas WHERE idTarjeta= '" + idTarjeta + "')" + " BEGIN"
            + " insert into t_tarjetas values ('" + idEstacionamiento + "','" + idTarjeta + "',GETDATE(),'" + user + "','" + estado + "')" +
            " END";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    int result = cmd.ExecuteNonQuery();
                    if (result <= 0)
                    {
                        oDataBaseResponse.Exito = false;
                        oDataBaseResponse.ErrorMessage = "No fue posible insertar el registro.";
                    }
                    else
                    {
                        RegistrarAuditoria(TipoAccion.insert, "t_tarjetas", query);
                    }
                }
            }

            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            response = javaScriptSerializer.Serialize(oDataBaseResponse);

            return response;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string CrearEntrada()
        {
            string idEstacionamiento = HttpContext.Current.Request.Params["idEstacionamiento"];
            string idTarjeta = HttpContext.Current.Request.Params["idTarjeta"];
            string carril = HttpContext.Current.Request.Params["carril"];
            string placa = HttpContext.Current.Request.Params["placa"];
            string fecha = HttpContext.Current.Request.Params["fecha"];
            string tipov = HttpContext.Current.Request.Params["tipov"];
            string modulo = string.Empty;

            string user = HttpContext.Current.User.Identity.Name.ToString();

            string dia = string.Empty;
            string mes = string.Empty;
            string anho = string.Empty;
            string hora = string.Empty;
            string min = string.Empty;

            //  string seg = string.Empty;

            string response = string.Empty;
            DataBaseResponse oDataBaseResponse = new DataBaseResponse();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;

            try
            {
                dia = fecha.Split('-')[0];
                if (dia.Length == 1)
                {
                    dia = "0" + dia;
                }
                mes = fecha.Split('-')[1];
                if (mes.Length == 1)
                {
                    mes = "0" + mes;
                }
                anho = fecha.Split('-')[2].Split(' ')[0];
                hora = fecha.Split(' ')[1].Split(':')[0];
                if (hora.Length == 1)
                {
                    hora = "0" + hora;
                }
                min = fecha.Split(' ')[1].Split(':')[1];
                if (min.Length == 1)
                {
                    min = "0" + min;
                }
                //seg = fecha.Split(' ')[1].Split(':')[2];
                //if (seg.Length == 1)
                //{
                //    seg = "0" + seg;
                //}
                string seg = "00";



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
                                RegistrarAuditoria(TipoAccion.select, "t_configuracion", query);
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

                    query = "insert into t_transacciones values ('" + anho + mes + dia + hora + min + seg + modulo + idEstacionamiento + "','" + 1 + "','" + carril + "','" + idEstacionamiento + "','" + idTarjeta + "','" + placa.ToUpper() + "'," + "convert(datetime,'" + fecha + "',103),'1900-01-01 00:00:00',NULL,NULL,NULL,'" + tipov + "',NULL, NULL, NULL, NULL, NULL, NULL, NULL, 'false', 'false', 'false')";



                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand(query, connection))
                        {
                            connection.Open();
                            int resultado = cmd.ExecuteNonQuery();
                            if (resultado <= 0)
                            {
                                oDataBaseResponse.Exito = false;
                                oDataBaseResponse.ErrorMessage = "No fue posible insertar el registro.";
                            }
                            else
                            {
                                RegistrarAuditoria(TipoAccion.insert, "t_transacciones", query);
                            }
                        }
                    }

                }
                else
                {
                    oDataBaseResponse.Exito = false;
                    oDataBaseResponse.ErrorMessage = "No se encontro modulo para el carril seleccionado.";
                }

                var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                response = javaScriptSerializer.Serialize(oDataBaseResponse);
            }
            catch (Exception e)
            {
                oDataBaseResponse.Exito = false;
                oDataBaseResponse.ErrorMessage = e.InnerException + " " + e.Message;
            }

            return response;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string CrearAutorizacion()
        {
            string nombre = HttpContext.Current.Request.Params["nombre"];
            string fechaInicio = HttpContext.Current.Request.Params["fechaInicio"];
            string fechaFin = HttpContext.Current.Request.Params["fechaFin"];
            string estado = HttpContext.Current.Request.Params["estado"];
            string idRegla = HttpContext.Current.Request.Params["idRegla"];
            string idEStacionamiento = HttpContext.Current.Request.Params["idEStacionamiento"];
            string tipo = HttpContext.Current.Request.Params["tipo"];

            string response = string.Empty;
            DataBaseResponse oDataBaseResponse = new DataBaseResponse();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;

            int idAutorizacion = 0;

            query = "select max(idautorizacion) from t_autorizaciones where idEstacionamiento='" + idEStacionamiento + "'";

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
                            RegistrarAuditoria(TipoAccion.select, "t_configuracion", query);
                            while (reader.Read())
                            {
                                if (reader[0].ToString() != string.Empty)
                                {
                                    idAutorizacion = Convert.ToInt32(reader[0].ToString());
                                }
                            }
                        }
                    }
                }
            }

            idAutorizacion = idAutorizacion + 1;
            query = "insert into T_Autorizaciones values ('" + idAutorizacion + "','" + idEStacionamiento + "','" + idRegla + "', '" + nombre + "', convert(datetime,'" + fechaInicio + "',103), convert(datetime,'" + fechaFin + "',103), '" + estado + "','False', " + tipo + ")";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    int respuesta = cmd.ExecuteNonQuery();

                    if (respuesta <= 0)
                    {
                        oDataBaseResponse.Exito = false;
                        oDataBaseResponse.ErrorMessage = "No fue posible insertar el registro.";
                    }
                    else
                    {
                        RegistrarAuditoria(TipoAccion.insert, "t_autorizaciones", query);
                    }
                }
            }


            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            response = javaScriptSerializer.Serialize(oDataBaseResponse);

            return response;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string CrearTarifa()
        {
            string idEstacionamiento = HttpContext.Current.Request.Params["idEstacionamiento"];
            string idTipoPago = HttpContext.Current.Request.Params["idTipoPago"];
            string idTipoVehiculo = HttpContext.Current.Request.Params["idTipoVehiculo"];
            string idAutorizacion = HttpContext.Current.Request.Params["idAutorizacion"];
            string valor = HttpContext.Current.Request.Params["valor"];
            string tipoCobro = HttpContext.Current.Request.Params["tipoCobro"];
            string estado = HttpContext.Current.Request.Params["estado"];
            string idConvenio = HttpContext.Current.Request.Params["idConvenio"];
            string idEvento = HttpContext.Current.Request.Params["idEvento"];


            string response = string.Empty;
            DataBaseResponse oDataBaseResponse = new DataBaseResponse();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;


            query = "insert into t_tarifas values ('" + idEstacionamiento + "','" + idTipoPago + "','" + idTipoVehiculo + "'," + (Convert.ToInt32(idConvenio) > 0 ? idConvenio.ToString() : "NULL") + "," + (Convert.ToInt32(idAutorizacion) > 0 ? idAutorizacion.ToString() : "NULL") + "," + (Convert.ToInt32(idEvento) > 0 ? idEvento.ToString() : "NULL") + ",'" + valor + "','" + tipoCobro + "','" + estado + "',0)";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    int respuesta = cmd.ExecuteNonQuery();

                    if (respuesta <= 0)
                    {
                        oDataBaseResponse.Exito = false;
                        oDataBaseResponse.ErrorMessage = "No fue posible crear el registro.";
                    }
                    else
                    {
                        RegistrarAuditoria(TipoAccion.insert, "t_tarifas", query);
                    }
                }
            }

            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            response = javaScriptSerializer.Serialize(oDataBaseResponse);

            return response;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string CrearConvenio()
        {
            string idEstacionamiento = HttpContext.Current.Request.Params["idEstacionamiento"];
            string nombre = HttpContext.Current.Request.Params["nombre"];
            string descripcion = HttpContext.Current.Request.Params["descripcion"];
            string fechaInicial = HttpContext.Current.Request.Params["fechaInicial"];
            string fechaFinal = HttpContext.Current.Request.Params["fechaFinal"];
            string estado = HttpContext.Current.Request.Params["estado"];
            string minuto = HttpContext.Current.Request.Params["minuto"];

            string query = string.Empty;
            string response = string.Empty;
            DataBaseResponse oDataBaseResponse = new DataBaseResponse();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            // query = "insert into t_convenios values ('" + nombre + "','" + descripcion + "','" + idEstacionamiento + "',convert(datetime,'" + fechaInicial + "',103), convert(datetime,'" + fechaFinal + "',103),'" + minuto + "', '" + estado + "',0)";
            query = "insert into t_convenios values ('" + nombre + "','" + descripcion + "','" + idEstacionamiento + "',(select convert(varchar,'" + fechaInicial + "', 108)), (select convert(varchar,'" + fechaFinal + "', 108)),'" + minuto + "', '" + estado + "',0)";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    int respuesta = cmd.ExecuteNonQuery();

                    if (respuesta <= 0)
                    {
                        oDataBaseResponse.Exito = false;
                        oDataBaseResponse.ErrorMessage = "No fue posible crear el registro.";
                    }
                    else
                    {
                        RegistrarAuditoria(TipoAccion.insert, "t_tarifas", query);
                    }
                }
            }

            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            response = javaScriptSerializer.Serialize(oDataBaseResponse);

            return response;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string CrearEvento()
        {
            string idEstacionamiento = HttpContext.Current.Request.Params["idEstacionamiento"];
            string evento = HttpContext.Current.Request.Params["evento"];
            string horas = HttpContext.Current.Request.Params["horas"];
            string estado = HttpContext.Current.Request.Params["estado"];

            string query = string.Empty;
            string response = string.Empty;
            DataBaseResponse oDataBaseResponse = new DataBaseResponse();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string user = HttpContext.Current.User.Identity.Name.ToString();

            query = "insert into t_eventos values ('" + idEstacionamiento + "','" + user + "','" + evento + "','" + horas + "', '" + estado + "')";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    int respuesta = cmd.ExecuteNonQuery();

                    if (respuesta <= 0)
                    {
                        oDataBaseResponse.Exito = false;
                        oDataBaseResponse.ErrorMessage = "No fue posible crear el registro.";
                    }
                    else
                    {
                        RegistrarAuditoria(TipoAccion.insert, "t_tarifas", query);
                    }
                }
            }

            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            response = javaScriptSerializer.Serialize(oDataBaseResponse);

            return response;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string CrearAutorizado()
        {
            string documento = HttpContext.Current.Request.Params["documento"].Trim();
            string nombre = HttpContext.Current.Request.Params["nombre"];
            string estado = HttpContext.Current.Request.Params["estado"];
            string idAutorizacion = HttpContext.Current.Request.Params["idAutorizacion"];
            string idEStacionamiento = HttpContext.Current.Request.Params["idEStacionamiento"];
            string idTarjeta = HttpContext.Current.Request.Params["idTarjeta"];
            string NombreEmpresa = HttpContext.Current.Request.Params["NombreEmpresa"];
            string Nit = HttpContext.Current.Request.Params["Nit"];
            string telefono = HttpContext.Current.Request.Params["telefono"];
            string email = HttpContext.Current.Request.Params["email"];
            string placa1 = HttpContext.Current.Request.Params["placa1"].Trim();
            string placa2 = HttpContext.Current.Request.Params["placa2"].Trim();
            string placa3 = HttpContext.Current.Request.Params["placa3"].Trim();
            string placa4 = HttpContext.Current.Request.Params["placa4"].Trim();
            string placa5 = HttpContext.Current.Request.Params["placa5"].Trim();
            string valorBolsa = HttpContext.Current.Request.Params["valorBolsa"].Trim();
            string Congelar = HttpContext.Current.Request.Params["Congelar"].Trim();

            string response = string.Empty;
            DataBaseResponse oDataBaseResponse = new DataBaseResponse();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string user = HttpContext.Current.User.Identity.Name.ToString();

            string query = string.Empty;
            string DocumentoCons = string.Empty;
            string IdTarjetaCons = string.Empty;

            query = "SELECT Documento,IdTarjeta FROM T_PersonasAutorizadas where Documento='" + documento + "' OR IdTarjeta='" + idTarjeta + "'";

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
                                DocumentoCons = reader["Documento"].ToString();
                                IdTarjetaCons = reader["IdTarjeta"].ToString();
                            }
                        }
                    }
                }
            }

            if (documento == string.Empty || idTarjeta == "null")
            {
                oDataBaseResponse.Exito = false;
                oDataBaseResponse.ErrorMessage = "No fue posible crear el registro por CAMPOS VACIOS";
            }
            else if (DocumentoCons != string.Empty || IdTarjetaCons != string.Empty)
            {
                oDataBaseResponse.Exito = false;
                oDataBaseResponse.ErrorMessage = "No fue posible crear el registro por DUPLICIDAD DE DATOS";
            }
            else
            {
                query = "insert into T_PersonasAutorizadas values ('" + documento + "','" + idAutorizacion + "','" + idEStacionamiento + "','" + nombre + "','" + idTarjeta + "','" + NombreEmpresa + "','" + Nit + "',GETDATE(),'" + user + "','" + estado + "','False',NULL,NULL, '" + telefono + "', '" + email + "','" + placa1 + "','" + placa2 + "','" + placa3 + "','" + placa4 + "','" + placa5 + "','" + valorBolsa + "',1,'" + Congelar + "')";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        int respuesta = cmd.ExecuteNonQuery();

                        if (respuesta <= 0)
                        {
                            oDataBaseResponse.Exito = false;
                            oDataBaseResponse.ErrorMessage = "No fue posible crear el registro.";
                        }
                        else
                        {
                            RegistrarAuditoria(TipoAccion.insert, "t_personasautorizadas", query);
                        }
                    }
                }
            }

            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            response = javaScriptSerializer.Serialize(oDataBaseResponse);

            return response;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string ReponerAutorizado()
        {
            string idTarjetaOld = HttpContext.Current.Request.Params["idTarjetaOld"];
            string idAuthorizado = HttpContext.Current.Request.Params["idAuthorizado"];
            string idTarjetaNew = HttpContext.Current.Request.Params["idTarjetaNew"];
            string idEstacionamiento = HttpContext.Current.Request.Params["idEstacionamiento"];

            string response = string.Empty;
            DataBaseResponse oDataBaseResponse = new DataBaseResponse();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string user = HttpContext.Current.User.Identity.Name.ToString();

            string query = string.Empty;

            query = "update T_PersonasAutorizadas set idtarjeta='" + idTarjetaNew + "' where documento='" + idAuthorizado + "'";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    int respuesta = cmd.ExecuteNonQuery();

                    if (respuesta <= 0)
                    {
                        oDataBaseResponse.Exito = false;
                        oDataBaseResponse.ErrorMessage = "No fue posible actualizar el registro.";
                    }
                    else
                    {
                        RegistrarAuditoria(TipoAccion.update, "t_personasautorizadas", query);
                    }
                }
            }


            if (oDataBaseResponse.Exito)
            {
                query = "update T_Tarjetas set estado='false' where idtarjeta='" + idTarjetaOld + "'";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        int respuesta = cmd.ExecuteNonQuery();

                        if (respuesta <= 0)
                        {
                            oDataBaseResponse.Exito = false;
                            oDataBaseResponse.ErrorMessage = "No fue posible actualizar el registro.";
                        }
                        else
                        {
                            RegistrarAuditoria(TipoAccion.update, "t_tarjetas", query);
                        }
                    }
                }
            }

            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            response = javaScriptSerializer.Serialize(oDataBaseResponse);

            return response;
        }

        //Seguridad
        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public ArrayList ObtenerPermisos()
        {
            //  string user = HttpContext.Current.Request.Params["user"];

            ArrayList array = new ArrayList();

            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string user = HttpContext.Current.User.Identity.Name.ToString();

            string query = string.Empty;

            query = "select NombreControl from T_Permisos where DocumentoUsuario = '" + user + "'";


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
                            RegistrarAuditoria(TipoAccion.select, "t_permisos", query);
                            // Read advances to the next row.
                            while (reader.Read())
                            {
                                array.Add(reader["NombreControl"]);
                            }
                        }
                    }
                }
            }
            return array;


            //array.Add("navTransacciones");
            //array.Add("navCargas");
            //array.Add("navArqueos");
            //array.Add("navAutorizados");
            //array.Add("navReportes");
            //array.Add("navPPM");
            //array.Add("rnavAlarmas");
            //array.Add("rnavConfiguracion");
            //array.Add("rnavConfiguracion-sistema");
            //array.Add("rnavConfiguracion-usuarios");
            //array.Add("rnavConfiguracion-tarifas");
            //array.Add("rnavConfiguracion-convenios");
            //array.Add("rnavConfiguracion-parametros");
        }



        //Seguridad
        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public ArrayList ObtenerPermisos1()
        {
            string user = HttpContext.Current.Request.Params["persona"];

            ArrayList array = new ArrayList();

            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            //string user = HttpContext.Current.User.Identity.Name.ToString();

            string query = string.Empty;

            query = "select NombreControl from T_Permisos where DocumentoUsuario = '" + user + "'";


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
                            RegistrarAuditoria(TipoAccion.select, "t_permisos", query);
                            // Read advances to the next row.
                            while (reader.Read())
                            {
                                array.Add(reader["NombreControl"]);
                            }
                        }
                    }
                }
            }
            return array;


            //array.Add("navTransacciones");
            //array.Add("navCargas");
            //array.Add("navArqueos");
            //array.Add("navAutorizados");
            //array.Add("navReportes");
            //array.Add("navPPM");
            //array.Add("rnavAlarmas");
            //array.Add("rnavConfiguracion");
            //array.Add("rnavConfiguracion-sistema");
            //array.Add("rnavConfiguracion-usuarios");
            //array.Add("rnavConfiguracion-tarifas");
            //array.Add("rnavConfiguracion-convenios");
            //array.Add("rnavConfiguracion-parametros");
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string ObtenerInformacionUsuario()
        {
            string user = HttpContext.Current.User.Identity.Name.ToString();

            ArrayList array = new ArrayList();
            string response = string.Empty;
            DataBaseResponse oDataBaseResponse = new DataBaseResponse();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;

            query = "select Documento, Nombres, Apellidos, Usuario, Cargo from t_usuarios where documento='" + user + "'";


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
                            RegistrarAuditoria(TipoAccion.select, "t_usuarios", query);
                            // Read advances to the next row.
                            while (reader.Read())
                            {
                                array.Add(new
                                {
                                    Documento = reader[0].ToString(),
                                    Nombres = reader[1].ToString() + " " + reader[2].ToString(),
                                    Usuario = reader[3].ToString(),
                                    Cargo = reader[4].ToString(),
                                });
                            }
                        }
                    }
                }
            }

            if (array.Count > 0)
            {
                oDataBaseResponse.Resultado = array;
            }
            else
            {
                oDataBaseResponse.Exito = false;
                oDataBaseResponse.ErrorMessage = "No encuentra informacion facturacion.";
            }

            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            response = javaScriptSerializer.Serialize(oDataBaseResponse);

            return response;
        }





        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string ObtenerInformacionCargo()
        {
            string user = HttpContext.Current.User.Identity.Name.ToString();

            ArrayList array = new ArrayList();
            string response = string.Empty;
            DataBaseResponse oDataBaseResponse = new DataBaseResponse();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;

            query = "select Cargo from t_usuarios where documento='" + user + "'";


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
                            RegistrarAuditoria(TipoAccion.select, "t_usuarios", query);
                            // Read advances to the next row.
                            while (reader.Read())
                            {
                                array.Add(new
                                {
                                    Cargo = reader[0].ToString(),
                                });
                            }
                        }
                    }
                }
            }

            if (array.Count > 0)
            {
                oDataBaseResponse.Resultado = array;
            }
            else
            {
                oDataBaseResponse.Exito = false;
                oDataBaseResponse.ErrorMessage = "No encuentra informacion facturacion.";
            }

            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            response = javaScriptSerializer.Serialize(oDataBaseResponse);

            return response;
        }





        //Factura
        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string ObtenerDatosFactura()
        {
            string idTransaccion = HttpContext.Current.Request.Params["idTransaccion"];

            ArrayList array = new ArrayList();
            string response = string.Empty;
            DataBaseResponse oDataBaseResponse = new DataBaseResponse();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;

            query = "select f.Prefijo + '-' + p.NumeroFactura, e.Nombre, e.TelefonoContacto, e.Direccion, c.IdModulo, p.FechaPago, p.IdTransaccion, t.PlacaEntrada, tp.TipoPago, p.Total, t.ValorRecibido, t.Cambio, p.Subtotal, p.Iva, f.NumeroResolucion + ' ' + f.FechaResolucion + ' DEL ' + f.FacturaInicial + ' AL ' + f.FacturaFinal , t.FechaEntrada, tv.TipoVehiculo,"
                    + " (select count(cantidad) from (select count(*) as cantidad"
                                + " from T_Pagos"
                                + " where IdTransaccion='2017040609064312'"
                                + " group by(NumeroFactura)) as myTable)"
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
                    + " order by p.NumeroFactura";


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
                            RegistrarAuditoria(TipoAccion.select, "t_pagos", query);
                            // Read advances to the next row.
                            while (reader.Read())
                            {
                                array.Add(new
                                {
                                    NumeroFactura = reader[0].ToString(),
                                    Nombre = reader[1].ToString(),
                                    Telefono = reader[2].ToString(),
                                    Direccion = reader[3].ToString(),
                                    Modulo = reader[4].ToString(),
                                    Fecha = reader[5].ToString(),
                                    IdTransaccion = reader[6].ToString(),
                                    Placa = reader[7].ToString(),
                                    Tipo = reader[8].ToString(),
                                    Total = reader[9].ToString(),
                                    ValorRecibido = reader[10].ToString(),
                                    Cambio = reader[11].ToString(),
                                    Subtotal = reader[12].ToString(),
                                    Iva = reader[13].ToString(),
                                    NumeroResolucion = reader[14].ToString(),
                                    FechaEntrada = reader[15].ToString(),
                                    TipoVehiculo = reader[16].ToString(),
                                    Cantidad = reader[17].ToString()
                                });
                            }
                        }
                    }
                }
            }

            if (array.Count > 0)
            {
                oDataBaseResponse.Resultado = array;
            }
            else
            {
                oDataBaseResponse.Exito = false;
                oDataBaseResponse.ErrorMessage = "No encuentra informacion facturacion.";
            }

            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            response = javaScriptSerializer.Serialize(oDataBaseResponse);

            return response;
        }



        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string ObtenerDatosFacturaxFactura()
        {
            string numFactura = HttpContext.Current.Request.Params["numFactura"];

            ArrayList array = new ArrayList();
            string response = string.Empty;
            DataBaseResponse oDataBaseResponse = new DataBaseResponse();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;

            query = "select f.Prefijo + '-' + p.NumeroFactura, e.Nombre, e.TelefonoContacto, e.Direccion, c.IdModulo, p.FechaPago, p.IdTransaccion, t.PlacaEntrada, tp.TipoPago, p.Total, t.ValorRecibido, t.Cambio, p.Subtotal, p.Iva, f.NumeroResolucion + ' ' + f.FechaResolucion + ' DEL ' + f.FacturaInicial + ' AL ' + f.FacturaFinal , t.FechaEntrada, tv.TipoVehiculo,"
                    + " (select count(cantidad) from (select count(*) as cantidad"
                                + " from T_Pagos"
                                + " where IdTransaccion='" + numFactura + "'"
                                + " group by(NumeroFactura)) as myTable)"
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
                    + " where f.Prefijo + '-' + p.NumeroFactura='" + numFactura + "'"
                    + " order by p.NumeroFactura";


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
                            RegistrarAuditoria(TipoAccion.select, "t_pagos", query);
                            // Read advances to the next row.
                            while (reader.Read())
                            {
                                array.Add(new
                                {
                                    NumeroFactura = reader[0].ToString(),
                                    Nombre = reader[1].ToString(),
                                    Telefono = reader[2].ToString(),
                                    Direccion = reader[3].ToString(),
                                    Modulo = reader[4].ToString(),
                                    Fecha = reader[5].ToString(),
                                    IdTransaccion = reader[6].ToString(),
                                    Placa = reader[7].ToString(),
                                    Tipo = reader[8].ToString(),
                                    Total = reader[9].ToString(),
                                    ValorRecibido = reader[10].ToString(),
                                    Cambio = reader[11].ToString(),
                                    Subtotal = reader[12].ToString(),
                                    Iva = reader[13].ToString(),
                                    NumeroResolucion = reader[14].ToString(),
                                    FechaEntrada = reader[15].ToString(),
                                    TipoVehiculo = reader[16].ToString(),
                                    Cantidad = reader[17].ToString()
                                });
                            }
                        }
                    }
                }
            }

            if (array.Count > 0)
            {
                oDataBaseResponse.Resultado = array;
            }
            else
            {
                query = "select top(1) f.Prefijo + '-' + p.NumeroFactura, e.Nombre, e.TelefonoContacto, e.Direccion, c.IdModulo, p.FechaPago, p.IdTransaccion, tp.TipoPago, p.Total, p.Subtotal, p.Iva, f.NumeroResolucion + ' ' + f.FechaResolucion + ' DEL ' + f.FacturaInicial + ' AL ' + f.FacturaFinal, au.NombreAutorizacion, pa.Documento"
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
                                    + " where f.Prefijo + '-' + p.NumeroFactura='" + numFactura + "'"
                                    + " order by p.NumeroFactura";


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
                                RegistrarAuditoria(TipoAccion.select, "t_pagos", query);
                                // Read advances to the next row.
                                while (reader.Read())
                                {
                                    array.Add(new
                                    {
                                        NumeroFactura = reader[0].ToString(),
                                        Nombre = reader[1].ToString(),
                                        Telefono = reader[2].ToString(),
                                        Direccion = reader[3].ToString(),
                                        Modulo = reader[4].ToString(),
                                        Fecha = reader[5].ToString(),
                                        IdTransaccion = reader[6].ToString(),
                                        Tipo = reader[7].ToString(),
                                        Total = reader[8].ToString(),
                                        Subtotal = reader[9].ToString(),
                                        Iva = reader[10].ToString(),
                                        NumeroResolucion = reader[11].ToString(),
                                        NombreAutorizacion = reader[12].ToString(),
                                        Documento = reader[13].ToString()
                                    });
                                }
                            }
                        }
                    }
                }


                if (array.Count > 0)
                {
                    oDataBaseResponse.Resultado = array;
                }
                else
                {
                    oDataBaseResponse.Exito = false;
                    oDataBaseResponse.ErrorMessage = "No encuentra informacion facturacion.";
                }
            }

            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            response = javaScriptSerializer.Serialize(oDataBaseResponse);

            return response;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string AnularFactura()
        {
            string numFactura = HttpContext.Current.Request.Params["numFactura"];
            string idFacturacion = string.Empty;
            string prefijo = numFactura.Split('-')[0];
            string factura = numFactura.Split('-')[1];

            ArrayList array = new ArrayList();
            string response = string.Empty;
            DataBaseResponse oDataBaseResponse = new DataBaseResponse();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;

            query = "select idfacturacion from t_facturacion where prefijo='" + prefijo + "'";


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
                            RegistrarAuditoria(TipoAccion.select, "t_faturacion", query);
                            // Read advances to the next row.
                            while (reader.Read())
                            {
                                idFacturacion = reader[0].ToString();
                            }
                        }
                        else
                        {
                            oDataBaseResponse.Exito = false;
                            oDataBaseResponse.ErrorMessage = "No encontro registro.";
                        }
                    }
                }
            }

            if (idFacturacion != string.Empty)
            {
                query = "update T_pagos set Anulada='true'"
                        + " where idfacturacion='" + idFacturacion + "' and NumeroFactura='" + factura + "'";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        int respuesta = cmd.ExecuteNonQuery();

                        if (respuesta <= 0)
                        {
                            oDataBaseResponse.Exito = false;
                            oDataBaseResponse.ErrorMessage = "No fue posible actualizar el registro.";
                        }
                        else
                        {
                            RegistrarAuditoria(TipoAccion.update, "t_pagos", query);
                        }
                    }
                }
            }

            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            response = javaScriptSerializer.Serialize(oDataBaseResponse);

            return response;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string ObtenerDatosFacturaMensualidad()
        {
            string idTransaccion = HttpContext.Current.Request.Params["idTransaccion"];
            string idAutorizacion = HttpContext.Current.Request.Params["idAutorizacion"];

            ArrayList array = new ArrayList();
            string response = string.Empty;
            DataBaseResponse oDataBaseResponse = new DataBaseResponse();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;
            try
            {
                query = "select top(1) f.Prefijo + '-' + p.NumeroFactura, e.Nombre, e.TelefonoContacto, e.Direccion, c.IdModulo, p.FechaPago, p.IdTransaccion, tp.TipoPago, p.Total, p.Subtotal, p.Iva, f.NumeroResolucion + ' ' + f.FechaResolucion + ' DEL ' + f.FacturaInicial + ' AL ' + f.FacturaFinal, au.NombreAutorizacion, pa.Documento"
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
                                RegistrarAuditoria(TipoAccion.select, "t_pagos", query);
                                // Read advances to the next row.
                                while (reader.Read())
                                {
                                    array.Add(new
                                    {
                                        NumeroFactura = reader[0].ToString(),
                                        Nombre = reader[1].ToString(),
                                        Telefono = reader[2].ToString(),
                                        Direccion = reader[3].ToString(),
                                        Modulo = reader[4].ToString(),
                                        Fecha = reader[5].ToString(),
                                        IdTransaccion = reader[6].ToString(),
                                        Tipo = reader[7].ToString(),
                                        Total = reader[8].ToString(),
                                        Subtotal = reader[9].ToString(),
                                        Iva = reader[10].ToString(),
                                        NumeroResolucion = reader[11].ToString(),
                                        NombreAutorizacion = reader[12].ToString(),
                                        Documento = reader[13].ToString()
                                    });
                                }
                            }
                        }
                    }
                }

                if (array.Count > 0)
                {
                    oDataBaseResponse.Resultado = array;
                }
                else
                {
                    oDataBaseResponse.Exito = false;
                    oDataBaseResponse.ErrorMessage = "No encuentra informacion facturacion.; " + query;
                }
            }
            catch (Exception e)
            {
                oDataBaseResponse.Exito = false;
                oDataBaseResponse.ErrorMessage = "Exception; .; " + e.InnerException + " " + e.Message;
            }

            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            response = javaScriptSerializer.Serialize(oDataBaseResponse);

            return response;
        }

        //Factura
        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string ObtenerDatosComprobanteArqueo()
        {
            string idArqueo = HttpContext.Current.Request.Params["idArqueo"];

            ArrayList array = new ArrayList();
            string response = string.Empty;
            DataBaseResponse oDataBaseResponse = new DataBaseResponse();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;

            query = "select e.Nombre, e.TelefonoContacto, e.Direccion, c.IdModulo, a.FechaFin, a.IdArqueo, a.Producido, a.CantTransacciones, a.Valor"
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
                            RegistrarAuditoria(TipoAccion.select, "t_arqueos", query);
                            // Read advances to the next row.
                            while (reader.Read())
                            {
                                array.Add(new
                                {
                                    Nombre = reader[0].ToString(),
                                    Telefono = reader[1].ToString(),
                                    Direccion = reader[2].ToString(),
                                    Modulo = reader[3].ToString(),
                                    Fecha = reader[4].ToString(),
                                    IdArqueo = reader[5].ToString(),
                                    Producido = reader[6].ToString(),
                                    CantTransacciones = reader[7].ToString(),
                                    Valor = reader[8].ToString(),
                                });
                            }
                        }
                    }
                }
            }

            if (array.Count > 0)
            {
                oDataBaseResponse.Resultado = array;
            }
            else
            {
                oDataBaseResponse.Exito = false;
                oDataBaseResponse.ErrorMessage = "No encuentra informacion ticket arquero.";
            }

            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            response = javaScriptSerializer.Serialize(oDataBaseResponse);

            return response;
        }

        //Factura
        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string ObtenerDatosComprobanteCarga()
        {
            string idCarga = HttpContext.Current.Request.Params["idCarga"];

            ArrayList array = new ArrayList();
            string response = string.Empty;
            DataBaseResponse oDataBaseResponse = new DataBaseResponse();
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
                            RegistrarAuditoria(TipoAccion.select, "t_carga", query);
                            // Read advances to the next row.
                            while (reader.Read())
                            {
                                array.Add(new
                                {
                                    Nombre = reader[0].ToString(),
                                    Telefono = reader[1].ToString(),
                                    Direccion = reader[2].ToString(),
                                    Modulo = reader[3].ToString(),
                                    Fecha = reader[4].ToString(),
                                    IdCarga = reader[5].ToString(),
                                    Valor = reader[6].ToString(),
                                    IdUsuario = reader[7].ToString(),
                                    IdEstacionamiento = reader[8].ToString(),
                                });
                            }
                        }
                    }
                }
            }

            if (array.Count > 0)
            {
                oDataBaseResponse.Resultado = array;
            }
            else
            {
                oDataBaseResponse.Exito = false;
                oDataBaseResponse.ErrorMessage = "No encuentra informacion ticket carga.";
            }

            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            response = javaScriptSerializer.Serialize(oDataBaseResponse);

            return response;
        }


        //Reporte
        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string GetItemsReportes()
        {
            int sEcho = ToInt(HttpContext.Current.Request.Params["sEcho"]);
            int iDisplayLength = ToInt(HttpContext.Current.Request.Params["iDisplayLength"]);
            int iDisplayStart = ToInt(HttpContext.Current.Request.Params["iDisplayStart"]);
            //string rawSearch = HttpContext.Current.Request.Params["sSearch"];

            string columnas = HttpContext.Current.Request.Params["columnas"];
            string from = HttpContext.Current.Request.Params["from"];
            string where = HttpContext.Current.Request.Params["where"];
            string colum0 = HttpContext.Current.Request.Params["colum0"];
            string groupby = HttpContext.Current.Request.Params["groupby"];

            string idusuario = HttpContext.Current.Request.Params["idusuario"];
            string idmodulo = HttpContext.Current.Request.Params["idmodulo"];
            string idsede = HttpContext.Current.Request.Params["idsede"];
            string idestacionamiento = HttpContext.Current.Request.Params["idestacionamiento"];
            string fechaAntesFin = HttpContext.Current.Request.Params["sFechaAntesFin"];
            string fechaDespuesFin = HttpContext.Current.Request.Params["sFechaDespuesFin"];
            string fechaAntesCortesia = HttpContext.Current.Request.Params["sFechaAntesCortesia"];
            string fechaDespuesCortesia = HttpContext.Current.Request.Params["sFechaDespuesCortesia"];
            string idMotivo = HttpContext.Current.Request.Params["idMotivo"];
            string fechaSola = HttpContext.Current.Request.Params["fechaSola"];
            string fechaPagoAntes = HttpContext.Current.Request.Params["fechaPagoAntes"];
            string fechaPagoDespues = HttpContext.Current.Request.Params["fechaPagoDespues"];
            string tipoVehiculo = HttpContext.Current.Request.Params["tipoVehiculo"];



            var sb = new StringBuilder();

            var whereClause = string.Empty;
            if (idusuario != null && idusuario != "0")
            {
                sb.Append(" Where Documento = ");
                sb.Append("'" + idusuario + "'");
                whereClause = sb.ToString();
            }
            sb.Clear();
            if (idmodulo != null && idmodulo != "0")
            {
                if (whereClause == string.Empty)
                {
                    sb.Append(" Where idmodulo = ");
                }
                else
                {
                    sb.Append(" and idmodulo = ");
                }
                sb.Append("'" + idmodulo + "'");
                whereClause += sb.ToString();
            }
            sb.Clear();
            if (idsede != null && idsede != "0")
            {
                if (whereClause == string.Empty)
                {
                    sb.Append(" Where s.idsede = ");
                }
                else
                {
                    sb.Append(" and s.idsede = ");
                }
                sb.Append("'" + idsede + "'");
                whereClause += sb.ToString();
            }
            sb.Clear();
            if (idestacionamiento != null && idestacionamiento != "0")
            {
                if (whereClause == string.Empty)
                {
                    sb.Append(" Where e.idestacionamiento = ");
                }
                else
                {
                    sb.Append(" and e.idestacionamiento = ");
                }
                sb.Append("'" + idestacionamiento + "'");
                whereClause += sb.ToString();
            }
            sb.Clear();
            if (fechaAntesFin != null && fechaDespuesFin != null)
            {
                if (whereClause == string.Empty)
                {
                    sb.Append(" Where FechaFin between ");
                }
                else
                {
                    sb.Append(" and FechaFin between ");
                }
                sb.Append("convert(datetime,'" + fechaAntesFin + "',103) and convert(datetime,'" + fechaDespuesFin + "',103)");
                whereClause += sb.ToString();
            }
            sb.Clear();
            if (fechaAntesCortesia != null && fechaDespuesCortesia != null)
            {
                if (whereClause == string.Empty)
                {
                    sb.Append(" Where FechaCortesia between ");
                }
                else
                {
                    sb.Append(" and FechaCortesia between ");
                }
                sb.Append("convert(datetime,'" + fechaAntesCortesia + "',103) and convert(datetime,'" + fechaDespuesCortesia + "',103)");
                whereClause += sb.ToString();
            }
            sb.Clear();
            if (idMotivo != null && idMotivo != "0")
            {
                if (whereClause == string.Empty)
                {
                    sb.Append(" Where mc.idMotivo = ");
                }
                else
                {
                    sb.Append(" and mc.idmotivo = ");
                }
                sb.Append("'" + idMotivo + "'");
                whereClause += sb.ToString();
            }
            sb.Clear();
            if (fechaSola != null && fechaSola != "0")
            {
                if (whereClause == string.Empty)
                {
                    sb.Append(" Where fechapago BETWEEN DATEADD(dd, DATEDIFF(dd,0,convert(datetime,'" + fechaSola + "',103)), 0) AND DATEADD(ss,-1,DATEADD(dd, DATEDIFF(dd,0,convert(datetime,'" + fechaSola + "',103)), 1))");
                }
                else
                {
                    sb.Append(" and fechapago BETWEEN DATEADD(dd, DATEDIFF(dd,0,convert(datetime,'" + fechaSola + "',103)), 0) AND DATEADD(ss,-1,DATEADD(dd, DATEDIFF(dd,0,convert(datetime,'" + fechaSola + "',103)), 1))");
                }
                whereClause += sb.ToString();
            }
            sb.Clear();
            if (fechaPagoAntes != null && fechaPagoDespues != null)
            {
                if (whereClause == string.Empty)
                {
                    sb.Append(" Where p.FechaPago between ");
                }
                else
                {
                    sb.Append(" and p.FechaPago between ");
                }
                sb.Append("convert(datetime,'" + fechaPagoAntes + "',103) and convert(datetime,'" + fechaPagoDespues + "',103)");
                whereClause += sb.ToString();
            }
            sb.Clear();
            if (tipoVehiculo != null && tipoVehiculo != "0")
            {
                if (whereClause == string.Empty)
                {
                    sb.Append(" Where tv.idTipoVehiculo = ");
                }
                else
                {
                    sb.Append(" and tv.idTipoVehiculo = ");
                }
                sb.Append("'" + tipoVehiculo + "'");
                whereClause += sb.ToString();
            }
            sb.Clear();


            var filteredWhere = string.Empty;

            //ORDERING

            sb.Clear();

            string orderByClause = string.Empty;

            if (columnas == "DATEPART(DD,p.FechaPago) AS Dia,SUM(p.Total) as ValorRecaudo")
            {
                orderByClause = "order by dia asc";
            }
            else if (columnas == "DATEPART(MM,p.FechaPago) AS Mes, DATEPART(DD,p.FechaPago) AS Dia,SUM(p.Total) as ValorRecaudo")
            {
                orderByClause = "ORDER BY Mes ASC, Dia ASC";
            }
            else if (columnas == "DATEPART(MM,p.FechaPago) AS Mes,isnull(tv.TipoVehiculo,'Mensualidad') as TipoVehiculo,COUNT(p.IdTransaccion) AS CantidadTransacciones")
            {
                orderByClause = "ORDER BY Mes ASC";
            }
            else
            {
                sb.Append(ToInt(HttpContext.Current.Request.Params["iSortCol_0"]));

                sb.Append(" ");

                sb.Append(HttpContext.Current.Request.Params["sSortDir_0"]);

                orderByClause = sb.ToString();

                if (!String.IsNullOrEmpty(orderByClause))
                {
                    int i = 0;
                    foreach (var item in columnas.Split(','))
                    {
                        orderByClause = orderByClause.Replace(i.ToString(), ", " + ((item.Split('.').Length > 1) ? item.Split('.')[1] : item.Split('.')[0]) + " ");
                        i++;
                    }
                    //orderByClause = orderByClause.Replace("0", ", Nombre ");
                    //orderByClause = orderByClause.Replace("1", ", ModuloEntrada ");
                    //orderByClause = orderByClause.Replace("2", ", FechaEntrada ");
                    //orderByClause = orderByClause.Replace("3", ", PlacaEntrada ");
                    //orderByClause = orderByClause.Replace("4", ", ModuloSalida ");
                    //orderByClause = orderByClause.Replace("5", ", FechaSalida ");
                    //orderByClause = orderByClause.Replace("6", ", PlacaSalida ");
                    //orderByClause = orderByClause.Replace("7", ", IdTransaccion ");
                    //orderByClause = orderByClause.Replace("8", ", IdConvenio1 ");
                    //orderByClause = orderByClause.Replace("9", ", IdConvenio2 ");
                    //orderByClause = orderByClause.Replace("10", ", IdConvenio3 ");
                    //orderByClause = orderByClause.Replace("11", ", IdTipoVehiculo ");
                    //orderByClause = orderByClause.Replace("12", ", IdEstacionamiento ");

                    orderByClause = orderByClause.Remove(0, 1);
                }
                else
                {
                    orderByClause = colum0 + " ASC";
                }
                orderByClause = "ORDER BY " + orderByClause;

            }

            sb.Clear();

            var numberOfRowsToReturn = "";
            numberOfRowsToReturn = iDisplayLength == -1 ? "TotalRows" : (iDisplayStart + iDisplayLength).ToString();

            string query = @"               
                            SELECT *
                            FROM
	                            (SELECT row_number() OVER ({0}) AS RowNumber
		                              , *
	                             FROM
		                             (SELECT (
                                                select count(*) 
                                                    from (
                                                           select " +
                                                                   columnas
                                                                   + " from "
                                                                   + from
                                                                   + @" {5} {1} ) as t1" +
                                                @") AS TotalRows
                                           , (
                                                select count(*) 
                                                    from (
                                                           select " +
                                                                   columnas
                                                                   + " from "
                                                                   + from
                                                                   + @" {4} {5} {1} ) as t1" +
                                                @") AS TotalDisplayRows	
			                               , " + columnas
                                      + @" FROM " +
                                          from + @" {4} {5} {1} ) RawResults) Results
                            WHERE
	                            RowNumber BETWEEN {2} AND {3}";


            query = String.Format(query, orderByClause, filteredWhere, iDisplayStart + 1, numberOfRowsToReturn, whereClause, groupby);

            //var connectionString = "Data Source=CECHECE-OFFLAP\\MSSQLSERVER2012;Initial Catalog=Parking;;User ID=sa;Password=3GL0B4LT12+";
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);

            try
            {
                conn.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            var DB = new SqlCommand();
            DB.Connection = conn;
            DB.CommandText = query;
            var data = DB.ExecuteReader();
            RegistrarAuditoria(TipoAccion.select, "t_reportes", query);

            var totalDisplayRecords = "";
            var totalRecords = "";
            string outputJson = string.Empty;

            var rowClass = "";
            var count = 0;

            while (data.Read())
            {
                if (totalRecords.Length == 0)
                {
                    totalRecords = data["TotalRows"].ToString();
                    totalDisplayRecords = data["TotalDisplayRows"].ToString();
                }
                sb.Append("{");
                sb.AppendFormat(@"""DT_RowId"": ""{0}""", count++);
                sb.Append(",");
                sb.AppendFormat(@"""DT_RowClass"": ""{0}""", rowClass);

                if (columnas == "DATEPART(DD,p.FechaPago) AS Dia,SUM(p.Total) as ValorRecaudo")
                {

                    sb.Append(",");
                    sb.AppendFormat(@"""0"": ""{0}""", data["Dia"]);
                    sb.Append(",");
                    sb.AppendFormat(@"""1"": ""{0}""", data["ValorRecaudo"]);
                }
                else if (columnas == "DATEPART(MM,p.FechaPago) AS Mes, DATEPART(DD,p.FechaPago) AS Dia,SUM(p.Total) as ValorRecaudo")
                {
                    sb.Append(",");
                    sb.AppendFormat(@"""0"": ""{0}""", data["Mes"]);
                    sb.Append(",");
                    sb.AppendFormat(@"""1"": ""{0}""", data["Dia"]);
                    sb.Append(",");
                    sb.AppendFormat(@"""2"": ""{0}""", data["ValorRecaudo"]);
                }
                else if (columnas == "p.IdTransaccion,isnull(e.Nombre,'PARQUEARSE AMB') as Estacionamiento,isnull(tv.TipoVehiculo,'Mensualidad') as TipoVehiculo,tp.TipoPago,p.FechaPago,p.Subtotal,p.Iva,p.Total,p.NumeroFactura")
                {
                    sb.Append(",");
                    sb.AppendFormat(@"""0"": ""{0}""", data["IdTransaccion"]);
                    sb.Append(",");
                    sb.AppendFormat(@"""1"": ""{0}""", data["Estacionamiento"]);
                    sb.Append(",");
                    sb.AppendFormat(@"""2"": ""{0}""", data["TipoVehiculo"]);
                    sb.Append(",");
                    sb.AppendFormat(@"""3"": ""{0}""", data["TipoPago"]);
                    sb.Append(",");
                    sb.AppendFormat(@"""4"": ""{0}""", data["FechaPago"]);
                    sb.Append(",");
                    sb.AppendFormat(@"""5"": ""{0}""", data["Subtotal"]);
                    sb.Append(",");
                    sb.AppendFormat(@"""6"": ""{0}""", data["Iva"]);
                    sb.Append(",");
                    sb.AppendFormat(@"""7"": ""{0}""", data["Total"]);
                    sb.Append(",");
                    sb.AppendFormat(@"""8"": ""{0}""", data["NumeroFactura"]);
                }
                else if (columnas == "DATEPART(MM,p.FechaPago) AS Mes,isnull(tv.TipoVehiculo,'Mensualidad') as TipoVehiculo,COUNT(p.IdTransaccion) AS CantidadTransacciones")
                {
                    sb.Append(",");
                    sb.AppendFormat(@"""0"": ""{0}""", data["Mes"]);
                    sb.Append(",");
                    sb.AppendFormat(@"""1"": ""{0}""", data["TipoVehiculo"]);
                    sb.Append(",");
                    sb.AppendFormat(@"""2"": ""{0}""", data["CantidadTransacciones"]);
                }
                else
                {
                    int i = 0;
                    foreach (var item in columnas.Split(','))
                    {
                        sb.Append(",");
                        sb.AppendFormat(@"""" + i.ToString() + @""": ""{0}""", data[((item.Split('.').Length > 1) ? item.Split('.')[1] : item.Split('.')[0])]);
                        i++;
                    }
                }

                //sb.Append(",");
                //sb.AppendFormat(@"""0"": ""{0}""", data["Nombre"]);
                //sb.Append(",");
                //sb.AppendFormat(@"""1"": ""{0}""", data["ModuloEntrada"]);
                //sb.Append(",");
                //sb.AppendFormat(@"""2"": ""{0}""", data["FechaEntrada"]);
                //sb.Append(",");
                //sb.AppendFormat(@"""3"": ""{0}""", data["PlacaEntrada"]);
                //sb.Append(",");
                //sb.AppendFormat(@"""4"": ""{0}""", data["ModuloSalida"]);
                //sb.Append(",");
                //sb.AppendFormat(@"""5"": ""{0}""", data["FechaSalida"]);
                //sb.Append(",");
                //sb.AppendFormat(@"""6"": ""{0}""", data["PlacaSalida"]);
                //sb.Append(",");
                //sb.AppendFormat(@"""7"": ""{0}""", data["IdTransaccion"]);
                //sb.Append(",");
                //sb.AppendFormat(@"""8"": ""{0}""", data["IdConvenio1"]);
                //sb.Append(",");
                //sb.AppendFormat(@"""9"": ""{0}""", data["IdConvenio2"]);
                //sb.Append(",");
                //sb.AppendFormat(@"""10"": ""{0}""", data["IdConvenio3"]);
                //sb.Append(",");
                //sb.AppendFormat(@"""11"": ""{0}""", data["IdTipoVehiculo"]);
                //sb.Append(",");
                //sb.AppendFormat(@"""12"": ""{0}""", data["IdEstacionamiento"]);

                sb.Append("},");
            }

            // handles zero records
            if (totalRecords.Length == 0)
            {
                sb.Append("{");
                sb.Append(@"""sEcho"": ");
                sb.AppendFormat(@"""{0}""", sEcho);
                sb.Append(",");
                sb.Append(@"""iTotalRecords"": 0");
                sb.Append(",");
                sb.Append(@"""iTotalDisplayRecords"": 0");
                sb.Append(", ");
                sb.Append(@"""aaData"": [ ");
                sb.Append("]}");
                outputJson = sb.ToString();

                return outputJson;
            }
            outputJson = sb.Remove(sb.Length - 1, 1).ToString();
            sb.Clear();

            sb.Append("{");
            sb.Append(@"""sEcho"": ");
            sb.AppendFormat(@"""{0}""", sEcho);
            sb.Append(",");
            sb.Append(@"""iTotalRecords"": ");
            sb.Append(totalRecords);
            sb.Append(",");
            sb.Append(@"""iTotalDisplayRecords"": ");
            sb.Append(totalDisplayRecords);
            sb.Append(", ");
            sb.Append(@"""aaData"": [ ");
            sb.Append(outputJson);
            sb.Append("]}");
            outputJson = sb.ToString();

            return outputJson;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string ObtenerDatosInformeIngresosPP()
        {
            ArrayList array = new ArrayList();
            string response = string.Empty;
            DataBaseResponse oDataBaseResponse = new DataBaseResponse();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;

            DateTime fManana = DateTime.Now.AddDays(1);
            DateTime fAnteAyer = DateTime.Now.AddDays(-3);

            query = "select count(*) as Cantidad, DATEPART(HOUR, FechaEntrada)  as Hora, month(FechaEntrada) as Mes, day(FechaEntrada) as Dia " +
                    "from T_Transacciones " +
                    "where FechaEntrada between '" + fAnteAyer.ToString("yyyy-MM-dd") + "' and '" + fManana.ToString("yyyy-MM-dd") + "' " +
                    "group by DATEPART(HOUR, FechaEntrada), DATEPART(HOUR, FechaEntrada), month(FechaEntrada), day(FechaEntrada)";


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
                            RegistrarAuditoria(TipoAccion.select, "t_transacciones", query);
                            // Read advances to the next row.
                            while (reader.Read())
                            {
                                array.Add(new
                                {
                                    Cantidad = Convert.ToInt32(reader[0].ToString()),
                                    Hora = Convert.ToInt32(reader[1].ToString()),
                                    Mes = Convert.ToInt32(0),
                                    Dia = Convert.ToInt32(reader[3].ToString()),
                                });
                            }
                        }
                    }
                }
            }

            int hoy = Convert.ToInt32(DateTime.Now.ToString("dd"));
            int ayer = Convert.ToInt32(DateTime.Now.AddDays(-1).ToString("dd"));
            int anteayer = Convert.ToInt32(DateTime.Now.AddDays(-2).ToString("dd"));

            ArrayList listaHoy = new ArrayList();
            ArrayList listaAyer = new ArrayList();
            ArrayList listaAnteayer = new ArrayList();


            foreach (dynamic item in array)
            {
                if (item.Dia == hoy)
                {
                    listaHoy.Add(new
                    {
                        Cantidad = Convert.ToInt32(item.Cantidad),
                        Hora = Convert.ToInt32(item.Hora),
                        Mes = Convert.ToInt32(item.Mes),
                        Dia = Convert.ToInt32(item.Dia),
                    });
                }
                else if (item.Dia == ayer)
                {
                    listaAyer.Add(new
                    {
                        Cantidad = Convert.ToInt32(item.Cantidad),
                        Hora = Convert.ToInt32(item.Hora),
                        Mes = Convert.ToInt32(item.Mes),
                        Dia = Convert.ToInt32(item.Dia),
                    });
                }
                else if (item.Dia == anteayer)
                {
                    listaAnteayer.Add(new
                    {
                        Cantidad = Convert.ToInt32(item.Cantidad),
                        Hora = Convert.ToInt32(item.Hora),
                        Mes = Convert.ToInt32(item.Mes),
                        Dia = Convert.ToInt32(item.Dia),
                    });
                }
            }

            bool find = false;
            for (int i = 7; i < 23; i++)
            {
                foreach (dynamic item in listaHoy)
                {
                    if (item.Hora == i)
                    {
                        find = true;
                    }
                }

                if (!find)
                {
                    listaHoy.Insert(i - 7, new
                    {
                        Cantidad = Convert.ToInt32(0),
                        Hora = Convert.ToInt32(i),
                        Mes = Convert.ToInt32(0),
                        Dia = Convert.ToInt32(hoy),
                    });
                }

                find = false;
            }

            find = false;
            for (int i = 7; i < 23; i++)
            {
                foreach (dynamic item in listaAyer)
                {
                    if (item.Hora == i)
                    {
                        find = true;
                    }
                }

                if (!find)
                {
                    listaAyer.Insert(i - 7, new
                    {
                        Cantidad = Convert.ToInt32(0),
                        Hora = Convert.ToInt32(i),
                        Mes = Convert.ToInt32(0),
                        Dia = Convert.ToInt32(ayer),
                    });
                }

                find = false;
            }


            find = false;
            for (int i = 7; i < 23; i++)
            {
                foreach (dynamic item in listaAnteayer)
                {
                    if (item.Hora == i)
                    {
                        find = true;
                    }
                }

                if (!find)
                {
                    listaAnteayer.Insert(i - 7, new
                    {
                        Cantidad = Convert.ToInt32(0),
                        Hora = Convert.ToInt32(i),
                        Mes = Convert.ToInt32(0),
                        Dia = Convert.ToInt32(ayer),
                    });
                }

                find = false;
            }

            int totalListaHoy = 0;
            ArrayList ListaFinalHoy = new ArrayList();
            foreach (dynamic item in listaHoy)
            {
                totalListaHoy += item.Cantidad;
                ListaFinalHoy.Add(totalListaHoy);
            }

            int totalListaAyer = 0;
            ArrayList ListaFinalAyer = new ArrayList();
            foreach (dynamic item in listaAyer)
            {
                totalListaAyer += item.Cantidad;
                ListaFinalAyer.Add(totalListaAyer);
            }

            int totalListaAnteayer = 0;
            ArrayList ListaFinalAnteayer = new ArrayList();
            foreach (dynamic item in listaAnteayer)
            {
                totalListaAnteayer += item.Cantidad;
                ListaFinalAnteayer.Add(totalListaAnteayer);
            }

            Object final = new
            {
                Hoy = ListaFinalHoy,
                Ayer = ListaFinalAyer,
                Anteayer = ListaFinalAnteayer
            };


            if (final != null)
            {
                oDataBaseResponse.Resultado = final;
            }
            else
            {
                oDataBaseResponse.Exito = false;
                oDataBaseResponse.ErrorMessage = "No encuentra informacion de transacciones.";
            }

            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            response = javaScriptSerializer.Serialize(oDataBaseResponse);

            return response;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string ObtenerDatosInformeIngresosDineroPP()
        {
            ArrayList array = new ArrayList();
            string response = string.Empty;
            DataBaseResponse oDataBaseResponse = new DataBaseResponse();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

            string query = string.Empty;

            DateTime fManana = DateTime.Now.AddDays(1);
            DateTime fAnteAyer = DateTime.Now.AddDays(-3);

            query = "select CASE WHEN sum(ValorRecibido) is null THEN 0 ELSE sum(ValorRecibido) END as Valor, DATEPART(HOUR, FechaEntrada)  as Hora, month(FechaEntrada) as Mes, day(FechaEntrada) as Dia " +
                    "from T_Transacciones " +
                    "where FechaEntrada between '" + fAnteAyer.ToString("yyyy-MM-dd") + "' and '" + fManana.ToString("yyyy-MM-dd") + "' " +
                    "group by DATEPART(HOUR, FechaEntrada), DATEPART(HOUR, FechaEntrada), month(FechaEntrada), day(FechaEntrada)";


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
                            RegistrarAuditoria(TipoAccion.select, "t_transacciones", query);
                            // Read advances to the next row.
                            while (reader.Read())
                            {
                                array.Add(new
                                {
                                    Cantidad = Convert.ToInt32(reader[0].ToString()),
                                    Hora = Convert.ToInt32(reader[1].ToString()),
                                    Mes = Convert.ToInt32(0),
                                    Dia = Convert.ToInt32(reader[3].ToString()),
                                });
                            }
                        }
                    }
                }
            }

            int hoy = Convert.ToInt32(DateTime.Now.ToString("dd"));
            int ayer = Convert.ToInt32(DateTime.Now.AddDays(-1).ToString("dd"));
            int anteayer = Convert.ToInt32(DateTime.Now.AddDays(-2).ToString("dd"));

            ArrayList listaHoy = new ArrayList();
            ArrayList listaAyer = new ArrayList();
            ArrayList listaAnteayer = new ArrayList();


            foreach (dynamic item in array)
            {
                if (item.Dia == hoy)
                {
                    listaHoy.Add(new
                    {
                        Cantidad = Convert.ToInt32(item.Cantidad),
                        Hora = Convert.ToInt32(item.Hora),
                        Mes = Convert.ToInt32(item.Mes),
                        Dia = Convert.ToInt32(item.Dia),
                    });
                }
                else if (item.Dia == ayer)
                {
                    listaAyer.Add(new
                    {
                        Cantidad = Convert.ToInt32(item.Cantidad),
                        Hora = Convert.ToInt32(item.Hora),
                        Mes = Convert.ToInt32(item.Mes),
                        Dia = Convert.ToInt32(item.Dia),
                    });
                }
                else if (item.Dia == anteayer)
                {
                    listaAnteayer.Add(new
                    {
                        Cantidad = Convert.ToInt32(item.Cantidad),
                        Hora = Convert.ToInt32(item.Hora),
                        Mes = Convert.ToInt32(item.Mes),
                        Dia = Convert.ToInt32(item.Dia),
                    });
                }
            }

            bool find = false;
            for (int i = 7; i < 23; i++)
            {
                foreach (dynamic item in listaHoy)
                {
                    if (item.Hora == i)
                    {
                        find = true;
                    }
                }

                if (!find)
                {
                    listaHoy.Insert(i - 7, new
                    {
                        Cantidad = Convert.ToInt32(0),
                        Hora = Convert.ToInt32(i),
                        Mes = Convert.ToInt32(0),
                        Dia = Convert.ToInt32(hoy),
                    });
                }

                find = false;
            }

            find = false;
            for (int i = 7; i < 23; i++)
            {
                foreach (dynamic item in listaAyer)
                {
                    if (item.Hora == i)
                    {
                        find = true;
                    }
                }

                if (!find)
                {
                    listaAyer.Insert(i - 7, new
                    {
                        Cantidad = Convert.ToInt32(0),
                        Hora = Convert.ToInt32(i),
                        Mes = Convert.ToInt32(0),
                        Dia = Convert.ToInt32(ayer),
                    });
                }

                find = false;
            }


            find = false;
            for (int i = 7; i < 23; i++)
            {
                foreach (dynamic item in listaAnteayer)
                {
                    if (item.Hora == i)
                    {
                        find = true;
                    }
                }

                if (!find)
                {
                    listaAnteayer.Insert(i - 7, new
                    {
                        Cantidad = Convert.ToInt32(0),
                        Hora = Convert.ToInt32(i),
                        Mes = Convert.ToInt32(0),
                        Dia = Convert.ToInt32(ayer),
                    });
                }

                find = false;
            }

            int totalListaHoy = 0;
            ArrayList ListaFinalHoy = new ArrayList();
            foreach (dynamic item in listaHoy)
            {
                totalListaHoy += item.Cantidad;
                ListaFinalHoy.Add(totalListaHoy);
            }

            int totalListaAyer = 0;
            ArrayList ListaFinalAyer = new ArrayList();
            foreach (dynamic item in listaAyer)
            {
                totalListaAyer += item.Cantidad;
                ListaFinalAyer.Add(totalListaAyer);
            }

            int totalListaAnteayer = 0;
            ArrayList ListaFinalAnteayer = new ArrayList();
            foreach (dynamic item in listaAnteayer)
            {
                totalListaAnteayer += item.Cantidad;
                ListaFinalAnteayer.Add(totalListaAnteayer);
            }

            Object final = new
            {
                Hoy = ListaFinalHoy,
                Ayer = ListaFinalAyer,
                Anteayer = ListaFinalAnteayer
            };


            if (final != null)
            {
                oDataBaseResponse.Resultado = final;
            }
            else
            {
                oDataBaseResponse.Exito = false;
                oDataBaseResponse.ErrorMessage = "No encuentra informacion de transacciones.";
            }

            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            response = javaScriptSerializer.Serialize(oDataBaseResponse);

            return response;
        }


        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string ObtenerDatosInformeCuposDisponibles()
        {
            ArrayList array = new ArrayList();
            string response = string.Empty;
            DataBaseResponse oDataBaseResponse = new DataBaseResponse();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            int respuesta = -1;

            string query = string.Empty;

            //query = "select " +
            //        "(select SUM(CAST(valor AS INT)) " +
            //        "from T_Parametros " +
            //        "where codigo='totalCuposEstacionamiento') " +
            //        "- " +
            //        "(select count(*) " +
            //        "from T_Transacciones " +
            //        "where FechaSalida is null)";

            query = "select " +
                    "(select SUM(CAST(valor AS INT)) " +
                    "from T_Parametros " +
                    "where codigo='totalCuposEstacionamiento') " +
                    "- " +
                    "(select count(*) " +
                    "from T_Transacciones " +
                    "where FechaSalida is null and (FechaEntrada BETWEEN getdate() AND getdate()-1))";


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
                            RegistrarAuditoria(TipoAccion.select, "t_parametros", query);
                            // Read advances to the next row.
                            while (reader.Read())
                            {
                                respuesta = Convert.ToInt32(reader[0].ToString());
                            }
                        }
                    }
                }
            }

            if (respuesta != -1)
            {
                oDataBaseResponse.Resultado = respuesta;
            }
            else
            {
                oDataBaseResponse.Resultado = 0;
                //oDataBaseResponse.ErrorMessage = "No se puede obtener infromacion de cupos disponibles";
                //oDataBaseResponse.Exito = false;
            }

            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            response = javaScriptSerializer.Serialize(oDataBaseResponse);

            return response;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string ObtenerDatosInformeRecaudoDia()
        {
            ArrayList array = new ArrayList();
            string response = string.Empty;
            DataBaseResponse oDataBaseResponse = new DataBaseResponse();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            int respuesta = -1;

            string query = string.Empty;

            query = "select sum(ValorRecibido) " +
                    "from T_Transacciones " +
                    "where day(FechaEntrada)=day(getdate()) " +
                    "and month(FechaEntrada)=month(getdate()) " +
                    "and year(FechaEntrada)=year(getdate())";


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
                            RegistrarAuditoria(TipoAccion.select, "t_transacciones", query);
                            // Read advances to the next row.
                            while (reader.Read())
                            {
                                string temporal = reader[0].ToString();
                                if (temporal != string.Empty)
                                {
                                    respuesta = Convert.ToInt32(temporal);
                                }
                            }
                        }
                    }
                }
            }

            if (respuesta != -1)
            {
                oDataBaseResponse.Resultado = respuesta;
            }
            else
            {
                oDataBaseResponse.Resultado = 0;
                //oDataBaseResponse.ErrorMessage = "No se puede obtener informacion de recaudo diario";
                //oDataBaseResponse.Exito = false;
            }

            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            response = javaScriptSerializer.Serialize(oDataBaseResponse);

            return response;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string ObtenerDatosAlarmasDia()
        {
            ArrayList array = new ArrayList();
            string response = string.Empty;
            DataBaseResponse oDataBaseResponse = new DataBaseResponse();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            int respuesta = -1;

            string query = string.Empty;

            query = "select count(*) " +
                    "from t_alarmas " +
                    "where day(FechaRegistro)=day(getdate()) " +
                    "and month(FechaRegistro)=month(getdate()) " +
                    "and year(FechaRegistro)=year(getdate())";


            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        RegistrarAuditoria(TipoAccion.select, "t_alarmas", query);
                        // Check is the reader has any rows at all before starting to read.
                        if (reader.HasRows)
                        {
                            // Read advances to the next row.
                            while (reader.Read())
                            {
                                respuesta = Convert.ToInt32(reader[0].ToString());
                            }
                        }
                    }
                }
            }

            if (respuesta != -1)
            {
                oDataBaseResponse.Resultado = respuesta;
            }
            else
            {
                oDataBaseResponse.ErrorMessage = "No se puede obtener informacion de alarmas diario";
                oDataBaseResponse.Exito = false;
            }

            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            response = javaScriptSerializer.Serialize(oDataBaseResponse);

            return response;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string ConsultarValorPagar()
        {
            string mensualidad = HttpContext.Current.Request.Params["mensualidad"];
            string repo = HttpContext.Current.Request.Params["repo"];
            string tipoVehiculo = HttpContext.Current.Request.Params["tipoVehiculo"];
            string idTransaccion = HttpContext.Current.Request.Params["idTransaccion"];
            string idTarjeta = HttpContext.Current.Request.Params["idTarjeta"];

            ArrayList array = new ArrayList();
            string response = string.Empty;
            DataBaseResponse oDataBaseResponse = new DataBaseResponse();

            try
            {

                string url = ConfigurationManager.AppSettings["URLLiquidar"].ToString();

                LiquidacionServiceClient miCLiente = new LiquidacionServiceClient();
                miCLiente.Endpoint.Address = new EndpointAddress(url);


                Liquidacion_Request request = new Liquidacion_Request();
                request.bMensualidad = Convert.ToBoolean(mensualidad);
                request.bReposicion = Convert.ToBoolean(repo); ;
                request.iTipoVehiculo = Convert.ToInt32(tipoVehiculo);
                request.sSecuencia = idTransaccion;
                request.sIdtarjeta = idTarjeta;

                Liquidacion_Response responseWS = miCLiente.getDatosLiquidacion(request);
                if (responseWS.olstDtoLiquidacion != null)
                {
                    foreach (ServiceDtoDatosLiquidacion item in responseWS.olstDtoLiquidacion)
                    {
                        array.Add(
                        new
                        {
                            Tipo = item.Tipo,
                            SubTotal = item.SubTotal,
                            Iva = item.Iva,
                            Total = item.Total
                        });
                    }

                    //array.Add(
                    //    new { Tipo = 1, SubTotal = 1640, Iva = 360, Total = 2000 }
                    //);

                    oDataBaseResponse.Exito = true;
                    oDataBaseResponse.Resultado = array;
                }
                else
                {
                    oDataBaseResponse.Exito = false;
                    oDataBaseResponse.ErrorMessage = "No se encontraron items de liquidacion";
                }
            }
            catch (Exception e)
            {
                oDataBaseResponse.Exito = false;
                oDataBaseResponse.ErrorMessage = e.InnerException + " " + e.Message;
            }

            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            response = javaScriptSerializer.Serialize(oDataBaseResponse);

            return response;
        }

        [WebMethod]
        [ScriptMethod(UseHttpGet = true)]
        public string RegistrarAuditoriaRemoto()
        {
            string nombreReporte = HttpContext.Current.Request.Params["nombreReporte"];
            string tipo = HttpContext.Current.Request.Params["tipo"];

            string response = string.Empty;
            DataBaseResponse oDataBaseResponse = new DataBaseResponse();
            var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;


            RegistrarAuditoria(TipoAccion.select, nombreReporte, tipo);

            var javaScriptSerializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            response = javaScriptSerializer.Serialize(oDataBaseResponse);

            return response;
        }

        private string Encrypt(string clearText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        private string Decrypt(string cipherText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }

        private bool RegistrarAuditoria(TipoAccion eTipoAccion, string sTable, string sQuery)
        {
            bool bResponse = false;

            if (eTipoAccion == TipoAccion.select && sQuery != "Reporte")
            {
                bResponse = true;
            }
            else
            {
                var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

                string user = HttpContext.Current.User.Identity.Name.ToString();

                string query = string.Empty;
                try
                {
                    query = "insert into T_Auditoria(Documento,Accion,Tabla, Fecha, Query) " +
                            "values ('" + user + "','" + eTipoAccion.ToString() + "','" + sTable + "',getdate(), '" + sQuery.Replace("'", "''") + "')";

                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        using (SqlCommand cmd = new SqlCommand(query, connection))
                        {
                            connection.Open();

                            int result2 = Convert.ToInt32(cmd.ExecuteScalar());

                            if (result2 != 0)
                            {
                                bResponse = true;
                            }
                            else
                            {
                                bResponse = false;
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    bResponse = false;
                }
            }

            return bResponse;
        }

    }

    public enum TipoAccion
    {
        insert,
        select,
        update,
        delete,
    }

    public class DataBaseResponse
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

        private Object _Resultado;

        public Object Resultado
        {
            get { return _Resultado; }
            set { _Resultado = value; }
        }
    }

    public class DatosInterfazContable
    {
        public string empresa { get; set; }
        public string documento { get; set; }
        public string numero { get; set; }
        public string fecha { get; set; }
        public string item { get; set; }
        public string concepto { get; set; }
        public string tercero { get; set; }
        public string terceroconsecutivo { get; set; }
        public string cuentalocal { get; set; }
        public string proyecto { get; set; }
        public string naturaleza { get; set; }
        public string banco { get; set; }
        public string cuentabancaria { get; set; }
        public string centro { get; set; }
        public string valor { get; set; }
        public string referencia { get; set; }
    }
}
