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
        public List<DynAstNode> MatchStatements(DynToken endToken)
        {
            var r = new List<DynAstNode>();
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
        public DynAstNode MatchStatement()
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
        ///     operand '*' expression
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
                case DynToken.SymbolStar:
                case DynToken.SymbolEqual:
                    NextLexeme();
                    r.Operation = lexeme;
                    r.Right = MatchExpression();
                    break;
            }
            return r;
        }

        /// <summary>
        /// functionDefine ::= def identifier '(' ')' block
        /// </summary>
        /// <exception cref="DynException"></exception>
        public DynAstNodeFunctionDefine MatchFunctionDefine()
        {
            var r = new DynAstNodeFunctionDefine();

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

            r.Parameters = MatchFunctionParameters();

            var prt = NextLexeme();
            if (prt.Token != DynToken.SymbolPareRight)
            {
                throw new DynException($"语法错误，不是右括弧) {prt}");
            }
            
            r.Block = MatchBlock();
            return r;
        }

        /// <summary>
        /// functionParameters ::=
        ///     ∅
        ///     identifier (,identifier)*
        /// </summary>
        /// <returns></returns>
        public List<DynLexeme> MatchFunctionParameters()
        {
            var lexeme = PeekLexeme();
            var r = new List<DynLexeme>();

            while (lexeme.Token != DynToken.SymbolPareRight)
            {
                if (lexeme.Token != DynToken.Identifier)
                {
                    throw new DynException($"语法错误，不是有效的函数参数定义 {lexeme}");
                }
                lexeme = NextLexeme();
                r.Add(lexeme);
                lexeme = PeekLexeme();
                if (lexeme.Token == DynToken.SymbolComma)
                {
                    NextLexeme();
                    lexeme = PeekLexeme();
                }
            }
            return r;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DynLexeme NextLexeme()
        {
            if (lexemes.Count > 0)
            {
                return lexemes.Pop();
            }
            return lexer.NextLexeme();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public DynLexeme PeekLexeme()
        {
            var r = NextLexeme();
            lexemes.Push(r);
            return r;
        }
    }
}
