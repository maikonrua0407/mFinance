using System;
using System.Collections.Generic;
using System.IO;
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

namespace PresentationWPF.ZATestApp.Tainm
{
    /// <summary>
    /// Interaction logic for ucCopy.xaml
    /// </summary>
    public partial class ucCopy : UserControl
    {
        public ucCopy()
        {
            InitializeComponent();
            btnCopy.Click += BtnCopy_Click;
        }

        private void BtnCopy_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FileInfo fileInfo = new FileInfo(@"C:\Program Files (x86)\NGV GROUP\NGV BACKUP 2.0\temp\backup\F-NG-eFUND-20160119-191700.bak");
                if (fileInfo != null)
                {
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
