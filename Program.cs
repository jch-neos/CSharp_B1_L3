using System.Collections;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

new Queryable<int>().Select(x=>x).Where(x=>x<100).Count();

public class Queryable<T> : IOrderedQueryable<T>
{
   public Queryable()
   {
      Initialize(new QueryProvider(), null);
   }
 
   public Queryable(IQueryProvider provider)
   {
     Initialize(provider, null);
   }
 
   internal Queryable(IQueryProvider provider, Expression expression)
   {
      Initialize(provider, expression);
   }
 
   private void Initialize(IQueryProvider provider, Expression expression)
   {
      if (provider == null)
         throw new ArgumentNullException("provider");
      if (expression != null && !typeof(IQueryable<T>).
             IsAssignableFrom(expression.Type))
         throw new ArgumentException(
              String.Format("Not assignable from {0}", expression.Type), "expression");
 
      Provider = provider;
      Expression = expression ?? Expression.Constant(this);
   }
 
   public IEnumerator<T> GetEnumerator()
   {
      return (Provider.Execute<IEnumerable<T>>(Expression)).GetEnumerator();
   }
 
   System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
   {
      return (Provider.Execute<System.Collections.IEnumerable>(Expression)).GetEnumerator();
   }
 
   public Type ElementType
   {
      get { return typeof(T); }
   }
 
   public Expression Expression { get; private set; }
   public IQueryProvider Provider { get; private set; }
}

public class QueryProvider : IQueryProvider
{
 
   public QueryProvider()
   {

   }
 
   public virtual IQueryable CreateQuery(Expression expression)
   {
      Type elementType = expression.Type.GetInterfaces().Where(x=>x.GetGenericTypeDefinition()==typeof(IEnumerable<>)).First().GetGenericArguments()[0];
      try
      {
         return               
            (IQueryable)Activator.CreateInstance(typeof(Queryable<>).
                   MakeGenericType(elementType), new object[] { this, expression });
      }
      catch (TargetInvocationException e)
      {
         throw e.InnerException;
      }
   }
 
   public virtual IQueryable<T> CreateQuery<T>(Expression expression)
   {
      return new Queryable<T>(this, expression);
   }
 
   object IQueryProvider.Execute(Expression expression)
   {
    Console.WriteLine(expression);
      return null;
   }
 
   T IQueryProvider.Execute<T>(Expression expression)
   {
    Console.WriteLine(expression);

      return default(T);
   }
}