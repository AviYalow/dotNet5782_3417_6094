using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlApi;
using BO;
using System.Runtime.CompilerServices;
using DalApi;
using static BL.Cloning;

namespace BlApi
{
    partial class BL : IBL
    {
        Func<ClientToList, bool> activeClient = x => x.Active;
        Func<ClientToList, bool> clientHowSendPackege = client => (client.Arrived > 0 || client.NotArrived > 0);
        Func<ClientToList, bool> clientHowSendPackegeAndThePackegeArrive = client => client.Arrived > 0;
        Func<ClientToList, bool> clientHowSendPackegeAndThePackegeNotArrive = client => client.NotArrived > 0;
        Func<ClientToList, bool> clientHowGetingPackegesAndNotArrive = client => client.OnTheWay > 0;
        Func<ClientToList, bool> clientHowGetingPackegesAndArrive = client => client.received > 0;
        Func<ClientToList, bool> clientActiveHowGetingPackeges = client => client.received > 0 || client.OnTheWay > 0;
        Func<ClientToList, bool> clientById = null;



        /// <summary>
        /// list of clients activ
        /// </summary>
        /// <returns> list of clients</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<ClientToList> ClientToLists(bool filter = true)
        {
            lock (dalObj)
            {
                if (!filter)
                    return from client in dalObj.CilentList(x => true)
                           select client.convertClientDalToClientToList(dalObj);
                else
                    return from client in dalObj.CilentList(x => x.Active)
                           select client.convertClientDalToClientToList(dalObj);
            }

        }

        /// <summary>
        /// IEnumerable of client how send packege
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<ClientToList> ClientActiveHowSendPackegesToLists(bool filter = true)
        {
            lock (dalObj)
            {
                clientToListFilter -= clientHowSendPackege;
                clientToListFilter -= clientHowSendPackegeAndThePackegeArrive;
                clientToListFilter -= clientHowSendPackegeAndThePackegeNotArrive;
                if (filter)
                    clientToListFilter += clientHowSendPackege;
                return FilterClientList();
            }

        }

        /// <summary>
        /// IEnumerable of client how send packege and arrive
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<ClientToList> ClientActiveHowSendAndArrivePackegesToLists(bool filter = true)
        {
            lock (dalObj)
            {
                clientToListFilter -= clientHowSendPackege;
                clientToListFilter -= clientHowSendPackegeAndThePackegeArrive;
                clientToListFilter -= clientHowSendPackegeAndThePackegeNotArrive;
                if (filter)
                    clientToListFilter += clientHowSendPackegeAndThePackegeArrive;
                return FilterClientList();
            }

        }
        /// <summary>
        /// IEnumerable of client how send packege and not arrive
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<ClientToList> ClientActiveHowSendPackegesAndNotArriveToLists(bool filter = true)
        {
            lock (dalObj)
            {
                clientToListFilter -= clientHowSendPackege;
                clientToListFilter -= clientHowSendPackegeAndThePackegeArrive;
                clientToListFilter -= clientHowSendPackegeAndThePackegeNotArrive;
                if (filter)
                    clientToListFilter += clientHowSendPackegeAndThePackegeNotArrive;
                return FilterClientList();
            }

        }
        /// <summary>
        /// IEnumerable of client how need to get packege 
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<ClientToList> ClientActiveHowGetingPackegesAndNotArriveToLists(bool filter = true)
        {
            lock (dalObj)
            {
                clientToListFilter -= clientHowGetingPackegesAndNotArrive;
                clientToListFilter -= clientHowGetingPackegesAndArrive;
                clientToListFilter -= clientActiveHowGetingPackeges;
                if (filter)
                    clientToListFilter += clientHowGetingPackegesAndNotArrive;
                return FilterClientList();
            }
        }
        /// <summary>
        /// IEnumerable of client how need to get packege and they get 
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<ClientToList> ClientActiveHowGetingPackegesAndArriveToLists(bool filter = true)
        {
            lock (dalObj)
            {
                clientToListFilter -= clientHowGetingPackegesAndNotArrive;
                clientToListFilter -= clientHowGetingPackegesAndArrive;
                clientToListFilter -= clientActiveHowGetingPackeges;
                if (filter)
                    clientToListFilter += clientHowGetingPackegesAndArrive;
                return FilterClientList();
            }

        }
        /// <summary>
        /// IEnumerable of client how need to get packege and they not get 
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<ClientToList> ClientActiveHowGetingPackegesToLists(bool filter = true)
        {
            lock (dalObj)
            {
                clientToListFilter -= clientHowGetingPackegesAndNotArrive;
                clientToListFilter -= clientHowGetingPackegesAndArrive;
                clientToListFilter -= clientActiveHowGetingPackeges;
                if (filter)
                    clientToListFilter += clientActiveHowGetingPackeges;
                return FilterClientList();
            }

        }
        /// <summary>
        /// return client list it clientInPackege type
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<ClientInPackage> ClientInPackagesList(bool filter = true)
        {
            lock (dalObj)
            {
                return from client in dalObj.CilentList(x => x.Active)
                       select new ClientInPackage { Id = client.Id, Name = client.Name };
            }
        }
        /// <summary>
        /// return client list by filter
        /// </summary>
        /// <param name="active"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<ClientToList> FilterClientList(bool active =true)
        {
            lock (dalObj)
            {
                return from client in filerList(ClientToLists(active), clientToListFilter)
                       select client.Clone();
            }
        }
        /// <summary>
        /// return Id's client list
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<uint> ClientById(string id)
        {
            lock (dalObj)
            {
                clientById = x => x.ID.ToString().StartsWith(id);
                clientToListFilter -= clientById;
                if (id != "")
                    clientToListFilter += clientById;
                return from client in filerList(ClientToLists(), clientToListFilter)
                       select client.ID;
            }

        }
    }
}
