# Null Operators

- the null coalescing operator, ??
- the null-conditional operator, ?.
- the null-coalescing assignment operator ??=

```csharp
// If the lefthand expression is non-null, the righthand expression is never evaluated
string s1 = null;
string s2 = s1 ?? "nothing"; // s2 evaluates to "nothing"

//The ??= operator (introduced in C# 8) is the null-coalescing assignment operator. It says, “If the operand to the left is null, assign the right operand to the left operand.”
myVariable ??= someDefault;
//This is equivalent to
if (myVariable == null) myVariable = someDefault;

//Null-Conditional Operator . It allows you to call methods and access members just like the standard dot operator, except that if the operand on the left is null, the expression evaluates to null instead of throwing a NullReferenceException
System.Text.StringBuilder sb = null;
string s = sb?.ToString(); // No error; s is null

//The last line is equivalent to this:
string s = (sb == null ? null : sb.ToString());

string[] words = null;
string word = words?[1]; // word is null

x?.y?.z
// equivalent
x == null ? null
        : (x.y == null ? null : x.y.z) 
```

C# has no “elseif” keyword; however, the following pattern achieves the same result:

```csharp
if (age >= 35)
    Console.WriteLine ("You can be president!");
else if (age >= 21)
    Console.WriteLine ("You can drink!");
else if (age >= 18)
    Console.WriteLine ("You can vote!");
else
    Console.WriteLine ("You can wait!");
```

## Switching on types
```csharp
switch (x)
{
    case int i:
        Console.WriteLine ("It's an int!");
        break;
    case string s:
        Console.WriteLine (s.Length); // We can use s
        break;
    case bool b when b == true: // Fires when b is true
        Console.WriteLine ("True");
        break;
    case null: // You can also switch on null
        Console.WriteLine ("null");
        break;
}

switch (x)
{
    case float f when f > 1000:
    case double d when d > 1000:
    case decimal m when m > 1000:
        Console.WriteLine ("f, d and m are out of scope");
        break;
}

string cardName = cardNumber switch
{
    13 => "King",
    12 => "Queen",
    11 => "Jack",
    _ => "Pip card" // equivalent to 'default'
};

// Notice that the switch keyword appears after the variable name and that the case clauses are expressions (terminated by commas) rather than statements. You can also switch on multiple values (tuples):

int cardNumber = 12; string suite = "spades";
string cardName = (cardNumber, suite) switch
{
    (13, "spades") => "King of spades",
    (13, "clubs") => "King of clubs",
    ...
};

//  infinite loop
for (;;) Console.WriteLine ("interrupt me");
```

## The goto statement

```csharp
    int i = 1;
    startLoop:
if (i <= 5)
{
    Console.Write (i + " "); // 1 2 3 4 5
    i++;
    goto startLoop;
}
```

## using static

The using static directive imports a type rather than a namespace. All static members of that type can then be used without being qualified with the type name 
```csharp
using static System.Console;
WriteLine ("Hello");
```

## Aliasing Types and Namespaces

Importing a namespace can result in type-name collision. Rather than importing the whole namespace, you can import just the specific types you need, giving each type an alias. For example:

```csharp
using PropertyInfo2 = System.Reflection.PropertyInfo;
class Program { PropertyInfo2 p; }

// An entire namespace can be aliased, as follows:
using R = System.Reflection;
class Program { R.PropertyInfo p; }
```

## Alias any type (C# 12)
From C# 12, the using directive can alias any kind of type, including, for instance, arrays:

```csharp 12
using NumberList = double[];
NumberList numbers = { 2.5, 3.5 };
```

## deconstructing

```csharp 12
// mix and match existing and new variables when deconstructing
double x1 = 0;
(x1, double y2) = rect;

```

## Properties

```csharp 
public class Stock
{
    decimal currentPrice; // The private "backing" field
    public decimal CurrentPrice // The public property
    {
        get { return currentPrice; }
        set { currentPrice = value; }
    }

    // typically has a dedicated backing field.  However, it doesn’t need to; it can instead return a value computed from other data
    decimal currentPrice, sharesOwned;
    public decimal Worth
    {
    get { return currentPrice * sharesOwned; }
    }   

    // Expression-bodied properties
    public decimal Worth => currentPrice * sharesOwned;
    
    //From C# 7, set accessors can be expression-bodied too:
    public decimal Worth
    {
        get => currentPrice * sharesOwned;
        set => sharesOwned = value / currentPrice;
    }

    // Property initializers
    public decimal CurrentPrice { get; set; } = 123;
    public int Maximum { get; } = 999;

    private decimal x;
    public decimal X
    {
        get { return x; }
        // The typical use case for this is to have a public property with an internal or private access modifier on the setter:

        private set { x = Math.Round (value, 2); }
    }

    // Init-only setters
    // These init-only properties act like read-only properties, except that they can also be set via an object initializer
    public class Note
    {
        public int Pitch { get; init; } = 20;
        public int Duration { get; init; } = 100;
    }

    var note = new Note { Pitch = 50 };

    //The alternative to init-only properties is to have read-only properties that you populate via a constructor:
    public Note (int pitch = 20, int duration = 100)
    {
        Pitch = pitch; Duration = duration;
    }

    // Init-only properties have another significant advantage, which is that they allow for nondestructive mutation when used in conjunction with records (see “Records”)

    public class Point
    {
        readonly int _x;
        public int X { get => _x; init => _x = value; }
        ...

    }

    // Notice that the _x field is read-only: init-only setters are permitted to modify readonly fields in their own class. (Without this feature, _x would need to be writable, and the class would fail at being internally immutable.)


```

## Primary Constructors (C# 12)

```csharp 12
// mix and match existing and new variables when deconstructing
class Person (string firstName, string lastName)
{
    public void Print() =>
    Console.WriteLine (firstName + " " + lastName);
}

// This instructs the compiler to automatically build a primary constructor using the primary constructor parameters (firstName and lastName) so that we can instantiate our class as follows:
Person p = new Person ("Alice", "Jones");
p.Print(); // Alice Jones

// The constructor that C# builds is called primary because any additional constructors that you choose to (explicitly) write must invoke it: 
class Person (string firstName, string lastName)
{
    public Person (string first, string last, int age) : this (first, last) // Must call primary constructor
    { ... }
}
```

Primary constructors are best suited to simple scenarios due to the following limitations

You cannot add extra initialization code to a primary constructor.
Although it’s easy to expose a primary constructor parameter as a public property, you cannot easily incorporate validation logic unless the property is read-only

## Static Constructors

A static constructor executes once per type, rather than once per instance. A type can define only one static constructor, and it must be parameterless and have the same name as the type:

```csharp 
class Test
{
    static Test() { Console.Write ("Type Initialized"); }
}
```

From C# 9, you can also define module initializers, which execute once per assembly (when the assembly is first loaded). To define a module initializer, write a static void method and then apply
the [ModuleInitializer] attribute to that method:

```csharp 
[System.Runtime.CompilerServices.ModuleInitializer]
internal static void InitAssembly()
{
...
}
```

## Finalizers

Finalizers are class-only methods that execute before the garbage collector reclaims the memory for an unreferenced object. The syntax for a finalizer is the name of the class prefixed with the **~** symbol:

```csharp 
class Class1
{
    ~Class1() { ... }
}

```

## Required members (C# 11)

```csharp 
public class Asset
{
    public required string Name;
}

Asset a1 = new Asset { Name="House" }; // OK
Asset a2 = new Asset(); // Error

public Asset() { }
[System.Diagnostics.CodeAnalysis.SetsRequiredMembers]
public Asset (string n) => Name = n;

```

When an object is instantiated, initialization takes place in the following order:
-   1. From subclass to base class:
        - a. Fields are initialized.
        - b. Arguments to base class constructor calls are evaluated.
-   2. From base class to subclass:
        - c. Constructor bodies execute.

## The GetType Method and typeof Operator

All types in C# are represented at runtime with an instance of System.Type. There are two basic ways to get a System.Type object: call GetType on the instance or use the typeof operator on a type name. GetType is evaluated at runtime; typeof is evaluated statically at compile time 
```csharp 
int x = 3;
Console.Write (x.GetType().Name); // Int32
Console.Write (typeof(int).Name); // Int32
Console.Write (x.GetType().FullName); // System.Int32
Console.Write (x.GetType() == typeof(int)); // True

```

## Structs

A struct is similar to a class, with the following key differences:
    -   A struct is a value type, whereas a class is a reference type.
    -   A struct does not support inheritance (other than implicitly deriving from object, or more precisely, System.ValueType).

A struct can have all the members that a class can, except for a finalizer, and virtual or protected members.

Prior to C# 10, structs in C# had limitations in terms of constructors and field initializers

    -   No Parameterless Constructors : Structs could not define a parameterless constructor
    -   No Field Initializers: Field initializers in structs were not allowed

```csharp 
public struct Point
{
    public int X; // This is fine
    public int Y = 5; // This would result in a compile-time error before C# 10
}
```
```csharp 10
public struct Point
{
    public int X { get; set; } // auto-property
    public int Y; // field

    public Point()
    {
        X = 0; // Need to initialize all fields in parameterless constructor
        Y = 0;
    }

    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }
}
```

## readonly Structs and Functions 

apply the readonly modifier to a struct to enforce that all fields are readonly; this aids in declaring intent as well as allowing the compiler more optimization freedom:

```csharp 
readonly struct Point
{
    public readonly int X, Y; // X and Y must be readonly
}

// If you need to apply readonly at a more granular level, you can apply the readonly modifier (from C# 8) to a struct’s functions. This ensures that if the function attempts to modify any field, a compile-time error is generated:
struct Point
{
    public int X, Y;
    public readonly void ResetX() => X = 0; // Error!
}

// If a readonly function calls a non-readonly function, the compiler generates a warning (and defensively copies the struct to avoid the possibility of a mutation)
```

## Access Modifiers

-   public
-   internal : Accessible only within the containing assembly or friend assemblies. This is the default accessibility for non-nested types
-   private  : Accessible only within the containing type
-   private protected : The intersection of protected and internal accessibility (this is more restrictive than protected or internal alone).
-   protected internal: The union of protected and internal accessibility (this is more permissive than protected or internal alone in that it makes a member more accessible in two ways)
-   file (from C# 11) : Accessible only from within the same file. Intended for use by source generators (see “Extended partial methods”). This modifier can be applied only to type declarations
