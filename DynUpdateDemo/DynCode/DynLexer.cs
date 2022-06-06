using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DynCode
{
    public class DynLexer
    {
        const string SymbolsCharSet = "+-*/=,;{}()[]";
        public static readonly Dictionary<string, DynToken> Keywords = new Dictionary<string, DynToken>
        {
            { "def", DynToken.KeywordDef },
            { "ret", DynToken.KeywordRet },
            { "if", DynToken.KeywordIf },
        };
        public static readonly Dictionary<string, DynToken> Symbols = new Dictionary<string, DynToken>
        {
            { "+", DynToken.SymbolPlus },
            { "-", DynToken.SymbolMinus },
            { "*", DynToken.SymbolStar },
            { "=", DynToken.SymbolEqual },
            { ",", DynToken.SymbolComma },
            { ";", DynToken.SymbolSemicolon },
            { "{", DynToken.SymbolCurlyLeft },
            { "}", DynToken.SymbolCurlyRight },
            { "(", DynToken.SymbolPareLeft },
            { ")", DynToken.SymbolPareRight },
        };

        private StreamReader reader;
        private int? lastChar = null;

        public int Row { get; private set; }
        public int Column { get; private set; }

        public DynLexer(Stream input)
        {
            Row = 1;
            Column = 1;
            reader = new StreamReader(input);
        }

        public DynLexeme NextLexeme()
        {
            int c = NextChar();

            while (char.IsWhiteSpace((char)c))
            {
                c = NextChar();
                Column++;
            }

            if (c == '#')
            {
                do
                {
                    c = NextChar();
                    Column++;
                } while (c != -1 || c != '\n' || c != '\r');
                if (c != -1)
                {
                    c = NextChar();
                }
            }

            if (char.IsDigit((char)c))
            {
                StringBuilder sb = new StringBuilder();
                do
                {
                    sb.Append((char)c);
                    c = NextChar();
                } while (char.IsDigit((char)c) || c == '.');
                lastChar = c;
                var v = double.Parse(sb.ToString());
                return new DynLexeme()
                {
                    Token = DynToken.Number,
                    Row = Row,
                    Column = Column,
                    Number = v,
                };
            }

            if (char.IsLetter((char)c))
            {
                StringBuilder sb = new StringBuilder();
                do
                {
                    sb.Append((char)c);
                    c = NextChar();
                } while (char.IsLetterOrDigit((char)c));

                var id = sb.ToString();
                lastChar = c;
                if (Keywords.ContainsKey(id))
                {
                    return new DynLexeme()
                    {
                        Token = Keywords[id],
                        Row = Row,
                        Column = Column,
                        Identifier= id,
                    };
                }
                return new DynLexeme()
                {
                    Token = DynToken.Identifier,
                    Row = Row,
                    Column = Column,
                    Identifier = id,
                };
            }

            if (SymbolsCharSet.IndexOf((char)c) >= 0)
            {
                string sk = $"{(char)c}";
                if (Symbols.ContainsKey(sk))
                {
                    return new DynLexeme()
                    {
                        Token = Symbols[sk],
                        Row = Row,
                        Column = Column,
                        Identifier=sk,
                    };
                }
                throw new DynException($"词法错误，意外的操作符号 [{Row}, {Column}] {sk} （{c}）");
            }

            if (c == -1)
            {
                return new DynLexeme()
                {
                    Token=DynToken.EndOfFile,
                    Row = Row,
                    Column = Column,
                };
            }

            throw new DynException($"词法错误，意外字符 [{Row}, {Column}] {(int)c} （{c}）");
        }

        public int NextChar()
        {
            if (lastChar.HasValue)
            {
                int v = lastChar.Value;
                lastChar = null;
                return v;
            }

            int c = reader.Read();

            if (c == '\n' || c == '\r')
            {
                ++Row;
                Column = 1;
            }
            else
            {
                ++Column;
            }

            return c;
        }
    }
}
