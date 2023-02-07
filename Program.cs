using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;

BenchmarkSwitcher.FromAssemblies(AppDomain.CurrentDomain.GetAssemblies()).Run(args: args);

[MemoryDiagnoser, DisassemblyDiagnoser]
public class Bench {
  [Params(10_000L,100_000L,1_000_000L)]
  public int Count { get; set; }
  ImmutableArray<int> test = ImmutableArray<int>.Empty;
  int[] items;

  [GlobalSetup]
  public void BenchSetup() {
    items = new int[Count];
    Array.Fill(items, 0x12345678); // Range(1, Count).ToArray();
  }

  [Benchmark]
  // [MethodImpl(MethodImplOptions.NoOptimization)]
  public long BuildListTest() {
    long i=0L;
    foreach (var item in BuildList())
      i+=item;
    return i;
  }
  public ImmutableList<int> BuildList() {
    var b = ImmutableList<int>.Empty.ToBuilder();
    b.AddRange(items);
    return b.ToImmutable();
  }

  [Benchmark]
  // [MethodImpl(MethodImplOptions.NoOptimization)]
  public long AddRangeListTest() {
    long i=0L;
    foreach (var item in AddRangeList())
      i+=item;
    return i;
  }
  public ImmutableList<int> AddRangeList() {
    var b = ImmutableList<int>.Empty;
    b=b.AddRange(items);
    return b;
  }
  [Benchmark]
  // [MethodImpl(MethodImplOptions.NoOptimization)]
  public long AddListTest() {
    long i=0L;
    foreach (var item in AddList())
      i+=item;
    return i;
  }
  public ImmutableList<int> AddList() {
    var b = ImmutableList<int>.Empty;
    foreach (var i in items)
      b=b.Add(i);
    return b;
  }

  [Benchmark]
  // [MethodImpl(MethodImplOptions.NoOptimization)]
 
  public long AddMutableListTest() {
    long i=0L;
    foreach (var item in AddMutableList())
      i+=item;
    return i;
  } public List<int> AddMutableList() {
    var b = new List<int>();
    foreach (var i in items)
      b.Add(i);
    return b;
  }

  // [MethodImpl(MethodImplOptions.NoOptimization)]
  IEnumerable<int> Range(int start, int count){
    while(0<count) {
      yield return start++;
    }
  }
}
