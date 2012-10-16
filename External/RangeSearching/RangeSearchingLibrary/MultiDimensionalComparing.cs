using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Emil.RangeSearchingLibrary
{
	public delegate int MultiDimensionalComparison<T>(T x, T y, int dimension);

	/// <summary>
	/// The interface for items that are comparable in multiple dimensions.
	/// </summary>
	/// <typeparam name="T">The type of the item compared.</typeparam>
	public interface IMultiDimensionComparable<T>
	{
		int CompareTo(T other, int dimension);
	}

	/// <summary>
	/// The interface for comparers of multi-dimensional data.
	/// </summary>
	/// <typeparam name="T">The type of the item compared.</typeparam>
	public interface IMultiDimensionalComparer<T>
	{
		int Compare(T x, T y, int dimension);
	}

	/// <summary>
	/// A comparer for multi-dimensional data.
	/// </summary>
	/// <typeparam name="T">The type of the item compared.</typeparam>
	public abstract class MultiDimensionalComparer<T> : IMultiDimensionalComparer<T>
	{
		private static MultiDimensionalComparer<T> s_comparer;

		/// <summary>
		/// Gets the default comparer for the generic type.
		/// </summary>
		public static MultiDimensionalComparer<T> Default
		{
			get
			{
				if(s_comparer == null)
					s_comparer = CreateComparer();

				return s_comparer;
			}
		}

		/// <summary>
		/// Creates a multi-dimensional comperer from a multi-dimensional comparison delegate.
		/// </summary>
		/// <param name="comparison"></param>
		/// <returns></returns>
		public static MultiDimensionalComparer<T> CreateFromComparison(MultiDimensionalComparison<T> comparison)
		{
			return new MultiDimensionalDelegateComparer<T>(comparison);
		}

		/// <summary>
		/// Creates a new comparer for the generic type specified.
		/// </summary>
		/// <returns>The created comparer.</returns>
		private static MultiDimensionalComparer<T> CreateComparer()
		{
			if(typeof(IMultiDimensionComparable<T>).IsAssignableFrom(typeof(T)))
			{
				Type comparerType = typeof(MultiDimensionalGenericComparer<>).MakeGenericType(typeof(T));
				return (MultiDimensionalComparer<T>)comparerType.GetConstructor(new Type[] { }).Invoke(new object[] { });
			}
			else
				throw new InvalidOperationException("The type " + typeof(T).Name + " does not implement " + typeof(IMultiDimensionComparable<T>).Name + ".");
		}

		/// <summary>
		/// Compares two items in the specified dimension.
		/// </summary>
		/// <param name="x">The first item.</param>
		/// <param name="y">The second item.</param>
		/// <param name="dimension">The dimension in which to perform the comparison.</param>
		/// <returns>The result of the comparison.</returns>
		public abstract int Compare(T x, T y, int dimension);
	}

	internal class MultiDimensionalDelegateComparer<T> : MultiDimensionalComparer<T>
	{
		private MultiDimensionalComparison<T> m_comparison;

		/// <summary>
		/// Constructs a multi-dimensional comperer from a multi-dimensional comparison delegate.
		/// </summary>
		/// <param name="comparison"></param>
		public MultiDimensionalDelegateComparer(MultiDimensionalComparison<T> comparison)
		{
			m_comparison = comparison;
		}

		public override int Compare(T x, T y, int dimension)
		{
			return m_comparison(x, y, dimension);
		}
	}

	//[Serializable]
	/// <summary>
	/// A multi-dimensional comparer for items that implement the <see cref="T:RangeSearching.IMultiDimensionComparable`1" /> interface.
	/// </summary>
	/// <typeparam name="T">The type of the item compared.</typeparam>
	internal class MultiDimensionalGenericComparer<T> : MultiDimensionalComparer<T>
		where T : IMultiDimensionComparable<T>
	{
		public override int Compare(T item1, T item2, int dimension)
		{
			if(item1 != null)
			{
				if(item2 != null)
				{
					return item1.CompareTo(item2, dimension);
				}
				return 1;
			}
			if(item2 != null)
			{
				return -1;
			}
			return 0;
		}

		public override bool Equals(object obj)
		{
			return ((obj as MultiDimensionalGenericComparer<T>) != null);
		}

		public override int GetHashCode()
		{
			return base.GetType().Name.GetHashCode();
		}
	}


	/// <summary>
	/// Comparer which compares items based only on a specific dimension.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	internal class SpecificDimensionComparer<T> : Comparer<T>
	{
		private MultiDimensionalComparer<T> m_comparer;

		public SpecificDimensionComparer(MultiDimensionalComparer<T> comparer)
		{
			m_comparer = comparer;
			this.Dimension = 0;
		}

		/// <summary>
		/// The dimension in which to compare items.
		/// </summary>
		public int Dimension { get; set; }

		public override int Compare(T x, T y)
		{
			return m_comparer.Compare(x, y, this.Dimension);
		}
	}

}
