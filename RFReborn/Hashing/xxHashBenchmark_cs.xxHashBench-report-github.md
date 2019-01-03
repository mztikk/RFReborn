``` ini

BenchmarkDotNet=v0.11.3, OS=Windows 10.0.17763.195 (1809/October2018Update/Redstone5)
Intel Core i5-3570K CPU 3.40GHz (Ivy Bridge), 1 CPU, 4 logical and 4 physical cores
.NET Core SDK=3.0.100-preview-009844
  [Host]     : .NET Core 2.1.6 (CoreCLR 4.6.27019.06, CoreFX 4.6.27019.05), 64bit RyuJIT
  DefaultJob : .NET Core 2.1.6 (CoreCLR 4.6.27019.06, CoreFX 4.6.27019.05), 64bit RyuJIT


```
|             Method |        Mean |      Error |     StdDev |      Median | Rank | Gen 0/1k Op | Gen 1/1k Op | Gen 2/1k Op | Allocated Memory/Op |
|------------------- |------------:|-----------:|-----------:|------------:|-----:|------------:|------------:|------------:|--------------------:|
|    [RFReborn_XXHash](https://github.com/mztikk/RFReborn/blob/e20dcd123881974f21af0b4a2448eb633b98ed8d/RFReborn/Hashing/xxHash.cs) |    575.3 ns |   1.519 ns |   1.269 ns |    575.5 ns |    1 |           - |           - |           - |                   - |
|         [K4os_XXH32](https://github.com/MiloszKrajewski/K4os.Hash.xxHash/blob/94b264a8186d16a1be040a47a7d0742ccf46b4b2/src/K4os.Hash.xxHash/XXH32.cs) |    593.8 ns |  11.469 ns |  14.505 ns |    603.0 ns |    2 |           - |           - |           - |                   - |
|  [Standart_xxHash32](https://github.com/uranium62/xxHash/blob/462293ee0eaff624d3a69d60538a77edc199e9eb/src/Standart.Hash.xxHash/xxHash32.Unsafe.cs) |    594.2 ns |   6.913 ns |   6.467 ns |    593.3 ns |    2 |           - |           - |           - |                   - |
| [YYProject_XXHash32](https://github.com/differentrain/YYProject.XXHash/blob/a40f30b354e9d7bc1e9d4471541af6340eb31a08/XXHash/YYProject.XXHash/XXHash.cs) |  3,286.1 ns |  47.652 ns |  44.574 ns |  3,261.6 ns |    3 |      0.0458 |           - |           - |               152 B |
| [xxHashSharp_xxHash](https://github.com/noricube/xxHashSharp/blob/6c68c998e1b1ee2ba619b7bf998f8fc30083eaff/xxHashSharp/xxHash.cs) |  5,914.4 ns |  77.198 ns |  72.211 ns |  5,908.2 ns |    4 |      0.0229 |           - |           - |                96 B |
|  [NeoSmart_XXHash32](https://github.com/neosmart/hashing.net/blob/ed4831aedaea56f7a9f75b1bf934f9885ff70387/xxHash/Core/XXHash.cs) | 10,369.3 ns | 141.201 ns | 132.080 ns | 10,443.5 ns |    5 |      0.0153 |           - |           - |                96 B |


``` ini

BenchmarkDotNet=v0.11.3, OS=Windows 10.0.17763.195 (1809/October2018Update/Redstone5)
Intel Core i5-3570K CPU 3.40GHz (Ivy Bridge), 1 CPU, 4 logical and 4 physical cores
.NET Core SDK=3.0.100-preview-009844
  [Host]     : .NET Core 2.1.6 (CoreCLR 4.6.27019.06, CoreFX 4.6.27019.05), 64bit RyuJIT
  DefaultJob : .NET Core 2.1.6 (CoreCLR 4.6.27019.06, CoreFX 4.6.27019.05), 64bit RyuJIT


```
|             Method |        Mean |      Error |     StdDev | Rank | Gen 0/1k Op | Gen 1/1k Op | Gen 2/1k Op | Allocated Memory/Op |
|------------------- |------------:|-----------:|-----------:|-----:|------------:|------------:|------------:|--------------------:|
|    RFReborn_XXHash |    580.0 ns |   6.741 ns |   6.305 ns |    1 |           - |           - |           - |                   - |
|         K4os_XXH32 |    580.6 ns |   8.077 ns |   7.556 ns |    1 |           - |           - |           - |                   - |
|  Standart_xxHash32 |    594.9 ns |   6.882 ns |   6.437 ns |    2 |           - |           - |           - |                   - |
| YYProject_XXHash32 |  3,356.6 ns |  66.160 ns |  90.560 ns |    3 |      0.0458 |           - |           - |               152 B |
| xxHashSharp_xxHash |  5,824.7 ns |  10.540 ns |   8.229 ns |    4 |      0.0229 |           - |           - |                96 B |
|  NeoSmart_XXHash32 | 10,469.3 ns | 207.874 ns | 231.051 ns |    5 |      0.0153 |           - |           - |                96 B |


```csharp
    [Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.FastestToSlowest)]
    [RankColumn]
    [MemoryDiagnoser]
    [DisassemblyDiagnoser]
    public class xxHashBench
    {
        private byte[] bytes;

        private xxHashSharp.xxHash _xxHashSharp;

        private YYProject.XXHash.XXHash32 _YYProject;

        [GlobalSetup]
        public void Init()
        {
            bytes = bytes = File.ReadAllBytes("lorem.txt");

            _xxHashSharp = new xxHashSharp.xxHash();
            _YYProject = new YYProject.XXHash.XXHash32();
        }

        [Benchmark]
        public uint RFReborn_XXHash() => RFReborn.Hashing.xxHash.Hash(bytes);

        [Benchmark]
        public uint YYProject_XXHash32()
        {
            _YYProject.ComputeHash(bytes);
            return _YYProject.HashUInt32;
        }

        [Benchmark]
        public uint xxHashSharp_xxHash()
        {
            _xxHashSharp.Init();
            _xxHashSharp.Update(bytes, bytes.Length);
            return _xxHashSharp.Digest();
        }

        [Benchmark]
        public unsafe uint K4os_XXH32() => K4os.Hash.xxHash.XXH32.DigestOf(bytes);

        [Benchmark]
        public uint Standart_xxHash32() => Standart.Hash.xxHash.xxHash32.ComputeHash(bytes, bytes.Length);

        [Benchmark]
        public uint NeoSmart_XXHash32() => NeoSmart.Hashing.XXHash.XXHash32.Hash(bytes);
    }
```

lorem.txt:
```txt
Lorem ipsum dolor sit amet, consetetur sadipscing elitr,  sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr,  sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr,  sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet.
Duis autem vel eum iriure dolor in hendrerit in vulputate velit esse molestie consequat, vel illum dolore eu feugiat nulla facilisis at vero eros et accumsan et iusto odio dignissim qui blandit praesent luptatum zzril delenit augue duis dolore te feugait nulla facilisi. Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonummy nibh euismod tincidunt ut laoreet dolore magna aliquam erat volutpat.
Ut wisi enim ad minim veniam, quis nostrud exerci tation ullamcorper suscipit lobortis nisl ut aliquip ex ea commodo consequat. Duis autem vel eum iriure dolor in hendrerit in vulputate velit esse molestie consequat, vel illum dolore eu feugiat nulla facilisis at vero eros et accumsan et iusto odio dignissim qui blandit praesent luptatum zzril delenit augue duis dolore te feugait nulla facilisi.
Nam liber tempor cum soluta nobis eleifend option congue nihil imperdiet doming id quod mazim placerat facer possim assum. Lorem ipsum dolor sit amet, consectetuer adipiscing elit, sed diam nonummy nibh euismod tincidunt ut laoreet dolore magna aliquam erat volutpat. Ut wisi enim ad minim veniam, quis nostrud exerci tation ullamcorper suscipit lobortis nisl ut aliquip ex ea commodo consequat.
Duis autem vel eum iriure dolor in hendrerit in vulputate velit esse molestie consequat, vel illum dolore eu feugiat nulla facilisis.
At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr,  sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr,  At accusam aliquyam diam diam dolore dolores duo eirmod eos erat, et nonumy sed tempor et et invidunt justo labore Stet clita ea et gubergren, kasd magna no rebum. sanctus sea sed takimata ut vero voluptua. est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr,  sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat. 
Consetetur sadipscing elitr,  sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr,  sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr,  sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet.
```
