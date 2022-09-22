namespace SpreadsheetEngine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Name: MultiCommand
    /// Description: Handles multiple commands in an array.
    /// </summary>
    public class MultiCommand
    {
        private ICommand[] commands;
        private string commandName;

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiCommand"/> class.
        /// </summary>
        /// <param name="commands"> array of commands. </param>
        /// <param name="commandName"> the commands Name.</param>
        public MultiCommand(ICommand[] commands, string commandName)
        {
            this.commands = commands;
            this.commandName = commandName;
        }

        /// <summary>
        /// Gets the CommandName.
        /// </summary>
        public string GetCommandName
        {
            get { return this.commandName; }
        }

        /// <summary>
        /// Name: Execute.
        /// Description: Loops through list of commands and returns inverse array.
        /// </summary>
        /// <returns> Inverse array.</returns>
        public MultiCommand Execute()
        {
            List<ICommand> commandList = new List<ICommand>();

            foreach (ICommand c in this.commands)
            {
                commandList.Add(c.Execute());
            }

            return new MultiCommand(commandList.ToArray(), this.commandName);
        }
    }
}
