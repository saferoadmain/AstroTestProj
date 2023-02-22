using Saferoad.Protocol.Astro;
namespace AstroTestProj
{
    static class SimpleParser
    {
        public static void TestHeader(byte[] RawPacketBytes)
        {
            var hd = Astro500.GetHeader(RawPacketBytes);
            Console.WriteLine($"{DateTime.UtcNow}: (Header) MsgId: {hd.MsgId}, IMEI: {hd.IMEI}, Count: {hd.Count}");
        }

        public static void TestBody(byte[] RawPacketBytes)
        {
            var pkt = Astro500.GetPacket(RawPacketBytes);
            Console.WriteLine($"{DateTime.UtcNow}: (Body) MsgId: {pkt.MsgId}, IMEI: {pkt.IMEI}, Count: {pkt.Count}, LocationCounts: {pkt.Locations.Count}");
            foreach (var loc in pkt.Locations)
            {
                Console.WriteLine($"{DateTime.UtcNow}: (Body) - Loc: {loc.RecordDateTime}, Speed:{loc.Speed}, Lat:{loc.Latitude}, Long:{loc.Longitude}");
            }
        }
    }
}