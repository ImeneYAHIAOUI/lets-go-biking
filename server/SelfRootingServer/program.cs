using System;

using System.ServiceModel.Description;
using System.ServiceModel;

using static SelfRootingServer.LetsGoBiking;
using SelfRootingServer.OpenRouteService;
using System.Collections.Generic;

namespace SelfRootingServer
{
    internal class Program
    {
        static void Main(string[] args)
        {

            LetsGoBiking letsGoBiking = new LetsGoBiking();
            letsGoBiking.GetItinerary("paris", "nice");
            Uri httpUrl = new Uri("http://localhost:8090/SelfRootingServer/LetsGoBiking");
                //Create ServiceHost
                ServiceHost host = new ServiceHost(typeof(LetsGoBiking), httpUrl);

                // Multiple end points can be added to the Service using AddServiceEndpoint() method.
                // Host.Open() will run the service, so that it can be used by any client.

                // Example adding :
                // Uri tcpUrl = new Uri("net.tcp://localhost:8090/MyService/SimpleCalculator");
                // ServiceHost host = new ServiceHost(typeof(MyCalculatorService.SimpleCalculator), httpUrl, tcpUrl);

                //Add a service endpoint
                host.AddServiceEndpoint(typeof(ILetsGoBiking), new BasicHttpBinding(), "");
                //Enable metadata exchange
                ServiceMetadataBehavior smb = new ServiceMetadataBehavior();
                smb.HttpGetEnabled = true;
                host.Description.Behaviors.Add(smb);
                //Start the Service
                host.Open();
                Console.WriteLine("Service is hosted since " + DateTime.Now.ToString());
                Console.WriteLine("Host is running... Press <Enter> key to stop");
                //List<Itinerary> list = new LetsGoBiking().GetItinerary("61 boulevard du président wilson 06600 Antibes", "100 boulevard du président wilson 06600 Antibes");
                //List<Itinerary> list = new LetsGoBiking().GetItinerary("Nice", "Paris");
                Console.ReadLine();
        }
    }
}


