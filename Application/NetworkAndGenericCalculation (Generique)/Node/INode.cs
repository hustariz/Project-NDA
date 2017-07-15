using NetworkAndGenericCalculation.Worker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkAndGenericCalculation.Node
{
    // A node is composed of thread which can be local, or remote connected via sockets.
    public interface INode
    {
        // Get network Adress of the node.
        string IpAdress { get;  }

        // Get number of worker doing job for this node.
        int ActualWorker { get; }

        // Get the CPU usage of the node.
        float CPuUsage { get; }

        // Get the Memory usage of the node.
        float MemoryUsage { get; }
    }
}
