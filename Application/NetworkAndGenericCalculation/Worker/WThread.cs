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

        public WThread(LocalNode node, int id)
        {
            Node = node;
            GetID = id;
            IsAvailable = true;
        }

        public INode Node { get; }

        public int GetID { get; }

        public bool IsAvailable { get; set; }

        public override string ToString()
        {
            return "Worker ID : " + GetID;
        }
    }
}
