using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ProxyAndCache.JCDecauxItems;
namespace ProxyAndCache
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom de classe "Service1" à la fois dans le code et le fichier de configuration.
    public class Proxy : IProxy
    {
        private GenericProxyCache cache = new GenericProxyCache();
        public string GetContracts()
        {
            ContractList contracts = cache.Get<ContractList>("contracts", 3600);
            return contracts.GetContracts();

        }

        public string GetStations(string contractName)
        {
            Regex g = new Regex(@"s\b");
            MatchCollection matches = g.Matches(contractName);
            string suffix = "'s";
            if (matches.Count > 0)
            {
                suffix = "'";
            }
            StationList stations = cache.Get<StationList>(contractName + suffix + " stations", 60, contractName);
            return stations.GetStations();
        }

        public string GetAllStations()
        {

            StationList stations = cache.Get<StationList>("stations");
            return stations.GetStations();
        }
    }
}

