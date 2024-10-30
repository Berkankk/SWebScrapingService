using HtmlAgilityPack;
using SWebScrapingService.DTOS;

namespace SWebScrapingService.Service
{
    public class ScrapingService  //Burası web scraping için gerekli olanları yazacağız
    {
        private readonly HttpClient _httpClient; //İStek oluşturduk bakalım

        public ScrapingService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<HeadlineDto>> ScrapeHeadlinesAsync(string url) //url ile scraping işlemi yapacağız mesela gs sitesinin url i gibi
        {
            var response = await _httpClient.GetStringAsync(url);//url e istek attık(string olarak) e gel bunu response a kaydet
            var htmlDoc = new HtmlDocument(); //Bu html için kullanılır içeriği anlamak için kullanılır 
            htmlDoc.LoadHtml(response);

            var nodes = htmlDoc.DocumentNode.SelectNodes("//h2");
            var imageNodes = htmlDoc.DocumentNode.SelectNodes("//img"); //Tüm img etiketlerini al
            //string imageUrl = imageNodes != null && imageNodes.Count > 0 ? imageNodes.First().GetAttributeValue("src", string.Empty) : null; 
            //ilk image etiketini al
            //Bu HTML belgesindeki tüm <h2> etiketlerini seçer ve bunları nodes değişkenine atar, belirli elemanlar seçilir.

            var headlines = new List<HeadlineDto>(); //Dto türmüzde başlıkları tutacak liste oluşturur

            if (nodes != null && nodes.Count > 0)  //Null mı en az bir elemanı var mı buna bakarız 
            {
                foreach (var node in nodes)
                {
                    var parent = node.ParentNode; //Başlık için üst öğeyi alalım
                    var imageNode = parent.SelectSingleNode(".//img"); //üst öğenin içinde ki image i al sürekli aynı image dönme bana

                    //string[] alttitleArray = new string[] { "Alt Başlık 1", "Alt Başlık 2" };
                    // Burada alttitle dizisini oluşturabiliriz. Örneğin, birkaç alt başlık tanımlıyoruz.

                    string imageUrl = imageNode != null ? imageNode.GetAttributeValue("src", string.Empty) : null;
                    headlines.Add(new HeadlineDto //Heeerrr bir başlık için yeni bir dto oluşturulur postmande gördük 
                    {
                        Title = node.InnerText.Trim(),
                        Url = url,
                        ScrapedAt = DateTime.Now,
                        ImageUrl = imageUrl,
                        Description = node.InnerText.Trim(),
                        //AltTitle = alttitleArray,

                    });
                }
            }

            return headlines;
        }

    }
}
