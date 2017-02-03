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
using System.Collections.ObjectModel;
using Utilities.Common;

namespace PresentationWPF.CustomControl
{
    /// <summary>
    /// Control dùng để hiển thị và chọn các trạng thái nghiệp vụ
    /// </summary>
    public partial class CheckboxListTrangThaiNghiepVu : UserControl
    {
        public ObservableCollection<TrangThaiNghiepVu> TheList { get; set; }
        public CheckboxListTrangThaiNghiepVu()
        {
            InitializeComponent(); 
            // Khởi tạo control
            CreateCheckBoxList();
        }

        /// <summary>
        /// Lấy danh sách các item đã chọn
        /// </summary>
        /// <returns>List<string> value(ID,...) của các items đã chọn</returns>
        public string GetItemsSelected()
        {
            string str = string.Empty;
            foreach (var item in listBoxZone.Items)
            {
                ListBoxItem lbi = (ListBoxItem)(listBoxZone.ItemContainerGenerator.ContainerFromItem(item));
                if (lbi != null)
                {
                    if (lbi.IsSelected)
                    {
                        TrangThaiNghiepVu bsc = (TrangThaiNghiepVu)item;
                        if (!string.IsNullOrEmpty(bsc.TrangThai.ToString()) && bsc.TrangThai.ToString().Equals("") != true)
                            str = str + ",''" + bsc.TrangThai.ToString() + "''";
                    }
                }
            }
            if (!string.IsNullOrEmpty(str))
                str = "(" + str.Substring(1, str.Length - 1) + ")";
            else
                str = "NULL";
            return str;
        }
        /// <summary>
        /// Render các item cho checklist
        /// </summary>
        public void CreateCheckBoxList()
        {
            TheList = new ObservableCollection<TrangThaiNghiepVu>();
            //TheList.Add(new TrangThaiNghiepVu { TenTrangThai = LLanguage.SearchResourceByKey("U.DungChung.TrangThaiNghiepVu.LuuTam"), TrangThai = BusinessConstant.TrangThaiNghiepVu.LUU_TAM.layGiaTri() });
            TheList.Add(new TrangThaiNghiepVu { TenTrangThai = LLanguage.SearchResourceByKey("U.DungChung.TrangThaiNghiepVu.ChoDuyet"), TrangThai = BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri() });
            //TheList.Add(new TrangThaiNghiepVu { TenTrangThai = LLanguage.SearchResourceByKey("U.DungChung.TrangThaiNghiepVu.SuaSauDuyet"), TrangThai = BusinessConstant.TrangThaiNghiepVu.SUA_SAU_DUYET.layGiaTri() });
            TheList.Add(new TrangThaiNghiepVu { TenTrangThai = LLanguage.SearchResourceByKey("U.DungChung.TrangThaiNghiepVu.DaDuyet"), TrangThai = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri() });
            TheList.Add(new TrangThaiNghiepVu { TenTrangThai = LLanguage.SearchResourceByKey("U.DungChung.TrangThaiNghiepVu.TuChoi"), TrangThai = BusinessConstant.TrangThaiNghiepVu.TU_CHOI.layGiaTri() });
            TheList.Add(new TrangThaiNghiepVu { TenTrangThai = LLanguage.SearchResourceByKey("U.DungChung.TrangThaiNghiepVu.ThoaiDuyet"), TrangThai = BusinessConstant.TrangThaiNghiepVu.THOAI_DUYET.layGiaTri() });
            //TheList.Add(new TrangThaiNghiepVu { TenTrangThai = LLanguage.SearchResourceByKey("U.DungChung.TrangThaiNghiepVu.TuChoiCapTinDung"), TrangThai = BusinessConstant.TrangThaiNghiepVu.TU_CHOI_CAP_TIN_DUNG.layGiaTri() });
            this.DataContext = this;
        }

        private void cbAll_Checked(object sender, RoutedEventArgs e)
        {
            foreach (var item in listBoxZone.Items)
            {
                ListBoxItem lbi = (ListBoxItem)(listBoxZone.ItemContainerGenerator.ContainerFromItem(item));
                lbi.IsSelected = true;
            }
        }

        private void cbAll_Unchecked(object sender, RoutedEventArgs e)
        {
            foreach (var item in listBoxZone.Items)
            {
                ListBoxItem lbi = (ListBoxItem)(listBoxZone.ItemContainerGenerator.ContainerFromItem(item));
                lbi.IsSelected = false;
            }
        }
    }

    /// <summary>
    /// Khởi tạo lớp Trạng thái nghiệp vụ
    /// </summary>
    public class TrangThaiNghiepVu
    {
        public string TenTrangThai { get; set; }
        public string TrangThai { get; set; }
    }
}
