using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SelfRootingServer.OpenRouteService
{
    [DataContract]
    public class Step
    {
        [DataMember]
        public Double  distance { get; set; } // double
        [DataMember]
        public Double duration { get; set; } // double
        [DataMember]
        public string name { get; set; }    
        [DataMember]
        public string instruction { get; set; }
    }
}
