using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Threading;
using System.Globalization;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;



namespace PL
{
   internal static class HelpClass
    {
 /// <summary>
 /// replse IEnumrble to observationCollection
 /// </summary>
 /// <typeparam name="T"></typeparam>
 /// <param name="ienumerable"></param>
 /// <param name="ts"></param>
 /// <returns></returns>
        internal static ObservableCollection<T> ConvertIenmurbleToObserve<T>(this IEnumerable<T> ienumerable, ObservableCollection<T> ts)
        {
            lock (ts)lock(ienumerable)
            {

                    var pointer = ts.GetEnumerator();
                    try
                    {
                        int i = 0;
                        bool stopFlag = false;
                        int stop = ts.Count();
                        foreach (var item in ienumerable)
                        {
                            //replace beween ienumrble to observ item
                            if (i<stop)
                            {
                                ts[i] = item;
                                i++;

                            }
                            //if the place in obsrvation collection over
                            else
                            {
                                stopFlag = true;
                                ts.Add(item);
                            }
                           
                        }
                        //if the list shorted
                        if (!stopFlag)
                            while (i < stop)
                            {
                                ts.RemoveAt(i);
                                stop--;
                            }

                        return ts;
                    }
                    catch(Exception )
                    {
                        return ts;
                    }
            }
             
        }
    }

  
}
