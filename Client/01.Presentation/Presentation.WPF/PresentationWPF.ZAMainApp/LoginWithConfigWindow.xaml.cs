using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Threading;
using Telerik.Windows.Controls;
using Utilities.Common;
using Presentation.Process;
using Presentation.Process.Common;
using PresentationWPF.ZAMainApp.Localization;
using PresentationWPF.CustomControl;
using Microsoft.Windows.Controls.Ribbon;
using Presentation.Process.ZAMainAppServiceRef;
using System.Data;

namespace PresentationWPF.ZAMainApp
{
    

    /// <summary>
    /// Interaction logic for LoginWithConfigWindow.xaml
    /// </summary>
    public partial class LoginWithConfigWindow : Window
    {
        private string pathFolderLanguages = "";
        private ZAMainAppProcess process = null;

        public LoginWithConfigWindow()
        {
            InitializeComponent();
            process = new ZAMainAppProcess();

            //Icon & title setting
            CommonFunction.setIcon(this);
            this.Title += " - " + ClientInformation.ShortName;

            ApplicationConstant.DonViSuDung company = ApplicationConstant.layDonViSuDung(ClientInformation.Company);

            if (company == ApplicationConstant.DonViSuDung.BINHKHANH)
            {
                lblLanguage.Visibility = Visibility.Hidden;
                cboLanguage.Visibility = Visibility.Hidden;
                lblTitle.Visibility = Visibility.Visible;
            }

            string ServerPattern = ClientInformation.ServerList;
            Server server = new Server();
            List<Server> ServerList = server.getServerList(ServerPattern);

            foreach (Server item in ServerList)
            {
                MenuItem mnuItem = new MenuItem();
                mnuItem.Name = "" + item.ServerName;
                mnuItem.Header = "" + item.ServerName;
                mnuItem.Uid = "" + item.ServerIP + "#" + item.ServerPort;
                if (item.ServerName.Equals(ClientInformation.ServerName))
                {
                    mnuItem.Icon = new Image { Source = new BitmapImage(new Uri(@"pack://application:,,,/Utilities.Common;component/Images/Action/approve.png", UriKind.RelativeOrAbsolute)), Width = 12, Height = 12 };
                }
                mnuItem.Click += mnu_Click;

                mnuConfig.Items.Add(mnuItem);
            }
        }        

        private void mnu_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = (MenuItem)sender;
            string name = menuItem.Name;
            string ip = menuItem.Uid.SplitByDelimiter("#")[0];
            string port = menuItem.Uid.SplitByDelimiter("#")[1];

            if (!name.Equals(ClientInformation.ServerName))
            {
                Presentation.Process.Common.Utilities.SaveConfiguration(name, ip, port);
                string ServerPattern = ClientInformation.ServerList;
                Server server = new Server();
                List<Server> ServerList = server.getServerList(ServerPattern);
                mnuConfig.Items.Clear();
                foreach (Server item in ServerList)
                {
                    MenuItem mnuItem = new MenuItem();
                    mnuItem.Name = "" + item.ServerName;
                    mnuItem.Header = "" + item.ServerName;
                    mnuItem.Uid = "" + item.ServerIP + "#" + item.ServerPort;
                    if (item.ServerName.Equals(ClientInformation.ServerName))
                    {
                        mnuItem.Icon = new Image { Source = new BitmapImage(new Uri(@"pack://application:,,,/Utilities.Common;component/Images/Action/approve.png", UriKind.RelativeOrAbsolute)), Width = 12, Height = 12 };
                    }
                    mnuItem.Click += mnu_Click;

                    mnuConfig.Items.Add(mnuItem);
                }
            }
            else
            { }
        }        

        private delegate void LoadImageDelegate(Image obj, ImageSource value);

        /// <summary>
        /// Xử lý sự kiện Login
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (Validation())
            {
                //Load image
                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                bi.UriSource = new Uri(@"pack://application:,,,/Utilities.Common;component/Images/Other/M7_plant.gif", UriKind.RelativeOrAbsolute);
                bi.EndInit();
                LoadImageDelegate loadImageDelegate = new LoadImageDelegate(ImageBehavior.SetAnimatedSource);
                Application.Current.Dispatcher.Invoke(loadImageDelegate, DispatcherPriority.Background, new object[] { imgWaiting, bi });

                //Load config image
                //BitmapImage biConfig = new BitmapImage();
                //biConfig.BeginInit();
                //biConfig.UriSource = new Uri(@"pack://application:,,,/Utilities.Common;component/Images/Action/detail_process.png", UriKind.RelativeOrAbsolute);
                //biConfig.EndInit();

                Login();
            }
        }

        private void Login()
        {
            try
            {
                // @TrườngNX on 20120905
                // Lựa chọn ngôn ngữ của phiên làm việc
                // Căn cứ vào ngôn ngữ, load file language resources tương ứng
                //this.Cursor = Cursors.Wait;

                //var process = new ZAMainAppProcess();
                string userName = txtUsername.Text;
                string txtMD5 = LSecurity.MD5Encrypt(txtPassword.Password);
                //process.createSession();
                string responseMessage = "";
                NGON_NGU_DTO ngonNguDTO = null;
                bool ret = process.doLoginWithSession(userName, txtMD5, ref ngonNguDTO, ref responseMessage);

                // Nếu thành công
                if (ret)
                {
                    // Nếu yêu cầu cập nhật phiên bản
                    if (responseMessage != null &&
                    responseMessage.Equals(ApplicationConstant.CommonResponseMessage.M_ResponseMessage_Common_NotLatestVersion.layGiaTri())
                    )
                    {
                        //MessageBoxResult retQuestion = LMessage.ShowMessage(responseMessage, LMessage.MessageBoxType.Question);
                        //if (retQuestion == MessageBoxResult.Yes)
                        //{
                        //    // Cập nhật phiên bản
                        //}
                        //else
                        //{
                        //}

                        MessageBoxResult retMessage = LMessage.ShowMessage(responseMessage, LMessage.MessageBoxType.Question);
                        if (retMessage == MessageBoxResult.Yes)
                        {
                            UserControl p = null;
                            bool stretchWindow = false;
                            p = new PresentationWPF.QuanTriHeThong.PhienBan.ucPhienBanCapNhat();

                            if (p != null)
                            {
                                Window window = new Window
                                {
                                    Title = LLanguage.SearchResourceByKey("MENU.152_CAP_NHAT_PB"),
                                    WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen,
                                    //Icon = (rmb.ImageSource == null ? this.Icon : rmb.ImageSource),
                                    Width = (Double.IsNaN(p.Width) ? 1024 : p.Width),
                                    Height = (Double.IsNaN(p.Height) ? 700 : p.Height),
                                    Content = p,
                                };
                                if (stretchWindow == true)
                                {
                                    window.WindowState = WindowState.Maximized;
                                }
                                window.Width = 500;
                                window.Height = 150;
                                window.ResizeMode = ResizeMode.NoResize;

                                //Mouse.OverrideCursor = Cursors.Arrow;
                                this.Close();
                                window.ShowDialog();

                            }
                            else
                            {
                                Mouse.OverrideCursor = Cursors.Arrow;
                                LMessage.ShowMessage("Không tìm thấy chức năng này", LMessage.MessageBoxType.Warning);
                            }
                        }
                    }

                    ClientInformation.NgonNgu = ((RadComboBoxItem)cboLanguage.SelectedItem).Tag.ToString();

                    // @Truonglq set Culture ngôn ngữ cho  Coltrol (telerik)
                    //CultureInfo culture = new CultureInfo(((RadComboBoxItem)cboLanguage.SelectedItem).Tag.ToString());
                    //System.Threading.Thread.CurrentThread.CurrentCulture = culture;
                    //System.Threading.Thread.CurrentThread.CurrentUICulture = culture;
                    InitializeCultures();
                    //InitializeCulturesTelerik();
                    LocalizationManager.Manager = new LocalizationManager()
                    {
                        ResourceManager = LocalizationRadControl.ResourceManager
                    };

                    // Khởi tạo phiên làm việc, chọn đơn vị nghiệp vụ cần thao tác dữ liệu
                    if (!ClientInformation.LoaiNguoiSuDung.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_SA.layGiaTri()))
                    {
                        InitSession initSession = new InitSession(ClientInformation.ListPhongGD);

                        initSession.ResizeMode = ResizeMode.NoResize;
                        initSession.Height = 250;
                        initSession.Width = 600;
                        initSession.WindowStartupLocation = WindowStartupLocation.CenterScreen;

                        initSession.lblNguoiDungValue.Content = "(" + ClientInformation.TenDangNhap + ") " + ClientInformation.HoTen;
                        initSession.lblChiNhanhValue.Content = "(" + ClientInformation.MaDonVi + ") " + ClientInformation.TenDonVi;
                        //initSession.ListPhongGD = ClientInformation.ListPhongGD;

                        initSession.Show();
                        this.Cursor = Cursors.Arrow;
                        Close();
                    }
                    else
                    {
                        ClientInformation.TenDonViGiaoDich = ClientInformation.TenDonVi;
                        ClientInformation.MaDonViGiaoDich = ClientInformation.MaDonVi;
                        ClientInformation.IdDonViGiaoDich = ClientInformation.IdDonVi;
                        ClientInformation.PhuongPhapHachToan = null;

                        MainWindow mainwindow = new MainWindow();
                        mainwindow.Show();
                        this.Cursor = Cursors.Arrow;
                        Close();
                    }
                }
                else
                {
                    ImageBehavior.SetAnimatedSource(imgWaiting, null);

                    // Nếu yêu cầu đổi mật khẩu
                    if (responseMessage != null &&
                        (
                        responseMessage.Equals(ApplicationConstant.LoginResponseMessage.M_ResponseMessage_Login_YeuCauDoiMatKhauLanDau.layGiaTri()) ||
                        responseMessage.Equals(ApplicationConstant.LoginResponseMessage.M_ResponseMessage_Login_YeuCauDoiMatKhau.layGiaTri())
                        )
                        )
                    {
                        MessageBoxResult retQuestion = LMessage.ShowMessage(responseMessage, LMessage.MessageBoxType.Question);
                        if (retQuestion == MessageBoxResult.Yes)
                        {
                            txtPassword.Clear();

                            ChangePassword changePassword = new ChangePassword();
                            Window window = new Window();

                            window.ResizeMode = ResizeMode.NoResize;
                            window.Height = 180;
                            window.Width = 500;
                            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                            window.Title = LLanguage.SearchResourceByKey("U.ZAMainApp.ChangePassword.Title");
                            window.Content = changePassword;

                            changePassword.userName = userName;
                            window.ShowDialog();
                            //LMessage.ShowMessage("Change pass", LMessage.MessageBoxType.Warning);
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        LMessage.ShowMessage(responseMessage, LMessage.MessageBoxType.Warning);
                    }
                }
            }
            //else
            catch (Exception ex)
            {
                ImageBehavior.SetAnimatedSource(imgWaiting, null);

                //this.Close();
                //Application.Current.Shutdown();
                this.Cursor = Cursors.Arrow;
                if (ex.GetType() == typeof(CustomException))
                    new frmThongBaoLoi(ex.Message, ex.InnerException).ShowDialog();
                else
                {
                    if (ex.InnerException != null)
                    {
                        if (ex.InnerException.GetType() == typeof(CustomException))
                            new frmThongBaoLoi(ex.InnerException.Message, ex.InnerException).ShowDialog();
                        else
                            new frmThongBaoLoi("M.ResponseMessage.Login.KhongThanhCong", ex).ShowDialog();
                    }
                    else
                        new frmThongBaoLoi("M.ResponseMessage.Login.KhongThanhCong", ex).ShowDialog();
                }
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        /// <summary>
        /// Thiết lập Culture theo ngôn ngữ và tham số hệ thống
        /// </summary>
        /// <summary>
        /// Thiết lập Culture theo ngôn ngữ và tham số hệ thống
        /// </summary>
        private void InitializeCultures()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(ClientInformation.NgonNgu);//ClientInformation.NgonNgu);
            NumberFormatInfo nfi = new NumberFormatInfo();
            DateTimeFormatInfo dfi = new DateTimeFormatInfo();
            //nfi.NumberGroupSeparator = ".";
            //nfi.NumberDecimalSeparator = ",";
            //dfi.DateSeparator = "-";
            Thread.CurrentThread.CurrentCulture.NumberFormat = nfi;
            Thread.CurrentThread.CurrentCulture.DateTimeFormat = dfi;
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
        }

        /// <summary>
        /// Thiết lập Culture theo ngôn ngữ và tham số hệ thống
        /// </summary>
        //private void InitializeCulturesTelerik()
        //{
        //    Thread.CurrentThread.CurrentCulture = new CultureInfo(ClientInformation.NgonNgu);//ClientInformation.NgonNgu);
        //    //NumberFormatInfo nfi = new NumberFormatInfo();
        //    //DateTimeFormatInfo dfi = new DateTimeFormatInfo();
        //    //nfi.NumberGroupSeparator = "%";
        //    //dfi.DateSeparator = "-";
        //    //Thread.CurrentThread.CurrentCulture.NumberFormat = nfi;
        //    //Thread.CurrentThread.CurrentCulture.DateTimeFormat = dfi;
        //    Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
        //}

        /// <summary>
        /// Xử lý sự kiện Clear
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txtUsername.Clear();
            txtPassword.Clear();
        }

        /// <summary>
        /// Xử lý sự kiện Cancel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        /// <summary>
        /// Xử lý sự kiện khi thay đổi ngôn ngữ khi đăng nhập
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cboLanguage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // @TrườngNX on 20120905
            // Lựa chọn ngôn ngữ của phiên làm việc
            // Căn cứ vào ngôn ngữ, load file language resources tương ứng
            ChangeLanguage();
        }

        /// <summary>
        /// Load form login
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Hiển thị theo ngôn ngữ mặc định
            ChangeLanguage();
            txtUsername.Focus();

            //List<string> lst = new List<string>() { "1", "2" };
            //foreach (string item in lst)
            //{
            //    MenuItem mnuItem = new MenuItem();
            //    mnuItem.Name = item;
            //    mnuItem.Header = item;
            //    mnuConfig.Items.Add(mnuItem);
            //}
        }

        /// <summary>
        /// Namnt - 20120912
        /// Thay đổi ngôn ngữ
        /// </summary>
        private void ChangeLanguage()
        {
            try
            {
                // Tìm đường dẫn tới folder chứa các file language
                if (string.IsNullOrEmpty(pathFolderLanguages))
                {
                    //pathFolderLanguages = LFile.GetPropertyInXml(Environment.CurrentDirectory + "\\config\\config.conf", "LanguagesDir").ToString();
                    // Truongnx, đổi thông tin load đường dẫn languages
                    pathFolderLanguages = Environment.CurrentDirectory + "\\languages";
                }

                // Thay đổi ngôn ngữ
                if (!LLanguage.ApplyLanguage(pathFolderLanguages, cboLanguage.SelectedValue.ToString().Substring(0, 2)))
                {
                    new frmThongBaoLoi("M.ZAMainApp.LoginWindow.LoiDoiNgonNgu", null).ShowDialog();
                }
            }
            catch (Exception ex)
            {
                new frmThongBaoLoi("M.ZAMainApp.LoginWindow.LoiDoiNgonNgu", ex).ShowDialog();
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        private bool Validation()
        {
            if (string.IsNullOrEmpty(txtUsername.Text))
            {
                LMessage.ShowMessage("M.ZAMainApp.LoginWindow.ThieuTenDangNhap", LMessage.MessageBoxType.Warning);
                txtUsername.Focus();
                return false;
            }

            else if (LString.IsNullOrEmptyOrSpace(txtPassword.Password))
            {
                LMessage.ShowMessage("M.ZAMainApp.LoginWindow.ThieuMatKhau", LMessage.MessageBoxType.Warning);
                txtPassword.Focus();
                return false;
            }
            return true;
        }
    }
}
