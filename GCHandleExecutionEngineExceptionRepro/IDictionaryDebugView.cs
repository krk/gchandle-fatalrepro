using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace GCHandleExecutionEngineExceptionRepro
{
	internal sealed class IDictionaryDebugView<K, V>
	{
		private readonly IDictionary<K, V> _dictionary;

		public IDictionaryDebugView(IDictionary<K, V> dictionary)
		{
			if (dictionary == null)
				throw new ArgumentNullException(nameof(dictionary));

			_dictionary = dictionary;
		}

		[DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
		public KeyValuePair<K, V>[] Items
		{
			get
			{
				KeyValuePair<K, V>[] items = new KeyValuePair<K, V>[_dictionary.Count];
				_dictionary.CopyTo(items, 0);
				return items;
			}
		}
	}
}