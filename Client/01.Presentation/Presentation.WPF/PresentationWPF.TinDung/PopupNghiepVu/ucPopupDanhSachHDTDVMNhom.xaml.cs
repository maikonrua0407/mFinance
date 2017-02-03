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
using Presentation.Process.PopupServiceRef;
using Presentation.Process;
using Presentation.Process.Common;
using System.Data;
using Utilities.Common;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using PresentationWPF.CustomControl;

namespace PresentationWPF.TinDung.PopupNghiepVu
{
    /// <summary>
    /// Interaction logic for ucPopup.xaml
    /// </summary>
    public partial class ucPopupDanhSachHDTDVMNhom : UserControl
    {
        public int ID;
        public string Ma;
        public string Ten;
        DataTable dtMaster = new DataTable();
        DataTable dtDetail = new DataTable();
        DataTable dtRoot = new DataTable();
        static List<HeaderDto> lstHeader;
        static bool columnsWidthLoad = false;
        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        // khai báo 1 hàm delegate
        public delegate void LayDuLieu(List<DataRow> lst);
        // khai báo 1 kiểu hàm delegate
        public LayDuLieu DuLieuTraVe;

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            if (raddgrDanhSach.SelectedItems.Count > 0)
            {
                try
                {
                    if (DuLieuTraVe != null)
                    {
                        List<DataRow> list = new List<DataRow>();
                        foreach (DataRowView drRaw in raddgrDanhSach.SelectedItems)
                        {
                            foreach (DataRow dr in dtDetail.Select("SO_GDICH='" + drRaw["SO_GDICH"].ToString() + "'"))
                            {
                                list.Add(dr);
                            }
                            
                        }
                        DuLieuTraVe(list);
                    }
                    if (this.Parent is Window)
                        ((Window)this.Parent).Close();
                }
                catch (Exception ex)
                {
                    LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                }
            }
        }

        private void raddgrDanhSach_LoadingRowDetails(object sender, Telerik.Windows.Controls.GridView.GridViewRowDetailsEventArgs e)
        {
            DataRowView dtrv = e.Row.Item as DataRowView;
            DataTable dt = dtDetail.Clone();
            foreach (DataRow dr in dtDetail.Select("SO_HDTDVM ='" + dtrv["SO_HDTDVM"].ToString() + "'"))
            {
                dt.Rows.Add(dr.ItemArray);
            }
            RadGridView radGridViewDetail = e.DetailsElement.FindName("radDetailHDTD") as RadGridView;
            radGridViewDetail.ItemsSource = dt.DefaultView;
            radGridViewDetail.Rebind();
        }

        public ucPopupDanhSachHDTDVMNhom(bool isTree, SimplePopupResponse simplePopupResponse, bool multiSelect = false)
        {
            InitializeComponent();
            raddgrDanhSach.LoadingRowDetails += raddgrDanhSach_LoadingRowDetails;
            try
            {
                if (simplePopupResponse.DataSetSource != null)
                {
                    lstHeader = simplePopupResponse.ListHeader.ToList();
                    dtDetail = simplePopupResponse.DataSetSource.Tables[0];
                    if (simplePopupResponse.DataSetSource.Tables.Count > 1)
                    {
                        dtMaster = simplePopupResponse.DataSetSource.Tables[0];
                        dtDetail = simplePopupResponse.DataSetSource.Tables[1];
                    }
                    if (simplePopupResponse.DataSetSource.Tables.Count > 2)
                    {
                        dtMaster = simplePopupResponse.DataSetSource.Tables[0];
                        dtDetail = simplePopupResponse.DataSetSource.Tables[1];
                        dtRoot = simplePopupResponse.DataSetSource.Tables[2];
                    }
                    BuildGrid();
                    raddgrDanhSach.Columns[1].IsVisible = multiSelect;
                    gbLoc.Visibility = isTree ? Visibility.Visible : Visibility.Collapsed;

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
                        subItem.Tag = row["NODE"].ToString();
                        //subItem.IsExpanded = true;
                        if (node == null)
                            trvTree.Items.Add(subItem);
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
                        subItem.Tag = row["NODE"].ToString();
                        subItem.IsExpanded = false;
                        if (node == null)
                            trvTree.Items.Add(subItem);
                        else
                            node.Items.Add(subItem);
                        BuildTree(row["NODE"].ToString(), subItem);
                    }
                }
            }
            catch (Exception ex)
            { }
        }
        private void getSelectChild(ref List<string> lst, RadTreeViewItem selectItem)
        {
            if (selectItem.Items.Count > 0)
            {
                foreach (RadTreeViewItem item in selectItem.Items)
                {
                    //if (item.Items.Count == 0)
                    //{
                        lst.Add(item.Tag.ToString());
                        getSelectChild(ref lst, item);
                    //}
                }
            }
        }
        private void BuildGrid()
        {
            try
            {
                DataTable tableRaw = dtDetail.DefaultView.ToTable(true, "SO_HDTDVM");
                DataTable serverDataTable = dtDetail.Clone();
                foreach (DataRow dtr in tableRaw.Rows)
                {
                    DataRow dtrone = dtDetail.Select("SO_HDTDVM='" + dtr["SO_HDTDVM"] + "'").FirstOrDefault();
                    if (!dtrone.IsNullOrEmpty())
                    {
                        serverDataTable.Rows.Add(dtrone.ItemArray);
                    }

                }
                raddgrDanhSach.ItemsSource = serverDataTable.DefaultView;
                lblTongSoBanGhi.Content = serverDataTable.Rows.Count;
            }
            catch (Exception ex)
            { }
        }

        private void BuildGridNotTree()
        {
            DataTable grdData = new DataTable();
            foreach (DataColumn col in dtDetail.Columns)
            {
                Type type = typeof(string);
                if (col.DataType == typeof(decimal))
                    type = typeof(decimal);
                else if (col.DataType == typeof(int))
                    type = typeof(int);
                else if (col.DataType == typeof(float))
                    type = typeof(float);
                grdData.Columns.Add(col.ColumnName, type);
            }
            int stt = 0;
            foreach (DataRow row in dtDetail.Rows)
            {
                stt = stt + 1;
                row[0] = stt.ToString();
                grdData.ImportRow(row);
            }

            // Xử lý ngôn ngữ
            foreach (DataColumn dc in grdData.Columns)
            {
                if(dc.DataType == typeof(string))
                {
                    foreach (DataRow dr in grdData.Rows)
                    { 
                        if (dr[dc.ColumnName] != DBNull.Value)
                        {
                            dr[dc.ColumnName] = LLanguage.SearchResourceByKey(dr[dc.ColumnName].ToString());
                        }
                    }
                }
            }
            
            raddgrDanhSach.ItemsSource = grdData.DefaultView;
            lblTongSoBanGhi.Content = grdData.Rows.Count;
            gbLoc.Visibility = Visibility.Collapsed;
        }
        private void raddgrDanhSach_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement originalSender = e.OriginalSource as FrameworkElement;
            if (originalSender != null)
            {
                if (raddgrDanhSach.SelectedItems.Count > 0)
                {
                    try
                    {
                        if (DuLieuTraVe != null)
                        {
                            List<DataRow> list = new List<DataRow>();
                            foreach (DataRowView drRaw in raddgrDanhSach.SelectedItems)
                            {
                                foreach (DataRow dr in dtDetail.Select("SO_GDICH='" + drRaw["SO_GDICH"].ToString() + "'"))
                                {
                                    list.Add(dr);
                                }

                            }
                            DuLieuTraVe(list);
                        }
                        if (this.Parent is Window)
                            ((Window)this.Parent).Close();
                    }
                    catch (Exception ex)
                    {
                        LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                    }
                }
            }
        }

        private void raddgrDanhSach_Loaded(object sender, RoutedEventArgs e)
        {
            
        }
        private void loadWidthColumn()
        {
            if (raddgrDanhSach.Columns.Count >= lstHeader.Count)
            {
                int idx = 1;
                foreach (HeaderDto item in lstHeader)
                {
                    double width = (double)item.WidthValue;
                    if (width > 0)
                    {
                        Telerik.Windows.Controls.GridViewLengthUnitType unit = new Telerik.Windows.Controls.GridViewLengthUnitType();
                        if (item.WidthUnit.Equals(ApplicationConstant.layGiaTri(ApplicationConstant.UnitWidth.Pixel)))
                            unit = Telerik.Windows.Controls.GridViewLengthUnitType.Pixel;
                        else if (item.WidthUnit.Equals(ApplicationConstant.layGiaTri(ApplicationConstant.UnitWidth.Star)))
                            unit = Telerik.Windows.Controls.GridViewLengthUnitType.Star;
                        else if (item.WidthUnit.Equals(ApplicationConstant.layGiaTri(ApplicationConstant.UnitWidth.Percent)))
                            unit = Telerik.Windows.Controls.GridViewLengthUnitType.Auto;
                        raddgrDanhSach.Columns[idx].Width = new Telerik.Windows.Controls.GridViewLength(width, unit);
                        raddgrDanhSach.Columns[idx].Header = LLanguage.SearchResourceByKey(item.LanguageKey);
                    }
                    else
                        raddgrDanhSach.Columns[idx].IsVisible = false;
                    idx = idx + 1;
                }
                for (int i = idx; i < raddgrDanhSach.Columns.Count;i++ )
                {
                    raddgrDanhSach.Columns[i].IsVisible = false;
                }
                columnsWidthLoad = true;
            }
        }

        private void btnLoadGrid_Click(object sender, RoutedEventArgs e)
        {
            BuildGrid(); loadWidthColumn();
        }

        private void trvTree_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BuildGrid(); loadWidthColumn();
        }

        private void txtQuickSearch_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtQuickSearch.Text))
            {
                txtQuickSearch.Text = LLanguage.SearchResourceByKey("U.DungChung.TimKiemNhanh");
            }
        }

        private void txtQuickSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtQuickSearch.Text == LLanguage.SearchResourceByKey("U.DungChung.TimKiemNhanh"))
            {
                txtQuickSearch.Text = "";
            }
        }

        private void txtQuickSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (txtQuickSearch.Text == LLanguage.SearchResourceByKey("U.DungChung.TimKiemNhanh"))
                {
                    return;
                }
                PresentationWPF.CustomControl.CommonFunction.QuickSearchInGrid(raddgrDanhSach, txtQuickSearch.Text);
                loadWidthColumn();
            }
        }
    }
}
