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
using Presentation.Process.TinDungTDServiceRef;
using Telerik.Windows.Controls;
using Presentation.Process;
using Presentation.Process.PopupServiceRef;
using Presentation.Process.Common;
using System.Reflection;
using PresentationWPF.BaoCao.DungChung;
using Presentation.Process.BaoCaoServiceRef;
using System.Collections;

namespace PresentationWPF.TinDungTD.GiaiNgan
{
    /// <summary>
    /// Interaction logic for ucGiaiNganKheUocCT_01.xaml
    /// </summary>
    public partial class ucGiaiNganKheUocCT : UserControl
    {
        /// <summary>
        /// Khai bao
        /// </summary>
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
        int idKhachHang = 0;
        string maKhangHang = "";
        string tenKhachHang = "";

        string soGTLQ = "";
        string tenNhom = "";
        string TThaiNVu = "";
        public DatabaseConstant.Action action;
        public event EventHandler OnSavingCompleted;
        List<int> lstId = new List<int>();
        
        TDTD_GIAI_NGAN obj;
        THONG_TIN_KHE_UOC_GNGAN objKUOC;
        THONG_TIN_SO_TKIEM_GNGAN objSOTK;

        List<AutoCompleteEntry> lstSourceTienTe = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceLaiSuat = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceHinhThucToan = new List<AutoCompleteEntry>();

        private int idDiaBan = 0;
        private string maVongVay = "";
        private int heSo = 0;
        private int iDGDich = 0;
        private string maDonVi;
        string soTienHanMuc = "0";
        string soTienGoc = "0";
        string soTienLai = "0";
        string capPheDuyet = "";
        string loaiTien = ClientInformation.MaDongNoiTe;
        string hinhThucGoc = BusinessConstant.TINH_CHAT_VONG_VAY.THAY_DOI.layGiaTri();
        string hinhThucKyHan = BusinessConstant.TINH_CHAT_VONG_VAY.THAY_DOI.layGiaTri();
        string maLaiSuat = "";
        int idBieuPhi = 0;
        List<BIEU_PHI_CTIET_DTO> lstBieuPhi = null;
        #endregion

        /// <summary>
        /// Khoi tao
        /// </summary>
        #region Khoi tao
        public ucGiaiNganKheUocCT()
        {
            InitializeComponent();
            HeThong htHeThong = new HeThong();
            htHeThong.DuyetQuyenTinhNangToolbar("/PresentationWPF.TinDung;component/DonVayVon/ucGiaiNganKheUocCT_01.xaml", ref Toolbar, ref mnuMain);
            foreach (var item in mnuMain.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += new RoutedEventHandler(btnShortcutKey_Click);
            }
            BindHotkey();
            KhoiTaoComboBox();
            ShowControl();
            InitEventHandler();
            TaoBangDuLieu();
            ResetForm();
            txtSoKheUoc.Focus();
        }

        public ucGiaiNganKheUocCT(KIEM_SOAT _objKiemSoat) : this()
        {
            iDGDich = _objKiemSoat.ID;
            obj = new TDTD_GIAI_NGAN();
            obj.ID_GDICH = iDGDich;
            obj.MA_GDICH = _objKiemSoat.SO_GIAO_DICH;
            action = _objKiemSoat.action;
            SetDataForm(_objKiemSoat.SO_GIAO_DICH);
        }
        private void ShowControl()
        {
            HeThong hethong = new HeThong();
            ArrayList arr = new ArrayList();
            arr = hethong.SetVisibleControl("PresentationWPF.TinDung.DonVayVon.ucGiaiNganKheUocCT_01", "raddgrDSThuNhap");
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
            this.Unloaded += new RoutedEventHandler(ucGiaiNganKheUocCT_Unloaded);
            btnSoKheUoc.Click += new RoutedEventHandler(btnSoKheUoc_Click);
            btnSoTaiKhoan.Click += new RoutedEventHandler(btnSoTaiKhoan_Click);
            btnSoTaiKhoanTK.Click += new RoutedEventHandler(btnSoTaiKhoanTK_Click);
            cmbHinhThucToan.SelectionChanged += new SelectionChangedEventHandler(cmbHinhThucToan_SelectionChanged);
            cmbHinhThucToanTK.SelectionChanged += new SelectionChangedEventHandler(cmbHinhThucToanTK_SelectionChanged);
            btnPhiMoSo.Click += new RoutedEventHandler(btnPhiMoSo_Click);
            telTienMat.ValueChanged += new EventHandler<Telerik.Windows.RadRoutedEventArgs>(telTienMat_ValueChanged);
            telTienMatTK.ValueChanged += new EventHandler<Telerik.Windows.RadRoutedEventArgs>(telTienMatTK_ValueChanged);
            telSoTienPhi.ValueChanged += new EventHandler<Telerik.Windows.RadRoutedEventArgs>(telSoTienPhi_ValueChanged);
        }

        
        void ucGiaiNganKheUocCT_Unloaded(object sender, RoutedEventArgs e)
        {
            Release();
        }

        void TaoBangDuLieu()
        {
            capPheDuyet = new UtilitiesProcess().LayGiaTriThamSo(BusinessConstant.LoaiThamSo.TW, BusinessConstant.MaThamSo.TW_ON_CHECK_MATRIX_APPROVE, ClientInformation.MaDonVi);
            if (capPheDuyet.IsNullOrEmptyOrSpace())
                capPheDuyet = BusinessConstant.CoKhong.KHONG.layGiaTri();
            if (capPheDuyet.Equals(BusinessConstant.CoKhong.KHONG.layGiaTri()))
                spnCapDuyet.Visibility = Visibility.Collapsed;
            else
                spnCapDuyet.Visibility = Visibility.Visible;
            
        }

        void KhoiTaoComboBox()
        {
            List<COMBOBOX_DTO> lst = new List<COMBOBOX_DTO>();
            COMBOBOX_DTO obj = new COMBOBOX_DTO();
            List<string> lstDieuKien = new List<string>();

            obj.combobox = cmbLoaiTien;
            obj.maChon = ClientInformation.MaDongNoiTe;
            obj.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_TIENTE.getValue();
            obj.lstSource = lstSourceTienTe;
            obj.lstDieuKien = lstDieuKien;
            lst.Add(obj);

            obj = new COMBOBOX_DTO();
            lstDieuKien = new List<string>();
            obj.combobox = cmbTienTe;
            obj.maCSo = null;
            obj.lstSource = lstSourceTienTe;
            obj.lstDieuKien = lstDieuKien;
            lst.Add(obj);

            obj = new COMBOBOX_DTO();
            lstDieuKien = new List<string>();
            obj.combobox = cmbTienTeTK;
            obj.maCSo = null;
            obj.lstSource = lstSourceTienTe;
            obj.lstDieuKien = lstDieuKien;
            lst.Add(obj);

            obj = new COMBOBOX_DTO();
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.DVI_TINH_LSUAT.getValue());
            obj.combobox = cmbLaiSuat;
            obj.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();
            obj.lstSource = lstSourceLaiSuat;
            obj.lstDieuKien = lstDieuKien;
            lst.Add(obj);

            obj = new COMBOBOX_DTO();
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.HINH_THUC_GIAO_DICH.getValue());
            obj.combobox = cmbHinhThucToan;
            obj.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();
            obj.lstSource = lstSourceHinhThucToan;
            obj.lstDieuKien = lstDieuKien;
            obj.maChon = BusinessConstant.HINH_THUC_GIAO_DICH.TIEN_MAT.layGiaTri();
            lst.Add(obj);

            obj = new COMBOBOX_DTO();
            lstDieuKien = new List<string>();
            obj.combobox = cmbHinhThucToanTK;
            obj.maCSo = null;
            obj.lstSource = lstSourceHinhThucToan;
            obj.lstDieuKien = lstDieuKien;
            obj.maChon = BusinessConstant.HINH_THUC_GIAO_DICH.TIEN_MAT.layGiaTri();
            lst.Add(obj);

            new AutoComboBox().GenAutoComboBoxTheoList(ref lst);
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
            else if (strTinhNang.Equals("PreviewChungTu"))
            {
                OnPrint();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XEM_TRUOC)))
            {
                OnPreviewDonVayVon();
            }
            else if (strTinhNang.Equals("PreviewDonVayVon"))
            {
                OnPreviewDonVayVon();
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
            else if (strTinhNang.Equals("PreviewChungTu"))
            {
                OnPrint();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XEM_TRUOC)))
            {
                OnPreviewDonVayVon();
            }
            else if (strTinhNang.Equals("PreviewDonVayVon"))
            {
                OnPreviewDonVayVon();
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

        /// <summary>
        /// Xu ly giao dien
        /// </summary>
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

        /// <summary>
        /// Sự kiện chọn ngày của DatetimePicker
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DatePicker dtpControl = (DatePicker)sender;
                StringBuilder sbControl = new StringBuilder();
                sbControl.Append("teldt");
                sbControl.Append(dtpControl.Name.Substring(3));
                RadMaskedDateTimeInput telControl = (RadMaskedDateTimeInput)grMain.FindName(sbControl.ToString());
                if (telControl != null)
                    telControl.Value = dtpControl.SelectedDate;
                else
                    throw new System.NullReferenceException("Không tìm thấy control " + sbControl.ToString());
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }


        void btnSoTaiKhoanTK_Click(object sender, RoutedEventArgs e)
        {
            
        }

        void btnSoTaiKhoan_Click(object sender, RoutedEventArgs e)
        {
            
        }

        void btnSoKheUoc_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<string> lstDieuKien = new List<string>();
                lstDieuKien.Add(ClientInformation.MaDonViGiaoDich);
                lstDieuKien.Add(teldtNgayGiaoDich.Value.GetValueOrDefault().ToString(ApplicationConstant.defaultDateTimeFormat));
                lstPopup = new List<DataRow>();
                PopupProcess popupProcess = new PopupProcess();
                popupProcess.getPopupInformation("POPUP_DS_KUOC_GNGAN_TIN_DUNG_TDUNG", lstDieuKien);
                SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;
                ucPopup popup = new ucPopup(false, simplePopupResponse, false);
                popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
                Window win = new Window();
                win.Content = popup;
                win.Title = LLanguage.SearchResourceByKey(simplePopupResponse.PopupTitle);
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.ShowDialog();
                obj = new TDTD_GIAI_NGAN();
                if (lstPopup.Count > 0)
                {
                    foreach (DataRow dr in lstPopup)
                    {
                        obj.ID_KUOCVM = Convert.ToInt32(dr["ID_KUOC"]);
                        obj.MA_KUOCVM = dr["MA_KUOC"].ToString();
                        obj.NGAY_PHAT_VON = dr["NGAY_GIAI_NGAN"].ToString();
                        obj.TEN_NGUOI_GDICH = dr["TEN_KHANG"].ToString();
                        obj.SO_GTLQ = dr["SO_GTLQ"].ToString();

                        objKUOC = new THONG_TIN_KHE_UOC_GNGAN();
                        objKUOC.LOAI_TIEN = dr["LOAI_TIEN"].ToString();
                        objKUOC.LOAI_TIEN_GN = dr["LOAI_TIEN"].ToString();
                        objKUOC.LAI_SUAT = Convert.ToDecimal(dr["LAI_SUAT"]);
                        objKUOC.LSUAT_DVT = dr["LSUAT_DVT"].ToString();
                        objKUOC.MA_KHANG = dr["MA_KHANG"].ToString();
                        objKUOC.NGAY_NHAN_NO = dr["NGAY_GIAI_NGAN"].ToString();
                        objKUOC.SO_KUOC = obj.SO_KUOCVM = dr["SO_KUOC"].ToString();
                        objKUOC.SO_TIEN_GNGAN = Convert.ToDecimal(dr["SO_TIEN_PHAT_VAY"]);
                        objKUOC.SO_TIEN_MAT = Convert.ToDecimal(dr["SO_TIEN_PHAT_VAY"]);
                        objKUOC.SO_TIEN_NHAN_NO = Convert.ToDecimal(dr["SO_TIEN_GNGAN"]);
                        objKUOC.TEN_KHANG = dr["TEN_KHANG"].ToString();
                        objKUOC.NV_LOAI_NVON = dr["NV_LOAI_NVON"].ToString();
                        objKUOC.MA_SAN_PHAM = dr["MA_SAN_PHAM"].ToString();
                        objKUOC.MA_KUOC = obj.MA_KUOCVM = dr["MA_KUOC"].ToString();

                        objSOTK = new THONG_TIN_SO_TKIEM_GNGAN();
                        objSOTK.LOAI_TIEN = dr["LOAI_TIEN_TK"].ToString();
                        objSOTK.LOAI_TIEN_GN = dr["LOAI_TIEN_TK"].ToString();
                        objSOTK.MA_KHANG = Convert.ToString(dr["MA_KHANG"]);
                        objSOTK.SO_SO_TK = dr["SO_SO_TK"].ToString();
                        objSOTK.SO_TIEN_THU_TK = Convert.ToDecimal(dr["SO_TIEN_THU_TK"]);
                        objSOTK.TEN_KHANG = dr["TEN_KHANG"].ToString();
                        objSOTK.NV_LOAI_NVON = dr["NV_LOAI_NVON_TK"].ToString();
                        objSOTK.MA_PHI_MO_SO = dr["MA_BPHI"].ToString();
                        objSOTK.PHI_MO_SO = Convert.ToDecimal(dr["SO_TIEN_PHI_MO_SO"]);

                        objSOTK.BIEU_PHI = new BIEU_PHI_DTO();
                        objSOTK.BIEU_PHI.HTHUC_BTHANG = dr["HTHUC_BTHANG"].ToString();
                        objSOTK.BIEU_PHI.ID_BPHI = Convert.ToInt32(dr["ID_BPHI"]);
                        objSOTK.BIEU_PHI.LOAI_BPHI = dr["LOAI_BPHI"].ToString();
                        objSOTK.BIEU_PHI.LOAI_TIEN = dr["MA_LOAI_TIEN"].ToString();
                        objSOTK.BIEU_PHI.MA_BPHI = dr["MA_BPHI"].ToString();
                        objSOTK.BIEU_PHI.NGAY_ADUNG = dr["NGAY_ADUNG"].ToString();
                        if (dr["NGAY_HHAN"] != DBNull.Value)
                            objSOTK.BIEU_PHI.NGAY_HHAN = dr["NGAY_HHAN"].ToString();
                        objSOTK.BIEU_PHI.TCHAT_BPHI = dr["TCHAT_BPHI"].ToString();
                        objSOTK.BIEU_PHI.TEN_BPHI = dr["TEN_BPHI"].ToString();
                        objSOTK.BIEU_PHI.TY_LE_VAT = Convert.ToDecimal(dr["TY_LE_VAT"]);
                        GetThongTinBieuPhiCTiet();
                        objSOTK.BIEU_PHI.DSACH_BPHI_CT = lstBieuPhi.ToArray();
                        TinhPhiTraTruoc();
                        txtPhiMoSo.Text = objSOTK.MA_PHI_MO_SO;
                        telSoTienPhi.Value = (double)objSOTK.PHI_MO_SO;
                        txtSoKheUoc.Text = obj.MA_KUOCVM;
                        txtTenKHang.Text = obj.TEN_NGUOI_GDICH;
                        txtNgayNhanNo.Value = LDateTime.StringToDate(objKUOC.NGAY_NHAN_NO,ApplicationConstant.defaultDateTimeFormat);
                        cmbLoaiTien.SelectedIndex = lstSourceTienTe.IndexOf(lstSourceTienTe.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(objKUOC.LOAI_TIEN)));
                        cmbTienTe.SelectedIndex = lstSourceTienTe.IndexOf(lstSourceTienTe.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(objKUOC.LOAI_TIEN_GN)));
                        telnumSoTienGiaiNgan.Value = Convert.ToDouble(objKUOC.SO_TIEN_NHAN_NO);
                        telnumLaiSuat.Value = Convert.ToDouble(objKUOC.LAI_SUAT);
                        cmbLaiSuat.SelectedIndex = lstSourceLaiSuat.IndexOf(lstSourceLaiSuat.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(objKUOC.LSUAT_DVT)));
                        telnumGiaiNgan.Value = Convert.ToDouble(objKUOC.SO_TIEN_GNGAN);
                        telSoTienQuyDoi.Value = Convert.ToDouble(objKUOC.SO_TIEN_GNGAN);
                        telTienMat.Value = Convert.ToDouble(objKUOC.SO_TIEN_GNGAN);

                        telnumSoTienTK.Value = Convert.ToDouble(objSOTK.SO_TIEN_THU_TK);
                        telSoTienQuyDoiTK.Value = Convert.ToDouble(objSOTK.SO_TIEN_THU_TK);
                        telTienMatTK.Value = Convert.ToDouble(objSOTK.SO_TIEN_THU_TK + objSOTK.PHI_MO_SO);
                        cmbTienTeTK.SelectedIndex = lstSourceTienTe.IndexOf(lstSourceTienTe.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(objSOTK.LOAI_TIEN_GN)));

                        txtSoKheUoc.Tag = obj.ID_KUOCVM;
                    }
                }
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        void cmbHinhThucToanTK_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AutoCompleteEntry au = lstSourceHinhThucToan.ElementAt(cmbHinhThucToanTK.SelectedIndex);
            if (au.KeywordStrings.FirstOrDefault().Equals(BusinessConstant.HINH_THUC_GIAO_DICH.TIEN_MAT.layGiaTri()))
            {
                telTienMatTK.Value = telnumSoTienTK.Value + telSoTienPhi.Value;
                telChuyenKhoanTK.Value = 0;
                telTienMatTK.IsEnabled = false;
                txtSoTaiKhoanTK.IsEnabled = false;
            }
            else if (au.KeywordStrings.FirstOrDefault().Equals(BusinessConstant.HINH_THUC_GIAO_DICH.CHUYEN_KHOAN.layGiaTri()))
            {
                telTienMatTK.Value = 0;
                telChuyenKhoanTK.Value = telnumSoTienTK.Value + telSoTienPhi.Value;
                telTienMatTK.IsEnabled = false;
                txtSoTaiKhoanTK.IsEnabled = true;
            }
            else
            {
                telTienMatTK.Value = telnumSoTienTK.Value + telSoTienPhi.Value;
                telChuyenKhoanTK.Value = 0;
                telTienMatTK.IsEnabled = true;
                txtSoTaiKhoanTK.IsEnabled = true;
            }
        }

        void cmbHinhThucToan_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AutoCompleteEntry au = lstSourceHinhThucToan.ElementAt(cmbHinhThucToan.SelectedIndex);
            if (au.KeywordStrings.FirstOrDefault().Equals(BusinessConstant.HINH_THUC_GIAO_DICH.TIEN_MAT.layGiaTri()))
            {
                telTienMat.Value = telnumGiaiNgan.Value;
                telChuyenKhoan.Value = 0;
                telTienMat.IsEnabled = false;
                txtSoTaiKhoan.IsEnabled = false;
            }
            else if (au.KeywordStrings.FirstOrDefault().Equals(BusinessConstant.HINH_THUC_GIAO_DICH.CHUYEN_KHOAN.layGiaTri()))
            {
                telTienMat.Value = 0;
                telChuyenKhoan.Value = telnumGiaiNgan.Value;
                telTienMat.IsEnabled = false;
                txtSoTaiKhoan.IsEnabled = true;
            }
            else
            {
                telTienMat.Value = telnumGiaiNgan.Value;
                telChuyenKhoan.Value = 0;
                telTienMat.IsEnabled = true;
                txtSoTaiKhoan.IsEnabled = true;
            }
        }

        void btnPhiMoSo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AutoCompleteEntry au = lstSourceHinhThucToan.ElementAt(cmbHinhThucToanTK.SelectedIndex);
                lstPopup.Clear();
                List<string> lstDieuKien = new List<string>();
                lstDieuKien.Add(ClientInformation.MaDonVi);
                lstDieuKien.Add(ClientInformation.NgayLamViecHienTai);
                lstDieuKien.Add("'GN01','GN02','GN03'");
                PopupProcess popupProcess = new PopupProcess();
                popupProcess.getPopupInformation("POPUP_DS_BIEU_PHI_LOAI_GDICH", lstDieuKien);
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
                    objSOTK.BIEU_PHI = new BIEU_PHI_DTO();
                    objSOTK.BIEU_PHI.HTHUC_BTHANG = dr["HTHUC_BTHANG"].ToString();
                    objSOTK.BIEU_PHI.ID_BPHI = Convert.ToInt32(dr["ID"]);
                    objSOTK.BIEU_PHI.LOAI_BPHI = dr["LOAI_BPHI"].ToString();
                    objSOTK.BIEU_PHI.LOAI_TIEN = dr["MA_LOAI_TIEN"].ToString();
                    objSOTK.BIEU_PHI.MA_BPHI = dr["MA_BPHI"].ToString();
                    objSOTK.BIEU_PHI.NGAY_ADUNG = dr["NGAY_ADUNG"].ToString();
                    if (dr["NGAY_HHAN"] != DBNull.Value)
                        objSOTK.BIEU_PHI.NGAY_HHAN = dr["NGAY_HHAN"].ToString();
                    objSOTK.BIEU_PHI.TCHAT_BPHI = dr["TCHAT_BPHI"].ToString();
                    objSOTK.BIEU_PHI.TEN_BPHI = dr["TEN_BPHI"].ToString();
                    objSOTK.BIEU_PHI.TY_LE_VAT = Convert.ToDecimal(dr["TY_LE_VAT"]);
                    DataSet ds = new PhiProcess().GetPhiByID(objSOTK.BIEU_PHI.ID_BPHI);
                    DataTable dt = ds.Tables[1];
                    lstBieuPhi = new List<BIEU_PHI_CTIET_DTO>();
                    GetThongTinBieuPhiCTiet();
                    objSOTK.BIEU_PHI.DSACH_BPHI_CT = lstBieuPhi.ToArray();
                    TinhPhiTraTruoc();
                    txtPhiMoSo.Text = objSOTK.BIEU_PHI.MA_BPHI;
                    telSoTienPhi.Value = (double)objSOTK.PHI_MO_SO;
                    if (au.KeywordStrings.FirstOrDefault().Equals(BusinessConstant.HINH_THUC_GIAO_DICH.CHUYEN_KHOAN.layGiaTri()))
                    {
                        telTienMatTK.Value = 0;
                        telChuyenKhoanTK.Value = telnumSoTienTK.Value + telSoTienPhi.Value;
                    }
                    else
                    {
                        telTienMatTK.Value = telSoTienPhi.Value + telnumSoTienTK.Value;
                        telChuyenKhoanTK.Value = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        void telTienMatTK_ValueChanged(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            telChuyenKhoanTK.Value = telnumSoTienTK.Value + telSoTienPhi.Value - telTienMatTK.Value;
        }

        void telTienMat_ValueChanged(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            telChuyenKhoan.Value = telnumGiaiNgan.Value - telTienMat.Value;
        }

        void telSoTienPhi_ValueChanged(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            try
            {
                AutoCompleteEntry au = lstSourceHinhThucToan.ElementAt(cmbHinhThucToanTK.SelectedIndex);
                if (au.KeywordStrings.FirstOrDefault().Equals(BusinessConstant.HINH_THUC_GIAO_DICH.CHUYEN_KHOAN.layGiaTri()))
                {
                    telTienMatTK.Value = 0;
                    if (telSoTienPhi.Value.IsNullOrEmpty())
                        telSoTienPhi.Value = 0;
                    telChuyenKhoanTK.Value = telnumSoTienTK.Value + telSoTienPhi.Value;
                }
                else
                {
                    if (telSoTienPhi.Value.IsNullOrEmpty())
                        telSoTienPhi.Value = 0;
                    telTienMatTK.Value = telSoTienPhi.Value + telnumSoTienTK.Value;
                    telChuyenKhoanTK.Value = 0;
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        #endregion

        /// <summary>
        /// Xu ly nghiep vu
        /// </summary>
        #region Xu ly nghiep vu
        void GetDataForm(BusinessConstant.TrangThaiBanGhi bghi, BusinessConstant.TrangThaiNghiepVu nghiepvu)
        {
            try
            {
                AutoCompleteEntry auHinhThucGN = lstSourceHinhThucToan.ElementAt(cmbHinhThucToan.SelectedIndex);
                AutoCompleteEntry auHinhThucTK = lstSourceHinhThucToan.ElementAt(cmbHinhThucToanTK.SelectedIndex);
                AutoCompleteEntry auLoaiTien = lstSourceTienTe.ElementAt(cmbLoaiTien.SelectedIndex);
                AutoCompleteEntry auLoaiTienGN = lstSourceTienTe.ElementAt(cmbTienTe.SelectedIndex);
                AutoCompleteEntry auLoaiTienTK = lstSourceTienTe.ElementAt(cmbTienTeTK.SelectedIndex);
                AutoCompleteEntry auLaiSuat = lstSourceLaiSuat.ElementAt(cmbLaiSuat.SelectedIndex);
                
                objKUOC.HINH_THUC_TTOAN = auHinhThucGN.KeywordStrings.FirstOrDefault();
                objKUOC.ID_GDICH = iDGDich;
                objKUOC.MA_GDICH = txtSoGiaoDich.Text;
                objKUOC.SO_TIEN_NHAN_NO = Convert.ToDecimal(telnumSoTienGiaiNgan.Value.GetValueOrDefault());
                objKUOC.LOAI_TIEN = auLoaiTien.KeywordStrings.FirstOrDefault();
                objKUOC.NGAY_NHAN_NO = txtNgayNhanNo.Value.GetValueOrDefault().DateToString(ApplicationConstant.defaultDateTimeFormat);
                objKUOC.LAI_SUAT = Convert.ToDecimal(telnumLaiSuat.Value.GetValueOrDefault());
                objKUOC.LSUAT_DVT = auLaiSuat.KeywordStrings.FirstOrDefault();
                objKUOC.SO_TIEN_GNGAN = Convert.ToDecimal(telnumGiaiNgan.Value.GetValueOrDefault());
                objKUOC.LOAI_TIEN_GN = auLoaiTienGN.KeywordStrings.FirstOrDefault();
                objKUOC.SO_TIEN_QDOI = Convert.ToDecimal(telSoTienQuyDoi.Value.GetValueOrDefault());
                objKUOC.SO_TIEN_MAT = Convert.ToDecimal(telTienMat.Value.GetValueOrDefault());
                objKUOC.SO_TIEN_CK = Convert.ToDecimal(telChuyenKhoan.Value.GetValueOrDefault());
                objKUOC.SO_TKHOAN_CK = txtSoTaiKhoan.Text;
                
                objSOTK.HINH_THUC_TTOAN = auHinhThucTK.KeywordStrings.FirstOrDefault();
                objSOTK.ID_GDICH = iDGDich;
                objSOTK.MA_GDICH = txtSoGiaoDich.Text;
                objSOTK.SO_TIEN_THU_TK = Convert.ToDecimal(telnumSoTienTK.Value.GetValueOrDefault());
                objSOTK.LOAI_TIEN = auLoaiTien.KeywordStrings.FirstOrDefault();
                objSOTK.LOAI_TIEN_GN = auLoaiTienTK.KeywordStrings.FirstOrDefault();
                objSOTK.SO_TIEN_QDOI = Convert.ToDecimal(telSoTienQuyDoiTK.Value.GetValueOrDefault());
                objSOTK.SO_TIEN_MAT = Convert.ToDecimal(telTienMatTK.Value.GetValueOrDefault());
                objSOTK.SO_TIEN_CK = Convert.ToDecimal(telChuyenKhoanTK.Value.GetValueOrDefault());
                objSOTK.SO_TKHOAN_CK = txtSoTaiKhoanTK.Text;
                objSOTK.MA_PHI_MO_SO = txtPhiMoSo.Text;
                objSOTK.PHI_MO_SO = (decimal)telSoTienPhi.Value.GetValueOrDefault();

                obj.NGAY_PHAT_VON = teldtNgayGiaoDich.Value.GetValueOrDefault().DateToString(ApplicationConstant.defaultDateTimeFormat);
                obj.NGAY_GDICH = teldtNgayGiaoDich.Value.GetValueOrDefault().DateToString(ApplicationConstant.defaultDateTimeFormat);
                obj.ID_GDICH = iDGDich;
                obj.MA_GDICH = txtSoGiaoDich.Text;
                obj.DIEN_GIAI = txtDienGiai.Text;
                obj.LOAI_TIEN = auLoaiTien.KeywordStrings.FirstOrDefault();
                obj.MA_KUOCVM = txtSoKheUoc.Text;
                obj.TTIN_KUOC = objKUOC;
                obj.TTIN_SO_TK = objSOTK;
                obj.TTHAI_BGHI = bghi.layGiaTri();
                obj.TTHAI_NVU = nghiepvu.layGiaTri();
                obj.TEN_NGUOI_GDICH = txtTenKHang.Text;
                obj.MA_DVI_QLY = ClientInformation.MaDonVi;
                obj.MA_DVI_GD = ClientInformation.MaDonViGiaoDich;
                if (iDGDich > 0)
                {
                    obj.NGUOI_CNHAT = ClientInformation.TenDangNhap;
                    obj.NGAY_CNHAT = ClientInformation.NgayLamViecHienTai;
                    obj.NGUOI_LAP = txtNguoiLap.Text;
                    obj.NGAY_LAP = LDateTime.DateToString(teldtNgayNhap.Value.GetValueOrDefault(),ApplicationConstant.defaultDateTimeFormat);
                }
                else
                {
                    obj.NGUOI_LAP = ClientInformation.TenDangNhap;
                    obj.NGAY_LAP = ClientInformation.NgayLamViecHienTai;
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        public void SetDataForm(string sSoGiaoDich)
        {
            try
            {
                DataSet ds = new DataSet();
                DataTable dtPar = null;
                LDatatable.MakeParameterTable(ref dtPar);
                LDatatable.AddParameter(ref dtPar, "@SoGiaoDich", "String", sSoGiaoDich);
                LDatatable.AddParameter(ref dtPar, "@IdGiaoDich", "String", iDGDich.ToString());
                ds = new TinDungTDProcess().GetThongTinGiaiNgan(dtPar);
                Dispatcher.CurrentDispatcher.DelayInvoke("SetDataForm", () =>
                {
                    SetTabThongTinChung(ds);
                    
                    Dispatcher.CurrentDispatcher.DelayInvoke("SetTabThongKiemSoat", () =>
                    {
                        SetTabThongKiemSoat(ds);
                    }, TimeSpan.FromSeconds(0));

                }, TimeSpan.FromSeconds(0));
            }
            catch (Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        void SetTabThongTinChung(DataSet ds)
        {
            try
            {

                DataTable dt = ds.Tables["TTIN_CHUNG"];
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    obj = new TDTD_GIAI_NGAN();

                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(TThaiNVu);

                    obj.ID_GDICH = iDGDich = Convert.ToInt32(dr["ID_GDICH"]);
                    obj.MA_GDICH = Convert.ToString(dr["MA_GDICH"]);
                    obj.NGAY_PHAT_VON = dr["NGAY_PHAT_VON"].ToString();
                    obj.NGAY_GDICH = dr["NGAY_GIAO_DICH"].ToString();
                    obj.TTHAI_NVU = TThaiNVu = dr["TTHAI_NVU"].ToString();
                    txtDienGiai.Text = dr["DIEN_GIAI"].ToString();
                    txtSoGiaoDich.Text = obj.MA_GDICH;
                    teldtNgayGiaoDich.Value = LDateTime.StringToDate(obj.NGAY_GDICH, ApplicationConstant.defaultDateTimeFormat);
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(obj.TTHAI_NVU);
                    CommonFunction.RefreshButton(Toolbar, action, TThaiNVu, mnuMain, DatabaseConstant.Function.TDTD_GIAI_NGAN);
                    if (action.Equals(DatabaseConstant.Action.SUA))
                        SetEnabledAllControls(true);
                    else
                        SetEnabledAllControls(false);
                }

                dt = ds.Tables["TTIN_KUOC"];
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    objKUOC = new THONG_TIN_KHE_UOC_GNGAN();
                    objKUOC.ID_GDICH = iDGDich = Convert.ToInt32(dr["ID_GDICH"]);
                    objKUOC.MA_GDICH = Convert.ToString(dr["MA_GDICH"]);
                    objKUOC.LOAI_TIEN = dr["LOAI_TIEN"].ToString();
                    objKUOC.LOAI_TIEN_GN = dr["LOAI_TIEN_GN"].ToString();
                    objKUOC.LAI_SUAT = Convert.ToDecimal(dr["LAI_SUAT"]);
                    objKUOC.LSUAT_DVT = dr["LSUAT_DVT"].ToString();
                    objKUOC.MA_KHANG = dr["MA_KHANG"].ToString();
                    objKUOC.NGAY_NHAN_NO = dr["NGAY_NHAN_NO"].ToString();
                    objKUOC.SO_KUOC = obj.SO_KUOCVM = dr["SO_KUOC"].ToString();
                    objKUOC.SO_TIEN_GNGAN = Convert.ToDecimal(dr["SO_TIEN_NHAN_NO"]);
                    objKUOC.SO_TIEN_MAT = Convert.ToDecimal(dr["SO_TIEN_MAT"]);
                    objKUOC.SO_TIEN_NHAN_NO = Convert.ToDecimal(dr["SO_TIEN_GNGAN"]);
                    objKUOC.SO_TIEN_QDOI = Convert.ToDecimal(dr["SO_TIEN_QDOI"]);
                    objKUOC.SO_TIEN_CK = Convert.ToDecimal(dr["SO_TIEN_CK"]);
                    objKUOC.SO_TKHOAN_CK = Convert.ToString(dr["SO_TKHOAN_CK"]);
                    objKUOC.TEN_KHANG = dr["TEN_KHANG"].ToString();
                    objKUOC.NV_LOAI_NVON = dr["NV_LOAI_NVON"].ToString();
                    objKUOC.MA_SAN_PHAM = dr["MA_SAN_PHAM"].ToString();
                    objKUOC.MA_KUOC = obj.MA_KUOCVM = dr["MA_KUOC"].ToString();
                    objKUOC.HINH_THUC_TTOAN = dr["HINH_THUC_TTOAN"].ToString();
                    obj.ID_KUOCVM = Convert.ToInt32(dr["ID_KUOC"]);
                    
                    txtSoKheUoc.Text = objKUOC.MA_KUOC;
                    txtSoKheUoc.Tag = obj.ID_KUOCVM;
                    txtTenKHang.Text = objKUOC.TEN_KHANG;
                    txtNgayNhanNo.Value = LDateTime.StringToDate(objKUOC.NGAY_NHAN_NO, ApplicationConstant.defaultDateTimeFormat);
                    cmbLoaiTien.SelectedIndex = lstSourceTienTe.IndexOf(lstSourceTienTe.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(objKUOC.LOAI_TIEN)));
                    telnumSoTienGiaiNgan.Value = Convert.ToDouble(objKUOC.SO_TIEN_NHAN_NO);
                    telnumLaiSuat.Value = Convert.ToDouble(objKUOC.LAI_SUAT);
                    cmbLaiSuat.SelectedIndex = lstSourceLaiSuat.IndexOf(lstSourceLaiSuat.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(objKUOC.LSUAT_DVT)));
                    telnumGiaiNgan.Value = Convert.ToDouble(objKUOC.SO_TIEN_GNGAN);
                    cmbTienTe.SelectedIndex = lstSourceTienTe.IndexOf(lstSourceTienTe.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(objKUOC.LOAI_TIEN_GN)));
                    telSoTienQuyDoi.Value = Convert.ToDouble(objKUOC.SO_TIEN_QDOI);
                    telTienMat.Value = Convert.ToDouble(objKUOC.SO_TIEN_MAT);
                    telChuyenKhoan.Value = Convert.ToDouble(objKUOC.SO_TIEN_CK);
                    cmbHinhThucToan.SelectedIndex = lstSourceHinhThucToan.IndexOf(lstSourceHinhThucToan.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(objKUOC.HINH_THUC_TTOAN)));
                    txtSoTaiKhoan.Text = objKUOC.SO_TKHOAN_CK;
                }

                dt = ds.Tables["TTIN_SOTK"];
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    objSOTK = new THONG_TIN_SO_TKIEM_GNGAN();
                    objSOTK.ID_GDICH = iDGDich = Convert.ToInt32(dr["ID_GDICH"]);
                    objSOTK.MA_GDICH = Convert.ToString(dr["MA_GDICH"]);
                    objSOTK.LOAI_TIEN = dr["LOAI_TIEN"].ToString();
                    objSOTK.LOAI_TIEN_GN = dr["LOAI_TIEN_GN"].ToString();

                    objSOTK.MA_KHANG = dr["MA_KHANG"].ToString();
                    objSOTK.SO_SO_TK = dr["SO_SO_TK"].ToString();
                    objSOTK.SO_TIEN_THU_TK = Convert.ToDecimal(dr["SO_TIEN_THU_TK"]);
                    objSOTK.SO_TIEN_MAT = Convert.ToDecimal(dr["SO_TIEN_MAT"]);
                    objSOTK.SO_TIEN_QDOI = Convert.ToDecimal(dr["SO_TIEN_QDOI"]);
                    objSOTK.SO_TIEN_CK = Convert.ToDecimal(dr["SO_TIEN_CK"]);
                    objSOTK.TEN_KHANG = dr["TEN_KHANG"].ToString();
                    objSOTK.NV_LOAI_NVON = dr["NV_LOAI_NVON"].ToString();
                    objSOTK.HINH_THUC_TTOAN = dr["HINH_THUC_TTOAN"].ToString();
                    objSOTK.SO_TKHOAN_CK = Convert.ToString(dr["SO_TKHOAN_CK"]);
                    objSOTK.MA_PHI_MO_SO = dr["MA_BPHI"].ToString();
                    objSOTK.PHI_MO_SO = Convert.ToDecimal(dr["SO_TIEN_PHI"]);
                    objSOTK.BIEU_PHI = new BIEU_PHI_DTO();
                    objSOTK.BIEU_PHI.HTHUC_BTHANG = dr["HTHUC_BTHANG"].ToString();
                    objSOTK.BIEU_PHI.ID_BPHI = Convert.ToInt32(dr["ID_BPHI"]);
                    objSOTK.BIEU_PHI.LOAI_BPHI = dr["LOAI_BPHI"].ToString();
                    objSOTK.BIEU_PHI.LOAI_TIEN = dr["MA_LOAI_TIEN"].ToString();
                    objSOTK.BIEU_PHI.MA_BPHI = dr["MA_BPHI"].ToString();
                    objSOTK.BIEU_PHI.NGAY_ADUNG = dr["NGAY_ADUNG"].ToString();
                    if (dr["NGAY_HHAN"] != DBNull.Value)
                        objSOTK.BIEU_PHI.NGAY_HHAN = dr["NGAY_HHAN"].ToString();
                    objSOTK.BIEU_PHI.TCHAT_BPHI = dr["TCHAT_BPHI"].ToString();
                    objSOTK.BIEU_PHI.TEN_BPHI = dr["TEN_BPHI"].ToString();
                    objSOTK.BIEU_PHI.TY_LE_VAT = Convert.ToDecimal(dr["TY_LE_VAT"]);
                    GetThongTinBieuPhiCTiet();
                    objSOTK.BIEU_PHI.DSACH_BPHI_CT = lstBieuPhi.ToArray();
                    telnumSoTienTK.Value = Convert.ToDouble(objSOTK.SO_TIEN_THU_TK);
                    cmbTienTeTK.SelectedIndex = lstSourceTienTe.IndexOf(lstSourceTienTe.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(objSOTK.LOAI_TIEN_GN)));
                    telSoTienQuyDoiTK.Value = Convert.ToDouble(objSOTK.SO_TIEN_THU_TK);
                    cmbHinhThucToanTK.SelectedIndex = lstSourceHinhThucToan.IndexOf(lstSourceHinhThucToan.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(objSOTK.HINH_THUC_TTOAN)));
                    telTienMatTK.Value = Convert.ToDouble(objSOTK.SO_TIEN_MAT);
                    txtSoTaiKhoanTK.Text = objSOTK.SO_TKHOAN_CK;
                    telChuyenKhoanTK.Value = Convert.ToDouble(objSOTK.SO_TIEN_CK);
                    txtPhiMoSo.Text = objSOTK.MA_PHI_MO_SO;
                    telSoTienPhi.Value = (double)objSOTK.PHI_MO_SO;
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        void SetTabThongKiemSoat(DataSet ds)
        {
            try
            {
                if (!LObject.IsNullOrEmpty(ds) && ds.Tables.Count > 1)
                {
                    DataTable dt = ds.Tables["TTIN_CHUNG"];
                    if (dt.Rows.Count > 0)
                    {
                        txtNguoiLap.Text = dt.Rows[0]["NGUOI_NHAP"].ToString();
                        teldtNgayNhap.Value = LDateTime.StringToDate(dt.Rows[0]["NGAY_NHAP"].ToString(), ApplicationConstant.defaultDateTimeFormat);
                        txtTrangThai.Text = BusinessConstant.layNgonNguSuDung(dt.Rows[0]["TTHAI_BGHI"].ToString());
                        txtNguoiCapNhat.Text = dt.Rows[0]["NGUOI_CNHAT"].ToString();
                        if (!dt.Rows[0]["NGAY_CNHAT"].ToString().IsNullOrEmptyOrSpace())
                            teldtNgayCNhat.Value = LDateTime.StringToDate(dt.Rows[0]["NGAY_CNHAT"].ToString(), ApplicationConstant.defaultDateTimeFormat);
                    }
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        bool getThongTinKhachHang(int id, string maKHang)
        {
            bool bResutl = true;
            try
            {
                DataSet ds = new KhachHangProcess().getThongTinCoBanKHTheoMa(id, maKHang, 0);
                if (ds.Tables[0].Rows.Count == 1)
                {
                    idKhachHang = Convert.ToInt32(ds.Tables[0].Rows[0]["ID"]);
                    maKhangHang = ds.Tables[0].Rows[0]["MA_KHANG"].ToString();
                    tenKhachHang = ds.Tables[0].Rows[0]["TEN_KHANG"].ToString();
                    soGTLQ = ds.Tables[0].Rows[0]["DD_GTLQ_SO"].ToString();
                    tenNhom = ds.Tables[0].Rows[0]["TEN_CUM"].ToString() + "-" + ds.Tables[0].Rows[0]["TEN_NHOM"].ToString();
                }
            }
            catch (Exception ex)
            {
                bResutl = false;
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
            }
            return bResutl;
        }

        void ResetForm()
        {
            txtSoGiaoDich.Text = "";
            iDGDich = 0;
            obj = new TDTD_GIAI_NGAN();
            objKUOC = new THONG_TIN_KHE_UOC_GNGAN();
            objSOTK = new THONG_TIN_SO_TKIEM_GNGAN();
            teldtNgayGiaoDich.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);
            txtDienGiai.Text = "";
            txtSoKheUoc.Text = "";
            txtTenKHang.Text = "";
            txtSoKheUoc.Tag = null;
            txtNgayNhanNo.Value = null;
            telnumSoTienGiaiNgan.Value = 0;
            telnumLaiSuat.Value = 0;
            telnumGiaiNgan.Value = 0;
            telSoTienQuyDoi.Value = 0;
            cmbHinhThucToan.SelectedIndex = 0;
            telTienMat.Value = 0;
            txtSoTaiKhoan.Text = "";
            txtSoTaiKhoan.Tag = null;
            telChuyenKhoan.Value = 0;
            telnumSoTienTK.Value = 0;
            telSoTienQuyDoiTK.Value = 0;
            telTienMatTK.Value = 0;
            txtSoTaiKhoanTK.Text = "";
            txtSoTaiKhoanTK.Tag = null;
            telChuyenKhoanTK.Value = 0;
            teldtNgayNhap.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);
            txtNguoiLap.Text = ClientInformation.TenDangNhap;
            teldtNgayCNhat.Value = null;
            txtNguoiCapNhat.Text = "";

            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.THEM, "", mnuMain, DatabaseConstant.Function.TDTD_GIAI_NGAN);
        }

        void beforeModify()
        {
            action = DatabaseConstant.Action.SUA;
            SetEnabledAllControls(true);
            OnModify();
        }

        private void SetEnabledAllControls(bool enable)
        {
            grbThongTinGiaoDich.IsEnabled = enable;
            grbThongTinKheUoc.IsEnabled = enable;
            grbThongTinGiaiNgan.IsEnabled = enable;
            grbThongTinTietKiem.IsEnabled = enable;
        }

        private void Release()
        {
            // Yêu cầu Unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
            DatabaseConstant.Function.TDTD_GIAI_NGAN,
            DatabaseConstant.Table.KT_GIAO_DICH,
            DatabaseConstant.Action.SUA,
            lstId);
        }

        void OnModify()
        {
            UtilitiesProcess process = new UtilitiesProcess();
            bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
            DatabaseConstant.Function.TDTD_GIAI_NGAN,
            DatabaseConstant.Table.KT_GIAO_DICH,
            action,
            lstId);
            SetEnabledAllControls(true);
            CommonFunction.RefreshButton(Toolbar, action, TThaiNVu, mnuMain, DatabaseConstant.Function.TDTD_GIAI_NGAN);
        }

        bool Validation()
        {
            bool bReturn = true;
            if (txtSoKheUoc.Tag.IsNullOrEmpty() || txtSoKheUoc.Tag.ToString().IsNullOrEmptyOrSpace() || obj.IsNullOrEmpty() || obj.ID_KUOCVM <= 0)
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
                iret = new TinDungTDProcess().GiaiNgan(DatabaseConstant.Action.THEM,ref obj, ref lstResponseDetail);
            else
                iret = new TinDungTDProcess().GiaiNgan(DatabaseConstant.Action.SUA, ref obj, ref lstResponseDetail);
            AfterSave(lstResponseDetail, iret);
        }

        void AfterSave(List<ClientResponseDetail> lstResponseDetail, int iret)
        {

            if (iret > 0)
            {
                action = DatabaseConstant.Action.XEM;
                SetInfomation();
                // Yêu cầu Unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDTD_GIAI_NGAN,
                DatabaseConstant.Table.KT_GIAO_DICH,
                DatabaseConstant.Action.SUA,
                lstId);
                if (OnSavingCompleted != null)
                    OnSavingCompleted(null, EventArgs.Empty);
                if ((bool)cbMultiAdd.IsChecked)
                    ResetForm();
            }
            CommonFunction.ThongBaoKetQua(lstResponseDetail);
        }

        void AfterDelete(string sSoGiaoDich, List<ClientResponseDetail> ResponseDetail, int iret)
        {
            CommonFunction.ThongBaoKetQua(ResponseDetail);
            // Yêu cầu Unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
            DatabaseConstant.Function.TDTD_GIAI_NGAN,
            DatabaseConstant.Table.KT_GIAO_DICH,
            DatabaseConstant.Action.XOA,
            lstId);
            if (OnSavingCompleted != null)
                OnSavingCompleted(null, EventArgs.Empty);
            if (iret < 1)
                SetInfomation();
            else
                ResetForm();
        }

        void OnDelete()
        {
            int iret = 0;
            List<ClientResponseDetail> ResponseDetail = new List<ClientResponseDetail>();
            iret = new TinDungTDProcess().GiaiNgan(DatabaseConstant.Action.XOA, ref obj, ref ResponseDetail);
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
                        DatabaseConstant.Function.TDTD_GIAI_NGAN,
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
                DatabaseConstant.Function.TDTD_GIAI_NGAN,
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
            DatabaseConstant.Function.TDTD_GIAI_NGAN,
            DatabaseConstant.Table.KT_GIAO_DICH,
            DatabaseConstant.Action.DUYET,
            lstId);
            iDGDich = lstId.FirstOrDefault();
            action = DatabaseConstant.Action.XEM;
            if (OnSavingCompleted != null)
                OnSavingCompleted(null, EventArgs.Empty);
            SetDataForm(sSoGiaoDich);
        }

        void OnApprove()
        {
            int iret = 0;
            List<ClientResponseDetail> ResponseDetail = new List<ClientResponseDetail>();
            iret = new TinDungTDProcess().GiaiNgan(DatabaseConstant.Action.DUYET, ref obj, ref ResponseDetail);
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
                        DatabaseConstant.Function.TDTD_GIAI_NGAN,
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
                DatabaseConstant.Function.TDTD_GIAI_NGAN,
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
            DatabaseConstant.Function.TDTD_GIAI_NGAN,
            DatabaseConstant.Table.KT_GIAO_DICH,
            DatabaseConstant.Action.TU_CHOI_DUYET,
            lstId);
            iDGDich = lstId.FirstOrDefault();
            action = DatabaseConstant.Action.XEM;
            if (OnSavingCompleted != null)
                OnSavingCompleted(null, EventArgs.Empty);
            SetDataForm(sSoGiaoDich);
        }

        void OnRefuse()
        {
            int iret = 0;
            List<ClientResponseDetail> ResponseDetail = new List<ClientResponseDetail>();
            iret = new TinDungTDProcess().GiaiNgan(DatabaseConstant.Action.TU_CHOI_DUYET, ref obj, ref ResponseDetail);
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
                        DatabaseConstant.Function.TDTD_GIAI_NGAN,
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
                DatabaseConstant.Function.TDTD_GIAI_NGAN,
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
            DatabaseConstant.Function.TDTD_GIAI_NGAN,
            DatabaseConstant.Table.KT_GIAO_DICH,
            DatabaseConstant.Action.THOAI_DUYET,
            lstId);
            iDGDich = lstId.FirstOrDefault();
            action = DatabaseConstant.Action.XEM;
            if (OnSavingCompleted != null)
                OnSavingCompleted(null, EventArgs.Empty);
            SetDataForm(sSoGiaoDich);
        }

        void OnCancel()
        {
            int iret = 0;
            List<ClientResponseDetail> ResponseDetail = new List<ClientResponseDetail>();
            iret = new TinDungTDProcess().GiaiNgan(DatabaseConstant.Action.THOAI_DUYET, ref obj, ref ResponseDetail);
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
                        DatabaseConstant.Function.TDTD_GIAI_NGAN,
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
                DatabaseConstant.Function.TDTD_GIAI_NGAN,
                DatabaseConstant.Table.KT_GIAO_DICH,
                DatabaseConstant.Action.THOAI_DUYET,
                lstId);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void OnPrint()
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
                objGIAO_DICH_BASE.ChucNang = DatabaseConstant.Function.TDTD_GIAI_NGAN;

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
        /// Xem báo cáo
        /// </summary>
        private void OnPreviewDonVayVon()
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
                objGIAO_DICH_BASE.ChucNang = DatabaseConstant.Function.TDTD_GIAI_NGAN;

                GDKT_GIAO_DICH objGDKT_GIAO_DICH = new GDKT_GIAO_DICH();
                objGDKT_GIAO_DICH.MaGiaoDich = txtSoGiaoDich.Text;

                doiTuongBaoCao.objGIAO_DICH_BASE = objGIAO_DICH_BASE;
                doiTuongBaoCao.objGDKT_GIAO_DICH = objGDKT_GIAO_DICH;
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
                lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(obj.TTHAI_NVU);
                action = DatabaseConstant.Action.XEM;
                TThaiNVu = obj.TTHAI_NVU;
                iDGDich = obj.ID_GDICH;
                txtSoGiaoDich.Text = obj.MA_GDICH;
                CommonFunction.RefreshButton(Toolbar, action, TThaiNVu, mnuMain, DatabaseConstant.Function.TDTD_GIAI_NGAN);
                SetEnabledAllControls(false);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void GetThongTinBieuPhiCTiet()
        {
            DataSet ds = new PhiProcess().GetPhiByID(objSOTK.BIEU_PHI.ID_BPHI);
            DataTable dt = ds.Tables[1];
            lstBieuPhi = new List<BIEU_PHI_CTIET_DTO>();
            foreach (DataRow dtr in dt.Rows)
            {
                BIEU_PHI_CTIET_DTO objBieuPhiCT = new BIEU_PHI_CTIET_DTO();
                objBieuPhiCT.ID_BPHI = objSOTK.BIEU_PHI.ID_BPHI;
                objBieuPhiCT.LOAI_BPHI = dtr["LOAI_BPHI"].ToString();
                objBieuPhiCT.MA_BPHI = dtr["MA_BPHI"].ToString();
                if (dtr["SO_TIEN"] != DBNull.Value)
                    objBieuPhiCT.SO_TIEN_TINH_PHI = Convert.ToDecimal(dtr["SO_TIEN"]);
                if (dtr["SO_TIEN_PHI"] != DBNull.Value)
                    objBieuPhiCT.SO_TIEN_PHI = Convert.ToDecimal(dtr["SO_TIEN_PHI"]);
                if (dtr["STIEN_PHI_TDA"] != DBNull.Value)
                    objBieuPhiCT.SO_TIEN_TDA = Convert.ToDecimal(dtr["STIEN_PHI_TDA"]);
                if (dtr["STIEN_PHI_TTHIEU"] != DBNull.Value)
                    objBieuPhiCT.SO_TIEN_TTHIEU = Convert.ToDecimal(dtr["STIEN_PHI_TTHIEU"]);
                if (dtr["TY_LE_PHI"] != DBNull.Value)
                    objBieuPhiCT.TY_LE_PHI = Convert.ToDecimal(dtr["TY_LE_PHI"]);
                if (dtr["TY_LE_VAT"] != DBNull.Value)
                    objBieuPhiCT.TY_LE_VAT = Convert.ToDecimal(dtr["TY_LE_VAT"]);
                lstBieuPhi.Add(objBieuPhiCT);

            }
            objSOTK.BIEU_PHI.DSACH_BPHI_CT = lstBieuPhi.ToArray();
        }

        private void TinhPhiTraTruoc()
        {
            decimal soDu = objSOTK.SO_TIEN_THU_TK;
            decimal soTienPhi = 0;
            decimal tyLe = 0;
            decimal soTien = 0;
            decimal soTienTThieu = 0;
            decimal soTienTDa = 0;
            lstBieuPhi = objSOTK.BIEU_PHI.DSACH_BPHI_CT.ToList();
            soTienTThieu = lstBieuPhi.FirstOrDefault().SO_TIEN_TTHIEU;
            soTienTDa = lstBieuPhi.FirstOrDefault().SO_TIEN_TDA;

            if (objSOTK.BIEU_PHI.TCHAT_BPHI.Equals(BusinessConstant.TCHAT_BPHI.DTH.layGiaTri()))
            {
                if (objSOTK.BIEU_PHI.HTHUC_BTHANG.Equals(BusinessConstant.HTHUC_BTHANG_PHI.TY_LE.layGiaTri()))
                {
                    tyLe = lstBieuPhi.FirstOrDefault().TY_LE_PHI;
                }
                else if (objSOTK.BIEU_PHI.HTHUC_BTHANG.Equals(BusinessConstant.HTHUC_BTHANG_PHI.SO_TIEN.layGiaTri()))
                {
                    soTien = lstBieuPhi.FirstOrDefault().SO_TIEN_PHI;
                }
                else if (objSOTK.BIEU_PHI.HTHUC_BTHANG.Equals(BusinessConstant.HTHUC_BTHANG_PHI.STIEN_TLE.layGiaTri()))
                {
                    tyLe = lstBieuPhi.FirstOrDefault().TY_LE_PHI;
                    soTien = lstBieuPhi.FirstOrDefault().SO_TIEN_PHI;
                }
            }
            else
            {
            }
            if (objSOTK.BIEU_PHI.HTHUC_BTHANG.Equals(BusinessConstant.HTHUC_BTHANG_PHI.TY_LE.layGiaTri()))
            {
                soTienPhi = soDu * (tyLe / 100);
            }
            else if (objSOTK.BIEU_PHI.HTHUC_BTHANG.Equals(BusinessConstant.HTHUC_BTHANG_PHI.SO_TIEN.layGiaTri()))
            {
                soTienPhi = soTien;
            }
            else if (objSOTK.BIEU_PHI.HTHUC_BTHANG.Equals(BusinessConstant.HTHUC_BTHANG_PHI.STIEN_TLE.layGiaTri()))
            {

            }
            if (soTienPhi < soTienTThieu)
                soTienPhi = soTienTThieu;
            if (soTienPhi > soTienTDa)
                soTienPhi = soTienTDa;
            objSOTK.PHI_MO_SO = soTienPhi;
            objSOTK.MA_PHI_MO_SO = objSOTK.BIEU_PHI.MA_BPHI;
        }
        #endregion
    }
}
