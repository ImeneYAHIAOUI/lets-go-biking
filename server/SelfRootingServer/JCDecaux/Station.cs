using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using SelfRootingServer.JCDecaux;

namespace SelfRootingServer.JCDecaux
{
    [DataContract]
    public class Station
    {

        [DataMember] public string name { get; set; }
        [DataMember] public string address { get; set; }
        [DataMember] public bool banking { get; set; }
        [DataMember] public bool bonus { get; set; }
        [DataMember] public int bike_stands { get; set; }
        [DataMember] public int available_bike_stands { get; set; }
        [DataMember] public int available_bikes { get; set; }
        [DataMember] public string status { get; set; }
        [DataMember] public Position position { get; set; }
        [DataMember]public string contract_name { get; set; }
        [DataMember] public int number { get; set; }



        
        override
        public string ToString()
        {
            return address+ position;
        }

        


        }
    }
