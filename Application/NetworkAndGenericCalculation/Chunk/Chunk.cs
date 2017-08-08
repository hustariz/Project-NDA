using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkAndGenericCalculation.Chunk
{
    public class Chunk<T>
    {
        public int Index { get; protected set; }
        public ChunkState State { get; set; }
        public T[] Data { get; protected set; }

        public long RealLength
        {
            get
            {
                if (Data == null) return -1;
                long length = 0;
                for (int i = 0; i < Data.Length; ++i)
                {
                    if (Data[i] != null) ++length;
                }
                return length;
            }
        }

        public Chunk(T[] data, int index)
        {
            Data = data;
            Index = index;
            State = ChunkState.AVALAIBLE;
        }
    }
}

