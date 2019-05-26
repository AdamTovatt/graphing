using System;
using System.Collections.Generic;
using System.Drawing;

namespace Graphing
{
    /// <summary>
    /// Represents a set of data
    /// </summary>
    public class DataSet
    {
        /// <summary>
        /// The points of the data set
        /// </summary>
        public IReadOnlyCollection<Point> Points { get { return _points.AsReadOnly(); } }
        private List<Point> _points = new List<Point>();

        /// <summary>
        /// The color of the points
        /// </summary>
        public Color PointColor { get; set; }
        /// <summary>
        /// The color of the lines
        /// </summary>
        public Color LineColor { get; set; }
        /// <summary>
        /// The color of the fill
        /// </summary>
        public Color FillColor { get; set; }
        /// <summary>
        /// Size of the points in pixels
        /// </summary>
        public double PointSize { get; set; }
        /// <summary>
        /// The thickness of the lines in pixels
        /// </summary>
        public double LineThickness { get; set; }
        /// <summary>
        /// If the graph should be "filled" or not, setting this to true will yield results in which the area under the graph is colored
        /// </summary>
        public bool Fill { get; set; }
        /// <summary>
        /// If the dataset should allow adding values that are non numbers
        /// </summary>
        public bool AllowNaNValues { get; set; } = false;
        /// <summary>
        /// If the dataset should allow adding values that are infinite
        /// </summary>
        public bool AllowInfinityValues { get; set; } = false;

        /// <summary>
        /// Constructor for the data set. All colors will be set to the value of graphColor at first but can be individually changed later
        /// </summary>
        /// <param name="graphColor"></param>
        public DataSet(Color graphColor)
        {
            _points = new List<Point>();
            PointColor = graphColor;
            LineColor = graphColor;
            FillColor = graphColor;
            PointSize = 2;
            LineThickness = 1;
        }

        /// <summary>
        /// Adds a point to the data set
        /// </summary>
        /// <param name="point">The point to add</param>
        /// <returns>If the point was added successfully</returns>
        public bool AddPoint(Point point)
        {
            if (!AllowNaNValues && (double.IsNaN(point.Y) || double.IsNaN(point.X))) return false;
            if (!AllowInfinityValues && (double.IsInfinity(point.Y) || double.IsInfinity(point.X))) return false;

            for (int i = _points.Count; i > 0; i--)
            {
                if (_points[i - 1].X < point.X)
                {
                    _points.Insert(i, point); //backar igenom listan och sätter in om den hittar ett x-värde som är mindre än det aktuella
                    return true;
                }
            }
            _points.Insert(0, point); //inget x-värde har hittats så det aktuella är det minsta, sätt det först
            return true;
        }

        /// <summary>
        /// Will draw the data set on a bitmap using the specified graph for size
        /// </summary>
        /// <param name="graph">The graph the data set should be drawn with respect to</param>
        /// <param name="bitmap">The bitmap to draw the dataset on</param>
        internal void Draw(Graph graph, DirectBitmap bitmap)
        {
            double ySize = graph.MaxY - graph.MinY;
            double xScale = (graph.MaxX - graph.MinX) / graph.BitmapWidth;
            double yScale =  ySize / graph.BitmapHeight;

            //punkter
            foreach (Point point in Points)
            {
                for (int x = 0; x < PointSize; x++)
                {
                    for (int y = 0; y < PointSize; y++)
                    {
                        bitmap.SetPixel(
                            (int)Math.Round(point.X / xScale + (x - PointSize / 2), 0, MidpointRounding.AwayFromZero), 
                            (int)Math.Round(ySize / yScale - (point.Y / yScale + (y - PointSize / 2)), 0, MidpointRounding.AwayFromZero), 
                            PointColor);
                    }
                }
            }

            //linjer
            for (int i = 0; i < Points.Count - 1; i++)
            {
                Point p1 = _points[i];
                Point p2 = _points[i + 1]; //vi kan anta att p2.x > p1.x

                double k = (p2.Y - p1.Y) / ((p2.X - p1.X) / xScale);
                int xRange = p2.IntX - p1.IntX;

                for (int x = 0; x < (int)Math.Round(xRange / xScale, 0, MidpointRounding.AwayFromZero); x++)
                {
                    double yValue = ySize / yScale - ((x * k) + p1.Y) / yScale;
                    double xValue = p1.IntX / xScale + x;
                    for (int tx = 0; tx < LineThickness; tx++)
                    {
                        for (int ty = 0; ty < LineThickness; ty++)
                        {
                            bitmap.SetPixel(
                                (int)Math.Round(xValue + (tx - (LineThickness / 2) + 1), 0, MidpointRounding.AwayFromZero), 
                                (int)Math.Round(yValue + (ty - (LineThickness / 2) + 1), 0, MidpointRounding.AwayFromZero), 
                                LineColor);
                        }
                    }

                    if (Fill)
                    {
                        for (int _x = 0; _x < 2; _x++)
                        {
                            for (int y = 0; y < bitmap.Height - yValue; y++)
                            {
                                bitmap.SetPixel((int)Math.Round(xValue + _x, 0, MidpointRounding.AwayFromZero), bitmap.Height - y, FillColor);
                            }
                        }
                    }
                }
            }
        }
    }
}