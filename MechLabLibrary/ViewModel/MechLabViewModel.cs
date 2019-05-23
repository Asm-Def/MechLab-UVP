using System;
using MechLabLibrary.Models;
using System.Threading;
using System.Collections.Generic;

/// <summary>
/// 场景的ViewModel
/// </summary>
namespace MechLabLibrary.ViewModel
{
    public class MechLabViewModel
    {
        public String Name { get; set; }

        /// <summary>
        /// 缩放比例,(vx, vy) = (x - X, y - Y) / EyeShot
        /// </summary>
        public double EyeShot { get; set; }

        /// <summary>
        /// 视野左上角的实际X坐标
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// 视野左上角的实际Y坐标
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// 场景编号
        /// </summary>
        public Guid ID { get { return Simulator.ID; } }

        private MechSimulator Simulator { get; set; }

        private List<MechObjectView> _objectviews = new List<MechObjectView>();

        private Timer _timer;

        /// <summary>
        /// 获取新的ObjectView
        /// </summary>
        /// <returns></returns>
        public MechObjectView AddObjectView(double x = 0, double y = 0, double vx = 0, double vy = 0, double m = 1)
        {
            MechObject mechObject = Simulator.AddObject(x, y, vx, vy, m);
            MechObjectView result = new MechObjectView(_objectviews.Count, mechObject, this);
            _objectviews.Add(result);
            return result;
        }

        /// <summary>
        /// 获取新的PlanetView
        /// </summary>
        /// <returns></returns>
        public MechPlanetView AddPlanetView(double x = 0, double y = 0, double vx = 0, double vy = 0, double m = 1, double r = 0)
        {
            MechPlanet mechPlanet = Simulator.AddPlanet(x, y, vx, vy, m, r);
            MechPlanetView result = new MechPlanetView(_objectviews.Count, mechPlanet, this);
            _objectviews.Add(result);
            return result;
        }

        /// <summary>
        /// 从现有的MechSimulator创建MechLabView
        /// </summary>
        /// <param name="sim"></param>
        /// <param name="eyeshot"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public MechLabViewModel(MechSimulator sim, double eyeshot = 1, double x = 0, double y = 0)
        {
            _timer = new Timer((s) => { foreach (var obj in _objectviews) obj.Refresh(); }, null, 0, 1000 / 40); // 设定刷新频率

            Simulator = sim; EyeShot = eyeshot; X = x; Y = y; Name = "New Lab";
            foreach(MechObject mechObject in sim._objects)
            {
                if (mechObject.Type == "Planet")
                {
                    MechPlanetView planetView = new MechPlanetView(_objectviews.Count, (MechPlanet) mechObject, this);
                    _objectviews.Add(planetView);
                }
                else
                {
                    MechObjectView objectView = new MechObjectView(_objectviews.Count, mechObject, this);
                    _objectviews.Add(objectView);
                }
            }
        }

        public MechLabViewModel(double eyeshot = 1, double x = 0, double y = 0)
            : this(new MechSimulator(), eyeshot, x, y) { }
    }
}
