using System.Collections.Generic;

namespace System
{
    public class SortEntry<Key, Value> where Key : notnull
    {
        // Initialize 'Values' directly to ensure it's never null.
        public Dictionary<Key, Value> Values = new Dictionary<Key, Value>();

        // Optionally, you can add constructors to further initialize 'Values' or perform other setups.
        public SortEntry()
        {
        }

        // Example constructor that allows initial capacity specification.
        public SortEntry(int capacity)
        {
            Values = new Dictionary<Key, Value>(capacity);
        }
    }
}
