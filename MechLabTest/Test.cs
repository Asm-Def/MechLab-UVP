using System;
using System.IO;
using MechLabLibrary.Models;
using MechLabLibrary.ViewModel;
using System.Threading;
using System.Text;
using System.Collections.Generic;

namespace MechLabTest
{
    public class Test
    {
        public static int Main()
        {
            MechLabViewModel viewModel = null ;
            try
            {
                viewModel = new MechLabViewModel();
                // TODO: 改为ViewModelLocator
            }
            catch (System.Exception exp) {
                Console.WriteLine(exp.ToString());
                Console.ReadLine();
            }
            StreamReader sr = null;
            try
            {
                sr = new StreamReader("input.txt", Encoding.Default);
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
            String line;
            while ((line = sr.ReadLine()) != null)
            {
                string[] sArr = line.Split(' ');
                double x = double.Parse(sArr[0]);
                double y = double.Parse(sArr[1]);
                double vx = double.Parse(sArr[2]);
                double vy = double.Parse(sArr[3]);
                double m = double.Parse(sArr[4]);
                double r = double.Parse(sArr[5]);

                viewModel.AddPlanetView(x, y, vx, vy, m, r);
            }

            {
                viewModel.StartRunningCommand.Execute(null);
                Console.ReadLine();
                viewModel.StopRunningCommand.Execute(null);
            }
            return 0;
        }
    }
}
