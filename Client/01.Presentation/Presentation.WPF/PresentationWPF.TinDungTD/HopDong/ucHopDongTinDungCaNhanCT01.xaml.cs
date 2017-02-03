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
using Utilities.Common;
using Microsoft.Windows.Controls.Ribbon;
using Presentation.Process.Common;
using PresentationWPF.CustomControl;
using Presentation.Process;
using Presentation.Process.PopupServiceRef;
using System.Data;
using Presentation.Process.TinDungTDServiceRef;
using PresentationWPF.BaoCao.DungChung;
using Presentation.Process.BaoCaoServiceRef;

namespace PresentationWPF.TinDungTD
{
    /// <summary>
    /// Interaction logic for ucHopDongTinDungCaNhanCT01.xaml
    /// </summary>
    public partial class ucHopDongTinDungCaNhanCT01 : UserControl
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
        List<int> lstId = new List<int>();
        public DatabaseConstant.Action action = DatabaseConstant.Action.THEM;

        List<AutoCompleteEntry> lstTienTe = new List<AutoCompleteEntry>();

        List<AutoCompleteEntry> lstLuongCoBanBLanhQLoan = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstThuNhapKhacBLanhQLoan = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstTongThuNhapBLanhQLoan = new List<AutoCompleteEntry>();

        List<AutoCompleteEntry> lstThoiGianVay = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstThoiGianRut = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstTanSuatLSuat = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstHinhThucTraGoc = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstHinhThucTraLai = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceMoiQuanHe = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceDanToc = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceLoaiHinhCuTru = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceGioiTinh = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceTinhTrangHonNhan = new List<AutoCompleteEntry>();
        List<DataRow> lstPopup = new List<DataRow>();
        public void LayDuLieuTuPopup(List<DataRow> lst)
        {
            lstPopup = lst;
        }
        TD_HDTD_TD obj = null;
        TDTD_HDTD objHDTD = null;
        List<TDTD_HDTD> lst = null;
        string TThaiNVu = "";
        int idHDTDVM = 0;
        string maHDTDVM = "";
        #endregion

        #region Khoi tao
        public ucHopDongTinDungCaNhanCT01()
        {
            InitializeComponent();
            KhoiTaoComboBox();
            ResetForm();
            InitEventHanler();
        }

        private void KhoiTaoComboBox()
        {
            List<COMBOBOX_DTO> lst = new List<COMBOBOX_DTO>();

            COMBOBOX_DTO obj = new COMBOBOX_DTO();
            List<string> lstDieuKien = new List<string>();

            obj = new COMBOBOX_DTO();
            lstDieuKien = new List<string>();
            obj.combobox = cmbTienTe;
            obj.maChon = ClientInformation.MaDongNoiTe;
            obj.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_TIENTE.getValue();
            obj.lstSource = lstTienTe;
            obj.lstDieuKien = lstDieuKien;
            lst.Add(obj);

            obj = new COMBOBOX_DTO();
            lstDieuKien = new List<string>();
            obj.combobox = cmbLuongCoBanBLanhQLoan;
            obj.maChon = null;
            obj.maCSo = null;
            obj.lstSource = lstLuongCoBanBLanhQLoan;
            obj.lstDieuKien = lstDieuKien;
            lst.Add(obj);

            obj = new COMBOBOX_DTO();
            lstDieuKien = new List<string>();
            obj.combobox = cmbThuNhapKhacBLanhQLoan;
            obj.maChon = null;
            obj.maCSo = null;
            obj.lstSource = lstThuNhapKhacBLanhQLoan;
            obj.lstDieuKien = lstDieuKien;
            lst.Add(obj);

            obj = new COMBOBOX_DTO();
            lstDieuKien = new List<string>();
            obj.combobox = cmbTongThuNhapBLanhQLoan;
            obj.maChon = null;
            obj.maCSo = null;
            obj.lstSource = lstTongThuNhapBLanhQLoan;
            obj.lstDieuKien = lstDieuKien;
            lst.Add(obj);

            obj = new COMBOBOX_DTO();
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.DVI_TINH_KY_HAN.getValue());
            obj.combobox = cmbTGianVay;
            obj.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();
            obj.lstSource = lstThoiGianVay;
            obj.lstDieuKien = lstDieuKien;
            lst.Add(obj);

            obj = new COMBOBOX_DTO();
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.DVI_TINH_KY_HAN.getValue());
            obj.combobox = cmbTGianRut;
            obj.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();
            obj.lstSource = lstThoiGianRut;
            obj.lstDieuKien = lstDieuKien;
            obj.maChon = "NGAY";
            lst.Add(obj);

            obj = new COMBOBOX_DTO();
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.DVI_TINH_LSUAT.getValue());
            obj.combobox = cmbTHLaiSuat;
            obj.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();
            obj.lstSource = lstTanSuatLSuat;
            obj.lstDieuKien = lstDieuKien;
            lst.Add(obj);

            obj = new COMBOBOX_DTO();
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.DVI_TINH_LSUAT.getValue());
            obj.combobox = cmbQHLaiSuat;
            obj.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();
            obj.lstSource = lstTanSuatLSuat;
            obj.lstDieuKien = lstDieuKien;
            lst.Add(obj);

            obj = new COMBOBOX_DTO();
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.HINH_THUC_TRA_GOC.getValue());
            obj.combobox = cmbHinhThucTraGoc;
            obj.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();
            obj.lstSource = lstHinhThucTraGoc;
            obj.lstDieuKien = lstDieuKien;
            lst.Add(obj);

            obj = new COMBOBOX_DTO();
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.HINH_THUC_TRA_LAI.getValue());
            obj.combobox = cmbHinhThucTraLai;
            obj.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();
            obj.lstSource = lstHinhThucTraLai;
            obj.lstDieuKien = lstDieuKien;
            lst.Add(obj);

            obj = new COMBOBOX_DTO();
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.MOI_QUAN_HE.getValue());
            obj.combobox = cmbQuanHeNguoiThuaKe;
            obj.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();
            obj.lstSource = lstSourceMoiQuanHe;
            obj.lstDieuKien = lstDieuKien;
            lst.Add(obj);

            obj = new COMBOBOX_DTO();
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.MOI_QUAN_HE.getValue());
            obj.combobox = cmbQuanHeNguoiBaoLanh;
            obj.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();
            obj.lstSource = lstSourceMoiQuanHe;
            obj.lstDieuKien = lstDieuKien;
            lst.Add(obj);

            obj = new COMBOBOX_DTO();
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.MOI_QUAN_HE.getValue());
            obj.combobox = cmbQuanHeNguoiBaoLanhQLoan;
            obj.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();
            obj.lstSource = lstSourceMoiQuanHe;
            obj.lstDieuKien = lstDieuKien;
            lst.Add(obj);

            obj = new COMBOBOX_DTO();
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.MOI_QUAN_HE.getValue());
            obj.combobox = cmbQuanHeNguoiThamChieu;
            obj.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();
            obj.lstSource = lstSourceMoiQuanHe;
            obj.lstDieuKien = lstDieuKien;
            lst.Add(obj);

            obj = new COMBOBOX_DTO();
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.DAN_TOC.getValue());
            obj.combobox = cmbDanTocBLanhQLoan;
            obj.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();
            obj.lstSource = lstSourceDanToc;
            obj.lstDieuKien = lstDieuKien;
            lst.Add(obj);

            obj = new COMBOBOX_DTO();
            lstDieuKien = new List<string>();
            lstDieuKien.Add("LOAI_HINH_CU_TRU");
            obj.combobox = cmbLoaiHinhCuTruBLanhQLoan;
            obj.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();
            obj.lstSource = lstSourceLoaiHinhCuTru;
            obj.lstDieuKien = lstDieuKien;
            lst.Add(obj);

            obj = new COMBOBOX_DTO();
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.GIOI_TINH.getValue());
            obj.combobox = cmbGioiTinhBLanhQLoan;
            obj.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();
            obj.lstSource = lstSourceGioiTinh;
            obj.lstDieuKien = lstDieuKien;
            lst.Add(obj);

            obj = new COMBOBOX_DTO();
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.TINH_TRANG_HON_NHAN.getValue());
            obj.combobox = cmbTinhTrangHonNhanBLanhQLoan;
            obj.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();
            obj.lstSource = lstSourceTinhTrangHonNhan;
            obj.lstDieuKien = lstDieuKien;
            lst.Add(obj);
            
            new AutoComboBox().GenAutoComboBoxTheoList(ref lst);

        }

        private void InitEventHanler()
        {
            btnSoDonVayVon.Click += new RoutedEventHandler(btnSoDonVayVon_Click);
            btnMaMucDich.Click += new RoutedEventHandler(btnMaMucDich_Click);
            btnMaCanBo.Click += new RoutedEventHandler(btnMaCanBo_Click);
            btnSoHopDongTC.Click += new RoutedEventHandler(btnSoHopDongTC_Click);
        }

        #endregion

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
            else if (strTinhNang.Equals("PreviewHDTD"))
            {
                OnPreviewHopDong();
            }
            else if (strTinhNang.Equals("PreviewPhuLuc"))
            {
                OnPreviewPhuLuc();
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
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
            { }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.NHAN_BAN)))
            { }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU_TAM)))
            { }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU)))
            { }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.BANG_KE)))
            { }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DUYET)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
            {
            }
            else if (strTinhNang.Equals("PreviewHDTD"))
            {
                OnPreviewHopDong();
            }
            else if (strTinhNang.Equals("PreviewPhuLuc"))
            {
                OnPreviewPhuLuc();
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

        #region Xu ly giao dien
        private void ResetForm()
        {
            txtSoDonVayVon.Text = "";
            teldtNgayLapDon.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);
            txtMaKhachHang.Text = "";
            txtMaKhachHang.Tag = "";
            txtTenKhachHang.Text = "";
            txtSoHopDong.Text = "";
            teldtNgayHopDong.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);
            txtMaSanPham.Text = "";
            txtMaSanPham.Tag = "";
            lblTenSanPham.Content = LLanguage.SearchResourceByKey("U.TinDungTD.ucHopDongTinDungCaNhanCT.TenSanPham");
            telnumSoTienVay.Value = 0;
            //cmbTienTe.SelectedIndex = 0;
            cmbTGianVay.SelectedIndex = 0;
            cmbTHLaiSuat.SelectedIndex = 0;
            cmbQHLaiSuat.SelectedIndex = 0;
            cmbHinhThucTraGoc.SelectedIndex = 0;
            cmbHinhThucTraLai.SelectedIndex = 0;
            telnumTGianVay.Value = 0;
            telnumTGianRut.Value = 0;
            teldtNgayHieuLuc.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);
            teldtNgayDaoHan.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);
            telnumLaiSuat.Value = 0;
            telnumQHLaiSuat.Value = 0;
            txtMaMucDich.Text = "";
            txtMaMucDich.Tag = "";
            lblTenMucDich.Content = LLanguage.SearchResourceByKey("U.TinDungTD.ucHopDongTinDungCaNhanCT.TenMucDichVay");
            txtMaCanBo.Text = "";
            txtMaCanBo.Tag = "";
            lblMaCanBo.Content = LLanguage.SearchResourceByKey("U.TinDungTD.ucHopDongTinDungCaNhanCT.TenCanBo");
            txtDienGiai.Text = "";
            obj = new TD_HDTD_TD();
            objHDTD = new TDTD_HDTD();
            idHDTDVM = 0;
            TThaiNVu = "";
            action = DatabaseConstant.Action.THEM;
            txtNguoiThuaKe.Text = "";
            txtTenNguoiThuaKe.Text = "";
            txtTenBoNguoiThuaKe.Text = "";
            txtSoCMNDNguoiThuaKe.Text = "";
            txtNgayCapCMNDNguoiThuaKe.Value = null;
            txtNoiCapCMNDNguoiThuaKe.Text = "";
            txtDiaChiNguoiThuaKe.Text = "";
            txtDienThoaiNguoiThuaKe.Text = "";
            txtDiDongNguoiThuaKe.Text = "";
            cmbQuanHeNguoiThuaKe.SelectedIndex = 0;
            txtNguoiBaoLanh.Text = "";
            txtTenNguoiBaoLanh.Text = "";
            txtTenBoNguoiBaoLanh.Text = "";
            txtSoCMNDNguoiBaoLanh.Text = "";
            txtNgayCapCMNDNguoiBaoLanh.Value = null;
            txtNoiCapCMNDNguoiBaoLanh.Text = "";
            txtDiaChiNguoiBaoLanh.Text = "";
            txtDienThoaiNguoiBaoLanh.Text = "";
            txtDiDongNguoiBaoLanh.Text = "";
            cmbQuanHeNguoiBaoLanh.SelectedIndex = 0;

            txtSoHopDongTC.Text = "";
            teldtNgayHopDongTC.Value = null;
            raddgrDSachTSDB.ItemsSource = null;
            txtDienGiaiHDTC.Text = "";
            SetGtriTong(null);
            CommonFunction.RefreshButton(Toolbar, action, TThaiNVu, mnuMain, DatabaseConstant.Function.TDTD_HOP_DONG_CA_NHAN);
            SetEnabledAllControls(true);
        }

        void btnMaCanBo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<string> lstDieuKien = new List<string>();
                lstDieuKien.Add(ClientInformation.MaDonViGiaoDich);
                lstDieuKien.Add("CHINH_THUC");
                lstDieuKien.Add("(0)");
                lstPopup = new List<DataRow>();
                PopupProcess popupProcess = new PopupProcess();
                popupProcess.getPopupInformation("POPUP_DS_HOSO_NHAN_SU_QLY", lstDieuKien);
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
                        txtMaCanBo.Text = dr["MA_NS_HO_SO"].ToString();
                        txtMaCanBo.Tag = Convert.ToInt32(dr["ID_NS_HO_SO"]);
                        lblMaCanBo.Content = dr["TEN_HO_SO"].ToString();
                        objHDTD.MA_NGUOI_QLY = dr["MA_NS_HO_SO"].ToString();
                        objHDTD.ID_NGUOI_QLY = Convert.ToInt32(dr["ID_NS_HO_SO"]);
                    }
                }
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        void btnMaMucDich_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<string> lstDieuKien = new List<string>();
                lstDieuKien.Add(DatabaseConstant.DanhMuc.MUC_DICH_VAY_VON.getValue());
                lstPopup = new List<DataRow>();
                PopupProcess popupProcess = new PopupProcess();
                popupProcess.getPopupInformation("POPUP_DS_MUC_DICH_VAY_VON", lstDieuKien);
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
                        txtMaMucDich.Text = dr["MA_DMUC"].ToString();
                        lblTenMucDich.Content = dr["MA_NNGU"].ToString();
                        objHDTD.MUC_DICH_VAY = dr["MA_DMUC"].ToString();
                    }
                }
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        void btnSoDonVayVon_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<string> lstDieuKien = new List<string>();
                lstDieuKien.Add(ClientInformation.MaDonViGiaoDich);
                lstDieuKien.Add(teldtNgayHopDong.Value.GetValueOrDefault().ToString(ApplicationConstant.defaultDateTimeFormat));
                lstPopup = new List<DataRow>();
                PopupProcess popupProcess = new PopupProcess();
                popupProcess.getPopupInformation("POPUP_DS_DON_XIN_VAY_VON_TIN_DUNG_TDUNG", lstDieuKien);
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
                        if (dr["HE_SO"] != DBNull.Value)
                            objHDTD.HE_SO = Convert.ToDecimal(dr["HE_SO"]);
                        if (dr["ID_DIABAN"] != DBNull.Value)
                            objHDTD.ID_DIABAN = Convert.ToInt32(dr["ID_DIABAN"]);
                        if (dr["ID"] != DBNull.Value)
                            objHDTD.ID_DXVV = Convert.ToInt32(dr["ID"]);
                        if (dr["ID_KHANG"] != DBNull.Value)
                            objHDTD.ID_KHANG = Convert.ToInt32(dr["ID_KHANG"]);
                        if (dr["ID_NGUOI_DTN"] != DBNull.Value)
                            objHDTD.ID_NGUOI_DTN = Convert.ToInt32(dr["ID_NGUOI_DTN"]);
                        if (dr["ID_NGUOI_QLY"] != DBNull.Value)
                            objHDTD.ID_NGUOI_QLY = Convert.ToInt32(dr["ID_NGUOI_QLY"]);
                        if (dr["ID_SAN_PHAM"] != DBNull.Value)
                            txtMaSanPham.Tag = objHDTD.ID_SAN_PHAM = Convert.ToInt32(dr["ID_SAN_PHAM"]);
                        if (dr["KHOACH_HTHUC_LAP"] != DBNull.Value)
                            objHDTD.KHOACH_HTHUC_LAP = dr["KHOACH_HTHUC_LAP"].ToString();
                        if (dr["KHOACH_NGAY_LAP"] != DBNull.Value)
                            objHDTD.KHOACH_NGAY_LAP = dr["KHOACH_NGAY_LAP"].ToString();
                        if (dr["LSUAT_BDO"] != DBNull.Value)
                            objHDTD.LSUAT_BDO = Convert.ToDecimal(dr["LSUAT_BDO"]);
                        if (dr["LSUAT_CCAU"] != DBNull.Value)
                            objHDTD.LSUAT_CCAU = Convert.ToDecimal(dr["LSUAT_CCAU"]);
                        if (dr["LSUAT_CTRA"] != DBNull.Value)
                            objHDTD.LSUAT_CTRA = Convert.ToDecimal(dr["LSUAT_CTRA"]);
                        if (dr["LSUAT_LOAI"] != DBNull.Value)
                            objHDTD.LSUAT_LOAI = dr["LSUAT_LOAI"].ToString();
                        if (dr["LSUAT_MA"] != DBNull.Value)
                            objHDTD.LSUAT_MA = dr["LSUAT_MA"].ToString();
                        if (dr["LSUAT_QHAN"] != DBNull.Value)
                        {
                            objHDTD.LSUAT_QHAN = Convert.ToDecimal(dr["LSUAT_QHAN"]);
                            telnumQHLaiSuat.Value = Convert.ToDouble(dr["LSUAT_QHAN"]);
                        }
                        if (dr["LSUAT_TSUAT"] != DBNull.Value)
                            objHDTD.LSUAT_TSUAT = Convert.ToInt32(dr["LSUAT_TSUAT"]);
                        if (dr["LSUAT_TSUAT_DVI_TINH"] != DBNull.Value)
                        {
                            objHDTD.LSUAT_TSUAT_DVI_TINH = dr["LSUAT_TSUAT_DVI_TINH"].ToString();
                            cmbTHLaiSuat.SelectedIndex = lstTanSuatLSuat.IndexOf(lstTanSuatLSuat.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(dr["LSUAT_TSUAT_DVI_TINH"].ToString())));
                            cmbQHLaiSuat.SelectedIndex = lstTanSuatLSuat.IndexOf(lstTanSuatLSuat.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(dr["LSUAT_TSUAT_DVI_TINH"].ToString())));
                        }
                        if (dr["LSUAT_VAY"] != DBNull.Value)
                        {
                            objHDTD.LSUAT_VAY = Convert.ToDecimal(dr["LSUAT_VAY"]);
                            telnumLaiSuat.Value = Convert.ToDouble(dr["LSUAT_VAY"]);
                        }
                        if (dr["MA_DIABAN"] != DBNull.Value)
                            objHDTD.MA_DIABAN = dr["MA_DIABAN"].ToString();
                        if (dr["MA_DXVV"] != DBNull.Value)
                        {
                            objHDTD.MA_DXVV = txtSoDonVayVon.Text = dr["MA_DXVV"].ToString();
                            txtSoDonVayVon.Text = objHDTD.MA_DXVV;
                        }
                        if (dr["MA_HMUC"] != DBNull.Value)
                            objHDTD.MA_HMUC = dr["MA_HMUC"].ToString();
                        if (dr["MA_KHANG"] != DBNull.Value)
                        {
                            objHDTD.MA_KHANG = dr["MA_KHANG"].ToString();
                            txtMaKhachHang.Text = dr["MA_KHANG"].ToString();
                        }
                        if (dr["ID_KHANG"] != DBNull.Value)
                        {
                            objHDTD.ID_KHANG = Convert.ToInt32(dr["ID_KHANG"]);
                            txtMaKhachHang.Tag = Convert.ToInt32(dr["ID_KHANG"]);
                        }
                        if (dr["MA_NGUOI_DTN"] != DBNull.Value)
                            objHDTD.MA_NGUOI_DTN = dr["MA_NGUOI_DTN"].ToString();
                        if (dr["MA_NGUOI_QLY"] != DBNull.Value)
                            objHDTD.MA_NGUOI_QLY = dr["MA_NGUOI_QLY"].ToString();
                        if (dr["MA_SAN_PHAM"] != DBNull.Value)
                            objHDTD.MA_SAN_PHAM = txtMaSanPham.Text = dr["MA_SAN_PHAM"].ToString();
                        if (dr["TEN_SAN_PHAM"] != DBNull.Value)
                            lblTenSanPham.Content = dr["TEN_SAN_PHAM"].ToString();
                        if (dr["MUC_DICH_VAY"] != DBNull.Value)
                            objHDTD.MUC_DICH_VAY = dr["MUC_DICH_VAY"].ToString();
                        if (dr["NGANH_KINH_TE"] != DBNull.Value)
                            objHDTD.NGANH_KINH_TE = dr["NGANH_KINH_TE"].ToString();
                        if (dr["NGAY_CHUYEN_QH"] != DBNull.Value)
                            objHDTD.NGAY_CHUYEN_QH = dr["NGAY_CHUYEN_QH"].ToString();
                        if (dr["NGAY_DAO_HAN"] != DBNull.Value)
                            objHDTD.NGAY_DAO_HAN = dr["NGAY_DAO_HAN"].ToString();
                        if (dr["NGAY_GIA_HAN"] != DBNull.Value)
                            objHDTD.NGAY_GIA_HAN = dr["NGAY_GIA_HAN"].ToString();
                        if (dr["NGAY_HD"] != DBNull.Value)
                        {
                            objHDTD.NGAY_LAP_HD = dr["NGAY_HD"].ToString();
                            teldtNgayLapDon.Value = LDateTime.StringToDate(dr["NGAY_HD"].ToString(), ApplicationConstant.defaultDateTimeFormat);
                            teldtNgayHopDong.Value = LDateTime.StringToDate(dr["NGAY_HD"].ToString(), ApplicationConstant.defaultDateTimeFormat);
                        }
                        if (dr["PHI_MO_HD"] != DBNull.Value)
                            objHDTD.PHI_MO_HD = Convert.ToDecimal(dr["PHI_MO_HD"]);
                        if (dr["PHUONG_THUC_VAY"] != DBNull.Value)
                            objHDTD.PHUONG_THUC_VAY = dr["PHUONG_THUC_VAY"].ToString();
                        objHDTD.SO_GDICH = "";
                        if (dr["SO_TIEN_CAN"] != DBNull.Value)
                            objHDTD.SO_TIEN_CAN = Convert.ToDecimal(dr["SO_TIEN_CAN"]);
                        if (dr["SO_TIEN_GOC_MOI_KY"] != DBNull.Value)
                            objHDTD.SO_TIEN_GOC_MOI_KY = Convert.ToDecimal(dr["SO_TIEN_GOC_MOI_KY"]);
                        if (dr["SO_TIEN_LAI_MOI_KY"] != DBNull.Value)
                            objHDTD.SO_TIEN_LAI_MOI_KY = Convert.ToDecimal(dr["SO_TIEN_LAI_MOI_KY"]);
                        if (dr["SO_TIEN_MOI_KY"] != DBNull.Value)
                            objHDTD.SO_TIEN_MOI_KY = Convert.ToDecimal(dr["SO_TIEN_MOI_KY"]);
                        if (dr["SO_TIEN_TKBB"] != DBNull.Value)
                            objHDTD.SO_TIEN_TKBB = Convert.ToDecimal(dr["SO_TIEN_TKBB"]);
                        if (dr["SO_TIEN_TU_CO"] != DBNull.Value)
                            objHDTD.SO_TIEN_TU_CO = Convert.ToDecimal(dr["SO_TIEN_TU_CO"]);
                        if (dr["SO_TIEN_VAY"] != DBNull.Value)
                        {
                            objHDTD.SO_TIEN_VAY = Convert.ToDecimal(dr["SO_TIEN_VAY"]);
                            telnumSoTienVay.Value = Convert.ToDouble(dr["SO_TIEN_VAY"]);
                        }
                        if (dr["SO_TIEN_XIN_VAY"] != DBNull.Value)
                            objHDTD.SO_TIEN_XIN_VAY = Convert.ToDecimal(dr["SO_TIEN_XIN_VAY"]);
                        if (dr["SO_TKHOAN_NHAN_NO"] != DBNull.Value)
                            objHDTD.SO_TKHOAN_NHAN_NO = dr["SO_TKHOAN_NHAN_NO"].ToString();
                        if (dr["TGIAN_VAY"] != DBNull.Value)
                        {
                            objHDTD.TGIAN_VAY = Convert.ToInt32(dr["TGIAN_VAY"]);
                            telnumTGianVay.Value = Convert.ToDouble(dr["TGIAN_VAY"]);
                        }
                        if (dr["TGIAN_VAY_DVI_TINH"] != DBNull.Value)
                        {
                            objHDTD.TGIAN_VAY_DVI_TINH = dr["TGIAN_VAY_DVI_TINH"].ToString();
                            cmbTGianVay.SelectedIndex = lstThoiGianVay.IndexOf(lstThoiGianVay.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(dr["TGIAN_VAY_DVI_TINH"].ToString())));
                        }
                        if (dr["TEN_KHANG"] != DBNull.Value)
                            txtTenKhachHang.Text = dr["TEN_KHANG"].ToString();
                        if (dr["TEN_SAN_PHAM"] != DBNull.Value)
                            lblTenSanPham.Content = dr["TEN_SAN_PHAM"].ToString();
                        if (dr["MA_LOAI_TIEN"] != DBNull.Value)
                            cmbTienTe.SelectedIndex = lstTienTe.IndexOf(lstTienTe.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(dr["MA_LOAI_TIEN"].ToString())));
                        if (dr["TRGOC_HTHUC"] != DBNull.Value)
                        {
                            objHDTD.TRGOC_HTHUC = dr["TRGOC_HTHUC"].ToString();
                            cmbHinhThucTraGoc.SelectedIndex = lstHinhThucTraGoc.IndexOf(lstHinhThucTraGoc.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(dr["TRGOC_HTHUC"].ToString())));
                        }
                        if (dr["TRLAI_HTHUC"] != DBNull.Value)
                        {
                            objHDTD.TRGOC_HTHUC = dr["TRLAI_HTHUC"].ToString();
                            cmbHinhThucTraLai.SelectedIndex = lstHinhThucTraLai.IndexOf(lstHinhThucTraLai.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(dr["TRLAI_HTHUC"].ToString())));
                        }
                        if (dr["DIEN_GIAI"] != DBNull.Value)
                        {
                            objHDTD.DIEN_GIAI = dr["DIEN_GIAI"].ToString();                            
                        }
                        SetDataDonXinVayVon(objHDTD.MA_DXVV);
                    }
                }
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        void btnSoHopDongTC_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtMaKhachHang.Text.IsNullOrEmptyOrSpace())
                    return;
                List<string> lstDieuKien = new List<string>();
                lstDieuKien.Add(ClientInformation.MaDonViQuanLy);
                lstDieuKien.Add(txtMaKhachHang.Text);
                lstPopup = new List<DataRow>();
                PopupProcess popupProcess = new PopupProcess();
                popupProcess.getPopupInformation("POPUP_DS_HDTC_TDTD", lstDieuKien);
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
                        txtSoHopDongTC.Text = dr["MA"].ToString();
                        teldtNgayHopDongTC.Value = LDateTime.StringToDate(dr["NGAY_HDTC"].ToString(), "yyyyMMdd");
                        txtDienGiaiHDTC.Text = dr["DIEN_GIAI"].ToString();

                        DataTable dt = null;
                        LDatatable.MakeParameterTable(ref dt);
                        LDatatable.AddParameter(ref dt, "@INP_IDHDTC", "string", dr["ID"].ToString());
                        DataSet dsTSDB = new TaiSanDamBaoProcess().GetHopDongTheChapTDTD(dt);

                        if (!dsTSDB.IsNullOrEmpty() && dsTSDB.Tables.Count > 1)
                        {
                            raddgrDSachTSDB.ItemsSource = null;
                            raddgrDSachTSDB.ItemsSource = dsTSDB.Tables[1].DefaultView;

                            SetGtriTong(dsTSDB.Tables[1].Rows.OfType<DataRow>().ToList());
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }
        #endregion

        #region Xu ly nghiep vu

        public void SetDataDonXinVayVon(string sSoGiaoDich)
        {
            try
            {
                DataSet ds = new DataSet();
                DataTable dtPar = null;
                LDatatable.MakeParameterTable(ref dtPar);
                LDatatable.AddParameter(ref dtPar, "@SoGiaoDich", "String", sSoGiaoDich);
                ds = new TinDungTDProcess().GetThongTinDonXinVayVonTinDungTieuDung(dtPar);
                //Dispatcher.CurrentDispatcher.DelayInvoke("SetDataForm", () =>
                //{
                //    SetTabThongTinChung(ds);
                //    Dispatcher.CurrentDispatcher.DelayInvoke("SetKeHoachSuDungVonVay", () =>
                //    {
                //        SetKeHoachSuDungVonVay(ds);
                //    }, TimeSpan.FromSeconds(0));

                //    Dispatcher.CurrentDispatcher.DelayInvoke("SetTabThongKiemSoat", () =>
                //    {
                //        SetTabThongKiemSoat(ds);
                //    }, TimeSpan.FromSeconds(0));

                //}, TimeSpan.FromSeconds(0));

                if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
                {
                    SetTabThongTinChungDonXinVay(ds);
                    SetTabThongTinBaoLanhTKe(ds);
                }
            }
            catch (Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        void SetTabThongTinChungDonXinVay(DataSet ds)
        {
            try
            {

                DataTable dt = ds.Tables["TTIN_CHUNG"];
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    DON_XIN_VAY_VON_CTIET objDXVVDTO = new DON_XIN_VAY_VON_CTIET();
                    objDXVVDTO.ID = Convert.ToInt32(dr["ID"]);
                    if (dr["CAP_LNHIEM"] != DBNull.Value)
                        objDXVVDTO.CAP_LIEN_NHIEM = dr["CAP_LNHIEM"].ToString();
                    if (dr["CAP_LNHIEM_LSUAT"] != DBNull.Value)
                        objDXVVDTO.LAI_SUAT_CAP_LIEN_NHIEM = Convert.ToDecimal(dr["CAP_LNHIEM_LSUAT"]);
                    if (dr["HE_SO"] != DBNull.Value)
                        objDXVVDTO.HE_SO = Convert.ToInt32(dr["HE_SO"]);
                    if (dr["ID_DIABAN"] != DBNull.Value)
                        objDXVVDTO.ID_DIA_BAN = Convert.ToInt32(dr["ID_DIABAN"]);
                    if (dr["ID_KHANG"] != DBNull.Value)
                        objDXVVDTO.ID_KHANG = Convert.ToInt32(dr["ID_KHANG"]);
                    if (dr["ID_NGUOI_DTN"] != DBNull.Value)
                        objDXVVDTO.ID_NGUOI_DTN = Convert.ToInt32(dr["ID_NGUOI_DTN"]);
                    if (dr["ID_NGUOI_QLY"] != DBNull.Value)
                        objDXVVDTO.ID_NGUOI_QLY = Convert.ToInt32(dr["ID_NGUOI_QLY"]);
                    if (dr["ID_SAN_PHAM"] != DBNull.Value)
                        objDXVVDTO.ID_SAN_PHAM = Convert.ToInt32(dr["ID_SAN_PHAM"]);
                    if (dr["KHOACH_HTHUC_LAP"] != DBNull.Value)
                        objDXVVDTO.KHOACH_HTHUC_LAP = Convert.ToString(dr["KHOACH_HTHUC_LAP"]);
                    if (dr["KHOACH_NGAY_LAP"] != DBNull.Value)
                        objDXVVDTO.KHOACH_NGAY_LAP = Convert.ToString(dr["KHOACH_NGAY_LAP"]);
                    if (dr["LSUAT_BDO"] != DBNull.Value)
                        objDXVVDTO.BIEN_DO_LAI_SUAT = Convert.ToDecimal(dr["LSUAT_BDO"]);
                    if (dr["LSUAT_CCAU"] != DBNull.Value)
                        objDXVVDTO.LAI_SUAT_CO_CAU = Convert.ToDecimal(dr["LSUAT_CCAU"]);
                    if (dr["LSUAT_CTRA"] != DBNull.Value)
                        objDXVVDTO.LAI_SUAT_CHAM_TRA = Convert.ToDecimal(dr["LSUAT_CTRA"]);
                    if (dr["LSUAT_LOAI"] != DBNull.Value)
                        objDXVVDTO.LOAI_LAI_SUAT = Convert.ToString(dr["LSUAT_LOAI"]);
                    if (dr["LSUAT_MA"] != DBNull.Value)
                        objDXVVDTO.MA_LSUAT = Convert.ToString(dr["LSUAT_MA"]);
                    if (dr["LSUAT_QHAN"] != DBNull.Value)
                        objDXVVDTO.LAI_SUAT_QUA_HAN = Convert.ToDecimal(dr["LSUAT_QHAN"]);
                    if (dr["LSUAT_TSUAT"] != DBNull.Value)
                        objDXVVDTO.TAN_SUAT_LAI_SUAT = Convert.ToInt32(dr["LSUAT_TSUAT"]);
                    if (dr["LSUAT_TSUAT_DVI_TINH"] != DBNull.Value)
                        objDXVVDTO.DVT_TAN_SUAT = Convert.ToString(dr["LSUAT_TSUAT_DVI_TINH"]);
                    if (dr["LSUAT_VAY"] != DBNull.Value)
                        objDXVVDTO.LAI_SUAT_VAY = Convert.ToDecimal(dr["LSUAT_VAY"]);
                    if (dr["MA_DIABAN"] != DBNull.Value)
                        objDXVVDTO.MA_DIA_BAN = Convert.ToString(dr["MA_DIABAN"]);
                    if (dr["MA_DVI_QLY"] != DBNull.Value)
                        objDXVVDTO.MA_DVI_QLY = Convert.ToString(dr["MA_DVI_QLY"]);
                    if (dr["MA_DVI_TAO"] != DBNull.Value)
                        objDXVVDTO.MA_DVI_TAO = Convert.ToString(dr["MA_DVI_TAO"]);
                    if (dr["MA_DXVV"] != DBNull.Value)
                        objDXVVDTO.MA_DXVV = Convert.ToString(dr["MA_DXVV"]);
                    if (dr["MA_HMUC"] != DBNull.Value)
                        objDXVVDTO.MA_HMUC = Convert.ToString(dr["MA_HMUC"]);
                    if (dr["MA_KHANG"] != DBNull.Value)
                        objDXVVDTO.MA_KHANG = Convert.ToString(dr["MA_KHANG"]);
                    if (dr["MA_NGUOI_DTN"] != DBNull.Value)
                        objDXVVDTO.MA_NGUOI_DTN = Convert.ToString(dr["MA_NGUOI_DTN"]);
                    if (dr["MA_NGUOI_QLY"] != DBNull.Value)
                        objDXVVDTO.MA_NGUOI_QLY = Convert.ToString(dr["MA_NGUOI_QLY"]);
                    if (dr["MA_SAN_PHAM"] != DBNull.Value)
                        objDXVVDTO.MA_SAN_PHAM = Convert.ToString(dr["MA_SAN_PHAM"]);
                    if (dr["MUC_DICH_VAY"] != DBNull.Value)
                        objDXVVDTO.MA_MUC_DICH_VAY = Convert.ToString(dr["MUC_DICH_VAY"]);
                    if (dr["NGANH_KINH_TE"] != DBNull.Value)
                        objDXVVDTO.MA_NGANH_KTE = Convert.ToString(dr["NGANH_KINH_TE"]);
                    if (dr["NGAY_CHUYEN_QH"] != DBNull.Value)
                        objDXVVDTO.NGAY_CHUYEN_QUA_HAN = Convert.ToString(dr["NGAY_CHUYEN_QH"]);
                    if (dr["NGAY_CNHAT"] != DBNull.Value)
                        objDXVVDTO.NGAY_CNHAT = Convert.ToString(dr["NGAY_CNHAT"]);
                    if (dr["NGAY_DAO_HAN"] != DBNull.Value)
                        objDXVVDTO.NGAY_DAO_HAN = Convert.ToString(dr["NGAY_DAO_HAN"]);
                    if (dr["NGAY_GIA_HAN"] != DBNull.Value)
                        objDXVVDTO.NGAY_GIA_HAN = Convert.ToString(dr["NGAY_GIA_HAN"]);
                    if (dr["NGAY_HD"] != DBNull.Value)
                        objDXVVDTO.NGAY_LAP = Convert.ToString(dr["NGAY_HD"]);
                    if (dr["NGAY_NHAP"] != DBNull.Value)
                        objDXVVDTO.NGAY_NHAP = Convert.ToString(dr["NGAY_NHAP"]);
                    if (dr["NGUOI_CNHAT"] != DBNull.Value)
                        objDXVVDTO.NGUOI_CNHAT = Convert.ToString(dr["NGUOI_CNHAT"]);
                    if (dr["NGUOI_NHAP"] != DBNull.Value)
                        objDXVVDTO.NGUOI_NHAP = Convert.ToString(dr["NGUOI_NHAP"]);
                    if (dr["PHI_MO_HD"] != DBNull.Value)
                        objDXVVDTO.PHI_MO_HOP_DONG = Convert.ToDecimal(dr["PHI_MO_HD"]);
                    if (dr["PHUONG_THUC_VAY"] != DBNull.Value)
                        objDXVVDTO.MA_PTHUC_VAY = Convert.ToString(dr["PHUONG_THUC_VAY"]);
                    if (dr["SO_GDICH"] != DBNull.Value)
                        objDXVVDTO.SO_GDICH = Convert.ToString(dr["SO_GDICH"]);
                    if (dr["SO_TIEN_CAN"] != DBNull.Value)
                        objDXVVDTO.SO_TIEN_CAN = Convert.ToDecimal(dr["SO_TIEN_CAN"]);
                    if (dr["SO_TIEN_GOC_MOI_KY"] != DBNull.Value)
                        objDXVVDTO.SO_TIEN_GOC_MOI_KY = Convert.ToDecimal(dr["SO_TIEN_GOC_MOI_KY"]);
                    if (dr["SO_TIEN_LAI_MOI_KY"] != DBNull.Value)
                        objDXVVDTO.SO_TIEN_LAI_MOI_KY = Convert.ToDecimal(dr["SO_TIEN_LAI_MOI_KY"]);
                    if (dr["SO_TIEN_MOI_KY"] != DBNull.Value)
                        objDXVVDTO.SO_TIEN_MOI_KY = Convert.ToDecimal(dr["SO_TIEN_MOI_KY"]);
                    if (dr["SO_TIEN_TKBB"] != DBNull.Value)
                        objDXVVDTO.SO_TIEN_TKBB = Convert.ToDecimal(dr["SO_TIEN_TKBB"]);
                    if (dr["SO_TIEN_TU_CO"] != DBNull.Value)
                        objDXVVDTO.SO_TIEN_CO = Convert.ToDecimal(dr["SO_TIEN_TU_CO"]);
                    if (dr["SO_TIEN_VAY"] != DBNull.Value)
                        objDXVVDTO.SO_TIEN_VAY = Convert.ToDecimal(dr["SO_TIEN_VAY"]);
                    if (dr["SO_TIEN_XIN_VAY"] != DBNull.Value)
                        objDXVVDTO.SO_TIEN_XIN_VAY = Convert.ToDecimal(dr["SO_TIEN_XIN_VAY"]);
                    if (dr["SO_TKHOAN_NHAN_NO"] != DBNull.Value)
                        objDXVVDTO.TAI_KHOAN_NHAN_NO = Convert.ToString(dr["SO_TKHOAN_NHAN_NO"]);
                    if (dr["TGIAN_VAY"] != DBNull.Value)
                        objDXVVDTO.THOI_GIAN_VAY = Convert.ToInt32(dr["TGIAN_VAY"]);
                    if (dr["TGIAN_VAY_DVI_TINH"] != DBNull.Value)
                        objDXVVDTO.DVT_THOI_GIAN_VAY = Convert.ToString(dr["TGIAN_VAY_DVI_TINH"]);
                    if (dr["TRGOC_DVI_TINH"] != DBNull.Value)
                        objDXVVDTO.DVT_SO_KY_TRA_GOC = Convert.ToString(dr["TRGOC_DVI_TINH"]);
                    if (dr["TRGOC_HTHUC"] != DBNull.Value)
                        objDXVVDTO.HINH_THUC_TRA_GOC = Convert.ToString(dr["TRGOC_HTHUC"]);
                    if (dr["TRGOC_SO_KY"] != DBNull.Value)
                        objDXVVDTO.SO_KY_TRA_GOC = Convert.ToInt32(dr["TRGOC_SO_KY"]);
                    if (dr["TRGOC_SO_TKHOAN"] != DBNull.Value)
                        objDXVVDTO.TAI_KHOAN_TRA_GOC = Convert.ToString(dr["TRGOC_SO_TKHOAN"]);
                    if (dr["TRLAI_DVI_TINH"] != DBNull.Value)
                        objDXVVDTO.DVT_SO_KY_TRA_LAI = Convert.ToString(dr["TRLAI_DVI_TINH"]);
                    if (dr["TRLAI_HTHUC"] != DBNull.Value)
                        objDXVVDTO.HINH_THUC_TRA_LAI = Convert.ToString(dr["TRLAI_HTHUC"]);
                    if (dr["TRLAI_SO_KY"] != DBNull.Value)
                        objDXVVDTO.SO_KY_TRA_LAI = Convert.ToInt32(dr["TRLAI_SO_KY"]);
                    if (dr["TRLAI_SO_TKHOAN"] != DBNull.Value)
                        objDXVVDTO.TAI_KHOAN_TRA_LAI = Convert.ToString(dr["TRLAI_SO_TKHOAN"]);
                    if (dr["TTHAI_BGHI"] != DBNull.Value)
                        objDXVVDTO.TTHAI_BGHI = Convert.ToString(dr["TTHAI_BGHI"]);
                    if (dr["TTHAI_GIAI_NGAN"] != DBNull.Value)
                        objDXVVDTO.TTHAI_GIAI_NGAN = Convert.ToString(dr["TTHAI_GIAI_NGAN"]);
                    if (dr["TTHAI_LY_DO"] != DBNull.Value)
                        objDXVVDTO.TTHAI_LY_DO = Convert.ToString(dr["TTHAI_LY_DO"]);
                    if (dr["TTHAI_NVU"] != DBNull.Value)
                        objDXVVDTO.TTHAI_NVU = Convert.ToString(dr["TTHAI_NVU"]);
                    if (dr["DIEN_GIAI"] != DBNull.Value)
                        objDXVVDTO.DIEN_GIAI = Convert.ToString(dr["DIEN_GIAI"]);

                    if (dr["LOAI_DXVV"].ToString() == "TDTD_QLOAN")
                    {
                        titemNguoiDongTrachNhiem.Visibility = Visibility.Collapsed;
                        titemThongTinThamChieuBLanh.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        titemNguoiDongTrachNhiem.Visibility = Visibility.Visible;
                        titemThongTinThamChieuBLanh.Visibility = Visibility.Collapsed;
                    }
                    
                    txtSoDonVayVon.Text = objDXVVDTO.MA_DXVV;
                    teldtNgayLapDon.Value = LDateTime.StringToDate(objDXVVDTO.NGAY_LAP, ApplicationConstant.defaultDateTimeFormat);
                    txtMaKhachHang.Text = objDXVVDTO.MA_KHANG;
                    txtMaKhachHang.Tag = Convert.ToInt32(dr["ID_KHANG"]);
                    txtTenKhachHang.Text = dr["TEN_KHANG"].ToString();
                    //txtMaNhom.Text = objDXVVDTO.MA_DIA_BAN;
                    //txtMaNhom.Tag = objDXVVDTO.ID_DIA_BAN;
                    //txtTenNhomTruong.Text = dr["TEN_NHOM_TRUONG"].ToString();
                    //txtSoCMND.Text = dr["DD_SO_GTLQ"].ToString();
                    //txtThonAp.Text = dr["TEN_CUM"].ToString();
                    //txtPhuongXa.Text = dr["TEN_KVUC"].ToString();
                    //txtQuanHuyen.Text = dr["TEN_QUAN_HUYEN"].ToString();

                    //cmbLoaiSanPham.SelectedIndex = lstSourceLoaiSanPham.IndexOf(lstSourceLoaiSanPham.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(dr["MA_LOAI_SAN_PHAM"].ToString())));
                    txtMaSanPham.Text = objDXVVDTO.MA_SAN_PHAM;
                    txtMaSanPham.Tag = objDXVVDTO.ID_SAN_PHAM;
                    lblTenSanPham.Content = dr["TEN_SAN_PHAM"].ToString();
                    telnumTGianVay.Value = objDXVVDTO.THOI_GIAN_VAY;
                    cmbTGianVay.SelectedIndex = lstThoiGianVay.IndexOf(lstThoiGianVay.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(objDXVVDTO.DVT_THOI_GIAN_VAY)));
                    //cmbTGianRut.SelectedIndex = lstThoiGianRut.IndexOf(lstThoiGianRut.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals("NGAY")));
                    telnumLaiSuat.Value = Convert.ToDouble(objDXVVDTO.LAI_SUAT_VAY);
                    telnumQHLaiSuat.Value = Convert.ToDouble(objDXVVDTO.LAI_SUAT_QUA_HAN);
                    cmbTHLaiSuat.SelectedIndex = lstTanSuatLSuat.IndexOf(lstTanSuatLSuat.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(objDXVVDTO.DVT_TAN_SUAT)));
                    telnumSoTienVay.Value = Convert.ToDouble(objDXVVDTO.SO_TIEN_VAY);
                    cmbTienTe.SelectedIndex = lstTienTe.IndexOf(lstTienTe.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(dr["MA_LOAI_TIEN"].ToString())));
                    cmbHinhThucTraGoc.SelectedIndex = lstHinhThucTraGoc.IndexOf(lstHinhThucTraGoc.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(objDXVVDTO.HINH_THUC_TRA_GOC)));
                    cmbHinhThucTraLai.SelectedIndex = lstHinhThucTraLai.IndexOf(lstHinhThucTraLai.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(objDXVVDTO.HINH_THUC_TRA_LAI)));
                    
                    txtMaMucDich.Text = objDXVVDTO.MA_MUC_DICH_VAY;
                    lblTenMucDich.Content = LLanguage.SearchResourceByKey(dr["TEN_MUC_DICH_VAY"].ToString());
                    txtDienGiai.Text = objDXVVDTO.DIEN_GIAI;
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        void SetTabThongTinBaoLanhTKe(DataSet ds)
        {
            try
            {
                DataTable dt = ds.Tables["TTIN_THUAKE"];
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    txtNguoiThuaKe.Text = dr["MA_KHANG"] != null ? dr["MA_KHANG"].ToString() : "";
                    txtNguoiThuaKe.Tag = dr["ID_KHANG"] != null ? Convert.ToInt32(dr["ID_KHANG"]) : 0;
                    if (dr["TEN_NGUOI_DTN"] != DBNull.Value)
                        txtTenNguoiThuaKe.Text = dr["TEN_NGUOI_DTN"].ToString();
                    if (dr["TEN_BO"] != DBNull.Value)
                        txtTenBoNguoiThuaKe.Text = dr["TEN_BO"].ToString();
                    if (dr["GTLQ_SO"] != DBNull.Value)
                        txtSoCMNDNguoiThuaKe.Text = dr["GTLQ_SO"].ToString();
                    if (dr["NGAY_CAP"] != DBNull.Value && !dr["NGAY_CAP"].ToString().IsNullOrEmptyOrSpace() && dr["NGAY_CAP"].ToString().IsDate(ApplicationConstant.defaultDateTimeFormat))
                        txtNgayCapCMNDNguoiThuaKe.Value = LDateTime.StringToDate(dr["NGAY_CAP"].ToString(), ApplicationConstant.defaultDateTimeFormat);
                    if (dr["NOI_CAP"] != DBNull.Value)
                        txtNoiCapCMNDNguoiThuaKe.Text = dr["NOI_CAP"].ToString();
                    if (dr["DIA_CHI"] != DBNull.Value)
                        txtDiaChiNguoiThuaKe.Text = dr["DIA_CHI"].ToString();
                    if (dr["SO_DIEN_THOAI"] != DBNull.Value)
                        txtDienThoaiNguoiThuaKe.Text = dr["SO_DIEN_THOAI"].ToString();
                    if (dr["SO_DI_DONG"] != DBNull.Value)
                        txtDiDongNguoiThuaKe.Text = dr["SO_DI_DONG"].ToString();
                    if (dr["MOI_QUAN_HE"] != DBNull.Value)
                        cmbQuanHeNguoiThuaKe.SelectedIndex = lstSourceMoiQuanHe.IndexOf(lstSourceMoiQuanHe.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(dr["MOI_QUAN_HE"].ToString())));
                }

                dt = ds.Tables["TTIN_BLANH"];
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    txtNguoiBaoLanh.Text = dr["MA_KHANG"] != null ? dr["MA_KHANG"].ToString() : "";
                    txtNguoiBaoLanh.Tag = dr["ID_KHANG"] !=null ? (dr["ID_KHANG"].Equals("") ? 0 : Convert.ToInt32(dr["ID_KHANG"])) : 0;
                    if (dr["TEN_NGUOI_DTN"] != DBNull.Value)
                        txtTenNguoiBaoLanh.Text = dr["TEN_NGUOI_DTN"].ToString();
                    if (dr["TEN_BO"] != DBNull.Value)
                        txtTenBoNguoiBaoLanh.Text = dr["TEN_BO"].ToString();
                    if (dr["GTLQ_SO"] != DBNull.Value)
                        txtSoCMNDNguoiBaoLanh.Text = dr["GTLQ_SO"].ToString();
                    if (dr["NGAY_CAP"] != DBNull.Value && !dr["NGAY_CAP"].ToString().IsNullOrEmptyOrSpace() && dr["NGAY_CAP"].ToString().IsDate(ApplicationConstant.defaultDateTimeFormat))
                        txtNgayCapCMNDNguoiBaoLanh.Value = LDateTime.StringToDate(dr["NGAY_CAP"].ToString(), ApplicationConstant.defaultDateTimeFormat);
                    if (dr["NOI_CAP"] != DBNull.Value)
                        txtNoiCapCMNDNguoiBaoLanh.Text = dr["NOI_CAP"].ToString();
                    if (dr["DIA_CHI"] != DBNull.Value)
                        txtDiaChiNguoiBaoLanh.Text = dr["DIA_CHI"].ToString();
                    if (dr["SO_DIEN_THOAI"] != DBNull.Value)
                        txtDienThoaiNguoiBaoLanh.Text = dr["SO_DIEN_THOAI"].ToString();
                    if (dr["SO_DI_DONG"] != DBNull.Value)
                        txtDiDongNguoiBaoLanh.Text = dr["SO_DI_DONG"].ToString();
                    if (dr["MOI_QUAN_HE"] != DBNull.Value)
                        cmbQuanHeNguoiBaoLanh.SelectedIndex = lstSourceMoiQuanHe.IndexOf(lstSourceMoiQuanHe.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(dr["MOI_QUAN_HE"].ToString())));
                }

                dt = ds.Tables["TTIN_TCHIEU"];
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    txtNguoiThamChieu.Text = dr["MA_KHANG"].ToString();
                    if (dr["TEN_NGUOI_DTN"] != DBNull.Value)
                        txtTenNguoiThamChieu.Text = dr["TEN_NGUOI_DTN"].ToString();
                    if (dr["DIA_CHI"] != DBNull.Value)
                        txtDiaChiNguoiThamChieu.Text = dr["DIA_CHI"].ToString();
                    if (dr["SO_DIEN_THOAI"] != DBNull.Value)
                        txtDienThoaiNguoiThamChieu.Text = dr["SO_DIEN_THOAI"].ToString();
                    if (dr["SO_DI_DONG"] != DBNull.Value)
                        txtDiDongNguoiThamChieu.Text = dr["SO_DI_DONG"].ToString();
                    if (dr["MOI_QUAN_HE"] != DBNull.Value)
                        cmbQuanHeNguoiThamChieu.SelectedIndex = lstSourceMoiQuanHe.IndexOf(lstSourceMoiQuanHe.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(dr["MOI_QUAN_HE"].ToString())));
                }

                dt = ds.Tables["TTIN_BLANH_QLOAN"];
                if (dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    if (dr["MA_KHANG"] != DBNull.Value && !dr["MA_KHANG"].ToString().IsNullOrEmptyOrSpace())
                    {
                        txtNguoiBaoLanhQLoan.Text = dr["MA_KHANG"].ToString();
                    }
                    if (dr["TEN_NGUOI_DTN"] != DBNull.Value)
                        txtTenNguoiBaoLanhQLoan.Text = dr["TEN_NGUOI_DTN"].ToString();
                    if (dr["TEN_BO"] != DBNull.Value)
                        txtTenBoNguoiBaoLanhQLoan.Text = dr["TEN_BO"].ToString();
                    if (dr["GTLQ_SO"] != DBNull.Value)
                        txtSoCMNDNguoiBaoLanhQLoan.Text = dr["GTLQ_SO"].ToString();
                    if (dr["NGAY_CAP"] != DBNull.Value && !dr["NGAY_CAP"].ToString().IsNullOrEmptyOrSpace() && dr["NGAY_CAP"].ToString().IsDate(ApplicationConstant.defaultDateTimeFormat))
                        txtNgayCapCMNDNguoiBaoLanhQLoan.Value = LDateTime.StringToDate(dr["NGAY_CAP"].ToString(), ApplicationConstant.defaultDateTimeFormat);
                    if (dr["NOI_CAP"] != DBNull.Value)
                        txtNoiCapCMNDNguoiBaoLanhQLoan.Text = dr["NOI_CAP"].ToString();
                    if (dr["DIA_CHI"] != DBNull.Value)
                        txtDiaChiNguoiBaoLanhQLoan.Text = dr["DIA_CHI"].ToString();
                    if (dr["SO_DIEN_THOAI"] != DBNull.Value)
                        txtDienThoaiNguoiBaoLanhQLoan.Text = dr["SO_DIEN_THOAI"].ToString();
                    if (dr["SO_DI_DONG"] != DBNull.Value)
                        txtDiDongNguoiBaoLanhQLoan.Text = dr["SO_DI_DONG"].ToString();
                    if (dr["MOI_QUAN_HE"] != DBNull.Value)
                        cmbQuanHeNguoiBaoLanhQLoan.SelectedIndex = lstSourceMoiQuanHe.IndexOf(lstSourceMoiQuanHe.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(dr["MOI_QUAN_HE"].ToString())));
                    if (dr["NGAY_SINH"] != DBNull.Value && dr["NGAY_SINH"].ToString().IsDate(ApplicationConstant.defaultDateTimeFormat))
                        txtNgaySinhNguoiBLanhQLoan.Value = LDateTime.StringToDate(dr["NGAY_SINH"].ToString(), ApplicationConstant.defaultDateTimeFormat);
                    if (dr["DAN_TOC"] != DBNull.Value)
                        cmbDanTocBLanhQLoan.SelectedIndex = lstSourceDanToc.IndexOf(lstSourceDanToc.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(dr["DAN_TOC"].ToString())));
                    if (dr["GIOI_TINH"] != DBNull.Value)
                        cmbGioiTinhBLanhQLoan.SelectedIndex = lstSourceGioiTinh.IndexOf(lstSourceGioiTinh.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(dr["GIOI_TINH"].ToString())));
                    if (dr["TTRANG_HON_NHAN"] != DBNull.Value)
                        cmbTinhTrangHonNhanBLanhQLoan.SelectedIndex = lstSourceTinhTrangHonNhan.IndexOf(lstSourceTinhTrangHonNhan.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(dr["TTRANG_HON_NHAN"].ToString())));
                    if (dr["EMAIL"] != DBNull.Value)
                        txtDiaChiEmailBLanhQLoan.Text = dr["EMAIL"].ToString();
                    if (dr["LOAI_HINH_CTRU"] != DBNull.Value)
                    {
                        string[] arrLoaiHinh = dr["LOAI_HINH_CTRU"].ToString().Split('#');
                        cmbLoaiHinhCuTruBLanhQLoan.SelectedIndex = lstSourceLoaiHinhCuTru.IndexOf(lstSourceLoaiHinhCuTru.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(arrLoaiHinh[0])));
                        if (arrLoaiHinh.Count() > 1)
                            txtLoaiHinhCuTruBLanhQLoan.Text = arrLoaiHinh[1];
                    }
                    if (dr["TEN_CONG_TY"] != DBNull.Value)
                        txtTenCongTyBLanhQLoan.Text = dr["TEN_CONG_TY"].ToString();
                    if (dr["SO_DIEN_THOAI_CTY"] != DBNull.Value)
                        txtSoDienThoaiCongTyBLanhQLoan.Text = dr["SO_DIEN_THOAI_CTY"].ToString();
                    if (dr["DIA_CHI_CTY"] != DBNull.Value)
                        txtDiaChiCongTyBLanhQLoan.Text = dr["DIA_CHI_CTY"].ToString();
                    if (dr["VI_TRI_LVIEC"] != DBNull.Value)
                        txtViTriHienTaiBLanhQLoan.Text = dr["VI_TRI_LVIEC"].ToString();
                    if (dr["PHONG_BAN"] != DBNull.Value)
                        txtPhongBan.Text = dr["PHONG_BAN"].ToString();
                    if (dr["TNHAP_CO_BAN"] != DBNull.Value && dr["TNHAP_CO_BAN"].ToString().IsNumeric())
                        txtLuongCoBanBLanhQLoan.Value = LNumber.StringToDouble(dr["TNHAP_CO_BAN"].ToString());
                    if (dr["TONG_THU_NHAP"] != DBNull.Value && dr["TONG_THU_NHAP"].ToString().IsNumeric())
                        txtThuNhapKhacBLanhQLoan.Value = LNumber.StringToDouble(dr["TONG_THU_NHAP"].ToString());
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        void GetDataForm(BusinessConstant.TrangThaiBanGhi bghi, BusinessConstant.TrangThaiNghiepVu nghiepvu)
        {
            try
            {
                AutoCompleteEntry auHinhThucTraGoc = lstHinhThucTraGoc.ElementAt(cmbHinhThucTraGoc.SelectedIndex);
                AutoCompleteEntry auHinhThucTraLai = lstHinhThucTraLai.ElementAt(cmbHinhThucTraLai.SelectedIndex);
                AutoCompleteEntry auThoiHanVay = lstThoiGianVay.ElementAt(cmbTGianVay.SelectedIndex);
                AutoCompleteEntry auThoiHanRut = lstThoiGianRut.ElementAt(cmbTGianRut.SelectedIndex);
                AutoCompleteEntry auThoiHanLSuat = lstTanSuatLSuat.ElementAt(cmbTHLaiSuat.SelectedIndex);
                AutoCompleteEntry auThoiHanQHLSuat = lstTanSuatLSuat.ElementAt(cmbQHLaiSuat.SelectedIndex);
                AutoCompleteEntry auLoaiTien = lstTienTe.ElementAt(cmbTienTe.SelectedIndex);
                objHDTD.MA_DXVV = txtSoDonVayVon.Text;
                objHDTD.MA_KHANG = txtMaKhachHang.Text;
                objHDTD.ID_KHANG = Convert.ToInt32(txtMaKhachHang.Tag);
                objHDTD.MA_HDTD = maHDTDVM;
                objHDTD.NGAY_LAP_HD = LDateTime.DateToString(teldtNgayHopDong.Value.GetValueOrDefault(), ApplicationConstant.defaultDateTimeFormat);
                objHDTD.ID_SAN_PHAM = Convert.ToInt32(txtMaSanPham.Tag);
                objHDTD.MA_SAN_PHAM = txtMaSanPham.Text;
                objHDTD.SO_TIEN_VAY = Convert.ToDecimal(telnumSoTienVay.Value.GetValueOrDefault());
                objHDTD.TGIAN_VAY = Convert.ToInt32(telnumTGianVay.Value.GetValueOrDefault());
                objHDTD.TGIAN_VAY_DVI_TINH = auThoiHanVay.KeywordStrings.FirstOrDefault();
                objHDTD.TGIAN_RUT = Convert.ToInt32(telnumTGianRut.Value.GetValueOrDefault());
                objHDTD.TGIAN_RUT_DVI_TINH = auThoiHanRut.KeywordStrings.FirstOrDefault();
                objHDTD.NGAY_HD = LDateTime.DateToString(teldtNgayHieuLuc.Value.GetValueOrDefault(), ApplicationConstant.defaultDateTimeFormat);
                objHDTD.NGAY_DAO_HAN = LDateTime.DateToString(teldtNgayDaoHan.Value.GetValueOrDefault(), ApplicationConstant.defaultDateTimeFormat);
                objHDTD.LSUAT_VAY = Convert.ToDecimal(telnumLaiSuat.Value.GetValueOrDefault());
                objHDTD.LSUAT_VAY_DVI_TINH = auThoiHanLSuat.KeywordStrings.FirstOrDefault();
                objHDTD.LSUAT_QHAN = Convert.ToDecimal(telnumQHLaiSuat.Value.GetValueOrDefault());
                objHDTD.LSUAT_QHAN_DVI_TINH = auThoiHanQHLSuat.KeywordStrings.FirstOrDefault();
                objHDTD.MUC_DICH_VAY = txtMaMucDich.Text;
                objHDTD.ID_NGUOI_QLY = Convert.ToInt32(txtMaCanBo.Tag);
                objHDTD.MA_NGUOI_QLY = txtMaCanBo.Text;
                objHDTD.DIEN_GIAI = txtDienGiai.Text;
                objHDTD.TRGOC_HTHUC = auHinhThucTraGoc.KeywordStrings.FirstOrDefault();
                objHDTD.TRLAI_HTHUC = auHinhThucTraLai.KeywordStrings.FirstOrDefault();
                objHDTD.TTHAI_NVU = nghiepvu.layGiaTri();
                objHDTD.TTHAI_BGHI = bghi.layGiaTri();
                objHDTD.LOAI_TIEN = auLoaiTien.KeywordStrings.FirstOrDefault();
                if (idHDTDVM > 0)
                {
                    objHDTD.ID = idHDTDVM;
                    objHDTD.NGUOI_CNHAT = ClientInformation.TenDangNhap;
                    objHDTD.NGAY_CNHAT = ClientInformation.NgayLamViecHienTai;
                    objHDTD.MA_DVI_QLY = ClientInformation.MaDonVi;
                    objHDTD.MA_DVI_TAO = ClientInformation.MaDonViGiaoDich;
                }
                else
                {
                    objHDTD.NGAY_NHAP = ClientInformation.NgayLamViecHienTai;
                    objHDTD.NGUOI_NHAP = ClientInformation.TenDangNhap;
                    objHDTD.MA_DVI_QLY = ClientInformation.MaDonVi;
                    objHDTD.MA_DVI_TAO = ClientInformation.MaDonViGiaoDich;
                }

                objHDTD.MA_HDTC = txtSoHopDongTC.Text;

                obj.HDTD_TD = objHDTD;                
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
                ds = new TinDungTDProcess().HopDongTinDungCaNhanChiTiet(dtPar);
                Dispatcher.CurrentDispatcher.DelayInvoke("SetDataForm", () =>
                {
                    SetTabThongTinChung(ds);
                    Dispatcher.CurrentDispatcher.DelayInvoke("SetTabThongTinBaoLanhTKe", () =>
                    {
                        SetTabThongTinBaoLanhTKe(ds);
                    }, TimeSpan.FromSeconds(0));
                    Dispatcher.CurrentDispatcher.DelayInvoke("SetTabThongKiemSoat", () =>
                    {
                        SetTabThongKiemSoat(ds);
                    }, TimeSpan.FromSeconds(0));
                    Dispatcher.CurrentDispatcher.DelayInvoke("SetTabHopDongTC", () =>
                    {
                        SetTabHopDongTC(ds);
                    }, TimeSpan.FromSeconds(0));
                }, TimeSpan.FromSeconds(0));
            }
            catch (Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        void SetTabHopDongTC(DataSet ds)
        {
            try
            {
                if (!LObject.IsNullOrEmpty(ds) && ds.Tables.Count > 1)
                {
                    DataTable dt = ds.Tables["TTIN_HDTC"];
                    if (dt.Rows.Count > 0)
                    {
                        txtSoHopDongTC.Text = dt.Rows[0]["MA_HDTC"].ToString();
                        teldtNgayHopDongTC.Value = LDateTime.StringToDate(dt.Rows[0]["NGAY_HDTC"].ToString(), ApplicationConstant.defaultDateTimeFormat);
                        txtDienGiaiHDTC.Text = dt.Rows[0]["DIEN_GIAI"].ToString();
                    }

                    dt = ds.Tables["TTIN_HDTC_TSDB"];
                    if (dt.Rows.Count > 0)
                    {
                        raddgrDSachTSDB.ItemsSource = dt.DefaultView;
                        SetGtriTong(dt.Rows.OfType<DataRow>().ToList());
                    }
                }
            }
            catch (Exception ex)
            {
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
                    obj = new TD_HDTD_TD();
                    objHDTD = new TDTD_HDTD();
                    objHDTD.ID = idHDTDVM = Convert.ToInt32(dr["ID"]);
                   
                    if (dr["HE_SO"] != DBNull.Value)
                        objHDTD.HE_SO = Convert.ToInt32(dr["HE_SO"]);
                    if (dr["ID_DXVV"] != DBNull.Value)
                        objHDTD.ID_DXVV = Convert.ToInt32(dr["ID_DXVV"]);
                    if (dr["ID_KHANG"] != DBNull.Value)
                        objHDTD.ID_KHANG = Convert.ToInt32(dr["ID_KHANG"]);
                    if (dr["ID_NGUOI_DTN"] != DBNull.Value)
                        objHDTD.ID_NGUOI_DTN = Convert.ToInt32(dr["ID_NGUOI_DTN"]);
                    if (dr["ID_NGUOI_QLY"] != DBNull.Value)
                        objHDTD.ID_NGUOI_QLY = Convert.ToInt32(dr["ID_NGUOI_QLY"]);
                    if (dr["ID_SAN_PHAM"] != DBNull.Value)
                        objHDTD.ID_SAN_PHAM = Convert.ToInt32(dr["ID_SAN_PHAM"]);
                    if (dr["KHOACH_HTHUC_LAP"] != DBNull.Value)
                        objHDTD.KHOACH_HTHUC_LAP = Convert.ToString(dr["KHOACH_HTHUC_LAP"]);
                    if (dr["KHOACH_NGAY_LAP"] != DBNull.Value)
                        objHDTD.KHOACH_NGAY_LAP = Convert.ToString(dr["KHOACH_NGAY_LAP"]);
                    if (dr["LSUAT_BDO"] != DBNull.Value)
                        objHDTD.LSUAT_BDO = Convert.ToDecimal(dr["LSUAT_BDO"]);
                    if (dr["LSUAT_CCAU"] != DBNull.Value)
                        objHDTD.LSUAT_CCAU = Convert.ToDecimal(dr["LSUAT_CCAU"]);
                    if (dr["LSUAT_CTRA"] != DBNull.Value)
                        objHDTD.LSUAT_CTRA = Convert.ToDecimal(dr["LSUAT_CTRA"]);
                    if (dr["LSUAT_LOAI"] != DBNull.Value)
                        objHDTD.LSUAT_LOAI = Convert.ToString(dr["LSUAT_LOAI"]);
                    if (dr["LSUAT_MA"] != DBNull.Value)
                        objHDTD.LSUAT_MA = Convert.ToString(dr["LSUAT_MA"]);
                    if (dr["LSUAT_QHAN"] != DBNull.Value)
                        objHDTD.LSUAT_QHAN = Convert.ToDecimal(dr["LSUAT_QHAN"]);
                    if (dr["LSUAT_QHAN_DVI_TINH"] != DBNull.Value)
                        objHDTD.LSUAT_QHAN_DVI_TINH = Convert.ToString(dr["LSUAT_QHAN_DVI_TINH"]);
                    if (dr["LSUAT_TSUAT"] != DBNull.Value)
                        objHDTD.LSUAT_TSUAT = Convert.ToInt32(dr["LSUAT_TSUAT"]);
                    if (dr["LSUAT_TSUAT_DVI_TINH"] != DBNull.Value)
                        objHDTD.LSUAT_TSUAT_DVI_TINH = Convert.ToString(dr["LSUAT_TSUAT_DVI_TINH"]);
                    if (dr["LSUAT_VAY"] != DBNull.Value)
                        objHDTD.LSUAT_VAY = Convert.ToDecimal(dr["LSUAT_VAY"]);
                    if (dr["LSUAT_VAY_DVI_TINH"] != DBNull.Value)
                        objHDTD.LSUAT_VAY_DVI_TINH = Convert.ToString(dr["LSUAT_VAY_DVI_TINH"]);
                    if (dr["MA_DVI_QLY"] != DBNull.Value)
                        objHDTD.MA_DVI_QLY = Convert.ToString(dr["MA_DVI_QLY"]);
                    if (dr["MA_DVI_TAO"] != DBNull.Value)
                        objHDTD.MA_DVI_TAO = Convert.ToString(dr["MA_DVI_TAO"]);
                    if (dr["MA_DXVV"] != DBNull.Value)
                        objHDTD.MA_DXVV = Convert.ToString(dr["MA_DXVV"]);
                    if (dr["MA_HDTD"] != DBNull.Value)
                        objHDTD.MA_HDTD = maHDTDVM = Convert.ToString(dr["MA_HDTD"]);
                    if (dr["SO_HDTD"] != DBNull.Value)
                        objHDTD.SO_HDTD =  Convert.ToString(dr["SO_HDTD"]);
                    if (dr["SO_GDICH"] != DBNull.Value)
                        objHDTD.SO_GDICH = Convert.ToString(dr["SO_GDICH"]);
                    if (dr["MA_HMUC"] != DBNull.Value)
                        objHDTD.MA_HMUC = Convert.ToString(dr["MA_HMUC"]);
                    if (dr["MA_KHANG"] != DBNull.Value)
                        objHDTD.MA_KHANG = Convert.ToString(dr["MA_KHANG"]);
                    if (dr["MA_NGUOI_DTN"] != DBNull.Value)
                        objHDTD.MA_NGUOI_DTN = Convert.ToString(dr["MA_NGUOI_DTN"]);
                    if (dr["MA_NGUOI_QLY"] != DBNull.Value)
                        objHDTD.MA_NGUOI_QLY = Convert.ToString(dr["MA_NGUOI_QLY"]);
                    if (dr["MA_SAN_PHAM"] != DBNull.Value)
                        objHDTD.MA_SAN_PHAM = Convert.ToString(dr["MA_SAN_PHAM"]);
                    if (dr["MUC_DICH_VAY"] != DBNull.Value)
                        objHDTD.MUC_DICH_VAY = Convert.ToString(dr["MUC_DICH_VAY"]);
                    if (dr["NGANH_KINH_TE"] != DBNull.Value)
                        objHDTD.NGANH_KINH_TE = Convert.ToString(dr["NGANH_KINH_TE"]);
                    if (dr["NGAY_CHUYEN_QH"] != DBNull.Value)
                        objHDTD.NGAY_CHUYEN_QH = Convert.ToString(dr["NGAY_CHUYEN_QH"]);
                    if (dr["NGAY_CNHAT"] != DBNull.Value)
                        objHDTD.NGAY_CNHAT = Convert.ToString(dr["NGAY_CNHAT"]);
                    if (dr["NGAY_DAO_HAN"] != DBNull.Value)
                        objHDTD.NGAY_DAO_HAN = Convert.ToString(dr["NGAY_DAO_HAN"]);
                    if (dr["NGAY_GIA_HAN"] != DBNull.Value)
                        objHDTD.NGAY_GIA_HAN = Convert.ToString(dr["NGAY_GIA_HAN"]);
                    if (dr["NGAY_HD"] != DBNull.Value)
                        objHDTD.NGAY_HD = Convert.ToString(dr["NGAY_HD"]);
                    if (dr["NGAY_NHAP"] != DBNull.Value)
                        objHDTD.NGAY_NHAP = Convert.ToString(dr["NGAY_NHAP"]);
                    if (dr["NGUOI_CNHAT"] != DBNull.Value)
                        objHDTD.NGUOI_CNHAT = Convert.ToString(dr["NGUOI_CNHAT"]);
                    if (dr["NGUOI_NHAP"] != DBNull.Value)
                        objHDTD.NGUOI_NHAP = Convert.ToString(dr["NGUOI_NHAP"]);
                    if (dr["PHI_MO_HD"] != DBNull.Value)
                        objHDTD.PHI_MO_HD = Convert.ToDecimal(dr["PHI_MO_HD"]);
                    if (dr["PHUONG_THUC_VAY"] != DBNull.Value)
                        objHDTD.PHUONG_THUC_VAY = Convert.ToString(dr["PHUONG_THUC_VAY"]);
                    if (dr["SO_GDICH"] != DBNull.Value)
                        objHDTD.SO_GDICH = Convert.ToString(dr["SO_GDICH"]);
                    if (dr["SO_TIEN_CAN"] != DBNull.Value)
                        objHDTD.SO_TIEN_CAN = Convert.ToDecimal(dr["SO_TIEN_CAN"]);
                    if (dr["SO_TIEN_GOC_MOI_KY"] != DBNull.Value)
                        objHDTD.SO_TIEN_GOC_MOI_KY = Convert.ToDecimal(dr["SO_TIEN_GOC_MOI_KY"]);
                    if (dr["SO_TIEN_LAI_MOI_KY"] != DBNull.Value)
                        objHDTD.SO_TIEN_LAI_MOI_KY = Convert.ToDecimal(dr["SO_TIEN_LAI_MOI_KY"]);
                    if (dr["SO_TIEN_MOI_KY"] != DBNull.Value)
                        objHDTD.SO_TIEN_MOI_KY = Convert.ToDecimal(dr["SO_TIEN_MOI_KY"]);
                    if (dr["SO_TIEN_TKBB"] != DBNull.Value)
                        objHDTD.SO_TIEN_TKBB = Convert.ToDecimal(dr["SO_TIEN_TKBB"]);
                    if (dr["SO_TIEN_TU_CO"] != DBNull.Value)
                        objHDTD.SO_TIEN_TU_CO = Convert.ToDecimal(dr["SO_TIEN_TU_CO"]);
                    if (dr["SO_TIEN_VAY"] != DBNull.Value)
                        objHDTD.SO_TIEN_VAY = Convert.ToDecimal(dr["SO_TIEN_VAY"]);
                    if (dr["SO_TIEN_XIN_VAY"] != DBNull.Value)
                        objHDTD.SO_TIEN_XIN_VAY = Convert.ToDecimal(dr["SO_TIEN_XIN_VAY"]);
                    if (dr["SO_TKHOAN_NHAN_NO"] != DBNull.Value)
                        objHDTD.SO_TKHOAN_NHAN_NO = Convert.ToString(dr["SO_TKHOAN_NHAN_NO"]);
                    if (dr["TGIAN_VAY"] != DBNull.Value)
                        objHDTD.TGIAN_VAY = Convert.ToInt32(dr["TGIAN_VAY"]);
                    if (dr["TGIAN_VAY_DVI_TINH"] != DBNull.Value)
                        objHDTD.TGIAN_VAY_DVI_TINH = Convert.ToString(dr["TGIAN_VAY_DVI_TINH"]);
                    if (dr["TGIAN_RUT"] != DBNull.Value)
                        objHDTD.TGIAN_RUT = Convert.ToInt32(dr["TGIAN_RUT"]);
                    if (dr["TGIAN_RUT_DVI_TINH"] != DBNull.Value)
                        objHDTD.TGIAN_RUT_DVI_TINH = Convert.ToString(dr["TGIAN_RUT_DVI_TINH"]);
                    if (dr["TRGOC_DVI_TINH"] != DBNull.Value)
                        objHDTD.TRGOC_DVI_TINH = Convert.ToString(dr["TRGOC_DVI_TINH"]);
                    if (dr["TRGOC_HTHUC"] != DBNull.Value)
                        objHDTD.TRGOC_HTHUC = Convert.ToString(dr["TRGOC_HTHUC"]);
                    if (dr["TRGOC_SO_KY"] != DBNull.Value)
                        objHDTD.TRGOC_SO_KY = Convert.ToInt32(dr["TRGOC_SO_KY"]);
                    if (dr["TRGOC_SO_TKHOAN"] != DBNull.Value)
                        objHDTD.TRGOC_SO_TKHOAN = Convert.ToString(dr["TRGOC_SO_TKHOAN"]);
                    if (dr["TRLAI_DVI_TINH"] != DBNull.Value)
                        objHDTD.TRLAI_DVI_TINH = Convert.ToString(dr["TRLAI_DVI_TINH"]);
                    if (dr["TRLAI_HTHUC"] != DBNull.Value)
                        objHDTD.TRLAI_HTHUC = Convert.ToString(dr["TRLAI_HTHUC"]);
                    if (dr["TRLAI_SO_KY"] != DBNull.Value)
                        objHDTD.TRLAI_SO_KY = Convert.ToInt32(dr["TRLAI_SO_KY"]);
                    if (dr["TRLAI_SO_TKHOAN"] != DBNull.Value)
                        objHDTD.TRLAI_SO_TKHOAN = Convert.ToString(dr["TRLAI_SO_TKHOAN"]);
                    if (dr["TTHAI_BGHI"] != DBNull.Value)
                        objHDTD.TTHAI_BGHI = Convert.ToString(dr["TTHAI_BGHI"]);
                    if (dr["TTHAI_GIAI_NGAN"] != DBNull.Value)
                        objHDTD.TTHAI_GIAI_NGAN = Convert.ToString(dr["TTHAI_GIAI_NGAN"]);
                    if (dr["TTHAI_LY_DO"] != DBNull.Value)
                        objHDTD.TTHAI_LY_DO = Convert.ToString(dr["TTHAI_LY_DO"]);
                    if (dr["TTHAI_NVU"] != DBNull.Value)
                    {
                        TThaiNVu = Convert.ToString(dr["TTHAI_NVU"]);
                        objHDTD.TTHAI_NVU = Convert.ToString(dr["TTHAI_NVU"]);
                    }
                    if (dr["DIEN_GIAI"] != DBNull.Value)
                        objHDTD.DIEN_GIAI = Convert.ToString(dr["DIEN_GIAI"]);

                    obj.HDTD_TD = objHDTD;
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(TThaiNVu);
                    txtSoDonVayVon.Text = objHDTD.MA_DXVV;
                    txtSoHopDong.Text = objHDTD.SO_HDTD;
                    teldtNgayLapDon.Value = LDateTime.StringToDate(dr["NGAY_LAP_DON"].ToString(), ApplicationConstant.defaultDateTimeFormat);
                    teldtNgayHopDong.Value = LDateTime.StringToDate(dr["NGAY_LAP_HD"].ToString(), ApplicationConstant.defaultDateTimeFormat);
                    txtMaKhachHang.Text = objHDTD.MA_KHANG;
                    txtMaKhachHang.Tag = Convert.ToInt32(dr["ID_KHANG"]);
                    txtTenKhachHang.Text = dr["TEN_KHANG"].ToString();
                    txtMaSanPham.Text = objHDTD.MA_SAN_PHAM;
                    txtMaSanPham.Tag = objHDTD.ID_SAN_PHAM;
                    lblTenSanPham.Content = dr["TEN_SAN_PHAM"].ToString();
                    cmbTienTe.SelectedIndex = lstTienTe.IndexOf(lstTienTe.FirstOrDefault(f=>f.KeywordStrings.FirstOrDefault().Equals(dr["MA_LOAI_TIEN"].ToString())));
                    telnumSoTienVay.Value = Convert.ToDouble(objHDTD.SO_TIEN_VAY);
                    telnumTGianVay.Value = Convert.ToDouble(objHDTD.TGIAN_VAY);
                    cmbTGianVay.SelectedIndex = lstThoiGianVay.IndexOf(lstThoiGianVay.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(objHDTD.TGIAN_VAY_DVI_TINH)));
                    telnumTGianRut.Value = Convert.ToDouble(objHDTD.TGIAN_RUT);
                    cmbTGianRut.SelectedIndex = lstThoiGianRut.IndexOf(lstThoiGianRut.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(objHDTD.TGIAN_RUT_DVI_TINH)));
                    cmbTHLaiSuat.SelectedIndex = lstTanSuatLSuat.IndexOf(lstTanSuatLSuat.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(objHDTD.LSUAT_VAY_DVI_TINH)));
                    cmbQHLaiSuat.SelectedIndex = lstTanSuatLSuat.IndexOf(lstTanSuatLSuat.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(objHDTD.LSUAT_QHAN_DVI_TINH)));
                    teldtNgayHieuLuc.Value = LDateTime.StringToDate(objHDTD.NGAY_HD, ApplicationConstant.defaultDateTimeFormat);
                    teldtNgayDaoHan.Value = LDateTime.StringToDate(objHDTD.NGAY_DAO_HAN, ApplicationConstant.defaultDateTimeFormat);
                    telnumLaiSuat.Value = Convert.ToDouble(objHDTD.LSUAT_VAY);
                    telnumQHLaiSuat.Value = Convert.ToDouble(objHDTD.LSUAT_QHAN);
                    txtMaMucDich.Text = objHDTD.MUC_DICH_VAY;
                    lblTenMucDich.Content = LLanguage.SearchResourceByKey(dr["TEN_MUC_DICH_VAY"].ToString());
                    txtMaCanBo.Text = objHDTD.MA_NGUOI_QLY;
                    txtMaCanBo.Tag = objHDTD.ID_NGUOI_QLY;
                    lblMaCanBo.Content = dr["TEN_NGUOI_QLY"].ToString();
                    txtDienGiai.Text = objHDTD.DIEN_GIAI;
                    cmbHinhThucTraGoc.SelectedIndex = lstHinhThucTraGoc.IndexOf(lstHinhThucTraGoc.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(objHDTD.TRGOC_HTHUC)));
                    cmbHinhThucTraLai.SelectedIndex = lstHinhThucTraLai.IndexOf(lstHinhThucTraLai.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(objHDTD.TRLAI_HTHUC)));

                    CommonFunction.RefreshButton(Toolbar, action, TThaiNVu, mnuMain, DatabaseConstant.Function.TDTD_HOP_DONG_CA_NHAN);
                    if (action.Equals(DatabaseConstant.Action.SUA))
                        SetEnabledAllControls(true);
                    else
                        SetEnabledAllControls(false);
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

        void beforeModify()
        {
            action = DatabaseConstant.Action.SUA;
            SetEnabledAllControls(true);
            OnModify();
        }

        private void SetEnabledAllControls(bool enable)
        {
            txtSoDonVayVon.IsEnabled = enable;
            txtMaKhachHang.IsEnabled = enable;
            teldtNgayHopDong.IsEnabled = enable;
            telnumTGianRut.IsEnabled = enable;
            teldtNgayHieuLuc.IsEnabled = enable;
            telnumQHLaiSuat.IsEnabled = enable;
            //txtMaMucDich.IsEnabled = enable;
            txtMaCanBo.IsEnabled = enable;
            txtDienGiai.IsEnabled = enable;
            cmbHinhThucTraGoc.IsEnabled = enable;
            cmbHinhThucTraLai.IsEnabled = enable;

            btnSoHopDongTC.IsEnabled = enable;
        }

        private void Release()
        {
            // Yêu cầu Unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
            DatabaseConstant.Function.TDTD_HOP_DONG_CA_NHAN,
            DatabaseConstant.Table.TDTD_HDTD,
            DatabaseConstant.Action.SUA,
            lstId);
        }

        void OnModify()
        {
            UtilitiesProcess process = new UtilitiesProcess();
            lstId.Add(idHDTDVM);
            bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
            DatabaseConstant.Function.TDTD_HOP_DONG_CA_NHAN,
            DatabaseConstant.Table.TDTD_HDTD,
            action,
            lstId);
            SetEnabledAllControls(true);
            CommonFunction.RefreshButton(Toolbar, action, TThaiNVu, mnuMain, DatabaseConstant.Function.TDTD_HOP_DONG_CA_NHAN);
        }

        bool Validation()
        {
            bool bReturn = true;
            if (txtMaKhachHang.Text.IsNullOrEmptyOrSpace())
            {
                CommonFunction.ThongBaoTrong(lblMaKhachHang.Content.ToString());
                txtMaKhachHang.Focus();
                return false;
            }
            if (txtMaMucDich.Text.IsNullOrEmptyOrSpace())
            {
                CommonFunction.ThongBaoTrong(lblMucDichVay.Content.ToString());
                txtMaMucDich.Focus();
                return false;
            }
            if (cmbTGianRut.SelectedIndex < 0)
            {
                CommonFunction.ThongBaoTrong(lblTGianRut.Content.ToString());
                cmbTGianRut.Focus();
                return false;
            }
            if (telnumTGianRut.Value.IsNullOrEmpty() || telnumTGianRut.Value.GetValueOrDefault()<=0)
            {
                CommonFunction.ThongBaoTrong(lblTGianRut.Content.ToString());
                telnumTGianRut.Focus();
                return false;
            }

            if (txtMaCanBo.Text.IsNullOrEmptyOrSpace())
            {
                CommonFunction.ThongBaoTrong(lblCanBoQly.Content.ToString());
                txtMaCanBo.Focus();
                return false;
            }
            if (cmbHinhThucTraGoc.SelectedIndex < 0)
            {
                CommonFunction.ThongBaoTrong(lblHinhThucTraGoc.Content.ToString());
                cmbHinhThucTraGoc.Focus();
                return false;
            }
            if (cmbHinhThucTraLai.SelectedIndex < 0)
            {
                CommonFunction.ThongBaoTrong(lblHinhThucTraLai.Content.ToString());
                cmbHinhThucTraLai.Focus();
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
            if (idHDTDVM==0)
                iret = new TinDungTDProcess().HopDongTinDungCaNhan(DatabaseConstant.Action.THEM, ref obj, ref lstResponseDetail);
            else
                iret = new TinDungTDProcess().HopDongTinDungCaNhan(DatabaseConstant.Action.SUA, ref obj, ref lstResponseDetail);
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
                DatabaseConstant.Function.TDTD_HOP_DONG_CA_NHAN,
                DatabaseConstant.Table.TDTD_HDTD,
                DatabaseConstant.Action.SUA,
                lstId);
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
            DatabaseConstant.Function.TDTD_HOP_DONG_CA_NHAN,
            DatabaseConstant.Table.TDTD_HDTD,
            DatabaseConstant.Action.XOA,
            lstId);
           
            if (iret < 1)
                SetInfomation();
            else
                ResetForm();
        }

        void OnDelete()
        {
            int iret = 0;
            List<ClientResponseDetail> ResponseDetail = new List<ClientResponseDetail>();
            obj.DSACH_HDTD_TD = lst.ToArray();
            iret = new TinDungTDProcess().HopDongTinDungCaNhan(DatabaseConstant.Action.XOA, ref obj, ref ResponseDetail);
            AfterDelete(maHDTDVM, ResponseDetail, iret);
        }

        void BeforeDelete()
        {
            if (tlbDelete.IsEnabled == false)
                return;
            try
            {
                Cursor = Cursors.Wait;
                if (!maHDTDVM.IsNullOrEmptyOrSpace())
                {
                    if (LMessage.ShowMessage("M.DungChung.HoiXoa", LMessage.MessageBoxType.Question) == MessageBoxResult.Yes)
                    {
                        lst = new List<TDTD_HDTD>();
                        lst.Add(objHDTD);
                        lstId = new List<int>();
                        lstId.Add(idHDTDVM);
                        // Yêu cầu lock dữ liệu
                        UtilitiesProcess process = new UtilitiesProcess();
                        bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                        DatabaseConstant.Function.TDTD_HOP_DONG_CA_NHAN,
                        DatabaseConstant.Table.TDTD_HDTD,
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
                DatabaseConstant.Function.TDTD_HOP_DONG_CA_NHAN,
                DatabaseConstant.Table.TDTD_HDTD,
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
            DatabaseConstant.Function.TDTD_HOP_DONG_CA_NHAN,
            DatabaseConstant.Table.TDTD_HDTD,
            DatabaseConstant.Action.DUYET,
            lstId);
            action = DatabaseConstant.Action.XEM;

            TThaiNVu = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();
            lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(TThaiNVu);
            CommonFunction.RefreshButton(Toolbar, action, TThaiNVu, mnuMain, DatabaseConstant.Function.TDTD_HOP_DONG_CA_NHAN);            
        }

        void OnApprove()
        {
            int iret = 0;
            List<ClientResponseDetail> ResponseDetail = new List<ClientResponseDetail>();
            obj.DSACH_HDTD_TD = lst.ToArray();
            iret = new TinDungTDProcess().HopDongTinDungCaNhan(DatabaseConstant.Action.DUYET, ref obj, ref ResponseDetail);
            AfterApprove(maHDTDVM, ResponseDetail);
        }

        void BeforeApprove()
        {
            if (!tlbApprove.IsEnabled)
                return;
            try
            {
                if (!maHDTDVM.IsNullOrEmptyOrSpace())
                {
                    if (LMessage.ShowMessage("M.DungChung.HoiDuyet", LMessage.MessageBoxType.Question) == MessageBoxResult.Yes)
                    {
                        lst = new List<TDTD_HDTD>();
                        lst.Add(objHDTD);

                        lstId = new List<int>();
                        lstId.Add(idHDTDVM);
                        // Yêu cầu lock dữ liệu
                        UtilitiesProcess process = new UtilitiesProcess();
                        bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                        DatabaseConstant.Function.TDTD_HOP_DONG_CA_NHAN,
                        DatabaseConstant.Table.TDTD_HDTD,
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
                DatabaseConstant.Function.TDTD_HOP_DONG_CA_NHAN,
                DatabaseConstant.Table.TDTD_HDTD,
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
            DatabaseConstant.Function.TDTD_HOP_DONG_CA_NHAN,
            DatabaseConstant.Table.TDTD_HDTD,
            DatabaseConstant.Action.TU_CHOI_DUYET,
            lstId);
            action = DatabaseConstant.Action.XEM;

            TThaiNVu = BusinessConstant.TrangThaiNghiepVu.TU_CHOI.layGiaTri();
            lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(TThaiNVu);
        }

        void OnRefuse()
        {
            int iret = 0;
            List<ClientResponseDetail> ResponseDetail = new List<ClientResponseDetail>();
            obj.DSACH_HDTD_TD = lst.ToArray();
            iret = new TinDungTDProcess().HopDongTinDungCaNhan(DatabaseConstant.Action.TU_CHOI_DUYET, ref obj, ref ResponseDetail);
            AfterRefuse(maHDTDVM, ResponseDetail);
        }

        void BeforeRefuse()
        {
            if (!tlbRefuse.IsEnabled)
                return;
            try
            {
                if (!maHDTDVM.IsNullOrEmptyOrSpace())
                {
                    if (LMessage.ShowMessage("M.DungChung.HoiTuChoiDuyet", LMessage.MessageBoxType.Question) == MessageBoxResult.Yes)
                    {
                        lst = new List<TDTD_HDTD>();
                        lst.Add(objHDTD);
                        lstId = new List<int>();
                        lstId.Add(idHDTDVM);
                        // Yêu cầu lock dữ liệu
                        UtilitiesProcess process = new UtilitiesProcess();
                        bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                        DatabaseConstant.Function.TDTD_HOP_DONG_CA_NHAN,
                        DatabaseConstant.Table.TDTD_HDTD,
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
                DatabaseConstant.Function.TDTD_HOP_DONG_CA_NHAN,
                DatabaseConstant.Table.TDTD_HDTD,
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
            DatabaseConstant.Function.TDTD_HOP_DONG_CA_NHAN,
            DatabaseConstant.Table.TDTD_HDTD,
            DatabaseConstant.Action.THOAI_DUYET,
            lstId);
            action = DatabaseConstant.Action.XEM;

            TThaiNVu = BusinessConstant.TrangThaiNghiepVu.THOAI_DUYET.layGiaTri();
            lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(TThaiNVu);
            CommonFunction.RefreshButton(Toolbar, action, TThaiNVu, mnuMain, DatabaseConstant.Function.TDTD_HOP_DONG_CA_NHAN);
        }

        void OnCancel()
        {
            int iret = 0;
            List<ClientResponseDetail> ResponseDetail = new List<ClientResponseDetail>();
            obj.DSACH_HDTD_TD = lst.ToArray();
            iret = new TinDungTDProcess().HopDongTinDungCaNhan(DatabaseConstant.Action.THOAI_DUYET, ref obj, ref ResponseDetail);
            AfterCancel(maHDTDVM, ResponseDetail);
        }

        void BeforeCancel()
        {
            if (!tlbCancel.IsEnabled)
                return;
            try
            {
                if (!maHDTDVM.IsNullOrEmptyOrSpace())
                {
                    if (LMessage.ShowMessage("M.DungChung.HoiThoaiDuyet", LMessage.MessageBoxType.Question) == MessageBoxResult.Yes)
                    {
                        lst = new List<TDTD_HDTD>();
                        lst.Add(objHDTD);
                        lstId = new List<int>();
                        lstId.Add(idHDTDVM);
                        // Yêu cầu lock dữ liệu
                        UtilitiesProcess process = new UtilitiesProcess();
                        bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                        DatabaseConstant.Function.TDTD_HOP_DONG_CA_NHAN,
                        DatabaseConstant.Table.TDTD_HDTD,
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
                DatabaseConstant.Function.TDTD_HOP_DONG_CA_NHAN,
                DatabaseConstant.Table.TDTD_HDTD,
                DatabaseConstant.Action.THOAI_DUYET,
                lstId);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void OnPrint()
        {
            // Cảnh báo khi không có dữ liệu
            if (LObject.IsNullOrEmpty(maHDTDVM))
            {
                LMessage.ShowMessage("Không có thông tin hợp đồng cần xử lý", LMessage.MessageBoxType.Warning);
                return;
            }
            else
            {
                if (ClientInformation.Company.Equals("M7MFI") || ClientInformation.Company.Equals("BINHKHANH"))
                {
                    // Cảnh báo phải lựa chọn hợp đồng
                    List<string> lstMaHDTD = new List<string>();
                    List<DataRow> listDataRow = getListSeletedDataRow();
                    if (listDataRow.Count == 0)
                    {
                        LMessage.ShowMessage("M.DungChung.ChuaChonBanGhi", LMessage.MessageBoxType.Warning);
                        return;
                    }
                    else
                    {
                        foreach (DataRow dr in listDataRow)
                        {
                            lstMaHDTD.Add(dr["MA_HDTD"].ToString());
                        }
                    }

                    VanHanhGiaoDich xemBaoCao = new VanHanhGiaoDich();
                    DoiTuongBaoCao doiTuongBaoCao = new DoiTuongBaoCao();

                    GIAO_DICH_BASE objGIAO_DICH_BASE = new GIAO_DICH_BASE();
                    objGIAO_DICH_BASE.BaoCao = DatabaseConstant.DanhSachBaoCaoTheoGiaoDich.TDVM_HDTD;
                    objGIAO_DICH_BASE.ChucNang = DatabaseConstant.Function.TDTD_HOP_DONG_CA_NHAN;

                    List<Presentation.Process.BaoCaoServiceRef.TDVM_HDTD> listTDVM_HDTD = new List<Presentation.Process.BaoCaoServiceRef.TDVM_HDTD>();
                    foreach (string maHDTD in lstMaHDTD)
                    {
                        Presentation.Process.BaoCaoServiceRef.TDVM_HDTD objGDKT_GIAO_DICH = new Presentation.Process.BaoCaoServiceRef.TDVM_HDTD();
                        objGDKT_GIAO_DICH.SoHopDong = maHDTDVM;
                        listTDVM_HDTD.Add(objGDKT_GIAO_DICH);
                    }


                    doiTuongBaoCao.objGIAO_DICH_BASE = objGIAO_DICH_BASE;
                    doiTuongBaoCao.listTDVM_HDTD = listTDVM_HDTD.ToArray();
                    xemBaoCao.GiaoDichPhatSinh(ref doiTuongBaoCao);
                }
                else if (ClientInformation.Company.Equals("BANTAYVANG"))
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

                    KhachHangProcess khProcess = new KhachHangProcess();
                    DataSet dsKhachHang = new DataSet();
                    dsKhachHang = khProcess.getThongTinKHTheoID(objHDTD.ID_KHANG);
                    DataTable dtKhangHSo = null;
                    bool IsCheckedMauMoi = false;
                    if (IsCheckedMauMoi == false)
                    {
                        if (dsKhachHang != null && dsKhachHang.Tables.Count > 0)
                        {
                            dtKhangHSo = dsKhachHang.Tables[0];
                            makhachhang = dtKhangHSo.Rows[0]["ma_khang"].ToString(); ;
                            tenkhachhang = dtKhangHSo.Rows[0]["ten_khang"].ToString();
                            diachi = dtKhangHSo.Rows[0]["dia_chi"].ToString();
                            gioitinh = dtKhangHSo.Rows[0]["DD_GIOI_TINH"].ToString();
                            ngaysinh = dtKhangHSo.Rows[0]["DD_NGAY_SINH"].ToString();
                            socmnd = dtKhangHSo.Rows[0]["DD_GTLQ_SO"].ToString();
                            ngaycap = dtKhangHSo.Rows[0]["DD_GTLQ_NGAY_CAP"].ToString();
                            noicap = dtKhangHSo.Rows[0]["DD_GTLQ_NOI_CAP"].ToString();
                        }

                        idkhachhang = objHDTD.ID_KHANG.ToString();
                        sotienvay = "0";
                        mahopdong = maHDTDVM;
                        masanpham = txtMaSanPham.Text;
                    }
                    else
                    {
                        makhachhang = "";
                        idkhachhang = "";
                        sotienvay = "0";
                        mahopdong = "";
                        masanpham = "";
                        tenkhachhang = "";
                        diachi = "";
                        gioitinh = "";
                        ngaysinh = "";
                        socmnd = "";
                        ngaycap = "";
                        noicap = "";
                    }
                    ngaybaocao = ClientInformation.NgayLamViecHienTai;

                    lstThamSo.Add(new ThamSoBaoCao("@MaKhachHang", makhachhang, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("@IdKhachHang", idkhachhang, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("@SoTienVay", sotienvay, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("@MaHopDongTD", mahopdong, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("@NgayBaoCao", ngaybaocao, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("@MaSanPhamTD", masanpham, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("MaNgonNgu", ClientInformation.NgonNgu, ApplicationConstant.LoaiThamSoBaoCao.LANG.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("MaDinhDang", ApplicationConstant.LoaiDinhDangBaoCao.WORD.layGiaTri(), ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("@MA_DON_V", ClientInformation.MaDonViGiaoDich, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));

                    lstThamSo.Add(new ThamSoBaoCao("P_TEN_KHACH_HANG", tenkhachhang, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("P_TEN_THUONG_GOI", tenkhachhang, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("P_GIOI_TINH", gioitinh, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("P_NGAY_SINH", ngaysinh, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("P_DIA_CHI", diachi, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("P_SO_CMND", socmnd, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("P_NGAY_CAP", ngaycap, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("P_NOI_CAP", noicap, ApplicationConstant.LoaiThamSoBaoCao.GUI.layGiaTri()));

                    string maBaoCao = DatabaseConstant.LayMaBaoCaoBTV(DatabaseConstant.DanhSachBaoCaoBTV.TDVM_DON_VAY_VON);
                    xemBaoCao.LayDuLieu(maBaoCao, lstThamSo);
                }
                else
                {
                }
            }
        }

        private void OnPreviewHopDong()
        {
            // Cảnh báo khi không có dữ liệu
            if (LObject.IsNullOrEmpty(objHDTD.MA_HDTD))
            {
                LMessage.ShowMessage("M.DungChung.KhongCoThongTin", LMessage.MessageBoxType.Warning);
                return;
            }
            else
            {
                DataSet ds = new DataSet();
                DataTable dtPar = null;
                LDatatable.MakeParameterTable(ref dtPar);
                LDatatable.AddParameter(ref dtPar, "@SoGiaoDich", "String", txtSoDonVayVon.Text);
                ds = new TinDungTDProcess().GetThongTinDonXinVayVonTinDungTieuDung(dtPar);
                if (ds == null || ds.Tables.Count <= 0)
                {
                    LMessage.ShowMessage("Không có thông tin cần xử lý", LMessage.MessageBoxType.Warning);
                    return;
                }

                string loaiDXVV = ds.Tables["TTIN_CHUNG"].Rows[0]["LOAI_DXVV"].ToString();
                string maBaoCao = DatabaseConstant.LayMaBaoCaoBIDV(DatabaseConstant.DanhSachBaoCaoBIDV.TDTT_HOP_DONG_TIN_DUNG);
                if (loaiDXVV == "TDTD_TIN_CHAP")
                {
                    maBaoCao = DatabaseConstant.LayMaBaoCaoBIDV(DatabaseConstant.DanhSachBaoCaoBIDV.TDTT_HOP_DONG_TIN_DUNG);
                }
                else if (loaiDXVV == "TDTD_BAO_LANH_BEN_3")
                {
                    maBaoCao = DatabaseConstant.LayMaBaoCaoBIDV(DatabaseConstant.DanhSachBaoCaoBIDV.TDTT_HOP_DONG_TIN_DUNG_BLANH_BEN_THU_BA);
                }

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

                KhachHangProcess khProcess = new KhachHangProcess();
                DataSet dsKhachHang = new DataSet();
                dsKhachHang = khProcess.getThongTinKHTheoID(obj.HDTD_TD.ID_KHANG);
                DataTable dtKhangHSo = null;
                bool IsCheckedMauMoi = false;
                if (IsCheckedMauMoi == false)
                {
                    if (dsKhachHang != null && dsKhachHang.Tables.Count > 0)
                    {
                        dtKhangHSo = dsKhachHang.Tables[0];
                        makhachhang = dtKhangHSo.Rows[0]["ma_khang"].ToString(); ;
                        tenkhachhang = dtKhangHSo.Rows[0]["ten_khang"].ToString();
                        diachi = dtKhangHSo.Rows[0]["dia_chi"].ToString();
                        gioitinh = dtKhangHSo.Rows[0]["DD_GIOI_TINH"].ToString();
                        ngaysinh = dtKhangHSo.Rows[0]["DD_NGAY_SINH"].ToString();
                        socmnd = dtKhangHSo.Rows[0]["DD_GTLQ_SO"].ToString();
                        ngaycap = dtKhangHSo.Rows[0]["DD_GTLQ_NGAY_CAP"].ToString();
                        noicap = dtKhangHSo.Rows[0]["DD_GTLQ_NOI_CAP"].ToString();
                    }

                    idkhachhang = obj.HDTD_TD.ID_KHANG.ToString();
                    sotienvay = "0";
                    mahopdong = obj.HDTD_TD.MA_HDTD;
                    masanpham = txtMaSanPham.Text;
                }
                else
                {
                    makhachhang = "";
                    idkhachhang = "";
                    sotienvay = "0";
                    mahopdong = "";
                    masanpham = "";
                    tenkhachhang = "";
                    diachi = "";
                    gioitinh = "";
                    ngaysinh = "";
                    socmnd = "";
                    ngaycap = "";
                    noicap = "";
                }
                ngaybaocao = ClientInformation.NgayLamViecHienTai;

                lstThamSo.Add(new ThamSoBaoCao("@MaKhachHang", makhachhang, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                lstThamSo.Add(new ThamSoBaoCao("@IdKhachHang", idkhachhang, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                lstThamSo.Add(new ThamSoBaoCao("@SoTienVay", sotienvay, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                lstThamSo.Add(new ThamSoBaoCao("@MaHopDongTD", mahopdong, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
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
                
                xemBaoCao.LayDuLieu(maBaoCao, lstThamSo);
            }
        }

        private void OnPreviewPhuLuc()
        {
            // Cảnh báo khi không có dữ liệu
            if (LObject.IsNullOrEmpty(objHDTD.MA_HDTD))
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

                KhachHangProcess khProcess = new KhachHangProcess();
                DataSet dsKhachHang = new DataSet();
                dsKhachHang = khProcess.getThongTinKHTheoID(obj.HDTD_TD.ID_KHANG);
                DataTable dtKhangHSo = null;
                bool IsCheckedMauMoi = false;
                if (IsCheckedMauMoi == false)
                {
                    if (dsKhachHang != null && dsKhachHang.Tables.Count > 0)
                    {
                        dtKhangHSo = dsKhachHang.Tables[0];
                        makhachhang = dtKhangHSo.Rows[0]["ma_khang"].ToString(); ;
                        tenkhachhang = dtKhangHSo.Rows[0]["ten_khang"].ToString();
                        diachi = dtKhangHSo.Rows[0]["dia_chi"].ToString();
                        gioitinh = dtKhangHSo.Rows[0]["DD_GIOI_TINH"].ToString();
                        ngaysinh = dtKhangHSo.Rows[0]["DD_NGAY_SINH"].ToString();
                        socmnd = dtKhangHSo.Rows[0]["DD_GTLQ_SO"].ToString();
                        ngaycap = dtKhangHSo.Rows[0]["DD_GTLQ_NGAY_CAP"].ToString();
                        noicap = dtKhangHSo.Rows[0]["DD_GTLQ_NOI_CAP"].ToString();
                    }

                    idkhachhang = obj.HDTD_TD.ID_KHANG.ToString();
                    sotienvay = "0";
                    mahopdong = obj.HDTD_TD.MA_HDTD;
                    masanpham = txtMaSanPham.Text;
                }
                else
                {
                    makhachhang = "";
                    idkhachhang = "";
                    sotienvay = "0";
                    mahopdong = "";
                    masanpham = "";
                    tenkhachhang = "";
                    diachi = "";
                    gioitinh = "";
                    ngaysinh = "";
                    socmnd = "";
                    ngaycap = "";
                    noicap = "";
                }
                ngaybaocao = ClientInformation.NgayLamViecHienTai;

                lstThamSo.Add(new ThamSoBaoCao("@MaKhachHang", makhachhang, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                lstThamSo.Add(new ThamSoBaoCao("@IdKhachHang", idkhachhang, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                lstThamSo.Add(new ThamSoBaoCao("@SoTienVay", sotienvay, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                lstThamSo.Add(new ThamSoBaoCao("@MaHopDongTD", mahopdong, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
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

                string maBaoCao = DatabaseConstant.LayMaBaoCaoBIDV(DatabaseConstant.DanhSachBaoCaoBIDV.TDTT_HOP_DONG_TIN_DUNG);
                xemBaoCao.LayDuLieu(maBaoCao, lstThamSo);
            }
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
                idHDTDVM = obj.HDTD_TD.ID;
                TThaiNVu = obj.HDTD_TD.TTHAI_NVU;
                maHDTDVM = obj.HDTD_TD.MA_HDTD;

                objHDTD.ID = obj.HDTD_TD.ID;
                objHDTD.MA_HDTD = obj.HDTD_TD.MA_HDTD;

                txtSoHopDong.Text = obj.HDTD_TD.SO_HDTD;
                lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(obj.HDTD_TD.TTHAI_NVU);
                
                action = DatabaseConstant.Action.XEM;
                CommonFunction.RefreshButton(Toolbar, action, TThaiNVu, mnuMain, DatabaseConstant.Function.TDTD_HOP_DONG_CA_NHAN);
                SetEnabledAllControls(false);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }
        #endregion

        private void SetGtriTong(List<DataRow> lst)
        {
            decimal soTaiSan = 0;
            decimal tongGiaTriTS = 0;
            decimal tongGiaTriDB = 0;
            if (lst != null && lst.Count > 0)
            {
                foreach (DataRow dr in lst)
                {
                    tongGiaTriTS += LNumber.ToDecimal(dr["GTRI_TAI_SAN"]);
                    tongGiaTriDB += LNumber.ToDecimal(dr["GTRI_DAM_BAO"]);
                    soTaiSan++;
                }
            }
            lblSumTaiSan.Content = soTaiSan.ToString();
            lblSumGiaTriTS.Content = tongGiaTriTS.ToString("0,0.#");
            lblSumGiaTriDB.Content = tongGiaTriDB.ToString("0,0.#");
        }
    }
}
