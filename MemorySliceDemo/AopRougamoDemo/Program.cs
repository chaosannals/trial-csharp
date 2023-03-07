using AopRougamoDemo;

Console.WriteLine("Start!");

TargetFirst.SyncFunc(44);
await TargetFirst.AsyncFunc(44);
TargetFirst.SyncNoTryFunc(44);
var r = TargetFirst.ThrowPublicExceptionFunc();
Console.WriteLine($"ThrowPublicExceptionFunc return: {r}");

try
{
    TargetFirst.ThrowExceptionFunc(123);
    TargetFirst.ThrowExceptionFunc("eeee");
} catch
{
}

var ri = TargetFirst.DoReturnInt();
Console.WriteLine($"DoReturnInt result; {ri}");

TargetSecond.SyncPublicFunc(4);

try
{
    TargetSecond.ThrowPublicExceptionFunc();
}
catch { }

var third = new TargetThird();
third.DoSync("aaaaa");
await third.DoAsync("bbbbb");
third.NoByInterface();
third.DoSome();

Console.WriteLine("End!");

Console.ReadKey();
