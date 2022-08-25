namespace BlockAndPass.ValetWinform
{
    partial class Valet
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Valet));
            this.btnHistory = new System.Windows.Forms.Button();
            this.btnNew = new System.Windows.Forms.Button();
            this.grvIngresados = new System.Windows.Forms.DataGridView();
            this.grvSaliendo = new System.Windows.Forms.DataGridView();
            this.lblUsuario = new System.Windows.Forms.Label();
            this.lblDocumento = new System.Windows.Forms.Label();
            this.lblEstacionamiento = new System.Windows.Forms.Label();
            this.lblSede = new System.Windows.Forms.Label();
            this.cbSede = new System.Windows.Forms.ComboBox();
            this.cbEstacionamiento = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.grvIngresados)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grvSaliendo)).BeginInit();
            this.SuspendLayout();
            // 
            // btnHistory
            // 
            this.btnHistory.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnHistory.BackgroundImage")));
            this.btnHistory.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnHistory.FlatAppearance.BorderSize = 0;
            this.btnHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHistory.Location = new System.Drawing.Point(778, 79);
            this.btnHistory.Name = "btnHistory";
            this.btnHistory.Size = new System.Drawing.Size(76, 61);
            this.btnHistory.TabIndex = 3;
            this.btnHistory.UseVisualStyleBackColor = true;
            // 
            // btnNew
            // 
            this.btnNew.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnNew.BackgroundImage")));
            this.btnNew.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnNew.FlatAppearance.BorderSize = 0;
            this.btnNew.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNew.Location = new System.Drawing.Point(696, 79);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(76, 61);
            this.btnNew.TabIndex = 4;
            this.btnNew.UseVisualStyleBackColor = true;
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // grvIngresados
            // 
            this.grvIngresados.AllowUserToAddRows = false;
            this.grvIngresados.AllowUserToDeleteRows = false;
            this.grvIngresados.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            this.grvIngresados.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grvIngresados.Location = new System.Drawing.Point(25, 162);
            this.grvIngresados.MultiSelect = false;
            this.grvIngresados.Name = "grvIngresados";
            this.grvIngresados.ReadOnly = true;
            this.grvIngresados.RowHeadersVisible = false;
            this.grvIngresados.Size = new System.Drawing.Size(401, 314);
            this.grvIngresados.TabIndex = 5;
            this.grvIngresados.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grvIngresados_CellClick);
            this.grvIngresados.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.grvIngresados_DataBindingComplete);
            // 
            // grvSaliendo
            // 
            this.grvSaliendo.AllowUserToAddRows = false;
            this.grvSaliendo.AllowUserToDeleteRows = false;
            this.grvSaliendo.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            this.grvSaliendo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grvSaliendo.Location = new System.Drawing.Point(442, 162);
            this.grvSaliendo.MultiSelect = false;
            this.grvSaliendo.Name = "grvSaliendo";
            this.grvSaliendo.ReadOnly = true;
            this.grvSaliendo.RowHeadersVisible = false;
            this.grvSaliendo.Size = new System.Drawing.Size(401, 314);
            this.grvSaliendo.TabIndex = 6;
            this.grvSaliendo.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grvSaliendo_CellClick);
            this.grvSaliendo.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.grvSaliendo_DataBindingComplete);
            // 
            // lblUsuario
            // 
            this.lblUsuario.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUsuario.Location = new System.Drawing.Point(13, 79);
            this.lblUsuario.Name = "lblUsuario";
            this.lblUsuario.Size = new System.Drawing.Size(406, 23);
            this.lblUsuario.TabIndex = 7;
            this.lblUsuario.Text = "Usuario:";
            // 
            // lblDocumento
            // 
            this.lblDocumento.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDocumento.Location = new System.Drawing.Point(13, 117);
            this.lblDocumento.Name = "lblDocumento";
            this.lblDocumento.Size = new System.Drawing.Size(406, 23);
            this.lblDocumento.TabIndex = 8;
            this.lblDocumento.Text = "Documento:";
            // 
            // lblEstacionamiento
            // 
            this.lblEstacionamiento.AutoSize = true;
            this.lblEstacionamiento.Location = new System.Drawing.Point(316, 14);
            this.lblEstacionamiento.Name = "lblEstacionamiento";
            this.lblEstacionamiento.Size = new System.Drawing.Size(85, 13);
            this.lblEstacionamiento.TabIndex = 12;
            this.lblEstacionamiento.Text = "Estacionamiento";
            // 
            // lblSede
            // 
            this.lblSede.AutoSize = true;
            this.lblSede.Location = new System.Drawing.Point(22, 14);
            this.lblSede.Name = "lblSede";
            this.lblSede.Size = new System.Drawing.Size(32, 13);
            this.lblSede.TabIndex = 11;
            this.lblSede.Text = "Sede";
            // 
            // cbSede
            // 
            this.cbSede.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSede.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbSede.FormattingEnabled = true;
            this.cbSede.Location = new System.Drawing.Point(25, 30);
            this.cbSede.Name = "cbSede";
            this.cbSede.Size = new System.Drawing.Size(255, 33);
            this.cbSede.TabIndex = 10;
            this.cbSede.SelectedIndexChanged += new System.EventHandler(this.cbSede_SelectedIndexChanged);
            // 
            // cbEstacionamiento
            // 
            this.cbEstacionamiento.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbEstacionamiento.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbEstacionamiento.FormattingEnabled = true;
            this.cbEstacionamiento.Location = new System.Drawing.Point(310, 30);
            this.cbEstacionamiento.Name = "cbEstacionamiento";
            this.cbEstacionamiento.Size = new System.Drawing.Size(255, 33);
            this.cbEstacionamiento.TabIndex = 9;
            this.cbEstacionamiento.SelectedIndexChanged += new System.EventHandler(this.cbEstacionamiento_SelectedIndexChanged);
            // 
            // Valet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(875, 524);
            this.Controls.Add(this.lblEstacionamiento);
            this.Controls.Add(this.lblSede);
            this.Controls.Add(this.cbSede);
            this.Controls.Add(this.cbEstacionamiento);
            this.Controls.Add(this.lblDocumento);
            this.Controls.Add(this.lblUsuario);
            this.Controls.Add(this.grvSaliendo);
            this.Controls.Add(this.grvIngresados);
            this.Controls.Add(this.btnNew);
            this.Controls.Add(this.btnHistory);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Valet";
            this.Text = "VALET";
            ((System.ComponentModel.ISupportInitialize)(this.grvIngresados)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grvSaliendo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnHistory;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.DataGridView grvIngresados;
        private System.Windows.Forms.DataGridView grvSaliendo;
        private System.Windows.Forms.Label lblUsuario;
        private System.Windows.Forms.Label lblDocumento;
        private System.Windows.Forms.Label lblEstacionamiento;
        private System.Windows.Forms.Label lblSede;
        private System.Windows.Forms.ComboBox cbSede;
        private System.Windows.Forms.ComboBox cbEstacionamiento;
    }
}