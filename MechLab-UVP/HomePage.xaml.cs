﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using MechLabLibrary.ViewModel;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace MechLab_UVP
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class HomePage : Page
    {
        public LabInfoViewModel ViewModel;
        public HomePage()
        {
            this.InitializeComponent();
            ViewModel = ViewModelLocator.Instance.LabInfoViewModel;
        }

        private void LabInfoGridView_OnItemClick(object sender, ItemClickEventArgs e)
        {
            Messenger.Default.Send<object>(e.ClickedItem,"OpenTab");
        }
    }
}
