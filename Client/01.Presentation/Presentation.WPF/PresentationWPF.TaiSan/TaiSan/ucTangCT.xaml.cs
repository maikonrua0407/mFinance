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
using PresentationWPF.CustomControl;
using Telerik.Windows.Controls;
using System.Data;
using Utilities.Common;
using Presentation.Process.Common;
using System.Collections;
using System.Reflection;
using Microsoft.Windows.Controls.Ribbon;
using Presentation.Process;
using Presentation.Process.PopupServiceRef;
using Presentation.Process.TaiSanServiceRef;
using Telerik.Windows.Controls.GridView;
using PresentationWPF.BaoCao.DungChung;
using Presentation.Process.BaoCaoServiceRef;

namespace PresentationWPF.TaiSan.TaiSan
{
    /// <summary>
    /// Interaction logic for ucTangCT.xaml
    /// </summary>
    public partial class ucTangCT : UserControl
    {
        /// <summary>
        /// Khai bao
        /// </summary>
        #region Khai bao
        public static RoutedCommand AddCommand = new RoutedCommand();
        public static RoutedCommand ModifyCommand = new RoutedCommand();
        public static RoutedCommand DeleteCommand = new RoutedCommand();
        public static RoutedCommand CloneCommand = new RoutedCommand();
        public static RoutedCommand HoldCommand = new RoutedCommand();
        public static RoutedCommand SaveCommand = new RoutedCommand();
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
        public event EventHandler OnSavingCompleted;
        public void LayDuLieuTuPopup(List<DataRow> lst)
        {
            lstPopup = lst;
        }
        //string TThaiNVu = "";
        public int idTangTS = 0;

        private string tthaiNvu = "";
        public string TThaiNVu
        {
            get { return tthaiNvu; }
            set { tthaiNvu = value; }
        }
        //public string TthaiNvu
        //{
        //    get { return tthaiNvu; }
        //    set { tthaiNvu = value; }
        //}

        public DatabaseConstant.Action Action;
        public DatabaseConstant.Function Function;
        TAI_SAN_DTO objTSDto = new TAI_SAN_DTO();
        List<TS_TANG_TTINH> lstThuocTinh = new List<TS_TANG_TTINH>();
        List<TS_TANG_PHU_KIEN> lstPhuKien = new List<TS_TANG_PHU_KIEN>();
        List<TS_TANG_NG> lstNguyenGia = new List<TS_TANG_NG>();
        //List<TS_TANG_NG> lstNguyenGia_DDU = new List<TS_TANG_NG>();
        #endregion

        /// <summary>
        /// Khoi tao
        /// </summary>
        #region Khoi tao
        public ucTangCT()
        {
            InitializeComponent();
            HeThong htHeThong = new HeThong();
            htHeThong.DuyetQuyenTinhNangToolbar("/PresentationWPF.TaiSan;component/TaiSan/ucTangCT.xaml", ref Toolbar, ref mnuMain);
            foreach (var item in mnuMain.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += new RoutedEventHandler(btnShortcutKey_Click);
            }
            BindHotkey();
            KhoiTaoGiaTriChoComboBox();
            InitEventHandler();
            ShowControl();
            ClearForm();
            grdPhuKien.ItemsSource = lstPhuKien;
            //grdNguyenGia_DDU.ItemsSource = lstNguyenGia_DDU;
            grdNguyenGia.ItemsSource = lstNguyenGia;
            teldtNgayLap.Value = ClientInformation.NgayLamViecHienTai.StringToDate("yyyyMMdd");
            if (idTangTS > 0)
            {
                //grdNguyenGia_DDU.Visibility = Visibility.Visible;

                objTSDto.TS_TAI_SAN.ID = idTangTS;
                // Refresh buttons
                CommonFunction.RefreshButton(Toolbar, Action, tthaiNvu, mnuMain, DatabaseConstant.Function.TS_TANG);
            }
            else
            {
                //grdNguyenGia_DDU.Visibility = Visibility.Collapsed;

                //spDuyetTuChoiHuy.Visibility = Visibility.Collapsed;
            }
            titemThongTinChung.Focus();
            // Lần đầu không cho chọn nguyên nhân thay đổi
        }

        public ucTangCT(string maThamChieu, DatabaseConstant.Action action)
        {
            if (!maThamChieu.IsNullOrEmptyOrSpace())
                idTangTS = maThamChieu.Split('.')[1].StringToInt32();
            InitializeComponent();
            HeThong htHeThong = new HeThong();
            htHeThong.DuyetQuyenTinhNangToolbar("/PresentationWPF.TaiSan;component/TaiSan/ucTangCT.xaml", ref Toolbar, ref mnuMain);
            foreach (var item in mnuMain.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += new RoutedEventHandler(btnShortcutKey_Click);
            }
            BindHotkey();
            KhoiTaoGiaTriChoComboBox();
            InitEventHandler();
            ShowControl();
            ClearForm();
            grdPhuKien.ItemsSource = lstPhuKien;
            //grdNguyenGia_DDU.ItemsSource = lstNguyenGia_DDU;
            grdNguyenGia.ItemsSource = lstNguyenGia;
            teldtNgayLap.Value = ClientInformation.NgayLamViecHienTai.StringToDate("yyyyMMdd");
            if (idTangTS > 0)
            {
                Action = action;
                //grdNguyenGia_DDU.Visibility = Visibility.Visible;
                if (objTSDto.TS_TAI_SAN.IsNullOrEmpty()) objTSDto.TS_TAI_SAN = new TS_TANG();
                objTSDto.TS_TAI_SAN.ID = idTangTS;
                // Refresh buttons
                CommonFunction.RefreshButton(Toolbar, Action, tthaiNvu, mnuMain, DatabaseConstant.Function.TS_TANG);
            }
            else
            {
                //grdNguyenGia_DDU.Visibility = Visibility.Collapsed;

                //spDuyetTuChoiHuy.Visibility = Visibility.Collapsed;
            }
            titemThongTinChung.Focus();
            // Lần đầu không cho chọn nguyên nhân thay đổi
        }

        void KhoiTaoGiaTriChoComboBox()
        {
            try
            {
                List<DatabaseConstant.LOAI_DMUC_TSAN> lstDanhMucLoai = new List<DatabaseConstant.LOAI_DMUC_TSAN>();
                lstDanhMucLoai.Add(DatabaseConstant.LOAI_DMUC_TSAN.HINH_THUC_NHAP);
                lstDanhMucLoai.Add(DatabaseConstant.LOAI_DMUC_TSAN.DON_VI_TINH);
                lstDanhMucLoai.Add(DatabaseConstant.LOAI_DMUC_TSAN.NGUON_GOC);
                lstDanhMucLoai.Add(DatabaseConstant.LOAI_DMUC_TSAN.TINH_TRANG);
                lstDanhMucLoai.Add(DatabaseConstant.LOAI_DMUC_TSAN.PP_KHAU_HAO);
                lstDanhMucLoai.Add(DatabaseConstant.LOAI_DMUC_TSAN.THONG_TIN_KHAC);
                List<DMUC_TSAN_DTO> lstDanhMucDto = new List<DMUC_TSAN_DTO>();
                TaiSanProcess process = new TaiSanProcess();
                if (process.LayDanhMucTaiSanTheoLoai(ref lstDanhMucDto, lstDanhMucLoai))
                {
                    foreach (DMUC_TSAN_DTO item in lstDanhMucDto)
                    {
                        if (item.MaLoai == DatabaseConstant.LOAI_DMUC_TSAN.HINH_THUC_NHAP)
                        {
                            foreach (TS_DM_DMUC_GTRI itemDanhMuc in item.LstObj)
                            {
                                cmbHinhThucNhap.Items.Add(new AutoCompleteEntry(itemDanhMuc.TEN_DMUC, itemDanhMuc.MA_DMUC, itemDanhMuc.MA_DMUC_LOAI));
                            }
                            foreach (var subitem in cmbHinhThucNhap.Items)
                            {
                                if (((AutoCompleteEntry)subitem).KeywordStrings.First().Equals(BusinessConstant.HINH_THUC_NHAP_TS.HOAN_THANH.layGiaTri()))
                                {
                                    cmbHinhThucNhap.SelectedItem = subitem;
                                    break;
                                }
                            }
                            continue;
                        }
                        else if (item.MaLoai == DatabaseConstant.LOAI_DMUC_TSAN.DON_VI_TINH)
                        {
                            foreach (TS_DM_DMUC_GTRI itemDanhMuc in item.LstObj)
                            {
                                cmbDonViTinh.Items.Add(new AutoCompleteEntry(itemDanhMuc.TEN_DMUC, itemDanhMuc.MA_DMUC, itemDanhMuc.MA_DMUC_LOAI));
                            }
                            cmbDonViTinh.SelectedIndex = 0; continue;
                        }
                        else if (item.MaLoai == DatabaseConstant.LOAI_DMUC_TSAN.NGUON_GOC)
                        {
                            foreach (TS_DM_DMUC_GTRI itemDanhMuc in item.LstObj)
                            {
                                cmbNguonGoc.Items.Add(new AutoCompleteEntry(itemDanhMuc.TEN_DMUC, itemDanhMuc.MA_DMUC, itemDanhMuc.MA_DMUC_LOAI));
                            }
                            cmbNguonGoc.SelectedIndex = 0; continue;
                        }
                        else if (item.MaLoai == DatabaseConstant.LOAI_DMUC_TSAN.TINH_TRANG)
                        {
                            foreach (TS_DM_DMUC_GTRI itemDanhMuc in item.LstObj)
                            {
                                cmbTinhTrang.Items.Add(new AutoCompleteEntry(itemDanhMuc.TEN_DMUC, itemDanhMuc.MA_DMUC, itemDanhMuc.MA_DMUC_LOAI));
                            }
                            cmbTinhTrang.SelectedIndex = 0; continue;
                        }
                        else if (item.MaLoai == DatabaseConstant.LOAI_DMUC_TSAN.PP_KHAU_HAO)
                        {
                            foreach (TS_DM_DMUC_GTRI itemDanhMuc in item.LstObj)
                            {
                                cmbPhuongPhapKH.Items.Add(new AutoCompleteEntry(itemDanhMuc.TEN_DMUC, itemDanhMuc.MA_DMUC, itemDanhMuc.MA_DMUC_LOAI));
                            }
                            foreach (var subitem in cmbPhuongPhapKH.Items)
                            {
                                if (((AutoCompleteEntry)subitem).KeywordStrings.First().Equals(BusinessConstant.PP_KHAU_HAO.DUONG_THANG.layGiaTri()))
                                {
                                    cmbPhuongPhapKH.SelectedItem = subitem;
                                    break;
                                }
                            }
                            continue;
                        }
                        else if (item.MaLoai == DatabaseConstant.LOAI_DMUC_TSAN.THONG_TIN_KHAC)
                        {
                            lstThuocTinh = new List<TS_TANG_TTINH>();
                            foreach (TS_DM_DMUC_GTRI itemDanhMuc in item.LstObj)
                            {
                                TS_TANG_TTINH ttinh = new TS_TANG_TTINH();
                                ttinh.TEN_TTINH = itemDanhMuc.TEN_DMUC;
                                lstThuocTinh.Add(ttinh);
                            }
                            grdThuocTinh.ItemsSource = lstThuocTinh; continue;
                        }
                    }
                    List<TS_DM_NHOM_TSCD> lstNhomTS = new List<TS_DM_NHOM_TSCD>();
                    if (process.LayNhomTaiSanNhoNhat(ref lstNhomTS))
                    {
                        foreach (TS_DM_NHOM_TSCD itemDanhMuc in lstNhomTS)
                        {
                            cmbNhomTS.Items.Add(new AutoCompleteEntry(itemDanhMuc.TEN_NHOM, itemDanhMuc.MA_NHOM, itemDanhMuc.KHAU_HAO_TU.ToString() + "-" + itemDanhMuc.KHAU_HAO_DEN.ToString()));
                        }
                        cmbNhomTS.SelectedIndex = 0;
                    }
                    else
                        LMessage.ShowMessage(LLanguage.SearchResourceByKey(ApplicationConstant.NghiepVuResponseMessage.M_ResponseMessage_DungChung_LoiKhongXacDinh.layGiaTri()), LMessage.MessageBoxType.Error);
                }
                else
                    LMessage.ShowMessage(LLanguage.SearchResourceByKey(ApplicationConstant.NghiepVuResponseMessage.M_ResponseMessage_DungChung_LoiKhongXacDinh.layGiaTri()), LMessage.MessageBoxType.Error);

            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        void InitEventHandler()
        {
            chkTinhKhauHao.Checked += chkTinhKhauHao_Checked;
        }

        private void ShowControl()
        {
            /*
            HeThong hethong = new HeThong();
            ArrayList arr = new ArrayList();
            arr = hethong.SetVisibleControl("PresentationWPF.TaiSan.TaiSan.ucTaiSanCT", "RibbonButton");
            foreach (List<string> lst in arr)
            {
                object item = Toolbar.FindName(lst.First());
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
            */
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
                    if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THEM)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.N, ModifierKeys.Control);
                        key = new KeyBinding(AddCommand, keyg);
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
                        KeyGesture keyg = new KeyGesture(Key.D, ModifierKeys.Control);
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
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRINH_DUYET)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.S, ModifierKeys.Control);
                        key = new KeyBinding(SaveCommand, keyg);
                        key.Gesture = keyg;
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.BANG_KE)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.S, ModifierKeys.Control | ModifierKeys.Shift);
                        key = new KeyBinding(CashStmtCommand, keyg);
                        key.Gesture = keyg;
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DUYET)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.S, ModifierKeys.Control | ModifierKeys.Shift);
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

        private void AddCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void AddCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ResetData();
            SetEnabledAllControl(true);
        }

        private void ModifyCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void ModifyCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Modify();
        }

        private void DeleteCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void DeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            BeforeDelete();
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
            BeforeSave(BusinessConstant.TrangThaiNghiepVu.LUU_TAM, BusinessConstant.TrangThaiBanGhi.SU_DUNG);
        }

        private void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            BeforeSave(BusinessConstant.TrangThaiNghiepVu.CHO_DUYET, BusinessConstant.TrangThaiBanGhi.SU_DUNG);
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
            BeforeApprove();
        }

        private void CancelCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void CancelCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            BeforeCancel();
        }

        private void RefuseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void RefuseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            BeforeRefuse();
        }

        private void PreviewCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void PreviewCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OnPreviewChungTu();
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
                ResetData();
                SetEnabledAllControl(true);
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
                Modify();
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
                OnPreviewChungTu();
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
            // Truongnx
            string strTinhNang = ((MenuItem)sender).Name.Substring(3, ((MenuItem)sender).Name.Length - 3);
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.NHAP_DU_LIEU)))
            { }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THEM)))
            {
                ResetData();
                SetEnabledAllControl(true);
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
                Modify();
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
                OnPreviewChungTu();
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
                {
                    telControl.Value = dtpControl.SelectedDate;
                    telControl.Focus();
                }
                else
                    throw new System.NullReferenceException(LLanguage.SearchResourceByKey("M.TinDung.KheUoc.ucKheUocCT.KhongTimThayControl") + sbControl.ToString());
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        /// <summary>
        /// Sự kiện load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            teldtNgayLap.Value = ClientInformation.NgayLamViecHienTai.StringToDate("yyyyMMdd");
            Function = DatabaseConstant.Function.TS_TANG;

            //Hiển thị Form khi xem dữ liệu
            if (Action == DatabaseConstant.Action.XEM || Action == DatabaseConstant.Action.SUA)
            {
                if (idTangTS > 0)
                {
                    //grdNguyenGia_DDU.Visibility = Visibility.Visible;

                    //spDuyetTuChoiHuy.Visibility = Visibility.Visible;
                }
                else
                {
                    //grdNguyenGia_DDU.Visibility = Visibility.Collapsed;

                    //spDuyetTuChoiHuy.Visibility = Visibility.Collapsed;
                }

                if (objTSDto.TS_TAI_SAN.IsNullOrEmpty())
                    objTSDto.TS_TAI_SAN = new TS_TANG();
                objTSDto.TS_TAI_SAN.ID = idTangTS;
                SetDataForm();
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

        /// <summary>
        /// Hàm xử lý giải phóng lock dữ liệu cho form
        /// </summary>
        private void Release()
        {
            // Yêu cầu unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(idTangTS);

            bool ret = process.UnlockData(DatabaseConstant.Module.QLTS,
                Function,
                DatabaseConstant.Table.TS_TANG,
                DatabaseConstant.Action.SUA,
                listLockId);
        }

        void ClearForm()
        {
            txtTenTS.Text = "";
            foreach (var subitem in cmbHinhThucNhap.Items)
            {
                if (((AutoCompleteEntry)subitem).KeywordStrings.First().Equals(BusinessConstant.HINH_THUC_NHAP_TS.HOAN_THANH.layGiaTri()))
                {
                    cmbHinhThucNhap.SelectedItem = subitem;
                    break;
                }
            }
            cmbDonViTinh.SelectedIndex = 0;
            cmbTinhTrang.SelectedIndex = 0;
            teldtNgayLap.Value = ClientInformation.NgayLamViecHienTai.StringToDate("yyyyMMdd");
            cmbNhomTS.SelectedIndex = 0;
            cmbNguonGoc.SelectedIndex = 0;
            chkTinhKhauHao.IsChecked = true;
            numThoiGianKH.Value = 0;
            numThoiGianKH.Value = 0;
            List<TS_TANG_NG> lstNguyenGia = new List<TS_TANG_NG>();
            grdNguyenGia.ItemsSource = lstNguyenGia;
            grdNguyenGia.Rebind();
            List<TS_TANG_PHU_KIEN> lstPhuKien = new List<TS_TANG_PHU_KIEN>();
            grdPhuKien.ItemsSource = lstPhuKien;
            grdPhuKien.Rebind();
            grdThuocTinh.ItemsSource = lstThuocTinh;
            grdThuocTinh.Rebind();
            Action = DatabaseConstant.Action.THEM;
            CommonFunction.RefreshButton(Toolbar, Action, TThaiNVu, mnuMain, DatabaseConstant.Function.TS_TANG);
            RefreshButton();
        }

        private void ResetData()
        {
            Action = DatabaseConstant.Action.THEM;
            idTangTS = 0;

            ClearForm();
        }

        private void SetEnabledAllControl(bool bBool)
        {
            txtTenTS.IsEnabled = bBool;
            cmbHinhThucNhap.IsEnabled = bBool;
            cmbDonViTinh.IsEnabled = bBool;
            cmbTinhTrang.IsEnabled = bBool;
            teldtNgayLap.IsEnabled = bBool;
            cmbNhomTS.IsEnabled = bBool;
            cmbNguonGoc.IsEnabled = bBool;
            chkTinhKhauHao.IsEnabled = bBool;
            cmbPhuongPhapKH.IsEnabled = false;
            numThoiGianKH.IsEnabled = bBool;
            numThoiGianKH.IsEnabled = bBool;

            btnAddNguyeGia.IsEnabled = bBool;
            btnCommitNguyeGia.IsEnabled = bBool;
            btnCancelNguyeGia.IsEnabled = bBool;
            btnDeleteNguyeGia.IsEnabled = bBool;
            tlbAddNguyeGia.IsEnabled = bBool;
            tlbDeleteNguyeGia.IsEnabled = bBool;
            tlbSubmitNguyeGia.IsEnabled = bBool;
            tlbApproveNguyeGia.IsEnabled = bBool;
            tlbRefuseNguyeGia.IsEnabled = bBool;
            btnAddPhuKien.IsEnabled = bBool;
            btnCommitPhuKien.IsEnabled = bBool;
            btnCancelPhuKien.IsEnabled = bBool;
            btnDeletePhuKien.IsEnabled = bBool;
        }

        private void RefreshButton()
        {
            if (TThaiNVu.Equals(BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri()))
            {
                tlbSubmitNguyeGia.IsEnabled = false;
                tlbApproveNguyeGia.IsEnabled = false;
                tlbRefuseNguyeGia.IsEnabled = false;
            }
            else if (TThaiNVu.Equals(BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri()))
            {
                tlbSubmit.IsEnabled = false;
                tlbApprove.IsEnabled = false;
                tlbRefuse.IsEnabled = false;
            }

            if (Action.Equals(DatabaseConstant.Action.THEM))
            {
                //btnDeleteNguyeGia.IsEnabled = false;

                tlbSubmitNguyeGia.IsEnabled = false;
                tlbApproveNguyeGia.IsEnabled = false;
                tlbRefuseNguyeGia.IsEnabled = false;
            }
            else { }
        }

        private void grdNguyenGia_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            if (TThaiNVu.Equals(BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri()))
            {
                TS_TANG_NG row = (TS_TANG_NG)grdNguyenGia.SelectedItem;
                if (idTangTS > 0)
                {
                    if (row.IsNullOrEmpty() || row.TTHAI_NVU.IsNullOrEmptyOrSpace())
                    {
                        tlbApproveNguyeGia.IsEnabled = false;
                        tlbRefuseNguyeGia.IsEnabled = false;
                        tlbSubmitNguyeGia.IsEnabled = false;
                        tlbDeleteNguyeGia.IsEnabled = true;
                    }
                    else
                    {
                        string tthai = row.TTHAI_NVU.ToString();
                        if (tthai.Equals("DDU"))
                        {
                            tlbApproveNguyeGia.IsEnabled = false;
                            tlbRefuseNguyeGia.IsEnabled = true;
                            tlbSubmitNguyeGia.IsEnabled = false;
                            tlbDeleteNguyeGia.IsEnabled = false;
                        }
                        else
                        {
                            tlbApproveNguyeGia.IsEnabled = true;
                            tlbRefuseNguyeGia.IsEnabled = true;
                            tlbSubmitNguyeGia.IsEnabled = true;
                            tlbDeleteNguyeGia.IsEnabled = true;
                        }
                        //tlbApproveNguyeGia.IsEnabled = false;
                        //tlbRefuseNguyeGia.IsEnabled = false;

                        //btnDeleteNguyeGia.IsEnabled = false;
                    }
                }
                else
                {
                    tlbSubmitNguyeGia.IsEnabled = false;
                    tlbApproveNguyeGia.IsEnabled = false;
                    tlbRefuseNguyeGia.IsEnabled = false;
                }
            }
            else
            {
                tlbSubmitNguyeGia.IsEnabled = false;
                tlbApproveNguyeGia.IsEnabled = false;
                tlbRefuseNguyeGia.IsEnabled = false;
            }
        }

        private void ucCauThanhNG_EditCellEnd(object sender, EventArgs e)
        {
            TS_TANG_NG obj = ucCauThanhNG.cellEdit.ParentRow.Item as TS_TANG_NG;
            obj.CAU_THANH = ucCauThanhNG.GiaTri;
            lstNguyenGia.ElementAt(lstNguyenGia.IndexOf(obj)).CAU_THANH = ucCauThanhNG.GiaTri;
        }

        private void ucHinhThucTT_EditCellEnd(object sender, EventArgs e)
        {
            TS_TANG_NG obj = ucHinhThucTT.cellEdit.ParentRow.Item as TS_TANG_NG;
            obj.HTHUC_TTOAN = ucHinhThucTT.GiaTri;
            lstNguyenGia.ElementAt(lstNguyenGia.IndexOf(obj)).HTHUC_TTOAN = ucHinhThucTT.GiaTri;
            if (ucHinhThucTT.GiaTri.Equals(BusinessConstant.HINH_THUC_GIAO_DICH.TIEN_MAT.layGiaTri()))
            {
                TextBox txt = (TextBox)ucHinhThucTT.cellEdit.ParentRow.FindName("txtTKTT");
                Button btn = (Button)ucHinhThucTT.cellEdit.ParentRow.FindName("btnTKTT");
                if (!txt.IsNullOrEmpty())
                    txt.IsEnabled = false;
                if (!btn.IsNullOrEmpty())
                    btn.IsEnabled = false;
            }
        }

        private void ucDonViTinh_EditCellEnd(object sender, EventArgs e)
        {
            TS_TANG_PHU_KIEN obj = ucDonViTinh.cellEdit.ParentRow.Item as TS_TANG_PHU_KIEN;
            obj.DON_VI_TINH = ucDonViTinh.GiaTri;
            lstPhuKien.ElementAt(lstPhuKien.IndexOf(obj)).DON_VI_TINH = ucDonViTinh.GiaTri;
        }

        private void chkTinhKhauHao_Checked(object sender, RoutedEventArgs e)
        {
            cmbPhuongPhapKH.IsEnabled = chkTinhKhauHao.IsChecked.Value;
            numThoiGianKH.IsEnabled = chkTinhKhauHao.IsChecked.Value;
        }

        private void TaiKhoanNguyenGia_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            GridViewRow grrow = btn.ParentOfType<GridViewRow>();
            TaiKhoanNguyenGia(grrow);
        }

        private void TaiKhoanNguyenGia_LostFocus(object sender, RoutedEventArgs e)
        {
            var txt = sender as TextBox;

        }

        private void TaiKhoanNguyenGia_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F3)
            {
                var txt = sender as TextBox;
                GridViewRow grrow = txt.ParentOfType<GridViewRow>();
                TaiKhoanNguyenGia(grrow);
            }
        }

        private void TaiKhoanNguyenGia(GridViewRow grrow)
        {
            try
            {
                TS_TANG_NG obj = grrow.Item as TS_TANG_NG;
                List<string> lstDieuKien = new List<string>();
                lstDieuKien.Add(ClientInformation.MaDonViGiaoDich);
                var process = new PopupProcess();
                lstPopup.Clear();
                process.getPopupInformation("POPUP_TAI_KHOAN_NOI_BO", lstDieuKien);

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
                    obj.TKHOAN_TTOAN = row[2].ToString();
                }
            }
            catch (System.Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void cmbHinhThucNhap_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string hinhThucNhap = ((AutoCompleteEntry)cmbHinhThucNhap.SelectedItem).KeywordStrings.First();
            if (TThaiNVu.Equals(BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri()) && hinhThucNhap.Equals(BusinessConstant.HINH_THUC_NHAP_TS.HOAN_THANH.layGiaTri()))
                tlbApproveHoanThanh.IsEnabled = true;
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
                objTSDto.NGAY_GDICH = ClientInformation.NgayLamViecHienTai;
                if (objTSDto.TS_TAI_SAN.IsNullOrEmpty())
                    objTSDto.TS_TAI_SAN = new TS_TANG();
                objTSDto.TS_TAI_SAN.MA_TAI_SAN = txtMaTS.Text;
                objTSDto.TS_TAI_SAN.TEN_TAI_SAN = txtTenTS.Text;
                objTSDto.TS_TAI_SAN.DON_VI_TINH = ((AutoCompleteEntry)cmbDonViTinh.SelectedItem).KeywordStrings.First();
                objTSDto.TS_TAI_SAN.MA_TINH_TRANG = ((AutoCompleteEntry)cmbHinhThucNhap.SelectedItem).KeywordStrings.First();
                objTSDto.TS_TAI_SAN.TINH_TRANG_HIEN_TAI = ((AutoCompleteEntry)cmbTinhTrang.SelectedItem).KeywordStrings.First();
                objTSDto.TS_TAI_SAN.NGAY_QUYET_DINH = ((DateTime)teldtNgayLap.Value).DateToString("yyyyMMdd");
                objTSDto.TS_TAI_SAN.MA_NHOM_TSCD = ((AutoCompleteEntry)cmbNhomTS.SelectedItem).KeywordStrings.First();
                objTSDto.TS_TAI_SAN.NGUON_TAI_SAN = ((AutoCompleteEntry)cmbNguonGoc.SelectedItem).KeywordStrings.First();
                if (chkTinhKhauHao.IsChecked.Value)
                {
                    objTSDto.TS_TAI_SAN.PHUONG_PHAP_TINH_KH = ((AutoCompleteEntry)cmbPhuongPhapKH.SelectedItem).KeywordStrings.First();
                    objTSDto.TS_TAI_SAN.THOI_GIAN_KH = Convert.ToInt32(numThoiGianKH.Value);
                }
                else
                {
                    objTSDto.TS_TAI_SAN.PHUONG_PHAP_TINH_KH = "";
                    objTSDto.TS_TAI_SAN.THOI_GIAN_KH = 0;
                }
                objTSDto.TS_TAI_SAN.TONG_NGUYEN_GIA = 0;
                lstThuocTinh = grdThuocTinh.ItemsSource as List<TS_TANG_TTINH>;
                objTSDto.Lst_TS_TTINH = lstThuocTinh.ToArray();
                lstPhuKien = grdPhuKien.ItemsSource as List<TS_TANG_PHU_KIEN>;
                objTSDto.Lst_TS_PHU_KIEN = lstPhuKien.ToArray();
                if (lstNguyenGia.IsNullOrEmpty() || lstNguyenGia.Count == 0)
                    lstNguyenGia = grdNguyenGia.ItemsSource as List<TS_TANG_NG>;
                foreach (TS_TANG_NG ng in lstNguyenGia)
                {
                    if (!ng.NGAY_CTU.IsDate("yyyyMMdd"))
                        ng.NGAY_CTU = (Convert.ToDateTime(ng.NGAY_CTU)).DateToString("yyyyMMdd");
                }

                objTSDto.Lst_TTS_TANG_NG = lstNguyenGia.ToArray();
                if (idTangTS > 0)
                {
                    objTSDto.TS_TAI_SAN.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                    objTSDto.TS_TAI_SAN.TTHAI_NVU = nghiepvu.layGiaTri();
                    objTSDto.TS_TAI_SAN.NGAY_CNHAT = ClientInformation.NgayLamViecHienTai;
                    objTSDto.TS_TAI_SAN.NGUOI_CNHAT = ClientInformation.TenDangNhap;
                }
                else if (idTangTS == 0)
                {
                    objTSDto.TS_TAI_SAN.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                    objTSDto.TS_TAI_SAN.TTHAI_NVU = nghiepvu.layGiaTri();
                    objTSDto.TS_TAI_SAN.NGAY_NHAP = ClientInformation.NgayLamViecHienTai;
                    objTSDto.TS_TAI_SAN.NGUOI_NHAP = ClientInformation.TenDangNhap;
                }

                // Tổng nguyên giá
                objTSDto.TS_TAI_SAN.TONG_NGUYEN_GIA = lstNguyenGia.Sum(e => e.GIA_TRI);
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }

        }

        public void SetDataForm()
        {
            try
            {
                // Do something
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                TaiSanProcess process = new TaiSanProcess();
                List<int> lstID = new List<int>();
                if (process.QuanLyTaiSan(DatabaseConstant.Function.TS_TANG, DatabaseConstant.Action.XEM, lstID, ref objTSDto, ref listClientResponseDetail))
                {
                    txtMaTS.Text = objTSDto.TS_TAI_SAN.MA_TAI_SAN;
                    txtTenTS.Text = objTSDto.TS_TAI_SAN.TEN_TAI_SAN;
                    foreach (var item in cmbHinhThucNhap.Items)
                    {
                        AutoCompleteEntry entry = (AutoCompleteEntry)item;
                        if (entry.KeywordStrings.First().Equals(objTSDto.TS_TAI_SAN.MA_TINH_TRANG))
                            cmbHinhThucNhap.SelectedItem = item;
                    }
                    foreach (var item in cmbDonViTinh.Items)
                    {
                        AutoCompleteEntry entry = (AutoCompleteEntry)item;
                        if (entry.KeywordStrings.First().Equals(objTSDto.TS_TAI_SAN.DON_VI_TINH))
                            cmbDonViTinh.SelectedItem = item;
                    }
                    foreach (var item in cmbTinhTrang.Items)
                    {
                        AutoCompleteEntry entry = (AutoCompleteEntry)item;
                        if (entry.KeywordStrings.First().Equals(objTSDto.TS_TAI_SAN.TINH_TRANG_HIEN_TAI))
                            cmbTinhTrang.SelectedItem = item;
                    }
                    teldtNgayLap.Value = DateTime.Today;
                    foreach (var item in cmbNhomTS.Items)
                    {
                        AutoCompleteEntry entry = (AutoCompleteEntry)item;
                        if (entry.KeywordStrings.First().Equals(objTSDto.TS_TAI_SAN.MA_NHOM_TSCD))
                            cmbNhomTS.SelectedItem = item;
                    }
                    foreach (var item in cmbNguonGoc.Items)
                    {
                        AutoCompleteEntry entry = (AutoCompleteEntry)item;
                        if (entry.KeywordStrings.First().Equals(objTSDto.TS_TAI_SAN.NGUON_TAI_SAN))
                            cmbNguonGoc.SelectedItem = item;
                    }
                    chkTinhKhauHao.IsChecked = !objTSDto.TS_TAI_SAN.PHUONG_PHAP_TINH_KH.IsNullOrEmptyOrSpace();

                    cmbPhuongPhapKH.IsEnabled = chkTinhKhauHao.IsChecked.Value;
                    numThoiGianKH.IsEnabled = chkTinhKhauHao.IsChecked.Value;
                    if (chkTinhKhauHao.IsChecked.Value)
                    {
                        foreach (var item in cmbPhuongPhapKH.Items)
                        {
                            AutoCompleteEntry entry = (AutoCompleteEntry)item;
                            if (entry.KeywordStrings.First().Equals(objTSDto.TS_TAI_SAN.PHUONG_PHAP_TINH_KH))
                                cmbPhuongPhapKH.SelectedItem = item;
                        }
                        numThoiGianKH.Value = objTSDto.TS_TAI_SAN.THOI_GIAN_KH;
                    }
                    lblNgayBanGiaoTam.Content = "";
                    lblNgayBanGiaoChinhThuc.Content = "";
                    lblDonViSuDung.Content = objTSDto.TS_TAI_SAN.DVI_SDUNG;
                    lblDoiTuongSuDung.Content = "";
                    txtTrangThai.Text = objTSDto.TS_TAI_SAN.TTHAI_BGHI;
                    if (!objTSDto.TS_TAI_SAN.NGAY_QUYET_DINH.IsNullOrEmpty())
                        teldtNgayLap.Value = objTSDto.TS_TAI_SAN.NGAY_QUYET_DINH.StringToDate("yyyyMMdd");
                    teldtNgayNhap.Value = objTSDto.TS_TAI_SAN.NGAY_NHAP.StringToDate("yyyyMMdd");
                    txtNguoiLap.Text = objTSDto.TS_TAI_SAN.NGUOI_NHAP;
                    if (!objTSDto.TS_TAI_SAN.NGAY_CNHAT.IsNullOrEmpty())
                        teldtNgayCNhat.Value = objTSDto.TS_TAI_SAN.NGAY_CNHAT.StringToDate("yyyyMMdd");
                    if (!objTSDto.TS_TAI_SAN.NGUOI_CNHAT.IsNullOrEmpty())
                        txtNguoiCapNhat.Text = objTSDto.TS_TAI_SAN.NGUOI_CNHAT;
                    List<TS_TANG_TTINH> lstGet = objTSDto.Lst_TS_TTINH.ToList();
                    lstPhuKien = objTSDto.Lst_TS_PHU_KIEN.ToList();
                    foreach (TS_TANG_NG ng in objTSDto.Lst_TTS_TANG_NG.ToList())
                        ng.TRANG_THAI = BusinessConstant.layNgonNguNghiepVu(ng.TTHAI_NVU);
                    lstNguyenGia = objTSDto.Lst_TTS_TANG_NG.ToList();
                    grdPhuKien.ItemsSource = lstPhuKien;
                    grdNguyenGia.ItemsSource = lstNguyenGia;
                    if (!lstGet.IsNullOrEmpty())
                    {
                        foreach (var ttinh in lstThuocTinh)
                        {
                            var tt = lstGet.FirstOrDefault(e => e.TEN_TTINH.Equals(ttinh.TEN_TTINH) && e.ID_TAI_SAN_TANG == idTangTS);
                            if (!tt.IsNullOrEmpty())
                                ttinh.GIA_TRI_TTINH = tt.GIA_TRI_TTINH;
                        }
                        grdThuocTinh.ItemsSource = lstThuocTinh;
                    }
                    TThaiNVu = objTSDto.TS_TAI_SAN.TTHAI_NVU;

                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(TThaiNVu);
                    #region Tab thông tin kiểm soát
                    txtTrangThai.Text = BusinessConstant.layNgonNguNghiepVu(objTSDto.TS_TAI_SAN.TTHAI_BGHI);
                    teldtNgayNhap.Value = LDateTime.StringToDate(objTSDto.TS_TAI_SAN.NGAY_NHAP, "yyyyMMdd");
                    txtNguoiLap.Text = objTSDto.TS_TAI_SAN.NGUOI_NHAP;
                    if (LDateTime.IsDate(objTSDto.TS_TAI_SAN.NGAY_CNHAT, "yyyyMMdd") == true)
                        teldtNgayCNhat.Value = LDateTime.StringToDate(objTSDto.TS_TAI_SAN.NGAY_CNHAT, "yyyyMMdd");
                    else
                        teldtNgayCNhat.Value = null;
                    txtNguoiCapNhat.Text = objTSDto.TS_TAI_SAN.NGUOI_CNHAT;
                    #endregion

                    if (!Action.Equals(DatabaseConstant.Action.SUA))
                    {
                        CommonFunction.RefreshButton(Toolbar, Action, TThaiNVu, mnuMain, DatabaseConstant.Function.TS_TANG);
                        SetEnabledAllControl(false);
                        RefreshButton();
                    }
                    else
                    {
                        if (TThaiNVu.Equals(BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri()))
                        {
                            CommonFunction.RefreshButton(Toolbar, Action, TThaiNVu, mnuMain, DatabaseConstant.Function.TS_TANG);
                            SetEnabledAllControl(false);
                            RefreshButton();
                            if (objTSDto.TS_TAI_SAN.MA_TINH_TRANG.Equals(BusinessConstant.HINH_THUC_NHAP_TS.DO_DANG.layGiaTri()))
                            {
                                tlbAddNguyeGia.IsEnabled = true;
                                cmbHinhThucNhap.IsEnabled = true;
                            }
                        }
                        else
                            Modify();
                    }
                }
            }
            catch (Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        void SetDataTabThongTinChung(DataSet ds)
        {
            try
            {

            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        void SetDataTabThongTinKiemSoat(DataSet ds)
        {
            try
            {

            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }

        }

        bool Validation()
        {
            bool bReturn = true;
            string khoangKhauHao = ((AutoCompleteEntry)cmbNhomTS.SelectedItem).KeywordStrings[1];
            int khauHaoTu = khoangKhauHao.SplitByDelimiter("-")[0].StringToInt32();
            int khauHaoDen = khoangKhauHao.SplitByDelimiter("-")[1].StringToInt32();
            try
            {
                if (teldtNgayLap.Value.IsNullOrEmpty())
                {
                    LMessage.ShowMessage("Thiếu ngày lập", LMessage.MessageBoxType.Warning);
                    teldtNgayLap.Focus();
                    return false;
                }
                else if (txtTenTS.Text.IsNullOrEmptyOrSpace())
                {
                    LMessage.ShowMessage("Thiếu tên sản phẩm", LMessage.MessageBoxType.Warning);
                    txtTenTS.Focus();
                    return false;
                }
                else if (chkTinhKhauHao.IsChecked == true)
                {
                    if (numThoiGianKH.Value.IsNullOrEmpty() || numThoiGianKH.Value == 0)
                    {
                        LMessage.ShowMessage("Thời gian khấu hao phải lớn hơn 0.", LMessage.MessageBoxType.Warning);
                        numThoiGianKH.Focus();
                        return false;
                    }
                    else if (numThoiGianKH.Value < khauHaoTu || numThoiGianKH.Value > khauHaoDen)
                    {
                        LMessage.ShowMessage("Thời gian khấu hao phải từ " + khauHaoTu + " tháng đến " + khauHaoDen + " tháng.", LMessage.MessageBoxType.Warning);
                        numThoiGianKH.Focus();
                        return false;
                    }
                }

                List<TS_TANG_NG> lstNguyenGiaTmp = grdNguyenGia.ItemsSource as List<TS_TANG_NG>;
                if (lstNguyenGiaTmp == null || (lstNguyenGiaTmp != null && lstNguyenGiaTmp.Count == 0))
                {
                    LMessage.ShowMessage("Thiếu nguyên giá", LMessage.MessageBoxType.Warning);
                    return false;
                }
                else
                {
                    foreach (TS_TANG_NG item in lstNguyenGiaTmp)
                    {
                        if (item.HTHUC_TTOAN.Equals(BusinessConstant.HINH_THUC_GIAO_DICH.CHUYEN_KHOAN) && item.TKHOAN_TTOAN.IsNullOrEmptyOrSpace())
                        {
                            LMessage.ShowMessage("Nguyên giá có số chứng từ " + item.SO_CTU + " chưa nhập tài khoản.", LMessage.MessageBoxType.Warning);
                            return false;
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                bReturn = false;
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            return bReturn;
        }

        bool ValidationHoanThanh()
        {
            bool bReturn = true;
            try
            {
                List<TS_TANG_NG> lstNguyenGiaTmp = grdNguyenGia.ItemsSource as List<TS_TANG_NG>;
                if (lstNguyenGiaTmp == null || (lstNguyenGiaTmp != null && lstNguyenGiaTmp.Count == 0))
                {
                    LMessage.ShowMessage("Thiếu nguyên giá", LMessage.MessageBoxType.Warning);
                    return false;
                }
                else
                {
                    lstNguyenGiaTmp = lstNguyenGiaTmp.Where(e => !e.TTHAI_NVU.Equals(BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri())).ToList();
                    if (!lstNguyenGiaTmp.IsNullOrEmpty() && lstNguyenGiaTmp.Count > 0)
                    {
                        LMessage.ShowMessage("Chưa duyệt hết các nguyên giá sản phẩm.", LMessage.MessageBoxType.Warning);
                        return false;
                    }
                }
            }
            catch (System.Exception ex)
            {
                bReturn = false;
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            return bReturn;
        }

        void Modify()
        {
            List<int> lstId = new List<int>();
            lstId.Add(idTangTS);
            UtilitiesProcess process = new UtilitiesProcess();
            bool retLockData = process.LockData(DatabaseConstant.Module.QLTS,
            DatabaseConstant.Function.TS_TANG,
            DatabaseConstant.Table.TS_TANG,
            DatabaseConstant.Action.SUA,
            lstId);
            Action = DatabaseConstant.Action.SUA;
            CommonFunction.RefreshButton(Toolbar, Action, TThaiNVu, mnuMain, DatabaseConstant.Function.TS_TANG);
            RefreshButton();
            SetEnabledAllControl(true);
        }

        void BeforeSave(BusinessConstant.TrangThaiNghiepVu nghiepvu, BusinessConstant.TrangThaiBanGhi bghi)
        {
            Cursor = Cursors.Wait;
            try
            {
                //GetDataForm(bghi, nghiepvu);
                if (!nghiepvu.Equals(BusinessConstant.TrangThaiNghiepVu.LUU_TAM))
                {
                    if (!Validation())
                    {
                        Cursor = Cursors.Arrow;
                        return;
                    }
                }
                GetDataForm(bghi, nghiepvu);
                OnSave();
            }
            catch (System.Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                Cursor = Cursors.Arrow;
            }

        }
        void OnSave()
        {
            try
            {
                List<int> lstID = new List<int>();
                TaiSanProcess process = new TaiSanProcess();
                List<ClientResponseDetail> lstResponseDetail = new List<ClientResponseDetail>();
                int iret = 0;
                if (idTangTS == 0)
                {
                    Action = DatabaseConstant.Action.THEM;
                    objTSDto.TS_TAI_SAN.NGAY_NHAP = ClientInformation.NgayLamViecHienTai;
                    objTSDto.TS_TAI_SAN.NGUOI_NHAP = ClientInformation.TenDangNhap;
                    objTSDto.TS_TAI_SAN.MA_DVI_QLY = ClientInformation.MaDonVi;
                    objTSDto.TS_TAI_SAN.MA_DVI_TAO = ClientInformation.MaDonViGiaoDich;
                    if (!process.QuanLyTaiSan(DatabaseConstant.Function.TS_TANG, Action, lstID, ref objTSDto, ref lstResponseDetail))
                        iret = 1;
                }
                else
                {
                    Action = DatabaseConstant.Action.SUA;
                    objTSDto.TS_TAI_SAN.NGUOI_CNHAT = ClientInformation.NgayLamViecHienTai;
                    objTSDto.TS_TAI_SAN.NGUOI_CNHAT = ClientInformation.TenDangNhap;
                    objTSDto.TS_TAI_SAN.ID = idTangTS;
                    if (!process.QuanLyTaiSan(DatabaseConstant.Function.TS_TANG, Action, lstID, ref objTSDto, ref lstResponseDetail))
                        iret = 1;
                }
                AfterSave(lstResponseDetail, iret);
            }
            catch (System.Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }
        void AfterSave(List<ClientResponseDetail> lstResponseDetail, int iret)
        {
            try
            {
                if (iret == 0)
                    LMessage.ShowMessage("M.DungChung.LuuDuLieuThanhCong", LMessage.MessageBoxType.Information);
                else
                    CommonFunction.ThongBaoKetQua(lstResponseDetail);
                // Yêu cầu Unlock dữ liệu
                List<int> lstId = new List<int>();
                lstId.Add(idTangTS);
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.UnlockData(DatabaseConstant.Module.QLTS,
                DatabaseConstant.Function.TS_TANG,
                DatabaseConstant.Table.TS_TANG,
                DatabaseConstant.Action.SUA,
                lstId);
                if (OnSavingCompleted != null)
                    OnSavingCompleted(null, EventArgs.Empty);
                if (cbMultiAdd.IsChecked != true || Action == DatabaseConstant.Action.SUA)
                {
                    Action = DatabaseConstant.Action.XEM;
                    idTangTS = objTSDto.TS_TAI_SAN.ID;
                    TThaiNVu = objTSDto.TS_TAI_SAN.TTHAI_NVU;
                    txtMaTS.Text = objTSDto.TS_TAI_SAN.MA_TAI_SAN;
                    SetEnabledAllControl(false);
                    CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, DatabaseConstant.Function.TS_TANG);
                }
                else
                {
                    ClearForm();
                    idTangTS = 0;
                    TThaiNVu = "";
                    CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.THEM, tthaiNvu, mnuMain, DatabaseConstant.Function.TS_TANG);
                }

            }
            catch (System.Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        void BeforeDelete()
        {
            Cursor = Cursors.Wait;
            try
            {
                // Yêu cầu Lock dữ liệu
                List<int> lstId = new List<int>();
                lstId.Add(idTangTS);
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.LockData(DatabaseConstant.Module.QLTS,
                DatabaseConstant.Function.TS_TANG,
                DatabaseConstant.Table.TS_TANG,
                DatabaseConstant.Action.XOA,
                lstId);
                OnDelete();
            }
            catch (System.Exception ex)
            {
                // Yêu cầu UnLock dữ liệu
                List<int> lstId = new List<int>();
                lstId.Add(idTangTS);
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.UnlockData(DatabaseConstant.Module.QLTS,
                DatabaseConstant.Function.TS_TANG,
                DatabaseConstant.Table.TS_TANG,
                DatabaseConstant.Action.XOA,
                lstId);
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally { Cursor = Cursors.Arrow; }
        }
        void OnDelete()
        {
            List<ClientResponseDetail> lstResponseDetail = new List<ClientResponseDetail>();
            int iret = 0;
            if (idTangTS != 0)
            {
                List<int> lstID = new List<int>();
                TaiSanProcess process = new TaiSanProcess();
                objTSDto.TS_TAI_SAN.ID = idTangTS;
                if (!process.QuanLyTaiSan(DatabaseConstant.Function.TS_TANG, DatabaseConstant.Action.XOA, lstID, ref objTSDto, ref lstResponseDetail))
                    iret = 1;
            }
            AfterDelete(lstResponseDetail, iret);
        }
        void AfterDelete(List<ClientResponseDetail> lstResponseDetail, int iret)
        {
            if (iret == 0)
                LMessage.ShowMessage("M.DungChung.XoaThanhCong", LMessage.MessageBoxType.Information);
            else
                CommonFunction.ThongBaoKetQua(lstResponseDetail);
            // Yêu cầu Unlock dữ liệu
            List<int> lstId = new List<int>();
            lstId.Add(idTangTS);
            UtilitiesProcess process = new UtilitiesProcess();
            bool retLockData = process.UnlockData(DatabaseConstant.Module.QLTS,
            DatabaseConstant.Function.TS_TANG,
            DatabaseConstant.Table.TS_TANG,
            DatabaseConstant.Action.XOA,
            lstId);
            Action = DatabaseConstant.Action.XEM;
            if (OnSavingCompleted != null)
                OnSavingCompleted(null, EventArgs.Empty);
            Cursor = Cursors.Arrow;
            if (iret == 0) CommonFunction.CloseUserControl(this);
        }

        void BeforeApprove()
        {
            Cursor = Cursors.Wait;
            try
            {
                // Yêu cầu Lock dữ liệu
                List<int> lstId = new List<int>();
                lstId.Add(idTangTS);
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.LockData(DatabaseConstant.Module.QLTS,
                DatabaseConstant.Function.TS_TANG,
                DatabaseConstant.Table.TS_TANG,
                DatabaseConstant.Action.DUYET,
                lstId);
                OnApprove();
            }
            catch (System.Exception ex)
            {
                // Yêu cầu UnLock dữ liệu
                List<int> lstId = new List<int>();
                lstId.Add(idTangTS);
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.UnlockData(DatabaseConstant.Module.QLTS,
                DatabaseConstant.Function.TS_TANG,
                DatabaseConstant.Table.TS_TANG,
                DatabaseConstant.Action.DUYET,
                lstId);
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally { Cursor = Cursors.Arrow; }
        }
        void OnApprove()
        {
            List<ClientResponseDetail> lstResponseDetail = new List<ClientResponseDetail>();
            int iret = 0;
            if (idTangTS != 0)
            {
                List<int> lstID = new List<int>();
                TaiSanProcess process = new TaiSanProcess();
                objTSDto.TS_TAI_SAN.ID = idTangTS;
                if (!process.QuanLyTaiSan(DatabaseConstant.Function.TS_TANG, DatabaseConstant.Action.DUYET, lstID, ref objTSDto, ref lstResponseDetail))
                    iret = 1;
            }
            AfterApprove(lstResponseDetail, iret);
        }
        void AfterApprove(List<ClientResponseDetail> lstResponseDetail, int iret)
        {
            if (iret == 0)
                LMessage.ShowMessage("M.DungChung.DuyetThanhCong", LMessage.MessageBoxType.Information);
            else
                CommonFunction.ThongBaoKetQua(lstResponseDetail);
            // Yêu cầu Unlock dữ liệu
            List<int> lstId = new List<int>();
            lstId.Add(idTangTS);
            UtilitiesProcess process = new UtilitiesProcess();
            bool retLockData = process.UnlockData(DatabaseConstant.Module.QLTS,
            DatabaseConstant.Function.TS_TANG,
            DatabaseConstant.Table.TS_TANG,
            DatabaseConstant.Action.DUYET,
            lstId);
            Action = DatabaseConstant.Action.XEM;
            if (OnSavingCompleted != null)
                OnSavingCompleted(null, EventArgs.Empty);
            SetDataForm();
            Cursor = Cursors.Arrow;
        }

        void BeforeRefuse()
        {
            try
            {
                Cursor = Cursors.Wait;
                // Yêu cầu Lock dữ liệu
                List<int> lstId = new List<int>();
                lstId.Add(idTangTS);
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.LockData(DatabaseConstant.Module.QLTS,
                DatabaseConstant.Function.TS_TANG,
                DatabaseConstant.Table.TS_TANG,
                DatabaseConstant.Action.TU_CHOI_DUYET,
                lstId);
                OnRefuse();
            }
            catch (System.Exception ex)
            {
                // Yêu cầu UnLock dữ liệu
                List<int> lstId = new List<int>();
                lstId.Add(idTangTS);
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.UnlockData(DatabaseConstant.Module.QLTS,
                DatabaseConstant.Function.TS_TANG,
                DatabaseConstant.Table.TS_TANG,
                DatabaseConstant.Action.TU_CHOI_DUYET,
                lstId);
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally { Cursor = Cursors.Arrow; }
        }
        void OnRefuse()
        {
            List<ClientResponseDetail> lstResponseDetail = new List<ClientResponseDetail>();
            int iret = 0;
            if (idTangTS != 0)
            {
                List<int> lstID = new List<int>();
                TaiSanProcess process = new TaiSanProcess();
                objTSDto.TS_TAI_SAN.ID = idTangTS;
                process.QuanLyTaiSan(DatabaseConstant.Function.TS_TANG, DatabaseConstant.Action.TU_CHOI_DUYET, lstID, ref objTSDto, ref lstResponseDetail);
            }
            AfterRefuse(lstResponseDetail, iret);
        }
        void AfterRefuse(List<ClientResponseDetail> lstResponseDetail, int iret)
        {
            CommonFunction.ThongBaoKetQua(lstResponseDetail);
            // Yêu cầu Unlock dữ liệu
            List<int> lstId = new List<int>();
            lstId.Add(idTangTS);
            UtilitiesProcess process = new UtilitiesProcess();
            bool retLockData = process.UnlockData(DatabaseConstant.Module.QLTS,
            DatabaseConstant.Function.TS_TANG,
            DatabaseConstant.Table.TS_TANG,
            DatabaseConstant.Action.TU_CHOI_DUYET,
            lstId);
            Action = DatabaseConstant.Action.XEM;
            if (OnSavingCompleted != null)
                OnSavingCompleted(null, EventArgs.Empty);
            Cursor = Cursors.Arrow;
            SetInfomation();
        }

        void BeforeCancel()
        {
            try
            {
                Cursor = Cursors.Wait;
                // Yêu cầu Lock dữ liệu
                List<int> lstId = new List<int>();
                lstId.Add(idTangTS);
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.LockData(DatabaseConstant.Module.QLTS,
                DatabaseConstant.Function.TS_TANG,
                DatabaseConstant.Table.TS_TANG,
                DatabaseConstant.Action.THOAI_DUYET,
                lstId);
                OnCancel();
            }
            catch (System.Exception ex)
            {
                // Yêu cầu UnLock dữ liệu
                List<int> lstId = new List<int>();
                lstId.Add(idTangTS);
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.UnlockData(DatabaseConstant.Module.QLTS,
                DatabaseConstant.Function.TS_TANG,
                DatabaseConstant.Table.TS_TANG,
                DatabaseConstant.Action.THOAI_DUYET,
                lstId);
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);

            }
            finally
            {
                Cursor = Cursors.Arrow;
            }
        }
        void OnCancel()
        {
            List<ClientResponseDetail> lstResponseDetail = new List<ClientResponseDetail>();
            int iret = 0;
            if (idTangTS != 0)
            {
                List<int> lstID = new List<int>();
                TaiSanProcess process = new TaiSanProcess();
                objTSDto.TS_TAI_SAN.ID = idTangTS;
                process.QuanLyTaiSan(DatabaseConstant.Function.TS_TANG, DatabaseConstant.Action.THOAI_DUYET, lstID, ref objTSDto, ref lstResponseDetail);
            }
            AfterCancel(lstResponseDetail, iret);
        }
        void AfterCancel(List<ClientResponseDetail> lstResponseDetail, int iret)
        {
            CommonFunction.ThongBaoKetQua(lstResponseDetail);
            // Yêu cầu Unlock dữ liệu
            List<int> lstId = new List<int>();
            lstId.Add(idTangTS);
            UtilitiesProcess process = new UtilitiesProcess();
            bool retLockData = process.UnlockData(DatabaseConstant.Module.QLTS,
            DatabaseConstant.Function.TS_TANG,
            DatabaseConstant.Table.TS_TANG,
            DatabaseConstant.Action.THOAI_DUYET,
            lstId);
            Action = DatabaseConstant.Action.XEM;
            if (OnSavingCompleted != null)
                OnSavingCompleted(null, EventArgs.Empty);
            Cursor = Cursors.Arrow;
            SetInfomation();
        }

        private void OnPreviewChungTu()
        {
            // Cảnh báo khi không có dữ liệu
            if (LObject.IsNullOrEmpty(idTangTS))
            {
                LMessage.ShowMessage("M.TinDung.KheUoc.ucKheUocCT.KhongCoThongTinKheUoc", LMessage.MessageBoxType.Warning);
                return;
            }
            else
            {
                // Lấy thông tin giao dịch theo biến động
                TaiSanProcess process = new TaiSanProcess();
                BIEN_DONG_DTO objBienDongDTO = new BIEN_DONG_DTO();
                KIEM_SOAT objKiemSoat = new KIEM_SOAT();
                objBienDongDTO.Function = DatabaseConstant.Function.TS_TANG;
                objBienDongDTO.IdBienDong = idTangTS;

                bool ret = process.LayThongTinGiaoDich(ref objKiemSoat, objBienDongDTO);

                if (ret)
                {
                    VanHanhGiaoDich xemBaoCao = new VanHanhGiaoDich();
                    DoiTuongBaoCao doiTuongBaoCao = new DoiTuongBaoCao();
                    DatabaseConstant.Function _function = DatabaseConstant.Function.KT_PHIEU_KE_TOAN;

                    GIAO_DICH_BASE objGIAO_DICH_BASE = new GIAO_DICH_BASE();
                    objGIAO_DICH_BASE.BaoCao = DatabaseConstant.DanhSachBaoCaoTheoGiaoDich.GDKT_IN_GIAO_DICH;
                    objGIAO_DICH_BASE.ChucNang = _function;

                    GDKT_GIAO_DICH objGDKT_GIAO_DICH = new GDKT_GIAO_DICH();
                    objGDKT_GIAO_DICH.MaGiaoDich = objKiemSoat.SO_GIAO_DICH;

                    doiTuongBaoCao.objGIAO_DICH_BASE = objGIAO_DICH_BASE;
                    doiTuongBaoCao.objGDKT_GIAO_DICH = objGDKT_GIAO_DICH;
                    xemBaoCao.GiaoDichPhatSinh(ref doiTuongBaoCao);
                }
                else
                {
                }
            }

        }

        private void OnSaveNguyenGia()
        {
            Cursor = Cursors.Wait;
            try
            {
                TaiSanProcess process = new TaiSanProcess();
                List<TS_TANG_NG> lstNguyenGiaThem = new List<TS_TANG_NG>();
                foreach (TS_TANG_NG item in grdNguyenGia.Items)
                {
                    if (!item.IsNullOrEmpty() && !item.TTHAI_NVU.Equals(BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri()))
                    {
                        TS_TANG_NG nguyenGia = new TS_TANG_NG();
                        nguyenGia.ID_TANG = objTSDto.TS_TAI_SAN.ID;
                        nguyenGia.LAN_TANG = item.LAN_TANG;
                        nguyenGia.GIA_TRI = item.GIA_TRI;
                        nguyenGia.CAU_THANH = item.CAU_THANH;
                        nguyenGia.HTHUC_TTOAN = item.HTHUC_TTOAN;
                        if (!item.NGAY_CTU.IsDate("yyyyMMdd"))
                            item.NGAY_CTU = (Convert.ToDateTime(item.NGAY_CTU)).DateToString("yyyyMMdd");
                        nguyenGia.NGAY_CTU = item.NGAY_CTU;
                        nguyenGia.SO_CTU = item.SO_CTU;
                        nguyenGia.TKHOAN_TTOAN = item.TKHOAN_TTOAN;
                        nguyenGia.NGAY_NHAP = ClientInformation.NgayLamViecHienTai;
                        nguyenGia.NGUOI_NHAP = ClientInformation.TenDangNhap;
                        nguyenGia.MA_DVI_QLY = ClientInformation.MaDonVi;
                        nguyenGia.MA_DVI_TAO = ClientInformation.MaDonViGiaoDich;
                        nguyenGia.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                        nguyenGia.TTHAI_NVU = BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri();
                        lstNguyenGiaThem.Add(nguyenGia);
                    }
                }
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                DatabaseConstant.Action action = DatabaseConstant.Action.THEM;
                bool isSuccsess = process.TangNguyenGia(action, ref lstNguyenGiaThem, ref listClientResponseDetail);
                AfterProcessNguyenGia(isSuccsess, listClientResponseDetail);
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally { Cursor = Cursors.Arrow; }
        }

        private void SetInfomation()
        {
            if (!LObject.IsNullOrEmpty(objTSDto))
            {
                SetEnabledAllControl(false);
                CommonFunction.RefreshButton(Toolbar, Action, TThaiNVu, mnuMain, DatabaseConstant.Function.TS_TANG);
                RefreshButton();
            }
            else
                SetDataForm();
        }

        private void tlbSubmitNguyeGia_Click(object sender, RoutedEventArgs e)
        {
            OnSaveNguyenGia();
        }

        private void tlbApproveNguyeGia_Click(object sender, RoutedEventArgs e)
        {
            Cursor = Cursors.Wait;
            TaiSanProcess process = new TaiSanProcess();
            List<TS_TANG_NG> lstNguyenGiaThem = new List<TS_TANG_NG>();
            lstNguyenGia = grdNguyenGia.ItemsSource as List<TS_TANG_NG>;
            foreach (TS_TANG_NG item in lstNguyenGia)
            {
                if (!item.IsNullOrEmpty() && item.TTHAI_NVU.Equals(BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri()))
                {
                    TS_TANG_NG nguyenGia = item;
                    if (!nguyenGia.NGAY_CTU.IsDate("yyyyMMdd"))
                        nguyenGia.NGAY_CTU = (Convert.ToDateTime(nguyenGia.NGAY_CTU)).DateToString("yyyyMMdd");
                    nguyenGia.TKHOAN_TTOAN = item.TKHOAN_TTOAN;
                    nguyenGia.NGAY_CNHAT = ClientInformation.NgayLamViecHienTai;
                    nguyenGia.NGUOI_CNHAT = ClientInformation.TenDangNhap;
                    nguyenGia.MA_DVI_QLY = ClientInformation.MaDonVi;
                    nguyenGia.MA_DVI_TAO = ClientInformation.MaDonViGiaoDich;
                    nguyenGia.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                    nguyenGia.TTHAI_NVU = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();
                    lstNguyenGiaThem.Add(nguyenGia);
                }
            }
            List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
            DatabaseConstant.Action action = DatabaseConstant.Action.DUYET;
            bool isSuccsess = process.TangNguyenGia(action, ref lstNguyenGiaThem, ref listClientResponseDetail);
            AfterProcessNguyenGia(isSuccsess, listClientResponseDetail);

            Cursor = Cursors.Arrow;
        }

        private void tlbRefuseNguyeGia_Click(object sender, RoutedEventArgs e)
        {
            Cursor = Cursors.Wait;
            TaiSanProcess process = new TaiSanProcess();
            List<TS_TANG_NG> lstNguyenGia = new List<TS_TANG_NG>();
            foreach (TS_TANG_NG item in grdNguyenGia.SelectedItems)
            {
                if (!item.IsNullOrEmpty() && item.TTHAI_NVU.Equals(BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri()))
                {
                    TS_TANG_NG nguyenGia = item;
                    if (!nguyenGia.NGAY_CTU.IsDate("yyyyMMdd"))
                        nguyenGia.NGAY_CTU = (Convert.ToDateTime(nguyenGia.NGAY_CTU)).DateToString("yyyyMMdd");
                    nguyenGia.TKHOAN_TTOAN = item.TKHOAN_TTOAN;
                    nguyenGia.NGAY_CNHAT = ClientInformation.NgayLamViecHienTai;
                    nguyenGia.NGUOI_CNHAT = ClientInformation.TenDangNhap;
                    nguyenGia.MA_DVI_QLY = ClientInformation.MaDonVi;
                    nguyenGia.MA_DVI_TAO = ClientInformation.MaDonViGiaoDich;
                    nguyenGia.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                    nguyenGia.TTHAI_NVU = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();
                    lstNguyenGia.Add(nguyenGia);
                }
            }
            List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
            DatabaseConstant.Action action = DatabaseConstant.Action.TU_CHOI_DUYET;
            bool isSuccsess = process.TangNguyenGia(action, ref lstNguyenGia, ref listClientResponseDetail);
            AfterProcessNguyenGia(isSuccsess, listClientResponseDetail);
            Cursor = Cursors.Arrow;
        }

        private void tlbAddNguyeGia_Click(object sender, RoutedEventArgs e)
        {
            //this.grdNguyenGia.BeginInsert();
            lstNguyenGia = grdNguyenGia.ItemsSource as List<TS_TANG_NG>;
            TS_TANG_NG ng = new TS_TANG_NG();
            ng.GIA_TRI = 0;
            ng.ID = 0;
            ng.ID_TANG = idTangTS;
            ng.NGAY_CTU = ClientInformation.NgayLamViecHienTai;
            ng.LAN_TANG = 0;
            ng.TTHAI_NVU = "NA";
            ng.HTHUC_TTOAN = BusinessConstant.HINH_THUC_GIAO_DICH.TIEN_MAT.layGiaTri();
            lstNguyenGia.Add(ng);
            grdNguyenGia.ItemsSource = lstNguyenGia;
            grdNguyenGia.Rebind();
            /*
            GridViewDataRowInfo rowInfo = new GridViewDataRowInfo(this.radGridView1.MasterView);
            rowInfo.Cells[0].Value = "GridViewDataRowInfo";
            rowInfo.Cells[1].Value = 11.4;
            rowInfo.Cells[2].Value = DateTime.Now.AddDays(5);
            rowInfo.Cells[3].Value = true;
            radGridView1.Rows.Add(rowInfo);   
            */
            /*
            DataRow rowInfo = new DataRow();
            rowInfo["ID"] = "0";
            rowInfo["SO_CTU"] = "GridViewDataRowInfo";
            rowInfo["NGAY_CTU"] = "GridViewDataRowInfo";
            rowInfo["CAU_THANH"] = "GridViewDataRowInfo";
            rowInfo["GIA_TRI"] = "GridViewDataRowInfo";
            rowInfo["HTHUC_TTOAN"] = "GridViewDataRowInfo";
            rowInfo["TKHOAN_TTOAN"] = "GridViewDataRowInfo";
            rowInfo["TTHAI_NVU"] = "GridViewDataRowInfo";
            grdNguyenGia.Items.Add(rowInfo);
            */
        }

        private void tlbDeleteNguyeGia_Click(object sender, RoutedEventArgs e)
        {
            Cursor = Cursors.Wait;
            if (grdNguyenGia.SelectedItems.Count > 0)
            {
                TS_TANG_NG nguyenGiaXoa = grdNguyenGia.SelectedItem as TS_TANG_NG;
                if (!nguyenGiaXoa.IsNullOrEmpty())
                {
                    if (nguyenGiaXoa.TTHAI_NVU.Equals("NA"))
                        grdNguyenGia.Items.Remove(grdNguyenGia.SelectedItem);
                    else if (nguyenGiaXoa.TTHAI_NVU.Equals(BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri()))
                    {
                        LMessage.ShowMessage("Nguyên giá đã duyệt không được phép xóa.", LMessage.MessageBoxType.Warning);
                        return;
                    }
                    else if (nguyenGiaXoa.TTHAI_NVU.Equals(BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri()))
                    {
                        TaiSanProcess process = new TaiSanProcess();
                        List<TS_TANG_NG> lstNguyenGia = new List<TS_TANG_NG>();
                        lstNguyenGia.Add(nguyenGiaXoa);
                        DatabaseConstant.Action action = DatabaseConstant.Action.XOA;
                        List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                        bool isSuccsess = process.TangNguyenGia(action, ref lstNguyenGia, ref listClientResponseDetail);
                        AfterProcessNguyenGia(isSuccsess, listClientResponseDetail);
                    }
                }
            }
            else
            {
                LMessage.ShowMessage("Chưa chọn nguyên giá để xóa", LMessage.MessageBoxType.Warning);
                return;
            }
            Cursor = Cursors.Arrow;
        }

        private void grdNguyenGia_AddingNewDataItem(object sender, GridViewAddingNewEventArgs e)
        {
            int lantang = 1;
            List<TS_TANG_NG> lstNguyenGiaTmp = grdNguyenGia.ItemsSource as List<TS_TANG_NG>;
            if (lstNguyenGiaTmp != null && lstNguyenGiaTmp.Count > 0)
            {
                if (lstNguyenGiaTmp.Where(n => !n.TTHAI_NVU.Equals(BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri())).Count() > 0)
                    lantang = lstNguyenGiaTmp.Where(n => !n.TTHAI_NVU.Equals(BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri())).Max(n => n.LAN_TANG.Value);
                else
                    lantang = lstNguyenGiaTmp.Max(n => n.LAN_TANG.Value) + 1;
            }
            e.NewObject = new TS_TANG_NG() { HTHUC_TTOAN = BusinessConstant.HINH_THUC_GIAO_DICH.TIEN_MAT.layGiaTri(), LAN_TANG = lantang, TTHAI_NVU = "NA", TRANG_THAI = "NA", NGAY_CTU = ClientInformation.NgayLamViecHienTai };

            //e.Cancel = true;
            //var newRow = this.da.DefaultView.AddNew();
            //newRow["FirstName"] = "John";
            //newRow["LastName"] = "Doe";
            //e.NewObject = newRow;
        }

        private void grdNguyenGia_RowEditEnded(object sender, GridViewRowEditEndedEventArgs e)
        {
            if (e.EditAction == GridViewEditAction.Cancel)
            {
                return;
            }
            if (e.EditOperationType == GridViewEditOperationType.Insert)
            {
                //Add the new entry to the data base.
            }
        }

        private void tlbApproveHoanThanh_Click(object sender, RoutedEventArgs e)
        {
            Cursor = Cursors.Wait;
            try
            {
                if (!ValidationHoanThanh())
                {
                    Cursor = Cursors.Arrow;
                    return;
                }
                else
                {
                    GetDataForm(BusinessConstant.TrangThaiBanGhi.SU_DUNG, BusinessConstant.TrangThaiNghiepVu.DA_DUYET);
                    List<int> lstID = new List<int>();
                    TaiSanProcess process = new TaiSanProcess();
                    List<ClientResponseDetail> lstResponseDetail = new List<ClientResponseDetail>();
                    int iret = 0;
                    process.QuanLyTaiSan(DatabaseConstant.Function.TS_TANG, DatabaseConstant.Action.THUC_HIEN, lstID, ref objTSDto, ref lstResponseDetail);
                    AfterSave(lstResponseDetail, iret);
                }
            }
            catch (System.Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally { Cursor = Cursors.Arrow; }
        }

        void AfterProcessNguyenGia(bool isSuccsess, List<ClientResponseDetail> lstResponseDetail)
        {
            if (isSuccsess)
                LMessage.ShowMessage("M.DungChung.LuuDuLieuThanhCong", LMessage.MessageBoxType.Information);
            else
                CommonFunction.ThongBaoKetQua(lstResponseDetail);
            if (OnSavingCompleted != null)
                OnSavingCompleted(null, EventArgs.Empty);
            SetDataForm();
            Cursor = Cursors.Arrow;
        }
        #endregion
    }
}
