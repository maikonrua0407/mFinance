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
using System.Data;
using Microsoft.Windows.Controls.Ribbon;
using Utilities.Common;
using PresentationWPF.CustomControl;
using Telerik.Windows.Controls;
using Presentation.Process;
using Presentation.Process.KeToanServiceRef;
using Presentation.Process.Common;

namespace PresentationWPF.KeToan.TaiKhoanTongHop
{
    /// <summary>
    /// Interaction logic for ucPhanLoaiDS.xaml
    /// </summary>
    public partial class ucTKTHDS : UserControl
    {
        #region Khai bao
        public static RoutedCommand AddCommand = new RoutedCommand();
        public static RoutedCommand ModifyCommand = new RoutedCommand();
        public static RoutedCommand DeleteCommand = new RoutedCommand();
        public static RoutedCommand ViewCommand = new RoutedCommand();
        public static RoutedCommand SearchCommand = new RoutedCommand();
        public static RoutedCommand ReloadCommand = new RoutedCommand();
        public static RoutedCommand ExportCommand = new RoutedCommand();
        public static RoutedCommand HelpCommand = new RoutedCommand();
        public static RoutedCommand CloseCommand = new RoutedCommand();

        private List<AutoCompleteEntry> lstSourceDonVi = new List<AutoCompleteEntry>();

        private TKTONGHOP dtMaPhanLoai = new TKTONGHOP();
        #endregion

        #region Khoi tao
        public ucTKTHDS()
        {
            InitializeComponent();
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.KeToan;component/TaiKhoanTongHop/ucTKTHDS.xaml", ref Toolbar, ref mnuGrid);
            foreach (var item in mnuGrid.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += tlbHotKey_Click;
            }
            BindHotkey();
            radPage.PageSize = (int)nudPageSize.Value;
            InitCombobox();
            cmbDonVi.SelectionChanged += new SelectionChangedEventHandler(cmbDonVi_SelectionChanged);
            trvPhanLoai.SelectionChanged += trvPhanLoai_SelectionChanged;
        }

        public void InitCombobox()
        {
            AutoComboBox au = new AutoComboBox();
            List<string> lstDieuKien = new List<string>();
            try
            {
                //List<string> lstDK = new List<string>();
                //lstDK.Add(Presentation.Process.Common.ClientInformation.IdDonViGiaoDich.ToString());
                //au.GenAutoComboBox(ref lstSourceDonVi, ref cmbDonVi, DatabaseConstant.DanhSachTruyVan.COMBOBOX_CHI_NHANH.getValue(), lstDK);

                au.GenAutoComboBox(ref lstSourceDonVi, ref cmbDonVi, "COMBOBOX_HE_THONG_TKHOAN_TH", null);
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiLoadDuLieu", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                au = null;
                lstDieuKien = null;
            }
        }
        #endregion

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
                        key = new KeyBinding(AddCommand, keyg);
                        key.Gesture = keyg;
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.M, ModifierKeys.Control);
                        key = new KeyBinding(ModifyCommand, keyg);
                        key.Gesture = keyg;
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.Delete, ModifierKeys.Shift);
                        key = new KeyBinding(DeleteCommand, keyg);
                        key.Gesture = keyg;
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XEM)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.W, ModifierKeys.Control);
                        key = new KeyBinding(ViewCommand, keyg);
                        key.Gesture = keyg;
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TIM_KIEM)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.F, ModifierKeys.Control);
                        key = new KeyBinding(SearchCommand, keyg);
                        key.Gesture = keyg;
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LAY_LAI)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.F5, ModifierKeys.None);
                        key = new KeyBinding(ReloadCommand, keyg);
                        key.Gesture = keyg;
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XUAT_DU_LIEU)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.E, ModifierKeys.Control | ModifierKeys.Shift);
                        key = new KeyBinding(ExportCommand, keyg);
                        key.Gesture = keyg;
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRO_GIUP)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.F1, ModifierKeys.None);
                        key = new KeyBinding(HelpCommand, keyg);
                        key.Gesture = keyg;
                    }
                    if (key != null)
                        InputBindings.Add(key);
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

        private void ViewCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbView.IsEnabled;
        }
        private void ViewCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Xem();
        }

        private void SearchCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbSearch.IsEnabled;
        }
        private void SearchCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            TimKiem();
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
            //RibbonButton tlb = (RibbonButton)sender;
            string strTinhNang = ""; // tlb.Name.Substring(3, tlb.Name.Length - 3);

            if (sender is RibbonButton)
                strTinhNang = ((RibbonButton)sender).Name.Substring(3, ((RibbonButton)sender).Name.Length - 3);
            else
                strTinhNang = ((MenuItem)sender).Name.Substring(3, ((MenuItem)sender).Name.Length - 3);

            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THEM)))
            {
                Them();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
                Sua();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
            {
                Xoa();
            }

            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XEM)))
            {
                Xem();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TIM_KIEM)))
            {
                TimKiem();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LAY_LAI)))
            {
                TimKiem();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XUAT_DU_LIEU)))
            {
                XuatExcel();
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

        /// <summary>
        /// Trợ giúp
        /// </summary>
        private void onHelp()
        {
            CustomControl.CommonFunction.ShowHelp(this);
        }

        /// <summary>
        /// Sự kiện double click trên data grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grKhachHangDS_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Xem();
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
            PresentationWPF.CustomControl.CommonFunction.QuickSearchInGrid(raddgrDanhSachTK, txtTimKiemNhanh.Text);
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
        /// Sự kiện thay đổi số bản ghi trên một page hiển thị
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nudPageSize_ValueChanged(object sender, RadRangeBaseValueChangedEventArgs e)
        {
            if (raddgrDanhSachTK != null && raddgrDanhSachTK.DataContext != null)
            {
                DataTable dt = ((DataView)raddgrDanhSachTK.DataContext).Table;
                if (dt != null)
                {
                    radPage.PageSize = (int)nudPageSize.Value;
                    raddgrDanhSachTK.DataContext = dt.DefaultView;
                }
            }
        }

        /// <summary>
        /// Xuất dữ liệu ra file excel
        /// </summary>
        private void XuatExcel()
        {
            PresentationWPF.CustomControl.CommonFunction.ExportGridToExcel(raddgrDanhSachTK);
            //PresentationWPF.CustomControl.CommonFunction.ExportGridToExcel("GET_DS_DONVI");
        }

        /// <summary>
        /// Xử lý sự kiện escape thoát form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            PresentationWPF.CustomControl.CommonFunction.CloseUserControl(e, this);
            PresentationWPF.CustomControl.CommonFunction.SelectNextControl(e);
        }

        void cmbDonVi_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TimKiem();
        }
        #endregion

        void trvPhanLoai_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List<string> lst = new List<string>();
            RadTreeViewItem subItem = new RadTreeViewItem();
            subItem = (RadTreeViewItem)trvPhanLoai.SelectedItem;
            GetValueTreeView(subItem, ref lst);
            BuildGrid(lst);
        }

        void GetValueTreeView(RadTreeViewItem item, ref List<string> lst)
        {
            lst.Add(item.Tag.ToString());
            foreach(RadTreeViewItem subitem in item.Items)
            {
                GetValueTreeView(subitem, ref lst);
            }
        }
        private void TaoTreeView()
        {
            try
            {
                if(!dtMaPhanLoai.IsNullOrEmpty())
                {
                    List<TKTONGHOP_CTIET> lstTKhoan = new List<TKTONGHOP_CTIET>();
                    lstTKhoan = dtMaPhanLoai.DSACHHTTLTHCT.Where(f => f.MA_TKTH_CHA == null || f.MA_TKTH_CHA=="").ToList();
                    if (!lstTKhoan.IsNullOrEmpty())
                    {
                        foreach (TKTONGHOP_CTIET objTKhoan in lstTKhoan)
                        {
                            RadTreeViewItem subItem = new RadTreeViewItem();
                            subItem.Header = objTKhoan.MA_TKTH + " - " + objTKhoan.TEN_TKTH;
                            subItem.Tag = objTKhoan.MA_TKTH;
                            //subItem.IsExpanded = true;
                            subItem.IsChecked = true;
                            TaoSubTreeView(subItem, objTKhoan.MA_TKTH);
                            trvPhanLoai.Items.Add(subItem);
                        }
                    }
                }
                
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiLoadDuLieu", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                
            }
        }

        private void TaoSubTreeView(RadTreeViewItem itemRoot, string maPhanLoai)
        {
            List<TKTONGHOP_CTIET> lstTKhoan = new List<TKTONGHOP_CTIET>();
            lstTKhoan = dtMaPhanLoai.DSACHHTTLTHCT.Where(f => f.MA_TKTH_CHA == maPhanLoai).ToList();
            if(!lstTKhoan.IsNullOrEmpty())
            {
                foreach(TKTONGHOP_CTIET objTKhoan in lstTKhoan)
                {
                    RadTreeViewItem subItem = new RadTreeViewItem();
                    subItem.Header = objTKhoan.MA_TKTH + " - " + objTKhoan.TEN_TKTH;
                    subItem.Tag = objTKhoan.MA_TKTH;
                    //subItem.IsExpanded = true;
                    subItem.IsChecked = true;
                    itemRoot.Items.Add(subItem);
                    TaoSubTreeView(subItem, objTKhoan.MA_TKTH);
                }
            }
        }

        #region Xu ly nghiep vu
        /// <summary>
        /// Xử lý sự kiện thêm
        /// </summary>
        private void Them()
        {
            Window window = new Window();
            ucTKTHCT uc = new ucTKTHCT();
            uc.TthaiNvu = "";
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.Title = DatabaseConstant.layNgonNguTieuDeForm(DatabaseConstant.Function.KT_TAI_KHOAN_TH);

            window.Content = uc;
            window.ShowDialog();
            TimKiem();
        }

        /// <summary>
        /// Xử lý sự kiện sửa
        /// </summary>
        private void Sua()
        {
            TKTONGHOP_CTIET dr = (TKTONGHOP_CTIET)raddgrDanhSachTK.SelectedItem;
            if (dr == null)
            {
                LMessage.ShowMessage("M.DungChung.ChuaChonBanGhi", LMessage.MessageBoxType.Warning);
            }
            else
            {
                Window window = new Window();
                ucTKTHCT uc = new ucTKTHCT(dr.ID,dr.TTHAI_NVU, DatabaseConstant.Action.SUA);
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                window.Title = DatabaseConstant.layNgonNguTieuDeForm(DatabaseConstant.Function.KT_TAI_KHOAN_TH);
                window.Content = uc;
                window.ShowDialog();
                TimKiem();
            }
        }

        /// <summary>
        /// Xử lý sự kiện sửa
        /// </summary>
        private void Xem()
        {
            TKTONGHOP_CTIET dr = (TKTONGHOP_CTIET)raddgrDanhSachTK.SelectedItem;
            if (dr == null)
            {
                LMessage.ShowMessage("M.DungChung.ChuaChonBanGhi", LMessage.MessageBoxType.Warning);
            }
            else
            {
                Window window = new Window();
                ucTKTHCT uc = new ucTKTHCT(dr.ID, dr.TTHAI_NVU, DatabaseConstant.Action.XEM);
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                window.Title = DatabaseConstant.layNgonNguTieuDeForm(DatabaseConstant.Function.KT_TAI_KHOAN_TH);
                window.Content = uc;
                window.ShowDialog();
                TimKiem();
            }
        }

        /// <summary>
        /// Xử lý xóa dữ liệu
        /// </summary>
        private void Xoa()
        {
            try
            {
                KeToanProcess process = new KeToanProcess();
                List<int> lstKtTkhoan = new List<int>();
                List<ClientResponseDetail> lstResponse = new List<ClientResponseDetail>();
                Mouse.OverrideCursor = Cursors.Wait;
                try
                {
                    foreach (TKTONGHOP_CTIET dr in raddgrDanhSachTK.SelectedItems)
                    {
                        lstKtTkhoan.Add(dr.ID);
                    }

                    if (lstKtTkhoan.Count == 0)
                    {
                        LMessage.ShowMessage("M.DungChung.ChuaChonBanGhi", LMessage.MessageBoxType.Warning);
                        return;
                    }

                    MessageBoxResult messResult = LMessage.ShowMessage("M.DungChung.HoiXoa", LMessage.MessageBoxType.Question);
                    if (messResult == MessageBoxResult.Yes)
                    {
                        TKTONGHOP objHeThong = new TKTONGHOP();
                        objHeThong.DSACHID = lstKtTkhoan.ToArray();
                        ApplicationConstant.ResponseStatus ret = process.TaiKhoanTongHop(DatabaseConstant.Action.XOA, ref objHeThong, ref lstResponse);
                        CommonFunction.ThongBaoKetQua(lstResponse);
                        TimKiem();
                    }
                }
                catch (System.Exception ex)
                {
                    this.Cursor = Cursors.Arrow;
                    CommonFunction.ThongBaoLoi(ex);
                    LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                }
                Mouse.OverrideCursor = Cursors.Arrow;
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        /// <summary>
        /// Tìm kiếm nâng cao
        /// </summary>
        private void TimKiem()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            List<ClientResponseDetail> lstResponse = new List<ClientResponseDetail>();
            KeToanProcess process = new KeToanProcess();
            try
            {
                TKTONGHOP objHeThong = new TKTONGHOP();
                objHeThong.HTTKTHCT = new TKTONGHOP_CTIET();
                if (cmbDonVi.SelectedIndex < 0)
                    return;
                AutoCompleteEntry au = lstSourceDonVi.ElementAt(cmbDonVi.SelectedIndex);
                objHeThong.HTTKTHCT.MA_HT_TKTH = au.KeywordStrings.FirstOrDefault();
                if (process.TaiKhoanTongHop(DatabaseConstant.Action.LOAD, ref objHeThong, ref lstResponse) == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    dtMaPhanLoai = objHeThong;
                    TaoTreeView();
                }
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiLoadDuLieu", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                process = null;
            }

            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void BuildGrid(List<string> lst)
        {
            List<TKTONGHOP_CTIET> lstTKhoan = new List<TKTONGHOP_CTIET>();
            lstTKhoan = dtMaPhanLoai.DSACHHTTLTHCT.Where(f => lst.Contains(f.MA_TKTH)).ToList();
            raddgrDanhSachTK.ItemsSource = lstTKhoan;
            raddgrDanhSachTK.Rebind();
        }
        #endregion

    }
}
