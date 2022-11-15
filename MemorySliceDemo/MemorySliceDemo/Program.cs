

using System.Runtime.InteropServices;

unsafe
{
    var p = stackalloc ExtraStruct[1];
    Span<ExtraStruct> pa = new Span<ExtraStruct>(p, 1);
    IntPtr pb = new IntPtr(p);
    var sb = Marshal.PtrToStructure<ExtraStruct>(pb);

    var ai = new int[123];
    fixed(int* ptr = ai)
    {
        
    }
}


string s = "dfgdfgdgd";
var ss = s[1..3];
var sss = s.AsSpan().Slice(1);

var a = new int[] { 1, 2, 3, 4 };
var aa = a[0..1];

struct ExtraStruct
{
    public int aint;
    public char achar;
    public float afloat;
}