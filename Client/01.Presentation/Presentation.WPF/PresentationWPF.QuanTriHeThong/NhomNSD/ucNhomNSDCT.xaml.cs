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
using Presentation.Process.QuanTriHeThongServiceRef;
using Telerik.Windows.Controls;
using System.Data;
using System.Collections;
using System.Reflection;

namespace PresentationWPF.QuanTriHeThong.NhomNSD
{
    /// <summary>
    /// Interaction logic for ucNhomNSDCT.xaml
    /// </summary>
    public partial class ucNhomNSDCT : UserControl
    {
        #region Khai bao

        private int id = 0;
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private string tthaiNvu = "";
        public string TthaiNvu
        {
            get { return tthaiNvu; }
            set { tthaiNvu = value; }
        }

        public event EventHandler OnSavingCompleted;

        public string formCase = null;
        public HT_NHNSD obj;
        List<HT_NSD> dsNSD = new List<HT_NSD>();
        DataTable dt = new DataTable();
        QuanTriHeThongProcess qtht = new QuanTriHeThongProcess();
        public DatabaseConstant.Action luuAction = DatabaseConstant.Action.THEM;
        List<DataRow> lstPopup = new List<DataRow>();
        public void LayDuLieuTuPopup(List<DataRow> lst)
        {
            lstPopup = lst;
        }
        private bool chiXem = false;

        public bool ChiXem
        {
            get { return chiXem; }
            set { chiXem = value; }
        }
        private bool isLoaded = false;

        List<HT_TRUY_CAP> lstTruyCap = new List<HT_TRUY_CAP>();

        private DataTable dtTruyCap;

        private string MAC = BusinessConstant.LOAI_DIA_CHI.MAC.layGiaTri();
        private string IP = BusinessConstant.LOAI_DIA_CHI.IP.layGiaTri();

        List<AutoCompleteEntry> lstSourceDonVi = new List<AutoCompleteEntry>();

        public static RoutedCommand SaveCommand = new RoutedCommand();
        public static RoutedCommand ModifyCommand = new RoutedCommand();
        public static RoutedCommand DeleteCommand = new RoutedCommand();
        public static RoutedCommand HelpCommand = new RoutedCommand();
        public static RoutedCommand CloseCommand = new RoutedCommand();
        #endregion

        #region Khoi tao
        public ucNhomNSDCT()
        {
            InitializeComponent();
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.QuanTriHeThong;component/NhomNSD/ucNhomNSDCT.xaml", ref Toolbar, ref mnuMain);
            foreach (var item in mnuMain.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += btnShortcutKey_Click;
            }            
            BindShortkey();
            KhoiTaoGridTruyCap();
            cmbDonVi.Focus();            
        }

        /// <summary>
        /// Đưa form về trạng thái mặc định
        /// </summary>
        private void ResetForm()
        {
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
                    if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU)))
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
        private void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            onSave();
        }

        private void ModifyCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void ModifyCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            beforeModify();
        }

        private void DeleteCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void DeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            beforeDelete();
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
            onClose();
        }

        private void tlbHotKey_Click(object sender, RoutedEventArgs e)
        {
            RibbonButton tlb = (RibbonButton)sender;
            string strTinhNang = tlb.Name.Substring(3, tlb.Name.Length - 3);
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU)))
            {
                onSave();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
                beforeModify();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
            {
                beforeDelete();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRO_GIUP)))
            {
                onHelp();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DONG)))
            {
                onClose();
            }
        }

        private void btnShortcutKey_Click(object sender, RoutedEventArgs e)
        {
            string strTinhNang = ((MenuItem)sender).Name.Substring(3, ((MenuItem)sender).Name.Length - 3);
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU)))
            {
                onSave();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
                beforeModify();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
            {
                beforeDelete();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRO_GIUP)))
            {
                onHelp();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DONG)))
            {
                onClose();
            }
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
            if (e.Key == Key.Escape)
            {
                Release();
                if (OnSavingCompleted != null)
                {
                    OnSavingCompleted(this, EventArgs.Empty);
                }
                PresentationWPF.CustomControl.CommonFunction.CloseUserControl(e, this);
            }
            else
            {
                CustomControl.CommonFunction.SelectNextControl(e);
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                if (!isLoaded)
                {
                    if (luuAction == DatabaseConstant.Action.XEM)
                        formCase = "XEM";
                    else if (luuAction == DatabaseConstant.Action.SUA)
                        formCase = "SUA";
                    else if (luuAction == DatabaseConstant.Action.THEM)
                        formCase = "MANAGE";

                    if (formCase == null)
                    {
                        formCase = ClientInformation.FormCase;
                    }
                    string strTrangThai = string.Empty;
                    if (obj != null)
                    {
                        id = obj.ID;
                        tthaiNvu = obj.TTHAI_NVU;

                        // khởi tạo combobox
                        AutoComboBox auto = new AutoComboBox();
                        auto = new AutoComboBox();
                        List<string> lstDieuKien = new List<string>();

                        //auto.GenAutoComboBox(ref lstSourceDonVi, ref cmbDonVi, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DONVI.getValue(), null, obj.MA_DVI_QLY);
                        lstDieuKien = new List<string>();
                        lstDieuKien.Add("'" + DatabaseConstant.ToChucDonVi.HSO.getValue() + "', '" + DatabaseConstant.ToChucDonVi.CNH.getValue() + "'");
                        auto.GenAutoComboBox(ref lstSourceDonVi, ref cmbDonVi, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DONVITHEOLOAI.getValue(), lstDieuKien, obj.MA_DVI_QLY);

                        //lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(obj.TTHAI_NVU);
                        lblTrangThai.Content = BusinessConstant.layNgonNguSuDung(obj.TTHAI_BGHI);
                        
                        txtMaNhom.Text = obj.MA_NHNSD;
                        txtTenNhom.Text = obj.TEN_NHNSD;
                        txtMoTa.Text = obj.MO_TA;

                        if (obj.HAN_CHE_TRUY_CAP.Equals(BusinessConstant.CoKhong.CO.layGiaTri()))
                            chkHoatDong.IsChecked = true;
                        else
                            chkHoatDong.IsChecked = false;

                        txtTrangThai.Text = BusinessConstant.layNgonNguSuDung(obj.TTHAI_BGHI);
                        if (!string.IsNullOrEmpty(obj.NGAY_NHAP) && obj.NGAY_NHAP != "")
                            raddtNgayNhap.Value = LDateTime.StringToDate(obj.NGAY_NHAP, "yyyyMMdd");
                        else
                            raddtNgayNhap.Value = null;
                        txtNguoiLap.Text = obj.NGUOI_NHAP;
                        txtNguoiCapNhat.Text = obj.NGUOI_CNHAT;
                        if (!string.IsNullOrEmpty(obj.NGAY_CNHAT) && obj.NGAY_CNHAT != "")
                            raddtNgayCNhat.Value = LDateTime.StringToDate(obj.NGAY_CNHAT, "yyyyMMdd");
                        else
                            raddtNgayCNhat.Value = null;

                        BuildGridDoiTuong();
                        loadWidthColumnDoiTuong();
                        strTrangThai = obj.TTHAI_NVU;
                    }
                    else
                    {
                        // khởi tạo combobox
                        AutoComboBox auto = new AutoComboBox();
                        auto = new AutoComboBox();
                        List<string> lstDieuKien = new List<string>();

                        //auto.GenAutoComboBox(ref lstSourceDonVi, ref cmbDonVi, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DONVI.getValue(), null, ClientInformation.MaDonVi);
                        lstDieuKien = new List<string>();
                        lstDieuKien.Add("'" + DatabaseConstant.ToChucDonVi.HSO.getValue() + "', '" + DatabaseConstant.ToChucDonVi.CNH.getValue() + "'");
                        auto.GenAutoComboBox(ref lstSourceDonVi, ref cmbDonVi, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DONVITHEOLOAI.getValue(), lstDieuKien, ClientInformation.MaDonVi);

                        lblTrangThai.Content = string.Empty;
                        txtTrangThai.Text = string.Empty;
                        raddtNgayNhap.Value = null;
                        txtNguoiLap.Text = string.Empty;
                        raddtNgayCNhat.Value = null;
                        txtNguoiCapNhat.Text = string.Empty;
                        strTrangThai = string.Empty;
                    }
                    
                    CommonFunction.RefreshButton(Toolbar, luuAction, strTrangThai, mnuMain, DatabaseConstant.Function.HT_NHNSD);

                    if (luuAction != DatabaseConstant.Action.THEM)
                        cbMultiAdd.Visibility = Visibility.Collapsed;

                    ((RadTabItem)tabNSDCT.Items[0]).IsSelected = true;
                    tabNSDCT.SelectionChanged += tabNSDCT_SelectionChanged;
                    LoadGridTruyCap();
                    cmbDonVi.Focus();
                    isLoaded = true;
                }
                HideControl();
                if (ClientInformation.LoaiNguoiSuDung.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_QTDV.layGiaTri()) ||
                        ClientInformation.LoaiNguoiSuDung.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_NVDV.layGiaTri()))
                {
                    cmbDonVi.IsEnabled = false;
                }
                else
                {
                    cmbDonVi.IsEnabled = true;
                }
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void BuildGridDoiTuong()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                dt = new DataTable();
                dsNSD = new List<HT_NSD>();
                dsNSD = qtht.layNSDTheoNhom(obj);
                // Tạo source thông tin đối tượng
                dt = new DataTable();
                dt.Columns.Add(LLanguage.SearchResourceByKey("U.DungChung.STT"), typeof(string));
                dt.Columns.Add("ID", typeof(string));
                dt.Columns.Add(LLanguage.SearchResourceByKey("U.QuanTriHeThong.NhomNSD.ucNSDCT01.Ma"), typeof(string));
                dt.Columns.Add(LLanguage.SearchResourceByKey("U.QuanTriHeThong.NhomNSD.ucNSDCT01.Ten"), typeof(string));
                int stt = 0;
                foreach (var item in dsNSD)
                {
                    DataRow r = dt.NewRow();
                    stt = stt + 1;
                    r[0] = stt;
                    r[1] = item.ID;
                    r[2] = item.MA_NSD;
                    r[3] = item.TEN_DAY_DU;
                    dt.Rows.Add(r);
                }
                // đổ source lên lưới
                grDanhSach.ItemsSource = dt;
                HideControl();
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void HideControl()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                if (!string.IsNullOrEmpty(formCase))
                {
                    HeThong hethong = new HeThong();
                    ArrayList arr = new ArrayList();
                    arr = hethong.SetVisibleControl("PresentationWPF.QuanTriHeThong.NhomNSD.ucNhomNSDCT", formCase);
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
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void grDanhSach_Loaded(object sender, RoutedEventArgs e)
        {
            loadWidthColumnDoiTuong();
        }

        private void loadWidthColumnDoiTuong()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                for (int i = 0; i < grDanhSach.Columns.Count; i++)
                {
                    if (i == 2)
                        grDanhSach.Columns[i].IsVisible = false;
                    else if (i == 1)
                        grDanhSach.Columns[i].Width = new Telerik.Windows.Controls.GridViewLength(40, Telerik.Windows.Controls.GridViewLengthUnitType.Pixel);
                    else if (i == 3)
                        grDanhSach.Columns[i].Width = new Telerik.Windows.Controls.GridViewLength(1, Telerik.Windows.Controls.GridViewLengthUnitType.Auto);
                    else if (i == 4)
                        grDanhSach.Columns[i].Width = new Telerik.Windows.Controls.GridViewLength(1, Telerik.Windows.Controls.GridViewLengthUnitType.Star);
                }
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void tabNSDCT_SelectionChanged(object sender, Telerik.Windows.Controls.RadSelectionChangedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                if (tabNSDCT.SelectedIndex == 0)
                {
                    if (tabNSDCT != null)
                    {
                        UpdateLayout();
                        cmbDonVi.Focus();
                    }
                }
                else if (tabNSDCT.SelectedIndex == 3)
                {
                    if (txtTrangThai != null)
                    {
                        UpdateLayout();
                        txtTrangThai.Focus();
                    }
                }
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void btnAddTruyCap_Click(object sender, RoutedEventArgs e)
        {
            int stt = dtTruyCap.Rows.Count;
            dtTruyCap.Rows.Add(stt + 1, "", MAC, true);
            grdTruyCap.DataContext = dtTruyCap.DefaultView;
            grdTruyCap.Focus();
        }

        private void btnDeleteTruyCap_Click(object sender, RoutedEventArgs e)
        {
            List<DataRowView> lstSelected = new List<DataRowView>();
            for (int i = 0; i < grdTruyCap.SelectedItems.Count; i++)
            {
                DataRowView dr = (DataRowView)grdTruyCap.SelectedItems[i];
                if (!dr["STT"].ToString().IsNullOrEmptyOrSpace())
                    lstSelected.Add(dr);
            }

            if (lstSelected.Count == 0)
            {
                //LMessage.ShowMessage("M.DungChung.ChuaChonBanGhi", LMessage.MessageBoxType.Warning);
                return;
            }
            else
            {
                foreach (DataRowView dr in lstSelected)
                {
                    grdTruyCap.Items.Remove(dr);
                }

                for (int i = 0; i < grdTruyCap.Items.Count; i++)
                {

                    DataRowView dr = (DataRowView)grdTruyCap.Items[i];
                    if (!dr["STT"].ToString().IsNullOrEmptyOrSpace() && Convert.ToInt32(dr["STT"]) > 0)
                    {
                        if (!dr[2].ToString().IsNullOrEmptyOrSpace())
                        {
                            dr["STT"] = i + 1;
                            grdTruyCap.SelectedItem = grdTruyCap.Items[i];
                            grdTruyCap.CurrentItem = dr;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }

        private DataTable LayMacIP()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID");
            dt.Columns.Add("NAME");
            dt.Rows.Add(MAC, MAC);
            dt.Rows.Add(IP, IP);
            return dt;
        }

        private void KhoiTaoGridTruyCap()
        {
            dtTruyCap = new DataTable();
            dtTruyCap.Columns.Add("STT", typeof(int));
            dtTruyCap.Columns.Add("DIA_CHI", typeof(string));
            dtTruyCap.Columns.Add("LOAI_DIA_CHI", typeof(string));
            dtTruyCap.Columns.Add("KICH_HOAT", typeof(bool));
        }

        private void LoadGridTruyCap()
        {
            if (luuAction == DatabaseConstant.Action.SUA || luuAction == DatabaseConstant.Action.XEM)
            {
                List<HT_TRUY_CAP> lstTruyCap = new List<HT_TRUY_CAP>();
                lstTruyCap = qtht.layTruyCapTheoNhomNSD(obj);
                if (lstTruyCap != null && lstTruyCap.Count > 0)
                {
                    for (int i = 0; i < lstTruyCap.Count; i++)
                    {
                        DataRow dr = dtTruyCap.NewRow();
                        dr["STT"] = i + 1;
                        dr["DIA_CHI"] = lstTruyCap[i].DIA_CHI;
                        dr["LOAI_DIA_CHI"] = lstTruyCap[i].LOAI_DIA_CHI;

                        if (lstTruyCap[i].KICH_HOAT.Equals(BusinessConstant.CoKhong.CO.layGiaTri()))
                            dr["KICH_HOAT"] = true;
                        else
                            dr["KICH_HOAT"] = false;

                        dtTruyCap.Rows.Add(dr);
                    }
                }

            }

            if (dtTruyCap != null)
            {
                grdTruyCap.DataContext = dtTruyCap.DefaultView;
                ((GridViewComboBoxColumn)grdTruyCap.Columns[3]).ItemsSource = LayMacIP().DefaultView;
            }
        }
        #endregion

        #region Xu ly nghiep vu

        public void onSave()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                if (Validate())
                {
                    QuanTriHeThongProcess process = new QuanTriHeThongProcess();
                    List<string> lstStrIdNSD = (from row in dt.AsEnumerable() select row.Field<string>("ID")).Distinct().ToList();
                    List<int> lstIdNSD = lstStrIdNSD.Select(i => i.StringToInt32()).ToList();
                    ApplicationConstant.ResponseStatus ret = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                    string responseMessage = null;

                    // Nếu là thêm mới
                    if (id == 0)
                    {
                        getObject(DatabaseConstant.Action.THEM);
                        ret = process.ThemNHNSD(ref obj, lstIdNSD, lstTruyCap, ref responseMessage);
                        afterAddNew(ret, obj, responseMessage);
                    }
                    else 
                    {
                        getObject(DatabaseConstant.Action.SUA);
                        ret = process.SuaNHNSD(ref obj, lstIdNSD, lstTruyCap, ref responseMessage);
                        afterModify(ret, obj, responseMessage);
                    }
                }
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        public void getObject(DatabaseConstant.Action action)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                if (obj == null)
                    obj = new HT_NHNSD();
                //obj.MA_DVI_QLY = txtMaDonVi.Text;
                obj.MA_DVI_QLY = lstSourceDonVi.ElementAt(cmbDonVi.SelectedIndex).KeywordStrings.First();
                obj.MA_DVI_TAO = ClientInformation.MaDonVi;
                obj.MA_NHNSD = txtMaNhom.Text;
                obj.MO_TA = txtMoTa.Text;
                if (chkHoatDong.IsChecked == true)
                    obj.HAN_CHE_TRUY_CAP = BusinessConstant.CoKhong.CO.layGiaTri();
                else
                    obj.HAN_CHE_TRUY_CAP = BusinessConstant.CoKhong.KHONG.layGiaTri();
                obj.NGAY_CNHAT = (DateTime.Today).ToString("yyyyMMdd");
                if (obj.ID == 0)
                    obj.NGAY_NHAP = (DateTime.Today).ToString("yyyyMMdd");
                obj.NGUOI_CNHAT = ClientInformation.TenDangNhap;
                if (obj.ID == 0)
                    obj.NGUOI_NHAP = ClientInformation.TenDangNhap;
                obj.TEN_NHNSD = txtTenNhom.Text;
                string strTTNV = string.Empty;
                if (obj.ID == 0)
                {
                    obj.NGUON_TAO_DL = "NSD";
                    obj.PVI_KTHAC = "K";
                    strTTNV = obj.TTHAI_NVU;
                }
                obj.TTHAI_NVU = CommonFunction.LayTrangThaiBanGhi(action, BusinessConstant.layTrangThaiNghiepVu(strTTNV));
                obj.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();

                // Luôn là đã duyệt (???)
                obj.TTHAI_NVU = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();

                //Lấy thông tin hạn chế truy cập MAC or IP
                lstTruyCap = new List<HT_TRUY_CAP>();
                foreach (DataRow dr in dtTruyCap.Rows)
                {
                    if (!dr["STT"].ToString().IsNullOrEmptyOrSpace())
                    {
                        HT_TRUY_CAP objTruyCap = new HT_TRUY_CAP();
                        objTruyCap.ID_DTUONG = obj.ID;
                        objTruyCap.MA_DTUONG = obj.MA_NHNSD;
                        objTruyCap.LOAI_DTUONG = BusinessConstant.LoaiDoiTuong.NHOM_NGUOI_SDUNG.layGiaTri();

                        if (Convert.ToBoolean(dr["KICH_HOAT"]) == true)
                            objTruyCap.KICH_HOAT = BusinessConstant.CoKhong.CO.layGiaTri();
                        else
                            objTruyCap.KICH_HOAT = BusinessConstant.CoKhong.KHONG.layGiaTri();

                        objTruyCap.DIA_CHI = dr["DIA_CHI"].ToString().ToUpper();
                        objTruyCap.LOAI_DIA_CHI = dr["LOAI_DIA_CHI"].ToString();

                        lstTruyCap.Add(objTruyCap);
                    }
                }
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void btnThem_Click(object sender, RoutedEventArgs e)
        {
            //Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                lstPopup = new List<DataRow>();
                var process = new PopupProcess();
                List<string> lstDieuKien=new List<string>();
                //lstDieuKien.Add("'" + txtMaDonVi.Text + "'");
                lstDieuKien.Add("'" + lstSourceDonVi.ElementAt(cmbDonVi.SelectedIndex).KeywordStrings.First() + "'");
                process.getPopupInformation(DatabaseConstant.DanhSachTruyVan.POPUP_HT_NSD.getValue(), lstDieuKien);

                SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;

                ucPopup popup = new ucPopup(false, simplePopupResponse, true);
                popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
                Window win = new Window();
                win.Title = LLanguage.SearchResourceByKey(simplePopupResponse.PopupTitle);
                win.Content = popup;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.ShowDialog();
                if (lstPopup != null)
                {
                    List<string> lstID = (from row in dt.AsEnumerable()
                                          select row.Field<string>("ID")).Distinct().ToList();
                    if (dt.Rows.Count == 0)
                    {
                        dt = new DataTable();
                        dt.Columns.Add(LLanguage.SearchResourceByKey("U.DungChung.STT"), typeof(string));
                        dt.Columns.Add("ID", typeof(string));
                        dt.Columns.Add(LLanguage.SearchResourceByKey("U.QuanTriHeThong.NhomNSD.ucNSDCT01.Ma"), typeof(string));
                        dt.Columns.Add(LLanguage.SearchResourceByKey("U.QuanTriHeThong.NhomNSD.ucNSDCT01.Ten"), typeof(string));
                    }
                    foreach (DataRow row in lstPopup)
                    {
                        if (lstID.Contains(row[1]) != true)
                        {
                            DataRow r = dt.NewRow();
                            r[0] = dt.Rows.Count + 1;
                            r[1] = row[1];
                            r[2] = row[2];
                            r[3] = row[3];
                            dt.Rows.Add(r);
                        }
                    }
                    grDanhSach.ItemsSource = null;
                    grDanhSach.ItemsSource = dt;
                    loadWidthColumnDoiTuong();
                }
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            //Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void btnXoa_Click(object sender, RoutedEventArgs e)
        {
            //Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                if (grDanhSach.SelectedItems.Count > 0)
                {
                    List<DataRow> lstRowDel = new List<DataRow>();
                    foreach (DataRow item in grDanhSach.SelectedItems)
                    {
                        DataRow r = dt.AsEnumerable().FirstOrDefault(d => d.Field<string>("ID").Equals(item.Field<string>("ID")));
                        lstRowDel.Add(r);
                    }
                    foreach (DataRow item in lstRowDel)
                    {
                        for (int i = dt.Rows.Count - 1; i >= 0; i--)
                        {
                            if (dt.Rows[i][1].Equals(item[1]))
                            {
                                dt.Rows.RemoveAt(i);
                                break;
                            }
                        }
                    }
                    for (int i = dt.Rows.Count; i > 0; i--)
                    {
                        dt.Rows[i - 1][0] = i.ToString();
                    }
                    grDanhSach.ItemsSource = null;
                    grDanhSach.ItemsSource = dt;
                    loadWidthColumnDoiTuong();
                }
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            //Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void btnPopupDonVi_Click(object sender, RoutedEventArgs e)
        {
            ShowPopupDonVi();
        }

        private void txtMaDonVi_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.F3)
            {
                ShowPopupDonVi();
            }
        }

        private void ShowPopupDonVi()
        {
            try
            {
                lstPopup = new List<DataRow>();
                var process = new PopupProcess();
                process.getPopupInformation(DatabaseConstant.DanhSachTruyVan.POPUP_DM_DONVI.getValue());

                SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;

                ucPopup popup = new ucPopup(false, simplePopupResponse);
                popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
                Window win = new Window();
                win.Title = LLanguage.SearchResourceByKey(simplePopupResponse.PopupTitle);
                win.Content = popup;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.ShowDialog();
                if (lstPopup != null)
                {
                    //if (txtMaDonVi.Text != lstPopup[0][2].ToString())
                    //{
                    //    // Reset source thông tin đối tượng
                    //    dt = new DataTable();
                    //    dt.Columns.Add("STT", typeof(string));
                    //    dt.Columns.Add("ID", typeof(string));
                    //    dt.Columns.Add("Mã", typeof(string));
                    //    dt.Columns.Add("Tên", typeof(string));
                    //    grDanhSach.ItemsSource = dt;
                    //    txtMaDonVi.Text = lstPopup[0][2].ToString();
                    //    lblDonViQL.Content = lstPopup[0][3].ToString();
                    //}
                }
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private bool Validate()
        {
            if (cmbDonVi.SelectedIndex < 0)
            {
                CommonFunction.ThongBaoChuaChon(lblMaDonVi.Content.ToString());
                cmbDonVi.Focus();
                return false;
            }

            if (txtMaNhom.Text.IsNullOrEmptyOrSpace())
            {
                CommonFunction.ThongBaoChuaNhap(lblMaNhom.Content.ToString());
                txtMaNhom.Focus();
                return false;
            }

            if (txtTenNhom.Text.IsNullOrEmptyOrSpace())
            {
                CommonFunction.ThongBaoChuaNhap(lblTenNhom.Content.ToString());
                txtTenNhom.Focus();
                return false;
            }

            if (dtTruyCap != null && dtTruyCap.Rows.Count > 0)
            {
                string message = "";
                string dsMAC = "";
                string dsIP = "";
                foreach (DataRow dr in dtTruyCap.Rows)
                {
                    if (dr["LOAI_DIA_CHI"].ToString().Equals(MAC) && !LSecurity.IsMacAddress(dr["DIA_CHI"].ToString()))
                        dsMAC = dsMAC + " " + dr["STT"].ToString();

                    else if (dr["LOAI_DIA_CHI"].ToString().Equals(IP) && !LSecurity.IsIPv4Address(dr["DIA_CHI"].ToString()))
                        dsIP = dsIP + " " + dr["STT"].ToString();
                }

                if (dsMAC.Length > 0)
                    message = message + "MAC address is not valid. Row number:" + dsMAC + "\n";

                if (dsIP.Length > 0)
                    message = message + "IP address is not valid. Row number:" + dsIP;

                if (message.Length > 0)
                {                    
                    LMessage.ShowMessage(message, LMessage.MessageBoxType.Warning);
                    grdTruyCap.Focus();
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Trước khi sửa
        /// </summary>
        private void beforeModify()
        {
            // Yêu cầu lock bản ghi cần sửa
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(id);

            bool ret = process.LockData(DatabaseConstant.Module.QTHT,
                DatabaseConstant.Function.HT_NHNSD,
                DatabaseConstant.Table.HT_NHNSD,
                DatabaseConstant.Action.SUA,
                listLockId);

            // Nếu lock thành công >> cho phép sửa
            if (ret)
            {
                formCase = "SUA";
                HideControl();
                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.SUA, tthaiNvu, mnuMain, DatabaseConstant.Function.HT_NHNSD);
            }
            else
            {
                LMessage.ShowMessage("M.ResponseMessage.Common.LockDataInvalid", LMessage.MessageBoxType.Information);
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
                // Lock dữ liệu nếu người dùng chấp nhận cảnh báo
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(id);
                List<int> listLockedId = new List<int>();

                bool retLockData = process.LockData(DatabaseConstant.Module.QTHT,
                    DatabaseConstant.Function.HT_NHNSD,
                    DatabaseConstant.Table.HT_NHNSD,
                    DatabaseConstant.Action.XOA,
                    listLockId);

                // Nếu lock thành công >> cho phép sửa
                if (retLockData)
                {
                    // Gọi tới hàm xóa dữ liệu
                    onDelete();
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

        /// <summary>
        /// Xóa dữ liệu
        /// </summary>
        private void onDelete()
        {
            QuanTriHeThongProcess process = new QuanTriHeThongProcess();
            int[] arrayID = new int[0];
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                Array.Resize(ref arrayID, arrayID.Length + 1);
                arrayID[arrayID.Length - 1] = id;

                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = process.XoaListNHNSD(arrayID, ref listClientResponseDetail);

                afterDelete(ret, listClientResponseDetail);
            }
            catch (System.Exception ex)
            {
                this.Cursor = Cursors.Arrow;
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }
        
        /// <summary>
        /// Sau khi thêm mới
        /// </summary>
        /// <param name="ret"></param>
        private void afterAddNew(ApplicationConstant.ResponseStatus ret, HT_NHNSD obj, string responseMessage)
        {
            if (ret == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                LMessage.ShowMessage("M.DungChung.ThemThanhCong", LMessage.MessageBoxType.Information);

                if (cbMultiAdd.IsChecked == true)
                {
                    ResetForm();
                }
                else if (!DatabaseConstant.CLOSE_DETAIL_FORM)
                {
                    id = obj.ID;
                    TthaiNvu = obj.TTHAI_NVU;

                    lblLabelTrangThai.Content = BusinessConstant.layNgonNguSuDung(obj.TTHAI_BGHI);
                    txtTrangThai.Text = BusinessConstant.layNgonNguSuDung(obj.TTHAI_BGHI);
                    raddtNgayNhap.Value = LDateTime.StringToDate(obj.NGAY_NHAP, "yyyyMMdd");
                    txtNguoiLap.Text = obj.NGUOI_NHAP;
                    raddtNgayCNhat.Value = LDateTime.StringToDate(obj.NGAY_CNHAT, "yyyyMMdd");
                    txtNguoiCapNhat.Text = obj.NGUOI_CNHAT;
                    CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, DatabaseConstant.Function.HT_NHNSD);

                    formCase = "XEM";
                    HideControl();
                }
                else
                {
                    onClose();
                }
            }
            else
            {
                LMessage.ShowMessage(responseMessage, LMessage.MessageBoxType.Error);
            }
        }

        /// <summary>
        /// Sau khi sửa
        /// </summary>
        /// <param name="ret"></param>
        private void afterModify(ApplicationConstant.ResponseStatus ret, HT_NHNSD obj, string responseMessage)
        {
            if (ret == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                LMessage.ShowMessage("M.DungChung.CapNhatThanhCong", LMessage.MessageBoxType.Information);

                id = obj.ID;
                TthaiNvu = obj.TTHAI_NVU;

                lblTrangThai.Content = BusinessConstant.layNgonNguSuDung(obj.TTHAI_BGHI);
                txtTrangThai.Text = BusinessConstant.layNgonNguSuDung(obj.TTHAI_BGHI);
                raddtNgayNhap.Value = LDateTime.StringToDate(obj.NGAY_NHAP, "yyyyMMdd");
                txtNguoiLap.Text = obj.NGUOI_NHAP;
                raddtNgayCNhat.Value = LDateTime.StringToDate(obj.NGAY_CNHAT, "yyyyMMdd");
                txtNguoiCapNhat.Text = obj.NGUOI_CNHAT;
                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, tthaiNvu, mnuMain, DatabaseConstant.Function.HT_NHNSD);

                formCase = "XEM";
                HideControl();
            }
            else
            {
                LMessage.ShowMessage(responseMessage, LMessage.MessageBoxType.Error);
            }

            // Yêu cầu Unlock bản ghi cần sửa
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(id);

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.QTHT,
                DatabaseConstant.Function.HT_NHNSD,
                DatabaseConstant.Table.HT_NHNSD,
                DatabaseConstant.Action.SUA,
                listLockId);
        }

        /// <summary>
        /// Sau khi xóa
        /// </summary>
        /// <param name="ret"></param>
        private void afterDelete(bool ret, List<ClientResponseDetail> listClientResponseDetail)
        {
            if (ret)
            {
                LMessage.ShowMessage("M.DungChung.XoaThanhCong", LMessage.MessageBoxType.Information);
            }
            else
            {
                //LMessage.ShowMessage("M.DungChung.XoaKhongThanhCong", LMessage.MessageBoxType.Warning);
                CommonFunction.ThongBaoKetQua(listClientResponseDetail);
            }

            // Yêu cầu unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(id);
            List<int> listUnlockId = new List<int>();

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.QTHT,
                DatabaseConstant.Function.HT_NHNSD,
                DatabaseConstant.Table.HT_NHNSD,
                DatabaseConstant.Action.XOA,
                listLockId);

            // Đóng cửa sổ chi tiết sau khi xóa thành công
            if (ret)
            {
                onClose();
            }
        }

        /// <summary>
        /// Trợ giúp
        /// </summary>
        private void onHelp()
        {
            CustomControl.CommonFunction.ShowHelp(this);
        }

        /// <summary>
        /// Đóng form
        /// </summary>
        private void onClose()
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
            listLockId.Add(id);

            bool ret = process.UnlockData(DatabaseConstant.Module.QTHT,
                DatabaseConstant.Function.HT_NHNSD,
                DatabaseConstant.Table.HT_NHNSD,
                DatabaseConstant.Action.SUA,
                listLockId);
        }

        /// <summary>
        /// Sự kiện unlod cho form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            Release();
            if (OnSavingCompleted != null)
            {
                OnSavingCompleted(this, EventArgs.Empty);
            }
            CustomControl.CommonFunction.CloseUserControl(this);
        }

        #endregion

        
    }
}
