using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Notifications
{
    public partial class Form1 : Form
    {
        public Form1(string[] args)
        {
            InitializeComponent();
            if (args.Length > 0)
            {
                string message = string.Join(" ", args);
                MessageBox.Show(message, "TV Show Reminder", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("No information provided.", "TV Show Reminder", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
