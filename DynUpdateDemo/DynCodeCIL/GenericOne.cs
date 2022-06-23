using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynCodeCIL;

public class GenericOne<T1, T2>
{
    public T1? MyT1 { get; set; } = default(T1);
    private T2? t2 = default(T2);
}
