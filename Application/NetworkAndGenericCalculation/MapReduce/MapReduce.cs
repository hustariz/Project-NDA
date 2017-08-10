using System;
using System.Collections.Generic;

namespace c_projet_adn.Worker
{
    public class MapReduce<K1, V1, K2, V2, K3, V3>
    {
        //private Action<K1, V1, Map<K2, V2>> map;
        //private Action<K2, IList<V2>, Reduce<K3, V3>> reduce;
        //private Map<K2, V2> mapcontext = new Map<K2, V2>();
        //private Reduce<K3, V3> redcontext = new Reduce<K3, V3>();

        //public MapReduce(Action<K1, V1, Map<K2, V2>> map, Action<K2, IList<V2>, Reduce<K3, V3>> reduce)
        //{
        //    this.map = map;
        //    this.reduce = reduce;
        //}

        //public void MapKeyValue(K1 key, V1 value)
        //{
        //    map(key, value, mapcontext);
        //}

        //public void MapValue(V1 value)
        //{
        //    map(default(K1), value, mapcontext);
        //}

        //public void Map(IEnumerable<KVPair<K1, V1>> keyvalues)
        //{
        //    foreach (var keyvalue in keyvalues)
        //        MapKeyValue(keyvalue.Key, keyvalue.Value);
        //}

        //public IEnumerable<KVPair<K3, V3>> Reduce()
        //{
        //    foreach (var key in mapcontext.KeyValues.Keys)
        //        reduce(key, mapcontext.KeyValues[key], redcontext);

        //    return redcontext.Pairs;
        //}
    }
}
