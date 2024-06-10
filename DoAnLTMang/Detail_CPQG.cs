using Microsoft.Win32.TaskScheduler;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection.Emit;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace DoAn
{
    public partial class Detail_CPQG : Form
    {
        private HtmlAgilityPack.HtmlDocument document;
        private WebClient myClient;
        dynamic film;
        public Detail_CPQG(dynamic film)
        {
            InitializeComponent();
            this.film = film;
            Load_Movie();
        }
        private void Load_Movie()
        {
            pictureBox1.ImageLocation = film.ImageUrl;
            label1.Text = film.FilmName;
            label2.Text = film.Introduction;
            lbDaoDien.Text = film.Director;
            lbDienVien.Text = film.Actors;
            lbTheLoai.Text = film.Category;
            lbNgonNgu.Text = film.CountryName;
            lbThoiLuong.Text = film.Duration + " phút";
            Console.WriteLine(film.VideoUrl);
            string video_Url = film.VideoUrl;
            MessageBox.Show("Connect successful");
            try
            {
                if (webView21 != null && webView21.CoreWebView2 != null && !string.IsNullOrEmpty(video_Url))
                {
                    webView21.CoreWebView2.Navigate(video_Url);
                }
            }
            catch { }

            string Link_Uri = "https://chieuphimquocgia.com.vn/movies/" + film.Id;
            Console.WriteLine(Link_Uri);
            myClient = new WebClient();
            myClient.Encoding = System.Text.Encoding.UTF8;
            Uri uri = new Uri(Link_Uri);
            try
            {
                Stream replied = myClient.OpenRead(uri);
                string html = myClient.DownloadString(uri);
                document = new HtmlAgilityPack.HtmlDocument();
                document.LoadHtml(html);
                replied.Close();
                myClient.Dispose();
            }
            catch
            {
                MessageBox.Show("Lỗi kết nối", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (document == null) return;


            // Get Dates chiếu
            var Dates = document.DocumentNode.SelectNodes("/html/body/div[5]/div[3]/div[1]/button/div[1]/p");
            //Check schedule

            var listDate = Dates.ToList();
            String Date = "";
            Date += Dates[2].InnerText.ToString() + " - " + Dates[1].InnerText.ToString() + " - " + Dates[0].InnerText.ToString();
            //for(int i=0; i<dateFilms.Count;i++)
            //{

            //Load lịch chiếu theo ngày
            var TimeSche = document.DocumentNode.SelectNodes("/html/body/div[5]/div[3]/div[3]/div/div/div/button/p");
            if (TimeSche == null) { MessageBox.Show("Hôm nay không có lịch chiếu!"); return; }
            var times = TimeSche.ToList();

            // Add tabPage
            TabPage tabPg = new TabPage();
            tabPg.Text = Date;
            for (int j = 0; j < times.Count; j++)
            {
                Button btn = new Button();
                btn.Location = new System.Drawing.Point(40 + 118 * (j % 5), 0 + 40 * (j / 5) + 20);
                btn.Name = "btnDatLich";
                btn.Size = new System.Drawing.Size(110, 40);
                btn.TabIndex = 17;
                btn.Text = times[j].InnerText;
                btn.UseVisualStyleBackColor = true;
                btn.Click += (sender, e) =>
                {
                    if (DateTime.Parse(btn.Text) < DateTime.Now)
                    {
                        MessageBox.Show("This show has already aired.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (DateTime.Parse(btn.Text) < DateTime.Now.AddMinutes(5))
                    {
                        MessageBox.Show("This show is about to start.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else 
                    {
                        if (MessageBox.Show("Are you sure you want to schedule notifications for this program?", "Recheck", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                            == DialogResult.Yes)
                        {
                            string executablePath = Path.Combine(Application.StartupPath, "Notifications.exe");
                            CreateScheduledTask(label1.Text, DateTime.Parse(btn.Text).AddMinutes(-5), executablePath);
                            MessageBox.Show("Scheduled task created successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                };
                tabPg.Controls.Add(btn);
            }

            this.tabCtrl.Controls.Add(tabPg);
        }

        public static void CreateScheduledTask(string taskName, DateTime startTime, string executablePath)
        {
            using (TaskService ts = new TaskService())
            {
                TaskDefinition td = ts.NewTask();
                td.RegistrationInfo.Description = "The program you booked: " + taskName + " is about to premiere.";

                // Create a trigger that will fire the task at the specified time
                TimeTrigger trigger = new TimeTrigger()
                {
                    StartBoundary = startTime,
                    EndBoundary = startTime.AddMinutes(15) // Set EndBoundary to 10 minutes after StartBoundary
                };
                td.Triggers.Add(trigger);
                td.Settings.DeleteExpiredTaskAfter = TimeSpan.FromMinutes(15); // Delete the task 10 minutes after it has run
                ExecAction action = new ExecAction();
                action.Path = executablePath;
                string modifiedTaskName = Regex.Replace(taskName, @"\s+", "_");

                action.Arguments = modifiedTaskName;
                // Create an action that will launch the specified executable
                td.Actions.Add(action);
                td.Principal.UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                td.Principal.LogonType = TaskLogonType.InteractiveToken;
                td.Principal.RunLevel = TaskRunLevel.Highest;
                // Register the task in the root folder
                string TName = "Show Reminder " + DateTime.Now.ToString("MdyyyyHmmss");
                ts.RootFolder.RegisterTaskDefinition(TName, td);
            }
        }
    }
}
