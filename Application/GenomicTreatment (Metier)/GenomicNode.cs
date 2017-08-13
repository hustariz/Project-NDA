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
    public class GenomicNode : Client
    {
        public GenomicNode(Action<string> logger) : base(logger)
        {
        }


        public override void ProcessInput(Byte[] coucou)
        {
            throw new NotImplementedException();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            // INT = TaskID
            Tuple<String, Reduce, int> tuplou = (Tuple<String, Reduce, int>)e.Argument;
            // doit renvoyer une liste de tuple de char/int
            List<Tuple<char, int>> listProccesed =  Methodprocess();
            e.Result = new Tuple<Object, Reduce>(listProccesed,tuplou.Item2);
            
            //= new Tuple<String, IReducer>("toto",);
        }
    }
}
