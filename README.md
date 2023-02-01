<figure align="center">
  <img
  src=".\astro_logo.png"
  alt="Astro Devices by Saferoad" width="30%">
</figure>
<!-- <img src=".\test.png" alt="testing" width="280"/> -->

# Create your project
Use your termainal to create new project, <BR>
You can pull this github repo directly instead

```console
dotnet new console -o "AstroTestProj"
```

## Add Astro latest Package Reference to your Project
```xml
<ItemGroup>
    <PackageReference Include="Saferoad.Protocol.Astro" Version="1.0.6" />
</ItemGroup>
```
Then Restore your project as follow
```console
dotnet restore
```


## Run The Sampe Project

Then run your project execute the command in terminal
```console
dotnet run
```

# Simple Parser Example
## Call the Library and include it in your code
```C#
using Saferoad.Protocol.Astro;
namespace AstroTestProj
{
    class Program
    {
        static void Main(string[] args)
        {
            // ************************************************************************************************
            // Fast Parsing for header only, used to reterive and store body bytes only.
            // MsgId: Define Message Type(UNKNOWN, TER_HEART_BEAT, TER_LOG_OUT, TER_REGISTER, TER_LOC_INFO, TER_LOC_BATCH_INFO).
            // IMEI: Device IMEI Number.
            // Response: The response must be send back, to register the devices.
            // ************************************************************************************************
            (MessageIDs MsgId, string IMEI, byte[] Response) = Astro500.ParseHeader(TestConst.locBytes);
            Console.WriteLine($"Test Header: MsgId: {MsgId}, IMEI: {IMEI}");
            .
            .
            .
            // ************************************************************************************************
            // Full Parsing for the packet,
            // MsgId: Define Message Type(UNKNOWN, TER_HEART_BEAT, TER_LOG_OUT, TER_REGISTER, TER_LOC_INFO, TER_LOC_BATCH_INFO).
            // IMEI: Device IMEI Number.
            // Response: The response must be send back, to register the devices.
            // Locations: List, could be empty in case of packet types,
            //            Locations = 0: (UNKNOWN, TER_HEART_BEAT, TER_LOG_OUT, TER_REGISTER)
            //            Locations > 0: (TER_LOC_INFO)
            //            Locations > 1: (TER_LOC_BATCH_INFO)
            // ************************************************************************************************
            (MessageIDs MsgId, string IMEI, byte[] Response, List<Astro500Location> Locations) = Astro500.ParseBody(RawPacketBytes);
            Console.WriteLine($"Test Body: MsgId: {MsgId}, IMEI: {IMEI}, LocationCounts: {Locations.Count}");
            .            
```

## Example and Result

<img src=".\test.png" alt="testing" width="800"/>


# Simple TCP Licenser Example
## Call the Library and include it in your code

### Main TCP Listener
```C#
    TcpListener tcpListener = new TcpListener(ipAddress, port);
    .
    .
    .
        tcpListener.Start();
        try
        {
            while (true)
            {
                using (var client = await tcpListener.AcceptTcpClientAsync())
                {
                    Console.WriteLine("Connection accepted.");
                    await RunReader(client);
```

### Main TCP Listener
```C#
        public static async Task RunReader(TcpClient client)
        {
            using (var stream = client.GetStream())
            {
                try
                {
                    var fullPacket = new List<byte>();
                    var bytes = new byte[4096];
                    while (stream.Read(bytes, 0, bytes.Length) != 0)
                    {
                        byte[] RawPacketBytes = TrimEnd(bytes);
                        .
                        // ************************************************************************************************
                        // Full Parsing for the packet
                        // Get Response bytes object, 
                        // send it back to the device using 
                        // ************************************************************************************************
                        (MsgId, IMEI, Response, List<Astro500Location> Locations) = Astro500.ParseBody(RawPacketBytes);
                        Console.WriteLine($"Body: MsgId: {MsgId}, IMEI: {IMEI}, LocationCounts: {Locations.Count}");
                        foreach (var loc in Locations)
                        {
                            Console.WriteLine($"Body - Loc: {loc.RecordDateTime}, Speed:{loc.Speed}, Lat:{loc.Latitude}, Long:{loc.Longitude}");
                        }
                        
                        
                        // send response back to the device using stream.Write
                        stream.Write(Response, 0, Response.Length);
                        .
                        .
                        .
```

