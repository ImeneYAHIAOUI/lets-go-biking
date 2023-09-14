using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Threading.Tasks;
using SelfRootingServer.OpenStreetMapService;
using SelfRootingServer.JCDecaux;
using System.Text.Json;
using Contract = SelfRootingServer.JCDecaux.Contract;
using SelfRootingServer.OpenRouteService;
using System.Net.Http;
using System.Linq;
using System.ServiceModel;
using System.IO;

namespace SelfRootingServer
{

    [ServiceBehavior(IncludeExceptionDetailInFaults = true)]
    public class LetsGoBiking : ILetsGoBiking
    {

        readonly Proxy.ProxyClient proxyClient = new Proxy.ProxyClient();
        List<Itinerary> result = new List<Itinerary>();
        readonly List<Station> clientStations = new List<Station>();
        
        
        public List<Itinerary> GetItinerary(string departure, string destination)
        {
            
            List<Itinerary> itineraries = new List<Itinerary>();
            List<Station> stations = new List<Station>();
            result.Clear();
            clientStations.Clear();
            
            try
            {
                OpenRoute openRouteSERVICE = new OpenRoute();
                OpenStreetMap openStreetMapSERVICE = new OpenStreetMap();
                Station originStation = GetNearstOriginStation(departure);
                Station destinationStation = GetNearstDestinationStation(destination);


                if (destinationStation.position.Equals(originStation.position))
                {
                    List<Itinerary> itenByFoot = GetItineraryByFoot(departure, destination);
                    Itinerary fastestRoute = itenByFoot.OrderBy(t => t.duration).Min();
                    fastestRoute.profile = Profile.Walk;
                    result.Add(fastestRoute);
                }
                else
                {
                    stations.Add(originStation);
                    if (originStation.contract_name.Equals(destinationStation.contract_name))
                    {
                        Itinerary fastestRouteByFoot1 = GetBestItinerary(Profile.Walk, GetGeoCoordinate(departure), new GeoCoordinate(originStation.position.lat, originStation.position.lng));
                        Itinerary fastestRouteByBike = GetBestItinerary(Profile.Cycle, new GeoCoordinate(originStation.position.lat, originStation.position.lng), new GeoCoordinate(destinationStation.position.lat, destinationStation.position.lng));
                        Itinerary fastestRouteByFoot2 = GetBestItinerary(Profile.Walk, new GeoCoordinate(destinationStation.position.lat, destinationStation.position.lng), GetGeoCoordinate(destination));
                        itineraries.Add(fastestRouteByFoot1);
                        itineraries.Add(fastestRouteByBike);
                        itineraries.Add(fastestRouteByFoot2);
                    }
                    else
                    {
                        itineraries.Add(GetBestItinerary(Profile.Walk, GetGeoCoordinate(departure), new GeoCoordinate(originStation.position.lat, originStation.position.lng)));
                        Tuple<List<Itinerary>, List<Station>> itinerariesAndStations = GetMultipleContractItinerary(originStation, destinationStation.contract_name, destination,destinationStation);
                        itinerariesAndStations.Item1.ForEach(i => itineraries.Add(i));
                        itinerariesAndStations.Item2.ForEach(s => stations.Add(s));
                        itineraries.Add(GetBestItinerary(Profile.Walk, new GeoCoordinate(destinationStation.position.lat, destinationStation.position.lng), GetGeoCoordinate(destination)));
                    }
                    List<Itinerary> itenByFoot = GetItineraryByFoot(departure, destination);
                    Itinerary  fastestRouteOnlyByFoot = itenByFoot.OrderBy(t => t.duration).Min();
                    fastestRouteOnlyByFoot.profile = Profile.Walk;
                    List<Itinerary> itinerariesByFOOT = new List<Itinerary>();
                    itinerariesByFOOT.Add(fastestRouteOnlyByFoot);
                    
                    Double durationByFoot = GetDuration(itinerariesByFOOT);
                    Double durationByBike = GetDuration(itineraries);
                    
                    if (durationByBike > durationByFoot)
                    {
                        itineraries.Clear();
                        result.Add(fastestRouteOnlyByFoot);
                    }
                    else
                    {
                        stations.ForEach(s => clientStations.Add(s));
                        clientStations.Add(destinationStation);
                        itineraries.ForEach(itinerarie => result.Add(itinerarie));
                    }
                }
            }
            catch (UnavailableBikeStation)
            {
                clientStations.Clear();
                List<Itinerary> itenByFoot = GetItineraryByFoot(departure, destination);
                Itinerary fastestItinerary = itenByFoot.OrderBy(t => t.duration).Min();
                fastestItinerary.profile = Profile.Walk;
                result.Add(fastestItinerary);
            }
            catch (Exception e) when (e is HttpRequestException || e is InvalidAdress || e is System.AggregateException)
            {
                clientStations.Clear();
                result.Clear();
            }
           
           
            return result;
            
        }


        public String GetItineraryOntheMQ(string departure, string destination)
        {
            try
            {
                if (result.Count == 0)
                    GetItinerary(departure, destination);
                string q = Guid.NewGuid().ToString("N");
                ActiveMQService.SendItinerariesToTheMQ(result, q);
                if (result.Count > 1)
                {
                    result.RemoveAt(0);

                }
                return q;
            }
            catch (Exception)
            {
                return "Error while sending data to the MQ";
            }


        }

        public List<Itinerary> UdateItinerary(string departure, string destination)
        {
            if (clientStations.Count > 0)
            {
                try
                {
                    Station originStation = GetNearstOriginStation(departure);
                    Station destinationStation = GetNearstDestinationStation(destination);
                    if (!clientStations[clientStations.Count - 1].name.Equals(destinationStation.name) || !clientStations[0].name.Equals(originStation.name))
                    {
                        GetItinerary(departure, destination);

                    }
                    if (clientStations.Count == 4)
                    {
                        Station dropStation = GetNearestDropStation(originStation, destination);
                        Station pickUpStation = GetNearestPickUpStation(dropStation, destinationStation.contract_name);
                        if (!clientStations[1].name.Equals(dropStation.name) || !clientStations[2].name.Equals(pickUpStation.name))
                        {
                            GetItinerary(departure, destination);
                        }
                    }
                }
                catch (Exception)
                {
                    GetItinerary(departure, destination);
                }
            }
            else
            {
                try
                {
                    Station station = GetNearstOriginStation(departure);
                    GetItinerary(departure, destination);
                }
                catch (Exception) //no station founnd
                { }

            }
            return result;
        }
        public String UdateItineraryOnTheMQ(string departure, string destination)
        {
            try
            {
                UdateItinerary(departure, destination);
                string q = "queue " + Guid.NewGuid().ToString("N");
                ActiveMQService.SendItinerariesToTheMQ(result, q);
                return q;
            }
            catch
            {
                return "error while sending data to the MQ";
            }
        }
        private Station GetNearstOriginStation(string address)
        {
            string city = GetCity(address);

            List<Station> stations;
            try
            {
                Contract contract = GetContract(city);
                stations = GetContractStations(contract.name);
            }
            catch (NullReferenceException)
            {
                stations = GetAllStations();
            }
            catch (NoContractFound)
            {
                stations = GetAllStations();
            }
            GeoCoordinate destination = GetGeoCoordinate(address);

            double MinDistance = 0;
            Station nearstStation = null;
            foreach (Station station in stations)
            {
                double Distance = destination.GetDistanceTo(new GeoCoordinate(station.position.lat, station.position.lng));
                if (nearstStation == null)
                {
                    if (station.available_bikes > 0)
                    {
                        MinDistance = Distance;
                        nearstStation = station;
                    }
                    else
                        continue;
                }
                else
                {
                    if (Distance < MinDistance)
                    {
                        if (station.available_bikes > 0)
                        {
                            nearstStation = station;
                            MinDistance = Distance;
                        }
                        else
                            continue;
                    }
                }
            }
            if (nearstStation == null)
                throw new Exception("No station with available bikes was found");
            return nearstStation;
        }






 ////////////////////////////////////////////////////////////////////////////////////////////////PRIVATE METHODS//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private List<Station> GetContractStations(string Contract)
        {
            Task<string> stationTask = proxyClient.GetStationsAsync(Contract);
            List<Station> stations = JsonSerializer.Deserialize<List<Station>>(stationTask.Result);
            return stations;
        }

        private Contract GetContract(string city)
        {

            Task<string> contractTask = proxyClient.GetContractsAsync();
            List<Contract> contractsJson = JsonSerializer.Deserialize<List<Contract>>(contractTask.Result);
            foreach (Contract c in contractsJson)
            {
                if (c.name.Equals(city))
                {
                    return c;
                }
                if (c.cities != null)
                {
                    if (c.cities.Contains(city))
                    {
                        return c;
                    }
                }
            }
            throw new NoContractFound("no contract was found for this address");
        }

        private Station GetNearstDestinationStation(string address)
        {
            string city = GetCity(address);
            List<Station> stations;
            try
            {
                Contract contract = GetContract(city);
                stations = GetContractStations(contract.name);
            }
            catch
            {
                stations = GetAllStations();
            }
            GeoCoordinate destination = GetGeoCoordinate(address);

            double MinDistance = 0;
            Station nearstStation = null;
            foreach (Station station in stations)
            {

                double Distance = destination.GetDistanceTo(new GeoCoordinate(station.position.lat, station.position.lng));
                if (nearstStation == null)
                {
                    if (station.available_bike_stands > 0)
                    {
                        MinDistance = Distance;
                        nearstStation = station;
                    }
                    else
                        continue;
                }
                else
                {
                    if (Distance < MinDistance)
                    {
                        if (station.available_bike_stands > 0)
                        {
                            nearstStation = station;
                            MinDistance = Distance;
                        }
                        else
                            continue;
                    }
                }
            }
            if (nearstStation == null)
                throw new UnavailableBikeStation("No station with available bike stands was found");

            return nearstStation;
        }

        private List<Itinerary> GetItineraryByFoot(string departure, string destination)
        {
            List<Itinerary> itenByFoot = new OpenRoute().getItineraries(GetGeoCoordinate(departure),
                GetGeoCoordinate(destination), Profile.Walk).Result;
            return itenByFoot;
        }
        private Double GetDuration(List<Itinerary> itineraries)
        {
            Double duration = 0;
            itineraries.ForEach(it => {
                duration += it.duration;
            });
            return duration;
        }

        private Itinerary GetBestItinerary(Profile p, GeoCoordinate origin, GeoCoordinate dest)
        {
            OpenRoute openRouteSERVICE = new OpenRoute();
            List<Itinerary> itn = openRouteSERVICE.getItineraries(dest, origin, p).Result;
            if (itn.Count > 0)
            {
                itn.ForEach(it => it.profile = p);
            }
            return itn.OrderBy(t => t.duration).Min();

        }
        
        private Tuple<List<Itinerary>,List<Station>> GetMultipleContractItinerary(Station originStation, string contract,string destination, Station destinationStation)
        {
            List<Itinerary> itineraries = new List<Itinerary>();
            List<Station> stations = new List<Station>();
            OpenStreetMap openStreetMapSERVICE = new OpenStreetMap();
            Station dropStation = GetNearestDropStation(originStation, destination);
            Station pickUpStation = GetNearestPickUpStation(dropStation, contract);
            stations.Add(dropStation);
            stations.Add(pickUpStation);
            GeoCoordinate originStationLocation = new GeoCoordinate(originStation.position.lat, originStation.position.lng);
            GeoCoordinate dropStationLocation = new GeoCoordinate(dropStation.position.lat, dropStation.position.lng);

            GeoCoordinate dropLocation = new GeoCoordinate(dropStation.position.lat, dropStation.position.lng);
            GeoCoordinate pickUpLocation = new GeoCoordinate(pickUpStation.position.lat, pickUpStation.position.lng);
            
            itineraries.Add(GetBestItinerary(Profile.Cycle, originStationLocation, dropLocation));
            itineraries.Add(GetBestItinerary(Profile.Walk, dropLocation, pickUpLocation));
            itineraries.Add(GetBestItinerary(Profile.Cycle, pickUpLocation, dropStationLocation));
            return new Tuple<List<Itinerary>, List<Station>>(itineraries, stations);
        }

      
        
        private GeoCoordinate GetGeoCoordinate(string address)
        {
            string longitude;
            string latitude;
            if (address != null)
            {
                OpenStreetMap openStreetMapSERVICE = new OpenStreetMap();

                Place place = openStreetMapSERVICE.GetPlaceInformation(address).Result;
                if (place != null)
                {
                    longitude = place.lon;
                    latitude = place.lat;

                    try
                    {
                        Convert.ToDouble(latitude);
                    }
                    catch
                    {
                        longitude = longitude.Replace('.', ',');
                        latitude = latitude.Replace('.', ',');
                    }
                    return new GeoCoordinate(Convert.ToDouble(latitude), Convert.ToDouble(longitude));

                }
            }
            else
            {
                throw new InvalidAdress("invalid adress");
            }

            return null;

        }

        private string GetCity(string address)
        {
            string town = null;
            OpenStreetMap openStreetMapSERVICE = new OpenStreetMap();
            

            Place place = openStreetMapSERVICE.GetPlaceInformation(address).Result;

            if (place is null){
                throw new InvalidAdress("invalid adress");
            }

            town = place.address.town;
            if (town is null)
            {

                town = place.address.city;
            }
            
            return town;
        }

        private List<Station> GetAllStations()
        {
            Task<string> stationsTask = proxyClient.GetAllStationsAsync();
            List<Station> stations = JsonSerializer.Deserialize<List<Station>>(stationsTask.Result);
            return stations;
        }
        private List<Contract> GetContracts()
        {
            Task<string> contractTask = proxyClient.GetContractsAsync();
            List<Contract> contracts = JsonSerializer.Deserialize<List<Contract>>(contractTask.Result);
            return contracts;
        }




        private Station GetNearestDropStation(Station station, string destination)
        {
            GeoCoordinate stationGeoCoordinate = new GeoCoordinate(station.position.lat, station.position.lng);
            GeoCoordinate destinationGeoCoordinate = GetGeoCoordinate(destination);
            List<Station> stations = GetContractStations(station.contract_name);

            double MinDistance = 0;
            Station nearstStation = null;
            foreach (Station station2 in stations)
            {
                if (station2.number == station.number)
                    continue;
                double Distance1 = stationGeoCoordinate.GetDistanceTo(new GeoCoordinate(station2.position.lat, station2.position.lng));
                double Distance2 = destinationGeoCoordinate.GetDistanceTo(new GeoCoordinate(station2.position.lat, station2.position.lng));
                double Distance = Distance1 + Distance2;
                if (nearstStation == null)
                {
                    if (station2.available_bike_stands > 0)
                    {
                        MinDistance = Distance;
                        nearstStation = station2;
                    }
                    else
                        continue;
                }
                else
                {
                    if (Distance < MinDistance)
                    {
                        if (station2.available_bike_stands > 0)
                        {
                            nearstStation = station2;
                            MinDistance = Distance;
                        }
                        else
                            continue;
                    }
                }
            }
            return nearstStation;
        }

        private Station GetNearestPickUpStation(Station station, string contract)
        {
            GeoCoordinate stationGeoCoordinate = new GeoCoordinate(station.position.lat, station.position.lng);
            List<Station> stations = GetContractStations(contract);
            double MinDistance = 0;
            Station nearstStation = null;
            foreach (Station station2 in stations)
            {
               
                double Distance = stationGeoCoordinate.GetDistanceTo(new GeoCoordinate(station2.position.lat, station2.position.lng));
                if (nearstStation == null)
                {
                    if (station2.available_bike_stands > 0)
                    {
                        MinDistance = Distance;
                        nearstStation = station2;
                    }
                    else
                        continue;
                }
                else
                {
                    if (Distance < MinDistance)
                    {
                        if (station2.available_bike_stands > 0)
                        {
                            nearstStation = station2;
                            MinDistance = Distance;
                        }
                        else
                            continue;
                    }
                }
            }
            return nearstStation;
        }


    }
}

