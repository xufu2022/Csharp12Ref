# Enumeration

An enumerator is a read-only, forward-only cursor over a sequence of values.
C# treats a type as an enumerator if it does any of the following:
- Has a public parameterless method named MoveNext and property called Current
- Implements System.Collections.Generic.IEnumerator<T>
- Implements System.Collections.IEnumerator

The foreach statement iterates over an enumerable object. An enumerable object is the logical representation of a sequence. It is not itself a cursor but an object that produces cursors over itself. C# treats a type as enumerable if it does any of the following

- Has a public parameterless method named GetEnumerator that returns an enumerator
- Implements System.Collections.Generic.IEnumerable<T>
- Implements System.Collections.IEnumerable
- (From C# 9) Can bind to an extension method named GetEnumerator that returns an enumerator (see “Extension Methods”)

```csharp

class Enumerator // Typically implements IEnumerator<T>
{
    public IteratorVariableType Current { get {...} }
    public bool MoveNext() {...}
}
class Enumerable // Typically implements IEnumerable<T>
{
    public Enumerator GetEnumerator() {...}
}
// high-level way to iterate through
foreach (char c in "beer") Console.WriteLine (c);
// low-level way to iterate through
using (var enumerator = "beer".GetEnumerator())
while (enumerator.MoveNext())
{
    var element = enumerator.Current;
    Console.WriteLine (element);
}
// If the enumerator implements IDisposable, the foreach statement also acts as a using statement, implicitly disposing the enumerator object.
```

## Collection Initializers and Collection Expressions

```csharp
List<int> list = new List<int> {1, 2, 3};
// From C# 12, you can shorten the last line further with a collection expression (note the square brackets):
List<int> list = [1, 2, 3];
// Collection expressions are target-typed, meaning that the type of [1,2,3] depends on the type to which it’s assigned (in this case, List<int>). In the following example, the target type is an array:
int[] array = [1, 2, 3];
// Target typing means that you can omit the type in other scenarios where the compiler can infer it, such as when calling methods:
Foo ([1, 2, 3]);
void Foo (List<int> numbers) { ... }
// The compiler translates this into the following:
List<int> list = new List<int>();
list.Add (1); list.Add (2); list.Add (3);
//  initialize dictionaries (types that implement System.Collections.IDictionary)
var dict = new Dictionary<int, string>()
{
    { 5, "five" },
    { 10, "ten" }
};
var dict = new Dictionary<int, string>()
{
    [5] = "five",
    [10] = "ten"
};
//The latter is valid not only with dictionaries but with any type for which an indexer exists.

```

## Iterators
Whereas a foreach statement is a consumer of an enumerator, an iterator is a producer of an enumerator. In this example, we use an iterator to return a sequence of Fibonacci numbers (for which each number is the sum of the previous two):
```csharp
foreach (int fib in Fibs (6))
    Console.Write (fib + " ");

IEnumerable<int> Fibs (int fibCount)
{
    for (int i=0, prevFib=1, curFib=1; i<fibCount; i++)
    {
        yield return prevFib; // a yield return statement expresses, “Here’s the next element you asked me to yield from this enumerator.
        int newFib = prevFib+curFib;
        prevFib = curFib;
        curFib = newFib;
    }
}
// OUTPUT: 1 1 2 3 5 8
// On each yield statement, control is returned to the caller

foreach (var number in numbers)
{
    if (number > 10)
    {
        yield break;  // No more numbers will be returned after this point.
    }

    if (number % 2 == 0)
    {
        yield return number;
    }
}

// Multiple yield statements
IEnumerable<string> Foo()
{
    yield return "One";
    yield return "Two";
    yield return "Three";
}
```

In C#, the yield keyword is used in the context of an iterator block to produce a sequence of values that can be iterated over with foreach or GetEnumerator() calls. The yield keyword must be followed by either return, to return a value or an object as part of the sequence, or break, to end the sequence generation.

# Null

Nullable<T> Struct
```csharp
public struct Nullable<T> where T : struct
{
    public T Value {get;}
    public bool HasValue {get;}
    public T GetValueOrDefault();
    public T GetValueOrDefault (T defaultValue);
...
}

int? i = null;
Console.WriteLine (i == null);
//translates to:
Nullable<int> i = new Nullable<int>();
Console.WriteLine (! i.HasValue);

// Nullable Conversions
// The conversion from T to T? is implicit, while from T? to T the conversion is explicit. For example:
int? x = 5; // implicit
int y = (int)x; // explicit

// The explicit cast is directly equivalent to calling the nullable object’s Value property. Hence, an InvalidOperationException is thrown if HasValue is false
```
## Boxing/Unboxing Nullable Values
When T? is boxed, the boxed value on the heap contains T, not T?. This optimization is possible because a boxed value is a reference type that can already express null.
C# also permits the unboxing of nullable types with the as operator. The result will be null if the cast fails:

```csharp
object o = "string";
int? x = o as int?;
Console.WriteLine (x.HasValue); // False

// The Nullable<T> struct does not define operators such as <, >, or even ==. Despite this, the following code compiles and executes correctly:

// Operator Lifting
int? x = 5;
int? y = 10;
bool b = x < y; // true

//explaination:
bool b = (x.HasValue && y.HasValue) ? (x.Value < y.Value) : false;
```
above works because the compiler borrows or “lifts” the less-than operator from the underlying value type. Semantically, it translates the preceding comparison expression into this:
Operator lifting means that you can implicitly use T’s operators on T?. You can define operators for T? in order to provide special-purpose null behavior, but in the vast majority of cases, it’s best to rely on the compiler automatically applying systematic nullable logic for you.

```csharp

// Equality operators (==, !=)
Console.WriteLine ( null == null); // True
Console.WriteLine ((bool?)null == (bool?)null); // True
```
- If exactly one operand is null, the operands are unequal.
- If both operands are non-null, their Values are compared.

### bool? with & and | Operators
When supplied operands of type bool?, the & and | operators treat null as an unknown value. So, null | true is true, because:
- If the unknown value is false, the result would be true.
- If the unknown value is true, the result would be true.
Similarly, null & false is false. This behavior should be familiar to SQL users. The following example enumerates other combinations:
```csharp
// bool? with & and | Operators
bool? n = null, f = false, t = true;
Console.WriteLine (n | n); // (null)
Console.WriteLine (n | f); // (null)
Console.WriteLine (n | t); // True
Console.WriteLine (n & n); // (null)
Console.WriteLine (n & f); // False
Console.WriteLine (n & t); // (null)
```
### Nullable Reference Types

```csharp
<Nullable>enable</Nullable>

#nullable enable // enables NRT from this point on
#nullable disable // disables NRT from this point on
#nullable restore // resets NRT to project setting

void Foo (string? s) => Console.Write (s.Length);
// To remove the warning, you can use the null-forgiving operator (!):
void Foo (string? s) => Console.Write (s!.Length);

void Foo (string? s)
{
    if (s != null) Console.Write (s.Length);
}
```
```csharp
```
```csharp
```
```csharp
```
