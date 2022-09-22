using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetEngine
{
    public abstract class Cell : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        protected readonly int rowIndex;
        protected readonly int columnIndex;
        protected string pText;
        protected string pValue;

        /// <summary>
        /// Name: Cell
        /// Description: Sets up a cell with inputed Row and Column.
        /// </summary>
        /// <param name="newRow">Inputed Row</param>
        /// <param name="newColumn">Inputed Column</param>
        public Cell(int newRow, int newColumn)
        {
            this.rowIndex = newRow;
            this.columnIndex = newColumn;
        }
        
        /// <summary>
        /// Name: RowIndex
        /// Description: Returns Row Index
        /// </summary>
        public int RowIndex
        {
            get { return this.rowIndex; }
        }

        /// <summary>
        /// Name: ColumnIndex
        /// Description: Returns the Column Index.
        /// </summary>
        public int ColumnIndex
        {
            get { return this.ColumnIndex; }
        }

        /// <summary>
        /// Name: Text
        /// Description: Gets and sets the Text of a cell.
        /// </summary>
        public string Text
        {
            get { return this.pText; }
            set
            {
                this.pText = value; //Update text.
                PropertyChanged(this, new PropertyChangedEventArgs("Text")); //Notify Subscribers.
            }
        }

        /// <summary>
        /// Name: Value
        /// Description: Gets and sets the Value of a cell
        /// </summary>
        public string Value
        {
            get { return this.pValue; }
            protected internal set //Only Spreadsheet class can edit
            {
                this.pValue = value;//Change the value Property.
                PropertyChanged(this, new PropertyChangedEventArgs("Value")); //Notify Subscribers.
            }
        }
    }
}
