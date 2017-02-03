using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
using Presentation.Process;
using Presentation.Process.Common;
using Presentation.Process.SMSServiceRef;
using Presentation.Process.ZATestAppServiceRef;
using PresentationWPF.CustomControl;
using Utilities.Common;

namespace PresentationWPF.SMS.ThietBiSMS
{
    /// <summary>
    /// Interaction logic for ucDangKyDichVu.xaml
    /// </summary>
    public partial class ucThietBiCT : UserControl
    {
        #region Khai bao
        public static RoutedCommand ImportCommand = new RoutedCommand();
        public static RoutedCommand ModifyCommand = new RoutedCommand();
        public static RoutedCommand DeleteCommand = new RoutedCommand();
        public static RoutedCommand CloneCommand = new RoutedCommand();
        public static RoutedCommand HoldCommand = new RoutedCommand();
        public static RoutedCommand SubmitCommand = new RoutedCommand();
        public static RoutedCommand CashStmtCommand = new RoutedCommand();
        public static RoutedCommand ApproveCommand = new RoutedCommand();
        public static RoutedCommand CancelCommand = new RoutedCommand();
        public static RoutedCommand RefuseCommand = new RoutedCommand();
        public static RoutedCommand PreviewCommand = new RoutedCommand();
        public static RoutedCommand ViewCommand = new RoutedCommand();
        public static RoutedCommand ExportCommand = new RoutedCommand();
        public static RoutedCommand SearchCommand = new RoutedCommand();
        public static RoutedCommand HelpCommand = new RoutedCommand();
        public static RoutedCommand CloseCommand = new RoutedCommand();

        private List<AutoCompleteEntry> lstSourcePortName = new List<AutoCompleteEntry>();
        private List<AutoCompleteEntry> lstSourceTimeOut = new List<AutoCompleteEntry>();
        private List<AutoCompleteEntry> lstSourceBaurate = new List<AutoCompleteEntry>();
        private List<AutoCompleteEntry> lstNetworkName = new List<AutoCompleteEntry>();

        List<MODEM> lstModems = new List<MODEM>();
        private string tthai = "";
        #endregion
        /// <summary>
        /// Dang ky hot key, shortcut key
        /// </summary>
        #region Dang ky hot key, shortcut key
        /// <summary>
        /// Binding HotKey
        /// </summary>
        private void BindHotkey()
        {
            foreach (var child in Toolbar.Children)
            {
                if (child.GetType() == typeof(RibbonButton))
                {
                    RibbonButton tlb = (RibbonButton)child;
                    KeyBinding key = new KeyBinding();
                    string strTinhNang = tlb.Name.Substring(3, tlb.Name.Length - 3);
                    if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.NHAP_DU_LIEU)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.I, ModifierKeys.Control);
                        key = new KeyBinding(ImportCommand, keyg);
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
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.NHAN_BAN)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.V, ModifierKeys.Control | ModifierKeys.Shift);
                        key = new KeyBinding(CloneCommand, keyg);
                        key.Gesture = keyg;
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU_TAM)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.H, ModifierKeys.Control);
                        key = new KeyBinding(HoldCommand, keyg);
                        key.Gesture = keyg;
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.S, ModifierKeys.Control);
                        key = new KeyBinding(SubmitCommand, keyg);
                        key.Gesture = keyg;
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.BANG_KE)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.S, ModifierKeys.Control | ModifierKeys.Shift);
                        key = new KeyBinding(CashStmtCommand, keyg);
                        key.Gesture = keyg;
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DUYET)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.A, ModifierKeys.Control | ModifierKeys.Shift);
                        key = new KeyBinding(ApproveCommand, keyg);
                        key.Gesture = keyg;
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.R, ModifierKeys.Control | ModifierKeys.Shift);
                        key = new KeyBinding(RefuseCommand, keyg);
                        key.Gesture = keyg;
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.C, ModifierKeys.Control | ModifierKeys.Shift);
                        key = new KeyBinding(CancelCommand, keyg);
                        key.Gesture = keyg;
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XEM_TRUOC)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.W, ModifierKeys.Control | ModifierKeys.Shift);
                        key = new KeyBinding(PreviewCommand, keyg);
                        key.Gesture = keyg;
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XEM)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.W, ModifierKeys.Control);
                        key = new KeyBinding(ViewCommand, keyg);
                        key.Gesture = keyg;
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XUAT_DU_LIEU)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.E, ModifierKeys.Shift);
                        key = new KeyBinding(ExportCommand, keyg);
                        key.Gesture = keyg;
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TIM_KIEM)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.F, ModifierKeys.Control | ModifierKeys.Shift);
                        key = new KeyBinding(SearchCommand, keyg);
                        key.Gesture = keyg;
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRO_GIUP)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.F1, ModifierKeys.None);
                        key = new KeyBinding(HelpCommand, keyg);
                        key.Gesture = keyg;
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DONG)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.Escape, ModifierKeys.None);
                        key = new KeyBinding(CloseCommand, keyg);
                        key.Gesture = keyg;
                    }

                    InputBindings.Add(key);
                }
            }
        }
        private void ImportCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void ImportCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Nhập dữ liệu");
        }
        private void ModifyCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void ModifyCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Sửa dữ liệu");
        }
        private void DeleteCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void DeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Xóa dữ liệu");
        }
        private void CloneCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void CloneCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Nhân bản dữ liệu");
        }
        private void HoldCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void HoldCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Lưu tạm dữ liệu");
        }
        private void SubmitCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void SubmitCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Lưu dữ liệu");
        }
        private void CashStmtCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void CashStmtCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Bảng kê tiền mặt");
        }
        private void ApproveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void ApproveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Duyệt dữ liệu");
        }
        private void CancelCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void CancelCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Hủy duyệt dữ liệu");
        }
        private void RefuseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void RefuseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Từ chối dữ liệu");
        }
        private void PreviewCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void PreviewCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Xem trước dữ liệu");
        }
        private void ViewCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void ViewCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Xem dữ liệu");
        }
        private void ExportCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void ExportCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Xuất dữ liệu");
        }
        private void SearchCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void SearchCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Tìm kiếm dữ liệu");
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
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.NHAP_DU_LIEU)))
            { }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THEM)))
            {
                Them();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THUC_HIEN)))
            {
                Process();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
            {
                //Xoa();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.NHAN_BAN)))
            { }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU_TAM)))
            {
                //LuuDuLieu(BusinessConstant.TrangThaiBanGhi.SU_DUNG, BusinessConstant.TrangThaiNghiepVu.LUU_TAM, DatabaseConstant.Action.LUU_TAM);
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRINH_DUYET)))
            {
                //LuuDuLieu(BusinessConstant.TrangThaiBanGhi.SU_DUNG, BusinessConstant.TrangThaiNghiepVu.CHO_DUYET, DatabaseConstant.Action.TRINH_DUYET);
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.BANG_KE)))
            { }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DUYET)))
            {
                //Duyet();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET)))
            {
                //TuChoiDuyet();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
            {
                //ThoaiDuyet();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XEM_TRUOC)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XEM)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XUAT_DU_LIEU)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TIM_KIEM)))
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
        /// Sự kiện shortcutkey
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnShortcutKey_Click(object sender, RoutedEventArgs e)
        {
            string strTinhNang = ((MenuItem)sender).Name.Substring(3, ((MenuItem)sender).Name.Length - 3);
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.NHAP_DU_LIEU)))
            { }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THEM)))
            {
                Them();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THUC_HIEN)))
            {
                Process();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
            {
                //Xoa();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.NHAN_BAN)))
            { }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU_TAM)))
            {
                //LuuDuLieu(BusinessConstant.TrangThaiBanGhi.SU_DUNG, BusinessConstant.TrangThaiNghiepVu.LUU_TAM, DatabaseConstant.Action.LUU_TAM);
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRINH_DUYET)))
            {
                //LuuDuLieu(BusinessConstant.TrangThaiBanGhi.SU_DUNG, BusinessConstant.TrangThaiNghiepVu.CHO_DUYET, DatabaseConstant.Action.TRINH_DUYET);
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.BANG_KE)))
            { }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DUYET)))
            {
                //Duyet();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET)))
            {
                //TuChoiDuyet();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
            {
                //ThoaiDuyet();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XEM_TRUOC)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XEM)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XUAT_DU_LIEU)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TIM_KIEM)))
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

        #region Khoi tao
        public ucThietBiCT()
        {
            InitializeComponent();
            KhoiTaoCombobox();
        }

        public ucThietBiCT(MODEM _modem) : this()
        {
            cmbPort.SelectedIndex =
                lstSourcePortName.IndexOf(
                    lstSourcePortName.FirstOrDefault(
                        f => f.KeywordStrings.FirstOrDefault().Equals(_modem.PORT.ToString())));
            cmbBaurate.SelectedIndex =
                lstSourceBaurate.IndexOf(
                    lstSourceBaurate.FirstOrDefault(
                        f => f.KeywordStrings.FirstOrDefault().Equals(_modem.BAURATE.ToString())));
            cmbTimeOut.SelectedIndex =
                lstSourceTimeOut.IndexOf(
                    lstSourceTimeOut.FirstOrDefault(
                        f => f.KeywordStrings.FirstOrDefault().Equals(_modem.TIMEOUT.ToString())));
            numSecond.Value = _modem.sendSecond;
            numMinute.Value = _modem.sendMin;
            numHour.Value = _modem.sendhour;
            numDay.Value = _modem.sendDay;
            tthai = _modem.sTatus;
            if (tthai.Equals(BusinessConstant.CoKhong.KHONG.layGiaTri()))
            {
                lblTrangThai.Content = LLanguage.SearchResourceByKey("U.DMUC_GTRI.CO_KHONG.KHONG");
            }
            else
            {
                lblTrangThai.Content = LLanguage.SearchResourceByKey("U.DMUC_GTRI.CO_KHONG.CO");
            }
        }
        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            PresentationWPF.CustomControl.CommonFunction.CloseUserControl(e, this);
            PresentationWPF.CustomControl.CommonFunction.SelectNextControl(e);
        }

        private void KhoiTaoCombobox()
        {
            AutoCompleteEntry auto = null;
            lstSourceTimeOut = new List<AutoCompleteEntry>();
            lstSourceBaurate = new List<AutoCompleteEntry>();
            lstSourcePortName = new List<AutoCompleteEntry>();
            lstModems = new List<MODEM>();
            List<ClientResponseDetail> lstClientResponseDetails = new List<ClientResponseDetail>();
            int iret = new SMSProcess().Modem(DatabaseConstant.Action.LOAD_DATA, ref lstModems, ref lstClientResponseDetails);
            
            if (iret > 0)
            {
                foreach (MODEM modem in lstModems)
                {
                    auto = new AutoCompleteEntry(modem.PORTNAME, modem.PORT.ToString(), modem.PORT.ToString(), modem.PORTNAME);
                    lstSourcePortName.Add(auto);
                }
            }
            auto = new AutoCompleteEntry(300.ToString(), 300.ToString(), 300.ToString(), 300.ToString());
            lstSourceBaurate.Add(auto);
            auto = new AutoCompleteEntry(1200.ToString(), 1200.ToString(), 1200.ToString(), 1200.ToString());
            lstSourceBaurate.Add(auto);
            auto = new AutoCompleteEntry(2400.ToString(), 2400.ToString(), 2400.ToString(), 2400.ToString());
            lstSourceBaurate.Add(auto);
            auto = new AutoCompleteEntry(4800.ToString(), 4800.ToString(), 4800.ToString(), 4800.ToString());
            lstSourceBaurate.Add(auto);
            auto = new AutoCompleteEntry(9600.ToString(), 9600.ToString(), 9600.ToString(), 9600.ToString());
            lstSourceBaurate.Add(auto);
            auto = new AutoCompleteEntry(14400.ToString(), 14400.ToString(), 14400.ToString(), 14400.ToString());
            lstSourceBaurate.Add(auto);
            auto = new AutoCompleteEntry(19200.ToString(), 19200.ToString(), 19200.ToString(), 19200.ToString());
            lstSourceBaurate.Add(auto);
            auto = new AutoCompleteEntry(28800.ToString(), 28800.ToString(), 28800.ToString(), 28800.ToString());
            lstSourceBaurate.Add(auto);
            auto = new AutoCompleteEntry(38400.ToString(), 38400.ToString(), 38400.ToString(), 38400.ToString());
            lstSourceBaurate.Add(auto);
            auto = new AutoCompleteEntry(57600.ToString(), 57600.ToString(), 57600.ToString(), 57600.ToString());
            lstSourceBaurate.Add(auto);
            auto = new AutoCompleteEntry(115200.ToString(), 115200.ToString(), 115200.ToString(), 115200.ToString());
            lstSourceBaurate.Add(auto);
            auto = new AutoCompleteEntry(230400.ToString(), 230400.ToString(), 230400.ToString(), 230400.ToString());
            lstSourceBaurate.Add(auto);
            for(int i=100;i<=1000;i=i+100)
            {
                auto = new AutoCompleteEntry(i.ToString(), i.ToString(), i.ToString(), i.ToString());
                lstSourceTimeOut.Add(auto);
            }
            new AutoComboBox().GenAutoComboBox(ref lstSourcePortName, ref cmbPort, null);
            new AutoComboBox().GenAutoComboBox(ref lstSourceTimeOut, ref cmbTimeOut, null, null, "300");
            new AutoComboBox().GenAutoComboBox(ref lstSourceBaurate, ref cmbBaurate, null, null, "19200");

        }
        #endregion

        #region Xu ly giao dien

        private void ClearForm()
        {
            numSecond.Value = 0;
            numMinute.Value = 0;
            numHour.Value = 0;
            numDay.Value = 0;
            tlbExecute.IsEnabled = false;
            lblLabelTrangThai.Content = "";
        }
        #endregion

        #region Xy ly nghiep vu

        private int GetDataForm()
        {
            int iret = 1;
            MODEM modem = new MODEM();
            lstModems = new List<MODEM>();
            try
            {
                AutoCompleteEntry auPortName = lstSourcePortName.ElementAt(cmbPort.SelectedIndex);
                AutoCompleteEntry auBaurate = lstSourceBaurate.ElementAt(cmbBaurate.SelectedIndex);
                AutoCompleteEntry auTimeOut = lstSourceTimeOut.ElementAt(cmbTimeOut.SelectedIndex);
                modem.BAURATE = Convert.ToInt32(auBaurate.KeywordStrings.FirstOrDefault());
                modem.TIMEOUT = Convert.ToInt32(auTimeOut.KeywordStrings.FirstOrDefault());
                modem.PORTNAME = auPortName.DisplayName;
                modem.PORT = Convert.ToInt32(auPortName.KeywordStrings.FirstOrDefault());
                modem.sendSecond = Convert.ToInt32(numSecond.Value);
                modem.sendMin = Convert.ToInt32(numMinute.Value);
                modem.sendhour = Convert.ToInt32(numHour.Value);
                modem.sendDay = Convert.ToInt32(numDay.Value);
                lstModems.Add(modem);
            }
            catch (Exception ex)
            {
                iret = 0;
                LLogging.WriteLog(ex.TargetSite.Name,LLogging.LogType.ERR, ex);
            }
            
            return iret;
        }

        private void Them()
        {
            try
            {
                if (cmbPort.IsEnabled)
                {
                    List<ClientResponseDetail> lstClientResponseDetails = new List<ClientResponseDetail>();
                    int iret = GetDataForm();
                    if (iret > 0)
                    {
                        iret = new SMSProcess().Modem(DatabaseConstant.Action.THEM, ref lstModems,
                            ref lstClientResponseDetails);
                        CommonFunction.ThongBaoKetQua(lstClientResponseDetails);
                        if (iret > 0)
                        {
                            cmbPort.IsEnabled = false;
                            tthai = BusinessConstant.CoKhong.CO.layGiaTri();
                            lblTrangThai.Content = LLanguage.SearchResourceByKey("U.DMUC_GTRI.CO_KHONG.CO");
                            tlbExecute.IsEnabled = true;
                        }
                    }
                    else
                    {
                        LMessage.ShowMessage("M.DungChung.LoiKhongXacDinh", LMessage.MessageBoxType.Error);
                    }
                }
                else
                {
                    ClearForm();
                }
                
            }
            catch (Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiKhongXacDinh", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void Process()
        {
            try
            {
                List<ClientResponseDetail> lstClientResponseDetails = new List<ClientResponseDetail>();
                int iret = GetDataForm();
                if (iret > 0)
                {
                    if (tthai.Equals(BusinessConstant.CoKhong.CO.layGiaTri()))
                        iret = new SMSProcess().Modem(DatabaseConstant.Action.HUY_KET_NOI, ref lstModems,
                            ref lstClientResponseDetails);
                    else
                        iret = new SMSProcess().Modem(DatabaseConstant.Action.KET_NOI, ref lstModems,
                                ref lstClientResponseDetails);
                    CommonFunction.ThongBaoKetQua(lstClientResponseDetails);
                    if (iret > 0)
                    {
                        if (tthai.Equals(BusinessConstant.CoKhong.CO.layGiaTri()))
                        {
                            tthai = BusinessConstant.CoKhong.KHONG.layGiaTri();
                            lblTrangThai.Content = LLanguage.SearchResourceByKey("U.DMUC_GTRI.CO_KHONG.KHONG");
                        }
                        else
                        {
                            tthai = BusinessConstant.CoKhong.CO.layGiaTri();
                            lblTrangThai.Content = LLanguage.SearchResourceByKey("U.DMUC_GTRI.CO_KHONG.CO");
                        }
                    }
                }
                else
                {
                    LMessage.ShowMessage("M.DungChung.LoiKhongXacDinh", LMessage.MessageBoxType.Error);
                }
            }
            catch (Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiKhongXacDinh", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }
        #endregion
    }
}
