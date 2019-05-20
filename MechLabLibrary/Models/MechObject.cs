using System;
using System.Collections.Generic;
using System.Text;
using MechLabLibrary.ViewModel;
using System.Threading;

namespace MechLabLibrary.Models
{
    public class Vector
    {
        public double X { get; set; }
        public double Y { get; set; }
        public Vector () { X = 0; Y = 0; }
        public Vector (double x, double y) { X = x; Y = y;}
        public static Vector operator * (Vector a, double b)
        {
            double ax = a.X, ay = a.Y;
            return new Vector (ax * b, ay * b);
        }
        public static Vector operator + (Vector a, Vector b)
        {
            double ax = a.X, ay = a.Y, bx = b.X, by = b.Y;
            return new Vector (ax + bx, ay + by);
        }
        public static Vector operator - (Vector a, Vector b)
        {
            double ax = a.X, ay = a.Y, bx = b.X, by = b.Y;
            return new Vector (ax - bx, ay - by);
        }
        public static double operator * (Vector a, Vector b)
        {
            double ax = a.X, ay = a.Y, bx = b.X, by = b.Y;
            return ax * bx + ay * by;
        }
        public static double operator ^ (Vector a, Vector b)
        {
            double ax = a.X, ay = a.Y, bx = b.X, by = b.Y;
            return ax * by - ay * bx;
        }
        public double Length { get { return Math.Sqrt(this * this); } }
        public override string ToString()
        {
            return X.ToString() + " " + Y.ToString();
        }
    }
    public class MechObject
    {
        /// <summary>
        /// 元件位置
        /// </summary>
        public Vector Location { get; set; } = new Vector();
        /// <summary>
        /// 元件速度
        /// </summary>
        public Vector Velocity { get; set; } = new Vector();

        /// <summary>
        /// 物体编号
        /// </summary>
        readonly public int ID;
        public double X { get { return Location.X; } set { Location.X = X; } }
        public double Y { get { return Location.Y; } set { Location.Y = Y; } }
        public double VX { get { return Velocity.X; } set { Velocity.X = X; } }
        public double VY { get { return Velocity.Y; } set { Velocity.Y = Y; } }
        public double M { get; set; }
        public Timer _timer { get; set; }
        DateTime _lastTime { get; set; }
        MechObjectView _mechObjectView;

        public MechSimulator _parent { get; set; }

        public MechObject(int ID) { M = 1; }
        public MechObject(int ID, double x, double y, double vx, double vy, double m, MechSimulator p = null)
        {
            Location = new Vector(x, y);
            Velocity = new Vector(vx, vy);
            _timer = new Timer(new TimerCallback(Simulate));
            M = m; _parent = p;
        }

        public void Init()
        {
            _lastTime = DateTime.Now;
        }

#if DEBUG
        readonly DateTime Begin = DateTime.Now; // for test

#endif

        public void Simulate(object state)
        {
            if (_parent == null) return;

            const double G = 6.67e-5; // G: km^3 / t / s^2
            double ax = 0, ay = 0;
            foreach(MechObject obj in _parent._objects)
            {
                if (this == obj) continue;
                double d = (obj.Location - Location).Length, Acc = 0.0;
                if (d < 1e-5)
                {

                }
                Acc = G * obj.M / d / d; // km / s^2
                ax += Acc * (obj.X - X) / d;
                ay += Acc * (obj.Y - Y) / d;
            }
            DateTime newt = DateTime.Now;
            double ts = (newt - _lastTime).TotalSeconds;
            Location = Location + Velocity * ts;
            Velocity = Velocity + new Vector (ax * ts, ay * ts);
            _lastTime = newt;
#if DEBUG
            Console.WriteLine(ID + " " + (newt-Begin).TotalMilliseconds + " "
                + Location.ToString());
#endif
        }
        
    }
}
