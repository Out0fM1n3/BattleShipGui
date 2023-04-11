using System;
using System.Windows.Forms;

namespace BattleShipGui
{
    public partial class StartWindow : Form
    {
        public StartWindow()
        {
            InitializeComponent();
        }

        private void PlayBtn_Click(object sender, EventArgs e)
        {
            this.Hide();
            new Editor().Show();
        }
    }
}
