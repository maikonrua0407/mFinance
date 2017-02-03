using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Utilities.Common;
using PresentationWPF.CustomControl;
using PresentationWPF.BaoCao.DungChung;
using Presentation.Process;
using Presentation.Process.Common;

namespace PresentationWPF.BaoCao._BTV.NSTL
{
    /// <summary>
    /// Interaction logic for NSTL_HDLD_CHINH_THUC.xaml
    /// </summary>
    public partial class NSTL_HDLD_CHINH_THUC : UserControl
    {
        #region Khai bao
        private string soHopDong = "";        
        #endregion

        public NSTL_HDLD_CHINH_THUC()
        {
            InitializeComponent();
        }

        private void GetFormData()
        {
            soHopDong = txtSoHopDong.Text;            
        }

        private bool Validation()
        {
            if (txtSoHopDong.Text.IsNullOrEmptyOrSpace())
            {
                CommonFunction.ThongBaoChuaNhap(lblSoHopDong.Content.ToString());
                txtSoHopDong.Focus();
                return false;
            }            
            return true;
        }

        public List<ThamSoBaoCao> GetParameters()
        {
            if (!Validation())
            {
                return null;
            }

            GetFormData();

            List<ThamSoBaoCao> listThamSoBaoCao = new List<ThamSoBaoCao>();
            listThamSoBaoCao.Add(new ThamSoBaoCao("P_NgayLamViec", ClientInformation.NgayLamViecHienTai, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@HD_ID", soHopDong, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));            

            return listThamSoBaoCao;
        }
    }
}
