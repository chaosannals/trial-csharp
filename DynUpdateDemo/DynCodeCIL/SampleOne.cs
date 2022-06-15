namespace DynCodeCIL;

public class SampleOne
{
    public async Task DoSomeAsync()
    {
        var a = 123;
        var b = a  + 334;
        Console.WriteLine(b);
        await Task.CompletedTask;
    }

    public async void DoSomeVoidAsync()
    {
        var a = 123;
        var b = a  + 334;
        Console.WriteLine(b);
        await Task.CompletedTask;
    }

    public async Task<int> DoSomeIntAsync()
    {
        var a = 123;
        var b = a  + 334;
        Console.WriteLine(b);
        await Task.CompletedTask;
        return b;
    }
}