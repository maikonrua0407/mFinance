using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using Utilities.Common;

using Telerik.Windows.Controls.GridView;
using Telerik.Windows.Controls.GridView.Clipboard;
using Telerik.Windows.Controls.GridView.Columns;
using Telerik.Windows.Data;
using Telerik.Windows.Controls;
using System.Windows.Media.Imaging;
using vhCalendar;
using System.ComponentModel;
using System.Windows.Controls.Primitives;


namespace PresentationWPF.CustomControl
{
    public class ColumnsDateTimeMulti : GridViewBoundColumnBase, INotifyPropertyChanged
    {
        //#region Khai bao RoutedEvent
        ///// <summary>
        ///// Event for the DateSelectionChanged raised when the date changes
        ///// </summary>
        //public static readonly RoutedEvent SelectedDateChangedEvent = EventManager.RegisterRoutedEvent("SelectedDateChanged",
        //    RoutingStrategy.Bubble, typeof(SelectedDateChangedEventHandler), typeof(vhCalendar.Calendar));

        ///// <summary>
        ///// Event for the DateSelectionChanged raised when the date changes
        ///// </summary>
        //public event SelectedDateChangedEventHandler SelectedDateChanged
        //{
        //    add { AddHandler(SelectedDateChangedEvent, value); }
        //    remove { RemoveHandler(SelectedDateChangedEvent, value); }
        //}

        ///// <summary>
        ///// Event for the DateSelectionChanged raised when the date changes
        ///// </summary>
        //public static readonly RoutedEvent SelectedDatesChangedEvent = EventManager.RegisterRoutedEvent("SelectedDatesChanged",
        //    RoutingStrategy.Bubble, typeof(SelectedDatesChangedEventHandler), typeof(vhCalendar.Calendar));

        ///// <summary>
        ///// Event for the DateSelectionChanged raised when the date changes
        ///// </summary>
        //public event SelectedDatesChangedEventHandler SelectedDatesChanged
        //{
        //    add { AddHandler(SelectedDatesChangedEvent, value); }
        //    remove { RemoveHandler(SelectedDatesChangedEvent, value); }
        //}
        //#endregion
        #region KhaiBaoConts
        private static string DisplayDateProMember = "DisplayDate";
        private static string IsChangeProMember = "IsChange";
        private static string DataMemberBindingProMember = "DataMemberBinding";
        private static string SeparatorProMember = "Separator";
        private static string FormatStringProMember = "FormatString";
        private static string ValuesStringProMember = "ValuesString";
        #endregion
        #region Khai bao control, biến
        Telerik.Windows.Controls.GridView.GridViewCell cellCurrent = null;
        #endregion
        #region Khoi tao
        static ColumnsDateTimeMulti()
        {
            // DisplayDate
            FrameworkPropertyMetadata displayDateMetaData = new FrameworkPropertyMetadata
            {
                DefaultValue = DateTime.Now,
                PropertyChangedCallback = new PropertyChangedCallback(OnDisplayDateChanged),
                AffectsRender = true,
                AffectsMeasure = true
            };
            DisplayDateProperty = DependencyProperty.Register(DisplayDateProMember, typeof(DateTime), typeof(ColumnsDateTimeMulti), displayDateMetaData);

            // DisplayDate
            FrameworkPropertyMetadata isChangeMetaData = new FrameworkPropertyMetadata
            {
                DefaultValue = true,
                PropertyChangedCallback = new PropertyChangedCallback(OnIsChangeChanged),
                AffectsRender = true,
                AffectsMeasure = true
            };
            IsChangeProperty = DependencyProperty.Register(IsChangeProMember, typeof(Boolean), typeof(ColumnsDateTimeMulti), isChangeMetaData);

            // DisplayDate
            FrameworkPropertyMetadata dataMemberBindingMetaData = new FrameworkPropertyMetadata
            {
                DefaultValue = "",
                PropertyChangedCallback = new PropertyChangedCallback(OnDataMemberBindingChanged),
                AffectsRender = true,
                AffectsMeasure = true
            };
            DataMemberBindingProperty = DependencyProperty.Register(DataMemberBindingProMember, typeof(String), typeof(ColumnsDateTimeMulti), dataMemberBindingMetaData);

            // DisplayDate
            FrameworkPropertyMetadata separatorMetaData = new FrameworkPropertyMetadata
            {
                DefaultValue = ";",
                PropertyChangedCallback = new PropertyChangedCallback(OnSeparatorChanged),
                AffectsRender = true,
                AffectsMeasure = true
            };
            SeparatorProperty = DependencyProperty.Register(SeparatorProMember, typeof(String), typeof(ColumnsDateTimeMulti), separatorMetaData);

            // DisplayDate
            FrameworkPropertyMetadata formatStringMetaData = new FrameworkPropertyMetadata
            {
                DefaultValue = "dd/MM/yyyy",
                PropertyChangedCallback = new PropertyChangedCallback(OnFormatStringChanged),
                AffectsRender = true,
                AffectsMeasure = true
            };
            FormatStringProperty = DependencyProperty.Register(FormatStringProMember, typeof(String), typeof(ColumnsDateTimeMulti), formatStringMetaData);

            // DisplayDate
            FrameworkPropertyMetadata valuesStringMetaData = new FrameworkPropertyMetadata
            {
                DefaultValue = "",
                PropertyChangedCallback = new PropertyChangedCallback(OnValuesStringChanged),
                AffectsRender = true,
                AffectsMeasure = true
            };
            ValuesStringProperty = DependencyProperty.Register(ValuesStringProMember, typeof(String), typeof(ColumnsDateTimeMulti), valuesStringMetaData);
        }
        #endregion

        #region INotifyPropertyChanged Members
        /// <summary>
        /// Event raised when a property is changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the property changed event
        /// </summary>
        /// <param name="e">The arguments to pass</param>
        protected void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, e);
            }
        }
        #endregion

        #region Khai bao Properties
        //string _dataMemberBinding;

        //public string DataMemberBinding
        //{
        //    get { return _dataMemberBinding; }
        //    set { _dataMemberBinding = value; }
        //}
        #region DisplayDate
        /// <summary>
        /// Gets/Sets the date that is being displayed in the calendar
        /// </summary>
        public static readonly DependencyProperty DisplayDateProperty;

        public DateTime DisplayDate
        {
            get { return (DateTime)this.GetValue(DisplayDateProperty); }
            set { this.SetValue(DisplayDateProperty, value); }
        }

        private static void OnDisplayDateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ColumnsDateTimeMulti vc = d as ColumnsDateTimeMulti;
            vc.OnPropertyChanged(new PropertyChangedEventArgs(DisplayDateProMember));
        }
        #endregion

        #region IsChange
        /// <summary>
        /// Gets/Sets the date that is being displayed in the calendar
        /// </summary>
        public static readonly DependencyProperty IsChangeProperty;

        public Boolean IsChange
        {
            get { return (Boolean)this.GetValue(IsChangeProperty); }
            set { this.SetValue(IsChangeProperty, value); }
        }

        private static void OnIsChangeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ColumnsDateTimeMulti vc = d as ColumnsDateTimeMulti;
            vc.OnPropertyChanged(new PropertyChangedEventArgs(IsChangeProMember));
        }
        #endregion

        #region DataMember
        /// <summary>
        /// Gets/Sets the DataMemberBinding that is being displayed in the calendar
        /// </summary>
        public static readonly DependencyProperty DataMemberBindingProperty;

        public String DataMemberBinding
        {
            get { return (String)this.GetValue(DataMemberBindingProperty); }
            set { this.SetValue(DataMemberBindingProperty, value); }
        }

        private static void OnDataMemberBindingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ColumnsDateTimeMulti vc = d as ColumnsDateTimeMulti;
            vc.OnPropertyChanged(new PropertyChangedEventArgs(DataMemberBindingProMember));
        }
        #endregion

        #region Separator
        /// <summary>
        /// Gets/Sets the DataMemberBinding that is being displayed in the calendar
        /// </summary>
        public static readonly DependencyProperty SeparatorProperty;

        public String Separator
        {
            get { return (String)this.GetValue(SeparatorProperty); }
            set { this.SetValue(SeparatorProperty, value); }
        }

        private static void OnSeparatorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ColumnsDateTimeMulti vc = d as ColumnsDateTimeMulti;
            vc.OnPropertyChanged(new PropertyChangedEventArgs(SeparatorProMember));
        }
        #endregion

        #region FormatString
        /// <summary>
        /// Gets/Sets the DataMemberBinding that is being displayed in the calendar
        /// </summary>
        public static readonly DependencyProperty FormatStringProperty;

        public String FormatString
        {
            get { return (String)this.GetValue(FormatStringProperty); }
            set { this.SetValue(FormatStringProperty, value); }
        }

        private static void OnFormatStringChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ColumnsDateTimeMulti vc = d as ColumnsDateTimeMulti;
            vc.OnPropertyChanged(new PropertyChangedEventArgs(FormatStringProMember));
        }
        #endregion

        #region ValuesString
        /// <summary>
        /// Gets/Sets the DataMemberBinding that is being displayed in the calendar
        /// </summary>
        public static readonly DependencyProperty ValuesStringProperty;

        public String ValuesString
        {
            get { return (String)this.GetValue(ValuesStringProperty); }
            set { this.SetValue(ValuesStringProperty, value); }
        }

        private static void OnValuesStringChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ColumnsDateTimeMulti vc = d as ColumnsDateTimeMulti;
            vc.OnPropertyChanged(new PropertyChangedEventArgs(ValuesStringProMember));
        }
        #endregion


        #endregion
        public override FrameworkElement CreateCellElement(Telerik.Windows.Controls.GridView.GridViewCell cell, object dataItem)
        {
            TextBlock txtBlock = new TextBlock();
            txtBlock.SetBinding(TextBlock.TextProperty,BindingDataSource(dataItem,DataMemberBinding));
            return txtBlock;
        }

        public override FrameworkElement CreateCellEditElement(Telerik.Windows.Controls.GridView.GridViewCell cell, object dataItem)
        {
            cellCurrent = cell;
            return CreateGrid(dataItem);
        }

        void calendar_SelectedDatesChanged(object sender, vhCalendar.SelectedDatesChangedEventArgs e)
        {
            SelectedDatesChangedGridViewEventArgs even = new SelectedDatesChangedGridViewEventArgs(SelectedDatesGridViewChangedEvent);
            ValuesString = "";
            foreach (DateTime datetime in e.NewDates.OrderBy(f => f))
            {
                ValuesString += Separator + datetime.ToString(FormatString);
            }
            if (ValuesString.Length > 1)
                ValuesString = ValuesString.Substring(1);
            even.NewDates = e.NewDates;
            even.OldDates = e.OldDates;
            even.Cell = cellCurrent;
            TextBox txt = cellCurrent.FindChildByType<TextBox>();
            if (!LObject.IsNullOrEmpty(txt))
                txt.Text = ValuesString;
            RaiseEvent(even);
        }

        private Binding BindingDataSource(object dataSource, string Path)
        {
            Binding binding = new Binding();
            binding.Path = new PropertyPath(Path);
            binding.Source = dataSource;
            return binding;
        }

        private Grid CreateGrid(object dataItem)
        {
            Grid gridPanel = new Grid();
            try
            {
                TextBox txt = new TextBox();
                RadToggleButton toggleButton = new RadToggleButton();
                txt.Margin = new Thickness(0, 0, 35, 0);
                txt.HorizontalAlignment = HorizontalAlignment.Stretch;
                txt.SetBinding(TextBox.TextProperty, BindingDataSource(dataItem, DataMemberBinding));
                
                System.Windows.Controls.Image ima = new System.Windows.Controls.Image();
                ima.Width = 25;
                ima.Height = 25;
                BitmapImage myBitmapImage = new BitmapImage();

                // Set image vào image box
                myBitmapImage.BeginInit();
                myBitmapImage.UriSource = new Uri("/Utilities.Common;component/Images/Action/cash_statement.png", UriKind.Relative);
                myBitmapImage.DecodePixelWidth = (int)20;
                myBitmapImage.DecodePixelHeight = (int)20;
                myBitmapImage.EndInit();
                ima.Source = myBitmapImage;
                ima.HorizontalAlignment = HorizontalAlignment.Center;
                ima.VerticalAlignment = VerticalAlignment.Center;
                ima.Height = 20;
                ima.Width = 20;
                toggleButton.Content = ima;
                toggleButton.Width = 25;
                toggleButton.HorizontalAlignment = HorizontalAlignment.Right;
                Popup popup = new Popup();
                popup.StaysOpen = false;
                vhCalendar.Calendar calendar = new vhCalendar.Calendar();
                calendar.HorizontalAlignment = HorizontalAlignment.Stretch;
                calendar.Width = 250;
                calendar.Height = 220;
                calendar.SelectionMode = SelectionType.Multiple;
                calendar.IsChange = IsChange;
                calendar.Theme = "Royale";
                calendar.DisplayDate = DisplayDate;
                calendar.IsTodayHighlighted = IsChange;
                calendar.FooterVisibility = Visibility.Collapsed;
                calendar.WeekColumnVisibility = Visibility.Collapsed;
                calendar.IsTabStop = false;
                calendar.SelectedDatesChanged += new vhCalendar.SelectedDatesChangedEventHandler(calendar_SelectedDatesChanged);
                popup.Child = calendar;
                popup.SetBinding(Popup.IsOpenProperty, BindingDataSource(toggleButton, "IsChecked"));
                //gridPanel.Children.Add(popup);
                var ControlText = gridPanel.FindChildByType<TextBox>();
                if (ControlText == null)
                    gridPanel.Children.Add(txt);
                var Control = gridPanel.FindChildByType<RadToggleButton>();
                if (Control == null)
                    gridPanel.Children.Add(toggleButton);
                var ControlPopup = gridPanel.FindChildByType<Popup>();
                if (ControlPopup == null)
                    gridPanel.Children.Add(popup);
                
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            return gridPanel;
        }

        /// <summary>
        /// Event for the DateSelectionChanged raised when the date changes
        /// </summary>
        public static readonly RoutedEvent SelectedDatesGridViewChangedEvent = EventManager.RegisterRoutedEvent("SelectedDatesGridViewChanged",
            RoutingStrategy.Bubble, typeof(SelectedDatesChangedGridViewEventHandler), typeof(ColumnsDateTimeMulti));

        /// <summary>
        /// Event for the DateSelectionChanged raised when the date changes
        /// </summary>
        public event SelectedDatesChangedGridViewEventHandler SelectedDatesGridViewChanged
        {
            add { AddHandler(SelectedDatesGridViewChangedEvent, value); }
            remove { RemoveHandler(SelectedDatesGridViewChangedEvent, value); }
        }
    }
}
