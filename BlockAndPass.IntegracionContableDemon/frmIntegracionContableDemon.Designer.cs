namespace BlockAndPass.IntegracionContableDemon
{
    partial class frmIntegracionContableDemon
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmIntegracionContableDemon));
            this.mynotifyicon = new System.Windows.Forms.NotifyIcon(this.components);
            this.lbEventos = new System.Windows.Forms.ListBox();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnCerrarServicio = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // mynotifyicon
            // 
            this.mynotifyicon.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.mynotifyicon.BalloonTipText = "Minimizado Demonio de Actualizacion";
            this.mynotifyicon.BalloonTipTitle = "Demonio Integracion Contable";
            this.mynotifyicon.Icon = ((System.Drawing.Icon)(resources.GetObject("mynotifyicon.Icon")));
            this.mynotifyicon.Text = "Integracion Contable";
            this.mynotifyicon.Visible = true;
            this.mynotifyicon.DoubleClick += new System.EventHandler(this.mynotifyicon_DoubleClick);
            // 
            // lbEventos
            // 
            this.lbEventos.FormattingEnabled = true;
            this.lbEventos.Location = new System.Drawing.Point(12, 12);
            this.lbEventos.Name = "lbEventos";
            this.lbEventos.Size = new System.Drawing.Size(489, 212);
            this.lbEventos.TabIndex = 0;
            // 
            // btnStop
            // 
            this.btnStop.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStop.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnStop.Location = new System.Drawing.Point(198, 238);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(118, 23);
            this.btnStop.TabIndex = 1;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = false;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnStart
            // 
            this.btnStart.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnStart.Enabled = false;
            this.btnStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStart.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnStart.Location = new System.Drawing.Point(385, 238);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(118, 23);
            this.btnStart.TabIndex = 2;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnCerrarServicio
            // 
            this.btnCerrarServicio.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnCerrarServicio.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCerrarServicio.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnCerrarServicio.Location = new System.Drawing.Point(11, 238);
            this.btnCerrarServicio.Name = "btnCerrarServicio";
            this.btnCerrarServicio.Size = new System.Drawing.Size(118, 23);
            this.btnCerrarServicio.TabIndex = 3;
            this.btnCerrarServicio.Text = "Cerrar";
            this.btnCerrarServicio.UseVisualStyleBackColor = false;
            this.btnCerrarServicio.Click += new System.EventHandler(this.btnCerrarServicio_Click);
            // 
            // frmIntegracionContableDemon
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(513, 273);
            this.Controls.Add(this.btnCerrarServicio);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.lbEventos);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmIntegracionContableDemon";
            this.ShowInTaskbar = false;
            this.Text = "Demonio Integracion Contable";
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmPrincipalUpdateDemon_FormClosing);
            this.Resize += new System.EventHandler(this.frmPrincipalUpdateDemon_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NotifyIcon mynotifyicon;
        private System.Windows.Forms.ListBox lbEventos;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnCerrarServicio;
    }
}