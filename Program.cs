using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System.Collections.Frozen;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;

BenchmarkSwitcher.FromAssemblies(AppDomain.CurrentDomain.GetAssemblies()).Run(args: args);

[MemoryDiagnoser, DisassemblyDiagnoser]
public class Bench {
  [Params(10_000L, 100_000L, 1_000_000L)]
  public int Count { get; set; }
  ImmutableArray<int> test = ImmutableArray<int>.Empty;
  int[]? items;
  Dictionary<string, int> d = new Dictionary<string, int> { };
  ImmutableDictionary<string, int> id = ImmutableDictionary<string, int>.Empty;
  FrozenDictionary<string, int> fd = FrozenDictionary<string, int>.Empty;
  IEnumerable<int> set = Enumerable.Empty<int>();


  [GlobalSetup]
  public void BenchSetup() {
    items = new int[Count];
    Array.Fill(items, 0x12345678); // Range(1, Count).ToArray();
    set = Enumerable.Range(0, Count);
    d = set.ToDictionary(i => i.ToString(), i => 31 * i % 17);
    fd = set.ToFrozenDictionary(i => i.ToString(), i => 31 * i % 17);
    id = set.ToImmutableDictionary(i => i.ToString(), i => 31 * i % 17);
  }

  [IterationSetup]
  public void ItSetup() {
    Random r = new Random(5214);
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
  public void BuildDictTest() {
    var dict = set.ToFrozenDictionary(i => i.ToString(), i => 31 * i % 17);
  }

  [Benchmark]
  public void BuildFrozenDictTest() {
    var dict = set.ToFrozenDictionary(i => i.ToString(), i => 31 * i % 17);
  }

  [Benchmark]
  public void BuildImmutableDictTest() {
    var dict = set.ToImmutableDictionary(i => i.ToString(), i => 31 * i % 17); ;
  }

}
