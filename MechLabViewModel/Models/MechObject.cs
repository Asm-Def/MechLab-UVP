using System;
using System.Collections.Generic;
using System.Text;
using MechLabLibrary.ViewModel;

namespace MechLabLibrary.Models
{
    public class MechObject
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double VX { get; set; }
        public double VY { get; set; }
        public double M { get; set; }
        MechObjectView _mechObjectView;
    }
}
