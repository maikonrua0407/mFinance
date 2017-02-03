using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Telerik.Windows.Controls;
using Utilities.Common;
using Presentation.Process.Common;
using System.Data;
using Microsoft.Win32;
using System.IO;
using System.Reflection;
using Microsoft.Windows.Controls.Ribbon;
using Presentation.Process.ZAMainAppServiceRef;
using System.Text.RegularExpressions;


namespace PresentationWPF.CustomControl
{
    public static class CommonFunction
    {
        #region Escape Close form
        /// <summary>
        /// Close usercontrol khi nhấn Escape
        /// </summary>
        /// <param name="e">Key được nhấn</param>
        /// <param name="uc">Usercontrol</param>
        public static void CloseUserControl(KeyEventArgs e, UserControl uc)
        {
            //if (e.Key == Key.Escape)
            //{
            //    DependencyObject parentTabControl = FindVisualParent<TabControl>(uc);
            //    DependencyObject parentWindow = FindVisualParent<Window>(uc);
            //    if (parentTabControl != null)
            //    {
            //        TabControl tb = parentTabControl as TabControl;
            //        tb.Items.Remove(tb.SelectedItem);
            //    }
            //    else
            //    {
            //        if (parentWindow != null)
            //        {
            //            (parentWindow as Window).Close();
            //        }
            //    }
            //}
        }

        /// <summary>
        /// Xử lý sự kiện đóng form
        /// </summary>
        /// <param name="uc"></param>
        public static void CloseUserControl(UserControl uc)
        {
            DependencyObject parentTabControl = FindVisualParent<TabControl>(uc);
            DependencyObject parentWindow = FindVisualParent<Window>(uc);
            if (parentTabControl != null)
            {
                string askBeforeClose = "0";
                try
                {
                    string thuocTinh = uc.Uid;
                    askBeforeClose = thuocTinh.SplitByDelimiter("#")[4];
                }
                catch(Exception ex)
                {
                    LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, "CloseUserControl: " + ex);
                    askBeforeClose = "0";
                }

                if (askBeforeClose.Equals("1"))
                {
                    MessageBoxResult retMessage = LMessage.ShowMessage("M.DungChung.HoiDongChucNang", LMessage.MessageBoxType.Question);
                    if (retMessage == MessageBoxResult.Yes)
                    {
                        TabControl tb = parentTabControl as TabControl;
                        tb.Items.Remove(tb.SelectedItem);
                    }
                }
                else
                {
                    TabControl tb = parentTabControl as TabControl;
                    tb.Items.Remove(tb.SelectedItem);
                }
            }
            else
            {
                if (parentWindow != null)
                {
                    (parentWindow as Window).Close();
                }
            }
        }

        public static void SetWindowTitle(UserControl uc,DatabaseConstant.Function func)
        {
            DependencyObject parentWindow = CommonFunction.FindVisualParent<Window>(uc);
            if (parentWindow != null)
            {
                ((Window)parentWindow).Title = DatabaseConstant.layNgonNguTieuDeForm(func);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sender"></param>
        /// <returns></returns>
        public static T FindVisualParent<T>(DependencyObject sender) where T : DependencyObject
        {
            if (sender == null)
            {
                return (null);
            }
            else if (VisualTreeHelper.GetParent(sender) is T)
            {
                return (VisualTreeHelper.GetParent(sender) as T);
            }
            else
            {
                DependencyObject parent = VisualTreeHelper.GetParent(sender);
                return (FindVisualParent<T>(parent));
            }
        }

        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }
#endregion

        #region Help
        public static void ShowHelp(UserControl uc)
        {
            string indexTopic = "999999";
            //string helpPath = ClientInformation.WorkingDir + "help" + "\\" + "mFinance.chm";

            DependencyObject parentTabControl = FindVisualParent<TabControl>(uc);
            DependencyObject parentWindow = FindVisualParent<Window>(uc);
            try
            {
                if (parentTabControl != null)
                {
                    indexTopic = ((parentTabControl as TabControl).SelectedItem as CloseableTabItem).Uid.ToString();
                }
                else
                {
                    indexTopic = (parentWindow as Window).Uid.ToString();
                }
            }
            catch (Exception ex)
            {
                indexTopic = "999999";
                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, ex);
            }
            ChucNangDto chucNang = ClientInformation.ListChucNang.FirstOrDefault(f=>f.IDChucNang.Equals(indexTopic.StringToInt32()));
            if (!chucNang.IsNullOrEmpty())
            {
                WindowHelp windowsHelp = new WindowHelp(chucNang.MenuHelp, chucNang.TieuDe);
                windowsHelp.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                windowsHelp.ShowDialog();
            }
        }
        #endregion

        #region Quick search grid
        public static void QuickSearchInGrid(RadGridView grid, string strSearch)
        {
            if (1==1 || !LString.IsNullOrEmptyOrSpace(strSearch))
            {
                Mouse.OverrideCursor = Cursors.Wait;
                try
                {
                    if (grid != null)
                    {
                        //Tao cau lenh tim kiem
                        string rowFilter = "";
                        for (int i = 0; i < grid.Columns.Count; i++)
                        {
                            if (grid.Columns[i].IsVisible)
                            {
                                if (!LString.IsNullOrEmptyOrSpace(grid.Columns[i].UniqueName) && grid.Columns[i].UniqueName != "STT")
                                {
                                    rowFilter += "convert([" + grid.Columns[i].UniqueName + "],System.String) like '%" + strSearch + "%' or ";
                                }
                            }
                        }

                        if (rowFilter.Length <= 3)
                        {
                            Mouse.OverrideCursor = Cursors.Arrow;
                            return;
                        }

                        rowFilter = rowFilter.Substring(0, rowFilter.Length - 3);

                        //Thuc hien tim kiem
                        object source = null;
                        if (grid.DataContext != null)
                        {
                            source = grid.DataContext;
                        }
                        else if (grid.ItemsSource != null)
                        {
                            source = grid.ItemsSource;
                        }
                        if (source != null)
                        {
                            DataView dataView = new DataView();
                            if (source is DataView)
                            {
                                dataView = (DataView)source;

                            }
                            else if (source is DataTable)
                            {
                                dataView = ((DataTable)source).DefaultView;
                            }

                            if (dataView != null)
                            {
                                //DataSet ds = dataView.DataViewManager.DataSet;

                                //dataView.DataViewManager.DataViewSettings[dataView.DataViewManager.DataSet.Tables[0]].RowFilter = rowFilter;

                                //DataView dataResult = new DataView();
                                //dataResult = dataView.DataViewManager.CreateDataView(dataView.DataViewManager.DataSet.Tables[0]); ;
                                dataView.RowFilter = rowFilter;

                                //Gan ket qua vao grid
                                if (grid.DataContext != null)
                                {
                                    if (source is DataView)
                                    {
                                        grid.DataContext = dataView;
                                    }
                                    else
                                    {
                                        grid.DataContext = dataView.ToTable();
                                    }
                                }
                                else if (grid.ItemsSource != null)
                                {
                                    if (source is DataView)
                                    {
                                        grid.ItemsSource = dataView;
                                    }
                                    else
                                    {
                                        grid.ItemsSource = dataView.ToTable();
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LMessage.ShowMessage("M.DanhMuc.ucDonViDS.LoiTimKiemNhanh", LMessage.MessageBoxType.Error);
                    LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                }
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }
#endregion

        #region Export to excel
        /// <summary>
        /// Export dữ liệu trong grid ra file excel
        /// </summary>
        /// <param name="grid">Grid truyền vào</param>
        public static void ExportGridToExcel(RadGridView grid)
        {
            ExportFormat format = ExportFormat.ExcelML;
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.DefaultExt = "xls";
            dialog.Filter = String.Format("{1} files (*.{0})|*.{0}|All files (*.*)|*.*", "xls", format);
            dialog.FilterIndex = 1;
            
            if (dialog.ShowDialog() == true)
            {
                try
                {
                    using (Stream stream = dialog.OpenFile())
                    {
                        GridViewExportOptions exportOptions = new GridViewExportOptions();
                        exportOptions.Format = format;
                        exportOptions.ShowColumnFooters = true;
                        exportOptions.ShowColumnHeaders = true;
                        exportOptions.ShowGroupFooters = true;

                        grid.Export(stream, exportOptions);
                    }
                    LMessage.ShowMessage("M.CustomControl.CommonFunction.ExportSuccess", LMessage.MessageBoxType.Information);
                }
                catch (Exception ex)
                {
                    LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                    LMessage.ShowMessage("M.CustomControl.CommonFunction.ExportFail", LMessage.MessageBoxType.Error);
                }
            }
        }

        /// <summary>
        /// Export dữ liệu ra file excel theo câu lệnh SQL truyền vào
        /// </summary>
        /// <param name="sSQL">câu lệnh SQL lấy dữ liệu</param>
        //public static void ExportGridToExcel(string maTruyVan)
        //{
        //    Presentation.Process.TruyVanProcess bus = new Presentation.Process.TruyVanProcess();
        //    try
        //    {
        //        DataSet ds = new DataSet();
        //        ds = bus.TruyVan(maTruyVan);

        //        SaveFileDialog dialog = new SaveFileDialog();
        //        dialog.DefaultExt = "xls";
        //        dialog.Filter = String.Format("{1} files (*.{0})|*.{0}|All files (*.*)|*.*", "xls", "Excel");
        //        dialog.FilterIndex = 1;
        //        if (dialog.ShowDialog() == true)
        //        {
        //            Excel.Application oXL;
        //            Excel.Workbook oWB;
        //            Excel.Worksheet oSheet;
        //            //Excel.Range oRange;

        //            // Start Excel and get Application object. 
        //            oXL = new Excel.Application();

        //            // Set some properties 
        //            oXL.Visible = false;
        //            oXL.DisplayAlerts = false;

        //            // Get a new workbook. 
        //            oWB = oXL.Workbooks.Add(Missing.Value);

        //            // Get the active sheet 
        //            oSheet = (Excel.Worksheet)oWB.ActiveSheet;
        //            oSheet.Name = "Sheet1";

        //            // Process the DataTable 
        //            // BE SURE TO CHANGE THIS LINE TO USE *YOUR* DATATABLE 
        //            DataTable dt = ds.Tables[0];

        //            int rowCount = 1;
        //            foreach (DataRow dr in dt.Rows)
        //            {
        //                rowCount += 1;
        //                for (int i = 1; i < dt.Columns.Count + 1; i++)
        //                {
        //                    // Add the header the first time through 
        //                    if (rowCount == 2)
        //                    {
        //                        oSheet.Cells[1, i] = dt.Columns[i - 1].ColumnName;
        //                    }
        //                    oSheet.Cells[rowCount, i] = dr[i - 1].ToString();
        //                }
        //            }

        //            // Resize the columns 

        //            //oRange = oSheet.get_Range(oSheet.Cells[1, 1],oSheet.Cells[rowCount, dt.Columns.Count]);
        //            //oRange.EntireColumn.AutoFit();

        //            // Save the sheet and close 
        //            oSheet = null;
        //            //oRange = null;
        //            oWB.SaveAs(dialog.FileName, Excel.XlFileFormat.xlWorkbookNormal,
        //                Missing.Value, Missing.Value, Missing.Value, Missing.Value,
        //                Excel.XlSaveAsAccessMode.xlExclusive,
        //                Missing.Value, Missing.Value, Missing.Value,
        //                Missing.Value, Missing.Value);
        //            oWB.Close(Missing.Value, Missing.Value, Missing.Value);
        //            oWB = null;
        //            oXL.Quit();

        //            // Clean up 
        //            // NOTE: When in release mode, this does the trick 
        //            GC.WaitForPendingFinalizers();
        //            GC.Collect();
        //            GC.WaitForPendingFinalizers();
        //            GC.Collect();
        //            LMessage.ShowMessage("M.CustomControl.CommonFunction.ExportSuccess", LMessage.MessageBoxType.Information);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
        //        LMessage.ShowMessage("M.CustomControl.CommonFunction.ExportFail", LMessage.MessageBoxType.Error);
        //    }
        //}
#endregion

        #region Select next control

        public static void SelectNextControl(KeyEventArgs e)
        {
    if ((e.Key == Key.Tab && (Keyboard.Modifiers == ModifierKeys.None)))
            {
                TraversalRequest tRequest = new TraversalRequest(FocusNavigationDirection.Next);
                UIElement keyboardFocus = Keyboard.FocusedElement as UIElement;

                if (keyboardFocus != null)
                {
                    keyboardFocus.MoveFocus(tRequest);
                }

                e.Handled = true;
            }
    else if ((Keyboard.Modifiers == ModifierKeys.Shift) && e.Key == (Key.Tab))
            {
                TraversalRequest tRequest = new TraversalRequest(FocusNavigationDirection.Previous);
                UIElement keyboardFocus = Keyboard.FocusedElement as UIElement;

                if (keyboardFocus != null)
                {
                    keyboardFocus.MoveFocus(tRequest);
                }

                e.Handled = true;
            }
        }

        public static void SelectNextControl()
        {
            TraversalRequest tRequest = new TraversalRequest(FocusNavigationDirection.Next);
            UIElement keyboardFocus = Keyboard.FocusedElement as UIElement;

            if (keyboardFocus != null)
            {
                keyboardFocus.MoveFocus(tRequest);
            }
        }

#endregion

        #region TreeView

        /// <summary>
        /// Tạo dataset binding với treeview
        /// </summary>
        /// <param name="ds">DataSet đầu vào</param>
        /// <param name="trv">TreeView cần tạo</param>
        /// <param name="columnID">Tên cột chứa mã</param>
        /// <param name="columnParentID">Tên cột chứa mã cha</param>
        /// <param name="columnDesc">Tên cột chứa phần hiển thị</param>
        /// <param name="isChecked">Check/uncheck tất cả các node</param>
        /// <param name="lstID">Danh sách ID các node được check</param>
        public static void CreateDataTreeView(DataSet ds, TreeView trv, string columnID, string columnParentID, string columnDesc, bool isChecked,ref List<string> lstID)
        {
            DataSet dsReturn = new DataSet();
            try
            {
                DataTable dt = new DataTable("data");
                dt.Columns.Add("Check", typeof(bool));
                dt.Columns.Add("ID");
                dt.Columns.Add("ParentID");
                dt.Columns.Add("Desc");

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string parentID = ds.Tables[0].Rows[i][columnParentID].ToString();
                    string id = ds.Tables[0].Rows[i][columnID].ToString();
                    string desc = ds.Tables[0].Rows[i][columnDesc].ToString();
                    if (string.IsNullOrEmpty(parentID))
                    {
                        dt.Rows.Add(isChecked, id, null, desc);
                    }
                    else
                    {
                        dt.Rows.Add(isChecked, id, parentID, desc);
                    }
                    if (isChecked)
                    {
                        lstID.Add(id);
                    }
                }

                dsReturn.Tables.Add(dt);
                dsReturn.Relations.Add("Master2Detail", dsReturn.Tables["data"].Columns["ID"], dsReturn.Tables["data"].Columns["ParentID"]);

                DataView dv = dsReturn.Tables["data"].DefaultView;
                dv.RowFilter = "ParentID IS NULL"; 
                trv.DataContext = dv;
            }
            catch (System.Exception ex)
            {
                lstID.Clear();
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        /// <summary>
        /// Sự kiện check
        /// </summary>
        /// <param name="trv">Treeview</param>
        /// <param name="e"></param>
        public static void EventCheckTreeView(TreeView trv, string id, bool isChecked, ref List<string> listChildRowChecked)
        {
            try
            {

                DataTable dt = ((DataView)trv.DataContext).Table;
                //Lay parent id
                DataRow drCurrent = GetRowByID(id, dt);
                string parentId = drCurrent["ParentID"].ToString();
                drCurrent["Check"] = isChecked;
                if (isChecked)
                {
                    listChildRowChecked.Add(drCurrent["ID"].ToString());
                }
                else
                {
                    listChildRowChecked.Remove(drCurrent["ID"].ToString());
                }
                //Check parent
                if (!string.IsNullOrEmpty(parentId))
                {
                    DataRow drParent = GetRowByID(parentId, dt);
                    CheckParent(drParent, dt, isChecked, ref listChildRowChecked);
                }

                //Check childs
                DataRow[] drChilds = GetChilds(drCurrent, dt);
                if (drChilds.Length > 0)
                {
                    foreach (DataRow dr in drChilds)
                    {
                        CheckChilds(dr, dt, isChecked, ref listChildRowChecked);
                    }
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        /// <summary>
        /// Xử lý check/uncheck node theo id truyền vào
        /// </summary>
        /// <param name="trv"></param>
        /// <param name="id"></param>
        /// <param name="isChecked"></param>
        /// <param name="listChildRowChecked"></param>
        public static void CheckByID(TreeView trv, string id, bool isChecked, ref List<string> listChildRowChecked)
        {
            try
            {
                DataTable dt = ((DataView)trv.DataContext).Table;
                DataRow dr = GetRowByID(id, dt);
                dr["Check"] = isChecked;
                if (isChecked)
                {
                    listChildRowChecked.Add(dr["ID"].ToString());
                }
                else
                {
                    listChildRowChecked.Remove(dr["ID"].ToString());
                }
            }
            catch(Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        /// <summary>
        /// Xử lý sự kiện check all
        /// </summary>
        /// <param name="trv">Treeview</param>
        /// <param name="isChecked">check/uncheck tất cả các node</param>
        /// <param name="listChildRowChecked">list id các node được check</param>
        public static void CheckAll(TreeView trv, bool isChecked, ref List<string> listChildRowChecked)
        {
            try
            {
                DataTable dt = ((DataView)trv.DataContext).Table;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dt.Rows[i]["Check"] = isChecked;
                    if (isChecked)
                    {
                        listChildRowChecked.Add(dt.Rows[i]["ID"].ToString());
                    }
                    else
                    {
                        listChildRowChecked.Remove(dt.Rows[i]["ID"].ToString());
                    }
                }
            }
            catch(Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        /// <summary>
        /// Check parent
        /// </summary>
        /// <param name="dr">Datarow hiện tại</param>
        /// <param name="dt">DataTable source</param>
        /// <param name="isChecked">true/false</param>
        private static void CheckParent(DataRow dr, DataTable dt, bool isChecked, ref List<string> listChildChecked)
        {
            dr["Check"] = DBNull.Value;
            listChildChecked.Remove(dr["ID"].ToString());
            bool childChecked = false;
            DataRow[] drChilds = GetChilds(dr, dt);
            bool dbnull = false;
            for (int i = 0; i < drChilds.Length; i++)
            {
                if (string.IsNullOrEmpty(drChilds[i]["Check"].ToString()))
                {
                    dbnull = true;
                    break;
                }
                else
                {
                    if (isChecked != Convert.ToBoolean(drChilds[i]["Check"]))
                    {
                        childChecked = false;
                        break;
                    }
                    else
                    {
                        childChecked = true;
                    }
                }
            }

            if (!dbnull && childChecked)
            {
                dr["Check"] = isChecked;
                if (isChecked)
                {
                    listChildChecked.Add(dr["ID"].ToString());
                }
            }

            string parentID = dr["ParentID"].ToString();
            if (!string.IsNullOrEmpty(parentID))
            {
                DataRow drParent = GetRowByID(parentID, dt);
                CheckParent(drParent, dt, isChecked, ref listChildChecked);
            }
        }

        /// <summary>
        /// Check childs
        /// </summary>
        /// <param name="dr">Datarow hiện tại</param>
        /// <param name="dt">DataTable source</param>
        /// <param name="isChecked">true/false</param>
        private static void CheckChilds(DataRow dr, DataTable dt, bool isChecked, ref List<string> listChildChecked)
        {
            dr["Check"] = isChecked;
            if (isChecked)
            {
                listChildChecked.Add(dr["ID"].ToString());
            }
            else
            {
                listChildChecked.Remove(dr["ID"].ToString());
            }
            DataRow[] drChilds = GetChilds(dr, dt);
            if (drChilds.Length > 0)
            {
                for (int i = 0; i < drChilds.Length; i++)
                {
                    CheckChilds(drChilds[i], dt, isChecked, ref listChildChecked);
                }
            }
        }

        /// <summary>
        /// Get danh sách childs của một row theo datatable source của treeview
        /// </summary>
        /// <param name="dr">DataRow hiện tại</param>
        /// <param name="dt">DataTable source</param>
        /// <returns></returns>
        private static DataRow[] GetChilds(DataRow dr, DataTable dt)
        {
            return dt.Select("ParentID='" + dr["ID"].ToString() + "'");
        }

        /// <summary>
        /// Get row theo ID truyền vào
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="dt">DataTable source</param>
        /// <returns></returns>
        private static DataRow GetRowByID(string id, DataTable dt)
        {
            DataRow[] drCurrent = dt.Select("ID='" + id + "'");
            if (drCurrent.Length > 0)
            {
                return drCurrent[0];
            }
            else
            {
                return null;
            }
        }
        
        /// <summary>
        /// Get datarow theo list ID truyền vào
        /// </summary>
        /// <param name="lstId">List ID</param>
        /// <param name="dt">DataTable</param>
        /// <returns></returns>
        private static DataRow[] GetRowsByListID(List<string> lstId, DataTable dt)
        {
            string cond = "(";
            for (int i = 0; i < lstId.Count; i++)
            {
                cond += "'" + lstId[i] + "',";
            }

            cond = cond.Substring(0, cond.Length - 1) + ")";
            return dt.Select("ID in " + cond);
        }

        #endregion

        #region Thông báo dữ liệu

        /// <summary>
        /// Hiện thông báo không chưa chọn 1 cái gì đó
        /// </summary>
        /// <param name="tenThongBao">Chuỗi cần truyền vào</param>
        public static void ThongBaoChuaChon(string tenThongBao)
        {
            tenThongBao = tenThongBao.Trim();
            if (tenThongBao.EndsWith(":"))
                tenThongBao = tenThongBao.Substring(0, tenThongBao.Length - 1);
            string[] s = new string[1] { tenThongBao };
            LMessage.ShowMessage("M.DungChung.ThongBao.ChuaChon", s, LMessage.MessageBoxType.Warning);
        }

        /// <summary>
        /// Hiện thông báo không chưa nhập 1 cái gì đó
        /// </summary>
        /// <param name="tenThongBao">Chuỗi cần truyền vào</param>
        public static void ThongBaoChuaNhap(string tenThongBao)
        {
            tenThongBao = tenThongBao.Trim();
            if (tenThongBao.EndsWith(":"))
                tenThongBao = tenThongBao.Substring(0, tenThongBao.Length - 1);
            string[] s = new string[1] { tenThongBao };
            LMessage.ShowMessage("M.DungChung.ThongBao.ChuaNhap", s, LMessage.MessageBoxType.Warning);
        }

        /// <summary>
        /// Hiện thông báo không được để trống
        /// </summary>
        /// <param name="tenThongBao">Chuỗi cần truyền vào</param>
        public static void ThongBaoTrong(string tenThongBao)
        {
            tenThongBao = tenThongBao.Trim();
            if (tenThongBao.EndsWith(":"))
                tenThongBao = tenThongBao.Substring(0, tenThongBao.Length - 1);
            string[] s = new string[1] { tenThongBao };
            LMessage.ShowMessage("M.DungChung.ThongBao.KhongDuocDeTrong",s, LMessage.MessageBoxType.Warning);    
        }

        #endregion

        #region Lay trang thai ban ghi

        /// <summary>
        /// Lấy trạng thái bản ghi dựa theo action và trạng thái bản ghi hiện tại
        /// </summary>
        /// <returns></returns>
        public static string LayTrangThaiBanGhi(DatabaseConstant.Action action, BusinessConstant.TrangThaiNghiepVu status)
        {
            string trangthai = "";
            switch (action)
            {
                case DatabaseConstant.Action.LUU_TAM:
                    if (status == BusinessConstant.TrangThaiNghiepVu.DA_DUYET)
                    {
                        trangthai = BusinessConstant.TrangThaiNghiepVu.SUA_SAU_DUYET.layGiaTri();
                    }
                    else if (status == BusinessConstant.TrangThaiNghiepVu.CHO_DUYET)
                    {
                        trangthai = BusinessConstant.TrangThaiNghiepVu.LUU_TAM.layGiaTri();
                    }
                    else if (status == BusinessConstant.TrangThaiNghiepVu.SUA_SAU_DUYET)
                    {
                        trangthai = BusinessConstant.layGiaTri(BusinessConstant.TrangThaiNghiepVu.LUU_TAM_SUA_SAU_DUYET);
                    }
                    else
                    {
                        trangthai = BusinessConstant.TrangThaiNghiepVu.LUU_TAM.layGiaTri();
                    }
                    break;
                case DatabaseConstant.Action.LUU:
                    if (status == BusinessConstant.TrangThaiNghiepVu.DA_DUYET)
                    {
                        trangthai = BusinessConstant.TrangThaiNghiepVu.SUA_SAU_DUYET.layGiaTri();
                    }
                    else if (status == BusinessConstant.TrangThaiNghiepVu.SUA_SAU_DUYET)
                    {
                        trangthai = BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri();
                    }
                    else
                    {
                        trangthai = BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri();
                    }
                    break;
                case DatabaseConstant.Action.TRINH_DUYET:
                    if (status == BusinessConstant.TrangThaiNghiepVu.DA_DUYET)
                    {
                        trangthai = BusinessConstant.TrangThaiNghiepVu.SUA_SAU_DUYET.layGiaTri();
                    }
                    else if (status == BusinessConstant.TrangThaiNghiepVu.SUA_SAU_DUYET)
                    {
                        trangthai = BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri();
                    }
                    else
                    {
                        trangthai = BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri();
                    }
                    break;
                case DatabaseConstant.Action.DUYET:
                    trangthai = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();
                    break;
                case DatabaseConstant.Action.THOAI_DUYET:
                    trangthai = BusinessConstant.TrangThaiNghiepVu.THOAI_DUYET.layGiaTri();
                    break;
                case DatabaseConstant.Action.TU_CHOI_DUYET:
                    trangthai = BusinessConstant.TrangThaiNghiepVu.TU_CHOI.layGiaTri();
                    break;
            }
            return trangthai;
        }

        /// <summary>
        /// Tạo trạng thái bản ghi dựa theo hành động và trạng thái hiện tại của bản ghi
        /// </summary>
        /// <param name="action">Hành động</param>
        /// <param name="trangthaiHienTai">Trạng thái hiện tại của bản ghi</param>
        /// <returns></returns>
        private static string TaoTrangThaiBanGhi(DatabaseConstant.Action action, string trangthaiHienTai)
        {
            if (string.IsNullOrEmpty(trangthaiHienTai)
                        || trangthaiHienTai == BusinessConstant.TrangThaiNghiepVu.LUU_TAM.layGiaTri()
                        || trangthaiHienTai == BusinessConstant.TrangThaiNghiepVu.TU_CHOI.layGiaTri()
                        || trangthaiHienTai == BusinessConstant.TrangThaiNghiepVu.THOAI_DUYET.layGiaTri()
                )
            {
                if (action == DatabaseConstant.Action.LUU_TAM)
                {
                    return BusinessConstant.TrangThaiNghiepVu.LUU_TAM.layGiaTri();
                }
                else if (action == DatabaseConstant.Action.LUU)
                {
                    return BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri();
                }
            }
            else
            {
                if (trangthaiHienTai == BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri())
                {
                    if (action == DatabaseConstant.Action.LUU_TAM)
                    {
                        return BusinessConstant.TrangThaiNghiepVu.LUU_TAM.layGiaTri();
                    }
                    else if (action == DatabaseConstant.Action.LUU)
                    {
                        return BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri();
                    }
                    else if (action == DatabaseConstant.Action.DUYET)
                    {
                        return BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();
                    }
                    else if (action == DatabaseConstant.Action.TU_CHOI_DUYET)
                    {
                        return BusinessConstant.TrangThaiNghiepVu.TU_CHOI.layGiaTri();
                    }
                }
                else if (trangthaiHienTai == BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri())
                {
                    if (action == DatabaseConstant.Action.LUU_TAM)
                    {
                        // Lưu tạm sửa sau duyệt
                        return "";
                    }
                    else if (action == DatabaseConstant.Action.LUU)
                    {
                        return BusinessConstant.TrangThaiNghiepVu.SUA_SAU_DUYET.layGiaTri();
                    }
                    else if (action == DatabaseConstant.Action.THOAI_DUYET)
                    {
                        return BusinessConstant.TrangThaiNghiepVu.THOAI_DUYET.layGiaTri();
                    }
                }
                else if (trangthaiHienTai == BusinessConstant.TrangThaiNghiepVu.SUA_SAU_DUYET.layGiaTri())
                {
                    if (action == DatabaseConstant.Action.LUU)
                    {
                        return BusinessConstant.TrangThaiNghiepVu.SUA_SAU_DUYET.layGiaTri();
                    }
                    else if (action == DatabaseConstant.Action.LUU_TAM)
                    {
                        // Lưu tạm sửa sau duyệt
                        return "";
                    }
                    else if (action == DatabaseConstant.Action.DUYET)
                    {
                        return BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();
                    }
                    else if (action == DatabaseConstant.Action.TU_CHOI_DUYET)
                    {
                        // Từ chối sửa sau duyệt
                        return "";
                    }
                }
                else
                {
                    if (action == DatabaseConstant.Action.LUU)
                    {
                        // Sửa sau duyệt
                        return BusinessConstant.TrangThaiNghiepVu.SUA_SAU_DUYET.layGiaTri();
                    }
                    else if (action == DatabaseConstant.Action.LUU_TAM)
                    {
                        //Lưu tạm sửa sau duyệt
                        return "";
                    }
                }
            }
            return "";
        }

        #endregion

        #region Ẩn hiện nút theo hành động và trạng thái
        private static List<Tuple<string, string, string, bool>> lstButton = new List<Tuple<string, string, string, bool>>();

        /// <summary>
        /// Tạo quan hệ giữa các hành động + trạng thái bản ghi và button trên toolbar
        /// </summary>
        // Bộ khởi tạo cho chức năng mặc định
        // thao tac,trang thai,tinh nang
        private static void KhoiTao()
        {
            lstButton.Clear();

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.THEM.getValue(), "", DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.THEM.getValue(), "", DatabaseConstant.Action.TRINH_DUYET.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.THEM.getValue(), "", DatabaseConstant.Action.LAY_LAI.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.THEM.getValue(), "", DatabaseConstant.Action.LUU.getValue(), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.THEM.getValue(), BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.THEM.getValue(), BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri(), DatabaseConstant.Action.TRINH_DUYET.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.THEM.getValue(), BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri(), DatabaseConstant.Action.LAY_LAI.getValue(), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.THEM.getValue(), BusinessConstant.TrangThaiNghiepVu.LUU_TAM.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.THEM.getValue(), BusinessConstant.TrangThaiNghiepVu.LUU_TAM.layGiaTri(), DatabaseConstant.Action.TRINH_DUYET.getValue(), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.THEM.getValue(), BusinessConstant.TrangThaiNghiepVu.SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.THEM.getValue(), BusinessConstant.TrangThaiNghiepVu.SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.TRINH_DUYET.getValue(), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.THEM.getValue(), BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.THEM.getValue(), BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri(), DatabaseConstant.Action.TRINH_DUYET.getValue(), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.THEM.getValue(), BusinessConstant.TrangThaiNghiepVu.TU_CHOI.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.THEM.getValue(), BusinessConstant.TrangThaiNghiepVu.TU_CHOI.layGiaTri(), DatabaseConstant.Action.TRINH_DUYET.getValue(), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.THEM.getValue(), BusinessConstant.TrangThaiNghiepVu.THOAI_DUYET.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.THEM.getValue(), BusinessConstant.TrangThaiNghiepVu.THOAI_DUYET.layGiaTri(), DatabaseConstant.Action.TRINH_DUYET.getValue(), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.THEM.getValue(), BusinessConstant.TrangThaiNghiepVu.LUU_TAM_SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.THEM.getValue(), BusinessConstant.TrangThaiNghiepVu.LUU_TAM_SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.TRINH_DUYET.getValue(), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.THEM.getValue(), BusinessConstant.TrangThaiNghiepVu.TU_CHOI_SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.THEM.getValue(), BusinessConstant.TrangThaiNghiepVu.TU_CHOI_SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.TRINH_DUYET.getValue(), true));

            //==========================================================================================================

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.XEM.getValue(), BusinessConstant.TrangThaiNghiepVu.LUU_TAM.layGiaTri(), DatabaseConstant.Action.THEM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.XEM.getValue(), BusinessConstant.TrangThaiNghiepVu.LUU_TAM.layGiaTri(), DatabaseConstant.Action.SUA.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.XEM.getValue(), BusinessConstant.TrangThaiNghiepVu.LUU_TAM.layGiaTri(), DatabaseConstant.Action.XOA.getValue(), true));

            //Khi xem mot ban ghi dang la TU_CHOI, se duoc thuc hien cac action: THEM, SUA, XOA, DUYET, TU_CHOI
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.XEM.getValue(), BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri(), DatabaseConstant.Action.THEM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.XEM.getValue(), BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri(), DatabaseConstant.Action.SUA.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.XEM.getValue(), BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri(), DatabaseConstant.Action.XOA.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.XEM.getValue(), BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri(), DatabaseConstant.Action.TU_CHOI_DUYET.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.XEM.getValue(), BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri(), DatabaseConstant.Action.DUYET.getValue(), true));

            //Khi xem mot ban ghi dang la TU_CHOI, se duoc thuc hien cac action: THEM, SUA, XOA
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.XEM.getValue(), BusinessConstant.TrangThaiNghiepVu.TU_CHOI.layGiaTri(), DatabaseConstant.Action.THEM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.XEM.getValue(), BusinessConstant.TrangThaiNghiepVu.TU_CHOI.layGiaTri(), DatabaseConstant.Action.SUA.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.XEM.getValue(), BusinessConstant.TrangThaiNghiepVu.TU_CHOI.layGiaTri(), DatabaseConstant.Action.XOA.getValue(), true));

            //Khi xem mot ban ghi dang la DA_DUYET, se duoc thuc hien cac action: THEM, DUYET
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.XEM.getValue(), BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri(), DatabaseConstant.Action.THEM.getValue(), true));
            //lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.XEM.getValue(), BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri(), DatabaseConstant.Action.SUA.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.XEM.getValue(), BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri(), DatabaseConstant.Action.THOAI_DUYET.getValue(), true));

            //Khi xem mot ban ghi dang la THOAI_DUYET, se duoc thuc hien cac action: THEM, SUA, XOA
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.XEM.getValue(), BusinessConstant.TrangThaiNghiepVu.THOAI_DUYET.layGiaTri(), DatabaseConstant.Action.THEM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.XEM.getValue(), BusinessConstant.TrangThaiNghiepVu.THOAI_DUYET.layGiaTri(), DatabaseConstant.Action.SUA.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.XEM.getValue(), BusinessConstant.TrangThaiNghiepVu.THOAI_DUYET.layGiaTri(), DatabaseConstant.Action.XOA.getValue(), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.XEM.getValue(), BusinessConstant.TrangThaiNghiepVu.SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.THEM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.XEM.getValue(), BusinessConstant.TrangThaiNghiepVu.SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.SUA.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.XEM.getValue(), BusinessConstant.TrangThaiNghiepVu.SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.XEM.getValue(), BusinessConstant.TrangThaiNghiepVu.SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.getValue(DatabaseConstant.Action.DUYET), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.XEM.getValue(), BusinessConstant.TrangThaiNghiepVu.LUU_TAM_SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.THEM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.XEM.getValue(), BusinessConstant.TrangThaiNghiepVu.LUU_TAM_SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.SUA.getValue(), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.XEM.getValue(), BusinessConstant.TrangThaiNghiepVu.TU_CHOI_SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.THEM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.XEM.getValue(), BusinessConstant.TrangThaiNghiepVu.TU_CHOI_SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.SUA.getValue(), true));

            //==========================================================================================================

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.SUA.getValue(), BusinessConstant.TrangThaiNghiepVu.LUU_TAM.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.SUA.getValue(), BusinessConstant.TrangThaiNghiepVu.LUU_TAM.layGiaTri(), DatabaseConstant.Action.TRINH_DUYET.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.SUA.getValue(), BusinessConstant.TrangThaiNghiepVu.LUU_TAM.layGiaTri(), DatabaseConstant.Action.LAY_LAI.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.SUA.getValue(), BusinessConstant.TrangThaiNghiepVu.LUU_TAM.layGiaTri(), DatabaseConstant.Action.LUU.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.SUA.getValue(), BusinessConstant.TrangThaiNghiepVu.LUU_TAM.layGiaTri(), DatabaseConstant.Action.XOA.getValue(), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.SUA.getValue(), BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.SUA.getValue(), BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri(), DatabaseConstant.Action.TRINH_DUYET.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.SUA.getValue(), BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri(), DatabaseConstant.Action.LUU.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.SUA.getValue(), BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri(), DatabaseConstant.Action.LAY_LAI.getValue(), true));
            //lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.SUA.getValue(), BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri(), DatabaseConstant.Action.XOA.getValue(), true));
            //lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.SUA.getValue(), BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri(), DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET), true));
            //lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.SUA.getValue(), BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri(), DatabaseConstant.getValue(DatabaseConstant.Action.DUYET), true));
            //lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.SUA.getValue(), BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri(), DatabaseConstant.getValue(DatabaseConstant.Action.IN), true));
            //lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.SUA.getValue(), BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri(), DatabaseConstant.getValue(DatabaseConstant.Action.XEM_TRUOC), true));

            //lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.SUA.getValue(), BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));
            //lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.SUA.getValue(), BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri(), DatabaseConstant.Action.TRINH_DUYET.getValue(), true));
            //lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.SUA.getValue(), BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri(), DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET), true));
            //lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.SUA.getValue(), BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri(), DatabaseConstant.getValue(DatabaseConstant.Action.IN), true));
            //lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.SUA.getValue(), BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri(), DatabaseConstant.getValue(DatabaseConstant.Action.XEM_TRUOC), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.SUA.getValue(), BusinessConstant.TrangThaiNghiepVu.TU_CHOI.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.SUA.getValue(), BusinessConstant.TrangThaiNghiepVu.TU_CHOI.layGiaTri(), DatabaseConstant.Action.TRINH_DUYET.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.SUA.getValue(), BusinessConstant.TrangThaiNghiepVu.TU_CHOI.layGiaTri(), DatabaseConstant.Action.LUU.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.SUA.getValue(), BusinessConstant.TrangThaiNghiepVu.TU_CHOI.layGiaTri(), DatabaseConstant.Action.LAY_LAI.getValue(), true));
            //lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.SUA.getValue(), BusinessConstant.TrangThaiNghiepVu.TU_CHOI.layGiaTri(), DatabaseConstant.Action.XOA.getValue(), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.SUA.getValue(), BusinessConstant.TrangThaiNghiepVu.THOAI_DUYET.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.SUA.getValue(), BusinessConstant.TrangThaiNghiepVu.THOAI_DUYET.layGiaTri(), DatabaseConstant.Action.TRINH_DUYET.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.SUA.getValue(), BusinessConstant.TrangThaiNghiepVu.THOAI_DUYET.layGiaTri(), DatabaseConstant.Action.LUU.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.SUA.getValue(), BusinessConstant.TrangThaiNghiepVu.THOAI_DUYET.layGiaTri(), DatabaseConstant.Action.LAY_LAI.getValue(), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.SUA.getValue(), BusinessConstant.TrangThaiNghiepVu.TU_CHOI_SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.SUA.getValue(), BusinessConstant.TrangThaiNghiepVu.TU_CHOI_SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.TRINH_DUYET.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.SUA.getValue(), BusinessConstant.TrangThaiNghiepVu.TU_CHOI_SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.LUU.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.SUA.getValue(), BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri(), DatabaseConstant.Action.LAY_LAI.getValue(), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.SUA.getValue(), BusinessConstant.layGiaTri(BusinessConstant.TrangThaiNghiepVu.LUU_TAM_SUA_SAU_DUYET), DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.SUA.getValue(), BusinessConstant.layGiaTri(BusinessConstant.TrangThaiNghiepVu.LUU_TAM_SUA_SAU_DUYET), DatabaseConstant.Action.TRINH_DUYET.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.SUA.getValue(), BusinessConstant.layGiaTri(BusinessConstant.TrangThaiNghiepVu.LUU_TAM_SUA_SAU_DUYET), DatabaseConstant.Action.LUU.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.SUA.getValue(), BusinessConstant.TrangThaiNghiepVu.LUU_TAM_SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.LAY_LAI.getValue(), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.SUA.getValue(), BusinessConstant.TrangThaiNghiepVu.SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.SUA.getValue(), BusinessConstant.TrangThaiNghiepVu.SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.TRINH_DUYET.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.SUA.getValue(), BusinessConstant.TrangThaiNghiepVu.SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.LUU.getValue(), true));
            //lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.SUA.getValue(), BusinessConstant.TrangThaiNghiepVu.SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET), true));
            //lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.SUA.getValue(), BusinessConstant.TrangThaiNghiepVu.SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.getValue(DatabaseConstant.Action.DUYET), true));

            //==========================================================================================================

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU_TAM.getValue(), "", DatabaseConstant.Action.XOA.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU_TAM.getValue(), "", DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU_TAM.getValue(), "", DatabaseConstant.Action.TRINH_DUYET.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU_TAM.getValue(), "", DatabaseConstant.Action.LUU.getValue(), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU_TAM.getValue(), BusinessConstant.TrangThaiNghiepVu.LUU_TAM.layGiaTri(), DatabaseConstant.Action.XOA.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU_TAM.getValue(), BusinessConstant.TrangThaiNghiepVu.LUU_TAM.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU_TAM.getValue(), BusinessConstant.TrangThaiNghiepVu.LUU_TAM.layGiaTri(), DatabaseConstant.Action.TRINH_DUYET.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU_TAM.getValue(), BusinessConstant.TrangThaiNghiepVu.LUU_TAM.layGiaTri(), DatabaseConstant.Action.LUU.getValue(), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU_TAM.getValue(), BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri(), DatabaseConstant.Action.XOA.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU_TAM.getValue(), BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU_TAM.getValue(), BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri(), DatabaseConstant.Action.TRINH_DUYET.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU_TAM.getValue(), BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri(), DatabaseConstant.Action.LUU.getValue(), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU_TAM.getValue(), BusinessConstant.TrangThaiNghiepVu.TU_CHOI.layGiaTri(), DatabaseConstant.Action.XOA.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU_TAM.getValue(), BusinessConstant.TrangThaiNghiepVu.TU_CHOI.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU_TAM.getValue(), BusinessConstant.TrangThaiNghiepVu.TU_CHOI.layGiaTri(), DatabaseConstant.Action.TRINH_DUYET.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU_TAM.getValue(), BusinessConstant.TrangThaiNghiepVu.TU_CHOI.layGiaTri(), DatabaseConstant.Action.LUU.getValue(), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU_TAM.getValue(), BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU_TAM.getValue(), BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri(), DatabaseConstant.Action.TRINH_DUYET.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU_TAM.getValue(), BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri(), DatabaseConstant.Action.LUU.getValue(), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU_TAM.getValue(), BusinessConstant.TrangThaiNghiepVu.SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU_TAM.getValue(), BusinessConstant.TrangThaiNghiepVu.SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.TRINH_DUYET.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU_TAM.getValue(), BusinessConstant.TrangThaiNghiepVu.SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.LUU.getValue(), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU_TAM.getValue(), BusinessConstant.TrangThaiNghiepVu.TU_CHOI_SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU_TAM.getValue(), BusinessConstant.TrangThaiNghiepVu.TU_CHOI_SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.TRINH_DUYET.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU_TAM.getValue(), BusinessConstant.TrangThaiNghiepVu.TU_CHOI_SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.LUU.getValue(), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU_TAM.getValue(), BusinessConstant.TrangThaiNghiepVu.LUU_TAM_SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU_TAM.getValue(), BusinessConstant.TrangThaiNghiepVu.LUU_TAM_SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.TRINH_DUYET.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU_TAM.getValue(), BusinessConstant.TrangThaiNghiepVu.LUU_TAM_SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.LUU.getValue(), true));

            //==========================================================================================================

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.TRINH_DUYET.getValue(), "", DatabaseConstant.Action.XOA.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.TRINH_DUYET.getValue(), "", DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.TRINH_DUYET.getValue(), "", DatabaseConstant.Action.TRINH_DUYET.getValue(), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.TRINH_DUYET.getValue(), BusinessConstant.TrangThaiNghiepVu.LUU_TAM.layGiaTri(), DatabaseConstant.Action.XOA.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.TRINH_DUYET.getValue(), BusinessConstant.TrangThaiNghiepVu.LUU_TAM.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.TRINH_DUYET.getValue(), BusinessConstant.TrangThaiNghiepVu.LUU_TAM.layGiaTri(), DatabaseConstant.Action.TRINH_DUYET.getValue(), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.TRINH_DUYET.getValue(), BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri(), DatabaseConstant.Action.XOA.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.TRINH_DUYET.getValue(), BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.TRINH_DUYET.getValue(), BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri(), DatabaseConstant.Action.TRINH_DUYET.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.TRINH_DUYET.getValue(), BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri(), DatabaseConstant.Action.DUYET.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.TRINH_DUYET.getValue(), BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri(), DatabaseConstant.Action.TU_CHOI_DUYET.getValue(), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.TRINH_DUYET.getValue(), BusinessConstant.TrangThaiNghiepVu.TU_CHOI.layGiaTri(), DatabaseConstant.Action.XOA.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.TRINH_DUYET.getValue(), BusinessConstant.TrangThaiNghiepVu.TU_CHOI.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.TRINH_DUYET.getValue(), BusinessConstant.TrangThaiNghiepVu.TU_CHOI.layGiaTri(), DatabaseConstant.Action.TRINH_DUYET.getValue(), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.TRINH_DUYET.getValue(), BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.TRINH_DUYET.getValue(), BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri(), DatabaseConstant.Action.TRINH_DUYET.getValue(), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.TRINH_DUYET.getValue(), BusinessConstant.TrangThaiNghiepVu.SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.TRINH_DUYET.getValue(), BusinessConstant.TrangThaiNghiepVu.SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.TRINH_DUYET.getValue(), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.TRINH_DUYET.getValue(), BusinessConstant.TrangThaiNghiepVu.TU_CHOI_SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.TRINH_DUYET.getValue(), BusinessConstant.TrangThaiNghiepVu.TU_CHOI_SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.TRINH_DUYET.getValue(), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.TRINH_DUYET.getValue(), BusinessConstant.TrangThaiNghiepVu.LUU_TAM_SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.TRINH_DUYET.getValue(), BusinessConstant.TrangThaiNghiepVu.LUU_TAM_SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.TRINH_DUYET.getValue(), true));

            //==========================================================================================================

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.TU_CHOI_DUYET.getValue(), BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri(), DatabaseConstant.Action.XOA.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.TU_CHOI_DUYET.getValue(), BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.TU_CHOI_DUYET.getValue(), BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri(), DatabaseConstant.Action.TRINH_DUYET.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.TU_CHOI_DUYET.getValue(), BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri(), DatabaseConstant.Action.LUU.getValue(), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.TU_CHOI_DUYET.getValue(), BusinessConstant.TrangThaiNghiepVu.SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.TU_CHOI_DUYET.getValue(), BusinessConstant.TrangThaiNghiepVu.SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.TRINH_DUYET.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.TU_CHOI_DUYET.getValue(), BusinessConstant.TrangThaiNghiepVu.SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.LUU.getValue(), true));

            //==========================================================================================================

            //lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.DUYET.getValue(), BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri(), DatabaseConstant.Action.THOAI_DUYET.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.DUYET.getValue(), BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri(), DatabaseConstant.Action.TRINH_DUYET.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.DUYET.getValue(), BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri(), DatabaseConstant.Action.LUU.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.DUYET.getValue(), BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));

            //lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.DUYET.getValue(), BusinessConstant.TrangThaiNghiepVu.SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.THOAI_DUYET.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.DUYET.getValue(), BusinessConstant.TrangThaiNghiepVu.SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.TRINH_DUYET.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.DUYET.getValue(), BusinessConstant.TrangThaiNghiepVu.SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.LUU.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.DUYET.getValue(), BusinessConstant.TrangThaiNghiepVu.SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));

            //==========================================================================================================
            
            //lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.THOAI_DUYET.getValue(), BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri(), DatabaseConstant.Action.XOA.getValue(), true));
            //lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.THOAI_DUYET.getValue(), BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri(), DatabaseConstant.Action.TRINH_DUYET.getValue(), true));
            //lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.THOAI_DUYET.getValue(), BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri(), DatabaseConstant.Action.LUU.getValue(), true));
            //lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.THOAI_DUYET.getValue(), BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));

            //==========================================================================================================

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU.getValue(), "", DatabaseConstant.Action.XOA.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU.getValue(), "", DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU.getValue(), "", DatabaseConstant.Action.LUU.getValue(), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU.getValue(), BusinessConstant.TrangThaiNghiepVu.LUU_TAM.layGiaTri(), DatabaseConstant.Action.XOA.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU.getValue(), BusinessConstant.TrangThaiNghiepVu.LUU_TAM.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU.getValue(), BusinessConstant.TrangThaiNghiepVu.LUU_TAM.layGiaTri(), DatabaseConstant.Action.LUU.getValue(), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU.getValue(), BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri(), DatabaseConstant.Action.XOA.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU.getValue(), BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU.getValue(), BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri(), DatabaseConstant.Action.LUU.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU.getValue(), BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri(), DatabaseConstant.Action.DUYET.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU.getValue(), BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri(), DatabaseConstant.Action.TU_CHOI_DUYET.getValue(), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU.getValue(), BusinessConstant.TrangThaiNghiepVu.TU_CHOI.layGiaTri(), DatabaseConstant.Action.XOA.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU.getValue(), BusinessConstant.TrangThaiNghiepVu.TU_CHOI.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU.getValue(), BusinessConstant.TrangThaiNghiepVu.TU_CHOI.layGiaTri(), DatabaseConstant.Action.LUU.getValue(), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU.getValue(), BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU.getValue(), BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri(), DatabaseConstant.Action.LUU.getValue(), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU.getValue(), BusinessConstant.TrangThaiNghiepVu.SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU.getValue(), BusinessConstant.TrangThaiNghiepVu.SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.LUU.getValue(), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU.getValue(), BusinessConstant.TrangThaiNghiepVu.TU_CHOI_SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU.getValue(), BusinessConstant.TrangThaiNghiepVu.TU_CHOI_SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.LUU.getValue(), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU.getValue(), BusinessConstant.TrangThaiNghiepVu.LUU_TAM_SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU.getValue(), BusinessConstant.TrangThaiNghiepVu.LUU_TAM_SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.LUU.getValue(), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LAY_LAI.getValue(), "", DatabaseConstant.Action.XOA.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LAY_LAI.getValue(), "", DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LAY_LAI.getValue(), "", DatabaseConstant.Action.TRINH_DUYET.getValue(), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LAY_LAI.getValue(), BusinessConstant.TrangThaiNghiepVu.LUU_TAM.layGiaTri(), DatabaseConstant.Action.XOA.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LAY_LAI.getValue(), BusinessConstant.TrangThaiNghiepVu.LUU_TAM.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LAY_LAI.getValue(), BusinessConstant.TrangThaiNghiepVu.LUU_TAM.layGiaTri(), DatabaseConstant.Action.TRINH_DUYET.getValue(), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LAY_LAI.getValue(), BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri(), DatabaseConstant.Action.XOA.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LAY_LAI.getValue(), BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LAY_LAI.getValue(), BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri(), DatabaseConstant.Action.TRINH_DUYET.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LAY_LAI.getValue(), BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri(), DatabaseConstant.Action.DUYET.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LAY_LAI.getValue(), BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri(), DatabaseConstant.Action.TU_CHOI_DUYET.getValue(), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LAY_LAI.getValue(), BusinessConstant.TrangThaiNghiepVu.TU_CHOI.layGiaTri(), DatabaseConstant.Action.XOA.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LAY_LAI.getValue(), BusinessConstant.TrangThaiNghiepVu.TU_CHOI.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LAY_LAI.getValue(), BusinessConstant.TrangThaiNghiepVu.TU_CHOI.layGiaTri(), DatabaseConstant.Action.TRINH_DUYET.getValue(), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LAY_LAI.getValue(), BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LAY_LAI.getValue(), BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri(), DatabaseConstant.Action.TRINH_DUYET.getValue(), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LAY_LAI.getValue(), BusinessConstant.TrangThaiNghiepVu.SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LAY_LAI.getValue(), BusinessConstant.TrangThaiNghiepVu.SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.TRINH_DUYET.getValue(), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LAY_LAI.getValue(), BusinessConstant.TrangThaiNghiepVu.TU_CHOI_SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LAY_LAI.getValue(), BusinessConstant.TrangThaiNghiepVu.TU_CHOI_SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.TRINH_DUYET.getValue(), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LAY_LAI.getValue(), BusinessConstant.TrangThaiNghiepVu.LUU_TAM_SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LAY_LAI.getValue(), BusinessConstant.TrangThaiNghiepVu.LUU_TAM_SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.TRINH_DUYET.getValue(), true));
        }

        // Bộ khởi tạo cho chức năng không cần duyệt
        // thao tac,trang thai,tinh nang
        private static void KhoiTaoKhongCanDuyet()
        {
            lstButton.Clear();

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.XEM.getValue(), "", DatabaseConstant.Action.LUU.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.THEM.getValue(), "", DatabaseConstant.Action.LUU.getValue(), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU.getValue(), BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri(), DatabaseConstant.Action.THEM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.XEM.getValue(), BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri(), DatabaseConstant.Action.THEM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.SUA.getValue(), BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri(), DatabaseConstant.Action.THEM.getValue(), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.XEM.getValue(), BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri(), DatabaseConstant.Action.SUA.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.XEM.getValue(), BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri(), DatabaseConstant.Action.XOA.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.XEM.getValue(), BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri(), DatabaseConstant.Action.SUA.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.XEM.getValue(), BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri(), DatabaseConstant.Action.XOA.getValue(), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.SUA.getValue(), BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri(), DatabaseConstant.Action.LUU.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.SUA.getValue(), BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri(), DatabaseConstant.Action.LUU.getValue(), true));            
        }

        // Bộ khởi tạo cho chức năng sửa sau duyệt
        // thao tac,trang thai,tinh nang
        private static void KhoiTaoSuaSauDuyet()
        {
            lstButton.Clear();

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.THEM.getValue(), "", DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.THEM.getValue(), "", DatabaseConstant.Action.TRINH_DUYET.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.THEM.getValue(), "", DatabaseConstant.Action.LAY_LAI.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.THEM.getValue(), "", DatabaseConstant.Action.LUU.getValue(), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.THEM.getValue(), BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.THEM.getValue(), BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri(), DatabaseConstant.Action.TRINH_DUYET.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.THEM.getValue(), BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri(), DatabaseConstant.Action.LAY_LAI.getValue(), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.THEM.getValue(), BusinessConstant.TrangThaiNghiepVu.LUU_TAM.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.THEM.getValue(), BusinessConstant.TrangThaiNghiepVu.LUU_TAM.layGiaTri(), DatabaseConstant.Action.TRINH_DUYET.getValue(), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.THEM.getValue(), BusinessConstant.TrangThaiNghiepVu.SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.THEM.getValue(), BusinessConstant.TrangThaiNghiepVu.SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.TRINH_DUYET.getValue(), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.THEM.getValue(), BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.THEM.getValue(), BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri(), DatabaseConstant.Action.TRINH_DUYET.getValue(), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.THEM.getValue(), BusinessConstant.TrangThaiNghiepVu.TU_CHOI.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.THEM.getValue(), BusinessConstant.TrangThaiNghiepVu.TU_CHOI.layGiaTri(), DatabaseConstant.Action.TRINH_DUYET.getValue(), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.THEM.getValue(), BusinessConstant.TrangThaiNghiepVu.THOAI_DUYET.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.THEM.getValue(), BusinessConstant.TrangThaiNghiepVu.THOAI_DUYET.layGiaTri(), DatabaseConstant.Action.TRINH_DUYET.getValue(), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.THEM.getValue(), BusinessConstant.TrangThaiNghiepVu.LUU_TAM_SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.THEM.getValue(), BusinessConstant.TrangThaiNghiepVu.LUU_TAM_SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.TRINH_DUYET.getValue(), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.THEM.getValue(), BusinessConstant.TrangThaiNghiepVu.TU_CHOI_SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.THEM.getValue(), BusinessConstant.TrangThaiNghiepVu.TU_CHOI_SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.TRINH_DUYET.getValue(), true));

            //==========================================================================================================

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.XEM.getValue(), BusinessConstant.TrangThaiNghiepVu.LUU_TAM.layGiaTri(), DatabaseConstant.Action.THEM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.XEM.getValue(), BusinessConstant.TrangThaiNghiepVu.LUU_TAM.layGiaTri(), DatabaseConstant.Action.SUA.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.XEM.getValue(), BusinessConstant.TrangThaiNghiepVu.LUU_TAM.layGiaTri(), DatabaseConstant.Action.XOA.getValue(), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.XEM.getValue(), BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri(), DatabaseConstant.Action.THEM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.XEM.getValue(), BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri(), DatabaseConstant.Action.SUA.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.XEM.getValue(), BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri(), DatabaseConstant.Action.XOA.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.XEM.getValue(), BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri(), DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.XEM.getValue(), BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri(), DatabaseConstant.getValue(DatabaseConstant.Action.DUYET), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.XEM.getValue(), BusinessConstant.TrangThaiNghiepVu.TU_CHOI.layGiaTri(), DatabaseConstant.Action.THEM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.XEM.getValue(), BusinessConstant.TrangThaiNghiepVu.TU_CHOI.layGiaTri(), DatabaseConstant.Action.SUA.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.XEM.getValue(), BusinessConstant.TrangThaiNghiepVu.TU_CHOI.layGiaTri(), DatabaseConstant.Action.XOA.getValue(), true));

            //lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.XEM.getValue(), BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri(), DatabaseConstant.Action.THEM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.XEM.getValue(), BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri(), DatabaseConstant.Action.SUA.getValue(), true));
            //lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.XEM.getValue(), BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri(), DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET), true));

            //lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.XEM.getValue(), BusinessConstant.TrangThaiNghiepVu.THOAI_DUYET.layGiaTri(), DatabaseConstant.Action.SUA.getValue(), true));
            //lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.XEM.getValue(), BusinessConstant.TrangThaiNghiepVu.THOAI_DUYET.layGiaTri(), DatabaseConstant.Action.XOA.getValue(), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.XEM.getValue(), BusinessConstant.TrangThaiNghiepVu.SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.THEM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.XEM.getValue(), BusinessConstant.TrangThaiNghiepVu.SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.SUA.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.XEM.getValue(), BusinessConstant.TrangThaiNghiepVu.SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.XEM.getValue(), BusinessConstant.TrangThaiNghiepVu.SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.getValue(DatabaseConstant.Action.DUYET), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.XEM.getValue(), BusinessConstant.TrangThaiNghiepVu.LUU_TAM_SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.THEM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.XEM.getValue(), BusinessConstant.TrangThaiNghiepVu.LUU_TAM_SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.SUA.getValue(), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.XEM.getValue(), BusinessConstant.TrangThaiNghiepVu.TU_CHOI_SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.THEM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.XEM.getValue(), BusinessConstant.TrangThaiNghiepVu.TU_CHOI_SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.SUA.getValue(), true));

            //==========================================================================================================

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.SUA.getValue(), BusinessConstant.TrangThaiNghiepVu.LUU_TAM.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.SUA.getValue(), BusinessConstant.TrangThaiNghiepVu.LUU_TAM.layGiaTri(), DatabaseConstant.Action.TRINH_DUYET.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.SUA.getValue(), BusinessConstant.TrangThaiNghiepVu.LUU_TAM.layGiaTri(), DatabaseConstant.Action.LAY_LAI.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.SUA.getValue(), BusinessConstant.TrangThaiNghiepVu.LUU_TAM.layGiaTri(), DatabaseConstant.Action.LUU.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.SUA.getValue(), BusinessConstant.TrangThaiNghiepVu.LUU_TAM.layGiaTri(), DatabaseConstant.Action.XOA.getValue(), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.SUA.getValue(), BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.SUA.getValue(), BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri(), DatabaseConstant.Action.TRINH_DUYET.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.SUA.getValue(), BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri(), DatabaseConstant.Action.LUU.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.SUA.getValue(), BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri(), DatabaseConstant.Action.LAY_LAI.getValue(), true));
            //lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.SUA.getValue(), BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri(), DatabaseConstant.Action.XOA.getValue(), true));
            //lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.SUA.getValue(), BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri(), DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET), true));
            //lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.SUA.getValue(), BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri(), DatabaseConstant.getValue(DatabaseConstant.Action.DUYET), true));
            //lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.SUA.getValue(), BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri(), DatabaseConstant.getValue(DatabaseConstant.Action.IN), true));
            //lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.SUA.getValue(), BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri(), DatabaseConstant.getValue(DatabaseConstant.Action.XEM_TRUOC), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.SUA.getValue(), BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri(), DatabaseConstant.Action.LUU.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.SUA.getValue(), BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri(), DatabaseConstant.Action.TRINH_DUYET.getValue(), true));
            //lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.SUA.getValue(), BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri(), DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET), true));
            //lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.SUA.getValue(), BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri(), DatabaseConstant.getValue(DatabaseConstant.Action.IN), true));
            //lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.SUA.getValue(), BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri(), DatabaseConstant.getValue(DatabaseConstant.Action.XEM_TRUOC), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.SUA.getValue(), BusinessConstant.TrangThaiNghiepVu.TU_CHOI.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.SUA.getValue(), BusinessConstant.TrangThaiNghiepVu.TU_CHOI.layGiaTri(), DatabaseConstant.Action.TRINH_DUYET.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.SUA.getValue(), BusinessConstant.TrangThaiNghiepVu.TU_CHOI.layGiaTri(), DatabaseConstant.Action.LUU.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.SUA.getValue(), BusinessConstant.TrangThaiNghiepVu.TU_CHOI.layGiaTri(), DatabaseConstant.Action.LAY_LAI.getValue(), true));
            //lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.SUA.getValue(), BusinessConstant.TrangThaiNghiepVu.TU_CHOI.layGiaTri(), DatabaseConstant.Action.XOA.getValue(), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.SUA.getValue(), BusinessConstant.TrangThaiNghiepVu.THOAI_DUYET.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.SUA.getValue(), BusinessConstant.TrangThaiNghiepVu.THOAI_DUYET.layGiaTri(), DatabaseConstant.Action.TRINH_DUYET.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.SUA.getValue(), BusinessConstant.TrangThaiNghiepVu.THOAI_DUYET.layGiaTri(), DatabaseConstant.Action.LUU.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.SUA.getValue(), BusinessConstant.TrangThaiNghiepVu.THOAI_DUYET.layGiaTri(), DatabaseConstant.Action.LAY_LAI.getValue(), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.SUA.getValue(), BusinessConstant.TrangThaiNghiepVu.TU_CHOI_SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.SUA.getValue(), BusinessConstant.TrangThaiNghiepVu.TU_CHOI_SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.TRINH_DUYET.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.SUA.getValue(), BusinessConstant.TrangThaiNghiepVu.TU_CHOI_SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.LUU.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.SUA.getValue(), BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri(), DatabaseConstant.Action.LAY_LAI.getValue(), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.SUA.getValue(), BusinessConstant.layGiaTri(BusinessConstant.TrangThaiNghiepVu.LUU_TAM_SUA_SAU_DUYET), DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.SUA.getValue(), BusinessConstant.layGiaTri(BusinessConstant.TrangThaiNghiepVu.LUU_TAM_SUA_SAU_DUYET), DatabaseConstant.Action.TRINH_DUYET.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.SUA.getValue(), BusinessConstant.layGiaTri(BusinessConstant.TrangThaiNghiepVu.LUU_TAM_SUA_SAU_DUYET), DatabaseConstant.Action.LUU.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.SUA.getValue(), BusinessConstant.TrangThaiNghiepVu.LUU_TAM_SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.LAY_LAI.getValue(), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.SUA.getValue(), BusinessConstant.TrangThaiNghiepVu.SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.SUA.getValue(), BusinessConstant.TrangThaiNghiepVu.SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.TRINH_DUYET.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.SUA.getValue(), BusinessConstant.TrangThaiNghiepVu.SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.LUU.getValue(), true));
            //lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.SUA.getValue(), BusinessConstant.TrangThaiNghiepVu.SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET), true));
            //lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.SUA.getValue(), BusinessConstant.TrangThaiNghiepVu.SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.getValue(DatabaseConstant.Action.DUYET), true));

            //==========================================================================================================

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU_TAM.getValue(), "", DatabaseConstant.Action.XOA.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU_TAM.getValue(), "", DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU_TAM.getValue(), "", DatabaseConstant.Action.TRINH_DUYET.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU_TAM.getValue(), "", DatabaseConstant.Action.LUU.getValue(), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU_TAM.getValue(), BusinessConstant.TrangThaiNghiepVu.LUU_TAM.layGiaTri(), DatabaseConstant.Action.XOA.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU_TAM.getValue(), BusinessConstant.TrangThaiNghiepVu.LUU_TAM.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU_TAM.getValue(), BusinessConstant.TrangThaiNghiepVu.LUU_TAM.layGiaTri(), DatabaseConstant.Action.TRINH_DUYET.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU_TAM.getValue(), BusinessConstant.TrangThaiNghiepVu.LUU_TAM.layGiaTri(), DatabaseConstant.Action.LUU.getValue(), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU_TAM.getValue(), BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri(), DatabaseConstant.Action.XOA.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU_TAM.getValue(), BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU_TAM.getValue(), BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri(), DatabaseConstant.Action.TRINH_DUYET.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU_TAM.getValue(), BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri(), DatabaseConstant.Action.LUU.getValue(), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU_TAM.getValue(), BusinessConstant.TrangThaiNghiepVu.TU_CHOI.layGiaTri(), DatabaseConstant.Action.XOA.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU_TAM.getValue(), BusinessConstant.TrangThaiNghiepVu.TU_CHOI.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU_TAM.getValue(), BusinessConstant.TrangThaiNghiepVu.TU_CHOI.layGiaTri(), DatabaseConstant.Action.TRINH_DUYET.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU_TAM.getValue(), BusinessConstant.TrangThaiNghiepVu.TU_CHOI.layGiaTri(), DatabaseConstant.Action.LUU.getValue(), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU_TAM.getValue(), BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU_TAM.getValue(), BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri(), DatabaseConstant.Action.TRINH_DUYET.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU_TAM.getValue(), BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri(), DatabaseConstant.Action.LUU.getValue(), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU_TAM.getValue(), BusinessConstant.TrangThaiNghiepVu.SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU_TAM.getValue(), BusinessConstant.TrangThaiNghiepVu.SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.TRINH_DUYET.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU_TAM.getValue(), BusinessConstant.TrangThaiNghiepVu.SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.LUU.getValue(), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU_TAM.getValue(), BusinessConstant.TrangThaiNghiepVu.TU_CHOI_SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU_TAM.getValue(), BusinessConstant.TrangThaiNghiepVu.TU_CHOI_SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.TRINH_DUYET.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU_TAM.getValue(), BusinessConstant.TrangThaiNghiepVu.TU_CHOI_SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.LUU.getValue(), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU_TAM.getValue(), BusinessConstant.TrangThaiNghiepVu.LUU_TAM_SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU_TAM.getValue(), BusinessConstant.TrangThaiNghiepVu.LUU_TAM_SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.TRINH_DUYET.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU_TAM.getValue(), BusinessConstant.TrangThaiNghiepVu.LUU_TAM_SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.LUU.getValue(), true));

            //==========================================================================================================

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.TRINH_DUYET.getValue(), "", DatabaseConstant.Action.XOA.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.TRINH_DUYET.getValue(), "", DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.TRINH_DUYET.getValue(), "", DatabaseConstant.Action.TRINH_DUYET.getValue(), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.TRINH_DUYET.getValue(), BusinessConstant.TrangThaiNghiepVu.LUU_TAM.layGiaTri(), DatabaseConstant.Action.XOA.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.TRINH_DUYET.getValue(), BusinessConstant.TrangThaiNghiepVu.LUU_TAM.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.TRINH_DUYET.getValue(), BusinessConstant.TrangThaiNghiepVu.LUU_TAM.layGiaTri(), DatabaseConstant.Action.TRINH_DUYET.getValue(), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.TRINH_DUYET.getValue(), BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri(), DatabaseConstant.Action.XOA.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.TRINH_DUYET.getValue(), BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.TRINH_DUYET.getValue(), BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri(), DatabaseConstant.Action.TRINH_DUYET.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.TRINH_DUYET.getValue(), BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri(), DatabaseConstant.Action.DUYET.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.TRINH_DUYET.getValue(), BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri(), DatabaseConstant.Action.TU_CHOI_DUYET.getValue(), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.TRINH_DUYET.getValue(), BusinessConstant.TrangThaiNghiepVu.TU_CHOI.layGiaTri(), DatabaseConstant.Action.XOA.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.TRINH_DUYET.getValue(), BusinessConstant.TrangThaiNghiepVu.TU_CHOI.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.TRINH_DUYET.getValue(), BusinessConstant.TrangThaiNghiepVu.TU_CHOI.layGiaTri(), DatabaseConstant.Action.TRINH_DUYET.getValue(), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.TRINH_DUYET.getValue(), BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.TRINH_DUYET.getValue(), BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri(), DatabaseConstant.Action.TRINH_DUYET.getValue(), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.TRINH_DUYET.getValue(), BusinessConstant.TrangThaiNghiepVu.SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.TRINH_DUYET.getValue(), BusinessConstant.TrangThaiNghiepVu.SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.TRINH_DUYET.getValue(), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.TRINH_DUYET.getValue(), BusinessConstant.TrangThaiNghiepVu.TU_CHOI_SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.TRINH_DUYET.getValue(), BusinessConstant.TrangThaiNghiepVu.TU_CHOI_SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.TRINH_DUYET.getValue(), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.TRINH_DUYET.getValue(), BusinessConstant.TrangThaiNghiepVu.LUU_TAM_SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.TRINH_DUYET.getValue(), BusinessConstant.TrangThaiNghiepVu.LUU_TAM_SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.TRINH_DUYET.getValue(), true));

            //==========================================================================================================

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.TU_CHOI_DUYET.getValue(), BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri(), DatabaseConstant.Action.XOA.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.TU_CHOI_DUYET.getValue(), BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.TU_CHOI_DUYET.getValue(), BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri(), DatabaseConstant.Action.TRINH_DUYET.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.TU_CHOI_DUYET.getValue(), BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri(), DatabaseConstant.Action.LUU.getValue(), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.TU_CHOI_DUYET.getValue(), BusinessConstant.TrangThaiNghiepVu.SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.TU_CHOI_DUYET.getValue(), BusinessConstant.TrangThaiNghiepVu.SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.TRINH_DUYET.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.TU_CHOI_DUYET.getValue(), BusinessConstant.TrangThaiNghiepVu.SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.LUU.getValue(), true));

            //==========================================================================================================

            //lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.DUYET.getValue(), BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri(), DatabaseConstant.Action.THOAI_DUYET.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.DUYET.getValue(), BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri(), DatabaseConstant.Action.TRINH_DUYET.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.DUYET.getValue(), BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri(), DatabaseConstant.Action.LUU.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.DUYET.getValue(), BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));

            //lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.DUYET.getValue(), BusinessConstant.TrangThaiNghiepVu.SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.THOAI_DUYET.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.DUYET.getValue(), BusinessConstant.TrangThaiNghiepVu.SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.TRINH_DUYET.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.DUYET.getValue(), BusinessConstant.TrangThaiNghiepVu.SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.LUU.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.DUYET.getValue(), BusinessConstant.TrangThaiNghiepVu.SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));

            //==========================================================================================================

            //lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.THOAI_DUYET.getValue(), BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri(), DatabaseConstant.Action.XOA.getValue(), true));
            //lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.THOAI_DUYET.getValue(), BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri(), DatabaseConstant.Action.TRINH_DUYET.getValue(), true));
            //lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.THOAI_DUYET.getValue(), BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri(), DatabaseConstant.Action.LUU.getValue(), true));
            //lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.THOAI_DUYET.getValue(), BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));

            //==========================================================================================================

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU.getValue(), "", DatabaseConstant.Action.XOA.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU.getValue(), "", DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU.getValue(), "", DatabaseConstant.Action.LUU.getValue(), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU.getValue(), BusinessConstant.TrangThaiNghiepVu.LUU_TAM.layGiaTri(), DatabaseConstant.Action.XOA.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU.getValue(), BusinessConstant.TrangThaiNghiepVu.LUU_TAM.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU.getValue(), BusinessConstant.TrangThaiNghiepVu.LUU_TAM.layGiaTri(), DatabaseConstant.Action.LUU.getValue(), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU.getValue(), BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri(), DatabaseConstant.Action.XOA.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU.getValue(), BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU.getValue(), BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri(), DatabaseConstant.Action.LUU.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU.getValue(), BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri(), DatabaseConstant.Action.DUYET.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU.getValue(), BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri(), DatabaseConstant.Action.TU_CHOI_DUYET.getValue(), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU.getValue(), BusinessConstant.TrangThaiNghiepVu.TU_CHOI.layGiaTri(), DatabaseConstant.Action.XOA.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU.getValue(), BusinessConstant.TrangThaiNghiepVu.TU_CHOI.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU.getValue(), BusinessConstant.TrangThaiNghiepVu.TU_CHOI.layGiaTri(), DatabaseConstant.Action.LUU.getValue(), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU.getValue(), BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU.getValue(), BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri(), DatabaseConstant.Action.LUU.getValue(), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU.getValue(), BusinessConstant.TrangThaiNghiepVu.SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU.getValue(), BusinessConstant.TrangThaiNghiepVu.SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.LUU.getValue(), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU.getValue(), BusinessConstant.TrangThaiNghiepVu.TU_CHOI_SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU.getValue(), BusinessConstant.TrangThaiNghiepVu.TU_CHOI_SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.LUU.getValue(), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU.getValue(), BusinessConstant.TrangThaiNghiepVu.LUU_TAM_SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LUU.getValue(), BusinessConstant.TrangThaiNghiepVu.LUU_TAM_SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.LUU.getValue(), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LAY_LAI.getValue(), "", DatabaseConstant.Action.XOA.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LAY_LAI.getValue(), "", DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LAY_LAI.getValue(), "", DatabaseConstant.Action.TRINH_DUYET.getValue(), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LAY_LAI.getValue(), BusinessConstant.TrangThaiNghiepVu.LUU_TAM.layGiaTri(), DatabaseConstant.Action.XOA.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LAY_LAI.getValue(), BusinessConstant.TrangThaiNghiepVu.LUU_TAM.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LAY_LAI.getValue(), BusinessConstant.TrangThaiNghiepVu.LUU_TAM.layGiaTri(), DatabaseConstant.Action.TRINH_DUYET.getValue(), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LAY_LAI.getValue(), BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri(), DatabaseConstant.Action.XOA.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LAY_LAI.getValue(), BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LAY_LAI.getValue(), BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri(), DatabaseConstant.Action.TRINH_DUYET.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LAY_LAI.getValue(), BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri(), DatabaseConstant.Action.DUYET.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LAY_LAI.getValue(), BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri(), DatabaseConstant.Action.TU_CHOI_DUYET.getValue(), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LAY_LAI.getValue(), BusinessConstant.TrangThaiNghiepVu.TU_CHOI.layGiaTri(), DatabaseConstant.Action.XOA.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LAY_LAI.getValue(), BusinessConstant.TrangThaiNghiepVu.TU_CHOI.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LAY_LAI.getValue(), BusinessConstant.TrangThaiNghiepVu.TU_CHOI.layGiaTri(), DatabaseConstant.Action.TRINH_DUYET.getValue(), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LAY_LAI.getValue(), BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LAY_LAI.getValue(), BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri(), DatabaseConstant.Action.TRINH_DUYET.getValue(), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LAY_LAI.getValue(), BusinessConstant.TrangThaiNghiepVu.SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LAY_LAI.getValue(), BusinessConstant.TrangThaiNghiepVu.SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.TRINH_DUYET.getValue(), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LAY_LAI.getValue(), BusinessConstant.TrangThaiNghiepVu.TU_CHOI_SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LAY_LAI.getValue(), BusinessConstant.TrangThaiNghiepVu.TU_CHOI_SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.TRINH_DUYET.getValue(), true));

            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LAY_LAI.getValue(), BusinessConstant.TrangThaiNghiepVu.LUU_TAM_SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.LUU_TAM.getValue(), true));
            lstButton.Add(new Tuple<string, string, string, bool>(DatabaseConstant.Action.LAY_LAI.getValue(), BusinessConstant.TrangThaiNghiepVu.LUU_TAM_SUA_SAU_DUYET.layGiaTri(), DatabaseConstant.Action.TRINH_DUYET.getValue(), true));
        }

        /// <summary>
        /// Xử lý ẩn hiện button trên toolbar
        /// </summary>
        /// <param name="toolbar">WrapPanel chứa toolbar</param>
        /// <param name="action">Một hành động được định nghĩa trong DatabaseConstant.Action</param>
        /// <param name="status"></param>
        public static void RefreshButton(WrapPanel toolbar, DatabaseConstant.Action action, string status,ContextMenu context = null,DatabaseConstant.Function func = DatabaseConstant.Function.DEFAULT)
        {
            int approvedMatrixValue = 0;
            approvedMatrixValue = DatabaseConstant.getApprovedMatrixValue(func);

            // FIX tam cac function cho phep sua sau duyet
            if (func.Equals(DatabaseConstant.Function.KH_THANH_VIEN) ||
                func.Equals(DatabaseConstant.Function.KH_THANH_VIEN) ||
                func.Equals(DatabaseConstant.Function.KH_THANH_VIEN) ||
                func.Equals(DatabaseConstant.Function.KH_THANH_VIEN))
            {
                if (!Presentation.Process.Common.ClientInformation.Company.Equals("BIDV") 
                    && !Presentation.Process.Common.ClientInformation.Company.Equals("BIDV_BLF"))
                {
                    approvedMatrixValue = 2;
                }
            }

            if (approvedMatrixValue == 0)
            {
                KhoiTaoKhongCanDuyet();
            }
            else if (approvedMatrixValue == 1)
            {
                KhoiTao();
            }
            else if (approvedMatrixValue == 2)
            {
                KhoiTaoSuaSauDuyet();
            }
            else
            {
                KhoiTao();
            }
            List<Tuple<string, string, string, bool>> query = lstButton.Where(e => e.Item1 == DatabaseConstant.getValue(action) && e.Item2 == status).ToList();
            foreach (var item in toolbar.Children)
            {
                if (item is RibbonButton)
                {
                    RibbonButton rb = (RibbonButton)item;
                    if (((RibbonButton)item).Tag != "false")
                    {
                        string strTinhNang = rb.Name.Substring(3, rb.Name.Length - 3);
                        if (strTinhNang != DatabaseConstant.Action.DONG.getValue() && strTinhNang != DatabaseConstant.Action.TRO_GIUP.getValue() && !strTinhNang.Contains(DatabaseConstant.Action.XEM_TRUOC.getValue()) && strTinhNang != DatabaseConstant.Action.IN.getValue())
                        {
                            var x = query.Select(e => e.Item3).Contains(strTinhNang);
                            if (query.Select(e => e.Item3).Contains(strTinhNang))
                            {
                                rb.IsEnabled = true;
                            }
                            else
                            {
                                rb.IsEnabled = false;
                            }
                            if (strTinhNang.Equals(DatabaseConstant.Action.LUU_TAM.getValue()))
                                rb.Visibility = Visibility.Collapsed;
                            if (strTinhNang.Equals(DatabaseConstant.Action.XUAT_DU_LIEU.getValue()))
                                rb.Visibility = Visibility.Collapsed;
                        }
                    }
                }
            }
            if (context != null)
            {
                foreach (var item in context.Items)
                {
                    if (item is MenuItem)
                    {
                        //MenuItem mnu = (MenuItem)item;
                        //if (mnu.IsEnabled == true)
                        //{
                        //    string strTinhNang = mnu.Name.Substring(3, mnu.Name.Length - 3);
                        //    if (strTinhNang != DatabaseConstant.Action.DONG.getValue() && 
                        //        strTinhNang != DatabaseConstant.Action.TRO_GIUP.getValue())
                        //    {
                        //        var x = query.Select(e => e.Item3).Contains(strTinhNang);
                        //        if (query.Select(e => e.Item3).Contains(strTinhNang))
                        //        {
                        //            mnu.IsEnabled = true;
                        //        }
                        //        else
                        //        {
                        //            mnu.IsEnabled = false;
                        //            mnu.Opacity = .5;
                        //        }
                        //    }
                        //}

                        MenuItem mnu = (MenuItem)item;
                        string strTinhNang = mnu.Name.Substring(3, mnu.Name.Length - 3);
                        if (strTinhNang != DatabaseConstant.Action.DONG.getValue() && strTinhNang != DatabaseConstant.Action.TRO_GIUP.getValue() && strTinhNang != DatabaseConstant.Action.XEM_TRUOC.getValue())
                        {
                            var x = query.Select(e => e.Item3).Contains(strTinhNang);
                            if (query.Select(e => e.Item3).Contains(strTinhNang))
                            {
                                mnu.IsEnabled = true;
                                mnu.Opacity = 1;
                            }
                            else
                            {
                                mnu.IsEnabled = false;
                                mnu.Opacity = .5;
                            }
                        }
                        if (strTinhNang.Equals(DatabaseConstant.Action.XUAT_DU_LIEU.getValue()))
                            mnu.Visibility = Visibility.Collapsed;
                    }
                }
            }
        }

        #endregion

        #region Thông báo lỗi
        /// <summary>
        /// Thông báo lỗi
        /// </summary>
        /// <param name="ex"></param>
        public static void ThongBaoLoi(System.Exception ex)
        {
            if (ex.GetType() == typeof(CustomException))
            {
                new frmThongBaoLoi(ex.Message, ex.InnerException).ShowDialog();
            }
            else if (ex.InnerException != null && ex.InnerException.GetType() == typeof(CustomException))
            {
                new frmThongBaoLoi(ex.InnerException.Message, ex.InnerException).ShowDialog();
            }
            else
            {
                new frmThongBaoLoi("M.ResponseMessage.Common.KhongThanhCong", ex).ShowDialog();
            }
        }

        /// <summary>
        /// Thông báo lỗi không Exception
        /// </summary>
        /// <param name="message">Chuỗi thông báo lỗi</param>
        public static void ThongBaoLoi(string message)
        {
            new frmThongBaoLoi(message, null).ShowDialog();
        }

        /// <summary>
        /// Thông báo kết quả
        /// </summary>
        /// <param name="listClientResponseDetail"></param>
        public static void ThongBaoKetQua(List<ClientResponseDetail> listClientResponseDetail)
        {
            if (listClientResponseDetail != null && listClientResponseDetail.Count > 0)
            {
                frmThongBaoKetQua frm = new frmThongBaoKetQua();
                frm.ListClientResponseDetail = listClientResponseDetail;
                frm.ShowDialog();                
            }
        }

        /// <summary>
        /// Thông báo kết quả
        /// </summary>
        /// <param name="listClientResponseDetail"></param>
        public static void ThongBaoKetQua(DataTable dtKetQua)
        {
            if (dtKetQua != null && dtKetQua.Rows.Count > 0)
            {
                frmThongBaoKetQuaLoi frm = new frmThongBaoKetQuaLoi();
                frm.dtKetQua = dtKetQua;
                frm.ShowDialog();
            }
        }
        #endregion
        
        #region Vị trí trang danh sách
        /// <summary>
        /// Hàm chuyển con trỏ đến đúng vị trí của bản ghi khi thay đổi thông tin từ Form chi tiết xuống Form danh sách
        /// </summary>
        /// <param name="id">ID của bản ghi</param>
        /// <param name="grid">Grid danh sách</param>
        /// <param name="pageSize">Control số bản ghi trên 1 trang</param>
        /// <param name="page">Control trang</param>
        public static void GoToPosition(int id, ref RadGridView grid,RadDataPager page, RadNumericUpDown pageSize)
        {
            try
            {
                page.PageSize = 10000;
                int iPage = 0;
                int iposition = 0;
                int id1 = 0;
                DataRowView dataRowView;
                DataRow dataRow;

                for (int i = 0; i < grid.Items.Count; i++)
                {                     
                    var obj= grid.Items[i];
                    if (obj is DataRowView)
                    {
                        dataRowView = (DataRowView)obj;
                        id1 = Convert.ToInt32(dataRowView["ID"]);
                    }
                    else if (obj is DataRow)
                    {
                        dataRow = (DataRow)obj;
                        id1 = Convert.ToInt32(dataRow["ID"]);
                    }

                    if (id1 == id)
                    {
                        int iPageSize = (int)pageSize.Value;
                        iPage = i / iPageSize;
                        iposition = i % iPageSize;
                        break;
                    }
                }
                page.PageSize = (int)pageSize.Value;                
                grid.Items.MoveToPage(iPage);
                grid.Items.MoveCurrentToPosition(iposition);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }
        #endregion

        #region Phân trang
        /// <summary>
        /// Hàm trả về DataTable trên Client phục vụ phân trang dữ liệu
        /// </summary>
        /// <param name="serverDataTable"></param>
        /// <param name="paggingIndex"></param>
        /// <param name="paggingSize"></param>
        /// <param name="totalRecord"></param>
        /// <returns></returns>
        public static DataTable BuildClientDataTable(DataTable serverDataTable, int paggingIndex, int paggingSize, int totalRecord)
        {
            if (serverDataTable == null)
            {
                return null;
            }
            
            DataTable clientDataTable = new DataTable();
            clientDataTable = serverDataTable.Clone();

            int prevNullItem = (paggingIndex - 1) * paggingSize;
            int nextNullItem = (paggingIndex - 1) * paggingSize + serverDataTable.Rows.Count;

            for (int i = 0; i < prevNullItem; ++i)
            {
                DataRow emptydr = clientDataTable.NewRow();
                clientDataTable.Rows.Add(emptydr);
            }

            for (int i = 0; i < serverDataTable.Rows.Count; ++i)
            {
                DataRow mydr;
                mydr = clientDataTable.NewRow();
                mydr.ItemArray = serverDataTable.Rows[i].ItemArray;
                clientDataTable.Rows.Add(mydr);
            }

            for (int i = nextNullItem; i < totalRecord; ++i)
            {
                DataRow emptydr = clientDataTable.NewRow();
                clientDataTable.Rows.Add(emptydr);
            }

            return clientDataTable;
        }

        public static DataSet ToDataSet<T>(this IList<T> list)
        {
            Type elementType = typeof(T);
            DataSet ds = new DataSet();
            DataTable t = new DataTable();
            ds.Tables.Add(t);

            //add a column to table for each public property on T
            foreach (var propInfo in elementType.GetProperties())
            {
                t.Columns.Add(propInfo.Name, propInfo.PropertyType);
            }

            //go through each property on T and add each value to the table
            foreach (T item in list)
            {
                DataRow row = t.NewRow();
                foreach (var propInfo in elementType.GetProperties())
                {
                    row[propInfo.Name] = propInfo.GetValue(item, null);
                }
                t.Rows.Add(row);
            }

            return ds;
        }
        #endregion

        #region Thiết lập Icon
        public static void setIcon(Window thisWindow)
        {
            Uri iconUri = new Uri(ClientInformation.IconName, UriKind.Relative);
            thisWindow.Icon = BitmapFrame.Create(iconUri);
        }
        #endregion

        #region Data Validation
        public static bool IsValidFormat(
            ApplicationConstant.FormatType formatType,
            string dataValue)
        {
            bool kq = true;
            string regexPattern = "";
            ApplicationConstant.DonViSuDung company = ApplicationConstant.layDonViSuDung(ClientInformation.Company);

            if (formatType.Equals(ApplicationConstant.FormatType.EMAIL))
            {
                regexPattern =
                        @"^([0-9a-zA-Z]" + //Start with a digit or alphabate
                        @"([\+\-_\.][0-9a-zA-Z]+)*" + // No continues or ending +-_. chars in email
                        @")+" +
                        @"@(([0-9a-zA-Z][-\w]*[0-9a-zA-Z]*\.)+[a-zA-Z0-9]{2,17})$";

                if (!LObject.IsNullOrEmpty(dataValue))
                {
                    if (Regex.IsMatch(dataValue, regexPattern) == false)
                    {
                        LMessage.ShowMessage("M.ResponseMessage.Common.InvalidEmailFormat", LMessage.MessageBoxType.Warning);
                        return false;
                    }
                }
            }
            if (formatType.Equals(ApplicationConstant.FormatType.PHONE))
            {
                if (company.Equals(ApplicationConstant.DonViSuDung.BIDV) 
                    || company.Equals(ApplicationConstant.DonViSuDung.BIDV_BLF))
                    regexPattern =
                            @"^[0-9]+$"; // any number
                else
                    regexPattern =
                        @"^[0-9]{9,11}$"; // Start with 0, length 9 - 11

                if (!LObject.IsNullOrEmpty(dataValue))
                {
                    if (Regex.IsMatch(dataValue, regexPattern) == false)
                    {
                        LMessage.ShowMessage("M.ResponseMessage.Common.InvalidPhoneFormat", LMessage.MessageBoxType.Warning);
                        return false;
                    }
                }

            }
            if (formatType.Equals(ApplicationConstant.FormatType.CMND))
            {
                if (company.Equals(ApplicationConstant.DonViSuDung.BIDV))
                    regexPattern =
                        //@"^[0-9]+$"; // any number
                        @"^([0-9])+" +
                        @"(\/)+" +
                        @"([A-Z])+" +
                        @"(\()+" +
                        @"([A-Z])+" +
                        @"(\))+" +
                        @"([0-9])+$";
                else if(company.Equals(ApplicationConstant.DonViSuDung.BIDV_BLF))
                    regexPattern = "";
                else
                    regexPattern =
                        @"^[0-9]{9,11}$"; // Start with 0, length 9 - 11

                if (!LObject.IsNullOrEmpty(dataValue))
                {
                    if (Regex.IsMatch(dataValue, regexPattern) == false)
                    {
                        LMessage.ShowMessage("M.ResponseMessage.Common.InvalidCMNDFormat", LMessage.MessageBoxType.Warning);
                        return false;
                    }
                }
            }
            if (formatType.Equals(ApplicationConstant.FormatType.HOKHAU))
            {
            }
            else
            {
            }

            return kq;
        }
        #endregion
    }
}
