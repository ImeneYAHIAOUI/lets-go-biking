using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SelfRootingServer.OpenRouteService
{

    [DataContract]
    public class Itinerary
    {

        [DataMember] public Double distance { get; set; } // double
        [DataMember]  public Double duration { get; set; }
        [DataMember] public List<Step> steps { get; set; }
        [DataMember] public Geometry geometry { get; set; }

        [DataMember] public Profile profile { get; set; }

    }
}
