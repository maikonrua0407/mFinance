using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Presentation.Process.Common;
using Utilities.Common;

namespace PresentationWPF.Popup.QuanTriHeThong
{
    /// <summary>
    /// Interaction logic for frmThongBaoLoi.xaml
    /// </summary>
    public partial class frmThongBaoLoi : Window
    {
        private string _message = "";
        private Exception _exception = null;
        private bool _detail = false;

        public frmThongBaoLoi(string message, Exception exception)
        {
            InitializeComponent();
            _message = LLanguage.SearchResourceByKey(message);
            _exception = exception;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            lblMesage.Content = _message;
            if (_exception.IsNullOrEmpty())
            {
                // Ẩn nút Chi tiết nếu không có lỗi chi tiết
                btnShowDetail.Visibility = System.Windows.Visibility.Hidden;
                txtDetail.Text = _message;
            }
            else
            {
                // Hiển thị cao hơn nếu có nút chi tiết để mở rộng chi tiết không bị tràn xuống dưới
                Top = (Screen.FromHandle(new WindowInteropHelper(this).Handle).Bounds.Height - Height) / 4;
                txtDetail.Text = _message + "\r\n" + _exception.Message + "\n" + _exception.StackTrace;
            }
        }

        private void btnShowDetail_Click(object sender, RoutedEventArgs e)
        {
            Height = (_detail ? 200 : 500);
            txtDetail.Visibility = (_detail ? System.Windows.Visibility.Hidden : System.Windows.Visibility.Visible);
            btnCopy.Visibility = txtDetail.Visibility;
            _detail = !_detail;
        }

        private void btnCopy_Click(object sender, RoutedEventArgs e)
        {
            txtDetail.SelectAll();
            txtDetail.Copy();
            txtDetail.Select(0, 0);
        }
    }
}
