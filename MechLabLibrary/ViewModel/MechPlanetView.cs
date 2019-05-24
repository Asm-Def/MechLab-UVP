using System;
using System.Collections.Generic;
using System.Text;
using MechLabLibrary.Models;

namespace MechLabLibrary.ViewModel
{
    public class MechPlanetView : MechObjectView
    {
        public MechPlanetView(int ID, MechPlanet mechPlanet, MechLabViewModel mechLab)
            : base(ID, mechPlanet, mechLab) { }

        public double ViewR
        {
            get { return ((MechPlanet)_mechObject).R / _parent.EyeShot; }
            set { ((MechPlanet)_mechObject).R = value * _parent.EyeShot; }
        }
    }
}
