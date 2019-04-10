using System;

namespace MechLabLibrary.ViewModel
{
    public class MechLabViewModel
    {
        /// <summary>
        /// 缩放比例
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
    }
}
