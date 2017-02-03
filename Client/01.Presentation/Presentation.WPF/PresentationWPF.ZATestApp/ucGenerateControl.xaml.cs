using System;
using System.IO;
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
using System.Windows.Markup;
using System.Xml;
using System.Reflection;
using Telerik.Windows;

namespace PresentationWPF.ZATestApp
{
    /// <summary>
    /// Interaction logic for ucGenerateControl.xaml
    /// </summary>
    public partial class ucGenerateControl : UserControl
    {
        public ucGenerateControl()
        {
            InitializeComponent();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            Person obj = new Person();
            obj.FirstName = "sujeet";
            CreateControlsUsingObjects(obj);
        }

        private void CreateControlsUsingObjects(Person obj)
        {
            List<Person> objList = new List<Person>();

            objList.Add(obj);
            Grid rootGrid = new Grid();
            rootGrid.Margin = new Thickness(10.0);

            rootGrid.ColumnDefinitions.Add(
               new ColumnDefinition() { Width = new GridLength(100.0) });
            rootGrid.ColumnDefinitions.Add(
                 new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });

            rootGrid.ColumnDefinitions.Add(
                new ColumnDefinition() { Width = new GridLength(100.0) });
            rootGrid.ColumnDefinitions.Add(
                   new ColumnDefinition() { Width = new GridLength(100.0) });


            PropertyInfo[] propertyInfos;
            propertyInfos = typeof(Person).GetProperties();
            rootGrid.RowDefinitions.Add(CreateRowDefinition());
            int j = 1;

            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                if (propertyInfo.PropertyType.Name == "String")
                {

                    rootGrid.RowDefinitions.Add(CreateRowDefinition());

                    var Label = CreateTextBlock(propertyInfo.Name, j, 0);
                    rootGrid.Children.Add(Label);

                    var Textbox = CreateTextBox(j, 1);
                    rootGrid.Children.Add(Textbox);
                    j++;
                }
                if (propertyInfo.PropertyType.Name == "Boolean")
                {
                    rootGrid.RowDefinitions.Add(CreateRowDefinition());

                    var Label = CreateTextBlock(propertyInfo.Name, j, 0);
                    rootGrid.Children.Add(Label);

                    var Textbox = CreateCheckBox(j, 1);
                    rootGrid.Children.Add(Textbox);
                    j++;
                }



            }
            rootGrid.RowDefinitions.Add(CreateRowDefinition());
            var Button = CreateButton("Save", j + 1, 1);
            Button.Click += new RoutedEventHandler(button_Click);

            rootGrid.Children.Add(Button);
            LayoutRoot.Children.Add(rootGrid);



        }

        void button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Saved Successfully");
        }

        private Button CreateButton(string text, int row, int column)
        {
            Button tb = new Button() { Content = text, VerticalAlignment = VerticalAlignment.Top, HorizontalAlignment = HorizontalAlignment.Left, Margin = new Thickness(5, 8, 0, 5) };
            tb.Width = 90;
            tb.Height = 25;
            tb.Margin = new Thickness(5);
            Grid.SetColumn(tb, column);
            Grid.SetRow(tb, row);
            return tb;
        }

        private Telerik.Windows.Controls.RadTabItem CreateTabItemControl()
        {
            Telerik.Windows.Controls.RadTabItem tabItem = new Telerik.Windows.Controls.RadTabItem();
            tabItem.Name = "";

            return tabItem;
        }

        private TextBlock CreateTextBlock(string text, int row, int column)
        {
            string[] aa = BreakUpperCB(text);
            string prop = "";
            for (int i = 0; i < aa.Length; i++)
            {
                prop = prop + " " + aa[i];
            }
            TextBlock tb = new TextBlock() { Text = prop, Margin = new Thickness(5, 8, 0, 5) };
            tb.MinWidth = 90;
            //tb.FontWeight
            tb.FontWeight = FontWeights.Bold;
            //tb.FontStyle=new System.Windows.FontStyle(){ FontWeight="Bold" ,  
            tb.Margin = new Thickness(5);
            var bc = new BrushConverter();
            tb.Foreground = (Brush)bc.ConvertFrom("#FF2D72BC");
            Grid.SetColumn(tb, column);
            Grid.SetRow(tb, row);
            return tb;
        }

        private TextBox CreateTextBox(int row, int column)
        {
            TextBox tb = new TextBox();
            tb.Margin = new Thickness(5);
            tb.Height = 22;
            tb.Width = 150;

            Grid.SetColumn(tb, column);
            Grid.SetRow(tb, row);

            return tb;
        }


        private CheckBox CreateCheckBox(int row, int column)
        {
            CheckBox cb = new CheckBox();
            cb.Margin = new Thickness(5);
            cb.Height = 22;
            cb.MinWidth = 50;

            Grid.SetColumn(cb, column);
            Grid.SetRow(cb, row);

            return cb;
        }

        private RowDefinition CreateRowDefinition()
        {
            RowDefinition RowDefinition = new RowDefinition();
            RowDefinition.Height = GridLength.Auto;
            return RowDefinition;
        }


        public string[] BreakUpperCB(string sInput)
        {
            StringBuilder[] sReturn = new StringBuilder[1];
            sReturn[0] = new StringBuilder(sInput.Length);
            const string CUPPER = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            int iArrayCount = 0;
            for (int iIndex = 0; iIndex < sInput.Length; iIndex++)
            {
                string sChar = sInput.Substring(iIndex, 1); // get a char
                if ((CUPPER.Contains(sChar)) && (iIndex > 0))
                {
                    iArrayCount++;
                    System.Text.StringBuilder[] sTemp = new System.Text.StringBuilder[iArrayCount + 1];
                    Array.Copy(sReturn, 0, sTemp, 0, iArrayCount);
                    sTemp[iArrayCount] = new StringBuilder(sInput.Length);
                    sReturn = sTemp;
                }
                sReturn[iArrayCount].Append(sChar);
            }
            string[] sReturnString = new string[iArrayCount + 1];
            for (int iIndex = 0; iIndex < sReturn.Length; iIndex++)
            {
                sReturnString[iIndex] = sReturn[iIndex].ToString();
            }
            return sReturnString;
        }



    }

    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool isDefault { get; set; }
        public string EmailId { get; set; }
        public string EmployeeNo { get; set; }
        public string Age { get; set; }

        public string EmailId2 { get; set; }
        public bool isMale { get; set; }
        public string MobileNo { get; set; }
        public string TelephoneNo { get; set; }
    }
}
