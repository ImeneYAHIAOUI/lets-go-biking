using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace SelfRootingServer.JCDecaux
{
    [DataContract]
    public class Contract
    {
        [DataMember] public string name { get; set; }
        [DataMember] public string commercial_name { get; set; }
        [DataMember] public string country_code { get; set; }
        [DataMember] public List<string> cities { get; set; }
        
        


        
        

       
       override public string ToString()
        {
            return name;
        }



    }
}
