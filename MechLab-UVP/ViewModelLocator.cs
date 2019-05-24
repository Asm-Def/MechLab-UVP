using GalaSoft.MvvmLight.Ioc;
using MechLabLibrary.ViewModel;

namespace MechLab_UVP
{
    public class ViewModelLocator
    {
        public static readonly ViewModelLocator Instance = new ViewModelLocator();

        private ViewModelLocator()
        {
            SimpleIoc.Default.Register<MainPageViewModel>();
            SimpleIoc.Default.Register<LabInfoViewModel>();
            SimpleIoc.Default.Register<LabPageViewModel>();
        }

        public MainPageViewModel MainPageViewModel => SimpleIoc.Default.GetInstance<MainPageViewModel>();
        public LabInfoViewModel LabInfoViewModel => SimpleIoc.Default.GetInstance<LabInfoViewModel>();
        public LabPageViewModel LabPageViewModel(string key) => SimpleIoc.Default.GetInstance<LabPageViewModel>(key);
    }
}