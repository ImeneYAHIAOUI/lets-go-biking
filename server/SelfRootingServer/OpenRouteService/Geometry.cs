using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SelfRootingServer.OpenRouteService
{
    [DataContract]
    public class Geometry
    {
        [DataMember]
        public string type { get; set; }
        [DataMember]
        public List<List<double>> coordinates { get; set; }
    }
}