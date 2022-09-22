// Luke Schauble
// 11510454

namespace SpreadsheetEngine
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.IO;
    using System.Linq;
    using System.Xml;
    using System.Xml.Linq;

    public class Spreadsheet
    {
        private Cell[,] pSpreadsheet;
        private int pRows;
        private int pColumns;
        private Dictionary<Cell, List<Cell>> dependence;
        private Stack<MultiCommand> undos = new Stack<MultiCommand>();
        private Stack<MultiCommand> redos = new Stack<MultiCommand>();
        private List<Cell> markVisited = new List<Cell>();

        /// <summary>
        /// Initializes a new instance of the <see cref="Spreadsheet"/> class.
        /// Name: Spreadsheet
        /// Description: Sets up a spreadsheet with the number of rows and columns inputed.
        /// </summary>
        /// <param name="newRow">inputed row.</param>
        /// <param name="newColumn">inputed columns.</param>
        public Spreadsheet(int newRow, int newColumn)
        {
            this.pSpreadsheet = new Cell[newRow, newColumn]; // Sets up a 2D array.
            this.pRows = newRow;
            this.pColumns = newColumn;
            this.dependence = new Dictionary<Cell, List<Cell>>();

            for (int i = 0; i < newRow; i++)
            {
                for (int j = 0; j < newColumn; j++)
                {
                    Cell newCell = new NewCell(i, j); // Creates a new cell with row i, and column j.
                    newCell.PropertyChanged += this.UpdateCell; // subscribe cell.
                    this.pSpreadsheet[i, j] = newCell; // add cell to the spreadsheet.
                }
            }
        }

        public event PropertyChangedEventHandler CellPropertyChanged = (sender, e) => { };

        /// <summary>
        /// Gets
        /// Name: RowCount
        /// Description: Gets the number of rows in the spreadsheet.
        /// </summary>
        /// <returns>Number of Rows in spreadsheet.</returns>
        public int RowCount
        {
            get { return this.pRows; }
        }

        /// <summary>
        /// Gets
        /// Name: ColumnCount
        /// Description: Gets the number of Columns in the spreadsheet.
        /// </summary>
        /// <returns>Number of Columns in spreadsheet.</returns>
        public int ColumnCount
        {
            get { return this.pColumns; }
        }

        /// <summary>
        /// Gets a value indicating whether undos stack is empty.
        /// Name: UndosCheck.
        /// </summary>
        public bool UndosCheck
        {
            get
            {
                return this.undos.Count == 0;
            }
        }

        /// <summary>
        /// Gets a value indicating whether redos stack is empty.
        /// Name: RedosCheck.
        /// </summary>
        public bool RedosCheck
        {
            get
            {
                return this.redos.Count == 0;
            }
        }

        /// <summary>
        /// Gets
        /// Name: PeekUndoStack
        /// Description: peeks at the undo stack and returns the name of the command.
        /// </summary>
        public string PeekUndoStack
        {
            get
            {
                if (this.undos.Count != 0)
                {
                    return this.undos.Peek().GetCommandName; // return the command name of last command on stack.
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// Gets
        /// Name: PeekUndoStack
        /// Description: peeks at the undo stack and returns the name of the command.
        /// </summary>
        public string PeekRedoStack
        {
            get
            {
                if (this.redos.Count != 0)
                {
                    return this.redos.Peek().GetCommandName; // return the command name of last command on stack.
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// Name: GetCell
        /// Description: Locates a cell based on what row and column it is in.
        /// </summary>
        /// <param name="row">Row to locate.</param>
        /// <param name="column">Column to locate.</param>
        /// <returns>Desired cell.</returns>
        public Cell GetCell(int row, int column)
        {
            return this.pSpreadsheet[row, column];
        }

        /// <summary>
        /// Gets a cell location based on its cell name and if it is a valid cell.
        /// </summary>
        /// <param name="name"> The name of a cell. </param>
        /// <returns> Calls GetCell on the row and column, which returns its location.</returns>
        public Cell GetCell(string name)
        {
            int col = name[0] - 'A';
            int row;
            Cell cell;

            // if condition passes than first char is not a letter.
            if (!char.IsLetter(name[0]))
            {
                return null;
            }

            // if condition passes than first char is not upper case.
            if (!char.IsUpper(name[0]))
            {
                return null;
            }

            // if condition passes, string that follows capital letter is not an integer.
            if (!int.TryParse(name.Substring(1), out row))
            {
                return null;
            }

            // attempts to retrieve cell.
            try
            {
                cell = this.GetCell(row - 1, col);
            }
            catch
            {
                return null;
            }

            return cell; // Valid cell.
        }

        /// <summary>
        /// Name: SetValueTestHelper
        /// Description: This is a function that allows Testing for setting the value.
        /// </summary>
        public void SetValueTestHelper()
        {
            this.pSpreadsheet[0, 0].Value = "Test1";
            this.pSpreadsheet[5, 5].Value = "Test2";
            this.pSpreadsheet[10, 10].Value = "Test3";
            this.pSpreadsheet[20, 5].Value = "Test4";
        }

        /// <summary>
        /// Name: AddUndo.
        /// Description: Adds undo commands onto a stack.
        /// </summary>
        /// <param name="undos"> list of undos.</param>
        public void AddUndo(MultiCommand undos)
        {
            this.undos.Push(undos);
        }

        /// <summary>
        /// Name: Undo.
        /// Description: Executes an undo.
        /// </summary>
        public void Undo()
        {
            if (this.undos.Count != 0)
            {
                MultiCommand commands = this.undos.Pop(); // pop undo command off stack.
                this.redos.Push(commands.Execute());
            }
        }

        /// <summary>
        /// Name: Redo.
        /// Description: Executes an redo.
        /// </summary>
        public void Redo()
        {
            if (this.redos.Count != 0)
            {
                MultiCommand commands = this.redos.Pop(); // pop redo command off stack.
                this.undos.Push(commands.Execute());
            }
        }

        /// <summary>
        /// Clears the undos and redo stacks.
        /// </summary>
        public void ClearCommands()
        {
            this.undos.Clear();
            this.redos.Clear();
        }

        /// <summary>
        /// Name: Save.
        /// Description: saves spreadsheet as xml file.
        /// </summary>
        /// <param name="file"> File to save to. </param>
        public void Save(Stream file)
        {
            XmlWriter xFile = XmlWriter.Create(file);
            xFile.WriteStartElement("spreadsheet"); // start tag for spreasheet.

            for (int i = 0; i < this.pRows; i++)
            {
                for (int j = 0; j < this.pColumns; j++)
                {
                    Cell cell = this.pSpreadsheet[i, j];

                    if (cell.Text != string.Empty || cell.Value != string.Empty || cell.BGColor != 0)
                    {
                        xFile.WriteStartElement("cell"); // start tag for a cell.
                        xFile.WriteElementString("name", cell.Name.ToString()); // Name property of cell.
                        if (cell.Text != null)
                        {
                            xFile.WriteElementString("text", cell.Text.ToString()); // Text property of cell.
                        }
                        else
                        {
                            cell.Text = string.Empty;
                            xFile.WriteElementString("text", cell.Text.ToString()); // Text property of cell.
                        }

                        xFile.WriteElementString("color", cell.BGColor.ToString()); // BGColor property of cell.
                        xFile.WriteEndElement();
                    }
                }
            }

            xFile.WriteEndElement();
            xFile.Close();
        }

        /// <summary>
        /// Name: Load.
        /// Description: loads xml file.
        /// </summary>
        /// <param name="file"> File to load. </param>
        public void Load(Stream file)
        {
            XDocument xFile = XDocument.Load(file);

            // loops through the tags in the xml file.
            foreach (XElement tag in xFile.Root.Elements("cell"))
            {
                Cell cell = this.GetCell(tag.Element("name").Value);

                // loads properties based on xml tags.
                if (tag.Element("text") != null)
                {
                    cell.Text = tag.Element("text").Value.ToString();
                }

                if (tag.Element("color") != null)
                {
                    uint color = Convert.ToUInt32(tag.Element("color").Value.ToString());
                    cell.BGColor = color;
                }
            }
        }

        /// <summary>
        /// Name: CircularRefCheck
        /// Description: Checks to see if there is a circular reference.
        /// </summary>
        /// <param name="cell"> The cell. </param>
        /// <returns> true or false depending on if theres a circular reference. </returns>
        public bool CircularRefCheck(Cell cell)
        {
            if (this.markVisited.Count() != this.markVisited.Distinct().Count())
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Name:AddDep.
        /// Description: Adds dependencies for a cell.
        /// </summary>
        /// <param name="refCell"> reference cell.</param>
        /// <param name="ind">independents. </param>
        private void AddDep(Cell refCell, string[] ind)
        {
            foreach (string i in ind)
            {
                Cell independentCell = this.GetCell(i);
                this.dependence[independentCell] = new List<Cell>();
                this.dependence[independentCell].Add(refCell);
            }
        }

        /// <summary>
        /// Name:RemoveDep.
        /// Description: Adds dependencies for a cell.
        /// </summary>
        /// <param name="refCell"> reference cell.</param>
        private void RemoveDep(Cell refCell)
        {
            foreach (List<Cell> dep in this.dependence.Values)
            {
                if (dep.Contains(refCell))
                {
                    dep.Remove(refCell);
                }
            }
        }

        /// <summary>
        /// Name:UpdateDep.
        /// Description: Adds dependencies for a cell.
        /// </summary>
        /// <param name="ind"> reference cell.</param>
        private void UpdateDep(Cell ind)
        {
            foreach (Cell dep in this.dependence[ind].ToArray())
            {
                this.UpdateCell(dep);
            }
        }

        /// <summary>
        /// Name: UpdateCell
        /// Description: updates cell only when Text property is changed.
        /// </summary>
        /// <param name="sender"> Cell to update.</param>
        /// <param name="e"> Event Arguments.</param>
        private void UpdateCell(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "TextChanged")
            {
                Cell cellUpdate = sender as Cell; // get cell to update.
                this.UpdateCell(cellUpdate);
            }
            else if (e.PropertyName == "ColorChanged")
            {
                Cell cellUpdate = sender as Cell; // get cell to update.
                this.CellPropertyChanged(cellUpdate, new PropertyChangedEventArgs("ColorPropertyChanged")); // Notify subscribers that color changed.
            }
        }

        /// <summary>
        /// Name: UpdateCell
        /// Description: overload of the UpdateCell method.Checks if the cell starts with a "=" or not, and changes Value appropriatly.
        /// </summary>
        /// <param name="cellUpdate">cell to be updated.</param>
        private void UpdateCell(Cell cellUpdate)
        {
            this.RemoveDep(cellUpdate);

            // Check if string is empty.
            if (string.IsNullOrEmpty(cellUpdate.Text))
            {
                cellUpdate.Value = string.Empty;
            }

            // If text does not begin with an '='.
            else if (cellUpdate.Text[0] != '=')
            {
                double cellValue;

                // If the cell Text is a double.
                if (double.TryParse(cellUpdate.Text, out cellValue))
                {
                    ExpressionTree cellTree = new ExpressionTree(cellUpdate.Text); // Create new expression tree to set the cells variable.
                    cellValue = cellTree.Evaluate();
                    cellTree.SetVariable(cellUpdate.Name, cellValue);
                    cellUpdate.Value = cellValue.ToString();
                }

                // If not, value is set to text.
                else
                {
                    cellUpdate.Value = cellUpdate.Text;
                }
            }

            // Evaluate Expression.
            else
            {
                string expression = cellUpdate.Text.Substring(1);
                ExpressionTree cellTree = new ExpressionTree(expression);
                cellTree.Evaluate();
                string[] variables = cellTree.GetVariables();
                bool refError = false;

                foreach (var variable in variables)
                {
                    double value = 0;
                    Cell cell = this.GetCell(variable);

                    // Bad Reference check.
                    if (cell == null)
                    {
                        cellUpdate.Value = "!(Bad Reference)";
                        this.CellPropertyChanged(cellUpdate, new PropertyChangedEventArgs("TextPropertyChanged"));
                        refError = true;
                    }

                    // Self Reference check.
                    else if (cellUpdate.Name == variable)
                    {
                        cellUpdate.Value = "!(Self Reference)";
                        this.CellPropertyChanged(cellUpdate, new PropertyChangedEventArgs("TextPropertyChanged"));
                        refError = true;
                    }

                    // If there is a bad/self reference, update if there are dependent cells.
                    if (refError == true)
                    {
                        if (this.dependence.ContainsKey(cellUpdate))
                        {
                            this.UpdateDep(cellUpdate);
                        }

                        return;
                    }

                    double.TryParse(cell.Value, out value);
                    cellTree.SetVariable(variable, value);
                    this.markVisited.Add(cell); // Add cell to HashSet so we can check for a circular reference.
                }

                cellUpdate.Value = cellTree.Evaluate().ToString();
                this.AddDep(cellUpdate, variables);

                // Checks for a circular reference.
                if (this.CircularRefCheck(cellUpdate))
                {
                    cellUpdate.Value = "!(Circular Reference)";
                    this.CellPropertyChanged(cellUpdate, new PropertyChangedEventArgs("TextPropertyChanged"));
                    refError = true;
                }

                if (refError == true)
                {
                    return;
                }
            }

            // Updates all dependent cells that depend on what we updated.
            if (this.dependence.ContainsKey(cellUpdate))
            {
                this.UpdateDep(cellUpdate);
            }

            this.CellPropertyChanged(cellUpdate, new PropertyChangedEventArgs("TextPropertyChanged")); // Notify of change.
            this.markVisited.Clear(); // Clear visited cells once cell is changed.
        }

        /// <summary>
        /// Name: NewCell
        /// Description: Constructor for a cell.
        /// </summary>
        private class NewCell : Cell
        {
            public NewCell(int newRow, int newColumn)
                : base(newRow, newColumn)
            {
            }
        }
    }
}
