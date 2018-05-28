// Copyright (c) .NET Foundation and contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace System.CommandLine
{
    [DebuggerStepThrough]
    public abstract class AliasedSet<T> : IReadOnlyCollection<T>
        where T : class
    {
        private readonly HashSet<T> _items = new HashSet<T>();

        public T this[string alias] =>
            _items.SingleOrDefault(o => ContainsItemWithRawAlias(o, alias)) ??
            _items.SingleOrDefault(o => ContainsItemWithAlias(o, alias));

        public int Count => _items.Count;

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<T> GetEnumerator() => _items.GetEnumerator();

        protected abstract bool ContainsItemWithAlias(T item, string alias);

        protected abstract bool ContainsItemWithRawAlias(T item, string alias);

        internal void Add(T item)
        {
            var preexistingAlias = GetAliases(item)
                .FirstOrDefault(alias =>
                                    _items.Any(o =>
                                                    ContainsItemWithRawAlias(o, alias)));

            if (preexistingAlias != null)
            {
                throw new ArgumentException($"Alias '{preexistingAlias}' is already in use.");
            }

            _items.Add(item);
        }

        protected abstract IReadOnlyCollection<string> GetAliases(T item);

        public bool Contains(string alias) =>
            _items.Any(option => ContainsItemWithAlias(option, alias));
    }
}