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
using MechLabLibrary.ViewModel;
using Microsoft.Toolkit.Uwp.UI.Controls;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace MechLab_UVP
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPageViewModel ViewModel;

        public MainPage()
        {
            this.InitializeComponent();
            ViewModel = ViewModelLocator.Instance.MainPageViewModel;
        }

        private async void MainTabs_OnTabClosing(object sender, TabClosingEventArgs e)
        {
            if (!(e.Tab.Content is LabPage content)) return;
            e.Cancel = true;
            if (await content.CanClose()) ViewModel.CloseTab.Execute(e.Tab);
        }
    }
}
