using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            OrderedDictionary MatchLog = new OrderedDictionary(2);
            OrderedDictionary DepthLog = new OrderedDictionary(2);
            Console.WriteLine(MatchLog.Count);
           MatchLog.Insert(0,"k1","v1");
            Console.WriteLine(MatchLog.Count);
            MatchLog.Insert(0,"k2","v2");
            Console.WriteLine(MatchLog.Count);
            MatchLog.Insert(0,"k3", "v3");
            Console.WriteLine(MatchLog.Count);
            foreach (DictionaryEntry de in MatchLog) {
                Console.WriteLine(de.Key.ToString() + "+" + de.Value.ToString());
            }
            Console.WriteLine(MatchLog[0]);            
            Console.ReadLine();

        }
    }
}
