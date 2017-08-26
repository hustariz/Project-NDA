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
        public List<Tuple<char, int>> reduceResult { get; set; }
        public ConcurrentBag<Tuple<char,int>> ReduceConccurent { get; set; }
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
                    map("method1", (string[])dateReceived.Data, 0, 0);
                    break;

            }

            return null;
        }


        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                Interlocked.Increment(ref counter);
                string[] dataTab = (string[])e.Argument;
                List<Tuple<char, int>> workReduced = new List<Tuple<char, int>>();
                workReduced = CountBases(dataTab);
                reduceResult = ReduceMethod1(reduceResult, workReduced);
            }
            catch (Exception ex)
            {            
                throw ex;
            }
           

        }

        private void Bw_OnWorkComplete(object sender, RunWorkerCompletedEventArgs e)
        {

            Interlocked.Decrement(ref counter);

            if (counter == 0)
            {

                foreach (Tuple<char, int> dataTa in reduceResult)
                {
                    Console.WriteLine("dataTa Client: " + dataTa.Item1 + " " + dataTa.Item2);
                }

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
            }
        }


        private List<Tuple<char, int>> CountBases(string[] dataTab)
        {
            List<Tuple<char, int>> dataToReduce = new List<Tuple<char, int>>();

            string finalData = "";

            foreach(string dataS in dataTab)
            {
                finalData += dataS;
            }

            char[] chartab;
            chartab = null;

            chartab = finalData.ToCharArray();

            foreach (char data in chartab)
            {

                bool present = false;
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
            reduceResult = new List<Tuple<char, int>>();
        }

        public List<Tuple<char, int>> ReduceMethod1(List<Tuple<char, int>> listGlobale, List<Tuple<char, int>> listMapped)
        {
            List<Tuple<char, int>> newListGlobal = listGlobale;


            if (newListGlobal == null || newListGlobal.Count == 0)
            {
                for(int i = 0; i < listMapped.Count; i++)
                {
                    ReduceConccurent.Add(listMapped[i]);
                    ReduceConccurent.TryTake();
                }
                newListGlobal = listMapped;
            } else
            {
                for (int i = 0; i < listMapped.Count; i++)
                {
                    bool present = false;
                    for (int e = 0; e < newListGlobal.Count; e++)
                    {
                        if (newListGlobal[e].Item1 == listMapped[i].Item1)
                        {
                            present = true;
                            newListGlobal[e] = new Tuple<char, int>(newListGlobal[i].Item1, newListGlobal[i].Item2 + listMapped[i].Item2);
                        }
                    }
                    if (!present)
                        newListGlobal.Add(listMapped[i]);
                }
            }
            return newListGlobal;
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
    }
}
