namespace SpreadsheetEngine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    internal class OperatorNodeFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OperatorNodeFactory"/> class.
        /// Name: OperatorNodeFactory.
        /// </summary>
        public OperatorNodeFactory()
        {
        }

        /// <summary>
        /// Name: GetOperatorNode.
        /// Description: gets OperatorNode with opp.
        /// </summary>
        /// <param name="opp">New operator.</param>
        /// <returns> Operator Node with operator.</returns>
        public static OperatorNode GetOperatorNode(char opp)
        {
            return new OperatorNode(opp);
        }

        /// <summary>
        /// Name: CreateOppNode.
        /// Description: creates a new operator node.
        /// </summary>
        /// <param name="opp"> Operator.</param>
        /// <returns> New OpperatorNode.</returns>
        public OperatorNode CreateOppNode(char opp)
        {
            return GetOperatorNode(opp);
        }
    }
}
