using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Emil.RangeSearchingLibrary
{
	/// <summary>
	/// The base class for multi-dimensional ranges.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public abstract class MultiDimensionalRange<T>
	{
		/// <summary>
		/// Compares the value to the range.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="dimension">Teh dimension in which to compare.</param>
		/// <returns>A negative integer if the value is smaller than the min bound of the range,
		/// zero if the value is inside of the range,
		/// and a positive integer if the value is greater than the max bound of the range.</returns>
		public abstract int CompareTo(T value, int dimension);
	}

	/// <summary>
	/// Represents a range determined by a min and a max value.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class MinMaxBoundedRange<T> : MultiDimensionalRange<T>
	{
		private MultiDimensionalComparer<T> m_comparer;
		
		/// <summary>
		/// The item that has the minimum value in each dimension.
		/// </summary>
		public T MinCorner { get; set; }
		
		/// <summary>
		/// The item that has the maximum value in each dimension.
		/// </summary>
		public T MaxCorner { get; set; }

		public MinMaxBoundedRange(MultiDimensionalComparer<T> comparer)
		{
			m_comparer = comparer;
		}

		public MinMaxBoundedRange(MultiDimensionalComparer<T> comparer, T minCorner, T maxCorner)
		{
			m_comparer = comparer;
			this.MinCorner = minCorner;
			this.MaxCorner = maxCorner;
		}

		public override int CompareTo(T value, int dimension)
		{
			bool lessThanMin = m_comparer.Compare(value, this.MinCorner, dimension) < 0;
			bool lessThanOrEqualToMax = m_comparer.Compare(value, this.MaxCorner, dimension) <= 0;

			if(lessThanMin)
				return -1;
			else if(lessThanOrEqualToMax)
				return 0;
			else
				return 1;
		}
	}
}