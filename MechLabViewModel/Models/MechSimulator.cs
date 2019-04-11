using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
namespace MechLabLibrary.Models
{
    public class MechSimulator
    {
        MechObject[] _objects;
        /// <summary>
        /// 控制模拟的进行，当Running=true时持续对_objects进行模拟
        /// </summary>
        bool Running = false;
        /// <summary>
        /// 应在程序开始时另开一个线程执行
        /// </summary>
        void Simulate()
        {
            Thread[] threads;
            while(true)
            {
                if (!Running) continue;

            }
        }
    }
}
