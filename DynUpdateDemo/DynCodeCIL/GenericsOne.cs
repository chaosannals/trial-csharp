using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynCodeCIL;

public class GenericsOne<T>
{
    public T V1 { get; set; }

    public T V2 { get; set; }

    public void DoSome(T one)
    {
        V1 = one;
    }

    public void LetV1V2()
    {
        V1 = V2;
    }
}
