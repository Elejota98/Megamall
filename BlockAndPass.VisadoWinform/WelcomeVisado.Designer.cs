namespace BlockAndPass.VisadoWinform
{
    partial class WelcomeVisado
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WelcomeVisado));
            this.btn_Convenio = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btn_Next = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tbClave = new System.Windows.Forms.TextBox();
            this.tbUsuario = new System.Windows.Forms.TextBox();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.errorProvider2 = new System.Windows.Forms.ErrorProvider(this.components);
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider2)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_Convenio
            // 
            this.btn_Convenio.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btn_Convenio.BackgroundImage")));
            this.btn_Convenio.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_Convenio.FlatAppearance.BorderSize = 0;
            this.btn_Convenio.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Convenio.Location = new System.Drawing.Point(12, 16);
            this.btn_Convenio.Name = "btn_Convenio";
            this.btn_Convenio.Size = new System.Drawing.Size(132, 124);
            this.btn_Convenio.TabIndex = 2;
            this.btn_Convenio.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.btn_Convenio.UseVisualStyleBackColor = true;
            this.btn_Convenio.Click += new System.EventHandler(this.btn_Convenio_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btn_Next);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.tbClave);
            this.groupBox1.Controls.Add(this.tbUsuario);
            this.groupBox1.Location = new System.Drawing.Point(161, 16);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(183, 124);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Administrar";
            // 
            // btn_Next
            // 
            this.btn_Next.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_Next.FlatAppearance.BorderSize = 0;
            this.btn_Next.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_Next.Image = ((System.Drawing.Image)(resources.GetObject("btn_Next.Image")));
            this.btn_Next.Location = new System.Drawing.Point(119, 86);
            this.btn_Next.Name = "btn_Next";
            this.btn_Next.Size = new System.Drawing.Size(32, 32);
            this.btn_Next.TabIndex = 4;
            this.btn_Next.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.btn_Next.UseVisualStyleBackColor = true;
            this.btn_Next.Click += new System.EventHandler(this.btn_Next_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(34, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Clave";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Usuario";
            // 
            // tbClave
            // 
            this.tbClave.Location = new System.Drawing.Point(62, 62);
            this.tbClave.Name = "tbClave";
            this.tbClave.PasswordChar = '*';
            this.tbClave.Size = new System.Drawing.Size(89, 20);
            this.tbClave.TabIndex = 1;
            this.tbClave.Validating += new System.ComponentModel.CancelEventHandler(this.tbClave_Validating);
            this.tbClave.Validated += new System.EventHandler(this.tbClave_Validated);
            // 
            // tbUsuario
            // 
            this.tbUsuario.Location = new System.Drawing.Point(62, 36);
            this.tbUsuario.Name = "tbUsuario";
            this.tbUsuario.Size = new System.Drawing.Size(89, 20);
            this.tbUsuario.TabIndex = 0;
            this.tbUsuario.Validating += new System.ComponentModel.CancelEventHandler(this.tbUsuario_Validating);
            this.tbUsuario.Validated += new System.EventHandler(this.tbUsuario_Validated);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // errorProvider2
            // 
            this.errorProvider2.ContainerControl = this;
            // 
            // WelcomeVisado
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(356, 159);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btn_Convenio);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "WelcomeVisado";
            this.Text = "Visado";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_Convenio;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbClave;
        private System.Windows.Forms.TextBox tbUsuario;
        private System.Windows.Forms.Button btn_Next;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.ErrorProvider errorProvider2;
    }
}

