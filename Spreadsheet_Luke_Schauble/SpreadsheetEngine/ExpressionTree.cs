// Luke Schauble.
// 11510454.

namespace SpreadsheetEngine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class ExpressionTree
    {
        private static readonly Dictionary<string, double> Variables = new Dictionary<string, double>();
        private string expression;
        private Node root = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionTree"/> class.
        /// </summary>
        /// <param name="expression"> Entered Expression.</param>
        public ExpressionTree(string expression)
        {
            expression.Replace(" ", string.Empty);
            Variables.Clear();
            this.Expression = expression;
            this.root = Format(expression);
        }

        /// <summary>
        /// Gets or Sets
        /// Name: Expression.
        /// </summary>
        public string Expression
        {
            get { return this.expression; }
            set { this.expression = value; }
        }

        /// <summary>
        /// Name: SetVariable.
        /// Description: Adds a variable to the dictionary with its cooresponding value.
        /// </summary>
        /// <param name="variableName"> Name of variable.</param>
        /// <param name="variableValue"> Value of variable.</param>
        public void SetVariable(string variableName, double variableValue)
        {
            Variables[variableName] = variableValue;
        }

        /// <summary>
        /// gets variables as an array.
        /// </summary>
        /// <returns> an array of variables. </returns>
        public string[] GetVariables()
        {
            return Variables.Keys.ToArray();
        }

        /// <summary>
        /// Name: Evaluate.
        /// Description: Public function for private function Evaluate.
        /// </summary>
        /// <returns> The correct node, or an error.</returns>
        public double Evaluate()
        {
            return this.Evaluate(this.root);
        }

        /// <summary>
        /// Name: Format.
        /// Description: Builds a tree based on an expression.
        /// </summary>
        /// <param name="str"> Expression.</param>
        /// <returns> The correct kind of Node.</returns>
        private static Node Format(string str)
        {
            int pCounter = 0; // Counts parenthasis.

            // if string is null or empty.
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }

            // if begining character is an open parenthasis.
            if (str[0] == '(')
            {
                pCounter++;

                for (int i = 0; i < str.Length; i++)
                {
                    if (str[i] == '(')
                    {
                        i++;
                    }
                    else if (str[i] == ')')
                    {
                        pCounter--;

                        if (pCounter == 0)
                        {
                            if (i != str.Length - 1)
                            {
                                break; // No extra Parenthasis.
                            }
                            else
                            {
                                return Format(str.Substring(1, str.Length - 2)); // Remove outer most Parenthasis and restart.
                            }
                        }
                    }
                }
            }

            char[] legalOperators = { '+', '-', '*', '/' };

            foreach (char opp in legalOperators)
            {
                Node node = Operator(str, opp);
                if (node != null)
                {
                    return node;
                }
            }

            // Constant Node.
            if (double.TryParse(str, out double value))
            {
                return new ConstantNode()
                {
                    Value = value,
                };
            }

            // Variable Node.
            else
            {
                VariableNode variable = new VariableNode();
                variable.Name = str;
                Variables[str] = 0;
                return variable;
            }
        }

        /// <summary>
        /// Name: Opperator.
        /// Description: Finds opperator node and calls Format function.
        /// </summary>
        /// <param name="expression"> Expression.</param>
        /// <param name="opp"> Opperator.</param>
        /// <returns> An Operator Node. </returns>
        private static Node Operator(string expression, char opp)
        {
            int pCounter = 0;

            OperatorNodeFactory factory = new OperatorNodeFactory();

            for (int i = expression.Length - 1; i >= 0; i--)
            {
                // if found open parenthasis
                if (expression[i] == '(')
                {
                    pCounter++;
                }

                // if encounter closed parenthasis
                else if (expression[i] == ')')
                {
                    pCounter--;
                }

                // If pCounter is 0 and we found operator.
                if (pCounter == 0 && expression[i] == opp)
                {
                    OperatorNode opNode = factory.CreateOppNode(expression[i]); // create new operator node.
                    opNode.Left = Format(expression.Substring(0, i)); // assign Left opNode
                    opNode.Right = Format(expression.Substring(i + 1)); // assign right opNode
                    return opNode;
                }
            }

            return null;
        }

        /// <summary>
        /// Name: Evaluate.
        /// Description: Evaluates to see what kind of node were dealing with.
        /// </summary>
        /// <param name="node"> a Node.</param>
        /// <returns> a double. </returns>
        private double Evaluate(Node node)
        {
            // Variable Node.
            if (node is VariableNode variableNode)
            {
                // If there are variables in the dictionary.
                if (Variables.Count != 0)
                {
                    return Variables[variableNode.Name]; // return the variable nodes name.
                }
                else
                {
                    return 0;
                }
            }

            // Constant Node.
            if (node is ConstantNode constantNode)
            {
                return constantNode.Value;
            }

            // Operator Node.
            if (node is OperatorNode operatorNode)
            {
                switch (operatorNode.Opp)
                {
                    case '+':
                        return this.Evaluate(operatorNode.Left) + this.Evaluate(operatorNode.Right);
                    case '-':
                        return this.Evaluate(operatorNode.Left) - this.Evaluate(operatorNode.Right);
                    case '*':
                        return this.Evaluate(operatorNode.Left) * this.Evaluate(operatorNode.Right);
                    case '/':
                        return this.Evaluate(operatorNode.Left) / this.Evaluate(operatorNode.Right);
                    default:
                        throw new NotSupportedException("Operator not available");
                }
            }
            else
            {
                return 0;
            }
        }
    }
}
