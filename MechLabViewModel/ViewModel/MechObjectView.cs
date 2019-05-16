using System;
using System.Collections.Generic;
using System.Text;
using MechLabLibrary.Models;

namespace MechLabLibrary.ViewModel
{
    public class MechObjectView
    {
        /// <summary>
        /// 在Canvas模型中的X坐标
        /// </summary>
        public double ViewX
        {
            get { return (_mechObject.X - _parent.X) / _parent.EyeShot; }
            set { _mechObject.X = ViewX * _parent.EyeShot + _parent.X; }
        }
        /// <summary>
        /// 在Canvas模型中的Y坐标
        /// </summary>
        public double ViewY
        {
            get { return (_mechObject.Y - _parent.Y) / _parent.EyeShot; }
            set { _mechObject.Y = ViewY * _parent.EyeShot + _parent.Y; }
        }
        /// <summary>
        /// 指向它的物理模型
        /// </summary>
        protected MechObject _mechObject;
        protected MechLabViewModel _parent;

        public MechObjectView(MechObject mechObject, MechLabViewModel mechLab)
        {
            _mechObject = mechObject;
            _parent = mechLab;
        }

        /// <summary>
        /// 用于更新_mechObject的实际坐标
        /// </summary>
        public void Refresh()
        {

        }

    }
}
