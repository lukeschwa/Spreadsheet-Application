namespace SpreadsheetEngine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Name: ICommand.
    /// Description: Defines Method execute to be used by subclasses.
    /// </summary>
    public interface ICommand
    {
        ICommand Execute();
    }
}
