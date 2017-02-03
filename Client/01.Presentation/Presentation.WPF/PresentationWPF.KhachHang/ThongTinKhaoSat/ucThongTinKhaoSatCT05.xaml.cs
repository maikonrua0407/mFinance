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
using Telerik.Windows.Controls;
using Utilities.Common;
using Presentation.Process;
using Presentation.Process.Common;
using Presentation.Process.PopupServiceRef;
using Presentation.Process.KhachHangServiceRef;
using PresentationWPF.CustomControl;
using PresentationWPF.KhachHang.KhachHang.Popup;
using PresentationWPF.BaoCao.DungChung;

namespace PresentationWPF.KhachHang.ThongTinKhaoSat
{
    /// <summary>
    /// Interaction logic for ucThongTinKhaoSatCT05.xaml
    /// </summary>
    public partial class ucThongTinKhaoSatCT05 : UserControl
    {
        #region Khai bao
        // các thao tác trên Form: thêm, sửa, xem... (để mặc định là thêm)
        private DatabaseConstant.Action action = DatabaseConstant.Action.THEM;
        public DatabaseConstant.Action Action
        {
            get { return action; }
            set { action = value; }
        }

        private DatabaseConstant.Function function = DatabaseConstant.Function.KH_THANH_VIEN;

        public event EventHandler OnSavingCompleted;

        private int id = 0;
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        private int idKhachHang = 0;
        private string maKhachHang = "";
        private string loaiDXVV = "";

        private KH_THONG_TIN_KHAO_SAT obj;
        public KH_THONG_TIN_KHAO_SAT Obj
        {
            get { return obj; }
            set { obj = value; }
        }

        private string sTrangThaiNVu = "";
        
        private DataSet dsSource  = new DataSet();

        List<AutoCompleteEntry> lstSourceGioiTinh = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceGioiTinhNguoiThuaKe = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceGioiTinhNguoiBaoLanh = new List<AutoCompleteEntry>();

        List<AutoCompleteEntry> lstSourceDanToc = new List<AutoCompleteEntry>();       
        List<AutoCompleteEntry> lstSourceDanTocNguoiThuaKe = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceDanTocNguoiBaoLanh = new List<AutoCompleteEntry>();

        List<AutoCompleteEntry> lstSourcePhuongTienDiLai = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceNoiGuiTK = new List<AutoCompleteEntry>();
        

        List<DataRow> lstPopup = new List<DataRow>();
        public void LayDuLieuTuPopup(List<DataRow> lst)
        {
            lstPopup = lst;
        }

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
        public ucThongTinKhaoSatCT05()
        {
            InitializeComponent();

            DuyetQuyenTinhNang();

            BindShortkey();

            LoadCombobox();

            InitEventHandler();

            loaiDXVV = ClientInformation.FormCase;
        }

        private void DuyetQuyenTinhNang()
        {
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.KhachHang;component/ThongTinKhaoSat/ucThongTinKhaoSatCT05.xaml", ref Toolbar, ref mnuMain);
            foreach (var item in mnuMain.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += btnShortcutKey_Click;
            }
        }

        private void LoadCombobox()
        {
            COMBOBOX_DTO combo = null;
            List<string> lstDieuKien = null;
            List<COMBOBOX_DTO> lstCombobox = new List<COMBOBOX_DTO>();

            //Giới tính
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.GIOI_TINH.getValue());
            combo = new COMBOBOX_DTO();
            combo.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();
            combo.combobox = cmbGioiTinh;
            combo.lstSource = lstSourceGioiTinh;
            combo.lstDieuKien = lstDieuKien;
            lstCombobox.Add(combo);

            //Dân tộc
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.DAN_TOC.getValue());
            combo = new COMBOBOX_DTO();
            combo.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();
            combo.combobox = cmbDanToc;
            combo.lstSource = lstSourceDanToc;
            combo.lstDieuKien = lstDieuKien;
            lstCombobox.Add(combo);

            //Giới tính người thừa kế
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.GIOI_TINH.getValue());
            combo = new COMBOBOX_DTO();
            combo.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();
            combo.combobox = cmbGioiTinhNguoiThuaKe;
            combo.lstSource = lstSourceGioiTinhNguoiThuaKe;
            combo.lstDieuKien = lstDieuKien;
            lstCombobox.Add(combo);

            //Dân tộc người thừa kế
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.DAN_TOC.getValue());
            combo = new COMBOBOX_DTO();
            combo.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();
            combo.combobox = cmbDanTocNguoiThuaKe;
            combo.lstSource = lstSourceDanTocNguoiThuaKe;
            combo.lstDieuKien = lstDieuKien;
            lstCombobox.Add(combo);

            //Giới tính người bảo lãnh
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.GIOI_TINH.getValue());
            combo = new COMBOBOX_DTO();
            combo.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();
            combo.combobox = cmbGioiTinhNguoiBaoLanh;
            combo.lstSource = lstSourceGioiTinhNguoiBaoLanh;
            combo.lstDieuKien = lstDieuKien;
            lstCombobox.Add(combo);

            //Dân tộc người bảo lãnh
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.DAN_TOC.getValue());
            combo = new COMBOBOX_DTO();
            combo.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();
            combo.combobox = cmbDanTocNguoiBaoLanh;
            combo.lstSource = lstSourceDanTocNguoiBaoLanh;
            combo.lstDieuKien = lstDieuKien;
            lstCombobox.Add(combo);

            //Phương tiện đi lại
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.PHUONG_TIEN_DI_LAI.getValue());
            combo = new COMBOBOX_DTO();
            combo.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();
            combo.combobox = cmbPhuongTien;
            combo.lstSource = lstSourcePhuongTienDiLai;
            combo.lstDieuKien = lstDieuKien;
            lstCombobox.Add(combo);

            //Nơi gửi tiền
            lstDieuKien = new List<string>();
            lstDieuKien.Add("GDINH_KTE_LHINH_TKIEM");
            combo = new COMBOBOX_DTO();
            combo.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();
            combo.combobox = cmbNoiGuiTK;
            combo.lstSource = lstSourceNoiGuiTK;
            combo.lstDieuKien = lstDieuKien;
            lstCombobox.Add(combo);

            AutoComboBox auto = new AutoComboBox();
            auto.GenAutoComboBoxTheoList(ref lstCombobox);

            //Combobox tính chất sử dụng đất - Grid Sử dụng đất
            DataSet ds = new DanhMucProcess().GetDanhMucGTri("TINH_CHAT_SDUNG_DAT");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                ((GridViewComboBoxColumn)grCongCuSuDungDat.Columns["TINH_CHAT"]).ItemsSource = ds.Tables[0].DefaultView;
            }
            else
            {
                ((GridViewComboBoxColumn)grCongCuSuDungDat.Columns["TINH_CHAT"]).ItemsSource = null;
            }
        }       

        private void InitEventHandler()
        {
            chkThemNhieuLan.Checked += new RoutedEventHandler(chkThemNhieuLan_Checked);

            ucColChiTieu.EditCellEnd += new EventHandler(ucColChiTieu_EditCellEnd);
            ucColChiTieuNguoiThuaKe.EditCellEnd += new EventHandler(ucColChiTieuNguoiThuaKe_EditCellEnd);
            ucColChiTieuNguoiBaoLanh.EditCellEnd += new EventHandler(ucColChiTieuNguoiBaoLanh_EditCellEnd);
            ucColNoiCuTru.EditCellEnd += new EventHandler(ucColNoiCuTru_EditCellEnd);
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
            OnHold();
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
            BeforeApprove();
        }

        private void RefuseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void RefuseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            BeforeRefuse();
        }

        private void CancelCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void CancelCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            BeforeCancel();
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

            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU_TAM)))
            {
                OnHold();
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
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XEM_TRUOC)))
            {

            }
            else if (strTinhNang.Equals("PreviewKhaoSat"))
            {
                OnPreviewKhaoSat();
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
                OnHold();
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
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XEM_TRUOC)))
            {
            }
            else if (strTinhNang.Equals("PreviewKhaoSat"))
            {
                OnPreviewKhaoSat();
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

            bool ret = process.UnlockData(DatabaseConstant.Module.KHTV,
                DatabaseConstant.Function.KH_THANH_VIEN,
                DatabaseConstant.Table.KH_KHANG_HSO,
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

        private void chkThemNhieuLan_Checked(object sender, RoutedEventArgs e)
        {
            action = DatabaseConstant.Action.THEM;
            id = 0;
            obj = null;
            sTrangThaiNVu = "";

            ResetForm();
            SetEnabledControls();
            CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain, function);

            chkThemNhieuLan.IsChecked = false;
        }

        private void btnDonXinVayVon_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                //list điều kiện theo từng loại sổ
                List<string> lstDieuKien = new List<string>();

                if (ClientInformation.FormCase == "")
                    lstDieuKien.Add("KSKH");
                else
                    lstDieuKien.Add(ClientInformation.FormCase);
                lstDieuKien.Add(ClientInformation.MaDonViGiaoDich);

                var process = new PopupProcess();
                process.getPopupInformation(DatabaseConstant.DanhSachTruyVan.POPUP_DS_KSKH_DON_XIN_VAY_VON.getValue(), lstDieuKien);

                SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;

                ucPopup popup = new ucPopup(false, simplePopupResponse, false);
                popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
                Window win = new Window();
                win.Content = popup;
                Mouse.OverrideCursor = Cursors.Arrow;
                win.Title = LLanguage.SearchResourceByKey("MENU.TDTD_DON_VAY_VON_DS");
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.ShowDialog();
                if (lstPopup != null)
                {
                    DataRow row = lstPopup[0];

                    txtDonXinVayVon.Text = row["MA_DXVV"].ToString();
                    txtTenKhachHang.Text = row["TEN_KHANG"].ToString();                    
                    raddtNgayLapDon.Value = LDateTime.StringToDate(row["NGAY_HD"].ToString(),"yyyyMMdd");
                    idKhachHang = Convert.ToInt32(row["ID_KHANG"]);
                    maKhachHang = row["MA_KHANG"].ToString();

                    LayThongTin();
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void LayThongTin()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                KhachHangProcess processKhachHang = new KhachHangProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = false;

                obj = new KH_THONG_TIN_KHAO_SAT();
                obj.LOAI_DXVV = ClientInformation.FormCase;
                obj.MA_DXVV = txtDonXinVayVon.Text;
                obj.ID_KHANG = idKhachHang;
                
                ret = processKhachHang.ThongTinKhaoSat05(DatabaseConstant.Action.LOAD_DATA, ref obj, ref listClientResponseDetail);
                if(ret == true)
                {
                    //Thông tin người vay vốn
                    cmbGioiTinh.SelectedIndex = lstSourceGioiTinh.IndexOf(lstSourceGioiTinh.FirstOrDefault(i => i.KeywordStrings.First().Equals(obj.OBJ_VKH_THONG_TIN_KHAO_SAT.GIOI_TINH)));
                    txtTenCha.Text = obj.OBJ_VKH_THONG_TIN_KHAO_SAT.TEN_CHA;
                    if (LDateTime.IsDate(obj.OBJ_VKH_THONG_TIN_KHAO_SAT.NGAY_SINH, "yyyyMMdd"))
                        raddtNgaySinh.Value = LDateTime.StringToDate(obj.OBJ_VKH_THONG_TIN_KHAO_SAT.NGAY_SINH, "yyyyMMdd");
                    txtCMND.Text = obj.OBJ_VKH_THONG_TIN_KHAO_SAT.SO_CMND;
                    if (LDateTime.IsDate(obj.OBJ_VKH_THONG_TIN_KHAO_SAT.NGAY_CAP, "yyyyMMdd"))
                        raddtNgayCap.Value = LDateTime.StringToDate(obj.OBJ_VKH_THONG_TIN_KHAO_SAT.NGAY_CAP, "yyyyMMdd");
                    txtNoiCap.Text = obj.OBJ_VKH_THONG_TIN_KHAO_SAT.NOI_CAP;
                    cmbDanToc.SelectedIndex = lstSourceDanToc.IndexOf(lstSourceDanToc.FirstOrDefault(i => i.KeywordStrings.First().Equals(obj.OBJ_VKH_THONG_TIN_KHAO_SAT.DAN_TOC)));
                    txtDienThoai.Text = obj.OBJ_VKH_THONG_TIN_KHAO_SAT.SO_DIEN_THOAI;
                    txtDiaChi.Text = obj.OBJ_VKH_THONG_TIN_KHAO_SAT.DIA_CHI;
                    txtEmail.Text = obj.OBJ_VKH_THONG_TIN_KHAO_SAT.EMAIL;

                    //Thông tin người thừa kế
                    txtTenNguoiThuaKe.Text = obj.OBJ_VKH_THONG_TIN_KHAO_SAT.TEN_NTKE;
                    if (LDateTime.IsDate(obj.OBJ_VKH_THONG_TIN_KHAO_SAT.NGAY_SINH_NTKE, "yyyyMMdd"))
                        raddtNgaySinhNguoiThuaKe.Value = LDateTime.StringToDate(obj.OBJ_VKH_THONG_TIN_KHAO_SAT.NGAY_SINH_NTKE, "yyyyMMdd");
                    cmbGioiTinhNguoiThuaKe.SelectedIndex = lstSourceGioiTinhNguoiThuaKe.IndexOf(lstSourceGioiTinhNguoiThuaKe.FirstOrDefault(i => i.KeywordStrings.First().Equals(obj.OBJ_VKH_THONG_TIN_KHAO_SAT.GIOI_TINH_NTKE)));
                    txtTenCha.Text = obj.OBJ_VKH_THONG_TIN_KHAO_SAT.TEN_CHA_NTKE;
                    txtCMNDNguoiThuaKe.Text = obj.OBJ_VKH_THONG_TIN_KHAO_SAT.SO_CMND_NTKE;
                    if (LDateTime.IsDate(obj.OBJ_VKH_THONG_TIN_KHAO_SAT.NGAY_CAP_NTKE, "yyyyMMdd"))
                        raddtNgayCapNguoiThuaKe.Value = LDateTime.StringToDate(obj.OBJ_VKH_THONG_TIN_KHAO_SAT.NGAY_CAP_NTKE, "yyyyMMdd");
                    txtNoiCapNguoiThuaKe.Text = obj.OBJ_VKH_THONG_TIN_KHAO_SAT.NOI_CAP_NTKE;
                    cmbDanTocNguoiThuaKe.SelectedIndex = lstSourceDanTocNguoiThuaKe.IndexOf(lstSourceDanTocNguoiThuaKe.FirstOrDefault(i => i.KeywordStrings.First().Equals(obj.OBJ_VKH_THONG_TIN_KHAO_SAT.DAN_TOC_NTKE)));
                    txtDienThoaiNguoiThuaKe.Text = obj.OBJ_VKH_THONG_TIN_KHAO_SAT.SO_DIEN_THOAI_NTKE;
                    txtEmailNguoiThuaKe.Text = obj.OBJ_VKH_THONG_TIN_KHAO_SAT.EMAIL_NTKE;
                    txtDiaChiNguoiThuaKe.Text = obj.OBJ_VKH_THONG_TIN_KHAO_SAT.DIA_CHI_NTKE;

                    //Thông tin người bảo lãnh
                    txtTenNguoiBaoLanh.Text = obj.OBJ_VKH_THONG_TIN_KHAO_SAT.TEN_BLANH;
                    if (LDateTime.IsDate(obj.OBJ_VKH_THONG_TIN_KHAO_SAT.NGAY_SINH_BLANH, "yyyyMMdd"))
                        raddtNgaySinhNguoiBaoLanh.Value = LDateTime.StringToDate(obj.OBJ_VKH_THONG_TIN_KHAO_SAT.NGAY_SINH_BLANH, "yyyyMMdd");
                    cmbGioiTinhNguoiBaoLanh.SelectedIndex = lstSourceGioiTinhNguoiBaoLanh.IndexOf(lstSourceGioiTinhNguoiBaoLanh.FirstOrDefault(i => i.KeywordStrings.First().Equals(obj.OBJ_VKH_THONG_TIN_KHAO_SAT.GIOI_TINH_BLANH)));
                    txtTenCha.Text = obj.OBJ_VKH_THONG_TIN_KHAO_SAT.TEN_CHA_BLANH;
                    txtCMNDNguoiBaoLanh.Text = obj.OBJ_VKH_THONG_TIN_KHAO_SAT.SO_CMND_BLANH;
                    if (LDateTime.IsDate(obj.OBJ_VKH_THONG_TIN_KHAO_SAT.NGAY_CAP_BLANH, "yyyyMMdd"))
                        raddtNgayCapNguoiBaoLanh.Value = LDateTime.StringToDate(obj.OBJ_VKH_THONG_TIN_KHAO_SAT.NGAY_CAP_BLANH, "yyyyMMdd");
                    txtNoiCapNguoiBaoLanh.Text = obj.OBJ_VKH_THONG_TIN_KHAO_SAT.NOI_CAP_BLANH;
                    cmbDanTocNguoiBaoLanh.SelectedIndex = lstSourceDanTocNguoiBaoLanh.IndexOf(lstSourceDanTocNguoiBaoLanh.FirstOrDefault(i => i.KeywordStrings.First().Equals(obj.OBJ_VKH_THONG_TIN_KHAO_SAT.DAN_TOC_BLANH)));
                    txtDienThoaiNguoiBaoLanh.Text = obj.OBJ_VKH_THONG_TIN_KHAO_SAT.SO_DIEN_THOAI_BLANH;
                    txtDiaChiNguoiBaoLanh.Text = obj.OBJ_VKH_THONG_TIN_KHAO_SAT.DIA_CHI_BLANH;

                    if(obj.IS_KHAO_SAT == true)
                    {
                        dsSource = obj.DATA_SET;

                        foreach (DataRow dr in dsSource.Tables["CHI_TIEU_NGUOI_VAY_VON"].Rows)
                            dr["TEN"] = LLanguage.SearchResourceByKey(dr["MA_NNGU"].ToString());

                        foreach (DataRow dr in dsSource.Tables["CHI_TIEU_NGUOI_THUA_KE"].Rows)
                            dr["TEN"] = LLanguage.SearchResourceByKey(dr["MA_NNGU"].ToString());

                        foreach (DataRow dr in dsSource.Tables["CHI_TIEU_NGUOI_BAO_LANH"].Rows)
                            dr["TEN"] = LLanguage.SearchResourceByKey(dr["MA_NNGU"].ToString());

                        foreach (DataRow dr in dsSource.Tables["THU_NHAP"].Rows)
                            dr["TEN"] = LLanguage.SearchResourceByKey(dr["MA_NNGU"].ToString());

                        foreach (DataRow dr in dsSource.Tables["CHI_PHI"].Rows)
                            dr["TEN"] = LLanguage.SearchResourceByKey(dr["MA_NNGU"].ToString());

                        foreach (DataRow dr in dsSource.Tables["NOI_CU_TRU"].Rows)
                            dr["TEN"] = LLanguage.SearchResourceByKey(dr["MA_NNGU"].ToString());

                        foreach (DataRow dr in dsSource.Tables["SU_DUNG_DAT"].Rows)
                            dr["TEN"] = LLanguage.SearchResourceByKey(dr["MA_NNGU"].ToString());

                        foreach (DataRow dr in dsSource.Tables["TINH_HINH_TDUNG"].Rows)
                            dr["TEN"] = LLanguage.SearchResourceByKey(dr["MA_NNGU"].ToString());

                        foreach (DataRow dr in dsSource.Tables["GDINH_NGUOI_TKE"].Rows)
                        { 
                            if (dr["GIOI_TINH1"] != null)
                                dr["GIOI_TINH1"] = LLanguage.SearchResourceByKey(dr["GIOI_TINH1"].ToString());

                            if (dr["TEN_VAI_TRO"] != null)
                                dr["TEN_VAI_TRO"] = LLanguage.SearchResourceByKey(dr["TEN_VAI_TRO"].ToString());
                        }

                        grChiTieu.DataContext = dsSource.Tables["CHI_TIEU_NGUOI_VAY_VON"].DefaultView;
                        grChiTieuNguoiThuaKe.DataContext = dsSource.Tables["CHI_TIEU_NGUOI_THUA_KE"].DefaultView;
                        grChiTieuNguoiBaoLanh.DataContext = dsSource.Tables["CHI_TIEU_NGUOI_BAO_LANH"].DefaultView;
                        grNguoiThuaKe.ItemsSource = dsSource.Tables["GDINH_NGUOI_TKE"].DefaultView;
                        grThuNhap.DataContext = dsSource.Tables["THU_NHAP"].DefaultView;
                        grChiPhi.DataContext = dsSource.Tables["CHI_PHI"].DefaultView;
                        grNoiCuTru.DataContext = dsSource.Tables["NOI_CU_TRU"].DefaultView;
                        grCongCuSuDungDat.DataContext = dsSource.Tables["SU_DUNG_DAT"].DefaultView;
                        grTinhHinhTinDung.DataContext = dsSource.Tables["TINH_HINH_TDUNG"].DefaultView;

                        //Thông tin tài sản                    
                        cmbPhuongTien.SelectedIndex = lstSourcePhuongTienDiLai.IndexOf(lstSourcePhuongTienDiLai.FirstOrDefault(i => i.KeywordStrings.First().Equals(obj.OBJ_VKH_THONG_TIN_KHAO_SAT.PTIEN_DI_LAI)));
                        numGiaPhuongTienUocTinh.Value = Convert.ToDouble(obj.OBJ_VKH_THONG_TIN_KHAO_SAT.PTIEN_GIA_TRI);
                        numTienMatTichLuy.Value = Convert.ToDouble(obj.OBJ_VKH_THONG_TIN_KHAO_SAT.TONG_TIEN_TICH_LUY);
                        numSoTienGuiTK.Value = Convert.ToDouble(obj.OBJ_VKH_THONG_TIN_KHAO_SAT.TONG_TIEN_GUI_TKIEM);
                        cmbNoiGuiTK.SelectedIndex = lstSourceNoiGuiTK.IndexOf(lstSourceNoiGuiTK.FirstOrDefault(i => i.KeywordStrings.First().Equals(obj.OBJ_VKH_THONG_TIN_KHAO_SAT.NOI_GUI_TKIEM)));
                        numTaiSanKhac.Value = Convert.ToDouble(obj.OBJ_VKH_THONG_TIN_KHAO_SAT.TAI_SAN_KHAC);
                        numTongTaiSanUocTinh.Value = Convert.ToDouble(obj.OBJ_VKH_THONG_TIN_KHAO_SAT.TONG_TAI_SAN);   
                    }
                    else
                    {

                        dsSource.Tables.Remove("GDINH_NGUOI_TKE");
                        dsSource.Tables.Add(obj.DATA_SET.Tables["VKH_GDINH_NGUOI_TKE"].Copy());
                        dsSource.Tables["VKH_GDINH_NGUOI_TKE"].TableName = "GDINH_NGUOI_TKE";
                        foreach (DataRow dr in dsSource.Tables["GDINH_NGUOI_TKE"].Rows)
                        {
                            if (dr["GIOI_TINH1"] != null)
                                dr["GIOI_TINH1"] = LLanguage.SearchResourceByKey(dr["GIOI_TINH1"].ToString());

                            if (dr["TEN_VAI_TRO"] != null)
                                dr["TEN_VAI_TRO"] = LLanguage.SearchResourceByKey(dr["TEN_VAI_TRO"].ToString());
                        }
                        grNguoiThuaKe.ItemsSource = dsSource.Tables["GDINH_NGUOI_TKE"].DefaultView;
                    }
                    
                }
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

        private void ucColChiTieu_EditCellEnd(object sender, EventArgs e)
        {
            DataRowView dtrv = ucColChiTieu.objDataItem as DataRowView;
            dtrv.Row["GIA_TRI"] = ucColChiTieu.GiaTri;
            grChiTieu.CurrentItem = dtrv;
        }

        private void ucColChiTieuNguoiThuaKe_EditCellEnd(object sender, EventArgs e)
        {
            DataRowView dtrv = ucColChiTieuNguoiThuaKe.objDataItem as DataRowView;
            dtrv.Row["GIA_TRI"] = ucColChiTieuNguoiThuaKe.GiaTri;
            grChiTieuNguoiThuaKe.CurrentItem = dtrv;
        }

        private void ucColChiTieuNguoiBaoLanh_EditCellEnd(object sender, EventArgs e)
        {
            DataRowView dtrv = ucColChiTieuNguoiBaoLanh.objDataItem as DataRowView;
            dtrv.Row["GIA_TRI"] = ucColChiTieuNguoiBaoLanh.GiaTri;
            grChiTieuNguoiBaoLanh.CurrentItem = dtrv;
        }

        private void ucColNoiCuTru_EditCellEnd(object sender, EventArgs e)
        {
            DataRowView dtrv = ucColNoiCuTru.objDataItem as DataRowView;
            dtrv.Row["GIA_TRI"] = ucColNoiCuTru.GiaTri;
            grNoiCuTru.CurrentItem = dtrv;
        }

        private void btnAddNguoiThuaKe_Click(object sender, RoutedEventArgs e)
        {
            Window window = new Window();
            ucGiaDinhNguoiThuaKe04 uc = new ucGiaDinhNguoiThuaKe04();
            uc.Action = DatabaseConstant.Action.THEM;
            uc.DuLieuTraVe = new ucGiaDinhNguoiThuaKe04.LayDuLieu(AddToGridNguoiThuaKe);
            window.Content = uc;
            window.Title = LLanguage.SearchResourceByKey("U.KhachHang.KhachHang.Popup.ucGiaDinhNguoiThuaKe.Tittle");
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.ShowDialog();
        }

        public void AddToGridNguoiThuaKe(DataRow dr)
        {
            dsSource.Tables["GDINH_NGUOI_TKE"].Rows.Add(dr.ItemArray);

            for (int i = 0; i < dsSource.Tables["GDINH_NGUOI_TKE"].Rows.Count; i++)
            {
                dsSource.Tables["GDINH_NGUOI_TKE"].Rows[i]["STT"] = i + 1;
            }

            grNguoiThuaKe.DataContext = dsSource.Tables["GDINH_NGUOI_TKE"].DefaultView;
        }

        private void btnModifyNguoiThuaKe_Click(object sender, RoutedEventArgs e)
        {
            if (grNguoiThuaKe == null)
            {
                LMessage.ShowMessage("M.DungChung.ChuaChonBanGhi", LMessage.MessageBoxType.Warning);
                return;
            }

            DataRowView drx = (DataRowView)grNguoiThuaKe.SelectedItem;
            if (drx == null)
            {
                LMessage.ShowMessage("M.DungChung.ChuaChonBanGhi", LMessage.MessageBoxType.Warning);
                return;
            }

            if (grNguoiThuaKe.SelectedItems.Count > 1)
            {
                LMessage.ShowMessage("M.DungChung.KhongDuocChonNhieu", LMessage.MessageBoxType.Warning);
                return;
            }
            else
            {
                Window window = new Window();
                ucGiaDinhNguoiThuaKe04 uc = new ucGiaDinhNguoiThuaKe04();
                DataRowView dr = (DataRowView)grNguoiThuaKe.SelectedItem;
                int i = Convert.ToInt32(dr["STT"]);
                uc.drSource = dsSource.Tables["GDINH_NGUOI_TKE"].Rows[i - 1];
                uc.Action = DatabaseConstant.Action.SUA;
                uc.DuLieuTraVe = new ucGiaDinhNguoiThuaKe04.LayDuLieu(EditToGridNguoiThuaKe);
                window.Content = uc;
                window.Title = LLanguage.SearchResourceByKey("U.KhachHang.KhachHang.Popup.ucGiaDinhNguoiThuaKe.Tittle");
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                window.ShowDialog();
            }
        }

        public void EditToGridNguoiThuaKe(DataRow dr)
        {
            int i = Convert.ToInt32(dr["STT"]) - 1;

            if (i >= 0)
            {
                dsSource.Tables["GDINH_NGUOI_TKE"].Rows[i]["VAI_TRO"] = dr["VAI_TRO"];
                dsSource.Tables["GDINH_NGUOI_TKE"].Rows[i]["HO_TEN"] = dr["HO_TEN"];
                dsSource.Tables["GDINH_NGUOI_TKE"].Rows[i]["NGAY_SINH"] = dr["NGAY_SINH"];
                dsSource.Tables["GDINH_NGUOI_TKE"].Rows[i]["GIOI_TINH"] = dr["GIOI_TINH"];
                dsSource.Tables["GDINH_NGUOI_TKE"].Rows[i]["DAN_TOC"] = dr["DAN_TOC"];
                dsSource.Tables["GDINH_NGUOI_TKE"].Rows[i]["QH_VOI_TVIEN"] = dr["QH_VOI_TVIEN"];
                dsSource.Tables["GDINH_NGUOI_TKE"].Rows[i]["TDO_HVAN"] = dr["TDO_HVAN"];
                dsSource.Tables["GDINH_NGUOI_TKE"].Rows[i]["NGHE_NGHIEP"] = dr["NGHE_NGHIEP"];
                dsSource.Tables["GDINH_NGUOI_TKE"].Rows[i]["SUC_KHOE"] = dr["SUC_KHOE"];
                dsSource.Tables["GDINH_NGUOI_TKE"].Rows[i]["SO_CMND"] = dr["SO_CMND"];
                dsSource.Tables["GDINH_NGUOI_TKE"].Rows[i]["NGAY_CAP"] = dr["NGAY_CAP"];
                dsSource.Tables["GDINH_NGUOI_TKE"].Rows[i]["NOI_CAP"] = dr["NOI_CAP"];
                dsSource.Tables["GDINH_NGUOI_TKE"].Rows[i]["CUNG_HKHAU_TVIEN"] = dr["CUNG_HKHAU_TVIEN"];
                dsSource.Tables["GDINH_NGUOI_TKE"].Rows[i]["SO_HKHAU"] = dr["SO_HKHAU"];
                dsSource.Tables["GDINH_NGUOI_TKE"].Rows[i]["SDT"] = dr["SDT"];
                dsSource.Tables["GDINH_NGUOI_TKE"].Rows[i]["DIA_CHI"] = dr["DIA_CHI"];
                dsSource.Tables["GDINH_NGUOI_TKE"].Rows[i]["GHI_CHU"] = dr["GHI_CHU"];
                dsSource.Tables["GDINH_NGUOI_TKE"].Rows[i]["TEN_BO"] = dr["TEN_BO"];

                dsSource.Tables["GDINH_NGUOI_TKE"].Rows[i]["TEN_BAN_DIA"] = dr["TEN_BAN_DIA"];
                dsSource.Tables["GDINH_NGUOI_TKE"].Rows[i]["EMAIL"] = dr["EMAIL"];
                dsSource.Tables["GDINH_NGUOI_TKE"].Rows[i]["DIA_CHI_NOI_CAP"] = dr["DIA_CHI_NOI_CAP"];

            }

            for (int j = 0; j < dsSource.Tables["GDINH_NGUOI_TKE"].Rows.Count; j++)
            {
                dsSource.Tables["GDINH_NGUOI_TKE"].Rows[j]["STT"] = j + 1;
            }

            grNguoiThuaKe.DataContext = dsSource.Tables["GDINH_NGUOI_TKE"].DefaultView;
        }

        private void btnDeleteNguoiThuaKe_Click(object sender, RoutedEventArgs e)
        {
            List<int> lstSTT = new List<int>();
            for (int i = 0; i < grNguoiThuaKe.SelectedItems.Count; i++)
            {
                DataRowView dr = (DataRowView)grNguoiThuaKe.SelectedItems[i];
                lstSTT.Add(Convert.ToInt32(dr["STT"]));
            }
            lstSTT.SortByDesc();
            foreach (int stt in lstSTT)
                dsSource.Tables["GDINH_NGUOI_TKE"].Rows.RemoveAt(stt - 1);

            for (int i = 0; i < dsSource.Tables["GDINH_NGUOI_TKE"].Rows.Count; i++)
            {
                dsSource.Tables["GDINH_NGUOI_TKE"].Rows[i]["STT"] = i + 1;
            }

            grNguoiThuaKe.DataContext = dsSource.Tables["GDINH_NGUOI_TKE"].DefaultView;
        }       
        #endregion

        #region Xử lý nghiệp vụ
        private void GetFormData(ref KH_THONG_TIN_KHAO_SAT obj, string sTrangThaiNVu)
        {
            try
            {
                obj = new KH_THONG_TIN_KHAO_SAT();

                obj.ID = id;
                obj.LOAI_DXVV = loaiDXVV;
                obj.MA_DXVV = txtDonXinVayVon.Text;
                obj.ID_KHANG = idKhachHang;
                obj.MA_KHANG = maKhachHang;

                obj.MA_DVI = ClientInformation.MaDonVi;
                obj.MA_DVI_GDICH = ClientInformation.MaDonViGiaoDich;
                obj.TTHAI_NVU = sTrangThaiNVu;
                obj.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                obj.NGAY_NHAP = Convert.ToDateTime(raddtNgayLap.Value).ToString("yyyyMMdd");
                obj.NGUOI_NHAP = txtNguoiLap.Text;
                if (action != DatabaseConstant.Action.THEM)
                {
                    obj.NGAY_CNHAT = ClientInformation.NgayLamViecHienTai;
                    obj.NGUOI_CNHAT = ClientInformation.TenDangNhap;
                }

                VKH_THONG_TIN_KHAO_SAT objThongTinKhaoSat = new VKH_THONG_TIN_KHAO_SAT();

                //Thông tin người vay vốn
                objThongTinKhaoSat.ID = id;
                objThongTinKhaoSat.LOAI_DXVV = obj.LOAI_DXVV;
                objThongTinKhaoSat.MA_DXVV = obj.MA_DXVV;
                if (raddtNgayLapDon.Value != null)
                    objThongTinKhaoSat.NGAY_LAP_DON = Convert.ToDateTime(raddtNgayLapDon.Value).ToString("yyyyMMdd");
                objThongTinKhaoSat.ID_KHANG = idKhachHang;
                objThongTinKhaoSat.MA_KHANG = maKhachHang;
                objThongTinKhaoSat.TEN_KHANG = txtTenKhachHang.Text;
                if (cmbGioiTinh.SelectedIndex >= 0)
                    objThongTinKhaoSat.GIOI_TINH = lstSourceGioiTinh.ElementAt(cmbGioiTinh.SelectedIndex).KeywordStrings.ElementAt(0);
                objThongTinKhaoSat.TEN_CHA = txtTenCha.Text;
                objThongTinKhaoSat.SO_CMND = txtCMND.Text;
                if (raddtNgayCap.Value != null)
                    objThongTinKhaoSat.NGAY_CAP = Convert.ToDateTime(raddtNgayCap.Value).ToString("yyyyMMdd");
                objThongTinKhaoSat.NOI_CAP = txtNoiCap.Text;
                if (raddtNgaySinh.Value != null)
                    objThongTinKhaoSat.NGAY_SINH = Convert.ToDateTime(raddtNgaySinh.Value).ToString("yyyyMMdd");
                if (cmbDanToc.SelectedIndex >= 0)
                    objThongTinKhaoSat.DAN_TOC = lstSourceDanToc.ElementAt(cmbDanToc.SelectedIndex).KeywordStrings.ElementAt(0);
                objThongTinKhaoSat.SO_DIEN_THOAI = txtDienThoai.Text;
                objThongTinKhaoSat.DIA_CHI = txtDiaChi.Text;
                objThongTinKhaoSat.EMAIL = txtEmail.Text;

                //Thông tin người thừa kế
                objThongTinKhaoSat.TEN_NTKE = txtTenNguoiThuaKe.Text;
                if (raddtNgaySinhNguoiThuaKe.Value != null)
                    objThongTinKhaoSat.NGAY_SINH_NTKE = Convert.ToDateTime(raddtNgaySinhNguoiThuaKe.Value).ToString("yyyyMMdd");
                if (cmbGioiTinhNguoiThuaKe.SelectedIndex >= 0)
                    objThongTinKhaoSat.GIOI_TINH_NTKE = lstSourceGioiTinhNguoiThuaKe.ElementAt(cmbGioiTinhNguoiThuaKe.SelectedIndex).KeywordStrings.ElementAt(0);
                objThongTinKhaoSat.TEN_CHA_NTKE = txtTenChaNguoiThuaKe.Text;
                objThongTinKhaoSat.SO_CMND_NTKE = txtCMNDNguoiThuaKe.Text;
                if (raddtNgayCapNguoiThuaKe.Value != null)
                    objThongTinKhaoSat.NGAY_CAP_NTKE = Convert.ToDateTime(raddtNgayCapNguoiThuaKe.Value).ToString("yyyyMMdd");
                objThongTinKhaoSat.NOI_CAP_NTKE = txtNoiCapNguoiThuaKe.Text;
                if (cmbDanTocNguoiThuaKe.SelectedIndex >= 0)
                    objThongTinKhaoSat.DAN_TOC_NTKE = lstSourceDanTocNguoiThuaKe.ElementAt(cmbDanTocNguoiThuaKe.SelectedIndex).KeywordStrings.ElementAt(0);
                objThongTinKhaoSat.SO_DIEN_THOAI_NTKE = txtDienThoaiNguoiThuaKe.Text;
                objThongTinKhaoSat.DIA_CHI_NTKE = txtDiaChiNguoiThuaKe.Text;
                objThongTinKhaoSat.EMAIL_NTKE = txtEmailNguoiThuaKe.Text;
                objThongTinKhaoSat.SO_TIEN_DU_NTKE = Convert.ToDecimal(numSoTienDuHangThangNguoiThuaKe.Value).ToString();

                //Thông tin người bảo lãnh
                objThongTinKhaoSat.TEN_BLANH = txtTenNguoiBaoLanh.Text;
                if (raddtNgaySinhNguoiBaoLanh.Value != null)
                    objThongTinKhaoSat.NGAY_SINH_BLANH = Convert.ToDateTime(raddtNgaySinhNguoiBaoLanh.Value).ToString("yyyyMMdd");
                if (cmbGioiTinhNguoiBaoLanh.SelectedIndex >= 0)
                    objThongTinKhaoSat.GIOI_TINH_BLANH = lstSourceGioiTinhNguoiBaoLanh.ElementAt(cmbGioiTinhNguoiBaoLanh.SelectedIndex).KeywordStrings.ElementAt(0);
                objThongTinKhaoSat.TEN_CHA_BLANH = txtTenChaNguoiBaoLanh.Text;
                objThongTinKhaoSat.SO_CMND_BLANH = txtCMNDNguoiBaoLanh.Text;
                if (raddtNgayCapNguoiBaoLanh.Value != null)
                    objThongTinKhaoSat.NGAY_CAP_BLANH = Convert.ToDateTime(raddtNgayCapNguoiBaoLanh.Value).ToString("yyyyMMdd");
                objThongTinKhaoSat.NOI_CAP_BLANH = txtNoiCapNguoiBaoLanh.Text;
                if (cmbDanTocNguoiBaoLanh.SelectedIndex >= 0)
                    objThongTinKhaoSat.DAN_TOC_BLANH = lstSourceDanTocNguoiBaoLanh.ElementAt(cmbDanTocNguoiBaoLanh.SelectedIndex).KeywordStrings.ElementAt(0);
                objThongTinKhaoSat.SO_DIEN_THOAI_BLANH = txtDienThoaiNguoiBaoLanh.Text;
                objThongTinKhaoSat.DIA_CHI_BLANH = txtDiaChiNguoiBaoLanh.Text;
                objThongTinKhaoSat.EMAIL_BLANH = txtEmailNguoiBaoLanh.Text;
                objThongTinKhaoSat.SO_TIEN_DU_BLANH = Convert.ToDecimal(numSoTienDuHangThangNguoiBaoLanh.Value).ToString();
                

                //Thông tin tài sản
                if (cmbPhuongTien.SelectedIndex >=0 )
                    objThongTinKhaoSat.PTIEN_DI_LAI = lstSourcePhuongTienDiLai.ElementAt(cmbPhuongTien.SelectedIndex).KeywordStrings.ElementAt(0);
                objThongTinKhaoSat.PTIEN_GIA_TRI = Convert.ToDecimal(numGiaPhuongTienUocTinh.Value).ToString();
                objThongTinKhaoSat.TONG_TIEN_TICH_LUY = Convert.ToDecimal(numTienMatTichLuy.Value).ToString();
                objThongTinKhaoSat.TONG_TIEN_GUI_TKIEM = Convert.ToDecimal(numSoTienGuiTK.Value).ToString();                
                if (cmbNoiGuiTK.SelectedIndex >= 0)
                    objThongTinKhaoSat.NOI_GUI_TKIEM = lstSourceNoiGuiTK.ElementAt(cmbNoiGuiTK.SelectedIndex).KeywordStrings.ElementAt(0);
                objThongTinKhaoSat.TAI_SAN_KHAC = Convert.ToDecimal(numTaiSanKhac.Value).ToString();
                objThongTinKhaoSat.TONG_TAI_SAN = Convert.ToDecimal(numTongTaiSanUocTinh.Value).ToString();

                //Thông tin kiểm soát                
                objThongTinKhaoSat.TEN_BANG = "KH_KHANG_HSO";
                objThongTinKhaoSat.MA_DOI_TUONG = "VKH_THONG_TIN_KHAO_SAT";
                objThongTinKhaoSat.NGAY_DL = ClientInformation.NgayLamViecHienTai;
                objThongTinKhaoSat.MA_DVI_TAO = obj.MA_DVI_GDICH;
                objThongTinKhaoSat.TTHAI_NVU = obj.TTHAI_NVU;
                objThongTinKhaoSat.TTHAI_BGHI = obj.TTHAI_BGHI;
                objThongTinKhaoSat.NGAY_NHAP = obj.NGAY_NHAP;
                objThongTinKhaoSat.NGUOI_NHAP = obj.NGUOI_NHAP;
                objThongTinKhaoSat.NGAY_CNHAT = obj.NGAY_CNHAT;
                objThongTinKhaoSat.NGUOI_CNHAT = obj.NGUOI_CNHAT;

                obj.OBJ_VKH_THONG_TIN_KHAO_SAT = objThongTinKhaoSat;
                obj.DATA_SET = dsSource;                
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        private void SetFormData()
        {
            KhachHangProcess processKhachHang = new KhachHangProcess();
            List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
            obj = new KH_THONG_TIN_KHAO_SAT();
            try
            {
                bool ret = false;
                obj.ID = id;
                ret = processKhachHang.ThongTinKhaoSat05(DatabaseConstant.Action.LOAD, ref obj, ref listClientResponseDetail);
                if (ret == true)
                {
                    sTrangThaiNVu = obj.OBJ_VKH_THONG_TIN_KHAO_SAT.TTHAI_NVU;
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);

                    loaiDXVV = obj.OBJ_VKH_THONG_TIN_KHAO_SAT.LOAI_DXVV;
                    idKhachHang = (int)obj.OBJ_VKH_THONG_TIN_KHAO_SAT.ID_KHANG;
                    maKhachHang = obj.OBJ_VKH_THONG_TIN_KHAO_SAT.MA_KHANG;
                    txtDonXinVayVon.Text = obj.OBJ_VKH_THONG_TIN_KHAO_SAT.MA_DXVV;
                    if (LDateTime.IsDate(obj.OBJ_VKH_THONG_TIN_KHAO_SAT.NGAY_LAP_DON, "yyyyMMdd"))
                        raddtNgayLapDon.Value = LDateTime.StringToDate(obj.OBJ_VKH_THONG_TIN_KHAO_SAT.NGAY_LAP_DON, "yyyyMMdd");
                    txtTenKhachHang.Text = obj.OBJ_VKH_THONG_TIN_KHAO_SAT.TEN_KHANG;
                    cmbGioiTinh.SelectedIndex = lstSourceGioiTinh.IndexOf(lstSourceGioiTinh.FirstOrDefault(i => i.KeywordStrings.First().Equals(obj.OBJ_VKH_THONG_TIN_KHAO_SAT.GIOI_TINH)));
                    txtTenCha.Text = obj.OBJ_VKH_THONG_TIN_KHAO_SAT.TEN_CHA;
                    if (LDateTime.IsDate(obj.OBJ_VKH_THONG_TIN_KHAO_SAT.NGAY_SINH, "yyyyMMdd"))
                        raddtNgaySinh.Value = LDateTime.StringToDate(obj.OBJ_VKH_THONG_TIN_KHAO_SAT.NGAY_SINH, "yyyyMMdd");
                    txtCMND.Text = obj.OBJ_VKH_THONG_TIN_KHAO_SAT.SO_CMND;
                    if (LDateTime.IsDate(obj.OBJ_VKH_THONG_TIN_KHAO_SAT.NGAY_CAP, "yyyyMMdd"))
                        raddtNgayCap.Value = LDateTime.StringToDate(obj.OBJ_VKH_THONG_TIN_KHAO_SAT.NGAY_CAP, "yyyyMMdd");
                    txtNoiCap.Text = obj.OBJ_VKH_THONG_TIN_KHAO_SAT.NOI_CAP;
                    cmbDanToc.SelectedIndex = lstSourceDanToc.IndexOf(lstSourceDanToc.FirstOrDefault(i => i.KeywordStrings.First().Equals(obj.OBJ_VKH_THONG_TIN_KHAO_SAT.DAN_TOC)));
                    txtDienThoai.Text = obj.OBJ_VKH_THONG_TIN_KHAO_SAT.SO_DIEN_THOAI;
                    txtDiaChi.Text = obj.OBJ_VKH_THONG_TIN_KHAO_SAT.DIA_CHI;
                    txtEmail.Text = obj.OBJ_VKH_THONG_TIN_KHAO_SAT.EMAIL;

                    //Thông tin người thừa kế
                    txtTenNguoiThuaKe.Text = obj.OBJ_VKH_THONG_TIN_KHAO_SAT.TEN_NTKE;
                    if (LDateTime.IsDate(obj.OBJ_VKH_THONG_TIN_KHAO_SAT.NGAY_SINH_NTKE, "yyyyMMdd"))
                        raddtNgaySinhNguoiThuaKe.Value = LDateTime.StringToDate(obj.OBJ_VKH_THONG_TIN_KHAO_SAT.NGAY_SINH_NTKE, "yyyyMMdd");
                    cmbGioiTinhNguoiThuaKe.SelectedIndex = lstSourceGioiTinhNguoiThuaKe.IndexOf(lstSourceGioiTinhNguoiThuaKe.FirstOrDefault(i => i.KeywordStrings.First().Equals(obj.OBJ_VKH_THONG_TIN_KHAO_SAT.GIOI_TINH_NTKE)));
                    txtTenChaNguoiThuaKe.Text = obj.OBJ_VKH_THONG_TIN_KHAO_SAT.TEN_CHA_NTKE;
                    txtCMNDNguoiThuaKe.Text = obj.OBJ_VKH_THONG_TIN_KHAO_SAT.SO_CMND_NTKE;
                    if (LDateTime.IsDate(obj.OBJ_VKH_THONG_TIN_KHAO_SAT.NGAY_CAP_NTKE, "yyyyMMdd"))
                        raddtNgayCapNguoiThuaKe.Value = LDateTime.StringToDate(obj.OBJ_VKH_THONG_TIN_KHAO_SAT.NGAY_CAP_NTKE, "yyyyMMdd");
                    txtNoiCapNguoiThuaKe.Text = obj.OBJ_VKH_THONG_TIN_KHAO_SAT.NOI_CAP_NTKE;
                    cmbDanTocNguoiThuaKe.SelectedIndex = lstSourceDanTocNguoiThuaKe.IndexOf(lstSourceDanTocNguoiThuaKe.FirstOrDefault(i => i.KeywordStrings.First().Equals(obj.OBJ_VKH_THONG_TIN_KHAO_SAT.DAN_TOC_NTKE)));
                    txtDienThoaiNguoiThuaKe.Text = obj.OBJ_VKH_THONG_TIN_KHAO_SAT.SO_DIEN_THOAI_NTKE;
                    txtDiaChiNguoiThuaKe.Text = obj.OBJ_VKH_THONG_TIN_KHAO_SAT.DIA_CHI_NTKE;
                    txtEmailNguoiThuaKe.Text = obj.OBJ_VKH_THONG_TIN_KHAO_SAT.EMAIL_NTKE;
                    numSoTienDuHangThangNguoiThuaKe.Value = Convert.ToDouble(obj.OBJ_VKH_THONG_TIN_KHAO_SAT.SO_TIEN_DU_NTKE);

                    //Thông tin người bảo lãnh
                    txtTenNguoiBaoLanh.Text = obj.OBJ_VKH_THONG_TIN_KHAO_SAT.TEN_BLANH;
                    if (LDateTime.IsDate(obj.OBJ_VKH_THONG_TIN_KHAO_SAT.NGAY_SINH_BLANH, "yyyyMMdd"))
                        raddtNgaySinhNguoiBaoLanh.Value = LDateTime.StringToDate(obj.OBJ_VKH_THONG_TIN_KHAO_SAT.NGAY_SINH_BLANH, "yyyyMMdd");
                    cmbGioiTinhNguoiBaoLanh.SelectedIndex = lstSourceGioiTinhNguoiBaoLanh.IndexOf(lstSourceGioiTinhNguoiBaoLanh.FirstOrDefault(i => i.KeywordStrings.First().Equals(obj.OBJ_VKH_THONG_TIN_KHAO_SAT.GIOI_TINH_BLANH)));
                    txtTenChaNguoiBaoLanh.Text = obj.OBJ_VKH_THONG_TIN_KHAO_SAT.TEN_CHA_BLANH;
                    txtCMNDNguoiBaoLanh.Text = obj.OBJ_VKH_THONG_TIN_KHAO_SAT.SO_CMND_BLANH;
                    if (LDateTime.IsDate(obj.OBJ_VKH_THONG_TIN_KHAO_SAT.NGAY_CAP_BLANH, "yyyyMMdd"))
                        raddtNgayCapNguoiBaoLanh.Value = LDateTime.StringToDate(obj.OBJ_VKH_THONG_TIN_KHAO_SAT.NGAY_CAP_BLANH, "yyyyMMdd");
                    txtNoiCapNguoiBaoLanh.Text = obj.OBJ_VKH_THONG_TIN_KHAO_SAT.NOI_CAP_BLANH;
                    cmbDanTocNguoiBaoLanh.SelectedIndex = lstSourceDanTocNguoiBaoLanh.IndexOf(lstSourceDanTocNguoiBaoLanh.FirstOrDefault(i => i.KeywordStrings.First().Equals(obj.OBJ_VKH_THONG_TIN_KHAO_SAT.DAN_TOC_BLANH)));
                    txtDienThoaiNguoiBaoLanh.Text = obj.OBJ_VKH_THONG_TIN_KHAO_SAT.SO_DIEN_THOAI_BLANH;
                    txtDiaChiNguoiBaoLanh.Text = obj.OBJ_VKH_THONG_TIN_KHAO_SAT.DIA_CHI_BLANH;
                    txtEmailNguoiBaoLanh.Text = obj.OBJ_VKH_THONG_TIN_KHAO_SAT.EMAIL_BLANH;
                    numSoTienDuHangThangNguoiBaoLanh.Value = Convert.ToDouble(obj.OBJ_VKH_THONG_TIN_KHAO_SAT.SO_TIEN_DU_BLANH);

                    //Thông tin tài sản                    
                    cmbPhuongTien.SelectedIndex = lstSourcePhuongTienDiLai.IndexOf(lstSourcePhuongTienDiLai.FirstOrDefault(i => i.KeywordStrings.First().Equals(obj.OBJ_VKH_THONG_TIN_KHAO_SAT.PTIEN_DI_LAI)));
                    numGiaPhuongTienUocTinh.Value = Convert.ToDouble(obj.OBJ_VKH_THONG_TIN_KHAO_SAT.PTIEN_GIA_TRI);
                    numTienMatTichLuy.Value = Convert.ToDouble(obj.OBJ_VKH_THONG_TIN_KHAO_SAT.TONG_TIEN_TICH_LUY);
                    numSoTienGuiTK.Value = Convert.ToDouble(obj.OBJ_VKH_THONG_TIN_KHAO_SAT.TONG_TIEN_GUI_TKIEM);                    
                    cmbNoiGuiTK.SelectedIndex = lstSourceNoiGuiTK.IndexOf(lstSourceNoiGuiTK.FirstOrDefault(i => i.KeywordStrings.First().Equals(obj.OBJ_VKH_THONG_TIN_KHAO_SAT.NOI_GUI_TKIEM)));
                    numTaiSanKhac.Value = Convert.ToDouble(obj.OBJ_VKH_THONG_TIN_KHAO_SAT.TAI_SAN_KHAC);
                    numTongTaiSanUocTinh.Value = Convert.ToDouble(obj.OBJ_VKH_THONG_TIN_KHAO_SAT.TONG_TAI_SAN);                    

                    #region DATA_SET
                    dsSource = obj.DATA_SET;
                    foreach (DataRow dr in dsSource.Tables["CHI_TIEU_NGUOI_VAY_VON"].Rows)
                        dr["TEN"] = LLanguage.SearchResourceByKey(dr["MA_NNGU"].ToString());

                    foreach (DataRow dr in dsSource.Tables["CHI_TIEU_NGUOI_THUA_KE"].Rows)
                        dr["TEN"] = LLanguage.SearchResourceByKey(dr["MA_NNGU"].ToString());

                    foreach (DataRow dr in dsSource.Tables["CHI_TIEU_NGUOI_BAO_LANH"].Rows)
                        dr["TEN"] = LLanguage.SearchResourceByKey(dr["MA_NNGU"].ToString());

                    foreach (DataRow dr in dsSource.Tables["THU_NHAP"].Rows)
                        dr["TEN"] = LLanguage.SearchResourceByKey(dr["MA_NNGU"].ToString());

                    foreach (DataRow dr in dsSource.Tables["CHI_PHI"].Rows)
                        dr["TEN"] = LLanguage.SearchResourceByKey(dr["MA_NNGU"].ToString());

                    foreach (DataRow dr in dsSource.Tables["NOI_CU_TRU"].Rows)
                        dr["TEN"] = LLanguage.SearchResourceByKey(dr["MA_NNGU"].ToString());

                    foreach (DataRow dr in dsSource.Tables["SU_DUNG_DAT"].Rows)
                        dr["TEN"] = LLanguage.SearchResourceByKey(dr["MA_NNGU"].ToString());

                    foreach (DataRow dr in dsSource.Tables["TINH_HINH_TDUNG"].Rows)
                        dr["TEN"] = LLanguage.SearchResourceByKey(dr["MA_NNGU"].ToString());

                    foreach (DataRow dr in dsSource.Tables["GDINH_NGUOI_TKE"].Rows)
                    {
                        if (dr["GIOI_TINH1"] != null)
                            dr["GIOI_TINH1"] = LLanguage.SearchResourceByKey(dr["GIOI_TINH1"].ToString());

                        if (dr["TEN_VAI_TRO"] != null)
                            dr["TEN_VAI_TRO"] = LLanguage.SearchResourceByKey(dr["TEN_VAI_TRO"].ToString());
                    }

                    grChiTieu.DataContext = dsSource.Tables["CHI_TIEU_NGUOI_VAY_VON"].DefaultView;
                    grChiTieuNguoiThuaKe.DataContext = dsSource.Tables["CHI_TIEU_NGUOI_THUA_KE"].DefaultView;
                    grChiTieuNguoiBaoLanh.DataContext = dsSource.Tables["CHI_TIEU_NGUOI_BAO_LANH"].DefaultView;
                    grNguoiThuaKe.DataContext = dsSource.Tables["GDINH_NGUOI_TKE"].DefaultView;
                    grThuNhap.DataContext = dsSource.Tables["THU_NHAP"].DefaultView;
                    grChiPhi.DataContext = dsSource.Tables["CHI_PHI"].DefaultView;
                    grNoiCuTru.DataContext = dsSource.Tables["NOI_CU_TRU"].DefaultView;
                    grCongCuSuDungDat.DataContext = dsSource.Tables["SU_DUNG_DAT"].DefaultView;
                    grTinhHinhTinDung.DataContext = dsSource.Tables["TINH_HINH_TDUNG"].DefaultView;
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
                processKhachHang = null;
                listClientResponseDetail = null;
            }
        }

        private void SetGridData()
        {
           KhachHangProcess processKhachHang = new KhachHangProcess();
           dsSource = processKhachHang.getViewThongTinKhaoSat(id);
            if (dsSource != null && dsSource.Tables.Count > 0)
            {
                foreach (DataRow dr in dsSource.Tables["CHI_TIEU_NGUOI_VAY_VON"].Rows)
                    dr["TEN"] = LLanguage.SearchResourceByKey(dr["MA_NNGU"].ToString());

                foreach (DataRow dr in dsSource.Tables["CHI_TIEU_NGUOI_THUA_KE"].Rows)
                    dr["TEN"] = LLanguage.SearchResourceByKey(dr["MA_NNGU"].ToString());

                foreach (DataRow dr in dsSource.Tables["CHI_TIEU_NGUOI_BAO_LANH"].Rows)
                    dr["TEN"] = LLanguage.SearchResourceByKey(dr["MA_NNGU"].ToString());

                foreach (DataRow dr in dsSource.Tables["THU_NHAP"].Rows)
                    dr["TEN"] = LLanguage.SearchResourceByKey(dr["MA_NNGU"].ToString());

                foreach (DataRow dr in dsSource.Tables["CHI_PHI"].Rows)
                    dr["TEN"] = LLanguage.SearchResourceByKey(dr["MA_NNGU"].ToString());

                foreach (DataRow dr in dsSource.Tables["NOI_CU_TRU"].Rows)
                    dr["TEN"] = LLanguage.SearchResourceByKey(dr["MA_NNGU"].ToString());

                foreach (DataRow dr in dsSource.Tables["SU_DUNG_DAT"].Rows)
                    dr["TEN"] = LLanguage.SearchResourceByKey(dr["MA_NNGU"].ToString());

                foreach (DataRow dr in dsSource.Tables["TINH_HINH_TDUNG"].Rows)
                    dr["TEN"] = LLanguage.SearchResourceByKey(dr["MA_NNGU"].ToString());

                grChiTieu.DataContext = dsSource.Tables["CHI_TIEU_NGUOI_VAY_VON"].DefaultView;
                grChiTieuNguoiThuaKe.DataContext = dsSource.Tables["CHI_TIEU_NGUOI_THUA_KE"].DefaultView;
                grChiTieuNguoiBaoLanh.DataContext = dsSource.Tables["CHI_TIEU_NGUOI_BAO_LANH"].DefaultView;
                grThuNhap.DataContext = dsSource.Tables["THU_NHAP"].DefaultView;
                grChiPhi.DataContext = dsSource.Tables["CHI_PHI"].DefaultView;
                grNoiCuTru.DataContext = dsSource.Tables["NOI_CU_TRU"].DefaultView;
                grCongCuSuDungDat.DataContext = dsSource.Tables["SU_DUNG_DAT"].DefaultView;
                grTinhHinhTinDung.DataContext = dsSource.Tables["TINH_HINH_TDUNG"].DefaultView;
            }
        }

        private void ResetForm()
        {
            

        }        

        private bool Validation()
        {
            try
            {
                

                return true;
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);                
                return false;
            }
        }

        private void SetVisibledControl()
        {
            
        }

        private void SetEnabledControls()
        {
            #region Thêm
            if (action == DatabaseConstant.Action.THEM)
            {
                //Thông tin người vay vốn
                gbThongTinNguoiVayVon.IsEnabled = true;
                grChiTieu.IsReadOnly = false;

                //Thông tin người thừa kế
                gbThongTinNguoiThuaKe.IsEnabled = true;
                grChiTieuNguoiThuaKe.IsReadOnly = false;
                numSoTienDuHangThangNguoiThuaKe.IsEnabled = true;

                //Thông tin người bảo lãnh
                gbThongTinNguoiBaoLanh.IsEnabled = true;
                grChiTieuNguoiBaoLanh.IsReadOnly = false;
                numSoTienDuHangThangNguoiBaoLanh.IsEnabled = true;

                //Thông tin gia đình và người thừa kế
                btnAddNguoiThuaKe.IsEnabled = true;
                btnModifyNguoiThuaKe.IsEnabled = true;
                btnDeleteNguoiThuaKe.IsEnabled = true;

                //Thông tin thu nhập chi phí
                grThuNhap.IsReadOnly = false;
                grChiPhi.IsReadOnly = false;

                //Thông tin tài sản
                grNoiCuTru.IsReadOnly = false;
                numGiaTriNhaUocTinh.IsEnabled = true;
                grCongCuSuDungDat.IsReadOnly = false;
                gbThongTinTaiSan.IsEnabled = true;

                //Tình hình tín dụng
                grTinhHinhTinDung.IsReadOnly = false;
            }
            #endregion

            #region Sửa
            else if (action == DatabaseConstant.Action.SUA)
            {
                //Thông tin người vay vốn
                gbThongTinNguoiVayVon.IsEnabled = true;
                grChiTieu.IsReadOnly = false;

                //Thông tin người thừa kế
                gbThongTinNguoiThuaKe.IsEnabled = true;
                grChiTieuNguoiThuaKe.IsReadOnly = false;
                numSoTienDuHangThangNguoiThuaKe.IsEnabled = true;

                //Thông tin người bảo lãnh
                gbThongTinNguoiBaoLanh.IsEnabled = true;
                grChiTieuNguoiBaoLanh.IsReadOnly = false;
                numSoTienDuHangThangNguoiBaoLanh.IsEnabled = true;

                //Thông tin gia đình và người thừa kế
                btnAddNguoiThuaKe.IsEnabled = true;
                btnModifyNguoiThuaKe.IsEnabled = true;
                btnDeleteNguoiThuaKe.IsEnabled = true;

                //Thông tin thu nhập chi phí
                grThuNhap.IsReadOnly = false;
                grChiPhi.IsReadOnly = false;

                //Thông tin tài sản
                grNoiCuTru.IsReadOnly = false;
                numGiaTriNhaUocTinh.IsEnabled = true;
                grCongCuSuDungDat.IsReadOnly = false;
                gbThongTinTaiSan.IsEnabled = true;

                //Tình hình tín dụng
                grTinhHinhTinDung.IsReadOnly = false;
            }
            #endregion

            #region Xem ~ Mặc định
            else
            {
                //Thông tin người vay vốn
                gbThongTinNguoiVayVon.IsEnabled = false;
                grChiTieu.IsReadOnly = true;

                //Thông tin người thừa kế
                gbThongTinNguoiThuaKe.IsEnabled = false;
                grChiTieuNguoiThuaKe.IsReadOnly = true;
                numSoTienDuHangThangNguoiThuaKe.IsEnabled = false;

                //Thông tin người bảo lãnh
                gbThongTinNguoiBaoLanh.IsEnabled = false;
                grChiTieuNguoiBaoLanh.IsReadOnly = true;
                numSoTienDuHangThangNguoiBaoLanh.IsEnabled = false;

                //Thông tin gia đình và người thừa kế
                btnAddNguoiThuaKe.IsEnabled = false;
                btnModifyNguoiThuaKe.IsEnabled = false;
                btnDeleteNguoiThuaKe.IsEnabled = false;

                //Thông tin thu nhập chi phí
                grThuNhap.IsReadOnly = true;
                grChiPhi.IsReadOnly = true;

                //Thông tin tài sản
                grNoiCuTru.IsReadOnly = true;
                numGiaTriNhaUocTinh.IsEnabled = false;
                grCongCuSuDungDat.IsReadOnly = true;
                gbThongTinTaiSan.IsEnabled = false;

                //Tình hình tín dụng
                grTinhHinhTinDung.IsReadOnly = true;
            }
            #endregion
        }


        public void OnHold()
        {

        }

        public void OnSave()
        {
            try
            {
                if (!Validation()) return;

                string trangThai = "";
                trangThai = CommonFunction.LayTrangThaiBanGhi(DatabaseConstant.Action.LUU, BusinessConstant.layTrangThaiNghiepVu(sTrangThaiNVu));
                if(trangThai.Equals(BusinessConstant.TrangThaiNghiepVu.SUA_SAU_DUYET.layGiaTri()))
                    trangThai = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();

                obj = new KH_THONG_TIN_KHAO_SAT();

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
            SetVisibledControl();
            BeforeViewFromDetail();
        }


        public void BeforeAddNew()
        {
            action = DatabaseConstant.Action.THEM;
            SetGridData();
            ResetForm();
            SetVisibledControl();
            SetEnabledControls();
            CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain, function);
        }

        public void OnAddNew(KH_THONG_TIN_KHAO_SAT obj)
        {

            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                KhachHangProcess processKhachHang = new KhachHangProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = false;

                ret = processKhachHang.ThongTinKhaoSat05(DatabaseConstant.Action.THEM, ref obj, ref listClientResponseDetail);
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

        public void AfterAddNew(bool ret, KH_THONG_TIN_KHAO_SAT obj, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (ret)
                {
                    LMessage.ShowMessage("M.DungChung.ThemThanhCong", LMessage.MessageBoxType.Information);

                    id = obj.ID;                    
                    sTrangThaiNVu = obj.TTHAI_NVU;
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);
                    txtTrangThaiBanGhi.Text = BusinessConstant.layNgonNguSuDung(obj.TTHAI_BGHI);

                    BeforeViewFromDetail();
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

                bool ret = process.LockData(DatabaseConstant.Module.KHTV,
                    DatabaseConstant.Function.KH_THANH_VIEN,
                    DatabaseConstant.Table.KH_KHANG_HSO,
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
            action = DatabaseConstant.Action.SUA;
            SetFormData();
            SetVisibledControl();
            SetEnabledControls();
            CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain, function);
        }

        public void OnModify(KH_THONG_TIN_KHAO_SAT obj)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                KhachHangProcess processKhachHang = new KhachHangProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = false;

                ret = processKhachHang.ThongTinKhaoSat05(DatabaseConstant.Action.SUA, ref obj, ref listClientResponseDetail);
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

        public void AfterModify(bool ret, KH_THONG_TIN_KHAO_SAT obj, List<ClientResponseDetail> listClientResponseDetail)
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

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.KHTV,
                    DatabaseConstant.Function.KH_THANH_VIEN,
                    DatabaseConstant.Table.KH_KHANG_HSO,
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

                    bool retLockData = process.LockData(DatabaseConstant.Module.KHTV,
                        DatabaseConstant.Function.KH_THANH_VIEN,
                        DatabaseConstant.Table.KH_KHANG_HSO,
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

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.KHTV,
                    DatabaseConstant.Function.KH_THANH_VIEN,
                    DatabaseConstant.Table.KH_KHANG_HSO,
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
            KhachHangProcess processKhachHang = new KhachHangProcess();
            try
            {
                bool ret = false;
                obj.ID = id;
                ret = processKhachHang.ThongTinKhaoSat05(action, ref obj, ref listClientResponseDetail);
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
                processKhachHang = null;
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

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.KHTV,
                    DatabaseConstant.Function.KH_THANH_VIEN,
                    DatabaseConstant.Table.KH_KHANG_HSO,
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

                    bool retLockData = process.LockData(DatabaseConstant.Module.KHTV,
                        DatabaseConstant.Function.KH_THANH_VIEN,
                        DatabaseConstant.Table.KH_KHANG_HSO,
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
            KhachHangProcess processKhachHang = new KhachHangProcess();
            try
            {
                bool ret = false;
                obj.ID = id;
                ret = processKhachHang.ThongTinKhaoSat05(action, ref obj, ref listClientResponseDetail);
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
                processKhachHang = null;
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

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.KHTV,
                    DatabaseConstant.Function.KH_THANH_VIEN,
                    DatabaseConstant.Table.KH_KHANG_HSO,
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

                    bool retLockData = process.LockData(DatabaseConstant.Module.KHTV,
                        DatabaseConstant.Function.KH_THANH_VIEN,
                        DatabaseConstant.Table.KH_KHANG_HSO,
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
            KhachHangProcess processKhachHang = new KhachHangProcess();
            try
            {
                bool ret = false;
                obj.ID = id;
                ret = processKhachHang.ThongTinKhaoSat05(action, ref obj, ref listClientResponseDetail);
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
                processKhachHang = null;
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

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.KHTV,
                    DatabaseConstant.Function.KH_THANH_VIEN,
                    DatabaseConstant.Table.KH_KHANG_HSO,
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

                    bool retLockData = process.LockData(DatabaseConstant.Module.KHTV,
                        DatabaseConstant.Function.KH_THANH_VIEN,
                        DatabaseConstant.Table.KH_KHANG_HSO,
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
            KhachHangProcess processKhachHang = new KhachHangProcess();
            try
            {
                bool ret = false;
                obj.ID = id;
                ret = processKhachHang.ThongTinKhaoSat05(action, ref obj, ref listClientResponseDetail);
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
                processKhachHang = null;
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

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.KHTV,
                    DatabaseConstant.Function.KH_THANH_VIEN,
                    DatabaseConstant.Table.KH_KHANG_HSO,
                    DatabaseConstant.Action.TU_CHOI_DUYET,
                    listLockId);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }


        private void OnPreviewKhaoSat()
        {
            // Cảnh báo khi không có dữ liệu
            if (LObject.IsNullOrEmpty(obj) || LObject.IsNullOrEmpty(txtDonXinVayVon.Text) || LObject.IsNullOrEmpty(txtDonXinVayVon.Text) || sTrangThaiNVu == "")
            {
                LMessage.ShowMessage("M.ResponseMessage.Common.KhongTonTaiDuLieu", LMessage.MessageBoxType.Warning);
                return;
            }
            else
            {

                VanHanhGiaoDich xemBaoCao = new VanHanhGiaoDich();
                List<ThamSoBaoCao> lstThamSo = new List<ThamSoBaoCao>();

                lstThamSo.Add(new ThamSoBaoCao("@LoaiDXVV", loaiDXVV, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                lstThamSo.Add(new ThamSoBaoCao("@MaDXVV", txtDonXinVayVon.Text, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                lstThamSo.Add(new ThamSoBaoCao("@MaKhachHang", maKhachHang, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                lstThamSo.Add(new ThamSoBaoCao("@IdKhachHang", idKhachHang.ToString(), ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                lstThamSo.Add(new ThamSoBaoCao("@NgayBaoCao", ClientInformation.NgayLamViecHienTai, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                lstThamSo.Add(new ThamSoBaoCao("MaNgonNgu", ClientInformation.NgonNgu, ApplicationConstant.LoaiThamSoBaoCao.LANG.layGiaTri()));
                lstThamSo.Add(new ThamSoBaoCao("MaDinhDang", ApplicationConstant.LoaiDinhDangBaoCao.WORD.layGiaTri(), ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri()));
                lstThamSo.Add(new ThamSoBaoCao("@MaChiNhanh", ClientInformation.MaDonVi, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                lstThamSo.Add(new ThamSoBaoCao("@MaPhongGD", ClientInformation.MaDonViGiaoDich, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));

                lstThamSo.Add(new ThamSoBaoCao("P_TEN_KHACH_HANG", txtTenKhachHang.Text, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                lstThamSo.Add(new ThamSoBaoCao("P_TEN_THUONG_GOI", txtTenKhachHang.Text, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                lstThamSo.Add(new ThamSoBaoCao("P_NGAY_SINH", raddtNgaySinh.Text, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                lstThamSo.Add(new ThamSoBaoCao("P_DIA_CHI", txtDiaChi.Text, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                lstThamSo.Add(new ThamSoBaoCao("P_SO_CMND", txtCMND.Text, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                lstThamSo.Add(new ThamSoBaoCao("P_NGAY_CAP", raddtNgayCap.Text, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                lstThamSo.Add(new ThamSoBaoCao("P_NOI_CAP", txtNoiCap.Text, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));

                string maBaoCao = DatabaseConstant.LayMaBaoCaoQuangBinh(DatabaseConstant.DanhSachBaoCaoQuangBinh.KHTV_BAN_KHAO_SAT_HOAN_CANH_KTXH);
                xemBaoCao.LayDuLieu(maBaoCao, lstThamSo);
            }
        }
        #endregion

    }
}
