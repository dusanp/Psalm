using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Psalm
{
    public class Interpreter
    {
        private Token[] _program;
        private Stack<byte> _stack;
        private Dictionary<byte, byte> _heap;
        private byte _programPointer;

        private Dictionary<string, Action> _commands;

        private Dictionary<string, Action<byte>> _functions;


        public Interpreter(IEnumerable<Token> tokens)
        {
            this._program = tokens.ToArray();
            _commands = new Dictionary<string, Action>
            {
                { "ADD", ()=>_stack.Push((byte)(_stack.Pop() + _stack.Pop())) },
                { "SUBTRACT", ()=>_stack.Push((byte)(_stack.Pop() - _stack.Pop())) },
                { "MULTIPLY", ()=>_stack.Push((byte)(_stack.Pop() * _stack.Pop())) },
                { "DIVIDE", ()=>_stack.Push((byte)(_stack.Pop() / _stack.Pop())) },

                { "AND", ()=>_stack.Push((byte)(_stack.Pop()!=0 & _stack.Pop()!=0 ? 1:0)) },
                { "OR", ()=>_stack.Push((byte)(_stack.Pop()!=0 | _stack.Pop()!=0 ? 1:0)) },
                { "XOR", ()=>_stack.Push((byte)(_stack.Pop()!=0 ^ _stack.Pop()!=0 ? 1:0)) },
                { "NOT", ()=>_stack.Push((byte)(_stack.Pop()==0?1:0)) },

                { "GTR", ()=>_stack.Push((byte)(_stack.Pop()>_stack.Pop() ? 1:0)) },
                { "LSS", ()=>_stack.Push((byte)(_stack.Pop()<_stack.Pop() ? 1:0)) },

                { "POP", ()=>Console.Write(_stack.Pop()) },
                { "CPOP", ()=>Console.Write((char)_stack.Pop()) },
                { "CPUSH", ()=> 
                    {
                        _stack.Push(byte.Parse(Console.ReadLine())); //todo add checks
                    }
                },

            };
            _functions = new Dictionary<string, Action<byte>>
            {
                { "PUSH", b=>_stack.Push(b) },
                { "IFGOTO", b=>_programPointer=_stack.Pop()!=0?b:_programPointer },
                { "STORE", b=>_heap[b]=_stack.Pop() },
                { "LOAD", b=>_stack.Push(_heap[b]) },

                { "PUSHX", b=>_stack.Push(_heap[b]) },
                { "IFGOTOX", b=>_programPointer=_stack.Pop()!=0?_heap[b]:_programPointer },
                { "STOREX", b=>_heap[_heap[b]]=_stack.Pop() },
                { "LOADX", b=>_stack.Push(_heap[_heap[b]]) },
            };
        }

        public Interpreter(string program) : this(Tokenizer.Tokenize(program)) { }

        public void Execute()
        {
            _stack = new Stack<byte>();
            _heap = new Dictionary<byte, byte>();
            _programPointer = 0;

            Token currentToken;
            while (_programPointer < _program.Length)
            {
                currentToken = _program[_programPointer];
                switch (currentToken.Type)
                {
                    case TokenType.Command:
                        _commands[currentToken.Value]();
                        break;
                    case TokenType.Function:
                        _programPointer++;
                        if (_programPointer>= _program.Length)
                        {
                            throw new Exception("Expected Value type, encountered EOF instead.");
                        }
                        Token argument = _program[_programPointer];
                        if (argument.Type != TokenType.Value)
                        {
                            throw new Exception("Expected Value type, encountered "+argument.Type.ToString()+" instead.");
                        }
                        _functions[currentToken.Value](byte.Parse(argument.Value));
                        break;
                    case TokenType.Value:
                        throw new Exception("Expected Function or Command type, encountered Value instead.");
                }
                _programPointer++;//todo solve gotos
            }

        }

    }
}
