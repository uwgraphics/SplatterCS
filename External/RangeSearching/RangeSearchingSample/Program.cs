using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emil.RangeSearchingLibrary;

namespace Emil.RangeSearchingSample
{
	struct Point : IMultiDimensionComparable<Point>
	{
		public double X { get; set; }
		public double Y { get; set; }

		public Point(double x, double y)
			: this()
		{
			this.X = x;
			this.Y = y;
		}

		public int CompareTo(Point other, int dimension)
		{
			if(dimension == 0)
				return this.X.CompareTo(other.X);
			else if(dimension == 1)
				return this.Y.CompareTo(other.Y);
			else
				throw new ArgumentException("Invalid dimension.");
		}
	}

	class Program
	{
		static void Main(string[] args)
		{
			List<Point> points = new List<Point>()
			{
				new Point(3, 6),
				new Point(2, 5),
				new Point(5, 2),
				new Point(2, 1),
				new Point(3, 4),
				new Point(4, 4),
				new Point(2, 6),
				new Point(2, 2)
			};

			RangeSearchTree<Point> tree = new RangeSearchTree<Point>(2, points);

			List<Point> pointsInRange = tree.GetAllInRange(new Point(2, 3), new Point(5, 5));

			foreach(Point pointInRange in pointsInRange)
				Console.WriteLine(pointInRange.X + ", " + pointInRange.Y);

			Console.ReadKey();
		}
	}
}
