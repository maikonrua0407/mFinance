using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities.Common;
using Presentation.Process.TinDungTDServiceRef;
using Presentation.Process.TruyVanServiceRef;
using System.ServiceModel;
using System.ServiceModel.Description;
using Presentation.Process.Common;
using System.Data;

namespace Presentation.Process
{
    public class TinDungTDProcess
    {
        /// <summary>
        /// Khởi tạo service TinDungTThuongService
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        private TinDungTDServiceClient TinDungTDServiceClient(ApplicationConstant.SystemService service)
        {
            EndpointAddress endpointAddress = Common.Utilities.getEndpointAddress(service.layGiaTri());
            BasicHttpBinding basicHttpBinding = Common.Utilities.getBasicHttpBinding(service.layGiaTri());
            TinDungTDServiceClient Client = new TinDungTDServiceClient(basicHttpBinding, endpointAddress);

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

        #region Sản phẩm tín dụng
        /// <summary>
        /// Lưu sản phẩm tín dụng
        /// </summary>
        /// <param name="objSanPham"></param>
        /// <returns></returns>
        public int SanPhamTinDung(DatabaseConstant.Action action, ref List<TinDungTDServiceRef.TDTD_SAN_PHAM> objSanPham, 
                                    ref List<Presentation.Process.TinDungTDServiceRef.KT_PHAN_HE_PLOAI> listPhanLoai, 
                                    ref List<ClientResponseDetail> listClientResponseDetail)
        {
            TinDungTDServiceClient client = null;
            TinDungTDRequest request = null;
            TinDungTDResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungTDService.layGiaTri());

                client = TinDungTDServiceClient(ApplicationConstant.SystemService.TinDungTDService);
                request = Common.Utilities.PrepareRequest(new TinDungTDServiceRef.TinDungTDRequest());
                response = new TinDungTDServiceRef.TinDungTDResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.TDTD_SAN_PHAM;
                request.Action = action;
                request.lstSanPham = objSanPham.ToArray();
                request.lstPhanLoai = listPhanLoai.ToArray();

                // make a call to service client here
                response = client.TinDungTieuDung(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response == null)
                {
                    return 0;
                }
                else
                {
                    listClientResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                    objSanPham = response.lstSanPham.ToList();
                    return response.iKetQua;
                }
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

        public DataSet GetDSSanPhamTinDungTieuDung(DataTable dt)
        {
            // Kiểm tra kết nối, server, service trước khi request
            TruyVanServiceClient client = null;
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            client = TruyVanServiceClient(ApplicationConstant.SystemService.TruyVanService);

            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();

            request.dtThamSo = dt;
            request.inquiryName = "DANH_SACH";
            request.objectName = "INQ.DS.TDTD_SAN_PHAM";
            request.typePara = "UDTT";
            request.type = "Multi";
            // Lấy kết quả trả về
            response = client.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet getSanPhamTDByID(DataTable dt)
        {
            // Kiểm tra kết nối, server, service trước khi request
            TruyVanServiceClient client = null;
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            client = TruyVanServiceClient(ApplicationConstant.SystemService.TruyVanService);

            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();

            request.dtThamSo = dt;
            request.inquiryName = "CHI_TIET";
            request.objectName = "INQ.DS.TDTD_SAN_PHAM";
            request.typePara = "UDTT";
            request.type = "Multi";
            // Lấy kết quả trả về
            response = client.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet GetTaiKhoanHachToan(string maDoiTuong, string maDonVi)
        {
            // Kiểm tra kết nối, server, service trước khi request
            TruyVanServiceClient client = null;
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            client = TruyVanServiceClient(ApplicationConstant.SystemService.TruyVanService);

            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@MA_PHAN_HE", "STRING", DatabaseConstant.Module.TDTD.getValue());
            LDatatable.AddParameter(ref dt, "@MA_DTUONG", "STRING", maDoiTuong);
            LDatatable.AddParameter(ref dt, "@MA_DON_VI", "STRING", maDonVi);

            request.dtThamSo = dt;
            request.objectName = DatabaseConstant.DanhSachTruyVanTinDung.INQ_CT_TD_SAN_PHAM.getValue();
            request.inquiryName = "TAI_KHOAN_HACH_TOAN";

            // Lấy kết quả trả về
            response = client.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }
        #endregion

        #region Đơn xin vay vốn
        public int DonXinVayVonTinDung(DatabaseConstant.Action action, ref TinDungTDServiceRef.TDTD_DON_XIN_VAY_VON obj, ref List<ClientResponseDetail> lstResponseDetail)
        {
            TinDungTDServiceClient client = null;
            TinDungTDRequest request = null;
            TinDungTDResponse response = null;
            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungTDService.layGiaTri());

                client = TinDungTDServiceClient(ApplicationConstant.SystemService.TinDungTDService);
                request = Common.Utilities.PrepareRequest(new TinDungTDServiceRef.TinDungTDRequest());
                response = new TinDungTDServiceRef.TinDungTDResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.TDTD_DON_XIN_VAY_VON;
                request.Action = action;
                request.objDonXinVayVon = obj;

                // make a call to service client here
                response = client.TinDungTieuDung(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                lstResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    obj = response.objDonXinVayVon;
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

        public DataSet LayDSDonVayVonBIDV(int idKhachHang)
        {
            // Kiểm tra kết nối, server, service trước khi request
            TruyVanServiceClient client = null;
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            client = TruyVanServiceClient(ApplicationConstant.SystemService.TruyVanService);
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();

            DataTable dt = null;
            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@ID_KHACH_HANG", "string", idKhachHang.ToString());

            request.dtThamSo = dt;
            request.objectName = "INQ.DS_DON_XIN_VAY_VON_TIEU_DUNG";
            request.inquiryName = "DSDXVVTD";

            // Lấy kết quả trả về
            response = client.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet GetDSDonXinVayVonTinDungTieuDung(DataTable dt)
        {
            // Kiểm tra kết nối, server, service trước khi request
            TruyVanServiceClient client = null;
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            client = TruyVanServiceClient(ApplicationConstant.SystemService.TruyVanService);

            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();

            request.dtThamSo = dt;
            request.inquiryName = "%";
            request.objectName = "INQ.DS.TDVM_DON_XIN_VAY_VON_TIEU_DUNG";
            request.typePara = "UDTT";
            request.type = "Multi";
            // Lấy kết quả trả về
            response = client.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet GetThongTinDonXinVayVonTinDungTieuDung(DataTable dt)
        {
            // Kiểm tra kết nối, server, service trước khi request
            TruyVanServiceClient client = null;
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            client = TruyVanServiceClient(ApplicationConstant.SystemService.TruyVanService);
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();

            request.dtThamSo = dt;
            request.inquiryName = "%";
            request.objectName = "INQ.CT.TDTD_DON_XIN_VAY_VON";
            request.typePara = "UDTT";
            request.type = "Multi";
            // Lấy kết quả trả về
            response = client.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }
        #endregion

        #region Hop dong tin dung ca nhan
        public int HopDongTinDungCaNhan(DatabaseConstant.Action action, ref TD_HDTD_TD obj, ref List<ClientResponseDetail> lstResponseDetail)
        {
            TinDungTDServiceClient client = null;
            TinDungTDRequest request = null;
            TinDungTDResponse response = null;
            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungTDService.layGiaTri());

                client = TinDungTDServiceClient(ApplicationConstant.SystemService.TinDungTDService);
                request = Common.Utilities.PrepareRequest(new TinDungTDServiceRef.TinDungTDRequest());
                response = new TinDungTDServiceRef.TinDungTDResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.TDTD_HOP_DONG_CA_NHAN;
                request.Action = action;
                request.objHopDongCN = obj;

                // make a call to service client here
                response = client.TinDungTieuDung(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                lstResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    obj = response.objHopDongCN;
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

        public DataSet HopDongTinDungCaNhanChiTiet(DataTable dt)
        {
            // Kiểm tra kết nối, server, service trước khi request
            TruyVanServiceClient client = null;
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            client = TruyVanServiceClient(ApplicationConstant.SystemService.TruyVanService);
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();

            request.dtThamSo = dt;
            request.inquiryName = "%";
            request.objectName = "INQ.CT.TDTD_HOP_DONG_TIN_DUNG_CA_NHAN";
            request.typePara = "UDTT";
            request.type = "Multi";
            // Lấy kết quả trả về
            response = client.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet HopDongTinDungCaNhanDanhSach(DataTable dt)
        {
            // Kiểm tra kết nối, server, service trước khi request
            TruyVanServiceClient client = null;
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            client = TruyVanServiceClient(ApplicationConstant.SystemService.TruyVanService);
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();

            request.dtThamSo = dt;
            request.inquiryName = "%";
            request.objectName = "INQ.DS.TDTD_HOP_DONG_TIN_DUNG_CA_NHAN";
            request.typePara = "UDTT";
            request.type = "Multi";
            // Lấy kết quả trả về
            response = client.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }
        #endregion

        #region Khe uoc tin dung tieu dung
        public int KheUocTieuDung(DatabaseConstant.Action action, ref TDTD_KHE_UOC obj, ref List<ClientResponseDetail> lstResponseDetail)
        {
            TinDungTDServiceClient client = null;
            TinDungTDRequest request = null;
            TinDungTDResponse response = null;
            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungTDService.layGiaTri());

                client = TinDungTDServiceClient(ApplicationConstant.SystemService.TinDungTDService);
                request = Common.Utilities.PrepareRequest(new TinDungTDServiceRef.TinDungTDRequest());
                response = new TinDungTDServiceRef.TinDungTDResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.TDTD_KHE_UOC;
                request.Action = action;
                request.objKheUoc = obj;

                // make a call to service client here
                response = client.TinDungTieuDung(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                lstResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    obj = response.objKheUoc;
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

        public bool KheUocTieuDungDanhSach(DatabaseConstant.Action action, ref List<TDTD_KHE_UOC> lst, ref List<ClientResponseDetail> lstResponseDetail)
        {
            TinDungTDServiceClient client = null;
            TinDungTDRequest request = null;
            TinDungTDResponse response = null;
            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungTDService.layGiaTri());

                client = TinDungTDServiceClient(ApplicationConstant.SystemService.TinDungTDService);
                request = Common.Utilities.PrepareRequest(new TinDungTDServiceRef.TinDungTDRequest());
                response = new TinDungTDServiceRef.TinDungTDResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.TDTD_KHE_UOC;
                request.Action = action;
                request.lstKheUoc = lst.ToArray();

                // make a call to service client here
                response = client.TinDungTieuDung(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                lstResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    lst = response.lstKheUoc.ToList();
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

        public DataSet getDanhSachKUOC(DataTable dt)
        {
            // Kiểm tra kết nối, server, service trước khi request
            TruyVanServiceClient client = null;
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            client = TruyVanServiceClient(ApplicationConstant.SystemService.TruyVanService);

            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();

            request.dtThamSo = dt;
            request.inquiryName = "DANH_SACH";
            request.objectName = "INQ.DS.TDTD_KHE_UOC";
            request.typePara = "UDTT";
            request.type = "Multi";
            // Lấy kết quả trả về
            response = client.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet getThucThuKUOC(DataTable dt)
        {
            // Kiểm tra kết nối, server, service trước khi request
            TruyVanServiceClient client = null;
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            client = TruyVanServiceClient(ApplicationConstant.SystemService.TruyVanService);

            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();

            request.dtThamSo = dt;
            request.inquiryName = "THUC_THU";
            request.objectName = "INQ.CT.TDTD_KHE_UOC";
            request.typePara = "UDTT";
            request.type = "Multi";
            // Lấy kết quả trả về
            response = client.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet getDanhSachKUOCTheoKH(int idKhachHang)
        {
            // Kiểm tra kết nối, server, service trước khi request
            TruyVanServiceClient client = null;
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            client = TruyVanServiceClient(ApplicationConstant.SystemService.TruyVanService);
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();

            DataTable dt = null;
            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@ID_KHACH_HANG", "string", idKhachHang.ToString());

            request.dtThamSo = dt;
            request.objectName = "INQ.DS_HOP_DONG_KHE_UOC_TD";
            request.inquiryName = "DSKUOCTD";

            // Lấy kết quả trả về
            response = client.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }
        #endregion

        #region Giai ngan tin dung tieu dung
        public int GiaiNgan(DatabaseConstant.Action action, ref TDTD_GIAI_NGAN obj, ref List<ClientResponseDetail> lstResponseDetail)
        {
            TinDungTDServiceClient client = null;
            TinDungTDRequest request = null;
            TinDungTDResponse response = null;
            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungTDService.layGiaTri());

                client = TinDungTDServiceClient(ApplicationConstant.SystemService.TinDungTDService);
                request = Common.Utilities.PrepareRequest(new TinDungTDServiceRef.TinDungTDRequest());
                response = new TinDungTDServiceRef.TinDungTDResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.TDTD_GIAI_NGAN;
                request.Action = action;
                request.objGiaiNgan = obj;

                // make a call to service client here
                response = client.TinDungTieuDung(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                lstResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    obj = response.objGiaiNgan;
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

        public DataSet GetThongTinGiaiNgan(DataTable dt)
        {
            // Kiểm tra kết nối, server, service trước khi request
            TruyVanServiceClient client = null;
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            client = TruyVanServiceClient(ApplicationConstant.SystemService.TruyVanService);
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();

            request.dtThamSo = dt;
            request.inquiryName = "%";
            request.objectName = "INQ.CT.TDTD_GIAI_NGAN";
            request.typePara = "UDTT";
            request.type = "Multi";
            // Lấy kết quả trả về
            response = client.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }
        #endregion

        #region Giai ngan tin dung tieu dung qua dai ly
        public int GiaiNganDaiLy(DatabaseConstant.Action action, ref TDTD_GIAI_NGAN_DAI_LY obj, ref List<ClientResponseDetail> lstResponseDetail)
        {
            TinDungTDServiceClient client = null;
            TinDungTDRequest request = null;
            TinDungTDResponse response = null;
            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungTDService.layGiaTri());

                client = TinDungTDServiceClient(ApplicationConstant.SystemService.TinDungTDService);
                request = Common.Utilities.PrepareRequest(new TinDungTDServiceRef.TinDungTDRequest());
                response = new TinDungTDServiceRef.TinDungTDResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.TDTD_GIAI_NGAN_DAI_LY;
                request.Action = action;
                request.objGiaiNganDaiLy = obj;

                // make a call to service client here
                response = client.TinDungTieuDung(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                lstResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    obj = response.objGiaiNganDaiLy;
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

        public DataSet GetThongTinGiaiNganDaiLy(DataTable dt)
        {
            // Kiểm tra kết nối, server, service trước khi request
            TruyVanServiceClient client = null;
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            client = TruyVanServiceClient(ApplicationConstant.SystemService.TruyVanService);
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();

            request.dtThamSo = dt;
            request.inquiryName = "%";
            request.objectName = "INQ.CT.TDTD_GIAI_NGAN_DAI_LY";
            request.typePara = "UDTT";
            request.type = "Multi";
            // Lấy kết quả trả về
            response = client.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }
        #endregion

        #region Kiểm soát rủi ro
        public int KiemSoatRuiRo(DatabaseConstant.Action action, ref List<TD_KIEM_SOAT_RR> lst, ref List<ClientResponseDetail> lstResponseDetail)
        {
            TinDungTDServiceClient client = null;
            TinDungTDRequest request = null;
            TinDungTDResponse response = null;
            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungTDService.layGiaTri());

                client = TinDungTDServiceClient(ApplicationConstant.SystemService.TinDungTDService);
                request = Common.Utilities.PrepareRequest(new TinDungTDServiceRef.TinDungTDRequest());
                response = new TinDungTDServiceRef.TinDungTDResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.TDTD_KIEM_SOAT_RR;
                request.Action = action;
                request.lstKiemSoatRR = lst.ToArray();

                // make a call to service client here
                response = client.TinDungTieuDung(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                lstResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    lst = response.lstKiemSoatRR.ToList();
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

        public DataSet GetThongTinKheUoc(DataTable dt)
        {
            // Kiểm tra kết nối, server, service trước khi request
            TruyVanServiceClient client = null;
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            client = TruyVanServiceClient(ApplicationConstant.SystemService.TruyVanService);

            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();

            request.dtThamSo = dt;
            request.inquiryName = "TTIN_CHUNG";
            request.objectName = "INQ.CT.TDTD_KHE_UOC";
            request.typePara = "UDTT";
            request.type = "Multi";
            // Lấy kết quả trả về
            response = client.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet GetDanhSachKiemSoatRuiRo(DataTable dt)
        {
            // Kiểm tra kết nối, server, service trước khi request
            TruyVanServiceClient client = null;
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            client = TruyVanServiceClient(ApplicationConstant.SystemService.TruyVanService);

            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();

            request.dtThamSo = dt;
            request.inquiryName = "DANH_SACH";
            request.objectName = "INQ.DS.TDTD_KIEM_SOAT_RR";
            request.typePara = "UDTT";
            request.type = "Multi";
            // Lấy kết quả trả về
            response = client.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet GetThongTinKiemSoatRuiRo(DataTable dt)
        {
            // Kiểm tra kết nối, server, service trước khi request
            TruyVanServiceClient client = null;
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            client = TruyVanServiceClient(ApplicationConstant.SystemService.TruyVanService);

            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();

            request.dtThamSo = dt;
            request.inquiryName = "%";
            request.objectName = "INQ.CT.TDTD_KIEM_SOAT_RR";
            request.typePara = "UDTT";
            request.type = "Multi";
            // Lấy kết quả trả về
            response = client.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }
        #endregion

        #region Thu goc lai vay
        public int ThuGocLaiVay(DatabaseConstant.Action action, ref TDTD_THU_GOC_LAI_VAY obj, ref List<ClientResponseDetail> lstResponseDetail)
        {
            TinDungTDServiceClient client = null;
            TinDungTDRequest request = null;
            TinDungTDResponse response = null;
            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungTDService.layGiaTri());

                client = TinDungTDServiceClient(ApplicationConstant.SystemService.TinDungTDService);
                request = Common.Utilities.PrepareRequest(new TinDungTDServiceRef.TinDungTDRequest());
                response = new TinDungTDServiceRef.TinDungTDResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.TDTD_THU_GOC_LAI;
                request.Action = action;
                request.objThuGocLai = obj;

                // make a call to service client here
                response = client.TinDungTieuDung(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                lstResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    obj = response.objThuGocLai;
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

        public DataSet GetThongTinThuGocLaiVay(DataTable dt)
        {
            // Kiểm tra kết nối, server, service trước khi request
            TruyVanServiceClient client = null;
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            client = TruyVanServiceClient(ApplicationConstant.SystemService.TruyVanService);

            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();

            request.dtThamSo = dt;
            request.inquiryName = "%";
            request.objectName = "INQ.CT.TDTD_THU_GOC_LAI_VAY";
            request.typePara = "UDTT";
            request.type = "Multi";
            // Lấy kết quả trả về
            response = client.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet GetThongTinKeHoachThuGocLai(DataTable dt)
        {
            // Kiểm tra kết nối, server, service trước khi request
            TruyVanServiceClient client = null;
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            client = TruyVanServiceClient(ApplicationConstant.SystemService.TruyVanService);

            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            // Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());

            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();

            request.dtThamSo = dt;
            request.inquiryName = "KE_HOACH";
            request.objectName = "INQ.CT.TDTD_KE_HOACH_THU_GOC_LAI";
            request.typePara = "UDTT";
            request.type = "Multi";
            // Lấy kết quả trả về
            response = client.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }
        #endregion

        #region Chuyen no qua han
        public int ChuyenNoQuaHan(DatabaseConstant.Action action, ref TDTD_CHUYEN_NO_QHAN obj, ref List<ClientResponseDetail> lstResponseDetail)
        {
            TinDungTDServiceClient client = null;
            TinDungTDRequest request = null;
            TinDungTDResponse response = null;
            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungTDService.layGiaTri());

                client = TinDungTDServiceClient(ApplicationConstant.SystemService.TinDungTDService);
                request = Common.Utilities.PrepareRequest(new TinDungTDServiceRef.TinDungTDRequest());
                response = new TinDungTDServiceRef.TinDungTDResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.TDTD_CHUYEN_NO;
                request.Action = action;
                request.objChuyenNo = obj;

                // make a call to service client here
                response = client.TinDungTieuDung(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                lstResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    obj = response.objChuyenNo;
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

        #region Chuyen hoan nhom no
        public int ChuyenHoanNhomNo(DatabaseConstant.Action action, ref TDTD_CHUYEN_HOAN_NO obj, ref List<ClientResponseDetail> lstResponseDetail)
        {
            TinDungTDServiceClient client = null;
            TinDungTDRequest request = null;
            TinDungTDResponse response = null;
            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungTDService.layGiaTri());

                client = TinDungTDServiceClient(ApplicationConstant.SystemService.TinDungTDService);
                request = Common.Utilities.PrepareRequest(new TinDungTDServiceRef.TinDungTDRequest());
                response = new TinDungTDServiceRef.TinDungTDResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.TDTD_CHUYEN_HOAN;
                request.Action = action;
                request.objChuyenHoan = obj;

                // make a call to service client here
                response = client.TinDungTieuDung(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                lstResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    obj = response.objChuyenHoan;
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

        #region Du thu
        public int DuThu(DatabaseConstant.Action action, ref TDTD_DU_THU obj, ref List<ClientResponseDetail> lstResponseDetail)
        {
            TinDungTDServiceClient client = null;
            TinDungTDRequest request = null;
            TinDungTDResponse response = null;
            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungTDService.layGiaTri());

                client = TinDungTDServiceClient(ApplicationConstant.SystemService.TinDungTDService);
                request = Common.Utilities.PrepareRequest(new TinDungTDServiceRef.TinDungTDRequest());
                response = new TinDungTDServiceRef.TinDungTDResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.TDTD_DU_THU;
                request.Action = action;
                request.objDuThu = obj;

                // make a call to service client here
                response = client.TinDungTieuDung(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                lstResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    obj = response.objDuThu;
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

        #region Trich lap du phong
        public int TrichLapDuPhong(DatabaseConstant.Action action, ref TDTD_TRICH_LAP_DU_PHONG obj, ref List<ClientResponseDetail> lstResponseDetail)
        {
            TinDungTDServiceClient client = null;
            TinDungTDRequest request = null;
            TinDungTDResponse response = null;
            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TinDungTDService.layGiaTri());

                client = TinDungTDServiceClient(ApplicationConstant.SystemService.TinDungTDService);
                request = Common.Utilities.PrepareRequest(new TinDungTDServiceRef.TinDungTDRequest());
                response = new TinDungTDServiceRef.TinDungTDResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.TDTD_TRICH_LAP_DP;
                request.Action = action;
                request.objTrichLapDP = obj;

                // make a call to service client here
                response = client.TinDungTieuDung(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                lstResponseDetail = Common.Utilities.convertToClientResponseDetail(response);
                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    obj = response.objTrichLapDP;
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

    }
}
