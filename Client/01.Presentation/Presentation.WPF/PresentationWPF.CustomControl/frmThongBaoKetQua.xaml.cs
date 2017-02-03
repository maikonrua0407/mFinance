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
using Presentation.Process.Common;

namespace PresentationWPF.CustomControl
{
    /// <summary>
    /// Interaction logic for frmThongBaoKetQua.xaml
    /// </summary>
    public partial class frmThongBaoKetQua : Window
    {
        private List<ClientResponseDetail> listClientResponseDetail;
        public List<ClientResponseDetail> ListClientResponseDetail
        {
            set {listClientResponseDetail = value;}
            get { return listClientResponseDetail; }
        }
        public frmThongBaoKetQua()
        {
            InitializeComponent();
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        //public frmThongBaoKetQua(List<ClientResponseDetail> listClientResponseDetail)
        //{
        //    InitializeComponent();
        //    this.listClientResponseDetail = listClientResponseDetail;
        //    //grThongBao.DataContext = listClientResponseDetail;
        //}

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            grThongBao.DataContext = listClientResponseDetail;
        }

        private void btnButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
