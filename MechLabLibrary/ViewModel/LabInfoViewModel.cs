using System.Collections.ObjectModel;
using System.Diagnostics;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MechLabLibrary.Models;

namespace MechLabLibrary.ViewModel
{
    public class LabInfoViewModel:ViewModelBase
    {
        private MechLabServices _mechLabServices;
        private ObservableCollection<MechLabData> _mechLabDataCollection;

        public ObservableCollection<MechLabData> MechLabDataCollection
        {
            get => _mechLabDataCollection;
            set => Set(nameof(MechLabDataCollection), ref _mechLabDataCollection, value);
        }

        public LabInfoViewModel()
        {
            _mechLabServices = new MechLabServices();
            GetLabAll();
        }

        private async void GetLabAll()
        {
            var labs = await _mechLabServices.GetMechLabs();
            Debug.WriteLine(labs.Count);
            MechLabDataCollection = new ObservableCollection<MechLabData>(labs);
        }

        
        
    }
}