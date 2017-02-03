using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;

namespace PresentationAspNet.MVC
{
    public class ConnectService
    {

        /// <summary>
        /// Lấy địa chỉ service
        /// </summary>
        /// <param name="endpointAddress"></param>
        /// <returns></returns>
        public static EndpointAddress getEndpointAddress(string endpointAddress)
        {
            string address = "http://localhost:4505/Service.svc";
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
    }
}