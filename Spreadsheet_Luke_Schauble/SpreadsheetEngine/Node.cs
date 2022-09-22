// Luke Schauble.
// 11510454.

namespace SpreadsheetEngine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Name: Node
    /// Description: Constructor for a base Node class.
    /// </summary>
    internal abstract class Node
    {
        /// <summary>
        /// Name: Evaluate.
        /// Description: Method for subclasses to inherit.
        /// </summary>
        /// <returns> A Double.</returns>
        public abstract double Evaluate();
    }
}
