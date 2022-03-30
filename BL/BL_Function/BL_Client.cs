using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using System.Collections;
using DalApi;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;

namespace BlApi
{
    partial class BL : IBL
    {
        /// <summary>
        /// return the client location
        /// </summary>
        /// <param name="id"> id client</param>
        /// <returns> client location</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Location ClientLocation(uint id)
        {
            lock (dalObj)
            {
                DO.Client client = new DO.Client();

                try
                {

                    client = dalObj.CilentByNumber(id);
                }
                catch (DO.ItemNotFoundException ex)
                {
                    throw (new ItemNotFoundException(ex));
                }
                Location location_client = new Location();
                location_client.Latitude = client.Latitude;
                location_client.Longitude = client.Longitude;
                return location_client;
            }
        }

        /// <summary>
        /// add client
        /// </summary>
        /// <param name="client"> client to add</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddClient(Client client)
        {
            lock (dalObj)
            {
                //checking id
                if (client.Id < 100000000)
                { throw new NumberNotEnoughException(9); }
                if (client.Id > 999999999)
                { throw new NumberMoreException(); }

                //chcing phon number
                chekingFon(client.Phone);
                chckingPoint(client.Location);
                try
                {
                    dalObj.AddClient(new DO.Client
                    {
                        Id = client.Id,
                        Latitude = client.Location.Latitude,
                        Longitude = client.Location.Longitude,
                        Name = client.Name,
                        PhoneNumber = client.Phone
                    });
                }
                catch (DO.ItemFoundException ex)
                {
                    throw (new ItemFoundExeption(ex));
                }
            }
        }

        /// <summary>
        /// help mathod to chack phone number
        /// </summary>
        /// <param name="fon"> phone number</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        void chekingFon(string fon)
        {
            if (fon.Count() < 10)
            { throw new NumberNotEnoughException(10); }
            if (fon.Count() > 10)

            { throw new NumberMoreException(); }
            if (fon[0] != '0' || fon[1] != '5')
            { throw new StartingException("0,5"); }
            if (fon.Any(c => c < '0' || c > '9'))
            { throw new IllegalDigitsException(); }



        }

        /// <summary>
        /// Update fields at a client
        /// </summary>
        /// <param name="client"> client </param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateClient(Client client)
        {
            lock (dalObj)
            {
                //checking id
                if (client.Id < 100000000)
                { throw new NumberNotEnoughException(9); }
                if (client.Id > 999999999)
                { throw new NumberMoreException(); }
                //checking phone number
                chekingFon(client.Phone);
                chckingPoint(client.Location);
                try
                {
                    var clientFromDal = dalObj.CilentByNumber(client.Id);
                    if (client.Name != "")
                        clientFromDal.Name = client.Name;
                    if (client.Phone != "")
                        clientFromDal.PhoneNumber = client.Phone;
                    if (client.Location.Latitude != 0)
                        clientFromDal.Latitude = client.Location.Latitude;
                    if (client.Location.Longitude != 0)
                        clientFromDal.Latitude = client.Location.Longitude;

                    dalObj.UpdateClient(clientFromDal);
                }
                catch (DO.ItemNotFoundException ex)
                {
                    throw new ItemNotFoundException(ex);
                }
            }
        }

        /// <summary>
        /// Receiving a client by ID
        /// </summary>
        /// <param name="id"> client ID</param>
        /// <returns> client</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Client GetingClient(uint id)
        {
            lock (dalObj)
            {
                try
                {
                    var client = dalObj.CilentByNumber(id);
                    var loc = new Location();
                    loc.Latitude = client.Latitude;
                    loc.Longitude = client.Longitude;
                    var returnClient = new Client { Id = client.Id, Name = client.Name, Phone = client.PhoneNumber, Location = loc };
                    returnClient.ToClient = new ObservableCollection<PackageAtClient>();
                    var packege = dalObj.PackegeList(x => x.GetingClient == id);


                    if (packege.Count() != 0)
                        foreach (var packegeInList in packege)
                        {
                            returnClient.ToClient.Add(packegeInList.convretPackegeDalToPackegeAtClient(packegeInList.GetingClient, dalObj));
                        }
                    returnClient.FromClient = new ObservableCollection<PackageAtClient>();
                    packege = dalObj.PackegeList(x => x.SendClient == id);
                    if (packege.Count() != 0)
                        foreach (var packegeInList in packege)
                        {
                            returnClient.FromClient.Add(packegeInList.convretPackegeDalToPackegeAtClient(packegeInList.SendClient, dalObj));
                        }
                    return returnClient;
                }
                catch (DO.ItemNotFoundException ex)
                {
                    throw new ItemNotFoundException(ex);
                }

            }
        }

        /// <summary>
        /// delete client
        /// </summary>
        /// <param name="id"> client id</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteClient(uint id)
        {
            lock (dalObj)
            {
                List<uint> packegesDelete = new List<uint>();
                try
                {
                    dalObj.DeleteClient(id);

                }
                catch (DO.ItemNotFoundException ex)
                {
                    throw new ItemNotFoundException(ex);
                }
            }
        }



    }
}
