using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.Schema;
using static DoAn.CPQG;
using static System.Net.WebRequestMethods;
using Newtonsoft.Json;

namespace DoAn
{
    public partial class CPQG : Form
    {
        private HtmlAgilityPack.HtmlDocument document;
        List<Film> films;
        private WebClient myClient;
        private List<dynamic> list_detail_json_film = new List<dynamic>();

        public class Film
        {
            public string Link_Img { get; set; }
            public string Link_Uri { get; set; }
            public string Film_Name { get; set; }

            public int Id { get; set; }
        }

        /*class Detail
        {
            public string Id { get; set; }
            public string FilmName { get; set; }
        }
        */
        public CPQG()
        {
            InitializeComponent();
            Load_Movies();
        }

        private void Load_Movies()
        {
            myClient = new WebClient();
            myClient.Encoding = System.Text.Encoding.UTF8;
            Uri uri = new Uri("https://chieuphimquocgia.com.vn/");
            Stream replied = myClient.OpenRead(uri);


            string html = myClient.DownloadString(uri);
            document = new HtmlAgilityPack.HtmlDocument();
            document.LoadHtml(html);


            replied.Close();
            myClient.Dispose();

            string patt = @"\{\\""SharingFilms\\"":.*?\\""ProjectTime\\"":null}";
            // Tìm kiếm và trích xuất chuỗi 
            // [\"$\",\"$L17\",null,{\"data\":
            // Console.WriteLine(html.IndexOf(patt));
            MatchCollection temps = Regex.Matches(html, patt);
            foreach (Match match in temps)
            {
                string temp = match.Value;
                temp = temp.Replace("\\\\", "---");
                temp = temp.Replace("\\", string.Empty);
                temp = temp.Replace("---", "\\");
                temp = temp.Replace("\\", "\\" + "\\\\");
                Console.WriteLine(temp);
                dynamic detail = JsonConvert.DeserializeObject(temp);
                list_detail_json_film.Add(detail);
                Console.WriteLine(detail.Id);
            }

            // Initialize list_films
            films = new List<Film>();
            string link_img = "https://chieuphimquocgia.com.vn";
            if (document == null) return;
            var film_List = document.DocumentNode.SelectNodes("/html/body/div[5]/div/div/div/div[1]/div[1]/div/div/div/div/img");
            film_List.ToList().ForEach(film =>
            {
                string input = film.Attributes["src"].Value;
                string pattern = @"/_next.*?" + "g&amp";
                pattern = pattern.Substring(0, pattern.Length - 3);
                MatchCollection matches = Regex.Matches(input, pattern);
                films.Add(new Film { Link_Img = link_img + matches[0].Value + "&w=384&q=75" });
            });

            film_List = document.DocumentNode.SelectNodes("/html/body/div[5]/div/div/div/div[1]/div[1]/div/div/div/div[2]");
            var temp_list = film_List.ToList();
            for (int i = 0; i < temp_list.Count; i++)
            {
                films[i].Film_Name = temp_list[i].InnerText;

                foreach (dynamic json in list_detail_json_film)
                {
                    if (json.FilmName == films[i].Film_Name)
                    {
                        films[i].Id = json.Id;
                        films[i].Link_Uri = json.Href2;
                    }
                }
                Console.WriteLine(films[i].Link_Img);
                //films[i].Link_Uri = "https://betacinemas.vn" + temp_list[i].Attributes["href"].Value;
            }
            film_List.Clear();

            for (int i = 0; i < films.Count; i++)
            {
                // 
                // Pbx_MoviePoster
                //
                PictureBox Pbx_MoviePoster = new PictureBox();
                Pbx_MoviePoster.Location = new System.Drawing.Point(0, 0);
                Pbx_MoviePoster.Name = "pbx" + films[i].Id.ToString();
                Pbx_MoviePoster.Size = new System.Drawing.Size(315, 406);
                Pbx_MoviePoster.TabIndex = films[i].Id;
                Pbx_MoviePoster.TabStop = false;
                Pbx_MoviePoster.ImageLocation = films[i].Link_Img;
                Pbx_MoviePoster.SizeMode = PictureBoxSizeMode.StretchImage;
                Pbx_MoviePoster.Click += (sender, e) =>
                {
                    PictureBox pbx = sender as PictureBox;
                    foreach (dynamic json in list_detail_json_film)
                    {
                        if (json.Id == pbx.TabIndex)
                        {
                            Detail_CPQG form = new Detail_CPQG(json);
                            form.Show();
                            form = null;

                            break;
                        }
                    }
                };
                // 
                // Label_MovieName
                //
                Label Label_MovieName = new Label();
                Label_MovieName.AutoSize = true;
                Label_MovieName.Location = new System.Drawing.Point(0, 409);
                Label_MovieName.MaximumSize = new System.Drawing.Size(314, 75);
                Label_MovieName.Name = "lbl" + films[i].Id.ToString();
                Label_MovieName.Size = new System.Drawing.Size(286, 42);
                Label_MovieName.TabIndex = films[i].Id;
                Label_MovieName.Text = films[i].Film_Name;
                Label_MovieName.Click += (sender, e) =>
                {
                    Label lbl = sender as Label;
                    foreach (dynamic json in list_detail_json_film)
                    {
                        if (json.Id == lbl.TabIndex)
                        {
                            Detail_CPQG form = new Detail_CPQG(json);
                            form.Show();
                            form = null;

                            break;
                        }
                    }

                };

                Panel Panel_Carousel = new Panel();
                Panel_Carousel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                Panel_Carousel.Controls.Add(Label_MovieName);
                Panel_Carousel.Controls.Add(Pbx_MoviePoster);
                Panel_Carousel.Location = new System.Drawing.Point(200 + 55 * (i % 3 - 1) + 315 * (i % 3), 40 + 20 * (i / 3 - 1) + 478 * (i / 3));
                Panel_Carousel.Name = films[i].Id.ToString();
                Panel_Carousel.Size = new System.Drawing.Size(315, 478);
                Panel_Carousel.TabIndex = films[i].Id;
                Panel_Carousel.Click += (sender, e) =>
                {
                    Panel pnl = sender as Panel;
                    foreach (dynamic json in list_detail_json_film)
                    {
                        if (json.Id == pnl.TabIndex)
                        {
                            Detail_CPQG form = new Detail_CPQG(json);
                            form.Show();
                            form = null;

                            break;
                        }
                    }
                };
                this.Panel_Outer.Controls.Add(Panel_Carousel);

            }
        }
    }
}
