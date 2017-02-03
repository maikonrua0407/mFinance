using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text;
using Utilities.Common;
using System.ServiceModel;
using Presentation.Process.TruyVanServiceRef;
using System.ServiceModel.Description;
using Presentation.Process.TaiSanServiceRef;
using Presentation.Process.Common;
namespace Presentation.Process
{
    public class TaiSanProcess
    {
        /// <summary>
        /// Biến cục bộ chứa dịch vụ tiện ích của server
        /// </summary>
        private static TaiSanServiceClient Client { get; set; }
        private static TruyVanServiceClient ClientTruyVan { get; set; }
        /// <summary>
        /// Phương thức khởi tạo
        /// </summary>
        public TaiSanProcess()
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

            EndpointAddress endpointAddress = Common.Utilities.getEndpointAddress(ApplicationConstant.SystemService.TaiSanService.layGiaTri());
            BasicHttpBinding basicHttpBinding = Common.Utilities.getBasicHttpBinding(ApplicationConstant.SystemService.TaiSanService.layGiaTri());
            Client = new TaiSanServiceClient(basicHttpBinding, endpointAddress);

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

        public int functionName(int id)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TaiSanService.layGiaTri());

            TaiSanServiceRef.TaiSanRequest request = Common.Utilities.PrepareRequest(new TaiSanServiceRef.TaiSanRequest());
            TaiSanServiceRef.TaiSanResponse response = new TaiSanServiceRef.TaiSanResponse();
            try
            {
                // make a call to service client here
                response = Client.functionName(request);                
            }
            catch(Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
            return 0;
        }

        public bool NhomTaiSanCT(DatabaseConstant.Action action, ref TS_DM_NHOM_TSCD objNhomTSanCT, ref List<KT_PHAN_HE_PLOAI> lstPLoai, string sMaNhomCha, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            //Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TaiSanService.layGiaTri());
            TaiSanRequest request = Common.Utilities.PrepareRequest(new TaiSanServiceRef.TaiSanRequest());
            TaiSanResponse response = new TaiSanServiceRef.TaiSanResponse();
            if (lstPLoai == null) lstPLoai = new List<KT_PHAN_HE_PLOAI>();
            try
            {
                //Khoi tao request
                request.Function = DatabaseConstant.Function.TS_DM_NHOM_TS_CT;
                request.Action = action;
                request.objNhomTSCD = objNhomTSanCT;
                request.lstPLoai = lstPLoai.ToArray();
                request.sMaNhomCha = sMaNhomCha;           
                
                // make a call to service client
                response = Client.NhomTaiSanCT(request);

                //Kiem tra ket qua tra ve
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    objNhomTSanCT = response.objNhomTSCD;
                    if (response.lstPLoai != null) lstPLoai = response.lstPLoai.ToList();
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
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                return false;                
            }          
        }

        public DataSet GetTreeDonVi(string sIDDonVi)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@ID_DON_VI", "STRING", sIDDonVi);


            request.dtThamSo = dt;
            request.objectName = "INQ.DS.QLTS_DUNG_CHUNG";
            request.inquiryName = "TREE_DVI_PGD";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet GetTreeViewNhomTaiSan()
        {
            //Kiem tra ket noi server, service truoc khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());
            TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanRequest());
            TruyVanResponse response = new TruyVanResponse();
            DataSet dsNhomTaisan = null;
            try
            {
                DataTable dtThamSo = null;
                LDatatable.MakeParameterTable(ref dtThamSo);
                LDatatable.AddParameter(ref dtThamSo, "", "", "");
                request.dtThamSo = dtThamSo;
                request.objectName = "INQ.TS_PLOAI_TS";
                request.inquiryName = "TREE_PLOAI_TS";

                //make a call to service client
                response = ClientTruyVan.TruyVanMessage(request);

                //kiem tra ket qua tra ve
                Common.Utilities.ValidResponse(request, response);
                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    dsNhomTaisan = response.dsResult;
                }
                return dsNhomTaisan;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                return null;
            }
        }

        public DataSet LayDanhSachNhomTaiSan(DataTable dtThamso)
        {
            DataSet dsNhomTS = new DataSet();
            //Kiem tra ket noi server, service
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());
            TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanRequest());
            TruyVanResponse response = new TruyVanResponse();
            try
            {
                
                request.dtThamSo = dtThamso;
                request.objectName = "INQ.TS.DS_NHOM_TS";
                request.inquiryName = "DS_NHOM_TS";

                //make a call service client
                response = ClientTruyVan.TruyVanMessage(request);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    dsNhomTS = response.dsResult;
                }
                return dsNhomTS;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                return null;
            }
        }

        public DataSet LayDanhSachNhomTaiSan()
        {
            DataSet dsNhomTS = new DataSet();
            //Kiem tra ket noi server, service
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());
            TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanRequest());
            TruyVanResponse response = new TruyVanResponse();
            try
            {
                DataTable dtThamso = null;
                LDatatable.MakeParameterTable(ref dtThamso);
                request.dtThamSo = dtThamso;
                request.objectName = "INQ.TS.DS_NHOM_TS";
                request.inquiryName = "DS_NHOM_BCAO";

                //make a call service client
                response = ClientTruyVan.TruyVanMessage(request);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    dsNhomTS = response.dsResult;
                }
                return dsNhomTS;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                return null;
            }
        }

        /// <summary>
        /// Lay object nhom tai san cha
        /// </summary>
        /// <param name="objNhomTSanCha"></param>
        /// <returns></returns>
        public int GetNhomTSanCha(ref TS_DM_NHOM_TSCD objNhomTSanCha)
        {
            int kq = 1;
            //Kiem tra ket noi server, service
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TaiSanService.layGiaTri());
            TaiSanRequest request = Common.Utilities.PrepareRequest(new TaiSanRequest());
            TaiSanResponse response = new TaiSanResponse();
            try
            {
                //Khoi tao request
                request.objNhomTSCha = objNhomTSanCha;
                request.Function = DatabaseConstant.Function.TS_DM_NHOM_TS_GET_NHOM_CHA;

                //Make a call service client
                response = Client.NhomTaiSanCT(request);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    objNhomTSanCha = response.objNhomTSCha;
                }
                else
                {
                    kq = 0;
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                kq = 0;
            }
            return kq;
        }

        public bool DanhSachNhomTaiSan(ref List<TS_DM_NHOM_TSCD> lstNhomTSan, ref List<ClientResponseDetail> lstClientResponseDetail,DatabaseConstant.Action action,string sTThaiNVu)
        {
            //Kiem tra ket noi server, service
            bool kq = true;
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TaiSanService.layGiaTri());
            TaiSanRequest request = Common.Utilities.PrepareRequest(new TaiSanRequest());
            TaiSanResponse response = new TaiSanResponse();
            try
            {
                //Khoi tao request
                request.Function = DatabaseConstant.Function.TS_DM_NHOM_TS_DS;
                request.lstNhomTSan = lstNhomTSan.ToArray();
                request.Action = action;
                request.sMaNhomCha = sTThaiNVu;

                //make a call to client
                response = Client.NhomTaiSanCT(request);

                //Kiem tra ket qua tra ve
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    kq = true;
                }
                else
                {
                    kq = false;
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                kq = false;
            }
            return kq;
        }

        public bool QuanLyTaiSan(DatabaseConstant.Function function, DatabaseConstant.Action action, List<int> lstID, ref TAI_SAN_DTO obj, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            //Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TaiSanService.layGiaTri());
            TaiSanRequest request = Common.Utilities.PrepareRequest(new TaiSanServiceRef.TaiSanRequest());
            TaiSanResponse response = new TaiSanServiceRef.TaiSanResponse();
            try
            {
                //Khoi tao request
                request.Function = function;
                request.Action = action;
                request.objTangTS_Dto = obj;
                request.lstID = lstID.ToArray();

                // make a call to service client
                response = Client.QuanLyTaiSan(request);

                //Kiem tra ket qua tra ve
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    obj = response.objTangTS_Dto;
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
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                return false;
            }
        }

        public bool ThongTinTaiSan(DatabaseConstant.Function function, DatabaseConstant.Action action, List<int> lstID, ref TS_TAI_SAN_DTO obj, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            //Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TaiSanService.layGiaTri());
            TaiSanRequest request = Common.Utilities.PrepareRequest(new TaiSanServiceRef.TaiSanRequest());
            TaiSanResponse response = new TaiSanServiceRef.TaiSanResponse();
            try
            {
                //Khoi tao request
                request.Function = function;
                request.Action = action;
                request.objTS_Dto = obj;
                request.lstID = lstID.ToArray();

                // make a call to service client
                response = Client.QuanLyTaiSan(request);

                //Kiem tra ket qua tra ve
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    obj = response.objTS_Dto;
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
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                return false;
            }
        }

        public bool TangNguyenGia(DatabaseConstant.Action action, ref List<TS_TANG_NG> lstObj, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            //Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TaiSanService.layGiaTri());
            TaiSanRequest request = Common.Utilities.PrepareRequest(new TaiSanServiceRef.TaiSanRequest());
            TaiSanResponse response = new TaiSanServiceRef.TaiSanResponse();
            try
            {
                //Khoi tao request
                request.Function = DatabaseConstant.Function.TS_TANG_NG;
                request.Action = action;
                request.lstTangNG = lstObj.ToArray();

                // make a call to service client
                response = Client.QuanLyTaiSan(request);

                //Kiem tra ket qua tra ve
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    lstObj = response.lstTangNG.ToList();
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
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                return false;
                
            }
        }

        public bool LayLoaiDanhMucTaiSan(ref List<TS_DM_DMUC_LOAI> lstLoaiDanhMuc)
        {
            //Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TaiSanService.layGiaTri());
            TaiSanRequest request = Common.Utilities.PrepareRequest(new TaiSanServiceRef.TaiSanRequest());
            TaiSanResponse response = new TaiSanServiceRef.TaiSanResponse();
            try
            {
                //Khoi tao request
                request.lstDanhMucLoai = lstLoaiDanhMuc.ToArray();

                // make a call to service client
                response = Client.LayDanhMucLoai(request);

                //Kiem tra ket qua tra ve
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    lstLoaiDanhMuc = response.lstDanhMucLoai.ToList();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                return false;
                
            }
        }

        public bool LayDanhMucTaiSanTheoLoai(ref List<DMUC_TSAN_DTO> lstDanhMucDto, List<DatabaseConstant.LOAI_DMUC_TSAN> lstMaLoai)
        {
            //Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TaiSanService.layGiaTri());
            TaiSanRequest request = Common.Utilities.PrepareRequest(new TaiSanServiceRef.TaiSanRequest());
            TaiSanResponse response = new TaiSanServiceRef.TaiSanResponse();
            
            try
            {
                //Khoi tao request
                request.lstMaLoai = lstMaLoai.ToArray();
                request.lstDanhMucDto = lstDanhMucDto.ToArray();
                

                // make a call to service client
                response = Client.LayDanhMucTheoLoai(request);

                //Kiem tra ket qua tra ve
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    lstDanhMucDto = response.lstDanhMucDto.ToList();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                return false; 
                
            }
        }

        public bool LayNhomTaiSanNhoNhat(ref List<TS_DM_NHOM_TSCD> lstNhomTS)
        {
            //Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TaiSanService.layGiaTri());
            TaiSanRequest request = Common.Utilities.PrepareRequest(new TaiSanServiceRef.TaiSanRequest());
            TaiSanResponse response = new TaiSanServiceRef.TaiSanResponse();
            
            try
            {
                //Khoi tao request
                request.lstNhomTSan = lstNhomTS.ToArray();
                

                // make a call to service client
                response = Client.LayNhomTaiSanNhoNhat(request);

                //Kiem tra ket qua tra ve
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    lstNhomTS = response.lstNhomTSan.ToList();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                return false; 
                
            }
        }

        public DataSet GetDanhSachTaiSanTang(string sMaDonVi, string sTrangThaiNVu, string sMaTS, string sTenTS,
                                         string sNhomTS, string dNguyenGiaTu, string dNguyenGiaDen, string sNgayNhapTu, string sNgayNhapDen,
                                         string sNgaySDTu, string sNgaySDDen, string sMaNguoiNhan, string sTenNguoiNhan,
                                         string sStartRow = "0", string sEndRow = "0")
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@INP_MA_DON_VI", "STRING", sMaDonVi);
            LDatatable.AddParameter(ref dt, "@INP_TTHAI_NVU", "STRING", sTrangThaiNVu);
            LDatatable.AddParameter(ref dt, "@INP_MA_TAI_SAN", "STRING", sMaTS);
            LDatatable.AddParameter(ref dt, "@INP_TEN_TAI_SAN", "STRING", sTenTS);
            LDatatable.AddParameter(ref dt, "@INP_NHOM_TAI_SAN", "STRING", sNhomTS);
            LDatatable.AddParameter(ref dt, "@INP_NGUYEN_GIA_TU", "STRING", dNguyenGiaTu);
            LDatatable.AddParameter(ref dt, "@INP_NGUYEN_GIA_DEN", "STRING", dNguyenGiaDen);
            LDatatable.AddParameter(ref dt, "@INP_NGAY_NHAP_TU", "STRING", sNgayNhapTu);
            LDatatable.AddParameter(ref dt, "@INP_NGAY_NHAP_DEN", "STRING", sNgayNhapDen);
            LDatatable.AddParameter(ref dt, "@INP_NGAY_SDUNG_TU", "STRING", sNgaySDTu);
            LDatatable.AddParameter(ref dt, "@INP_NGAY_SDUNG_DEN", "STRING", sNgaySDDen);
            LDatatable.AddParameter(ref dt, "@INP_MA_NGUOI_NHAN", "STRING", sMaNguoiNhan);
            LDatatable.AddParameter(ref dt, "@INP_TEN_NGUOI_NHAN", "STRING", sTenNguoiNhan);
            LDatatable.AddParameter(ref dt, "@INP_START_ROW", "INT", sStartRow);
            LDatatable.AddParameter(ref dt, "@INP_END_ROW", "INT", sEndRow);

            request.dtThamSo = dt;
            request.objectName = "INQ.DS.TS_TANG.DANH_SACH";
            request.inquiryName = "%";

            // Lấy kết quả trả về
            response = ClientTruyVan.TruyVanMessage(request);
            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }
        public DataSet GetDanhSachTaiBanGiao(string sMaDonVi, string sTrangThaiNVu, string sMaTS, string sTenTS,
                                         string sSoBienBan,string sNgaySDTu, string sNgaySDDen, string sMaDonViSD, string sTenDonViSD,
                                         string sStartRow = "0", string sEndRow = "0")
        {
            //Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TaiSanService.layGiaTri());
            TaiSanRequest request = Common.Utilities.PrepareRequest(new TaiSanServiceRef.TaiSanRequest());
            TaiSanResponse response = new TaiSanServiceRef.TaiSanResponse();
            
            try
            {
                //Khoi tao request
                request.Function = DatabaseConstant.Function.TS_BAN_GIAO_DS;
                request.Action = DatabaseConstant.Action.XEM;

                // make a call to service client
                response = Client.QuanLyTaiSan(request);

                //Kiem tra ket qua tra ve
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    return new DataSet();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {                
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                return null;
            }
        }

        public bool BanGiaoTaiSan(DatabaseConstant.Function function, DatabaseConstant.Action action, List<int> lstID, DIEU_KIEN_TIM_KIEM_DTO dieuKien, ref BAN_GIAO_DTO obj, ref List<DANH_SACH_BAN_GIAO_DTO> lstDanhSach, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            //Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TaiSanService.layGiaTri());
            TaiSanRequest request = Common.Utilities.PrepareRequest(new TaiSanServiceRef.TaiSanRequest());
            TaiSanResponse response = new TaiSanServiceRef.TaiSanResponse();
            
            try
            {
                //Khoi tao request
                request.Function = function;
                request.Action = action;
                request.dieuKien = dieuKien;
                request.objBanGiaodto = obj;
                request.dsBanGiaodto = lstDanhSach.ToArray();
                
                request.lstID = lstID.ToArray();

                // make a call to service client
                response = Client.BanGiaoTaiSan(request);

                //Kiem tra ket qua tra ve
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    if (!response.objBanGiaodto.IsNullOrEmpty())
                        obj = response.objBanGiaodto;
                    if (!response.dsBanGiaodto.IsNullOrEmpty())
                        lstDanhSach = response.dsBanGiaodto.ToList();
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
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                return false;
            }
        }

        public bool NangCapTaiSan(DatabaseConstant.Function function, DatabaseConstant.Action action, List<int> lstID, DIEU_KIEN_TIM_KIEM_DTO dieuKien, ref NANG_CAP_DTO obj, ref List<DANH_SACH_NANG_CAP_DTO> lstDanhSach, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            //Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TaiSanService.layGiaTri());
            TaiSanRequest request = Common.Utilities.PrepareRequest(new TaiSanServiceRef.TaiSanRequest());
            TaiSanResponse response = new TaiSanServiceRef.TaiSanResponse();
            
            try
            {
                //Khoi tao request
                request.Function = function;
                request.Action = action;
                request.dieuKien = dieuKien;
                request.objNangCapdto = obj;
                request.dsNangCapdto = lstDanhSach.ToArray();
                
                request.lstID = lstID.ToArray();

                // make a call to service client
                response = Client.NangCapTaiSan(request);

                //Kiem tra ket qua tra ve
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    if (!response.objNangCapdto.IsNullOrEmpty())
                        obj = response.objNangCapdto;
                    if (!response.dsNangCapdto.IsNullOrEmpty())
                        lstDanhSach = response.dsNangCapdto.ToList();
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
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                return false;
            }
        }

        public bool DanhGiaTaiSan(DatabaseConstant.Function function, DatabaseConstant.Action action, List<int> lstID, DIEU_KIEN_TIM_KIEM_DTO dieuKien, ref DANH_GIA_DTO obj, ref List<DANH_SACH_DANH_GIA_DTO> lstDanhSach, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            //Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TaiSanService.layGiaTri());
            TaiSanRequest request = Common.Utilities.PrepareRequest(new TaiSanServiceRef.TaiSanRequest());
            TaiSanResponse response = new TaiSanServiceRef.TaiSanResponse();
            
            try
            {
                //Khoi tao request
                request.Function = function;
                request.Action = action;
                request.dieuKien = dieuKien;
                request.objDanhGiaDto = obj;
                request.dsDanhGiaDto = lstDanhSach.ToArray();
                
                request.lstID = lstID.ToArray();

                // make a call to service client
                response = Client.DanhGiaTaiSan(request);

                //Kiem tra ket qua tra ve
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    if (!response.objDanhGiaDto.IsNullOrEmpty())
                        obj = response.objDanhGiaDto;
                    if (!response.dsDanhGiaDto.IsNullOrEmpty())
                        lstDanhSach = response.dsDanhGiaDto.ToList();
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
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                return false;
            }
        }

        public bool GiamTaiSan(DatabaseConstant.Function function, DatabaseConstant.Action action, List<int> lstID, DIEU_KIEN_TIM_KIEM_DTO dieuKien, ref GIAM_DTO obj, ref List<DANH_SACH_GIAM_DTO> lstDanhSach, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            //Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TaiSanService.layGiaTri());
            TaiSanRequest request = Common.Utilities.PrepareRequest(new TaiSanServiceRef.TaiSanRequest());
            TaiSanResponse response = new TaiSanServiceRef.TaiSanResponse();
            
            try
            {
                //Khoi tao request
                request.Function = function;
                request.Action = action;
                request.dieuKien = dieuKien;
                request.objGiamDto = obj;
                request.dsGiamDto = lstDanhSach.ToArray();
                
                request.lstID = lstID.ToArray();

                // make a call to service client
                response = Client.GiamTaiSan(request);

                //Kiem tra ket qua tra ve
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    if (!response.objGiamDto.IsNullOrEmpty())
                        obj = response.objGiamDto;
                    if (!response.dsGiamDto.IsNullOrEmpty())
                        lstDanhSach = response.dsGiamDto.ToList();
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
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                return false;
            }
        }

        public bool KhauHaoTaiSan(DatabaseConstant.Function function, DatabaseConstant.Action action, List<int> lstID, DIEU_KIEN_TIM_KIEM_DTO dieuKien, ref KHAU_HAO_DTO obj, ref List<DANH_SACH_KHAU_HAO_DTO> lstDanhSach, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            //Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TaiSanService.layGiaTri());
            TaiSanRequest request = Common.Utilities.PrepareRequest(new TaiSanServiceRef.TaiSanRequest());
            TaiSanResponse response = new TaiSanServiceRef.TaiSanResponse();
            
            try
            {
                //Khoi tao request
                request.Function = function;
                request.Action = action;
                request.dieuKien = dieuKien;
                request.objKhauHaoDto = obj;
                request.dsKhauHaoDto = lstDanhSach.ToArray();
                
                request.lstID = lstID.ToArray();

                // make a call to service client
                response = Client.KhauHaoTaiSan(request);

                //Kiem tra ket qua tra ve
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    if (!response.objKhauHaoDto.IsNullOrEmpty())
                        obj = response.objKhauHaoDto;
                    if (!response.dsKhauHaoDto.IsNullOrEmpty())
                        lstDanhSach = response.dsKhauHaoDto.ToList();
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
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                return false;
            }
        }

        public bool LayThongTinGiaoDich(ref KIEM_SOAT objKiemSoat, BIEN_DONG_DTO objBienDongDto)
        {
            //Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TaiSanService.layGiaTri());
            TaiSanRequest request = Common.Utilities.PrepareRequest(new TaiSanServiceRef.TaiSanRequest());
            TaiSanResponse response = new TaiSanServiceRef.TaiSanResponse();

            try
            {
                //Khoi tao request
                request.objBienDongDto = objBienDongDto;

                // make a call to service client
                response = Client.LayThongTinGiaoDich(request);

                //Kiem tra ket qua tra ve
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    objKiemSoat = response.objKiemSoat;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                return false;
            }
        }
    }
}
