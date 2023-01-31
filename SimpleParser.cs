
using Saferoad.Protocol.Astro;
namespace AstroTestProj
{
    static class SimpleParser
    {
        public static void TestHeader(byte[] RawPacketBytes)
        {
            (MessageIDs MsgId, string IMEI, byte[] Response) = Astro500.ParseHeader(RawPacketBytes);
            Console.WriteLine($"Test Header: MsgId: {MsgId}, IMEI: {IMEI}");
        }

        public static void TestBody(byte[] RawPacketBytes)
        {
            (MessageIDs MsgId, string IMEI, byte[] Response, List<Astro500Location> Locations) = Astro500.ParseBody(RawPacketBytes);
            Console.WriteLine($"Test Body: MsgId: {MsgId}, IMEI: {IMEI}, LocationCounts: {Locations.Count}");
            foreach (var loc in Locations)
            {
                Console.WriteLine($"Test Body - Loc: {loc.RecordDateTime}, Speed:{loc.Speed}, Lat:{loc.Latitude}, Long:{loc.Longitude}");
            }
        }
    }
}