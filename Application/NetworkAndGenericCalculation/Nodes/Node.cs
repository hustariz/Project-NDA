using NetworkAndGenericCalculation.Worker;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NetworkAndGenericCalculation.Nodes
{
    //Node running on the computer where is it launched.
    public class Node : INode
    {


        private PerformanceCounter processorCounter;
        private PerformanceCounter memoryCounter;

        public Node(int ThreadsCount, string NetworkAdress)
        {

            this.NetworkAdress = NetworkAdress;
            Workers = new WThread[ThreadsCount];
            for (int i = 0; i < ThreadsCount; ++i)
            {
                Workers[i] = new WThread(this, i);
            }
            processorCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            memoryCounter = new PerformanceCounter("Memory", "Available MBytes");
        }



        public IList<IWorker> Workers { get; protected set; }

        public string NetworkAdress { get; protected set; }
    
        // Filter a sequence of valor following a predicate


        public int ActualWorker => Workers.Where(workers => !workers.IsAvailable).Count();

        public bool isWorkerActive => (ActualWorker > 0);

        public float ProcessorUsage => processorCounter.NextValue();

        public float MemoryUsage => memoryCounter.NextValue();

        public bool isAvailable => (ActualWorker == 0);




        public override string ToString() => "HostAdress [" + NetworkAdress + "]";
    }
}
