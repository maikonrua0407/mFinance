using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.ServiceModel;
using Utilities.Common;
using Presentation.Process.NhanSuServiceRef;
using Presentation.Process.TruyVanServiceRef;
using Presentation.Process.Common;
using System.ServiceModel.Description;

namespace Presentation.Process
{
    public class NhanSuProcess
    {
        /// <summary>
        /// Phương thức khởi tạo
        /// </summary>
        public NhanSuProcess()
        {
        }

        /// <summary>
        /// Khởi tạo service NhanSuService
        /// </summary>
        /// <param name="service"></param>
        /// <returns></returns>
        private NhanSuServiceClient NhanSuServiceClient(ApplicationConstant.SystemService service)
        {
            EndpointAddress endpointAddress = Common.Utilities.getEndpointAddress(service.layGiaTri());
            BasicHttpBinding basicHttpBinding = Common.Utilities.getBasicHttpBinding(service.layGiaTri());
            NhanSuServiceClient Client = new NhanSuServiceClient(basicHttpBinding, endpointAddress);

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

        public int functionName(int id)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

            NhanSuServiceClient client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
            NhanSuServiceRef.NhanSuRequest request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
            NhanSuServiceRef.NhanSuResponse response = new NhanSuServiceRef.NhanSuResponse();
            try
            {
                // make a call to service client here
                response = client.functionName(request);
            }
            catch(Exception ex)
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
            }
            return 0;
        }


        #region Dùng chung
        public DataSet getThongTinNhanSuTheoID(int idNhanSu)
        {
            // Kiểm tra kết nối, server, service trước khi request
            Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.TruyVanService.layGiaTri());

            //Khởi tạo và gán các giá trị cho request
            TruyVanServiceRef.TruyVanRequest request = Common.Utilities.PrepareRequest(new TruyVanServiceRef.TruyVanRequest());
            TruyVanServiceRef.TruyVanResponse response = new TruyVanServiceRef.TruyVanResponse();
            DataTable dt = null;

            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@ID_NHANSU", "INT", idNhanSu.ToString());

            request.dtThamSo = dt;
            request.inquiryName = "%";
            request.objectName = "INQ.CT.NHAN_SU";

            // Lấy kết quả trả về
            TruyVanServiceClient client = TruyVanServiceClient(ApplicationConstant.SystemService.TruyVanService);
            response = client.TruyVanMessage(request);

            // Kiểm tra kết quả trả về
            Common.Utilities.ValidResponse(request, response);

            return response.dsResult;
        }

        public DataSet GetDanhSachDanhMuc(string tenBang, string maDonVi)
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
                LDatatable.AddParameter(ref dt, "@TEN_BANG", "STRING", tenBang);
                LDatatable.AddParameter(ref dt, "@MA_DVI", "STRING", maDonVi);

                request.dtThamSo = dt;
                request.objectName = "sp_INQ.DS.NS_DANH_MUC";
                request.inquiryName = "CHUNG";

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
        #endregion

        #region Hồ sơ
        public bool HoSo(DatabaseConstant.Action action, ref NS_HO_SO objHoSo, List<NS_HO_SO_QHE_GDINH> lstQuanHeGiaDinh, List<NS_HO_SO_TDO_HVAN> lstTrinhDoHocVan, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_HO_SO_CT;
                request.Action = action;
                request.objHoSo = objHoSo;
                if(lstQuanHeGiaDinh != null)
                    request.lstQuanHeGiaDinh = lstQuanHeGiaDinh.ToArray();
                if(lstTrinhDoHocVan != null)
                    request.lstTrinhDoHocVan = lstTrinhDoHocVan.ToArray();

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    objHoSo = response.objHoSo;
                    if(response.lstQuanHeGiaDinh != null)
                        lstQuanHeGiaDinh = response.lstQuanHeGiaDinh.ToList();
                    if (response.lstTrinhDoHocVan != null)
                        lstTrinhDoHocVan = response.lstTrinhDoHocVan.ToList();
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

        public bool HoSo(DatabaseConstant.Action action, ref NS_HO_SO objHoSo,    
                                                        ref NS_TEMP_HO_SO objTempHoSo,
                                                        ref List<NS_HSO_DU_AN_DTO> lstDuAn,                             
                                                        ref List<NS_HO_SO_QHE_GDINH> lstQuanHeGiaDinh,
                                                        ref List<NS_HO_SO_TDO_HVAN> lstTrinhDoHocVan,
                                                        ref List<NS_DM_LOAI_HSO> lstDMLoaiHoSo,
                                                        ref List<NS_DM_GIOI_TINH> lstDMGioiTinh,
                                                        ref List<NS_DM_TDO_HVAN> lstDMTrinhDoHocVan,
                                                        ref List<NS_DM_CNGANH_DTAO> lstDMChuyenNganhDaoTao,
                                                        ref List<NS_DM_DVI_CTAC> lstDMDonViCongTac,
                                                        ref List<NS_DM_CHUC_VU> lstDMChucVu,
                                                        ref List<NS_DM_HTHUC_LVIEC> lstDMHinhThucLamViec,
                                                        ref List<NS_DM_QUOC_TICH> lstDMQuocTich,
                                                        ref List<NS_DM_TON_GIAO> lstDMTonGiao,
                                                        ref List<NS_DM_DAN_TOC> lstDMDanToc,
                                                        ref List<NS_DM_TTRANG_HNHAN> lstDMTinhTrangHonNhan,
                                                        ref List<NS_DM_TINH_TP> lstDMTinhThanhPho,
                                                        ref List<NS_DM_QUAN_HUYEN> lstDMQuanHuyen,
                                                        ref List<NS_DM_PHUONG_XA> lstDMPhuongXa,
                                                        ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_HO_SO_CT;
                request.Action = action;
                request.objHoSo = objHoSo;

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    objHoSo = response.objHoSo;
                    objTempHoSo = response.objTempHoSo;

                    if (response.lstDuAn != null)
                        lstDuAn = response.lstDuAn.ToList();

                    if (response.lstQuanHeGiaDinh != null)
                        lstQuanHeGiaDinh = response.lstQuanHeGiaDinh.ToList();

                    if (response.lstTrinhDoHocVan != null)
                        lstTrinhDoHocVan = response.lstTrinhDoHocVan.ToList();

                    if (response.lstDMLoaiHoSo != null)
                        lstDMLoaiHoSo = response.lstDMLoaiHoSo.ToList();

                    if(response.lstDMGioiTinh != null)
                        lstDMGioiTinh = response.lstDMGioiTinh.ToList();

                    if (response.lstDMTrinhDoHocVan != null)
                        lstDMTrinhDoHocVan = response.lstDMTrinhDoHocVan.ToList();

                    if (response.lstDMChuyenNganhDaoTao != null)
                        lstDMChuyenNganhDaoTao = response.lstDMChuyenNganhDaoTao.ToList();

                    if (response.lstDMDonViCongTac != null)
                        lstDMDonViCongTac = response.lstDMDonViCongTac.ToList();

                    if (response.lstDMChucVu != null)
                        lstDMChucVu = response.lstDMChucVu.ToList();

                    if (response.lstDMHinhThucLamViec != null)
                        lstDMHinhThucLamViec = response.lstDMHinhThucLamViec.ToList();

                    if (response.lstDMQuocTich != null)
                        lstDMQuocTich = response.lstDMQuocTich.ToList();

                    if (response.lstDMTonGiao != null)
                        lstDMTonGiao = response.lstDMTonGiao.ToList();

                    if (response.lstDMDanToc != null)
                        lstDMDanToc = response.lstDMDanToc.ToList();

                    if (response.lstDMTinhTrangHonNhan != null)
                        lstDMTinhTrangHonNhan = response.lstDMTinhTrangHonNhan.ToList();

                    if (response.lstDMTinhThanhPho != null)
                        lstDMTinhThanhPho = response.lstDMTinhThanhPho.ToList();

                    if (response.lstDMQuanHuyen != null)
                        lstDMQuanHuyen = response.lstDMQuanHuyen.ToList();

                    if (response.lstDMPhuongXa != null)
                        lstDMPhuongXa = response.lstDMPhuongXa.ToList();
                    
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

        public bool DanhSachHoSo(DatabaseConstant.Action action, ref List<NS_HO_SO> lstHoSo, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_HO_SO_DS;
                request.Action = action;
                request.lstHoSo = lstHoSo.ToArray();

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

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

        public DataSet GetDanhSachHoSo(string maDonVi,string trangThaiNVu, string loaiHoSo, string maHoSo, string tenHoSo, string soCMND, int idGioiTinh, int idChucVu, int idPhongBan, int idHinhThucLamViec)
        {
            TruyVanServiceClient client = null;
            TruyVanRequest request = null;
            TruyVanResponse response = null;
            DataSet ds = null;
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
                LDatatable.AddParameter(ref dt, "@LOAI_HSO", "STRING", loaiHoSo);
                LDatatable.AddParameter(ref dt, "@MA_HSO", "STRING", maHoSo);
                LDatatable.AddParameter(ref dt, "@TEN_HSO", "STRING", tenHoSo);
                LDatatable.AddParameter(ref dt, "@SO_CMND", "STRING", soCMND);
                LDatatable.AddParameter(ref dt, "@ID_GIOI_TINH", "INT", idGioiTinh.ToString());
                LDatatable.AddParameter(ref dt, "@ID_CHUC_VU", "INT", idChucVu.ToString());
                LDatatable.AddParameter(ref dt, "@ID_PHONG_BAN", "INT", idPhongBan.ToString());
                LDatatable.AddParameter(ref dt, "@ID_HTHUC_LVIEC", "INT", idHinhThucLamViec.ToString());

                request.dtThamSo = dt;
                request.objectName = "INQ.DS.NS_HOSO";
                request.inquiryName = "DANH_SACH";

                // make a call to service client here
                response = client.TruyVanMessage(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                ds = response.dsResult;
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                ds = null;
                //throw ex;
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
            return ds;
        }

        public DataSet GetDanhSachHoSo(string maDonVi, string trangThaiNVu, string loaiHoSo)
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
                LDatatable.AddParameter(ref dt, "@LOAI_HSO", "STRING", loaiHoSo);                

                request.dtThamSo = dt;
                request.objectName = "INQ.DS.NS_HOSO";
                request.inquiryName = "DANH_SACH1";

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

        #region Hợp đồng
        public bool HopDong(DatabaseConstant.Action action, ref NS_HOP_DONG objHopDong, List<NS_PHU_CAP> lstPhuCap, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_HOP_DONG_CT;
                request.Action = action;
                request.objHopDong = objHopDong;
                if (lstPhuCap != null)
                    request.lstPhuCap = lstPhuCap.ToArray();

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    objHopDong = response.objHopDong;
                    if (response.lstPhuCap != null)
                        lstPhuCap = response.lstPhuCap.ToList();
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

        public bool HopDong(DatabaseConstant.Action action, ref NS_HOP_DONG objHopDong,
                                                            ref NS_TEMP_HOP_DONG objTempHopDong,
                                                            ref List<NS_PHU_CAP> lstPhuCap,
                                                            ref List<NS_DM_CHUC_VU> lstDMChucVu,
                                                            ref List<NS_DM_LOAI_HDLD> lstDMLoaiHdld,
                                                            ref List<NS_DM_THAN_HDLD> lstDMThoiHanHdld,
                                                            ref List<NS_DM_DVI_TGIAN> lstDMDonViThoiGian,
                                                            ref List<NS_DM_HTHUC_TLUONG> lstDMHinhThucTraLuong,
                                                            ref List<NS_DM_PHU_CAP> lstDMLoaiPhuCap,
                                                            ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_HOP_DONG_CT;
                request.Action = action;
                request.objHopDong = objHopDong;

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    objHopDong = response.objHopDong;
                    objTempHopDong = response.objTempHopDong;

                    if (response.lstPhuCap != null)
                        lstPhuCap = response.lstPhuCap.ToList();

                    if (response.lstDMChucVu != null)
                        lstDMChucVu = response.lstDMChucVu.ToList();

                    if (response.lstDMLoaiHdld != null)
                        lstDMLoaiHdld = response.lstDMLoaiHdld.ToList();

                    if (response.lstDMThoiHanHdld != null)
                        lstDMThoiHanHdld = response.lstDMThoiHanHdld.ToList();

                    if (response.lstDMDonViThoiGian != null)
                        lstDMDonViThoiGian = response.lstDMDonViThoiGian.ToList();

                    if (response.lstDMHinhThucTraLuong != null)
                        lstDMHinhThucTraLuong = response.lstDMHinhThucTraLuong.ToList();

                    if (response.lstDMChucVu != null)
                        lstDMChucVu = response.lstDMChucVu.ToList();

                    if (response.lstDMPhuCap != null)
                        lstDMLoaiPhuCap = response.lstDMPhuCap.ToList();

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

        public bool DanhSachHopDong(DatabaseConstant.Action action, ref List<NS_HOP_DONG> lstHopDong, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_HOP_DONG_DS;
                request.Action = action;
                request.lstHopDong = lstHopDong.ToArray();

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

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

        public DataSet GetDanhSachHopDong(string maDonVi, string trangThaiNVu, string loaiHopDong, string maHopDong, string maNhanVien, string tenNhanVien, int idLoaiThoiHan, int thoiHan, int idDonViThoiGian)
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
                LDatatable.AddParameter(ref dt, "@LOAI_HOPDONG", "STRING", loaiHopDong);
                LDatatable.AddParameter(ref dt, "@MA_HOPDONG", "STRING", maHopDong);
                LDatatable.AddParameter(ref dt, "@MA_HSO", "STRING", maNhanVien);
                LDatatable.AddParameter(ref dt, "@TEN_HSO", "STRING", tenNhanVien);
                LDatatable.AddParameter(ref dt, "@ID_LOAI_THOI_HAN", "INT", idLoaiThoiHan.ToString());
                LDatatable.AddParameter(ref dt, "@THOI_HAN", "INT", thoiHan.ToString());
                LDatatable.AddParameter(ref dt, "@ID_DVI_TGIAN", "INT", idDonViThoiGian.ToString());

                request.dtThamSo = dt;
                request.objectName = "INQ.DS.NS_HODONG";
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
        #endregion

        #region Thuyên chuyển công tác
        public bool ThuyenChuyenCongTac(DatabaseConstant.Action action, ref NS_TCHUYEN_CTAC objThuyenChuyenCongTac, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {                
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_THUYEN_CHUYEN_CT;
                request.Action = action;
                request.objThuyenChuyenCongTac = objThuyenChuyenCongTac;

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    objThuyenChuyenCongTac = response.objThuyenChuyenCongTac;
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

        public bool ThuyenChuyenCongTac(DatabaseConstant.Action action, ref NS_TCHUYEN_CTAC objThuyenChuyenCongTac,
                                                            ref NS_TEMP_TCHUYEN_CTAC objTempThuyenChuyenCongTac,                                                           
                                                            ref List<NS_DM_CHUC_VU> lstDMChucVu,
                                                            ref List<NS_DM_DVI_CTAC> lstDMDonViCongTac,                                                            
                                                            ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_THUYEN_CHUYEN_CT;
                request.Action = action;
                request.objThuyenChuyenCongTac = objThuyenChuyenCongTac;

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    objThuyenChuyenCongTac = response.objThuyenChuyenCongTac;
                    objTempThuyenChuyenCongTac = response.objTempThuyenChuyenCongTac;

                    if (response.lstDMChucVu != null)
                        lstDMChucVu = response.lstDMChucVu.ToList();

                    if (response.lstDMDonViCongTac != null)
                        lstDMDonViCongTac = response.lstDMDonViCongTac.ToList();                    

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

        public bool DanhSachThuyenChuyenCongTac(DatabaseConstant.Action action, ref List<NS_TCHUYEN_CTAC> lstThuyenChuyenCongTac, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_THUYEN_CHUYEN_DS;
                request.Action = action;
                request.lstThuyenChuyenCongTac = lstThuyenChuyenCongTac.ToArray();

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

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

        public DataSet GetDanhSachThuyenChuyenCongTac(string maDonVi, string trangThaiNVu, string maThuyenChuyen, string tuNgay, string denNgay, string maHoSo, string tenHoSo, int idChucVuCu, int idChucVuMoi, int idBoPhanCu, int idBoPhanMoi)
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
                LDatatable.AddParameter(ref dt, "@MA_THUYEN_CHUYEN", "STRING", maThuyenChuyen);
                LDatatable.AddParameter(ref dt, "@TU_NGAY", "STRING", tuNgay);
                LDatatable.AddParameter(ref dt, "@DEN_NGAY", "STRING", denNgay);
                LDatatable.AddParameter(ref dt, "@MA_HSO", "STRING", maHoSo);
                LDatatable.AddParameter(ref dt, "@TEN_HSO", "STRING", tenHoSo.ToString());
                LDatatable.AddParameter(ref dt, "@ID_CHUC_VU_CU", "INT", idChucVuCu.ToString());
                LDatatable.AddParameter(ref dt, "@ID_CHUC_VU_MOI", "INT", idChucVuMoi.ToString());
                LDatatable.AddParameter(ref dt, "@ID_BO_PHAN_CU", "INT", idBoPhanCu.ToString());
                LDatatable.AddParameter(ref dt, "@ID_BO_PHAN_MOI", "INT", idBoPhanMoi.ToString());

                request.dtThamSo = dt;
                request.objectName = "INQ.DS.NS_THUYEN_CHUYEN";
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
        #endregion

        #region Thôi việc
        public bool ThoiViec(DatabaseConstant.Action action, ref NS_THOI_VIEC objThoiViec, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_THOI_VIEC_CT;
                request.Action = action;
                request.objThoiViec = objThoiViec;

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    objThoiViec = response.objThoiViec;
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

        public bool ThoiViec(DatabaseConstant.Action action, ref NS_THOI_VIEC objThoiViec,
                                                            ref NS_TEMP_THOI_VIEC objTempThoiViec,
                                                            ref List<NS_DM_CHUC_VU> lstDMChucVu,
                                                            ref List<NS_DM_DVI_CTAC> lstDMDonViCongTac,
                                                            ref List<NS_DM_LDO_TVIEC> lstDMLyDoThoiViec,
                                                            ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_THOI_VIEC_CT;
                request.Action = action;
                request.objThoiViec = objThoiViec;

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    objThoiViec = response.objThoiViec;
                    objTempThoiViec = response.objTempThoiViec;

                    if (response.lstDMChucVu != null)
                        lstDMChucVu = response.lstDMChucVu.ToList();

                    if (response.lstDMDonViCongTac != null)
                        lstDMDonViCongTac = response.lstDMDonViCongTac.ToList();

                    if (response.lstDMLyDoThoiViec != null)
                        lstDMLyDoThoiViec = response.lstDMLyDoThoiViec.ToList();

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

        public bool DanhSachThoiViec(DatabaseConstant.Action action, ref List<NS_THOI_VIEC> lstThoiViec, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_THOI_VIEC_DS;
                request.Action = action;
                request.lstThoiViec = lstThoiViec.ToArray();

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

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

        public DataSet GetDanhSachThoiViec(string maDonVi, string trangThaiNVu, string maThoiViec, string tuNgay, string denNgay, string maHoSo, string tenHoSo, int idLyDo)
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
                LDatatable.AddParameter(ref dt, "@MA_THOI_VIEC", "STRING", maThoiViec);
                LDatatable.AddParameter(ref dt, "@TU_NGAY", "STRING", tuNgay);
                LDatatable.AddParameter(ref dt, "@DEN_NGAY", "STRING", denNgay);
                LDatatable.AddParameter(ref dt, "@MA_HSO", "STRING", maHoSo);
                LDatatable.AddParameter(ref dt, "@TEN_HSO", "STRING", tenHoSo.ToString());
                LDatatable.AddParameter(ref dt, "@ID_LY_DO", "INT", idLyDo.ToString());                

                request.dtThamSo = dt;
                request.objectName = "INQ.DS.NS_THOI_VIEC";
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
        #endregion

        #region Bảng lương
        public DataSet GetDanhSachBangLuong(DataTable dt)
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
                

                request.dtThamSo = dt;
                request.objectName = "INQ.DS.NS_BANG_LUONG";
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

        public bool BangLuong(DatabaseConstant.Action action, ref List<NS_BAC_LUONG> lstBangLuong, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_BANG_LUONG;
                request.Action = action;
                request.lstBangLuong = lstBangLuong.ToArray();

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    lstBangLuong = response.lstBangLuong.ToList();
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

        #region Tiêu chi phụ cấp
        public DataSet GetPhuCapCoDinh(DataTable dt)
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

                request.dtThamSo = dt;
                request.objectName = "INQ.DS.NS_TIEU_CHI_PHU_CAP_CTV";
                request.inquiryName = "CO_DINH";

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

        public DataSet GetPhuCapBoSung(DataTable dt)
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
                

                request.dtThamSo = dt;
                request.objectName = "INQ.DS.NS_TIEU_CHI_PHU_CAP_CTV";
                request.inquiryName = "BO_SUNG";

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

        public bool TieuChiPhuCap(DatabaseConstant.Action action, ref NS_TIEU_CHI_PHU_CAP objTieuChiPhuCap, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_TIEU_CHI_PHU_CAP_CTV;
                request.Action = action;
                request.objTieuChiPhuCap = objTieuChiPhuCap;

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    objTieuChiPhuCap = response.objTieuChiPhuCap;
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

        #region Lương
        public bool Luong(DatabaseConstant.Action action, ref NS_THONG_TIN_LUONG objThongTinLuong, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {                
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_LUONG_CT;
                request.Action = action;
                request.objThongTinLuong = objThongTinLuong;

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    objThongTinLuong = response.objThongTinLuong;
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

        public bool DanhSachLuong(DatabaseConstant.Action action, ref List<NS_THONG_TIN_LUONG> lstThongTinLuong, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {                
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_LUONG_DS;
                request.Action = action;
                request.lstThongTinLuong = lstThongTinLuong.ToArray();

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

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

        public DataSet GetDanhSachLuong(string maDonVi, string trangThaiNVu, string maNhanVien, string tenNhanVien, int idChucVu, int bacLuongTu, int bacLuongDen)
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
                LDatatable.AddParameter(ref dt, "@MA_HSO", "STRING", maNhanVien);
                LDatatable.AddParameter(ref dt, "@TEN_HSO", "STRING", tenNhanVien);
                LDatatable.AddParameter(ref dt, "@ID_CHUC_VU", "INT", idChucVu.ToString());
                LDatatable.AddParameter(ref dt, "@BAC_LUONG_TU", "INT", bacLuongTu.ToString());
                LDatatable.AddParameter(ref dt, "@BAC_LUONG_DEN", "INT", bacLuongDen.ToString());

                request.dtThamSo = dt;
                request.objectName = "INQ.DS.NS_LUONG";
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
        #endregion

        #region Lương điều chỉnh
        public bool LuongDieuChinh(DatabaseConstant.Action action, ref NS_THONG_TIN_LUONG_DCHINH objThongTinLuongDieuChinh, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;
            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_LUONG_DCHINH_CT;
                request.Action = action;
                request.objThongTinLuongDieuChinh = objThongTinLuongDieuChinh;

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    objThongTinLuongDieuChinh = response.objThongTinLuongDieuChinh;
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

        public bool DanhSachLuongDieuChinh(DatabaseConstant.Action action, ref List<NS_THONG_TIN_LUONG_DCHINH> lstThongTinLuongDieuChinh, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_LUONG_DCHINH_DS;
                request.Action = action;
                request.lstThongTinLuongDieuChinh = lstThongTinLuongDieuChinh.ToArray();

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

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

        public DataSet GetDanhSachLuongDieuChinh(string maDonVi, string trangThaiNVu, string maNhanVien, string tenNhanVien, int idChucVu, string dienGiai)
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
                LDatatable.AddParameter(ref dt, "@MA_HSO", "STRING", maNhanVien);
                LDatatable.AddParameter(ref dt, "@TEN_HSO", "STRING", tenNhanVien);
                LDatatable.AddParameter(ref dt, "@ID_CHUC_VU", "INT", idChucVu.ToString());
                LDatatable.AddParameter(ref dt, "@DIEN_GIAI", "STRING", dienGiai);

                request.dtThamSo = dt;
                request.objectName = "INQ.DS.NS_LUONG_DCHINH";
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
        #endregion

        #region Tính lương
        public bool TinhLuong(DatabaseConstant.Action action, ref NS_THONG_TIN_TINH_LUONG objThongTinTinhLuong, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_TINH_LUONG_CT;
                request.Action = action;
                request.objThongTinTinhLuong = objThongTinTinhLuong;

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    objThongTinTinhLuong = response.objThongTinTinhLuong;
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

        public bool DanhSachTinhLuong(DatabaseConstant.Action action, ref List<NS_THONG_TIN_TINH_LUONG> lstThongTinTinhLuong, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_TINH_LUONG_DS;
                request.Action = action;
                request.lstThongTinTinhLuong = lstThongTinTinhLuong.ToArray();

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

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

        public DataSet GetDanhSachTinhLuong(string maDonVi, string trangThaiNVu, string soGiaoDich)
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
                LDatatable.AddParameter(ref dt, "@MA_GDICH", "STRING", soGiaoDich);

                request.dtThamSo = dt;
                request.objectName = "INQ.DS.NS_TINH_LUONG";
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
        #endregion

        #region Phụ cấp cộng tác viên
        public bool PhuCapCTV(DatabaseConstant.Action action, ref NS_THONG_TIN_PHU_CAP_CTV objThongTinPhuCapCTV, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_TINH_PHU_CAP_CTV_CT;
                request.Action = action;
                request.objThongTinPhuCapCTV = objThongTinPhuCapCTV;

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    objThongTinPhuCapCTV = response.objThongTinPhuCapCTV;
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

        public bool DanhSachPhuCapCTV(DatabaseConstant.Action action, ref List<NS_THONG_TIN_PHU_CAP_CTV> lstThongTinPhuCapCTV, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_TINH_PHU_CAP_CTV_DS;
                request.Action = action;
                request.lstThongTinPhuCapCTV = lstThongTinPhuCapCTV.ToArray();

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

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

        public DataSet GetDanhSachPhuCapCTV(string maDonVi, string trangThaiNVu, string soGiaoDich)
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
                LDatatable.AddParameter(ref dt, "@MA_GDICH", "STRING", soGiaoDich);

                request.dtThamSo = dt;
                request.objectName = "INQ.DS.NS_PHU_CAP_CTV";
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
        #endregion

        #region Quản lý dự án
        public DataSet GetDanhSachQuanLyDuAn(DataTable dt)
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
                

                request.dtThamSo = dt;
                request.objectName = "INQ.DS.NS_DU_AN";
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

        public bool QuanLyDuAn(DatabaseConstant.Action action, ref NS_QLY_DU_AN_DTO objQuanLyDuAn, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_QLY_DU_AN;
                request.Action = action;
                request.objQuanLyDuAn = objQuanLyDuAn;

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    objQuanLyDuAn = response.objQuanLyDuAn;
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


        #region Danh mục bằng cấp
        public bool BangCap(DatabaseConstant.Action action, ref NS_DM_BANG_CAP objDMBangCap, ref List<ClientResponseDetail> listClientResponseDetail)
        {            
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_DM_BANG_CAP_CT;
                request.Action = action;
                request.objDMBangCap = objDMBangCap;

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    objDMBangCap = response.objDMBangCap;
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

        public bool DanhSachBangCap(DatabaseConstant.Action action, ref List<NS_DM_BANG_CAP> lstDMBangCap, ref List<ClientResponseDetail> listClientResponseDetail)
        {            
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_DM_BANG_CAP_DS;
                request.Action = action;
                request.lstDMBangCap = lstDMBangCap.ToArray();

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

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

        #region Danh mục Bệnh viện khám chữa bệnh
        public bool BenhVienKhamChuaBenh(DatabaseConstant.Action action, ref NS_DM_BVIEN_KCB objDMBenhVienKhamChuaBenh, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_DM_BVIEN_KCB_CT;
                request.Action = action;
                request.objDMBenhVienKhamChuaBenh = objDMBenhVienKhamChuaBenh;

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    objDMBenhVienKhamChuaBenh = response.objDMBenhVienKhamChuaBenh;
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

        public bool DanhSachBenhVienKhamChuaBenh(DatabaseConstant.Action action, ref List<NS_DM_BVIEN_KCB> lstDMBenhVienKhamChuaBenh, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_DM_BVIEN_KCB_DS;
                request.Action = action;
                request.lstDMBenhVienKhamChuaBenh = lstDMBenhVienKhamChuaBenh.ToArray();

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

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

        #region Danh mục chức vụ
        public bool ChucVu(DatabaseConstant.Action action, ref NS_DM_CHUC_VU objDMChucVu, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_DM_CHUC_VU_CT;
                request.Action = action;
                request.objDMChucVu = objDMChucVu;
                
                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    objDMChucVu = response.objDMChucVu;
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

        public bool DanhSachChucVu(DatabaseConstant.Action action, ref List<NS_DM_CHUC_VU> lstDMChucVu, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_DM_CHUC_VU_DS;
                request.Action = action;
                request.lstDMChucVu = lstDMChucVu.ToArray();

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

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

        public DataSet GetDanhSachChucVu(string maDonVi)
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
                request.objectName = "sp_INQ.DS.NS_DANH_MUC";
                request.inquiryName = "CHUC_VU";

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

        #region Danh mục Chuyên ngành đào tạo
        public bool ChuyenNganhDaoTao(DatabaseConstant.Action action, ref NS_DM_CNGANH_DTAO objDMChuyenNganhDaoTao, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_DM_CNGANH_DTAO_CT;
                request.Action = action;
                request.objDMChuyenNganhDaoTao = objDMChuyenNganhDaoTao;

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    objDMChuyenNganhDaoTao = response.objDMChuyenNganhDaoTao;
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

        public bool DanhSachChuyenNganhDaoTao(DatabaseConstant.Action action, ref List<NS_DM_CNGANH_DTAO> lstDMChuyenNganhDaoTao, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_DM_CNGANH_DTAO_DS;
                request.Action = action;
                request.lstDMChuyenNganhDaoTao = lstDMChuyenNganhDaoTao.ToArray();

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

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

        #region Danh mục Cư trú
        public bool CuTru(DatabaseConstant.Action action, ref NS_DM_CU_TRU objDMCuTru, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_DM_CU_TRU_CT;
                request.Action = action;
                request.objDMCuTru = objDMCuTru;

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    objDMCuTru = response.objDMCuTru;
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

        public bool DanhSachCuTru(DatabaseConstant.Action action, ref List<NS_DM_CU_TRU> lstDMCuTru, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_DM_CU_TRU_DS;
                request.Action = action;
                request.lstDMCuTru = lstDMCuTru.ToArray();

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

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

        #region Danh mục Dân tộc
        public bool DanToc(DatabaseConstant.Action action, ref NS_DM_DAN_TOC objDMDanToc, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_DM_DAN_TOC_CT;
                request.Action = action;
                request.objDMDanToc = objDMDanToc;

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    objDMDanToc = response.objDMDanToc;
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

        public bool DanhSachDanToc(DatabaseConstant.Action action, ref List<NS_DM_DAN_TOC> lstDMDanToc, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_DM_DAN_TOC_DS;
                request.Action = action;
                request.lstDMDanToc = lstDMDanToc.ToArray();

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

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

        #region Danh mục Đơn vị công tác
        public bool DonViCongTac(DatabaseConstant.Action action, ref NS_DM_DVI_CTAC objDMDonViCongTac, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_DM_DVI_CTAC_CT;
                request.Action = action;
                request.objDMDonViCongTac = objDMDonViCongTac;

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    objDMDonViCongTac = response.objDMDonViCongTac;
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

        public bool DanhSachDonViCongTac(DatabaseConstant.Action action, ref List<NS_DM_DVI_CTAC> lstDMDonViCongTac, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_DM_DVI_CTAC_DS;
                request.Action = action;
                request.lstDMDonViCongTac = lstDMDonViCongTac.ToArray();

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

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

        #region Danh mục Đơn vị thời gian
        public bool DonViThoiGian(DatabaseConstant.Action action, ref NS_DM_DVI_TGIAN objDMDonViThoiGian, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_DM_DVI_TGIAN_CT;
                request.Action = action;
                request.objDMDonViThoiGian = objDMDonViThoiGian;

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    objDMDonViThoiGian = response.objDMDonViThoiGian;
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

        public bool DanhSachDonViThoiGian(DatabaseConstant.Action action, ref List<NS_DM_DVI_TGIAN> lstDMDonViThoiGian, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_DM_DVI_TGIAN_DS;
                request.Action = action;
                request.lstDMDonViThoiGian = lstDMDonViThoiGian.ToArray();

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

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

        #region Danh mục Giới tính
        public bool GioiTinh(DatabaseConstant.Action action, ref NS_DM_GIOI_TINH objDMGioiTinh, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_DM_GIOI_TINH_CT;
                request.Action = action;
                request.objDMGioiTinh = objDMGioiTinh;

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    objDMGioiTinh = response.objDMGioiTinh;
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

        public bool DanhSachGioiTinh(DatabaseConstant.Action action, ref List<NS_DM_GIOI_TINH> lstDMGioiTinh, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_DM_GIOI_TINH_DS;
                request.Action = action;
                request.lstDMGioiTinh = lstDMGioiTinh.ToArray();

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

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

        #region Danh mục Học hàm
        public bool HocHam(DatabaseConstant.Action action, ref NS_DM_HOC_HAM objDMHocHam, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_DM_HOC_HAM_CT;
                request.Action = action;
                request.objDMHocHam = objDMHocHam;

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    objDMHocHam = response.objDMHocHam;
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

        public bool DanhSachHocHam(DatabaseConstant.Action action, ref List<NS_DM_HOC_HAM> lstDMHocHam, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_DM_HOC_HAM_DS;
                request.Action = action;
                request.lstDMHocHam = lstDMHocHam.ToArray();

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

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

        #region Danh mục Học vị
        public bool HocVi(DatabaseConstant.Action action, ref NS_DM_HOC_VI objDMHocVi, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_DM_HOC_VI_CT;
                request.Action = action;
                request.objDMHocVi = objDMHocVi;

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    objDMHocVi = response.objDMHocVi;
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

        public bool DanhSachHocVi(DatabaseConstant.Action action, ref List<NS_DM_HOC_VI> lstDMHocVi, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_DM_HOC_VI_DS;
                request.Action = action;
                request.lstDMHocVi = lstDMHocVi.ToArray();

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

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

        #region Danh mục Hình thức đào tạo
        public bool HinhThucDaoTao(DatabaseConstant.Action action, ref NS_DM_HTHUC_DTAO objDMHinhThucDaoTao, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_DM_HTHUC_DTAO_CT;
                request.Action = action;
                request.objDMHinhThucDaoTao = objDMHinhThucDaoTao;

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    objDMHinhThucDaoTao = response.objDMHinhThucDaoTao;
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

        public bool DanhSachHinhThucDaoTao(DatabaseConstant.Action action, ref List<NS_DM_HTHUC_DTAO> lstDMHinhThucDaoTao, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_DM_HTHUC_DTAO_DS;
                request.Action = action;
                request.lstDMHinhThucDaoTao = lstDMHinhThucDaoTao.ToArray();

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

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

        #region Danh mục Hình thức kỷ luật
        public bool HinhThucKyLuat(DatabaseConstant.Action action, ref NS_DM_HTHUC_KLUAT objDMHinhThucKyLuat, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_DM_HTHUC_KLUAT_CT;
                request.Action = action;
                request.objDMHinhThucKyLuat = objDMHinhThucKyLuat;

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    objDMHinhThucKyLuat = response.objDMHinhThucKyLuat;
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

        public bool DanhSachHinhThucKyLuat(DatabaseConstant.Action action, ref List<NS_DM_HTHUC_KLUAT> lstDMHinhThucKyLuat, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_DM_HTHUC_KLUAT_DS;
                request.Action = action;
                request.lstDMHinhThucKyLuat = lstDMHinhThucKyLuat.ToArray();

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

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

        #region Danh mục Hình thức khen thưởng
        public bool HinhThucKhenThuong(DatabaseConstant.Action action, ref NS_DM_HTHUC_KTHUONG objDMHinhThucKhenThuong, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_DM_HTHUC_KTHUONG_CT;
                request.Action = action;
                request.objDMHinhThucKhenThuong = objDMHinhThucKhenThuong;

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    objDMHinhThucKhenThuong = response.objDMHinhThucKhenThuong;
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

        public bool DanhSachHinhThucKhenThuong(DatabaseConstant.Action action, ref List<NS_DM_HTHUC_KTHUONG> lstDMHinhThucKhenThuong, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_DM_HTHUC_KTHUONG_DS;
                request.Action = action;
                request.lstDMHinhThucKhenThuong = lstDMHinhThucKhenThuong.ToArray();

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

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

        #region Danh mục Hình thức làm việc
        public bool HinhThucLamViec(DatabaseConstant.Action action, ref NS_DM_HTHUC_LVIEC objDMHinhThucLamViec, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_DM_HTHUC_LVIEC_CT;
                request.Action = action;
                request.objDMHinhThucLamViec = objDMHinhThucLamViec;

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    objDMHinhThucLamViec = response.objDMHinhThucLamViec;
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

        public bool DanhSachHinhThucLamViec(DatabaseConstant.Action action, ref List<NS_DM_HTHUC_LVIEC> lstDMHinhThucLamViec, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_DM_HTHUC_LVIEC_DS;
                request.Action = action;
                request.lstDMHinhThucLamViec = lstDMHinhThucLamViec.ToArray();

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

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

        #region Danh mục Hình thức trả lương
        public bool HinhThucTraLuong(DatabaseConstant.Action action, ref NS_DM_HTHUC_TLUONG objDMHinhThucTraLuong, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_DM_HTHUC_TLUONG_CT;
                request.Action = action;
                request.objDMHinhThucTraLuong = objDMHinhThucTraLuong;

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    objDMHinhThucTraLuong = response.objDMHinhThucTraLuong;
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

        public bool DanhSachHinhThucTraLuong(DatabaseConstant.Action action, ref List<NS_DM_HTHUC_TLUONG> lstDMHinhThucTraLuong, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_DM_HTHUC_TLUONG_DS;
                request.Action = action;
                request.lstDMHinhThucTraLuong = lstDMHinhThucTraLuong.ToArray();

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

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

        #region Danh mục Ký hiệu chấm công
        public bool KyHieuChamCong(DatabaseConstant.Action action, ref NS_DM_KHIEU_CCONG objDMKyHieuChamCong, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_DM_KHIEU_CCONG_CT;
                request.Action = action;
                request.objDMKyHieuChamCong = objDMKyHieuChamCong;

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    objDMKyHieuChamCong = response.objDMKyHieuChamCong;
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

        public bool DanhSachKyHieuChamCong(DatabaseConstant.Action action, ref List<NS_DM_KHIEU_CCONG> lstDMKyHieuChamCong, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_DM_KHIEU_CCONG_DS;
                request.Action = action;
                request.lstDMKyHieuChamCong = lstDMKyHieuChamCong.ToArray();

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

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

        #region Danh mục Khóa đào tạo
        public bool KhoaDaoTao(DatabaseConstant.Action action, ref NS_DM_KHOA_DTAO objDMKhoaDaoTao, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_DM_KHOA_DTAO_CT;
                request.Action = action;
                request.objDMKhoaDaoTao = objDMKhoaDaoTao;

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    objDMKhoaDaoTao = response.objDMKhoaDaoTao;
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

        public bool DanhSachKhoaDaoTao(DatabaseConstant.Action action, ref List<NS_DM_KHOA_DTAO> lstDMKhoaDaoTao, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_DM_KHOA_DTAO_DS;
                request.Action = action;
                request.lstDMKhoaDaoTao = lstDMKhoaDaoTao.ToArray();

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

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

        #region Danh mục Kỹ năng
        public bool KyNang(DatabaseConstant.Action action, ref NS_DM_KY_NANG objDMKyNang, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_DM_KY_NANG_CT;
                request.Action = action;
                request.objDMKyNang = objDMKyNang;

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    objDMKyNang = response.objDMKyNang;
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

        public bool DanhSachKyNang(DatabaseConstant.Action action, ref List<NS_DM_KY_NANG> lstDMKyNang, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_DM_KY_NANG_DS;
                request.Action = action;
                request.lstDMKyNang = lstDMKyNang.ToArray();

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

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

        #region Danh mục Lý do nghỉ phép
        public bool LyDoNghiPhep(DatabaseConstant.Action action, ref NS_DM_LDO_NPHEP objDMLyDoNghiPhep, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_DM_LDO_NPHEP_CT;
                request.Action = action;
                request.objDMLyDoNghiPhep = objDMLyDoNghiPhep;

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    objDMLyDoNghiPhep = response.objDMLyDoNghiPhep;
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

        public bool DanhSachLyDoNghiPhep(DatabaseConstant.Action action, ref List<NS_DM_LDO_NPHEP> lstDMLyDoNghiPhep, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_DM_LDO_NPHEP_DS;
                request.Action = action;
                request.lstDMLyDoNghiPhep = lstDMLyDoNghiPhep.ToArray();

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

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

        #region Danh mục Lý do thôi việc
        public bool LyDoThoiViec(DatabaseConstant.Action action, ref NS_DM_LDO_TVIEC objDMLyDoThoiViec, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_DM_LDO_TVIEC_CT;
                request.Action = action;
                request.objDMLyDoThoiViec = objDMLyDoThoiViec;

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    objDMLyDoThoiViec = response.objDMLyDoThoiViec;
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

        public bool DanhSachLyDoThoiViec(DatabaseConstant.Action action, ref List<NS_DM_LDO_TVIEC> lstDMLyDoThoiViec, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_DM_LDO_TVIEC_DS;
                request.Action = action;
                request.lstDMLyDoThoiViec = lstDMLyDoThoiViec.ToArray();

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

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

        #region Danh mục Loại chi phí
        public bool LoaiChiPhi(DatabaseConstant.Action action, ref NS_DM_LOAI_CPHI objDMLoaiChiPhi, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_DM_LOAI_CPHI_CT;
                request.Action = action;
                request.objDMLoaiChiPhi = objDMLoaiChiPhi;

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    objDMLoaiChiPhi = response.objDMLoaiChiPhi;
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

        public bool DanhSachLoaiChiPhi(DatabaseConstant.Action action, ref List<NS_DM_LOAI_CPHI> lstDMLoaiChiPhi, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_DM_LOAI_CPHI_DS;
                request.Action = action;
                request.lstDMLoaiChiPhi = lstDMLoaiChiPhi.ToArray();

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

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

        #region Danh mục Loại giấy tờ
        public bool LoaiGiayTo(DatabaseConstant.Action action, ref NS_DM_LOAI_GTO objDMLoaiGiayTo, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_DM_LOAI_GTO_CT;
                request.Action = action;
                request.objDMLoaiGiayTo = objDMLoaiGiayTo;

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    objDMLoaiGiayTo = response.objDMLoaiGiayTo;
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

        public bool DanhSachLoaiGiayTo(DatabaseConstant.Action action, ref List<NS_DM_LOAI_GTO> lstDMLoaiGiayTo, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_DM_LOAI_GTO_DS;
                request.Action = action;
                request.lstDMLoaiGiayTo = lstDMLoaiGiayTo.ToArray();

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

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

        #region Danh mục Loại hợp đồng lao động
        public bool LoaiHdld(DatabaseConstant.Action action, ref NS_DM_LOAI_HDLD objDMLoaiHdld, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_DM_LOAI_HDLD_CT;
                request.Action = action;
                request.objDMLoaiHdld = objDMLoaiHdld;

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    objDMLoaiHdld = response.objDMLoaiHdld;
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

        public bool DanhSachLoaiHdld(DatabaseConstant.Action action, ref List<NS_DM_LOAI_HDLD> lstDMLoaiHdld, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_DM_LOAI_HDLD_DS;
                request.Action = action;
                request.lstDMLoaiHdld = lstDMLoaiHdld.ToArray();

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

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

        #region Danh mục Loại hồ sơ
        public bool LoaiHoSo(DatabaseConstant.Action action, ref NS_DM_LOAI_HSO objDMLoaiHoSo, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_DM_LOAI_HSO_CT;
                request.Action = action;
                request.objDMLoaiHoSo = objDMLoaiHoSo;

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    objDMLoaiHoSo = response.objDMLoaiHoSo;
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

        public bool DanhSachLoaiHoSo(DatabaseConstant.Action action, ref List<NS_DM_LOAI_HSO> lstDMLoaiHoSo, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_DM_LOAI_HSO_DS;
                request.Action = action;
                request.lstDMLoaiHoSo = lstDMLoaiHoSo.ToArray();

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

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

        #region Danh mục Loại thu nhập
        public bool LoaiThuNhap(DatabaseConstant.Action action, ref NS_DM_LOAI_TNHAP objDMLoaiThuNhap, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_DM_LOAI_TNHAP_CT;
                request.Action = action;
                request.objDMLoaiThuNhap = objDMLoaiThuNhap;

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    objDMLoaiThuNhap = response.objDMLoaiThuNhap;
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

        public bool DanhSachLoaiThuNhap(DatabaseConstant.Action action, ref List<NS_DM_LOAI_TNHAP> lstDMLoaiThuNhap, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_DM_LOAI_TNHAP_DS;
                request.Action = action;
                request.lstDMLoaiThuNhap = lstDMLoaiThuNhap.ToArray();

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

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

        #region Danh mục Nghề nghiệp
        public bool NgheNghiep(DatabaseConstant.Action action, ref NS_DM_NGHE_NGHIEP objDMNgheNghiep, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_DM_NGHE_NGHIEP_CT;
                request.Action = action;
                request.objDMNgheNghiep = objDMNgheNghiep;

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    objDMNgheNghiep = response.objDMNgheNghiep;
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

        public bool DanhSachNgheNghiep(DatabaseConstant.Action action, ref List<NS_DM_NGHE_NGHIEP> lstDMNgheNghiep, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_DM_NGHE_NGHIEP_DS;
                request.Action = action;
                request.lstDMNgheNghiep = lstDMNgheNghiep.ToArray();

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

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

        #region Danh mục Phường xã
        public bool PhuongXa(DatabaseConstant.Action action, ref NS_DM_PHUONG_XA objDMPhuongXa, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_DM_PHUONG_XA_CT;
                request.Action = action;
                request.objDMPhuongXa = objDMPhuongXa;

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    objDMPhuongXa = response.objDMPhuongXa;
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

        public bool DanhSachPhuongXa(DatabaseConstant.Action action, ref List<NS_DM_PHUONG_XA> lstDMPhuongXa, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_DM_PHUONG_XA_DS;
                request.Action = action;
                request.lstDMPhuongXa = lstDMPhuongXa.ToArray();

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

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

        public DataSet GetDanhSachPhuongXa(string maDonVi)
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
                request.objectName = "sp_INQ.DS.NS_DANH_MUC";
                request.inquiryName = "PHUONG_XA";

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

        #region Danh mục Quan hệ gia đình
        public bool QuanHeGiaDinh(DatabaseConstant.Action action, ref NS_DM_QHE_GDINH objDMQuanHeGiaDinh, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_DM_QHE_GDINH_CT;
                request.Action = action;
                request.objDMQuanHeGiaDinh = objDMQuanHeGiaDinh;

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    objDMQuanHeGiaDinh = response.objDMQuanHeGiaDinh;
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

        public bool DanhSachQuanHeGiaDinh(DatabaseConstant.Action action, ref List<NS_DM_QHE_GDINH> lstDMQuanHeGiaDinh, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_DM_QHE_GDINH_DS;
                request.Action = action;
                request.lstDMQuanHeGiaDinh = lstDMQuanHeGiaDinh.ToArray();

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

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

        #region Danh mục Quận huyện
        public bool QuanHuyen(DatabaseConstant.Action action, ref NS_DM_QUAN_HUYEN objDMQuanHuyen, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_DM_QUAN_HUYEN_CT;
                request.Action = action;
                request.objDMQuanHuyen = objDMQuanHuyen;

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    objDMQuanHuyen = response.objDMQuanHuyen;
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

        public bool DanhSachQuanHuyen(DatabaseConstant.Action action, ref List<NS_DM_QUAN_HUYEN> lstDMQuanHuyen, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_DM_QUAN_HUYEN_DS;
                request.Action = action;
                request.lstDMQuanHuyen = lstDMQuanHuyen.ToArray();

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

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

        public DataSet GetDanhSachQuanHuyen(string maDonVi)
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
                request.objectName = "sp_INQ.DS.NS_DANH_MUC";
                request.inquiryName = "QUAN_HUYEN";

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

        #region Danh mục Quốc tịch
        public bool QuocTich(DatabaseConstant.Action action, ref NS_DM_QUOC_TICH objDMQuocTich, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_DM_QUOC_TICH_CT;
                request.Action = action;
                request.objDMQuocTich = objDMQuocTich;

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    objDMQuocTich = response.objDMQuocTich;
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

        public bool DanhSachQuocTich(DatabaseConstant.Action action, ref List<NS_DM_QUOC_TICH> lstDMQuocTich, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_DM_QUOC_TICH_DS;
                request.Action = action;
                request.lstDMQuocTich = lstDMQuocTich.ToArray();

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

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

        #region Danh mục Trình độ chính trị
        public bool TrinhDoChinhTri(DatabaseConstant.Action action, ref NS_DM_TDO_CTRI objDMTrinhDoChinhTri, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_DM_TDO_CTRI_CT;
                request.Action = action;
                request.objDMTrinhDoChinhTri = objDMTrinhDoChinhTri;

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    objDMTrinhDoChinhTri = response.objDMTrinhDoChinhTri;
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

        public bool DanhSachTrinhDoChinhTri(DatabaseConstant.Action action, ref List<NS_DM_TDO_CTRI> lstDMTrinhDoChinhTri, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_DM_TDO_CTRI_DS;
                request.Action = action;
                request.lstDMTrinhDoChinhTri = lstDMTrinhDoChinhTri.ToArray();

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

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

        #region Danh mục Trình độ học vấn
        public bool TrinhDoHocVan(DatabaseConstant.Action action, ref NS_DM_TDO_HVAN objDMTrinhDoHocVan, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_DM_TDO_HVAN_CT;
                request.Action = action;
                request.objDMTrinhDoHocVan = objDMTrinhDoHocVan;

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    objDMTrinhDoHocVan = response.objDMTrinhDoHocVan;
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

        public bool DanhSachTrinhDoHocVan(DatabaseConstant.Action action, ref List<NS_DM_TDO_HVAN> lstDMTrinhDoHocVan, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_DM_TDO_HVAN_DS;
                request.Action = action;
                request.lstDMTrinhDoHocVan = lstDMTrinhDoHocVan.ToArray();

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

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

        #region Danh mục Trình độ tiếng anh
        public bool TrinhDoTiengAnh(DatabaseConstant.Action action, ref NS_DM_TDO_TANH objDMTrinhDoTiengAnh, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_DM_TDO_TANH_CT;
                request.Action = action;
                request.objDMTrinhDoTiengAnh = objDMTrinhDoTiengAnh;

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    objDMTrinhDoTiengAnh = response.objDMTrinhDoTiengAnh;
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

        public bool DanhSachTrinhDoTiengAnh(DatabaseConstant.Action action, ref List<NS_DM_TDO_TANH> lstDMTrinhDoTiengAnh, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_DM_TDO_TANH_DS;
                request.Action = action;
                request.lstDMTrinhDoTiengAnh = lstDMTrinhDoTiengAnh.ToArray();

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

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

        #region Danh mục Trình độ tin học
        public bool TrinhDoTinHoc(DatabaseConstant.Action action, ref NS_DM_TDO_THOC objDMTrinhDoTinHoc, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_DM_TDO_THOC_CT;
                request.Action = action;
                request.objDMTrinhDoTinHoc = objDMTrinhDoTinHoc;

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    objDMTrinhDoTinHoc = response.objDMTrinhDoTinHoc;
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

        public bool DanhSachTrinhDoTinHoc(DatabaseConstant.Action action, ref List<NS_DM_TDO_THOC> lstDMTrinhDoTinHoc, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_DM_TDO_THOC_DS;
                request.Action = action;
                request.lstDMTrinhDoTinHoc = lstDMTrinhDoTinHoc.ToArray();

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

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

        #region Danh mục Thời hạn hợp đồng lao động
        public bool ThoiHanHdld(DatabaseConstant.Action action, ref NS_DM_THAN_HDLD objDMThoiHanHdld, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_DM_THAN_HDLD_CT;
                request.Action = action;
                request.objDMThoiHanHdld = objDMThoiHanHdld;

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    objDMThoiHanHdld = response.objDMThoiHanHdld;
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

        public bool DanhSachThoiHanHdld(DatabaseConstant.Action action, ref List<NS_DM_THAN_HDLD> lstDMThoiHanHdld, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_DM_THAN_HDLD_DS;
                request.Action = action;
                request.lstDMThoiHanHdld = lstDMThoiHanHdld.ToArray();

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

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

        #region Danh mục Tỉnh thành phố
        public bool TinhThanhPho(DatabaseConstant.Action action, ref NS_DM_TINH_TP objDMTinhThanhPho, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_DM_TINH_TP_CT;
                request.Action = action;
                request.objDMTinhThanhPho = objDMTinhThanhPho;

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    objDMTinhThanhPho = response.objDMTinhThanhPho;
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

        public bool DanhSachTinhThanhPho(DatabaseConstant.Action action, ref List<NS_DM_TINH_TP> lstDMTinhThanhPho, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_DM_TINH_TP_DS;
                request.Action = action;
                request.lstDMTinhThanhPho = lstDMTinhThanhPho.ToArray();

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

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

        public DataSet GetDanhSachTinhThanhPho(string maDonVi)
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
                request.objectName = "sp_INQ.DS.NS_DANH_MUC";
                request.inquiryName = "TINH_TP";

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

        #region Danh mục Tôn giáo
        public bool TonGiao(DatabaseConstant.Action action, ref NS_DM_TON_GIAO objDMTonGiao, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_DM_TON_GIAO_CT;
                request.Action = action;
                request.objDMTonGiao = objDMTonGiao;

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    objDMTonGiao = response.objDMTonGiao;
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

        public bool DanhSachTonGiao(DatabaseConstant.Action action, ref List<NS_DM_TON_GIAO> lstDMTonGiao, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_DM_TON_GIAO_DS;
                request.Action = action;
                request.lstDMTonGiao = lstDMTonGiao.ToArray();

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

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

        #region Danh mục Trường đào tạo
        public bool TruongDaoTao(DatabaseConstant.Action action, ref NS_DM_TRUONG_DTAO objDMTruongDaoTao, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_DM_TRUONG_DTAO_CT;
                request.Action = action;
                request.objDMTruongDaoTao = objDMTruongDaoTao;

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    objDMTruongDaoTao = response.objDMTruongDaoTao;
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

        public bool DanhSachTruongDaoTao(DatabaseConstant.Action action, ref List<NS_DM_TRUONG_DTAO> lstDMTruongDaoTao, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_DM_TRUONG_DTAO_DS;
                request.Action = action;
                request.lstDMTruongDaoTao = lstDMTruongDaoTao.ToArray();

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

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

        #region Danh mục Tình trạng hôn nhân
        public bool TinhTrangHonNhan(DatabaseConstant.Action action, ref NS_DM_TTRANG_HNHAN objDMTinhTrangHonNhan, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_DM_TTRANG_HNHAN_CT;
                request.Action = action;
                request.objDMTinhTrangHonNhan = objDMTinhTrangHonNhan;

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    objDMTinhTrangHonNhan = response.objDMTinhTrangHonNhan;
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

        public bool DanhSachTinhTrangHonNhan(DatabaseConstant.Action action, ref List<NS_DM_TTRANG_HNHAN> lstDMTinhTrangHonNhan, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_DM_TTRANG_HNHAN_DS;
                request.Action = action;
                request.lstDMTinhTrangHonNhan = lstDMTinhTrangHonNhan.ToArray();

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

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

        #region Danh mục Xếp loại
        public bool XepLoai(DatabaseConstant.Action action, ref NS_DM_XEP_LOAI objDMXepLoai, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_DM_XEP_LOAI_CT;
                request.Action = action;
                request.objDMXepLoai = objDMXepLoai;

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    objDMXepLoai = response.objDMXepLoai;
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

        public bool DanhSachXepLoai(DatabaseConstant.Action action, ref List<NS_DM_XEP_LOAI> lstDMXepLoai, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_DM_XEP_LOAI_DS;
                request.Action = action;
                request.lstDMXepLoai = lstDMXepLoai.ToArray();

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

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

        #region Danh mục Phụ cấp
        public bool PhuCap(DatabaseConstant.Action action, ref NS_DM_PHU_CAP objDMPhuCap, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_DM_PHU_CAP_CT;
                request.Action = action;
                request.objDMPhuCap = objDMPhuCap;

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    objDMPhuCap = response.objDMPhuCap;
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

        public bool DanhSachPhuCap(DatabaseConstant.Action action, ref List<NS_DM_PHU_CAP> lstDMPhuCap, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_DM_PHU_CAP_DS;
                request.Action = action;
                request.lstDMPhuCap = lstDMPhuCap.ToArray();

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

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

        #region Danh mục nhóm cộng tác viên
        public bool NhomCongTacVien(DatabaseConstant.Action action, ref NS_DM_NHOM_CTV objDMNhomCongTacVien, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_DM_NHOM_CTV_CT;
                request.Action = action;
                request.objDMNhomCongTacVien = objDMNhomCongTacVien;

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    objDMNhomCongTacVien = response.objDMNhomCongTacVien;
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

        public bool DanhSachNhomCongTacVien(DatabaseConstant.Action action, ref List<NS_DM_NHOM_CTV> lstDMNhomCongTacVien, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_DM_NHOM_CTV_DS;
                request.Action = action;
                request.lstDMNhomCongTacVien = lstDMNhomCongTacVien.ToArray();

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

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

        #region Danh mục dự án
        public bool DuAn(DatabaseConstant.Action action, ref NS_DM_DU_AN objDMDuAn, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_DM_DU_AN_CT;
                request.Action = action;
                request.objDMDuAn = objDMDuAn;

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    objDMDuAn = response.objDMDuAn;
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

        public bool DanhSachDuAn(DatabaseConstant.Action action, ref List<NS_DM_DU_AN> lstDMDuAn, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_DM_DU_AN_DS;
                request.Action = action;
                request.lstDMDuAn = lstDMDuAn.ToArray();

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

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

        #region Danh mục chức vụ dự án
        public bool ChucVuDuAn(DatabaseConstant.Action action, ref NS_DM_CHUC_VU_DU_AN objDMChucVuDuAn, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_DM_CHUC_VU_DU_AN_CT;
                request.Action = action;
                request.objDMChucVuDuAn = objDMChucVuDuAn;

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    objDMChucVuDuAn = response.objDMChucVuDuAn;
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

        public bool DanhSachChucVuDuAn(DatabaseConstant.Action action, ref List<NS_DM_CHUC_VU_DU_AN> lstDMChucVuDuAn, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_DM_CHUC_VU_DU_AN_DS;
                request.Action = action;
                request.lstDMChucVuDuAn = lstDMChucVuDuAn.ToArray();

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

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

        public DataSet GetDanhSachChucVuDuAn(string maDuAn, string maDonVi)
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
                LDatatable.AddParameter(ref dt, "@MA_DU_AN", "STRING", maDuAn);
                LDatatable.AddParameter(ref dt, "@MA_DVI", "STRING", maDonVi);

                request.dtThamSo = dt;
                request.objectName = "sp_INQ.DS.NS_DANH_MUC";
                request.inquiryName = "CHUC_VU_DU_AN1";

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

        #region Danh mục chức vụ cộng tác viên
        public bool ChucVuCongTacVien(DatabaseConstant.Action action, ref NS_DM_CHUC_VU_CTV objDMChucVuCongTacVien, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_DM_CHUC_VU_CTV_CT;
                request.Action = action;
                request.objDMChucVuCongTacVien = objDMChucVuCongTacVien;

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    objDMChucVuCongTacVien = response.objDMChucVuCongTacVien;
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

        public bool DanhSachChucVuCongTacVien(DatabaseConstant.Action action, ref List<NS_DM_CHUC_VU_CTV> lstDMChucVuCongTacVien, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_DM_CHUC_VU_CTV_DS;
                request.Action = action;
                request.lstDMChucVuCongTacVien = lstDMChucVuCongTacVien.ToArray();

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

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

        #region Danh mục danh mục
        public bool DanhMuc(DatabaseConstant.Action action, ref NS_DM_DANH_MUC objDMDanhMuc, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_DM_DANH_MUC_PL;
                request.Action = action;
                request.objDMDanhMuc = objDMDanhMuc;

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    objDMDanhMuc = response.objDMDanhMuc;
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

        public bool DanhSachDanhMuc(DatabaseConstant.Action action, ref List<NS_DM_DANH_MUC> lstDMDanhMuc, ref List<ClientResponseDetail> listClientResponseDetail)
        {
            NhanSuServiceClient client = null;
            NhanSuRequest request = null;
            NhanSuResponse response = null;

            try
            {
                // Kiểm tra kết nối, server, service trước khi request
                Common.Utilities.IsRequestAllow(ApplicationConstant.SystemService.NhanSuService.layGiaTri());

                client = NhanSuServiceClient(ApplicationConstant.SystemService.NhanSuService);
                request = Common.Utilities.PrepareRequest(new NhanSuServiceRef.NhanSuRequest());
                response = new NhanSuServiceRef.NhanSuResponse();

                //Khởi tạo request
                request.Function = DatabaseConstant.Function.NS_DM_DANH_MUC_DS;
                request.Action = action;
                request.lstDMDanhMuc = lstDMDanhMuc.ToArray();

                // make a call to service client here
                response = client.NhanSu(request);

                //Kiểm tra kết quả trả về
                Common.Utilities.ValidResponse(request, response);

                if (response != null && response.ResponseStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    lstDMDanhMuc = response.lstDMDanhMuc.ToList();
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
