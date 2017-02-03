using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.IO;
using System.Xml;
using System.Text;
using System.Data;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Windows.Controls.Ribbon;
using PresentationWPF.CustomControl;
using Utilities.Common;
using Presentation.Process.Common;
using Presentation.Process.ZAMainAppServiceRef;
using System.Windows.Controls.Primitives;
using Presentation.Process;

namespace PresentationWPF.ZAMainApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : RibbonWindow
    {
        private string ImageSource;
        private const string _qatFileName = "mFinance_QAT.xml";

        // Có khởi tạo lại session cho người dùng không?
        private bool ResetSession = true;

        public MainWindow()
        {
            InitializeComponent();
            CommonFunction.setIcon(this);
            this.Title = ClientInformation.ShortName + " - " + ClientInformation.Version;
            this.AddHandler(CloseableTabItem.CloseTabEvent, new RoutedEventHandler(this.CloseTab));
            BindingStatusBar();
            BindingMenu();
            ResetSession = true;
        }

        /// <summary>
        /// Lấy thông tin cho thanh StatusBar
        /// </summary>
        public void BindingStatusBar()
        {
            try
            {
                lbUser.Content = ClientInformation.HoTen;
                lbBranch.Content = ClientInformation.TenDonVi + 
                    (!LString.IsNullOrEmptyOrSpace(ClientInformation.TenDonViGiaoDich) ? " | " + ClientInformation.TenDonViGiaoDich : "");
                lbWorkingDay.Content = ClientInformation.NgayLamViecHienTai != null ? LDateTime.DateToString(LDateTime.StringToDate(ClientInformation.NgayLamViecHienTai, "yyyyMMdd"), "dd/MM/yyyy") : null;
                foreach (StatusBarItem item in stbTrangThai.Items)
                {
                    item.Width = dpMain.Width / 6;
                }
            }
            catch (Exception ex)
            {
                new frmThongBaoLoi(LLanguage.SearchResourceByKey("Unknown exception. Contact administrator for support."), ex).ShowDialog();
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }
        /// <summary>
        /// Tạo sự kiện đóng Tab
        /// </summary>
        /// <param name="source"></param>
        /// <param name="args"></param>
        private void CloseTab(object source, RoutedEventArgs args)
        {
            TabItem tabItem = args.Source as TabItem;
            if (tabItem != null)
            {
                TabControl tabControl = tabItem.Parent as TabControl;
                if (tabControl != null)
                    tabControl.Items.Remove(tabItem);
            }
        }
        /// <summary>
        /// Tạo menu
        /// </summary>
        private void BindingMenu()
        {
            try
            {
                // Gọi user control RibbonMenu
                UserControlRibbonMenu ucHeThong = new UserControlRibbonMenu();
                ucHeThong.KhoiTaoMenu(ref rbMenu);
                RoutedCommand rc;

                RibbonApplicationMenu ram = rbMenu.ApplicationMenu;
                foreach (object am in ram.Items)
                {
                    if (am is RibbonApplicationSplitMenuItem)
                    {
                        RibbonApplicationSplitMenuItem rasmi = (RibbonApplicationSplitMenuItem)am;
                        rc = new RoutedCommand();
                        rasmi.Command = rc;
                        rasmi.CommandBindings.Add(new CommandBinding(rc, ExecuteCommand, CanExecuteCommand));

                        foreach (RibbonApplicationMenuItem item in rasmi.Items)
                        {
                            rc = new RoutedCommand();
                            item.Command = rc;
                            item.CommandBindings.Add(new CommandBinding(rc, ExecuteCommand, CanExecuteCommand));
                        }
                    }
                    else if (am is RibbonApplicationMenuItem)
                    {
                        RibbonApplicationMenuItem rami = (RibbonApplicationMenuItem)am;
                        rc = new RoutedCommand();
                        rami.Command = rc;
                        rami.CommandBindings.Add(new CommandBinding(rc, ExecuteCommand, CanExecuteCommand));
                        foreach (RibbonApplicationMenuItem item in rami.Items)
                        {
                            rc = new RoutedCommand();
                            item.Command = rc;
                            item.CommandBindings.Add(new CommandBinding(rc, ExecuteCommand, CanExecuteCommand));
                        }
                    }
                }
                foreach (RibbonButton rb in ((Grid)(ram.FooterPaneContent)).Children)
                {
                    rc = new RoutedCommand();
                    rb.Command = rc;
                    rb.CommandBindings.Add(new CommandBinding(rc, ExecuteCommand, CanExecuteCommand));
                }

                foreach (RibbonTab tb in rbMenu.Items)
                    foreach (RibbonGroup gr in tb.Items)
                        foreach (object btn in gr.Items)
                        {
                            if (btn is RibbonSplitButton)
                            {
                                RibbonSplitButton rsb = (RibbonSplitButton)btn;
                                rc = new RoutedCommand();
                                rsb.Command = rc;
                                rsb.CommandBindings.Add(new CommandBinding(rc, ExecuteCommand, CanExecuteCommand));
                                foreach (RibbonMenuItem item in rsb.Items)
                                {
                                    rc = new RoutedCommand();
                                    item.Command = rc;
                                    item.CommandBindings.Add(new CommandBinding(rc, ExecuteCommand, CanExecuteCommand));
                                }
                            }
                            else if (btn is RibbonMenuButton)
                            {
                                RibbonMenuButton rmb = (RibbonMenuButton)btn;
                                rc = new RoutedCommand();
                                rmb.QuickAccessToolBarId = rc;
                                rmb.CommandBindings.Add(new CommandBinding(rc, ExecuteCommand, CanExecuteCommand));
                                foreach (RibbonMenuItem item in rmb.Items)
                                {
                                    rc = new RoutedCommand();
                                    item.Command = rc;
                                    item.CommandBindings.Add(new CommandBinding(rc, ExecuteCommand, CanExecuteCommand));
                                }
                            }
                            else if (btn is RibbonButton)
                            {
                                RibbonButton rb = (RibbonButton)btn;
                                rc = new RoutedCommand();
                                rb.Command = rc;
                                rb.CommandBindings.Add(new CommandBinding(rc, ExecuteCommand, CanExecuteCommand));
                            }
                        }

                // Show / Hide ribbon button
                RibbonToggleButton rtbShowHide = new RibbonToggleButton();
                rtbShowHide.ToolTip = LLanguage.SearchResourceByKey("ACTION.TOOLTIP.Collapsethemenu"); //Language
                BitmapImage bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.UriSource = new Uri(ApplicationConstant.defaultImageSource + "Action/up.png", UriKind.RelativeOrAbsolute);
                bmp.EndInit();
                rtbShowHide.SmallImageSource = bmp;
                rtbShowHide.Click += new RoutedEventHandler(this.btnShowHideRibbon_Click);
                rtbShowHide.CanAddToQuickAccessToolBarDirectly = false;

                // Help button
                RibbonButton rbHelp = new RibbonButton();
                rbHelp.ToolTip = LLanguage.SearchResourceByKey("ACTION.TOOLTIP.Help"); //Language
                bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.UriSource = new Uri(ApplicationConstant.defaultImageSource + "Action/help_main.png", UriKind.RelativeOrAbsolute);
                bmp.EndInit();
                rbHelp.SmallImageSource = bmp;
                rbHelp.Click += new RoutedEventHandler(this.btnHelp_Click);
                rbHelp.CanAddToQuickAccessToolBarDirectly = false;

                StackPanel sp = new StackPanel();
                sp.Orientation = Orientation.Horizontal;
                sp.Children.Add(rtbShowHide);
                sp.Children.Add(rbHelp);
                rbMenu.HelpPaneContent = sp;
                rbMenu.SizeChanged += new SizeChangedEventHandler(this.rbMenuIsMinimized_Change);
            }
            catch (Exception ex)
            {
                // Ghi log Exception
                new frmThongBaoLoi(LLanguage.SearchResourceByKey("M.DungChung.LoiKhongXacDinh"), ex).ShowDialog();
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        private void RibbonWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Load QAT
            if (rbMenu.QuickAccessToolBar == null)
            {
                rbMenu.QuickAccessToolBar = new RibbonQuickAccessToolBar();
            }
            LoadQatItems();
        }

        private void RibbonWindow_Closed(object sender, EventArgs e)
        {
            //this.Cursor = Cursors.Wait;
            if (ResetSession)
            {
                var process = new ZAMainAppProcess();
                process.doLogout();
            }
            //Application.Current.Shutdown(); Không được shutdown vì nếu không sẽ đóng luôn cả màn hình đăng nhập lại
            SaveQatItems();
            LMessageBox.Unregister();
        }

        private void LoadQatItems()
        {
            QatItemCollection qatItems = new QatItemCollection();
            try
            {
                if (File.Exists(_qatFileName))
                {
                    XmlReader xmlReader = XmlReader.Create(_qatFileName);
                    qatItems = (QatItemCollection)XamlReader.Load(xmlReader);
                    xmlReader.Close();
                }

                RibbonApplicationMenu ram = rbMenu.ApplicationMenu;
                foreach (QatItem qat in qatItems)
                {
                    foreach (object am in ram.Items)
                    {
                        if (am is RibbonApplicationSplitMenuItem)
                        {
                            RibbonApplicationSplitMenuItem rasmi = (RibbonApplicationSplitMenuItem)am;
                            if (qat.ID_CNANG == rasmi.Name)
                            {
                                RibbonCommands.AddToQuickAccessToolBarCommand.Execute(null, rasmi as Control);
                                goto Next;
                            }
                            foreach (RibbonApplicationMenuItem item in rasmi.Items)
                                if (qat.ID_CNANG == item.Name)
                                {
                                    RibbonCommands.AddToQuickAccessToolBarCommand.Execute(null, item as Control);
                                    goto Next;
                                }
                        }
                        else if (am is RibbonApplicationMenuItem)
                        {
                            RibbonApplicationMenuItem rami = (RibbonApplicationMenuItem)am;
                            if (qat.ID_CNANG == rami.Name)
                            {
                                RibbonCommands.AddToQuickAccessToolBarCommand.Execute(null, rami as Control);
                                goto Next;
                            }
                            foreach (RibbonApplicationMenuItem item in rami.Items)
                                if (qat.ID_CNANG == item.Name)
                                {
                                    RibbonCommands.AddToQuickAccessToolBarCommand.Execute(null, item as Control);
                                    goto Next;
                                }
                        }
                    }
                    foreach (RibbonButton rb in ((Grid)(ram.FooterPaneContent)).Children)
                    {
                        //rb.CanAddToQuickAccessToolBarDirectly = false;
                        if (qat.ID_CNANG == rb.Name)
                        {
                            //RibbonCommands.AddToQuickAccessToolBarCommand.Execute(null, rb as Control);
                            RibbonButton why = new RibbonButton();
                            why.Name = rb.Name;
                            why.Label = rb.Label;
                            why.ToolTip = rb.ToolTip;
                            why.LargeImageSource = rb.LargeImageSource;
                            why.SmallImageSource = rb.SmallImageSource;
                            why.Tag = rb.Tag;
                            why.IsEnabled = (rb.Tag as ChucNangDto).Quyen > 0; // Luôn false???
                            why.Command = rb.Command;
                            why.CommandBindings.Add(rb.CommandBindings[0]);
                            rbMenu.QuickAccessToolBar.Items.Add(why);

                            goto Next;
                        }
                    }

                    foreach (RibbonTab tb in rbMenu.Items)
                        foreach (RibbonGroup gr in tb.Items)
                            foreach (object btn in gr.Items)
                            {
                                if (btn is RibbonSplitButton)
                                {
                                    RibbonSplitButton rsb = (RibbonSplitButton)btn;
                                    if (qat.ID_CNANG == rsb.Name)
                                    {
                                        RibbonCommands.AddToQuickAccessToolBarCommand.Execute(null, rsb as Control);
                                        goto Next;
                                    }
                                    foreach (RibbonMenuItem item in rsb.Items)
                                        if (qat.ID_CNANG == item.Name)
                                        {
                                            RibbonCommands.AddToQuickAccessToolBarCommand.Execute(null, item as Control);
                                            goto Next;
                                        }
                                }
                                else if (btn is RibbonMenuButton)
                                {
                                    RibbonMenuButton rmb = (RibbonMenuButton)btn;
                                    if (qat.ID_CNANG == rmb.Name)
                                    {
                                        RibbonCommands.AddToQuickAccessToolBarCommand.Execute(null, rmb as Control);
                                        goto Next;
                                    }
                                    foreach (RibbonMenuItem item in rmb.Items)
                                        if (qat.ID_CNANG == item.Name)
                                        {
                                            RibbonCommands.AddToQuickAccessToolBarCommand.Execute(null, item as Control);
                                            goto Next;
                                        }
                                }
                                else if (btn is RibbonButton)
                                {
                                    RibbonButton rb = (RibbonButton)btn;
                                    if (qat.ID_CNANG == rb.Name)
                                    {
                                        RibbonCommands.AddToQuickAccessToolBarCommand.Execute(null, rb as Control);
                                        goto Next;
                                    }
                                }
                            }
                Next: continue;
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                if (File.Exists(_qatFileName))
                {
                    File.Delete(_qatFileName);
                }
            }
            finally
            {
                qatItems = null;
            }
        }

        private void SaveQatItems()
        {
            try
            {
                RibbonQuickAccessToolBar rqat = rbMenu.QuickAccessToolBar;
                if (rqat != null)
                {
                    QatItemCollection qatItems = new QatItemCollection();
                    foreach (object qat in rqat.Items)
                    {
                        qatItems.Add(new QatItem((qat as FrameworkElement).Name));
                    }

                    XmlWriter xmlWriter = XmlWriter.Create(_qatFileName);
                    XamlWriter.Save(qatItems, xmlWriter);
                    xmlWriter.Close();
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void rbMenuIsMinimized_Change(object sender, SizeChangedEventArgs e)
        {
            RibbonToggleButton rtbShowHide = (rbMenu.HelpPaneContent as StackPanel).Children[0] as RibbonToggleButton;
            BitmapImage bmp = new BitmapImage();
            if (rbMenu.IsMinimized)
            {
                rtbShowHide.ToolTip = LLanguage.SearchResourceByKey("ACTION.TOOLTIP.Showthemenu"); //Language
                bmp.BeginInit();
                bmp.UriSource = new Uri(ApplicationConstant.defaultImageSource + "Action/down.png", UriKind.RelativeOrAbsolute);
                bmp.EndInit();
                rtbShowHide.SmallImageSource = bmp;
            }
            else
            {
                rtbShowHide.ToolTip = LLanguage.SearchResourceByKey("ACTION.TOOLTIP.Collapsethemenu"); //Language
                bmp.BeginInit();
                bmp.UriSource = new Uri(ApplicationConstant.defaultImageSource + "Action/up.png", UriKind.RelativeOrAbsolute);
                bmp.EndInit();
                rtbShowHide.SmallImageSource = bmp;
            }
            rtbShowHide.IsChecked = rbMenu.IsMinimized;
        }

        private void btnShowHideRibbon_Click(object sender, RoutedEventArgs e)
        {
            rbMenu.IsMinimized = !rbMenu.IsMinimized;
        }

        private void btnHelp_Click(object sender, RoutedEventArgs e)
        {
            string indexTopic = "999999";
            string helpPath = ClientInformation.WorkingDir + "help" + "\\" + "mFinance.chm";
            if (MainTab.Items.Count > 0)
            {
                // Help cho tab đang mở
                //LMessage.ShowMessageWithoutKey("Hướng dẫn sử dụng " + (MainTab.SelectedItem as CloseableTabItem).ToolTip, LMessage.MessageBoxType.Information);
                
                try
                {
                    indexTopic = (MainTab.SelectedItem as CloseableTabItem).Uid.ToString();
                }
                catch (Exception ex)
                {
                    indexTopic = "999999";
                    LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, ex);
                }
                System.Windows.Forms.Help.ShowHelp(null, helpPath, System.Windows.Forms.HelpNavigator.TopicId, indexTopic);
            }
            else
            {
                // Trang help chính của phần mềm hoặc about hoặc hướng dẫn ribbon ...
                //LMessage.ShowMessageWithoutKey("Hướng dẫn sử dụng Phần mềm Tài chính vi mô - mFinance v2.0", LMessage.MessageBoxType.Information);

                System.Windows.Forms.Help.ShowHelp(null, helpPath, System.Windows.Forms.HelpNavigator.TopicId, indexTopic);
            }
        }

        public void btnChangePassword_Click()
        {
            try
            {
                ChangePassword changePassword = new ChangePassword();
                Window window = new Window();

                window.ResizeMode = ResizeMode.NoResize;
                window.Height = 180;
                window.Width = 500;
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                window.Title = LLanguage.SearchResourceByKey("U.ZAMainApp.ChangePassword.Title");
                window.Content = changePassword;

                changePassword.userName = ClientInformation.TenDangNhap;
                window.ShowDialog();
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        public void btnSwitchOprDept_Click()
        {
            try
            {
                // Lấy lại thông tin danh sách phòng giao dịch
                QuanTriHeThongProcess qthtProcess = new QuanTriHeThongProcess();
                List<Presentation.Process.QuanTriHeThongServiceRef.DM_DON_VI> listPhongGD = new List<Presentation.Process.QuanTriHeThongServiceRef.DM_DON_VI>();
                List<Presentation.Process.ZAMainAppServiceRef.DM_DON_VI> listPhong = new List<Presentation.Process.ZAMainAppServiceRef.DM_DON_VI>();
                string responseMessage = "";
                ApplicationConstant.ResponseStatus ret = qthtProcess.LayDanhSachPhongGD(ref listPhongGD, ref responseMessage);

                if (ret == ApplicationConstant.ResponseStatus.THANH_CONG && listPhongGD != null && listPhongGD.Count > 0)
                {
                    if (listPhongGD != null)
                    {
                        foreach (Presentation.Process.QuanTriHeThongServiceRef.DM_DON_VI item in listPhongGD)
                        {
                            Presentation.Process.ZAMainAppServiceRef.DM_DON_VI dv = new Presentation.Process.ZAMainAppServiceRef.DM_DON_VI();
                            dv.ID = item.ID;
                            dv.MA_DVI = item.MA_DVI;
                            dv.TEN_GDICH = item.TEN_GDICH;
                            dv.MA_HACH_TOAN = item.MA_HACH_TOAN;
                            dv.MA_DVI_CHA = item.MA_DVI_CHA;

                            listPhong.Add(dv);
                        }
                        ClientInformation.ListPhongGD = listPhong.ToList();
                    }

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
                    ResetSession = false;
                    Close();
                }
                else
                {
                    LMessage.ShowMessage(responseMessage, LMessage.MessageBoxType.Warning);
                }
            }
            catch (Exception ex)
            {
                CommonFunction.ThongBaoLoi(ex);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        public void btnLogout_Click()
        {
            try
            {
                this.Cursor = Cursors.Wait;
                var process = new ZAMainAppProcess();
                process.doLogout();
                //ClientInformation.Release();
                LoginWindow loginWindow = new LoginWindow();
                loginWindow.Show();
                this.Cursor = Cursors.Arrow;
                Close();
                ClientInformation.Release();
            }
            catch (Exception ex)
            {
                //this.Close();
                //Application.Current.Shutdown();
                this.Cursor = Cursors.Arrow;
                /*
                // Không hiển thị thông báo lỗi trong trường hợp logout
                if (ex.GetType() == typeof(CustomException))
                    new frmThongBaoLoi(ex.Message, ex.InnerException).ShowDialog();
                else if (ex.InnerException.GetType() == typeof(CustomException))
                    new frmThongBaoLoi(ex.InnerException.Message, ex.InnerException).ShowDialog();
                else
                    new frmThongBaoLoi("M.ResponseMessage.Common.KhongThanhCong", ex).ShowDialog();
                */
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);

                // Vẫn xử lý tại client cho logout
                LoginWindow loginWindow = new LoginWindow();
                loginWindow.Show();
                this.Cursor = Cursors.Arrow;
                Close();
            }
        }

        public void btnExit_Click()
        {
            try
            {
                var process = new ZAMainAppProcess();
                process.doLogout();
                //ClientInformation.Release();
                Application.Current.Shutdown();
            }
            catch (Exception ex)
            {
                //this.Close();
                //Application.Current.Shutdown();
                this.Cursor = Cursors.Arrow;
                /*
                // Không hiển thị thông báo lỗi trong trường hợp logout
                if (ex.GetType() == typeof(CustomException))
                    new frmThongBaoLoi(ex.Message, ex.InnerException).ShowDialog();
                else if (ex.InnerException.GetType() == typeof(CustomException))
                    new frmThongBaoLoi(ex.InnerException.Message, ex.InnerException).ShowDialog();
                else
                    new frmThongBaoLoi("M.ResponseMessage.Common.KhongThanhCong", ex).ShowDialog();
                
                */
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);

                // Vẫn xử lý tại client cho logout
                Application.Current.Shutdown();
            }
        }

        /// <summary>
        /// Binding command
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExecuteCommand(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                //ZAMainAppProcess process = new ZAMainAppProcess();
                //bool ret = false;
                //ret = process.doCheckSession();
                //if (!ret)
                //    return;


                bool exist = false;
                UserControl p;
                Cursor = Cursors.Wait;
                if (!(e.OriginalSource is RibbonButton && e.Source is RibbonApplicationSplitMenuItem))
                {
                    if (e.Source is RibbonApplicationSplitMenuItem)
                    {
                        RibbonApplicationSplitMenuItem rasmi = e.Source as RibbonApplicationSplitMenuItem;
                        ChucNangDto cn = (ChucNangDto)(rasmi.Tag);
                        ClientInformation.ChucNangVuaChon = cn.TieuDe;
                        ClientInformation.FormCase = cn.FormCase;
                        RibbonApplicationMenuItem rami = null;
                        foreach (RibbonApplicationMenuItem item in rasmi.Items)
                        {
                            if ((item.Tag as ChucNangDto).ChucNang == cn.ChucNang)
                            {
                                rami = item;
                                break;
                            }
                        }
                        // Kiểm tra chức năng đã được mở chưa, nếu đã mở thì forcus
                        foreach (CloseableTabItem item in MainTab.Items)
                        {
                            if (item.Name.Equals("tab" + (rami == null ? rasmi.Name : rami.Name)))
                            {
                                exist = true;
                                MainTab.SelectedItem = item;
                                break;
                            }
                        }
                        // nếu chưa mở thì tạo tab mới
                        if (!exist)
                        {
                            if (cn.ChucNang.IsNullOrEmptyOrSpace())
                                LMessage.ShowMessage("U.ZAMainApp.MainWindow.DangPhatTrien", LMessage.MessageBoxType.Information);
                            else
                            {
                                if (cn.PhuongThuc == "Function")
                                {
                                    GetType().GetMethod(cn.ChucNang).Invoke(this, null);
                                }
                                else
                                {
                                    p = (UserControl)System.Windows.Application.LoadComponent(new Uri(cn.ChucNang, System.UriKind.RelativeOrAbsolute));
                                    p.Tag = cn;
                                    p.Uid = cn.ThuocTinh;

                                    if (cn.PhuongThuc == "Page")
                                    {
                                        CloseableTabItem tabItem = new CloseableTabItem();
                                        tabItem.Name = "tab" + (rami == null ? rasmi.Name : rami.Name);
                                        tabItem.Header = (rami == null ? rasmi.Header : rami.Header);
                                        tabItem.ToolTip = (rami == null ? rasmi.ToolTip : rami.ToolTip);
                                        tabItem.Content = p;
                                        MainTab.Items.Add(tabItem);
                                        MainTab.SelectedItem = tabItem;
                                        tabItem.Uid = cn.IDChucNang.ToString();
                                    }
                                    else
                                    {
                                        Window window = new Window
                                        {
                                            Title = (rami == null ? rasmi.Header.ToString() : rami.Header.ToString()),
                                            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen,
                                            Icon = ((rami == null ? rasmi.ImageSource : rami.ImageSource) == null ? this.Icon : (rami == null ? rasmi.ImageSource : rami.ImageSource)),
                                            Width = (Double.IsNaN(p.Width) ? 1024 : p.Width),
                                            Height = (Double.IsNaN(p.Height) ? 700 : p.Height),
                                            Content = p,
                                            Uid = cn.IDChucNang.ToString()
                                        };
                                        p.Width = Double.NaN;
                                        p.Height = Double.NaN;

                                        // Neu la HDTTK thi maximize windows
                                        if (cn.ChucNang.Equals("/PresentationWPF.TinDung;component/HoaDon/ucHDThuTienKyCT.xaml") ||
                                            cn.ChucNang.Equals("/PresentationWPF.HoaDonTienKy;component/HoaDon/ucThuGocLaiVay.xaml"))
                                        {
                                            window.WindowState = WindowState.Maximized;
                                        }

                                        // Neu la kiem tra / cap nhat phien ban thi set thong tin width, height
                                        if (cn.ChucNang.Equals("/PresentationWPF.QuanTriHeThong;component/PhienBan/ucPhienBanKiemTra.xaml") ||
                                            cn.ChucNang.Equals("/PresentationWPF.QuanTriHeThong;component/PhienBan/ucPhienBanCapNhat.xaml"))
                                        {
                                            window.Width = 500;
                                            window.Height = 150;
                                            window.ResizeMode = ResizeMode.NoResize;
                                        }

                                        window.Closing += new CancelEventHandler(MfSubWindow_Close);
                                        window.ShowDialog();
                                    }
                                }
                            }
                        }
                    }
                    else if (e.Source is RibbonApplicationMenuItem)
                    {
                        RibbonApplicationMenuItem rami = e.Source as RibbonApplicationMenuItem;
                        ChucNangDto cn = (ChucNangDto)(rami.Tag);
                        ClientInformation.ChucNangVuaChon = cn.TieuDe;
                        ClientInformation.FormCase = cn.FormCase;
                        // Kiểm tra chức năng đã được mở chưa, nếu đã mở thì forcus
                        foreach (CloseableTabItem item in MainTab.Items)
                        {
                            if (item.Name.Equals("tab" + rami.Name))
                            {
                                exist = true;
                                MainTab.SelectedItem = item;
                                break;
                            }
                        }
                        // nếu chưa mở thì tạo tab mới
                        if (!exist)
                        {
                            if (cn.ChucNang.IsNullOrEmptyOrSpace())
                                LMessage.ShowMessage("U.ZAMainApp.MainWindow.DangPhatTrien", LMessage.MessageBoxType.Information);
                            else
                            {
                                if (cn.PhuongThuc == "Function")
                                {
                                    GetType().GetMethod(cn.ChucNang).Invoke(this, null);
                                }
                                else
                                {
                                    p = (UserControl)System.Windows.Application.LoadComponent(new Uri(cn.ChucNang, System.UriKind.RelativeOrAbsolute));
                                    p.Tag = cn;
                                    p.Uid = cn.ThuocTinh;

                                    if (cn.PhuongThuc == "Page")
                                    {
                                        CloseableTabItem tabItem = new CloseableTabItem();
                                        tabItem.Name = "tab" + rami.Name;
                                        tabItem.Header = rami.Header;
                                        tabItem.ToolTip = rami.ToolTip;
                                        tabItem.Content = p;
                                        MainTab.Items.Add(tabItem);
                                        MainTab.SelectedItem = tabItem;
                                        tabItem.Uid = cn.IDChucNang.ToString();
                                    }
                                    else
                                    {
                                        Window window = new Window
                                        {
                                            Title = rami.Header.ToString(),
                                            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen,
                                            Icon = (rami.ImageSource == null ? this.Icon : rami.ImageSource),
                                            Width = (Double.IsNaN(p.Width) ? 1024 : p.Width),
                                            Height = (Double.IsNaN(p.Height) ? 700 : p.Height),
                                            Content = p,
                                            Uid = cn.IDChucNang.ToString()
                                        };
                                        p.Width = Double.NaN;
                                        p.Height = Double.NaN;

                                        // Neu la HDTTK thi maximize windows
                                        if (cn.ChucNang.Equals("/PresentationWPF.TinDung;component/HoaDon/ucHDThuTienKyCT.xaml") ||
                                            cn.ChucNang.Equals("/PresentationWPF.TinDung;component/HoaDon/BinhKhanh/ucHDThuTienKyCTBinhKhanh.xaml") ||
                                            cn.ChucNang.Equals("/PresentationWPF.HoaDonTienKy;component/HoaDon/ucThuGocLaiVay.xaml"))
                                        {
                                            window.WindowState = WindowState.Maximized;
                                        }

                                        // Neu la kiem tra / cap nhat phien ban thi set thong tin width, height
                                        if (cn.ChucNang.Equals("/PresentationWPF.QuanTriHeThong;component/PhienBan/ucPhienBanKiemTra.xaml") ||
                                            cn.ChucNang.Equals("/PresentationWPF.QuanTriHeThong;component/PhienBan/ucPhienBanCapNhat.xaml"))
                                        {
                                            window.Width = 500;
                                            window.Height = 150;
                                            window.ResizeMode = ResizeMode.NoResize;
                                        }

                                        window.Closing += new CancelEventHandler(MfSubWindow_Close);
                                        window.ShowDialog();
                                    }
                                }
                            }
                        }
                    }
                    else if (e.Source is RibbonSplitButton)
                    {
                        RibbonSplitButton rsb = e.Source as RibbonSplitButton;
                        ChucNangDto cn = (ChucNangDto)(rsb.Tag);
                        ClientInformation.ChucNangVuaChon = cn.TieuDe;
                        ClientInformation.FormCase = cn.FormCase;
                        RibbonMenuItem rmi = null;
                        RibbonApplicationMenuItem rami = null;
                        if (rsb.Items.Count > 0)
                        {
                            if (rsb.Items[0] is RibbonMenuItem)
                                foreach (RibbonMenuItem item in rsb.Items)
                                {
                                    if ((item.Tag as ChucNangDto).ChucNang == cn.ChucNang)
                                    {
                                        rmi = item;
                                        break;
                                    }
                                }
                            if (rsb.Items[0] is RibbonApplicationMenuItem)
                                foreach (RibbonApplicationMenuItem item in rsb.Items)
                                {
                                    if ((item.Tag as ChucNangDto).ChucNang == cn.ChucNang)
                                    {
                                        rami = item;
                                        break;
                                    }
                                }
                        }
                        if ((rsb.Tag as ChucNangDto).Quyen == 0) exist = true;
                        // Kiểm tra chức năng đã được mở chưa, nếu đã mở thì forcus
                        foreach (CloseableTabItem item in MainTab.Items)
                        {
                            if (item.Name.Equals("tab" + (rmi == null ? (rami == null ? rsb.Name : rami.Name) : rmi.Name)))
                            {
                                exist = true;
                                MainTab.SelectedItem = item;
                                break;
                            }
                        }
                        // nếu chưa mở thì tạo tab mới
                        if (!exist)
                        {
                            if (cn.ChucNang.IsNullOrEmptyOrSpace())
                                LMessage.ShowMessage("U.ZAMainApp.MainWindow.DangPhatTrien", LMessage.MessageBoxType.Information);
                            else
                            {
                                if (cn.PhuongThuc == "Function")
                                {
                                    GetType().GetMethod(cn.ChucNang).Invoke(this, null);
                                }
                                else
                                {
                                    p = (UserControl)System.Windows.Application.LoadComponent(new Uri(cn.ChucNang, System.UriKind.RelativeOrAbsolute));
                                    p.Tag = cn;
                                    p.Uid = cn.ThuocTinh;

                                    if (cn.PhuongThuc == "Page")
                                    {
                                        CloseableTabItem tabItem = new CloseableTabItem();
                                        tabItem.Name = "tab" + (rmi == null ? (rami == null ? rsb.Name : rami.Name) : rmi.Name);
                                        tabItem.Header = (rmi == null ? (rami == null ? rsb.Label : rami.Header) : rmi.Header);
                                        tabItem.ToolTip = (rmi == null ? (rami == null ? rsb.ToolTip : rami.ToolTip) : rmi.ToolTip);
                                        tabItem.Content = p;
                                        MainTab.Items.Add(tabItem);
                                        MainTab.SelectedItem = tabItem;
                                        tabItem.Uid = cn.IDChucNang.ToString();
                                    }
                                    else
                                    {
                                        Window window = new Window
                                        {
                                            Title = (rmi == null ? (rami == null ? rsb.Label.ToString() : rami.Header.ToString()) : rmi.Header.ToString()),
                                            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen,
                                            Icon = ((rmi == null ? (rami == null ? rsb.SmallImageSource : rami.ImageSource) : rmi.ImageSource) == null ? this.Icon : (rmi == null ? (rami == null ? rsb.SmallImageSource : rami.ImageSource) : rmi.ImageSource)),
                                            Width = (Double.IsNaN(p.Width) ? 1024 : p.Width),
                                            Height = (Double.IsNaN(p.Height) ? 700 : p.Height),
                                            Content = p,
                                            Uid = cn.IDChucNang.ToString()
                                        };
                                        p.Width = Double.NaN;
                                        p.Height = Double.NaN;

                                        // Neu la HDTTK thi maximize windows
                                        if (cn.ChucNang.Equals("/PresentationWPF.TinDung;component/HoaDon/ucHDThuTienKyCT.xaml"))
                                        {
                                            window.WindowState = WindowState.Maximized;
                                        }

                                        // Neu la kiem tra / cap nhat phien ban thi set thong tin width, height
                                        if (cn.ChucNang.Equals("/PresentationWPF.QuanTriHeThong;component/PhienBan/ucPhienBanKiemTra.xaml") ||
                                            cn.ChucNang.Equals("/PresentationWPF.TinDung;component/HoaDon/BinhKhanh/ucHDThuTienKyCTBinhKhanh.xaml") ||
                                            cn.ChucNang.Equals("/PresentationWPF.HoaDonTienKy;component/HoaDon/ucThuGocLaiVay.xaml"))
                                        {
                                            window.Width = 500;
                                            window.Height = 150;
                                            window.ResizeMode = ResizeMode.NoResize;
                                        }

                                        window.Closing += new CancelEventHandler(MfSubWindow_Close);
                                        window.ShowDialog();
                                    }
                                }
                            }
                        }
                    }
                    else if (e.Source is RibbonMenuItem)
                    {
                        RibbonMenuItem rmb = e.Source as RibbonMenuItem;
                        ChucNangDto cn = (ChucNangDto)(rmb.Tag);
                        ClientInformation.ChucNangVuaChon = cn.TieuDe;
                        ClientInformation.FormCase = cn.FormCase;
                        // Kiểm tra chức năng đã được mở chưa, nếu đã mở thì forcus
                        foreach (CloseableTabItem item in MainTab.Items)
                        {
                            if (item.Name.Equals("tab" + rmb.Name))
                            {
                                exist = true;
                                MainTab.SelectedItem = item;
                                break;
                            }
                        }
                        // nếu chưa mở thì tạo tab mới
                        if (!exist)
                        {
                            if (cn.ChucNang.IsNullOrEmptyOrSpace())
                                LMessage.ShowMessage("U.ZAMainApp.MainWindow.DangPhatTrien", LMessage.MessageBoxType.Information);
                            else
                            {
                                if (cn.PhuongThuc == "Function")
                                {
                                    GetType().GetMethod(cn.ChucNang).Invoke(this, null);
                                }
                                else
                                {
                                    p = (UserControl)System.Windows.Application.LoadComponent(new Uri(cn.ChucNang, System.UriKind.RelativeOrAbsolute));
                                    p.Tag = cn;
                                    p.Uid = cn.ThuocTinh;

                                    if (cn.PhuongThuc == "Page")
                                    {
                                        CloseableTabItem tabItem = new CloseableTabItem();
                                        tabItem.Name = "tab" + rmb.Name;
                                        tabItem.Header = rmb.Header;
                                        tabItem.ToolTip = rmb.ToolTip;
                                        tabItem.Content = p;
                                        MainTab.Items.Add(tabItem);
                                        MainTab.SelectedItem = tabItem;
                                        tabItem.Uid = cn.IDChucNang.ToString();
                                    }
                                    else
                                    {
                                        Window window = new Window
                                        {
                                            Title = rmb.Header.ToString(),
                                            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen,
                                            Icon = (rmb.ImageSource == null ? this.Icon : rmb.ImageSource),
                                            Width = (Double.IsNaN(p.Width) ? 1024 : p.Width),
                                            Height = (Double.IsNaN(p.Height) ? 700 : p.Height),
                                            Content = p,
                                            Uid = cn.IDChucNang.ToString()
                                        };
                                        p.Width = Double.NaN;
                                        p.Height = Double.NaN;

                                        // Neu la HDTTK thi maximize windows
                                        if (cn.ChucNang.Equals("/PresentationWPF.TinDung;component/HoaDon/ucHDThuTienKyCT.xaml") ||
                                            cn.ChucNang.Equals("/PresentationWPF.TinDung;component/HoaDon/BinhKhanh/ucHDThuTienKyCTBinhKhanh.xaml") ||
                                            cn.ChucNang.Equals("/PresentationWPF.HoaDonTienKy;component/HoaDon/ucThuGocLaiVay.xaml"))
                                        {
                                            window.WindowState = WindowState.Maximized;
                                        }

                                        // Neu la kiem tra / cap nhat phien ban thi set thong tin width, height
                                        if (cn.ChucNang.Equals("/PresentationWPF.QuanTriHeThong;component/PhienBan/ucPhienBanKiemTra.xaml") ||
                                            cn.ChucNang.Equals("/PresentationWPF.QuanTriHeThong;component/PhienBan/ucPhienBanCapNhat.xaml"))
                                        {
                                            window.Width = 500;
                                            window.Height = 150;
                                            window.ResizeMode = ResizeMode.NoResize;
                                        }

                                        window.Closing += new CancelEventHandler(MfSubWindow_Close);
                                        window.ShowDialog();
                                    }
                                }
                            }
                        }
                    }
                    else if (e.Source is RibbonButton)
                    {
                        RibbonButton btn = e.Source as RibbonButton;
                        ChucNangDto cn = (ChucNangDto)(btn.Tag);
                        ClientInformation.ChucNangVuaChon = cn.TieuDe;
                        ClientInformation.FormCase = cn.FormCase;
                        // Kiểm tra chức năng đã được mở chưa, nếu đã mở thì forcus
                        foreach (CloseableTabItem item in MainTab.Items)
                        {
                            if (item.Name.Equals("tab" + btn.Name))
                            {
                                exist = true;
                                MainTab.SelectedItem = item;
                                break;
                            }
                        }
                        // nếu chưa mở thì tạo tab mới
                        if (!exist)
                        {
                            if (cn.ChucNang.IsNullOrEmptyOrSpace())
                                LMessage.ShowMessage("U.ZAMainApp.MainWindow.DangPhatTrien", LMessage.MessageBoxType.Information);
                            else
                            {
                                if (cn.PhuongThuc == "Function")
                                {
                                    GetType().GetMethod(cn.ChucNang).Invoke(this, null);
                                }
                                else
                                {
                                    p = (UserControl)System.Windows.Application.LoadComponent(new Uri(cn.ChucNang, System.UriKind.RelativeOrAbsolute));
                                    p.Tag = cn;
                                    p.Uid = cn.ThuocTinh;

                                    if (cn.PhuongThuc == "Page")
                                    {
                                        CloseableTabItem tabItem = new CloseableTabItem();
                                        tabItem.Name = "tab" + btn.Name;
                                        tabItem.Header = btn.Label;
                                        tabItem.ToolTip = btn.ToolTip;
                                        tabItem.Content = p;
                                        MainTab.Items.Add(tabItem);
                                        MainTab.SelectedItem = tabItem;
                                        tabItem.Uid = cn.IDChucNang.ToString();
                                    }
                                    else
                                    {
                                        Window window = new Window
                                        {
                                            Title = btn.Label.ToString(),
                                            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen,
                                            Icon = (btn.SmallImageSource == null ? this.Icon : btn.SmallImageSource),
                                            Width = (Double.IsNaN(p.Width) ? 1024 : p.Width),
                                            Height = (Double.IsNaN(p.Height) ? 700 : p.Height),
                                            Content = p,
                                            Uid = cn.IDChucNang.ToString()
                                        };
                                        p.Width = Double.NaN;
                                        p.Height = Double.NaN;

                                        // Neu la HDTTK thi maximize windows
                                        if (cn.ChucNang.Equals("/PresentationWPF.TinDung;component/HoaDon/ucHDThuTienKyCT.xaml") ||
                                            cn.ChucNang.Equals("/PresentationWPF.TinDung;component/HoaDon/BinhKhanh/ucHDThuTienKyCTBinhKhanh.xaml") ||
                                            cn.ChucNang.Equals("/PresentationWPF.HoaDonTienKy;component/HoaDon/ucThuGocLaiVay.xaml"))
                                        {
                                            window.WindowState = WindowState.Maximized;
                                        }

                                        // Neu la kiem tra / cap nhat phien ban thi set thong tin width, height
                                        if (cn.ChucNang.Equals("/PresentationWPF.QuanTriHeThong;component/PhienBan/ucPhienBanKiemTra.xaml") ||
                                            cn.ChucNang.Equals("/PresentationWPF.QuanTriHeThong;component/PhienBan/ucPhienBanCapNhat.xaml"))
                                        {
                                            window.Width = 500;
                                            window.Height = 150;
                                            window.ResizeMode = ResizeMode.NoResize;
                                        }

                                        window.Closing += new CancelEventHandler(MfSubWindow_Close);
                                        window.ShowDialog();                                        
                                    }
                                }
                            }
                        }
                    }
                }
                e.Handled = true;
                Cursor = Cursors.Arrow;
            }
            catch (Exception ex)
            {
                // Ghi log Exception
                Cursor = Cursors.Arrow;
                if (ex.GetType() == typeof(CustomException))
                    new frmThongBaoLoi(ex.Message, ex.InnerException).ShowDialog();
                else if (ex.InnerException != null && ex.InnerException.GetType() == typeof(CustomException))
                    new frmThongBaoLoi(ex.InnerException.Message, ex.InnerException).ShowDialog();
                else
                    new frmThongBaoLoi("M.ResponseMessage.Common.KhongThanhCong", ex).ShowDialog();
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        public void MfSubWindow_Close(object sender, CancelEventArgs e)
        {
            string askBeforeClose = "0";
            try
            {
                UserControl uc = (UserControl)((Window)sender).Content;
                string thuocTinh = uc.Uid;
                askBeforeClose = thuocTinh.SplitByDelimiter("#")[4];
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, "CloseUserControl: " + ex);
                askBeforeClose = "0";
            }

            if (askBeforeClose.Equals("1"))
            {
                MessageBoxResult retMessage = LMessage.ShowMessage("M.DungChung.HoiDongChucNang", LMessage.MessageBoxType.Question);
                if (retMessage != MessageBoxResult.Yes)
                {
                    e.Cancel = true;
                }
            }
            else
            {

            }
        }

        private void CanExecuteCommand(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
            e.Handled = true;
        }

        private void Image_MouseUp(object sender, MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.ngvgroup.vn");
        }
    }

    

    /// <summary>
    /// QuickAccessToolbar Item
    /// </summary>
    public class QatItem
    {
        public QatItem()
        {
        }

        public QatItem(string ID)
        {
            ID_CNANG = ID;
        }

        public string ID_CNANG { get; set; }
    }
    public class QatItemCollection : Collection<QatItem>
    {
    }
}
