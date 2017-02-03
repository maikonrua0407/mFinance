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
using Telerik.Windows.Controls.GridView;
using Utilities.Common;
using Presentation.Process;
using Presentation.Process.Common;
using PresentationWPF.CustomControl;
using Presentation.Process.PopupServiceRef;
using Presentation.Process.NhanSuServiceRef;
using Presentation.Process.BaoCaoServiceRef;

namespace PresentationWPF.NhanSu.PhuCapCongTacVien
{
    /// <summary>
    /// Interaction logic for ucTieuChiPhuCap.xaml
    /// </summary>
    public partial class ucTieuChiPhuCap : UserControl
    {
        #region Khai bao
        public event EventHandler OnSavingCompleted;

        private int id;
        public int ID
        {
            get { return id; }
            set { id = value; }
        }

        private string trangThaiNVu = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();

        private string loaiPhuCap = "";

        private NS_TIEU_CHI_PHU_CAP obj;

        private DataSet dsCoDinh = null;
        private DataSet dsBoSung = null;

        public static RoutedCommand HoldCommand = new RoutedCommand();
        public static RoutedCommand SaveCommand = new RoutedCommand();
        public static RoutedCommand ModifyCommand = new RoutedCommand();
        public static RoutedCommand DeleteCommand = new RoutedCommand();
        public static RoutedCommand ApproveCommand = new RoutedCommand();
        public static RoutedCommand RefuseCommand = new RoutedCommand();
        public static RoutedCommand CancelCommand = new RoutedCommand();
        public static RoutedCommand HelpCommand = new RoutedCommand();
        public static RoutedCommand CloseCommand = new RoutedCommand();

        List<AutoCompleteEntry> lstSourceLoaiPhuCap = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourcePhuCapCho = new List<AutoCompleteEntry>();
        #endregion

        #region Khoi tao
        public ucTieuChiPhuCap()
        {
            InitializeComponent();

            DuyetQuyenTinhNang();

            BindShortkey();

            SetVisibled();

            LoadCombobox();

            LoadDuLieu();
        }

        private void LoadCombobox()
        {
            COMBOBOX_DTO combo = null;
            List<string> lstDieuKien = null;
            List<COMBOBOX_DTO> lstCombobox = new List<COMBOBOX_DTO>();

            //Loại phụ cấp
            lstDieuKien = new List<string>();
            lstDieuKien.Add("LOAI_PHU_CAP");
            combo = new COMBOBOX_DTO();
            combo.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC_NHAN_SU.getValue();
            combo.combobox = cmbLoaiPhuCap;
            combo.lstSource = lstSourceLoaiPhuCap;
            combo.lstDieuKien = lstDieuKien;
            lstCombobox.Add(combo);

            //Phụ cấp cho
            combo = new COMBOBOX_DTO();
            combo.maCSo = DatabaseConstant.DanhSachTruyVan.COMBOBOX_NS_NHOM_CTV.getValue();
            combo.combobox = cmbPhuCapCho;
            combo.lstSource = lstSourcePhuCapCho;
            combo.lstDieuKien = lstDieuKien;
            lstCombobox.Add(combo);

            AutoComboBox auto = new AutoComboBox();
            auto.GenAutoComboBoxTheoList(ref lstCombobox);

            cmbLoaiPhuCap.SelectedIndex = 0;
        }

        private void DuyetQuyenTinhNang()
        {
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.NhanSu;component/PhuCapCongTacVien/ucTieuChiPhuCap.xaml", ref Toolbar, ref mnuMain);
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
            OnSave();
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

            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU)))
            {
                OnSave();
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

            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU)))
            {
                OnSave();
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

            bool ret = process.UnlockData(DatabaseConstant.Module.NSTL, 
                DatabaseConstant.Function.NS_BANG_LUONG,                 
                DatabaseConstant.Table.NS_BAC_LUONG,
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

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            dsBoSung.Tables[0].Rows.Add(dsBoSung.Tables[0].Rows.Count + 1, null, null, 0, 0);
            grBoSung.DataContext = dsBoSung.Tables[0].DefaultView;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            List<int> lstSTT = new List<int>();
            for (int i = 0; i < grBoSung.SelectedItems.Count; i++)
            {
                DataRowView dr = (DataRowView)grBoSung.SelectedItems[i];
                lstSTT.Add(Convert.ToInt32(dr["STT"]));
            }
            lstSTT.SortByDesc();
            foreach (int stt in lstSTT)
                dsBoSung.Tables[0].Rows.RemoveAt(stt - 1);

            for (int i = 0; i < dsBoSung.Tables[0].Rows.Count; i++)
            {
                dsBoSung.Tables[0].Rows[i]["STT"] = i + 1;
            }

            grBoSung.DataContext = dsBoSung.Tables[0].DefaultView;
        }

        private void cmbLoaiPhuCap_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbLoaiPhuCap.SelectedIndex >= 0)
            {
                loaiPhuCap = lstSourceLoaiPhuCap.ElementAt(cmbLoaiPhuCap.SelectedIndex).KeywordStrings.ElementAt(0);

                if (loaiPhuCap.Equals(BusinessConstant.LOAI_PHU_CAP.CO_DINH.layGiaTri()))
                {
                    cmbPhuCapCho.SelectedIndex = -1;
                    cmbPhuCapCho.IsEnabled = false;

                    grCoDinh.Visibility = System.Windows.Visibility.Visible;
                    gridBoSung.Visibility = System.Windows.Visibility.Collapsed;
                }
                else if (loaiPhuCap.Equals(BusinessConstant.LOAI_PHU_CAP.BO_SUNG.layGiaTri()))
                {
                    cmbPhuCapCho.SelectedIndex = 0;
                    cmbPhuCapCho.IsEnabled = true;

                    grCoDinh.Visibility = System.Windows.Visibility.Collapsed;
                    gridBoSung.Visibility = System.Windows.Visibility.Visible;
                }

                LoadDuLieu();
            }

        }

        private void cmbPhuCapCho_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbPhuCapCho.SelectedIndex >= 0)
            {
                LoadDuLieu();
            }
        }

        #endregion

        #region Xử lý nghiệp vụ
        private void GetFormData(ref NS_TIEU_CHI_PHU_CAP obj)
        {
            try
            {
                obj = new NS_TIEU_CHI_PHU_CAP();

                obj.LOAI_PHU_CAP = loaiPhuCap;
                obj.MA_DVI = ClientInformation.MaDonVi;
                obj.MA_DVI_GDICH = ClientInformation.MaDonViGiaoDich;
                obj.TRANG_THAI_BAN_GHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                obj.TRANG_THAI_NGHIEP_VU = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();
                obj.NGAY_LAP = ClientInformation.NgayLamViecHienTai;
                obj.NGUOI_LAP = ClientInformation.TenDangNhap;
                obj.NGAY_CAP_NHAT = ClientInformation.NgayLamViecHienTai;
                obj.NGUOI_CAP_NHAT = ClientInformation.TenDangNhap;

                if(loaiPhuCap.Equals(BusinessConstant.LOAI_PHU_CAP.CO_DINH.layGiaTri()))
                {
                    List<NS_PHU_CAP_CDINH_CTV> lst = new List<NS_PHU_CAP_CDINH_CTV>();
                    NS_PHU_CAP_CDINH_CTV objCoDinh = null;
                    foreach (DataRow dr in dsCoDinh.Tables[0].Rows)
                    {                        
                        objCoDinh = new NS_PHU_CAP_CDINH_CTV();

                        objCoDinh.ID_CHUC_VU_CTV = Convert.ToInt32(dr["ID_CHUC_VU"]);
                        objCoDinh.SO_TIEN = Convert.ToDecimal(dr["SO_TIEN"]);
                        objCoDinh.MA_DVI_QLY = obj.MA_DVI;
                        objCoDinh.MA_DVI_TAO = obj.MA_DVI_GDICH;
                        objCoDinh.TTHAI_BGHI = obj.TRANG_THAI_BAN_GHI;
                        objCoDinh.TTHAI_NVU = obj.TRANG_THAI_NGHIEP_VU;
                        objCoDinh.NGAY_NHAP = obj.NGAY_LAP;
                        objCoDinh.NGUOI_NHAP = obj.NGUOI_LAP;
                        objCoDinh.NGAY_CNHAT = obj.NGAY_CAP_NHAT;
                        objCoDinh.NGUOI_CNHAT = obj.NGUOI_CAP_NHAT;

                        lst.Add(objCoDinh);
                    }

                    obj.LST_PHU_CAP_CDINH = lst.ToArray();
                }
                else if(loaiPhuCap.Equals(BusinessConstant.LOAI_PHU_CAP.BO_SUNG.layGiaTri()))
                {
                    obj.ID_NHOM_CTV = Convert.ToInt32(lstSourcePhuCapCho.ElementAt(cmbPhuCapCho.SelectedIndex).KeywordStrings.ElementAt(1));
                    obj.MA_NHOM_CTV = lstSourcePhuCapCho.ElementAt(cmbPhuCapCho.SelectedIndex).KeywordStrings.ElementAt(0);

                    List<NS_PHU_CAP_BSUNG_CTV> lst = new List<NS_PHU_CAP_BSUNG_CTV>();
                    NS_PHU_CAP_BSUNG_CTV objBoSung = null;
                    foreach (DataRow dr in dsBoSung.Tables[0].Rows)
                    {
                        objBoSung = new NS_PHU_CAP_BSUNG_CTV();

                        objBoSung.ID_NHOM_CTV = obj.ID_NHOM_CTV;
                        objBoSung.TU = Convert.ToInt32(dr["TU"]);
                        objBoSung.DEN = Convert.ToInt32(dr["DEN"]);
                        objBoSung.MUC1 = Convert.ToDecimal(dr["MUC1"]);
                        objBoSung.MUC2 = Convert.ToDecimal(dr["MUC2"]);
                        objBoSung.MUC3 = Convert.ToDecimal(dr["MUC3"]);
                        objBoSung.MA_DVI_QLY = obj.MA_DVI;
                        objBoSung.MA_DVI_TAO = obj.MA_DVI_GDICH;
                        objBoSung.TTHAI_BGHI = obj.TRANG_THAI_BAN_GHI;
                        objBoSung.TTHAI_NVU = obj.TRANG_THAI_NGHIEP_VU;
                        objBoSung.NGAY_NHAP = obj.NGAY_LAP;
                        objBoSung.NGUOI_NHAP = obj.NGUOI_LAP;
                        objBoSung.NGAY_CNHAT = obj.NGAY_CAP_NHAT;
                        objBoSung.NGUOI_CNHAT = obj.NGUOI_CAP_NHAT;

                        lst.Add(objBoSung);
                    }

                    obj.LST_PHU_CAP_BSUNG = lst.ToArray();

                }

            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        private void LoadDuLieu()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                NhanSuProcess processNhanSu = new NhanSuProcess();
                DataTable dt = null;
                LDatatable.MakeParameterTable(ref dt);
                LDatatable.AddParameter(ref dt, "@MA_DVI", "STRING", ClientInformation.MaDonViQuanLy);
                if (loaiPhuCap.Equals(BusinessConstant.LOAI_PHU_CAP.CO_DINH.layGiaTri()))
                {
                    dsCoDinh = processNhanSu.GetPhuCapCoDinh(dt);
                    if (dsCoDinh != null && dsCoDinh.Tables.Count > 0)
                    {
                        grCoDinh.DataContext = dsCoDinh.Tables[0].DefaultView;
                    }
                    else
                    {
                        grCoDinh.DataContext = null;
                    }
                }
                else if (loaiPhuCap.Equals(BusinessConstant.LOAI_PHU_CAP.BO_SUNG.layGiaTri()))
                {
                    if (cmbPhuCapCho.SelectedIndex >= 0)
                    {
                        int idNhomCTV = Convert.ToInt32(lstSourcePhuCapCho.ElementAt(cmbPhuCapCho.SelectedIndex).KeywordStrings.ElementAt(1));
                        
                        LDatatable.MakeParameterTable(ref dt);
                        LDatatable.AddParameter(ref dt, "@MA_DVI", "STRING", ClientInformation.MaDonViQuanLy);
                        LDatatable.AddParameter(ref dt, "@ID_NHOM_CTV", "STRING", idNhomCTV.ToString());
                        dsBoSung = processNhanSu.GetPhuCapBoSung(dt);
                        if (dsBoSung != null && dsBoSung.Tables.Count > 0)
                        {
                            grBoSung.DataContext = dsBoSung.Tables[0].DefaultView;
                        }
                        else
                        {
                            grBoSung.DataContext = null;
                        }
                    }
                    else
                    {
                        grBoSung.DataContext = null;
                    }
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);                
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private bool Validation()
        {
            try
            {
                if (cmbLoaiPhuCap.SelectedIndex < 0)
                {
                    CommonFunction.ThongBaoChuaChon(lblLoaiPhuCap.Content.ToString());
                    cmbLoaiPhuCap.Focus();
                    return false;
                }

                if (loaiPhuCap.Equals(BusinessConstant.LOAI_PHU_CAP.BO_SUNG.layGiaTri()))
                {
                    if (cmbPhuCapCho.SelectedIndex < 0)
                    {
                        CommonFunction.ThongBaoChuaChon(lblPhuCapCho.Content.ToString());
                        cmbPhuCapCho.Focus();
                        return false;
                    }

                    string message = "";
                    if (KiemTraGridBoSung(ref message) == false)
                    {
                        LMessage.ShowMessage(message, LMessage.MessageBoxType.Warning);
                        return false;
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

        private bool KiemTraGridBoSung(ref string message)
        {
            bool kq = true;

            if (!loaiPhuCap.Equals(BusinessConstant.LOAI_PHU_CAP.BO_SUNG.layGiaTri())) return true;            

            message = "Không hợp lệ các dòng:";
            foreach (DataRow dr in dsBoSung.Tables[0].Rows)
            {
                if (!dr["TU"].ToString().IsNumeric())
                {
                    message = message + " " + dr["STT"].ToString();
                    kq = false;
                    continue;
                }
                if (!dr["DEN"].ToString().IsNumeric())
                {
                    message = message + " " + dr["STT"].ToString();
                    kq = false;
                    continue;
                }
                if (!dr["MUC1"].ToString().IsNumeric())
                {
                    message = message + " " + dr["STT"].ToString();
                    kq = false;
                    continue;
                }
                if (!dr["MUC2"].ToString().IsNumeric())
                {
                    message = message + " " + dr["STT"].ToString();
                    kq = false;
                    continue;
                }
                if (!dr["MUC3"].ToString().IsNumeric())
                {
                    message = message + " " + dr["STT"].ToString();
                    kq = false;
                    continue;
                }

                double tu = Convert.ToDouble(dr["TU"]);
                double den = Convert.ToDouble(dr["DEN"]);
                if (tu >= den)
                {
                    message = message + " " + dr["STT"].ToString();
                    kq = false;
                    continue;
                }
                
            }

            return kq;
        }

        private void SetVisibled()
        {
       
        }

        public void OnSave()
        {
            try
            {
                if (!Validation()) return;
                
                GetFormData(ref obj);

                NhanSuProcess processNhanSu = new NhanSuProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = false;

                Mouse.OverrideCursor = Cursors.Wait;
                ret = processNhanSu.TieuChiPhuCap(DatabaseConstant.Action.LUU, ref obj, ref listClientResponseDetail);
                Mouse.OverrideCursor = Cursors.Arrow;

                AfterSave(ret, listClientResponseDetail);

            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        public void AfterSave(bool ret, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (ret)
                {
                    LMessage.ShowMessage("M.DungChung.ThemThanhCong", LMessage.MessageBoxType.Information);
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
    }
}
