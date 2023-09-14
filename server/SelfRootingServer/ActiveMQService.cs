using System;
using System.Collections.Generic;
using System.Net.Http;
using Apache.NMS;
using Apache.NMS.ActiveMQ;
using SelfRootingServer.JCDecaux;
using SelfRootingServer.OpenRouteService;
using SelfRootingServer.OpenStreetMapService;

namespace SelfRootingServer
{
    class ActiveMQService
    {
        public static void sendMessage(string queue, string message)
        {
            // Create a Connection Factory.
            Uri connecturi = new Uri("activemq:tcp://localhost:61616");
            ConnectionFactory connectionFactory = new ConnectionFactory(connecturi);

            // Create a single Connection from the Connection Factory.
            IConnection connection = connectionFactory.CreateConnection();
            connection.Start();

            // Create a session from the Connection.
            ISession session = connection.CreateSession();

            // Use the session to target a queue.
            IDestination destination = session.GetQueue(queue);

            // Create a Producer targetting the selected queue.
            IMessageProducer producer = session.CreateProducer(destination);

            // You may configure everything to your needs, for instance:
            producer.DeliveryMode = MsgDeliveryMode.Persistent;

            ITextMessage msg = session.CreateTextMessage(message);
            producer.Send(msg);

            session.Close();
            connection.Close();
        }
      
        public static void SendItinerariesToTheMQ(List<Itinerary> itineraries, string queue)
        {
            if (itineraries.Count == 0) {
                sendMessage(queue, "Error");

            }
            else if (itineraries.Count == 1)
            {
                foreach (Itinerary itinerary in itineraries)
                {
                    List<Step> steps = itinerary.steps;
                    foreach (Step step in steps)
                    {
                        string message = "go straight for " +
                            step.distance + " m and then " +
                            step.instruction;
                        sendMessage(queue, message);
                    }
                }
            }
            else 
            {
                Itinerary itinerary=itineraries[0];
                List<Step> steps = itinerary.steps;
                foreach (Step step in steps)
                {
                    string message = "go straight for " +
                        step.distance + " m and then " +
                        step.instruction;
                    sendMessage(queue, message);
                }

            }
           

        }

    }
}
