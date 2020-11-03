using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UndertaleBattleSystemPrototype
{
    public partial class Pause : Form
    {
        Boolean escapeDown = false;
        private static Pause pauseForm;
        private static DialogResult buttonResult = new DialogResult();


        public Pause()
        {
            InitializeComponent();
        }
        public static DialogResult Show()
        {
            pauseForm = new Pause();
            pauseForm.StartPosition = FormStartPosition.CenterParent;

            pauseForm.ShowDialog();
            return buttonResult;
        }

        private void Pause_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    escapeDown = true;
                    break;

            }
        }
        private void Pause_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    escapeDown = false;
                    break;

            }
        }

        private void pauseTimer_Tick(object sender, EventArgs e)
        {
            if (escapeDown == true)
            {
                TownScreen ts = new TownScreen();
                Form form = this.FindForm();

                ts.Location = new Point((form.Width - ts.Width) / 2, (form.Height - ts.Height) / 2);
                form.Controls.Add(ts);
                form.Controls.Remove(this);
                ts.Focus();
            }
        }
    }
}
