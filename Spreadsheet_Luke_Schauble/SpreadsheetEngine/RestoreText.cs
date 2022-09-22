namespace SpreadsheetEngine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Name: RestoreText.
    /// Description: Restore text by returning inverse.
    /// </summary>
    public class RestoreText : ICommand
    {
        private Cell cell;
        private string text;

        /// <summary>
        /// Initializes a new instance of the <see cref="RestoreText"/> class.
        /// </summary>
        /// <param name="cell"> Cell.</param>
        /// <param name="text"> Text.</param>
        public RestoreText(Cell cell, string text)
        {
            this.cell = cell;
            this.text = text;
        }

        /// <summary>
        /// Name: Execute.
        /// </summary>
        /// <returns> Inverse of Cell. </returns>
        public ICommand Execute()
        {
            var temp = new RestoreText(this.cell, this.cell.Text);
            this.cell.Text = this.text;
            return temp;
        }
    }
}
