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

namespace PresentationWPF.ZATestApp.Regex
{
    /// <summary>
    /// Interaction logic for ucRegex.xaml
    /// </summary>
    public partial class ucRegex : UserControl
    {
        public ucRegex()
        {
            InitializeComponent();
            TestData data = new TestData();
            data.EmailAddress = "yo_mamma@hotbabes26.com";
            data.DateString = DateTime.Now.Date.ToShortDateString();
            data.ProductCode = "WRT.823";
            data.MiscellaneousInput = "1234567890";
            this.DataContext = data;

        }
    }
}
