using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
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
using System.Data;
using Microsoft.Windows.Controls.Ribbon;
using Telerik.Windows.Controls;
using Utilities.Common;
using Presentation.Process;
using Presentation.Process.Common;
using Presentation.Process.PopupServiceRef;
using Presentation.Process.DanhMucServiceRef;
using PresentationWPF.CustomControl;
using PresentationWPF.BaoCao.DungChung;
using Presentation.Process.BaoCaoServiceRef;

namespace PresentationWPF.DanhMuc.Nhom
{
    /// <summary>
    /// Interaction logic for ucNhomCT_02.xaml
    /// </summary>
    public partial class ucNhomCT_02 : UserControl
    {
        #region Khai bao
        // các thao tác trên Form: thêm, sửa, xem... (để mặc định là thêm)
        private DatabaseConstant.Action action = DatabaseConstant.Action.THEM;
        public DatabaseConstant.Action Action
        {
            get { return action; }
            set { action = value; }
        }

        private DatabaseConstant.Function function = DatabaseConstant.Function.DC_DM_NHOM;

        public event EventHandler OnSavingCompleted;

        private int id;
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        private DM_NHOM_DTO obj;
        public DM_NHOM_DTO Obj
        {
            get { return obj; }
            set { obj = value; }
        }

        private string sTrangThaiNVu = "";
        
        List<KH_KHANG_NHOM> lstKHangNhom = new List<KH_KHANG_NHOM>();
        DataTable dtkhachHang;        

        List<AutoCompleteEntry> lstSourceChiNhanh = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourcePhongGiaoDich = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceKhuVuc = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceCum = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceNhomCha = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceLoaiNhom = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceCBQL = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceGioiTinh = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceDanToc = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceNgheNghiep = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceNganhNgheTHT = new List<AutoCompleteEntry>();


        List<DataRow> lstPopup = new List<DataRow>();
        public void LayDuLieuTuPopup(List<DataRow> lst)
        {
            lstPopup = lst;
        }

        public static RoutedCommand HoldCommand = new RoutedCommand();
        public static RoutedCommand SaveCommand = new RoutedCommand();
        public static RoutedCommand AddCommand = new RoutedCommand();
        public static RoutedCommand ModifyCommand = new RoutedCommand();
        public static RoutedCommand DeleteCommand = new RoutedCommand();
        public static RoutedCommand ApproveCommand = new RoutedCommand();
        public static RoutedCommand RefuseCommand = new RoutedCommand();
        public static RoutedCommand CancelCommand = new RoutedCommand();
        public static RoutedCommand PreviewCommand = new RoutedCommand();
        public static RoutedCommand HelpCommand = new RoutedCommand();
        public static RoutedCommand CloseCommand = new RoutedCommand();
        #endregion

        #region Khoi tao
        public ucNhomCT_02()
        {
            InitializeComponent();

            DuyetQuyenTinhNang();

            BindShortkey();

            LoadCombobox();

            KhoiTaoDataSource();

            InitEventHandler();

            ShowControl();

        }

        private void ShowControl()
        {
            HeThong hethong = new HeThong();
            ArrayList arr = new ArrayList();
            arr = hethong.SetVisibleControl("PresentationWPF.DanhMuc.Nhom.ucNhomCT_02", "");
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
            //stpLoaiNhom.Visibility = Visibility.Collapsed;
            //cmbLoaiNhom.Visibility = Visibility.Collapsed;
        }

        private void DuyetQuyenTinhNang()
        {
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.DanhMuc;component/Nhom/ucNhomCT_02.xaml", ref Toolbar, ref mnuMain);
            foreach (var item in mnuMain.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += btnShortcutKey_Click;
            }
        }

        private void InitEventHandler()
        {
            chkThemNhieuLan.Checked += new RoutedEventHandler(chkThemNhieuLan_Checked);

            cmbChiNhanh.SelectionChanged += new SelectionChangedEventHandler(cmbChiNhanh_SelectionChanged);
            cmbPhongGiaoDich.SelectionChanged += new SelectionChangedEventHandler(cmbPhongGiaoDich_SelectionChanged);
            cmbKhuVuc.SelectionChanged += new SelectionChangedEventHandler(cmbKhuVuc_SelectionChanged);
            cmbCum.SelectionChanged += new SelectionChangedEventHandler(cmbCum_SelectionChanged);
            cmbLoaiNhom.SelectionChanged += new SelectionChangedEventHandler(cmbLoaiNhom_SelectionChanged);

            tlbAddKHang.Click += new RoutedEventHandler(tlbAddKHang_Click);
            tlbDelKHang.Click += new RoutedEventHandler(tlbDelKHang_Click);
        }

        private void LoadCombobox()
        {

            AutoComboBox auto = new AutoComboBox();            
            COMBOBOX_DTO combo = null;
            List<COMBOBOX_DTO> lstCombobox = new List<COMBOBOX_DTO>();
            List<string> lstDieuKien = new List<string>();

            //Chi nhánh
            auto.GenAutoComboBox(ref lstSourceChiNhanh, ref cmbChiNhanh, DatabaseConstant.DanhSachTruyVan.COMBOBOX_CNHANH.getValue(), null, ClientInformation.MaDonVi);

            if (cmbChiNhanh.SelectedIndex >= 0)
            {
                string maChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.First();
                string idChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.ElementAt(1);
                cmbPhongGiaoDich.Items.Clear();
                lstDieuKien.Clear();
                lstSourcePhongGiaoDich.Clear();
                lstDieuKien.Add(maChiNhanh);
                auto.GenAutoComboBox(ref lstSourcePhongGiaoDich, ref cmbPhongGiaoDich, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DONVITHEOMACHA.getValue(), lstDieuKien, ClientInformation.MaDonViGiaoDich);
            }

            if (cmbPhongGiaoDich.SelectedIndex >= 0)
            {
                string maPhongGD = lstSourcePhongGiaoDich.ElementAt(cmbPhongGiaoDich.SelectedIndex).KeywordStrings.First();
                string idPhongGD = lstSourcePhongGiaoDich.ElementAt(cmbPhongGiaoDich.SelectedIndex).KeywordStrings.ElementAt(1);

                cmbKhuVuc.Items.Clear();
                lstDieuKien.Clear();
                lstSourceKhuVuc.Clear();
                lstDieuKien.Add(idPhongGD);
                auto.GenAutoComboBox(ref lstSourceKhuVuc, ref cmbKhuVuc, DatabaseConstant.DanhSachTruyVan.COMBOBOX_KHUVUC.getValue(), lstDieuKien);

                cmbCBQL.Items.Clear();
                lstDieuKien.Clear();
                lstSourceCBQL.Clear();
                lstDieuKien.Add(maPhongGD);
                auto.GenAutoComboBox(ref lstSourceCBQL, ref cmbCBQL, DatabaseConstant.DanhSachTruyVan.COMBOBOX_NSD.getValue(), lstDieuKien);
            }

            if (cmbKhuVuc.SelectedIndex >= 0)
            {
                lstDieuKien.Clear();
                string idPhongGD = lstSourcePhongGiaoDich.ElementAt(cmbPhongGiaoDich.SelectedIndex).KeywordStrings.ElementAt(1);
                string idKhuVuc = lstSourceKhuVuc.ElementAt(cmbKhuVuc.SelectedIndex).KeywordStrings.ElementAt(1);
                lstDieuKien.Add(idPhongGD);
                lstDieuKien.Add(idKhuVuc);
                auto.GenAutoComboBox(ref lstSourceCum, ref cmbCum, DatabaseConstant.DanhSachTruyVan.COMBOBOX_CUM.getValue(), lstDieuKien);                
            }
            
            //Loại nhóm
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.LOAI_NHOM.getValue());
            combo = new COMBOBOX_DTO();
            combo.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();
            combo.combobox = cmbLoaiNhom;
            combo.lstSource = lstSourceLoaiNhom;
            combo.lstDieuKien = lstDieuKien;
            lstCombobox.Add(combo);


            //Giới tính
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.GIOI_TINH.getValue());
            combo = new COMBOBOX_DTO();
            combo.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();
            combo.combobox = cmbGioiTinh;
            combo.lstSource = lstSourceGioiTinh;
            combo.lstDieuKien = lstDieuKien;
            lstCombobox.Add(combo);

            //Dân tộc
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.DAN_TOC.getValue());
            combo = new COMBOBOX_DTO();
            combo.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();
            combo.combobox = cmbDanToc;
            combo.lstSource = lstSourceDanToc;
            combo.lstDieuKien = lstDieuKien;
            lstCombobox.Add(combo);

            //Nghề nghiệp
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.NGHE_NGHIEP.getValue());
            combo = new COMBOBOX_DTO();
            combo.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();
            combo.combobox = cmbNgheNghiep;
            combo.lstSource = lstSourceNgheNghiep;
            combo.lstDieuKien = lstDieuKien;
            lstCombobox.Add(combo);

            //Ngành nghề THT
            lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.NGHE_NGHIEP.getValue());
            combo = new COMBOBOX_DTO();
            combo.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();
            combo.combobox = cmbNganhNgheTHT;
            combo.lstSource = lstSourceNganhNgheTHT;
            combo.lstDieuKien = lstDieuKien;
            lstCombobox.Add(combo);

            auto.GenAutoComboBoxTheoList(ref lstCombobox);

            cmbLoaiNhom.SelectedIndex = 0;
            cmbGioiTinh.SelectedIndex = 0;
            cmbDanToc.SelectedIndex = 0;
            cmbNgheNghiep.SelectedIndex = 0;
            cmbNganhNgheTHT.SelectedIndex = 0;

        }

        private void KhoiTaoDataSource()
        {
            dtkhachHang = new DataTable();
            dtkhachHang.Columns.Add("ID", typeof(int));
            dtkhachHang.Columns.Add("ID_KHANG", typeof(int));
            dtkhachHang.Columns.Add("MA_KHANG", typeof(string));
            dtkhachHang.Columns.Add("TEN_KHANG", typeof(string));
            dtkhachHang.Columns.Add("DD_GTLQ_SO", typeof(string));
            dtkhachHang.Columns.Add("TRUONG_NHOM", typeof(string));
            dtkhachHang.Columns.Add("MA_NHOM", typeof(string));
            dtkhachHang.Columns.Add("TTHAI_NVU", typeof(string));
            dtkhachHang.Columns.Add("TTHAI_BGHI", typeof(string));
            raddgrKhachHangTVien.ItemsSource = dtkhachHang.DefaultView;
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
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.S, ModifierKeys.Control);
                        key = new KeyBinding(SaveCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THEM)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.N, ModifierKeys.Control);
                        key = new KeyBinding(AddCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.M, ModifierKeys.Control);
                        key = new KeyBinding(ModifyCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.D, ModifierKeys.Control);
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
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XEM_TRUOC)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.W, ModifierKeys.Control | ModifierKeys.Shift);
                        key = new KeyBinding(PreviewCommand, keyg);
                        key.Gesture = keyg;
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

        }

        private void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OnSave();
        }

        private void AddCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void AddCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            BeforeAddNew();
        }

        private void ModifyCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void ModifyCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            BeforeModifyFromDetail();
        }

        private void PreviewCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void PreviewCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Xem trước dữ liệu");
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

        }

        private void RefuseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void RefuseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void CancelCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void CancelCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {

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

            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU)))
            {
                OnSave();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THEM)))
            {
                BeforeAddNew();
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

            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
            {

            }
            else if (strTinhNang.Equals("PreviewQuyetDinh"))
            {
                OnPreviewQuyetDinh();
            }
            else if (strTinhNang.Equals("PreviewHopDong"))
            {
                OnPreviewHopDong();
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

            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU)))
            {
                OnSave();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THEM)))
            {
                BeforeAddNew();
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

            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET)))
            {

            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
            {
            }
            else if (strTinhNang.Equals("PreviewQuyetDinh"))
            {
                OnPreviewQuyetDinh();
            }
            else if (strTinhNang.Equals("PreviewHopDong"))
            {
                OnPreviewHopDong();
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
            listLockId.Add(id);

            bool ret = process.UnlockData(DatabaseConstant.Module.DMDC,
                DatabaseConstant.Function.DC_DM_NHOM,
                DatabaseConstant.Table.DM_NHOM,
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
            CustomControl.CommonFunction.CloseUserControl(this);
        }

        /// <summary>
        /// Sự kiện khi thay đổi giá trị của combobox cmbDonVi
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbChiNhanh_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbChiNhanh.SelectedIndex >= 0)
            {
                AutoComboBox auto = new AutoComboBox();
                List<string> lstDieuKien = new List<string>();

                string maChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.First();
                string idChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.ElementAt(1);
                cmbPhongGiaoDich.Items.Clear();
                lstDieuKien.Clear();
                lstSourcePhongGiaoDich.Clear();
                lstDieuKien.Add(maChiNhanh);
                auto.GenAutoComboBox(ref lstSourcePhongGiaoDich, ref cmbPhongGiaoDich, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DONVITHEOMACHA.getValue(), lstDieuKien);
            }
            else
            {
                cmbPhongGiaoDich.Items.Clear();
                lstSourcePhongGiaoDich.Clear();

                cmbCBQL.Items.Clear();
                lstSourceCBQL.Clear();
            }
        }

        private void cmbPhongGiaoDich_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbPhongGiaoDich.SelectedIndex >= 0)
            {
                AutoComboBox auto = new AutoComboBox();
                List<string> lstDieuKien = new List<string>();

                string maPhongGD = lstSourcePhongGiaoDich.ElementAt(cmbPhongGiaoDich.SelectedIndex).KeywordStrings.First();
                string idPhongGD = lstSourcePhongGiaoDich.ElementAt(cmbPhongGiaoDich.SelectedIndex).KeywordStrings.ElementAt(1);

                cmbKhuVuc.Items.Clear();
                lstDieuKien.Clear();
                lstSourceKhuVuc.Clear();
                lstDieuKien.Add(idPhongGD);
                auto.GenAutoComboBox(ref lstSourceKhuVuc, ref cmbKhuVuc, DatabaseConstant.DanhSachTruyVan.COMBOBOX_KHUVUC.getValue(), lstDieuKien);

                cmbCBQL.Items.Clear();
                lstDieuKien.Clear();
                lstSourceCBQL.Clear();
                lstDieuKien.Add(maPhongGD);
                auto.GenAutoComboBox(ref lstSourceCBQL, ref cmbCBQL, DatabaseConstant.DanhSachTruyVan.COMBOBOX_NSD.getValue(), lstDieuKien);
            }
            else
            {
                cmbKhuVuc.Items.Clear();
                lstSourceKhuVuc.Clear();

                cmbCBQL.Items.Clear();
                lstSourceCBQL.Clear();
            }
        }

        private void cmbKhuVuc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbKhuVuc.SelectedIndex >= 0)
            {
                AutoComboBox auto = new AutoComboBox();
                List<string> lstDieuKien = new List<string>();

                string idPhongGD = lstSourcePhongGiaoDich.ElementAt(cmbPhongGiaoDich.SelectedIndex).KeywordStrings.ElementAt(1);
                string idKhuVuc = lstSourceKhuVuc.ElementAt(cmbKhuVuc.SelectedIndex).KeywordStrings.ElementAt(1);
                lstDieuKien.Add(idPhongGD);
                lstDieuKien.Add(idKhuVuc);
                lstSourceCum.Clear();
                cmbCum.Items.Clear();
                auto.GenAutoComboBox(ref lstSourceCum, ref cmbCum, DatabaseConstant.DanhSachTruyVan.COMBOBOX_CUM.getValue(), lstDieuKien);
            }
            else
            {
                cmbCBQL.Items.Clear();
                lstSourceCBQL.Clear();
            }
        }

        private void cmbCum_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AutoComboBox au = new AutoComboBox();
            List<string> lstDK = new List<string>();
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                AutoCompleteEntry auPGD = au.getEntryByDisplayName(lstSourcePhongGiaoDich, ref cmbPhongGiaoDich);
                AutoCompleteEntry auKhuVuc = au.getEntryByDisplayName(lstSourceKhuVuc, ref cmbKhuVuc);
                AutoCompleteEntry auCum = au.getEntryByDisplayName(lstSourceCum, ref cmbCum);
                if (auPGD != null && auCum != null)
                {
                    //Set mặc định cán bộ quản lý nhóm bằng cán bộ quản lý cụm
                    if (cmbCum.SelectedIndex >= 0)
                    {
                        DM_CUM objCum = null;
                        int idCum = Convert.ToInt32(lstSourceCum.ElementAt(cmbCum.SelectedIndex).KeywordStrings[1]);
                        string responseMessage = "";
                        ApplicationConstant.ResponseStatus responseStatus = new DanhMucProcess().getCumById(idCum, ref objCum, ref responseMessage);
                        if (objCum != null)
                            cmbCBQL.SelectedIndex = lstSourceCBQL.IndexOf(lstSourceCBQL.FirstOrDefault(i => i.KeywordStrings.First().Equals(objCum.MA_CBO_QLY)));
                    }
                }

                dtkhachHang.Rows.Clear();
                raddgrKhachHangTVien.ItemsSource = dtkhachHang.DefaultView;
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiLoadDuLieu", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
                au = null;
            }
        }

        private void cmbLoaiNhom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dtkhachHang.Rows.Clear();
            raddgrKhachHangTVien.ItemsSource = dtkhachHang.DefaultView;
        }

        private void chkThemNhieuLan_Checked(object sender, RoutedEventArgs e)
        {
            //action = DatabaseConstant.Action.THEM;
            //ResetForm();
            //SetEnabledControls();
            //CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain, function);
            //txtTenNhanVien.Focus();

            //chkThemNhieuLan.IsChecked = false;
        }

        private void tlbDelKHang_Click(object sender, RoutedEventArgs e)
        {
            List<DataRow> lstRow = new List<DataRow>();
            foreach (DataRowView dr in raddgrKhachHangTVien.SelectedItems)
            {
                lstRow.Add(dr.Row);
            }

            foreach (DataRow dr in lstRow)
            {
                dtkhachHang.Rows.Remove(dr);
            }
            raddgrKhachHangTVien.ItemsSource = dtkhachHang.DefaultView;
        }

        private void tlbAddKHang_Click(object sender, RoutedEventArgs e)
        {
            string LoaiNhom = lstSourceLoaiNhom.ElementAt(cmbLoaiNhom.SelectedIndex).KeywordStrings.FirstOrDefault();
            if (LoaiNhom == "NHOM_TRA_DAN")
            {
                PopUpKhachHangNhomTraDan();
            }
            else if (LoaiNhom == "NHOM_THOI_VU")
            {
                PopUpKhachHangNhomThoiVu();
            }
            else
            {
                PopUpKhachHangNhomTraDan(); 
            }
        }

        private void PopUpKhachHangNhomTraDan()
        {
            try
            {
                ucPopup popup = null;
                if (ClientInformation.Company.Equals("QUANGBINH"))
                {
                    AutoCompleteEntry aceKhuVuc = new AutoComboBox().getEntryByDisplayName(lstSourceKhuVuc, ref cmbKhuVuc);
                    string idKhuVuc = "-1";
                    if (aceKhuVuc != null)
                    {
                        idKhuVuc = aceKhuVuc.KeywordStrings.ElementAt(1);
                    }
                    else
                    {
                        idKhuVuc = "-1";
                    }

                    Window window = new Window();
                    window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    window.RenderSize = new Size(1024, 768);
                    PopupProcess popupProcess = new PopupProcess();
                    List<string> lstDieuKien = new List<string>();
                    lstDieuKien.Add(ClientInformation.MaDonViGiaoDich);
                    lstDieuKien.Add(idKhuVuc);
                    popupProcess.getPopupInformation("POPUP_DS_KHANG_HANG_TRA_DAN_QB", lstDieuKien);
                    SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;
                    popup = new ucPopup(false, simplePopupResponse, true);
                    popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
                    window.Title = LLanguage.SearchResourceByKey(simplePopupResponse.PopupTitle);
                    window.Content = popup;
                    window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    window.ShowDialog();
                }
                else
                {
                    Window window = new Window();
                    window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    window.RenderSize = new Size(1024, 768);
                    PopupProcess popupProcess = new PopupProcess();
                    List<string> lstDieuKien = new List<string>();
                    lstDieuKien.Add(ClientInformation.MaDonViGiaoDich);
                    popupProcess.getPopupInformation("POPUP_DS_KHANG_HANG_TRA_DAN", lstDieuKien);
                    SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;
                    popup = new ucPopup(false, simplePopupResponse, true);
                    popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
                    window.Title = LLanguage.SearchResourceByKey(simplePopupResponse.PopupTitle);
                    window.Content = popup;
                    window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                    window.ShowDialog();
                }
                if (lstPopup.Count > 0)
                {
                    foreach (DataRow drv in lstPopup)
                    {
                        //DataSet dsSourceKHang = new KhachHangProcess().getThongTinKHTheoID(Convert.ToInt32(drv["ID"]));
                        if (KiemTraKhachHang(drv["MA_KHANG"].ToString()))
                        {
                            DataRow dr = dtkhachHang.NewRow();
                            dr["ID_KHANG"] = drv["ID"];
                            dr["MA_KHANG"] = drv["MA_KHANG"];
                            dr["TEN_KHANG"] = drv["TEN_KHANG"];
                            dr["DD_GTLQ_SO"] = drv["DD_GTLQ_SO"];
                            dr["TRUONG_NHOM"] = "false";
                            dr["TTHAI_NVU"] = drv["TTHAI_NVU"];
                            dr["TTHAI_BGHI"] = "T";
                            dtkhachHang.Rows.Add(dr);
                        }
                    }
                    raddgrKhachHangTVien.ItemsSource = dtkhachHang.DefaultView;
                }
                popup = null;

            }
            catch
            {
                LMessage.ShowMessage("M.DungChung.Result.KhongThanhCong", LMessage.MessageBoxType.Error);
            }
        }

        private void PopUpKhachHangNhomThoiVu()
        {
            try
            {
                lstPopup.Clear();
                string lstIDKHang = "";
                foreach (DataRow dr in dtkhachHang.Rows)
                {
                    lstIDKHang += "," + dr["ID_KHANG"].ToString();
                }
                if (lstIDKHang.Length > 0)
                    lstIDKHang = lstIDKHang.Substring(1);
                else
                    lstIDKHang = "(0)";

                AutoCompleteEntry aceCum = new AutoComboBox().getEntryByDisplayName(lstSourceCum, ref cmbCum);
                string idCum = "-1";
                if (aceCum != null)
                {
                    idCum = aceCum.KeywordStrings.ElementAt(1);
                }
                else
                {
                    idCum = "-1";
                }

                Window window = new Window();
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                window.RenderSize = new Size(1024, 768);
                PopupProcess popupProcess = new PopupProcess();
                List<string> lstDieuKien = new List<string>();
                lstDieuKien.Add(lstIDKHang);
                lstDieuKien.Add(idCum);
                popupProcess.getPopupInformation("POPUP_DS_KHANG_HANG_THOI_VU_02", lstDieuKien);
                SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;
                ucPopup popup = new ucPopup(false, simplePopupResponse, true);
                popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
                window.Title = LLanguage.SearchResourceByKey(simplePopupResponse.PopupTitle);
                window.Content = popup;
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                window.ShowDialog();
                if (lstPopup.Count > 0)
                {
                    foreach (DataRow drv in lstPopup)
                    {
                        //DataSet dsSourceKHang = new KhachHangProcess().getThongTinKHTheoID(Convert.ToInt32(drv["ID"]));
                        if (KiemTraKhachHang(drv["MA_KHANG"].ToString()))
                        {
                            DataRow dr = dtkhachHang.NewRow();
                            dr["ID_KHANG"] = drv["ID"];
                            dr["MA_KHANG"] = drv["MA_KHANG"];
                            dr["TEN_KHANG"] = drv["TEN_KHANG"];
                            dr["DD_GTLQ_SO"] = drv["DD_GTLQ_SO"];
                            dr["TRUONG_NHOM"] = "false";
                            dr["TTHAI_NVU"] = drv["TTHAI_NVU"];
                            dr["TTHAI_BGHI"] = "T";
                            dtkhachHang.Rows.Add(dr);
                        }
                    }
                    raddgrKhachHangTVien.ItemsSource = dtkhachHang.DefaultView;
                }
                popup = null;
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.Result.KhongThanhCong", LMessage.MessageBoxType.Error);
            }

        }

        private bool KiemTraKhachHang(string maKhachHang)
        {
            foreach (DataRow dr in dtkhachHang.Rows)
            {
                if (dr["MA_KHANG"].ToString().Equals(maKhachHang))
                {
                    return false;
                }
            }
            return true;
        }

        #endregion

        #region Xử lý nghiệp vụ
        private void GetFormData(ref DM_NHOM_DTO obj, string sTrangThaiNVu)
        {
            try
            {
                obj = new DM_NHOM_DTO();
                obj.ID = id;

                #region DM_NHOM
                DM_NHOM objNhom = new DM_NHOM();
                objNhom.ID = id;
                objNhom.ID_DVI = Convert.ToInt32(lstSourcePhongGiaoDich.ElementAt(cmbPhongGiaoDich.SelectedIndex).KeywordStrings.ElementAt(1));
                objNhom.ID_KVUC = Convert.ToInt32(lstSourceKhuVuc.ElementAt(cmbKhuVuc.SelectedIndex).KeywordStrings.ElementAt(1));
                objNhom.ID_CUM = Convert.ToInt32(lstSourceCum.ElementAt(cmbCum.SelectedIndex).KeywordStrings.ElementAt(1));
                objNhom.MA_DVI = lstSourcePhongGiaoDich.ElementAt(cmbPhongGiaoDich.SelectedIndex).KeywordStrings.First();
                objNhom.MA_KVUC = lstSourceKhuVuc.ElementAt(cmbKhuVuc.SelectedIndex).KeywordStrings.First();
                objNhom.MA_CUM = lstSourceCum.ElementAt(cmbCum.SelectedIndex).KeywordStrings.First();
                objNhom.MA_NHOM = txtMaNhom.Text;
                objNhom.MA_LOAI_NHOM = lstSourceLoaiNhom.ElementAt(cmbLoaiNhom.SelectedIndex).KeywordStrings.First();
                if (raddtNgayThanhLap.Value != null)
                    objNhom.NGAY_TLAP = Convert.ToDateTime(raddtNgayThanhLap.Value).ToString("yyyyMMdd");
                if (raddtNgayHetHL.Value != null)
                    objNhom.NGAY_HET_HLUC = Convert.ToDateTime(raddtNgayHetHL.Value).ToString("yyyyMMdd");
                objNhom.TEN_NHOM = txtTenNhom.Text;
                objNhom.TEN_TAT = txtTenTat.Text;
                objNhom.ID_CBO_QLY = Convert.ToInt32(lstSourceCBQL.ElementAt(cmbCBQL.SelectedIndex).KeywordStrings.ElementAt(1));
                objNhom.MA_CBO_QLY = lstSourceCBQL.ElementAt(cmbCBQL.SelectedIndex).KeywordStrings.First();

                //Thông tin kiểm soát
                objNhom.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                objNhom.TTHAI_NVU = sTrangThaiNVu;
                objNhom.MA_DVI_QLY = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.First();
                objNhom.MA_DVI_TAO = objNhom.MA_DVI;
                objNhom.NGAY_NHAP = Convert.ToDateTime(raddtNgayLap.Value).ToString("yyyyMMdd");
                objNhom.NGUOI_NHAP = txtNguoiLap.Text;
                if (action != DatabaseConstant.Action.THEM)
                {
                    objNhom.NGAY_CNHAT = ClientInformation.NgayLamViecHienTai;
                    objNhom.NGUOI_CNHAT = ClientInformation.TenDangNhap;
                }                
                #endregion

                #region DM_NHOM_NGUOI_BLANH
                DM_NHOM_NGUOI_BLANH objNguoiBaoLanh = new DM_NHOM_NGUOI_BLANH();
                objNguoiBaoLanh.ID_NHOM = id;
                objNguoiBaoLanh.MA_NHOM = txtMaNhom.Text;
                objNguoiBaoLanh.HO_TEN = txtHoTen.Text;
                if (raddtNgaySinh.Value != null)
                    objNguoiBaoLanh.NGAY_SINH = Convert.ToDateTime(raddtNgaySinh.Value).ToString("yyyyMMdd");
                if(cmbGioiTinh.SelectedIndex>=0)
                    objNguoiBaoLanh.GIOI_TINH = lstSourceGioiTinh.ElementAt(cmbGioiTinh.SelectedIndex).KeywordStrings.First();
                if(cmbDanToc.SelectedIndex>=0)
                    objNguoiBaoLanh.DAN_TOC = lstSourceDanToc.ElementAt(cmbDanToc.SelectedIndex).KeywordStrings.First();
                objNguoiBaoLanh.SO_CMND = txtSoCMND.Text;
                if (raddtNgayCap.Value != null)
                    objNguoiBaoLanh.NGAY_CAP = Convert.ToDateTime(raddtNgayCap.Value).ToString("yyyyMMdd");
                objNguoiBaoLanh.NOI_CAP = txtNoiCap.Text;
                if(cmbNgheNghiep.SelectedIndex>=0)
                    objNguoiBaoLanh.NGHE_NGHIEP = lstSourceNgheNghiep.ElementAt(cmbNgheNghiep.SelectedIndex).KeywordStrings.First();
                objNguoiBaoLanh.SO_HO_KHAU = txtSoHoKhau.Text;
                objNguoiBaoLanh.SO_DIEN_THOAI = txtSoDienThoai.Text;
                objNguoiBaoLanh.DIA_CHI = txtDiaChi.Text;
                if(cmbNgheNghiep.SelectedIndex>=0)
                    objNguoiBaoLanh.NGANH_NGHE_THT = lstSourceNganhNgheTHT.ElementAt(cmbNganhNgheTHT.SelectedIndex).KeywordStrings.First();
                objNguoiBaoLanh.TONG_SO_TVIEN_THT = (int)numTongSoTV.Value;
                objNguoiBaoLanh.DIA_CHI_THT = txtDiaChiTHT.Text;
                objNguoiBaoLanh.MA_DVI_QLY = objNhom.MA_DVI_QLY;
                objNguoiBaoLanh.MA_DVI_TAO = objNhom.MA_DVI_TAO;
                objNguoiBaoLanh.NGAY_CNHAT = objNhom.NGAY_CNHAT;
                objNguoiBaoLanh.NGAY_NHAP = objNhom.NGAY_NHAP;
                objNguoiBaoLanh.NGUOI_CNHAT = objNhom.NGUOI_CNHAT;
                objNguoiBaoLanh.NGUOI_NHAP = objNhom.NGUOI_NHAP;
                objNguoiBaoLanh.TTHAI_BGHI = objNhom.TTHAI_BGHI;
                objNguoiBaoLanh.TTHAI_NVU = objNhom.TTHAI_NVU;
                #endregion

                #region KH_KHANG_NHOM
                lstKHangNhom = new List<KH_KHANG_NHOM>();
                foreach (DataRow dr in dtkhachHang.Rows)
                {
                    KH_KHANG_NHOM objKHNhom = new KH_KHANG_NHOM();
                    objKHNhom.ID_KHANG = Convert.ToInt32(dr["ID_KHANG"]);
                    objKHNhom.ID_NHOM = id;
                    objKHNhom.MA_DVI_QLY = objNhom.MA_DVI_QLY;
                    objKHNhom.MA_DVI_TAO = objNhom.MA_DVI_TAO;
                    objKHNhom.MA_KHANG = dr["MA_KHANG"].ToString();
                    objKHNhom.MA_LOAI_NHOM = objNhom.MA_LOAI_NHOM;
                    objKHNhom.MA_NHOM = objNhom.MA_NHOM;
                    objKHNhom.NGAY_CNHAT = objNhom.NGAY_CNHAT;
                    objKHNhom.NGAY_NHAP = objNhom.NGAY_NHAP;
                    objKHNhom.NGUOI_CNHAT = objNhom.NGUOI_CNHAT;
                    objKHNhom.NGUOI_NHAP = objNhom.NGUOI_NHAP;
                    objKHNhom.TTHAI_BGHI = objNhom.TTHAI_BGHI;
                    objKHNhom.TTHAI_NVU = objNhom.TTHAI_NVU;
                    lstKHangNhom.Add(objKHNhom);

                    if (dr["TRUONG_NHOM"].ToString().ToLower() == "true")
                    {
                        objNhom.ID_NHOM_TRUONG = Convert.ToInt32(dr["ID_KHANG"]);
                        objNhom.MA_NHOM_TRUONG = dr["MA_KHANG"].ToString();
                    }
                }
                #endregion

                obj.OBJ_NHOM = objNhom;
                obj.OBJ_NHOM_NGUOI_BLANH = objNguoiBaoLanh;
                obj.LST_KH_NHOM = lstKHangNhom.ToArray();
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        private void SetFormData()
        {
            DanhMucProcess processDanhMuc = new DanhMucProcess();

            List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
            obj = new DM_NHOM_DTO();
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                bool ret = false;
                obj.ID = id;                
                ret = processDanhMuc.Nhom02(DatabaseConstant.Action.LOAD, ref obj, ref listClientResponseDetail);
                if (ret == true)
                {
                    sTrangThaiNVu = obj.OBJ_NHOM.TTHAI_NVU;
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);

                    #region Thông tin chung
                    cmbChiNhanh.SelectedIndex = lstSourceChiNhanh.IndexOf(lstSourceChiNhanh.FirstOrDefault(e => e.KeywordStrings.First().Equals(obj.OBJ_NHOM.MA_DVI_QLY)));
                    cmbPhongGiaoDich.SelectedIndex = lstSourcePhongGiaoDich.IndexOf(lstSourcePhongGiaoDich.FirstOrDefault(e => e.KeywordStrings.First().Equals(obj.OBJ_NHOM.MA_DVI_TAO)));
                    cmbKhuVuc.SelectedIndex = lstSourceKhuVuc.IndexOf(lstSourceKhuVuc.FirstOrDefault(e => e.KeywordStrings.First().Equals(obj.OBJ_NHOM.MA_KVUC)));
                    cmbCum.SelectedIndex = lstSourceCum.IndexOf(lstSourceCum.FirstOrDefault(e => e.KeywordStrings.First().Equals(obj.OBJ_NHOM.MA_CUM)));
                    cmbLoaiNhom.SelectedIndex = lstSourceLoaiNhom.IndexOf(lstSourceLoaiNhom.FirstOrDefault(e => e.KeywordStrings.First().Equals(obj.OBJ_NHOM.MA_LOAI_NHOM)));                    
                    txtMaNhom.Text = obj.OBJ_NHOM.MA_NHOM;
                    txtTenNhom.Text = obj.OBJ_NHOM.TEN_NHOM;
                    txtTenTat.Text = obj.OBJ_NHOM.TEN_TAT;
                    if (LDateTime.IsDate(obj.OBJ_NHOM.NGAY_TLAP, "yyyyMMdd"))
                        raddtNgayThanhLap.Value = LDateTime.StringToDate(obj.OBJ_NHOM.NGAY_TLAP, "yyyyMMdd");
                    if (LDateTime.IsDate(obj.OBJ_NHOM.NGAY_HET_HLUC, "yyyyMMdd"))
                        raddtNgayHetHL.Value = LDateTime.StringToDate(obj.OBJ_NHOM.NGAY_HET_HLUC, "yyyyMMdd");
                    cmbCBQL.SelectedIndex = lstSourceCBQL.IndexOf(lstSourceCBQL.FirstOrDefault(e => e.KeywordStrings.First().Equals(obj.OBJ_NHOM.MA_CBO_QLY)));
                    #endregion

                    #region Thông tin người bảo lãnh nhóm
                    if(obj.OBJ_NHOM_NGUOI_BLANH != null)
                    {
                        txtHoTen.Text = obj.OBJ_NHOM_NGUOI_BLANH.HO_TEN;
                        if (LDateTime.IsDate(obj.OBJ_NHOM_NGUOI_BLANH.NGAY_SINH, "yyyyMMdd"))
                            raddtNgaySinh.Value = LDateTime.StringToDate(obj.OBJ_NHOM_NGUOI_BLANH.NGAY_SINH, "yyyyMMdd");
                        cmbGioiTinh.SelectedIndex = lstSourceGioiTinh.IndexOf(lstSourceGioiTinh.FirstOrDefault(e => e.KeywordStrings.First().Equals(obj.OBJ_NHOM_NGUOI_BLANH.GIOI_TINH)));
                        cmbDanToc.SelectedIndex = lstSourceDanToc.IndexOf(lstSourceDanToc.FirstOrDefault(e => e.KeywordStrings.First().Equals(obj.OBJ_NHOM_NGUOI_BLANH.DAN_TOC)));
                        txtSoCMND.Text = obj.OBJ_NHOM_NGUOI_BLANH.SO_CMND;                        
                        if (LDateTime.IsDate(obj.OBJ_NHOM_NGUOI_BLANH.NGAY_CAP, "yyyyMMdd"))
                            raddtNgayCap.Value = LDateTime.StringToDate(obj.OBJ_NHOM_NGUOI_BLANH.NGAY_CAP, "yyyyMMdd");
                        txtNoiCap.Text = obj.OBJ_NHOM_NGUOI_BLANH.NOI_CAP;
                        cmbNgheNghiep.SelectedIndex = lstSourceNgheNghiep.IndexOf(lstSourceNgheNghiep.FirstOrDefault(e => e.KeywordStrings.First().Equals(obj.OBJ_NHOM_NGUOI_BLANH.NGHE_NGHIEP)));
                        txtSoHoKhau.Text = obj.OBJ_NHOM_NGUOI_BLANH.SO_HO_KHAU;
                        txtSoDienThoai.Text = obj.OBJ_NHOM_NGUOI_BLANH.SO_DIEN_THOAI;
                        txtDiaChi.Text = obj.OBJ_NHOM_NGUOI_BLANH.DIA_CHI;
                        cmbNganhNgheTHT.SelectedIndex = lstSourceNganhNgheTHT.IndexOf(lstSourceNganhNgheTHT.FirstOrDefault(e => e.KeywordStrings.First().Equals(obj.OBJ_NHOM_NGUOI_BLANH.NGANH_NGHE_THT)));
                        numTongSoTV.Value = obj.OBJ_NHOM_NGUOI_BLANH.TONG_SO_TVIEN_THT;
                        txtDiaChiTHT.Text = obj.OBJ_NHOM_NGUOI_BLANH.DIA_CHI_THT;
                    }
                    #endregion

                    if (obj.DATA_SET != null && obj.DATA_SET.Tables.Count > 0)
                    {
                        dtkhachHang = obj.DATA_SET.Tables[0];
                        raddgrKhachHangTVien.ItemsSource = dtkhachHang.DefaultView;
                    }

                    #region Thông tin kiểm soát
                    txtTrangThaiBanGhi.Text = BusinessConstant.layNgonNguSuDung(obj.OBJ_NHOM.TTHAI_BGHI);
                    raddtNgayLap.Value = LDateTime.StringToDate(obj.OBJ_NHOM.NGAY_NHAP, "yyyyMMdd");
                    txtNguoiLap.Text = obj.OBJ_NHOM.NGUOI_NHAP;
                    if (LDateTime.IsDate(obj.OBJ_NHOM.NGAY_CNHAT, "yyyyMMdd") == true)
                        raddtNgayCapNhat.Value = LDateTime.StringToDate(obj.OBJ_NHOM.NGAY_CNHAT, "yyyyMMdd");
                    txtNguoiCapNhat.Text = obj.OBJ_NHOM.NGUOI_CNHAT;
                    #endregion

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
            id = 0;            
            obj = null;
            lstKHangNhom = new List<KH_KHANG_NHOM>();
            dtkhachHang.Rows.Clear();

            #region Thông tin chung
            txtMaNhom.Text = "";
            txtTenNhom.Text = "";
            txtTenTat.Text = "";
            raddtNgayThanhLap.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
            raddtNgayHetHL.Value = null;            
            #endregion

            raddgrKhachHangTVien.ItemsSource = dtkhachHang.DefaultView;

            #region Thông tin người bảo lãnh
            txtHoTen.Text = "";
            raddtNgaySinh.Value = null;
            cmbGioiTinh.SelectedIndex = 0;
            cmbDanToc.SelectedIndex = 0;
            txtSoCMND.Text = "";
            raddtNgayCap.Value = null;
            txtNoiCap.Text = "";
            cmbNgheNghiep.SelectedIndex = 0;
            txtSoHoKhau.Text = "";
            txtSoDienThoai.Text = "";
            txtDiaChi.Text = "";
            cmbNganhNgheTHT.SelectedIndex = 0;
            numTongSoTV.Value = 0;
            txtDiaChiTHT.Text = "";
            #endregion

            #region Thông tin kiểm soát
            txtTrangThaiBanGhi.Text = "";
            raddtNgayLap.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
            txtNguoiLap.Text = ClientInformation.TenDangNhap;
            raddtNgayCapNhat.Value = null;
            txtNguoiCapNhat.Text = "";
            #endregion

        }

        private bool Validation()
        {
            try
            {
                if (cmbChiNhanh.SelectedIndex < 0)
                {
                    CommonFunction.ThongBaoChuaChon(lblChiNhanh.Content.ToString());
                    cmbChiNhanh.Focus();
                    return false;
                }

                if (cmbPhongGiaoDich.SelectedIndex < 0)
                {
                    CommonFunction.ThongBaoChuaChon(lblPhongGiaoDich.Content.ToString());
                    cmbPhongGiaoDich.Focus();
                    return false;
                }

                if (txtTenNhom.Text.IsNullOrEmptyOrSpace())
                {
                    CommonFunction.ThongBaoChuaChon(lblTenNhom.Content.ToString());
                    txtTenNhom.Focus();
                    return false;
                }

                if (txtTenTat.Text.IsNullOrEmptyOrSpace())
                {
                    CommonFunction.ThongBaoChuaChon(lblTenTat.Content.ToString());
                    txtTenTat.Focus();
                    return false;
                }

                if (cmbCBQL.SelectedIndex < 0)
                {
                    CommonFunction.ThongBaoChuaChon(lblCBQL.Content.ToString());
                    cmbCBQL.Focus();
                    return false;
                }

                string maLoaiNhom = lstSourceLoaiNhom.ElementAt(cmbLoaiNhom.SelectedIndex).KeywordStrings.FirstOrDefault();
                int soLuong = raddgrKhachHangTVien.Items.Count;
                if (maLoaiNhom.Equals("NHOM_THOI_VU"))
                {
                    string toithieu = new UtilitiesProcess().LayGiaTriThamSo(BusinessConstant.LoaiThamSo.TW, BusinessConstant.MaThamSo.TW_KHANG_TVIEN_SNGUOI_TTHIEU_NHOM_MVU, ClientInformation.MaDonVi);
                    string toiDa = new UtilitiesProcess().LayGiaTriThamSo(BusinessConstant.LoaiThamSo.TW, BusinessConstant.MaThamSo.TW_KHANG_TVIEN_SNGUOI_TDA_NHOM_MVU, ClientInformation.MaDonVi);

                    int min = 0;
                    int max = 100;
                    if (toithieu.IsNumeric())
                        min = Convert.ToInt32(toithieu);

                    if (toiDa.IsNumeric())
                        max = Convert.ToInt32(toiDa);

                    if (soLuong < min)
                    {
                        MessageBoxResult result = LMessage.ShowMessage("Số khách hàng nhỏ hơn số lượng người tối thiểu trong nhóm. \nTiếp tục hay không?", LMessage.MessageBoxType.Question);
                        if (result == MessageBoxResult.No || result == MessageBoxResult.Cancel)
                        {
                            tlbAddKHang.Focus();
                            return false;
                        }

                    }

                    if (soLuong > max)
                    {
                        MessageBoxResult result = LMessage.ShowMessage("Số khách hàng lớn hơn số lượng người tối đa trong nhóm. \nTiếp tục hay không?", LMessage.MessageBoxType.Question);
                        if (result == MessageBoxResult.No || result == MessageBoxResult.Cancel)
                        {
                            tlbDelKHang.Focus();
                            return false;
                        }

                    }

                }
                else if (maLoaiNhom.Equals("NHOM_TRA_DAN"))
                {
                    string toithieu = new UtilitiesProcess().LayGiaTriThamSo(BusinessConstant.LoaiThamSo.TW, BusinessConstant.MaThamSo.TW_KHANG_TVIEN_SNGUOI_TTHIEU_NHOM_TDAN, ClientInformation.MaDonVi);
                    string toiDa = new UtilitiesProcess().LayGiaTriThamSo(BusinessConstant.LoaiThamSo.TW, BusinessConstant.MaThamSo.TW_KHANG_TVIEN_SNGUOI_TDA_NHOM_TDAN, ClientInformation.MaDonVi);

                    int min = 0;
                    int max = 100;
                    if (toithieu.IsNumeric())
                        min = Convert.ToInt32(toithieu);

                    if (toiDa.IsNumeric())
                        max = Convert.ToInt32(toiDa);

                    if (soLuong < min)
                    {
                        MessageBoxResult result = LMessage.ShowMessage("Số khách hàng nhỏ hơn số lượng người tối thiểu trong nhóm. \nTiếp tục hay không?", LMessage.MessageBoxType.Question);
                        if (result == MessageBoxResult.No || result == MessageBoxResult.Cancel)
                        {
                            tlbAddKHang.Focus();
                            return false;
                        }

                    }

                    if (soLuong > max)
                    {
                        MessageBoxResult result = LMessage.ShowMessage("Số khách hàng lớn hơn số lượng người tối đa trong nhóm. \nTiếp tục hay không?", LMessage.MessageBoxType.Question);
                        if (result == MessageBoxResult.No || result == MessageBoxResult.Cancel)
                        {
                            tlbDelKHang.Focus();
                            return false;
                        }

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
                cmbChiNhanh.IsEnabled = false;
                cmbPhongGiaoDich.IsEnabled = false;
                cmbKhuVuc.IsEnabled = true;
                cmbCum.IsEnabled = true;
                cmbLoaiNhom.IsEnabled = true;                
                txtMaNhom.IsEnabled = false;
                txtTenNhom.IsEnabled = true;
                txtTenTat.IsEnabled = true;
                raddtNgayThanhLap.IsEnabled = true;
                dtpNgayThanhLap.IsEnabled = true;
                raddtNgayHetHL.IsEnabled = true;
                dtpNgayHetHL.IsEnabled = true;
                cmbCBQL.IsEnabled = true;
                tlbAddKHang.IsEnabled = true;
                tlbDelKHang.IsEnabled = true;

                txtHoTen.IsEnabled = true;
                raddtNgaySinh.IsEnabled = true;
                dtpNgaySinh.IsEnabled = true;
                cmbGioiTinh.IsEnabled = true;
                cmbDanToc.IsEnabled = true;
                txtSoCMND.IsEnabled = true;
                raddtNgayCap.IsEnabled = true;
                dtpNgayCap.IsEnabled = true;
                txtNoiCap.IsEnabled = true;
                cmbNgheNghiep.IsEnabled = true;
                txtSoHoKhau.IsEnabled = true;
                txtSoDienThoai.IsEnabled = true;
                txtDiaChi.IsEnabled = true;
                cmbNganhNgheTHT.IsEnabled = true;
                numTongSoTV.IsEnabled = true;
                txtDiaChiTHT.IsEnabled = true;


            }
            #endregion

            #region Sửa
            else if (action == DatabaseConstant.Action.SUA)
            {
                cmbChiNhanh.IsEnabled = false;
                cmbPhongGiaoDich.IsEnabled = false;
                cmbKhuVuc.IsEnabled = true;
                cmbCum.IsEnabled = true;
                cmbLoaiNhom.IsEnabled = true;                
                txtMaNhom.IsEnabled = false;
                txtTenNhom.IsEnabled = true;
                txtTenTat.IsEnabled = true;
                raddtNgayThanhLap.IsEnabled = true;
                dtpNgayThanhLap.IsEnabled = true;
                raddtNgayHetHL.IsEnabled = true;
                dtpNgayHetHL.IsEnabled = true;
                cmbCBQL.IsEnabled = true;
                tlbAddKHang.IsEnabled = true;
                tlbDelKHang.IsEnabled = true;

                txtHoTen.IsEnabled = true;
                raddtNgaySinh.IsEnabled = true;
                dtpNgaySinh.IsEnabled = true;
                cmbGioiTinh.IsEnabled = true;
                cmbDanToc.IsEnabled = true;
                txtSoCMND.IsEnabled = true;
                raddtNgayCap.IsEnabled = true;
                dtpNgayCap.IsEnabled = true;
                txtNoiCap.IsEnabled = true;
                cmbNgheNghiep.IsEnabled = true;
                txtSoHoKhau.IsEnabled = true;
                txtSoDienThoai.IsEnabled = true;
                txtDiaChi.IsEnabled = true;
                cmbNganhNgheTHT.IsEnabled = true;
                numTongSoTV.IsEnabled = true;
                txtDiaChiTHT.IsEnabled = true;

            }
            #endregion

            #region Xem ~ Mặc định
            else
            {
                cmbChiNhanh.IsEnabled = false;
                cmbPhongGiaoDich.IsEnabled = false;
                cmbKhuVuc.IsEnabled = false;
                cmbCum.IsEnabled = false;
                cmbLoaiNhom.IsEnabled = false;                
                txtMaNhom.IsEnabled = false;
                txtTenNhom.IsEnabled = false;
                txtTenTat.IsEnabled = false;
                raddtNgayThanhLap.IsEnabled = false;
                dtpNgayThanhLap.IsEnabled = false;
                raddtNgayHetHL.IsEnabled = false;
                dtpNgayHetHL.IsEnabled = false;
                cmbCBQL.IsEnabled = false;
                tlbAddKHang.IsEnabled = false;
                tlbDelKHang.IsEnabled = false;

                txtHoTen.IsEnabled = false;
                raddtNgaySinh.IsEnabled = false;
                dtpNgaySinh.IsEnabled = false;
                cmbGioiTinh.IsEnabled = false;
                cmbDanToc.IsEnabled = false;
                txtSoCMND.IsEnabled = false;
                raddtNgayCap.IsEnabled = false;
                dtpNgayCap.IsEnabled = false;
                txtNoiCap.IsEnabled = false;
                cmbNgheNghiep.IsEnabled = false;
                txtSoHoKhau.IsEnabled = false;
                txtSoDienThoai.IsEnabled = false;
                txtDiaChi.IsEnabled = false;
                cmbNganhNgheTHT.IsEnabled = false;
                numTongSoTV.IsEnabled = false;
                txtDiaChiTHT.IsEnabled = false;
            }
            #endregion
        }

        private void SetVisibledControls()
        {
            string LoaiNhom = lstSourceLoaiNhom.ElementAt(cmbLoaiNhom.SelectedIndex).KeywordStrings.FirstOrDefault();
            if (LoaiNhom == "NHOM_THOI_VU")
            {
                //lblNhomCha.Visibility = System.Windows.Visibility.Visible;
                //cmbNhomCha.Visibility = System.Windows.Visibility.Visible;                
            }
            else
            {
                //lblNhomCha.Visibility = System.Windows.Visibility.Collapsed;
                //cmbNhomCha.Visibility = System.Windows.Visibility.Collapsed;
            }
        }


        public void OnSave()
        {
            try
            {
                if (!Validation()) return;

                string trangThai = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();

                obj = new DM_NHOM_DTO();

                GetFormData(ref obj, trangThai);

                if (action == DatabaseConstant.Action.THEM)
                {
                    OnAddNew(obj);
                }
                else if (action == DatabaseConstant.Action.SUA)
                {
                    OnModify(obj);
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
            SetEnabledControls();
            SetVisibledControls();
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
            SetEnabledControls();
            SetVisibledControls();
            CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain, function);
            txtTenNhom.Focus();
        }

        public void OnAddNew(DM_NHOM_DTO obj)
        {

            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                DanhMucProcess processDanhMuc = new DanhMucProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = false;
                
                ret = processDanhMuc.Nhom02(DatabaseConstant.Action.THEM, ref obj, ref listClientResponseDetail);
                AfterAddNew(ret, obj, listClientResponseDetail);
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

        public void AfterAddNew(bool ret, DM_NHOM_DTO obj, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (ret)
                {
                    LMessage.ShowMessage("M.DungChung.ThemThanhCong", LMessage.MessageBoxType.Information);

                    if (chkThemNhieuLan.IsChecked == true)
                    {
                        BeforeAddNew();
                    }
                    else
                    {
                        id = obj.ID;
                        txtMaNhom.Text = obj.OBJ_NHOM.MA_NHOM;

                        sTrangThaiNVu = obj.OBJ_NHOM.TTHAI_NVU;
                        lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);
                        txtTrangThaiBanGhi.Text = BusinessConstant.layNgonNguSuDung(obj.OBJ_NHOM.TTHAI_BGHI);

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
                listLockId.Add(id);

                bool ret = process.LockData(DatabaseConstant.Module.DMDC,
                    DatabaseConstant.Function.DC_DM_NHOM,
                    DatabaseConstant.Table.DM_NHOM,
                    DatabaseConstant.Action.SUA,
                    listLockId);

                if (ret)
                {
                    action = DatabaseConstant.Action.SUA;
                    SetEnabledControls();
                    SetVisibledControls();
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
            SetEnabledControls();
            SetVisibledControls();
            CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain, function);
        }

        public void OnModify(DM_NHOM_DTO obj)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                DanhMucProcess processDanhMuc = new DanhMucProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = false;
                
                ret = processDanhMuc.Nhom02(DatabaseConstant.Action.SUA, ref obj, ref listClientResponseDetail);
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

        public void AfterModify(bool ret, DM_NHOM_DTO obj, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (ret)
                {
                    LMessage.ShowMessage("M.DungChung.CapNhatThanhCong", LMessage.MessageBoxType.Information);

                    //sTrangThaiNVu = obj.TTHAI_NVU;
                    //lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(sTrangThaiNVu);
                    //txtTrangThaiBanGhi.Text = BusinessConstant.layNgonNguSuDung(obj.TTHAI_BGHI);
                    //raddtNgayCapNhat.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
                    //txtNguoiCapNhat.Text = ClientInformation.TenDangNhap;

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
                listLockId.Add(id);

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.DMDC,
                    DatabaseConstant.Function.DC_DM_NHOM,
                    DatabaseConstant.Table.DM_NHOM,
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

                    bool retLockData = process.LockData(DatabaseConstant.Module.DMDC,
                        DatabaseConstant.Function.DC_DM_NHOM,
                        DatabaseConstant.Table.DM_NHOM,
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
                listLockId.Add(id);
                List<int> listUnlockId = new List<int>();

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.DMDC,
                    DatabaseConstant.Function.DC_DM_NHOM,
                    DatabaseConstant.Table.DM_NHOM,
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
            DanhMucProcess processDanhMuc = new DanhMucProcess();
            try
            {
                bool ret = false;
                obj.ID = id;
                
                ret = processDanhMuc.Nhom02(action, ref obj, ref listClientResponseDetail);
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
                processDanhMuc = null;
            }
        }

        public void AfterDelete(bool ret, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (ret)
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
                listLockId.Add(id);
                List<int> listUnlockId = new List<int>();

                bool retUnlockData = process.UnlockData(DatabaseConstant.Module.DMDC,
                    DatabaseConstant.Function.DC_DM_NHOM,
                    DatabaseConstant.Table.DM_NHOM,
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

        #endregion

        private void OnPreviewQuyetDinh()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            // Cảnh báo khi không có dữ liệu
            if (LObject.IsNullOrEmpty(txtMaNhom.Text))
            {
                LMessage.ShowMessage("Không có thông tin dữ liệu cần hiển thị", LMessage.MessageBoxType.Warning);

            }
            else
            {
                if (ClientInformation.Company.Equals("QUANGBINH"))
                {
                    VanHanhGiaoDich xemBaoCao = new VanHanhGiaoDich();

                    List<ThamSoBaoCao> lstThamSo = new List<ThamSoBaoCao>();
                    lstThamSo.Add(new ThamSoBaoCao("@MaChiNhanh", ClientInformation.MaDonVi, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("@MaNhom", txtMaNhom.Text, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("@NgayBaoCao", ClientInformation.NgayLamViecHienTai, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("MaNgonNgu", ClientInformation.NgonNgu, ApplicationConstant.LoaiThamSoBaoCao.LANG.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("MaDinhDang", ApplicationConstant.LoaiDinhDangBaoCao.EXCEL.layGiaTri(), ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri()));

                    string maBaoCao = DatabaseConstant.LayMaBaoCaoQuangBinh(DatabaseConstant.DanhSachBaoCaoQuangBinh.KHTV_QUYET_DINH_CONG_NHAN_TVIEN);
                    xemBaoCao.LayDuLieu(maBaoCao, lstThamSo);
                }
                else
                {

                }
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void OnPreviewHopDong()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            // Cảnh báo khi không có dữ liệu
            if (LObject.IsNullOrEmpty(txtMaNhom.Text))
            {
                LMessage.ShowMessage("Không có thông tin dữ liệu cần hiển thị", LMessage.MessageBoxType.Warning);

            }
            else
            {
                if (ClientInformation.Company.Equals("QUANGBINH"))
                {
                    VanHanhGiaoDich xemBaoCao = new VanHanhGiaoDich();

                    List<ThamSoBaoCao> lstThamSo = new List<ThamSoBaoCao>();
                    lstThamSo.Add(new ThamSoBaoCao("@MaChiNhanh", ClientInformation.MaDonVi, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("@MaNhom", txtMaNhom.Text, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("@NgayBaoCao", ClientInformation.NgayLamViecHienTai, ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("MaNgonNgu", ClientInformation.NgonNgu, ApplicationConstant.LoaiThamSoBaoCao.LANG.layGiaTri()));
                    lstThamSo.Add(new ThamSoBaoCao("MaDinhDang", ApplicationConstant.LoaiDinhDangBaoCao.EXCEL.layGiaTri(), ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri()));

                    string maBaoCao = DatabaseConstant.LayMaBaoCaoQuangBinh(DatabaseConstant.DanhSachBaoCaoQuangBinh.KHTV_HOP_DONG_BAO_LANH_NHOM);
                    xemBaoCao.LayDuLieu(maBaoCao, lstThamSo);
                }
                else
                {

                }
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }
    }
}
