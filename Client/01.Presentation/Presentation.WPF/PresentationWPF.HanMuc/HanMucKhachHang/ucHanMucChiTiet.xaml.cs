using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Reflection;
using System.Collections;
using Microsoft.Windows.Controls.Ribbon;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using Utilities.Common;
using Presentation.Process;
using Presentation.Process.HanMucServiceRef;
using Presentation.Process.QuanTriHeThongServiceRef;
using Presentation.Process.Common;
using Presentation.Process.PopupServiceRef;
using PresentationWPF.CustomControl;

namespace PresentationWPF.HanMuc.HanMucKhachHang
{
    /// <summary>
    /// Interaction logic for ucHanMucChiTiet.xaml
    /// </summary>
    public partial class ucHanMucChiTiet : UserControl
    {
        #region Khai bao
        private DatabaseConstant.Action action = DatabaseConstant.Action.THEM;
        public DatabaseConstant.Action Action
        {
            get { return action; }
            set { action = value; }
        }

        private DatabaseConstant.Function function = DatabaseConstant.Function.HM_CTIET_CT;

        public event EventHandler OnSavingCompleted;


        private int id;
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        private int idKhachHang = 0;
        private int idSanPham = 0;
        private int idHanMucTong = 0;

        private HM_HAN_MUC_KHACH_HANG_CTIET obj;
        public HM_HAN_MUC_KHACH_HANG_CTIET Obj
        {
            get { return obj; }
            set { obj = value; }
        }

        List<HM_CTIET_LOAITS> lstHMLoaiTS = new List<HM_CTIET_LOAITS>();

        private DataTable dtTSDB = null;

        private string sTrangThaiNVu = "";

        List<AutoCompleteEntry> lstSourceLoaiKH = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceLoaiTien = new List<AutoCompleteEntry>();

        List<DataRow> lstPopup = new List<DataRow>();
        public void LayDuLieuTuPopup(List<DataRow> lst)
        {
            lstPopup = lst;
        }

        public static RoutedCommand HoldCommand = new RoutedCommand();
        public static RoutedCommand SaveCommand = new RoutedCommand();
        public static RoutedCommand AddCommand = new RoutedCommand();
        public static RoutedCommand ModifyCommand = new RoutedCommand();
        public static RoutedCommand DeleteCommand = new RoutedCommand();
        public static RoutedCommand ApproveCommand = new RoutedCommand();
        public static RoutedCommand RefuseCommand = new RoutedCommand();
        public static RoutedCommand CancelCommand = new RoutedCommand();
        public static RoutedCommand HelpCommand = new RoutedCommand();
        public static RoutedCommand CloseCommand = new RoutedCommand();
        #endregion

        #region Khoi tao
        public ucHanMucChiTiet()
        {
            InitializeComponent();

            DuyetQuyenTinhNang();

            BindShortkey();            

            LoadCombobox();

            InitEventHandler();

            btnMaKH.Focus();
        }

        private void DuyetQuyenTinhNang()
        {
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.HanMuc;component/HanMucKhachHang/ucHanMucChiTiet.xaml", ref Toolbar, ref mnuMain);
            foreach (var item in mnuMain.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += btnShortcutKey_Click;
            }
        }

        private void InitEventHandler()
        {            
            btnMaKH.Click += new RoutedEventHandler(btnMaKH_Click);
            btnMaHMTong.Click += new RoutedEventHandler(btnMaHMTong_Click);
            btnMaSanPham.Click += new RoutedEventHandler(btnMaSanPham_Click);
            btnThem.Click += new RoutedEventHandler(btnThem_Click);
            btnXoa.Click += new RoutedEventHandler(btnXoa_Click);
            txtMaKH.KeyDown += new KeyEventHandler(txtMaKH_KeyDown);
            txtMaHMTong.KeyDown += new KeyEventHandler(txtMaHMTong_KeyDown);
            txtMaSanPham.KeyDown += new KeyEventHandler(txtMaSanPham_KeyDown);
            numHMPheDuyet.LostFocus += new RoutedEventHandler(numHMPheDuyet_LostFocus);
            numHMKhongCoTaiSan.LostFocus += new RoutedEventHandler(numHMKhongCoTaiSan_LostFocus);
            ucNhomSP.EditCellEnd += new EventHandler(ucNhomSP_EditCellEnd);
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
                combo.lstSource = lstSourceLoaiKH;
                combo.lstDieuKien = lstDieuKien;
                combo.maChon = "KH_DON_LE";
                lstCombobox.Add(combo);

                //Loại tiền
                combo = new COMBOBOX_DTO();
                combo.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_LOAITIEN.getValue();
                combo.combobox = cmbLoaiTien;
                combo.lstSource = lstSourceLoaiTien;
                combo.maChon = ClientInformation.MaDongNoiTe;
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
                        key = new KeyBinding(HoldCommand, keyg);
                        key.Gesture = keyg;
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRINH_DUYET)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.S, ModifierKeys.Control);
                        key = new KeyBinding(SaveCommand, keyg);
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
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TINH_TOAN)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.T, ModifierKeys.Control | ModifierKeys.Shift);
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

        private void HoldCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void HoldCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //OnHold();
        }

        private void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OnSave();
        }

        private void ModifyCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void ModifyCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            BeforeModifyFromDetail();
        }

        private void DeleteCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void DeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            BeforeDelete();
        }

        private void ApproveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void ApproveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //BeforeApprove();
        }

        private void RefuseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void RefuseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //BeforeRefuse();
        }

        private void CancelCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void CancelCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //BeforeCancel();
        }

        private void CaculateCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void CaculateCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //TinhDuChi();
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

        private void tlbHotKey_Click(object sender, RoutedEventArgs e)
        {
            RibbonButton tlb = (RibbonButton)sender;
            string strTinhNang = tlb.Name.Substring(3, tlb.Name.Length - 3);
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU_TAM)))
            {
                //OnHold();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRINH_DUYET)))
            {
                OnSave();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
                BeforeModifyFromDetail();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
            {
                BeforeDelete();
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
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRO_GIUP)))
            {
                OnHelp();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DONG)))
            {
                OnClose();
            }
        }

        private void btnShortcutKey_Click(object sender, RoutedEventArgs e)
        {
            string strTinhNang = ((MenuItem)sender).Name.Substring(3, ((MenuItem)sender).Name.Length - 3);
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU_TAM)))
            {
                //OnHold();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRINH_DUYET)))
            {
                OnSave();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
                BeforeModifyFromDetail();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
            {
                BeforeDelete();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DUYET)))
            {
                //BeforeApprove();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET)))
            {
                //BeforeRefuse();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
            {
                //BeforeCancel();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TINH_TOAN)))
            {
                //TinhDuChi();
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

        #endregion

        #region Xu ly Giao dien
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //Hiển thị Form khi thêm mới dữ liệu
            if (action == DatabaseConstant.Action.THEM)
            {
                BeforeAddNew();
            }

            //Hiển thị Form khi sửa dữ liệu
            else if (action == DatabaseConstant.Action.SUA)
            {
                BeforeModifyFromList();
            }

            //Hiển thị Form khi xem dữ liệu
            else if (action == DatabaseConstant.Action.XEM)
            {
                BeforeViewFromList();
            }            
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

        /// <summary>
        /// Hàm xử lý giải phóng lock dữ liệu cho form
        /// </summary>
        private void Release()
        {
            // Yêu cầu unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(id);

            bool ret = process.UnlockData(DatabaseConstant.Module.HMUC,
                DatabaseConstant.Function.HM_CTIET_CT,
                DatabaseConstant.Table.HM_CTIET,
                DatabaseConstant.Action.SUA,
                listLockId);
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

        private void btnMaKH_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<string> lstDieuKien = new List<string>();
                string lstIDKH = "";
                if (!txtMaKH.Text.IsNullOrEmptyOrSpace())
                    lstIDKH = "(" + idKhachHang.ToString() + ")";
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
                    DataRow dr = lstPopup[0];
                    idKhachHang = Convert.ToInt32(dr["ID"]);                    
                    txtMaKH.Text = dr["MA_KHANG"].ToString();
                    lblTenKH.Content = dr["TEN_KHANG"].ToString();                    
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

        private void btnMaHMTong_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                //list điều kiện theo từng loại sổ
                List<string> lstDieuKien = new List<string>();
                lstDieuKien.Add(idKhachHang.ToString());
                lstDieuKien.Add(ClientInformation.NgayLamViecHienTai);
                lstDieuKien.Add(ClientInformation.MaDonVi);

                var process = new PopupProcess();
                process.getPopupInformation(DatabaseConstant.DanhSachTruyVan.POPUP_DS_HMUC_TONG.getValue(), lstDieuKien);

                SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;

                ucPopup popup = new ucPopup(false, simplePopupResponse, false);
                popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
                Window win = new Window();
                win.Content = popup;
                Mouse.OverrideCursor = Cursors.Arrow;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.ShowDialog();
                if (lstPopup != null)
                {
                    DataRow dr = lstPopup[0];
                    idHanMucTong = Convert.ToInt32(dr["ID"].ToString());
                    txtMaHMTong.Text = dr["MA_HMUC"].ToString();                    
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void btnMaSanPham_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                //list điều kiện theo từng loại sổ
                List<string> lstDieuKien = new List<string>();
                lstDieuKien.Add(ClientInformation.NgayLamViecHienTai);
                lstDieuKien.Add(ClientInformation.MaDonVi);

                var process = new PopupProcess();
                process.getPopupInformation(DatabaseConstant.DanhSachTruyVan.POPUP_DS_SAN_PHAM_HMUC.getValue(), lstDieuKien);

                SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;

                ucPopup popup = new ucPopup(false, simplePopupResponse, false);
                popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
                Window win = new Window();
                win.Content = popup;
                Mouse.OverrideCursor = Cursors.Arrow;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.ShowDialog();
                if (lstPopup != null)
                {
                    DataRow dr = lstPopup[0];
                    idSanPham = Convert.ToInt32(dr["ID"].ToString());
                    txtMaSanPham.Text = dr["MA_SAN_PHAM"].ToString();
                    //lblTenSanPham.Content = drSanPham["TEN_SAN_PHAM"].ToString();
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void txtMaKH_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F3)
            {
                btnMaKH_Click(null, null);
            }
        }

        private void txtMaHMTong_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F3)
            {
                btnMaHMTong_Click(null, null);
            }
        }

        private void txtMaSanPham_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F3)
            {
                btnMaSanPham_Click(null, null);
            }
        }

        private void btnThem_Click(object sender, RoutedEventArgs e)
        {
            HM_CTIET_LOAITS objNhomSP = new HM_CTIET_LOAITS();
            objNhomSP.MA_DVI_QLY = ClientInformation.MaDonVi;
            lstHMLoaiTS.Add(objNhomSP);
            radgrTSDB.Rebind();
        }

        private void btnXoa_Click(object sender, RoutedEventArgs e)
        {
            HM_CTIET_LOAITS obj = ucNhomSP.cellEdit.ParentRow.Item as HM_CTIET_LOAITS;
            lstHMLoaiTS.Remove(obj);
        }

        private void ucNhomSP_EditCellEnd(object sender, EventArgs e)
        {
            HM_CTIET_LOAITS obj = ucNhomSP.cellEdit.ParentRow.Item as HM_CTIET_LOAITS;
            lstHMLoaiTS[lstHMLoaiTS.IndexOf(obj)].MA_LOAI_TSDB = ucNhomSP.GiaTri;
            lstHMLoaiTS[lstHMLoaiTS.IndexOf(obj)].ID_LOAI_TSDB = Convert.ToInt32(ucNhomSP.lstComboBox.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(ucNhomSP.GiaTri)).KeywordStrings[1]);
        }

        private void numHMPheDuyet_LostFocus(object sender, RoutedEventArgs e)
        {
            if (numHMKhongCoTaiSan.Value > numHMPheDuyet.Value)
            {
                LMessage.ShowMessage("M_ResponseMessage_SoTien_KhongHopLe", LMessage.MessageBoxType.Warning);
                numHMPheDuyet.Focus();
            }
            else
            {
                numHMCoTaiSan.Value = numHMPheDuyet.Value - numHMKhongCoTaiSan.Value;
            }
        }

        private void numHMKhongCoTaiSan_LostFocus(object sender, RoutedEventArgs e)
        {
            if (numHMKhongCoTaiSan.Value > numHMPheDuyet.Value)
            {
                LMessage.ShowMessage("M_ResponseMessage_SoTien_KhongHopLe", LMessage.MessageBoxType.Warning);                
            }
            else
            {
                numHMCoTaiSan.Value = numHMPheDuyet.Value - numHMKhongCoTaiSan.Value;
            }
        }


        #endregion

        #region Xử lý nghiệp vụ
        private void GetFormData(ref HM_HAN_MUC_KHACH_HANG_CTIET obj, string sTrangThaiNVu)
        {
            try
            {
                obj = new HM_HAN_MUC_KHACH_HANG_CTIET();

                #region HM_CTIET
                HM_CTIET objHMCTiet = new HM_CTIET();
                objHMCTiet.ID = id;
                objHMCTiet.ID_DTUONG = idKhachHang;
                objHMCTiet.ID_HMUC_TONG = idHanMucTong;
                objHMCTiet.LOAI_DTUONG = lstSourceLoaiKH[cmbLoaiKH.SelectedIndex].KeywordStrings[0];
                objHMCTiet.MA_DTUONG = txtMaKH.Text;
                objHMCTiet.MA_HMUC= txtMaHanMuc.Text;
                objHMCTiet.MA_HMUC_TONG = txtMaHMTong.Text;
                objHMCTiet.MA_SAN_PHAM = txtMaSanPham.Text;
                objHMCTiet.MA_LOAI_TIEN = lstSourceLoaiTien[cmbLoaiTien.SelectedIndex].KeywordStrings[0];
                objHMCTiet.HMUC_PDUYET = Convert.ToDecimal(numHMPheDuyet.Value);
                objHMCTiet.HMUC_CO_TSAN = Convert.ToDecimal(numHMCoTaiSan.Value);
                objHMCTiet.HMUC_KHONG_TSAN = Convert.ToDecimal(numHMKhongCoTaiSan.Value);
                objHMCTiet.HMUC_SDUNG = Convert.ToDecimal(numHMSuDung.Value);
                objHMCTiet.HMUC_DA_DUNG = Convert.ToDecimal(numHMDaDung.Value);
                objHMCTiet.HMUC_KDUNG = Convert.ToDecimal(numHMKhaDung.Value);

                if (radHMCoDinh.IsChecked == true)
                    objHMCTiet.LOAI_HMUC = "CO_DINH";
                else if (radHMBienDoi.IsChecked == true)
                    objHMCTiet.LOAI_HMUC = "BIEN_DOI";                

                if (raddtNgayHieuLuc.Value != null)
                    objHMCTiet.NGAY_HLUC = Convert.ToDateTime(raddtNgayHieuLuc.Value).ToString("yyyyMMdd");
                if (raddtNgayHetHieuLuc.Value != null)
                    objHMCTiet.NGAY_HET_HLUC = Convert.ToDateTime(raddtNgayHetHieuLuc.Value).ToString("yyyyMMdd");

                objHMCTiet.DIEN_GIAI = txtDienGiai.Text;

                objHMCTiet.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                objHMCTiet.TTHAI_NVU = sTrangThaiNVu;
                objHMCTiet.MA_DVI_QLY = ClientInformation.MaDonVi;
                objHMCTiet.MA_DVI_TAO = ClientInformation.MaDonViGiaoDich;
                objHMCTiet.NGAY_NHAP = Convert.ToDateTime(raddtNgayLap.Value).ToString("yyyyMMdd");
                objHMCTiet.NGUOI_NHAP = txtNguoiLap.Text;
                if (action != DatabaseConstant.Action.THEM)
                {
                    objHMCTiet.NGAY_CNHAT = ClientInformation.NgayLamViecHienTai;
                    objHMCTiet.NGUOI_CNHAT = ClientInformation.TenDangNhap;
                }


                obj.OBJ_HM_CTIET = objHMCTiet;
                #endregion

                #region HM_CTIET_LOAITS
                HM_CTIET_LOAITS objLoaiTS = null;                
                List<HM_CTIET_LOAITS> lst = new List<HM_CTIET_LOAITS>();
                foreach (HM_CTIET_LOAITS item in lstHMLoaiTS)
                {
                    objLoaiTS = new HM_CTIET_LOAITS();

                    objLoaiTS.ID = 0;
                    objLoaiTS.ID_HMUC_CTIET = id;
                    objLoaiTS.ID_LOAI_TSDB = item.ID_LOAI_TSDB;
                    objLoaiTS.ID_DTUONG = objHMCTiet.ID_DTUONG;
                    objLoaiTS.MA_HMUC_CTIET = objHMCTiet.MA_HMUC;
                    objLoaiTS.MA_LOAI_TSDB = item.MA_LOAI_TSDB;
                    objLoaiTS.LOAI_DTUONG = objHMCTiet.LOAI_DTUONG;
                    objLoaiTS.MA_DTUONG = objHMCTiet.MA_DTUONG;
                    objLoaiTS.GIA_TRI_TDA = item.GIA_TRI_TDA;
                    objLoaiTS.TTHAI_BGHI = objHMCTiet.TTHAI_BGHI;
                    objLoaiTS.TTHAI_NVU = objHMCTiet.TTHAI_NVU;
                    objLoaiTS.MA_DVI_QLY = objHMCTiet.MA_DVI_QLY;
                    objLoaiTS.MA_DVI_TAO = objHMCTiet.MA_DVI_TAO;
                    objLoaiTS.NGAY_NHAP = objHMCTiet.NGAY_NHAP;
                    objLoaiTS.NGUOI_NHAP = objHMCTiet.NGUOI_NHAP;
                    objLoaiTS.NGAY_CNHAT = objHMCTiet.NGAY_CNHAT;
                    objLoaiTS.NGUOI_CNHAT = objHMCTiet.NGUOI_CNHAT;
                    objLoaiTS.TTHAI_LY_DO = objHMCTiet.TTHAI_LY_DO;

                    lst.Add(objLoaiTS);                    
                }
                obj.LIST_HM_CTIET_LOAITS = lst.ToArray();
                #endregion

                obj.ID = id;                
                obj.MA_DVI = ClientInformation.MaDonVi;
                obj.MA_DVI_GDICH = ClientInformation.MaDonViGiaoDich;
                obj.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                obj.TTHAI_NVU = sTrangThaiNVu;
                obj.NGAY_NHAP = objHMCTiet.NGAY_NHAP;
                obj.NGUOI_NHAP = objHMCTiet.NGUOI_NHAP;
                obj.NGUOI_CNHAT = objHMCTiet.NGUOI_CNHAT;
                obj.NGAY_CNHAT = objHMCTiet.NGAY_CNHAT;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        private void SetFormData()
        {
            HanMucProcess processHanMuc = new HanMucProcess();
            List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
            obj = new HM_HAN_MUC_KHACH_HANG_CTIET();
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                bool ret = false;
                obj.ID = id;
                ret = processHanMuc.HanMucKhachHangChiTiet(DatabaseConstant.Action.LOAD, ref obj, ref listClientResponseDetail);
                if (ret == true)
                {
                    sTrangThaiNVu = obj.TTHAI_NVU;
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);
                    idKhachHang = obj.OBJ_HM_CTIET.ID_DTUONG;
                    idHanMucTong = obj.OBJ_HM_CTIET.ID_HMUC_TONG;

                    #region Thông tin chung
                    cmbLoaiKH.SelectedIndex = lstSourceLoaiKH.IndexOf(lstSourceLoaiKH.FirstOrDefault(i => i.KeywordStrings[0].Equals(obj.OBJ_HM_CTIET.LOAI_DTUONG)));
                    txtMaKH.Text = obj.OBJ_HM_CTIET.MA_DTUONG;
                    txtMaHanMuc.Text = obj.OBJ_HM_CTIET.MA_HMUC;
                    lblTenKH.Content = obj.TEN_KHANG;
                    txtMaHMTong.Text = obj.OBJ_HM_CTIET.MA_HMUC_TONG;
                    txtMaHanMuc.Text = obj.OBJ_HM_CTIET.MA_HMUC;
                    txtMaSanPham.Text = obj.OBJ_HM_CTIET.MA_SAN_PHAM;
                    cmbLoaiTien.SelectedIndex = lstSourceLoaiTien.IndexOf(lstSourceLoaiTien.FirstOrDefault(i => i.KeywordStrings[0].Equals(obj.OBJ_HM_CTIET.MA_LOAI_TIEN)));

                    if (obj.OBJ_HM_CTIET.LOAI_HMUC.Equals("CO_DINH"))
                        radHMCoDinh.IsChecked = true;
                    else if (obj.OBJ_HM_CTIET.LOAI_HMUC.Equals("BIEN_DOI"))
                        radHMBienDoi.IsChecked = true;

                    numHMPheDuyet.Value = Convert.ToDouble(obj.OBJ_HM_CTIET.HMUC_PDUYET);
                    numHMKhongCoTaiSan.Value = Convert.ToDouble(obj.OBJ_HM_CTIET.HMUC_KHONG_TSAN);
                    numHMCoTaiSan.Value = Convert.ToDouble(obj.OBJ_HM_CTIET.HMUC_CO_TSAN);
                    numHMSuDung.Value = Convert.ToDouble(obj.OBJ_HM_CTIET.HMUC_SDUNG);
                    numHMDaDung.Value = Convert.ToDouble(obj.OBJ_HM_CTIET.HMUC_DA_DUNG);
                    numHMKhaDung.Value = Convert.ToDouble(obj.OBJ_HM_CTIET.HMUC_KHONG_TSAN);
                    if (obj.OBJ_HM_CTIET.NGAY_HLUC != null)
                        raddtNgayHieuLuc.Value = LDateTime.StringToDate(obj.OBJ_HM_CTIET.NGAY_HLUC, "yyyyMMdd");
                    else
                        raddtNgayHieuLuc.Value = null;

                    if (obj.OBJ_HM_CTIET.NGAY_HET_HLUC != null)
                        raddtNgayHetHieuLuc.Value = LDateTime.StringToDate(obj.OBJ_HM_CTIET.NGAY_HET_HLUC, "yyyyMMdd");
                    else
                        raddtNgayHetHieuLuc.Value = null;

                    txtDienGiai.Text = obj.OBJ_HM_CTIET.DIEN_GIAI;

                    lstHMLoaiTS = obj.LIST_HM_CTIET_LOAITS.ToList();
                    radgrTSDB.ItemsSource = lstHMLoaiTS;
                    #endregion                    

                    #region Thông tin kiểm soát
                    txtTrangThaiBanGhi.Text = BusinessConstant.layNgonNguSuDung(obj.TTHAI_BGHI);
                    raddtNgayLap.Value = LDateTime.StringToDate(obj.NGAY_NHAP, "yyyyMMdd");
                    txtNguoiLap.Text = obj.NGUOI_NHAP;
                    if (LDateTime.IsDate(obj.NGAY_CNHAT, "yyyyMMdd") == true)
                        raddtNgayCapNhat.Value = LDateTime.StringToDate(obj.NGAY_CNHAT, "yyyyMMdd");
                    txtNguoiCapNhat.Text = obj.NGUOI_CNHAT;
                    #endregion

                }
                else
                {
                    LMessage.ShowMessage("M_ResponseMessage_HanhDong_LayDuLieuKhongThanhCong", LMessage.MessageBoxType.Warning);
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                LMessage.ShowMessage("M_ResponseMessage_HanhDong_LayDuLieuKhongThanhCong", LMessage.MessageBoxType.Warning);
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private void ResetForm()
        {
            sTrangThaiNVu = "";
            lblTrangThai.Content = "";            

            #region Thông tin chung
            txtMaKH.Text = "";
            lblTenKH.Content = "";
            txtMaHMTong.Text = "";
            txtMaHanMuc.Text = "";
            txtMaSanPham.Text = "";
            numHMPheDuyet.Value = 0;
            numHMKhongCoTaiSan.Value = 0;
            numHMCoTaiSan.Value = 0;
            numHMSuDung.Value = 0;
            numHMDaDung.Value = 0;
            numHMKhaDung.Value = 0;
            radHMCoDinh.IsChecked = true;
            raddtNgayHieuLuc.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
            raddtNgayHetHieuLuc.Value = null;
            txtDienGiai.Text = "";
            lstHMLoaiTS = new List<HM_CTIET_LOAITS>();
            radgrTSDB.ItemsSource = lstHMLoaiTS;

            #endregion

            #region Thông tin kiểm soát
            txtTrangThaiBanGhi.Text = "";
            raddtNgayLap.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
            txtNguoiLap.Text = ClientInformation.TenDangNhap;
            raddtNgayCapNhat.Value = null;
            txtNguoiCapNhat.Text = "";
            #endregion

        }

        private void ResetData()
        {
            action = DatabaseConstant.Action.THEM;
            obj = null;
            id = 0;
            idKhachHang = 0;
            idHanMucTong = 0;
            idSanPham = 0;            
            sTrangThaiNVu = "";

            ResetForm();
            SetEnabledControls();
            CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain, function);

            //chkThemNhieuLan.IsChecked = false;
        }

        private bool Validation()
        {
            try
            {
                if (txtMaKH.Text.IsNullOrEmptyOrSpace())
                {
                    CommonFunction.ThongBaoChuaChon(lblMaKH.Content.ToString());
                    txtMaKH.Focus();
                    return false;
                }

                else if (txtMaHMTong.Text.IsNullOrEmptyOrSpace())
                {
                    CommonFunction.ThongBaoChuaChon(lblMaHMTong.Content.ToString());
                    txtMaHMTong.Focus();
                    return false;
                }

                else if (txtMaSanPham.Text.IsNullOrEmptyOrSpace())
                {
                    CommonFunction.ThongBaoChuaChon(lblMaSanPham.Content.ToString());
                    txtMaSanPham.Focus();
                    return false;
                }

                else if (numHMPheDuyet.Value == null || numHMPheDuyet.Value == 0)
                {
                    CommonFunction.ThongBaoChuaNhap(lblHMPheDuyet.Content.ToString());
                    numHMPheDuyet.Focus();
                    return false;
                }

                else if (raddtNgayHieuLuc.Value == null || raddtNgayHieuLuc.Text == "__/__/____")
                {
                    CommonFunction.ThongBaoChuaNhap(lblNgayHieuLuc.Content.ToString());
                    raddtNgayHieuLuc.Focus();
                    return false;
                }

                else if (lstHMLoaiTS.Count == 0)
                {
                    LMessage.ShowMessage("M_ResponseMessage_HanMuc_ChuaNhapTaiSanDamBao", LMessage.MessageBoxType.Warning);
                    return false;
                }

                else if (((double)lstHMLoaiTS.Sum(e => e.GIA_TRI_TDA)) > numHMCoTaiSan.Value)
                {
                    LMessage.ShowMessage("M_ResponseMessage_HanMuc_TongTSDBToiDaLonHonHMCoTSDB", LMessage.MessageBoxType.Warning);
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                return false;
            }
        }

        private void SetEnabledControls()
        {
            #region Thêm
            if (action == DatabaseConstant.Action.THEM)
            {
                cmbLoaiKH.IsEnabled = false;
                txtMaKH.IsEnabled = true;
                btnMaKH.IsEnabled = true;
                txtMaHMTong.IsEnabled = true;
                btnMaHMTong.IsEnabled = true;
                txtMaHanMuc.IsEnabled = false;
                txtMaSanPham.IsEnabled = true;
                btnMaSanPham.IsEnabled = true;
                cmbLoaiTien.IsEnabled = true;
                numHMPheDuyet.IsEnabled = true;
                radHMCoDinh.IsEnabled = true;
                radHMBienDoi.IsEnabled = true;
                numHMKhongCoTaiSan.IsEnabled = true;
                numHMCoTaiSan.IsEnabled = true;
                numHMSuDung.IsEnabled = false;
                numHMDaDung.IsEnabled = false;
                numHMKhaDung.IsEnabled = false;
                raddtNgayHieuLuc.IsEnabled = true;
                dtpNgayHieuLuc.IsEnabled = true;
                raddtNgayHetHieuLuc.IsEnabled = true;
                dtpNgayHetHieuLuc.IsEnabled = true;
                txtDienGiai.IsEnabled = true;
                btnThem.IsEnabled = true;
                btnXoa.IsEnabled = true;

                radgrTSDB.IsReadOnly = false;
            }
            #endregion

            #region Sửa
            else if (action == DatabaseConstant.Action.SUA)
            {
                cmbLoaiKH.IsEnabled = false;
                txtMaKH.IsEnabled = true;
                btnMaKH.IsEnabled = true;
                txtMaHMTong.IsEnabled = true;
                btnMaHMTong.IsEnabled = true;
                txtMaHanMuc.IsEnabled = false;
                txtMaSanPham.IsEnabled = true;
                btnMaSanPham.IsEnabled = true;
                cmbLoaiTien.IsEnabled = true;
                numHMPheDuyet.IsEnabled = true;
                radHMCoDinh.IsEnabled = true;
                radHMBienDoi.IsEnabled = true;
                numHMKhongCoTaiSan.IsEnabled = true;
                numHMCoTaiSan.IsEnabled = true;
                numHMSuDung.IsEnabled = false;
                numHMDaDung.IsEnabled = false;
                numHMKhaDung.IsEnabled = false;
                raddtNgayHieuLuc.IsEnabled = true;
                dtpNgayHieuLuc.IsEnabled = true;
                raddtNgayHetHieuLuc.IsEnabled = true;
                dtpNgayHetHieuLuc.IsEnabled = true;
                txtDienGiai.IsEnabled = true;
                btnThem.IsEnabled = true;
                btnXoa.IsEnabled = true;

                radgrTSDB.IsReadOnly = false;
            }
            #endregion

            #region Xem ~ Mặc định
            else
            {
                cmbLoaiKH.IsEnabled = false;
                txtMaKH.IsEnabled = false;
                btnMaKH.IsEnabled = false;
                txtMaHMTong.IsEnabled = false;
                btnMaHMTong.IsEnabled = false;
                txtMaHanMuc.IsEnabled = false;
                txtMaSanPham.IsEnabled = false;
                btnMaSanPham.IsEnabled = false;
                cmbLoaiTien.IsEnabled = false;
                numHMPheDuyet.IsEnabled = false;
                radHMCoDinh.IsEnabled = false;
                radHMBienDoi.IsEnabled = false;
                numHMKhongCoTaiSan.IsEnabled = false;
                numHMCoTaiSan.IsEnabled = false;
                numHMSuDung.IsEnabled = false;
                numHMDaDung.IsEnabled = false;
                numHMKhaDung.IsEnabled = false;
                raddtNgayHieuLuc.IsEnabled = false;
                dtpNgayHieuLuc.IsEnabled = false;
                raddtNgayHetHieuLuc.IsEnabled = false;
                dtpNgayHetHieuLuc.IsEnabled = false;
                txtDienGiai.IsEnabled = false;
                btnThem.IsEnabled = false;
                btnXoa.IsEnabled = false;

                radgrTSDB.IsReadOnly = true;
            }
            #endregion
        }


        public void OnHold()
        {
            try
            {
                if (!Validation()) return;

                string trangThai = "";
                if (sTrangThaiNVu.Equals(BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri()))
                    trangThai = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();
                else
                    trangThai = CommonFunction.LayTrangThaiBanGhi(DatabaseConstant.Action.LUU_TAM, BusinessConstant.layTrangThaiNghiepVu(sTrangThaiNVu));

                obj = new HM_HAN_MUC_KHACH_HANG_CTIET();

                GetFormData(ref obj, trangThai);

                if (action == DatabaseConstant.Action.THEM)
                {
                    OnAddNew(obj);
                }
                else if (action == DatabaseConstant.Action.SUA)
                {
                    OnModify(obj);
                }
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        public void OnSave()
        {
            try
            {
                if (!Validation()) return;

                string trangThai = "";
                if (sTrangThaiNVu.Equals(BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri()))
                    trangThai = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();
                else
                    trangThai = CommonFunction.LayTrangThaiBanGhi(DatabaseConstant.Action.LUU, BusinessConstant.layTrangThaiNghiepVu(sTrangThaiNVu));

                obj = new HM_HAN_MUC_KHACH_HANG_CTIET();

                GetFormData(ref obj, trangThai);

                if (action == DatabaseConstant.Action.THEM)
                {
                    OnAddNew(obj);
                }
                else if (action == DatabaseConstant.Action.SUA)
                {
                    OnModify(obj);
                }
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }

        }

        public void BeforeViewFromDetail()
        {
            action = DatabaseConstant.Action.XEM;
            SetEnabledControls();
            CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain, function);
        }

        public void BeforeViewFromList()
        {
            SetFormData();
            BeforeViewFromDetail();
        }


        public void BeforeAddNew()
        {
            action = DatabaseConstant.Action.THEM;
            ResetForm();
            SetEnabledControls();
            CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain, function);
        }

        public void OnAddNew(HM_HAN_MUC_KHACH_HANG_CTIET obj)
        {

            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                HanMucProcess processHanMuc = new HanMucProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = false;

                ret = processHanMuc.HanMucKhachHangChiTiet(DatabaseConstant.Action.THEM, ref obj, ref listClientResponseDetail);
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

        public void AfterAddNew(bool ret, HM_HAN_MUC_KHACH_HANG_CTIET obj, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (ret)
                {
                    LMessage.ShowMessage("M.DungChung.ThemThanhCong", LMessage.MessageBoxType.Information);

                    if (chkThemNhieuLan.IsChecked == true)
                    {
                        ResetData();
                    }
                    else
                    {
                        id = obj.ID;
                        
                        sTrangThaiNVu = obj.TTHAI_NVU;
                        txtMaHanMuc.Text = obj.MA_HMUC;
                        lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);
                        txtTrangThaiBanGhi.Text = BusinessConstant.layNgonNguSuDung(obj.TTHAI_BGHI);

                        numHMSuDung.Value = (double)obj.OBJ_HM_CTIET.HMUC_SDUNG;
                        numHMDaDung.Value = (double)obj.OBJ_HM_CTIET.HMUC_DA_DUNG;
                        numHMKhaDung.Value = (double)obj.OBJ_HM_CTIET.HMUC_KDUNG;

                        BeforeViewFromDetail();
                    }
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


        public void BeforeModifyFromDetail()
        {
            try
            {
                // Yêu cầu lock bản ghi cần sửa
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(id);

                bool ret = process.LockData(DatabaseConstant.Module.HMUC,
                    DatabaseConstant.Function.HM_CTIET_CT,
                    DatabaseConstant.Table.HM_CTIET,
                    DatabaseConstant.Action.SUA,
                    listLockId);

                if (ret)
                {
                    action = DatabaseConstant.Action.SUA;
                    SetEnabledControls();
                    CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain, function);
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

        public void BeforeModifyFromList()
        {
            SetFormData();
            action = DatabaseConstant.Action.SUA;
            SetEnabledControls();
            CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain, function);
        }

        public void OnModify(HM_HAN_MUC_KHACH_HANG_CTIET obj)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                HanMucProcess processHanMuc = new HanMucProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = false;

                ret = processHanMuc.HanMucKhachHangChiTiet(DatabaseConstant.Action.SUA, ref obj, ref listClientResponseDetail);
                AfterModify(ret, obj, listClientResponseDetail);
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

        public void AfterModify(bool ret, HM_HAN_MUC_KHACH_HANG_CTIET obj, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (ret)
                {
                    LMessage.ShowMessage("M.DungChung.CapNhatThanhCong", LMessage.MessageBoxType.Information);

                    sTrangThaiNVu = obj.TTHAI_NVU;
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);
                    txtTrangThaiBanGhi.Text = BusinessConstant.layNgonNguSuDung(obj.TTHAI_BGHI);
                    raddtNgayCapNhat.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
                    txtNguoiCapNhat.Text = ClientInformation.TenDangNhap;

                    numHMSuDung.Value = (double)obj.OBJ_HM_CTIET.HMUC_SDUNG;
                    numHMDaDung.Value = (double)obj.OBJ_HM_CTIET.HMUC_DA_DUNG;
                    numHMKhaDung.Value = (double)obj.OBJ_HM_CTIET.HMUC_KDUNG;

                    BeforeViewFromDetail();
                }
                else
                {
                    CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                    //LMessage.ShowMessage("M.DungChung.CapNhatKoThanhCong", LMessage.MessageBoxType.Error);
                }

                // Yêu cầu Unlock bản ghi cần sửa
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(id);

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.HMUC,
                    DatabaseConstant.Function.HM_CTIET_CT,
                    DatabaseConstant.Table.HM_CTIET,
                    DatabaseConstant.Action.SUA,
                    listLockId);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }


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
                    listLockId.Add(id);
                    List<int> listLockedId = new List<int>();

                    bool retLockData = process.LockData(DatabaseConstant.Module.HMUC,
                        DatabaseConstant.Function.HM_CTIET_CT,
                        DatabaseConstant.Table.HM_CTIET,
                        DatabaseConstant.Action.XOA,
                        listLockId);

                    if (retLockData)
                    {
                        action = DatabaseConstant.Action.XOA;
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
                listLockId.Add(id);
                List<int> listUnlockId = new List<int>();

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.HMUC,
                    DatabaseConstant.Function.HM_CTIET_CT,
                    DatabaseConstant.Table.HM_CTIET,
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
            HanMucProcess processHanMuc = new HanMucProcess();
            try
            {
                bool ret = false;
                obj.ID = id;
                ret = processHanMuc.HanMucKhachHangChiTiet(action, ref obj, ref listClientResponseDetail);
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
                processHanMuc = null;
            }
        }

        public void AfterDelete(bool ret, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (ret)
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
                listLockId.Add(id);
                List<int> listUnlockId = new List<int>();

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.HMUC,
                    DatabaseConstant.Function.HM_CTIET_CT,
                    DatabaseConstant.Table.HM_CTIET,
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


        public void BeforeApprove()
        {
            try
            {
                // Cảnh báo người dùng
                MessageBoxResult ret = LMessage.ShowMessage("M.DungChung.HoiDuyet", LMessage.MessageBoxType.Question);

                if (ret == MessageBoxResult.Yes)
                {
                    // Lock dữ liệu nếu người dùng chấp nhận cảnh báo
                    UtilitiesProcess process = new UtilitiesProcess();
                    List<int> listLockId = new List<int>();
                    listLockId.Add(id);
                    List<int> listLockedId = new List<int>();

                    bool retLockData = process.LockData(DatabaseConstant.Module.HMUC,
                        DatabaseConstant.Function.HM_CTIET_CT,
                        DatabaseConstant.Table.HM_CTIET,
                        DatabaseConstant.Action.DUYET,
                        listLockId);

                    if (retLockData)
                    {
                        action = DatabaseConstant.Action.DUYET;
                        OnApprove();
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
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        public void OnApprove()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
            HanMucProcess processHanMuc = new HanMucProcess();
            try
            {
                bool ret = false;
                obj.ID = id;
                ret = processHanMuc.HanMucKhachHangChiTiet(action, ref obj, ref listClientResponseDetail);
                AfterApprove(ret, listClientResponseDetail);
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
                processHanMuc = null;
            }
        }

        public void AfterApprove(bool ret, List<ClientResponseDetail> listClientResponseDetail)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
            try
            {
                if (ret)
                {
                    LMessage.ShowMessage("M.DungChung.DuyetThanhCong", LMessage.MessageBoxType.Information);

                    sTrangThaiNVu = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);

                    BeforeViewFromDetail();
                }
                else
                {
                    CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                }

                // Yêu cầu unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(id);
                List<int> listUnlockId = new List<int>();

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.HMUC,
                    DatabaseConstant.Function.HM_CTIET_CT,
                    DatabaseConstant.Table.HM_CTIET,
                    DatabaseConstant.Action.DUYET,
                    listLockId);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }


        public void BeforeCancel()
        {
            try
            {
                // Cảnh báo người dùng
                MessageBoxResult ret = LMessage.ShowMessage("M.DungChung.HoiThoaiDuyet", LMessage.MessageBoxType.Question);

                if (ret == MessageBoxResult.Yes)
                {
                    // Lock dữ liệu nếu người dùng chấp nhận cảnh báo
                    UtilitiesProcess process = new UtilitiesProcess();
                    List<int> listLockId = new List<int>();
                    listLockId.Add(id);
                    List<int> listLockedId = new List<int>();

                    bool retLockData = process.LockData(DatabaseConstant.Module.HMUC,
                        DatabaseConstant.Function.HM_CTIET_CT,
                        DatabaseConstant.Table.HM_CTIET,
                        DatabaseConstant.Action.THOAI_DUYET,
                        listLockId);

                    if (retLockData)
                    {
                        action = DatabaseConstant.Action.THOAI_DUYET;
                        OnCancel();
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
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        public void OnCancel()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
            HanMucProcess processHanMuc = new HanMucProcess();
            try
            {
                bool ret = false;
                obj.ID = id;
                ret = processHanMuc.HanMucKhachHangChiTiet(action, ref obj, ref listClientResponseDetail);
                AfterCancel(ret, listClientResponseDetail);
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
                processHanMuc = null;
            }
        }

        public void AfterCancel(bool ret, List<ClientResponseDetail> listClientResponseDetail)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
            try
            {
                if (ret)
                {
                    LMessage.ShowMessage("M.DungChung.ThoaiDuyetThanhCong", LMessage.MessageBoxType.Information);

                    sTrangThaiNVu = BusinessConstant.TrangThaiNghiepVu.THOAI_DUYET.layGiaTri();
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);

                    BeforeViewFromDetail();
                }
                else
                {
                    CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                }

                // Yêu cầu unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(id);
                List<int> listUnlockId = new List<int>();

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.HMUC,
                    DatabaseConstant.Function.HM_CTIET_CT,
                    DatabaseConstant.Table.HM_CTIET,
                    DatabaseConstant.Action.THOAI_DUYET,
                    listLockId);

            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }


        public void BeforeRefuse()
        {
            try
            {
                // Cảnh báo người dùng
                MessageBoxResult ret = LMessage.ShowMessage("M.DungChung.HoiTuChoiDuyet", LMessage.MessageBoxType.Question);

                if (ret == MessageBoxResult.Yes)
                {
                    // Lock dữ liệu nếu người dùng chấp nhận cảnh báo
                    UtilitiesProcess process = new UtilitiesProcess();
                    List<int> listLockId = new List<int>();
                    listLockId.Add(id);
                    List<int> listLockedId = new List<int>();

                    bool retLockData = process.LockData(DatabaseConstant.Module.HMUC,
                        DatabaseConstant.Function.HM_CTIET_CT,
                        DatabaseConstant.Table.HM_CTIET,
                        DatabaseConstant.Action.TU_CHOI_DUYET,
                        listLockId);

                    if (retLockData)
                    {
                        action = DatabaseConstant.Action.TU_CHOI_DUYET;
                        OnRefuse();
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
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        public void OnRefuse()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
            HanMucProcess processHanMuc = new HanMucProcess();
            try
            {
                bool ret = false;
                obj.ID = id;
                ret = processHanMuc.HanMucKhachHangChiTiet(action, ref obj, ref listClientResponseDetail);
                AfterRefuse(ret, listClientResponseDetail);
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
                processHanMuc = null;
            }
        }

        public void AfterRefuse(bool ret, List<ClientResponseDetail> listClientResponseDetail)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
            try
            {
                if (ret)
                {
                    LMessage.ShowMessage("M.DungChung.TuChoiDuyetThanhCong", LMessage.MessageBoxType.Information);

                    sTrangThaiNVu = BusinessConstant.TrangThaiNghiepVu.TU_CHOI.layGiaTri();
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);

                    BeforeViewFromDetail();
                }
                else
                {
                    CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                }

                // Yêu cầu unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(id);
                List<int> listUnlockId = new List<int>();

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.HMUC,
                    DatabaseConstant.Function.HM_CTIET_CT,
                    DatabaseConstant.Table.HM_CTIET,
                    DatabaseConstant.Action.TU_CHOI_DUYET,
                    listLockId);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }


        #endregion                        

    }
}
