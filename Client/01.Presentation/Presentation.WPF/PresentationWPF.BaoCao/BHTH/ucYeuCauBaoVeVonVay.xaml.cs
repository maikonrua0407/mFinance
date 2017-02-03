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
using PresentationWPF.CustomControl;
using Utilities.Common;
using Presentation.Process.Common;
using Presentation.Process.PopupServiceRef;
using Presentation.Process;

namespace PresentationWPF.BaoCao.BHTH
{
    /// <summary>
    /// Interaction logic for ucYeuCauBaoVeVonVay.xaml
    /// </summary>
    public partial class ucYeuCauBaoVeVonVay : UserControl
    {
        // Các tham số báo cáo từ form điều kiện
        List<AutoCompleteEntry> lstSourceChiNhanh = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourcePhongGD = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourcePhongGD_Select = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceLoaiTien = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceCum = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceCum_Select = new List<AutoCompleteEntry>();
        DataTable dtKheUoc = new DataTable();
        List<DataRow> lstPopup = null;
        public void LayDuLieuTuPopup(List<DataRow> lst)
        {
            lstPopup = lst;
        } 
        public ucYeuCauBaoVeVonVay()
        {
            InitializeComponent();
            KhoiTaoComboBox();
        }

        private void KhoiTaoComboBox()
        {
            // khởi tạo combobox
            AutoComboBox auto = new AutoComboBox();
            // Khởi tạo điều kiện gọi danh mục
            List<string> lstDieuKien = new List<string>();
            //LoadCombobox(); ???
            // Nếu người dùng là đơn vị >> disable thông tin chi nhánh
            if (ClientInformation.LoaiNguoiSuDung.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_QTDV.layGiaTri()) ||
                ClientInformation.LoaiNguoiSuDung.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_NVDV.layGiaTri()))
            {
                cmbChiNhanh.IsEnabled = false;
            }
            else
            {
                cmbChiNhanh.IsEnabled = true;
            }
            // khởi tạo combobox
            lstSourceChiNhanh.Insert(0, new AutoCompleteEntry(LLanguage.SearchResourceByKey("U.DungChung.TatCa"), new string[2] { "%", "0" }));
            auto = new AutoComboBox();
            auto.GenAutoComboBox(ref lstSourceChiNhanh, ref cmbChiNhanh, DatabaseConstant.DanhSachTruyVan.COMBOBOX_CNHANH.getValue(), null, ClientInformation.MaDonVi);

            // khởi tạo combobox
            auto = new AutoComboBox();
            auto.GenAutoComboBox(ref lstSourcePhongGD, ref cmbPhongGD, DatabaseConstant.DanhSachTruyVan.COMBOBOX_PHONG_GD.getValue());
            auto.GenAutoComboBox(ref lstSourceCum, ref cmbCum, DatabaseConstant.DanhSachTruyVan.COMBOBOX_CUM_ALL.getValue());
            LoadComboboxPhongGD();
            LoadComboboxCum();

        }

        private void LoadComboboxPhongGD()
        {
            // khởi tạo combobox
            AutoComboBox auto = new AutoComboBox();

            lstSourcePhongGD_Select = new List<AutoCompleteEntry>();
            string maChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.First();
            lstSourcePhongGD_Select = lstSourcePhongGD.Where(e => e.KeywordStrings.ElementAt(1).Equals(maChiNhanh)).ToList();
            lstSourcePhongGD_Select.Insert(0, new AutoCompleteEntry(LLanguage.SearchResourceByKey("U.DungChung.TatCa"), new string[2] { "%", "0" }));
            // khởi tạo combobox
            auto = new AutoComboBox();
            cmbPhongGD.Items.Clear();
            auto.GenAutoComboBox(ref lstSourcePhongGD_Select, ref cmbPhongGD, null);
            cmbPhongGD.SelectedIndex = 0;
            cmbPhongGD.IsEnabled = !maChiNhanh.Equals("%");
        }

        private void LoadComboboxCum()
        {
            // khởi tạo combobox
            AutoComboBox auto = new AutoComboBox();

            lstSourceCum_Select = new List<AutoCompleteEntry>();
            string maChiNhanh = lstSourceChiNhanh.ElementAt(cmbChiNhanh.SelectedIndex).KeywordStrings.First();
            string maPhongGiaoDich = lstSourcePhongGD_Select.ElementAt(cmbPhongGD.SelectedIndex).KeywordStrings.First();
            if (maPhongGiaoDich == "%")
                maPhongGiaoDich = maChiNhanh;
            lstSourceCum_Select = lstSourceCum.Where(e => e.KeywordStrings.ElementAt(1).Substring(0, maPhongGiaoDich.Length).Equals(maPhongGiaoDich)).ToList();
            lstSourceCum_Select.Insert(0, new AutoCompleteEntry(LLanguage.SearchResourceByKey("U.DungChung.TatCa"), new string[2] { "%", "0" }));
            // khởi tạo combobox
            auto = new AutoComboBox();
            cmbCum.Items.Clear();
            auto.GenAutoComboBox(ref lstSourceCum_Select, ref cmbCum, null);
            cmbCum.SelectedIndex = 0;
            cmbCum.SelectedIndex = 0;
            cmbCum.IsEnabled = !maPhongGiaoDich.Equals("%");
        }

        private void tlbDetailAdd_Click(object sender, RoutedEventArgs e)
        {
            Cursor = Cursors.Wait;
            try
            {
                string sidKheUoc = "";
                if (lstPopup.IsNullOrEmpty())
                {
                    foreach (DataRow dr in lstPopup)
                    {
                        sidKheUoc += "," + dr["ID"].ToString();
                    }
                }
                if (sidKheUoc.Length > 0)
                    sidKheUoc = sidKheUoc.Substring(1);
                else
                    sidKheUoc = "0";
                AutoCompleteEntry auCum = lstSourceCum_Select.ElementAt(cmbCum.SelectedIndex);
                AutoCompleteEntry auPGD = lstSourcePhongGD_Select.ElementAt(cmbPhongGD.SelectedIndex);
                List<string> lstDieuKien = new List<string>();
                lstDieuKien.Add(sidKheUoc);
                lstDieuKien.Add(auPGD.KeywordStrings.FirstOrDefault());
                lstDieuKien.Add(auCum.KeywordStrings.FirstOrDefault());
                lstPopup = new List<DataRow>();
                PopupProcess popupProcess = new PopupProcess();
                popupProcess.getPopupInformation("POPUP_DS_KHEUOC_BH", lstDieuKien);
                SimplePopupResponse simplePopupResponse = ClientInformation.SimplePopup;
                ucPopup popup = new ucPopup(true, simplePopupResponse, true);
                popup.DuLieuTraVe = new ucPopup.LayDuLieu(LayDuLieuTuPopup);
                Window win = new Window();
                win.Title = LLanguage.SearchResourceByKey(simplePopupResponse.PopupTitle);
                win.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                win.Content = popup;
                win.ShowDialog();
                if (lstPopup.Count > 0)
                {
                    foreach (DataRow drv in lstPopup)
                    {
                        dtKheUoc.Rows.Add(drv);
                    }
                }
                raddgrTUngCT.ItemsSource = null;
                raddgrTUngCT.ItemsSource = dtKheUoc;
            }
            catch (Exception ex)
            {
                LMessage.ShowMessage("M.DungChung.LoiChung", LMessage.MessageBoxType.Error);
                LLogging.WriteLog(ex.TargetSite.Name,LLogging.LogType.ERR,ex);
            }
            Cursor = Cursors.Arrow;
        }

        private void tlbDetailDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cmbChiNhanh_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadComboboxPhongGD();
        }

        private void cmbPhongGD_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbPhongGD.SelectedIndex >= 0)
                LoadComboboxCum();
        }
    }
}
