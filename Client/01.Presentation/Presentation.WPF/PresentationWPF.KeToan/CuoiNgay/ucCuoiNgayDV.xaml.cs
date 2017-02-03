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
using PresentationWPF.CustomControl;
using System.Data;
using Utilities.Common;
using Microsoft.Windows.Controls.Ribbon;
using Telerik.Windows.Controls;
using Presentation.Process;
using Presentation.Process.Common;
using PresentationWPF.KeToan.KiemSoat;
using System.Runtime.InteropServices;

namespace PresentationWPF.KeToan.CuoiNgay
{
    /// <summary>
    /// Interaction logic for ucCuoiNgayCT.xaml
    /// </summary>
    public partial class ucCuoiNgayDV : UserControl
    {
        #region Khai bao
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool AllocConsole();

        public static RoutedCommand ExecuteCommand = new RoutedCommand();
        public static RoutedCommand HelpCommand = new RoutedCommand();
        public static RoutedCommand CloseCommand = new RoutedCommand();

        private List<AutoCompleteEntry> lstSourceDonVi = new List<AutoCompleteEntry>();

        private DataSet dtMaPhanLoai = new DataSet();

        private List<int> listLockId = new List<int>();

        private string formCase = null;

        #endregion

        #region Khoi tao
        public ucCuoiNgayDV()
        {
            InitializeComponent();
            if(formCase.IsNullOrEmptyOrSpace())
                formCase = ClientInformation.FormCase;
            //HeThong hethong = new HeThong();
            //hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.KeToan;component/PhanLoai/ucPhanLoaiDS.xaml", ref Toolbar, ref mnuGrid);
            //foreach (var item in mnuGrid.Items)
            //{
            //    if (item is MenuItem)
            //        ((MenuItem)item).Click += btnShortcutKey_Click;
            //}
            BindHotkey();
            LoadCombobox();
        }

        /// <summary>
        /// Khởi tạo các combobox
        /// </summary>
        private void LoadCombobox()
        {
            if (raddgrNghiepVu.Items.Count == 0)
            {
                try
                {
                    AutoComboBox auto = new AutoComboBox();
                    List<string> lstDieuKien = new List<string>();
                    if (formCase.Equals(BusinessConstant.LOAI_CUOI_NGAY.KHOA_SO.layGiaTri()))
                        lstDieuKien.Add(DatabaseConstant.DanhMuc.NGHIEP_VU_KHOA_SO.getValue());
                    if (formCase.Equals(BusinessConstant.LOAI_CUOI_NGAY.MO_SO.layGiaTri()))
                        lstDieuKien.Add(DatabaseConstant.DanhMuc.NGHIEP_VU_MO_SO.getValue());
                    List<AutoCompleteEntry> lstNghiepVu = new List<AutoCompleteEntry>();
                    RadComboBox cmbNghiepVu = new RadComboBox();
                    auto.GenAutoComboBox(ref lstNghiepVu, ref cmbNghiepVu, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien);

                    lblDonVi.Content = ClientInformation.TenDonVi;
                    lblNgayLamViec.Content = ClientInformation.NgayLamViecHienTai.StringToDate("yyyyMMdd").ToString("dd/MM/yyyy");
                    DataTable dtNghiepVu = new DataTable();
                    dtNghiepVu.Columns.Add("STT", typeof(string));
                    dtNghiepVu.Columns.Add("MA", typeof(string));
                    dtNghiepVu.Columns.Add("TEN", typeof(string));
                    dtNghiepVu.Columns.Add("MO_TA", typeof(string));
                    for (int i = 1; i <= lstNghiepVu.Count; i++)
                    {
                        AutoCompleteEntry entry = lstNghiepVu[i - 1];
                        dtNghiepVu.NewRow();
                        dtNghiepVu.Rows.Add(i, entry.KeywordStrings.First(), entry.DisplayName, "");
                    }
                    raddgrNghiepVu.ItemsSource = dtNghiepVu.DefaultView;
                }
                catch (Exception ex)
                {
                    CommonFunction.ThongBaoLoi(ex);
                    LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                }
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
                    if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRO_GIUP)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.F1, ModifierKeys.Shift);
                        key = new KeyBinding(HelpCommand, keyg);
                        key.Gesture = keyg;
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THUC_HIEN)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.R, ModifierKeys.Control | ModifierKeys.Shift);
                        key = new KeyBinding(ExecuteCommand, keyg);
                        key.Gesture = keyg;
                    }
                }
            }
        }

        private void ExecuteCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbExecute.IsEnabled;
        }
        private void ExecuteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (formCase.Equals(BusinessConstant.LOAI_CUOI_NGAY.KHOA_SO.layGiaTri()))
                OnProccess(BusinessConstant.NGHIEP_VU_CUOI_NGAY.NGHIEP_VU_KHOA_SO);
            if (formCase.Equals(BusinessConstant.LOAI_CUOI_NGAY.MO_SO.layGiaTri()))
                OnProccess(BusinessConstant.NGHIEP_VU_CUOI_NGAY.NGHIEP_VU_MO_SO);
            
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
            onClose();
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
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THUC_HIEN)))
            {
                if (formCase.Equals(BusinessConstant.LOAI_CUOI_NGAY.KHOA_SO.layGiaTri()))
                    OnProccess(BusinessConstant.NGHIEP_VU_CUOI_NGAY.NGHIEP_VU_KHOA_SO);
                if (formCase.Equals(BusinessConstant.LOAI_CUOI_NGAY.MO_SO.layGiaTri()))
                    OnProccess(BusinessConstant.NGHIEP_VU_CUOI_NGAY.NGHIEP_VU_MO_SO);
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRO_GIUP)))
            {
                onHelp();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DONG)))
            {
                onClose();
            }
        }

        #endregion

        #region Xu ly giao dien

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

        /// <summary>
        /// Trợ giúp
        /// </summary>
        private void onHelp()
        {
            CustomControl.CommonFunction.ShowHelp(this);
        }

        /// <summary>
        /// Đóng form
        /// </summary>
        private void onClose()
        {
            CustomControl.CommonFunction.CloseUserControl(this);
        }

        #endregion

        #region  Xu ly nghiep vu

        private void OnProccess(BusinessConstant.NGHIEP_VU_CUOI_NGAY maNghiepVu)
        {
            KeToanProcess proccess = new KeToanProcess();
            try
            {
                this.Cursor = Cursors.Wait;
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                afterModify(proccess.ThucHienCuoiNgay(maNghiepVu, DatabaseConstant.Action.THUC_HIEN, ref listClientResponseDetail, ClientInformation.MaDonVi));
                if (listClientResponseDetail.Count > 0)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("STT", typeof(string));
                    dt.Columns.Add("OBJECT", typeof(string));
                    dt.Columns.Add("RESULT", typeof(string));
                    dt.Columns.Add("DETAIL", typeof(string));
                    foreach (ClientResponseDetail detail in listClientResponseDetail)
                    {
                        dt.NewRow();
                        dt.Rows.Add(detail.Stt, detail.Object, detail.Result, detail.Detail);
                    }
                    raddgrGiaoDichLoi.ItemsSource = dt;
                    tabKetQua.IsSelected = true;
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
                proccess = null;
            }
        }

        private void afterModify(ApplicationConstant.ResponseStatus status)
        {
            if (status == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                if (formCase.Equals(BusinessConstant.LOAI_CUOI_NGAY.KHOA_SO.layGiaTri()))
                {
                    LMessage.ShowMessage("M.KeToan.CuoiNgay.ucCuoiNgayDV.KhoaSoThanhCong", LMessage.MessageBoxType.Information);
                }
                else
                {
                    LMessage.ShowMessage("M.KeToan.CuoiNgay.ucCuoiNgayDV.MoSoThanhCong", LMessage.MessageBoxType.Information);
                }
            }
            else
            {
                if (formCase.Equals(BusinessConstant.LOAI_CUOI_NGAY.KHOA_SO.layGiaTri()))
                {
                    LMessage.ShowMessage("M.KeToan.CuoiNgay.ucCuoiNgayDV.KhoaSoKhongThanhCong", LMessage.MessageBoxType.Error);
                }
                else
                {
                    LMessage.ShowMessage("M.KeToan.CuoiNgay.ucCuoiNgayDV.MoSoKhongThanhCong", LMessage.MessageBoxType.Error);
                }
            }
        }
        #endregion

        private void tlbProccess_Click(object sender, RoutedEventArgs e)
        {
            if (sender is RibbonButton)
            {
                RibbonButton rb = (RibbonButton)sender;
                BusinessConstant.NGHIEP_VU_CUOI_NGAY maNghiepVu = BusinessConstant.layEnumNghiepVuCuoiNgay(rb.Tag.ToString());
                OnProccess(maNghiepVu);
            }
        }
    }
}
