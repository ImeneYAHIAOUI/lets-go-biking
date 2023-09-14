using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SelfRootingServer
{
    public class InvalidAdress : Exception
    {
        public InvalidAdress(string message) : base(message)
        {}
    }
}
