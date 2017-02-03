using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Utilities.Common;
using Presentation.Process.UtilitiesServiceRef;
using System.Data;
using Presentation.Process.Common;
using Presentation.Process.TruyVanServiceRef;
using System.ServiceModel.Description;

namespace Presentation.Process
{
    /// <summary>
    /// Thư viện tiện ích của server
    /// </summary>
    public class UtilitiesProcess
    {
        /// <summary>
        /// Biến cục bộ chứa dịch vụ tiện ích của server
        /// </summary>
        private static TruyVanServiceClient ClientTruyVan { get; set; }
        private static UtilitiesServiceClient Client { get; set; }

        /// <summary>
        /// Phương thức khởi tạo
        /// </summary>
        static UtilitiesProcess()
        {
            EndpointAddress endpointAddressTruyVan = Common.Utilities.getEndpointAddress(ApplicationConstant.SystemService.TruyVanService.layGiaTri());
            BasicHttpBinding basicHttpBindingTruyVan = Common.Utilities.getBasicHttpBinding(ApplicationConstant.SystemService.TruyVanService.layGiaTri());
            ClientTruyVan = new TruyVanServiceClient(basicHttpBindingTruyVan, endpointAddressTruyVan);

            foreach (var operationDescription in ClientTruyVan.Endpoint.Contract.Operations)
            {
                DataContractSerializerOperationBehavior dcsob =
                    operationDescription.Behaviors.Find<DataContractSerializerOperationBehavior>();
                if (dcsob != null)
                {
                    dcsob.MaxItemsInObjectGraph = 2147483646;
                }
            }

            //Client = new UtilitiesServiceClient();
            EndpointAddress endpointAddress = Common.Utilities.getEndpointAddress(ApplicationConstant.SystemService.UtilitiesService.layGiaTri());
            BasicHttpBinding basicHttpBinding = Common.Utilities.getBasicHttpBinding(ApplicationConstant.SystemService.UtilitiesService.layGiaTri());
            Client = new UtilitiesServiceClient(basicHttpBinding, endpointAddress);

            foreach (var operationDescription in Client.Endpoint.Contract.Operations)
            {
                DataContractSerializerOperationBehavior dcsob =
                    operationDescription.Behaviors.Find<DataContractSerializerOperationBehavior>();
                if (dcsob != null)
                {
                    dcsob.MaxItemsInObjectGraph = 2147483646;
                }
            }
        }

        /// <summary>
        /// Kiểm tra ngày tháng có phải là ngày làm việc
        /// </summary>
        /// <param name="value">Ngày tháng cần kiểm tra</param>
        /// <returns>Trả lại true nếu là ngày làm việc. Trả lại false nếu là ngày nghỉ</returns>
        public bool IsWorkingDay(DateTime value)
        {
            return Client.IsWorkingDay(value);
        }

        /// <summary>
        /// Kiểm tra chuỗi ngày tháng có phải là ngày làm việc
        /// </summary>
        /// <param name="value">Chuỗi chứa ngày tháng cần kiểm tra</param>
        /// <param name="format">Format (dạng ngày tháng) của chuỗi cần kiểm tra</param>
        /// <returns>Trả lại true nếu là ngày làm việc. Trả lại false nếu là ngày nghỉ</returns>
        public bool IsWorkingDay(string value, string format = ApplicationConstant.defaultDateTimeFormat)
        {
            //return value.StringToDate(format).IsWorkingDay();
            DateTime dateTime = value.StringToDate(format);
            return IsWorkingDay(dateTime);
        }

        /// <summary>
        /// Kiểm tra ngày tháng có phải là ngày nghỉ
        /// </summary>
        /// <param name="value">Ngày tháng cần kiểm tra</param>
        /// <returns>Trả lại true nếu là ngày nghỉ. Trả lại false nếu là làm việc</returns>
        public static bool IsNotWorkingDay(DateTime value)
        {
            //return !value.IsWorkingDay();
            return !IsNotWorkingDay(value);
        }

        /// <summary>
        /// Kiểm tra chuỗi ngày tháng có phải là ngày nghỉ
        /// </summary>
        /// <param name="value">Chuỗi chứa ngày tháng cần kiểm tra</param>
        /// <param name="format">Format (dạng ngày tháng) của chuỗi cần kiểm tra</param>
        /// <returns>Trả lại true nếu là ngày nghỉ. Trả lại false nếu là làm việc</returns>
        public static bool IsNotWorkingDay(string value, string format = ApplicationConstant.defaultDateTimeFormat)
        {
            //return value.StringToDate(format).IsNotWorkingDay();
            return !IsNotWorkingDay(value);
        }

        public List<DateTime> LayNgayHopCum(int idCum, string ngayDoiChieu)
        {
            return Client.LayNgayHopCum(idCum, ngayDoiChieu).ToList();
        }

        public List<DateTime> LayNgayHopCum(string maCum, string maDvi, string ngayDoiChieu)
        {
            return Client.LayNgayHopCumTheoMa(maCum, maDvi, ngayDoiChieu).ToList();
        }

        public bool LockData(DatabaseConstant.Module module,
            DatabaseConstant.Function function,
            DatabaseConstant.Table table,
            DatabaseConstant.Action action,
            List<int> listLockId)
        {
            // Truongnx: test performance
            //return true;

            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.UtilitiesService.layGiaTri());

            //Khởi tạo và gán các giá trị cho request
            LockRequest request = Common.Utilities.PrepareRequest(new LockRequest());
            LockResponse response = new LockResponse();

            request.Module = module;
            request.Function = function;
            request.Table = table;
            request.Action = action;
            request.ListLockId = listLockId.ToArray();

            // Lấy kết quả trả về
            response = Client.LockData(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            //return response.ListLockedId.ToList();
            if (response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                return true;
            else
            {
                if (ClientInformation.ClientType.Equals(ApplicationConstant.ClientType.DESKTOP.layGiaTri()))
                    LMessage.ShowMessage("M.ResponseMessage.Common.LockDataInvalid", LMessage.MessageBoxType.Information);
                return false;
            }
                
        }

        public bool UnlockData(DatabaseConstant.Module module,
            DatabaseConstant.Function function,
            DatabaseConstant.Table table,
            DatabaseConstant.Action action,
            List<int> listLockedId)
        {
            // Truongnx: test performance
            //return true;

            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.UtilitiesService.layGiaTri());

            //Khởi tạo và gán các giá trị cho request
            UnlockRequest request = Common.Utilities.PrepareRequest(new UnlockRequest());
            UnlockResponse response = new UnlockResponse();

            request.Module = module;
            request.Function = function;
            request.Table = table;
            request.Action = action;
            request.ListLockId = listLockedId.ToArray();

            // Lấy kết quả trả về
            response = Client.UnlockData(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            //return response.ListUnlockedId.ToList();
            if (response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                return true;
            else
                return false;
        }

        public bool UnlockDataFromFunctionByUser(DatabaseConstant.Module module,
            DatabaseConstant.Function function)
        {
            // Truongnx: test performance
            //return true;

            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.UtilitiesService.layGiaTri());

            //Khởi tạo và gán các giá trị cho request
            UnlockRequest request = Common.Utilities.PrepareRequest(new UnlockRequest());
            UnlockResponse response = new UnlockResponse();

            request.Module = module;
            request.Function = function;

            // Lấy kết quả trả về
            response = Client.UnlockDataFromFunctionByUser(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            //return response.ListUnlockedId.ToList();
            if (response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                return true;
            else
                return false;
        }

        public bool UnlockDataFromUser()
        {
            // Truongnx: test performance
            // return true;

            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.UtilitiesService.layGiaTri());

            //Khởi tạo và gán các giá trị cho request
            UnlockRequest request = Common.Utilities.PrepareRequest(new UnlockRequest());
            UnlockResponse response = new UnlockResponse();

            // Lấy kết quả trả về
            response = Client.UnlockDataFromUser(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            //return response.ListUnlockedId.ToList();
            if (response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                return true;
            else
                return false;
        }

        public int CheckDuplicateData(string tenBang,
            string tenTruong,
            string giaTri)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.UtilitiesService.layGiaTri());

            //Khởi tạo và gán các giá trị cho request
            CheckDuplicateRequest request = Common.Utilities.PrepareRequest(new CheckDuplicateRequest());
            CheckDuplicateResponse response = new CheckDuplicateResponse();

            request.TenBang = tenBang;
            request.TenTruong = tenTruong;
            request.GiaTri = giaTri;

            // Lấy kết quả trả về
            response = Client.CheckDuplicateData(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.KetQua;
        }

        public string LayGiaTriThamSo(BusinessConstant.LoaiThamSo loaiThamSo,
            BusinessConstant.MaThamSo maThamSo,
            string maDonVi)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.UtilitiesService.layGiaTri());

            //Khởi tạo và gán các giá trị cho request
            ThamSoRequest request = Common.Utilities.PrepareRequest(new ThamSoRequest());
            ThamSoResponse response = new ThamSoResponse();

            request.LoaiThamSo = loaiThamSo;
            request.MaThamSo = maThamSo;
            request.MaDonVi = maDonVi;

            // Lấy kết quả trả về
            response = Client.LayGiaTriThamSo(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.GiaTriThamSo;
        }

        public HT_TSO LayThamSo(BusinessConstant.LoaiThamSo loaiThamSo,
            BusinessConstant.MaThamSo maThamSo,
            string maDonVi)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.UtilitiesService.layGiaTri());

            //Khởi tạo và gán các giá trị cho request
            ThamSoRequest request = Common.Utilities.PrepareRequest(new ThamSoRequest());
            ThamSoResponse response = new ThamSoResponse();

            request.LoaiThamSo = loaiThamSo;
            request.MaThamSo = maThamSo;
            request.MaDonVi = maDonVi;

            // Lấy kết quả trả về
            response = Client.LayThamSo(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.objThamSo;
        }

        public bool Utilites(DatabaseConstant.Action action,
            ApplicationConstant.FormatType type,
            string value,
            string objType,
            ref ApplicationConstant.UtilitesResponseMessage responseMessage,
            ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.UtilitiesService.layGiaTri());

            //Khởi tạo và gán các giá trị cho request
            UtilitiesRequest request = Common.Utilities.PrepareRequest(new UtilitiesRequest());
            UtilitiesResponse response = new UtilitiesResponse();

            request.Action = action;
            request.type = type;
            request.value = value;
            request.objType = objType;

            // Lấy kết quả trả về
            response = Client.Utilites(request);

            //Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                responseMessage = response.utilitesResponseMessage;
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
