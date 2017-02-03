using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities.Common;
using System.IO;
using System.Xml.Linq;
using System.Data;

namespace Presentation.Process.Common
{
    public class ClientInitProcess
    {
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
                    else Console.WriteLine("Presentation.Process.Common: Chua khai bao tham so LanguageList trong file cau hinh");

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

                    ClientInformation.IpAddress = Utilities.GetIpAddress();
                    ClientInformation.MacAddress = Utilities.GetMacAddress();

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

        public bool docLaiThongTinCauHinhClient(DataTable dt)
        {
            try
            {

                // string filePath=@"D:\InCompany\Resources\VSS\NG-mFINA\2.SourceCode\NG.mFinance\Build\Build.Client\Dev\config\config.conf";
                string systemPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
                string filePath = systemPath + "config\\config.conf";

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

                    ClientInformation.IpAddress = Utilities.GetIpAddress();
                    ClientInformation.MacAddress = Utilities.GetMacAddress();

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
    }    
}
