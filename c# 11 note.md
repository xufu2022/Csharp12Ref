# c# 11 new features

## Generic attributes
Generic attributes in C# 11 allow developers to define and use attributes that can take generic type parameters. This feature enhances the expressiveness and flexibility of attribute usage in C#.

> Explanation
Prior to C# 11, attributes in C# could not be generic. With C# 11, you can define an attribute class with generic type parameters, allowing for more dynamic and type-safe attribute scenarios.

```cs
//Imagine a scenario where you need to configure serialization behavior for different classes, and you want to specify a serializer type through an attribute. Here's how you could use a generic attribute for this purpose:

// Define a generic attribute for specifying a serializer
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class SerializerAttribute<TSerializer> : Attribute
    where TSerializer : ISerializer
{
}

public interface ISerializer
{
    string Serialize(object obj);
}

public class JsonSerializer : ISerializer
{
    public string Serialize(object obj)
    {
        // Implement JSON serialization logic here
        return "Serialized in JSON";
    }
}

// Apply the attribute to a class
[SerializerAttribute<JsonSerializer>]
public class MyClass
{
}

public class Program
{
    public static void Main()
    {
        // Retrieving the attribute
        var attribute = Attribute.GetCustomAttribute(typeof(MyClass), typeof(SerializerAttribute<JsonSerializer>)) as SerializerAttribute<JsonSerializer>;
        if (attribute != null)
        {
            var serializer = Activator.CreateInstance<JsonSerializer>();
            var serializedData = serializer.Serialize(new MyClass());
            Console.WriteLine(serializedData);
        }
    }
}

// 
Generic attributes can be particularly useful in scenarios where you want to annotate classes, methods, or other members with metadata that includes generic type information. This allows for more flexible and type-safe attribute usage.

Example Scenario: Configuring Serialization
Imagine a scenario where you need to configure serialization behavior for different classes, and you want to specify a serializer type through an attribute. Here's how you could use a generic attribute for this purpose:

csharp
Copy code
// Define a generic attribute for specifying a serializer
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class SerializerAttribute<TSerializer> : Attribute
    where TSerializer : ISerializer
{
}

public interface ISerializer
{
    string Serialize(object obj);
}

public class JsonSerializer : ISerializer
{
    public string Serialize(object obj)
    {
        // Implement JSON serialization logic here
        return "Serialized in JSON";
    }
}

// Apply the attribute to a class
[SerializerAttribute<JsonSerializer>]
public class MyClass
{
}

public class Program
{
    public static void Main()
    {
        // Retrieving the attribute
        var attribute = Attribute.GetCustomAttribute(typeof(MyClass), typeof(SerializerAttribute<JsonSerializer>)) as SerializerAttribute<JsonSerializer>;
        if (attribute != null)
        {
            var serializer = Activator.CreateInstance<JsonSerializer>();
            var serializedData = serializer.Serialize(new MyClass());
            Console.WriteLine(serializedData);
        }
    }
}
// In this example, the SerializerAttribute<TSerializer> is a generic attribute that specifies what kind of serializer should be used for a class. When a class like MyClass is decorated with [SerializerAttribute<JsonSerializer>], it indicates that JsonSerializer should be used to serialize instances of MyClass


```
## File-local types

FileLocalClass is a file-local type, only accessible within the same file. PublicClass uses FileLocalClass internally, demonstrating encapsulation. The xUnit test verifies the functionality of PublicClass. This feature helps in keeping the internal implementation details of a class hidden from other parts of the application. Remember to include necessary using directives for xUnit in your test project.

**more details**

This means that the type cannot be used or accessed from any other file in the same project or assembly. This feature helps to encapsulate types that are meant to be implementation details of a specific file, rather than being part of the broader API or accessible throughout the entire project.

Here's a breakdown of how file-local types work:
- Declaration: You declare a file-local type using the file modifier before the class or struct keyword.
- Scope: The scope of the file-local type is limited to the file in which it is declared. It cannot be accessed or used outside of this file.
- Encapsulation: This feature is valuable for encapsulating types that are only relevant to the implementation details within a single file. It helps keep the internal workings of a class or method private and unexposed to the rest of the application.
- Use Cases: File-local types are particularly useful in large projects where you want to prevent types from being used outside of their intended context. It helps in maintaining clean code architecture by reducing unnecessary exposure of internal types.
- Compilation: During compilation, file-local types are treated as internal to the file, ensuring that they don't conflict with types of the same name in other files.

```cs
// This type is only accessible within this file
file class FileLocalClass
{
    public string GetMessage() => "Hello from a file-local class!";
}
```

## Extended nameof scope

Extended nameof scope in C# 11 allows the nameof expression to reference more than just accessible symbols. Previously, nameof was limited to symbols that were accessible in the current scope. With this update, nameof can reference any symbol that can be named in the current context, even if it's not directly accessible.