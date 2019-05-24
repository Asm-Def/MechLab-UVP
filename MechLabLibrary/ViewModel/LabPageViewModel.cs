using System;
using System.Collections.ObjectModel;
using System.Threading;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MechLabLibrary.Models;

namespace MechLabLibrary.ViewModel
{
    public class LabPageViewModel:ViewModelBase
    {
        private MechLabServices _mechLabServices;
        private MechLabViewModel _mechLabViewModel;

        private Guid _labId;

        public Guid LabId
        {
            get => _labId;
            set => Set(nameof(LabId), ref _labId, value);
        }

        private MechSimulator _simulator;

        public MechSimulator Simulator
        {
            get => _simulator;
            set => Set(nameof(Simulator), ref _simulator, value);
        }

        private bool _isSaved;

        public bool IsSaved
        {
            get => _isSaved;
            set => Set(nameof(IsSaved), ref _isSaved, value);
        }
        



        public LabPageViewModel()
        {
            _mechLabServices = new MechLabServices();
            IsSaved = true;
            PropertyChanged += (s, e) =>
            {
                if (e.PropertyName != nameof(IsSaved)) IsSaved = false;
            };
        }

        public async void LoadMechLab(Guid id)
        {
            _mechLabViewModel = new MechLabViewModel(await _mechLabServices.GetSimulator(id));
        }

        public void NewMechLab()
        {
            _mechLabViewModel = new MechLabViewModel();
        }


        private RelayCommand _saveCommand;

        public RelayCommand SaveCommand => _saveCommand ?? (_saveCommand = new RelayCommand(() =>
        {
            IsSaved = true;
        }));
    }
}