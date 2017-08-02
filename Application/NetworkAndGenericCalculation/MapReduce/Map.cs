using System.Collections.Generic;

namespace c_projet_adn.Worker
{
    public class Map<K, V>
    {
        private IDictionary<K, IList<V>> keyvalues = new Dictionary<K, IList<V>>();

        public IDictionary<K, IList<V>> KeyValues { get { return keyvalues; } }

        public void AddPair(K key, V value)
        {
            if (!keyvalues.ContainsKey(key))
                keyvalues[key] = new List<V>();

            keyvalues[key].Add(value);
        }
    }
}
