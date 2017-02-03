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
using Microsoft.Windows.Controls.Ribbon;
using PresentationWPF.CustomControl;

namespace PresentationWPF.KhachHang.NhomKhachHang
{
    /// <summary>
    /// Interaction logic for ucNhomKhachHang.xaml
    /// </summary>
    public partial class ucNhomKhachHangCT : UserControl
    {
        #region Khai bao
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
        public ucNhomKhachHangCT()
        {
            InitializeComponent();
            //HeThong hethong = new HeThong();
            //hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.KhachHang;component/NhomKhachHang/ucNhomKhachHangCT.xaml", ref Toolbar, ref mnuMain);
            //foreach (var item in mnuMain.Items)
            //{
            //    if (item is MenuItem)
            //        ((MenuItem)item).Click += btnShortcutKey_Click;
            //}
            txtMaNhom.Focus();
            lblTenCum.Content = "";
            raddtNgayTao.Value = LDateTime.GetCurrentDate();
            dtpNgayTao.SelectedDate = raddtNgayTao.Value = LDateTime.GetCurrentDate();
        }
        #endregion

        #region Dang ky hot key, shortcut key

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
                        key = new KeyBinding(ucNhomKhachHangCT.HoldCommand, keyg);
                        key.Gesture = keyg;
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.M, ModifierKeys.Control);
                        key = new KeyBinding(ucNhomKhachHangCT.ModifyCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.Delete, ModifierKeys.Shift);
                        key = new KeyBinding(ucNhomKhachHangCT.DeleteCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DUYET)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.A, ModifierKeys.Control | ModifierKeys.Shift);
                        key = new KeyBinding(ucNhomKhachHangCT.ApproveCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.R, ModifierKeys.Control | ModifierKeys.Shift);
                        key = new KeyBinding(ucNhomKhachHangCT.RefuseCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.C, ModifierKeys.Control | ModifierKeys.Shift);
                        key = new KeyBinding(ucNhomKhachHangCT.CancelCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRO_GIUP)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.F1, ModifierKeys.None);
                        key = new KeyBinding(ucNhomKhachHangCT.HelpCommand, keyg);
                    }
                    if (key != null)
                    {
                        InputBindings.Add(key);
                    }
                }
            }
        }

        private void HoldCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbHold.IsEnabled;
        }
        private void HoldCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
        }

        private void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbSave.IsEnabled;
        }
        private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
        }

        private void ModifyCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbModify.IsEnabled;
        }
        private void ModifyCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            tlbModify.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }

        private void DeleteCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbDelete.IsEnabled;
        }
        private void DeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            tlbDelete.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }

        private void ApproveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbApprove.IsEnabled;
        }
        private void ApproveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            tlbApprove.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }

        private void RefuseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbRefuse.IsEnabled;
        }
        private void RefuseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            tlbRefuse.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }

        private void CancelCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbCancel.IsEnabled;
        }
        private void CancelCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            tlbCancel.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
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

        private void tlbHotKey_Click(object sender, RoutedEventArgs e)
        {
            RibbonButton tlb = (RibbonButton)sender;
            string strTinhNang = tlb.Name.Substring(3, tlb.Name.Length - 3);
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU_TAM)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
            {
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

        private void btnShortcutKey_Click(object sender, RoutedEventArgs e)
        {
            string strTinhNang = ((MenuItem)sender).Name.Substring(3, ((MenuItem)sender).Name.Length - 3);
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU_TAM)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
            {
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

        #endregion

        #region Xu ly giao dien
        /// <summary>
        /// Sự kiện nhấn key trên form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            PresentationWPF.CustomControl.CommonFunction.CloseUserControl(e, this);
            PresentationWPF.CustomControl.CommonFunction.SelectNextControl(e);
        }

        /// <summary>
        /// Sự kiện bật popup cụm
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMaCum_Click(object sender, RoutedEventArgs e)
        {

        }

        /// <summary>
        /// Sự kiện nhấn F3 để bật popup cụm
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtMaCum_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F3)
            {
                btnMaCum_Click(null, null);
            }
        }

        #endregion

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            raddtNgayTao.Value = dtpNgayTao.SelectedDate;
        }

        #region Xu ly nghiep vu

        /// <summary>
        /// Tạo trạng thái bản ghi dựa theo hành động và trạng thái hiện tại của bản ghi
        /// </summary>
        /// <param name="action">Hành động</param>
        /// <param name="trangthaiHienTai">Trạng thái hiện tại của bản ghi</param>
        /// <returns></returns>
        private string TaoTrangThaiBanGhi(DatabaseConstant.Action action, string trangthaiHienTai)
        {
            if (string.IsNullOrEmpty(trangthaiHienTai)
                        || trangthaiHienTai == BusinessConstant.layGiaTri(BusinessConstant.TrangThaiNghiepVu.LUU_TAM)
                        || trangthaiHienTai == BusinessConstant.layGiaTri(BusinessConstant.TrangThaiNghiepVu.TU_CHOI)
                        || trangthaiHienTai == BusinessConstant.layGiaTri(BusinessConstant.TrangThaiNghiepVu.THOAI_DUYET)
                )
            {
                if (action == DatabaseConstant.Action.LUU_TAM)
                {
                    return BusinessConstant.layGiaTri(BusinessConstant.TrangThaiNghiepVu.LUU_TAM);
                }
                else if (action == DatabaseConstant.Action.LUU)
                {
                    return BusinessConstant.layGiaTri(BusinessConstant.TrangThaiNghiepVu.CHO_DUYET);
                }
            }
            else
            {
                if (trangthaiHienTai == BusinessConstant.layGiaTri(BusinessConstant.TrangThaiNghiepVu.CHO_DUYET))
                {
                    if (action == DatabaseConstant.Action.LUU_TAM)
                    {
                        return BusinessConstant.layGiaTri(BusinessConstant.TrangThaiNghiepVu.LUU_TAM);
                    }
                    else if (action == DatabaseConstant.Action.LUU)
                    {
                        return BusinessConstant.layGiaTri(BusinessConstant.TrangThaiNghiepVu.CHO_DUYET);
                    }
                    else if (action == DatabaseConstant.Action.DUYET)
                    {
                        return BusinessConstant.layGiaTri(BusinessConstant.TrangThaiNghiepVu.DA_DUYET);
                    }
                    else if (action == DatabaseConstant.Action.TU_CHOI_DUYET)
                    {
                        return BusinessConstant.layGiaTri(BusinessConstant.TrangThaiNghiepVu.TU_CHOI);
                    }
                }
                else if (trangthaiHienTai == BusinessConstant.layGiaTri(BusinessConstant.TrangThaiNghiepVu.DA_DUYET))
                {
                    if (action == DatabaseConstant.Action.LUU_TAM)
                    {
                        // Lưu tạm sửa sau duyệt
                        return "";
                    }
                    else if (action == DatabaseConstant.Action.LUU)
                    {
                        return BusinessConstant.layGiaTri(BusinessConstant.TrangThaiNghiepVu.SUA_SAU_DUYET);
                    }
                    else if (action == DatabaseConstant.Action.THOAI_DUYET)
                    {
                        return BusinessConstant.layGiaTri(BusinessConstant.TrangThaiNghiepVu.THOAI_DUYET);
                    }
                }
                else if (trangthaiHienTai == BusinessConstant.layGiaTri(BusinessConstant.TrangThaiNghiepVu.SUA_SAU_DUYET))
                {
                    if (action == DatabaseConstant.Action.LUU)
                    {
                        return BusinessConstant.layGiaTri(BusinessConstant.TrangThaiNghiepVu.SUA_SAU_DUYET);
                    }
                    else if (action == DatabaseConstant.Action.LUU_TAM)
                    {
                        // Lưu tạm sửa sau duyệt
                        return "";
                    }
                    else if (action == DatabaseConstant.Action.DUYET)
                    {
                        return BusinessConstant.layGiaTri(BusinessConstant.TrangThaiNghiepVu.DA_DUYET);
                    }
                    else if (action == DatabaseConstant.Action.TU_CHOI_DUYET)
                    {
                        // Từ chối sửa sau duyệt
                        return "";
                    }
                }
                else
                {
                    if (action == DatabaseConstant.Action.LUU)
                    {
                        // Sửa sau duyệt
                        return BusinessConstant.layGiaTri(BusinessConstant.TrangThaiNghiepVu.SUA_SAU_DUYET);
                    }
                    else if (action == DatabaseConstant.Action.LUU_TAM)
                    {
                        //Lưu tạm sửa sau duyệt
                        return "";
                    }
                }
            }
            return "";
        }

        #endregion
    }
}
