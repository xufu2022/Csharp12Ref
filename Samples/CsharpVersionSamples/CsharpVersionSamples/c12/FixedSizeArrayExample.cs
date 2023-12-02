namespace CsharpVersionSamples.c12;

using System.Runtime.CompilerServices;

[System.Runtime.CompilerServices.InlineArray(10)]
public struct Buffer10<T>
{
    private T _element0;
}

public class Buffer10Usage
{
    public Buffer10<int> CreateBuffer10()
    {
        var buffer = new Buffer10<int>();
        // Normally, you would use the buffer here as needed
        return buffer;
    }
}

[System.Runtime.CompilerServices.InlineArray(10)]
public struct Buffer
{
    private int _element0;
}