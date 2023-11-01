# Interfaces

An interface can contain only **methods, properties, events, and indexers**, which not coincidentally are precisely the members of a
class that can be abstract

## Extending an Interface

Interfaces can derive from other interfaces. For instance:
```csharp 
public interface IUndoable { void Undo(); }
public interface IRedoable : IUndoable { void Redo(); }
```

### Implementing Interface Members Virtually

### Default Interface Members
From C# 8, you can add a default implementation to an interface member,
making it optional to implement:
```csharp 
interface ILogger
{
    void Log (string text) => Console.WriteLine (text);
}

class Logger : ILogger { }
...
((ILogger)new Logger()).Log ("message");
```

Default implementations are always explicit, so if a class implementing ILogger fails to define a Log method, the only way to call it is through the interface:

### Static Interface Members
An interface can also declare static members. There are two kinds of static interface members:

- Static nonvirtual interface members
- Static virtual/abstract interface members

Static nonvirtual interface members exist mainly to help with writing default interface members. They are not implemented by classes or structs; instead, they are consumed directly

```csharp 
interface ILogger
{
    void Log (string text) => Console.WriteLine (Prefix + text);
    static string Prefix = "";
}

ILogger.Prefix = "File log: ";

interface ITypeDescribable
{
    static abstract string Description { get; }
    static virtual string Category => null;
}

class CustomerTest : ITypeDescribable
{
    public static string Description => "Customer tests";
    public static string Category => "Unit testing";
}

```
Static virtual/abstract interface members (from C# 11) are marked with static abstract or static virtual and enable static polymorphism, an advanced feature that we will discuss fully in “Static Polymorphism”:

## Flags Enums

```csharp 
[Flags]
public enum BorderSides { None=0, Left=1, Right=2, Top=4, Bottom=8 }

BorderSides leftRight = BorderSides.Left | BorderSides.Right;
if ((leftRight & BorderSides.Left) != 0)
    Console.WriteLine ("Includes Left"); // Includes Left
string formatted = leftRight.ToString(); // "Left, Right"
BorderSides s = BorderSides.Left;
s |= BorderSides.Right;
Console.WriteLine (s == leftRight); // True

[Flags] public enum BorderSides
{
    None=0,
    Left=1, Right=2, Top=4, Bottom=8,
    LeftRight = Left | Right,
    TopBottom = Top | Bottom,
    All = LeftRight | TopBottom
}

```

# Generics

typeof and Unbound Generic Types

```csharp 
class A<T> {}
class A<T1,T2> {}
...
Type a1 = typeof (A<>); // Unbound type
Type a2 = typeof (A<,>); // Indicates 2 type args
Console.Write (a2.GetGenericArguments().Count()); // 2
Type a3 = typeof (A<int,int>);

static void Zap<T> (T[] array)
{
    for (int i = 0; i < array.Length; i++)
    // The default Generic Value
    array[i] = default(T);
}

```
## Generic Constraints

By default, a type parameter can be substituted with any type whatsoever. Constraints can be applied to a type parameter to require more specific type arguments. There are eight kinds of constraints:

```csharp 
where T : base-class // Base class constraint
where T : interface // Interface constraint
where T : class // Reference type constraint
where T : class? // (See "Nullable Reference Types")
where T : struct // Value type constraint
where T : unmanaged // Unmanaged constraint
where T : new() // Parameterless constructor // constraint
where U : T // Naked type constraint
where T : notnull // Non-nullable value type // or non-nullable reference type

static void Initialize<T> (T[] array) where T : new()
{
    for (int i = 0; i < array.Length; i++)
    // The parameterless constructor constraint requires T to have a public parameterless constructor and allows you to call new() on T
    array[i] = new T();
}

```

## Self-Referencing Generic Declarations

```csharp
public interface IEquatable<T> { bool Equals (T obj); }
public class Balloon : IEquatable<Balloon>
{
    public bool Equals (Balloon b) { ... }
}

```

## Static Data

Static data is unique for each closed type:

```csharp
    Console.WriteLine (++Bob<int>.Count); // 1
    Console.WriteLine (++Bob<int>.Count); // 2
    Console.WriteLine (++Bob<string>.Count); // 1
    Console.WriteLine (++Bob<object>.Count); // 1

    class Bob<T> { public static int Count; }
```

## Covariance

Covariance and contravariance are advanced concepts. The motivation behind their introduction into C# was to allow generic interfaces and generics (in particular, those defined in .NET, such as IEnumerable<T>) to work more as you’d expect. You can benefit from this without understanding the details behind covariance and contravariance
specifically with delegates and generic interfaces, allow for more flexibility and type safety when dealing with generic types

- Covariance: Allows a method to return a more derived type than specified by the delegate or interface. In other words, it allows you to use a more derived type than originally specified. It is supported for reference types and is denoted by the out keyword

- Contravariance: Allows a method to accept parameters of a less derived type than specified by the delegate or interface. It permits a less derived type to be used. It is also supported for reference types and is denoted by the in keyword

```csharp
    class Animal { }
    class Mammal : Animal { }
    class Cat : Mammal { }
    // Covariance:
    IEnumerable<Cat> cats = new List<Cat>();
    IEnumerable<Animal> animals = cats;
    // Contravariance:
    Action<Animal> actOnAnimal = (Animal a) => Console.WriteLine(a.GetType().Name);
    Action<Cat> actOnCat = actOnAnimal;
```
When to use:
- Covariance (out keyword): When you only intend to retrieve, or output, items from a data structure and not modify them.

- Contravariance (in keyword): When you only intend to input or store items in a data structure and not retrieve them.

```csharp
    // we created a contravariant interface IProcessor<in T> which allows us to assign an IProcessor<Animal> instance to an IProcessor<Cat> variable.
    interface IProcessor<in T>
    {
        void Process(T item);
    }

    class AnimalProcessor : IProcessor<Animal>
    {
        public void Process(Animal a) => Console.WriteLine($"Processing an {a.GetType().Name}");
    }
    // Using the processor
    var catProcessor = new AnimalProcessor() as IProcessor<Cat>;  // Contravariance allows this
    catProcessor.Process(new Cat());
```

## Writing Plug-In Methods with Delegates

A delegate variable is assigned a method at runtime. This is useful for writing plug-in methods

```csharp
int[] values = { 1, 2, 3 };
Transform (values, Square); // Hook in the Square method
foreach (int i in values)
    Console.Write (i + " "); // 1 4 9

void Transform (int[] values, Transformer t)
{
    for (int i = 0; i < values.Length; i++)
    values[i] = t (values[i]);
}

int Square (int x) => x * x;

delegate int Transformer (int x);
```

Instance and Static Method Targets

A delegate’s target method can be a **local, static, or instance** method.
When an instance method is assigned to a delegate object, the latter must maintain a reference not only to the method but also to the instance to which the method belongs. The System.Delegate class’s Target property represents this instance (and will be null for a delegate referencing a static method).

Delegates are **immutable**, so when you call += or -=, you’re in fact creating a new delegate instance and assigning it to the existing variable

Generic Delegate Types
```csharp
public delegate T Transformer<T> (T arg);
Transformer<double> s = Square;
Console.WriteLine (s (3.3)); // 10.89
double Square (double x) => x * x;

```

Delegate Compatibility
Delegate types are all incompatible with one another, even if their signatures are the same:

```csharp
delegate void D1(); delegate void D2();
D1 d1 = Method1;
D2 d2 = d1; // Compile-time error
// The following, however, is permitted:
D2 d2 = new D2 (d1);
// Delegate instances are considered equal if they have the same type and method target(s). For multicast delegates, the order of the method targets is significant.
```
### Return type variance
When you call a method, you might get back a type that is more specific than what you asked for. This is ordinary polymorphic behavior. In keeping with this, a delegate target method might return a more specific type than described by the delegate. This is covariance:

In C#, the term "variance" refers to how you can use derived types (subtypes) or base types (supertypes) in place of specified types in certain contexts. Variance enables you to make assignments that look counterintuitive at first. The most common context where variance comes into play is with delegates and generic type parameters.

## Type parameter variance for generic delegates
it’s a good practice to do the following:
-   Mark a type parameter used only on the return value as covariant (out)
-   Mark any type parameters used only on parameters as contravariant (in)

## Events
When you’re using delegates, two emergent roles commonly appear: broadcaster and subscriber. The broadcaster is a type that contains a delegate field. The broadcaster decides when to broadcast, by invoking the delegate. The subscribers are the method target recipients. A subscriber decides when to start and stop listening, by calling += and -= on the
broadcaster’s delegate. A subscriber does not know about, or interfere with, other subscribers.

Events are a language feature that formalizes this pattern. An event is a construct that exposes just the subset of delegate features required for the broadcaster/subscriber model. The main purpose of events is to **prevent subscribers from interfering with one another**

```csharp
public class Broadcaster
{
    // put the event keyword in front of a delegate member
    public event ProgressReporter Progress;
}

public delegate void PriceChangedHandler (decimal oldPrice, decimal newPrice);

public class Stock
{
string symbol; decimal price;
public Stock (string symbol) => this.symbol = symbol;
public event PriceChangedHandler PriceChanged;
public decimal Price {get => price; 
    set
        {
        if (price == value) return;
        // Fire event if invocation list isn't empty:
        if (PriceChanged != null)
            PriceChanged (price, value);
        price = value;
        }
    }
}



```
If we remove the event keyword from our example so that PriceChanged becomes an ordinary delegate field, our example would give the same results. However, Stock would be less robust in that subscribers could do the following things to interfere with one another:

-   Replace other subscribers by reassigning PriceChanged (instead of using the += operator)
-   Clear all subscribers (by setting PriceChanged to null)
-   Broadcast to other subscribers by invoking the delegate

Events are a layer on top of delegates. They use delegates behind the scenes but add an additional layer of encapsulation. Specifically, while any code can invoke a delegate directly (assuming they have a reference to it), only the class that declares an event can raise that event.
```csharp
public delegate void SimpleDelegate(string message);

public class MyClass
{
    public event SimpleDelegate MyEvent;

    public void RaiseEvent(string message)
    {
        MyEvent?.Invoke(message);
    }
}

var obj = new MyClass();
obj.MyEvent += (msg) => Console.WriteLine(msg);
obj.RaiseEvent("Hello from event!");
```

If you remove the event keyword and expose a public delegate, you're making that delegate fully accessible. That means any external code can invoke it, clear all its subscriptions using = null, or even combine it with other methods.

```csharp
public class MyClassWithoutEvent
{
    public SimpleDelegate MyDelegate;  // This is not common and not recommended.

    public void RaiseDelegate(string message)
    {
        MyDelegate?.Invoke(message);
    }
}

var obj = new MyClassWithoutEvent();
obj.MyDelegate += (msg) => Console.WriteLine(msg);
obj.MyDelegate("Directly invoking!");  // This is now allowed, but typically undesirable.
```
Key Difference:
- Encapsulation: The primary difference is about encapsulation. An event can only be raised by the class that declares it. This isn't the case for a plain delegate; any code that has access to the delegate can invoke it.

- Safety: Exposing a public delegate can be dangerous because external code can overwrite the delegate, clearing all existing subscriptions. With events, subscribers can only add or remove themselves.

## Lambda Expressions

A lambda expression has the following form:
    (parameters) => expression-or-statement-block

For convenience, you can omit the parentheses if and only if there is exactly one parameter of an inferable type. 
```csharp
    x => x * x;
    x => { return x * x; };
    Func<int,int> sqr = x => x * x;
    Func<int,int> sqr = (int x) => x * x
    obj.Clicked += (sender,args) => Console.Write ("Click");
    // From C# 10, the compiler permits implicit typing with lambda expressions
    var greeter = () => "Hello, world";
    // Default Lambda Parameters (C# 12)
    void Print (string info = "") => Console.Write (info);
    var print = (string info = "") => Console.Write (info);
```

## Capturing Outer Variables
```csharp
    int factor = 2;
    Func<int, int> multiplier = n => n * factor;
    Console.WriteLine (multiplier (3)); // 6
    // Outer variables referenced by a lambda expression are called captured variables. A lambda expression that captures variables is called a closure.
    int factor = 2;
    Func<int, int> multiplier = n => n * factor;
    factor = 10;
    Console.WriteLine (multiplier (3)); // 30

```
## Static lambdas
From C# 9, you can ensure that a lambda expression, local function, or anonymous method doesn’t capture state by applying the static keyword

If we later tried to modify the lambda expression such that it captured a local variable, the compiler will generate an error. This feature is more useful in local methods (because a lambda expression itself incurs a memory allocation).

```csharp
Func<int, int> multiplier = static n => n * 2

void Foo()
{
    int factor = 123;
    static int Multiply (int x) => x * 2;
}
```

## Lambda Expressions Versus Local Methods

The functionality of local methods (see “Local methods”) overlaps with that of lambda expressions. Local methods have the advantages of allowing for recursion and avoiding the clutter of specifying a delegate. Avoiding the indirection of a delegate also makes them slightly more efficient, and they can access local variables of the containing method without the compiler
having to “hoist” the captured variables into a hidden class

However, in many cases you need a delegate, most commonly when calling a higher-order function (i.e., a method with a delegate-typed parameter):
public void Foo (Func<int,bool> predicate) { ... }

```csharp
// A higher-order function that takes a function as an argument and uses it.
public static int Calculate(int x, int y, Func<int, int, int> operation)
    {
        return operation(x, y);
    }

    int sum = Calculate(5, 3, (a, b) => a + b);
    Console.WriteLine(sum);  // Output: 8
        
    int product = Calculate(5, 3, (a, b) => a * b);
    Console.WriteLine(product);  // Output: 15
```

## Anonymous Methods

```csharp
Transformer sqr = delegate (int x) {return x * x;};
Transformer sqr = (int x) => {return x * x;};
Transformer sqr = x => x * x;
```
A unique feature of anonymous methods is that you can omit the parameter declaration entirely—even if the delegate expects it. This can be useful in declaring events with a default empty handler:

```csharp
public event EventHandler Clicked = delegate { };
// This avoids the need for a null check before firing the event. The following is also legal (notice the lack of parameters):
Clicked += delegate { Console.Write ("clicked"); };

```

## try Statements and Exceptions

When an exception is thrown within a try statement, the CLR performs a test:

Does the try statement have any compatible catch blocks?
- If so, execution jumps to the compatible catch block, followed by the finally block (if present), and then execution continues normally.
- If not, execution jumps directly to the finally block (if present), and then the CLR looks up the call stack for other try blocks and, if found, repeats the test.

## The catch Clause
A catch clause specifies what type of exception to catch. This must be either System.Exception or a subclass of System.Exception.
Catching System.Exception catches all possible errors. This is useful when:

- Your program can potentially recover regardless of the specific exception type.
- You plan to rethrow the exception (perhaps after logging it).
- Your error handler is the last resort, prior to termination of the program.

```csharp
try
{
    DoSomething();
}
catch (IndexOutOfRangeException ex) { ... }
catch (FormatException ex) { ... }
catch (OverflowException ex) { ... }

// You can catch an exception without specifying a variable, if you don’t need to access its properties:
catch (OverflowException) // no variable
{ ... }

// Furthermore, you can omit both the variable and the type (meaning that all exceptions will be caught):
catch { ... }
```

### Exception filters
You can specify an exception filter in a catch clause by adding a when
clause:
```csharp
catch (WebException ex) when (ex.Status == WebExceptionStatus.Timeout)
{ ... }
catch (WebException ex) when (ex.Status == something)
{ ... }
catch (WebException ex) when (ex.Status == somethingelse)
{ ... }

// The Boolean expression in the when clause can be side-effecting, such as a method that logs the exception for diagnostic purposes
```

## Throwing Exceptions
Exceptions can be thrown either by the runtime or in user code. In this example, Display throws a System.ArgumentNul lException:

```csharp
if (name == null)
    throw new ArgumentNullException (nameof (name));

//From C# 7, throw can appear as an expression in expression-bodied functions:
public string Foo() => throw new NotImplementedException();
// A throw expression can also appear in a ternary conditional expression:
string ProperCase (string value) => value == null ? throw new ArgumentException ("value") : value == "" ? "" : char.ToUpper (value[0]) + value.Substring (1);

// Rethrowing an exception
try { ... }
catch (Exception ex)
{
// Log error
...
throw; // Rethrow same exception
}
// Rethrowing in this manner lets you log an error without swallowing it. It also lets you back out of handling an exception should circumstances turn out to be outside what you expected.
try
{
... // parse a date of birth from XML element data
}
catch (FormatException ex)
{
throw new XmlException ("Invalid date of birth", ex);
}

```

**If we replaced throw with throw ex, the example would still work, but the StackTrace property of the exception would no longer reflect the original error.**

### Key Properties of System.Exception

The most important properties of System.Exception are the following:
- StackTrace : A string representing all the methods that are called from the origin of the exception to the catch block.
- Message : A string with a description of the error.
- InnerException: The inner exception (if any) that caused the outer exception. This, itself, might have another InnerException.
