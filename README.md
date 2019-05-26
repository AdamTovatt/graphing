# Graphing
Simple C# graphing library for visualizing data

## Usage
Clone the repository and build the project. Add a reference to the built Graphing.dll file in the project you want to use it in.
Also, add a reference to System.Drawing if it doesn't already exist.

FYI the dataset used in this example is a simple tab-separated list of 10 values. It looks like this:
```text
1	12.52
2	12.7
3	15.62
4	14.34
5	18
6	18.62
7	16.48
8	20.62
9	22.15
10	21.86
```

The result will be as follows (the background is transparent):

[![INSERT YOUR GRAPHIC HERE](https://i.imgur.com/29Dyd7g.png)]()

Add these three using directives
```C#
using System.Drawing;
using Point = Graphing.Point;
using System.Globalization;
```

Create a new Graph object
```C#
Graph graph = new Graph(400, 200, 0, 10, 0, 25);
```
This one will produce an 400x200 image.
The smallest x-value that will be included is 0 and the largest is 10.
The smallest y-value that will be included is 0 and the largest is 25.
These min and max values are set based on the values in the data.

Create a new DataSet object:
```C#
DataSet dataSet = new DataSet(Color.Blue) //the default color of the lines, points and fill will now be Color.Blue
{
    LineThickness = 2,
    PointSize = 5,
    PointColor = Color.DarkBlue
};
```
Now fill the data set. This code might be different in different applications, this is just an example:
```C#
foreach (string line in File.ReadAllLines("dataset.txt"))
{
    string[] points = line.Split('\t');

    double x = double.Parse(points[0], CultureInfo.InvariantCulture);
    double y = double.Parse(points[1], CultureInfo.InvariantCulture);

    dataSet.AddPoint(new Point(x, y));
}
```
Add the created data set to the graph:
```C#
graph.AddDataset(dataSet);
```

That's it, the Bitmap-property of the graph will now contain an image of the graph.
One way of saving and viewing it would be like this:
```C#
graph.Bitmap.Save("graph.bmp");
```
