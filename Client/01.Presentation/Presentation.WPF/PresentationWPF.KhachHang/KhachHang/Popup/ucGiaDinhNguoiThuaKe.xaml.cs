﻿using System;
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
using Utilities.Common;
using Presentation.Process.Common;
using PresentationWPF.CustomControl;
using System.Data;
using Telerik.Windows.Controls.GridView;

namespace PresentationWPF.KhachHang.KhachHang.Popup
{
    public partial class ucGiaDinhNguoiThuaKe : UserControl
    {
        #region Khai bao

        private DatabaseConstant.Action action = DatabaseConstant.Action.THEM;
        public DatabaseConstant.Action Action
        {
            get { return action; }
            set { action = value; }
        }        

        private DataRow _drSource = null;
        public DataRow drSource
        {
            get { return _drSource; }
            set { _drSource = value; }
        }

        private int idTuSinh = 0;
        public int idSinhMa
        {
            set { idTuSinh = value; }
        }

        public delegate void LayDuLieu(DataRow dr);

        public LayDuLieu DuLieuTraVe;

        List<AutoCompleteEntry> lstSourceDanToc = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceGioiTinh = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceMoiQuanHe = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceTrinhDoHocVan = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceNgheNghiep = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceSucKhoe = new List<AutoCompleteEntry>();        
        List<AutoCompleteEntry> lstSourceNoiCap = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceQuanHeKT = new List<AutoCompleteEntry>();
        
        #endregion

        #region Khoi tao
        public ucGiaDinhNguoiThuaKe()
        {
            InitializeComponent();
            LoadDuLieuCombobox();
            KhoiTaoDataSource();
        }

        /// <summary>
        /// Khởi tạo cấu trúc datasource
        /// </summary>
        private void KhoiTaoDataSource()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("STT", typeof(int));            
            dt.Columns.Add("VAI_TRO", typeof(string));
            dt.Columns.Add("TEN_VAI_TRO", typeof(string));
            dt.Columns.Add("HO_TEN", typeof(string));
            dt.Columns.Add("NGAY_SINH", typeof(string));
            dt.Columns.Add("GIOI_TINH", typeof(string));
            dt.Columns.Add("GIOI_TINH1", typeof(string));
            dt.Columns.Add("DAN_TOC", typeof(string));
            dt.Columns.Add("QH_VOI_TVIEN", typeof(string));
            dt.Columns.Add("QH_VOI_TVIEN1", typeof(string));
            dt.Columns.Add("TDO_HVAN", typeof(string));
            dt.Columns.Add("TDO_HVAN1", typeof(string));
            dt.Columns.Add("NGHE_NGHIEP", typeof(string));
            dt.Columns.Add("NGHE_NGHIEP1", typeof(string));
            dt.Columns.Add("SUC_KHOE", typeof(string));
            dt.Columns.Add("SO_CMND", typeof(string));
            dt.Columns.Add("NGAY_CAP", typeof(string));
            dt.Columns.Add("NOI_CAP", typeof(string));
            dt.Columns.Add("QUAN_HE_KT", typeof(string));
            dt.Columns.Add("QUAN_HE_KT1", typeof(string));
            dt.Columns.Add("CUNG_HKHAU_TVIEN", typeof(string));
            dt.Columns.Add("SO_HKHAU", typeof(string));
            dt.Columns.Add("SDT", typeof(string));
            dt.Columns.Add("DIA_CHI", typeof(string));
            dt.Columns.Add("GHI_CHU", typeof(string));

            drSource = dt.NewRow();
            
        }

        /// <summary>
        /// Load du lieu vao combobox
        /// </summary>
        private void LoadDuLieuCombobox()
        {
            COMBOBOX_DTO combo = null;
            List<string> lstDieuKien = null;
            List<COMBOBOX_DTO> lstCombobox = new List<COMBOBOX_DTO>();

            //Giới tính
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.GIOI_TINH.getValue());
            combo = new COMBOBOX_DTO();
            combo.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();
            combo.combobox = cmbGioiTinh;
            combo.lstSource = lstSourceGioiTinh;
            combo.lstDieuKien = lstDieuKien;
            lstCombobox.Add(combo);

            //Dân tộc
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.DAN_TOC.getValue());
            combo = new COMBOBOX_DTO();
            combo.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();
            combo.combobox = cmbDanToc;
            combo.lstSource = lstSourceDanToc;
            combo.lstDieuKien = lstDieuKien;
            lstCombobox.Add(combo);

            //Mối quan hệ
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.MOI_QUAN_HE.getValue());
            combo = new COMBOBOX_DTO();
            combo.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();
            combo.combobox = cmbQuanHeVoiThanhVien;
            combo.lstSource = lstSourceMoiQuanHe;
            combo.lstDieuKien = lstDieuKien;
            lstCombobox.Add(combo);

            //Nghề nghiệp
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.NGHE_NGHIEP.getValue());
            combo = new COMBOBOX_DTO();
            combo.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();
            combo.combobox = cmbNgheNghiep;
            combo.lstSource = lstSourceNgheNghiep;
            combo.lstDieuKien = lstDieuKien;
            lstCombobox.Add(combo);

            //Tình trạng sức khỏe
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.TINH_TRANG_SUC_KHOE.getValue());
            combo = new COMBOBOX_DTO();
            combo.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();
            combo.combobox = cmbTinhTrangSucKhoe;
            combo.lstSource = lstSourceSucKhoe;
            combo.lstDieuKien = lstDieuKien;
            lstCombobox.Add(combo);

            //Trình độ học vấn
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.TRINH_DO_HOC_VAN.getValue());
            combo = new COMBOBOX_DTO();
            combo.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();
            combo.combobox = cmbTrinhDoHocVan;
            combo.lstSource = lstSourceTrinhDoHocVan;
            combo.lstDieuKien = lstDieuKien;
            lstCombobox.Add(combo);

            //Quan hệ kinh tế
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.QUAN_HE_KT.getValue());
            combo = new COMBOBOX_DTO();
            combo.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();
            combo.combobox = cmbQuanHeKT;
            combo.lstSource = lstSourceQuanHeKT;
            combo.lstDieuKien = lstDieuKien;
            lstCombobox.Add(combo);

            //Nơi cấp                        
            combo = new COMBOBOX_DTO();
            combo.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_TINHTP.getValue();
            combo.combobox = cmbNoiCap;
            combo.lstSource = lstSourceNoiCap;
            lstCombobox.Add(combo);            

            AutoComboBox auto = new AutoComboBox();
            auto.GenAutoComboBoxTheoList(ref lstCombobox);
        }
        #endregion

        #region Xu ly giao dien

        /// <summary>
        /// Load du lieu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (action == DatabaseConstant.Action.THEM)
            {
                ResetForm();
            }
            else
            {
                SetFormData();
            }
        }

        /// <summary>
        /// Su kien nhan key
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            CustomControl.CommonFunction.CloseUserControl(e, this);
            CustomControl.CommonFunction.SelectNextControl(e);
        }

        /// <summary>
        /// Dong form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlbClose_Click(object sender, RoutedEventArgs e)
        {
            CustomControl.CommonFunction.CloseUserControl(this);
        }

        private void tlbSave_Click(object sender, RoutedEventArgs e)
        {
            Save();
        }

        #endregion

        #region Xu ly nghiep vu

        private void GetFormData()
        {

            if (chkNguoiThuaKe.IsChecked == true)
            {
                drSource["VAI_TRO"] = "NGUOI_THUA_KE";
                drSource["TEN_VAI_TRO"] = "Người thừa kế";
            }
            else if (chkNguoiDongTrachNhiem.IsChecked == true)
            {
                drSource["VAI_TRO"] = "NGUOI_DONG_TRACH_NHIEM";
                drSource["TEN_VAI_TRO"] = "Người đồng trách nhiệm";
            }
            else
            {
                drSource["VAI_TRO"] = "";
                drSource["TEN_VAI_TRO"] = "";
            }

            drSource["HO_TEN"] = txtHoTen.Text;
            drSource["NGAY_SINH"] = raddtNgaySinh.Text;
            if (cmbGioiTinh.SelectedIndex >= 0)
            {
                drSource["GIOI_TINH"] = lstSourceGioiTinh.ElementAt(cmbGioiTinh.SelectedIndex).KeywordStrings.ElementAt(0);
                drSource["GIOI_TINH1"] = lstSourceGioiTinh.ElementAt(cmbGioiTinh.SelectedIndex).DisplayName;
            }
            if (cmbDanToc.SelectedIndex >= 0)
                drSource["DAN_TOC"] = lstSourceDanToc.ElementAt(cmbDanToc.SelectedIndex).KeywordStrings.ElementAt(0);
            if (cmbQuanHeVoiThanhVien.SelectedIndex >= 0)
            {
                drSource["QH_VOI_TVIEN"] = lstSourceMoiQuanHe.ElementAt(cmbQuanHeVoiThanhVien.SelectedIndex).KeywordStrings.ElementAt(0);
                drSource["QH_VOI_TVIEN1"] = lstSourceMoiQuanHe.ElementAt(cmbQuanHeVoiThanhVien.SelectedIndex).DisplayName;
            }
            if (cmbTrinhDoHocVan.SelectedIndex >= 0)
            {
                drSource["TDO_HVAN"] = lstSourceTrinhDoHocVan.ElementAt(cmbTrinhDoHocVan.SelectedIndex).KeywordStrings.ElementAt(0);
                drSource["TDO_HVAN1"] = lstSourceTrinhDoHocVan.ElementAt(cmbTrinhDoHocVan.SelectedIndex).DisplayName;
            }
            if (cmbNgheNghiep.SelectedIndex >= 0)
            {
                drSource["NGHE_NGHIEP"] = lstSourceNgheNghiep.ElementAt(cmbNgheNghiep.SelectedIndex).KeywordStrings.ElementAt(0);
                drSource["NGHE_NGHIEP1"] = lstSourceNgheNghiep.ElementAt(cmbNgheNghiep.SelectedIndex).DisplayName;
            }
            if (cmbTinhTrangSucKhoe.SelectedIndex >= 0)
                drSource["SUC_KHOE"] = lstSourceSucKhoe.ElementAt(cmbTinhTrangSucKhoe.SelectedIndex).KeywordStrings.ElementAt(0);
            drSource["SO_CMND"] = txtSoCMND.Text;
            drSource["NGAY_CAP"] = raddtNgayCap.Text;
            if (cmbNoiCap.SelectedIndex >= 0)
                drSource["NOI_CAP"] = lstSourceNoiCap.ElementAt(cmbNoiCap.SelectedIndex).KeywordStrings.ElementAt(0);
            if (cmbQuanHeKT.SelectedIndex >= 0)
            {
                drSource["QUAN_HE_KT"] = lstSourceQuanHeKT.ElementAt(cmbQuanHeKT.SelectedIndex).KeywordStrings.ElementAt(0);
                drSource["QUAN_HE_KT1"] = lstSourceQuanHeKT.ElementAt(cmbQuanHeKT.SelectedIndex).DisplayName;
            }

            drSource["CUNG_HKHAU_TVIEN"] = chkKhongCungHoKhauKH.IsChecked.ToString();
            drSource["SO_HKHAU"] = txtSoHoKhau.Text;
            drSource["SDT"] = txtSoDienThoai.Text;
            drSource["DIA_CHI"] = txtDiaChi.Text;
            drSource["GHI_CHU"] = txtGhiChu.Text;
        }

        private void SetFormData()
        {
            //Thông tin chung
            if (drSource["VAI_TRO"].ToString().Equals("NGUOI_THUA_KE"))
                chkNguoiThuaKe.IsChecked = true;
            else if (drSource["VAI_TRO"].ToString().Equals("NGUOI_DONG_TRACH_NHIEM"))
                chkNguoiDongTrachNhiem.IsChecked = true;
            txtHoTen.Text = drSource["HO_TEN"].ToString();
            if (LDateTime.IsDate(drSource["NGAY_SINH"].ToString(), "dd/MM/yyyy"))
                raddtNgaySinh.Value = LDateTime.StringToDate(drSource["NGAY_SINH"].ToString(), "dd/MM/yyyy");
            cmbGioiTinh.SelectedIndex = lstSourceGioiTinh.IndexOf(lstSourceGioiTinh.FirstOrDefault(i => i.KeywordStrings.First().Equals(drSource["GIOI_TINH"])));
            cmbDanToc.SelectedIndex = lstSourceDanToc.IndexOf(lstSourceDanToc.FirstOrDefault(i => i.KeywordStrings.First().Equals(drSource["DAN_TOC"])));
            cmbQuanHeVoiThanhVien.SelectedIndex = lstSourceMoiQuanHe.IndexOf(lstSourceMoiQuanHe.FirstOrDefault(i => i.KeywordStrings.First().Equals(drSource["QH_VOI_TVIEN"])));
            cmbTrinhDoHocVan.SelectedIndex = lstSourceTrinhDoHocVan.IndexOf(lstSourceTrinhDoHocVan.FirstOrDefault(i => i.KeywordStrings.First().Equals(drSource["TDO_HVAN"])));
            cmbNgheNghiep.SelectedIndex = lstSourceNgheNghiep.IndexOf(lstSourceNgheNghiep.FirstOrDefault(i => i.KeywordStrings.First().Equals(drSource["NGHE_NGHIEP"])));
            cmbTinhTrangSucKhoe.SelectedIndex = lstSourceSucKhoe.IndexOf(lstSourceSucKhoe.FirstOrDefault(i => i.KeywordStrings.First().Equals(drSource["SUC_KHOE"])));
            txtSoCMND.Text = drSource["SO_CMND"].ToString();
            if (LDateTime.IsDate(drSource["NGAY_CAP"].ToString(), "dd/MM/yyyy"))
                raddtNgayCap.Value = LDateTime.StringToDate(drSource["NGAY_CAP"].ToString(), "dd/MM/yyyy");            
            cmbNoiCap.SelectedIndex = lstSourceNoiCap.IndexOf(lstSourceNoiCap.FirstOrDefault(i => i.KeywordStrings.First().Equals(drSource["NOI_CAP"])));
            cmbQuanHeKT.SelectedIndex = lstSourceQuanHeKT.IndexOf(lstSourceQuanHeKT.FirstOrDefault(i => i.KeywordStrings.First().Equals(drSource["QUAN_HE_KT"])));
            chkKhongCungHoKhauKH.IsChecked = Convert.ToBoolean(drSource["CUNG_HKHAU_TVIEN"].ToString());

            //Người thừa kế không cùng hộ khẩu KH
            txtSoHoKhau.Text = drSource["SO_HKHAU"].ToString();
            txtSoDienThoai.Text = drSource["SDT"].ToString();
            txtDiaChi.Text = drSource["DIA_CHI"].ToString();
            txtGhiChu.Text = drSource["GHI_CHU"].ToString();
        }

        private void ResetForm()
        {
            //Thông tin chung
            chkNguoiThuaKe.IsChecked = false;
            chkNguoiDongTrachNhiem.IsChecked = false;
            txtHoTen.Text = "";
            raddtNgaySinh.Value = null;
            cmbGioiTinh.SelectedIndex = 0;
            cmbDanToc.SelectedIndex = 0;
            cmbQuanHeVoiThanhVien.SelectedIndex = 0;
            cmbTrinhDoHocVan.SelectedIndex = 0;
            cmbNgheNghiep.SelectedIndex = 0;
            cmbTinhTrangSucKhoe.SelectedIndex = 0;
            txtSoCMND.Text = "";
            raddtNgayCap.Value = null;
            cmbNoiCap.SelectedIndex = lstSourceNoiCap.IndexOf(lstSourceNoiCap.FirstOrDefault(i => i.KeywordStrings.First().Equals(ClientInformation.TinhTPDonViGiaoDich)));
            chkKhongCungHoKhauKH.IsChecked = false;

            //Người thừa kế không cùng hộ khẩu KH
            txtSoHoKhau.Text = "";
            txtSoDienThoai.Text = "";
            txtDiaChi.Text = "";
            txtGhiChu.Text = "";
        }

        private bool Validation()
        {
            try
            {
                if (txtHoTen.Text.IsNullOrEmptyOrSpace())
                {
                    CommonFunction.ThongBaoChuaNhap(lblHoTen.Content.ToString());
                    txtHoTen.Focus();
                    return false;
                }
                if (raddtNgaySinh.Value == null || raddtNgaySinh.Text.Equals("__/__/____"))
                {
                    CommonFunction.ThongBaoChuaNhap(lblNgaySinh.Content.ToString());
                    raddtNgaySinh.Focus();
                    return false;
                }
                if (cmbGioiTinh.SelectedIndex < 0)
                {
                    CommonFunction.ThongBaoChuaChon(lblGioiTinh.Content.ToString());
                    cmbGioiTinh.Focus();
                    return false;
                }
                if (cmbQuanHeVoiThanhVien.SelectedIndex < 0)
                {
                    CommonFunction.ThongBaoChuaChon(lblQuanHeVoiThanhVien.Content.ToString());
                    cmbQuanHeVoiThanhVien.Focus();
                    return false;
                }
                //if (chkNguoiThuaKe.IsChecked == true || chkNguoiDongTrachNhiem.IsChecked == true)
                //{
                //    if (txtSoCMND.Text.IsNullOrEmptyOrSpace())
                //    {
                //        CommonFunction.ThongBaoChuaNhap(lblSoCMND.Content.ToString());
                //        txtSoCMND.Focus();
                //        return false;
                //    }
                //}

                return true;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                return false;
            }
        }

        private void Save()
        {
            if (!Validation()) return;

            GetFormData();

            if (DuLieuTraVe != null)
            {
                DuLieuTraVe(drSource);
            }

            if (action == DatabaseConstant.Action.THEM && chkThemNhieuLan.IsChecked == true)
            {
                LMessage.ShowMessage("M.DungChung.ThemThanhCong", LMessage.MessageBoxType.Information);
                ResetForm();
                txtHoTen.Focus();
            }
            else
            {
                CustomControl.CommonFunction.CloseUserControl(this);
            }            
        }        
        #endregion
    }
}