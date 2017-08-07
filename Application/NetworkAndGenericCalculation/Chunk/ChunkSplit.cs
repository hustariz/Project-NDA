using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkAndGenericCalculation.Chunk
{
    public class ChunkSplit
    {

        public byte[] chunkBytes { get; set; }
        public int offset { get; set; }

        public ChunkSplit()
        {

        }

        public ChunkSplit(byte[] byteTab, int currentOffset)
        {

            this.chunkBytes = byteTab;
            this.offset = currentOffset;


        }

    }
}
