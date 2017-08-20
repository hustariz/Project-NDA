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

        public GenomicServeur(IPAddress host, int portNumber, Action<string> servLogger, Action<string, string, int, int, float, float> gridupdater) : base(host, portNumber, servLogger, gridupdater)
        {

        }

        public override void ProcessInput(DataInput dateReceived)
        {
            

            base.ProcessInput(dateReceived);
            switch (dateReceived.Method)
            {
                case "globalReduceMethod1":
                    // Divisé le fichier 

                    //
                    
                    foreach(Tuple<char, int> dataTa in (List<Tuple<char, int>>)dateReceived.Data)
                    {
                        Console.WriteLine("dataTa : "+dataTa.Item1+" "+dataTa.Item2); 
                    }

                    ReduceMethod1(globalResultMethod1, (List<Tuple<char, int>>)dateReceived.Data);

                    for(int i =0; i < tasksInProcess.Count; i++)
                    {
                        if(tasksInProcess[i].Item1 == dateReceived.NodeGUID && tasksInProcess[i].Item4 == dateReceived.SubTaskId)
                        {
                            tasksInProcess[i] = new Tuple<string, int, string, int>(tasksInProcess[i].Item1, tasksInProcess[i].Item2, "isAvalaible", tasksInProcess[i].Item4);
                        }

                            
                    }
                    foreach(Tuple<char, int> datiti in (List<Tuple<char, int>>)globalResultMethod1)
                    {
                        Console.WriteLine("datiti : " + datiti.Item1 + " " + datiti.Item2);
                    }

                        tasksInProcess.Add(new Tuple<string, int, string, int>(dateReceived.NodeGUID, 1, "done", subTaskCount));
                    break;
            }

           
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
                    }
                }
            }

            pairs = pairsList.ToArray<string>();
            Tuple<int, string[]> chunkTosend = new Tuple<int, string[]>(lastIndex+i , pairs);
            return chunkTosend;
        }

        public List<Tuple<char, int>> ReduceMethod1(List<Tuple<char, int>> listGlobale, List<Tuple<char, int>> listMapped)
        {
            List<Tuple<char, int>> result = (List<Tuple<char, int>>)listGlobale;
            foreach (Tuple<char, int> inputpl in (List<Tuple<char, int>>)listMapped)
            {
                bool present = false;
                for (int i = 0; i < result.Count; i++)
                {
                    if (result[i].Item1 == inputpl.Item1)
                    {
                        present = true;
                        result[i] = new Tuple<char, int>(result[i].Item1, result[i].Item2 + inputpl.Item2);
                    }
                }
                if (!present)
                    result.Add(inputpl);
            }

            return result;

        }
    }
}
