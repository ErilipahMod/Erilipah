using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erilipah.Core
{
    public class MutableLookup<TKey, TElement> // too lazy to implement ILookup
    {
        private readonly Dictionary<TKey, List<TElement>> values = new Dictionary<TKey, List<TElement>>();

        public IEnumerable<TElement> this[TKey key] => values[key];

        public void Add(TKey key, TElement value)
        {
            if (values.TryGetValue(key, out List<TElement> elements))
            {
                elements.Add(value);
            }
            else
            {
                values[key] = new List<TElement>() { value };
            }
        }

        public bool Remove(TKey key, TElement value)
        {
            if (values.TryGetValue(key, out List<TElement> elements))
            {
                return elements.Remove(value);
            }
            return false;
        }

        public bool TryGetValue(TKey key, out IEnumerable<TElement> elements)
        {
            bool ret = values.TryGetValue(key, out List<TElement> e);
            elements = e;
            return ret;
        }
    }
}
