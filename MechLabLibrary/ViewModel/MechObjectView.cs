using System;
using System.Collections.Generic;
using System.Text;
using MechLabLibrary.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MechLabLibrary.ViewModel
{
    public class MechObjectView : INotifyPropertyChanged
    {

        /// <summary>
        /// 物体编号
        /// </summary>
        readonly public int ID;

        /// <summary>
        /// 在Canvas模型中的X坐标
        /// </summary>
        public double ViewX
        {
            get { return (_mechObject.X - _parent.X) / _parent.EyeShot; }
            set { _mechObject.X = (ViewX * _parent.EyeShot) + _parent.X; }
        }
        /// <summary>
        /// 在Canvas模型中的Y坐标
        /// </summary>
        public double ViewY
        {
            get { return (_mechObject.Y - _parent.Y) / _parent.EyeShot; }
            set { _mechObject.Y = (ViewY * _parent.EyeShot) + _parent.Y; }
        }

        /// <summary>
        /// 指向它的物理模型
        /// </summary>
        protected MechObject _mechObject;
        protected MechLabViewModel _parent;

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            // Raise the PropertyChanged event, passing the name of the property whose value has changed.
            this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public MechObjectView(int ID, MechObject mechObject, MechLabViewModel mechLab)
        {
            this.ID = ID;
            _mechObject = mechObject;
            _parent = mechLab;
        }

        /// <summary>
        /// 用于更新_mechObject的显示坐标
        /// </summary>
        // TODO: 若不成功，则修改为手动计算ViewX ViewY属性值
        public void Refresh() => OnPropertyChanged("");
    }
}
