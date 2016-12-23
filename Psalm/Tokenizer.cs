using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psalm
{
    public static class Tokenizer
    {
        private static byte _useless = 0;
        private static readonly string[] _functions = new string[] 
        {
            "PUSH",
            "IFGOTO",
            "STORE",
            "LOAD",

            "PUSHX",
            "IFGOTOX",
            "STOREX",
            "LOADX",
        };
        private static readonly string[] _commands = new string[] 
        {
            "ADD",
            "SUBTRACT",
            "MULTIPLY",
            "DIVIDE",
            "AND",
            "OR",
            "XOR",
            "NOT",
            "GTR",
            "LSS",
            "POP",
            "CPOP",
            "CPUSH",
        };
        private static readonly Dictionary<TokenType, Func<string, bool>> _grammar =
            new Dictionary<TokenType, Func<string, bool>>
        {
            { TokenType.Function, (s)=>_functions.Contains(s) },
            { TokenType.Command, (s)=>_commands.Contains(s) },
            { TokenType.Value, (s)=>byte.TryParse(s, out _useless) },
        };

        public static IEnumerable<Token> Tokenize(string input)
        {
            var words = input.Split(new[] { " ", Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            foreach(var word in words)
            {
                Token result;
                try
                {
                    result = new Token
                    {
                        Value = word,
                        Type = _grammar.Single(g => g.Value(word)).Key,
                    };
                }
                catch (InvalidOperationException){ throw new Exception("Word is not a valid token"); }
                yield return result;
            }
        }
    }
}
