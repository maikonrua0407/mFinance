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
    public partial class ucGiayToLienQuan : UserControl
    {
        #region Khai bao
        List<AutoCompleteEntry> lstSourceLoaiGiayTo = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceNoiCap = new List<AutoCompleteEntry>();

        private int idNhomDinhKhoan = -1;
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
        public ucGiayToLienQuan()
        {
            InitializeComponent();
            raddtNgayCap.Value = null;
            raddtNgayHetHan.Value = null;
            LoadDuLieuCombobox();
            KhoiTaoDataSource();
        }

        /// <summary>
        /// Khởi tạo cấu trúc datasource
        /// </summary>
        private void KhoiTaoDataSource()
        {
            Presentation.Process.KhachHangProcess process = new Presentation.Process.KhachHangProcess();
            DataSet ds = process.getThongTinKhac("VKH_GTO_LQUAN");
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
            for (int i = rowCount - 1; i >= 0; i--)
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
            //Load combobox LoaiGiayTo
            List<string> lstDK = new List<string>();
            lstDK.Add(DatabaseConstant.DanhMuc.LOAI_GIAY_TO.getValue());
            auto.GenAutoComboBox(ref lstSourceLoaiGiayTo, ref cmbLoaiGiayTo, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDK);

            //Load du lieu combobox NoiCap ~ TinhThanh
            auto.GenAutoComboBox(ref lstSourceNoiCap, ref cmbNoiCap, DatabaseConstant.DanhSachTruyVan.COMBOBOX_TINHTP.getValue(), null);
        }

        /// <summary>
        /// Check all grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkAll_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < _dtSource.Rows.Count; i++)
            {
                _dtSource.Rows[i]["CHON"] = chkAll.IsChecked;
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
            txtSoGiayTo.Text = "";
            raddtNgayCap.Value = LDateTime.GetCurrentDate();
            raddtNgayHetHan.Value = LDateTime.GetCurrentDate();
            cmbLoaiGiayTo.SelectedIndex = 0;
            cmbNoiCap.SelectedIndex = 0;
            cmbLoaiGiayTo.Focus();
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
            cmbLoaiGiayTo.Focus();
            grGiayToLienQuanDS.ItemsSource = _dtSource.DefaultView;
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
            if (Validation())
            {
                AutoComboBox au = new AutoComboBox();
                string loai_giay_to = au.getEntryByDisplayName(lstSourceLoaiGiayTo, ref cmbLoaiGiayTo).KeywordStrings[0];
                string noi_cap = au.getEntryByDisplayName(lstSourceNoiCap, ref cmbNoiCap).KeywordStrings[0];
                DateTime ngay_cap = Convert.ToDateTime(raddtNgayCap.Value);
                DateTime ngay_het_han = Convert.ToDateTime(raddtNgayHetHan.Value);

                if (!isUpdate)
                {
                    _dtSource.Rows.Add(false, grGiayToLienQuanDS.Items.Count + 1, -1, -1, loai_giay_to, txtSoGiayTo.Text.Trim(), ngay_cap.ToString("yyyyMMdd"),
                                        ngay_het_han.ToString("yyyyMMdd"), noi_cap, cmbLoaiGiayTo.Text, ngay_cap.ToString("dd/MM/yyyy"), ngay_het_han.ToString("dd/MM/yyyy"),
                                        cmbNoiCap.Text);
                    idTuSinh = idTuSinh - 1;
                }
                else
                {
                    DataRowView dr = (DataRowView)grGiayToLienQuanDS.SelectedItem;
                    dr["GTLQ_LOAI"] = loai_giay_to;
                    dr["GTLQ_SO"] = txtSoGiayTo.Text.Trim();
                    dr["GTLQ_NGAY_CAP"] = ngay_cap.ToString("yyyyMMdd");
                    dr["GTLQ_NGAY_HHAN"] = ngay_het_han.ToString("yyyyMMdd");
                    dr["GTLQ_NOI_CAP"] = noi_cap;
                    dr["GTLQ_LOAI_TEXT"] = cmbLoaiGiayTo.Text;
                    dr["GTLQ_NGAY_CAP_TEXT"] = ngay_cap.ToString("dd/MM/yyyy");
                    dr["GTLQ_NGAY_HHAN_TEXT"] = ngay_het_han.ToString("dd/MM/yyyy");
                    dr["GTLQ_NOI_CAP_TEXT"] = cmbNoiCap.Text;
                }
                ResetForm();
            }
        }

        /// <summary>
        /// Kiem tra du lieu truoc khi luu
        /// </summary>
        /// <returns></returns>
        private bool Validation()
        {
            AutoComboBox au = new AutoComboBox();
            AutoCompleteEntry loai_giay_to = au.getEntryByDisplayName(lstSourceLoaiGiayTo, ref cmbLoaiGiayTo);
            AutoCompleteEntry noi_cap = au.getEntryByDisplayName(lstSourceNoiCap, ref cmbNoiCap);
            if (loai_giay_to == null)
            {
                CommonFunction.ThongBaoTrong(lblLoaiGiayTo.Content.ToString());
                cmbLoaiGiayTo.Focus();
                return false;
            }
            else if (LString.IsNullOrEmptyOrSpace(txtSoGiayTo.Text))
            {
                CommonFunction.ThongBaoTrong(lblSoGiayTo.Content.ToString());
                txtSoGiayTo.Focus();
                return false;
            }
            else if (raddtNgayCap.Value == null)
            {
                CommonFunction.ThongBaoTrong(lblNgayCap.Content.ToString());
                raddtNgayCap.Focus();
                return false;
            }
            else if (noi_cap == null)
            {
                CommonFunction.ThongBaoTrong(lblNoiCap.Content.ToString());
                cmbNoiCap.Focus();
                return false;
            }
            else if (raddtNgayHetHan.Value == null)
            {
                CommonFunction.ThongBaoTrong(lblNgayHetHan.Content.ToString());
                raddtNgayHetHan.Focus();
                return false;
            }
            else if (Convert.ToDateTime(raddtNgayHetHan.Value) < Convert.ToDateTime(raddtNgayCap.Value))
            {
                LMessage.ShowMessage("M.KhachHang.Popup.ucThongTinCoBanHoGD.LoiNgayHetHan", LMessage.MessageBoxType.Warning);
                raddtNgayHetHan.Focus();
                return false;
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

        private void grGiayToLienQuanDS_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            if (grGiayToLienQuanDS.SelectedItem != null)
            {
                DataRowView dr = (DataRowView)grGiayToLienQuanDS.SelectedItem;
                txtSoGiayTo.Text = dr["GTLQ_SO"].ToString();
                raddtNgayHetHan.Value = LDateTime.StringToDate(dr["GTLQ_NGAY_HHAN"].ToString(), "yyyyMMdd");
                raddtNgayCap.Value = LDateTime.StringToDate(dr["GTLQ_NGAY_CAP"].ToString(), "yyyyMMdd");
                cmbLoaiGiayTo.SelectedIndex = lstSourceLoaiGiayTo.IndexOf(lstSourceLoaiGiayTo.FirstOrDefault(f => f.KeywordStrings.First().Equals(dr["GTLQ_LOAI"].ToString())));
                cmbNoiCap.SelectedIndex = lstSourceNoiCap.IndexOf(lstSourceNoiCap.FirstOrDefault(f => f.KeywordStrings.First().Equals(dr["GTLQ_NOI_CAP"].ToString())));
                isUpdate = true;
            }
        }
    }
}