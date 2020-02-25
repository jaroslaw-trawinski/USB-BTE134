using System.Runtime.InteropServices;

namespace USB.BTE.Wrapper.Models
{
    /// <summary>
    ///     Device info structure
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 8)]
    public class DeviceInfo
    {
        [MarshalAs(UnmanagedType.LPStr)]
        public string SerialNumber;

        [MarshalAs(UnmanagedType.LPStr)]
        public string DevicePath;

        public UsbRelayDeviceType Type { get; set; }
    }
}