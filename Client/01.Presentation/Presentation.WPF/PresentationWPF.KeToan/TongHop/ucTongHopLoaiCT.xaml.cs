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
using System.Data;
using Utilities.Common;
using Microsoft.Windows.Controls.Ribbon;
using Presentation.Process.Common;
using Presentation.Process;
using Presentation.Process.KeToanServiceRef;

namespace PresentationWPF.KeToan.TongHop
{
    /// <summary>
    /// Interaction logic for ucTongHopLoai.xaml
    /// </summary>
    public partial class ucTongHopLoaiCT : UserControl
    {
        #region Khai bao
        public static RoutedCommand ModifyCommand = new RoutedCommand();
        public static RoutedCommand MakeCommand = new RoutedCommand();
        public static RoutedCommand DeleteCommand = new RoutedCommand();
        public static RoutedCommand SaveCommand = new RoutedCommand();
        public static RoutedCommand HelpCommand = new RoutedCommand();
        public static RoutedCommand CloseCommand = new RoutedCommand();

        // Source combobox
        List<AutoCompleteEntry> lstSourceDonVi = new List<AutoCompleteEntry>();

        public event EventHandler OnSavingCompleted;

        private DataRow drPhanLoai = null;

        List<DataRow> lstPopup = new List<DataRow>();
        public void LayDuLieuTuPopup(List<DataRow> lst)
        {
            lstPopup = lst;
        }

        private string tthaiNvu = "";
        public string TthaiNvu
        {
            get { return tthaiNvu; }
            set { tthaiNvu = value; }
        }

        private int _idTaiKhoan = -1;

        private string dauPhanCachTK = ".";

        private bool daPhatSinhGD = false;
        private string tthaiDongMoTK = "MO";

        #endregion

        #region Khoi tao
        public ucTongHopLoaiCT()
        {
            InitializeComponent();
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.KhachHang;component/ChuyenDiaBan/ucPhanLoaiCT.xaml", ref Toolbar, ref mnuMain);
            foreach (var item in mnuMain.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += btnShortcutKey_Click;
            }

            
            
            BindShortkey();
            InitCombobox();
        }

        
        /// <summary>
        /// Khoi tao combobox
        /// </summary>
        private void InitCombobox()
        {
            AutoComboBox auto = new AutoComboBox();
            List<string> lstDK = new List<string>();
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                // Khởi tạo điều kiện gọi danh mục
                List<string> lstDieuKien = new List<string>();

                // khởi tạo combobox
                auto.GenAutoComboBox(ref lstSourceDonVi, ref cmbChiNhanh, DatabaseConstant.DanhSachTruyVan.COMBOBOX_CNHANH.getValue(), lstDieuKien, ClientInformation.MaDonVi);
                dtTuNgay.Value = ClientInformation.NgayLamViecHienTai.StringToDate(ApplicationConstant.defaultDateTimeFormat);
                DataTable dt=null;
                LDatatable.MakeParameterTable(ref dt);
                DataSet ds = new KeToanProcess().getDanhSachTongHop(dt);
                raddgrDoiTuongTK.ItemsSource = ds.Tables[0].DefaultView;
                raddgrDoiTuongTK.Rebind();
            }
            catch (System.Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiLoadDuLieu", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
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
                    if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.S, ModifierKeys.Control);
                        key = new KeyBinding(DeleteCommand, keyg);
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
            e.CanExecute = false;
        }
        private void SaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //onSave();
        }

        private void MakeCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = tlbSyn.IsEnabled;
        }
        private void MakeCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //onMake();
        }

        private void ModifyCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
        }
        private void ModifyCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //beforeModifyFromDetail();
        }

        private void DeleteCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
        }
        private void DeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //beforeDelete();
        }

        private void HelpCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void HelpCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            //onHelp();
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
            else if (strTinhNang.Equals("Syn"))
            {
                TongHopDuLieu();
                //beforeModifyFromDetail();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
                //beforeModifyFromDetail();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
            {
                //beforeDelete();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TAO_DU_LIEU)))
            {
                //onMake();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRO_GIUP)))
            {
                onHelp();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DONG)))
            {
                onClose();
            }
        }

        private void btnShortcutKey_Click(object sender, RoutedEventArgs e)
        {
            string strTinhNang = ((MenuItem)sender).Name.Substring(3, ((MenuItem)sender).Name.Length - 3);
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU)))
            {
                //onSave();
            }
            else if (strTinhNang.Equals("Syn"))
            {
                TongHopDuLieu();
                //beforeModifyFromDetail();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TAO_DU_LIEU)))
            {
                //onMake();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
                //beforeModifyFromDetail();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
            {
                //beforeDelete();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRO_GIUP)))
            {
                onHelp();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DONG)))
            {
                onClose();
            }
        }

        #endregion 

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

        #region Xy ly nghiep vu
        private void TongHopDuLieu()
        {
            Cursor = Cursors.Wait;
            try
            {
                DataTable dtTongHop = new DataTable();
                DataTable dtThamSo = null;
                TONG_HOP_DU_LIEU objTongHop = new TONG_HOP_DU_LIEU();
                LDatatable.MakeParameterTable(ref dtThamSo);
                List<ClientResponseDetail> lstResponse = new List<ClientResponseDetail>();
                if (!Validation())
                {
                    foreach (DataRowView dv in raddgrDoiTuongTK.SelectedItems)
                    {
                        if (dtTongHop.Columns.Count == 0)
                        {
                            foreach (DataColumn dc in dv.Row.Table.Columns)
                            {
                                dtTongHop.Columns.Add(dc.ColumnName,dc.DataType);
                            }
                        }
                        dtTongHop.Rows.Add(dv.Row.ItemArray);
                        
                    }
                    AutoCompleteEntry au = lstSourceDonVi.ElementAt(cmbChiNhanh.SelectedIndex);
                    LDatatable.AddParameter(ref dtThamSo, "@MA_DVI_QLY", "String", au.KeywordStrings.FirstOrDefault());
                    LDatatable.AddParameter(ref dtThamSo, "@TU_NGAY", "String", LDateTime.DateToString(dtTuNgay.Value.GetValueOrDefault(), ApplicationConstant.defaultDateTimeFormat));
                    LDatatable.AddParameter(ref dtThamSo, "@NGUOI_THOP", "String", ClientInformation.TenDangNhap);
                    LDatatable.AddParameter(ref dtThamSo, "@NGAY_THOP", "String", ClientInformation.NgayLamViecHienTai);
                    dtThamSo.TableName = "ThamSo";
                    dtTongHop.TableName = "TongHop";
                    objTongHop.DSACH_THAM_SO = dtThamSo;
                    objTongHop.DSACH_TONG_HOP = dtTongHop;
                    if (ApplicationConstant.ResponseStatus.THANH_CONG == new KeToanProcess().TongHopDuLieu(DatabaseConstant.Function.KT_TONG_HOP_DS, DatabaseConstant.Action.TONG_HOP, ref objTongHop, ref lstResponse))
                    {
                        Cursor = Cursors.Arrow;
                        LMessage.ShowMessage("Tổng hợp dữ liệu thành công", LMessage.MessageBoxType.Information);
                    }
                    else
                    {
                        Cursor = Cursors.Arrow;
                        LMessage.ShowMessage("Tổng hợp dữ liệu không thành công", LMessage.MessageBoxType.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                LMessage.ShowMessage("Tổng hợp dữ liệu không thành công", LMessage.MessageBoxType.Error);
            }
            finally
            {
                Cursor = Cursors.Arrow;
            }
        }

        private bool Validation()
        {
            if (MessageBoxResult.OK != LMessage.ShowMessage("Hãy tạm dừng giao dịch trước khi tổng hợp dữ liệu", LMessage.MessageBoxType.Question))
            {
                return false;
            }
            if (raddgrDoiTuongTK.SelectedItems.IsNullOrEmpty() || raddgrDoiTuongTK.SelectedItems.Count < 1)
            {
                LMessage.ShowMessage("M.DungChung.ChuaChonBanGhi", LMessage.MessageBoxType.Warning);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Trợ giúp
        /// </summary>
        private void onHelp()
        {
        }

        /// <summary>
        /// Đóng form
        /// </summary>
        private void onClose()
        {
            Release();
            if (OnSavingCompleted != null)
            {
                OnSavingCompleted(this, EventArgs.Empty);
            }
            CustomControl.CommonFunction.CloseUserControl(this);
        }

        /// <summary>
        /// Hàm xử lý giải phóng lock dữ liệu cho form
        /// </summary>
        private void Release()
        {
            
        }
        #endregion
    }
}
