using NetworkAndGenericCalculation.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkAndGenericCalculation.Chunk;

namespace NetworkAndGenericCalculation.Nodes
{
    class GenericNode : Client
    {

        public GenericNode(String adress, String port,String name):base(adress,port,name)
        {
        }

        public override List<string> nodeMethods()
        {
            throw new NotImplementedException();
        }

        public override object ProcessInput(DataInput dataI)
        {
            switch (dataI.Method)
            {
                case "IdentNode":
                    NodeID = (String)dataI.Data;
                    break;
            }



            return null;
        }
    }
}
