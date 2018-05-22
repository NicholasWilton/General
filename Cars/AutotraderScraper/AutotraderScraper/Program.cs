using HtmlAgilityPack;
using ScrapySharp.Extensions;
using ScrapySharp.Network;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutotraderScraper
{
    class Program
    {
        static void Main(string[] args)
        {
            var b = new ScrapingBrowser();

            WebPage PageResult = b.NavigateToPage(new Uri("https://www.autotrader.co.uk/car-search?sort=price-desc&radius=1500&postcode=e148dw&onesearchad=Used&onesearchad=Nearly%20New&onesearchad=New&make=NISSAN&model=JUKE&aggregatedTrim=Nismo%20RS&page=1"));
            //HtmlNode countNode = PageResult.Html.SelectNodes("//html/body/main/section[2]/div[1]/header/nav/ul/li[3]").FirstOrDefault();
            var nodes = PageResult.Html.SelectNodes("//*[@id=\"main-content\"]/div[1]/header/nav/ul/li[3]");

            //*[@id="main-content"]/div[1]/header/nav/ul/li[3]
            string count = nodes.First().InnerText;
            int current = int.Parse(count.Split(' ')[1]);
            int total = int.Parse(count.Split(' ')[3]);


            using (var csv = File.AppendText(@".\cars.csv"))
            {
                int id = 0;
                csv.WriteLine("id,description,price,mileage,year,transmission,size,power,fuel");
                for (int page = 2; page <= total; page++)
                {
                    
                    var results = PageResult.Html.SelectNodes("//html/body/main/section[2]/div[1]/ul/li");

                    foreach (var result in results)
                    {
                        if (result.InnerText.StartsWith("\nFeatrured"))
                            continue;

                        if (result.SelectNodes("article/section[1]/div") == null)
                            continue;

                        var also = result.SelectNodes("span");
                        if (also != null && also.First().InnerText.Contains("also like"))
                            continue;

                        var info = result.SelectNodes("article/section[1]/div").First();
                        var price = result.SelectNodes("article/section[2]/a/div").First().InnerText.Trim('?').Replace(",", "");
                        string description = info.SelectNodes("h2/a").First().InnerText;
                        var subInfo = info.SelectNodes("ul/li");
                        string mileage = subInfo[2].InnerText.Replace(" miles", "").Replace(",", "");
                        string year = subInfo[0].InnerText.Substring(0, 4);
                        string transmission = subInfo[3].InnerText;
                        string size = subInfo[4].InnerText.Replace("L", "");
                        string power = subInfo[5].InnerText.Replace("bhp", "");
                        string fuel = subInfo[6].InnerText;
                        csv.WriteLine($"{id},{description},{price},{mileage},{year},{transmission},{size},{power},{fuel}");
                        id++;
                    }
                    PageResult = b.NavigateToPage(new Uri($"https://www.autotrader.co.uk/car-search?sort=price-desc&radius=1500&postcode=e148dw&onesearchad=Used&onesearchad=Nearly%20New&onesearchad=New&make=NISSAN&model=JUKE&aggregatedTrim=Nismo%20RS&page={page}"));
                }

               
            }
        }
    }
}
