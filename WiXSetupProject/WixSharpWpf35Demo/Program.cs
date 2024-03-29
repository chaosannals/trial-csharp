﻿using System;
using WixSharp;

namespace WixSharpWpf35Demo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var project = new Project("MyProduct",
                          new Dir(@"%ProgramFiles%\My Company\My Product",
                             new File("Program.cs")));

            project.GUID = new Guid("6f330b47-2577-43ad-9095-1861ba25889b");

            Compiler.BuildMsi(project);
        }
    }
}
