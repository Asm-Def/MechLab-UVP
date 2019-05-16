using System;
using System.Collections.Generic;
using System.Text;
using MechLabLibrary.Models;

namespace MechLabLibrary.ViewModel
{
    public class MechPlanetView : MechObjectView
    {
        public MechPlanetView(MechPlanet mechPlanet, MechLabViewModel mechLab)
            : base(mechPlanet, mechLab) { }

        public double ViewR
        {
            get { return ((MechPlanet)_mechObject).R / _parent.EyeShot; }
            set { ((MechPlanet)_mechObject).R = ViewR * _parent.EyeShot; }
        }
    }
}
