using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;

namespace USB_BTE.App.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private WindowState _curWindowState;
        public WindowState CurWindowState
        {
            get => _curWindowState;
            set
            {
                _curWindowState = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the MainViewModel class.
        /// </summary>
        [PreferredConstructor]
        public MainViewModel(TrayViewModel trayViewModel)
        {
            _curWindowState = WindowState.Normal;
        }
    }
}