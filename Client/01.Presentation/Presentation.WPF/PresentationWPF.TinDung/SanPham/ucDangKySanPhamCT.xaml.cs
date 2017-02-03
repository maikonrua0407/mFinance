using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Data;
using Microsoft.Windows.Controls.Ribbon;
using Utilities.Common;
using PresentationWPF.CustomControl;
using Presentation.Process;
using Presentation.Process.TinDungServiceRef;
using Presentation.Process.Common;
using Presentation.Process.PopupServiceRef;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using Presentation.Process.QuanTriHeThongServiceRef;
using System.Collections;
using System.Reflection;

namespace PresentationWPF.TinDung.SanPham
{
    /// <summary>
    /// Interaction logic for ucDangKySanPhamCT.xaml
    /// </summary>
    public partial class ucDangKySanPhamCT : UserControl
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
        List<AutoCompleteEntry> lstPhuongThucVay = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstMucDichSuDung = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstLoaiSanPham = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstNguonVonVay = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstNhomVongVay = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstPhuongThucTinhLai = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstCoSoTinhLai = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstLoaiLSuat = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstTanSuat = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstLoaiHachToan = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstLoaiTienKyQuy = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstLoaiTienLai = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstLoaiTienGoc = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstLoaiTienGop = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstPThucTinhKQuy = new List<AutoCompleteEntry>();
        private List<AutoCompleteEntry> lstThoiHanVay = new List<AutoCompleteEntry>();
        private List<AutoCompleteEntry> lstHinhThucTra = new List<AutoCompleteEntry>();
        private List<AutoCompleteEntry> lstHinhThucTraGoc = new List<AutoCompleteEntry>();
        private List<AutoCompleteEntry> lstHinhThucTraLai = new List<AutoCompleteEntry>();
        private List<AutoCompleteEntry> lstTienTe = new List<AutoCompleteEntry>();
        private List<AutoCompleteEntry> lstCanCuTinhLaiQH = new List<AutoCompleteEntry>();

        DataSet dsLaiSuat;
        DataTable dtGocLaiVayDS;
        int idSanPham = 0;
        int idLaiSuat = 0;
        int idCoSoTinhLai = 0;
        int idVongVay = 0;
        string maLaiSuat="";
        string maHinhThucChoVay="";
        string maMucDichSuDung="";
        string maLoaiSanPham="";
        string maNguonVon="";
        string maVongVay="";
        string maCoSoTLai="";
        string maLoaiLSuat="";
        string maTanSuatDanhGia="";
        string maPhuongThucTLai="";
        decimal dLaiSuat;
        string sTrangThai="";
        string sBaremTinhLai = "";
        List<DataRow> lstPopup = new List<DataRow>();
        public void LayDuLieuTuPopup(List<DataRow> lst)
        {
            lstPopup = lst;
        }
        public int IdSanPham
        {
            get { return idSanPham; }
            set { idSanPham = value; }
        }
        public event EventHandler OnSavingComleted = null;
        #endregion

        /// <summary>
        /// Khoi tao
        /// </summary>
        #region Khoi tao
        public ucDangKySanPhamCT()
        {
            InitializeComponent();
            HeThong htHeThong = new HeThong();
            htHeThong.DuyetQuyenTinhNangToolbar("/PresentationWPF.TinDung;component/SanPham/ucDangKySanPhamCT.xaml", ref Toolbar, ref mnuMain);
            foreach (var item in mnuMain.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += new RoutedEventHandler(btnShortcutKey_Click);
            }
            BindHotkey();
            InitEventHanler();
            // Khởi tạo giá trị cho ComboBox
            KhoiTaoComboBox();
            // Khoi tao gia tri cho Griview Bang ke goc lai
            KhoiTaoGiaTriBangGocLaiVayDS();
            ShowControl();
            ClearForm();
        }
        private void ShowControl()
        {
            HeThong hethong = new HeThong();
            ArrayList arr = new ArrayList();
            arr = hethong.SetVisibleControl("PresentationWPF.TinDung.SanPham.ucDangKySanPhamCT", "");
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
            sBaremTinhLai = new UtilitiesProcess().LayGiaTriThamSo(BusinessConstant.LoaiThamSo.TW, BusinessConstant.MaThamSo.TW_SO_TIEN_ADUNG_TINH_BAREM, ClientInformation.MaDonVi);
            grbLaiSuat.Header = LLanguage.SearchResourceByKey("U.TinDung.ucDangKySanPhamCT.GrLaiSuat", new string[1] { LNumber.ReadNumber(sBaremTinhLai.StringToDecimal()).ToLower() });
        }

        public void InitEventHanler()
        {
            txtMaLaiSuat.KeyDown +=new KeyEventHandler(txtMaLaiSuat_KeyDown);
            cmbLoaiHachToan.SelectionChanged += new SelectionChangedEventHandler(cmbLoaiHachToan_SelectionChanged);
            numSoTienGop.LostFocus += new RoutedEventHandler(numSoTienGop_LostFocus);
            rdbTuyetDoi.IsChecked = true;
        }

        void KhoiTaoComboBox()
        {
            List<string> lstDieuKien = new List<string>();
            List<string> lstMaChon = new List<string>();
            string maTruyVan = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();
            // Gán giá trị điều kiện
            lstDieuKien.Add(DatabaseConstant.getValue(DatabaseConstant.DanhMuc.PHUONG_THUC_VAY));
            KhoiTaoGiaTriComboBox(ref lstPhuongThucVay, ref cmbHinhThucVay, maTruyVan, lstDieuKien);
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.getValue(DatabaseConstant.DanhMuc.MUC_DICH_VAY_VON));
            KhoiTaoGiaTriComboBox(ref lstMucDichSuDung, ref cmbMucDichVayVon, maTruyVan, lstDieuKien);
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.getValue(DatabaseConstant.DanhMuc.LOAI_SAN_PHAM_TDUNG));
            KhoiTaoGiaTriComboBox(ref lstLoaiSanPham, ref cmbLoaiSanPham, maTruyVan, lstDieuKien, "TIN_CHAP");
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.getValue(DatabaseConstant.DanhMuc.PHUONG_THUC_TINH_LAI));
            KhoiTaoGiaTriComboBox(ref lstPhuongThucTinhLai, ref cmbPhuongThucTinh, maTruyVan, lstDieuKien);
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.getValue(DatabaseConstant.DanhMuc.DVI_TINH_KY_HAN));
            KhoiTaoGiaTriComboBox(ref lstThoiHanVay, ref cmbThoiHanVay, maTruyVan, lstDieuKien, BusinessConstant.DON_VI_TINH_THOI_GIAN.THANG.layGiaTri());
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.getValue(DatabaseConstant.DanhMuc.HINH_THUC_NOP_KQUY));
            KhoiTaoGiaTriComboBox(ref lstHinhThucTra, ref cmbHinhThucNop, maTruyVan, lstDieuKien);
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.getValue(DatabaseConstant.DanhMuc.PP_TINH_NOP_KQUY));
            KhoiTaoGiaTriComboBox(ref lstPThucTinhKQuy, ref cmbPPTinh, maTruyVan, lstDieuKien);
            //lstDieuKien = new List<string>();
            //lstDieuKien.Add(DatabaseConstant.getValue(DatabaseConstant.DanhMuc.LOAI_LAI_SUAT));
            //KhoiTaoGiaTriComboBox(ref lstLoaiLSuat, ref cmbLoaiSuat, maTruyVan, lstDieuKien);
            //lstDieuKien = new List<string>();
            //lstDieuKien.Add(DatabaseConstant.getValue(DatabaseConstant.DanhMuc.TAN_SUAT));
            //KhoiTaoGiaTriComboBox(ref lstTanSuat, ref cmbTanSuatDanhGia, maTruyVan, lstDieuKien);
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.getValue(DatabaseConstant.DanhMuc.DON_VI_HACH_TOAN));
            KhoiTaoGiaTriComboBox(ref lstLoaiHachToan, ref cmbLoaiHachToan, maTruyVan, lstDieuKien,DatabaseConstant.PHUONG_PHAP_HACH_TOAN.DOC_LAP.layGiaTri());
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.HINH_THUC_TRA_GOC.getValue());
            lstMaChon.Add(BusinessConstant.HINH_THUC_TRA_GOC.DINH_KY.layGiaTri());
            lstMaChon.Add(BusinessConstant.HINH_THUC_TRA_GOC.TRA_SAU.layGiaTri());
            KhoiTaoGiaTriComboBox(ref lstHinhThucTraGoc, ref cmbDinhKyTraGoc, maTruyVan, lstDieuKien, BusinessConstant.HINH_THUC_TRA_GOC.DINH_KY.layGiaTri(), lstMaChon);
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.HINH_THUC_TRA_LAI.getValue());
            KhoiTaoGiaTriComboBox(ref lstHinhThucTraLai, ref cmbDinhKyTraLai, maTruyVan, lstDieuKien, BusinessConstant.HINH_THUC_TRA_GOC.DINH_KY.layGiaTri());
            maTruyVan = "COMBOBOX_NHOMVONGVAY";
            lstNhomVongVay.Insert(0,new AutoCompleteEntry("", "", "0"));
            KhoiTaoGiaTriComboBox(ref lstNhomVongVay, ref cmbNhomVongVay, maTruyVan);
            maTruyVan = DatabaseConstant.DanhSachTruyVan.COMBOBOX_LOAI_LSUAT.getValue();
            KhoiTaoGiaTriComboBox(ref lstCoSoTinhLai, ref cmbCSTinhLai, maTruyVan);
            lstDieuKien.Clear();
            maTruyVan = DatabaseConstant.DanhSachTruyVan.COMBOBOX_LOAITIEN.getValue();
            KhoiTaoGiaTriComboBox(ref lstTienTe, ref cmbLoaiTien, maTruyVan, lstDieuKien, ClientInformation.MaDongNoiTe);
            KhoiTaoGiaTriComboBox(ref lstLoaiTienGop, ref cmbLoaiTienGop, maTruyVan, lstDieuKien, ClientInformation.MaDongNoiTe);
            KhoiTaoGiaTriComboBox(ref lstLoaiTienKyQuy, ref cmbLoaiTienKyQuy, maTruyVan, lstDieuKien, ClientInformation.MaDongNoiTe);
            KhoiTaoGiaTriComboBox(ref lstLoaiTienLai, ref cmbLoaiTienLai, maTruyVan, lstDieuKien, ClientInformation.MaDongNoiTe);
            KhoiTaoGiaTriComboBox(ref lstLoaiTienGoc, ref cmbLoaiTienGoc, maTruyVan, lstDieuKien, ClientInformation.MaDongNoiTe);

            lstDieuKien = new List<string>();
            maTruyVan = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.CAN_CU_TINH_LAI_QH.getValue());
            KhoiTaoGiaTriComboBox(ref lstCanCuTinhLaiQH, ref cmbCanCuTinhLaiQuaHan, maTruyVan, lstDieuKien, "CONG_TI_LE");
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
                        KeyGesture keyg = new KeyGesture(Key.I, ModifierKeys.Control);
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
                        KeyGesture keyg = new KeyGesture(Key.V, ModifierKeys.Control|ModifierKeys.Shift);
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
                        KeyGesture keyg = new KeyGesture(Key.S, ModifierKeys.Control|ModifierKeys.Shift);
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
                        KeyGesture keyg = new KeyGesture(Key.W,ModifierKeys.Control | ModifierKeys.Shift);
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
                        KeyGesture keyg = new KeyGesture(Key.F, ModifierKeys.Control | ModifierKeys.Shift);
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
            RibbonButton tlb = (RibbonButton)sender;
            string strTinhNang = tlb.Name.Substring(3, tlb.Name.Length - 3);
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.NHAP_DU_LIEU)))
            { }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THEM)))
            {
                ClearForm();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
                Sua();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
            {
                Xoa();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.NHAN_BAN)))
            { }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU_TAM)))
            {
                LuuDuLieu(BusinessConstant.TrangThaiBanGhi.SU_DUNG, BusinessConstant.TrangThaiNghiepVu.LUU_TAM, DatabaseConstant.Action.LUU_TAM);
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRINH_DUYET)))
            {
                LuuDuLieu(BusinessConstant.TrangThaiBanGhi.SU_DUNG, BusinessConstant.TrangThaiNghiepVu.CHO_DUYET, DatabaseConstant.Action.TRINH_DUYET);
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.BANG_KE)))
            { }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DUYET)))
            {
                Duyet();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET)))
            {
                TuChoiDuyet();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
            {
                ThoaiDuyet();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XEM_TRUOC)))
            {
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
                ClearForm();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
                Sua();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
            {
                Xoa();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.NHAN_BAN)))
            { }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU_TAM)))
            {
                LuuDuLieu(BusinessConstant.TrangThaiBanGhi.SU_DUNG, BusinessConstant.TrangThaiNghiepVu.LUU_TAM, DatabaseConstant.Action.LUU_TAM);
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU)))
            {
                LuuDuLieu(BusinessConstant.TrangThaiBanGhi.SU_DUNG, BusinessConstant.TrangThaiNghiepVu.CHO_DUYET, DatabaseConstant.Action.LUU);
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.BANG_KE)))
            { }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DUYET)))
            {
                Duyet();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET)))
            {
                TuChoiDuyet();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
            {
                ThoaiDuyet();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XEM_TRUOC)))
            {
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbNhomVongVay_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbNhomVongVay.SelectedIndex > 0)
                {
                    idVongVay = Convert.ToInt32(lstNhomVongVay.ElementAt(cmbNhomVongVay.SelectedIndex).KeywordStrings.ElementAt(1));
                    maVongVay = lstNhomVongVay.ElementAt(cmbNhomVongVay.SelectedIndex).KeywordStrings.First();
                    TinhToanBangKeGocLai();
                }
                else
                {
                    titemsGocLaiVay.Visibility = System.Windows.Visibility.Collapsed;
                }
            }
            catch (Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        /// <summary>
        /// Lấy mã hình thức cho vay
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbHinhThucVay_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbHinhThucVay.SelectedIndex >= 0)
                maHinhThucChoVay = lstPhuongThucVay.ElementAt(cmbHinhThucVay.SelectedIndex).KeywordStrings.First();
        }

        /// <summary>
        /// Lấy mã mục đích vay vốn
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbMucDichVayVon_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbMucDichVayVon.SelectedIndex >= 0)
                maMucDichSuDung = lstMucDichSuDung.ElementAt(cmbMucDichVayVon.SelectedIndex).KeywordStrings.First();
        }

        /// <summary>
        /// Lấy mã loại sản phẩm
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbLoaiSanPham_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbLoaiSanPham.SelectedIndex >= 0)
                maLoaiSanPham = lstLoaiSanPham.ElementAt(cmbLoaiSanPham.SelectedIndex).KeywordStrings.First();
        }

        /// <summary>
        /// Lấy mã phương thức tính lãi
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbPhuongThucTinh_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbPhuongThucTinh.SelectedIndex >= 0)
                maPhuongThucTLai = lstPhuongThucTinhLai.ElementAt(cmbPhuongThucTinh.SelectedIndex).KeywordStrings.First();
        }

        /// <summary>
        /// Lấy mã cơ sở tỉnh lãi
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbCSTinhLai_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbCSTinhLai.SelectedIndex < 0)
                return;
            maCoSoTLai = lstCoSoTinhLai.ElementAt(cmbCSTinhLai.SelectedIndex).KeywordStrings.First();
            idCoSoTinhLai = Convert.ToInt32(lstCoSoTinhLai.ElementAt(cmbCSTinhLai.SelectedIndex).KeywordStrings.ElementAt(1));
        }

        /// <summary>
        /// Lấy mã loại lãi suất
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void cmbLoaiSuat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (cmbLoaiSuat.SelectedIndex < 0)
        //        return;
        //    maLoaiLSuat = lstLoaiLSuat.ElementAt(cmbLoaiSuat.SelectedIndex).KeywordStrings.First();
        //    if (maLoaiLSuat.Equals(BusinessConstant.LOAI_LAI_SUAT.CO_DINH.layGiaTri()))
        //    {
        //        txtTanSuatDanhGia.IsEnabled = false;
        //        cmbTanSuatDanhGia.IsEnabled = false;
        //    }
        //    else
        //    {
        //        txtTanSuatDanhGia.IsEnabled = true;
        //        cmbTanSuatDanhGia.IsEnabled = true;
        //    }
        //}

        /// <summary>
        /// Lấy mã tần suất đánh giá lãi suất
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void cmbTanSuatDanhGia_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (cmbTanSuatDanhGia.SelectedIndex >= 0)
        //        maTanSuatDanhGia = lstTanSuat.ElementAt(cmbTanSuatDanhGia.SelectedIndex).KeywordStrings.First();
        //}

        /// <summary>
        /// Sự kiện chọn lãi suất
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMaLSuat_Click(object sender, RoutedEventArgs e)
        {
            PopupProcess popupProcess = new PopupProcess();
            List<string> lstDieuKien = new List<string>();
            lstDieuKien.Add("TDVM");
            lstDieuKien.Add(ClientInformation.MaDonVi);
            popupProcess.getPopupInformation("POPUP_DS_LAISUAT", lstDieuKien);
            SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;
            ucPopup popup = new ucPopup(false, simplePopupResponse);
            popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
            Window win = new Window();
            win.Content = popup;
            win.Title = LLanguage.SearchResourceByKey(simplePopupResponse.PopupTitle);
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.ShowDialog();
            if (lstPopup.Count > 0)
            {
                idLaiSuat = Convert.ToInt32(lstPopup[0]["ID"]);
                LayThongTinLSuat();
            }
        }

        private void txtMaLaiSuat_LostFocus(object sender, RoutedEventArgs e)
        {
            if(txtMaLaiSuat.Text.IsNullOrEmptyOrSpace())
                idLaiSuat = 0;
            else
            {
                DataSet ds = new LaiSuatProcess().GetDSLaiSuat(ClientInformation.MaDonVi);
                List<DataRow> lstdr = ds.Tables["LAI_SUAT_DS"].Select("MA_LSUAT='" + txtMaLaiSuat.Text + "' AND TTHAI_NVU='DDU'").ToList();
                if (!LObject.IsNullOrEmpty(lstdr) && lstdr.Count > 0)
                    idLaiSuat = Convert.ToInt32(lstdr[0]["ID"]);
                else
                    idLaiSuat = 0;
            }
            LayThongTinLSuat();
        }

        /// <summary>
        /// Khởi tạo lấy Items cho ComboBox 
        /// </summary>
        /// <param name="lstAutoComplete"></param>
        /// <param name="cmbControl"></param>
        /// <param name="sMaTruyVan"></param>
        /// <param name="lstDKien"></param>
        /// <param name="bSelectChanged"></param>
        void KhoiTaoGiaTriComboBox(ref List<AutoCompleteEntry> lstAutoComplete, ref RadComboBox cmbControl, string sMaTruyVan, List<string> lstDKien = null, string Chon = null, List<string> lstMaChon = null)
        {
            AutoComboBox autoComboBox = new AutoComboBox();
            autoComboBox.GenAutoComboBox(ref lstAutoComplete, ref cmbControl, sMaTruyVan, lstDKien, Chon, lstMaChon);
        }

        /// <summary>
        /// Khoi tao gia tri cho GridView Goc Lai vay
        /// </summary>
        void KhoiTaoGiaTriBangGocLaiVayDS()
        {
            dtGocLaiVayDS = new DataTable();
            dtGocLaiVayDS.Columns.Add(LLanguage.SearchResourceByKey("U.TinDung.ucDangKySanPhamCT.Vong"), typeof(string));
            dtGocLaiVayDS.Columns.Add(LLanguage.SearchResourceByKey("U.TinDung.ucDangKySanPhamCT.GocVay"), typeof(decimal));
            dtGocLaiVayDS.Columns.Add(LLanguage.SearchResourceByKey("U.TinDung.ucDangKySanPhamCT.LaiVay"), typeof(decimal));
            dtGocLaiVayDS.Columns.Add(LLanguage.SearchResourceByKey("U.TinDung.ucDangKySanPhamCT.GocLai"), typeof(decimal));
            raddgrGocLaiVayDS.ItemsSource = null;
        }

        /// <summary>
        /// Clear form dữ liệu
        /// </summary>
        void ClearForm()
        {
            cmbHinhThucVay.Focus();
            txtMaLaiSuat.Text = "";
            txtMaSanPham.Text = "";
            idSanPham = 0;
            
            txtTenSanPham.Text = "";
            txtMaLaiSuat.Text = "";
            txtBienDo.Text = "";
            txtLaiSuat.Value = null;
            //txtTanSuatDanhGia.Value = null;
            KhoiTaoGiaTriBangGocLaiVayDS();
            lblTrangThai.Content = "";
            txtNguoiLap.Text = "";
            txtNguoiCapNhat.Text = "";
            teldtNgayHieuLuc.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);
            teldtNgayHetHieuLuc.Value = null;
            teldtNgayNhap.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);
            teldtNgayCNhat.Value = null;
            txtTrangThai.Text = sTrangThai = "";
            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.THEM, sTrangThai, mnuMain, DatabaseConstant.Function.TDVM_SAN_PHAM_TDVM);
            LoadDuLieuTaiKhoanHachToan("MACDINH");
            grbGocLaiVay.IsEnabled = true;
            grbThongTinChung.IsEnabled = true;
            grbLaiSuat.IsEnabled = true;
            if (lstPhuongThucVay.Count > 0)
                cmbHinhThucVay.SelectedIndex = 0;
        }

        void LockControl(List<Control> lstControl, bool bBool)
        {
            foreach (Control control in lstControl)
            {
                control.IsEnabled = bBool;
            }
            grdTKhoan.IsReadOnly = !bBool;
        }

        /// <summary>
        /// Load dữ liệu chi tiết cho các control
        /// </summary>
        /// <param name="bSua"></param>
        public void LoadDuLieuCT(bool bSua)
        {
            try
            {
                DataSet dsChiTiet = new TinDungProcess().getSanPhamTDByID(idSanPham.ToString());
                if (dsChiTiet.Tables[0].Rows.Count > 0)
                {
                    LoadDuLieuThongTinChung(dsChiTiet);
                    LoadDuLieuKiemSoat(dsChiTiet);
                    LoadDuLieuTaiKhoanHachToan(txtMaSanPham.Text);
                    TinhSoTienGopHangKy();
                }
                List<Control> lstConTrol = new List<Control>();
                lstConTrol.Add((Control)grbGocLaiVay);
                lstConTrol.Add((Control)grbThongTinChung);
                lstConTrol.Add((Control)grbLaiSuat);
                lstConTrol.Add((Control)raddgrGocLaiVayDS);
                lstConTrol.Add((Control)grbKyQuy);
                lstConTrol.Add((Control)grbHinhThuc);
                if (!bSua)
                {
                    LockControl(lstConTrol, false);
                    CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, dsChiTiet.Tables[0].Rows[0]["TTHAI_NVU"].ToString(), mnuMain, DatabaseConstant.Function.TDVM_SAN_PHAM_TDVM);
                }
                else
                {
                    LockControl(lstConTrol, true);
                    CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.SUA, dsChiTiet.Tables[0].Rows[0]["TTHAI_NVU"].ToString(), mnuMain, DatabaseConstant.Function.TDVM_SAN_PHAM_TDVM);
                }
            }
            catch (Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        /// <summary>
        /// Đưa dữ liệu vào các điều khiển tab thông tin chung
        /// </summary>
        /// <param name="dsThongTin"></param>
        private void LoadDuLieuThongTinChung(DataSet dsThongTin)
        {
            lblTrangThai.Content = dsThongTin.Tables[0].Rows[0]["TEN_TTHAI_NVU"].ToString();
            sTrangThai = dsThongTin.Tables[0].Rows[0]["TTHAI_NVU"].ToString();
            txtMaSanPham.Text = dsThongTin.Tables[0].Rows[0]["MA_SAN_PHAM"].ToString();
            cmbHinhThucVay.SelectedIndex = lstPhuongThucVay.IndexOf(lstPhuongThucVay.FirstOrDefault(i => i.KeywordStrings.FirstOrDefault().Equals(dsThongTin.Tables[0].Rows[0]["PTHUC_VAY"])));
            txtTenSanPham.Text = dsThongTin.Tables[0].Rows[0]["TEN_SAN_PHAM"].ToString();
            cmbMucDichVayVon.SelectedIndex = lstMucDichSuDung.IndexOf(lstMucDichSuDung.FirstOrDefault(i => i.KeywordStrings.FirstOrDefault().Equals(dsThongTin.Tables[0].Rows[0]["MUC_DICH_VAY"])));
            cmbLoaiSanPham.SelectedIndex = lstLoaiSanPham.IndexOf(lstLoaiSanPham.FirstOrDefault(i => i.KeywordStrings.FirstOrDefault().Equals(dsThongTin.Tables[0].Rows[0]["LOAI_SAN_PHAM"])));
            cmbNhomVongVay.SelectedIndex = lstNhomVongVay.IndexOf(lstNhomVongVay.FirstOrDefault(i => i.KeywordStrings.FirstOrDefault().Equals(dsThongTin.Tables[0].Rows[0]["MA_VONG_VAY"])));
            
            teldtNgayHieuLuc.Value = LDateTime.StringToDate(dsThongTin.Tables[0].Rows[0]["NGAY_ADUNG"].ToString(), "yyyyMMdd");
            if (!dsThongTin.Tables[0].Rows[0]["NGAY_HHAN"].ToString().IsNullOrEmptyOrSpace())
                teldtNgayHetHieuLuc.Value = LDateTime.StringToDate(dsThongTin.Tables[0].Rows[0]["NGAY_HHAN"].ToString(), "yyyyMMdd");
            cmbPhuongThucTinh.SelectedIndex = lstPhuongThucTinhLai.IndexOf(lstPhuongThucTinhLai.FirstOrDefault(i => i.KeywordStrings.FirstOrDefault().Equals(dsThongTin.Tables[0].Rows[0]["TLAI_PTHUC"])));
            cmbCSTinhLai.SelectedIndex = lstCoSoTinhLai.IndexOf(lstCoSoTinhLai.FirstOrDefault(i => i.KeywordStrings.FirstOrDefault().Equals(dsThongTin.Tables[0].Rows[0]["MA_CSO_TLAI"])));
            string LoaiKyQuy = dsThongTin.Tables[0].Rows[0]["LOAI_KQUY"].ToString();
            if (LoaiKyQuy.Equals(BusinessConstant.PPHAP_TINH_RGOC.TUYETDOI.layGiaTri()))
            {
                rdbTuyetDoi.IsChecked = true;
                numSoTienKyQuy.Value = Convert.ToDouble(dsThongTin.Tables[0].Rows[0]["SO_TIEN_KQUY"]);
            }
            else
            {
                rdbTuongDoi.IsChecked = true;
                numTyLeKyQuy.Value = Convert.ToDouble(dsThongTin.Tables[0].Rows[0]["SO_TIEN_KQUY"]);
            }
            cmbHinhThucNop.SelectedIndex = lstHinhThucTra.IndexOf(lstHinhThucTra.FirstOrDefault(i => i.KeywordStrings.FirstOrDefault().Equals(dsThongTin.Tables[0].Rows[0]["HINH_THUC_NOP_KQUY"])));
            cmbPPTinh.SelectedIndex = lstPThucTinhKQuy.IndexOf(lstPThucTinhKQuy.FirstOrDefault(i => i.KeywordStrings.FirstOrDefault().Equals(dsThongTin.Tables[0].Rows[0]["PP_TINH_KQUY"])));
            cmbLoaiTien.SelectedIndex = lstTienTe.IndexOf(lstTienTe.FirstOrDefault(i => i.KeywordStrings.FirstOrDefault().Equals(dsThongTin.Tables[0].Rows[0]["MA_LOAI_TIEN"])));
            numSoTienGop.Value = Convert.ToDouble(dsThongTin.Tables[0].Rows[0]["SO_TIEN_GOP"]);
            idSanPham = Convert.ToInt32(dsThongTin.Tables[0].Rows[0]["ID"]);
            idLaiSuat = Convert.ToInt32(dsThongTin.Tables[0].Rows[0]["ID_LSUAT"]);
            idCoSoTinhLai = Convert.ToInt32(dsThongTin.Tables[0].Rows[0]["ID_CS_TLAI"]);
            cmbCanCuTinhLaiQuaHan.SelectedIndex = lstCanCuTinhLaiQH.IndexOf(lstCanCuTinhLaiQH.FirstOrDefault(i => i.KeywordStrings.FirstOrDefault().Equals(dsThongTin.Tables[0].Rows[0]["CAN_CU_TINH_LAI_QH"].ToString())));
            numTyLeLaiQuaHan.Value = Convert.ToDouble(dsThongTin.Tables[0].Rows[0]["TY_LE_TINH_LAI_QH"]);
            LayThongTinLSuat();
        }

        /// <summary>
        /// Đưa dữ liệu vào các điều khiển tab kiểm soát
        /// </summary>
        /// <param name="dsThongTin"></param>
        private void LoadDuLieuKiemSoat(DataSet dsThongTin)
        {
            txtTrangThai.Text = dsThongTin.Tables[0].Rows[0]["TEN_TTHAI_BGHI"].ToString();
            teldtNgayNhap.Value = LDateTime.StringToDate(dsThongTin.Tables[0].Rows[0]["NGAY_NHAP"].ToString(), "yyyyMMdd");
            txtNguoiLap.Text = dsThongTin.Tables[0].Rows[0]["NGUOI_NHAP"].ToString();
            if (dsThongTin.Tables[0].Rows[0]["NGAY_CNHAT"].ToString().Length >= 8)
                teldtNgayCNhat.Value = LDateTime.StringToDate(dsThongTin.Tables[0].Rows[0]["NGAY_CNHAT"].ToString(), "yyyyMMdd");
            else
                teldtNgayCNhat.Value = null;
            txtNguoiCapNhat.Text = dsThongTin.Tables[0].Rows[0]["NGUOI_CNHAT"].ToString();
        }

        private void tlbTaiKhoanHachToan_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void LoadDuLieuTaiKhoanHachToan(string sDoiTuong)
        {
            DataTable dt = new TinDungProcess().GetTaiKhoanHachToan(sDoiTuong, ClientInformation.MaDonVi).Tables["TAI_KHOAN_HACH_TOAN"];
            grdTKhoan.ItemsSource = dt.DefaultView;
        }

        private void PhanLoaiTK_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            GridViewRow grrow = btn.ParentOfType<GridViewRow>();
            PhanLoaiTaiKhoan(grrow);
        }

        private void PhanLoaiTK_LostFocus(object sender, RoutedEventArgs e)
        {
            var txt = sender as TextBox;

        }

        private void txtBienDo_ValueChanged(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            txtLaiSuat.Value = Convert.ToDouble(dLaiSuat) + txtBienDo.Value.GetValueOrDefault(0);
        }

        private void PhanLoaiTKBSO_LostFocus(object sender, RoutedEventArgs e)
        {
            var txt = sender as TextBox;
        }

        private void PhanLoaiTKBSO_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            GridViewRow grrow = btn.ParentOfType<GridViewRow>();
            PhanLoaiTaiKhoanBSo(grrow);
        }

        private void txtMaLaiSuat_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F3)
                btnMaLSuat_Click(sender, null);
        }

        private void PhanLoaiTK_KeyDown(object sender, KeyEventArgs e)
        {
            var txt = sender as TextBox;
            GridViewRow grrow = txt.ParentOfType<GridViewRow>();
            PhanLoaiTaiKhoan(grrow);
        }

        private void PhanLoaiTKBSO_KeyDown(object sender, KeyEventArgs e)
        {
            var txt = sender as TextBox;
            GridViewRow grrow = txt.ParentOfType<GridViewRow>();
            PhanLoaiTaiKhoanBSo(grrow);
        } 

        private void PhanLoaiTaiKhoan(GridViewRow grrow)
        {
            try
            {
                DataRowView drv = grrow.Item as DataRowView;
                string maKyHieu = drv["MA_KY_HIEU"].ToString();
                string maPhanLoai = "%";
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
                win.Content = popup;
                win.Title = LLanguage.SearchResourceByKey(simplePopupResponse.PopupTitle);
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.ShowDialog();
                if (lstPopup != null && lstPopup.Count > 0)
                {
                    DataRow row = lstPopup[0];
                    drv["MA_PLOAI"] = row[2].ToString();
                    drv["TEN_PLOAI"] = row[3].ToString();
                    drv["MA_PLOAI_BSO"] = row[2].ToString();
                    drv["TEN_PLOAI"] = row[3].ToString();
                    grdTKhoan.CurrentItem = drv;
                }
            }
            catch (System.Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void PhanLoaiTaiKhoanBSo(GridViewRow grrow)
        {
            try
            {
                DataRowView drv = grrow.Item as DataRowView;
                string maKyHieu = drv["MA_KY_HIEU"].ToString();
                string maPhanLoai = "%";
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
                win.Content = popup;
                win.Title = LLanguage.SearchResourceByKey(simplePopupResponse.PopupTitle);
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.ShowDialog();
                if (lstPopup != null && lstPopup.Count > 0)
                {
                    DataRow row = lstPopup[0];
                    drv["MA_PLOAI_BSO"] = row[2].ToString();
                    drv["TEN_PLOAI_BSO"] = row[3].ToString();
                    grdTKhoan.CurrentItem = drv;
                }
            }
            catch (System.Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void cmbLoaiHachToan_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string PPHachToan = lstLoaiHachToan.ElementAt(cmbLoaiHachToan.SelectedIndex).KeywordStrings.FirstOrDefault();
            if (PPHachToan.Equals(DatabaseConstant.PHUONG_PHAP_HACH_TOAN.DOC_LAP.layGiaTri()))
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
            if (ClientInformation.MaDonViGiaoDich.Equals(ClientInformation.MaDonVi))
            {
                grdTKhoan.Columns[6].IsVisible = true;
                grdTKhoan.Columns[7].IsVisible = true;
                grdTKhoan.Columns[8].IsVisible = true;
                grdTKhoan.Columns[9].IsVisible = true;
            }
        }

        private void groupKyQuy_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton rdo = sender as RadioButton;
            if (rdo.Name.Equals("rdbTuyetDoi"))
            {
                lblSoTienKyQuy.Visibility = System.Windows.Visibility.Visible;
                numSoTienKyQuy.Visibility = System.Windows.Visibility.Visible;
                cmbLoaiTienKyQuy.Visibility = System.Windows.Visibility.Visible;
                lblTyLeKyQuy.Visibility = System.Windows.Visibility.Hidden;
                numTyLeKyQuy.Visibility = System.Windows.Visibility.Hidden;
                cmbPPTinh.Visibility = System.Windows.Visibility.Hidden;
                
            }
            else
            {
                lblSoTienKyQuy.Visibility = System.Windows.Visibility.Hidden;
                numSoTienKyQuy.Visibility = System.Windows.Visibility.Hidden;
                cmbLoaiTienKyQuy.Visibility = System.Windows.Visibility.Hidden;
                lblTyLeKyQuy.Visibility = System.Windows.Visibility.Visible;
                numTyLeKyQuy.Visibility = System.Windows.Visibility.Visible;
                cmbPPTinh.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void TinhSoTienGopHangKy()
        {
            decimal ThoiGianVay = (decimal)telThoiGianVay.Value.GetValueOrDefault();
            decimal SoTienGop = (decimal)numSoTienGop.Value.GetValueOrDefault();
            decimal SoTienGoc = 0;
            decimal SoTienLai = 0;
            int iret = new TinDungProcess().BaremTinhLaiTienVay(SoTienGop, ThoiGianVay, out SoTienGoc, out SoTienLai, ClientInformation.MaDonVi);
            if (iret > 0)
            {
                numSoTienGoc.Value = Convert.ToDouble(SoTienGoc);
                numSoTienLai.Value = Convert.ToDouble(SoTienLai);
            }
            else
            {
                numSoTienGoc.Value = 0;
                numSoTienLai.Value = 0;
            }
        }

        void numSoTienGop_LostFocus(object sender, RoutedEventArgs e)
        {
            TinhSoTienGopHangKy();
        }
        #endregion

        /// <summary>
        /// Xu ly nghiep vu
        /// </summary>
        #region Xu ly nghiep vu

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objSP"></param>
        /// <param name="bangghi"></param>
        /// <param name="nghiepvu"></param>
        void LayDuLieu(ref TD_SAN_PHAM objSP,ref List<KT_PHAN_HE_PLOAI> lstPhanHe, BusinessConstant.TrangThaiBanGhi bangghi, BusinessConstant.TrangThaiNghiepVu nghiepvu)
        {
            string sLoaiKyQuy = BusinessConstant.PPHAP_TINH_RGOC.TUYETDOI.layGiaTri();
            decimal SoTienKyQuy = 0;
            string sHinhThucNop = "NOP_1LAN";
            string sPPhapTinh = "TONG_DNO_KUOC";
            string sMaLoaiTien = ClientInformation.MaDongNoiTe;
            string sCanCuTinhLQH = "CONG_TI_LE";
            if (rdbTuyetDoi.IsChecked.GetValueOrDefault())
            {
                sLoaiKyQuy = BusinessConstant.PPHAP_TINH_RGOC.TUYETDOI.layGiaTri();
                SoTienKyQuy = (decimal)numSoTienKyQuy.Value.GetValueOrDefault();
            }
            if (rdbTuongDoi.IsChecked.GetValueOrDefault())
            {
                sLoaiKyQuy = BusinessConstant.PPHAP_TINH_RGOC.TUONGDOI.layGiaTri();
                SoTienKyQuy = (decimal)numTyLeKyQuy.Value.GetValueOrDefault();
            }
            if(cmbHinhThucNop.SelectedIndex>-1)
            {
                AutoCompleteEntry auHinhThucNop = lstHinhThucTra.ElementAt(cmbHinhThucNop.SelectedIndex);
                if (!LObject.IsNullOrEmpty(auHinhThucNop))
                    sHinhThucNop = auHinhThucNop.KeywordStrings[0];
            }
            if (cmbPPTinh.SelectedIndex > -1)
            {
                AutoCompleteEntry auPPhapTinh = lstPThucTinhKQuy.ElementAt(cmbPPTinh.SelectedIndex);
                if (!LObject.IsNullOrEmpty(auPPhapTinh))
                    sPPhapTinh = auPPhapTinh.KeywordStrings[0];
            }
            if (cmbLoaiTien.SelectedIndex > -1)
            {
                AutoCompleteEntry auLoaiTien = lstTienTe.ElementAt(cmbLoaiTien.SelectedIndex);
                if (!LObject.IsNullOrEmpty(auLoaiTien))
                    sMaLoaiTien = auLoaiTien.KeywordStrings[0];
            }
            if (cmbCanCuTinhLaiQuaHan.SelectedIndex > -1)
            {
                AutoCompleteEntry auCanCuLQH = lstCanCuTinhLaiQH.ElementAt(cmbCanCuTinhLaiQuaHan.SelectedIndex);
                if (!LObject.IsNullOrEmpty(auCanCuLQH))
                    sCanCuTinhLQH = auCanCuLQH.KeywordStrings[0];
            }
            objSP.ID_CS_TLAI = idCoSoTinhLai;
            objSP.ID_LSUAT = idLaiSuat;
            objSP.MA_NHOM_SP = "";
            objSP.NGAY_ADUNG = teldtNgayHieuLuc.Value != null ? LDateTime.DateToString((DateTime)teldtNgayHieuLuc.Value, ApplicationConstant.defaultDateTimeFormat) : "";
            objSP.LOAI_SAN_PHAM = maLoaiSanPham;
            objSP.NGAY_HHAN = teldtNgayHetHieuLuc.Value != null ? LDateTime.DateToString((DateTime)teldtNgayHetHieuLuc.Value, ApplicationConstant.defaultDateTimeFormat) : "";
            objSP.TEN_SAN_PHAM = txtTenSanPham.Text;
            objSP.MA_VONG_VAY = maVongVay;
            objSP.MA_LOAI_TIEN = sMaLoaiTien;
            objSP.PTHUC_VAY = maHinhThucChoVay;
            objSP.LOAI_THAN_VAY = "";
            objSP.NGUON_VAY = maNguonVon;
            objSP.MUC_DICH_VAY = maMucDichSuDung;
            objSP.MA_LSUAT = maLaiSuat;
            objSP.MA_CSO_TLAI = maCoSoTLai;
            objSP.LSUAT_BIEN_DO = txtBienDo.Value != null ? Convert.ToDecimal((double)txtBienDo.Value) : 0;
            objSP.TLAI_DVI_TINH = "";
            objSP.TLAI_PTHUC = maPhuongThucTLai;
            objSP.DKY_TDOI_LSUAT = maTanSuatDanhGia;
            objSP.LOAI_LSUAT = maLoaiLSuat;
            objSP.DGIA_SO_DKY = null;
            objSP.DGIA_DVI_TINH = null;
            objSP.TLE_LPHAT = null;
            objSP.CTHI_QHAN = null;
            objSP.CTHI_DTHU = null;
            objSP.HAN_MUC = null;
            objSP.MA_HMUC = null;
            objSP.NTAC_LTRON = null;
            objSP.LOAI_KQUY = sLoaiKyQuy;
            objSP.SO_TIEN_KQUY = SoTienKyQuy;
            objSP.HINH_THUC_NOP_KQUY = sHinhThucNop;
            objSP.PP_TINH_KQUY = sPPhapTinh;
            objSP.SO_TIEN_GOP = (decimal)numSoTienGop.Value.GetValueOrDefault();
            objSP.TTHAI_BGHI = BusinessConstant.layGiaTri(bangghi);
            objSP.TTHAI_NVU = BusinessConstant.layGiaTri(nghiepvu);
            objSP.CAN_CU_TINH_LAI_QH = sCanCuTinhLQH;
            objSP.TY_LE_TINH_LAI_QH = Convert.ToDecimal(numTyLeLaiQuaHan.Value.GetValueOrDefault());
            objSP.NGUOI_NHAP = ClientInformation.TenDangNhap;
            objSP.NGAY_NHAP = ClientInformation.NgayLamViecHienTai;
            if (idSanPham > 0)
            {
                objSP.MA_SAN_PHAM = txtMaSanPham.Text;
                objSP.ID = idSanPham;
                objSP.NGUOI_NHAP = txtNguoiLap.Text;
                objSP.NGAY_NHAP = LDateTime.DateToString((DateTime)teldtNgayNhap.Value, ApplicationConstant.defaultDateTimeFormat);
                objSP.NGUOI_CNHAT = ClientInformation.TenDangNhap;
                objSP.NGAY_CNHAT = ClientInformation.NgayLamViecHienTai;
            }
            else
            {
                objSP.MA_DVI_QLY = ClientInformation.MaDonVi;
                objSP.MA_DVI_TAO = ClientInformation.MaDonViGiaoDich;
            }
            DataView dv = (DataView)grdTKhoan.ItemsSource;
            foreach(DataRowView drv in dv)
                {
                    KT_PHAN_HE_PLOAI objPhanHePLoai = new KT_PHAN_HE_PLOAI();
                    objPhanHePLoai.ID_PHAN_HE = 0;
                    objPhanHePLoai.ID = Convert.ToInt32(drv["ID"]);
                    objPhanHePLoai.MA_DTUONG = txtMaSanPham.Text;
                    objPhanHePLoai.MA_PHAN_HE = DatabaseConstant.Module.TDVM.getValue();
                    objPhanHePLoai.MA_KY_HIEU = drv["MA_KY_HIEU"].ToString();
                    objPhanHePLoai.MA_PLOAI = drv["MA_PLOAI"].ToString();
                    objPhanHePLoai.MA_PLOAI_BSO = drv["MA_PLOAI_BSO"].ToString();
                    objPhanHePLoai.TTHAI_BGHI = BusinessConstant.layGiaTri(bangghi);
                    objPhanHePLoai.TTHAI_NVU = BusinessConstant.layGiaTri(nghiepvu);
                    objPhanHePLoai.MA_DVI_QLY = ClientInformation.MaDonVi;
                    objPhanHePLoai.MA_DVI_TAO = ClientInformation.MaDonViGiaoDich;
                    objPhanHePLoai.NGUOI_NHAP = ClientInformation.TenDangNhap;
                    objPhanHePLoai.NGAY_NHAP = ClientInformation.NgayLamViecHienTai;
                    lstPhanHe.Add(objPhanHePLoai);
                }
        }

        /// <summary>
        /// Check dữ liệu trước khi lưu
        /// </summary>
        /// <returns></returns>
        bool VadidateData()
        {
            if (txtTenSanPham.Text.IsNullOrEmptyOrSpace())
            {
                CommonFunction.ThongBaoTrong(lblTenSPham.Content.ToString());
                txtTenSanPham.Focus();
                return false;
            }
            else if (cmbCSTinhLai.Text.IsNullOrEmptyOrSpace())
            {
                CommonFunction.ThongBaoTrong(lblTenSPham.Content.ToString());
                cmbCSTinhLai.Focus();
                return false;
            }
            else if (numSoTienGop.IsVisible && numSoTienGop.Value.IsNullOrEmpty())
            {
                CommonFunction.ThongBaoTrong(lblSoTienGop.Content.ToString());
                numSoTienGop.Focus();
                return false;
            }
            else if (numSoTienGoc.IsVisible && numSoTienGoc.Value.GetValueOrDefault(0) < 1)
            {
                CommonFunction.ThongBaoTrong(lblSoTienGoc.Content.ToString());
                numSoTienGop.Focus();
                return false;
            }
            else if (!teldtNgayHetHieuLuc.Value.IsNullOrEmpty())
            {
                if (teldtNgayHetHieuLuc.Value < teldtNgayHieuLuc.Value)
                {
                    LMessage.ShowMessage("M.DungChung.ThongBao.NgayHetHanNhoNgayHieuLuc", LMessage.MessageBoxType.Warning);
                    teldtNgayHetHieuLuc.Focus();
                    return false;
                }
                return true;
            }
            else
                return true;
        }

        /// <summary>
        /// Lấy thông tin nhóm vòng vay vốn
        /// </summary>

        /// <summary>
        /// Lấy thông tin lãi suất
        /// </summary>
        private void LayThongTinLSuat()
        {
            TinDungProcess tindungProcess = new TinDungProcess();
            dsLaiSuat = tindungProcess.getLaiSuatByID(idLaiSuat.ToString());
            if (dsLaiSuat != null & dsLaiSuat.Tables[0].Rows.Count > 0 )
            {

                maLaiSuat = txtMaLaiSuat.Text = dsLaiSuat.Tables[0].Rows[0]["MA_LSUAT"].ToString();
                lblTenLSuat.Content = dsLaiSuat.Tables[0].Rows[0]["MO_TA"].ToString();
                dLaiSuat = Convert.ToDecimal(dsLaiSuat.Tables[0].Rows[0]["LAI_SUAT"]);
                if (dsLaiSuat.Tables[0].Rows[0]["PPHAP_TINH_LSUAT"].Equals("DTH"))
                    txtLaiSuat.Value = Convert.ToDouble(dLaiSuat) + txtBienDo.Value.GetValueOrDefault(0);
                else
                    txtLaiSuat.Value = null;
                TinhToanBangKeGocLai();
            }
            else
            {
                txtLaiSuat.Value = null;
                lblTenLSuat.Content = "";
                dLaiSuat = 0;
                txtLaiSuat.Value = null;
                titemsGocLaiVay.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Tính toán bảng kê gốc lãi
        /// </summary>
        private void TinhToanBangKeGocLai()
        {
            try
            {
                DataSet objVV = new TinDungProcess().getVongVonVayByID(idVongVay.ToString());
            if (!LObject.IsNullOrEmpty(objVV) && objVV.Tables.Count > 0)
            {
                cmbThoiHanVay.SelectedIndex = lstThoiHanVay.IndexOf(lstThoiHanVay.FirstOrDefault(i => i.KeywordStrings.FirstOrDefault().Equals(objVV.Tables[0].Rows[0]["KY_HAN_DVI_TINH"].ToString())));
                decimal ThoiGianVay = 0;
                int iCount = 0;
                bool bResult = true;
                foreach (DataRow dr in objVV.Tables[0].Rows)
                {
                    decimal dTGianVay = Convert.ToDecimal(dr["KY_HAN"]);
                    if (iCount > 0)
                    {
                        if (ThoiGianVay != dTGianVay)
                            bResult = false;
                    }
                    else
                        ThoiGianVay = dTGianVay;
                    iCount++;
                    if (!bResult)
                        break;
                }
                if (bResult)
                    telThoiGianVay.Value = Convert.ToDouble(ThoiGianVay);
                else
                    telThoiGianVay.Value = 0;
            }
            if (ClientInformation.Company.Equals(ApplicationConstant.DonViSuDung.BINHKHANH.layGiaTri()))
            {
                TinhSoTienGopHangKy();
            }
            else
            {
                if (cmbCSTinhLai.SelectedIndex < 0 || cmbNhomVongVay.SelectedIndex < 0)
                    return;
                TinDungProcess tindungProcess = new TinDungProcess();
                SAN_PHAM_TIN_DUNG obj = new SAN_PHAM_TIN_DUNG();
                obj.SAN_PHAM_TD = new TD_SAN_PHAM();
                obj.SAN_PHAM_TD.MA_CSO_TLAI = lstCoSoTinhLai.ElementAt(cmbCSTinhLai.SelectedIndex).KeywordStrings.First();
                obj.SAN_PHAM_TD.ID_CS_TLAI = Convert.ToInt32(lstCoSoTinhLai.ElementAt(cmbCSTinhLai.SelectedIndex).KeywordStrings.ElementAt(1));
                obj.SAN_PHAM_TD.ID_LSUAT = idLaiSuat;
                obj.SAN_PHAM_TD.MA_LSUAT = txtMaLaiSuat.Text;
                obj.SAN_PHAM_TD.MA_VONG_VAY = lstNhomVongVay.ElementAt(cmbNhomVongVay.SelectedIndex).KeywordStrings.First();
                int iret = new TinDungProcess().TinhToanBangKeGoclai(ref obj);
                if (!LObject.IsNullOrEmpty(obj.DSACH_GOC_LAI) && obj.DSACH_GOC_LAI.Count() > 0)
                {
                    titemsGocLaiVay.Visibility = System.Windows.Visibility.Visible;
                    raddgrGocLaiVayDS.ItemsSource = null;
                    raddgrGocLaiVayDS.ItemsSource = obj.DSACH_GOC_LAI;
                }
                else
                    titemsGocLaiVay.Visibility = System.Windows.Visibility.Collapsed;
            }
            }
            catch
            {

            }
        }

        private void LuuDuLieu(BusinessConstant.TrangThaiBanGhi banghi, BusinessConstant.TrangThaiNghiepVu nghiepvu, DatabaseConstant.Action action)
        {
            if (action != DatabaseConstant.Action.LUU_TAM)
            {
                if (!VadidateData())
                    return;
            }
            int iResult = 0;
            TinDungProcess tindungProcess = new TinDungProcess();
            try
            {
                // Yêu cầu lock dữ liệu
                List<int> lstId = new List<int>();
                lstId.Add(idSanPham);
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_SAN_PHAM_TDVM,
                DatabaseConstant.Table.TD_SAN_PHAM,
                DatabaseConstant.Action.SUA,
                lstId);
                TD_SAN_PHAM objSanPham = new TD_SAN_PHAM();
                List<KT_PHAN_HE_PLOAI> lstPhanHe = new List<KT_PHAN_HE_PLOAI>();
                LayDuLieu(ref objSanPham,ref lstPhanHe, banghi, nghiepvu);
                if (idSanPham == 0)
                    iResult = tindungProcess.LuuSanPhamTinDung(objSanPham, lstPhanHe);
                else
                    iResult = tindungProcess.SuaSanPhamTinDung(objSanPham, lstPhanHe);
                if (iResult > 0)
                {
                    LMessage.ShowMessage("M.DungChung.LuuDuLieuThanhCong", LMessage.MessageBoxType.Information);
                    // Yêu cầu Unlock dữ liệu
                    retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                    DatabaseConstant.Function.TDVM_SAN_PHAM_TDVM,
                    DatabaseConstant.Table.TD_SAN_PHAM,
                    DatabaseConstant.Action.SUA,
                    lstId);
                    idSanPham = iResult;
                    LoadDuLieuCT(false);
                    if (OnSavingComleted != null)
                        OnSavingComleted(null, EventArgs.Empty);
                    if (cbMultiAdd.IsChecked == true)
                    {
                        ClearForm();
                        idSanPham = 0;
                    }
                    objSanPham = null;
                }
                else
                    LMessage.ShowMessage("M.DungChung.LoiLuuDuLieu", LMessage.MessageBoxType.Error);
            }
            catch (Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiLuuDuLieu", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        /// <summary>
        /// Xóa dữ liệu
        /// </summary>
        private void Xoa()
        {
            if (tlbDelete.IsEnabled == false)
                return;
            if (idSanPham > 0)
            {
                try
                {
                    if (LMessage.ShowMessage("M.DungChung.HoiXoa", LMessage.MessageBoxType.Question) == MessageBoxResult.Yes)
                    {
                        TinDungProcess tindungProcess = new TinDungProcess();
                        List<ClientResponseDetail> ClientResponseDetail = new List<Presentation.Process.Common.ClientResponseDetail>();
                        // Yêu cầu lock dữ liệu
                        List<int> lstId = new List<int>();
                        lstId.Add(idSanPham);
                        UtilitiesProcess process = new UtilitiesProcess();
                        bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                        DatabaseConstant.Function.TDVM_SAN_PHAM_TDVM,
                        DatabaseConstant.Table.TD_SAN_PHAM,
                        DatabaseConstant.Action.XOA,
                        lstId);
                        bool bResult = tindungProcess.XoaSanPhamTinDung(lstId, ref ClientResponseDetail);
                        if (bResult)
                        {
                            CommonFunction.ThongBaoKetQua(ClientResponseDetail);
                            // Yeu cau Unlook du lieu
                            retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                        DatabaseConstant.Function.TDVM_SAN_PHAM_TDVM,
                        DatabaseConstant.Table.TD_SAN_PHAM,
                        DatabaseConstant.Action.XOA,
                        lstId);
                            if (OnSavingComleted != null)
                                OnSavingComleted(null, EventArgs.Empty);
                            CommonFunction.CloseUserControl(this);
                        }
                        else
                            LMessage.ShowMessage("M.DungChung.XoaKhongThanhCong", LMessage.MessageBoxType.Error);
                    }
                }
                catch (Exception ex)
                {
                    LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                    LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                }
            }

        }

        /// <summary>
        /// Duyệt chi tiết
        /// </summary>
        private void Duyet()
        {
            if (tlbApprove.IsEnabled == false)
                return;
            try
            {
                if (idSanPham > 0 & sTrangThai.Equals(BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri()))
                {
                    List<ClientResponseDetail> ClientResponseDetail = new List<ClientResponseDetail>();
                    if (LMessage.ShowMessage("M.DungChung.HoiDuyet", LMessage.MessageBoxType.Question) == MessageBoxResult.Yes)
                    {
                        List<int> lstID = new List<int>();
                        lstID.Add(idSanPham);
                        UtilitiesProcess process = new UtilitiesProcess();
                        bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                        DatabaseConstant.Function.TDVM_SAN_PHAM_TDVM,
                        DatabaseConstant.Table.TD_SAN_PHAM,
                        DatabaseConstant.Action.DUYET,
                        lstID);
                        bool bResult = new TinDungProcess().DuyetSanPhamTinDung(lstID, ref ClientResponseDetail);
                        if (bResult)
                        {

                            CommonFunction.ThongBaoKetQua(ClientResponseDetail);
                            // Yeu cau Unlook du lieu
                            retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                        DatabaseConstant.Function.TDVM_SAN_PHAM_TDVM,
                        DatabaseConstant.Table.TD_SAN_PHAM,
                        DatabaseConstant.Action.DUYET,
                        lstID);
                            if (OnSavingComleted != null)
                                OnSavingComleted(null, EventArgs.Empty);
                            LoadDuLieuCT(false);
                        }
                        else
                        {
                            LMessage.ShowMessage("M.DungChung.DuyetKhongThanhCong", LMessage.MessageBoxType.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        /// <summary>
        /// Thoái duyệt
        /// </summary>
        private void ThoaiDuyet()
        {
            if (tlbCancel.IsEnabled == false)
                return;
            try
            {
                if (idSanPham > 0 & sTrangThai.Equals(BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri()))
                {
                    if (LMessage.ShowMessage("M.DungChung.HoiThoaiDuyet", LMessage.MessageBoxType.Question) == MessageBoxResult.Yes)
                    {
                        List<ClientResponseDetail> ClientResponseDetail = new List<ClientResponseDetail>();
                        List<int> lstID = new List<int>();
                        lstID.Add(idSanPham);
                        UtilitiesProcess process = new UtilitiesProcess();
                        bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                        DatabaseConstant.Function.TDVM_SAN_PHAM_TDVM,
                        DatabaseConstant.Table.TD_SAN_PHAM,
                        DatabaseConstant.Action.THOAI_DUYET,
                        lstID);
                        bool bResult = new TinDungProcess().HuyDuyetSanPhamTinDung(lstID,ref ClientResponseDetail);
                        if (bResult)
                        {
                            CommonFunction.ThongBaoKetQua(ClientResponseDetail);
                            // Yeu cau Unlook du lieu
                            retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                        DatabaseConstant.Function.TDVM_SAN_PHAM_TDVM,
                        DatabaseConstant.Table.TD_SAN_PHAM,
                        DatabaseConstant.Action.THOAI_DUYET,
                        lstID);
                            if (OnSavingComleted != null)
                                OnSavingComleted(null, EventArgs.Empty);
                            LoadDuLieuCT(false);
                        }
                        else
                        {
                            LMessage.ShowMessage("M.DungChung.ThoaiDuyetKhongThanhCong", LMessage.MessageBoxType.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        /// <summary>
        /// Từ chối duyệt
        /// </summary>
        private void TuChoiDuyet()
        {
            if (tlbRefuse.IsEnabled == false)
                return;
            try
            {
                if (idSanPham > 0 & sTrangThai.Equals(BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri()))
                {
                    if (LMessage.ShowMessage("M.DungChung.HoiTuChoiDuyet", LMessage.MessageBoxType.Question) == MessageBoxResult.Yes)
                    {
                        List<ClientResponseDetail> ClientResponseDetail = new List<ClientResponseDetail>();
                        List<int> lstID = new List<int>();
                        lstID.Add(idSanPham);
                        UtilitiesProcess process = new UtilitiesProcess();
                        bool retLockData = process.LockData(DatabaseConstant.Module.TDVM,
                        DatabaseConstant.Function.TDVM_SAN_PHAM_TDVM,
                        DatabaseConstant.Table.TD_SAN_PHAM,
                        DatabaseConstant.Action.TU_CHOI_DUYET,
                        lstID);
                        bool bResult = new TinDungProcess().TuChoiSanPhamTinDung(lstID,ref ClientResponseDetail);
                        if (bResult)
                        {
                            CommonFunction.ThongBaoKetQua(ClientResponseDetail);
                            // Yeu cau Unlook du lieu
                            retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                        DatabaseConstant.Function.TDVM_SAN_PHAM_TDVM,
                        DatabaseConstant.Table.TD_SAN_PHAM,
                        DatabaseConstant.Action.TU_CHOI_DUYET,
                        lstID);
                            if (OnSavingComleted != null)
                                OnSavingComleted(null, EventArgs.Empty);
                            LoadDuLieuCT(false);
                        }
                        else
                        {
                            LMessage.ShowMessage("M.DungChung.TuChoiDuyetKhongThanhCong", LMessage.MessageBoxType.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        /// <summary>
        /// Sửa
        /// </summary>
        private void Sua()
        {
            if(sTrangThai.Equals(BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri()))
            {
                grdTKhoan.IsReadOnly = false;
                grdTKhoan.IsReadOnly = false;
                grbThongTinChung.IsEnabled = true;
                foreach (Control ct in grbThongTinChung.ChildrenOfType<Control>())
                {
                    if (ct.Name == "txtTenSanPham")
                        ct.IsEnabled = true;
                    else
                        ct.IsEnabled = false;
                }
            }
            else
            {
                grbGocLaiVay.IsEnabled = true;
                grbThongTinChung.IsEnabled = true;
                grbLaiSuat.IsEnabled = true;
                grdTKhoan.IsReadOnly = false;
                grbKyQuy.IsEnabled = true;
                grbHinhThuc.IsEnabled = true;
            }
            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.SUA, sTrangThai,mnuMain,DatabaseConstant.Function.TDVM_SAN_PHAM_TDVM);
        }
        #endregion        

        private void cmbLoaiSuat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cmbTanSuatDanhGia_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

    }
}
