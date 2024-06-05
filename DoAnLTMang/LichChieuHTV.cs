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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace DoAn
{
    public partial class LichChieuHTV : Form
    {
        public LichChieuHTV()
        {
            InitializeComponent();
            // Console.OutputEncoding = Encoding.UTF8;

            listViewHTV1.View = View.Details;
            listViewHTV2.View = View.Details;
            listViewHTV7.View = View.Details;
            listViewHTV9.View = View.Details;

            tabControl1.TabPages[0].Text = "HTV1";
            tabControl1.TabPages[1].Text = "HTV2";
            tabControl1.TabPages[2].Text = "HTV7";
            tabControl1.TabPages[3].Text = "HTV9";
            Crawl_LichPhatSong("https://www.htv.com.vn/lich-phat-song?channel=11", listViewHTV1);
            Crawl_LichPhatSong("https://www.htv.com.vn/lich-phat-song?channel=12", listViewHTV2);
            Crawl_LichPhatSong("https://www.htv.com.vn/lich-phat-song?channel=1", listViewHTV7);
            Crawl_LichPhatSong("https://www.htv.com.vn/lich-phat-song?channel=3", listViewHTV9);


        }

        public void Crawl_LichPhatSong(string url, System.Windows.Forms.ListView lstview)
        {
            using (WebClient client = new WebClient())
            {

                client.Encoding = Encoding.UTF8;
                client.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/125.0.0.0 Safari/537.36");
                HtmlDocument htmlDoc = new HtmlDocument();
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

                                ListViewItem item = new ListViewItem(time);
                                item.SubItems.Add(name);
                                lstview.Items.Add(item);

                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Không thể kết nối đến website");
                }
            }
        }

    }
}
