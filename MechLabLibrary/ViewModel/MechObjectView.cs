using System;
using System.Collections.Generic;
using System.Text;
using MechLabLibrary.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Threading;

namespace MechLabLibrary.ViewModel
{
    public class MechObjectView : ViewModelBase
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
            set
            {
                _mechObject.X = (value * _parent.EyeShot) + _parent.X;
                if (_parent.EditingObject == this) OnPropertyChanged("ViewX");
            }
        }
        /// <summary>
        /// 在Canvas模型中的Y坐标
        /// </summary>
        public double ViewY
        {
            get { return (_mechObject.Y - _parent.Y) / _parent.EyeShot; }
            set { _mechObject.Y = (value * _parent.EyeShot) + _parent.Y;
                if (_parent.EditingObject == this) OnPropertyChanged("ViewY");
            }
        }

        public double VX
        {
            get => _mechObject.VX;
            set { _mechObject.VX = value; OnPropertyChanged("VX"); }
        }

        public double VY
        {
            get => _mechObject.VY;
            set { _mechObject.VY = value; OnPropertyChanged("VY"); }
        }

        public double M
        {
            get => _mechObject.M;
            set { _mechObject.M = value; OnPropertyChanged("M"); }
        }

        /// <summary>
        /// 指向它的物理模型
        /// </summary>
        protected MechObject _mechObject;
        public MechLabViewModel _parent;

        public void OnPropertyChanged(string propertyName = "")
        {
            // Raise the PropertyChanged event, passing the name of the property whose value has changed.
            DispatcherHelper.CheckBeginInvokeOnUI(() =>
            {
                RaisePropertyChanged(propertyName);
                //访问VM的属性
            });
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
        public void Refresh() { if(_parent.IsRunning) OnPropertyChanged(""); }
    }
}
