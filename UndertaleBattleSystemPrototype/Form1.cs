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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //bring up the battle screen
            BattleScreen bs = new BattleScreen();
            this.Controls.Add(bs);

            //focus on the battle system for user input
            bs.Focus();
        }
    }
}
