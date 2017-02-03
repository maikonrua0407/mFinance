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
using System.Data;
using Presentation.Process;
using PresentationWPF.CustomControl;
using Presentation.Process.BaoCaoServiceRef;
using System.Reflection;
using System.Collections;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using Presentation.Process.Common;
using PresentationWPF.BaoCao.HDVO;
using PresentationWPF.BaoCao.TDVM;
using PresentationWPF.BaoCao.GDKT;
using System.IO;

namespace PresentationWPF.BaoCao.DungChung
{
    /// <summary>
    /// Interaction logic for ucPhanQuyenCN.xaml
    /// </summary>
    public partial class ucDanhSach : UserControl
    {
        #region Khai bao

        Application app = Application.Current;
        UserControl uc;

        public static RoutedCommand HelpCommand = new RoutedCommand();
        public static RoutedCommand CloseCommand = new RoutedCommand();

        BaoCaoProcess process = new BaoCaoProcess();
        bool isLoaded = false;
        public string formCase = null;
        List<HT_CNANG> lstBaoCaoTheoPhanHe = new List<HT_CNANG>();

        delegate void DieuKienBaoCao();

        HT_BAOCAO htBaoCao = null;
        List<HT_BAOCAO_TSO> lstHtBaoCaoTso = null;

        #endregion

        #region Khoi tao

        public ucDanhSach()
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
                ShowReport();
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
            ShowReport();
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

        private void ShowReport()
        {
            try
            {

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
                dtBaoCao.Columns.Add(LLanguage.SearchResourceByKey("U.DungChung.STT"), typeof(int));
                dtBaoCao.Columns.Add(LLanguage.SearchResourceByKey("U.BaoCao.MaBaoCao"), typeof(string));
                dtBaoCao.Columns.Add(LLanguage.SearchResourceByKey("U.BaoCao.TenBaoCao"), typeof(string));
                int stt = 0;

                string maPhanHeBaoCao = ClientInformation.FormCase;
                lstBaoCaoTheoPhanHe = process.LayDanhSachBaoCaoTheoPhanHe(maPhanHeBaoCao);
                foreach (var item in lstBaoCaoTheoPhanHe)
                {
                    DataRow r = dtBaoCao.NewRow();
                    stt = stt + 1;
                    r[0] = item.ID;
                    r[1] = stt;
                    r[2] = item.MA_CNANG;
                    r[3] = LLanguage.SearchResourceByKey(item.MA_NNGU);
                    dtBaoCao.Rows.Add(r);
                }
                // đổ source lên lưới
                grDSBaoCao.ItemsSource = dtBaoCao.DefaultView;
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
                    else
                    {
                        grDSBaoCao.Columns[i].Width = new Telerik.Windows.Controls.GridViewLength(1, Telerik.Windows.Controls.GridViewLengthUnitType.Auto);
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
                    string maBaoCao = dr[2].ToString();

                    // Lấy thông tin báo cáo và tham số                    
                    process.LayThongTinBaoCao(idBaoCao, maBaoCao, ref htBaoCao, ref lstHtBaoCaoTso);
                    ReportInformation.MaBaoCao = maBaoCao;
                    ReportInformation.IdBaoCao = idBaoCao;
                    ReportInformation.DinhDang = htBaoCao.LOAI_FILE_BAOCAO;
                    ReportInformation.NgonNgu = ClientInformation.NgonNgu;
                    ReportInformation.ObjBaoCao = htBaoCao;

                    string uriBaoCaoDieuKien = "/" + htBaoCao.NHOM_DIEUKIEN + ";component/" + htBaoCao.FILE_DIEUKIEN;

                    uc = (UserControl)System.Windows.Application.LoadComponent(new Uri(uriBaoCaoDieuKien, System.UriKind.RelativeOrAbsolute));

                    if (!LObject.IsNullOrEmpty(uc.GetType().GetProperty("MaBaoCao")))
                    {
                        uc.GetType().GetProperty("MaBaoCao").SetValue(uc, maBaoCao, null);
                    }

                    if (!LObject.IsNullOrEmpty(uc.GetType().GetProperty("IDBaoCao")))
                    {
                        uc.GetType().GetProperty("IDBaoCao").SetValue(uc, idBaoCao, null);
                    }

                    if (!LObject.IsNullOrEmpty(uc.GetType().GetProperty("ObjBaoCao")))
                    {
                        uc.GetType().GetProperty("ObjBaoCao").SetValue(uc, htBaoCao, null);
                    }

                    if (!LObject.IsNullOrEmpty(uc.GetType().GetProperty("LstHtBaoCaoTso")))
                    {
                        uc.GetType().GetProperty("LstHtBaoCaoTso").SetValue(uc, lstHtBaoCaoTso, null);
                    }

                    frFormInput.Content = uc;
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

        #endregion

        void NavigationService_LoadCompleted(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            //((PageNavigation)e.Content).MessageFromCallingWindow = (string)e.ExtraData;
        }

        private void tlbView_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Mouse.OverrideCursor = Cursors.Wait;
                // Lấy dữ liệu từ form điều kiện                
                List<HT_BAOCAO_TSO> listHtBaoCaoTso = lstHtBaoCaoTso;
                List<ThamSoBaoCao> listThamSoBaoCao = new List<ThamSoBaoCao>();
                DatabaseConstant.Action action = DatabaseConstant.Action.IN;
                MethodInfo mi;
                if (uc != null)
                {
                    mi = uc.GetType().GetMethod("GetParameters");
                }
                else
                {
                    Mouse.OverrideCursor = Cursors.Arrow;
                    LMessage.ShowMessage("Missing report conditional form", LMessage.MessageBoxType.Error);
                    return;
                }

                object ret = mi.Invoke(uc, null);
                if (ret != null)
                {
                    listThamSoBaoCao = (List<ThamSoBaoCao>)ret;
                }
                else
                {
                    Mouse.OverrideCursor = Cursors.Arrow;
                    return;
                }
                DataSet ds = new DataSet();
                // Chuẩn bị điều kiện cho báo cáo
                if (listThamSoBaoCao != null && listThamSoBaoCao.Count > 0)
                {
                    if (listHtBaoCaoTso.Where(t => t.LOAI_TSO == ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri() && t.MA_TSO.Equals("@DT_THAMSO")).Count() > 0)
                    {
                        listHtBaoCaoTso = new List<HT_BAOCAO_TSO>();
                        foreach (ThamSoBaoCao thamSoBaoCao in listThamSoBaoCao)
                        {
                            HT_BAOCAO_TSO tso = new HT_BAOCAO_TSO();
                            tso.MA_TSO = thamSoBaoCao.MaThamSo;
                            tso.LOAI_TSO = thamSoBaoCao.LoaiThamSo;
                            tso.GTRI_TSO = thamSoBaoCao.GiaTriThamSo;
                            listHtBaoCaoTso.Add(tso);
                            if (!LObject.IsNullOrEmpty(thamSoBaoCao.DsThamSo))
                                ds = thamSoBaoCao.DsThamSo;
                        }
                    }
                    else
                    {
                        foreach (HT_BAOCAO_TSO htBaoCaoTso in listHtBaoCaoTso)
                        {
                            foreach (ThamSoBaoCao thamSoBaoCao in listThamSoBaoCao)
                            {
                                if (htBaoCaoTso.MA_TSO.Equals(thamSoBaoCao.MaThamSo) &&
                                    htBaoCaoTso.LOAI_TSO.Equals(thamSoBaoCao.LoaiThamSo))
                                {
                                    htBaoCaoTso.GTRI_TSO = thamSoBaoCao.GiaTriThamSo;
                                    break;
                                }
                                if (!LObject.IsNullOrEmpty(thamSoBaoCao.DsThamSo))
                                    ds = thamSoBaoCao.DsThamSo;
                            }
                        }
                    }
                }

                ApplicationConstant.ResponseStatus retStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                FileBase fileResponse = new FileBase();
                List<FileBase> lstFileResponse = new List<FileBase>();
                string responseMessage = null;

                if (htBaoCao.MA_BAOCAO.Equals(DatabaseConstant.DanhSachBaoCaoTheoDinhKy.TDVM_PHIEU_THEO_DOI_HOAN_TRA_TVIEN.layMaBaoCao()))
                    retStatus = process.LayDuLieu(htBaoCao, listHtBaoCaoTso, ref lstFileResponse, ref responseMessage, ds, action);
                else if (htBaoCao.MA_BAOCAO.Equals(DatabaseConstant.LayMaBaoCaoBTV(DatabaseConstant.DanhSachBaoCaoBTV.TDVM_LICH_THU_NO)))
                    retStatus = process.LayDuLieu(htBaoCao, listHtBaoCaoTso, ref fileResponse, ref responseMessage, ds, action);          
                else
                    retStatus = process.LayDuLieu(htBaoCao, listHtBaoCaoTso, ref fileResponse, ref responseMessage, ds, action);

                if (retStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    string fileReport = "";
                    string folderReport = ClientInformation.TempDir;

                    if (htBaoCao.MA_BAOCAO.Equals(DatabaseConstant.DanhSachBaoCaoTheoDinhKy.TDVM_PHIEU_THEO_DOI_HOAN_TRA_TVIEN.layMaBaoCao()))
                    {
                        folderReport = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                        folderReport = folderReport + "\\Hoan_Tra_TV";
                        if (!lstFileResponse.IsNullOrEmpty() && lstFileResponse.Count > 0)
                        {                            
                            if (Directory.Exists(folderReport))
                            {
                                LFile.DeleteFolder(folderReport);
                                Directory.CreateDirectory(folderReport);
                            }
                            else
                            {
                                Directory.CreateDirectory(folderReport);
                            }
                            foreach (FileBase item in lstFileResponse)
                                ShowFile(ref folderReport, ref fileReport, false, item, mi, listThamSoBaoCao);

                            //OnShowResult
                            if (uc != null)
                            {
                                mi = uc.GetType().GetMethod("OnShowResult");
                                //Object[] paras = new Object[] { fileReport};
                                object retShowResult = mi.Invoke(uc, new Object[] { fileReport });
                            }
                        }
                        else
                        {
                            ShowFile(ref folderReport, ref fileReport, true, fileResponse, mi, listThamSoBaoCao);
                        }
                    
                    }
                    else if (htBaoCao.MA_BAOCAO.Equals(DatabaseConstant.DanhSachBaoCaoTheoDinhKy.GDKT_KET_XUAT_BCDKT_THEO_CAU_TRUC.layMaBaoCao()))
                    {
                        folderReport = System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                        ShowFile(ref folderReport, ref fileReport, true, fileResponse, mi, listThamSoBaoCao);
                    }
                    else
                    {
                        ShowFile(ref folderReport, ref fileReport, true, fileResponse, mi, listThamSoBaoCao);
                    }
                }
                else
                {
                    Mouse.OverrideCursor = Cursors.Arrow;
                    LMessage.ShowMessage(responseMessage, LMessage.MessageBoxType.Error);
                    return;
                }

                /*
                FileBase fileResponse = process.LayDuLieuBaoCao();
                string fileReport = ClientInformation.TempDir + "\\" + fileResponse.FileName + ".pdf";
                LFile.WriteFileFromByteArray(fileResponse.FileData, fileReport);

                // show file
                Stream stream = LFile.ConvertByteArrayToStream(fileResponse.FileData);

                System.Diagnostics.Process.Start(fileReport);
                 */
            }
            catch (System.Exception ex)
            {
                Mouse.OverrideCursor = Cursors.Arrow;
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }

        }

        private void ShowFile(ref string folderReport, ref string fileReport, bool isOpenFile, FileBase fileResponse, MethodInfo mi, List<ThamSoBaoCao> listThamSoBaoCao)
        {
            try
            {
                if (fileResponse.FileFormat.Equals(ApplicationConstant.LoaiDinhDangBaoCao.TEXT.layGiaTri()))
                    fileReport = folderReport + "\\" + fileResponse.FileName;
                else
                    fileReport = folderReport + "\\" + fileResponse.FileName + "." + fileResponse.FileFormat;

                LFile.WriteFileFromByteArray(fileResponse.FileData, fileReport);
                if (isOpenFile)
                {
                    // show file 
                    if (fileResponse.FileFormat == "rar")
                    {
                        LZip.UnZipFiles(fileReport, folderReport, "ng-mFina", false);
                        string format = "";
                        string loaiThamSo = ApplicationConstant.LoaiThamSoBaoCao.FORMAT.layGiaTri();
                        format = listThamSoBaoCao.Where(item => item.LoaiThamSo.Equals(loaiThamSo)).FirstOrDefault().GiaTriThamSo;
                        string originalFormat = "";
                        if (format.Equals(ApplicationConstant.LoaiDinhDangBaoCao.PDF.layGiaTri()))
                        {
                            originalFormat = "pdf";
                        }
                        else if (format.Equals(ApplicationConstant.LoaiDinhDangBaoCao.EXCEL.layGiaTri()))
                        {
                            originalFormat = "xls";
                        }
                        else if (format.Equals(ApplicationConstant.LoaiDinhDangBaoCao.WORD.layGiaTri()))
                        {
                            originalFormat = "doc";
                        }
                        else if (format.Equals(ApplicationConstant.LoaiDinhDangBaoCao.TEXT.layGiaTri()))
                        {
                            originalFormat = "txt";
                        }
                        else
                        {
                            originalFormat = "pdf";
                        }
                        string originalFileReport = ClientInformation.TempDir + "\\" + fileResponse.FileName + "." + originalFormat;
                        //Stream stream = LFile.ConvertByteArrayToStream(fileResponse.FileData);
                        Mouse.OverrideCursor = Cursors.Arrow;
                        System.Diagnostics.Process.Start(originalFileReport);
                    }
                    else
                    {
                        //Stream stream = LFile.ConvertByteArrayToStream(fileResponse.FileData);
                        Mouse.OverrideCursor = Cursors.Arrow;

                        if (fileResponse.FileFormat.Equals(ApplicationConstant.LoaiDinhDangBaoCao.TEXT.layGiaTri()))
                        {
                            //OnShowResult
                            if (uc != null)
                            {
                                mi = uc.GetType().GetMethod("OnShowResult");
                                //Object[] paras = new Object[] { fileReport};
                                object retShowResult = mi.Invoke(uc, new Object[] { fileReport });
                            }
                        }
                        else
                        {
                            System.Diagnostics.Process.Start(fileReport);
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                Mouse.OverrideCursor = Cursors.Arrow;
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }
    }
}
