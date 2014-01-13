using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace tap2_dikstrij
{
    public class Dikstrij
    {
		//change
        private List<Point> _pointList = new List<Point>();
        private Dictionary<Point, Point> _connectionList = new Dictionary<Point, Point>();
        private int _numOfConnections;
        //ILOSC_PUNKTOW = 5
        //MIN_GESTOSC = 1
        //MAX_GESTOSC = 3
        //MIN_D = 0.5
        //MAX_D = 0.6
		
        public void GenerateRandomPoints(int numberOfPoints)
        {
            Random rnd = new Random();
            for (int i = 0; i < numberOfPoints; i++)
                _pointList.Add(new Point() { X = rnd.Next(0, 1000), Y = rnd.Next(0, 1000) });
        }

        public void GenerateRandomConnectionsOrdered(int probability)
        {
            Random rnd = new Random();
            foreach (Point p1 in _pointList)
            {
                foreach (Point p2 in _pointList)
                {
                    if (p1.Equals(p2))
                        continue;
                    if (rnd.Next(0, 100) >= probability)
                    {
                        _connectionList.Add(p1, p2);
                        _numOfConnections++;
                    }
                }
            }
        }
		
		public double Hypotenuse (double a, double b)
		{
			return Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2));
		}

        public Rectangle GenerateSearchingField(Point start, Point end, double level)
        {
            //need 4 equations
			var extPoints = ExtendedPoints(start, end, (double)level);
			double length = Hypotenuse(p2.X-p1.X, p2.Y-p1.Y) * level;


            return new Rectangle();
        }
		
		public 
		
		
		

        public double[] GiveLineAB(Point p1, Point p2)
        {
            var AB = new double[2];
            //a
            AB[0] = (p2.Y - p1.Y) / (p2.X - p1.X);
            //b
            AB[1] = p1.Y - AB[0] * p1.X;

            return AB;
        }

        public double LineFunctionResult(double[] AB, double x)
        {
            //return y
            return AB[0] * x + AB[1];
        }

        public Point[] ExtendPoints(Point p1, Point p2, double increasing)
        {
            var extendedPoints = new Point[2];
            if (p1.X > p2.X)
            {
                int tmp;
                tmp = p2.X;
                p2.X = p1.X;
                p1.X = tmp;
            }
            Point p3 = new Point();
            Point p4 = new Point();

            p3.X = p2.X + (p2.X - p1.X) * (int)increasing;
            p4.X = p1.X - (p2.X - p1.X) * (int)increasing;
            p3.Y = (int)LineFunctionResult(GiveLineAB(p1, p2), (double)p3.X);
            p4.Y = (int)LineFunctionResult(GiveLineAB(p1, p2), (double)p4.X);
            extendedPoints[0] = p3;
            extendedPoints[1] = p4;
            return extendedPoints;
        }
		
		
		
    }
}
