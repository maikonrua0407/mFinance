using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Collections;
using System.Reflection;
using System.Data;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using Microsoft.Windows.Controls.Ribbon;
using Utilities.Common;
using PresentationWPF.CustomControl;
using Presentation.Process;
using Presentation.Process.Common;
using Presentation.Process.PopupServiceRef;
using Presentation.Process.TinDungTDServiceRef;
using Presentation.Process.QuanTriHeThongServiceRef;
using PresentationWPF.BaoCao.DungChung;

namespace PresentationWPF.TinDungTD.KheUoc
{

    public partial class ucKheUocCT : UserControl
    {

        #region Khai bao
        private DatabaseConstant.Action action = DatabaseConstant.Action.THEM;
        public DatabaseConstant.Action Action
        {
            get { return action; }
            set { action = value; }
        }

        private DatabaseConstant.Function function = DatabaseConstant.Function.TDTD_KHE_UOC;

        public event EventHandler OnSavingCompleted;

        private TDTD_KHE_UOC _obj = new TDTD_KHE_UOC();
        public TDTD_KHE_UOC obj
        {
            get { return _obj; }
            set { _obj = value; }
        }

        List<AutoCompleteEntry> lstSourceLoaiTien = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceThoiHanVay = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourcePhuongThucGiaiNgan = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceLaiSuatTrongHan = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceLaiSuatQuaHan = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceHinhThucTraGoc = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceHinhThucTraLai = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceTanSuatTraGoc = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceTanSuatTraLai = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceCachTinhSoNgay = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourcePhuongThucDieuChinhNgayTraNo = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceNhomNo = new List<AutoCompleteEntry>();

        private string sTrangThaiNVu = "";

        private int tgianRut = 0;
        private string tgianRutDvi = "";

        List<DataRow> lstPopup = new List<DataRow>();
        public void LayDuLieuTuPopup(List<DataRow> lst)
        {
            lstPopup = lst;
        }

        private string cso_TINH_LAI = "";
        private double soTienTKBBCuaDXVV = 0;

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

        private decimal TW_TY_LE_TINH_SO_TIEN_MO_SO_TK = 0;
        private string ngayLapKheUoc = ClientInformation.NgayLamViecHienTai;
        string loaiSanPham;
        string ngayCoSoTinhKHoach;
        List<KE_HOACH_CHI_TIET> lstHienThiKeHoach;
        #endregion

        #region Khoi tao
        public ucKheUocCT()
        {
            InitializeComponent();

            DuyetQuyenTinhNang();

            BindShortkey();

            LoadCombobox();

            ResetForm();

            InitEventHandler();

            GetSystemParam();
        }

        private void DuyetQuyenTinhNang()
        {
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.TinDungTD;component/KheUoc/ucKheUocCT.xaml", ref Toolbar, ref mnuMain);
            foreach (var item in mnuMain.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += btnShortcutKey_Click;
            }
        }

        private void InitEventHandler()
        {

        }

        private void GetSystemParam()
        {
            try
            {
                string param = new UtilitiesProcess().LayGiaTriThamSo(BusinessConstant.LoaiThamSo.TW, BusinessConstant.MaThamSo.TW_TY_LE_TINH_SO_TIEN_MO_SO_TK, ClientInformation.MaDonVi);
                if (!param.IsNullOrEmptyOrSpace())
                {
                    TW_TY_LE_TINH_SO_TIEN_MO_SO_TK = Convert.ToDecimal(param);
                }
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
            else if (strTinhNang.Equals("PreviewKheUocRutVon"))
            {
                OnPreview();
            }
            else if (strTinhNang.Equals("PreviewKheUocRutVon_BLTBA"))
            {
                OnPreviewBaoLanhBenThuBa();
            }
            else if (strTinhNang.Equals("PreviewKHoach"))
            {
                OnPreviewPhanKyTraNo();
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
            else if (strTinhNang.Equals("PreviewKheUocRutVon"))
            {
                OnPreview();
            }
            else if (strTinhNang.Equals("PreviewKheUocRutVon_BLTBA"))
            {
                OnPreviewBaoLanhBenThuBa();
            }
            else if (strTinhNang.Equals("PreviewKHoach"))
            {
                OnPreviewPhanKyTraNo();
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
            if (_obj != null && _obj.objKuoc != null)
            {
                listLockId.Add(_obj.objKuoc.ID);

                bool ret = process.UnlockData(DatabaseConstant.Module.TDTD,
                    DatabaseConstant.Function.TDTD_KHE_UOC,
                    DatabaseConstant.Table.TDTD_KUOC,
                    DatabaseConstant.Action.SUA,
                    listLockId);
            }
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

        #region Load Combobox
        private void LoadCombobox()
        {

            COMBOBOX_DTO combo = null;
            List<string> lstDieuKien = new List<string>();
            List<COMBOBOX_DTO> lstCombobox = new List<COMBOBOX_DTO>();
            AutoComboBox auCombo = new AutoComboBox();

            try
            {

                //Loại tiền
                combo = new COMBOBOX_DTO();
                combo.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_LOAITIEN.getValue();
                combo.combobox = cmbLoaiTien;
                combo.lstSource = lstSourceLoaiTien;
                combo.maChon = ClientInformation.MaDongNoiTe;
                lstCombobox.Add(combo);

                //Thời hạn vay
                combo = null;
                combo = new COMBOBOX_DTO();
                lstDieuKien = new List<string>();
                lstDieuKien.Add(DatabaseConstant.DanhMuc.DVI_TINH_KY_HAN.getValue());
                combo.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();
                combo.lstDieuKien = lstDieuKien;
                combo.lstSource = lstSourceThoiHanVay;
                combo.combobox = cmbThoiHanVay;
                combo.maChon = BusinessConstant.KY_HAN_DVI_TINH.THANG.layGiaTri();
                lstCombobox.Add(combo);

                //Phương thức giải ngân
                combo = null;
                combo = new COMBOBOX_DTO();
                lstDieuKien = new List<string>();
                lstDieuKien.Add(DatabaseConstant.DanhMuc.HINH_THUC_GD.getValue());
                combo.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();
                combo.lstDieuKien = lstDieuKien;
                combo.lstSource = lstSourcePhuongThucGiaiNgan;
                combo.combobox = cmbPhuongThucGiaiNgan;
                combo.maChon = BusinessConstant.HINH_THUC_GIAO_DICH.TIEN_MAT.layGiaTri();
                lstCombobox.Add(combo);

                //Lãi suất trong hạn
                combo = null;
                combo = new COMBOBOX_DTO();
                lstDieuKien = new List<string>();
                lstDieuKien.Add(DatabaseConstant.DanhMuc.DVI_TINH_LSUAT.getValue());
                combo.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();
                combo.lstDieuKien = lstDieuKien;
                combo.lstSource = lstSourceLaiSuatTrongHan;
                combo.combobox = cmbLSuatTrongHanDViTinh;
                combo.maChon = BusinessConstant.KY_HAN_DVI_TINH.NAM.layGiaTri();
                lstCombobox.Add(combo);

                //Lãi suất quá hạn
                combo = null;
                combo = new COMBOBOX_DTO();
                lstDieuKien = new List<string>();
                lstDieuKien.Add(DatabaseConstant.DanhMuc.DVI_TINH_LSUAT.getValue());
                combo.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();
                combo.lstDieuKien = lstDieuKien;
                combo.lstSource = lstSourceLaiSuatQuaHan;
                combo.combobox = cmbLSuatQuaHanDViTinh;
                combo.maChon = BusinessConstant.KY_HAN_DVI_TINH.NAM.layGiaTri();
                lstCombobox.Add(combo);

                //Hình thức trả gốc
                combo = null;
                combo = new COMBOBOX_DTO();
                lstDieuKien = new List<string>();
                lstDieuKien.Add(DatabaseConstant.DanhMuc.HINH_THUC_TRA_GOC.getValue());
                combo.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();
                combo.lstDieuKien = lstDieuKien;
                combo.lstSource = lstSourceHinhThucTraGoc;
                combo.combobox = cmbHinhThucTraGoc;
                combo.maChon = BusinessConstant.HINH_THUC_TRA_GOC.DINH_KY.layGiaTri();
                lstCombobox.Add(combo);

                //Hình thức trả lãi
                combo = null;
                combo = new COMBOBOX_DTO();
                lstDieuKien = new List<string>();
                lstDieuKien.Add(DatabaseConstant.DanhMuc.HINH_THUC_TRA_LAI.getValue());
                combo.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();
                combo.lstDieuKien = lstDieuKien;
                combo.lstSource = lstSourceHinhThucTraLai;
                combo.combobox = cmbHinhThucTraLai;
                combo.maChon = BusinessConstant.HINH_THUC_TRA_LAI.DINH_KY.layGiaTri();
                lstCombobox.Add(combo);

                //Tần suất trả gốc
                combo = null;
                combo = new COMBOBOX_DTO();
                lstDieuKien = new List<string>();
                lstDieuKien.Add(DatabaseConstant.DanhMuc.TAN_SUAT.getValue());
                combo.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();
                combo.lstDieuKien = lstDieuKien;
                combo.lstSource = lstSourceTanSuatTraGoc;
                combo.combobox = cmbTanSuatTraGoc;
                combo.maChon = BusinessConstant.TAN_SUAT.THANG.layGiaTri();
                lstCombobox.Add(combo);

                //Tần suất trả lãi
                combo = null;
                combo = new COMBOBOX_DTO();
                lstDieuKien = new List<string>();
                lstDieuKien.Add(DatabaseConstant.DanhMuc.TAN_SUAT.getValue());
                combo.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();
                combo.lstDieuKien = lstDieuKien;
                combo.lstSource = lstSourceTanSuatTraLai;
                combo.combobox = cmbTanSuatTraLai;
                combo.maChon = BusinessConstant.TAN_SUAT.THANG.layGiaTri();
                lstCombobox.Add(combo);

                //Cách tính số ngày
                combo = null;
                combo = new COMBOBOX_DTO();
                lstDieuKien = new List<string>();
                lstDieuKien.Add(DatabaseConstant.DanhMuc.CACH_TINH_SO_NGAY_TD.getValue());
                combo.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();
                combo.lstDieuKien = lstDieuKien;
                combo.lstSource = lstSourceCachTinhSoNgay;
                combo.combobox = cmbCachTinhSoNgay;
                combo.maChon = BusinessConstant.CACH_TINH_SO_NGAY_TD.NGAY_THUC_TE.layGiaTri();
                lstCombobox.Add(combo);

                //Phương thức điều chỉnh ngày trả nợ
                combo = null;
                combo = new COMBOBOX_DTO();
                lstDieuKien = new List<string>();
                lstDieuKien.Add(DatabaseConstant.DanhMuc.NGAY_GD_ROI_VAO_NGAY_NGHI.getValue());
                combo.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();
                combo.lstDieuKien = lstDieuKien;
                combo.lstSource = lstSourcePhuongThucDieuChinhNgayTraNo;
                combo.combobox = cmbPhuongThucDieuChinhNgayTraNo;
                combo.maChon = BusinessConstant.NGAY_GD_ROI_VAO_NGAY_NGHI.NGAY_SAU.layGiaTri();
                lstCombobox.Add(combo);

                //Nhóm nợ hiện tại
                combo = null;
                combo = new COMBOBOX_DTO();
                lstDieuKien = new List<string>();
                lstDieuKien.Add(DatabaseConstant.DanhMuc.NHOM_NO.getValue());
                combo.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();
                combo.lstDieuKien = lstDieuKien;
                combo.lstSource = lstSourceNhomNo;
                combo.combobox = cmbNhomNoHienTai; ;
                combo.maChon = BusinessConstant.NHOM_NO.NHOM1.layGiaTri();
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

        #region Xử lý Popup
        private void btnMaHDTD_Click(object sender, RoutedEventArgs e)
        {
            lstPopup.Clear();
            List<string> lstDieuKien = new List<string>();
            lstDieuKien.Add(ClientInformation.MaDonViGiaoDich);
            lstDieuKien.Add(ClientInformation.NgayLamViecHienTai);
            PopupProcess popupProcess = new PopupProcess();
            popupProcess.getPopupInformation("POPUP_DS_TDTD_HDTD", lstDieuKien);
            SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;
            ucPopup popup = new ucPopup(false, simplePopupResponse);
            popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
            Window win = new Window();
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.Title = LLanguage.SearchResourceByKey(simplePopupResponse.PopupTitle);
            win.Content = popup;
            win.ShowDialog();
            if (lstPopup.Count > 0)
            {
                if (_obj.objKuoc == null)
                {
                    _obj.objKuoc = new THONG_TIN_KHE_UOC();
                }
                _obj.objKuoc.ID_HDTD = Convert.ToInt32(lstPopup[0]["ID"].ToString());
                _obj.objKuoc.SO_HDTD = lstPopup[0]["SO_HDTD"].ToString();
                _obj.objKuoc.MA_HDTD = lstPopup[0]["MA_HDTD"].ToString();
                _obj.objKuoc.MA_SAN_PHAM = lstPopup[0]["MA_SAN_PHAM"].ToString();
                _obj.objKuoc.ID_KHANG = Convert.ToInt32(lstPopup[0]["ID_KHANG"].ToString());
                _obj.objKuoc.MA_KHANG = lstPopup[0]["MA_KHANG"].ToString();
                txtSoHDTD.Text = lstPopup[0]["SO_HDTD"].ToString();
                txtHoTen.Text = lstPopup[0]["TEN_KHANG"].ToString();
                teldtNgayHopDong.Value = LDateTime.StringToDate(lstPopup[0]["NGAY_HD"].ToString(), "yyyyMMdd");
                teldtNgayDaoHanHD.Value = LDateTime.StringToDate(lstPopup[0]["NGAY_DAO_HAN"].ToString(), "yyyyMMdd");
                numLaiSuatTrongHan.Value = Convert.ToDouble(lstPopup[0]["LSUAT_VAY"]);
                numLaiSuatQuaHan.Value = Convert.ToDouble(lstPopup[0]["LSUAT_QHAN"]);
                cmbLSuatTrongHanDViTinh.SelectedIndex = lstSourceLaiSuatTrongHan.IndexOf(lstSourceLaiSuatTrongHan.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(lstPopup[0]["LSUAT_VAY_DVI_TINH"].ToString())));
                cmbLoaiTien.SelectedIndex = lstSourceLoaiTien.IndexOf(lstSourceLoaiTien.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(lstPopup[0]["LOAI_TIEN"].ToString())));
                if (lstPopup[0]["DIEN_GIAI"] != DBNull.Value)
                {
                    txtDienGiai.Text = lstPopup[0]["DIEN_GIAI"].ToString();
                }

                if (lstPopup[0]["SO_TIEN_VAY"] != DBNull.Value)
                {
                    numSoTienVay.Value = LNumber.StringToDouble(lstPopup[0]["SO_TIEN_VAY"].ToString());
                }
                else
                {
                    numSoTienVay.Value = 0;
                }

                if (lstPopup[0]["SO_TIEN_TKBB"] != DBNull.Value)
                {
                    soTienTKBBCuaDXVV = LNumber.StringToDouble(lstPopup[0]["SO_TIEN_TKBB"].ToString());
                }
                else
                {
                    soTienTKBBCuaDXVV = 0;
                }

                if (lstPopup[0]["SO_TIEN_DA_GNGAN"] != DBNull.Value)
                {
                    numSoTienGiaiNgan.Value = LNumber.StringToDouble(lstPopup[0]["SO_TIEN_DA_GNGAN"].ToString());
                }
                else
                {
                    numSoTienGiaiNgan.Value = 0;
                }

                if (lstPopup[0]["TRGOC_HTHUC"] != DBNull.Value)
                {
                    cmbHinhThucTraGoc.SelectedIndex = lstSourceHinhThucTraGoc.IndexOf(lstSourceHinhThucTraGoc.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(lstPopup[0]["TRGOC_HTHUC"].ToString())));
                }

                if (lstPopup[0]["TRLAI_HTHUC"] != DBNull.Value)
                {
                    cmbHinhThucTraLai.SelectedIndex = lstSourceHinhThucTraLai.IndexOf(lstSourceHinhThucTraLai.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(lstPopup[0]["TRLAI_HTHUC"].ToString())));
                }

                if (lstPopup[0]["TGIAN_RUT"] != DBNull.Value)
                {
                    tgianRut = Convert.ToInt32(lstPopup[0]["TGIAN_RUT"]);
                    tgianRutDvi = lstPopup[0]["TGIAN_RUT_DVI_TINH"].ToString();
                }

                if (lstPopup[0]["LOAI_SAN_PHAM"] != DBNull.Value)
                {
                    loaiSanPham = lstPopup[0]["LOAI_SAN_PHAM"].ToString();
                    if (loaiSanPham.Equals("TDTD_QLOAN"))
                    {
                        if (lstPopup[0]["NGAY_TGOC_MDINH"] != DBNull.Value)
                        {
                            txtNgayTraGoc.Text = lstPopup[0]["NGAY_TGOC_MDINH"].ToString();
                        }
                        if (lstPopup[0]["NGAY_TLAI_MDINH"] != DBNull.Value)
                        {
                            txtNgayTraLai.Text = lstPopup[0]["NGAY_TLAI_MDINH"].ToString();
                        }
                        if (lstPopup[0]["NGAY_CSO_TINH_KH"] != DBNull.Value)
                        {
                            ngayCoSoTinhKHoach = lstPopup[0]["NGAY_CSO_TINH_KH"].ToString();
                        }
                        numTanSuatTraGoc.Value = 1;
                        numTanSuatTraLai.Value = 1;
                        numSoTienNhanNo.Value = numSoTienVay.Value - numSoTienGiaiNgan.Value;
                        numThoiHanVay.Value = LNumber.StringToDouble(lstPopup[0]["TGIAN_VAY"].ToString());
                        cmbThoiHanVay.SelectedIndex = lstSourceThoiHanVay.IndexOf(lstSourceThoiHanVay.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(lstPopup[0]["TGIAN_VAY_DVI_TINH"].ToString())));
                        if (lstPopup[0]["TGIAN_VAY_DVI_TINH"].ToString().Equals(BusinessConstant.TAN_SUAT.THANG.layGiaTri()))
                            teldtNgayDaoHan.Value = teldtNgayGiaiNgan.Value.GetValueOrDefault().AddMonths(Convert.ToInt32(numThoiHanVay.Value.GetValueOrDefault()));
                        else if (lstPopup[0]["TGIAN_VAY_DVI_TINH"].ToString().Equals(BusinessConstant.TAN_SUAT.NGAY.layGiaTri()))
                            teldtNgayDaoHan.Value = teldtNgayGiaiNgan.Value.GetValueOrDefault().AddDays(Convert.ToInt32(numThoiHanVay.Value.GetValueOrDefault()));
                        else if (lstPopup[0]["TGIAN_VAY_DVI_TINH"].ToString().Equals(BusinessConstant.TAN_SUAT.NAM.layGiaTri()))
                            teldtNgayDaoHan.Value = teldtNgayGiaiNgan.Value.GetValueOrDefault().AddYears(Convert.ToInt32(numThoiHanVay.Value.GetValueOrDefault()));
                        if (lstPopup[0]["NGAY_MUA_HANG"] != DBNull.Value)
                        {
                            _obj.objKuoc.NGAY_GIAI_NGAN = lstPopup[0]["NGAY_MUA_HANG"].ToString();
                            teldtNgayGiaiNgan.Value = LDateTime.StringToDate(_obj.objKuoc.NGAY_GIAI_NGAN,
                                ApplicationConstant.defaultDateTimeFormat);
                        }
                    }
                }

                cso_TINH_LAI = lstPopup[0]["MA_CSO_TLAI"].ToString();
            }
        }
        #endregion

        private void txtSoHDTD_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void numThoiHanVay_ValueChanged(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            TinhToanNgayDaoHan();
        }

        private void cmbThoiHanVay_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TinhToanNgayDaoHan();
        }

        private void TinhToanNgayDaoHan()
        {
            try
            {
                if (teldtNgayGiaiNgan.Value != null)
                {
                    if (numThoiHanVay.Value != null && numThoiHanVay.Value != 0)
                    {
                        string dviTinh = lstSourceThoiHanVay.ElementAt(cmbThoiHanVay.SelectedIndex).KeywordStrings.FirstOrDefault();
                        teldtNgayDaoHan.Value = LDateTime.PlusDaysComposite(teldtNgayGiaiNgan.Value.Value, Convert.ToInt32(numThoiHanVay.Value), dviTinh);
                        if (teldtNgayDaoHanHD.Value != null)
                        {
                            if (teldtNgayDaoHan.Value.Value > LDateTime.PlusDaysComposite(teldtNgayDaoHanHD.Value.Value, tgianRut, tgianRutDvi))
                            {
                                //CommonFunction.ThongBaoLoi("Ngày đáo hạn của khế ước phải nhỏ hơn hợp đồng");
                                LMessage.ShowMessage("M_ResponseMessage_KUOCVM_NgayDaoHanKhongHopLe", LMessage.MessageBoxType.Warning);
                                numThoiHanVay.Focus();
                            }
                        }
                    }
                    else
                    {
                        teldtNgayDaoHan.Value = null;
                    }
                }
                else
                {
                    teldtNgayDaoHan.Value = null;
                }
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        private void teldtNgayGiaiNgan_ValueChanged(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            TinhToanNgayDaoHan();
        }

        private void TaoKeHoachTuDong()
        {
            List<KE_HOACH_CHI_TIET> lstKeHoach = new List<KE_HOACH_CHI_TIET>();
            try
            {
                #region Tinh lai
                GetFormData(sTrangThaiNVu);
                TinDungTDProcess process = new TinDungTDProcess();
                List<ClientResponseDetail> lstClientResponse = new List<ClientResponseDetail>();
                process.KheUocTieuDung(DatabaseConstant.Action.TINH_TOAN_LICH_TRA_NO, ref _obj, ref lstClientResponse);
                #endregion

                #region Gop lai de phuc vu hien thi
                List<KE_HOACH_CHI_TIET> lstTemp = new List<KE_HOACH_CHI_TIET>();
                List<KE_HOACH_CHI_TIET> lstHienThiKeHoach = new List<KE_HOACH_CHI_TIET>();

                lstTemp = (from f in _obj.lstKeHoachCT
                           select new KE_HOACH_CHI_TIET
                           {
                               LOAI = f.LOAI,
                               TU_NGAY = f.TU_NGAY,
                               DEN_NGAY = f.DEN_NGAY,
                               NGAY_BAT_DAU = f.NGAY_BAT_DAU,
                               NGAY_KET_THUC = f.NGAY_KET_THUC,
                               SO_NGAY = f.SO_NGAY,
                               SO_TIEN_GOC = f.SO_TIEN_GOC,
                               SO_TIEN_LAI = f.SO_TIEN_LAI,
                               SO_DU = f.SO_DU,
                               LAI_SUAT = f.LAI_SUAT,
                               LAI_SUAT_DVI_TINH = f.LAI_SUAT_DVI_TINH,
                               TRANG_THAI = f.TRANG_THAI
                           }).ToList();

                lstTemp = lstTemp.OrderBy(e => e.DEN_NGAY).ToList();
                foreach (KE_HOACH_CHI_TIET objTemp in lstTemp)
                {
                    if (lstHienThiKeHoach.Select(e => e.DEN_NGAY).Contains(objTemp.DEN_NGAY))
                    {
                        int index = lstHienThiKeHoach.IndexOf(lstHienThiKeHoach.FirstOrDefault(e => e.DEN_NGAY.Equals(objTemp.DEN_NGAY)));
                        if (objTemp.LOAI == "TRA_GOC")
                        {
                            lstHienThiKeHoach[index].SO_DU = objTemp.SO_DU;
                        }
                        lstHienThiKeHoach[index].SO_TIEN_GOC += objTemp.SO_TIEN_GOC;
                        lstHienThiKeHoach[index].SO_TIEN_LAI += objTemp.SO_TIEN_LAI;
                    }
                    else
                    {
                        lstHienThiKeHoach.Add(objTemp);
                    }
                }

                #endregion

                #region Rebind
                //raddgrKeHoachDS.ItemsSource = _obj.lstKeHoachCT.ToList();
                raddgrKeHoachDS.ItemsSource = lstHienThiKeHoach;
                raddgrKeHoachDS.Rebind();
                #endregion
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        private void btnLapKeHoach_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateTaoKeHoach())
            {
                TaoKeHoachTuDong();
            }
        }

        private void btnAddKeHoach_Click(object sender, RoutedEventArgs e)
        {
            KE_HOACH_CHI_TIET obj = new KE_HOACH_CHI_TIET();
            if (raddgrKeHoachDS.Items.Count > 0)
            {
                obj.TU_NGAY = ((KE_HOACH_CHI_TIET)raddgrKeHoachDS.Items[raddgrKeHoachDS.Items.Count - 1]).DEN_NGAY;
                obj.NGAY_BAT_DAU = LDateTime.StringToDate(obj.TU_NGAY, "yyyyMMdd");
                obj.DEN_NGAY = obj.TU_NGAY;
                obj.NGAY_KET_THUC = obj.NGAY_BAT_DAU;
                obj.SO_NGAY = 0;

            }
            else
            {
                obj.TU_NGAY = LDateTime.DateToString(teldtNgayGiaiNgan.Value.Value, "yyyyMMdd");
                obj.NGAY_BAT_DAU = teldtNgayGiaiNgan.Value.Value;
                obj.DEN_NGAY = LDateTime.DateToString(teldtNgayGiaiNgan.Value.Value, "yyyyMMdd");
                obj.NGAY_KET_THUC = teldtNgayGiaiNgan.Value.Value;
                obj.SO_NGAY = 0;
            }
            raddgrKeHoachDS.Items.Add(obj);
            raddgrKeHoachDS.Rebind();
        }

        private void btnModifyKeHoach_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnDeleteKeHoach_Click(object sender, RoutedEventArgs e)
        {
            for (int i = raddgrKeHoachDS.SelectedItems.Count - 1; i >= 0; i--)
            {
                raddgrKeHoachDS.Items.Remove(raddgrKeHoachDS.SelectedItems[i]);
            }
        }

        private void cmbTanSuatTraGoc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbTanSuatTraGoc.SelectedIndex != -1)
            {
                string tanSuatTraGoc = lstSourceTanSuatTraGoc.ElementAt(cmbTanSuatTraGoc.SelectedIndex).KeywordStrings.FirstOrDefault();
                if (tanSuatTraGoc.Equals(BusinessConstant.TAN_SUAT.NGAY.layGiaTri()))
                {
                    txtNgayTraGoc.Text = "";
                    txtNgayTraGoc.IsEnabled = false;
                    chkLuuNgayTraGoc.IsChecked = false;
                    chkLuuNgayTraGoc.IsEnabled = false;
                }
                else if (tanSuatTraGoc.Equals(BusinessConstant.TAN_SUAT.THANG.layGiaTri()))
                {
                    txtNgayTraGoc.Text = "";
                    txtNgayTraGoc.IsEnabled = true;
                    chkLuuNgayTraGoc.IsChecked = true;
                    chkLuuNgayTraGoc.IsEnabled = true;
                }
            }
        }

        private void cmbTanSuatTraLai_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbTanSuatTraLai.SelectedIndex != -1)
            {
                string tanSuatTraLai = lstSourceTanSuatTraLai.ElementAt(cmbTanSuatTraLai.SelectedIndex).KeywordStrings.FirstOrDefault();
                if (tanSuatTraLai.Equals(BusinessConstant.TAN_SUAT.NGAY.layGiaTri()))
                {
                    txtNgayTraLai.Text = "";
                    txtNgayTraLai.IsEnabled = false;
                    chkLuuNgayTraLai.IsChecked = false;
                    chkLuuNgayTraLai.IsEnabled = false;
                }
                else if (tanSuatTraLai.Equals(BusinessConstant.TAN_SUAT.THANG.layGiaTri()))
                {
                    txtNgayTraLai.Text = "";
                    txtNgayTraLai.IsEnabled = true;
                    chkLuuNgayTraLai.IsChecked = true;
                    chkLuuNgayTraLai.IsEnabled = true;
                }
            }
        }

        private void txtNgayTraGoc_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        private Boolean IsTextAllowed(String text)
        {
            return Array.TrueForAll<Char>(text.ToCharArray(),
                delegate (Char c) { return Char.IsDigit(c) || Char.IsControl(c); });
        }

        private void cmbHinhThucTraGoc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cmbHinhThucTraLai_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void numSoTienNhanNo_ValueChanged(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (loaiSanPham.Equals("TDTD_QLOAN"))
                numSoTienTKBB.Value = 0;
            else
            {
                if(soTienTKBBCuaDXVV > 0)
                    numSoTienTKBB.Value = numSoTienNhanNo.Value * Convert.ToDouble(TW_TY_LE_TINH_SO_TIEN_MO_SO_TK) / 100;
                else
                    numSoTienTKBB.Value = 0;
            }
        }
        #endregion

        #region Xử lý nghiệp vụ
        private void GetFormData(string sTrangThaiNVu)
        {
            try
            {
                if (_obj == null)
                {
                    _obj = new TDTD_KHE_UOC();
                }

                if (_obj.objKuoc == null)
                {
                    _obj.objKuoc = new THONG_TIN_KHE_UOC();
                }

                if (_obj.objKeHoach == null)
                {
                    _obj.objKeHoach = new THONG_TIN_KE_HOACH();
                }

                _obj.MA_DVI_QLY = ClientInformation.MaDonVi;
                _obj.MA_DVI_TAO = ClientInformation.MaDonViGiaoDich;
                _obj.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                _obj.NGUOI_NHAP = txtNguoiLap.Text;
                _obj.NGAY_NHAP = Convert.ToDateTime(teldtNgayNhap.Value).ToString("yyyyMMdd");
                _obj.TTHAI_NVU = sTrangThaiNVu;
                if (action != DatabaseConstant.Action.THEM)
                {
                    _obj.NGAY_CNHAT = ClientInformation.NgayLamViecHienTai;
                    _obj.NGUOI_CNHAT = ClientInformation.TenDangNhap;
                }

                #region TDTD_KUOC

                _obj.objKuoc.SO_TIEN_NHAN_NO = Convert.ToDecimal(numSoTienNhanNo.Value);
                _obj.objKuoc.THOI_HAN_VAY = Convert.ToInt32(numThoiHanVay.Value);
                _obj.objKuoc.THOI_HAN_VAY_DVI = lstSourceThoiHanVay[cmbThoiHanVay.SelectedIndex].KeywordStrings[0];
                _obj.objKuoc.PHUONG_THUC_GIAI_NGAN = lstSourcePhuongThucGiaiNgan[cmbPhuongThucGiaiNgan.SelectedIndex].KeywordStrings[0];
                _obj.objKuoc.NGAY_LAP_KUOC = ngayLapKheUoc;
                if (teldtNgayGiaiNgan.Value != null)
                {
                    _obj.objKuoc.NGAY_GIAI_NGAN = LDateTime.DateToString(teldtNgayGiaiNgan.Value.Value, "yyyyMMdd");
                }

                if (teldtNgayDaoHan.Value != null)
                {
                    _obj.objKuoc.NGAY_DAO_HAN = LDateTime.DateToString(teldtNgayDaoHan.Value.Value, "yyyyMMdd");
                }

                if (teldtNgayTinhLai.Value != null)
                {
                    _obj.objKuoc.NGAY_TINH_LAI = LDateTime.DateToString(teldtNgayTinhLai.Value.Value, "yyyyMMdd");
                }
                _obj.objKuoc.LSUAT_TRONG_HAN = Convert.ToDecimal(numLaiSuatTrongHan.Value);
                _obj.objKuoc.LSUAT_TRONG_HAN_DVI = lstSourceLaiSuatTrongHan[cmbLSuatTrongHanDViTinh.SelectedIndex].KeywordStrings[0];
                _obj.objKuoc.LSUAT_QUA_HAN = Convert.ToDecimal(numLaiSuatQuaHan.Value);
                _obj.objKuoc.LSUAT_QUA_HAN_DVI = lstSourceLaiSuatQuaHan[cmbLSuatQuaHanDViTinh.SelectedIndex].KeywordStrings[0];
                _obj.objKuoc.LOAI_TIEN = lstSourceLoaiTien[cmbLoaiTien.SelectedIndex].KeywordStrings[0];
                _obj.objKuoc.NHOM_NO_HIEN_TAI = lstSourceNhomNo[cmbNhomNoHienTai.SelectedIndex].KeywordStrings[0];
                _obj.objKuoc.CSO_TINH_LAI = cso_TINH_LAI;
                _obj.objKuoc.TTHAI_KUOC = BusinessConstant.TRANG_THAI_TAT_TOAN.CHUA_GIAI_NGAN.layGiaTri();

                _obj.objKuoc.SO_TIEN_TKBB = Convert.ToDecimal(numSoTienTKBB.Value);
                if (teldtNgayGiaiNgan.Value != null)
                {
                    _obj.objKuoc.DU_THU_DEN_NGAY = LDateTime.DateToString(teldtNgayGiaiNgan.Value.Value, "yyyyMMdd");
                }
                _obj.objKuoc.LAI_DU_THU_LUY_KE = Convert.ToDecimal(numDuThuLuyKe.Value);

                if (teldtPhanBoDenNgay.Value != null)
                {
                    _obj.objKuoc.PHAN_BO_DEN_NGAY = LDateTime.DateToString(teldtPhanBoDenNgay.Value.Value, "yyyyMMdd");
                }

                _obj.objKuoc.TONG_LAI_TRA_TRUOC = Convert.ToDecimal(numTongLaiTraTruoc.Value);
                _obj.objKuoc.SO_TIEN_DA_PHAN_BO = Convert.ToDecimal(numSoTienDaPhanBo.Value);
                _obj.objKuoc.SO_TIEN_CHO_PHAN_BO = Convert.ToDecimal(numSoTiencHOPhanBo.Value);

                #endregion

                #region TDTD_KHOACH

                _obj.objKeHoach = new THONG_TIN_KE_HOACH();
                if (chkLapKeHoachTuDong.IsChecked.Value)
                {
                    _obj.objKeHoach.LAP_KE_HOACH_TU_DONG = true;
                    _obj.objKeHoach.HINH_THUC_TRA_GOC = lstSourceHinhThucTraGoc[cmbHinhThucTraGoc.SelectedIndex].KeywordStrings[0];
                    _obj.objKeHoach.HINH_THUC_TRA_LAI = lstSourceHinhThucTraLai[cmbHinhThucTraLai.SelectedIndex].KeywordStrings[0];
                    _obj.objKeHoach.TAN_SUAT_TRA_GOC = Convert.ToInt32(numTanSuatTraGoc.Value);
                    _obj.objKeHoach.TAN_SUAT_TRA_GOC_DVI = lstSourceTanSuatTraGoc[cmbTanSuatTraGoc.SelectedIndex].KeywordStrings[0];
                    _obj.objKeHoach.TAN_SUAT_TRA_LAI = Convert.ToInt32(numTanSuatTraLai.Value);
                    _obj.objKeHoach.TAN_SUAT_TRA_LAI_DVI = lstSourceTanSuatTraLai[cmbTanSuatTraLai.SelectedIndex].KeywordStrings[0];
                    _obj.objKeHoach.NGAY_TRA_GOC = txtNgayTraGoc.Text.Trim();
                    _obj.objKeHoach.LUU_NGAY_TRA_GOC = chkLuuNgayTraGoc.IsChecked.Value;
                    _obj.objKeHoach.NGAY_TRA_LAI = txtNgayTraLai.Text.Trim();
                    _obj.objKeHoach.LUU_NGAY_TRA_LAI = chkLuuNgayTraLai.IsChecked.Value;

                    if (cmbCachTinhSoNgay.SelectedIndex != -1)
                    {
                        _obj.objKeHoach.CACH_TINH_SO_NGAY = lstSourceCachTinhSoNgay[cmbCachTinhSoNgay.SelectedIndex].KeywordStrings[0];
                    }

                    if (cmbPhuongThucDieuChinhNgayTraNo.SelectedIndex != -1)
                    {
                        _obj.objKeHoach.DIEU_CHINH_NGAY_TRA_NO = lstSourcePhuongThucDieuChinhNgayTraNo[cmbPhuongThucDieuChinhNgayTraNo.SelectedIndex].KeywordStrings[0];
                    }
                }
                else
                {
                    _obj.objKeHoach.LAP_KE_HOACH_TU_DONG = false;
                }


                #endregion

                #region TDTD_KHOACH_CT
                //var lstKeHoach = raddgrKeHoachDS.ItemsSource;
                //if (lstKeHoach == null)
                //{
                //    lstKeHoach = new List<KE_HOACH_CHI_TIET>();
                //}
                //List<KE_HOACH_CHI_TIET> lstConvert = (List<KE_HOACH_CHI_TIET>)lstKeHoach;
                //_obj.lstKeHoachCT = lstConvert.ToArray();
                #endregion
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        private void SetFormData()
        {
            TinDungTDProcess processTinDungTD = new TinDungTDProcess();
            List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();

            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                int ret = 0;
                ret = processTinDungTD.KheUocTieuDung(DatabaseConstant.Action.LOAD, ref _obj, ref listClientResponseDetail);
                if (ret > 0)
                {
                    if (_obj.objKuoc != null)
                    {
                        sTrangThaiNVu = _obj.TTHAI_NVU;
                        lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);

                        //Group hop dong
                        txtSoHDTD.Text = _obj.objKuoc.SO_HDTD;
                        txtSoHDTD.Tag = _obj.objKuoc.MA_HDTD;

                        DataSet ds = new DataSet();
                        DataTable dtPar = null;
                        LDatatable.MakeParameterTable(ref dtPar);
                        LDatatable.AddParameter(ref dtPar, "@SoGiaoDich", "String", _obj.objKuoc.MA_HDTD);
                        ds = new TinDungTDProcess().HopDongTinDungCaNhanChiTiet(dtPar);
                        if (ds != null && ds.Tables.Count > 0 && ds.Tables["TTIN_CHUNG"] != null && ds.Tables["TTIN_CHUNG"].Rows.Count > 0)
                        {
                            tgianRut = Convert.ToInt32(ds.Tables["TTIN_CHUNG"].Rows[0]["TGIAN_RUT"]);
                            tgianRutDvi = ds.Tables["TTIN_CHUNG"].Rows[0]["TGIAN_RUT_DVI_TINH"].ToString();
                            cmbLoaiTien.SelectedIndex = lstSourceLoaiTien.IndexOf(lstSourceLoaiTien.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(ds.Tables["TTIN_CHUNG"].Rows[0]["MA_LOAI_TIEN"].ToString())));
                            txtHoTen.Text = ds.Tables["TTIN_CHUNG"].Rows[0]["TEN_KHANG"].ToString();
                            teldtNgayHopDong.Value = LDateTime.StringToDate(ds.Tables["TTIN_CHUNG"].Rows[0]["NGAY_HD"].ToString(), "yyyyMMdd");
                            teldtNgayDaoHanHD.Value = LDateTime.StringToDate(ds.Tables["TTIN_CHUNG"].Rows[0]["NGAY_DAO_HAN"].ToString(), "yyyyMMdd");
                            numSoTienVay.Value = Convert.ToDouble(ds.Tables["TTIN_CHUNG"].Rows[0]["SO_TIEN_VAY"]);
                            if (ds.Tables["TTIN_CHUNG"].Rows[0]["SO_TIEN_DA_GNGAN"] != DBNull.Value)
                            {
                                numSoTienGiaiNgan.Value = Convert.ToDouble(ds.Tables["TTIN_CHUNG"].Rows[0]["SO_TIEN_DA_GNGAN"]);
                            }
                            teldtNgayNhap.Value = LDateTime.StringToDate(ds.Tables["TTIN_CHUNG"].Rows[0]["NGAY_NHAP"].ToString(), "yyyyMMdd");
                            txtNguoiLap.Text = ds.Tables["TTIN_CHUNG"].Rows[0]["NGUOI_NHAP"].ToString();
                            if (ds.Tables["TTIN_CHUNG"].Rows[0]["NGAY_CNHAT"] != DBNull.Value)
                                teldtNgayCNhat.Value = LDateTime.StringToDate(ds.Tables["TTIN_CHUNG"].Rows[0]["NGAY_CNHAT"].ToString(), "yyyyMMdd");
                            txtNguoiCapNhat.Text = ds.Tables["TTIN_CHUNG"].Rows[0]["NGUOI_CNHAT"].ToString();

                        }

                        //Group khe uoc
                        ngayLapKheUoc = _obj.objKuoc.NGAY_LAP_KUOC;
                        txtSoKheUoc.Text = _obj.objKuoc.SO_KHE_UOC;
                        txtSoKheUoc.Tag = _obj.objKuoc.MA_KHE_UOC;
                        numSoTienNhanNo.Value = Convert.ToDouble(_obj.objKuoc.SO_TIEN_NHAN_NO);
                        numThoiHanVay.Value = Convert.ToDouble(_obj.objKuoc.THOI_HAN_VAY);
                        cmbThoiHanVay.SelectedIndex = lstSourceThoiHanVay.IndexOf(lstSourceThoiHanVay.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(_obj.objKuoc.THOI_HAN_VAY_DVI)));
                        cmbPhuongThucGiaiNgan.SelectedIndex = lstSourcePhuongThucGiaiNgan.IndexOf(lstSourcePhuongThucGiaiNgan.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(_obj.objKuoc.PHUONG_THUC_GIAI_NGAN)));
                        teldtNgayGiaiNgan.Value = LDateTime.StringToDate(_obj.objKuoc.NGAY_GIAI_NGAN, "yyyyMMdd");
                        teldtNgayDaoHan.Value = LDateTime.StringToDate(_obj.objKuoc.NGAY_DAO_HAN, "yyyyMMdd");
                        teldtNgayTinhLai.Value = LDateTime.StringToDate(_obj.objKuoc.NGAY_TINH_LAI, "yyyyMMdd");
                        numLaiSuatTrongHan.Value = Convert.ToDouble(_obj.objKuoc.LSUAT_TRONG_HAN);
                        numLaiSuatQuaHan.Value = Convert.ToDouble(_obj.objKuoc.LSUAT_QUA_HAN);
                        cmbLSuatTrongHanDViTinh.SelectedIndex = lstSourceLaiSuatTrongHan.IndexOf(lstSourceLaiSuatTrongHan.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(_obj.objKuoc.LSUAT_TRONG_HAN_DVI)));
                        numLaiSuatQuaHan.Value = Convert.ToDouble(_obj.objKuoc.LSUAT_QUA_HAN);
                        cmbLSuatQuaHanDViTinh.SelectedIndex = lstSourceLaiSuatQuaHan.IndexOf(lstSourceLaiSuatQuaHan.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(_obj.objKuoc.LSUAT_QUA_HAN_DVI)));
                        cso_TINH_LAI = _obj.objKuoc.CSO_TINH_LAI;
                        numSoTienTKBB.Value = Convert.ToDouble(_obj.objKuoc.SO_TIEN_TKBB);

                        //Group ke hoach tra no
                        if (_obj.objKeHoach.LAP_KE_HOACH_TU_DONG)
                        {
                            chkLapKeHoachTuDong.IsChecked = true;
                            grbDieuKienLapKeHoach.IsEnabled = true;
                        }
                        else
                        {
                            chkLapKeHoachThuCong.IsChecked = true;
                            grbDieuKienLapKeHoach.IsEnabled = false;
                        }

                        cmbHinhThucTraGoc.SelectedIndex = lstSourceHinhThucTraGoc.IndexOf(lstSourceHinhThucTraGoc.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(_obj.objKeHoach.HINH_THUC_TRA_GOC)));
                        cmbHinhThucTraLai.SelectedIndex = lstSourceHinhThucTraLai.IndexOf(lstSourceHinhThucTraLai.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(_obj.objKeHoach.HINH_THUC_TRA_LAI)));
                        numTanSuatTraGoc.Value = Convert.ToDouble(_obj.objKeHoach.TAN_SUAT_TRA_GOC);
                        cmbTanSuatTraGoc.SelectedIndex = lstSourceTanSuatTraGoc.IndexOf(lstSourceTanSuatTraGoc.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(_obj.objKeHoach.TAN_SUAT_TRA_GOC_DVI)));
                        numTanSuatTraLai.Value = Convert.ToDouble(_obj.objKeHoach.TAN_SUAT_TRA_LAI);
                        cmbTanSuatTraLai.SelectedIndex = lstSourceTanSuatTraLai.IndexOf(lstSourceTanSuatTraLai.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(_obj.objKeHoach.TAN_SUAT_TRA_LAI_DVI)));

                        chkLuuNgayTraGoc.IsChecked = _obj.objKeHoach.LUU_NGAY_TRA_GOC;

                        txtNgayTraGoc.Text = _obj.objKeHoach.NGAY_TRA_GOC;


                        chkLuuNgayTraLai.IsChecked = _obj.objKeHoach.LUU_NGAY_TRA_LAI;

                        txtNgayTraLai.Text = _obj.objKeHoach.NGAY_TRA_LAI;


                        if (!string.IsNullOrEmpty(_obj.objKeHoach.CACH_TINH_SO_NGAY))
                        {
                            cmbCachTinhSoNgay.SelectedIndex = lstSourceCachTinhSoNgay.IndexOf(lstSourceCachTinhSoNgay.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(_obj.objKeHoach.CACH_TINH_SO_NGAY)));
                        }
                        else
                        {
                            cmbCachTinhSoNgay.SelectedIndex = -1;
                        }

                        if (!string.IsNullOrEmpty(_obj.objKeHoach.DIEU_CHINH_NGAY_TRA_NO))
                        {
                            cmbPhuongThucDieuChinhNgayTraNo.SelectedIndex = lstSourcePhuongThucDieuChinhNgayTraNo.IndexOf(lstSourcePhuongThucDieuChinhNgayTraNo.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(_obj.objKeHoach.DIEU_CHINH_NGAY_TRA_NO)));
                        }
                        else
                        {
                            cmbPhuongThucDieuChinhNgayTraNo.SelectedIndex = -1;
                        }

                        #region Gop lai de phuc vu hien thi
                        List<KE_HOACH_CHI_TIET> lstTemp = new List<KE_HOACH_CHI_TIET>();
                        lstHienThiKeHoach = new List<KE_HOACH_CHI_TIET>();

                        lstTemp = (from f in _obj.lstKeHoachCT
                                   select new KE_HOACH_CHI_TIET
                                   {
                                       LOAI = f.LOAI,
                                       TU_NGAY = f.TU_NGAY,
                                       DEN_NGAY = f.DEN_NGAY,
                                       NGAY_BAT_DAU = f.NGAY_BAT_DAU,
                                       NGAY_KET_THUC = f.NGAY_KET_THUC,
                                       SO_NGAY = f.SO_NGAY,
                                       SO_TIEN_GOC = f.SO_TIEN_GOC,
                                       SO_TIEN_LAI = f.SO_TIEN_LAI,
                                       SO_DU = f.SO_DU,
                                       LAI_SUAT = f.LAI_SUAT,
                                       LAI_SUAT_DVI_TINH = f.LAI_SUAT_DVI_TINH,
                                       TRANG_THAI = f.TRANG_THAI
                                   }).ToList();

                        lstTemp = lstTemp.OrderBy(e => e.DEN_NGAY).ToList();
                        foreach (KE_HOACH_CHI_TIET objTemp in lstTemp)
                        {
                            if (lstHienThiKeHoach.Select(e => e.DEN_NGAY).Contains(objTemp.DEN_NGAY))
                            {
                                int index = lstHienThiKeHoach.IndexOf(lstHienThiKeHoach.FirstOrDefault(e => e.DEN_NGAY.Equals(objTemp.DEN_NGAY)));
                                if (objTemp.LOAI == "TRA_GOC")
                                {
                                    lstHienThiKeHoach[index].SO_DU = objTemp.SO_DU;
                                }
                                lstHienThiKeHoach[index].SO_TIEN_GOC += objTemp.SO_TIEN_GOC;
                                lstHienThiKeHoach[index].SO_TIEN_LAI += objTemp.SO_TIEN_LAI;
                            }
                            else
                            {
                                lstHienThiKeHoach.Add(objTemp);
                            }
                        }

                        #endregion
                        //raddgrKeHoachDS.ItemsSource = _obj.lstKeHoachCT.ToList();
                        raddgrKeHoachDS.ItemsSource = lstHienThiKeHoach;
                        raddgrKeHoachDS.Rebind();

                        //Group Thong Tin Khac
                        if (!_obj.objKuoc.DU_THU_DEN_NGAY.IsNullOrEmptyOrSpace())
                        {
                            teldtDuThuDenNgay.Value = LDateTime.StringToDate(_obj.objKuoc.DU_THU_DEN_NGAY, "yyyyMMdd");
                        }
                        numDuThuLuyKe.Value = Convert.ToDouble(_obj.objKuoc.LAI_DU_THU_LUY_KE);

                        if (!_obj.objKuoc.PHAN_BO_DEN_NGAY.IsNullOrEmptyOrSpace())
                        {
                            teldtPhanBoDenNgay.Value = LDateTime.StringToDate(_obj.objKuoc.PHAN_BO_DEN_NGAY, "yyyyMMdd");
                        }
                        else
                        {
                            teldtPhanBoDenNgay.Value = null;
                        }
                        numTongLaiTraTruoc.Value = Convert.ToDouble(_obj.objKuoc.TONG_LAI_TRA_TRUOC);
                        numSoTienDaPhanBo.Value = Convert.ToDouble(_obj.objKuoc.SO_TIEN_DA_PHAN_BO);
                        numSoTiencHOPhanBo.Value = Convert.ToDouble(_obj.objKuoc.SO_TIEN_CHO_PHAN_BO);
                        cmbNhomNoHienTai.SelectedIndex = lstSourceNhomNo.IndexOf(lstSourceNhomNo.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(_obj.objKuoc.NHOM_NO_HIEN_TAI)));
                        ProcessDataThucThu();
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
            sTrangThaiNVu = "";
            lblTrangThai.Content = "";
            _obj = new TDTD_KHE_UOC();

            #region Thong tin hop dong
            txtSoHDTD.Text = "";
            txtHoTen.Text = "";
            teldtNgayHopDong.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
            cmbLoaiTien.SelectedIndex = lstSourceLoaiTien.IndexOf(lstSourceLoaiTien.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(ClientInformation.MaDongNoiTe)));
            teldtNgayDaoHanHD.Value = null;
            numSoTienVay.Value = 0;
            numSoTienGiaiNgan.Value = 0;
            #endregion

            #region Thong tin khe uoc
            txtSoKheUoc.Text = "";
            numSoTienNhanNo.Value = 0;
            numThoiHanVay.Value = 0;
            cmbThoiHanVay.SelectedIndex = lstSourceThoiHanVay.IndexOf(lstSourceThoiHanVay.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(BusinessConstant.KY_HAN_DVI_TINH.THANG.layGiaTri())));
            cmbPhuongThucGiaiNgan.SelectedIndex = lstSourcePhuongThucGiaiNgan.IndexOf(lstSourcePhuongThucGiaiNgan.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(BusinessConstant.HINH_THUC_GIAO_DICH.TIEN_MAT.layGiaTri())));
            teldtNgayGiaiNgan.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
            teldtNgayDaoHan.Value = null;
            teldtNgayTinhLai.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
            numLaiSuatTrongHan.Value = 0;
            cmbLSuatTrongHanDViTinh.SelectedIndex = lstSourceLaiSuatTrongHan.IndexOf(lstSourceLaiSuatTrongHan.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(BusinessConstant.KY_HAN_DVI_TINH.NAM.layGiaTri())));
            numLaiSuatQuaHan.Value = 0;
            cmbLSuatQuaHanDViTinh.SelectedIndex = lstSourceLaiSuatQuaHan.IndexOf(lstSourceLaiSuatQuaHan.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(BusinessConstant.KY_HAN_DVI_TINH.NAM.layGiaTri())));
            #endregion

            #region Thong tin ke hoach
            chkLapKeHoachTuDong.IsChecked = true;
            chkLapKeHoachThuCong.IsChecked = false;
            cmbHinhThucTraGoc.SelectedIndex = lstSourceHinhThucTraGoc.IndexOf(lstSourceHinhThucTraGoc.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(BusinessConstant.HINH_THUC_TRA_GOC.DINH_KY.layGiaTri())));
            cmbHinhThucTraLai.SelectedIndex = lstSourceHinhThucTraLai.IndexOf(lstSourceHinhThucTraLai.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(BusinessConstant.HINH_THUC_TRA_LAI.DINH_KY.layGiaTri())));
            numTanSuatTraGoc.Value = 0;
            cmbTanSuatTraGoc.SelectedIndex = lstSourceTanSuatTraGoc.IndexOf(lstSourceTanSuatTraGoc.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(BusinessConstant.TAN_SUAT.THANG.layGiaTri())));
            numTanSuatTraLai.Value = 0;
            cmbTanSuatTraLai.SelectedIndex = lstSourceTanSuatTraLai.IndexOf(lstSourceTanSuatTraLai.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(BusinessConstant.TAN_SUAT.THANG.layGiaTri())));
            txtNgayTraGoc.Text = "";
            txtNgayTraLai.Text = "";
            chkLuuNgayTraGoc.IsChecked = false;
            chkLuuNgayTraLai.IsChecked = false;
            cmbCachTinhSoNgay.SelectedIndex = lstSourceCachTinhSoNgay.IndexOf(lstSourceCachTinhSoNgay.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(BusinessConstant.CACH_TINH_SO_NGAY_TD.NGAY_THUC_TE.layGiaTri())));
            cmbPhuongThucDieuChinhNgayTraNo.SelectedIndex = lstSourcePhuongThucDieuChinhNgayTraNo.IndexOf(lstSourcePhuongThucDieuChinhNgayTraNo.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(BusinessConstant.NGAY_GD_ROI_VAO_NGAY_NGHI.NGAY_SAU.layGiaTri())));
            chkLapKeHoachThuCong.IsChecked = false;
            chkLapKeHoachTuDong.IsChecked = true;
            raddgrKeHoachDS.Rebind();
            #endregion

            #region Thong tin khac
            teldtDuThuDenNgay.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
            numDuThuLuyKe.Value = null;
            teldtPhanBoDenNgay.Value = null;
            numTongLaiTraTruoc.Value = null;
            numSoTienDaPhanBo.Value = null;
            numSoTiencHOPhanBo.Value = null;
            cmbNhomNoHienTai.SelectedIndex = lstSourceNhomNo.IndexOf(lstSourceNhomNo.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(BusinessConstant.NHOM_NO.NHOM1.layGiaTri())));
            #endregion

            #region Thông tin kiểm soát
            txtTrangThai.Text = "";
            teldtNgayNhap.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
            txtNguoiLap.Text = ClientInformation.TenDangNhap;
            teldtNgayCNhat.Value = null;
            txtNguoiCapNhat.Text = "";
            #endregion

            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.THEM, "", mnuMain, DatabaseConstant.Function.TDTD_KHE_UOC);

        }

        private bool Validation()
        {
            bool kq = true;
            try
            {
                kq = ValidateKeHoach();
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                kq = false;
            }
            return kq;
        }

        private bool ValidateKeHoach()
        {
            if (txtNgayTraGoc.Text.Trim().IsNullOrEmptyOrSpace())
            {
                txtNgayTraGoc.Text = teldtNgayGiaiNgan.Value.Value.Day.ToString();
            }
            if (txtNgayTraLai.Text.Trim().IsNullOrEmptyOrSpace())
            {
                txtNgayTraLai.Text = teldtNgayGiaiNgan.Value.Value.Day.ToString();
            }

            if (txtSoHDTD.Text.IsNullOrEmptyOrSpace())
            {
                CommonFunction.ThongBaoTrong(lblSoHDTD.Content.ToString());
                txtSoHDTD.Focus();
                return false;
            }
            else if (numThoiHanVay.Value == null || numThoiHanVay.Value.Value == 0)
            {
                CommonFunction.ThongBaoTrong(lblThoiHanVay.Content.ToString());
                numThoiHanVay.Focus();
                return false;
            }
            else if (teldtNgayDaoHan.Value.Value > LDateTime.PlusDaysComposite(teldtNgayDaoHanHD.Value.Value, tgianRut, tgianRutDvi))
            {
                //CommonFunction.ThongBaoLoi("Ngày đáo hạn của khế ước phải nhỏ hơn hợp đồng");
                LMessage.ShowMessage("M_ResponseMessage_KUOCVM_NgayDaoHanKhongHopLe", LMessage.MessageBoxType.Warning);
                numThoiHanVay.Focus();
                return false;
            }
            else if (numSoTienNhanNo.Value.GetValueOrDefault(0) < 1)
            {
                CommonFunction.ThongBaoTrong(lblSoTienNhanNo.Content.ToString());
                numSoTienNhanNo.Focus();
                return false;
            }
            else if (numSoTienGiaiNgan.Value + numSoTienNhanNo.Value > numSoTienVay.Value)
            {
                //CommonFunction.ThongBaoLoi("Tổng số tiền giải ngân khế ước không được lớn hơn số tiền vay ở hợp đồng");
                LMessage.ShowMessage("M_ResponseMessage_KUOCVM_SoTienGiaiNganKhongHopLe", LMessage.MessageBoxType.Warning);
                numSoTienNhanNo.Focus();
                return false;
            }
            else if (numThoiHanVay.Value.GetValueOrDefault(0) < 1)
            {
                CommonFunction.ThongBaoTrong(lblThoiHanVay.Content.ToString());
                numThoiHanVay.Focus();
                return false;
            }
            else if (cmbThoiHanVay.Text.IsNullOrEmptyOrSpace())
            {
                CommonFunction.ThongBaoTrong(lblThoiHanVay.Content.ToString());
                cmbThoiHanVay.Focus();
                return false;
            }
            else if (cmbPhuongThucGiaiNgan.Text.IsNullOrEmptyOrSpace())
            {
                CommonFunction.ThongBaoTrong(lblPhuongThucGiaiNgan.Content.ToString());
                cmbPhuongThucGiaiNgan.Focus();
                return false;
            }
            else if (teldtNgayGiaiNgan.Value == null)
            {
                CommonFunction.ThongBaoTrong(lblNgayGiaiNgan.Content.ToString());
                teldtNgayGiaiNgan.Focus();
                return false;
            }
            else if (teldtNgayDaoHan.Value == null)
            {
                CommonFunction.ThongBaoTrong(lblNgayDaoHan.Content.ToString());
                teldtNgayDaoHan.Focus();
                return false;
            }
            else if (teldtNgayTinhLai.Value == null)
            {
                CommonFunction.ThongBaoTrong(lblNgayTinhLai.Content.ToString());
                teldtNgayTinhLai.Focus();
                return false;
            }
            else if (numLaiSuatQuaHan.Value == null || numLaiSuatQuaHan.Value == 0)
            {
                CommonFunction.ThongBaoTrong(lblLaiSuatQuaHan.Content.ToString());
                numLaiSuatQuaHan.Focus();
                return false;
            }
            else if (chkLapKeHoachTuDong.IsChecked.Value)
            {
                if (Convert.ToInt32(txtNgayTraGoc.Text) > 31 || Convert.ToInt32(txtNgayTraGoc.Text) < 1)
                {
                    LMessage.ShowMessage("M_ResponesMessage_KheUoc_NgayTraGocKhongHopLe", LMessage.MessageBoxType.Warning);
                    numSoTienNhanNo.Focus();
                    return false;
                }
                else if (Convert.ToInt32(txtNgayTraLai.Text) > 31 || Convert.ToInt32(txtNgayTraLai.Text) < 1)
                {
                    LMessage.ShowMessage("M_ResponesMessage_KheUoc_NgayTraLaiKhongHopLe", LMessage.MessageBoxType.Warning);
                    numSoTienNhanNo.Focus();
                    return false;
                }
                else
                    return true;
            }
            else if (chkLapKeHoachThuCong.IsChecked.Value)
            {
                if (raddgrKeHoachDS.Items == null && raddgrKeHoachDS.Items.Count == 0)
                {
                    LMessage.ShowMessage("M_ResponseMessage_KheUoc_ChuaLapKeHoach", LMessage.MessageBoxType.Warning);
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
                return true;
        }

        private bool ValidateTaoKeHoach()
        {
            if (txtNgayTraGoc.Text.Trim().IsNullOrEmptyOrSpace())
            {
                txtNgayTraGoc.Text = teldtNgayGiaiNgan.Value.Value.Day.ToString();
            }
            if (txtNgayTraLai.Text.Trim().IsNullOrEmptyOrSpace())
            {
                txtNgayTraLai.Text = teldtNgayGiaiNgan.Value.Value.Day.ToString();
            }

            if (txtSoHDTD.Text.IsNullOrEmptyOrSpace())
            {
                CommonFunction.ThongBaoTrong(lblSoHDTD.Content.ToString());
                txtSoHDTD.Focus();
                return false;
            }
            else if (numThoiHanVay.Value == null || numThoiHanVay.Value.Value == 0)
            {
                CommonFunction.ThongBaoTrong(lblThoiHanVay.Content.ToString());
                numThoiHanVay.Focus();
                return false;
            }
            else if (teldtNgayDaoHan.Value.Value > LDateTime.PlusDaysComposite(teldtNgayDaoHanHD.Value.Value, tgianRut, tgianRutDvi))
            {
                //CommonFunction.ThongBaoLoi("Ngày đáo hạn của khế ước phải nhỏ hơn hợp đồng");
                LMessage.ShowMessage("M_ResponseMessage_KUOCVM_NgayDaoHanKhongHopLe", LMessage.MessageBoxType.Warning);
                numThoiHanVay.Focus();
                return false;
            }
            else if (numSoTienNhanNo.Value.GetValueOrDefault(0) < 1)
            {
                CommonFunction.ThongBaoTrong(lblSoTienNhanNo.Content.ToString());
                numSoTienNhanNo.Focus();
                return false;
            }
            else if (numSoTienGiaiNgan.Value + numSoTienNhanNo.Value > numSoTienVay.Value)
            {
                //CommonFunction.ThongBaoLoi("Tổng số tiền giải ngân khế ước không được lớn hơn số tiền vay ở hợp đồng");
                LMessage.ShowMessage("M_ResponseMessage_KUOCVM_SoTienGiaiNganKhongHopLe", LMessage.MessageBoxType.Warning);
                numSoTienNhanNo.Focus();
                return false;
            }
            else if (numThoiHanVay.Value.GetValueOrDefault(0) < 1)
            {
                CommonFunction.ThongBaoTrong(lblThoiHanVay.Content.ToString());
                numThoiHanVay.Focus();
                return false;
            }
            else if (cmbThoiHanVay.Text.IsNullOrEmptyOrSpace())
            {
                CommonFunction.ThongBaoTrong(lblThoiHanVay.Content.ToString());
                cmbThoiHanVay.Focus();
                return false;
            }
            else if (cmbPhuongThucGiaiNgan.Text.IsNullOrEmptyOrSpace())
            {
                CommonFunction.ThongBaoTrong(lblPhuongThucGiaiNgan.Content.ToString());
                cmbPhuongThucGiaiNgan.Focus();
                return false;
            }
            else if (teldtNgayGiaiNgan.Value == null)
            {
                CommonFunction.ThongBaoTrong(lblNgayGiaiNgan.Content.ToString());
                teldtNgayGiaiNgan.Focus();
                return false;
            }
            else if (teldtNgayDaoHan.Value == null)
            {
                CommonFunction.ThongBaoTrong(lblNgayDaoHan.Content.ToString());
                teldtNgayDaoHan.Focus();
                return false;
            }
            else if (teldtNgayTinhLai.Value == null)
            {
                CommonFunction.ThongBaoTrong(lblNgayTinhLai.Content.ToString());
                teldtNgayTinhLai.Focus();
                return false;
            }
            else if (chkLapKeHoachTuDong.IsChecked.Value)
            {
                if (cmbHinhThucTraGoc.Text.IsNullOrEmptyOrSpace())
                {
                    CommonFunction.ThongBaoTrong(lblHinhThucTraGoc.Content.ToString());
                    cmbHinhThucTraGoc.Focus();
                    return false;
                }
                else if (numTanSuatTraGoc.Value.GetValueOrDefault(0) < 1)
                {
                    CommonFunction.ThongBaoTrong(lblTanSuatTraGoc.Content.ToString());
                    numTanSuatTraGoc.Focus();
                    return false;
                }
                else if (cmbTanSuatTraGoc.Text.IsNullOrEmptyOrSpace())
                {
                    CommonFunction.ThongBaoTrong(lblTanSuatTraGoc.Content.ToString());
                    cmbTanSuatTraGoc.Focus();
                    return false;
                }
                if (cmbHinhThucTraLai.Text.IsNullOrEmptyOrSpace())
                {
                    CommonFunction.ThongBaoTrong(lblHinhThucTraLai.Content.ToString());
                    cmbHinhThucTraLai.Focus();
                    return false;
                }
                else if (numTanSuatTraLai.Value.GetValueOrDefault(0) < 1)
                {
                    CommonFunction.ThongBaoTrong(lblTanSuatTraLai.Content.ToString());
                    numTanSuatTraLai.Focus();
                    return false;
                }
                else if (cmbTanSuatTraLai.Text.IsNullOrEmptyOrSpace())
                {
                    CommonFunction.ThongBaoTrong(lblTanSuatTraLai.Content.ToString());
                    cmbTanSuatTraLai.Focus();
                    return false;
                }
                else if (Convert.ToInt32(txtNgayTraGoc.Text) > 31 || Convert.ToInt32(txtNgayTraGoc.Text) < 1)
                {
                    LMessage.ShowMessage("M_ResponesMessage_KheUoc_NgayTraGocKhongHopLe", LMessage.MessageBoxType.Warning);
                    numSoTienNhanNo.Focus();
                    return false;
                }
                else if (Convert.ToInt32(txtNgayTraLai.Text) > 31 || Convert.ToInt32(txtNgayTraLai.Text) < 1)
                {
                    LMessage.ShowMessage("M_ResponesMessage_KheUoc_NgayTraLaiKhongHopLe", LMessage.MessageBoxType.Warning);
                    numSoTienNhanNo.Focus();
                    return false;
                }
                else
                    return true;
            }
            else
                return true;
        }

        private void SetEnabledControls(bool isEnable)
        {
            grbThongTinKheUoc.IsEnabled = isEnable;
            grbDieuKienLapKeHoach.IsEnabled = isEnable;
            btnMaHDTD.IsEnabled = isEnable;
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

                GetFormData(trangThai);

                if (action == DatabaseConstant.Action.THEM)
                {
                    OnAddNew(_obj);
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

        public void OnSave()
        {
            try
            {
                if (!Validation()) return;

                if (raddgrKeHoachDS.Items == null || raddgrKeHoachDS.Items.Count == 0)
                {
                    if (LMessage.ShowMessage("M_ResponseMessage_KheUoc_HoiChuaLapKeHoachKheUoc", LMessage.MessageBoxType.Question) == MessageBoxResult.Yes)
                    {
                        string tSuatTraGoc = lstSourceHinhThucTraGoc[cmbHinhThucTraGoc.SelectedIndex].KeywordStrings[0];
                        string tSuatTraLai = lstSourceHinhThucTraLai[cmbHinhThucTraLai.SelectedIndex].KeywordStrings[0];

                        if (tSuatTraGoc == BusinessConstant.HINH_THUC_TRA_GOC.DINH_KY.layGiaTri())
                        {
                            if (numTanSuatTraGoc.Value == null || numTanSuatTraGoc.Value.Value == 0)
                            {
                                cmbHinhThucTraGoc.SelectedIndex = lstSourceHinhThucTraGoc.IndexOf(lstSourceHinhThucTraGoc.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(BusinessConstant.HINH_THUC_TRA_GOC.TRA_SAU.layGiaTri())));
                            }
                        }

                        if (tSuatTraLai == BusinessConstant.HINH_THUC_TRA_LAI.DINH_KY.layGiaTri())
                        {
                            if (numTanSuatTraLai.Value == null || numTanSuatTraLai.Value.Value == 0)
                            {
                                cmbHinhThucTraLai.SelectedIndex = lstSourceHinhThucTraLai.IndexOf(lstSourceHinhThucTraLai.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(BusinessConstant.HINH_THUC_TRA_LAI.CUOI_KY.layGiaTri())));
                            }
                        }
                    }

                    TaoKeHoachTuDong();
                }

                string trangThai = "";
                if (sTrangThaiNVu.Equals(BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri()))
                    trangThai = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();
                else
                    trangThai = CommonFunction.LayTrangThaiBanGhi(DatabaseConstant.Action.LUU, BusinessConstant.layTrangThaiNghiepVu(sTrangThaiNVu));

                GetFormData(trangThai);

                if (action == DatabaseConstant.Action.THEM)
                {
                    OnAddNew(_obj);
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

        public void BeforeViewFromDetail()
        {
            action = DatabaseConstant.Action.XEM;
            SetEnabledControls(false);
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
            SetEnabledControls(true);
            CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain, function);
        }

        public void OnAddNew(TDTD_KHE_UOC obj)
        {

            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                TinDungTDProcess processTinDungTD = new TinDungTDProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                int ret = 0;

                ret = processTinDungTD.KheUocTieuDung(DatabaseConstant.Action.THEM, ref _obj, ref listClientResponseDetail);
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

        public void AfterAddNew(int ret, TDTD_KHE_UOC obj, List<ClientResponseDetail> listClientResponseDetail)
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
                        sTrangThaiNVu = obj.TTHAI_NVU;
                        lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);
                        txtSoKheUoc.Text = obj.objKuoc.SO_KHE_UOC;
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
                listLockId.Add(_obj.objKuoc.ID);

                bool ret = process.LockData(DatabaseConstant.Module.TDTD,
                    DatabaseConstant.Function.TDTD_KHE_UOC,
                    DatabaseConstant.Table.TDTD_KUOC,
                    DatabaseConstant.Action.SUA,
                    listLockId);

                if (ret)
                {
                    action = DatabaseConstant.Action.SUA;
                    SetEnabledControls(true);
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
            SetEnabledControls(true);
            CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain, function);
        }

        public void OnModify(TDTD_KHE_UOC obj)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                TinDungTDProcess processTinDungTD = new TinDungTDProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                int ret = 0;

                ret = processTinDungTD.KheUocTieuDung(DatabaseConstant.Action.SUA, ref obj, ref listClientResponseDetail);
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

        public void AfterModify(int ret, TDTD_KHE_UOC obj, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (ret > 0)
                {
                    LMessage.ShowMessage("M.DungChung.CapNhatThanhCong", LMessage.MessageBoxType.Information);

                    sTrangThaiNVu = obj.TTHAI_NVU;
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);

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
                listLockId.Add(_obj.objKuoc.ID);

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.TDTD,
                    DatabaseConstant.Function.TDTD_KHE_UOC,
                    DatabaseConstant.Table.TDTD_KUOC,
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
                    listLockId.Add(_obj.objKuoc.ID);
                    List<int> listLockedId = new List<int>();

                    bool retLockData = process.LockData(DatabaseConstant.Module.TDTD,
                        DatabaseConstant.Function.TDTD_KHE_UOC,
                        DatabaseConstant.Table.TDTD_KUOC,
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
                listLockId.Add(_obj.objKuoc.ID);
                List<int> listUnlockId = new List<int>();

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.TDTD,
                    DatabaseConstant.Function.TDTD_KHE_UOC,
                    DatabaseConstant.Table.TDTD_KUOC,
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
            TinDungTDProcess processTinDungTD = new TinDungTDProcess();
            try
            {
                int ret = 0;
                ret = processTinDungTD.KheUocTieuDung(action, ref _obj, ref listClientResponseDetail);
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
                processTinDungTD = null;
            }
        }

        public void AfterDelete(int ret, List<ClientResponseDetail> listClientResponseDetail)
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
                listLockId.Add(_obj.objKuoc.ID);
                List<int> listUnlockId = new List<int>();

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.TDTD,
                    DatabaseConstant.Function.TDTD_KHE_UOC,
                    DatabaseConstant.Table.TDTD_KUOC,
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
                    listLockId.Add(_obj.objKuoc.ID);
                    List<int> listLockedId = new List<int>();

                    bool retLockData = process.LockData(DatabaseConstant.Module.TDTD,
                        DatabaseConstant.Function.TDTD_KHE_UOC,
                        DatabaseConstant.Table.TDTD_KUOC,
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
            TinDungTDProcess processTinDungTD = new TinDungTDProcess();
            try
            {
                int ret = 0;
                ret = processTinDungTD.KheUocTieuDung(action, ref _obj, ref listClientResponseDetail);
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
                processTinDungTD = null;
            }
        }

        public void AfterApprove(int ret, List<ClientResponseDetail> listClientResponseDetail)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
            try
            {
                if (ret > 0)
                {
                    CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                    //LMessage.ShowMessage("M.DungChung.DuyetThanhCong", LMessage.MessageBoxType.Information);

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
                listLockId.Add(_obj.objKuoc.ID);
                List<int> listUnlockId = new List<int>();

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.TDTD,
                    DatabaseConstant.Function.TDTD_KHE_UOC,
                    DatabaseConstant.Table.TDTD_KUOC,
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
                    listLockId.Add(_obj.objKuoc.ID);
                    List<int> listLockedId = new List<int>();

                    bool retLockData = process.LockData(DatabaseConstant.Module.TDTD,
                        DatabaseConstant.Function.TDTD_KHE_UOC,
                        DatabaseConstant.Table.TDTD_KUOC,
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
            TinDungTDProcess processTinDungTD = new TinDungTDProcess();
            try
            {
                int ret = 0;
                ret = processTinDungTD.KheUocTieuDung(action, ref _obj, ref listClientResponseDetail);
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
                processTinDungTD = null;
            }
        }

        public void AfterCancel(int ret, List<ClientResponseDetail> listClientResponseDetail)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
            try
            {
                if (ret > 0)
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
                listLockId.Add(_obj.objKuoc.ID);
                List<int> listUnlockId = new List<int>();

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.TDTD,
                    DatabaseConstant.Function.TDTD_KHE_UOC,
                    DatabaseConstant.Table.TDTD_KUOC,
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
                    listLockId.Add(_obj.objKuoc.ID);
                    List<int> listLockedId = new List<int>();

                    bool retLockData = process.LockData(DatabaseConstant.Module.TDTD,
                        DatabaseConstant.Function.TDTD_KHE_UOC,
                        DatabaseConstant.Table.TDTD_KUOC,
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
            TinDungTDProcess processTinDungTD = new TinDungTDProcess();
            try
            {
                int ret = 0;
                ret = processTinDungTD.KheUocTieuDung(action, ref _obj, ref listClientResponseDetail);
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
                processTinDungTD = null;
            }
        }

        public void AfterRefuse(int ret, List<ClientResponseDetail> listClientResponseDetail)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
            try
            {
                if (ret > 0)
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
                listLockId.Add(_obj.objKuoc.ID);
                List<int> listUnlockId = new List<int>();

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.TDTD,
                    DatabaseConstant.Function.TDTD_KHE_UOC,
                    DatabaseConstant.Table.TDTD_KUOC,
                    DatabaseConstant.Action.TU_CHOI_DUYET,
                    listLockId);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        private void OnPreview()
        {
            // Cảnh báo khi không có dữ liệu
            if (txtSoKheUoc.Text.IsNullOrEmptyOrSpace())
            {
                LMessage.ShowMessage("M.DungChung.KhongCoThongTin", LMessage.MessageBoxType.Warning);
                return;
            }
            else
            {
                VanHanhGiaoDich xemBaoCao = new VanHanhGiaoDich();
                List<ThamSoBaoCao> lstThamSo = new List<ThamSoBaoCao>();

                string ngaybaocao = ClientInformation.NgayLamViecHienTai;

                lstThamSo.Add(new ThamSoBaoCao("@MaChiNhanh", ClientInformation.MaDonVi, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                lstThamSo.Add(new ThamSoBaoCao("@MaPhongGD", ClientInformation.MaDonViGiaoDich, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                lstThamSo.Add(new ThamSoBaoCao("@NgayBaoCao", ngaybaocao, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                lstThamSo.Add(new ThamSoBaoCao("@MaKuoc", _obj.objKuoc.MA_KHE_UOC, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                lstThamSo.Add(new ThamSoBaoCao("MaNgonNgu", ClientInformation.NgonNgu, ApplicationConstant.LoaiThamSoBaoCao.LANG.layGiaTri()));
                lstThamSo.Add(new ThamSoBaoCao("MaDinhDang", ApplicationConstant.LoaiDinhDangBaoCao.WORD.layGiaTri(), ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri()));


                string maBaoCao = DatabaseConstant.LayMaBaoCaoBIDV(DatabaseConstant.DanhSachBaoCaoBIDV.TDTT_KHE_UOC_RUT_VON);
                xemBaoCao.LayDuLieu(maBaoCao, lstThamSo);
            }
        }

        private void OnPreviewBaoLanhBenThuBa()
        {
            // Cảnh báo khi không có dữ liệu
            if (txtSoKheUoc.Text.IsNullOrEmptyOrSpace())
            {
                LMessage.ShowMessage("M.DungChung.KhongCoThongTin", LMessage.MessageBoxType.Warning);
                return;
            }
            else
            {
                VanHanhGiaoDich xemBaoCao = new VanHanhGiaoDich();
                List<ThamSoBaoCao> lstThamSo = new List<ThamSoBaoCao>();

                string idkhachhang = "";
                string makhachhang = "";
                string tenkhachhang = "";
                string gioitinh = "";
                string ngaysinh = "";
                string diachi = "";
                string socmnd = "";
                string ngaycap = "";
                string noicap = "";

                string sotienvay = "";
                string mahopdong = "";
                string ngaybaocao = "";
                string masanpham = "";
                ngaybaocao = ClientInformation.NgayLamViecHienTai;

                lstThamSo.Add(new ThamSoBaoCao("@MaKhachHang", makhachhang, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                lstThamSo.Add(new ThamSoBaoCao("@IdKhachHang", idkhachhang, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                lstThamSo.Add(new ThamSoBaoCao("@SoTienVay", sotienvay, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                lstThamSo.Add(new ThamSoBaoCao("@MaKUOCTD", mahopdong, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                lstThamSo.Add(new ThamSoBaoCao("@NgayBaoCao", ngaybaocao, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                lstThamSo.Add(new ThamSoBaoCao("@MaSanPhamTD", masanpham, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                lstThamSo.Add(new ThamSoBaoCao("MaNgonNgu", ClientInformation.NgonNgu, ApplicationConstant.LoaiThamSoBaoCao.LANG.layGiaTri()));
                lstThamSo.Add(new ThamSoBaoCao("MaDinhDang", ApplicationConstant.LoaiDinhDangBaoCao.WORD.layGiaTri(), ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri()));
                lstThamSo.Add(new ThamSoBaoCao("@MA_DON_VI", ClientInformation.MaDonViGiaoDich, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));

                lstThamSo.Add(new ThamSoBaoCao("P_TEN_KHACH_HANG", tenkhachhang, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                lstThamSo.Add(new ThamSoBaoCao("P_TEN_THUONG_GOI", tenkhachhang, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                lstThamSo.Add(new ThamSoBaoCao("P_GIOI_TINH", gioitinh, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                lstThamSo.Add(new ThamSoBaoCao("P_NGAY_SINH", ngaysinh, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                lstThamSo.Add(new ThamSoBaoCao("P_DIA_CHI", diachi, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                lstThamSo.Add(new ThamSoBaoCao("P_SO_CMND", socmnd, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                lstThamSo.Add(new ThamSoBaoCao("P_NGAY_CAP", ngaycap, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                lstThamSo.Add(new ThamSoBaoCao("P_NOI_CAP", noicap, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));

                string maBaoCao = DatabaseConstant.LayMaBaoCaoBIDV(DatabaseConstant.DanhSachBaoCaoBIDV.TDVM_BANG_KE_RUT_VON);
                xemBaoCao.LayDuLieu(maBaoCao, lstThamSo);
            }
        }

        private void OnPreviewPhanKyTraNo()
        {
            // Cảnh báo khi không có dữ liệu
            if (txtSoKheUoc.Text.IsNullOrEmptyOrSpace())
            {
                LMessage.ShowMessage("M.DungChung.KhongCoThongTin", LMessage.MessageBoxType.Warning);
                return;
            }
            else
            {
                VanHanhGiaoDich xemBaoCao = new VanHanhGiaoDich();
                List<ThamSoBaoCao> lstThamSo = new List<ThamSoBaoCao>();

                string ngaybaocao = ClientInformation.NgayLamViecHienTai;

                lstThamSo.Add(new ThamSoBaoCao("@MaChiNhanh", ClientInformation.MaDonVi, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                lstThamSo.Add(new ThamSoBaoCao("@MaPhongGD", ClientInformation.MaDonViGiaoDich, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                lstThamSo.Add(new ThamSoBaoCao("@NgayBaoCao", ngaybaocao, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                lstThamSo.Add(new ThamSoBaoCao("@ID", obj.objKuoc.ID.ToString(), ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                lstThamSo.Add(new ThamSoBaoCao("@MaKuoc", txtSoKheUoc.Text, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                lstThamSo.Add(new ThamSoBaoCao("MaNgonNgu", ClientInformation.NgonNgu, ApplicationConstant.LoaiThamSoBaoCao.LANG.layGiaTri()));
                lstThamSo.Add(new ThamSoBaoCao("MaDinhDang", ApplicationConstant.LoaiDinhDangBaoCao.EXCEL.layGiaTri(), ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri()));


                string maBaoCao = DatabaseConstant.LayMaBaoCaoBIDV(DatabaseConstant.DanhSachBaoCaoBIDV.TDTD_BANG_KHOACH_TRA_NO);
                xemBaoCao.LayDuLieu(maBaoCao, lstThamSo);
            }
        }

        private void ProcessDataThucThu()
        {
            DataSet ds = new DataSet();
            DataTable dtPar = null;
            LDatatable.MakeParameterTable(ref dtPar);
            LDatatable.AddParameter(ref dtPar, "@ID_KUOC", "String", _obj.objKuoc.ID.ToString());
            LDatatable.AddParameter(ref dtPar, "@MA_KUOC", "String", _obj.objKuoc.MA_KHE_UOC);
            ds = new TinDungTDProcess().getThucThuKUOC(dtPar);
            
            List<THONG_TIN_THU> lstThongTinThu = new List<THONG_TIN_THU>();
            foreach (KE_HOACH_CHI_TIET objKeHoachCT in lstHienThiKeHoach)
            {
                THONG_TIN_THU objTTinThu = new THONG_TIN_THU();
                objTTinThu.TU_NGAY = objKeHoachCT.TU_NGAY;
                objTTinThu.DEN_NGAY = objKeHoachCT.DEN_NGAY;
                objTTinThu.GOC_KH = objKeHoachCT.SO_TIEN_GOC;
                objTTinThu.LAI_KH = objKeHoachCT.SO_TIEN_LAI;
                List<THONG_TIN_THU.CHI_TIET_THUC_THU> lstChiTietThucThu =
                    ds.Tables["THUC_THU"].AsEnumerable().Where(f => f.Field<string>("DEN_NGAY") == objKeHoachCT.DEN_NGAY).
                    Select(f => new THONG_TIN_THU.CHI_TIET_THUC_THU
                    {
                        MA_GDICH = f.Field<string>("MA_GDICH"),
                        NGAY_TT = f.Field<string>("NGAY_THU"),
                        GOC_TT = f.Field<decimal>("TRA_GOC_TT"),
                        LAI_TT = f.Field<decimal>("TRA_LAI_TT"),
                        TONG_TIEN = f.Field<decimal>("TRA_GOC_TT") + f.Field<decimal>("TRA_LAI_TT"),
                    }).ToList();
                if(!lstChiTietThucThu.IsNullOrEmpty())
                {
                    objTTinThu.GOC_TT = lstChiTietThucThu.Sum(f => f.GOC_TT);
                    objTTinThu.LAI_TT = lstChiTietThucThu.Sum(f => f.LAI_TT);
                }
                objTTinThu.DSACH_THU = lstChiTietThucThu;

                //decimal tongKeHoach = objTTinThu.GOC_KH + objTTinThu.LAI_KH;
                //decimal tongThucTe = objTTinThu.GOC_TT + objTTinThu.LAI_TT;
                decimal tongKeHoach = objTTinThu.GOC_KH + objTTinThu.LAI_KH;
                decimal tongThucTe = 0;
                string ngayHienTaiDuLieu = ClientInformation.NgayLamViecHienTai;
                string ngayTraKeHoach = objTTinThu.DEN_NGAY;                

                string ngayThuGiaoDichDauTien = "";
                decimal tongNgayThuGiaoDichDauTien = 0;
                if (lstChiTietThucThu.IsNullOrEmpty())
                {
                    ngayThuGiaoDichDauTien = "";
                    tongNgayThuGiaoDichDauTien = 0;
                }
                else if (lstChiTietThucThu.Count == 0)
                {
                    ngayThuGiaoDichDauTien = "";
                    tongNgayThuGiaoDichDauTien = 0;
                }
                else
                {
                    ngayThuGiaoDichDauTien = "";
                    ngayThuGiaoDichDauTien = lstChiTietThucThu.Min(e => e.NGAY_TT);
                    tongNgayThuGiaoDichDauTien = lstChiTietThucThu.Where(i => i.NGAY_TT == ngayThuGiaoDichDauTien).Select(i => i.TONG_TIEN).Sum();

                    tongThucTe = lstChiTietThucThu.Where(i => i.NGAY_TT == ngayThuGiaoDichDauTien).Select(i => i.GOC_TT).Sum() +
                                 lstChiTietThucThu.Where(i => i.NGAY_TT == ngayThuGiaoDichDauTien).Select(i => i.LAI_TT).Sum();

                    ngayHienTaiDuLieu = ngayThuGiaoDichDauTien;
                }

                // Neu khong co thong tin tra
                if (tongThucTe <= 0)
                {
                    if (ngayTraKeHoach.CompareTo(ngayHienTaiDuLieu) <= 0)
                    {
                        objTTinThu.TT_THU = -1;
                    }
                    else
                    {
                        objTTinThu.TT_THU = 0;
                    }
                }
                // Neu co thong tin tra
                else if (tongThucTe > 0)
                {
                    if (tongKeHoach == tongThucTe)
                    {
                        if (ngayTraKeHoach.CompareTo(ngayHienTaiDuLieu) > 0)
                        {
                            objTTinThu.TT_THU = 1;
                        }
                        else if (ngayTraKeHoach.CompareTo(ngayHienTaiDuLieu) == 0)
                        {
                            objTTinThu.TT_THU = 0;
                        }
                        else
                        {
                            if (tongNgayThuGiaoDichDauTien > tongKeHoach)
                                objTTinThu.TT_THU = -1;
                            else if (tongNgayThuGiaoDichDauTien == tongKeHoach)
                                objTTinThu.TT_THU = -1;
                            else if (tongNgayThuGiaoDichDauTien < tongKeHoach)
                                objTTinThu.TT_THU = -1;
                        }
                    }
                    else if (tongKeHoach < tongThucTe)
                    {
                        if (ngayTraKeHoach.CompareTo(ngayHienTaiDuLieu) > 0)
                        {
                            objTTinThu.TT_THU = 1;
                        }
                        else if (ngayTraKeHoach.CompareTo(ngayHienTaiDuLieu) == 0)
                        {
                            objTTinThu.TT_THU = 1;
                        }
                        else
                        {
                            if (tongNgayThuGiaoDichDauTien > tongKeHoach)
                                objTTinThu.TT_THU = 1;
                            else if (tongNgayThuGiaoDichDauTien == tongKeHoach)
                                objTTinThu.TT_THU = 1;
                            else if (tongNgayThuGiaoDichDauTien < tongKeHoach)
                                objTTinThu.TT_THU = -1;
                        }
                    }
                    else if (tongKeHoach > tongThucTe)
                    {
                        if (ngayTraKeHoach.CompareTo(ngayHienTaiDuLieu) > 0)
                        {
                            objTTinThu.TT_THU = 1;
                        }
                        else if (ngayTraKeHoach.CompareTo(ngayHienTaiDuLieu) == 0)
                        {
                            objTTinThu.TT_THU = -1;
                        }
                        else
                        {
                            if (tongNgayThuGiaoDichDauTien > tongKeHoach)
                                objTTinThu.TT_THU = 1;
                            else if (tongNgayThuGiaoDichDauTien == tongKeHoach)
                                objTTinThu.TT_THU = 1;
                            else if (tongNgayThuGiaoDichDauTien < tongKeHoach)
                                objTTinThu.TT_THU = -1;
                        }
                    }
                }

                //
                //if (tongKeHoach == tongThucTe)
                //{
                //    objTTinThu.TT_THU = 0;
                //}
                //else if ((tongKeHoach < tongThucTe)
                //    ||((ngayTraKeHoach.CompareTo(ngayHienTaiDuLieu) > 0) && (tongThucTe > 0)))
                //{
                //    objTTinThu.TT_THU = 1;
                //}
                //else  if (((tongThucTe > 0) && (tongKeHoach > tongThucTe))
                //    || (ngayTraKeHoach.CompareTo(ngayHienTaiDuLieu) < 0)) 
                //{
                //    objTTinThu.TT_THU = -1;
                //} 
                //               

                lstThongTinThu.Add(objTTinThu);
            }            

            raddgrNoTrongHan.ItemsSource = lstThongTinThu;
            raddgrNoTrongHan.Rebind();
        }
        #endregion

        #region Class process
        public class THONG_TIN_THU
        {
            string _tU_NGAY;
            string _dEN_NGAY;
            decimal _gOC_KH;
            decimal _lAI_KH;
            decimal _gOC_TT;
            decimal _lAI_TT;
            int _tT_THU; // tinh trang thu: dung han (0), cham tra (-1), tra truoc (1)
            List<CHI_TIET_THUC_THU> _dSACH_THU;

            public string TU_NGAY
            {
                get
                {
                    return _tU_NGAY;
                }

                set
                {
                    _tU_NGAY = value;
                }
            }

            public string DEN_NGAY
            {
                get
                {
                    return _dEN_NGAY;
                }

                set
                {
                    _dEN_NGAY = value;
                }
            }
            public decimal GOC_KH
            {
                get
                {
                    return _gOC_KH;
                }

                set
                {
                    _gOC_KH = value;
                }
            }

            public decimal LAI_KH
            {
                get
                {
                    return _lAI_KH;
                }

                set
                {
                    _lAI_KH = value;
                }
            }

            public decimal GOC_TT
            {
                get
                {
                    return _gOC_TT;
                }

                set
                {
                    _gOC_TT = value;
                }
            }

            public decimal LAI_TT
            {
                get
                {
                    return _lAI_TT;
                }

                set
                {
                    _lAI_TT = value;
                }
            }

            public int TT_THU
            {
                get
                {
                    return _tT_THU;
                }

                set
                {
                    _tT_THU = value;
                }
            }

            public List<CHI_TIET_THUC_THU> DSACH_THU
            {
                get
                {
                    return _dSACH_THU;
                }

                set
                {
                    _dSACH_THU = value;
                }
            }

            public class CHI_TIET_THUC_THU
            {
                string _mA_GDICH;
                string _nGAY_TT;
                decimal _gOC_TT;
                decimal _lAI_TT;
                decimal _tONG_TIEN;

                public string MA_GDICH
                {
                    get
                    {
                        return _mA_GDICH;
                    }

                    set
                    {
                        _mA_GDICH = value;
                    }
                }

                public string NGAY_TT
                {
                    get
                    {
                        return _nGAY_TT;
                    }

                    set
                    {
                        _nGAY_TT = value;
                    }
                }

                public decimal GOC_TT
                {
                    get
                    {
                        return _gOC_TT;
                    }

                    set
                    {
                        _gOC_TT = value;
                    }
                }

                public decimal LAI_TT
                {
                    get
                    {
                        return _lAI_TT;
                    }

                    set
                    {
                        _lAI_TT = value;
                    }
                }

                public decimal TONG_TIEN
                {
                    get
                    {
                        return _tONG_TIEN;
                    }

                    set
                    {
                        _tONG_TIEN = value;
                    }
                }
            }
        }
        #endregion
    }
}
