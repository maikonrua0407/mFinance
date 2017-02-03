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
using System.Windows.Media.Animation;
using System.ComponentModel;
using System.Threading;
using Presentation.Process;
using Presentation.Process.Common;
using Utilities.Common;
using PresentationWPF.Update;
using System.Diagnostics;
using Presentation.Process.QuanTriHeThongServiceRef;
using System.IO;

namespace PresentationWPF.QuanTriHeThong.PhienBan
{
    /// <summary>
    /// Interaction logic for ucPhienBanCapNhat.xaml
    /// </summary>
    /// 
    public partial class ucPhienBanCapNhat : UserControl
    {
        private readonly BackgroundWorker Worker_KiemTra = new BackgroundWorker();
        private readonly BackgroundWorker Worker_Download = new BackgroundWorker();
        private string updateApp = "PresentationWPF.Update.exe";

        private bool IsLastestVersion = true;
        private string ServerVersion = "";
        private string LastestClientVersion = "";
        private string ResContent = "";
        private ApplicationConstant.ResponseStatus ResStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;

        private HT_PBAN HtPban;
        private List<HT_PBAN_CTIET> ListHtPbanCtiet;
        private List<HT_PBAN_FILE> ListHtPbanFile;

        private string PathFile = "";

        public ucPhienBanCapNhat()
        {
            InitializeComponent();
            lblTitle.Content += " - " + ClientInformation.ShortName;
            btnDownload.Visibility = Visibility.Collapsed;
            btnUpdate.Visibility = Visibility.Collapsed;
            Worker_KiemTra.DoWork += Worker_DoWork_KiemTra;
            Worker_KiemTra.RunWorkerCompleted += Worker_RunWorkerCompleted_KiemTra;
            //Process();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //Duration duration = new Duration(TimeSpan.FromSeconds(20));
            //DoubleAnimation doubleanimation = new DoubleAnimation(200.0, duration);
            //prbProcess.BeginAnimation(ProgressBar.ValueProperty, doubleanimation);
            Worker_KiemTra.RunWorkerAsync();
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            CustomControl.CommonFunction.CloseUserControl(this);
        }

        private void Worker_DoWork_KiemTra(object sender, DoWorkEventArgs e)
        {
            try
            {
                // background
                QuanTriHeThongProcess process = new QuanTriHeThongProcess();
                ApplicationConstant.ResponseStatus ret = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                string serverVersion = "";
                string lastestClientVersion = "";
                HT_PBAN htPban = new HT_PBAN();
                List<HT_PBAN_CTIET> listHtPbanCtiet = new List<HT_PBAN_CTIET>();
                List<HT_PBAN_FILE> listHtPbanFile = new List<HT_PBAN_FILE>();
                string responseMessage = "";
                ret = process.CheckClientVersion(ClientInformation.Version, ref serverVersion, ref lastestClientVersion, ref htPban, ref listHtPbanCtiet, ref listHtPbanFile, ref responseMessage);
                ResStatus = ret;
                if (ret == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    ServerVersion = serverVersion;
                    LastestClientVersion = lastestClientVersion;
                    if (ClientInformation.Version.Equals(LastestClientVersion))
                    {
                        IsLastestVersion = true;
                        ResContent = LLanguage.SearchResourceByKey("M.ResponseMessage.QuanTriHeThong.PhienBan.SystemUpToDate") + " " + ClientInformation.Version;
                    }
                    else
                    {
                        IsLastestVersion = false;
                        HtPban = htPban;
                        ListHtPbanCtiet = listHtPbanCtiet;
                        ListHtPbanFile = listHtPbanFile;
                        ResContent = LLanguage.SearchResourceByKey("M.ResponseMessage.QuanTriHeThong.PhienBan.PhienBanHienTai") +
                            " " + ClientInformation.Version + ". " +
                            LLanguage.SearchResourceByKey("M.ResponseMessage.QuanTriHeThong.PhienBan.PhienBanMoiNhat") +
                            " " + LastestClientVersion;
                    }
                }
                else if (ret == ApplicationConstant.ResponseStatus.KHONG_THANH_CONG)
                {
                    ResContent = LLanguage.SearchResourceByKey(responseMessage);
                }
                else
                {
                    ResContent = "";
                }

                Thread.Sleep(3000);
            }
            catch (Exception ex)
            {
                ResStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                ResContent = LLanguage.SearchResourceByKey("M.ResponseMessage.QuanTriHeThong.PhienBan.KiemTraKhongThanhCong");
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.SYS, ex);
                Worker_KiemTra.CancelAsync();
                // Vẫn xử lý tại client cho logout
                //Application.Current.Shutdown();
            }
        }

        private void Worker_RunWorkerCompleted_KiemTra(object sender,
                                               RunWorkerCompletedEventArgs e)
        {
            // complete
            lblContent.Content = ResContent;
            if (ResStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                prbProcess.Visibility = Visibility.Collapsed;
                btnDownload.Visibility = IsLastestVersion ? Visibility.Collapsed : Visibility.Visible;
                btnUpdate.Visibility = Visibility.Collapsed;
            }
            else if (ResStatus == ApplicationConstant.ResponseStatus.KHONG_THANH_CONG || e.Cancelled == true)
            {
                prbProcess.Visibility = Visibility.Collapsed;
                btnDownload.Visibility = Visibility.Collapsed;
                btnUpdate.Visibility = Visibility.Collapsed;
            }
            else
            {
                prbProcess.Visibility = Visibility.Collapsed;
                btnDownload.Visibility = Visibility.Collapsed;
                btnUpdate.Visibility = Visibility.Collapsed;
            }
        }

        private void Worker_DoWork_Download(object sender, DoWorkEventArgs e)
        {
            try
            {
                // background
                QuanTriHeThongProcess process = new QuanTriHeThongProcess();
                ApplicationConstant.ResponseStatus ret = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                FileBase fileResponse = new FileBase();
                string responseMessage = "";
                PhienBanDTO phienBanDTO = new PhienBanDTO();
                ret = process.DownloadClientVersion(ClientInformation.Version, LastestClientVersion, HtPban, ref phienBanDTO, ref responseMessage);
                ResStatus = ret;
                if (ret == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    string phuongThucCapNhat = HtPban.PTHUC_CNHAT;
                    string maPhienBanTruoc = HtPban.MA_PBAN_TRUOC;
                    // Nếu cập nhật toàn bộ ứng dụng
                    if (phuongThucCapNhat.Equals("FULL") ||
                        (phuongThucCapNhat.Equals("CHANGE") && !maPhienBanTruoc.Equals(ClientInformation.Version)))
                    {
                        LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.SYS, "FULL_UPDATE");
                        string tenFile = phienBanDTO.ListPhienBanItemDTO.FirstOrDefault().HtPbanFile.TEN_FILE;
                        string folderPath = ClientInformation.OtaVersionDir + "\\" + HtPban.MA_PBAN;
                        string filePath = ClientInformation.OtaVersionDir + "\\" + HtPban.MA_PBAN + "\\" + tenFile;
                        PathFile = filePath;

                        if (Directory.Exists(folderPath))
                        {
                            LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.SYS, "DeleteDirectory: " + folderPath);
                            Directory.Delete(folderPath, true);
                        }
                        LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.SYS, "CreateDirectory: " + folderPath);
                        
                        try
                        {
                            DirectoryInfo dirInfo = Directory.CreateDirectory(folderPath);
                            LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.SYS, "CreateDirectory result: " + dirInfo.ToString());
                        }
                        catch (System.Exception ex)
                        {
                            LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.SYS, "CreateDirectory result failed: " + ex);
                            ResContent = LLanguage.SearchResourceByKey("M.ResponseMessage.QuanTriHeThong.PhienBan.TaiVeKhongThanhCong");
                            lblContent.Content = ResContent;
                            prbProcess.Visibility = Visibility.Collapsed;
                            btnDownload.Visibility = Visibility.Collapsed;
                            btnUpdate.Visibility = Visibility.Collapsed;
                        }
                        LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.SYS, "WriteFileFromByteArray: " + filePath);
                        bool retWriteFile = LFile.WriteFileFromByteArray(phienBanDTO.ListPhienBanItemDTO.FirstOrDefault().HtPbanData.FileData, filePath);
                        LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.SYS, "WriteFileFromByteArray result: " + retWriteFile.ToString());
                        if (!retWriteFile)
                        {
                            ResStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                            ResContent = LLanguage.SearchResourceByKey("M.ResponseMessage.QuanTriHeThong.PhienBan.TaiVeKhongThanhCong");
                            Worker_Download.CancelAsync();
                        }
                        HtPban.PTHUC_CNHAT = "FULL";
                    }
                    // Nếu cập nhật phần thay đổi của ứng dụng
                    else if (phuongThucCapNhat.Equals("CHANGE"))
                    {
                        LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.SYS, "CHANGED_UPDATE");
                        List<PhienBanItemDTO> listPhienBanItemDTO = phienBanDTO.ListPhienBanItemDTO.ToList();
                        string folderPath = ClientInformation.OtaVersionDir + "\\" + HtPban.MA_PBAN;
                        PathFile = folderPath;

                        if (Directory.Exists(folderPath))
                        {
                            LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.SYS, "DeleteDirectory: " + folderPath);
                            Directory.Delete(folderPath, true);
                        }
                        LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.SYS, "CreateDirectory: " + folderPath);
                        
                        try
                        {
                            DirectoryInfo dirInfo = Directory.CreateDirectory(folderPath);
                            LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.SYS, "CreateDirectory result: " + dirInfo.ToString());
                        }
                        catch (System.Exception ex)
                        {
                            LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.SYS, "CreateDirectory result failed: " + ex);
                            ResContent = LLanguage.SearchResourceByKey("M.ResponseMessage.QuanTriHeThong.PhienBan.TaiVeKhongThanhCong");
                            lblContent.Content = ResContent;
                            prbProcess.Visibility = Visibility.Collapsed;
                            btnDownload.Visibility = Visibility.Collapsed;
                            btnUpdate.Visibility = Visibility.Collapsed;
                        }

                        foreach (PhienBanItemDTO item in listPhienBanItemDTO)
                        {
                            string tenFile = item.HtPbanFile.TEN_FILE;                        
                            string filePath = ClientInformation.OtaVersionDir + "\\" + HtPban.MA_PBAN + "\\" + ((item.HtPbanFile.DUONG_DAN != null && !item.HtPbanFile.DUONG_DAN.Equals("")) ? item.HtPbanFile.DUONG_DAN + "\\" : "") + tenFile;
                            LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.SYS, "WriteAbsFileFromByteArray: " + filePath);
                            bool retWriteFile = LFile.WriteAbsFileFromByteArray(item.HtPbanData.FileData, filePath);
                            LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.SYS, "WriteAbsFileFromByteArray result: " + retWriteFile.ToString());
                            if (!retWriteFile)
                            {
                                ResStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                                ResContent = LLanguage.SearchResourceByKey("M.ResponseMessage.QuanTriHeThong.PhienBan.TaiVeKhongThanhCong");
                                Worker_Download.CancelAsync();
                            }
                        }
                        HtPban.PTHUC_CNHAT = "CHANGE";
                    }
                    // Còn lại
                    else
                    {
                    
                    }
                    ResContent = LLanguage.SearchResourceByKey("M.ResponseMessage.QuanTriHeThong.PhienBan.CaiDat");
                }
                else if (ret == ApplicationConstant.ResponseStatus.KHONG_THANH_CONG)
                {
                    ResContent = LLanguage.SearchResourceByKey(responseMessage);
                }
                else
                {
                    ResContent = "";
                }

                Thread.Sleep(3000);
            }
            catch (Exception ex)
            {
                ResStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                ResContent = LLanguage.SearchResourceByKey("M.ResponseMessage.QuanTriHeThong.PhienBan.TaiVeKhongThanhCong");
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.SYS, ex);
                Worker_Download.CancelAsync();
                // Vẫn xử lý tại client cho logout
                //Application.Current.Shutdown();
            }
        }

        private void Worker_RunWorkerCompleted_Download(object sender,
                                               RunWorkerCompletedEventArgs e)
        {
            // complete
            lblContent.Content = ResContent;
            if (ResStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                prbProcess.Visibility = Visibility.Collapsed;
                btnDownload.Visibility = Visibility.Collapsed;
                btnUpdate.Visibility = Visibility.Visible;
            }
            else if (ResStatus == ApplicationConstant.ResponseStatus.KHONG_THANH_CONG || e.Cancelled == true)
            {
                prbProcess.Visibility = Visibility.Collapsed;
                btnDownload.Visibility = Visibility.Collapsed;
                btnUpdate.Visibility = Visibility.Collapsed;
            }
            else
            {
                prbProcess.Visibility = Visibility.Collapsed;
                btnDownload.Visibility = Visibility.Collapsed;
                btnUpdate.Visibility = Visibility.Collapsed;
            }
        }

        private void btnDownload_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ResContent = LLanguage.SearchResourceByKey("M.ResponseMessage.QuanTriHeThong.PhienBan.TaiVe");
                lblContent.Content = ResContent;
                btnDownload.Visibility = Visibility.Collapsed;
                btnUpdate.Visibility = Visibility.Collapsed;
                prbProcess.Visibility = Visibility.Visible;
                Worker_Download.RunWorkerAsync();
                Worker_Download.DoWork += Worker_DoWork_Download;
                Worker_Download.RunWorkerCompleted += Worker_RunWorkerCompleted_Download;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.SYS, ex);
                this.Cursor = Cursors.Arrow;
                // Vẫn xử lý tại client cho logout
                //Application.Current.Shutdown();
            }

        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string phuongThucCapNhat = HtPban.PTHUC_CNHAT;
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.SYS, "Update type: " + phuongThucCapNhat);
                // Nếu cập nhật toàn bộ ứng dụng
                if (phuongThucCapNhat.Equals("FULL"))
                {
                    Process oldProcess = Process.GetCurrentProcess();
                    int oldPid = Process.GetCurrentProcess().Id;
                    string pathFile = ClientInformation.WorkingDir + updateApp;
                    string updateDir = ClientInformation.WorkingDir + "updater";
                    string updateFile = updateDir + "\\" + updateApp;
                    string pathFileLog = ClientInformation.WorkingDir + "log4net.dll";
                    string updateFileLog = updateDir + "\\" + "log4net.dll";
                    if (Directory.Exists(updateDir))
                    {
                        LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.SYS, "Delete: " + updateDir);
                        FileAttributes fileAtt = File.GetAttributes(updateDir);
                        if (fileAtt != FileAttributes.Normal)
                            File.SetAttributes(updateDir, FileAttributes.Normal);
                        Directory.Delete(updateDir, true);
                    }
                    LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.SYS, "CreateDirectory: " + updateDir);
                    try
                    {
                        DirectoryInfo dirInfo = Directory.CreateDirectory(updateDir);
                        File.SetAttributes(updateDir, FileAttributes.Normal);
                        LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.SYS, "CreateDirectory result: " + dirInfo.ToString());
                    }
                    catch (System.Exception ex)
                    {
                        LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.SYS, "CreateDirectory result: " + ex);
                        ResContent = LLanguage.SearchResourceByKey("M.ResponseMessage.QuanTriHeThong.PhienBan.CapNhatKhongThanhCong");
                        lblContent.Content = ResContent;
                        prbProcess.Visibility = Visibility.Collapsed;
                        btnDownload.Visibility = Visibility.Collapsed;
                        btnUpdate.Visibility = Visibility.Collapsed;
                        return;
                    }         
                    File.Copy(pathFile, updateFile);
                    File.SetAttributes(updateFile, FileAttributes.Normal);
                    File.Copy(pathFileLog, updateFileLog);
                    File.SetAttributes(updateFileLog, FileAttributes.Normal);

                    string key = "mfclient.update.ngvgroup.vn";
                    string flag = "UPDATE";
                    string type = "FULL";
                    string path = PathFile;
                    string parameter = ClientInformation.WorkingDir + "@" + 
                        ClientInformation.Log4NetOutput + "@" + 
                        ClientInformation.OtaVersionDir + "@" + 
                        ClientInformation.Log4NetUpdConfig + "@" +
                        ClientInformation.Version + "@" + 
                        LastestClientVersion + "@" + 
                        key + "@" + 
                        flag + "@" + 
                        type + "@" + 
                        path;

                    parameter = parameter.Replace(' ', '*');

                    LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.SYS, parameter);
                    System.Diagnostics.Process.Start(updateFile, parameter);
                    oldProcess.Kill();
                }
                // Nếu cập nhật phần thay đổi của ứng dụng
                else if (phuongThucCapNhat.Equals("CHANGE"))
                {
                    Process oldProcess = Process.GetCurrentProcess();
                    int oldPid = Process.GetCurrentProcess().Id;
                    string pathFile = ClientInformation.WorkingDir + updateApp;
                    string updateDir = ClientInformation.WorkingDir + "updater";
                    string updateFile = updateDir + "\\" + updateApp;
                    string pathFileLog = ClientInformation.WorkingDir + "log4net.dll";
                    string updateFileLog = updateDir + "\\" + "log4net.dll";
                    if (Directory.Exists(updateDir))
                    {
                        LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.SYS, "Delete: " + updateDir);
                        FileAttributes fileAtt = File.GetAttributes(updateDir);
                        if (fileAtt != FileAttributes.Normal)
                            File.SetAttributes(updateDir, FileAttributes.Normal);
                        Directory.Delete(updateDir, true);
                    }
                    LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.SYS, "CreateDirectory: " + updateDir);
                    try
                    {
                        DirectoryInfo dirInfo = Directory.CreateDirectory(updateDir);
                        File.SetAttributes(updateDir, FileAttributes.Normal);
                        LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.SYS, "CreateDirectory result: " + dirInfo.ToString());
                    }
                    catch (System.Exception ex)
                    {
                        LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.SYS, "CreateDirectory result failed: " + ex);
                        ResContent = LLanguage.SearchResourceByKey("M.ResponseMessage.QuanTriHeThong.PhienBan.CapNhatKhongThanhCong");
                        lblContent.Content = ResContent;
                        prbProcess.Visibility = Visibility.Collapsed;
                        btnDownload.Visibility = Visibility.Collapsed;
                        btnUpdate.Visibility = Visibility.Collapsed;
                        return;
                    }
                    File.Copy(pathFile, updateFile);
                    File.SetAttributes(updateFile, FileAttributes.Normal);
                    File.Copy(pathFileLog, updateFileLog);
                    File.SetAttributes(updateFileLog, FileAttributes.Normal);

                    string key = "mfclient.update.ngvgroup.vn";
                    string flag = "UPDATE";
                    string type = "CHANGE";
                    string path = PathFile;
                    string parameter = ClientInformation.WorkingDir + "@" +
                        ClientInformation.Log4NetOutput + "@" + 
                        ClientInformation.OtaVersionDir + "@" + 
                        ClientInformation.Log4NetUpdConfig + "@" +
                        ClientInformation.Version + "@" + 
                        LastestClientVersion + "@" + 
                        key + "@" + 
                        flag + "@" + 
                        type + "@" + 
                        path;

                    parameter = parameter.Replace(' ', '*');
                    
                    LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.SYS, parameter);
                    System.Diagnostics.Process.Start(updateFile, parameter);
                    oldProcess.Kill();
                }
                // Còn lại
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.SYS, ex);
                ResContent = LLanguage.SearchResourceByKey("M.ResponseMessage.QuanTriHeThong.PhienBan.CapNhatKhongThanhCong");
                lblContent.Content = ResContent;
                prbProcess.Visibility = Visibility.Collapsed;
                btnDownload.Visibility = Visibility.Collapsed;
                btnUpdate.Visibility = Visibility.Collapsed;
                // Vẫn xử lý tại client cho logout
                //Application.Current.Shutdown();
            }
            
        }
    }
}
