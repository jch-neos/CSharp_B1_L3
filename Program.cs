using Humanizer;

var numbers = Enumerable.Range(1, 20)
    .Select(x => trace(x, "before"))
    .Select(x => new { id = x, name = x.ToWords() })
    .Select(x => trace(x, "after"));

Separator("Ordered enumerable defined");
Wait("Press a key to enumerate");

foreach (var n in numbers)
{
    Console.WriteLine(n);
}

Separator("Ordered enumerable enumerated");
Wait();

var dict = numbers
    .ToDictionary(x => x.id);

Separator("Dictionary defined");
Wait("Press a key to enumerate dictionary");

foreach (var n in dict)
{
    Console.WriteLine(n);
}
Separator("Dictionary enumerated");
Wait();
var sort = numbers
    .OrderByDescending(x => x.id);

Separator("Ordered enumerable defined");
Wait("Press a key to enumerate ordered sequence");

foreach (var n in sort)
{
    Console.WriteLine(n);
}


T trace<T>(T val, string? prefix = null)
{
    if (prefix is not null) prefix += " ";
    Console.WriteLine(prefix + val);
    return val;
}

static void Wait(string prompt="Press a key to continue") => Separator(prompt, true);
static void Separator(string prompt, bool wait=false)
{
    if(wait) {
        Console.Write($"- {prompt} -");
        Console.ReadKey();
        Console.CursorLeft = 0;
    } else {
        Console.WriteLine(new String('=', 100));
        Console.WriteLine($"{prompt}");
        Console.WriteLine(new String('=', 100));
    }
}