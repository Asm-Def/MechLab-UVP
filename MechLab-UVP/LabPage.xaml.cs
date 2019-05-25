using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Imaging;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
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
        private Canvas _canvas;

        public LabPage(Guid id)
        {
            var isNew = false;
            if (id == Guid.Empty)
            {
                isNew = true;
                id = Guid.NewGuid();
            }
            ViewModel = ViewModelLocator.Instance.MechLabViewModel(id.ToString());
            ViewModel.LoadMechLab(id, isNew);
            this.InitializeComponent();
            Messenger.Default.Register<object>(this,id, (obj) =>
            {
                Debug.WriteLine("Received RenderCanvas");
                RenderCanvas();
            });
        }

        public async void RenderCanvas()
        {
            try
            {
                if (_canvas == null) return;
                RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap();
                Debug.WriteLine(_canvas.ActualWidth);
                Debug.WriteLine(_canvas.ActualHeight);
                if (_canvas.ActualWidth <= 0 || _canvas.ActualHeight <= 0) return;
                await renderTargetBitmap.RenderAsync(_canvas);
                byte[] bytes = (await renderTargetBitmap.GetPixelsAsync()).ToArray();
                Debug.WriteLine(bytes.Length);
                ViewModel.SaveLabAsync(bytes);
            }
            catch (Exception e)
            {
                Debug.WriteLine("RenderCanvas failed");
                Debug.WriteLine(e.Message);
            }
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
            Messenger.Default.Unregister(this);
            return true;
        }

        private void EditingNameTextBox_OnLosingFocus(UIElement sender, LosingFocusEventArgs args)
        {
            if (!(sender is TextBox textBox)) return;
            if (textBox.Text.Length == 0) textBox.Text = "Untitled";
        }

        private void Canvas_OnManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            if (e.IsInertial) e.Complete();
            if (ViewModel.IsMovingObject)
            {
                ViewModel.EditingObject.ViewX += e.Delta.Translation.X;
                ViewModel.EditingObject.ViewY += e.Delta.Translation.Y;
                ViewModel.EditingObject.UpdateXYR();
            }
            else
            {
                ViewModel.X -= e.Delta.Translation.X;
                ViewModel.Y -= e.Delta.Translation.Y;
                ViewModel.RefreshView();
            }

            e.Handled = true;
        }
        

        private void UIElement_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            if (ViewModel.IsRunning) return;
            var x = e.GetPosition(_canvas).X;
            var y = e.GetPosition(_canvas).Y;
            ViewModel.TappedObject(x, y);
            e.Handled = true;
        }

        private void TextBox_OnLosingFocus(UIElement sender, LosingFocusEventArgs args)
        {
            if (!(sender is TextBox textBox)) return;
            if (textBox.Text.Length > 0) return;
            if (textBox.Header != null && textBox.Header.Equals("M")) textBox.Text = "1";
            else textBox.Text = "0.0";
        }

        private void MainCanvas_OnLoaded(object sender, RoutedEventArgs e)
        {
            _canvas = sender as Canvas;
        }
    }
}
