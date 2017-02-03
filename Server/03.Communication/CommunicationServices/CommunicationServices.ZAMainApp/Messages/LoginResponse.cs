using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using CommunicationMessages.Base.MessageBases;
using DataModel.EntityFramework;
using BusinessServices.Utilities.DTO;

namespace CommunicationServices.ZAMainApp.Messages
{
    [DataContract]
    public class LoginResponse : ResponseBase
    {
        [DataMember]
        public HT_NSD NguoiSuDung;

        // Phòng giao dịch
        [DataMember]
        public DM_DON_VI DonViGiaoDich;

        // Chi nhánh giao dịch
        [DataMember]
        public DM_DON_VI DonVi;

        // Đơn vị quản lý
        [DataMember]
        public DM_DON_VI DonViQuanLy;

        // Đơn vị root
        [DataMember]
        public DM_DON_VI DonViRoot;

        [DataMember]
        public List<DM_DON_VI> ListPhongGD;

        [DataMember]
        public List<ChucNangDto> ListChucNang;

        [DataMember]
        public NGON_NGU_DTO NgonNguDTO;

        [DataMember]
        public DSACH_SHOW_CONFIG ShowConfigDTO;

        // SessionId của phiên làm việc
        [DataMember]
        public string SessionId = "";

        // Các thông tin nghiệp vụ
        [DataMember]
        public string NgayLamViecTruoc;
        [DataMember]
        public string NgayLamViecHienTai;
        [DataMember]
        public string NgayLamViecSau;
        [DataMember]
        public string MaDongNoiTe;
        [DataMember]
        public string MaQuocGiaBanDia;
    }
}
