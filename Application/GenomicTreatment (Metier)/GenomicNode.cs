using NetworkAndGenericCalculation.Chunk;
using NetworkAndGenericCalculation.MapReduce;
using NetworkAndGenericCalculation.Sockets;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GenomicTreatment
{
    public class GenomicNode : Client
    {
        public int counter = 0;
        public List<Tuple<string, int>> reduceResult = new List<Tuple<string, int>>();
        //public ConcurrentBag<Tuple<Dictionary<string,int>,string>> ReduceConccurent { get; set; }
        public ConcurrentDictionary<int,Tuple<bool,Dictionary<string,int>>> dico { get; set; }
        public DataInput dataReceived { get; set; }
        public int increment;

        public GenomicNode(Action<string> logger) : base(logger)
        {
            //ReduceConccurent = new ConcurrentBag<Tuple<Dictionary<string, int>, string>>();
            dico = new ConcurrentDictionary<int, Tuple<bool, Dictionary<string, int>>>();
        }

        /// <summary>
        /// Liste des méthodes de calculs génomiques
        /// </summary>
        /// <returns></returns>
        public new List<string> nodeMethods()
        {
            List<string> methodList = new List<string>();
            methodList.Add("CountOccurence");
            methodList.Add("CountBasePairs");
            return methodList;
          
        }


        public override Object ProcessInput(DataInput dateReceived)
        {
            dataReceived = dateReceived;

            base.ProcessInput(dateReceived);
            switch (dateReceived.Method)
            {
                case "method1":
                    reduceResult = new List<Tuple<string, int>>();
                    string[] dataReceiveded = (string[])dateReceived.Data;
                    map("method1", dataReceiveded, 0, 0);
                    break;
            }
            return null;
        }


        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                //Interlocked.Increment(ref counter);
                Tuple<int,string[]> dataTab = (Tuple<int, string[]>)e.Argument;
                Dictionary<string, int> workReduced = new Dictionary<string, int>();
                workReduced = CountBases(dataTab.Item2);

                foreach(string key in workReduced.Keys)
                {
                    Console.WriteLine("Prems:" + dataTab.Item1 + ":" + key + ":"+ workReduced[key]);
                }
                e.Result = new Tuple<int, Dictionary<string, int>>(dataTab.Item1, workReduced);

                //ReduceConccurent.Add(workReduced);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception : " + ex);
                throw ex;
            }
           

        }

        private void Bw_OnWorkComplete(object sender, RunWorkerCompletedEventArgs e)
        {

            Interlocked.Decrement(ref counter);

            Tuple<int, Dictionary<string, int>> tupleresult = (Tuple < int, Dictionary< string, int>>)e.Result;

            foreach(string key in tupleresult.Item2.Keys)
            {
                Console.WriteLine("Result:"+tupleresult.Item1 + ":" + key + ":" + tupleresult.Item2[key]);
            }

            foreach(int key in dico.Keys)
            {
                if(key == tupleresult.Item1)
                {
                    dico[key] = new Tuple<bool, Dictionary<string, int>>(true, tupleresult.Item2);
                }

            }

            bool isNotComplete = false;
            foreach (int key in dico.Keys)
            {
                if(dico[key].Item1 != true)
                {
                    isNotComplete = true;
                }
            }


            if (!isNotComplete)
            {

              
                Dictionary<string, int> datatruc =  lastReduce(dico);
 

                 DataInput dataI = new DataInput()
                 {
                     TaskId = dataReceived.TaskId,
                     SubTaskId = dataReceived.SubTaskId,
                     Method = "globalReduceMethod1",
                     Data = datatruc,
                     NodeGUID = dataReceived.NodeGUID
                 };

                Console.WriteLine("SUBTASK SENT : " + dataReceived.SubTaskId);
                Send(ClientSocket, dataI);

               reduceResult = null;
               //ReduceConccurent = null;
            }
        }


        private Dictionary<string,int> CountBases(string[] dataTab)
        {
            Dictionary<string,int> dataToReduce = new Dictionary<string, int>();

            int nbOccur;

            foreach(string data in dataTab)
            {
                if (dataToReduce.TryGetValue(data,out nbOccur)){
                    dataToReduce[data] = nbOccur + 1;
                }else
                {
                    dataToReduce.Add(data, 1);
                }
            }

            return dataToReduce;
            
        }

        private List<Tuple<char, int>> Methodprocess()
        {
            List<Tuple<char, int>> toto = new List<Tuple<char, int>>();
            return toto;
        }

        public override object map(string Method, string[] text, int chunkSize, int offsets)
        {
            //Creer des bw auxquel on va associer la bonne méthode à faire
            base.map(Method, text, chunkSize, offsets);
            switch (Method)
            {
                case "1":
                    Console.WriteLine("J'suis MAP");
                    break;

                case "method1":
                    
                    List<string[]> tabToprocess = Split(text,text.Length);
                    int e = 0;
                    for(int i = 0; i < tabToprocess.Count; i++)
                    {
                        Interlocked.Increment(ref counter);
                        Dictionary<string, int> ProccessDico = new Dictionary<string, int>();
                        Tuple<bool, Dictionary<string, int>> ProcessTuple = new Tuple<bool, Dictionary<string, int>>(false, ProccessDico);
                        dico.TryAdd(counter, ProcessTuple);
                        BackgroundWorker bc = new BackgroundWorker();
                        bc.DoWork += backgroundWorker1_DoWork;
                        bc.RunWorkerCompleted += Bw_OnWorkComplete;
                        bc.RunWorkerAsync(new Tuple<int,string[]>(counter,tabToprocess[i]));
                    }

                    break; 
            }
            return null;
        }

        public override object reduce()
        {
            throw new NotImplementedException();
        }

        public GenomicNode(string adress, string port, string name) :base(adress,port,name)
        {
            reduceResult = new List<Tuple<string, int>>();
            //ReduceConccurent = new ConcurrentBag<Tuple<char, int>>();


        }

        public List<string[]> Split(string[] array, int index)
        {

            string[] out1 = array.Take(index/2).ToArray();
            string[] out2 = array.Skip(index/2).ToArray();
            string[] tab1 = out1.Take(out1.Length/2).ToArray();
            string[] tab2 = out1.Skip(out1.Length/2).ToArray();
            string[] tab3 = out2.Take(out2.Length/2).ToArray();
            string[] tab4 = out2.Skip(out2.Length/2).ToArray();

            List<string[]> tabToprcess = new List<string[]>();

            tabToprcess.Add(tab1);
            tabToprcess.Add(tab2);
            tabToprcess.Add(tab3);
            tabToprcess.Add(tab4);

            Console.WriteLine(tab1.Length+" : "+ tab2.Length+" : "+ tab3.Length + " : "+ tab4.Length);

            return tabToprcess;
        }

        public Dictionary<string, int> lastReduce(ConcurrentDictionary<int, Tuple<bool, Dictionary<string, int>>> datas)
        {

            Dictionary<string, int> finalList = new Dictionary <string, int>();

           
            foreach (var tuple in datas)
            {
                foreach(var key in tuple.Value.Item2)
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
            }
            return finalList;
        }
    }
}
