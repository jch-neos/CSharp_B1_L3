using System.Collections;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;


new MyQueryable<int>().Select(x => x).Where(x => x < 100).Count();
// var q = from t in new MyQueryable<int>()
//         join u in new MyQueryable<Wrapper<int>>() on t equals u.Value
//         into UGroup
//         let i = t
//         let j = UGroup.Sum(x => x.Value)
//         let k = 2 * i
//         select i + j + k;
// q.Sum();

// record class Wrapper<T>(T Value);
// IEnumerable<string> listStr = "a new world".Split(), filtered, upper;
// filtered = from str in listStr
//            where str.Length > 1
//            select str;
// upper = from str in filtered
//         select str.ToUpper();

// filtered = listStr.Where(x => x.Length > 1);
// upper = filtered.Select(x => x.ToUpper());

// (from str in new MyQueryable<string>()
//  from word in str.Split(' ', StringSplitOptions.None)
//  select word).ToList();

// from person in people
// join pet in pets on person equals pet.Owner
// into personPets
// select new {
//     OwnerName = person.FirstName,
//     Pets = personPets
// };

public class MyQueryable<T> : IOrderedQueryable<T> {
  public MyQueryable() : this(new QueryProvider(), null) { }

  public MyQueryable(IQueryProvider provider) : this(provider, null) { }

  internal MyQueryable(IQueryProvider provider, Expression? expression) {
    if (provider == null)
      throw new ArgumentNullException("provider");
    if (expression != null && !typeof(IQueryable<T>).IsAssignableFrom(expression.Type))
      throw new ArgumentException(String.Format("Not assignable from {0}", expression.Type), "expression");

    Provider = provider;
    Expression = expression ?? Expression.Constant(this);
  }

  public IEnumerator<T> GetEnumerator() =>
   (Provider.Execute<IEnumerable<T>>(Expression)).GetEnumerator();


  System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    => (Provider.Execute<System.Collections.IEnumerable>(Expression)).GetEnumerator();


  public Type ElementType { get => typeof(T); }

  public Expression Expression { get; private set; }
  public IQueryProvider Provider { get; private set; }
}

public class QueryProvider : IQueryProvider {

  public QueryProvider() {

  }

  public virtual IQueryable CreateQuery(Expression expression) {
    Type elementType = expression.Type.GetInterfaces().Where(x => x.GetGenericTypeDefinition() == typeof(IEnumerable<>)).First().GetGenericArguments()[0];
    try {
      return
         (IQueryable)Activator.CreateInstance(typeof(MyQueryable<>).
                MakeGenericType(elementType), new object[] { this, expression })!;
    } catch (TargetInvocationException e) {
      throw e.InnerException!;
    }
  }

  public virtual IQueryable<T> CreateQuery<T>(Expression expression) {
    return new MyQueryable<T>(this, expression);
  }

  object IQueryProvider.Execute(Expression expression) {
    Console.WriteLine(format(expression));
    return MkEnum(expression.Type);
  }

  T IQueryProvider.Execute<T>(Expression expression) {
    Console.WriteLine(format(expression));
    return MkEnum<T>();

  }

  string format(Expression expression) {
    var text = expression.ToString();
    text = text
        .Replace(").", ")\n\t.")
        .Replace("<>f__AnonymousType", "_anon")
        .Replace("<>h__TransparentIdentifier", "_ident")
        ;
    return text;
  }

  #region piping
  private T MkEnum<T>() => (T)MkEnum(typeof(T));
  private object? MkEnum(Type type) {
    var enumType = getEnumType(type);
    if (enumType != null)
      return typeof(Enumerable).GetMethod("Empty")!.MakeGenericMethod(new Type[] { getEnumType(type) }).Invoke(null, new object[0]);
    if (type.IsValueType)
      return Activator.CreateInstance(type);
    return null;
  }

  Type getEnumType<T>() => getEnumType(typeof(T));
  Type getEnumType(Type src) {
    Type t;
    if (IsEnum(src)) t = src;
    else t = src.GetInterfaces().FirstOrDefault(IsEnum);
    return t?.GetGenericArguments()?.First();

    static bool IsEnum(Type x) {
      return x.IsInterface && x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEnumerable<>);
    }
  }
  #endregion
}