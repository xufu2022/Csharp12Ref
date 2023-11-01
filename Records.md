# Records

A record (from C# 9) is a special kind of class or struct that’s designed to work well with immutable (read-only) data. Its most useful feature is allowing nondestructive mutation, whereby to “modify” an immutable object, you create a new one and copy over the data while incorporating your modifications

Records are also useful in creating types that just combine or hold data. In simple cases, they eliminate boilerplate code while honoring structural equality semantics (two objects are the same if their data is the same), which is usually what you want with immutable types.

A record is purely a C# compile-time construct. At runtime, the CLR sees them just as classes or structs (with a bunch of extra “synthesized” members added by the compiler).

## Defining a Record

A record definition is like a class or struct definition and can contain the same kinds of members, including fields, properties, methods, and so on. Records can implement interfaces, and (class-based) records can subclass other (class based) records.

```csharp
record Point { } // Point is a class
// From C# 10, the underlying type of a record can also be a struct:
record struct Point { } // Point is a struct
// (record class is also legal and has the same meaning as record.)

record Point
{
    public Point (double x, double y) => (X, Y) = (x, y);
    public double X { get; init; }
    public double Y { get; init; }
}

// preceding record declaration expands into something like this:
class Point
{
public Point (double x, double y) => (X, Y) = (x, y);
public double X { get; init; }
public double Y { get; init; }
protected Point (Point original) // “Copy constructor”
{
    this.X = original.X; this.Y = original.Y
}
// This method has a strange compiler-generated name:
public virtual Point <Clone>$() => new Point (this);
// Additional code to override Equals, ==, !=, GetHashCode, ToString()...

// Parameter lists
record Point (double X, double Y)
{
...
}

```
Parameters can include the in and params modifiers but **not out or ref**.
If a parameter list is specified, the compiler performs the following extra steps:
It writes an init-only property per parameter (or a **writable property**, in the case of record structs).
It writes a primary constructor to populate the properties.
It writes a deconstructor.

```csharp
record Point3D (double X, double Y, double Z)
: Point (X, Y);

class Point3D : Point
{
public double Z { get; init; }
public Point3D (double X, double Y, double Z)
: base (X, Y)
=> this.Z = Z;
}

```
## Nondestructive Mutation
The most important step that the compiler performs with all records is to write a copy constructor (and a hidden Clone method). This enables nondestructive mutation via the with keyword:

```csharp
Point p1 = new Point (3, 3);
Point p2 = p1 with { Y = 4 };
Console.WriteLine (p2); // Point { X = 3, Y = 4 }
record Point (double X, double Y);
```
Nondestructive mutation occurs in two phases:
- First, the copy constructor clones the record. By default, it copies each of the record’s underlying fields, creating a faithful replica while bypassing (the overhead of) any logic in the init accessors. All fields are included (public and private, as well as the hidden fields that back automatic properties)
- 2. Then, each property in the member initializer list is updated (this time using the init accessors).

```csharp
// The compiler translates the following:
Test t2 = t1 with { A = 10, C = 30 };
// into something functionally equivalent to this:
Test t2 = new Test(t1); // Clone t1
t2.A = 10; // Update property A
t2.C = 30; // Update property C 

// (The same code would not compile if you wrote it explicitly because A and C are init-only properties. Furthermore, the copy constructor is protected; C# works around this by invoking it via a public hidden method that it writes into the record called <Clone>$.)

// If necessary, you can define your own copy constructor. C# will then use your definition instead of writing one itself:
protected Point (Point original)
{
this.X = original.X; this.Y = original.Y;
}

// When subclassing another record, the copy constructor is responsible for copying only its own fields. To copy the base record’s fields, delegate to the base:
protected Point (Point original) : base (original)
{
...
}
```

## Primary Constructors
When you define a record with a parameter list, the compiler generates property declarations automatically, as well as a primary constructor (and a deconstructor). This works well in simple cases, and in more complex cases you can omit the parameter list and write the property declarations and constructor manually. C# also offers the mildly useful intermediate option of defining a parameter list while writing some or all of the property declarations yourself:

```csharp
record Student(int ID, string Surname, string FirstName)
{
    public int ID { get; } = ID;
}

// In this case, we “took over” the ID property definition, defining it as read-only (instead of init-only), preventing it from partaking in nondestructive mutation. If you never need to nondestructively mutate a particular property, making it read-only lets you cache computed data in the record without having to code up a refresh mechanism.

// making changes to a data structure or object without altering the original instance. This is typically contrasted with "destructive mutation," where you modify the original data directly.
//Notice that we needed to include a property initializer (in boldface):
public int ID { get; } = ID;
// Note that the ID; in boldface refers to the primary constructor parameter, not the ID property

// You can also take over a property definition with explicit accessors:
int _id = ID;
public int ID { get => _id; init => _id = value; }
// Again, the ID in boldface refers to the primary constructor parameter, not the property. (The reason for there not being an ambiguity is that it’s illegal to access properties from initializers.)

```

## Records and Equality Comparison

Unlike with classes and structs, you do not (and cannot) override the object.Equals method if you want to customize equality behavior. 
Instead, you define a public Equals method with the following signature:

```csharp
record Point (double X, double Y)
{
    public virtual bool Equals (Point other) =>other != null && X == other.X && Y == other.Y;
}
// The Equals method must be virtual (not override), and it must be strongly typed such that it accepts the actual record type (Point in this case, not object). Once you get the signature right, the compiler will automatically patch in your method.
```

## Patterns
Patterns are supported in the following contexts:
- After the is operator (variable is pattern)
- In switch statements
- In switch expressions
```csharp
// var Pattern
bool IsJanetOrJohn (string name) => name.ToUpper() is var upper && (upper == "JANET" || upper == "JOHN");
//equivalent to:
bool IsJanetOrJohn (string name)
{
    string upper = name.ToUpper();
    return upper == "JANET" || upper == "JOHN";
}

// Constant Pattern
// The constant pattern lets you match directly to a constant and is useful when working with the object type:
void Foo (object obj)
{
    if (obj is 3) ...
}
//This expression in boldface is equivalent to the following:
obj is int && (int)obj == 3

// Relational Patterns
// From C# 9, you can use the <, >, <=, and >= operators in patterns:
if (x is > 100) Console.Write ("x is greater than 100");
string GetWeightCategory (decimal bmi) => bmi switch
{
    < 18.5m => "underweight",
    < 25m => "normal",
    < 30m => "overweight",
    _ => "obese"
};

// Pattern Combinators
// From C# 9, you can use the and, or, and not keywords to combine patterns:

bool IsJanetOrJohn (string name) => name.ToUpper() is "JANET" or "JOHN";
bool IsVowel (char c) => c is 'a' or 'e' or 'i' or 'o' or 'u';
bool Between1And9 (int n) => n is >= 1 and <= 9;
bool IsLetter (char c) => c is >= 'a' and <= 'z' or >= 'A' and <= 'Z';

// As with the && and || operators, and has higher precedence than or. You can override this with parentheses. A nice trick is to combine the not combinator with the type pattern to test whether an object is (not) a type:
if (obj is not string) ...
//This looks nicer than:
if (!(obj is string)) ...

// Tuple and Positional Patterns
// The tuple pattern (introduced in C# 8) matches tuples:
var p = (2, 3);
Console.WriteLine (p is (2, 3)); // True

//The tuple pattern can be considered a special case of the positional pattern (C# 8+), which matches any type that exposes a Deconstruct method (see “Deconstructors”). In the following example, we leverage the Point record’s compiler-generated deconstructor:

var p = new Point (2, 2);
Console.WriteLine (p is (2, 2)); // True
record Point (int X, int Y);

Console.WriteLine (p is (var x, var y) && x == y);

// Here’s a switch expression that combines a type pattern with a positional pattern
string Print (object obj) => obj switch
{
Point (0, 0) => "Empty point",
Point (var x, var y) when x == y => "Diagonal"
...
};

// Property Patterns
// A property pattern (C# 8+) matches on one or more of an object’s property values:
if (obj is string { Length:4 }) ...
//However, this doesn’t save much over the following:
if (obj is string s && s.Length == 4) ...

//With switch statements and expressions, property patterns are more useful. Consider the System.Uri class, which represents a URI. It has properties that include Scheme, Host, Port, and IsLoopback. In writing a firewall, we could decide whether to allow or block a URI by employing a switch expression that uses property patterns:

bool ShouldAllow (Uri uri) => uri switch
{
    { Scheme: "http", Port: 80 } => true,
    { Scheme: "https", Port: 443 } => true,
    { Scheme: "ftp", Port: 21 } => true,
    { IsLoopback: true } => true,
    _ => false
};
// You can nest properties, making the following clause legal: { Scheme: { Length: 4 }, Port: 80 } => true,

// which, from C# 10, can be simplified to:
{ Scheme.Length: 4, Port: 80 } => true,

// You can use other patterns inside property patterns, including the relational pattern:
{ Host: { Length: < 1000 }, Port: > 0 } => true,

// You can introduce a variable at the end of a clause and then consume that variable in a when clause:
{ Scheme: "http", Port: 80 } httpUri
when httpUri.Host.Length < 1000 => true,

// You can also introduce variables at the property level:
{ Scheme: "http", Port: 80, Host: var host }
when host.Length < 1000 => true,

// In this case, however, the following is shorter and simpler:
{ Scheme: "http", Port: 80, Host: { Length: < 1000 } }

// List Patterns
// List patterns (from C# 11) work with any collection type that is countable (with a Count or Length property) and indexable (with an indexer of type int or System.Index)

int[] numbers = { 0, 1, 2, 3, 4 };
Console.Write (numbers is [0, 1, 2, 3, 4]); // True

// An underscore matches a single element of any value:
Console.Write (numbers is [0, 1, _, _, 4]); // True

// The var pattern also works in matching a single element:
Console.Write (numbers is [0, 1, var x, 3, 4] && x > 1);
// Two dots indicate a slice. A slice matches zero or more elements:
Console.Write (numbers is [0, .., 4]); // True

// With arrays and other types that support indices and ranges (see “Indices and Ranges”), you can follow a slice with a var pattern:
Console.Write (numbers is [0, .. var mid, 4] && mid.Contains (2)); // True
// A list pattern can include at most one slice.
```

