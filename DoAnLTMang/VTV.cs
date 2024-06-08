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
    public partial class VTV : Form
    {
        private HtmlAgilityPack.HtmlDocument document;
        List<Show> Shows1, Shows2, Shows3, Shows4, Shows5;
        private WebClient myClient;

        public class Show
        {
            public string Show_Title { get; set; }
            public string Show_Time { get; set; }
            public string Show_Genre { get; set; }
        }

        public VTV()
        {
            InitializeComponent();
            Load_Shows();
            Load_Image();
        }
        public async void Load_Image()
        {
            string url = "https://public-web.vtvdigital.vn/public/images/logo.png";

            try
            {
                pbLoGo.ImageLocation = "https://img-cache.coccoc.com/image2?i=1&l=74/90879272";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error!!" + ex.Message);
            }
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

            PopulateChannelPanel(Shows1 = GetShowsForChannel(1), pnVTV1);
            PopulateChannelPanel(Shows2 = GetShowsForChannel(2), pnVTV2);
            PopulateChannelPanel(Shows3 = GetShowsForChannel(3), pnVTV3);
            PopulateChannelPanel(Shows4 = GetShowsForChannel(4), pnVTV4);
            PopulateChannelPanel(Shows5 = GetShowsForChannel(5), pnVTV5);
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

                LinkLabel lblGenre = new LinkLabel
                {
                    Text = show.Show_Genre,
                    AutoSize = true,
                    Location = new Point(10, 65),
                    Font = new Font("Microsoft Sans Serif", 7.8F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)))
                };
                panel.Controls.Add(lblGenre);

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
    }
}
