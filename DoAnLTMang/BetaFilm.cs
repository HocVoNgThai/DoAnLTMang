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
            if (webView21 != null && webView21.CoreWebView2 != null)
            {
                webView21.CoreWebView2.Navigate(linkTrailer);
            }
        }
    }
}
