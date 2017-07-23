using NetworkAndGenericCalculation.Node;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkAndGenericCalculation.Worker
{

    public interface IWorker
    {
        // Get the node of the worker.
        INode Node { get; }
        
        // Get the worker ID.
        int GetID { get; }

        //Returns true if worker available.
        bool IsAvailable { get; set; }


        R ExecuteTask<R ,O>( O data);

        void ExecuteFunction<X,G> (X v1, G v2,Action<X, G> function);
    }
}
