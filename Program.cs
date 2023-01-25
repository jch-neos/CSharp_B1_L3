class Test {
  static void Main() {
    Test1();
  }
  static void Test1() {
    int i = 1;
    var fun1 = () => i++;
    i = 1;
    var fun2 = () => i;
    Console.WriteLine(fun1()); /* 1 */
    Console.WriteLine(fun2()); /* 2 */
    Console.WriteLine(fun1()); /* 2 */
    Console.WriteLine(fun2()); /* 3 */
    Console.WriteLine(fun1()); /* 3 */
    Console.WriteLine("================");

  }
  static void Test2(){
    Test2Internal();
    GC.Collect();
    GC.WaitForPendingFinalizers();
  }

  static void Test2Internal() {
    Func<int> funa1 = GetFun();

    GC.Collect();
    GC.WaitForPendingFinalizers();

    Console.WriteLine(funa1()); /* 1 */
    Console.WriteLine(funa1()); /* 2 */
    Console.WriteLine(funa1()); /* 3 */
  }
  static Func<int> GetFun() {
    var a = new A();
    var funa1 = a.Test();
    return funa1;
  }
}


class A {
  int j = 1;
  public Func<int> Test() => () => j++;

  ~A() {
    Console.WriteLine("================");
    Console.WriteLine("A finalized");
    Console.WriteLine("================");
  }

}