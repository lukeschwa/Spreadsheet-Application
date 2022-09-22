// Luke Schauble.
// 11510454.

namespace SpreadsheetEngine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class ConstantNode : Node
    {
        private double value;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstantNode"/> class.
        /// Name: ConstantNode.
        /// </summary>
        public ConstantNode()
        {
        }

        /// <summary>
        /// Gets or Sets
        /// Name: Value.
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
