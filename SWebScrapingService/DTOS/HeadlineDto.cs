namespace SWebScrapingService.DTOS
{
    public class HeadlineDto
    {
        public string Title { get; set; } //Başlık olmazsa olmaz her sitede vardır 
        public string Url { get; set; } //Başlığı hangi sayfadan alındığını görmek için var 
        public DateTime ScrapedAt { get; set; } //Scraping yaptığımız zaman 

        public string Description { get; set; }

        //public string[] AltTitle  { get; set; }

        public string ImageUrl { get; set; }
    }
}
//Burası veritabanına kaydedilecek verileri temsil eder 