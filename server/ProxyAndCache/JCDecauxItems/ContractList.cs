using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace ProxyAndCache.JCDecauxItems
{

    [DataContract]
    public class ContractList : JCDecauxItem
    {
        private string contracts;
        public ContractList() : base()
        {
            string url = "https://api.jcdecaux.com/vls/v1/contracts" + "?apiKey=" + APIkey;
            Task<string> contractTask = SendRequestAsync(url);
            contracts = contractTask.Result;


        }

        public string GetContracts()
        {
            return this.contracts;
        }


    }
}
