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
using Presentation.Process;
using Utilities.Common;
using Microsoft.Windows.Controls.Ribbon;
using Presentation.Process.Common;
using PresentationWPF.CustomControl;
using Presentation.Process.TinDungServiceRef;
using System.Data;
using Presentation.Process.PopupServiceRef;
using PresentationWPF.TinDung.ThuocTinh;
using Telerik.Windows.Controls;
using PresentationWPF.TinDung.KUOC;
using System.Collections;
using System.Reflection;
using PresentationWPF.TinDung.HDTD;

namespace PresentationWPF.TinDung.KiemSoatRuiRo
{
    /// <summary>
    /// Interaction logic for ucKiemSoatRuiRo.xaml
    /// </summary>
    public partial class ucKiemSoatRuiRoCT : UserControl
    {
        #region Khai bao
        private DatabaseConstant.Action action = DatabaseConstant.Action.THEM;
        public DatabaseConstant.Action Action
        {
            get { return action; }
            set { action = value; }
        }

        private string sTrangThaiNVu = "";

        private DatabaseConstant.Function function = DatabaseConstant.Function.TDVM_KIEM_SOAT_RR;

        public event EventHandler OnSavingCompleted;

        public static RoutedCommand SaveCommand = new RoutedCommand();
        public static RoutedCommand ModifyCommand = new RoutedCommand();
        public static RoutedCommand SubmitCommand = new RoutedCommand();
        public static RoutedCommand DeleteCommand = new RoutedCommand();
        public static RoutedCommand ApproveCommand = new RoutedCommand();
        public static RoutedCommand RefuseCommand = new RoutedCommand();
        public static RoutedCommand CancelCommand = new RoutedCommand();
        public static RoutedCommand HelpCommand = new RoutedCommand();
        public static RoutedCommand CloseCommand = new RoutedCommand();

        private TD_KIEM_SOAT_RR _obj = new TD_KIEM_SOAT_RR();
        public TD_KIEM_SOAT_RR obj
        {
            get { return _obj; }
            set { _obj = value; }
        }

        private TD_KIEM_SOAT_RR_EXT _objExt = new TD_KIEM_SOAT_RR_EXT();
        public TD_KIEM_SOAT_RR_EXT objExt
        {
            get { return _objExt; }
            set { _objExt = value; }
        }

        private List<TD_KIEM_SOAT_RR> lst = new List<TD_KIEM_SOAT_RR>();

        List<DataRow> lstPopup = new List<DataRow>();
        public void LayDuLieuTuPopup(List<DataRow> lst)
        {
            lstPopup = lst;
        }
        string maKiemSoatRR = "";
        #endregion

        #region Khoi tao
        public ucKiemSoatRuiRoCT()
        {
            InitializeComponent();
            BindShortkey();
            ShowControl();
            ResetForm();
        }

        private void ShowControl()
        {
            HeThong hethong = new HeThong();
            ArrayList arr = new ArrayList();
            arr = hethong.SetVisibleControl("PresentationWPF.TinDung.KiemSoatRuiRo.ucKiemSoatRuiRoCT", "grbDanhSachKheUoc");
            foreach (List<string> lst in arr)
            {
                object item = grbDanhSachKheUoc.FindName(lst.First());
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

        public ucKiemSoatRuiRoCT(List<TD_KIEM_SOAT_RR> _lst, string soGiaoDichKU, string soGiaoDichHD) : this()
        {
            lst = _lst;
            txtSoKiemSoat.Text = lst[0].MA_KIEM_SOAT;
            txtSoLo.Text = soGiaoDichKU;
            txtSoKheUoc.Text = lst[0].SO_KUOC;
            txtSoHDTD.Text = lst[0].SO_HDTD;
            txtSoKheUoc.Tag = soGiaoDichKU;
            txtSoHDTD.Tag = soGiaoDichHD;
            txtDienGiai.Text = lst[0].DIEN_GIAI;
            sTrangThaiNVu = lst[0].TTHAI_NVU;
            lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);
            txtNguoiLap.Text = lst[0].NGUOI_NHAP;
            txtNguoiCapNhat.Text = lst[0].NGUOI_CNHAT;
            txtTrangThai.Text = BusinessConstant.layNgonNguNghiepVu(lst[0].TTHAI_BGHI);
            if (lst[0].NGAY_NHAP.IsDate(ApplicationConstant.defaultDateTimeFormat))
                teldtNgayNhap.Value = LDateTime.StringToDate(lst[0].NGAY_NHAP, ApplicationConstant.defaultDateTimeFormat);
            if (lst[0].NGAY_CNHAT.IsDate(ApplicationConstant.defaultDateTimeFormat))
                teldtNgayCNhat.Value = LDateTime.StringToDate(lst[0].NGAY_CNHAT, ApplicationConstant.defaultDateTimeFormat);
            SetFormData();
        }
        #endregion

        #region Dang ky hotkey, shortcut key
        private void BindShortkey()
        {
            foreach (var child in Toolbar.Children)
            {
                if (child.GetType() == typeof(RibbonButton))
                {
                    RibbonButton tlb = (RibbonButton)child;
                    KeyBinding key = new KeyBinding();
                    string strTinhNang = tlb.Name.Substring(3, tlb.Name.Length - 3);
                    if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRINH_DUYET)))
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

        private void SubmitCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void SubmitCommand_Executed(object sender, ExecutedRoutedEventArgs e)
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
            RibbonButton tlb = (RibbonButton)sender;
            string strTinhNang = tlb.Name.Substring(3, tlb.Name.Length - 3);
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THEM)))
            {
                ResetForm();
                SetEnabledControls(true);
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
                ResetForm();
                SetEnabledControls(true);
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
            //if (_obj != null && _obj.objKuoc != null)
            //{
            //    listLockId.Add(_obj.objKuoc.ID);

            //    bool ret = process.UnlockData(DatabaseConstant.Module.TDVM,
            //        DatabaseConstant.Function.TDVM_KHE_UOC,
            //        DatabaseConstant.Table.TDVM_KUOC,
            //        DatabaseConstant.Action.SUA,
            //        listLockId);
            //}
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

        private void LoadDanhSachKheUocByMaGDich()
        {
            DataSet ds = new TinDungProcess().getThongTinChiTietKUOCVMDSByGDich(txtSoLo.Text);
            DataTable dt = ds.Tables["KUOC"];
            List<TDVM_KHE_UOC> lstDSachKUOCVM = new List<TDVM_KHE_UOC>();
            foreach (DataRow dr in dt.Rows)
            {
                TDVM_KHE_UOC dtoTDKUOCVM = new TDVM_KHE_UOC();
                
                dtoTDKUOCVM.ID_KHANG = Convert.ToInt32(dr["ID_KHANG"]);
                dtoTDKUOCVM.MA_KHANG = dr["MA_KHANG"].ToString();
                dtoTDKUOCVM.NGAY_BD_TRA = dr["KHOACH_NGAY_LAP"].ToString();
                dtoTDKUOCVM.SAN_PHAM = new TD_SAN_PHAM();
                dtoTDKUOCVM.SAN_PHAM.MA_SAN_PHAM = dr["MA_SAN_PHAM"].ToString();
                dtoTDKUOCVM.SAN_PHAM.TEN_SAN_PHAM = dr["TEN_SAN_PHAM"].ToString();
                dtoTDKUOCVM.SO_TIEN_XIN_VAY = Convert.ToDecimal(dr["SO_TIEN_XIN_VAY"]);
                dtoTDKUOCVM.TEN_KHANG = dr["TEN_KHANG"].ToString();
                dtoTDKUOCVM.DD_GTLQ_SO = dr["DD_GTLQ_SO"].ToString();
                dtoTDKUOCVM.KUOC_VM = new TD_KUOCVM();
                dtoTDKUOCVM.KUOC_VM.ID = Convert.ToInt32(dr["ID"]);
                dtoTDKUOCVM.KUOC_VM.MA_SAN_PHAM = dr["MA_SAN_PHAM"].ToString();
                dtoTDKUOCVM.KUOC_VM.ID_HDTDVM = Convert.ToInt32(dr["ID_HDTDVM"]);
                dtoTDKUOCVM.KUOC_VM.ID_KHANG = Convert.ToInt32(dr["ID_KHANG"]);
                dtoTDKUOCVM.KUOC_VM.MA_KHANG = dr["MA_KHANG"].ToString();
                dtoTDKUOCVM.KUOC_VM.MA_HDTDVM = dr["MA_HDTDVM"].ToString();
                dtoTDKUOCVM.KUOC_VM.MA_KUOCVM = dr["MA_KUOCVM"].ToString();
                dtoTDKUOCVM.KUOC_VM.SO_KUOCVM = dr["SO_KUOCVM"].ToString();
                dtoTDKUOCVM.SO_HDTDVM = dr["SO_HDTDVM"].ToString();
                dtoTDKUOCVM.KUOC_VM.MA_LSUAT = dr["MA_LSUAT"].ToString();
                dtoTDKUOCVM.KUOC_VM.HE_SO = Convert.ToInt32(dr["HE_SO"]);
                dtoTDKUOCVM.KUOC_VM.TGIAN_VAY = Convert.ToInt32(dr["TGIAN_VAY"]);
                dtoTDKUOCVM.KUOC_VM.TGIAN_VAY_DVI_TINH = dr["TGIAN_VAY_DVI_TINH"].ToString();
                dtoTDKUOCVM.KUOC_VM.SO_TIEN_GIAI_NGAN = Convert.ToDecimal(dr["SO_TIEN_GIAI_NGAN"]);
                dtoTDKUOCVM.KUOC_VM.NGAY_DAO_HAN = dr["NGAY_DAO_HAN"].ToString();
                dtoTDKUOCVM.KUOC_VM.MUC_DICH_VAY = dr["MUC_DICH_VAY"].ToString();
                dtoTDKUOCVM.KUOC_VM.TRGOC_HTHUC = dr["TRGOC_HTHUC"].ToString();
                dtoTDKUOCVM.KUOC_VM.TRLAI_HTHUC = dr["TRLAI_HTHUC"].ToString();
                dtoTDKUOCVM.KUOC_VM.NGAY_GIAI_NGAN = dr["NGAY_GIAI_NGAN"].ToString();
                dtoTDKUOCVM.KUOC_VM.KHOACH_NGAY_LAP = dr["KHOACH_NGAY_LAP"].ToString();
                dtoTDKUOCVM.KUOC_VM.NV_LOAI_NVON = dr["NV_LOAI_NVON"].ToString();
                dtoTDKUOCVM.KUOC_VM.SO_TIEN_TKBB = Convert.ToDecimal(dr["SO_TIEN_TKBB"]);
                dtoTDKUOCVM.KUOC_VM.TTHAI_BGHI = dr["TTHAI_BGHI"].ToString();
                dtoTDKUOCVM.KUOC_VM.TTHAI_KUOC = dr["TTHAI_KUOC"].ToString();
                dtoTDKUOCVM.KUOC_VM.TTHAI_NVU = dr["TTHAI_NVU"].ToString();
                dtoTDKUOCVM.KUOC_VM.MA_DVI_QLY = dr["MA_DVI_QLY"].ToString();
                dtoTDKUOCVM.KUOC_VM.MA_DVI_TAO = dr["MA_DVI_TAO"].ToString();
                dtoTDKUOCVM.KUOC_VM.NGAY_CNHAT = dr["NGAY_CNHAT"].ToString();
                dtoTDKUOCVM.KUOC_VM.NGAY_NHAP = dr["NGAY_NHAP"].ToString();
                dtoTDKUOCVM.KUOC_VM.NGUOI_CNHAT = dr["NGUOI_CNHAT"].ToString();
                dtoTDKUOCVM.KUOC_VM.NGUOI_NHAP = dr["NGUOI_NHAP"].ToString();
                dtoTDKUOCVM.KUOC_VM.LAI_SUAT = Convert.ToDecimal(dr["LAI_SUAT"]);
                dtoTDKUOCVM.DD_GTLQ_SO = dr["DD_GTLQ_SO"].ToString();
                dtoTDKUOCVM.KUOC_VM.NV_LOAI_NVON_CT = dr["NV_LOAI_NVON_CT"].ToString();
                dtoTDKUOCVM.KUOC_VM.NV_MA_HOP_DONG = dr["NV_MA_HOP_DONG"].ToString();
                dtoTDKUOCVM.SP_MUC_DICH_VAY = dr["SP_MUC_DICH_VAY"].ToString();
                dtoTDKUOCVM.KUOC_VM.NGAY_LAP_KUOC = dr["NGAY_LAP_KUOC"].ToString();
                dtoTDKUOCVM.NGAY_HD = dr["NGAY_HD"].ToString();
                lstDSachKUOCVM.Add(dtoTDKUOCVM);
            }
            raddgrHDTDDS.ItemsSource = lstDSachKUOCVM;
            raddgrHDTDDS.Rebind();
        }
        #endregion

        #region Xu ly nghiep vu
        private void GetFormData(string sTrangThaiNVu)
        {
            try
            {
                List<TDVM_KHE_UOC> lstDSachKUOCVM = new List<TDVM_KHE_UOC>();
                lstDSachKUOCVM = raddgrHDTDDS.ItemsSource as List<TDVM_KHE_UOC>;
                List<TD_KIEM_SOAT_RR> lstRaw = new List<TD_KIEM_SOAT_RR>();
                lstRaw.AddRange(lst);
                lst = new List<TD_KIEM_SOAT_RR>();
                foreach (TDVM_KHE_UOC objKuoc in lstDSachKUOCVM)
                {
                    _obj = new TD_KIEM_SOAT_RR();
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
                    _obj.MA_KIEM_SOAT = txtSoKiemSoat.Text.Trim();
                    _obj.MA_PHAN_HE = DatabaseConstant.Module.TDVM.getValue();
                    _obj.DIEN_GIAI = txtDienGiai.Text.Trim();
                    _obj.SO_KIEM_SOAT = txtSoKiemSoat.Text.Trim();
                    _obj.MA_HDTD = objKuoc.KUOC_VM.MA_HDTDVM;
                    _obj.SO_HDTD = objKuoc.SO_HDTDVM;
                    _obj.SO_KUOC = objKuoc.KUOC_VM.SO_KUOCVM;
                    _obj.MA_KUOC = objKuoc.KUOC_VM.MA_KUOCVM;
                    _obj.ID_HDTD = objKuoc.KUOC_VM.ID_HDTDVM;
                    _obj.ID_KUOC = objKuoc.KUOC_VM.ID;
                    _obj.NGAY_HOP_DONG = objKuoc.NGAY_HD;
                    _obj.NGAY_KUOC = objKuoc.KUOC_VM.NGAY_LAP_KUOC;
                    if (!lstRaw.FirstOrDefault(f => f.ID_KUOC == objKuoc.KUOC_VM.ID).IsNullOrEmpty())
                        _obj.ID = lstRaw.FirstOrDefault(f => f.ID_KUOC == objKuoc.KUOC_VM.ID).ID;
                    _obj.NGAY_KIEM_SOAT = LDateTime.DateToString(teldtNgayKiemSoat.Value.Value, "yyyyMMdd");
                    lst.Add(_obj);
                }
                
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        private void SetFormData()
        {
            TinDungProcess processTinDung = new TinDungProcess();
            List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();

            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                int ret = 1;
                if (ret > 0)
                {
                    DataTable dt = null;
                    LDatatable.MakeParameterTable(ref dt);
                    LDatatable.MakeParameterTable(ref dt);
                    LDatatable.AddParameter(ref dt, "@INP_SO_KIEMSOAT", "string", txtSoKiemSoat.Text);
                    LoadDanhSachKheUocByMaGDich();
                    CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.THEM, sTrangThaiNVu, mnuMain, DatabaseConstant.Function.TDVM_KIEM_SOAT_RR);
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
            action = DatabaseConstant.Action.THEM;
            _obj = new TD_KIEM_SOAT_RR();
            txtSoKiemSoat.Text = "";
            teldtNgayKiemSoat.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai,"yyyyMMdd");
            txtSoLo.Text = "";
            txtSoKheUoc.Text = "";
            txtSoHDTD.Text = "";
            raddgrHDTDDS.ItemsSource = null;
            txtDienGiai.Text = "";

            #region Thông tin kiểm soát
            txtTrangThai.Text = "";
            teldtNgayNhap.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
            txtNguoiLap.Text = ClientInformation.TenDangNhap;
            teldtNgayCNhat.Value = null;
            txtNguoiCapNhat.Text = "";
            #endregion

            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.THEM, "", mnuMain, DatabaseConstant.Function.TDVM_KIEM_SOAT_RR);

        }

        private bool Validation()
        {
            try
            {
                if (txtSoLo.Text.Trim().IsNullOrEmpty())
                {
                    CommonFunction.ThongBaoChuaNhap(lblSoLo.Content.ToString());
                    txtSoLo.Focus();
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

        private void SetEnabledControls(bool isEnable)
        {
            txtSoLo.IsEnabled = isEnable;
            grbThongTinKiemSoat.IsEnabled = isEnable;
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

        public void OnSave()
        {
            try
            {
                if (!Validation()) return;

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

        public void OnAddNew()
        {

            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                TinDungProcess processTinDung = new TinDungProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();

                string trangThai = "";
                if (sTrangThaiNVu.Equals(BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri()))
                    trangThai = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();
                else
                    trangThai = CommonFunction.LayTrangThaiBanGhi(DatabaseConstant.Action.LUU, BusinessConstant.layTrangThaiNghiepVu(sTrangThaiNVu));

                GetFormData(trangThai);

                int ret = 0;
                ret = processTinDung.KiemSoatRuiRo(DatabaseConstant.Action.THEM, ref lst, ref listClientResponseDetail);
                _obj = lst[0];
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

        public void AfterAddNew(int ret, List<ClientResponseDetail> listClientResponseDetail)
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
                        sTrangThaiNVu = _obj.TTHAI_NVU;
                        lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);
                        txtSoKiemSoat.Text = _obj.SO_KIEM_SOAT;
                        BeforeViewFromDetail();
                        maKiemSoatRR = _obj.SO_KIEM_SOAT;
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
                listLockId.AddRange(lst.Select(f => f.ID));

                bool ret = process.LockData(DatabaseConstant.Module.TDVM,
                    DatabaseConstant.Function.TDVM_KIEM_SOAT_RR,
                    DatabaseConstant.Table.TD_KIEM_SOAT_RR,
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

        public void OnModify()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                TinDungProcess processTinDung = new TinDungProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();

                string trangThai = "";
                if (sTrangThaiNVu.Equals(BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri()))
                    trangThai = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();
                else
                    trangThai = CommonFunction.LayTrangThaiBanGhi(DatabaseConstant.Action.LUU, BusinessConstant.layTrangThaiNghiepVu(sTrangThaiNVu));

                GetFormData(trangThai);

                int ret = 0;
                ret = processTinDung.KiemSoatRuiRo(DatabaseConstant.Action.SUA, ref lst, ref listClientResponseDetail);
                _obj = lst[0];
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

        public void AfterModify(int ret, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (ret > 0)
                {
                    LMessage.ShowMessage("M.DungChung.CapNhatThanhCong", LMessage.MessageBoxType.Information);

                    sTrangThaiNVu = _obj.TTHAI_NVU;
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
                listLockId.Add(_obj.ID);

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                    DatabaseConstant.Function.TDVM_KIEM_SOAT_RR,
                    DatabaseConstant.Table.TD_KIEM_SOAT_RR,
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
                    string trangThai = "";
                    if (sTrangThaiNVu.Equals(BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri()))
                        trangThai = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();
                    else
                        trangThai = CommonFunction.LayTrangThaiBanGhi(DatabaseConstant.Action.LUU, BusinessConstant.layTrangThaiNghiepVu(sTrangThaiNVu));

                    GetFormData(trangThai);

                    // Lock dữ liệu nếu người dùng chấp nhận cảnh báo
                    UtilitiesProcess process = new UtilitiesProcess();
                    List<int> listLockId = new List<int>();
                    listLockId.AddRange(lst.Select(f=>f.ID));

                    bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                        DatabaseConstant.Function.TDVM_KIEM_SOAT_RR,
                        DatabaseConstant.Table.TD_KIEM_SOAT_RR,
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
                listLockId.AddRange(lst.Select(f => f.ID));

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                    DatabaseConstant.Function.TDVM_KIEM_SOAT_RR,
                    DatabaseConstant.Table.TD_KIEM_SOAT_RR,
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
            TinDungProcess processTinDung = new TinDungProcess();
            try
            {
                int ret = 0;
                _obj = lst[0];
                List<TD_KIEM_SOAT_RR> lstRaw = new List<TD_KIEM_SOAT_RR>();
                lstRaw.Add(_obj);
                ret = processTinDung.KiemSoatRuiRo(action, ref lstRaw, ref listClientResponseDetail);
                _obj = lst[0];
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
                processTinDung = null;
            }
        }

        public void AfterDelete(int ret, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                // Yêu cầu unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.AddRange(lst.Select(f => f.ID));

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                    DatabaseConstant.Function.TDVM_KIEM_SOAT_RR,
                    DatabaseConstant.Table.TD_KIEM_SOAT_RR,
                    DatabaseConstant.Action.XOA,
                    listLockId);
                if (ret > 0)
                {
                    LMessage.ShowMessage("M.DungChung.XoaThanhCong", LMessage.MessageBoxType.Information);
                }
                else
                {
                    CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                    //LMessage.ShowMessage("M.DungChung.XoaKhongThanhCong", LMessage.MessageBoxType.Warning);
                }

                

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
                    string trangThai = "";
                    if (sTrangThaiNVu.Equals(BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri()))
                        trangThai = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();
                    else
                        trangThai = CommonFunction.LayTrangThaiBanGhi(DatabaseConstant.Action.LUU, BusinessConstant.layTrangThaiNghiepVu(sTrangThaiNVu));

                    GetFormData(trangThai);

                    // Lock dữ liệu nếu người dùng chấp nhận cảnh báo
                    UtilitiesProcess process = new UtilitiesProcess();
                    List<int> listLockId = new List<int>();
                    listLockId.AddRange(lst.Select(f => f.ID));
                    bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                        DatabaseConstant.Function.TDVM_KIEM_SOAT_RR,
                        DatabaseConstant.Table.TD_KIEM_SOAT_RR,
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
            TinDungProcess processTinDung = new TinDungProcess();
            try
            {
                int ret = 0;
                _obj = lst[0];
                List<TD_KIEM_SOAT_RR> lstRaw = new List<TD_KIEM_SOAT_RR>();
                lstRaw.Add(_obj);
                ret = processTinDung.KiemSoatRuiRo(action, ref lstRaw, ref listClientResponseDetail);
                _obj = lst[0];
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
                processTinDung = null;
            }
        }

        public void AfterApprove(int ret, List<ClientResponseDetail> listClientResponseDetail)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
            try
            {
                // Yêu cầu unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.AddRange(lst.Select(f => f.ID));

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                    DatabaseConstant.Function.TDVM_KIEM_SOAT_RR,
                    DatabaseConstant.Table.TD_KIEM_SOAT_RR,
                    DatabaseConstant.Action.DUYET,
                    listLockId);
                if (ret > 0)
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
                    string trangThai = "";
                    if (sTrangThaiNVu.Equals(BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri()))
                        trangThai = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();
                    else
                        trangThai = CommonFunction.LayTrangThaiBanGhi(DatabaseConstant.Action.LUU, BusinessConstant.layTrangThaiNghiepVu(sTrangThaiNVu));

                    GetFormData(trangThai);

                    // Lock dữ liệu nếu người dùng chấp nhận cảnh báo
                    UtilitiesProcess process = new UtilitiesProcess();
                    List<int> listLockId = new List<int>();
                    listLockId.AddRange(lst.Select(f => f.ID));

                    bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                        DatabaseConstant.Function.TDVM_KIEM_SOAT_RR,
                        DatabaseConstant.Table.TD_KIEM_SOAT_RR,
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
            TinDungProcess processTinDung = new TinDungProcess();
            try
            {
                int ret = 0;
                _obj = lst[0];
                List<TD_KIEM_SOAT_RR> lstRaw = new List<TD_KIEM_SOAT_RR>();
                lstRaw.Add(_obj);
                ret = processTinDung.KiemSoatRuiRo(action, ref lstRaw, ref listClientResponseDetail);
                _obj = lst[0];
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
                processTinDung = null;
            }
        }

        public void AfterCancel(int ret, List<ClientResponseDetail> listClientResponseDetail)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
            try
            {
                // Yêu cầu unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.AddRange(lst.Select(f => f.ID));

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                    DatabaseConstant.Function.TDVM_KIEM_SOAT_RR,
                    DatabaseConstant.Table.TD_KIEM_SOAT_RR,
                    DatabaseConstant.Action.THOAI_DUYET,
                    listLockId);
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
                    string trangThai = "";
                    if (sTrangThaiNVu.Equals(BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri()))
                        trangThai = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();
                    else
                        trangThai = CommonFunction.LayTrangThaiBanGhi(DatabaseConstant.Action.LUU, BusinessConstant.layTrangThaiNghiepVu(sTrangThaiNVu));

                    GetFormData(trangThai);

                    // Lock dữ liệu nếu người dùng chấp nhận cảnh báo
                    UtilitiesProcess process = new UtilitiesProcess();
                    List<int> listLockId = new List<int>();
                    listLockId.AddRange(lst.Select(f => f.ID));

                    bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                        DatabaseConstant.Function.TDVM_KIEM_SOAT_RR,
                        DatabaseConstant.Table.TD_KIEM_SOAT_RR,
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
            TinDungProcess processTinDung = new TinDungProcess();
            try
            {
                int ret = 0;
                _obj = lst[0];
                List<TD_KIEM_SOAT_RR> lstRaw = new List<TD_KIEM_SOAT_RR>();
                lstRaw.Add(_obj);
                ret = processTinDung.KiemSoatRuiRo(action, ref lstRaw, ref listClientResponseDetail);
                _obj = lst[0];
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
                processTinDung = null;
            }
        }

        public void AfterRefuse(int ret, List<ClientResponseDetail> listClientResponseDetail)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
            try
            {

                // Yêu cầu unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.AddRange(lst.Select(f => f.ID));

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                    DatabaseConstant.Function.TDVM_KIEM_SOAT_RR,
                    DatabaseConstant.Table.TD_KIEM_SOAT_RR,
                    DatabaseConstant.Action.TU_CHOI_DUYET,
                    listLockId);
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

            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        #endregion

        private void txtSoKuoc_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F3)
            {
                btnSoLo_Click(null, null);
            }
        }

        private void btnSoLo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<string> lstDieuKien = new List<string>();
                lstDieuKien.Add(ClientInformation.MaDonViGiaoDich);
                lstPopup = new List<DataRow>();
                PopupProcess popupProcess = new PopupProcess();
                popupProcess.getPopupInformation("POPUP_DS_KUOCVM_RUI_RO", lstDieuKien);
                SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;
                ucPopup popup = new ucPopup(false, simplePopupResponse, false);
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
                        txtSoLo.Text = dr["SO_GDICH_KU"].ToString();
                        txtSoKheUoc.Text = dr["SO_KUOCVM"].ToString();
                        txtSoHDTD.Text = dr["SO_HDTDVM"].ToString();
                        txtSoKheUoc.Tag = dr["SO_GDICH_KU"].ToString();
                        txtSoHDTD.Tag = dr["SO_GDICH_HD"].ToString();
                        LoadDanhSachKheUocByMaGDich();
                    }
                }
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        private void btnViewKuoc_Click(object sender, RoutedEventArgs e)
        {
            Cursor = Cursors.Wait;
            ucLapKheUocDS_01 objKheUocCT = new ucLapKheUocDS_01();
            objKheUocCT.action = DatabaseConstant.Action.XEM;
            objKheUocCT.objKUOCVMDS = new Presentation.Process.TinDungServiceRef.TDVM_KHE_UOC_DSACH();
            objKheUocCT.objKUOCVMDS.MA_GDICH = txtSoLo.Text;
            objKheUocCT.SetDataForm();
            string tittle = DatabaseConstant.layNgonNguTieuDeForm(DatabaseConstant.Function.TDVM_DANH_SACH_KHE_UOC_01);
            Window window = new Window();
            window.Title = tittle;
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.Content = objKheUocCT;
            window.ShowDialog();
            Cursor = Cursors.Arrow;
        }

        private void btnViewHDTD_Click(object sender, RoutedEventArgs e)
        {
            Cursor = Cursors.Wait;
            ucHDTDTheoNhomCT objHDTDThoaThuan = new ucHDTDTheoNhomCT();
            objHDTDThoaThuan.action = DatabaseConstant.Action.XEM;
            objHDTDThoaThuan.SetDataForm(txtSoHDTD.Tag.ToString());
            string tittle = DatabaseConstant.layNgonNguTieuDeForm(DatabaseConstant.Function.TDVM_CHI_TIET_HOP_DONG);
            Window window = new Window();
            window.Title = tittle;
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.Content = objHDTDThoaThuan;
            window.ShowDialog();
            Cursor = Cursors.Arrow;
        }

        private void btnViewDoiTuong_Click(object sender, RoutedEventArgs e)
        {
            if(cboDoiTuong.SelectedValue.Equals("HD") && !txtSoHDTD.Tag.IsNullOrEmpty())
            {
                btnViewHDTD_Click(null, null);
            }
            else if (cboDoiTuong.SelectedValue.Equals("KU") && !txtSoLo.Text.IsNullOrEmptyOrSpace())
            {
                btnViewKuoc_Click(null, null);
            }
        }

        private void txtSoKuoc_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                try
                {
                    List<string> lstDieuKien = new List<string>();
                    lstDieuKien.Add(ClientInformation.MaDonViGiaoDich);
                    lstPopup = new List<DataRow>();
                    PopupProcess popupProcess = new PopupProcess();
                    popupProcess.getPopupInformation("POPUP_DS_KUOCVM_RUI_RO", lstDieuKien);
                    SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;
                    ucPopup popup = new ucPopup(false, simplePopupResponse, false);
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
                            txtSoLo.Text = dr["MA_GDICH"].ToString();
                            LoadDanhSachKheUocByMaGDich();
                        }
                    }
                }
                catch (Exception ex)
                {
                    CommonFunction.ThongBaoLoi(ex);
                    LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                }
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }
    }
}
