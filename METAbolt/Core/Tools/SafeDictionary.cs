//  Copyright (c) 2008 - 2011, www.metabolt.net (METAbolt)
//  All rights reserved.

//  Redistribution and use in source and binary forms, with or without modification, 
//  are permitted provided that the following conditions are met:

//  * Redistributions of source code must retain the above copyright notice, 
//    this list of conditions and the following disclaimer. 
//  * Redistributions in binary form must reproduce the above copyright notice, 
//    this list of conditions and the following disclaimer in the documentation 
//    and/or other materials provided with the distribution. 

//  THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND 
//  ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED 
//  WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. 
//  IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, 
//  INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT 
//  NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR 
//  PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, 
//  WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) 
//  ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
//  POSSIBILITY OF SUCH DAMAGE.


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace METAbolt
{
    public class SafeDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private readonly object syncRoot = new object();
        private Dictionary<TKey, TValue> d = new Dictionary<TKey, TValue>();

        #region IDictionary<TKey,TValue> Members

        public void Add(TKey key, TValue value)
        {
            try
            {
                lock (syncRoot)
                {
                    d.Add(key, value);
                }
            }
            catch { ; }
        }

        public bool ContainsKey(TKey key)
        {
            lock (syncRoot)
            {
                return d.ContainsKey(key);
            }
        }

        public ICollection<TKey> Keys
        {
            get
            {
                lock (syncRoot)
                {
                    return d.Keys;
                }
            }
        }

        public bool Remove(TKey key)
        {
            lock (syncRoot)
            {
                try
                {
                    return d.Remove(key);
                }
                catch { return false; }
            }
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            lock (syncRoot)
            {
                return d.TryGetValue(key, out value);
            }
        }

        public ICollection<TValue> Values
        {
            get
            {
                lock (syncRoot)
                {
                    return d.Values;
                }
            }
        }

        public TValue this[TKey key]
        {
            get
            {
                try
                {
                    return d[key];
                }
                catch { return default(TValue); }
            }
            set
            {
                lock (syncRoot)
                {
                    d[key] = value;
                }
            }
        }

        #endregion

        #region ICollection<KeyValuePair<TKey,TValue>> Members

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            try
            {
                lock (syncRoot)
                {
                    ((ICollection<KeyValuePair<TKey, TValue>>)d).Add(item);
                }
            }
            catch { ; }
        }

        public void Clear()
        {
            try
            {
                lock (syncRoot)
                {
                    d.Clear();
                }
            }
            catch { ; }
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            lock (syncRoot)
            {
                return ((ICollection<KeyValuePair<TKey, TValue>>)d).Contains(item);
            }
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int
        arrayIndex)
        {
            lock (syncRoot)
            {
                ((ICollection<KeyValuePair<TKey, TValue>>)d).CopyTo(array, arrayIndex);
            }
        }

        public int Count
        {
            get
            {
                lock (syncRoot)
                {
                    return d.Count;
                }
            }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            lock (syncRoot)
            {
                try
                {
                    return ((ICollection<KeyValuePair<TKey,
                    TValue>>)d).Remove(item);
                }
                catch { return false; }
            }
        }

        #endregion

        #region IEnumerable<KeyValuePair<TKey,TValue>> Members

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            lock (syncRoot)
            {
                return ((ICollection<KeyValuePair<TKey, TValue>>)d).GetEnumerator();
            }
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator
        System.Collections.IEnumerable.GetEnumerator()
        {
            lock (syncRoot)
            {
                return ((System.Collections.IEnumerable)d).GetEnumerator();
            }
        }

        #endregion
    }

}
