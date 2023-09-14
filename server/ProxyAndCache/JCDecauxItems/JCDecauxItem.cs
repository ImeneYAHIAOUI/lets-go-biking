using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Collections;
using System.Text.Json.Nodes;
using System.Runtime.Serialization;
using ProxyAndCache.JCDecauxItems;

namespace ProxyAndCache
{
    [DataContract]
    public class JCDecauxItem
    {

        protected string APIkey = "cb1189911097c968b8bfb2205b90188a5b614f90";

        static readonly HttpClient client = new HttpClient();



        public async Task<string> SendRequestAsync(string Url)
        {
            HttpResponseMessage response = await client.GetAsync(Url);

            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();


            return responseBody;


        }

    }


}
