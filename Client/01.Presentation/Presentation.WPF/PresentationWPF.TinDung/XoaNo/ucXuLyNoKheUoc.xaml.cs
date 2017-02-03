using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Reflection;
using Microsoft.Windows.Controls.Ribbon;
using Utilities.Common;
using PresentationWPF.TinDung;
using PresentationWPF.CustomControl;
using Presentation.Process;
using Presentation.Process.PopupServiceRef;
using Presentation.Process.Common;

namespace PresentationWPF.TinDung.XoaNo
{
    /// <summary>
    /// Interaction logic for ucXuLyNoKheUoc.xaml
    /// </summary>
    public partial class ucXuLyNoKheUoc : UserControl
    {
        #region KhaiBao
        public static RoutedCommand SelectCommand = new RoutedCommand();
        public static RoutedCommand CloseCommand = new RoutedCommand();
        List<XuLyNoQuaHan> lstXuLyNoQuaHan = new List<XuLyNoQuaHan>();

        public delegate void LayGiaTriXuLy(List<XuLyNoQuaHan> lstXuLyNo);
        public LayGiaTriXuLy LayGiaTriTraVe;
        int idKheUoc=0;
        int idKhachHang=0;
        string mAKhachHang="";
        string tEnKhachHang="";
        List<AutoCompleteEntry> lstThoiHanVay = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstNhomNo = new List<AutoCompleteEntry>();
        List<XuLyNoQuaHan.TSDBXuLy> lstTSDB = new List<XuLyNoQuaHan.TSDBXuLy>();
        List<DataRow> lstPopup = new List<DataRow>();
        
        public void LayDuLieuTuPopup(List<DataRow> lst)
        {
            lstPopup = lst;
        }
        #endregion
        #region KhoiTao
        public ucXuLyNoKheUoc()
        {
            InitializeComponent();
            BindHotkey();
            ClearForm();
        }
        
        #endregion

        #region Dang ky hot key, shortcut key
        /// <summary>
        /// Binding HotKey
        /// </summary>
        private void BindHotkey()
        {
            foreach (var child in Toolbar.Children)
            {
                if (child.GetType() == typeof(RibbonButton))
                {
                    RibbonButton tlb = (RibbonButton)child;
                    KeyBinding key = new KeyBinding();
                    string strTinhNang = tlb.Name.Substring(3, tlb.Name.Length - 3);
                    if (strTinhNang.Equals("Select"))
                    {
                        KeyGesture keyg = new KeyGesture(Key.S, ModifierKeys.Control);
                        key = new KeyBinding(SelectCommand, keyg);
                        key.Gesture = keyg;
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DONG)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.Escape, ModifierKeys.None);
                        key = new KeyBinding(CloseCommand, keyg);
                        key.Gesture = keyg;
                    }

                    InputBindings.Add(key);
                }
            }
        }
        private void SelectCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void SelectCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            onClosePopup();
        }
        private void CloseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            CustomControl.CommonFunction.CloseUserControl(this);
        }
        private void CloseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
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
            if (strTinhNang.Equals("Select"))
            {
                onClosePopup();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DONG)))
            {
                CustomControl.CommonFunction.CloseUserControl(this);
            }
        }

        /// <summary>
        /// Sự kiện shortcutkey
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnShortcutKey_Click(object sender, RoutedEventArgs e)
        {
            string strTinhNang = ((MenuItem)sender).Name.Substring(3, ((MenuItem)sender).Name.Length - 3);
            if (strTinhNang.Equals("Select"))
            {
                onClosePopup();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DONG)))
            {
                CustomControl.CommonFunction.CloseUserControl(this);
            }
        }
        #endregion

        #region XuLyGiaoDien
        void ClearForm()
        {
            teldtNgayVay.Value = null;
            teldtNgayDaoHan.Value = null;
        }
        #endregion
        #region XuLyNghiepVu
        void onClosePopup()
        {
            LayGiaTriTraVe(lstXuLyNoQuaHan);
            CommonFunction.CloseUserControl(this);
        }
        void LoadDataGridViewKheUoc()
        {
            grdKheUoc.ItemsSource = lstXuLyNoQuaHan;
        }

        void LoadDataGridViewTaiSanDB()
        {
            grdTaiSanDamBao.ItemsSource = lstTSDB;
        }
        #endregion

        private void btnAddKheUoc_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!Vadidated())
                    return;
                XuLyNoQuaHan objXuLyNoQHan = new XuLyNoQuaHan();
                objXuLyNoQHan.KheUocXuLy = new XuLyNoQuaHan.KheUocXuLyNo();
                objXuLyNoQHan.KheUocXuLy.ID = idKheUoc;
                objXuLyNoQHan.KheUocXuLy.MA_KUOC = txtSoKheUoc.Text;
                objXuLyNoQHan.KheUocXuLy.ID_KHANG = idKhachHang;
                objXuLyNoQHan.KheUocXuLy.MA_KHANG = mAKhachHang;
                objXuLyNoQHan.KheUocXuLy.TEN_KHANG = tEnKhachHang;
                objXuLyNoQHan.KheUocXuLy.NGAY_VAY = LDateTime.DateToString((DateTime)teldtNgayVay.Value, ApplicationConstant.defaultDateTimeFormat);
                objXuLyNoQHan.KheUocXuLy.THOI_HAN_VAY = (int)txtThoiHanVay.Value;
                objXuLyNoQHan.KheUocXuLy.THOI_HAN_DVI = lstThoiHanVay.ElementAt(cmbThoiHanVay.SelectedIndex).KeywordStrings.First();
                objXuLyNoQHan.KheUocXuLy.LSuat = (decimal)txtLaiSuat.Value;
                objXuLyNoQHan.KheUocXuLy.NGAY_DHAN = LDateTime.DateToString((DateTime)teldtNgayDaoHan.Value, ApplicationConstant.defaultDateTimeFormat);
                objXuLyNoQHan.KheUocXuLy.SO_TIEN_VAY = (decimal)txtSoTienVay.Value;
                objXuLyNoQHan.KheUocXuLy.DU_NO_GOC = (decimal)txtDuNoGoc.Value;
                objXuLyNoQHan.KheUocXuLy.DU_NO_LAI = (decimal)txtDuNoLai.Value;
                objXuLyNoQHan.KheUocXuLy.NHOM_NO_HT = lstNhomNo.ElementAt(cmbNhomNo.SelectedIndex).KeywordStrings.First();
                objXuLyNoQHan.XU_LY_NO_GOC = (decimal)txtXuKyDuNoGoc.Value;
                objXuLyNoQHan.XU_LY_NO_LAI = (decimal)txtXuKyDuNoLai.Value;
                objXuLyNoQHan.DU_PHONG_CHUNG = (decimal)txtDuPhongChung.Value;
                objXuLyNoQHan.DU_PHONG_CU_THE = (decimal)txtDuPhongCuThe.Value;
                objXuLyNoQHan.CHI_PHI = (decimal)txtChiPhi.Value;
                objXuLyNoQHan.DANH_SACH_TSDB = lstTSDB;
                int iIndex = -1;
                iIndex = lstXuLyNoQuaHan.IndexOf(lstXuLyNoQuaHan.FirstOrDefault(i => i.KheUocXuLy.ID.Equals(idKheUoc)));
                if (iIndex > -1)
                    lstXuLyNoQuaHan[iIndex] = objXuLyNoQHan;
                else
                    lstXuLyNoQuaHan.Add(objXuLyNoQHan);
            }
            catch (Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        bool Vadidated()
        {
            bool bBool = true;
            if (idKheUoc == 0)
                bBool = false;
            else if (idKhachHang == 0)
                bBool = false;
            else if (txtXuKyDuNoGoc.Value == 0)
                bBool = false;
            else if (txtXuKyDuNoLai.Value == 0)
                bBool = false;
            else if (lstTSDB.Count == 0)
                bBool = false;
            return bBool;
        }

        private void btnAddTSDB_Click(object sender, RoutedEventArgs e)
        {
            List<string> lstDieuKien = new List<string>();
            lstDieuKien.Add(idKheUoc.ToString());
            lstPopup = new List<DataRow>();
            PopupProcess popupProcess = new PopupProcess();
            popupProcess.getPopupInformation("POPUP_DS_TAISAN_XULY", lstDieuKien);
            SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;
            ucPopup popup = new ucPopup(true, simplePopupResponse, true);
            popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
            Window win = new Window();
            win.Content = popup;
            win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            win.ShowDialog();
            if (lstPopup.Count > 0)
            {
                foreach (DataRow dr in lstPopup)
                {
                    XuLyNoQuaHan.TSDBXuLy objTSDB = new XuLyNoQuaHan.TSDBXuLy();
                    for(int i=0;i<dr.Table.Columns.Count;i++)
                    {
                        string sName = dr.Table.Columns[i].ColumnName;
                        PropertyInfo property = objTSDB.GetType().GetProperty(sName);
                        if (property != null)
                        {
                            if (property.PropertyType == typeof(decimal))
                                property.SetValue(objTSDB, Convert.ToDecimal(dr[i]), null);
                            else if (property.PropertyType == typeof(int))
                                property.SetValue(objTSDB, Convert.ToInt32(dr[i]), null);
                            else
                                property.SetValue(objTSDB, dr[i].ToString(), null);
                        }
                    }
                    lstTSDB.Add(objTSDB);
                }
                grdTaiSanDamBao.ItemsSource = lstTSDB;
            }
        }

        private void btnDeleteTSDB_Click(object sender, RoutedEventArgs e)
        {
            if (LMessage.ShowMessage("M.DungChung.HoiXoa", LMessage.MessageBoxType.Question) != MessageBoxResult.Yes)
                return;
            foreach (XuLyNoQuaHan.TSDBXuLy objTSDB in grdTaiSanDamBao.SelectedItems)
            {
                lstTSDB.Remove(objTSDB);
            }
        }

        private void btnDeleteKheUoc_Click(object sender, RoutedEventArgs e)
        {
            if (LMessage.ShowMessage("M.DungChung.HoiXoa", LMessage.MessageBoxType.Question) != MessageBoxResult.Yes)
                return;
            foreach (XuLyNoQuaHan objXuLyNo in grdTaiSanDamBao.SelectedItems)
            {
                lstXuLyNoQuaHan.Remove(objXuLyNo);
            }
        }

        private void grdKheUoc_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (grdKheUoc.SelectedItems.Count == 0)
                return;
            XuLyNoQuaHan objXuLyNoQHan = (XuLyNoQuaHan)grdKheUoc.SelectedItems[0];
            if (objXuLyNoQHan != null)
            {
                LoadControlData(objXuLyNoQHan);
            }
        }

        public void LoadControlData(XuLyNoQuaHan objXuLyNoQHan)
        {
            idKheUoc = objXuLyNoQHan.KheUocXuLy.ID;
            txtSoKheUoc.Text = objXuLyNoQHan.KheUocXuLy.MA_KUOC;
            idKhachHang = objXuLyNoQHan.KheUocXuLy.ID_KHANG;
            mAKhachHang = objXuLyNoQHan.KheUocXuLy.MA_KHANG;
            tEnKhachHang = objXuLyNoQHan.KheUocXuLy.TEN_KHANG;
            teldtNgayVay.Value = LDateTime.StringToDate(objXuLyNoQHan.KheUocXuLy.NGAY_VAY,ApplicationConstant.defaultDateTimeFormat);
            txtThoiHanVay.Value = (double)objXuLyNoQHan.KheUocXuLy.THOI_HAN_VAY;
            cmbThoiHanVay.SelectedIndex = lstThoiHanVay.IndexOf(lstThoiHanVay.FirstOrDefault(i => i.KeywordStrings.First().Equals(objXuLyNoQHan.KheUocXuLy.THOI_HAN_DVI)));
            txtLaiSuat.Value = (double)objXuLyNoQHan.KheUocXuLy.LSuat;
            teldtNgayDaoHan.Value = LDateTime.StringToDate(objXuLyNoQHan.KheUocXuLy.NGAY_DHAN,ApplicationConstant.defaultDateTimeFormat);
            txtSoTienVay.Value = (double)objXuLyNoQHan.KheUocXuLy.SO_TIEN_VAY;
            txtDuNoGoc.Value = (double)objXuLyNoQHan.KheUocXuLy.DU_NO_GOC;
            txtDuNoLai.Value = (double)objXuLyNoQHan.KheUocXuLy.DU_NO_LAI;
            cmbNhomNo.SelectedIndex = lstNhomNo.IndexOf(lstNhomNo.FirstOrDefault(i => i.KeywordStrings.First().Equals(objXuLyNoQHan.KheUocXuLy.NHOM_NO_HT)));
            txtXuKyDuNoGoc.Value = (double)objXuLyNoQHan.XU_LY_NO_GOC;
            txtXuKyDuNoLai.Value = (double)objXuLyNoQHan.XU_LY_NO_LAI;
            txtDuPhongChung.Value = (double)objXuLyNoQHan.DU_PHONG_CHUNG;
            txtDuPhongCuThe.Value = (double)objXuLyNoQHan.DU_PHONG_CU_THE;
            txtChiPhi.Value = (double)objXuLyNoQHan.CHI_PHI;
            lstTSDB = objXuLyNoQHan.DANH_SACH_TSDB;
        }

        private void btnSoKheUoc_Click(object sender, RoutedEventArgs e)
        {

        }

        public void SetValue(List<XuLyNoQuaHan> lst)
        {
            lstXuLyNoQuaHan = lst;
        }
    }
}
