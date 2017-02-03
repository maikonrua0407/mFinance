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
using Microsoft.Windows.Controls.Ribbon;
using Utilities.Common;
using Presentation.Process;
using Presentation.Process.Common;
using Presentation.Process.PopupServiceRef;
using PresentationWPF.CustomControl;
using System.Data;
using Telerik.Windows.Controls;
using Presentation.Process.DanhMucServiceRef;

namespace PresentationWPF.DanhMuc.DungChung
{
    /// <summary>
    /// Interaction logic for ucDanhMucCT.xaml
    /// </summary>
    public partial class ucDanhMucCT : UserControl
    {
        #region Khai bao

        private bool chiXem;

        public bool ChiXem
        {
            get { return chiXem; }
            set { chiXem = value; }
        }

        List<DataRow> lstChiTiet;

        public List<DataRow> LstChiTiet
        {
            get { return lstChiTiet; }
            set { lstChiTiet = value; }
        }

        private string trangThaiNV;

        public string TrangThaiNV
        {
            get { return trangThaiNV; }
            set { trangThaiNV = value; }
        }

        List<AutoCompleteEntry> lstSourceTrangThai = new List<AutoCompleteEntry>();

        private string maTrangThai;

        public string MaTrangThai
        {
            get { return maTrangThai; }
            set { maTrangThai = value; }
        }

        private int ID = 0;

        List<AutoCompleteEntry> lstSourceLoai = new List<AutoCompleteEntry>();

        private string maLoai;

        public string MaLoai
        {
            get { return maLoai; }
            set { maLoai = value; }
        }

        public static RoutedCommand AddCommand = new RoutedCommand();
        public static RoutedCommand HoldCommand = new RoutedCommand();
        public static RoutedCommand SaveCommand = new RoutedCommand();
        public static RoutedCommand ModifyCommand = new RoutedCommand();
        public static RoutedCommand DeleteCommand = new RoutedCommand();
        public static RoutedCommand ApproveCommand = new RoutedCommand();
        public static RoutedCommand RefuseCommand = new RoutedCommand();
        public static RoutedCommand CancelCommand = new RoutedCommand();
        public static RoutedCommand HelpCommand = new RoutedCommand();
        public static RoutedCommand CloseCommand = new RoutedCommand();

        #endregion

        #region Khoi tao

        public ucDanhMucCT()
        {
            InitializeComponent();
            tabChiTiet.SelectionChanged -= tabChiTiet_SelectionChanged;
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.DanhMuc;component/DungChung/ucDanhMucCT.xaml", ref Toolbar, ref mnuMain);
            foreach (var item in mnuMain.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += btnShortcutKey_Click;
            }
            BindShortkey();
            // khởi tạo combobox
            AutoComboBox auto = new AutoComboBox();
            // Khởi tạo điều kiện gọi danh mục
            List<string> lstDieuKien = new List<string>();
            // Gán giá trị điều kiện
            lstDieuKien.Add(DatabaseConstant.getValue(DatabaseConstant.DanhMuc.TRANG_THAI_BAN_GHI));
            auto.GenAutoComboBox(ref lstSourceTrangThai, ref cmbTrangThaiSDung, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien);

            // khởi tạo combobox Loại danh mục
            auto = new AutoComboBox();
            auto.GenAutoComboBox(ref lstSourceLoai, ref cmbLoaiDanhMuc, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC_LOAI.getValue());
            // khởi tạo sự kiện combobox
            cmbTrangThaiSDung.KeyDown += cmb_KeyDown;
            cmbLoaiDanhMuc.KeyDown += cmb_KeyDown;
            txtMa.Focus();
        }
        #endregion

        #region Dang ky hot key

        private void BindShortkey()
        {
            foreach (var child in Toolbar.Children)
            {
                if (child.GetType() == typeof(RibbonButton))
                {
                    RibbonButton tlb = (RibbonButton)child;
                    KeyBinding key = new KeyBinding();
                    string strTinhNang = tlb.Name.Substring(3, tlb.Name.Length - 3);
                    if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU_TAM)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.H, ModifierKeys.Control);
                        key = new KeyBinding(HoldCommand, keyg);
                        key.Gesture = keyg;
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.S, ModifierKeys.Control);
                        key = new KeyBinding(DeleteCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.M, ModifierKeys.Control);
                        key = new KeyBinding(ModifyCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.Delete, ModifierKeys.Shift);
                        key = new KeyBinding(DeleteCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DUYET)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.A, ModifierKeys.Control | ModifierKeys.Shift);
                        key = new KeyBinding(ApproveCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.R, ModifierKeys.Control | ModifierKeys.Shift);
                        key = new KeyBinding(RefuseCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.C, ModifierKeys.Control | ModifierKeys.Shift);
                        key = new KeyBinding(CancelCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRO_GIUP)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.F1, ModifierKeys.None);
                        key = new KeyBinding(HelpCommand, keyg);
                    }
                    if (key != null)
                    {
                        InputBindings.Add(key);
                    }
                }
            }
        }

        private void tlbHotKey_Click(object sender, RoutedEventArgs e)
        {
            RibbonButton tlb = (RibbonButton)sender;
            string strTinhNang = tlb.Name.Substring(3, tlb.Name.Length - 3);
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU_TAM)))
            {
                if (TrangThaiNV.Equals(BusinessConstant.TrangThaiNghiepVu.LUU_TAM))
                    Luu(BusinessConstant.TrangThaiNghiepVu.LUU_TAM.layGiaTri());
                if (TrangThaiNV.Equals(BusinessConstant.TrangThaiNghiepVu.DA_DUYET))
                    Luu(BusinessConstant.TrangThaiNghiepVu.LUU_TAM_SUA_SAU_DUYET.layGiaTri());
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU)))
            {
                if (TrangThaiNV.Equals(BusinessConstant.TrangThaiNghiepVu.LUU_TAM))
                    Luu(BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri());
                if (TrangThaiNV.Equals(BusinessConstant.TrangThaiNghiepVu.DA_DUYET))
                    Luu(BusinessConstant.TrangThaiNghiepVu.SUA_SAU_DUYET.layGiaTri());
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
                ChiXem = false;
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
            {
                Xoa();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET)))
            {
                TuChoi();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
            {
                ThoaiDuyet();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRO_GIUP)))
            {
                onHelp();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DONG)))
            {
                CustomControl.CommonFunction.CloseUserControl(this);
            }
        }

        #endregion

        #region Dang ky shortcut key

        private void HoldCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void HoldCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            tlbHold.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }

        private void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            tlbSave.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }

        private void ModifyCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void ModifyCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            tlbModify.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }

        private void DeleteCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void DeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            tlbDelete.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }

        private void ApproveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void RefuseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void CancelCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void HelpCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void HelpCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            onHelp();
        }

        private void CloseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void CloseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            CustomControl.CommonFunction.CloseUserControl(this);
        }

        private void btnShortcutKey_Click(object sender, RoutedEventArgs e)
        {
            string strTinhNang = ((MenuItem)sender).Name.Substring(3, ((MenuItem)sender).Name.Length - 3);
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU_TAM)))
            {
                if (TrangThaiNV.Equals(BusinessConstant.TrangThaiNghiepVu.LUU_TAM))
                    Luu(BusinessConstant.TrangThaiNghiepVu.LUU_TAM.layGiaTri());
                if (TrangThaiNV.Equals(BusinessConstant.TrangThaiNghiepVu.DA_DUYET))
                    Luu(BusinessConstant.TrangThaiNghiepVu.LUU_TAM_SUA_SAU_DUYET.layGiaTri());
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU)))
            {
                if (TrangThaiNV.Equals(BusinessConstant.TrangThaiNghiepVu.LUU_TAM))
                    Luu(BusinessConstant.TrangThaiNghiepVu.LUU_TAM.layGiaTri());
                if (TrangThaiNV.Equals(BusinessConstant.TrangThaiNghiepVu.DA_DUYET))
                    Luu(BusinessConstant.TrangThaiNghiepVu.LUU_TAM_SUA_SAU_DUYET.layGiaTri());
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
                ChiXem = false;
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
            {
                Xoa();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET)))
            {
                TuChoi();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
            {
                ThoaiDuyet();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRO_GIUP)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DONG)))
            {
                CustomControl.CommonFunction.CloseUserControl(this);
            }
        }

        /// <summary>
        /// Trợ giúp
        /// </summary>
        private void onHelp()
        {
            CustomControl.CommonFunction.ShowHelp(this);
        }

        #endregion

        #region Xu ly Giao dien

        /// <summary>
        /// Sự kiện ấn key trên form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            PresentationWPF.CustomControl.CommonFunction.CloseUserControl(e, this);
            PresentationWPF.CustomControl.CommonFunction.SelectNextControl(e);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (LstChiTiet != null)
            {
                UpdateLayout();
                DataRow row = LstChiTiet[0];
                if (!string.IsNullOrWhiteSpace(row[2].ToString()))
                    txtMa.Text = row[2].ToString();
                if (!string.IsNullOrWhiteSpace(row[3].ToString()))
                    txtTen.Text = row[3].ToString();
                if (!string.IsNullOrWhiteSpace(row[6].ToString()))
                {
                    lblTrangThai.Content = row[6].ToString();
                    TrangThaiNV = row[6].ToString();
                }
                if (!string.IsNullOrWhiteSpace(row[5].ToString()))
                    setMaLoaiDanhMuc(row[5].ToString());
                if (!string.IsNullOrWhiteSpace(row[7].ToString()))
                    setMaTrangThaiSDung(row[7].ToString());
                if (!string.IsNullOrWhiteSpace(row[1].ToString()))
                    ID = Convert.ToInt32(row[1]);
            }
            else
            {
                tlbDelete.IsEnabled = false;
                tlbModify.IsEnabled = false;
            }
            tabChiTiet.SelectionChanged += tabChiTiet_SelectionChanged;
            EnableControl();
        }

        private void EnableControl()
        {
            txtMa.IsEnabled = !ChiXem;
            raddtNgayNhap.IsEnabled = !ChiXem;
            raddtNgayCNhat.IsEnabled = !ChiXem;
            txtNguoiCapNhat.IsEnabled = !ChiXem;
            txtNguoiLap.IsEnabled = !ChiXem;
            txtTen.IsEnabled = !ChiXem;
            txtTrangThai.IsEnabled = !ChiXem;
            cmbLoaiDanhMuc.IsEnabled = !ChiXem;
            cmbTrangThaiSDung.IsEnabled = !ChiXem;
        }

        private void tabChiTiet_SelectionChanged(object sender, Telerik.Windows.Controls.RadSelectionChangedEventArgs e)
        {
            if (tabChiTiet.SelectedIndex == 0)
            {
                if (txtMa != null)
                {
                    UpdateLayout();
                    txtMa.Focus();
                }
            }
            else if (tabChiTiet.SelectedIndex == 1)
            {
                if (txtTrangThai != null)
                {
                    UpdateLayout();
                    txtTrangThai.Focus();
                }
            }
        }

        private void cmb_KeyDown(object sender, KeyEventArgs e)
        {
            ((RadComboBox)sender).IsDropDownOpen = true;
        }

        public void setMaTrangThaiSDung(string maChon)
        {
            cmbTrangThaiSDung.SelectedIndex = lstSourceTrangThai.IndexOf(lstSourceTrangThai.FirstOrDefault(i => i.KeywordStrings.First().Equals(maChon)));
        }

        public void setMaLoaiDanhMuc(string maChon)
        {
            cmbLoaiDanhMuc.SelectedIndex = lstSourceLoai.IndexOf(lstSourceLoai.FirstOrDefault(i => i.KeywordStrings.First().Equals(maChon)));
        }

        #endregion

        #region Xu ly Nghiep vu

        /// <summary>
        /// Xử lý xóa dữ liệu
        /// </summary>
        private void Luu(string trangThai)
        {
            DanhMucProcess danhmucProcess = new DanhMucProcess();
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                DM_DMUC_GTRI obj = new DM_DMUC_GTRI();
                if (ID > 0)
                    obj.ID = ID;
                obj.ID_DMUC_LOAI = Convert.ToInt32(lstSourceLoai.ElementAt(cmbLoaiDanhMuc.SelectedIndex).KeywordStrings.ElementAt(1));
                obj.MA_DMUC = txtMa.Text;
                obj.MA_DMUC_LOAI = lstSourceLoai.ElementAt(cmbLoaiDanhMuc.SelectedIndex).KeywordStrings.First();
                obj.MA_DVI_QLY = ClientInformation.MaDonVi;
                obj.MA_DVI_TAO = ClientInformation.MaDonVi;
                obj.NGAY_CNHAT = string.IsNullOrEmpty(raddtNgayCNhat.Value.ToString().Trim()) ? ((DateTime)raddtNgayCNhat.Value).ToString("yyyyMMdd") : (DateTime.Today).ToString("yyyyMMdd");
                obj.NGAY_NHAP = string.IsNullOrEmpty(raddtNgayNhap.Value.ToString().Trim()) ? ((DateTime)raddtNgayNhap.Value).ToString("yyyyMMdd") : (DateTime.Today).ToString("yyyyMMdd");
                obj.NGUOI_CNHAT = ClientInformation.TenDangNhap;
                if (ID == 0)
                    obj.NGUOI_NHAP = ClientInformation.TenDangNhap;
                obj.TEN_DMUC = txtTen.Text;
                obj.TTHAI_BGHI = lstSourceTrangThai.ElementAt(cmbTrangThaiSDung.SelectedIndex).KeywordStrings.First();
                obj.TTHAI_NVU = trangThai;
                if (ID == 0)
                    danhmucProcess.ThemDungChung(obj);
                else
                    danhmucProcess.SuaDungChung(obj);
                HoanThanh();
            }
            catch (System.Exception ex)
            {
                this.Cursor = Cursors.Arrow;
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
                    new frmThongBaoLoi("M.DanhMuc.ucDonViDS.LoiXoaDuLieu", ex).ShowDialog();
                }
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// Xử lý xóa dữ liệu
        /// </summary>
        private void Xoa()
        {
            if (LMessage.ShowMessage("M.DungChung.HoiXoa", LMessage.MessageBoxType.Question) == MessageBoxResult.Yes)
            {
                DanhMucProcess danhmucProcess = new DanhMucProcess();
                Mouse.OverrideCursor = Cursors.Wait;
                try
                {
                    List<int> lstID = new List<int>();
                    List<ClientResponseDetail> listResponseDetail = new List<ClientResponseDetail>();

                    lstID.Add((int)LstChiTiet[0][1]);

                    if (danhmucProcess.XoaDungChung(lstID.ToArray(), ref listResponseDetail))
                    {
                        HoanThanh();
                        LMessage.ShowMessage("M.DanhMuc.ucDonViDS.XoaThanhCong", LMessage.MessageBoxType.Information);
                    }
                    else
                    {
                        LMessage.ShowMessage("M.DanhMuc.ucDonViDS.XoaKhongThanhCong", LMessage.MessageBoxType.Warning);
                    }
                }
                catch (System.Exception ex)
                {
                    this.Cursor = Cursors.Arrow;
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
                        new frmThongBaoLoi("M.DanhMuc.ucDonViDS.LoiXoaDuLieu", ex).ShowDialog();
                    }
                    LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                }
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        /// <summary>
        /// Xử lý duyệt
        /// </summary>
        private void Duyet()
        {
            DanhMucProcess danhmucProcess = new DanhMucProcess();
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                List<int> lstID = new List<int>();
                List<ClientResponseDetail> listResponseDetail = new List<ClientResponseDetail>();

                lstID.Add((int)LstChiTiet[0][1]);

                if (danhmucProcess.DuyetDungChung(lstID.ToArray(), ref listResponseDetail))
                {
                    CustomControl.CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.DUYET, LstChiTiet[0][1].ToString());
                    LMessage.ShowMessage("M.DungChung.DuyetThanhCong", LMessage.MessageBoxType.Information);
                    HoanThanh();
                }
                else
                {
                    LMessage.ShowMessage("M.DungChung.DuyetKhongThanhCong", LMessage.MessageBoxType.Warning);
                }
            }
            catch (System.Exception ex)
            {
                this.Cursor = Cursors.Arrow;
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
                    new frmThongBaoLoi("M.DungChung.LoiDuyet", ex).ShowDialog();
                }
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void ThoaiDuyet()
        {
            DanhMucProcess danhmucProcess = new DanhMucProcess();
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                List<int> lstID = new List<int>();
                List<ClientResponseDetail> listResponseDetail = new List<ClientResponseDetail>();

                lstID.Add((int)LstChiTiet[0][1]);

                if (danhmucProcess.ThoaiDuyetDungChung(lstID.ToArray(), ref listResponseDetail))
                {
                    CustomControl.CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.THOAI_DUYET, LstChiTiet[0][1].ToString());
                    LMessage.ShowMessage("M.DungChung.ThoaiDuyetThanhCong", LMessage.MessageBoxType.Information);
                    HoanThanh();
                }
                else
                {
                    LMessage.ShowMessage("M.DungChung.ThoaiDuyetKhongThanhCong", LMessage.MessageBoxType.Warning);
                }
            }
            catch (System.Exception ex)
            {
                this.Cursor = Cursors.Arrow;
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
                    new frmThongBaoLoi("M.DungChung.LoiThoaiDuyet", ex).ShowDialog();
                }
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void TuChoi()
        {
            DanhMucProcess danhmucProcess = new DanhMucProcess();
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                List<int> lstID = new List<int>();
                List<ClientResponseDetail> listResponseDetail = new List<ClientResponseDetail>();
                lstID.Add((int)LstChiTiet[0][1]);

                if (danhmucProcess.TuChoiDungChung(lstID.ToArray(), ref listResponseDetail))
                {
                    CustomControl.CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.TU_CHOI_DUYET, LstChiTiet[0][1].ToString());
                    LMessage.ShowMessage("M.DungChung.TuChoiDuyetThanhCong", LMessage.MessageBoxType.Information);
                    HoanThanh();
                }
                else
                {
                    LMessage.ShowMessage("M.DungChung.TuChoiDuyetKhongThanhCong", LMessage.MessageBoxType.Warning);
                }
            }
            catch (System.Exception ex)
            {
                this.Cursor = Cursors.Arrow;
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
                    new frmThongBaoLoi("M.DungChung.LoiTuChoiDuyet", ex).ShowDialog();
                }
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private bool validateThongTin()
        {
            bool result = true;
            try
            {
                if (!lstSourceLoai.Select(i => i.DisplayName).Contains(cmbLoaiDanhMuc.Text))
                {

                    result = false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return result;
        }

        private void HoanThanh()
        {
            if (ID == 0 && cbMultiAdd.IsChecked.Value)
            {
                txtMa.Text = string.Empty;
                raddtNgayNhap.Value = null;
                raddtNgayCNhat.Value = null;
                txtNguoiCapNhat.Text = string.Empty;
                txtNguoiLap.Text = string.Empty;
                txtTen.Text = string.Empty;
                txtTrangThai.Text = string.Empty;
                txtMa.Focus();
            }
            else
                CustomControl.CommonFunction.CloseUserControl(this);
        }

        #endregion
    }
}
