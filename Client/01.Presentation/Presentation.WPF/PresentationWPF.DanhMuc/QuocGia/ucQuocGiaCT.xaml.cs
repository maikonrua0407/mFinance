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
using System.Data;
using PresentationWPF.CustomControl;
using Microsoft.Windows.Controls.Ribbon;
using Utilities.Common;
using Presentation.Process.Common;
using Presentation.Process;

namespace PresentationWPF.DanhMuc.QuocGia
{
    /// <summary>
    /// Interaction logic for ucQuocGiaCT.xaml
    /// </summary>
    public partial class ucQuocGiaCT : UserControl
    {
        #region Khai bao
        public event EventHandler OnSavingComleted = null;
        public static RoutedCommand HoldCommand = new RoutedCommand();
        public static RoutedCommand SaveCommand = new RoutedCommand();
        public static RoutedCommand ModifyCommand = new RoutedCommand();
        public static RoutedCommand DeleteCommand = new RoutedCommand();
        public static RoutedCommand ApproveCommand = new RoutedCommand();
        public static RoutedCommand RefuseCommand = new RoutedCommand();
        public static RoutedCommand CancelCommand = new RoutedCommand();
        public static RoutedCommand HelpCommand = new RoutedCommand();
        public static RoutedCommand CloseCommand = new RoutedCommand();
        DataRow lstChiTiet = null;

        public DataRow LstChiTiet
        {
            get { return lstChiTiet; }
            set { lstChiTiet = value; }
        }

        List<AutoCompleteEntry> lstSourceVung = new List<AutoCompleteEntry>();
        private string maVung;

        public string MaVung
        {
            get { return maVung; }
            set { maVung = value; }
        }

        List<AutoCompleteEntry> lstSourceMien = new List<AutoCompleteEntry>();
        private string maMien;

        public string MaMien
        {
            get { return maMien; }
            set { maMien = value; }
        }

        private string idMien;

        public string IdMien
        {
            get { return idMien; }
            set { idMien = value; }
        }

        private string idVung;

        public string IdVung
        {
            get { return idVung; }
            set { idVung = value; }
        }

        private int idTinhTP = 0;

        public int IdTinhTP
        {
            get { return idTinhTP; }
            set { idTinhTP = value; }
        }
        #endregion

        #region Khoi tao
        public ucQuocGiaCT()
        {
            InitializeComponent();
            ClearForm();

            BindShortkey();
            AutoComboBox auto = new AutoComboBox();
            txtMaSo.Focus();
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

        private void tlbHotKey_Click(object sender, RoutedEventArgs e)
        {
            RibbonButton tlb = (RibbonButton)sender;
            string strTinhNang = tlb.Name.Substring(3, tlb.Name.Length - 3);
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU_TAM)))
            {
                Luu(BusinessConstant.TrangThaiBanGhi.SU_DUNG, BusinessConstant.TrangThaiNghiepVu.LUU_TAM);
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU)))
            {
                Luu(BusinessConstant.TrangThaiBanGhi.SU_DUNG, BusinessConstant.TrangThaiNghiepVu.CHO_DUYET);
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
                Sua(true);
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
            {
                Xoa();
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

        #region Dang ky shortcut key

        private void HoldCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void HoldCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            tlbHold.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
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
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU_TAM)))
            {
                Luu(BusinessConstant.TrangThaiBanGhi.SU_DUNG, BusinessConstant.TrangThaiNghiepVu.LUU_TAM);
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
                Sua(true);
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

        /// <summary>
        /// Sự kiện ClearForm khi chọn thêm mới nhiều lần
        /// </summary>
        void ClearForm()
        {
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.DanhMuc;component/TinhTP/ucQuocGiaCT.xaml", ref Toolbar, ref mnuMain);
            foreach (var item in mnuMain.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += btnShortcutKey_Click;
            }
            txtMaSo.Text = "";
            txtTenTat.Text = "";
            txtNgayLap.Value = LDateTime.GetCurrentDate();
            txtNgayDuyet.Value = null;
            txtNguoiLap.Text = ClientInformation.TenDangNhap;
            txtNguoiDuyet.Text = "";
            txtTrangThaiBanGhi.Text = BusinessConstant.TrangThaiBanGhi.SU_DUNG.ToString();
            lblTrangThai.Content = BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.ToString();
        }

        #endregion

        #region Xu ly Nghiep Vu
        /// <summary>
        /// Luu du lieu
        /// </summary>
        /// <param name="banghi"></param>
        /// <param name="nghiepvu"></param>
        void Luu(BusinessConstant.TrangThaiBanGhi banghi, BusinessConstant.TrangThaiNghiepVu nghiepvu)
        {
            if (Vadidate())
            {
                DanhMucProcess danhmucProcess = new DanhMucProcess();
                try
                {
                    Presentation.Process.DanhMucServiceRef.DM_TINH_TP objTTP = new Presentation.Process.DanhMucServiceRef.DM_TINH_TP();
                    LayDuLieu(ref objTTP, banghi, nghiepvu);
                    int iResutl = 0;

                    //if (LstChiTiet == null)
                    //    iResutl = danhmucProcess.ThemTinhTP(objTTP);
                    //else
                    //{
                    //    objTTP.ID = int.Parse(LstChiTiet[1].ToString());
                    //    iResutl = danhmucProcess.updateTinhTP(objTTP);
                    //}

                    if (iResutl > 0)
                    {
                        LMessage.ShowMessage("M.DanhMuc.ucQuocGiaCT.LuuDuLieuThanhCong", LMessage.MessageBoxType.Information);
                        if (OnSavingComleted != null)
                            OnSavingComleted(null, EventArgs.Empty);
                        if (cbMultiAdd.IsChecked == true)
                            ClearForm();
                        else
                            PresentationWPF.CustomControl.CommonFunction.CloseUserControl(this);
                    }
                    else
                        LMessage.ShowMessage("M.DanhMuc.ucQuocGiaCT.LoiLuuDuLieu", LMessage.MessageBoxType.Error);
                }
                catch (Exception ex)
                {
                    LMessage.ShowMessage("M.DanhMuc.ucQuocGiaCT.LoiLuuDuLieu", LMessage.MessageBoxType.Error);
                    LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                }
            }
        }

        /// <summary>
        /// Sua du lieu
        /// </summary>
        void Sua(bool iview)
        {
            txtTenDayDu.IsEnabled = iview;
            txtTenTat.IsEnabled = iview;
        }

        /// <summary>
        /// Xoa du lieu
        /// </summary>
        void Xoa()
        {
            DanhMucProcess danhmucProcess = new Presentation.Process.DanhMucProcess();
            try
            {
                if (idTinhTP == null)
                {
                    LMessage.ShowMessage("M.DungChung.XoaKhongThanhCong", LMessage.MessageBoxType.Warning);
                }
                else
                {
                    int[] listID = new int[1];
                    List<ClientResponseDetail> listResponseDetail = new List<ClientResponseDetail>();

                    listID[0] = idTinhTP;
                    if (danhmucProcess.XoaTinhTP(listID.ToArray(), ref listResponseDetail))
                    {
                        LMessage.ShowMessage("M.DungChung.XoaThanhCong", LMessage.MessageBoxType.Information);
                        if (OnSavingComleted != null)
                            OnSavingComleted(null, EventArgs.Empty);
                        PresentationWPF.CustomControl.CommonFunction.CloseUserControl(this);
                    }
                    else
                    {
                        LMessage.ShowMessage("M.DungChung.XoaKhongThanhCong", LMessage.MessageBoxType.Error);
                    }
                }

            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        /// <summary>
        /// Kiem tra tinh hop le cua du lieu
        /// </summary>
        /// <returns></returns>
        bool Vadidate()
        {
            if (txtMaSo.Text.IsNullOrEmpty())
            {
                LMessage.ShowMessage("M.DanhMuc.ucQuocGiaCT.ThieuMaQuocGia", LMessage.MessageBoxType.Warning);
                txtMaSo.Focus();
                return false;
            }
            else if (txtTenDayDu.Text.IsNullOrEmpty())
            {
                LMessage.ShowMessage("M.DanhMuc.ucQuocGiaCT.ThieuTenDayDuQuocGia", LMessage.MessageBoxType.Warning);
                txtTenDayDu.Focus();
                return false;
            }
            else if (txtTenTat.Text.IsNullOrEmpty())
            {
                LMessage.ShowMessage("M.DanhMuc.ucQuocGiaCT.ThieuTenTatQuocGia", LMessage.MessageBoxType.Warning);
                txtTenTat.Focus();
                return false;
            }


            return true;
        }

        public void LoadForm(bool bBool)
        {
            //STT,ID,MA_TINHTP,TEN_TINHTP,TEN_TAT,MA_MIEN,TEN_MIEN,MA_VUNG,TEN_VUNG,TTHAI_BGHI,TTHAI_NVU,NGAY_NHAP,NGUOI_NHAP,NGAY_CNHA,NGUOI_CNHA,KEY
            idTinhTP = int.Parse(LstChiTiet[1].ToString());
            txtMaSo.IsEnabled = false;
            txtMaSo.Text = LstChiTiet[1].ToString();
            txtTenDayDu.Text = LstChiTiet[3].ToString();
            txtTenTat.Text = LstChiTiet[4].ToString();
            lblTrangThai.Content = LstChiTiet[10].ToString();
            txtTrangThaiBanGhi.Text = LstChiTiet[9].ToString();
            if (!LstChiTiet[11].ToString().IsNullOrEmptyOrSpace())
                txtNgayLap.Value = LDateTime.StringToDate(LstChiTiet[11].ToString(), "yyyyMMdd");
            if (!LstChiTiet[13].ToString().IsNullOrEmptyOrSpace())
                txtNgayDuyet.Value = LDateTime.StringToDate(LstChiTiet[13].ToString(), "yyyyMMdd");
            txtNguoiLap.Text = LstChiTiet[12].ToString();
            txtNguoiDuyet.Text = LstChiTiet[14].ToString();
            Sua(bBool);
            lblTrangThai.Content = BusinessConstant.layNgonNguNghiepVu(lblTrangThai.Content.ToString());
        }

        /// <summary>
        /// Lay du lieu tu control dieu khien dua vao object
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="banghi"></param>
        /// <param name="nghiepvu"></param>
        void LayDuLieu(ref Presentation.Process.DanhMucServiceRef.DM_TINH_TP obj, BusinessConstant.TrangThaiBanGhi banghi, BusinessConstant.TrangThaiNghiepVu nghiepvu)
        {
            obj.MA_TINHTP = txtMaSo.Text;
            obj.TEN_TINHTP = txtTenDayDu.Text;
            obj.TEN_TAT = txtTenTat.Text;
            obj.ID_VUNG_MIEN = int.Parse(IdVung);
            obj.NGAY_NHAP = LDateTime.GetCurrentDate(ApplicationConstant.defaultDateTimeFormat);
            obj.MA_DVI_QLY = ClientInformation.MaDonVi;
            obj.MA_DVI_TAO = ClientInformation.MaDonVi;
            obj.NGUOI_NHAP = ClientInformation.TenDangNhap;
            obj.TTHAI_BGHI = BusinessConstant.layGiaTri(banghi);
            obj.TTHAI_NVU = BusinessConstant.layGiaTri(nghiepvu);
        }

        #endregion
    }
}
