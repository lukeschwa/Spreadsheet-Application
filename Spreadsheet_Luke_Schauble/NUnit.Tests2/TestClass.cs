
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using SpreadsheetEngine;

namespace NUnit.Tests2
{
    [TestFixture]
    public class TestClass
    {
        /// <summary>
        /// Tests adding of numbers
        /// </summary>
        [Test]
        public void AddTest()
        {
            double answer = 8;
            ExpressionTree test = new ExpressionTree("5+3");
            Assert.AreEqual(test.Evaluate(), answer);
        }

        /// <summary>
        /// Tests adding of numbers
        /// </summary>
        [Test]
        public void SubtractTest()
        {
            double answer = 2;
            ExpressionTree test = new ExpressionTree("5-3");
            Assert.AreEqual(test.Evaluate(), answer);
        }

        /// <summary>
        /// Tests adding of numbers
        /// </summary>
        [Test]
        public void MultiplyTest()
        {
            double answer = 15;
            ExpressionTree test = new ExpressionTree("5*3");
            Assert.AreEqual(test.Evaluate(), answer);
        }

        /// <summary>
        /// Tests adding of numbers
        /// </summary>
        [Test]
        public void DivideTest()
        {
            double answer = 2;
            ExpressionTree test = new ExpressionTree("5-3");
            Assert.AreEqual(test.Evaluate(), answer);
        }
    }
}
