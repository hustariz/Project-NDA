using NetworkAndGenericCalculation.Chunk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetworkAndGenericCalculation.MapReduce
{
    public interface IMapper
    {
        int Length { get; }
        int ChunkDefaultLength { get; }
        int ChunkCount { get; }
        bool IsActive { get; }
        int ChunkRemainsLength { get; }

        object map(string Method, string[] text, int chunkSize, int offsets);

        //Chunk<T> NextChunk();
    }
}
