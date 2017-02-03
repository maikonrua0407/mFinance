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
using Utilities.Common;
using System.Data;
using PresentationWPF.CustomControl;
using Microsoft.Windows.Controls.Ribbon;
using Presentation.Process;
using Presentation.Process.Common;
using Presentation.Process.KhachHangServiceRef;
using PresentationWPF.KhachHang.KhachHang;

namespace PresentationWPF.KhachHang.QuanLyHinhAnh
{
    /// <summary>
    /// Interaction logic for ucQuanLyHinhAnh.xaml
    /// </summary>
    public partial class ucQuanLyHinhAnh : UserControl
    {
        #region Khai bao
        private DatabaseConstant.Action action = DatabaseConstant.Action.THEM;
        public DatabaseConstant.Action Action
        {
            get { return action; }
            set { action = value; }
        }

        private DatabaseConstant.Function function = DatabaseConstant.Function.KH_QL_HINH_HANH;

        public event EventHandler OnSavingCompleted;

        //private int idKhachHang;

        private string sTrangThaiNVu = "";

        List<AutoCompleteEntry> lstSourceLoaiHinhAnh = new List<AutoCompleteEntry>();
        List<LoaiHinhAnh> lstLoaiHinhAnhDef = new List<LoaiHinhAnh>()
        {
            new LoaiHinhAnh(){ MaLoai = "HSPL", TenLoai = "Personal Profile"},
	        //new LoaiHinhAnh(){ MaLoai = "HSKT", TenLoai = "Economic Profile"},
            //new LoaiHinhAnh(){ MaLoai = "TCVM", TenLoai = "Quan hệ với tổ chức TCVM"}
        };
        List<LoaiHinhAnh> lstLoaiHinhAnhSel = new List<LoaiHinhAnh>();
        List<DoiTuongHinhAnh> lstDoiTuongHinhAnhDef = new List<DoiTuongHinhAnh>()
        {
            new DoiTuongHinhAnh(){ MaLoai = "HSPL", MaDoiTuong="CK", TenDoiTuong = "Signature"},
            new DoiTuongHinhAnh(){ MaLoai = "HSPL", MaDoiTuong="HA", TenDoiTuong = "Portrait"},
	        new DoiTuongHinhAnh(){ MaLoai = "HSPL", MaDoiTuong="CMTND", TenDoiTuong = "Identity card"},
            new DoiTuongHinhAnh(){ MaLoai = "HSPL", MaDoiTuong="KHAC", TenDoiTuong = "Others"},

            //new DoiTuongHinhAnh(){ MaLoai = "HSKT", MaDoiTuong="SODO", TenDoiTuong = "Property confirmation"},
            //new DoiTuongHinhAnh(){ MaLoai = "HSKT", MaDoiTuong="HDLD", TenDoiTuong = "Labor contract"},
            //new DoiTuongHinhAnh(){ MaLoai = "HSKT", MaDoiTuong="SAOKE", TenDoiTuong = "Account statement"},
            //new DoiTuongHinhAnh(){ MaLoai = "HSKT", MaDoiTuong="KHAC", TenDoiTuong = "Others"},

            //new DoiTuongHinhAnh(){ MaLoai = "TCVM", MaDoiTuong="SOTK", TenDoiTuong = "Sổ tiết kiệm"},
            //new DoiTuongHinhAnh(){ MaLoai = "TCVM", MaDoiTuong="HDTD", TenDoiTuong = "Hợp đồng tín dụng"},
            //new DoiTuongHinhAnh(){ MaLoai = "TCVM", MaDoiTuong="KHTV", TenDoiTuong = "Xác nhận thành viên"},
            //new DoiTuongHinhAnh(){ MaLoai = "TCVM", MaDoiTuong="KHAC", TenDoiTuong = "Others"}
        };
        List<DoiTuongHinhAnh> lstDoiTuongHinhAnhSel = new List<DoiTuongHinhAnh>();

        private KH_KHANG_HSO _objKH = new KH_KHANG_HSO();
        public KH_KHANG_HSO objKH
        {
            get { return _objKH = new KH_KHANG_HSO(); }
            set { _objKH = value; }
        }

        private List<DuLieuHinhAnh> lstDuLieuHinhAnh = new List<DuLieuHinhAnh>();
        private List<VKH_CKY_HANH> lstCkyHanh = new List<VKH_CKY_HANH>();

        private byte[] imageData = null;
        private string imageName = "";
        private string imageFormat = "";
        private List<BS_FileObject> lstChuKyHinhAnh = new List<BS_FileObject>();

        List<DataRow> lstPopup = new List<DataRow>();
        public void LayDuLieuTuPopup(List<DataRow> lst)
        {
            lstPopup = lst;
        }

        public static RoutedCommand SaveCommand = new RoutedCommand();
        public static RoutedCommand AddCommand = new RoutedCommand();
        public static RoutedCommand ModifyCommand = new RoutedCommand();
        public static RoutedCommand DeleteCommand = new RoutedCommand();
        public static RoutedCommand ApproveCommand = new RoutedCommand();
        public static RoutedCommand RefuseCommand = new RoutedCommand();
        public static RoutedCommand CancelCommand = new RoutedCommand();
        public static RoutedCommand HelpCommand = new RoutedCommand();
        public static RoutedCommand CloseCommand = new RoutedCommand();
        #endregion

        #region Khoi tao
        public ucQuanLyHinhAnh()
        {
            InitializeComponent();

            DuyetQuyenTinhNang();

            BindShortkey();            

            LoadCombobox();

            ResetForm();

            InitEventHandler();            
        }

        private void DuyetQuyenTinhNang()
        {
            HeThong hethong = new HeThong();
            hethong.DuyetQuyenTinhNangToolbar("/PresentationWPF.KhachHang;component/QuanLyHinhAnh/ucQuanLyHinhAnh.xaml", ref Toolbar, ref mnuMain);
            foreach (var item in mnuMain.Items)
            {
                if (item is MenuItem)
                    ((MenuItem)item).Click += btnShortcutKey_Click;
            }
        }

        private void InitEventHandler()
        {
            
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

                    if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRINH_DUYET)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.S, ModifierKeys.Control);
                        key = new KeyBinding(SaveCommand, keyg);
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
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DUYET)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.A, ModifierKeys.Control | ModifierKeys.Shift);
                        key = new KeyBinding(ApproveCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.R, ModifierKeys.Control | ModifierKeys.Shift);
                        key = new KeyBinding(RefuseCommand, keyg);
                    }
                    else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
                    {
                        KeyGesture keyg = new KeyGesture(Key.C, ModifierKeys.Control | ModifierKeys.Shift);
                        key = new KeyBinding(CancelCommand, keyg);
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
            OnSave();
        }

        private void ModifyCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void ModifyCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            BeforeModifyFromDetail();
        }

        private void DeleteCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void DeleteCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            BeforeDelete();
        }

        private void ApproveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void ApproveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            BeforeApprove();
        }

        private void RefuseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void RefuseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            BeforeRefuse();
        }

        private void CancelCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void CancelCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            BeforeCancel();
        }

        private void HelpCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void HelpCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OnHelp();
        }

        private void CloseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        private void CloseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            OnClose();
        }

        private void tlbHotKey_Click(object sender, RoutedEventArgs e)
        {
            RibbonButton tlb = (RibbonButton)sender;
            string strTinhNang = tlb.Name.Substring(3, tlb.Name.Length - 3);
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU_TAM)))
            {
                //OnHold();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRINH_DUYET)))
            {
                OnSave();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
                BeforeModifyFromDetail();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
            {
                BeforeDelete();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DUYET)))
            {
                BeforeApprove();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET)))
            {
                BeforeRefuse();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
            {
                BeforeCancel();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRO_GIUP)))
            {
                OnHelp();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DONG)))
            {
                OnClose();
            }
        }

        private void btnShortcutKey_Click(object sender, RoutedEventArgs e)
        {
            string strTinhNang = ((MenuItem)sender).Name.Substring(3, ((MenuItem)sender).Name.Length - 3);
            if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.LUU_TAM)))
            {
                //OnHold();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRINH_DUYET)))
            {
                OnSave();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.SUA)))
            {
                BeforeModifyFromDetail();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XOA)))
            {
                BeforeDelete();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DUYET)))
            {
                BeforeApprove();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TU_CHOI_DUYET)))
            {
                BeforeRefuse();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.THOAI_DUYET)))
            {
                BeforeCancel();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TINH_TOAN)))
            {
                //TinhDuChi();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.XEM_TRUOC)))
            {
                //OnPreview();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.TRO_GIUP)))
            {
                OnHelp();
            }
            else if (strTinhNang.Equals(DatabaseConstant.getValue(DatabaseConstant.Action.DONG)))
            {
                OnClose();
            }
        }

        #endregion

        #region Xu ly Giao dien
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //Hiển thị Form khi thêm mới dữ liệu
            if (action == DatabaseConstant.Action.THEM)
            {
                BeforeAddNew();
            }

            //Hiển thị Form khi sửa dữ liệu
            else if (action == DatabaseConstant.Action.SUA)
            {
                BeforeModifyFromList();
            }

            //Hiển thị Form khi xem dữ liệu
            else if (action == DatabaseConstant.Action.XEM)
            {
                BeforeViewFromList();
            }            
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            Release();
            if (OnSavingCompleted != null)
            {
                OnSavingCompleted(this, EventArgs.Empty);
            }
            CustomControl.CommonFunction.CloseUserControl(this);
        }

        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            PresentationWPF.CustomControl.CommonFunction.CloseUserControl(e, this);
            PresentationWPF.CustomControl.CommonFunction.SelectNextControl(e);
        }

        private void tlbAddHinhAnh_Click(object sender, RoutedEventArgs e)
        {
            if (ValidationHSHA("THEM"))
            {
                string MaLoai = ((LoaiHinhAnh)cmbLoaiHSHA.SelectedItem).MaLoai;
                string TenLoai = ((LoaiHinhAnh)cmbLoaiHSHA.SelectedItem).TenLoai;
                string MaDoiTuong = ((DoiTuongHinhAnh)cmbDoiTuongHSHA.SelectedItem).MaDoiTuong;
                string TenDoiTuong = ((DoiTuongHinhAnh)cmbDoiTuongHSHA.SelectedItem).TenDoiTuong;
                string TenHinhAnh = txtMaHSHA.Text;

                int count = grDLieuHAnh.Items != null ? grDLieuHAnh.Items.Count + 1 : 1;

                DuLieuHinhAnh duLieuHinhAnh = new DuLieuHinhAnh();
                duLieuHinhAnh.ID = -1;
                duLieuHinhAnh.STT = count;
                duLieuHinhAnh.MaLoai = MaLoai;
                duLieuHinhAnh.TenLoai = TenLoai;
                duLieuHinhAnh.MaDoiTuong = MaDoiTuong;
                duLieuHinhAnh.TenDoiTuong = TenDoiTuong;
                duLieuHinhAnh.MaHinhAnh = count.ToString();
                duLieuHinhAnh.TenHinhAnh = TenHinhAnh;
                duLieuHinhAnh.HieuLuc = false;
                duLieuHinhAnh.HienThiHS = false;
                duLieuHinhAnh.CHON = false;
                duLieuHinhAnh.Data = imageData;
                duLieuHinhAnh.ImageName = count.ToString();
                duLieuHinhAnh.ImageFormat = imageFormat;
                duLieuHinhAnh.TrangThai = BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri();
                duLieuHinhAnh.TenTrangThai = BusinessConstant.layNgonNguNghiepVu(BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri());
                duLieuHinhAnh.NgayDuLieu = ClientInformation.NgayLamViecHienTai;
                duLieuHinhAnh.NgayHieuLuc = ClientInformation.NgayLamViecHienTai;

                lstDuLieuHinhAnh.Add(duLieuHinhAnh);

                grDLieuHAnh.ItemsSource = null;
                grDLieuHAnh.ItemsSource = lstDuLieuHinhAnh;
            }
        }

        private void tlbModifyHinhAnh_Click(object sender, RoutedEventArgs e)
        {            
            if (ValidationHSHA("SUA") && grDLieuHAnh.Items.Count > 0)
            {
                DuLieuHinhAnh item = (DuLieuHinhAnh)grDLieuHAnh.SelectedItems.FirstOrDefault();
                if (item.TrangThai.Equals(BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri()))
                {
                    LMessage.ShowMessage("M.DungChung.DaDuyetKhongDuocSua", LMessage.MessageBoxType.Warning);
                    return;
                }

                string MaLoai = ((LoaiHinhAnh)cmbLoaiHSHA.SelectedItem).MaLoai;
                string TenLoai = ((LoaiHinhAnh)cmbLoaiHSHA.SelectedItem).TenLoai;
                string MaDoiTuong = ((DoiTuongHinhAnh)cmbDoiTuongHSHA.SelectedItem).MaDoiTuong;
                string TenDoiTuong = ((DoiTuongHinhAnh)cmbDoiTuongHSHA.SelectedItem).TenDoiTuong;
                string TenHinhAnh = txtMaHSHA.Text;

                //int count = grDLieuHAnh.Items != null ? grDLieuHAnh.Items.Count + 1 : 1;
                //dsSource.Tables["VKH_CKY_HANH"].Rows.Add(count, -1, -1, MaLoai, MaDoiTuong, TenHinhAnh, false, count.ToString(), false);
                lstDuLieuHinhAnh = grDLieuHAnh.ItemsSource as List<DuLieuHinhAnh>;

                DuLieuHinhAnh duLieuHinhAnh = grDLieuHAnh.SelectedItem as DuLieuHinhAnh;
                int index = lstDuLieuHinhAnh.IndexOf(duLieuHinhAnh);

                duLieuHinhAnh.ID = -1;
                //duLieuHinhAnh.STT = count;
                duLieuHinhAnh.MaLoai = MaLoai;
                duLieuHinhAnh.TenLoai = TenLoai;
                duLieuHinhAnh.MaDoiTuong = MaDoiTuong;
                duLieuHinhAnh.TenDoiTuong = TenDoiTuong;
                //duLieuHinhAnh.MaHinhAnh = count.ToString();
                duLieuHinhAnh.TenHinhAnh = TenHinhAnh;
                duLieuHinhAnh.HieuLuc = false;
                duLieuHinhAnh.HienThiHS = false;
                duLieuHinhAnh.CHON = false;
                duLieuHinhAnh.Data = imageData;
                //duLieuHinhAnh.ImageName = count.ToString();
                duLieuHinhAnh.ImageFormat = imageFormat;
                duLieuHinhAnh.TrangThai = BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri();
                duLieuHinhAnh.TenTrangThai = BusinessConstant.layNgonNguNghiepVu(BusinessConstant.TrangThaiNghiepVu.CHO_DUYET.layGiaTri());
                duLieuHinhAnh.NgayDuLieu = ClientInformation.NgayLamViecHienTai;
                duLieuHinhAnh.NgayHieuLuc = ClientInformation.NgayLamViecHienTai;

                lstDuLieuHinhAnh[index] = duLieuHinhAnh;

                grDLieuHAnh.ItemsSource = null;
                grDLieuHAnh.ItemsSource = lstDuLieuHinhAnh;
            }            
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void chkAll_Click(object sender, RoutedEventArgs e)
        {

        }

        private void grDLieuHAnh_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            if (grDLieuHAnh.SelectedItems.Count > 0)
            {
                List<string> key = new List<string>();
                foreach (DuLieuHinhAnh dlha in grDLieuHAnh.SelectedItems)
                {
                    key.Add(dlha.TenHinhAnh);
                }
                string keySelected = key.FirstOrDefault();
                DuLieuHinhAnh duLieuHinhAnhSelected = lstDuLieuHinhAnh.Where(item => item.TenHinhAnh.Equals(keySelected)).FirstOrDefault();

                lstLoaiHinhAnhSel = lstLoaiHinhAnhDef;
                cmbLoaiHSHA.ItemsSource = lstLoaiHinhAnhSel;
                LoaiHinhAnh loaiHA = lstLoaiHinhAnhSel.Where(item => item.MaLoai == duLieuHinhAnhSelected.MaLoai).FirstOrDefault();
                cmbLoaiHSHA.DisplayMemberPath = "TenLoai";
                cmbLoaiHSHA.SelectedItem = loaiHA;

                lstDoiTuongHinhAnhSel = new List<DoiTuongHinhAnh>();
                string MaLoai = duLieuHinhAnhSelected.MaLoai;
                lstDoiTuongHinhAnhSel = lstDoiTuongHinhAnhDef.Where(item => item.MaLoai == MaLoai).ToList();
                cmbDoiTuongHSHA.ItemsSource = lstDoiTuongHinhAnhSel;
                DoiTuongHinhAnh doiTuongHA = lstDoiTuongHinhAnhSel.Where(item => item.MaDoiTuong == duLieuHinhAnhSelected.MaDoiTuong).FirstOrDefault();
                cmbDoiTuongHSHA.DisplayMemberPath = "TenDoiTuong";
                cmbDoiTuongHSHA.SelectedItem = doiTuongHA;

                txtMaHSHA.Text = duLieuHinhAnhSelected.TenHinhAnh;

                byte[] source = duLieuHinhAnhSelected.Data;
                BitmapImage myBitmapImage = new BitmapImage();
                myBitmapImage = LImage.LoadImageFromByteArray(source);
                if (myBitmapImage != null)
                {
                    imgAvatar.Source = myBitmapImage;
                    imgAvatar.Tag = duLieuHinhAnhSelected.TenHinhAnh;

                    imageData = source;
                    imageName = duLieuHinhAnhSelected.ImageName;
                    imageFormat = duLieuHinhAnhSelected.ImageFormat;
                }
            }
        }

        private void imgAvatar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && e.ClickCount == 2)
            {
                Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
                try
                {
                    dlg.FileName = "Document";
                    dlg.DefaultExt = ".jpg";
                    dlg.Filter = "Image (.jpg)|*.jpg";

                    // Show open file dialog box
                    Nullable<bool> result = dlg.ShowDialog();

                    if (result == true)
                    {
                        imgAvatar.Tag = dlg.FileName;
                        LoadImageInClient(dlg.FileName, imgAvatar);

                        imageData = LImage.GetByteArrayFromImage(dlg.FileName);
                        string[] str = @dlg.FileName.Split('\\');
                        string imageFullName = str[str.Length - 1];
                        string[] strFormat = imageFullName.Split('.');
                        imageFormat = strFormat[strFormat.Length - 1];
                        imageName = "";
                    }
                }
                catch (Exception ex)
                {
                    LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                }
            }
        }

        private void mniXoaAnh_Click(object sender, RoutedEventArgs e)
        {
            ResetImage();
        }

        private void ResetImage()
        {
            BitmapImage logo = new BitmapImage();
            logo.BeginInit();
            logo.UriSource = new Uri("pack://application:,,,/Utilities.Common;component/Images/Other/picture.png");
            logo.EndInit();
            imgAvatar.Source = logo;
            imgAvatar.Tag = "ResetImage";
        }

        private void ResetImage(Image img)
        {
            BitmapImage logo = new BitmapImage();
            logo.BeginInit();
            logo.UriSource = new Uri("pack://application:,,,/Utilities.Common;component/Images/Other/picture.png");
            logo.EndInit();
            img.Source = logo;
            img.Tag = "";
        }

        private void LoadImageInClient(string path, Image img)
        {
            // Tạo image source
            BitmapImage myBitmapImage = new BitmapImage();

            // Set image vào image box
            myBitmapImage.BeginInit();
            myBitmapImage.UriSource = new Uri(path);
            myBitmapImage.DecodePixelWidth = (int)brdAvatar.ActualWidth;
            myBitmapImage.DecodePixelHeight = (int)brdAvatar.ActualHeight;
            myBitmapImage.EndInit();
            img.Source = myBitmapImage;
        }

        private void LoadImageInServer(string imageName, Image img)
        {
            Presentation.Process.DanhMucProcess process = new Presentation.Process.DanhMucProcess();
            byte[] source = process.LayAnhTuSever(DatabaseConstant.Table.KH_KHANG_HSO.getValue() + "\\" + imageName);
            BitmapImage myBitmapImage = new BitmapImage();
            myBitmapImage = LImage.LoadImageFromByteArray(source);
            if (myBitmapImage != null)
            {
                img.Source = myBitmapImage;
            }
        }

        private void ResetHSHA()
        {
            ResetImage(imgAvatar);
            txtMaHSHA.Text = "";
        }

        private void mniXemAnh_Click(object sender, RoutedEventArgs e)
        {

        }

        private void tlbDeleteHinhAnh_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (grDLieuHAnh.SelectedItems.Count > 0)
                {
                    //List<DuLieuHinhAnh> lstRowDel = new List<DuLieuHinhAnh>();
                    //foreach (var item in grDLieuHAnh.SelectedItems)
                    //{
                    //    string tenHinhAnh = ((DataRow)item).Field<string>("TenHinhAnh");
                    //    DuLieuHinhAnh r = lstDuLieuHinhAnh.FirstOrDefault(d => d.TenHinhAnh.Equals(tenHinhAnh));
                    //    lstRowDel.Add(r);
                    //}

                    //foreach (DuLieuHinhAnh item in lstRowDel)
                    //{
                    //    for (int i = lstDuLieuHinhAnh.Count - 1; i >= 0; i--)
                    //    {
                    //        if ((lstDuLieuHinhAnh.ElementAt(i).TenHinhAnh).Equals(item.TenHinhAnh))
                    //        {
                    //            lstDuLieuHinhAnh.RemoveAt(i);
                    //            break;
                    //        }
                    //    }
                    //}
                    //for (int i = lstDuLieuHinhAnh.Count; i > 0; i--)
                    //{
                    //    lstDuLieuHinhAnh.ElementAt(i).STT = i;
                    //}

                    DuLieuHinhAnh item = (DuLieuHinhAnh)grDLieuHAnh.SelectedItems.FirstOrDefault();

                    if (!item.TrangThai.Equals(BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri()))
                    {
                        foreach (DuLieuHinhAnh dlha in grDLieuHAnh.SelectedItems)
                        {
                            lstDuLieuHinhAnh.Remove(dlha);
                        }
                    }
                    else
                    {
                        LMessage.ShowMessage("M.DungChung.DaDuyetKhongDuocSua", LMessage.MessageBoxType.Warning);
                    }

                    grDLieuHAnh.ItemsSource = null;
                    grDLieuHAnh.ItemsSource = lstDuLieuHinhAnh;
                }
            }
            catch (System.Exception ex)
            {

            }
        }

        private bool ValidationHSHA(string action)
        {
            if (cmbLoaiHSHA.SelectedIndex < 0)
            {
                LMessage.ShowMessage("M_ResponseMessage_QuanLyHinhAnh_ChuaLuaChonLoaiHinhAnh", LMessage.MessageBoxType.Warning);
                cmbLoaiHSHA.Focus();
                return false;
            }

            if (cmbDoiTuongHSHA.SelectedIndex < 0)
            {
                LMessage.ShowMessage("M_ResponseMessage_QuanLyHinhAnh_ChuaLuaChonDoiTuongHinhAnh", LMessage.MessageBoxType.Warning);
                cmbDoiTuongHSHA.Focus();
                return false;
            }

            if (txtMaHSHA.Text.IsNullOrEmptyOrSpace())
            {
                LMessage.ShowMessage("M_ResponseMessage_QuanLyHinhAnh_ChuaLuaChonTenHinhAnh", LMessage.MessageBoxType.Warning);
                txtMaHSHA.Focus();
                return false;
            }

            BitmapImage logo = new BitmapImage();
            logo.BeginInit();
            logo.UriSource = new Uri("pack://application:,,,/Utilities.Common;component/Images/Other/picture.png");
            logo.EndInit();

            if (imgAvatar.Source == null || imgAvatar.Tag == null || LString.IsNullOrEmptyOrSpace(imgAvatar.Tag.ToString()))
            {                
                LMessage.ShowMessage("M_ResponseMessage_QuanLyHinhAnh_ChuaLuaChonHinhAnh", LMessage.MessageBoxType.Warning);
                return false;
            }

            if (imgAvatar.Source != null && imgAvatar.Tag != null && !LString.IsNullOrEmptyOrSpace(imgAvatar.Tag.ToString()) && imgAvatar.Tag.ToString().Equals("ResetImage"))
            {
                LMessage.ShowMessage("M_ResponseMessage_QuanLyHinhAnh_ChuaLuaChonHinhAnh", LMessage.MessageBoxType.Warning);
                return false;
            }

            if (action.Equals("THEM"))
            {
                if (lstDuLieuHinhAnh.Count > 0)
                {
                    string tenHinhAnh = txtMaHSHA.Text;
                    if (lstDuLieuHinhAnh.Where(item => item.TenHinhAnh.Equals(tenHinhAnh)).ToList().Count() > 0)
                    {
                        string[] s = new string[1] { tenHinhAnh };
                        LMessage.ShowMessage("M_ResponseMessage_QuanLyHinhAnh_DaTonTaiTenHinhAnh", s, LMessage.MessageBoxType.Warning);
                        txtMaHSHA.Focus();
                        return false;
                    }
                }
            }
            else if (action.Equals("SUA"))
            {
                if (lstDuLieuHinhAnh.Count > 0)
                {
                    string tenHinhAnh = txtMaHSHA.Text;
                    List<string> tenHinhAnhDangChon = new List<string>();
                    foreach (DuLieuHinhAnh dlha in grDLieuHAnh.SelectedItems)
                    {
                        tenHinhAnhDangChon.Add(dlha.TenHinhAnh);
                    }
                    if (lstDuLieuHinhAnh.Where(item => item.TenHinhAnh.Equals(tenHinhAnh) && !tenHinhAnhDangChon.Contains(item.TenHinhAnh)).ToList().Count() > 0)
                    {
                        string[] s = new string[1] { tenHinhAnh };
                        LMessage.ShowMessage("M_ResponseMessage_QuanLyHinhAnh_DaTonTaiTenHinhAnh", s, LMessage.MessageBoxType.Warning);
                        txtMaHSHA.Focus();
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Hàm xử lý giải phóng lock dữ liệu cho form
        /// </summary>
        private void Release()
        {
            // Yêu cầu unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            //listLockId.Add(_objKH.ID);

            //bool ret = process.UnlockData(DatabaseConstant.Module.TDTT,
            //    DatabaseConstant.Function.TD_SAN_PHAMTT,
            //    DatabaseConstant.Table.TD_SAN_PHAMTT,
            //    DatabaseConstant.Action.SUA,
            //    listLockId);
        }

        /// <summary>
        /// Trợ giúp
        /// </summary>
        private void OnHelp()
        {
            CustomControl.CommonFunction.ShowHelp(this);
        }

        /// <summary>
        /// Đóng form
        /// </summary>
        private void OnClose()
        {
            Release();
            CustomControl.CommonFunction.CloseUserControl(this);
        }

        #region Load Combobox
        private void LoadCombobox()
        {
            try
            {
                lstLoaiHinhAnhSel = lstLoaiHinhAnhDef;
                cmbLoaiHSHA.ItemsSource = lstLoaiHinhAnhSel;
                cmbLoaiHSHA.DisplayMemberPath = "TenLoai";
                cmbLoaiHSHA.SelectedItem = lstLoaiHinhAnhSel.ElementAt(0);
                cmbLoaiHSHA.SelectionChanged += new SelectionChangedEventHandler(cmbLoaiHSHA_SelectionChanged);

                lstDoiTuongHinhAnhSel = new List<DoiTuongHinhAnh>();
                string MaLoai = ((LoaiHinhAnh)cmbLoaiHSHA.SelectedItem).MaLoai;
                lstDoiTuongHinhAnhSel = lstDoiTuongHinhAnhDef.Where(item => item.MaLoai == MaLoai).ToList();
                cmbDoiTuongHSHA.ItemsSource = lstDoiTuongHinhAnhSel;
                cmbDoiTuongHSHA.DisplayMemberPath = "TenDoiTuong";
                cmbDoiTuongHSHA.SelectedItem = lstDoiTuongHinhAnhSel.ElementAt(0);

            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        private void cmbLoaiHSHA_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbLoaiHSHA.SelectedIndex >= 0)
            {
                lstLoaiHinhAnhSel = new List<LoaiHinhAnh>();
                string MaLoai = ((LoaiHinhAnh)cmbLoaiHSHA.SelectedItem).MaLoai;
                lstDoiTuongHinhAnhSel = lstDoiTuongHinhAnhDef.Where(item => item.MaLoai == MaLoai).ToList();
            }
            else
            {
                lstLoaiHinhAnhSel = new List<LoaiHinhAnh>();
            }

            cmbDoiTuongHSHA.ItemsSource = lstDoiTuongHinhAnhSel;
            cmbDoiTuongHSHA.DisplayMemberPath = "TenDoiTuong";
            cmbDoiTuongHSHA.SelectedItem = lstDoiTuongHinhAnhSel.ElementAt(0);
        }
        #endregion

        #region Xử lý Popup    

        private void txtMaKhachHang_LostFocus(object sender, RoutedEventArgs e)
        {
            return;
            try
            {
                if (!txtMaKhachHang.Text.IsNullOrEmptyOrSpace())
                {
                    KhachHangProcess processKH = new KhachHangProcess();
                    //DataSet ds = processKH.getThongTinCoBanKHTheoMa(0, txtMaKhachHang.Text, Convert.ToInt32(ClientInformation.IdDonVi));
                    DataSet ds = processKH.getThongTinCoBanKHTheoMa(0, txtMaKhachHang.Text, 0);
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        if (_objKH == null)
                        {
                            _objKH = new KH_KHANG_HSO();
                        }
                        DataRow dr = ds.Tables[0].Rows[0];
                        _objKH.ID = Convert.ToInt32(dr["ID_KHANG"]);
                        txtMaKhachHang.Text = dr["MA_KHANG"].ToString();
                        lblTenKH.Content = dr["TEN_KHANG"].ToString();

                        _objKH.MA_KHANG = txtMaKhachHang.Text;
                        _objKH.TEN_KHANG = lblTenKH.Content.ToString();
                        SetFormData();
                    }
                    else
                    {
                        txtMaKhachHang.Text = "";
                        lblTenKH.Content = LLanguage.SearchResourceByKey("U.HuyDongVon.MoSo.ucTienGuiCoKyHanCT.TenKhachHang");
                        
                    }
                }
                else
                {
                    LMessage.ShowMessage("M_ResponseMessage_KhachHang_KhongTonTai", LMessage.MessageBoxType.Warning);
                    txtMaKhachHang.Text = "";
                    lblTenKH.Content = LLanguage.SearchResourceByKey("U.HuyDongVon.MoSo.ucTienGuiCoKyHanCT.TenKhachHang");
                    _objKH = null;
                    lstChuKyHinhAnh.Clear();
                    lstCkyHanh.Clear();
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);

            }
        }

        private void txtMaKhachHang_KeyDown(object sender, KeyEventArgs e)
        {
            return;
            if (e.Key == Key.F3)
            {
                btnKhachHang_Click(null, null);
            }
        }

        private void btnKhachHang_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                Window window = new Window();
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                ucPopupKhachHang uc = new ucPopupKhachHang();
                window.Title = LLanguage.SearchResourceByKey("U.HuyDongVon.MoSo.ucTienGuiCoKyHanCT.DanhSachKhachHang");
                window.Content = uc;
                Mouse.OverrideCursor = Cursors.Arrow;
                window.ShowDialog();
                if (uc.lstData != null && uc.lstData.Count > 0)
                {
                    if(_objKH == null)
                    {
                        _objKH = new KH_KHANG_HSO();
                    }

                    DataRowView drKhachHang = uc.lstData[0];

                    if (((ClientInformation.Company.Equals(ApplicationConstant.DonViSuDung.BIDV.layGiaTri())) 
                        || (ClientInformation.Company.Equals(ApplicationConstant.DonViSuDung.BIDV_BLF.layGiaTri()))) &&
                        drKhachHang["TTHAI_NVU"].Equals(BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri())
                        )
                    {
                        KhachHangProcess processKH = new KhachHangProcess();
                        _objKH.ID = Convert.ToInt32(drKhachHang["ID"]);
                        Mouse.OverrideCursor = Cursors.Wait;

                        txtMaKhachHang.Text = drKhachHang["MA_KHANG"].ToString();
                        lblTenKH.Content = drKhachHang["TEN_KHANG"].ToString();
                        _objKH.MA_KHANG = drKhachHang["MA_KHANG"].ToString();
                        _objKH.TEN_KHANG = drKhachHang["TEN_KHANG"].ToString();

                        cmbLoaiHSHA.SelectedIndex = 1;
                        cmbDoiTuongHSHA.SelectedIndex = 1;
                        cmbPhamVi.SelectedIndex = 1;
                        txtMaHSHA.Text = "";
                        ResetImage();

                        SetFormData();
                        Mouse.OverrideCursor = Cursors.Arrow;
                    }
                    else
                    {
                        Mouse.OverrideCursor = Cursors.Wait;
                        LMessage.ShowMessage("M.ResponseMessage.Common.ChuaDuyet", LMessage.MessageBoxType.Warning);
                        Mouse.OverrideCursor = Cursors.Arrow;
                    }
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        #endregion

        #endregion

        #region Xử lý nghiệp vụ
        private void GetFormData(string sTrangThaiNVu)
        {
            try
            {
                //obj = new DuLieuHinhAnh();
                if (lstDuLieuHinhAnh.Count > 0)
                {
                    KH_KHANG_HSO objKH = new KH_KHANG_HSO();
                    if (_objKH == null)
                    {
                        _objKH = new KH_KHANG_HSO();
                    }
                    //_objKH.ID = idKhachHang;
                    _objKH.MA_KHANG = txtMaKhachHang.Text.Trim();

                    lstChuKyHinhAnh = new List<BS_FileObject>();
                    lstCkyHanh.Clear();
                    int ii = 0;
                    foreach (DuLieuHinhAnh item in lstDuLieuHinhAnh)
                    {
                        ii++;
                        int STT = item.STT;
                        int ID = -1;
                        //int ID_KHANG = _objKH.ID;
                        string CKHA_LOAI = item.MaLoai;
                        string CKHA_DTUONG = item.MaDoiTuong;
                        string CKHA_MA = item.TenHinhAnh;
                        bool CKHA_HIEU_LUC = false;
                        //string CKHA_DUONG_DAN = item.STT.ToString() + "." + item.ImageFormat;
                        string CKHA_DUONG_DAN = ii.ToString() + "." + item.ImageFormat;
                        bool CKHA_HIEN_THI_HS = false;

                        VKH_CKY_HANH objCky = new VKH_CKY_HANH();
                        objCky.ID = ID;
                        objCky.ID_KHANG = _objKH.ID;
                        objCky.TEN_BANG = "KH_KHANG_HSO";
                        objCky.MA_DOI_TUONG = "VKH_CKY_HANH";
                        objCky.NGAY_DL = item.NgayDuLieu; // ClientInformation.NgayLamViecHienTai;
                        objCky.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();
                        //objCky.TTHAI_NVU = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();
                        objCky.TTHAI_NVU = item.TrangThai;
                        objCky.MA_DVI_TAO = ClientInformation.MaDonViGiaoDich;
                        objCky.NGAY_NHAP = ClientInformation.NgayLamViecHienTai;
                        objCky.NGUOI_NHAP = ClientInformation.TenDangNhap;
                        objCky.NGAY_CNHAT = ClientInformation.NgayLamViecHienTai;
                        objCky.NGUOI_CNHAT = ClientInformation.TenDangNhap;
                        objCky.CKHA_LOAI = CKHA_LOAI;
                        objCky.CKHA_MA = CKHA_MA;
                        objCky.CKHA_DTUONG = CKHA_DTUONG;
                        objCky.CKHA_HIEU_LUC = CKHA_HIEU_LUC.ToString();
                        objCky.CKHA_DUONG_DAN = CKHA_DUONG_DAN;
                        objCky.CKHA_HIEN_THI_HS = CKHA_HIEN_THI_HS.ToString();
                        objCky.CKHA_NGAY_HIEU_LUC = item.NgayHieuLuc;
                        lstCkyHanh.Add(objCky);

                        BS_FileObject img = new BS_FileObject();
                        img.FileName = item.ImageName;
                        img.FileFormat = item.ImageFormat;
                        img.FileData = item.Data;

                        lstChuKyHinhAnh.Add(img);
                    }
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        private void SetFormData()
        {
            KhachHangProcess processKhachHang = new KhachHangProcess();
            List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                bool ret = false;
                ret = processKhachHang.QuanLyHinhAnh(DatabaseConstant.Action.LOAD, ref _objKH, ref lstCkyHanh, ref lstChuKyHinhAnh, ref listClientResponseDetail);
                lstDuLieuHinhAnh = new List<DuLieuHinhAnh>();
                int ii = 1;

                foreach (VKH_CKY_HANH item in lstCkyHanh)
                {
                    DuLieuHinhAnh duLieuHinhAnh = new DuLieuHinhAnh();
                    duLieuHinhAnh.ID = -1;
                    duLieuHinhAnh.STT = ii;
                    duLieuHinhAnh.MaLoai = item.CKHA_LOAI;
                    duLieuHinhAnh.TenLoai = lstLoaiHinhAnhDef.Where(e => e.MaLoai == duLieuHinhAnh.MaLoai).Select(e => e.TenLoai).FirstOrDefault();
                    duLieuHinhAnh.MaDoiTuong = item.CKHA_DTUONG;
                    duLieuHinhAnh.TenDoiTuong = lstDoiTuongHinhAnhDef.Where(e => e.MaLoai == duLieuHinhAnh.MaLoai && e.MaDoiTuong == duLieuHinhAnh.MaDoiTuong).Select(e => e.TenDoiTuong).FirstOrDefault();
                    duLieuHinhAnh.MaHinhAnh = item.CKHA_MA;
                    duLieuHinhAnh.TenHinhAnh = item.CKHA_MA;
                    duLieuHinhAnh.HieuLuc = false;
                    duLieuHinhAnh.HienThiHS = false;
                    duLieuHinhAnh.CHON = false;
                    imageData = lstChuKyHinhAnh.Where(f => f.FileName + ".jpg" == item.CKHA_DUONG_DAN).Select(f => f.FileData).FirstOrDefault();
                    duLieuHinhAnh.Data = imageData;
                    //duLieuHinhAnh.ImageName = item["CKHA_DUONG_DAN"].ToString();
                    duLieuHinhAnh.ImageName = ii.ToString();
                    duLieuHinhAnh.ImageFormat = "jpg";
                    duLieuHinhAnh.TrangThai = item.TTHAI_NVU;
                    duLieuHinhAnh.TenTrangThai = BusinessConstant.layNgonNguNghiepVu(item.TTHAI_NVU);
                    duLieuHinhAnh.NgayDuLieu = item.NGAY_DL;
                    duLieuHinhAnh.NgayHieuLuc = item.CKHA_NGAY_HIEU_LUC;

                    lstDuLieuHinhAnh.Add(duLieuHinhAnh);
                    ii++;                    
                }

                tlbModify.IsEnabled = true;
                tlbDelete.IsEnabled = true;
                tlbSubmit.IsEnabled = false;
                tlbApprove.IsEnabled = false;
                tlbRefuse.IsEnabled = false;
                tlbCancel.IsEnabled = false;
                //action = DatabaseConstant.Action.SUA;
                //sTrangThaiNVu = "CDU";
                //CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain, function);

                grDLieuHAnh.ItemsSource = null;
                grDLieuHAnh.ItemsSource = lstDuLieuHinhAnh;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                LMessage.ShowMessage("M_ResponseMessage_HanhDong_LayDuLieuKhongThanhCong", LMessage.MessageBoxType.Warning);
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        private void ResetForm()
        {
            sTrangThaiNVu = "";
            _objKH = null;
            //idKhachHang = 0;

            #region Thông tin chung
            
            
            #endregion

            #region Thông tin kiểm soát
            txtTrangThai.Text = "";
            teldtNgayNhap.Value = LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd");
            txtNguoiLap.Text = ClientInformation.TenDangNhap;
            teldtNgayCNhat.Value = null;
            txtNguoiCapNhat.Text = "";
            #endregion
        }

        private bool Validation()
        {
            try
            {
                if (lstDuLieuHinhAnh == null)
                {
                    LMessage.ShowMessage("M_ResponseMessage_QuanLyHinhAnh_KhongCoDuLieuHinhAnh", LMessage.MessageBoxType.Warning);
                    return false;
                }

                if (lstDuLieuHinhAnh.Count == 0)
                {
                    LMessage.ShowMessage("M_ResponseMessage_QuanLyHinhAnh_KhongCoDuLieuHinhAnh", LMessage.MessageBoxType.Warning);
                    return false;
                }

                if (txtMaKhachHang.IsNullOrEmpty())
                {
                    LMessage.ShowMessage("M_ResponseMessage_QuanLyHinhAnh_KhongCoDuLieuHinhAnh", LMessage.MessageBoxType.Warning);
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                return false;
            }
        }

        private void SetEnabledControls()
        {
            #region Thêm
            if (action == DatabaseConstant.Action.THEM)
            {
                //tlbAddHinhAnh.IsEnabled = true;
                //tlbModifyHinhAnh.IsEnabled = true;
                //tlbDeleteHinhAnh.IsEnabled = true;
                txtMaHSHA.IsEnabled = true;
                cmbDoiTuongHSHA.IsEnabled = true;
                cmbLoaiHSHA.IsEnabled = true;
                txtMaKhachHang.IsEnabled = false;
                btnKhachHang.IsEnabled = true;
            }
            #endregion

            #region Sửa
            else if (action == DatabaseConstant.Action.SUA)
            {
                //tlbAddHinhAnh.IsEnabled = true;
                //tlbModifyHinhAnh.IsEnabled = true;
                //tlbDeleteHinhAnh.IsEnabled = true;
                txtMaHSHA.IsEnabled = true;
                cmbDoiTuongHSHA.IsEnabled = true;
                cmbLoaiHSHA.IsEnabled = true;
                txtMaKhachHang.IsEnabled = false;
                btnKhachHang.IsEnabled = true; 
            }
            #endregion

            #region Xem ~ Mặc định
            else
            {
                //tlbAddHinhAnh.IsEnabled = false;
                //tlbModifyHinhAnh.IsEnabled = false;
                //tlbDeleteHinhAnh.IsEnabled = false;
                txtMaHSHA.IsEnabled = true;
                cmbDoiTuongHSHA.IsEnabled = true;
                cmbLoaiHSHA.IsEnabled = true;
                txtMaKhachHang.IsEnabled = false;
                btnKhachHang.IsEnabled = true;
            }
            #endregion
        }

        public void OnSave()
        {
            try
            {
                if (!Validation()) return;

                string trangThai = "";
                if (sTrangThaiNVu.Equals(BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri()))
                {
                    trangThai = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();
                }
                else
                {
                    trangThai = CommonFunction.LayTrangThaiBanGhi(DatabaseConstant.Action.LUU, BusinessConstant.layTrangThaiNghiepVu(sTrangThaiNVu));
                }

                GetFormData(trangThai);

                if (action == DatabaseConstant.Action.THEM)
                {
                    OnAddNew();
                }
                else if (action == DatabaseConstant.Action.SUA)
                {
                    OnModify();
                }
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }

        }

        public void BeforeViewFromDetail()
        {
            action = DatabaseConstant.Action.XEM;
            SetEnabledControls();
            CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain, function);
        }

        public void BeforeViewFromList()
        {
            SetFormData();
            BeforeViewFromDetail();
        }


        public void BeforeAddNew()
        {
            action = DatabaseConstant.Action.THEM;
            ResetForm();
            SetEnabledControls();
            CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain, function);
        }

        public void OnAddNew()
        {

            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                KhachHangProcess process = new KhachHangProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = false;

                ret = process.QuanLyHinhAnh(DatabaseConstant.Action.THEM, ref _objKH, ref lstCkyHanh, ref lstChuKyHinhAnh, ref listClientResponseDetail);
                AfterAddNew(ret, listClientResponseDetail);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        public void AfterAddNew(bool ret, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (ret)
                {
                    LMessage.ShowMessage("M.DungChung.ThemThanhCong", LMessage.MessageBoxType.Information);

                    if (cbMultiAdd.IsChecked == true)
                    {
                        ResetForm();
                    }
                    else
                    {
                        sTrangThaiNVu = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();              
                        BeforeViewFromDetail();
                        tlbDelete.IsEnabled = true;
                    }
                }
                else
                {
                    CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        public void BeforeModifyFromDetail()
        {
            try
            {
                // Yêu cầu lock bản ghi cần sửa
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                //listLockId.Add(_objKH.ID);

                //bool ret = process.LockData(DatabaseConstant.Module.TDTT,
                //    DatabaseConstant.Function.TD_SAN_PHAMTT,
                //    DatabaseConstant.Table.TD_SAN_PHAMTT,
                //    DatabaseConstant.Action.SUA,
                //    listLockId);
                bool ret = true;

                if (ret)
                {
                    //action = DatabaseConstant.Action.SUA;
                    //sTrangThaiNVu = "CDU";
                    //SetEnabledControls();
                    //CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain, function);

                    action = DatabaseConstant.Action.THEM;

                    string chucNang = "/PresentationWPF.KhachHang;component/QuanLyHinhAnh/ucQuanLyHinhAnh.xaml";
                    HeThong hethong = new HeThong();
                    if ((hethong.KiemTraQuyenTheoChucNang(chucNang, DatabaseConstant.Action.THEM)) ||
                        (hethong.KiemTraQuyenTheoChucNang(chucNang, DatabaseConstant.Action.SUA)) ||
                        (hethong.KiemTraQuyenTheoChucNang(chucNang, DatabaseConstant.Action.TOAN_QUYEN)))
                        tlbSubmit.IsEnabled = true;
                    else
                        tlbSubmit.IsEnabled = false;

                    if ((hethong.KiemTraQuyenTheoChucNang(chucNang, DatabaseConstant.Action.DUYET)) ||
                        (hethong.KiemTraQuyenTheoChucNang(chucNang, DatabaseConstant.Action.TOAN_QUYEN)))
                        tlbApprove.IsEnabled = true;
                    else
                        tlbApprove.IsEnabled = false;

                    if ((hethong.KiemTraQuyenTheoChucNang(chucNang, DatabaseConstant.Action.TU_CHOI_DUYET)) ||
                        (hethong.KiemTraQuyenTheoChucNang(chucNang, DatabaseConstant.Action.TOAN_QUYEN)))
                        tlbRefuse.IsEnabled = true;
                    else
                        tlbRefuse.IsEnabled = false;

                    if ((hethong.KiemTraQuyenTheoChucNang(chucNang, DatabaseConstant.Action.THOAI_DUYET)) ||
                        (hethong.KiemTraQuyenTheoChucNang(chucNang, DatabaseConstant.Action.TOAN_QUYEN)))
                        tlbCancel.IsEnabled = true;
                    else
                        tlbCancel.IsEnabled = false;
                }
                else
                {
                    LMessage.ShowMessage("M.ResponseMessage.Common.LockDataInvalid", LMessage.MessageBoxType.Information);
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        public void BeforeModifyFromList()
        {
            SetFormData();
            action = DatabaseConstant.Action.SUA;
            SetEnabledControls();
            CommonFunction.RefreshButton(Toolbar, action, sTrangThaiNVu, mnuMain, function);
        }

        public void OnModify()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                KhachHangProcess process = new KhachHangProcess();
                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = false;

                ret = process.QuanLyHinhAnh(DatabaseConstant.Action.SUA, ref _objKH, ref lstCkyHanh, ref lstChuKyHinhAnh, ref listClientResponseDetail);
                AfterModify(ret, listClientResponseDetail);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
            }
        }

        public void AfterModify(bool ret, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (ret)
                {
                    LMessage.ShowMessage("M.DungChung.CapNhatThanhCong", LMessage.MessageBoxType.Information);

                    sTrangThaiNVu = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();                                        

                    BeforeViewFromDetail();
                }
                else
                {
                    CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                    //LMessage.ShowMessage("M.DungChung.CapNhatKoThanhCong", LMessage.MessageBoxType.Error);
                }

                // Yêu cầu Unlock bản ghi cần sửa
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                //listLockId.Add(_objKH.ID);

                //bool retUnlockData = process.UnlockData(DatabaseConstant.Module.TDTT,
                //    DatabaseConstant.Function.TD_SAN_PHAMTT,
                //    DatabaseConstant.Table.TD_SAN_PHAMTT,
                //    DatabaseConstant.Action.SUA,
                //    listLockId);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        public void BeforeDelete()
        {
            try
            {
                // Cảnh báo người dùng
                MessageBoxResult ret = LMessage.ShowMessage("M.DungChung.HoiXoa", LMessage.MessageBoxType.Question);

                if (ret == MessageBoxResult.Yes)
                {
                    // Lock dữ liệu nếu người dùng chấp nhận cảnh báo
                    UtilitiesProcess process = new UtilitiesProcess();
                    List<int> listLockId = new List<int>();
                    //listLockId.Add(_objKH.ID);
                    List<int> listLockedId = new List<int>();

                    //bool retLockData = process.LockData(DatabaseConstant.Module.TDTT,
                    //    DatabaseConstant.Function.TD_SAN_PHAMTT,
                    //    DatabaseConstant.Table.TD_SAN_PHAMTT,
                    //    DatabaseConstant.Action.XOA,
                    //    listLockId);

                    bool retLockData = true;

                    if (retLockData)
                    {
                        action = DatabaseConstant.Action.XOA;
                        OnDelete();
                    }
                    else
                    {
                        LMessage.ShowMessage("M.ResponseMessage.Common.LockDataInvalid", LMessage.MessageBoxType.Information);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                // Yêu cầu unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                //listLockId.Add(_objKH.ID);
                List<int> listUnlockId = new List<int>();

                //bool retUnlockData = process.UnlockData(DatabaseConstant.Module.KHTV,
                //    DatabaseConstant.Function.KH_CA_NHAN,
                //    DatabaseConstant.Table.TD_SAN_PHAMTT,
                //    DatabaseConstant.Action.XOA,
                //    listLockId);

                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        public void OnDelete()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
            KhachHangProcess process = new KhachHangProcess();
            try
            {
                bool ret = false;
                ret = process.QuanLyHinhAnh(DatabaseConstant.Action.XOA, ref _objKH, ref lstCkyHanh, ref lstChuKyHinhAnh, ref listClientResponseDetail);
                AfterDelete(ret, listClientResponseDetail);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
                listClientResponseDetail = null;
                process = null;
            }
        }

        public void AfterDelete(bool ret, List<ClientResponseDetail> listClientResponseDetail)
        {
            try
            {
                if (ret)
                {
                    LMessage.ShowMessage("M.DungChung.XoaThanhCong", LMessage.MessageBoxType.Information);
                    // Đóng cửa sổ chi tiết sau khi xóa
                    OnClose();
                }
                else
                {
                    CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                    //LMessage.ShowMessage("M.DungChung.XoaKhongThanhCong", LMessage.MessageBoxType.Warning);
                }

                // Yêu cầu unlock dữ liệu
                UtilitiesProcess process = new UtilitiesProcess();
                List<int> listLockId = new List<int>();
                //listLockId.Add(_objKH.ID);
                List<int> listUnlockId = new List<int>();

                //bool retUnlockData = process.UnlockData(DatabaseConstant.Module.TDTT,
                //    DatabaseConstant.Function.TD_SAN_PHAMTT,
                //    DatabaseConstant.Table.TD_SAN_PHAMTT,
                //    DatabaseConstant.Action.XOA,
                //    listLockId);

                
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        public void BeforeApprove()
        {
            try
            {
                // Cảnh báo người dùng
                MessageBoxResult ret = LMessage.ShowMessage("M.DungChung.HoiDuyet", LMessage.MessageBoxType.Question);

                if (ret == MessageBoxResult.Yes)
                {
                    // Lock dữ liệu nếu người dùng chấp nhận cảnh báo
                    //UtilitiesProcess process = new UtilitiesProcess();
                    //List<int> listLockId = new List<int>();
                    //listLockId.Add(id);
                    //List<int> listLockedId = new List<int>();

                    //bool retLockData = process.LockData(DatabaseConstant.Module.KHTV,
                    //    DatabaseConstant.Function.KH_THANH_VIEN,
                    //    DatabaseConstant.Table.KH_KHANG_HSO,
                    //    DatabaseConstant.Action.DUYET,
                    //    listLockId);

                    bool retLockData = true;

                    if (retLockData)
                    {
                        action = DatabaseConstant.Action.DUYET;
                        OnApprove();
                    }
                    else
                    {
                        LMessage.ShowMessage("M.ResponseMessage.Common.LockDataInvalid", LMessage.MessageBoxType.Information);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        public void OnApprove()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
            KhachHangProcess processKhachHang = new KhachHangProcess();
            try
            {
                bool ret = false;
                ret = processKhachHang.QuanLyHinhAnh(DatabaseConstant.Action.DUYET, ref _objKH, ref lstCkyHanh, ref lstChuKyHinhAnh, ref listClientResponseDetail);
                AfterApprove(ret, listClientResponseDetail);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
                listClientResponseDetail = null;
                processKhachHang = null;
            }
        }

        public void AfterApprove(bool ret, List<ClientResponseDetail> listClientResponseDetail)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
            try
            {
                if (ret)
                {
                    LMessage.ShowMessage("M.DungChung.DuyetThanhCong", LMessage.MessageBoxType.Information);
                    CommonFunction.ThongBaoKetQua(listClientResponseDetail);

                    sTrangThaiNVu = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();
                    BeforeViewFromDetail();
                }
                else
                {
                    CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                }

                // Yêu cầu unlock dữ liệu
                //UtilitiesProcess process = new UtilitiesProcess();
                //List<int> listLockId = new List<int>();
                //listLockId.Add(id);
                //List<int> listUnlockId = new List<int>();

                //bool retUnlockData = process.UnlockData(DatabaseConstant.Module.KHTV,
                //    DatabaseConstant.Function.KH_THANH_VIEN,
                //    DatabaseConstant.Table.KH_KHANG_HSO,
                //    DatabaseConstant.Action.DUYET,
                //    listLockId);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        public void BeforeRefuse()
        {
            try
            {
                // Cảnh báo người dùng
                MessageBoxResult ret = LMessage.ShowMessage("M.DungChung.HoiTuChoiDuyet", LMessage.MessageBoxType.Question);

                if (ret == MessageBoxResult.Yes)
                {
                    // Lock dữ liệu nếu người dùng chấp nhận cảnh báo
                    //UtilitiesProcess process = new UtilitiesProcess();
                    //List<int> listLockId = new List<int>();
                    //listLockId.Add(id);
                    //List<int> listLockedId = new List<int>();

                    //bool retLockData = process.LockData(DatabaseConstant.Module.KHTV,
                    //    DatabaseConstant.Function.KH_THANH_VIEN,
                    //    DatabaseConstant.Table.KH_KHANG_HSO,
                    //    DatabaseConstant.Action.DUYET,
                    //    listLockId);

                    bool retLockData = true;

                    if (retLockData)
                    {
                        action = DatabaseConstant.Action.TU_CHOI_DUYET;
                        OnRefuse();
                    }
                    else
                    {
                        LMessage.ShowMessage("M.ResponseMessage.Common.LockDataInvalid", LMessage.MessageBoxType.Information);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        public void OnRefuse()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
            KhachHangProcess processKhachHang = new KhachHangProcess();
            try
            {
                bool ret = false;
                ret = processKhachHang.QuanLyHinhAnh(DatabaseConstant.Action.TU_CHOI_DUYET, ref _objKH, ref lstCkyHanh, ref lstChuKyHinhAnh, ref listClientResponseDetail);
                AfterRefuse(ret, listClientResponseDetail);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
                listClientResponseDetail = null;
                processKhachHang = null;
            }
        }

        public void AfterRefuse(bool ret, List<ClientResponseDetail> listClientResponseDetail)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
            try
            {
                if (ret)
                {
                    LMessage.ShowMessage("M.DungChung.TuChoiDuyetThanhCong", LMessage.MessageBoxType.Information);
                    CommonFunction.ThongBaoKetQua(listClientResponseDetail);

                    sTrangThaiNVu = BusinessConstant.TrangThaiNghiepVu.TU_CHOI.layGiaTri();
                    BeforeViewFromDetail();
                }
                else
                {
                    CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                }

                // Yêu cầu unlock dữ liệu
                //UtilitiesProcess process = new UtilitiesProcess();
                //List<int> listLockId = new List<int>();
                //listLockId.Add(id);
                //List<int> listUnlockId = new List<int>();

                //bool retUnlockData = process.UnlockData(DatabaseConstant.Module.KHTV,
                //    DatabaseConstant.Function.KH_THANH_VIEN,
                //    DatabaseConstant.Table.KH_KHANG_HSO,
                //    DatabaseConstant.Action.DUYET,
                //    listLockId);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        public void BeforeCancel()
        {
            try
            {
                // Cảnh báo người dùng
                MessageBoxResult ret = LMessage.ShowMessage("M.DungChung.HoiThoaiDuyet", LMessage.MessageBoxType.Question);

                if (ret == MessageBoxResult.Yes)
                {
                    // Lock dữ liệu nếu người dùng chấp nhận cảnh báo
                    //UtilitiesProcess process = new UtilitiesProcess();
                    //List<int> listLockId = new List<int>();
                    //listLockId.Add(id);
                    //List<int> listLockedId = new List<int>();

                    //bool retLockData = process.LockData(DatabaseConstant.Module.KHTV,
                    //    DatabaseConstant.Function.KH_THANH_VIEN,
                    //    DatabaseConstant.Table.KH_KHANG_HSO,
                    //    DatabaseConstant.Action.DUYET,
                    //    listLockId);

                    bool retLockData = true;

                    if (retLockData)
                    {
                        action = DatabaseConstant.Action.THOAI_DUYET;
                        OnCancel();
                    }
                    else
                    {
                        LMessage.ShowMessage("M.ResponseMessage.Common.LockDataInvalid", LMessage.MessageBoxType.Information);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        public void OnCancel()
        {
            Mouse.OverrideCursor = Cursors.Wait;
            List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
            KhachHangProcess processKhachHang = new KhachHangProcess();
            try
            {
                bool ret = false;
                ret = processKhachHang.QuanLyHinhAnh(DatabaseConstant.Action.THOAI_DUYET, ref _objKH, ref lstCkyHanh, ref lstChuKyHinhAnh, ref listClientResponseDetail);
                AfterCancel(ret, listClientResponseDetail);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
                listClientResponseDetail = null;
                processKhachHang = null;
            }
        }

        public void AfterCancel(bool ret, List<ClientResponseDetail> listClientResponseDetail)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
            try
            {
                if (ret)
                {
                    LMessage.ShowMessage("M.DungChung.ThoaiDuyetThanhCong", LMessage.MessageBoxType.Information);
                    CommonFunction.ThongBaoKetQua(listClientResponseDetail);

                    sTrangThaiNVu = BusinessConstant.TrangThaiNghiepVu.THOAI_DUYET.layGiaTri();
                    BeforeViewFromDetail();
                }
                else
                {
                    CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                }

                // Yêu cầu unlock dữ liệu
                //UtilitiesProcess process = new UtilitiesProcess();
                //List<int> listLockId = new List<int>();
                //listLockId.Add(id);
                //List<int> listUnlockId = new List<int>();

                //bool retUnlockData = process.UnlockData(DatabaseConstant.Module.KHTV,
                //    DatabaseConstant.Function.KH_THANH_VIEN,
                //    DatabaseConstant.Table.KH_KHANG_HSO,
                //    DatabaseConstant.Action.DUYET,
                //    listLockId);
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
        }
        #endregion                        
    }
}
