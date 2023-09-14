using ProxyAndCache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace ProxyAndCache.JCDecauxItems

{
    [DataContract]
    public class StationList : JCDecauxItem

    {
        private string stations;
        public StationList(string contractName) : base()
        {
            string url = "https://api.jcdecaux.com/vls/v1/stations?contract=" + contractName + "&apiKey=" + APIkey;
            BuildStationList(url);
        }

        public StationList() : base()
        {
            string url = "https://api.jcdecaux.com/vls/v1/stations?apiKey=" + APIkey;
            BuildStationList(url);

        }

        public void BuildStationList(string url)
        {
            Task<string> stationsTask = SendRequestAsync(url);
            this.stations = stationsTask.Result;
            /* List<JsonObject> stationsJson = stationsTask.Result;
             foreach (JsonObject station in stationsJson)
             {
                 stations.Add(new Station(station));
             }*/
        }

        public string GetStations()
        {
            return this.stations;
        }


    }
}
