using System;
using System.Data;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Xml;
using System.Xml.Linq;
//using <xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation">;
//using <xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">;
using System.Windows.Markup;
using Utilities.Common;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Input;
using Telerik.Windows.Controls.MaskedInput;
using Telerik.Windows.Controls.MaskedTextBox;
using Telerik.Windows.Controls.Data;
using PresentationWPF.CustomControl;

namespace PresentationWPF.Demo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public static EventModel _event;
        List<DataRow> lstControl = new List<DataRow>();

        public MainWindow()
        {
            InitializeComponent();

            //_event = new EventModel();

            //LoadUI();

        }

        public event EventHandler<KeyEventArgs> EnterKeyPressed;

        protected virtual void OnEnterKeyPressed(KeyEventArgs e)
        {
            EventHandler<KeyEventArgs> handler = this.EnterKeyPressed;
            if (null != handler)
            {
                handler(this, e);
            }
        }

        private bool ProcessDatePickerKey(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.System:
                    {
                        switch (e.SystemKey)
                        {
                            case Key.Down:
                                {
                                    if ((Keyboard.Modifiers & ModifierKeys.Alt) == ModifierKeys.Alt)
                                    {
                                        //TogglePopUp();
                                        return true;
                                    }
                                    else return false;
                                }
                            default: return false;
                        }
                    }

                case Key.Enter:
                    {
                        OnEnterKeyPressed(e);
                        return true;
                    }

                default: return false;
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            grdDynamicContent.Children.Clear();
            lstControl.Clear();
            LoadUIFromXMLFile();
            //LoadUIFromXML();
            //LoadUI();
        }

        public void LoadUIFromXMLFile()
        {
            try
            {
                DataSet dsUI = ReadXMLFile(@"D:\Dev\ASPNET\GenerateForm\pgExcel.xml");

                if (!LObject.IsNullOrEmpty(dsUI) && dsUI.Tables.Count > 0)
                {
                    CreateGridView(dsUI.Tables["Control"].Rows.Count);

                    DataRow dr = null;
                    Object obj = null;
                    for (int i = 0; i < dsUI.Tables["Control"].Rows.Count; i++)
                    {
                        dr = dsUI.Tables["Control"].Rows[i];
                        lstControl.Add((DataRow)dr);
                        CreateControl(dr, ref obj);
                        grdDynamicContent.Children.Add((UIElement)obj);

                        //if (obj.GetType().Name.Equals("Label"))
                        //    grdDynamicContent.Children.Add((System.Windows.Controls.Label)obj);
                        //else if (obj.GetType().Name.Equals("TextBox"))
                        //    grdDynamicContent.Children.Add((System.Windows.Controls.TextBox)obj);
                        //else if (obj.GetType().Name.Equals("ComboBox"))
                        //    grdDynamicContent.Children.Add((System.Windows.Controls.ComboBox)obj);
                    }

                    //System.Windows.Controls.TextBox txt = new System.Windows.Controls.TextBox();
                    //txt.Name = "abc";
                    //Grid.SetColumn(txt, 0);
                    //Grid.SetRow(txt, 0);
                    //grdDynamicContent.Children.Add(txt);
                    //obj = txt;
                }
            }
            catch (Exception ex)
            {
                LMessage.ShowMessage(ex.Message, LMessage.MessageBoxType.Error);
            }

        }

        public void LoadUIFromXML()
        {
            try
            {
                Grid grd = new Grid();
                System.Windows.Controls.GroupBox grp = new System.Windows.Controls.GroupBox();
                
                DataSet dsUI = ReadXMLFile(@"D:\Dev\ASPNET\GenerateForm\pgExcel.xml");

                if (!LObject.IsNullOrEmpty(dsUI) && dsUI.Tables.Count > 0)
                {
                    CreateGridView(dsUI.Tables["Control"].Rows.Count, ref grd);

                    DataRow dr = null;
                    Object obj = null;
                    for (int i = 0; i < dsUI.Tables["Control"].Rows.Count; i++)
                    {
                        dr = dsUI.Tables["Control"].Rows[i];
                        lstControl.Add((DataRow)dr);
                        CreateControl(dr, ref obj);
                        grd.Children.Add((UIElement)obj);
                    }
                }

                grp.Content = grd;

                DynamicContent.Content = grp;
            }
            catch (Exception ex)
            {
                LMessage.ShowMessage(ex.Message, LMessage.MessageBoxType.Error);
            }

        }

        public int CreateGridView(int irow)
        {
            int iret = 0;
            try
            {
                //setting margin
                grdDynamicContent.Margin = new Thickness(10.0);

                //setting column
                grdDynamicContent.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(110.0) });
                grdDynamicContent.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
                grdDynamicContent.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(35.0) });
                grdDynamicContent.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(10.0) });
                grdDynamicContent.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(110.0) });
                grdDynamicContent.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
                grdDynamicContent.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(35.0) });

                //setting rows
                RowDefinition row1 = null;
                RowDefinition row2 = new RowDefinition();

                for (int i = 0; i < irow; i++)
                {
                    row1 = new RowDefinition() { Height = new GridLength(28) };
                    row2 = new RowDefinition() { Height = new GridLength(6) };
                    grdDynamicContent.RowDefinitions.Add(row1);
                    grdDynamicContent.RowDefinitions.Add(row2);
                }

                iret = 1;
            }
            catch (Exception ex)
            {
                iret = 0;
                LMessage.ShowMessage(ex.Message, LMessage.MessageBoxType.Error);
            }

            return iret;
        }

        public int CreateGridView(int irow, ref Grid grd)
        {
            int iret = 0;
            try
            {
                if (grd == null) grd = new Grid();
                //setting margin
                grd.Margin = new Thickness(10.0);

                //setting column
                grd.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(110.0) });
                grd.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
                grd.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(35.0) });
                grd.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(10.0) });
                grd.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(110.0) });
                grd.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
                grd.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(35.0) });

                //setting rows
                RowDefinition row1 = null;
                RowDefinition row2 = new RowDefinition();

                for (int i = 0; i < irow; i++)
                {
                    row1 = new RowDefinition() { Height = new GridLength(28) };
                    row2 = new RowDefinition() { Height = new GridLength(6) };
                    grd.RowDefinitions.Add(row1);
                    grd.RowDefinitions.Add(row2);
                }

                iret = 1;
            }
            catch (Exception ex)
            {
                iret = 0;
                LMessage.ShowMessage(ex.Message, LMessage.MessageBoxType.Error);
            }

            return iret;
        }

        public int CreateControl(DataRow drControl, ref Object obj)
        {
            int iret = 0;
            Binding binding = null;
            try
            {

                if (drControl["Type"].ToString().Equals("Label"))
                {
                    System.Windows.Controls.Label lbl = new System.Windows.Controls.Label();
                    lbl.Name = drControl["Name"].ToString();
                    lbl.Content = drControl["Title"].ToString();
                    lbl.ToolTip = drControl["ToolTip"].ToString();
                    lbl.HorizontalAlignment = drControl["HorizontalAlignment"].ToString().Equals("Stretch") ? System.Windows.HorizontalAlignment.Stretch : System.Windows.HorizontalAlignment.Left;

                    Grid.SetColumn(lbl, LNumber.StringToInt32(drControl["GridColumn"].ToString()));
                    Grid.SetRow(lbl, LNumber.StringToInt32(drControl["GridRow"].ToString()));
                    obj = lbl;
                }
                else if (drControl["Type"].ToString().Equals("TextBox"))
                {
                    System.Windows.Controls.TextBox txt = new System.Windows.Controls.TextBox();
                    txt.Name = drControl["Name"].ToString();
                    txt.ToolTip = drControl["ToolTip"].ToString();
                    txt.HorizontalAlignment = drControl["HorizontalAlignment"].ToString().Equals("Stretch") ? System.Windows.HorizontalAlignment.Stretch : System.Windows.HorizontalAlignment.Left;

                    txt.KeyDown += new KeyEventHandler(ctl_KeyDown);
                    //txt.KeyUp += new KeyEventHandler(ctl_KeyDown);
                    txt.PreviewKeyDown += new KeyEventHandler(ctl_KeyDown);
                    //txt.PreviewKeyUp += new KeyEventHandler(ctl_KeyDown);

                    Grid.SetColumn(txt, LNumber.StringToInt32(drControl["GridColumn"].ToString()));
                    Grid.SetRow(txt, LNumber.StringToInt32(drControl["GridRow"].ToString()));
                    obj = txt;
                }
                else if (drControl["Type"].ToString().Equals("RadMaskedDateTimeInput"))
                {
                    RadMaskedDateTimeInput rdt = new RadMaskedDateTimeInput();
                    rdt.Name = drControl["Name"].ToString();
                    rdt.ToolTip = drControl["ToolTip"].ToString();
                    rdt.HorizontalAlignment = drControl["HorizontalAlignment"].ToString().Equals("Stretch") ? System.Windows.HorizontalAlignment.Stretch : System.Windows.HorizontalAlignment.Left;

                    //Binding binding = new Binding { Source = randomObject, Path = new PropertyPath("Name") };
                    //binding = new Binding
                    //{
                    //    //NotifyOnSourceUpdated = true,
                    //    ElementName = "dtpNgayThamGia",
                    //    //Mode = BindingMode.TwoWay,
                    //    Path = new PropertyPath("SelectedDate")// ,

                    //    //Source = new PropertyPath("dtpNgayThamGia"),
                    //    //UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
                    //    //Value="{Binding Path=SelectedDate,ElementName=dtpNgayHoatDong}"
                    //    //BindsDirectlyToSource = true,
                    //    //Source = DateTime.Now.AddMonths(1)
                    //    //ElementName = drControl["BindingElementName"].ToString(),
                    //    //Path = new PropertyPath(drControl["BindingPath"].ToString()),
                    //    //Mode = BindingMode.TwoWay
                    //};

                    //BindingOperations.SetBinding(rdt, RadMaskedDateTimeInput.ValueProperty, new Binding("dtpNgayThamGia") { Path = new PropertyPath("SelectedDate") });
                    //rdt.SetBinding(DatePicker.SelectedDateProperty, binding);
                    //rdt.SetBinding(RadMaskedDateTimeInput.ValueProperty, new Binding("dtpNgayThamGia"));
                    //rdt.SetValue(RadMaskedDateTimeInput.UpdateValueEventProperty, "{Binding Path=SelectedDate,ElementName=dtpNgayHoatDong}");
                    //rdt.SetValue(RadMaskedDateTimeInput.ValueProperty, "{Binding Path=SelectedDate,ElementName=dtpNgayHoatDong}");

                    rdt.Mask = drControl["FormatMask"].ToString();

                    rdt.ValueChanged += new EventHandler<Telerik.Windows.RadRoutedEventArgs>(RadMaskedDateTimeInput_ValueChanged);

                    rdt.PreviewKeyDown += new KeyEventHandler(ctl_KeyDown);
                    //rdt.PreviewKeyUp += new KeyEventHandler(ctl_KeyDown);
                    //rdt.ValueChanged += new EventHandler<Telerik.Windows.RadRoutedEventArgs>(RadMaskedDateTimeInput_ValueChanged);
                    Grid.SetColumn(rdt, LNumber.StringToInt32(drControl["GridColumn"].ToString()));
                    Grid.SetRow(rdt, LNumber.StringToInt32(drControl["GridRow"].ToString()));
                    obj = rdt;
                }
                else if (drControl["Type"].ToString().Equals("DatePicker"))
                {
                    DatePicker dtp = new DatePicker();
                    dtp.Name = drControl["Name"].ToString();
                    dtp.ToolTip = drControl["ToolTip"].ToString();
                    dtp.HorizontalAlignment = drControl["HorizontalAlignment"].ToString().Equals("Stretch") ? System.Windows.HorizontalAlignment.Stretch : System.Windows.HorizontalAlignment.Left;

                    dtp.SelectedDateChanged += new EventHandler<SelectionChangedEventArgs>(DatePicker_SelectedDateChanged);
                    dtp.IsEnabled = true;
                    dtp.SelectedDateFormat = DatePickerFormat.Short;

                    dtp.PreviewKeyDown += new KeyEventHandler(ctl_KeyDown);
                    //dtp.PreviewKeyUp += new KeyEventHandler(ctl_KeyDown);

                    Grid.SetColumn(dtp, LNumber.StringToInt32(drControl["GridColumn"].ToString()));
                    Grid.SetRow(dtp, LNumber.StringToInt32(drControl["GridRow"].ToString()));
                    obj = dtp;
                }
                else if (drControl["Type"].ToString().Equals("RadComboBox"))
                {
                    RadComboBox rcb = new RadComboBox();
                    rcb.Name = drControl["Name"].ToString();
                    rcb.ToolTip = drControl["ToolTip"].ToString();
                    rcb.HorizontalAlignment = drControl["HorizontalAlignment"].ToString().Equals("Stretch") ? System.Windows.HorizontalAlignment.Stretch : System.Windows.HorizontalAlignment.Left;
                    rcb.IsEditable = true;
                    rcb.IsTextSearchEnabled = true;
                    rcb.TextSearchMode = TextSearchMode.Contains;
                    
                    if (!LObject.IsNullOrEmpty(drControl["Datasource"]) && !drControl["Datasource"].ToString().Equals(""))
                    {
                        DataTable table = GetDatasource(drControl["Datasource"].ToString());
                        //table.Columns.RemoveAt(1);
                        //IEnumerable<DataRow> itemsource = table.AsEnumerable();
                        //rcb.ItemsSource = itemsource;

                        rcb.ItemsSource = table.DefaultView;
                        rcb.DisplayMemberPath = "VALUE";
                        rcb.SelectedValuePath = "ID";
                        rcb.SelectedIndex = 0;

                        //DataTable table = new DataTable();
                        //table.Columns.Add(new DataColumn() { ColumnName = "ID", DataType = typeof(string) });
                        //table.Columns.Add(new DataColumn() { ColumnName = "Description", DataType = typeof(string) });
                        //DataRow row = null;
                        //for (var i = 0; i < 10; i++)
                        //{
                        //    row = table.NewRow();
                        //    row["ID"] = i.ToString();
                        //    row["Description"] = "Description " + i.ToString();
                        //    table.Rows.Add(row);

                        //    rcb.Items.Insert(i, new AutoCompleteEntry(row["Description"].ToString(),row["ID"] .ToString()));
                        //}


                        //DataTable dt = GetDatasource(drControl["Datasource"].ToString());
                        //Binding bindingCMB = new Binding()
                        //{
                        //    Source = new TinhTPViewModel()
                        //};
                        //rcb.SetBinding(RadComboBox.ItemsSourceProperty, bindingCMB); //(System.Collections.IEnumerable)TinhTPViewModel;

                        //RadComboBox.TemplateProperty temp = new DependencyProperty();
                        //rcb.SetBinding(RadComboBox.ItemsSourceProperty,
                        //                new Binding
                        //                {


                        //                });
                        //rcb.SetBinding(RadComboBox.ItemsSourceProperty,
                        //                new Binding("SelectedItem")
                        //                {
                        //                    Source = (new ViewModel()).TinhTP
                        //                }
                        //              );

                        //rcb.SetBinding(RadComboBox.TextProperty,
                        //                new Binding{
                        //                                Source = (new TinhTPViewModel()).TinhTP.


                        //TinhTPViewModel isTinhTP = new TinhTPViewModel();

                        //TinhTPViewModel isTinhTP = new TinhTPViewModel();
                        //rcb.ItemsSource = (System.Collections.IEnumerable)isTinhTP;

                        //rcb.ItemsSource = new 
                        //foreach (DataRow dr in dt)
                        //{
                        //    RadComboBoxItem item = new RadComboBoxItem();
                        //    item.SetValue( = dr["VALUE"].ToString();

                        //    rcb.Items.Add(auto);
                        //}
                        //rcb.SelectedIndex = 0;

                    }


                    //rcb.SelectionChanged += new SelectionChangedEventHandler(cmb_SelectionChanged);
                    rcb.LostFocus += new RoutedEventHandler(cmb_LostFocus);
                    rcb.KeyDown += new KeyEventHandler(ctl_KeyDown);
                    rcb.PreviewKeyDown += new KeyEventHandler(ctl_KeyDown);
                    //rcb.KeyUp += new KeyEventHandler(ctl_KeyDown);

                    Grid.SetColumn(rcb, LNumber.StringToInt32(drControl["GridColumn"].ToString()));
                    Grid.SetRow(rcb, LNumber.StringToInt32(drControl["GridRow"].ToString()));
                    obj = rcb;
                }
                else
                {
                    iret = 0;
                    obj = null;
                }

                iret = 1;
            }
            catch (Exception ex)
            {
                iret = 0;
                LMessage.ShowMessage(ex.Message, LMessage.MessageBoxType.Error);
            }

            return iret;
        }

        public DataSet ReadXMLFile(string FullPath)
        {
            DataSet dsXML = new DataSet();

            dsXML.ReadXml(FullPath);

            return dsXML;
        }

        public void LoadUI()
        {
            try
            {
                string xml1 = @"
<UserControl xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
             xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
             xmlns:mc=""http://schemas.openxmlformats.org/markup-compatibility/2006""
             xmlns:d=""http://schemas.microsoft.com/expression/blend/2008""
             xmlns:telerik=""http://schemas.telerik.com/2008/xaml/presentation""
             xmlns:uc=""clr-namespace:PresentationWPF.CustomControl;assembly=PresentationWPF.CustomControl""
             xmlns:telerikMaskInput=""clr-namespace:Telerik.Windows.Controls.MaskedInput;assembly=Telerik.Windows.Controls.Input""
             xmlns:rb=""http://schemas.microsoft.com/winfx/2006/xaml/presentation/ribbon"" 
             xmlns:my=""clr-namespace:System;assembly=mscorlib"">
<GroupBox Name=""grp"" Header=""Nhóm"">
<Grid 
                Name=""gridABC"">
        <Grid.ColumnDefinitions>
            <ColumnDefinition Width=""130""/>
            <ColumnDefinition Width=""*"" />
            <ColumnDefinition Width=""10"" />
            <ColumnDefinition Width=""130""/>
            <ColumnDefinition Width=""*"" />
        </Grid.ColumnDefinitions>
        <Grid.RowDefinitions>
            <RowDefinition Height=""*"" />
            <RowDefinition Height=""6"" />
            <RowDefinition Height=""*"" />
            <RowDefinition Height=""6"" />
            <RowDefinition Height=""*"" />
            <RowDefinition Height=""6"" />
            <RowDefinition Height=""*"" />
            <RowDefinition Height=""6"" />
            <RowDefinition Height=""*"" />
            <RowDefinition Height=""6"" />
            <RowDefinition Height=""*"" />
            <RowDefinition Height=""6"" />
            <RowDefinition Height=""*"" />
            <RowDefinition Height=""6"" />
            <RowDefinition Height=""*"" />
            <RowDefinition Height=""6"" />
            <RowDefinition Height=""*"" />
        </Grid.RowDefinitions>
        <StackPanel Orientation=""Horizontal"" Grid.Row=""0"" Grid.Column=""0"">
            <Label Name=""lblChiNhanh"" />
            <Label Margin=""-5,0,0,0"" Foreground=""Red"" />
        </StackPanel>
        <telerik:RadComboBox Name=""cmbChiNhanh"" Grid.Row=""0"" Grid.Column=""1"" IsEditable=""True"" IsEnabled=""False"" />

        <StackPanel Orientation=""Horizontal"" Grid.Row=""0"" Grid.Column=""3"">
            <Label Name=""lblPhongGD"" />
        </StackPanel>
        <telerik:RadComboBox Name=""cmbPhongGD"" Grid.Row=""0"" Grid.Column=""4"" IsEditable=""True"" IsEnabled=""False"" />

        <StackPanel Orientation=""Horizontal"" Grid.Row=""2"" Grid.Column=""0"" Visibility=""Visible"">
            <Label Name=""lblKhuVuc"" />
        </StackPanel>
        <telerik:RadComboBox Name=""cmbKhuVuc"" Grid.Row=""2"" Grid.Column=""1"" IsEditable=""True"" Visibility=""Visible""/>

        <StackPanel Orientation=""Horizontal"" Grid.Row=""4"" Grid.Column=""0"">
            <Label Name=""lblMaCum"" />
            <Label Margin=""-5,0,0,0"" Foreground=""Red"" />
        </StackPanel>
        <TextBox Name=""txtMaCum"" Margin=""0,2,0,0"" Grid.Row=""4"" Grid.Column=""1"" IsEnabled=""False""/>
        <StackPanel Orientation=""Horizontal"" Grid.Row=""6"" Grid.Column=""0"">
            <Label Name=""lblTenCum""/>
            <Label Margin=""-5,0,0,0"" Foreground=""Red"" />
        </StackPanel>
        <TextBox Name=""txtTenCum"" Grid.Row=""6"" Grid.Column=""1"" Grid.ColumnSpan=""4"" Margin=""0,2,0,0"" />

        <StackPanel Orientation=""Horizontal"" Grid.Row=""8"" Grid.Column=""0"" >
            <Label Name=""lblTenTat"" />
            <Label Margin=""-5,0,0,0"" Foreground=""Red"" />
        </StackPanel>
        <TextBox Name=""txtTenTat"" Grid.Row=""8"" Grid.Column=""1"" Grid.ColumnSpan=""4"" Margin=""0,2,0,0""  />

        <StackPanel Orientation=""Horizontal"" Grid.Row=""10"" Grid.Column=""0"" >
            <Label Name=""lblNgayThanhLap"" />
            <Label Margin=""-5,0,0,0"" Foreground=""Red"" />
        </StackPanel>
        <telerik:RadMaskedDateTimeInput Name=""raddtNgayTLap"" Grid.Column=""1"" Grid.Row=""10"" Margin=""0,0,40,0"" Mask=""dd/MM/yyyy"" AcceptsReturn=""False"" HorizontalAlignment=""Stretch""/>
        <DatePicker HorizontalAlignment=""Right"" Grid.Column=""1"" Grid.Row=""10"" Margin=""0,0,0,0"" Width=""30"" AllowDrop=""True"" FirstDayOfWeek=""Monday"" Name=""dtpNgayTLap""/>

        <StackPanel Orientation=""Horizontal"" Grid.Row=""12"" Grid.Column=""0"">
            <Label Name=""lblCBQL"" />
            <Label Margin=""-5,0,0,0"" Foreground=""Red"" />
        </StackPanel>
        <telerik:RadComboBox Name=""cmbCanBoQLy"" Grid.Row=""12"" Grid.Column=""1""  IsEditable=""True""/>

        <StackPanel Orientation=""Horizontal"" Grid.Row=""14"" Grid.Column=""0"">
            <Label Name=""lblCumTruong"" />
        </StackPanel>
        <TextBox Name=""txtCumTruong"" Grid.Row=""14"" Grid.Column=""1"" Margin=""0,0,40,0"" />
        <Button Name=""btnPopupKhachHang"" Grid.Row=""14"" Grid.Column=""1"" Content=""F3"" Width=""30"" HorizontalAlignment=""Right"" />
    </Grid></GroupBox>
</UserControl>";

                XElement ui = XElement.Parse(xml1);
                DynamicContent.Content = (System.Windows.Controls.UserControl)System.Windows.Markup.XamlReader.Load(ui.CreateReader());
            }
            catch (Exception ex)
            {
                LMessage.ShowMessage(ex.Message, LMessage.MessageBoxType.Error);
            }
            //string text = "<E:Events xmlns:E=\"Event-Details\"><Date>12/27/2012</Date><Time>‎11:12 PM</Time><Message>Happy Birthday</Message></E:Events>";
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DatePicker dtp = (DatePicker)sender;
            //LMessage.ShowMessage(dtp.Text, LMessage.MessageBoxType.Information);

            if (!LObject.IsNullOrEmpty(dtp))
            {
                DataRow row = lstControl.Find(dr => dr["Name"].Equals(dtp.Name));
                if (!LObject.IsNullOrEmpty(row))
                {
                    RadMaskedDateTimeInput foundrdt = FindChild<RadMaskedDateTimeInput>(Application.Current.MainWindow, row["BindingElementName"].ToString());
                    if (!LObject.IsNullOrEmpty(foundrdt)) foundrdt.Value = dtp.SelectedDate;
                }
            }
        }

        private void RadMaskedDateTimeInput_ValueChanged(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            RadMaskedDateTimeInput rdt = (RadMaskedDateTimeInput)sender;
            if (!LObject.IsNullOrEmpty(rdt))
            {
                DataRow row = lstControl.Find(dr => dr["Name"].Equals(rdt.Name));
                if (!LObject.IsNullOrEmpty(row))
                {
                    DatePicker founddtp = FindChild<DatePicker>(Application.Current.MainWindow, row["BindingElementName"].ToString());
                    if (!LObject.IsNullOrEmpty(founddtp)) founddtp.SelectedDate = rdt.Value;
                }
            }
        }

        private void ctl_KeyDown(object sender, KeyEventArgs e)
        {
            SelectNextControl(e);
            //Control p;
            //p = ((Button)sender).Parent;
            //p.SelectNextControl(ActiveControl, true, true, true, true);
        }

        public static void SelectNextControl(KeyEventArgs e)
        {
            if (e.Key == Key.Enter || e.Key == Key.Tab)
            {
                TraversalRequest tRequest = new TraversalRequest(FocusNavigationDirection.Next);
                UIElement keyboardFocus = Keyboard.FocusedElement as UIElement;

                if (keyboardFocus != null)
                {
                    keyboardFocus.MoveFocus(tRequest);
                }

                e.Handled = true;
            }
            else if (e.Key == Key.PageUp)
            {
                TraversalRequest tRequest = new TraversalRequest(FocusNavigationDirection.Previous);
                UIElement keyboardFocus = Keyboard.FocusedElement as UIElement;

                if (keyboardFocus != null)
                {
                    keyboardFocus.MoveFocus(tRequest);
                }

                e.Handled = true;
            }
            else if (e.Key == Key.PageDown)
            {
                TraversalRequest tRequest = new TraversalRequest(FocusNavigationDirection.Next);
                UIElement keyboardFocus = Keyboard.FocusedElement as UIElement;

                if (keyboardFocus != null)
                {
                    keyboardFocus.MoveFocus(tRequest);
                }

                e.Handled = true;
            }
        }

        public static T FindChild<T>(DependencyObject parent, string childName) where T : DependencyObject
        {
            // Confirm parent and childName are valid. 
            if (parent == null) return null;

            T foundChild = null;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                // If the child is not of the request child type child
                T childType = child as T;
                if (childType == null)
                {
                    // recursively drill down the tree
                    foundChild = FindChild<T>(child, childName);

                    // If the child is found, break so we do not overwrite the found child. 
                    if (foundChild != null) break;
                }
                else if (!string.IsNullOrEmpty(childName))
                {
                    var frameworkElement = child as FrameworkElement;
                    // If the child's name is set for search
                    if (frameworkElement != null && frameworkElement.Name == childName)
                    {
                        // if the child's name is of the request name
                        foundChild = (T)child;
                        break;
                    }
                }
                else
                {
                    // child element found.
                    foundChild = (T)child;
                    break;
                }
            }

            return foundChild;
        }

        private void DatePicker_EnterKeyPressed(object sender, KeyEventArgs e)
        {
            var uie = e.OriginalSource as UIElement;

            if (e.Key == Key.Enter)
            {
                e.Handled = true;
                uie.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            }
        }

        private void RadMaskedDateTimeInput_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var dp = sender as RadMaskedDateTimeInput;
            if (dp == null) return;

            if (e.Key == Key.D && Keyboard.Modifiers == ModifierKeys.Control)
            {
                e.Handled = true;
                dp.SetValue(RadMaskedDateTimeInput.ValueProperty, DateTime.Today);
                return;
            }

            if (!dp.Value.HasValue) return;

            var date = dp.Value.Value;
            if (e.Key == Key.Up)
            {
                e.Handled = true;
                dp.SetValue(RadMaskedDateTimeInput.ValueProperty, date.AddDays(1));
            }
            else if (e.Key == Key.Down)
            {
                e.Handled = true;
                dp.SetValue(RadMaskedDateTimeInput.ValueProperty, date.AddDays(-1));
            }

            ctl_KeyDown(sender, e);
        }

        private DataTable GetDatasource(string dataSourceName)
        {
            DataTable dt = new DataTable();
            DataRow dr = null;
            //List<AutoCompleteEntry> lstSource = null;
            //if (dt == null)
            //{
            dt = new DataTable(dataSourceName);
            dt.Columns.Add("ID", typeof(string));
            dt.Columns.Add("FK", typeof(string));
            dt.Columns.Add("VALUE", typeof(string));
            //}

            if (dataSourceName.Equals("DS_GIOITINH"))
            {
                dr = dt.NewRow(); dr["ID"] = "10"; dr["FK"] = ""; dr["VALUE"] = "Nam"; dt.Rows.Add(dr);
                dr = dt.NewRow(); dr["ID"] = "11"; dr["FK"] = ""; dr["VALUE"] = "Nữ"; dt.Rows.Add(dr);
                dt.AcceptChanges();
            }
            else if (dataSourceName.Equals("DS_TINHTP"))
            {

                dr = dt.NewRow(); dr["ID"] = "10"; dr["FK"] = ""; dr["VALUE"] = "Hà nội"; dt.Rows.Add(dr);
                dr = dt.NewRow(); dr["ID"] = "11"; dr["FK"] = ""; dr["VALUE"] = "Nam định"; dt.Rows.Add(dr);
                dr = dt.NewRow(); dr["ID"] = "12"; dr["FK"] = ""; dr["VALUE"] = "Ninh Bình"; dt.Rows.Add(dr);
                dr = dt.NewRow(); dr["ID"] = "13"; dr["FK"] = ""; dr["VALUE"] = "Hà Nam"; dt.Rows.Add(dr);
                dr = dt.NewRow(); dr["ID"] = "14"; dr["FK"] = ""; dr["VALUE"] = "Bắc Ninh"; dt.Rows.Add(dr);
                dr = dt.NewRow(); dr["ID"] = "15"; dr["FK"] = ""; dr["VALUE"] = "Nghệ An"; dt.Rows.Add(dr);
                dt.AcceptChanges();
            }
            else if (dataSourceName.Equals("DS_QUANHUYEN"))
            {

                dr = dt.NewRow(); dr["ID"] = "10"; dr["FK"] = "10"; dr["VALUE"] = "Hà nội 10"; dt.Rows.Add(dr);
                dr = dt.NewRow(); dr["ID"] = "11"; dr["FK"] = "10"; dr["VALUE"] = "Hà nội 11"; dt.Rows.Add(dr);
                dr = dt.NewRow(); dr["ID"] = "12"; dr["FK"] = "10"; dr["VALUE"] = "Hà nội 12"; dt.Rows.Add(dr);
                dr = dt.NewRow(); dr["ID"] = "13"; dr["FK"] = "10"; dr["VALUE"] = "Hà nội 13"; dt.Rows.Add(dr);
                dr = dt.NewRow(); dr["ID"] = "14"; dr["FK"] = "10"; dr["VALUE"] = "Hà nội 14"; dt.Rows.Add(dr);
                dr = dt.NewRow(); dr["ID"] = "15"; dr["FK"] = "10"; dr["VALUE"] = "Hà nội 15"; dt.Rows.Add(dr);

                dr = dt.NewRow(); dr["ID"] = "16"; dr["FK"] = "11"; dr["VALUE"] = "Nam định 16"; dt.Rows.Add(dr);
                dr = dt.NewRow(); dr = dt.NewRow(); dr["ID"] = "17"; dr["FK"] = "11"; dr["VALUE"] = "Nam định 17"; dt.Rows.Add(dr);
                dr = dt.NewRow(); dr["ID"] = "18"; dr["FK"] = "11"; dr["VALUE"] = "Nam định 18"; dt.Rows.Add(dr);

                dr = dt.NewRow(); dr["ID"] = "19"; dr["FK"] = "12"; dr["VALUE"] = "Ninh Bình 19"; dt.Rows.Add(dr);
                dr = dt.NewRow(); dr["ID"] = "20"; dr["FK"] = "12"; dr["VALUE"] = "Ninh Bình 20"; dt.Rows.Add(dr);
                dr = dt.NewRow(); dr["ID"] = "21"; dr["FK"] = "12"; dr["VALUE"] = "Ninh Bình 21"; dt.Rows.Add(dr);
                dr = dt.NewRow(); dr["ID"] = "22"; dr["FK"] = "12"; dr["VALUE"] = "Ninh Bình 22"; dt.Rows.Add(dr);

                dr = dt.NewRow(); dr["ID"] = "23"; dr["FK"] = "13"; dr["VALUE"] = "Hà Nam 23"; dt.Rows.Add(dr);
                dr = dt.NewRow(); dr["ID"] = "24"; dr["FK"] = "13"; dr["VALUE"] = "Hà Nam 24"; dt.Rows.Add(dr);
                dr = dt.NewRow(); dr["ID"] = "25"; dr["FK"] = "13"; dr["VALUE"] = "Hà Nam 25"; dt.Rows.Add(dr);
                dr = dt.NewRow(); dr["ID"] = "26"; dr["FK"] = "13"; dr["VALUE"] = "Hà Nam 26"; dt.Rows.Add(dr);
                dr = dt.NewRow(); dr["ID"] = "27"; dr["FK"] = "13"; dr["VALUE"] = "Hà Nam 27"; dt.Rows.Add(dr);

                dr = dt.NewRow(); dr["ID"] = "28"; dr["FK"] = "14"; dr["VALUE"] = "Bắc Ninh 28"; dt.Rows.Add(dr);
                dr = dt.NewRow(); dr["ID"] = "29"; dr["FK"] = "14"; dr["VALUE"] = "Bắc Ninh 29"; dt.Rows.Add(dr);
                dr = dt.NewRow(); dr["ID"] = "30"; dr["FK"] = "14"; dr["VALUE"] = "Bắc Ninh 30"; dt.Rows.Add(dr);
                dr = dt.NewRow(); dr["ID"] = "31"; dr["FK"] = "14"; dr["VALUE"] = "Bắc Ninh 31"; dt.Rows.Add(dr);
                dr = dt.NewRow(); dr["ID"] = "32"; dr["FK"] = "14"; dr["VALUE"] = "Bắc Ninh 32"; dt.Rows.Add(dr);

                dr = dt.NewRow(); dr["ID"] = "33"; dr["FK"] = "15"; dr["VALUE"] = "Nghệ An 33"; dt.Rows.Add(dr);
                dr = dt.NewRow(); dr["ID"] = "34"; dr["FK"] = "15"; dr["VALUE"] = "Nghệ An 34"; dt.Rows.Add(dr);
                dr = dt.NewRow(); dr["ID"] = "35"; dr["FK"] = "15"; dr["VALUE"] = "Nghệ An 35"; dt.Rows.Add(dr);
                dr = dt.NewRow(); dr = dt.NewRow(); dr["ID"] = "36"; dr["FK"] = "15"; dr["VALUE"] = "Nghệ An 36"; dt.Rows.Add(dr);
                dr = dt.NewRow(); dr["ID"] = "37"; dr["FK"] = "15"; dr["VALUE"] = "Nghệ An 37"; dt.Rows.Add(dr);
                dr = dt.NewRow(); dr["ID"] = "38"; dr["FK"] = "15"; dr["VALUE"] = "Nghệ An 38"; dt.Rows.Add(dr);

                dt.AcceptChanges();
            }
            else if (dataSourceName.Equals("DS_PHUONGXA"))
            {
                dr = dt.NewRow();
                dr = dt.NewRow(); dr["ID"] = "10"; dr["FK"] = "10"; dr["VALUE"] = "Hà nội 10-10"; dt.Rows.Add(dr);
                dr = dt.NewRow(); dr["ID"] = "11"; dr["FK"] = "10"; dr["VALUE"] = "Hà nội 10-11"; dt.Rows.Add(dr);

                dr = dt.NewRow(); dr["ID"] = "12"; dr["FK"] = "11"; dr["VALUE"] = "Hà nội 11-12"; dt.Rows.Add(dr);
                dr = dt.NewRow(); dr["ID"] = "13"; dr["FK"] = "11"; dr["VALUE"] = "Hà nội 11-13"; dt.Rows.Add(dr);

                dr = dt.NewRow(); dr["ID"] = "14"; dr["FK"] = "12"; dr["VALUE"] = "Hà nội 12-14"; dt.Rows.Add(dr);
                dr = dt.NewRow(); dr["ID"] = "15"; dr["FK"] = "12"; dr["VALUE"] = "Hà nội 12-15"; dt.Rows.Add(dr);

                dr = dt.NewRow(); dr["ID"] = "16"; dr["FK"] = "13"; dr["VALUE"] = "Hà nội 13-16"; dt.Rows.Add(dr);
                dr = dt.NewRow(); dr["ID"] = "17"; dr["FK"] = "13"; dr["VALUE"] = "Hà nội 13-17"; dt.Rows.Add(dr);

                dr = dt.NewRow(); dr["ID"] = "18"; dr["FK"] = "14"; dr["VALUE"] = "Hà nội 14-18"; dt.Rows.Add(dr);
                dr = dt.NewRow(); dr["ID"] = "19"; dr["FK"] = "14"; dr["VALUE"] = "Hà nội 14-19"; dt.Rows.Add(dr);

                dr = dt.NewRow(); dr["ID"] = "20"; dr["FK"] = "15"; dr["VALUE"] = "Hà nội 15-20"; dt.Rows.Add(dr);
                dr = dt.NewRow(); dr["ID"] = "21"; dr["FK"] = "15"; dr["VALUE"] = "Hà nội 15-21"; dt.Rows.Add(dr);

                dr = dt.NewRow(); dr["ID"] = "22"; dr["FK"] = "16"; dr["VALUE"] = "Nam định 16-22"; dt.Rows.Add(dr);
                dr = dt.NewRow(); dr["ID"] = "23"; dr["FK"] = "16"; dr["VALUE"] = "Nam định 16-23"; dt.Rows.Add(dr);

                dr = dt.NewRow(); dr["ID"] = "171"; dr["FK"] = "17"; dr["VALUE"] = "Nam định 171"; dt.Rows.Add(dr);
                dr = dt.NewRow(); dr["ID"] = "172"; dr["FK"] = "17"; dr["VALUE"] = "Nam định 172"; dt.Rows.Add(dr);

                dr = dt.NewRow(); dr["ID"] = "181"; dr["FK"] = "18"; dr["VALUE"] = "Nam định 181"; dt.Rows.Add(dr);
                dr = dt.NewRow(); dr["ID"] = "182"; dr["FK"] = "18"; dr["VALUE"] = "Nam định 182"; dt.Rows.Add(dr);

                dr = dt.NewRow(); dr["ID"] = "191"; dr["FK"] = "19"; dr["VALUE"] = "Ninh Bình 191"; dt.Rows.Add(dr);
                dr = dt.NewRow(); dr["ID"] = "192"; dr["FK"] = "19"; dr["VALUE"] = "Ninh Bình 192"; dt.Rows.Add(dr);

                dr = dt.NewRow(); dr["ID"] = "201"; dr["FK"] = "20"; dr["VALUE"] = "Ninh Bình 201"; dt.Rows.Add(dr);
                dr = dt.NewRow(); dr["ID"] = "202"; dr["FK"] = "20"; dr["VALUE"] = "Ninh Bình 202"; dt.Rows.Add(dr);

                dr = dt.NewRow(); dr["ID"] = "211"; dr["FK"] = "21"; dr["VALUE"] = "Ninh Bình 211"; dt.Rows.Add(dr);
                dr = dt.NewRow(); dr["ID"] = "212"; dr["FK"] = "21"; dr["VALUE"] = "Ninh Bình 212"; dt.Rows.Add(dr);

                dr = dt.NewRow(); dr["ID"] = "221"; dr["FK"] = "22"; dr["VALUE"] = "Ninh Bình 221"; dt.Rows.Add(dr);
                dr = dt.NewRow(); dr["ID"] = "222"; dr["FK"] = "22"; dr["VALUE"] = "Ninh Bình 222"; dt.Rows.Add(dr);

                dr = dt.NewRow(); dr["ID"] = "231"; dr["FK"] = "23"; dr["VALUE"] = "Hà Nam 231"; dt.Rows.Add(dr);
                dr = dt.NewRow(); dr["ID"] = "232"; dr["FK"] = "23"; dr["VALUE"] = "Hà Nam 232"; dt.Rows.Add(dr);

                dr = dt.NewRow(); dr["ID"] = "241"; dr["FK"] = "24"; dr["VALUE"] = "Hà Nam 241"; dt.Rows.Add(dr);
                dr = dt.NewRow(); dr["ID"] = "242"; dr["FK"] = "24"; dr["VALUE"] = "Hà Nam 242"; dt.Rows.Add(dr);

                dr = dt.NewRow(); dr["ID"] = "251"; dr["FK"] = "25"; dr["VALUE"] = "Hà Nam 251"; dt.Rows.Add(dr);
                dr = dt.NewRow(); dr["ID"] = "252"; dr["FK"] = "25"; dr["VALUE"] = "Hà Nam 252"; dt.Rows.Add(dr);

                dr = dt.NewRow(); dr["ID"] = "261"; dr["FK"] = "26"; dr["VALUE"] = "Hà Nam 261"; dt.Rows.Add(dr);
                dr = dt.NewRow(); dr["ID"] = "262"; dr["FK"] = "26"; dr["VALUE"] = "Hà Nam 262"; dt.Rows.Add(dr);

                dr = dt.NewRow(); dr["ID"] = "271"; dr["FK"] = "27"; dr["VALUE"] = "Hà Nam 271"; dt.Rows.Add(dr);
                dr = dt.NewRow(); dr["ID"] = "272"; dr["FK"] = "27"; dr["VALUE"] = "Hà Nam 272"; dt.Rows.Add(dr);

                dr = dt.NewRow(); dr["ID"] = "281"; dr["FK"] = "28"; dr["VALUE"] = "Bắc Ninh 281"; dt.Rows.Add(dr);
                dr = dt.NewRow(); dr["ID"] = "282"; dr["FK"] = "28"; dr["VALUE"] = "Bắc Ninh 282"; dt.Rows.Add(dr);

                dr = dt.NewRow(); dr["ID"] = "291"; dr["FK"] = "29"; dr["VALUE"] = "Bắc Ninh 291"; dt.Rows.Add(dr);
                dr = dt.NewRow(); dr["ID"] = "292"; dr["FK"] = "29"; dr["VALUE"] = "Bắc Ninh 292"; dt.Rows.Add(dr);

                dr = dt.NewRow(); dr["ID"] = "301"; dr["FK"] = "30"; dr["VALUE"] = "Bắc Ninh 301"; dt.Rows.Add(dr);
                dr = dt.NewRow(); dr["ID"] = "302"; dr["FK"] = "30"; dr["VALUE"] = "Bắc Ninh 302"; dt.Rows.Add(dr);

                dr = dt.NewRow(); dr["ID"] = "311"; dr["FK"] = "31"; dr["VALUE"] = "Bắc Ninh 311"; dt.Rows.Add(dr);
                dr = dt.NewRow(); dr["ID"] = "312"; dr["FK"] = "31"; dr["VALUE"] = "Bắc Ninh 312"; dt.Rows.Add(dr);

                dr = dt.NewRow(); dr["ID"] = "321"; dr["FK"] = "32"; dr["VALUE"] = "Bắc Ninh 321"; dt.Rows.Add(dr);
                dr = dt.NewRow(); dr["ID"] = "322"; dr["FK"] = "32"; dr["VALUE"] = "Bắc Ninh 322"; dt.Rows.Add(dr);

                dr = dt.NewRow(); dr["ID"] = "331"; dr["FK"] = "33"; dr["VALUE"] = "Nghệ An 331"; dt.Rows.Add(dr);
                dr = dt.NewRow(); dr["ID"] = "332"; dr["FK"] = "33"; dr["VALUE"] = "Nghệ An 332"; dt.Rows.Add(dr);

                dr = dt.NewRow(); dr["ID"] = "341"; dr["FK"] = "34"; dr["VALUE"] = "Nghệ An 341"; dt.Rows.Add(dr);
                dr = dt.NewRow(); dr["ID"] = "342"; dr["FK"] = "34"; dr["VALUE"] = "Nghệ An 342"; dt.Rows.Add(dr);

                dr = dt.NewRow(); dr["ID"] = "351"; dr["FK"] = "35"; dr["VALUE"] = "Nghệ An 351"; dt.Rows.Add(dr);
                dr = dt.NewRow(); dr["ID"] = "352"; dr["FK"] = "35"; dr["VALUE"] = "Nghệ An 352"; dt.Rows.Add(dr);

                dr = dt.NewRow(); dr["ID"] = "361"; dr["FK"] = "36"; dr["VALUE"] = "Nghệ An 361"; dt.Rows.Add(dr);
                dr = dt.NewRow(); dr["ID"] = "362"; dr["FK"] = "36"; dr["VALUE"] = "Nghệ An 362"; dt.Rows.Add(dr);

                dr = dt.NewRow(); dr["ID"] = "371"; dr["FK"] = "37"; dr["VALUE"] = "Nghệ An 371"; dt.Rows.Add(dr);
                dr = dt.NewRow(); dr["ID"] = "372"; dr["FK"] = "37"; dr["VALUE"] = "Nghệ An 372"; dt.Rows.Add(dr);

                dr = dt.NewRow(); dr["ID"] = "381"; dr["FK"] = "38"; dr["VALUE"] = "Nghệ An 381"; dt.Rows.Add(dr);
                dr = dt.NewRow(); dr["ID"] = "382"; dr["FK"] = "38"; dr["VALUE"] = "Nghệ An 382"; dt.Rows.Add(dr);

                dt.AcceptChanges();
            }

            //lstSource = new List<AutoCompleteEntry>();
            //foreach (DataRow row in dt.Rows)
            //{
            //    lstSource.Add(new AutoCompleteEntry(row[2].ToString(), row[1].ToString(), row[0].ToString()));
            //}

            return dt;
        }

        private void cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                RadComboBox cmb = (RadComboBox)sender;
                //LMessage.ShowMessage(dtp.Text, LMessage.MessageBoxType.Information);

                if (!LObject.IsNullOrEmpty(cmb))
                {
                    DataRow rowActive = lstControl.Find(dr => dr["Name"].Equals(cmb.Name));
                    if (!LObject.IsNullOrEmpty(rowActive))
                    {
                        RadComboBox foundcmbActive = FindChild<RadComboBox>(Application.Current.MainWindow, rowActive["Name"].ToString());
                        RadComboBox foundcmbBinding = FindChild<RadComboBox>(Application.Current.MainWindow, rowActive["BindingElementName"].ToString());
                        DataRow rowBinding = lstControl.Find(dr => dr["Name"].Equals(foundcmbBinding.Name));

                        if (!LObject.IsNullOrEmpty(rowBinding))
                        {
                            DataTable table = GetDatasource(rowBinding["Datasource"].ToString());
                            DataRow[] drarray = table.Select("FK='" + foundcmbActive.SelectedValue.ToString() + "'");
                            //foundcmbBinding.Items.Remove(foundcmbBinding);
                            foundcmbBinding.ItemsSource = drarray.CopyToDataTable().DefaultView;
                            foundcmbBinding.DisplayMemberPath = "VALUE";
                            foundcmbBinding.SelectedValuePath = "ID";
                            foundcmbBinding.SelectedIndex = 0;
                        }
                        //LMessage.ShowMessage(foundcmb.SelectedValue.ToString(), LMessage.MessageBoxType.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                
                //throw ex;
            }
            
        }

        private void cmb_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                RadComboBox cmb = (RadComboBox)sender;
                //LMessage.ShowMessage(dtp.Text, LMessage.MessageBoxType.Information);

                if (!LObject.IsNullOrEmpty(cmb))
                {
                    DataRow rowActive = lstControl.Find(dr => dr["Name"].Equals(cmb.Name));
                    if (!LObject.IsNullOrEmpty(rowActive))
                    {
                        RadComboBox foundcmbActive = FindChild<RadComboBox>(Application.Current.MainWindow, rowActive["Name"].ToString());
                        RadComboBox foundcmbBinding = FindChild<RadComboBox>(Application.Current.MainWindow, rowActive["BindingElementName"].ToString());
                        DataRow rowBinding = lstControl.Find(dr => dr["Name"].Equals(foundcmbBinding.Name));

                        if (!LObject.IsNullOrEmpty(rowBinding))
                        {
                            DataTable table = GetDatasource(rowBinding["Datasource"].ToString());
                            DataRow[] drarray = table.Select("FK='" + foundcmbActive.SelectedValue.ToString() + "'");
                            //foundcmbBinding.Items.Remove(foundcmbBinding);
                            foundcmbBinding.ItemsSource = drarray.CopyToDataTable().DefaultView;
                            foundcmbBinding.DisplayMemberPath = "VALUE";
                            foundcmbBinding.SelectedValuePath = "ID";
                            foundcmbBinding.SelectedIndex = 0;
                        }
                        //LMessage.ShowMessage(foundcmb.SelectedValue.ToString(), LMessage.MessageBoxType.Information);
                    }
                }
            }
            catch (Exception ex)
            {

                LMessage.ShowMessage(ex.Message, LMessage.MessageBoxType.Error);
            }

        }
    }

    public class DanhMuc
    {
        private string ma;
        private string maTimKiem;
        private string ten;

        public DanhMuc()
        {
        }

        public DanhMuc(string Ma, string MaTimKiem, string Ten)
        {
            Ma = ma;
            MaTimKiem = maTimKiem;
            Ten = ten;
        }

        public string Ma
        {
            get
            {
                return this.ma;
            }
            set
            {
                this.ma = value;
            }
        }

        public string MaTimKiem
        {
            get
            {
                return this.maTimKiem;
            }
            set
            {
                this.maTimKiem = value;
            }
        }

        public string Ten
        {
            get
            {
                return this.ten;
            }
            set
            {
                this.ten = value;
            }
        }

        public override string ToString()
        {
            return string.Format("[DanhMuc : {0}]", this.Ten);
        }
    }

    public class TinhTPViewModel
    {
        private ObservableCollection<DanhMuc> tinhTP;

        public ObservableCollection<DanhMuc> TinhTP
        {
            get
            {
                if (tinhTP == null)
                {
                    tinhTP = new ObservableCollection<DanhMuc>();
                    tinhTP.Add(new DanhMuc("01", "", "Hà Nội"));
                    tinhTP.Add(new DanhMuc("02", "", "Hồ Chí Minh"));
                }
                return tinhTP;
            }
        }
    }

    public class ViewModel : TinhTPViewModel
    {

    }


    public class QuanHuyenViewModel
    {
        private ObservableCollection<DanhMuc> quanHuyen;

        public ObservableCollection<DanhMuc> QuanHuyen
        {
            get
            {
                if (quanHuyen == null)
                {
                    quanHuyen = new ObservableCollection<DanhMuc>();
                    quanHuyen.Add(new DanhMuc("01", "01", "Hà Nội - Quận 01"));
                    quanHuyen.Add(new DanhMuc("02", "01", "Hà Nội - Quận 02"));
                    quanHuyen.Add(new DanhMuc("03", "", "Hồ Chí Minh - Quận 03"));
                    quanHuyen.Add(new DanhMuc("04", "", "Hồ Chí Minh - Quận 04"));
                }
                return quanHuyen;
            }
        }
    }

    public class PhuongXaViewModel
    {
        private ObservableCollection<DanhMuc> phuongXa;

        public ObservableCollection<DanhMuc> PhuongXa
        {
            get
            {
                if (phuongXa == null)
                {
                    phuongXa = new ObservableCollection<DanhMuc>();
                    phuongXa.Add(new DanhMuc("01", "01", "Hà Nội - Quận 01 - Phường 01"));
                    phuongXa.Add(new DanhMuc("02", "01", "Hà Nội - Quận 01 - Phường 02"));

                    phuongXa.Add(new DanhMuc("03", "02", "Hà Nội - Quận 02 - Phường 03"));
                    phuongXa.Add(new DanhMuc("04", "02", "Hà Nội - Quận 02 - Phường 04"));

                    phuongXa.Add(new DanhMuc("05", "03", "Hồ Chí Minh - Quận 03 - Phường 05"));
                    phuongXa.Add(new DanhMuc("06", "03", "Hồ Chí Minh - Quận 03 - Phường 06"));

                    phuongXa.Add(new DanhMuc("07", "04", "Hồ Chí Minh - Quận 04 - Phường 07"));
                    phuongXa.Add(new DanhMuc("08", "04", "Hồ Chí Minh - Quận 04 - Phường 08"));
                }
                return phuongXa;
            }
        }
    }

    public class GioiTinhViewModel
    {
        private ObservableCollection<DanhMuc> gioiTinh;

        public ObservableCollection<DanhMuc> GioiTinh
        {
            get
            {
                if (gioiTinh == null)
                {
                    gioiTinh = new ObservableCollection<DanhMuc>();
                    gioiTinh.Add(new DanhMuc("01", "NAM", "Nam"));
                    gioiTinh.Add(new DanhMuc("02", "NU", "Nữ"));
                }
                return gioiTinh;
            }
        }
    }

    public class EventModel : INotifyPropertyChanged
    {
        private string stringValue;
        private double doubleValue;
        private int intValue;
        private DateTime datetimeValue;
        public EventModel()
        {
            this.DoubleValue = 125.6;
            this.IntValue = 23;
            this.StringValue = "125";
            this.DatetimeValue = DateTime.Now;
        }
        public string StringValue
        {
            get
            {
                return stringValue;
            }
            set
            {
                if (this.stringValue != value)
                {
                    this.stringValue = value;
                    this.OnPropertyChanged("StringValue");
                }
            }
        }
        public double DoubleValue
        {
            get
            {
                return doubleValue;
            }
            set
            {
                if (this.doubleValue != value)
                {
                    this.doubleValue = value;
                    this.OnPropertyChanged("DoubleValue");
                }
            }
        }
        public DateTime DatetimeValue
        {
            get
            {
                return datetimeValue;
            }
            set
            {
                if (this.datetimeValue != value)
                {
                    this.datetimeValue = value;
                    this.OnPropertyChanged("DatetimeValue");
                }
            }
        }
        public int IntValue
        {
            get
            {
                return intValue;
            }
            set
            {
                if (this.intValue != value)
                {
                    this.intValue = value;
                    this.OnPropertyChanged("IntValue");
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
