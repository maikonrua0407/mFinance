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
using Microsoft.Windows.Controls.Ribbon;
using Utilities.Common;
using Presentation.Process;
using Presentation.Process.Common;
using Presentation.Process.PopupServiceRef;
using PresentationWPF.CustomControl;
using Presentation.Process.DanhMucServiceRef;
using System.Data;

namespace PresentationWPF.DanhMuc.KhuVuc
{
    /// <summary>
    /// Interaction logic for ucKhuVucCT.xaml
    /// </summary>
    public partial class ucKhuVucCT_01 : UserControl
    {
        #region Khai bao
        public static RoutedCommand AddCommand = new RoutedCommand();
        public static RoutedCommand HoldCommand = new RoutedCommand();
        public static RoutedCommand SaveCommand = new RoutedCommand();
        public static RoutedCommand ModifyCommand = new RoutedCommand();
        public static RoutedCommand DeleteCommand = new RoutedCommand();
        public static RoutedCommand ApproveCommand = new RoutedCommand();
        public static RoutedCommand RefuseCommand = new RoutedCommand();
        public static RoutedCommand CancelCommand = new RoutedCommand();
        public static RoutedCommand HelpCommand = new RoutedCommand();
        public static RoutedCommand CloseCommand = new RoutedCommand();
        List<AutoCompleteEntry> lstSourcePGD = new List<AutoCompleteEntry>();
        string tthaiNVu;

        int idKhuVuc;
        public int IdKhuVuc
        {
            get { return idKhuVuc; }
            set { idKhuVuc = value; }
        }

        string maChiNhanh;
        public string MaChiNhanh
        {
            get { return maChiNhanh; }
            set { maChiNhanh = value; }
        }

        string maPhongGiaoDich;
        public string MaPhongGiaoDich
        {
            get { return maPhongGiaoDich; }
            set { maPhongGiaoDich = value; }
        }

        DM_KHU_VUC objKhuVuc;
        public DatabaseConstant.Action action = DatabaseConstant.Action.XEM;
        public EventHandler OnSavingCompleted;
        List<DM_KHU_VUC_NS> lstNhanSu = null;
        DataTable dtCanBoQLy;
        DataTable dtCanBoCTV;
        List<DataRow> lstPopup = new List<DataRow>();
        public void LayDuLieuTuPopup(List<DataRow> lst)
        {
            lstPopup = lst;
        }
        #endregion

        #region Khoi tao
        public ucKhuVucCT_01()
        {
            InitializeComponent();
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.DanhMuc;component/KhuVuc/ucKhuVucCT_01.xaml", ref Toolbar, ref mnuMain);
            foreach (var item in mnuMain.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += btnShortcutKey_Click;
            }
            BindShortkey();
            KhoiTaoDuLieu();
            KhoiTaoComboBox();
            InitEvenHanler();
            ResetForm();
        }

        public ucKhuVucCT_01(int id)
        {
            InitializeComponent();
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.DanhMuc;component/KhuVuc/ucKhuVucCT_01.xaml", ref Toolbar, ref mnuMain);
            foreach (var item in mnuMain.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += btnShortcutKey_Click;
            }
            BindShortkey();
            KhoiTaoDuLieu(id);
            KhoiTaoComboBox();
            InitEvenHanler();
            ResetForm();
        }

        private void KhoiTaoDuLieu()
        {
            if (action == DatabaseConstant.Action.THEM)
            {
                MaChiNhanh = ClientInformation.MaDonVi;
                MaPhongGiaoDich = ClientInformation.MaDonViGiaoDich;
            }
            else
            {
                
            }
        }

        private void KhoiTaoDuLieu(int id)
        {
            if (action == DatabaseConstant.Action.THEM)
            {
                MaChiNhanh = ClientInformation.MaDonVi;
                MaPhongGiaoDich = ClientInformation.MaDonViGiaoDich;
            }
            else
            {
                IdKhuVuc = id;
                DM_KHU_VUC obj = new DanhMucProcess().getKhuVucById(id);
                MaChiNhanh = obj.MA_DVI_QLY;
                MaPhongGiaoDich = obj.MA_DVI_TAO;
            }
        }

        private void KhoiTaoComboBox()
        {
            List<string> lstDieuKien = new List<string>();
            string sTruyVan = "";
            AutoComboBox auto = new AutoComboBox();
            sTruyVan = DatabaseConstant.DanhSachTruyVan.COMBOBOX_PHONG_GD.getValue();
            if (idKhuVuc == 0)
                lstDieuKien.Add(ClientInformation.MaDonVi);
            else
                lstDieuKien.Add(MaChiNhanh);
            //auto.GenAutoComboBox(ref lstSourcePGD, ref cmbDonVi, sTruyVan, lstDieuKien, ClientInformation.MaDonViGiaoDich);
            auto.GenAutoComboBox(ref lstSourcePGD, ref cmbDonVi, sTruyVan, lstDieuKien, MaPhongGiaoDich);
        }

        private void KhoiTaoDataTable()
        {
            dtCanBoQLy = new DataTable();
            dtCanBoCTV = new DataTable();

            dtCanBoQLy.Columns.Add("ID_NS_HO_SO", typeof(int));
            dtCanBoQLy.Columns.Add("MA_NS_HO_SO", typeof(string));
            dtCanBoQLy.Columns.Add("TEN_HO_SO", typeof(string));
            dtCanBoQLy.Columns.Add("CHUC_VU", typeof(string));
            dtCanBoQLy.Columns.Add("MA_LOAI_HSO", typeof(string));
            dtCanBoQLy.Columns.Add("LOAI_QLY_CHINH", typeof(string));

            dtCanBoCTV.Columns.Add("ID_NS_HO_SO", typeof(int));
            dtCanBoCTV.Columns.Add("MA_NS_HO_SO", typeof(string));
            dtCanBoCTV.Columns.Add("TEN_HO_SO", typeof(string));
            dtCanBoCTV.Columns.Add("CHUC_VU", typeof(string));
            dtCanBoCTV.Columns.Add("MA_LOAI_HSO", typeof(string));
            dtCanBoCTV.Columns.Add("LOAI_QLY_CHINH", typeof(string));

            raddgrCanBoQLy.ItemsSource = dtCanBoQLy;
            raddgrCongTacVien.ItemsSource = dtCanBoCTV;
            raddgrCanBoQLy.Rebind();
            raddgrCongTacVien.Rebind();
        }

        private void InitEvenHanler()
        {
            tlbAddCanBo.Click += new RoutedEventHandler(tlbAddCanBo_Click);
            tlbAddCongTac.Click += new RoutedEventHandler(tlbAddCongTac_Click);
            tlbDelCanBo.Click += new RoutedEventHandler(tlbDelCanBo_Click);
            tlbDelCongTac.Click += new RoutedEventHandler(tlbDelCongTac_Click);
        }

        
        #endregion

        #region Dang ky hot key

        private void BindShortkey()
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
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU_TAM)))
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

        private void tlbHotKey_Click(object sender, RoutedEventArgs e)
        {
            RibbonButton tlb = (RibbonButton)sender;
            string strTinhNang = tlb.Name.Substring(3, tlb.Name.Length - 3);
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THEM)))
            {
                ResetForm();
                SetEnabledAllControls(true);
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU_TAM)))
            {
                OnSave(BusinessConstant.TrangThaiNghiepVu.LUU_TAM, BusinessConstant.TrangThaiSuDung.SU_DUNG);
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU)))
            {
                OnSave(BusinessConstant.TrangThaiNghiepVu.DA_DUYET, BusinessConstant.TrangThaiSuDung.SU_DUNG);
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
                OnModify();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
            {
                beforeDelete();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
            {
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

        #endregion

        #region Dang ky shortcut key

        private void AddCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbAdd.IsEnabled;
        }
        private void AddCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ResetForm();
            SetEnabledAllControls(true);
        }

        private void HoldCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void HoldCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OnSave(BusinessConstant.TrangThaiNghiepVu.LUU_TAM, BusinessConstant.TrangThaiSuDung.SU_DUNG);
        }

        private void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OnSave(BusinessConstant.TrangThaiNghiepVu.DA_DUYET, BusinessConstant.TrangThaiSuDung.SU_DUNG);
        }

        private void ModifyCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void ModifyCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            tlbModify.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }

        private void DeleteCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void DeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            beforeDelete();
        }

        private void ApproveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void ApproveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            tlbApprove.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }

        private void RefuseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void RefuseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            tlbRefuse.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }

        private void CancelCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void CancelCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            tlbCancel.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
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

        private void btnShortcutKey_Click(object sender, RoutedEventArgs e)
        {
            string strTinhNang = ((MenuItem)sender).Name.Substring(3, ((MenuItem)sender).Name.Length - 3);
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THEM)))
            {
                ResetForm();
                SetEnabledAllControls(true);
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU_TAM)))
            {
                OnSave(BusinessConstant.TrangThaiNghiepVu.LUU_TAM, BusinessConstant.TrangThaiSuDung.SU_DUNG);
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU)))
            {
                OnSave(BusinessConstant.TrangThaiNghiepVu.DA_DUYET, BusinessConstant.TrangThaiSuDung.SU_DUNG);
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
                OnModify();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
            {
                beforeDelete();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
            {
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

        #region Xu ly Giao dien
        /// <summary>
        /// Sự kiện ấn key trên form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            PresentationWPF.CustomControl.CommonFunction.CloseUserControl(e, this);
            PresentationWPF.CustomControl.CommonFunction.SelectNextControl(e);
        }

        private void ResetForm()
        {
            idKhuVuc = 0;
            lblTrangThai.Content = tthaiNVu = "";
            txtMaKhuVuc.Text = "";
            txtTenKhuVuc.Text = "";
            txtTenTat.Text = "";
            cmbDonVi.SelectedIndex = lstSourcePGD.IndexOf(lstSourcePGD.FirstOrDefault(e => e.KeywordStrings.FirstOrDefault().Equals(ClientInformation.MaDonViGiaoDich)));
            txtNgayLap.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
            txtNguoiLap.Text = ClientInformation.TenDangNhap;
            txtNgayDuyet.Value = null;
            txtNguoiDuyet.Text = "";
            txtTrangThaiBanGhi.Text = "";
            txtTenKhuVuc.Focus();
            KhoiTaoDataTable();
            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.THEM, tthaiNVu, mnuMain, DatabaseConstant.Function.DC_DM_KHU_VUC);
        }

        /// <summary>
        /// SetEnabledAllControls
        /// </summary>
        /// <param name="enable"></param>
        private void SetEnabledAllControls(bool enable)
        {
            //cmbChiNhanh.IsEnabled = enable;
            //cmbPhongGD.IsEnabled = enable;
            cmbDonVi.IsEnabled = enable;
            txtTenKhuVuc.IsEnabled = enable;
            txtTenTat.IsEnabled = enable;
            tlbAddCanBo.IsEnabled = enable;
            tlbAddCongTac.IsEnabled = enable;
            tlbDelCanBo.IsEnabled = enable;
            tlbDelCongTac.IsEnabled = enable;
        }

        /// <summary>
        /// SetEnabledRequiredControls
        /// </summary>
        /// <param name="enable"></param>
        private void SetEnabledRequiredControls(bool enable)
        {
            cmbDonVi.IsEnabled = enable;
            txtMaKhuVuc.IsEnabled = enable;
        }

        private void tlbDelCongTac_Click(object sender, RoutedEventArgs e)
        {
            foreach (DataRow drv in raddgrCongTacVien.SelectedItems)
            {
                dtCanBoCTV.Rows.Remove(drv);
            }
            raddgrCongTacVien.ItemsSource = dtCanBoCTV;
            raddgrCongTacVien.Rebind();
        }

        private void tlbDelCanBo_Click(object sender, RoutedEventArgs e)
        {
            foreach (DataRow drv in raddgrCanBoQLy.SelectedItems)
            {
                dtCanBoQLy.Rows.Remove(drv);
            }
            raddgrCanBoQLy.ItemsSource = dtCanBoQLy;
            raddgrCanBoQLy.Rebind();
        }

        private void tlbAddCongTac_Click(object sender, RoutedEventArgs e)
        {
            List<string> lstDieuKien = new List<string>();
            string lstIDKH = "";
            for (int i = 0; i < dtCanBoCTV.Rows.Count; i++)
            {
                lstIDKH += "," + dtCanBoCTV.Rows[i]["ID_NS_HO_SO"].ToString();
            }
            if (lstIDKH.Length > 0)
                lstIDKH = "(" + lstIDKH.Substring(1) + ")";
            else
                lstIDKH = "(0)";
            lstDieuKien.Add(ClientInformation.MaDonViGiaoDich);
            lstDieuKien.Add(BusinessConstant.LOAI_HO_SO.CONG_TAC_VIEN.layGiaTri());
            lstDieuKien.Add(lstIDKH);
            lstPopup = new List<DataRow>();
            PopupProcess popupProcess = new PopupProcess();
            popupProcess.getPopupInformation(DatabaseConstant.DanhSachTruyVan.POPUP_DS_HOSO_NHAN_SU_CTV.getValue(), lstDieuKien);
            SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;
            ucPopup popup = new ucPopup(false, simplePopupResponse, true);
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
                    DataRow dradd = dtCanBoCTV.NewRow();
                    dradd["ID_NS_HO_SO"] = dr["ID_NS_HO_SO"];
                    dradd["MA_NS_HO_SO"] = dr["MA_NS_HO_SO"];
                    dradd["TEN_HO_SO"] = dr["TEN_HO_SO"];
                    dradd["CHUC_VU"] = dr["CHUC_VU"];
                    dradd["MA_LOAI_HSO"] = dr["MA_LOAI_HSO"];
                    dradd["LOAI_QLY_CHINH"] = dr["LOAI_QLY_CHINH"];
                    dtCanBoCTV.Rows.Add(dradd);
                }
                raddgrCongTacVien.ItemsSource = dtCanBoCTV;
                raddgrCongTacVien.Rebind();
            }
        }

        private void tlbAddCanBo_Click(object sender, RoutedEventArgs e)
        {
            List<string> lstDieuKien = new List<string>();
            string lstIDKH = "";
            for (int i = 0; i < dtCanBoQLy.Rows.Count; i++)
            {
                lstIDKH += "," + dtCanBoQLy.Rows[i]["ID_NS_HO_SO"].ToString();
            }
            if (lstIDKH.Length > 0)
                lstIDKH = "(" + lstIDKH.Substring(1) + ")";
            else
                lstIDKH = "(0)";
            lstDieuKien.Add(ClientInformation.MaDonViGiaoDich);
            lstDieuKien.Add(BusinessConstant.LOAI_HO_SO.CHINH_THUC.layGiaTri());
            lstDieuKien.Add(lstIDKH);
            lstPopup = new List<DataRow>();
            PopupProcess popupProcess = new PopupProcess();
            popupProcess.getPopupInformation(DatabaseConstant.DanhSachTruyVan.POPUP_DS_HOSO_NHAN_SU_QLY.getValue(), lstDieuKien);
            SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;
            ucPopup popup = new ucPopup(false, simplePopupResponse, true);
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
                    DataRow dradd = dtCanBoQLy.NewRow();
                    dradd["ID_NS_HO_SO"] = dr["ID_NS_HO_SO"];
                    dradd["MA_NS_HO_SO"] = dr["MA_NS_HO_SO"];
                    dradd["TEN_HO_SO"] = dr["TEN_HO_SO"];
                    dradd["CHUC_VU"] = dr["CHUC_VU"];
                    dradd["MA_LOAI_HSO"] = dr["MA_LOAI_HSO"];
                    dradd["LOAI_QLY_CHINH"] = dr["LOAI_QLY_CHINH"];
                    dtCanBoQLy.Rows.Add(dradd);
                }
                raddgrCanBoQLy.ItemsSource = dtCanBoQLy;
                raddgrCanBoQLy.Rebind();
            }
        }
        #endregion

        #region Xy ly nghiep vu
        bool Validation()
        {
            if (txtTenKhuVuc.Text.IsNullOrEmptyOrSpace())
            {
                CommonFunction.ThongBaoTrong(lblTenKhuVuc.Content.ToString());
                txtTenKhuVuc.Focus();
                return false;
            }
            else if (txtTenTat.Text.IsNullOrEmptyOrSpace())
            {
                CommonFunction.ThongBaoTrong(lblTenTat.Content.ToString());
                txtTenTat.Focus();
                return false;
            }
            else if (cmbDonVi.Text.IsNullOrEmptyOrSpace())
            {
                CommonFunction.ThongBaoTrong(lblDonVi.Content.ToString());
                cmbDonVi.Focus();
                return false;
            }
            return true;
        }

        private DM_KHU_VUC GetDataForm(BusinessConstant.TrangThaiNghiepVu nghiepvu, BusinessConstant.TrangThaiSuDung sudung)
        {
            DM_KHU_VUC obj = new DM_KHU_VUC();
            DataSet dsDVi = new DanhMucProcess().getDonViTheoMa(lstSourcePGD.ElementAt(cmbDonVi.SelectedIndex).KeywordStrings[0]);
            obj.ID = idKhuVuc;
            obj.ID_DVI = Convert.ToInt32(dsDVi.Tables[0].Rows[0]["ID"]);
            obj.MA_DVI = lstSourcePGD.ElementAt(cmbDonVi.SelectedIndex).KeywordStrings.FirstOrDefault();
            if (idKhuVuc == 0)
                obj.MA_DVI_QLY = ClientInformation.MaDonVi;
            else
                obj.MA_DVI_QLY = MaChiNhanh;
            obj.MA_DVI_TAO = lstSourcePGD.ElementAt(cmbDonVi.SelectedIndex).KeywordStrings.FirstOrDefault();
            obj.MA_KVUC = txtMaKhuVuc.Text;
            obj.TEN_KVUC = txtTenKhuVuc.Text;
            obj.TEN_TAT = txtTenTat.Text;
            obj.TTHAI_NVU = nghiepvu.layGiaTri();
            obj.TTHAI_BGHI = sudung.layGiaTri();
            obj.NGAY_CNHAT = idKhuVuc > 0 ? ClientInformation.NgayLamViecHienTai : "";
            obj.NGUOI_CNHAT = idKhuVuc > 0 ? ClientInformation.TenDangNhap : "";
            obj.NGAY_NHAP = idKhuVuc == 0 ? ClientInformation.NgayLamViecHienTai : LDateTime.DateToString(txtNgayLap.Value.GetValueOrDefault(), ApplicationConstant.defaultDateTimeFormat);
            obj.NGUOI_NHAP = idKhuVuc == 0 ? ClientInformation.TenDangNhap : txtNguoiLap.Text;
            lstNhanSu = new List<DM_KHU_VUC_NS>();
            if (!LObject.IsNullOrEmpty(dtCanBoQLy))
            {
                
                foreach (DataRowView drv in dtCanBoQLy.DefaultView)
                {
                    DM_KHU_VUC_NS objQLy = new DM_KHU_VUC_NS();
                    objQLy.ID_KHU_VUC = idKhuVuc;
                    objQLy.MA_KHU_VUC = txtMaKhuVuc.Text;
                    objQLy.MA_LOAI_HSO = drv["MA_LOAI_HSO"].ToString();
                    objQLy.ID_NS_HO_SO = drv["ID_NS_HO_SO"].ToString().StringToInt32();
                    objQLy.MA_NS_HO_SO = drv["MA_NS_HO_SO"].ToString();
                    objQLy.LOAI_QLY_CHINH = drv["LOAI_QLY_CHINH"].ToString();
                    objQLy.MA_DVI_QLY = obj.MA_DVI_QLY;
                    objQLy.MA_DVI_TAO = obj.MA_DVI_TAO;
                    objQLy.NGAY_CNHAT = obj.NGAY_CNHAT;
                    objQLy.NGAY_NHAP = obj.NGAY_NHAP;
                    objQLy.NGUOI_CNHAT = obj.NGUOI_CNHAT;
                    objQLy.NGUOI_NHAP = obj.NGUOI_NHAP;
                    objQLy.TTHAI_BGHI = obj.TTHAI_BGHI;
                    objQLy.TTHAI_NVU = obj.TTHAI_NVU;
                    lstNhanSu.Add(objQLy);
                }
            }
            if (!LObject.IsNullOrEmpty(dtCanBoCTV))
            {
                foreach (DataRowView drv in dtCanBoCTV.DefaultView)
                {
                    DM_KHU_VUC_NS objQLy = new DM_KHU_VUC_NS();
                    objQLy.ID_KHU_VUC = idKhuVuc;
                    objQLy.MA_KHU_VUC = txtMaKhuVuc.Text;
                    objQLy.MA_LOAI_HSO = drv["MA_LOAI_HSO"].ToString();
                    objQLy.ID_NS_HO_SO = drv["ID_NS_HO_SO"].ToString().StringToInt32();
                    objQLy.MA_NS_HO_SO = drv["MA_NS_HO_SO"].ToString();
                    objQLy.LOAI_QLY_CHINH = drv["LOAI_QLY_CHINH"].ToString();
                    objQLy.MA_DVI_QLY = obj.MA_DVI_QLY;
                    objQLy.MA_DVI_TAO = obj.MA_DVI_TAO;
                    objQLy.NGAY_CNHAT = obj.NGAY_CNHAT;
                    objQLy.NGAY_NHAP = obj.NGAY_NHAP;
                    objQLy.NGUOI_CNHAT = obj.NGUOI_CNHAT;
                    objQLy.NGUOI_NHAP = obj.NGUOI_NHAP;
                    objQLy.TTHAI_BGHI = obj.TTHAI_BGHI;
                    objQLy.TTHAI_NVU = obj.TTHAI_NVU;
                    lstNhanSu.Add(objQLy);
                }
            }
            return obj;
        }

        private void OnSave(BusinessConstant.TrangThaiNghiepVu nghiepvu, BusinessConstant.TrangThaiSuDung sudung)
        {
            try
            {
                Cursor = Cursors.Wait;
                if (Validation())
                {
                    DM_KHU_VUC obj = GetDataForm(nghiepvu, sudung);

                    // Lock dữ liệu nếu người dùng chấp nhận cảnh báo
                    UtilitiesProcess process = new UtilitiesProcess();
                    List<int> listLockId = new List<int>();
                    listLockId.Add(idKhuVuc);
                    bool retLockData = true;
                    if (idKhuVuc > 0)
                    {
                        //if (ClientInformation.MaDonViGiaoDich != MaPhongGiaoDich)
                        //{
                        //    LMessage.ShowMessage("Không thực hiện được đối với dữ liệu thuộc đơn vị khác", LMessage.MessageBoxType.Error);
                        //    return;
                        //}
                        retLockData = process.LockData(DatabaseConstant.Module.DMDC,
                            DatabaseConstant.Function.DC_DM_KHU_VUC,
                            DatabaseConstant.Table.DM_KHU_VUC,
                            DatabaseConstant.Action.SUA,
                            listLockId);
                    }
                    if (retLockData)
                        AfterSave(obj);
                }
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                Cursor = Cursors.Arrow;
                // Unlock dữ liệu nếu người dùng chấp nhận cảnh báo
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(idKhuVuc);
                bool retLockData = true;
                if (idKhuVuc > 0)
                    retLockData = process.UnlockData(DatabaseConstant.Module.DMDC,
                        DatabaseConstant.Function.DC_DM_KHU_VUC,
                        DatabaseConstant.Table.DM_KHU_VUC,
                        DatabaseConstant.Action.SUA,
                        listLockId);
            }
        }

        private void AfterSave(DM_KHU_VUC obj)
        {
            try
            {
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                if (idKhuVuc == 0)
                    obj = new DanhMucProcess().ThemKhuVuc(obj, ref listClientResponseDetail,lstNhanSu);
                else
                    obj = new DanhMucProcess().SuaKhuVuc(obj, ref listClientResponseDetail, lstNhanSu);
                BeforSave(obj, listClientResponseDetail);
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void BeforSave(DM_KHU_VUC obj, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (obj.ID > 0)
                {
                    SetThongTin(obj);
                    LMessage.ShowMessage("M.DungChung.LuuDuLieuThanhCong", LMessage.MessageBoxType.Information);                    
                    if (cbMultiAdd.IsChecked.GetValueOrDefault())
                    {
                        ResetForm();
                        SetEnabledAllControls(true);
                    }
                }
                else
                {
                    CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                }
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {

            }
        }

        /// <summary>
        /// Trước khi xóa
        /// </summary>
        private void beforeDelete()
        {
            // Cảnh báo người dùng
            MessageBoxResult ret = LMessage.ShowMessage("M.DungChung.HoiXoa", LMessage.MessageBoxType.Question);

            if (ret == MessageBoxResult.Yes)
            {
                //if (ClientInformation.MaDonViGiaoDich != MaPhongGiaoDich)
                //{
                //    LMessage.ShowMessage("Không thực hiện được đối với dữ liệu thuộc đơn vị khác", LMessage.MessageBoxType.Error);
                //    return;
                //}

                // Lock dữ liệu nếu người dùng chấp nhận cảnh báo
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(idKhuVuc);
                List<int> listLockedId = new List<int>();

                bool retLockData = process.LockData(DatabaseConstant.Module.DMDC,
                    DatabaseConstant.Function.DC_DM_KHU_VUC,
                    DatabaseConstant.Table.DM_KHU_VUC,
                    DatabaseConstant.Action.XOA,
                    listLockId);

                // Nếu lock thành công >> cho phép sửa
                if (retLockData)
                {
                    // Gọi tới hàm xóa dữ liệu
                    OnDelete(listLockId);
                    return;
                }
                // Nếu lock không thành công >> cảnh báo
                else
                {
                    LMessage.ShowMessage("M.ResponseMessage.Common.LockDataInvalid", LMessage.MessageBoxType.Information);
                    return;
                }
            }
            else
            {
                // Unlock dữ liệu
                // (Hiện tại không phải Unlock dữ liệu, vì chỉ được xóa khi chọn sửa - sẽ Unlock sau)
                return;
            }
        }

        private void OnDelete(List<int> lstID)
        {
            try
            {
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool bKetQua = new DanhMucProcess().XoaKhuVuc(lstID.ToArray(), ref listClientResponseDetail);
                AfterDelete(bKetQua, listClientResponseDetail);
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void AfterDelete(bool bKetQua, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (bKetQua)
                {
                    LMessage.ShowMessage("M.DungChung.XoaThanhCong", LMessage.MessageBoxType.Information);
                    CommonFunction.CloseUserControl(this);
                }
                else
                {
                    CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                }
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
            }
        }

        private void AfterApprove()
        {
            try
            {
                Cursor = Cursors.Wait;
                // Lock dữ liệu nếu người dùng chấp nhận cảnh báo
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(idKhuVuc);
                bool retLockData = true;
                if (idKhuVuc > 0)
                    retLockData = process.LockData(DatabaseConstant.Module.DMDC,
                        DatabaseConstant.Function.DC_DM_KHU_VUC,
                        DatabaseConstant.Table.DM_KHU_VUC,
                        DatabaseConstant.Action.DUYET,
                        listLockId);
                OnApprove(listLockId);
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                // Lock dữ liệu nếu người dùng chấp nhận cảnh báo
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(idKhuVuc);
                bool retLockData = true;
                if (idKhuVuc > 0)
                    retLockData = process.UnlockData(DatabaseConstant.Module.DMDC,
                        DatabaseConstant.Function.DC_DM_KHU_VUC,
                        DatabaseConstant.Table.DM_KHU_VUC,
                        DatabaseConstant.Action.DUYET,
                        listLockId);
                Cursor = Cursors.Arrow;
            }
        }

        private void OnApprove(List<int> lstID)
        {
            try
            {
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool bKetQua = new DanhMucProcess().DuyetKhuVuc(lstID.ToArray(), ref listClientResponseDetail);
                BeforApprove(bKetQua, listClientResponseDetail);
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void BeforApprove(bool bKetQua, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (bKetQua)
                {
                    LMessage.ShowMessage("M.DungChung.DuyetThanhCong", LMessage.MessageBoxType.Information);                    
                    SetDataForm(idKhuVuc);
                    if (cbMultiAdd.IsChecked.GetValueOrDefault())
                        ResetForm();
                }
                else
                {
                    CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                }
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
            }
        }


        private void AfterRefuse()
        {
            try
            {
                Cursor = Cursors.Wait;
                // Lock dữ liệu nếu người dùng chấp nhận cảnh báo
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(idKhuVuc);
                bool retLockData = true;
                if (idKhuVuc > 0)
                    retLockData = process.LockData(DatabaseConstant.Module.DMDC,
                        DatabaseConstant.Function.DC_DM_KHU_VUC,
                        DatabaseConstant.Table.DM_KHU_VUC,
                        DatabaseConstant.Action.TU_CHOI_DUYET,
                        listLockId);
                OnRefuse(listLockId);
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                // Lock dữ liệu nếu người dùng chấp nhận cảnh báo
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(idKhuVuc);
                bool retLockData = true;
                if (idKhuVuc > 0)
                    retLockData = process.UnlockData(DatabaseConstant.Module.DMDC,
                        DatabaseConstant.Function.DC_DM_KHU_VUC,
                        DatabaseConstant.Table.DM_KHU_VUC,
                        DatabaseConstant.Action.TU_CHOI_DUYET,
                        listLockId);
                Cursor = Cursors.Arrow;
            }
        }

        private void OnRefuse(List<int> lstID)
        {
            try
            {
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool bKetQua = new DanhMucProcess().TuChoiKhuVuc(lstID.ToArray(), ref listClientResponseDetail);
                BeforRefuse(bKetQua, listClientResponseDetail);
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void BeforRefuse(bool bKetQua, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (bKetQua)
                {
                    LMessage.ShowMessage("M.DungChung.TuChoiDuyetThanhCong", LMessage.MessageBoxType.Information);
                    SetDataForm(idKhuVuc);
                    if (cbMultiAdd.IsChecked.GetValueOrDefault())
                        ResetForm();
                }
                else
                {
                    CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                }
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
            }
        }


        private void AfterCancel()
        {
            try
            {
                Cursor = Cursors.Wait;
                // Lock dữ liệu nếu người dùng chấp nhận cảnh báo
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(idKhuVuc);
                bool retLockData = true;
                if (idKhuVuc > 0)
                    retLockData = process.LockData(DatabaseConstant.Module.DMDC,
                        DatabaseConstant.Function.DC_DM_KHU_VUC,
                        DatabaseConstant.Table.DM_KHU_VUC,
                        DatabaseConstant.Action.THOAI_DUYET,
                        listLockId);
                OnCancel(listLockId);
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                // Lock dữ liệu nếu người dùng chấp nhận cảnh báo
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(idKhuVuc);
                bool retLockData = true;
                if (idKhuVuc > 0)
                    retLockData = process.UnlockData(DatabaseConstant.Module.DMDC,
                        DatabaseConstant.Function.DC_DM_KHU_VUC,
                        DatabaseConstant.Table.DM_KHU_VUC,
                        DatabaseConstant.Action.THOAI_DUYET,
                        listLockId);
                Cursor = Cursors.Arrow;
            }
        }

        private void OnCancel(List<int> lstID)
        {
            try
            {
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool bKetQua = new DanhMucProcess().ThoaiDuyetKhuVuc(lstID.ToArray(), ref listClientResponseDetail);
                BeforCancel(bKetQua, listClientResponseDetail);
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void BeforCancel(bool bKetQua, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (bKetQua)
                {
                    LMessage.ShowMessage("M.DungChung.ThoaiDuyetThanhCong", LMessage.MessageBoxType.Error);
                    SetDataForm(idKhuVuc);
                    if (cbMultiAdd.IsChecked.GetValueOrDefault())
                        ResetForm();
                }
                else
                {
                    CommonFunction.ThongBaoKetQua(listClientResponseDetail);                    
                }
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
            }
        }

        private void SetThongTin(DM_KHU_VUC obj)
        {
            try
            {
                idKhuVuc = obj.ID;
                tthaiNVu = obj.TTHAI_NVU;
                maChiNhanh = obj.MA_DVI_QLY;
                maPhongGiaoDich = obj.MA_DVI_TAO;
                lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(tthaiNVu);
                txtMaKhuVuc.Text = obj.MA_KVUC;
                txtNguoiDuyet.Text = obj.NGUOI_CNHAT;
                if (!obj.NGAY_CNHAT.IsNullOrEmptyOrSpace())
                    txtNgayDuyet.Value = LDateTime.StringToDate(obj.NGAY_CNHAT, ApplicationConstant.defaultDateTimeFormat);
                txtTrangThaiBanGhi.Text = BusinessConstant.layNgonNguSuDung(obj.TTHAI_BGHI);
                action = DatabaseConstant.Action.XEM;
                CommonFunction.RefreshButton(Toolbar, action, tthaiNVu, mnuMain, DatabaseConstant.Function.DC_DM_KHU_VUC);
                SetEnabledAllControls(false);
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
            }

        }

        public void SetDataForm(int id)
        {
            try
            {
                idKhuVuc = id;
                DM_KHU_VUC obj = new DanhMucProcess().getKhuVucById(idKhuVuc);
                DataSet ds = new DanhMucProcess().getThongTinCTietKhuVuc(idKhuVuc.ToString());
                if (!LObject.IsNullOrEmpty(obj))
                {
                    tthaiNVu = obj.TTHAI_NVU;
                    txtMaKhuVuc.Text = obj.MA_KVUC;
                    txtTenKhuVuc.Text = obj.TEN_KVUC;
                    txtTenTat.Text = obj.TEN_TAT;
                    lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(tthaiNVu);
                    cmbDonVi.SelectedIndex = lstSourcePGD.IndexOf(lstSourcePGD.FirstOrDefault(e => e.KeywordStrings.First().Equals(obj.MA_DVI)));
                    txtNguoiLap.Text = obj.NGUOI_NHAP;
                    txtNgayLap.Value = LDateTime.StringToDate(obj.NGAY_NHAP, ApplicationConstant.defaultDateTimeFormat);
                    txtNguoiDuyet.Text = obj.NGUOI_CNHAT;
                    if (!obj.NGAY_CNHAT.IsNullOrEmptyOrSpace())
                        txtNgayDuyet.Value = LDateTime.StringToDate(obj.NGAY_CNHAT, ApplicationConstant.defaultDateTimeFormat);
                    txtTrangThaiBanGhi.Text = BusinessConstant.layNgonNguSuDung(obj.TTHAI_BGHI);
                    if(!LObject.IsNullOrEmpty(ds) && ds.Tables.Count > 1)
                    {
                        dtCanBoQLy = ds.Tables["CAN_BO_QUAN_LY"].Copy();
                        dtCanBoCTV = ds.Tables["CONG_TAC_VIEN"].Copy();
                        raddgrCanBoQLy.ItemsSource = dtCanBoQLy;
                        raddgrCongTacVien.ItemsSource = dtCanBoCTV;
                        raddgrCanBoQLy.Rebind();
                        raddgrCongTacVien.Rebind();
                    }
                    if (action.Equals(DatabaseConstant.Action.SUA))
                    {
                        SetEnabledAllControls(true);
                        SetEnabledRequiredControls(false);
                    }
                    else
                    {
                        SetEnabledAllControls(false);
                        SetEnabledRequiredControls(false);
                    }
                    CommonFunction.RefreshButton(Toolbar, action, tthaiNVu, mnuMain, DatabaseConstant.Function.DC_DM_KHU_VUC);
                }
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        void OnModify()
        {
            SetEnabledAllControls(true);
            SetEnabledRequiredControls(false);
            action = DatabaseConstant.Action.SUA;
            CommonFunction.RefreshButton(Toolbar, action, tthaiNVu, mnuMain, DatabaseConstant.Function.DC_DM_KHU_VUC);
        }
        #endregion
    }
}
