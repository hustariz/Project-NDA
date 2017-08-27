using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using NetworkAndGenericCalculation.Sockets;
using NetworkAndGenericCalculation.MapReduce;
using NetworkAndGenericCalculation.Chunk;

namespace GenomicTreatment
{
    public class GenomicServeur : Server
    {
        public List<Tuple<char, int>> globalResultMethod1 { get; set; }
        public int increment { get; set; }

        public GenomicServeur(IPAddress host, int portNumber, Action<string> servLogger, Action<string, string, int, int, float, float> gridupdater) : base(host, portNumber, servLogger, gridupdater)
        {
            globalResultMethod1 = new List<Tuple<char, int>>();
        }

       

        public override void ProcessInput(DataInput dateReceived)
        {


            base.ProcessInput(dateReceived);
            switch (dateReceived.Method)
            {
                case "globalReduceMethod1":

                    Console.WriteLine("globalReduceMethod : " + increment++);

                    for (int e = 0; e < taskList.Count; e++) { 

                        if(taskList[e].Item2 == dateReceived.NodeGUID && taskList[e].Item3 == dateReceived.SubTaskId)
                        {
                           
                            Console.WriteLine(taskList[e].Item1.Count);
                            Console.WriteLine(dateReceived.NodeGUID + " : " + dateReceived.SubTaskId);
                            List<Tuple<string, int>> listReceived = new List<Tuple<string, int>>();
                            listReceived = (List<Tuple<string, int>>)dateReceived.Data;
                            Console.WriteLine(listReceived.Count);
                            Tuple<List<Tuple<string, int>>, string, int, bool> newTuplou = new Tuple<List<Tuple<string, int>>, string, int, bool>(listReceived, dateReceived.NodeGUID, dateReceived.SubTaskId, true);
                            taskList[e] = newTuplou;

                            Console.WriteLine("TASKLIST E : " + taskList[e].Item2 + " : " + taskList[e].Item3 + " : " + taskList[e].Item4 +" : "+ taskList[e].Item1.Count);

                        }

                    }

                    //globalResultMethod1 = ReduceMethod1(globalResultMethod1, (List<Tuple<char, int>>)dateReceived.Data);

                    /*foreach (Tuple<char, int> dataTa in globalResultMethod1)
                    {
                        Console.WriteLine("dataTa Serveur: " + dataTa.Item1 + " " + dataTa.Item2);
                    }*/


                   /* foreach (Tuple<string, int> dataTa in (List<Tuple<string,int>>)dateReceived.Data)
                  {
                      Console.WriteLine("dataTa Serveur: " + dataTa.Item1 + " " + dataTa.Item2);
                  }*/



                    for (int i = 0; i < nodesConnected.Count; i++)
                    {
                        if (nodesConnected[i].NodeID == dateReceived.NodeGUID)
                        {
                            nodesConnected[i].isAvailable = true;
                        }
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

                    Console.WriteLine(taskList.Count);

                    foreach (Tuple<List<Tuple<string, int>>, string, int, bool> newTuplou  in taskList)
                    {
                        foreach(Tuple<string, int> tuplou2 in newTuplou.Item1){

                            Console.WriteLine("Tuploud : ID " + newTuplou.Item2 + " SubTask:" + newTuplou.Item3 +" KEY:"+ tuplou2.Item1 + " VALUE:" + tuplou2.Item2);

                        }
                    }
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
    }
}
