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
using System.Windows.Shapes;
using System.Data;
using Presentation.Process.Common;

namespace PresentationWPF.CustomControl
{
    /// <summary>
    /// Interaction logic for frmThongBaoKetQuaLoi.xaml
    /// </summary>
    public partial class frmThongBaoKetQuaLoi : Window
    {
        public DataTable dtKetQua { get; set; }
        
        public frmThongBaoKetQuaLoi()
        {
            InitializeComponent();
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            grThongBao.ItemsSource = dtKetQua.DefaultView;
        }

        private void btnButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
