using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Notifications
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            /*Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1(args));*/
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
