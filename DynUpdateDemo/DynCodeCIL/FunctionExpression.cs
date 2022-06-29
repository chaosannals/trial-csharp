using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynCodeCIL;

public class FunctionExpression
{
    public delegate R MyFunc<R, T1, T2>(T1 t1, T2 t2);

    public MyFunc<int, string, string> MyFuncOne = (a, b) =>
    {
        return a.Length + b.Length;
    };

    public Func<int, int, int> FOne { get; set; } = (a , b) =>
    {
        return a + b / 100;
    };

    public MyFunc<int, int, int> OneFC { get; set; }

    public FunctionExpression()
    {
        OneFC = (a,b) => new FCOne(123).Func(a,b);
    }

    public object MakeFunction()
    {
        return (int i) =>
        {
            return i + 100;
        };
    }

    public void DoFunction()
    {
        var c = 100;
        var f = (int a, int b) =>
        {
            a += 100;
            return b / a - c;
        };
        f(123, 456);
    }
}
