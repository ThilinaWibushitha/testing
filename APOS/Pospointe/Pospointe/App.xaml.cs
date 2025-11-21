using Pospointe.posAssist;
using System.Data;
using System.Net;
using System.Net.Sockets;
using System.Windows;
using System.Windows.Threading;

namespace Pospointe;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private DispatcherTimer _updateTimer;
    private DispatcherTimer _paxDetectionTimer;
    private const int PaxPort = 10009; // PAX A35 Port
    private const int ScanTimeout = 500;


    protected override void OnStartup(StartupEventArgs e)
    {
        if (Environment.OSVersion.Version.Major >= 6)
        {
            SetProcessDPIAware();
        }
        base.OnStartup(e);
        PrinterQueueService queueService = new PrinterQueueService();
        queueService.Start();
        StartAutoUpdateCheck();

    }


    private async Task<string> ScanForPaxA35()
    {
        string subnet = GetLocalSubnet();
        if (string.IsNullOrEmpty(subnet))
        {
            return "";
        }

        Task<string>[] scanTasks = Enumerable.Range(1, 254)
            .Select(i => CheckPaxDevice($"{subnet}.{i}", PaxPort))
            .ToArray();

        string[] results = await Task.WhenAll(scanTasks);
        return results.FirstOrDefault(ip => !string.IsNullOrEmpty(ip)) ?? "";
    }

    private async Task<string> CheckPaxDevice(string ipAddress, int port)
    {
        using (TcpClient client = new TcpClient())
        {
            try
            {
                Task connectTask = client.ConnectAsync(ipAddress, port);
                bool connected = await Task.WhenAny(connectTask, Task.Delay(ScanTimeout)) == connectTask;

                if (connected && client.Connected)
                {
                    return ipAddress;
                }
            }
            catch { }
        }
        return "";
    }

    private string GetLocalSubnet()
    {
        foreach (var ip in Dns.GetHostAddresses(Dns.GetHostName()))
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                string[] parts = ip.ToString().Split('.');
                return $"{parts[0]}.{parts[1]}.{parts[2]}";
            }
        }
        return "";
    }


    private void StartAutoUpdateCheck()
    {
        _updateTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMinutes(1)
        };
        _updateTimer.Tick += async (sender, args) => await CheckAndUpdateInBackground();
        _updateTimer.Start();

        _ = CheckAndUpdateInBackground();
    }

    private async Task CheckAndUpdateInBackground()
    {
        try
        {
            await Task.Run(() => UpdateManager.CheckAndUpdate());
        }
        catch (Exception ex)
        {
            Console.WriteLine("Auto Update Error: " + ex.Message);
        }
    }


    [System.Runtime.InteropServices.DllImport("user32.dll")]
    private static extern bool SetProcessDPIAware();



}
