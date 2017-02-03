using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Description;
using Utilities.Common;
using System.ServiceModel;
using Presentation.Process.ZAMainAppServiceRef;
using Presentation.Process.QuanTriHeThongServiceRef;
using Presentation.Process.DanhMucServiceRef;
using System.Net.NetworkInformation;
using System.Data;
using System.Xml;
using System.Web;

namespace Presentation.Process.Common
{
    public static class WebUtilities
    {
        
        /// <summary>
        /// Lấy địa chỉ service
        /// </summary>
        /// <param name="endpointAddress"></param>
        /// <returns></returns>
        public static EndpointAddress getEndpointAddress(string endpointAddress, string serverIP, string serverPort)
        {
            string address = "http://" + serverIP + ":" + serverPort + "/" + endpointAddress + ".svc";
            EndpointAddress endpoint = new EndpointAddress(address);
            return endpoint;
        }
        
        /// <summary>
        /// Thiết lập thông số dịch vụ kết nối giữa client và server
        /// </summary>
        /// <returns></returns>
        public static BasicHttpBinding getBasicHttpBinding(string bindingName)
        {
            System.ServiceModel.BasicHttpBinding binding = new System.ServiceModel.BasicHttpBinding();

            System.ServiceModel.ServiceBehaviorAttribute behavior = new System.ServiceModel.ServiceBehaviorAttribute();

            behavior.MaxItemsInObjectGraph = 2147483647;

            binding.Name = bindingName;

            //binding.CloseTimeout = new TimeSpan(0, 1, 0);
            binding.CloseTimeout = new TimeSpan(0, 30, 0);
            //binding.OpenTimeout = new TimeSpan(0, 1, 0);
            binding.OpenTimeout = new TimeSpan(0, 30, 0);
            binding.ReceiveTimeout = new TimeSpan(0, 30, 0);
            //binding.SendTimeout = new TimeSpan(0, 1, 0);
            binding.SendTimeout = new TimeSpan(0, 30, 0);
            binding.AllowCookies = true;
            binding.BypassProxyOnLocal = true;
            binding.HostNameComparisonMode = System.ServiceModel.HostNameComparisonMode.StrongWildcard;
            binding.MaxBufferSize = 2147483647;
            binding.MaxBufferPoolSize = 2147483647;
            binding.MaxReceivedMessageSize = 2147483647;
            binding.MessageEncoding = System.ServiceModel.WSMessageEncoding.Text;
            binding.TextEncoding = System.Text.Encoding.UTF8;
            binding.TransferMode = System.ServiceModel.TransferMode.Streamed;
            binding.UseDefaultWebProxy = false;

            binding.ReaderQuotas.MaxDepth = 2147483647;
            binding.ReaderQuotas.MaxStringContentLength = 2147483647;
            binding.ReaderQuotas.MaxArrayLength = 2147483647;
            binding.ReaderQuotas.MaxBytesPerRead = 2147483647;
            binding.ReaderQuotas.MaxNameTableCharCount = 2147483647;

            binding.Security.Mode = System.ServiceModel.BasicHttpSecurityMode.None;
            binding.Security.Transport.ClientCredentialType = System.ServiceModel.HttpClientCredentialType.None;
            binding.Security.Transport.ProxyCredentialType = System.ServiceModel.HttpProxyCredentialType.None;
            binding.Security.Message.ClientCredentialType = System.ServiceModel.BasicHttpMessageCredentialType.UserName;
            binding.Security.Message.AlgorithmSuite = System.ServiceModel.Security.SecurityAlgorithmSuite.Default;

            return binding;
        }

        /*
        public static WebHttpBinding getWebHttpBinding(string bindingName)
        {
            System.ServiceModel.WebHttpBinding binding = new System.ServiceModel.WebHttpBinding();

            System.ServiceModel.ServiceBehaviorAttribute behavior = new System.ServiceModel.ServiceBehaviorAttribute();

            behavior.MaxItemsInObjectGraph = 2147483647;

            binding.Name = bindingName;

            //binding.CloseTimeout = new TimeSpan(0, 1, 0);
            binding.CloseTimeout = new TimeSpan(0, 10, 0);
            //binding.OpenTimeout = new TimeSpan(0, 1, 0);
            binding.OpenTimeout = new TimeSpan(0, 10, 0);
            binding.ReceiveTimeout = new TimeSpan(0, 10, 0);
            //binding.SendTimeout = new TimeSpan(0, 1, 0);
            binding.SendTimeout = new TimeSpan(0, 10, 0);
            binding.AllowCookies = false;
            binding.BypassProxyOnLocal = false;
            binding.HostNameComparisonMode = System.ServiceModel.HostNameComparisonMode.StrongWildcard;
            binding.MaxBufferSize = 2147483647;
            binding.MaxBufferPoolSize = 2147483647;
            binding.MaxReceivedMessageSize = 2147483647;
            //binding.MessageEncoding = System.ServiceModel.WSMessageEncoding.Text;
            //binding.TextEncoding = System.Text.Encoding.UTF8;
            binding.TransferMode = System.ServiceModel.TransferMode.Streamed;
            binding.UseDefaultWebProxy = true;

            binding.ReaderQuotas.MaxDepth = 2147483647;
            binding.ReaderQuotas.MaxStringContentLength = 2147483647;
            binding.ReaderQuotas.MaxArrayLength = 2147483647;
            binding.ReaderQuotas.MaxBytesPerRead = 2147483647;
            binding.ReaderQuotas.MaxNameTableCharCount = 2147483647;

            //binding.Security.Mode = System.ServiceModel.BasicHttpSecurityMode.None;
            binding.Security.Transport.ClientCredentialType = System.ServiceModel.HttpClientCredentialType.None;
            binding.Security.Transport.ProxyCredentialType = System.ServiceModel.HttpProxyCredentialType.None;
            //binding.Security.Message.ClientCredentialType = System.ServiceModel.BasicHttpMessageCredentialType.UserName;
            //binding.Security.Message.AlgorithmSuite = System.ServiceModel.Security.SecurityAlgorithmSuite.Default;

            return binding;
        }
        */

        public static NetTcpBinding getNetTcpBinding(string bindingName)
        {
            System.ServiceModel.NetTcpBinding binding = new System.ServiceModel.NetTcpBinding();

            System.ServiceModel.ServiceBehaviorAttribute behavior = new System.ServiceModel.ServiceBehaviorAttribute();

            behavior.MaxItemsInObjectGraph = 2147483647;

            binding.Name = bindingName;

            /*
            //binding.CloseTimeout = new TimeSpan(0, 1, 0);
            binding.CloseTimeout = new TimeSpan(0, 10, 0);
            //binding.OpenTimeout = new TimeSpan(0, 1, 0);
            binding.OpenTimeout = new TimeSpan(0, 10, 0);
            binding.ReceiveTimeout = new TimeSpan(0, 10, 0);
            //binding.SendTimeout = new TimeSpan(0, 1, 0);
            binding.SendTimeout = new TimeSpan(0, 10, 0);
            binding.AllowCookies = false;
            binding.BypassProxyOnLocal = false;
            binding.HostNameComparisonMode = System.ServiceModel.HostNameComparisonMode.StrongWildcard;
            binding.MaxBufferSize = 2147483647;
            binding.MaxBufferPoolSize = 2147483647;
            binding.MaxReceivedMessageSize = 2147483647;
            binding.MessageEncoding = System.ServiceModel.WSMessageEncoding.Text;
            binding.TextEncoding = System.Text.Encoding.UTF8;
            binding.TransferMode = System.ServiceModel.TransferMode.Streamed;
            binding.UseDefaultWebProxy = true;

            binding.ReaderQuotas.MaxDepth = 2147483647;
            binding.ReaderQuotas.MaxStringContentLength = 2147483647;
            binding.ReaderQuotas.MaxArrayLength = 2147483647;
            binding.ReaderQuotas.MaxBytesPerRead = 2147483647;
            binding.ReaderQuotas.MaxNameTableCharCount = 2147483647;

            binding.Security.Mode = System.ServiceModel.BasicHttpSecurityMode.None;
            binding.Security.Transport.ClientCredentialType = System.ServiceModel.HttpClientCredentialType.None;
            binding.Security.Transport.ProxyCredentialType = System.ServiceModel.HttpProxyCredentialType.None;
            binding.Security.Message.ClientCredentialType = System.ServiceModel.BasicHttpMessageCredentialType.UserName;
            binding.Security.Message.AlgorithmSuite = System.ServiceModel.Security.SecurityAlgorithmSuite.Default;
            */
            return binding;
        }
        
        /// <summary>
        /// Lấy địa chỉ MAC
        /// </summary>
        /// <returns></returns>
        public static string GetMacAddress()
        {
            string macAddresses = "";
            
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                // Only consider Ethernet network interfaces, thereby ignoring any
                // loopback devices etc.

                if (nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                {
                    macAddresses = nic.GetPhysicalAddress().ToString();
                }

            }
            return macAddresses;
        }

        /// <summary>
        /// Lấy địa chỉ IP
        /// </summary>
        /// <returns></returns>
        public static string GetIpAddress()
        {
            string ipAddresses = "";
            System.Net.IPAddress localIP = System.Net.Dns.GetHostAddresses(System.Net.Dns.GetHostName()).FirstOrDefault(e => e.AddressFamily.ToString().Equals("InterNetwork"));
            ipAddresses = localIP.ToString();
            return ipAddresses;
        }

        /// <summary>
        /// Set các thông tin cơ bản của một request
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        public static T PrepareRequest<T>(T request, UserInformation userInformation) where T : class
        {
            request.GetType().GetProperty("RequestId").SetValue(request, Guid.NewGuid().ToString(), null);
            request.GetType().GetProperty("Company").SetValue(request, ApplicationConstant.layDonViSuDung(userInformation.Company), null);
            request.GetType().GetProperty("UserId").SetValue(request, userInformation.IdNguoiSuDung, null);
            request.GetType().GetProperty("UserName").SetValue(request, userInformation.TenDangNhap, null);
            request.GetType().GetProperty("ClientIP").SetValue(request, userInformation.IpAddress, null);
            request.GetType().GetProperty("ClientMAC").SetValue(request, userInformation.MacAddress, null);
            request.GetType().GetProperty("RequestSecurityKey").SetValue(request, userInformation.RequestSecurityKey, null);
            request.GetType().GetProperty("SessionId").SetValue(request, userInformation.SessionId, null);
            request.GetType().GetProperty("DeptCode").SetValue(request, userInformation.MaDonVi, null);
            request.GetType().GetProperty("OprDeptCode").SetValue(request, userInformation.MaDonViGiaoDich, null);
            request.GetType().GetProperty("MngDeptCode").SetValue(request, userInformation.MaDonViQuanLy, null);
            request.GetType().GetProperty("NgayLamViec").SetValue(request, userInformation.NgayLamViecHienTai, null);

            return request;
        }

        /// <summary>
        /// Kiểm tra kết nối tới một dịch vụ trên Server
        /// </summary>
        /// <param name="serverIp"></param>
        /// <param name="serverPort"></param>
        /// <param name="serviceEndpoint"></param>
        /// <returns></returns>
        public static void IsServiceAvailable(string serverIp, string serverPort, string serviceEndpoint)
        {
            //bool isAvailable = true;
            try
            {
                string address = "http://" + serverIp + ":" + serverPort + "/" + serviceEndpoint + ".svc?wsdl";
                //MetadataExchangeClient _mexClient = null;
                //_mexClient = new MetadataExchangeClient(new Uri(address), MetadataExchangeClientMode.HttpGet);
                //MetadataSet _metadataSet = _mexClient.GetMetadata();
                //isAvailable = true;

                System.ServiceModel.BasicHttpBinding _binding = getBasicHttpBinding(serviceEndpoint);
                _binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Windows;
                _binding.MaxReceivedMessageSize = 52428800;
                System.Xml.XmlDictionaryReaderQuotas xmlDictionaryReaderQuotas = new XmlDictionaryReaderQuotas();
                xmlDictionaryReaderQuotas.MaxNameTableCharCount = 2147483647;
                _binding.ReaderQuotas = xmlDictionaryReaderQuotas;
                
                MetadataExchangeClient __mexClient = null;

                MetadataSet __metadataSet = null;

                if (_binding != null)
                {
                    __mexClient = new MetadataExchangeClient(_binding);
                    __mexClient.MaximumResolvedReferences = 1000;
                    __metadataSet = __mexClient.GetMetadata(new Uri(address), MetadataExchangeClientMode.HttpGet);
                }

                else
                {
                    //Import metadata
                    __mexClient = new MetadataExchangeClient(new Uri(address), MetadataExchangeClientMode.HttpGet);
                    __mexClient.MaximumResolvedReferences = 1000;
                    __metadataSet = __mexClient.GetMetadata();
                } 
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.ERR, ex);
                throw new CustomException("M.ResponseMessage.Common.ServiceIsNotAvailable", null);
            }
            //return isAvailable;
        }

        /// <summary>
        /// Kiểm tra kết nối tới một dịch vụ trên Server
        /// </summary>
        /// <param name="serverIp"></param>
        /// <param name="serverPort"></param>
        /// <param name="serviceEndpoint"></param>
        /// <returns></returns>
        public static void IsServerAvailable(string serverIp, string serverPort)
        {
            //bool isAvailable = true;
            try
            {
                //isAvailable = true;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.ERR, ex);
                throw new CustomException("M.ResponseMessage.Common.ServerIsNotAvailable", null);
            }
            //return isAvailable;
        }

        /// <summary>
        /// Kiểm tra kết nối tới một dịch vụ trên Server
        /// </summary>
        /// <param name="serverIp"></param>
        /// <param name="serverPort"></param>
        /// <param name="serviceEndpoint"></param>
        /// <returns></returns>
        public static void IsConnectionAvailable(string serverIp)
        {
            //bool isAvailable = true;
            try
            {
                //isAvailable = true;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.ERR, ex);
                throw new CustomException("M.ResponseMessage.Common.ServerConnectionError", null);
            }
            //return isAvailable;
        }

        public static void IsRequestAllow(string serverIp, string serverPort, string serviceEndpoint)
        {
            IsConnectionAvailable(serverIp);
            IsServerAvailable(serverIp, serverPort);
            IsServiceAvailable(serverIp, serverPort, serviceEndpoint);
        }

        //public static void IsRequestAllow(string serviceEndpoint, string serverIP, string serverPort)
        //{
        //    // Truongnx: test performance
        //    //return;
        //    IsRequestAllow(serverIP, serverPort, serviceEndpoint);
        //}

        public static List<ClientResponseDetail> convertToClientResponseDetail<R>(R response) where R : class
        {
            List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
            List<object> responseDetail = ((object[])response.GetType().GetProperty("ResponseDetail").GetValue(response, null)).ToList();

            int i = 0 ;
            foreach (object item in responseDetail)
            {
                int id = int.Parse(item.GetType().GetProperty("Id").GetValue(item, null).ToString());
                string obj = item.GetType().GetProperty("Object").GetValue(item, null).IsNullOrEmpty() ? "" : item.GetType().GetProperty("Object").GetValue(item, null).ToString();
                string operation = item.GetType().GetProperty("Operation").GetValue(item, null).IsNullOrEmpty() ? "" : item.GetType().GetProperty("Operation").GetValue(item, null).ToString();
                string result = item.GetType().GetProperty("Result").GetValue(item, null).IsNullOrEmpty() ? "" : item.GetType().GetProperty("Result").GetValue(item, null).ToString();
                string detail = item.GetType().GetProperty("Detail").GetValue(item, null).IsNullOrEmpty() ? "" : item.GetType().GetProperty("Detail").GetValue(item, null).ToString();

                ++i;
                ClientResponseDetail clientResponseDetail = new ClientResponseDetail();
                clientResponseDetail.Stt = i;
                clientResponseDetail.Id = id;
                clientResponseDetail.Object = obj;
                clientResponseDetail.Operation = LLanguage.SearchResourceByKey(operation);
                clientResponseDetail.Result = LLanguage.SearchResourceByKey(result);
                clientResponseDetail.Detail = LLanguage.SearchResourceByKey(detail, '#');

                listClientResponseDetail.Add(clientResponseDetail);
            }
            return listClientResponseDetail;
        }


        public static void ValidResponse<T, U>(T request, U response, UserInformation userInformation) where T : class
        {
            if ((response == null))
            {
                // Set trạng thái xử lý cho client
                userInformation.OperationStatus = ApplicationConstant.OperationStatus.NoResponse.layGiaTri();

                // Ghi log
                LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, "No response from Server");
                throw new CustomException("M.ResponseMessage.Common.NoResponse", null);
            }
            // Nếu có thông tin trả về
            else
            {
                // Kiểm tra ResponseId
                if (response.GetType().GetProperty("ResponseId").GetValue(response, null).ToString() != request.GetType().GetProperty("RequestId").GetValue(request, null).ToString())
                {
                    // Ghi log
                    LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, "ResponseId and RequestId do not match");
                    throw new CustomException("M.ResponseMessage.Common.ResponseIdIsNotMatch", null);
                }

                // Kiểm tra ResposeSecurityKey
                if (response.GetType().GetProperty("ResponseSecurityKey").GetValue(response, null).ToString() != userInformation.ResponsetSecurityKey)
                {
                    // Ghi log
                    LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, "Invalid SecurityResponseKey");
                    throw new CustomException("M.ResponseMessage.Common.InvalidSecurityResponseKey", null);
                }
                //// Kiểm tra ResponseMessage
                //if (response.GetType().GetProperty("ResponseMessage").GetValue(response, null) != null &&
                //    response.GetType().GetProperty("ResponseMessage").GetValue(response, null).ToString() == ApplicationConstant.CommonResponseMessage.M_ResponseMessage_Common_InvalidOrExpiredOperationSession.layGiaTri())
                //{
                //    // Ghi log
                //    LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, response.GetType().GetProperty("ResponseMessage").GetValue(response, null).ToString());
                //    throw new CustomException("M.ResponseMessage.Common.InvalidOrExpiredOperationSession", null);
                //}

                // Nếu không thành công
                if (response.GetType().GetProperty("ResponseStatus").GetValue(response, null).ToString() == ApplicationConstant.ResponseStatus.KHONG_THANH_CONG.ToString())
                {
                    object responseMessage = response.GetType().GetProperty("ResponseMessage").GetValue(response, null);
                    byte[] exception = (byte[])response.GetType().GetProperty("ExceptionObject").GetValue(response, null);

                    if ((responseMessage != null) && (responseMessage.ToString() == ApplicationConstant.CommonResponseMessage.M_ResponseMessage_Common_InvalidOrExpiredOperationSession.layGiaTri()))
                    {
                        // Set trạng thái xử lý cho client
                        userInformation.OperationStatus = ApplicationConstant.OperationStatus.InvalidOrExpiredOperationSession.layGiaTri();
                        // Ghi log
                        LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, response.GetType().GetProperty("ResponseMessage").GetValue(response, null).ToString());
                        throw new CustomException(responseMessage.ToString(), null);
                    }
                    else if ((responseMessage != null) && (responseMessage.ToString() == ApplicationConstant.CommonResponseMessage.M_ResponseMessage_Common_LoseSession.layGiaTri()))
                    {
                        // Set trạng thái xử lý cho client
                        userInformation.OperationStatus = ApplicationConstant.OperationStatus.LoseSession.layGiaTri();
                        // Ghi log
                        LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, response.GetType().GetProperty("ResponseMessage").GetValue(response, null).ToString());
                        throw new CustomException(responseMessage.ToString(), null);
                    }
                    else if ((responseMessage != null) && (responseMessage.ToString() == ApplicationConstant.CommonResponseMessage.M_ResponseMessage_Common_InvalidWorkingDay.layGiaTri()))
                    {
                        // Set trạng thái xử lý cho client
                        userInformation.OperationStatus = ApplicationConstant.OperationStatus.InvalidWorkingDay.layGiaTri();
                        // Ghi log
                        LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, response.GetType().GetProperty("ResponseMessage").GetValue(response, null).ToString());
                        throw new CustomException(responseMessage.ToString(), null);
                    }
                    /*
                    else if ((responseMessage != null) && (responseMessage.ToString() == ApplicationConstant.LoginResponseMessage.M_ResponseMessage_Login_LoadUserRolesFailed.layGiaTri()))
                    {
                        // Ghi log
                        LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, response.GetType().GetProperty("ResponseMessage").GetValue(response, null).ToString());
                        throw new CustomException(responseMessage.ToString(), null);
                    }
                    else if ((responseMessage != null) && (responseMessage.ToString() == ApplicationConstant.LoginResponseMessage.M_ResponseMessage_Login_LoadUserBranchesFailed.layGiaTri()))
                    {
                        // Ghi log
                        LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, response.GetType().GetProperty("ResponseMessage").GetValue(response, null).ToString());
                        throw new CustomException(responseMessage.ToString(), null);
                    }
                    else if ((responseMessage != null) && (responseMessage.ToString() == ApplicationConstant.LoginResponseMessage.M_ResponseMessage_Login_YeuCauDoiMatKhauLanDau.layGiaTri()))
                    {
                        // Ghi log
                        LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, response.GetType().GetProperty("ResponseMessage").GetValue(response, null).ToString());
                        throw new CustomException(responseMessage.ToString(), null);
                    }
                    */
                    else if ((responseMessage != null) && (exception != null))
                    {
                        // Ghi log
                        LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, response.GetType().GetProperty("ResponseMessage").GetValue(response, null).ToString());
                        throw new CustomException(responseMessage.ToString(), exception != null ? LCollection.Deserialize(exception) : null);
                    }
                    /*
                    else 
                    {
                        // Ghi log
                        LLogging.WriteLog(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.ToString(), LLogging.LogType.SYS, "M.ResponseMessage.Common.KhongThanhCong");
                        //throw new CustomException("M.ResponseMessage.Common.KhongThanhCong", null);
                        //throw new CustomException("M.ResponseMessage.Common.KhongThanhCong", LCollection.Deserialize(response.GetType().GetProperty("ExceptionObject").GetValue(response, null)));
                    } 
                    */
                }
            }
        }

        public static void SaveConfiguration(string name, string ip, string port, UserInformation userInformation)
        {
            try
            {
                string ApplicationPath = AppDomain.CurrentDomain.BaseDirectory;
                //  Create new DataTable.
                DataTable dt = new DataTable("Config");
                DataSet ds = new DataSet("Framework");
                //  Declare DataColumn and DataRow variables.
                DataColumn dc;
                DataRow dr;

                dc = new DataColumn();
                dc.DataType = System.Type.GetType("System.String");
                dc.ColumnName = "Company";
                dt.Columns.Add(dc);

                dc = new DataColumn();
                dc.DataType = System.Type.GetType("System.String");
                dc.ColumnName = "WorkingDir";
                dt.Columns.Add(dc);

                dc = new DataColumn();
                dc.DataType = Type.GetType("System.String");
                dc.ColumnName = "ConfigDir";
                dt.Columns.Add(dc);

                dc = new DataColumn();
                dc.DataType = Type.GetType("System.String");
                dc.ColumnName = "DataDir";
                dt.Columns.Add(dc);

                dc = new DataColumn();
                dc.DataType = Type.GetType("System.String");
                dc.ColumnName = "HelpDir";
                dt.Columns.Add(dc);

                dc = new DataColumn();
                dc.DataType = Type.GetType("System.String");
                dc.ColumnName = "ImagesDir";
                dt.Columns.Add(dc);

                dc = new DataColumn();
                dc.DataType = Type.GetType("System.String");
                dc.ColumnName = "LanguagesDir";
                dt.Columns.Add(dc);

                dc = new DataColumn();
                dc.DataType = Type.GetType("System.String");
                dc.ColumnName = "TempDir";
                dt.Columns.Add(dc);

                dc = new DataColumn();
                dc.DataType = Type.GetType("System.String");
                dc.ColumnName = "IconName";
                dt.Columns.Add(dc);

                dc = new DataColumn();
                dc.DataType = Type.GetType("System.String");
                dc.ColumnName = "ShortName";
                dt.Columns.Add(dc);

                dc = new DataColumn();
                dc.DataType = Type.GetType("System.String");
                dc.ColumnName = "FullName";
                dt.Columns.Add(dc);

                dc = new DataColumn();
                dc.DataType = Type.GetType("System.String");
                dc.ColumnName = "VersionDir";
                dt.Columns.Add(dc);

                dc = new DataColumn();
                dc.DataType = Type.GetType("System.String");
                dc.ColumnName = "BackupVersionDir";
                dt.Columns.Add(dc);

                dc = new DataColumn();
                dc.DataType = Type.GetType("System.String");
                dc.ColumnName = "CurrentVersionDir";
                dt.Columns.Add(dc);

                dc = new DataColumn();
                dc.DataType = Type.GetType("System.String");
                dc.ColumnName = "DefaultVersionDir";
                dt.Columns.Add(dc);

                dc = new DataColumn();
                dc.DataType = Type.GetType("System.String");
                dc.ColumnName = "OtaVersionDir";
                dt.Columns.Add(dc);

                dc = new DataColumn();
                dc.DataType = Type.GetType("System.String");
                dc.ColumnName = "Log4NetConfig";
                dt.Columns.Add(dc);

                dc = new DataColumn();
                dc.DataType = Type.GetType("System.String");
                dc.ColumnName = "Log4NetUpdConfig";
                dt.Columns.Add(dc);

                dc = new DataColumn();
                dc.DataType = Type.GetType("System.String");
                dc.ColumnName = "Log4NetOutput";
                dt.Columns.Add(dc);

                dc = new DataColumn();
                dc.DataType = Type.GetType("System.String");
                dc.ColumnName = "ServerList";
                dt.Columns.Add(dc);

                dc = new DataColumn();
                dc.DataType = Type.GetType("System.String");
                dc.ColumnName = "ServerName";
                dt.Columns.Add(dc);

                dc = new DataColumn();
                dc.DataType = Type.GetType("System.String");
                dc.ColumnName = "ServerIP";
                dt.Columns.Add(dc);

                dc = new DataColumn();
                dc.DataType = Type.GetType("System.String");
                dc.ColumnName = "ServerPort";
                dt.Columns.Add(dc);

                dc = new DataColumn();
                dc.DataType = Type.GetType("System.String");
                dc.ColumnName = "License";
                dt.Columns.Add(dc);

                dc = new DataColumn();
                dc.DataType = Type.GetType("System.String");
                dc.ColumnName = "Version";
                dt.Columns.Add(dc);

                //  Create new DataRow objects and add to DataTable.    
                DataTable dtConfig = userInformation.DataTableConfig;
                dr = dt.NewRow();
                dr["Company"] = dtConfig.Rows[0]["Company"].ToString();
                dr["WorkingDir"] = dtConfig.Rows[0]["WorkingDir"].ToString();
                dr["ConfigDir"] = dtConfig.Rows[0]["ConfigDir"].ToString();
                dr["DataDir"] = dtConfig.Rows[0]["DataDir"].ToString();
                dr["HelpDir"] = dtConfig.Rows[0]["HelpDir"].ToString();
                dr["ImagesDir"] = dtConfig.Rows[0]["ImagesDir"].ToString();
                dr["LanguagesDir"] = dtConfig.Rows[0]["LanguagesDir"].ToString();
                dr["TempDir"] = dtConfig.Rows[0]["TempDir"].ToString();
                dr["IconName"] = dtConfig.Rows[0]["IconName"].ToString();
                dr["ShortName"] = dtConfig.Rows[0]["ShortName"].ToString();
                dr["FullName"] = dtConfig.Rows[0]["FullName"].ToString();
                dr["VersionDir"] = dtConfig.Rows[0]["VersionDir"].ToString();
                dr["BackupVersionDir"] = dtConfig.Rows[0]["BackupVersionDir"].ToString();
                dr["CurrentVersionDir"] = dtConfig.Rows[0]["CurrentVersionDir"].ToString();
                dr["DefaultVersionDir"] = dtConfig.Rows[0]["DefaultVersionDir"].ToString();
                dr["OtaVersionDir"] = dtConfig.Rows[0]["OtaVersionDir"].ToString();
                dr["Log4NetConfig"] = dtConfig.Rows[0]["Log4NetConfig"].ToString();
                dr["Log4NetUpdConfig"] = dtConfig.Rows[0]["Log4NetUpdConfig"].ToString();
                dr["Log4NetOutput"] = dtConfig.Rows[0]["Log4NetOutput"].ToString();
                dr["ServerList"] = dtConfig.Rows[0]["ServerList"].ToString();
                dr["ServerName"] = name; userInformation.ServerName = name;
                dr["ServerIP"] = ip; userInformation.ServerIP = ip;
                dr["ServerPort"] = port; userInformation.ServerPort = port;
                dr["License"] = dtConfig.Rows[0]["License"].ToString();
                dr["Version"] = dtConfig.Rows[0]["Version"].ToString();

                dt.Rows.Add(dr);
                ds.Tables.Add(dt);

                string systemPath = AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
                string filePath = systemPath + "config\\config.conf";
                string filePathTemp = systemPath + "config\\config.conf_";
                ds.WriteXml(filePathTemp);

                string keyEncrypt = "!=Q|A'Z?";
                if (LSecurity.DESEncryptFile(filePathTemp, filePath, keyEncrypt))
                {
                    new ClientInitProcess().docLaiThongTinCauHinhClient(dt);
                    LMessage.ShowMessage("Thay đổi thông tin kết nối thành công", LMessage.MessageBoxType.Information);
                }
                else
                {
                }
                LFile.DeleteFile(filePathTemp);
            }
            catch (Exception ex)
            {
                LMessage.ShowMessage("SaveConfiguration", LMessage.MessageBoxType.Warning);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }
    }
}
