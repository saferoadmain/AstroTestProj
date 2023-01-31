using Saferoad.Protocol.Astro;
namespace AstroTestProj
{
    class Program
    {
        static void Main(string[] args)
        {
            RunParser();
            SimpleTcpListener.RunListener();
        }

        static void RunParser()
        {
            Console.WriteLine("RunParser: Test Started");

            var samplePacketBytes = TestConst.locBytes;
            SimpleParser.TestHeader(samplePacketBytes);
            SimpleParser.TestBody(samplePacketBytes);

            var samplePacketBatchBytes = TestConst.BchBytes;
            SimpleParser.TestHeader(samplePacketBatchBytes);
            SimpleParser.TestBody(samplePacketBatchBytes);

            Console.WriteLine("RunParser: Test Completed");
        }
    }
}