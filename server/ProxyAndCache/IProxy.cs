using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Text.Json.Nodes;
using ProxyAndCache.JCDecauxItems;

namespace ProxyAndCache
{
    // REMARQUE : vous pouvez utiliser la commande Renommer du menu Refactoriser pour changer le nom d'interface "IService1" à la fois dans le code et le fichier de configuration.
    [ServiceContract]
    public interface IProxy
    {
        [OperationContract]
        string GetContracts();

        [OperationContract]
        string GetStations(string contractName);
        [OperationContract]
        string GetAllStations();



    }

}