using SelfRootingServer.OpenStreetMapService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SelfRootingServer.JCDecaux
{
    [DataContract]
    public class Position
    {
        [DataMember]
        public double lat { get; set; }
        [DataMember]
        public double lng { get; set; }
        public override bool Equals(Object obj)
        {
            //Check for null and compare run-time types.
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                Position p = (Position)obj;
                return (lat == p.lat) && (lng == p.lng);
            }
        }
        override
        public string ToString()
        {
            return lng +","+ lat;
        }

    }
}
