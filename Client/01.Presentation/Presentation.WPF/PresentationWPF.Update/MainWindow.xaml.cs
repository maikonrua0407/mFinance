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
using System.Diagnostics;
using System.ComponentModel;
using System.Threading;
using System.IO;
using log4net;

namespace PresentationWPF.Update
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly BackgroundWorker Worker_ChangeUpdate = new BackgroundWorker();
        private readonly BackgroundWorker Worker_FullUpdate = new BackgroundWorker();
        private string WorkingDir = "";
        private string Log4NetOutput = "";
        private string OtaVersionDir = "";
        private string Log4NetConfig = "";
        private string CurrentVersion = "";
        private string LastestVersion = "";
        private string Key = "";
        private string Flag = "";
        private string Type = "";
        private string Path = "";

        private string ResContent = "";
        private bool ResStatus = false;

        public MainWindow()
        {
            InitializeComponent();

            lblTitle.Content = "Phần mềm Tài chính vi mô - mFinance";
            string capNhat = "Cập nhật phiên bản mới...";
            string loiThamSo = "Lỗi khởi tạo tham số cập nhật phiên bản";
            string loiThamSoChung = "Lỗi khởi tạo tham số cập nhật phiên bản [NULL & COUNT]";
            string loiThamSoKey = "Lỗi khởi tạo tham số cập nhật phiên bản [KEY]";
            string loiThamSoFlag = "Lỗi khởi tạo tham số cập nhật phiên bản [FLAG (UPDATE)]";
            string loiThamSoType = "Lỗi khởi tạo tham số cập nhật phiên bản [TYPE (FULL|CHANGE)]";

            // Truongnx (0: workingdir; 1: log4netoutputdir; 2: otaversiondir; 3: logconfig; 4: currentversion; 5: lasterVersion; 6: key; 7: flag; 8: type; 9: path; ...)
            try
            {
                String[] args = App.mArgs;
                List<string> listParameter = args != null && args.Length > 0 ? args[0].Replace('*', ' ').Split('@').ToList() : null;

                //string args = "D:\\InCompany\\Resources\\VSS\\NG-mFINA\\2.SourceCode\\NG.mFinance\\Build\\Build.Client\\Dev@" +
                //    "D:\\InCompany\\Resources\\VSS\\NG-mFINA\\2.SourceCode\\NG.mFinance\\Build\\Build.Client\\Dev\\version\\backup@" +
                //    "D:\\InCompany\\Resources\\VSS\\NG-mFINA\\2.SourceCode\\NG.mFinance\\Build\\Build.Client\\Dev\\version\\ota@" +
                //    "C1.0.0004@" +
                //    "mfclient.update.ngvgroup.vn@" +
                //    "UPDATE";            
                //List<string> listParameter = args.Split('@').ToList();

                if (listParameter == null || (listParameter != null && listParameter.Count != 10))
                {
                    lblContent.Content = loiThamSoChung;
                    prbProcess.Visibility = Visibility.Collapsed;
                    btnDownload.Visibility = Visibility.Collapsed;
                    btnUpdate.Visibility = Visibility.Collapsed;
                    btnLogin.Visibility = Visibility.Visible;
                    btnExit.Visibility = Visibility.Visible;
                }
                else
                {
                    string workingDir = listParameter[0];
                    string log4NetOutputDir = listParameter[1];
                    string otaVersionDir = listParameter[2];
                    string log4NetConfig = listParameter[3];
                    string currentVersion = listParameter[4];
                    string lastestVersion = listParameter[5];
                    string key = listParameter[6];
                    string flag = listParameter[7];
                    string type = listParameter[8];
                    string path = listParameter[9];

                    if (!key.Equals("mfclient.update.ngvgroup.vn"))
                    {
                        lblContent.Content = loiThamSoKey;
                        prbProcess.Visibility = Visibility.Collapsed;
                        btnDownload.Visibility = Visibility.Collapsed;
                        btnUpdate.Visibility = Visibility.Collapsed;
                        btnLogin.Visibility = Visibility.Visible;
                        btnExit.Visibility = Visibility.Visible;
                    }
                    else if (flag.Equals("UPDATE"))
                    {
                        lblContent.Content = capNhat;
                        prbProcess.Visibility = Visibility.Visible;
                        btnDownload.Visibility = Visibility.Collapsed;
                        btnUpdate.Visibility = Visibility.Collapsed;
                        btnLogin.Visibility = Visibility.Collapsed;
                        btnExit.Visibility = Visibility.Collapsed;

                        WorkingDir = workingDir;
                        Log4NetOutput = log4NetOutputDir;
                        OtaVersionDir = otaVersionDir;
                        Log4NetConfig = log4NetConfig;
                        CurrentVersion = currentVersion;
                        LastestVersion = lastestVersion;
                        Key = key;
                        Flag = flag;
                        Type = type;
                        Path = path;

                        // log4net
                        log4net.ThreadContext.Properties["path"] = log4NetOutputDir;
                        log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(log4NetConfig));

                        WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LogType.UPD, "=========================");
                        WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LogType.UPD, "Khởi tạo cập nhật phiên bản: " + CurrentVersion + " => " + LastestVersion);
                        WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LogType.UPD, "Flag: " + Flag);
                        WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LogType.UPD, "Type: " + Type);
                        // backup
                        BackupJob();

                        // update
                        if (type.Equals("CHANGE"))
                            UpdateChangeJob();
                        else if (type.Equals("FULL"))
                            UpdateFullJob();
                        else
                        {
                            WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LogType.UPD, "Lỗi tham số cập nhật: [TYPE (FULL|CHANGE)]");
                            lblContent.Content = loiThamSoType;
                            prbProcess.Visibility = Visibility.Collapsed;
                            btnDownload.Visibility = Visibility.Collapsed;
                            btnUpdate.Visibility = Visibility.Collapsed;
                            btnLogin.Visibility = Visibility.Visible;
                            btnExit.Visibility = Visibility.Visible;
                        }

                        //finish
                        FinishJob();
                    }
                    else
                    {
                        lblContent.Content = loiThamSoFlag;
                        prbProcess.Visibility = Visibility.Collapsed;
                        btnDownload.Visibility = Visibility.Collapsed;
                        btnUpdate.Visibility = Visibility.Collapsed;
                        btnLogin.Visibility = Visibility.Visible;
                        btnExit.Visibility = Visibility.Visible;
                    }
                }   
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                lblContent.Content = loiThamSo;
                prbProcess.Visibility = Visibility.Collapsed;
                btnDownload.Visibility = Visibility.Collapsed;
                btnUpdate.Visibility = Visibility.Collapsed;
                btnLogin.Visibility = Visibility.Visible;
                btnExit.Visibility = Visibility.Visible;
            }
        }

        private void BackupJob()
        {
            // truongnx
        }

        private void UpdateChangeJob()
        {
            // truongnx
            Worker_ChangeUpdate.RunWorkerAsync();
            Worker_ChangeUpdate.DoWork += Worker_DoWork_ChangeUpdate;
            Worker_ChangeUpdate.RunWorkerCompleted += Worker_RunWorkerCompleted_ChangeUpdate;
        }

        private void UpdateFullJob()
        {
            // truongnx
            Worker_FullUpdate.RunWorkerAsync();
            Worker_FullUpdate.DoWork += Worker_DoWork_FullUpdate;
            Worker_FullUpdate.RunWorkerCompleted += Worker_RunWorkerCompleted_FullUpdate;         
        }

        private void FinishJob()
        {
            // truongnx
        }

        private void Worker_DoWork_ChangeUpdate(object sender, DoWorkEventArgs e)
        {
            WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LogType.UPD, "Bắt đầu tiến hành cập nhật [CHANGED_UPDATE (DirectoryCopy)]");
            string lastestVersionDir = OtaVersionDir + "\\" + LastestVersion;
            Console.WriteLine(OtaVersionDir);
            Console.WriteLine(LastestVersion);

            bool kq = DirectoryCopy(lastestVersionDir, WorkingDir, true);
            if (kq)
            {
                WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LogType.UPD, "Cập nhật thành công");
                WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LogType.UPD, "=========================");
                this.ResContent = "Cập nhật phiên bản thành công";
            }
            else
            {
                WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LogType.UPD, "Cập nhật không thành công");
                WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LogType.UPD, "=========================");
                this.ResContent = "Cập nhật phiên bản không thành công";
            }
            Thread.Sleep(3000);
        }

        private void Worker_RunWorkerCompleted_ChangeUpdate(object sender, RunWorkerCompletedEventArgs e)
        {
            // complete
            lblContent.Content = this.ResContent;
            prbProcess.Visibility = Visibility.Collapsed;
            btnDownload.Visibility = Visibility.Collapsed;
            btnUpdate.Visibility = Visibility.Collapsed;
            btnLogin.Visibility = Visibility.Visible;
            btnExit.Visibility = Visibility.Visible;
        }

        private void Worker_DoWork_FullUpdate(object sender, DoWorkEventArgs e)
        {
            // truongnx
            WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LogType.UPD, "Bắt đầu tiến hành cập nhật [FULL_UPDATE (DirectoryCleaning & Installation)]");
            bool cleaningResult = DirectoryCleaning(WorkingDir);
            //cleaningResult = false;
            if (cleaningResult)
            {
                WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LogType.UPD, "Chuyển sang file cài đặt chương trình");
                WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LogType.UPD, "=========================");
                Process oldProcess = Process.GetCurrentProcess();
                int oldPid = Process.GetCurrentProcess().Id;
                string pathFile = Path;
                Thread.Sleep(3000);
                System.Diagnostics.Process.Start(pathFile);
                oldProcess.Kill();
            }
            else
            {
                WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LogType.UPD, "Lỗi thao tác với thư mục làm việc hệ thống");
                WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LogType.UPD, "Chuyển sang file cài đặt chương trình");
                WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LogType.UPD, "=========================");
                Process oldProcess = Process.GetCurrentProcess();
                int oldPid = Process.GetCurrentProcess().Id;
                string pathFile = Path;
                Thread.Sleep(3000);
                System.Diagnostics.Process.Start(pathFile);
                oldProcess.Kill();
                /*
                WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LogType.UPD, "Cập nhật không thành công");
                WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LogType.UPD, "=========================");
                this.ResContent = "Lỗi thao tác với thư mục làm việc hệ thống";
                lblContent.Content = ResContent;
                prbProcess.Visibility = Visibility.Collapsed;
                btnDownload.Visibility = Visibility.Collapsed;
                btnUpdate.Visibility = Visibility.Collapsed;
                btnLogin.Visibility = Visibility.Visible;
                btnExit.Visibility = Visibility.Visible;
                 */ 
            }
        }

        private void Worker_RunWorkerCompleted_FullUpdate(object sender, RunWorkerCompletedEventArgs e)
        {
            // complete
            lblContent.Content = this.ResContent;
            prbProcess.Visibility = Visibility.Collapsed;
            btnDownload.Visibility = Visibility.Collapsed;
            btnUpdate.Visibility = Visibility.Collapsed;
            btnLogin.Visibility = Visibility.Visible;
            btnExit.Visibility = Visibility.Visible;
        }

        public static ILog upd = LogManager.GetLogger("UpdLogger");
        public enum LogType
        {
            UPD
        }
        public static void WriteLog(string message, LogType type, string ex)
        {
            //log4net.ThreadContext.Properties["LogType"] = type.ToString();
            //log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "\\config\\log.conf"));
            //log4net.Config.XmlConfigurator.Configure();
            log4net.ILog log = log4net.LogManager.GetLogger(message);
            if (type.ToString().Equals("UPD"))
            {
                log = upd;
            }            
            else
            {
                log = upd;
            }

            //log4net.ILog log = log4net.LogManager.GetLogger(message);
            string exOuput = ex;

            if (log.IsDebugEnabled)
            {
                log.Debug(exOuput);
            }
            else if (log.IsInfoEnabled)
            {
                log.Info(exOuput);
            }
            else if (log.IsWarnEnabled)
            {
                log.Warn(exOuput);
            }
            else if (log.IsErrorEnabled)
            {
                log.Error(exOuput);
            }
            else if (log.IsFatalEnabled)
            {
                log.Fatal(exOuput);
            }
            else
            {

                log.Debug(exOuput);
            }
        }

        public static bool DirectoryCleaning(string workingDir)
        {
            bool result = true;
            try
            {
                // Xóa toàn bộ thư mục workingDir
                // Trừ: logs, temp, updater, version
                // truongnx hard-code here

                // Xóa config
                // Xóa data
                // Xóa images
                // Xóa languages
                // Xóa các file trong workingDir

                DirectoryInfo dir = new DirectoryInfo(workingDir);
                DirectoryInfo[] dirs = dir.GetDirectories();
                FileInfo[] files = dir.GetFiles();

                WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LogType.UPD, "DirectoryCleaning: " + workingDir);

                if (dirs.Length == 0 && files.Length == 0)
                {
                    Directory.Delete(workingDir);
                    result = true;
                }

                foreach (DirectoryInfo subdir in dirs)
                {
                    if (!subdir.Name.Equals("logs") &&
                        !subdir.Name.Equals("temp") &&
                        !subdir.Name.Equals("updater") &&
                        !subdir.Name.Equals("version"))
                    {
                        WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LogType.UPD, "SubDirectoryCleaning: " + subdir.Name);
                        string temppath = System.IO.Path.Combine(workingDir, subdir.Name);
                        File.SetAttributes(temppath, FileAttributes.Normal);
                        DirectoryCleaning(temppath);
                    }
                }

                foreach (FileInfo file in files)
                {
                    WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LogType.UPD, "FileCleaning: " + file.Name);
                    string temppath = System.IO.Path.Combine(workingDir, file.Name);
                    File.SetAttributes(temppath, FileAttributes.Normal);
                    File.Delete(temppath);
                }

                result = true;
                return result;
            }
            catch (System.Exception ex)
            {
                result = false;
                WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LogType.UPD, "DirectoryCleaning result: " + ex.ToString());
            }
            return result;
        }

        public static bool DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LogType.UPD, "SourceDirName: " + sourceDirName);
            WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LogType.UPD, "DestDirName: " + destDirName);
            bool result = true;
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);
            DirectoryInfo[] dirs = dir.GetDirectories();

            try
            {
                if (!dir.Exists)
                {
                    result = false;
                    Console.WriteLine("!dir.Exists");
                    WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LogType.UPD, "Source directory does not exist or could not be found: " + sourceDirName);
                    throw new DirectoryNotFoundException(
                        "Source directory does not exist or could not be found: "
                        + sourceDirName);
                }

                if (!Directory.Exists(destDirName))
                {
                    Directory.CreateDirectory(destDirName);
                }

                FileInfo[] files = dir.GetFiles();
                foreach (FileInfo file in files)
                {
                    string temppath = System.IO.Path.Combine(destDirName, file.Name);
                    WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LogType.UPD, "Copy file: " + file.Name);
                    try
                    {
                        FileAttributes fileAtt = File.GetAttributes(temppath);
                        if (fileAtt != FileAttributes.Normal)
                            File.SetAttributes(temppath, FileAttributes.Normal);
                        FileInfo fileInfo = file.CopyTo(temppath, true);
                        File.SetAttributes(temppath, fileAtt);
                        WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LogType.UPD, "Copy file result: " + fileInfo.ToString());
                    }
                    catch (System.Exception ex)
                    {
                        WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LogType.UPD, "Copy file result failed: " + ex);
                        return false;
                    }                   
                    
                }

                if (copySubDirs)
                {
                    foreach (DirectoryInfo subdir in dirs)
                    {
                        string temppath = System.IO.Path.Combine(destDirName, subdir.Name);
                        WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LogType.UPD, "Copy dir: " + subdir.Name);
                        bool retDirCopy = DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                        WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LogType.UPD, "Copy dir result: " + retDirCopy.ToString());
                        if (!retDirCopy)
                            return false;
                    }
                }
            }
            catch (System.Exception ex)
            {
                result = false;
                WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LogType.UPD, "DirectoryCopy result: " + ex.ToString());
            }
            return result;
        }

        private void btnDownload_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("ex: " + ex.ToString());
            }

        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("ex: " + ex.ToString());
            }

        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process oldProcess = Process.GetCurrentProcess();
                int oldPid = Process.GetCurrentProcess().Id;
                string pathFile = WorkingDir + "PresentationWPF.ZAMainApp.exe";
                System.Diagnostics.Process.Start(pathFile);
                oldProcess.Kill();
            }
            catch (Exception ex)
            {
                Console.WriteLine("ex: " + ex.ToString());
            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Application.Current.Shutdown();
            }
            catch (Exception ex)
            {
                Console.WriteLine("ex: " + ex.ToString());
            }
        }
    }
}
