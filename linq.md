qa: group vs group by

## Asynchronous Streams

```csharp
async IAsyncEnumerable<int> RangeAsync ( int start, int count, int delay)
{
    for (int i = start; i < start + count; i++)
    {
        await Task.Delay (delay);
        yield return i;
    }
}
// To consume an asynchronous stream, use the await foreach statement:
await foreach (var number in RangeAsync (0, 10, 100))
    Console.WriteLine (number);
```
