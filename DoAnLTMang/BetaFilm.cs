using HtmlAgilityPack;
using Microsoft.Win32.TaskScheduler;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Net.WebRequestMethods;

namespace DoAn
{
    public partial class BetaFilm : Form
    {
        private HtmlAgilityPack.HtmlDocument document;
        private WebClient myClient;
        string linkMovie;
        public BetaFilm(string linkFilm)
        {
            InitializeComponent();
            linkMovie = linkFilm;
            Load_Movie();
        }
        private void Load_Movie()
        {
            myClient = new WebClient();
            myClient.Encoding = System.Text.Encoding.UTF8;
            Uri uri = new Uri($"{linkMovie}");
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
            var image = document.DocumentNode.SelectSingleNode("//*[@id=\"ctl00\"]/div/div/div/div/img");
            pictureBox1.ImageLocation = image.Attributes["src"].Value;
            var nameFilm = document.DocumentNode.SelectSingleNode("//*[@id=\"ctl00\"]/div/div/div/h1");
            label1.Text = nameFilm.InnerText;
            var script = document.DocumentNode.SelectSingleNode("//*[@id=\"ctl00\"]/div/div/div/p");
            label2.Text = script.InnerText;
            var listData = document.DocumentNode.SelectNodes("//*[@id=\"ctl00\"]/div/div/div/div/div");
            var list = listData.ToList();
            lbDaoDien.Text = list[1].InnerText;
            lbDienVien.Text = list[3].InnerText;
            lbTheLoai.Text = list[5].InnerText;
            lbNgonNgu.Text = list[7].InnerText;
            lbThoiLuong.Text = list[9].InnerText;
            var trailer = document.DocumentNode.SelectSingleNode("//*[@id=\"ctl00\"]/div/div/div/div/iframe");
            var linkTrailer = trailer.Attributes["src"].Value;
            MessageBox.Show("Connect successful");
            if (webView21 != null && webView21.CoreWebView2 != null && !string.IsNullOrEmpty(linkTrailer))
            {
                webView21.CoreWebView2.Navigate(linkTrailer);
            }

            ////*[@id=\"tab-content\"]/div
            var Schedule = document.DocumentNode.SelectNodes("//*[@id=\"ctl00\"]/div[1]/div[2]/div/ul/li/a");

            //Check schedule
            if (Schedule == null) return;
            var dateFilms = Schedule.ToList();
            //for(int i=0; i<dateFilms.Count;i++)
            //{

            //Check Date
            var idDate = Schedule[0].Attributes["href"].Value;
            idDate = idDate.Substring(1);
            string Date = Schedule[0].InnerText;
            var timeFilm = document.DocumentNode.SelectNodes($"//*[@id=\"{idDate}\"]/div/div/a");
            foreach (var tmp in timeFilm) Console.WriteLine(tmp.InnerHtml);
            if (timeFilm != null)
            {
                var times = timeFilm.ToList();

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
                                MessageBox.Show(executablePath);
                                string programInfo = "BetaFilm cenima:\n" + label1.Text + " at " + btn.Text;
                                CreateScheduledTask(label1.Text, DateTime.Parse(btn.Text).AddMinutes(-5), executablePath, programInfo);
                                MessageBox.Show("Scheduled task created successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    };
                    tabPg.Controls.Add(btn);
                }

                this.tabCtrl.Controls.Add(tabPg);
            }
            //}
        }

        public static void CreateScheduledTask(string taskName, DateTime startTime, string executablePath, string programInfo)
        {
            using (TaskService ts = new TaskService())
            {
                TaskDefinition td = ts.NewTask();
                td.RegistrationInfo.Description = "The program you booked: " + taskName + " is about to premiere.";

                // Create a trigger that will fire the task at the specified time
                TimeTrigger trigger = new TimeTrigger()
                {
                    StartBoundary = startTime,
                    EndBoundary = startTime.AddMinutes(10) // Set EndBoundary to 10 minutes after StartBoundary
                };
                td.Triggers.Add(trigger);

                // Create an action that will launch the specified executable
                td.Actions.Add(new ExecAction(executablePath, programInfo, null));
                td.Settings.DeleteExpiredTaskAfter = TimeSpan.FromMinutes(10); // Delete the task 1 minute after it has run

                // Register the task in the root folder
                string TName = "TV Show Reminder " + Convert.ToString(DateTime.Today.ToShortDateString());
                TName = TName.Replace("/", "-");
                ts.RootFolder.RegisterTaskDefinition(TName, td);
            }
        }
    }
}
