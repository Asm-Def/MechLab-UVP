using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
namespace MechLabLibrary.Models
{
    public class MechSimulator
    {
        /// <summary>
        /// 下一个新增的Object的ID
        /// </summary>
        private int _nextID = 0;

        public List<MechObject> _objects = new List<MechObject>();

        /// <summary>
        /// 控制模拟的进行，当Running=true时持续对_objects进行模拟
        /// </summary>
        public bool Running = false;

        /// <summary>
        /// 获取新的Object
        /// </summary>
        public MechObject AddObject(double x = 0,double y = 0, double vx = 0, double vy = 0, double m = 1)
        {
            MechObject result = new MechObject(_nextID++, x, y, vx, vy, m, this);
            _objects.Add(result);
            return result;
        }

        /// <summary>
        /// 在场景中创建一个mechObject的副本
        /// </summary>
        /// <param name="mechObject"></param>
        /// <returns></returns>
        public MechObject AddObject(MechObject mechObject)
        {
            MechObject result = null;
            if (mechObject.IsPlanet) result = new MechPlanet((MechPlanet)mechObject);
            else result = new MechObject(mechObject);
            result._parent = this;
            _objects.Add(result);
            return result;
        }

        public MechPlanet AddPlanet(double x = 0, double y = 0, double vx = 0, double vy = 0, double m = 1, double r = 0)
        {
            MechPlanet result = new MechPlanet(_nextID++, x, y, vx, vy, m, r, this);
            _objects.Add(result);
            return result;
        }

        /// <summary>
        /// 删除符合给定ID的物体
        /// </summary>
        /// <param name="ID"></param>
        public void DeleteObject(int ID)
        {
            _objects.RemoveAll((o) => o.ID == ID);
        }

        /// <summary>
        /// 开始进行模拟
        /// </summary>
        public void Start()
        {
            foreach(MechObject obj in _objects)
            {
                obj.Init();
                obj._timer.Change(0, 10);
            }
            Running = true;
        }

        /// <summary>
        /// 停止模拟
        /// </summary>
        public void Stop()
        {
            Running = false;
            foreach (MechObject obj in _objects)
            {
                obj._timer.Change(0, Timeout.Infinite);
            }
        }

        /// <summary>
        /// 场景编号
        /// </summary>
        public readonly Guid ID;
        public MechSimulator() { ID = Guid.NewGuid(); }
        public MechSimulator(Guid ID) { this.ID = ID; }
        public MechSimulator(Guid ID, List<MechObject> objects) : this(ID)
        {
            foreach (var obj in objects) AddObject(obj);
        }
    }
}
