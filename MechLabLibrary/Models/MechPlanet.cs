using System;
using System.Collections.Generic;
using System.Text;

namespace MechLabLibrary.Models
{
    public class MechPlanet : MechObject
    {
        public double R { get; set; }
        public MechPlanet(double x, double y, double vx, double vy, double m, double r = 0, MechSimulator p = null)
            : base(x, y, vx, vy, m, p)
        {
            R = r;
        }
    }
}
