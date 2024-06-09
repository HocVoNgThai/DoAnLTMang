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

namespace DoAn
{
    public partial class THVL : Form
    {
        private HtmlAgilityPack.HtmlDocument document;
        List<Show> Shows1, Shows2, Shows3, Shows4, Shows5;
        private WebClient myClient;

        public THVL()
        {
            InitializeComponent();
            Load_Image();
            Load_ShowsTHVL1();
            Load_ShowsTHVL2();
            Load_ShowsTHVL3();
            Load_ShowsTHVL4();
        }

        public class Show
        {
            public string Show_Title { get; set; }
            public string Show_Time { get; set; }
        }

        private void TruyenHinh_Load(object sender, EventArgs e)
        {

        }

        public async void Load_Image()
        {
            string url = "https://cdn.brvn.vn/topics/400px/2016/2270_THVL.png";

            try
            {   
                        PictureBox pictureBox = new PictureBox();
                        pictureBox.ImageLocation = url;
                        pictureBox.Size = new Size(385, 139);
                        pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                        flowLayoutPanel1.Controls.Add(pictureBox);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error!!" + ex.Message);
            }
        }

        private void Load_ShowsTHVL1()
        {
            myClient = new WebClient();
            myClient.Encoding = System.Text.Encoding.UTF8;
            // Lấy ngày hiện tại và định dạng theo yyyy-MM-dd
            string currentDate = DateTime.Now.ToString("yyyy-MM-dd");

            Uri uri = new Uri($"https://thvl.vn/lich-phat-song/?ngay={currentDate}&kenh=THVL1");
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

            PopulateChannelPanel(Shows1 = GetShowsForChannel(1), pnTHVL1);
        }

        private void Load_ShowsTHVL2()
        {
            myClient = new WebClient();
            myClient.Encoding = System.Text.Encoding.UTF8;
            // Lấy ngày hiện tại và định dạng theo yyyy-MM-dd
            string currentDate = DateTime.Now.ToString("yyyy-MM-dd");

            Uri uri = new Uri($"https://thvl.vn/lich-phat-song/?ngay={currentDate}&kenh=THVL2");
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

            PopulateChannelPanel(Shows2 = GetShowsForChannel(2), pnTHVL2);
        }

        private void Load_ShowsTHVL3()
        {
            myClient = new WebClient();
            myClient.Encoding = System.Text.Encoding.UTF8;
            // Lấy ngày hiện tại và định dạng theo yyyy-MM-dd
            string currentDate = DateTime.Now.ToString("yyyy-MM-dd");

            Uri uri = new Uri($"https://thvl.vn/lich-phat-song/?ngay={currentDate}&kenh=THVL3");
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

            PopulateChannelPanel(Shows3 = GetShowsForChannel(3), pnTHVL3);
        }

        private void Load_ShowsTHVL4()
        {
            myClient = new WebClient();
            myClient.Encoding = System.Text.Encoding.UTF8;
            // Lấy ngày hiện tại và định dạng theo yyyy-MM-dd
            string currentDate = DateTime.Now.ToString("yyyy-MM-dd");

            Uri uri = new Uri($"https://thvl.vn/lich-phat-song/?ngay={currentDate}&kenh=THVL4");
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

            PopulateChannelPanel(Shows4 = GetShowsForChannel(4), pnTHVL4);
        }

        private List<Show> GetShowsForChannel(int channelNumber)
        {
            List<Show> Shows = new List<Show>();
            var showNodes = document.DocumentNode.SelectNodes($"//*[@class=\"table\"]/div[@class=\"tbl-row\"]");

            if (showNodes != null)
            {
                foreach (var showNode in showNodes)
                {
                    var timeNode = showNode.SelectSingleNode(".//div[@class=\"time\"]");
                    var titleNode = showNode.SelectSingleNode(".//div[@class=\"program\"]");

                    if (timeNode != null && titleNode != null)
                    {
                        Shows.Add(new Show
                        {
                            Show_Time = timeNode.InnerText.Trim(),
                            Show_Title = titleNode.InnerText.Trim(),   
                        });
                    }
                }
            }

            return Shows;
        }

        private void PopulateChannelPanel(List<Show> Shows, Panel DisplayPanel)
        {
            DisplayPanel.Controls.Clear();
            DisplayPanel.AutoScroll = true; // Enable scrolling
            int yPosition = 0;

            // Lấy thời gian hiện tại của máy tính
            DateTime currentTime = DateTime.Now;
            //MessageBox.Show(DateTime.Now.ToString("HH:mm"));

            for (int i = 0; i < Shows.Count; i++)
            {
                var show = Shows[i];
                Panel panel = new Panel
                {
                    Width = DisplayPanel.Width - 25, // Adjust width to fit within the display panel and account for scroll bar
                    Height = 90, // Adjust height
                    BorderStyle = BorderStyle.FixedSingle,
                    Location = new Point(0, yPosition),
                    Margin = new Padding(5),
                };

                Label lblTime = new Label
                {
                    Text = show.Show_Time,
                    AutoSize = true,
                    Location = new Point(10, 10),
                    Font = new Font("Microsoft Sans Serif", 8.8F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)))
                };
                panel.Controls.Add(lblTime);

                Label lblTitle = new Label
                {
                    Text = show.Show_Title,
                    AutoSize = true,
                    Location = new Point(10, 35),
                    Font = new Font("Microsoft Sans Serif", 10.8F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)))
                };
                panel.Controls.Add(lblTitle);
                panel.DoubleClick += (sender, e) =>
                {
                    if (DateTime.Parse(show.Show_Time) < DateTime.Now)
                    {
                        MessageBox.Show("This show has already aired.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (DateTime.Parse(show.Show_Time) < DateTime.Now.AddMinutes(5))
                    {
                        MessageBox.Show("This show is about to start.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        if (MessageBox.Show("Are you sure you want to schedule notifications for this program?", "Recheck", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                            == DialogResult.Yes)
                        {
                            string executablePath = Path.Combine(Application.StartupPath, "Notifications.exe");
                            CreateScheduledTask(show.Show_Title, DateTime.Parse(show.Show_Time).AddMinutes(-5), executablePath);
                            MessageBox.Show("Scheduled task created successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }

                };

                //MessageBox.Show(show.Show_Time,IsCurrentTimeWithinShowTime(currentTime, show.Show_Time, i < Shows.Count - 1 ? Shows[i + 1].Show_Time : null).ToString());

                // So sánh thời gian hiện tại với thời gian của show
                if (IsCurrentTimeWithinShowTime(currentTime.ToString("HH:mm"), show.Show_Time, i < Shows.Count - 1 ? Shows[i + 1].Show_Time : null))
                {
                    panel.BackColor = Color.Turquoise; // Thay đổi màu sắc của panel
                }

                DisplayPanel.Controls.Add(panel);
                yPosition += panel.Height + 10; // Adjust the yPosition for the next panel
            }

            //MessageBox.Show(IsCurrentTimeWithinShowTime(currentTime.ToString("HH:mm"), "15:00", "15:50").ToString());

        }

        // Phương thức kiểm tra xem thời gian hiện tại có nằm trong khung giờ của show không
        private bool IsCurrentTimeWithinShowTime(string currentTime, string showStartTime, string showEndTime)
        {
            // Định dạng thời gian của show, giả sử định dạng là "HH:mm"
            string[] formats = { "HH:mm", "H:mm" };

            // Chuyển đổi chuỗi thời gian show thành đối tượng DateTime
            if (DateTime.TryParseExact(showStartTime, formats, null, System.Globalization.DateTimeStyles.None, out DateTime start) && DateTime.TryParseExact(currentTime, formats, null, System.Globalization.DateTimeStyles.None, out DateTime current))
            {
                // Kiểm tra nếu có thời gian kết thúc
                if (showEndTime != null && DateTime.TryParseExact(showEndTime, formats, null, System.Globalization.DateTimeStyles.None, out DateTime end))
                {
                    // Kiểm tra xem thời gian hiện tại có nằm trong khoảng thời gian của show không
                    return current.TimeOfDay >= start.TimeOfDay && current.TimeOfDay < end.TimeOfDay;
                }
                else
                {
                    // Nếu không có thời gian kết thúc (chương trình cuối cùng trong ngày), chỉ so sánh thời gian bắt đầu
                    // Hãy thay đổi return current >= start; thành return current == start; nếu muốn so sánh chính xác với thời gian bắt đầu.
                    return current.TimeOfDay >= start.TimeOfDay;
                }
            }

            // Nếu không chuyển đổi được, trả về false
            return false;
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
                    EndBoundary = startTime.AddMinutes(10) // Set EndBoundary to 10 minutes after StartBoundary
                };
                td.Triggers.Add(trigger);
                td.Settings.DeleteExpiredTaskAfter = TimeSpan.FromMinutes(15); // Delete the task 10 minutes after it has run

                // Create an action that will launch the specified executable
                td.Actions.Add(new ExecAction(executablePath, null, null));
                td.Principal.UserId = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                td.Principal.LogonType = TaskLogonType.InteractiveToken;
                td.Principal.RunLevel = TaskRunLevel.Highest;
                // Register the task in the root folder
                string TName = "TV Show Reminder " + DateTime.Now.ToString("MdyyyyHmmss");
                ts.RootFolder.RegisterTaskDefinition(TName, td);
            }
        }
    }
}

