
class Program {
  public static void Main() {
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

    T fail<T>(T val) =>throw new Exception();
  }
}