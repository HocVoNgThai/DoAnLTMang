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
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            string data = args.Length > 0 ? args[0] : string.Empty;
            string newData = data.Replace("_", " ");
            MessageBox.Show($"You have a show \"{newData}\" to watch in 5 minutes!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
            // Kết thúc ứng dụng
            Application.Exit();

        }
    }
}
