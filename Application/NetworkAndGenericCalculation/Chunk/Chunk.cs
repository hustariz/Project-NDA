using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkAndGenericCalculation.Chunk
{
    [Serializable]
    public class Chunk
    {
        //public String ClientGUID;
        public String NodeGUID;
        public int TaskId;
        public int SubTaskId;
        public String Method { get; set; }
        public Object Data { get; set; }

        public override string ToString()
        {
            return "Data -> Method : " + Method  + " NodeGuid : " + NodeGUID + " TaskId  : " + TaskId + " Data : " + Data;
        }
    }
}
