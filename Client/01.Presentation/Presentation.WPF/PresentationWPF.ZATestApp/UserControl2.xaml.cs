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
using Microsoft.Windows.Controls.Ribbon;

namespace PresentationWPF.ZATestApp
{
    /// <summary>
    /// Interaction logic for UserControl2.xaml
    /// </summary>
    public partial class UserControl2 : UserControl
    {
        public UserControl2()
        {
            InitializeComponent();
        }

        private void tbtnTest_Checked(object sender, RoutedEventArgs e)
        {
            RibbonToggleButton rb = (RibbonToggleButton)sender;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            this.Content = new ucDemo();
        }
    }
}
