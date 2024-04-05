// See https://aka.ms/new-console-template for more information
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

Console.WriteLine("Hello, World!");

LogLocalInterfaceAddresses();
StartTcpListener();

static void  StartSocketListener()
{
    IPAddress ipAddress = IPAddress.Any;

    var wifiListener = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    wifiListener.ExclusiveAddressUse = false;
    wifiListener.Bind (new IPEndPoint (ipAddress, 0));

    IPEndPoint endpoint = (IPEndPoint) wifiListener.LocalEndPoint!;

    Log ($"[SocketListener] Awaiting connection over WiFi to {endpoint.Address}:{endpoint.Port}");

    wifiListener.Listen (15);

    Thread.Sleep(120000);
}

static void  StartTcpListener()
{
    var wifiListener = new TcpListener(IPAddress.Any, 0);

    wifiListener.ExclusiveAddressUse = false;
    wifiListener.Start();

    IPEndPoint endpoint = (IPEndPoint) wifiListener.LocalEndpoint!;

    Log ($"[TcpListener] Awaiting connection over WiFi to {endpoint.Address}:{endpoint.Port}");

    Thread.Sleep(120000);
}

static void LogLocalInterfaceAddresses()
{
    var results = new List<string>();

    var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();

    foreach (var ni in networkInterfaces)
    {
        if (ni.Supports(NetworkInterfaceComponent.IPv4))
        {
            foreach (var ip in ni.GetIPProperties().UnicastAddresses)
            {
                if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    var strIp = ip.Address.ToString();

                    if (!results.Contains(strIp))
                    {
                        results.Add(strIp);
                    }
                }
            }
        }
    }

    Log($"Local IP addresses:");
    foreach (var ipAddress in results) {
        Log(ipAddress);
    }
}

static void Log(string message)
{
    Trace.WriteLine(message);
}