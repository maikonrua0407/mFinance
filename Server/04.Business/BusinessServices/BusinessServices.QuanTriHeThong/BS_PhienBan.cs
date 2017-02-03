using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities.Common;
using Hosts.Startup;
using DataModel.EntityFramework;
using BusinessServices.Utilities;
using BusinessServices.Utilities.DTO;
using DataServices.QuanTriHeThong;
using System.IO;

namespace BusinessServices.QuanTriHeThong
{
    public class BS_PhienBan
    {
        private string MA_FILE_SETUP = "FS0001";

        public ApplicationConstant.ResponseStatus CheckClientVersion(string clientVersion,
            ref string serverVersion,
            ref string lastestClientVersion,
            ref HT_PBAN htPban,
            ref List<HT_PBAN_CTIET> listHtPbanCtiet,
            ref List<HT_PBAN_FILE> listHtPbanFile,
            ref ApplicationConstant.QuanTriHeThongResponseMessage responseMessage)
        {
            // Lấy thông tin người dùng
            serverVersion = HostInformation.Version;

            List<HT_PBAN_MAPPING> listHtPbanMapping = new List<HT_PBAN_MAPPING>();
            new BS_CoHeThong().LayDanhSachPhienBanClientPhuHop(HostInformation.Version, ref listHtPbanMapping);
            List<string> listClientVersion = listHtPbanMapping.Select(e => e.MA_PBAN_CLIENT).ToList();
            lastestClientVersion = listClientVersion.LastOrDefault();

            if (lastestClientVersion == null)
            {
                responseMessage = ApplicationConstant.QuanTriHeThongResponseMessage.M_ResponseMessage_QuanTriHeThong_PhienBan_KiemTraKhongThanhCong;
                return ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
            }

            else
            {
                if (!clientVersion.Equals(lastestClientVersion))
                {
                    // Lấy các thông tin chi tiết cho phiên bản mới
                    htPban = new DS_HT_PBAN().GetByMa(lastestClientVersion);
                    listHtPbanCtiet = new DS_HT_PBAN_CTIET().GetListByMaPban(lastestClientVersion);
                    listHtPbanFile = new DS_HT_PBAN_FILE().GetListByMaPban(lastestClientVersion);
                }
                responseMessage = ApplicationConstant.QuanTriHeThongResponseMessage.M_ResponseMessage_QuanTriHeThong_ThanhCong;
                return ApplicationConstant.ResponseStatus.THANH_CONG;
            }
        }

        public ApplicationConstant.ResponseStatus DownloadClientVersion(string clientVersion,
            string lastestClientVersion,
            HT_PBAN htPban,
            ref BS_PhienBanDTO bsPhienBan,
            ref ApplicationConstant.QuanTriHeThongResponseMessage responseMessage)
        {
            try 
            { 
                string phuongThucCapNhat = htPban.PTHUC_CNHAT;
            
                // Nếu cập nhật toàn bộ ứng dụng
                if (phuongThucCapNhat.Equals("FULL"))
                {
                    List<HT_PBAN_CTIET> listHtPbanCtiet = new DS_HT_PBAN_CTIET().GetListByMaPban(htPban.MA_PBAN);
                    List<HT_PBAN_FILE> listHtPbanFile = new DS_HT_PBAN_FILE().GetListByMaPban(htPban.MA_PBAN);

                    if (listHtPbanCtiet == null || listHtPbanCtiet.Count == 0 || listHtPbanCtiet.Count > 1 ||
                        listHtPbanFile == null || listHtPbanFile.Count == 0 || listHtPbanFile.Count > 1)
                    {
                        responseMessage = ApplicationConstant.QuanTriHeThongResponseMessage.M_ResponseMessage_QuanTriHeThong_PhienBan_ThongTinKhongChinhXac;
                        return ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                    }
                    else if (listHtPbanCtiet.Count == 1 && listHtPbanFile.Count == 1)
                    {
                        HT_PBAN_CTIET htPbanCtiet = listHtPbanCtiet.FirstOrDefault();
                        HT_PBAN_FILE htPbanFile = listHtPbanFile.FirstOrDefault();
                        string filePath = HostInformation.OtaClientVersionDir + "\\" + htPban.MA_PBAN + "\\" + htPbanFile.TEN_FILE;

                        byte[] fileData = LFile.GetByteArrayFromFile(filePath);

                        BS_FileObject filePhienBan = new BS_FileObject();
                        filePhienBan.FileData = fileData;
                        filePhienBan.FileFormat = Path.GetExtension(filePath);
                        filePhienBan.FileName = htPbanFile.TEN_FILE;

                        BS_PhienBanItemDTO bsPhienBanItemDTO = new BS_PhienBanItemDTO();
                        bsPhienBanItemDTO.HtPbanCtiet = htPbanCtiet;
                        bsPhienBanItemDTO.HtPbanFile = htPbanFile;
                        bsPhienBanItemDTO.HtPbanData = filePhienBan;

                        bsPhienBan.HtPban = htPban;
                        List<BS_PhienBanItemDTO> ListPhienBanItem = new List<BS_PhienBanItemDTO>();
                        ListPhienBanItem.Add(bsPhienBanItemDTO);
                        bsPhienBan.ListPhienBanItem = ListPhienBanItem;

                        return ApplicationConstant.ResponseStatus.THANH_CONG;
                    }
                }
                // Nếu cập nhật phần thay đổi của ứng dụng
                else if (phuongThucCapNhat.Equals("CHANGE"))
                {
                    // Nếu phiên bản mới nhất là phiên bản kế tiếp
                    // thì cập nhật CHANGE
                    if (htPban.MA_PBAN_TRUOC != null && htPban.MA_PBAN_TRUOC.Equals(clientVersion))
                    {           
                        List<HT_PBAN_CTIET> listHtPbanCtiet = new DS_HT_PBAN_CTIET().GetListByMaPban(htPban.MA_PBAN);
                        List<HT_PBAN_FILE> listHtPbanFile = new DS_HT_PBAN_FILE().GetListByMaPban(htPban.MA_PBAN);

                        if (listHtPbanCtiet == null || listHtPbanCtiet.Count == 0 ||
                            listHtPbanFile == null || listHtPbanFile.Count == 0 ||
                            listHtPbanCtiet.Count != listHtPbanFile.Count)
                        {
                            responseMessage = ApplicationConstant.QuanTriHeThongResponseMessage.M_ResponseMessage_QuanTriHeThong_PhienBan_ThongTinKhongChinhXac;
                            return ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                        }
                        else if (listHtPbanCtiet.Count > 0 && listHtPbanFile.Count > 0)
                        {
                            List<BS_PhienBanItemDTO> listPhienBanItem = new List<BS_PhienBanItemDTO>();
                            for (int i = 0; i < listHtPbanCtiet.Count; i++)
                            {
                                HT_PBAN_CTIET htPbanCtiet = listHtPbanCtiet[i];
                                HT_PBAN_FILE htPbanFile = listHtPbanFile[i];
                                if (!htPbanCtiet.MA_FILE.Equals(MA_FILE_SETUP))
                                {
                                    string filePath = HostInformation.OtaClientVersionDir + "\\" + htPban.MA_PBAN + "\\" + ((htPbanFile.DUONG_DAN != null && !htPbanFile.DUONG_DAN.Equals("")) ? htPbanFile.DUONG_DAN + "\\" : "") + htPbanFile.TEN_FILE;

                                    byte[] fileData = LFile.GetByteArrayFromFile(filePath);

                                    BS_FileObject filePhienBan = new BS_FileObject();
                                    filePhienBan.FileData = fileData;
                                    filePhienBan.FileFormat = Path.GetExtension(filePath);
                                    filePhienBan.FileName = htPbanFile.TEN_FILE;

                                    BS_PhienBanItemDTO bsPhienBanItemDTO = new BS_PhienBanItemDTO();
                                    bsPhienBanItemDTO.HtPbanCtiet = htPbanCtiet;
                                    bsPhienBanItemDTO.HtPbanFile = htPbanFile;
                                    bsPhienBanItemDTO.HtPbanData = filePhienBan;

                                    listPhienBanItem.Add(bsPhienBanItemDTO);        
                                }
                            }

                            bsPhienBan.HtPban = htPban;
                            bsPhienBan.ListPhienBanItem = listPhienBanItem;                   

                            return ApplicationConstant.ResponseStatus.THANH_CONG;
                        }
                    }
                    // Nếu phiên bản mới nhất không phải là phiên bản kế tiếp
                    // thì cập nhật FULL
                    else if (htPban.MA_PBAN_TRUOC == null || (htPban.MA_PBAN_TRUOC != null && !htPban.MA_PBAN_TRUOC.Equals(clientVersion)))
                    {
                        List<HT_PBAN_CTIET> listHtPbanCtiet = new DS_HT_PBAN_CTIET().GetListByMaPban(htPban.MA_PBAN);
                        List<HT_PBAN_FILE> listHtPbanFile = new DS_HT_PBAN_FILE().GetListByMaPban(htPban.MA_PBAN);

                        if (listHtPbanCtiet == null || listHtPbanCtiet.Count == 0 ||
                            listHtPbanFile == null || listHtPbanFile.Count == 0 ||
                            listHtPbanCtiet.Count != listHtPbanFile.Count)
                        {
                            responseMessage = ApplicationConstant.QuanTriHeThongResponseMessage.M_ResponseMessage_QuanTriHeThong_PhienBan_ThongTinKhongChinhXac;
                            return ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                        }
                        else if (listHtPbanCtiet.Count > 0 && listHtPbanFile.Count > 0)
                        {
                            HT_PBAN_CTIET htPbanCtiet = listHtPbanCtiet.First(e => e.MA_FILE.Equals(MA_FILE_SETUP));
                            HT_PBAN_FILE htPbanFile = listHtPbanFile.First(e => e.MA_FILE.Equals(MA_FILE_SETUP));

                            if (htPbanCtiet == null || htPbanFile == null)
                            {
                                responseMessage = ApplicationConstant.QuanTriHeThongResponseMessage.M_ResponseMessage_QuanTriHeThong_PhienBan_ThongTinKhongChinhXac;
                                return ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                            }

                            string filePath = HostInformation.OtaClientVersionDir + "\\" + htPban.MA_PBAN + "\\" + htPbanFile.TEN_FILE;

                            byte[] fileData = LFile.GetByteArrayFromFile(filePath);

                            BS_FileObject filePhienBan = new BS_FileObject();
                            filePhienBan.FileData = fileData;
                            filePhienBan.FileFormat = Path.GetExtension(filePath);
                            filePhienBan.FileName = htPbanFile.TEN_FILE;

                            BS_PhienBanItemDTO bsPhienBanItemDTO = new BS_PhienBanItemDTO();
                            bsPhienBanItemDTO.HtPbanCtiet = htPbanCtiet;
                            bsPhienBanItemDTO.HtPbanFile = htPbanFile;
                            bsPhienBanItemDTO.HtPbanData = filePhienBan;

                            bsPhienBan.HtPban = htPban;
                            List<BS_PhienBanItemDTO> ListPhienBanItem = new List<BS_PhienBanItemDTO>();
                            ListPhienBanItem.Add(bsPhienBanItemDTO);
                            bsPhienBan.ListPhienBanItem = ListPhienBanItem;

                            return ApplicationConstant.ResponseStatus.THANH_CONG;
                        }
                    }
                }
                // Còn lại
                else
                {
                    responseMessage = ApplicationConstant.QuanTriHeThongResponseMessage.M_ResponseMessage_QuanTriHeThong_PhienBan_KiemTraKhongThanhCong;
                    return ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                }
                return ApplicationConstant.ResponseStatus.THANH_CONG;
            }
            catch (Exception ex)
            {
                responseMessage = ApplicationConstant.QuanTriHeThongResponseMessage.M_ResponseMessage_QuanTriHeThong_PhienBan_ThongTinKhongChinhXac;
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
                throw ex;
            }
        }

        public ApplicationConstant.ResponseStatus DownloadClientVersionItem(string clientVersion,
            string lastestClientVersion,
            HT_PBAN htPban,
            HT_PBAN_CTIET htPbanCtiet,
            ref BS_PhienBanItemDTO bsPhienBanItem,
            ref ApplicationConstant.QuanTriHeThongResponseMessage responseMessage)
        {
            return ApplicationConstant.ResponseStatus.THANH_CONG;
        }
    }
}
