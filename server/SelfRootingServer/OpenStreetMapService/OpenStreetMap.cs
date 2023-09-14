using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace SelfRootingServer.OpenStreetMapService
{
    public class OpenStreetMap
    {
        public OpenStreetMap()
        {

        }
        private static HttpClient client = new HttpClient();
        public  async Task<Place> GetPlaceInformation(string adress)
        {
            client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (compatible; AcmeInc/2.0)");
            string url = "https://nominatim.openstreetmap.org/search?q=";
            adress = adress.Replace(" ", "+");
            adress = adress.Replace(",", "%2C");
            url += adress + "&format=json&addressdetails=1";
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            List <Place> places = JsonSerializer.Deserialize<List<Place>>(responseBody);
            Place place = places!=null && places.Count>0 ? places[0] : null;
            return place;
        }
        public async Task<string> GetAdress(Double longi,Double lat)
        {
            client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (compatible; AcmeInc/2.0)");
            string url = "https://nominatim.openstreetmap.org/reverse?format=json&lat=";
            url += lat.ToString().Replace(',', '.') + "&lon="+ longi.ToString().Replace(',', '.');
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            JsonDocument jo = JsonSerializer.Deserialize<JsonDocument>(responseBody);
            var features = jo.RootElement.GetProperty("display_name");
            string adress = JsonSerializer.Deserialize<string>(features);
            adress.Replace(",", "");
            return adress;
        }
    }
}
