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
using Presentation.Process.TyGiaServiceRef;

namespace PresentationWPF.TyGia.TyGia
{
    /// <summary>
    /// Interaction logic for ucTyGiaCT.xaml
    /// </summary>
    public partial class ucTyGiaCT : UserControl
    {
        #region Khai bao
        public static RoutedCommand HoldCommand = new RoutedCommand();
        public static RoutedCommand SaveCommand = new RoutedCommand();
        public static RoutedCommand ModifyCommand = new RoutedCommand();
        public static RoutedCommand DeleteCommand = new RoutedCommand();
        public static RoutedCommand ApproveCommand = new RoutedCommand();
        public static RoutedCommand RefuseCommand = new RoutedCommand();
        public static RoutedCommand CancelCommand = new RoutedCommand();
        public static RoutedCommand HelpCommand = new RoutedCommand();
        public static RoutedCommand CloseCommand = new RoutedCommand();
        private List<AutoCompleteEntry> lstSourceHinhThucNiemYet = new List<AutoCompleteEntry>();
        private List<AutoCompleteEntry> lstSourceLoaiTyGia = new List<AutoCompleteEntry>();
        private List<AutoCompleteEntry> lstSourceLoaiTien = new List<AutoCompleteEntry>();
        private string tThaiNVu;
        private int idTienTe;
        private List<int> idTienTeCT = new List<int>();
        private TY_GIA_CT objTYGIA = new TY_GIA_CT();
        public DatabaseConstant.Action action;
        
        #endregion

        #region Khoi tao
        public ucTyGiaCT()
        {
            InitializeComponent();
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.TyGia;component/TyGia/ucTyGiaCT.xaml", ref Toolbar, ref mnuMain);
            foreach (var item in mnuMain.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += btnShortcutKey_Click;
            }
            BindShortkey();
            KhoiTaoComboBox();
            InitEventHanler();
            ClearForm();
        }        

        private void KhoiTaoComboBox()
        {
            List<string> lstDieuKien = new List<string>();
            AutoComboBox auto = new AutoComboBox();
            string maTruyVan = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();
            lstDieuKien.Add(DatabaseConstant.getValue(DatabaseConstant.DanhMuc.HINH_THUC_NIEM_YET));
            auto.GenAutoComboBox(ref lstSourceHinhThucNiemYet, ref cbbHinhThucNiemYet, maTruyVan, lstDieuKien, "TRUC_TIEP");
            lstDieuKien.Clear();
            lstDieuKien.Add(DatabaseConstant.getValue(DatabaseConstant.DanhMuc.THI_TRUONG_TIEN_TE));
            auto.GenAutoComboBox(ref lstSourceLoaiTyGia, ref cbbLoaiTyGia, maTruyVan, lstDieuKien, "LIEN_NGAN_HANG");
            maTruyVan = DatabaseConstant.DanhSachTruyVan.COMBOBOX_LOAITIEN.getValue();
            lstDieuKien.Clear();
            auto.GenAutoComboBox(ref lstSourceLoaiTien, ref cbbMaTienTe, maTruyVan, lstDieuKien, "USD");
        }

        private void InitEventHanler()
        {
            btnThemTyGia.Click += new RoutedEventHandler(btnThemTyGia_Click);
            btnSuaTyGia.Click += new RoutedEventHandler(btnSuaTyGia_Click);
            btnXoaTyGia.Click += new RoutedEventHandler(btnXoaTyGia_Click);
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
                        key = new KeyBinding(DeleteCommand, keyg);
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
            e.CanExecute = true;
        }
        private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
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
            tlbDelete.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
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

        }

        private void CloseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void CloseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            CustomControl.CommonFunction.CloseUserControl(this);
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
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRO_GIUP)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DONG)))
            {
                CustomControl.CommonFunction.CloseUserControl(this);
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
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRO_GIUP)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DONG)))
            {
                CustomControl.CommonFunction.CloseUserControl(this);
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
            PresentationWPF.CustomControl.CommonFunction.CloseUserControl(e, this);
            PresentationWPF.CustomControl.CommonFunction.SelectNextControl(e);
        }

        private void ClearForm()
        {
            tThaiNVu = "";
            lblTrangThai.Content = "";
            idTienTe = 0;
            idTienTeCT.Clear();
            raddtNgayApDung.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, ApplicationConstant.defaultDateTimeFormat);
            txtTyGiaMua.Value = 0;
            txtTyGiaBan.Value = 0;
            txtTyGiaBinhQuan.Value = 0;
            raddgrDSTyGia.ItemsSource = null;
            raddgrDSTyGia.Rebind();
            cbbMaTienTe.Focus();
        }

        void btnXoaTyGia_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        void btnSuaTyGia_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        void btnThemTyGia_Click(object sender, RoutedEventArgs e)
        {
            AutoCompleteEntry auLoaiTyGia = lstSourceLoaiTyGia.ElementAt(cbbLoaiTyGia.SelectedIndex);
            AutoCompleteEntry auHinhThucTyGia = lstSourceHinhThucNiemYet.ElementAt(cbbHinhThucNiemYet.SelectedIndex);
            AutoCompleteEntry auLoaiTien = lstSourceLoaiTien.ElementAt(cbbMaTienTe.SelectedIndex);
            DANH_SACH_TY_GIA_CT objTyGiaCT = new DANH_SACH_TY_GIA_CT();
            List<DANH_SACH_TY_GIA_CT> lstTyGiaCT = new List<DANH_SACH_TY_GIA_CT>();
            objTyGiaCT.ID_TY_GIA_CT = 0;
            objTyGiaCT.LOAI_TY_GIA = auLoaiTyGia.KeywordStrings.FirstOrDefault();
            objTyGiaCT.HINH_THUC_NIEM_YET = auHinhThucTyGia.KeywordStrings.FirstOrDefault();
            objTyGiaCT.MA_DVI_QLY = ClientInformation.MaDonViQuanLy;
            objTyGiaCT.MA_DVI_TAO = ClientInformation.MaDonViGiaoDich;
            objTyGiaCT.MA_LOAI_TIEN = auLoaiTien.KeywordStrings.FirstOrDefault();
            objTyGiaCT.NGAY_AP_DUNG = LDateTime.DateToString(raddtNgayApDung.Value.GetValueOrDefault(), ApplicationConstant.defaultDateTimeFormat);
            objTyGiaCT.NGAY_NHAP = ClientInformation.NgayLamViecHienTai;
            objTyGiaCT.NGUOI_NHAP = ClientInformation.TenDangNhap;
            objTyGiaCT.TEN_LOAI_TIEN = auLoaiTien.DisplayName;
            objTyGiaCT.TTHAI_DL = "T";
            objTyGiaCT.TY_GIA_BAN = (decimal)txtTyGiaBan.Value.GetValueOrDefault();
            objTyGiaCT.TY_GIA_BQUAN = (decimal)txtTyGiaBinhQuan.Value.GetValueOrDefault();
            objTyGiaCT.TY_GIA_MUA = (decimal)txtTyGiaMua.Value.GetValueOrDefault();
            objTyGiaCT.TRANG_THAI_BAN_GHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
            objTyGiaCT.TRANG_THAI_NGHIEP_VU = BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri();
            if (LObject.IsNullOrEmpty(raddgrDSTyGia.ItemsSource))
                lstTyGiaCT.Add(objTyGiaCT);
            else
            {
                lstTyGiaCT = raddgrDSTyGia.ItemsSource as List<DANH_SACH_TY_GIA_CT>;
                int icount = lstTyGiaCT.Where(f => f.HINH_THUC_NIEM_YET == objTyGiaCT.HINH_THUC_NIEM_YET && !f.TRANG_THAI_NGHIEP_VU.Equals(BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri())).Count();
                if (icount > 0)
                {
                    return;
                }
                else
                    lstTyGiaCT.Add(objTyGiaCT);
            }
            raddgrDSTyGia.ItemsSource = lstTyGiaCT;
            raddgrDSTyGia.Rebind();
        }
        #endregion               

        #region Xy ly Nghiep Vu

        bool Validation()
        {
            bool bReturn = true;
            try
            {
                
            }
            catch (System.Exception ex)
            {
                bReturn = false;
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            return bReturn;
        }

        void GetDataForm(BusinessConstant.TrangThaiBanGhi bghi, BusinessConstant.TrangThaiNghiepVu nghiepvu)
        {

            try
            {
                AutoCompleteEntry auLoaiTien = lstSourceLoaiTien.ElementAt(cbbMaTienTe.SelectedIndex);
                if (LObject.IsNullOrEmpty(objTYGIA))
                    objTYGIA = new TY_GIA_CT();
                List<DANH_SACH_TY_GIA_CT> lst = raddgrDSTyGia.ItemsSource as List<DANH_SACH_TY_GIA_CT>;
                objTYGIA.DSACH_TY_GIA_CT = lst.ToArray();
                objTYGIA.ID_TY_GIA = idTienTe;
                objTYGIA.MA_LOAI_TIEN = auLoaiTien.KeywordStrings.FirstOrDefault();
                objTYGIA.NGAY_AP_DUNG = ClientInformation.NgayLamViecHienTai;
                idTienTeCT = lst.Select(f => f.ID_TY_GIA_CT).ToList();
                
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }

        }

        void BeforeSave(BusinessConstant.TrangThaiNghiepVu nghiepvu, BusinessConstant.TrangThaiBanGhi bghi)
        {
            Cursor = Cursors.Wait;
            cbbMaTienTe.Focus();
            try
            {
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
                List<ClientResponseDetail> lstResponseDetail = new List<ClientResponseDetail>();
                bool kq;
                if (idTienTe == 0)
                    kq = new DanhMucProcess().TyGia(DatabaseConstant.Action.THEM, ref objTYGIA, ref lstResponseDetail);
                else
                    kq = new DanhMucProcess().TyGia(DatabaseConstant.Action.SUA, ref objTYGIA, ref lstResponseDetail);
                AfterSave(lstResponseDetail, kq);
            }
            catch (System.Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }           
        }
        void AfterSave(List<ClientResponseDetail> lstResponseDetail, bool kq)
        {
            try
            {
                CommonFunction.ThongBaoKetQua(lstResponseDetail);
                // Yêu cầu Unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                bool retLockData = process.UnlockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.DC_TY_GIA,
                DatabaseConstant.Table.DC_TY_GIA,
                DatabaseConstant.Action.SUA,
                idTienTeCT);
                action = DatabaseConstant.Action.XEM;
                if (!cbMultiAdd.IsChecked.GetValueOrDefault(false))
                {
                    if (kq)
                        SetInfomation();
                }
                else
                {
                    if (kq)
                        ClearForm();
                }
            }
            catch (System.Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }  
        }

        void SetInfomation()
        {
            try
            {

            }
            catch (System.Exception ex)
            {
            	
            }
        }
        #endregion
    }
}
