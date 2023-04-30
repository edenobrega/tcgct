using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTG.Classes
{
    internal class BulkResponse
    {
        public List<BulkItem> data { get; set; }

        public void Get()
        {
            string url = "https://api.scryfall.com";
            var options = new RestClientOptions(url);
            var client = new RestClient(options);
            var request = new RestRequest("/bulk-data");
            // The cancellation token comes from the caller. You can still make a call without it.
            var response = client.Get(request);
            data = JsonConvert.DeserializeObject<BulkResponse>(response.Content).data;
        }
    }

    internal class BulkItem
    {
        public string content_encoding { get; set; }
        public string content_type { get; set; }
        public string description { get; set; }
        public Uri download_uri { get; set; }
        public string ID { get; set; }
        public string Name { get; set; }
        public string @object { get; set; }
        public int size { get; set; }
        public string type { get; set; }
        public string updated_at { get; set; }
        public Uri uri { get; set; }
    }
}
