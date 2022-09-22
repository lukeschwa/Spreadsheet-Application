// Luke Schauble.
// 11510454.

namespace Spreadsheet_Luke_Schauble
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using SpreadsheetEngine;

    public partial class Form1 : Form
    {
        private Spreadsheet pSpreadsheet;

        public Form1()
        {
            this.InitializeComponent();
            this.pSpreadsheet = new Spreadsheet(50, 26);
            this.pSpreadsheet.CellPropertyChanged += this.UpdateForm;
            this.dataGridView1.CellBeginEdit += this.DataGridView1_CellBeginEdit;
            this.dataGridView1.CellEndEdit += this.DataGridView1_CellEndEdit;
            this.EditToolStripMenuItem();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            for (int i = 1; i <= 26; i++)
            {
                char letterTemp = (char)(64 + i); // create each letter acourding to Ascii codes.
                string letter = letterTemp.ToString(); // convert letter to string.
                this.dataGridView1.Columns.Add(i.ToString(), letter); // add letters to data grid view.
            }

            this.dataGridView1.Rows.Add(50); // adds 50 rows.

            for (int i = 0; i < 50; i++)
            {
                var row = this.dataGridView1.Rows[i]; // assign row[i] to var row.
                row.HeaderCell.Value = (i + 1).ToString(); // give each row a name.
            }
        }

        /// <summary>
        /// Name: UpdateForm
        /// Description: Updates the form by changing the cell that needs changing.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="e">Event Arguments.</param>
        private void UpdateForm(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "TextPropertyChanged")
            {
                Cell updateCell = sender as Cell; // get cell to be updated.

                if (updateCell != null)
                {
                    int cellRow = updateCell.RowIndex;
                    int cellColumn = updateCell.ColumnIndex;

                    this.dataGridView1.Rows[cellRow].Cells[cellColumn].Value = updateCell.Value; // Update Cell.
                }
            }
            else if (e.PropertyName == "ColorPropertyChanged")
            {
                Cell updateCell = sender as Cell;

                if (updateCell != null)
                {
                    int cellRow = updateCell.RowIndex;
                    int cellColumn = updateCell.ColumnIndex;

                    int temp = (int)updateCell.BGColor;
                    Color color = Color.FromArgb(temp);

                    this.dataGridView1.Rows[cellRow].Cells[cellColumn].Style.BackColor = color;
                }
            }
        }

        /// <summary>
        /// Name: demoButton_Click
        /// Description: Runs demo when button is pressed.
        /// </summary>
        /// <param name="sender">object.</param>
        /// <param name="e">event arguments.</param>
        private void DemoButton_Click(object sender, EventArgs e)
        {
            Random randNum = new Random();

            for (int i = 0; i < 50; i++)
            {
                int row = randNum.Next(0, 50); // generates random row.
                int col = randNum.Next(0, 26); // generates random column.

                Cell currentCell = this.pSpreadsheet.GetCell(row, col); // Locates the cell at (row, col).
                currentCell.Text = "Luke"; // Changes cell text to "Luke".
            }

            for (int i = 0; i < 50; i++)
            {
                Cell currentCell = this.pSpreadsheet.GetCell(i, 1); // gets first cell in the B, column.
                currentCell.Text = "This is Cell B" + (i + 1); // changes text to reflect what cell it is.
            }

            for (int i = 0; i < 50; i++)
            {
                Cell pCur = this.pSpreadsheet.GetCell(i, 0); // gets first cell in the A, column.
                pCur.Text = "=B" + i; // Changes text in A column cells to be identical to B column cells.
            }
        }

        /// <summary>
        /// dataGridView1_CellBeginEdit.
        /// Description: Runs when user begins editing a cell on the spreadsheet.
        /// </summary>
        /// <param name="sender"> The cell that is being edited.</param>
        /// <param name="e">Event Args.</param>
        private void DataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            int row = e.RowIndex;
            int col = e.ColumnIndex;
            Cell cellUpdate = this.pSpreadsheet.GetCell(row, col);

            if (cellUpdate != null)
            {
                this.dataGridView1.Rows[row].Cells[col].Value = cellUpdate.Text;
            }
        }

        /// <summary>
        /// dataGridView1_CellEndEdit.
        /// Description: Runs when user stops editing a cell on the spreadsheet.
        /// </summary>
        /// <param name="sender"> The cell that is being edited.</param>
        /// <param name="e">Event Args.</param>
        private void DataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            int row = e.RowIndex;
            int col = e.ColumnIndex;
            Cell cellUpdate = this.pSpreadsheet.GetCell(row, col);
            bool edit = true;
            RestoreText[] undoText = new RestoreText[1];
            string prevText = cellUpdate.Text;
            undoText[0] = new RestoreText(cellUpdate, prevText);

            if (cellUpdate != null)
            {
                try
                {
                    if (cellUpdate.Text == this.dataGridView1.Rows[row].Cells[col].Value.ToString())
                    {
                        edit = false;
                    }

                    cellUpdate.Text = this.dataGridView1.Rows[row].Cells[col].Value.ToString();
                }
                catch (NullReferenceException)
                {
                    if (cellUpdate.Text == null)
                    {
                        edit = false;
                    }

                    cellUpdate.Text = string.Empty;
                }

                this.dataGridView1.Rows[row].Cells[col].Value = cellUpdate.Value;

                if (edit)
                {
                    this.pSpreadsheet.AddUndo(new MultiCommand(undoText, "text change"));
                    this.EditToolStripMenuItem();
                }
            }
        }

        /// <summary>
        /// chooseBackgroundColorToolStripMenuItem_Click
        /// Description: Runs when user selects "change cell background" menu item.
        /// </summary>
        /// <param name="sender"> The cell that is being edited.</param>
        /// <param name="e">Event Args.</param>
        private void ChooseBackgroundColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            List<RestoreColor> undoColors = new List<RestoreColor>(); // create a list for multiple cell color change.

            // If user presses "Ok" in color dialog box.
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                // Loops through each of selected cells.
                foreach (DataGridViewCell cell in this.dataGridView1.SelectedCells)
                {
                    Cell cellUpdate = this.pSpreadsheet.GetCell(cell.RowIndex, cell.ColumnIndex); // gets cell to be updated.
                    uint prevColor = cellUpdate.BGColor; // gets previous color of that cell.

                    if (prevColor == 0)
                    {
                        prevColor = (uint)Color.White.ToArgb(); // change to white if color is 0.
                    }

                    cellUpdate.BGColor = (uint)colorDialog.Color.ToArgb();
                    RestoreColor restoreColor = new RestoreColor(cellUpdate, prevColor);
                    undoColors.Add(restoreColor); // adds cells previous color to list.
                }
            }

            this.pSpreadsheet.AddUndo(new MultiCommand(undoColors.ToArray(), "changed backround color")); // add all changes to undo stack.

            this.EditToolStripMenuItem();
        }

        /// <summary>
        /// undoToolStripMenuItem_Click.
        /// </summary>
        /// <param name="sender"> sending object.</param>
        /// <param name="e"> Event Args.</param>
        private void UndoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.pSpreadsheet.Undo();
            this.EditToolStripMenuItem();
        }

        /// <summary>
        /// undoToolStripMenuItem_Click.
        /// </summary>
        /// <param name="sender"> sending object.</param>
        /// <param name="e"> Event Args.</param>
        private void RedoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.pSpreadsheet.Redo();
            this.EditToolStripMenuItem();
        }

        /// <summary>
        /// Name: ClearSpreadsheet.
        /// Description: clears spreadsheet for new spreadsheet.
        /// </summary>
        private void ClearSpreadsheet()
        {
            for (int i = 0; i < this.pSpreadsheet.RowCount; i++)
            {
                for (int j = 0; j < this.pSpreadsheet.ColumnCount; j++)
                {
                    Cell cell = this.pSpreadsheet.GetCell(i, j);
                    cell.Text = string.Empty;
                    cell.Value = string.Empty;
                    cell.BGColor = 0;
                }
            }
        }

        /// <summary>
        /// Name: EditToolStripMenuItem
        /// Description: updates the menu strip items to reflect undo or redo of last command name.
        /// </summary>
        private void EditToolStripMenuItem()
        {
            ToolStripMenuItem items = this.menuStrip1.Items[0] as ToolStripMenuItem; // Menu items under the "Edit" tab.
            string undo = "Undo";
            string redo = "Redo";

            // Loop through "Edit" drop down menu items
            foreach (ToolStripMenuItem item in items.DropDownItems)
            {
                if (item.Text.Substring(0, 4) == undo)
                {
                    item.Enabled = !this.pSpreadsheet.UndosCheck; // Enable men strip item "Undo" if stack is not empty.
                    item.Text = undo + " " + this.pSpreadsheet.PeekUndoStack; // Change text to command name.
                }
                else if (item.Text.Substring(0, 4) == redo)
                {
                    item.Enabled = !this.pSpreadsheet.RedosCheck; // Enable men strip item "Undo" if stack is not empty.
                    item.Text = redo + " " + this.pSpreadsheet.PeekRedoStack; // Change text to command name.
                }
            }
        }

        /// <summary>
        /// Name: saveToolStripMenuItem_Click.
        /// Description: saves a file when save button is clicked.
        /// </summary>
        /// <param name="sender"> Sending object.</param>
        /// <param name="e"> Event Args.</param>
        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "XML File (*.xml)|*.xml|All files (*.*)|*.*";
            save.FilterIndex = 1;

            if (save.ShowDialog() == DialogResult.OK)
            {
                Stream file = new FileStream(save.FileName, FileMode.Create, FileAccess.Write);
                this.pSpreadsheet.Save(file);
                file.Dispose();
            }
        }

        /// <summary>
        /// Name: loadToolStripMenuItem_Click.
        /// Description: loads a file when load button is clicked.
        /// </summary>
        /// <param name="sender"> Sending object.</param>
        /// <param name="e"> Event Args.</param>
        private void LoadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog load = new OpenFileDialog();
            load.Filter = "XML File (*.xml)|*.xml|All files (*.*)|*.*";
            load.FilterIndex = 1;

            if (load.ShowDialog() == DialogResult.OK)
            {
                this.ClearSpreadsheet();
                Stream file = new FileStream(load.FileName, FileMode.Open, FileAccess.Read);
                this.pSpreadsheet.Load(file);
                file.Dispose();
                this.pSpreadsheet.ClearCommands();
            }

            this.EditToolStripMenuItem();
        }
    }
}
