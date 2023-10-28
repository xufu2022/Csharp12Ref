# Value Types Versus Reference Types

Value types comprise most built-in types (specifically, **all numeric types, the char type, and the bool type**) as well as custom struct and enum types.
Reference types comprise all class, array, delegate, and interface types.
The fundamental difference between value types and reference types is how they are handled in memory

Value Types: Cannot be assigned to null by default. You must use the nullable version to assign them to null.

Reference Types: Can be assigned to null, indicating they don't point to any memory location.

Integral conversions are **implicit** when the destination type can represent **every** possible value of the source type

## The checked and unchecked operators

The checked operator instructs the runtime to generate an OverflowException rather than overflowing silently when an integral typed expression or statement exceeds the arithmetic limits of that type. The checked operator affects expressions with the **++, −−, (unary) −, +, −, *, /**, and explicit conversion operators between integral types. Overflow checking incurs a small performance cost

```csharp

int a = 1000000, b = 1000000;
int c = checked (a * b); // Checks just the expression
checked // Checks all expressions
{ // in statement block
    c = a * b;
    ...
}

```

To avoid space inefficiency in the case of arrays, .NET provides a **BitArray** class in the System .Collections namespace that is designed to use just one bit per Boolean value

```csharp

// Initialize BitArray with a size and default value
            BitArray bitArray = new BitArray(5, false);

            // Set some values
            bitArray[0] = true;
            bitArray[2] = true;
            bitArray[4] = true;

            // Display the values in the BitArray
            for (int i = 0; i < bitArray.Length; i++)
            {
                Console.WriteLine($"Index {i}: {bitArray[i]}");
            }
// Output:
            // Index 0: True
            // Index 1: False
            // Index 2: True
            // Index 3: False
            // Index 4: True

```

## Strings and Characters

C#’s char type (aliasing the System.Char type) represents a Unicode character and occupies two bytes (UTF-16). A char literal is specified
inside single quotes:
```csharp
    char c = 'A'; // Simple character
```

Escape sequences express characters that cannot be expressed or interpreted literally. An escape sequence is a backslash followed by a character with a special meaning. For example:
```csharp
    char newLine = '\n';
    char backSlash = '\\';
```

| Char         | Meaning     | Value |
|--------------|-----------|------------|
| \'  | Single quote      |  0x0027     |
|\"   | Double quote      |  0x0022     |
|\\   | Backslash         |  0x005C     |
|\0   | Null              |  0x0000     |
|\a   | Alert             |  0x0007     |
|\b   | Backspace         |  0x0008     |
|\f   | Form feed         |  0x000C     |
|\n   | New line          |  0x000A     |
|\r   | Carriage return   |  0x000D     |
|\t   | Horizontal tab    |  0x0009     |
|\v   | Vertical tab      |  0x000B     |

The \a escape sequence represents the "alert" or "bell" character. When used, it can cause a beep sound (depending on the system and settings).

```csharp
    Console.Write("\a");  // This will produce a beep sound
```

### Raw string literals (C# 11)

Multiline raw string literals are subject to special rules. We can represent the string "Line 1\r\nLine 2" as follows:

""";

```csharp
    string raw = """<file path="c:\temp\test.txt"></file>""";
    string multiLineRaw = """
    Line 1
    Line 2
    """;

```

Notice that the opening and closing quotes must be on separate lines to the string content. Additionally:
-   Whitespace following the opening """ (on the same line) is ignored.
-   Whitespace preceding the closing """ (on the same line) is treated as common indentation and is removed from every line in the string. This lets you include indentation for source-code readability (as we did in our example) without that indentation becoming part of the string.

Raw string literals can be interpolated, subject to special rules described in “String interpolation”

```csharp 10
    const string greeting = "Hello";
    const string message = $"{greeting}, world";
```

```csharp 11
    string s = $"this interpolation spans {1 +    1} lines";
    string s = $"""The date and time is {DateTime.Now}""";
```

Using two (or more) $ characters in a raw string literal prefix changes the interpolation sequence from one brace to two (or more) braces. Consider the following string:
```csharp 11
    $$"""{ "TimeStamp": "{{DateTime.Now}}" }"""
    //This evaluates to:
    { "TimeStamp": "01/01/2024 12:13:25 PM" }
```

## String comparisons

string does not support < and > operators for comparisons. You must instead use string’s CompareTo method

```csharp
Console.Write ("Boston".CompareTo ("Austin")); // 1
Console.Write ("Boston".CompareTo ("Boston")); // 0
Console.Write ("Boston".CompareTo ("Chicago")); // -1

```

### UTF-8 Strings
From C# 11, you can use the **u8 suffix** to create string literals encoded in UTF-8 rather than UTF-16. This feature is intended for advanced scenarios such as the low-level handling of JSON text in performance hotspots:

```csharp 11
    ReadOnlySpan<byte> utf8 = "ab→cd"u8;
    Console.WriteLine (utf8.Length); // 7
```

## Arrays

An array represents a fixed number of elements of a particular type. The elements in an array are always stored in a contiguous block of memory, providing highly efficient access.

```csharp
Console.Write ("Boston".CompareTo ("Austin")); // 1
Console.Write ("Boston".CompareTo ("Boston")); // 0
Console.Write ("Boston".CompareTo ("Chicago")); // -1

for (int i = 0; i < vowels.Length; i++)
Console.Write (vowels [i]); // aeiou

foreach (char c in vowels) Console.Write (c); // aeiou
vowels[5] = 'y'; // Runtime error
```

Arrays also implement IEnumerable<T> (see “Enumeration and Iterators”), so you can also enumerate members with the foreach
statement:

All array indexing is bounds-checked by the runtime. An IndexOutOfRangeException is thrown if you use an invalid index:

```csharp 12
    char[] vowels = ['a','e','i','o','u']; // use square brackets instead of curly braces:
    //This is called a collection expression and has the advantage of also working when calling methods:
    Foo (['a','e','i','o','u']);
    void Foo (char[] letters) { ... }
```

- Dynamically create an array (CreateInstance)
- Get and set elements regardless of the array type (GetValue/SetValue)
- Search a sorted array (BinarySearch) or an unsorted array (IndexOf, LastIndexOf, Find, FindIndex, FindLastIndex)
- Sort an array (Sort)
- Copy an array (Copy)

```csharp
// 1. Dynamically create an array of integers with length 5
            Array dynamicArray = Array.CreateInstance(typeof(int), 5);
// 2. Set values using SetValue
            for (int i = 0; i < 5; i++)
            {
                dynamicArray.SetValue(i + 1, i);
            }
// 3. Search in array
            int searchValue = 3;
            int index = Array.IndexOf(dynamicArray, searchValue);
            Console.WriteLine($"IndexOf {searchValue}: {index}");

// For demonstration, let's use a sorted array for BinarySearch
            Array.Sort(dynamicArray);
            index = Array.BinarySearch(dynamicArray, searchValue);
            Console.WriteLine($"BinarySearch for {searchValue}: {index}");            

            int conditionValue = 4;
            int foundValue = Array.Find((int[])dynamicArray, x => x > conditionValue);
            Console.WriteLine($"Find first value greater than {conditionValue}: {foundValue}");

// 4. Sort the array in descending order
            Array.Sort(dynamicArray, (x, y) => ((int)y).CompareTo((int)x));
            Console.WriteLine("Sorted Array in Descending Order:");
            DisplayArray(dynamicArray);

// 5. Copy array
            Array copiedArray = Array.CreateInstance(typeof(int), 5);
            Array.Copy(dynamicArray, copiedArray, dynamicArray.Length);
            Console.WriteLine("Copied Array:");
            DisplayArray(copiedArray);

        static void DisplayArray(Array array)
            {
                for (int i = 0; i < array.Length; i++)
                {
                    Console.Write(array.GetValue(i) + " ");
                }
                Console.WriteLine();
            }

```

### Indices and Ranges

```csharp 8
    char[] vowels = new char[] {'a','e','i','o','u'};
    char lastElement = vowels[^1]; // 'u'
    char secondToLast = vowels[^2]; // 'o'
// (^0 equals the length of the array, so vowels[^0] generates an error.)

    Index first = 0;
    Index last = ^1;
    char firstElement = vowels [first]; // 'a'
    char lastElement = vowels [last]; // 'u'

// Ranges
// Ranges let you “slice” an array with the .. operator:

    char[] firstTwo = vowels [..2]; // 'a', 'e'
    char[] lastThree = vowels [2..]; // 'i', 'o', 'u'
    char[] middleOne = vowels [2..3]; // 'i'
    char[] lastTwo = vowels [^2..^0]; // 'o', 'u
```

### Jagged arrays

```csharp
    int[][] matrix = new int[3][];

    for (int i = 0; i < matrix.Length; i++)
    {
        matrix[i] = new int [3]; // Create inner array
        for (int j = 0; j < matrix[i].Length; j++)
        matrix[i][j] = i * 3 + j;
    }

    int[][] matrix = new int[][]
    {
        new int[] {0,1,2},
        new int[] {3,4,5},
        new int[] {6,7,8,9}
    };

```

Simplified Array Initialization Expressions

```csharp

    char[] vowels = new char[] {'a','e','i','o','u'};
    char[] vowels = {'a','e','i','o','u'};
    char[] vowels = ['a','e','i','o','u'];

```