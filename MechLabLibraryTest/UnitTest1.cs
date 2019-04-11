using NUnit.Framework;
using MechLabLibrary.Models;
using MechLabLibrary.ViewModel;
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
        }
    }
}