// Luke Schauble.
// 11510454.

namespace ExpressionTreeDemo
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using SpreadsheetEngine;

    class Program
    {
        private static ExpressionTree expTree;
        static void Main(string[] args)
        {
            expTree = new ExpressionTree("A1+2+A2+4"); //create a new Expression tree with expression
            RunDemo();
        }

        /// <summary>
        /// Name: RunDemo
        /// Description: Runs a Demo of the expression tree
        /// </summary>
        public static void RunDemo()
        {
            string input;
            int quit = 0;
            string newExpression;
            string newVariable;
            string newVariableValue;

            while (quit == 0)
            {
                Console.WriteLine("*WELCOME*", expTree.Expression);
                Console.WriteLine("This Program is a demo of the logic used for calculating expressions in my Spreadsheet application");
                Console.WriteLine("     1. Set variables values, the variable name format is 'Letter, Number' or for example A1, C4, K7");
                Console.WriteLine("     2. nter an expression using the variables created");
                Console.WriteLine("");
                Console.WriteLine("Current Expression = \"{0}\"", expTree.Expression);
                Console.WriteLine("\t1. Enter a new expression");
                Console.WriteLine("\t2. Set a variable value");
                Console.WriteLine("\t3. Evaluate Tree");
                Console.WriteLine("\t4. Quit");
                input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        Console.Clear();
                        Console.WriteLine("Current Expression=\"{0}\"", expTree.Expression);
                        Console.Write("Enter a new expression: ");
                        newExpression = Console.ReadLine();
                        expTree = new ExpressionTree(newExpression);
                        Console.Clear();
                        break;

                    case "2":
                        Console.Clear();
                        Console.WriteLine("Current Expression = \"{0}\"", expTree.Expression);
                        Console.Write("Enter a new Variable: ");
                        newVariable = Console.ReadLine();
                        Console.Write("Enter a new Variable Value: ");
                        newVariableValue = Console.ReadLine();
                        double value = double.Parse(newVariableValue);
                        expTree.SetVariable(newVariable, value);
                        Console.Clear();
                        break;

                    case "3":
                        Console.Clear();
                        Console.WriteLine("The Tree Evaluates to: {0}", expTree.Evaluate());
                        Console.WriteLine("");
                        break;

                    case "4":
                        quit = 1;
                        break;
                }
            }
        }
    }
}
