using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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
using MechLabLibrary.ViewModel;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace MechLab_UVP
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class LabPage : Page
    {
        public MechLabViewModel ViewModel;

        public LabPage(Guid id)
        {
            this.InitializeComponent();
            ViewModel = ViewModelLocator.Instance.MechLabViewModel(Guid.NewGuid().ToString());
            ViewModel.LoadMechLab(id);
        }

        public async Task<bool> CanClose()
        {
            if (ViewModel.IsSaved) return true;
            var noSaveDialog = new ContentDialog()
            {
                Title = "修改未保存",
                PrimaryButtonText = "保存",
                SecondaryButtonText = "不保存",
                CloseButtonText = "取消"
            };
            var result = await noSaveDialog.ShowAsync();
            if (result == ContentDialogResult.None) return false;
            if (result == ContentDialogResult.Primary) ViewModel.SaveCommand.Execute(null);
            SimpleIoc.Default.Unregister(ViewModel);
            return true;
        }

        private void EditingNameTextBox_OnLosingFocus(UIElement sender, LosingFocusEventArgs args)
        {
            if (!(sender is TextBox textBox)) return;
            if (textBox.Text.Length == 0) textBox.Text = "Untitled";
        }

    }
}
