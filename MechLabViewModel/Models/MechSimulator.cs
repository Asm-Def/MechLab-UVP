using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
namespace MechLabLibrary.Models
{
    public class MechSimulator
    {
        public List<MechObject> _objects = new List<MechObject>();

        /// <summary>
        /// 控制模拟的进行，当Running=true时持续对_objects进行模拟
        /// </summary>
        public bool Running = false;
        /// <summary>
        /// 开始进行模拟
        /// </summary>
        public void Start()
        {
            foreach(MechObject obj in _objects)
            {
                obj.timer = new Timer(new TimerCallback(obj.Simulate));
                obj.timer.Change(0, 10);
            }
        }
        /// <summary>
        /// 停止模拟
        /// </summary>
        public void Stop()
        {
            foreach(MechObject obj in _objects)
            {
                obj.timer.Dispose();
            }
        }
    }
}
