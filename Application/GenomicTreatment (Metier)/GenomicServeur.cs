using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using NetworkAndGenericCalculation.Sockets;
using NetworkAndGenericCalculation.MapReduce;
using NetworkAndGenericCalculation.Chunk;
using System.Collections.Concurrent;

namespace GenomicTreatment
{
    public class GenomicServeur : Server
    {
        public List<Tuple<char, int>> globalResultMethod1 { get; set; }
        public int increment { get; set; }

        

        public GenomicServeur(IPAddress host, int portNumber, Action<string> servLogger, Action<int, string, string, float, float> gridupdater) : base(host, portNumber, servLogger, gridupdater)
        {
           
        }

       

        public override void ProcessInput(DataInput dateReceived)
        {


            base.ProcessInput(dateReceived);
            switch (dateReceived.Method)
            {
                case "globalReduceMethod1":

                    //Console.WriteLine("globalReduceMethod : " + increment++);
                    Dictionary<string, int> incdico = new Dictionary<string, int>();
                    incdico = (Dictionary<string, int>)dateReceived.Data;

                    Console.WriteLine("SERVEUR SUBTASK RECEIVE : " + dateReceived.SubTaskId);

                    foreach (int key in dicoFinal.Keys)
                    {
                        if(key == dateReceived.SubTaskId)
                        {
                            dicoFinal[key] = new Tuple<bool, Dictionary<string, int>>(true, incdico);
                            Console.WriteLine("Received and update : " + dateReceived.SubTaskId);
                        }
                    }


                    bool isNotComplete = false;
                    foreach (int key in dicoFinal.Keys)
                    {
                        if (dicoFinal[key].Item1 != true)
                        {
                            isNotComplete = true;
                        }
                    }

                    if (!isNotComplete)
                    {
                        Dictionary<string, int> datatruc = lastReduce(dicoFinal);
                        foreach(string key in datatruc.Keys)
                        {
                            Console.WriteLine("KEY : "+ key + " DATA : " + datatruc[key]);
                        }

                        stopWatch.Stop();
                        Console.WriteLine("STOP WATCH" + stopWatch.Elapsed);
                    }
                    break;
            }
        }

        public override Object map(string Methodmap, string[] text, int chunkSize, int offsets)
        {
            base.map(Methodmap, text, chunkSize, offsets);
            switch (Methodmap)
            {
                case "1":
                    break;

                case "Method1":
                    Tuple<int, string[]> mapDone = mapForMethodOne(text, chunkSize, offsets);
                    return mapDone;

                case "check":

                    
                    break;
            }


            return null;
        }

        private Tuple<int, string[]> mapForMethodOne(string[] text, int howManyLine, int lastIndex)
        {

            string[] pairs;
            List<string> pairsList = new List<string>();
            List<char[]> charList = new List<char[]>();
            int i = 0;

            for (i = 0; i < howManyLine; i++)
            {
                if (!text[lastIndex + i].StartsWith("#"))
                {
                    foreach (char c in text[lastIndex + i].Split('\t')[3].ToCharArray())
                    {
                        pairsList.Add(c.ToString());
                    }
                }
            }

            pairs = pairsList.ToArray<string>();
            Tuple<int, string[]> chunkTosend = new Tuple<int, string[]>(lastIndex + i, pairs);

            return chunkTosend;
        }

        public override List<Tuple<char, int>> ReduceMethod1(List<Tuple<char, int>> listGlobale, List<Tuple<char, int>> listMapped)
        {
            if (listGlobale == null || listGlobale.Count == 0)
            {

                listGlobale = listMapped;

            }

            else
            {
                for (int e = 0; e < listMapped.Count; e++)
                {
                    bool present = false;
                    for (int i = 0; i < listGlobale.Count; i++)
                    {
                        if (listGlobale[i].Item1 == listMapped[e].Item1)
                        {
                            present = true;
                            listGlobale[i] = new Tuple<char, int>(listGlobale[i].Item1, listGlobale[i].Item2 + listMapped[e].Item2);
                        }
                    }
                    if (!present)
                        listGlobale.Add(listMapped[e]);
                }
            }
            return listGlobale;
        }

        public Dictionary<string, int> lastReduce(ConcurrentDictionary<int, Tuple<bool, Dictionary<string, int>>> datas)
        {

            Dictionary<string, int> finalList = new Dictionary<string, int>();

            Parallel.ForEach(datas, (tuple) => {

                foreach (var key in tuple.Value.Item2)
                {
                    int nbOccur;
                    if (finalList.TryGetValue(key.Key, out nbOccur))
                    {
                        finalList[key.Key] = finalList[key.Key] + key.Value;
                    }
                    else
                    {
                        finalList.Add(key.Key, key.Value);
                    }
                }
            });

            /*foreach (var tuple in datas)
            {
                foreach (var key in tuple.Value.Item2)
                {
                    int nbOccur;
                    if (finalList.TryGetValue(key.Key, out nbOccur))
                    {
                        finalList[key.Key] = finalList[key.Key] + key.Value;
                    }
                    else
                    {
                        finalList.Add(key.Key, key.Value);
                    }
                }
            }*/
            return finalList;
        }
    }
}
