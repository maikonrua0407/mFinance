using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities.Common;
using Presentation.Process.BaoCaoServiceRef;
using Presentation.Process;
using Presentation.Process.Common;
using System.IO;
using PresentationWPF.CustomControl;
using System.Data;

namespace PresentationWPF.BaoCao.DungChung
{
    public class VanHanhGiaoDich
    {
        BaoCaoProcess process = new BaoCaoProcess();

        public void GiaoDichPhatSinh(ref DoiTuongBaoCao doiTuongBaoCao)
        {
            try
            {
                ApplicationConstant.ResponseStatus retStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                FileBase fileResponse = new FileBase();
                string responseMessage = null;

                // Thêm một số thông tin khác về đối tượng báo cáo
                GIAO_DICH_BASE giaoDichBase = doiTuongBaoCao.objGIAO_DICH_BASE != null ? doiTuongBaoCao.objGIAO_DICH_BASE : new GIAO_DICH_BASE();
                giaoDichBase.MaNguoiDung = ClientInformation.TenDangNhap;
                giaoDichBase.TenNguoiDung = ClientInformation.HoTen;
                giaoDichBase.NgonNgu = ClientInformation.NgonNgu;
                giaoDichBase.DinhDang = ApplicationConstant.LoaiDinhDangBaoCao.PDF.layGiaTri();
                giaoDichBase.NgayThucHienBaoCao = ClientInformation.NgayLamViecHienTai;

                doiTuongBaoCao.objGIAO_DICH_BASE = giaoDichBase;

                retStatus = process.LayDuLieuVanHanhGiaoDich(doiTuongBaoCao, ref fileResponse, ref responseMessage);

                if (retStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    string fileReport = ClientInformation.TempDir + "\\" + fileResponse.FileName + "." + fileResponse.FileFormat;
                    LFile.WriteFileFromByteArray(fileResponse.FileData, fileReport);

                    // show file
                    Stream stream = LFile.ConvertByteArrayToStream(fileResponse.FileData);
                    System.Diagnostics.Process.Start(fileReport);
                }
                else
                {
                    LMessage.ShowMessage(responseMessage, LMessage.MessageBoxType.Error);
                    return;
                }
            }
            catch (System.Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                GC.Collect();
                CommonFunction.ThongBaoLoi(ex);                
            }            
        }

        /// <summary>
        /// Chung cho thực hiện các thông tin, báo cáo in ngay
        /// </summary>
        /// <param name="maBaoCao"></param>
        /// <param name="listThamSoBaoCao"></param>
        public void LayDuLieu(string maBaoCao, List<ThamSoBaoCao> listThamSoBaoCao)
        {
            try
            {
                DatabaseConstant.Action action = DatabaseConstant.Action.IN;

                // Lấy thông tin báo cáo và tham số
                BaoCaoProcess process = new BaoCaoProcess();
                int idBaoCao = 0;
                HT_BAOCAO htBaoCao = null;
                List<HT_BAOCAO_TSO> lstHtBaoCaoTso = null;
                process.LayThongTinBaoCao(idBaoCao, maBaoCao, ref htBaoCao, ref lstHtBaoCaoTso);

                List<HT_BAOCAO_TSO> listHtBaoCaoTso = lstHtBaoCaoTso;
                DataSet ds = new DataSet();

                // Chuẩn bị điều kiện cho báo cáo
                if (listThamSoBaoCao != null && listThamSoBaoCao.Count > 0)
                {
                    if (listHtBaoCaoTso.Where(t => t.LOAI_TSO == ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri() && t.MA_TSO.Equals("@DT_THAMSO")).Count() > 0)
                    {
                        listHtBaoCaoTso = new List<HT_BAOCAO_TSO>();
                        foreach (ThamSoBaoCao thamSoBaoCao in listThamSoBaoCao)
                        {
                            HT_BAOCAO_TSO tso = new HT_BAOCAO_TSO();
                            tso.MA_TSO = thamSoBaoCao.MaThamSo;
                            tso.LOAI_TSO = thamSoBaoCao.LoaiThamSo;
                            tso.GTRI_TSO = thamSoBaoCao.GiaTriThamSo;
                            listHtBaoCaoTso.Add(tso);
                        }
                    }
                    else
                    {
                        foreach (HT_BAOCAO_TSO htBaoCaoTso in listHtBaoCaoTso)
                        {
                            foreach (ThamSoBaoCao thamSoBaoCao in listThamSoBaoCao)
                            {
                                if (htBaoCaoTso.MA_TSO.Equals(thamSoBaoCao.MaThamSo) &&
                                    htBaoCaoTso.LOAI_TSO.Equals(thamSoBaoCao.LoaiThamSo))
                                {
                                    htBaoCaoTso.GTRI_TSO = thamSoBaoCao.GiaTriThamSo;
                                    break;
                                }
                                if (!LObject.IsNullOrEmpty(thamSoBaoCao.DsThamSo))
                                    ds = thamSoBaoCao.DsThamSo;
                            }
                        }
                    }
                }
                else
                {
                    return;
                }

                ApplicationConstant.ResponseStatus retStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                FileBase fileResponse = new FileBase();
                List<FileBase> lstFileResponse = new List<FileBase>();
                string responseMessage = null;

                retStatus = process.LayDuLieu(htBaoCao, listHtBaoCaoTso, ref fileResponse, ref responseMessage, ds, action);

                if (retStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    string fileReport = ClientInformation.TempDir + "\\" + fileResponse.FileName + "." + fileResponse.FileFormat; ;
                    LFile.WriteFileFromByteArray(fileResponse.FileData, fileReport);

                    // show file
                    Stream stream = LFile.ConvertByteArrayToStream(fileResponse.FileData);
                    System.Diagnostics.Process.Start(fileReport);
                }
                else
                {
                    LMessage.ShowMessage(responseMessage, LMessage.MessageBoxType.Error);
                    return;
                }
            }
            catch (System.Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                GC.Collect();
                CommonFunction.ThongBaoLoi(ex);
            }
        }

        /// <summary>
        /// Riêng cho thực hiện các chứng từ kế toán
        /// </summary>
        /// <param name="maBaoCao"></param>
        /// <param name="listThamSoBaoCao"></param>
        public void LayDuLieuGiaoDich(string maBaoCao, List<ThamSoBaoCao> listThamSoBaoCao)
        {
            try
            {
                DatabaseConstant.Action action = DatabaseConstant.Action.IN;

                // Lấy thông tin báo cáo và tham số
                BaoCaoProcess process = new BaoCaoProcess();
                int idBaoCao = 0;
                HT_BAOCAO htBaoCao = null;
                List<HT_BAOCAO_TSO> lstHtBaoCaoTso = null;
                process.LayThongTinBaoCao(idBaoCao, maBaoCao, ref htBaoCao, ref lstHtBaoCaoTso);

                List<HT_BAOCAO_TSO> listHtBaoCaoTso = lstHtBaoCaoTso;
                DataSet ds = new DataSet();

                // Chuẩn bị điều kiện cho báo cáo
                if (listThamSoBaoCao != null && listThamSoBaoCao.Count > 0)
                {
                    if (listHtBaoCaoTso.Where(t => t.LOAI_TSO == ApplicationConstant.LoaiThamSoBaoCao.SQL.layGiaTri() && t.MA_TSO.Equals("@DT_THAMSO")).Count() > 0)
                    {
                        listHtBaoCaoTso = new List<HT_BAOCAO_TSO>();
                        foreach (ThamSoBaoCao thamSoBaoCao in listThamSoBaoCao)
                        {
                            HT_BAOCAO_TSO tso = new HT_BAOCAO_TSO();
                            tso.MA_TSO = thamSoBaoCao.MaThamSo;
                            tso.LOAI_TSO = thamSoBaoCao.LoaiThamSo;
                            tso.GTRI_TSO = thamSoBaoCao.GiaTriThamSo;
                            listHtBaoCaoTso.Add(tso);
                        }
                    }
                    else
                    {
                        foreach (HT_BAOCAO_TSO htBaoCaoTso in listHtBaoCaoTso)
                        {
                            foreach (ThamSoBaoCao thamSoBaoCao in listThamSoBaoCao)
                            {
                                if (htBaoCaoTso.MA_TSO.Equals(thamSoBaoCao.MaThamSo) &&
                                    htBaoCaoTso.LOAI_TSO.Equals(thamSoBaoCao.LoaiThamSo))
                                {
                                    htBaoCaoTso.GTRI_TSO = thamSoBaoCao.GiaTriThamSo;
                                    break;
                                }
                                if (!LObject.IsNullOrEmpty(thamSoBaoCao.DsThamSo))
                                    ds = thamSoBaoCao.DsThamSo;
                            }
                        }
                    }
                }
                else
                {
                    return;
                }

                ApplicationConstant.ResponseStatus retStatus = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                FileBase fileResponse = new FileBase();
                List<FileBase> lstFileResponse = new List<FileBase>();
                string responseMessage = null;

                retStatus = process.LayDuLieu(htBaoCao, listHtBaoCaoTso, ref fileResponse, ref responseMessage, ds, action);

                if (retStatus == ApplicationConstant.ResponseStatus.THANH_CONG)
                {
                    string fileReport = ClientInformation.TempDir + "\\" + fileResponse.FileName + "." + fileResponse.FileFormat; ;
                    LFile.WriteFileFromByteArray(fileResponse.FileData, fileReport);

                    // show file
                    Stream stream = LFile.ConvertByteArrayToStream(fileResponse.FileData);
                    System.Diagnostics.Process.Start(fileReport);
                }
                else
                {
                    LMessage.ShowMessage(responseMessage, LMessage.MessageBoxType.Error);
                    return;
                }
            }
            catch (System.Exception ex)
            {
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
                GC.Collect();
                CommonFunction.ThongBaoLoi(ex);
            }
        }
    }
}
