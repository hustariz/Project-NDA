using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkAndGenericCalculation.Node;

namespace NetworkAndGenericCalculation.Worker
{
    // Worker designed to process data
    public class WThread : IWorker
    {

        public WThread(Node.Node node, int id)
        {
            Node = node;
            GetID = id;
            IsAvailable = true;
        }

        public INode Node { get; }

        public int GetID { get; }

        public bool IsAvailable { get; set; }


        public void ExecuteFunction<X, G>(X v1, G v2, Action<X, G> function)
        {
            function((X)v1,(G)v2);
            //throw new NotImplementedException();
        }

        public String ExecuteTask<String,Integer>(Integer data)
        {
            
            return (String)(Object)"coucou";
        }

        public string getMoncul()
        {
            return "Mabite";
        }


        public override string ToString()
        {
            return "Worker ID : " + GetID;
        }
    }
}
