namespace UndertaleBattleSystemPrototype
{
    partial class BattleScreen
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
            this.textOutput = new System.Windows.Forms.Label();
            this.actLabel1 = new System.Windows.Forms.Label();
            this.actLabel2 = new System.Windows.Forms.Label();
            this.actLabel3 = new System.Windows.Forms.Label();
            this.actLabel4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // gameTimer
            // 
            this.gameTimer.Enabled = true;
            this.gameTimer.Interval = 20;
            this.gameTimer.Tick += new System.EventHandler(this.gameTimer_Tick);
            // 
            // textOutput
            // 
            this.textOutput.AutoSize = true;
            this.textOutput.BackColor = System.Drawing.Color.Transparent;
            this.textOutput.Enabled = false;
            this.textOutput.Font = new System.Drawing.Font("Determination Mono Web", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textOutput.ForeColor = System.Drawing.Color.White;
            this.textOutput.Location = new System.Drawing.Point(60, 360);
            this.textOutput.Name = "textOutput";
            this.textOutput.Size = new System.Drawing.Size(735, 140);
            this.textOutput.TabIndex = 0;
            this.textOutput.Text = "* Hello. \r\n\r\n* Welcome to the Undetale Battle System Prototype.\r\n\r\n* Try some of " +
    "the buttons and menus out for testing.";
            // 
            // actLabel1
            // 
            this.actLabel1.AutoSize = true;
            this.actLabel1.BackColor = System.Drawing.Color.Transparent;
            this.actLabel1.Enabled = false;
            this.actLabel1.Font = new System.Drawing.Font("Determination Mono Web", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.actLabel1.ForeColor = System.Drawing.Color.White;
            this.actLabel1.Location = new System.Drawing.Point(60, 360);
            this.actLabel1.Name = "actLabel1";
            this.actLabel1.Size = new System.Drawing.Size(68, 28);
            this.actLabel1.TabIndex = 1;
            this.actLabel1.Text = "Act1";
            this.actLabel1.Visible = false;
            // 
            // actLabel2
            // 
            this.actLabel2.AutoSize = true;
            this.actLabel2.BackColor = System.Drawing.Color.Transparent;
            this.actLabel2.Enabled = false;
            this.actLabel2.Font = new System.Drawing.Font("Determination Mono Web", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.actLabel2.ForeColor = System.Drawing.Color.White;
            this.actLabel2.Location = new System.Drawing.Point(450, 360);
            this.actLabel2.Name = "actLabel2";
            this.actLabel2.Size = new System.Drawing.Size(68, 28);
            this.actLabel2.TabIndex = 2;
            this.actLabel2.Text = "Act2";
            this.actLabel2.Visible = false;
            // 
            // actLabel3
            // 
            this.actLabel3.AutoSize = true;
            this.actLabel3.BackColor = System.Drawing.Color.Transparent;
            this.actLabel3.Enabled = false;
            this.actLabel3.Font = new System.Drawing.Font("Determination Mono Web", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.actLabel3.ForeColor = System.Drawing.Color.White;
            this.actLabel3.Location = new System.Drawing.Point(60, 447);
            this.actLabel3.Name = "actLabel3";
            this.actLabel3.Size = new System.Drawing.Size(68, 28);
            this.actLabel3.TabIndex = 3;
            this.actLabel3.Text = "Act3";
            this.actLabel3.Visible = false;
            // 
            // actLabel4
            // 
            this.actLabel4.AutoSize = true;
            this.actLabel4.BackColor = System.Drawing.Color.Transparent;
            this.actLabel4.Enabled = false;
            this.actLabel4.Font = new System.Drawing.Font("Determination Mono Web", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.actLabel4.ForeColor = System.Drawing.Color.White;
            this.actLabel4.Location = new System.Drawing.Point(450, 447);
            this.actLabel4.Name = "actLabel4";
            this.actLabel4.Size = new System.Drawing.Size(68, 28);
            this.actLabel4.TabIndex = 4;
            this.actLabel4.Text = "Act4";
            this.actLabel4.Visible = false;
            // 
            // BattleScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.actLabel4);
            this.Controls.Add(this.actLabel3);
            this.Controls.Add(this.actLabel2);
            this.Controls.Add(this.actLabel1);
            this.Controls.Add(this.textOutput);
            this.DoubleBuffered = true;
            this.Name = "BattleScreen";
            this.Size = new System.Drawing.Size(944, 681);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.BattleScreen_Paint);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.BattleScreen_KeyUp);
            this.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.BattleScreen_PreviewKeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer gameTimer;
        private System.Windows.Forms.Label textOutput;
        private System.Windows.Forms.Label actLabel1;
        private System.Windows.Forms.Label actLabel2;
        private System.Windows.Forms.Label actLabel3;
        private System.Windows.Forms.Label actLabel4;
    }
}
