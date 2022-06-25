namespace DynCodeCIL;

public class SampleOne
{
    public void GODo1()
    {
        GenericsOne<int> a = new GenericsOne<int>();
        a.DoSome(5678);

        GenericsOne<SampleOne> b = new GenericsOne<SampleOne>();
        b.DoSome(new SampleOne());
    }

    public double IntAddDouble()
    {
        int a = 123;
        double b = 123.3455;
        return b + a;
    }

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