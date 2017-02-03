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
using Presentation.Process;
using Presentation.Process.PopupServiceRef;
using System.ComponentModel;
using Telerik.Windows.Data;

namespace PresentationWPF.KeToan.ButToan
{
    public partial class ucNhomDinhKhoan : UserControl
    {
        #region Khai bao
        List<AutoCompleteEntry> lstSourceDinhKhoan = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceLoaiChungTu = new List<AutoCompleteEntry>();

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

        List<DataRow> lstPopup = new List<DataRow>();
        public void LayDuLieuTuPopup(List<DataRow> lst)
        {
            lstPopup = lst;
        }

        private bool _insertValue = false;
        public bool insertValue
        {
            get { return _insertValue; }
        }
        #endregion

        #region Khoi tao
        public ucNhomDinhKhoan()
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
            _dtSource = new DataTable();
            _dtSource.Columns.Add("CHON", typeof(bool));
            _dtSource.Columns.Add("STT", typeof(int));
            _dtSource.Columns.Add("ID", typeof(int));
            _dtSource.Columns.Add("DINHKHOAN_MA", typeof(string));
            _dtSource.Columns.Add("DINHKHOAN_TEN", typeof(string));
            _dtSource.Columns.Add("MA_PHAN_LOAI", typeof(string));
            _dtSource.Columns.Add("TEN_PHAN_LOAI", typeof(string));
            _dtSource.Columns.Add("NHOM_DK", typeof(string));
            _dtSource.Columns.Add("CHUNGTU_MA", typeof(string));
            _dtSource.Columns.Add("CHUNGTU_TEN", typeof(string));
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
            if (ValidationGrid())
            {
                _insertValue = true;
                CustomControl.CommonFunction.CloseUserControl(this);
            }
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
            if (e.Key == Key.Escape)
            {
                if (ValidationGrid())
                {
                    _insertValue = true;
                    CustomControl.CommonFunction.CloseUserControl(e, this);
                }
            }
            CustomControl.CommonFunction.SelectNextControl(e);
        }

        /// <summary>
        /// Load du lieu vao combobox
        /// </summary>
        private void LoadDuLieuCombobox()
        {
            AutoComboBox auto = new AutoComboBox();
            List<string> lstDK = new List<string>();
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                // Dinh khoan
                lstDK.Add(DatabaseConstant.getValue(DatabaseConstant.DanhMuc.DINH_KHOAN));
                auto.GenAutoComboBox(ref lstSourceDinhKhoan, ref cmbDinhKhoan, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDK);

                // Loai chung tu
                lstDK.Clear();
                lstDK.Add(DatabaseConstant.getValue(DatabaseConstant.DanhMuc.LOAI_CHUNG_TU));
                auto.GenAutoComboBox(ref lstSourceLoaiChungTu, ref cmbLoaiChungTu, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDK);
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiLoadDuLieu", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
                auto = null;
            }
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
            cmbDinhKhoan.SelectedIndex = 0;
            cmbLoaiChungTu.SelectedIndex = 0;
            txtMaPhanLoai.Text = "";
            txtMaPhanLoai.Tag = "";
            lblTenPhanLoai.Content = "";
            txtNhomDinhKhoan.Text = "";
            cmbLoaiChungTu.IsEnabled = true;
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
            txtNhomDinhKhoan.Focus();
            grDinhKhoan.ItemsSource = _dtSource.DefaultView;
            if (_dtSource != null)
            {
                this.grDinhKhoan.GroupDescriptors.Add(new ColumnGroupDescriptor()
                {
                    Column = this.grDinhKhoan.Columns["NHOM_DK"],
                    SortDirection = ListSortDirection.Ascending
                });
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
                if (this.grDinhKhoan.GroupDescriptors.Count > 0)
                {
                    this.grDinhKhoan.GroupDescriptors.RemoveAt(0);
                }

                AutoComboBox au = new AutoComboBox();
                AutoCompleteEntry auDinhKhoan = au.getEntryByDisplayName(lstSourceDinhKhoan, ref cmbDinhKhoan);
                AutoCompleteEntry auLoaiChungTu = au.getEntryByDisplayName(lstSourceLoaiChungTu, ref cmbLoaiChungTu);

                DataRowView dr = null;
                if (!isUpdate)
                {
                    DataRow drAdd = _dtSource.NewRow();
                    _dtSource.Rows.Add(drAdd);
                    dr = _dtSource.DefaultView[_dtSource.Rows.Count - 1];
                    dr["CHON"] = false;
                    dr["STT"] = grDinhKhoan.Items.Count;
                    dr["ID"] = -1;
                    idTuSinh = idTuSinh - 1;
                }
                else
                {
                    dr = (DataRowView)grDinhKhoan.SelectedItem;
                }

                if (dr == null)
                {
                    DataRow drAdd = _dtSource.NewRow();
                    _dtSource.Rows.Add(drAdd);
                    dr = _dtSource.DefaultView[_dtSource.Rows.Count - 1];
                    dr["CHON"] = false;
                    dr["STT"] = grDinhKhoan.Items.Count;
                    dr["ID"] = -1;
                    idTuSinh = idTuSinh - 1;
                }

                dr["DINHKHOAN_MA"] = auDinhKhoan.KeywordStrings[0];
                dr["DINHKHOAN_TEN"] = auDinhKhoan.DisplayName;
                dr["MA_PHAN_LOAI"] = txtMaPhanLoai.Text;
                dr["TEN_PHAN_LOAI"] = lblTenPhanLoai.Content.ToString();
                dr["NHOM_DK"] = txtNhomDinhKhoan.Text;
                dr["CHUNGTU_MA"] = auLoaiChungTu.KeywordStrings[0];
                dr["CHUNGTU_TEN"] = auLoaiChungTu.DisplayName;

                this.grDinhKhoan.GroupDescriptors.Add(new ColumnGroupDescriptor()
                        {
                            Column = this.grDinhKhoan.Columns["NHOM_DK"],
                            SortDirection = ListSortDirection.Ascending
                        });
                ResetForm();
            }
        }

        /// <summary>
        /// Kiem tra du lieu truoc khi luu xuong grid
        /// </summary>
        /// <returns></returns>
        private bool Validation()
        {
            AutoComboBox au = new AutoComboBox();
            AutoCompleteEntry auDinhKhoan = au.getEntryByDisplayName(lstSourceDinhKhoan, ref cmbDinhKhoan);
            AutoCompleteEntry auLoaiChungTu = au.getEntryByDisplayName(lstSourceLoaiChungTu, ref cmbLoaiChungTu);

            if (LString.IsNullOrEmptyOrSpace(txtNhomDinhKhoan.Text))
            {
                LMessage.ShowMessage("Nhóm định khoản không được để trống", LMessage.MessageBoxType.Warning);
                txtNhomDinhKhoan.Focus();
                return false;
            }
            else if (auDinhKhoan == null)
            {
                LMessage.ShowMessage("Định khoản không được để trống", LMessage.MessageBoxType.Warning);
                cmbDinhKhoan.Focus();
                return false;
            }
            else if (LString.IsNullOrEmptyOrSpace(txtMaPhanLoai.Text))
            {
                LMessage.ShowMessage("Mã phân loại không được để trống", LMessage.MessageBoxType.Warning);
                cmbDinhKhoan.Focus();
                return false;
            }
            else if (auLoaiChungTu == null)
            {
                LMessage.ShowMessage("Loại chứng từ không được để trống", LMessage.MessageBoxType.Warning);
                cmbLoaiChungTu.Focus();
                return false;
            }
            return true;
        }

        /// <summary>
        /// Kiểm tra dữ liệu trên grid trước khi đóng form
        /// </summary>
        /// <returns></returns>
        private bool ValidationGrid()
        {
            bool kq = true;
            if (this.grDinhKhoan.Items.Groups != null)
            {
                foreach (IGroup group in this.grDinhKhoan.Items.Groups)
                {
                    List<DataRowView> items = group.Items.Cast<DataRowView>().ToList();
                    int ghiNo = 0;
                    int ghiCo = 0;
                    int nhap = 0;
                    int xuat = 0;
                    foreach (DataRowView dr in items)
                    {
                        switch (dr["DINHKHOAN_MA"].ToString())
                        {
                            case "GN":
                                ghiNo++;
                                break;
                            case "GC":
                                ghiCo++;
                                break;
                            case "NH":
                                nhap++;
                                break;
                            case "XT":
                                xuat++;
                                break;
                        }
                    }

                    if (ghiNo >= 2 && ghiCo >= 2)
                    {
                        LMessage.ShowMessage("Nhóm định khoản " + group.Key + " không được ghi nhiều nợ nhiều có", LMessage.MessageBoxType.Warning);
                        return false;
                    }
                    else if (ghiNo > 0 && ghiCo == 0)
                    {
                        LMessage.ShowMessage("Nhóm định khoản " + group.Key + " chưa có vế ghi có", LMessage.MessageBoxType.Warning);
                        return false;
                    }
                    else if (ghiNo == 0 && ghiCo > 0)
                    {
                        LMessage.ShowMessage("Nhóm định khoản " + group.Key + " chưa có vế ghi nợ", LMessage.MessageBoxType.Warning);
                        return false;
                    }
                }
            }
            return kq;
        }

        /// <summary>
        /// Luu du lieu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlbSave_Click(object sender, RoutedEventArgs e)
        {
            if (ValidationGrid())
            {
                _insertValue = true;
                CustomControl.CommonFunction.CloseUserControl(this);
            }
        }
        #endregion

        private void grDinhKhoan_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            if (grDinhKhoan.SelectedItem != null)
            {
                DataRowView dr = (DataRowView)grDinhKhoan.SelectedItem;
                txtNhomDinhKhoan.Text = dr["NHOM_DK"].ToString();
                cmbDinhKhoan.SelectedIndex = lstSourceDinhKhoan.IndexOf(lstSourceDinhKhoan.FirstOrDefault(f => f.KeywordStrings.First().Equals(dr["DINHKHOAN_MA"].ToString())));
                txtMaPhanLoai.Text = dr["MA_PHAN_LOAI"].ToString();
                lblTenPhanLoai.Content = dr["TEN_PHAN_LOAI"].ToString();
                cmbLoaiChungTu.SelectedIndex = lstSourceLoaiChungTu.IndexOf(lstSourceLoaiChungTu.FirstOrDefault(f => f.KeywordStrings.First().Equals(dr["CHUNGTU_MA"].ToString())));

                isUpdate = true;
            }
        }

        private void btnMaPhanLoai_Click(object sender, RoutedEventArgs e)
        {
            Presentation.Process.KeToanProcess ketoanProcess = new Presentation.Process.KeToanProcess();
            try
            {
                //Bat popup
                var process = new PopupProcess();
                process.getPopupInformation("POPUP_KT_PLOAI");

                SimplePopupResponse simplePopupResponse = Presentation.Process.Common.ClientInformation.SimplePopup;

                ucPopup popup = new ucPopup(false, simplePopupResponse, false);
                popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
                Window win = new Window();
                win.Title = "Danh sách mã phân loại tài khoản";
                win.Content = popup;
                win.ShowDialog();
                if (lstPopup != null && lstPopup.Count > 0)
                {
                    DataRow dr = lstPopup[0];
                    txtMaPhanLoai.Tag = dr[0].ToString();
                    txtMaPhanLoai.Text = dr[2].ToString();
                    lblTenPhanLoai.Content = dr[3].ToString();
                }
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiLoadDuLieu", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                ketoanProcess = null;
            }
        }

        private void txtMaPhanLoai_LostFocus(object sender, RoutedEventArgs e)
        {
            Presentation.Process.KeToanProcess ketoanProcess = new Presentation.Process.KeToanProcess();
            try
            {
                if (!LString.IsNullOrEmptyOrSpace(txtMaPhanLoai.Text))
                {
                    DataTable dt = ketoanProcess.getThongTinMaPhanLoaiTheoMa(txtMaPhanLoai.Text);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        txtMaPhanLoai.Tag = dt.Rows[0]["ID"].ToString();
                        txtMaPhanLoai.Text = dt.Rows[0]["MA_PLOAI"].ToString();
                        lblTenPhanLoai.Content = dt.Rows[0]["TEN_PLOAI"].ToString();
                    }
                    else
                    {
                        LMessage.ShowMessage("Không tồn tại mã phân loại tài khoản này", LMessage.MessageBoxType.Warning);
                        txtMaPhanLoai.Tag = "";
                        txtMaPhanLoai.Text = "";
                        lblTenPhanLoai.Content = "";
                    }
                }
                else
                {
                    txtMaPhanLoai.Tag = "";
                    txtMaPhanLoai.Text = "";
                    lblTenPhanLoai.Content = "";
                }
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiLoadDuLieu", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                ketoanProcess = null;
            }
        }

        private void txtMaPhanLoai_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F3)
            {
                Presentation.Process.KeToanProcess ketoanProcess = new Presentation.Process.KeToanProcess();
                try
                {
                    //Bat popup
                    var process = new PopupProcess();
                    process.getPopupInformation("POPUP_KT_PLOAI");

                    SimplePopupResponse simplePopupResponse = Presentation.Process.Common.ClientInformation.SimplePopup;

                    ucPopup popup = new ucPopup(false, simplePopupResponse, false);
                    popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
                    Window win = new Window();
                    win.Title = "Danh sách mã phân loại tài khoản";
                    win.Content = popup;
                    win.ShowDialog();
                    if (lstPopup != null && lstPopup.Count > 0)
                    {
                        DataRow dr = lstPopup[0];
                        txtMaPhanLoai.Tag = dr[0].ToString();
                        txtMaPhanLoai.Text = dr[2].ToString();
                        lblTenPhanLoai.Content = dr[3].ToString();
                    }
                }
                catch (System.Exception ex)
                {
                    LMessage.ShowMessage("M.DungChung.LoiLoadDuLieu", LMessage.MessageBoxType.Error);
                    LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                }
                finally
                {
                    ketoanProcess = null;
                }
            }
        }

        private void txtNhomDinhKhoan_LostFocus(object sender, RoutedEventArgs e)
        {
            if (this.grDinhKhoan.Items.Groups != null)
            {
                foreach (IGroup group in this.grDinhKhoan.Items.Groups)
                {
                    if (group.Key.ToString().Equals(txtNhomDinhKhoan.Text))
                    {
                        List<DataRowView> items = group.Items.Cast<DataRowView>().ToList();
                        if (items.Count > 0)
                        {
                            DataRowView dr = items[0];
                            cmbLoaiChungTu.SelectedIndex = lstSourceLoaiChungTu.IndexOf(lstSourceLoaiChungTu.FirstOrDefault(f => f.KeywordStrings.First().Equals(dr["CHUNGTU_MA"].ToString())));
                            cmbLoaiChungTu.IsEnabled = false;
                        }
                    }
                }
            }
        }
    }
}