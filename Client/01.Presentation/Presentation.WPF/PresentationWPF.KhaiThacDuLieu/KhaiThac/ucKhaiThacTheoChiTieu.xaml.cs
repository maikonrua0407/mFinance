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
using Presentation.Process;
using Presentation.Process.Common;
using Microsoft.Windows.Controls.Ribbon;
using System.Data;
using Presentation.Process.KhaiThacDuLieuServiceRef;
using System.Reflection;

namespace PresentationWPF.KhaiThacDuLieu.KhaiThac
{
    /// <summary>
    /// Interaction logic for ucKhaiThacTheoChiTieu.xaml
    /// </summary>
    public partial class ucKhaiThacTheoChiTieu : UserControl
    {
        #region Khai bao

        Application app = Application.Current;
        UserControl uc;

        public static RoutedCommand HelpCommand = new RoutedCommand();
        public static RoutedCommand CloseCommand = new RoutedCommand();

        bool isLoaded = false;
        public string formCase = null;
        public delegate void DieuKienBaoCao(out List<Tuple<string,string>> dsDieuKien);
        public delegate void BuildDuLieuBaoCao(DataSet ds);
        HT_BAOCAO htBaoCao = null;
        #endregion

        #region Khoi tao

        public ucKhaiThacTheoChiTieu()
        {
            try
            {
                InitializeComponent();
                InitEventHandler();
                LoadDuLieu();
            }
            catch (Exception ex)
            { }
        }

        /// <summary>
        /// InitEventHandler
        /// </summary>
        private void InitEventHandler()
        {
            txtTimKiemNhanh.GotFocus += txtTimKiemNhanh_GotFocus;
            txtTimKiemNhanh.LostFocus += txtTimKiemNhanh_LostFocus;
            txtTimKiemNhanh.KeyDown += new KeyEventHandler(txtTimKiemNhanh_KeyDown);

        }

        #endregion

        #region Dang ky hot key

        private void BindHotkey()
        {
            foreach (var child in Toolbar.Children)
            {
                if (child.GetType() == typeof(RibbonButton))
                {
                    RibbonButton tlb = (RibbonButton)child;
                    KeyBinding key = new KeyBinding();
                    string strTinhNang = tlb.Name.Substring(3, tlb.Name.Length - 3);
                    if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRO_GIUP)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.F1, ModifierKeys.None);
                        key = new KeyBinding(HelpCommand, keyg);
                        key.Gesture = keyg;
                    }
                    if (key != null)
                        InputBindings.Add(key);
                }
            }
        }

        /// <summary>
        /// Sự kiện hot key
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tlbHotKey_Click(object sender, RoutedEventArgs e)
        {
            RibbonButton tlb = (RibbonButton)sender;
            string strTinhNang = tlb.Name.Substring(3, tlb.Name.Length - 3);
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU)))
            {
                ShowData();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XUAT_DU_LIEU)))
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

        #endregion

        #region Dang ky shortcut key

        private void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ShowData();
        }

        private void ExportCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void ExportCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {

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
        /// Trợ giúp
        /// </summary>
        private void onHelp()
        {
            CustomControl.CommonFunction.ShowHelp(this);
        }

        #endregion

        #region Xu ly nghiep vu

        private void ShowData()
        {
            try
            {
                
                // Lấy dữ liệu từ form điều kiện             
                List<HT_BAOCAO_TSO> listHtBaoCaoTso = new List<HT_BAOCAO_TSO>();
                DatabaseConstant.Action action = DatabaseConstant.Action.TRUY_VAN;
                DataSet ds=null;
                // Lấy báo cáo được chọn từ grid
                DataRowView dr = (DataRowView)grDSBaoCao.SelectedItem;
                int idBaoCao = int.Parse(dr[0].ToString());
                int idBaoCaoCha = int.Parse(dr[4].ToString());
                string maBaoCao = dr[2].ToString();
                string responseMessage = "";
                if (idBaoCao > 0)
                {
                    MethodInfo mi = uc.GetType().GetMethod("GetParameters");
                    Delegate dieuKienBaoCao = Delegate.CreateDelegate(typeof(DieuKienBaoCao), uc, mi);
                    if (!LObject.IsNullOrEmpty(dieuKienBaoCao))
                    {
                        KhaiThacDuLieuProcess process = new KhaiThacDuLieuProcess();
                        DieuKienBaoCao dlgdieukienBaoCao = (DieuKienBaoCao)dieuKienBaoCao;
                        List<Tuple<string, string>> dsDieuKienBC;
                        dlgdieukienBaoCao(out dsDieuKienBC);
                        foreach (Tuple<string, string> tuple in dsDieuKienBC)
                        {
                            HT_BAOCAO_TSO tso = new HT_BAOCAO_TSO();
                            tso.MA_TSO = tuple.Item2;
                            tso.LOAI_TSO = ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri();
                            tso.GTRI_TSO = tuple.Item1;
                            listHtBaoCaoTso.Add(tso);
                        }
                        if (process.DuLieuChiTieu(action, idBaoCao, ref htBaoCao, ref listHtBaoCaoTso, ref ds, ref responseMessage) == ApplicationConstant.ResponseStatus.THANH_CONG)
                        {
                            mi = uc.GetType().GetMethod("BuildData");
                            Delegate buildDLBaoCao = Delegate.CreateDelegate(typeof(BuildDuLieuBaoCao), uc, mi);
                            if (!LObject.IsNullOrEmpty(buildDLBaoCao))
                            {
                                BuildDuLieuBaoCao dlgbuildDLBaoCao = (BuildDuLieuBaoCao)buildDLBaoCao;
                                dlgbuildDLBaoCao(ds);
                            }
                        }
                    }
                }
                
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        #endregion

        #region Xu ly giao dien

        private void LoadDuLieu()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                // Hiển thị lưới dữ liệu đối tượng
                BuildGridBaoCao();
                // Hiển thị lưới dữ liệu phân quyền
                //BuildFormDieuKien();
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!isLoaded)
            {
                formCase = ClientInformation.FormCase;

                // Khởi tạo các sự kiện cho controltxtTimKiemNhanh.KeyDown += txtTimKiemNhanh_KeyDown;
                txtTimKiemNhanh.GotFocus += txtTimKiemNhanh_GotFocus;
                txtTimKiemNhanh.LostFocus += txtTimKiemNhanh_LostFocus;
                grDSBaoCao.SelectionChanged += grDSBaoCao_SelectionChanged;
                grDSBaoCao.MouseDoubleClick += grDSBaoCao_MouseDoubleClick;
                isLoaded = true;
                txtTimKiemNhanh.Focus();
            }
        }

        /// <summary>
        /// Sự kiện tìm kiếm nhanh
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtTimKiemNhanh_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                PresentationWPF.CustomControl.CommonFunction.QuickSearchInGrid(grDSBaoCao, txtTimKiemNhanh.Text);
                loadWidthColumnBaoCao();
            }
        }

        /// <summary>
        /// Sự kiện focus vào textbox tìm kiếm nhanh
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtTimKiemNhanh_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtTimKiemNhanh.Text == LLanguage.SearchResourceByKey("U.DungChung.TimKiemNhanh"))
            {
                txtTimKiemNhanh.Text = "";
                txtTimKiemNhanh.Focus();
            }
        }

        /// <summary>
        /// Sự kiện rời focus khỏi textbox tìm kiếm nhanh
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtTimKiemNhanh_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtTimKiemNhanh.Text))
            {
                txtTimKiemNhanh.Text = LLanguage.SearchResourceByKey("U.DungChung.TimKiemNhanh");
            }
        }

        private void BuildGridBaoCao()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                DataTable dtBaoCao = new DataTable();
                dtBaoCao.Columns.Add("ID", typeof(int));
                dtBaoCao.Columns.Add("STT", typeof(string));
                dtBaoCao.Columns.Add("Mã chỉ tiêu", typeof(string));
                dtBaoCao.Columns.Add("Tên chỉ tiêu", typeof(string));
                dtBaoCao.Columns.Add("ID_CHA", typeof(int));
                int sttCha = 0;
                int sttCon = 0;

                //string maPhanHeBaoCao = ClientInformation.FormCase;
                string maPhanHeBaoCao = "KTDL_DM";
                KhaiThacDuLieuProcess process = new KhaiThacDuLieuProcess();
                ApplicationConstant.ResponseStatus ret = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                List<HT_BAOCAO> lstBaoCaoTheoPhanHe = new List<HT_BAOCAO>();
                DatabaseConstant.Action action = DatabaseConstant.Action.TRUY_VAN;
                string responseMessage = "";

                ret = process.DanhSachChiTieu(action, maPhanHeBaoCao, ref lstBaoCaoTheoPhanHe, ref responseMessage);
                if (ret == ApplicationConstant.ResponseStatus.THANH_CONG && lstBaoCaoTheoPhanHe.Count > 0)
                {
                    List<HT_BAOCAO> lstBaoCaoCha = lstBaoCaoTheoPhanHe.Where(e => e.ID_BAOCAO_CHA == null).ToList();
                    foreach (var itemCha in lstBaoCaoCha)
                    {
                        DataRow rCha = dtBaoCao.NewRow();
                        sttCha = sttCha + 1;
                        rCha[0] = itemCha.ID_BAOCAO;
                        rCha[1] = sttCha.ToString();
                        rCha[2] = itemCha.MA_BAOCAO;
                        rCha[3] = itemCha.TEN_BAOCAO;
                        rCha[4] = 0;
                        dtBaoCao.Rows.Add(rCha);
                        sttCon = 0;
                        foreach (var item in lstBaoCaoTheoPhanHe)
                        {
                            if (item.ID_BAOCAO_CHA == itemCha.ID_BAOCAO)
                            {
                                DataRow rCon = dtBaoCao.NewRow();
                                sttCon = sttCon + 1;
                                rCon[0] = item.ID_BAOCAO;
                                rCon[1] = sttCha.ToString() + "." + sttCon.ToString();
                                rCon[2] = item.MA_BAOCAO;
                                rCon[3] = "     " + item.TEN_BAOCAO;
                                rCon[4] = item.ID_BAOCAO_CHA;
                                dtBaoCao.Rows.Add(rCon);
                            }
                            else
                            {
                            }                            
                        }
                        // đổ source lên lưới
                        grDSBaoCao.ItemsSource = dtBaoCao.DefaultView;
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

        private void loadWidthColumnBaoCao()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {                
                for (int i = 0; i < grDSBaoCao.Columns.Count; i++)
                {
                    if (i == 0)
                    {
                        grDSBaoCao.Columns[i].IsVisible = false;

                    }
                    else if (i == 1)
                    {
                        grDSBaoCao.Columns[i].Width = new Telerik.Windows.Controls.GridViewLength(40, Telerik.Windows.Controls.GridViewLengthUnitType.Pixel);
                        grDSBaoCao.Columns[i].IsReadOnly = true;                        
                    }
                    else if (i == 2)
                    {
                        grDSBaoCao.Columns[i].IsVisible = false;
                    }
                    else if (i == 3)
                    {
                        grDSBaoCao.Columns[i].Width = new Telerik.Windows.Controls.GridViewLength(1, Telerik.Windows.Controls.GridViewLengthUnitType.Star);
                        grDSBaoCao.Columns[i].IsReadOnly = true;
                    }
                    else if (i == 4)
                    {
                        grDSBaoCao.Columns[i].IsVisible = false;
                    }
                    else
                    {
                        grDSBaoCao.Columns[i].Width = new Telerik.Windows.Controls.GridViewLength(1, Telerik.Windows.Controls.GridViewLengthUnitType.Auto);
                    }                    
                }

                foreach (DataRowView item in grDSBaoCao.Items)
                {
                    int idBaoCao = int.Parse(item[4].ToString());
                    if (idBaoCao == 0)
                    {
                        //BrushConverter bc = new BrushConverter();
                        //DataRow row = item.Row;
                        //row.Background = (Brush)bc.ConvertFrom("#C7DFFC");                        
                    }
                    else
                    {
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

        private void BuildFormDieuKien()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                if (grDSBaoCao.SelectedItems.Count > 0)
                {
                    // Lấy báo cáo được chọn từ grid
                    DataRowView dr = (DataRowView)grDSBaoCao.SelectedItem;
                    int idBaoCao = int.Parse(dr[0].ToString());
                    int idBaoCaoCha = int.Parse(dr[4].ToString());
                    string maBaoCao = dr[2].ToString();

                    // Lấy thông tin báo cáo và tham số    
                    if (idBaoCaoCha != 0)
                    {
                        KhaiThacDuLieuProcess process = new KhaiThacDuLieuProcess();
                        ApplicationConstant.ResponseStatus ret = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                        List<HT_BAOCAO_TSO> lstBaoCaoTso = new List<HT_BAOCAO_TSO>();
                        DatabaseConstant.Action action = DatabaseConstant.Action.TRUY_VAN;
                        string responseMessage = "";
                        process.DuLieuChiTieu(action, idBaoCao, ref htBaoCao, ref lstBaoCaoTso, ref responseMessage);
                        string uriBaoCaoDieuKien = "/" + htBaoCao.NHOM_DIEUKIEN + ";component/" + htBaoCao.FILE_DIEUKIEN;

                        uc = (UserControl)System.Windows.Application.LoadComponent(new Uri(uriBaoCaoDieuKien, System.UriKind.RelativeOrAbsolute));
                        if (!LObject.IsNullOrEmpty(uc.GetType().GetProperty("MaBaoCao")))
                        {
                            uc.GetType().GetProperty("MaBaoCao").SetValue(uc, maBaoCao, null);
                        }
                        frFormInput.Content = uc;
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

        private void grDSBaoCao_Loaded(object sender, RoutedEventArgs e)
        {
            loadWidthColumnBaoCao();
        }

        private void grDSBaoCao_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            BuildFormDieuKien();
        }

        private void grDSBaoCao_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BuildFormDieuKien();
        }

        void NavigationService_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            //((PageNavigation)e.Content).MessageFromCallingWindow = (string)e.ExtraData;
        }

        private void tlbView_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                ShowData();
            }
            catch (System.Exception ex)
            {
                
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        #endregion
    }
}
