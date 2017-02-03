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
    public partial class ucTaiKhoanTCTDKhac : UserControl
    {
        #region Khai bao
        List<AutoCompleteEntry> lstSourceTCTD = new List<AutoCompleteEntry>();

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
        public ucTaiKhoanTCTDKhac()
        {
            InitializeComponent();
            KhoiTaoDataSource();
        }

        /// <summary>
        /// Khởi tạo cấu trúc datasource
        /// </summary>
        private void KhoiTaoDataSource()
        {
            Presentation.Process.KhachHangProcess process = new Presentation.Process.KhachHangProcess();
            DataSet ds = process.getThongTinKhac("VKH_TKHOAN_NHANG");
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
            txtSoTaiKhoan.Text = "";
            txtTCTD.Text = "";
            chkAll.IsChecked = false;
            txtSoTaiKhoan.Focus();
        }

        /// <summary>
        /// Click chọn xem thông tin trên grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grTaiKhoan_CurrentCellChanged(object sender, Telerik.Windows.Controls.GridViewCurrentCellChangedEventArgs e)
        {
            if (e.OldCell != null && e.NewCell != null)
            {
                if (!e.NewCell.Equals(e.OldCell))
                {
                    DataRowView dr = (DataRowView)grTaiKhoan.SelectedItem;
                    txtSoTaiKhoan.Text = dr["TK_SO"].ToString();
                    txtSoTaiKhoan.Text = dr["TK_TEN_TCTD"].ToString();
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
            txtSoTaiKhoan.Focus();
            grTaiKhoan.ItemsSource = _dtSource.DefaultView;
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

                if (!isUpdate)
                {
                    _dtSource.Rows.Add(false, grTaiKhoan.Items.Count + 1, idTuSinh - 1, -1, txtSoTaiKhoan.Text.Trim(), txtTCTD.Text.Trim());
                    idTuSinh = idTuSinh - 1;
                }
                else
                {
                    DataRowView dr = (DataRowView)grTaiKhoan.SelectedItem;
                    dr["TK_SO"] = txtSoTaiKhoan.Text.Trim();
                    dr["TK_TEN_TCTD"] = txtTCTD.Text.Trim();
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
            if (LString.IsNullOrEmptyOrSpace(txtSoTaiKhoan.Text))
            {
                CommonFunction.ThongBaoTrong(lblSoTaiKhoan.Content.ToString());
                txtSoTaiKhoan.Focus();
                return false;
            }

            else if (LString.IsNullOrEmptyOrSpace(txtTCTD.Text))
            {
                CommonFunction.ThongBaoTrong(lblTenTCTD.Content.ToString());
                txtTCTD.Focus();
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

        private void grTaiKhoan_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            if (grTaiKhoan.SelectedItem != null)
            {
                DataRowView dr = (DataRowView)grTaiKhoan.SelectedItem;
                txtSoTaiKhoan.Text = dr["TK_SO"].ToString();
                txtTCTD.Text = dr["TK_TEN_TCTD"].ToString();
                isUpdate = true;
            }
        }
    }
}