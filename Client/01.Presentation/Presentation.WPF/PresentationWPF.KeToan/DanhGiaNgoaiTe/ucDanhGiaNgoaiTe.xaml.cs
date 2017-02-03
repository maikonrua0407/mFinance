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
using Presentation.Process.KeToanServiceRef;
using PresentationWPF.CustomControl;
using System.Data;
using Microsoft.Windows.Controls.Ribbon;
using Presentation.Process.Common;
using Presentation.Process;
using Presentation.Process.PopupServiceRef;

namespace PresentationWPF.KeToan.DanhGiaNgoaiTe
{
    /// <summary>
    /// Interaction logic for ucTemplateCT.xaml
    /// </summary>
    public partial class ucDanhGiaNgoaiTe : UserControl
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
        private DatabaseConstant.Module module = DatabaseConstant.Module.GDKT;

        //Khởi tạo action, mặc định là DatabaseConstant.Action.THEM
        private DatabaseConstant.Action action = DatabaseConstant.Action.THEM;
        public DatabaseConstant.Action Action
        {
            get { return action; }
            set { action = value; }
        }

        //Mỗi một form sẽ có một Function tạo riêng ở Ultilities.Common
        //Function được dùng để phân biệt các chức năng với nhau, từ đó có các xử lý khác nhau khi viết hàm chung (Ví dụ: các hàm trên Communication)
        private DatabaseConstant.Function function = DatabaseConstant.Function.KT_DANH_GIA_NGOAI_TE;

        //Event phục vụ việc khi mở form chi tiết từ form danh sách. Khi đóng form chi tiết sẽ thực thi một hàm nào đó trên form danh sách (thường là load lại dữ liệu)
        public event EventHandler OnSavingCompleted;

        //Object chứa dữ liệu của form
        //Object này có thể là object trong EntityFrameWork hoặc DTO trong Bussiness
        //Trong trường hợp form lấy dữ liệu từ nhiều bảng thì vote sử dụng DTO sau đó convert DTO sang EntityFrameWork (hoặc ngược lại) tại Bussiness Detail
        private DANH_GIA_NGOAI_TE _obj;

        private List<TKHOAN_DANH_GIA_NGOAI_TE> _lstTkhoanDanhGia = new List<TKHOAN_DANH_GIA_NGOAI_TE>();

        //Source cac combobox
        List<AutoCompleteEntry> lstSourceDonVi = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceLoaiTien = new List<AutoCompleteEntry>();

        //Khai báo popup
        List<DataRow> lstPopup = new List<DataRow>();
        public void LayDuLieuTuPopup(List<DataRow> lst)
        {
            lstPopup = lst;
        }

        //Tham số hệ thống
        private string _Tkhoan_danh_gia = "";
        private string _Tkhoan_thu_nhap = "";
        private string _Tkhoan_chi_phi = "";

        #endregion

        #region Khoi tao
        public ucDanhGiaNgoaiTe()
        {
            InitializeComponent();

            //Duyệt quyền của người dùng được phép thêm sửa xóa...
            DuyetQuyenTinhNang();

            //Bind shortcut key với button
            //Việc xử lý khi nhấn thêm, sửa, xóa... sẽ tập trung ở đây
            BindShortkey();

            //Khởi tạo các combobox
            LoadComBoBox();

            //Khởi tạo các event
            InitEventHandler();

            //Lấy giá trị tham số hệ thống
            InitSystemParam();
        }

        public ucDanhGiaNgoaiTe(KIEM_SOAT objKiemSoat)
        {
            InitializeComponent();

            //Set giá trị
            action = objKiemSoat.action;
            _obj = new DANH_GIA_NGOAI_TE();
            _obj.ID_GDICH = objKiemSoat.ID;

            //Duyệt quyền của người dùng được phép thêm sửa xóa...
            DuyetQuyenTinhNang();

            //Bind shortcut key với button
            //Việc xử lý khi nhấn thêm, sửa, xóa... sẽ tập trung ở đây
            BindShortkey();

            //Khởi tạo các combobox
            LoadComBoBox();

            //Khởi tạo các event
            InitEventHandler();

            //Lấy giá trị tham số hệ thống
            InitSystemParam();
        }

        /// <summary>
        /// Kiểm tra quyền của người dùng được quyền thêm, sửa, xóa, duyệt... hay không
        /// </summary>
        private void DuyetQuyenTinhNang()
        {
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.KeToan;component/DanhGiaNgoaiTe/ucDanhGiaNgoaiTe.xaml", ref Toolbar, ref mnuMain);
            foreach (var item in mnuMain.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += btnShortcutKey_Click;
            }
        }

        private void InitEventHandler()
        {
            //Các sự kiện cần khởi tạo trong code. Thông thường là sự kiện SelectionChanged của các combobox
            cmbLoaiTien.SelectionChanged += new SelectionChangedEventHandler(cmbLoaiTien_SelectionChanged);
        }

        private void InitSystemParam()
        {
            UtilitiesProcess process = new UtilitiesProcess();
            try
            {
                _Tkhoan_danh_gia = process.LayGiaTriThamSo(BusinessConstant.LoaiThamSo.TW, BusinessConstant.MaThamSo.TW_TKHOAN_DANH_GIA_NGOAI_TE, ClientInformation.MaDonVi);
                _Tkhoan_thu_nhap = process.LayGiaTriThamSo(BusinessConstant.LoaiThamSo.TW, BusinessConstant.MaThamSo.TW_TKHOAN_THU_NHAP, ClientInformation.MaDonVi);
                _Tkhoan_chi_phi = process.LayGiaTriThamSo(BusinessConstant.LoaiThamSo.TW, BusinessConstant.MaThamSo.TW_TKHOAN_CHI_PHI, ClientInformation.MaDonVi);
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            finally
            {
                process = null;
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
            if (_obj != null && _obj.ID_GDICH != 0)
            {
                // Yêu cầu unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(_obj.ID_GDICH);

                bool ret = process.UnlockData(module,
                    function,
                    DatabaseConstant.Table.KT_GIAO_DICH,
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

        private void LoadComBoBox()
        {
            COMBOBOX_DTO combo = null;
            List<string> lstDieuKien = new List<string>();
            List<COMBOBOX_DTO> lstCombobox = new List<COMBOBOX_DTO>();
            AutoComboBox au = new AutoComboBox();
            try
            {
                //Combobox đơn vị
                combo = new COMBOBOX_DTO();
                lstDieuKien.Add(ClientInformation.TenDangNhap);
                lstDieuKien.Add(ClientInformation.MaDonVi);
                lstDieuKien.Add(DatabaseConstant.ToChucDonVi.PGD.getValue());
                combo.combobox = cmbDonVi;
                combo.lstDieuKien = lstDieuKien;
                combo.lstSource = lstSourceDonVi;
                combo.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DONVI_THEOPVI.getValue();
                combo.maChon = ClientInformation.MaDonViGiaoDich;
                lstCombobox.Add(combo);

                //Combox loại tiền
                combo = new COMBOBOX_DTO();
                lstDieuKien = new List<string>();
                combo.combobox = cmbLoaiTien;
                combo.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_TIENTE.getValue();
                combo.lstDieuKien = lstDieuKien;
                combo.lstSource = lstSourceLoaiTien;
                lstCombobox.Add(combo);

                //Gen combobox
                au.GenAutoComboBoxTheoList(ref lstCombobox);

                //Loại bỏ tiền nội tệ
                lstSourceLoaiTien.Remove(lstSourceLoaiTien.FirstOrDefault(e => e.KeywordStrings[0].Equals(ClientInformation.MaDongNoiTe)));
                cmbLoaiTien.Items.Clear();
                foreach (AutoCompleteEntry auto in lstSourceLoaiTien)
                {
                    cmbLoaiTien.Items.Add(auto);
                }
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

        #region Xử lý popup

        #endregion

        #region Event
        private void cmbLoaiTien_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                // Lấy thông tin tỷ giá
                TruyVanProcess process = new TruyVanProcess();
                string loaiTien = lstSourceLoaiTien.ElementAt(cmbLoaiTien.SelectedIndex).KeywordStrings.FirstOrDefault();
                DataTable dt = null;
                LDatatable.MakeParameterTable(ref dt);
                LDatatable.AddParameter(ref dt, "@LOAI_TIEN", "string", loaiTien);
                LDatatable.AddParameter(ref dt, "@NGAY_DL", "string", ClientInformation.NgayLamViecHienTai);
                DataSet ds = process.TruyVan("INQ.CT.TY_GIA", dt);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    numTyGia.Value = Convert.ToDouble(ds.Tables[0].Rows[0]["TY_GIA_MUA"]);
                }
                else
                {
                    numTyGia.Value = 0;
                }

                // Lấy thông tin tài khoản
                dt = null;
                LDatatable.MakeParameterTable(ref dt);
                LDatatable.AddParameter(ref dt, "@MA_DVI_QLY", "string", ClientInformation.MaDonVi);
                LDatatable.AddParameter(ref dt, "@MA_LOAI_TIEN", "string", loaiTien);
                LDatatable.AddParameter(ref dt, "@NGAY_DL", "string", ClientInformation.NgayLamViecHienTai);
                DataSet dsCT = process.TruyVan("INQ.DS.TKOAN_DANH_GIA_NGOAI_TE", dt);
                _lstTkhoanDanhGia = new List<TKHOAN_DANH_GIA_NGOAI_TE>();

                if (dsCT != null && dsCT.Tables.Count > 0)
                {
                    if (dsCT.Tables[0] != null && dsCT.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dsCT.Tables[0].Rows)
                        {
                            TKHOAN_DANH_GIA_NGOAI_TE objTK = new TKHOAN_DANH_GIA_NGOAI_TE();
                            objTK.SO_TAI_KHOAN = dr["SO_TAI_KHOAN"].ToString();
                            objTK.TEN_TAI_KHOAN = dr["TEN_TAI_KHOAN"].ToString();
                            objTK.TON_QUY = Convert.ToDecimal(dr["TON_QUY"]);
                            objTK.SO_TIEN_NGOAI_TE = Convert.ToDecimal(dr["SO_TIEN_NGOAI_TE"]);
                            objTK.CHON = true;
                            _lstTkhoanDanhGia.Add(objTK);
                        }
                    }
                }

                

                // Quy đổi tiền
                QuyDoiNgoaiTeSangNoiTe(Convert.ToDecimal(numTyGia.Value));

                raddgrTkhoanDanhGiaDS.ItemsSource = _lstTkhoanDanhGia;
                raddgrTkhoanDanhGiaDS.Rebind();
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (cmbLoaiTien.SelectedIndex == -1)
            {
                PresentationWPF.CustomControl.CommonFunction.ThongBaoTrong(lblLoaiTien.Content.ToString());
                cmbLoaiTien.Focus();
                return;
            }

            Presentation.Process.KeToanProcess ketoanProcess = new Presentation.Process.KeToanProcess();
            try
            {
                List<string> lstDieuKien = new List<string>();
                string loaiTien = lstSourceLoaiTien.ElementAt(cmbLoaiTien.SelectedIndex).KeywordStrings.FirstOrDefault();
                string soTaiKhoan = "%";

                if (raddgrTkhoanDanhGiaDS.Items != null && raddgrTkhoanDanhGiaDS.Items.Count > 0)
                {
                    soTaiKhoan = "(";
                    foreach (object objTkhoan in raddgrTkhoanDanhGiaDS.Items)
                    {
                        soTaiKhoan += "''" + ((TKHOAN_DANH_GIA_NGOAI_TE)objTkhoan).SO_TAI_KHOAN + "'',";
                    }
                    soTaiKhoan = soTaiKhoan.Substring(0,soTaiKhoan.Length-1) + ")";
                }

                lstDieuKien.Add(ClientInformation.MaDonVi);
                lstDieuKien.Add(loaiTien);
                lstDieuKien.Add(LDateTime.DateToString(teldtNgayDanhGia.Value.Value,"yyyyMMdd"));
                lstDieuKien.Add(soTaiKhoan);
                //Bat popup
                var process = new PopupProcess();

                process.getPopupInformation("POPUP_TKHOAN_DS_DANH_GIA_NGOAI_TE", lstDieuKien);
                SimplePopupResponse simplePopupResponse = Presentation.Process.Common.ClientInformation.SimplePopup;

                lstPopup.Clear();
                ucPopup popup = new ucPopup(false, simplePopupResponse, false);
                popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
                Window win = new Window();
                win.Content = popup;
                win.Title = simplePopupResponse.PopupTitle;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.ShowDialog();
                if (lstPopup != null && lstPopup.Count > 0)
                {
                    foreach (DataRow dr in lstPopup)
                    {
                        TKHOAN_DANH_GIA_NGOAI_TE objTkhoan = new TKHOAN_DANH_GIA_NGOAI_TE();
                        objTkhoan.SO_TAI_KHOAN = dr["SO_TAI_KHOAN"].ToString();
                        objTkhoan.TEN_TAI_KHOAN = dr["TEN_TAI_KHOAN"].ToString();
                        objTkhoan.TON_QUY = Convert.ToDecimal(dr["TON_QUY"]);
                        objTkhoan.SO_TIEN_NGOAI_TE = Convert.ToDecimal(dr["SO_TIEN_NGOAI_TE"]);
                        objTkhoan.CHON = true;
                        objTkhoan.TON_QUY_DANH_GIA = objTkhoan.SO_TIEN_NGOAI_TE * Convert.ToDecimal(numTyGia.Value);
                        objTkhoan.CHENH_LECH = objTkhoan.TON_QUY - objTkhoan.TON_QUY_DANH_GIA;

                        if (_lstTkhoanDanhGia == null)
                        {
                            _lstTkhoanDanhGia = new List<TKHOAN_DANH_GIA_NGOAI_TE>();
                        }
                        _lstTkhoanDanhGia.Add(objTkhoan);
                    }

                    //Rebind
                    raddgrTkhoanDanhGiaDS.ItemsSource = _lstTkhoanDanhGia;
                    raddgrTkhoanDanhGiaDS.Rebind();
                }
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            finally
            {
                ketoanProcess = null;
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            for (int i = raddgrTkhoanDanhGiaDS.SelectedItems.Count - 1; i >= 0; i--)
            {
                raddgrTkhoanDanhGiaDS.Items.Remove(raddgrTkhoanDanhGiaDS.SelectedItems[i]);
            }
        }

        private void txtTKDanhGiaNgoaiTe_KeyDown(object sender, KeyEventArgs e)
        {
            List<string> lstDieuKien = new List<string>();
            if (e.Key == Key.F3)
            {
                HienThiPopupTaiKhoan(txtTKDanhGiaNgoaiTe, _Tkhoan_danh_gia + "%", ClientInformation.MaDongNoiTe, "%");
            }
        }

        private void HienThiPopupTaiKhoan(TextBox txtHienThi, string maPloai, string maNoiTe, string maNgoaiTe)
        {
            Presentation.Process.KeToanProcess ketoanProcess = new Presentation.Process.KeToanProcess();
            try
            {
                string maDonVi = lstSourceDonVi[cmbDonVi.SelectedIndex].KeywordStrings[0];

                List<string> lstDieuKien = new List<string>();
                lstDieuKien.Add(maPloai);
                lstDieuKien.Add("(''" + maDonVi + "'')");
                lstDieuKien.Add(maNoiTe);
                lstDieuKien.Add(maNgoaiTe);
                //Bat popup
                var process = new PopupProcess();

                process.getPopupInformation("POPUP_TKHOAN_CTIET_DANH_GIA_NGOAI_TE", lstDieuKien);
                SimplePopupResponse simplePopupResponse = Presentation.Process.Common.ClientInformation.SimplePopup;

                lstPopup.Clear();
                ucPopup popup = new ucPopup(false, simplePopupResponse, false);
                popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
                Window win = new Window();
                win.Content = popup;
                win.Title = LLanguage.SearchResourceByKey(simplePopupResponse.PopupTitle);
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.ShowDialog();
                if (lstPopup != null && lstPopup.Count > 0)
                {
                    txtHienThi.Text = lstPopup[0]["SO_TAI_KHOAN"].ToString();
                    lstPopup.Clear();
                }
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            finally
            {
                ketoanProcess = null;
            }
        }

        private void btnTKDanhGiaNgoaiTe_Click(object sender, RoutedEventArgs e)
        {
            HienThiPopupTaiKhoan(txtTKDanhGiaNgoaiTe, _Tkhoan_danh_gia + "%", ClientInformation.MaDongNoiTe, "%");
        }

        private void txtTKThuNhap_KeyDown(object sender, KeyEventArgs e)
        {
            HienThiPopupTaiKhoan(txtTKThuNhap, _Tkhoan_thu_nhap + "%", ClientInformation.MaDongNoiTe, "%");
        }

        private void btnTKThuNhap_Click(object sender, RoutedEventArgs e)
        {
            HienThiPopupTaiKhoan(txtTKThuNhap, _Tkhoan_thu_nhap + "%", ClientInformation.MaDongNoiTe, "%");
        }

        private void txtTKChiPhi_KeyDown(object sender, KeyEventArgs e)
        {
            HienThiPopupTaiKhoan(txtTKChiPhi, _Tkhoan_chi_phi + "%", ClientInformation.MaDongNoiTe, "%");
        }

        private void btnTKChiPhi_Click(object sender, RoutedEventArgs e)
        {
            HienThiPopupTaiKhoan(txtTKChiPhi, _Tkhoan_chi_phi + "%", ClientInformation.MaDongNoiTe, "%");
        }
        #endregion

        private void QuyDoiNgoaiTeSangNoiTe(decimal tygia)
        {
            if (_lstTkhoanDanhGia != null)
            {
                foreach (TKHOAN_DANH_GIA_NGOAI_TE objCT in _lstTkhoanDanhGia)
                {
                    objCT.TON_QUY_DANH_GIA = objCT.SO_TIEN_NGOAI_TE * Convert.ToDecimal(numTyGia.Value.Value);
                    objCT.CHENH_LECH = objCT.TON_QUY - objCT.TON_QUY_DANH_GIA;
                }
            }
        }

        #endregion

        #region Xu ly nghiep vu
        private void GetFormData(ref DANH_GIA_NGOAI_TE obj, string sTrangThaiNVu)
        {
            try
            {
                if (action != DatabaseConstant.Action.THEM)
                {
                    _obj.NGAY_CNHAT = ClientInformation.NgayLamViecHienTai;
                    _obj.NGUOI_CNHAT = ClientInformation.TenDangNhap;

                }
                else
                {
                    _obj = new DANH_GIA_NGOAI_TE();
                }

                _obj.NGAY_NHAP = Convert.ToDateTime(teldtNgayNhap.Value).ToString("yyyyMMdd");
                _obj.NGUOI_NHAP = txtNguoiLap.Text.Trim();
                _obj.MA_DVI_TAO = ClientInformation.MaDonViGiaoDich;
                _obj.MA_DVI_QLY = ClientInformation.MaDonVi;
                _obj.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                _obj.TTHAI_NVU = sTrangThaiNVu;

                _obj.MA_DVI = lstSourceDonVi[cmbDonVi.SelectedIndex].KeywordStrings[0];
                _obj.MA_GDICH = txtSoGiaoDich.Text.Trim();
                _obj.NGAY_DANH_GIA = LDateTime.DateToString(teldtNgayDanhGia.Value.Value, "yyyyMMdd");
                _obj.LOAI_TIEN = lstSourceLoaiTien[cmbLoaiTien.SelectedIndex].KeywordStrings[0];
                _obj.LOAI_TIEN_NOI_TE = ClientInformation.MaDongNoiTe;
                _obj.TY_GIA = Convert.ToDecimal(numTyGia.Value);
                _obj.NGOAI_TE_TON_QUY_TIEN_MAT = chkTienMat.IsChecked.Value;
                _obj.NGOAI_TE_TON_QUY_TKTG_TCTD = chkTienGuiTCTDKhac.IsChecked.Value;
                _obj.TKHOAN_DANH_GIA_NGOAI_TE = txtTKDanhGiaNgoaiTe.Text.Trim();
                _obj.TKHOAN_THU_NHAP = txtTKThuNhap.Text.Trim();
                _obj.TKHOAN_CHI_PHI = txtTKChiPhi.Text.Trim();
                _obj.DIEN_GIAI = txtDienGiai.Text.Trim();

                _obj.lstDanhGiaCT = _lstTkhoanDanhGia.ToArray();
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        private void SetFormData()
        {
            KeToanProcess process = new KeToanProcess();
            List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                ApplicationConstant.ResponseStatus ret = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                ret = process.DanhGiaNgoaiTe(function, DatabaseConstant.Action.LOAD, ref _obj, ref listClientResponseDetail);
                if (ret == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    if (_obj != null)
                    {
                        #region tab Thông tin chung
                        lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(_obj.TTHAI_NVU);
                        txtSoGiaoDich.Text = _obj.MA_GDICH;
                        cmbDonVi.SelectedIndex = lstSourceDonVi.IndexOf(lstSourceDonVi.FirstOrDefault(e => e.KeywordStrings.FirstOrDefault().Equals(_obj.MA_DVI)));
                        teldtNgayDanhGia.Value = LDateTime.StringToDate(_obj.NGAY_DANH_GIA,"yyyyMMdd");
                        cmbLoaiTien.SelectedIndex = lstSourceLoaiTien.IndexOf(lstSourceLoaiTien.FirstOrDefault(e => e.KeywordStrings.FirstOrDefault().Equals(_obj.LOAI_TIEN)));
                        numTyGia.Value = Convert.ToDouble(_obj.TY_GIA);
                        chkTienMat.IsChecked = _obj.NGOAI_TE_TON_QUY_TIEN_MAT;
                        chkTienGuiTCTDKhac.IsChecked = _obj.NGOAI_TE_TON_QUY_TKTG_TCTD;
                        txtTKDanhGiaNgoaiTe.Text = _obj.TKHOAN_DANH_GIA_NGOAI_TE;
                        txtTKChiPhi.Text = _obj.TKHOAN_CHI_PHI;
                        txtTKThuNhap.Text = _obj.TKHOAN_THU_NHAP;
                        txtDienGiai.Text = _obj.DIEN_GIAI;

                        _lstTkhoanDanhGia = _obj.lstDanhGiaCT.ToList();

                        raddgrTkhoanDanhGiaDS.ItemsSource = _lstTkhoanDanhGia;
                        raddgrTkhoanDanhGiaDS.Rebind();

                        #endregion

                        #region tab Thông tin kiểm soát
                        txtTrangThai.Text = BusinessConstant.layNgonNguNghiepVu(_obj.TTHAI_NVU);
                        teldtNgayNhap.Value = LDateTime.StringToDate(_obj.NGAY_NHAP, "yyyyMMdd");
                        txtNguoiLap.Text = _obj.NGUOI_NHAP;
                        if (!_obj.NGAY_CNHAT.IsNullOrEmptyOrSpace())
                        {
                            teldtNgayCNhat.Value = LDateTime.StringToDate(_obj.NGAY_CNHAT, "yyyyMMdd");
                        }

                        if (!_obj.NGUOI_CNHAT.IsNullOrEmptyOrSpace())
                        {
                            txtNguoiCapNhat.Text = _obj.NGUOI_CNHAT;
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
            _obj = new DANH_GIA_NGOAI_TE();
            _obj.TTHAI_NVU = BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri();

            #region Thông tin chung
            lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(_obj.TTHAI_NVU);
            teldtNgayDanhGia.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");

            raddgrTkhoanDanhGiaDS.Rebind();
            #endregion

            #region Thông tin kiểm soát
            txtTrangThai.Text = BusinessConstant.layNgonNguNghiepVu(_obj.TTHAI_NVU);
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
                if (txtTKDanhGiaNgoaiTe.Text.Trim().IsNullOrEmptyOrSpace())
                {
                    PresentationWPF.CustomControl.CommonFunction.ThongBaoTrong(lblTKDanhGiaNgoaiTe.Content.ToString());
                    txtTKDanhGiaNgoaiTe.Focus();
                    return false;
                }
                else if (txtTKThuNhap.Text.Trim().IsNullOrEmptyOrSpace())
                {
                    PresentationWPF.CustomControl.CommonFunction.ThongBaoTrong(lblTKThuNhap.Content.ToString());
                    txtTKThuNhap.Focus();
                    return false;
                }
                else if (txtTKChiPhi.Text.Trim().IsNullOrEmptyOrSpace())
                {
                    PresentationWPF.CustomControl.CommonFunction.ThongBaoTrong(lblTKChiPhi.Content.ToString());
                    txtTKChiPhi.Focus();
                    return false;
                }
                else if (raddgrTkhoanDanhGiaDS.Items == null || raddgrTkhoanDanhGiaDS.Items.Count == 0)
                {
                    if (_lstTkhoanDanhGia.Count(e => e.CHON == true) <= 0)
                    {
                        LMessage.ShowMessage("M_ResponseMessage_DanhGiaNgoaiTe_ChuaChonTaiKhoanDanhGiaTrongDSach", LMessage.MessageBoxType.Warning);
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
                grpThongTinChiTiet.IsEnabled = true;
            }
            #endregion

            #region Sửa
            else if (action == DatabaseConstant.Action.SUA)
            {
                grpThongTinChung.IsEnabled = true;
                grpThongTinChiTiet.IsEnabled = true;

            }
            #endregion

            #region Xem ~ Mặc định
            else
            {
                grpThongTinChung.IsEnabled = false;
                grpThongTinChiTiet.IsEnabled = false;
            }
            #endregion
        }


        private void OnHold()
        {
            try
            {
                if (!Validation()) return;

                string trangThai = "";
                if (_obj.TTHAI_NVU.Equals(BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri()))
                    trangThai = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();
                else
                    trangThai = CommonFunction.LayTrangThaiBanGhi(DatabaseConstant.Action.LUU_TAM, BusinessConstant.layTrangThaiNghiepVu(_obj.TTHAI_NVU));

                GetFormData(ref _obj, trangThai);

                if (action == DatabaseConstant.Action.THEM)
                {
                    OnAddNew();
                }
                else if (action == DatabaseConstant.Action.SUA)
                {
                    OnModify();
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
                if (_obj.TTHAI_NVU.Equals(BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri()))
                    trangThai = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();
                else
                    trangThai = CommonFunction.LayTrangThaiBanGhi(DatabaseConstant.Action.LUU, BusinessConstant.layTrangThaiNghiepVu(_obj.TTHAI_NVU));

                GetFormData(ref _obj, trangThai);

                if (action == DatabaseConstant.Action.THEM)
                {
                    OnAddNew();
                }
                else if (action == DatabaseConstant.Action.SUA)
                {
                    OnModify();
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
            CommonFunction.RefreshButton(Toolbar, action, _obj.TTHAI_NVU, mnuMain, function);
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
            CommonFunction.RefreshButton(Toolbar, action, _obj.TTHAI_NVU, mnuMain, function);
        }

        private void OnAddNew()
        {

            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                KeToanProcess process = new KeToanProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                ApplicationConstant.ResponseStatus ret = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;

                ret = process.DanhGiaNgoaiTe(function, DatabaseConstant.Action.THEM, ref _obj, ref listClientResponseDetail);
                AfterAddNew(ret, _obj, listClientResponseDetail);
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

        private void AfterAddNew(ApplicationConstant.ResponseStatus ret, DANH_GIA_NGOAI_TE obj, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (ret == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    LMessage.ShowMessage("M.DungChung.ThemThanhCong", LMessage.MessageBoxType.Information);

                    if (cbMultiAdd.IsChecked == true)
                    {
                        ResetForm();
                    }
                    else
                    {
                        lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(_obj.TTHAI_NVU);
                        txtTrangThai.Text = BusinessConstant.layNgonNguNghiepVu(_obj.TTHAI_NVU);
                        txtSoGiaoDich.Text = _obj.MA_GDICH;
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
                listLockId.Add(_obj.ID_GDICH);

                bool ret = process.LockData(module,
                    function,
                    DatabaseConstant.Table.KT_GIAO_DICH,
                    DatabaseConstant.Action.SUA,
                    listLockId);

                if (ret)
                {
                    action = DatabaseConstant.Action.SUA;
                    SetEnabledControls();
                    CommonFunction.RefreshButton(Toolbar, action, _obj.TTHAI_NVU, mnuMain, function);
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
            CommonFunction.RefreshButton(Toolbar, action, _obj.TTHAI_NVU, mnuMain, function);
        }

        private void OnModify()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                KeToanProcess process = new KeToanProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                ApplicationConstant.ResponseStatus ret = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;

                ret = process.DanhGiaNgoaiTe(function, DatabaseConstant.Action.SUA, ref _obj, ref listClientResponseDetail);
                AfterModify(ret, _obj, listClientResponseDetail);
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

        private void AfterModify(ApplicationConstant.ResponseStatus ret, DANH_GIA_NGOAI_TE obj, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (ret == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    LMessage.ShowMessage("M.DungChung.CapNhatThanhCong", LMessage.MessageBoxType.Information);

                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(_obj.TTHAI_NVU);
                    txtTrangThai.Text = BusinessConstant.layNgonNguNghiepVu(_obj.TTHAI_NVU);
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
                listLockId.Add(_obj.ID_GDICH);

                bool retUnlockData = process.UnlockData(module,
                    function,
                    DatabaseConstant.Table.KT_GIAO_DICH,
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
                    listLockId.Add(_obj.ID_GDICH);
                    List<int> listLockedId = new List<int>();

                    bool retLockData = process.LockData(module,
                        function,
                        DatabaseConstant.Table.KT_GIAO_DICH,
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
                listLockId.Add(_obj.ID_GDICH);
                List<int> listUnlockId = new List<int>();

                bool retUnlockData = process.UnlockData(module,
                    function,
                    DatabaseConstant.Table.KT_GIAO_DICH,
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
            KeToanProcess process = new KeToanProcess();
            try
            {
                ApplicationConstant.ResponseStatus ret = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                ret = process.DanhGiaNgoaiTe(function, action, ref _obj, ref listClientResponseDetail);
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

        private void AfterDelete(ApplicationConstant.ResponseStatus ret, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (ret == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    LMessage.ShowMessage("M.DungChung.XoaThanhCong", LMessage.MessageBoxType.Information);

                    // Đóng cửa sổ chi tiết sau khi xóa
                    OnClose();
                }
                else
                {
                    CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                    //LMessage.ShowMessage("M.DungChung.XoaKhongThanhCong", LMessage.MessageBoxType.Warning);
                }

                // Yêu cầu unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(_obj.ID_GDICH);
                List<int> listUnlockId = new List<int>();

                bool retUnlockData = process.UnlockData(module,
                    function,
                    DatabaseConstant.Table.KT_GIAO_DICH,
                    DatabaseConstant.Action.XOA,
                    listLockId);
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
                    listLockId.Add(_obj.ID_GDICH);
                    List<int> listLockedId = new List<int>();

                    bool retLockData = process.LockData(module,
                        function,
                        DatabaseConstant.Table.KT_GIAO_DICH,
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
            KeToanProcess process = new KeToanProcess();
            try
            {
                ApplicationConstant.ResponseStatus ret = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                ret = process.DanhGiaNgoaiTe(function, action, ref _obj, ref listClientResponseDetail);
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

        private void AfterApprove(ApplicationConstant.ResponseStatus ret, List<ClientResponseDetail> listClientResponseDetail)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
            try
            {
                if (ret == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    LMessage.ShowMessage("M.DungChung.DuyetThanhCong", LMessage.MessageBoxType.Information);
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(_obj.TTHAI_NVU);
                    txtTrangThai.Text = BusinessConstant.layNgonNguNghiepVu(_obj.TTHAI_NVU);
                    BeforeViewFromDetail();
                }
                else
                {
                    CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                }

                // Yêu cầu unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(_obj.ID_GDICH);
                List<int> listUnlockId = new List<int>();

                bool retUnlockData = process.UnlockData(module,
                    function,
                    DatabaseConstant.Table.KT_GIAO_DICH,
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
                    listLockId.Add(_obj.ID_GDICH);
                    List<int> listLockedId = new List<int>();

                    bool retLockData = process.LockData(module,
                        function,
                        DatabaseConstant.Table.KT_GIAO_DICH,
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
            KeToanProcess process = new KeToanProcess();
            try
            {
                ApplicationConstant.ResponseStatus ret = 0;
                ret = process.DanhGiaNgoaiTe(function, action, ref _obj, ref listClientResponseDetail);
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

        private void AfterCancel(ApplicationConstant.ResponseStatus ret, List<ClientResponseDetail> listClientResponseDetail)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
            try
            {
                if (ret == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    LMessage.ShowMessage("M.DungChung.ThoaiDuyetThanhCong", LMessage.MessageBoxType.Information);

                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(_obj.TTHAI_NVU);
                    txtTrangThai.Text = BusinessConstant.layNgonNguNghiepVu(_obj.TTHAI_NVU);
                    BeforeViewFromDetail();
                }
                else
                {
                    CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                }

                // Yêu cầu unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(_obj.ID_GDICH);
                List<int> listUnlockId = new List<int>();

                bool retUnlockData = process.UnlockData(module,
                    function,
                    DatabaseConstant.Table.KT_GIAO_DICH,
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
                    listLockId.Add(_obj.ID_GDICH);
                    List<int> listLockedId = new List<int>();

                    bool retLockData = process.LockData(module,
                        function,
                        DatabaseConstant.Table.KT_GIAO_DICH,
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
            KeToanProcess process = new KeToanProcess();
            try
            {
                ApplicationConstant.ResponseStatus ret = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                ret = process.DanhGiaNgoaiTe(function, action, ref _obj, ref listClientResponseDetail);
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

        private void AfterRefuse(ApplicationConstant.ResponseStatus ret, List<ClientResponseDetail> listClientResponseDetail)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
            try
            {
                if (ret == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    LMessage.ShowMessage("M.DungChung.TuChoiDuyetThanhCong", LMessage.MessageBoxType.Information);

                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(_obj.TTHAI_NVU);
                    txtTrangThai.Text = BusinessConstant.layNgonNguNghiepVu(_obj.TTHAI_NVU);
                    BeforeViewFromDetail();
                }
                else
                {
                    CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                }

                // Yêu cầu unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(_obj.ID_GDICH);
                List<int> listUnlockId = new List<int>();

                bool retUnlockData = process.UnlockData(module,
                    function,
                    DatabaseConstant.Table.KT_GIAO_DICH,
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

        private void numTyGia_ValueChanged(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            try
            {
                QuyDoiNgoaiTeSangNoiTe(Convert.ToDecimal(numTyGia.Value));
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }
    }
}

