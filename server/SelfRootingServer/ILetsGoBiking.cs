using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Remoting.Messaging;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Device.Location;
using System.Text.Json.Nodes;
using System.ServiceModel;
using SelfRootingServer.JCDecaux;
using SelfRootingServer.OpenRouteService;

namespace SelfRootingServer
{
    [ServiceContract]
    public interface ILetsGoBiking
    {
        [OperationContract]
        List<Itinerary> GetItinerary(string departure, string destination);
        
        [OperationContract]
        List<Itinerary> UdateItinerary(string departure, string destination);

        [OperationContract]
        String GetItineraryOntheMQ(string departure, string destination);


        [OperationContract]
        String UdateItineraryOnTheMQ(string departure, string destination);

    }


}
