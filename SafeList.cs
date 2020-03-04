using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Erilipah
{
    /// <summary>
    /// Identical to a <see cref="List{T}"/>, but any operations that would modify an enumerating collection are queued to take place after the enumeration is finished.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    public class SafeList<T> : IList<T>
    {
        private readonly List<T> list;
        private readonly ConcurrentQueue<SafeEnumerator> enumerators = new ConcurrentQueue<SafeEnumerator>();
        private Action queued;

        public SafeList(int capacity)
        {
            list = new List<T>(capacity);
        }

        public SafeList(IEnumerable<T> collection)
        {
            list = new List<T>(collection);
        }

        public SafeList()
        {
            list = new List<T>();
        }

        private void CacheIfPossible(Action a)
        {
            if (!enumerators.IsEmpty)
            {
                queued += a;
            }
            else
            {
                a();
            }
        }

        public T this[int index]
        {
            get => list[index];
            set => CacheIfPossible(() => list[index] = value);
        }

        public int Count => list.Count;

        bool ICollection<T>.IsReadOnly => ((IList<T>)list).IsReadOnly;

        public void Add(T item)
        {
            CacheIfPossible(() => list.Add(item));
        }

        public void Clear()
        {
            CacheIfPossible(list.Clear);
        }

        public bool Contains(T item)
        {
            return list.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            list.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new SafeEnumerator(this);
        }

        public int IndexOf(T item)
        {
            return list.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            CacheIfPossible(() => list.Insert(index, item));
        }

        public bool Remove(T item)
        {
            int index = list.IndexOf(item);
            if (index != -1)
            {
                RemoveAt(index);
                return true;
            }
            return false;
        }

        public void RemoveAt(int index)
        {
            CacheIfPossible(() => list.RemoveAt(index));
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private struct SafeEnumerator : IEnumerator<T>
        {
            private readonly SafeList<T> list;
            private int index;

            public SafeEnumerator(SafeList<T> list)
            {
                this.list = list;
                index = -1;
                list.enumerators.Enqueue(this);
            }

            public T Current => list[index];

            object IEnumerator.Current => Current;

            public void Dispose()
            {
                list.enumerators.TryDequeue(out _);
                if (list.enumerators.Count == 0 && list.queued != null)
                {
                    list.queued();
                    list.queued = null;
                }
            }

            public bool MoveNext()
            {
                if (++index < list.Count)
                {
                    return true;
                }
                return false;
            }

            public void Reset()
            {
                index = -1;
            }
        }
    }
}