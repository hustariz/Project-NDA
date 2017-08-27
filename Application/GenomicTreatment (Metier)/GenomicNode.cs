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
        public ConcurrentBag<Tuple<string, int>> ReduceConccurent = new ConcurrentBag<Tuple<string, int>>();
        public DataInput dataReceived { get; set; }
        public int increment;

        public GenomicNode(Action<string> logger) : base(logger)
        {
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
                    ReduceConccurent = new ConcurrentBag<Tuple<string, int>>();
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
                List<Tuple<string, int>> workReduced = new List<Tuple<string, int>>();
                workReduced = CountBases(dataTab);
                //Thread.Sleep(100);
                //reduceResult = ReduceMethod1(reduceResult, workReduced);
                ReduceMethod1(workReduced);
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
               
                foreach (Tuple<string, int> dataTa in ReduceConccurent)
                {
                    
                    //Console.WriteLine("ReduceConcurrent Client: " + dataTa.Item1 + " " + dataTa.Item2);
                    //Console.WriteLine("Index en cours :" + i + " ReduceConcurrent Client: " + dataTa.Item1 + " " + dataTa.Item2);

                    reduceResult = lastReduce(dataTa, reduceResult);
                  
                   /* foreach (Tuple<char, int> reduceData in reduceResult)
                    {
                        Console.WriteLine("reduceResult Client: " + reduceData.Item1 + " " + reduceData.Item2);
                       
                    } */
                }
                /*
                foreach(Tuple<char, int> dataTa in reduceResult)
                {
                    Console.WriteLine( dataTa.Item1 + " : " + dataTa.Item2);
                }
              

                /*
                foreach (Tuple<char, int> dataDA in lastList)
                {
                    Console.WriteLine("dataDA Client: " + dataDA.Item1 + " " + dataDA.Item2);
                }*/

                 DataInput dataI = new DataInput()
                 {
                     TaskId = dataReceived.TaskId,
                     SubTaskId = dataReceived.SubTaskId,
                     Method = "globalReduceMethod1",
                     Data = reduceResult,
                     NodeGUID = dataReceived.NodeGUID
                 };
                 Send(ClientSocket, dataI);

               reduceResult = null;
               ReduceConccurent = null;
            }
        }


        private List<Tuple<string, int>> CountBases(string[] dataTab)
        {
            List<Tuple<string, int>> dataToReduce = new List<Tuple<string, int>>();

            /*
            string finalData = "";

            foreach (string dataS in dataTab)
            {
                Console.WriteLine("DATAS :" + dataS);
                finalData += dataS;
            }

            char[] chartab;
            chartab = null;

            chartab = finalData.ToCharArray();

            foreach (char data in chartab)
            {

                if (dataToReduce.Count == 0 || dataToReduce == null)
                {
                    dataToReduce.Add(new Tuple<char, int>(data, 1));
                }

                bool present = false;

                Console.WriteLine("INDEX DE MERDE : " + dataToReduce.Count);
                for(i = 0; i < dataToReduce.Count; i++)
                {
                    if(dataToReduce[i].Item1 == data)
                    {
                        present = true;
                        dataToReduce[i] = new Tuple<char, int>(dataToReduce[i].Item1,dataToReduce[i].Item2+1);
                    }
                }
                if (!present)
                {
                    dataToReduce.Add(new Tuple<char, int>(data, 1));
                }
            } */

            foreach(string datas in dataTab)
            {
                if (dataToReduce.Count == 0 || dataToReduce == null)
                {
                    dataToReduce.Add(new Tuple<string, int>(datas, 1));
                }

                bool present = false;

                Console.WriteLine("INDEX DE MERDE : " + dataToReduce.Count);
                for (i = 0; i < dataToReduce.Count; i++)
                {
                    if (dataToReduce[i].Item1 == datas)
                    {
                        present = true;
                        dataToReduce[i] = new Tuple<string, int>(dataToReduce[i].Item1, dataToReduce[i].Item2 + 1);
                    }
                }
                if (!present)
                {
                    dataToReduce.Add(new Tuple<string, int>(datas, 1));
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
                    //Console.WriteLine("TEXT TAILLE : " + text.Length);
                    List<string[]> tabToprocess = Split(text,text.Length);
                    int e = 0;
                    for(int i = 0; i < tabToprocess.Count; i++)
                    {
                        Console.WriteLine(e++);
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

                    this.ReduceConccurent.Add(listMapped[i]);

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

        public List<Tuple<string,int>> lastReduce(Tuple<string, int> concurr, List<Tuple<string, int>> finalList)
        {

        //List<Tuple<char, int>> finalList = new List<Tuple<char, int>>();


            if (finalList == null || finalList.Count == 0)
            {
                finalList.Add(concurr);
                return finalList;
            }
            else
            {
                bool present = false;
                for (int i = 0; i < finalList.Count; i++)
                {
                    
                    if (finalList[i].Item1 == concurr.Item1)
                    {
                        //Console.WriteLine("INSIDE FINAL LIST == : " + finalList[i].Item1 +" : " + concurr.Item1);
                        present = true;
                        finalList[i] = new Tuple<string, int>(finalList[i].Item1, finalList[i].Item2 + concurr.Item2);
                    }
                    
                }
                if (!present)
                    {
                        //Console.WriteLine("INSIDE FINAL !Present : " + concurr.Item1 + " : " + concurr.Item2);
                        finalList.Add(concurr);
                    }
            }
            return finalList;
        }
    }
}
