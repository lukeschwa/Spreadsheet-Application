namespace SpreadsheetEngine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Name: RestoreColor.
    /// Description: Restore color by returning inverse.
    /// </summary>
    public class RestoreColor : ICommand
    {
        private Cell cell;
        private uint color;

        /// <summary>
        /// Initializes a new instance of the <see cref="RestoreColor"/> class.
        /// </summary>
        /// <param name="cell"> Cell.</param>
        /// <param name="color"> Color.</param>
        public RestoreColor(Cell cell, uint color)
        {
            this.cell = cell;
            this.color = color;
        }

        /// <summary>
        /// Name: Execute.
        /// </summary>
        /// <returns> Inverse of Cell.</returns>
        public ICommand Execute()
        {
            var temp = new RestoreColor(this.cell, this.cell.BGColor);
            this.cell.BGColor = this.color;
            return temp;
        }
    }
}
