using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace DO
    {
       public class ItemFoundException : Exception
        {
            public string type { get; set; }
            public uint key { get; set; }
            public ItemFoundException(string type, uint unic_key)
            {
                this.type = type;
                key = unic_key;
            }
            protected ItemFoundException(SerializationInfo serializableAttribute, StreamingContext context) : base(serializableAttribute, context) { }
            public override string ToString()
            {
                string Error_mashge = "";
                Error_mashge += $"this " + type + "\n";
                Error_mashge += $"number: {key}\n";
                Error_mashge += "alrdy found\n";
                return Error_mashge;

            }
        }

       public class ItemNotFoundException : Exception
        {
            public string type { get; set; }
            public uint key { get; set; }
            public ItemNotFoundException(string type, uint unic_key):base(type)
            {
                this.type = type;
                key = unic_key;
            }
            protected ItemNotFoundException(SerializationInfo serializableAttribute, StreamingContext context) : base(serializableAttribute, context) { }

            public override string ToString()
            {
                string Error_mashge = "";
                Error_mashge += $"this " + type ;
                Error_mashge += $" number: {key}";
                Error_mashge += " not found ";
                Error_mashge += $"Please check if {type} number: {key} existing.\n";
                Error_mashge += $"You can check {type} by issuing complete lists.\n";
                return Error_mashge;

            }
        }

        public class ListEmptyException : Exception
        {
            string ExceptionMesseg { get; set; }
            public ListEmptyException(string list)
            {
                ExceptionMesseg = $"ERROR: this {list} empty! ";
            }
            public override string ToString()
            {
                return ExceptionMesseg;
            }
        }

        public class NoItemWhitThisConditionException : Exception
        {
            string ExceptionMassege { get => " no items whit your condition!"; }
        }

    public class XMLFileLoadCreateException : Exception
    {
        public string xmlFilePath;
        public XMLFileLoadCreateException(string xmlPath) : base() { xmlFilePath = xmlPath; }
        public XMLFileLoadCreateException(string xmlPath, string message) :
            base(message)
        { xmlFilePath = xmlPath; }
        public XMLFileLoadCreateException(string xmlPath, string message, Exception innerException) :
            base(message, innerException)
        { xmlFilePath = xmlPath; }

        public override string ToString() => base.ToString() + $", fail to load or create xml file: {xmlFilePath}";
    }

}

