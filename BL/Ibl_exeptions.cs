using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using DalApi;
using System.Runtime.CompilerServices;

namespace BO
{
    /// <summary>
    /// exception for item alrdy exist
    /// </summary>
    public class ItemFoundExeption : Exception
    {
        DO.ItemFoundException exeption { get; set; }
        public ItemFoundExeption(DO.ItemFoundException ex) { exeption = ex; }
        protected ItemFoundExeption(SerializationInfo serializableAttribute, StreamingContext context) : base(serializableAttribute, context) { }
        public override string ToString()
        {
            string exeptionString = "";
            DateTime time = DateTime.Now;
            exeptionString += $"\n\aTime:{time.ToLongTimeString()}\n";
            exeptionString += exeption.ToString();
            return exeptionString;
        }
    }

    /// <summary>
    /// exception for item not exist
    /// </summary>
    public class ItemNotFoundException : Exception
    {
        DO.ItemNotFoundException exeption { get; set; }
        public string type { get; set; }
        public uint key { get; set; }

        public ItemNotFoundException(string type, uint unic_key) : base(type)
        {
            this.type = type;
            key = unic_key;
        }
        public ItemNotFoundException(DO.ItemNotFoundException ex) : base(ex.Message, ex) { exeption = ex; }
        protected ItemNotFoundException(SerializationInfo serializableAttribute, StreamingContext context) : base(serializableAttribute, context) { }
        public override string ToString()
        {
            string exeptionString = "";
            DateTime time = DateTime.Now;
            exeptionString += $"\aTime:{time.ToLongTimeString()}\n";
            exeptionString += exeption is null ? $"{type} number: {key} not Exisst" : exeption;
            return exeptionString;
        }
    }

    /// <summary>
    /// no place for drone in base
    /// </summary>
    public class NoPlaceForChargeException : Exception
    {
        uint base_ { get; set; }
        public NoPlaceForChargeException(uint base_) : base()
        { this.base_ = base_; }
        protected NoPlaceForChargeException(SerializationInfo serializableAttribute, StreamingContext context) : base(serializableAttribute, context) { }
        public override string ToString()
        {
            return $"Time:{DateTime.Now} \nIn this base number:{base_}\nno place for drone! \n" +
                $"plase chack the charging drone list , and relese drone with full buttry. ";
        }
    }

    /// <summary>
    /// the input is worng
    /// </summary>
    public class InputErrorException : Exception
    {

        public InputErrorException() : base() { }

        protected InputErrorException(SerializationInfo serializableAttribute, StreamingContext context) : base(serializableAttribute, context) { }
        public override string ToString()
        {
            return $"Time:{DateTime.Now} \nInput Error!";
        }
    }

    /// <summary>
    /// exception for try down number of charging station whan still have more drone in charge
    /// </summary>
    public class UpdateChargingPositionsException : Exception
    {
        public int DroneInCharge { get; set; }
        public uint BaseNumber { get; set; }
        public UpdateChargingPositionsException(int number, uint numberBase) : base() { DroneInCharge = number; BaseNumber = numberBase; }
        protected UpdateChargingPositionsException(SerializationInfo serializableAttribute, StreamingContext context) : base(serializableAttribute, context) { }
        public override string ToString()
        {
            return $"There are {DroneInCharge} skimmers in charging than the new amount of positions.\n" +
                "Please release skimmers from charging.";
        }
    }

    /// <summary>
    /// Exception for try to relese more dorne for spsific base 
    /// </summary>
    public class TryToPullOutMoreDrone : Exception
    {
        public TryToPullOutMoreDrone() : base() { }
        protected TryToPullOutMoreDrone(SerializationInfo serializableAttribute, StreamingContext context) : base(serializableAttribute, context) { }
        public override string ToString()
        {
            return "Your try to pull out more drone then statins charge! ";
        }
    }

    /// <summary>
    /// try to send drone in charge or work to charge
    /// </summary>
    public class DroneStillAtWorkException : Exception
    {
        public DroneStillAtWorkException() : base() { }
        protected DroneStillAtWorkException(SerializationInfo serializableAttribute, StreamingContext context) : base(serializableAttribute, context) { }
        public override string ToString()
        {
            return "The drone still in work";
        }
    }

    /// <summary>
    /// try to send drone to delivery with not enough buttry
    /// </summary>
    public class NoButrryToTripException : Exception
    {
        public double buttry { get; set; }
        public NoButrryToTripException(double butrry) : base() { this.buttry = butrry; }
        protected NoButrryToTripException(SerializationInfo serializableAttribute, StreamingContext context) : base(serializableAttribute, context) { }
        public override string ToString()
        {
            return $"This Drone hes{buttry}. he can't go to this point!" +
                $"You should send the skimmer for charging or transfer location ";
        }
    }

    /// <summary>
    /// not enough number in in-put
    /// </summary>
    public class NumberNotEnoughException : Exception
    {
        int amount { get; set; }
        public NumberNotEnoughException(int num) : base() { amount = num; }
        protected NumberNotEnoughException(SerializationInfo serializableAttribute, StreamingContext context) : base(serializableAttribute, context) { }
        public override string ToString()
        {
            return $"There is last then {amount} digit number";
        }
    }
    /// <summary>
    /// more number with neccery
    /// </summary>
    public class NumberMoreException : Exception
    {

        public NumberMoreException() : base() { }
        protected NumberMoreException(SerializationInfo serializableAttribute, StreamingContext context) : base(serializableAttribute, context) { }
        public override string ToString()
        {
            return "There is more then 10 digit number";
        }
    }
    /// <summary>
    /// illegal digint number in phone number
    /// </summary>
    public class IllegalDigitsException : Exception
    {
        public IllegalDigitsException() : base() { }
        protected IllegalDigitsException(SerializationInfo serializableAttribute, StreamingContext context) : base(serializableAttribute, context) { }
        public override string ToString()
        {
            return "Digits only without signs and letters";
        }
    }
    /// <summary>
    /// phone number start no with 05
    /// </summary>
    public class StartingException : Exception
    {
        public string Start { get; set; }
        public StartingException(string masegg) : base(masegg) { Start = masegg; }
        protected StartingException(SerializationInfo serializableAttribute, StreamingContext context) : base(serializableAttribute, context) { }
        public override string ToString()
        {
            return "You have to start whit" + Start + "only";
        }
    }
    /// <summary>
    /// list empty
    /// </summary>
    public class TheListIsEmptyException : Exception
    {
        public TheListIsEmptyException(string masseg = "\a ERROR: This list is empty") : base(masseg) { }
        protected TheListIsEmptyException(SerializationInfo serializableAttribute, StreamingContext context) : base(serializableAttribute, context) { }
    }
    /// <summary>
    /// try to send drone for delivery when he still in work or charge
    /// </summary>
    public class DroneCantMakeDliveryException : Exception
    {

        public DroneCantMakeDliveryException() : base() { }
        protected DroneCantMakeDliveryException(SerializationInfo serializableAttribute, StreamingContext context) : base(serializableAttribute, context) { }
        public override string ToString()
        {
            return "\a ERROR: Drone cant make the delivery";
        }
    }
    /// <summary>
    /// function Error
    /// </summary>
    public class FunctionErrorException : Exception
    {

        public FunctionErrorException(string message) : base(message) { }
        protected FunctionErrorException(SerializationInfo serializableAttribute, StreamingContext context) : base(serializableAttribute, context) { }
        public override string ToString()
        {
            return $"Error in" + Message + " function";
        }
    }
    /// <summary>
    /// Distans is out for the maximom for drone
    /// </summary>
    public class MoreDistasThenMaximomException : Exception
    {
        public uint Send { get; set; }
        public uint Get { get; set; }
        public MoreDistasThenMaximomException(uint send, uint get) : base() { Send = send; Get = get; }

        protected MoreDistasThenMaximomException(SerializationInfo serializableAttribute, StreamingContext context) : base(serializableAttribute, context) { }
        public override string ToString()
        {

            return $"Client:{Send} can't send to Client:{Get} packege! ";
        }
    }
    /// <summary>
    /// client out of area the coumpeny work with
    /// </summary>
    public class ClientOutOfRangeException : Exception
    {

        public ClientOutOfRangeException() : base() { }
        protected ClientOutOfRangeException(SerializationInfo serializableAttribute, StreamingContext context) : base(serializableAttribute, context) { }
        public override string ToString()
        {
            return $"Your location out of our range, we not working in your erea,Sorry! ";
        }
    }
    /// <summary>
    /// Exception when tring to delete packege when she sending
    /// </summary>
    public class ThePackegeAlredySendException : Exception
    {

        public ThePackegeAlredySendException() : base() { }
        protected ThePackegeAlredySendException(SerializationInfo serializableAttribute, StreamingContext context) : base(serializableAttribute, context) { }
        public override string ToString()
        {
            return $"The packege alredy send to auter client ";
        }
    }

    /// <summary>
    ///  Exception when the packege not assioction or collected yet 
    /// </summary>
    public class PackegeNotAssctionOrCollectedException : Exception
    {

        public PackegeNotAssctionOrCollectedException() : base() { }
        protected PackegeNotAssctionOrCollectedException(SerializationInfo serializableAttribute, StreamingContext context) : base(serializableAttribute, context) { }
        public override string ToString()
        {
            return $"The packege not assioction or collected yet ";
        }
    }

    /// <summary>
    ///  Exception when the packege not assioction or collected yet 
    /// </summary>
    public class DroneTryToStartSecondeSimolatorException : Exception
    {
        uint drone;
        public DroneTryToStartSecondeSimolatorException(uint drone) : base() { this.drone = drone; }
        protected DroneTryToStartSecondeSimolatorException(SerializationInfo serializableAttribute, StreamingContext context) : base(serializableAttribute, context) { }
        public override string ToString()
        {
            return $"This drone number{drone} try to start seconde simolation! ";
        }
    }
}

