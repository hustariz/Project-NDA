using NetworkAndGenericCalculation.MapReduce;
using System.Collections.Generic;
using System;
using NetworkAndGenericCalculation.Chunk;
using NetworkAndGenericCalculation.FileTreatment;

namespace NetworkAndGenericCalculation.MapReduce
{
    public class Map<T> : IMapper<T>
    {
0

        private Chunk<T>[] chunks;

        public Map(int chunkLength, IDataReader<T> dataSource)
        {
            DataSource = dataSource;
            ChunkDefaultLength = chunkLength;
            ChunkCount = (int)(DataSource.Length / ChunkDefaultLength);
            ChunkRemainsLength = (int)(Length - ChunkCount * ChunkDefaultLength);
            if (ChunkRemainsLength > 0) ChunkCount++;
            chunks = new Chunk<T>[ChunkCount];
        }

        /// <summary>
        /// Représenta la source de data, à modifier quand il y aura eu création de l'interface DataReader
        /// </summary>
        public IDataReader<T> DataSource { get; protected set; }

        public int ChunkDefaultLength { get; private set; }

        public int ChunkPreferredLength { get; private set; }

        public int ChunkCount { get; private set; }

        public long Length => DataSource.Length;

        public int ChunkRemainsLength { get; private set; }

        public bool IsActive { get; private set; }

        int IMapper<T>.Length => throw new NotImplementedException();

        int IMapper<T>.ChunkDefaultLength => throw new NotImplementedException();

        int IMapper<T>.ChunkCount => throw new NotImplementedException();

        bool IMapper<T>.IsActive => throw new NotImplementedException();

        int IMapper<T>.ChunkRemainsLength => throw new NotImplementedException();

        public void Dispose()
        {
            chunks = null;
            ChunkPreferredLength = ChunkRemainsLength = ChunkCount = 0;
            DataSource = null;
        }

        public Chunk<T> NextChunk()
        {
            for (int i = 0; i < ChunkCount; ++i)
            {
                if (chunks[i] != null)
                {
                    if (chunks[i].State == ChunkState.AVALAIBLE) return chunks[i];

                    continue;
                }

                chunks[i] = new Chunk<T>(DataSource.Next(ChunkLength(i)), i);
                return chunks[i];
            }

            return null;
        }


        private int ChunkLength(int chunkId)
        {
            if (ChunkRemainsLength == 0) return ChunkPreferredLength;
            return chunkId == ChunkCount - 1 ? ChunkRemainsLength : ChunkPreferredLength;
        }

        void IDisposable.Dispose()
        {
            throw new NotImplementedException();
        }

        Chunk<T> IMapper<T>.NextChunk()
        {
            throw new NotImplementedException();
        }
    }
}
