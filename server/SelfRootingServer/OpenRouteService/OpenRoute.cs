using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Nodes;
using System.Data;
namespace SelfRootingServer.OpenRouteService
{
    public class OpenRoute
    {
        private static HttpClient client = new HttpClient();

        private static  readonly string Api= "5b3ce3597851110001cf624887ebb8442e28402189146b6bca8f353c";
        public OpenRoute() { 
        }
        
        public async Task<List<Itinerary>> getItineraries(GeoCoordinate departure, GeoCoordinate destination, Profile profile)
        {
            string url = "https://api.openrouteservice.org/v2/directions/"; 

            if (profile == Profile.Cycle)
            {
                url += "cycling-road";
            }
            else
            {
                url += "foot-walking";
            }
            List<Itinerary> itineraries=new List<Itinerary>();
            if(departure!=null && destination != null)
            {
                url += "?api_key=" + Api + "&start=" + departure.Longitude.ToString().Replace(',', '.') + "," +
                    departure.Latitude.ToString().Replace(',', '.') +
                "&end=" + destination.Longitude.ToString().Replace(',', '.') + "," + 
                destination.Latitude.ToString().Replace(',', '.');
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                JsonDocument jo = JsonSerializer.Deserialize<JsonDocument>(responseBody);
                var features = jo.RootElement.GetProperty("features");
                Geometry geometry = JsonSerializer.Deserialize<Geometry>( features.EnumerateArray().Select(tk => tk.GetProperty("geometry")).ToList()[0]);
                var properties = features.EnumerateArray().Select(tk => tk.GetProperty("properties")).ToList()[0];
                var segments = properties.GetProperty("segments");
                itineraries = JsonSerializer.Deserialize<List<Itinerary>>(segments);
                itineraries.ForEach(itinerary => itinerary.geometry = geometry);
            }
            return itineraries;
        }



    }
}
