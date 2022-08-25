namespace BlockAndPass.VisadoWinform
{
    partial class AdministrarVisado
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdministrarVisado));
            this.lbConvenios = new System.Windows.Forms.ListBox();
            this.btn_Nuevo = new System.Windows.Forms.Button();
            this.btn_Editar = new System.Windows.Forms.Button();
            this.btn_Eliminar = new System.Windows.Forms.Button();
            this.gbNuevo = new System.Windows.Forms.GroupBox();
            this.lblUsadas = new System.Windows.Forms.Label();
            this.lblTotal = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.upUsadas = new System.Windows.Forms.NumericUpDown();
            this.upTotal = new System.Windows.Forms.NumericUpDown();
            this.chbBolsa = new System.Windows.Forms.CheckBox();
            this.tbDescripcion = new System.Windows.Forms.TextBox();
            this.tbNombre = new System.Windows.Forms.TextBox();
            this.tbIdentificador = new System.Windows.Forms.TextBox();
            this.btn_Cancelar = new System.Windows.Forms.Button();
            this.btn_Guardar = new System.Windows.Forms.Button();
            this.gbNuevo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.upUsadas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.upTotal)).BeginInit();
            this.SuspendLayout();
            // 
            // lbConvenios
            // 
            this.lbConvenios.FormattingEnabled = true;
            this.lbConvenios.Location = new System.Drawing.Point(12, 21);
            this.lbConvenios.Name = "lbConvenios";
            this.lbConvenios.Size = new System.Drawing.Size(170, 290);
            this.lbConvenios.TabIndex = 0;
            // 
            // btn_Nuevo
            // 
            this.btn_Nuevo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_Nuevo.FlatAppearance.BorderSize = 0;
            this.btn_Nuevo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Nuevo.Image = ((System.Drawing.Image)(resources.GetObject("btn_Nuevo.Image")));
            this.btn_Nuevo.Location = new System.Drawing.Point(188, 21);
            this.btn_Nuevo.Name = "btn_Nuevo";
            this.btn_Nuevo.Size = new System.Drawing.Size(32, 32);
            this.btn_Nuevo.TabIndex = 5;
            this.btn_Nuevo.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.btn_Nuevo.UseVisualStyleBackColor = true;
            this.btn_Nuevo.Click += new System.EventHandler(this.btn_Nuevo_Click);
            // 
            // btn_Editar
            // 
            this.btn_Editar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_Editar.FlatAppearance.BorderSize = 0;
            this.btn_Editar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Editar.Image = ((System.Drawing.Image)(resources.GetObject("btn_Editar.Image")));
            this.btn_Editar.Location = new System.Drawing.Point(188, 59);
            this.btn_Editar.Name = "btn_Editar";
            this.btn_Editar.Size = new System.Drawing.Size(32, 32);
            this.btn_Editar.TabIndex = 6;
            this.btn_Editar.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.btn_Editar.UseVisualStyleBackColor = true;
            this.btn_Editar.Click += new System.EventHandler(this.btn_Editar_Click);
            // 
            // btn_Eliminar
            // 
            this.btn_Eliminar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_Eliminar.FlatAppearance.BorderSize = 0;
            this.btn_Eliminar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Eliminar.Image = ((System.Drawing.Image)(resources.GetObject("btn_Eliminar.Image")));
            this.btn_Eliminar.Location = new System.Drawing.Point(188, 97);
            this.btn_Eliminar.Name = "btn_Eliminar";
            this.btn_Eliminar.Size = new System.Drawing.Size(32, 32);
            this.btn_Eliminar.TabIndex = 7;
            this.btn_Eliminar.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.btn_Eliminar.UseVisualStyleBackColor = true;
            this.btn_Eliminar.Click += new System.EventHandler(this.btn_Eliminar_Click);
            // 
            // gbNuevo
            // 
            this.gbNuevo.Controls.Add(this.lblUsadas);
            this.gbNuevo.Controls.Add(this.lblTotal);
            this.gbNuevo.Controls.Add(this.label3);
            this.gbNuevo.Controls.Add(this.label2);
            this.gbNuevo.Controls.Add(this.label1);
            this.gbNuevo.Controls.Add(this.upUsadas);
            this.gbNuevo.Controls.Add(this.upTotal);
            this.gbNuevo.Controls.Add(this.chbBolsa);
            this.gbNuevo.Controls.Add(this.tbDescripcion);
            this.gbNuevo.Controls.Add(this.tbNombre);
            this.gbNuevo.Controls.Add(this.tbIdentificador);
            this.gbNuevo.Controls.Add(this.btn_Cancelar);
            this.gbNuevo.Controls.Add(this.btn_Guardar);
            this.gbNuevo.Location = new System.Drawing.Point(226, 21);
            this.gbNuevo.Name = "gbNuevo";
            this.gbNuevo.Size = new System.Drawing.Size(196, 290);
            this.gbNuevo.TabIndex = 8;
            this.gbNuevo.TabStop = false;
            this.gbNuevo.Text = "groupBox1";
            this.gbNuevo.Visible = false;
            // 
            // lblUsadas
            // 
            this.lblUsadas.AutoSize = true;
            this.lblUsadas.Location = new System.Drawing.Point(106, 203);
            this.lblUsadas.Name = "lblUsadas";
            this.lblUsadas.Size = new System.Drawing.Size(46, 13);
            this.lblUsadas.TabIndex = 21;
            this.lblUsadas.Text = "Usadas:";
            this.lblUsadas.Visible = false;
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Location = new System.Drawing.Point(6, 203);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(34, 13);
            this.lblTotal.TabIndex = 20;
            this.lblTotal.Text = "Total:";
            this.lblTotal.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 86);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 13);
            this.label3.TabIndex = 19;
            this.label3.Text = "Descripcion:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 13);
            this.label2.TabIndex = 18;
            this.label2.Text = "Nombre:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 13);
            this.label1.TabIndex = 17;
            this.label1.Text = "Identificador:";
            // 
            // upUsadas
            // 
            this.upUsadas.Location = new System.Drawing.Point(109, 219);
            this.upUsadas.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.upUsadas.Name = "upUsadas";
            this.upUsadas.ReadOnly = true;
            this.upUsadas.Size = new System.Drawing.Size(75, 20);
            this.upUsadas.TabIndex = 16;
            this.upUsadas.Visible = false;
            // 
            // upTotal
            // 
            this.upTotal.Location = new System.Drawing.Point(9, 219);
            this.upTotal.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.upTotal.Name = "upTotal";
            this.upTotal.Size = new System.Drawing.Size(80, 20);
            this.upTotal.TabIndex = 15;
            this.upTotal.Visible = false;
            // 
            // chbBolsa
            // 
            this.chbBolsa.AutoSize = true;
            this.chbBolsa.Location = new System.Drawing.Point(9, 175);
            this.chbBolsa.Name = "chbBolsa";
            this.chbBolsa.Size = new System.Drawing.Size(52, 17);
            this.chbBolsa.TabIndex = 14;
            this.chbBolsa.Text = "Bolsa";
            this.chbBolsa.UseVisualStyleBackColor = true;
            this.chbBolsa.CheckedChanged += new System.EventHandler(this.chbBolsa_CheckedChanged);
            // 
            // tbDescripcion
            // 
            this.tbDescripcion.Location = new System.Drawing.Point(9, 102);
            this.tbDescripcion.Multiline = true;
            this.tbDescripcion.Name = "tbDescripcion";
            this.tbDescripcion.Size = new System.Drawing.Size(175, 67);
            this.tbDescripcion.TabIndex = 13;
            // 
            // tbNombre
            // 
            this.tbNombre.Location = new System.Drawing.Point(9, 62);
            this.tbNombre.Name = "tbNombre";
            this.tbNombre.Size = new System.Drawing.Size(175, 20);
            this.tbNombre.TabIndex = 12;
            // 
            // tbIdentificador
            // 
            this.tbIdentificador.Location = new System.Drawing.Point(109, 20);
            this.tbIdentificador.Name = "tbIdentificador";
            this.tbIdentificador.Size = new System.Drawing.Size(75, 20);
            this.tbIdentificador.TabIndex = 11;
            // 
            // btn_Cancelar
            // 
            this.btn_Cancelar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_Cancelar.FlatAppearance.BorderSize = 0;
            this.btn_Cancelar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Cancelar.Image = ((System.Drawing.Image)(resources.GetObject("btn_Cancelar.Image")));
            this.btn_Cancelar.Location = new System.Drawing.Point(119, 246);
            this.btn_Cancelar.Name = "btn_Cancelar";
            this.btn_Cancelar.Size = new System.Drawing.Size(32, 32);
            this.btn_Cancelar.TabIndex = 10;
            this.btn_Cancelar.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.btn_Cancelar.UseVisualStyleBackColor = true;
            this.btn_Cancelar.Click += new System.EventHandler(this.btn_Cancelar_Click);
            // 
            // btn_Guardar
            // 
            this.btn_Guardar.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_Guardar.FlatAppearance.BorderSize = 0;
            this.btn_Guardar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Guardar.Image = ((System.Drawing.Image)(resources.GetObject("btn_Guardar.Image")));
            this.btn_Guardar.Location = new System.Drawing.Point(157, 246);
            this.btn_Guardar.Name = "btn_Guardar";
            this.btn_Guardar.Size = new System.Drawing.Size(32, 32);
            this.btn_Guardar.TabIndex = 9;
            this.btn_Guardar.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.btn_Guardar.UseVisualStyleBackColor = true;
            this.btn_Guardar.Click += new System.EventHandler(this.btn_Guardar_Click);
            // 
            // AdministrarVisado
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(433, 331);
            this.Controls.Add(this.gbNuevo);
            this.Controls.Add(this.btn_Eliminar);
            this.Controls.Add(this.btn_Editar);
            this.Controls.Add(this.btn_Nuevo);
            this.Controls.Add(this.lbConvenios);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AdministrarVisado";
            this.Text = "Administrar Visado";
            this.gbNuevo.ResumeLayout(false);
            this.gbNuevo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.upUsadas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.upTotal)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox lbConvenios;
        private System.Windows.Forms.Button btn_Nuevo;
        private System.Windows.Forms.Button btn_Editar;
        private System.Windows.Forms.Button btn_Eliminar;
        private System.Windows.Forms.GroupBox gbNuevo;
        private System.Windows.Forms.Button btn_Cancelar;
        private System.Windows.Forms.Button btn_Guardar;
        private System.Windows.Forms.CheckBox chbBolsa;
        private System.Windows.Forms.TextBox tbDescripcion;
        private System.Windows.Forms.TextBox tbNombre;
        private System.Windows.Forms.TextBox tbIdentificador;
        private System.Windows.Forms.NumericUpDown upUsadas;
        private System.Windows.Forms.NumericUpDown upTotal;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblUsadas;
        private System.Windows.Forms.Label lblTotal;
    }
}