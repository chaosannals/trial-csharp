using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynCodeCIL;

public class FCOne
{
    private int c;
    public FCOne(int v)
    {
        c = v;
    }

    public int Func(int a, int b)
    {
        return a + b + c;
    }
}
