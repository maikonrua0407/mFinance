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
using System.Windows.Threading;
using System.Data;
using Microsoft.Windows.Controls.Ribbon;
using Utilities.Common;
using PresentationWPF.CustomControl;
using Telerik.Windows.Controls;
using Presentation.Process;
using Presentation.Process.PopupServiceRef;
using Presentation.Process.Common;
using System.Reflection;
using PresentationWPF.BaoCao.DungChung;
using Presentation.Process.BaoCaoServiceRef;
using System.Collections;
using PresentationWPF.HoaDonTienKy.PopupNghiepVu;
using Presentation.Process.TinDungServiceRef;

namespace PresentationWPF.HoaDonTienKy.ThuGocLai
{
    /// <summary>
    /// Interaction logic for ucThuGocLaiCT.xaml
    /// </summary>
    public partial class ucThuGocLaiCT : UserControl
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
        List<DataRow> lstPopup = new List<DataRow>();
        public void LayDuLieuTuPopup(List<DataRow> lst)
        {
            lstPopup = lst;
        }

        public void LayDuLieuTuPopup(DANH_SACH_KHE_UOC_VONG_VAY _obj)
        {
            if (!_obj.IsNullOrEmpty())
                _objTTinThuGocLai = _obj;
        }
        private List<AutoCompleteEntry> lstSourceTienTe = new List<AutoCompleteEntry>();
        private List<AutoCompleteEntry> lstSourceHinhThucThanhToan = new List<AutoCompleteEntry>();
        public DatabaseConstant.Action action;
        public event EventHandler OnSavingCompleted;
        TDVM_THU_GOC_LAI_TRUOC_HAN obj = new TDVM_THU_GOC_LAI_TRUOC_HAN();
        DANH_SACH_KHE_UOC_VONG_VAY _objTTinThuGocLai = new DANH_SACH_KHE_UOC_VONG_VAY();
        THONG_TIN_THU_NO objTTinThuNo = new THONG_TIN_THU_NO();
        List<DANH_SACH_SO> lstThongTinRutTK = null;
        List<DANH_SACH_SO> lstThongTinNopTK = null;
        List<THONG_TIN_THU_NO> lstThongTinThuNoTTruoc = null;
        List<THONG_TIN_THU_NO> lstThongTinThuNo = null;
        BIEU_PHI_DTO objBieuPhi = null;
        List<BIEU_PHI_CTIET_DTO> lstBieuPhi = null;
        string TThaiNVu = "";
        List<int> lstId = null;
        int iDGDich = 0;

        string _thuTuPhanBo;

        public decimal GocTrongHan { get; set; }
        public decimal GocQuaHan { get; set; }
        public decimal LaiTrongHan { get; set; }
        public decimal LaiQuaHan { get; set; }
        public decimal DuThuTrongHan { get; set; }
        public decimal DuThuQuaHan { get; set; }
        public decimal SoTienMat { get; set; }
        public decimal SoTienNopCATK { get; set; }
        public decimal SoTienNopThua { get; set; }
        public decimal SoTienGocLaiTruoc { get; set; }
        public decimal SoTienPhi { get; set; }
        public decimal LaiPhat { get; set; }
        public bool CoThuTienMat { get; set; }
        public bool CoThuTuCA { get; set; }
        public bool CoNopVaoCA { get; set; }
        public bool CoTraTruoc { get; set; }
        public bool CoTatToan { get; set; }
        public bool CoThuLai { get; set; }
        #endregion

        #region Khoi tao
        public ucThuGocLaiCT()
        {
            InitializeComponent();
            KhoiTaoComboBox();
            ResetForm();
            InitEventHanler();
            InitEventHanlerProcess();
            GetValueParams();
        }

        public ucThuGocLaiCT(KIEM_SOAT _objKiemSoat)
            : this()
        {
            obj.ID_GIAO_DICH = _objKiemSoat.ID;
            obj.MA_GIAO_DICH = _objKiemSoat.SO_GIAO_DICH;
            obj.TRANG_THAI_NGHIEP_VU = _objKiemSoat.TTHAI_NVU;
            action = _objKiemSoat.action;
            SetDataForm();
        }
        private void KhoiTaoComboBox()
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
                combo.combobox = txtLoaiTien;
                combo.lstSource = lstSourceTienTe;
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

        private void InitEventHanler()
        {
            tlbViewDetail.Click += new RoutedEventHandler(tlbViewDetail_Click);
            btnSoKheUoc.Click += new RoutedEventHandler(btnSoKheUoc_Click);
            chkTatToan.Click += new RoutedEventHandler(chkTatToan_Click);
        }

        private void InitEventHanlerProcess()
        {
            radNumSoTienGiaoDich.ValueChanged += new EventHandler<Telerik.Windows.RadRoutedEventArgs>(radNumSoTienGiaoDich_ValueChanged);
            radNumSoTienMat.ValueChanged += new EventHandler<Telerik.Windows.RadRoutedEventArgs>(radNumSoTienMat_ValueChanged);
            radNumSoTienCA.ValueChanged += new EventHandler<Telerik.Windows.RadRoutedEventArgs>(radNumSoTienCA_ValueChanged);
            radTongNopCA.ValueChanged += new EventHandler<Telerik.Windows.RadRoutedEventArgs>(radTongNopCA_ValueChanged);
            radTongNopTK.ValueChanged += new EventHandler<Telerik.Windows.RadRoutedEventArgs>(radTongNopTK_ValueChanged);
            radTongLaiPhat.ValueChanged += new EventHandler<Telerik.Windows.RadRoutedEventArgs>(radTongLaiPhat_ValueChanged);
            radTongPhi.ValueChanged += new EventHandler<Telerik.Windows.RadRoutedEventArgs>(radTongPhi_ValueChanged);
            radTongLai.ValueChanged += new EventHandler<Telerik.Windows.RadRoutedEventArgs>(radTongLai_ValueChanged);
        }

        private void ClearEventHanlerProcess()
        {
            radNumSoTienGiaoDich.ValueChanged -= new EventHandler<Telerik.Windows.RadRoutedEventArgs>(radNumSoTienGiaoDich_ValueChanged);
            radNumSoTienMat.ValueChanged -= new EventHandler<Telerik.Windows.RadRoutedEventArgs>(radNumSoTienMat_ValueChanged);
            radNumSoTienCA.ValueChanged -= new EventHandler<Telerik.Windows.RadRoutedEventArgs>(radNumSoTienCA_ValueChanged);
            radTongNopCA.ValueChanged -= new EventHandler<Telerik.Windows.RadRoutedEventArgs>(radTongNopCA_ValueChanged);
            radTongNopTK.ValueChanged -= new EventHandler<Telerik.Windows.RadRoutedEventArgs>(radTongNopTK_ValueChanged);
            radTongLaiPhat.ValueChanged -= new EventHandler<Telerik.Windows.RadRoutedEventArgs>(radTongLaiPhat_ValueChanged);
            radTongPhi.ValueChanged -= new EventHandler<Telerik.Windows.RadRoutedEventArgs>(radTongPhi_ValueChanged);
            radTongLai.ValueChanged -= new EventHandler<Telerik.Windows.RadRoutedEventArgs>(radTongLai_ValueChanged);
        }

        private void GetValueParams()
        {
            _thuTuPhanBo = new UtilitiesProcess().LayGiaTriThamSo(BusinessConstant.LoaiThamSo.TW, BusinessConstant.MaThamSo.TW_TDVM_THU_TU_PHAN_BO_THU_TIEN_KY, ClientInformation.MaDonVi);
            if (_thuTuPhanBo.IsNullOrEmptyOrSpace())
                _thuTuPhanBo = "GOCVAY#LAI_VAY#LAI_QHAN#TKBB#QUY_TT";
        }
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
                        KeyGesture keyg = new KeyGesture(Key.I, ModifierKeys.Shift);
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
                        KeyGesture keyg = new KeyGesture(Key.S, ModifierKeys.Shift);
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
                        KeyGesture keyg = new KeyGesture(Key.S, ModifierKeys.Control | ModifierKeys.Shift);
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
            txtSoGiaoDich.Focus();

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

            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.NHAP_DU_LIEU)))
            { }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THEM)))
            {
                ResetForm();
                SetEnabledAllControls(true);
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
                beforeModify();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
            {
                BeforeDelete();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.NHAN_BAN)))
            { }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU_TAM)))
            {
                BeforeSave(BusinessConstant.TrangThaiNghiepVu.LUU_TAM, BusinessConstant.TrangThaiBanGhi.SU_DUNG);
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRINH_DUYET)))
            {
                BeforeSave(BusinessConstant.TrangThaiNghiepVu.CHO_DUYET, BusinessConstant.TrangThaiBanGhi.SU_DUNG);
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.BANG_KE)))
            { }
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
            else if (strTinhNang.Equals("Print"))
            {
                OnPrint();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XEM_TRUOC)))
            {
                OnPreview();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XEM)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XUAT_DU_LIEU)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TIM_KIEM)))
            { }
            else if (strTinhNang.Equals("PreviewDonVayVon"))
            {
                OnPreviewDonVayVon();
            }
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
            txtSoGiaoDich.Focus();
            string strTinhNang = ((MenuItem)sender).Name.Substring(3, ((MenuItem)sender).Name.Length - 3);
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.NHAP_DU_LIEU)))
            { }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THEM)))
            {
                ResetForm();
                SetEnabledAllControls(true);
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
                beforeModify();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
            {
                BeforeDelete();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.NHAN_BAN)))
            { }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU_TAM)))
            {
                BeforeSave(BusinessConstant.TrangThaiNghiepVu.LUU_TAM, BusinessConstant.TrangThaiBanGhi.SU_DUNG);
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRINH_DUYET)))
            {
                BeforeSave(BusinessConstant.TrangThaiNghiepVu.CHO_DUYET, BusinessConstant.TrangThaiBanGhi.SU_DUNG);
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.BANG_KE)))
            { }
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
            else if (strTinhNang.Equals("Print"))
            {
                OnPrint();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XEM_TRUOC)))
            {
                OnPreview();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XEM)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XUAT_DU_LIEU)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TIM_KIEM)))
            { }
            else if (strTinhNang.Equals("PreviewDonVayVon"))
            {
                OnPreviewDonVayVon();
            }
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

        #region Xu ly giao dien

        /// <summary>
        /// Sự kiện nhấn key trên form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            PresentationWPF.CustomControl.CommonFunction.CloseUserControl(e, this);
            PresentationWPF.CustomControl.CommonFunction.SelectNextControl(e);
        }

        void tlbViewDetail_Click(object sender, RoutedEventArgs e)
        {
            if (txtSoKheUoc.Text.IsNullOrEmptyOrSpace())
                return;
            List<THONG_TIN_THU_NO> lstThongTinKH = new List<THONG_TIN_THU_NO>();
            lstThongTinKH.AddRange(lstThongTinThuNo);
            lstThongTinKH.AddRange(lstThongTinThuNoTTruoc);
            _objTTinThuGocLai.DSACH_SO_NOP_TIEN = lstThongTinNopTK.ToArray();
            _objTTinThuGocLai.DSACH_SO_RUT_TIEN = lstThongTinRutTK.ToArray();
            _objTTinThuGocLai.DSACH_THONG_TIN_THU_NO = lstThongTinKH.ToArray();
            _objTTinThuGocLai.TONG_SO_TIEN = (decimal)radNumSoTienGiaoDich.Value;
            _objTTinThuGocLai.THUC_THU_TIEN_MAT = (decimal)radNumSoTienMat.Value;
            _objTTinThuGocLai.THUC_NOP_TU_TKKKH = (decimal)radNumSoTienCA.Value;
            DANH_SACH_KHE_UOC_VONG_VAY _obj = new DANH_SACH_KHE_UOC_VONG_VAY();
            _obj = _objTTinThuGocLai;
            ucPopupThuGocLaiCT ucPopupCT = new ucPopupThuGocLaiCT(_obj, obj.NGAY_THU_TIEN_KY);
            ucPopupCT.DuLieuTraVe = new ucPopupThuGocLaiCT.LayDuLieu(LayDuLieuTuPopup);
            Window win = new Window();
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.WindowState = WindowState.Maximized;
            string[] tenKhang = new string[1];
            tenKhang[0] = txtTenKHang.Text;
            win.Title = LLanguage.SearchResourceByKey("U.TinDungTD.ucThuGocLaiCT.ThongTinChiTietThucThuKH", tenKhang);
            win.Content = ucPopupCT;
            win.ShowDialog();

            lstThongTinThuNo = _objTTinThuGocLai.DSACH_THONG_TIN_THU_NO.Where(f => f.NGAY_KH.CompareTo(ClientInformation.NgayLamViecHienTai) <= 0).ToList();
            lstThongTinThuNoTTruoc = _objTTinThuGocLai.DSACH_THONG_TIN_THU_NO.Where(f => f.NGAY_KH.CompareTo(ClientInformation.NgayLamViecHienTai) > 0).ToList();
            lstThongTinNopTK = _objTTinThuGocLai.DSACH_SO_NOP_TIEN.ToList();
            lstThongTinRutTK = _objTTinThuGocLai.DSACH_SO_RUT_TIEN.ToList();

            GocTrongHan = lstThongTinThuNo.Where(f => f.NHOM_NO.Equals("part0")).Sum(f => f.GOC_TT);
            GocQuaHan = lstThongTinThuNo.Where(f => !f.NHOM_NO.Equals("part0")).Sum(f => f.GOC_TT);
            LaiTrongHan = lstThongTinThuNo.Where(f => f.NHOM_NO.Equals("part0")).Sum(f => f.LAI_TT);
            LaiQuaHan = lstThongTinThuNo.Where(f => !f.NHOM_NO.Equals("part0")).Sum(f => f.LAI_TT);
            GocTrongHan += lstThongTinThuNoTTruoc.Where(f => f.NHOM_NO.Equals("part0")).Sum(f => f.GOC_TT);
            GocQuaHan += lstThongTinThuNoTTruoc.Where(f => !f.NHOM_NO.Equals("part0")).Sum(f => f.GOC_TT);
            LaiTrongHan += lstThongTinThuNoTTruoc.Where(f => f.NHOM_NO.Equals("part0")).Sum(f => f.LAI_TT);
            LaiQuaHan += lstThongTinThuNoTTruoc.Where(f => !f.NHOM_NO.Equals("part0")).Sum(f => f.LAI_TT);

            radTongGoc.Value = (double)(GocTrongHan + GocQuaHan);
            radTongLai.Value = (double)(LaiTrongHan + LaiQuaHan);
            radNumSoTienMat.Value = (double)_objTTinThuGocLai.THUC_THU_TIEN_MAT;
            radNumSoTienGiaoDich.Value = (double)_objTTinThuGocLai.THUC_THU_TONG;
            radNumSoTienCA.Value = (double)_objTTinThuGocLai.THUC_NOP_TU_TKKKH;
            radTongLaiPhat.Value = (double)_objTTinThuGocLai.LAI_PHAT;
            radTongPhi.Value = (double)_objTTinThuGocLai.PHI_TRA_TRUOC;
            radTongNopCA.Value = (double)lstThongTinNopTK.Where(f => f.LOAI_SAN_PHAM_HDV.Equals("T08")).Sum(f => f.SO_TIEN_NOP_VAO);
            radTongNopTK.Value = (double)lstThongTinNopTK.Where(f => !f.LOAI_SAN_PHAM_HDV.Equals("T08")).Sum(f => f.SO_TIEN_NOP_VAO);

        }

        void radTongNopTK_ValueChanged(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            ClearEventHanlerProcess();
            if ((decimal)radTongNopTK.Value > SoTienNopThua)
            {
                radTongNopTK.Value = (double)SoTienNopThua;
            }
            lstThongTinNopTK.ForEach(f => f.SO_TIEN_NOP_VAO = 0);
            lstThongTinNopTK.FirstOrDefault(f => !f.LOAI_SAN_PHAM_HDV.Equals("T08")).SO_TIEN_NOP_VAO = (decimal)radTongNopTK.Value;
            lstThongTinNopTK.FirstOrDefault(f => f.LOAI_SAN_PHAM_HDV.Equals("T08")).SO_TIEN_NOP_VAO = SoTienNopThua-(decimal)radTongNopTK.Value;
            radTongNopCA.Value = (double)SoTienNopThua - radTongNopTK.Value;
            InitEventHanlerProcess();
        }

        void radTongNopCA_ValueChanged(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            ClearEventHanlerProcess();
            if ((decimal)radTongNopCA.Value > SoTienNopThua)
            {
                radTongNopCA.Value = (double)SoTienNopThua;
            }
            lstThongTinNopTK.ForEach(f=>f.SO_TIEN_NOP_VAO = 0);
            lstThongTinNopTK.FirstOrDefault(f => f.LOAI_SAN_PHAM_HDV.Equals("T08")).SO_TIEN_NOP_VAO = (decimal)radTongNopCA.Value;
            lstThongTinNopTK.FirstOrDefault(f => !f.LOAI_SAN_PHAM_HDV.Equals("T08")).SO_TIEN_NOP_VAO = SoTienNopThua - (decimal)radTongNopCA.Value;
            radTongNopTK.Value = (double)SoTienNopThua - radTongNopCA.Value;
            InitEventHanlerProcess();
        }

        void radNumSoTienCA_ValueChanged(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            ClearEventHanlerProcess();
            decimal SoTienRutCA = Convert.ToDecimal(radNumSoTienCA.Value.GetValueOrDefault());
            decimal SoTienGiaoDich = Convert.ToDecimal(radNumSoTienGiaoDich.Value.GetValueOrDefault());
            SoTienRutCA = Math.Min(SoTienRutCA, SoTienGiaoDich);
            radNumSoTienCA.Value = (double)SoTienRutCA;
            _objTTinThuGocLai.THUC_NOP_TU_TKKKH = SoTienRutCA;
            decimal SoDuTK = 0;
            if (!lstThongTinRutTK.Where(f => f.LOAI_SAN_PHAM_HDV.Equals("T08")).IsNullOrEmpty())
            {
                foreach (DANH_SACH_SO objTTTK in lstThongTinRutTK.Where(f => f.LOAI_SAN_PHAM_HDV.Equals("T08")))
                {
                    SoDuTK = objTTTK.SO_DU;
                    objTTTK.SO_TIEN_RUT_RA = Math.Min(SoDuTK, SoTienRutCA);
                    SoTienRutCA -= objTTTK.SO_TIEN_RUT_RA;
                    if (SoTienRutCA == 0)
                        break;
                }
            }

            if (!lstThongTinRutTK.Where(f => !f.LOAI_SAN_PHAM_HDV.Equals("T08")).IsNullOrEmpty())
            {
                foreach (DANH_SACH_SO objTTTK in lstThongTinRutTK.Where(f => !f.LOAI_SAN_PHAM_HDV.Equals("T08")))
                {
                    SoDuTK = objTTTK.SO_DU;
                    objTTTK.SO_TIEN_RUT_RA = Math.Min(SoDuTK, SoTienRutCA);
                    SoTienRutCA -= objTTTK.SO_TIEN_RUT_RA;
                    if (SoTienRutCA == 0)
                        break;
                }
            }

            if (SoTienRutCA > 0)
            {
                SoTienRutCA = lstThongTinRutTK.Sum(f => f.SO_TIEN_RUT_RA);
                radNumSoTienCA.Value = (double)SoTienRutCA;
                _objTTinThuGocLai.THUC_NOP_TU_TKKKH = SoTienRutCA;
            }
            
            radNumSoTienMat.Value = radNumSoTienGiaoDich.Value.GetValueOrDefault() - radNumSoTienCA.Value.GetValueOrDefault();
            _objTTinThuGocLai.THUC_THU_TIEN_MAT = (decimal)radNumSoTienMat.Value.GetValueOrDefault();
            _objTTinThuGocLai.THUC_THU_TONG = _objTTinThuGocLai.THUC_THU_TIEN_MAT + _objTTinThuGocLai.THUC_NOP_TU_TKKKH;

            if (chkTatToan.IsChecked.GetValueOrDefault())
                TatToan();
            else
                TinhToanTraGocLai();
            InitEventHanlerProcess();
        }

        void radNumSoTienMat_ValueChanged(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            ClearEventHanlerProcess();
            TinhToanTraGocLai();
            InitEventHanlerProcess();
        }

        void radNumSoTienGiaoDich_ValueChanged(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            ClearEventHanlerProcess();
            radNumSoTienMat.Value = radNumSoTienGiaoDich.Value;
            radNumSoTienCA.Value = 0;
            lstThongTinRutTK.ForEach(f => f.SO_TIEN_RUT_RA = 0);
            _objTTinThuGocLai.THUC_NOP_TU_TKKKH = 0;
            if (chkTatToan.IsChecked.GetValueOrDefault())
                TatToan();
            else
                TinhToanTraGocLai();
            InitEventHanlerProcess();
        }

        void btnSoKheUoc_Click(object sender, RoutedEventArgs e)
        {
            lstPopup.Clear();
            List<string> lstDieuKien = new List<string>();
            lstDieuKien.Add(ClientInformation.MaDonViGiaoDich);
            lstDieuKien.Add(ClientInformation.NgayLamViecHienTai);
            PopupProcess popupProcess = new PopupProcess();
            popupProcess.getPopupInformation("POPUP_DS_TDVM_KHE_UOC_THU_GOC_LAI", lstDieuKien);
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
                DataRow dr = lstPopup.FirstOrDefault();
                obj.NGAY_THU_TIEN_KY = ClientInformation.NgayLamViecHienTai;
                _objTTinThuGocLai.ID_KHACH_HANG = Convert.ToInt32(dr["ID_KHANG"]);
                _objTTinThuGocLai.ID_KHE_UOC = Convert.ToInt32(dr["ID_KUOC"]);
                _objTTinThuGocLai.LOAI_TIEN = Convert.ToString(dr["LOAI_TIEN"]);
                _objTTinThuGocLai.MA_KHACH_HANG = Convert.ToString(dr["MA_KHANG"]);
                _objTTinThuGocLai.MA_KHE_UOC = Convert.ToString(dr["MA_KUOC"]);
                _objTTinThuGocLai.MA_SAN_PHAM_TD = Convert.ToString(dr["MA_SAN_PHAM"]);
                _objTTinThuGocLai.NGAY_GD = ClientInformation.NgayLamViecHienTai;
                obj.NGAY_GIAO_DICH = ClientInformation.NgayLamViecHienTai;
                obj.NGUOI_GD = ClientInformation.HoTen;
                obj.SO_CUM = Convert.ToString(dr["MA_CUM"]);
                _objTTinThuGocLai.NHOM_NO_HIEN_TAI = Convert.ToString(dr["NHOM_NO"]);
                _objTTinThuGocLai.NV_LOAI_NVON = Convert.ToString(dr["NV_LOAI_NVON"]);
                obj.DIA_CHI = Convert.ToString(dr["DIA_CHI"]);
                _objTTinThuGocLai.HOAN_DU_PHONG = Convert.ToDecimal(dr["DPHONG_DA_TRICH"]);
                if (dr["LAI_DA_XUAT_NB"] != DBNull.Value)
                    _objTTinThuGocLai.SO_TIEN_DU_THU_QH = Convert.ToDecimal(dr["LAI_DA_XUAT_NB"]);
                if (dr["LAI_DU_THU"] != DBNull.Value)
                    _objTTinThuGocLai.SO_TIEN_DU_THU = Convert.ToDecimal(dr["LAI_DU_THU"]);
                if (dr["DU_THU_DEN_NGAY"] != DBNull.Value)
                    _objTTinThuGocLai.NGAY_DU_THU = dr["DU_THU_DEN_NGAY"].ToString();
                _objTTinThuGocLai.TEN_KHACH_HANG = Convert.ToString(dr["TEN_KHANG"]);
                _objTTinThuGocLai.DU_NO = Convert.ToDecimal(dr["SO_DU"]);
                _objTTinThuGocLai.SO_HDTD = Convert.ToString(dr["SO_HDTD"]);
                _objTTinThuGocLai.SO_KHE_UOC = Convert.ToString(dr["SO_KUOC"]);
                txtSoKheUoc.Text = Convert.ToString(dr["SO_KUOC"]);
                txtSoHDTD.Text = Convert.ToString(dr["SO_HDTD"]);
                txtMaKHang.Text = Convert.ToString(dr["MA_KHANG"]);
                txtTenKHang.Text = Convert.ToString(dr["TEN_KHANG"]);
                txtSoDu.Value = Convert.ToDouble(dr["SO_DU"]);
                txtLoaiTien.SelectedIndex = lstSourceTienTe.IndexOf(lstSourceTienTe.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(_objTTinThuGocLai.LOAI_TIEN)));
                GetKeHoachThuGocLai();
            }
        }

        void radTongLaiPhat_ValueChanged(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            ClearEventHanlerProcess();
            LaiPhat = Convert.ToDecimal(radTongLaiPhat.Value.GetValueOrDefault());
            decimal TongTienGD = SoTienMat + SoTienNopCATK - GocTrongHan - GocQuaHan - LaiTrongHan - LaiQuaHan - SoTienGocLaiTruoc - SoTienPhi;
            if (TongTienGD < LaiPhat)
            {
                LaiPhat = TongTienGD;
                radTongLaiPhat.Value = (double)LaiPhat;
            }
            _objTTinThuGocLai.LAI_PHAT = LaiPhat;
            TinhToanTienThuaNopTKBB();
            InitEventHanlerProcess();
        }

        void chkTatToan_Click(object sender, RoutedEventArgs e)
        {
            ClearEventHanlerProcess();
            if (chkTatToan.IsChecked.GetValueOrDefault())
            {
                TatToan();
                _objTTinThuGocLai.THUC_THU_TONG = GocTrongHan + GocQuaHan + LaiTrongHan + LaiQuaHan + SoTienPhi + LaiPhat;
                _objTTinThuGocLai.THUC_THU_TIEN_MAT = _objTTinThuGocLai.THUC_THU_TONG;
                _objTTinThuGocLai.THUC_NOP_TU_TKKKH = 0;
                radNumSoTienMat.Value = (double)_objTTinThuGocLai.THUC_THU_TIEN_MAT;
                radNumSoTienGiaoDich.Value = (double)_objTTinThuGocLai.THUC_THU_TONG;
                radNumSoTienCA.Value = (double)_objTTinThuGocLai.THUC_NOP_TU_TKKKH;
                radTongNopCA.Value = (double)lstThongTinNopTK.Where(f => f.LOAI_SAN_PHAM_HDV.Equals("T08")).Sum(f => f.SO_TIEN_NOP_VAO);
                radTongNopTK.Value = (double)lstThongTinNopTK.Where(f => !f.LOAI_SAN_PHAM_HDV.Equals("T08")).Sum(f => f.SO_TIEN_NOP_VAO);
            }
            else
            {
                TinhToanTraGocLai();
            }
            InitEventHanlerProcess();
        }

        void radTongPhi_ValueChanged(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            ClearEventHanlerProcess();
            SoTienPhi = Convert.ToDecimal(radTongPhi.Value.GetValueOrDefault());
            decimal TongTienGD = SoTienMat + SoTienNopCATK - GocTrongHan - GocQuaHan - LaiTrongHan - LaiQuaHan - SoTienGocLaiTruoc - LaiPhat;
            if (TongTienGD < SoTienPhi)
            {
                SoTienPhi = TongTienGD;
                radTongPhi.Value = (double)SoTienPhi;
            }
            _objTTinThuGocLai.PHI_TRA_TRUOC = SoTienPhi;
            TinhToanTienThuaNopTKBB();
            InitEventHanlerProcess();
        }

        private void radTongLai_ValueChanged(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            ClearEventHanlerProcess();
            try
            {
                decimal soTienLai = (decimal)radTongLai.Value.GetValueOrDefault();
                decimal TongTienGD = SoTienMat + SoTienNopCATK - GocTrongHan - GocQuaHan - SoTienGocLaiTruoc - SoTienPhi - LaiPhat;
                if (TongTienGD < soTienLai)
                {
                    soTienLai = TongTienGD;
                    radTongLai.Value = (double)soTienLai;
                }
                foreach (THONG_TIN_THU_NO objTdtdThongTinThuNo in lstThongTinThuNo)
                {
                    decimal soTienLaiTT = Math.Min(objTdtdThongTinThuNo.LAI_KH, soTienLai);
                    objTdtdThongTinThuNo.LAI_TT = soTienLaiTT;
                    soTienLai -= soTienLaiTT;
                }
                foreach (THONG_TIN_THU_NO objTdtdThongTinThuNo in lstThongTinThuNoTTruoc)
                {
                    decimal soTienLaiTT = Math.Min(objTdtdThongTinThuNo.LAI_KH, soTienLai);
                    objTdtdThongTinThuNo.LAI_TT = soTienLaiTT;
                    soTienLai -= soTienLaiTT;
                }
                LaiTrongHan = lstThongTinThuNo.Where(f => f.NHOM_NO.Equals("part0")).Sum(f => f.LAI_TT);
                LaiQuaHan = lstThongTinThuNo.Where(f => !f.NHOM_NO.Equals("part0")).Sum(f => f.LAI_TT);
                LaiTrongHan += lstThongTinThuNoTTruoc.Where(f => f.NHOM_NO.Equals("part0")).Sum(f => f.LAI_TT);
                LaiQuaHan += lstThongTinThuNoTTruoc.Where(f => !f.NHOM_NO.Equals("part0")).Sum(f => f.LAI_TT);
                TinhToanTienThuaNopTKBB();
                InitEventHanlerProcess();
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                throw;
            }
            InitEventHanlerProcess();
        }

        #endregion

        #region Xu ly nghiep vu
        private void ResetForm()
        {
            radTongGoc.Value = 0;
            radTongLai.Value = 0;
            radTongLaiPhat.Value = 0;
            radTongPhi.Value = 0;
            radDuThuDenNgay.Value = null;
            radLaiDuThu.Value = 0;
            radTongNopCA.Value = 0;
            radTongNopTK.Value = 0;
            _objTTinThuGocLai = new DANH_SACH_KHE_UOC_VONG_VAY();
            obj = new TDVM_THU_GOC_LAI_TRUOC_HAN();
            lstThongTinNopTK = new List<DANH_SACH_SO>();
            lstThongTinRutTK = new List<DANH_SACH_SO>();
            lstThongTinThuNo = new List<THONG_TIN_THU_NO>();
            lstThongTinThuNoTTruoc = new List<THONG_TIN_THU_NO>();
            _objTTinThuGocLai.TAT_TOAN = BusinessConstant.CoKhong.KHONG.layGiaTri();
            _objTTinThuGocLai.TRA_GOC_LAI_TRUOC_HAN = BusinessConstant.CoKhong.KHONG.layGiaTri();
            _objTTinThuGocLai.HOAN_DU_THU = BusinessConstant.CoKhong.KHONG.layGiaTri();
            _objTTinThuGocLai.NGAY_GD = ClientInformation.NgayLamViecHienTai;
            obj.NGAY_GIAO_DICH = ClientInformation.NgayLamViecHienTai;
            obj.NGUOI_GD = ClientInformation.HoTen;
            obj.NGAY_LAP = ClientInformation.NgayLamViecHienTai;
            obj.TRANG_THAI_BAN_GHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
            grMain.DataContext = obj;
            teldtNgayCNhat.Value = null;
            teldtNgayNhap.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai,ApplicationConstant.defaultDateTimeFormat);
            teldtNgayGiaoDich.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);
            action = DatabaseConstant.Action.THEM;
            CommonFunction.RefreshButton(Toolbar, action, TThaiNVu, mnuMain, DatabaseConstant.Function.TDVM_THU_GOC_LAI_TRUOC_HAN);
            SetEnabledAllControls(true);
        }

        void GetDataForm(BusinessConstant.TrangThaiBanGhi bghi, BusinessConstant.TrangThaiNghiepVu nghiepvu)
        {
            try
            {
                List<THONG_TIN_THU_NO> lstThongTinKH = new List<THONG_TIN_THU_NO>();
                lstThongTinKH.AddRange(lstThongTinThuNo);
                lstThongTinKH.AddRange(lstThongTinThuNoTTruoc);
                _objTTinThuGocLai.DSACH_SO_NOP_TIEN = lstThongTinNopTK.ToArray();
                _objTTinThuGocLai.DSACH_SO_RUT_TIEN = lstThongTinRutTK.ToArray();
                _objTTinThuGocLai.DSACH_THONG_TIN_THU_NO = lstThongTinKH.ToArray();
                _objTTinThuGocLai.THUC_THU_GOC_VAY = lstThongTinKH.Where(f => f.NHOM_NO.Equals("part0")).Sum(f => f.GOC_TT) + lstThongTinKH.Where(f => !f.NHOM_NO.Equals("part0")).Sum(f => f.GOC_TT);
                _objTTinThuGocLai.THUC_THU_LAI_TRONG = lstThongTinKH.Sum(f => f.LAI_TT);
                _objTTinThuGocLai.THUC_THU_LAI_QUA_HAN = lstThongTinKH.Where(f => !f.NHOM_NO.Equals("part0")).Sum(f => f.LAI_TT);
                _objTTinThuGocLai.MA_PHI_TRA_TRUOC = _objTTinThuGocLai.BIEU_PHI.MA_BPHI;
                _objTTinThuGocLai.THUC_THU_TIEN_MAT = Convert.ToDecimal(radNumSoTienMat.Value.GetValueOrDefault());
                _objTTinThuGocLai.THUC_THU_TONG = Convert.ToDecimal(radNumSoTienMat.Value.GetValueOrDefault() + radNumSoTienCA.Value.GetValueOrDefault());
                obj.NGAY_LAP_HOA_DON = teldtNgayGiaoDich.Value.GetValueOrDefault().ToString(ApplicationConstant.defaultDateTimeFormat);
                _objTTinThuGocLai.THUC_NOP_TU_TKKKH = lstThongTinRutTK.Sum(f => f.SO_TIEN_RUT_RA);
                if (_objTTinThuGocLai.THUC_NOP_TU_TKKKH > 0)
                    _objTTinThuGocLai.NOP_TIEN_TU_TKKKH = BusinessConstant.CoKhong.CO.layGiaTri();
                _objTTinThuGocLai.THUC_THU_NOP_VAO_TKKKH = lstThongTinNopTK.Sum(f => f.SO_TIEN_NOP_VAO);
                if (_objTTinThuGocLai.THUC_THU_NOP_VAO_TKKKH > 0)
                    _objTTinThuGocLai.NOP_TIEN_VAO_TKKKH = BusinessConstant.CoKhong.CO.layGiaTri();
                _objTTinThuGocLai.MA_DON_VI = ClientInformation.MaDonViGiaoDich;
                List<DANH_SACH_KHE_UOC_VONG_VAY> lstDSach = new List<DANH_SACH_KHE_UOC_VONG_VAY>();
                lstDSach.Add(_objTTinThuGocLai);
                obj.DSACH_KHE_UOC = lstDSach.ToArray();
                obj.LOAI_TIEN = _objTTinThuGocLai.LOAI_TIEN;
                obj.DIEN_GIAI = txtDienGiai.Text;
                if (obj.ID_GIAO_DICH > 0)
                {
                    obj.NGUOI_CAP_NHAT = ClientInformation.TenDangNhap;
                    obj.NGAY_CAP_NHAT = ClientInformation.NgayLamViecHienTai;
                }
                else
                {
                    obj.NGUOI_LAP = ClientInformation.TenDangNhap;
                    obj.NGAY_LAP = ClientInformation.NgayLamViecHienTai;
                    obj.MA_DVI_GD = ClientInformation.MaDonViGiaoDich;
                    obj.MA_DVI = ClientInformation.MaDonVi;
                    
                    obj.TRANG_THAI_BAN_GHI = bghi.layGiaTri();
                    obj.TRANG_THAI_NGHIEP_VU = nghiepvu.layGiaTri();
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        public void SetDataForm()
        {
            try
            {
                List<ClientResponseDetail> lstResponseDetail = new List<ClientResponseDetail>();
                int iret = 0;
                iret = new TinDungProcess().ThuGocLaiVayTruocHan(DatabaseConstant.Action.LOAD_DATA, ref obj, ref lstResponseDetail);
                if (iret > 0)
                {
                    Dispatcher.CurrentDispatcher.DelayInvoke("SetDataForm", () =>
                    {
                        SetTabThongTinChung();
                        if (action.Equals(DatabaseConstant.Action.SUA))
                            SetEnabledAllControls(true);
                        else
                            SetEnabledAllControls(false);
                        Dispatcher.CurrentDispatcher.DelayInvoke("SetTabThongKiemSoat", () =>
                        {
                            SetTabThongKiemSoat();
                        }, TimeSpan.FromSeconds(0));

                    }, TimeSpan.FromSeconds(0));
                }
                
            }
            catch (Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        void SetTabThongTinChung()
        {
            try
            {
                string ngayGDich = obj.NGAY_GIAO_DICH;
                _objTTinThuGocLai = obj.DSACH_KHE_UOC[0];
                lstThongTinThuNo = _objTTinThuGocLai.DSACH_THONG_TIN_THU_NO.Where(f => f.NGAY_KH.CompareTo(ngayGDich) <= 0).ToList();
                lstThongTinThuNoTTruoc = _objTTinThuGocLai.DSACH_THONG_TIN_THU_NO.Where(f => f.NGAY_KH.CompareTo(ngayGDich) > 0).ToList();
                lstThongTinNopTK = _objTTinThuGocLai.DSACH_SO_NOP_TIEN.ToList();
                lstThongTinRutTK = _objTTinThuGocLai.DSACH_SO_RUT_TIEN.ToList();
                GocQuaHan = lstThongTinThuNo.Where(f => !f.NHOM_NO.Equals("part0")).Sum(f => f.GOC_TT) + lstThongTinThuNoTTruoc.Where(f => !f.NHOM_NO.Equals("part0")).Sum(f => f.GOC_TT);
                GocTrongHan = lstThongTinThuNo.Where(f => f.NHOM_NO.Equals("part0")).Sum(f => f.GOC_TT) + lstThongTinThuNoTTruoc.Where(f => f.NHOM_NO.Equals("part0")).Sum(f => f.GOC_TT);
                LaiTrongHan = lstThongTinThuNo.Where(f => f.NHOM_NO.Equals("part0")).Sum(f => f.LAI_TT) + lstThongTinThuNoTTruoc.Where(f => f.NHOM_NO.Equals("part0")).Sum(f => f.LAI_TT);
                LaiQuaHan = lstThongTinThuNo.Where(f => !f.NHOM_NO.Equals("part0")).Sum(f => f.LAI_TT) + lstThongTinThuNoTTruoc.Where(f => !f.NHOM_NO.Equals("part0")).Sum(f => f.LAI_TT); ;

                teldtNgayGiaoDich.Value = LDateTime.StringToDate(obj.NGAY_GIAO_DICH, ApplicationConstant.defaultDateTimeFormat);
                txtDienGiai.Text = obj.DIEN_GIAI;
                txtSoGiaoDich.Text = obj.MA_GIAO_DICH;
                txtTenKHang.Text = _objTTinThuGocLai.TEN_KHACH_HANG;
                txtSoKheUoc.Text = _objTTinThuGocLai.SO_KHE_UOC;
                txtSoHDTD.Text = _objTTinThuGocLai.SO_HDTD;
                txtMaKHang.Text = _objTTinThuGocLai.MA_KHACH_HANG;
                txtSoDu.Value = Convert.ToDouble(_objTTinThuGocLai.DU_NO);
                txtLoaiTien.SelectedIndex = lstSourceTienTe.IndexOf(lstSourceTienTe.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(_objTTinThuGocLai.LOAI_TIEN)));
                radTongGoc.Value = (double)(GocTrongHan + GocQuaHan);
                radTongLai.Value = (double)(LaiTrongHan + LaiQuaHan);
                radNumSoTienMat.Value = (double)_objTTinThuGocLai.THUC_THU_TIEN_MAT;
                radNumSoTienGiaoDich.Value = (double)_objTTinThuGocLai.THUC_THU_TONG;
                radNumSoTienCA.Value = (double)_objTTinThuGocLai.THUC_NOP_TU_TKKKH;
                radTongLaiPhat.Value = (double)_objTTinThuGocLai.LAI_PHAT;
                if (!lstThongTinNopTK.Where(f => f.LOAI_SAN_PHAM_HDV.Equals("T08")).IsNullOrEmpty())
                    radTongNopCA.Value = (double)lstThongTinNopTK.Where(f => f.LOAI_SAN_PHAM_HDV.Equals("T08")).Sum(f => f.SO_TIEN_NOP_VAO);
                if (!lstThongTinNopTK.Where(f => f.LOAI_SAN_PHAM_HDV.Equals("T08")).IsNullOrEmpty())
                    radTongNopTK.Value = (double)lstThongTinNopTK.Where(f => !f.LOAI_SAN_PHAM_HDV.Equals("T08")).Sum(f => f.SO_TIEN_NOP_VAO);
                radTongPhi.Value = (double)_objTTinThuGocLai.PHI_TRA_TRUOC;
                if (!_objTTinThuGocLai.NGAY_DU_THU.IsNullOrEmptyOrSpace())
                    radDuThuDenNgay.Value = LDateTime.StringToDate(_objTTinThuGocLai.NGAY_DU_THU, ApplicationConstant.defaultDateTimeFormat);
                else
                    radDuThuDenNgay.Value = null;
                radLaiDuThu.Value = (double)(_objTTinThuGocLai.SO_TIEN_DU_THU + _objTTinThuGocLai.SO_TIEN_DU_THU_QH);
                if (!lstThongTinNopTK.Where(f => f.LOAI_SAN_PHAM_HDV.Equals("T08")).IsNullOrEmpty())
                    radTongNopCA.IsEnabled = true;
                else
                    radTongNopCA.IsEnabled = false;
                if (!lstThongTinNopTK.Where(f => !f.LOAI_SAN_PHAM_HDV.Equals("T08")).IsNullOrEmpty())
                    radTongNopTK.IsEnabled = true;
                else
                    radTongNopTK.IsEnabled = false;
                if (lstThongTinRutTK.Count == 0)
                    radNumSoTienCA.IsEnabled = false;
                else
                    radNumSoTienCA.IsEnabled = true;
                lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(obj.TRANG_THAI_NGHIEP_VU);
                grMain.DataContext = obj;
                TThaiNVu = obj.TRANG_THAI_NGHIEP_VU;
                if (!_objTTinThuGocLai.BIEU_PHI.IsNullOrEmpty() && !_objTTinThuGocLai.BIEU_PHI.DSACH_BPHI_CT.IsNullOrEmpty())
                    lstBieuPhi = _objTTinThuGocLai.BIEU_PHI.DSACH_BPHI_CT.ToList();

                CommonFunction.RefreshButton(Toolbar, action, TThaiNVu, mnuMain, DatabaseConstant.Function.TDVM_THU_GOC_LAI_TRUOC_HAN);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        void SetTabThongKiemSoat()
        {
            try
            {
                if (!LObject.IsNullOrEmpty(obj))
                {
                    txtNguoiLap.Text = obj.NGUOI_LAP;
                    teldtNgayNhap.Value = LDateTime.StringToDate(obj.NGAY_LAP, ApplicationConstant.defaultDateTimeFormat);
                    txtTrangThai.Text = BusinessConstant.layNgonNguSuDung(obj.TRANG_THAI_BAN_GHI);
                    txtNguoiCapNhat.Text = obj.NGUOI_CAP_NHAT;
                    if (!obj.NGAY_CAP_NHAT.IsNullOrEmptyOrSpace())
                        teldtNgayCNhat.Value = LDateTime.StringToDate(obj.NGAY_CAP_NHAT, ApplicationConstant.defaultDateTimeFormat);
                    else
                        teldtNgayCNhat.Value = null;

                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        void beforeModify()
        {
            action = DatabaseConstant.Action.SUA;
            SetEnabledAllControls(true);
            OnModify();
        }

        private void SetEnabledAllControls(bool enable)
        {
            txtDienGiai.IsEnabled = enable;
            txtSoKheUoc.IsEnabled = enable;
            radNumSoTienGiaoDich.IsEnabled = enable;
            chkHoanDuThu.IsEnabled = enable;
            chkTatToan.IsEnabled = enable;
            radNumSoTienMat.IsEnabled = enable;
            radNumSoTienCA.IsEnabled = enable;
            radTongLaiPhat.IsEnabled = enable;
            radTongPhi.IsEnabled = enable;
            radTongNopCA.IsEnabled = enable;
            radTongNopTK.IsEnabled = enable;

        }

        private void Release()
        {
            // Yêu cầu Unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
            DatabaseConstant.Function.TDVM_THU_GOC_LAI_TRUOC_HAN,
            DatabaseConstant.Table.KT_GIAO_DICH,
            DatabaseConstant.Action.SUA,
            lstId);
        }

        void OnModify()
        {
            UtilitiesProcess process = new UtilitiesProcess();
            lstId = new List<int>();
            lstId.Add(iDGDich);
            bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
            DatabaseConstant.Function.TDVM_THU_GOC_LAI_TRUOC_HAN,
            DatabaseConstant.Table.KT_GIAO_DICH,
            action,
            lstId);
            SetEnabledAllControls(true);
            CommonFunction.RefreshButton(Toolbar, action, TThaiNVu, mnuMain, DatabaseConstant.Function.TDVM_THU_GOC_LAI_TRUOC_HAN);
        }

        bool Validation()
        {
            bool bReturn = true;
            if (_objTTinThuGocLai.IsNullOrEmpty() || _objTTinThuGocLai.ID_KHE_UOC <= 0)
            {
                CommonFunction.ThongBaoTrong(lblSoKheUoc.Content.ToString());
                txtSoKheUoc.Focus();
                return false;
            }
            if (txtDienGiai.Text.IsNullOrEmptyOrSpace())
            {
                CommonFunction.ThongBaoTrong(lblDienGiai.Content.ToString());
                txtDienGiai.Focus();
                return false;
            }
            double tongGiaoDich = radNumSoTienGiaoDich.Value.GetValueOrDefault();
            double tongTienMat = radNumSoTienMat.Value.GetValueOrDefault();
            double tongCA = radNumSoTienCA.Value.GetValueOrDefault();
            double tongGoc = radTongGoc.Value.GetValueOrDefault();
            double tongLai = radTongLai.Value.GetValueOrDefault();
            double tongLaiPhat = radTongLaiPhat.Value.GetValueOrDefault();
            double tongPhi = radTongPhi.Value.GetValueOrDefault();
            double tongNopCA = radTongNopCA.Value.GetValueOrDefault();
            double tongNopTK = radTongNopTK.Value.GetValueOrDefault();
            if ((tongGiaoDich != tongTienMat + tongCA) || (tongTienMat + tongCA != tongGoc + tongLai + tongLaiPhat + tongPhi + tongNopCA + tongNopTK))
            {
                LMessage.ShowMessage("M_ResponseMessage_SoTien_KhongHopLe", LMessage.MessageBoxType.Warning);
                radNumSoTienGiaoDich.Focus();
                return false;
            }
            return bReturn;
        }
        void BeforeSave(BusinessConstant.TrangThaiNghiepVu trangthai, BusinessConstant.TrangThaiBanGhi banghi)
        {
            try
            {
                Cursor = Cursors.Wait;
                if (!trangthai.Equals(BusinessConstant.TrangThaiNghiepVu.LUU_TAM))
                {
                    if (!Validation())
                        return;
                }
                GetDataForm(banghi, trangthai);
                OnSave();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                Cursor = Cursors.Arrow;
            }
        }

        void OnSave()
        {

            List<ClientResponseDetail> lstResponseDetail = new List<ClientResponseDetail>();
            int iret = 0;
            if (txtSoGiaoDich.Text == "")
                iret = new TinDungProcess().ThuGocLaiVayTruocHan(DatabaseConstant.Action.THEM, ref obj, ref lstResponseDetail);
            else
                iret = new TinDungProcess().ThuGocLaiVayTruocHan(DatabaseConstant.Action.SUA, ref obj, ref lstResponseDetail);
            AfterSave(lstResponseDetail, iret);
        }

        void AfterSave(List<ClientResponseDetail> lstResponseDetail, int iret)
        {
            CommonFunction.ThongBaoKetQua(lstResponseDetail);
            if (iret > 0)
            {
                action = DatabaseConstant.Action.XEM;
                SetInfomation();
                // Yêu cầu Unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_THU_GOC_LAI_TRUOC_HAN,
                DatabaseConstant.Table.KT_GIAO_DICH,
                DatabaseConstant.Action.SUA,
                lstId);
                if (OnSavingCompleted != null)
                    OnSavingCompleted(null, EventArgs.Empty);
                if ((bool)cbMultiAdd.IsChecked)
                    ResetForm();
            }

        }

        void AfterDelete(string sSoGiaoDich, List<ClientResponseDetail> ResponseDetail, int iret)
        {
            CommonFunction.ThongBaoKetQua(ResponseDetail);
            // Yêu cầu Unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
            DatabaseConstant.Function.TDVM_THU_GOC_LAI_TRUOC_HAN,
            DatabaseConstant.Table.KT_GIAO_DICH,
            DatabaseConstant.Action.XOA,
            lstId);
            if (OnSavingCompleted != null)
                OnSavingCompleted(null, EventArgs.Empty);
            if (iret < 1)
                SetInfomation();
            else
                CommonFunction.CloseUserControl(this);
        }

        void OnDelete()
        {
            int iret = 0;
            List<ClientResponseDetail> ResponseDetail = new List<ClientResponseDetail>();
            obj.NGUOI_CAP_NHAT = ClientInformation.TenDangNhap;
            obj.NGAY_CAP_NHAT = ClientInformation.NgayLamViecHienTai;
            iret = new TinDungProcess().ThuGocLaiVayTruocHan(DatabaseConstant.Action.XOA, ref obj, ref ResponseDetail);
            AfterDelete(txtSoGiaoDich.Text, ResponseDetail, iret);
        }

        void BeforeDelete()
        {
            if (tlbDelete.IsEnabled == false)
                return;
            try
            {
                Cursor = Cursors.Wait;
                if (!txtSoGiaoDich.Text.IsNullOrEmptyOrSpace())
                {
                    if (LMessage.ShowMessage("M.DungChung.HoiXoa", LMessage.MessageBoxType.Question) == MessageBoxResult.Yes)
                    {

                        lstId = new List<int>();
                        lstId.Add(iDGDich);

                        // Yêu cầu lock dữ liệu
                        UtilitiesProcess process = new UtilitiesProcess();
                        bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                        DatabaseConstant.Function.TDVM_THU_GOC_LAI_TRUOC_HAN,
                        DatabaseConstant.Table.KT_GIAO_DICH,
                        DatabaseConstant.Action.XOA,
                        lstId);
                        OnDelete();
                    }
                }
            }
            catch (Exception ex)
            {
                // Yêu cầu Unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_THU_GOC_LAI_TRUOC_HAN,
                DatabaseConstant.Table.KT_GIAO_DICH,
                DatabaseConstant.Action.XOA,
                lstId);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                Cursor = Cursors.Arrow;
            }
        }

        void AfterApprove(string sSoGiaoDich, List<ClientResponseDetail> ResponseDetail)
        {
            CommonFunction.ThongBaoKetQua(ResponseDetail);
            // Yêu cầu Unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
            DatabaseConstant.Function.TDVM_THU_GOC_LAI_TRUOC_HAN,
            DatabaseConstant.Table.KT_GIAO_DICH,
            DatabaseConstant.Action.DUYET,
            lstId);
            action = DatabaseConstant.Action.XEM;
            if (OnSavingCompleted != null)
                OnSavingCompleted(null, EventArgs.Empty);
            SetDataForm();
        }

        void OnApprove()
        {
            int iret = 0;
            List<ClientResponseDetail> ResponseDetail = new List<ClientResponseDetail>();
            obj.NGUOI_CAP_NHAT = ClientInformation.TenDangNhap;
            obj.NGAY_CAP_NHAT = ClientInformation.NgayLamViecHienTai;
            iret = new TinDungProcess().ThuGocLaiVayTruocHan(DatabaseConstant.Action.DUYET, ref obj, ref ResponseDetail);
            AfterApprove(txtSoGiaoDich.Text, ResponseDetail);
        }

        void BeforeApprove()
        {
            if (!tlbApprove.IsEnabled)
                return;
            try
            {
                if (!txtSoGiaoDich.Text.IsNullOrEmptyOrSpace())
                {
                    if (LMessage.ShowMessage("M.DungChung.HoiDuyet", LMessage.MessageBoxType.Question) == MessageBoxResult.Yes)
                    {
                        lstId = new List<int>();
                        lstId.Add(iDGDich);

                        // Yêu cầu lock dữ liệu
                        UtilitiesProcess process = new UtilitiesProcess();
                        bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                        DatabaseConstant.Function.TDVM_THU_GOC_LAI_TRUOC_HAN,
                        DatabaseConstant.Table.KT_GIAO_DICH,
                        DatabaseConstant.Action.DUYET,
                        lstId);
                        OnApprove();
                    }
                }
            }
            catch (Exception ex)
            {
                // Yêu cầu Unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_THU_GOC_LAI_TRUOC_HAN,
                DatabaseConstant.Table.KT_GIAO_DICH,
                DatabaseConstant.Action.DUYET,
                lstId);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        void AfterRefuse(string sSoGiaoDich, List<ClientResponseDetail> ResponseDetail)
        {
            CommonFunction.ThongBaoKetQua(ResponseDetail);
            // Yêu cầu Unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
            DatabaseConstant.Function.TDVM_THU_GOC_LAI_TRUOC_HAN,
            DatabaseConstant.Table.KT_GIAO_DICH,
            DatabaseConstant.Action.TU_CHOI_DUYET,
            lstId);
            action = DatabaseConstant.Action.XEM;
            if (OnSavingCompleted != null)
                OnSavingCompleted(null, EventArgs.Empty);
            SetDataForm();
        }

        void OnRefuse()
        {
            int iret = 0;
            List<ClientResponseDetail> ResponseDetail = new List<ClientResponseDetail>();
            obj.NGUOI_CAP_NHAT = ClientInformation.TenDangNhap;
            obj.NGAY_CAP_NHAT = ClientInformation.NgayLamViecHienTai;
            iret = new TinDungProcess().ThuGocLaiVayTruocHan(DatabaseConstant.Action.TU_CHOI_DUYET, ref obj, ref ResponseDetail);
            AfterRefuse(txtSoGiaoDich.Text, ResponseDetail);
        }

        void BeforeRefuse()
        {
            if (!tlbRefuse.IsEnabled)
                return;
            try
            {
                if (!txtSoGiaoDich.Text.IsNullOrEmptyOrSpace())
                {
                    if (LMessage.ShowMessage("M.DungChung.HoiTuChoiDuyet", LMessage.MessageBoxType.Question) == MessageBoxResult.Yes)
                    {

                        lstId = new List<int>();
                        lstId.Add(iDGDich);

                        // Yêu cầu lock dữ liệu
                        UtilitiesProcess process = new UtilitiesProcess();
                        bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                        DatabaseConstant.Function.TDVM_THU_GOC_LAI_TRUOC_HAN,
                        DatabaseConstant.Table.KT_GIAO_DICH,
                        DatabaseConstant.Action.TU_CHOI_DUYET,
                        lstId);
                        OnRefuse();
                    }
                }
            }
            catch (Exception ex)
            {
                // Yêu cầu Unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_THU_GOC_LAI_TRUOC_HAN,
                DatabaseConstant.Table.KT_GIAO_DICH,
                DatabaseConstant.Action.TU_CHOI_DUYET,
                lstId);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        void AfterCancel(string sSoGiaoDich, List<ClientResponseDetail> ResponseDetail)
        {
            CommonFunction.ThongBaoKetQua(ResponseDetail);
            // Yêu cầu Unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
            DatabaseConstant.Function.TDVM_THU_GOC_LAI_TRUOC_HAN,
            DatabaseConstant.Table.KT_GIAO_DICH,
            DatabaseConstant.Action.THOAI_DUYET,
            lstId);
            action = DatabaseConstant.Action.XEM;
            if (OnSavingCompleted != null)
                OnSavingCompleted(null, EventArgs.Empty);
            SetDataForm();
        }

        void OnCancel()
        {
            int iret = 0;
            List<ClientResponseDetail> ResponseDetail = new List<ClientResponseDetail>();
            obj.NGUOI_CAP_NHAT = ClientInformation.TenDangNhap;
            obj.NGAY_CAP_NHAT = ClientInformation.NgayLamViecHienTai;
            iret = new TinDungProcess().ThuGocLaiVayTruocHan(DatabaseConstant.Action.THOAI_DUYET, ref obj, ref ResponseDetail);
            AfterCancel(txtSoGiaoDich.Text, ResponseDetail);
        }

        void BeforeCancel()
        {
            if (!tlbCancel.IsEnabled)
                return;
            try
            {
                if (!txtSoGiaoDich.Text.IsNullOrEmptyOrSpace())
                {
                    if (LMessage.ShowMessage("M.DungChung.HoiThoaiDuyet", LMessage.MessageBoxType.Question) == MessageBoxResult.Yes)
                    {

                        lstId = new List<int>();
                        lstId.Add(iDGDich);

                        // Yêu cầu lock dữ liệu
                        UtilitiesProcess process = new UtilitiesProcess();
                        bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                        DatabaseConstant.Function.TDVM_THU_GOC_LAI_TRUOC_HAN,
                        DatabaseConstant.Table.KT_GIAO_DICH,
                        DatabaseConstant.Action.THOAI_DUYET,
                        lstId);
                        OnCancel();
                    }
                }
            }
            catch (Exception ex)
            {
                // Yêu cầu Unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_THU_GOC_LAI_TRUOC_HAN,
                DatabaseConstant.Table.KT_GIAO_DICH,
                DatabaseConstant.Action.THOAI_DUYET,
                lstId);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void OnPrint()
        {

        }

        private void OnPreviewDonVayVon()
        {

        }

        /// <summary>
        /// Xem báo cáo
        /// </summary>
        private void OnPreview()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            // Cảnh báo khi không có dữ liệu
            if (LObject.IsNullOrEmpty(txtSoGiaoDich.Text))
            {
                LMessage.ShowMessage("Không có thông tin dữ liệu cần hiển thị", LMessage.MessageBoxType.Warning);

            }
            else
            {

                VanHanhGiaoDich xemBaoCao = new VanHanhGiaoDich();
                DoiTuongBaoCao doiTuongBaoCao = new DoiTuongBaoCao();

                GIAO_DICH_BASE objGIAO_DICH_BASE = new GIAO_DICH_BASE();
                objGIAO_DICH_BASE.BaoCao = DatabaseConstant.DanhSachBaoCaoTheoGiaoDich.GDKT_IN_GIAO_DICH;
                objGIAO_DICH_BASE.ChucNang = DatabaseConstant.Function.TDVM_THU_GOC_LAI_TRUOC_HAN;

                GDKT_GIAO_DICH objGDKT_GIAO_DICH = new GDKT_GIAO_DICH();
                objGDKT_GIAO_DICH.MaGiaoDich = txtSoGiaoDich.Text;

                doiTuongBaoCao.objGIAO_DICH_BASE = objGIAO_DICH_BASE;
                doiTuongBaoCao.objGDKT_GIAO_DICH = objGDKT_GIAO_DICH;

                List<ThamSoBaoCao> listThamSoBaoCao = new List<ThamSoBaoCao>();
                listThamSoBaoCao.Add(new ThamSoBaoCao("@SoPhieu", txtSoGiaoDich.Text, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                listThamSoBaoCao.Add(new ThamSoBaoCao("MaNgonNgu", ClientInformation.NgonNgu, ApplicationConstant.LoaiThamSoBaoCao.LANG.layGiaTri()));
                listThamSoBaoCao.Add(new ThamSoBaoCao("MaDinhDang", ApplicationConstant.LoaiDinhDangBaoCao.WORD.layGiaTri(), ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri()));

                string maBaoCao = "GDKT_GIAO_DICH";
                xemBaoCao.GiaoDichPhatSinh(ref doiTuongBaoCao);
                
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// Lấy danh sách được chọn
        /// </summary>
        /// <returns></returns>
        private List<DataRow> getListSeletedDataRow()
        {
            List<DataRow> listDataRow = new List<DataRow>();
            return listDataRow;
        }

        private void SetInfomation()
        {
            try
            {
                lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(obj.TRANG_THAI_NGHIEP_VU);
                action = DatabaseConstant.Action.XEM;
                TThaiNVu = obj.TRANG_THAI_NGHIEP_VU;
                txtSoGiaoDich.Text = obj.MA_GIAO_DICH;
                CommonFunction.RefreshButton(Toolbar, action, TThaiNVu, mnuMain, DatabaseConstant.Function.TDVM_THU_GOC_LAI_TRUOC_HAN);
                SetEnabledAllControls(false);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void TinhToanTraGocLai()
        {
            SoTienMat = Convert.ToDecimal(radNumSoTienMat.Value.GetValueOrDefault());
            SoTienNopCATK = Convert.ToDecimal(radNumSoTienCA.Value.GetValueOrDefault());
            SoTienGocLaiTruoc = 0;
            LaiPhat = 0;
            decimal TongTienGD = SoTienMat + SoTienNopCATK;
            lstThongTinThuNo.ForEach(f => { f.GOC_TT = 0; f.LAI_TT = 0; });
            lstThongTinThuNoTTruoc.ForEach(f => { f.GOC_TT = 0; f.LAI_TT = 0; });
            for (int i = 0; i < lstThongTinThuNo.Count; i++)
            {
                //LaiPhat += lstThongTinThuNo[i].LAI_PHAT_KH;
                decimal soTienTraLai = lstThongTinThuNo[i].LAI_KH;
                decimal soTienTraGoc = lstThongTinThuNo[i].GOC_KH;
                decimal soTienLaiPhat = lstThongTinThuNo[i].LAI_PHAT_KH;
                foreach (string loaiPhanBo in _thuTuPhanBo.Split('#'))
                {
                    switch (loaiPhanBo)
                    {
                        case "GOCVAY":
                            if (TongTienGD <= soTienTraGoc)
                            {
                                lstThongTinThuNo[i].GOC_TT = TongTienGD;
                            }
                            else
                            {
                                lstThongTinThuNo[i].GOC_TT = soTienTraGoc;

                            }
                            TongTienGD -= Convert.ToDecimal(lstThongTinThuNo[i].GOC_TT);
                            break;
                        case "LAI_VAY":
                            if (TongTienGD <= soTienTraLai)
                            {
                                lstThongTinThuNo[i].LAI_TT = TongTienGD;
                            }
                            else
                            {
                                lstThongTinThuNo[i].LAI_TT = soTienTraLai;

                            }
                            TongTienGD -= Convert.ToDecimal(lstThongTinThuNo[i].LAI_TT);
                            break;
                        case "LAI_QHAN":
                            if (TongTienGD <= soTienLaiPhat)
                            {
                                lstThongTinThuNo[i].LAI_PHAT_TT = TongTienGD;
                            }
                            else
                            {
                                lstThongTinThuNo[i].LAI_PHAT_TT = soTienLaiPhat;

                            }
                            TongTienGD -= Convert.ToDecimal(lstThongTinThuNo[i].LAI_PHAT_TT);
                            break;
                        case "TKBB":

                            break;
                    }
                    if (TongTienGD == 0)
                        break;
                }
                if (TongTienGD == 0)
                    break;
            }
            GocTrongHan = lstThongTinThuNo.Where(f => f.NHOM_NO.Equals("part0")).Sum(f => f.GOC_TT);
            GocQuaHan = lstThongTinThuNo.Where(f => !f.NHOM_NO.Equals("part0")).Sum(f => f.GOC_TT);
            LaiTrongHan = lstThongTinThuNo.Where(f => f.NHOM_NO.Equals("part0")).Sum(f => f.LAI_TT);
            LaiQuaHan = lstThongTinThuNo.Where(f => !f.NHOM_NO.Equals("part0")).Sum(f => f.LAI_TT);
            LaiPhat = lstThongTinThuNo.Sum(f => f.LAI_PHAT_TT);
            radTongGoc.Value = (double)(GocTrongHan + GocQuaHan);
            radTongLai.Value = (double)(LaiTrongHan + LaiQuaHan);
            _objTTinThuGocLai.LAI_PHAT = LaiPhat;
            //if (TongTienGD > 0)
            //{
            //    LaiPhat = Math.Min(_objTTinThuGocLai.LAI_PHAT, TongTienGD);
            //    TongTienGD -= Math.Min(_objTTinThuGocLai.LAI_PHAT, TongTienGD);
            //    radTongLaiPhat.Value = (double)LaiPhat;
            //    radTongLaiPhat.IsEnabled = true;
            //    if(TongTienGD == 0)
            //        radTongPhi.IsEnabled = false;
            //    else
            //        radTongPhi.IsEnabled = true;
            //}
            //else
            //{
            //    LaiPhat = 0;
            //    radTongLaiPhat.Value = (double)LaiPhat;
            //    radTongLaiPhat.IsEnabled = false;
            //}
            radTongLaiPhat.Value = (double)LaiPhat;
            _objTTinThuGocLai.PHI_TRA_TRUOC = 0;
            radTongPhi.Value = 0;
            SoTienPhi = 0;
            TinhToanTienThuaNopTKBB();
        }

        private void TinhToanTienThuaNopTKBB()
        {
            SoTienMat = Convert.ToDecimal(radNumSoTienMat.Value.GetValueOrDefault());
            SoTienNopCATK = Convert.ToDecimal(radNumSoTienCA.Value.GetValueOrDefault());
            decimal TongTienGD = SoTienMat + SoTienNopCATK - GocTrongHan - GocQuaHan - LaiTrongHan - LaiQuaHan - SoTienGocLaiTruoc - SoTienPhi - LaiPhat;
            lstThongTinNopTK.ForEach(f => f.SO_TIEN_NOP_VAO = 0);
            
            if (TongTienGD <= 0)
            {
                TongTienGD = 0;
                radTongNopCA.Value = (double)lstThongTinNopTK.Where(f => f.LOAI_SAN_PHAM_HDV.Equals("T08")).Sum(f => f.SO_TIEN_NOP_VAO);
                radTongNopTK.Value = (double)lstThongTinNopTK.Where(f => !f.LOAI_SAN_PHAM_HDV.Equals("T08")).Sum(f => f.SO_TIEN_NOP_VAO);
                SoTienNopThua = TongTienGD;
                return;
            }
            SoTienNopThua = TongTienGD;
            lstThongTinNopTK.FirstOrDefault().SO_TIEN_NOP_VAO = SoTienNopThua;
            radTongNopCA.Value = (double)lstThongTinNopTK.Where(f => f.LOAI_SAN_PHAM_HDV.Equals("T08")).Sum(f => f.SO_TIEN_NOP_VAO);
            radTongNopTK.Value = (double)lstThongTinNopTK.Where(f => !f.LOAI_SAN_PHAM_HDV.Equals("T08")).Sum(f => f.SO_TIEN_NOP_VAO);
        }

        void GetKeHoachThuGocLai()
        {
            try
            {
                DataSet ds = new DataSet();
                DataTable dtPar = null;
                LDatatable.MakeParameterTable(ref dtPar);
                LDatatable.AddParameter(ref dtPar, "@ID_KUOC", "String", _objTTinThuGocLai.ID_KHE_UOC.ToString());
                LDatatable.AddParameter(ref dtPar, "@MA_KUOC", "String", _objTTinThuGocLai.MA_KHE_UOC.ToString());
                LDatatable.AddParameter(ref dtPar, "@NGAY_GDICH", "String", ClientInformation.NgayLamViecHienTai);
                ds = new TinDungProcess().GetThongTinKeHoachThuGocLai(dtPar);
                lstThongTinThuNo = new List<THONG_TIN_THU_NO>();
                lstThongTinThuNoTTruoc = new List<THONG_TIN_THU_NO>();
                lstThongTinNopTK = new List<DANH_SACH_SO>();
                lstThongTinRutTK = new List<DANH_SACH_SO>();
                if (!ds.IsNullOrEmpty() && !ds.Tables.IsNullOrEmpty() && ds.Tables.Count > 0)
                {
                    DataTable dt = ds.Tables[0];
                    foreach (DataRow dr in dt.Rows)
                    {
                        objTTinThuNo = new THONG_TIN_THU_NO();
                        objTTinThuNo.GOC_KH = Convert.ToDecimal(dr["TRA_GOC_KH"]);
                        objTTinThuNo.LAI_KH = Convert.ToDecimal(dr["TRA_LAI_KH"]);
                        objTTinThuNo.LAI_PHAT_KH = Convert.ToDecimal(dr["LAI_PHAT_KH"]);
                        objTTinThuNo.GOC_TT = Convert.ToDecimal(dr["TRA_GOC_TT"]);
                        objTTinThuNo.LAI_TT = Convert.ToDecimal(dr["TRA_LAI_TT"]);
                        objTTinThuNo.LAI_PHAT_TT = Convert.ToDecimal(dr["LAI_PHAT_TT"]);
                        objTTinThuNo.NGAY_KH = Convert.ToString(dr["NGAY_KH"]);
                        objTTinThuNo.ID_KUOC = Convert.ToInt32(dr["ID_KUOC"]);
                        objTTinThuNo.MA_KUOC = Convert.ToString(dr["MA_KUOC"]);
                        objTTinThuNo.NHOM_NO = Convert.ToString(dr["TTHAI_NO"]);
                        objTTinThuNo.SO_NGAY_QH = Convert.ToInt32(dr["SO_NGAY_QH"]);
                        lstThongTinThuNo.Add(objTTinThuNo);
                    }
                    dt = ds.Tables[1];
                    foreach (DataRow dr in dt.Rows)
                    {
                        objTTinThuNo = new THONG_TIN_THU_NO();
                        objTTinThuNo.GOC_KH = Convert.ToDecimal(dr["TRA_GOC_KH"]);
                        objTTinThuNo.LAI_KH = Convert.ToDecimal(dr["TRA_LAI_KH"]);
                        objTTinThuNo.LAI_PHAT_KH = Convert.ToDecimal(dr["LAI_PHAT_KH"]);
                        objTTinThuNo.GOC_TT = Convert.ToDecimal(dr["TRA_GOC_TT"]);
                        objTTinThuNo.LAI_TT = Convert.ToDecimal(dr["TRA_LAI_TT"]);
                        objTTinThuNo.LAI_PHAT_TT = Convert.ToDecimal(dr["LAI_PHAT_TT"]);
                        objTTinThuNo.NGAY_KH = Convert.ToString(dr["NGAY_KH"]);
                        objTTinThuNo.ID_KUOC = Convert.ToInt32(dr["ID_KUOC"]);
                        objTTinThuNo.MA_KUOC = Convert.ToString(dr["MA_KUOC"]);
                        objTTinThuNo.NHOM_NO = Convert.ToString(dr["TTHAI_NO"]);
                        objTTinThuNo.SO_NGAY_QH = Convert.ToInt32(dr["SO_NGAY_QH"]);
                        lstThongTinThuNoTTruoc.Add(objTTinThuNo);
                    }

                    dt = ds.Tables[2];
                    foreach (DataRow dr in dt.Rows)
                    {
                        DANH_SACH_SO objTTinTK = new DANH_SACH_SO();
                        objTTinTK.ID_NHOM = Convert.ToInt32(dr["ID_NHOM"]);
                        objTTinTK.LOAI_SAN_PHAM_HDV = Convert.ToString(dr["LOAI_SO"]);
                        objTTinTK.MA_KHANG = Convert.ToString(dr["MA_KHANG"]);
                        objTTinTK.SO_DU = Convert.ToDecimal(dr["SO_TIEN"]);
                        objTTinTK.SO_SO = Convert.ToString(dr["SO_SO_TK"]);
                        objTTinTK.TEN_KHANG = Convert.ToString(dr["TEN_KHANG"]);
                        objTTinTK.SO_TIEN_NOP_VAO = 0;
                        objTTinTK.SO_TIEN_RUT_RA = 0;
                        lstThongTinRutTK.Add(objTTinTK);
                    }

                    dt = ds.Tables[3];
                    foreach (DataRow dr in dt.Rows)
                    {
                        DANH_SACH_SO objTTinTK = new DANH_SACH_SO();
                        objTTinTK.ID_NHOM = Convert.ToInt32(dr["ID_NHOM"]);
                        objTTinTK.LOAI_SAN_PHAM_HDV = Convert.ToString(dr["LOAI_SO"]);
                        objTTinTK.MA_KHANG = Convert.ToString(dr["MA_KHANG"]);
                        objTTinTK.SO_DU = Convert.ToDecimal(dr["SO_TIEN"]);
                        objTTinTK.SO_SO = Convert.ToString(dr["SO_SO_TK"]);
                        objTTinTK.TEN_KHANG = Convert.ToString(dr["TEN_KHANG"]);
                        objTTinTK.SO_TIEN_NOP_VAO = 0;
                        objTTinTK.SO_TIEN_RUT_RA = 0;
                        lstThongTinNopTK.Add(objTTinTK);
                    }
                    decimal soTienGDich = lstThongTinThuNo.Sum(f => f.LAI_TT + f.GOC_TT + f.LAI_PHAT_TT);
                    radNumSoTienGiaoDich.Value = (double)soTienGDich;
                    radNumSoTienMat.Value = radNumSoTienGiaoDich.Value;
                    TinhToanTraGocLai();
                    radTongGoc.Value = (double)(GocTrongHan + GocQuaHan);
                    radTongLai.Value = (double)(LaiTrongHan + LaiQuaHan);
                    radNumSoTienCA.Value = 0;
                    radTongLaiPhat.Value = (double)_objTTinThuGocLai.LAI_PHAT;
                    radTongNopCA.Value = 0;
                    radTongNopTK.Value = 0;
                    radTongPhi.Value = 0;
                    if (!_objTTinThuGocLai.NGAY_DU_THU.IsNullOrEmptyOrSpace())
                        radDuThuDenNgay.Value = LDateTime.StringToDate(_objTTinThuGocLai.NGAY_DU_THU, ApplicationConstant.defaultDateTimeFormat);
                    else
                        radDuThuDenNgay.Value = null;
                    radLaiDuThu.Value = (double)(_objTTinThuGocLai.SO_TIEN_DU_THU + _objTTinThuGocLai.SO_TIEN_DU_THU_QH);
                    if (!lstThongTinNopTK.Where(f => f.LOAI_SAN_PHAM_HDV.Equals("T08")).IsNullOrEmpty())
                        radTongNopCA.IsEnabled = true;
                    else
                        radTongNopCA.IsEnabled = false;
                    if (!lstThongTinNopTK.Where(f => !f.LOAI_SAN_PHAM_HDV.Equals("T08")).IsNullOrEmpty())
                        radTongNopTK.IsEnabled = true;
                    else
                        radTongNopTK.IsEnabled = false;
                    if (lstThongTinRutTK.Count == 0)
                        radNumSoTienCA.IsEnabled = false;
                    else
                        radNumSoTienCA.IsEnabled = true;
                    List<THONG_TIN_THU_NO> lstThongTinKH = new List<THONG_TIN_THU_NO>();
                    lstThongTinKH.AddRange(lstThongTinThuNo);
                    lstThongTinKH.AddRange(lstThongTinThuNoTTruoc);
                    _objTTinThuGocLai.DSACH_SO_NOP_TIEN = lstThongTinNopTK.ToArray();
                    _objTTinThuGocLai.DSACH_SO_RUT_TIEN = lstThongTinRutTK.ToArray();
                    _objTTinThuGocLai.DSACH_THONG_TIN_THU_NO = lstThongTinKH.ToArray();
                    dt = ds.Tables[4];
                    if(dt.Rows.Count > 0)
                    {
                        DataRow dr = dt.Rows[0];
                        _objTTinThuGocLai.BIEU_PHI = new BIEU_PHI_DTO();
                        _objTTinThuGocLai.BIEU_PHI.HTHUC_BTHANG = dr["HTHUC_BTHANG"].ToString();
                        _objTTinThuGocLai.BIEU_PHI.ID_BPHI = Convert.ToInt32(dr["ID"]);
                        _objTTinThuGocLai.BIEU_PHI.LOAI_BPHI = dr["LOAI_BPHI"].ToString();
                        _objTTinThuGocLai.BIEU_PHI.LOAI_TIEN = dr["MA_LOAI_TIEN"].ToString();
                        _objTTinThuGocLai.BIEU_PHI.MA_BPHI = dr["MA_BPHI"].ToString();
                        _objTTinThuGocLai.BIEU_PHI.NGAY_ADUNG = dr["NGAY_ADUNG"].ToString();
                        if (dr["NGAY_HHAN"] != DBNull.Value)
                            _objTTinThuGocLai.BIEU_PHI.NGAY_HHAN = dr["NGAY_HHAN"].ToString();
                        _objTTinThuGocLai.BIEU_PHI.TCHAT_BPHI = dr["TCHAT_BPHI"].ToString();
                        _objTTinThuGocLai.BIEU_PHI.TEN_BPHI = dr["TEN_BPHI"].ToString();
                        _objTTinThuGocLai.BIEU_PHI.TY_LE_VAT = Convert.ToDecimal(dr["TY_LE_VAT"]);
                    }
                    else
                        _objTTinThuGocLai.BIEU_PHI = new BIEU_PHI_DTO();
                    dt = ds.Tables[5];
                    if (dt.Rows.Count > 0)
                    {
                        lstBieuPhi = new List<BIEU_PHI_CTIET_DTO>();
                        foreach (DataRow dr in dt.Rows)
                        {
                            BIEU_PHI_CTIET_DTO objBieuPhiCT = new BIEU_PHI_CTIET_DTO();
                            objBieuPhiCT.ID_BPHI = Convert.ToInt32(dr["ID_BPHI"]);
                            objBieuPhiCT.LOAI_BPHI = dr["LOAI_BPHI"].ToString();
                            objBieuPhiCT.MA_BPHI = dr["MA_BPHI"].ToString();
                            objBieuPhiCT.SO_TIEN_TINH_PHI = Convert.ToDecimal(dr["SO_TIEN"]);
                            objBieuPhiCT.SO_TIEN_PHI = Convert.ToDecimal(dr["SO_TIEN_PHI"]);
                            objBieuPhiCT.SO_TIEN_TDA = Convert.ToDecimal(dr["SO_TIEN_TDA"]);
                            objBieuPhiCT.SO_TIEN_TTHIEU = Convert.ToDecimal(dr["SO_TIEN_TTHIEU"]);
                            objBieuPhiCT.TY_LE_PHI = Convert.ToDecimal(dr["TY_LE_PHI"]);
                            objBieuPhiCT.TY_LE_VAT = Convert.ToDecimal(dr["TY_LE_VAT"]);
                            lstBieuPhi.Add(objBieuPhiCT);
                        }
                        
                    }
                    else
                    {
                        lstBieuPhi = new List<BIEU_PHI_CTIET_DTO>();
                    }
                    _objTTinThuGocLai.BIEU_PHI.DSACH_BPHI_CT = lstBieuPhi.ToArray();
                }

            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void TinhPhiTraTruoc()
        {
            try
            {
                string ngayDaoHan = _objTTinThuGocLai.DSACH_THONG_TIN_THU_NO.LastOrDefault(f => f.GOC_KH > 0).NGAY_KH;
                decimal soDu = _objTTinThuGocLai.DU_NO;
                decimal soTienPhi = 0;
                int soNgayTraTruoc = LDateTime.StringToDate(ngayDaoHan, ApplicationConstant.defaultDateTimeFormat).CountDayBetweenDates(LDateTime.StringToDate(obj.NGAY_GIAO_DICH, ApplicationConstant.defaultDateTimeFormat));
                decimal tyLe = 0;
                decimal soTien = 0;
                decimal soTienTThieu = 0;
                decimal soTienTDa = 0;
                if (!lstBieuPhi.IsNullOrEmpty())
                {
                    soTienTThieu = lstBieuPhi.FirstOrDefault().SO_TIEN_TTHIEU;
                    soTienTDa = lstBieuPhi.FirstOrDefault().SO_TIEN_TDA;
                }
                if (_objTTinThuGocLai.BIEU_PHI.TCHAT_BPHI.Equals(BusinessConstant.TCHAT_BPHI.DTH.layGiaTri()))
                {
                    if (_objTTinThuGocLai.BIEU_PHI.HTHUC_BTHANG.Equals(BusinessConstant.HTHUC_BTHANG_PHI.TY_LE.layGiaTri()))
                    {
                        tyLe = lstBieuPhi.FirstOrDefault().TY_LE_PHI;
                    }
                    else if (_objTTinThuGocLai.BIEU_PHI.HTHUC_BTHANG.Equals(BusinessConstant.HTHUC_BTHANG_PHI.SO_TIEN.layGiaTri()))
                    {
                        soTien = lstBieuPhi.FirstOrDefault().SO_TIEN_PHI;
                    }
                    else if (_objTTinThuGocLai.BIEU_PHI.HTHUC_BTHANG.Equals(BusinessConstant.HTHUC_BTHANG_PHI.STIEN_TLE.layGiaTri()))
                    {
                        tyLe = lstBieuPhi.FirstOrDefault().TY_LE_PHI;
                        soTien = lstBieuPhi.FirstOrDefault().SO_TIEN_PHI;
                    }
                }
                else
                {
                }
                if (_objTTinThuGocLai.BIEU_PHI.HTHUC_BTHANG.Equals(BusinessConstant.HTHUC_BTHANG_PHI.TY_LE.layGiaTri()))
                {
                    soTienPhi = _objTTinThuGocLai.DU_NO * soNgayTraTruoc * (tyLe / 360 / 100);
                }
                else if (_objTTinThuGocLai.BIEU_PHI.HTHUC_BTHANG.Equals(BusinessConstant.HTHUC_BTHANG_PHI.SO_TIEN.layGiaTri()))
                {
                    soTienPhi = soTien;
                }
                else if (_objTTinThuGocLai.BIEU_PHI.HTHUC_BTHANG.Equals(BusinessConstant.HTHUC_BTHANG_PHI.STIEN_TLE.layGiaTri()))
                {

                }
                if (soTienPhi < soTienTThieu)
                    soTienPhi = soTienTThieu;
                if (soTienPhi > soTienTDa)
                    soTienPhi = soTienTDa;
                soTienPhi = soTienPhi.Rounding(0);
                _objTTinThuGocLai.PHI_TRA_TRUOC = soTienPhi;
                _objTTinThuGocLai.MA_PHI_TRA_TRUOC = _objTTinThuGocLai.BIEU_PHI.MA_BPHI;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                LMessage.ShowMessage("M.DungChung.LoiKhongXacDinh", LMessage.MessageBoxType.Error);
            }
        }

        void TatToan()
        {
            try
            {
                LaiPhat = 0;
                SoTienPhi = 0;
                GocQuaHan = 0;
                GocTrongHan = 0;
                LaiTrongHan = 0;
                LaiQuaHan = 0;
                if (chkTatToan.IsChecked.GetValueOrDefault())
                {
                    LaiPhat = lstThongTinThuNo.Sum(f => f.LAI_PHAT_KH);

                    lstThongTinThuNo.ForEach(f => { f.GOC_TT = f.GOC_KH; f.LAI_TT = f.LAI_KH; f.LAI_PHAT_TT = f.LAI_PHAT_KH; });
                    lstThongTinThuNoTTruoc.ForEach(f => { f.GOC_TT = f.GOC_KH; f.LAI_TT = 0; f.LAI_PHAT_TT = 0; });
                    TinhLaiTatToanTruocHan();
                    TinhPhiTraTruoc();
                    SoTienPhi = _objTTinThuGocLai.PHI_TRA_TRUOC;
                    GocTrongHan = lstThongTinThuNo.Where(f => f.NHOM_NO.Equals("part0")).Sum(f => f.GOC_KH);
                    GocTrongHan += lstThongTinThuNoTTruoc.Where(f => f.NHOM_NO.Equals("part0")).Sum(f => f.GOC_KH);
                    GocQuaHan = lstThongTinThuNo.Where(f => !f.NHOM_NO.Equals("part0")).Sum(f => f.GOC_KH);
                    GocQuaHan += lstThongTinThuNoTTruoc.Where(f => !f.NHOM_NO.Equals("part0")).Sum(f => f.GOC_KH);
                    LaiTrongHan = lstThongTinThuNo.Where(f => f.NHOM_NO.Equals("part0")).Sum(f => f.LAI_KH);
                    LaiQuaHan += lstThongTinThuNo.Where(f => !f.NHOM_NO.Equals("part0")).Sum(f => f.LAI_KH);
                    LaiTrongHan += lstThongTinThuNoTTruoc.Sum(f => f.LAI_TT);

                    _objTTinThuGocLai.THUC_THU_GOC_VAY = GocTrongHan + GocQuaHan;
                    _objTTinThuGocLai.LAI_PHAT = LaiPhat;
                    radTongGoc.Value = (double)(GocTrongHan + GocQuaHan);
                    radTongLai.Value = (double)(LaiTrongHan + LaiQuaHan);
                    
                    radTongLaiPhat.Value = (double)_objTTinThuGocLai.LAI_PHAT;
                    radTongPhi.Value = (double)_objTTinThuGocLai.PHI_TRA_TRUOC;
                    TinhToanTienThuaNopTKBB();
                }
                
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        void TinhLaiTatToanTruocHan()
        {
            try
            {
                THONG_TIN_THU_NO objTinThuNo = lstThongTinThuNo.FirstOrDefault(f => f.GOC_KH > 0 || f.LAI_KH > 0);
                if (objTinThuNo.IsNullOrEmpty())
                {
                    int iret = 0;
                    List<ClientResponseDetail> ResponseDetail = new List<ClientResponseDetail>();
                    if (_objTTinThuGocLai.IsNullOrEmpty())
                        _objTTinThuGocLai = new DANH_SACH_KHE_UOC_VONG_VAY();
                    List<THONG_TIN_THU_NO> lstThongTinKH = new List<THONG_TIN_THU_NO>();
                    lstThongTinKH.AddRange(lstThongTinThuNo);
                    lstThongTinKH.AddRange(lstThongTinThuNoTTruoc);
                    _objTTinThuGocLai.DSACH_THONG_TIN_THU_NO = lstThongTinKH.ToArray();
                    obj.DSACH_KHE_UOC = new DANH_SACH_KHE_UOC_VONG_VAY[1];
                    obj.DSACH_KHE_UOC[0] = _objTTinThuGocLai;
                    iret = new TinDungProcess().ThuGocLaiVayTruocHan(DatabaseConstant.Action.TINH_TOAN_LICH_TRA_NO, ref obj, ref ResponseDetail);
                    _objTTinThuGocLai = obj.DSACH_KHE_UOC[0];
                    lstThongTinThuNo = _objTTinThuGocLai.DSACH_THONG_TIN_THU_NO.Where(f => f.NGAY_KH.CompareTo(ClientInformation.NgayLamViecHienTai) <= 0).ToList();
                    lstThongTinThuNoTTruoc = _objTTinThuGocLai.DSACH_THONG_TIN_THU_NO.Where(f => f.NGAY_KH.CompareTo(ClientInformation.NgayLamViecHienTai) > 0).ToList();
                }
            }
            catch (Exception ex)
            {

                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }
        #endregion

    }
}
