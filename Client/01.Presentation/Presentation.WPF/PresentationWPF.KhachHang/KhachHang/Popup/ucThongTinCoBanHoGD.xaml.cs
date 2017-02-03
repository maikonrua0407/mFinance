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
using Utilities.Common;
using PresentationWPF.CustomControl;
using System.Data;
using Telerik.Windows.Controls.GridView;

namespace PresentationWPF.KhachHang.KhachHang.Popup
{
    public partial class ucThongTinCoBanHoGD : UserControl
    {
        #region Khai bao
        List<AutoCompleteEntry> lstSourceGioiTinh = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceMoiQuanHe = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceSucKhoe = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceLoaiGiayTo = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceNoiCap = new List<AutoCompleteEntry>();

        private int idKhachHang = -1;
        private DataTable _dtSource = null;
        public DataTable dtSource
        {
            get { return _dtSource; }
            set { _dtSource = value; }
        }

        private int idTuSinh = 0;
        public int idSinhMa
        {
            set { idTuSinh = value; }
        }
        private bool isUpdate = false;
        #endregion

        #region Khoi tao
        public ucThongTinCoBanHoGD()
        {
            InitializeComponent();
            //raddtNgaySinh.Value = LDateTime.GetCurrentDate();
            //dtpNgaySinh.SelectedDate = LDateTime.GetCurrentDate();
            raddtNgayCap.Value = null;
            chkNguoiThuaKe_Unchecked(null, null);
            LoadDuLieuCombobox();
            KhoiTaoDataSource();
            ResetForm();
        }

        /// <summary>
        /// Khởi tạo cấu trúc datasource
        /// </summary>
        private void KhoiTaoDataSource()
        {
            Presentation.Process.KhachHangProcess process = new Presentation.Process.KhachHangProcess();
            DataSet ds = process.getThongTinKhac("VKH_GIA_DINH");
            if (ds != null && ds.Tables.Count > 0)
            {
                _dtSource = ds.Tables[0];
            }
        }
        #endregion

        #region Xu ly giao dien
        /// <summary>
        /// Dong form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlbClose_Click(object sender, RoutedEventArgs e)
        {
            CustomControl.CommonFunction.CloseUserControl(this);
        }

        /// <summary>
        /// Su kien xoa tren grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlbDeleteThongTin_Click(object sender, RoutedEventArgs e)
        {
            int rowCount = _dtSource.Rows.Count;
            for (int i = rowCount-1; i >= 0; i--)
            {
                if (Convert.ToBoolean(_dtSource.Rows[i]["CHON"]) == true)
                {
                    _dtSource.Rows.RemoveAt(i);
                    
                }
            }
            for (int i = 0; i < _dtSource.Rows.Count; i++)
            {
                _dtSource.Rows[i]["STT"] = i + 1;
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
        /// Load du lieu vao combobox
        /// </summary>
        private void LoadDuLieuCombobox()
        {
            AutoComboBox auto = new AutoComboBox();
            //Load combobox GioiTinh
            List<string> lstDK = new List<string>();
            lstDK.Add(DatabaseConstant.DanhMuc.GIOI_TINH.getValue());
            auto.GenAutoComboBox(ref lstSourceGioiTinh, ref cmbGioiTinh, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDK);

            //Load combobox MoiQuanHeKH
            lstDK.Clear();
            lstDK.Add(DatabaseConstant.DanhMuc.MOI_QUAN_HE.getValue());
            auto.GenAutoComboBox(ref lstSourceMoiQuanHe, ref cmbMoiQuanHe, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDK);

            //Load combobox TinhTrangSucKhoe
            lstDK.Clear();
            lstDK.Add(DatabaseConstant.DanhMuc.TINH_TRANG_SUC_KHOE.getValue());
            auto.GenAutoComboBox(ref lstSourceSucKhoe, ref cmbTTSucKhoe, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDK);

            //Load combobox LoaiGiayTo
            lstDK.Clear();
            lstDK.Add(DatabaseConstant.DanhMuc.LOAI_GIAY_TO.getValue());
            auto.GenAutoComboBox(ref lstSourceLoaiGiayTo, ref cmbLoaiGiayTo, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDK, null);

            //Load du lieu combobox NoiCap ~ TinhThanh
            auto.GenAutoComboBox(ref lstSourceNoiCap, ref cmbNoiCap, DatabaseConstant.DanhSachTruyVan.COMBOBOX_TINHTP.getValue(), null, null);
        }

        /// <summary>
        /// Check all grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkThongTinCoBanGDinh_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < _dtSource.Rows.Count; i++)
            {
                _dtSource.Rows[i]["CHON"] = chkThongTinCoBanGDinh.IsChecked;
            }
        }

        /// <summary>
        /// Thêm mới
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlbAddThongTin_Click(object sender, RoutedEventArgs e)
        {
            grbChiTiet.IsEnabled = true;
            isUpdate = false;
            ResetForm();
        }

        /// <summary>
        /// Đưa các control của form về trạng thái mặc định
        /// </summary>
        private void ResetForm()
        {
            isUpdate = false;
            txtTenKhachHang.Text = "";
            //raddtNgaySinh.Value = LDateTime.GetCurrentDate();
            cmbGioiTinh.SelectedIndex = 0;
            cmbMoiQuanHe.SelectedIndex = 0;
            cmbTTSucKhoe.SelectedIndex = 0;
            chkThongTinCoBanGDinh.IsChecked = false;
            chkNguoiThuaKe.IsChecked = false;
            chkNguoiDongTrachNhiem.IsChecked = false;
            txtSoGiayTo.Text = "";
            raddtNgaySinh.Value = null;
            raddtNgayCap.Value = null;
            cmbLoaiGiayTo.SelectedIndex = lstSourceLoaiGiayTo.IndexOf(lstSourceLoaiGiayTo.FirstOrDefault(f => f.KeywordStrings.First().Equals(BusinessConstant.LOAI_GIAY_TO.CHUNG_MINH_ND.layGiaTri())));
            cmbNoiCap.SelectedIndex = -1;
            txtTenKhachHang.Focus();

            txtMaThanhVienGD.Text = "";
            chkCoViecLam.IsChecked = false;
        }

        private void grThanhVienTrongGD_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            if (grThanhVienTrongGD.SelectedItem != null)
            {
                DataRowView dr = (DataRowView)grThanhVienTrongGD.SelectedItem;
                txtTenKhachHang.Text = dr["GD_HO_TEN"].ToString();
                raddtNgaySinh.Value = LDateTime.StringToDate(dr["GD_NGAY_SINH"].ToString(), "yyyyMMdd");
                cmbGioiTinh.SelectedIndex = lstSourceGioiTinh.IndexOf(lstSourceGioiTinh.FirstOrDefault(f => f.KeywordStrings.First().Equals(dr["GD_GIOI_TINH"].ToString())));
                cmbMoiQuanHe.SelectedIndex = lstSourceMoiQuanHe.IndexOf(lstSourceMoiQuanHe.FirstOrDefault(f => f.KeywordStrings.First().Equals(dr["GD_MOI_QUAN_HE"].ToString())));
                cmbTTSucKhoe.SelectedIndex = lstSourceSucKhoe.IndexOf(lstSourceSucKhoe.FirstOrDefault(f => f.KeywordStrings.First().Equals(dr["GD_TTRANG_SKHOE"].ToString())));
                chkThongTinCoBanGDinh.IsChecked = Convert.ToBoolean(dr["GD_NGHE_NGHIEP"]);
                chkNguoiThuaKe.IsChecked = Convert.ToBoolean(dr["GD_NGUOI_TKE"]);
                chkNguoiDongTrachNhiem.IsChecked = Convert.ToBoolean(dr["GD_NGUOI_DONG_TNHIEM"]);
                txtSoGiayTo.Text = dr["GD_GTLQ_SO"].ToString();
                if (!LString.IsNullOrEmptyOrSpace(dr["GD_GTLQ_NGAY_CAP"].ToString()))
                {
                    raddtNgayCap.Value = LDateTime.StringToDate(dr["GD_GTLQ_NGAY_CAP"].ToString(), "yyyyMMdd");
                }
                cmbLoaiGiayTo.SelectedIndex = lstSourceLoaiGiayTo.IndexOf(lstSourceLoaiGiayTo.FirstOrDefault(f => f.KeywordStrings.First().Equals(dr["GD_GTLQ_LOAI"].ToString())));
                cmbNoiCap.SelectedIndex = lstSourceNoiCap.IndexOf(lstSourceNoiCap.FirstOrDefault(f => f.KeywordStrings.First().Equals(dr["GD_GTLQ_NOI_CAP"].ToString())));
                isUpdate = true;

                txtMaThanhVienGD.Text = dr["GD_MA_DONG_TNHIEM"].ToString();
                chkCoViecLam.IsChecked = Convert.ToBoolean(dr["GD_NGHE_NGHIEP"]);
            }
        }

        private void chkNguoiThuaKe_Checked(object sender, RoutedEventArgs e)
        {
            //if (chkNguoiDongTrachNhiem.IsChecked == true)
            //{
            //    chkNguoiDongTrachNhiem.IsChecked = false;
            //}
            lblRedLoaiGiayTo.Visibility = Visibility.Visible;
            lblRedNgayCap.Visibility = Visibility.Visible;
            lblRedNoiCap.Visibility = Visibility.Visible;
            lblRedSoGiayTo.Visibility = Visibility.Visible;
            cmbLoaiGiayTo.IsEnabled = true;
            txtSoGiayTo.IsEnabled = true;
            raddtNgayCap.IsEnabled = true;
            dtpNgayCap.IsEnabled = true;
            cmbNoiCap.IsEnabled = true; 
        }

        private void chkNguoiThuaKe_Unchecked(object sender, RoutedEventArgs e)
        {
            if (chkNguoiDongTrachNhiem.IsChecked == false)
            {
                lblRedLoaiGiayTo.Visibility = Visibility.Hidden;
                lblRedNgayCap.Visibility = Visibility.Hidden;
                lblRedNoiCap.Visibility = Visibility.Hidden;
                lblRedSoGiayTo.Visibility = Visibility.Hidden;
                cmbLoaiGiayTo.IsEnabled = false;
                txtSoGiayTo.IsEnabled = false;
                raddtNgayCap.IsEnabled = false;
                dtpNgayCap.IsEnabled = false;
                cmbNoiCap.IsEnabled = false;
            }
        }

        private void chkNguoiDongTrachNhiem_Checked(object sender, RoutedEventArgs e)
        {
            //if (chkNguoiThuaKe.IsChecked == true)
            //{
            //    chkNguoiThuaKe.IsChecked = false;
            //}
            lblRedLoaiGiayTo.Visibility = Visibility.Visible;
            lblRedNgayCap.Visibility = Visibility.Visible;
            lblRedNoiCap.Visibility = Visibility.Visible;
            lblRedSoGiayTo.Visibility = Visibility.Visible;
            cmbLoaiGiayTo.IsEnabled = true;
            txtSoGiayTo.IsEnabled = true;
            raddtNgayCap.IsEnabled = true;
            dtpNgayCap.IsEnabled = true;
            cmbNoiCap.IsEnabled = true;

            //txtMaDongTrachNhiem.Visibility = Visibility.Visible;
            //btnMaDongTrachNhiem.Visibility = Visibility.Visible;
        }

        private void chkNguoiDongTrachNhiem_Unchecked(object sender, RoutedEventArgs e)
        {
            if (chkNguoiThuaKe.IsChecked == false)
            {
                lblRedLoaiGiayTo.Visibility = Visibility.Hidden;
                lblRedNgayCap.Visibility = Visibility.Hidden;
                lblRedNoiCap.Visibility = Visibility.Hidden;
                lblRedSoGiayTo.Visibility = Visibility.Hidden;
                cmbLoaiGiayTo.IsEnabled = false;
                txtSoGiayTo.IsEnabled = false;
                raddtNgayCap.IsEnabled = false;
                dtpNgayCap.IsEnabled = false;
                cmbNoiCap.IsEnabled = false;
            }
            //txtMaDongTrachNhiem.Visibility = Visibility.Hidden;
            //btnMaDongTrachNhiem.Visibility = Visibility.Hidden;
        }

        private void btnThanhVienGD_Click(object sender, RoutedEventArgs e)
        {
            HienPopUpThanhVienGD();
        }

        /// <summary>
        /// Hiện popup khách hàng chưa phải là thành viên
        /// </summary>
        private void HienPopUpThanhVienGD()
        {
            Window window = new Window();
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.RenderSize = new Size(1024, 768);
            ucPopupKhachHang uc = new ucPopupKhachHang(false);
            window.Title = "Danh sách khách hàng";
            window.Content = uc;
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.ShowDialog();
            if (!LString.IsNullOrEmptyOrSpace(uc.id))
            {
                int idKhachHang = Convert.ToInt32(uc.id);
                SetFormThanhVienGD(idKhachHang);
            }
            uc = null;
        }

        /// <summary>
        /// Đưa các control của form về trạng thái mặc định
        /// </summary>
        private void SetFormThanhVienGD(int idKhachHang)
        {
            Presentation.Process.KhachHangProcess process = new Presentation.Process.KhachHangProcess();
            try
            {
                DataSet dsSourceKHang = process.getThongTinKHTheoID(idKhachHang);
                if (dsSourceKHang != null && dsSourceKHang.Tables.Count > 0)
                {
                    DataRow dr = dsSourceKHang.Tables["KH_KHANG_HSO"].Rows[0];
                    isUpdate = false;
                    txtMaThanhVienGD.Text = dr["MA_KHANG"].ToString();
                    txtTenKhachHang.Text = dr["TEN_KHANG"].ToString();
                    raddtNgaySinh.Value = LDateTime.StringToDate(dr["DD_NGAY_SINH"].ToString(), "yyyyMMdd");
                    if (!LString.IsNullOrEmptyOrSpace(dr["DD_GIOI_TINH"].ToString()))
                    {
                        cmbGioiTinh.SelectedIndex = lstSourceGioiTinh.IndexOf(lstSourceGioiTinh.FirstOrDefault(f => f.KeywordStrings.First().Equals(dr["DD_GIOI_TINH"].ToString())));
                    }
                    cmbMoiQuanHe.SelectedIndex = 0;
                    cmbTTSucKhoe.SelectedIndex = 0;
                    chkThongTinCoBanGDinh.IsChecked = false;
                    chkNguoiThuaKe.IsChecked = false;
                    chkNguoiDongTrachNhiem.IsChecked = false;
                    txtSoGiayTo.Text = "";
                    raddtNgayCap.Value = null;
                    cmbLoaiGiayTo.SelectedIndex = lstSourceLoaiGiayTo.IndexOf(lstSourceLoaiGiayTo.FirstOrDefault(f => f.KeywordStrings.First().Equals(BusinessConstant.LOAI_GIAY_TO.CHUNG_MINH_ND.layGiaTri())));
                    cmbNoiCap.SelectedIndex = -1;
                    txtTenKhachHang.Focus();
                }
            }
            catch (System.Exception ex)
            {
                if (ex.GetType() == typeof(CustomException))
                {
                    new frmThongBaoLoi(ex.Message, ex.InnerException).ShowDialog();
                }
                else if (ex.InnerException.GetType() == typeof(CustomException))
                {
                    new frmThongBaoLoi(ex.InnerException.Message, ex.InnerException).ShowDialog();
                }
                else
                {
                    new frmThongBaoLoi("M.DungChung.LoiLoadDuLieu", ex).ShowDialog();
                }
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                process = null;
            }            
        }

        #endregion

        #region Xu ly nghiep vu
        /// <summary>
        /// Load du lieu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            txtTenKhachHang.Focus();
            grThanhVienTrongGD.ItemsSource = _dtSource.DefaultView;
            if (_dtSource != null)
            {
                grbChiTiet.IsEnabled = true;
            }
            else
            {
                grbChiTiet.IsEnabled = false;
            }
        }

        /// <summary>
        /// Su kien luu thong tin chi tiet xuong grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlbSaveThongTin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Validation())
                {
                    AutoComboBox au = new AutoComboBox();
                    string gioi_tinh = au.getEntryByDisplayName(lstSourceGioiTinh, ref cmbGioiTinh).KeywordStrings[0];
                    string suc_khoe = au.getEntryByDisplayName(lstSourceSucKhoe, ref cmbTTSucKhoe).KeywordStrings[0];
                    string moi_quan_he = au.getEntryByDisplayName(lstSourceMoiQuanHe, ref cmbMoiQuanHe).KeywordStrings[0];

                    DateTime ngay_sinh = Convert.ToDateTime(raddtNgaySinh.Value);
                    int tuoi = LDateTime.CountYearBetweenDates(LDateTime.GetCurrentDate(), Convert.ToDateTime(raddtNgaySinh.Value));
                    DataRowView dr = null;
                    if (!isUpdate)
                    {
                        DataRow drAdd = _dtSource.NewRow();
                        _dtSource.Rows.Add(drAdd);
                        dr = _dtSource.DefaultView[_dtSource.Rows.Count - 1];
                        dr["CHON"] = false;
                        dr["STT"] = grThanhVienTrongGD.Items.Count;
                        dr["ID_KHANG"] = -1;
                        idTuSinh = idTuSinh - 1;
                    }
                    else
                    {
                        dr = (DataRowView)grThanhVienTrongGD.SelectedItem;
                    }

                    if (dr == null)
                    {
                        DataRow drAdd = _dtSource.NewRow();
                        _dtSource.Rows.Add(drAdd);
                        dr = _dtSource.DefaultView[_dtSource.Rows.Count - 1];
                        dr["CHON"] = false;
                        dr["STT"] = grThanhVienTrongGD.Items.Count;
                        dr["ID_KHANG"] = -1;
                        idTuSinh = idTuSinh - 1;
                    }

                    dr["GD_HO_TEN"] = txtTenKhachHang.Text.Trim();
                    dr["GD_NGAY_SINH"] = ngay_sinh.ToString("yyyyMMdd");
                    dr["GD_NGAY_SINH_TEXT"] = ngay_sinh.ToString(@"dd/MM/yyyy");
                    dr["GD_TUOI"] = tuoi;
                    dr["GD_GIOI_TINH"] = gioi_tinh;
                    dr["GD_GIOI_TINH_TEXT"] = cmbGioiTinh.Text;
                    dr["GD_MOI_QUAN_HE"] = moi_quan_he;
                    dr["GD_MOI_QUAN_HE_TEXT"] = cmbMoiQuanHe.Text;
                    dr["GD_TTRANG_SKHOE"] = suc_khoe;
                    dr["GD_TTRANG_SKHOE_TEXT"] = cmbTTSucKhoe.Text;
                    dr["GD_NGHE_NGHIEP"] = chkCoViecLam.IsChecked;
                    dr["GD_NGUOI_TKE"] = chkNguoiThuaKe.IsChecked;
                    dr["GD_NGUOI_DONG_TNHIEM"] = chkNguoiDongTrachNhiem.IsChecked;
                    if (!LString.IsNullOrEmptyOrSpace(txtMaThanhVienGD.Text))
                    {
                        dr["GD_MA_DONG_TNHIEM"] = txtMaThanhVienGD.Text;
                    }
                    else
                    {
                        dr["GD_MA_DONG_TNHIEM"] = "DTN" + DateTime.Now.ToString("yyyyMMddHHmmss");
                    }

                    if (chkNguoiThuaKe.IsChecked == true || chkNguoiDongTrachNhiem.IsChecked == true)
                    {
                        AutoCompleteEntry auLoaiGiayTo = au.getEntryByDisplayName(lstSourceLoaiGiayTo, ref cmbLoaiGiayTo);
                        AutoCompleteEntry auNoiCap = au.getEntryByDisplayName(lstSourceNoiCap, ref cmbNoiCap);

                        dr["GD_GTLQ_SO"] = txtSoGiayTo.Text;
                        if (!LObject.IsNullOrEmpty(auNoiCap))
                            dr["GD_GTLQ_NOI_CAP"] = auNoiCap.KeywordStrings[0];
                        dr["GD_GTLQ_NOI_CAP_TEXT"] = cmbNoiCap.Text;
                        if (!LObject.IsNullOrEmpty(auLoaiGiayTo))
                            dr["GD_GTLQ_LOAI"] = auLoaiGiayTo.KeywordStrings[0];
                        dr["GD_GTLQ_LOAI_TEXT"] = cmbLoaiGiayTo.Text;
                        if (!LObject.IsNullOrEmpty(raddtNgayCap.Value))
                        {
                            DateTime ngay_cap = Convert.ToDateTime(raddtNgayCap.Value);
                            dr["GD_GTLQ_NGAY_CAP"] = ngay_cap.ToString("yyyyMMdd");
                            dr["GD_GTLQ_NGAY_CAP_TEXT"] = ngay_cap.ToString("dd/MM/yyyy");
                        }
                    }

                    ResetForm();
                }
            }
            catch (Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        /// <summary>
        /// Kiem tra du lieu truoc khi luu
        /// </summary>
        /// <returns></returns>
        private bool Validation()
        {
            AutoComboBox au = new AutoComboBox();
            if (LString.IsNullOrEmptyOrSpace(txtTenKhachHang.Text))
            {
                CommonFunction.ThongBaoTrong(lblHoTen.Content.ToString());
                txtTenKhachHang.Focus();
                return false;
            }
            else if (raddtNgaySinh.Value == null)
            {
                CommonFunction.ThongBaoTrong(lblNgayThangNamSinh.Content.ToString());
                raddtNgaySinh.Focus();
                return false;
            }
            else if (au.getEntryByDisplayName(lstSourceGioiTinh, ref cmbGioiTinh) == null)
            {
                CommonFunction.ThongBaoTrong(lblGioiTinh.Content.ToString());
                cmbGioiTinh.Focus();
                return false;
            }
            else if (au.getEntryByDisplayName(lstSourceMoiQuanHe, ref cmbMoiQuanHe) == null)
            {
                CommonFunction.ThongBaoTrong(lblMoiQuanHe.Content.ToString());
                cmbMoiQuanHe.Focus();
                return false;
            }
            else if (au.getEntryByDisplayName(lstSourceSucKhoe, ref cmbTTSucKhoe) == null)
            {
                CommonFunction.ThongBaoTrong(lblTTSucKhoe.Content.ToString());
                cmbTTSucKhoe.Focus();
                return false;
            }
            else if (Convert.ToDateTime(raddtNgaySinh.Value) > LDateTime.GetCurrentDate())
            {
                LMessage.ShowMessage("M.KhachHang.Popup.ucThongTinCoBanHoGD.LoiNgaySinh", LMessage.MessageBoxType.Warning);
                raddtNgaySinh.Focus();
                return false;
            }

            if (chkNguoiThuaKe.IsChecked == true || chkNguoiDongTrachNhiem.IsChecked == true)
            {
                AutoCompleteEntry loai_giay_to = au.getEntryByDisplayName(lstSourceLoaiGiayTo, ref cmbLoaiGiayTo);
                AutoCompleteEntry noi_cap = au.getEntryByDisplayName(lstSourceNoiCap, ref cmbNoiCap);
                //if (loai_giay_to == null)
                //{
                //    CommonFunction.ThongBaoTrong(lblLoaiGiayTo.Content.ToString());
                //    cmbLoaiGiayTo.Focus();
                //    return false;
                //}
                //else if (LString.IsNullOrEmptyOrSpace(txtSoGiayTo.Text))
                //{
                //    CommonFunction.ThongBaoTrong(lblSoGiayTo.Content.ToString());
                //    txtSoGiayTo.Focus();
                //    return false;
                //}
                //else if (raddtNgayCap.Value == null)
                //{
                //    CommonFunction.ThongBaoTrong(lblNgayCap.Content.ToString());
                //    raddtNgayCap.Focus();
                //    return false;
                //}
                //else if (noi_cap == null)
                //{
                //    CommonFunction.ThongBaoTrong(lblNoiCap.Content.ToString());
                //    cmbNoiCap.Focus();
                //    return false;
                //}
            }
            return true;
        }

        /// <summary>
        /// Luu du lieu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlbSave_Click(object sender, RoutedEventArgs e)
        {
            CustomControl.CommonFunction.CloseUserControl(this);
        }
        #endregion
    }
}