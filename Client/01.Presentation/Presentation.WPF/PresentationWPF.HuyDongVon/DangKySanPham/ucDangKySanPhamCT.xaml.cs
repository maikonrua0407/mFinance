using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Threading;
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
using Telerik.Windows.Controls.GridView;
using Utilities.Common;
using Presentation.Process;
using Presentation.Process.Common;
using PresentationWPF.CustomControl;
using Presentation.Process.PopupServiceRef;
using Presentation.Process.HuyDongVonServiceRef;
using Presentation.Process.LaiSuatServiceRef;
using Presentation.Process.KhachHangServiceRef;

namespace PresentationWPF.HuyDongVon.DangKySanPham
{
    /// <summary>
    /// Interaction logic for ucDangKySanPhamCT.xaml
    /// </summary>
    public partial class ucDangKySanPhamCT : UserControl
    {
        #region Khai bao

        // các thao tác trên Form: thêm, sửa, xem... (để mặc định là thêm)
        private DatabaseConstant.Action action = DatabaseConstant.Action.THEM;
        public DatabaseConstant.Action Action
        {
            get { return action; }
            set { action = value; }
        }

        public event EventHandler OnSavingCompleted;

        // Trạng thái nghiệp vụ (để mặc định là "")
        private string sTrangThaiNVu = "";        

        private int id;
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        private string maSanPham;
        public string MaSanPham
        {
            get { return maSanPham; }
            set { maSanPham = value; }
        }

        private int idLaiSuat = 0;
        private int idCoSoTinhLai = 0;


        private BL_SAN_PHAM obj;

        private List<BL_SAN_PHAM_CT> lstObjCT;

        private List<KT_PHAN_HE_PLOAI> lstPHPL;

        private DataSet dsTheoKhoang;
        private DataSet dsPhi = null;
        private DataSet dsTaiKhoanHachToan;

        List<DataRow> lstPopup = new List<DataRow>();
        public void LayDuLieuTuPopup(List<DataRow> lst)
        {
            lstPopup = lst;
        }

        List<AutoCompleteEntry> lstSourceTanSuat = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceNhomSP = new List<AutoCompleteEntry>();        
        List<AutoCompleteEntry> lstSourceHinhThucTLaiCoDinh = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceHinhThucTLai = new List<AutoCompleteEntry>();

        List<AutoCompleteEntry> lstSourceTanSuatTraLai = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceKyHan = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceDinhKyDGiaLai = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceLoaiTien = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceLoaiLS = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceCoSoTLai = new List<AutoCompleteEntry>();

        List<AutoCompleteEntry> lstSourceRutGocTyLeTuongDoi = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceRutGocTyLeTyLe = new List<AutoCompleteEntry>();

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
        public ucDangKySanPhamCT()
        {
            InitializeComponent();

            Dispatcher.CurrentDispatcher.DelayInvoke("DuyetQuyenTinhNang", () =>
            {
                DuyetQuyenTinhNang();
            }, TimeSpan.FromSeconds(0));

            Dispatcher.CurrentDispatcher.DelayInvoke("BindShortkey", () =>
            {
                BindShortkey();
            }, TimeSpan.FromSeconds(0));

            Dispatcher.CurrentDispatcher.DelayInvoke("InitEventHandler", () =>
            {
                InitEventHandler();
            }, TimeSpan.FromSeconds(0));


            Dispatcher.CurrentDispatcher.DelayInvoke("LoadComboBox", () =>
            {
                LoadComboBox();
            }, TimeSpan.FromSeconds(0));

            Dispatcher.CurrentDispatcher.DelayInvoke("KhoiTaoGridTheoKhoang", () =>
            {
                KhoiTaoGridTheoKhoang();
            }, TimeSpan.FromSeconds(0));

            Dispatcher.CurrentDispatcher.DelayInvoke("KhoiTaoGridPhi", () =>
            {
                KhoiTaoGridPhi();
            }, TimeSpan.FromSeconds(0));

            Dispatcher.CurrentDispatcher.DelayInvoke("HideControl", () =>
            {
                HideControl();
            }, TimeSpan.FromSeconds(0));

            cmbNhomSP.Focus();
        }

        private void DuyetQuyenTinhNang()
        {
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.HuyDongVon;component/DangKySanPham/ucDangKySanPhamCT.xaml", ref Toolbar, ref mnuMain);
            foreach (var item in mnuMain.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += btnShortcutKey_Click;
            }
        }

        private void InitEventHandler()
        {
            //Tab thông tin chung
            cmbNhomSP.SelectionChanged += new SelectionChangedEventHandler(cmbNhomSP_SelectionChanged);
            cmbHinhThuc.SelectionChanged += new SelectionChangedEventHandler(cmbHinhThuc_SelectionChanged);
            cmbLoaiLS.SelectionChanged += new SelectionChangedEventHandler(cmbLoaiLS_SelectionChanged);
            txtMaLaiSuat.KeyDown += new KeyEventHandler(txtMaLaiSuat_KeyDown);
            btnMaLaiSuat.Click += new RoutedEventHandler(btnMaLaiSuat_Click);
            btnLaiSuatAD.Click += new RoutedEventHandler(btnLaiSuatAD_Click);

            //Tab tiết kiệm quy định
            radTuyetDoi.Checked += new RoutedEventHandler(radTuyetDoi_Checked);
            radTuongDoi.Checked += new RoutedEventHandler(radTuongDoi_Checked);
            radTuyetDoi_TGui.Checked += new RoutedEventHandler(radTuyetDoi_TGui_Checked);
            radTuongDoi_TGui.Checked += new RoutedEventHandler(radTuongDoi_TGui_Checked);
            radTyLe_TGui.Checked += new RoutedEventHandler(radTyLe_TGui_Checked);
            radTheoKhoang_TGui.Checked += new RoutedEventHandler(radTheoKhoang_TGui_Checked);
            btnDelete_TGui.Click += new RoutedEventHandler(btnDelete_TGui_Click);            
            grdTGui.PreviewTextInput += new TextCompositionEventHandler(grdTGui_PreviewTextInput);
            grdTGui.CellEditEnded += new EventHandler<Telerik.Windows.Controls.GridViewCellEditEndedEventArgs>(grdTGui_CellEditEnded);
            grdTGui.CellValidating += new EventHandler<Telerik.Windows.Controls.GridViewCellValidatingEventArgs>(grdTGui_CellValidating);

            //Tab tiết kiệm có kỳ hạn
            chkChoPhepTTTH.Checked += new RoutedEventHandler(chkChoPhepTTTH_Checked);
            chkChoPhepTTTH.Unchecked += new RoutedEventHandler(chkChoPhepTTTH_UnChecked);

            //Tab tài khoản thanh toán
            btnMaPhi.Click += new RoutedEventHandler(btnMaPhi_Click);
            btnThemPhi.Click += new RoutedEventHandler(btnThemPhi_Click);
            btnXoaPhi.Click += new RoutedEventHandler(btnXoaPhi_Click);
            grdTTPhiTK.KeyDown += new KeyEventHandler(grdTTPhiTK_KeyDown);

            //Tab tài khoản hạch toán
            grdTKhoan.KeyDown += new KeyEventHandler(grdTKhoan_KeyDown);            
        }

        /// <summary>
        /// Hàm load dữ liệu lên các combobox
        /// </summary>
        private void LoadComboBox()
        {
            try
            {
                AutoComboBox auto = new AutoComboBox();
                string quy = BusinessConstant.TAN_SUAT.QUY.layGiaTri();

                //load dữ liệu hình thức trả lãi lên combobox
                List<string> lstDieuKien = new List<string>();
                lstDieuKien.Add(DatabaseConstant.getValue(DatabaseConstant.DanhMuc.HINH_THUC_TRA_LAI));
                lstSourceHinhThucTLai.Clear();
                auto.GenAutoComboBox(ref lstSourceHinhThucTLai, ref cmbHinhThuc, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien);
                //cmbHinhThuc.SelectedIndex = 0;
                //lstSourceHinhThucTLaiCoDinh = new List<CustomControl.AutoCompleteEntry>();
                //lstSourceHinhThucTLaiCoDinh = lstSourceHinhThucTLai;

                //load dữ liệu nhóm sản phẩm lên combobox
                lstDieuKien.Clear();
                lstDieuKien.Add(DatabaseConstant.getValue(DatabaseConstant.DanhMuc.NHOM_SPHAM_TKIEM));
                auto.GenAutoComboBox(ref lstSourceNhomSP, ref cmbNhomSP, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien);

                //load dữ liệu định kỳ tần suất trả lãi lên combobox (ngày, tháng, năm...)                
                lstDieuKien.Clear();
                lstDieuKien.Add(DatabaseConstant.getValue(DatabaseConstant.DanhMuc.TAN_SUAT));
                auto.GenAutoComboBox(ref lstSourceTanSuat, ref cmbTanSuatTraLai, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, BusinessConstant.TAN_SUAT.THANG.layGiaTri());
                lstSourceTanSuatTraLai = auto.CopyListEntry(lstSourceTanSuat);
                auto.removeEntry(ref lstSourceTanSuatTraLai, ref cmbTanSuatTraLai, quy, BusinessConstant.TAN_SUAT.THANG.layGiaTri());

                //load dữ liệu lên combobox kỳ hạn(ngày, tháng, năm...)
                lstSourceKyHan = auto.CopyListEntry(lstSourceTanSuat);
                auto.GenAutoComboBoxBySource(ref lstSourceKyHan, ref cmbKyHan, BusinessConstant.TAN_SUAT.THANG.layGiaTri());
                auto.removeEntry(ref lstSourceKyHan, ref cmbKyHan, quy, BusinessConstant.TAN_SUAT.THANG.layGiaTri());

                //load dữ liệu định kỳ đánh giá lại lãi suất lên combobox (ngày, tháng, năm...)
                lstSourceDinhKyDGiaLai = auto.CopyListEntry(lstSourceTanSuat);
                auto.GenAutoComboBoxBySource(ref lstSourceDinhKyDGiaLai, ref cmbDinhKyDGiaLai, BusinessConstant.TAN_SUAT.NGAY.layGiaTri());
                auto.removeEntry(ref lstSourceDinhKyDGiaLai, ref cmbDinhKyDGiaLai, quy, BusinessConstant.TAN_SUAT.NGAY.layGiaTri());

                //load dữ liệu loại tiền lên combobox                         
                auto.GenAutoComboBox(ref lstSourceLoaiTien, ref cmbLoaiTien, DatabaseConstant.DanhSachTruyVan.COMBOBOX_TIENTE.getValue(), null, ClientInformation.MaDongNoiTe);                                

                //load dữ liệu loại lãi suất lên combobox
                lstDieuKien.Clear();
                lstDieuKien.Add(DatabaseConstant.getValue(DatabaseConstant.DanhMuc.LOAI_LAI_SUAT));
                auto.GenAutoComboBox(ref lstSourceLoaiLS, ref cmbLoaiLS, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien);
                cmbLoaiLS.SelectedIndex = 0;

                //load dữ liệu cơ sở tính lãi lên combobox
                auto.GenAutoComboBox(ref lstSourceCoSoTLai, ref cmbCoSoTinhLai,DatabaseConstant.DanhSachTruyVan.COMBOBOX_COSOTINHLAI.getValue());
                cmbCoSoTinhLai.SelectedIndex = 0;

                //load dữ liệu cơ sở tỷ lệ tương đối lên combobox ở tab tiết kiệm quy định - group số tiền tối thiểu khi rút gốc 1 phần
                lstDieuKien.Clear();
                lstDieuKien.Add(DatabaseConstant.getValue(DatabaseConstant.DanhMuc.CSO_TINH_TLE_RGOC));
                auto.GenAutoComboBox(ref lstSourceRutGocTyLeTuongDoi, ref cmbTyLeTuongDoi, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien);
                cmbCoSoTinhLai.SelectedIndex = 0;

                //load dữ liệu cơ sở tính tỷ lệ lên combobox ở tab tiết kiệm quy định - Group số tiền gửi mỗi kỳ
                lstDieuKien.Clear();
                lstDieuKien.Add(DatabaseConstant.getValue(DatabaseConstant.DanhMuc.DU_NO_GOC_TINH_ST_GUI_KY));
                auto.GenAutoComboBox(ref lstSourceRutGocTyLeTyLe, ref cmbTyLe, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien);
                cmbCoSoTinhLai.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }
       
        private void KhoiTaoGridTheoKhoang()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("STT", typeof(int));
            dt.Columns.Add("DU_NO", typeof(decimal));
            dt.Columns.Add("SO_TIEN_KY", typeof(decimal));
            dsTheoKhoang = new DataSet();
            dsTheoKhoang.Tables.Add(dt);
            
        }

        private void KhoiTaoGridPhi()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("STT", typeof(int));
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("MA_BPHI", typeof(string));
            dt.Columns.Add("TEN_BPHI", typeof(string));
            dsPhi = new DataSet();
            dsPhi.Tables.Add(dt);
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
                        KeyGesture keyg = new KeyGesture(Key.S, ModifierKeys.Control | ModifierKeys.Shift);
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
            RibbonButton tlb = (RibbonButton)sender;
            string strTinhNang = tlb.Name.Substring(3, tlb.Name.Length - 3);
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

        /// <summary>
        /// Sự kiện load form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Dispatcher.CurrentDispatcher.DelayInvoke("UserControl_Loaded", () =>
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
                }, TimeSpan.FromSeconds(0));
                
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
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

            bool ret = process.UnlockData(DatabaseConstant.Module.HDVO,
                DatabaseConstant.Function.HDV_SAN_PHAM,
                DatabaseConstant.Table.BL_SAN_PHAM,
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
            if (OnSavingCompleted != null)
            {
                OnSavingCompleted(this, EventArgs.Empty);
            }
            CustomControl.CommonFunction.CloseUserControl(this);
        }

        /// <summary>
        /// Hàm load ResetForm
        /// </summary>
        private void ResetForm()
        {
            try
            {
                #region Thông tin chung
                //Trạng thái
                lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);

                //Group Thông tin sản phẩm                   
                cmbNhomSP.SelectedIndex = lstSourceNhomSP.IndexOf(lstSourceNhomSP.FirstOrDefault(i => i.KeywordStrings.First().Equals(BusinessConstant.NHOM_SPHAM_TKIEM.T01.layGiaTri())));
                txtMaSanPham.Text = "";
                txtTenSanPham.Text = "";
                cmbHinhThuc.SelectedIndex = lstSourceHinhThucTLai.IndexOf(lstSourceHinhThucTLai.FirstOrDefault(i => i.KeywordStrings.First().Equals(BusinessConstant.HINH_THUC_TRA_LAI.CUOI_KY.layGiaTri())));
                numTanSuatTraLai.Value = 0;
                cmbTanSuatTraLai.SelectedIndex = lstSourceTanSuatTraLai.IndexOf(lstSourceTanSuatTraLai.FirstOrDefault(i => i.KeywordStrings.First().Equals(BusinessConstant.DON_VI_TINH_THOI_GIAN.THANG.layGiaTri())));
                cmbLoaiTien.SelectedIndex = lstSourceLoaiTien.IndexOf(lstSourceLoaiTien.FirstOrDefault(i => i.KeywordStrings.First().Equals(ClientInformation.MaDongNoiTe)));
                numKyHan.Value = null;
                cmbKyHan.SelectedIndex = -1;

                //Group Thông tin lãi suất                    
                cmbLoaiLS.SelectedIndex = lstSourceLoaiLS.IndexOf(lstSourceLoaiLS.FirstOrDefault(i => i.KeywordStrings.First().Equals(BusinessConstant.LOAI_LAI_SUAT.CO_DINH.layGiaTri())));
                idLaiSuat = 0;
                txtMaLaiSuat.Text = "";
                lblTenLaiSuat.Content = LLanguage.SearchResourceByKey("U.HuyDongVon.ucDangKySanPhamCT.TenLaiSuat");
                numBienDoLS.Value = 0;
                numDinhKyLS.Value = 1;
                cmbDinhKyDGiaLai.SelectedIndex = lstSourceDinhKyDGiaLai.IndexOf(lstSourceDinhKyDGiaLai.FirstOrDefault(i => i.KeywordStrings.First().Equals(BusinessConstant.DON_VI_TINH_THOI_GIAN.NGAY.layGiaTri())));
                chkLSThayDoi.IsChecked = false;
                idCoSoTinhLai = 0;
                cmbCoSoTinhLai.SelectedIndex = lstSourceCoSoTLai.IndexOf(lstSourceCoSoTLai.FirstOrDefault(i => i.KeywordStrings.First().Equals(BusinessConstant.CO_SO_TINH_LAI.CS360_360.layGiaTri())));

                //Group Thông tin khác
                numSoDuToiThieu.Value = 0;
                numSoDuToiDa.Value = null;
                numSoDuToiThieuRutGoc1Phan.Value = 0;
                numSoDuToiThieuTinhLai.Value = 0;
                raddtNgayHL.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
                raddtNgayHetHL.Value = null;
                numSoDuToiThieuTinhLai.Value = 0;
                numSoDuToiThieuRutGoc1Phan.Value = 0;
                #endregion                

                #region Hiển thị tab Tài khoản(tiền gửi) có kỳ hạn
                radLaiDon.IsChecked = false;
                chkChoPhepTTTH.IsChecked = false;
                chkChoPhepRGTH.IsChecked = false;
                txtLaiSuatAD.Text = "";
                #endregion

                #region Hiển thị tab Tiền gửi thanh toán
                chkTTChoPhep.IsChecked =false;
                chkTTCoPhi.IsChecked = false;
                radTTSoDuThoiDiem.IsChecked = true;
                radTTSoDuBinhQuan.IsChecked = false;
                txtTTMaPhi.Text = "";
                chkTTChoPhepThauChi.IsChecked = false;
                numTC_ToiDa.Value = null;
                dsPhi.Tables[0].Rows.Clear();
                DataViewManager dataViewManager = new DataViewManager(dsPhi);
                DataView dataView = dataViewManager.CreateDataView(dsPhi.Tables[0]);
                grdTTPhiTK.DataContext = dataView;
                #endregion

                #region Hiển thị tab tài khoản hạch toán
                LoadGridTaiKhoanHachToan();
                #endregion

                #region Tab thông tin kiểm soát
                txtTrangThaiBanGhi.Text = "";
                raddtNgayLap.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
                txtNguoiLap.Text = ClientInformation.TenDangNhap;
                raddtNgayCapNhat.Value = null;
                txtNguoiCapNhat.Text = "";
                #endregion
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }            
        }

        private void HideControl()
        {
            try
            {
                if (ClientInformation.Company.Equals(ApplicationConstant.DonViSuDung.BANTAYVANG.layGiaTri()) || ClientInformation.Company.Equals(ApplicationConstant.DonViSuDung.BENTRE.layGiaTri()) || ClientInformation.Company.Equals(ApplicationConstant.DonViSuDung.HOCVIENNGANHANG.layGiaTri()))
                {
                    tbiTKQuyDinh.Visibility = System.Windows.Visibility.Collapsed;
                }                
                        
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            
        }

        private void chkThemNhieuLan_Checked(object sender, RoutedEventArgs e)
        {
            action = DatabaseConstant.Action.THEM;
            sTrangThaiNVu = "";
            id = 0;
            idLaiSuat = 0;
            idCoSoTinhLai = 0;
            obj = null;
            lstObjCT = null;
            lstPHPL = null;
            BeforeAddNew();

            chkThemNhieuLan.IsChecked = false;
            cmbNhomSP.Focus();
        }
        
        /// <summary>
        /// tạo grid ở tab tiết kiệm quy định - Group số tiền gửi mỗi kỳ
        /// </summary>
        private void LoadGridTheoKhoangTKQD()
        {
            try
            {
                if (action == DatabaseConstant.Action.THEM)
                {
                    dsTheoKhoang.Tables[0].Rows.Clear();
                    for(int i=0;i<30;i++)
                        dsTheoKhoang.Tables[0].Rows.Add(null, 0, 0);
                }
                else if (action == DatabaseConstant.Action.SUA || action == DatabaseConstant.Action.XEM)
                {
                    HuyDongVonProcess huyDongVonProcess = new HuyDongVonProcess();
                    dsTheoKhoang = huyDongVonProcess.GetTGuiMoiKyTheoID(id);
                    for (int i = 0; i < 30; i++)
                        dsTheoKhoang.Tables[0].Rows.Add(null, 0, 0);

                }
                if (dsTheoKhoang != null)
                {
                    DataViewManager dataViewManager = new DataViewManager(dsTheoKhoang);
                    DataView dataView = dataViewManager.CreateDataView(dsTheoKhoang.Tables[0]);
                    grdTGui.DataContext = dataView;
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        /// <summary>
        /// Tạo grid tài khoản hạch toán ở tab tài khoản hạch toán
        /// </summary>
        private void LoadGridTaiKhoanHachToan()
        {
            try
            {
                string maDoiTuong = "";
                if (action == DatabaseConstant.Action.THEM)
                {
                    string maNhomSP = lstSourceNhomSP.ElementAt(cmbNhomSP.SelectedIndex).KeywordStrings.ElementAt(0);
                    if(maNhomSP.Equals(BusinessConstant.layGiaTri(BusinessConstant.NHOM_SPHAM_TKIEM.T01)))
                        maDoiTuong = "MACDINH01";
                    else if (maNhomSP.Equals(BusinessConstant.layGiaTri(BusinessConstant.NHOM_SPHAM_TKIEM.T02)))
                        maDoiTuong = "MACDINH02";
                    else if (maNhomSP.Equals(BusinessConstant.layGiaTri(BusinessConstant.NHOM_SPHAM_TKIEM.T03)))
                        maDoiTuong = "MACDINH03";
                    else if (maNhomSP.Equals(BusinessConstant.layGiaTri(BusinessConstant.NHOM_SPHAM_TKIEM.T04)))
                        maDoiTuong = "MACDINH04";
                    else if (maNhomSP.Equals(BusinessConstant.layGiaTri(BusinessConstant.NHOM_SPHAM_TKIEM.T05)))
                        maDoiTuong = "MACDINH05";
                    else if (maNhomSP.Equals(BusinessConstant.layGiaTri(BusinessConstant.NHOM_SPHAM_TKIEM.T06)))
                        maDoiTuong = "MACDINH06";
                    else if (maNhomSP.Equals(BusinessConstant.layGiaTri(BusinessConstant.NHOM_SPHAM_TKIEM.T07)))
                        maDoiTuong = "MACDINH07";
                    else if (maNhomSP.Equals(BusinessConstant.layGiaTri(BusinessConstant.NHOM_SPHAM_TKIEM.T08)))
                        maDoiTuong = "MACDINH08";
                }
                else if (action == DatabaseConstant.Action.SUA || action == DatabaseConstant.Action.XEM)
                    maDoiTuong = txtMaSanPham.Text;
                else
                {
                    return;
                }

                HuyDongVonProcess huyDongVonProcess = new HuyDongVonProcess();

                DataTable dt = null;

                LDatatable.MakeParameterTable(ref dt);
                LDatatable.AddParameter(ref dt, "@MA_PHAN_HE", "STRING", DatabaseConstant.Module.HDVO.getValue());
                LDatatable.AddParameter(ref dt, "@MA_DTUONG", "STRING", maDoiTuong);
                LDatatable.AddParameter(ref dt, "@MA_DVI", "STRING", ClientInformation.MaDonVi);
                dsTaiKhoanHachToan = huyDongVonProcess.GetTaiKhoanHachToan(dt);
                

                if (dsTaiKhoanHachToan != null)
                {
                    DataSet ds = dsTaiKhoanHachToan.Copy();
                    //ds.Tables[0].Rows.Clear();                    

                    for (int i = ds.Tables[0].Rows.Count-1; i>=0; i--)
                    {
                        if (ds.Tables[0].Rows[i]["MA_PLOAI"].ToString().IsNullOrEmptyOrSpace())
                        {
                            ds.Tables[0].Rows.RemoveAt(i);
                        }
                    }

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        ds.Tables[0].Rows[i]["STT"] = i + 1;
                    }

                    if (ds != null)
                    {
                        DataViewManager dataViewManager = new DataViewManager(ds);
                        DataView dataView = dataViewManager.CreateDataView(ds.Tables[0]);
                        grdTKhoan.DataContext = dataView;
                    }

                    if (ClientInformation.PhuongPhapHachToan.Equals(DatabaseConstant.PHUONG_PHAP_HACH_TOAN.DOC_LAP.layGiaTri()))
                    {
                        grdTKhoan.Columns[6].IsVisible = true;
                        grdTKhoan.Columns[7].IsVisible = true;
                        grdTKhoan.Columns[8].IsVisible = false;
                        grdTKhoan.Columns[9].IsVisible = false;
                    }
                    else
                    {
                        grdTKhoan.Columns[6].IsVisible = false;
                        grdTKhoan.Columns[7].IsVisible = false;
                        grdTKhoan.Columns[8].IsVisible = true;
                        grdTKhoan.Columns[9].IsVisible = true;
                    }
                    
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }          

        /// <summary>
        /// Set trạng thái enable và disable cho các control
        /// </summary>
        /// <param name="enable"></param>
        private void SetEnabledAllControls(bool enable)
        {
            try
            {
                #region Tab thông tin chung
                //Group Thông tin sản phẩm                   
                cmbNhomSP.IsEnabled = enable;                
                txtTenSanPham.IsEnabled = enable;
                cmbHinhThuc.IsEnabled = enable;
                numTanSuatTraLai.IsEnabled = enable;
                cmbTanSuatTraLai.IsEnabled = enable;
                cmbLoaiTien.IsEnabled = false;
                if (cmbHinhThuc.IsEnabled == true)
                {
                    string hinhThucTraLai = lstSourceHinhThucTLai.ElementAt(cmbHinhThuc.SelectedIndex).KeywordStrings.ElementAt(0);
                    if (hinhThucTraLai.Equals(BusinessConstant.HINH_THUC_TRA_LAI.DINH_KY.layGiaTri()))
                    {
                        numTanSuatTraLai.IsEnabled = true;
                        cmbTanSuatTraLai.IsEnabled = true;                        
                    }
                    else
                    {
                        numTanSuatTraLai.IsEnabled = false;
                        cmbTanSuatTraLai.IsEnabled = false;                        
                    }
                }

                //Group Thông tin lãi suất                    
                cmbLoaiLS.IsEnabled = enable;
                txtMaLaiSuat.IsEnabled = enable;
                btnMaLaiSuat.IsEnabled = enable;                
                numBienDoLS.IsEnabled = enable;
                numDinhKyLS.IsEnabled = enable;
                cmbDinhKyDGiaLai.IsEnabled = enable;
                chkLSThayDoi.IsEnabled = false;
                cmbCoSoTinhLai.IsEnabled = enable;
                if (cmbLoaiLS.IsEnabled == true)
                {
                    string sLoaiLS = lstSourceLoaiLS.ElementAt(cmbLoaiLS.SelectedIndex).KeywordStrings.ElementAt(0).ToString();
                    if (sLoaiLS.Equals(BusinessConstant.LOAI_LAI_SUAT.CO_DINH.layGiaTri()))
                    {
                        numBienDoLS.IsEnabled = false;
                        numDinhKyLS.IsEnabled = false;
                        cmbDinhKyDGiaLai.IsEnabled = false;                        
                    }
                    if (sLoaiLS.Equals(BusinessConstant.LOAI_LAI_SUAT.THA_NOI.layGiaTri()))
                    {
                        numBienDoLS.IsEnabled = true;
                        numDinhKyLS.IsEnabled = true;
                        cmbDinhKyDGiaLai.IsEnabled = true;                        
                    }
                }
                numKyHan.IsEnabled = enable;
                cmbKyHan.IsEnabled = enable;

                //Group Thông tin khác
                numSoDuToiThieu.IsEnabled = enable;
                numSoDuToiDa.IsEnabled = enable;
                raddtNgayHL.IsEnabled = enable;
                raddtNgayHetHL.IsEnabled = enable;
                dtpNgayHetHL.IsEnabled = enable;
                dtpNgayHL.IsEnabled = enable;
                numSoDuToiThieuTinhLai.IsEnabled = enable;
                numSoDuToiThieuRutGoc1Phan.IsEnabled = enable;
                #endregion

                #region Tab Tài khoản(tiền gửi) có kỳ hạn

                radLaiDon.IsEnabled = enable;
                radLaiKep.IsEnabled = enable;
                chkChoPhepTTTH.IsEnabled = enable;
                chkChoPhepRGTH.IsEnabled = enable;
                txtLaiSuatAD.IsEnabled = enable;
                btnLaiSuatAD.IsEnabled = enable;                
                if (enable == true)
                {
                    if (chkChoPhepTTTH.IsChecked == true)
                    {
                        txtLaiSuatAD.IsEnabled = true;
                        btnLaiSuatAD.IsEnabled = true;
                    }
                    else
                    {
                        txtLaiSuatAD.IsEnabled = false;
                        btnLaiSuatAD.IsEnabled = false;
                    }
                }
                #endregion

                #region Tab Tiết kiệm quy định
                //Group số tiền tối thiểu khi rút gốc 1 phần
                radTuyetDoi.IsEnabled = enable;
                radTuongDoi.IsEnabled = enable;
                if (radTuyetDoi.IsChecked == true)
                {
                    numSoTienTuyetDoi.IsEnabled = enable;
                    numTyLeTuongDoi.IsEnabled = false;
                    cmbTyLeTuongDoi.IsEnabled = false;
                }
                else
                {
                    numSoTienTuyetDoi.IsEnabled = false;
                    numTyLeTuongDoi.IsEnabled = enable;
                    cmbTyLeTuongDoi.IsEnabled = enable;
                }
                chkSoDuToiThieu.IsEnabled = enable;

                //Group số tiền gửi mỗi kỳ
                radTuyetDoi_TGui.IsEnabled = enable;
                radTuongDoi_TGui.IsEnabled = enable;
                if (radTuyetDoi_TGui.IsChecked == true)
                {
                    numTuyetDoi_TGui.IsEnabled = enable;
                    radTyLe_TGui.IsEnabled = false;
                    radTheoKhoang_TGui.IsEnabled = false;
                    numTyLe_TGui.IsEnabled = false;
                    cmbTyLe.IsEnabled = false;
                    btnDelete_TGui.IsEnabled = false;
                    grdTGui.IsEnabled = false;
                }
                else
                {
                    numTuyetDoi_TGui.IsEnabled = false;
                    radTyLe_TGui.IsEnabled = enable;
                    radTheoKhoang_TGui.IsEnabled = enable;
                    if (radTyLe_TGui.IsChecked == true)
                    {
                        numTyLe_TGui.IsEnabled = enable;
                        cmbTyLe.IsEnabled = enable;
                        btnDelete_TGui.IsEnabled = false;
                        grdTGui.IsEnabled = false;
                    }
                    else
                    {
                        numTyLe_TGui.IsEnabled = false;
                        cmbTyLe.IsEnabled = false;
                        btnDelete_TGui.IsEnabled = enable;
                        grdTGui.IsEnabled = enable;
                    }
                }                
                #endregion

                #region Tab Tiền gửi thanh toán
                chkTTChoPhep.IsEnabled = enable;
                chkTTCoPhi.IsEnabled = enable;
                radTTSoDuThoiDiem.IsEnabled = enable;
                radTTSoDuBinhQuan.IsEnabled = enable;
                txtTTMaPhi.IsEnabled = enable;
                btnMaPhi.IsEnabled = enable;
                btnThemPhi.IsEnabled = enable;
                btnXoaPhi.IsEnabled = enable;
                grdTTPhiTK.IsEnabled = enable;
                chkTTChoPhepThauChi.IsEnabled = enable;
                #endregion

                #region Tab tài khoản hạch toán
                grdTKhoan.IsEnabled = enable;
                #endregion
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        private void SetEnabledRequiredControls(bool enable)
        {
            #region Nhóm sản phẩm
            if (action == DatabaseConstant.Action.THEM)
            {
                cmbNhomSP.IsEnabled = enable;
            }
            else
            {
                cmbNhomSP.IsEnabled = false;
            }
            #endregion

            #region Trạng thái checkbox lãi suất thay đổi theo bậc thang
            if (!txtMaLaiSuat.Text.IsNullOrEmptyOrSpace())
            {
                LaiSuatProcess processLaiSuat = new LaiSuatProcess();
                DataSet ds = processLaiSuat.GetLaiSuatByID(idLaiSuat);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    string phuongPhap = ds.Tables[0].Rows[0]["PPHAP_TINH_LSUAT"].ToString();
                    if(phuongPhap.Equals(BusinessConstant.PPHAP_TINH_LSUAT.BTH.layGiaTri()))
                    {
                        string hinhThucBacTang = ds.Tables[0].Rows[0]["HTHUC_BTHANG"].ToString();
                        if (hinhThucBacTang.Equals(BusinessConstant.HTHUC_BTHANG.KY_HAN.layGiaTri()) || hinhThucBacTang.Equals(BusinessConstant.HTHUC_BTHANG.KHAN_STIEN.layGiaTri()))
                        {
                            chkLSThayDoi.IsEnabled = enable;
                        }
                    }
                }
            }
            #endregion

            #region Kỳ hạn
            string maNhomSP = lstSourceNhomSP.ElementAt(cmbNhomSP.SelectedIndex).KeywordStrings.ElementAt(0);
            if (maNhomSP.Equals(BusinessConstant.NHOM_SPHAM_TKIEM.T03.layGiaTri()) || maNhomSP.Equals(BusinessConstant.NHOM_SPHAM_TKIEM.T04.layGiaTri()) || maNhomSP.Equals(BusinessConstant.NHOM_SPHAM_TKIEM.T05.layGiaTri()) || maNhomSP.Equals(BusinessConstant.NHOM_SPHAM_TKIEM.T06.layGiaTri()) || maNhomSP.Equals(BusinessConstant.NHOM_SPHAM_TKIEM.T07.layGiaTri()))
            {
                numKyHan.IsEnabled = enable;
                cmbKyHan.IsEnabled = enable;
            }
            else
            {
                numKyHan.IsEnabled = false;
                cmbKyHan.IsEnabled = false;
            }
            #endregion
        }

        /// <summary>
        /// Hiển thị thông tin của từng tab theo nhóm sản phẩm
        /// </summary>
        /// <param name="i">hiển thị tab thứ i  (2 tab thông tin chung + kiểm soát luôn hiển thị)</param>
        private void HienThiTab(int i)
        {
            try
            {
                tbiThongTinChung.Visibility = System.Windows.Visibility.Visible;
                tbiKiemSoat.Visibility = System.Windows.Visibility.Visible;

                if (i == 2)
                {
                    tbiTaiKhoanCKK.Visibility = System.Windows.Visibility.Visible;
                    tbiTKQuyDinh.Visibility = System.Windows.Visibility.Collapsed;
                    tbiTGThanhToan.Visibility = System.Windows.Visibility.Collapsed;
                }
                else if (i == 3)
                {
                    tbiTaiKhoanCKK.Visibility = System.Windows.Visibility.Collapsed;
                    tbiTKQuyDinh.Visibility = System.Windows.Visibility.Visible;
                    tbiTGThanhToan.Visibility = System.Windows.Visibility.Collapsed;
                }
                else if (i == 4)
                {
                    tbiTaiKhoanCKK.Visibility = System.Windows.Visibility.Collapsed;
                    tbiTKQuyDinh.Visibility = System.Windows.Visibility.Collapsed;
                    tbiTGThanhToan.Visibility = System.Windows.Visibility.Collapsed;
                }
                else
                {
                    tbiTaiKhoanCKK.Visibility = System.Windows.Visibility.Collapsed;
                    tbiTKQuyDinh.Visibility = System.Windows.Visibility.Collapsed;
                    tbiTGThanhToan.Visibility = System.Windows.Visibility.Collapsed;  
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }
       
        /// <summary>
        /// Sự kiện khi thay đổi giá trị combobx nhóm sản phẩm => sẽ hiển thị các tab tương ứng với nhóm sản phẩm
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbNhomSP_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                string maNhomSP = lstSourceNhomSP.ElementAt(cmbNhomSP.SelectedIndex).KeywordStrings.ElementAt(0);

                #region Hiển thị Tab
                if (maNhomSP.Equals(BusinessConstant.layGiaTri(BusinessConstant.NHOM_SPHAM_TKIEM.T02)))
                {
                    //Nếu nhóm sản phẩm là tiết kiệm tự nguyện không kỳ hạn
                    HienThiTab(0);
                }

                if (maNhomSP.Equals(BusinessConstant.layGiaTri(BusinessConstant.NHOM_SPHAM_TKIEM.T03)) || maNhomSP.Equals(BusinessConstant.layGiaTri(BusinessConstant.NHOM_SPHAM_TKIEM.T04)) || maNhomSP.Equals(BusinessConstant.layGiaTri(BusinessConstant.NHOM_SPHAM_TKIEM.T05)) || maNhomSP.Equals(BusinessConstant.layGiaTri(BusinessConstant.NHOM_SPHAM_TKIEM.T06)) || maNhomSP.Equals(BusinessConstant.layGiaTri(BusinessConstant.NHOM_SPHAM_TKIEM.T07)))
                {
                    HienThiTab(2);
                }
                else if (maNhomSP.Equals(BusinessConstant.layGiaTri(BusinessConstant.NHOM_SPHAM_TKIEM.T01)))
                {
                    HienThiTab(3);
                    if (ClientInformation.Company.Equals(ApplicationConstant.DonViSuDung.BANTAYVANG.layGiaTri())
                        || ClientInformation.Company.Equals(ApplicationConstant.DonViSuDung.BIDV_BLF.layGiaTri()))
                    {
                        tbiTKQuyDinh.Visibility = System.Windows.Visibility.Collapsed;
                    }
                }
                else if (maNhomSP.Equals(BusinessConstant.layGiaTri(BusinessConstant.NHOM_SPHAM_TKIEM.T08)))
                {
                    HienThiTab(4);
                }
                #endregion

                #region Kỳ hạn
                if (maNhomSP.Equals(BusinessConstant.NHOM_SPHAM_TKIEM.T03.layGiaTri()) || maNhomSP.Equals(BusinessConstant.NHOM_SPHAM_TKIEM.T04.layGiaTri()) || maNhomSP.Equals(BusinessConstant.NHOM_SPHAM_TKIEM.T05.layGiaTri()) || maNhomSP.Equals(BusinessConstant.NHOM_SPHAM_TKIEM.T06.layGiaTri()) || maNhomSP.Equals(BusinessConstant.NHOM_SPHAM_TKIEM.T07.layGiaTri()))
                {
                    numKyHan.IsEnabled = true;
                    cmbKyHan.IsEnabled = true;
                }
                else
                {
                    numKyHan.IsEnabled = false;
                    cmbKyHan.IsEnabled = false;
                }
                #endregion

                LoadGridTaiKhoanHachToan();
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void cmbHinhThuc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbHinhThuc.SelectedIndex < 0) return;

            string hinhThucTraLai = lstSourceHinhThucTLai.ElementAt(cmbHinhThuc.SelectedIndex).KeywordStrings.ElementAt(0);
            if(hinhThucTraLai.Equals(BusinessConstant.HINH_THUC_TRA_LAI.DINH_KY.layGiaTri()))
            {
                numTanSuatTraLai.IsEnabled = true;
                cmbTanSuatTraLai.IsEnabled = true;
                numTanSuatTraLai.Value = 1;
            }
            else
            {
                numTanSuatTraLai.IsEnabled = false;
                cmbTanSuatTraLai.IsEnabled = false;
                numTanSuatTraLai.Value = null;
            }
        }

        private void txtMaLaiSuat_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F3)
            {
                btnMaLaiSuat_Click(null, null);
            }
        }

        private void btnMaLaiSuat_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                var process = new PopupProcess();
                List<string> lstDieuKien = new List<string>();
                string maNhomSP = lstSourceNhomSP.ElementAt(cmbNhomSP.SelectedIndex).KeywordStrings.ElementAt(0);
                lstDieuKien.Add(maNhomSP);
                lstDieuKien.Add(ClientInformation.MaDonVi);
                process.getPopupInformation(DatabaseConstant.DanhSachTruyVan.POPUP_DM_LSUAT_HDV.getValue(), lstDieuKien);                
                SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;

                ucPopup popup = new ucPopup(false, simplePopupResponse, false);
                popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
                Window win = new Window();
                win.Content = popup;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.ShowDialog();
                if (lstPopup != null && lstPopup.Count > 0)
                {
                    DataRow row = lstPopup[0];
                    if (!string.IsNullOrWhiteSpace(row[1].ToString()))
                        idLaiSuat = Convert.ToInt32(row[1].ToString());
                    if (!string.IsNullOrWhiteSpace(row[2].ToString()))
                        txtMaLaiSuat.Text = row[2].ToString();
                    if (!string.IsNullOrWhiteSpace(row[3].ToString()))
                        lblTenLaiSuat.Content = row[3].ToString();

                    LaiSuatProcess processLaiSuat = new LaiSuatProcess();
                    DataSet ds = processLaiSuat.GetLaiSuatByID(idLaiSuat);
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        string phuongPhap = ds.Tables[0].Rows[0]["PPHAP_TINH_LSUAT"].ToString();
                        if (phuongPhap.Equals(BusinessConstant.PPHAP_TINH_LSUAT.BTH.layGiaTri()))
                        {
                            string hinhThucBacTang = ds.Tables[0].Rows[0]["HTHUC_BTHANG"].ToString();
                            if (hinhThucBacTang.Equals(BusinessConstant.HTHUC_BTHANG.KY_HAN.layGiaTri()) || hinhThucBacTang.Equals(BusinessConstant.HTHUC_BTHANG.KHAN_STIEN.layGiaTri()))
                            {
                                chkLSThayDoi.IsEnabled = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void radTuyetDoi_TGui_Checked(object sender, RoutedEventArgs e)
        {
            try
            {                
                numTuyetDoi_TGui.IsEnabled = true;

                radTyLe_TGui.IsEnabled = false;
                radTheoKhoang_TGui.IsEnabled = false;
                numTyLe_TGui.IsEnabled = false;
                cmbTyLe.IsEnabled = false;
                btnDelete_TGui.IsEnabled = false;
                grdTGui.IsEnabled = false;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }        

        private void radTuongDoi_TGui_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                numTuyetDoi_TGui.IsEnabled = false;
                radTyLe_TGui.IsEnabled = true;
                radTheoKhoang_TGui.IsEnabled = true;
                if (radTyLe_TGui.IsChecked == true)
                {
                    numTyLe_TGui.IsEnabled = true;
                    cmbTyLe.IsEnabled = true;
                    btnDelete_TGui.IsEnabled = false;
                    grdTGui.IsEnabled = false;
                }
                else
                {
                    numTyLe_TGui.IsEnabled = false;
                    cmbTyLeTuongDoi.IsEnabled = false;
                    btnDelete_TGui.IsEnabled = true;
                    grdTGui.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }

        }

        private void radTyLe_TGui_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (radTuongDoi_TGui.IsChecked == true)
                {
                    numTyLe_TGui.IsEnabled = true;
                    cmbTyLe.IsEnabled = true;
                    btnDelete_TGui.IsEnabled = false;
                    grdTGui.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void radTheoKhoang_TGui_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (radTuongDoi_TGui.IsChecked == true)
                {
                    numTyLe_TGui.IsEnabled = false;
                    cmbTyLe.IsEnabled = false;
                    btnDelete_TGui.IsEnabled = true;
                    grdTGui.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void radTuyetDoi_Checked(object sender, RoutedEventArgs e)
        {
            numSoTienTuyetDoi.IsEnabled = true;
            numTyLeTuongDoi.IsEnabled = false;
            cmbTyLeTuongDoi.IsEnabled = false;
        }

        private void radTuongDoi_Checked(object sender, RoutedEventArgs e)
        {
            numSoTienTuyetDoi.IsEnabled = false;
            numTyLeTuongDoi.IsEnabled = true;
            cmbTyLeTuongDoi.IsEnabled = true;
        }

        private void cmbLoaiLS_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                string sLoaiLS = lstSourceLoaiLS.ElementAt(cmbLoaiLS.SelectedIndex).KeywordStrings.ElementAt(0).ToString();
                if (sLoaiLS.Equals(BusinessConstant.LOAI_LAI_SUAT.CO_DINH.layGiaTri()))
                {
                    numBienDoLS.IsEnabled = false;
                    numDinhKyLS.IsEnabled = false;
                    cmbDinhKyDGiaLai.IsEnabled = false;
                    numBienDoLS.Value = 0;
                    numDinhKyLS.Value = null;                    
                }
                if (sLoaiLS.Equals(BusinessConstant.LOAI_LAI_SUAT.THA_NOI.layGiaTri()))
                {
                    numBienDoLS.IsEnabled = true;
                    numDinhKyLS.IsEnabled = true;
                    cmbDinhKyDGiaLai.IsEnabled = true;
                    numBienDoLS.Value = 0;
                    numDinhKyLS.Value = 1;
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void grdTGui_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            try
            {
                Convert.ToInt32(e.Text);
            }
            catch (Exception ex)
            {
                e.Handled = true;

            }
            //if (e.Text != "1" && e.Text != "2")
            //e.Handled = true;
        }

        private void grdTGui_CellEditEnded(object sender, Telerik.Windows.Controls.GridViewCellEditEndedEventArgs e)
        {
            try
            {
                int iRow = grdTGui.Items.IndexOf(grdTGui.Items.CurrentItem);                    
                DataRowView dr = (DataRowView)grdTGui.Items.CurrentItem;

                if (iRow == 0)
                {
                    if (Convert.ToDecimal(dr["DU_NO"]) == 0)
                    {
                        dr["DU_NO"] = Convert.ToInt32(dr["DU_NO"]);
                    }
                    dr["STT"] = iRow + 1;                          
                }
                else
                {
                    if (Convert.ToDecimal(dr["DU_NO"]) != 0)
                    {
                        if (iRow > 1)
                        {
                            DataRowView dr1 = (DataRowView)grdTGui.Items[iRow - 1];
                            if (Convert.ToDecimal(dr1["DU_NO"]) != 0)
                            {
                                dr["STT"] = iRow + 1;
                            }
                            else
                            {
                                dr["DU_NO"] = 0;
                                dr["SO_TIEN_KY"] = 0;
                            }
                        }
                        else
                        {
                            dr["STT"] = iRow + 1;
                        }
                    }
                    else
                    {
                        dr["SO_TIEN_KY"] = 0;
                    }
                }
                
                grdTGui.CurrentItem = dr;
                
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void btnLaiSuatAD_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var process = new PopupProcess();
                List<string> lstDieuKien = new List<string>();
                string maNhomSP = lstSourceNhomSP.ElementAt(cmbNhomSP.SelectedIndex).KeywordStrings.ElementAt(0);
                lstDieuKien.Add(maNhomSP);
                lstDieuKien.Add(ClientInformation.MaDonVi);
                process.getPopupInformation(DatabaseConstant.DanhSachTruyVan.POPUP_DM_LSUAT_HDV.getValue(), lstDieuKien);
                SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;

                ucPopup popup = new ucPopup(false, simplePopupResponse, false);
                popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
                Window win = new Window();
                win.Content = popup;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.ShowDialog();
                if (lstPopup != null)
                {
                    DataRow row = lstPopup[0];
                    if (!string.IsNullOrWhiteSpace(row[2].ToString()))
                        txtLaiSuatAD.Text = row[2].ToString();
                    if (!string.IsNullOrWhiteSpace(row[3].ToString()))
                        lblLaiSuatAD.Content = row[3].ToString();
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void grdTGui_CellValidating(object sender, Telerik.Windows.Controls.GridViewCellValidatingEventArgs e)
        {

            GridViewCell cell = e.Cell;
            if (cell.Column.UniqueName.Equals("DU_NO"))
            {
                bool isValid = true;
                string sMessage = "";
                int iRow = grdTGui.Items.IndexOf(grdTGui.Items.CurrentItem);
                if (iRow > 0 && Convert.ToDecimal(e.NewValue) != 0)
                {
                    DataRowView dr1 = (DataRowView)grdTGui.Items[iRow - 1];
                    DataRowView dr2 = (DataRowView)grdTGui.Items[iRow];
                    decimal x1 = Convert.ToDecimal(dr1["DU_NO"]);
                    decimal x2 = Convert.ToDecimal(e.NewValue);

                    if (x1 > x2)
                    {
                        isValid = false;
                        sMessage = x1.ToString() + " > " + x2.ToString();
                    }
                    if (x1 == x2)
                    {
                        isValid = false;
                        sMessage = x1.ToString() + " = " + x2.ToString();
                    }
                }
                e.ErrorMessage = sMessage;
                e.IsValid = isValid;
            }


        }

        private void btnDelete_TGui_Click(object sender, RoutedEventArgs e)
        {
            List<DataRowView> lstSelected = new List<DataRowView>();         
            for(int i = 0; i < grdTGui.SelectedItems.Count; i++)
            {
                DataRowView dr = (DataRowView)grdTGui.SelectedItems[i];
                lstSelected.Add(dr);
            }
            
            if (lstSelected.Count == 0)
            {
                //LMessage.ShowMessage("M.DungChung.ChuaChonBanGhi", LMessage.MessageBoxType.Warning);
                return;
            }
            else
            {
                foreach (DataRowView dr in lstSelected)
                {
                    grdTGui.Items.Remove(dr);
                }

                for (int i = 0; i < grdTGui.Items.Count; i++)
                {

                    DataRowView dr = (DataRowView)grdTGui.Items[i];
                    if (!dr["STT"].ToString().IsNullOrEmptyOrSpace() && Convert.ToInt32(dr["STT"]) > 0)
                    {
                        if (!dr[2].ToString().IsNullOrEmptyOrSpace())
                        {
                            dr["STT"] = i + 1;
                            grdTGui.SelectedItem = grdTGui.Items[i];
                            grdTGui.CurrentItem = dr;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
            
        }

        private void tlbTaiKhoanHachToan_Click(object sender, RoutedEventArgs e)
        {
            Button btnChon = (Button)sender;
            int iRow = Convert.ToInt32(btnChon.Tag) - 1;
            grdTKhoan.SelectedItem = (DataRowView)grdTKhoan.Items[iRow];
            DataRowView dr = (DataRowView)grdTKhoan.Items.CurrentItem;

            int ID = Convert.ToInt32(dr["ID"]);
            string maKyHieu = dr["MA_KY_HIEU"].ToString();
            string maPhanLoai = "";
            if (ClientInformation.PhuongPhapHachToan.Equals(DatabaseConstant.PHUONG_PHAP_HACH_TOAN.DOC_LAP.layGiaTri()))
                maPhanLoai = dr["MA_PLOAI"].ToString();
            else
                maPhanLoai = dr["MA_PLOAI_BSO"].ToString();
            List<string> lstDieuKien = new List<string>();            
            lstDieuKien.Add(maKyHieu);
            lstDieuKien.Add(maPhanLoai);
            lstDieuKien.Add(ClientInformation.MaDonVi);

            var process = new PopupProcess();
            lstPopup.Clear();
            process.getPopupInformation("POPUP_TAI_KHOAN_HACH_TOAN", lstDieuKien);

            SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;

            ucPopup popup = new ucPopup(false, simplePopupResponse, false);
            popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
            Window win = new Window();
            win.Title = LLanguage.SearchResourceByKey("U.POPUP.POPUP_KT_PLOAI");
            win.Content = popup;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.ShowDialog();
            if (lstPopup != null && lstPopup.Count>0)
            {
                DataRow row = lstPopup[0];
                if (maKyHieu.Equals(BusinessConstant.MA_KY_HIEU.TIENMAT.layGiaTri()))
                {
                    if (ClientInformation.PhuongPhapHachToan.Equals(DatabaseConstant.PHUONG_PHAP_HACH_TOAN.DOC_LAP.layGiaTri()))
                    {
                        dr["MA_PLOAI"] = row[2].ToString();
                        dr["TEN_PLOAI"] = row[3].ToString();
                    }
                    else
                    {
                        dr["MA_PLOAI_BSO"] = row[2].ToString();
                        dr["TEN_PLOAI_BSO"] = row[3].ToString();
                    }
                }
                else
                {
                    dr["MA_PLOAI"] = row[2].ToString();
                    dr["TEN_PLOAI"] = row[3].ToString();
                    dr["MA_PLOAI_BSO"] = row[2].ToString();
                    dr["TEN_PLOAI_BSO"] = row[3].ToString();
                }
                grdTKhoan.CurrentItem = dr;

                for (int i = 0; i < dsTaiKhoanHachToan.Tables[0].Rows.Count; i++)
                {
                    if ((int)dsTaiKhoanHachToan.Tables[0].Rows[i]["ID"] == ID)
                    {
                        dsTaiKhoanHachToan.Tables[0].Rows[i]["MA_PLOAI"] = dr["MA_PLOAI"].ToString();
                        dsTaiKhoanHachToan.Tables[0].Rows[i]["TEN_PLOAI"] = dr["TEN_PLOAI"].ToString();
                        dsTaiKhoanHachToan.Tables[0].Rows[i]["MA_PLOAI_BSO"] = dr["MA_PLOAI_BSO"].ToString();
                        dsTaiKhoanHachToan.Tables[0].Rows[i]["TEN_PLOAI_BSO"] = dr["TEN_PLOAI_BSO"].ToString();
                        break;
                    }
                }
            }
        }

        private void grdTKhoan_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F3)
            {
                DataRowView dr = (DataRowView)grdTKhoan.Items.CurrentItem;

                string maKyHieu = dr["MA_KY_HIEU"].ToString();
                string maPhanLoai = "";
                if (ClientInformation.PhuongPhapHachToan.Equals(DatabaseConstant.PHUONG_PHAP_HACH_TOAN.DOC_LAP.layGiaTri()))
                    maPhanLoai = dr["MA_PLOAI"].ToString();
                else
                    maPhanLoai = dr["MA_PLOAI_BSO"].ToString();
                List<string> lstDieuKien = new List<string>();
                lstDieuKien.Add(maKyHieu);
                lstDieuKien.Add(maPhanLoai);
                lstDieuKien.Add(ClientInformation.MaDonVi);

                var process = new PopupProcess();
                lstPopup.Clear();
                process.getPopupInformation("POPUP_TAI_KHOAN_HACH_TOAN", lstDieuKien);

                SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;

                ucPopup popup = new ucPopup(false, simplePopupResponse, false);
                popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
                Window win = new Window();
                win.Title = LLanguage.SearchResourceByKey("U.POPUP.POPUP_KT_PLOAI");
                win.Content = popup;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.ShowDialog();
                if (lstPopup != null && lstPopup.Count>0)
                {
                    DataRow row = lstPopup[0];

                    if (maKyHieu.Equals(BusinessConstant.MA_KY_HIEU.TIENMAT))
                    {
                        if (ClientInformation.PhuongPhapHachToan.Equals(DatabaseConstant.PHUONG_PHAP_HACH_TOAN.DOC_LAP.layGiaTri()))
                        {
                            dr["MA_PLOAI"] = row[2].ToString();
                            dr["TEN_PLOAI"] = row[3].ToString();
                        }
                        else
                        {
                            dr["MA_PLOAI_BSO"] = row[2].ToString();
                            dr["TEN_PLOAI_BSO"] = row[3].ToString();
                        }
                    }
                    else
                    {
                        dr["MA_PLOAI"] = row[2].ToString();
                        dr["TEN_PLOAI"] = row[3].ToString();
                        dr["MA_PLOAI_BSO"] = row[2].ToString();
                        dr["TEN_PLOAI_BSO"] = row[3].ToString();
                    }

                    grdTKhoan.CurrentItem = dr;

                    for (int i = 0; i < dsTaiKhoanHachToan.Tables[0].Rows.Count; i++)
                    {
                        if ((int)dsTaiKhoanHachToan.Tables[0].Rows[i]["ID"] == ID)
                        {
                            dsTaiKhoanHachToan.Tables[0].Rows[i]["MA_PLOAI"] = dr["MA_PLOAI"].ToString();
                            dsTaiKhoanHachToan.Tables[0].Rows[i]["TEN_PLOAI"] = dr["TEN_PLOAI"].ToString();
                            dsTaiKhoanHachToan.Tables[0].Rows[i]["MA_PLOAI_BSO"] = dr["MA_PLOAI_BSO"].ToString();
                            dsTaiKhoanHachToan.Tables[0].Rows[i]["TEN_PLOAI_BSO"] = dr["TEN_PLOAI_BSO"].ToString();
                            break;
                        }
                    }
                }
            }
        }

        private void btnThemPhi_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var process = new PopupProcess();
                List<string> lstDieuKien = new List<string>();
                lstDieuKien.Add(ClientInformation.MaDonVi);
                process.getPopupInformation("POPUP_DS_BIEU_PHI", lstDieuKien);

                SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;

                lstPopup.Clear();
                ucPopup popup = new ucPopup(false, simplePopupResponse, false);
                popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
                Window win = new Window();
                win.Title = LLanguage.SearchResourceByKey("MENU.2312_PHI_DS");
                win.Content = popup;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.ShowDialog();
                if (lstPopup != null)
                {
                    DataRow row = lstPopup[0];
                    int idPhi = 0;
                    string maPhi = "";
                    string tenPhi = "";

                    if (!string.IsNullOrWhiteSpace(row[1].ToString()))
                        idPhi = Convert.ToInt32(row[1]);
                    if (!string.IsNullOrWhiteSpace(row[2].ToString()))
                        maPhi = row[2].ToString();
                    if (!string.IsNullOrWhiteSpace(row[3].ToString()))
                        tenPhi = row[3].ToString();

                    dsPhi.Tables[0].Rows.Add((dsPhi.Tables[0].Rows.Count + 1), idPhi, maPhi, tenPhi);
                    DataViewManager dataViewManager = new DataViewManager(dsPhi);
                    DataView dataView = dataViewManager.CreateDataView(dsPhi.Tables[0]);
                    grdTTPhiTK.DataContext = dataView;
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        private void btnXoaPhi_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<int> lstSTT = new List<int>();
                for (int i = 0; i < grdTTPhiTK.SelectedItems.Count; i++)
                {
                    DataRowView dr = (DataRowView)grdTTPhiTK.SelectedItems[i];
                    lstSTT.Add(Convert.ToInt32(dr["STT"]));
                }
                lstSTT.SortByDesc();
                foreach (int stt in lstSTT)
                    dsPhi.Tables[0].Rows.RemoveAt(stt - 1);

                for (int i = 0; i < dsPhi.Tables[0].Rows.Count; i++)
                {
                    dsPhi.Tables[0].Rows[i]["STT"] = i + 1;
                }

                DataViewManager dataViewManager = new DataViewManager(dsPhi);
                DataView dataView = dataViewManager.CreateDataView(dsPhi.Tables[0]);
                grdTTPhiTK.DataContext = dataView;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        private void btnMaPhi_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var process = new PopupProcess();
                List<string> lstDieuKien = new List<string>();
                lstDieuKien.Add(ClientInformation.MaDonVi);
                process.getPopupInformation("POPUP_DS_BIEU_PHI", lstDieuKien);

                SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;

                lstPopup.Clear();
                ucPopup popup = new ucPopup(false, simplePopupResponse, false);
                popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
                Window win = new Window();
                win.Title = LLanguage.SearchResourceByKey("MENU.2312_PHI_DS");
                win.Content = popup;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.ShowDialog();
                if (lstPopup != null)
                {
                    DataRow row = lstPopup[0];

                    lblIDPhi.Content = row[1].ToString();
                    txtTTMaPhi.Text = row[2].ToString();
                    lblTenPhi.Content = row[3].ToString();
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        private void grdTTPhiTK_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F3)
            {
                btnThemPhi_Click(null, null);
            }
        }

        private void chkChoPhepTTTH_Checked(object sender, RoutedEventArgs e)
        {
            txtLaiSuatAD.IsEnabled = true;
            btnLaiSuatAD.IsEnabled = true;
        }

        private void chkChoPhepTTTH_UnChecked(object sender, RoutedEventArgs e)
        {
            txtLaiSuatAD.IsEnabled = false;
            btnLaiSuatAD.IsEnabled = false;
        }

        #endregion

        #region Xu ly nghiệp vụ

        private void GetFormData(ref BL_SAN_PHAM obj, ref List<BL_SAN_PHAM_CT> lstCT, ref List<KT_PHAN_HE_PLOAI> lstPHPL, string sTrangThaiNVu)
        {
            try
            {
                if (lstSourceNhomSP == null) return;

                string maNhomSP = lstSourceNhomSP.ElementAt(cmbNhomSP.SelectedIndex).KeywordStrings.ElementAt(0);

                if (action == DatabaseConstant.Action.SUA)
                    obj.ID = id;

                #region Lấy thông tin trên tab thông tin chung
                obj.ID_LSUAT = idLaiSuat;
                obj.ID_CS_TLAI = int.Parse(lstSourceCoSoTLai.ElementAt(cmbCoSoTinhLai.SelectedIndex).KeywordStrings.ElementAt(1));
                obj.MA_NHOM_SP = lstSourceNhomSP.ElementAt(cmbNhomSP.SelectedIndex).KeywordStrings.ElementAt(0);
                obj.MA_SAN_PHAM = txtMaSanPham.Text.Trim();
                obj.TEN_SAN_PHAM = txtTenSanPham.Text;
                obj.MA_LOAI_TIEN = lstSourceLoaiTien.ElementAt(cmbLoaiTien.SelectedIndex).KeywordStrings.ElementAt(0);
                obj.TLAI_HTHUC = lstSourceHinhThucTLai.ElementAt(cmbHinhThuc.SelectedIndex).KeywordStrings.ElementAt(0);
                obj.TLAI_TSUAT = Convert.ToInt32(numTanSuatTraLai.Value);
                if(cmbTanSuatTraLai.SelectedIndex>=0)
                    obj.TLAI_DVTINH = lstSourceTanSuatTraLai.ElementAt(cmbTanSuatTraLai.SelectedIndex).KeywordStrings.ElementAt(0);                
                if(numKyHan.Value != null)
                    obj.KY_HAN = (int?)numKyHan.Value;
                if(cmbKyHan.SelectedIndex>=0)
                    obj.KY_HAN_DVTINH = lstSourceKyHan.ElementAt(cmbKyHan.SelectedIndex).KeywordStrings.ElementAt(0);

                obj.LSUAT_TCHAT = lstSourceLoaiLS.ElementAt(cmbLoaiLS.SelectedIndex).KeywordStrings.ElementAt(0);
                obj.LSUAT_MA = txtMaLaiSuat.Text;
                obj.LSUAT_CSO_TLAI = lstSourceCoSoTLai.ElementAt(cmbCoSoTinhLai.SelectedIndex).KeywordStrings.ElementAt(0);
                obj.LSUAT_BIEN_DO = Convert.ToDecimal(numBienDoLS.Value);
                obj.LSUAT_SO_DKY_DGIA = Convert.ToInt32(numDinhKyLS.Value);
                if (cmbDinhKyDGiaLai.SelectedIndex >= 0)
                    obj.LSUAT_DVTINH_DGIA = lstSourceDinhKyDGiaLai.ElementAt(cmbDinhKyDGiaLai.SelectedIndex).KeywordStrings.ElementAt(0);
                obj.LSUAT_TDOI_BTHANG = (chkLSThayDoi.IsChecked == true ? BusinessConstant.CoKhong.CO.layGiaTri() : BusinessConstant.CoKhong.KHONG.layGiaTri());

                obj.NGAY_ADUNG = ((DateTime)raddtNgayHL.Value).ToString("yyyyMMdd");
                if(raddtNgayHetHL.Value != null || !raddtNgayHetHL.Text.Equals("__/__/____"))
                    obj.NGAY_HHAN = ((DateTime)raddtNgayHetHL.Value).ToString("yyyyMMdd");
                obj.SO_DU_TTHIEU = Convert.ToDecimal(numSoDuToiThieu.Value);
                if (numSoDuToiDa.Value != null)
                    obj.SO_DU_TDA = Convert.ToDecimal(numSoDuToiDa.Value);
                else
                    obj.SO_DU_TDA = null;
                obj.SO_DU_TTHIEU_TLAI = Convert.ToDecimal(numSoDuToiThieuTinhLai.Value);
                obj.SO_DU_TTHIEU_RGOC = Convert.ToDecimal(numSoDuToiThieuRutGoc1Phan.Value);
                #endregion

                #region Lấy thông tin trên tab tài khoản tiền gửi có kỳ hạn
                //set Giá trị mặc định cho TRHAN_TTOAN, TRHAN_RGOC, TRHAN_MA_LSUAT
                obj.TRHAN_TTOAN = BusinessConstant.CoKhong.KHONG.layGiaTri();
                obj.TRHAN_RGOC = BusinessConstant.CoKhong.KHONG.layGiaTri();
                obj.TRHAN_MA_LSUAT = "";
                if (maNhomSP.Equals(BusinessConstant.layGiaTri(BusinessConstant.NHOM_SPHAM_TKIEM.T02)) || maNhomSP.Equals(BusinessConstant.layGiaTri(BusinessConstant.NHOM_SPHAM_TKIEM.T03)) || maNhomSP.Equals(BusinessConstant.layGiaTri(BusinessConstant.NHOM_SPHAM_TKIEM.T04)) || maNhomSP.Equals(BusinessConstant.layGiaTri(BusinessConstant.NHOM_SPHAM_TKIEM.T05)) || maNhomSP.Equals(BusinessConstant.layGiaTri(BusinessConstant.NHOM_SPHAM_TKIEM.T06)) || maNhomSP.Equals("TGCKH"))
                {
                    obj.PPHAP_TLAI_GGOP = radLaiDon.IsChecked == true ? "LAI_DON" : "LAI_KEP";
                    obj.TRHAN_TTOAN = chkChoPhepTTTH.IsChecked == true ? BusinessConstant.CoKhong.CO.layGiaTri() : BusinessConstant.CoKhong.KHONG.layGiaTri();
                    obj.TRHAN_RGOC = chkChoPhepRGTH.IsChecked == true ? BusinessConstant.CoKhong.CO.layGiaTri() : BusinessConstant.CoKhong.KHONG.layGiaTri();
                    obj.TRHAN_MA_LSUAT = txtLaiSuatAD.Text;
                }
                #endregion

                #region Lấy thông tin trên tab tiết kiệm quy định

                //Set giá trị mặc định choRGOC_TLAI_SDU_TT
                obj.RGOC_TLAI_SDU_TT = BusinessConstant.CoKhong.KHONG.layGiaTri();
                if (maNhomSP.Equals(BusinessConstant.layGiaTri(BusinessConstant.NHOM_SPHAM_TKIEM.T01)))
                {
                    obj.RGOC_PPHAP = radTuyetDoi.IsChecked == true ? "TUYET_DOI" : "TUONG_DOI";
                    obj.RGOC_STIEN = Convert.ToDecimal(numSoTienTuyetDoi.Value);
                    obj.RGOC_TY_LE = Convert.ToDecimal(numTyLeTuongDoi.Value);
                    if (cmbTyLeTuongDoi.SelectedIndex >= 0)
                        obj.RGOC_CO_SO = lstSourceRutGocTyLeTuongDoi.ElementAt(cmbTyLeTuongDoi.SelectedIndex).KeywordStrings.ElementAt(0);

                    obj.RGOC_TLAI_SDU_TT = chkSoDuToiThieu.IsChecked == true ? BusinessConstant.CoKhong.CO.layGiaTri() : BusinessConstant.CoKhong.KHONG.layGiaTri();
                    obj.TGUI_PPHAP = radTuyetDoi_TGui.IsChecked == true ? "TUYET_DOI" : "TUONG_DOI";
                    obj.TGUI_TY_LE = Convert.ToDecimal(numTyLe_TGui.Value);
                    if (cmbTyLe.SelectedIndex >= 0)
                        obj.TGUI_CO_SO = lstSourceRutGocTyLeTyLe.ElementAt(cmbTyLe.SelectedIndex).KeywordStrings.ElementAt(0);
                    obj.TGUI_STIEN = Convert.ToDecimal(numTuyetDoi_TGui.Value);
                    obj.TGUI_CTHUC = radTheoKhoang_TGui.IsChecked == true ? BusinessConstant.CoKhong.CO.layGiaTri() : BusinessConstant.CoKhong.KHONG.layGiaTri();

                    #region Lấy thông tin sản phẩm chi tiết
                    //Nếu tiền gửi tương đối và theo khoảng tạo thêm obj chi tiết
                    if (radTuongDoi_TGui.IsChecked == true && radTheoKhoang_TGui.IsChecked == true)
                    {
                        try
                        {
                            if(LObject.IsNullOrEmpty(lstCT)) lstCT = new List<BL_SAN_PHAM_CT>();
                            for (int i = 0; i < grdTGui.Items.Count; i++)
                            {
                                
                                DataRowView dr = (DataRowView)grdTGui.Items[i];
                                if (!dr["STT"].ToString().IsNullOrEmptyOrSpace() && Convert.ToInt32(dr["STT"]) > 0)
                                {
                                    if (!dr[2].ToString().IsNullOrEmptyOrSpace())
                                    {
                                        BL_SAN_PHAM_CT objCT = new BL_SAN_PHAM_CT();

                                        objCT.LOAI_TTIN = "LOAI_TTIN";
                                        objCT.ID_SAN_PHAM = id;
                                        objCT.DU_NO = Convert.ToDecimal(dr[1]);
                                        objCT.SO_TIEN_KY = Convert.ToDecimal(dr[2]);
                                        objCT.TTHAI_BGHI = BusinessConstant.layGiaTri(BusinessConstant.TrangThaiBanGhi.SU_DUNG);
                                        objCT.TTHAI_NVU = BusinessConstant.layGiaTri(BusinessConstant.TrangThaiNghiepVu.CHO_DUYET);
                                        objCT.MA_DVI_TAO = Presentation.Process.Common.ClientInformation.MaDonVi;
                                        objCT.MA_DVI_QLY = Presentation.Process.Common.ClientInformation.MaDonVi;
                                        objCT.NGAY_NHAP = Convert.ToDateTime(raddtNgayLap.Value).ToString("yyyyMMdd");
                                        objCT.NGUOI_NHAP = txtNguoiLap.Text;
                                        objCT.NGAY_CNHAT = ClientInformation.NgayLamViecHienTai;
                                        objCT.NGUOI_CNHAT = ClientInformation.TenDangNhap;

                                        lstCT.Add(objCT);
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                            throw ex;
                        }
                    }
                    else
                    {
                        lstCT = null;
                    }

                    #endregion
                }
                else lstCT = null;
                #endregion

                #region Lấy thông tin trên tab tiền gửi thanh toán
                if (maNhomSP.Equals(BusinessConstant.layGiaTri(BusinessConstant.NHOM_SPHAM_TKIEM.T08)))
                {
                    obj.TT_SDU_TTHIEU = chkTTChoPhep.IsChecked == true ? BusinessConstant.CoKhong.CO.layGiaTri() : BusinessConstant.CoKhong.KHONG.layGiaTri();
                    obj.TT_SDU_TTHIEU_PHI = chkTTCoPhi.IsChecked == true ? BusinessConstant.CoKhong.CO.layGiaTri() : BusinessConstant.CoKhong.KHONG.layGiaTri();
                    obj.TT_SDU_TTHIEU_PP = radTTSoDuThoiDiem.IsChecked == true ? "SDU_HIEN_TAI" : "SDU_BQUAN_THANG";
                    obj.TT_MA_BPHI = txtTTMaPhi.Text;
                    obj.TC_THAU_CHI = chkTTChoPhepThauChi.IsChecked == true ? BusinessConstant.CoKhong.CO.layGiaTri() : BusinessConstant.CoKhong.KHONG.layGiaTri();
                    obj.TC_TOI_DA = Convert.ToInt32(numTC_ToiDa.Value);

                    if (dsPhi != null && dsPhi.Tables[0].Rows.Count > 0)
                    {
                        lstCT = new List<BL_SAN_PHAM_CT>();
                        foreach (DataRow dr in dsPhi.Tables[0].Rows)
                        {
                            BL_SAN_PHAM_CT objCT = new BL_SAN_PHAM_CT();
                            //objCT.ID = 
                            objCT.ID_SAN_PHAM = ID;
                            objCT.LOAI_TTIN = "LOAI_TTIN";
                            objCT.MA_BPHI = dr["MA_BPHI"].ToString();
                            //objCT.DU_NO = 
                            //objCT.SO_TIEN_KY = 
                            //objCT.TY_LE = 
                            objCT.TTHAI_BGHI = BusinessConstant.layGiaTri(BusinessConstant.TrangThaiBanGhi.SU_DUNG);
                            objCT.TTHAI_NVU = BusinessConstant.layGiaTri(BusinessConstant.TrangThaiNghiepVu.CHO_DUYET);
                            objCT.MA_DVI_TAO = Presentation.Process.Common.ClientInformation.MaDonVi;
                            objCT.MA_DVI_QLY = Presentation.Process.Common.ClientInformation.MaDonVi;
                            objCT.NGAY_NHAP = Convert.ToDateTime(raddtNgayLap.Value).ToString("yyyyMMdd");
                            objCT.NGUOI_NHAP = txtNguoiLap.Text;
                            objCT.NGAY_CNHAT = ClientInformation.NgayLamViecHienTai;
                            objCT.NGUOI_CNHAT = ClientInformation.TenDangNhap;

                            lstCT.Add(objCT);
                        }
                    }
                }
                #endregion

                #region Thông tin khác
                string tinhChatKyHan = "";
                string rutGoc1Phan = "";
                string coSoTinhLaiRutGoc1Phan = "";
                string guiThem = "";
                TaoThongTinNghiepVu(ref tinhChatKyHan, ref rutGoc1Phan, ref coSoTinhLaiRutGoc1Phan, ref guiThem);
                obj.TCHAT_KY_HAN = tinhChatKyHan;
                obj.RGOC_1PHAN = rutGoc1Phan;
                obj.RGOC_CO_SO_TLAI = coSoTinhLaiRutGoc1Phan;
                obj.TGUI_THEM = guiThem;
                #endregion

                #region Thông tin kiểm soát
                obj.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                obj.TTHAI_NVU = sTrangThaiNVu;
                obj.MA_DVI_TAO = Presentation.Process.Common.ClientInformation.MaDonVi;
                obj.MA_DVI_QLY = Presentation.Process.Common.ClientInformation.MaDonVi;
                obj.NGAY_NHAP = Convert.ToDateTime(raddtNgayLap.Value).ToString("yyyyMMdd");
                obj.NGUOI_NHAP = txtNguoiLap.Text;
                obj.NGAY_CNHAT = ClientInformation.NgayLamViecHienTai;
                obj.NGUOI_CNHAT = ClientInformation.TenDangNhap;
                #endregion

                #region Lấy thông tin trên tab tài khoản hạch toán
                foreach (DataRow dr in dsTaiKhoanHachToan.Tables[0].Rows)
                {
                    KT_PHAN_HE_PLOAI objPHPL = new KT_PHAN_HE_PLOAI();
                    if (action == DatabaseConstant.Action.SUA)
                        objPHPL.ID = Convert.ToInt32(dr["ID"]);
                    objPHPL.ID_PHAN_HE = Convert.ToInt32(dr["ID_PHAN_HE"]);
                    objPHPL.MA_PHAN_HE = DatabaseConstant.Module.HDVO.getValue();
                    objPHPL.MA_DTUONG = txtMaSanPham.Text;
                    objPHPL.MA_KY_HIEU = dr["MA_KY_HIEU"].ToString();
                    objPHPL.MA_PLOAI = dr["MA_PLOAI"].ToString();
                    objPHPL.MA_PLOAI_BSO = dr["MA_PLOAI_BSO"].ToString();
                    objPHPL.TTHAI_BGHI = obj.TTHAI_BGHI;
                    objPHPL.TTHAI_NVU = obj.TTHAI_NVU;
                    objPHPL.MA_DVI_QLY = obj.MA_DVI_QLY;
                    objPHPL.MA_DVI_TAO = obj.MA_DVI_TAO;
                    objPHPL.NGAY_NHAP = obj.NGAY_NHAP;
                    objPHPL.NGUOI_NHAP = obj.NGUOI_NHAP;
                    objPHPL.NGAY_CNHAT = obj.NGAY_CNHAT;
                    objPHPL.NGUOI_CNHAT = obj.NGUOI_CNHAT;

                    lstPHPL.Add(objPHPL);
                }                
                #endregion
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        private void SetFormData()
        {
            try
            {
                HuyDongVonProcess huyDongVonProcess = new HuyDongVonProcess();
                DataSet ds = huyDongVonProcess.GetThongTinSanPham(id);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    obj = new BL_SAN_PHAM();
                    obj.ID = id;
                    
                    sTrangThaiNVu = dr["TTHAI_NVU"].ToString();
                    string maNhomSP = dr["MA_NHOM_SP"].ToString();

                    #region Hiển thị tab thông tin chung
                    //Trạng thái
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);

                    //Group Thông tin sản phẩm                   
                    cmbNhomSP.SelectedIndex = lstSourceNhomSP.IndexOf(lstSourceNhomSP.FirstOrDefault(i => i.KeywordStrings.First().Equals(dr["MA_NHOM_SP"].ToString())));
                    txtMaSanPham.Text = dr["MA_SAN_PHAM"].ToString();
                    txtTenSanPham.Text = dr["TEN_SAN_PHAM"].ToString();
                    cmbHinhThuc.SelectedIndex = lstSourceHinhThucTLai.IndexOf(lstSourceHinhThucTLai.FirstOrDefault(i => i.KeywordStrings.First().Equals(dr["TLAI_HTHUC"].ToString())));
                    numTanSuatTraLai.Value = Convert.ToDouble(dr["TLAI_TSUAT"]);
                    cmbTanSuatTraLai.SelectedIndex = lstSourceTanSuatTraLai.IndexOf(lstSourceTanSuatTraLai.FirstOrDefault(i => i.KeywordStrings.First().Equals(dr["TLAI_DVTINH"].ToString())));
                    cmbLoaiTien.SelectedIndex = lstSourceLoaiTien.IndexOf(lstSourceLoaiTien.FirstOrDefault(i => i.KeywordStrings.First().Equals(dr["MA_LOAI_TIEN"].ToString())));
                    if(!dr["KY_HAN"].ToString().IsNullOrEmptyOrSpace())
                        numKyHan.Value = Convert.ToDouble(dr["KY_HAN"]);
                    cmbKyHan.SelectedIndex = lstSourceKyHan.IndexOf(lstSourceTanSuatTraLai.FirstOrDefault(i => i.KeywordStrings.First().Equals(dr["KY_HAN_DVTINH"].ToString())));

                    //Group Thông tin lãi suất                    
                    cmbLoaiLS.SelectedIndex = lstSourceLoaiLS.IndexOf(lstSourceLoaiLS.FirstOrDefault(i => i.KeywordStrings.First().Equals(dr["LSUAT_TCHAT"].ToString())));
                    idLaiSuat = Convert.ToInt32(dr["ID_LSUAT"]);
                    txtMaLaiSuat.Text = dr["LSUAT_MA"].ToString();
                    lblTenLaiSuat.Content = dr["LSUAT_TEN"].ToString();
                    numBienDoLS.Value = Convert.ToDouble(dr["LSUAT_BIEN_DO"]);
                    numDinhKyLS.Value = Convert.ToDouble(dr["LSUAT_SO_DKY_DGIA"]);
                    cmbDinhKyDGiaLai.SelectedIndex = lstSourceDinhKyDGiaLai.IndexOf(lstSourceDinhKyDGiaLai.FirstOrDefault(i => i.KeywordStrings.First().Equals(dr["LSUAT_DVTINH_DGIA"].ToString())));
                    chkLSThayDoi.IsChecked = dr["LSUAT_TDOI_BTHANG"].ToString().Equals(BusinessConstant.CoKhong.CO.layGiaTri()) ? true : false;
                    idCoSoTinhLai = Convert.ToInt32(dr["ID_CS_TLAI"]);
                    cmbCoSoTinhLai.SelectedIndex = lstSourceCoSoTLai.IndexOf(lstSourceCoSoTLai.FirstOrDefault(i => i.KeywordStrings.First().Equals(dr["LSUAT_CSO_TLAI"].ToString())));

                    //Group Thông tin khác
                    if (!dr["SO_DU_TTHIEU"].ToString().IsNullOrEmptyOrSpace())
                        numSoDuToiThieu.Value = Convert.ToDouble(dr["SO_DU_TTHIEU"]);
                    if (!dr["SO_DU_TDA"].ToString().IsNullOrEmptyOrSpace())
                        numSoDuToiDa.Value = Convert.ToDouble(dr["SO_DU_TDA"]);
                    else
                        numSoDuToiDa.Value = null;
                    if (LDateTime.IsDate(dr["NGAY_ADUNG"].ToString(), "yyyyMMdd"))
                    {
                        raddtNgayHL.Value = LDateTime.StringToDate(dr["NGAY_ADUNG"].ToString(), "yyyyMMdd");
                    }
                    else
                    {
                        raddtNgayHL.Value = null;
                    }
                    if (LDateTime.IsDate(dr["NGAY_HHAN"].ToString(), "yyyyMMdd"))
                    {
                        raddtNgayHetHL.Value = LDateTime.StringToDate(dr["NGAY_HHAN"].ToString(), "yyyyMMdd");
                    }
                    else
                    {
                        raddtNgayHetHL.Value = null;
                    }

                    if (!dr["SO_DU_TTHIEU_TLAI"].ToString().IsNullOrEmptyOrSpace())
                        numSoDuToiThieuTinhLai.Value = Convert.ToDouble(dr["SO_DU_TTHIEU_TLAI"]);
                    if (!dr["SO_DU_TTHIEU_RGOC"].ToString().IsNullOrEmptyOrSpace())
                        numSoDuToiThieuRutGoc1Phan.Value = Convert.ToDouble(dr["SO_DU_TTHIEU_RGOC"]);
                    #endregion

                    #region Hiển thị tab Tài khoản(tiền gửi) có kỳ hạn


                    if (maNhomSP.Equals(BusinessConstant.layGiaTri(BusinessConstant.NHOM_SPHAM_TKIEM.T02)) || maNhomSP.Equals(BusinessConstant.layGiaTri(BusinessConstant.NHOM_SPHAM_TKIEM.T03)) || maNhomSP.Equals(BusinessConstant.layGiaTri(BusinessConstant.NHOM_SPHAM_TKIEM.T04)) || maNhomSP.Equals(BusinessConstant.layGiaTri(BusinessConstant.NHOM_SPHAM_TKIEM.T05)) || maNhomSP.Equals(BusinessConstant.layGiaTri(BusinessConstant.NHOM_SPHAM_TKIEM.T06)) || maNhomSP.Equals("TGCKH"))
                    {
                        radLaiDon.IsChecked = (dr["PPHAP_TLAI_GGOP"].ToString() == "LAI_DON") ? true : false;
                        chkChoPhepTTTH.IsChecked = (dr["TRHAN_TTOAN"].ToString() == BusinessConstant.CoKhong.CO.layGiaTri()) ? true : false;
                        chkChoPhepRGTH.IsChecked = (dr["TRHAN_RGOC"].ToString() == BusinessConstant.CoKhong.CO.layGiaTri()) ? true : false;
                        txtLaiSuatAD.Text = dr["TRHAN_MA_LSUAT"].ToString();
                    }
                    #endregion

                    #region Hiển thị tab Tiết kiệm quy định

                    if (maNhomSP.Equals(BusinessConstant.layGiaTri(BusinessConstant.NHOM_SPHAM_TKIEM.T01)))
                    {

                        LoadGridTheoKhoangTKQD();

                        radTuyetDoi.IsChecked = (dr["RGOC_PPHAP"].ToString() == "TUYET_DOI") ? true : false;
                        radTuongDoi.IsChecked = !radTuyetDoi.IsChecked;
                        numSoTienTuyetDoi.Value = Convert.ToDouble(dr["RGOC_STIEN"]);
                        numTyLeTuongDoi.Value = Convert.ToDouble(dr["RGOC_TY_LE"]);
                        if (dr["RGOC_CO_SO"].ToString().IsNullOrEmptyOrSpace())
                            cmbTyLeTuongDoi.SelectedIndex = lstSourceRutGocTyLeTuongDoi.IndexOf(lstSourceRutGocTyLeTuongDoi.FirstOrDefault(i => i.KeywordStrings.First().Equals(dr["RGOC_CO_SO"].ToString())));

                        chkSoDuToiThieu.IsChecked = (dr["RGOC_TLAI_SDU_TT"].ToString() == BusinessConstant.CoKhong.CO.layGiaTri()) ? true : false;

                        radTuyetDoi_TGui.IsChecked = (dr["TGUI_PPHAP"].ToString() == "TUYET_DOI") ? true : false;
                        radTuongDoi_TGui.IsChecked = !radTuyetDoi_TGui.IsChecked;
                        numTuyetDoi_TGui.Value = Convert.ToDouble(dr["TGUI_STIEN"]);
                        radTheoKhoang_TGui.IsChecked = (dr["TGUI_CTHUC"].ToString() == BusinessConstant.CoKhong.CO.layGiaTri()) ? true : false;
                        numTyLe_TGui.Value = Convert.ToDouble(dr["TGUI_TY_LE"]);

                        if (dr["TGUI_CO_SO"].ToString().IsNullOrEmptyOrSpace())
                            cmbTyLe.SelectedIndex = lstSourceRutGocTyLeTyLe.IndexOf(lstSourceRutGocTyLeTyLe.FirstOrDefault(i => i.KeywordStrings.First().Equals(dr["TGUI_CO_SO"].ToString())));
                    }
                    #endregion

                    #region Hiển thị tab Tiền gửi thanh toán

                    if (maNhomSP.Equals(BusinessConstant.layGiaTri(BusinessConstant.NHOM_SPHAM_TKIEM.T08)))
                    {

                        chkTTChoPhep.IsChecked = (dr["TT_SDU_TTHIEU"].ToString() == BusinessConstant.CoKhong.CO.layGiaTri()) ? true : false;
                        chkTTCoPhi.IsChecked = (dr["TT_SDU_TTHIEU_PHI"].ToString() == BusinessConstant.CoKhong.CO.layGiaTri()) ? true : false;
                        radTTSoDuThoiDiem.IsChecked = (dr["TT_SDU_TTHIEU_PP"].ToString() == "SDU_HIEN_TAI") ? true : false;
                        radTTSoDuBinhQuan.IsChecked = !radTTSoDuThoiDiem.IsChecked;
                        //txtTTMaPhi.Text = dr["TT_MA_BPHI"].ToString();
                        //chkTTChoPhepThauChi.IsChecked = (dr["TC_THAU_CHI"].ToString() == BusinessConstant.CoKhong.CO.layGiaTri()) ? true : false;
                        //numTC_ToiDa.Value = Convert.ToDouble(dr["TC_TOI_DA"]);

                        dsPhi = huyDongVonProcess.GetBieuPhi(id);
                        if (dsPhi != null && dsPhi.Tables[0].Rows.Count > 0)
                        {
                            DataViewManager dataViewManager = new DataViewManager(dsPhi);
                            DataView dataView = dataViewManager.CreateDataView(dsPhi.Tables[0]);
                            grdTTPhiTK.DataContext = dataView;
                        }
                    }
                    #endregion

                    #region Hiển thị tab tài khoản hạch toán
                    LoadGridTaiKhoanHachToan();
                    #endregion

                    #region Tab thông tin kiểm soát
                    txtTrangThaiBanGhi.Text = BusinessConstant.layNgonNguSuDung(dr["TTHAI_BGHI"].ToString());
                    raddtNgayLap.Value = LDateTime.StringToDate(dr["NGAY_NHAP"].ToString(), "yyyyMMdd");
                    txtNguoiLap.Text = dr["NGUOI_NHAP"].ToString();
                    if (LDateTime.IsDate(dr["NGAY_CNHAT"].ToString(), "yyyyMMdd") == true)
                        raddtNgayCapNhat.Value = LDateTime.StringToDate(dr["NGAY_CNHAT"].ToString(), "yyyyMMdd");
                    else
                        raddtNgayCapNhat.Value = null;
                    txtNguoiCapNhat.Text = dr["NGUOI_CNHAT"].ToString();
                    #endregion
                }
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void TaoThongTinNghiepVu(ref string tinhChatKyHan, ref string rutGoc1Phan, ref string coSoTinhLaiRutGoc1Phan, ref string guiThem)
        {
            string maNhomSP = lstSourceNhomSP.ElementAt(cmbNhomSP.SelectedIndex).KeywordStrings.ElementAt(0);
            UtilitiesProcess processUtilities = new UtilitiesProcess();
            
            if (maNhomSP.Equals(BusinessConstant.layGiaTri(BusinessConstant.NHOM_SPHAM_TKIEM.T01)))
            {
                tinhChatKyHan = processUtilities.LayGiaTriThamSo(BusinessConstant.LoaiThamSo.TW, BusinessConstant.MaThamSo.TW_TCHAT_KY_HAN_T01, null);
                if(tinhChatKyHan.IsNullOrEmptyOrSpace())
                    tinhChatKyHan = BusinessConstant.TINH_CHAT_KY_HAN.KHONG_KY_HAN_TU_NGUYEN.layGiaTri();
                
                rutGoc1Phan = BusinessConstant.CoKhong.CO.layGiaTri();
                coSoTinhLaiRutGoc1Phan = processUtilities.LayGiaTriThamSo(BusinessConstant.LoaiThamSo.DV, BusinessConstant.MaThamSo.DV_SO_DU_TINH_LAI_KHI_RUT_GOC, ClientInformation.MaDonVi);
                guiThem = BusinessConstant.CoKhong.CO.layGiaTri();
            }
            else if (maNhomSP.Equals(BusinessConstant.layGiaTri(BusinessConstant.NHOM_SPHAM_TKIEM.T02)))
            {
                tinhChatKyHan = BusinessConstant.TINH_CHAT_KY_HAN.KHONG_KY_HAN_TU_NGUYEN.layGiaTri();
                rutGoc1Phan = BusinessConstant.CoKhong.CO.layGiaTri();
                coSoTinhLaiRutGoc1Phan = processUtilities.LayGiaTriThamSo(BusinessConstant.LoaiThamSo.DV, BusinessConstant.MaThamSo.DV_SO_DU_TINH_LAI_KHI_RUT_GOC, ClientInformation.MaDonVi);
                guiThem = BusinessConstant.CoKhong.CO.layGiaTri();
            }
            else if (maNhomSP.Equals(BusinessConstant.layGiaTri(BusinessConstant.NHOM_SPHAM_TKIEM.T03)))
            {
                tinhChatKyHan = BusinessConstant.TINH_CHAT_KY_HAN.CO_KY_HAN_TU_NGUYEN.layGiaTri();
                rutGoc1Phan = BusinessConstant.CoKhong.KHONG.layGiaTri();
                coSoTinhLaiRutGoc1Phan = processUtilities.LayGiaTriThamSo(BusinessConstant.LoaiThamSo.DV, BusinessConstant.MaThamSo.DV_SO_DU_TINH_LAI_KHI_RUT_GOC, ClientInformation.MaDonVi);
                guiThem = BusinessConstant.CoKhong.KHONG.layGiaTri();
            }
            else if (maNhomSP.Equals(BusinessConstant.layGiaTri(BusinessConstant.NHOM_SPHAM_TKIEM.T04)))
            {
                tinhChatKyHan = BusinessConstant.TINH_CHAT_KY_HAN.CO_KY_HAN_TU_NGUYEN.layGiaTri();
                rutGoc1Phan = BusinessConstant.CoKhong.KHONG.layGiaTri();
                coSoTinhLaiRutGoc1Phan = processUtilities.LayGiaTriThamSo(BusinessConstant.LoaiThamSo.DV, BusinessConstant.MaThamSo.DV_SO_DU_TINH_LAI_KHI_RUT_GOC, ClientInformation.MaDonVi);
                guiThem = BusinessConstant.CoKhong.KHONG.layGiaTri();
            }
            else if (maNhomSP.Equals(BusinessConstant.layGiaTri(BusinessConstant.NHOM_SPHAM_TKIEM.T05)))
            {
                tinhChatKyHan = BusinessConstant.TINH_CHAT_KY_HAN.CO_KY_HAN_TU_NGUYEN.layGiaTri();
                rutGoc1Phan = BusinessConstant.CoKhong.KHONG.layGiaTri();
                coSoTinhLaiRutGoc1Phan = processUtilities.LayGiaTriThamSo(BusinessConstant.LoaiThamSo.DV, BusinessConstant.MaThamSo.DV_SO_DU_TINH_LAI_KHI_RUT_GOC, ClientInformation.MaDonVi);
                guiThem = BusinessConstant.CoKhong.KHONG.layGiaTri();
            }
            else if (maNhomSP.Equals(BusinessConstant.layGiaTri(BusinessConstant.NHOM_SPHAM_TKIEM.T06)))
            {
                tinhChatKyHan = BusinessConstant.TINH_CHAT_KY_HAN.CO_KY_HAN_TU_NGUYEN.layGiaTri();
                rutGoc1Phan = BusinessConstant.CoKhong.CO.layGiaTri();
                coSoTinhLaiRutGoc1Phan = processUtilities.LayGiaTriThamSo(BusinessConstant.LoaiThamSo.DV, BusinessConstant.MaThamSo.DV_SO_DU_TINH_LAI_KHI_RUT_GOC, ClientInformation.MaDonVi);
                guiThem = BusinessConstant.CoKhong.CO.layGiaTri();
            }
            else if (maNhomSP.Equals(BusinessConstant.layGiaTri(BusinessConstant.NHOM_SPHAM_TKIEM.T07)))
            {
                tinhChatKyHan = BusinessConstant.TINH_CHAT_KY_HAN.CO_KY_HAN_TU_NGUYEN.layGiaTri();
                rutGoc1Phan = BusinessConstant.CoKhong.KHONG.layGiaTri();
                coSoTinhLaiRutGoc1Phan = processUtilities.LayGiaTriThamSo(BusinessConstant.LoaiThamSo.DV, BusinessConstant.MaThamSo.DV_SO_DU_TINH_LAI_KHI_RUT_GOC, ClientInformation.MaDonVi);
                guiThem = BusinessConstant.CoKhong.KHONG.layGiaTri();
            }
            else if (maNhomSP.Equals(BusinessConstant.layGiaTri(BusinessConstant.NHOM_SPHAM_TKIEM.T08)))
            {
                tinhChatKyHan = BusinessConstant.TINH_CHAT_KY_HAN.KHONG_KY_HAN_TU_NGUYEN.layGiaTri();
                rutGoc1Phan = BusinessConstant.CoKhong.CO.layGiaTri();
                coSoTinhLaiRutGoc1Phan = processUtilities.LayGiaTriThamSo(BusinessConstant.LoaiThamSo.DV, BusinessConstant.MaThamSo.DV_SO_DU_TINH_LAI_KHI_RUT_GOC, ClientInformation.MaDonVi);
                guiThem = BusinessConstant.CoKhong.CO.layGiaTri();
            }
            else
            {
                tinhChatKyHan = "";
                rutGoc1Phan = "";
                coSoTinhLaiRutGoc1Phan = "";
                guiThem = "";
            }            
        }

        private bool Validation()
        {
            try
            {
                if (cmbNhomSP.Text.IsNullOrEmptyOrSpace())
                {
                    CommonFunction.ThongBaoTrong(lblNhomSP.Content.ToString());
                    cmbNhomSP.Focus();
                    return false;
                }                
                else if (txtTenSanPham.Text.IsNullOrEmptyOrSpace())
                {
                    CommonFunction.ThongBaoTrong(lblTenSanPham.Content.ToString());
                    txtTenSanPham.Focus();
                    return false;
                }
                else if (cmbLoaiTien.Text.IsNullOrEmptyOrSpace())
                {
                    CommonFunction.ThongBaoTrong(lblLoaiTien.Content.ToString());
                    cmbLoaiTien.Focus();
                    return false;
                }
                else if (txtMaLaiSuat.Text.IsNullOrEmptyOrSpace())
                {
                    CommonFunction.ThongBaoTrong(lblMaLS.Content.ToString());
                    txtMaLaiSuat.Focus();
                    return false;
                }
                else if (cmbCoSoTinhLai.Text.IsNullOrEmptyOrSpace())
                {
                    CommonFunction.ThongBaoTrong(lblCoSoTinhLai.Content.ToString());
                    cmbCoSoTinhLai.Focus();
                    return false;
                }
                else if (raddtNgayHL.Value == null || !LDateTime.IsDate(raddtNgayHL.Text,"dd/MM/yyyy"))
                {
                    CommonFunction.ThongBaoTrong(lblNgayHL.Content.ToString());
                    raddtNgayHL.Focus();
                    return false;
                }
                else if (LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd") > raddtNgayHL.Value)
                {
                    LMessage.ShowMessage("M.DanhMuc.ucLaiSuatCT.NgayHieuLuc", LMessage.MessageBoxType.Warning);
                    raddtNgayHL.Focus();
                    return false;
                }

                string maNhomSP = lstSourceNhomSP.ElementAt(cmbNhomSP.SelectedIndex).KeywordStrings.ElementAt(0);                
                if (maNhomSP.Equals(BusinessConstant.NHOM_SPHAM_TKIEM.T03.layGiaTri()) || maNhomSP.Equals(BusinessConstant.NHOM_SPHAM_TKIEM.T04.layGiaTri()) || maNhomSP.Equals(BusinessConstant.NHOM_SPHAM_TKIEM.T05.layGiaTri()) || maNhomSP.Equals(BusinessConstant.NHOM_SPHAM_TKIEM.T06.layGiaTri()) || maNhomSP.Equals(BusinessConstant.NHOM_SPHAM_TKIEM.T07.layGiaTri()))
                {
                    if (numKyHan.Value == null || numKyHan.Value.Value < 1 || cmbKyHan.SelectedIndex < 0)
                    {
                        CommonFunction.ThongBaoChuaNhap(lblKyHan.Content.ToString());
                        numKyHan.Focus();
                        return false;
                    }

                    if (chkChoPhepTTTH.IsChecked == true && txtLaiSuatAD.Text.IsNullOrEmptyOrSpace())
                    {
                        CommonFunction.ThongBaoChuaNhap(lblLaiSuatKhiTTTH.Content.ToString() + ":");
                        tbiTaiKhoanCKK.IsSelected = true;
                        txtLaiSuatAD.Focus();
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                return false;
            }
        }


        public void OnHold()
        {
            try
            {
                if (!Validation()) return;

                string trangThai = CommonFunction.LayTrangThaiBanGhi(DatabaseConstant.Action.LUU_TAM, BusinessConstant.layTrangThaiNghiepVu(sTrangThaiNVu));
                obj = new BL_SAN_PHAM();
                lstObjCT = new List<BL_SAN_PHAM_CT>();
                lstPHPL = new List<KT_PHAN_HE_PLOAI>();
                GetFormData(ref obj, ref lstObjCT,ref lstPHPL, trangThai);

                if (action == DatabaseConstant.Action.THEM)
                {
                    OnAddNew(ref obj, ref lstObjCT);
                }
                else if (action == DatabaseConstant.Action.SUA)
                {
                    OnModify(ref obj,ref lstObjCT);
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

                string trangThai = CommonFunction.LayTrangThaiBanGhi(DatabaseConstant.Action.LUU, BusinessConstant.layTrangThaiNghiepVu(sTrangThaiNVu));
                if (sTrangThaiNVu.Equals(BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri()))
                    trangThai = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();

                obj = new BL_SAN_PHAM();
                lstPHPL = new List<KT_PHAN_HE_PLOAI>();
                GetFormData(ref obj, ref lstObjCT, ref lstPHPL, trangThai);

                if (action == DatabaseConstant.Action.THEM)
                {
                    OnAddNew(ref obj, ref lstObjCT);
                }
                else if (action == DatabaseConstant.Action.SUA)
                {
                    OnModify(ref obj, ref lstObjCT);
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
            SetEnabledAllControls(false);
            action = DatabaseConstant.Action.XEM;
            CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain, DatabaseConstant.Function.HDV_SAN_PHAM);            
        }

        public void BeforeViewFromList()
        {
            SetFormData();
            BeforeViewFromDetail();
        }


        public void BeforeAddNew()
        {
            ResetForm();
            SetEnabledAllControls(true);
            SetEnabledRequiredControls(true);
            action = DatabaseConstant.Action.THEM;
            CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain, DatabaseConstant.Function.HDV_SAN_PHAM);
        }

        public void OnAddNew(ref BL_SAN_PHAM obj, ref List<BL_SAN_PHAM_CT> lstCT)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                HuyDongVonProcess processHDV = new HuyDongVonProcess();
                bool ret = false;
                string sMessage = "";

                ret = processHDV.SanPham(DatabaseConstant.Action.THEM, ref obj, ref  lstCT, ref lstPHPL, ref sMessage);
                AfterAddNew(ret, obj, lstCT,lstPHPL, sMessage);
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

        public void AfterAddNew(bool ret,BL_SAN_PHAM obj, List<BL_SAN_PHAM_CT> lstCT, List<KT_PHAN_HE_PLOAI> lstPHPL, string sMessage)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
            try
            {
                if (ret)
                {
                    LMessage.ShowMessage("M.DungChung.ThemThanhCong", LMessage.MessageBoxType.Information);

                    txtMaSanPham.Text = obj.MA_SAN_PHAM;
                    id = obj.ID;

                    sTrangThaiNVu = obj.TTHAI_NVU;
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);
                    txtTrangThaiBanGhi.Text = BusinessConstant.layNgonNguSuDung(obj.TTHAI_BGHI);
                    raddtNgayLap.Value = LDateTime.StringToDate(obj.NGAY_NHAP, "yyyyMMdd");
                    txtNguoiLap.Text = obj.NGUOI_NHAP;
                    raddtNgayCapNhat.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
                    txtNguoiCapNhat.Text = ClientInformation.TenDangNhap;

                    #region Tài khoản phân loại
                    for (int i = 0; i < lstPHPL.Count; i++)
                    {
                        dsTaiKhoanHachToan.Tables[0].Rows[i]["ID"] = lstPHPL[i].ID;                        
                    }
                    #endregion

                    BeforeViewFromDetail();
                }
                else
                {
                    LMessage.ShowMessage(sMessage, LMessage.MessageBoxType.Warning);
                    //LMessage.ShowMessage("M.DungChung.ThemKhongThanhCong", LMessage.MessageBoxType.Error);
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

                bool ret = process.LockData(DatabaseConstant.Module.HDVO,
                    DatabaseConstant.Function.HDV_SAN_PHAM,
                    DatabaseConstant.Table.BL_SAN_PHAM,
                    DatabaseConstant.Action.SUA,
                    listLockId);

                if (ret)
                {
                    SetEnabledAllControls(true);
                    SetEnabledRequiredControls(true);
                    action = DatabaseConstant.Action.SUA;
                    CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain, DatabaseConstant.Function.HDV_SAN_PHAM);
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
            SetEnabledAllControls(true);
            SetEnabledRequiredControls(true);
            action = DatabaseConstant.Action.SUA;
            CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain, DatabaseConstant.Function.HDV_SAN_PHAM);
        }

        public void OnModify(ref BL_SAN_PHAM obj, ref List<BL_SAN_PHAM_CT> lstCT)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                HuyDongVonProcess processHDV = new HuyDongVonProcess();
                bool ret = false;
                string sMessage = "";

                ret = processHDV.SanPham(DatabaseConstant.Action.SUA, ref obj, ref  lstCT, ref lstPHPL, ref sMessage);
                AfterModify(ret, obj, lstCT,lstPHPL, sMessage);
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

        public void AfterModify(bool ret, BL_SAN_PHAM obj, List<BL_SAN_PHAM_CT> lstCT, List<KT_PHAN_HE_PLOAI> lstPHPL, string sMessage)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
            try
            {
                if (ret)
                {
                    LMessage.ShowMessage("M.DungChung.CapNhatThanhCong", LMessage.MessageBoxType.Information);

                    sTrangThaiNVu = obj.TTHAI_NVU;
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);
                    txtTrangThaiBanGhi.Text = BusinessConstant.layNgonNguSuDung(obj.TTHAI_BGHI);
                    raddtNgayLap.Value = LDateTime.StringToDate(obj.NGAY_NHAP, "yyyyMMdd");
                    txtNguoiLap.Text = obj.NGUOI_NHAP;
                    raddtNgayCapNhat.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
                    txtNguoiCapNhat.Text = ClientInformation.TenDangNhap;

                    BeforeViewFromDetail();
                 
                }
                else
                {
                    LMessage.ShowMessage(sMessage, LMessage.MessageBoxType.Warning);
                    //LMessage.ShowMessage("M.DungChung.CapNhatKoThanhCong", LMessage.MessageBoxType.Error);
                }

                // Yêu cầu Unlock bản ghi cần sửa
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(id);

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.HDVO,
                    DatabaseConstant.Function.HDV_SAN_PHAM,
                    DatabaseConstant.Table.BL_SAN_PHAM,
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

                    bool retLockData = process.LockData(DatabaseConstant.Module.HDVO,
                        DatabaseConstant.Function.HDV_SAN_PHAM,
                        DatabaseConstant.Table.BL_SAN_PHAM,
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
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        public void OnDelete()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                bool ret = false;
                string sMessage = "";
                HuyDongVonProcess processHDV = new HuyDongVonProcess();
                ret = processHDV.SanPham(action, ref obj, ref lstObjCT, ref lstPHPL, ref sMessage);
                AfterDelete(ret, sMessage);
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

        public void AfterDelete(bool ret, string sMessage)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
            try
            {
                if (ret)
                {
                    LMessage.ShowMessage("M.DungChung.XoaThanhCong", LMessage.MessageBoxType.Information);
                }
                else
                {
                    LMessage.ShowMessage(sMessage, LMessage.MessageBoxType.Warning);
                    //LMessage.ShowMessage("M.DungChung.XoaKhongThanhCong", LMessage.MessageBoxType.Warning);
                }

                // Yêu cầu unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(id);
                List<int> listUnlockId = new List<int>();

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.HDVO,
                    DatabaseConstant.Function.HDV_SAN_PHAM,
                    DatabaseConstant.Table.BL_SAN_PHAM,
                    DatabaseConstant.Action.XOA,
                    listLockId);

                // Đóng cửa sổ chi tiết sau khi xóa
                OnClose();
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
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

                    bool retLockData = process.LockData(DatabaseConstant.Module.HDVO,
                        DatabaseConstant.Function.HDV_SAN_PHAM,
                        DatabaseConstant.Table.BL_SAN_PHAM,
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
            Mouse.OverrideCursor = Cursors.Arrow;
            try
            {
                bool ret = false;
                string sMessage = "";
                List<BL_SAN_PHAM_CT> lstObjCT = new List<BL_SAN_PHAM_CT>();
                HuyDongVonProcess processHDV = new HuyDongVonProcess();
                ret = processHDV.SanPham(action, ref obj, ref lstObjCT, ref lstPHPL, ref sMessage);
                AfterApprove(ret, sMessage);
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

        public void AfterApprove(bool ret, string sMessage)
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
                    LMessage.ShowMessage(sMessage, LMessage.MessageBoxType.Warning);
                    //LMessage.ShowMessage("M.DungChung.DuyetKhongThanhCong", LMessage.MessageBoxType.Warning);
                }

                // Yêu cầu unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(id);
                List<int> listUnlockId = new List<int>();

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.HDVO,
                    DatabaseConstant.Function.HDV_SAN_PHAM,
                    DatabaseConstant.Table.BL_SAN_PHAM,
                    DatabaseConstant.Action.DUYET,
                    listLockId);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
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

                    bool retLockData = process.LockData(DatabaseConstant.Module.HDVO,
                        DatabaseConstant.Function.HDV_SAN_PHAM,
                        DatabaseConstant.Table.BL_SAN_PHAM,
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
            try
            {
                bool ret = false;
                string sMessage = "";
                List<BL_SAN_PHAM_CT> lstObjCT = new List<BL_SAN_PHAM_CT>();
                HuyDongVonProcess processHDV = new HuyDongVonProcess();

                ret = processHDV.SanPham(action, ref obj, ref lstObjCT, ref lstPHPL, ref sMessage);
                AfterCancel(ret, sMessage);
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

        public void AfterCancel(bool ret, string sMessage)
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
                    LMessage.ShowMessage(sMessage, LMessage.MessageBoxType.Warning);
                    //LMessage.ShowMessage("M.DungChung.ThoaiDuyetKhongThanhCong", LMessage.MessageBoxType.Warning);
                }

                // Yêu cầu unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(id);
                List<int> listUnlockId = new List<int>();

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.HDVO,
                    DatabaseConstant.Function.HDV_SAN_PHAM,
                    DatabaseConstant.Table.BL_SAN_PHAM,
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

                    bool retLockData = process.LockData(DatabaseConstant.Module.HDVO,
                        DatabaseConstant.Function.HDV_SAN_PHAM,
                        DatabaseConstant.Table.BL_SAN_PHAM,
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
            try
            {
                bool ret = false;
                string sMessage = "";
                List<BL_SAN_PHAM_CT> lstObjCT = new List<BL_SAN_PHAM_CT>();
                HuyDongVonProcess processHDV = new HuyDongVonProcess();
                ret = processHDV.SanPham(action, ref obj, ref lstObjCT, ref lstPHPL, ref sMessage);
                AfterRefuse(ret, sMessage);
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

        public void AfterRefuse(bool ret, string sMessage)
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
                    LMessage.ShowMessage(sMessage, LMessage.MessageBoxType.Warning);
                    //LMessage.ShowMessage("M.DungChung.TuChoiDuyetKhongThanhCong", LMessage.MessageBoxType.Warning);
                }

                // Yêu cầu unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(id);
                List<int> listUnlockId = new List<int>();                
                
                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.HDVO,
                    DatabaseConstant.Function.HDV_SAN_PHAM,
                    DatabaseConstant.Table.BL_SAN_PHAM,
                    DatabaseConstant.Action.TU_CHOI_DUYET,
                    listLockId);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }        

        #endregion

    }
}
