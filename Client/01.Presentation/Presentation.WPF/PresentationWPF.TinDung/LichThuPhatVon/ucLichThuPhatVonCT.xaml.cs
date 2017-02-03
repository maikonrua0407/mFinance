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

namespace PresentationWPF.TinDung.LichThuPhatVon
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
        ListCheckBoxCombo lstSourceCum = new ListCheckBoxCombo();
        #endregion

        #region Khoi tao
        public ucLichThuPhatVonCT()
        {
            InitializeComponent();
            HeThong htHeThong = new HeThong();
            htHeThong.DuyetQuyenTinhNangToolbar("/PresentationWPF.TinDung;component/LichThuPhatVon/ucLichThuPhatVonCT.xaml", ref Toolbar, ref mnuMain);
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
            cmbDonViTinhTGian.SelectionChanged += cmbDonViTinhTGian_SelectionChanged;
            telSoLanThuPhat.ValueChanged += telSoLanThuPhat_ValueChanged;
            tlbCopyDown.Click += new RoutedEventHandler(tlbCopyDown_Click);
            tlbCopyUp.Click += new RoutedEventHandler(tlbCopyUp_Click);
            raddtNamLichThuPhat.ValueChanged += raddtNamLichThuPhat_ValueChanged;
            tvwKhuVuc.SelectionChanged += tvwKhuVuc_SelectionChanged;
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
            LayDuLieu();
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
            else if (strTinhNang.Equals("Cal"))
            {
                tlbCal.Focus();
                OnCalculate();
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
            else if (strTinhNang.Equals("Cal"))
            {
                tlbCal.Focus();
                OnCalculate();
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
        /// Trợ giúp
        /// </summary>
        private void onHelp()
        {
            CustomControl.CommonFunction.ShowHelp(this);
        }

        private void cmbDanhSachCum_DropDownClosed(object sender, EventArgs e)
        {
            lstSourceCum = cmbDanhSachCum.ItemsSource as ListCheckBoxCombo;
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
                if (col.DisplayDate.GetLastDateOfMonth() < ClientInformation.NgayLamViecHienTai.StringToDate(ApplicationConstant.defaultDateTimeFormat).GetLastDateOfMonth())
                    e.Cancel = true;
            }
        }

        void radDanhSachLichPhat_BeginningEdit(object sender, GridViewBeginningEditRoutedEventArgs e)
        {
            if (!LObject.IsNullOrEmpty(e.Cell.Column.UniqueName) & e.Cell.Column.UniqueName.Contains("THANG"))
            {
                ColumnsDateTimeMulti col = e.Cell.Column as ColumnsDateTimeMulti;
                if (col.DisplayDate.GetLastDateOfMonth() < ClientInformation.NgayLamViecHienTai.StringToDate(ApplicationConstant.defaultDateTimeFormat).GetLastDateOfMonth())
                    e.Cancel = true;
            }
        }

        void tlbCopyUp_Click(object sender, RoutedEventArgs e)
        {
            return;

            lstPhatVon = new List<CUM_LICH>();
            CUM_LICH obj = null;
            foreach (CUM_LICH objThuVon in lstThuVon)
            {
                obj = new CUM_LICH();
                obj.APR = objThuVon.APR;
                obj.AUG = objThuVon.AUG;
                obj.DEC = objThuVon.DEC;
                obj.FEB = objThuVon.FEB;
                obj.ID_CUM = objThuVon.ID_CUM;
                obj.ID_KVUC = objThuVon.ID_KVUC;
                obj.JAN = objThuVon.JAN;
                obj.JUL = objThuVon.JUL;
                obj.JUN = objThuVon.JUN;
                obj.MA_CUM = objThuVon.MA_CUM;
                obj.MA_DVI_QLY = objThuVon.MA_DVI_QLY;
                obj.MA_DVI_TAO = objThuVon.MA_DVI_TAO;
                obj.MA_LOAI_LICH = "PHAT_VON";
                obj.MAR = objThuVon.MAR;
                obj.MAY = objThuVon.MAY;
                obj.NAM = objThuVon.NAM;
                obj.NGAY_CNHAT = objThuVon.NGAY_CNHAT;
                obj.NGAY_NHAP = objThuVon.NGAY_NHAP;
                obj.NGUOI_CNHAT = objThuVon.NGUOI_CNHAT;
                obj.NGUOI_NHAP = objThuVon.NGUOI_NHAP;
                obj.NOV = objThuVon.NOV;
                obj.OCT = objThuVon.OCT;
                obj.SEP = objThuVon.SEP;
                obj.TEN_CUM = objThuVon.TEN_CUM;
                obj.TTHAI_BGHI = objThuVon.TTHAI_BGHI;
                obj.TTHAI_NVU = objThuVon.TTHAI_NVU;
                lstPhatVon.Add(obj);
            }
            //lstPhatVon = Clone<List<CUM_LICH>>(lstThuVon);
            //lstPhatVon.ForEach(f => f.MA_LOAI_LICH = "PHAT_VON");
            radDanhSachLichPhat.ItemsSource = lstPhatVon;
            radDanhSachLichPhat.Rebind();
        }

        void tlbCopyDown_Click(object sender, RoutedEventArgs e)
        {
            return;

            if (!lstPhatVon.IsNullOrEmpty())
            {
                lstThuVon = new List<CUM_LICH>();
                CUM_LICH obj = null;
                foreach (CUM_LICH objPhatVon in lstPhatVon)
                {
                    obj = new CUM_LICH();
                    obj.APR = objPhatVon.APR;
                    obj.AUG = objPhatVon.AUG;
                    obj.DEC = objPhatVon.DEC;
                    obj.FEB = objPhatVon.FEB;
                    obj.ID_CUM = objPhatVon.ID_CUM;
                    obj.ID_KVUC = objPhatVon.ID_KVUC;
                    obj.JAN = objPhatVon.JAN;
                    obj.JUL = objPhatVon.JUL;
                    obj.JUN = objPhatVon.JUN;
                    obj.MA_CUM = objPhatVon.MA_CUM;
                    obj.MA_DVI_QLY = objPhatVon.MA_DVI_QLY;
                    obj.MA_DVI_TAO = objPhatVon.MA_DVI_TAO;
                    obj.MA_LOAI_LICH = "THU_VON";
                    obj.MAR = objPhatVon.MAR;
                    obj.MAY = objPhatVon.MAY;
                    obj.NAM = objPhatVon.NAM;
                    obj.NGAY_CNHAT = objPhatVon.NGAY_CNHAT;
                    obj.NGAY_NHAP = objPhatVon.NGAY_NHAP;
                    obj.NGUOI_CNHAT = objPhatVon.NGUOI_CNHAT;
                    obj.NGUOI_NHAP = objPhatVon.NGUOI_NHAP;
                    obj.NOV = objPhatVon.NOV;
                    obj.OCT = objPhatVon.OCT;
                    obj.SEP = objPhatVon.SEP;
                    obj.TEN_CUM = objPhatVon.TEN_CUM;
                    obj.TTHAI_BGHI = objPhatVon.TTHAI_BGHI;
                    obj.TTHAI_NVU = objPhatVon.TTHAI_NVU;
                    lstThuVon.Add(obj);
                }
                //lstThuVon = Clone<List<CUM_LICH>>(lstPhatVon);
                //lstThuVon.ForEach(f => f.MA_LOAI_LICH = "THU_VON");
                radDanhSachLichThu.ItemsSource = lstThuVon;
                radDanhSachLichThu.Rebind();
            }
        }

        private void raddtNamLichThuPhat_ValueChanged(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            lstPhatVon = new List<CUM_LICH>();
            lstThuVon = new List<CUM_LICH>();
            lstSourceCum = new ListCheckBoxCombo();
            radDanhSachLichPhat.ItemsSource = lstPhatVon;
            radDanhSachLichThu.ItemsSource = lstThuVon;
            radDanhSachLichPhat.Rebind();
            radDanhSachLichThu.Rebind();
            new AutoComboBoxListCheckes().GenAutoComboBox(ref lstSourceCum, ref cmbDanhSachCum, null);
        }

        private void tvwKhuVuc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lstPhatVon = new List<CUM_LICH>();
            lstThuVon = new List<CUM_LICH>();
            lstSourceCum = new ListCheckBoxCombo();
            radDanhSachLichPhat.ItemsSource = lstPhatVon;
            radDanhSachLichThu.ItemsSource = lstThuVon;
            radDanhSachLichPhat.Rebind();
            radDanhSachLichThu.Rebind();
            new AutoComboBoxListCheckes().GenAutoComboBox(ref lstSourceCum, ref cmbDanhSachCum, null);
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
                lstSourceCum = new ListCheckBoxCombo();
                LDatatable.MakeParameterTable(ref dt);
                LDatatable.AddParameter(ref dt, "@INP_ID_KVUC", "string", idKhuVuc);
                LDatatable.AddParameter(ref dt, "@INP_NAM_DL", "string", _namDL);
                LDatatable.AddParameter(ref dt, "@INP_NAM_TRUOC", "string", _namTruoc);
                DataSet ds = new TinDungProcess().GetThongTinDatLichThuPhatVon(dt);
                AutoCompleteCheckBox objComBoLich = null;
                objComBoLich = new AutoCompleteCheckBox();
                objComBoLich.ValueMember = new string[2] { "All", "0" };
                objComBoLich.DislayMember = LLanguage.SearchResourceByKey("U.DungChung.TatCa");
                objComBoLich.CheckedMember = false;
                lstSourceCum.Add(objComBoLich);
                if (!LObject.IsNullOrEmpty(ds) && ds.Tables.Count > 0)
                {
                    dt = ds.Tables["CHI_TIET"];
                    if (!LObject.IsNullOrEmpty(dt) && dt.Rows.Count > 0)
                    {
                        List<CUM_LICH> lstCumFull = new List<CUM_LICH>();
                        foreach (DataRow dr in dt.Rows)
                        {
                            objComBoLich = new AutoCompleteCheckBox();
                            CUM_LICH objCum = new CUM_LICH();
                            string[] value = new string[4];
                            objCum.ID = Convert.ToInt32(dr["ID"]);
                            objCum.ID_CUM = Convert.ToInt32(dr["ID_CUM"]);
                            value[1] = Convert.ToString(dr["ID_CUM"]);
                            objCum.ID_KVUC = Convert.ToInt32(dr["ID_KVUC"]);
                            value[2] = Convert.ToString(dr["ID_KVUC"]);
                            objCum.MA_CUM = dr["MA_CUM"].ToString();
                            value[0] = Convert.ToString(dr["MA_CUM"]);
                            objCum.TEN_CUM = dr["TEN_CUM"].ToString();
                            value[3] = Convert.ToString(dr["MA_CUM"]);
                            objComBoLich.DislayMember = dr["TEN_CUM"].ToString();
                            objComBoLich.ValueMember = value;
                            objComBoLich.CheckedMember = false;
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
                            if (dr["MA_LOAI_LICH"].ToString().Equals("PHAT_VON"))
                                lstSourceCum.Add(objComBoLich);
                        }
                        lstPhatVon = lstCumFull.Where(f => f.MA_LOAI_LICH == "PHAT_VON").ToList();
                        lstThuVon = lstCumFull.Where(f => f.MA_LOAI_LICH == "THU_VON").ToList();
                    }
                    else
                    {
                        lstPhatVon = new List<CUM_LICH>();
                        lstThuVon = new List<CUM_LICH>();
                    }
                    new AutoComboBoxListCheckes().GenAutoComboBox(ref lstSourceCum, ref cmbDanhSachCum, null, null);
                    telSoLanThuPhat.Value = 1;
                    radDanhSachLichPhat.ItemsSource = lstPhatVon;
                    radDanhSachLichThu.ItemsSource = lstThuVon;
                    radDanhSachLichPhat.Rebind();
                    radDanhSachLichThu.Rebind();
                    LMessage.ShowMessage("M.DungChung.Result.ThanhCong", LMessage.MessageBoxType.Information);
                }
            }
            catch (Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiKhongXacDinh", LMessage.MessageBoxType.Error);
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
            {
                LayDuLieu();
            }
            else
                CommonFunction.ThongBaoKetQua(lstResponseDetail);
        }

        private bool LayDuLieuTinhToanLich()
        {
            bool kq = true;
            try
            {
                if(lstSourceCum.Where(f=>f.CheckedMember == true).IsNullOrEmpty() || lstSourceCum.Where(f=>f.CheckedMember==true).Count()<=0)
                {
                    LMessage.ShowMessage("M.DungChung.ChuaChonBanGhi", LMessage.MessageBoxType.Warning);
                    return false;
                }
                if (LObject.IsNullOrEmpty(obj)) obj = new LICH_THU_PHAT_VON();
                List<CUM_LICH> lst = new List<CUM_LICH>();
                obj.NAM_DL = _namDL;
                obj.NGAY_LAP = ClientInformation.NgayLamViecHienTai;
                obj.TAN_SUAT = Convert.ToInt32(telSoLanThuPhat.Value.GetValueOrDefault());
                obj.DVI_TINH_TOAN = cmbDonViTinhTGian.SelectedValue.ToString();
                obj.TAN_SUAT_DVI_TINH = cmbDonViTinhTGian.SelectedValue.ToString();
                obj.TGIAN_DEN = 12;
                obj.TGIAN_TU = 1;
                if(_namDL == ClientInformation.NgayLamViecHienTai.StringToDate(ApplicationConstant.defaultDateTimeFormat).ToString("yyyy"))
                {
                    obj.TGIAN_TU = ClientInformation.NgayLamViecHienTai.StringToDate(ApplicationConstant.defaultDateTimeFormat).GetMonth();
                }
                if (cmbLoaiLich.SelectedValue.ToString().Equals("ALL"))
                {
                    List<string> lstMaCum = lstSourceCum.Where(f => f.CheckedMember == true).Select(f => f.ValueMember[0]).ToList();
                    lst.AddRange(lstPhatVon.Where(f => lstMaCum.Contains(f.MA_CUM)));
                    lst.AddRange(lstThuVon.Where(f => lstMaCum.Contains(f.MA_CUM)));
                }
                else if (cmbLoaiLich.SelectedValue.ToString().Equals("THU_VON"))
                {
                    List<string> lstMaCum = lstSourceCum.Where(f => f.CheckedMember == true).Select(f => f.ValueMember[0]).ToList();
                    //lst.AddRange(lstPhatVon.Where(f => lstMaCum.Contains(f.MA_CUM)));
                    lst.AddRange(lstThuVon.Where(f => lstMaCum.Contains(f.MA_CUM)));
                }
                else if (cmbLoaiLich.SelectedValue.ToString().Equals("PHAT_VON"))
                {
                    List<string> lstMaCum = lstSourceCum.Where(f => f.CheckedMember == true).Select(f => f.ValueMember[0]).ToList();
                    lst.AddRange(lstPhatVon.Where(f => lstMaCum.Contains(f.MA_CUM)));
                    //lst.AddRange(lstThuVon.Where(f => lstMaCum.Contains(f.MA_CUM)));
                }
                obj.DSACH_LICH_THU_PHAT_VON = lst.ToArray();
                string giaTriDinhDang = "";
                List<THONG_TIN_TINH_LICH> lstThongTin = radDanhSachNgay.ItemsSource as List<THONG_TIN_TINH_LICH>;
                foreach(THONG_TIN_TINH_LICH objThongTin in lstThongTin)
                {
                    if(obj.TAN_SUAT_DVI_TINH.Equals(BusinessConstant.TAN_SUAT.THANG.layGiaTri()))
                    {
                        giaTriDinhDang += "#" + objThongTin.NGAY.PadLeft(2,'0');
                    }
                    else
                    {
                        giaTriDinhDang += "#" + objThongTin.THU + "." + objThongTin.TUAN.ToString();
                    }
                }
                giaTriDinhDang = giaTriDinhDang.Substring(1);
                obj.GIA_TRI_DDANG = giaTriDinhDang;
            }
            catch (Exception ex)
            {
                kq = false;
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
            }
            return kq;
            
        }

        private void OnCalculate()
        {
            Cursor = Cursors.Wait;
            List<ClientResponseDetail> lstResponseDetail = new List<ClientResponseDetail>();
            int iret = 0;
            
            try
            {
                if (LayDuLieuTinhToanLich())
                {
                    iret = new TinDungProcess().LichThuPhatVon(DatabaseConstant.Action.TINH_TOAN, ref obj, ref lstResponseDetail);
                    AfterCalculate(iret, lstResponseDetail);
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

        private void AfterCalculate(int iret, List<ClientResponseDetail> lstResponseDetail)
        {
            if (iret > 0)
            {
                LMessage.ShowMessage("M.DungChung.TinhToanThanhCong", LMessage.MessageBoxType.Information);
                foreach(CUM_LICH objCumLich in obj.DSACH_LICH_THU_PHAT_VON)
                {
                    int index = -1;
                    if (objCumLich.MA_LOAI_LICH == "THU_VON")
                    {
                        index = lstThuVon.IndexOf(lstThuVon.FirstOrDefault(f => f.ID_CUM == objCumLich.ID_CUM));
                        lstThuVon[index] = objCumLich;
                    }
                    else if(objCumLich.MA_LOAI_LICH == "PHAT_VON")
                    {
                        index = lstPhatVon.IndexOf(lstPhatVon.FirstOrDefault(f => f.ID_CUM == objCumLich.ID_CUM));
                        lstPhatVon[index] = objCumLich;
                    }
                }
                radDanhSachLichPhat.Rebind();
                radDanhSachLichThu.Rebind();
            }
            else
                CommonFunction.ThongBaoKetQua(lstResponseDetail);
            
        }

        private void telSoLanThuPhat_ValueChanged(object sender, RadRangeBaseValueChangedEventArgs e)
        {
            List<THONG_TIN_TINH_LICH> lstThongTin = new List<THONG_TIN_TINH_LICH>();
            THONG_TIN_TINH_LICH objThongTin = null;
            for (int i = 0; i < telSoLanThuPhat.Value.GetValueOrDefault(); i++)
            {
                objThongTin = new THONG_TIN_TINH_LICH();
                objThongTin.NGAY = (i + 1).ToString().PadLeft(2, '0');
                objThongTin.THU = "MON";
                objThongTin.TUAN = i + 1;
                lstThongTin.Add(objThongTin);
            }
            radDanhSachNgay.ItemsSource = lstThongTin;
            radDanhSachNgay.Rebind();
        }

        private void cmbDonViTinhTGian_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbDonViTinhTGian.SelectedValue.ToString().Equals(BusinessConstant.TAN_SUAT.THANG.layGiaTri()))
            {
                radDanhSachNgay.Columns["NGAY"].IsVisible = true;
                radDanhSachNgay.Columns["THU"].IsVisible = false;
                radDanhSachNgay.Columns["TUAN"].IsVisible = false;
            }
            else
            {
                radDanhSachNgay.Columns["NGAY"].IsVisible = false;
                radDanhSachNgay.Columns["THU"].IsVisible = true;
                radDanhSachNgay.Columns["TUAN"].IsVisible = true;
            }
        }
        #endregion

    }

    public class THONG_TIN_TINH_LICH
    {
        string _nGAY;
        string _tHU;
        int _tUAN;

        public string NGAY
        {
            get
            {
                return _nGAY;
            }

            set
            {
                _nGAY = value;
            }
        }

        public string THU
        {
            get
            {
                return _tHU;
            }

            set
            {
                _tHU = value;
            }
        }

        public int TUAN
        {
            get
            {
                return _tUAN;
            }

            set
            {
                _tUAN = value;
            }
        }
    }
}
