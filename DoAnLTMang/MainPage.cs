using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoAn
{
    public partial class MainPage : Form
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void btnVTV_Click(object sender, EventArgs e)
        {
            VTV vTV = new VTV();
            vTV.ShowDialog();
        }

        private void btnHTV_Click(object sender, EventArgs e)
        {
            HTV hTV = new HTV();
            hTV.ShowDialog();
        }

        private void btnTHVL_Click(object sender, EventArgs e)
        {
            THVL tHVL = new THVL();
            tHVL.ShowDialog();
        }

        private void btnBETA_Click(object sender, EventArgs e)
        {
            BetaCine betaCine = new BetaCine();
            betaCine.ShowDialog();
        }

        private void btnNCC_Click(object sender, EventArgs e)
        {
            CPQG cPQG = new CPQG(); 
            cPQG.ShowDialog();
        }
    }
}
