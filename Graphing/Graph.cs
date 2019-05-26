using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphing
{
    /// <summary>
    /// Represents a graph with data sets
    /// </summary>
    public class Graph
    {
        /// <summary>
        /// The smallest x-value to include in the drawn area
        /// </summary>
        public double MinX { get { return _minx; } set { if(!double.IsInfinity(value)) _minx = value; redraw = true; } }
        private double _minx;
        /// <summary>
        /// The largest x-value to include in the drawn area
        /// </summary>
        public double MaxX { get { return _maxx; } set { if (!double.IsInfinity(value)) _maxx = value; redraw = true; } }
        private double _maxx;
        /// <summary>
        /// The smallest y-value to include in the drawn area
        /// </summary>
        public double MinY { get { return _miny; } set { if (!double.IsInfinity(value)) _miny = value; redraw = true; } }
        private double _miny;
        /// <summary>
        /// The largest y-value to include in the drawn area
        /// </summary>
        public double MaxY { get { return _maxy; } set { if (!double.IsInfinity(value)) _maxy = value; redraw = true; } }
        private double _maxy;

        /// <summary>
        /// The width of the bitmap on which the graph will be drawn
        /// </summary>
        public int BitmapWidth
        {
            get { return _bitmapWidth; }
            set { _bitmapWidth = value; redraw = true; }
        }
        private int _bitmapWidth;

        /// <summary>
        /// The height of the bitmap on which the graph will be drawn
        /// </summary>
        public int BitmapHeight
        {
            get { return _bitmapHeight; }
            set { _bitmapHeight = value; redraw = true; }
        }
        private int _bitmapHeight;

        /// <summary>
        /// The data sets in the graph
        /// </summary>
        public IReadOnlyCollection<DataSet> DataSets { get { return dataSets.AsReadOnly(); } }
        /// <summary>
        /// The graph drawn as a bitmap (used to display the graph and will yield the same result as .GetBitmap())
        /// </summary>
        public Bitmap Bitmap { get { return GetBitmap(); } }

        private DirectBitmap directBitmap;
        private List<DataSet> dataSets;

        private bool redraw = false;

        /// <summary>
        /// Constructor which will set the size of both the bitmap and the graph according to the specified values
        /// </summary>
        /// <param name="width">The width of the bitmap and graph</param>
        /// <param name="height">The height of the bitmap and graph</param>
        public Graph(int width, int height)
        {
            MaxX = width;
            MaxY = height;
            BitmapWidth = width;
            BitmapHeight = height;
            dataSets = new List<DataSet>();
        }

        /// <summary>
        /// Constructor which will set the bitmap according to the specified width and height values but set the size of the graph according to the other values
        /// </summary>
        /// <param name="width">Width of the bitmap</param>
        /// <param name="height">Height of the bitmap</param>
        /// <param name="minx">Smallest x-value to be drawn in the graph</param>
        /// <param name="maxx">Largest x-value to be drawn in the graph</param>
        /// <param name="miny">Smallest y-value to be drawn in the graph</param>
        /// <param name="maxy">Largest x-value to be drawn in the graph</param>
        public Graph(int width, int height, double minx, double maxx, double miny, double maxy)
        {
            BitmapWidth = width;
            BitmapHeight = height;
            dataSets = new List<DataSet>();
            MinX = minx;
            MaxX = maxx;
            MinY = miny;
            MaxY = maxy;
        }

        /// <summary>
        /// Adds a data set to the graph
        /// </summary>
        /// <param name="dataSet"></param>
        public void AddDataset(DataSet dataSet)
        {
            dataSets.Add(dataSet);
        }

        /// <summary>
        /// Removes a data set from the graph
        /// </summary>
        /// <param name="dataSet"></param>
        public void RemoveDataset(DataSet dataSet)
        {
            dataSets = dataSets.Where(x => x != dataSet).ToList();
        }

        /// <summary>
        /// Will automatically set the area to be drawn based on a data set
        /// </summary>
        /// <param name="dataSet"></param>
        public void Fit(DataSet dataSet)
        {
            MaxY = dataSet.Points.OrderByDescending(x => x.Y).Take(10).Average(x => x.Y) + 10;
        }

        /// <summary>
        /// Will draw the graph as a bitmap (will yield the same result as using the .Bitmap-property)
        /// </summary>
        /// <returns>The graph as a bitmap</returns>
        public Bitmap GetBitmap()
        {
            if (redraw)
            {
                //städa och skapa en ny från grunden
                directBitmap = new DirectBitmap(_bitmapWidth, _bitmapHeight);
            }

            //rita ut dataset som har förändrats
            foreach (DataSet set in dataSets)
            {
                set.Draw(this, directBitmap);
            }

            return directBitmap.Bitmap;
        }
    }
}