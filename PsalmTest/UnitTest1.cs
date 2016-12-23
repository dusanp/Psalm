using Psalm;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace PsalmTest
{
    [TestClass]
    public class TokenizerTest
    {
        private static readonly Dictionary<string, Token[]> _testCases = new Dictionary<string, Token[]>
        {
            
        };

        [TestMethod]
        public void TokenizeTest()
        {
            foreach (var testCase in _testCases)
            {
                CollectionAssert.AreEqual(Tokenizer.Tokenize(testCase.Key).ToArray(), testCase.Value);
            }
            
           
        }
    }
}
