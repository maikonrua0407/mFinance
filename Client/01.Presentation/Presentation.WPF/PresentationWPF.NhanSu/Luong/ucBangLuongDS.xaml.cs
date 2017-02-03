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

namespace PresentationWPF.NhanSu.Luong
{
    /// <summary>
    /// Interaction logic for ucBangLuongDS.xaml
    /// </summary>
    public partial class ucBangLuongDS : UserControl
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

        private int soBacLuong = 7; //Mặc định là 7 có thể thay đổi theo tham số

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

        #region Khoi tao
        public ucBangLuongDS()
        {
            InitializeComponent();

            DuyetQuyenTinhNang();

            BindShortkey();

            SetVisibled();

            LoadDuLieu();
        }

        private void DuyetQuyenTinhNang()
        {
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.NhanSu;component/Luong/ucBangLuongDS.xaml", ref Toolbar, ref mnuMain);
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

        private void grid_CellEditEnded(object sender, Telerik.Windows.Controls.GridViewCellEditEndedEventArgs e)
        {
            string column = e.Cell.Column.Name;

            if (column.Equals("BAC1"))
            {
                string truoc = "";
                string hienTai = "";
                for (int i = 2; i <= soBacLuong; i++)
                {
                    truoc = "BAC" + (i - 1).ToString();
                    hienTai = "BAC" + i.ToString();

                    DataRowView dr = (DataRowView)grid.CurrentCellInfo.Item;
                    dr[hienTai] = (Convert.ToDecimal(dr[truoc])) * (decimal)1.1;
                    grid.CurrentItem = dr;
                }
            }

        }

        #endregion

        #region Xử lý nghiệp vụ
        private void GetFormData(ref List<NS_BAC_LUONG> lst)
        {
            try
            {
                lst = new List<NS_BAC_LUONG>();
                NS_BAC_LUONG obj = null;
                foreach (DataRowView dr in grid.Items)
                {
                    obj = new NS_BAC_LUONG();
                    obj.ID_CHUC_VU = (int)dr["ID_CHUC_VU"];
                    if(!dr["BAC1"].ToString().IsNullOrEmpty())
                        obj.BAC1 = (decimal?)dr["BAC1"];

                    if (!dr["BAC2"].ToString().IsNullOrEmpty())
                        obj.BAC2 = (decimal?)dr["BAC2"];

                    if (!dr["BAC3"].ToString().IsNullOrEmpty())
                        obj.BAC3 = (decimal?)dr["BAC3"];

                    if (!dr["BAC4"].ToString().IsNullOrEmpty())
                        obj.BAC4 = (decimal?)dr["BAC4"];

                    if (!dr["BAC5"].ToString().IsNullOrEmpty())
                        obj.BAC5 = (decimal?)dr["BAC5"];

                    if (!dr["BAC6"].ToString().IsNullOrEmpty())
                        obj.BAC6 = (decimal?)dr["BAC6"];

                    if (!dr["BAC7"].ToString().IsNullOrEmpty())
                        obj.BAC7 = (decimal?)dr["BAC7"];

                    if (!dr["BAC8"].ToString().IsNullOrEmpty())
                        obj.BAC8 = (decimal?)dr["BAC8"];

                    if (!dr["BAC9"].ToString().IsNullOrEmpty())
                        obj.BAC9 = (decimal?)dr["BAC9"];

                    if (!dr["BAC10"].ToString().IsNullOrEmpty())
                        obj.BAC10 = (decimal?)dr["BAC10"];

                    if (!dr["BAC11"].ToString().IsNullOrEmpty())
                        obj.BAC11 = (decimal?)dr["BAC11"];

                    if (!dr["BAC12"].ToString().IsNullOrEmpty())
                        obj.BAC12 = (decimal?)dr["BAC12"];

                    if (!dr["BAC13"].ToString().IsNullOrEmpty())
                        obj.BAC13 = (decimal?)dr["BAC13"];

                    if (!dr["BAC14"].ToString().IsNullOrEmpty())
                        obj.BAC14 = (decimal?)dr["BAC14"];

                    if (!dr["BAC15"].ToString().IsNullOrEmpty())
                        obj.BAC15 = (decimal?)dr["BAC15"];

                    if (!dr["BAC16"].ToString().IsNullOrEmpty())
                        obj.BAC16 = (decimal?)dr["BAC16"];

                    if (!dr["BAC17"].ToString().IsNullOrEmpty())
                        obj.BAC17 = (decimal?)dr["BAC17"];

                    if (!dr["BAC18"].ToString().IsNullOrEmpty())
                        obj.BAC18 = (decimal?)dr["BAC18"];

                    if (!dr["BAC19"].ToString().IsNullOrEmpty())
                        obj.BAC19 = (decimal?)dr["BAC19"];

                    if (!dr["BAC20"].ToString().IsNullOrEmpty())
                        obj.BAC20 = (decimal?)dr["BAC20"];                    

                    #region Thông tin kiểm soát
                    obj.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                    obj.TTHAI_NVU = trangThaiNVu;
                    obj.MA_DVI_QLY = ClientInformation.MaDonVi;
                    obj.MA_DVI_TAO = ClientInformation.MaDonViGiaoDich;
                    obj.NGAY_NHAP = ClientInformation.NgayLamViecHienTai;
                    obj.NGUOI_NHAP = ClientInformation.TenDangNhap;
                    obj.NGAY_CNHAT = ClientInformation.NgayLamViecHienTai;
                    obj.NGUOI_CNHAT = ClientInformation.TenDangNhap;
                    #endregion

                    lst.Add(obj);
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
                DataSet ds = processNhanSu.GetDanhSachBangLuong(dt);
                if (ds != null && ds.Tables.Count > 0)
                {
                    grid.DataContext = ds.Tables[0].DefaultView;
                }
                else
                {
                    grid.DataContext = null;
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

                return true;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                return false;
            }
        }

        private void SetVisibled()
        {
            string column = "";
            for (int i = soBacLuong + 1; i <= 20; i++)
            {
                column = "BAC" + i.ToString();
                grid.Columns[column].IsVisible = false;
            }
        }

        public void OnSave()
        {
            try
            {
                if (!Validation()) return;

                List<NS_BAC_LUONG> lst = null;
                GetFormData(ref lst);

                NhanSuProcess processNhanSu = new NhanSuProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = false;

                Mouse.OverrideCursor = Cursors.Wait;
                ret = processNhanSu.BangLuong(DatabaseConstant.Action.LUU, ref lst, ref listClientResponseDetail);
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
