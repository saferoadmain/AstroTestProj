using Saferoad.Protocol.Astro;
namespace AstroTestProj
{
    static class SimpleParser
    {
        public static void TestHeader(byte[] RawPacketBytes)
        {
            var hd = Astro500.GetHeader(RawPacketBytes);
            Console.WriteLine($"{DateTime.UtcNow}: (Header) IMEI: {hd.IMEI}, Count: {hd.Headers.Count}, MsgIds: {String.Join(',', hd.Headers.Select(x => x.MsgId))}");
        }

        public static void TestBody(byte[] RawPacketBytes)
        {
            var pkt = Astro500.GetPacket(RawPacketBytes);
            Console.WriteLine($"{DateTime.UtcNow}: (Body) IMEI: {pkt.IMEI}, Count: {pkt.Headers.Count}, LocationCounts: {pkt.Locations.Count}, MsgIds: {String.Join(',', pkt.Headers.Select(x => x.MsgId))}");

            foreach (var loc in pkt.Locations)
            {
                Console.WriteLine($"{DateTime.UtcNow}: (Body) - Loc: {loc.RecordDateTime}, Speed:{loc.Speed}, Lat:{loc.Latitude}, Long:{loc.Longitude}");
            }
        }
    }
}