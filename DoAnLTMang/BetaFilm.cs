using HtmlAgilityPack;
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
using static DoAn.BetaCine;
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
            var checkDateFilm = document.DocumentNode.SelectSingleNode("//*[@id=\"tab-content\"]/div");
            var idDate = checkDateFilm.Attributes["id"].Value;
            var timeFilm = document.DocumentNode.SelectNodes($"//*[@id=\"{idDate}\"]/div/div/a");
            if (timeFilm != null)
            {
                var times = timeFilm.ToList();
                List<string> lstTime = new List<string>();
                foreach (HtmlNode time in times)
                {
                    lstTime.Add(time.InnerText);
                }
                for (int i = 0; i < lstTime.Count; i++)
                {
                    Button button1 = new Button();
                    button1.Location = new System.Drawing.Point(227 + 118 * (i % 5) + 50, 528 + 40 * (i / 5) + 20);
                    button1.Name = "btnDatLich";
                    button1.Size = new System.Drawing.Size(110, 40);
                    button1.TabIndex = 17;
                    button1.Text = lstTime[i];
                    button1.UseVisualStyleBackColor = true;
                    button1.Click += (sender, e) =>
                    {

                    };
                    this.Controls.Add(button1);
                }
            }
        }

        
    }
}
