using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Windows.Data;
using System.Globalization;
using System.Windows.Controls;
using System;
using System.Windows;

namespace PresentationWPF.ZATestApp
{
    [Category("Accordion")]
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        public UserControl1()
        {
            InitializeComponent();

        }

        private void AccordionItem_Selected(object sender, RoutedEventArgs e)
        {
            AccordionItem item = (AccordionItem)sender;
            lbAlert.Content = item.Header;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            this.Content = new ucDemo();
        }
    }
}
