namespace BlockAndPass.PPMWinform
{
    partial class PPM
    {
        /// <summary>
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben eliminar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PPM));
            this.btn_Leer = new System.Windows.Forms.Button();
            this.btn_Convenio = new System.Windows.Forms.Button();
            this.btn_Carga = new System.Windows.Forms.Button();
            this.btn_Arqueo = new System.Windows.Forms.Button();
            this.btn_Cortesia = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblPPM = new System.Windows.Forms.Label();
            this.lblEstacionamiento = new System.Windows.Forms.Label();
            this.lblSede = new System.Windows.Forms.Label();
            this.cbSede = new System.Windows.Forms.ComboBox();
            this.cbEstacionamiento = new System.Windows.Forms.ComboBox();
            this.cbPPM = new System.Windows.Forms.ComboBox();
            this.btn_Cerrar = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.tbRecibido = new System.Windows.Forms.TextBox();
            this.tbValorPagar = new System.Windows.Forms.TextBox();
            this.tbCambio = new System.Windows.Forms.TextBox();
            this.panelPagar = new System.Windows.Forms.Panel();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.tbPlaca = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.tbCasillero = new System.Windows.Forms.TextBox();
            this.btn_Casco = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.tbTiempo = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tbHoraPago = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tbHoraIngreso = new System.Windows.Forms.TextBox();
            this.chbMensualidad = new System.Windows.Forms.CheckBox();
            this.chbEstacionamiento = new System.Windows.Forms.CheckBox();
            this.lblTiempoFuera = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_Limpiar = new System.Windows.Forms.Button();
            this.btn_Pagar = new System.Windows.Forms.Button();
            this.tbIdTarjeta = new System.Windows.Forms.TextBox();
            this.tbIdTransaccion = new System.Windows.Forms.TextBox();
            this.btn_Copia = new System.Windows.Forms.Button();
            this.panelTodo = new System.Windows.Forms.Panel();
            this.PnelMensualidadPlaca = new System.Windows.Forms.Panel();
            this.ckMensualidadDocumento = new System.Windows.Forms.CheckBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txtPlaca = new System.Windows.Forms.TextBox();
            this.btn_Entrada = new System.Windows.Forms.Button();
            this.btn_Eventos = new System.Windows.Forms.Button();
            this.btn_Moto = new System.Windows.Forms.Button();
            this.lblHoraPago = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.tbUsuario = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.panelPagar.SuspendLayout();
            this.panelTodo.SuspendLayout();
            this.PnelMensualidadPlaca.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_Leer
            // 
            this.btn_Leer.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btn_Leer.BackgroundImage")));
            this.btn_Leer.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_Leer.FlatAppearance.BorderSize = 0;
            this.btn_Leer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Leer.Location = new System.Drawing.Point(663, 249);
            this.btn_Leer.Name = "btn_Leer";
            this.btn_Leer.Size = new System.Drawing.Size(76, 61);
            this.btn_Leer.TabIndex = 0;
            this.btn_Leer.UseVisualStyleBackColor = true;
            this.btn_Leer.Click += new System.EventHandler(this.btn_Leer_Click);
            // 
            // btn_Convenio
            // 
            this.btn_Convenio.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btn_Convenio.BackgroundImage")));
            this.btn_Convenio.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_Convenio.FlatAppearance.BorderSize = 0;
            this.btn_Convenio.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Convenio.Location = new System.Drawing.Point(827, 253);
            this.btn_Convenio.Name = "btn_Convenio";
            this.btn_Convenio.Size = new System.Drawing.Size(76, 61);
            this.btn_Convenio.TabIndex = 1;
            this.btn_Convenio.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.btn_Convenio.UseVisualStyleBackColor = true;
            this.btn_Convenio.Click += new System.EventHandler(this.btn_Convenio_Click);
            // 
            // btn_Carga
            // 
            this.btn_Carga.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btn_Carga.BackgroundImage")));
            this.btn_Carga.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_Carga.Enabled = false;
            this.btn_Carga.FlatAppearance.BorderSize = 0;
            this.btn_Carga.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Carga.Location = new System.Drawing.Point(827, 323);
            this.btn_Carga.Name = "btn_Carga";
            this.btn_Carga.Size = new System.Drawing.Size(76, 61);
            this.btn_Carga.TabIndex = 2;
            this.btn_Carga.UseVisualStyleBackColor = true;
            this.btn_Carga.Click += new System.EventHandler(this.btn_Carga_Click);
            // 
            // btn_Arqueo
            // 
            this.btn_Arqueo.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btn_Arqueo.BackgroundImage")));
            this.btn_Arqueo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_Arqueo.FlatAppearance.BorderSize = 0;
            this.btn_Arqueo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Arqueo.Location = new System.Drawing.Point(745, 323);
            this.btn_Arqueo.Name = "btn_Arqueo";
            this.btn_Arqueo.Size = new System.Drawing.Size(76, 61);
            this.btn_Arqueo.TabIndex = 5;
            this.btn_Arqueo.UseVisualStyleBackColor = true;
            this.btn_Arqueo.Click += new System.EventHandler(this.btn_Arqueo_Click);
            // 
            // btn_Cortesia
            // 
            this.btn_Cortesia.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btn_Cortesia.BackgroundImage")));
            this.btn_Cortesia.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_Cortesia.FlatAppearance.BorderSize = 0;
            this.btn_Cortesia.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Cortesia.Location = new System.Drawing.Point(663, 323);
            this.btn_Cortesia.Name = "btn_Cortesia";
            this.btn_Cortesia.Size = new System.Drawing.Size(76, 61);
            this.btn_Cortesia.TabIndex = 4;
            this.btn_Cortesia.UseVisualStyleBackColor = true;
            this.btn_Cortesia.Click += new System.EventHandler(this.btn_Cortesia_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblPPM);
            this.groupBox1.Controls.Add(this.lblEstacionamiento);
            this.groupBox1.Controls.Add(this.lblSede);
            this.groupBox1.Controls.Add(this.cbSede);
            this.groupBox1.Controls.Add(this.cbEstacionamiento);
            this.groupBox1.Controls.Add(this.cbPPM);
            this.groupBox1.Location = new System.Drawing.Point(17, 10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(800, 88);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "PPM";
            // 
            // lblPPM
            // 
            this.lblPPM.AutoSize = true;
            this.lblPPM.Location = new System.Drawing.Point(599, 33);
            this.lblPPM.Name = "lblPPM";
            this.lblPPM.Size = new System.Drawing.Size(30, 13);
            this.lblPPM.TabIndex = 5;
            this.lblPPM.Text = "PPM";
            // 
            // lblEstacionamiento
            // 
            this.lblEstacionamiento.AutoSize = true;
            this.lblEstacionamiento.Location = new System.Drawing.Point(323, 33);
            this.lblEstacionamiento.Name = "lblEstacionamiento";
            this.lblEstacionamiento.Size = new System.Drawing.Size(85, 13);
            this.lblEstacionamiento.TabIndex = 4;
            this.lblEstacionamiento.Text = "Estacionamiento";
            // 
            // lblSede
            // 
            this.lblSede.AutoSize = true;
            this.lblSede.Location = new System.Drawing.Point(29, 33);
            this.lblSede.Name = "lblSede";
            this.lblSede.Size = new System.Drawing.Size(32, 13);
            this.lblSede.TabIndex = 3;
            this.lblSede.Text = "Sede";
            // 
            // cbSede
            // 
            this.cbSede.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSede.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbSede.FormattingEnabled = true;
            this.cbSede.Location = new System.Drawing.Point(32, 49);
            this.cbSede.Name = "cbSede";
            this.cbSede.Size = new System.Drawing.Size(255, 33);
            this.cbSede.TabIndex = 2;
            // 
            // cbEstacionamiento
            // 
            this.cbEstacionamiento.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbEstacionamiento.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbEstacionamiento.FormattingEnabled = true;
            this.cbEstacionamiento.Location = new System.Drawing.Point(317, 49);
            this.cbEstacionamiento.Name = "cbEstacionamiento";
            this.cbEstacionamiento.Size = new System.Drawing.Size(255, 33);
            this.cbEstacionamiento.TabIndex = 1;
            // 
            // cbPPM
            // 
            this.cbPPM.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPPM.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbPPM.FormattingEnabled = true;
            this.cbPPM.Location = new System.Drawing.Point(593, 49);
            this.cbPPM.Name = "cbPPM";
            this.cbPPM.Size = new System.Drawing.Size(116, 33);
            this.cbPPM.TabIndex = 0;
            // 
            // btn_Cerrar
            // 
            this.btn_Cerrar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btn_Cerrar.BackgroundImage")));
            this.btn_Cerrar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_Cerrar.FlatAppearance.BorderSize = 0;
            this.btn_Cerrar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Cerrar.Location = new System.Drawing.Point(744, 8);
            this.btn_Cerrar.Name = "btn_Cerrar";
            this.btn_Cerrar.Size = new System.Drawing.Size(52, 37);
            this.btn_Cerrar.TabIndex = 7;
            this.btn_Cerrar.UseVisualStyleBackColor = true;
            this.btn_Cerrar.Click += new System.EventHandler(this.btn_Cerrar_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.ForeColor = System.Drawing.Color.Blue;
            this.progressBar1.Location = new System.Drawing.Point(12, 529);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(100, 23);
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBar1.TabIndex = 8;
            // 
            // tbRecibido
            // 
            this.tbRecibido.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbRecibido.Location = new System.Drawing.Point(116, 261);
            this.tbRecibido.Name = "tbRecibido";
            this.tbRecibido.Size = new System.Drawing.Size(86, 31);
            this.tbRecibido.TabIndex = 8;
            this.tbRecibido.TextChanged += new System.EventHandler(this.tbRecibido_TextChanged);
            // 
            // tbValorPagar
            // 
            this.tbValorPagar.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbValorPagar.Location = new System.Drawing.Point(116, 226);
            this.tbValorPagar.Name = "tbValorPagar";
            this.tbValorPagar.ReadOnly = true;
            this.tbValorPagar.Size = new System.Drawing.Size(270, 31);
            this.tbValorPagar.TabIndex = 9;
            // 
            // tbCambio
            // 
            this.tbCambio.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbCambio.Location = new System.Drawing.Point(284, 261);
            this.tbCambio.Name = "tbCambio";
            this.tbCambio.ReadOnly = true;
            this.tbCambio.Size = new System.Drawing.Size(102, 31);
            this.tbCambio.TabIndex = 10;
            // 
            // panelPagar
            // 
            this.panelPagar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelPagar.Controls.Add(this.textBox1);
            this.panelPagar.Controls.Add(this.label12);
            this.panelPagar.Controls.Add(this.tbPlaca);
            this.panelPagar.Controls.Add(this.label11);
            this.panelPagar.Controls.Add(this.label10);
            this.panelPagar.Controls.Add(this.tbCasillero);
            this.panelPagar.Controls.Add(this.btn_Casco);
            this.panelPagar.Controls.Add(this.label8);
            this.panelPagar.Controls.Add(this.tbTiempo);
            this.panelPagar.Controls.Add(this.label7);
            this.panelPagar.Controls.Add(this.tbHoraPago);
            this.panelPagar.Controls.Add(this.label6);
            this.panelPagar.Controls.Add(this.tbHoraIngreso);
            this.panelPagar.Controls.Add(this.chbMensualidad);
            this.panelPagar.Controls.Add(this.chbEstacionamiento);
            this.panelPagar.Controls.Add(this.lblTiempoFuera);
            this.panelPagar.Controls.Add(this.label5);
            this.panelPagar.Controls.Add(this.label4);
            this.panelPagar.Controls.Add(this.label3);
            this.panelPagar.Controls.Add(this.label2);
            this.panelPagar.Controls.Add(this.label1);
            this.panelPagar.Controls.Add(this.btn_Limpiar);
            this.panelPagar.Controls.Add(this.btn_Pagar);
            this.panelPagar.Controls.Add(this.tbIdTarjeta);
            this.panelPagar.Controls.Add(this.tbIdTransaccion);
            this.panelPagar.Controls.Add(this.tbValorPagar);
            this.panelPagar.Controls.Add(this.tbCambio);
            this.panelPagar.Controls.Add(this.tbRecibido);
            this.panelPagar.Location = new System.Drawing.Point(35, 104);
            this.panelPagar.Name = "panelPagar";
            this.panelPagar.Size = new System.Drawing.Size(516, 365);
            this.panelPagar.TabIndex = 11;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(69, 330);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(122, 20);
            this.textBox1.TabIndex = 34;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(12, 333);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(52, 13);
            this.label12.TabIndex = 33;
            this.label12.Text = "Convenio";
            this.label12.Click += new System.EventHandler(this.label12_Click);
            // 
            // tbPlaca
            // 
            this.tbPlaca.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbPlaca.Location = new System.Drawing.Point(116, 51);
            this.tbPlaca.Name = "tbPlaca";
            this.tbPlaca.ReadOnly = true;
            this.tbPlaca.Size = new System.Drawing.Size(270, 31);
            this.tbPlaca.TabIndex = 32;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(14, 61);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(43, 16);
            this.label11.TabIndex = 31;
            this.label11.Text = "Placa";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Enabled = false;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(208, 307);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(46, 13);
            this.label10.TabIndex = 29;
            this.label10.Text = "Casillero";
            // 
            // tbCasillero
            // 
            this.tbCasillero.Enabled = false;
            this.tbCasillero.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbCasillero.Location = new System.Drawing.Point(260, 303);
            this.tbCasillero.Name = "tbCasillero";
            this.tbCasillero.ReadOnly = true;
            this.tbCasillero.Size = new System.Drawing.Size(43, 20);
            this.tbCasillero.TabIndex = 28;
            // 
            // btn_Casco
            // 
            this.btn_Casco.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btn_Casco.BackgroundImage")));
            this.btn_Casco.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_Casco.Enabled = false;
            this.btn_Casco.FlatAppearance.BorderSize = 0;
            this.btn_Casco.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Casco.Location = new System.Drawing.Point(418, 201);
            this.btn_Casco.Name = "btn_Casco";
            this.btn_Casco.Size = new System.Drawing.Size(76, 61);
            this.btn_Casco.TabIndex = 27;
            this.btn_Casco.UseVisualStyleBackColor = true;
            this.btn_Casco.Click += new System.EventHandler(this.btn_Casco_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(14, 201);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(55, 16);
            this.label8.TabIndex = 26;
            this.label8.Text = "Tiempo";
            // 
            // tbTiempo
            // 
            this.tbTiempo.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbTiempo.Location = new System.Drawing.Point(116, 191);
            this.tbTiempo.Name = "tbTiempo";
            this.tbTiempo.ReadOnly = true;
            this.tbTiempo.Size = new System.Drawing.Size(270, 31);
            this.tbTiempo.TabIndex = 25;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(14, 166);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(57, 16);
            this.label7.TabIndex = 24;
            this.label7.Text = "H. Pago";
            // 
            // tbHoraPago
            // 
            this.tbHoraPago.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbHoraPago.Location = new System.Drawing.Point(116, 156);
            this.tbHoraPago.Name = "tbHoraPago";
            this.tbHoraPago.ReadOnly = true;
            this.tbHoraPago.Size = new System.Drawing.Size(270, 31);
            this.tbHoraPago.TabIndex = 23;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(14, 131);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(69, 16);
            this.label6.TabIndex = 22;
            this.label6.Text = "H. Ingreso";
            // 
            // tbHoraIngreso
            // 
            this.tbHoraIngreso.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbHoraIngreso.Location = new System.Drawing.Point(116, 121);
            this.tbHoraIngreso.Name = "tbHoraIngreso";
            this.tbHoraIngreso.ReadOnly = true;
            this.tbHoraIngreso.Size = new System.Drawing.Size(270, 31);
            this.tbHoraIngreso.TabIndex = 21;
            // 
            // chbMensualidad
            // 
            this.chbMensualidad.AutoSize = true;
            this.chbMensualidad.Enabled = false;
            this.chbMensualidad.Location = new System.Drawing.Point(116, 306);
            this.chbMensualidad.Name = "chbMensualidad";
            this.chbMensualidad.Size = new System.Drawing.Size(86, 17);
            this.chbMensualidad.TabIndex = 20;
            this.chbMensualidad.Text = "Mensualidad";
            this.chbMensualidad.UseVisualStyleBackColor = true;
            // 
            // chbEstacionamiento
            // 
            this.chbEstacionamiento.AutoSize = true;
            this.chbEstacionamiento.Enabled = false;
            this.chbEstacionamiento.Location = new System.Drawing.Point(10, 306);
            this.chbEstacionamiento.Name = "chbEstacionamiento";
            this.chbEstacionamiento.Size = new System.Drawing.Size(104, 17);
            this.chbEstacionamiento.TabIndex = 19;
            this.chbEstacionamiento.Text = "Estacionamiento";
            this.chbEstacionamiento.UseVisualStyleBackColor = true;
            // 
            // lblTiempoFuera
            // 
            this.lblTiempoFuera.Location = new System.Drawing.Point(347, 315);
            this.lblTiempoFuera.Name = "lblTiempoFuera";
            this.lblTiempoFuera.Size = new System.Drawing.Size(164, 15);
            this.lblTiempoFuera.TabIndex = 18;
            this.lblTiempoFuera.Text = "label6";
            this.lblTiempoFuera.Visible = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(213, 271);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(55, 16);
            this.label5.TabIndex = 17;
            this.label5.Text = "Cambio";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(14, 271);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 16);
            this.label4.TabIndex = 16;
            this.label4.Text = "Recibido";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(14, 236);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 16);
            this.label3.TabIndex = 15;
            this.label3.Text = "ValorPagar";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(14, 96);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 16);
            this.label2.TabIndex = 14;
            this.label2.Text = "IdTarjeta";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(14, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 16);
            this.label1.TabIndex = 6;
            this.label1.Text = "IdTransaccion";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // btn_Limpiar
            // 
            this.btn_Limpiar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btn_Limpiar.BackgroundImage")));
            this.btn_Limpiar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_Limpiar.FlatAppearance.BorderSize = 0;
            this.btn_Limpiar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Limpiar.Location = new System.Drawing.Point(418, 123);
            this.btn_Limpiar.Name = "btn_Limpiar";
            this.btn_Limpiar.Size = new System.Drawing.Size(76, 61);
            this.btn_Limpiar.TabIndex = 13;
            this.btn_Limpiar.UseVisualStyleBackColor = true;
            this.btn_Limpiar.Click += new System.EventHandler(this.btn_Limpiar_Click);
            // 
            // btn_Pagar
            // 
            this.btn_Pagar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btn_Pagar.BackgroundImage")));
            this.btn_Pagar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_Pagar.FlatAppearance.BorderSize = 0;
            this.btn_Pagar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Pagar.Location = new System.Drawing.Point(418, 56);
            this.btn_Pagar.Name = "btn_Pagar";
            this.btn_Pagar.Size = new System.Drawing.Size(76, 61);
            this.btn_Pagar.TabIndex = 12;
            this.btn_Pagar.UseVisualStyleBackColor = true;
            this.btn_Pagar.Click += new System.EventHandler(this.btn_Pagar_Click);
            // 
            // tbIdTarjeta
            // 
            this.tbIdTarjeta.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbIdTarjeta.Location = new System.Drawing.Point(116, 86);
            this.tbIdTarjeta.Name = "tbIdTarjeta";
            this.tbIdTarjeta.ReadOnly = true;
            this.tbIdTarjeta.Size = new System.Drawing.Size(270, 31);
            this.tbIdTarjeta.TabIndex = 12;
            // 
            // tbIdTransaccion
            // 
            this.tbIdTransaccion.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbIdTransaccion.Location = new System.Drawing.Point(116, 16);
            this.tbIdTransaccion.Name = "tbIdTransaccion";
            this.tbIdTransaccion.ReadOnly = true;
            this.tbIdTransaccion.Size = new System.Drawing.Size(270, 31);
            this.tbIdTransaccion.TabIndex = 11;
            this.tbIdTransaccion.TextChanged += new System.EventHandler(this.tbIdTransaccion_TextChanged);
            // 
            // btn_Copia
            // 
            this.btn_Copia.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btn_Copia.BackgroundImage")));
            this.btn_Copia.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_Copia.FlatAppearance.BorderSize = 0;
            this.btn_Copia.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Copia.Location = new System.Drawing.Point(745, 393);
            this.btn_Copia.Name = "btn_Copia";
            this.btn_Copia.Size = new System.Drawing.Size(76, 61);
            this.btn_Copia.TabIndex = 30;
            this.btn_Copia.UseVisualStyleBackColor = true;
            this.btn_Copia.Click += new System.EventHandler(this.btn_Copia_Click);
            // 
            // panelTodo
            // 
            this.panelTodo.Controls.Add(this.PnelMensualidadPlaca);
            this.panelTodo.Controls.Add(this.btn_Copia);
            this.panelTodo.Controls.Add(this.btn_Entrada);
            this.panelTodo.Controls.Add(this.btn_Eventos);
            this.panelTodo.Controls.Add(this.panelPagar);
            this.panelTodo.Controls.Add(this.groupBox1);
            this.panelTodo.Controls.Add(this.btn_Moto);
            this.panelTodo.Controls.Add(this.btn_Carga);
            this.panelTodo.Controls.Add(this.btn_Cortesia);
            this.panelTodo.Controls.Add(this.btn_Leer);
            this.panelTodo.Controls.Add(this.btn_Convenio);
            this.panelTodo.Controls.Add(this.btn_Arqueo);
            this.panelTodo.Enabled = false;
            this.panelTodo.Location = new System.Drawing.Point(12, 51);
            this.panelTodo.Name = "panelTodo";
            this.panelTodo.Size = new System.Drawing.Size(1042, 472);
            this.panelTodo.TabIndex = 9;
            this.panelTodo.Paint += new System.Windows.Forms.PaintEventHandler(this.panelTodo_Paint);
            // 
            // PnelMensualidadPlaca
            // 
            this.PnelMensualidadPlaca.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PnelMensualidadPlaca.Controls.Add(this.ckMensualidadDocumento);
            this.PnelMensualidadPlaca.Controls.Add(this.label13);
            this.PnelMensualidadPlaca.Controls.Add(this.txtPlaca);
            this.PnelMensualidadPlaca.Location = new System.Drawing.Point(596, 122);
            this.PnelMensualidadPlaca.Name = "PnelMensualidadPlaca";
            this.PnelMensualidadPlaca.Size = new System.Drawing.Size(369, 100);
            this.PnelMensualidadPlaca.TabIndex = 36;
            // 
            // ckMensualidadDocumento
            // 
            this.ckMensualidadDocumento.AutoSize = true;
            this.ckMensualidadDocumento.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ckMensualidadDocumento.Location = new System.Drawing.Point(21, 20);
            this.ckMensualidadDocumento.Name = "ckMensualidadDocumento";
            this.ckMensualidadDocumento.Size = new System.Drawing.Size(287, 24);
            this.ckMensualidadDocumento.TabIndex = 31;
            this.ckMensualidadDocumento.Text = "Renovar Mensualidad Con Placa";
            this.ckMensualidadDocumento.UseVisualStyleBackColor = true;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(43, 65);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(58, 20);
            this.label13.TabIndex = 33;
            this.label13.Text = "Placa:";
            // 
            // txtPlaca
            // 
            this.txtPlaca.BackColor = System.Drawing.Color.Gold;
            this.txtPlaca.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPlaca.Location = new System.Drawing.Point(127, 58);
            this.txtPlaca.Name = "txtPlaca";
            this.txtPlaca.Size = new System.Drawing.Size(104, 31);
            this.txtPlaca.TabIndex = 32;
            this.txtPlaca.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // btn_Entrada
            // 
            this.btn_Entrada.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btn_Entrada.BackgroundImage")));
            this.btn_Entrada.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_Entrada.FlatAppearance.BorderSize = 0;
            this.btn_Entrada.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Entrada.Location = new System.Drawing.Point(827, 393);
            this.btn_Entrada.Name = "btn_Entrada";
            this.btn_Entrada.Size = new System.Drawing.Size(76, 61);
            this.btn_Entrada.TabIndex = 13;
            this.btn_Entrada.UseVisualStyleBackColor = true;
            this.btn_Entrada.Click += new System.EventHandler(this.btn_Entrada_Click);
            // 
            // btn_Eventos
            // 
            this.btn_Eventos.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btn_Eventos.BackgroundImage")));
            this.btn_Eventos.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_Eventos.Enabled = false;
            this.btn_Eventos.FlatAppearance.BorderSize = 0;
            this.btn_Eventos.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Eventos.Location = new System.Drawing.Point(663, 393);
            this.btn_Eventos.Name = "btn_Eventos";
            this.btn_Eventos.Size = new System.Drawing.Size(76, 61);
            this.btn_Eventos.TabIndex = 12;
            this.btn_Eventos.UseVisualStyleBackColor = true;
            this.btn_Eventos.Click += new System.EventHandler(this.btn_Eventos_Click);
            // 
            // btn_Moto
            // 
            this.btn_Moto.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btn_Moto.BackgroundImage")));
            this.btn_Moto.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_Moto.Enabled = false;
            this.btn_Moto.FlatAppearance.BorderSize = 0;
            this.btn_Moto.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Moto.Location = new System.Drawing.Point(745, 249);
            this.btn_Moto.Name = "btn_Moto";
            this.btn_Moto.Size = new System.Drawing.Size(76, 61);
            this.btn_Moto.TabIndex = 7;
            this.btn_Moto.UseVisualStyleBackColor = true;
            this.btn_Moto.Click += new System.EventHandler(this.btn_Moto_Click);
            // 
            // lblHoraPago
            // 
            this.lblHoraPago.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHoraPago.Location = new System.Drawing.Point(683, 539);
            this.lblHoraPago.Name = "lblHoraPago";
            this.lblHoraPago.Size = new System.Drawing.Size(150, 23);
            this.lblHoraPago.TabIndex = 10;
            this.lblHoraPago.Text = "label6";
            this.lblHoraPago.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(26, 20);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(84, 24);
            this.label9.TabIndex = 11;
            this.label9.Text = "Usuario: ";
            // 
            // tbUsuario
            // 
            this.tbUsuario.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbUsuario.Location = new System.Drawing.Point(116, 19);
            this.tbUsuario.Name = "tbUsuario";
            this.tbUsuario.ReadOnly = true;
            this.tbUsuario.Size = new System.Drawing.Size(211, 26);
            this.tbUsuario.TabIndex = 12;
            // 
            // PPM
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(1066, 589);
            this.ControlBox = false;
            this.Controls.Add(this.tbUsuario);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.lblHoraPago);
            this.Controls.Add(this.panelTodo);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.btn_Cerrar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PPM";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panelPagar.ResumeLayout(false);
            this.panelPagar.PerformLayout();
            this.panelTodo.ResumeLayout(false);
            this.PnelMensualidadPlaca.ResumeLayout(false);
            this.PnelMensualidadPlaca.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_Leer;
        private System.Windows.Forms.Button btn_Convenio;
        private System.Windows.Forms.Button btn_Carga;
        private System.Windows.Forms.Button btn_Arqueo;
        private System.Windows.Forms.Button btn_Cortesia;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblPPM;
        private System.Windows.Forms.Label lblEstacionamiento;
        private System.Windows.Forms.Label lblSede;
        private System.Windows.Forms.ComboBox cbSede;
        private System.Windows.Forms.ComboBox cbEstacionamiento;
        private System.Windows.Forms.ComboBox cbPPM;
        private System.Windows.Forms.Button btn_Cerrar;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Panel panelPagar;
        private System.Windows.Forms.TextBox tbValorPagar;
        private System.Windows.Forms.TextBox tbCambio;
        private System.Windows.Forms.TextBox tbRecibido;
        private System.Windows.Forms.Button btn_Limpiar;
        private System.Windows.Forms.Button btn_Pagar;
        private System.Windows.Forms.TextBox tbIdTarjeta;
        private System.Windows.Forms.TextBox tbIdTransaccion;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panelTodo;
        private System.Windows.Forms.Label lblHoraPago;
        private System.Windows.Forms.Label lblTiempoFuera;
        private System.Windows.Forms.CheckBox chbMensualidad;
        private System.Windows.Forms.CheckBox chbEstacionamiento;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbHoraIngreso;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tbTiempo;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tbHoraPago;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tbUsuario;
        private System.Windows.Forms.Button btn_Eventos;
        private System.Windows.Forms.Button btn_Entrada;
        private System.Windows.Forms.Button btn_Casco;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox tbCasillero;
        private System.Windows.Forms.Button btn_Copia;
        private System.Windows.Forms.TextBox tbPlaca;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button btn_Moto;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Panel PnelMensualidadPlaca;
        private System.Windows.Forms.CheckBox ckMensualidadDocumento;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtPlaca;
    }
}

