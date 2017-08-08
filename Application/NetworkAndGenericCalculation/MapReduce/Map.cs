using NetworkAndGenericCalculation.MapReduce;
using System.Collections.Generic;
using System;
using NetworkAndGenericCalculation.Chunk;

namespace NetworkAndGenericCalculation.MapReduce
{
    public class Map<T> : IMapper<T>
    {
        /*private IDictionary<K, IList<V>> keyvalues = new Dictionary<K, IList<V>>();

        public IDictionary<K, IList<V>> KeyValues { get { return keyvalues; } }

        public void AddPair(K key, V value)
        {
            if (!keyvalues.ContainsKey(key))
                keyvalues[key] = new List<V>();

            keyvalues[key].Add(value);
        }*/

        private Chunk<T>[] chunks;

        public Map(int chunkLength, String dataSource)
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
        public String DataSource { get; protected set; }

        public int ChunkDefaultLength { get; private set; }

        public int ChunkCount { get; private set; }

        public int Length => DataSource.Length;

        public int ChunkRemainsLength { get; private set; }

        public bool IsActive { get; private set; }

        public void Dispose()
        {
            throw new NotImplementedException();
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

    }
}
