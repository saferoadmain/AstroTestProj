using System.Net;
using System.Net.Sockets;
using Saferoad.Protocol.Astro;

namespace AstroTestProj
{
    internal static class SimpleTcpListener
    {
        internal static async Task RunListener(int port = 500)
        {
            try
            {
                IPAddress ipAddress = IPAddress.Parse("0.0.0.0");
                while (true)
                {
                    Console.WriteLine($"{DateTime.UtcNow}: Starting TCP listener IP({ipAddress}), Port:({port})...");
                    TcpListener tcpListener = new TcpListener(ipAddress, port);

                    tcpListener.Start();
                    try
                    {
                        while (true)
                        {
                            using (var client = await tcpListener.AcceptTcpClientAsync())
                            {
                                Console.WriteLine($"{DateTime.UtcNow}: Connection accepted.");
                                await RunReader(client);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"{DateTime.UtcNow}: RunListener Error: " + ex.Message);
                        Console.ReadLine();
                    }
                    Console.WriteLine($"{DateTime.UtcNow}: Stopping TCP listener...");
                    tcpListener.Stop();
                    Console.WriteLine($"{DateTime.UtcNow}: Stopped TCP Stopped");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{DateTime.UtcNow}: RunListener Error: " + ex.Message);
                Console.ReadLine();
            }
        }

        internal static async Task RunReader(TcpClient client)
        {
            using (var stream = client.GetStream())
            {
                try
                {
                    var fullPacket = new List<byte>();

                    var bytes = new byte[4096];
                    while (await stream.ReadAsync(bytes, 0, bytes.Length) != 0)
                    {
                        byte[] RawPacketBytes = TrimEnd(bytes);
                        var hd = Astro500.GetHeader(RawPacketBytes);
                        Console.WriteLine($"{DateTime.UtcNow}: (Header) MsgId: {hd.MsgId}, IMEI: {hd.IMEI}, Count: {hd.Count}");

                        var pkt = Astro500.GetPacket(RawPacketBytes);
                        Console.WriteLine($"{DateTime.UtcNow}: (Body) MsgId: {pkt.MsgId}, IMEI: {pkt.IMEI}, Count: {pkt.Count}, LocationCounts: {pkt.Locations.Count}");
                        foreach (var loc in pkt.Locations)
                        {
                            Console.WriteLine($"{DateTime.UtcNow}: (Body) - Loc: {loc.RecordDateTime}, Speed:{loc.Speed}, Lat:{loc.Latitude}, Long:{loc.Longitude}");
                        }
                        if (client?.Connected ?? false) stream.Write(hd.Response, 0, hd.Response.Length);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{DateTime.UtcNow}: GetStream Error: " + ex.Message);
                }
            }

        }
        static byte[] TrimEnd(byte[] array)
        {
            int lastIndex = Array.FindLastIndex(array, b => b != 0);

            Array.Resize(ref array, lastIndex + 1);

            return array;
        }
    }
}