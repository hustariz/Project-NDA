using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using NetworkAndGenericCalculation.Sockets;
using NetworkAndGenericCalculation.MapReduce;

namespace GenomicTreatment
{
    public class GenomicServeur : Server
    {
        public GenomicServeur(IPAddress host, int portNumber, Action<string> servLogger, Action<string, string, int, int, float, float> gridupdater) : base(host, portNumber, servLogger, gridupdater)
        {

        }

        public override Object map(string Methodmap, string[] text, int chunkSize, int offsets)
        {
            base.map(Methodmap,text,chunkSize,offsets);
            switch (Methodmap)
            {
                case "1" :

                    Console.WriteLine("J'suis MAP");

                    break;

                case "Method1":
                    Tuple<int, string[]> mapDone = mapForMethodOne(text,chunkSize, offsets);
                    return mapDone;
            }


            return null;
        }

        private Tuple<int,string[]> mapForMethodOne(string[] text, int howManyLine, int lastIndex)
        {


            
            string[] pairs;
            List<string> pairsList = new List<string>();
            List<char[]> charList = new List<char[]>();
            int i = 0;

            for(i = 0 ; i < howManyLine ; i++)
            {
                if(!text[lastIndex+i].StartsWith("#"))
                {
                    foreach (char c in text[lastIndex + i].Split('\t')[3].ToCharArray())
                    {
                        pairsList.Add(c.ToString());
                        Console.WriteLine(c.ToString());
                    }
                }
            }

            pairs = pairsList.ToArray<string>();
            Tuple<int, string[]> chunkTosend = new Tuple<int, string[]>(lastIndex+i , pairs);
            return chunkTosend;
        }
    }
}
