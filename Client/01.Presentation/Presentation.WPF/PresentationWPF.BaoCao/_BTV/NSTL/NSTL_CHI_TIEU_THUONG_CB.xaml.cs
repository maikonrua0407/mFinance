﻿using System;
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
using Microsoft.Windows.Controls.Ribbon;
using Utilities.Common;
using Presentation.Process;
using Presentation.Process.PopupServiceRef;
using Presentation.Process.Common;
using PresentationWPF.CustomControl;
using PresentationWPF.BaoCao.DungChung;


namespace PresentationWPF.BaoCao._BTV.NSTL
{
    /// <summary>
    /// Interaction logic for NSTL_CHI_TIEU_THUONG_CB.xaml
    /// </summary>
    public partial class NSTL_CHI_TIEU_THUONG_CB : UserControl
    {
        #region Khai bao
        List<AutoCompleteEntry> lstSourceChiNhanh = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourcePhongGD = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourcePhongGD_Select = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceNgonNgu = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceDinhDang = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceLoaiTien = new List<AutoCompleteEntry>();

        public string machinhanh;
        public string maphonggd;
        public string tungay;
        public string denngay;
        public string ngaybaocao;
        public string maloaitien;
        List<string> lstDieuKien = new List<string>();
        public string madinhdang;
        public string mangonngu;
        public string thangtinhluong;
        #endregion

        #region Khoi tao
        public NSTL_CHI_TIEU_THUONG_CB()
        {
            InitializeComponent();
            LoadCombobox();
            if (ClientInformation.LoaiNguoiSuDung.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_QTDV.layGiaTri()) ||
                ClientInformation.LoaiNguoiSuDung.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_NVDV.layGiaTri()))
            {
                cmbChiNhanh.IsEnabled = false;
            }
            else
            {
                cmbChiNhanh.IsEnabled = true;
            }
            raddtDenNgay.Value = raddtNgayBaoCao.Value = raddtTuNgay.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);
        }

        private void LoadCombobox()
        {
            // khởi tạo combobox
            AutoComboBox auto = new AutoComboBox();
            // Khởi tạo điều kiện gọi danh mục
            List<string> lstDieuKien = new List<string>();

            // khởi tạo combobox
            auto = new AutoComboBox();
            auto.GenAutoComboBox(ref lstSourceChiNhanh, ref cmbChiNhanh, DatabaseConstant.DanhSachTruyVan.COMBOBOX_CNHANH.getValue(), null, ClientInformation.MaDonVi);

            // khởi tạo combobox
            auto = new AutoComboBox();
            auto.GenAutoComboBox(ref lstSourcePhongGD, ref cmbPhongGD, DatabaseConstant.DanhSachTruyVan.COMBOBOX_PHONG_GD.getValue());
            LoadComboboxPhongGD();

            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.DONG_TIEN_CAC_NUOC.getValue());
            // khởi tạo combobox
            auto = new AutoComboBox();
            auto.GenAutoComboBox(ref lstSourceLoaiTien, ref cmbLoaiTien, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ClientInformation.MaDongNoiTe);
            cmbLoaiTien.IsEnabled = false;

            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.NGON_NGU.getValue());
            // khởi tạo combobox
            auto = new AutoComboBox();
            auto.GenAutoComboBox(ref lstSourceNgonNgu, ref cmbNgonNgu, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ApplicationConstant.LoaiNgonNguBaoCao.vi_VN.layGiaTri());

            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.DINH_DANG_BAO_CAO.getValue());
            // khởi tạo combobox
            auto = new AutoComboBox();
            auto.GenAutoComboBox(ref lstSourceDinhDang, ref cmbDinhDang, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, ApplicationConstant.LoaiDinhDangBaoCao.PDF.layGiaTri());
        }

        private void LoadComboboxPhongGD()
        {
            // khởi tạo combobox
            AutoComboBox auto = new AutoComboBox();

            lstSourcePhongGD_Select = new List<AutoCompleteEntry>();
            string maChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.First();
            lstSourcePhongGD_Select = lstSourcePhongGD.Where(e => e.KeywordStrings.ElementAt(1).Equals(maChiNhanh)).ToList();

            // khởi tạo combobox
            auto = new AutoComboBox();
            cmbPhongGD.Items.Clear();
            auto.GenAutoComboBox(ref lstSourcePhongGD_Select, ref cmbPhongGD, null);
            cmbPhongGD.SelectedIndex = 0;
            cmbPhongGD.IsEnabled = !maChiNhanh.Equals("%");
        }

        private void LoadDuLieu()
        {
            if (raddtTuNgay.Value == null || raddtDenNgay == null) return;

            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                machinhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.FirstOrDefault();
                maphonggd = lstSourcePhongGD_Select.ElementAt(cmbPhongGD.SelectedIndex).KeywordStrings.FirstOrDefault();
                tungay = Convert.ToDateTime(raddtTuNgay.Value).ToString(ApplicationConstant.defaultDateTimeFormat);
                denngay = Convert.ToDateTime(raddtDenNgay.Value).ToString(ApplicationConstant.defaultDateTimeFormat);
                BaoCaoProcess bcProcess = new BaoCaoProcess();
                DataSet ds = new DataSet();
                ds = bcProcess.LayChiTieuThuongCanBo(machinhanh, maphonggd, tungay, denngay);
                if (ds != null && ds.Tables.Count > 0)
                {
                    grid.DataContext = ds.Tables[0].DefaultView;
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }

        }
        #endregion

        #region Xu ly giao dien
        private void cmbChiNhanh_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbChiNhanh.SelectedIndex == -1) return;
            LoadComboboxPhongGD();
        }

        private void cmbPhongGD_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbPhongGD.SelectedIndex == -1) return;
            LoadDuLieu();
        }
        #endregion

        #region Xu ly nghiep vu

        private void GetFormData()
        {
            lstDieuKien = new List<string>();
            string dieukien = "";
            machinhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.FirstOrDefault();
            maphonggd = lstSourcePhongGD_Select.ElementAt(cmbPhongGD.SelectedIndex).KeywordStrings.FirstOrDefault();
            maloaitien = lstSourceLoaiTien.ElementAt(cmbLoaiTien.SelectedIndex).KeywordStrings.FirstOrDefault();
            tungay = Convert.ToDateTime(raddtTuNgay.Value).ToString(ApplicationConstant.defaultDateTimeFormat);
            denngay = Convert.ToDateTime(raddtDenNgay.Value).ToString(ApplicationConstant.defaultDateTimeFormat);
            ngaybaocao = Convert.ToDateTime(raddtNgayBaoCao.Value).ToString(ApplicationConstant.defaultDateTimeFormat);

            foreach (DataRowView drv in grid.Items)
            {
                dieukien = drv["STT"].ToString() + "@" +
                        drv["MA_HSO"].ToString() + "@" +
                        drv["TEN_HSO"].ToString() + "@" +
                        drv["MA_CHUC_VU"].ToString() + "@" +
                        drv["TEN_CHUC_VU"].ToString() + "@" +
                        drv["DU_NO_RUI_RO_KH"] + "@" +
                        drv["TY_LE_BEN_VUNG_KH"] + "@" +
                        drv["SO_KH_TANG_KH"] + "@" +
                        drv["DU_NO_BINH_QUAN_KH"] + "@" +
                        drv["DU_NO_RUI_RO"] + "@" +
                        drv["TY_LE_BEN_VUNG"] + "@" +
                        drv["SO_KH_TANG"] + "@" +
                        drv["DU_NO_BINH_QUAN"] + "@" +
                        drv["DU_NO_RUI_RO_STIEN"] + "@" +
                        drv["TY_LE_BEN_VUNG_STIEN"] + "@" +
                        drv["SO_KH_TANG_STIEN"] + "@" +
                        drv["DU_NO_BINH_QUAN_STIEN"] + "@" +
                        drv["TONG"] + "@" +
                        drv["GHI_CHU"].ToString() + "@";

                lstDieuKien.Add(dieukien);
            }
        }

        private bool Validation()
        {
            if (raddtTuNgay.Value > raddtDenNgay.Value)
            {
                LMessage.ShowMessage("M.ResponseMessage.BaoCao.TuNgayLonHonDenNgay", LMessage.MessageBoxType.Warning);
                raddtTuNgay.Focus();
                return false;
            }
            if (grid.SelectedItems.Count == 0)
            {
                LMessage.ShowMessage("M.DungChung.ChuaChonBanGhi", LMessage.MessageBoxType.Warning);
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
            listThamSoBaoCao.Add(new ThamSoBaoCao("@TuNgay", tungay, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@DenNgay", denngay, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@MaChiNhanh", machinhanh, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@MaPhongGD", maphonggd, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@MaLoaiTien", maloaitien, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            listThamSoBaoCao.Add(new ThamSoBaoCao("@NgayBaoCao", ngaybaocao, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));

            for (int i = 0; i < lstDieuKien.Count; i++)
            {
                listThamSoBaoCao.Add(new ThamSoBaoCao("@DieuKien", lstDieuKien[i], ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            }

            return listThamSoBaoCao;
        }
        #endregion

        private void grid_CellEditEnded(object sender, Telerik.Windows.Controls.GridViewCellEditEndedEventArgs e)
        {
            try
            {
                DataRowView dr = (DataRowView)grid.CurrentCellInfo.Item;
                dr["TONG"] = Convert.ToDecimal(dr["DU_NO_RUI_RO_STIEN"]) + Convert.ToDecimal(dr["TY_LE_BEN_VUNG_STIEN"]) + Convert.ToDecimal(dr["SO_KH_TANG_STIEN"]) + Convert.ToDecimal(dr["DU_NO_BINH_QUAN_STIEN"]);
                grid.CurrentItem = dr;

            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        private void tlbLayDuLieu_Click(object sender, RoutedEventArgs e)
        {
            LoadDuLieu();
        }

        private void tlbBaoCaoTheThuong_Click(object sender, RoutedEventArgs e)
        {
            if (!Validation())
            {
                return;
            }

            DataRowView drv = (DataRowView)grid.SelectedItem;

            machinhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.FirstOrDefault();
            maphonggd = lstSourcePhongGD_Select.ElementAt(cmbPhongGD.SelectedIndex).KeywordStrings.FirstOrDefault();
            maloaitien = lstSourceLoaiTien.ElementAt(cmbLoaiTien.SelectedIndex).KeywordStrings.FirstOrDefault();
            tungay = Convert.ToDateTime(raddtTuNgay.Value).ToString(ApplicationConstant.defaultDateTimeFormat);
            denngay = Convert.ToDateTime(raddtDenNgay.Value).ToString(ApplicationConstant.defaultDateTimeFormat);
            ngaybaocao = Convert.ToDateTime(raddtNgayBaoCao.Value).ToString(ApplicationConstant.defaultDateTimeFormat);
            string maCanBo = drv["MA_HSO"].ToString();
            string tenCanBo = drv["TEN_HSO"].ToString();
            string chucVu = drv["MA_CHUC_VU"].ToString();
            string duNoRuiRo = Convert.ToDecimal(drv["DU_NO_RUI_RO"]).ToString();
            string duNoRuiRoSoTien = Convert.ToDecimal(drv["DU_NO_RUI_RO_STIEN"]).ToString();
            string tyLeBenVung = Convert.ToDecimal(drv["TY_LE_BEN_VUNG_STIEN"]).ToString();
            string soKHTang = Convert.ToDecimal(drv["SO_KH_TANG_STIEN"]).ToString();
            string duNoBinhQuan = Convert.ToDecimal(drv["DU_NO_BINH_QUAN"]).ToString();
            string duNoBinhQuanSoTien = Convert.ToDecimal(drv["DU_NO_BINH_QUAN_STIEN"]).ToString();
            string tong = Convert.ToDecimal(drv["TONG"]).ToString();

            List<ThamSoBaoCao> lstThamSo = new List<ThamSoBaoCao>();
            lstThamSo.Add(new ThamSoBaoCao("@MaChiNhanh", machinhanh, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("@MaPhongGD", maphonggd, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("@TuNgay", tungay, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("@DenNgay", tungay, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("@NgayBaoCao", ngaybaocao, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("TuNgay", tungay, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("DenNgay", denngay, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));            
            lstThamSo.Add(new ThamSoBaoCao("@MaCanBo", maCanBo, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("@TenCanBo", tenCanBo, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("@ChucVu", chucVu, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("@DuNoRuiRo", duNoRuiRo, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("@DuNoRuiRoSoTien", duNoRuiRoSoTien, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("@TyLeBenVung", tyLeBenVung, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("@SoKHTang", soKHTang, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("@DuNoBinhQuan", duNoBinhQuan, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("@DuNoBinhQuanSoTien", duNoBinhQuanSoTien, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
            lstThamSo.Add(new ThamSoBaoCao("@Tong", tong, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));

            string maBaoCao = DatabaseConstant.LayMaBaoCaoBTV(DatabaseConstant.DanhSachBaoCaoBTV.NSTL_THE_THUONG);

            VanHanhGiaoDich xemBaoCao = new VanHanhGiaoDich();
            xemBaoCao.LayDuLieu(maBaoCao, lstThamSo);
        }

        
    }
}
