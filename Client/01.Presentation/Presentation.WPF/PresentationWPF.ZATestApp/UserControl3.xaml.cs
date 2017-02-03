using System;
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
using Telerik.Windows.Controls;
using System.Data;
using Telerik.Windows.Controls.GridView;
using System.Globalization;
using System.Collections.ObjectModel;
using Microsoft.Windows.Controls.Ribbon;
using PresentationWPF.CustomControl;

namespace PresentationWPF.ZATestApp
{
    
    /// <summary>
    /// Interaction logic for UserControl3.xaml
    /// </summary>
    public partial class UserControl3 : UserControl
    {
        public static RoutedCommand ButtonClickCommand = new RoutedCommand();
        public static RoutedCommand CtrlVCommand = new RoutedCommand();
        static DataTable dt;
        static bool isLoaded = false;
        public UserControl3()
        {
            InitializeComponent();
            List<AutoCompleteEntry> lstSource = new List<AutoCompleteEntry>();
            lstSource.Add(new AutoCompleteEntry("Toyota Camry", "Toyota Camry", "camry", "car", "sedan"));
            lstSource.Add(new AutoCompleteEntry("Toyota Corolla", "Toyota Corolla", "corolla", "car", "compact"));
            lstSource.Add(new AutoCompleteEntry("Toyota Tundra", "Toyota Tundra", "tundra", "truck"));
            lstSource.Add(new AutoCompleteEntry("Chevy Impala", null));  // null matching string will default with just the name
            lstSource.Add(new AutoCompleteEntry("Chevy Tahoe", "Chevy Tahoe", "tahoe", "truck", "SUV"));
            lstSource.Add(new AutoCompleteEntry("Chevrolet Malibu", "Chevrolet Malibu", "malibu", "car", "sedan"));
            //comboBoxDiaBan = new ComboBoxDiaBan("POPUP_DS_DBAN");
            //comboBox2 = autoCombo;
            //textBox1.AddItem(new AutoCompleteEntry("Toyota Camry", "Toyota Camry", "camry", "car", "sedan"));
            //textBox1.AddItem(new AutoCompleteEntry("Toyota Corolla", "Toyota Corolla", "corolla", "car", "compact"));
            //textBox1.AddItem(new AutoCompleteEntry("Toyota Tundra", "Toyota Tundra", "tundra", "truck"));
            //textBox1.AddItem(new AutoCompleteEntry("Chevy Impala", null));  // null matching string will default with just the name
            //textBox1.AddItem(new AutoCompleteEntry("Chevy Tahoe", "Chevy Tahoe", "tahoe", "truck", "SUV"));
            //textBox1.AddItem(new AutoCompleteEntry("Chevrolet Malibu", "Chevrolet Malibu", "malibu", "car", "sedan"));

            comboBox.Items.Add(new AutoCompleteEntry("Toyota Camry", "Toyota Camry", "camry", "car", "sedan"));
            comboBox.Items.Add(new AutoCompleteEntry("Toyota Corolla", "Toyota Corolla", "corolla", "car", "compact"));
            comboBox.Items.Add(new AutoCompleteEntry("Toyota Tundra", "Toyota Tundra", "tundra", "truck"));
            comboBox.Items.Add(new AutoCompleteEntry("Chevy Impala", null));  // null matching string will default with just the name
            comboBox.Items.Add(new AutoCompleteEntry("Chevy Tahoe", "Chevy Tahoe", "tahoe", "truck", "SUV"));
            comboBox.Items.Add(new AutoCompleteEntry("Chevrolet Malibu", "Chevrolet Malibu", "malibu", "car", "sedan"));

            this.radBusyMain.IsEnabled = true;
            radBusyMain.IsBusy = true;
            UserControl_Loaded(this, null);
            KeyGesture keyg = new KeyGesture(Key.N, ModifierKeys.Control);
            KeyBinding kb = new KeyBinding(UserControl3.CtrlVCommand, keyg);
            InputBindings.Add(kb);

            KeyGesture keyg1 = new KeyGesture(Key.C, ModifierKeys.Control);
            KeyBinding kb1 = new KeyBinding(UserControl3.CtrlVCommand, keyg1);
            InputBindings.Add(kb1);

            KeyGesture keyg2 = new KeyGesture(Key.X, ModifierKeys.Control);
            KeyBinding kb2 = new KeyBinding(UserControl3.CtrlVCommand, keyg2);
            InputBindings.Add(kb2);

            KeyGesture keyg3 = new KeyGesture(Key.V, ModifierKeys.Control);
            KeyBinding kb3 = new KeyBinding(UserControl3.CtrlVCommand, keyg3);
            InputBindings.Add(kb3);
            radBusyMain.IsBusy = false;
        }
        private void radNumericUpDown1_ValueChanged(object sender, RadRangeBaseValueChangedEventArgs e)
        {
            if (isLoaded)
            {
                if (dt != null)
                {
                    pager.PageSize = (int)nudPageSize.Value;
                    GridView.ItemsSource = dt.DefaultView;
                }
            }
        }
         
        private void OnDoubleClick(object sender, Telerik.Windows.RadRoutedEventArgs e)  
        {
            GridViewCell cell = (GridViewCell)sender;
            MessageBox.Show(cell.Content.ToString());
        } 

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            this.Content = new ucDemo();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            dt = new DataTable();
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("UnitPrice", typeof(double));
            dt.Columns.Add("Date", typeof(string));
            dt.Columns.Add("Discontinued", typeof(string));
            for (int i = 0; i < 1000; i++)
            {
                dt.NewRow();
                dt.Rows.Add(new object[] { i, "Employee" + i.ToString(), i * 1000, DateTime.Today.ToShortDateString(), "Discontinued " + i.ToString() });
            }
            pager.PageSize = (int)nudPageSize.Value;
            GridView.ItemsSource = dt.DefaultView;
            //GridView.AddHandler(GridViewCell.MouseDoubleClickEvent, new EventHandler<Telerik.Windows.RadRoutedEventArgs>(this.OnDoubleClick)); 
            //new CustomControl.CustomContextMenu().CreateContextMenu(ref cmnMain, "/PresentationWPF.ZATestApp;component/UserControl3.xaml");
            //foreach (MenuItem item in cmnMain.Items)
            //{
                //item.Click += btnShortcutKey_Click;
                //item.CommandBindings.Add(new CommandBinding(MyCommand.ContextMenuCommand, ExecutedCustomCommand, CanExecuteCustomCommand));
                //if (InputBinding(item.Name) != null)
                //{
                //    CommandBinding commandBinding = new CommandBinding(ButtonClickCommand, ButtonClickCommandHandler);
                //    item.CommandBindings.Add(commandBinding);
                //    item.Command = ButtonClickCommand;
                //    InputBinding inputBinding = InputBinding(item.Name);
                //    item.InputBindings.Add(inputBinding);
                //    //GridView.InputBindings.Add(inputBinding);
                //}
            //}
            isLoaded = true;
        }
        private InputBinding InputBinding(string controlName)
        {
            if (controlName.Equals("cmtHelp"))
                return new InputBinding(ButtonClickCommand, new KeyGesture(Key.F1, ModifierKeys.None));
            else if (controlName.Equals("cmtCut"))
                return new InputBinding(ButtonClickCommand, new KeyGesture(Key.X, ModifierKeys.Control));
            else if (controlName.Equals("cmtCopy"))
                return new InputBinding(ButtonClickCommand, new KeyGesture(Key.C, ModifierKeys.Control));
            else if (controlName.Equals("cmtPaste"))
                return new InputBinding(ButtonClickCommand, new KeyGesture(Key.V, ModifierKeys.Control));
            else if (controlName.Equals("cmtClose"))
                return new InputBinding(ButtonClickCommand, new KeyGesture(Key.Escape, ModifierKeys.None));
            else if (controlName.Equals("btnShortcutKey"))
                return new InputBinding(ButtonClickCommand, new KeyGesture(Key.N, ModifierKeys.Control));
            else
                return null;
        }
        private void ButtonClickCommandHandler(object sender, ExecutedRoutedEventArgs e)
        {
            //MenuItem cmt = (MenuItem)sender;
            MessageBox.Show("OK");
        }

        private void telgrvbtnEdit_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as RadButton;
            var row = button.ParentOfType<GridViewRow>();
            if (row != null)
            {
                row.IsInEditMode = true;
                //or set its background:
                //row.Background = new SolidColorBrush(Colors.Tomato);      
            }
        }


        private void btnShortcutKey_CanExecute(object sender, CanExecuteRoutedEventArgs e)  
        {  
            e.CanExecute = true;  
        }  
 
        // Invoke the Click Routed Event by code   
        private void btnShortcutKey_Executed(object sender, ExecutedRoutedEventArgs e)  
        {
            btnShortcutKey.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));  
 
        } 
        private void btnShortcutKey_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(((MenuItem)sender).Name);
        }

        private void nudRowNum_ValueChanged(object sender, RadRangeBaseValueChangedEventArgs e)
        {
            string str = ucTThaiNVu.GetItemsSelected();
        }

        private void GridView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement originalSender = e.OriginalSource as FrameworkElement;
            if (originalSender != null)
            {
                var cell = originalSender.ParentOfType<GridViewCell>();
                if (cell != null)
                {
                    MessageBox.Show("The double-clicked cell is " + cell.Value);
                }

                var row = originalSender.ParentOfType<GridViewRow>();
                if (row != null)
                {
                    MessageBox.Show("The double-clicked row is " + row.Cells[1].ToString());
                }
            }
        }

        private void comboBox_KeyDown(object sender, KeyEventArgs e)
        {
            comboBox.IsDropDownOpen = true;
        }

    }

}
