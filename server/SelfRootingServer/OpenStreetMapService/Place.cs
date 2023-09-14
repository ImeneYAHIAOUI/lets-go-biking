using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace SelfRootingServer.OpenStreetMapService
{
    public class Place
    {
        public string lat { get; set; }
        public string lon { get; set; }
        public Address address { get; set; }

     
    }
}
