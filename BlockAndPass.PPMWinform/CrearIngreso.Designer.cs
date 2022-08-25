namespace BlockAndPass.PPMWinform
{
    partial class CrearIngreso
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CrearIngreso));
            this.label1 = new System.Windows.Forms.Label();
            this.btn_Cancel = new System.Windows.Forms.Button();
            this.btn_Ok = new System.Windows.Forms.Button();
            this.tbPlaca = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_Continuar = new System.Windows.Forms.Button();
            this.panelCreacion = new System.Windows.Forms.Panel();
            this.chbAutorizado = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cbEntrada = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.dtpFechaIngreso = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.cbTipoVehiculo = new System.Windows.Forms.ComboBox();
            this.panelCreacion.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(16, 11);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(245, 42);
            this.label1.TabIndex = 6;
            this.label1.Text = "Crear Ingreso";
            // 
            // btn_Cancel
            // 
            this.btn_Cancel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btn_Cancel.BackgroundImage")));
            this.btn_Cancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_Cancel.FlatAppearance.BorderSize = 0;
            this.btn_Cancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Cancel.Location = new System.Drawing.Point(124, 303);
            this.btn_Cancel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btn_Cancel.Name = "btn_Cancel";
            this.btn_Cancel.Size = new System.Drawing.Size(101, 75);
            this.btn_Cancel.TabIndex = 8;
            this.btn_Cancel.UseVisualStyleBackColor = true;
            this.btn_Cancel.Click += new System.EventHandler(this.btn_Cancel_Click);
            // 
            // btn_Ok
            // 
            this.btn_Ok.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btn_Ok.BackgroundImage")));
            this.btn_Ok.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_Ok.FlatAppearance.BorderSize = 0;
            this.btn_Ok.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Ok.Location = new System.Drawing.Point(261, 303);
            this.btn_Ok.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btn_Ok.Name = "btn_Ok";
            this.btn_Ok.Size = new System.Drawing.Size(101, 75);
            this.btn_Ok.TabIndex = 7;
            this.btn_Ok.UseVisualStyleBackColor = true;
            this.btn_Ok.Visible = false;
            this.btn_Ok.Click += new System.EventHandler(this.btn_Ok_Click);
            // 
            // tbPlaca
            // 
            this.tbPlaca.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbPlaca.Location = new System.Drawing.Point(124, 75);
            this.tbPlaca.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbPlaca.Name = "tbPlaca";
            this.tbPlaca.Size = new System.Drawing.Size(148, 37);
            this.tbPlaca.TabIndex = 9;
            this.tbPlaca.TextChanged += new System.EventHandler(this.tbPlaca_TextChanged);
            this.tbPlaca.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox1_KeyPress);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(17, 79);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 31);
            this.label2.TabIndex = 10;
            this.label2.Text = "Placa:";
            // 
            // btn_Continuar
            // 
            this.btn_Continuar.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btn_Continuar.BackgroundImage")));
            this.btn_Continuar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_Continuar.FlatAppearance.BorderSize = 0;
            this.btn_Continuar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Continuar.Location = new System.Drawing.Point(281, 75);
            this.btn_Continuar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btn_Continuar.Name = "btn_Continuar";
            this.btn_Continuar.Size = new System.Drawing.Size(47, 38);
            this.btn_Continuar.TabIndex = 11;
            this.btn_Continuar.UseVisualStyleBackColor = true;
            this.btn_Continuar.Click += new System.EventHandler(this.btn_Continuar_Click);
            // 
            // panelCreacion
            // 
            this.panelCreacion.Controls.Add(this.chbAutorizado);
            this.panelCreacion.Controls.Add(this.label5);
            this.panelCreacion.Controls.Add(this.cbEntrada);
            this.panelCreacion.Controls.Add(this.label4);
            this.panelCreacion.Controls.Add(this.dtpFechaIngreso);
            this.panelCreacion.Controls.Add(this.label3);
            this.panelCreacion.Controls.Add(this.cbTipoVehiculo);
            this.panelCreacion.Location = new System.Drawing.Point(24, 122);
            this.panelCreacion.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panelCreacion.Name = "panelCreacion";
            this.panelCreacion.Size = new System.Drawing.Size(339, 174);
            this.panelCreacion.TabIndex = 12;
            // 
            // chbAutorizado
            // 
            this.chbAutorizado.AutoSize = true;
            this.chbAutorizado.Location = new System.Drawing.Point(23, 20);
            this.chbAutorizado.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chbAutorizado.Name = "chbAutorizado";
            this.chbAutorizado.Size = new System.Drawing.Size(98, 21);
            this.chbAutorizado.TabIndex = 6;
            this.chbAutorizado.Text = "Autorizado";
            this.chbAutorizado.UseVisualStyleBackColor = true;
            this.chbAutorizado.Visible = false;
            this.chbAutorizado.CheckedChanged += new System.EventHandler(this.chbAutorizado_CheckedChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(19, 137);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(54, 17);
            this.label5.TabIndex = 5;
            this.label5.Text = "Modulo";
            // 
            // cbEntrada
            // 
            this.cbEntrada.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbEntrada.FormattingEnabled = true;
            this.cbEntrada.Location = new System.Drawing.Point(135, 133);
            this.cbEntrada.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbEntrada.Name = "cbEntrada";
            this.cbEntrada.Size = new System.Drawing.Size(191, 24);
            this.cbEntrada.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(19, 108);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(86, 17);
            this.label4.TabIndex = 3;
            this.label4.Text = "Fecha Hora:";
            // 
            // dtpFechaIngreso
            // 
            this.dtpFechaIngreso.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.dtpFechaIngreso.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFechaIngreso.Location = new System.Drawing.Point(135, 101);
            this.dtpFechaIngreso.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dtpFechaIngreso.Name = "dtpFechaIngreso";
            this.dtpFechaIngreso.Size = new System.Drawing.Size(191, 22);
            this.dtpFechaIngreso.TabIndex = 2;
            this.dtpFechaIngreso.ValueChanged += new System.EventHandler(this.dtpFechaIngreso_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(19, 71);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 17);
            this.label3.TabIndex = 1;
            this.label3.Text = "Tipo Vehiculo:";
            // 
            // cbTipoVehiculo
            // 
            this.cbTipoVehiculo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbTipoVehiculo.FormattingEnabled = true;
            this.cbTipoVehiculo.Location = new System.Drawing.Point(135, 68);
            this.cbTipoVehiculo.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cbTipoVehiculo.Name = "cbTipoVehiculo";
            this.cbTipoVehiculo.Size = new System.Drawing.Size(191, 24);
            this.cbTipoVehiculo.TabIndex = 0;
            this.cbTipoVehiculo.SelectedIndexChanged += new System.EventHandler(this.cbTipoVehiculo_SelectedIndexChanged);
            // 
            // CrearIngreso
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(379, 393);
            this.ControlBox = false;
            this.Controls.Add(this.panelCreacion);
            this.Controls.Add(this.btn_Continuar);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbPlaca);
            this.Controls.Add(this.btn_Cancel);
            this.Controls.Add(this.btn_Ok);
            this.Controls.Add(this.label1);
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CrearIngreso";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.panelCreacion.ResumeLayout(false);
            this.panelCreacion.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_Cancel;
        private System.Windows.Forms.Button btn_Ok;
        private System.Windows.Forms.TextBox tbPlaca;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn_Continuar;
        private System.Windows.Forms.Panel panelCreacion;
        private System.Windows.Forms.ComboBox cbTipoVehiculo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dtpFechaIngreso;
        private System.Windows.Forms.ComboBox cbEntrada;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox chbAutorizado;
    }
}