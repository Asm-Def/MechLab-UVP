using NUnit.Framework;
using MechLabLibrary.Models;
using MechLabLibrary.ViewModel;
using System.Threading;
using System;
using System.IO;
using System.Text;

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
            //MechPlanet planet = new MechPlanet();
            //MechObject obj = planet;
            //Console.WriteLine(obj is Obj);
            //Console.WriteLine(typeof(Obj).IsInstanceOfType(obj));
            //Console.WriteLine(typeof(MechPlanet).IsInstanceOfType(obj));

            MechSimulator sim = new MechSimulator();
            StreamReader sr = null;
            try
            {
                sr = new StreamReader("D:\\93567\\source\\repos\\MechLab-UVP\\MechLabLibraryTest\\input.txt", Encoding.Default);
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
            String line;
            int c = 0;
            while((line = sr.ReadLine()) != null)
            {
                string[] sArr = line.Split(' ');
                double x = double.Parse(sArr[0]);
                double y = double.Parse(sArr[1]);
                double vx = double.Parse(sArr[2]);
                double vy = double.Parse(sArr[3]);
                double m = double.Parse(sArr[4]);
                double r = double.Parse(sArr[5]);
                MechPlanet A = new MechPlanet(x, y, vx, vy, m, r, sim) { ID = c++ };

                sim._objects.Add(A);
            }
//            MechPlanet A = new MechPlanet(0, 0, 0, 0, 1e8, 0, sim);
//            MechPlanet B = new MechPlanet(0, 10, 25.826343140289914, 10, 10, 0, sim);
//            MechPlanet C = new MechPlanet(0, -20, 18.261982367749674, 0, 10, 0, sim);
//#if DEBUG
//            A.ID = 0; B.ID = 1; C.ID = 2;
//#endif
//            sim._objects.Add(A);
//            sim._objects.Add(B);
//            sim._objects.Add(C);

            sim.Start();
            Thread.Sleep(10000);
            sim.Stop();
        }
    }
}