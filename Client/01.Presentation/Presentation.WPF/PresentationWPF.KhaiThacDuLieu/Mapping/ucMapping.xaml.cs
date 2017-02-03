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
using Presentation.Process.Common;
using Microsoft.Windows.Controls.Ribbon;
using Utilities.Common;
using Telerik.Windows.Controls;
using Presentation.Process;
using Presentation.Process.KhaiThacDuLieuServiceRef;

namespace PresentationWPF.KhaiThacDuLieu.Mapping
{
    /// <summary>
    /// Interaction logic for ucMapping.xaml
    /// </summary>
    public partial class ucMapping : UserControl
    {
        #region Khai bao
        private int flag = 0;
        private int currentPosition;
        private int currentPage;
        private int currentID;

        // Phân trang
        int StartRow = 1;
        int EndRow = ClientInformation.SoLuongBanGhi;
        int CurrentPagging = 1;
        int PaggingSize = ClientInformation.SoLuongBanGhi;

        public static RoutedCommand SaveCommand = new RoutedCommand();
        public static RoutedCommand HelpCommand = new RoutedCommand();
        public static RoutedCommand CloseCommand = new RoutedCommand();
        #endregion

        #region Khoi tao
        public ucMapping()
        {
            InitializeComponent();
            HeThong hethong = new HeThong();
            BindShortkey();
            LoadgrdDSLoaiTK();
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
                    if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRINH_DUYET)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.S, ModifierKeys.Control);
                        key = new KeyBinding(SaveCommand, keyg);
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
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRINH_DUYET)))
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
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRINH_DUYET)))
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

        #region Xu ly giao dien
        /// <summary>
        /// Sự kiện LoadForm
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //txtTimKiemNhanh.Focus();
        }  

        private void LoadgrdDSCTieu(string maLoaiTK)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                DataTable dt = null;
                LDatatable.MakeParameterTable(ref dt);
                                         
                
                LDatatable.AddParameter(ref dt, "@INP_USER", "string", ClientInformation.TenDangNhap);
                LDatatable.AddParameter(ref dt, "@MA_DVI_QLY", "string", ClientInformation.MaDonVi);
                LDatatable.AddParameter(ref dt, "@StartRow", "INT", StartRow.ToString());
                LDatatable.AddParameter(ref dt, "@EndRow", "INT", EndRow.ToString());
                LDatatable.AddParameter(ref dt, "@MA_LOAITK", "string", maLoaiTK);

                Presentation.Process.KhaiThacDuLieuProcess process = new Presentation.Process.KhaiThacDuLieuProcess();
                DataSet ds = process.GetDanhSachMaTK(dt);
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count>0)
                {
                    grdDSCTieu.ItemsSource = ds.Tables[0].DefaultView;
                }
                else
                {
                    grdDSCTieu.ItemsSource = null;
                }


            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }

            Mouse.OverrideCursor = Cursors.Arrow;      
        }

        private void LoadgrdDSLoaiTK()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                DataTable dt = null;
                LDatatable.MakeParameterTable(ref dt);


                LDatatable.AddParameter(ref dt, "@INP_USER", "string", ClientInformation.TenDangNhap);
                LDatatable.AddParameter(ref dt, "@MA_DVI_QLY", "string", ClientInformation.MaDonVi);
                LDatatable.AddParameter(ref dt, "@StartRow", "INT", StartRow.ToString());
                LDatatable.AddParameter(ref dt, "@EndRow", "INT", EndRow.ToString());

                Presentation.Process.KhaiThacDuLieuProcess process = new Presentation.Process.KhaiThacDuLieuProcess();
                DataSet ds = process.GetDanhSachLoaiTK(dt);
                if (ds != null && ds.Tables.Count > 0)
                {
                    grdDSLoaiTK.ItemsSource = ds.Tables[0].DefaultView;
                }
                else
                {
                    grdDSLoaiTK.ItemsSource = null;
                }


            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }

            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void LoadgrdDSDKien(string dkien)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {

                Presentation.Process.KhaiThacDuLieuProcess process = new Presentation.Process.KhaiThacDuLieuProcess();
                DataSet ds = process.GetDanhSachDKien(dkien);
                if (ds != null && ds.Tables.Count > 0)
                {
                    grdDSDKien.ItemsSource = ds.Tables[0].DefaultView;
                }
                else
                {
                    grdDSDKien.ItemsSource = null;
                }


            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }

            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void LoadCheckBox(string maLoaiTK, string maTK)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                DataTable dt = null;
                LDatatable.MakeParameterTable(ref dt);


                LDatatable.AddParameter(ref dt, "@INP_USER", "string", ClientInformation.TenDangNhap);
                LDatatable.AddParameter(ref dt, "@MA_DVI_QLY", "string", ClientInformation.MaDonVi);
                LDatatable.AddParameter(ref dt, "@StartRow", "INT", StartRow.ToString());
                LDatatable.AddParameter(ref dt, "@EndRow", "INT", EndRow.ToString());
                LDatatable.AddParameter(ref dt, "@INP_USER", "string", ClientInformation.TenDangNhap);
                LDatatable.AddParameter(ref dt, "@MA_LOAITK", "string", maLoaiTK);
                LDatatable.AddParameter(ref dt, "@MATK", "string", maTK);

                Presentation.Process.KhaiThacDuLieuProcess process = new Presentation.Process.KhaiThacDuLieuProcess();
                DataSet ds = process.GetDanhSachMapping(dt);
                grdDSDKien.UnselectAll();

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count>0)
                {
                    List<DataRowView> lstDr = new List<DataRowView>();
                    for (int i = 0; i < grdDSDKien.Items.Count; i++)
                    {
                        DataRowView dr = (DataRowView)grdDSDKien.Items[i];
                        for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                        {                            
                            if(dr["MA_DKIEN"].ToString().Equals(ds.Tables[0].Rows[j]["MA_MAPPING"]))
                                lstDr.Add(dr);
                        }
                    }
                    grdDSDKien.Select(lstDr);
                }
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }

            Mouse.OverrideCursor = Cursors.Arrow;
        }

        /// <summary>
        /// Load dữ liệu lên Form
        /// </summary>
        private void LoadDuLieu(object sender, EventArgs e)
        {
            try
            {
                LoadgrdDSLoaiTK();
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }
                

        /// <summary>
        /// Xử lý sự kiện escape thoát form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            PresentationWPF.CustomControl.CommonFunction.CloseUserControl(e, this);
            PresentationWPF.CustomControl.CommonFunction.SelectNextControl(e);
        }
                       
        private void TimKiemPhanTrang()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            DataTable dt = null;
            LDatatable.MakeParameterTable(ref dt);


            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void grdDSLoaiTK_SelectedCellsChanged(object sender, Telerik.Windows.Controls.GridView.GridViewSelectedCellsChangedEventArgs e)
        {
            LoadgrdDSCTieu(getSeletedRowLoaiTK()["MA_LOAITK"].ToString());
            grdDSDKien.ItemsSource = null;
        }

        private void grdDSCTieu_SelectedCellsChanged(object sender, Telerik.Windows.Controls.GridView.GridViewSelectedCellsChangedEventArgs e)
        {
            LoadgrdDSDKien(getSeletedRowLoaiTK()["NGUON_MAPPING"].ToString());
            LoadCheckBox(getSeletedRowLoaiTK()["MA_LOAITK"].ToString(), getSeletedRowCTieu()["MATK"].ToString());
        }

        private void grdDSDKien_SelectedCellsChanged(object sender, Telerik.Windows.Controls.GridView.GridViewSelectedCellsChangedEventArgs e)
        {

        }

        #endregion

        #region Xử lý nghiệp vụ

        private bool Validation()
        {
            try
            {
                if (grdDSDKien.ItemsSource == null)
                {
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

        public void OnSave()
        {
            try
            {
                if (!Validation()) return;

                BeforeAddNew();
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        

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
            PresentationWPF.CustomControl.CommonFunction.CloseUserControl(this);
        }

        /// <summary>
        /// Trước khi thêm mới
        /// </summary>
        /// 
        private void BeforeAddNew()
        {
            OnAddNew(getListSeletedDKien());
        }

        /// <summary>
        /// Thêm
        /// </summary>
        private void OnAddNew(List<DataRowView> lstDataRowView)
        {
            List<BC_MATK_MAPPING> lstObj = new List<BC_MATK_MAPPING>();
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                KhaiThacDuLieuProcess processKTDL = new KhaiThacDuLieuProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = false;

                string maTK = getSeletedRowCTieu()["MATK"].ToString();
                string maLoaiTK = getSeletedRowLoaiTK()["MA_LOAITK"].ToString();
                string loaiBCTK = getSeletedRowLoaiTK()["LOAI_BCTK"].ToString();
                string maDviQly = ClientInformation.MaDonVi;
                string maDviTao = ClientInformation.MaDonViGiaoDich;
                string ngayNhap = Convert.ToDateTime(DateTime.Today).ToString("yyyyMMdd");
                string nguoiNhap = ClientInformation.TenDangNhap;
                string ttBGhi = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                string ttNVu = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();

                if (lstDataRowView.Count > 0)
                {
                    foreach (DataRowView drv in lstDataRowView)
                    {
                        BC_MATK_MAPPING obj = new BC_MATK_MAPPING();
                        obj.MA_MAPPING = drv["MA_DKIEN"].ToString();
                        obj.MATK = maTK;
                        obj.MA_LOAITK = maLoaiTK;
                        obj.LOAI_BCTK = loaiBCTK;
                        obj.MA_DVI_QLY = maDviQly;
                        obj.MA_DVI_TAO = maDviTao;
                        obj.NGAY_NHAP = ngayNhap;
                        obj.NGUOI_NHAP = nguoiNhap;
                        obj.TTHAI_BGHI = ttBGhi;
                        obj.TTHAI_NVU = ttNVu;
                        lstObj.Add(obj);
                    }
                }
                else
                {
                    BC_MATK_MAPPING obj = new BC_MATK_MAPPING();
                    obj.MATK = maTK;
                    obj.MA_LOAITK = maLoaiTK;
                    obj.TTHAI_LY_DO =  "UncheckAll";
                    lstObj.Add(obj);
                }

                ret = processKTDL.Mapping(DatabaseConstant.Action.THEM, ref lstObj, ref listClientResponseDetail);
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

        private List<DataRowView> getListSeletedDKien()
        {
            try
            {
                List<DataRowView> listDataRow = new List<DataRowView>();

                if (grdDSDKien.SelectedItems.Count > 0)
                {
                    for (int i = 0; i < grdDSDKien.SelectedItems.Count; i++)
                    {
                        DataRowView dr = (DataRowView)grdDSDKien.SelectedItems[i];
                        listDataRow.Add(dr);
                    }
                }
                return listDataRow;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                return null;
            }

        }


        private DataRowView getSeletedRowLoaiTK()
        {
            try
            {

                DataRowView dataRow = (DataRowView)grdDSLoaiTK.SelectedItems.First();
                
                return dataRow;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                return null;
            }

        }

        private DataRowView getSeletedRowCTieu()
        {
            try
            {

                DataRowView dataRow = (DataRowView)grdDSCTieu.SelectedItems.First();

                return dataRow;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                return null;
            }

        }
        #endregion        

    }
}
