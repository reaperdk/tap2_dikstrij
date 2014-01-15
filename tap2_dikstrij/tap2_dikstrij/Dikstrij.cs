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
        private Dictionary<Point, Dictionary<Point, double>> _connectionDictionary = new Dictionary<Point, Dictionary<Point, double>>();
        private List<Point> _queuePointList = new List<Point>();
        //ILOSC_PUNKTOW = 5
        //MIN_GESTOSC = 1
        //MAX_GESTOSC = 3
        //MIN_D = 0.5
        //MAX_D = 0.6
		
        public List<Point> Resolve (int numberOfPoints, int minDensity, int maxDensity)
        {
            GenerateRandomPoints(numberOfPoints, minDensity, maxDensity);
            Dijkstra();
            
            return new List<Point>();
        }

        public void Dijkstra ()
        {
            Random rnd = new Random();
            int startIndex = rnd.Next(0, _pointList.Count-1);
            int endIndex;
            while (true)
            {
                endIndex = rnd.Next(0, _pointList.Count - 1);
                if (startIndex != endIndex)
                    break;
            }
            var start = _pointList[startIndex];
            var end = _pointList[endIndex];

            var searchingField = GenerateSearchingField(start, end, 0.3);

            foreach (var p in _pointList)
            {
                if (IsInSearchingField(searchingField, p))
                    _queuePointList.Add(p);
            }


   //Dijkstra(G,w,s):
   //dla każdego wierzchołka v w V[G] wykonaj
   //   d[v] := nieskończoność
   //   poprzednik[v] := niezdefiniowane
   //d[s] := 0
   //Q := V
   //dopóki Q niepuste wykonaj
   //   u := Zdejmij_Min(Q)
   //   dla każdego wierzchołka v – sąsiada u wykonaj
   //      jeżeli d[v] > d[u] + w(u, v) to
   //         d[v] := d[u] + w(u, v)
   //         poprzednik[v] := u
   //         Dodaj(Q, v)

   //cout <<"Droga wynosi: "<<d[v];

        }

        public void GenerateRandomPoints(int numberOfPoints, int minDensity, int maxDensity)
        {
            Random rnd = new Random();
            for (int i = 0; i < numberOfPoints; i++)
                _pointList.Add(new Point() { X = rnd.Next(0, 1000), Y = rnd.Next(0, 1000) });
        }

        public void GenerateConnections(int minDensity, int maxDensity)
        {
            Random rnd = new Random();
            foreach(var p in _connectionDictionary.Keys)
            {
                int connections = rnd.Next(minDensity, maxDensity);
                for (int i = 0; i <= connections; i++)
                {
                    var dP = _pointList[rnd.Next(0, _pointList.Count-1)];
                    if (_connectionDictionary[p].ContainsKey(dP))
                        continue;
                    _connectionDictionary[p].Add(dP, Hypotenuse(dP.X-p.X, dP.Y-p.Y));
                }
            }
        }
		
		public double Hypotenuse (double a, double b)
		{
			return Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2));
		}
		
        public double[] GiveLineAB(Point p1, Point p2)
        {
            var AB = new double[2];
            //a
            AB[0] = (p2.Y - p1.Y) / (p2.X - p1.X);
            //b
            AB[1] = p1.Y - AB[0] * p1.X;

            return AB;
        }

        public Func<double, int> GiveLineFunction(double[] AB)
        {
            return x => (int)( AB[0] * x + AB[1]);
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

            var lineFunction = GiveLineFunction(GiveLineAB(p1, p2));

            p3.X = p2.X + (p2.X - p1.X) * (int)increasing;
            p4.X = p1.X - (p2.X - p1.X) * (int)increasing;
            p3.Y = lineFunction(p3.X);
            p4.Y = lineFunction(p4.X);

            extendedPoints[0] = p3;
            extendedPoints[1] = p4;
            return extendedPoints;
        }

        public List<Func<double, int>> giveHelpRectangle(Point p1, Point p2, double d)
        {
            Point tmpP1;
            Point tmpP2;

            if (p1.Y > p2.Y)
            {
                tmpP1 = p1;
                tmpP2 = p2;
            }
            else
            {
                tmpP1 = p2;
                tmpP2 = p1;
            };

            double[] AB = GiveLineAB(tmpP1, tmpP2);
            var lineFunction = GiveLineFunction(AB);
            double newA = -1.0 / AB[0];
            double newB1 = p2.Y - newA * p2.X;
            double newB2 = p1.Y - newA * p1.X;

            double[] AB1 = new double[2] { newA, newB1 };
            double[] AB2 = new double[2] { newA, newB2 };
            double[] AB3 = new double[2] { AB[0], AB[1] + d * Math.Sqrt(1 + AB[0] * AB[0]) };
            double[] AB4 = new double[2] { AB[0], AB[1] + d * Math.Sqrt(1 + AB[0] * AB[0]) };

            List<Func<double, int>> lineFunctions = new List<Func<double, int>>();
            lineFunctions.Add(GiveLineFunction(AB1));
            lineFunctions.Add(GiveLineFunction(AB2));
            lineFunctions.Add(GiveLineFunction(AB3));
            lineFunctions.Add(GiveLineFunction(AB4));
            return lineFunctions;
        }

        public List<Func<double, int>> GenerateSearchingField(Point start, Point end, double level)
        {
            var extPoints = ExtendPoints(start, end, (double)level);
            double length = Hypotenuse(end.X - start.X, end.Y - start.Y) * level;
            return giveHelpRectangle(extPoints[0], extPoints[1], length);
        }

        public bool IsInSearchingField (List<Func<double, int>> r, Point p)
        {
            return (p.Y < r[0](p.X) 
                && p.Y < r[2](p.X)
                && p.Y > r[1](p.X)
                && p.Y > r[3](p.X));
        }
    }
}
