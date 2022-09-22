// Luke Schauble
// 11510454.

namespace SpreadsheetEngine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class OperatorNode : Node
    {
        private char opp;
        private Node left;
        private Node right;

        /// <summary>
        /// Initializes a new instance of the <see cref="OperatorNode"/> class.
        /// Name: OperatorNode.
        /// </summary>
        /// <param name="opp">new operator.</param>
        public OperatorNode(char opp)
        {
            this.opp = opp;
            this.Left = null;
            this.Right = null;
        }

        /// <summary>
        /// Gets or Sets.
        /// Name: Opp.
        /// </summary>
        public char Opp
        {
            get { return this.opp; }
            set { this.opp = value; }
        }

        /// <summary>
        /// Gets or Sets.
        /// Name: Opp.
        /// </summary>
        public Node Left
        {
            get { return this.left; }
            set { this.left = value; }
        }

        /// <summary>
        /// Gets or Sets.
        /// Name: Opp.
        /// </summary>
        public Node Right
        {
            get { return this.right; }
            set { this.right = value; }
        }

        /// <summary>
        /// Name: Evaluate.
        /// Description: Method for subclasses to inherit.
        /// </summary>
        /// <returns> A Double.</returns>
        public override double Evaluate()
        {
            return 0;
        }
    }
}
