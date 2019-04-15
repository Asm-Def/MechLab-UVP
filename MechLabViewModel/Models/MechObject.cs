using System;
using System.Collections.Generic;
using System.Text;
using MechLabLibrary.ViewModel;
using System.Threading;

namespace MechLabLibrary.Models
{
    public class MechObject
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double VX { get; set; }
        public double VY { get; set; }
        public double M { get; set; }
        public Timer timer;
        MechObjectView _mechObjectView;
        public MechSimulator _parent;

        public double dis2(MechObject obj)
        {
            return (X - obj.X) * (X - obj.X) + (Y - obj.Y) * (Y - obj.Y);
        }

        int cnt = 0;
        public void Simulate(object state)
        {
            //double Fx = 0, Fy = 0,G = 6.67e-11; // G: N * km^2 / t^2
            //foreach(MechObject obj in _parent._objects)
            //{
            //    if (this == obj) continue;
            //    double F = G * M * obj.M / dis2(obj);
            //    Fx += F;
            //}
            //cnt++;
            //Console.WriteLine((cnt++).ToString() + " " + X + " " + Y + " " + M + " " + Fx);

        }
        
    }
}
