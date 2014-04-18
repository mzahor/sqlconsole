using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace SQLConsole.BizLogic.Extendability
{
    public interface ISQLConsoleComponent
    {
        UserControl ExtensionUI { get; }
        string Name { get; }
        Guid UniqueID { get; }
        string Caption { get; }

    }
}
