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
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Linq;
using System.Data;

namespace PresentationWPF.ZAMainApp
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private string pathFolderLanguages = "";
        private ZAMainAppProcess process = null;

        public LoginWindow()
        {
            InitializeComponent();

            // IP, MAC Address
            new ClientInitProcess().docThongTinCauHinhClient(1);

            // No IP, MAC Address
            //docThongTinCauHinhClient(1);

            if (!ClientInformation.Theme.Equals("default"))
                SetThemes();

            /* LANGUAGE */
            if (!ClientInformation.LanguageList.Equals(""))
            {
                string LanguagePattern = ClientInformation.LanguageList;
                List<Language> LanguageList = (new Language()).getLanguageList(LanguagePattern);

                int i = 0;
                int index = 0;
                string value = "";
                foreach (Language e in LanguageList)
                {
                    RadComboBoxItem item = new RadComboBoxItem();
                    item.Content = e.LanguageName; item.Tag = e.LanguageCode;
                    cboLanguage.Items.Add(item);

                    if (e.LanguageStatus.Equals("1"))
                    {
                        index = i;
                        value = e.LanguageStatus;
                    }
                    ++i;
                }

                cboLanguage.SelectedIndex = index;
                //cboLanguage.SelectedValue = value;
            }
            else
            { }
            /* LANGUAGE */

            process = new ZAMainAppProcess();

            //Icon & title setting
            CommonFunction.setIcon(this);

            //Set licence Aspose Words and Cells
            MemoryStream aspLic = new MemoryStream();
            StreamWriter sw = new StreamWriter(aspLic, Encoding.UTF8);
            Aspose.Words.License licenseWord = new Aspose.Words.License();
            //Set the license of Aspose.Cells to avoid the evaluation
            //limitations
            sw.WriteLine(BusinessConstant.asposeWordLic);
            sw.Flush();
            aspLic.Position = 0;
            licenseWord.SetLicense(aspLic);

            Aspose.Cells.License licenseExcel = new Aspose.Cells.License();
            //Set the license of Aspose.Cells to avoid the evaluation
            //limitations
            sw.WriteLine(BusinessConstant.asposeWordLic);
            sw.Flush();
            aspLic.Position = 0;
            licenseExcel.SetLicense(aspLic);

            ApplicationConstant.DonViSuDung company = ApplicationConstant.layDonViSuDung(ClientInformation.Company);

            if (company == ApplicationConstant.DonViSuDung.BINHKHANH)
            {
                lblLanguage.Visibility = Visibility.Hidden;
                cboLanguage.Visibility = Visibility.Hidden;
                lblTitle.Visibility = Visibility.Visible;

                //imgConfig.Visibility = Visibility.Collapsed;
                string ServerPattern = ClientInformation.ServerList;
                if (!string.IsNullOrEmpty(ServerPattern))
                {
                    Server server = new Server();
                    List<Server> ServerList = server.getServerList(ServerPattern);

                    if (ServerList.Count <= 1)
                    {
                        imgConfig.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        foreach (Server item in ServerList)
                        {
                            MenuItem mnuItem = new MenuItem();
                            mnuItem.Name = "" + item.ServerName;
                            mnuItem.Header = "" + item.ServerName;
                            mnuItem.Uid = "" + item.ServerIP + "#" + item.ServerPort + "#" + item.ServerCode;
                            if (item.ServerName.Equals(ClientInformation.ServerName))
                            {
                                mnuItem.Icon = new Image { Source = new BitmapImage(new Uri(@"pack://application:,,,/Utilities.Common;component/Images/Action/approve.png", UriKind.RelativeOrAbsolute)), Width = 12, Height = 12 };
                            }
                            mnuItem.Click += mnu_Click;

                            mnuConfig.Items.Add(mnuItem);
                        }
                    }
                }
                else
                {
                    imgConfig.Visibility = Visibility.Collapsed;
                }
            }
            else if (company == ApplicationConstant.DonViSuDung.M7MFI)
            {
                string ServerPattern = ClientInformation.ServerList;
                if (!string.IsNullOrEmpty(ServerPattern))
                {
                    Server server = new Server();
                    List<Server> ServerList = server.getServerList(ServerPattern);

                    if (ServerList.Count <= 1)
                    {
                        imgConfig.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        foreach (Server item in ServerList)
                        {
                            MenuItem mnuItem = new MenuItem();
                            mnuItem.Name = "" + item.ServerName;
                            mnuItem.Header = "" + item.ServerName;
                            mnuItem.Uid = "" + item.ServerIP + "#" + item.ServerPort + "#" + item.ServerCode;
                            if (item.ServerName.Equals(ClientInformation.ServerName))
                            {
                                mnuItem.Icon = new Image { Source = new BitmapImage(new Uri(@"pack://application:,,,/Utilities.Common;component/Images/Action/approve.png", UriKind.RelativeOrAbsolute)), Width = 12, Height = 12 };
                            }
                            mnuItem.Click += mnu_Click;

                            mnuConfig.Items.Add(mnuItem);
                        }
                    }
                }
                else 
                {
                    imgConfig.Visibility = Visibility.Collapsed;
                }
            }
            else 
            {
                if (company == ApplicationConstant.DonViSuDung.BIDV
                    || company == ApplicationConstant.DonViSuDung.BIDV_BLF)
                {
                    //cboLanguage.SelectedIndex = 1;
                }

                //imgConfig.Visibility = Visibility.Collapsed;
                string ServerPattern = ClientInformation.ServerList;
                if (!string.IsNullOrEmpty(ServerPattern))
                {
                    Server server = new Server();
                    List<Server> ServerList = server.getServerList(ServerPattern);

                    if (ServerList.Count <= 1)
                    {
                        imgConfig.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        foreach (Server item in ServerList)
                        {
                            MenuItem mnuItem = new MenuItem();
                            mnuItem.Name = "" + item.ServerName;
                            mnuItem.Header = "" + item.ServerName;
                            mnuItem.Uid = "" + item.ServerIP + "#" + item.ServerPort + "#" + item.ServerCode;
                            if (item.ServerName.Equals(ClientInformation.ServerName))
                            {
                                mnuItem.Icon = new Image { Source = new BitmapImage(new Uri(@"pack://application:,,,/Utilities.Common;component/Images/Action/approve.png", UriKind.RelativeOrAbsolute)), Width = 12, Height = 12 };
                            }
                            mnuItem.Click += mnu_Click;

                            mnuConfig.Items.Add(mnuItem);
                        }
                    }
                }
                else
                {
                    imgConfig.Visibility = Visibility.Collapsed;
                }
            }
            cboLanguage.SelectionChanged += cboLanguage_SelectionChanged;
        }

        private void mnu_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = (MenuItem)sender;
            string name = menuItem.Name;
            string ip = menuItem.Uid.SplitByDelimiter("#")[0];
            string port = menuItem.Uid.SplitByDelimiter("#")[1];
            string company = menuItem.Uid.SplitByDelimiter("#")[2];

            if (!name.Equals(ClientInformation.ServerName))
            {
                Presentation.Process.Common.Utilities.SaveConfiguration(name, ip, port, company);
                string ServerPattern = ClientInformation.ServerList;
                Server server = new Server();
                List<Server> ServerList = server.getServerList(ServerPattern);
                mnuConfig.Items.Clear();
                foreach (Server item in ServerList)
                {
                    MenuItem mnuItem = new MenuItem();
                    mnuItem.Name = "" + item.ServerName;
                    mnuItem.Header = "" + item.ServerName;
                    mnuItem.Uid = "" + item.ServerIP + "#" + item.ServerPort + "#" + item.ServerCode;
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
                bool ret = process.doLoginWithSession(userName, txtMD5,ref ngonNguDTO, ref responseMessage);

                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, "ret value: " + ret.ToString());
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, "responseMessage value: " + responseMessage);
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


                    #region Ghi ngôn ngữ ra file UIResources                    
                    if (ngonNguDTO != null)
                    {
                        List<string> lstResource_vi = new List<string>();
                        List<string> lstResource_en = new List<string>();
                        List<string> lstResource_gl = new List<string>();
                        List<string> lstMessage_vi = new List<string>();
                        List<string> lstMessage_en = new List<string>();
                        List<string> lstMessage_gl = new List<string>();

                        string pathFileResource_vi = ClientInformation.LanguagesDir + "\\UIResources_vi.xaml";
                        string pathFileResource_en = ClientInformation.LanguagesDir + "\\UIResources_en.xaml";
                        string pathFileResource_gl = ClientInformation.LanguagesDir + "\\UIResources_gl.xaml";
                        string pathFileMessage_vi = ClientInformation.LanguagesDir + "\\MessageResources_vi.xaml";
                        string pathFileMessage_en = ClientInformation.LanguagesDir + "\\MessageResources_en.xaml";
                        string pathFileMessage_gl = ClientInformation.LanguagesDir + "\\MessageResources_gl.xaml";


                        string begin = "<ResourceDictionary xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\""
                                    + "\n                " + "xmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\""
                                    + "\n                " + "xmlns:my=\"clr-namespace:System;assembly=mscorlib\">";
                        string end = "\n\n</ResourceDictionary>";



                        if (!ngonNguDTO.checkResource && ngonNguDTO.lstResource != null)
                        {
                            lstResource_vi.Add(begin);
                            lstResource_en.Add(begin);
                            lstResource_gl.Add(begin);

                            foreach (var item in ngonNguDTO.lstResource)
                            {
                                lstResource_vi.Add("\n    <my:String x:Key=\"" + item.MA + "\">" + item.VIET_NAM + "</my:String>");
                                lstResource_en.Add("\n    <my:String x:Key=\"" + item.MA + "\">" + item.ENGLISH + "</my:String>");
                                lstResource_gl.Add("\n    <my:String x:Key=\"" + item.MA + "\">" + item.GLOBAL + "</my:String>");
                            }

                            lstResource_vi.Add(end);
                            lstResource_en.Add(end);
                            lstResource_gl.Add(end);

                            LFile.DeleteFile(pathFileResource_vi);
                            LFile.DeleteFile(pathFileResource_en);
                            LFile.DeleteFile(pathFileResource_gl);

                            LFile.WriteListTextToFile(pathFileResource_vi, true, lstResource_vi);
                            LFile.WriteListTextToFile(pathFileResource_en, true, lstResource_en);
                            LFile.WriteListTextToFile(pathFileResource_gl, true, lstResource_gl);
                        }

                        if (!ngonNguDTO.checkMessage && ngonNguDTO.lstMessage != null)
                        {
                            lstMessage_vi.Add(begin);
                            lstMessage_en.Add(begin);
                            lstMessage_gl.Add(begin);

                            foreach (var item in ngonNguDTO.lstMessage)
                            {
                                lstMessage_vi.Add("\n    <my:String x:Key=\"" + item.MA + "\">" + item.VIET_NAM + "</my:String>");
                                lstMessage_en.Add("\n    <my:String x:Key=\"" + item.MA + "\">" + item.ENGLISH + "</my:String>");
                                lstMessage_gl.Add("\n    <my:String x:Key=\"" + item.MA + "\">" + item.GLOBAL + "</my:String>");
                            }

                            lstMessage_vi.Add(end);
                            lstMessage_en.Add(end);
                            lstMessage_gl.Add(end);

                            LFile.DeleteFile(pathFileMessage_vi);
                            LFile.DeleteFile(pathFileMessage_en);
                            LFile.DeleteFile(pathFileMessage_gl);

                            LFile.WriteListTextToFile(pathFileMessage_vi, true, lstMessage_vi);
                            LFile.WriteListTextToFile(pathFileMessage_en, true, lstMessage_en);
                            LFile.WriteListTextToFile(pathFileMessage_gl, true, lstMessage_gl);
                        }

                        ChangeLanguage();

                    }                    
                    #endregion


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
                        LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, "BeforeInitSession");
                        InitSession initSession = new InitSession(ClientInformation.ListPhongGD);
                        LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, "AfterInitSession");

                        initSession.ResizeMode = ResizeMode.NoResize;
                        initSession.Height = 250;
                        initSession.Width = 600;
                        initSession.WindowStartupLocation = WindowStartupLocation.CenterScreen;

                        initSession.lblNguoiDungValue.Content = "(" + ClientInformation.TenDangNhap + ") " + ClientInformation.HoTen;
                        initSession.lblChiNhanhValue.Content = "(" + ClientInformation.MaDonVi + ") " + ClientInformation.TenDonVi;
                        //initSession.ListPhongGD = ClientInformation.ListPhongGD;

                        LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, "BeforeInitSession Show");
                        initSession.Show();
                        LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, "AfterInitSession Show");
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
                        LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, "IF M_ResponseMessage_Login_YeuCauDoiMatKhau");
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
                        LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, "ELSE M_ResponseMessage_Login_YeuCauDoiMatKhau");
                        LMessage.ShowMessage(responseMessage, LMessage.MessageBoxType.Warning);
                    }
                }
            }
            //else
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, "Login() Exception");
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
        private void InitializeCultures()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(ClientInformation.NgonNgu);//ClientInformation.NgonNgu);
            NumberFormatInfo nfi = new NumberFormatInfo();
            DateTimeFormatInfo dfi = new DateTimeFormatInfo();
            //nfi.NumberGroupSeparator = "%";
            //dfi.DateSeparator = "-";
            Thread.CurrentThread.CurrentCulture.NumberFormat = nfi;
            Thread.CurrentThread.CurrentCulture.DateTimeFormat = dfi;
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
        }

        /// <summary>
        /// Thiết lập Culture theo ngôn ngữ và tham số hệ thống
        /// </summary>
        private void InitializeCulturesTelerik()
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(ClientInformation.NgonNgu);//ClientInformation.NgonNgu);
            //NumberFormatInfo nfi = new NumberFormatInfo();
            //DateTimeFormatInfo dfi = new DateTimeFormatInfo();
            //nfi.NumberGroupSeparator = "%";
            //dfi.DateSeparator = "-";
            //Thread.CurrentThread.CurrentCulture.NumberFormat = nfi;
            //Thread.CurrentThread.CurrentCulture.DateTimeFormat = dfi;
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
        }

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
                    this.Title = "Sign in" + " - " + ClientInformation.ShortName;
                    this.lblTitle.Content = "...Login to mFinance System...";
                    this.lblLanguage.Content = "Language";
                    this.lblUsername.Content = "Username";
                    this.lblPassword.Content = "Password";
                    this.btnLogin.Content = "Login";
                    this.btnClear.Content = "Clear";
                    this.btnCancel.Content = "Close";

                    LMessageBox.OK = "OK";
                    LMessageBox.Ignore = "Ignore";
                    LMessageBox.Cancel = "Cancel";
                    LMessageBox.No = "No";
                    LMessageBox.Retry = "Retry";
                    LMessageBox.Abort = "Abort";
                    LMessageBox.Yes = "Yes";
                    LMessageBox.Register();

                    System.Windows.Window windows = new frmThongBaoLoi("Language key [" + cboLanguage.SelectedValue.ToString().Substring(0, 2) + "] is missing or invalid", null, true);
                    windows.ShowDialog();
                }
                else
                {
                    this.Title = LLanguage.SearchResourceByKey("U.ZAMainApp.LoginWindow.Title") + " - " + ClientInformation.ShortName;
                    LMessageBox.OK = LLanguage.SearchResourceByKey("U.DungChung.OK");
                    LMessageBox.Ignore = LLanguage.SearchResourceByKey("U.DungChung.Ignore");
                    LMessageBox.Cancel = LLanguage.SearchResourceByKey("U.DungChung.Cancel");
                    LMessageBox.No = LLanguage.SearchResourceByKey("U.DungChung.No");
                    LMessageBox.Retry = LLanguage.SearchResourceByKey("U.DungChung.Retry");
                    LMessageBox.Abort = LLanguage.SearchResourceByKey("U.DungChung.Abort");
                    LMessageBox.Yes = LLanguage.SearchResourceByKey("U.DungChung.Yes");
                    LMessageBox.Register();
                }
            }
            catch (Exception ex)
            {
                new frmThongBaoLoi("M.ZAMainApp.LoginWindow.LoiDoiNgonNgu", ex).ShowDialog();
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(),LLogging.LogType.ERR,ex);
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

        /// <summary>
        /// Đọc thông tin cấu hình client
        /// </summary>
        /// <param name="type">0: IIS App, 1: WPF App</param>
        /// <returns></returns>
        public bool docThongTinCauHinhClient(int type)
        {
            try
            {

                // string filePath=@"D:\InCompany\Resources\VSS\NG-mFINA\2.SourceCode\NG.mFinance\Build\Build.Client\Dev\config\config.conf";
                //string systemPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
                string systemPath = "";
                if (type == 0)
                    systemPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
                else if (type == 1)
                    systemPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;

                string filePath = systemPath + "config\\config.conf";
                MemoryStream mStream = LSecurity.DESDecryptFile(filePath, @"!=Q|A'Z?");
                XElement xml = XElement.Load(mStream);
                DataTable dt = LDatatable.XElementToDataTable(xml);
                if (dt.Rows.Count > 0)
                {
                    ClientInformation.DataTableConfig = dt;
                    if (dt.Columns.Contains("Company")) ClientInformation.Company = dt.Rows[0]["Company"].ToString();
                    else Console.WriteLine("Presentation.Process.Common: Chua khai bao tham so Company trong file cau hinh");

                    if (dt.Columns.Contains("ClientType")) ClientInformation.ClientType = dt.Rows[0]["ClientType"].ToString();
                    else Console.WriteLine("Presentation.Process.Common: Chua khai bao tham so ClientType trong file cau hinh");

                    if (dt.Columns.Contains("WorkingDir")) ClientInformation.WorkingDir = systemPath + dt.Rows[0]["WorkingDir"].ToString();
                    else Console.WriteLine("Presentation.Process.Common: Chua khai bao tham so WorkingDir trong file cau hinh");

                    if (dt.Columns.Contains("ConfigDir")) ClientInformation.ConfigDir = systemPath + dt.Rows[0]["ConfigDir"].ToString();
                    else Console.WriteLine("Presentation.Process.Common: Chua khai bao tham so ConfigDir trong file cau hinh");

                    if (dt.Columns.Contains("DataDir")) ClientInformation.DataDir = systemPath + dt.Rows[0]["DataDir"].ToString();
                    else Console.WriteLine("Presentation.Process.Common: Chua khai bao tham so DataDir trong file cau hinh");

                    if (dt.Columns.Contains("HelpDir")) ClientInformation.HelpDir = systemPath + dt.Rows[0]["HelpDir"].ToString();
                    else Console.WriteLine("Presentation.Process.Common: Chua khai bao tham so HelpDir trong file cau hinh");

                    if (dt.Columns.Contains("ImagesDir")) ClientInformation.ImagesDir = systemPath + dt.Rows[0]["ImagesDir"].ToString();
                    else Console.WriteLine("Presentation.Process.Common: Chua khai bao tham so ImagesDir trong file cau hinh");

                    if (dt.Columns.Contains("LanguagesDir")) ClientInformation.LanguagesDir = systemPath + dt.Rows[0]["LanguagesDir"].ToString();
                    else Console.WriteLine("Presentation.Process.Common: Chua khai bao tham so LanguagesDir trong file cau hinh");

                    if (dt.Columns.Contains("TempDir")) ClientInformation.TempDir = systemPath + dt.Rows[0]["TempDir"].ToString();
                    else Console.WriteLine("Presentation.Process.Common: Chua khai bao tham so TempDir trong file cau hinh");

                    if (dt.Columns.Contains("IconName")) ClientInformation.IconName = ClientInformation.ImagesDir + "\\" + dt.Rows[0]["IconName"].ToString() + ".ico";
                    else Console.WriteLine("Presentation.Process.Common: Chua khai bao tham so IconName trong file cau hinh");

                    if (dt.Columns.Contains("ShortName")) ClientInformation.ShortName = dt.Rows[0]["ShortName"].ToString();
                    else Console.WriteLine("Presentation.Process.Common: Chua khai bao tham so ShortName trong file cau hinh");

                    if (dt.Columns.Contains("FullName")) ClientInformation.FullName = dt.Rows[0]["FullName"].ToString();
                    else Console.WriteLine("Presentation.Process.Common: Chua khai bao tham so FullName trong file cau hinh");

                    if (dt.Columns.Contains("Theme")) ClientInformation.Theme = dt.Rows[0]["Theme"].ToString();
                    else
                    {
                        ClientInformation.Theme = "default";
                        Console.WriteLine("Presentation.Process.Common: Chua khai bao tham so Theme trong file cau hinh");
                    }

                    if (dt.Columns.Contains("LanguageList")) ClientInformation.LanguageList = dt.Rows[0]["LanguageList"].ToString();
                    else
                    {
                        ClientInformation.LanguageList = "";
                        Console.WriteLine("Presentation.Process.Common: Chua khai bao tham so LanguageList trong file cau hinh");
                    }

                    if (dt.Columns.Contains("VersionDir")) ClientInformation.VersionDir = systemPath + dt.Rows[0]["VersionDir"].ToString();
                    else Console.WriteLine("Presentation.Process.Common: Chua khai bao tham so VersionDir trong file cau hinh");

                    if (dt.Columns.Contains("BackupVersionDir")) ClientInformation.BackupVersionDir = ClientInformation.VersionDir + "\\" + dt.Rows[0]["BackupVersionDir"].ToString();
                    else Console.WriteLine("Presentation.Process.Common: Chua khai bao tham so BackupVersionDir trong file cau hinh");

                    if (dt.Columns.Contains("CurrentVersionDir")) ClientInformation.CurrentVersionDir = ClientInformation.VersionDir + "\\" + dt.Rows[0]["CurrentVersionDir"].ToString();
                    else Console.WriteLine("Presentation.Process.Common: Chua khai bao tham so CurrentVersionDir trong file cau hinh");

                    if (dt.Columns.Contains("DefaultVersionDir")) ClientInformation.DefaultVersionDir = ClientInformation.VersionDir + "\\" + dt.Rows[0]["DefaultVersionDir"].ToString();
                    else Console.WriteLine("Presentation.Process.Common: Chua khai bao tham so DefaultVersionDir trong file cau hinh");

                    if (dt.Columns.Contains("OtaVersionDir")) ClientInformation.OtaVersionDir = ClientInformation.VersionDir + "\\" + dt.Rows[0]["OtaVersionDir"].ToString();
                    else Console.WriteLine("Presentation.Process.Common: Chua khai bao tham so OtaVersionDir trong file cau hinh");

                    if (dt.Columns.Contains("Log4NetConfig")) ClientInformation.Log4NetConfig = systemPath + dt.Rows[0]["Log4NetConfig"].ToString();
                    else Console.WriteLine("Presentation.Process.Common: Chua khai bao tham so Log4NetConfig trong file cau hinh");

                    if (dt.Columns.Contains("Log4NetUpdConfig")) ClientInformation.Log4NetUpdConfig = systemPath + dt.Rows[0]
["Log4NetUpdConfig"].ToString();
                    else Console.WriteLine("Presentation.Process.Common: Chua khai bao tham so Log4NetUpdConfig trong file cau hinh");

                    if (dt.Columns.Contains("Log4NetOutput")) ClientInformation.Log4NetOutput = systemPath + dt.Rows[0]["Log4NetOutput"].ToString();
                    else Console.WriteLine("Presentation.Process.Common: Chua khai bao tham so Log4NetOutput trong file cau hinh");

                    if (dt.Columns.Contains("ServerList")) ClientInformation.ServerList = dt.Rows[0]["ServerList"].ToString();
                    else Console.WriteLine("Presentation.Process.Common: Chua khai bao tham so ServerList trong file cau hinh");

                    if (dt.Columns.Contains("ServerName")) ClientInformation.ServerName = dt.Rows[0]["ServerName"].ToString();
                    else Console.WriteLine("Presentation.Process.Common: Chua khai bao tham so ServerName trong file cau hinh");

                    if (dt.Columns.Contains("ServerIP")) ClientInformation.ServerIP = dt.Rows[0]["ServerIP"].ToString();
                    else Console.WriteLine("Presentation.Process.Common: Chua khai bao tham so ServerIP trong file cau hinh");

                    if (dt.Columns.Contains("ServerPort")) ClientInformation.ServerPort = dt.Rows[0]["ServerPort"].ToString();
                    else Console.WriteLine("Presentation.Process.Common: Chua khai bao tham so ServerPort trong file cau hinh");

                    if (dt.Columns.Contains("License")) ClientInformation.License = dt.Rows[0]["License"].ToString();
                    else Console.WriteLine("Presentation.Process.Common: Chua khai bao tham so License trong file cau hinh");

                    if (dt.Columns.Contains("Version")) ClientInformation.Version = dt.Rows[0]["Version"].ToString();
                    else Console.WriteLine("Presentation.Process.Common: Chua khai bao tham so Version trong file cau hinh");

                    //ClientInformation.IpAddress = Utilities.GetIpAddress();
                    //ClientInformation.MacAddress = Utilities.GetMacAddress();

                    // log4net
                    log4net.ThreadContext.Properties["path"] = ClientInformation.Log4NetOutput;
                    log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(ClientInformation.Log4NetConfig));
                }
                else
                {
                    Console.WriteLine("Presentation.Process.Common: Doc thong tin cau hinh khong thanh cong; ");
                }

                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, "Client initialization");

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Presentation.Process.Common: Doc thong tin cau hinh khong thanh cong; " + ex.ToString());
                return false;
            }
        }

        private void SetThemes()
        {
            ResourceDictionary dict = new ResourceDictionary();
            try
            {
                string pathName = @"pack://application:,,,/PresentationWPF.CustomControl;component/Theme/" + ClientInformation.Theme + @".xaml";
                // Đổi UIResource
                dict.Source = new Uri(pathName, UriKind.RelativeOrAbsolute);
                Application.Current.Resources.MergedDictionaries.Add(dict);
            }
            catch (System.Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            finally
            {
                dict = null;
            }
        }
    }
}
