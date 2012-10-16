using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Emil.RangeSearchingLibrary
{
	public class RangeSearchTree<T>
	{
		private class Node
		{
			public Node LeftChild;
			public Node RightChild;
			public Node NextDimensionRoot;

			public T Min;
			public T Max;
			public T Value;
		}

		private MultiDimensionalComparer<T> m_comparer;
		private SpecificDimensionComparer<T> m_specificDimensionComparer;
		private Node m_root;

		/// <summary>
		/// The number of dimensions for searching.
		/// </summary>
		public int DimensionCount { get; private set; }

		/// <summary>
		/// Constructs a multi-dimensional range search tree.
		/// </summary>
		/// <param name="dimensionCount">The number of dimensions to search.</param>
		/// <param name="items">The items to search.</param>
		public RangeSearchTree(int dimensionCount, IEnumerable<T> items)
			: this(dimensionCount, items, MultiDimensionalComparer<T>.Default)
		{
		}

		/// <summary>
		/// Constructs a multi-dimensional range search tree.
		/// </summary>
		/// <param name="dimensionCount">The number of dimensions to search.</param>
		/// <param name="items">The items to search.</param>
		/// <param name="comparer">The comparer for the items.</param>
		public RangeSearchTree(int dimensionCount, IEnumerable<T> items, MultiDimensionalComparer<T> comparer)
		{
			m_comparer = comparer;
			this.DimensionCount = dimensionCount;
			m_specificDimensionComparer = new SpecificDimensionComparer<T>(m_comparer);
			m_root = BuildTree(items, 0);
		}

		/// <summary>
		/// Builds a search tree for the specified items starting with the specified dimension.
		/// This function should only be called for the root of a tree.
		/// </summary>
		/// <param name="unsortedItems">The items.</param>
		/// <param name="dimension">The dimension to start with.</param>
		/// <returns>The root of the tree built.</returns>
		private Node BuildTree(IEnumerable<T> unsortedItems, int dimension)
		{
			m_specificDimensionComparer.Dimension = dimension;

			List<T> items = new List<T>(unsortedItems);
			items.Sort(m_specificDimensionComparer);

			return BuildSubtree(items, dimension);
		}

		// TODO: Optimize this so that it only uses one array (instead of many lists) and reorders items inside of it.
		/// <summary>
		/// Builds a subtree for the specified items starting with the specified dimension.
		/// This function assumes that the items are already sorted in the specified dimension.
		/// </summary>
		/// <param name="items">The items sorted in the specified dimension.</param>
		/// <param name="dimension">The dimension to start with.</param>
		/// <returns>The node of teh subtree built.</returns>
		private Node BuildSubtree(List<T> sortedItems, int dimension)
		{
			Node node = new Node();
			node.Min = sortedItems[0];
			node.Max = sortedItems[sortedItems.Count - 1];

			if(sortedItems.Count > 1)
			{
				List<T> leftItems;
				List<T> rightItems;
				SplitList(sortedItems, out leftItems, out rightItems);

				node.LeftChild = BuildSubtree(leftItems, dimension);
				node.RightChild = BuildSubtree(rightItems, dimension);
			}
			else
			{
				node.Value = sortedItems[0];
			}

			if(dimension + 1 < this.DimensionCount)
				node.NextDimensionRoot = BuildTree(sortedItems, dimension + 1);

			return node;
		}

		// TODO: Optimize this by using arrays instead of lists.
		/// <summary>
		/// Splits a list in half.
		/// </summary>
		/// <param name="list">The list.</param>
		/// <param name="leftList">The first half.</param>
		/// <param name="rightList">The second half.</param>
		private static void SplitList(List<T> list, out List<T> leftList, out List<T> rightList)
		{
			int leftListCount = list.Count - list.Count / 2;

			leftList = new List<T>(leftListCount);
			rightList = new List<T>(list.Count - leftListCount);

			int index = 0;

			foreach(T item in list)
			{
				if(index < leftListCount)
					leftList.Add(item);
				else
					rightList.Add(item);

				index++;
			}
		}

		/// <summary>
		/// Gets all items in the specified range.
		/// </summary>
		/// <param name="min">The item that has the minimum value in each dimension.</param>
		/// <param name="max">The item that has the maximum value in each dimension.</param>
		/// <returns>The items in the specified range.</returns>
		public List<T> GetAllInRange(T min, T max)
		{
			MinMaxBoundedRange<T> range = new MinMaxBoundedRange<T>(m_comparer, min, max);
			return GetAllInRange(range);
		}

		/// <summary>
		/// Gets all items in the specified range.
		/// </summary>
		/// <param name="range">The range.</param>
		/// <returns>The items in the specified range.</returns>
		public List<T> GetAllInRange(MultiDimensionalRange<T> range)
		{
			List<T> matchingItems = new List<T>();
			CollectAllInRange(range, m_root, 0, matchingItems);
			return matchingItems;
		}

		/// <summary>
		/// Collects all values in the specific range of the subtree rooted at the specified node.
		/// </summary>
		/// <param name="range">The range.</param>
		/// <param name="node">The root of the subtree to search within.</param>
		/// <param name="dimension">The dimension of which the specified node belongs to.</param>
		/// <param name="matchingItems">The items which are found to be in the specified range are placed in this list.</param>
		private void CollectAllInRange(MultiDimensionalRange<T> range, Node node, int dimension, List<T> matchingItems)
		{
			if(node == null)
				return;

			int minRangeComparison = range.CompareTo(node.Min, dimension);
			int maxRangeComparison = range.CompareTo(node.Max, dimension);

			if(minRangeComparison == 0 && maxRangeComparison == 0)
			{
				// The node's range is fully contained within the search range.

				if(dimension == this.DimensionCount - 1)
				{
					// This is the last dimension so collect all of the values in this subtree.
					CollectAllValues(node, matchingItems);
				}
				else
				{
					// Continue searching in the next dimension.
					CollectAllInRange(range, node.NextDimensionRoot, dimension + 1, matchingItems);
				}
			}
			else if(minRangeComparison <= 0 || minRangeComparison >= 0)
			{
				// The node's range intersects the search range so continue searching all of the children.
				CollectAllInRange(range, node.LeftChild, dimension, matchingItems);
				CollectAllInRange(range, node.RightChild, dimension, matchingItems);
			}
		}

		/// <summary>
		/// Collects all of the values within the subtree rooted at the specified node.
		/// </summary>
		/// <param name="node">The root of the subtree.</param>
		/// <param name="matchingItems">The to which all of the items should be added to.</param>
		private void CollectAllValues(Node node, List<T> matchingItems)
		{
			if(node == null)
				return;

			CollectAllValues(node.LeftChild, matchingItems);
			CollectAllValues(node.RightChild, matchingItems);

			if(node.LeftChild == null && node.RightChild == null)
				matchingItems.Add(node.Value);
		}
	}
}
