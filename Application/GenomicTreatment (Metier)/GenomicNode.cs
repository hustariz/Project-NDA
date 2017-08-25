﻿using NetworkAndGenericCalculation.Chunk;
using NetworkAndGenericCalculation.MapReduce;
using NetworkAndGenericCalculation.Sockets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenomicTreatment
{
    public class GenomicNode : Node
    {
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

            base.ProcessInput(dateReceived);
            switch (dateReceived.Method)
            {
                case "method1":
                    // Divisé le fichier 

                    // 

                    BackgroundWorker bc = new BackgroundWorker();
                    bc.DoWork += backgroundWorker1_DoWork;
                    bc.RunWorkerAsync("coucou");
                    break;
            }

            return null;
        }


        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            // INT = TaskID
            String test = (String)e.Argument;
            Console.WriteLine(test);

            //Tuple<String, Reduce, int> tuplou = (Tuple<String, Reduce, int>)e.Argument;
            // doit renvoyer une liste de tuple de char/int
            //List<Tuple<char, int>> listProccesed =  Methodprocess();
            //e.Result = new Tuple<Object, Reduce>(listProccesed,tuplou.Item2);
            
            //= new Tuple<String, IReducer>("toto",);
        }

        private List<Tuple<char, int>> Methodprocess()
        {
            List<Tuple<char, int>> toto = new List<Tuple<char, int>>();
            return toto;
        }
    }
}
