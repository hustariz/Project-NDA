using NetworkAndGenericCalculation.Chunk;
using NetworkAndGenericCalculation.MapReduce;
using NetworkAndGenericCalculation.Sockets;
using System;
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
        public DataInput dataReceived { get; set; }
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
                    // Divisé le fichier 

                    // 
                    map("method1", (string[])dateReceived.Data, 0, 0);

                    char[] dataToProcess = (char[])dateReceived.Data;
                    BackgroundWorker bc = new BackgroundWorker();
                    bc.DoWork += backgroundWorker1_DoWork;
                    bc.RunWorkerCompleted += Bw_OnWorkComplete;
                    bc.RunWorkerAsync(dataToProcess);
                    break;

            }

            return null;
        }


        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                BackgroundWorker worker = sender as BackgroundWorker;

                Interlocked.Increment(ref counter);

                string[] dataTab = (string[])e.Argument;

                List<Tuple<char, int>> workReduced = new List<Tuple<char, int>>();

                workReduced = CountBases(dataTab);

                ReduceMethod1(workReduced, workReduced);

                e.Result = workReduced;
            }
            catch (Exception ex)
            {
                // here  catch your exception and decide what to do                  
                throw ex;
            }
           

        }

        private void Bw_OnWorkComplete(object sender, RunWorkerCompletedEventArgs e)
        {

            Interlocked.Decrement(ref counter);

            if (counter == 0)
            {
                DataInput dataI = new DataInput()
                {
                    TaskId = dataReceived.TaskId,
                    SubTaskId = dataReceived.SubTaskId,
                    Method = "globalReduceMethod1",
                    Data = reduceResult,
                    NodeGUID = dataReceived.NodeGUID
                };
                Send(ClientSocket, dataI);
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
            foreach(Tuple<char,int> data in dataToReduce)
            {
                Console.WriteLine("Truc qui compte mon cul : "+data.Item1+" "+data.Item2);
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
                    //string dataToProcess = (String)dateReceived.Data;
                    BackgroundWorker bc = new BackgroundWorker();
                    bc.DoWork += backgroundWorker1_DoWork;
                    bc.RunWorkerAsync(text);
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

            if (listGlobale == null || listGlobale.Count == 0)
            {

                listGlobale = listMapped;

            } else
            {
                foreach (Tuple<char, int> inputpl in listMapped)
                {
                    bool present = false;
                    for (int i = 0; i < listGlobale.Count; i++)
                    {
                        if (listGlobale[i].Item1 == inputpl.Item1)
                        {
                            present = true;
                            listGlobale[i] = new Tuple<char, int>(listGlobale[i].Item1, listGlobale[i].Item2 + inputpl.Item2);
                        }
                    }
                    if (!present)
                        listGlobale.Add(inputpl);
                }
            }

          
            return listGlobale;

        }
    }
}
