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
using System.Collections;
using Microsoft.Windows.Controls.Ribbon;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using Utilities.Common;
using Presentation.Process;
using Presentation.Process.HanMucServiceRef;
using Presentation.Process.QuanTriHeThongServiceRef;
using Presentation.Process.Common;
using PresentationWPF.CustomControl;

namespace PresentationWPF.HanMuc.HanMuc
{
    /// <summary>
    /// Interaction logic for ucHanMucChung.xaml
    /// </summary>
    public partial class ucHanMucChung : UserControl
    {
        #region Khai bao

        List<AutoCompleteEntry> lstSourceLoaiDTuong = new List<AutoCompleteEntry>();

        List<HT_NSD> dsNSD = new List<HT_NSD>();
        List<HT_NHNSD> dsNHNSD = new List<HT_NHNSD>();
        
        DataTable dtDoiTuong = new DataTable();

        QuanTriHeThongProcess qtht = new QuanTriHeThongProcess();

        public static RoutedCommand SaveCommand = new RoutedCommand();
        public static RoutedCommand ExportCommand = new RoutedCommand();
        public static RoutedCommand HelpCommand = new RoutedCommand();
        public static RoutedCommand CloseCommand = new RoutedCommand();


        #endregion

        #region Khoi tao

        public ucHanMucChung()
        {
            InitializeComponent();

            LoadCombobox();

            KhoiTaoGridDoiTuong();

            InitEventHandler();
        }

        private void LoadCombobox()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                
                AutoComboBox auto = new AutoComboBox();
                List<string> lstDieuKien = new List<string>();                

                #region Combobox Loại đối tượng
                lstDieuKien = new List<string>();
                lstDieuKien.Add(DatabaseConstant.DanhMuc.LOAI_DTUONG_KTHAC_TNGUYEN.getValue());
                auto.GenAutoComboBox(ref lstSourceLoaiDTuong, ref cmbDoiTuong, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien,
                    BusinessConstant.LoaiDoiTuong.NGUOI_SDUNG.layGiaTri());
                #endregion

            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void KhoiTaoGridDoiTuong()
        {
            DataTable dtDoiTuong = new DataTable();
            dtDoiTuong.Columns.Add("STT", typeof(int));
            dtDoiTuong.Columns.Add("ID", typeof(int));
            dtDoiTuong.Columns.Add("MA_CNANG", typeof(string));
            dtDoiTuong.Columns.Add("TEN_CNANG", typeof(string));
            dtDoiTuong.Columns.Add("HMUC_MIN", typeof(decimal));
            dtDoiTuong.Columns.Add("HMUC_MAX", typeof(decimal));

        }

        private void InitEventHandler()
        {           
            cmbDoiTuong.SelectionChanged += cmbDoiTuong_SelectionChanged;
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

        private void GetFormData(ref List<DC_HAN_MUC> lstHanMuc)
        {            
            try
            {
                lstHanMuc = new List<DC_HAN_MUC>();
                DC_HAN_MUC obj = new DC_HAN_MUC();

                foreach(DataRow dr in dtDoiTuong.Rows)
                {
                    obj = new DC_HAN_MUC();

                    obj.ID = 0;
                    obj.MA_DTUONG_LOAI = lstSourceLoaiDTuong.ElementAt(cmbDoiTuong.SelectedIndex).KeywordStrings.First();
                    obj.ID_DTUONG = Convert.ToInt32(dr["ID"]);
                    obj.MA_DTUONG = dr["MA"].ToString();
                    obj.LOAI_HAN_MUC = BusinessConstant.LOAI_HAN_MUC.CHUNG.layGiaTri();
                    obj.ID_CNANG = null;
                    obj.MA_CNANG = null;
                    obj.ID_TNANG = null;
                    obj.MA_TNANG = null;
                    obj.MIN = Convert.ToDecimal(dr["HMUC_MIN"]);
                    obj.MAX = Convert.ToDecimal(dr["HMUC_MAX"]);

                    //Thông tin kiểm soát
                    obj.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                    obj.TTHAI_NVU = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();
                    obj.MA_DVI_QLY = ClientInformation.MaDonVi;
                    obj.MA_DVI_TAO = ClientInformation.MaDonViGiaoDich;
                    obj.NGAY_NHAP = ClientInformation.NgayLamViecHienTai;
                    obj.NGUOI_NHAP = ClientInformation.TenDangNhap;
                    obj.NGAY_CNHAT = ClientInformation.NgayLamViecHienTai;
                    obj.NGUOI_CNHAT = ClientInformation.TenDangNhap;

                    lstHanMuc.Add(obj);
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        private void Luu()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            List<DC_HAN_MUC> lstHanMuc = null;            
            try
            {
                if (!Validation()) return;                

                GetFormData(ref lstHanMuc);

                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = false;
                ret = new HanMucProcess().HanMucChung(DatabaseConstant.Action.SUA, ref lstHanMuc,ref listClientResponseDetail);

                if (ret == true)
                {
                    LMessage.ShowMessage("M.DungChung.CapNhatThanhCong", LMessage.MessageBoxType.Information);
                }
                else
                {
                    CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                }
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            finally
            {
                lstHanMuc = null;
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private bool Validation()
        {
            try
            {

                if (cmbDoiTuong.SelectedIndex < 0)
                {
                    CommonFunction.ThongBaoChuaChon(lblLoaiDTuong.Content.ToString());
                    cmbDoiTuong.Focus();
                    return false;
                }
                if (grDSDoiTuong.SelectedItems.Count < 1)
                {
                    LMessage.ShowMessage("Chưa chọn đối tượng", LMessage.MessageBoxType.Warning);
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

        #endregion

        #region Xu ly giao dien

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {           
            BuildGridDoiTuong();
        }

        private void BuildGridDoiTuong()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                string maLoaiDTuong = lstSourceLoaiDTuong.ElementAt(cmbDoiTuong.SelectedIndex).KeywordStrings.First();
                if (maLoaiDTuong.IsNullOrEmptyOrSpace())
                {
                    dtDoiTuong = null;
                    grDSDoiTuong.ItemsSource = null;
                    return;
                }

                DataSet ds = new HanMucProcess().GetHanMucChung(maLoaiDTuong, ClientInformation.MaDonVi);
                if (ds != null && ds.Tables.Count > 0)
                {
                    dtDoiTuong = ds.Tables[0];
                    grDSDoiTuong.ItemsSource = null;
                    grDSDoiTuong.ItemsSource = dtDoiTuong.DefaultView;
                }
                else
                {
                    dtDoiTuong = null;
                    grDSDoiTuong.ItemsSource = null;
                }
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

        private string layTenDonViTheoDanhSach(string maDonVi, List<DM_DON_VI> listDonVi)
        {
            foreach (DM_DON_VI item in listDonVi)
            {
                if (maDonVi.Equals(item.MA_DVI))
                    return item.TEN_GDICH;
            }
            return "";
        }        

        private void cmbDoiTuong_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbDoiTuong.SelectedIndex >= 0)
            {                
                BuildGridDoiTuong();
            }
        }

        #endregion
    }
}
