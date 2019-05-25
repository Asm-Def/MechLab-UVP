using System.Collections.ObjectModel;
using System.Diagnostics;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
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
            Messenger.Default.Register<string>(this,"UpdateHome", (s) =>
            {
                Debug.WriteLine("Received Message UpdateHome");
                GetLabAll();
            });
        }

        private void GetLabAll()
        {
            var labs = _mechLabServices.GetMechLabs().Result;
            labs.ForEach((e) =>
            {
                Debug.WriteLine(e.LabID);
                Debug.WriteLine(e.Name);
                Debug.WriteLine(e.ModifiedTime);
            });
            Debug.WriteLine(labs.Count);
            MechLabDataCollection = new ObservableCollection<MechLabData>(labs);
        }

        
        
    }
}