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
using PresentationWPF.CustomControl;
using Utilities.Common;
using Presentation.Process;
using System.Data;

namespace PresentationWPF.KhachHang.NhomKhachHang
{
    /// <summary>
    /// Interaction logic for ucNhomKhachHangDS.xaml
    /// </summary>
    public partial class ucNhomKhachHangDS : UserControl
    {
        public static RoutedCommand AddCommand = new RoutedCommand();
        public static RoutedCommand ModifyCommand = new RoutedCommand();
        public static RoutedCommand DeleteCommand = new RoutedCommand();
        public static RoutedCommand ApproveCommand = new RoutedCommand();
        public static RoutedCommand RefuseCommand = new RoutedCommand();
        public static RoutedCommand CancelCommand = new RoutedCommand();
        public static RoutedCommand ViewCommand = new RoutedCommand();
        public static RoutedCommand SearchCommand = new RoutedCommand();
        public static RoutedCommand ReloadCommand = new RoutedCommand();
        public static RoutedCommand ExportCommand = new RoutedCommand();
        public static RoutedCommand HelpCommand = new RoutedCommand();
        public static RoutedCommand CloseCommand = new RoutedCommand();

        public ucNhomKhachHangDS()
        {
            InitializeComponent();
            //HeThong hethong = new HeThong();
            //hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.KhachHang;component/NhomKhachHang/ucNhomKhachHangDS.xaml", ref Toolbar, ref mnuGrid);
            //foreach (var item in mnuGrid.Items)
            //{
            //    if (item is MenuItem)
            //        ((MenuItem)item).Click += btnShortcutKey_Click;
            //}
            BindHotkey();
            radPage.PageSize = (int)nudPageSize.Value;
            LoadDuLieu();
            raddtTuNgayCapNhat.Value = LDateTime.GetCurrentDate();
            raddtDenNgayCapNhat.Value = LDateTime.GetCurrentDate();
            dtpNgayCapNhatTu.SelectedDate = LDateTime.GetCurrentDate();
            dtpNgayCapNhatDen.SelectedDate = LDateTime.GetCurrentDate();
        }

        #region Dang ky hot key, shortcut key
        private void BindHotkey()
        {
            foreach (var child in Toolbar.Children)
            {
                if (child.GetType() == typeof(RibbonButton))
                {
                    RibbonButton tlb = (RibbonButton)child;
                    KeyBinding key = new KeyBinding();
                    string strTinhNang = tlb.Name.Substring(3, tlb.Name.Length - 3);
                    if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THEM)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.N, ModifierKeys.Control);
                        key = new KeyBinding(ucNhomKhachHangDS.AddCommand, keyg);
                        key.Gesture = keyg;
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.M, ModifierKeys.Control);
                        key = new KeyBinding(ucNhomKhachHangDS.ModifyCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.Delete, ModifierKeys.Shift);
                        key = new KeyBinding(ucNhomKhachHangDS.DeleteCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DUYET)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.A, ModifierKeys.Control | ModifierKeys.Shift);
                        key = new KeyBinding(ucNhomKhachHangDS.ApproveCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.R, ModifierKeys.Control | ModifierKeys.Shift);
                        key = new KeyBinding(ucNhomKhachHangDS.RefuseCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.C, ModifierKeys.Control | ModifierKeys.Shift);
                        key = new KeyBinding(ucNhomKhachHangDS.CancelCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XEM)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.W, ModifierKeys.Control);
                        key = new KeyBinding(ucNhomKhachHangDS.ViewCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TIM_KIEM)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.F, ModifierKeys.Control);
                        key = new KeyBinding(ucNhomKhachHangDS.SearchCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LAY_LAI)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.F5, ModifierKeys.None);
                        key = new KeyBinding(ucNhomKhachHangDS.ReloadCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XUAT_DU_LIEU)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.E, ModifierKeys.Control|ModifierKeys.Shift);
                        key = new KeyBinding(ucNhomKhachHangDS.ExportCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRO_GIUP)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.F1, ModifierKeys.None);
                        key = new KeyBinding(ucNhomKhachHangDS.HelpCommand, keyg);
                    }
                    if (key != null)
                    {
                        InputBindings.Add(key);
                    }
                }
            }
        }
        private void AddCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbAdd.IsEnabled;
        }
        private void AddCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Them();
        }

        private void ModifyCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbModify.IsEnabled;
        }
        private void ModifyCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Sua();
        }

        private void DeleteCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbDelete.IsEnabled;
        }
        private void DeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Xoa();
        }

        private void ApproveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbApprove.IsEnabled;
        }
        private void ApproveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            
        }

        private void RefuseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbRefuse.IsEnabled;
        }
        private void RefuseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void CancelCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbCancel.IsEnabled;
        }
        private void CancelCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void ViewCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbView.IsEnabled;
        }
        private void ViewCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void SearchCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbSearch.IsEnabled;
        }
        private void SearchCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            txtTimKiemNhanh.RaiseEvent(new RoutedEventArgs(Button.GotFocusEvent));
        }

        private void ReloadCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbReload.IsEnabled;
        }
        private void ReloadCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void ExportCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbExport.IsEnabled;
        }
        private void ExportCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            XuatExcel();
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

        /// <summary>
        /// Sự kiện hot key
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlbHotKey_Click(object sender, RoutedEventArgs e)
        {
            RibbonButton tlb = (RibbonButton)sender;
            string strTinhNang = tlb.Name.Substring(3, tlb.Name.Length - 3);
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THEM)))
                Them();
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
                Sua();
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
                Xoa();
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DUYET)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XEM)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TIM_KIEM)))
            {
                txtTimKiemNhanh.RaiseEvent(new RoutedEventArgs(Button.GotFocusEvent));
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LAY_LAI)))
                LayLai();
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XUAT_DU_LIEU)))
                XuatExcel();
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRO_GIUP)))
            {
                onHelp();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DONG)))
            {
                CustomControl.CommonFunction.CloseUserControl(this);
            }
        }

        /// <summary>
        /// Sự kiện shortcutkey
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnShortcutKey_Click(object sender, RoutedEventArgs e)
        {
            string strTinhNang = ((MenuItem)sender).Name.Substring(3, ((MenuItem)sender).Name.Length - 3);
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THEM)))
                Them();
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
                Sua();
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
                Xoa();
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DUYET)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XEM)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TIM_KIEM)))
            {
                txtTimKiemNhanh.RaiseEvent(new RoutedEventArgs(Button.GotFocusEvent));
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LAY_LAI)))
                LayLai();
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XUAT_DU_LIEU)))
                XuatExcel();
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRO_GIUP)))
            {
                onHelp();
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

        #region Xu ly giao dien
        /// <summary>
        /// Sự kiện tìm kiếm nhanh
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtTimKiemNhanh_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtTimKiemNhanh.Text == LLanguage.SearchResourceByKey("U.DungChung.TimKiemNhanh"))
            {
                return;
            }
            PresentationWPF.CustomControl.CommonFunction.QuickSearchInGrid(grNhomKhachHangDS, txtTimKiemNhanh.Text);
        }

        /// <summary>
        /// Sự kiện focus vào textbox tìm kiếm nhanh
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtTimKiemNhanh_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtTimKiemNhanh.Text == LLanguage.SearchResourceByKey("U.DungChung.TimKiemNhanh"))
            {
                txtTimKiemNhanh.Text = "";
                txtTimKiemNhanh.Focus();
            }
        }

        /// <summary>
        /// Sự kiện rời focus khỏi textbox tìm kiếm nhanh
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtTimKiemNhanh_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtTimKiemNhanh.Text))
            {
                txtTimKiemNhanh.Text = LLanguage.SearchResourceByKey("U.DungChung.TimKiemNhanh");
            }
        }

        /// <summary>
        /// Xử lý sự kiện keydown trên form
        /// Bao gồm:
        /// Nhấn Escape để thoát form
        /// Nhấn Enter/Tab để focus vào control tiếp theo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            // Kiểm tra escape thoát form
            PresentationWPF.CustomControl.CommonFunction.CloseUserControl(e, this);

            // Nhấn enter để chuyển focus tới control tiếp theo
            PresentationWPF.CustomControl.CommonFunction.SelectNextControl(e);
        }

        /// <summary>
        /// Xuất dữ liệu ra file excel
        /// </summary>
        private void XuatExcel()
        {
            PresentationWPF.CustomControl.CommonFunction.ExportGridToExcel(grNhomKhachHangDS);
            
        }

        private void dtpNgayCapNhatTu_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            raddtTuNgayCapNhat.Value = dtpNgayCapNhatTu.SelectedDate;
        }

        private void dtpNgayCapNhatDen_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            raddtDenNgayCapNhat.Value = dtpNgayCapNhatDen.SelectedDate;
        }

        #endregion

        #region Xu ly nghiep vu
        /// <summary>
        /// Xử lý sự kiện load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ucNhomKhachHangDS_Loaded(object sender, RoutedEventArgs e)
        {
            txtTimKiemNhanh.Focus();
        }

        /// <summary>
        /// Thêm mới dữ liệu
        /// </summary>
        private void Them()
        {
            ucNhomKhachHangCT uc = new ucNhomKhachHangCT();
            Window frm = new Window();
            frm.Content = uc;
            frm.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            frm.ShowDialog();
        }

        /// <summary>
        /// Sửa dữ liệu
        /// </summary>
        private void Sua()
        {
            if (grNhomKhachHangDS.SelectedItems.Count > 0)
            {
                try
                {
                    DataRowView dr = (DataRowView)grNhomKhachHangDS.SelectedItem;
                    //ucNhomKhachHangCT userControl = new PresentationWPF.KhachHang.NhomKhachHang.ucNhomKhachHangCT(Convert.ToInt32(dr["id"]), true);
                    //userControl.OnSavingCompleted += new EventHandler(LoadDuLieu);
                    Window frm = new Window();
                    //frm.Content = userControl;
                    frm.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    frm.ShowDialog();
                }
                catch (Exception ex)
                {
                    //LMessage.ShowMessage("M.DanhMuc.ucDonViDS.LoiSuaDuLieu", LMessage.MessageBoxType.Error);
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
                        new frmThongBaoLoi("M.DanhMuc.ucDonViDS.LoiSuaDuLieu", ex).ShowDialog();
                    }
                    LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                }
            }
        }

        /// <summary>
        /// Xóa dữ liệu
        /// </summary>
        private void Xoa()
        {
            if (LMessage.ShowMessage("Thực hiện xóa dữ liệu ?", LMessage.MessageBoxType.Question) == MessageBoxResult.Yes)
            {
                KhachHangProcess process = new KhachHangProcess();
                int[] arrayID = new int[0];
                Mouse.OverrideCursor = Cursors.Wait;
                try
                {
                    DataRowView dr = (DataRowView)grNhomKhachHangDS.SelectedItem;
                    Array.Resize(ref arrayID, arrayID.Length + 1);
                    arrayID[arrayID.Length - 1] = Convert.ToInt32(dr["id"]);

                    //if (process.deleteDonVi(arrayID))
                    //{
                    //    LMessage.ShowMessage("M.DungChung.XoaThanhCong", LMessage.MessageBoxType.Information);
                    //}
                    //else
                    //{
                    //    LMessage.ShowMessage("M.DungChung.XoaKhongThanhCong", LMessage.MessageBoxType.Warning);
                    //}
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
                        new frmThongBaoLoi("M.DungChung.XoaKhongThanhCong", ex).ShowDialog();
                    }
                    LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                }
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        /// <summary>
        /// Lấy lại dữ liệu
        /// </summary>
        private void LayLai()
        {
            LoadDuLieu();
        }

        /// <summary>
        /// Load dữ liệu lên datagrid
        /// </summary>
        private void LoadDuLieu()
        {
            if (!LObject.IsNullOrEmpty(grNhomKhachHangDS.SelectedItems))
                grNhomKhachHangDS.SelectedItems.Clear();
        }

        /// <summary>
        /// Xem dữ liệu
        /// </summary>
        private void Xem()
        {
            if (grNhomKhachHangDS.SelectedItems.Count > 0)
            {
                try
                {
                    DataRowView dr = (DataRowView)grNhomKhachHangDS.SelectedItem;
                    //ucNhomKhachHangCT userControl = new PresentationWPF.KhachHang.NhomKhachHang.ucNhomKhachHangCT(Convert.ToInt32(dr["id"]), true);
                    //userControl.OnSavingCompleted += new EventHandler(LoadDuLieu);
                    Window frm = new Window();
                    //frm.Content = userControl;
                    frm.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    frm.ShowDialog();
                }
                catch (Exception ex)
                {
                    //LMessage.ShowMessage("M.DanhMuc.ucDonViDS.LoiSuaDuLieu", LMessage.MessageBoxType.Error);
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
                        new frmThongBaoLoi("M.DanhMuc.ucDonViDS.LoiXemDuLieu", ex).ShowDialog();
                    }
                    LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                }
            }
        }
        #endregion

        private void grNhomKhachHangDS_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Xem();
        }
    }
}
