using BlockAndPass.Utilidades;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlockAndPass.IntegracionContableDemon
{
    public partial class frmIntegracionContableDemon : Form
    {
        
        #region ConfigParams
        private int _PeriodoEjecucionSegundos
        {
            get
            {
                string sPeriodoEjecucionSegundos = ConfigurationManager.AppSettings["PeriodoEjecucionSegundos"];
                if (string.IsNullOrEmpty(sPeriodoEjecucionSegundos))
                {
                    return 10;
                }
                else
                {
                    return Convert.ToInt32(sPeriodoEjecucionSegundos);
                }
            }
        }
        private int _UpdateTimeBegin
        {
            get
            {
                string sSerial = ConfigurationManager.AppSettings["HourUpdateTimeBegin"];
                if (string.IsNullOrEmpty(sSerial))
                {
                    return 4;
                }
                else
                {
                    return Convert.ToInt32(sSerial);
                }
            }
        }
        private string _LogFileUpdate
        {
            get
            {
                string sSerial = ConfigurationManager.AppSettings["LogUpdateFileName"];
                if (string.IsNullOrEmpty(sSerial))
                {
                    return "updateLogFile.txt";
                }
                else
                {
                    return sSerial;
                }
            }
        }
        private string _ConnectionStringSql
        {
            get
            {
                string sSerial = ConfigurationManager.AppSettings["UrlSQL"];
                if (string.IsNullOrEmpty(sSerial))
                {
                    return "Data Source=169.47.207.158;Initial Catalog=Parking;User ID=AdminParkingUser;Password=P4rqu1ng+";
                }
                else
                {
                    return sSerial;
                }
            }
        }
        private string _ConnectionStringFirebird
        {
            get
            {
                string sSerial = ConfigurationManager.AppSettings["UrlFireBird"];
                if (string.IsNullOrEmpty(sSerial))
                {
                    return "User ID=SYSDBA;Password=masterkey;Database=C://magister/datos/magisterz.mgt;DataSource=localhost;Charset=NONE;";
                }
                else
                {
                    return sSerial;
                }
            }
        }
        private bool _TestMode
        {
            get
            {
                string sSerial = ConfigurationManager.AppSettings["TestMode"];
                if (string.IsNullOrEmpty(sSerial))
                {
                    return true;
                }
                else
                {
                    return Convert.ToBoolean(sSerial);
                }
            }
        }
        private DateTime _TestModeValue
        {
            get
            {
                string sSerial = ConfigurationManager.AppSettings["TestModeValue"];
                if (string.IsNullOrEmpty(sSerial))
                {
                    return DateTime.Now.AddDays(-1);
                }
                else
                {
                    return new DateTime(Convert.ToInt32(sSerial.Split('/')[2]), Convert.ToInt32(sSerial.Split('/')[1]), Convert.ToInt32(sSerial.Split('/')[0]));
                }
            }
        }
        #endregion

        #region Timer
        private System.Timers.Timer oTimer;
        private static object objLock = new object();
        void oTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            oTimer.Enabled = false;

            MensajeAListBox("El servicio inicia integracion");

            try
            {
                lock (objLock)
                {
                    if (_UpdateTimeBegin == DateTime.Now.Hour)
                    {
                        MensajeAListBox("El servicio se encuentra en hora de integracion");

                        for (int day = 1; day < DateTime.Now.Day; day++)
                        {
                            DateTime dtFechaSubir = new DateTime(DateTime.Now.Year, DateTime.Now.Month, day);
                            MetodoCompleto(dtFechaSubir);
                        }

                        //DateTime dtFechaSubir = DateTime.Now.AddDays(-1);
                        //MetodoCompleto(dtFechaSubir);
                    }
                    else
                    {
                        MensajeAListBox("El servicio NO se encuentra en hora de integracion");
                    }
                }
            }
            catch (Exception ex)
            {
                MensajeAListBox("Excepcion: " + ex.InnerException + " " + ex.Message + " " + ex.Source);
                oTimer.Enabled = true;
            }
            
            oTimer.Enabled = true;
        }
        private void MetodoCompleto(DateTime dtFechaSubir)
        {
            MensajeAListBox("Obteniendo estacionamientos de integracion en la fecha = " + dtFechaSubir.ToString("dd-MM-yyyy"));
            List<EmpresaParquearse> lstIdEstacionamientos = ObtenerEstacionamientosByP();

            if (lstIdEstacionamientos != null && lstIdEstacionamientos.Count > 0)
            {
                MensajeAListBox("Obtiene estacionamientos de integracion");

                foreach (var item in lstIdEstacionamientos)
                {
                    RspEstadoFirebird resp = LookExistingRegisterFirebird(dtFechaSubir, item.IdEstacionamiento, item.Idc_Empresa, item.Idc_Documento);
                    if (resp == RspEstadoFirebird.Falso)
                    {
                        DataTable dtUnion = ConsultarQueryFromFile(item.IdEstacionamiento, dtFechaSubir);
                        if (dtUnion != null && dtUnion.Rows.Count > 0)
                        {
                            int consecutivo = 1;
                            
                            if (WriteIntoFirebird(dtUnion, consecutivo, item.IdEstacionamiento))
                            {
                                MensajeAListBox("Finaliza escritura con estacionamiento = " + item.IdEstacionamiento);
                            }
                            else
                            {
                                MensajeAListBox("Falla escritura con estacionamiento = " + item.IdEstacionamiento);
                            }
                        }
                        else
                        {
                            MensajeAListBox("No Obtiene query con estacionamiento o viene sin filas = " + item.IdEstacionamiento);
                        }
                    }
                    else if (resp == RspEstadoFirebird.ErrorConexion)
                    {
                        MensajeAListBox("No continua por error conexion en BD en fecha = " + dtFechaSubir.ToString("dd-MM-yyyy") + " en estadionamiento id = " + item.IdEstacionamiento);
                    }
                    else
                    {
                        MensajeAListBox("No continua por registro existente en fecha = " + dtFechaSubir.ToString("dd-MM-yyyy") + " en estadionamiento id = " + item.IdEstacionamiento);
                    }
                }
            }
            else
            {
                MensajeAListBox("No Obtiene estacionamientos de integracion");
            }
            //}
        }
        #endregion

        #region Firebird
        private RspEstadoFirebird LookExistingRegisterFirebird(DateTime dtFecha, int iIdEstacionamiento, int IDC_EMPRESA, string IDC_DOCUMENTO)
        {
            RspEstadoFirebird respuesta = RspEstadoFirebird.Falso;

            double MyDouble = dtFecha.ToOADate();
            FbConnection fbCon = new FbConnection(_ConnectionStringFirebird);
            try
            {
                fbCon.Open();

                FbCommand readCommand =
                  new FbCommand("Select * From ITEMSDOCCONTABLE where IDC_EMPRESA = " + IDC_EMPRESA + " and IDC_FECHA = '" + MyDouble + "' and IDC_DOCUMENTO = '" + IDC_DOCUMENTO + "'", fbCon);
                FbDataReader myreader = readCommand.ExecuteReader();
                while (myreader.Read())
                {
                    if (myreader[0] is DBNull)
                    {
                        respuesta = RspEstadoFirebird.Falso;
                    }
                    else
                    {
                        respuesta = RspEstadoFirebird.Verdadero;
                    }
                }
                myreader.Close();
            }
            catch (Exception ex)
            {
                respuesta = RspEstadoFirebird.ErrorConexion;
                MensajeAListBox("Excepcion Consultando query Firebird: " + ex.InnerException + " " + ex.Message + " " + ex.Source);
            }
            finally
            {
                fbCon.Close();
            }


            return respuesta;
        }
        private int GetCurrentIndexFromFireBird()
        {
            int consecutivo = 0;
            FbConnection fbCon = new FbConnection(_ConnectionStringFirebird);
            try
            {
                fbCon.Open();

                FbCommand readCommand =
                  new FbCommand("Select MAX(IDC_ITEM) From ITEMSDOCCONTABLE", fbCon);
                FbDataReader myreader = readCommand.ExecuteReader();
                while (myreader.Read())
                {
                    if (myreader[0] is DBNull)
                    {
                        consecutivo = 0;
                    }
                    else
                    {
                        consecutivo = Convert.ToInt32(myreader[0]);
                    }
                }
                myreader.Close();
            }
            catch (Exception ex)
            {
                MensajeAListBox("Excepcion Consultando query Firebird: " + ex.InnerException + " " + ex.Message + " " + ex.Source);
            }
            finally
            {
                fbCon.Close();
            }
            return consecutivo;
        }
        private bool WriteIntoFirebird(DataTable dtUnion, int iConsecutivo, int iIdEstacionamiento)
        {
            bool resultado = true;

            FbConnection fbCon = new FbConnection(_ConnectionStringFirebird);
            try
            {
                fbCon.Open();
                using (var dbContextTransaction = fbCon.BeginTransaction())
                {
                    try
                    {
                        FbCommand addDetailsCommand;
                        double MyDouble = 0;
                        string numero = string.Empty;
                        int idc_empresa = 0;
                        string documentoempresa = string.Empty;
                        foreach (DataRow row in dtUnion.Rows)
                        {
                            string[] nuevaFecha = row[3].ToString().Split('/');
                            DateTime MyDate = new DateTime(Convert.ToInt32(nuevaFecha[2]), Convert.ToInt32(nuevaFecha[1]), Convert.ToInt32(nuevaFecha[0]));
                            MyDouble = MyDate.ToOADate();
                            
                            idc_empresa = Convert.ToInt32(row[0]);
                            documentoempresa = row[1].ToString();
                            numero = row[2].ToString();

                            string SQLCommandText = "INSERT into ITEMSDOCCONTABLE Values ("
                                + row[0] 
                                + ",'"
                                + row[1]
                                + "',"
                                + row[2]
                                + ",'"
                                + MyDouble
                                + "',"
                                + iConsecutivo
                                + ",'"
                                + row[5]
                                + "',"
                                + row[6]
                                + ","
                                + row[7]
                                + ","
                                + row[8]
                                + ","
                                +"NULL"
                                +",'"
                                + row[10]
                                + "',"
                                +"NULL"
                                +","
                                +"NULL"
                                +",'"
                                + row[13]
                                + "',"
                                + row[14] 
                                + ");";

                            string rutaFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Registros-" + MyDate.Day.ToString() + "-" + MyDate.Month.ToString() + "-" + MyDate.Year.ToString());
                            string path = rutaFolder + "/" + iIdEstacionamiento.ToString() + @".txt";

                            if (!Directory.Exists(rutaFolder))
                            {
                                Directory.CreateDirectory(rutaFolder);
                            }

                            using (System.IO.StreamWriter file = new System.IO.StreamWriter(path, true))
                            {
                                file.WriteLine(SQLCommandText);
                            }

                            iConsecutivo++;
                            addDetailsCommand = new FbCommand(SQLCommandText, fbCon, dbContextTransaction);
                            addDetailsCommand.ExecuteNonQuery();
                        }

                        string SQLCommandText2 = "INSERT into DOCCONTABLE Values (" + idc_empresa + ",'"
                            + documentoempresa
                            + "','"
                            + numero
                            + "','"
                            + MyDouble
                            + "',NULL,NULL,NULL,NULL,NULL);";
                        addDetailsCommand = new FbCommand(SQLCommandText2, fbCon, dbContextTransaction);
                        addDetailsCommand.ExecuteNonQuery();

                        dbContextTransaction.Commit();

                    }
                    catch (Exception ex)
                    {
                        resultado = false;
                        MensajeAListBox("Excepcion Escribiendo query Firebird: " + ex.InnerException + " " + ex.Message + " " + ex.Source);
                        dbContextTransaction.Rollback();
                    }
                }
            }
            catch (Exception x)
            {
                resultado = false;
                MensajeAListBox("Excepcion Escribiendo query Firebird: " + x.InnerException + " " + x.Message + " " + x.Source);
            }
            finally
            {
                fbCon.Close();
            }
            return resultado;
        }
        #endregion

        #region ByP
        private DataTable ConsultarQueryFromFile(int iIdEstacionamiento, DateTime dtFecha)
        {
            DataTable dtRespuesta = new DataTable();
            string query = string.Empty;

            try
            {
                using (SqlConnection connection = new SqlConnection(_ConnectionStringSql))
                {
                    using (SqlCommand cmd = new SqlCommand("P_ObtenerInformacionInterfazContable", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@IdEstacionamientoConsulta", SqlDbType.BigInt).Value = iIdEstacionamiento;
                        cmd.Parameters.Add("@FechaCosulta", SqlDbType.DateTime2).Value = dtFecha;

                        connection.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            dtRespuesta.Load(reader);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                dtRespuesta = null;
                MensajeAListBox("Excepcion Consultando query SQL: " + ex.InnerException + " " + ex.Message + " " + ex.Source);
            }

            return dtRespuesta;
        }
        private List<EmpresaParquearse> ObtenerEstacionamientosByP()
        {
            List<EmpresaParquearse> lstEmpresaParquearse = new List<EmpresaParquearse>();
            string query = "select IdEstacionamiento, Idc_Empresa, DocumentoEmpresa from T_EmpresaParquearse";
            using (SqlConnection connection = new SqlConnection(_ConnectionStringSql))
            {
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lstEmpresaParquearse.Add(new EmpresaParquearse(Convert.ToInt32(reader[0]), Convert.ToInt32(reader[1]), reader[2].ToString()));
                        }
                    }
                }
            }
            return lstEmpresaParquearse;
        }
        #endregion

        #region Formulario
        private void frmPrincipalUpdateDemon_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == this.WindowState)
            {
                mynotifyicon.ShowBalloonTip(200);
                this.Hide();
            }
        }
        private void mynotifyicon_DoubleClick(object sender, EventArgs e)
        {
            this.Show();
            this.ShowInTaskbar = true;
            this.WindowState = FormWindowState.Normal;
            this.BringToFront();
            this.Activate();
        }
        private void frmPrincipalUpdateDemon_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            mynotifyicon.ShowBalloonTip(500);
            this.Hide();
        }

        public frmIntegracionContableDemon()
        {
            CheckForIllegalCrossThreadCalls = false;
            oTimer = new System.Timers.Timer(_PeriodoEjecucionSegundos * 1000);
            oTimer.Elapsed += new System.Timers.ElapsedEventHandler(oTimer_Elapsed);
            InitializeComponent();
            MensajeAListBox("Demonio Integracion Iniciado con TestMode = " + _TestMode);
            
            if (!_TestMode)
            {
                oTimer.Enabled = true;
            }
            else
            {
                DateTime dtFechaSubir = _TestModeValue;
                MetodoCompleto(dtFechaSubir);
                this.Show();
                this.ShowInTaskbar = true;
                this.WindowState = FormWindowState.Normal;
                this.BringToFront();
                this.Activate();
                MensajeAListBox("Finaliza Proceso");
                btnStop.Enabled = false;
                btnStart.Enabled = true;
                oTimer.Enabled = false;
            }
        }
        
        private void btnStart_Click(object sender, EventArgs e)
        {
            MensajeAListBox("Demonio Integracion Iniciado");
            btnStart.Enabled = false;
            btnStop.Enabled = true;
            oTimer.Enabled = true;
        }
        private void btnStop_Click(object sender, EventArgs e)
        {
            MensajeAListBox("Demonio Integracion Detenido");
            btnStop.Enabled = false;
            btnStart.Enabled = true;
            oTimer.Enabled = false;
        }
        private void btnCerrarServicio_Click(object sender, EventArgs e)
        {
            this.Dispose();
            this.Close();
        }
        #endregion

        #region Mensajes
        private void MensajeAListBox(string mensaje)
        {
            lbEventos.Items.Add(DateTime.Now.ToString("dd/MM/yy HH:mm:ss") + " -> " + mensaje);
            this.lbEventos.SelectedIndex = this.lbEventos.Items.Count - 1;
            TraceHandler.WriteLine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Logs\Log"), "MENSAJE: " + mensaje, TipoLog.TRAZA);
        }
        #endregion

    }

    public class EmpresaParquearse
    {
        public EmpresaParquearse(int recibeEstacionamiento, int recibeIdc, string documentoEmpresa)
        {
            IdEstacionamiento = recibeEstacionamiento;
            Idc_Empresa = recibeIdc;
            Idc_Documento = documentoEmpresa;
        }

        private int _IdEstacionamiento = 0;

        public int IdEstacionamiento
        {
            get { return _IdEstacionamiento; }
            set { _IdEstacionamiento = value; }
        }

        private int _Idc_Empresa = 0;

        public int Idc_Empresa
        {
            get { return _Idc_Empresa; }
            set { _Idc_Empresa = value; }
        }

        private string _Idc_Documento = string.Empty;

        public string Idc_Documento
        {
            get { return _Idc_Documento; }
            set { _Idc_Documento = value; }
        }
    }

    public enum RspEstadoFirebird
    {
        Verdadero,
        Falso,
        ErrorConexion,
    }
}
