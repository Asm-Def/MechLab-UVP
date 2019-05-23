using System;
using System.Collections.Generic;
using System.Text;

namespace MechLabLibrary.Models
{
    public class MechPlanet : MechObject
    {
        public double R { get; set; }
        public MechPlanet(int ID, double x, double y, double vx, double vy, double m, double r = 0, MechSimulator p = null)
            : base(ID, x, y, vx, vy, m, p)
        {
            R = r;
        }
        /// <summary>
        /// 拷贝构造函数，创建一个与Simulator无关的MechPlanet拷贝
        /// </summary>
        /// <param name="planet"></param>
        public MechPlanet(MechPlanet planet)
            : this(planet.ID, planet.X, planet.Y, planet.VX, planet.VY, planet.M, planet.R) { }
    }
}
