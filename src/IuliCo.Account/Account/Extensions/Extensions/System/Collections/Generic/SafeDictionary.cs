namespace System.Collections.Generic
{
    using System;
    using System.Reflection;

    // Add 'notnull' constraint to ensure 'T' is always non-nullable
    public class SafeDictionary<T, T2> : Dictionary<T, T2?> where T : notnull
    {
        public SafeDictionary() : base()
        {
        }

        public SafeDictionary(int capacity) : base(capacity)
        {
        }

        public new void Add(T key, T2 value)
        {
            base[key] = value;  // Simplifies the addition; behaves like an upsert
        }

        public new T2? this[T key]  // Explicitly mark the return type as nullable
        {
            get
            {
                if (base.ContainsKey(key))
                {
                    return base[key];
                }
                return default;  // Simplified from 'default(T2)' to 'default'
            }
            set
            {
                base[key] = value;
            }
        }
    }
}
