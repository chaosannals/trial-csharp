﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynCode
{
    public class DynAstNodeBlock : IDynAstNode
    {
        public List<IDynAstNode> Statements { get; set; }

        public void Effect(DynMachine machine)
        {
            foreach(var s in Statements)
            {
                s.Effect(machine);
            }
        }

        public string Explain()
        {
            StringBuilder sb = new StringBuilder();
            foreach (IDynAstNode node in Statements)
            {
                sb.Append($"    {node.Explain()}");
            }
            return sb.ToString();
        }
    }
}
