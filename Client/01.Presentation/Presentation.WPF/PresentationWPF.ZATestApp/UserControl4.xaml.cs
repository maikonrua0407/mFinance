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
using Telerik.Windows.Controls;
namespace PresentationWPF.ZATestApp
{
    /// <summary>
    /// Interaction logic for UserControl4.xaml
    /// </summary>
    public partial class UserControl4 : UserControl
    {
        public UserControl4()
        {
            InitializeComponent();
        }
        DataTable dt;
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            page.PageSize = (int)nudPageSize.Value;
            ragrdKhangHang.ItemsSource = LayDuLieu().DefaultView;


            ((GridViewComboBoxColumn)this.ragrdKhangHang.Columns[2]).ItemsSource = LayGioiTinh().DefaultView;

        }
        private DataTable LayDuLieu()
        {
            dt = new DataTable();
            dt.Columns.Add("MAKH", typeof(int));
            dt.Columns.Add("TENKH", typeof(string));
            dt.Columns.Add("GIOITINH", typeof(int));
            dt.Columns.Add("BAOHIEM", typeof(bool));
            for (int i = 1; i < 100; i++)
            {
                dt.NewRow();
                dt.Rows.Add(i, "Khách hàng" + i.ToString(), i % 2,false);

            }
            return dt;
        }
        private DataTable LayGioiTinh()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID");
            dt.Columns.Add("Name");
            dt.Rows.Add(0, "Nam");
            dt.Rows.Add(1, "Nữ");
            return dt;
        }

        private void GridViewCheckBoxColumn_MouseLeave(object sender, MouseEventArgs e)
        {
            MessageBox.Show(ragrdKhangHang.CurrentColumn.ToString());
        }

        private void ragrdKhangHang_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            //MessageBox.Show(ragrdKhangHang.CurrentCell.ParentRow.ToString());
            //ragrdKhangHang.Columns[6].Width = 40;
            //DataRow r = (DataRow)ragrdKhangHang.SelectedItem;
            //r[0].ToString();
        }

        private void RadNumericUpDown_ValueChanged(object sender, RadRangeBaseValueChangedEventArgs e)
        {
            if (dt != null)
            {
                page.PageSize = (int)nudPageSize.Value;
                ragrdKhangHang.ItemsSource = dt.DefaultView;
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            //PresentationWPF.CustomControl.CommonFunction.ExportGridToExcel("GET_DS_DONVI");
        }        
    }
    
}
