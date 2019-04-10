using System;
using System.Collections.Generic;
using System.Text;
using MechLabLibrary.Models;

namespace MechLabLibrary.ViewModel
{
    class MechObjectView
    {
        /// <summary>
        /// 在Canvas模型中的X坐标
        /// </summary>
        public double ViewX { get; set; }
        /// <summary>
        /// 在Canvas模型中的Y坐标
        /// </summary>
        public double ViewY { get; set; }
        /// <summary>
        /// 指向它的物理模型
        /// </summary>
        private MechObject _mechObject;

        /// <summary>
        /// 用于更新_mechObject的实际坐标
        /// </summary>
        public void Refresh()
        {

        }

    }
}
