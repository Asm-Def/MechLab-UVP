﻿using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MechLabLibrary.Models;

namespace MechLabLibrary.ViewModel
{
    public class MechLabViewModel : ViewModelBase
    {
        private MechLabServices _mechLabServices;

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
            set => Set(nameof(EyeShot), ref _eyeShot, value);
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

        private bool _editingName;

        public bool EditingName
        {
            get => _editingName;
            set => Set(nameof(EditingName), ref _editingName, value);
        }

        private bool _isEditingObject;

        public bool IsEditingObject
        {
            get => _isEditingObject;
            set => Set(nameof(IsEditingObject), ref _isEditingObject, value);
        }

        private MechObjectView _editingObject;

        public MechObjectView EditingObject
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
            EyeShot = 1;
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
            EyeShot = 1;
            X = 0;
            Y = 0;
            Name = "Untitled";
            foreach (MechObject mechObject in Simulator._objects)
            {
                if (mechObject.Type == "Planet")
                {
                    MechPlanetView planetView =
                        new MechPlanetView(ObjectViewCollection.Count, (MechPlanet) mechObject, this);
                    ObjectViewCollection.Add(planetView);
                }
                else
                {
                    MechObjectView objectView = new MechObjectView(ObjectViewCollection.Count, mechObject, this);
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
            MechObjectView result = new MechObjectView(ObjectViewCollection.Count, mechObject, this);
            ObjectViewCollection.Add(result);
            return result;
        }

        /// <summary>
        /// 获取新的PlanetView
        /// </summary>
        /// <returns></returns>
        public MechPlanetView AddPlanetView(double x = 0, double y = 0, double vx = 0, double vy = 0, double m = 1,
            double r = 0)
        {
            MechPlanet mechPlanet = Simulator.AddPlanet(x, y, vx, vy, m, r);
            MechPlanetView result = new MechPlanetView(ObjectViewCollection.Count, mechPlanet, this);
            ObjectViewCollection.Add(result);
            return result;
        }



        private RelayCommand _toggleEditingName;

        public RelayCommand ToggleEditingName => _toggleEditingName ?? (_toggleEditingName =
                                                     new RelayCommand(() =>
                                                     {
                                                         EditingName = !EditingName;
                                                     }));

        private RelayCommand _saveCommand;

        public RelayCommand SaveCommand => _saveCommand ?? (_saveCommand = new RelayCommand(() =>
        {
            Debug.WriteLine("saved");
            EditingName = false;
            IsSaved = true;
        }));

        private RelayCommand _addPlanetCommand;

        public RelayCommand AddPlanetCommand => _addPlanetCommand ?? (_addPlanetCommand = new RelayCommand(() =>
        {
            Debug.WriteLine("AddPlanet");
        }));

        private RelayCommand _addObjectCommand;

        public RelayCommand AddObjectCommand => _addObjectCommand ?? (_addObjectCommand = new RelayCommand(() =>
        {
            Debug.WriteLine("AddObject");
        }));

        private RelayCommand _copyObjectCommand;

        public RelayCommand CopyObjectCommand =>
            _copyObjectCommand ?? (_copyObjectCommand = new RelayCommand(() =>
            {
                Debug.WriteLine("Copy");
            }));

        private RelayCommand _deleteObjectCommand;

        public RelayCommand DeleteObjectCommand =>
            _deleteObjectCommand ?? (_deleteObjectCommand = new RelayCommand(() =>
            {
                Debug.WriteLine("Delete");
                ObjectViewCollection.Remove(EditingObject);
            }));

        private RelayCommand _startRunningCommand;

        public RelayCommand StartRunningCommand => _startRunningCommand ??
                                                   (_startRunningCommand = new RelayCommand(() =>
                                                   {
                                                       Simulator.Start();
                                                       IsRunning = true;
                                                   }));

        private RelayCommand _stopRunningCommand;

        public RelayCommand StopRunningCommand => _stopRunningCommand ?? (_stopRunningCommand = new RelayCommand(() =>
        {
            Simulator.Stop();
            IsRunning = false;
        }));
    }
}