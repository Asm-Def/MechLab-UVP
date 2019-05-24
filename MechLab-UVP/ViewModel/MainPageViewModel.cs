using System;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml.Controls;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MechLabLibrary.Models;
using MechLab_UVP;
using Microsoft.Toolkit.Uwp.UI.Controls;

namespace MechLabLibrary.ViewModel
{
    public class MainPageViewModel: ViewModelBase
    {

        private ObservableCollection<TabViewItem> _tabViewItems = new ObservableCollection<TabViewItem>();

        public ObservableCollection<TabViewItem> TabViewItems
        {
            get => _tabViewItems;
            set => Set(nameof(TabViewItems), ref _tabViewItems, value);
        }

        private TabViewItem _currentTab;

        public TabViewItem CurrentTab
        {
            get => _currentTab;
            set => Set(nameof(CurrentTab), ref _currentTab, value);
        }

        private int _untitledCount;

        public MainPageViewModel()
        {
            _untitledCount = 0;
            TabViewItems = new ObservableCollection<TabViewItem>
            {
                new TabViewItem()
                {
                    Header = "Home",
                    Icon = new SymbolIcon(Symbol.Home),
                    IsClosable = false,
                    Content = new HomePage()
                }
            };
            Messenger.Default.Register<object>(this, "OpenTab", (obj) =>
            {
                if (!(obj is MechLabData data)) return;
                if (TabViewItems.FirstOrDefault(e => ((e.Content as LabPage)?.ViewModel.Simulator.ID == data.LabID)) is TabViewItem
                    tab) CurrentTab = tab;
                else OpenTab.Execute(data.LabID);
            });
        }

        private RelayCommand<Guid> _openTab;

        public RelayCommand<Guid> OpenTab => _openTab ?? (_openTab = new RelayCommand<Guid>(id =>
        {
            var tab = new TabViewItem()
            {
                Header = id == Guid.Empty ? "Untitled " + ++_untitledCount : id.ToString(),
                Icon = new SymbolIcon(Symbol.Document),
                Content = new LabPage(id)
            };
            TabViewItems.Add(tab);
            CurrentTab = tab;
        }));

        private RelayCommand<TabViewItem> _closeTab;

        public RelayCommand<TabViewItem> CloseTab =>
            _closeTab ?? (_closeTab = new RelayCommand<TabViewItem>(tab =>
            {
                if (CurrentTab == tab) CurrentTab = TabViewItems[0];
                TabViewItems.Remove(tab);
            }));
    }
}