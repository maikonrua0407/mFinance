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

namespace PresentationWPF.BaoCao._BTV.HDVO
{
    /// <summary>
    /// Interaction logic for HDVO_GIAY_GUI_TIEN.xaml
    /// </summary>
    public partial class HDVO_GIAY_GUI_TIEN : UserControl
    {
        #region Khai bao
        private string soSoTG = "";
        private string maGiaoDich = "";
        #endregion

        public HDVO_GIAY_GUI_TIEN()
        {
            InitializeComponent();
        }

        private void GetFormData()
        {
            soSoTG = txtSoSoTG.Text;
            maGiaoDich = txtMaGiaoDich.Text;
        }

        private bool Validation()
        {
            if (txtSoSoTG.Text.IsNullOrEmptyOrSpace())
            {
                CommonFunction.ThongBaoChuaNhap(lblSoSoTG.Content.ToString());
                txtSoSoTG.Focus();
                return false;
            }
            if (txtMaGiaoDich.Text.IsNullOrEmptyOrSpace())
            {
                CommonFunction.ThongBaoChuaNhap(lblMaGiaoDich.Content.ToString());
                txtMaGiaoDich.Focus();
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
            listThamSoBaoCao.Add(new ThamSoBaoCao("@SoSoTG", soSoTG, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@MaGiaoDich", maGiaoDich, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));           

            return listThamSoBaoCao;
        }
    }
}
