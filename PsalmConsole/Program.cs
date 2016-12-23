using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Psalm;

namespace PsalmConsole
{
    class Program
    {
        static string _program = "CPUSH PUSH 10 GTR IFGOTO 11 PUSH 110 PUSH 1 IFGOTO 13 PUSH 121 CPOP";
        static void Main(string[] args)
        {
            new Interpreter(_program).Execute();
            Console.ReadKey();
        }
    }
}
