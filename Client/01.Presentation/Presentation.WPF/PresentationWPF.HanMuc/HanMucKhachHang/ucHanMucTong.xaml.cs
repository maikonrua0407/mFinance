using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
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
using Presentation.Process;
using Presentation.Process.Common;
using PresentationWPF.CustomControl;
using Presentation.Process.PopupServiceRef;
using Telerik.Windows.Controls;
using Presentation.Process.HanMucServiceRef;

namespace PresentationWPF.HanMuc.HanMucKhachHang
{
    /// <summary>
    /// Interaction logic for ucHanMucTong.xaml
    /// </summary>
    public partial class ucHanMucTong : UserControl
    {
        #region Khai bao
        public event EventHandler OnSavingCompleted;

        public static RoutedCommand AddCommand = new RoutedCommand();
        public static RoutedCommand ModifyCommand = new RoutedCommand();
        public static RoutedCommand DeleteCommand = new RoutedCommand();
        public static RoutedCommand CloneCommand = new RoutedCommand();
        public static RoutedCommand HoldCommand = new RoutedCommand();
        public static RoutedCommand SubmitCommand = new RoutedCommand();
        public static RoutedCommand ApproveCommand = new RoutedCommand();
        public static RoutedCommand CancelCommand = new RoutedCommand();
        public static RoutedCommand RefuseCommand = new RoutedCommand();
        public static RoutedCommand PreviewCommand = new RoutedCommand();
        public static RoutedCommand HelpCommand = new RoutedCommand();
        public static RoutedCommand CloseCommand = new RoutedCommand();

        //Source cac combobox
        List<AutoCompleteEntry> lstLoaiKhachHang = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstLoaiTien = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstLoaiHanMuc = new List<AutoCompleteEntry>();

        //Khai bao popup
        List<DataRow> lstPopup = new List<DataRow>();
        public void LayDuLieuTuPopup(List<DataRow> lst)
        {
            lstPopup = lst;
        }

        //Object thông tin hạn mức tổng
        private HM_TONG _objHMTong = new HM_TONG();

        private List<HM_TONG_NHOMSP> _lstNhomSP = new List<HM_TONG_NHOMSP>();

        //Thông tin function và trạng thái nghiệp vụ
        private DatabaseConstant.Function _function = DatabaseConstant.Function.HM_TONG;

        private DatabaseConstant.Action _action = DatabaseConstant.Action.THEM;

        private string tthaiNvu = "";
        #endregion

        #region Khoi tao
        public ucHanMucTong()
        {
            KhoiTaoChung();
            BeforeAddNew();
            teldtNgayHetHieuLuc.Value = null;
            ucNhomSP.EditCellEnd += new EventHandler(ucNhomSP_EditCellEnd);
        }

        public ucHanMucTong(KIEM_SOAT obj)
        {
            KhoiTaoChung();
            tthaiNvu = obj.TTHAI_NVU;
            _action = obj.action;
            _objHMTong = new HM_TONG();
            _objHMTong.ID = obj.ID;
            SetFormData();
            BeforeModifyFromList(_action);
        }

        private void KhoiTaoChung()
        {
            InitializeComponent();
            DuyetQuyenTinhNang();
            LoadCombobox();
            BindHotkey();
            
        }

        private void DuyetQuyenTinhNang()
        {
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.HanMuc;component/HanMucKhachHang/ucHanMucTong.xaml", ref Toolbar, ref mnuMain);
            foreach (var item in mnuMain.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += btnShortcutKey_Click;
            }
        }
        #endregion

        /// <summary>
        /// Dang ky hot key, shortcut key
        /// </summary>
        #region Dang ky hot key, shortcut key
        private void BindHotkey()
        {
            try
            {
                foreach (var child in Toolbar.Children)
                {
                    if (child.GetType() == typeof(RibbonButton))
                    {
                        RibbonButton tlb = (RibbonButton)child;
                        KeyBinding key = new KeyBinding();
                        string strTinhNang = tlb.Name.Substring(3, tlb.Name.Length - 3);
                        if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
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
                        else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRINH_DUYET)))
                        {
                            KeyGesture keyg = new KeyGesture(Key.S, ModifierKeys.Control);
                            key = new KeyBinding(SubmitCommand, keyg);
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
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }
        private void AddCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute =tlbAdd.IsEnabled;
        }
        private void AddCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            BeforeAddNew();
        }
        private void ModifyCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbModify.IsEnabled;
        }
        private void ModifyCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            BeforeModifyFromDetail();
        }
        private void DeleteCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbDelete.IsEnabled;
        }
        private void DeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            BeforeDelete();
        }
        private void CloneCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void CloneCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //MessageBox.Show("Nhân bản dữ liệu");
        }
        private void HoldCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            //e.CanExecute = tlbHold.IsEnabled;
        }
        private void HoldCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OnHold();
        }
        private void SubmitCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbSubmit.IsEnabled;
        }
        private void SubmitCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OnSave();
        }
        private void CashStmtCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void ApproveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbApprove.IsEnabled;
        }
        private void ApproveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            BeforeApprove();
        }
        private void CancelCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbCancel.IsEnabled;
        }
        private void CancelCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            BeforeCancel();
        }
        private void RefuseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbRefuse.IsEnabled;
        }
        private void RefuseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            BeforeRefuse();
        }
        private void PreviewCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void PreviewCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //MessageBox.Show("Xem trước dữ liệu");
        }
        private void HelpCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void HelpCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OnHelp();
        }
        private void CloseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void CloseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OnClose();
        }

        /// <summary>
        /// Sự kiện hot key
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlbHotKey_Click(object sender, RoutedEventArgs e)
        {
            // Truongnx
            string strTinhNang = "";
            if (sender is RibbonButton)
            {
                RibbonButton tlb = (RibbonButton)sender;
                strTinhNang = tlb.Name.Substring(3, tlb.Name.Length - 3);
            }
            else if (sender is RibbonMenuItem)
            {
                RibbonMenuItem tlb = (RibbonMenuItem)sender;
                strTinhNang = tlb.Name.Substring(3, tlb.Name.Length - 3);
            }
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THEM)))
            {
                BeforeAddNew();
            }
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
                BeforeModifyFromDetail();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THEM)))
            {
                ResetForm();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
            {
                BeforeDelete();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.NHAN_BAN)))
            {

            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU_TAM)))
            {
                OnHold();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRINH_DUYET)))
            {
                OnSave();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.BANG_KE)))
            {

            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DUYET)))
            {
                BeforeApprove();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET)))
            {
                BeforeRefuse();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
            {
                BeforeCancel();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XEM_TRUOC)))
            {
                //OnPreview();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRO_GIUP)))
            {
                OnHelp();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DONG)))
            {
                OnClose();
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
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THEM)))
            {
                BeforeAddNew();
            }
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
                BeforeModifyFromDetail();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THEM)))
            {
                ResetForm();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
            {
                BeforeDelete();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.NHAN_BAN)))
            {

            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU_TAM)))
            {
                OnHold();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRINH_DUYET)))
            {
                OnSave();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.BANG_KE)))
            {

            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DUYET)))
            {
                BeforeApprove();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET)))
            {
                BeforeRefuse();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
            {
                BeforeCancel();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XEM_TRUOC)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRO_GIUP)))
            {
                OnHelp();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DONG)))
            {
                OnClose();
            }
            //else if (strTinhNang.Equals("Detail"))
            //{
            //    btnDetail_Click(null, null);
            //}
        }
        #endregion

        #region Xu ly Giao dien
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            raddgrDSachHanMuc.ItemsSource = _lstNhomSP;
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            Release();
            if (OnSavingCompleted != null)
            {
                OnSavingCompleted(this, EventArgs.Empty);
            }
            CustomControl.CommonFunction.CloseUserControl(this);
        }

        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            PresentationWPF.CustomControl.CommonFunction.CloseUserControl(e, this);
            PresentationWPF.CustomControl.CommonFunction.SelectNextControl(e);
        }

        private void LoadCombobox()
        {
            
            COMBOBOX_DTO combo = null;
            List<string> lstDieuKien = null;
            List<COMBOBOX_DTO> lstCombobox = new List<COMBOBOX_DTO>();
            AutoComboBox auCombo = new AutoComboBox();

            try
            {
                //Loại khách hàng
                lstDieuKien = new List<string>();
                lstDieuKien.Add("LOAI_KHANG_HAN_MUC");
                combo = new COMBOBOX_DTO();
                combo.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();
                combo.combobox = cmbLoaiKH;
                combo.lstSource = lstLoaiKhachHang;
                combo.lstDieuKien = lstDieuKien;
                combo.maChon = "KH_DON_LE";
                lstCombobox.Add(combo);

                //Loại tiền
                combo = new COMBOBOX_DTO();
                combo.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_LOAITIEN.getValue();
                combo.combobox = cmbLoaiTien;
                combo.lstSource = lstLoaiTien;
                combo.maChon = ClientInformation.MaDongNoiTe;
                lstCombobox.Add(combo);

                //Loại hạn mức
                lstDieuKien = new List<string>();
                lstDieuKien.Add("HM_LOAI_HAN_MUC");
                combo = new COMBOBOX_DTO();
                combo.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();
                combo.combobox = cmbLoaiHM;
                combo.lstSource = lstLoaiHanMuc;
                combo.lstDieuKien = lstDieuKien;
                combo.maChon = "QUAY_VONG";
                lstCombobox.Add(combo);

                //Gen combobox
                auCombo.GenAutoComboBoxTheoList(ref lstCombobox);
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        private void btnMaKH_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<string> lstDieuKien = new List<string>();
                string lstIDKH = "";
                if (!txtMaKH.Text.IsNullOrEmptyOrSpace())
                    lstIDKH = "(" + txtMaKH.Tag.ToString() + ")";
                else
                    lstIDKH = "(0)";
                lstDieuKien.Add(ClientInformation.MaDonViGiaoDich);
                lstDieuKien.Add(lstIDKH);
                lstDieuKien.Add("NULL");
                lstPopup = new List<DataRow>();
                PopupProcess popupProcess = new PopupProcess();
                popupProcess.getPopupInformation("POPUP_DS_KHACHHANG_TDVM", lstDieuKien);
                SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;
                ucPopup popup = new ucPopup(true, simplePopupResponse, false);
                popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
                Window win = new Window();
                win.Content = popup;
                win.Title = LLanguage.SearchResourceByKey(simplePopupResponse.PopupTitle);
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.ShowDialog();
                if (lstPopup.Count > 0)
                {
                    foreach (DataRow dr in lstPopup)
                    {
                        txtMaKH.Tag = Convert.ToInt32(dr["ID"]);
                        txtMaKH.Text = dr["MA_KHANG"].ToString();
                        lblTenKH.Content = dr["TEN_KHANG"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnThemHM_Click(object sender, RoutedEventArgs e)
        {
            HM_TONG_NHOMSP objNhomSP = new HM_TONG_NHOMSP();
            objNhomSP.MA_PHAN_HE = "TDTT";

            _lstNhomSP.Add(objNhomSP);
            raddgrDSachHanMuc.Rebind();
        }

        private void btnXoaHM_Click(object sender, RoutedEventArgs e)
        {
            HM_TONG_NHOMSP obj = null;
            if (ucNhomSP.cellEdit != null)
            {
                obj = ucNhomSP.cellEdit.ParentRow.Item as HM_TONG_NHOMSP;
            }
            else
            {
                obj = raddgrDSachHanMuc.CurrentItem as HM_TONG_NHOMSP;
                obj = raddgrDSachHanMuc.CurrentCell.ParentRow.Item as HM_TONG_NHOMSP;
            }

            if (obj != null)
            {
                _lstNhomSP.Remove(obj);
            }
        }

        private void ucNhomSP_EditCellEnd(object sender, EventArgs e)
        {
            HM_TONG_NHOMSP obj = ucNhomSP.cellEdit.ParentRow.Item as HM_TONG_NHOMSP;
            _lstNhomSP[_lstNhomSP.IndexOf(obj)].MA_PHAN_HE = ucNhomSP.GiaTri;
        }

        private void SetEnabledAllControls(bool enable)
        {
            raddgrDSachHanMuc.IsReadOnly = !enable;
            grbKhachHang.IsEnabled = enable;
            grbHanMuc.IsEnabled = enable;
            btnThemHM.IsEnabled = enable;
            btnXoaHM.IsEnabled = enable;
        }

        /// <summary>
        /// Hàm xử lý giải phóng lock dữ liệu cho form
        /// </summary>
        private void Release()
        {
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(_objHMTong.ID);

        }

        /// <summary>
        /// Trợ giúp
        /// </summary>
        private void OnHelp()
        {
            CustomControl.CommonFunction.ShowHelp(this);
        }

        /// <summary>
        /// Đóng form
        /// </summary>
        private void OnClose()
        {
            Release();
            CustomControl.CommonFunction.CloseUserControl(this);
        }
        #endregion

        #region Xử lý nghiệp vụ

        private void SetFormData()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                #region Get data từ server
                HanMucProcess process = new HanMucProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                ApplicationConstant.ResponseStatus ret = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                HM_HMUC_TONG obj = new HM_HMUC_TONG();
                obj.objTong = _objHMTong;
                ret = process.HanMucTong(_function,DatabaseConstant.Action.LOAD, ref obj, ref listClientResponseDetail);
                #endregion

                if (ret == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    _objHMTong = obj.objTong;
                    _lstNhomSP = obj.lstTongNhomSP.ToList();
                    tthaiNvu = obj.objTong.TTHAI_NVU;

                    #region Tab thông tin hạn mức tổng
                    txtDienGiai.Text = _objHMTong.DIEN_GIAI;
                    txtMaHM.Text = _objHMTong.MA_HMUC;
                    txtMaHM.Tag = _objHMTong.ID;
                    txtMaKH.Tag = _objHMTong.ID_DTUONG;
                    txtMaKH.Text = _objHMTong.MA_DTUONG;
                    numHMKhaDung.Value = Convert.ToDouble(_objHMTong.HMUC_KDUNG);
                    numHMPheDuyet.Value = Convert.ToDouble(_objHMTong.HMUC_PDUYET);
                    teldtNgayHieuLuc.Value = LDateTime.StringToDate(_objHMTong.NGAY_HLUC,"yyyyMMdd");
                    if (!_objHMTong.NGAY_HET_HLUC.IsNullOrEmptyOrSpace())
                    {
                        teldtNgayHetHieuLuc.Value = LDateTime.StringToDate(_objHMTong.NGAY_HET_HLUC, "yyyyMMdd");
                    }
                    else
                    {
                        teldtNgayHetHieuLuc.Value = null;
                    }
                    cmbLoaiKH.SelectedIndex = lstLoaiKhachHang.IndexOf(lstLoaiKhachHang.FirstOrDefault(e => e.KeywordStrings[0].Equals(_objHMTong.LOAI_DTUONG)));
                    cmbLoaiHM.SelectedIndex = lstLoaiHanMuc.IndexOf(lstLoaiHanMuc.FirstOrDefault(e=>e.KeywordStrings[0].Equals(_objHMTong.LOAI_HMUC)));
                    cmbLoaiTien.SelectedIndex = lstLoaiTien.IndexOf(lstLoaiTien.FirstOrDefault(e=>e.KeywordStrings[0].Equals(_objHMTong.MA_LOAI_TIEN)));
                    #endregion

                    #region Tab thông tin kiểm soát
                    txtTrangThai.Text = BusinessConstant.layNgonNguSuDung(_objHMTong.TTHAI_BGHI.ToString());
                    teldtNgayNhap.Value = LDateTime.StringToDate(_objHMTong.NGAY_NHAP, "yyyyMMdd");
                    txtNguoiLap.Text = _objHMTong.NGUOI_NHAP;
                    if (LDateTime.IsDate(_objHMTong.NGAY_CNHAT, "yyyyMMdd") == true)
                        teldtNgayCNhat.Value = LDateTime.StringToDate(_objHMTong.NGAY_CNHAT, "yyyyMMdd");
                    else
                        teldtNgayCNhat.Value = null;
                    txtNguoiCapNhat.Text = _objHMTong.NGUOI_CNHAT;
                    #endregion
                }
                else
                {
                    CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                CommonFunction.ThongBaoLoi(ex);
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private bool Validation()
        {
            #region Kiểm tra mã khách hàng
            if (txtMaKH.Text.IsNullOrEmptyOrSpace())
            {
                CommonFunction.ThongBaoTrong(lblMaKH.Content.ToString());
                txtMaKH.Focus();
                return false;
            }
            #endregion

            #region Kiểm tra hạn mức phê duyệt
            if (numHMPheDuyet.Value.IsNullOrEmpty())
            {
                CommonFunction.ThongBaoTrong(lblHMPheDuyet.Content.ToString());
                numHMPheDuyet.Focus();
                return false;
            }
            #endregion

            #region Kiểm tra ngày phê duyệt
            if (teldtNgayHieuLuc.Value.IsNullOrEmpty())
            {
                CommonFunction.ThongBaoTrong(lblNgayHieuLuc.Content.ToString());
                teldtNgayHieuLuc.Focus();
                return false;
            }
            #endregion     

            #region Kiểm tra ngày hết hiệu lực
            if (!teldtNgayHetHieuLuc.Value.IsNullOrEmpty())
            {
                if (teldtNgayHetHieuLuc.Value <= teldtNgayHieuLuc.Value)
                {
                    CommonFunction.ThongBaoLoi("Ngày hết hiệu lực phải lớn hơn ngày hiệu lực");
                    teldtNgayHetHieuLuc.Focus();
                    return false;
                }
            }
            #endregion

            #region Kiểm tra grid nhóm sản phẩm
            if (_lstNhomSP == null || _lstNhomSP.Count == 0)
            {
                CommonFunction.ThongBaoChuaNhap("Chưa nhập thông tin hạn mức cho nhóm sản phẩm");
                return false;
            }
            else
            {
                foreach (HM_TONG_NHOMSP obj in _lstNhomSP)
                {
                    if (obj.HMUC_PDUYET == 0)
                    {
                        CommonFunction.ThongBaoTrong("Chưa nhập hạn mức phê duyệt cho nhóm sản phẩm");
                        return false;
                    }
                }

                decimal sumHmNhomSP = _lstNhomSP.Sum(e => e.HMUC_PDUYET);
                if (Convert.ToDecimal(numHMPheDuyet.Value) < sumHmNhomSP)
                {
                    CommonFunction.ThongBaoLoi("Tổng hạn mức nhóm sản phẩm phải nhỏ hơn hạn mức phê duyệt");
                    return false;
                }
            }
            
            #endregion

            return true;
        }

        private void GetFormData(ref HM_HMUC_TONG objTong)
        {
            try
            {
                UtilitiesProcess processUtilities = new UtilitiesProcess();
                AutoCompleteEntry auLoaiHM = (AutoCompleteEntry)cmbLoaiHM.SelectedItem;
                AutoCompleteEntry auLoaiKH = (AutoCompleteEntry)cmbLoaiKH.SelectedItem;
                AutoCompleteEntry auLoaiTien = (AutoCompleteEntry)cmbLoaiTien.SelectedItem;

                _objHMTong.ID_DTUONG = Convert.ToInt32(txtMaKH.Tag);
                _objHMTong.LOAI_DTUONG = auLoaiKH.KeywordStrings[0];
                _objHMTong.MA_DTUONG = txtMaKH.Text.Trim();
                _objHMTong.MA_HMUC = txtMaHM.Text;
                _objHMTong.LOAI_HMUC = auLoaiHM.KeywordStrings[0];
                _objHMTong.HMUC_PDUYET = Convert.ToDecimal(numHMPheDuyet.Value);
                _objHMTong.HMUC_KDUNG = Convert.ToDecimal(numHMKhaDung.Value);
                _objHMTong.MA_LOAI_TIEN = auLoaiTien.KeywordStrings[0];
                _objHMTong.NGAY_HLUC = LDateTime.DateToString(teldtNgayHieuLuc.Value.Value, "yyyyMMdd");
                if (!teldtNgayHetHieuLuc.Value.IsNullOrEmpty())
                {
                    _objHMTong.NGAY_HET_HLUC = LDateTime.DateToString(teldtNgayHetHieuLuc.Value.Value, "yyyyMMdd");
                }
                _objHMTong.DIEN_GIAI = txtDienGiai.Text.Trim();
                _objHMTong.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                _objHMTong.TTHAI_NVU = tthaiNvu;
                _objHMTong.MA_DVI_QLY = ClientInformation.MaDonVi;

                if (_action != DatabaseConstant.Action.THEM)
                {
                    _objHMTong.NGAY_CNHAT = ClientInformation.NgayLamViecHienTai;
                    _objHMTong.NGUOI_CNHAT = ClientInformation.TenDangNhap;
                }
                else
                {
                    _objHMTong.NGAY_NHAP = ClientInformation.NgayLamViecHienTai;
                    _objHMTong.NGUOI_NHAP = ClientInformation.TenDangNhap;
                    _objHMTong.MA_DVI_TAO = ClientInformation.MaDonViGiaoDich;
                }

                foreach (HM_TONG_NHOMSP obj in _lstNhomSP)
                {
                    obj.ID_DTUONG = _objHMTong.ID_DTUONG;
                    obj.MA_DTUONG = _objHMTong.MA_DTUONG;
                    obj.LOAI_DTUONG = _objHMTong.LOAI_DTUONG;
                    obj.TTHAI_BGHI = _objHMTong.TTHAI_BGHI;
                    obj.TTHAI_NVU = _objHMTong.TTHAI_NVU;
                    obj.MA_DVI_QLY = _objHMTong.MA_DVI_QLY;
                    obj.MA_DVI_TAO = _objHMTong.MA_DVI_TAO;
                    obj.NGAY_NHAP = _objHMTong.NGAY_NHAP;
                    obj.NGUOI_NHAP = _objHMTong.NGUOI_NHAP;
                    obj.NGAY_CNHAT = _objHMTong.NGAY_CNHAT;
                    obj.NGUOI_CNHAT = _objHMTong.NGUOI_CNHAT;
                }

                objTong.objTong = _objHMTong;
                objTong.lstTongNhomSP = _lstNhomSP.ToArray();
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        private void ResetForm()
        {
            _objHMTong = new HM_TONG();
            _lstNhomSP = new List<HM_TONG_NHOMSP>();
            tthaiNvu = "";
            txtMaKH.Text = "";
            lblTenKH.Content = "";
            txtMaKH.Tag = "";
            txtMaHM.Text = "";
            cmbLoaiKH.SelectedIndex = 0;
            cmbLoaiTien.SelectedIndex = lstLoaiTien.IndexOf(lstLoaiTien.FirstOrDefault(e => e.KeywordStrings[0].Equals(ClientInformation.MaDongNoiTe)));
            cmbLoaiHM.SelectedIndex = 0;
            numHMKhaDung.Value = 0;
            numHMPheDuyet.Value = 0;
            teldtNgayHieuLuc.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai,"yyyyMMdd");
            teldtNgayHetHieuLuc.Value = null;
            txtDienGiai.Text = "";
            raddgrDSachHanMuc.Rebind();
        }

        public void OnHold()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                if (!Validation()) return;
                tthaiNvu = CommonFunction.LayTrangThaiBanGhi(DatabaseConstant.Action.LUU_TAM, BusinessConstant.layTrangThaiNghiepVu(tthaiNvu));
                HM_HMUC_TONG obj = new HM_HMUC_TONG();
                GetFormData(ref obj);
                if (_action == DatabaseConstant.Action.THEM)
                {
                    OnAddNew(ref obj);
                }
                else if (_action == DatabaseConstant.Action.SUA)
                {
                    OnModify(ref obj);
                }
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        public void OnSave()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                if (!Validation()) return;
                tthaiNvu = CommonFunction.LayTrangThaiBanGhi(DatabaseConstant.Action.TRINH_DUYET, BusinessConstant.layTrangThaiNghiepVu(tthaiNvu));
                HM_HMUC_TONG obj = new HM_HMUC_TONG();
                GetFormData(ref obj);
                if (_action == DatabaseConstant.Action.THEM)
                {
                    OnAddNew(ref obj);
                }
                else if (_action == DatabaseConstant.Action.SUA)
                {
                    OnModify(ref obj);
                }
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        #region Action View
        //Sau khi luu chuyen sang trang thai xem thi goi toi ham nay
        public void BeforeViewFromDetail()
        {
            SetEnabledAllControls(false);
            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, _function);
        }

        public void BeforeViewFromList()
        {
            try
            {
                SetFormData();
                BeforeViewFromDetail();
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }
        #endregion

        #region Action AddNew
        public void BeforeAddNew()
        {
            tthaiNvu = CommonFunction.LayTrangThaiBanGhi(DatabaseConstant.Action.TRINH_DUYET, BusinessConstant.layTrangThaiNghiepVu(tthaiNvu));
            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.THEM, tthaiNvu, mnuMain, _function);
            ResetForm();
            teldtNgayCNhat.Value = null;
        }

        public void OnAddNew(ref HM_HMUC_TONG obj)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                HanMucProcess process = new HanMucProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                ApplicationConstant.ResponseStatus ret = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;

                ret = process.HanMucTong(_function,DatabaseConstant.Action.THEM, ref obj, ref listClientResponseDetail);
                AfterAddNew(ret, obj, listClientResponseDetail);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        public void AfterAddNew(ApplicationConstant.ResponseStatus ret, HM_HMUC_TONG obj, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (ret == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    LMessage.ShowMessage("M.DungChung.ThemThanhCong", LMessage.MessageBoxType.Information);
                    SetEnabledAllControls(false);
                    _objHMTong = obj.objTong;
                    _lstNhomSP = obj.lstTongNhomSP.ToList();
                    
                    tthaiNvu = obj.objTong.TTHAI_NVU;
                    CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, _function);
                    tthaiNvu = obj.objTong.TTHAI_NVU;
                    txtMaHM.Tag = obj.objTong.ID.ToString();
                    txtMaHM.Text = obj.objTong.MA_HMUC;
                }
                else
                {
                    CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }
        #endregion

        #region Action Modify
        public void BeforeModifyFromDetail()
        {
            try
            {
                // Yêu cầu lock bản ghi cần sửa
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(_objHMTong.ID);

                bool ret = process.LockData(DatabaseConstant.Module.TDTT,
                    DatabaseConstant.Function.HM_TONG,
                    DatabaseConstant.Table.HM_TONG,
                    DatabaseConstant.Action.SUA,
                    listLockId);

                if (ret)
                {
                    _action = DatabaseConstant.Action.SUA;
                    SetEnabledAllControls(true);
                    CommonFunction.RefreshButton(Toolbar, _action, tthaiNvu, mnuMain, _function);
                }
                else
                {
                    LMessage.ShowMessage("M.ResponseMessage.Common.LockDataInvalid", LMessage.MessageBoxType.Information);
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        public void BeforeModifyFromList(DatabaseConstant.Action act)
        {
            try
            {
                if (_action != DatabaseConstant.Action.THEM && _action != DatabaseConstant.Action.SUA)
                {
                    SetEnabledAllControls(false);
                }
                CommonFunction.RefreshButton(Toolbar, _action, tthaiNvu, mnuMain, _function);
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }                                         
        }

        public void OnModify(ref HM_HMUC_TONG obj)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                HanMucProcess process = new HanMucProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                ApplicationConstant.ResponseStatus ret = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;

                ret = process.HanMucTong(_function,DatabaseConstant.Action.SUA, ref obj, ref listClientResponseDetail);
                AfterAddNew(ret, obj, listClientResponseDetail);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        public void AfterModify(ApplicationConstant.ResponseStatus ret, HM_HMUC_TONG obj, List<ClientResponseDetail> listClientResponseDetail)
        {
            if (ret == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                _objHMTong = obj.objTong;
                _lstNhomSP = obj.lstTongNhomSP.ToList();
                LMessage.ShowMessage("M.DungChung.SuaThanhCong", LMessage.MessageBoxType.Information);
            }
            else
            {
                CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                //LMessage.ShowMessage("M.DungChung.XoaKhongThanhCong", LMessage.MessageBoxType.Warning);
            }
        }
        #endregion

        #region Action Delete

        public void BeforeDelete()
        {
            try
            {
            // Cảnh báo người dùng
                MessageBoxResult ret = LMessage.ShowMessage("M.DungChung.HoiXoa", LMessage.MessageBoxType.Question);

                if (ret == MessageBoxResult.Yes)
                {
                    // Lock dữ liệu nếu người dùng chấp nhận cảnh báo
                    UtilitiesProcess process = new UtilitiesProcess();
                    List<int> listLockId = new List<int>();
                    listLockId.Add(_objHMTong.ID);
                    List<int> listLockedId = new List<int>();

                    bool retLockData = process.LockData(DatabaseConstant.Module.TDTT,
                        DatabaseConstant.Function.HM_TONG,
                        DatabaseConstant.Table.HM_TONG,
                        DatabaseConstant.Action.XOA,
                        listLockId);

                    if (retLockData)
                    {
                        _action = DatabaseConstant.Action.XOA;
                        OnDelete();
                    }
                    else
                    {
                        LMessage.ShowMessage("M.ResponseMessage.Common.LockDataInvalid", LMessage.MessageBoxType.Information);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                // Yêu cầu unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(_objHMTong.ID);
                List<int> listUnlockId = new List<int>();

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.TDTT,
                    DatabaseConstant.Function.HM_TONG,
                    DatabaseConstant.Table.HM_TONG,
                    DatabaseConstant.Action.XOA,
                    listLockId);

                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        public void OnDelete()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
            HanMucProcess process = new HanMucProcess();
            try
            {
                ApplicationConstant.ResponseStatus ret = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                HM_HMUC_TONG obj = new HM_HMUC_TONG();
                obj.objTong = _objHMTong;
                obj.lstTongNhomSP = _lstNhomSP.ToArray();
                ret = process.HanMucTong(_function,_action, ref obj, ref listClientResponseDetail);
                AfterDelete(ret, listClientResponseDetail);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
                listClientResponseDetail = null;
                process = null;
            }
        }

        public void AfterDelete(ApplicationConstant.ResponseStatus ret, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (ret == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    LMessage.ShowMessage("M.DungChung.XoaThanhCong", LMessage.MessageBoxType.Information);
                }
                else
                {
                    CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                    //LMessage.ShowMessage("M.DungChung.XoaKhongThanhCong", LMessage.MessageBoxType.Warning);
                }

                // Yêu cầu unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(_objHMTong.ID);
                List<int> listUnlockId = new List<int>();

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.TDTT,
                    DatabaseConstant.Function.HM_TONG,
                    DatabaseConstant.Table.HM_TONG,
                    DatabaseConstant.Action.XOA,
                    listLockId);

                // Đóng cửa sổ chi tiết sau khi xóa
                OnClose();
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }
        #endregion

        #region Action Approve
        private void BeforeApprove()
        {
            // Cảnh báo người dùng
            MessageBoxResult ret = LMessage.ShowMessage("M.DungChung.HoiDuyet", LMessage.MessageBoxType.Question);

            if (ret == MessageBoxResult.Yes)
            {
                // Lock dữ liệu nếu người dùng chấp nhận cảnh báo
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(_objHMTong.ID);
                List<int> listLockedId = new List<int>();

                bool retLockData = process.LockData(DatabaseConstant.Module.TDTT,
                    _function,
                    DatabaseConstant.Table.HM_TONG,
                    DatabaseConstant.Action.DUYET,
                    listLockId);

                // Nếu lock thành công >> cho phép sửa
                if (retLockData)
                {
                    // Gọi tới hàm duyệt dữ liệu
                    tthaiNvu = CommonFunction.LayTrangThaiBanGhi(DatabaseConstant.Action.DUYET, BusinessConstant.layTrangThaiNghiepVu(tthaiNvu));
                    OnApprove();
                    return;
                }
                // Nếu lock không thành công >> cảnh báo
                else
                {
                    LMessage.ShowMessage("M.ResponseMessage.Common.LockDataInvalid", LMessage.MessageBoxType.Information);
                    return;
                }
            }
            else
            {
                // Unlock dữ liệu
                // (Hiện tại không phải Unlock dữ liệu, vì chỉ được xóa khi chọn sửa - sẽ Unlock sau)
                return;
            }
        }

        private void OnApprove()
        {
            //string trangThai = CommonFunction.LayTrangThaiBanGhi(DatabaseConstant.Action.DUYET, BusinessConstant.layTrangThaiNghiepVu(tthaiNvu));
            HanMucProcess process = new HanMucProcess();
            try
            {
                HM_HMUC_TONG obj = new HM_HMUC_TONG();
                ApplicationConstant.ResponseStatus ret = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                List<ClientResponseDetail> listResponseDetail = new List<ClientResponseDetail>();
                // Dữ liệu truyền vào và dữ liệu trả về

                Mouse.OverrideCursor = Cursors.Wait;
                //Getformdata(tthainvu)
                GetFormData(ref obj);
                ret = process.HanMucTong(_function,DatabaseConstant.Action.DUYET, ref obj, ref listResponseDetail);

                Mouse.OverrideCursor = Cursors.Arrow;
                AfterApprove(ret, obj, listResponseDetail);
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            finally
            {
                process = null;
            }
        }

        private void AfterApprove(ApplicationConstant.ResponseStatus ret, HM_HMUC_TONG obj, List<ClientResponseDetail> listClientResponseDetail)
        {
            //CommonFunction.ThongBaoKetQua(listClientResponseDetail);
            if (ret == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                LMessage.ShowMessage("M.DungChung.DuyetThanhCong", LMessage.MessageBoxType.Information);
                tthaiNvu = obj.objTong.TTHAI_NVU;
                _objHMTong = obj.objTong;
                _lstNhomSP = obj.lstTongNhomSP.ToList();
                SetEnabledAllControls(false);
                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, _function);
                tlbImport.IsEnabled = true;
                txtTrangThai.Text = BusinessConstant.layNgonNguNghiepVu(tthaiNvu);
                txtNguoiCapNhat.Text = obj.objTong.NGUOI_CNHAT;
                teldtNgayCNhat.Value = LDateTime.StringToDate(obj.objTong.NGAY_CNHAT, "yyyyMMdd");
            }
            else
            {
                CommonFunction.ThongBaoKetQua(listClientResponseDetail);
            }

            // Yêu cầu Unlock bản ghi cần sửa
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(obj.objTong.ID);

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.TDTT,
                _function,
                DatabaseConstant.Table.HM_TONG,
                DatabaseConstant.Action.SUA,
                listLockId);
        }
        #endregion

        #region Action Cancel
        private void BeforeCancel()
        {
            // Cảnh báo người dùng
            MessageBoxResult ret = LMessage.ShowMessage("M.DungChung.HoiThoaiDuyet", LMessage.MessageBoxType.Question);

            if (ret == MessageBoxResult.Yes)
            {
                // Lock dữ liệu nếu người dùng chấp nhận cảnh báo
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(_objHMTong.ID);
                List<int> listLockedId = new List<int>();

                bool retLockData = process.LockData(DatabaseConstant.Module.TDTT,
                    _function,
                    DatabaseConstant.Table.HM_TONG,
                    DatabaseConstant.Action.THOAI_DUYET,
                    listLockId);

                // Nếu lock thành công >> cho phép sửa
                if (retLockData)
                {
                    // Gọi tới hàm thoái duyệt dữ liệu
                    tthaiNvu = CommonFunction.LayTrangThaiBanGhi(DatabaseConstant.Action.THOAI_DUYET, BusinessConstant.layTrangThaiNghiepVu(tthaiNvu));
                    OnCancel();
                    return;
                }
                // Nếu lock không thành công >> cảnh báo
                else
                {
                    LMessage.ShowMessage("M.ResponseMessage.Common.LockDataInvalid", LMessage.MessageBoxType.Information);
                    return;
                }
            }
            else
            {
                // Unlock dữ liệu
                // (Hiện tại không phải Unlock dữ liệu, vì chỉ được xóa khi chọn sửa - sẽ Unlock sau)
                return;
            }
        }

        private void OnCancel()
        {
            //string trangThai = CommonFunction.LayTrangThaiBanGhi(DatabaseConstant.Action.THOAI_DUYET, BusinessConstant.layTrangThaiNghiepVu(tthaiNvu));
            HanMucProcess process = new HanMucProcess();
            try
            {
                HM_HMUC_TONG obj = new HM_HMUC_TONG();
                ApplicationConstant.ResponseStatus ret = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                List<ClientResponseDetail> listResponseDetail = new List<ClientResponseDetail>();
                // Dữ liệu truyền vào và dữ liệu trả về

                Mouse.OverrideCursor = Cursors.Wait;
                GetFormData(ref obj);
                ret = process.HanMucTong(_function,DatabaseConstant.Action.THOAI_DUYET, ref obj, ref listResponseDetail);

                Mouse.OverrideCursor = Cursors.Arrow;
                AfterCancel(ret, obj, listResponseDetail);

            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            finally
            {
                process = null;
            }
        }

        private void AfterCancel(ApplicationConstant.ResponseStatus ret, HM_HMUC_TONG obj, List<ClientResponseDetail> listClientResponseDetail)
        {
            //CommonFunction.ThongBaoKetQua(listClientResponseDetail);
            if (ret == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                LMessage.ShowMessage("M.DungChung.ThoaiDuyetThanhCong", LMessage.MessageBoxType.Information);
                tthaiNvu = obj.objTong.TTHAI_NVU;
                _objHMTong = obj.objTong;
                _lstNhomSP = obj.lstTongNhomSP.ToList();
                SetEnabledAllControls(false);
                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, _function);
                
                txtTrangThai.Text = BusinessConstant.layNgonNguNghiepVu(tthaiNvu);
                //txtTrangThai.Text = lblTrangThai.Content.ToString();
                txtNguoiCapNhat.Text = obj.objTong.NGUOI_CNHAT;
                teldtNgayCNhat.Value = LDateTime.StringToDate(obj.objTong.NGAY_CNHAT, "yyyyMMdd");
            }
            else
            {
                CommonFunction.ThongBaoKetQua(listClientResponseDetail);
            }

            // Yêu cầu Unlock bản ghi cần sửa
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(obj.objTong.ID);

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.TDTT,
                _function,
                DatabaseConstant.Table.HM_TONG,
                DatabaseConstant.Action.SUA,
                listLockId);
        }
        #endregion

        #region Action Refuse
        private void BeforeRefuse()
        {
            // Cảnh báo người dùng
            MessageBoxResult ret = LMessage.ShowMessage("M.DungChung.HoiTuChoiDuyet", LMessage.MessageBoxType.Question);

            if (ret == MessageBoxResult.Yes)
            {
                // Lock dữ liệu nếu người dùng chấp nhận cảnh báo
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(_objHMTong.ID);
                List<int> listLockedId = new List<int>();

                bool retLockData = process.LockData(DatabaseConstant.Module.TDTT,
                    _function,
                    DatabaseConstant.Table.HM_TONG,
                    DatabaseConstant.Action.TU_CHOI_DUYET,
                    listLockId);

                // Nếu lock thành công >> cho phép sửa
                if (retLockData)
                {
                    // Gọi tới hàm xóa dữ liệu
                    tthaiNvu = CommonFunction.LayTrangThaiBanGhi(DatabaseConstant.Action.TU_CHOI_DUYET, BusinessConstant.layTrangThaiNghiepVu(tthaiNvu));
                    OnRefuse();
                    return;
                }
                // Nếu lock không thành công >> cảnh báo
                else
                {
                    LMessage.ShowMessage("M.ResponseMessage.Common.LockDataInvalid", LMessage.MessageBoxType.Information);
                    return;
                }
            }
            else
            {
                // Unlock dữ liệu
                // (Hiện tại không phải Unlock dữ liệu, vì chỉ được xóa khi chọn sửa - sẽ Unlock sau)
                return;
            }
        }

        private void OnRefuse()
        {
            //string trangThai = CommonFunction.LayTrangThaiBanGhi(DatabaseConstant.Action.TU_CHOI_DUYET, BusinessConstant.layTrangThaiNghiepVu(tthaiNvu));
            HanMucProcess process = new HanMucProcess();
            try
            {
                HM_HMUC_TONG obj = new HM_HMUC_TONG();
                ApplicationConstant.ResponseStatus ret = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                List<ClientResponseDetail> listResponseDetail = new List<ClientResponseDetail>();
                // Dữ liệu truyền vào và dữ liệu trả về

                Mouse.OverrideCursor = Cursors.Wait;
                GetFormData(ref obj);
                ret = process.HanMucTong(_function,DatabaseConstant.Action.TU_CHOI_DUYET, ref obj, ref listResponseDetail);

                Mouse.OverrideCursor = Cursors.Arrow;
                afterRefuse(ret, obj, listResponseDetail);

            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            finally
            {
                process = null;
            }
        }

        private void afterRefuse(ApplicationConstant.ResponseStatus ret, HM_HMUC_TONG obj, List<ClientResponseDetail> listClientResponseDetail)
        {
            //CommonFunction.ThongBaoKetQua(listClientResponseDetail);
            if (ret == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                LMessage.ShowMessage("M.DungChung.TuChoiDuyetThanhCong", LMessage.MessageBoxType.Information);
                tthaiNvu = obj.objTong.TTHAI_NVU;
                _objHMTong = obj.objTong;
                _lstNhomSP = obj.lstTongNhomSP.ToList();
                SetEnabledAllControls(false);
                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, _function);
                tlbImport.IsEnabled = true;
                txtTrangThai.Text = BusinessConstant.layNgonNguNghiepVu(tthaiNvu);
                //txtTrangThai.Text = lblTrangThai.Content.ToString();
                txtNguoiCapNhat.Text = obj.objTong.NGUOI_CNHAT;
                teldtNgayCNhat.Value = LDateTime.StringToDate(obj.objTong.NGAY_CNHAT, "yyyyMMdd");
            }
            else
            {
                CommonFunction.ThongBaoKetQua(listClientResponseDetail);
            }

            // Yêu cầu Unlock bản ghi cần sửa
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(obj.objTong.ID);

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.TDTT,
                _function,
                DatabaseConstant.Table.HM_TONG,
                DatabaseConstant.Action.SUA,
                listLockId);
        }
        #endregion

        #endregion
    }
}