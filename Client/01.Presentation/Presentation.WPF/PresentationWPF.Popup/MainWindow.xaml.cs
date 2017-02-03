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
using PresentationWPF.Popup.QuanTriHeThong;

namespace PresentationWPF.Popup
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            try
            {
                int i = 0;
                int a = 3 / i;
            }
            catch (Exception ex)
            {
                new frmThongBaoLoi("Lỗi chia cho 0.", ex).ShowDialog();
            }
        }
    }
}
