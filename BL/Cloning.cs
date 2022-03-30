using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using BlApi;
using BO;
using DalApi;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Runtime.Serialization;


namespace BL
{
    /// <summary>
    /// Help class for deep copy
    /// </summary>
  public static class Cloning
    {

        public static T Clone<T>(this T source)
        {

            DataContractSerializer serializer = new DataContractSerializer(typeof(T));
            using (MemoryStream ms = new MemoryStream())
            {
                serializer.WriteObject(ms, source);
                ms.Seek(0, SeekOrigin.Begin);
                return (T)serializer.ReadObject(ms);
            }
        }
        public static void CopyPropertiesTo<T, S>(this S from, T to)
            {
                foreach (PropertyInfo propTo in to.GetType().GetProperties())
                {
                    PropertyInfo propFrom = typeof(S).GetProperty(propTo.Name);
                    if (propFrom == null)
                        continue;
                    var value = propFrom.GetValue(from, null);
                    if (value is ValueType || value is string)
                        propTo.SetValue(to, value);
                }
            }
            public static object CopyPropertiesToNew<S>(this S from, Type type)
            {
                object to = Activator.CreateInstance(type); // new object of Type
                from.CopyPropertiesTo(to);
                return to;
            }
            
        }
    }
