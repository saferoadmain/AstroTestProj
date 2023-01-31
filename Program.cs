using Saferoad.Protocol.Astro;
namespace AstroTestProj
{
    class Program
    {
        static void Main(string[] args)
        {
            var samplePacketBytes = TestConst.locBytes;
            TestHeader(samplePacketBytes);
            TestBody(samplePacketBytes);

            var samplePacketBatchBytes = TestConst.BchBytes;
            TestHeader(samplePacketBatchBytes);
            TestBody(samplePacketBatchBytes);

            Console.WriteLine("Test Completed");
        }

        static void TestHeader(byte[] RawPacketBytes)
        {
            (MessageIDs MsgId, string IMEI, byte[] Response) = Astro500.ParseHeader(RawPacketBytes);
            Console.WriteLine($"Test Header: MsgId: {MsgId}, IMEI: {IMEI}");
        }

        static void TestBody(byte[] RawPacketBytes)
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