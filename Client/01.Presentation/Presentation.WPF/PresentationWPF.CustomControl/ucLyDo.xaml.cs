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

namespace PresentationWPF.CustomControl
{
    /// <summary>
    /// Interaction logic for ucLyDo.xaml
    /// </summary>
    public partial class ucLyDo : UserControl
    {
        public List<Utilities.Common.TTHAI_LY_DO> lstLyDo = new List<Utilities.Common.TTHAI_LY_DO>();
        // khai báo 1 hàm delegate
        public delegate void LayDuLieu(List<Utilities.Common.TTHAI_LY_DO> lst);
        // khai báo 1 kiểu hàm delegate
        public LayDuLieu DuLieuTraVe;

        public ucLyDo()
        {
            InitializeComponent();
            Mouse.OverrideCursor = Cursors.Arrow;
            txtLyDoDuyet.LostFocus += new RoutedEventHandler(txtLyDoDuyet_LostFocus);
            btnLuu.Click += new RoutedEventHandler(btnLuu_Click);
            this.Unloaded += new RoutedEventHandler(ucLyDo_Unloaded);
        }

        void ucLyDo_Unloaded(object sender, RoutedEventArgs e)
        {
            lstLyDo = raddgrLyDo.ItemsSource as List<Utilities.Common.TTHAI_LY_DO>;
            DuLieuTraVe(lstLyDo);
            Mouse.OverrideCursor = Cursors.Wait;
        }

        void btnLuu_Click(object sender, RoutedEventArgs e)
        {
            if (this.Parent is Window)
                ((Window)this.Parent).Close();
        }

        void txtLyDoDuyet_LostFocus(object sender, RoutedEventArgs e)
        {
            lstLyDo.ForEach(f => f.LY_DO = txtLyDoDuyet.Text);
            raddgrLyDo.ItemsSource = lstLyDo;
            raddgrLyDo.Rebind();
        }

        public ucLyDo(List<Utilities.Common.TTHAI_LY_DO> _lstLyDo)
            : this()
        {
            lstLyDo = _lstLyDo;
            raddgrLyDo.ItemsSource = lstLyDo;
            raddgrLyDo.Rebind();
        }
    }
}
