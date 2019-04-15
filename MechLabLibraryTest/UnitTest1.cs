using NUnit.Framework;
using MechLabLibrary.Models;
using MechLabLibrary.ViewModel;
using System.Threading;
using System;

namespace Tests
{
    public class Obj : MechObject
    {
        public double tmp { get; set; }
    }
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            MechPlanet planet = new MechPlanet();
            MechObject obj = planet;
            Console.WriteLine(obj is Obj);
            Console.WriteLine(typeof(Obj).IsInstanceOfType(obj));
            Console.WriteLine(typeof(MechPlanet).IsInstanceOfType(obj));

            MechSimulator sim = new MechSimulator();
            MechPlanet A = new MechPlanet(), B = new MechPlanet();
            A.X = 0.3131; A.Y = 0.1313;  A.M = 1000; A._parent = sim;
            B.X = -0.3131; B.Y = -0.1313; B.M = 1000; B._parent = sim;
            sim._objects.Add(A);
            sim._objects.Add(B);
            sim.Start();
            Thread.Sleep(2000);
            sim.Stop();
        }
    }
}