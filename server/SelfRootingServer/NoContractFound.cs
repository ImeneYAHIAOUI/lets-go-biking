using System;
using System.Runtime.Serialization;

namespace SelfRootingServer
{
    [Serializable]
    internal class NoContractFound : Exception
    {
        public NoContractFound()
        { }

        public NoContractFound(string message) : base(message)
        { }
        public NoContractFound(string message, Exception innerException) : base(message, innerException)
        {}
        protected NoContractFound(SerializationInfo info, StreamingContext context) : base(info, context)
        {}
    }
}