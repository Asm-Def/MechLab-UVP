using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using MechLabLibrary.Models;

namespace MechLabLibrary.ViewModel
{
    public class MechLabViewModel : ViewModelBase
    {
        /// <summary>
        /// 下一个新增的Object的ID
        /// </summary>
        private int _nextID = 0;

        private MechLabServices _mechLabServices;

        private MechLabData _labData;

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

        private string _name;

        public string Name
        {
            get => _name;
            set => Set(nameof(Name), ref _name, value);
        }

        private double _eyeShot;

        public double EyeShot
        {
            get => _eyeShot;
            set { Set(nameof(EyeShot), ref _eyeShot, value); RefreshView();}
        }

        private double _x;

        public double X
        {
            get => _x;
            set => Set(nameof(X), ref _x, value);
        }

        private double _y;

        public double Y
        {
            get => _y;
            set => Set(nameof(Y), ref _y, value);
        }

        private ObservableCollection<MechObjectView> _objectViewCollection;

        public ObservableCollection<MechObjectView> ObjectViewCollection
        {
            get => _objectViewCollection;
            set => Set(nameof(ObjectViewCollection), ref _objectViewCollection, value);
        }

        private Timer _timer;

        private bool _isEditingName;

        public bool IsEditingName
        {
            get => _isEditingName;
            set => Set(nameof(IsEditingName), ref _isEditingName, value);
        }

        private bool _isEditingObject;

        public bool IsEditingObject
        {
            get => _isEditingObject;
            set => Set(nameof(IsEditingObject), ref _isEditingObject, value);
        }

        private bool _isMovingObject;

        public bool IsMovingObject
        {
            get => _isMovingObject;
            set => Set(nameof(IsMovingObject), ref _isMovingObject, value);
        }

        private MechPlanetView _editingObject;

        public MechPlanetView EditingObject
        {
            get => _editingObject;
            set => Set(nameof(EditingObject), ref _editingObject, value);
        }

        private bool _isRunning;

        public bool IsRunning
        {
            get => _isRunning;
            set => Set(nameof(IsRunning), ref _isRunning, value);
        }




        public MechLabViewModel()
        {
            _mechLabServices = new MechLabServices();
            ObjectViewCollection = new ObservableCollection<MechObjectView>();
            IsRunning = false;
            IsEditingName = false;
            IsEditingObject = false;
            IsMovingObject = false;
            PropertyChanged += (s, e) =>
            {
                if (e.PropertyName != nameof(IsSaved)) IsSaved = false;
            };
        }

        public async void LoadMechLab(Guid id)
        {
            _timer = new Timer((s) =>
                {
                    foreach (var obj in ObjectViewCollection) obj.Refresh();
                }, null, 0, 1000 / 40); // 设定刷新频率

            Simulator = id == Guid.Empty ? new MechSimulator() : await _mechLabServices.GetSimulator(id);
            _labData = id == Guid.Empty ? new MechLabData() : _mechLabServices.GetLabData(id);
            Name = id == Guid.Empty ? "Untitled" : _labData.Name;
            EyeShot = 1;
            X = 0;
            Y = 0;
            foreach (MechObject mechObject in Simulator._objects)
            {
                if (mechObject.Type == "Planet")
                {
                    MechPlanetView planetView =
                        new MechPlanetView(_nextID++, (MechPlanet) mechObject, this);
                    ObjectViewCollection.Add(planetView);
                }
                else
                {
                    MechObjectView objectView = new MechObjectView(_nextID++, mechObject, this);
                    ObjectViewCollection.Add(objectView);
                }
            }
            IsSaved = true;
        }


        /// <summary>
        /// 获取新的ObjectView
        /// </summary>
        /// <returns></returns>
        public MechObjectView AddObjectView(double x = 0, double y = 0, double vx = 0, double vy = 0, double m = 1)
        {
            MechObject mechObject = Simulator.AddObject(x, y, vx, vy, m);
            MechObjectView result = new MechObjectView(_nextID++, mechObject, this);
            ObjectViewCollection.Add(result);
            return result;
        }

        /// <summary>
        /// 获取新的PlanetView
        /// </summary>
        /// <returns></returns>
        public MechPlanetView AddPlanetView(double x = 0, double y = 0, double vx = 0, double vy = 0, double m = 1,
            double r = 10)
        {
            MechPlanet mechPlanet = Simulator.AddPlanet(x, y, vx, vy, m, r);
            MechPlanetView result = new MechPlanetView(_nextID++, mechPlanet, this);
            ObjectViewCollection.Add(result);
            return result;
        }

        public void TappedObject(double x, double y)
        {
            Debug.WriteLine(x);
            Debug.WriteLine(y);
            foreach (var mechObjectView in ObjectViewCollection)
            {
                var r= ((MechPlanetView) mechObjectView).ViewR;
                var xx = (mechObjectView.ViewX - x);
                var yy = (mechObjectView.ViewY - y);
                if (xx * xx + yy * yy <= r*r)
                {
                    Debug.WriteLine(mechObjectView.ID);
                    EditingObject = (MechPlanetView)mechObjectView;
                    IsEditingObject = true;
                    IsMovingObject = true;
                    break;
                }

            }
        }

        public void RefreshView()
        {
            foreach (var mechObjectView in ObjectViewCollection) mechObjectView.UpdateXYR();
        }



        private RelayCommand _toggleEditingName;

        public RelayCommand ToggleEditingName => _toggleEditingName ?? (_toggleEditingName =
                                                     new RelayCommand(() =>
                                                     {
                                                         IsEditingName = !IsEditingName;
                                                     }));

        private RelayCommand _saveCommand;

        public RelayCommand SaveCommand => _saveCommand ?? (_saveCommand = new RelayCommand(() =>
        {
            Debug.WriteLine("saved");
            IsEditingName = false;
            IsEditingObject = false;
            IsMovingObject = false;
            EditingObject = null;
            _labData.Name = Name;
            _labData.LabID = Simulator.ID;
            _labData.ModifiedTime=DateTime.Now;
            _mechLabServices.SaveMechLab(_labData,Simulator._objects);
            Debug.WriteLine(_labData.Name);
            Debug.WriteLine(_labData.ModifiedTime);
            Messenger.Default.Send<string>("", "UpdateHome");
            IsSaved = true;
        }));

        private RelayCommand _addPlanetCommand;

        public RelayCommand AddPlanetCommand => _addPlanetCommand ?? (_addPlanetCommand = new RelayCommand(() =>
        {
            Debug.WriteLine("AddPlanet");
            EditingObject = AddPlanetView(X,Y);
            IsEditingObject = true;
            IsMovingObject = true;
        }));

        private RelayCommand _deleteObjectCommand;

        public RelayCommand DeleteObjectCommand =>
            _deleteObjectCommand ?? (_deleteObjectCommand = new RelayCommand(() =>
            {
                Debug.WriteLine("Delete");
                Simulator.DeleteObject(EditingObject.ID);
                ObjectViewCollection.Remove(EditingObject);
                IsEditingObject = false;
                IsMovingObject = false;
                EditingObject = null;
            }));

        private RelayCommand _startRunningCommand;

        public RelayCommand StartRunningCommand => _startRunningCommand ??
                                                   (_startRunningCommand = new RelayCommand(() =>
                                                   {
                                                       IsEditingName = false;
                                                       IsEditingObject = false;
                                                       IsMovingObject = false;
                                                       IsRunning = true;
                                                       EditingObject = null;
                                                       Simulator.Start();
                                                   }));

        private RelayCommand _stopRunningCommand;

        public RelayCommand StopRunningCommand => _stopRunningCommand ?? (_stopRunningCommand = new RelayCommand(() =>
        {
            IsRunning = false;
            Simulator.Stop();
        }));
    }
}