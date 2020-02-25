using System;
using System.Runtime.InteropServices;
using USB.BTE.Wrapper.Models;

namespace USB.BTE.Wrapper
{
    /// <summary>
    ///     Wrapper class over "usb_relay_device.dll" C++ library
    /// </summary>
    public static class RelayDeviceWrapper
    {
        private static bool _initialized;

        [DllImport("USB_RELAY_DEVICE.dll", EntryPoint = "usb_relay_init", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        private static extern int UsbRelayInit();
        
        [DllImport("USB_RELAY_DEVICE.dll", EntryPoint = "usb_relay_exit", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Auto)]
        private static extern int UsbRelayExit();

        [DllImport("usb_relay_device.dll", EntryPoint = "usb_relay_device_enumerate", CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr UsbRelayDeviceEnumerate();

        [DllImport("usb_relay_device.dll", EntryPoint = "usb_relay_device_open_with_serial_number", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true,
            CallingConvention = CallingConvention.Cdecl)]
        private static extern int UsbRelayDeviceOpenWithSerialNumber([MarshalAs(UnmanagedType.LPStr)] string serialNumber, int len);

        [DllImport("usb_relay_device.dll", EntryPoint = "usb_relay_device_open_one_relay_channel", SetLastError = true,
            CharSet = CharSet.Ansi, ExactSpelling = true,
            CallingConvention = CallingConvention.Cdecl)]
        private static extern int UsbRelayDeviceOpenOneRelayChannel(int hHandle, int index);

        [DllImport("usb_relay_device.dll", EntryPoint = "usb_relay_device_open_all_relay_channel", SetLastError = true,
            CharSet = CharSet.Ansi, ExactSpelling = true,
            CallingConvention = CallingConvention.Cdecl)]
        private static extern int UsbRelayDeviceOpenAllRelayChannel(int hHandle);

        [DllImport("usb_relay_device.dll", EntryPoint = "usb_relay_device_close_one_relay_channel", SetLastError = true,
            CharSet = CharSet.Ansi, ExactSpelling = true,
            CallingConvention = CallingConvention.Cdecl)]
        private static extern int UsbRelayDeviceCloseOneRelayChannel(int hHandle, int index);

        [DllImport("usb_relay_device.dll", EntryPoint = "usb_relay_device_close_all_relay_channel", SetLastError = true,
            CharSet = CharSet.Ansi, ExactSpelling = true,
            CallingConvention = CallingConvention.Cdecl)]
        private static extern int UsbRelayDeviceCloseAllRelayChannel(int hHandle);


        /// <summary>
        ///     Trying to initialize a library. This function should be called before all other.
        /// </summary>
        public static bool Init()
        {
            _initialized = (UsbRelayInit() == 0);
            return _initialized;
        }

        /// <summary>
        ///     Trying to close a library. This function should be called before all other.
        /// </summary>
        /// <returns>True if succeeded, otherwise false.</returns>
        public static bool Exit()
        {
            if (_initialized && UsbRelayExit() == 0)
            {
                _initialized = false;
                return true;
            }
            return false;
        }

        /// <summary>
        ///     Gets device device information.
        /// </summary>
        /// <returns>True if succeeded, otherwise false.</returns>
        public static DeviceInfo GetDeviceInfo()
        {
            var x = UsbRelayDeviceEnumerate();
            var deviceInfo = (DeviceInfo)Marshal.PtrToStructure(x, typeof(DeviceInfo));
            return deviceInfo;
        }

        /// <summary>
        ///     Opens device based on provided serial number.
        /// </summary>
        /// <param name="serialNumber">The device's serial number.</param>
        /// <returns>Device handle.</returns>
        public static int OpenDevice(string serialNumber)
        {
            return UsbRelayDeviceOpenWithSerialNumber(serialNumber, serialNumber.Length);
        }

        /// <summary>
        ///     Opens particular relay.
        /// </summary>
        /// <param name="dHandle">The device handle</param>
        /// <param name="index">The relay index.</param>
        /// <returns>True if succeeded, otherwise false.</returns>
        public static bool OpenRelay(int dHandle, int index)
        {
            return UsbRelayDeviceOpenOneRelayChannel(dHandle, index) == 0;
        }

        /// <summary>
        ///     Opens all relays in device.
        /// </summary>
        /// <param name="dHandle">The device handle.</param>
        /// <returns>True if succeeded, otherwise false.</returns>
        public static bool OpenAllRelays(int dHandle)
        {
            return UsbRelayDeviceOpenAllRelayChannel(dHandle) == 0;
        }

        /// <summary>
        ///     Closes particular relay.
        /// </summary>
        /// <param name="dHandle">The device handle.</param>
        /// <param name="index">The index of the relay.</param>
        /// <returns>True if succeeded, otherwise false.</returns>
        public static bool CloseRelay(int dHandle, int index)
        {
            return UsbRelayDeviceCloseOneRelayChannel(dHandle, index) == 0;
        }

        /// <summary>
        ///     Closes all relays.
        /// </summary>
        /// <param name="dHandle">The device handle.</param>
        /// <returns>True if succeeded, otherwise false.</returns>
        public static bool CloseAllRelays(int dHandle)
        {
            return UsbRelayDeviceCloseAllRelayChannel(dHandle) == 0;
        }
    }
}