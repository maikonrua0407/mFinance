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
using System.ComponentModel;

namespace PresentationWPF.CustomControl
{
    /// <summary>
    /// Interaction logic for ucComboBoxNguonDL.xaml
    /// </summary>
    public partial class ucComboBoxNguonDL : UserControl
    {
        static List<AutoCompleteEntry> lstSource=new List<AutoCompleteEntry>();
        private string ma;

        public string Ma
        {
            get { return ma; }
            set { ma = value; }
        }
        public ucComboBoxNguonDL()
        {
            InitializeComponent();
            if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
                return;
            lstSource.Add(new AutoCompleteEntry("Người sử dụng", "NSD"));
            lstSource.Add(new AutoCompleteEntry("Hệ thống", "HTH"));
            foreach (AutoCompleteEntry auto in lstSource)
            {
                comboBox.Items.Add(auto);
            }
        }
        private void comboBox_KeyDown(object sender, KeyEventArgs e)
        {
            comboBox.IsDropDownOpen = true;
        }
        public void setMa(string maChon)
        {
            comboBox.SelectedIndex = lstSource.IndexOf(lstSource.FirstOrDefault(i => i.KeywordStrings.First().Equals(maChon)));
        }
        private void comboBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!lstSource.Select(i => i.DisplayName).Contains(comboBox.Text))
            {
                MessageBox.Show("Mã không đúng, hãy nhập lại");
                this.Focus();
            }
            else
                Ma = lstSource.ElementAt(comboBox.SelectedIndex).KeywordStrings.First();
        }
    }
}
