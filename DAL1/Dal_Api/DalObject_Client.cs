using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalApi;
using Ds;
using DO;
using System.Runtime.CompilerServices;

namespace Dal
{
    sealed partial class DalObject : DalApi.IDal
    {



        /// <summary>
        /// Adding a new client
        /// </summary>
        /// <param name="client"> client to add</param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddClient(Client client)
        {

          
            if (DataSource.clients.Any(x=>x.Id==client.Id&&x.Active))
                throw (new ItemFoundException("Client", client.Id));
            client.Active = true;
            DataSource.clients.Add(client);


        }

        /// <summary>
        /// Display client data desired 
        /// </summary>
        /// <param name="id">ID of desired client </param>
        /// <returns> string of data </returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public Client CilentByNumber(uint id)
        {
            if (!DataSource.clients.Any(x => x.Id == id))
                throw (new ItemNotFoundException("client", id));
            return DataSource.clients[DataSource.clients.FindIndex(x => x.Id == id)];

        }


        /// <summary>
        ///return all clients
        /// </summary>
        /// <returns> list of client</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<Client> CilentList(Predicate<Client> predicate)
        {
            return from x in DataSource.clients
                   where predicate(x)
                   select x;
        }

        /// <summary>
        /// delete a spsific client 
        /// </summary>
        /// <param name="id"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void DeleteClient(uint id)
        {
            if (!DataSource.clients.Any(x=>x.Id==id&&x.Active))
                throw (new ItemNotFoundException("client", id));

            for (int i = 0; i < DataSource.clients.Count; i++)
            {
                if(DataSource.clients[i].Id==id)
                {
                    var client = DataSource.clients[i];
                    client.Active = false;
                    DataSource.clients[i] = client;
                    break;
                }    
            }


        }

        /// <summary>
        /// update fileds at a given client
        /// </summary>
        /// <param name="client"> a given client </param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateClient(Client client)
        {
            int index = DataSource.clients.FindIndex(x => x.Id == client.Id&&x.Active);
            if (index != -1)
                DataSource.clients[index] = client;
            else
                throw (new DO.ItemNotFoundException("client", client.Id));
        }
    }
}
