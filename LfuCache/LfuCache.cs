using System.Collections.Generic;
using Priority_Queue;

namespace LfuCache
{
    public class LfuCache<TKey, TValue>
    {
        private readonly int capacity;
        private readonly Dictionary<TKey, CacheEntry> items;
        private readonly FastPriorityQueue<CacheEntry> lfuHeap;

        public LfuCache(int capacity)
        {
            this.capacity = capacity;
            items = new Dictionary<TKey, CacheEntry>();
            lfuHeap = new FastPriorityQueue<CacheEntry>(capacity);
        }

        public int Count { get; private set; }

        public bool Contains(TKey key) => items.ContainsKey(key);

        public void Add(TKey key, TValue value)
        {
            if (!items.ContainsKey(key))
            {
                if (Count++ == capacity)
                {
                    EvictLfu();
                }
                var cachedItem = new CacheEntry(key, value);
                items.Add(key, cachedItem);
                lfuHeap.Enqueue(cachedItem, 0);
            }
            else
            {
                var cachedItem = items[key];
                cachedItem.Value = value;
            }
        }

        public TValue Get(TKey key)
        {
            var item = items[key];
            lfuHeap.UpdatePriority(item, item.Priority + 1);
            return item.Value;
        }

        public void Remove(TKey key)
        {
            var item = items[key];
            items.Remove(key);
            lfuHeap.Remove(item);
            Count--;
        }

        private void EvictLfu()
        {
            var lfuItem = lfuHeap.Dequeue();
            items.Remove(lfuItem.Key);
            Count--;
        }

        private class CacheEntry : FastPriorityQueueNode
        {
            public CacheEntry(TKey key, TValue value)
            {
                Key = key;
                Value = value;
            }
            public TKey Key { get; }
            public TValue Value { get; set; }
        }
    }
}
