using System.Net;
using System.Net.Sockets;
using Saferoad.Protocol.Astro;

namespace AstroTestProj
{
    static class SimpleTcpListener
    {
        public static void RunListener(int port = 500)
        {
            try
            {
                IPAddress ipAddress = IPAddress.Parse("127.0.0.1");

                while (true)
                {
                    Console.WriteLine($"Starting TCP listener: IP({ipAddress}), Port:({port})...");
                    TcpListener listener = new TcpListener(ipAddress, port);

                    listener.Start();
                    using (var client = listener.AcceptTcpClient())
                    {
                        Console.WriteLine("Connection accepted.");
                        using (var stream = client.GetStream())
                        {
                            try
                            {
                                var fullPacket = new List<byte>();

                                var bytes = new byte[4096];
                                while (stream.Read(bytes, 0, bytes.Length) != 0)
                                {
                                    byte[] RawPacketBytes = TrimEnd(bytes);
                                    (MessageIDs MsgId, string IMEI, byte[] Response) = Astro500.ParseHeader(RawPacketBytes);
                                    Console.WriteLine($"Test Header: MsgId: {MsgId}, IMEI: {IMEI}");

                                    (MsgId, IMEI, Response, List<Astro500Location> Locations) = Astro500.ParseBody(RawPacketBytes);
                                    Console.WriteLine($"Body: MsgId: {MsgId}, IMEI: {IMEI}, LocationCounts: {Locations.Count}");
                                    foreach (var loc in Locations)
                                    {
                                        Console.WriteLine($"Body - Loc: {loc.RecordDateTime}, Speed:{loc.Speed}, Lat:{loc.Latitude}, Long:{loc.Longitude}");
                                    }
                                    stream.Write(Response, 0, Response.Length);
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine("GetStream Error: " + ex.Message);
                            }
                        }
                    }
                    Console.WriteLine("Starting TCP listener...");
                    listener.Stop();
                    Console.WriteLine("Starting TCP Stopped");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("RunListener Error: " + ex.Message);
                Console.ReadLine();
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