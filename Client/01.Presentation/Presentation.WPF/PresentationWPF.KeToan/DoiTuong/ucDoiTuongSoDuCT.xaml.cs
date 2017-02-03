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
using Utilities.Common;
using System.Data;
using Microsoft.Windows.Controls.Ribbon;
using Presentation.Process;
using Presentation.Process.Common;
using Presentation.Process.PopupServiceRef;
using Presentation.Process.KeToanServiceRef;

namespace PresentationWPF.KeToan.DoiTuong
{
    /// <summary>
    /// Interaction logic for ucDoiTuongSoDuCT.xaml
    /// </summary>
    public partial class ucDoiTuongSoDuCT : UserControl
    {

        #region Khai bao
        // các thao tác trên Form: thêm, sửa, xem... (để mặc định là thêm)
        private DatabaseConstant.Action action = DatabaseConstant.Action.THEM;
        public DatabaseConstant.Action Action
        {
            get { return action; }
            set { action = value; }
        }

        public event EventHandler OnSavingCompleted;

        List<DataRow> lstPopup = new List<DataRow>();
        public void LayDuLieuTuPopup(List<DataRow> lst)
        {
            lstPopup = lst;
        }
        private string trangThaiNVu = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();
        private string TTHAI_BGHI;
        private string TTHAI_NVU;
        private string MA_DVI_QLY;
        private string MA_DVI_TAO;
        private string NGAY_NHAP;
        private string NGUOI_NHAP;
        private int ID_TKHOAN;
        private string MA_LSDU;
        private DateTime ngayChotDL;
        private DOI_TUONG_SDU_TKHOAN _obj = new DOI_TUONG_SDU_TKHOAN();
        private List<DOI_TUONG_SDU_TKHOAN_CT> lstDTuongCT = new List<DOI_TUONG_SDU_TKHOAN_CT>();

        List<AutoCompleteEntry> lstSourceDoiTuongLoai = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceNguonVon = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceDonVi = new List<AutoCompleteEntry>();

        public static RoutedCommand HoldCommand = new RoutedCommand();
        public static RoutedCommand SaveCommand = new RoutedCommand();
        public static RoutedCommand ModifyCommand = new RoutedCommand();
        public static RoutedCommand DeleteCommand = new RoutedCommand();
        public static RoutedCommand ApproveCommand = new RoutedCommand();
        public static RoutedCommand RefuseCommand = new RoutedCommand();
        public static RoutedCommand CancelCommand = new RoutedCommand();
        public static RoutedCommand HelpCommand = new RoutedCommand();
        public static RoutedCommand CloseCommand = new RoutedCommand();
        #endregion

        #region Khoitao
        public ucDoiTuongSoDuCT()
        {
            InitializeComponent();
            InitComboBox();
            InitEventHanler();
        }

        private void InitEventHanler()
        {
            btnSoTaiKhoan.Click += new RoutedEventHandler(btnSoTaiKhoan_Click);
            tlbDetailAdd.Click += new RoutedEventHandler(tlbDetailAdd_Click);
            tlbDetailDelete.Click += new RoutedEventHandler(tlbDetailDelete_Click);
            TCHAT_SDU.EditCellEnd += new EventHandler(TCHAT_SDU_EditCellEnd);
        }


        private void InitComboBox()
        {
            AutoComboBox auto = new AutoComboBox();
            List<string> lstDieuKien = new List<string>();
            lstDieuKien.Add(ClientInformation.MaDonVi);
            auto.GenAutoComboBox(ref lstSourceDoiTuongLoai, ref cmbLoaiDoiTuong, "COMBOBOX_LOAI_DOI_TUONG_THEO_BANG", lstDieuKien);

            auto.GenAutoComboBox(ref lstSourceNguonVon, ref cmbNguonVon, DatabaseConstant.DanhSachTruyVan.COMBOBOX_NGUON_VON.getValue(), null);
            cmbNguonVon.SelectionChanged += new SelectionChangedEventHandler(cmbNguonVon_SelectionChanged);
            cmbLoaiDoiTuong.SelectionChanged += new SelectionChangedEventHandler(cmbLoaiDoiTuong_SelectionChanged);
        }

        void cmbLoaiDoiTuong_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbLoaiDoiTuong.SelectedIndex > -1)
            {
                tlbDetailAdd.IsEnabled = true;
                tlbDetailDelete.IsEnabled = true;
            }
            else
            {
                tlbDetailAdd.IsEnabled = false;
                tlbDetailDelete.IsEnabled = false;
            }
        }

        private void DuyetQuyenTinhNang()
        {
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.KeToan;component/DoiTuong/ucDoiTuongCT.xaml", ref Toolbar, ref mnuMain);
            foreach (var item in mnuMain.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += btnShortcutKey_Click;
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
            e.CanExecute = tlbSave.IsEnabled;
        }
        private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OnSave();
        }

        private void ModifyCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbModify.IsEnabled;
        }
        private void ModifyCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OnModify();
        }

        private void DeleteCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void DeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
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
            RibbonButton tlb = (RibbonButton)sender;
            string strTinhNang = tlb.Name.Substring(3, tlb.Name.Length - 3);
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU_TAM)))
            {
                
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU)))
            {
                OnSave();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
                OnModify();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
            {
                
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XEM_TRUOC)))
            {

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
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
                OnModify();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
            {
                
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XEM_TRUOC)))
            {
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

        /// <summary>
        /// Trợ giúp
        /// </summary>
        private void OnHelp()
        {
            PresentationWPF.CustomControl.CommonFunction.ShowHelp(this);
        }

        /// <summary>
        /// Đóng form
        /// </summary>
        private void OnClose()
        {
            Release();
            PresentationWPF.CustomControl.CommonFunction.CloseUserControl(this);
        }

        /// <summary>
        /// Hàm xử lý giải phóng lock dữ liệu cho form
        /// </summary>
        private void Release()
        {
            // Yêu cầu unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            
        }

        private void tlbDetailDelete_Click(object sender, RoutedEventArgs e)
        {
            foreach (DOI_TUONG_SDU_TKHOAN_CT objDTuongCT in raddgrTUngCT.SelectedItems)
            {
                lstDTuongCT.Remove(objDTuongCT);
            }
            raddgrTUngCT.ItemsSource = lstDTuongCT;
            raddgrTUngCT.Rebind();
        }

        private void tlbDetailAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cmbLoaiDoiTuong.SelectedIndex == -1)
                    return;
                DOI_TUONG_SDU_TKHOAN_CT objDTuongCT = null;
                AutoCompleteEntry au = lstSourceDoiTuongLoai.ElementAt(cmbLoaiDoiTuong.SelectedIndex);
                //Bat popup
                PopupProcess process = new PopupProcess();
                List<string> lstDieuKien = new List<string>();
                lstDieuKien.Add(Presentation.Process.Common.ClientInformation.MaDonVi);
                lstDieuKien.Add(au.KeywordStrings.FirstOrDefault());

                process.getPopupInformation(DatabaseConstant.DanhSachTruyVan.POPUP_DS_DOI_TUONG.getValue(), lstDieuKien);
                SimplePopupResponse simplePopupResponse = Presentation.Process.Common.ClientInformation.SimplePopup;

                lstPopup.Clear();
                ucPopup popup = new ucPopup(false, simplePopupResponse, true);
                popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
                Window win = new Window();
                win.Title = LLanguage.SearchResourceByKey("U.KeToan.DoiTuong.ucDoiTuongSoDuCT.DanhSachDoiTuong");
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.Content = popup;
                win.ShowDialog();
                if (lstPopup != null && lstPopup.Count > 0)
                {
                    foreach (DataRow dr in lstPopup)
                    {

                        if (!lstDTuongCT.Select(f => f.MA_DTUONG).Contains(dr[2].ToString()))
                        {
                            objDTuongCT = new DOI_TUONG_SDU_TKHOAN_CT();
                            objDTuongCT.MA_DTUONG = dr[2].ToString();
                            objDTuongCT.TEN_TAI_KHOAN = dr[3].ToString();
                            objDTuongCT.MA_LSDU = MA_LSDU;
                            objDTuongCT.NGAY_DL = ClientInformation.NgayLamViecHienTai;
                            objDTuongCT.NGAY_GDICH = ClientInformation.NgayLamViecHienTai;
                            objDTuongCT.NGAY_HLUC = ClientInformation.NgayLamViecHienTai;
                            objDTuongCT.SODU = 0;
                            objDTuongCT.ID_DTUONG = Convert.ToInt32(dr[1].ToString());
                            lstDTuongCT.Add(objDTuongCT);
                        }
                    }
                    raddgrTUngCT.ItemsSource = lstDTuongCT;
                    raddgrTUngCT.Rebind();
                }
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        private void btnSoTaiKhoan_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Bat popup
                if (cmbNguonVon.SelectedIndex < 0)
                    return;
                PopupProcess process = new PopupProcess();
                List<string> lstDieuKien = new List<string>();
                AutoCompleteEntry au = lstSourceNguonVon.ElementAt(cmbNguonVon.SelectedIndex);
                lstDieuKien.Add("%");
                lstDieuKien.Add("%");
                lstDieuKien.Add(Presentation.Process.Common.ClientInformation.MaDonViGiaoDich);
                lstDieuKien.Add(au.KeywordStrings.FirstOrDefault());
                process.getPopupInformation(DatabaseConstant.DanhSachTruyVan.POPUP_TKHOAN_CTIET.getValue(), lstDieuKien);
                SimplePopupResponse simplePopupResponse = Presentation.Process.Common.ClientInformation.SimplePopup;

                lstPopup.Clear();
                ucPopup popup = new ucPopup(false, simplePopupResponse, false);
                popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
                Window win = new Window();
                win.Title = simplePopupResponse.PopupTitle;
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.Content = popup;
                win.ShowDialog();
                if (lstPopup != null && lstPopup.Count > 0)
                {
                    txtSoTaiKhoan.Text = lstPopup[0][2].ToString();
                    lblTenTaiKhoan.Content = lstPopup[0][3].ToString();
                    numSoDu.Value = Convert.ToDouble(lstPopup[0]["SODU"]);
                    ID_TKHOAN = Convert.ToInt32(lstPopup[0][1]);
                    MA_LSDU = lstPopup[0]["MA_LSDU"].ToString();
                    if (!lstPopup[0]["LOAI_DTUONG"].ToString().IsNullOrEmptyOrSpace())
                    {
                        cmbLoaiDoiTuong.SelectedIndex = lstSourceDoiTuongLoai.IndexOf(lstSourceDoiTuongLoai.FirstOrDefault(f => f.KeywordStrings.FirstOrDefault().Equals(lstPopup[0]["LOAI_DTUONG"].ToString())));
                        cmbLoaiDoiTuong.IsEnabled = false;
                    }
                    else
                    {
                        cmbLoaiDoiTuong.SelectedIndex = -1;
                        cmbLoaiDoiTuong.IsEnabled = true;
                    }
                    if (lstPopup[0]["MA_NHOM_PLOAI"].ToString().Equals("LT"))
                        raddgrTUngCT.Columns["TCHAT_SDU"].IsReadOnly = false;
                    else
                        raddgrTUngCT.Columns["TCHAT_SDU"].IsReadOnly = true;
                    if (lstPopup[0]["NGAY_GDICH"] != DBNull.Value)
                    {
                        telNgayChotDL.Value = LDateTime.StringToDate(lstPopup[0]["NGAY_GDICH"].ToString(), ApplicationConstant.defaultDateTimeFormat);
                        ngayChotDL = LDateTime.StringToDate(lstPopup[0]["NGAY_GDICH"].ToString(), ApplicationConstant.defaultDateTimeFormat);
                    }
                    DataTable dt = null;
                    LDatatable.MakeParameterTable(ref dt);
                    LDatatable.AddParameter(ref dt, "@IdTaiKhoan", "INT", ID_TKHOAN.ToString());
                    DataSet ds = new KeToanProcess().getDanhSachTaiKhoanChiTiet(dt, "DOI_TUONG");
                    lstDTuongCT = new List<DOI_TUONG_SDU_TKHOAN_CT>();
                    if (!ds.IsNullOrEmpty() && !ds.Tables[0].IsNullOrEmpty())
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            DOI_TUONG_SDU_TKHOAN_CT objDTuongCT = new DOI_TUONG_SDU_TKHOAN_CT();
                            objDTuongCT.MA_DTUONG = dr["MA_DTUONG"].ToString();
                            objDTuongCT.TEN_TAI_KHOAN = dr["TEN_DTUONG"].ToString();
                            objDTuongCT.MA_LSDU = dr["MA_LSDU"].ToString();
                            objDTuongCT.NGAY_DL = dr["NGAY_DL"].ToString();
                            objDTuongCT.NGAY_GDICH = dr["NGAY_GDICH"].ToString();
                            objDTuongCT.NGAY_HLUC = dr["NGAY_HLUC"].ToString();
                            objDTuongCT.SODU = Convert.ToDecimal(dr["SO_DU"].ToString());
                            objDTuongCT.ID_DTUONG = Convert.ToInt32(dr["ID"].ToString());
                            lstDTuongCT.Add(objDTuongCT);
                        }
                    }
                    raddgrTUngCT.ItemsSource = lstDTuongCT;
                    raddgrTUngCT.Rebind();
                }
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        void cmbNguonVon_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txtSoTaiKhoan.Text = "";
            numSoDu.Value = 0;
            lblTenTaiKhoan.Content = "";
            cmbLoaiDoiTuong.SelectedIndex = -1;
            lstDTuongCT = new List<DOI_TUONG_SDU_TKHOAN_CT>();
            raddgrTUngCT.ItemsSource = lstDTuongCT;
            raddgrTUngCT.Rebind();
        }

        private void SetEnabledControls(bool bBool)
        {
            txtSoTaiKhoan.IsEnabled = bBool;
            cmbLoaiDoiTuong.IsEnabled = bBool;
            cmbNguonVon.IsEnabled = bBool;
            tlbDetailAdd.IsEnabled = bBool;
            tlbDetailDelete.IsEnabled = bBool;
            raddgrTUngCT.IsReadOnly = !bBool;
        }

        void TCHAT_SDU_EditCellEnd(object sender, EventArgs e)
        {
            DOI_TUONG_SDU_TKHOAN_CT objDTuong = TCHAT_SDU.cellEdit.ParentRow.Item as DOI_TUONG_SDU_TKHOAN_CT;
            lstDTuongCT[lstDTuongCT.IndexOf(objDTuong)].MA_LSDU = TCHAT_SDU.GiaTri;
        }

        #endregion

        #region Xy ly nghiep vu
        private bool Validation()
        {
            try
            {
                if (txtSoTaiKhoan.Text.IsNullOrEmptyOrSpace())
                {
                    CommonFunction.ThongBaoTrong(lblTaiKhoan.Content.ToString());
                    txtSoTaiKhoan.Focus();
                    return false;
                }
                else if (lstDTuongCT.IsNullOrEmpty() || lstDTuongCT.Count < 1)
                {
                    CommonFunction.ThongBaoTrong(lblDSachKheUoc.Content.ToString());
                    tlbDetailAdd.Focus();
                    return false;
                }
                else if (telNgayChotDL.Value.GetValueOrDefault() < ngayChotDL)
                {
                    LMessage.ShowMessage("M.DungChung.DuLieuKhongHopLe",LMessage.MessageBoxType.Error);
                    telNgayChotDL.Focus();
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

        private void GetFormData(ref DOI_TUONG_SDU_TKHOAN obj)
        {
            TAIKHOAN objTK = new TAIKHOAN();
            try
            {
                AutoCompleteEntry au = lstSourceDoiTuongLoai.ElementAt(cmbLoaiDoiTuong.SelectedIndex);
                objTK.SO_TAI_KHOAN = txtSoTaiKhoan.Text;
                objTK.ID = ID_TKHOAN;
                objTK.ID_LOAI_DOI_TUONG = Convert.ToInt32(au.KeywordStrings[1]);
                objTK.LOAI_DOI_TUONG = au.KeywordStrings[0];
                obj.TAI_KHOAN = objTK;
                obj.DSACH_DTUONG_TKHOAN_CT = lstDTuongCT.ToArray();
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        public void BeforeSave()
        {
            action = DatabaseConstant.Action.LUU;
            // Yêu cầu lock bản ghi cần sửa
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(ID_TKHOAN);

            bool ret = process.LockData(DatabaseConstant.Module.GDKT,
            DatabaseConstant.Function.DC_DM_DTUONG_SODU,
            DatabaseConstant.Table.KT_TKHOAN,
                DatabaseConstant.Action.LUU,
                listLockId);
            if (ret)
                GetFormData(ref _obj);
        }

        public void OnSave()
        {
            if (MessageBoxResult.Yes != LMessage.ShowMessage("M.KeToan.HoiTruocKhiLuuDoiTuongSoDu", LMessage.MessageBoxType.Question))
                return;
            Cursor = Cursors.Wait;
            List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
            ApplicationConstant.ResponseStatus iret = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
            try
            {
                if (!Validation()) return;

                _obj = new DOI_TUONG_SDU_TKHOAN();
                GetFormData(ref _obj);
                action = DatabaseConstant.Action.LUU;
                // Yêu cầu lock bản ghi cần sửa
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(ID_TKHOAN);

                bool ret = process.LockData(DatabaseConstant.Module.GDKT,
                DatabaseConstant.Function.DC_DM_DTUONG_SODU,
                DatabaseConstant.Table.KT_TKHOAN,
                    DatabaseConstant.Action.LUU,
                    listLockId);
                iret = new KeToanProcess().DoiTuongSoDuCT(DatabaseConstant.Function.DC_DM_DTUONG_SODU, action, ref _obj, ref listClientResponseDetail);
                AfterSave(iret, listClientResponseDetail);

            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                // Yêu cầu lock bản ghi cần sửa
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                listLockId.Add(ID_TKHOAN);

                bool ret = process.UnlockData(DatabaseConstant.Module.GDKT,
                DatabaseConstant.Function.DC_DM_DTUONG_SODU,
                DatabaseConstant.Table.KT_TKHOAN,
                    DatabaseConstant.Action.LUU,
                    listLockId);
            }
            finally
            {
                Cursor = Cursors.Arrow;
            }
        }

        public void AfterSave(ApplicationConstant.ResponseStatus ret, List<ClientResponseDetail> listClientResponseDetail)
        {
            CommonFunction.ThongBaoKetQua(listClientResponseDetail);
            if(ret == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                TTHAI_NVU = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();
                lblLabelTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(TTHAI_NVU);
                CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.XEM, TTHAI_NVU, mnuMain, DatabaseConstant.Function.DC_DM_DTUONG_SODU);
                SetEnabledControls(false);
            }
        }

        public void OnModify()
        {
            CommonFunction.RefreshButton(Toolbar, DatabaseConstant.Action.SUA, TTHAI_NVU, mnuMain, DatabaseConstant.Function.DC_DM_DTUONG_SODU);
            SetEnabledControls(true);
        }
        #endregion
    }
}
