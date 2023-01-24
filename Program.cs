
class Program {
  public static void Main() {
    Simple();
  }

  private static void Simple() {
    var source = Enumerable.Range(1, 10);
    var acoll = source.Select(trace).Where(x => x < 5);
    foreach (var item in acoll) {
      Console.WriteLine($"item : {item}");
    }
  }

  void Combined() {
    var source = Enumerable.Range(1, 5);

    try {
      var acoll = source
        .Where(x => x >= 1)
        .Select(i => 100 - i)
        .Where(x => x <= 100)
        .Select(x => 2 * x)
        .Where(x => true)
        .Select(fail);
      acoll.GetEnumerator().MoveNext();
    } catch (Exception e) {
      Console.WriteLine(e.StackTrace);
    }

  }
  static T trace<T>(T val) { Console.WriteLine($"trace : {val}"); return val; }
  static T fail<T>(T val) => throw new Exception();
}