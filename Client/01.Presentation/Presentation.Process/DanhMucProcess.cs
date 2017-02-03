using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Presentation.Process.TruyVanServiceRef;
using Utilities.Common;
using System.Data;
using Presentation.Process.DanhMucServiceRef;
using System.Drawing;
using System.IO;
using Presentation.Process.Common;
using Presentation.Process.TyGiaServiceRef;
using System.ServiceModel.Description;

namespace Presentation.Process
{
    public class DanhMucProcess
    {
        private static TruyVanServiceClient ClientTruyVan { get; set; }
        private static DanhMucServiceClient ClientDanhMuc { get; set; }

        static DanhMucProcess()
        {
            //ClientTruyVan = new TruyVanServiceClient();
            //ClientDanhMuc = new DanhMucServiceClient();
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

            EndpointAddress endpointAddressDanhMuc = Common.Utilities.getEndpointAddress(ApplicationConstant.SystemService.DanhMucService.layGiaTri());
            BasicHttpBinding basicHttpBindingDanhMuc = Common.Utilities.getBasicHttpBinding(ApplicationConstant.SystemService.DanhMucService.layGiaTri());
            ClientDanhMuc = new DanhMucServiceClient(basicHttpBindingDanhMuc, endpointAddressDanhMuc);

            foreach (var operationDescription in ClientDanhMuc.Endpoint.Contract.Operations)
            {
                DataContractSerializerOperationBehavior dcsob =
                    operationDescription.Behaviors.Find<DataContractSerializerOperationBehavior>();
                if (dcsob != null)
                {
                    dcsob.MaxItemsInObjectGraph = 2147483646;
                }
            }
        }

        #region Danh mục đơn vị DM_DON_VI
        /// <summary>
        /// Lấy toàn bộ danh sách đơn vị
        /// </summary>
        /// <returns>DataSet chứa danh sách đơn vị</returns>
        public DataSet getDanhSachDonVi()
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "1", "1", "1");

            request.dtThamSo = dt;
            request.inquiryName = "GET_DS_DONVI";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        /// <summary>
        /// Lấy thông tin đơn vị theo id đơn vị truyền vào
        /// </summary>
        /// <param name="maDonVi">id</param>
        /// <returns></returns>
        public DataSet getDonViTheoID(string maDonVi)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            //Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());
            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@ID", "INT", maDonVi);

            request.dtThamSo = dt;
            request.inquiryName = "GET_DONVI_THEO_ID";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet getThongTinTaiKhoanDonVi(string idDonVi)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@ID_DVI", "String", idDonVi);

            request.dtThamSo = dt;
            request.inquiryName = "TKHOAN";
            request.objectName = "INQ.CT.DONVI";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        /// <summary>
        /// Lấy thông tin đơn vị theo id đơn vị truyền vào
        /// </summary>
        /// <param name="maDonVi">id</param>
        /// <returns></returns>
        public DM_DON_VI getDonViById(int idDonVi)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            //Khởi tạo và gán các giá trị cho request
            DMDonViRequest request = Common.Utilities.PrepareRequest(new DMDonViRequest());
            DMDonViResponse response = new DMDonViResponse();

            request.Id = idDonVi;

            // Lấy kết quả trả về
            response = ClientDanhMuc.GetThongTinDonVi(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.obj;
        }

        /// <summary>
        /// Lấy thông tin đơn vị theo mã đơn vị truyền vào
        /// </summary>
        /// <param name="maDonVi">mã đơn vị</param>
        /// <returns></returns>
        public DataSet getDonViTheoMa(string maDonVi)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            //Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());
            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@MA_DVI", "STRING", maDonVi);

            request.dtThamSo = dt;
            request.inquiryName = "GET_DONVI_THEO_MA";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet getDonViTheoListMa(string listDonVi)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            //Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());
            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@MA_DVI", "STRING", listDonVi);

            request.dtThamSo = dt;
            request.inquiryName = "GET_DS_DONVI_LIST_MA";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        /// <summary>
        /// Lấy thông tin đơn vị theo mã đơn vị truyền vào
        /// </summary>
        /// <param name="maDonVi">mã đơn vị</param>
        /// <returns></returns>
        public DataSet getDonViCha(string id)
        {
            DataSet ds = null;

            if (id != "0")
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

                //Khởi tạo và gán các giá trị cho request
                TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());
                TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
                DataTable dt = null;

                LDatatable.MakeParameterTable(ref dt);
                LDatatable.AddParameter(ref dt, "@ID", "INT", id);

                request.dtThamSo = dt;
                request.inquiryName = "GET_DONVI_CHA";

                // Lấy kết quả trả về
                response = ClientTruyVan.TruyVanMessage(request);

                // Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                ds = response.dsResult;
            }
            else
            {
                ds = getDanhSachDonVi();
                DataRow dr = ds.Tables[0].NewRow();
                ds.Tables[0].Rows.Add(dr);
            }
            return ds;
        }

        public DM_DON_VI getDonViByMaCum(string maCum)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            //Khởi tạo và gán các giá trị cho request
            DMDonViRequest request = Common.Utilities.PrepareRequest(new DMDonViRequest());
            DMDonViResponse response = new DMDonViResponse();

            request.MaCum = maCum;

            // Lấy kết quả trả về
            response = ClientDanhMuc.GetDonViByMaCum(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.obj;
        }

        public ApplicationConstant.ResponseStatus GetDonViChaByMaDonVi(string ma, ref DM_DON_VI dmDonVi, ref HT_NGAY_LVIEC ngayLamViec, ref string responseMessage)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            //Khởi tạo và gán các giá trị cho request
            DMDonViRequest request = Common.Utilities.PrepareRequest(new DMDonViRequest());
            DMDonViResponse response = new DMDonViResponse();

            request.Ma = ma;

            // Lấy kết quả trả về
            response = ClientDanhMuc.GetDonViChaByMaDonVi(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            if (response == null)
            {
                return ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
            }
            else
            {
                responseMessage = response.ResponseMessage;
                dmDonVi = response.obj;
                ngayLamViec = response.NgayLamViec;
                return response.ResponseStatus;
            }
        }

        public ApplicationConstant.ResponseStatus ThemDonVi(ref DanhMucServiceRef.DM_DON_VI obj, byte[] imageData, string imageName, ref string responseMessage)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new DMDonViRequest());

            request.Module = DatabaseConstant.Module.DMDC;
            request.Function = DatabaseConstant.Function.DC_DM_DON_VI;
            request.Action = DatabaseConstant.Action.THEM;

            // Lấy thông tin ảnh
            ImageBase image = new ImageBase();
            image.ImageData = imageData;
            image.ImageName = imageName;
            request.DonViLogo = image;

            request.obj = obj;
            // Lấy kết quả trả về
            DMDonViResponse response = ClientDanhMuc.LuuDonVi(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            if (response == null)
            {
                return ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
            }
            else
            {
                responseMessage = response.ResponseMessage;
                obj = response.obj;
                return response.ResponseStatus;
            }
        }

        public ApplicationConstant.ResponseStatus ThemDonVi(ref DanhMucServiceRef.DM_DON_VI obj, ref List<DM_DON_VI_TKHOAN> lstTaiKhoan, byte[] imageData, string imageName, ref string responseMessage)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new DMDonViRequest());

            request.Module = DatabaseConstant.Module.DMDC;
            request.Function = DatabaseConstant.Function.DC_DM_DON_VI;
            request.Action = DatabaseConstant.Action.THEM;

            // Lấy thông tin ảnh
            ImageBase image = new ImageBase();
            image.ImageData = imageData;
            image.ImageName = imageName;
            request.DonViLogo = image;

            request.obj = obj;
            request.listTaiKhoan = lstTaiKhoan.ToArray();
            // Lấy kết quả trả về
            DMDonViResponse response = ClientDanhMuc.LuuDonVi(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            if (response == null)
            {
                return ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
            }
            else
            {
                responseMessage = response.ResponseMessage;
                obj = response.obj;
                return response.ResponseStatus;
            }
        }

        /// <summary>
        /// Kiểm tra mã đơn vị đã được sử dụng chưa
        /// </summary>
        /// <param name="maDonVi">mã đơn vị</param>
        /// <returns>
        ///         True: nếu đã sử dụng
        ///         False: nếu chưa sử dụng
        /// </returns>
        public bool checkMaDonViDaSuDung(string maDonVi)
        {
            DataSet ds = null;
            ds = getDonViTheoMa(maDonVi);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Xóa đơn vị theo list mã đơn vị
        /// </summary>
        /// <param name="maDonVi"></param>
        /// <returns></returns>
        public bool XoaDonVi(int[] listID,
            ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            DMDonViRequest request = Common.Utilities.PrepareRequest(new DMDonViRequest());
            request.listID = listID;

            request.Module = DatabaseConstant.Module.DMDC;
            request.Function = DatabaseConstant.Function.DC_DM_DON_VI;
            request.Action = DatabaseConstant.Action.XOA;

            DMDonViResponse response = ClientDanhMuc.XoaListDonViTheoID(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Xử lý dữ liệu trả về
            if (response == null)
            {
                return false;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return response.KetQua;                
            }
        }

        public ApplicationConstant.ResponseStatus SuaDonVi(ref DanhMucServiceRef.DM_DON_VI obj, byte[] imageData, string imageName, ref string responseMessage)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            DMDonViRequest request = Common.Utilities.PrepareRequest(new DMDonViRequest());

            request.Module = DatabaseConstant.Module.DMDC;
            request.Function = DatabaseConstant.Function.DC_DM_DON_VI;
            request.Action = DatabaseConstant.Action.SUA;

            // Lấy thông tin ảnh
            ImageBase image = new ImageBase();
            image.ImageData = imageData;
            image.ImageName = imageName;
            request.DonViLogo = image;

            request.obj = obj;
            request.Action = DatabaseConstant.Action.SUA;

            DMDonViResponse response = ClientDanhMuc.SuaThongTinDonVi(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Xử lý dữ liệu trả về
            if (response == null)
            {
                return ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
            }
            else
            {
                responseMessage = response.ResponseMessage;
                obj = response.obj;
                return response.ResponseStatus;
            }
        }

        public ApplicationConstant.ResponseStatus SuaDonVi(ref DanhMucServiceRef.DM_DON_VI obj, ref List<DM_DON_VI_TKHOAN> lstTaiKhoan, byte[] imageData, string imageName, ref string responseMessage)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            DMDonViRequest request = Common.Utilities.PrepareRequest(new DMDonViRequest());

            request.Module = DatabaseConstant.Module.DMDC;
            request.Function = DatabaseConstant.Function.DC_DM_DON_VI;
            request.Action = DatabaseConstant.Action.SUA;

            // Lấy thông tin ảnh
            ImageBase image = new ImageBase();
            image.ImageData = imageData;
            image.ImageName = imageName;
            request.DonViLogo = image;

            request.obj = obj;
            request.listTaiKhoan = lstTaiKhoan.ToArray();
            request.Action = DatabaseConstant.Action.SUA;

            DMDonViResponse response = ClientDanhMuc.SuaThongTinDonVi(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Xử lý dữ liệu trả về
            if (response == null)
            {
                return ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
            }
            else
            {
                responseMessage = response.ResponseMessage;
                obj = response.obj;
                return response.ResponseStatus;
            }
        }

        public bool DuyetDonVi(int[] listID,
            ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            DMDonViRequest request = Common.Utilities.PrepareRequest(new DMDonViRequest());
            request.listID = listID;

            request.Module = DatabaseConstant.Module.DMDC;
            request.Function = DatabaseConstant.Function.DC_DM_DON_VI;
            request.Action = DatabaseConstant.Action.DUYET;

            DMDonViResponse response = ClientDanhMuc.SuaThongTinDonVi(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Xử lý dữ liệu trả về
            if (response == null)
            {
                return false;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return response.KetQua;
            }
        }

        public bool ThoaiDuyetDonVi(int[] listID,
            ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            DMDonViRequest request = Common.Utilities.PrepareRequest(new DMDonViRequest());
            request.listID = listID;

            request.Module = DatabaseConstant.Module.DMDC;
            request.Function = DatabaseConstant.Function.DC_DM_DON_VI;
            request.Action = DatabaseConstant.Action.THOAI_DUYET;

            DMDonViResponse response = ClientDanhMuc.SuaThongTinDonVi(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Xử lý dữ liệu trả về
            if (response == null)
            {
                return false;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return response.KetQua;
            }
        }

        public bool TuChoiDonVi(int[] listID,
            ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            DMDonViRequest request = Common.Utilities.PrepareRequest(new DMDonViRequest());
            request.listID = listID;

            request.Module = DatabaseConstant.Module.DMDC;
            request.Function = DatabaseConstant.Function.DC_DM_DON_VI;
            request.Action = DatabaseConstant.Action.TU_CHOI_DUYET;

            DMDonViResponse response = ClientDanhMuc.SuaThongTinDonVi(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Xử lý dữ liệu trả về
            if (response == null)
            {
                return false;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return response.KetQua;
            }
        }

        /// <summary>
        /// Lấy ảnh từ server
        /// </summary>
        /// <param name="imageName"></param>
        /// <returns></returns>
        public byte[] LayAnhTuSever(string imageName)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());
            // Khởi tạo và gán các giá trị cho request
            DMDonViRequest objRequest = Common.Utilities.PrepareRequest(new DMDonViRequest());
            objRequest.DonViLogo = new Presentation.Process.DanhMucServiceRef.ImageBase();
            objRequest.DonViLogo.ImageName = imageName;
            DMDonViResponse response = ClientDanhMuc.LoadImageFormFile(objRequest);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(objRequest, response);
            if(response == null)
            {
                return null;
            }
            else
            {
                return response.DonViLogo.ImageData;
            }
        }

        #endregion

        #region Danh mục cụm

        /// <summary>
        /// Lấy toàn bộ danh sách cụm
        /// </summary>
        /// <returns>DataSet chứa danh sách cụm</returns>
        public DataSet getDanhSachCum()
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "", "", "");

            request.dtThamSo = dt;
            request.inquiryName = DatabaseConstant.DanhSachTruyVan.DM_CUM.getValue();

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet getDanhSachCum01(DataTable dt)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            
            request.dtThamSo = dt;
            request.objectName = "INQ.DS.CUM";
            request.inquiryName = "DANH_SACH_01";


            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet getDanhSachCum02(DataTable dt)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            

            request.dtThamSo = dt;
            request.objectName = "INQ.DS.CUM";
            request.inquiryName = "DANH_SACH_02";


            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet getTreeCum_01(DataTable dt)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            
            request.dtThamSo = dt;
            request.objectName = "INQ.DS.CUM";
            request.inquiryName = "TREE_01";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet getTreeCum_02(DataTable dt)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            
            request.dtThamSo = dt;
            request.objectName = "INQ.DS.CUM";
            request.inquiryName = "TREE_02";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet getCumTheoMa(string maCum)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            //Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());
            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@MA_CUM", "STRING", maCum);

            request.dtThamSo = dt;
            request.inquiryName = "GET_CUM_THEO_MA";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        /// <summary>
        /// Lấy thông tin cụm theo id
        /// </summary>
        public ApplicationConstant.ResponseStatus getCumById(int id, ref DM_CUM obj, ref string responseMessage)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            //Khởi tạo và gán các giá trị cho request
            DMCumRequest request = Common.Utilities.PrepareRequest(new DMCumRequest());
            DMCumResponse response = new DMCumResponse();

            request.Id = id;

            // Lấy kết quả trả về
            response = ClientDanhMuc.GetThongTinCum(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Lấy kết quả trả về
            if (response == null)
            {
                return ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
            }
            else
            {
                responseMessage = response.ResponseMessage;
                obj = response.obj;
                return response.ResponseStatus;
            }
        }

        /// <summary>
        /// Lấy toàn bộ danh sách khu vực
        /// </summary>
        /// <returns>DataSet chứa danh sách khu vực</returns>
        public DataSet getThongTinCTietCum(string idCum)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@ID_CUM", "String", idCum);

            request.dtThamSo = dt;
            request.inquiryName = "%";
            request.objectName = "INQ.CT.CUM_CT";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public ApplicationConstant.ResponseStatus ThemCum(ref DM_CUM obj, ref DC_TSUAT_CUM objTSuatCum, ref string responseMessage, List<DanhMucServiceRef.DM_CUM_NS> lstNhanSu = null)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new DMCumRequest());
            request.obj = obj;
            request.objTanSuatCum = objTSuatCum;
            if (!LObject.IsNullOrEmpty(lstNhanSu))
                request.lstNhanSu = lstNhanSu.ToArray();

            request.Module = DatabaseConstant.Module.DMDC;
            request.Function = DatabaseConstant.Function.DC_DM_CUM;
            request.Action = DatabaseConstant.Action.THEM;

            // Lấy kết quả trả về
            DMCumResponse response = ClientDanhMuc.LuuCum(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Lấy kết quả trả về
            if (response == null)
            {
                return ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
            }
            else
            {
                responseMessage = response.ResponseMessage;
                obj = response.obj;
                objTSuatCum = response.objTanSuatCum;
                return response.ResponseStatus;
            }
        }

        public ApplicationConstant.ResponseStatus SuaCum(ref DM_CUM obj, ref DC_TSUAT_CUM objTSuatCum, ref string responseMessage, List<DanhMucServiceRef.DM_CUM_NS> lstNhanSu = null)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            DMCumRequest request = Common.Utilities.PrepareRequest(new DMCumRequest());
            request.obj = obj;
            request.objTanSuatCum = objTSuatCum;
            if (!LObject.IsNullOrEmpty(lstNhanSu))
                request.lstNhanSu = lstNhanSu.ToArray();

            request.Module = DatabaseConstant.Module.DMDC;
            request.Function = DatabaseConstant.Function.DC_DM_CUM;
            request.Action = DatabaseConstant.Action.SUA;

            DMCumResponse response = ClientDanhMuc.SuaThongTinCum(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Lấy kết quả trả về
            if (response == null)
            {
                return ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
            }
            else
            {
                responseMessage = response.ResponseMessage;
                obj = response.obj;
                objTSuatCum = response.objTanSuatCum;
                return response.ResponseStatus;
            }
        }

        public bool XoaCum(int[] listID,
            ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            DMCumRequest request = Common.Utilities.PrepareRequest(new DMCumRequest());
            request.listID = listID;

            request.Module = DatabaseConstant.Module.DMDC;
            request.Function = DatabaseConstant.Function.DC_DM_CUM;
            request.Action = DatabaseConstant.Action.XOA;

            DMCumResponse response = ClientDanhMuc.XoaListCumTheoID(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Xử lý dữ liệu trả về
            if (response == null)
            {
                return false;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return response.KetQua;
            }
        }

        public bool DuyetCum(int[] listID,
            ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            DMCumRequest request = Common.Utilities.PrepareRequest(new DMCumRequest());
            request.listID = listID.ToArray();

            request.Module = DatabaseConstant.Module.DMDC;
            request.Function = DatabaseConstant.Function.DC_DM_CUM;
            request.Action = DatabaseConstant.Action.DUYET;

            DMCumResponse response = ClientDanhMuc.SuaThongTinCum(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Xử lý dữ liệu trả về
            if (response == null)
            {
                return false;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return response.KetQua;
            }
        }

        public bool ThoaiDuyetCum(int[] listID,
            ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            DMCumRequest request = Common.Utilities.PrepareRequest(new DMCumRequest());
            request.listID = listID.ToArray();

            request.Module = DatabaseConstant.Module.DMDC;
            request.Function = DatabaseConstant.Function.DC_DM_CUM;
            request.Action = DatabaseConstant.Action.THOAI_DUYET;

            DMCumResponse response = ClientDanhMuc.SuaThongTinCum(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Xử lý dữ liệu trả về
            if (response == null)
            {
                return false;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return response.KetQua;
            }
        }

        public bool TuChoiCum(int[] listID,
            ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            DMCumRequest request = Common.Utilities.PrepareRequest(new DMCumRequest());
            request.listID = listID.ToArray();

            request.Module = DatabaseConstant.Module.DMDC;
            request.Function = DatabaseConstant.Function.DC_DM_CUM;
            request.Action = DatabaseConstant.Action.TU_CHOI_DUYET;

            DMCumResponse response = ClientDanhMuc.SuaThongTinCum(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Xử lý dữ liệu trả về
            if (response == null)
            {
                return false;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return response.KetQua;
            }
        }

        public bool Cum02(DatabaseConstant.Action action, ref DM_CUM obj, ref DM_TEMP_CUM objTempCum, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            DMCumRequest request = null;
            DMCumResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.KhachHangService.layGiaTri());

                request = Common.Utilities.PrepareRequest(new DanhMucServiceRef.DMCumRequest());
                response = new DanhMucServiceRef.DMCumResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.DC_DM_CUM;
                request.Action = action;
                request.obj = obj;
                request.objTempCum = objTempCum;

                // make a call to service client here
                response = ClientDanhMuc.Cum02(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    obj = response.obj;
                    objTempCum = response.objTempCum;
                    listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                    return true;
                }
                else
                {
                    listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                    return false;
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                request = null;
                response = null;
            }
        }

        public bool DanhSachCum02(DatabaseConstant.Action action, ref List<DM_CUM> lstCum, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            DMCumRequest request = null;
            DMCumResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.KhachHangService.layGiaTri());

                request = Common.Utilities.PrepareRequest(new DanhMucServiceRef.DMCumRequest());
                response = new DanhMucServiceRef.DMCumResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.DC_DM_CUM_DS;
                request.Action = action;
                if (lstCum.Count > 0)
                    request.lstCum = lstCum.ToArray();
                request.lstCum = lstCum.ToArray();

                // make a call to service client here
                response = ClientDanhMuc.Cum02(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    lstCum = response.lstCum.ToList();
                    listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                    return true;
                }
                else
                {
                    listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                    return false;
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                request = null;
                response = null;
            }
        }

        public DataSet GetDanhSachCum02(string maDonVi, string trangThaiNVu)
        {
            TruyVanServiceClient client = null;
            TruyVanRequest request = null;
            TruyVanResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

                client = TruyVanServiceClient(ApplicationConstant.SystemService.TruyVanService);
                request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());
                response = new TruyVanServiceRef.TruyVanResponse();

                //Khởi tạo request
                DataTable dt = null;
                LDatatable.MakeParameterTable(ref dt);
                LDatatable.AddParameter(ref dt, "@TRANG_THAI_NVU", "STRING", trangThaiNVu);
                LDatatable.AddParameter(ref dt, "@MA_DVI", "STRING", maDonVi);

                request.dtThamSo = dt;
                request.objectName = "INQ.DS.DS_CUM";
                request.inquiryName = "DANH_SACH_02";

                // make a call to service client here
                response = client.TruyVanMessage(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                DataSet ds = response.dsResult;
                return ds;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                if (client.State == CommunicationState.Faulted)
                {
                    client.Abort();
                }
                else
                {
                    client.Close();
                }

                client = null;
                request = null;
                response = null;
            }
        }

        #endregion

        #region Danh mục địa bàn

        /// <summary>
        /// Lấy toàn bộ danh sách địa bàn
        /// </summary>
        /// <returns>DataSet chứa danh sách địa bạn</returns>
        public DataSet getDanhSachDiaBan()
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "", "", "");

            request.dtThamSo = dt;
            request.inquiryName = DatabaseConstant.DanhSachTruyVan.DM_DIABAN.getValue();

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        /// <summary>
        /// Lấy thông tin địa bàn theo id
        /// </summary>
        public DM_DIA_BAN getDiaBanById(int id)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            //Khởi tạo và gán các giá trị cho request
            DMDiaBanRequest request = Common.Utilities.PrepareRequest(new DMDiaBanRequest());
            DMDiaBanResponse response = new DMDiaBanResponse();

            request.Id = id;

            // Lấy kết quả trả về
            response = ClientDanhMuc.GetThongTinDiaBan(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.obj;
        }

        /// <summary>
        /// Lấy thông tin địa bàn theo id
        /// </summary>
        public DMDiaBanResponse getDiaBanById01(int id)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            //Khởi tạo và gán các giá trị cho request
            DMDiaBanRequest request = Common.Utilities.PrepareRequest(new DMDiaBanRequest());
            DMDiaBanResponse response = new DMDiaBanResponse();

            request.Id = id;

            // Lấy kết quả trả về
            response = ClientDanhMuc.GetThongTinDiaBan01(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response;
        }

        public DM_DIA_BAN ThemDiaBan(DanhMucServiceRef.DM_DIA_BAN obj)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new DMDiaBanRequest());
            request.obj = obj;

            request.Module = DatabaseConstant.Module.DMDC;
            request.Function = DatabaseConstant.Function.DC_DM_DIA_BAN;
            request.Action = DatabaseConstant.Action.THEM;

            // Lấy kết quả trả về
            DMDiaBanResponse response = ClientDanhMuc.LuuDiaBan(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            if (response == null)
            {
                return null;
            }
            else
            {
                return response.obj;
            }
        }

        public DM_DIA_BAN ThemDiaBan01(DanhMucServiceRef.DM_DIA_BAN obj, VDM_DBAN_TTIN_KHAC objTTKhac)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new DMDiaBanRequest());
            request.obj = obj;
            request.objTTKhac = objTTKhac;

            request.Module = DatabaseConstant.Module.DMDC;
            request.Function = DatabaseConstant.Function.DC_DM_DIA_BAN;
            request.Action = DatabaseConstant.Action.THEM;

            // Lấy kết quả trả về
            DMDiaBanResponse response = ClientDanhMuc.LuuDiaBan01(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            if (response == null)
            {
                return null;
            }
            else
            {
                return response.obj;
            }
        }

        public DM_DIA_BAN SuaDiaBan(DanhMucServiceRef.DM_DIA_BAN obj)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            DMDiaBanRequest request = Common.Utilities.PrepareRequest(new DMDiaBanRequest());
            request.obj = obj;

            request.Module = DatabaseConstant.Module.DMDC;
            request.Function = DatabaseConstant.Function.DC_DM_DIA_BAN;
            request.Action = DatabaseConstant.Action.SUA;

            DMDiaBanResponse response = ClientDanhMuc.SuaThongTinDiaBan(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Xử lý dữ liệu trả về
            if (response == null)
            {
                return null;
            }
            else
            {
                return response.obj;
            }
        }

        public DM_DIA_BAN SuaDiaBan01(DanhMucServiceRef.DM_DIA_BAN obj, VDM_DBAN_TTIN_KHAC objTTKhac)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            DMDiaBanRequest request = Common.Utilities.PrepareRequest(new DMDiaBanRequest());
            request.obj = obj;
            request.objTTKhac = objTTKhac;

            request.Module = DatabaseConstant.Module.DMDC;
            request.Function = DatabaseConstant.Function.DC_DM_DIA_BAN;
            request.Action = DatabaseConstant.Action.SUA;

            DMDiaBanResponse response = ClientDanhMuc.SuaThongTinDiaBan01(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Xử lý dữ liệu trả về
            if (response == null)
            {
                return null;
            }
            else
            {
                return response.obj;
            }
        }

        public bool XoaDiaBan(int[] listID,
            ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            DMDiaBanRequest request = Common.Utilities.PrepareRequest(new DMDiaBanRequest());
            request.listID = listID;

            request.Module = DatabaseConstant.Module.DMDC;
            request.Function = DatabaseConstant.Function.DC_DM_DIA_BAN;
            request.Action = DatabaseConstant.Action.XOA;

            DMDiaBanResponse response = ClientDanhMuc.XoaListDiaBanTheoID(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Xử lý dữ liệu trả về
            if (response == null)
            {
                return false;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return response.bKetQua;
            }
        }

        public bool XoaDiaBan01(int[] listID,
            ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            DMDiaBanRequest request = Common.Utilities.PrepareRequest(new DMDiaBanRequest());
            request.listID = listID;

            request.Module = DatabaseConstant.Module.DMDC;
            request.Function = DatabaseConstant.Function.DC_DM_DIA_BAN;
            request.Action = DatabaseConstant.Action.XOA;

            DMDiaBanResponse response = ClientDanhMuc.XoaListDiaBanTheoID01(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Xử lý dữ liệu trả về
            if (response == null)
            {
                return false;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return response.bKetQua;
            }
        }

        public bool DuyetDiaBan(int[] listID,
            ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            DMDiaBanRequest request = Common.Utilities.PrepareRequest(new DMDiaBanRequest());
            request.listID = listID.ToArray();

            request.Module = DatabaseConstant.Module.DMDC;
            request.Function = DatabaseConstant.Function.DC_DM_DIA_BAN;
            request.Action = DatabaseConstant.Action.DUYET;

            DMDiaBanResponse response = ClientDanhMuc.SuaThongTinDiaBan(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Xử lý dữ liệu trả về
            if (response == null)
            {
                return false;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return response.bKetQua;
            }
        }

        public bool ThoaiDuyetDiaBan(int[] listID,
            ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            DMDiaBanRequest request = Common.Utilities.PrepareRequest(new DMDiaBanRequest());
            request.listID = listID.ToArray();

            request.Module = DatabaseConstant.Module.DMDC;
            request.Function = DatabaseConstant.Function.DC_DM_DIA_BAN;
            request.Action = DatabaseConstant.Action.THOAI_DUYET;

            DMDiaBanResponse response = ClientDanhMuc.SuaThongTinDiaBan(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Xử lý dữ liệu trả về
            if (response == null)
            {
                return false;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return response.bKetQua;
            }
        }

        public bool TuChoiDiaBan(int[] listID,
            ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            DMDiaBanRequest request = Common.Utilities.PrepareRequest(new DMDiaBanRequest());
            request.listID = listID.ToArray();

            request.Module = DatabaseConstant.Module.DMDC;
            request.Function = DatabaseConstant.Function.DC_DM_DIA_BAN;
            request.Action = DatabaseConstant.Action.TU_CHOI_DUYET;

            DMDiaBanResponse response = ClientDanhMuc.SuaThongTinDiaBan(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Xử lý dữ liệu trả về
            if (response == null)
            {
                return false;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return response.bKetQua;
            }
        }

        #endregion

        #region Danh mục đối tượng

        /// <summary>
        /// Lấy toàn bộ danh sách đối tượng
        /// </summary>
        /// <returns>List chứa danh sách đối tượng</returns>
        public List<DM_DTUONG> getDanhSachDoiTuong(string maDonVi)
        {
            TruyVanServiceClient client = null;
            TruyVanRequest request = null;
            TruyVanResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

                client = TruyVanServiceClient(ApplicationConstant.SystemService.TruyVanService);
                request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());
                response = new TruyVanServiceRef.TruyVanResponse();

                //Khởi tạo request
                DataTable dt = null;
                LDatatable.MakeParameterTable(ref dt);
                LDatatable.AddParameter(ref dt, "@MA_DVI", "STRING", maDonVi);

                request.dtThamSo = dt;
                request.objectName = "sp_INQ.DS.DM_DOI_TUONG";
                request.inquiryName = "DOI_TUONG";

                // make a call to service client here
                response = client.TruyVanMessage(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                DataSet ds = response.dsResult;
                List<DM_DTUONG> lstDoiTuong = new List<DM_DTUONG>();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    DM_DTUONG DTuong = new DM_DTUONG();
                    if (row["ID"].ToString().Length > 0)
                        DTuong.ID = int.Parse(row["ID"].ToString());
                    if (row["ID_LOAI_DTUONG"].ToString().Length > 0)
                        DTuong.ID_LOAI_DTUONG = int.Parse(row["ID_LOAI_DTUONG"].ToString());
                    if (row["ID_TCHIEU"].ToString().Length > 0)
                        DTuong.ID_TCHIEU = int.Parse(row["ID_TCHIEU"].ToString());
                    DTuong.MA_DTUONG = row["MA_DTUONG"].ToString();
                    DTuong.MA_DVI = row["MA_DVI"].ToString();
                    DTuong.MA_DVI_QLY = row["MA_DVI_QLY"].ToString();
                    DTuong.MA_DVI_TAO = row["MA_DVI_TAO"].ToString();
                    DTuong.MA_LOAI_DTUONG = row["MA_LOAI_DTUONG"].ToString();
                    DTuong.MA_LOAI_TCHIEU = row["MA_LOAI_TCHIEU"].ToString();
                    DTuong.MA_TCHIEU = row["MA_TCHIEU"].ToString();
                    DTuong.MO_TA = row["MO_TA"].ToString();
                    DTuong.NGAY_CNHAT = row["NGAY_CNHAT"].ToString();
                    DTuong.NGAY_NHAP = row["NGAY_NHAP"].ToString();
                    DTuong.NGUOI_CNHAT = row["NGUOI_CNHAT"].ToString();
                    DTuong.NGUOI_NHAP = row["NGUOI_NHAP"].ToString();
                    DTuong.TEN_DTUONG = row["TEN_DTUONG"].ToString();
                    DTuong.TTHAI_BGHI = row["TTHAI_BGHI"].ToString();
                    DTuong.TTHAI_LY_DO = row["TTHAI_LY_DO"].ToString();
                    DTuong.TTHAI_NVU = row["TTHAI_NVU"].ToString();
                    lstDoiTuong.Add(DTuong);
                }
                return lstDoiTuong;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                if (client.State == CommunicationState.Faulted)
                {
                    client.Abort();
                }
                else
                {
                    client.Close();
                }

                client = null;
                request = null;
                response = null;
            }
        }
        
        /// <summary>
        /// Lấy thông tin địa bàn theo id
        /// </summary>
        public bool getDoiTuongById(int id, ref DM_DTUONG obj, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            //Khởi tạo và gán các giá trị cho request
            DMDoiTuongRequest request = Common.Utilities.PrepareRequest(new DMDoiTuongRequest());
            DMDoiTuongResponse response = new DMDoiTuongResponse();

            request.Id = id;

            // Lấy kết quả trả về
            response = ClientDanhMuc.GetThongTinDoiTuong(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                obj = response.obj;
                return true;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return false;
            }
        }

        public bool ThemDoiTuong(ref DM_DTUONG obj, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new DMDoiTuongRequest());
            request.obj = obj;

            request.Module = DatabaseConstant.Module.DMDC;
            //request.Function = DatabaseConstant.Function.DC_DM_DIA_BAN; lack
            request.Action = DatabaseConstant.Action.THEM;

            // Lấy kết quả trả về
            DMDoiTuongResponse response = ClientDanhMuc.LuuDoiTuong(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                obj = response.obj;
                return true;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return false;
            }
        }

        public bool SuaDoiTuong(ref DM_DTUONG obj, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            var request = Common.Utilities.PrepareRequest(new DMDoiTuongRequest());
            request.obj = obj;

            request.Module = DatabaseConstant.Module.DMDC;
            //request.Function = DatabaseConstant.Function.DC_DM_DIA_BAN; lack
            request.Action = DatabaseConstant.Action.SUA;

            DMDoiTuongResponse response = ClientDanhMuc.SuaThongTinDoiTuong(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Xử lý dữ liệu trả về
            if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                obj = response.obj;
                return true;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return false;
            }
        }

        public bool XoaDoiTuong(int[] listID, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            var request = Common.Utilities.PrepareRequest(new DMDoiTuongRequest());
            request.listID = listID;

            request.Module = DatabaseConstant.Module.DMDC;
            //request.Function = DatabaseConstant.Function.DC_DM_DIA_BAN; lack
            request.Action = DatabaseConstant.Action.XOA;

            DMDoiTuongResponse response = ClientDanhMuc.XoaListDoiTuongTheoID(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Xử lý dữ liệu trả về
            if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                return true;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return false;
            }
        }
        
        #endregion

        #region Danh mục loại đối tượng
        /// <summary>
        /// Lấy toàn bộ danh sách loại đối tượng
        /// </summary>
        /// <returns>List chứa danh sách loại đối tượng</returns>
        public List<DM_LOAI_DTUONG> getDanhSachDoiTuongLoai(string maDonVi)
        {
            TruyVanServiceClient client = null;
            TruyVanRequest request = null;
            TruyVanResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

                client = TruyVanServiceClient(ApplicationConstant.SystemService.TruyVanService);
                request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());
                response = new TruyVanServiceRef.TruyVanResponse();

                //Khởi tạo request
                DataTable dt = null;
                LDatatable.MakeParameterTable(ref dt);
                LDatatable.AddParameter(ref dt, "@MA_DVI", "STRING", maDonVi);

                request.dtThamSo = dt;
                request.objectName = "sp_INQ.DS.DM_DOI_TUONG";
                request.inquiryName = "LOAI_DOI_TUONG";

                // make a call to service client here
                response = client.TruyVanMessage(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                DataSet ds = response.dsResult;
                List<DM_LOAI_DTUONG> lstDoiTuongLoai = new List<DM_LOAI_DTUONG>();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    DM_LOAI_DTUONG objDoiTuongLoai = new DM_LOAI_DTUONG();

                    objDoiTuongLoai.ID = int.Parse(row["ID"].ToString());
                    objDoiTuongLoai.MA_DVI_QLY = row["MA_DVI_QLY"].ToString();
                    objDoiTuongLoai.MA_DVI_TAO = row["MA_DVI_TAO"].ToString();
                    objDoiTuongLoai.MA_LOAI_DTUONG = row["MA_LOAI_DTUONG"].ToString();
                    objDoiTuongLoai.MO_TA = row["MO_TA"].ToString();
                    objDoiTuongLoai.NGAY_CNHAT = row["NGAY_CNHAT"].ToString();
                    objDoiTuongLoai.NGAY_NHAP = row["NGAY_NHAP"].ToString();
                    objDoiTuongLoai.NGUOI_CNHAT = row["NGUOI_CNHAT"].ToString();
                    objDoiTuongLoai.NGUOI_NHAP = row["NGUOI_NHAP"].ToString();
                    objDoiTuongLoai.TEN_LOAI_DTUONG = row["TEN_LOAI_DTUONG"].ToString();
                    objDoiTuongLoai.TTHAI_BGHI = row["TTHAI_BGHI"].ToString();
                    objDoiTuongLoai.TTHAI_LY_DO = row["TTHAI_LY_DO"].ToString();
                    objDoiTuongLoai.TTHAI_NVU = row["TTHAI_NVU"].ToString();
                    lstDoiTuongLoai.Add(objDoiTuongLoai);
                }
                return lstDoiTuongLoai;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                if (client.State == CommunicationState.Faulted)
                {
                    client.Abort();
                }
                else
                {
                    client.Close();
                }

                client = null;
                request = null;
                response = null;
            }
        }

        /// <summary>
        /// Lấy thông tin địa bàn theo id
        /// </summary>
        public bool getDoiTuongLoaiById(int id, ref DM_LOAI_DTUONG obj, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            //Khởi tạo và gán các giá trị cho request
            DMDoiTuongLoaiRequest request = Common.Utilities.PrepareRequest(new DMDoiTuongLoaiRequest());
            DMDoiTuongLoaiResponse response = new DMDoiTuongLoaiResponse();

            request.Id = id;

            // Lấy kết quả trả về
            response = ClientDanhMuc.GetThongTinDoiTuongLoai(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                obj = response.obj;
                return true;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return false;
            }
        }

        public bool ThemDoiTuongLoai(ref DM_LOAI_DTUONG obj, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new DMDoiTuongLoaiRequest());
            request.obj = obj;

            request.Module = DatabaseConstant.Module.DMDC;
            //request.Function = DatabaseConstant.Function.DC_DM_DIA_BAN; lack
            request.Action = DatabaseConstant.Action.THEM;

            // Lấy kết quả trả về
            DMDoiTuongLoaiResponse response = ClientDanhMuc.LuuDoiTuongLoai(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                obj = response.obj;
                return true;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return false;
            }
        }

        public bool SuaDoiTuongLoai(ref DM_LOAI_DTUONG obj, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            var request = Common.Utilities.PrepareRequest(new DMDoiTuongLoaiRequest());
            request.obj = obj;

            request.Module = DatabaseConstant.Module.DMDC;
            //request.Function = DatabaseConstant.Function.DC_DM_DIA_BAN; lack
            request.Action = DatabaseConstant.Action.SUA;

            DMDoiTuongLoaiResponse response = ClientDanhMuc.SuaThongTinDoiTuongLoai(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Xử lý dữ liệu trả về
            if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                obj = response.obj;
                return true;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return false;
            }
        }

        public bool XoaDoiTuongLoai(int[] listID, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            var request = Common.Utilities.PrepareRequest(new DMDoiTuongLoaiRequest());
            request.listID = listID;

            request.Module = DatabaseConstant.Module.DMDC;
            //request.Function = DatabaseConstant.Function.DC_DM_DIA_BAN; lack
            request.Action = DatabaseConstant.Action.XOA;

            DMDoiTuongLoaiResponse response = ClientDanhMuc.XoaListDoiTuongLoaiTheoID(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Xử lý dữ liệu trả về
            if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                return true;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return false;
            }
        }
        #endregion

        #region Danh mục dùng chung

        /// <summary>
        /// Lấy toàn bộ danh sách danh mục
        /// </summary>
        /// <returns>DataSet chứa danh sách danh mục</returns>
        public DataSet getDanhSachDungChung()
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "", "", "");

            request.dtThamSo = dt;
            request.inquiryName = DatabaseConstant.DanhSachTruyVan.DM_DUNGCHUNG.getValue();

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        /// <summary>
        /// Lấy thông tin địa bàn theo id
        /// </summary>
        public DM_DMUC_GTRI getDungChungById(int id)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            //Khởi tạo và gán các giá trị cho request
            DMDungChungRequest request = Common.Utilities.PrepareRequest(new DMDungChungRequest());
            DMDungChungResponse response = new DMDungChungResponse();

            request.Id = id;

            // Lấy kết quả trả về
            response = ClientDanhMuc.GetThongTinDungChung(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.obj;
        }

        public DM_DMUC_GTRI ThemDungChung(DanhMucServiceRef.DM_DMUC_GTRI obj)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new DMDungChungRequest());
            request.obj = obj;

            request.Module = DatabaseConstant.Module.DMDC;
            request.Function = DatabaseConstant.Function.DC_DM_DUNG_CHUNG;
            request.Action = DatabaseConstant.Action.THEM;

            // Lấy kết quả trả về
            DMDungChungResponse response = ClientDanhMuc.LuuDungChung(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            if (response == null)
            {
                return null;
            }
            else
            {
                return response.obj;
            }
        }

        public DM_DMUC_GTRI SuaDungChung(DanhMucServiceRef.DM_DMUC_GTRI obj)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            DMDungChungRequest request = Common.Utilities.PrepareRequest(new DMDungChungRequest());
            request.obj = obj;

            request.Module = DatabaseConstant.Module.DMDC;
            request.Function = DatabaseConstant.Function.DC_DM_DUNG_CHUNG;
            request.Action = DatabaseConstant.Action.SUA;

            DMDungChungResponse response = ClientDanhMuc.SuaThongTinDungChung(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Xử lý dữ liệu trả về
            if (response == null)
            {
                return null;
            }
            else
            {
                return response.obj;
            }
        }

        public bool XoaDungChung(int[] listID,
            ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            DMDungChungRequest request = Common.Utilities.PrepareRequest(new DMDungChungRequest());
            request.listID = listID;

            request.Module = DatabaseConstant.Module.DMDC;
            request.Function = DatabaseConstant.Function.DC_DM_DUNG_CHUNG;
            request.Action = DatabaseConstant.Action.XOA;

            DMDungChungResponse response = ClientDanhMuc.XoaListDungChungTheoID(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Xử lý dữ liệu trả về
            if (response == null)
            {
                return false;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return response.bKetQua;
            }
        }

        public bool DuyetDungChung(int[] listID,
            ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            DMDungChungRequest request = Common.Utilities.PrepareRequest(new DMDungChungRequest());
            request.listID = listID.ToArray();

            request.Module = DatabaseConstant.Module.DMDC;
            request.Function = DatabaseConstant.Function.DC_DM_DUNG_CHUNG;
            request.Action = DatabaseConstant.Action.DUYET;

            DMDungChungResponse response = ClientDanhMuc.SuaThongTinDungChung(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Xử lý dữ liệu trả về
            if (response == null)
            {
                return false;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return response.bKetQua;
            }
        }

        public bool ThoaiDuyetDungChung(int[] listID,
            ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            DMDungChungRequest request = Common.Utilities.PrepareRequest(new DMDungChungRequest());
            request.listID = listID.ToArray();

            request.Module = DatabaseConstant.Module.DMDC;
            request.Function = DatabaseConstant.Function.DC_DM_DUNG_CHUNG;
            request.Action = DatabaseConstant.Action.THOAI_DUYET;

            DMDungChungResponse response = ClientDanhMuc.SuaThongTinDungChung(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Xử lý dữ liệu trả về
            if (response == null)
            {
                return false;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return response.bKetQua;
            }
        }

        public bool TuChoiDungChung(int[] listID,
            ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            DMDungChungRequest request = Common.Utilities.PrepareRequest(new DMDungChungRequest());
            request.listID = listID.ToArray();

            request.Module = DatabaseConstant.Module.DMDC;
            request.Function = DatabaseConstant.Function.DC_DM_DUNG_CHUNG;
            request.Action = DatabaseConstant.Action.TU_CHOI_DUYET;

            DMDungChungResponse response = ClientDanhMuc.SuaThongTinDungChung(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Xử lý dữ liệu trả về
            if (response == null)
            {
                return false;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return response.bKetQua;
            }
        }

        /// <summary>
        /// Duyệt thông tin địa bàn
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool khongSuDungDungChung(int[] listID,
            ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            DMDungChungRequest request = Common.Utilities.PrepareRequest(new DMDungChungRequest());
            request.listID = listID.ToArray();

            request.Module = DatabaseConstant.Module.DMDC;
            request.Function = DatabaseConstant.Function.DC_DM_DUNG_CHUNG;
            request.Action = DatabaseConstant.Action.TU_CHOI_DUYET;

            DMDungChungResponse response = ClientDanhMuc.SuaThongTinDungChung(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Xử lý dữ liệu trả về
            if (response == null)
            {
                return false;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return response.bKetQua;
            }
        }

        /// <summary>
        /// Duyệt thông tin địa bàn
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool suDungDungChung(int[] listID,
            ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            DMDungChungRequest request = Common.Utilities.PrepareRequest(new DMDungChungRequest());
            request.listID = listID.ToArray();

            request.Module = DatabaseConstant.Module.DMDC;
            request.Function = DatabaseConstant.Function.DC_DM_DUNG_CHUNG;
            request.Action = DatabaseConstant.Action.TU_CHOI_DUYET;

            DMDungChungResponse response = ClientDanhMuc.SuaThongTinDungChung(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Xử lý dữ liệu trả về
            if (response == null)
            {
                return false;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return response.bKetQua;
            }
        }

        /// <summary>
        /// Duyệt thông tin địa bàn
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public DM_DMUC_LOAI layDungChungLoai(DanhMucServiceRef.DM_DMUC_LOAI obj)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            DMDungChungRequest request = Common.Utilities.PrepareRequest(new DMDungChungRequest());

            request.objLoai = obj;

            DMDungChungResponse response = ClientDanhMuc.LayDungChungLoai(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Xử lý dữ liệu trả về
            if (response == null)
            {
                return null;
            }
            else
            {
                return response.objLoai;
            }
        }

        /// <summary>
        /// Lấy thông tin địa bàn theo id
        /// </summary>
        public DM_DMUC_LOAI getDungChungLoaiById(int id)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            //Khởi tạo và gán các giá trị cho request
            DMDungChungRequest request = Common.Utilities.PrepareRequest(new DMDungChungRequest());
            DMDungChungResponse response = new DMDungChungResponse();

            request.Id = id;

            // Lấy kết quả trả về
            response = ClientDanhMuc.GetThongTinDungChungLoai(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.objLoai;
        }

        public DM_DMUC_LOAI ThemDungChungLoai(DanhMucServiceRef.DM_DMUC_LOAI obj)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new DMDungChungRequest());
            request.objLoai = obj;

            request.Module = DatabaseConstant.Module.DMDC;
            request.Function = DatabaseConstant.Function.DC_DM_DUNG_CHUNG;
            request.Action = DatabaseConstant.Action.THEM;

            // Lấy kết quả trả về
            DMDungChungResponse response = ClientDanhMuc.LuuDungChungLoai(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            if (response == null)
            {
                return null;
            }
            else
            {
                return response.objLoai;
            }
        }

        public DM_DMUC_LOAI SuaDungChungLoai(DanhMucServiceRef.DM_DMUC_LOAI obj)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            DMDungChungRequest request = Common.Utilities.PrepareRequest(new DMDungChungRequest());
            request.objLoai = obj;

            request.Module = DatabaseConstant.Module.DMDC;
            request.Function = DatabaseConstant.Function.DC_DM_DUNG_CHUNG;
            request.Action = DatabaseConstant.Action.SUA;

            DMDungChungResponse response = ClientDanhMuc.SuaThongTinDungChungLoai(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Xử lý dữ liệu trả về
            if (response == null)
            {
                return null;
            }
            else
            {
                return response.objLoai;
            }
        }

        public bool XoaDungChungLoai(int[] listID,
            ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            DMDungChungRequest request = Common.Utilities.PrepareRequest(new DMDungChungRequest());
            request.listID = listID;

            request.Module = DatabaseConstant.Module.DMDC;
            request.Function = DatabaseConstant.Function.DC_DM_DUNG_CHUNG;
            request.Action = DatabaseConstant.Action.XOA;

            DMDungChungResponse response = ClientDanhMuc.XoaListDungChungLoaiTheoID(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Xử lý dữ liệu trả về
            if (response == null)
            {
                return false;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return response.bKetQua;
            }
        }

        public bool DuyetDungChungLoai(int[] listID,
            ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            DMDungChungRequest request = Common.Utilities.PrepareRequest(new DMDungChungRequest());
            request.listID = listID.ToArray();

            request.Module = DatabaseConstant.Module.DMDC;
            request.Function = DatabaseConstant.Function.DC_DM_DUNG_CHUNG;
            request.Action = DatabaseConstant.Action.DUYET;

            DMDungChungResponse response = ClientDanhMuc.SuaThongTinDungChungLoai(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Xử lý dữ liệu trả về
            if (response == null)
            {
                return false;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return response.bKetQua;
            }
        }

        public bool ThoaiDuyetDungChungLoai(int[] listID,
            ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            DMDungChungRequest request = Common.Utilities.PrepareRequest(new DMDungChungRequest());
            request.listID = listID.ToArray();

            request.Module = DatabaseConstant.Module.DMDC;
            request.Function = DatabaseConstant.Function.DC_DM_DUNG_CHUNG;
            request.Action = DatabaseConstant.Action.THOAI_DUYET;

            DMDungChungResponse response = ClientDanhMuc.SuaThongTinDungChungLoai(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Xử lý dữ liệu trả về
            if (response == null)
            {
                return false;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return response.bKetQua;
            }
        }

        public bool TuChoiDungChungLoai(int[] listID,
            ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            DMDungChungRequest request = Common.Utilities.PrepareRequest(new DMDungChungRequest());
            request.listID = listID.ToArray();

            request.Module = DatabaseConstant.Module.DMDC;
            request.Function = DatabaseConstant.Function.DC_DM_DUNG_CHUNG;
            request.Action = DatabaseConstant.Action.TU_CHOI_DUYET;

            DMDungChungResponse response = ClientDanhMuc.SuaThongTinDungChungLoai(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Xử lý dữ liệu trả về
            if (response == null)
            {
                return false;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return response.bKetQua;
            }
        }

        /// <summary>
        /// Duyệt thông tin địa bàn
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool khongSuDungDungChungLoai(List<int> listID)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            DMDungChungRequest request = Common.Utilities.PrepareRequest(new DMDungChungRequest());

            request.listIDLoai = listID.ToArray();
            request.Action = DatabaseConstant.Action.KHONG_SU_DUNG;

            DMDungChungResponse response = ClientDanhMuc.SuaThongTinDungChungLoai(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Xử lý dữ liệu trả về
            if (response == null)
            {
                return false;
            }
            else
            {
                return response.bKetQua;
            }
        }

        /// <summary>
        /// Duyệt thông tin địa bàn
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool suDungDungChungLoai(List<int> listID)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            DMDungChungRequest request = Common.Utilities.PrepareRequest(new DMDungChungRequest());

            request.listIDLoai = listID.ToArray();
            request.Action = DatabaseConstant.Action.SU_DUNG;

            DMDungChungResponse response = ClientDanhMuc.SuaThongTinDungChungLoai(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Xử lý dữ liệu trả về
            if (response == null)
            {
                return false;
            }
            else
            {
                return response.bKetQua;
            }
        }

        public DataSet GetDanhMucGTri(string loai)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@MA_DMUC_LOAI", "String", loai);

            request.dtThamSo = dt;
            request.objectName = "INQ.DS.DMUC_GTRI";
            request.inquiryName = "INQ.DS.DMUC_GTRI";


            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }
        #endregion

        #region Danh mục khu vực

        /// <summary>
        /// Lấy toàn bộ danh sách khu vực
        /// </summary>
        /// <returns>DataSet chứa danh sách khu vực</returns>
        public DataSet getDanhSachKhuVuc()
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "", "", "");

            request.dtThamSo = dt;
            request.inquiryName = DatabaseConstant.DanhSachTruyVan.DM_KHUVUC.getValue();

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet getDanhSachKhuVuc01(DataTable dt)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            

            request.dtThamSo = dt;
            request.objectName = "INQ.DS.KHU_VUC";
            request.inquiryName = "DANH_SACH_01";
            

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet getTreeKhuVuc_01(DataTable dt)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            

            request.dtThamSo = dt;
            request.objectName = "INQ.DS.KHU_VUC";
            request.inquiryName = "TREE_01";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        /// <summary>
        /// Lấy toàn bộ danh sách khu vực
        /// </summary>
        /// <returns>DataSet chứa danh sách khu vực</returns>
        public DataSet getThongTinCTietKhuVuc(string idKhuVuc)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@ID_KHU_VUC", "String", idKhuVuc);

            request.dtThamSo = dt;
            request.inquiryName = "%";
            request.objectName = "INQ.CT.KHU_VUC_CT";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        /// <summary>
        /// Lấy toàn bộ danh sách nguoi dai dien
        /// </summary>
        /// <returns>DataSet chứa danh sách khu vực</returns>
        public DataSet getThongTinNguoiDaiDien(string idKhuVuc)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@ID_KHU_VUC", "String", idKhuVuc);

            request.dtThamSo = dt;
            request.inquiryName = "NGUOI_DAI_DIEN";
            request.objectName = "INQ.CT.KHU_VUC_CT_01";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        /// <summary>
        /// Lấy thông tin địa bàn theo id
        /// </summary>
        public DM_KHU_VUC getKhuVucById(int id)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            //Khởi tạo và gán các giá trị cho request
            DMKhuVucRequest request = Common.Utilities.PrepareRequest(new DMKhuVucRequest());
            DMKhuVucResponse response = new DMKhuVucResponse();

            request.Id = id;

            // Lấy kết quả trả về
            response = ClientDanhMuc.GetThongTinKhuVuc(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.obj;
        }
        public DM_KHU_VUC getKhuVucByIdDonVi(int id)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            //Khởi tạo và gán các giá trị cho request
            DMKhuVucRequest request = Common.Utilities.PrepareRequest(new DMKhuVucRequest());
            DMKhuVucResponse response = new DMKhuVucResponse();

            request.Id = id;

            // Lấy kết quả trả về
            response = ClientDanhMuc.GetThongTinKhuVucTheoDonVi(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.obj;
        }

        public DM_KHU_VUC ThemKhuVuc(DanhMucServiceRef.DM_KHU_VUC obj, ref List<ClientResponseDetail> listClientResponseDetail,List<DanhMucServiceRef.DM_KHU_VUC_NS> lstNhanSu=null)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new DMKhuVucRequest());
            request.obj = obj;
            if (!LObject.IsNullOrEmpty(lstNhanSu))
                request.lstNhanSu = lstNhanSu.ToArray();

            request.Module = DatabaseConstant.Module.DMDC;
            request.Function = DatabaseConstant.Function.DC_DM_KHU_VUC;
            request.Action = DatabaseConstant.Action.THEM;

            // Lấy kết quả trả về
            DMKhuVucResponse response = ClientDanhMuc.KhuVuc(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            if (response == null)
            {
                return null;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return response.obj;
            }
        }

        public DM_KHU_VUC SuaKhuVuc(DanhMucServiceRef.DM_KHU_VUC obj, ref List<ClientResponseDetail> listClientResponseDetail, List<DanhMucServiceRef.DM_KHU_VUC_NS> lstNhanSu = null)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            DMKhuVucRequest request = Common.Utilities.PrepareRequest(new DMKhuVucRequest());
            request.obj = obj;
            if (!LObject.IsNullOrEmpty(lstNhanSu))
                request.lstNhanSu = lstNhanSu.ToArray();

            request.Module = DatabaseConstant.Module.DMDC;
            request.Function = DatabaseConstant.Function.DC_DM_KHU_VUC;
            request.Action = DatabaseConstant.Action.SUA;

            DMKhuVucResponse response = ClientDanhMuc.KhuVuc(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Xử lý dữ liệu trả về
            if (response == null)
            {
                return null;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return response.obj;
            }
        }

        public bool XoaKhuVuc(int[] listID,
            ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            DMKhuVucRequest request = Common.Utilities.PrepareRequest(new DMKhuVucRequest());
            request.listID = listID;

            request.Module = DatabaseConstant.Module.DMDC;
            request.Function = DatabaseConstant.Function.DC_DM_KHU_VUC;
            request.Action = DatabaseConstant.Action.XOA;

            DMKhuVucResponse response = ClientDanhMuc.KhuVuc(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Xử lý dữ liệu trả về
            if (response == null)
            {
                return false;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return response.bKetQua;
            }
        }

        public bool DuyetKhuVuc(int[] listID,
            ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            DMKhuVucRequest request = Common.Utilities.PrepareRequest(new DMKhuVucRequest());
            request.listID = listID.ToArray();

            request.Module = DatabaseConstant.Module.DMDC;
            request.Function = DatabaseConstant.Function.DC_DM_KHU_VUC;
            request.Action = DatabaseConstant.Action.DUYET;

            DMKhuVucResponse response = ClientDanhMuc.KhuVuc(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Xử lý dữ liệu trả về
            if (response == null)
            {
                return false;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return response.bKetQua;
            }
        }

        public bool ThoaiDuyetKhuVuc(int[] listID,
            ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            DMKhuVucRequest request = Common.Utilities.PrepareRequest(new DMKhuVucRequest());
            request.listID = listID.ToArray();

            request.Module = DatabaseConstant.Module.DMDC;
            request.Function = DatabaseConstant.Function.DC_DM_KHU_VUC;
            request.Action = DatabaseConstant.Action.THOAI_DUYET;

            DMKhuVucResponse response = ClientDanhMuc.KhuVuc(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Xử lý dữ liệu trả về
            if (response == null)
            {
                return false;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return response.bKetQua;
            }
        }

        public bool TuChoiKhuVuc(int[] listID,
            ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            DMKhuVucRequest request = Common.Utilities.PrepareRequest(new DMKhuVucRequest());
            request.listID = listID.ToArray();

            request.Module = DatabaseConstant.Module.DMDC;
            request.Function = DatabaseConstant.Function.DC_DM_KHU_VUC;
            request.Action = DatabaseConstant.Action.TU_CHOI_DUYET;

            DMKhuVucResponse response = ClientDanhMuc.KhuVuc(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Xử lý dữ liệu trả về
            if (response == null)
            {
                return false;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return response.bKetQua;
            }
        }

        public bool KhuVuc02(DatabaseConstant.Action action, ref DM_KHU_VUC obj, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            DMKhuVucRequest request = null;
            DMKhuVucResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.KhachHangService.layGiaTri());

                request = Common.Utilities.PrepareRequest(new DanhMucServiceRef.DMKhuVucRequest());
                response = new DanhMucServiceRef.DMKhuVucResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.DC_DM_KHU_VUC;
                request.Action = action;
                request.obj = obj;

                // make a call to service client here
                response = ClientDanhMuc.KhuVuc02(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    obj = response.obj;
                    listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                    return true;
                }
                else
                {
                    listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                    return false;
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                request = null;
                response = null;
            }
        }

        public bool DanhSachKhuVuc02(DatabaseConstant.Action action, ref List<DM_KHU_VUC> lstKhuVuc, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            DMKhuVucRequest request = null;
            DMKhuVucResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.KhachHangService.layGiaTri());

                request = Common.Utilities.PrepareRequest(new DanhMucServiceRef.DMKhuVucRequest());
                response = new DanhMucServiceRef.DMKhuVucResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.DC_DM_KHU_VUC_DS;
                request.Action = action;
                if (lstKhuVuc.Count > 0)
                    request.lstKhuVuc = lstKhuVuc.ToArray();
                request.lstKhuVuc = lstKhuVuc.ToArray();

                // make a call to service client here
                response = ClientDanhMuc.KhuVuc02(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    lstKhuVuc = response.lstKhuVuc.ToList();                    
                    listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                    return true;
                }
                else
                {
                    listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                    return false;
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                request = null;
                response = null;
            }
        }

        public DataSet GetDanhSachKhuVuc02(string maDonVi, string trangThaiNVu)
        {
            TruyVanServiceClient client = null;
            TruyVanRequest request = null;
            TruyVanResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

                client = TruyVanServiceClient(ApplicationConstant.SystemService.TruyVanService);
                request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());
                response = new TruyVanServiceRef.TruyVanResponse();

                //Khởi tạo request
                DataTable dt = null;
                LDatatable.MakeParameterTable(ref dt);
                LDatatable.AddParameter(ref dt, "@TRANG_THAI_NVU", "STRING", trangThaiNVu);
                LDatatable.AddParameter(ref dt, "@MA_DVI", "STRING", maDonVi);

                request.dtThamSo = dt;
                request.objectName = "INQ.DS.KHU_VUC";
                request.inquiryName = "DANH_SACH_02";

                // make a call to service client here
                response = client.TruyVanMessage(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                DataSet ds = response.dsResult;
                return ds;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                if (client.State == CommunicationState.Faulted)
                {
                    client.Abort();
                }
                else
                {
                    client.Close();
                }

                client = null;
                request = null;
                response = null;
            }
        }

        public DataSet GetDanhSachKhuVuc03(string maDonVi, string trangThaiNVu)
        {
            TruyVanServiceClient client = null;
            TruyVanRequest request = null;
            TruyVanResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

                client = TruyVanServiceClient(ApplicationConstant.SystemService.TruyVanService);
                request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());
                response = new TruyVanServiceRef.TruyVanResponse();

                //Khởi tạo request
                DataTable dt = null;
                LDatatable.MakeParameterTable(ref dt);
                LDatatable.AddParameter(ref dt, "@TRANG_THAI_NVU", "STRING", trangThaiNVu);
                LDatatable.AddParameter(ref dt, "@MA_DVI", "STRING", maDonVi);

                request.dtThamSo = dt;
                request.objectName = "INQ.DS.KHU_VUC";
                request.inquiryName = "DANH_SACH_03";

                // make a call to service client here
                response = client.TruyVanMessage(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                DataSet ds = response.dsResult;
                return ds;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                if (client.State == CommunicationState.Faulted)
                {
                    client.Abort();
                }
                else
                {
                    client.Close();
                }

                client = null;
                request = null;
                response = null;
            }
        }

        #endregion

        #region Danh mục nhóm

        /// <summary>
        /// Lấy toàn bộ danh sách nhóm
        /// </summary>
        /// <returns>DataSet chứa danh sách nhóm</returns>
        public DataSet getDanhSachNhom()
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "", "", "");

            request.dtThamSo = dt;
            request.inquiryName = DatabaseConstant.DanhSachTruyVan.DM_NHOM.getValue();

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet getDanhSachNhom01(DataTable dt)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            

            request.dtThamSo = dt;
            request.objectName = "INQ.DS.NHOM";
            request.inquiryName = "DANH_SACH_01";


            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet getDanhSachNhom02(DataTable dt)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            

            request.dtThamSo = dt;
            request.objectName = "INQ.DS.NHOM";
            request.inquiryName = "DANH_SACH_02";


            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet getTreeNhom_01(DataTable dt)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            

            request.dtThamSo = dt;
            request.objectName = "INQ.DS.NHOM";
            request.inquiryName = "TREE_01";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet getTreeNhom_02(DataTable dt)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            

            request.dtThamSo = dt;
            request.objectName = "INQ.DS.NHOM";
            request.inquiryName = "TREE_02";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        /// <summary>
        /// Lấy thông tin địa bàn theo id
        /// </summary>
        public DM_NHOM getNhomById(int id)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            //Khởi tạo và gán các giá trị cho request
            DMNhomRequest request = Common.Utilities.PrepareRequest(new DMNhomRequest());
            DMNhomResponse response = new DMNhomResponse();

            request.Id = id;

            // Lấy kết quả trả về
            response = ClientDanhMuc.GetThongTinNhom(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.obj;
        }

        public DataSet getThongTinCTietNhom(string idNhom)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@ID_NHOM", "String", idNhom);

            request.dtThamSo = dt;
            request.inquiryName = "%";
            request.objectName = "INQ.CT.NHOM_CT";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        /// <summary>
        /// Lấy thông tin cụm theo id
        /// </summary>
        public ApplicationConstant.ResponseStatus getNhomById(int id, ref DM_NHOM obj, ref string responseMessage)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            //Khởi tạo và gán các giá trị cho request
            DMNhomRequest request = Common.Utilities.PrepareRequest(new DMNhomRequest());
            DMNhomResponse response = new DMNhomResponse();

            request.Id = id;

            // Lấy kết quả trả về
            response = ClientDanhMuc.GetThongTinNhom(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Lấy kết quả trả về
            if (response == null)
            {
                return ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
            }
            else
            {
                responseMessage = response.ResponseMessage;
                obj = response.obj;
                return response.ResponseStatus;
            }
        }

        public ApplicationConstant.ResponseStatus ThemNhom(ref DM_NHOM obj, ref string responseMessage,List<KH_KHANG_NHOM> lstKHNhom=null)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new DMNhomRequest());
            request.obj = obj;
            if (!LObject.IsNullOrEmpty(lstKHNhom))
                request.lstNhomKHang = lstKHNhom.ToArray();

            request.Module = DatabaseConstant.Module.DMDC;
            request.Function = DatabaseConstant.Function.DC_DM_NHOM;
            request.Action = DatabaseConstant.Action.THEM;

            // Lấy kết quả trả về
            DMNhomResponse response = ClientDanhMuc.LuuNhom(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Lấy kết quả trả về
            if (response == null)
            {
                return ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
            }
            else
            {
                responseMessage = response.ResponseMessage;
                obj = response.obj;
                return response.ResponseStatus;
            }
        }

        public ApplicationConstant.ResponseStatus ThemNhom(ref DM_NHOM obj, ref string responseMessage, ref List<string> responseMessageData, List<KH_KHANG_NHOM> lstKHNhom = null)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new DMNhomRequest());
            request.obj = obj;
            if (!LObject.IsNullOrEmpty(lstKHNhom))
                request.lstNhomKHang = lstKHNhom.ToArray();

            request.Module = DatabaseConstant.Module.DMDC;
            request.Function = DatabaseConstant.Function.DC_DM_NHOM;
            request.Action = DatabaseConstant.Action.THEM;

            // Lấy kết quả trả về
            DMNhomResponse response = ClientDanhMuc.LuuNhom(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Lấy kết quả trả về
            if (response == null)
            {
                return ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
            }
            else
            {
                responseMessage = response.ResponseMessage;
                responseMessageData = response.ResponseMessageData != null ? response.ResponseMessageData.ToList() : null;
                obj = response.obj;
                return response.ResponseStatus;
            }
        }

        public ApplicationConstant.ResponseStatus SuaNhom(ref DM_NHOM obj, ref string responseMessage, List<KH_KHANG_NHOM> lstKHNhom = null,List<int> lstIDKHNhomXoa=null)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            DMNhomRequest request = Common.Utilities.PrepareRequest(new DMNhomRequest());
            request.obj = obj;
            if (!LObject.IsNullOrEmpty(lstKHNhom))
                request.lstNhomKHang = lstKHNhom.ToArray();
            if (!LObject.IsNullOrEmpty(lstIDKHNhomXoa))
                request.listIDNhomKHangXoa = lstIDKHNhomXoa.ToArray();

            request.Module = DatabaseConstant.Module.DMDC;
            request.Function = DatabaseConstant.Function.DC_DM_NHOM;
            request.Action = DatabaseConstant.Action.SUA;

            DMNhomResponse response = ClientDanhMuc.SuaThongTinNhom(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Lấy kết quả trả về
            if (response == null)
            {
                return ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
            }
            else
            {
                responseMessage = response.ResponseMessage;
                obj = response.obj;
                return response.ResponseStatus;
            }
        }

        public DM_NHOM ThemNhom(DanhMucServiceRef.DM_NHOM obj)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new DMNhomRequest());
            request.obj = obj;

            request.Module = DatabaseConstant.Module.DMDC;
            request.Function = DatabaseConstant.Function.DC_DM_NHOM;
            request.Action = DatabaseConstant.Action.THEM;

            // Lấy kết quả trả về
            DMNhomResponse response = ClientDanhMuc.LuuNhom(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            if (response == null)
            {
                return null;
            }
            else
            {
                return response.obj;
            }
        }

        public DM_NHOM SuaNhom(DanhMucServiceRef.DM_NHOM obj)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            DMNhomRequest request = Common.Utilities.PrepareRequest(new DMNhomRequest());
            request.obj = obj;

            request.Module = DatabaseConstant.Module.DMDC;
            request.Function = DatabaseConstant.Function.DC_DM_NHOM;
            request.Action = DatabaseConstant.Action.SUA;

            DMNhomResponse response = ClientDanhMuc.SuaThongTinNhom(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Xử lý dữ liệu trả về
            if (response == null)
            {
                return null;
            }
            else
            {
                return response.obj;
            }
        }

        public bool XoaNhom(int[] listID,
            ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            DMNhomRequest request = Common.Utilities.PrepareRequest(new DMNhomRequest());
            request.listID = listID;

            request.Module = DatabaseConstant.Module.DMDC;
            request.Function = DatabaseConstant.Function.DC_DM_NHOM;
            request.Action = DatabaseConstant.Action.XOA;

            DMNhomResponse response = ClientDanhMuc.XoaListNhomTheoID(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Xử lý dữ liệu trả về
            if (response == null)
            {
                return false;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return response.bKetQua;
            }
        }

        public bool DuyetNhom(int[] listID,
            ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            DMNhomRequest request = Common.Utilities.PrepareRequest(new DMNhomRequest());
            request.listID = listID.ToArray();

            request.Module = DatabaseConstant.Module.DMDC;
            request.Function = DatabaseConstant.Function.DC_DM_NHOM;
            request.Action = DatabaseConstant.Action.DUYET;

            DMNhomResponse response = ClientDanhMuc.SuaThongTinNhom(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Xử lý dữ liệu trả về
            if (response == null)
            {
                return false;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return response.bKetQua;
            }
        }

        public bool ThoaiDuyetNhom(int[] listID,
            ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            DMNhomRequest request = Common.Utilities.PrepareRequest(new DMNhomRequest());
            request.listID = listID.ToArray();

            request.Module = DatabaseConstant.Module.DMDC;
            request.Function = DatabaseConstant.Function.DC_DM_NHOM;
            request.Action = DatabaseConstant.Action.THOAI_DUYET;

            DMNhomResponse response = ClientDanhMuc.SuaThongTinNhom(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Xử lý dữ liệu trả về
            if (response == null)
            {
                return false;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return response.bKetQua;
            }
        }

        public bool TuChoiNhom(int[] listID,
            ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            DMNhomRequest request = Common.Utilities.PrepareRequest(new DMNhomRequest());
            request.listID = listID.ToArray();

            request.Module = DatabaseConstant.Module.DMDC;
            request.Function = DatabaseConstant.Function.DC_DM_NHOM;
            request.Action = DatabaseConstant.Action.TU_CHOI_DUYET;

            DMNhomResponse response = ClientDanhMuc.SuaThongTinNhom(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Xử lý dữ liệu trả về
            if (response == null)
            {
                return false;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return response.bKetQua;
            }
        }

        public List<DM_NHOM> TimKiemNhomKhachHang()
        {
            
             return null;
        }

        public bool Nhom02(DatabaseConstant.Action action, ref DM_NHOM_DTO objNhomDTO, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            DMNhomRequest request = null;
            DMNhomResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.KhachHangService.layGiaTri());

                request = Common.Utilities.PrepareRequest(new DanhMucServiceRef.DMNhomRequest());
                response = new DanhMucServiceRef.DMNhomResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.DC_DM_NHOM;
                request.Action = action;
                request.objNhomDTO = objNhomDTO;                

                // make a call to service client here
                response = ClientDanhMuc.Nhom02(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    objNhomDTO = response.objNhomDTO;                    
                    listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                    return true;
                }
                else
                {
                    listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                    return false;
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                request = null;
                response = null;
            }
        }

        public bool DanhSachNhom02(DatabaseConstant.Action action, ref List<DM_NHOM_DTO> lstNhomDTO, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            DMNhomRequest request = null;
            DMNhomResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.KhachHangService.layGiaTri());

                request = Common.Utilities.PrepareRequest(new DanhMucServiceRef.DMNhomRequest());
                response = new DanhMucServiceRef.DMNhomResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.DC_DM_NHOM_DS;
                request.Action = action;
                if (lstNhomDTO.Count > 0)
                    request.lstNhomDTO = lstNhomDTO.ToArray();                

                // make a call to service client here
                response = ClientDanhMuc.Nhom02(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    lstNhomDTO = response.lstNhomDTO.ToList();
                    listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                    return true;
                }
                else
                {
                    listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                    return false;
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                request = null;
                response = null;
            }
        }

        #endregion

        #region Danh mục phân hệ GD

        /// <summary>
        /// Lấy toàn bộ danh sách phân hệ GD
        /// </summary>
        /// <returns>DataSet chứa danh sách phân hệ GD</returns>
        public DataSet getDanhSachPhanHeGD()
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "", "", "");

            request.dtThamSo = dt;
            request.inquiryName = DatabaseConstant.DanhSachTruyVan.DM_PHANHE_GD.getValue();

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet getDanhSachPhanHeGDTheoID(int id)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@ID", "INT", id.ToString());

            request.dtThamSo = dt;
            request.inquiryName = DatabaseConstant.DanhSachTruyVan.DM_PHANHE_GD.getValue();

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet getDanhSachPhanHeGDTheoMa(string maPhanHeGD)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@MaPhanHeGD", "STRING", maPhanHeGD);

            request.dtThamSo = dt;
            request.inquiryName = "INQ.CT.DM_PHAN_HE_GD";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        /// <summary>
        /// Lấy thông tin địa bàn theo id
        /// </summary>
        public DM_PHAN_HE_GD getPhanHeGDById(int id)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            //Khởi tạo và gán các giá trị cho request
            DMPhanHeGDRequest request = Common.Utilities.PrepareRequest(new DMPhanHeGDRequest());
            DMPhanHeGDResponse response = new DMPhanHeGDResponse();

            request.Id = id;

            // Lấy kết quả trả về
            response = ClientDanhMuc.GetThongTinPhanHeGD(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.obj;
        }

        public DM_PHAN_HE_GD ThemPhanHeGD(DanhMucServiceRef.DM_PHAN_HE_GD obj)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new DMPhanHeGDRequest());
            request.obj = obj;

            request.Module = DatabaseConstant.Module.DMDC;
            request.Function = DatabaseConstant.Function.DC_DM_PHAN_HE_GD;
            request.Action = DatabaseConstant.Action.THEM;

            // Lấy kết quả trả về
            DMPhanHeGDResponse response = ClientDanhMuc.LuuPhanHeGD(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            if (response == null)
            {
                return null;
            }
            else
            {
                return response.obj;
            }
        }

        public DM_PHAN_HE_GD SuaPhanHeGD(DanhMucServiceRef.DM_PHAN_HE_GD obj)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            DMPhanHeGDRequest request = Common.Utilities.PrepareRequest(new DMPhanHeGDRequest());
            request.obj = obj;

            request.Module = DatabaseConstant.Module.DMDC;
            request.Function = DatabaseConstant.Function.DC_DM_PHAN_HE_GD;
            request.Action = DatabaseConstant.Action.SUA;

            DMPhanHeGDResponse response = ClientDanhMuc.SuaThongTinPhanHeGD(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Xử lý dữ liệu trả về
            if (response == null)
            {
                return null;
            }
            else
            {
                return response.obj;
            }
        }

        public bool XoaPhanHeGD(int[] listID,
            ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            DMPhanHeGDRequest request = Common.Utilities.PrepareRequest(new DMPhanHeGDRequest());
            request.listID = listID;

            request.Module = DatabaseConstant.Module.DMDC;
            request.Function = DatabaseConstant.Function.DC_DM_PHAN_HE_GD;
            request.Action = DatabaseConstant.Action.XOA;

            DMPhanHeGDResponse response = ClientDanhMuc.XoaListPhanHeGDTheoID(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Xử lý dữ liệu trả về
            if (response == null)
            {
                return false;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return response.bKetQua;
            }
        }

        public bool DuyetPhanHeGD(int[] listID,
            ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            DMPhanHeGDRequest request = Common.Utilities.PrepareRequest(new DMPhanHeGDRequest());
            request.listID = listID.ToArray();

            request.Module = DatabaseConstant.Module.DMDC;
            request.Function = DatabaseConstant.Function.DC_DM_PHAN_HE_GD;
            request.Action = DatabaseConstant.Action.DUYET;

            DMPhanHeGDResponse response = ClientDanhMuc.SuaThongTinPhanHeGD(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Xử lý dữ liệu trả về
            if (response == null)
            {
                return false;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return response.bKetQua;
            }
        }

        public bool ThoaiDuyetPhanHeGD(int[] listID,
            ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            DMPhanHeGDRequest request = Common.Utilities.PrepareRequest(new DMPhanHeGDRequest());
            request.listID = listID.ToArray();

            request.Module = DatabaseConstant.Module.DMDC;
            request.Function = DatabaseConstant.Function.DC_DM_PHAN_HE_GD;
            request.Action = DatabaseConstant.Action.THOAI_DUYET;

            DMPhanHeGDResponse response = ClientDanhMuc.SuaThongTinPhanHeGD(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Xử lý dữ liệu trả về
            if (response == null)
            {
                return false;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return response.bKetQua;
            }
        }

        public bool TuChoiPhanHeGD(int[] listID,
            ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            DMPhanHeGDRequest request = Common.Utilities.PrepareRequest(new DMPhanHeGDRequest());
            request.listID = listID.ToArray();

            request.Module = DatabaseConstant.Module.DMDC;
            request.Function = DatabaseConstant.Function.DC_DM_PHAN_HE_GD;
            request.Action = DatabaseConstant.Action.TU_CHOI_DUYET;

            DMPhanHeGDResponse response = ClientDanhMuc.SuaThongTinPhanHeGD(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Xử lý dữ liệu trả về
            if (response == null)
            {
                return false;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return response.bKetQua;
            }
        }

        /// <summary>
        /// Duyệt thông tin địa bàn
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool khongSuDungPhanHeGD(int[] listID,
            ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            DMPhanHeGDRequest request = Common.Utilities.PrepareRequest(new DMPhanHeGDRequest());
            request.listID = listID.ToArray();

            request.Module = DatabaseConstant.Module.DMDC;
            request.Function = DatabaseConstant.Function.DC_DM_PHAN_HE_GD;
            request.Action = DatabaseConstant.Action.TU_CHOI_DUYET;

            DMPhanHeGDResponse response = ClientDanhMuc.SuaThongTinPhanHeGD(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Xử lý dữ liệu trả về
            if (response == null)
            {
                return false;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return response.bKetQua;
            }
        }

        /// <summary>
        /// Duyệt thông tin địa bàn
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool suDungPhanHeGD(int[] listID,
            ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            DMPhanHeGDRequest request = Common.Utilities.PrepareRequest(new DMPhanHeGDRequest());
            request.listID = listID.ToArray();

            request.Module = DatabaseConstant.Module.DMDC;
            request.Function = DatabaseConstant.Function.DC_DM_PHAN_HE_GD;
            request.Action = DatabaseConstant.Action.TU_CHOI_DUYET;

            DMPhanHeGDResponse response = ClientDanhMuc.SuaThongTinPhanHeGD(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Xử lý dữ liệu trả về
            if (response == null)
            {
                return false;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return response.bKetQua;
            }
        }

        /// <summary>
        /// Duyệt thông tin địa bàn
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public DM_PHAN_HE layPhanHe(DanhMucServiceRef.DM_PHAN_HE obj)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            DMPhanHeGDRequest request = Common.Utilities.PrepareRequest(new DMPhanHeGDRequest());

            request.objPhanHe = obj;

            DMPhanHeGDResponse response = ClientDanhMuc.LayPhanHe(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Xử lý dữ liệu trả về
            if (response == null)
            {
                return null;
            }
            else
            {
                return response.objPhanHe;
            }
        }

        /// <summary>
        /// Lấy thông tin địa bàn theo id
        /// </summary>
        public DM_PHAN_HE getPhanHeById(int id)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            //Khởi tạo và gán các giá trị cho request
            DMPhanHeGDRequest request = Common.Utilities.PrepareRequest(new DMPhanHeGDRequest());
            DMPhanHeGDResponse response = new DMPhanHeGDResponse();

            request.Id = id;

            // Lấy kết quả trả về
            response = ClientDanhMuc.GetThongTinPhanHe(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.objPhanHe;
        }

        public DM_PHAN_HE ThemPhanHe(DanhMucServiceRef.DM_PHAN_HE obj)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new DMPhanHeGDRequest());
            request.objPhanHe = obj;

            request.Module = DatabaseConstant.Module.DMDC;
            request.Function = DatabaseConstant.Function.DC_DM_PHAN_HE_GD;
            request.Action = DatabaseConstant.Action.THEM;

            // Lấy kết quả trả về
            DMPhanHeGDResponse response = ClientDanhMuc.LuuPhanHe(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            if (response == null)
            {
                return null;
            }
            else
            {
                return response.objPhanHe;
            }
        }

        public DM_PHAN_HE SuaPhanHe(DanhMucServiceRef.DM_PHAN_HE obj)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            DMPhanHeGDRequest request = Common.Utilities.PrepareRequest(new DMPhanHeGDRequest());
            request.objPhanHe = obj;

            request.Module = DatabaseConstant.Module.DMDC;
            request.Function = DatabaseConstant.Function.DC_DM_PHAN_HE_GD;
            request.Action = DatabaseConstant.Action.SUA;

            DMPhanHeGDResponse response = ClientDanhMuc.SuaThongTinPhanHe(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Xử lý dữ liệu trả về
            if (response == null)
            {
                return null;
            }
            else
            {
                return response.objPhanHe;
            }
        }

        public bool XoaPhanHe(int[] listID,
            ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            DMPhanHeGDRequest request = Common.Utilities.PrepareRequest(new DMPhanHeGDRequest());
            request.listID = listID;

            request.Module = DatabaseConstant.Module.DMDC;
            request.Function = DatabaseConstant.Function.DC_DM_PHAN_HE_GD;
            request.Action = DatabaseConstant.Action.XOA;

            DMPhanHeGDResponse response = ClientDanhMuc.XoaListPhanHeTheoID(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Xử lý dữ liệu trả về
            if (response == null)
            {
                return false;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return response.bKetQua;
            }
        }

        public bool DuyetPhanHe(int[] listID,
            ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            DMPhanHeGDRequest request = Common.Utilities.PrepareRequest(new DMPhanHeGDRequest());
            request.listID = listID.ToArray();

            request.Module = DatabaseConstant.Module.DMDC;
            request.Function = DatabaseConstant.Function.DC_DM_PHAN_HE_GD;
            request.Action = DatabaseConstant.Action.DUYET;

            DMPhanHeGDResponse response = ClientDanhMuc.SuaThongTinPhanHe(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Xử lý dữ liệu trả về
            if (response == null)
            {
                return false;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return response.bKetQua;
            }
        }

        public bool ThoaiDuyetPhanHe(int[] listID,
            ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            DMPhanHeGDRequest request = Common.Utilities.PrepareRequest(new DMPhanHeGDRequest());
            request.listID = listID.ToArray();

            request.Module = DatabaseConstant.Module.DMDC;
            request.Function = DatabaseConstant.Function.DC_DM_PHAN_HE_GD;
            request.Action = DatabaseConstant.Action.THOAI_DUYET;

            DMPhanHeGDResponse response = ClientDanhMuc.SuaThongTinPhanHe(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Xử lý dữ liệu trả về
            if (response == null)
            {
                return false;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return response.bKetQua;
            }
        }

        public bool TuChoiPhanHe(int[] listID,
            ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            DMPhanHeGDRequest request = Common.Utilities.PrepareRequest(new DMPhanHeGDRequest());
            request.listID = listID.ToArray();

            request.Module = DatabaseConstant.Module.DMDC;
            request.Function = DatabaseConstant.Function.DC_DM_PHAN_HE_GD;
            request.Action = DatabaseConstant.Action.TU_CHOI_DUYET;

            DMPhanHeGDResponse response = ClientDanhMuc.SuaThongTinPhanHe(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Xử lý dữ liệu trả về
            if (response == null)
            {
                return false;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return response.bKetQua;
            }
        }

        /// <summary>
        /// Duyệt thông tin địa bàn
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool khongSuDungPhanHe(int[] listID,
            ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            DMPhanHeGDRequest request = Common.Utilities.PrepareRequest(new DMPhanHeGDRequest());
            request.listID = listID.ToArray();

            request.Module = DatabaseConstant.Module.DMDC;
            request.Function = DatabaseConstant.Function.DC_DM_PHAN_HE_GD;
            request.Action = DatabaseConstant.Action.TU_CHOI_DUYET;

            DMPhanHeGDResponse response = ClientDanhMuc.SuaThongTinPhanHe(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Xử lý dữ liệu trả về
            if (response == null)
            {
                return false;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return response.bKetQua;
            }
        }

        /// <summary>
        /// Duyệt thông tin địa bàn
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool suDungPhanHe(int[] listID,
            ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            DMPhanHeGDRequest request = Common.Utilities.PrepareRequest(new DMPhanHeGDRequest());
            request.listID = listID.ToArray();

            request.Module = DatabaseConstant.Module.DMDC;
            request.Function = DatabaseConstant.Function.DC_DM_PHAN_HE_GD;
            request.Action = DatabaseConstant.Action.TU_CHOI_DUYET;

            DMPhanHeGDResponse response = ClientDanhMuc.SuaThongTinPhanHe(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Xử lý dữ liệu trả về
            if (response == null)
            {
                return false;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return response.bKetQua;
            }
        }

        #endregion

        #region Danh mục tỉnh thành DM_TINH_TP

        public DataSet getTinhTPTheoMa(string maTinhTP)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            //Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());
            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@MA_TINH_TP", "STRING", maTinhTP);

            request.dtThamSo = dt;
            request.inquiryName = "GET_TINH_TP_THEO_MA";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        /// <summary>
        /// Lấy toàn bộ danh sách tỉnh thành
        /// </summary>
        /// <returns>DataSet chứa danh sách tỉnh thành</returns>
        public DataSet getDanhSachTinhThanh()
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "", "", "");

            request.dtThamSo = dt;
            request.inquiryName = DatabaseConstant.DanhSachTruyVan.DM_TINHTP.getValue();

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        /// <summary>
        /// Lấy thông tin địa bàn theo id
        /// </summary>
        public DM_TINH_TP getTinhTPById(int id)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            //Khởi tạo và gán các giá trị cho request
            DMTinhTPRequest request = Common.Utilities.PrepareRequest(new DMTinhTPRequest());
            DMTinhTPResponse response = new DMTinhTPResponse();

            request.Id = id;

            // Lấy kết quả trả về
            response = ClientDanhMuc.GetThongTinTinhTP(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.obj;
        }

        /// <summary>
        /// Lấy thông tin địa bàn theo id
        /// </summary>
        public DMTinhTPResponse getTinhTPById01(int id)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            //Khởi tạo và gán các giá trị cho request
            DMTinhTPRequest request = Common.Utilities.PrepareRequest(new DMTinhTPRequest());
            DMTinhTPResponse response = new DMTinhTPResponse();

            request.Id = id;

            // Lấy kết quả trả về
            response = ClientDanhMuc.GetThongTinTinhTP01(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response;
        }

        public ApplicationConstant.ResponseStatus ThemTinhTP(ref DM_TINH_TP obj,
            ref string responseMessage)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new DMTinhTPRequest());
            request.obj = obj;

            request.Module = DatabaseConstant.Module.DMDC;
            request.Function = DatabaseConstant.Function.DC_DM_TINH_THANH;
            request.Action = DatabaseConstant.Action.THEM;

            // Lấy kết quả trả về
            DMTinhTPResponse response = ClientDanhMuc.LuuTinhTP(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            if (response == null)
            {
                return ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
            }
            else
            {
                responseMessage = response.ResponseMessage;
                obj = response.obj;
                return response.ResponseStatus;
            }
        }

        public ApplicationConstant.ResponseStatus ThemTinhTP01(ref DM_TINH_TP obj,
            ref VDM_DBAN_TTIN_KHAC objTTKhac,
            ref string responseMessage)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new DMTinhTPRequest());
            request.obj = obj;
            request.objTTKhac = objTTKhac;

            request.Module = DatabaseConstant.Module.DMDC;
            request.Function = DatabaseConstant.Function.DC_DM_TINH_THANH;
            request.Action = DatabaseConstant.Action.THEM;

            // Lấy kết quả trả về
            DMTinhTPResponse response = ClientDanhMuc.LuuTinhTP01(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            if (response == null)
            {
                return ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
            }
            else
            {
                responseMessage = response.ResponseMessage;
                obj = response.obj;
                objTTKhac = response.objTTKhac;
                return response.ResponseStatus;
            }
        }

        public ApplicationConstant.ResponseStatus SuaTinhTP(ref DM_TINH_TP obj,
            ref string responseMessage)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            DMTinhTPRequest request = Common.Utilities.PrepareRequest(new DMTinhTPRequest());
            request.obj = obj;

            request.Module = DatabaseConstant.Module.DMDC;
            request.Function = DatabaseConstant.Function.DC_DM_TINH_THANH;
            request.Action = DatabaseConstant.Action.SUA;

            DMTinhTPResponse response = ClientDanhMuc.SuaThongTinTinhTP(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Xử lý dữ liệu trả về
            if (response == null)
            {
                return ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
            }
            else
            {
                responseMessage = response.ResponseMessage;
                obj = response.obj;
                return response.ResponseStatus;
            }
        }

        public ApplicationConstant.ResponseStatus SuaTinhTP01(ref DM_TINH_TP obj,
            ref VDM_DBAN_TTIN_KHAC objTTKhac,
            ref string responseMessage)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            DMTinhTPRequest request = Common.Utilities.PrepareRequest(new DMTinhTPRequest());
            request.obj = obj;
            request.objTTKhac = objTTKhac;

            request.Module = DatabaseConstant.Module.DMDC;
            request.Function = DatabaseConstant.Function.DC_DM_TINH_THANH;
            request.Action = DatabaseConstant.Action.SUA;

            DMTinhTPResponse response = ClientDanhMuc.SuaThongTinTinhTP01(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Xử lý dữ liệu trả về
            if (response == null)
            {
                return ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
            }
            else
            {
                responseMessage = response.ResponseMessage;
                obj = response.obj;
                objTTKhac = response.objTTKhac;
                return response.ResponseStatus;
            }
        }

        public bool XoaTinhTP(int[] listID,
            ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            DMTinhTPRequest request = Common.Utilities.PrepareRequest(new DMTinhTPRequest());
            request.listID = listID;

            request.Module = DatabaseConstant.Module.DMDC;
            request.Function = DatabaseConstant.Function.DC_DM_TINH_THANH;
            request.Action = DatabaseConstant.Action.XOA;

            DMTinhTPResponse response = ClientDanhMuc.XoaListTinhTPTheoID(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Xử lý dữ liệu trả về
            if (response == null)
            {
                return false;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return response.bKetQua;
            }
        }

        public bool XoaTinhTP01(int[] listID,
            ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            DMTinhTPRequest request = Common.Utilities.PrepareRequest(new DMTinhTPRequest());
            request.listID = listID;

            request.Module = DatabaseConstant.Module.DMDC;
            request.Function = DatabaseConstant.Function.DC_DM_TINH_THANH;
            request.Action = DatabaseConstant.Action.XOA;

            DMTinhTPResponse response = ClientDanhMuc.XoaListTinhTPTheoID01(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Xử lý dữ liệu trả về
            if (response == null)
            {
                return false;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return response.bKetQua;
            }
        }

        public bool DuyetTinhTP(int[] listID,
            ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            DMTinhTPRequest request = Common.Utilities.PrepareRequest(new DMTinhTPRequest());
            request.listID = listID.ToArray();

            request.Module = DatabaseConstant.Module.DMDC;
            request.Function = DatabaseConstant.Function.DC_DM_TINH_THANH;
            request.Action = DatabaseConstant.Action.DUYET;

            DMTinhTPResponse response = ClientDanhMuc.SuaThongTinTinhTP(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Xử lý dữ liệu trả về
            if (response == null)
            {
                return false;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return response.bKetQua;
            }
        }

        public bool ThoaiDuyetTinhTP(int[] listID,
            ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            DMTinhTPRequest request = Common.Utilities.PrepareRequest(new DMTinhTPRequest());
            request.listID = listID.ToArray();

            request.Module = DatabaseConstant.Module.DMDC;
            request.Function = DatabaseConstant.Function.DC_DM_TINH_THANH;
            request.Action = DatabaseConstant.Action.THOAI_DUYET;

            DMTinhTPResponse response = ClientDanhMuc.SuaThongTinTinhTP(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Xử lý dữ liệu trả về
            if (response == null)
            {
                return false;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return response.bKetQua;
            }
        }

        public bool TuChoiTinhTP(int[] listID,
            ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            DMTinhTPRequest request = Common.Utilities.PrepareRequest(new DMTinhTPRequest());
            request.listID = listID.ToArray();

            request.Module = DatabaseConstant.Module.DMDC;
            request.Function = DatabaseConstant.Function.DC_DM_TINH_THANH;
            request.Action = DatabaseConstant.Action.TU_CHOI_DUYET;

            DMTinhTPResponse response = ClientDanhMuc.SuaThongTinTinhTP(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Xử lý dữ liệu trả về
            if (response == null)
            {
                return false;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return response.bKetQua;
            }
        }
        
        public bool checkMaTinhThanhPhoDaSuDung(string maTinhTP)
        {
            DataSet ds = null;
            ds = getTinhTPTheoMa(maTinhTP);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #endregion

        #region Danh mục tỉnh thành DM_VUNG_MIEN

        public DataSet getVungMienTheoMa(string maVungMien)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            //Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());
            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@MA_VUNG_MIEN", "STRING", maVungMien);

            request.dtThamSo = dt;
            request.inquiryName = "GET_VUNG_MIEN_THEO_MA";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        /// <summary>
        /// Lấy toàn bộ danh sách tỉnh thành
        /// </summary>
        /// <returns>DataSet chứa danh sách tỉnh thành</returns>
        public DataSet getDanhSachVungMien()
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "", "", "");

            request.dtThamSo = dt;
            //request.inquiryName = DatabaseConstant.DanhSachTruyVan.DM_VungMien.getValue();

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        /// <summary>
        /// Lấy thông tin địa bàn theo id
        /// </summary>
        public DM_VUNG_MIEN getVungMienById(int id)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            //Khởi tạo và gán các giá trị cho request
            DMVungMienRequest request = Common.Utilities.PrepareRequest(new DMVungMienRequest());
            DMVungMienResponse response = new DMVungMienResponse();

            request.Id = id;

            // Lấy kết quả trả về
            response = ClientDanhMuc.GetThongTinVungMien(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.obj;
        }

        public DM_VUNG_MIEN ThemVungMien(DanhMucServiceRef.DM_VUNG_MIEN obj)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new DMVungMienRequest());
            request.obj = obj;

            request.Module = DatabaseConstant.Module.DMDC;
            request.Function = DatabaseConstant.Function.DC_DM_TINH_THANH;
            request.Action = DatabaseConstant.Action.THEM;

            // Lấy kết quả trả về
            DMVungMienResponse response = ClientDanhMuc.LuuVungMien(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            if (response == null)
            {
                return null;
            }
            else
            {
                return response.obj;
            }
        }

        public DM_VUNG_MIEN SuaVungMien(DanhMucServiceRef.DM_VUNG_MIEN obj)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            DMVungMienRequest request = Common.Utilities.PrepareRequest(new DMVungMienRequest());
            request.obj = obj;

            request.Module = DatabaseConstant.Module.DMDC;
            request.Function = DatabaseConstant.Function.DC_DM_TINH_THANH;
            request.Action = DatabaseConstant.Action.SUA;

            DMVungMienResponse response = ClientDanhMuc.SuaThongTinVungMien(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Xử lý dữ liệu trả về
            if (response == null)
            {
                return null;
            }
            else
            {
                return response.obj;
            }
        }

        public bool XoaVungMien(int[] listID,
            ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            DMVungMienRequest request = Common.Utilities.PrepareRequest(new DMVungMienRequest());
            request.listID = listID;

            request.Module = DatabaseConstant.Module.DMDC;
            request.Function = DatabaseConstant.Function.DC_DM_TINH_THANH;
            request.Action = DatabaseConstant.Action.XOA;

            DMVungMienResponse response = ClientDanhMuc.XoaListVungMienTheoID(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Xử lý dữ liệu trả về
            if (response == null)
            {
                return false;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return response.bKetQua;
            }
        }

        public bool DuyetVungMien(int[] listID,
            ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            DMVungMienRequest request = Common.Utilities.PrepareRequest(new DMVungMienRequest());
            request.listID = listID.ToArray();

            request.Module = DatabaseConstant.Module.DMDC;
            request.Function = DatabaseConstant.Function.DC_DM_TINH_THANH;
            request.Action = DatabaseConstant.Action.DUYET;

            DMVungMienResponse response = ClientDanhMuc.SuaThongTinVungMien(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Xử lý dữ liệu trả về
            if (response == null)
            {
                return false;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return response.bKetQua;
            }
        }

        public bool ThoaiDuyetVungMien(int[] listID,
            ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            DMVungMienRequest request = Common.Utilities.PrepareRequest(new DMVungMienRequest());
            request.listID = listID.ToArray();

            request.Module = DatabaseConstant.Module.DMDC;
            request.Function = DatabaseConstant.Function.DC_DM_TINH_THANH;
            request.Action = DatabaseConstant.Action.THOAI_DUYET;

            DMVungMienResponse response = ClientDanhMuc.SuaThongTinVungMien(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Xử lý dữ liệu trả về
            if (response == null)
            {
                return false;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return response.bKetQua;
            }
        }

        public bool TuChoiVungMien(int[] listID,
            ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            DMVungMienRequest request = Common.Utilities.PrepareRequest(new DMVungMienRequest());
            request.listID = listID.ToArray();

            request.Module = DatabaseConstant.Module.DMDC;
            request.Function = DatabaseConstant.Function.DC_DM_TINH_THANH;
            request.Action = DatabaseConstant.Action.TU_CHOI_DUYET;

            DMVungMienResponse response = ClientDanhMuc.SuaThongTinVungMien(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Xử lý dữ liệu trả về
            if (response == null)
            {
                return false;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return response.bKetQua;
            }
        }

        #endregion

        public DataSet getTreeView(DataTable dt, string inqName)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.DanhMucService.layGiaTri());

            //Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());
            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();

            request.dtThamSo = dt;
            request.inquiryName = inqName;
            request.objectName = inqName;

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet GetTreeDonVi(string maDangNhap, string maDonVi)
        {
            TruyVanServiceClient client = null;
            TruyVanRequest request = null;
            TruyVanResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

                client = TruyVanServiceClient(ApplicationConstant.SystemService.TruyVanService);
                request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());
                response = new TruyVanServiceRef.TruyVanResponse();

                //Khởi tạo request
                DataTable dt = null;
                LDatatable.MakeParameterTable(ref dt);
                LDatatable.AddParameter(ref dt, "@MA_DANG_NHAP", "STRING", maDangNhap);
                LDatatable.AddParameter(ref dt, "@MA_DVI_QLY", "STRING", maDonVi);

                request.dtThamSo = dt;
                request.objectName = "INQ.DS.TREE_PVI";
                request.inquiryName = "DANH_SACH";

                // make a call to service client here
                response = client.TruyVanMessage(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                DataSet ds = response.dsResult;
                return ds;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                if (client.State == CommunicationState.Faulted)
                {
                    client.Abort();
                }
                else
                {
                    client.Close();
                }

                client = null;
                request = null;
                response = null;
            }
        }

        #region Tần suất

        /// <summary>
        /// Lấy toàn bộ danh sách cụm
        /// </summary>
        /// <returns>DataSet chứa danh sách cụm</returns>
        public DataSet getDanhSachTanSuat()
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "", "", "");

            request.dtThamSo = dt;
            //request.inquiryName = DatabaseConstant.DanhSachTruyVan.DC_TSUAT.getValue();

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet getTanSuatCT(int Id)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.UtilitiesService.layGiaTri());

            //Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());
            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@INP_MA_TSUAT", "STRING", Id.ToString());

            request.dtThamSo = dt;
            request.inquiryName = "INQ.CT.TAN_SUAT";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        /// <summary>
        /// Lấy thông tin cụm theo id
        /// </summary>
        public DC_TSUAT getTanSuatById(int id)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.UtilitiesService.layGiaTri());

            //Khởi tạo và gán các giá trị cho request
            DCTanSuatRequest request = Common.Utilities.PrepareRequest(new DCTanSuatRequest());
            DCTanSuatResponse response = new DCTanSuatResponse();

            request.Id = id;

            // Lấy kết quả trả về
            response = ClientDanhMuc.GetThongTinTanSuat(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.obj;
        }

        /// <summary>
        /// Lấy thông tin cụm theo id
        /// </summary>
        public DC_TSUAT GetTanSuatbyMaDonVi(string maDonVi)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.UtilitiesService.layGiaTri());

            //Khởi tạo và gán các giá trị cho request
            DCTanSuatRequest request = Common.Utilities.PrepareRequest(new DCTanSuatRequest());
            DCTanSuatResponse response = new DCTanSuatResponse();

            request.MaDonVi = maDonVi;

            // Lấy kết quả trả về
            response = ClientDanhMuc.GetTanSuatbyMaDonVi(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.obj;
        }

        /// <summary>
        /// Lấy thông tin cụm theo id
        /// </summary>
        public DC_TSUAT_CUM GetTanSuatCumbyIdCum(int idCum)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.UtilitiesService.layGiaTri());

            //Khởi tạo và gán các giá trị cho request
            DCTanSuatRequest request = Common.Utilities.PrepareRequest(new DCTanSuatRequest());
            DCTanSuatResponse response = new DCTanSuatResponse();

            request.IdCum = idCum;

            // Lấy kết quả trả về
            response = ClientDanhMuc.GetTanSuatbyIdCum(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.objTSuatCum;
        }

        public DC_TSUAT ThemTanSuat(DanhMucServiceRef.DC_TSUAT obj)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.UtilitiesService.layGiaTri());

            // Khởi tạo và gán giá trị cho request
            var request = Common.Utilities.PrepareRequest(new DCTanSuatRequest());
            request.obj = obj;

            request.Module = DatabaseConstant.Module.DMDC;
            request.Function = DatabaseConstant.Function.DC_TSUAT;
            request.Action = DatabaseConstant.Action.THEM;

            // Lấy kết quả trả về
            DCTanSuatResponse response = ClientDanhMuc.LuuTanSuat(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            if (response == null)
            {
                return null;
            }
            else
            {
                return response.obj;
            }
        }

        public DC_TSUAT SuaTanSuat(DanhMucServiceRef.DC_TSUAT obj)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.UtilitiesService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            DCTanSuatRequest request = Common.Utilities.PrepareRequest(new DCTanSuatRequest());
            request.obj = obj;

            request.Module = DatabaseConstant.Module.DMDC;
            request.Function = DatabaseConstant.Function.DC_TSUAT;
            request.Action = DatabaseConstant.Action.SUA;

            DCTanSuatResponse response = ClientDanhMuc.SuaThongTinTanSuat(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Xử lý dữ liệu trả về
            if (response == null)
            {
                return null;
            }
            else
            {
                return response.obj;
            }
        }

        public bool XoaTanSuat(int[] listID,
            ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.UtilitiesService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            DCTanSuatRequest request = Common.Utilities.PrepareRequest(new DCTanSuatRequest());
            request.listID = listID;

            request.Module = DatabaseConstant.Module.DMDC;
            request.Function = DatabaseConstant.Function.DC_TSUAT;
            request.Action = DatabaseConstant.Action.XOA;

            DCTanSuatResponse response = ClientDanhMuc.XoaListTanSuatTheoID(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Xử lý dữ liệu trả về
            if (response == null)
            {
                return false;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return response.KetQua;
            }
        }

        public bool DuyetTanSuat(int[] listID,
            ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.UtilitiesService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            DCTanSuatRequest request = Common.Utilities.PrepareRequest(new DCTanSuatRequest());
            request.listID = listID.ToArray();

            request.Module = DatabaseConstant.Module.DMDC;
            request.Function = DatabaseConstant.Function.DC_TSUAT;
            request.Action = DatabaseConstant.Action.DUYET;

            DCTanSuatResponse response = ClientDanhMuc.SuaThongTinTanSuat(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Xử lý dữ liệu trả về
            if (response == null)
            {
                return false;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return response.KetQua;
            }
        }

        public bool ThoaiDuyetTanSuat(int[] listID,
            ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.UtilitiesService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            DCTanSuatRequest request = Common.Utilities.PrepareRequest(new DCTanSuatRequest());
            request.listID = listID.ToArray();

            request.Module = DatabaseConstant.Module.DMDC;
            request.Function = DatabaseConstant.Function.DC_TSUAT;
            request.Action = DatabaseConstant.Action.THOAI_DUYET;

            DCTanSuatResponse response = ClientDanhMuc.SuaThongTinTanSuat(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Xử lý dữ liệu trả về
            if (response == null)
            {
                return false;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return response.KetQua;
            }
        }

        public bool TuChoiTanSuat(int[] listID,
            ref List<ClientResponseDetail> listClientResponseDetail)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.UtilitiesService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            DCTanSuatRequest request = Common.Utilities.PrepareRequest(new DCTanSuatRequest());
            request.listID = listID.ToArray();

            request.Module = DatabaseConstant.Module.DMDC;
            request.Function = DatabaseConstant.Function.DC_TSUAT;
            request.Action = DatabaseConstant.Action.TU_CHOI_DUYET;

            DCTanSuatResponse response = ClientDanhMuc.SuaThongTinTanSuat(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            // Xử lý dữ liệu trả về
            if (response == null)
            {
                return false;
            }
            else
            {
                listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                return response.KetQua;
            }
        }

        #endregion

        #region Ty gia
        /// <summary>
        /// Khởi tạo service NhanSuService
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        private TyGiaServiceClient TyGiaServiceClient(ApplicationConstant.SystemService service)
        {
            EndpointAddress endpointAddress = Common.Utilities.getEndpointAddress(service.layGiaTri());
            BasicHttpBinding basicHttpBinding = Common.Utilities.getBasicHttpBinding(service.layGiaTri());
            TyGiaServiceClient Client = new TyGiaServiceClient(basicHttpBinding, endpointAddress);

            foreach (var operationDescription in Client.Endpoint.Contract.Operations)
            {
                DataContractSerializerOperationBehavior dcsob =
                    operationDescription.Behaviors.Find<DataContractSerializerOperationBehavior>();
                if (dcsob != null)
                {
                    dcsob.MaxItemsInObjectGraph = 2147483646;
                }
            }

            return Client;
        }

        /// <summary>
        /// Khởi tạo service TruyVanService
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        private TruyVanServiceClient TruyVanServiceClient(ApplicationConstant.SystemService service)
        {
            EndpointAddress endpointAddress = Common.Utilities.getEndpointAddress(service.layGiaTri());
            BasicHttpBinding basicHttpBinding = Common.Utilities.getBasicHttpBinding(service.layGiaTri());
            TruyVanServiceClient Client = new TruyVanServiceClient(basicHttpBinding, endpointAddress);

            foreach (var operationDescription in Client.Endpoint.Contract.Operations)
            {
                DataContractSerializerOperationBehavior dcsob =
                    operationDescription.Behaviors.Find<DataContractSerializerOperationBehavior>();
                if (dcsob != null)
                {
                    dcsob.MaxItemsInObjectGraph = 2147483646;
                }
            }

            return Client;
        }

        public bool TyGia(DatabaseConstant.Action action, ref TY_GIA_CT obj, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            TyGiaServiceClient client = null;
            TyGiaRequest request = null;
            TyGiaResponse response = null; 
            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TyGiaService.layGiaTri());

                client = TyGiaServiceClient(ApplicationConstant.SystemService.TyGiaService);
                request = Common.Utilities.PrepareRequest(new TyGiaServiceRef.TyGiaRequest());
                response = new TyGiaServiceRef.TyGiaResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_HO_SO_CT;
                request.Action = action;
                request.objTyGiaCT = obj;

                // make a call to service client here
                response = client.TyGia(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null)
                {
                    obj = response.objTyGiaCT;
                    listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                if (client.State == CommunicationState.Faulted)
                {
                    client.Abort();
                }
                else
                {
                    client.Close();
                }

                client = null;
                request = null;
                response = null;
            }
        }
        #endregion

        #region Tổ chức tín dụng

        public int ToChucTinDung(DatabaseConstant.Function function, DatabaseConstant.Action action, ref DM_TO_CHUC_TIN_DUNG obj, ref List<ClientResponseDetail> lstResponseDetail)
        {
            DMToChucTinDungRequest request = null;
            DMToChucTinDungResponse response = null;
            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungTDService.layGiaTri());

                request = Common.Utilities.PrepareRequest(new DanhMucServiceRef.DMToChucTinDungRequest());
                response = new DanhMucServiceRef.DMToChucTinDungResponse();

                //Khởi tạo request
                request.Function = function;
                request.Action = action;
                request.objToChucTinDung = obj;

                // make a call to service client here
                response = ClientDanhMuc.ToChucTinDung(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                lstResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    obj = response.objToChucTinDung;
                    return 1;
                }
                else
                    return 0;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                request = null;
                response = null;
            }
        }

        public bool DanhSachToChucTinDung(DatabaseConstant.Function function, DatabaseConstant.Action action, ref List<DM_TO_CHUC_TIN_DUNG> lstToChucTinDung, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            DMToChucTinDungRequest request = null;
            DMToChucTinDungResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungTTService.layGiaTri());

                request = Common.Utilities.PrepareRequest(new DanhMucServiceRef.DMToChucTinDungRequest());
                response = new DanhMucServiceRef.DMToChucTinDungResponse();

                //Khởi tạo request
                request.Function = function;
                request.Action = action;
                request.lstToChucTinDung = lstToChucTinDung.ToArray();

                // make a call to service client here
                response = ClientDanhMuc.ToChucTinDung(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    lstToChucTinDung = response.lstToChucTinDung.ToList();
                    return true;
                }
                else
                {
                    listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                    return false;
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                request = null;
                response = null;
            }
        }

        public DataSet GetDanhSachToChucTinDung(DataTable dt)
        {
            TruyVanRequest request = null;
            TruyVanResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

                request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());
                response = new TruyVanServiceRef.TruyVanResponse();

                request.dtThamSo = dt;
                request.objectName = "INQ.DS.TO_CHUC_TIN_DUNG";
                request.inquiryName = "DANH_SACH";

                // make a call to service client here
                response = ClientTruyVan.TruyVanMessage(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                DataSet ds = response.dsResult;
                return ds;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                throw ex;
            }
            finally
            {
                
                request = null;
                response = null;
            }
        }

        #endregion
    }
}
