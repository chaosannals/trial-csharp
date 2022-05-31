using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using DynCode;

namespace DynCodeDemo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Scripts");
                string entry = Path.Combine(dir, "1.dc.txt");
                DynInterpreter interpreter = new DynInterpreter();
                using (var stream = File.OpenRead(entry))
                {
                    DynLexer lexer = new DynLexer(stream);
                    DynParser parser = new DynParser(lexer);
                    var tree = parser.Parse();
                    Console.WriteLine(tree.Explain());
                    interpreter.Interpret();
                    //DynLexeme Lexeme;
                    //do
                    //{
                    //    Lexeme = lexer.NextLexeme();
                    //    Console.WriteLine("l: {0}", Lexeme);
                    //} while (Lexeme.Token != DynToken.EndOfFile);
                }
            }
            catch (DynException e)
            {
                Console.WriteLine("异常: {0} {1}", e.Message, e.StackTrace);
            }
            Console.ReadKey();
        }
    }
}
