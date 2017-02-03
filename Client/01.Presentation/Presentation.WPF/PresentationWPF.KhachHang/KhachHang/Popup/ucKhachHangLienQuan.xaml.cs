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
    public partial class ucKhachHangLienQuan : UserControl
    {
        #region Khai bao
        List<AutoCompleteEntry> lstSourceMoiQuanHe = new List<AutoCompleteEntry>();

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
        public ucKhachHangLienQuan()
        {
            InitializeComponent();
            lblTenKhachHang.Content = "";
            LoadDuLieuCombobox();
            KhoiTaoDataSource();
        }

        /// <summary>
        /// Khởi tạo cấu trúc datasource
        /// </summary>
        private void KhoiTaoDataSource()
        {
            Presentation.Process.KhachHangProcess process = new Presentation.Process.KhachHangProcess();
            DataSet ds = process.getThongTinKhac("VKH_KHANG_LQUAN");
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
            //Load combobox MoiQuanHeKH
            List<string> lstDK = new List<string>();
            lstDK.Add(DatabaseConstant.DanhMuc.MOI_QUAN_HE.getValue());
            auto.GenAutoComboBox(ref lstSourceMoiQuanHe, ref cmbMoiQuanHe, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDK);
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
            txtMaKhachHang.Text = "";
            lblTenKhachHang.Content = "";
            txtMaKhachHang.Tag = "";
            cmbMoiQuanHe.SelectedIndex = 0;
            chkAll.IsChecked = false;
            txtMaKhachHang.Focus();
        }

        /// <summary>
        /// Click chọn xem thông tin trên grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grKhachHang_CurrentCellChanged(object sender, Telerik.Windows.Controls.GridViewCurrentCellChangedEventArgs e)
        {
            if (e.OldCell != null && e.NewCell != null)
            {
                if (!e.NewCell.Equals(e.OldCell))
                {
                    DataRowView dr = (DataRowView)grKhachHang.SelectedItem;
                    txtMaKhachHang.Text = dr[""].ToString();
                    cmbMoiQuanHe.SelectedIndex = lstSourceMoiQuanHe.IndexOf(lstSourceMoiQuanHe.FirstOrDefault(f => f.KeywordStrings.First().Equals(dr["KHLQ_MOI_QUAN_HE"].ToString())));
                }
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
            txtMaKhachHang.Focus();
            grKhachHang.ItemsSource = _dtSource.DefaultView;
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
                string moi_quan_he = au.getEntryByDisplayName(lstSourceMoiQuanHe, ref cmbMoiQuanHe).KeywordStrings[0];

                if (!isUpdate)
                {
                    _dtSource.Rows.Add(false, grKhachHang.Items.Count + 1, idTuSinh - 1, -1, txtMaKhachHang.Text.Trim(),moi_quan_he,lblTenKhachHang.Content,
                                        cmbMoiQuanHe.Text);
                    idTuSinh = idTuSinh - 1;
                }
                else
                {
                    DataRowView dr = (DataRowView)grKhachHang.SelectedItem;
                    dr["KHLQ_MA"] = txtMaKhachHang.Text.Trim();
                    dr["KHLQ_TEN"] = lblTenKhachHang.Content;
                    dr["KHLQ_MOI_QUAN_HE"] = moi_quan_he;
                    dr["KHLQ_MOI_QUAN_HE_TEXT"] = cmbMoiQuanHe.Text;
                    
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
            if (LString.IsNullOrEmptyOrSpace(txtMaKhachHang.Text))
            {
                CommonFunction.ThongBaoTrong(lblMaKhachHang.Content.ToString());
                txtMaKhachHang.Focus();
                return false;
            }
            
            else if (au.getEntryByDisplayName(lstSourceMoiQuanHe, ref cmbMoiQuanHe) == null)
            {
                CommonFunction.ThongBaoTrong(lblMoiQuanHe.Content.ToString());
                cmbMoiQuanHe.Focus();
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

        private void grKhachHang_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            DataRowView dr = (DataRowView)grKhachHang.SelectedItem;
            txtMaKhachHang.Text = dr["KHLQ_MA"].ToString();
            lblTenKhachHang.Content = dr["KHLQ_TEN"].ToString();
            cmbMoiQuanHe.SelectedIndex = lstSourceMoiQuanHe.IndexOf(lstSourceMoiQuanHe.FirstOrDefault(f => f.KeywordStrings.First().Equals(dr["KHLQ_MOI_QUAN_HE"].ToString())));
            isUpdate = true;
        }

        private void btnKhachHang_Click(object sender, RoutedEventArgs e)
        {
            Window window = new Window();
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.RenderSize = new Size(1024, 768);
            ucPopupKhachHang uc = new ucPopupKhachHang();
            window.Content = uc;
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.ShowDialog();
            if (!LString.IsNullOrEmptyOrSpace(uc.id))
            {
                txtMaKhachHang.Text = uc.ma;
                lblTenKhachHang.Content = uc.ten;
            }
            uc = null;
        }

        private void txtMaKhachHang_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F3)
            {
                btnKhachHang_Click(null, null);
            }
            else if (e.Key == Key.Escape)
            {
                CustomControl.CommonFunction.CloseUserControl(this);
            }
            else
            {
                e.Handled = true;
            }
        }
    }
}