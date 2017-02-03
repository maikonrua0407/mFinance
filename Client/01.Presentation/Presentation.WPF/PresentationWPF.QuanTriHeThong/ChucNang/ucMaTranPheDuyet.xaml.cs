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
using System.Xml.Linq;
using System.Windows.Threading;
using System.Data;
using PresentationWPF.CustomControl;
using Presentation.Process.QuanTriHeThongServiceRef;
using Presentation.Process;
using Presentation.Process.Common;
using Telerik.Windows.Controls.GridView;

namespace PresentationWPF.QuanTriHeThong.ChucNang
{
    /// <summary>
    /// Interaction logic for ucMaTranPheDuyet.xaml
    /// </summary>
    public partial class ucMaTranPheDuyet : UserControl
    {
        #region Khai bao

        List<AutoCompleteEntry> lstSourcePhanHe = new List<AutoCompleteEntry>();

        List<HT_CNANG> dsCNangAll = new List<HT_CNANG>();

        DataTable dt = new DataTable();
        QuanTriHeThongProcess qtht = new QuanTriHeThongProcess();
        bool isLoaded = false;

        public static RoutedCommand SaveCommand = new RoutedCommand();
        public static RoutedCommand ExportCommand = new RoutedCommand();
        public static RoutedCommand HelpCommand = new RoutedCommand();
        public static RoutedCommand CloseCommand = new RoutedCommand();

        string strGridMain = "";

        static List<DsNhomCap> lstNhomCap;

        DataSet dataSource = new DataSet();

        #endregion

        #region Khoi tao

        public ucMaTranPheDuyet()
        {
            try
            {
                InitializeComponent();
                LoadDuLieu();
            }
            catch (Exception ex)
            { }
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
                    if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.N, ModifierKeys.Control);
                        key = new KeyBinding(SaveCommand, keyg);
                        key.Gesture = keyg;
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XUAT_DU_LIEU)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.E, ModifierKeys.Control | ModifierKeys.Shift);
                        key = new KeyBinding(ExportCommand, keyg);
                        key.Gesture = keyg;
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRO_GIUP)))
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
                Luu();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XUAT_DU_LIEU)))
            { }
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

        private void SaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Luu();
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

        }

        private void CloseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void CloseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            CustomControl.CommonFunction.CloseUserControl(this);
        }

        #endregion

        #region Xu ly nghiep vu

        private void Luu()
        {
            //Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                string maTranPheDuyet = getObject();
                DataRowView dr = (DataRowView)grDanhSach.SelectedItem;
                string maChucNang = dr["MA"].ToString();

                HT_CNANG htCNang = new HT_CNANG();
                htCNang.MA_CNANG = maChucNang;
                htCNang.MA_TRAN_PHE_DUYET = maTranPheDuyet;

                ApplicationConstant.ResponseStatus ret = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                string responseMessage = "";

                ret = qtht.MaTranPheDuyet(ref htCNang, ref responseMessage);
                if (ret == ApplicationConstant.ResponseStatus.THANH_CONG)
                    LMessage.ShowMessage("Thiết lập ma trận phê duyệt thành công cho chức năng được chọn", LMessage.MessageBoxType.Information);
                else
                    LMessage.ShowMessage(responseMessage, LMessage.MessageBoxType.Warning);
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            //Mouse.OverrideCursor = Cursors.Arrow;
        }

        #endregion

        #region Xu ly giao dien

        private void LoadDuLieu()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                // lấy dữ liệu đổ source cho combobox Phân hệ chức năng
                AutoComboBox auto = new AutoComboBox();
                List<string> lstDieuKien = new List<string>();
                lstDieuKien.Add(DatabaseConstant.DanhMuc.DANH_MUC_PHAN_HE.getValue());
                //lstSourcePhanHe.Add(new AutoCompleteEntry("Tất cả",""));
                auto.GenAutoComboBox(ref lstSourcePhanHe, ref cmbPhanHeChucNang, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien);
                // Nếu là M7MFI
                if (ClientInformation.Company.Equals(ApplicationConstant.DonViSuDung.M7MFI.layGiaTri()))
                {
                    // Không thực hiện cho TDTT, BHTH, QLTS
                    auto.removeEntry(ref lstSourcePhanHe, ref cmbPhanHeChucNang, DatabaseConstant.Module.TDTT.getValue());
                    auto.removeEntry(ref lstSourcePhanHe, ref cmbPhanHeChucNang, DatabaseConstant.Module.BHTH.getValue());
                    //auto.removeEntry(ref lstSourcePhanHe, ref cmbPhanHeChucNang, DatabaseConstant.Module.NSTL.getValue());
                    //auto.removeEntry(ref lstSourcePhanHe, ref cmbPhanHeChucNang, DatabaseConstant.Module.KTDL.getValue());
                    auto.removeEntry(ref lstSourcePhanHe, ref cmbPhanHeChucNang, DatabaseConstant.Module.QLTS.getValue());
                }
                else if (ClientInformation.Company.Equals(ApplicationConstant.DonViSuDung.BINHKHANH.layGiaTri()))
                {
                    // Không thực hiện cho TDTT, BHTH, KTDL, QLTS
                    auto.removeEntry(ref lstSourcePhanHe, ref cmbPhanHeChucNang, DatabaseConstant.Module.TDTT.getValue());
                    auto.removeEntry(ref lstSourcePhanHe, ref cmbPhanHeChucNang, DatabaseConstant.Module.BHTH.getValue());
                    //auto.removeEntry(ref lstSourcePhanHe, ref cmbPhanHeChucNang, DatabaseConstant.Module.NSTL.getValue());
                    auto.removeEntry(ref lstSourcePhanHe, ref cmbPhanHeChucNang, DatabaseConstant.Module.KTDL.getValue());
                    auto.removeEntry(ref lstSourcePhanHe, ref cmbPhanHeChucNang, DatabaseConstant.Module.QLTS.getValue());
                }
                else if (ClientInformation.Company.Equals(ApplicationConstant.DonViSuDung.BANTAYVANG.layGiaTri()))
                {
                    // Chỉ thực hiện cho TDVM
                    auto.removeEntry(ref lstSourcePhanHe, ref cmbPhanHeChucNang, DatabaseConstant.Module.QTHT.getValue());
                    auto.removeEntry(ref lstSourcePhanHe, ref cmbPhanHeChucNang, DatabaseConstant.Module.DMDC.getValue());
                    auto.removeEntry(ref lstSourcePhanHe, ref cmbPhanHeChucNang, DatabaseConstant.Module.KHTV.getValue());
                    auto.removeEntry(ref lstSourcePhanHe, ref cmbPhanHeChucNang, DatabaseConstant.Module.GDKT.getValue());
                    auto.removeEntry(ref lstSourcePhanHe, ref cmbPhanHeChucNang, DatabaseConstant.Module.HDVO.getValue());
                    auto.removeEntry(ref lstSourcePhanHe, ref cmbPhanHeChucNang, DatabaseConstant.Module.TDTT.getValue());
                    auto.removeEntry(ref lstSourcePhanHe, ref cmbPhanHeChucNang, DatabaseConstant.Module.BHTH.getValue());
                    auto.removeEntry(ref lstSourcePhanHe, ref cmbPhanHeChucNang, DatabaseConstant.Module.NSTL.getValue());
                    auto.removeEntry(ref lstSourcePhanHe, ref cmbPhanHeChucNang, DatabaseConstant.Module.KTDL.getValue());
                    auto.removeEntry(ref lstSourcePhanHe, ref cmbPhanHeChucNang, DatabaseConstant.Module.QLTS.getValue());
                }
                else 
                {
                    // Chỉ thực hiện cho TDVM
                    auto.removeEntry(ref lstSourcePhanHe, ref cmbPhanHeChucNang, DatabaseConstant.Module.QTHT.getValue());
                    auto.removeEntry(ref lstSourcePhanHe, ref cmbPhanHeChucNang, DatabaseConstant.Module.DMDC.getValue());
                    auto.removeEntry(ref lstSourcePhanHe, ref cmbPhanHeChucNang, DatabaseConstant.Module.KHTV.getValue());
                    auto.removeEntry(ref lstSourcePhanHe, ref cmbPhanHeChucNang, DatabaseConstant.Module.GDKT.getValue());
                    auto.removeEntry(ref lstSourcePhanHe, ref cmbPhanHeChucNang, DatabaseConstant.Module.HDVO.getValue());
                    auto.removeEntry(ref lstSourcePhanHe, ref cmbPhanHeChucNang, DatabaseConstant.Module.TDTT.getValue());
                    auto.removeEntry(ref lstSourcePhanHe, ref cmbPhanHeChucNang, DatabaseConstant.Module.BHTH.getValue());
                    auto.removeEntry(ref lstSourcePhanHe, ref cmbPhanHeChucNang, DatabaseConstant.Module.NSTL.getValue());
                    auto.removeEntry(ref lstSourcePhanHe, ref cmbPhanHeChucNang, DatabaseConstant.Module.KTDL.getValue());
                    auto.removeEntry(ref lstSourcePhanHe, ref cmbPhanHeChucNang, DatabaseConstant.Module.QLTS.getValue());
                }

                // Hiển thị lưới dữ liệu phân quyền
                BuildGrid();
                LoadContent();
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void LoadContent()
        { 
            try
            {
                if (grDanhSach.SelectedItems.Count > 0)
                {
                    string strResource = string.Empty;
                    DataRowView dr = (DataRowView)grDanhSach.SelectedItem;
                    string maChucNang = dr["MA"].ToString();
                    if (!maChucNang.IsNullOrEmptyOrSpace())
                    {
                        //strResource = "0-20(" + maChucNang.Substring(0, 1) + "1, " + maChucNang.Substring(0, 1) + "2," + maChucNang.Substring(0, 1) + "3;" + maChucNang.Substring(0, 1) + "I," + maChucNang.Substring(0, 1) + "II," + maChucNang.Substring(0, 1) + "III@DDU1^" + maChucNang.Substring(0, 1) + "1, " + maChucNang.Substring(0, 1) + "2," + maChucNang.Substring(0, 1) + "3;" + maChucNang.Substring(0, 1) + "I," + maChucNang.Substring(0, 1) + "II," + maChucNang.Substring(0, 1) + "III@DDU2)#20-50(" + maChucNang.Substring(0, 1) + "1, " + maChucNang.Substring(0, 1) + "2," + maChucNang.Substring(0, 1) + "3;" + maChucNang.Substring(0, 1) + "I," + maChucNang.Substring(0, 1) + "II," + maChucNang.Substring(0, 1) + "III@DDU1)#50-+(" + maChucNang.Substring(0, 1) + "1, " + maChucNang.Substring(0, 1) + "2," + maChucNang.Substring(0, 1) + "3;" + maChucNang.Substring(0, 1) + "I," + maChucNang.Substring(0, 1) + "II," + maChucNang.Substring(0, 1) + "III@DDU1)";
                        strResource = dr["AP"].ToString();
                    }
                    if (!strResource.IsNullOrEmptyOrSpace())
                    {
                        lstNhomCap = new List<DsNhomCap>();
                        strGridMain = "";
                        createMainGrid();
                        string[] group = strResource.SplitByDelimiter("#");
                        string addRowGroup = "";
                        for (int i = 0; i < group.Count(); i++)
                        {
                            addRowGroup += "\n" + "        <RowDefinition />";
                            addGroup(group[i].SplitByDelimiter("(")[0].SplitByDelimiter("-")[0], group[i].SplitByDelimiter("(")[0].SplitByDelimiter("-")[1]);
                            string[] cap = group[i].SplitByDelimiter("(")[1].Replace(")", "").SplitByDelimiter("^");
                            for (int j = 0; j < cap.Count(); j++)
                            {
                                addCap("NhomCap" + i, cap[j].SplitByDelimiter("@")[1], cap[j].SplitByDelimiter("@")[0]);
                            }
                        }
                        strGridMain = strGridMain.Replace("<Grid.RowDefinitions>", "<Grid.RowDefinitions>" + addRowGroup);
                        string strMain = strGridMain;
                        ShowForm(strMain); ;
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

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!isLoaded)
            {
                // Khởi tạo các sự kiện cho control
                cmbPhanHeChucNang.SelectionChanged += cmbPhanHeChucNang_SelectionChanged;
                grDanhSach.SelectionChanged += grDanhSach_SelectionChanged;
                //grDanhSach.SelectedCellsChanged += grDanhSach_SelectedCellsChanged;
                isLoaded = true;
            }
        }

        private void BuildGrid()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                // Lấy chức năng theo phân hệ
                string maPhanHe = lstSourcePhanHe.ElementAt(cmbPhanHeChucNang.SelectedIndex).KeywordStrings.First();

                dsCNangAll = qtht.layCNangThietLapAPTheoPhanHe(maPhanHe);

                // Lấy CNangTnang theo chức năng trong phân hệ
                List<int> lstIdChucNang = dsCNangAll.Select(e => e.ID).Distinct().ToList();
                
                // Khởi tạo Table source: STT, Mã, Tên 
                dt = new DataTable();
                dt.Columns.Add("STT", typeof(int));
                dt.Columns.Add("MA", typeof(string));
                dt.Columns.Add("CHUCNANG", typeof(string));
                dt.Columns.Add("AP", typeof(string));

                int stt = 0;

                foreach (var item in dsCNangAll)
                {
                    DataRow r = dt.NewRow();

                    stt = stt + 1;
                    r[0] = stt;
                    r[1] = item.MA_CNANG;
                    r[2] = item.TEN_CNANG;
                    r[3] = item.MA_TRAN_PHE_DUYET;
                    dt.Rows.Add(r);
                }

                // đổ source lên lưới
                grDanhSach.ItemsSource = null;
                grDanhSach.ItemsSource = dt.DefaultView;
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private string layTenDonViTheoDanhSach(string maDonVi, List<DM_DON_VI> listDonVi)
        {
            foreach (DM_DON_VI item in listDonVi)
            {
                if (maDonVi.Equals(item.MA_DVI))
                    return item.TEN_GDICH;
            }
            return "";
        }

        private void loadWidthColumn()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                for (int i = 0; i < grDanhSach.Columns.Count; i++)
                {
                    if (i == 0)
                    {
                        grDanhSach.Columns[i].Width = new Telerik.Windows.Controls.GridViewLength(40, Telerik.Windows.Controls.GridViewLengthUnitType.Pixel);
                        grDanhSach.Columns[i].IsReadOnly = true;
                    }
                    else if (i == 1)
                        grDanhSach.Columns[i].IsVisible = false;
                    else if (i == 2)
                    {
                        grDanhSach.Columns[i].Width = new Telerik.Windows.Controls.GridViewLength(1, Telerik.Windows.Controls.GridViewLengthUnitType.Star);
                        grDanhSach.Columns[i].IsReadOnly = true;
                    }
                    else if (i == 3)
                        grDanhSach.Columns[i].IsVisible = false;
                    else
                    {
                        grDanhSach.Columns[i].Width = new Telerik.Windows.Controls.GridViewLength(1, Telerik.Windows.Controls.GridViewLengthUnitType.Auto);
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

        private void cmbPhanHeChucNang_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BuildGrid();
            loadWidthColumn();
        }

        private void grDanhSach_Loaded(object sender, RoutedEventArgs e)
        {
            loadWidthColumn();
        }

        private void grDanhSach_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            LoadContent();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (strGridMain.IsNullOrEmptyOrSpace())
                createMainGrid();
            strGridMain = strGridMain.Replace("<Grid.RowDefinitions>", "<Grid.RowDefinitions>\n" + "        <RowDefinition />");
            string strMain = strGridMain;
            addGroup();
            ShowForm(strMain);
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            RibbonButton btnRemove = (RibbonButton)sender;
            lstNhomCap.RemoveAt(Convert.ToInt32(btnRemove.Tag.ToString()));
            strGridMain = strGridMain.Replace("<Grid.RowDefinitions>\n" + "        <RowDefinition />", "<Grid.RowDefinitions>");
            string strMain = strGridMain;
            ShowForm(strMain);
        }

        private void btnAddContent_Click(object sender, RoutedEventArgs e)
        {
            RibbonButton btnAddContent = (RibbonButton)sender;
            string strMain = strGridMain;
            addCap(btnAddContent.Tag.ToString());
            ShowForm(strMain);
        }

        private void btnContentRemove_Click(object sender, RoutedEventArgs e)
        {
            RibbonButton btnContentRemove = (RibbonButton)sender;
            string[] tag = btnContentRemove.Tag.ToString().SplitByDelimiter("#");

            DsNhomCap itemNhomCap = lstNhomCap[Convert.ToInt32(tag[0].Replace("NhomCap", ""))];
            itemNhomCap.LstCap.RemoveAt(Convert.ToInt32(tag[1]));
            itemNhomCap.StrGroupBox = itemNhomCap.StrGroupBox.Replace("<Grid.RowDefinitions>\n" + "                <RowDefinition Height=\"30\" />", "<Grid.RowDefinitions>");
            string strMain = strGridMain;
            ShowForm(strMain);
        }

        private void createMainGrid()
        {
            strGridMain += "<UserControl";
            strGridMain += "\n" + "xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"";
            strGridMain += "\n" + "xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\"";
            strGridMain += "\n" + "xmlns:telerik=\"http://schemas.telerik.com/2008/xaml/presentation\"";
            strGridMain += "\n" + "xmlns:ribbon=\"clr-namespace:Microsoft.Windows.Controls.Ribbon;assembly=RibbonControlsLibrary\"";
            strGridMain += "\n" + "xmlns:uc=\"clr-namespace:PresentationWPF.CustomControl;assembly=PresentationWPF.CustomControl\" >";


            strGridMain += "\n" + "<ScrollViewer VerticalScrollBarVisibility=\"Auto\">";
            strGridMain += "\n" + "    <Grid>";
            strGridMain += "\n" + "        <Grid.RowDefinitions>";
            strGridMain += "\n" + "            <RowDefinition />";
            strGridMain += "\n" + "        </Grid.RowDefinitions>";
            strGridMain += "\n" + "        <Grid.ColumnDefinitions>";
            strGridMain += "\n" + "            <ColumnDefinition Width=\"50\" />";
            strGridMain += "\n" + "            <ColumnDefinition />";
            strGridMain += "\n" + "        </Grid.ColumnDefinitions>";
            strGridMain += "\n" + "        GroupBoxsContent";// Tạo các Group bên trong
            strGridMain += "\n" + "    </Grid>";
            strGridMain += "\n" + "</ScrollViewer>";
            strGridMain += "\n" + "</UserControl>";
        }

        private void addGroup(string txtTu = "", string txtDen = "")
        {
            int ind = 0;
            if (!lstNhomCap.IsNullOrEmpty() && lstNhomCap.Count > 0)
                ind = lstNhomCap.Count;
            else
                lstNhomCap = new List<DsNhomCap>();

            DsNhomCap nhomCap = new DsNhomCap();
            nhomCap.MaNhomCap = "NhomCap" + ind;

            string groupBox = "";
            groupBox += "\n" + "    <ribbon:RibbonButton Grid.Row=\"" + ind + "\" Grid.Column=\"0\" VerticalAlignment=\"Top\" Name=\"btnRemove" + ind + "\" Tag=\"" + ind + "\" SmallImageSource=\"pack://application:,,,/Utilities.Common;component/Images/Action/detail_delete.png\" Width=\"30\" />";
            groupBox += "\n" + "    <GroupBox Name=\"grbGroup" + ind + "\" Tag=\"" + nhomCap.MaNhomCap + "\" Grid.Row=\"" + ind + "\" Grid.Column=\"1\" Header=\"Nhóm " + (ind + 1) + "\" VerticalAlignment=\"Stretch\">";
            //groupBox += "\n" + "         <ScrollViewer VerticalScrollBarVisibility=\"Auto\">";
            groupBox += "\n" + "            <Grid>";
            groupBox += "\n" + "                <Grid.RowDefinitions>";
            groupBox += "\n" + "                    <RowDefinition Height=\"30\" />";
            groupBox += "\n" + "                </Grid.RowDefinitions>";
            groupBox += "\n" + "                <Grid.ColumnDefinitions>";
            groupBox += "\n" + "                    <ColumnDefinition Width=\"50\" />";
            groupBox += "\n" + "                    <ColumnDefinition Width=\"*\" />";
            groupBox += "\n" + "                    <ColumnDefinition Width=\"80\" />";
            groupBox += "\n" + "                    <ColumnDefinition Width=\"2*\" />";
            groupBox += "\n" + "                    <ColumnDefinition Width=\"100\" />";
            groupBox += "\n" + "                    <ColumnDefinition Width=\"2*\" />";
            groupBox += "\n" + "                    <ColumnDefinition Width=\"50\" />";
            groupBox += "\n" + "                </Grid.ColumnDefinitions>";
            //groupBox += "\n" + "                <StackPanel Orientation=\"Horizontal\" Grid.ColumnSpan=\"6\">";
            groupBox += "\n" + "                    <Label Margin=\"5,0,0,0\" Grid.Row=\"0\" Grid.Column=\"0\" HorizontalAlignment=\"Right\" Content=\"Từ:\" />";
            groupBox += "\n" + "                    <TextBox Margin=\"3\" Grid.Row=\"0\" Grid.Column=\"1\" VerticalAlignment=\"Top\" HorizontalAlignment=\"Left\" Name=\"txtTu" + ind + "\" Tag=\"" + nhomCap.MaNhomCap + "\" Text=\"" + txtTu + "\" Width=\"120\"  />";
            groupBox += "\n" + "                    <Label Margin=\"5,0,0,0\" Grid.Row=\"0\" Grid.Column=\"2\" HorizontalAlignment=\"Right\" Content=\"Đến:\" />";
            groupBox += "\n" + "                    <TextBox Margin=\"3\" Grid.Row=\"0\" Grid.Column=\"3\" VerticalAlignment=\"Top\" HorizontalAlignment=\"Left\" Name=\"txtDen" + ind + "\" Tag=\"" + nhomCap.MaNhomCap + "\" Text=\"" + txtDen + "\" Width=\"120\"  />";
            //groupBox += "\n" + "                </StackPanel>";
            groupBox += "\n" + "                <ribbon:RibbonButton Margin=\"3\" Grid.Row=\"0\" Grid.Column=\"6\" Name=\"btnAddContent" + ind + "\" Tag=\"" + nhomCap.MaNhomCap + "\" SmallImageSource=\"pack://application:,,,/Utilities.Common;component/Images/Action/detail_add.png\" Width=\"30\" />";
            groupBox += "\n" + "                ControlContent";// Tạo các Control bên trong Group
            groupBox += "\n" + "            </Grid>";
            //groupBox += "\n" + "         </ScrollViewer>";
            groupBox += "\n" + "    </GroupBox>";

            nhomCap.StrGroupBox = groupBox;
            lstNhomCap.Add(nhomCap);
        }

        private void addCap(string maNhomCap, string maCap = "", string value = "")
        {
            DsNhomCap nhomCap = lstNhomCap.FirstOrDefault(n => n.MaNhomCap.Equals(maNhomCap));
            if (!nhomCap.IsNullOrEmpty())
            {
                string nsd = "";
                string nnsd = "";
                if (!value.IsNullOrEmptyOrSpace())
                {
                    string[] objNSD = value.SplitByDelimiter(";")[0].SplitByDelimiter(",");
                    string[] objNNSD = value.SplitByDelimiter(";")[1].SplitByDelimiter(",");
                    foreach (string str in objNSD)
                        nsd += str + ",";
                    foreach (string str in objNNSD)
                        nnsd += str + ",";
                    if (!nsd.IsNullOrEmptyOrSpace())
                        nsd = nsd.Substring(0, nsd.Length - 1);
                    if (!nnsd.IsNullOrEmptyOrSpace())
                        nnsd = nnsd.Substring(0, nnsd.Length - 1);
                }
                int ind = 0;
                if (!nhomCap.LstCap.IsNullOrEmpty() && nhomCap.LstCap.Count > 0)
                    ind = nhomCap.LstCap.Count;
                string strCap = "";
                strCap += "\n" + "            <Label Margin=\"5,0,0,0\" Grid.Row=\"" + (ind + 1) + "\" Grid.Column=\"0\" VerticalAlignment=\"Top\" HorizontalAlignment=\"Right\" Content=\"Cấp:\" />";
                strCap += "\n" + "            <uc:AutoCompleteTextBox DelayTime=\"100\" Threshold=\"2\" Margin=\"3\" Grid.Row=\"" + (ind + 1) + "\" Grid.Column=\"1\" VerticalAlignment=\"Top\" x:Name=\"txtMaCap" + nhomCap.MaNhomCap + "_" + ind + "\" Tag=\"" + nhomCap.MaNhomCap + "\" HorizontalAlignment=\"Stretch\" Text=\"" + maCap + "\" />";
                strCap += "\n" + "            <Label Margin=\"5,0,0,0\" Grid.Row=\"" + (ind + 1) + "\" Grid.Column=\"2\" VerticalAlignment=\"Top\" HorizontalAlignment=\"Right\" Content=\"NSD:\" />";
                strCap += "\n" + "            <uc:AutoCompleteTextBox DelayTime=\"100\" Threshold=\"2\" Margin=\"3\" Grid.Row=\"" + (ind + 1) + "\" Grid.Column=\"3\" VerticalAlignment=\"Top\" x:Name=\"txtCapNhomCapNSD" + nhomCap.MaNhomCap + "_" + ind + "\" Tag=\"" + nhomCap.MaNhomCap + "\" HorizontalAlignment=\"Stretch\" Text=\"" + nsd + "\" />";
                strCap += "\n" + "            <Label Margin=\"5,0,0,0\" Grid.Row=\"" + (ind + 1) + "\" Grid.Column=\"4\" VerticalAlignment=\"Top\" HorizontalAlignment=\"Right\" Content=\"Nhóm NSD:\" />";
                strCap += "\n" + "            <uc:AutoCompleteTextBox DelayTime=\"100\" Threshold=\"2\" Margin=\"3\" Grid.Row=\"" + (ind + 1) + "\" Grid.Column=\"5\" VerticalAlignment=\"Top\" x:Name=\"txtCapNhomCapNNSD" + nhomCap.MaNhomCap + "_" + ind + "\" Tag=\"" + nhomCap.MaNhomCap + "\" HorizontalAlignment=\"Stretch\" Text=\"" + nnsd + "\" />";
                strCap += "\n" + "            <ribbon:RibbonButton Grid.Row=\"" + (ind + 1) + "\" Grid.Column=\"6\" Name=\"btnContentRemove" + nhomCap.MaNhomCap + "_" + ind + "\" Tag=\"" + nhomCap.MaNhomCap + "#" + ind + "\" SmallImageSource=\"pack://application:,,,/Utilities.Common;component/Images/Action/detail_delete.png\" Width=\"30\" />";
                if (nhomCap.LstCap.IsNullOrEmpty())
                    nhomCap.LstCap = new List<string>();
                nhomCap.LstCap.Add(strCap);
                nhomCap.StrGroupBox = nhomCap.StrGroupBox.Replace("<Grid.RowDefinitions>", "<Grid.RowDefinitions>\n" + "                <RowDefinition Height=\"30\" />");
            }
        }

        private void EnabledRemoveButton()
        {
            List<RibbonButton> listRibbonButton = CustomerObject.GetLogicalChildCollection<RibbonButton>(this);
            foreach (RibbonButton rbtn in listRibbonButton)
            {
                if (rbtn.Name.Contains("btnContentRemove"))
                {
                    string[] tag = rbtn.Tag.ToString().SplitByDelimiter("#");

                    DsNhomCap itemNhomCap = lstNhomCap[Convert.ToInt32(tag[0].Replace("NhomCap", ""))];
                    if (Convert.ToInt32(tag[1]) == (itemNhomCap.LstCap.Count - 1))
                        rbtn.IsEnabled = true;
                    else
                        rbtn.IsEnabled = false;
                }
                else if (rbtn.Name.Contains("btnRemove"))
                {
                    if (Convert.ToInt32(rbtn.Tag.ToString()) == (lstNhomCap.Count - 1))
                        rbtn.IsEnabled = true;
                    else
                        rbtn.IsEnabled = false;
                }
            }
        }

        private void ShowForm(string strMain)
        {
            string strNhomCap = "";
            foreach (DsNhomCap nhomCap in lstNhomCap)
            {
                string tempNhomCap = nhomCap.StrGroupBox;
                string strCap = "";
                if (!nhomCap.LstCap.IsNullOrEmpty() && nhomCap.LstCap.Count > 0)
                {
                    foreach (string temCap in nhomCap.LstCap)
                    {
                        strCap += temCap;
                    }
                }
                tempNhomCap = tempNhomCap.Replace("ControlContent", strCap);
                strNhomCap += tempNhomCap;
            }
            strMain = strMain.Replace("GroupBoxsContent", strNhomCap);
            XElement ui = XElement.Parse(strMain);
            UserControl uc = (System.Windows.Controls.UserControl)System.Windows.Markup.XamlReader.Load(ui.CreateReader());
            CntContent.Content = uc;
            Dispatcher.CurrentDispatcher.DelayInvoke("InitSomething", () =>
            {
                List<RibbonButton> listRibbonButton = CustomerObject.GetLogicalChildCollection<RibbonButton>(this);
                foreach (RibbonButton rbtn in listRibbonButton)
                {
                    if (rbtn.Name.Contains("btnRemove"))
                        rbtn.Click += btnRemove_Click;
                    else if (rbtn.Name.Contains("btnAddContent"))
                        rbtn.Click += btnAddContent_Click;
                    else if (rbtn.Name.Contains("btnContentRemove"))
                        rbtn.Click += btnContentRemove_Click;
                }
                List<AutoCompleteTextBox> listAutoCompleteTextBox = CustomerObject.GetLogicalChildCollection<AutoCompleteTextBox>(this);
                List<AutoCompleteTextBox> listNSD = listAutoCompleteTextBox.Where(t => t.Name.Contains("txtCapNhomCapNSD")).ToList();
                List<AutoCompleteTextBox> listNNSD = listAutoCompleteTextBox.Where(t => t.Name.Contains("txtCapNhomCapNNSD")).ToList();

                List<HT_NSD> dsNSD = qtht.layNSD(ClientInformation.LoaiNguoiSuDung);
                List<HT_NHNSD> dsNhomNSD = qtht.layNhomNSD(ClientInformation.LoaiNguoiSuDung);
                if (!dsNSD.IsNullOrEmpty() && dsNSD.Count > 0)
                {
                    foreach (HT_NSD NSD in dsNSD)
                    {
                        foreach (AutoCompleteTextBox txt in listNSD)
                            txt.AddItem(new AutoCompleteEntry(NSD.MA_NSD, NSD.MA_NSD, NSD.TEN_GOI, NSD.TEN_HO_DEM));
                    }
                }

                if (!dsNhomNSD.IsNullOrEmpty() && dsNhomNSD.Count > 0)
                {
                    foreach (HT_NHNSD NhomNSD in dsNhomNSD)
                    {
                        foreach (AutoCompleteTextBox txt in listNNSD)
                            txt.AddItem(new AutoCompleteEntry(NhomNSD.MA_NHNSD, NhomNSD.MA_NHNSD, NhomNSD.TEN_NHNSD));
                    }
                }
                EnabledRemoveButton();
            }, TimeSpan.FromSeconds(0));
        }

        private string getObject()
        {
            // string strResource = "0-20(id1|ma1,id2|ma2,id3|ma3;idI|maI,idII|maII,idIII|maIII@DDU1^id1|ma1,id2|ma2,id3|ma3;idI|maI,idII|maII,idIII|maIII@DDU2)
            string result = "";
            List<TextBox> listTextBox = CustomerObject.GetLogicalChildCollection<TextBox>(this);
            List<AutoCompleteTextBox> listAutoCompleteTextBox = CustomerObject.GetLogicalChildCollection<AutoCompleteTextBox>(this);
            foreach (DsNhomCap nhomCap in lstNhomCap)
            {
                string group = "";
                int soCap = listAutoCompleteTextBox.Where(t => t.Name.Contains("txtMaCap") && t.Tag.ToString().Equals(nhomCap.MaNhomCap)).Count();
                List<AutoCompleteTextBox> subListTextBox = listAutoCompleteTextBox.Where(t => t.Tag.ToString().Equals(nhomCap.MaNhomCap)).ToList();
                for (int i = 0; i < soCap; i++)
                {
                    if (i > 0)
                        group += "^";
                    string strTMaCap = subListTextBox.FirstOrDefault(t => t.Name.Equals("txtMaCap" + nhomCap.MaNhomCap + "_" + i) && t.Tag.ToString().Equals(nhomCap.MaNhomCap)).Text;
                    string strCapNhomCapNSD = subListTextBox.FirstOrDefault(t => t.Name.Equals("txtCapNhomCapNSD" + nhomCap.MaNhomCap + "_" + i)).Text;
                    if (!strCapNhomCapNSD.IsNullOrEmptyOrSpace())
                    {
                        strCapNhomCapNSD = strCapNhomCapNSD.Trim().Replace(" ", "");
                        strCapNhomCapNSD = strCapNhomCapNSD.Substring(0, strCapNhomCapNSD.Length);
                    }
                    string strCapNhomCapNNSD = subListTextBox.FirstOrDefault(t => t.Name.Equals("txtCapNhomCapNNSD" + nhomCap.MaNhomCap + "_" + i)).Text;
                    if (!strCapNhomCapNNSD.IsNullOrEmptyOrSpace())
                    {
                        strCapNhomCapNNSD = strCapNhomCapNNSD.Trim().Replace(" ", "");
                        strCapNhomCapNNSD = strCapNhomCapNNSD.Substring(0, strCapNhomCapNNSD.Length);
                    }
                    group += strCapNhomCapNSD + ";" + strCapNhomCapNNSD + "@" + strTMaCap;
                }
                string strTu = listTextBox.FirstOrDefault(t => t.Name.Contains("txtTu") && t.Tag.ToString().Equals(nhomCap.MaNhomCap)).Text;
                string strDen = listTextBox.FirstOrDefault(t => t.Name.Contains("txtDen") && t.Tag.ToString().Equals(nhomCap.MaNhomCap)).Text;
                result += strTu + "-" + strDen + "(" + group + ")";
                result += "#";
            }
            if (!result.IsNullOrEmptyOrSpace())
                result = result.Substring(0, result.Length - 1);
            return result;
        }

        #endregion
    }
    partial class DsNhomCap
    {
        string maNhomCap;

        public string MaNhomCap
        {
            get { return maNhomCap; }
            set { maNhomCap = value; }
        }

        string strGroupBox;

        public string StrGroupBox
        {
            get { return strGroupBox; }
            set { strGroupBox = value; }
        }

        private List<string> lstCap;

        public List<string> LstCap
        {
            get { return lstCap; }
            set { lstCap = value; }
        }
    }
}
