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
    public partial class AdministrarVisado : Form
    {
        Convenios misConvenios = new Convenios();

        string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "convenioFile.dat");

        public AdministrarVisado()
        {
            //241; 370
            //450; 370

            InitializeComponent();
            this.Width = 241;
            this.Height = 370;

            LoadData();
            
        }

        private void LoadData()
        {
            //lbConvenios.Items.Clear();


            if (File.Exists(path))
            {
                misConvenios = DeSerializeObject<Convenios>(path);
                if (misConvenios != null)
                {
                    lbConvenios.DataSource = misConvenios.ListConvenio;
                    lbConvenios.DisplayMember = "Nombre"; // Just set the correct name of the properties 
                    lbConvenios.ValueMember = "Identificador";
                }
                else
                {
                    misConvenios = new Convenios();
                }
            }
            else
            {
                SerializeObject<Convenios>(misConvenios, path);
            }
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

        private void btn_Nuevo_Click(object sender, EventArgs e)
        {

            gbNuevo.Text = "Nuevo";

            this.Width = 450;
            this.Height = 370;

            gbNuevo.Visible = true;
            tbIdentificador.ReadOnly = false;

            LimpiarCampos();

            HabilitarBotones(false);

        }

        private void LimpiarCampos()
        {
            tbIdentificador.Text = string.Empty;
            tbDescripcion.Text = string.Empty;
            tbNombre.Text = string.Empty;
            chbBolsa.Checked = false;
            upTotal.Value = 0;
            upUsadas.Value = 0;
        }

        private void HabilitarBotones(bool bActividad)
        {
            

            btn_Nuevo.Enabled = bActividad;
            btn_Editar.Enabled = bActividad;
            btn_Eliminar.Enabled = bActividad;

            lbConvenios.Enabled = bActividad;
        }

        private void chbBolsa_CheckedChanged(object sender, EventArgs e)
        {
            if (chbBolsa.Checked)
            {
                lblTotal.Visible = true;
                lblUsadas.Visible = true;
                upTotal.Visible = true;
                upUsadas.Visible = true;
            }
            else
            {
                lblTotal.Visible = false;
                lblUsadas.Visible = false;
                upTotal.Visible = false;
                upUsadas.Visible = false;
            }
        }

        private void btn_Cancelar_Click(object sender, EventArgs e)
        {
            DialogResult result1 = MessageBox.Show("Si cierra perdera los cambios \n ¿desea cerrar?", "Visado", MessageBoxButtons.YesNo);
            if (result1 == DialogResult.Yes)
            {
                this.Width = 241;
                this.Height = 370;
                HabilitarBotones(true);
                LoadData();
            }
        }

        private void btn_Guardar_Click(object sender, EventArgs e)
        {
            if (gbNuevo.Text == "Nuevo")
            {
                TipoConvenio eTipo = TipoConvenio.Normal;
                int total = 0;
                int usadas = 0;
                int iIdentificador = 0;

                if (chbBolsa.Checked)
                {
                    eTipo = TipoConvenio.Bolsa;
                    total = Convert.ToInt32(upTotal.Value);
                }

                if (Int32.TryParse(tbIdentificador.Text, out iIdentificador))
                {
                    Convenio oConvenio = new Convenio(Convert.ToInt32(tbIdentificador.Text), tbNombre.Text, tbDescripcion.Text, eTipo, total, usadas);
                    misConvenios.ListConvenio.Add(oConvenio);

                    SerializeObject<Convenios>(misConvenios, path);

                    this.Width = 241;
                    this.Height = 370;
                    HabilitarBotones(true);

                    LoadData();
                }
                else
                {
                    MessageBox.Show("El identificador debe ser numerico.", "Visado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                DialogResult result1 = MessageBox.Show("¿Esta seguro que desea guardar los cambios del siguiente item? \nIdentificador: " + tbIdentificador.Text + "\nNombre: " + tbNombre.Text, "Visado", MessageBoxButtons.YesNo);
                if (result1 == DialogResult.Yes)
                {
                    TipoConvenio eTipo = TipoConvenio.Normal;
                    int total = 0;
                    int usadas = Convert.ToInt32(upUsadas.Value);
                    int iIdentificador = Convert.ToInt32(tbIdentificador.Text);

                    if (chbBolsa.Checked)
                    {
                        eTipo = TipoConvenio.Bolsa;
                        total = Convert.ToInt32(upTotal.Value);
                    }

                    foreach (var item in misConvenios.ListConvenio)
                    {
                        if (item.Identificador == iIdentificador)
                        {
                            item.Nombre = tbNombre.Text;
                            item.Descripcion = tbDescripcion.Text;

                            if (item.Tipo == TipoConvenio.Normal)
                            {
                                if (eTipo == TipoConvenio.Bolsa)
                                {
                                    item.Tipo = TipoConvenio.Bolsa;
                                    item.CantidadHorasTotal = total;
                                    item.CantidadHorasUsadas = 0;
                                }
                                else
                                {
                                    item.Tipo = TipoConvenio.Normal;
                                    item.CantidadHorasTotal = 0;
                                    item.CantidadHorasUsadas = 0;
                                }
                            }
                            else
                            {
                                if (eTipo == TipoConvenio.Normal)
                                {
                                    item.Tipo = TipoConvenio.Normal;
                                    item.CantidadHorasTotal = 0;
                                    item.CantidadHorasUsadas = 0;
                                }
                                else
                                {
                                    item.Tipo = TipoConvenio.Bolsa;
                                    item.CantidadHorasTotal = total;
                                }
                            }

                            break;
                        }
                    }



                    SerializeObject<Convenios>(misConvenios, path);

                    this.Width = 241;
                    this.Height = 370;
                    HabilitarBotones(true);

                    LoadData();
                }
                else
                {
                    this.Width = 241;
                    this.Height = 370;
                    HabilitarBotones(true);
                    LoadData();
                }
            }
        }

        private void btn_Eliminar_Click(object sender, EventArgs e)
        {
            Convenio oConvenio = new Convenio();
            oConvenio = (Convenio)lbConvenios.SelectedItem;

            DialogResult result1 = MessageBox.Show("¿Esta seguro que desea eliminar el siguiente item? \nIdentificador: " + oConvenio.Identificador + "\nNombre: " + oConvenio.Nombre, "Visado", MessageBoxButtons.YesNo);
            if (result1 == DialogResult.Yes)
            {
                for (int i = misConvenios.ListConvenio.Count - 1; i >= 0; i--)
                {
                    if (misConvenios.ListConvenio[i].Identificador == oConvenio.Identificador)
                    {
                        misConvenios.ListConvenio.RemoveAt(i);
                        break;
                    }
                }
            }

            SerializeObject<Convenios>(misConvenios, path);

            LoadData();
            
        }

        private void btn_Editar_Click(object sender, EventArgs e)
        {
            gbNuevo.Text = "Editar";

            this.Width = 450;
            this.Height = 370;

            gbNuevo.Visible = true;
            tbIdentificador.ReadOnly = true;

            LimpiarCampos();

            HabilitarBotones(false);

            Convenio oConvenio = new Convenio();
            oConvenio = (Convenio)lbConvenios.SelectedItem;

            tbIdentificador.Text = oConvenio.Identificador.ToString();
            tbNombre.Text = oConvenio.Nombre;
            tbDescripcion.Text = oConvenio.Descripcion;
            chbBolsa.Checked = oConvenio.Tipo == TipoConvenio.Bolsa ? true : false;
            upTotal.Value = oConvenio.CantidadHorasTotal;
            upUsadas.Value = oConvenio.CantidadHorasUsadas;

        }

    }

    public class Convenios
    {
        private List<Convenio> _ListConvenio = new List<Convenio>();

        public List<Convenio> ListConvenio
        {
            get { return _ListConvenio; }
            set { _ListConvenio = value; }
        }
    }

    public class Convenio
    {
        public Convenio()
        {

        }

        public Convenio(int sIdentificador, string sNombre, string sDescripcion, TipoConvenio eTipo, int iCantidad1, int iCantidad2)
        {
            _Identificador = sIdentificador;
            _Nombre = sNombre;
            _Descripcion = sDescripcion;
            _Tipo = eTipo;
            _CantidadHorasTotal = iCantidad1;
            _CantidadHorasUsadas = iCantidad2;
        }

        private int _Identificador = 0;

        public int Identificador
        {
            get { return _Identificador; }
            set { _Identificador = value; }
        }

        private string _Nombre = string.Empty;

        public string Nombre
        {
            get { return _Nombre; }
            set { _Nombre = value; }
        }

        private string _Descripcion = string.Empty;

        public string Descripcion
        {
            get { return _Descripcion; }
            set { _Descripcion = value; }
        }

        private TipoConvenio _Tipo = TipoConvenio.Normal;

        public TipoConvenio Tipo
        {
            get { return _Tipo; }
            set { _Tipo = value; }
        }

        private int _CantidadHorasTotal = 0;

        public int CantidadHorasTotal
        {
            get { return _CantidadHorasTotal; }
            set { _CantidadHorasTotal = value; }
        }

        private int _CantidadHorasUsadas = 0;

        public int CantidadHorasUsadas
        {
            get { return _CantidadHorasUsadas; }
            set { _CantidadHorasUsadas = value; }
        }
    }

    public enum TipoConvenio
    {
        Normal,
        Bolsa,
    }
}
