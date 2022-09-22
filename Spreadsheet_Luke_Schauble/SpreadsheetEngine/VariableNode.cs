// Luke Schauble.
// 11510454.

namespace SpreadsheetEngine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class VariableNode : Node
    {
        private string name;
        private double value;

        /// <summary>
        /// Initializes a new instance of the <see cref="VariableNode"/> class.
        /// Name: VariableNode.
        /// </summary>
        public VariableNode()
        {
        }

        /// <summary>
        /// Gets or Sets.
        /// </summary>
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        /// <summary>
        /// Gets or Sets.
        /// </summary>
        public double Value
        {
            get { return this.value; }
            set { this.value = value; }
        }

        /// <summary>
        /// Name: Evaluate.
        /// Description: Inherits the base class method.
        /// </summary>
        /// <returns> value.</returns>
        public override double Evaluate()
        {
            return this.value;
        }
    }
}
