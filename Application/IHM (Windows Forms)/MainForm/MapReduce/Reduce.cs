using System.Collections.Generic;

namespace c_projet_adn.Worker
{
    public class Reduce<K, V>
    {
        private IList<KVPair<K, V>> pairs = new List<KVPair<K, V>>();

        public IEnumerable<KVPair<K, V>> Pairs { get { return pairs; } }

        public void AddPair(K key, V value)
        {
            pairs.Add(new KVPair<K, V>() { Key = key, Value = value });
        }
    }
}
