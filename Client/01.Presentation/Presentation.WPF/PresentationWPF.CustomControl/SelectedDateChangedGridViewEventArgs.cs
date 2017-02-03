using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace PresentationWPF.CustomControl
{
    /// <summary>
    /// Routed event args for SelectedDateChanged
    /// </summary>
    public class SelectedDateChangedGridViewEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// Constructor for the event args
        /// </summary>
        /// <param name="routedEvent">The event for which the args will be passed</param>
        public SelectedDateChangedGridViewEventArgs(RoutedEvent routedEvent) : base(routedEvent) { }
        /// <summary>
        /// Gets/Sets the new date that was set
        /// </summary>
        public DateTime NewDate { get; set; }
        /// <summary>
        /// Gets/Sets the old date that was set
        /// </summary>
        public DateTime OldDate { get; set; }

        public Telerik.Windows.Controls.GridView.GridViewCell Cell { get; set; }
    }

    /// <summary>
    /// Delegate for the SelectedDateChanged event
    /// </summary>
    /// <param name="sender">The object that raised the event</param>
    /// <param name="e">Event arguments for the SelectedDateChanged event</param>
    public delegate void SelectedDateChangedGridViewEventHandler(object sender, SelectedDateChangedGridViewEventArgs e);
}
