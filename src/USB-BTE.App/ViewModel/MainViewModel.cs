using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using USB.BTE.Wrapper;
using USB_BTE.App.Handlers;
using USB_BTE.App.Properties;

namespace USB_BTE.App.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private bool _initialized;
        private int _deviceHandle;

        private Icon Icon => AreRaleysOpen ? Resources.ball_green : Resources.ball_grey;

        private readonly TrayViewModel _trayViewModel;
        private readonly IStateHandler _stateHandler;

        private WindowState _curWindowState;

        private bool _areRaleysOpen;
        private bool AreRaleysOpen
        {
            get => _areRaleysOpen;
            set
            {
                _areRaleysOpen = value;
                RaisePropertyChanged();
            }
        }

        public WindowState CurWindowState
        {
            get => _curWindowState;
            set
            {
                _curWindowState = value;
                RaisePropertyChanged();
            }
        }

        private Visibility _visibility;
        public Visibility Visibility
        {
            get => _visibility;
            set
            {
                _visibility = value;
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
        public MainViewModel(TrayViewModel trayViewModel, IStateHandler stateHandler)
        {
            this.PropertyChanged += MainViewModel_PropertyChanged;
            _curWindowState = WindowState.Minimized;
            _visibility = Visibility.Hidden;

            _trayViewModel = trayViewModel;
            _stateHandler = stateHandler;
            _trayViewModel.SetMenuItems(new List<TrayMenuItemViewModel>()
            {
                new TrayMenuItemViewModel
                {
                    Id = "show",
                    Text = "Show"
                },
                new TrayMenuItemViewModel
                {
                    Id = "exit",
                    Text = "Exit"
                }
            });
            _trayViewModel.OnDoubleClick += TrayViewModelOnOnDoubleClick;
            _trayViewModel.OnMenuItemClick += _trayViewModel_OnMenuItemClick;
            Initialize();
        }

        private void MainViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(AreRaleysOpen))
                _trayViewModel.SetIcon(Icon);
        }

        /// <summary>
        ///     Initializes C++ library 
        /// </summary>
        private void Initialize()
        {
            _initialized = RelayDeviceWrapper.Init();
            if (_initialized)
            {
                var deviceInfo = RelayDeviceWrapper.GetDeviceInfo();
                if (deviceInfo != null)
                {
                    _deviceHandle = RelayDeviceWrapper.OpenDevice(deviceInfo.SerialNumber);
                    var raleysWereOpened = _stateHandler.GetState();
                    if (raleysWereOpened)
                    {
                        if (RelayDeviceWrapper.OpenAllRelays(_deviceHandle))
                        {
                            AreRaleysOpen = true;
                            return;
                        }
                    }
                }
                else
                {
                    _initialized = false;
                }
            }
            AreRaleysOpen = false;
        }

        private void _trayViewModel_OnMenuItemClick(object sender, EventArgs.MenuItemClickEventArgs e)
        {
            switch (e.Id)
            {
                case "show":
                    {
                        if (CurWindowState == WindowState.Minimized)
                        {
                            CurWindowState = WindowState.Normal;
                            Visibility = Visibility.Visible;
                        }
                        break;
                    }
                case "exit":
                    {
                        Application.Current.Shutdown();
                        break;
                    }
            }
        }

        /// <summary>
        ///     Toggle Relay On/Off
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TrayViewModelOnOnDoubleClick(object sender, System.EventArgs e)
        {
            if (_initialized)
            {
                if (AreRaleysOpen)
                {
                    RelayDeviceWrapper.CloseAllRelays(_deviceHandle);
                }
                else
                {
                    RelayDeviceWrapper.OpenAllRelays(_deviceHandle);
                }
                AreRaleysOpen = !AreRaleysOpen;
                _stateHandler.SetState(AreRaleysOpen);
            }
        }
    }
}