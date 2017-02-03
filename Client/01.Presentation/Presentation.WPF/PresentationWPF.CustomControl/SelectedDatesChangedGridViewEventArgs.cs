using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows;

namespace PresentationWPF.CustomControl
{
    /// <summary>
    /// Routed event args for SelectedDatesChanged
    /// </summary>
    public class SelectedDatesChangedGridViewEventArgs : RoutedEventArgs
    {
        /// <summary>
        /// Constructor for the event args
        /// </summary>
        /// <param name="routedEvent">The event for which the args will be passed</param>
        public SelectedDatesChangedGridViewEventArgs(RoutedEvent routedEvent) : base(routedEvent) { }
        /// <summary>
        /// Gets/Sets the new date collection
        /// </summary>
        public ObservableCollection<DateTime> NewDates { get; set; }
        /// <summary>
        /// Gets/Sets the old date collection
        /// </summary>
        public ObservableCollection<DateTime> OldDates { get; set; }

        public Telerik.Windows.Controls.GridView.GridViewCell Cell { get; set; }
    }

    /// <summary>
    /// Delegate for the SelectedDatesChanged event
    /// </summary>
    /// <param name="sender">The object that raised the event</param>
    /// <param name="e">Event arguments for the SelectedDatesChanged event</param>
    public delegate void SelectedDatesChangedGridViewEventHandler(object sender, SelectedDatesChangedGridViewEventArgs e);
}
