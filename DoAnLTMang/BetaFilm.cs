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

                    };
                    tabPg.Controls.Add(btn);
                }

                this.tabCtrl.Controls.Add(tabPg);
            }
            //}
        }
    }
}
