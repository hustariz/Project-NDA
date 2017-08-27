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
        public ConcurrentBag<Dictionary<string,int>> ReduceConccurent { get; set; }
        public DataInput dataReceived { get; set; }
        public int increment;

        public GenomicNode(Action<string> logger) : base(logger)
        {
            ReduceConccurent = new ConcurrentBag<Dictionary<string, int>>();
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
                    Console.WriteLine("SUBTASK RECEIVED : " + dateReceived.SubTaskId);
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
                Interlocked.Increment(ref counter);
                Console.WriteLine(counter);
                string[] dataTab = (string[])e.Argument;
                Dictionary<string, int> workReduced = new Dictionary<string, int>();
                workReduced = CountBases(dataTab);
                ReduceConccurent.Add(workReduced);
                //Thread.Sleep(100);
                //reduceResult = ReduceMethod1(reduceResult, workReduced);
                //ReduceMethod1(workReduced);
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

            if (counter == 0)
            {

                Dictionary<string, int> datatruc =  lastReduce(ReduceConccurent);
 
                foreach(string key in datatruc.Keys)
                {
                    Console.WriteLine("KEY : "+ key + ":" + datatruc[key]);
                }
               


                 DataInput dataI = new DataInput()
                 {
                     TaskId = dataReceived.TaskId,
                     SubTaskId = dataReceived.SubTaskId,
                     Method = "globalReduceMethod1",
                     Data = reduceResult,
                     NodeGUID = dataReceived.NodeGUID
                 };

                Console.WriteLine("SUBTASK SENT : " + dataReceived.SubTaskId);
                Send(ClientSocket, dataI);

               reduceResult = null;
               ReduceConccurent = null;
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
                        
                        BackgroundWorker bc = new BackgroundWorker();
                        bc.DoWork += backgroundWorker1_DoWork;
                        bc.RunWorkerCompleted += Bw_OnWorkComplete;
                        bc.RunWorkerAsync(tabToprocess[i]);
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

        public void ReduceMethod1(List<Tuple<string, int>> listMapped)
        {
            //List<Tuple<char, int>> newListGlobal = listGlobale;

            //Console.WriteLine("INSIDE REDUCE METHOD  : " + listMapped.Count);

                for (int i = 0; i < listMapped.Count; i++)
                {
                    /* bool present = false;
                     foreach(Tuple<char,int> listTuple in ReduceConccurent)
                     {
                         if(listTuple.Item1 == listMapped[i].Item1)
                         {
                             present = true;
                             char char1 = listTuple.Item1;
                             int intToAdd = listTuple.Item2 + listMapped[i].Item2;

                             Console.WriteLine(listTuple.Item1 +" : "+ listTuple.Item2);
                             Tuple<char, int> result;
                             ReduceConccurent.TryTake(out result);
                             Console.WriteLine("TryTake : {0}", result);
                             ReduceConccurent.Add(new Tuple<char, int>(char1, intToAdd));
                         }
                     }*/

                    //TODO this.ReduceConccurent.Add(listMapped[i]);

                   /* if (!present)
                        ReduceConccurent.Add(listMapped[i]); */
                }
            
            //return newListGlobal;
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

        public Dictionary<string, int> lastReduce(ConcurrentBag<Dictionary<string,int>> datas)
        {

            Dictionary<string, int> finalList = new Dictionary < string, int>();

            int nbOccur;

            foreach (Dictionary<string, int> data in datas)
            {

                foreach(string key in data.Keys)
                {
                    if (finalList.TryGetValue(key, out nbOccur))
                    {

                        finalList[key] = nbOccur + data[key];
                    }
                    else
                    {
                        finalList.Add(key, data[key]);
                    }
                }
               
               
            }



            return finalList;
        }
    }
}
