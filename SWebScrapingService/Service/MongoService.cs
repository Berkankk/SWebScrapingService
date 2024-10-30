using MongoDB.Driver;
using SWebScrapingService.DTOS;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWebScrapingService.Service
{
    public class MongoService //MongoDb ile iletişim kurmak için gerekli olan sınıfımızı yazdık 
    {
        private readonly IMongoCollection<HeadlineDto> _collection; //Mongodb ye headline ı gönderiyoruz

        public MongoService(string connectionString, string databaseName, string collectionName) //Gerekli parametreleri yazdık 
        {
            var client = new MongoClient(connectionString); //Mongodb clientı oluşturduk :)
            var database = client.GetDatabase(databaseName); //Belirttiğim veritabanına bağlan dedik
            _collection = database.GetCollection<HeadlineDto>(collectionName); //Dto türünde erişim sağla dtoda olana göre 

        }

        public async Task SaveHeadlinesAsync(List<HeadlineDto> headlines) //Gelelim mongoya kaydetmeye 
        {
            if (headlines == null || headlines.Count == 0) //Boş mu null mı 
            {
                throw new ArgumentException("İstek atmalısınız", nameof(headlines)); //Boşsun hata 
            }

            var bulkOps = new List<WriteModel<HeadlineDto>>(); //Sana bir liste yazdım yazmak için 

            foreach (var headline in headlines)
            {
                bulkOps.Add(new InsertOneModel<HeadlineDto>(headline));
                // Her başlık için yeni model oluştur ve bunu bulksop listesine ekle 
            }

            await _collection.BulkWriteAsync(bulkOps);
            //performans açısında mongo db kullandık ve bunu da mongo db ye kaydet hadi bakalım
        }


    }
}
