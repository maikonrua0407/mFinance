using Presentation.Process;
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

namespace PresentationWPF.ZATestApp._TEST
{
    /// <summary>
    /// Interaction logic for ucTEST.xaml
    /// </summary>
    public partial class ucTEST : UserControl
    {
        public ucTEST()
        {
            InitializeComponent();
        }

        private void btnTEST_Click(object sender, RoutedEventArgs e)
        {
            ZATestAppProcess process = new ZATestAppProcess();
            //string ret = process._TEST();
        }
    }
}
