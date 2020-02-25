using System;
using USB.BTE.Wrapper;

namespace USB.BTE.TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.Write("Initializing...");
                if (RelayDeviceWrapper.Init())
                {
                    Console.WriteLine("succeeded.");
                    var deviceInfo = RelayDeviceWrapper.GetDeviceInfo();
                    var serialNumber = deviceInfo.SerialNumber;
                    Console.WriteLine($"Device serial number: {serialNumber}");

                    var deviceHandle = RelayDeviceWrapper.OpenDevice(serialNumber);
                    Console.WriteLine($"Device: {serialNumber} opened, handle: {deviceHandle}");

                    Console.Write("Calling method to open all relays... ");
                    if(RelayDeviceWrapper.OpenAllRelays(deviceHandle))
                    {
                        Console.WriteLine("succeeded.");
                    }
                    else
                    {
                        Console.WriteLine("failed.");
                    }

                    System.Threading.Thread.Sleep(2000);
                    Console.Write("Calling method to close all relays... ");
                    if (RelayDeviceWrapper.CloseAllRelays(deviceHandle))
                    {
                        Console.WriteLine("succeeded.");
                    }
                    else
                    {
                        Console.WriteLine("failed.");
                    }

                    System.Threading.Thread.Sleep(2000);
                    for (var i = 1; i <= 16; i++)
                    {
                        System.Threading.Thread.Sleep(500);
                        Console.Write($"Opening relay index: {i}... ");
                        if (RelayDeviceWrapper.OpenRelay(deviceHandle, i))
                        {
                            Console.WriteLine("succeeded.");
                        }
                        else
                        {
                            Console.WriteLine("failed.");
                        }
                    }

                    System.Threading.Thread.Sleep(2000);
                    for (var i = 1; i <= 16; i++)
                    {
                        System.Threading.Thread.Sleep(500);
                        Console.Write($"Closing relay index: {i}... ");
                        if (RelayDeviceWrapper.CloseRelay(deviceHandle, i))
                        {
                            Console.WriteLine("succeeded.");
                        }
                        else
                        {
                            Console.WriteLine("failed.");
                        }
                    }
                    Console.WriteLine("Exiting...");

                    if (RelayDeviceWrapper.Exit())
                    {
                        Console.WriteLine("Exited successfully.");
                    }
                }
                else
                {
                    Console.WriteLine("failed.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occured: {ex}");
            }
            Console.ReadLine();
        }
    }
}