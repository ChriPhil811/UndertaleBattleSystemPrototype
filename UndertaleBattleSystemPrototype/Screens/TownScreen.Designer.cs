namespace UndertaleBattleSystemPrototype
{
    partial class TownScreen
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.gameTimer = new System.Windows.Forms.Timer(this.components);
            this.textLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // gameTimer
            // 
            this.gameTimer.Enabled = true;
            this.gameTimer.Interval = 20;
            this.gameTimer.Tick += new System.EventHandler(this.gameTimer_Tick);
            // 
            // textLabel
            // 
            this.textLabel.BackColor = System.Drawing.Color.Transparent;
            this.textLabel.ForeColor = System.Drawing.SystemColors.ActiveBorder;
            this.textLabel.Location = new System.Drawing.Point(157, 625);
            this.textLabel.Name = "textLabel";
            this.textLabel.Size = new System.Drawing.Size(910, 165);
            this.textLabel.TabIndex = 0;
            this.textLabel.Text = "label1";
            this.textLabel.Visible = false;
            // 
            // TownScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.textLabel);
            this.DoubleBuffered = true;
            this.Name = "TownScreen";
            this.Size = new System.Drawing.Size(1277, 885);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.TownScreen_Paint);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TownScreen_KeyUp);
            this.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.TownScreen_PreviewKeyDown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer gameTimer;
        private System.Windows.Forms.Label textLabel;
    }
}
