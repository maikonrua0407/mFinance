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
using Utilities.Common;
using Microsoft.Windows.Controls.Ribbon;
using Presentation.Process;
using Presentation.Process.TinDungServiceRef;
using Telerik.Windows.Controls;
using PresentationWPF.CustomControl;
using Presentation.Process.Common;

namespace PresentationWPF.TinDungTT.PopupNghiepVu
{
    /// <summary>
    /// Interaction logic for ucPopopKeHoachCT.xaml
    /// </summary>
    public partial class ucPopopKeHoachCT : UserControl
    {
        #region Khai bao
        public static RoutedCommand SaveCommand = new RoutedCommand();
        public static RoutedCommand CloseCommand = new RoutedCommand();
        private List<AutoCompleteEntry> lstNgayDauKy = new List<AutoCompleteEntry>();
        private TDVM_KHE_UOC _TDVMKHEUOC = new TDVM_KHE_UOC();
        TDVM_KHE_UOC_DSACH KUOCVMDS = new TDVM_KHE_UOC_DSACH();
        List<TDVM_KHE_UOC> objDanhSach = new List<TDVM_KHE_UOC>();
        public delegate void LayGiaTriLapLich(List<TDVM_KHE_UOC> lst);
        public LayGiaTriLapLich LayGiaTri;
        int index = -1;
        #endregion

        #region Khoi tao
        public ucPopopKeHoachCT()
        {
            InitializeComponent();
            InitEventHanler();
        }

        void InitEventHanler()
        {
            radDanhSachKheUoc.SelectionChanged += new EventHandler<SelectionChangeEventArgs>(radDanhSachKheUoc_SelectionChanged);
            tlbClose.Click += new RoutedEventHandler(tlbClose_Click);
            tlbSave.Click += new RoutedEventHandler(tlbSave_Click);
            tlbCapNhatLich.Click += new RoutedEventHandler(tlbCapNhatLich_Click);
            raddgrThongTinLapLich.BeginningEdit +=new EventHandler<GridViewBeginningEditRoutedEventArgs>(raddgrThongTinLapLich_BeginningEdit);
            this.Unloaded += new RoutedEventHandler(ucPopopKeHoachCT_Unloaded);
        }

        void ucPopopKeHoachCT_Unloaded(object sender, RoutedEventArgs e)
        {
            LayGiaTri(objDanhSach);
        }

        public ucPopopKeHoachCT(ref List<TDVM_KHE_UOC> _objDanhSach, TDVM_KHE_UOC objKheUoc,TDVM_KHE_UOC_DSACH objKUOCVMDS)
            : this()
        {
            System.Linq.Expressions.Expression<Func<TDVM_KHE_UOC, decimal?>> expressionKHGoc = kh => (kh.DSACH_KHOACHVM_CTIET.Sum(ds => ds.KH_TRA_GOC.GetValueOrDefault(0)));
            GridViewExpressionColumn column = this.radDanhSachKheUoc.Columns["TongTienGoc"] as GridViewExpressionColumn;
            column.Expression = expressionKHGoc;
            System.Linq.Expressions.Expression<Func<TDVM_KHE_UOC, decimal?>> expressionKHLai = kh => (kh.DSACH_KHOACHVM_CTIET.Sum(ds => ds.KH_TRA_LAI.GetValueOrDefault(0)));
            column = this.radDanhSachKheUoc.Columns["TongTienLai"] as GridViewExpressionColumn;
            column.Expression = expressionKHLai;
            System.Linq.Expressions.Expression<Func<TD_KHOACHVM_CT, decimal?>> expressionKH = kh => (kh.KH_TRA_GOC.IsNullOrEmpty() && kh.KH_TRA_LAI.IsNullOrEmpty() && kh.KH_TRA_PHI.IsNullOrEmpty() ? null : (decimal?)kh.KH_TRA_GOC.GetValueOrDefault(0) + kh.KH_TRA_LAI.GetValueOrDefault(0) + kh.KH_TRA_PHI.GetValueOrDefault(0));
            column = this.raddgrLichTraNo.Columns["Cong"] as GridViewExpressionColumn;
            column.Expression = expressionKH;
            column = this.raddgrThucTraNo.Columns["CongKH"] as GridViewExpressionColumn;
            column.Expression = expressionKH;
            System.Linq.Expressions.Expression<Func<TD_KHOACHVM_CT, decimal?>> expressionTTTT = kh => (kh.TT_TRA_GOC.IsNullOrEmpty() && kh.TT_TRA_LAI.IsNullOrEmpty() && kh.TT_TRA_PHI.IsNullOrEmpty() ? null : (decimal?)kh.TT_TRA_GOC.GetValueOrDefault(0) + kh.TT_TRA_LAI.GetValueOrDefault(0) + kh.TT_TRA_PHI.GetValueOrDefault(0));
            column = this.raddgrThucTraNo.Columns["CongTT"] as GridViewExpressionColumn;
            column.Expression = expressionTTTT;
            radDanhSachKheUoc.ItemsSource = null;
            objDanhSach = _objDanhSach;
            radDanhSachKheUoc.ItemsSource = objDanhSach;
            KUOCVMDS = objKUOCVMDS;
            if (!LObject.IsNullOrEmpty(objKheUoc))
            {
                _TDVMKHEUOC = objKheUoc;
                raddgrThongTinLapLich.ItemsSource = _TDVMKHEUOC.DSACH_KHOACHVM;
                raddgrLichTraNo.ItemsSource = _TDVMKHEUOC.DSACH_KHOACHVM_CTIET;
                raddgrThucTraNo.ItemsSource = _TDVMKHEUOC.DSACH_KHOACHVM_CTIET;
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
                    if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRINH_DUYET)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.S, ModifierKeys.Control);
                        key = new KeyBinding(SaveCommand, keyg);
                    }
                    if (key != null)
                    {
                        InputBindings.Add(key);
                    }
                }
            }
        }

        private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //onSave();
        }

        private void CloseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void CloseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //onClose();
        }

        private void tlbHotKey_Click(object sender, RoutedEventArgs e)
        {
            RibbonButton tlb = (RibbonButton)sender;
            string strTinhNang = tlb.Name.Substring(3, tlb.Name.Length - 3);
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU)))
            {
                //onSave();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DONG)))
            {
                //onClose();
            }
        }

        private void btnShortcutKey_Click(object sender, RoutedEventArgs e)
        {
            string strTinhNang = ((MenuItem)sender).Name.Substring(3, ((MenuItem)sender).Name.Length - 3);
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRINH_DUYET)))
            {
                //onSave();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DONG)))
            {
                //onClose();
            }
        }

        #endregion

        #region Xu ly giao dien
        /// <summary>
        /// Sự kiện nhấn key trên form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            PresentationWPF.CustomControl.CommonFunction.CloseUserControl(e, this);
            PresentationWPF.CustomControl.CommonFunction.SelectNextControl(e);
        }

        /// <summary>
        /// Sự kiện load 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiLoadDuLieu", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void tlbAddLich_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void tlbDeleteLich_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void tlbDelTTin_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void tlbAddTTin_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void tlbLapKeHoach_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void raddgrThongTinLapLich_CellValidating(object sender, GridViewCellValidatingEventArgs e)
        {
            if (e.Cell.Column.UniqueName == "LOAI_HINH_LAP_KE_HOACH")
            {

            }
            else if (e.Cell.Column.UniqueName == "HINH_THUC_THANH_TOAN")
            {

            }
            else if (e.Cell.Column.UniqueName == "NGAY_BDAU")
            {

            }
            else if (e.Cell.Column.UniqueName == "SO_KY")
            {

            }
            else if (e.Cell.Column.UniqueName == "TAN_SUAT")
            {

            }
        }

        void tlbCapNhatLich_Click(object sender, RoutedEventArgs e)
        {
            _TDVMKHEUOC = radDanhSachKheUoc.SelectedItem as TDVM_KHE_UOC;
            index = objDanhSach.IndexOf(_TDVMKHEUOC);
            if (index < 0)
                return;
            List<TDVM_KHE_UOC> lstTDVM_KHE_UOC = new List<TDVM_KHE_UOC>();
            TD_KHOACHVM[] lstDieuKien = raddgrThongTinLapLich.ItemsSource as TD_KHOACHVM[];
            TD_KHOACHVM_CT[] lstKeHoach = null;
            _TDVMKHEUOC.DSACH_KHOACHVM = lstDieuKien;
            _TDVMKHEUOC.DSACH_KHOACHVM_CTIET = lstKeHoach;
            lstTDVM_KHE_UOC.Add(_TDVMKHEUOC);
            KUOCVMDS.DSACH_KHE_UOC = lstTDVM_KHE_UOC.ToArray();
            List<ClientResponseDetail> lstresponseDetail = new List<ClientResponseDetail>();
            int iret = new TinDungProcess().LapKheUocDanhSach_01(DatabaseConstant.Action.TINH_TOAN_LICH_TRA_NO, ref KUOCVMDS, ref lstresponseDetail);
            if (iret > 0)
            {
                lstTDVM_KHE_UOC = KUOCVMDS.DSACH_KHE_UOC.ToList();
                _TDVMKHEUOC = lstTDVM_KHE_UOC.FirstOrDefault();
                objDanhSach[index] = _TDVMKHEUOC;
                radDanhSachKheUoc.ItemsSource = objDanhSach;
                radDanhSachKheUoc.Rebind();
                radDanhSachKheUoc.SelectedItem = _TDVMKHEUOC;
            }
            else
            {
                CommonFunction.ThongBaoKetQua(lstresponseDetail);
            }
        }

        private void raddgrThongTinLapLich_BeginningEdit(object sender, GridViewBeginningEditRoutedEventArgs e)
        {
            if (!e.Cell.Column.UniqueName.IsNullOrEmptyOrSpace())
            {
                if (e.Cell.Column.UniqueName.Equals("NGAY_BDAU"))
                {
                    lstNgayDauKy = new List<AutoCompleteEntry>();
                    foreach (string str in _TDVMKHEUOC.DSACH_NGAY_DKY)
                    {
                        lstNgayDauKy.Add(new AutoCompleteEntry(LDateTime.StringToDate(str,ApplicationConstant.defaultDateTimeFormat).ToShortDateString(),str,""));
                    }
                    ucColNgayBDau.GiaTri = LDateTime.DateToString(Convert.ToDateTime(e.Cell.Value), ApplicationConstant.defaultDateTimeFormat);
                    ucColNgayBDau.lstComboBox = lstNgayDauKy;
                }
            }
        }

        void radDanhSachKheUoc_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            raddgrThongTinLapLich.ItemsSource = null;
            raddgrLichTraNo.ItemsSource = null;
            raddgrThucTraNo.ItemsSource = null;
            _TDVMKHEUOC = radDanhSachKheUoc.SelectedItem as TDVM_KHE_UOC;
            if (LObject.IsNullOrEmpty(_TDVMKHEUOC)) _TDVMKHEUOC = new TDVM_KHE_UOC();
            raddgrThongTinLapLich.ItemsSource = _TDVMKHEUOC.DSACH_KHOACHVM;
            raddgrLichTraNo.ItemsSource = _TDVMKHEUOC.DSACH_KHOACHVM_CTIET;
            raddgrThucTraNo.ItemsSource = _TDVMKHEUOC.DSACH_KHOACHVM_CTIET;
        }

        void LockControl()
        {
            
        }

        void tlbSave_Click(object sender, RoutedEventArgs e)
        {
            LayGiaTri(objDanhSach);
            CommonFunction.CloseUserControl(this);
        }

        void tlbClose_Click(object sender, RoutedEventArgs e)
        {
            LayGiaTri(objDanhSach);
            CommonFunction.CloseUserControl(this);
        }

        #endregion

        #region Xu ly nghiep vu
        bool Validation()
        {
            return true;
        }
        #endregion
    }
}
