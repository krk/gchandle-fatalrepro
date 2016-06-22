using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GCHandleExecutionEngineExceptionRepro
{
	class Program
	{
		static void Main(string[] args)
		{
			ConcurrentWeakDictionaryTests_FromCorefx.TestAdd1();

			var sv = new ConcurrentSingleValueContainer<A>();
			var a2 = sv[new A { Value = 1 }];
			sv.Trim();

			var a1 = sv[new A { Value = 1 }];
			a1 = null;
			a2 = null;
			sv.Trim();

			GC.Collect(2);
			GC.WaitForPendingFinalizers();
			GC.Collect(2);
			GC.Collect(2);
			GC.WaitForPendingFinalizers();
			GC.Collect(2);
			GC.Collect(2);
			GC.WaitForPendingFinalizers();
			GC.Collect(2);
			GC.Collect(2);
			GC.WaitForPendingFinalizers();
			GC.Collect(2);
			sv.Trim();

			var a3 = sv[new A { Value = 1 }];
			sv.Trim();

			var eq1 = object.ReferenceEquals(a1, a2);
			var eq2 = object.ReferenceEquals(a1, a3);
			var eq3 = object.ReferenceEquals(a2, a3);

		}
	}

	public class A
	{
		public int Value { get; set; }

		public override int GetHashCode()
		{
			return Value;
		}

		public override bool Equals(object obj)
		{
			return Value.Equals(((A)obj).Value);
		}
	}

	/// <summary>
	/// Represents a container that provides a single instance of equal instances.
	/// </summary>
	/// <remarks>
	/// The container only keeps a weak reference to each unique element stored in the container. If an 
	/// element is not used elsewhere in the application domain, the element will eventually be removed 
	/// from this container.
	/// </remarks>
	/// <typeparam name="T">The type of the elements in the container.</typeparam>
	public class ConcurrentSingleValueContainer<T>
		where T : class
	{
		private readonly ConcurrentWeakDictionary<T, T> _dictionary;

		/// <summary>
		/// Initializes a new instance of ConcurrentSingleValueContainer`1{T} class.
		/// </summary>
		public ConcurrentSingleValueContainer()
			: this(EqualityComparer<T>.Default)
		{
		}

		/// <summary>
		/// Initializes a new instance of ConcurrentSingleValueContainer`1{T} class.
		/// </summary>
		/// <param name="comparer">An IEqualityComparer`1{T} that is used for equality tests and hash codes.</param>
		public ConcurrentSingleValueContainer(IEqualityComparer<T> comparer)
		{
			_dictionary = new ConcurrentWeakDictionary<T, T>(comparer ?? EqualityComparer<T>.Default);
		}

		/// <summary>
		/// Finalizes an instance of the <see cref="ConcurrentSingleValueContainer{T}"/> class.
		/// </summary>
		~ConcurrentSingleValueContainer()
		{
			Clear();
		}

		/// <summary>
		/// Gets the single instance of the provided value.
		/// </summary>
		/// <remarks>
		/// If no instance of the provided value exists in the container, it will be added and the value (or the create value key) will be returned.
		/// </remarks>
		/// <param name="value">Identifies the element to get.</param>
		/// <returns>The single instance of the element that is equal to <paramref name="value"/>.</returns>
		public T this[T value]
		{
			get
			{
				if (value == null)
				{
					return default(T);
				}

				T existingValue;

				if (_dictionary.TryGetValue(value, out existingValue))
				{
					return existingValue;
				}

				_dictionary[value] = value;

				return value;
			}
		}

		/// <summary>
		/// Removes all elements from this container.
		/// </summary>
		public void Clear()
		{
			_dictionary.Clear();
		}

		public void Trim()
		{
			_dictionary.Trim();
		}
	}
}
