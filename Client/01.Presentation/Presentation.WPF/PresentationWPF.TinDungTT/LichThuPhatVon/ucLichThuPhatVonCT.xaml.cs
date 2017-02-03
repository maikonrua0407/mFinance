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
using Microsoft.Windows.Controls.Ribbon;
using Utilities.Common;
using PresentationWPF.CustomControl;
using Presentation.Process.TinDungServiceRef;
using Presentation.Process.Common;
using Presentation.Process;
using Telerik.Windows.Controls;
using Presentation.Process.PopupServiceRef;
using System.Reflection;
using Telerik.Windows.Controls.GridView;
using PresentationWPF.BaoCao.DungChung;
using System.Collections;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace PresentationWPF.TinDungTT.LichThuPhatVon
{
    /// <summary>
    /// Interaction logic for ucLichThuPhatVonCT.xaml
    /// </summary>
    public partial class ucLichThuPhatVonCT : UserControl
    {
        #region Khai bao
        public static RoutedCommand AddCommand = new RoutedCommand();
        public static RoutedCommand ModifyCommand = new RoutedCommand();
        public static RoutedCommand DeleteCommand = new RoutedCommand();
        public static RoutedCommand ApproveCommand = new RoutedCommand();
        public static RoutedCommand RefuseCommand = new RoutedCommand();
        public static RoutedCommand CancelCommand = new RoutedCommand();
        public static RoutedCommand ViewCommand = new RoutedCommand();
        public static RoutedCommand SearchCommand = new RoutedCommand();
        public static RoutedCommand ReloadCommand = new RoutedCommand();
        public static RoutedCommand ExportCommand = new RoutedCommand();
        public static RoutedCommand HelpCommand = new RoutedCommand();
        public static RoutedCommand CloseCommand = new RoutedCommand();

        private string _namDL;
        private LICH_THU_PHAT_VON obj = null;
        List<CUM_LICH> lstPhatVon = null;
        List<CUM_LICH> lstThuVon = null;
        DataTable dtTreeDLy;
        #endregion

        #region Khoi tao
        public ucLichThuPhatVonCT()
        {
            InitializeComponent();
            HeThong htHeThong = new HeThong();
            htHeThong.DuyetQuyenTinhNangToolbar("/PresentationWPF.TinDungTT;component/LichThuPhatVon/ucLichThuPhatVonCT.xaml", ref Toolbar, ref mnuMain);
            foreach (var item in mnuMain.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += new RoutedEventHandler(btnShortcutKey_Click);
            }
            BindHotkey();
            BuildTreeKhuVuc();
            InitEvenHanler();
        }

        public void InitEvenHanler()
        {
            //radDanhSachLichPhat.CellEditEnded += radDanhSachLichPhat_CellEditEnded;
            radDanhSachLichPhat.BeginningEdit += radDanhSachLichPhat_BeginningEdit;
            radDanhSachLichThu.BeginningEdit += radDanhSachLichThu_BeginningEdit;
            ucPhatDateJan.SelectedDatesGridViewChanged += ucPhatControl_SelectedDatesGridViewChanged;
            ucPhatDateFeb.SelectedDatesGridViewChanged += ucPhatControl_SelectedDatesGridViewChanged;
            ucPhatDateMar.SelectedDatesGridViewChanged += ucPhatControl_SelectedDatesGridViewChanged;
            ucPhatDateApr.SelectedDatesGridViewChanged += ucPhatControl_SelectedDatesGridViewChanged;
            ucPhatDateMay.SelectedDatesGridViewChanged += ucPhatControl_SelectedDatesGridViewChanged;
            ucPhatDateJun.SelectedDatesGridViewChanged += ucPhatControl_SelectedDatesGridViewChanged;
            ucPhatDateJul.SelectedDatesGridViewChanged += ucPhatControl_SelectedDatesGridViewChanged;
            ucPhatDateAug.SelectedDatesGridViewChanged += ucPhatControl_SelectedDatesGridViewChanged;
            ucPhatDateSep.SelectedDatesGridViewChanged += ucPhatControl_SelectedDatesGridViewChanged;
            ucPhatDateOct.SelectedDatesGridViewChanged += ucPhatControl_SelectedDatesGridViewChanged;
            ucPhatDateNov.SelectedDatesGridViewChanged += ucPhatControl_SelectedDatesGridViewChanged;
            ucPhatDateDec.SelectedDatesGridViewChanged += ucPhatControl_SelectedDatesGridViewChanged;
            ucThuDateJan.SelectedDatesGridViewChanged += ucThuControl_SelectedDatesGridViewChanged;
            ucThuDateFeb.SelectedDatesGridViewChanged += ucThuControl_SelectedDatesGridViewChanged;
            ucThuDateMar.SelectedDatesGridViewChanged += ucThuControl_SelectedDatesGridViewChanged;
            ucThuDateApr.SelectedDatesGridViewChanged += ucThuControl_SelectedDatesGridViewChanged;
            ucThuDateMay.SelectedDatesGridViewChanged += ucThuControl_SelectedDatesGridViewChanged;
            ucThuDateJun.SelectedDatesGridViewChanged += ucThuControl_SelectedDatesGridViewChanged;
            ucThuDateJul.SelectedDatesGridViewChanged += ucThuControl_SelectedDatesGridViewChanged;
            ucThuDateAug.SelectedDatesGridViewChanged += ucThuControl_SelectedDatesGridViewChanged;
            ucThuDateSep.SelectedDatesGridViewChanged += ucThuControl_SelectedDatesGridViewChanged;
            ucThuDateOct.SelectedDatesGridViewChanged += ucThuControl_SelectedDatesGridViewChanged;
            ucThuDateNov.SelectedDatesGridViewChanged += ucThuControl_SelectedDatesGridViewChanged;
            ucThuDateDec.SelectedDatesGridViewChanged += ucThuControl_SelectedDatesGridViewChanged;
            tlbCopyDown.Click += new RoutedEventHandler(tlbCopyDown_Click);
            tlbCopyUp.Click += new RoutedEventHandler(tlbCopyUp_Click);
        }

        private void FormatData()
        {
            
        }

        void BuildTreeKhuVuc()
        {
            dtTreeDLy = new TinDungProcess().GetTreeViewKhuVuc(ClientInformation.MaDonViQuanLy, ClientInformation.TenDangNhap).Tables[0];
            try
            {
                RadTreeViewItem Item = new RadTreeViewItem();
                //Item.Header = "Danh mục địa lý";
                //Item.IsExpanded = true;
                //Item.IsChecked = true;
                //tvwKhuVuc.Items.Add(Item);
                BuildSubTreeKhuVuc(Item, null, 0);
            }
            catch (Exception ex)
            { }
        }

        void BuildSubTreeKhuVuc(RadTreeViewItem Item, DataRow dr, int iLevel)
        {

            List<DataRow> lstDataRow = null;
            if (dr != null)
                lstDataRow = dtTreeDLy.Select("MA_DVI_CHA='" + dr["MA_DVI"] + "' AND LEVEL=" + iLevel).ToList();
            else
                lstDataRow = dtTreeDLy.Select("LEVEL=0").ToList();
            foreach (DataRow row in lstDataRow)
            {
                RadTreeViewItem subItem = new RadTreeViewItem();
                subItem.Header = row["TEN_GDICH"].ToString();
                subItem.Tag = row["MA_DVI"].ToString();
                //subItem.IsExpanded = true;
                subItem.IsChecked = true;
                if (row["LEVEL"].Equals(0))
                    tvwKhuVuc.Items.Add(subItem);
                else
                    Item.Items.Add(subItem);
                BuildSubTreeKhuVuc(subItem, row, Convert.ToInt32(row["LEVEL"]) + 1);
            }
        }
        #endregion

        /// <summary>
        /// Dang ky hot key, shortcut key
        /// </summary>
        #region Dang ky hot key, shortcut key
        private void BindHotkey()
        {
            foreach (var child in Toolbar.Children)
            {
                if (child.GetType() == typeof(RibbonButton))
                {
                    RibbonButton tlb = (RibbonButton)child;
                    KeyBinding key = new KeyBinding();
                    string strTinhNang = tlb.Name.Substring(3, tlb.Name.Length - 3);
                    if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THEM)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.N, ModifierKeys.Control);
                        key = new KeyBinding(AddCommand, keyg);
                        key.Gesture = keyg;
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
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XEM)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.W, ModifierKeys.Control);
                        key = new KeyBinding(ViewCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TIM_KIEM)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.F, ModifierKeys.Control);
                        key = new KeyBinding(SearchCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LAY_LAI)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.F5, ModifierKeys.None);
                        key = new KeyBinding(ReloadCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XUAT_DU_LIEU)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.E, ModifierKeys.Control | ModifierKeys.Shift);
                        key = new KeyBinding(ExportCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRO_GIUP)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.F1, ModifierKeys.None);
                        key = new KeyBinding(HelpCommand, keyg);
                    }
                    //else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DONG)))
                    //{
                    //    KeyGesture keyg = new KeyGesture(Key.Escape, ModifierKeys.None);
                    //    key = new KeyBinding(ucDonViDS.HelpCommand, keyg);
                    //}

                    if (key != null)
                    {
                        InputBindings.Add(key);
                    }
                }
            }
        }

        private void AddCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void AddCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void ModifyCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void ModifyCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void DeleteCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void DeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void ApproveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void ApproveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void RefuseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void RefuseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void CancelCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void CancelCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void ViewCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void ViewCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {

        }

        private void SearchCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void SearchCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
   
        }

        private void ReloadCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void ReloadCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {

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
                OnSave();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
             
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
            {
            
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DUYET)))
            {
            
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET)))
            {
            
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
            {
            
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XEM)))
            {
                
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TIM_KIEM)))
            {
                LayDuLieu();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LAY_LAI)))
            { }
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
        /// <summary>
        /// Xử lý sự kiện keydown trên form
        /// Bao gồm:
        /// Nhấn Escape để thoát form
        /// Nhấn Enter/Tab để focus vào control tiếp theo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            // Kiểm tra escape thoát form
            PresentationWPF.CustomControl.CommonFunction.CloseUserControl(e, this);

            // Nhấn enter để chuyển focus tới control tiếp theo
            PresentationWPF.CustomControl.CommonFunction.SelectNextControl(e);
        }

        /// <summary>
        /// Sự kiện shortcutkey
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnShortcutKey_Click(object sender, RoutedEventArgs e)
        {
            string strTinhNang = ((MenuItem)sender).Name.Substring(3, ((MenuItem)sender).Name.Length - 3);
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU)))
            {
                OnSave();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            { }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DUYET)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XEM)))
            {
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TIM_KIEM)))
            {
                LayDuLieu();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LAY_LAI)))
            { }
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

        /// <summary>
        /// Trợ giúp
        /// </summary>
        private void onHelp()
        {
            CustomControl.CommonFunction.ShowHelp(this);
        }

        #endregion

        #region Xy ly giao dien
        //void radDanhSachLichPhat_CellEditEnded(object sender, GridViewCellEditEndedEventArgs e)
        //{
        //    CUM_LICH objCumLich = e.Cell.ParentRow.Item as CUM_LICH;
        //    Grid gridparent = e.EditingElement as Grid;
        //    int index = lstPhatVon.IndexOf(objCumLich);
        //    lstPhatVon[index] = objCumLich;
        //}

        void ucPhatControl_SelectedDatesGridViewChanged(object sender, CustomControl.SelectedDatesChangedGridViewEventArgs e)
        {
            CUM_LICH objCumLich = e.Cell.ParentRow.Item as CUM_LICH;
            string ValuesString = "";
            foreach (DateTime datetime in e.NewDates.OrderBy(f => f))
            {
                ValuesString += ";" + LDateTime.DateToString(datetime, "dd");
            }
            if (ValuesString.Length > 1)
                ValuesString = ValuesString.Substring(1);
            int index = lstPhatVon.IndexOf(objCumLich);
            if (e.Cell.Column.UniqueName == "THANG_1")
                objCumLich.JAN = ValuesString;
            else if (e.Cell.Column.UniqueName == "THANG_2")
                objCumLich.FEB = ValuesString;
            else if (e.Cell.Column.UniqueName == "THANG_3")
                objCumLich.MAR = ValuesString;
            else if (e.Cell.Column.UniqueName == "THANG_4")
                objCumLich.APR = ValuesString;
            else if (e.Cell.Column.UniqueName == "THANG_5")
                objCumLich.MAY = ValuesString;
            else if (e.Cell.Column.UniqueName == "THANG_6")
                objCumLich.JUN = ValuesString;
            else if (e.Cell.Column.UniqueName == "THANG_7")
                objCumLich.JUL = ValuesString;
            else if (e.Cell.Column.UniqueName == "THANG_8")
                objCumLich.AUG = ValuesString;
            else if (e.Cell.Column.UniqueName == "THANG_9")
                objCumLich.SEP = ValuesString;
            else if (e.Cell.Column.UniqueName == "THANG_10")
                objCumLich.OCT = ValuesString;
            else if (e.Cell.Column.UniqueName == "THANG_11")
                objCumLich.NOV = ValuesString;
            else if (e.Cell.Column.UniqueName == "THANG_12")
                objCumLich.DEC = ValuesString;
            lstPhatVon[index] = objCumLich;
        }

        void ucThuControl_SelectedDatesGridViewChanged(object sender, CustomControl.SelectedDatesChangedGridViewEventArgs e)
        {
            CUM_LICH objCumLich = e.Cell.ParentRow.Item as CUM_LICH;
            string ValuesString = "";
            foreach (DateTime datetime in e.NewDates.OrderBy(f => f))
            {
                ValuesString += ";" + LDateTime.DateToString(datetime, "dd");
            }
            if (ValuesString.Length > 1)
                ValuesString = ValuesString.Substring(1);
            int index = lstThuVon.IndexOf(objCumLich);
            if (e.Cell.Column.UniqueName == "THANGTHU_1")
                objCumLich.JAN = ValuesString;
            else if (e.Cell.Column.UniqueName == "THANGTHU_2")
                objCumLich.FEB = ValuesString;
            else if (e.Cell.Column.UniqueName == "THANGTHU_3")
                objCumLich.MAR = ValuesString;
            else if (e.Cell.Column.UniqueName == "THANGTHU_4")
                objCumLich.APR = ValuesString;
            else if (e.Cell.Column.UniqueName == "THANGTHU_5")
                objCumLich.MAY = ValuesString;
            else if (e.Cell.Column.UniqueName == "THANGTHU_6")
                objCumLich.JUN = ValuesString;
            else if (e.Cell.Column.UniqueName == "THANGTHU_7")
                objCumLich.JUL = ValuesString;
            else if (e.Cell.Column.UniqueName == "THANGTHU_8")
                objCumLich.AUG = ValuesString;
            else if (e.Cell.Column.UniqueName == "THANGTHU_9")
                objCumLich.SEP = ValuesString;
            else if (e.Cell.Column.UniqueName == "THANGTHU_10")
                objCumLich.OCT = ValuesString;
            else if (e.Cell.Column.UniqueName == "THANGTHU_11")
                objCumLich.NOV = ValuesString;
            else if (e.Cell.Column.UniqueName == "THANGTHU_12")
                objCumLich.DEC = ValuesString;
            lstThuVon[index] = objCumLich;
        }

        void radDanhSachLichThu_BeginningEdit(object sender, GridViewBeginningEditRoutedEventArgs e)
        {
            if (!LObject.IsNullOrEmpty(e.Cell.Column.UniqueName) & e.Cell.Column.UniqueName.Contains("THANG"))
            {
                ColumnsDateTimeMulti col = e.Cell.Column as ColumnsDateTimeMulti;
                if (col.DisplayDate < ClientInformation.NgayLamViecHienTai.StringToDate(ApplicationConstant.defaultDateTimeFormat))
                    e.Cancel = true;
            }
        }

        void radDanhSachLichPhat_BeginningEdit(object sender, GridViewBeginningEditRoutedEventArgs e)
        {
            if (!LObject.IsNullOrEmpty(e.Cell.Column.UniqueName) & e.Cell.Column.UniqueName.Contains("THANG"))
            {
                ColumnsDateTimeMulti col = e.Cell.Column as ColumnsDateTimeMulti;
                if (col.DisplayDate < ClientInformation.NgayLamViecHienTai.StringToDate(ApplicationConstant.defaultDateTimeFormat))
                    e.Cancel = true;
            }
        }

        void tlbCopyUp_Click(object sender, RoutedEventArgs e)
        {
            //lstPhatVon = Clone<List<CUM_LICH>>(lstThuVon);
            lstPhatVon.ForEach(f => f.MA_LOAI_LICH = "PHAT_VON");
            radDanhSachLichPhat.ItemsSource = lstPhatVon;
            radDanhSachLichPhat.Rebind();
        }

        void tlbCopyDown_Click(object sender, RoutedEventArgs e)
        {
            if (!lstPhatVon.IsNullOrEmpty())
            {
                //lstThuVon = Clone<List<CUM_LICH>>(lstPhatVon);
                lstThuVon.ForEach(f => f.MA_LOAI_LICH = "THU_VON");
                radDanhSachLichThu.ItemsSource = lstThuVon;
                radDanhSachLichThu.Rebind();
            }
        }
        #endregion
        #region Xu ly nghiep vu
        private void LayDuLieu()
        {
            Cursor = Cursors.Wait;
            try
            {
                string idKhuVuc = "0";
                string _namTruoc = "";
                _namDL = LDateTime.DateToString(raddtNamLichThuPhat.Value.GetValueOrDefault(), "yyyy");
                if (chkDuLieuNamTruoc.IsChecked.GetValueOrDefault())
                    _namTruoc = LDateTime.DateToString(raddtNamLichThuPhat.Value.GetValueOrDefault().AddYears(-1), "yyyy");
                if (tvwKhuVuc.SelectedItem == null)
                    tvwKhuVuc.SelectedItem = tvwKhuVuc.Items[0];
                RadTreeViewItem item = tvwKhuVuc.SelectedItem as RadTreeViewItem;
                if (item.Tag.ToString().Substring(0, 3) == "KVU")
                    idKhuVuc = item.Tag.ToString().Substring(3);
                SetDataControl();
                DataTable dt = null;
                LDatatable.MakeParameterTable(ref dt);
                LDatatable.AddParameter(ref dt, "@INP_ID_KVUC", "string", idKhuVuc);
                LDatatable.AddParameter(ref dt, "@INP_NAM_DL", "string", _namDL);
                LDatatable.AddParameter(ref dt, "@INP_NAM_TRUOC", "string", _namTruoc);
                DataSet ds = new TinDungProcess().GetThongTinDatLichThuPhatVon(dt);
                if (!LObject.IsNullOrEmpty(ds) && ds.Tables.Count > 0)
                {
                    dt = ds.Tables["CHI_TIET"];
                    if (!LObject.IsNullOrEmpty(dt) && dt.Rows.Count > 0)
                    {
                        List<CUM_LICH> lstCumFull = new List<CUM_LICH>();
                        foreach (DataRow dr in dt.Rows)
                        {
                            CUM_LICH objCum = new CUM_LICH();
                            objCum.ID = Convert.ToInt32(dr["ID"]);
                            objCum.ID_CUM = Convert.ToInt32(dr["ID_CUM"]);
                            objCum.ID_KVUC = Convert.ToInt32(dr["ID_KVUC"]);
                            objCum.MA_CUM = dr["MA_CUM"].ToString();
                            objCum.TEN_CUM = dr["TEN_CUM"].ToString();
                            objCum.MA_LOAI_LICH = dr["MA_LOAI_LICH"].ToString();
                            objCum.NAM = dr["NAM"].ToString();
                            objCum.JAN = dr["JAN"].ToString();
                            objCum.FEB = dr["FEB"].ToString();
                            objCum.MAR = dr["MAR"].ToString();
                            objCum.APR = dr["APR"].ToString();
                            objCum.MAY = dr["MAY"].ToString();
                            objCum.JUN = dr["JUN"].ToString();
                            objCum.JUL = dr["JUL"].ToString();
                            objCum.AUG = dr["AUG"].ToString();
                            objCum.SEP = dr["SEP"].ToString();
                            objCum.OCT = dr["OCT"].ToString();
                            objCum.NOV = dr["NOV"].ToString();
                            objCum.DEC = dr["DEC"].ToString();
                            objCum.TTHAI_BGHI = dr["TTHAI_BGHI"].ToString();
                            objCum.TTHAI_NVU = dr["TTHAI_NVU"].ToString();
                            objCum.MA_DVI_QLY = dr["MA_DVI_QLY"].ToString();
                            objCum.MA_DVI_TAO = dr["MA_DVI_TAO"].ToString();
                            objCum.NGAY_NHAP = dr["NGAY_NHAP"].ToString();
                            objCum.NGUOI_NHAP = dr["NGUOI_NHAP"].ToString();
                            objCum.NGAY_CNHAT = dr["NGAY_CNHAT"].ToString();
                            objCum.NGUOI_CNHAT = dr["NGUOI_CNHAT"].ToString();
                            lstCumFull.Add(objCum);
                        }
                        lstPhatVon = lstCumFull.Where(f => f.MA_LOAI_LICH == "PHAT_VON").ToList();
                        lstThuVon = lstCumFull.Where(f => f.MA_LOAI_LICH == "THU_VON").ToList();
                    }
                    else
                    {
                        lstPhatVon = new List<CUM_LICH>();
                        lstThuVon = new List<CUM_LICH>();
                    }
                    radDanhSachLichPhat.ItemsSource = lstPhatVon;
                    radDanhSachLichThu.ItemsSource = lstThuVon;
                    radDanhSachLichPhat.Rebind();
                    radDanhSachLichThu.Rebind();
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                Cursor = Cursors.Arrow;
            }
        }
        private void SetDataControl()
        {
            ucPhatDateJan.DisplayDate = LDateTime.StringToDate(_namDL + "0101", ApplicationConstant.defaultDateTimeFormat);
            ucPhatDateFeb.DisplayDate = LDateTime.StringToDate(_namDL + "0201", ApplicationConstant.defaultDateTimeFormat);
            ucPhatDateMar.DisplayDate = LDateTime.StringToDate(_namDL + "0301", ApplicationConstant.defaultDateTimeFormat);
            ucPhatDateApr.DisplayDate = LDateTime.StringToDate(_namDL + "0401", ApplicationConstant.defaultDateTimeFormat);
            ucPhatDateMay.DisplayDate = LDateTime.StringToDate(_namDL + "0501", ApplicationConstant.defaultDateTimeFormat);
            ucPhatDateJun.DisplayDate = LDateTime.StringToDate(_namDL + "0601", ApplicationConstant.defaultDateTimeFormat);
            ucPhatDateJul.DisplayDate = LDateTime.StringToDate(_namDL + "0701", ApplicationConstant.defaultDateTimeFormat);
            ucPhatDateAug.DisplayDate = LDateTime.StringToDate(_namDL + "0801", ApplicationConstant.defaultDateTimeFormat);
            ucPhatDateSep.DisplayDate = LDateTime.StringToDate(_namDL + "0901", ApplicationConstant.defaultDateTimeFormat);
            ucPhatDateOct.DisplayDate = LDateTime.StringToDate(_namDL + "1001", ApplicationConstant.defaultDateTimeFormat);
            ucPhatDateNov.DisplayDate = LDateTime.StringToDate(_namDL + "1101", ApplicationConstant.defaultDateTimeFormat);
            ucPhatDateDec.DisplayDate = LDateTime.StringToDate(_namDL + "1201", ApplicationConstant.defaultDateTimeFormat);
            ucThuDateJan.DisplayDate = LDateTime.StringToDate(_namDL + "0101", ApplicationConstant.defaultDateTimeFormat);
            ucThuDateFeb.DisplayDate = LDateTime.StringToDate(_namDL + "0201", ApplicationConstant.defaultDateTimeFormat);
            ucThuDateMar.DisplayDate = LDateTime.StringToDate(_namDL + "0301", ApplicationConstant.defaultDateTimeFormat);
            ucThuDateApr.DisplayDate = LDateTime.StringToDate(_namDL + "0401", ApplicationConstant.defaultDateTimeFormat);
            ucThuDateMay.DisplayDate = LDateTime.StringToDate(_namDL + "0501", ApplicationConstant.defaultDateTimeFormat);
            ucThuDateJun.DisplayDate = LDateTime.StringToDate(_namDL + "0601", ApplicationConstant.defaultDateTimeFormat);
            ucThuDateJul.DisplayDate = LDateTime.StringToDate(_namDL + "0701", ApplicationConstant.defaultDateTimeFormat);
            ucThuDateAug.DisplayDate = LDateTime.StringToDate(_namDL + "0801", ApplicationConstant.defaultDateTimeFormat);
            ucThuDateSep.DisplayDate = LDateTime.StringToDate(_namDL + "0901", ApplicationConstant.defaultDateTimeFormat);
            ucThuDateOct.DisplayDate = LDateTime.StringToDate(_namDL + "1001", ApplicationConstant.defaultDateTimeFormat);
            ucThuDateNov.DisplayDate = LDateTime.StringToDate(_namDL + "1101", ApplicationConstant.defaultDateTimeFormat);
            ucThuDateDec.DisplayDate = LDateTime.StringToDate(_namDL + "1201", ApplicationConstant.defaultDateTimeFormat);
        }
        private bool GetDataForm()
        {
            bool kq = true;
            try
            {
                if (LObject.IsNullOrEmpty(obj)) obj = new LICH_THU_PHAT_VON();
                List<CUM_LICH> lst = new List<CUM_LICH>();
                obj.NAM_DL = _namDL;
                obj.NGAY_LAP = ClientInformation.NgayLamViecHienTai;
                lst.AddRange(lstPhatVon);
                lst.AddRange(lstThuVon);
                lst.ForEach(f =>
                {
                    f.NAM = _namDL;
                    f.MA_DVI_QLY = ClientInformation.MaDonVi; f.MA_DVI_TAO = ClientInformation.MaDonViGiaoDich; f.NGAY_CNHAT = ClientInformation.NgayLamViecHienTai;
                    f.NGAY_NHAP = ClientInformation.NgayLamViecHienTai; f.NGUOI_CNHAT = ClientInformation.TenDangNhap; f.NGUOI_NHAP = ClientInformation.TenDangNhap;
                    f.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri(); f.TTHAI_NVU = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();
                });
                obj.DSACH_LICH_THU_PHAT_VON = lst.ToArray();
            }
            catch (Exception ex)
            {
                kq = false;
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            return kq;
        }
        private bool Validation()
        {
            return true;
        }

        private void OnSave()
        {
            Cursor = Cursors.Wait;
            List<ClientResponseDetail> lstResponseDetail = new List<ClientResponseDetail>();
            int iret = 0;
            try
            {
                if (Validation())
                {
                    if (GetDataForm())
                    {
                        iret = new TinDungProcess().LichThuPhatVon(DatabaseConstant.Action.THEM, ref obj, ref lstResponseDetail);
                        AfterSave(iret, lstResponseDetail);
                    }
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
            }
            finally
            {
                lstResponseDetail = null;
                Cursor = Cursors.Arrow;
            }
        }

        private void AfterSave(int iret, List<ClientResponseDetail> lstResponseDetail)
        {
            if (iret > 0)
                LMessage.ShowMessage("M.DungChung.LuuDuLieuThanhCong", LMessage.MessageBoxType.Information);
            else
                CommonFunction.ThongBaoKetQua(lstResponseDetail);
            LayDuLieu();
        }
        #endregion

    }

}
