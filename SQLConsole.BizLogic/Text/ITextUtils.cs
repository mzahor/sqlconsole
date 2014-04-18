using System.Collections.Generic;

namespace SQLConsole.BizLogic.Text
{
    public interface ITextUtils
    {
        IList<SqlScript> GetScripts(string input);
    }
}