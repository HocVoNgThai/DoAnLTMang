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
using static DoAn.BetaCine;
using Microsoft.Win32.TaskScheduler;


namespace DoAn
{
    public partial class VTV12345 : Form
    {
        private HtmlAgilityPack.HtmlDocument document;
        List<Show> Shows1, Shows2, Shows3, Shows4, Shows5;
        private WebClient myClient;

        public class Show
        {
            public string Link_Uri { get; set; }
            public string Show_Title { get; set; }
            public string Show_Time { get; set; }
            public string Show_Genre { get; set; }
        }

        public VTV12345()
        {
            InitializeComponent();
            Load_Shows();
        }

        private void Load_Shows()
        {
            myClient = new WebClient();
            myClient.Encoding = System.Text.Encoding.UTF8;
            Uri uri = new Uri("https://vtv.vn/lich-phat-song.htm");
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

            PopulateChannelPanel(Shows1 = GetShowsForChannel(1), panel1);
            PopulateChannelPanel(Shows2 = GetShowsForChannel(2), panel2);
            PopulateChannelPanel(Shows3 = GetShowsForChannel(3), panel3);
            PopulateChannelPanel(Shows4 = GetShowsForChannel(4), panel4);
            PopulateChannelPanel(Shows5 = GetShowsForChannel(5), panel5);
        }

        private List<Show> GetShowsForChannel(int channelNumber)
        {
            List<Show> Shows = new List<Show>();
            var showTimeNodes = document.DocumentNode.SelectNodes($"//*[@id=\"wrapper\"]/ul[{channelNumber}]/li/span[1]");
            var showTitleNodes = document.DocumentNode.SelectNodes($"//*[@id=\"wrapper\"]/ul[{channelNumber}]/li/span[2]");
            var showGenreNodes = document.DocumentNode.SelectNodes($"//*[@id=\"wrapper\"]/ul[{channelNumber}]/li/a");

            if (showTimeNodes != null)
            {
                for (int i = 0; i < showTimeNodes.Count; i++)
                {
                    Shows.Add(new Show { Show_Time = showTimeNodes[i].InnerText });
                }
            }

            if (showTitleNodes != null)
            {
                for (int i = 0; i < showTitleNodes.Count; i++)
                {
                    Shows[i].Show_Title = showTitleNodes[i].InnerText;
                }
            }

            if (showGenreNodes != null)
            {
                int genreIndex = 0;
                for (int i = 0; i < Shows.Count; i++)
                {
                    if (genreIndex < showGenreNodes.Count && showGenreNodes[genreIndex].ParentNode.SelectSingleNode($"span[1]").InnerText == Shows[i].Show_Time)
                    {
                        Shows[i].Show_Genre = showGenreNodes[genreIndex].InnerText;
                        genreIndex++;
                    }
                    else
                    {
                        Shows[i].Show_Genre = "";
                    }
                }
            }

            // Ensure Show_Genre is not null
            foreach (var show in Shows)
            {
                if (string.IsNullOrWhiteSpace(show.Show_Genre))
                {
                    show.Show_Genre = "";
                }
            }

            return Shows;
        }

        private void PopulateChannelPanel(List<Show> Shows, Panel DisplayPanel)
        {
            DisplayPanel.Controls.Clear();
            int xPosition = 0;
            int i = 0;
            foreach (var show in Shows)
            {
                Panel panel = new Panel
                {
                    Width = 180, // Adjust width
                    Height = 70, // Adjust height
                    BorderStyle = BorderStyle.FixedSingle,
                    Location = new Point(xPosition, 0),
                    Margin = new Padding(5),
                };

                Label lblTime = new Label
                {
                    Text = show.Show_Time,
                    AutoSize = true,
                    Location = new Point(10, 10)
                };
                panel.Controls.Add(lblTime);

                Label lblTitle = new Label
                {
                    Text = show.Show_Title,
                    AutoSize = true,
                    Location = new Point(10, 30),
                    Font = new Font("Microsoft Sans Serif", 10.8F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)))
                };
                panel.Controls.Add(lblTitle);

                LinkLabel lblGenre = new LinkLabel
                {
                    Text = show.Show_Genre,
                    AutoSize = true,
                    Location = new Point(10, 50),
                    Font = new Font("Microsoft Sans Serif", 7.8F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)))
                };
                panel.Controls.Add(lblGenre);
                panel.DoubleClick += (sender, e) =>
                {
                    listView1.Items.Add(new ListViewItem
                        {
                            Text  = show.Show_Time + ":00",
                            SubItems = { show.Show_Title }
                        }
                    );
                };

                DateTime currentTime = DateTime.Now;
                if (IsCurrentTimeWithinShowTime(currentTime.ToString("HH:mm"), show.Show_Time, i < Shows.Count - 1 ? Shows[i + 1].Show_Time : null))
                {
                    panel.BackColor = Color.SkyBlue; // Thay đổi màu sắc của panel
                }

                DisplayPanel.Controls.Add(panel);
                xPosition += panel.Width + 10;
                i++;
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0 && listView1.SelectedItems[0].SubItems.Count > 1)
            {
                DateTime dateTime;
                if (DateTime.TryParse(listView1.SelectedItems[0].Text, out dateTime))
                {
                    string taskName = listView1.SelectedItems[0].SubItems[0].Text;
                    if (string.IsNullOrWhiteSpace(taskName))
                    {
                        MessageBox.Show("Invalid task name.");
                        return;
                    }

                    DateTime startTime = DateTime.Parse(listView1.SelectedItems[0].Text); // Schedule at least 1 minute in the future
                    MessageBox.Show("Task scheduled at " + startTime.ToString("yyyy-MM-dd HH:mm:ss"));
                    string executablePath = @"F:\Try to Code\HappyBirthdayProject\bin\Debug\HappyBirthdayProject.exe";

                    CreateScheduledTask(taskName, startTime, executablePath);
                }
                else
                {
                    MessageBox.Show("Invalid date format.");
                }
            }
            else
            {
                MessageBox.Show("Invalid selection.");
            }
        }

        public void CreateScheduledTask(string taskName, DateTime startTime, string executablePath)
        {
            using (TaskService ts = new TaskService())
            {
                TaskDefinition td = ts.NewTask();
                td.RegistrationInfo.Description = "TV Show Reminder";

                Trigger trigger = new TimeTrigger() { StartBoundary = startTime };
                td.Triggers.Add(trigger);

                td.Actions.Add(new ExecAction(executablePath, null, null));

                ts.RootFolder.RegisterTaskDefinition(@"test", td);
                //taskName must be ASCII, no space, no special characters. If your language is not English, you should convert it to ASCII.
                //keyword is: SanitizeTaskName
            }
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
    }
}
