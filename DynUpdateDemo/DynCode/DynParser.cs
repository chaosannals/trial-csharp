using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace DynCode
{
    public class DynParser
    {
        private DynLexer lexer;
        private Stack<DynLexeme> lexemes;

        public DynParser(DynLexer lexer)
        {
            this.lexer = lexer;
            lexemes = new Stack<DynLexeme>();
        }

        public DynAstNodeRoot Parse()
        {
            return new DynAstNodeRoot()
            {
                Statements = MatchStatements(DynToken.EndOfFile),
            };
        }

        /// <summary>
        /// statemens ::= statement+
        /// </summary>
        public List<IDynAstNode> MatchStatements(DynToken endToken)
        {
            var r = new List<IDynAstNode>();
            var lexeme = PeekLexeme();
            while (lexeme.Token != endToken)
            {
                r.Add(MatchStatement());
                lexeme = PeekLexeme();
            }
            return r;
        }

        /// <summary>
        /// statement ::=
        ///     expression ';'
        ///     functionDefine
        /// </summary>
        public IDynAstNode MatchStatement()
        {
            var lexeme = PeekLexeme();

            if (lexeme.Token == DynToken.KeywordDef)
            {
                return MatchFunctionDefine();
            }

            var r = new DynAstNodeStatement();

            if (lexeme.Token == DynToken.KeywordRet)
            {
                NextLexeme();
                r.IsReturn = true;
            }

            r.Expression = MatchExpression();

            var end = NextLexeme();
            if (end.Token != DynToken.SymbolSemicolon)
            {
                throw new DynException($"语法错误，不是有效的语句结束分号; {end}");
            }
            return r;
        }

        /// <summary>
        /// block ::= '{' statements '}'
        /// </summary>
        /// <exception cref="DynException"></exception>
        public DynAstNodeBlock MatchBlock()
        {
            var clt = NextLexeme();
            if (clt.Token != DynToken.SymbolCurlyLeft)
            {
                throw new DynException($"语法错误，不是左花括号{{ {clt}");
            }

            var ss = MatchStatements(DynToken.SymbolCurlyRight);

            var crt = NextLexeme();
            if (crt.Token != DynToken.SymbolCurlyRight)
            {
                throw new DynException($"语法错误，不是右花括号}} {crt}");
            }
            return new DynAstNodeBlock() { Statements=ss };
        }

        /// <summary>
        /// operand ::=
        ///     identifier
        ///     number
        /// </summary>
        public DynAstNodeOperand MatchOperand()
        {
            var lexeme = NextLexeme();
            if (lexeme.Token == DynToken.Number)
            {
                return new DynAstNodeOperand()
                {
                    Lexeme = lexeme,
                };
            }

            if (lexeme.Token == DynToken.Identifier)
            {
                return new DynAstNodeOperand()
                {
                    Lexeme= lexeme,
                };
            }

            throw new DynException($"语法错误，不是有效的操作数 {lexeme}");
        }

        /// <summary>
        /// expression ::=
        ///     operand '=' expression
        ///     operand '+' expression
        ///     operand '-' expression
        ///     operand
        /// </summary>
        /// <exception cref="DynException"></exception>
        public DynAstNodeExpression MatchExpression()
        {
            var r = new DynAstNodeExpression();
            r.Left = MatchOperand();

            var lexeme = PeekLexeme();
            switch (lexeme.Token)
            {
                case DynToken.SymbolPlus:
                case DynToken.SymbolMinus:
                case DynToken.SymbolEqual:
                    NextLexeme();
                    r.Operation = lexeme.Token;
                    r.Right = MatchExpression();
                    break;
            }
            return r;
        }

        /// <summary>
        /// functionDefine ::= def identifier '(' ')' block
        /// </summary>
        /// <exception cref="DynException"></exception>
        public DynAstNodeFunction MatchFunctionDefine()
        {
            var r = new DynAstNodeFunction();

            var defLexeme = NextLexeme();
            if (defLexeme.Token != DynToken.KeywordDef)
            {
                throw new DynException($"语法错误，不是有效的函数定义 {defLexeme}");
            }

            var idLexeme = NextLexeme();
            if (idLexeme.Token != DynToken.Identifier)
            {
                throw new DynException($"语法错误，不是有效的函数标识符 {idLexeme}");
            }
            r.Name = idLexeme.Identifier;

            var plt = NextLexeme();
            if (plt.Token != DynToken.SymbolPareLeft)
            {
                throw new DynException($"语法错误，不是左括弧( {plt}");
            }

            var prt = NextLexeme();
            if (prt.Token != DynToken.SymbolPareRight)
            {
                throw new DynException($"语法错误，不是右括弧) {prt}");
            }
            
            r.Block = MatchBlock();
            return r;
        }

        public DynLexeme NextLexeme()
        {
            if (lexemes.Count > 0)
            {
                return lexemes.Pop();
            }
            return lexer.NextLexeme();
        }

        public DynLexeme PeekLexeme()
        {
            var r = NextLexeme();
            lexemes.Push(r);
            return r;
        }
    }
}
