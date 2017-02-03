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
using Presentation.Process;
using Presentation.Process.Common;
using Utilities.Common;
using Presentation.Process.HuyDongVonServiceRef;
using PresentationWPF.CustomControl;
using Telerik.Windows.Controls;

namespace PresentationWPF.TinDung.PopupNghiepVu
{
    /// <summary>
    /// Interaction logic for ucPopupSoTGui.xaml
    /// </summary>
    public partial class ucPopupSoTGui : UserControl
    {
        #region Khai báo

        private string sSanPham = "";        
        private string sIDCum = "";

        DataTable dtTreeSanPham = new DataTable();
        DataTable dtTreeDonVi = new DataTable();        

        DataTable dtSoTGui = new DataTable();

        

        private DatabaseConstant.Function function;
        public DatabaseConstant.Function Function
        {
            get { return function; }
            set { function = value; }
        }

        public bool isMultiSelect = true;

        // khai báo 1 hàm delegate
        public delegate void LayDuLieu(List<DataRow> lst);
        // khai báo 1 kiểu hàm delegate
        public LayDuLieu DuLieuTraVe;
        #endregion

        #region Khởi tạo
        public ucPopupSoTGui()
        {
            InitializeComponent();            

            Mouse.OverrideCursor = Cursors.Arrow;
        }


        #endregion

        #region Xu ly giao dien
        /// <summary>
        /// Sự kiện LoadForm
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            txtTimKiemNhanh.Focus();

            if (function == DatabaseConstant.Function.HDV_GUI_THEM_TIEN_THEO_SO
                || function == DatabaseConstant.Function.HDV_RUT_BOT_GOC
                || function == DatabaseConstant.Function.HDV_TRA_LAI
                || function == DatabaseConstant.Function.HDV_TAT_TOAN)
            {
                isMultiSelect = false;
            }
            else
            {
                isMultiSelect = true;
            }

            LoadTreeview();

        }

        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            PresentationWPF.CustomControl.CommonFunction.CloseUserControl(e, this);
            PresentationWPF.CustomControl.CommonFunction.SelectNextControl(e);
        }

        /// <summary>
        /// Load dữ liệu lên Treeview
        /// </summary>
        private void LoadTreeview()
        {
            try
            {
                HuyDongVonProcess huyDongVonProcess = new HuyDongVonProcess();
                string sIdDonVi = ClientInformation.IdDonVi.ToString();
                string sMaDonVi = ClientInformation.MaDonVi;
                DataSet dsTreeSanPham;

                dtTreeDonVi = huyDongVonProcess.GetTreeDonVi(sIdDonVi).Tables[0];

                #region Tree Sản phẩm
                if (function == DatabaseConstant.Function.HDV_RUT_BOT_GOC || function == DatabaseConstant.Function.HDV_RUT_GOC_THEO_DANH_SACH)
                {
                    dsTreeSanPham = huyDongVonProcess.GetTreeSanPhamRutGoc(sMaDonVi);
                }
                else if (function == DatabaseConstant.Function.HDV_GUI_THEM_TIEN_THEO_SO || function == DatabaseConstant.Function.HDV_GUI_THEM_TIEN_THEO_DANH_SACH)
                {
                    dsTreeSanPham = huyDongVonProcess.GetTreeSanPhamGuiThem(sMaDonVi);
                }
                else if (function == DatabaseConstant.Function.HDV_TRA_LAI || function == DatabaseConstant.Function.HDV_TRA_LAI_THEO_DANH_SACH)
                {
                    dsTreeSanPham = huyDongVonProcess.GetTreeSanPhamTraLai(sMaDonVi);
                }
                else if (function == DatabaseConstant.Function.HDV_DU_CHI)
                {
                    dsTreeSanPham = huyDongVonProcess.GetTreeSanPhamDuChi(sMaDonVi);
                }
                else if (function == DatabaseConstant.Function.HDV_PHAN_BO)
                {
                    dsTreeSanPham = huyDongVonProcess.GetTreeSanPhamPhanBo(sMaDonVi);
                }
                else if (function == DatabaseConstant.Function.HDV_DIEU_CHINH_LS)
                {
                    dsTreeSanPham = huyDongVonProcess.GetTreeSanPhamThayDoiLS(sMaDonVi);
                }
                else
                {
                    dsTreeSanPham = huyDongVonProcess.GetTreeSanPham(sMaDonVi);
                }


                if (dsTreeSanPham != null && dsTreeSanPham.Tables[0].Rows.Count > 0)
                {
                    dtTreeSanPham = dsTreeSanPham.Tables[0];
                }
                #endregion

                itemSanPham.Items.Clear();
                //Cấu trúc của Tag: GiaTri#Level#LoaiTree ( VD: MaSP001#2#SAN_PHAM)
                itemSanPham.Tag = "0#0#SAN_PHAM";
                itemSanPham.IsExpanded = false;
                BuildTree(itemSanPham, dtTreeSanPham, "SAN_PHAM");

                itemDonVi.Items.Clear();
                //Cấu trúc của Tag: GiaTri#Level#LoaiTree ( VD: MaSP001#2#SAN_PHAM)
                itemDonVi.Tag = "0#0#DON_VI";
                itemDonVi.IsExpanded = false;
                BuildTree(itemDonVi, dtTreeDonVi, "DON_VI");

            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        protected void BuildTree(RadTreeViewItem item, DataTable dt, string sLoaiTree)
        {
            try
            {
                //Cấu trúc của Tag: GiaTri#Level#LoaiTree  ( VD:  MaSP001#2#SAN_PHAM hoặc CUM001#3#DON_VI)
                string sTag = item.Tag.ToString();
                int i1 = sTag.IndexOf("#");
                int i2 = sTag.LastIndexOf("#");

                string sValue = sTag.Substring(0, i1);
                int iLevel = Convert.ToInt32(sTag.Substring(i1 + 1, i2 - i1 - 1));

                foreach (DataRow row in dt.Rows)
                {
                    if (iLevel == Convert.ToInt32(row["LEVEL"]) - 1)
                    {
                        if (row["NODE_PARENT"].ToString() == sValue)
                        {
                            RadTreeViewItem subItem = new RadTreeViewItem();
                            subItem.Header = row["NODE_NAME"].ToString();
                            subItem.Tag = row["NODE"].ToString() + "#" + row["LEVEL"].ToString() + "#" + sLoaiTree;
                            subItem.IsExpanded = false;
                            item.Items.Add(subItem);
                            BuildTree(subItem, dt, sLoaiTree);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
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
                PresentationWPF.CustomControl.CommonFunction.QuickSearchInGrid(grSoTienGuiDS, txtTimKiemNhanh.Text);
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


        /// <summary>
        /// Sự kiện thay đổi số bản ghi trên một page hiển thị
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nudPageSize_ValueChanged(object sender, RadRangeBaseValueChangedEventArgs e)
        {
            if (grSoTienGuiDS != null && grSoTienGuiDS.ItemsSource != null)
            {
                if (dtSoTGui != null)
                {
                    radpage.PageSize = (int)nudPageSize.Value;
                    grSoTienGuiDS.ItemsSource = dtSoTGui;
                }
            }
        }

        private void grSoTienGuiDS_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            tlbSelect_Click(null, null);
        }

        private void tlbSelect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (grSoTienGuiDS.SelectedItems.Count == 0)
                {
                    LMessage.ShowMessage("M.DungChung.ChuaChonBanGhi", LMessage.MessageBoxType.Warning);
                    return;
                }
                if (grSoTienGuiDS.SelectedItems.Count > 0)
                {
                    if (isMultiSelect == false && grSoTienGuiDS.SelectedItems.Count > 1)
                    {
                        LMessage.ShowMessage("M.DungChung.KhongDuocChonNhieu", LMessage.MessageBoxType.Warning);
                        return;
                    }

                    if (DuLieuTraVe != null)
                    {
                        List<DataRow> list = new List<DataRow>();
                        foreach (DataRow dr in grSoTienGuiDS.SelectedItems)
                        {
                            list.Add(dr);
                        }
                        DuLieuTraVe(list);
                    }
                }

                if (this.Parent is Window)
                    ((Window)this.Parent).Close();
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        private void tlbSearch_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                sSanPham = "";
                sIDCum = "";
                foreach (RadTreeViewItem item in tvwTree.CheckedItems)
                {

                    ///Cấu trúc của Tag: GiaTri#Level#LoaiTree  ( VD:  MaSP001#2#SAN_PHAM hoặc CUM001#3#DON_VI)
                    string sTag = item.Tag.ToString();
                    int i1 = sTag.IndexOf("#");
                    int i2 = sTag.LastIndexOf("#");

                    string sValue = sTag.Substring(0, i1);
                    int iLevel = Convert.ToInt32(sTag.Substring(i1 + 1, i2 - i1 - 1));
                    string sLoaiTree = sTag.Substring(i2 + 1);

                    if (sLoaiTree.Equals("SAN_PHAM"))
                    {
                        if (iLevel == 2)
                        {
                            sSanPham = sSanPham + "''" + sValue + "'',";
                        }
                    }

                    if (sLoaiTree.Equals("DON_VI"))
                    {
                        if (iLevel == 3)
                        {
                            sIDCum = sIDCum + "''" + sValue + "'',";
                        }
                    }

                }

                if (sSanPham.Length > 0)
                    sSanPham = sSanPham.Substring(0, sSanPham.Length - 1);

                if (sIDCum.Length > 0)
                    sIDCum = sIDCum.Substring(0, sIDCum.Length - 1);


                //if (itemSanPham.CheckState == System.Windows.Automation.ToggleState.On)
                //{
                //    sSanPham = "%";
                //}

                if (itemDonVi.CheckState == System.Windows.Automation.ToggleState.On)
                {
                    sIDCum = "%";
                }

                if (sSanPham.Equals(""))
                {
                    sSanPham = "''''";
                }

                if (sIDCum.Equals(""))
                {
                    sIDCum = "''''";
                }
                LoadGrid();

            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private void LoadGrid()
        {
            try
            {
                HuyDongVonProcess huyDongVonProcess = new HuyDongVonProcess();
                DataSet ds = null;
                if (DatabaseConstant.Function.TDVM_TAM_UNG == function)
                {
                    ds = huyDongVonProcess.GetDanhSachSoRutGoc(ClientInformation.MaDonViGiaoDich, sSanPham, sIDCum, ClientInformation.NgayLamViecHienTai);
                }

                if (ds != null)
                {
                    dtSoTGui = ds.Tables[0];
                    grSoTienGuiDS.ItemsSource = dtSoTGui;

                    int soSoTgui = 0;
                    decimal tongSoDu = 0;
                    decimal soDuBinhQuan = 0;
                    if (dtSoTGui.Rows.Count > 0)
                    {
                        soSoTgui = dtSoTGui.Rows.Count;
                        for (int i = 0; i < dtSoTGui.Rows.Count; i++)
                        {
                            tongSoDu += Convert.ToDecimal(dtSoTGui.Rows[i]["SO_DU"]);
                        }
                        soDuBinhQuan = tongSoDu / soSoTgui;
                    }

                    lblSumSoSo.Content = String.Format("{0:#,#}", soSoTgui);
                    lblSumSoDu.Content = String.Format("{0:#,#}", tongSoDu);
                    lblSoDuBQ.Content = String.Format("{0:#,#}", soDuBinhQuan);
                }
                else
                {
                    dtSoTGui.Rows.Clear();
                    grSoTienGuiDS.ItemsSource = dtSoTGui;
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        private void tlbClose_Click(object sender, RoutedEventArgs e)
        {
            CustomControl.CommonFunction.CloseUserControl(this);
        }
        #endregion
        

        
    }
}
