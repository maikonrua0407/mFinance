using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Telerik.Windows.Controls.GridView;
using Telerik.Windows.Controls;

namespace PresentationWPF.CustomControl
{
    public class CustomCommandProvider : DefaultKeyboardCommandProvider
    {
        GridViewDataControl parentGrid;

        public CustomCommandProvider(GridViewDataControl grid)
            : base(grid)
        {
            this.parentGrid = grid;
        }

        public override IEnumerable<ICommand> ProvideCommandsForKey(Key key)
        {
            IList<ICommand> commandsToExecute = base.ProvideCommandsForKey(key).ToList();
            if (key == Key.Enter)
            {
                if (!parentGrid.CurrentCell.IsInEditMode)
                {
                    commandsToExecute.Clear();
                    commandsToExecute.Add(RadGridViewCommands.MoveDown);
                    commandsToExecute.Add(RadGridViewCommands.BeginEdit);
                }
            }
            return commandsToExecute;
        }
    }
}
