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
```