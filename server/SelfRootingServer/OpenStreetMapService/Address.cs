using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfRootingServer.OpenStreetMapService
{
    
    public class Address
    {
        //"address":{"building":"Résidence Villa Nador","house_number":"61",
        //"road":"Boulevard du Président Wilson","suburb":"Juan-les-Pins","town":"Antibes",
        //"municipality":"Grasse","county":"Alpes-Maritimes","ISO3166-2-lvl6":"FR-06",
        //"state":"Provence-Alpes-Côte d'Azur","ISO3166-2-lvl4":"FR-PAC"
        //,"region":"France métropolitaine","postcode":"06600","country":"France","country_code":"fr"}
        public string house_number { get; set; }
        public string road { get; set; }
        public string suburb { get; set; }
        public string town { get; set; }
        public string city { get; set; }
        public string municipality { get; set; }
        public string county { get; set; }
        public string postcode { get; set; }
        public string state { get; set; }
        public string country { get; set; }


    }
}
