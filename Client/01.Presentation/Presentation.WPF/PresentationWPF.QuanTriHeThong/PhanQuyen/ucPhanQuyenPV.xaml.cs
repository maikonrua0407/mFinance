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
using Presentation.Process.QuanTriHeThongServiceRef;
using System.Data;
using Presentation.Process;
using Microsoft.Windows.Controls.Ribbon;
using Utilities.Common;
using Presentation.Process.Common;
using Telerik.Windows.Controls;
using Presentation.Process.PopupServiceRef;
using System.Collections;

namespace PresentationWPF.QuanTriHeThong.PhanQuyen
{
    /// <summary>
    /// Interaction logic for ucPhanQuyenPV.xaml
    /// </summary>
    public partial class ucPhanQuyenPV : UserControl
    {
        #region Khai bao

        List<AutoCompleteEntry> lstSourceLoaiPhamVi = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceLoaiDTuong = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceLoaiTNguyen = new List<AutoCompleteEntry>();

        List<HT_NSD> dsNSD = new List<HT_NSD>();
        List<HT_NHNSD> dsNHNSD = new List<HT_NHNSD>();

        DataTable dt = new DataTable();

        DataTable dtMaster = new DataTable();
        DataTable dtRoot = new DataTable();

        QuanTriHeThongProcess qtht = new QuanTriHeThongProcess();
        bool isLoaded = false;

        public static RoutedCommand SaveCommand = new RoutedCommand();
        public static RoutedCommand ExportCommand = new RoutedCommand();
        public static RoutedCommand HelpCommand = new RoutedCommand();
        public static RoutedCommand CloseCommand = new RoutedCommand();

        List<DonViNode> lstDonViNode = new List<DonViNode>();
        List<PhamViDonViNode> lstPhamViDonViNode = new List<PhamViDonViNode>();
        #endregion

        #region Khoi tao

        public ucPhanQuyenPV()
        {
            InitializeComponent();
            LoadDuLieu();
            BindShortkey();
        }

        #endregion

        #region Dang ky hot key

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
            {
                XuatExcel();
            }
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
                        key = new KeyBinding(SaveCommand, keyg);
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
            Luu();
        }

        private void ExportCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void ExportCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            XuatExcel();
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

        #endregion

        #region Xu ly nghiep vu

        private void Luu()
        {
            //Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                string loaiDoiTuong = null;
                loaiDoiTuong = lstSourceLoaiDTuong.ElementAt(cmbDoiTuong.SelectedIndex).KeywordStrings.First();
                DataRow dr = (DataRow)grDSDoiTuong.SelectedItem;
                if (dr != null)
                {
                    PHAM_VI objPhamVi = new PHAM_VI();
                    objPhamVi.ID_DTUONG = int.Parse(dr["ID"].ToString());
                    objPhamVi.MA_DTUONG = dr[2].ToString();
                    objPhamVi.MA_DVI_QLY = dr[5].ToString();
                    objPhamVi.MA_DVI_TAO = ClientInformation.MaDonViGiaoDich;
                    objPhamVi.MA_DTUONG_LOAI = loaiDoiTuong;
                    objPhamVi.MA_PVI_LOAI = lstSourceLoaiPhamVi.ElementAt(cmbLoaiPhamVi.SelectedIndex).KeywordStrings.First();
                    int idxMA_PVI = 0;
                    List<string> lstMaPhamVi = new List<string>();
                    foreach (RadTreeViewItem item in trvTreeDonVi.CheckedItems)
                    {
                        //if (item.Level == 2)
                        //{
                        //    lstMaPhamVi.Add(item.Tag.ToString());
                        //}
                        lstMaPhamVi.Add(item.Tag.ToString());
                    }

                    objPhamVi.MA_PVI = new string[lstMaPhamVi.Count];
                    objPhamVi.ID_PVI = new int[lstMaPhamVi.Count];
                    objPhamVi.PHAN_LOAI = new string[lstMaPhamVi.Count];
                    foreach (string item in lstMaPhamVi)
                    {
                        string[] tag = item.Split('#');
                        objPhamVi.MA_PVI[idxMA_PVI] = tag[0];
                        objPhamVi.PHAN_LOAI[idxMA_PVI] = tag[1];
                        objPhamVi.ID_PVI[idxMA_PVI] = Int32.Parse(tag[2]);
                        idxMA_PVI++;
                    }
                    objPhamVi.NGUOI_NHAP = ClientInformation.TenDangNhap;
                    objPhamVi.NGUOI_CNHAT = ClientInformation.TenDangNhap;
                    objPhamVi.NGAY_NHAP = ClientInformation.NgayLamViecHienTai;
                    objPhamVi.NGAY_CNHAT = ClientInformation.NgayLamViecHienTai;
                    QuanTriHeThongProcess process = new QuanTriHeThongProcess();
                    string responseMessage=string.Empty;
                    ApplicationConstant.ResponseStatus status = process.LuuPhanQuyenPhamVi(ref objPhamVi, ref responseMessage);
                    if (status == ApplicationConstant.ResponseStatus.THANH_CONG)
                    {
                        LoadDuLieuPhanQuyen(objPhamVi);
                        LMessage.ShowMessage("M.DungChung.LuuDuLieuThanhCong", LMessage.MessageBoxType.Information);
                        //LMessage.ShowMessage(responseMessage, LMessage.MessageBoxType.Information);
                    }
                    else
                        LMessage.ShowMessage("M.DungChung.LuuDuLieuKhongThanhCong", LMessage.MessageBoxType.Warning);
                }
                else
                {
                    LMessage.ShowMessage("Choose object for role setting", LMessage.MessageBoxType.Warning);
                    return;
                }
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            //Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void onHelp()
        {
            CustomControl.CommonFunction.ShowHelp(this);
        }

        private void XuatExcel()
        {
            PresentationWPF.CustomControl.CommonFunction.ExportGridToExcel(grDSDoiTuong);
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
                lstSourceLoaiPhamVi.Insert(0, new AutoCompleteEntry("Organization scope", new string[2] { BusinessConstant.LOAI_PHAM_VI.DON_VI.layGiaTri(), "1" }));
                lstSourceLoaiPhamVi.Insert(1, new AutoCompleteEntry("Area scope", new string[2] { BusinessConstant.LOAI_PHAM_VI.DIA_LY.layGiaTri(), "2" }));
                auto.GenAutoComboBox(ref lstSourceLoaiPhamVi, ref cmbLoaiPhamVi, null, lstDieuKien);
                cmbLoaiPhamVi.SelectedIndex = 0;
                cmbLoaiPhamVi.IsEnabled = false;

                // lấy dữ liệu đổ source cho combobox Loại đối tượng
                lstDieuKien = new List<string>();
                lstDieuKien.Add(DatabaseConstant.DanhMuc.LOAI_DTUONG_KTHAC_TNGUYEN.getValue());
                auto.GenAutoComboBox(ref lstSourceLoaiDTuong, ref cmbDoiTuong, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien,
                    BusinessConstant.LoaiDoiTuong.NHOM_NGUOI_SDUNG.layGiaTri());

                // Hiển thị lưới dữ liệu đối tượng
                BuildGridDoiTuong();
                // Hiển thị lưới dữ liệu phân quyền
                BuildTreeDonVi();
                LoadDuLieuPhanQuyen(null);
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void LoadDuLieuPhanQuyen(PHAM_VI objPhamVi)
        {
            if (objPhamVi.IsNullOrEmpty())
            {
                lstPhamViDonViNode = null;
                lstPhamViDonViNode = new List<PhamViDonViNode>();

                string loaiDoiTuong = null;
                loaiDoiTuong = lstSourceLoaiDTuong.ElementAt(cmbDoiTuong.SelectedIndex).KeywordStrings.First();
                DataRow dr = (DataRow)grDSDoiTuong.SelectedItem;
                if (dr != null)
                {
                    objPhamVi = new PHAM_VI();
                    objPhamVi.ID_DTUONG = int.Parse(dr["ID"].ToString());
                    objPhamVi.MA_DTUONG = dr[2].ToString();
                    objPhamVi.MA_DVI_QLY = dr[5].ToString();
                    objPhamVi.MA_DVI_TAO = ClientInformation.MaDonVi;
                    objPhamVi.MA_DTUONG_LOAI = loaiDoiTuong;
                    objPhamVi.MA_PVI_LOAI = lstSourceLoaiPhamVi.ElementAt(cmbLoaiPhamVi.SelectedIndex).KeywordStrings.First();
                    QuanTriHeThongProcess process = new QuanTriHeThongProcess();
                    string responseMessage = string.Empty;
                    ApplicationConstant.ResponseStatus status = process.LayPhanQuyenPhamVi(ref objPhamVi, ref responseMessage);
                    if (status == ApplicationConstant.ResponseStatus.THANH_CONG && !objPhamVi.MA_PVI.IsNullOrEmpty())
                    {
                        for (int i = 0; i < objPhamVi.MA_PVI.Count(); i++)
                        {
                            PhamViDonViNode phamViDonViNode = new PhamViDonViNode();
                            phamViDonViNode.ID_PVI = Int32.Parse(objPhamVi.ID_PVI[i].ToString());
                            phamViDonViNode.MA_PVI = objPhamVi.MA_PVI[i].ToString();
                            phamViDonViNode.PHAN_LOAI = objPhamVi.PHAN_LOAI[i].ToString();

                            lstPhamViDonViNode.Add(phamViDonViNode);
                        }

                        foreach (RadTreeViewItem item in getTreeViewItems(trvTreeDonVi))
                        {
                            if (!item.Tag.IsNullOrEmpty())
                            {
                                string[] tag = item.Tag.ToString().Split('#');
                                int sub_count_dm = Int32.Parse(tag[3]);
                                string node = tag[0].ToString();

                                if (sub_count_dm > 0)
                                {
                                    int sub_count_pv = getSubCountNodePv(node);
                                    if (sub_count_pv < sub_count_dm && sub_count_pv > 0)
                                    {
                                        item.CheckState = System.Windows.Automation.ToggleState.Indeterminate;
                                    }
                                    else if (sub_count_pv == sub_count_dm)
                                    {
                                        item.IsChecked = true;
                                    }
                                    else
                                    {
                                        item.IsChecked = false;
                                    }
                                }
                                else
                                {
                                    item.IsChecked = objPhamVi.MA_PVI.Contains(node);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                foreach (RadTreeViewItem item in trvTreeDonVi.Items)
                {
                    item.IsChecked = objPhamVi.MA_PVI.Contains(item.Tag.ToString());
                }
            }
        }

        private int getSubCountNodePv(string node)
        {
            List<string> lstMaDonVi = lstDonViNode.Where(item => item.NODE_PARENT.Equals(node)).Select(e => e.NODE).ToList();

            List<string> lstMaPhamViDonVi = lstPhamViDonViNode.Select(item => item.MA_PVI).ToList();
            List<string> lstMaPhamViSubDonVi = new List<string>();

            foreach (string item in lstMaPhamViDonVi)
            {
                if (lstMaDonVi.Contains(item))
                {
                    lstMaPhamViSubDonVi.Add(item);
                }
            }

            int ret = lstMaPhamViSubDonVi.Count();

            return ret;
        }

        private static List<RadTreeViewItem> getTreeViewItems(RadTreeView treeView)
        {
            List<RadTreeViewItem> returnItems = new List<RadTreeViewItem>();
            for (int x = 0; x < treeView.Items.Count; x++)
            {
                returnItems.AddRange(getTreeViewItems((RadTreeViewItem)treeView.Items[x]));
            }
            return returnItems;
        }
        private static List<RadTreeViewItem> getTreeViewItems(RadTreeViewItem currentTreeViewItem)
        {
            List<RadTreeViewItem> returnItems = new List<RadTreeViewItem>();
            returnItems.Add(currentTreeViewItem);
            for (int x = 0; x < currentTreeViewItem.Items.Count; x++)
            {
                returnItems.AddRange(getTreeViewItems((RadTreeViewItem)currentTreeViewItem.Items[x]));
            }
            return returnItems;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!isLoaded)
            {
                // Khởi tạo các sự kiện cho control
                cmbDoiTuong.SelectionChanged += cmbDoiTuong_SelectionChanged;
                cmbLoaiPhamVi.SelectionChanged += cmbLoaiPhamVi_SelectionChanged;
                grDSDoiTuong.SelectionChanged += grDSDoiTuong_SelectionChanged;
                //grDanhSach.SelectionChanged += grDanhSach_SelectionChanged;
                //grDanhSach.SelectedCellsChanged += grDanhSach_SelectedCellsChanged;
                isLoaded = true;
            }
        }

        private void BuildTreeDonVi()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                lstDonViNode = null;
                lstDonViNode = new List<DonViNode>();

                DataRow dr = (DataRow)grDSDoiTuong.SelectedItem;
                if (dr != null)
                {
                    string selectDonVi = dr[5].ToString();
                    PopupProcess Popupprocess = new PopupProcess();
                    List<string> lstDK = new List<string>();
                    Popupprocess.getPopupInformation(DatabaseConstant.DanhSachTruyVan.DM_TREE_DONVI.getValue(), lstDK);
                    SimplePopupResponse simplePopupResponse = Presentation.Process.Common.ClientInformation.SimplePopup;
                    if (simplePopupResponse.DataSetSource.Tables.Count > 0)
                    {
                        dtMaster = simplePopupResponse.DataSetSource.Tables[0];

                        foreach (DataRow row in dtMaster.Rows)
                        {
                            DonViNode donViNode = new DonViNode();
                            donViNode.NODE = row["NODE"].ToString();
                            donViNode.NODE_PARENT = row["NODE_PARENT"].ToString();
                            donViNode.NODE_NAME = row["NODE_NAME"].ToString();
                            donViNode.TYPE = row["TYPE"].ToString();
                            donViNode.EXT = Int32.Parse(row["EXT"].ToString());
                            donViNode.SUB_COUNT = Int32.Parse(row["SUB_COUNT"].ToString());

                            lstDonViNode.Add(donViNode);
                        }

                        string loaiDonVi="";
                        foreach (DataRow row in dtMaster.Rows)
                        {
                            if (row["NODE"].ToString().Equals(selectDonVi))
                            {
                                loaiDonVi = row["TYPE"].ToString();
                                break;
                            }
                        }
                        if (!loaiDonVi.Equals("HSO"))
                        {
                            for (int i = dtMaster.Rows.Count - 1; i >= 0; i--)
                            {
                                DataRow row = dtMaster.Rows[i];
                                if (row["TYPE"].ToString() != "DVI" && row["NODE"].ToString() != selectDonVi && row["NODE_PARENT"].ToString() != selectDonVi)
                                    dtMaster.Rows.Remove(row);
                            }
                        }
                    }
                    if (simplePopupResponse.DataSetSource.Tables.Count > 1)
                    {
                        dtMaster = simplePopupResponse.DataSetSource.Tables[0];
                        dtRoot = simplePopupResponse.DataSetSource.Tables[1];
                    }

                    // Lấy chức năng theo phân hệ
                    string maPhanHe = lstSourceLoaiPhamVi.ElementAt(cmbLoaiPhamVi.SelectedIndex).KeywordStrings.First();
                    trvTreeDonVi.Items.Clear();
                    if (dtRoot != null && dtRoot.Rows.Count > 0)
                        BuildParentTree("");
                    else
                        BuildTree("");
                }
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        protected void BuildParentTree(string parentNote, RadTreeViewItem node = null)
        {
            try
            {
                foreach (DataRow row in dtRoot.Rows)
                {
                    if (row["NODE_PARENT"].ToString() == parentNote)
                    {
                        RadTreeViewItem subItem = new RadTreeViewItem();
                        subItem.Header = row["NODE_NAME"].ToString();
                        subItem.Tag = row["NODE"].ToString() + "#" + row["TYPE"].ToString() + "#" + row["EXT"].ToString() + "#" + row["SUB_COUNT"].ToString();
                        subItem.Uid = row["EXT"].ToString();
                        //subItem.IsExpanded = true;
                        if (node == null)
                            trvTreeDonVi.Items.Add(subItem);
                        else
                            node.Items.Add(subItem);
                        BuildTree(row["NODE"].ToString(), subItem);
                        BuildParentTree(row["NODE"].ToString(), subItem);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void BuildTree(string parentNote, RadTreeViewItem node = null)
        {
            try
            {
                foreach (DataRow row in dtMaster.Rows)
                {
                    if (row["NODE_PARENT"].ToString() == parentNote)
                    {
                        RadTreeViewItem subItem = new RadTreeViewItem();
                        subItem.Header = row["NODE_NAME"].ToString();
                        subItem.Tag = row["NODE"].ToString() + "#" + row["TYPE"].ToString() + "#" + row["EXT"].ToString() + "#" + row["SUB_COUNT"].ToString();
                        subItem.Uid = row["EXT"].ToString();
                        subItem.IsExpanded = true;
                        if (node == null)
                            trvTreeDonVi.Items.Add(subItem);
                        else
                            node.Items.Add(subItem);
                        BuildTree(row["NODE"].ToString(), subItem);
                    }
                }
            }
            catch (Exception ex)
            { }
        }

        private void BuildGridDoiTuong()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                List<DM_DON_VI> listDonVi = new List<DM_DON_VI>();
                // lấy loại đối tượng từ combo
                string loaiDoiTuong = lstSourceLoaiDTuong.ElementAt(cmbDoiTuong.SelectedIndex).KeywordStrings.First();
                // Tạo source thông tin đối tượng
                DataTable dtDoiTuong = new DataTable();
                dtDoiTuong.Columns.Add("ID", typeof(int));
                dtDoiTuong.Columns.Add(LLanguage.SearchResourceByKey("U.DungChung.STT"), typeof(int));
                dtDoiTuong.Columns.Add(LLanguage.SearchResourceByKey("U.QuanTriHeThong.PhanQuyen.ucPhanQuyenCN.Ma"), typeof(string));
                dtDoiTuong.Columns.Add(LLanguage.SearchResourceByKey("U.QuanTriHeThong.PhanQuyen.ucPhanQuyenCN.Ten"), typeof(string));
                dtDoiTuong.Columns.Add(LLanguage.SearchResourceByKey("U.QuanTriHeThong.PhanQuyen.ucPhanQuyenCN.DonVi"), typeof(string));
                dtDoiTuong.Columns.Add("MA_DON_VI", typeof(string));
                int stt = 0;

                // Nếu người dùng là CAP_SA hoặc CAP_QTTW thì lấy danh sách đơn vị
                if (ClientInformation.LoaiNguoiSuDung.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_SA.layGiaTri()) ||
                    ClientInformation.LoaiNguoiSuDung.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_QTTW.layGiaTri()))
                {
                    listDonVi = qtht.layDanhSachDonVi();
                }
                // Còn lại lấy đơn vị của người dùng đang đăng nhập
                else
                {
                    DM_DON_VI dv = new DM_DON_VI();
                    dv.MA_DVI = ClientInformation.MaDonVi;
                    dv.TEN_GDICH = ClientInformation.TenDonVi;

                    listDonVi.Add(dv);
                }
                // Lấy dữ liệu đổ vào source với loại đối tượng tương ứng
                if (loaiDoiTuong.Equals(BusinessConstant.LoaiDoiTuong.NGUOI_SDUNG.layGiaTri()))
                {
                    dsNSD = qtht.layNSD(ClientInformation.LoaiNguoiSuDung);
                    foreach (var item in dsNSD)
                    {
                        DataRow r = dtDoiTuong.NewRow();
                        stt = stt + 1;
                        r[0] = item.ID;
                        r[1] = stt;
                        r[2] = item.MA_NSD;
                        r[3] = item.TEN_DAY_DU;
                        r[4] = layTenDonViTheoDanhSach(item.MA_DVI_QLY, listDonVi);
                        r[5] = item.MA_DVI_QLY;
                        dtDoiTuong.Rows.Add(r);
                    }
                }
                else if (loaiDoiTuong.Equals(BusinessConstant.LoaiDoiTuong.NHOM_NGUOI_SDUNG.layGiaTri()))
                {
                    dsNHNSD = qtht.layNhomNSD(ClientInformation.LoaiNguoiSuDung);
                    foreach (var item in dsNHNSD)
                    {
                        DataRow r = dtDoiTuong.NewRow();
                        stt = stt + 1;
                        r[0] = item.ID;
                        r[1] = stt;
                        r[2] = item.MA_NHNSD;
                        r[3] = item.TEN_NHNSD;
                        r[4] = layTenDonViTheoDanhSach(item.MA_DVI_QLY, listDonVi);
                        r[5] = item.MA_DVI_QLY;
                        dtDoiTuong.Rows.Add(r);
                    }
                }
                // đổ source lên lưới
                grDSDoiTuong.ItemsSource = dtDoiTuong;
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

        private void loadWidthColumnDoiTuong()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                for (int i = 0; i < grDSDoiTuong.Columns.Count; i++)
                {
                    if (i == 0)
                        grDSDoiTuong.Columns[i].IsVisible = false;
                    else if (i == 1)
                    {
                        grDSDoiTuong.Columns[i].Width = new Telerik.Windows.Controls.GridViewLength(40, Telerik.Windows.Controls.GridViewLengthUnitType.Pixel);
                        grDSDoiTuong.Columns[i].IsReadOnly = true;
                    }
                    else if (i == 2)
                    {
                        grDSDoiTuong.Columns[i].Width = new Telerik.Windows.Controls.GridViewLength(1, Telerik.Windows.Controls.GridViewLengthUnitType.Auto);
                        grDSDoiTuong.Columns[i].IsReadOnly = true;
                    }
                    else if (i == 3)
                    {
                        grDSDoiTuong.Columns[i].Width = new Telerik.Windows.Controls.GridViewLength(1, Telerik.Windows.Controls.GridViewLengthUnitType.Auto);
                        grDSDoiTuong.Columns[i].IsReadOnly = true;
                    }
                    else if (i == 4)
                    {
                        grDSDoiTuong.Columns[i].Width = new Telerik.Windows.Controls.GridViewLength(1, Telerik.Windows.Controls.GridViewLengthUnitType.Star);
                        grDSDoiTuong.Columns[i].IsReadOnly = true;
                    }
                    else if (i == 5)
                    {
                        grDSDoiTuong.Columns[i].IsVisible = false;
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

        private void cmbLoaiPhamVi_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BuildTreeDonVi();
            LoadDuLieuPhanQuyen(null);
        }

        private void cmbDoiTuong_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BuildGridDoiTuong();
            loadWidthColumnDoiTuong();
            BuildTreeDonVi();
            LoadDuLieuPhanQuyen(null);
        }

        private void grDSDoiTuong_Loaded(object sender, RoutedEventArgs e)
        {
            loadWidthColumnDoiTuong();
        }

        private void grDSDoiTuong_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            BuildTreeDonVi();
            LoadDuLieuPhanQuyen(null);
        }

        private void cmbLoaiTaiNguyen_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BuildTreeDonVi();
            LoadDuLieuPhanQuyen(null);
        }

        private void trvTreeDonVi_Checked(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            bool isInitiallyChecked = (e as RadTreeViewCheckEventArgs).IsUserInitiated;
        }

        #endregion

        #region Inner Class
        private class DonViNode
        {
            string _NODE_PARENT;
            string _NODE;
            string _NODE_NAME;
            string _TYPE;
            int _EXT;
            int _SUB_COUNT;

            public string NODE_PARENT
            {
                get { return _NODE_PARENT; }
                set { _NODE_PARENT = value; }
            }

            public string NODE
            {
                get { return _NODE; }
                set { _NODE = value; }
            }

            public string NODE_NAME
            {
                get { return _NODE_NAME; }
                set { _NODE_NAME = value; }
            }

            public string TYPE
            {
                get { return _TYPE; }
                set { _TYPE = value; }
            }

            public int EXT
            {
                get { return _EXT; }
                set { _EXT = value; }
            }

            public int SUB_COUNT
            {
                get { return _SUB_COUNT; }
                set { _SUB_COUNT = value; }
            }
        }

        private class PhamViDonViNode
        {
            int _ID_PVI;
            string _MA_PVI;
            string _PHAN_LOAI;

            public int ID_PVI
            {
                get { return _ID_PVI; }
                set { _ID_PVI = value; }
            }

            public string MA_PVI
            {
                get { return _MA_PVI; }
                set { _MA_PVI = value; }
            }

            public string PHAN_LOAI
            {
                get { return _PHAN_LOAI; }
                set { _PHAN_LOAI = value; }
            }
        }
        #endregion
    }
}
