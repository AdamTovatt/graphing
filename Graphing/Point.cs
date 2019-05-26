using System;

namespace Graphing
{
    /// <summary>
    /// Represents a point in a data set
    /// </summary>
    public class Point
    {
        /// <summary>
        /// X-value of the point
        /// </summary>
        public double X;
        /// <summary>
        /// Y-value of the point
        /// </summary>
        public double Y;

        /// <summary>
        /// X-value of the point rounded as an int
        /// </summary>
        public int IntX { get { return (int)Math.Round(X, 0, MidpointRounding.AwayFromZero); } }
        /// <summary>
        /// Y-value of the point rounded as an int
        /// </summary>
        public int IntY { get { return (int)Math.Round(Y, 0, MidpointRounding.AwayFromZero); } }

        /// <summary>
        /// Constructor setting the x and y values
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Writes the point as a string
        /// </summary>
        /// <returns>The point as a string</returns>
        public override string ToString()
        {
            return string.Format("({0}, {1})", X, Y);
        }
    }
}
