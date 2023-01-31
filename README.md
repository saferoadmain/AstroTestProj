<figure align="center">
  <img
  src=".\astro_logo.png"
  alt="Astro Devices by Saferoad" width="30%">
</figure>
<!-- <img src=".\test.png" alt="testing" width="280"/> -->

# Create your project
Use your termainal to create new project, <BR>
You can pull this github repo directly instead

```Terminal
dotnet new console -o "AstroTestProj"
```

# Add Astro latest Package Reference to your Project
```XML
<ItemGroup>
    <PackageReference Include="Saferoad.Protocol.Astro" Version="1.0.6" />
</ItemGroup>
```
Then Restore your project as follow
```Terminal
dotnet restore
```

# Call the Library and include it in your code
```C#
using Saferoad.Protocol.Astro;
namespace AstroTestProj
{
    class Program
    {
        static void Main(string[] args)
        {
            .
            .
            .
            (MessageIDs MsgId, string IMEI, byte[] Response) = Astro500.ParseHeader(TestConst.locBytes);
            Console.WriteLine($"Test Header: MsgId: {MsgId}, IMEI: {IMEI}");
            .
            .
            .
            
```

# Run The Sampe Project

Then run your project execute the command in terminal
```Terminal
dotnet run
```


# Example and Result

<img src=".\test.png" alt="testing" width="800"/>
