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

namespace PresentationWPF.BaoCao._HVNH.GDKT
{
    /// <summary>
    /// Interaction logic for HDVO_GIAY_GUI_TIEN.xaml
    /// </summary>
    public partial class GDKT_NX_NGOAI_BANG : UserControl
    {
        #region Khai bao
        private string soSoTG = "";
        private string maGiaoDich = "";
        private List<AutoCompleteEntry> lstSourceLoaiChungTu = new List<AutoCompleteEntry>();
        #endregion

        #region Khoi tao
        public GDKT_NX_NGOAI_BANG()
        {
            InitializeComponent();
            KhoiTaoCombobox();
        }

        private void KhoiTaoCombobox()
        {
            AutoComboBox auto = new AutoComboBox();
            List<string> lstDK = new List<string>();
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                //Combobox loại chứng từ
                auto.GenAutoComboBox(ref lstSourceLoaiChungTu, ref cbbLoaiCT, DatabaseConstant.DanhSachTruyVan.COMBOBOX_LOAICHUNGTU_NGOAIBANG.getValue(), lstDK);
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiLoadDuLieu", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }
        #endregion

        #region Xu ly nghiep vu
        private void GetFormData()
        {
            maGiaoDich = txtMaGiaoDich.Text;
        }

        private bool Validation()
        {
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
            listThamSoBaoCao.Add(new ThamSoBaoCao("@MaGiaoDich", maGiaoDich, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@TenChiNhanh", ClientInformation.TenDonVi, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@TenBaoCao", cbbLoaiCT.Text, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("MaNgonNgu", ClientInformation.NgonNgu, ApplicationConstant.LoaiThamSoBaoCao.LANG.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("MaDinhDang", ApplicationConstant.LoaiDinhDangBaoCao.WORD.layGiaTri(), ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri()));

            return listThamSoBaoCao;
        }
        #endregion
    }
}
