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
using Microsoft.Windows.Controls.Ribbon;
using Utilities.Common;
using System.Data;
using Presentation.Process;
using PresentationWPF.CustomControl;
using Presentation.Process.QuanTriHeThongServiceRef;
using System.Reflection;
using System.Collections;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using Presentation.Process.Common;

namespace PresentationWPF.QuanTriHeThong.PhanQuyen
{
    /// <summary>
    /// Interaction logic for ucPhanQuyenCN.xaml
    /// </summary>
    public partial class ucPhanQuyenBC : UserControl
    {
        #region Khai bao

        List<AutoCompleteEntry> lstSourcePhanHe = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceLoaiDTuong = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceLoaiTNguyen = new List<AutoCompleteEntry>();

        List<HT_NSD> dsNSD = new List<HT_NSD>();
        List<HT_NHNSD> dsNHNSD = new List<HT_NHNSD>();

        List<HT_TNANG> dsTNangAll = new List<HT_TNANG>();
        List<HT_CNANG_TNANG> dsCNangTNangAll = new List<HT_CNANG_TNANG>();
        List<HT_CNANG> dsCNangAll = new List<HT_CNANG>();

        List<HT_CNANG_PQUYEN> dsCNangPQuyenDoiTuong = new List<HT_CNANG_PQUYEN>();
        List<HT_CNANG_TNANG> dsCNangTNangDoiTuong = new List<HT_CNANG_TNANG>();

        DataTable dt = new DataTable();
        QuanTriHeThongProcess qtht = new QuanTriHeThongProcess();
        bool isLoaded = false;

        public static RoutedCommand SaveCommand = new RoutedCommand();
        public static RoutedCommand ExportCommand = new RoutedCommand();
        public static RoutedCommand HelpCommand = new RoutedCommand();
        public static RoutedCommand CloseCommand = new RoutedCommand();


        #endregion

        #region Khoi tao

        public ucPhanQuyenBC()
        {
            InitializeComponent();
            LoadDuLieu();
            BindShortkey();
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
                int idDoiTuong = 0;
                string maDoiTuong = null;
                string maDonVi = null;
                loaiDoiTuong = lstSourceLoaiDTuong.ElementAt(cmbDoiTuong.SelectedIndex).KeywordStrings.First();
                DataRow dr = (DataRow)grDSDoiTuong.SelectedItem;
                if (dr != null)
                {
                    idDoiTuong = int.Parse(dr["ID"].ToString());
                    maDoiTuong = dr[2].ToString();
                    maDonVi = dr[5].ToString();
                }
                else
                {
                    LMessage.ShowMessage("Choose object for role setting", LMessage.MessageBoxType.Warning);
                    return;
                }

                // Khởi tạo biến ArrayList để chứa nội dung cập nhật phân quyền
                ArrayList lstCheckBox = new ArrayList();

                // Đọc dữ liệu trên lưới đưa vào dữ liệu cập nhật
                foreach (int id in dsTNangAll.Select(e => e.ID))
                {
                    // Nếu là phân quyền tính năng, 
                    // ngoài việc khởi tạo List thì gán ID tính năng vào item đầu tiên của List
                    List<string> lstItem = new List<string>();
                    lstItem.Add(id.ToString());
                    lstCheckBox.Add(lstItem);
                }
                foreach (DataRow r in dt.Rows)
                {
                    // Lấy vị trí của fullcontrol
                    int fullControllPos = 3 + dsTNangAll.Count - 1;

                    if ((bool)r[fullControllPos] == true)
                    {
                        ((List<string>)lstCheckBox[dsTNangAll.Count - 1]).Add(r[1].ToString());
                    }
                    else
                    {
                        for (int i = 0; i < dsTNangAll.Count; i++)
                        {
                            if (r[i + 3] != null && r[i + 3].ToString() != "")
                            {
                                if ((bool)r[i + 3] == true)
                                    ((List<string>)lstCheckBox[i]).Add(r[1].ToString());
                            }
                        }
                    }
                }

                if (lstCheckBox != null)
                {
                    // Tìm các thông tin CNangTNang được check cho phân quyền
                    List<HT_CNANG_TNANG> dsCNangTNangCheckBox = new List<HT_CNANG_TNANG>();
                    foreach (HT_CNANG_TNANG item in dsCNangTNangAll)
                    {
                        for (int i = 0; i < dsTNangAll.Count; i++)
                        {
                            List<string> listCNangByTNang = (List<string>)lstCheckBox[i];
                            for (int j = 0; j < listCNangByTNang.Count; j++)
                            {
                                if (listCNangByTNang[j].Equals(item.MA_CNANG) && item.ID_TNANG == dsTNangAll[i].ID)
                                {
                                    dsCNangTNangCheckBox.Add(item);
                                }
                            }
                        }
                    }
                    // Tìm các thông tin được thêm và xóa
                    List<HT_CNANG_TNANG> dsCNangTNangThem = new List<HT_CNANG_TNANG>();
                    List<HT_CNANG_PQUYEN> dsCNangPQuyenXoa = new List<HT_CNANG_PQUYEN>();
                    List<int> lstIdCNangPQuyenDoiTuong = dsCNangPQuyenDoiTuong.Select(e => e.ID_CNANG_TNANG.Value).ToList();
                    List<int> lstIdCNangTNangCheckBox = dsCNangTNangCheckBox.Select(e => e.ID).ToList();
                    foreach (HT_CNANG_TNANG item in dsCNangTNangCheckBox)
                    {
                        if (!lstIdCNangPQuyenDoiTuong.Contains(item.ID))
                            dsCNangTNangThem.Add(item);
                    }
                    foreach (HT_CNANG_PQUYEN item in dsCNangPQuyenDoiTuong)
                    {
                        if (!lstIdCNangTNangCheckBox.Contains(item.ID_CNANG_TNANG.Value))
                            dsCNangPQuyenXoa.Add(item);
                    }

                    if ((dsCNangTNangThem != null &&
                        dsCNangTNangThem.Count > 0) ||
                        (dsCNangPQuyenXoa != null &&
                        dsCNangPQuyenXoa.Count > 0))
                    {
                        // Thực hiện lưu phân quyền
                        bool ret = qtht.luuPhanQuyenChucNang(
                            loaiDoiTuong,
                            idDoiTuong,
                            maDoiTuong,
                            maDonVi,
                            dsCNangPQuyenXoa,
                            dsCNangTNangThem);
                        if (ret)
                            LMessage.ShowMessage("M.DungChung.LuuDuLieuThanhCong", LMessage.MessageBoxType.Information);
                        else
                            LMessage.ShowMessage("M.DungChung.LuuDuLieuKhongThanhCong", LMessage.MessageBoxType.Warning);
                    }
                    else
                    {
                        LMessage.ShowMessage("No changed data for role setting", LMessage.MessageBoxType.Information);
                    }
                }
                else
                    LMessage.ShowMessage("No changed data for role setting", LMessage.MessageBoxType.Warning);
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
            PresentationWPF.CustomControl.CommonFunction.ExportGridToExcel(grDanhSach);
        }
        #endregion

        #region Xu ly giao dien

        private void LoadDuLieu()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                var names = Enum.GetNames(typeof(DatabaseConstant.PhanHeKhaiThacDuLieu));
                foreach (string name in names)
                {
                    // do something!
                    int idItem = (int)DatabaseConstant.getValuePhanHeKhaiThacDuLieu(name);
                    string codeItem = name;
                    string nameItem = LLanguage.SearchResourceByKey(DatabaseConstant.getLanguagePhanHeKhaiThacDuLieu(DatabaseConstant.getValuePhanHeKhaiThacDuLieu(name)));
                    AutoCompleteEntry entry = new AutoCompleteEntry(nameItem, codeItem, idItem.ToString());
                    lstSourcePhanHe.Add(entry);
                    cmbPhanHeChucNang.Items.Add(entry);
                }
                // Nếu là Bình Khánh
                if (ClientInformation.Company.Equals(ApplicationConstant.DonViSuDung.BINHKHANH.layGiaTri()))
                {
                    foreach (string name in names)
                    {
                        if (name.Equals(DatabaseConstant.PhanHeKhaiThacDuLieu.KTDL_QTHT.getStringPhanHeKhaiThacDuLieu())
                            || name.Equals(DatabaseConstant.PhanHeKhaiThacDuLieu.KTDL_KHTV.getStringPhanHeKhaiThacDuLieu())
                            || name.Equals(DatabaseConstant.PhanHeKhaiThacDuLieu.KTDL_GDKT.getStringPhanHeKhaiThacDuLieu())
                            || name.Equals(DatabaseConstant.PhanHeKhaiThacDuLieu.KTDL_BHTH.getStringPhanHeKhaiThacDuLieu())
                            || name.Equals(DatabaseConstant.PhanHeKhaiThacDuLieu.KTDL_NSTL.getStringPhanHeKhaiThacDuLieu())
                            || name.Equals(DatabaseConstant.PhanHeKhaiThacDuLieu.KTDL_BCTK.getStringPhanHeKhaiThacDuLieu())
                            || name.Equals(DatabaseConstant.PhanHeKhaiThacDuLieu.KTDL_QLTS.getStringPhanHeKhaiThacDuLieu())
                            || name.Equals(DatabaseConstant.PhanHeKhaiThacDuLieu.KTDL_BCTC.getStringPhanHeKhaiThacDuLieu())
                            || name.Equals(DatabaseConstant.PhanHeKhaiThacDuLieu.KTDL_CICR.getStringPhanHeKhaiThacDuLieu())
                            || name.Equals(DatabaseConstant.PhanHeKhaiThacDuLieu.KTDL_BHTG.getStringPhanHeKhaiThacDuLieu()))
                        {
                            // do something!
                            AutoComboBox autoPhanHe = new AutoComboBox();
                            autoPhanHe.removeEntry(ref lstSourcePhanHe, ref cmbPhanHeChucNang, name);
                        }
                    }
                    cmbPhanHeChucNang.SelectedIndex = lstSourcePhanHe.IndexOf(lstSourcePhanHe.FirstOrDefault(e => e.KeywordStrings.First().Equals(DatabaseConstant.PhanHeKhaiThacDuLieu.KTDL_HDVO.getStringPhanHeKhaiThacDuLieu())));
                }
                else if (ClientInformation.Company.Equals(ApplicationConstant.DonViSuDung.M7MFI.layGiaTri()))
                {
                    foreach (string name in names)
                    {
                        if (name.Equals(DatabaseConstant.PhanHeKhaiThacDuLieu.KTDL_QLTS.getStringPhanHeKhaiThacDuLieu())
                            || name.Equals(DatabaseConstant.PhanHeKhaiThacDuLieu.KTDL_BCTC.getStringPhanHeKhaiThacDuLieu())
                            || name.Equals(DatabaseConstant.PhanHeKhaiThacDuLieu.KTDL_CICR.getStringPhanHeKhaiThacDuLieu())
                            || name.Equals(DatabaseConstant.PhanHeKhaiThacDuLieu.KTDL_BHTG.getStringPhanHeKhaiThacDuLieu()))
                        {
                            // do something!
                            AutoComboBox autoPhanHe = new AutoComboBox();
                            autoPhanHe.removeEntry(ref lstSourcePhanHe, ref cmbPhanHeChucNang, name);
                        }
                    }
                    cmbPhanHeChucNang.SelectedIndex = lstSourcePhanHe.IndexOf(lstSourcePhanHe.FirstOrDefault(e => e.KeywordStrings.First().Equals(DatabaseConstant.PhanHeKhaiThacDuLieu.KTDL_KHTV.getStringPhanHeKhaiThacDuLieu())));
                }
                else if (ClientInformation.Company.Equals(ApplicationConstant.DonViSuDung.BANTAYVANG.layGiaTri()))
                {
                    foreach (string name in names)
                    {
                        if (name.Equals(DatabaseConstant.PhanHeKhaiThacDuLieu.KTDL_QTHT.getStringPhanHeKhaiThacDuLieu())
                            || name.Equals(DatabaseConstant.PhanHeKhaiThacDuLieu.KTDL_BHTH.getStringPhanHeKhaiThacDuLieu())
                            || name.Equals(DatabaseConstant.PhanHeKhaiThacDuLieu.KTDL_BCTK.getStringPhanHeKhaiThacDuLieu())
                            || name.Equals(DatabaseConstant.PhanHeKhaiThacDuLieu.KTDL_CICR.getStringPhanHeKhaiThacDuLieu())
                            || name.Equals(DatabaseConstant.PhanHeKhaiThacDuLieu.KTDL_BHTG.getStringPhanHeKhaiThacDuLieu()))
                        {
                            // do something!
                            AutoComboBox autoPhanHe = new AutoComboBox();
                            autoPhanHe.removeEntry(ref lstSourcePhanHe, ref cmbPhanHeChucNang, name);
                        }
                    }
                    cmbPhanHeChucNang.SelectedIndex = lstSourcePhanHe.IndexOf(lstSourcePhanHe.FirstOrDefault(e => e.KeywordStrings.First().Equals(DatabaseConstant.PhanHeKhaiThacDuLieu.KTDL_KHTV.getStringPhanHeKhaiThacDuLieu())));
                }
                if (ClientInformation.Company.Equals(ApplicationConstant.DonViSuDung.BIDV.layGiaTri()))
                {
                    foreach (string name in names)
                    {
                        if (name.Equals(DatabaseConstant.PhanHeKhaiThacDuLieu.KTDL_QTHT.getStringPhanHeKhaiThacDuLieu())
                            || name.Equals(DatabaseConstant.PhanHeKhaiThacDuLieu.KTDL_BHTH.getStringPhanHeKhaiThacDuLieu())
                            || name.Equals(DatabaseConstant.PhanHeKhaiThacDuLieu.KTDL_NSTL.getStringPhanHeKhaiThacDuLieu())                            
                            || name.Equals(DatabaseConstant.PhanHeKhaiThacDuLieu.KTDL_QLTS.getStringPhanHeKhaiThacDuLieu())
                            //|| name.Equals(DatabaseConstant.PhanHeKhaiThacDuLieu.KTDL_BCTK.getStringPhanHeKhaiThacDuLieu())
                            //|| name.Equals(DatabaseConstant.PhanHeKhaiThacDuLieu.KTDL_BCTH.getStringPhanHeKhaiThacDuLieu())
                            || name.Equals(DatabaseConstant.PhanHeKhaiThacDuLieu.KTDL_BCTC.getStringPhanHeKhaiThacDuLieu())
                            || name.Equals(DatabaseConstant.PhanHeKhaiThacDuLieu.KTDL_CICR.getStringPhanHeKhaiThacDuLieu())
                            || name.Equals(DatabaseConstant.PhanHeKhaiThacDuLieu.KTDL_BHTG.getStringPhanHeKhaiThacDuLieu()))
                        {
                            // do something!
                            AutoComboBox autoPhanHe = new AutoComboBox();
                            autoPhanHe.removeEntry(ref lstSourcePhanHe, ref cmbPhanHeChucNang, name);
                        }
                    }
                    cmbPhanHeChucNang.SelectedIndex = lstSourcePhanHe.IndexOf(lstSourcePhanHe.FirstOrDefault(e => e.KeywordStrings.First().Equals(DatabaseConstant.PhanHeKhaiThacDuLieu.KTDL_HDVO.getStringPhanHeKhaiThacDuLieu())));
                }
                if (ClientInformation.Company.Equals(ApplicationConstant.DonViSuDung.PHUTHO.layGiaTri()))
                {
                    foreach (string name in names)
                    {
                        if (name.Equals(DatabaseConstant.PhanHeKhaiThacDuLieu.KTDL_QTHT.getStringPhanHeKhaiThacDuLieu())
                            || name.Equals(DatabaseConstant.PhanHeKhaiThacDuLieu.KTDL_TDTT.getStringPhanHeKhaiThacDuLieu())
                            || name.Equals(DatabaseConstant.PhanHeKhaiThacDuLieu.KTDL_BHTH.getStringPhanHeKhaiThacDuLieu())
                            || name.Equals(DatabaseConstant.PhanHeKhaiThacDuLieu.KTDL_NSTL.getStringPhanHeKhaiThacDuLieu())
                            || name.Equals(DatabaseConstant.PhanHeKhaiThacDuLieu.KTDL_QLTS.getStringPhanHeKhaiThacDuLieu())
                            || name.Equals(DatabaseConstant.PhanHeKhaiThacDuLieu.KTDL_BCTK.getStringPhanHeKhaiThacDuLieu())
                            || name.Equals(DatabaseConstant.PhanHeKhaiThacDuLieu.KTDL_CICR.getStringPhanHeKhaiThacDuLieu())
                            || name.Equals(DatabaseConstant.PhanHeKhaiThacDuLieu.KTDL_BHTG.getStringPhanHeKhaiThacDuLieu()))
                        {
                            // do something!
                            AutoComboBox autoPhanHe = new AutoComboBox();
                            autoPhanHe.removeEntry(ref lstSourcePhanHe, ref cmbPhanHeChucNang, name);
                        }
                    }
                    cmbPhanHeChucNang.SelectedIndex = lstSourcePhanHe.IndexOf(lstSourcePhanHe.FirstOrDefault(e => e.KeywordStrings.First().Equals(DatabaseConstant.PhanHeKhaiThacDuLieu.KTDL_KHTV.getStringPhanHeKhaiThacDuLieu())));
                }
                else
                {
                    cmbPhanHeChucNang.SelectedIndex = lstSourcePhanHe.IndexOf(lstSourcePhanHe.FirstOrDefault(e => e.KeywordStrings.First().Equals(DatabaseConstant.PhanHeKhaiThacDuLieu.KTDL_KHTV.getStringPhanHeKhaiThacDuLieu())));
                }


                //// lấy dữ liệu đổ source cho combobox Phân hệ chức năng
                AutoComboBox auto = new AutoComboBox();
                List<string> lstDieuKien = new List<string>();
                //lstDieuKien.Add(DatabaseConstant.DanhMuc.DANH_MUC_PHAN_HE.getValue());
                ////lstSourcePhanHe.Add(new AutoCompleteEntry("Tất cả",""));
                //auto.GenAutoComboBox(ref lstSourcePhanHe, ref cmbPhanHeChucNang, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien);

                // lấy dữ liệu đổ source cho combobox Loại đối tượng
                lstDieuKien = new List<string>();
                lstDieuKien.Add(DatabaseConstant.DanhMuc.LOAI_DTUONG_KTHAC_TNGUYEN.getValue());
                auto.GenAutoComboBox(ref lstSourceLoaiDTuong, ref cmbDoiTuong, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien,
                    BusinessConstant.LoaiDoiTuong.NHOM_NGUOI_SDUNG.layGiaTri());

                // Hiển thị lưới dữ liệu đối tượng
                BuildGridDoiTuong();
                // Hiển thị lưới dữ liệu phân quyền
                BuildGrid();
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (!isLoaded)
            {
                // Khởi tạo các sự kiện cho control
                cmbDoiTuong.SelectionChanged += cmbDoiTuong_SelectionChanged;
                cmbPhanHeChucNang.SelectionChanged += cmbPhanHeChucNang_SelectionChanged;
                grDSDoiTuong.SelectionChanged += grDSDoiTuong_SelectionChanged;
                //grDanhSach.SelectionChanged += grDanhSach_SelectionChanged;
                //grDanhSach.SelectedCellsChanged += grDanhSach_SelectedCellsChanged;
                isLoaded = true;
            }
        }

        private void BuildGrid()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                // Lấy chức năng theo phân hệ
                string maPhanHe = lstSourcePhanHe.ElementAt(cmbPhanHeChucNang.SelectedIndex).KeywordStrings.First();

                dsCNangAll = qtht.layCNangTheoPhanHe(maPhanHe);

                // Lấy CNangTnang theo chức năng trong phân hệ
                List<int> lstIdChucNang = dsCNangAll.Select(e => e.ID).Distinct().ToList();
                dsCNangTNangAll = qtht.layCNangTNangTheoListIdChucNang(lstIdChucNang);

                // Khởi tạo Table source: STT, Mã, Tên 
                dt = new DataTable();
                dt.Columns.Add("STT", typeof(int));
                dt.Columns.Add("MA", typeof(string));
                dt.Columns.Add("CHUCNANG", typeof(string));

                // Lấy các tính năng được phân quyền trong hệ thống
                dsTNangAll = qtht.layTNangDuocPhanQuyen();

                // Khởi tạo Table source: Các cột tính năng được phân quyền trong hệ thống
                foreach (HT_TNANG item in dsTNangAll)
                {
                    dt.Columns.Add(item.MA_TNANG, typeof(bool));
                }

                // Uncheck toàn bộ các header
                View.IsChecked = false;
                Add.IsChecked = false;
                Modify.IsChecked = false;
                Delete.IsChecked = false;
                Approve.IsChecked = false;
                Cancel.IsChecked = false;
                Refuse.IsChecked = false;
                FullControl.IsChecked = false;

                int stt = 0;
                if (grDSDoiTuong.SelectedItems.Count > 0)
                {
                    // Lấy loại đối tượng phân quyền là loại NSD hay NHNSD được chọn từ combo
                    string loaiDoiTuong = lstSourceLoaiDTuong.ElementAt(cmbDoiTuong.SelectedIndex).KeywordStrings.First();
                    // Lấy đối tượng phân quyền là NSD hay NHNSD được chọn từ grid
                    DataRow dr = (DataRow)grDSDoiTuong.SelectedItem;
                    string maDoiTuong = dr[2].ToString();

                    // Lấy dữ liệu phân quyền của đối tượng trong phân hệ được chọn
                    //dsCNangPQuyenDoiTuong = qtht.layCNangPQuyenTheoDoiTuong(maDoiTuong, loaiDoiTuong).ToList();
                    dsCNangPQuyenDoiTuong = qtht.layCNangPQuyenTheoDoiTuongChucNang(maDoiTuong, loaiDoiTuong, lstIdChucNang).ToList();

                    List<int> lstIdCNangTNang = dsCNangPQuyenDoiTuong.Select(e => e.ID_CNANG_TNANG.Value).ToList();
                    dsCNangTNangDoiTuong = dsCNangTNangAll.Where(e => lstIdCNangTNang.Contains(e.ID)).ToList();
                    foreach (HT_CNANG item in dsCNangAll)
                    {
                        // Lấy dữ liệu để đối chiếu và hiển thị các Chức năng, Tính năng đã hay chưa phân quyền
                        List<int> dsIdTNangAll = new List<int>();
                        dsIdTNangAll = dsCNangTNangAll.Where(e => e.ID_CNANG == item.ID).Select(e => e.ID_TNANG).ToList();

                        List<int> dsIdTNangDoiTuong = new List<int>();
                        dsIdTNangDoiTuong = dsCNangTNangDoiTuong.Where(e => e.ID_CNANG == item.ID).Select(e => e.ID_TNANG).ToList();

                        DataRow r = dt.NewRow();
                        bool isAdd = false;
                        // Lấy mã chức năng ...
                        HT_CNANG_TNANG htCNangTNangFull = dsCNangTNangDoiTuong.FirstOrDefault(e => e.ID_CNANG == item.ID && e.MA_TNANG.Equals("FullControl"));
                        // đối chiếu với source, nếu có thì hiển thị checked
                        for (int i = 0; i < dsTNangAll.Count; i++)
                        {

                            if (dsIdTNangAll.Contains(dsTNangAll[i].ID))
                            {
                                if (dsIdTNangDoiTuong.Contains(dsTNangAll[i].ID) || htCNangTNangFull != null)
                                    r[i + 3] = true;
                                else
                                    r[i + 3] = false;
                                isAdd = true;
                            }
                            else
                                r[i + 3] = DBNull.Value;
                        }
                        // add vào source
                        if (isAdd)
                        {
                            stt = stt + 1;
                            r[0] = stt;
                            r[1] = item.MA_CNANG;
                            r[2] = LLanguage.SearchResourceByKey(item.MA_NNGU);
                            dt.Rows.Add(r);
                        }
                    }
                }
                else
                {
                    foreach (var item in dsCNangAll)
                    {
                        // Lấy dữ liệu để đối chiếu và hiển thị các Chức năng, Tính năng đã hay chưa phân quyền
                        List<int> dsIdTNangAll = new List<int>();
                        dsIdTNangAll = dsCNangTNangAll.Where(e => e.ID_CNANG == item.ID).Select(e => e.ID_TNANG).ToList();

                        List<int> dsIdTNangDoiTuong = new List<int>();
                        dsIdTNangDoiTuong = dsCNangTNangDoiTuong.Where(e => e.ID_CNANG == item.ID).Select(e => e.ID_TNANG).ToList();

                        DataRow r = dt.NewRow();
                        bool isAdd = false;
                        // đối chiếu với source, nếu có thì hiển thị checked
                        for (int i = 0; i < dsTNangAll.Count; i++)
                        {
                            if (dsIdTNangAll.Contains(dsTNangAll[i].ID))
                            {
                                if (dsIdTNangDoiTuong.Contains(dsTNangAll[i].ID))
                                    r[i + 3] = true;
                                else
                                    r[i + 3] = false;
                                isAdd = true;
                            }
                            else
                                r[i + 3] = DBNull.Value;
                        }
                        // add vào source
                        if (isAdd)
                        {
                            stt = stt + 1;
                            r[0] = stt;
                            r[1] = item.MA_CNANG;
                            r[2] = LLanguage.SearchResourceByKey(item.MA_NNGU);
                            dt.Rows.Add(r);
                        }
                    }
                }

                // đổ source lên lưới
                grDanhSach.ItemsSource = null;
                grDanhSach.ItemsSource = dt.DefaultView;
            }
            catch (System.Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
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
                dtDoiTuong.Columns.Add(LLanguage.SearchResourceByKey("U.QuanTriHeThong.PhanQuyen.ucPhanQuyenBC.Ma"), typeof(string));
                dtDoiTuong.Columns.Add(LLanguage.SearchResourceByKey("U.QuanTriHeThong.PhanQuyen.ucPhanQuyenBC.Ten"), typeof(string));
                dtDoiTuong.Columns.Add(LLanguage.SearchResourceByKey("U.QuanTriHeThong.PhanQuyen.ucPhanQuyenBC.DonVi"), typeof(string));
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

        private void loadWidthColumn()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                for (int i = 0; i < grDanhSach.Columns.Count; i++)
                {
                    if (i == 0)
                    {
                        grDanhSach.Columns[i].Width = new Telerik.Windows.Controls.GridViewLength(40, Telerik.Windows.Controls.GridViewLengthUnitType.Pixel);
                        grDanhSach.Columns[i].IsReadOnly = true;
                    }
                    else if (i == 1)
                        grDanhSach.Columns[i].IsVisible = false;
                    else if (i == 2)
                    {
                        grDanhSach.Columns[i].Width = new Telerik.Windows.Controls.GridViewLength(1, Telerik.Windows.Controls.GridViewLengthUnitType.Star);
                        grDanhSach.Columns[i].IsReadOnly = true;
                    }
                    else
                    {
                        grDanhSach.Columns[i].Width = new Telerik.Windows.Controls.GridViewLength(1, Telerik.Windows.Controls.GridViewLengthUnitType.Auto);
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

        private void cmbPhanHeChucNang_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BuildGrid();
            loadWidthColumn();
        }

        private void grDanhSach_Loaded(object sender, RoutedEventArgs e)
        {
            loadWidthColumn();
        }

        private void cmbDoiTuong_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BuildGridDoiTuong();
            loadWidthColumnDoiTuong();

            //cmbPhanHeChucNang.SelectedIndex = lstSourcePhanHe.IndexOf(lstSourcePhanHe.FirstOrDefault(item => item.KeywordStrings.First().Equals(DatabaseConstant.PhanHeKhaiThacDuLieu.KTDL_QTHT.getStringPhanHeKhaiThacDuLieu())));
            if (ClientInformation.Company.Equals("BINHKHANH"))
            {
                cmbPhanHeChucNang.SelectedIndex = lstSourcePhanHe.IndexOf(lstSourcePhanHe.FirstOrDefault(item => item.KeywordStrings.First().Equals(DatabaseConstant.PhanHeKhaiThacDuLieu.KTDL_HDVO.getStringPhanHeKhaiThacDuLieu())));
            }
            else if (ClientInformation.Company.Equals("M7S"))
            {
                cmbPhanHeChucNang.SelectedIndex = lstSourcePhanHe.IndexOf(lstSourcePhanHe.FirstOrDefault(item => item.KeywordStrings.First().Equals(DatabaseConstant.PhanHeKhaiThacDuLieu.KTDL_KHTV.getStringPhanHeKhaiThacDuLieu())));
            }
            else
            {
                cmbPhanHeChucNang.SelectedIndex = lstSourcePhanHe.IndexOf(lstSourcePhanHe.FirstOrDefault(item => item.KeywordStrings.First().Equals(DatabaseConstant.PhanHeKhaiThacDuLieu.KTDL_KHTV.getStringPhanHeKhaiThacDuLieu())));
            }
            BuildGrid();
            loadWidthColumn();
        }

        private void grDSDoiTuong_Loaded(object sender, RoutedEventArgs e)
        {
            loadWidthColumnDoiTuong();
        }

        private void grDSDoiTuong_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            //cmbPhanHeChucNang.SelectedIndex = lstSourcePhanHe.IndexOf(lstSourcePhanHe.FirstOrDefault(item => item.KeywordStrings.First().Equals(DatabaseConstant.PhanHeKhaiThacDuLieu.KTDL_QTHT.getStringPhanHeKhaiThacDuLieu())));
            // Nếu là Bình Khánh
            if (ClientInformation.Company.Equals("BINHKHANH"))
            {
                cmbPhanHeChucNang.SelectedIndex = lstSourcePhanHe.IndexOf(lstSourcePhanHe.FirstOrDefault(item => item.KeywordStrings.First().Equals(DatabaseConstant.PhanHeKhaiThacDuLieu.KTDL_HDVO.getStringPhanHeKhaiThacDuLieu())));
            }
            else if (ClientInformation.Company.Equals("M7S"))
            {
                cmbPhanHeChucNang.SelectedIndex = lstSourcePhanHe.IndexOf(lstSourcePhanHe.FirstOrDefault(item => item.KeywordStrings.First().Equals(DatabaseConstant.PhanHeKhaiThacDuLieu.KTDL_KHTV.getStringPhanHeKhaiThacDuLieu())));
            }
            else
            {
                cmbPhanHeChucNang.SelectedIndex = lstSourcePhanHe.IndexOf(lstSourcePhanHe.FirstOrDefault(item => item.KeywordStrings.First().Equals(DatabaseConstant.PhanHeKhaiThacDuLieu.KTDL_KHTV.getStringPhanHeKhaiThacDuLieu())));
            }
            BuildGrid();
            loadWidthColumn();
        }

        private void cmbLoaiTaiNguyen_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BuildGrid();
            loadWidthColumn();
        }

        private void grDanhSach_SelectedCellsChanged(object sender, GridViewSelectedCellsChangedEventArgs e)
        {
            if (grDanhSach.CurrentCell != null)
                grDanhSach.CurrentCell.BeginEdit();
        }

        private void AllColumnCheckBox_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            if (checkBox != null)
            {
                if (checkBox.IsChecked.GetValueOrDefault())
                {
                    if (checkBox.Name.Equals("FullControl"))
                    {
                        foreach (DataRow r in dt.Rows)
                        {
                            for (int i = 3; i < dt.Columns.Count; i++)
                            {
                                string s = r[i].ToString();
                                if (!LString.IsNullOrEmptyOrSpace(s))
                                    r[i] = true;
                            }
                        }
                    }
                    else
                    {
                        foreach (DataRow r in dt.Rows)
                        {
                            string s = r[checkBox.Name].ToString();
                            if (!LString.IsNullOrEmptyOrSpace(s))
                                r[checkBox.Name] = true;
                        }
                    }
                }
                else
                {
                    if (checkBox.Name.Equals("FullControl"))
                    {
                        foreach (DataRow r in dt.Rows)
                        {
                            for (int i = 3; i < dt.Columns.Count; i++)
                            {
                                string s = r[i].ToString();
                                if (!LString.IsNullOrEmptyOrSpace(s))
                                    r[i] = false;
                            }
                        }
                    }
                    else
                    {
                        foreach (DataRow r in dt.Rows)
                        {
                            string s = r[checkBox.Name].ToString();
                            if (!LString.IsNullOrEmptyOrSpace(s))
                            {
                                r[checkBox.Name] = false;
                                // Bỏ check cho các checkbox toàn quyền con
                                r["FullControl"] = false;
                            }


                        }

                        // Bỏ check toàn quyền
                        FullControl.IsChecked = false;
                    }
                }
            }
        }

        private void FullControlCheckBox_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            if (checkBox != null)
            {
                string maChucNang = checkBox.Tag.ToString();
                if (checkBox.IsChecked.GetValueOrDefault())
                {
                    foreach (DataRow r in dt.Rows)
                    {
                        if (r["MA"].Equals(maChucNang))
                        {
                            for (int i = 3; i < dt.Columns.Count; i++)
                            {
                                string s = r[i].ToString();
                                if (!LString.IsNullOrEmptyOrSpace(s))
                                    r[i] = true;
                            }
                        }
                    }
                }
                else
                {
                    foreach (DataRow r in dt.Rows)
                    {
                        if (r["MA"].Equals(maChucNang))
                        {
                            for (int i = 3; i < dt.Columns.Count; i++)
                            {
                                string s = r[i].ToString();
                                if (!LString.IsNullOrEmptyOrSpace(s))
                                    r[i] = false;
                            }
                        }
                    }

                    // Đối với tính năng theo cột
                    string tenCheckBox = checkBox.Name.Substring(3);
                    //if (tenCheckBox.Equals(DatabaseConstant.Action.XEM.getValue()))
                    //    View.IsChecked = false;
                    //else if (tenCheckBox.Equals(DatabaseConstant.Action.THEM.getValue()))
                    //    Add.IsChecked = false;
                    //else if (tenCheckBox.Equals(DatabaseConstant.Action.SUA.getValue()))
                    //    Modify.IsChecked = false;
                    //else if (tenCheckBox.Equals(DatabaseConstant.Action.XOA.getValue()))
                    //    Delete.IsChecked = false;
                    //else if (tenCheckBox.Equals(DatabaseConstant.Action.DUYET.getValue()))
                    //    Approve.IsChecked = false;
                    //else if (tenCheckBox.Equals(DatabaseConstant.Action.THOAI_DUYET.getValue()))
                    //    Cancel.IsChecked = false;
                    //else if (tenCheckBox.Equals(DatabaseConstant.Action.TU_CHOI_DUYET.getValue()))
                    //    Refuse.IsChecked = false;
                    //else if (tenCheckBox.Equals(DatabaseConstant.Action.XU_LY.getValue()))
                    //    Process.IsChecked = false;
                    if (tenCheckBox.Equals(DatabaseConstant.Action.TOAN_QUYEN.getValue()))
                        FullControl.IsChecked = false;
                }
            }
        }

        private void CheckBox_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var checkBox = sender as CheckBox;
            if (checkBox != null)
            {
                string maChucNang = checkBox.Tag.ToString();

                if (checkBox.IsChecked.GetValueOrDefault())
                {
                    // Đối với chức năng theo hàng
                    foreach (DataRow r in dt.Rows)
                    {
                        if (r["MA"].Equals(maChucNang))
                        {
                            string s = r[3].ToString();
                            if (!LString.IsNullOrEmptyOrSpace(s))
                                r[3] = true;
                            bool all = true;
                            for (int i = 3; i < dt.Columns.Count - 1; i++)
                            {
                                string ss = r[i].ToString();
                                if (!LString.IsNullOrEmptyOrSpace(ss))
                                    if ((bool)r[i] == false)
                                    {
                                        all = false;
                                        break;
                                    }
                            }
                            string sFull = r["FullControl"].ToString();
                            if (!LString.IsNullOrEmptyOrSpace(sFull))
                                r["FullControl"] = all;
                        }
                    }

                    // Đối với tính năng theo cột
                }
                else
                {
                    // Đối với chức năng theo hàng
                    foreach (DataRow r in dt.Rows)
                    {
                        if (r["MA"].Equals(maChucNang))
                        {
                            string sFull = r["FullControl"].ToString();
                            if (!LString.IsNullOrEmptyOrSpace(sFull))
                                r["FullControl"] = false;
                        }
                    }

                    // Đối với tính năng theo cột
                    string tenCheckBox = checkBox.Name.Substring(3);
                    if (tenCheckBox.Equals(DatabaseConstant.Action.XEM.getValue()))
                        View.IsChecked = false;
                    else if (tenCheckBox.Equals(DatabaseConstant.Action.THEM.getValue()))
                        Add.IsChecked = false;
                    else if (tenCheckBox.Equals(DatabaseConstant.Action.SUA.getValue()))
                        Modify.IsChecked = false;
                    else if (tenCheckBox.Equals(DatabaseConstant.Action.XOA.getValue()))
                        Delete.IsChecked = false;
                    else if (tenCheckBox.Equals(DatabaseConstant.Action.DUYET.getValue()))
                        Approve.IsChecked = false;
                    else if (tenCheckBox.Equals(DatabaseConstant.Action.THOAI_DUYET.getValue()))
                        Cancel.IsChecked = false;
                    else if (tenCheckBox.Equals(DatabaseConstant.Action.TU_CHOI_DUYET.getValue()))
                        Refuse.IsChecked = false;
                    else if (tenCheckBox.Equals(DatabaseConstant.Action.XU_LY.getValue()))
                        Process.IsChecked = false;
                    //else if (tenCheckBox.Equals(DatabaseConstant.Action.TOAN_QUYEN.getValue()))
                    //    FullControl.IsChecked = false;

                    // Bỏ check toàn quyền
                    FullControl.IsChecked = false;
                }
            }
        }

        #endregion
    }
}
