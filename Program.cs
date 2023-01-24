using System.Globalization;
using System.Numerics;

var enumerable = getFib().Take(100);

var arr = enumerable.ToArray();
var list = enumerable.ToList();

var fib10 = arr[10];
fib10 = list[10];

list.Add(list[9] + list[10]);

char[] separators = " ,\r\n.-:[#]*_—();“”!?‘’&£\"/%'$".ToCharArray();
var text = File.ReadAllText("1661-0.txt");

var words = text.Split(separators, StringSplitOptions.RemoveEmptyEntries);
var lookup = words.Distinct(StringComparer.CurrentCultureIgnoreCase).ToLookup(x => x.Length);
var dict = words.GroupBy(x => x, StringComparer.CurrentCultureIgnoreCase).ToDictionary(x => x.Key, x => x.Count(), StringComparer.CurrentCultureIgnoreCase);

foreach (var word6 in lookup[6]) {
  Console.WriteLine(word6);
}

foreach (var name in new string[] { "Sherlock", "Holmes", "Watson", "murder" }) {
  Console.WriteLine($"{name} appears {dict.GetValueOrDefault(name)} times");
}



IEnumerable<BigInteger> getFib() {
  yield return BigInteger.One;
  var (a, b) = (BigInteger.One, BigInteger.One);
  BigInteger t = a + b;
  while (true) {
    (a, b) = (b, t);
    yield return a;
    t = a + b;
  }
}
