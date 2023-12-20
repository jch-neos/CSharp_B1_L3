using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System.Collections.Frozen;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;

BenchmarkSwitcher.FromAssemblies(AppDomain.CurrentDomain.GetAssemblies()).Run(args: args);

[MemoryDiagnoser, DisassemblyDiagnoser]
public class Bench {
  [Params(10_000L,100_000L,1_000_000L)]
  public int Count { get; set; }
  ImmutableArray<int> test = ImmutableArray<int>.Empty;
  int[]? items;
  Dictionary<string, int>? d;
  ImmutableDictionary<string, int>? id ;
  FrozenDictionary<string, int>? fd ;
  

  [GlobalSetup]
  public void BenchSetup() {
    items = new int[Count];
    Array.Fill(items, 0x12345678); // Range(1, Count).ToArray();
    var set = Enumerable.Range(0, Count).Select(i => new KeyValuePair<string, int>(i.ToString(), 31*i%17)).ToArray();
    d = new Dictionary<string, int>(set);
    fd = d.ToFrozenDictionary();
    id = d.ToImmutableDictionary();
  }

  [Benchmark]
  public void LookupFrozenDict() {
    var dict = fd[Random.Shared.Next(Count).ToString()];
  }

  [Benchmark]
  public void LookupDict() {
    var dict = d[Random.Shared.Next(Count).ToString()];
  }
  [Benchmark]
  public void LookupImmutableDict() {
    var dict = id[Random.Shared.Next(Count).ToString()];
  }
  
  [Benchmark]
  public void  BuildDictTest() {
    var dict = new Dictionary<string, int>(d!);
  }

  [Benchmark]
  public void  BuildFrozenDictTest() {
    var dict = d!.ToFrozenDictionary();
  }

  [Benchmark]
  public void BuildImmutableDictTest() {
    var dict = d!.ToImmutableDictionary();
  }

}
