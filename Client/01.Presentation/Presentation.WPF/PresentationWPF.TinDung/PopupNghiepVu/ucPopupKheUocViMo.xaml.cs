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
using Presentation.Process.PopupServiceRef;
using Utilities.Common;
using Telerik.Windows.Controls;

namespace PresentationWPF.TinDung
{
    /// <summary>
    /// Interaction logic for ucPopupKheUocViMo.xaml
    /// </summary>
    public partial class ucPopupKheUocViMo : UserControl
    {
        List<int> lstIDKUOC = new List<int>();
        List<DataRow> lstDataRow = new List<DataRow>();
        public delegate void LayListID(List<int> lst);
        public LayListID LayGiaTriListID;
        public delegate void LayListDataRow(List<DataRow> lst);
        public LayListDataRow LayGiaTriListDataRow;
        DataTable dtTreeDVi = new DataTable();
        DataTable dtTreeSP = new DataTable();
        DataTable dtDetail = new DataTable();
        static List<HeaderDto> lstHeader;
        static bool columnsWidthLoad = false;
        string kieuDuLieu = "";
        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        public ucPopupKheUocViMo(bool isTree, SimplePopupResponse simplePopupResponse, bool multiSelect = false)
        {
            InitializeComponent();
            trvDonVi.SelectionChanged += new SelectionChangedEventHandler(TreeViewCommon_SelectionChanged);
            trvSanPham.SelectionChanged += new SelectionChangedEventHandler(TreeViewCommon_SelectionChanged);
            grvKheUoc.Loaded += new RoutedEventHandler(grvKheUoc_Loaded);
            BuildDataForm(isTree, simplePopupResponse, multiSelect);
        }

        void grvKheUoc_Loaded(object sender, RoutedEventArgs e)
        {
            loadWidthColumn();
        }

        void TreeViewCommon_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BuildGrid();
            loadWidthColumn();
        }

        private void grvKheUoc_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            onCloseForm();
        }

        void onCloseForm()
        {
            if (LayGiaTriListID != null )
            {
                if (grvKheUoc.SelectedItems.Count > 0)
                {
                    foreach (DataRowView dr in grvKheUoc.SelectedItems)
                    {
                        lstIDKUOC.Add(Convert.ToInt32(dr["ID_KHE_UOC"]));
                    }
                    LayGiaTriListID(lstIDKUOC);
                }
                PresentationWPF.CustomControl.CommonFunction.CloseUserControl(this);
            }
            else if (LayGiaTriListDataRow != null)
            {
                if (grvKheUoc.SelectedItems.Count > 0)
                {
                    foreach (DataRowView dr in grvKheUoc.SelectedItems)
                    {
                        lstDataRow.Add(dr.Row);
                    }
                    LayGiaTriListDataRow(lstDataRow);
                }
                PresentationWPF.CustomControl.CommonFunction.CloseUserControl(this);
            }
        }

        void BuildDataForm(bool isTree, SimplePopupResponse simplePopupResponse, bool multiSelect = false)
        {
            if (simplePopupResponse.DataSetSource != null)
            {
                dtTreeDVi = simplePopupResponse.DataSetSource.Tables[0];
                dtTreeSP = simplePopupResponse.DataSetSource.Tables[1];
                dtDetail = simplePopupResponse.DataSetSource.Tables[2];
                string[] arrKieuDuLieu = null;
                if (!LObject.IsNullOrEmpty(simplePopupResponse.FormatData))
                    arrKieuDuLieu = simplePopupResponse.FormatData.Split('#');
                int idx = 0;
                foreach (DataColumn dtc in dtDetail.Columns)
                {
                    GridViewDataColumn grvDataColumn = new GridViewDataColumn();
                    if (!LObject.IsNullOrEmpty(simplePopupResponse.FormatData))
                    {
                        if (idx < arrKieuDuLieu.Length)
                        {
                            if (arrKieuDuLieu[idx].ToUpper().Contains("NUMBER"))
                            {
                                grvDataColumn.DataFormatString = "{0:N" + arrKieuDuLieu[idx].Substring(6) + "}";
                            }
                        }
                    }
                    grvDataColumn.DataMemberBinding = new System.Windows.Data.Binding(dtc.ColumnName);
                    grvKheUoc.Columns.Add(grvDataColumn);
                   
                    idx++;
                }
                lstHeader = simplePopupResponse.ListHeader.ToList();
                if (!isTree)
                {
                    dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
                    dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 1);
                    dispatcherTimer.Start();
                    
                }
                else
                {
                    if (dtTreeDVi != null && dtTreeDVi.Rows.Count > 0)
                    {
                        BuildTreeDVi("");
                    }
                    else
                    {
                        grdTree.RowDefinitions.ElementAt(0).Height = new GridLength(0);
                        grdTree.RowDefinitions.ElementAt(1).Height = new GridLength(0);
                    }
                    if (dtTreeSP != null && dtTreeSP.Rows.Count > 0)
                    {
                        BuildTreeSPham("");
                    }
                    else
                    {
                        grdTree.RowDefinitions.ElementAt(2).Height = new GridLength(0);
                        grdTree.RowDefinitions.ElementAt(1).Height = new GridLength(0);
                    }
                }
            }
        }
        void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            dispatcherTimer.Stop();
            grvKheUoc.ItemsSource = dtDetail.DefaultView;
            if (!LObject.IsNullOrEmpty(grvKheUoc.SelectedItems))
                grvKheUoc.SelectedItems.Clear();
            grdMain.ColumnDefinitions.ElementAt(0).Width = new GridLength(0);
            grdMain.ColumnDefinitions.ElementAt(2).Width = new GridLength((Application.Current.MainWindow.ActualWidth) - 20);
            lblTongSoBanGhi.Content = dtDetail.Rows.Count.ToString("#");
            UpdateLayout();
            loadWidthColumn();
        }
        protected void BuildTreeDVi(string parentNote, RadTreeViewItem node = null)
        {
            try
            {
                foreach (DataRow row in dtTreeDVi.Rows)
                {
                    if (row["NODE_PARENT"].ToString() == parentNote)
                    {
                        RadTreeViewItem subItem = new RadTreeViewItem();
                        subItem.Header = row["NODE_NAME"].ToString();
                        subItem.Tag = row["NODE"].ToString();
                        subItem.IsExpanded = false;
                        if (node == null)
                            trvDonVi.Items.Add(subItem);
                        else
                            node.Items.Add(subItem);
                        BuildTreeDVi(row["NODE"].ToString(), subItem);
                    }
                }
            }
            catch (Exception ex)
            { }
        }

        protected void BuildTreeSPham(string parentNote, RadTreeViewItem node = null)
        {
            try
            {
                foreach (DataRow row in dtTreeSP.Rows)
                {
                    if (row["NODE_PARENT"].ToString() == parentNote)
                    {
                        RadTreeViewItem subItem = new RadTreeViewItem();
                        subItem.Header = row["NODE_NAME"].ToString();
                        subItem.Tag = row["NODE"].ToString();
                        subItem.IsExpanded = false;
                        if (node == null)
                            trvSanPham.Items.Add(subItem);
                        else
                            node.Items.Add(subItem);
                        BuildTreeSPham(row["NODE"].ToString(), subItem);
                    }
                }
            }
            catch (Exception ex)
            { }
        }

        private void BuildGrid()
        {
            try
            {
                List<string> lstDonVi = new List<string>();
                RadTreeViewItem selectItemDonVi = (RadTreeViewItem)trvDonVi.SelectedItem;
                if (selectItemDonVi != null)
                {
                    lstDonVi.Add(selectItemDonVi.Tag.ToString());
                    getSelectChild(ref lstDonVi, selectItemDonVi);
                }
                List<string> lstSPham = new List<string>();
                RadTreeViewItem selectItemSPham = (RadTreeViewItem)trvSanPham.SelectedItem;
                if (selectItemSPham != null)
                {
                    lstSPham.Add(selectItemSPham.Tag.ToString());
                    getSelectChild(ref lstSPham, selectItemSPham);
                }
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
                if (dtTreeDVi.Rows.Count > 0 && dtTreeSP.Rows.Count > 0)
                {
                    if (lstDonVi.Count > 0)
                    {
                        int stt = 0;
                        foreach (DataRow row in dtDetail.Rows)
                        {
                            if (lstDonVi.Contains(row["KEY_DVI_QLY"].ToString()))
                            {
                                if (lstSPham.Contains(row["MA_SAN_PHAM"].ToString()))
                                {
                                    stt = stt + 1;
                                    row[0] = stt.ToString();
                                    grdData.ImportRow(row);
                                }
                            }
                        }
                    }
                }
                else if (dtTreeDVi.Rows.Count > 0)
                {
                    if (lstDonVi.Count > 0)
                    {
                        int stt = 0;
                        foreach (DataRow row in dtDetail.Rows)
                        {
                            if (lstDonVi.Contains(row["KEY_DVI_QLY"].ToString()))
                            {
                                stt = stt + 1;
                                row[0] = stt.ToString();
                                grdData.ImportRow(row);
                            }
                        }
                    }
                }
                else
                {
                    if (lstSPham.Count > 0)
                    {
                        int stt = 0;
                        foreach (DataRow row in dtDetail.Rows)
                        {
                            if (lstSPham.Contains(row["MA_SAN_PHAM"].ToString()))
                            {
                                stt = stt + 1;
                                row[0] = stt.ToString();
                                grdData.ImportRow(row);
                            }
                        }
                    }
                }
                grvKheUoc.ItemsSource = grdData.DefaultView;
                grvKheUoc.Rebind();
                if (!LObject.IsNullOrEmpty(grvKheUoc.SelectedItems))
                    grvKheUoc.SelectedItems.Clear();
                lblTongSoBanGhi.Content = dtDetail.Rows.Count.ToString("#");
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

        private void loadWidthColumn()
        {
            if (grvKheUoc.Columns.Count >= lstHeader.Count)
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
                        grvKheUoc.Columns[idx].Width = new Telerik.Windows.Controls.GridViewLength(width, unit);
                        grvKheUoc.Columns[idx].Header = LLanguage.SearchResourceByKey(item.LanguageKey);
                    }
                    else
                        grvKheUoc.Columns[idx].IsVisible = false;
                    idx = idx + 1;
                }
                for (int i = idx; i < grvKheUoc.Columns.Count;i++)
                {
                    grvKheUoc.Columns[i].IsVisible = false;
                }
                    columnsWidthLoad = true;
            }
        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            onCloseForm();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            lstIDKUOC = new List<int>();
            onCloseForm();
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
                PresentationWPF.CustomControl.CommonFunction.QuickSearchInGrid(grvKheUoc, txtQuickSearch.Text);
                loadWidthColumn();
            }
        }
    }
}
