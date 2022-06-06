using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynCodeMSIL
{
    internal class SampleOne
    {
        public string Text { get; set; }
        public int IntValue { get; set; }
        public int intField;

        public SampleOne()
        {
            Text = String.Empty;
            IntValue = 0;
            intField = 1;
        }

        public SampleOne(string text, int intValue, int intField)
        {
            Text=text;
            IntValue=intValue;
            this.intField = intField;
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public int Mult(int a, int b)
        {
            int c = 12324;
            int d = a / b;
            return a + b + c * d;
        }

        public SampleOne Mult(SampleOne a, SampleOne b)
        {
            return a + b;
        }

        public SampleOne Mult(SampleOne other)
        {
            return this + other;
        }

        public void TheType(object a)
        {
            if (a is int)
            {
                Console.WriteLine(a.GetType().FullName);
            }
            if (a is string)
            {
                Console.WriteLine(a.GetType().FullName);
            }
            if (a is double)
            {
                Console.WriteLine(a.GetType().FullName);
            }
        }

        public static SampleOne operator+(SampleOne a, SampleOne b)
        {
            return new SampleOne(a.Text + b.Text, a.IntValue + b.IntValue, a.intField + b.intField);
        }

        public string TextFormat()
        {
            return $"aaaa{Text} ++++ {IntValue}";
        }
    }
}
