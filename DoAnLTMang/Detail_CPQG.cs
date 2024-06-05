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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DoAn
{
    public partial class Detail_CPQG : Form
    {
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
            lbThoiLuong.Text = film.Duration;
            Console.WriteLine(film.VideoUrl);
            string video_Url=film.VideoUrl;
            MessageBox.Show("Connect successful");
            if (webView21 != null && webView21.CoreWebView2 != null)
            {
                webView21.CoreWebView2.Navigate(video_Url);
            }
        }
    }
}
