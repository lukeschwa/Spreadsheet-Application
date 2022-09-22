// Luke Schauble.
// 11510454.

namespace SpreadsheetEngine
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Cell : INotifyPropertyChanged
    {
        protected readonly int rowIndex;
        protected readonly int columnIndex;
        protected string pText;
        protected string pValue;
        protected string pName;
        protected uint pColor;

        /// <summary>
        /// Initializes a new instance of the <see cref="Cell"/> class.
        /// Name: Cell
        /// Description: Sets up a cell with inputed Row and Column.
        /// </summary>
        /// <param name="newRow">Inputed Row.</param>
        /// <param name="newColumn">Inputed Column.</param>
        public Cell(int newRow, int newColumn)
        {
            this.rowIndex = newRow;
            this.columnIndex = newColumn;

            this.pName += Convert.ToChar('A' + newColumn);
            this.pName += (newRow + 1).ToString();
        }

        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        /// <summary>
        /// Gets
        /// Name: RowIndex
        /// Description: Returns Row Index.
        /// </summary>
        public int RowIndex
        {
            get { return this.rowIndex; }
        }

        /// <summary>
        /// Gets
        /// Name: ColumnIndex
        /// Description: Returns the Column Index.
        /// </summary>
        public int ColumnIndex
        {
            get { return this.columnIndex; }
        }

        /// <summary>
        /// Gets or Sets
        /// Name: Text
        /// Description: Gets and sets the Text of a cell.
        /// </summary>
        public string Text
        {
            get
            {
                return this.pText;
            }

            set
            {
                this.pText = value; // Update text.
                this.PropertyChanged(this, new PropertyChangedEventArgs("TextChanged")); // Notify Subscribers.
            }
        }

        /// <summary>
        /// Gets or Sets
        /// Name: Value
        /// Description: Gets and sets the Value of a cell.
        /// </summary>
        public string Value
        {
            get
            {
                return this.pValue;
            }

            set
            {
                this.pValue = value; // Change the value Property.
                this.PropertyChanged(this, new PropertyChangedEventArgs("ValueChanged")); // Notify Subscribers.
            }
        }

        /// <summary>
        /// Gets the Name of Cell.
        /// </summary>
        public string Name
        {
            get { return this.pName; }
        }

        /// <summary>
        /// Gets or sets the color of a cell.
        /// </summary>
        public uint BGColor
        {
            get
            {
                return this.pColor;
            }

            set
            {
                // If color is being changed to the same color.
                if (this.pColor == value)
                {
                    return;
                }

                this.pColor = value;
                this.PropertyChanged(this, new PropertyChangedEventArgs("ColorChanged")); // Notify subscribers that color property changed.
            }
        }
    }
}
