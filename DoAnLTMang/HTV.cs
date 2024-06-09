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

namespace DoAn
{
    public partial class HTV : Form
    {
        public class Show
        {
            public string Show_Title { get; set; }
            public string Show_Time { get; set; }

            public Show(string name, string time)
            {
                Show_Title = name;
                Show_Time = time;
            }
        }

        public HTV()
        {
            InitializeComponent();
            // Console.OutputEncoding = Encoding.UTF8;



            tbHTVChanel.TabPages[0].Text = "HTV1";
            tbHTVChanel.TabPages[1].Text = "HTV2";
            tbHTVChanel.TabPages[2].Text = "HTV7";
            tbHTVChanel.TabPages[3].Text = "HTV9";
            Crawl_LichPhatSong("https://www.htv.com.vn/lich-phat-song?channel=11", pnHTV1);
            Crawl_LichPhatSong("https://www.htv.com.vn/lich-phat-song?channel=12", pbHTV2);
            Crawl_LichPhatSong("https://www.htv.com.vn/lich-phat-song?channel=1", pnHTV7);
            Crawl_LichPhatSong("https://www.htv.com.vn/lich-phat-song?channel=3", pbHTV9);
            


        }

        public void Crawl_LichPhatSong(string url, System.Windows.Forms.Panel panel)
        {
            List<Show> ShowTruyenHinh = new List<Show>();
            using (WebClient client = new WebClient())
            {

                client.Encoding = Encoding.UTF8;
                client.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/125.0.0.0 Safari/537.36");
                HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
                try
                {

                    var html = client.DownloadString(url);

                    htmlDoc.LoadHtml(html);

                    client.Dispose();
                }
                catch (Exception e)
                {
                    MessageBox.Show("Không thể kết nối đến website");
                    return;
                }

                var calendar_list = htmlDoc.DocumentNode.SelectSingleNode("//div[contains(@class, 'calendar-list')]");
                HtmlNode htmlNode = htmlDoc.DocumentNode.SelectSingleNode("//div[contains(@class, 'calendar-list')]");
                HtmlNodeCollection parentNode = htmlNode.ChildNodes;
                if (parentNode != null)
                {
                    foreach (HtmlNode node in parentNode)
                    {

                        HtmlNodeCollection childNodes = node.ChildNodes;
                        if (childNodes == null)
                        {
                            continue;
                        }

                        foreach (HtmlNode childNode in childNodes)
                        {
                            if (childNode.NodeType == HtmlNodeType.Element)
                            {

                                //Console.WriteLine(childNode.InnerText.Trim());
                                string[] parts = childNode.InnerText.Trim().Split('\n', '\r', '\t');
                                string time = parts[0];
                                string name = parts[parts.Length - 1];

                                Show s = new Show(name, time);
                                ShowTruyenHinh.Add(s);
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Không thể kết nối đến website");
                }
                PopulateChannelPanel(ShowTruyenHinh, panel);
            }
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
                    else
                    {
                        if (MessageBox.Show("Are you sure you want to schedule notifications for this program?", "Recheck", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                            == DialogResult.Yes)
                        {
                            string executablePath = Path.Combine(Application.StartupPath, "Notifications.exe");
                            string programInfo = "HTV chanel:\n" + lblTitle.Text + " at " + lblTime;
                            CreateScheduledTask(show.Show_Title, DateTime.Parse(show.Show_Time).AddMinutes(-5), executablePath, programInfo);
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

        private void LichChieuHTV_Load(object sender, EventArgs e)
        {

        }

        public static void CreateScheduledTask(string taskName, DateTime startTime, string executablePath, string programInfo)
        {
            using (TaskService ts = new TaskService())
            {
                TaskDefinition td = ts.NewTask();
                td.RegistrationInfo.Description = "The program you booked: " + taskName + " is about to premiere.";

                // Create a trigger that will fire the task at the specified time
                Trigger trigger = new TimeTrigger { StartBoundary = startTime };
                td.Triggers.Add(trigger);

                // Create an action that will launch the specified executable
                td.Actions.Add(new ExecAction(executablePath, programInfo, null));

                // Register the task in the root folder
                ts.RootFolder.RegisterTaskDefinition("TV Show Reminder", td);
            }
        }
    }
}
