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
using Presentation.Process.KhachHangServiceRef;
using PresentationWPF.CustomControl;
using System.Data;
using Microsoft.Windows.Controls.Ribbon;
using Presentation.Process.Common;
using Presentation.Process;
using Presentation.Process.DanhMucServiceRef;
using Presentation.Process.PopupServiceRef;
using Telerik.Windows.Controls.GridView;
using Telerik.Windows.Controls;
using System.Collections;
using System.Reflection;
using System.Threading.Tasks;

namespace PresentationWPF.DanhMuc.ToChucTinDung
{
    /// <summary>
    /// Interaction logic for ucTemplateCT.xaml
    /// </summary>
    public partial class ucToChucTinDungCT : UserControl
    {
        #region Khai bao

        //Khởi tạo các RoutedCommand để phục vụ cho Hotkey
        //Các RoutedCommand này tương ứng với các CommandBinding trong UserControl.CommandBindings
        public static RoutedCommand HoldCommand = new RoutedCommand();
        public static RoutedCommand SaveCommand = new RoutedCommand();
        public static RoutedCommand ModifyCommand = new RoutedCommand();
        public static RoutedCommand DeleteCommand = new RoutedCommand();
        public static RoutedCommand ApproveCommand = new RoutedCommand();
        public static RoutedCommand RefuseCommand = new RoutedCommand();
        public static RoutedCommand CancelCommand = new RoutedCommand();
        public static RoutedCommand HelpCommand = new RoutedCommand();
        public static RoutedCommand CloseCommand = new RoutedCommand();

        //Module của chương trình
        //Có các module: HDVO (Huy động vốn), TDVM (Tín dụng vi mô), TDTT (Tín dụng thông thường)....
        //Tương đối giống dữ liệu trong bảng DM_PHAN_HE
        private DatabaseConstant.Module module = DatabaseConstant.Module.DMDC;

        //Khởi tạo action, mặc định là DatabaseConstant.Action.THEM
        private DatabaseConstant.Action action = DatabaseConstant.Action.THEM;
        public DatabaseConstant.Action Action
        {
            get { return action; }
            set { action = value; }
        }

        //Mỗi một form sẽ có một Function tạo riêng ở Ultilities.Common
        //Function được dùng để phân biệt các chức năng với nhau, từ đó có các xử lý khác nhau khi viết hàm chung (Ví dụ: các hàm trên Communication)
        private DatabaseConstant.Function function = DatabaseConstant.Function.DC_DM_TCTD;

        //Event phục vụ việc khi mở form chi tiết từ form danh sách. Khi đóng form chi tiết sẽ thực thi một hàm nào đó trên form danh sách (thường là load lại dữ liệu)
        public event EventHandler OnSavingCompleted;

        //Object chứa dữ liệu của form
        //Object này có thể là object trong EntityFrameWork hoặc DTO trong Bussiness
        //Trong trường hợp form lấy dữ liệu từ nhiều bảng thì vote sử dụng DTO sau đó convert DTO sang EntityFrameWork (hoặc ngược lại) tại Bussiness Detail
        private DM_TO_CHUC_TIN_DUNG _obj;
        public DM_TO_CHUC_TIN_DUNG obj
        {
            get { return _obj; }
            set { _obj = value; }
        }

        private List<DM_TCTD_TKHOAN> _lstTkhoan = null;

        private List<DM_TCTD_TTIN_KHAC> _lstTTinKhac = null;

        //Khai báo popup mã phân loại hạch toán
        List<DataRow> lstPopup = new List<DataRow>();
        public void LayDuLieuTuPopup(List<DataRow> lst)
        {
            lstPopup = lst;
        }

        private List<AutoCompleteEntry> lstSourceToChuc = new List<AutoCompleteEntry>(); 
        #endregion

        #region Khoi tao
        public ucToChucTinDungCT()
        {
            InitializeComponent();

            //Duyệt quyền của người dùng được phép thêm sửa xóa...
            DuyetQuyenTinhNang();

            //Bind shortcut key với button
            //Việc xử lý khi nhấn thêm, sửa, xóa... sẽ tập trung ở đây
            BindShortkey();
            KhoiTaoComboBox();
            ShowControl();
            InitEventHandler();
            teldtNgayHetHopDong.Value = null;
        }

        /// <summary>
        /// Kiểm tra quyền của người dùng được quyền thêm, sửa, xóa, duyệt... hay không
        /// </summary>
        private void DuyetQuyenTinhNang()
        {
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.DanhMuc;component/ToChucTinDung/ucToChucTinDungCT.xaml", ref Toolbar, ref mnuMain);
            foreach (var item in mnuMain.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += btnShortcutKey_Click;
            }
        }

        void KhoiTaoComboBox()
        {
            AutoComboBox auto = new AutoComboBox();
            List<string> lstDieuKien = new List<string>();
            lstDieuKien.Clear();
            lstDieuKien.Add("LOAI_TO_CHUC");
            auto.GenAutoComboBox(ref lstSourceToChuc, ref cbbLoaiTCTD, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien, "NHTM");
            List<DM_TCTD_TTIN_KHAC> _lstTTinDienThoai = new List<DM_TCTD_TTIN_KHAC>();
            raddDienThoai.ItemsSource = _lstTTinDienThoai;
        }

        private void ShowControl()
        {
            string maToChuc = lstSourceToChuc.ElementAt(cbbLoaiTCTD.SelectedIndex).KeywordStrings.FirstOrDefault();
            HeThong hethong = new HeThong();
            ArrayList arr = new ArrayList();
            arr = hethong.SetVisibleControl("PresentationWPF.TinDung.DonVayVon.ucToChucTinDungCT", maToChuc);
            foreach (List<string> lst in arr)
            {
                object item = grMain.FindName(lst.First());
                string strProperty = lst.ElementAt(1);
                PropertyInfo prty = item.GetType().GetProperty(strProperty);
                if (strProperty.Equals("Visibility"))
                {
                    if (lst.ElementAt(2).Equals("0"))
                        prty.SetValue(item, Visibility.Collapsed, null);
                    else if (lst.ElementAt(2).Equals("1"))
                        prty.SetValue(item, Visibility.Visible, null);
                    else
                        prty.SetValue(item, Visibility.Hidden, null);
                }
                else
                {
                    if (lst.ElementAt(2).Equals("0"))
                        prty.SetValue(item, false, null);
                    else
                        prty.SetValue(item, true, null);
                }
            }
        }

        void InitEventHandler()
        {
            cbbLoaiTCTD.SelectionChanged += cbbLoaiTCTD_SelectionChanged;

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
            RibbonButton tlb = (RibbonButton)sender;
            string strTinhNang = tlb.Name.Substring(3, tlb.Name.Length - 3);
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THEM)))
            {
                BeforeAddNew();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU_TAM)))
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
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THEM)))
            {
                BeforeAddNew();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU_TAM)))
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
                OnPreview();
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

        #region Xu ly giao dien
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
            PresentationWPF.CustomControl.CommonFunction.CloseUserControl(this);
        }

        /// <summary>
        /// Xử lý việc nhấn Tab, Enter để tự focus vào control khác
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
            if (_obj != null && _obj.objTCTD != null && _obj.objTCTD.ID != 0)
            {
                // Yêu cầu unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(_obj.objTCTD.ID);

                bool ret = process.UnlockData(module,
                    function,
                    DatabaseConstant.Table.DM_TCTD,
                    DatabaseConstant.Action.SUA,
                    listLockId);
            }
        }

        /// <summary>
        /// Trợ giúp
        /// </summary>
        private void OnHelp()
        {
            PresentationWPF.CustomControl.CommonFunction.ShowHelp(this);
        }

        /// <summary>
        /// Đóng form
        /// </summary>
        private void OnClose()
        {
            Release();
            PresentationWPF.CustomControl.CommonFunction.CloseUserControl(this);
        }


        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            DM_TCTD_TKHOAN objTkhoan = new DM_TCTD_TKHOAN();
            if (_lstTkhoan == null)
            {
                _lstTkhoan = new List<DM_TCTD_TKHOAN>();
            }

            _lstTkhoan.Add(objTkhoan);

            //Rebind
            raddTaiKhoan.ItemsSource = _lstTkhoan;
            raddTaiKhoan.Rebind();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            for (int i = raddTaiKhoan.SelectedItems.Count - 1; i >= 0; i--)
            {
                raddTaiKhoan.Items.Remove(raddTaiKhoan.SelectedItems[i]);
            }
        }

        private void PhanLoaiTK_LostFocus(object sender, RoutedEventArgs e)
        {
            var txt = sender as TextBox;
        }

        private void PhanLoaiTK_KeyDown(object sender, KeyEventArgs e)
        {
            var txt = sender as TextBox;
            GridViewRow grrow = txt.ParentOfType<GridViewRow>();
            PhanLoaiTaiKhoan(grrow);
        }

        private void PhanLoaiTK_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            GridViewRow grrow = btn.ParentOfType<GridViewRow>();
            PhanLoaiTaiKhoan(grrow);
        }

        private void PhanLoaiTaiKhoan(GridViewRow grrow)
        {
            if (action == DatabaseConstant.Action.XEM)
                return;

            try
            {
                DM_TCTD_TKHOAN drv = grrow.Item as DM_TCTD_TKHOAN;
                List<string> lstDieuKien = new List<string>();
                lstDieuKien.Add("%");
                lstDieuKien.Add("NOI_BANG");
                lstDieuKien.Add(Presentation.Process.Common.ClientInformation.MaDonViGiaoDich);
                lstDieuKien.Add("%");
                var process = new PopupProcess();
                lstPopup.Clear();
                process.getPopupInformation(DatabaseConstant.DanhSachTruyVan.POPUP_TKHOAN_CTIET.getValue(), lstDieuKien);

                SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;

                ucPopup popup = new ucPopup(false, simplePopupResponse, false);
                popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
                Window win = new Window();
                win.Content = popup;
                win.Title = LLanguage.SearchResourceByKey(simplePopupResponse.PopupTitle);
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.ShowDialog();
                if (lstPopup != null && lstPopup.Count > 0)
                {
                    DataRow row = lstPopup[0];
                    drv.MA_PLOAI = row[2].ToString();
                    drv.ID_PLOAI = Convert.ToInt32(row[1]);

                    raddTaiKhoan.CurrentItem = drv;
                    raddTaiKhoan.Rebind();
                }
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void cbbLoaiTCTD_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowControl();
        }
        #endregion

        #region Xu ly nghiep vu
        private void GetFormData(string sTrangThaiNVu)
        {
            try
            {
                if (action != DatabaseConstant.Action.THEM)
                {
                    _obj.objTCTD.NGAY_CNHAT = ClientInformation.NgayLamViecHienTai;
                    _obj.objTCTD.NGUOI_CNHAT = ClientInformation.TenDangNhap;

                }
                else
                {
                    _obj = new DM_TO_CHUC_TIN_DUNG();
                    _obj.objTCTD = new DM_TCTD();
                }
                if(_lstTTinKhac.IsNullOrEmpty())
                    _lstTTinKhac = new List<DM_TCTD_TTIN_KHAC>();
                foreach (DM_TCTD_TTIN_KHAC objTctdTtinKhac in raddDienThoai.Items)
                {
                    objTctdTtinKhac.LOAI_TTIN = "SO_DIEN_THOAI";
                    objTctdTtinKhac.MA_TTIN = "SO_DIEN_THOAI";
                    _lstTTinKhac.Add(objTctdTtinKhac);
                }
                _obj.objTCTD.MA_TCTD = txtMa.Text.Trim();
                _obj.objTCTD.TEN_TCTD = txtTen.Text.Trim();
                _obj.objTCTD.DIA_CHI = txtDiaChi.Text.Trim();
                _obj.objTCTD.GHI_CHU = txtGhiChu.Text.Trim();
                string maToChuc = lstSourceToChuc.ElementAt(cbbLoaiTCTD.SelectedIndex).KeywordStrings.FirstOrDefault();
                _obj.objTCTD.LOAI_TCTD = maToChuc;
                _obj.objTCTD.SO_HOP_DONG = txtSoHopDong.Text;
                if (!teldtNgayHopDong.Value.IsNullOrEmpty())
                    _obj.objTCTD.NGAY_HLUC =
                        teldtNgayHopDong.Value.Value.DateToString(ApplicationConstant.defaultDateTimeFormat);
                if (!teldtNgayHetHopDong.Value.IsNullOrEmpty())
                    _obj.objTCTD.NGAY_HET_HLUC =
                        teldtNgayHetHopDong.Value.Value.DateToString(ApplicationConstant.defaultDateTimeFormat);

                _obj.objTCTD.NGAY_NHAP = Convert.ToDateTime(teldtNgayNhap.Value).ToString("yyyyMMdd");
                _obj.objTCTD.NGUOI_NHAP = txtNguoiLap.Text.Trim();
                _obj.objTCTD.MA_DVI_TAO = ClientInformation.MaDonViGiaoDich;
                _obj.objTCTD.MA_DVI_QLY = ClientInformation.MaDonVi;
                _obj.objTCTD.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                _obj.objTCTD.TTHAI_NVU = sTrangThaiNVu;

                _obj.lstTkhoan = _lstTkhoan.ToArray();
                _obj.lstTTinKhac = _lstTTinKhac.ToArray();

            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        private void SetFormData()
        {
            DanhMucProcess process = new DanhMucProcess();
            List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                int ret = 0;
                ret = process.ToChucTinDung(function, DatabaseConstant.Action.LOAD, ref _obj, ref listClientResponseDetail);
                if (ret > 0)
                {
                    if (_obj != null)
                    {
                        #region tab Thông tin chung
                        lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(_obj.objTCTD.TTHAI_NVU);
                        txtMa.Text = _obj.objTCTD.MA_TCTD;
                        txtTen.Text = _obj.objTCTD.TEN_TCTD;
                        txtDiaChi.Text = _obj.objTCTD.DIA_CHI;
                        txtGhiChu.Text = _obj.objTCTD.GHI_CHU;
                        txtSoHopDong.Text = _obj.objTCTD.SO_HOP_DONG;
                        if (!_obj.objTCTD.LOAI_TCTD.IsNullOrEmptyOrSpace())
                            cbbLoaiTCTD.SelectedIndex =
                                lstSourceToChuc.IndexOf(
                                    lstSourceToChuc.FirstOrDefault(
                                        f => f.KeywordStrings.FirstOrDefault().Equals(_obj.objTCTD.LOAI_TCTD)));
                        if (!_obj.objTCTD.NGAY_HLUC.IsNullOrEmptyOrSpace())
                            teldtNgayHopDong.Value = LDateTime.StringToDate(_obj.objTCTD.NGAY_HLUC,
                                ApplicationConstant.defaultDateTimeFormat);
                        if (!_obj.objTCTD.NGAY_HET_HLUC.IsNullOrEmptyOrSpace())
                            teldtNgayHetHopDong.Value = LDateTime.StringToDate(_obj.objTCTD.NGAY_HET_HLUC,
                                ApplicationConstant.defaultDateTimeFormat);
                        _lstTkhoan = _obj.lstTkhoan.ToList();
                        raddTaiKhoan.ItemsSource = _lstTkhoan;
                        raddTaiKhoan.Rebind();

                        List<DM_TCTD_TTIN_KHAC> _lstTTinDienThoai = new List<DM_TCTD_TTIN_KHAC>();
                        foreach (DM_TCTD_TTIN_KHAC objTctdTtinKhac in _obj.lstTTinKhac)
                        {
                            if (objTctdTtinKhac.LOAI_TTIN == "SO_DIEN_THOAI")
                                _lstTTinKhac.Add(objTctdTtinKhac);
                        }
                        raddDienThoai.ItemsSource = _lstTTinDienThoai;

                        #endregion

                        #region tab Thông tin kiểm soát
                        txtTrangThai.Text = BusinessConstant.layNgonNguNghiepVu(_obj.objTCTD.TTHAI_NVU);
                        teldtNgayNhap.Value = LDateTime.StringToDate(_obj.objTCTD.NGAY_NHAP, "yyyyMMdd");
                        txtNguoiLap.Text = _obj.objTCTD.NGUOI_NHAP;
                        if (!_obj.objTCTD.NGAY_CNHAT.IsNullOrEmptyOrSpace())
                        {
                            teldtNgayCNhat.Value = LDateTime.StringToDate(_obj.objTCTD.NGAY_CNHAT, "yyyyMMdd");
                        }

                        if (!_obj.objTCTD.NGUOI_CNHAT.IsNullOrEmptyOrSpace())
                        {
                            txtNguoiCapNhat.Text = _obj.objTCTD.NGUOI_CNHAT;
                        }
                        #endregion
                    }
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
            lblTrangThai.Content = "";
            _obj = null;
            _obj = new DM_TO_CHUC_TIN_DUNG();
            _obj.objTCTD = new DM_TCTD();
            _obj.objTCTD.TTHAI_NVU = BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri();

            _lstTkhoan = null;
            _lstTkhoan = new List<DM_TCTD_TKHOAN>();
            #region Thông tin chung
            lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(_obj.objTCTD.TTHAI_NVU);
            txtMa.Text = _obj.objTCTD.MA_TCTD;
            txtTen.Text = _obj.objTCTD.TEN_TCTD;
            txtDiaChi.Text = _obj.objTCTD.DIA_CHI;
            txtGhiChu.Text = "";

            raddTaiKhoan.ItemsSource = _lstTkhoan;
            raddTaiKhoan.Rebind();

            #endregion

            #region Thông tin kiểm soát
            txtTrangThai.Text = BusinessConstant.layNgonNguNghiepVu(_obj.objTCTD.TTHAI_NVU);
            teldtNgayNhap.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
            txtNguoiLap.Text = ClientInformation.TenDangNhap;
            teldtNgayCNhat.Value = null;
            txtNguoiCapNhat.Text = "";
            #endregion

        }

        private bool Validation()
        {
            try
            {
                //Các thông tin cần kiểm tra
                if (txtTen.Text.Trim().IsNullOrEmptyOrSpace())
                {
                    CommonFunction.ThongBaoTrong(lblTen.Content.ToString());
                    txtTen.Focus();
                    return false;
                }
                else if (txtDiaChi.Text.Trim().IsNullOrEmptyOrSpace())
                {
                    CommonFunction.ThongBaoTrong(lblDiaChi.Content.ToString());
                    txtDiaChi.Focus();
                    return false;
                }
                else if (raddTaiKhoan.ItemsSource == null || raddTaiKhoan.Items.Count == 0)
                {
                    LMessage.ShowMessage("M_ResponseMessage_ToChucTinDung_ChuaNhapTaiKhoanHachToan", LMessage.MessageBoxType.Warning);
                    return false;
                }
                else
                {
                    int i = 0;
                    foreach (DM_TCTD_TKHOAN objTkhoan in _lstTkhoan)
                    {
                        i++;
                        if (objTkhoan.SO_TKHOAN.IsNullOrEmptyOrSpace())
                        {
                            string message =
                                LLanguage.SearchResourceByKey("M_ResponseMessage_PhieuKeToan_GridRow_ChuaCoTaiKhoan",
                                    new string[1] {i.ToString()});
                            LMessage.ShowMessage(message, LMessage.MessageBoxType.Warning);
                            return false;
                        }

                        if (objTkhoan.TEN_TKHOAN.IsNullOrEmptyOrSpace())
                        {
                            LMessage.ShowMessage("M_ResponseMessage_TaiKhoan_TenKhongHopLe", LMessage.MessageBoxType.Warning);
                            return false;
                        }
                       
                        if (objTkhoan.MA_PLOAI.IsNullOrEmptyOrSpace())
                        {
                            LMessage.ShowMessage("M.KeToan.PhanLoai.ucPhanLoaiCT.LoiMaPhanLoaiTrong", LMessage.MessageBoxType.Warning);
                            return false;
                        }
                    }
                }
                if (!teldtNgayHetHopDong.Value.IsNullOrEmpty())
                {
                    if (teldtNgayHetHopDong.Value < teldtNgayHopDong.Value)
                    {
                        LMessage.ShowMessage("M.DungChung.ThongBao.NgayHetHanNhoNgayHieuLuc", LMessage.MessageBoxType.Warning);
                        return false;
                    }
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
                grpThongTinChung.IsEnabled = true;
                grpTaiKhoan.IsEnabled = true;
            }
            #endregion

            #region Sửa
            else if (action == DatabaseConstant.Action.SUA)
            {
                if (_obj.objTCTD.TTHAI_NVU == BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri())
                {
                    grpThongTinChung.IsEnabled = false;
                    grpTaiKhoan.IsEnabled = false;
                }
                else
                {
                    grpThongTinChung.IsEnabled = true;
                    grpTaiKhoan.IsEnabled = true;
                }
            }
            #endregion

            #region Xem ~ Mặc định
            else
            {
                grpThongTinChung.IsEnabled = false;
                grpTaiKhoan.IsEnabled = false;
                
            }
            #endregion
        }


        private void OnHold()
        {
            try
            {
                if (!Validation()) return;

                string trangThai = "";
                if (_obj.objTCTD.TTHAI_NVU.Equals(BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri()))
                    trangThai = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();
                else
                    trangThai = CommonFunction.LayTrangThaiBanGhi(DatabaseConstant.Action.LUU_TAM, BusinessConstant.layTrangThaiNghiepVu(_obj.objTCTD.TTHAI_NVU));

                GetFormData(trangThai);

                if (action == DatabaseConstant.Action.THEM)
                {
                    OnAddNew();
                }
                else if (action == DatabaseConstant.Action.SUA)
                {
                    OnModify(_obj);
                }
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        private void OnSave()
        {
            try
            {
                if (!Validation()) return;

                string trangThai = "";
                if (_obj.objTCTD.TTHAI_NVU.Equals(BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri()))
                    trangThai = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();
                else
                    trangThai = CommonFunction.LayTrangThaiBanGhi(DatabaseConstant.Action.LUU, BusinessConstant.layTrangThaiNghiepVu(_obj.objTCTD.TTHAI_NVU));

                GetFormData(trangThai);

                if (action == DatabaseConstant.Action.THEM)
                {
                    OnAddNew();
                }
                else if (action == DatabaseConstant.Action.SUA)
                {
                    OnModify(_obj);
                }
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }

        }

        private void BeforeViewFromDetail()
        {
            action = DatabaseConstant.Action.XEM;
            SetEnabledControls();
            CommonFunction.RefreshButton(Toolbar, action, _obj.objTCTD.TTHAI_NVU, mnuMain, function);
        }

        private void BeforeViewFromList()
        {
            SetFormData();
            BeforeViewFromDetail();
        }


        private void BeforeAddNew()
        {
            action = DatabaseConstant.Action.THEM;
            ResetForm();
            SetEnabledControls();
            CommonFunction.RefreshButton(Toolbar, action, _obj.objTCTD.TTHAI_NVU, mnuMain, function);
        }

        private void OnAddNew()
        {

            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                DanhMucProcess process = new DanhMucProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                int ret = 0;

                ret = process.ToChucTinDung(function, DatabaseConstant.Action.THEM, ref _obj, ref listClientResponseDetail);
                AfterAddNew(ret, listClientResponseDetail);
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

        private void AfterAddNew(int ret, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (ret > 0)
                {
                    LMessage.ShowMessage("M.DungChung.ThemThanhCong", LMessage.MessageBoxType.Information);

                    if (cbMultiAdd.IsChecked == true)
                    {
                        ResetForm();
                    }
                    else
                    {
                        lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(_obj.objTCTD.TTHAI_NVU);
                        txtMa.Text = obj.objTCTD.MA_TCTD;
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


        private void BeforeModifyFromDetail()
        {
            try
            {
                // Yêu cầu lock bản ghi cần sửa
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(_obj.objTCTD.ID);

                bool ret = process.LockData(module,
                    function,
                    DatabaseConstant.Table.DM_TCTD,
                    DatabaseConstant.Action.SUA,
                    listLockId);

                if (ret)
                {
                    action = DatabaseConstant.Action.SUA;
                    SetEnabledControls();
                    CommonFunction.RefreshButton(Toolbar, action, _obj.objTCTD.TTHAI_NVU, mnuMain, function);
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

        private void BeforeModifyFromList()
        {
            SetFormData();
            action = DatabaseConstant.Action.SUA;
            SetEnabledControls();
            CommonFunction.RefreshButton(Toolbar, action, _obj.objTCTD.TTHAI_NVU, mnuMain, function);
        }

        private void OnModify(DM_TO_CHUC_TIN_DUNG obj)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                DanhMucProcess process = new DanhMucProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                int ret = 0;

                ret = process.ToChucTinDung(function, DatabaseConstant.Action.SUA, ref obj, ref listClientResponseDetail);
                AfterModify(ret, listClientResponseDetail);
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

        private void AfterModify(int ret, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (ret > 0)
                {
                    LMessage.ShowMessage("M.DungChung.CapNhatThanhCong", LMessage.MessageBoxType.Information);

                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(_obj.objTCTD.TTHAI_NVU);

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
                listLockId.Add(_obj.objTCTD.ID);

                bool retUnlockData = process.UnlockData(module,
                    function,
                    DatabaseConstant.Table.DM_TCTD,
                    DatabaseConstant.Action.SUA,
                    listLockId);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }


        private void BeforeDelete()
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
                    listLockId.Add(_obj.objTCTD.ID);
                    List<int> listLockedId = new List<int>();

                    bool retLockData = process.LockData(module,
                        function,
                        DatabaseConstant.Table.DM_TCTD,
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
                listLockId.Add(_obj.objTCTD.ID);
                List<int> listUnlockId = new List<int>();

                bool retUnlockData = process.UnlockData(module,
                    function,
                    DatabaseConstant.Table.DM_TCTD,
                    DatabaseConstant.Action.XOA,
                    listLockId);

                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        private void OnDelete()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
            DanhMucProcess process = new DanhMucProcess();
            try
            {
                int ret = 0;
                ret = process.ToChucTinDung(function, action, ref _obj, ref listClientResponseDetail);
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

        private void AfterDelete(int ret, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (ret > 0)
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
                listLockId.Add(_obj.objTCTD.ID);
                List<int> listUnlockId = new List<int>();

                bool retUnlockData = process.UnlockData(module,
                    function,
                    DatabaseConstant.Table.DM_TCTD,
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


        private void BeforeApprove()
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
                    listLockId.Add(_obj.objTCTD.ID);
                    List<int> listLockedId = new List<int>();

                    bool retLockData = process.LockData(module,
                        function,
                        DatabaseConstant.Table.DM_TCTD,
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

        private void OnApprove()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
            DanhMucProcess process = new DanhMucProcess();
            try
            {
                int ret = 0;
                ret = process.ToChucTinDung(function, action, ref _obj, ref listClientResponseDetail);
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
                process = null;
            }
        }

        private void AfterApprove(int ret, List<ClientResponseDetail> listClientResponseDetail)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
            try
            {
                if (ret > 0)
                {
                    LMessage.ShowMessage("M.DungChung.DuyetThanhCong", LMessage.MessageBoxType.Information);
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(_obj.objTCTD.TTHAI_NVU);

                    BeforeViewFromDetail();
                }
                else
                {
                    CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                }

                // Yêu cầu unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(_obj.objTCTD.ID);
                List<int> listUnlockId = new List<int>();

                bool retUnlockData = process.UnlockData(module,
                    function,
                    DatabaseConstant.Table.DM_TCTD,
                    DatabaseConstant.Action.DUYET,
                    listLockId);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }


        private void BeforeCancel()
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
                    listLockId.Add(_obj.objTCTD.ID);
                    List<int> listLockedId = new List<int>();

                    bool retLockData = process.LockData(module,
                        function,
                        DatabaseConstant.Table.DM_TCTD,
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

        private void OnCancel()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
            DanhMucProcess process = new DanhMucProcess();
            try
            {
                int ret = 0;
                ret = process.ToChucTinDung(function, action, ref _obj, ref listClientResponseDetail);
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
                process = null;
            }
        }

        private void AfterCancel(int ret, List<ClientResponseDetail> listClientResponseDetail)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
            try
            {
                if (ret > 0)
                {
                    LMessage.ShowMessage("M.DungChung.ThoaiDuyetThanhCong", LMessage.MessageBoxType.Information);

                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(_obj.objTCTD.TTHAI_NVU);

                    BeforeViewFromDetail();
                }
                else
                {
                    CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                }

                // Yêu cầu unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(_obj.objTCTD.ID);
                List<int> listUnlockId = new List<int>();

                bool retUnlockData = process.UnlockData(module,
                    function,
                    DatabaseConstant.Table.DM_TCTD,
                    DatabaseConstant.Action.THOAI_DUYET,
                    listLockId);

            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }


        private void BeforeRefuse()
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
                    listLockId.Add(_obj.objTCTD.ID);
                    List<int> listLockedId = new List<int>();

                    bool retLockData = process.LockData(module,
                        function,
                        DatabaseConstant.Table.DM_TCTD,
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

        private void OnRefuse()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
            DanhMucProcess process = new DanhMucProcess();
            try
            {
                int ret = 0;
                ret = process.ToChucTinDung(function, action, ref _obj, ref listClientResponseDetail);
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
                process = null;
            }
        }

        private void AfterRefuse(int ret, List<ClientResponseDetail> listClientResponseDetail)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
            try
            {
                if (ret > 0)
                {
                    LMessage.ShowMessage("M.DungChung.TuChoiDuyetThanhCong", LMessage.MessageBoxType.Information);

                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(_obj.objTCTD.TTHAI_NVU);

                    BeforeViewFromDetail();
                }
                else
                {
                    CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                }

                // Yêu cầu unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(_obj.objTCTD.ID);
                List<int> listUnlockId = new List<int>();

                bool retUnlockData = process.UnlockData(module,
                    function,
                    DatabaseConstant.Table.DM_TCTD,
                    DatabaseConstant.Action.TU_CHOI_DUYET,
                    listLockId);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        /// <summary>
        /// Chủ yếu phục vụ báo cáo
        /// </summary>
        private void OnPreview()
        {
        }
        #endregion

    }
}
