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
using System.Data;
using Microsoft.Windows.Controls.Ribbon;
using Utilities.Common;
using PresentationWPF.CustomControl;
using Presentation.Process;
using Telerik.Windows.Controls;
using Presentation.Process.Common;
using Presentation.Process.KeToanServiceRef;
using Telerik.Windows.Controls.GridView;

namespace PresentationWPF.KeToan.SoTaiKhoan
{
    /// <summary>
    /// Interaction logic for ucSoTaiKhoanKhaiBao.xaml
    /// </summary>
    public partial class ucSoTaiKhoanKhaiBao : UserControl
    {
        #region Khai bao
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

        private DataTable dtTreeDLy = null;
        private GHI_SO obj = new GHI_SO();
        private List<GHI_SO_KBAO> lst = null;
        private List<AutoCompleteEntry> lstSourcePGD = new List<AutoCompleteEntry>();
        #endregion

        #region Khoi tao
        public ucSoTaiKhoanKhaiBao()
        {
            InitializeComponent();
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.KeToan;component/SoTaiKhoan/ucSoTaiKhoanKhaiBao.xaml", ref Toolbar, ref mnuGrid);
            foreach (var item in mnuGrid.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += btnShortcutKey_Click;
            }
            BindHotkey();
            InitCombobox();
        }

        public void InitCombobox()
        {
            AutoComboBox au = new AutoComboBox();
            try
            {
                List<string> lstDK = new List<string>();
                lstDK.Add(Presentation.Process.Common.ClientInformation.IdDonVi.ToString());
                lstDK.Add(Presentation.Process.Common.ClientInformation.TenDangNhap);
                lstDK.Add(Presentation.Process.Common.ClientInformation.MaDonViQuanLy);
                au.GenAutoComboBox(ref lstSourcePGD, ref cmbPhongGDich, DatabaseConstant.DanhSachTruyVan.COMBOBOX_PGDLIST.getValue(), lstDK, ClientInformation.MaDonViGiaoDich);
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiLoadDuLieu", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                au = null;
            }
        }
        #endregion

        /// <summary>
        /// Dang ky hot key, shortcut key
        /// </summary>
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
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XEM)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.W, ModifierKeys.Control);
                        key = new KeyBinding(ViewCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TIM_KIEM)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.F, ModifierKeys.Control);
                        key = new KeyBinding(SearchCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LAY_LAI)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.F5, ModifierKeys.None);
                        key = new KeyBinding(ReloadCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XUAT_DU_LIEU)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.E, ModifierKeys.Control | ModifierKeys.Shift);
                        key = new KeyBinding(ExportCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRO_GIUP)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.F1, ModifierKeys.None);
                        key = new KeyBinding(HelpCommand, keyg);
                    }
                    //else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DONG)))
                    //{
                    //    KeyGesture keyg = new KeyGesture(Key.Escape, ModifierKeys.None);
                    //    key = new KeyBinding(ucDonViDS.HelpCommand, keyg);
                    //}

                    if (key != null)
                    {
                        InputBindings.Add(key);
                    }
                }
            }
        }

        private void AddCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void AddCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void ModifyCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void ModifyCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void DeleteCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void DeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void ApproveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void ApproveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void RefuseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void RefuseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void CancelCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void CancelCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void ViewCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void ViewCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void SearchCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void SearchCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void ReloadCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void ReloadCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void ExportCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void ExportCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {

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
            {

            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {

            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU)))
            {
                onSave();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
            {

            }
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
                TimKiemDuLieu();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TIM_KIEM)))
            {
                TimKiemDuLieu();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LAY_LAI)))
            { }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XUAT_DU_LIEU)))
            { }
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
        /// Sự kiện shortcutkey
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnShortcutKey_Click(object sender, RoutedEventArgs e)
        {
            string strTinhNang = ((MenuItem)sender).Name.Substring(3, ((MenuItem)sender).Name.Length - 3);
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THEM)))
            { }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            { }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU)))
            {
                onSave();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
            {
            }
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
                TimKiemDuLieu();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TIM_KIEM)))
            {
                TimKiemDuLieu();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LAY_LAI)))
            { }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XUAT_DU_LIEU)))
            { }
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
        private void chkNo_Click(object sender, RoutedEventArgs e)
        {
            var chk = sender as CheckBox;
            GridViewRow grrow = chk.ParentOfType<GridViewRow>();
            GHI_SO_KBAO obj = grrow.Item as GHI_SO_KBAO;
            obj.NO = chk.IsChecked.GetValueOrDefault().ToString();
            raddgrTaiKhoan.CurrentItem = obj;
            chkAll.IsChecked = false;
            chkNoAll.IsChecked = false;
        }

        private void chkCo_Click(object sender, RoutedEventArgs e)
        {
            var chk = sender as CheckBox;
            GridViewRow grrow = chk.ParentOfType<GridViewRow>();
            GHI_SO_KBAO obj = grrow.Item as GHI_SO_KBAO;
            obj.CO = chk.IsChecked.GetValueOrDefault().ToString();
            raddgrTaiKhoan.CurrentItem = obj;
            chkAll.IsChecked = false;
            chkCoAll.IsChecked = false;
        }

        private void chkTatCa_Click(object sender, RoutedEventArgs e)
        {
            var chk = sender as CheckBox;
            GridViewRow grrow = chk.ParentOfType<GridViewRow>();
            GHI_SO_KBAO obj = grrow.Item as GHI_SO_KBAO;
            obj.NO = chk.IsChecked.GetValueOrDefault().ToString();
            obj.CO = chk.IsChecked.GetValueOrDefault().ToString();
            obj.TATCA = chk.IsChecked.GetValueOrDefault().ToString();
            raddgrTaiKhoan.CurrentItem = obj;
            chkAll.IsChecked = false;
            chkCoAll.IsChecked = false;
            chkNoAll.IsChecked = false;
        }

        private void chkNoAll_Click(object sender, RoutedEventArgs e)
        {
            lst.ForEach(f => f.NO = chkNoAll.IsChecked.ToString());
            raddgrTaiKhoan.ItemsSource = lst;
            raddgrTaiKhoan.Rebind();
        }

        private void chkCoAll_Click(object sender, RoutedEventArgs e)
        {
            lst.ForEach(f => f.CO = chkCoAll.IsChecked.ToString());
            raddgrTaiKhoan.ItemsSource = lst;
            raddgrTaiKhoan.Rebind();
        }

        private void chkAll_Click(object sender, RoutedEventArgs e)
        {
            lst.ForEach(f => { f.NO = chkAll.IsChecked.ToString(); f.CO = chkAll.IsChecked.ToString(); f.TATCA = chkAll.IsChecked.ToString(); });
            chkCoAll.IsChecked = chkNoAll.IsChecked = chkAll.IsChecked;
            raddgrTaiKhoan.ItemsSource = lst;
            raddgrTaiKhoan.Rebind();
        }
        #endregion

        #region Xy ly nghiep vu
        private void TimKiemDuLieu()
        {
            AutoCompleteEntry au = lstSourcePGD.ElementAt(cmbPhongGDich.SelectedIndex);
            string maDonVi = au.KeywordStrings.FirstOrDefault();
            try
            {
                lst = new List<GHI_SO_KBAO>();
                DataSet ds = new KeToanProcess().getThongTinChungTuGhiSoKBao(maDonVi, ClientInformation.MaDonVi);
                if (!LObject.IsNullOrEmpty(ds) && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables["TKHOAN_KBAO"];
                    foreach (DataRow drv in dt.Rows)
                    {
                        GHI_SO_KBAO objGhiSo = new GHI_SO_KBAO();
                        objGhiSo.CO = drv["CO"].ToString();
                        objGhiSo.MA_DVI_QLY = drv["MA_DVI_QLY"].ToString();
                        objGhiSo.MA_DVI_TAO = drv["MA_DVI_TAO"].ToString();
                        objGhiSo.MA_PLOAI = drv["MA_PLOAI"].ToString();
                        objGhiSo.MA_PLOAI_CHA = drv["MA_PLOAI_CHA"].ToString();
                        objGhiSo.NO = drv["NO"].ToString();
                        objGhiSo.TATCA = drv["TATCA"].ToString();
                        objGhiSo.TEN_PLOAI = drv["TEN_PLOAI"].ToString();
                        lst.Add(objGhiSo);
                    }
                    raddgrTaiKhoan.ItemsSource = lst;
                    raddgrTaiKhoan.Rebind();
                }
            }
            catch (Exception ex)
            {
            }
        }
        private bool GetDataForm()
        {
            try
            {
                AutoCompleteEntry au = lstSourcePGD.ElementAt(cmbPhongGDich.SelectedIndex);
                if (LObject.IsNullOrEmpty(obj)) obj = new GHI_SO();
                obj.DSACH_GHI_SO_KBAO = lst.ToArray();
                obj.MA_DVI_QLY = ClientInformation.MaDonVi;
                obj.MA_DVI_TAO = au.KeywordStrings.FirstOrDefault();
                obj.NGAY_LVIEC = ClientInformation.NgayLamViecHienTai;
                obj.NGAY_NHAP = ClientInformation.NgayLamViecHienTai;
                obj.NGUOI_NHAP = ClientInformation.TenDangNhap;
                obj.TTHAI_BGHI = BusinessConstant.TrangThaiSuDung.SU_DUNG.layGiaTri();
                obj.TTHAI_NVU = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();
                return true;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                return false;
            }
        }
        private bool Validation()
        {
            return true;
        }
        /// <summary>
        /// Lưu dữ liệu (hay trình duyệt)
        /// </summary>
        private void onSave()
        {
            raddgrTaiKhoan.CommitEdit();
            if (Validation())
            {
                KeToanProcess process = new KeToanProcess();
                try
                {
                    ApplicationConstant.ResponseStatus ret = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                    List<ClientResponseDetail> listResponseDetail = new List<ClientResponseDetail>();
                    // Dữ liệu truyền vào và dữ liệu trả về

                    this.Cursor = Cursors.Wait;
                    if (GetDataForm())
                    {
                        ret = process.ChungTuGhiSoKBao(DatabaseConstant.Function.KT_CHUNG_TU_GHI_SO, DatabaseConstant.Action.THEM, ref obj, ref listResponseDetail);
                        afterSave(ret, obj, listResponseDetail);
                    }
                }
                catch (System.Exception ex)
                {
                    this.Cursor = Cursors.Arrow;
                    CommonFunction.ThongBaoLoi(ex);
                    LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                }
                finally
                {
                    this.Cursor = Cursors.Arrow;
                    process = null;
                }
            }
        }

        /// <summary>
        /// Sau khi thêm mới
        /// </summary>
        /// <param name="ret"></param>
        private void afterSave(ApplicationConstant.ResponseStatus ret, Presentation.Process.KeToanServiceRef.GHI_SO obj, List<ClientResponseDetail> listResponseDetail)
        {
            if (ret == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                LMessage.ShowMessage("M.DungChung.ThemThanhCong", LMessage.MessageBoxType.Information);
            }
            else
            {
                CommonFunction.ThongBaoKetQua(listResponseDetail);
            }
        }
        #endregion

    }
}
