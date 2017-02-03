using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities.Common
{
    public static class DatabaseConstant
    {
        /// <summary>
        /// Danh sách phân hệ nghiệp vụ
        /// </summary>M
        public enum Module
        {
            //FINA, //Phân hệ nghiệp vụ
            QTHT, //Quản trị hệ thống
            DMDC, //Danh mục dùng chung
            KHTV, //Khách hàng thành viên
            GDKT, //Giao dịch kế toán
            //NQUY, //Ngân quỹ
            //GVCP, //Góp vốn cổ phần
            HDVO, //Huy động vốn 
            TDVM, //Tín dụng vi mô
            TDTT, //Tín dụng thông thường
            TDTD, //Tín dụng tiêu dùng
            //TSDB, //Tài sản đảm bảo            
            //TGUI, //Tiền gửi TCTD khác
            //TVAY, //Tiền vay TCTD khác
            BHTH, //Bảo hiểm tương hỗ
            NSTL, //Nhân sự tiền lương
            TSDB, //Tài sản đảm bảo
            //DCQU, //Điều chuyển quỹ
            //TKKT, //Tài khoản kế toán
            KTDL, //Khai thác dữ liệu
            PTHU, // Phiếu thu
            PCHI, // Phiếu chi
            PKET, // Phiếu kế toán
            UNCHI, // Phiếu ủy nhiệm chi
            NBANG, // Nhập xuất ngoại bảng
            DCHINH, // Bút toán điều chỉnh
            QLTS, //Quản lý tài sản
            KCHUYEN, //Kết chuyển
            HMUC, //Hạn mức
            SMS //SMS
        }

        public static string getValue(this Module module)
        {
            switch (module)
            {
                //case Module.FINA: return "FINA"; //Phân hệ nghiệp vụ
                case Module.QTHT: return "QTHT"; //Quản trị hệ thống
                case Module.DMDC: return "DMDC"; //Danh mục dùng chung
                case Module.KHTV: return "KHTV"; //Khách hàng thành viên
                case Module.KTDL: return "KTDL"; //Khai thác dữ liệu
                //case Module.NQUY: return "NQUY"; //Ngân quỹ
                //case Module.GVCP: return "GVCP"; //Góp vốn cổ phần
                case Module.HDVO: return "HDVO"; //Huy động vốn 
                case Module.TDVM: return "TDVM"; //Tín dụng vi mô
                case Module.TDTT: return "TDTT"; //Tín dụng thông thường
                case Module.TDTD: return "TDTD"; //Tín dụng tiêu dùng
                //case Module.TSDB: return "TSDB"; //Tài sản đảm bảo
                case Module.GDKT: return "GDKT"; //Giao dịch kế toán
                //case Module.TGUI: return "TGUI"; //Tiền gửi TCTD khác
                //case Module.TVAY: return "TVAY"; //Tiền vay TCTD khác
                case Module.BHTH: return "BHTH"; //Bảo hiểm tương hỗ
                case Module.NSTL: return "NSTL"; //Nhân sự tiền lương
                case Module.TSDB: return "TSDB"; //Tài sản đảm bảo
                //case Module.DCQU: return "DCQU"; //Điều chuyển quỹ
                //case Module.TKKT: return "TKKT"; //Tài khoản kế toán
                case Module.PTHU: return "PTHU"; //Phiếu thu
                case Module.PCHI: return "PCHI"; //Phiếu chi
                case Module.PKET: return "PKET"; //Phiếu kế toán
                case Module.UNCHI: return "UNCHI"; //Phiếu ủy nhiệm chi
                case Module.NBANG: return "NBANG"; //Phiếu kế toán
                case Module.DCHINH: return "DCHINH"; //Điều chỉnh
                case Module.QLTS: return "QLTS"; //Tài sản
                case Module.KCHUYEN: return "KCHUYEN"; //Kết chuyển
                case Module.HMUC: return "HMUC"; //Hạn mức
                case Module.SMS: return "SMS";
                default: return "TDVM";
            }
        }

        public static Module getModule(string module)
        {
            switch (module)
            {
                case "QTHT": return Module.QTHT; //Quản trị hệ thống
                case "DMDC": return Module.DMDC; //Danh mục dùng chung
                case "KHTV": return Module.KHTV; //Khách hàng thành viên
                case "GDKT": return Module.GDKT; //Giao dịch kế toán
                //case "NQUY": return Module.NQUY; //Ngân quỹ
                //case "GVCP": return Module.GVCP; //Góp vốn cổ phần
                case "HDVO": return Module.HDVO; //Huy động vốn 
                case "TDVM": return Module.TDVM; //Tín dụng vi mô
                case "TDTT": return Module.TDTT; //Tín dụng thông thường
                case "TDTD": return Module.TDTD; //Tín dụng tiêu dùng
                //case "TSDB": return Module.TSDB; //Tài sản đảm bảo            
                //case "TGUI": return Module.TGUI; //Tiền gửi TCTD khác
                //case "TVAY": return Module.TVAY; //Tiền vay TCTD khác
                case "BHTH": return Module.BHTH; //Bảo hiểm tương hỗ
                case "NSTL": return Module.NSTL; //Nhân sự tiền lương
                case "TSDB": return Module.TSDB; //Nhân sự tiền lương
                //case "DCQU": return Module.DCQU; //Điều chuyển quỹ
                //case "TKKT": return Module.TKKT; //Tài khoản kế toán
                case "KTDL": return Module.KTDL; //Khai thác dữ liệu
                case "PTHU": return Module.PTHU; //Phiếu thu
                case "PCHI": return Module.PCHI; //Phiếu chi
                case "PKET": return Module.PKET; //Phiếu kế toán
                case "UNCHI": return Module.UNCHI; //Phiếu ủy nhiệm chi
                case "NBANG": return Module.NBANG; //Phiếu kế toán
                case "DCHINH": return Module.DCHINH; //Điều chỉnh
                case "QLTS": return Module.QLTS; //Tài sản
                case "KCHUYEN": return Module.KCHUYEN; //Kết chuyển
                case "HMUC": return Module.HMUC; //Hạn mức
                case "SMS": return Module.SMS; //SMS
                default: return Module.TDVM;
            }
        }


        /// <summary>
        /// enum cho module báo cáo khai thác dữ liệu
        /// </summary>
        /// <param name="phanHeKhaiThacDuLieu"></param>
        /// <returns></returns>
        public enum PhanHeKhaiThacDuLieu
        {
            KTDL_QTHT,
            //KTDL_DMDC,
            KTDL_KHTV,
            KTDL_GDKT,
            KTDL_HDVO,
            KTDL_TDVM,
            KTDL_TDTT,
            KTDL_BHTH,
            KTDL_NSTL,
            KTDL_QLTS,
            KTDL_BCTK,
            KTDL_BCTH,
            KTDL_BCTC,
            KTDL_CICR,
            KTDL_BHTG
        }
        public static string getStringPhanHeKhaiThacDuLieu(this PhanHeKhaiThacDuLieu phanHeKhaiThacDuLieu)
        {
            switch (phanHeKhaiThacDuLieu)
            {
                case PhanHeKhaiThacDuLieu.KTDL_QTHT: return "KTDL_QTHT";
                //case PhanHeKhaiThacDuLieu.KTDL_DMDC: return "KTDL_DMDC";
                case PhanHeKhaiThacDuLieu.KTDL_KHTV: return "KTDL_KHTV";
                case PhanHeKhaiThacDuLieu.KTDL_GDKT: return "KTDL_GDKT";
                case PhanHeKhaiThacDuLieu.KTDL_HDVO: return "KTDL_HDVO";
                case PhanHeKhaiThacDuLieu.KTDL_TDVM: return "KTDL_TDVM";
                case PhanHeKhaiThacDuLieu.KTDL_TDTT: return "KTDL_TDTT";
                case PhanHeKhaiThacDuLieu.KTDL_BHTH: return "KTDL_BHTH";
                case PhanHeKhaiThacDuLieu.KTDL_NSTL: return "KTDL_NSTL";
                case PhanHeKhaiThacDuLieu.KTDL_QLTS: return "KTDL_QLTS";
                case PhanHeKhaiThacDuLieu.KTDL_BCTK: return "KTDL_BCTK";
                case PhanHeKhaiThacDuLieu.KTDL_BCTH: return "KTDL_BCTH";
                case PhanHeKhaiThacDuLieu.KTDL_BCTC: return "KTDL_BCTC";
                case PhanHeKhaiThacDuLieu.KTDL_CICR: return "KTDL_CICR";
                case PhanHeKhaiThacDuLieu.KTDL_BHTG: return "KTDL_BHTG";
                default: return "";
            }
        }
        public static PhanHeKhaiThacDuLieu getValuePhanHeKhaiThacDuLieu(string phanHeKhaiThacDuLieu)
        {
            switch (phanHeKhaiThacDuLieu)
            {
                case "KTDL_QTHT": return PhanHeKhaiThacDuLieu.KTDL_QTHT;
                //case "KTDL_DMDC": return PhanHeKhaiThacDuLieu.KTDL_DMDC;
                case "KTDL_KHTV": return PhanHeKhaiThacDuLieu.KTDL_KHTV;
                case "KTDL_GDKT": return PhanHeKhaiThacDuLieu.KTDL_GDKT;
                case "KTDL_HDVO": return PhanHeKhaiThacDuLieu.KTDL_HDVO;
                case "KTDL_TDVM": return PhanHeKhaiThacDuLieu.KTDL_TDVM;
                case "KTDL_TDTT": return PhanHeKhaiThacDuLieu.KTDL_TDTT;
                case "KTDL_BHTH": return PhanHeKhaiThacDuLieu.KTDL_BHTH;
                case "KTDL_NSTL": return PhanHeKhaiThacDuLieu.KTDL_NSTL;
                case "KTDL_QLTS": return PhanHeKhaiThacDuLieu.KTDL_QLTS;
                case "KTDL_BCTK": return PhanHeKhaiThacDuLieu.KTDL_BCTK;
                case "KTDL_BCTH": return PhanHeKhaiThacDuLieu.KTDL_BCTH;
                case "KTDL_BCTC": return PhanHeKhaiThacDuLieu.KTDL_BCTC;
                case "KTDL_CICR": return PhanHeKhaiThacDuLieu.KTDL_CICR;
                case "KTDL_BHTG": return PhanHeKhaiThacDuLieu.KTDL_BHTG;
                default: return PhanHeKhaiThacDuLieu.KTDL_QTHT;
            }
        }
        public static string getLanguagePhanHeKhaiThacDuLieu(this PhanHeKhaiThacDuLieu phanHeKhaiThacDuLieu)
        {
            switch (phanHeKhaiThacDuLieu)
            {
                case PhanHeKhaiThacDuLieu.KTDL_QTHT: return "U.PhanHeKhaiThacDuLieu.KTDL_QTHT";
                //case PhanHeKhaiThacDuLieu.KTDL_DMDC: return "U.PhanHeKhaiThacDuLieu.KTDL_DMDC";
                case PhanHeKhaiThacDuLieu.KTDL_KHTV: return "U.PhanHeKhaiThacDuLieu.KTDL_KHTV";
                case PhanHeKhaiThacDuLieu.KTDL_GDKT: return "U.PhanHeKhaiThacDuLieu.KTDL_GDKT";
                case PhanHeKhaiThacDuLieu.KTDL_HDVO: return "U.PhanHeKhaiThacDuLieu.KTDL_HDVO";
                case PhanHeKhaiThacDuLieu.KTDL_TDVM: return "U.PhanHeKhaiThacDuLieu.KTDL_TDVM";
                case PhanHeKhaiThacDuLieu.KTDL_TDTT: return "U.PhanHeKhaiThacDuLieu.KTDL_TDTT";
                case PhanHeKhaiThacDuLieu.KTDL_BHTH: return "U.PhanHeKhaiThacDuLieu.KTDL_BHTH";
                case PhanHeKhaiThacDuLieu.KTDL_NSTL: return "U.PhanHeKhaiThacDuLieu.KTDL_NSTL";
                case PhanHeKhaiThacDuLieu.KTDL_QLTS: return "U.PhanHeKhaiThacDuLieu.KTDL_QLTS";
                case PhanHeKhaiThacDuLieu.KTDL_BCTK: return "U.PhanHeKhaiThacDuLieu.KTDL_BCTK";
                case PhanHeKhaiThacDuLieu.KTDL_BCTH: return "U.PhanHeKhaiThacDuLieu.KTDL_BCTH";
                case PhanHeKhaiThacDuLieu.KTDL_BCTC: return "U.PhanHeKhaiThacDuLieu.KTDL_BCTC";
                case PhanHeKhaiThacDuLieu.KTDL_CICR: return "U.PhanHeKhaiThacDuLieu.KTDL_CICR";
                case PhanHeKhaiThacDuLieu.KTDL_BHTG: return "U.PhanHeKhaiThacDuLieu.KTDL_BHTG";
                default: return "";
            }
        }

        /// <summary>
        /// enum cho module báo cáo khai thác dữ liệu
        /// </summary>
        /// <param name="phanHeKhaiThacDuLieu"></param>
        /// <returns></returns>
        public enum PhanHeKhaiThacDuLieuEF
        {
            EF_KTDL_KHTV,
            EF_KTDL_TCKT,
            EF_KTDL_HDVO,
            EF_KTDL_TDTT,
            EF_KTDL_TGUI,
            EF_KTDL_TVAY,
            EF_KTDL_NQUY,
            EF_KTDL_ANTO,
            EF_KTDL_BHTG,
            EF_KTDL_DID,
            EF_KTDL_TVKH,
            EF_KTDL_QTLS
        }
        public static string getStringPhanHeKhaiThacDuLieu(this PhanHeKhaiThacDuLieuEF phanHeKhaiThacDuLieuEF)
        {
            switch (phanHeKhaiThacDuLieuEF)
            {
                case PhanHeKhaiThacDuLieuEF.EF_KTDL_KHTV: return "EF_KTDL_KHTV";
                case PhanHeKhaiThacDuLieuEF.EF_KTDL_TCKT: return "EF_KTDL_TCKT";
                case PhanHeKhaiThacDuLieuEF.EF_KTDL_HDVO: return "EF_KTDL_HDVO";
                case PhanHeKhaiThacDuLieuEF.EF_KTDL_TDTT: return "EF_KTDL_TDTT";
                case PhanHeKhaiThacDuLieuEF.EF_KTDL_TGUI: return "EF_KTDL_TGUI";
                case PhanHeKhaiThacDuLieuEF.EF_KTDL_TVAY: return "EF_KTDL_TVAY";
                case PhanHeKhaiThacDuLieuEF.EF_KTDL_NQUY: return "EF_KTDL_NQUY";
                case PhanHeKhaiThacDuLieuEF.EF_KTDL_ANTO: return "EF_KTDL_ANTO";
                case PhanHeKhaiThacDuLieuEF.EF_KTDL_BHTG: return "EF_KTDL_BHTG";
                case PhanHeKhaiThacDuLieuEF.EF_KTDL_DID: return "EF_KTDL_DID";
                case PhanHeKhaiThacDuLieuEF.EF_KTDL_TVKH: return "EF_KTDL_TVKH";
                case PhanHeKhaiThacDuLieuEF.EF_KTDL_QTLS: return "EF_KTDL_QTLS";
                default: return "";
            }
        }
        public static PhanHeKhaiThacDuLieuEF getValuePhanHeKhaiThacDuLieuEF(string phanHeKhaiThacDuLieuEF)
        {
            switch (phanHeKhaiThacDuLieuEF)
            {
                case "EF_KTDL_KHTV": return PhanHeKhaiThacDuLieuEF.EF_KTDL_KHTV;
                case "EF_KTDL_TCKT": return PhanHeKhaiThacDuLieuEF.EF_KTDL_TCKT;
                case "EF_KTDL_HDVO": return PhanHeKhaiThacDuLieuEF.EF_KTDL_HDVO;
                case "EF_KTDL_TDTT": return PhanHeKhaiThacDuLieuEF.EF_KTDL_TDTT;
                case "EF_KTDL_TGUI": return PhanHeKhaiThacDuLieuEF.EF_KTDL_TGUI;
                case "EF_KTDL_TVAY": return PhanHeKhaiThacDuLieuEF.EF_KTDL_TVAY;
                case "EF_KTDL_NQUY": return PhanHeKhaiThacDuLieuEF.EF_KTDL_NQUY;
                case "EF_KTDL_ANTO": return PhanHeKhaiThacDuLieuEF.EF_KTDL_ANTO;
                case "EF_KTDL_BHTG": return PhanHeKhaiThacDuLieuEF.EF_KTDL_BHTG;
                case "EF_KTDL_DID": return PhanHeKhaiThacDuLieuEF.EF_KTDL_DID;
                case "EF_KTDL_TVKH": return PhanHeKhaiThacDuLieuEF.EF_KTDL_TVKH;
                case "EF_KTDL_QTLS": return PhanHeKhaiThacDuLieuEF.EF_KTDL_QTLS;
                default: return PhanHeKhaiThacDuLieuEF.EF_KTDL_KHTV;
            }
        }
        public static string getLanguagePhanHeKhaiThacDuLieuEF(this PhanHeKhaiThacDuLieuEF phanHeKhaiThacDuLieuEF)
        {
            switch (phanHeKhaiThacDuLieuEF)
            {
                case PhanHeKhaiThacDuLieuEF.EF_KTDL_KHTV: return "U.PhanHeKhaiThacDuLieuEF.EF_KTDL_KHTV";
                case PhanHeKhaiThacDuLieuEF.EF_KTDL_TCKT: return "U.PhanHeKhaiThacDuLieuEF.EF_KTDL_TCKT";
                case PhanHeKhaiThacDuLieuEF.EF_KTDL_HDVO: return "U.PhanHeKhaiThacDuLieuEF.EF_KTDL_HDVO";
                case PhanHeKhaiThacDuLieuEF.EF_KTDL_TDTT: return "U.PhanHeKhaiThacDuLieuEF.EF_KTDL_TDTT";
                case PhanHeKhaiThacDuLieuEF.EF_KTDL_TGUI: return "U.PhanHeKhaiThacDuLieuEF.EF_KTDL_TGUI";
                case PhanHeKhaiThacDuLieuEF.EF_KTDL_TVAY: return "U.PhanHeKhaiThacDuLieuEF.EF_KTDL_TVAY";
                case PhanHeKhaiThacDuLieuEF.EF_KTDL_NQUY: return "U.PhanHeKhaiThacDuLieuEF.EF_KTDL_NQUY";
                case PhanHeKhaiThacDuLieuEF.EF_KTDL_ANTO: return "U.PhanHeKhaiThacDuLieuEF.EF_KTDL_ANTO";
                case PhanHeKhaiThacDuLieuEF.EF_KTDL_BHTG: return "U.PhanHeKhaiThacDuLieuEF.EF_KTDL_BHTG";
                case PhanHeKhaiThacDuLieuEF.EF_KTDL_DID: return "U.PhanHeKhaiThacDuLieuEF.EF_KTDL_DID";
                case PhanHeKhaiThacDuLieuEF.EF_KTDL_TVKH: return "U.PhanHeKhaiThacDuLieuEF.EF_KTDL_TVKH";
                case PhanHeKhaiThacDuLieuEF.EF_KTDL_QTLS: return "U.PhanHeKhaiThacDuLieuEF.EF_KTDL_QTLS";
                default: return "";
            }
        }


        /// <summary>
        /// Danh sách mã loại giao dịch
        /// </summary>
        public enum LoaiGiaoDich
        {
            MS01,   //HDVO --> MS01 --> Mở sổ tiết kiệm quy định
            MS02,   //HDVO --> MS02 --> Mở sổ tiết kiệm không kỳ hạn
            MS03,   //HDVO --> MS03 --> Mở sổ tiết kiệm có kỳ hạn
            MS04,   //HDVO --> MS04 --> Mở sổ tiền gửi có kỳ hạn
            MS05,   //HDVO --> MS05 --> Mở tài khoản thanh toán
            GT01,   //HDVO --> GT01 --> Gửi thêm tiền theo từng sổ
            GT02,   //HDVO --> GT02 --> Gửi thêm tiền theo danh sách
            GT03,   //HDVO --> GT03 --> Gửi thêm tiền theo danh sách Excel
            RG01,   //HDVO --> RG01 --> Rút gốc một phần
            RG02,   //HDVO --> RG02 --> Rút gốc theo danh sách
            TT01,   //HDVO --> TT01 --> Tất toán sổ tiền gửi
            TT02,   //HDVO --> TT02 --> Tất toán sổ tiền gửi theo danh sách
            PK01,   //HDVO --> PK01 --> Phong tỏa tài khoản
            GK01,   //HDVO --> GK01 --> Giải tỏa tài khoản
            TL01,   //HDVO --> TL01 --> Trả lãi tiền gửi
            TL02,   //HDVO --> TL02 --> Trả lãi tiền gửi theo danh sách
            DC01,   //HDVO --> DC01 --> Dự chi
            PC01,   //HDVO --> PC01 --> Phân bổ chi phí
            LG01,   //HDVO --> LG01 --> Lãi nhập gốc theo từng sổ
            LG02,   //HDVO --> LG02 --> Lãi nhập gốc theo danh sách
            DK01,   //HDVO --> DK01 --> Đóng tài khoản
            MK01,   //HDVO --> MK01 --> Mở tài khoản
            GN01,   //TDVM --> GN01 --> Giải ngân
            TU01,   //TDVM --> TU01 --> Tạm ứng giải ngân
            HU01,   //TDVM --> HU01 --> Hoàn ứng giải ngân
            DT01,   //TDVM --> DT01 --> Dự thu
            DP01,   //TDVM --> DP01 --> Trích lập dự phòng
            GH01,   //TDVM --> GH01 --> Gia hạn nợ
            CH01,   //TDVM --> CH01 --> Chuyển hoàn nhóm nợ
            CN01,   //TDVM --> CN01 --> Chuyển nợ quá hạn
            XL01,   //TDVM --> XL01 --> Xử lý nợ
            HD01,   //TDVM --> HD01 --> Hóa đơn thu tiền kỳ
            TH01,   //TDVM --> TH01 --> Thu gốc lãi trước hạn
            PL01,   //TDVM --> PL01 --> Phân bổ lãi vay
            PN01,   //TDVM --> PN01 --> Phân loại nợ
            TS01,   //TSDB --> TS01 --> Nhập tài sản
            TS02,   //TSDB --> TS02 --> Xuất tài sản
            KT01,   //GDKT --> KT01 --> Phiếu thu
            KT02,   //GDKT --> KT02 --> Phiếu chi
            KT03,   //GDKT --> KT03 --> Nhập xuất ngoại bảng
            KT04,   //GDKT --> KT04 --> Phiếu kế toán
            KT05,   //GDKT --> KT05 --> Bút toán điều chỉnh
            KT06,   //GDKT --> KT06 --> Kết chuyển
            KT07,   //GDKT --> KT07 --> Uy nhiem chi
            KT0033, //GDKT --> KT0033 --> Đánh giá ngoại tệ
            BH01,   //BHTH --> BH01 --> Thu hộ phí xác lập thành viên
            BH02,   //BHTH --> BH02 --> Chi hộ phí xác lập thành viên
            BH03,   //BHTH --> BH03 --> Thu hộ phí bảo hiểm
            BH04,   //BHTH --> BH04 --> Chi hộ phí bảo hiểm
            NTS01,   //QLTS --> NTS01 --> Nhập tài sản cố định sử dụng
            NTSDD01,   //QLTS --> NTSDD01 --> Nhập tài sản dở dang
            BGTS01,   //QLTS --> BGTS01 --> Bàn giao tài sản chính thức đưa vào sử dụng
            KH01,   //QLTS --> KH01 --> Khấu hao
            NCSC01,   //QLTS --> NCSC01 --> Nâng cấp sửa chữa lớn
            DGL01,   //QLTS --> DGL01 --> Đánh giá lại
            GTS01,   //QLTS --> GTS01 --> Giảm tài sản do thanh lý
            GTS02,   //QLTS --> GTS02 --> Giảm tài sản do mất
            NS01,   //NSTL --> NS01 --> Tính lương nhân viên
            NS02,   //NSTL --> NS02 --> Tính phụ cấp cộng tác viên
            NXTS01, //TSDB --> NXTS01 --> Nhap xuat tai san dam bao 

            GN03,   //TDTD --> GN03 --> Giải ngân
            TU03,   //TDTD --> TU03 --> Tạm ứng giải ngân
            HU03,   //TDTD --> HU03 --> Hoàn ứng giải ngân
            DT03,   //TDTD --> DT03 --> Dự thu
            DP03,   //TDTD --> DP03 --> Trích lập dự phòng
            GH03,   //TDTD --> GH03 --> Gia hạn nợ
            CH03,   //TDTD --> CH03 --> Chuyển hoàn nhóm nợ
            CN03,   //TDTD --> CN03 --> Chuyển nợ quá hạn
            XL03,   //TDTD --> XL03 --> Xử lý nợ
            HD03,   //TDTD --> HD03 --> Hóa đơn thu tiền kỳ
            TH03,   //TDTD --> TH03 --> Thu gốc lãi trước hạn
            PL03,   //TDTD --> PL03 --> Phân bổ lãi vay
            PN03,   //TDTD --> PN03 --> Phân loại nợ
            GNDL03,   //TDTD --> GNDL03 --> Phân loại nợ
            NNB03,  //TDTD --> NNB03 --> Nhập ngoại bảng
            XNB03  //TDTD --> XNB03 --> Xuất ngoại bảng
        }

        public static string layGiaTri(this LoaiGiaoDich loaiGiaoDich)
        {
            switch (loaiGiaoDich)
            {
                case LoaiGiaoDich.MS01: return "MS01"; //HDVO --> MS01 --> Mở sổ tiết kiệm quy định
                case LoaiGiaoDich.MS02: return "MS02"; //HDVO --> MS02 --> Mở sổ tiết kiệm không kỳ hạn
                case LoaiGiaoDich.MS03: return "MS03"; //HDVO --> MS03 --> Mở sổ tiết kiệm có kỳ hạn
                case LoaiGiaoDich.MS04: return "MS04"; //HDVO --> MS04 --> Mở sổ tiền gửi có kỳ hạn
                case LoaiGiaoDich.MS05: return "MS05"; //HDVO --> MS05 --> Mở tài khoản thanh toán
                case LoaiGiaoDich.GT01: return "GT01"; //HDVO --> GT01 --> Gửi thêm tiền theo từng sổ
                case LoaiGiaoDich.GT02: return "GT02"; //HDVO --> GT02 --> Gửi thêm tiền theo danh sách
                case LoaiGiaoDich.GT03: return "GT03"; //HDVO --> GT03 --> Gửi thêm tiền theo danh sách Excel
                case LoaiGiaoDich.RG01: return "RG01"; //HDVO --> RG01 --> Rút gốc một phần
                case LoaiGiaoDich.RG02: return "RG02"; //HDVO --> RG02 --> Rút gốc theo danh sách
                case LoaiGiaoDich.TT01: return "TT01"; //HDVO --> TT01 --> Tất toán sổ tiền gửi
                case LoaiGiaoDich.TT02: return "TT02"; //HDVO --> TT02 --> Tất toán sổ tiền gửi theo danh sách
                case LoaiGiaoDich.PK01: return "PK01"; //HDVO --> PK01 --> Phong tỏa tài khoản
                case LoaiGiaoDich.GK01: return "GK01"; //HDVO --> GK01 --> Giải tỏa tài khoản
                case LoaiGiaoDich.TL01: return "TL01"; //HDVO --> TL01 --> Trả lãi tiền gửi
                case LoaiGiaoDich.TL02: return "TL02"; //HDVO --> TL02 --> Trả lãi tiền gửi theo danh sách
                case LoaiGiaoDich.DC01: return "DC01"; //HDVO --> DC01 --> Dự chi
                case LoaiGiaoDich.PC01: return "PC01"; //HDVO --> PC01 --> Phân bổ chi phí
                case LoaiGiaoDich.LG01: return "LG01"; //HDVO --> LG01 --> Lãi nhập gốc theo từng sổ
                case LoaiGiaoDich.LG02: return "LG02"; //HDVO --> LG02 --> Lãi nhập gốc theo danh sách
                case LoaiGiaoDich.DK01: return "DK01"; //HDVO --> DK01 --> Đóng tài khoản
                case LoaiGiaoDich.MK01: return "MK01"; //HDVO --> MK01 --> Mở tài khoản
                case LoaiGiaoDich.GN01: return "GN01"; //TDVM --> GN01 --> Giải ngân
                case LoaiGiaoDich.TU01: return "TU01"; //TDVM --> TU01 --> Tạm ứng giải ngân
                case LoaiGiaoDich.HU01: return "HU01"; //TDVM --> HU01 --> Hoàn ứng giải ngân
                case LoaiGiaoDich.DT01: return "DT01"; //TDVM --> DT01 --> Dự thu
                case LoaiGiaoDich.DP01: return "DP01"; //TDVM --> DP01 --> Trích lập dự phòng
                case LoaiGiaoDich.GH01: return "GH01"; //TDVM --> GH01 --> Gia hạn nợ
                case LoaiGiaoDich.CH01: return "CH01"; //TDVM --> CH01 --> Chuyển hoàn nhóm nợ
                case LoaiGiaoDich.CN01: return "CN01"; //TDVM --> CN01 --> Chuyển nợ quá hạn
                case LoaiGiaoDich.XL01: return "XL01"; //TDVM --> XL01 --> Xử lý nợ
                case LoaiGiaoDich.HD01: return "HD01"; //TDVM --> HD01 --> Hóa đơn thu tiền kỳ
                case LoaiGiaoDich.TH01: return "TH01"; //TDVM --> TH01 --> Thu gốc lãi trước hạn
                case LoaiGiaoDich.PL01: return "PL01"; //TDVM --> PL01 --> Phân bổ lãi vay
                case LoaiGiaoDich.PN01: return "PN01"; //TDVM --> PN01 --> Phân loại nợ
                case LoaiGiaoDich.TS01: return "TS01"; //TSDB --> TS01 --> Nhập tài sản
                case LoaiGiaoDich.TS02: return "TS02"; //TSDB --> TS02 --> Xuất tài sản
                case LoaiGiaoDich.KT01: return "KT01"; //GDKT --> KT01 --> Phiếu thu
                case LoaiGiaoDich.KT02: return "KT02"; //GDKT --> KT02 --> Phiếu chi
                case LoaiGiaoDich.KT03: return "KT03"; //GDKT --> KT03 --> Nhập xuất ngoại bảng
                case LoaiGiaoDich.KT04: return "KT04"; //GDKT --> KT04 --> Phiếu kế toán
                case LoaiGiaoDich.KT05: return "KT05"; //GDKT --> KT05 --> Bút toán điều chỉnh
                case LoaiGiaoDich.KT06: return "KT06"; //GDKT --> KT06 --> Kết chuyển
                case LoaiGiaoDich.KT07: return "KT07"; //GDKT --> KT07 --> Uy nhiem chi
                case LoaiGiaoDich.BH01: return "BH01"; //BHTH --> BH01 --> Thu hộ phí xác lập thành viên
                case LoaiGiaoDich.BH02: return "BH02"; //BHTH --> BH02 --> Chi hộ phí xác lập thành viên
                case LoaiGiaoDich.BH03: return "BH03"; //BHTH --> BH03 --> Thu hộ phí bảo hiểm
                case LoaiGiaoDich.BH04: return "BH04"; //BHTH --> BH04 --> Chi hộ phí bảo hiểm
                case LoaiGiaoDich.NTS01: return "NTS01"; //QLTS -->NTS01--Nhập tài sản cố định sử dụng
                case LoaiGiaoDich.NTSDD01: return "NTSDD01"; //QLTS -->NTSDD01--Nhập tài sản dở dang
                case LoaiGiaoDich.BGTS01: return "BGTS01"; //QLTS -->BGTS01--Bàn giao tài sản chính thức đưa vào sử dụng
                case LoaiGiaoDich.KH01: return "KH01"; //QLTS -->KH01--Khấu hao
                case LoaiGiaoDich.NCSC01: return "NCSC01"; //QLTS -->NCSC01--Nâng cấp sửa chữa lớn
                case LoaiGiaoDich.DGL01: return "DGL01"; //QLTS -->DGL01--Đánh giá lại
                case LoaiGiaoDich.GTS01: return "GTS01"; //QLTS -->GTS01--Giảm tài sản do thanh lý
                case LoaiGiaoDich.GTS02: return "GTS02"; //QLTS -->GTS02--Giảm tài sản do mất
                case LoaiGiaoDich.NS01: return "NS01"; //NSTL -->NS01--Tính lương nhân viên
                case LoaiGiaoDich.NS02: return "NS02"; //NSTL -->NS02--Tính phụ cấp cộng tác viên
                case LoaiGiaoDich.NXTS01: return "NXTS01"; //TSDB -->NXTS01--Nhap xuat ngoai bang TSDB
                case LoaiGiaoDich.GN03: return "GN03"; //TDVM --> GN01 --> Giải ngân
                case LoaiGiaoDich.TU03: return "TU03"; //TDVM --> TU01 --> Tạm ứng giải ngân
                case LoaiGiaoDich.HU03: return "HU03"; //TDVM --> HU01 --> Hoàn ứng giải ngân
                case LoaiGiaoDich.DT03: return "DT03"; //TDVM --> DT01 --> Dự thu
                case LoaiGiaoDich.DP03: return "DP03"; //TDVM --> DP01 --> Trích lập dự phòng
                case LoaiGiaoDich.GH03: return "GH03"; //TDVM --> GH01 --> Gia hạn nợ
                case LoaiGiaoDich.CH03: return "CH03"; //TDVM --> CH01 --> Chuyển hoàn nhóm nợ
                case LoaiGiaoDich.CN03: return "CN03"; //TDVM --> CN01 --> Chuyển nợ quá hạn
                case LoaiGiaoDich.XL03: return "XL03"; //TDVM --> XL01 --> Xử lý nợ
                case LoaiGiaoDich.HD03: return "HD03"; //TDVM --> HD01 --> Hóa đơn thu tiền kỳ
                case LoaiGiaoDich.TH03: return "TH03"; //TDVM --> TH01 --> Thu gốc lãi trước hạn
                case LoaiGiaoDich.PL03: return "PL03"; //TDVM --> PL01 --> Phân bổ lãi vay
                case LoaiGiaoDich.PN03: return "PN03"; //TDVM --> PN01 --> Phân loại nợ
                case LoaiGiaoDich.KT0033: return "KT0033"; //GDKT --> KT0033 --> Đánh giá ngoại tệ
                case LoaiGiaoDich.GNDL03: return "GNDL03"; //GDKT --> GNDL03 --> Đánh giá ngoại tệ
                case LoaiGiaoDich.NNB03: return "NNB03"; //TDTD --> NNB03 --> Nhập ngoại bảng
                case LoaiGiaoDich.XNB03: return "XNB03"; //TDTD --> XNB03 --> Xuất ngoại bảng
                default: return "";
            }
        }

        public static LoaiGiaoDich layLoaiGiaoDich(string maLoaiGiaoDich)
        {
            switch (maLoaiGiaoDich)
            {
                case "MS01": return LoaiGiaoDich.MS01;    //HDVO --> MS01 --> Mở sổ tiết kiệm quy định
                case "MS02": return LoaiGiaoDich.MS02;    //HDVO --> MS02 --> Mở sổ tiết kiệm không kỳ hạn
                case "MS03": return LoaiGiaoDich.MS03;    //HDVO --> MS03 --> Mở sổ tiết kiệm có kỳ hạn
                case "MS04": return LoaiGiaoDich.MS04;    //HDVO --> MS04 --> Mở sổ tiền gửi có kỳ hạn
                case "MS05": return LoaiGiaoDich.MS05;    //HDVO --> MS05 --> Mở tài khoản thanh toán
                case "GT01": return LoaiGiaoDich.GT01;    //HDVO --> GT01 --> Gửi thêm tiền theo từng sổ
                case "GT02": return LoaiGiaoDich.GT02;    //HDVO --> GT02 --> Gửi thêm tiền theo danh sách
                case "GT03": return LoaiGiaoDich.GT03;    //HDVO --> GT03 --> Gửi thêm tiền theo danh sách Excel
                case "RG01": return LoaiGiaoDich.RG01;    //HDVO --> RG01 --> Rút gốc một phần
                case "RG02": return LoaiGiaoDich.RG02;    //HDVO --> RG02 --> Rút gốc theo danh sách
                case "TT01": return LoaiGiaoDich.TT01;    //HDVO --> TT01 --> Tất toán sổ tiền gửi
                case "TT02": return LoaiGiaoDich.TT02;    //HDVO --> TT02 --> Tất toán sổ tiền gửi theo danh sách
                case "PK01": return LoaiGiaoDich.PK01;    //HDVO --> PK01 --> Phong tỏa tài khoản
                case "GK01": return LoaiGiaoDich.GK01;    //HDVO --> GK01 --> Giải tỏa tài khoản
                case "TL01": return LoaiGiaoDich.TL01;    //HDVO --> TL01 --> Trả lãi tiền gửi
                case "TL02": return LoaiGiaoDich.TL02;    //HDVO --> TL02 --> Trả lãi tiền gửi theo danh sách
                case "DC01": return LoaiGiaoDich.DC01;    //HDVO --> DC01 --> Dự chi
                case "PC01": return LoaiGiaoDich.PC01;    //HDVO --> PC01 --> Phân bổ chi phí
                case "LG01": return LoaiGiaoDich.LG01;    //HDVO --> LG01 --> Lãi nhập gốc theo từng sổ
                case "LG02": return LoaiGiaoDich.LG02;    //HDVO --> LG02 --> Lãi nhập gốc theo danh sách
                case "DK01": return LoaiGiaoDich.DK01;    //HDVO --> DK01 --> Đóng tài khoản
                case "MK01": return LoaiGiaoDich.MK01;    //HDVO --> MK01 --> Mở tài khoản
                case "GN01": return LoaiGiaoDich.GN01;    //TDVM --> GN01 --> Giải ngân
                case "TU01": return LoaiGiaoDich.TU01;    //TDVM --> TU01 --> Tạm ứng giải ngân
                case "HU01": return LoaiGiaoDich.HU01;    //TDVM --> HU01 --> Hoàn ứng giải ngân
                case "DT01": return LoaiGiaoDich.DT01;    //TDVM --> DT01 --> Dự thu
                case "DP01": return LoaiGiaoDich.DP01;    //TDVM --> DP01 --> Trích lập dự phòng
                case "GH01": return LoaiGiaoDich.GH01;    //TDVM --> GH01 --> Gia hạn nợ
                case "CH01": return LoaiGiaoDich.CH01;    //TDVM --> CH01 --> Chuyển hoàn nhóm nợ
                case "CN01": return LoaiGiaoDich.CN01;    //TDVM --> CN01 --> Chuyển nợ quá hạn
                case "XL01": return LoaiGiaoDich.XL01;    //TDVM --> XL01 --> Xử lý nợ
                case "HD01": return LoaiGiaoDich.HD01;    //TDVM --> HD01 --> Hóa đơn thu tiền kỳ
                case "TH01": return LoaiGiaoDich.TH01;    //TDVM --> TH01 --> Thu gốc lãi trước hạn
                case "PL01": return LoaiGiaoDich.PL01;    //TDVM --> PL01 --> Phân bổ lãi vay
                case "PN01": return LoaiGiaoDich.PN01;    //TDVM --> PL01 --> Phân loại nợ
                case "TS01": return LoaiGiaoDich.TS01;    //TSDB --> TS01 --> Nhập tài sản
                case "TS02": return LoaiGiaoDich.TS02;    //TSDB --> TS02 --> Xuất tài sản
                case "KT01": return LoaiGiaoDich.KT01;    //GDKT --> KT01 --> Phiếu thu
                case "KT02": return LoaiGiaoDich.KT02;    //GDKT --> KT02 --> Phiếu chi
                case "KT03": return LoaiGiaoDich.KT03;    //GDKT --> KT03 --> Nhập xuất ngoại bảng
                case "KT04": return LoaiGiaoDich.KT04;    //GDKT --> KT04 --> Phiếu kế toán
                case "KT05": return LoaiGiaoDich.KT05;    //GDKT --> KT05 --> Bút toán điều chỉnh
                case "KT06": return LoaiGiaoDich.KT06;    //GDKT --> KT06 --> Kết chuyển
                case "KT07": return LoaiGiaoDich.KT07;    //GDKT --> KT07 --> Uy nhiem chi
                case "BH01": return LoaiGiaoDich.BH01;    //BHTH --> BH01 --> Thu hộ phí xác lập thành viên
                case "BH02": return LoaiGiaoDich.BH02;    //BHTH --> BH02 --> Chi hộ phí xác lập thành viên
                case "BH03": return LoaiGiaoDich.BH03;    //BHTH --> BH03 --> Thu hộ phí bảo hiểm
                case "BH04": return LoaiGiaoDich.BH04;    //BHTH --> BH04 --> Chi hộ phí bảo hiểm
                case "NTS01": return LoaiGiaoDich.NTS01; //QLTS -->NTS01--Nhập tài sản cố định sử dụng
                case "NTSDD01": return LoaiGiaoDich.NTSDD01; //QLTS -->NTSDD01--Nhập tài sản dở dang
                case "BGTS01": return LoaiGiaoDich.BGTS01; //QLTS -->BGTS01--Bàn giao tài sản chính thức đưa vào sử dụng
                case "KH01": return LoaiGiaoDich.KH01; //QLTS -->KH01--Khấu hao
                case "NCSC01": return LoaiGiaoDich.NCSC01; //QLTS -->NCSC01--Nâng cấp sửa chữa lớn
                case "DGL01": return LoaiGiaoDich.DGL01; //QLTS -->DGL01--Đánh giá lại
                case "GTS01": return LoaiGiaoDich.GTS01; //QLTS -->GTS01--Giảm tài sản do thanh lý
                case "GTS02": return LoaiGiaoDich.GTS02; //QLTS -->GTS02--Giảm tài sản do mất
                case "NS01": return LoaiGiaoDich.NS01; //NSTL -->NS01--Tính lương nhân viên
                case "NS02": return LoaiGiaoDich.NS02; //NSTL -->NS02--Tính phụ cấp cộng tác viên
                case "NXTS01": return LoaiGiaoDich.NXTS01; //TSDB -->NXTS01--Nhap xuat ngoai bang
                case "GN03": return LoaiGiaoDich.GN03;    //TDVM --> GN01 --> Giải ngân
                case "TU03": return LoaiGiaoDich.TU03;    //TDVM --> TU01 --> Tạm ứng giải ngân
                case "HU03": return LoaiGiaoDich.HU03;    //TDVM --> HU01 --> Hoàn ứng giải ngân
                case "DT03": return LoaiGiaoDich.DT03;    //TDVM --> DT01 --> Dự thu
                case "DP03": return LoaiGiaoDich.DP03;    //TDVM --> DP01 --> Trích lập dự phòng
                case "GH03": return LoaiGiaoDich.GH03;    //TDVM --> GH01 --> Gia hạn nợ
                case "CH03": return LoaiGiaoDich.CH03;    //TDVM --> CH01 --> Chuyển hoàn nhóm nợ
                case "CN03": return LoaiGiaoDich.CN03;    //TDVM --> CN01 --> Chuyển nợ quá hạn
                case "XL03": return LoaiGiaoDich.XL03;    //TDVM --> XL01 --> Xử lý nợ
                case "HD03": return LoaiGiaoDich.HD03;    //TDVM --> HD01 --> Hóa đơn thu tiền kỳ
                case "TH03": return LoaiGiaoDich.TH03;    //TDVM --> TH01 --> Thu gốc lãi trước hạn
                case "PL03": return LoaiGiaoDich.PL03;    //TDVM --> PL01 --> Phân bổ lãi vay
                case "PN03": return LoaiGiaoDich.PN03;    //TDVM --> PL01 --> Phân loại nợ
                case "KT0033": return LoaiGiaoDich.KT0033; //GDKT --> KT0033 --> Đánh giá ngoại tệ
                case "GNDL03": return LoaiGiaoDich.GNDL03; //GDKT --> KT0033 --> Đánh giá ngoại tệ
                case "NNB03": return LoaiGiaoDich.NNB03; //TDTD --> NNB03 --> Nhập ngoại bảng
                case "XNB03": return LoaiGiaoDich.XNB03; //TDTD --> XNB03 --> Xuất ngoại bảng
                default: return LoaiGiaoDich.MS01;
            }
        }

        public enum Function
        {
            BAO_CAO,

            // Hệ thống
            HT_LOGIN,
            HT_LOGOUT,
            HT_RESET_PASS,
            HT_NHNSD,
            HT_NSD,
            HT_NHNSD_NSD,
            HT_NSD_HIEN_THOI,
            HT_PQ_CHUC_NANG,
            HT_PQ_PHAM_VI,
            HT_THAM_SO,
            HT_SAO_LUU,
            HT_PHUC_HOI,
            HT_KIEM_TRA_PB,
            HT_CAP_NHAT_PB,
            HT_DANG_NHAP,
            // Dùng chung, danh mục
            DC_DM_TINH_THANH,
            DC_DM_DIA_BAN,
            DC_DM_DON_VI,
            DC_DM_KHU_VUC,
            DC_DM_KHU_VUC_DS,
            DC_DM_CUM,
            DC_DM_CUM_DS,
            DC_DM_NHOM,
            DC_DM_NHOM_DS,
            DC_DM_DUNG_CHUNG,
            DC_DM_PHAN_HE_GD,
            DC_DM_QUOC_GIA,
            DC_DM_TIEN_TE,
            DC_DM_LICH_HOP,
            DC_LAI_SUAT_DS,
            DC_LAI_SUAT_CT,
            DC_PHI,
            DC_LOAI_TY_GIA,
            DC_TY_GIA,
            DC_HAN_MUC,
            DC_HAN_MUC_CTIET,
            DC_TSUAT,
            DC_DM_DTUONG,
            DC_DM_LOAI_DTUONG,
            DC_DM_DTUONG_SODU,
            DC_DM_TCTD,
            DC_DM_TCTD_DS,
            // Khách hàng
            KH_NHOM,
            KH_THANH_VIEN,
            KH_CA_NHAN,
            KH_TO_CHUC,
            KH_DANH_SACH,
            KH_CHUYEN_DIA_BAN,
            KH_CAP_NHAT_KH,
            KH_XEP_HANG_TD,
            KH_XEP_HANG_NGHEO,
            KH_QL_HINH_HANH,
            KH_THONG_TIN_KHAO_SAT_CT,
            KH_THONG_TIN_KHAO_SAT_DS,
            // Kế toán
            KT_PHAN_LOAI_DS,
            KT_PHAN_LOAI_CT,
            KT_BUT_TOAN_DS,
            KT_BUT_TOAN_CT,
            KT_CAN_DOI_DS,
            KT_CAN_DOI_CT,
            KT_TAI_KHOAN_DS,
            KT_TAI_KHOAN_CT,
            KT_TONG_HOP_DS,
            KT_TONG_HOP_CT,
            KT_PHIEU_THU,
            KT_PHIEU_CHI,
            KT_PHIEU_KE_TOAN,
            KT_NGOAI_BANG,
            KT_DIEU_CHINH,
            KT_KET_CHUYEN,
            KT_GIAO_DICH,
            KT_CUOI_NGAY,
            KT_PHIEU_UY_NHIEM_CHI,
            KT_GIAO_DICH_DOI_TUONG,
            KT_CHUNG_TU_GHI_SO,
            KT_LAY_TONG_SO_DU,
            KT_DANH_GIA_NGOAI_TE,
            KT_HE_THONG_TKTH,
            KT_TAI_KHOAN_TH,
            
            // Huy động vốn
            HDV_DANH_SACH_SP,
            HDV_SAN_PHAM,
            HDV_DANH_SACH_SO,
            HDV_SO_TKQD,
            HDV_SO_TKKKH,
            HDV_SO_TGCKH,
            HDV_SO_TK_TGTT,
            HDV_SO_TKCKH,
            HDV_GUI_THEM_TIEN_THEO_SO,
            HDV_GUI_THEM_TIEN_THEO_DANH_SACH,
            HDV_GUI_THEM_TIEN_THEO_EXCEL,
            HDV_RUT_BOT_GOC,
            HDV_RUT_GOC_THEO_DANH_SACH,
            HDV_TAT_TOAN,
            HDV_TAT_TOAN_THEO_DANH_SACH,
            HDV_PHONG_TOA_SD,
            HDV_DANH_SACH_PHONG_TOA_SD,
            HDV_GIAI_TOA_SD,
            HDV_TRA_LAI,
            HDV_TRA_LAI_THEO_DANH_SACH,
            HDV_DU_CHI,
            HDV_DAT_LICH_DU_CHI,
            HDV_LAI_NHAP_GOC_THEO_SO,
            HDV_LAI_NHAP_GOC_THEO_DANH_SACH,
            HDV_PHAN_BO,
            HDV_DAT_LICH_PHAN_BO,
            HDV_DAT_LICH_LAI_NHAP_GOC,
            HDV_DONG_TK,
            HDV_DANH_SACH_DONG_TK,
            HDV_MO_LAI_TK,
            HDV_DIEU_CHINH_LS,
            HDV_DANH_SACH_DIEU_CHINH_LS,
            HDV_DANG_KY_RUT_GOC,
            HDV_DANG_KY_RUT_GOC_CT,
            HDV_DANG_KY_RUT_GOC_DS,

            //Tín dụng
            TD_CHI_TIET_LOAI_TAI_SAN,
            TD_DANH_SACH_LOAI_TAI_SAN,
            TD_CHI_TIET_TAI_SAN_DAM_BAO,
            TD_DANH_SACH_TAI_SAN_DAM_BAO,
            TD_CHI_TIET_HOP_DONG_THE_CHAP,
            TD_DANH_SACH_HOP_DONG_THE_CHAP,
            // Tín dụng vi mô            
            TDVM_SAN_PHAM_TDVM,
            TDVM_VONG_VAY,
            TDVM_HAN_MUC,
            TDVM_TSDB,
            TDVM_THE_CHAP,
            TDVM_DANH_SACH_HOP_DONG,
            TDVM_CHI_TIET_HOP_DONG,
            TDVM_DANH_SACH_KHE_UOC,
            TDVM_DANH_SACH_KHE_UOC_01,
            TDVM_CHI_TIET_KHE_UOC,
            TDVM_THOA_THUAN,
            TDVM_HOP_DONG_NHOM,
            TDVM_DAT_LICH_TRA_NO,
            TDVM_TAM_UNG,
            TDVM_HOAN_UNG,
            TDVM_GIAI_NGAN,
            TDVM_DU_THU,
            TDVM_DAT_LICH_DU_THU,
            TDVM_TRICH_LAP_DU_PHONG,
            TDVM_DAT_LICH_TRICH_LAP_DU_PHONG,
            TDVM_DIEU_CHINH_LAI_SUAT,
            TDVM_GIA_HAN_NO,
            TDVM_CHUYEN_HOAN_NHOM_NO,
            TDVM_CHUYEN_NO_QUA_HAN,
            TDVM_XU_LY_NO,
            TDVM_LAP_HOA_DON_TIEN_KY,
            TDVM_PHAN_BO_LAI_VAY,
            TDVM_DAT_LICH_PHAN_BO_LAI_VAY,
            TDVM_THU_GOC_LAI_TRUOC_HAN,
            TDVM_DAT_LICH_THU_PHAT_VON,
            TDVM_LAY_KH_THEO_KUOC,
            TDVM_DON_XIN_VAY_VON,
            TDVM_DIA_BAN_SAN_PHAM,
            TDVM_PHAN_LOAI_NO,
            TDVM_LAP_LICH_THU_GOC_LAI,
            TDVM_KIEM_SOAT_RR,
            // Bảo hiểm
            BH_SAN_PHAM_BHTH,
            BH_CAM_KET_THEO_KH,
            BH_CAM_KET_THEO_DS,
            BH_THU_HO_PHIXL,
            BH_CHI_HO_PHIXL,
            BH_THU_HO_PHIBH,
            BH_CHI_HO_PHIBH,
            BH_CHAM_DUT,
            // Nhân sự
            NS_DM_QUOC_TICH_CT,
            NS_DM_TINH_TP_CT,
            NS_DM_QUAN_HUYEN_CT,
            NS_DM_PHUONG_XA_CT,
            NS_DM_DAN_TOC_CT,
            NS_DM_TON_GIAO_CT,
            NS_DM_GIOI_TINH_CT,
            NS_DM_TTRANG_HNHAN_CT,
            NS_DM_CU_TRU_CT,
            NS_DM_HTHUC_LVIEC_CT,
            NS_DM_TDO_HVAN_CT,
            NS_DM_HTHUC_DTAO_CT,
            NS_DM_TRUONG_DTAO_CT,
            NS_DM_BANG_CAP_CT,
            NS_DM_KHOA_DTAO_CT,
            NS_DM_CNGANH_DTAO_CT,
            NS_DM_HOC_HAM_CT,
            NS_DM_HOC_VI_CT,
            NS_DM_TDO_TANH_CT,
            NS_DM_TDO_THOC_CT,
            NS_DM_TDO_CTRI_CT,
            NS_DM_XEP_LOAI_CT,
            NS_DM_QHE_GDINH_CT,
            NS_DM_KY_NANG_CT,
            NS_DM_NGHE_NGHIEP_CT,
            NS_DM_CHUC_VU_CT,
            NS_DM_LDO_NPHEP_CT,
            NS_DM_LDO_TVIEC_CT,
            NS_DM_THAN_HDLD_CT,
            NS_DM_LOAI_HDLD_CT,
            NS_DM_HTHUC_TLUONG_CT,
            NS_DM_LOAI_TNHAP_CT,
            NS_DM_LOAI_CPHI_CT,
            NS_DM_HTHUC_KTHUONG_CT,
            NS_DM_HTHUC_KLUAT_CT,
            NS_DM_KHIEU_CCONG_CT,
            NS_DM_LOAI_HSO_CT,
            NS_DM_DVI_TGIAN_CT,
            NS_DM_DVI_CTAC_CT,
            NS_DM_LOAI_GTO_CT,
            NS_DM_BVIEN_KCB_CT,
            NS_DM_NHOM_CTV_CT,
            NS_DM_DU_AN_CT,
            NS_DM_CHUC_VU_DU_AN_CT,
            NS_DM_CHUC_VU_CTV_CT,
            NS_DM_PHU_CAP_CT,
            NS_DM_DANH_MUC_CT,
            NS_DM_DANH_MUC_PL,

            NS_DM_QUOC_TICH_DS,
            NS_DM_TINH_TP_DS,
            NS_DM_QUAN_HUYEN_DS,
            NS_DM_PHUONG_XA_DS,
            NS_DM_DAN_TOC_DS,
            NS_DM_TON_GIAO_DS,
            NS_DM_GIOI_TINH_DS,
            NS_DM_TTRANG_HNHAN_DS,
            NS_DM_CU_TRU_DS,
            NS_DM_HTHUC_LVIEC_DS,
            NS_DM_TDO_HVAN_DS,
            NS_DM_HTHUC_DTAO_DS,
            NS_DM_TRUONG_DTAO_DS,
            NS_DM_BANG_CAP_DS,
            NS_DM_KHOA_DTAO_DS,
            NS_DM_CNGANH_DTAO_DS,
            NS_DM_HOC_HAM_DS,
            NS_DM_HOC_VI_DS,
            NS_DM_TDO_TANH_DS,
            NS_DM_TDO_THOC_DS,
            NS_DM_TDO_CTRI_DS,
            NS_DM_XEP_LOAI_DS,
            NS_DM_QHE_GDINH_DS,
            NS_DM_KY_NANG_DS,
            NS_DM_NGHE_NGHIEP_DS,
            NS_DM_CHUC_VU_DS,
            NS_DM_LDO_NPHEP_DS,
            NS_DM_LDO_TVIEC_DS,
            NS_DM_THAN_HDLD_DS,
            NS_DM_LOAI_HDLD_DS,
            NS_DM_HTHUC_TLUONG_DS,
            NS_DM_LOAI_TNHAP_DS,
            NS_DM_LOAI_CPHI_DS,
            NS_DM_HTHUC_KTHUONG_DS,
            NS_DM_HTHUC_KLUAT_DS,
            NS_DM_KHIEU_CCONG_DS,
            NS_DM_LOAI_HSO_DS,
            NS_DM_DVI_TGIAN_DS,
            NS_DM_DVI_CTAC_DS,
            NS_DM_LOAI_GTO_DS,
            NS_DM_BVIEN_KCB_DS,
            NS_DM_NHOM_CTV_DS,
            NS_DM_DU_AN_DS,
            NS_DM_CHUC_VU_DU_AN_DS,
            NS_DM_CHUC_VU_CTV_DS,
            NS_DM_PHU_CAP_DS,
            NS_DM_DANH_MUC_DS,

            NS_HO_SO_CT,
            NS_HO_SO_DS,
            NS_HOP_DONG_CT,
            NS_HOP_DONG_DS,
            NS_THUYEN_CHUYEN_CT,
            NS_THUYEN_CHUYEN_DS,
            NS_THOI_VIEC_CT,
            NS_THOI_VIEC_DS,
            NS_BANG_LUONG,
            NS_LUONG_CT,
            NS_LUONG_DS,
            NS_LUONG_DCHINH_CT,
            NS_LUONG_DCHINH_DS,
            NS_TINH_LUONG_CT,
            NS_TINH_LUONG_DS,
            NS_TIEU_CHI_PHU_CAP_CTV,
            NS_TINH_PHU_CAP_CTV_CT,
            NS_TINH_PHU_CAP_CTV_DS,
            NS_QLY_DU_AN,

            NS_LUONG,
            NS_CHUC_VU,
            NS_KHEN_THUONG,
            NS_KY_LUAT,
            NS_DAO_TAO,
            NS_NGHI_PHEP,
            NS_TIEU_CHI,
            NS_DANH_SACH,
            NS_PHU_CAP,
            // Khai thác dữ liệu
            KTDL_MAU_KHAI_THAC_DL,
            KTDL_PHAN_HE_CHUNG,
            KTDL_KHACH_HANG,
            KTDL_KE_TOAN,
            KTDL_HUY_DONG,
            KTDL_TIN_DUNG_VI_MO,
            KTDL_TIN_DUNG,
            KTDL_BAO_HIEM,
            KTDL_NHAN_SU,
            KTDL_SMS_KICH_HOAT,
            KTDL_SMS_DONG_BO,
            KTDL_SMS_CAU_HINH,
            KTDL_SMS_DANG_KY,
            KTDL_SMS_TRUY_VAN,
            KTDL_SMS_DUYET,
            KTDL_DM_DL_CT,
            KTDL_DM_CT,
            KTDL_DM_DS,
            KTDL_MAPPING_LOAITK_CT,
            KTDL_MAPPING_LOAITK_DS,
            KTDL_MAPPING_MATK_CT,
            KTDL_MAPPING_MATK_DS,
            KTDL_MAPPING_MAPPING,
            TS_DM_NHOM_TS_CT,
            TS_DM_NHOM_TS_GET_NHOM_CHA,
            TS_DM_NHOM_TS_DS,
            TS_TAI_SAN,
            TS_TANG,
            TS_TANG_DS,
            TS_TANG_NG,
            TS_BAN_GIAO,
            TS_BAN_GIAO_DS,
            TS_NANG_CAP,
            TS_NANG_CAP_DS,
            TS_DANH_GIA,
            TS_DANH_GIA_DS,
            TS_GIAM,
            TS_GIAM_DS,
            TS_KHAU_HAO,
            TS_KHAU_HAO_DS,
            // Tài sản đảm bảo
            TD_TSDB_LOAI_CT,
            TD_TSDB_LOAI_DS,
            TD_TSDB_CT,
            TD_TSDB_DS,
            TD_TSDB_LOAI,

            // Hạn mức
            HM_TONG,
            HM_CTIET_CT,
            HM_CTIET_DS,

            //Hợp đồng thế chấp
            TSDB_HOP_DONG_THE_CHAP,
            TSDB_HOP_DONG_THE_CHAP_NHAP,
            TSDB_HOP_DONG_THE_CHAP_XUAT,

            // Tín dụng thông thường
            TD_SAN_PHAMTT,
            TD_SAN_PHAMTT_DS,
            TD_HDTD,
            TD_HDTD_DS,
            TD_KUOC,
            TD_KUOC_DS,
            TD_GIAI_NGAN,

            // Tín dụng tiêu dùng
            TDTD_SAN_PHAM,
            TDTD_DON_XIN_VAY_VON,
            TDTD_HOP_DONG_CA_NHAN,
            TDTD_KHE_UOC,
            TDTD_GIAI_NGAN,
            TDTD_GIAI_NGAN_DAI_LY,
            TDTD_THU_GOC_LAI,
            TDTD_KIEM_SOAT_RR,
            TDTD_CHUYEN_HOAN,
            TDTD_CHUYEN_NO,
            TDTD_DU_THU,
            TDTD_TRICH_LAP_DP,

            SYS_JOB_EMAIL,
            SYS_JOB_SUBSCRIBE,
            SYS_JOB_HIS,

            //SMS
            SMS_DANG_KY_DVU,
            SMS_DANG_KY_LOAI_DTUONG,
            SMS_DANG_KY_DTUONG,
            SMS_DANG_KY_TNHAN_CDONG,
            SMS_DSACH_TNHAN_DEN,
            SMS_DSACH_TNHAN_DI,
            SMS_DANG_KY_SO_DIEN_THOAI,
            SMS_QUAN_LY_KET_NOI,
            //Default
            DEFAULT,

            //TSDB
            TDTD_TSDB_CT,
            TDTD_TSDB_DS,
            TDTD_HDTC_CT,
            TDTD_HDTC_DS
        }
        public static string getValue(this Function function)
        {
            switch (function)
            {
                case Function.BAO_CAO: return "BAO_CAO";
                //Hệ thống
                case Function.HT_LOGIN: return "HT_LOGIN";
                case Function.HT_LOGOUT: return "HT_LOGOUT";
                case Function.HT_RESET_PASS: return "HT_RESET_PASS";
                case Function.HT_NHNSD: return "HT_NHNSD";
                case Function.HT_NSD: return "HT_NSD";
                case Function.HT_NHNSD_NSD: return "HT_NHNSD_NSD";
                case Function.HT_NSD_HIEN_THOI: return "HT_NSD_HIEN_THOI";
                case Function.HT_PQ_CHUC_NANG: return "HT_PQ_CHUC_NANG";
                case Function.HT_PQ_PHAM_VI: return "HT_PQ_PHAM_VI";
                case Function.HT_THAM_SO: return "HT_THAM_SO";
                case Function.HT_SAO_LUU: return "HT_SAO_LUU";
                case Function.HT_PHUC_HOI: return "HT_PHUC_HOI";
                case Function.HT_KIEM_TRA_PB: return "HT_KIEM_TRA_PB";
                case Function.HT_CAP_NHAT_PB: return "HT_CAP_NHAT_PB";
                case Function.HT_DANG_NHAP: return "HT_DANG_NHAP";
                //Dùng chung
                case Function.DC_DM_TINH_THANH: return "DC_DM_TINH_THANH";
                case Function.DC_DM_DIA_BAN: return "DC_DM_DIA_BAN";
                case Function.DC_DM_DON_VI: return "DC_DM_DON_VI";
                case Function.DC_DM_KHU_VUC: return "DC_DM_KHU_VUC";
                case Function.DC_DM_KHU_VUC_DS: return "DC_DM_KHU_VUC_DS";
                case Function.DC_DM_CUM: return "DC_DM_CUM";
                case Function.DC_DM_CUM_DS: return "DC_DM_CUM_DS";
                case Function.DC_DM_NHOM: return "313_NHOM_KH";
                case Function.DC_DM_NHOM_DS: return "313_NHOM_KH";
                case Function.DC_DM_DUNG_CHUNG: return "DC_DM_DUNG_CHUNG";
                case Function.DC_DM_PHAN_HE_GD: return "DC_DM_PHAN_HE_GD";
                case Function.DC_DM_QUOC_GIA: return "DC_DM_QUOC_GIA";
                case Function.DC_DM_TIEN_TE: return "DC_DM_TIEN_TE";
                case Function.DC_DM_LICH_HOP: return "DC_DM_LICH_HOP";
                case Function.DC_LAI_SUAT_CT: return "DC_LAI_SUAT_CT";
                case Function.DC_LAI_SUAT_DS: return "DC_LAI_SUAT_DS";
                case Function.DC_PHI: return "231_PHI";
                case Function.DC_LOAI_TY_GIA: return "DC_LOAI_TY_GIA";
                case Function.DC_TY_GIA: return "DC_TY_GIA";
                case Function.DC_HAN_MUC: return "251_HAN_MUC";
                case Function.DC_HAN_MUC_CTIET: return "251_HAN_MUC";
                case Function.DC_TSUAT: return "DC_TSUAT";
                case Function.DC_DM_DTUONG: return "DC_DM_DTUONG";
                case Function.DC_DM_LOAI_DTUONG: return "DC_DM_LOAI_DTUONG";
                case Function.DC_DM_DTUONG_SODU: return "DC_DM_DTUONG_SODU";
                case Function.DC_DM_TCTD: return "2112_TO_CHUC_TIN_DUNG";
                case Function.DC_DM_TCTD_DS: return "2112_TO_CHUC_TIN_DUNG";
                //Khách hàng
                case Function.KH_NHOM: return "313_NHOM_KH";
                case Function.KH_THANH_VIEN: return "314_KHACH_HANG";
                case Function.KH_CA_NHAN: return "314_KHACH_HANG";
                case Function.KH_TO_CHUC: return "314_KHACH_HANG";
                case Function.KH_DANH_SACH: return "314_KHACH_HANG";
                case Function.KH_CHUYEN_DIA_BAN: return "315_CHUYEN_DIA_BAN";
                case Function.KH_CAP_NHAT_KH: return "316_CAP_NHAT_KH";
                case Function.KH_XEP_HANG_TD: return "331_XEP_HANG_TD";
                case Function.KH_XEP_HANG_NGHEO: return "332_XEP_HANG_NGHEO";
                case Function.KH_QL_HINH_HANH: return "317_QL_HINH_ANH";
                case Function.KH_THONG_TIN_KHAO_SAT_CT: return "TDTD_PHIEU_KS";
                case Function.KH_THONG_TIN_KHAO_SAT_DS: return "TDTD_PHIEU_KS";
                //Kế toán
                case Function.KT_PHAN_LOAI_CT: return "411_PHAN_LOAI";
                case Function.KT_PHAN_LOAI_DS: return "411_PHAN_LOAI";
                case Function.KT_BUT_TOAN_CT: return "415_BUT_TOAN";
                case Function.KT_BUT_TOAN_DS: return "415_BUT_TOAN";
                case Function.KT_CAN_DOI_CT: return "413_CAN_DOI";
                case Function.KT_CAN_DOI_DS: return "413_CAN_DOI";
                case Function.KT_TAI_KHOAN_CT: return "414_TAI_KHOAN";
                case Function.KT_TAI_KHOAN_DS: return "414_TAI_KHOAN";
                case Function.KT_TONG_HOP_CT: return "441_TONG_HOP_KT";
                case Function.KT_TONG_HOP_DS: return "441_TONG_HOP_KT";
                case Function.KT_PHIEU_THU: return "422_PHIEU_THU";
                case Function.KT_PHIEU_CHI: return "423_PHIEU_CHI";
                case Function.KT_PHIEU_KE_TOAN: return "424_PHIEU_KE_TOAN";
                case Function.KT_NGOAI_BANG: return "425_NGOAI_BANG";
                case Function.KT_DIEU_CHINH: return "426_DIEU_CHINH";
                case Function.KT_KET_CHUYEN: return "431_KET_CHUYEN";
                case Function.KT_GIAO_DICH: return "KT_GIAO_DICH";
                case Function.KT_CUOI_NGAY: return "462_MO_KHOA_SO_DV";
                case Function.KT_PHIEU_UY_NHIEM_CHI: return "KT_PHIEU_UY_NHIEM_CHI";
                case Function.KT_GIAO_DICH_DOI_TUONG: return "KT_GIAO_DICH_DOI_TUONG";
                case Function.KT_CHUNG_TU_GHI_SO: return "KT_CHUNG_TU_GHI_SO";
                case Function.KT_LAY_TONG_SO_DU: return "KT_LAY_TONG_SO_DU";
                case Function.KT_DANH_GIA_NGOAI_TE: return "KT_DANH_GIA_NGOAI_TE";
                case Function.KT_HE_THONG_TKTH: return "KT_HE_THONG_TKTH";
                case Function.KT_TAI_KHOAN_TH: return "KT_TAI_KHOAN_TH";
                //Huy động vốn
                case Function.HDV_DANH_SACH_SP: return "511_SAN_PHAM_HDV";
                case Function.HDV_SAN_PHAM: return "511_SAN_PHAM_HDV";
                case Function.HDV_DANH_SACH_SO: return "521_MO_SO";
                case Function.HDV_SO_TKQD: return "521_MO_SO";
                case Function.HDV_SO_TKKKH: return "521_MO_SO";
                case Function.HDV_SO_TGCKH: return "521_MO_SO";
                case Function.HDV_SO_TK_TGTT: return "521_MO_SO";
                case Function.HDV_SO_TKCKH: return "521_MO_SO";
                case Function.HDV_GUI_THEM_TIEN_THEO_SO: return "522_GUI_THEM_TIEN";
                case Function.HDV_GUI_THEM_TIEN_THEO_DANH_SACH: return "522_GUI_THEM_TIEN";
                case Function.HDV_GUI_THEM_TIEN_THEO_EXCEL: return "527_NOP_TIEN_CA_EXCEL";
                case Function.HDV_RUT_BOT_GOC: return "523_RUT_BOT_GOC";
                case Function.HDV_RUT_GOC_THEO_DANH_SACH: return "523_RUT_BOT_GOC";
                case Function.HDV_TAT_TOAN: return "524_TAT_TOAN";
                case Function.HDV_TAT_TOAN_THEO_DANH_SACH: return "524_TAT_TOAN";
                case Function.HDV_PHONG_TOA_SD: return "525_PHONG_TOA_SD";
                case Function.HDV_DANH_SACH_PHONG_TOA_SD: return "525_PHONG_TOA_SD";
                case Function.HDV_GIAI_TOA_SD: return "526_GIAI_TOA_SD";
                case Function.HDV_TRA_LAI: return "531_TRA_LAI";
                case Function.HDV_TRA_LAI_THEO_DANH_SACH: return "531_TRA_LAI";
                case Function.HDV_DU_CHI: return "532_DU_CHI";
                case Function.HDV_DAT_LICH_DU_CHI: return "533_DU_CHI_LICH";
                case Function.HDV_LAI_NHAP_GOC_THEO_SO: return "534_LAI_NHAP_GOC";
                case Function.HDV_LAI_NHAP_GOC_THEO_DANH_SACH: return "534_LAI_NHAP_GOC";
                case Function.HDV_PHAN_BO: return "535_PHAN_BO";
                case Function.HDV_DAT_LICH_PHAN_BO: return "536_PHAN_BO_LICH";
                case Function.HDV_DAT_LICH_LAI_NHAP_GOC: return "537_LAI_NHAP_GOC_LICH";
                case Function.HDV_DONG_TK: return "541_DONG_TK";
                case Function.HDV_DANH_SACH_DONG_TK: return "541_DONG_TK";
                case Function.HDV_MO_LAI_TK: return "542_MO_LAI_TK";
                case Function.HDV_DIEU_CHINH_LS: return "551_DIEU_CHINH_LS";
                case Function.HDV_DANH_SACH_DIEU_CHINH_LS: return "551_DIEU_CHINH_LS";
                case Function.HDV_DANG_KY_RUT_GOC: return "HDV_DANG_KY_RUT_GOC";

                //Tài sản và hợp đồng thế chấp - dùng chung cho tín dụng
                case Function.TD_CHI_TIET_LOAI_TAI_SAN: return "TD_CHI_TIET_LOAI_TAI_SAN";
                case Function.TD_DANH_SACH_LOAI_TAI_SAN: return "TD_DANH_SACH_LOAI_TAI_SAN";
                case Function.TD_CHI_TIET_TAI_SAN_DAM_BAO: return "TD_CHI_TIET_TAI_SAN_DAM_BAO";
                case Function.TD_DANH_SACH_TAI_SAN_DAM_BAO: return "TD_DANH_SACH_TAI_SAN_DAM_BAO";
                case Function.TD_CHI_TIET_HOP_DONG_THE_CHAP: return "TD_CHI_TIET_HOP_DONG_THE_CHAP";
                case Function.TD_DANH_SACH_HOP_DONG_THE_CHAP: return "TD_DANH_SACH_HOP_DONG_THE_CHAP";
                //Tín dụng vi mô
                case Function.TDVM_SAN_PHAM_TDVM: return "611_SAN_PHAM_TDVM";
                case Function.TDVM_VONG_VAY: return "612_VONG_VAY";
                case Function.TDVM_HAN_MUC: return "613_HAN_MUC";
                case Function.TDVM_TSDB: return "621_TSDB";
                case Function.TDVM_THE_CHAP: return "622_HDCC_TC_BL";
                case Function.TDVM_DANH_SACH_HOP_DONG: return "641_HOP_DONG";
                case Function.TDVM_CHI_TIET_HOP_DONG: return "641_HOP_DONG";
                case Function.TDVM_DANH_SACH_KHE_UOC: return "644_KHE_UOC";
                case Function.TDVM_DANH_SACH_KHE_UOC_01: return "644_KHE_UOC";
                case Function.TDVM_CHI_TIET_KHE_UOC: return "644_KHE_UOC";
                case Function.TDVM_THOA_THUAN: return "642_THOA_THUAN";
                case Function.TDVM_HOP_DONG_NHOM: return "643_HOP_DONG_NHOM";
                case Function.TDVM_DAT_LICH_TRA_NO: return "645_LICH_TRA_NO";
                case Function.TDVM_TAM_UNG: return "651_TAM_UNG";
                case Function.TDVM_HOAN_UNG: return "652_HOAN_UNG";
                case Function.TDVM_GIAI_NGAN: return "653_GIAI_NGAN";
                case Function.TDVM_DU_THU: return "654_DU_THU";
                case Function.TDVM_DAT_LICH_DU_THU: return "655_DU_THU_LICH";
                case Function.TDVM_TRICH_LAP_DU_PHONG: return "656_TRICH_LAP_DP";
                case Function.TDVM_DAT_LICH_TRICH_LAP_DU_PHONG: return "657_TRICH_LAP_DP_LICH";
                case Function.TDVM_DIEU_CHINH_LAI_SUAT: return "658_DIEU_CHINH_LS";
                case Function.TDVM_GIA_HAN_NO: return "661_GIA_HAN";
                case Function.TDVM_CHUYEN_HOAN_NHOM_NO: return "662_CHUYEN_HOAN";
                case Function.TDVM_CHUYEN_NO_QUA_HAN: return "663_CHUYEN_NO";
                case Function.TDVM_XU_LY_NO: return "664_XOA_NO";
                case Function.TDVM_LAP_HOA_DON_TIEN_KY: return "671_HOA_DON_TTK";
                case Function.TDVM_PHAN_BO_LAI_VAY: return "672_PHAN_BO_LV";
                case Function.TDVM_DAT_LICH_PHAN_BO_LAI_VAY: return "TDVM_DAT_LICH_PHAN_BO_LAI_VAY";
                case Function.TDVM_THU_GOC_LAI_TRUOC_HAN: return "673_THU_GOC_LAI";
                case Function.TDVM_DAT_LICH_THU_PHAT_VON: return "614_LICH_THU_PHAT_VON";
                case Function.TDVM_LAY_KH_THEO_KUOC: return "TDVM_LAY_KH_THEO_KUOC";
                case Function.TDVM_DON_XIN_VAY_VON: return "646_DON_VAY_VON";
                case Function.TDVM_DIA_BAN_SAN_PHAM: return "615_DIA_BAN_SAN_PHAM";
                case Function.TDVM_PHAN_LOAI_NO: return "665_PHAN_LOAI_NO";
                case Function.TDVM_LAP_LICH_THU_GOC_LAI: return "6451_LICH_TRA_NO_CT";
                case Function.TDVM_KIEM_SOAT_RR: return "TDVM_KIEM_SOAT_RR";
                //Bảo hiểm
                case Function.BH_SAN_PHAM_BHTH: return "BH_SAN_PHAM_BHTH";
                case Function.BH_CAM_KET_THEO_KH: return "BH_CAM_KET_THEO_KH";
                case Function.BH_CAM_KET_THEO_DS: return "BH_CAM_KET_THEO_DS";
                case Function.BH_THU_HO_PHIXL: return "BH_THU_HO_PHIXL";
                case Function.BH_CHI_HO_PHIXL: return "BH_CHI_HO_PHIXL";
                case Function.BH_THU_HO_PHIBH: return "BH_THU_HO_PHIBH";
                case Function.BH_CHI_HO_PHIBH: return "BH_CHI_HO_PHIBH";
                case Function.BH_CHAM_DUT: return "BH_CHAM_DUT";
                //Nhân sự
                case Function.NS_DM_QUOC_TICH_CT: return "NS_DM_QUOC_TICH_CT";
                case Function.NS_DM_TINH_TP_CT: return "NS_DM_TINH_TP_CT";
                case Function.NS_DM_QUAN_HUYEN_CT: return "NS_DM_QUAN_HUYEN_CT";
                case Function.NS_DM_PHUONG_XA_CT: return "NS_DM_PHUONG_XA_CT";
                case Function.NS_DM_DAN_TOC_CT: return "NS_DM_DAN_TOC_CT";
                case Function.NS_DM_TON_GIAO_CT: return "NS_DM_TON_GIAO_CT";
                case Function.NS_DM_GIOI_TINH_CT: return "NS_DM_GIOI_TINH_CT";
                case Function.NS_DM_TTRANG_HNHAN_CT: return "NS_DM_TTRANG_HNHAN_CT";
                case Function.NS_DM_CU_TRU_CT: return "NS_DM_CU_TRU_CT";
                case Function.NS_DM_HTHUC_LVIEC_CT: return "NS_DM_HTHUC_LVIEC_CT";
                case Function.NS_DM_TDO_HVAN_CT: return "NS_DM_TDO_HVAN_CT";
                case Function.NS_DM_HTHUC_DTAO_CT: return "NS_DM_HTHUC_DTAO_CT";
                case Function.NS_DM_TRUONG_DTAO_CT: return "NS_DM_TRUONG_DTAO_CT";
                case Function.NS_DM_BANG_CAP_CT: return "NS_DM_BANG_CAP_CT";
                case Function.NS_DM_KHOA_DTAO_CT: return "NS_DM_KHOA_DTAO_CT";
                case Function.NS_DM_CNGANH_DTAO_CT: return "NS_DM_CNGANH_DTAO_CT";
                case Function.NS_DM_HOC_HAM_CT: return "NS_DM_HOC_HAM_CT";
                case Function.NS_DM_HOC_VI_CT: return "NS_DM_HOC_VI_CT";
                case Function.NS_DM_TDO_TANH_CT: return "NS_DM_TDO_TANH_CT";
                case Function.NS_DM_TDO_THOC_CT: return "NS_DM_TDO_THOC_CT";
                case Function.NS_DM_TDO_CTRI_CT: return "NS_DM_TDO_CTRI_CT";
                case Function.NS_DM_XEP_LOAI_CT: return "NS_DM_XEP_LOAI_CT";
                case Function.NS_DM_QHE_GDINH_CT: return "NS_DM_QHE_GDINH_CT";
                case Function.NS_DM_KY_NANG_CT: return "NS_DM_KY_NANG_CT";
                case Function.NS_DM_NGHE_NGHIEP_CT: return "NS_DM_NGHE_NGHIEP_CT";
                case Function.NS_DM_CHUC_VU_CT: return "NS_DM_CHUC_VU_CT";
                case Function.NS_DM_LDO_NPHEP_CT: return "NS_DM_LDO_NPHEP_CT";
                case Function.NS_DM_LDO_TVIEC_CT: return "NS_DM_LDO_TVIEC_CT";
                case Function.NS_DM_THAN_HDLD_CT: return "NS_DM_THAN_HDLD_CT";
                case Function.NS_DM_LOAI_HDLD_CT: return "NS_DM_LOAI_HDLD_CT";
                case Function.NS_DM_HTHUC_TLUONG_CT: return "NS_DM_HTHUC_TLUONG_CT";
                case Function.NS_DM_LOAI_TNHAP_CT: return "NS_DM_LOAI_TNHAP_CT";
                case Function.NS_DM_LOAI_CPHI_CT: return "NS_DM_LOAI_CPHI_CT";
                case Function.NS_DM_HTHUC_KTHUONG_CT: return "NS_DM_HTHUC_KTHUONG_CT";
                case Function.NS_DM_HTHUC_KLUAT_CT: return "NS_DM_HTHUC_KLUAT_CT";
                case Function.NS_DM_KHIEU_CCONG_CT: return "NS_DM_KHIEU_CCONG_CT";
                case Function.NS_DM_LOAI_HSO_CT: return "NS_DM_LOAI_HSO_CT";
                case Function.NS_DM_DVI_TGIAN_CT: return "NS_DM_DVI_TGIAN_CT";
                case Function.NS_DM_DVI_CTAC_CT: return "NS_DM_DVI_CTAC_CT";
                case Function.NS_DM_LOAI_GTO_CT: return "NS_DM_LOAI_GTO_CT";
                case Function.NS_DM_BVIEN_KCB_CT: return "NS_DM_BVIEN_KCB_CT";
                case Function.NS_DM_NHOM_CTV_CT: return "NS_DM_NHOM_CTV_CT";
                case Function.NS_DM_DU_AN_CT: return "NS_DM_DU_AN_CT";
                case Function.NS_DM_CHUC_VU_DU_AN_CT: return "NS_DM_CHUC_VU_DU_AN_CT";
                case Function.NS_DM_CHUC_VU_CTV_CT: return "NS_DM_CHUC_VU_CTV_CT";
                case Function.NS_DM_PHU_CAP_CT: return "NS_DM_PHU_CAP_CT";
                case Function.NS_DM_DANH_MUC_CT: return "NS_DM_DANH_MUC_CT";
                case Function.NS_DM_DANH_MUC_PL: return "NS_DM_DANH_MUC_PL";

                case Function.NS_DM_QUOC_TICH_DS: return "NS_DM_QUOC_TICH_DS";
                case Function.NS_DM_TINH_TP_DS: return "NS_DM_TINH_TP_DS";
                case Function.NS_DM_QUAN_HUYEN_DS: return "NS_DM_QUAN_HUYEN_DS";
                case Function.NS_DM_PHUONG_XA_DS: return "NS_DM_PHUONG_XA_DS";
                case Function.NS_DM_DAN_TOC_DS: return "NS_DM_DAN_TOC_DS";
                case Function.NS_DM_TON_GIAO_DS: return "NS_DM_TON_GIAO_DS";
                case Function.NS_DM_GIOI_TINH_DS: return "NS_DM_GIOI_TINH_DS";
                case Function.NS_DM_TTRANG_HNHAN_DS: return "NS_DM_TTRANG_HNHAN_DS";
                case Function.NS_DM_CU_TRU_DS: return "NS_DM_CU_TRU_DS";
                case Function.NS_DM_HTHUC_LVIEC_DS: return "NS_DM_HTHUC_LVIEC_DS";
                case Function.NS_DM_TDO_HVAN_DS: return "NS_DM_TDO_HVAN_DS";
                case Function.NS_DM_HTHUC_DTAO_DS: return "NS_DM_HTHUC_DTAO_DS";
                case Function.NS_DM_TRUONG_DTAO_DS: return "NS_DM_TRUONG_DTAO_DS";
                case Function.NS_DM_BANG_CAP_DS: return "NS_DM_BANG_CAP_DS";
                case Function.NS_DM_KHOA_DTAO_DS: return "NS_DM_KHOA_DTAO_DS";
                case Function.NS_DM_CNGANH_DTAO_DS: return "NS_DM_CNGANH_DTAO_DS";
                case Function.NS_DM_HOC_HAM_DS: return "NS_DM_HOC_HAM_DS";
                case Function.NS_DM_HOC_VI_DS: return "NS_DM_HOC_VI_DS";
                case Function.NS_DM_TDO_TANH_DS: return "NS_DM_TDO_TANH_DS";
                case Function.NS_DM_TDO_THOC_DS: return "NS_DM_TDO_THOC_DS";
                case Function.NS_DM_TDO_CTRI_DS: return "NS_DM_TDO_CTRI_DS";
                case Function.NS_DM_XEP_LOAI_DS: return "NS_DM_XEP_LOAI_DS";
                case Function.NS_DM_QHE_GDINH_DS: return "NS_DM_QHE_GDINH_DS";
                case Function.NS_DM_KY_NANG_DS: return "NS_DM_KY_NANG_DS";
                case Function.NS_DM_NGHE_NGHIEP_DS: return "NS_DM_NGHE_NGHIEP_DS";
                case Function.NS_DM_CHUC_VU_DS: return "NS_DM_CHUC_VU_DS";
                case Function.NS_DM_LDO_NPHEP_DS: return "NS_DM_LDO_NPHEP_DS";
                case Function.NS_DM_LDO_TVIEC_DS: return "NS_DM_LDO_TVIEC_DS";
                case Function.NS_DM_THAN_HDLD_DS: return "NS_DM_THAN_HDLD_DS";
                case Function.NS_DM_LOAI_HDLD_DS: return "NS_DM_LOAI_HDLD_DS";
                case Function.NS_DM_HTHUC_TLUONG_DS: return "NS_DM_HTHUC_TLUONG_DS";
                case Function.NS_DM_LOAI_TNHAP_DS: return "NS_DM_LOAI_TNHAP_DS";
                case Function.NS_DM_LOAI_CPHI_DS: return "NS_DM_LOAI_CPHI_DS";
                case Function.NS_DM_HTHUC_KTHUONG_DS: return "NS_DM_HTHUC_KTHUONG_DS";
                case Function.NS_DM_HTHUC_KLUAT_DS: return "NS_DM_HTHUC_KLUAT_DS";
                case Function.NS_DM_KHIEU_CCONG_DS: return "NS_DM_KHIEU_CCONG_DS";
                case Function.NS_DM_LOAI_HSO_DS: return "NS_DM_LOAI_HSO_DS";
                case Function.NS_DM_DVI_TGIAN_DS: return "NS_DM_DVI_TGIAN_DS";
                case Function.NS_DM_DVI_CTAC_DS: return "NS_DM_DVI_CTAC_DS";
                case Function.NS_DM_LOAI_GTO_DS: return "NS_DM_LOAI_GTO_DS";
                case Function.NS_DM_BVIEN_KCB_DS: return "NS_DM_BVIEN_KCB_DS";
                case Function.NS_DM_NHOM_CTV_DS: return "NS_DM_NHOM_CTV_DS";
                case Function.NS_DM_DU_AN_DS: return "NS_DM_DU_AN_DS";
                case Function.NS_DM_CHUC_VU_DU_AN_DS: return "NS_DM_CHUC_VU_DU_AN_DS";
                case Function.NS_DM_CHUC_VU_CTV_DS: return "NS_DM_CHUC_VU_CTV_DS";
                case Function.NS_DM_PHU_CAP_DS: return "NS_DM_PHU_CAP_DS";
                case Function.NS_DM_DANH_MUC_DS: return "NS_DM_DANH_MUC_DS";

                case Function.NS_HO_SO_CT: return "811_HO_SO";
                case Function.NS_HO_SO_DS: return "811_HO_SO";
                case Function.NS_HOP_DONG_CT: return "812_HOP_DONG";
                case Function.NS_HOP_DONG_DS: return "812_HOP_DONG";
                case Function.NS_THUYEN_CHUYEN_CT: return "821_THUYEN_CHUYEN";
                case Function.NS_THUYEN_CHUYEN_DS: return "821_THUYEN_CHUYEN";
                case Function.NS_THOI_VIEC_CT: return "828_THOI_VIEC";
                case Function.NS_THOI_VIEC_DS: return "828_THOI_VIEC";
                case Function.NS_BANG_LUONG: return "844_BANG_LUONG";
                case Function.NS_LUONG_CT: return "842_LUONG";
                case Function.NS_LUONG_DS: return "842_LUONG";
                case Function.NS_LUONG_DCHINH_CT: return "848_DIEU_CHINH_LUONG";
                case Function.NS_LUONG_DCHINH_DS: return "848_DIEU_CHINH_LUONG";
                case Function.NS_TINH_LUONG_CT: return "845_TINH_LUONG";
                case Function.NS_TINH_LUONG_DS: return "845_TINH_LUONG";
                case Function.NS_TIEU_CHI_PHU_CAP_CTV: return "846_BANG_PHU_CAP_CTV";
                case Function.NS_TINH_PHU_CAP_CTV_CT: return "847_TINH_PHU_CAP_CTV";
                case Function.NS_TINH_PHU_CAP_CTV_DS: return "847_TINH_PHU_CAP_CTV";
                case Function.NS_QLY_DU_AN: return "NS_QLY_DU_AN";

                case Function.NS_LUONG: return "NS_LUONG";
                case Function.NS_CHUC_VU: return "NS_CHUC_VU";
                case Function.NS_KHEN_THUONG: return "NS_KHEN_THUONG";
                case Function.NS_KY_LUAT: return "NS_KY_LUAT";
                case Function.NS_DAO_TAO: return "NS_DAO_TAO";
                case Function.NS_NGHI_PHEP: return "NS_NGHI_PHEP";
                case Function.NS_TIEU_CHI: return "NS_TIEU_CHI";
                case Function.NS_DANH_SACH: return "NS_DANH_SACH";
                case Function.NS_PHU_CAP: return "NS_PHU_CAP";
                //Khai thác dữ liệu
                case Function.KTDL_MAU_KHAI_THAC_DL: return "KTDL_MAU_KHAI_THAC_DL";
                case Function.KTDL_PHAN_HE_CHUNG: return "KTDL_PHAN_HE_CHUNG";
                case Function.KTDL_KHACH_HANG: return "KTDL_KHACH_HANG";
                case Function.KTDL_KE_TOAN: return "KTDL_KE_TOAN";
                case Function.KTDL_HUY_DONG: return "KTDL_HUY_DONG";
                case Function.KTDL_TIN_DUNG_VI_MO: return "KTDL_TIN_DUNG_VI_MO";
                case Function.KTDL_TIN_DUNG: return "KTDL_TIN_DUNG";
                case Function.KTDL_BAO_HIEM: return "KTDL_BAO_HIEM";
                case Function.KTDL_NHAN_SU: return "KTDL_NHAN_SU";
                case Function.KTDL_SMS_KICH_HOAT: return "KTDL_SMS_KICH_HOAT";
                case Function.KTDL_SMS_DONG_BO: return "KTDL_SMS_DONG_BO";
                case Function.KTDL_SMS_CAU_HINH: return "KTDL_SMS_CAU_HINH";
                case Function.KTDL_SMS_DANG_KY: return "KTDL_SMS_DANG_KY";
                case Function.KTDL_SMS_TRUY_VAN: return "KTDL_SMS_TRUY_VAN";
                case Function.KTDL_SMS_DUYET: return "KTDL_SMS_DUYET";
                case Function.KTDL_DM_DL_CT: return "KTDL_DM_DL_CT";
                case Function.KTDL_DM_CT: return "KTDL_DM_CT";
                case Function.KTDL_DM_DS: return "KTDL_DM_DS";

                case Function.TS_DM_NHOM_TS_CT: return "TS_DM_NHOM_TS_CT";
                case Function.TS_DM_NHOM_TS_GET_NHOM_CHA: return "TS_DM_NHOM_TS_GET_NHOM_CHA";
                case Function.TS_DM_NHOM_TS_DS: return "TS_DM_NHOM_TS_DS";
                case Function.TS_TAI_SAN: return "TS_TAI_SAN";
                case Function.TS_TANG: return "TS_TANG";
                case Function.TS_TANG_DS: return "TS_TANG_DS";
                case Function.TS_TANG_NG: return "TS_TANG_NG";
                case Function.TS_BAN_GIAO: return "TS_BAN_GIAO";
                case Function.TS_BAN_GIAO_DS: return "TS_BAN_GIAO_DS";
                case Function.TS_NANG_CAP: return "TS_NANG_CAP";
                case Function.TS_NANG_CAP_DS: return "TS_NANG_CAP_DS";
                case Function.TS_DANH_GIA: return "TS_DANH_GIA";
                case Function.TS_DANH_GIA_DS: return "TS_DANH_GIA_DS";
                case Function.TS_GIAM: return "TS_GIAM";
                case Function.TS_GIAM_DS: return "TS_GIAM_DS";
                case Function.TS_KHAU_HAO: return "TS_KHAU_HAO";
                case Function.TS_KHAU_HAO_DS: return "TS_KHAU_HAO_DS";

                //Tài sản đảm bảo
                case Function.TD_TSDB_LOAI_CT: return "TSDB_LOAI_CT";
                case Function.TD_TSDB_LOAI_DS: return "TSDB_LOAI_DS";
                case Function.TD_TSDB_CT: return "TD_TSDB_CT";
                case Function.TD_TSDB_DS: return "TD_TSDB_DS";
                case Function.TD_TSDB_LOAI: return "TSDB_LOAI";

                //Hợp đồng thế chấp
                case Function.TSDB_HOP_DONG_THE_CHAP: return "TSDB_HOP_DONG_THE_CHAP";
                case Function.TSDB_HOP_DONG_THE_CHAP_NHAP: return "TSDB_HOP_DONG_THE_CHAP_NHAP";
                case Function.TSDB_HOP_DONG_THE_CHAP_XUAT: return "TSDB_HOP_DONG_THE_CHAP_XUAT";

                //Hạn mức
                case Function.HM_TONG: return "HM_TONG";
                case Function.HM_CTIET_CT: return "HM_CTIET_CT";
                case Function.HM_CTIET_DS: return "HM_CTIET_DS";

                //Tín dụng thông thường
                case Function.TD_SAN_PHAMTT: return "TD_SAN_PHAMTT";
                case Function.TD_SAN_PHAMTT_DS: return "TD_SAN_PHAMTT_DS";
                case Function.TD_HDTD: return "TD_HDTD";
                case Function.TD_HDTD_DS: return "TD_HDTD_DS";
                case Function.TD_KUOC: return "TD_KUOC";
                case Function.TD_KUOC_DS: return "TD_KUOC_DS";
                case Function.TD_GIAI_NGAN: return "TD_GIAI_NGAN";

                //Tín dụng tiêu dùng
                case Function.TDTD_SAN_PHAM: return "TDTD_SAN_PHAM";
                case Function.TDTD_DON_XIN_VAY_VON: return "TDTD_DON_VAY_VON";
                case Function.TDTD_HOP_DONG_CA_NHAN: return "TDTD_HOP_DONG";
                case Function.TDTD_KHE_UOC: return "TDTD_KHE_UOC";
                case Function.TDTD_GIAI_NGAN: return "TDTD_GIAI_NGAN";
                case Function.TDTD_GIAI_NGAN_DAI_LY: return "TDTD_GIAI_NGAN_DAI_LY";
                case Function.TDTD_THU_GOC_LAI: return "TDTD_THU_GOC_LAI";
                case Function.TDTD_KIEM_SOAT_RR: return "TDTD_KIEM_SOAT_RR";
                case Function.TDTD_CHUYEN_HOAN: return "TDTD_CHUYEN_HOAN";
                case Function.TDTD_CHUYEN_NO: return "TDTD_CHUYEN_NO";
                case Function.TDTD_DU_THU: return "TDTD_DU_THU";
                case Function.TDTD_TRICH_LAP_DP: return "TDTD_TRICH_LAP_DP";
                case Function.TDTD_TSDB_CT: return "TDTD_TSDB_CT";
                case Function.TDTD_TSDB_DS: return "TDTD_TSDB_DS";
                case Function.TDTD_HDTC_CT: return "TDTD_HDTC_CT";
                case Function.TDTD_HDTC_DS: return "TDTD_HDTC_DS";

                case Function.SYS_JOB_EMAIL: return "SYS_JOB_EMAIL";
                case Function.SYS_JOB_SUBSCRIBE: return "SYS_JOB_SUBSCRIBE";
                case Function.SYS_JOB_HIS: return "SYS_JOB_HIS";

                case Function.SMS_DANG_KY_DTUONG: return "SMS_DANG_KY_DTUONG";
                case Function.SMS_DANG_KY_DVU: return "SMS_DANG_KY_DVU";
                case Function.SMS_DANG_KY_LOAI_DTUONG: return "SMS_DANG_KY_LOAI_DTUONG";
                case Function.SMS_DANG_KY_TNHAN_CDONG: return "SMS_DANG_KY_TNHAN_CDONG";
                case Function.SMS_DSACH_TNHAN_DEN: return "SMS_DSACH_TNHAN_DEN";
                case Function.SMS_DSACH_TNHAN_DI: return "SMS_DSACH_TNHAN_DI";
                case Function.SMS_DANG_KY_SO_DIEN_THOAI: return "SMS_DANG_KY_SO_DIEN_THOAI";
                case Function.SMS_QUAN_LY_KET_NOI: return "SMS_QUAN_LY_KET_NOI";

                default: return "";
            }
        }
        public static string getSinhMa(this Function function)
        {
            switch (function)
            {
                //Huy động vốn
                case Function.HDV_SO_TKQD: return "1";
                case Function.HDV_SO_TKKKH: return "1";
                case Function.HDV_SO_TGCKH: return "1";
                case Function.HDV_SO_TK_TGTT: return "0";
                case Function.HDV_SO_TKCKH: return "1";

                //Tín dụng vi mô

                case Function.TDVM_CHI_TIET_HOP_DONG: return "6";
                case Function.TDVM_DANH_SACH_KHE_UOC: return "6";
                case Function.TDVM_DANH_SACH_KHE_UOC_01: return "6";
                case Function.TDVM_CHI_TIET_KHE_UOC: return "6";
                case Function.TDVM_THOA_THUAN: return "6";
                case Function.TDVM_HOP_DONG_NHOM: return "6";
                case Function.TDVM_DAT_LICH_TRA_NO: return "6";
                case Function.TDVM_TAM_UNG: return "6";
                case Function.TDVM_HOAN_UNG: return "6";
                case Function.TDVM_GIAI_NGAN: return "6";
                case Function.TDVM_DU_THU: return "6";
                case Function.TDVM_DAT_LICH_DU_THU: return "6";
                case Function.TDVM_TRICH_LAP_DU_PHONG: return "6";
                case Function.TDVM_DAT_LICH_TRICH_LAP_DU_PHONG: return "6";
                case Function.TDVM_DIEU_CHINH_LAI_SUAT: return "6";
                case Function.TDVM_GIA_HAN_NO: return "6";
                case Function.TDVM_CHUYEN_HOAN_NHOM_NO: return "6";
                case Function.TDVM_CHUYEN_NO_QUA_HAN: return "6";
                case Function.TDVM_XU_LY_NO: return "6";
                case Function.TDVM_LAP_HOA_DON_TIEN_KY: return "6";
                case Function.TDVM_PHAN_BO_LAI_VAY: return "6";
                case Function.TDVM_DAT_LICH_PHAN_BO_LAI_VAY: return "6";
                case Function.TDVM_THU_GOC_LAI_TRUOC_HAN: return "6";
                case Function.TDVM_DAT_LICH_THU_PHAT_VON: return "6";
                case Function.TDVM_LAY_KH_THEO_KUOC: return "6";
                case Function.TDVM_DON_XIN_VAY_VON: return "6";
                case Function.TDVM_DIA_BAN_SAN_PHAM: return "6";
                case Function.TDVM_PHAN_LOAI_NO: return "6";
                case Function.TDVM_LAP_LICH_THU_GOC_LAI: return "6";

                //Tín dụng tiêu dùng
                case Function.TDTD_DON_XIN_VAY_VON: return "5";
                case Function.TDTD_HOP_DONG_CA_NHAN: return "5";
                case Function.TDTD_KHE_UOC: return "5";
                case Function.TDTD_GIAI_NGAN: return "5";
                case Function.TDTD_GIAI_NGAN_DAI_LY: return "5";
                case Function.TDTD_THU_GOC_LAI: return "5";
                case Function.TDTD_KIEM_SOAT_RR: return "5";
                case Function.TDTD_CHUYEN_HOAN: return "5";
                case Function.TDTD_CHUYEN_NO: return "5";
                case Function.TDTD_DU_THU: return "5";
                case Function.TDTD_TRICH_LAP_DP: return "5";

                default: return "";
            }
        }

        public static int getApprovedMatrixValue(this Function function)
        {
            switch (function)
            {
                //Hệ thống
                case Function.HT_LOGIN: return 0;
                case Function.HT_LOGOUT: return 0;
                case Function.HT_NHNSD: return 0;
                case Function.HT_NSD: return 0;
                case Function.HT_NHNSD_NSD: return 0;
                case Function.HT_NSD_HIEN_THOI: return 0;
                case Function.HT_PQ_CHUC_NANG: return 0;
                case Function.HT_PQ_PHAM_VI: return 0;
                case Function.HT_THAM_SO: return 0;
                case Function.HT_SAO_LUU: return 0;
                case Function.HT_PHUC_HOI: return 0;
                case Function.HT_KIEM_TRA_PB: return 0;
                case Function.HT_CAP_NHAT_PB: return 0;
                case Function.HT_DANG_NHAP: return 0;
                //Dùng chung
                case Function.DC_DM_TINH_THANH: return 0;
                case Function.DC_DM_DIA_BAN: return 0;
                case Function.DC_DM_DON_VI: return 0;
                case Function.DC_DM_KHU_VUC: return 0;
                case Function.DC_DM_KHU_VUC_DS: return 0;
                case Function.DC_DM_CUM: return 0;
                case Function.DC_DM_CUM_DS: return 0;
                case Function.DC_DM_NHOM: return 0;
                case Function.DC_DM_NHOM_DS: return 0;
                case Function.DC_DM_DUNG_CHUNG: return 0;
                case Function.DC_DM_PHAN_HE_GD: return 0;
                case Function.DC_DM_QUOC_GIA: return 0;
                case Function.DC_DM_TIEN_TE: return 0;
                case Function.DC_DM_LICH_HOP: return 0;
                case Function.DC_LAI_SUAT_CT: return 2;
                case Function.DC_LAI_SUAT_DS: return 1;
                case Function.DC_PHI: return 1;
                case Function.DC_LOAI_TY_GIA: return 0;
                case Function.DC_TY_GIA: return 0;
                case Function.DC_HAN_MUC: return 0;
                case Function.DC_HAN_MUC_CTIET: return 0;
                case Function.DC_DM_DTUONG_SODU: return 0;
                case Function.DC_DM_LOAI_DTUONG: return 0;
                case Function.DC_DM_DTUONG: return 0;
                //Khách hàng
                case Function.KH_NHOM: return 1;
                case Function.KH_THANH_VIEN: return 1; // Sua cho BIDV: 2 thanh 1
                case Function.KH_CA_NHAN: return 1; // Sua cho BIDV: 2 thanh 1
                case Function.KH_TO_CHUC: return 1; // Sua cho BIDV: 2 thanh 1
                case Function.KH_DANH_SACH: return 1; // Sua cho BIDV: 2 thanh 1
                case Function.KH_CHUYEN_DIA_BAN: return 1;
                case Function.KH_CAP_NHAT_KH: return 1;
                case Function.KH_XEP_HANG_TD: return 1;
                case Function.KH_XEP_HANG_NGHEO: return 1;
                case Function.KH_QL_HINH_HANH: return 0;
                case Function.KH_THONG_TIN_KHAO_SAT_CT: return 1;
                case Function.KH_THONG_TIN_KHAO_SAT_DS: return 1;
                //Kế toán
                case Function.KT_TAI_KHOAN_TH: return 1;
                case Function.KT_HE_THONG_TKTH: return 1;
                case Function.KT_PHAN_LOAI_CT: return 1;
                case Function.KT_PHAN_LOAI_DS: return 1;
                case Function.KT_BUT_TOAN_CT: return 1;
                case Function.KT_BUT_TOAN_DS: return 1;
                case Function.KT_CAN_DOI_CT: return 1;
                case Function.KT_CAN_DOI_DS: return 1;
                case Function.KT_TAI_KHOAN_CT: return 0;
                case Function.KT_TAI_KHOAN_DS: return 1;
                case Function.KT_TONG_HOP_CT: return 1;
                case Function.KT_TONG_HOP_DS: return 1;
                case Function.KT_PHIEU_THU: return 1;
                case Function.KT_PHIEU_CHI: return 1;
                case Function.KT_PHIEU_KE_TOAN: return 1;
                case Function.KT_NGOAI_BANG: return 1;
                case Function.KT_DIEU_CHINH: return 1;
                case Function.KT_KET_CHUYEN: return 1;
                case Function.KT_GIAO_DICH: return 1;
                case Function.KT_PHIEU_UY_NHIEM_CHI: return 1;
                case Function.KT_CHUNG_TU_GHI_SO: return 1;
                case Function.KT_DANH_GIA_NGOAI_TE: return 1;
                //Huy động vốn
                case Function.HDV_SAN_PHAM: return 2;
                case Function.HDV_DANH_SACH_SO: return 1;
                case Function.HDV_SO_TKQD: return 1;
                case Function.HDV_SO_TKKKH: return 1;
                case Function.HDV_SO_TGCKH: return 1;
                case Function.HDV_SO_TK_TGTT: return 1;
                case Function.HDV_SO_TKCKH: return 1;
                case Function.HDV_GUI_THEM_TIEN_THEO_SO: return 1;
                case Function.HDV_GUI_THEM_TIEN_THEO_DANH_SACH: return 1;
                case Function.HDV_GUI_THEM_TIEN_THEO_EXCEL: return 1;
                case Function.HDV_RUT_BOT_GOC: return 1;
                case Function.HDV_RUT_GOC_THEO_DANH_SACH: return 1;
                case Function.HDV_TAT_TOAN: return 1;
                case Function.HDV_TAT_TOAN_THEO_DANH_SACH: return 1;
                case Function.HDV_PHONG_TOA_SD: return 1;
                case Function.HDV_GIAI_TOA_SD: return 1;
                case Function.HDV_TRA_LAI: return 1;
                case Function.HDV_TRA_LAI_THEO_DANH_SACH: return 1;
                case Function.HDV_DU_CHI: return 1;
                case Function.HDV_DAT_LICH_DU_CHI: return 1;
                case Function.HDV_LAI_NHAP_GOC_THEO_SO: return 1;
                case Function.HDV_LAI_NHAP_GOC_THEO_DANH_SACH: return 1;
                case Function.HDV_PHAN_BO: return 1;
                case Function.HDV_DAT_LICH_PHAN_BO: return 1;
                case Function.HDV_DAT_LICH_LAI_NHAP_GOC: return 1;
                case Function.HDV_DONG_TK: return 1;
                case Function.HDV_MO_LAI_TK: return 1;
                case Function.HDV_DIEU_CHINH_LS: return 1;
                //Tài sản và hợp đồng thế chấp - dùng chung cho tín dụng
                case Function.TD_CHI_TIET_LOAI_TAI_SAN: return 1;
                case Function.TD_DANH_SACH_LOAI_TAI_SAN: return 1;
                case Function.TD_CHI_TIET_TAI_SAN_DAM_BAO: return 1;
                case Function.TD_DANH_SACH_TAI_SAN_DAM_BAO: return 1;
                case Function.TD_CHI_TIET_HOP_DONG_THE_CHAP: return 1;
                case Function.TD_DANH_SACH_HOP_DONG_THE_CHAP: return 1;
                //Tín dụng vi mô
                case Function.TDVM_SAN_PHAM_TDVM: return 2;
                case Function.TDVM_VONG_VAY: return 2;
                case Function.TDVM_HAN_MUC: return 1;
                case Function.TDVM_TSDB: return 1;
                case Function.TDVM_THE_CHAP: return 1;
                case Function.TDVM_DANH_SACH_HOP_DONG: return 2;
                case Function.TDVM_CHI_TIET_HOP_DONG: return 2;
                case Function.TDVM_DANH_SACH_KHE_UOC: return 2;
                case Function.TDVM_DANH_SACH_KHE_UOC_01: return 2;
                case Function.TDVM_CHI_TIET_KHE_UOC: return 2;
                case Function.TDVM_THOA_THUAN: return 1;
                case Function.TDVM_HOP_DONG_NHOM: return 1;
                case Function.TDVM_DAT_LICH_TRA_NO: return 1;
                case Function.TDVM_TAM_UNG: return 1;
                case Function.TDVM_HOAN_UNG: return 1;
                case Function.TDVM_GIAI_NGAN: return 1;
                case Function.TDVM_DU_THU: return 1;
                case Function.TDVM_DAT_LICH_DU_THU: return 1;
                case Function.TDVM_TRICH_LAP_DU_PHONG: return 1;
                case Function.TDVM_DAT_LICH_TRICH_LAP_DU_PHONG: return 1;
                case Function.TDVM_DIEU_CHINH_LAI_SUAT: return 1;
                case Function.TDVM_GIA_HAN_NO: return 1;
                case Function.TDVM_CHUYEN_HOAN_NHOM_NO: return 1;
                case Function.TDVM_CHUYEN_NO_QUA_HAN: return 1;
                case Function.TDVM_XU_LY_NO: return 1;
                case Function.TDVM_LAP_HOA_DON_TIEN_KY: return 1;
                case Function.TDVM_PHAN_BO_LAI_VAY: return 1;
                case Function.TDVM_DAT_LICH_PHAN_BO_LAI_VAY: return 1;
                case Function.TDVM_THU_GOC_LAI_TRUOC_HAN: return 1;
                case Function.TDVM_DAT_LICH_THU_PHAT_VON: return 1;
                case Function.TDVM_DON_XIN_VAY_VON: return 1;
                case Function.TDVM_DIA_BAN_SAN_PHAM: return 1;
                case Function.TDVM_PHAN_LOAI_NO: return 1;
                case Function.TDVM_KIEM_SOAT_RR: return 1;
                //Bảo hiểm
                case Function.BH_SAN_PHAM_BHTH: return 1;
                case Function.BH_CAM_KET_THEO_KH: return 1;
                case Function.BH_CAM_KET_THEO_DS: return 1;
                case Function.BH_THU_HO_PHIXL: return 1;
                case Function.BH_CHI_HO_PHIXL: return 1;
                case Function.BH_THU_HO_PHIBH: return 1;
                case Function.BH_CHI_HO_PHIBH: return 1;
                case Function.BH_CHAM_DUT: return 1;
                //Nhân sự
                case Function.NS_DM_QUOC_TICH_CT: return 0;
                case Function.NS_DM_TINH_TP_CT: return 0;
                case Function.NS_DM_QUAN_HUYEN_CT: return 0;
                case Function.NS_DM_PHUONG_XA_CT: return 0;
                case Function.NS_DM_DAN_TOC_CT: return 0;
                case Function.NS_DM_TON_GIAO_CT: return 0;
                case Function.NS_DM_GIOI_TINH_CT: return 0;
                case Function.NS_DM_TTRANG_HNHAN_CT: return 0;
                case Function.NS_DM_CU_TRU_CT: return 0;
                case Function.NS_DM_HTHUC_LVIEC_CT: return 0;
                case Function.NS_DM_TDO_HVAN_CT: return 0;
                case Function.NS_DM_HTHUC_DTAO_CT: return 0;
                case Function.NS_DM_TRUONG_DTAO_CT: return 0;
                case Function.NS_DM_BANG_CAP_CT: return 0;
                case Function.NS_DM_KHOA_DTAO_CT: return 0;
                case Function.NS_DM_CNGANH_DTAO_CT: return 0;
                case Function.NS_DM_HOC_HAM_CT: return 0;
                case Function.NS_DM_HOC_VI_CT: return 0;
                case Function.NS_DM_TDO_TANH_CT: return 0;
                case Function.NS_DM_TDO_THOC_CT: return 0;
                case Function.NS_DM_TDO_CTRI_CT: return 0;
                case Function.NS_DM_XEP_LOAI_CT: return 0;
                case Function.NS_DM_QHE_GDINH_CT: return 0;
                case Function.NS_DM_KY_NANG_CT: return 0;
                case Function.NS_DM_NGHE_NGHIEP_CT: return 0;
                case Function.NS_DM_CHUC_VU_CT: return 0;
                case Function.NS_DM_LDO_NPHEP_CT: return 0;
                case Function.NS_DM_LDO_TVIEC_CT: return 0;
                case Function.NS_DM_THAN_HDLD_CT: return 0;
                case Function.NS_DM_LOAI_HDLD_CT: return 0;
                case Function.NS_DM_HTHUC_TLUONG_CT: return 0;
                case Function.NS_DM_LOAI_TNHAP_CT: return 0;
                case Function.NS_DM_LOAI_CPHI_CT: return 0;
                case Function.NS_DM_HTHUC_KTHUONG_CT: return 0;
                case Function.NS_DM_HTHUC_KLUAT_CT: return 0;
                case Function.NS_DM_KHIEU_CCONG_CT: return 0;
                case Function.NS_DM_LOAI_HSO_CT: return 0;
                case Function.NS_DM_DVI_TGIAN_CT: return 0;
                case Function.NS_DM_DVI_CTAC_CT: return 0;
                case Function.NS_DM_LOAI_GTO_CT: return 0;
                case Function.NS_DM_BVIEN_KCB_CT: return 0;
                case Function.NS_DM_NHOM_CTV_CT: return 0;
                case Function.NS_DM_DU_AN_CT: return 0;
                case Function.NS_DM_CHUC_VU_DU_AN_CT: return 0;
                case Function.NS_DM_CHUC_VU_CTV_CT: return 0;
                case Function.NS_DM_PHU_CAP_CT: return 0;
                case Function.NS_DM_DANH_MUC_CT: return 0;
                case Function.NS_DM_DANH_MUC_PL: return 0;

                case Function.NS_DM_QUOC_TICH_DS: return 0;
                case Function.NS_DM_TINH_TP_DS: return 0;
                case Function.NS_DM_QUAN_HUYEN_DS: return 0;
                case Function.NS_DM_PHUONG_XA_DS: return 0;
                case Function.NS_DM_DAN_TOC_DS: return 0;
                case Function.NS_DM_TON_GIAO_DS: return 0;
                case Function.NS_DM_GIOI_TINH_DS: return 0;
                case Function.NS_DM_TTRANG_HNHAN_DS: return 0;
                case Function.NS_DM_CU_TRU_DS: return 0;
                case Function.NS_DM_HTHUC_LVIEC_DS: return 0;
                case Function.NS_DM_TDO_HVAN_DS: return 0;
                case Function.NS_DM_HTHUC_DTAO_DS: return 0;
                case Function.NS_DM_TRUONG_DTAO_DS: return 0;
                case Function.NS_DM_BANG_CAP_DS: return 0;
                case Function.NS_DM_KHOA_DTAO_DS: return 0;
                case Function.NS_DM_CNGANH_DTAO_DS: return 0;
                case Function.NS_DM_HOC_HAM_DS: return 0;
                case Function.NS_DM_HOC_VI_DS: return 0;
                case Function.NS_DM_TDO_TANH_DS: return 0;
                case Function.NS_DM_TDO_THOC_DS: return 0;
                case Function.NS_DM_TDO_CTRI_DS: return 0;
                case Function.NS_DM_XEP_LOAI_DS: return 0;
                case Function.NS_DM_QHE_GDINH_DS: return 0;
                case Function.NS_DM_KY_NANG_DS: return 0;
                case Function.NS_DM_NGHE_NGHIEP_DS: return 0;
                case Function.NS_DM_CHUC_VU_DS: return 0;
                case Function.NS_DM_LDO_NPHEP_DS: return 0;
                case Function.NS_DM_LDO_TVIEC_DS: return 0;
                case Function.NS_DM_THAN_HDLD_DS: return 0;
                case Function.NS_DM_LOAI_HDLD_DS: return 0;
                case Function.NS_DM_HTHUC_TLUONG_DS: return 0;
                case Function.NS_DM_LOAI_TNHAP_DS: return 0;
                case Function.NS_DM_LOAI_CPHI_DS: return 0;
                case Function.NS_DM_HTHUC_KTHUONG_DS: return 0;
                case Function.NS_DM_HTHUC_KLUAT_DS: return 0;
                case Function.NS_DM_KHIEU_CCONG_DS: return 0;
                case Function.NS_DM_LOAI_HSO_DS: return 0;
                case Function.NS_DM_DVI_TGIAN_DS: return 0;
                case Function.NS_DM_DVI_CTAC_DS: return 0;
                case Function.NS_DM_LOAI_GTO_DS: return 0;
                case Function.NS_DM_BVIEN_KCB_DS: return 0;
                case Function.NS_DM_NHOM_CTV_DS: return 0;
                case Function.NS_DM_DU_AN_DS: return 0;
                case Function.NS_DM_CHUC_VU_DU_AN_DS: return 0;
                case Function.NS_DM_CHUC_VU_CTV_DS: return 0;
                case Function.NS_DM_PHU_CAP_DS: return 0;
                case Function.NS_DM_DANH_MUC_DS: return 0;

                case Function.NS_HO_SO_CT: return 2;
                case Function.NS_HO_SO_DS: return 2;
                case Function.NS_HOP_DONG_CT: return 1;
                case Function.NS_HOP_DONG_DS: return 1;
                case Function.NS_THUYEN_CHUYEN_CT: return 1;
                case Function.NS_THUYEN_CHUYEN_DS: return 1;
                case Function.NS_THOI_VIEC_CT: return 1;
                case Function.NS_THOI_VIEC_DS: return 1;
                case Function.NS_BANG_LUONG: return 2;
                case Function.NS_LUONG_CT: return 2;
                case Function.NS_LUONG_DS: return 2;
                case Function.NS_LUONG_DCHINH_CT: return 1;
                case Function.NS_LUONG_DCHINH_DS: return 1;
                case Function.NS_TINH_LUONG_CT: return 1;
                case Function.NS_TINH_LUONG_DS: return 1;
                case Function.NS_TIEU_CHI_PHU_CAP_CTV: return 2;
                case Function.NS_TINH_PHU_CAP_CTV_CT: return 1;
                case Function.NS_TINH_PHU_CAP_CTV_DS: return 1;
                case Function.NS_QLY_DU_AN: return 2;

                case Function.NS_LUONG: return 1;
                case Function.NS_CHUC_VU: return 1;
                case Function.NS_KHEN_THUONG: return 1;
                case Function.NS_KY_LUAT: return 1;
                case Function.NS_DAO_TAO: return 1;
                case Function.NS_NGHI_PHEP: return 1;
                case Function.NS_TIEU_CHI: return 1;
                case Function.NS_DANH_SACH: return 1;
                case Function.NS_PHU_CAP: return 1;
                //Khai thác dữ liệu
                case Function.KTDL_MAU_KHAI_THAC_DL: return 1;
                case Function.KTDL_PHAN_HE_CHUNG: return 1;
                case Function.KTDL_KHACH_HANG: return 1;
                case Function.KTDL_KE_TOAN: return 1;
                case Function.KTDL_HUY_DONG: return 1;
                case Function.KTDL_TIN_DUNG_VI_MO: return 1;
                case Function.KTDL_TIN_DUNG: return 1;
                case Function.KTDL_BAO_HIEM: return 1;
                case Function.KTDL_NHAN_SU: return 1;
                case Function.KTDL_SMS_KICH_HOAT: return 1;
                case Function.KTDL_SMS_DONG_BO: return 1;
                case Function.KTDL_SMS_CAU_HINH: return 1;
                case Function.KTDL_SMS_DANG_KY: return 1;
                case Function.KTDL_SMS_TRUY_VAN: return 1;
                case Function.KTDL_SMS_DUYET: return 1;
                case Function.TS_DM_NHOM_TS_CT: return 2;
                case Function.TS_TAI_SAN: return 1;
                case Function.TS_TANG: return 1;
                case Function.TS_TANG_DS: return 1;
                case Function.TS_BAN_GIAO: return 1;
                case Function.TS_BAN_GIAO_DS: return 1;
                case Function.TS_NANG_CAP: return 1;
                case Function.TS_NANG_CAP_DS: return 1;
                case Function.TS_DANH_GIA: return 1;
                case Function.TS_DANH_GIA_DS: return 1;
                case Function.TS_GIAM: return 1;
                case Function.TS_GIAM_DS: return 1;
                case Function.TS_KHAU_HAO: return 1;
                case Function.TS_KHAU_HAO_DS: return 1;
                //Tài sản đảm bảo
                case Function.TD_TSDB_LOAI_CT: return 1;
                case Function.TD_TSDB_LOAI_DS: return 1;
                case Function.TD_TSDB_CT: return 1;
                case Function.TD_TSDB_DS: return 1;
                case Function.TD_TSDB_LOAI: return 1;

                //Hợp đồng thế chấp
                case Function.TSDB_HOP_DONG_THE_CHAP: return 1;
                case Function.TSDB_HOP_DONG_THE_CHAP_NHAP: return 1;
                case Function.TSDB_HOP_DONG_THE_CHAP_XUAT: return 1;
                //Hạn mức
                case Function.HM_TONG: return 1;
                case Function.HM_CTIET_CT: return 2;
                case Function.HM_CTIET_DS: return 2;

                //Tín dụng thông thường
                case Function.TD_SAN_PHAMTT: return 1;
                case Function.TD_SAN_PHAMTT_DS: return 1;
                case Function.TD_HDTD: return 1;
                case Function.TD_HDTD_DS: return 1;
                case Function.TD_KUOC: return 1;
                case Function.TD_KUOC_DS: return 1;
                case Function.TD_GIAI_NGAN: return 1;

                //Tín dụng tiêu dùng
                case Function.TDTD_SAN_PHAM: return 1;
                case Function.TDTD_DON_XIN_VAY_VON: return 1;
                case Function.TDTD_HOP_DONG_CA_NHAN: return 1;
                case Function.TDTD_KHE_UOC: return 1;
                case Function.TDTD_GIAI_NGAN: return 1;
                case Function.TDTD_GIAI_NGAN_DAI_LY: return 1;
                case Function.TDTD_THU_GOC_LAI: return 1;
                case Function.TDTD_KIEM_SOAT_RR: return 1;
                case Function.TDTD_TRICH_LAP_DP: return 1;
                case Function.TDTD_CHUYEN_HOAN: return 1;
                case Function.TDTD_CHUYEN_NO: return 1;
                case Function.TDTD_DU_THU: return 1;
                case Function.TDTD_TSDB_CT: return 1;
                case Function.TDTD_TSDB_DS: return 1;
                case Function.TDTD_HDTC_CT: return 1;
                case Function.TDTD_HDTC_DS: return 1;

                case Function.SYS_JOB_EMAIL: return 0;
                case Function.SYS_JOB_SUBSCRIBE: return 0;
                case Function.SYS_JOB_HIS: return 0;

                case Function.SMS_DANG_KY_DTUONG: return 2;
                case Function.SMS_DANG_KY_DVU: return 0;
                case Function.SMS_DANG_KY_LOAI_DTUONG: return 0;
                case Function.SMS_DANG_KY_TNHAN_CDONG: return 1;
                case Function.SMS_DSACH_TNHAN_DEN: return 0;
                case Function.SMS_DSACH_TNHAN_DI: return 0;
                case Function.SMS_DANG_KY_SO_DIEN_THOAI: return 0;
                case Function.SMS_QUAN_LY_KET_NOI: return 0;

                default: return 1;
            }
        }

        public static string layNgonNguTieuDeForm(this Function function)
        {
            switch (function)
            {
                //Hệ thống
                case Function.HT_LOGIN: return LLanguage.SearchResourceByKey("HT_LOGIN");
                case Function.HT_LOGOUT: return LLanguage.SearchResourceByKey("HT_LOGOUT");
                case Function.HT_RESET_PASS: return LLanguage.SearchResourceByKey("ACTION.Resetpass");
                case Function.HT_NHNSD: return LLanguage.SearchResourceByKey("MENU.111_NHNSD_CT");
                case Function.HT_NSD: return LLanguage.SearchResourceByKey("MENU.112_NSD_CT");
                case Function.HT_NHNSD_NSD: return LLanguage.SearchResourceByKey("HT_NHNSD_NSD");
                case Function.HT_NSD_HIEN_THOI: return LLanguage.SearchResourceByKey("MENU.114_NSD_HIEN_THOI");
                case Function.HT_PQ_CHUC_NANG: return LLanguage.SearchResourceByKey("MENU.121_PQ_CHUC_NANG");
                case Function.HT_PQ_PHAM_VI: return LLanguage.SearchResourceByKey("MENU.122_PQ_PHAM_VI");
                case Function.HT_THAM_SO: return LLanguage.SearchResourceByKey("MENU.131_THAM_SO_CT");
                case Function.HT_SAO_LUU: return LLanguage.SearchResourceByKey("MENU.141_SAO_LUU");
                case Function.HT_PHUC_HOI: return LLanguage.SearchResourceByKey("MENU.142_PHUC_HOI");
                case Function.HT_KIEM_TRA_PB: return LLanguage.SearchResourceByKey("MENU.151_KIEM_TRA_PB");
                case Function.HT_CAP_NHAT_PB: return LLanguage.SearchResourceByKey("MENU.152_CAP_NHAT_PB");
                case Function.HT_DANG_NHAP: return LLanguage.SearchResourceByKey("HT_DANG_NHAP");
                //Dùng chung
                case Function.DC_DM_TINH_THANH: return LLanguage.SearchResourceByKey("MENU.2111_TINH_THANH_CT");
                case Function.DC_DM_DIA_BAN: return LLanguage.SearchResourceByKey("MENU.2121_DIA_BAN_CT");
                case Function.DC_DM_DON_VI: return LLanguage.SearchResourceByKey("MENU.2131_DON_VI_CT");
                case Function.DC_DM_KHU_VUC: return LLanguage.SearchResourceByKey("MENU.2141_KHU_VUC_CT");
                case Function.DC_DM_CUM: return LLanguage.SearchResourceByKey("MENU.2151_CUM_CT");
                case Function.DC_DM_NHOM: return LLanguage.SearchResourceByKey("MENU.2161_NHOM_CT");
                case Function.DC_DM_DUNG_CHUNG: return LLanguage.SearchResourceByKey("MENU.2171_DUNG_CHUNG_CT");
                case Function.DC_DM_PHAN_HE_GD: return LLanguage.SearchResourceByKey("MENU.2181_PHAN_HE_CT");
                case Function.DC_DM_QUOC_GIA: return LLanguage.SearchResourceByKey("MENU.2191_QUOC_GIA_CT");
                case Function.DC_DM_TIEN_TE: return LLanguage.SearchResourceByKey("MENU.21101_TIEN_TE_CT");
                case Function.DC_DM_LICH_HOP: return LLanguage.SearchResourceByKey("MENU.21101_TIEN_TE_CT");
                case Function.DC_LAI_SUAT_CT: return LLanguage.SearchResourceByKey("MENU.2211_LAI_SUAT_CT");
                case Function.DC_LAI_SUAT_DS: return LLanguage.SearchResourceByKey("MENU.2212_LAI_SUAT_DS");
                case Function.DC_PHI: return LLanguage.SearchResourceByKey("MENU.2311_PHI_CT");
                case Function.DC_LOAI_TY_GIA: return LLanguage.SearchResourceByKey("MENU.241_LOAI_TY_GIA");
                case Function.DC_TY_GIA: return LLanguage.SearchResourceByKey("MENU.2421_TY_GIA_CT");
                case Function.DC_HAN_MUC: return LLanguage.SearchResourceByKey("MENU.2511_HAN_MUC_CT");
                case Function.DC_DM_LOAI_DTUONG: return LLanguage.SearchResourceByKey("MENU.2811_LOAI_DTUONG_CT");
                case Function.DC_DM_DTUONG: return LLanguage.SearchResourceByKey("MENU.2812_DTUONG_CT");
                case Function.DC_DM_TCTD: return LLanguage.SearchResourceByKey("MENU.2811_TO_CHUC_TIN_DUNG");
                case Function.DC_DM_TCTD_DS: return LLanguage.SearchResourceByKey("MENU.2812_TO_CHUC_TIN_DUNG_DS");
                //Khách hàng
                case Function.KH_NHOM: return LLanguage.SearchResourceByKey("MENU.3111_NHOM_KH_CT");
                case Function.KH_THANH_VIEN: return LLanguage.SearchResourceByKey("MENU.3141_KH_THANH_VIEN_CT");
                case Function.KH_CA_NHAN: return LLanguage.SearchResourceByKey("MENU.3142_KH_CA_NHAN_CT");
                case Function.KH_TO_CHUC: return LLanguage.SearchResourceByKey("MENU.3143_KH_TO_CHUC_CT");
                case Function.KH_DANH_SACH: return LLanguage.SearchResourceByKey("MENU.3144_KHACH_HANG_DS");
                case Function.KH_CHUYEN_DIA_BAN: return LLanguage.SearchResourceByKey("MENU.3151_CHUYEN_DIA_BAN_CT");
                case Function.KH_CAP_NHAT_KH: return LLanguage.SearchResourceByKey("MENU.3161_CAP_NHAT_KH_CT");
                case Function.KH_XEP_HANG_TD: return LLanguage.SearchResourceByKey("MENU.3311_XEP_HANG_TD_CT");
                case Function.KH_XEP_HANG_NGHEO: return LLanguage.SearchResourceByKey("MENU.3321_XEP_HANG_NGHEO_CT");
                //Kế toán
                case Function.KT_PHAN_LOAI_CT: return LLanguage.SearchResourceByKey("MENU.4111_PHAN_LOAI_CT");
                case Function.KT_PHAN_LOAI_DS: return LLanguage.SearchResourceByKey("MENU.4111_PHAN_LOAI_CT");
                case Function.KT_BUT_TOAN_CT: return LLanguage.SearchResourceByKey("MENU.4151_BUT_TOAN_CT");
                case Function.KT_BUT_TOAN_DS: return LLanguage.SearchResourceByKey("MENU.4151_BUT_TOAN_CT");
                case Function.KT_CAN_DOI_CT: return LLanguage.SearchResourceByKey("MENU.4131_CAN_DOI_CT");
                case Function.KT_CAN_DOI_DS: return LLanguage.SearchResourceByKey("MENU.4131_CAN_DOI_CT");
                case Function.KT_TAI_KHOAN_CT: return LLanguage.SearchResourceByKey("MENU.4141_TAI_KHOAN_CT");
                case Function.KT_TAI_KHOAN_DS: return LLanguage.SearchResourceByKey("MENU.4141_TAI_KHOAN_CT");
                case Function.KT_TONG_HOP_CT: return LLanguage.SearchResourceByKey("MENU.441_TONG_HOP_KT");
                case Function.KT_TONG_HOP_DS: return LLanguage.SearchResourceByKey("MENU.441_TONG_HOP_KT");
                case Function.KT_PHIEU_THU: return LLanguage.SearchResourceByKey("MENU.4221_PHIEU_THU_CT");
                case Function.KT_PHIEU_CHI: return LLanguage.SearchResourceByKey("MENU.4231_PHIEU_CHI_CT");
                case Function.KT_PHIEU_KE_TOAN: return LLanguage.SearchResourceByKey("MENU.4241_PHIEU_KE_TOAN_CT");
                case Function.KT_NGOAI_BANG: return LLanguage.SearchResourceByKey("MENU.4251_NGOAI_BANG_CT");
                case Function.KT_DIEU_CHINH: return LLanguage.SearchResourceByKey("MENU.4261_DIEU_CHINH_CT");
                case Function.KT_KET_CHUYEN: return LLanguage.SearchResourceByKey("MENU.4311_KET_CHUYEN_CT");
                case Function.KT_GIAO_DICH: return LLanguage.SearchResourceByKey("KT_GIAO_DICH");
                case Function.KT_PHIEU_UY_NHIEM_CHI: return LLanguage.SearchResourceByKey("KT_PHIEU_UY_NHIEM_CHI");
                case Function.KT_HE_THONG_TKTH: return LLanguage.SearchResourceByKey("MENU.KT_HE_THONG_TKTH");
                case Function.KT_TAI_KHOAN_TH: return LLanguage.SearchResourceByKey("MENU.KT_TAI_KHOAN_TH");
                //Huy động vốn
                case Function.HDV_SAN_PHAM: return LLanguage.SearchResourceByKey("MENU.5111_SAN_PHAM_HDV_CT");
                case Function.HDV_DANH_SACH_SO: return LLanguage.SearchResourceByKey("HDV_DANH_SACH_SO");
                case Function.HDV_SO_TKQD: return LLanguage.SearchResourceByKey("MENU.5211_DK_TKQD");
                case Function.HDV_SO_TKKKH: return LLanguage.SearchResourceByKey("MENU.5212_MO_SO_TKKKH");
                case Function.HDV_SO_TGCKH: return LLanguage.SearchResourceByKey("MENU.5213_MO_SO_TGCKH");
                case Function.HDV_SO_TK_TGTT: return LLanguage.SearchResourceByKey("MENU.5214_MO_TK_TGTT");
                case Function.HDV_SO_TKCKH: return LLanguage.SearchResourceByKey("MENU.5216_MO_SO_TKCKH");
                case Function.HDV_GUI_THEM_TIEN_THEO_SO: return LLanguage.SearchResourceByKey("MENU.5221_GUI_THEM_TIEN_CT");
                case Function.HDV_GUI_THEM_TIEN_THEO_DANH_SACH: return LLanguage.SearchResourceByKey("MENU.5222_GUI_THEM_TIEN_DS_CT");
                case Function.HDV_RUT_BOT_GOC: return LLanguage.SearchResourceByKey("MENU.5231_RUT_BOT_GOC_CT");
                case Function.HDV_RUT_GOC_THEO_DANH_SACH: return LLanguage.SearchResourceByKey("MENU.5232_RUT_BOT_GOC_DS_CT");
                case Function.HDV_TAT_TOAN: return LLanguage.SearchResourceByKey("MENU.5241_TAT_TOAN_CT");
                case Function.HDV_TAT_TOAN_THEO_DANH_SACH: return LLanguage.SearchResourceByKey("MENU.5242_TAT_TOAN_DS");
                case Function.HDV_PHONG_TOA_SD: return LLanguage.SearchResourceByKey("MENU.5251_PHONG_TOA_SD_CT");
                case Function.HDV_GIAI_TOA_SD: return LLanguage.SearchResourceByKey("MENU.5261_GIAI_TOA_SD_CT");
                case Function.HDV_TRA_LAI: return LLanguage.SearchResourceByKey("MENU.5311_TRA_LAI_CT");
                case Function.HDV_TRA_LAI_THEO_DANH_SACH: return LLanguage.SearchResourceByKey("MENU.5312_TRA_LAI_DS");
                case Function.HDV_DU_CHI: return LLanguage.SearchResourceByKey("MENU.5321_DU_CHI_CT");
                case Function.HDV_DAT_LICH_DU_CHI: return LLanguage.SearchResourceByKey("MENU.533_DU_CHI_LICH");
                case Function.HDV_LAI_NHAP_GOC_THEO_SO: return LLanguage.SearchResourceByKey("MENU.5341_LNG_TUNG_SO_CT");
                case Function.HDV_LAI_NHAP_GOC_THEO_DANH_SACH: return LLanguage.SearchResourceByKey("MENU.5343_LAI_NHAP_GOC_DS");
                case Function.HDV_PHAN_BO: return LLanguage.SearchResourceByKey("MENU.5351_PHAN_BO_CT");
                case Function.HDV_DAT_LICH_PHAN_BO: return LLanguage.SearchResourceByKey("MENU.536_PHAN_BO_LICH");
                case Function.HDV_DAT_LICH_LAI_NHAP_GOC: return LLanguage.SearchResourceByKey("MENU.537_LAI_NHAP_GOC_LICH");
                case Function.HDV_DONG_TK: return LLanguage.SearchResourceByKey("MENU.5411_DONG_TK_CT");
                case Function.HDV_MO_LAI_TK: return LLanguage.SearchResourceByKey("MENU.5421_MO_LAI_TK_CT");
                case Function.HDV_DIEU_CHINH_LS: return LLanguage.SearchResourceByKey("MENU.5511_DIEU_CHINH_LS_CT");
                //Tài sản và hợp đồng thế chấp - dùng chung cho tín dụng
                case Function.TD_CHI_TIET_LOAI_TAI_SAN: return LLanguage.SearchResourceByKey("TD_CHI_TIET_LOAI_TAI_SAN");
                case Function.TD_DANH_SACH_LOAI_TAI_SAN: return LLanguage.SearchResourceByKey("TD_DANH_SACH_LOAI_TAI_SAN");
                case Function.TD_CHI_TIET_TAI_SAN_DAM_BAO: return LLanguage.SearchResourceByKey("MENU.6211_TSDB_CT");
                case Function.TD_DANH_SACH_TAI_SAN_DAM_BAO: return LLanguage.SearchResourceByKey("TD_DANH_SACH_TAI_SAN_DAM_BAO");
                case Function.TD_CHI_TIET_HOP_DONG_THE_CHAP: return LLanguage.SearchResourceByKey("MENU.6221_HDCC_TC_BL_CT");
                case Function.TD_DANH_SACH_HOP_DONG_THE_CHAP: return LLanguage.SearchResourceByKey("TD_DANH_SACH_HOP_DONG_THE_CHAP");
                //Tín dụng vi mô
                case Function.TDVM_SAN_PHAM_TDVM: return LLanguage.SearchResourceByKey("MENU.6111_SAN_PHAM_TDVM_CT");
                case Function.TDVM_VONG_VAY: return LLanguage.SearchResourceByKey("MENU.6121_VONG_VAY_CT");
                case Function.TDVM_HAN_MUC: return LLanguage.SearchResourceByKey("MENU.6131_HAN_MUC_CT");
                case Function.TDVM_TSDB: return LLanguage.SearchResourceByKey("MENU.6211_TSDB_CT");
                case Function.TDVM_THE_CHAP: return LLanguage.SearchResourceByKey("MENU.6221_HDCC_TC_BL_CT");
                case Function.TDVM_DANH_SACH_HOP_DONG: return LLanguage.SearchResourceByKey("MENU.6412_HOP_DONG_DS");
                case Function.TDVM_CHI_TIET_HOP_DONG: return LLanguage.SearchResourceByKey("MENU.6411_HOP_DONG_CT");
                case Function.TDVM_DANH_SACH_KHE_UOC: return LLanguage.SearchResourceByKey("MENU.6442_KHE_UOC_DS");
                case Function.TDVM_DANH_SACH_KHE_UOC_01: return LLanguage.SearchResourceByKey("MENU.6442_KHE_UOC_DS");
                case Function.TDVM_CHI_TIET_KHE_UOC: return LLanguage.SearchResourceByKey("MENU.6441_KHE_UOC_CT");
                case Function.TDVM_THOA_THUAN: return LLanguage.SearchResourceByKey("MENU.6421_THOA_THUAN_CT");
                case Function.TDVM_HOP_DONG_NHOM: return LLanguage.SearchResourceByKey("MENU.6431_HOP_DONG_NHOM_CT");
                case Function.TDVM_DAT_LICH_TRA_NO: return LLanguage.SearchResourceByKey("MENU.6451_LICH_TRA_NO_CT");
                case Function.TDVM_TAM_UNG: return LLanguage.SearchResourceByKey("MENU.6511_TAM_UNG_CT");
                case Function.TDVM_HOAN_UNG: return LLanguage.SearchResourceByKey("MENU.6521_HOAN_UNG_CT");
                case Function.TDVM_GIAI_NGAN: return LLanguage.SearchResourceByKey("MENU.6531_GIAI_NGAN_CT");
                case Function.TDVM_DU_THU: return LLanguage.SearchResourceByKey("MENU.6541_DU_THU_CT");
                case Function.TDVM_DAT_LICH_DU_THU: return LLanguage.SearchResourceByKey("MENU.6543_DU_THU_LICH");
                case Function.TDVM_TRICH_LAP_DU_PHONG: return LLanguage.SearchResourceByKey("MENU.6551_TRICH_LAP_DP_CT");
                case Function.TDVM_DAT_LICH_TRICH_LAP_DU_PHONG: return LLanguage.SearchResourceByKey("MENU.6553_TRICH_LAP_DP_LICH");
                case Function.TDVM_DIEU_CHINH_LAI_SUAT: return LLanguage.SearchResourceByKey("MENU.6561_DIEU_CHINH_LS_CT");
                case Function.TDVM_GIA_HAN_NO: return LLanguage.SearchResourceByKey("MENU.6611_GIA_HAN_CT");
                case Function.TDVM_CHUYEN_HOAN_NHOM_NO: return LLanguage.SearchResourceByKey("MENU.6621_CHUYEN_HOAN_CT");
                case Function.TDVM_CHUYEN_NO_QUA_HAN: return LLanguage.SearchResourceByKey("MENU.6631_CHUYEN_NO_CT");
                case Function.TDVM_XU_LY_NO: return LLanguage.SearchResourceByKey("MENU.6641_XOA_NO_CT");
                case Function.TDVM_LAP_HOA_DON_TIEN_KY: return LLanguage.SearchResourceByKey("MENU.6711_HOA_DON_TTK_CT");
                case Function.TDVM_PHAN_BO_LAI_VAY: return LLanguage.SearchResourceByKey("MENU.6721_PHAN_BO_LV_CT");
                case Function.TDVM_DAT_LICH_PHAN_BO_LAI_VAY: return LLanguage.SearchResourceByKey("MENU.6723_PHAN_BO_LV_LICH");
                case Function.TDVM_THU_GOC_LAI_TRUOC_HAN: return LLanguage.SearchResourceByKey("MENU.6731_THU_GOC_LAI_CT");
                case Function.TDVM_DON_XIN_VAY_VON: return LLanguage.SearchResourceByKey("MENU.6461_DON_VAY_VON_CT");
                case Function.TDVM_DIA_BAN_SAN_PHAM: return LLanguage.SearchResourceByKey("MENU.6151_DIA_BAN_SAN_PHAM_CT");
                case Function.TDVM_PHAN_LOAI_NO: return LLanguage.SearchResourceByKey("MENU.665_PHAN_LOAI_NO");
                case Function.TDVM_KIEM_SOAT_RR: return LLanguage.SearchResourceByKey("MENU.TDVM_KIEM_SOAT_RR");
                //Bảo hiểm
                case Function.BH_SAN_PHAM_BHTH: return LLanguage.SearchResourceByKey("BH_SAN_PHAM_BHTH");
                case Function.BH_CAM_KET_THEO_KH: return LLanguage.SearchResourceByKey("BH_CAM_KET_THEO_KH");
                case Function.BH_CAM_KET_THEO_DS: return LLanguage.SearchResourceByKey("BH_CAM_KET_THEO_DS");
                case Function.BH_THU_HO_PHIXL: return LLanguage.SearchResourceByKey("BH_THU_HO_PHIXL");
                case Function.BH_CHI_HO_PHIXL: return LLanguage.SearchResourceByKey("BH_CHI_HO_PHIXL");
                case Function.BH_THU_HO_PHIBH: return LLanguage.SearchResourceByKey("BH_THU_HO_PHIBH");
                case Function.BH_CHI_HO_PHIBH: return LLanguage.SearchResourceByKey("BH_CHI_HO_PHIBH");
                case Function.BH_CHAM_DUT: return LLanguage.SearchResourceByKey("BH_CHAM_DUT");
                //Nhân sự
                case Function.NS_DM_QUOC_TICH_CT: return LLanguage.SearchResourceByKey("MENU.8381_QUOC_TICH_CT");
                case Function.NS_DM_TINH_TP_CT: return LLanguage.SearchResourceByKey("MENU.8401_TINH_TP_CT");
                case Function.NS_DM_QUAN_HUYEN_CT: return LLanguage.SearchResourceByKey("MENU.8371_QUAN_HUYEN_CT");
                case Function.NS_DM_PHUONG_XA_CT: return LLanguage.SearchResourceByKey("MENU.8351_PHUONG_XA_CT");
                case Function.NS_DM_DAN_TOC_CT: return LLanguage.SearchResourceByKey("MENU.8301_DAN_TOC_CT");
                case Function.NS_DM_TON_GIAO_CT: return LLanguage.SearchResourceByKey("MENU.8421_TON_GIAO_CT");
                case Function.NS_DM_GIOI_TINH_CT: return LLanguage.SearchResourceByKey("MENU.8321_GIOI_TINH_CT");
                case Function.NS_DM_TTRANG_HNHAN_CT: return LLanguage.SearchResourceByKey("MENU.8411_TINH_TRANG_HON_NHAN_CT");
                case Function.NS_DM_CU_TRU_CT: return LLanguage.SearchResourceByKey("MENU.8291_CU_TRU_CT");
                case Function.NS_DM_HTHUC_LVIEC_CT: return LLanguage.SearchResourceByKey("MENU.8101_HINH_THUC_LAM_VIEC_CT");
                case Function.NS_DM_TDO_HVAN_CT: return LLanguage.SearchResourceByKey("MENU.8241_TRINH_DO_HOC_VAN_CT");
                case Function.NS_DM_HTHUC_DTAO_CT: return LLanguage.SearchResourceByKey("MENU.8071_HINH_THUC_DAO_TAO_CT");
                case Function.NS_DM_TRUONG_DTAO_CT: return LLanguage.SearchResourceByKey("MENU.8051_CO_SO_DAO_TAO_CT");
                case Function.NS_DM_BANG_CAP_CT: return LLanguage.SearchResourceByKey("MENU.8021_DM_BANG_CAP_CHUNG_CHI_CT");
                case Function.NS_DM_KHOA_DTAO_CT: return LLanguage.SearchResourceByKey("MENU.8141_KHOA_DAO_TAO_CT");
                case Function.NS_DM_CNGANH_DTAO_CT: return LLanguage.SearchResourceByKey("MENU.8041_CHUYEN_NGANH_DAO_TAO_CT");
                case Function.NS_DM_HOC_HAM_CT: return LLanguage.SearchResourceByKey("MENU.8121_HOC_HAM_CT");
                case Function.NS_DM_HOC_VI_CT: return LLanguage.SearchResourceByKey("MENU.8131_HOC_VI_CT");
                case Function.NS_DM_TDO_TANH_CT: return LLanguage.SearchResourceByKey("MENU.8251_TRINH_DO_NGOAI_NGU_CT");
                case Function.NS_DM_TDO_THOC_CT: return LLanguage.SearchResourceByKey("MENU.8261_TRINH_DO_TIN_HOC_CT");
                case Function.NS_DM_TDO_CTRI_CT: return LLanguage.SearchResourceByKey("MENU.8231_TRINH_DO_CHINH_TRI_CT");
                case Function.NS_DM_XEP_LOAI_CT: return LLanguage.SearchResourceByKey("MENU.8271_XEP_LOAI_CT");
                case Function.NS_DM_QHE_GDINH_CT: return LLanguage.SearchResourceByKey("MENU.8361_QUAN_HE_GD_CT");
                case Function.NS_DM_KY_NANG_CT: return LLanguage.SearchResourceByKey("MENU.8161_KY_NANG_CT");
                case Function.NS_DM_NGHE_NGHIEP_CT: return LLanguage.SearchResourceByKey("MENU.8341_NGHE_NGHIEP_CT");
                case Function.NS_DM_CHUC_VU_CT: return LLanguage.SearchResourceByKey("MENU.8031_DM_CHUC_VU_CT");
                case Function.NS_DM_LDO_NPHEP_CT: return LLanguage.SearchResourceByKey("MENU.8211_LY_DO_NGHI_PHEP_CT");
                case Function.NS_DM_LDO_TVIEC_CT: return LLanguage.SearchResourceByKey("MENU.8221_LY_DO_THOI_VIEC_CT");
                case Function.NS_DM_THAN_HDLD_CT: return LLanguage.SearchResourceByKey("MENU.8391_THOI_HAN_HDLD_CT");
                case Function.NS_DM_LOAI_HDLD_CT: return LLanguage.SearchResourceByKey("MENU.8181_LOAI_HDLD_CT");
                case Function.NS_DM_HTHUC_TLUONG_CT: return LLanguage.SearchResourceByKey("MENU.8201_LOAI_THU_NHAP_CT");
                case Function.NS_DM_LOAI_TNHAP_CT: return LLanguage.SearchResourceByKey("MENU.8201_LOAI_THU_NHAP_CT");
                case Function.NS_DM_LOAI_CPHI_CT: return LLanguage.SearchResourceByKey("MENU.8171_LOAI_CHI_PHI_CT");
                case Function.NS_DM_HTHUC_KTHUONG_CT: return LLanguage.SearchResourceByKey("MENU.8081_HINH_THUC_KHEN_THUONG_CT");
                case Function.NS_DM_HTHUC_KLUAT_CT: return LLanguage.SearchResourceByKey("MENU.8091_HINH_THUC_KY_LUAT_CT");
                case Function.NS_DM_KHIEU_CCONG_CT: return LLanguage.SearchResourceByKey("MENU.8151_KY_HIEU_CHAM_CONG_CT");
                case Function.NS_DM_LOAI_HSO_CT: return LLanguage.SearchResourceByKey("MENU.8191_LOAI_HO_SO_CT");
                case Function.NS_DM_DVI_TGIAN_CT: return LLanguage.SearchResourceByKey("MENU.8311_DON_VI_THOI_GIAN_CT");
                case Function.NS_DM_DVI_CTAC_CT: return LLanguage.SearchResourceByKey("MENU.8061_DON_VI_CONG_TAC_CT");
                case Function.NS_DM_LOAI_GTO_CT: return LLanguage.SearchResourceByKey("MENU.8331_LOAI_GIAY_TO_CT");
                case Function.NS_DM_BVIEN_KCB_CT: return LLanguage.SearchResourceByKey("MENU.8281_BENH_VIEN_CT");
                case Function.NS_DM_NHOM_CTV_CT: return LLanguage.SearchResourceByKey("MENU.8441_NHOM_CTV_CT");
                case Function.NS_DM_DU_AN_CT: return LLanguage.SearchResourceByKey("MENU.8431_DU_AN_CT");
                case Function.NS_DM_CHUC_VU_DU_AN_CT: return LLanguage.SearchResourceByKey("MENU.8451_CHUC_VU_DA_CT");
                case Function.NS_DM_CHUC_VU_CTV_CT: return LLanguage.SearchResourceByKey("MENU.8461_CHUC_VU_CTV_CT");
                case Function.NS_DM_PHU_CAP_CT: return LLanguage.SearchResourceByKey("MENU.8471_PHU_CAP_CT");
                case Function.NS_DM_DANH_MUC_CT: return LLanguage.SearchResourceByKey("MENU.8471_DANH_MUC_CT");
                case Function.NS_DM_DANH_MUC_PL: return LLanguage.SearchResourceByKey("MENU.8471_DANH_MUC_PL");

                case Function.NS_DM_QUOC_TICH_DS: return LLanguage.SearchResourceByKey("MENU.8012_DANH_MUC_DS");
                case Function.NS_DM_TINH_TP_DS: return LLanguage.SearchResourceByKey("MENU.8012_DANH_MUC_DS");
                case Function.NS_DM_QUAN_HUYEN_DS: return LLanguage.SearchResourceByKey("MENU.8012_DANH_MUC_DS");
                case Function.NS_DM_PHUONG_XA_DS: return LLanguage.SearchResourceByKey("MENU.8012_DANH_MUC_DS");
                case Function.NS_DM_DAN_TOC_DS: return LLanguage.SearchResourceByKey("MENU.8012_DANH_MUC_DS");
                case Function.NS_DM_TON_GIAO_DS: return LLanguage.SearchResourceByKey("MENU.8012_DANH_MUC_DS");
                case Function.NS_DM_GIOI_TINH_DS: return LLanguage.SearchResourceByKey("MENU.8012_DANH_MUC_DS");
                case Function.NS_DM_TTRANG_HNHAN_DS: return LLanguage.SearchResourceByKey("MENU.8012_DANH_MUC_DS");
                case Function.NS_DM_CU_TRU_DS: return LLanguage.SearchResourceByKey("MENU.8012_DANH_MUC_DS");
                case Function.NS_DM_HTHUC_LVIEC_DS: return LLanguage.SearchResourceByKey("MENU.8012_DANH_MUC_DS");
                case Function.NS_DM_TDO_HVAN_DS: return LLanguage.SearchResourceByKey("MENU.8012_DANH_MUC_DS");
                case Function.NS_DM_HTHUC_DTAO_DS: return LLanguage.SearchResourceByKey("MENU.8012_DANH_MUC_DS");
                case Function.NS_DM_TRUONG_DTAO_DS: return LLanguage.SearchResourceByKey("MENU.8012_DANH_MUC_DS");
                case Function.NS_DM_BANG_CAP_DS: return LLanguage.SearchResourceByKey("MENU.8012_DANH_MUC_DS");
                case Function.NS_DM_KHOA_DTAO_DS: return LLanguage.SearchResourceByKey("MENU.8012_DANH_MUC_DS");
                case Function.NS_DM_CNGANH_DTAO_DS: return LLanguage.SearchResourceByKey("MENU.8012_DANH_MUC_DS");
                case Function.NS_DM_HOC_HAM_DS: return LLanguage.SearchResourceByKey("MENU.8012_DANH_MUC_DS");
                case Function.NS_DM_HOC_VI_DS: return LLanguage.SearchResourceByKey("MENU.8012_DANH_MUC_DS");
                case Function.NS_DM_TDO_TANH_DS: return LLanguage.SearchResourceByKey("MENU.8012_DANH_MUC_DS");
                case Function.NS_DM_TDO_THOC_DS: return LLanguage.SearchResourceByKey("MENU.8012_DANH_MUC_DS");
                case Function.NS_DM_TDO_CTRI_DS: return LLanguage.SearchResourceByKey("MENU.8012_DANH_MUC_DS");
                case Function.NS_DM_XEP_LOAI_DS: return LLanguage.SearchResourceByKey("MENU.8012_DANH_MUC_DS");
                case Function.NS_DM_QHE_GDINH_DS: return LLanguage.SearchResourceByKey("MENU.8012_DANH_MUC_DS");
                case Function.NS_DM_KY_NANG_DS: return LLanguage.SearchResourceByKey("MENU.8012_DANH_MUC_DS");
                case Function.NS_DM_NGHE_NGHIEP_DS: return LLanguage.SearchResourceByKey("MENU.8012_DANH_MUC_DS");
                case Function.NS_DM_CHUC_VU_DS: return LLanguage.SearchResourceByKey("MENU.8012_DANH_MUC_DS");
                case Function.NS_DM_LDO_NPHEP_DS: return LLanguage.SearchResourceByKey("MENU.8012_DANH_MUC_DS");
                case Function.NS_DM_LDO_TVIEC_DS: return LLanguage.SearchResourceByKey("MENU.8012_DANH_MUC_DS");
                case Function.NS_DM_THAN_HDLD_DS: return LLanguage.SearchResourceByKey("MENU.8012_DANH_MUC_DS");
                case Function.NS_DM_LOAI_HDLD_DS: return LLanguage.SearchResourceByKey("MENU.8012_DANH_MUC_DS");
                case Function.NS_DM_HTHUC_TLUONG_DS: return LLanguage.SearchResourceByKey("MENU.8012_DANH_MUC_DS");
                case Function.NS_DM_LOAI_TNHAP_DS: return LLanguage.SearchResourceByKey("MENU.8012_DANH_MUC_DS");
                case Function.NS_DM_LOAI_CPHI_DS: return LLanguage.SearchResourceByKey("MENU.8012_DANH_MUC_DS");
                case Function.NS_DM_HTHUC_KTHUONG_DS: return LLanguage.SearchResourceByKey("MENU.8012_DANH_MUC_DS");
                case Function.NS_DM_HTHUC_KLUAT_DS: return LLanguage.SearchResourceByKey("MENU.8012_DANH_MUC_DS");
                case Function.NS_DM_KHIEU_CCONG_DS: return LLanguage.SearchResourceByKey("MENU.8012_DANH_MUC_DS");
                case Function.NS_DM_LOAI_HSO_DS: return LLanguage.SearchResourceByKey("MENU.8012_DANH_MUC_DS");
                case Function.NS_DM_DVI_TGIAN_DS: return LLanguage.SearchResourceByKey("MENU.8012_DANH_MUC_DS");
                case Function.NS_DM_DVI_CTAC_DS: return LLanguage.SearchResourceByKey("MENU.8012_DANH_MUC_DS");
                case Function.NS_DM_LOAI_GTO_DS: return LLanguage.SearchResourceByKey("MENU.8012_DANH_MUC_DS");
                case Function.NS_DM_BVIEN_KCB_DS: return LLanguage.SearchResourceByKey("MENU.8012_DANH_MUC_DS");
                case Function.NS_DM_NHOM_CTV_DS: return LLanguage.SearchResourceByKey("MENU.8012_DANH_MUC_DS");
                case Function.NS_DM_DU_AN_DS: return LLanguage.SearchResourceByKey("MENU.8012_DANH_MUC_DS");
                case Function.NS_DM_CHUC_VU_DU_AN_DS: return LLanguage.SearchResourceByKey("MENU.8012_DANH_MUC_DS");
                case Function.NS_DM_CHUC_VU_CTV_DS: return LLanguage.SearchResourceByKey("MENU.8012_DANH_MUC_DS");
                case Function.NS_DM_PHU_CAP_DS: return LLanguage.SearchResourceByKey("MENU.8012_DANH_MUC_DS");
                case Function.NS_DM_DANH_MUC_DS: return LLanguage.SearchResourceByKey("MENU.8012_DANH_MUC_DS");

                case Function.NS_HO_SO_CT: return LLanguage.SearchResourceByKey("MENU.8111_HO_SO_CT");
                case Function.NS_HO_SO_DS: return LLanguage.SearchResourceByKey("MENU.8112_HO_SO_DS");
                case Function.NS_HOP_DONG_CT: return LLanguage.SearchResourceByKey("MENU.8121_HOP_DONG_CT");
                case Function.NS_HOP_DONG_DS: return LLanguage.SearchResourceByKey("MENU.8122_HOP_DONG_DS");
                case Function.NS_THUYEN_CHUYEN_CT: return LLanguage.SearchResourceByKey("MENU.8211_THUYEN_CHUYEN_CT");
                case Function.NS_THUYEN_CHUYEN_DS: return LLanguage.SearchResourceByKey("MENU.8212_THUYEN_CHUYEN_DS");
                case Function.NS_THOI_VIEC_CT: return LLanguage.SearchResourceByKey("MENU.8281_THOI_VIEC_CT");
                case Function.NS_THOI_VIEC_DS: return LLanguage.SearchResourceByKey("MENU.8282_THOI_VIEC_DS");
                case Function.NS_BANG_LUONG: return LLanguage.SearchResourceByKey("MENU.844_BANG_LUONG");
                case Function.NS_LUONG_CT: return LLanguage.SearchResourceByKey("MENU.842_LUONG");
                case Function.NS_LUONG_DS: return LLanguage.SearchResourceByKey("MENU.842_LUONG");
                case Function.NS_LUONG_DCHINH_CT: return LLanguage.SearchResourceByKey("MENU.848_DIEU_CHINH_LUONG");
                case Function.NS_LUONG_DCHINH_DS: return LLanguage.SearchResourceByKey("MENU.848_DIEU_CHINH_LUONG");
                case Function.NS_TINH_LUONG_CT: return LLanguage.SearchResourceByKey("MENU.845_TINH_LUONG");
                case Function.NS_TINH_LUONG_DS: return LLanguage.SearchResourceByKey("MENU.845_TINH_LUONG");
                case Function.NS_TIEU_CHI_PHU_CAP_CTV: return LLanguage.SearchResourceByKey("MENU.846_BANG_PHU_CAP_CTV");
                case Function.NS_TINH_PHU_CAP_CTV_CT: return LLanguage.SearchResourceByKey("MENU.847_TINH_PHU_CAP_CTV");
                case Function.NS_TINH_PHU_CAP_CTV_DS: return LLanguage.SearchResourceByKey("MENU.847_TINH_PHU_CAP_CTV");
                case Function.NS_QLY_DU_AN: return LLanguage.SearchResourceByKey("NS_QLY_DU_AN");

                case Function.NS_LUONG: return LLanguage.SearchResourceByKey("NS_LUONG");
                case Function.NS_CHUC_VU: return LLanguage.SearchResourceByKey("NS_CHUC_VU");
                case Function.NS_KHEN_THUONG: return LLanguage.SearchResourceByKey("NS_KHEN_THUONG");
                case Function.NS_KY_LUAT: return LLanguage.SearchResourceByKey("NS_KY_LUAT");
                case Function.NS_DAO_TAO: return LLanguage.SearchResourceByKey("NS_DAO_TAO");
                case Function.NS_NGHI_PHEP: return LLanguage.SearchResourceByKey("NS_NGHI_PHEP");
                case Function.NS_TIEU_CHI: return LLanguage.SearchResourceByKey("NS_TIEU_CHI");
                case Function.NS_DANH_SACH: return LLanguage.SearchResourceByKey("NS_DANH_SACH");
                case Function.NS_PHU_CAP: return LLanguage.SearchResourceByKey("NS_PHU_CAP");
                //Khai thác dữ liệu
                case Function.KTDL_MAU_KHAI_THAC_DL: return LLanguage.SearchResourceByKey("KTDL_MAU_KHAI_THAC_DL");
                case Function.KTDL_PHAN_HE_CHUNG: return LLanguage.SearchResourceByKey("KTDL_PHAN_HE_CHUNG");
                case Function.KTDL_KHACH_HANG: return LLanguage.SearchResourceByKey("KTDL_KHACH_HANG");
                case Function.KTDL_KE_TOAN: return LLanguage.SearchResourceByKey("KTDL_KE_TOAN");
                case Function.KTDL_HUY_DONG: return LLanguage.SearchResourceByKey("KTDL_HUY_DONG");
                case Function.KTDL_TIN_DUNG_VI_MO: return LLanguage.SearchResourceByKey("KTDL_TIN_DUNG_VI_MO");
                case Function.KTDL_TIN_DUNG: return LLanguage.SearchResourceByKey("KTDL_TIN_DUNG");
                case Function.KTDL_BAO_HIEM: return LLanguage.SearchResourceByKey("KTDL_BAO_HIEM");
                case Function.KTDL_NHAN_SU: return LLanguage.SearchResourceByKey("KTDL_NHAN_SU");
                case Function.KTDL_SMS_KICH_HOAT: return LLanguage.SearchResourceByKey("KTDL_SMS_KICH_HOAT");
                case Function.KTDL_SMS_DONG_BO: return LLanguage.SearchResourceByKey("KTDL_SMS_DONG_BO");
                case Function.KTDL_SMS_CAU_HINH: return LLanguage.SearchResourceByKey("KTDL_SMS_CAU_HINH");
                case Function.KTDL_SMS_DANG_KY: return LLanguage.SearchResourceByKey("KTDL_SMS_DANG_KY");
                case Function.KTDL_SMS_TRUY_VAN: return LLanguage.SearchResourceByKey("KTDL_SMS_TRUY_VAN");
                case Function.KTDL_SMS_DUYET: return LLanguage.SearchResourceByKey("KTDL_SMS_DUYET");
                case Function.TS_DM_NHOM_TS_CT: return LLanguage.SearchResourceByKey("MENU.TS_NHOM_TS");

                case Function.TS_TAI_SAN: return LLanguage.SearchResourceByKey("MENU.TS_TAI_SAN");
                case Function.TS_TANG: return LLanguage.SearchResourceByKey("MENU.TS_TANG");
                case Function.TS_TANG_DS: return LLanguage.SearchResourceByKey("MENU.TS_TANG_DS");
                case Function.TS_TANG_NG: return LLanguage.SearchResourceByKey("MENU.TS_TANG_NG");
                case Function.TS_BAN_GIAO: return LLanguage.SearchResourceByKey("MENU.TS_BAN_GIAO");
                case Function.TS_BAN_GIAO_DS: return LLanguage.SearchResourceByKey("MENU.TS_BAN_GIAO_DS");
                case Function.TS_NANG_CAP: return LLanguage.SearchResourceByKey("MENU.TS_SUA_CHUA");
                case Function.TS_NANG_CAP_DS: return LLanguage.SearchResourceByKey("MENU.TS_SUA_CHUA_DS");
                case Function.TS_DANH_GIA: return LLanguage.SearchResourceByKey("MENU.TS_DANH_GIA_LAI");
                case Function.TS_DANH_GIA_DS: return LLanguage.SearchResourceByKey("MENU.TS_DANH_GIA_LAI_DS");
                case Function.TS_GIAM: return LLanguage.SearchResourceByKey("MENU.TS_GIAM");
                case Function.TS_GIAM_DS: return LLanguage.SearchResourceByKey("MENU.TS_GIAM_DS");
                case Function.TS_KHAU_HAO: return LLanguage.SearchResourceByKey("MENU.TS_KHAU_HAO");
                case Function.TS_KHAU_HAO_DS: return LLanguage.SearchResourceByKey("MENU.TS_KHAU_HAO_DS");
                //Tài sản đảm bảo
                case Function.TD_TSDB_LOAI_CT: return LLanguage.SearchResourceByKey("MENU.TSDB_LOAI_CT");
                case Function.TD_TSDB_LOAI_DS: return LLanguage.SearchResourceByKey("MENU.TSDB_LOAI_DS");
                case Function.TD_TSDB_CT: return LLanguage.SearchResourceByKey("MENU.TSDB_TAI_SAN_DAM_BAO_CT");
                case Function.TD_TSDB_DS: return LLanguage.SearchResourceByKey("MENU.TSDB_TAI_SAN_DAM_BAO_DS");

                //Hạn mức
                case Function.HM_TONG: return LLanguage.SearchResourceByKey("MENU.HM_HAN_MUC_TONG");
                case Function.HM_CTIET_CT: return LLanguage.SearchResourceByKey("MENU.HM_HAN_MUC_CHI_TIET");
                case Function.HM_CTIET_DS: return LLanguage.SearchResourceByKey("MENU.HM_HAN_MUC_DANH_SACH");

                //Tin dung thong thuong
                case Function.TD_SAN_PHAMTT: return LLanguage.SearchResourceByKey("MENU.TDTT_SAN_PHAM_CT");
                case Function.TD_SAN_PHAMTT_DS: return LLanguage.SearchResourceByKey("MENU.TDTT_SAN_PHAM_CT");
                case Function.TD_HDTD: return LLanguage.SearchResourceByKey("MENU.TD_HDTD");
                case Function.TD_HDTD_DS: return LLanguage.SearchResourceByKey("MENU.TD_HDTD_DS");
                case Function.TD_KUOC: return LLanguage.SearchResourceByKey("MENU.TD_KUOC");
                case Function.TD_KUOC_DS: return LLanguage.SearchResourceByKey("MENU.TD_KUOC_DS");
                case Function.TD_GIAI_NGAN: return LLanguage.SearchResourceByKey("MENU.TD_GIAI_NGAN");
                

                case Function.SYS_JOB_EMAIL: return LLanguage.SearchResourceByKey("MENU.SYS_JOB_EMAIL");
                case Function.SYS_JOB_SUBSCRIBE: return LLanguage.SearchResourceByKey("MENU.SYS_JOB_SUBSCRIBE");
                case Function.SYS_JOB_HIS: return LLanguage.SearchResourceByKey("MENU.SYS_JOB_HIS");

                //Tín dụng tiêu dùng
                case Function.TDTD_SAN_PHAM: return LLanguage.SearchResourceByKey("MENU.TDTD_SAN_PHAM");
                case Function.TDTD_DON_XIN_VAY_VON: return LLanguage.SearchResourceByKey("MENU.TDTD_DON_VAY_VON");
                case Function.TDTD_HOP_DONG_CA_NHAN: return LLanguage.SearchResourceByKey("MENU.TDTD_HOP_DONG");
                case Function.TDTD_KHE_UOC: return LLanguage.SearchResourceByKey("MENU.TDTD_KHE_UOC");
                case Function.TDTD_GIAI_NGAN: return LLanguage.SearchResourceByKey("MENU.TDTD_GIAI_NGAN");
                case Function.TDTD_GIAI_NGAN_DAI_LY: return LLanguage.SearchResourceByKey("MENU.TDTD_GIAI_NGAN_DAI_LY");
                case Function.TDTD_THU_GOC_LAI: return LLanguage.SearchResourceByKey("MENU.TDTD_THU_GOC_LAI");
                case Function.TDTD_KIEM_SOAT_RR: return LLanguage.SearchResourceByKey("MENU.TDTD_KIEM_SOAT_RR");
                case Function.TDTD_DU_THU: return LLanguage.SearchResourceByKey("MENU.TDTD_DU_THU");
                case Function.TDTD_CHUYEN_HOAN: return LLanguage.SearchResourceByKey("MENU.TDTD_CHUYEN_HOAN");
                case Function.TDTD_CHUYEN_NO: return LLanguage.SearchResourceByKey("MENU.TDTD_CHUYEN_NO");
                case Function.TDTD_TRICH_LAP_DP: return LLanguage.SearchResourceByKey("MENU.TDTD_TRICH_LAP_DP");
                case Function.TDTD_TSDB_CT: return LLanguage.SearchResourceByKey("MENU.TDTD_TSDB_CT");
                case Function.TDTD_TSDB_DS: return LLanguage.SearchResourceByKey("MENU.TDTD_TSDB_DS");
                case Function.TDTD_HDTC_CT: return LLanguage.SearchResourceByKey("MENU.TDTD_HDTC_CT");
                case Function.TDTD_HDTC_DS: return LLanguage.SearchResourceByKey("MENU.TDTD_HDTC_DS");

                case Function.SMS_DANG_KY_DTUONG: return LLanguage.SearchResourceByKey("MENU.SMS_DANG_KY_DTUONG");
                case Function.SMS_DANG_KY_DVU: return LLanguage.SearchResourceByKey("MENU.SMS_DANG_KY_DVU");
                case Function.SMS_DANG_KY_LOAI_DTUONG: return LLanguage.SearchResourceByKey("MENU.SMS_DANG_KY_LOAI_DTUONG");
                case Function.SMS_DANG_KY_TNHAN_CDONG: return LLanguage.SearchResourceByKey("MENU.SMS_DANG_KY_TNHAN_CDONG");
                case Function.SMS_DSACH_TNHAN_DEN: return LLanguage.SearchResourceByKey("MENU.SMS_DSACH_TNHAN_DEN");
                case Function.SMS_DSACH_TNHAN_DI: return LLanguage.SearchResourceByKey("MENU.SMS_DSACH_TNHAN_DI");
                case Function.SMS_DANG_KY_SO_DIEN_THOAI: return LLanguage.SearchResourceByKey("MENU.SMS_DANG_KY_SO_DIEN_THOAI");
                case Function.SMS_QUAN_LY_KET_NOI: return LLanguage.SearchResourceByKey("MENU.SMS_QUAN_LY_KET_NOI");

                default: return "";
            }
        }

        public enum Table
        {
            BC_MAU,
            BC_MAU_KHAI_THAC,
            BC_MAU_THAM_SO,
            BC_LOAITK,
            BC_MATK,
            BC_MATK_MAPPING,
            BC_THAM_SO,
            BL_SAN_PHAM,
            BL_SAN_PHAM_CT,
            BL_TDOI_LSUAT,
            BL_TDOI_LSUAT_CT,
            BL_TIEN_GUI,
            BL_TIEN_GUI_DCSH,
            BL_TIEN_GUI_KHOACH,
            BL_TIEN_GUI_KHOACH_LSU,
            BL_TIEN_GUI_LSU,
            BL_TIEN_GUI_TKHOAN,
            BL_TTINH,
            BL_TTINH_GTRI,
            BL_TTINH_GTRI_LSU,
            BL_DKY_RGOC,
            BL_DKY_RGOC_CT,
            CD_NGUON_VON,
            CD_NSD,
            DC_BPHI,
            DC_BPHI_CTIET,
            DC_BPHI_GDICH,
            DC_CSO_TLAI,
            DC_CSO_TLAI_CTIET,
            DC_CT_CTIEU,
            DC_CT_DLIEU,
            DC_CT_DLIEU_DCHIEU,
            DC_CT_GOC,
            DC_CT_GOC_CTHUC,
            DC_CT_GOC_MATKE,
            DC_CT_LOAITKE,
            DC_CT_MATKE,
            DC_CT_MATKE_MANB,
            DC_CT_MAUNL,
            DC_CT_MAUNL_CTIEU,
            DC_CT_MBIEU,
            DC_CT_MBIEU_DLIEU,
            DC_CT_NHOM,
            DC_CTRUC,
            DC_CTRUC_CTIET,
            DC_HMUC,
            DC_HMUC_DTUONG,
            DC_HMUC_DTUONG_LSU,
            DC_HMUC_LSU,
            DC_LSUAT,
            DC_LSUAT_CTIET,
            DC_NGUOI_QLY,
            DC_PPHTOAN,
            DC_PPHTOAN_BTOAN,
            DC_PPHTOAN_BTOAN_KT,
            DC_PPHTOAN_DVI,
            DC_PTOA,
            DC_PTOA_LSU,
            DC_TSUAT,
            DC_TSUAT_CUM,
            DC_TTINH,
            DC_TTINH_GTRI,
            DC_TY_GIA,
            DM_CUM,
            DM_DIA_BAN,
            DM_DMUC_GTRI,
            DM_DMUC_LOAI,
            DM_DON_VI,
            DM_DON_VI_CCAU,
            DM_KHU_VUC,
            DM_NHOM,
            DM_PHAN_HE,
            DM_PHAN_HE_GD,
            DM_PHAN_HE_GD_CNANG,
            DM_QUOC_GIA,
            DM_TIEN_TE,
            DM_TINH_TP,
            DM_TTINH,
            DM_TTINH_GTRI,
            DM_VUNG_MIEN,
            DM_DTUONG,
            DM_LOAI_DTUONG,
            DM_TCTD,
            DM_TCTD_TKHOAN,

            HM_CTIET,
            HM_CTIET_LOAITS,
            HM_CTIET_TSDB,
            HM_TONG,
            HM_TONG_NHOMSP,

            HT_CNANG,
            HT_CNANG_HDSD,
            HT_CNANG_TNANG,
            HT_CNANG_TSO,
            HT_CSO,
            HT_CSO_TSO,
            HT_DATA_CONSTRAINT,
            HT_DLIEU_RBUOC,
            HT_HANG_DOI,
            HT_HANG_DOI_LS,
            HT_LICH,
            HT_LICH_LVIEC,
            HT_LICH_NGHI,
            HT_LICH_SU,
            HT_LICH_SU_CT,
            HT_LSU_GDICH,
            HT_NGAY_LVIEC,
            HT_NHNSD,
            HT_NHNSD_HSO,
            HT_NHNSD_NSD,
            HT_NSD,
            HT_NSD_HSO,
            HT_PBAN,
            HT_PBAN_CTIET,
            HT_PHIEN_LVIEC,
            HT_PHIEN_LVIEC_LSU,
            HT_TNANG,
            HT_TNGUYEN,
            HT_TNGUYEN_KBAO,
            HT_TNGUYEN_KTHAC,
            HT_TNGUYEN_LOAI,
            HT_TNGUYEN_LOAI_GTRI,
            HT_TSO,
            HT_TSO_LOAI,
            HT_TTINH,
            HT_TTINH_GTRI,
            HT_TTRINH,
            HT_TVAN,
            HT_TVAN_CTIET,
            KH_CHUYEN_DBAN,
            KH_CHUYEN_DBAN_CTIET,
            KH_KHANG_HSO,
            KH_KHANG_HSO_LSU,
            KH_TTINH,
            KH_TTINH_GTRI,
            KH_TTINH_GTRI_LSU,
            KT_BPHI_PLOAI,
            KT_DOI_CHIEU,
            KT_GIAO_DICH,
            KT_GIAO_DICH_PHI,
            KT_NHOM_PLOAI,
            KT_NHOM_PLOAI_LSDU,
            KT_PHAT_SINH,
            KT_PHAT_SINH_CT,
            KT_PLOAI,
            KT_SO_CAI,
            KT_SO_CAI_CTHUC,
            KT_SO_CAI_DNGHIA,
            KT_SO_CAI_LOAI,
            KT_TCHI_THOP,
            KT_TKHOAN,
            KT_TKHOAN_SDU,
            KT_TTINH,
            KT_TTINH_GTRI,
            KT_TTINH_GTRI_LSU,
            KT_HT_TKTH,
            KT_TKTH,
            KT_TKTH_PLOAI,
            NQ_BKE_TMAT,
            NQ_BKE_TMAT_CT,
            NQ_BKE_TMAT_THOP,
            NQ_MENH_GIA,

            NS_DM_QUOC_TICH,
            NS_DM_TINH_TP,
            NS_DM_QUAN_HUYEN,
            NS_DM_PHUONG_XA,
            NS_DM_DAN_TOC,
            NS_DM_TON_GIAO,
            NS_DM_GIOI_TINH,
            NS_DM_TTRANG_HNHAN,
            NS_DM_CU_TRU,
            NS_DM_HTHUC_LVIEC,
            NS_DM_TDO_HVAN,
            NS_DM_HTHUC_DTAO,
            NS_DM_TRUONG_DTAO,
            NS_DM_BANG_CAP,
            NS_DM_KHOA_DTAO,
            NS_DM_CNGANH_DTAO,
            NS_DM_HOC_HAM,
            NS_DM_HOC_VI,
            NS_DM_TDO_TANH,
            NS_DM_TDO_THOC,
            NS_DM_TDO_CTRI,
            NS_DM_XEP_LOAI,
            NS_DM_QHE_GDINH,
            NS_DM_KY_NANG,
            NS_DM_NGHE_NGHIEP,
            NS_DM_CHUC_VU,
            NS_DM_LDO_NPHEP,
            NS_DM_LDO_TVIEC,
            NS_DM_THAN_HDLD,
            NS_DM_LOAI_HDLD,
            NS_DM_HTHUC_TLUONG,
            NS_DM_LOAI_TNHAP,
            NS_DM_LOAI_CPHI,
            NS_DM_HTHUC_KTHUONG,
            NS_DM_HTHUC_KLUAT,
            NS_DM_KHIEU_CCONG,
            NS_DM_LOAI_HSO,
            NS_DM_DVI_TGIAN,
            NS_DM_DVI_CTAC,
            NS_DM_LOAI_GTO,
            NS_DM_BVIEN_KCB,
            NS_DM_NHOM_CTV,
            NS_DM_DU_AN,
            NS_DM_CHUC_VU_DU_AN,
            NS_DM_CHUC_VU_CTV,
            NS_DM_PHU_CAP,
            NS_DM_DANH_MUC,

            NS_HO_SO,
            NS_HO_LSU,
            NS_HO_SO_QHE_GDINH,
            NS_HO_SO_TDO_HVAN,
            NS_HOP_DONG,
            NS_PHU_CAP,
            NS_QTRINH_CTAC,
            NS_TCHUYEN_CTAC,
            NS_THOI_VIEC,
            NS_BAC_LUONG,
            NS_LUONG,
            NS_LUONG_DCHINH,
            NS_TINH_LUONG,
            NS_PHU_CAP_CDINH_CTV,
            NS_PHU_CAP_BSUNG_CTV,
            NS_TINH_PHU_CAP_CTV,
            NS_TTINH,
            NS_TTINH_GTRI,
            NS_TTINH_GTRI_LS,
            NS_QLY_DU_AN,

            TD_HDTC,
            TD_HDTC_TSDB,
            TD_HDTD,
            TD_HDTD_LSU,
            TD_HDTDVM,
            TD_HDTDVM_LSU,
            TD_KHOACH,
            TD_KHOACH_CT,
            TD_KHOACHVM,
            TD_KHOACHVM_CT,
            TD_KUOC,
            TD_KUOC_LSU,
            TD_KUOCVM,
            TD_KUOCVM_LSU,
            TD_SAN_PHAM,
            TD_SAN_PHAMTT,
            TD_TDUNG_HMUC,
            TD_TSAN_LOAI,
            TD_TSDB,
            TD_TTINH,
            TD_TTINH_GTRI,
            TD_TTINH_GTRI_LSU,
            TD_VONG_VAY,
            TD_VONG_VAY_CTIET,
            TD_TDOI_LSUAT,
            TD_DXVVVM,
            TD_DXVVVM_LSU,
            TDTD_SAN_PHAM,
            TDTD_HDTD,
            TDTD_KUOC,
            TD_KIEM_SOAT_RR,

            TS_DM_NHOM_TSCD,
            TS_TAI_SAN,
            TS_TANG,
            TS_TANG_NG,
            TS_BAN_GIAO,
            TS_NANG_CAP,
            TS_DANH_GIA,
            TS_GIAM,
            TS_KHAU_HAO,

            VKT_HDV_DKY_RGOC,
            TDTD_TSDB,
            TDTD_HDTC,
            TDTD_HDTC_TSDB
        }

        public static string layNgonNguBangDuLieu(string table)
        {
            if (table == getValue(Table.BC_MAU)) return "M.DungChung.Table.BC_MAU";
            else if (table == getValue(Table.BC_MAU_KHAI_THAC)) return "M.DungChung.Table.BC_MAU_KHAI_THAC";
            else if (table == getValue(Table.BC_MAU_THAM_SO)) return "M.DungChung.Table.BC_MAU_THAM_SO";
            else if (table == getValue(Table.BC_THAM_SO)) return "M.DungChung.Table.BC_THAM_SO";
            else if (table == getValue(Table.BL_SAN_PHAM)) return "M.DungChung.Table.BL_SAN_PHAM";
            else if (table == getValue(Table.BL_SAN_PHAM_CT)) return "M.DungChung.Table.BL_SAN_PHAM_CT";
            else if (table == getValue(Table.BL_TDOI_LSUAT)) return "M.DungChung.Table.BL_TDOI_LSUAT";
            else if (table == getValue(Table.BL_TDOI_LSUAT_CT)) return "M.DungChung.Table.BL_TDOI_LSUAT_CT";
            else if (table == getValue(Table.BL_TIEN_GUI)) return "M.DungChung.Table.BL_TIEN_GUI";
            else if (table == getValue(Table.BL_TIEN_GUI_DCSH)) return "M.DungChung.Table.BL_TIEN_GUI_DCSH";
            else if (table == getValue(Table.BL_TIEN_GUI_KHOACH)) return "M.DungChung.Table.BL_TIEN_GUI_KHOACH";
            else if (table == getValue(Table.BL_TIEN_GUI_KHOACH_LSU)) return "M.DungChung.Table.BL_TIEN_GUI_KHOACH_LSU";
            else if (table == getValue(Table.BL_TIEN_GUI_LSU)) return "M.DungChung.Table.BL_TIEN_GUI_LSU";
            else if (table == getValue(Table.BL_TIEN_GUI_TKHOAN)) return "M.DungChung.Table.BL_TIEN_GUI_TKHOAN";
            else if (table == getValue(Table.BL_TTINH)) return "M.DungChung.Table.BL_TTINH";
            else if (table == getValue(Table.BL_TTINH_GTRI)) return "M.DungChung.Table.BL_TTINH_GTRI";
            else if (table == getValue(Table.BL_TTINH_GTRI_LSU)) return "M.DungChung.Table.BL_TTINH_GTRI_LSU";
            else if (table == getValue(Table.BL_DKY_RGOC)) return "M.DungChung.Table.BL_DKY_RGOC";
            else if (table == getValue(Table.BL_DKY_RGOC_CT)) return "M.DungChung.Table.BL_DKY_RGOC_CT";
            else if (table == getValue(Table.CD_NGUON_VON)) return "M.DungChung.Table.CD_NGUON_VON";
            else if (table == getValue(Table.CD_NSD)) return "M.DungChung.Table.CD_NSD";
            else if (table == getValue(Table.DC_BPHI)) return "M.DungChung.Table.DC_BPHI";
            else if (table == getValue(Table.DC_BPHI_CTIET)) return "M.DungChung.Table.DC_BPHI_CTIET";
            else if (table == getValue(Table.DC_BPHI_GDICH)) return "M.DungChung.Table.DC_BPHI_GDICH";
            else if (table == getValue(Table.DC_CSO_TLAI)) return "M.DungChung.Table.DC_CSO_TLAI";
            else if (table == getValue(Table.DC_CSO_TLAI_CTIET)) return "M.DungChung.Table.DC_CSO_TLAI_CTIET";
            else if (table == getValue(Table.DC_CT_CTIEU)) return "M.DungChung.Table.DC_CT_CTIEU";
            else if (table == getValue(Table.DC_CT_DLIEU)) return "M.DungChung.Table.DC_CT_DLIEU";
            else if (table == getValue(Table.DC_CT_DLIEU_DCHIEU)) return "M.DungChung.Table.DC_CT_DLIEU_DCHIEU";
            else if (table == getValue(Table.DC_CT_GOC)) return "M.DungChung.Table.DC_CT_GOC";
            else if (table == getValue(Table.DC_CT_GOC_CTHUC)) return "M.DungChung.Table.DC_CT_GOC_CTHUC";
            else if (table == getValue(Table.DC_CT_GOC_MATKE)) return "M.DungChung.Table.DC_CT_GOC_MATKE";
            else if (table == getValue(Table.DC_CT_LOAITKE)) return "M.DungChung.Table.DC_CT_LOAITKE";
            else if (table == getValue(Table.DC_CT_MATKE)) return "M.DungChung.Table.DC_CT_MATKE";
            else if (table == getValue(Table.DC_CT_MATKE_MANB)) return "M.DungChung.Table.DC_CT_MATKE_MANB";
            else if (table == getValue(Table.DC_CT_MAUNL)) return "M.DungChung.Table.DC_CT_MAUNL";
            else if (table == getValue(Table.DC_CT_MAUNL_CTIEU)) return "M.DungChung.Table.DC_CT_MAUNL_CTIEU";
            else if (table == getValue(Table.DC_CT_MBIEU)) return "M.DungChung.Table.DC_CT_MBIEU";
            else if (table == getValue(Table.DC_CT_MBIEU_DLIEU)) return "M.DungChung.Table.DC_CT_MBIEU_DLIEU";
            else if (table == getValue(Table.DC_CT_NHOM)) return "M.DungChung.Table.DC_CT_NHOM";
            else if (table == getValue(Table.DC_CTRUC)) return "M.DungChung.Table.DC_CTRUC";
            else if (table == getValue(Table.DC_CTRUC_CTIET)) return "M.DungChung.Table.DC_CTRUC_CTIET";
            else if (table == getValue(Table.DC_HMUC)) return "M.DungChung.Table.DC_HMUC";
            else if (table == getValue(Table.DC_HMUC_DTUONG)) return "M.DungChung.Table.DC_HMUC_DTUONG";
            else if (table == getValue(Table.DC_HMUC_DTUONG_LSU)) return "M.DungChung.Table.DC_HMUC_DTUONG_LSU";
            else if (table == getValue(Table.DC_HMUC_LSU)) return "M.DungChung.Table.DC_HMUC_LSU";
            else if (table == getValue(Table.DC_LSUAT)) return "M.DungChung.Table.DC_LSUAT";
            else if (table == getValue(Table.DC_LSUAT_CTIET)) return "M.DungChung.Table.DC_LSUAT_CTIET";
            else if (table == getValue(Table.DC_NGUOI_QLY)) return "M.DungChung.Table.DC_NGUOI_QLY";
            else if (table == getValue(Table.DC_PPHTOAN)) return "M.DungChung.Table.DC_PPHTOAN";
            else if (table == getValue(Table.DC_PPHTOAN_BTOAN)) return "M.DungChung.Table.DC_PPHTOAN_BTOAN";
            else if (table == getValue(Table.DC_PPHTOAN_BTOAN_KT)) return "M.DungChung.Table.DC_PPHTOAN_BTOAN_KT";
            else if (table == getValue(Table.DC_PPHTOAN_DVI)) return "M.DungChung.Table.DC_PPHTOAN_DVI";
            else if (table == getValue(Table.DC_PTOA)) return "M.DungChung.Table.DC_PTOA";
            else if (table == getValue(Table.DC_PTOA_LSU)) return "M.DungChung.Table.DC_PTOA_LSU";
            else if (table == getValue(Table.DC_TSUAT)) return "M.DungChung.Table.DC_TSUAT";
            else if (table == getValue(Table.DC_TSUAT_CUM)) return "M.DungChung.Table.DC_TSUAT_CUM";
            else if (table == getValue(Table.DC_TTINH)) return "M.DungChung.Table.DC_TTINH";
            else if (table == getValue(Table.DC_TTINH_GTRI)) return "M.DungChung.Table.DC_TTINH_GTRI";
            else if (table == getValue(Table.DC_TY_GIA)) return "M.DungChung.Table.DC_TY_GIA";
            else if (table == getValue(Table.DM_CUM)) return "M.DungChung.Table.DM_CUM";
            else if (table == getValue(Table.DM_DIA_BAN)) return "M.DungChung.Table.DM_DIA_BAN";
            else if (table == getValue(Table.DM_DMUC_GTRI)) return "M.DungChung.Table.DM_DMUC_GTRI";
            else if (table == getValue(Table.DM_DMUC_LOAI)) return "M.DungChung.Table.DM_DMUC_LOAI";
            else if (table == getValue(Table.DM_DON_VI)) return "M.DungChung.Table.DM_DON_VI";
            else if (table == getValue(Table.DM_DON_VI_CCAU)) return "M.DungChung.Table.DM_DON_VI_CCAU";
            else if (table == getValue(Table.DM_KHU_VUC)) return "M.DungChung.Table.DM_KHU_VUC";
            else if (table == getValue(Table.DM_NHOM)) return "M.DungChung.Table.DM_NHOM";
            else if (table == getValue(Table.DM_PHAN_HE)) return "M.DungChung.Table.DM_PHAN_HE";
            else if (table == getValue(Table.DM_PHAN_HE_GD)) return "M.DungChung.Table.DM_PHAN_HE_GD";
            else if (table == getValue(Table.DM_PHAN_HE_GD_CNANG)) return "M.DungChung.Table.DM_PHAN_HE_GD_CNANG";
            else if (table == getValue(Table.DM_QUOC_GIA)) return "M.DungChung.Table.DM_QUOC_GIA";
            else if (table == getValue(Table.DM_TIEN_TE)) return "M.DungChung.Table.DM_TIEN_TE";
            else if (table == getValue(Table.DM_TINH_TP)) return "M.DungChung.Table.DM_TINH_TP";
            else if (table == getValue(Table.DM_TTINH)) return "M.DungChung.Table.DM_TTINH";
            else if (table == getValue(Table.DM_TTINH_GTRI)) return "M.DungChung.Table.DM_TTINH_GTRI";
            else if (table == getValue(Table.DM_VUNG_MIEN)) return "M.DungChung.Table.DM_VUNG_MIEN";
            else if (table == getValue(Table.DM_DTUONG)) return "M.DungChung.Table.DM_DTUONG";
            else if (table == getValue(Table.DM_LOAI_DTUONG)) return "M.DungChung.Table.DM_LOAI_DTUONG";
            else if (table == getValue(Table.DM_TCTD)) return "M.DungChung.Table.DM_TCTD";
            else if (table == getValue(Table.DM_TCTD_TKHOAN)) return "M.DungChung.Table.DM_TCTD_TKHOAN";
            else if (table == getValue(Table.HM_CTIET)) return "M.DungChung.Table.HM_CTIET";
            else if (table == getValue(Table.HM_CTIET_LOAITS)) return "M.DungChung.Table.HM_CTIET_LOAITS";
            else if (table == getValue(Table.HM_CTIET_TSDB)) return "M.DungChung.Table.HM_CTIET_TSDB";
            else if (table == getValue(Table.HM_TONG)) return "M.DungChung.Table.HM_TONG";
            else if (table == getValue(Table.HM_TONG_NHOMSP)) return "M.DungChung.Table.HM_TONG_NHOMSP";
            else if (table == getValue(Table.HT_CNANG)) return "M.DungChung.Table.HT_CNANG";
            else if (table == getValue(Table.HT_CNANG_HDSD)) return "M.DungChung.Table.HT_CNANG_HDSD";
            else if (table == getValue(Table.HT_CNANG_TNANG)) return "M.DungChung.Table.HT_CNANG_TNANG";
            else if (table == getValue(Table.HT_CNANG_TSO)) return "M.DungChung.Table.HT_CNANG_TSO";
            else if (table == getValue(Table.HT_CSO)) return "M.DungChung.Table.HT_CSO";
            else if (table == getValue(Table.HT_CSO_TSO)) return "M.DungChung.Table.HT_CSO_TSO";
            else if (table == getValue(Table.HT_DATA_CONSTRAINT)) return "M.DungChung.Table.HT_DATA_CONSTRAINT";
            else if (table == getValue(Table.HT_DLIEU_RBUOC)) return "M.DungChung.Table.HT_DLIEU_RBUOC";
            else if (table == getValue(Table.HT_HANG_DOI)) return "M.DungChung.Table.HT_HANG_DOI";
            else if (table == getValue(Table.HT_HANG_DOI_LS)) return "M.DungChung.Table.HT_HANG_DOI_LS";
            else if (table == getValue(Table.HT_LICH)) return "M.DungChung.Table.HT_LICH";
            else if (table == getValue(Table.HT_LICH_LVIEC)) return "M.DungChung.Table.HT_LICH_LVIEC";
            else if (table == getValue(Table.HT_LICH_NGHI)) return "M.DungChung.Table.HT_LICH_NGHI";
            else if (table == getValue(Table.HT_LICH_SU)) return "M.DungChung.Table.HT_LICH_SU";
            else if (table == getValue(Table.HT_LICH_SU_CT)) return "M.DungChung.Table.HT_LICH_SU_CT";
            else if (table == getValue(Table.HT_LSU_GDICH)) return "M.DungChung.Table.HT_LSU_GDICH";
            else if (table == getValue(Table.HT_NGAY_LVIEC)) return "M.DungChung.Table.HT_NGAY_LVIEC";
            else if (table == getValue(Table.HT_NHNSD)) return "M.DungChung.Table.HT_NHNSD";
            else if (table == getValue(Table.HT_NHNSD_HSO)) return "M.DungChung.Table.HT_NHNSD_HSO";
            else if (table == getValue(Table.HT_NHNSD_NSD)) return "M.DungChung.Table.HT_NHNSD_NSD";
            else if (table == getValue(Table.HT_NSD)) return "M.DungChung.Table.HT_NSD";
            else if (table == getValue(Table.HT_NSD_HSO)) return "M.DungChung.Table.HT_NSD_HSO";
            else if (table == getValue(Table.HT_PBAN)) return "M.DungChung.Table.HT_PBAN";
            else if (table == getValue(Table.HT_PBAN_CTIET)) return "M.DungChung.Table.HT_PBAN_CTIET";
            else if (table == getValue(Table.HT_PHIEN_LVIEC)) return "M.DungChung.Table.HT_PHIEN_LVIEC";
            else if (table == getValue(Table.HT_PHIEN_LVIEC_LSU)) return "M.DungChung.Table.HT_PHIEN_LVIEC_LSU";
            else if (table == getValue(Table.HT_TNANG)) return "M.DungChung.Table.HT_TNANG";
            else if (table == getValue(Table.HT_TNGUYEN)) return "M.DungChung.Table.HT_TNGUYEN";
            else if (table == getValue(Table.HT_TNGUYEN_KBAO)) return "M.DungChung.Table.HT_TNGUYEN_KBAO";
            else if (table == getValue(Table.HT_TNGUYEN_KTHAC)) return "M.DungChung.Table.HT_TNGUYEN_KTHAC";
            else if (table == getValue(Table.HT_TNGUYEN_LOAI)) return "M.DungChung.Table.HT_TNGUYEN_LOAI";
            else if (table == getValue(Table.HT_TNGUYEN_LOAI_GTRI)) return "M.DungChung.Table.HT_TNGUYEN_LOAI_GTRI";
            else if (table == getValue(Table.HT_TSO)) return "M.DungChung.Table.HT_TSO";
            else if (table == getValue(Table.HT_TSO_LOAI)) return "M.DungChung.Table.HT_TSO_LOAI";
            else if (table == getValue(Table.HT_TTINH)) return "M.DungChung.Table.HT_TTINH";
            else if (table == getValue(Table.HT_TTINH_GTRI)) return "M.DungChung.Table.HT_TTINH_GTRI";
            else if (table == getValue(Table.HT_TTRINH)) return "M.DungChung.Table.HT_TTRINH";
            else if (table == getValue(Table.HT_TVAN)) return "M.DungChung.Table.HT_TVAN";
            else if (table == getValue(Table.HT_TVAN_CTIET)) return "M.DungChung.Table.HT_TVAN_CTIET";
            else if (table == getValue(Table.KH_CHUYEN_DBAN)) return "M.DungChung.Table.KH_CHUYEN_DBAN";
            else if (table == getValue(Table.KH_CHUYEN_DBAN_CTIET)) return "M.DungChung.Table.KH_CHUYEN_DBAN_CTIET";
            else if (table == getValue(Table.KH_KHANG_HSO)) return "M.DungChung.Table.KH_KHANG_HSO";
            else if (table == getValue(Table.KH_KHANG_HSO_LSU)) return "M.DungChung.Table.KH_KHANG_HSO_LSU";
            else if (table == getValue(Table.KH_TTINH)) return "M.DungChung.Table.KH_TTINH";
            else if (table == getValue(Table.KH_TTINH_GTRI)) return "M.DungChung.Table.KH_TTINH_GTRI";
            else if (table == getValue(Table.KH_TTINH_GTRI_LSU)) return "M.DungChung.Table.KH_TTINH_GTRI_LSU";
            else if (table == getValue(Table.KT_BPHI_PLOAI)) return "M.DungChung.Table.KT_BPHI_PLOAI";
            else if (table == getValue(Table.KT_DOI_CHIEU)) return "M.DungChung.Table.KT_DOI_CHIEU";
            else if (table == getValue(Table.KT_GIAO_DICH)) return "M.DungChung.Table.KT_GIAO_DICH";
            else if (table == getValue(Table.KT_GIAO_DICH_PHI)) return "M.DungChung.Table.KT_GIAO_DICH_PHI";
            else if (table == getValue(Table.KT_NHOM_PLOAI)) return "M.DungChung.Table.KT_NHOM_PLOAI";
            else if (table == getValue(Table.KT_NHOM_PLOAI_LSDU)) return "M.DungChung.Table.KT_NHOM_PLOAI_LSDU";
            else if (table == getValue(Table.KT_PHAT_SINH)) return "M.DungChung.Table.KT_PHAT_SINH";
            else if (table == getValue(Table.KT_PHAT_SINH_CT)) return "M.DungChung.Table.KT_PHAT_SINH_CT";
            else if (table == getValue(Table.KT_PLOAI)) return "M.DungChung.Table.KT_PLOAI";
            else if (table == getValue(Table.KT_SO_CAI)) return "M.DungChung.Table.KT_SO_CAI";
            else if (table == getValue(Table.KT_SO_CAI_CTHUC)) return "M.DungChung.Table.KT_SO_CAI_CTHUC";
            else if (table == getValue(Table.KT_SO_CAI_DNGHIA)) return "M.DungChung.Table.KT_SO_CAI_DNGHIA";
            else if (table == getValue(Table.KT_SO_CAI_LOAI)) return "M.DungChung.Table.KT_SO_CAI_LOAI";
            else if (table == getValue(Table.KT_TCHI_THOP)) return "M.DungChung.Table.KT_TCHI_THOP";
            else if (table == getValue(Table.KT_TKHOAN)) return "M.DungChung.Table.KT_TKHOAN";
            else if (table == getValue(Table.KT_TKHOAN_SDU)) return "M.DungChung.Table.KT_TKHOAN_SDU";
            else if (table == getValue(Table.KT_TTINH)) return "M.DungChung.Table.KT_TTINH";
            else if (table == getValue(Table.KT_TTINH_GTRI)) return "M.DungChung.Table.KT_TTINH_GTRI";
            else if (table == getValue(Table.KT_TTINH_GTRI_LSU)) return "M.DungChung.Table.KT_TTINH_GTRI_LSU";
            else if (table == getValue(Table.KT_HT_TKTH)) return "M.DungChung.Table.KT_HT_TKTH";
            else if (table == getValue(Table.KT_TKTH)) return "M.DungChung.Table.KT_TKTH";
            else if (table == getValue(Table.KT_TKTH_PLOAI)) return "M.DungChung.Table.KT_TKTH_PLOAI";
            else if (table == getValue(Table.NQ_BKE_TMAT)) return "M.DungChung.Table.NQ_BKE_TMAT";
            else if (table == getValue(Table.NQ_BKE_TMAT_CT)) return "M.DungChung.Table.NQ_BKE_TMAT_CT";
            else if (table == getValue(Table.NQ_BKE_TMAT_THOP)) return "M.DungChung.Table.NQ_BKE_TMAT_THOP";
            else if (table == getValue(Table.NQ_MENH_GIA)) return "M.DungChung.Table.NQ_MENH_GIA";
            else if (table == getValue(Table.NS_DM_QUOC_TICH)) return "M.DungChung.Table.NS_DM_QUOC_TICH";
            else if (table == getValue(Table.NS_DM_TINH_TP)) return "M.DungChung.Table.NS_DM_TINH_TP";
            else if (table == getValue(Table.NS_DM_QUAN_HUYEN)) return "M.DungChung.Table.NS_DM_QUAN_HUYEN";
            else if (table == getValue(Table.NS_DM_PHUONG_XA)) return "M.DungChung.Table.NS_DM_PHUONG_XA";
            else if (table == getValue(Table.NS_DM_DAN_TOC)) return "M.DungChung.Table.NS_DM_DAN_TOC";
            else if (table == getValue(Table.NS_DM_TON_GIAO)) return "M.DungChung.Table.NS_DM_TON_GIAO";
            else if (table == getValue(Table.NS_DM_GIOI_TINH)) return "M.DungChung.Table.NS_DM_GIOI_TINH";
            else if (table == getValue(Table.NS_DM_TTRANG_HNHAN)) return "M.DungChung.Table.NS_DM_TTRANG_HNHAN";
            else if (table == getValue(Table.NS_DM_CU_TRU)) return "M.DungChung.Table.NS_DM_CU_TRU";
            else if (table == getValue(Table.NS_DM_HTHUC_LVIEC)) return "M.DungChung.Table.NS_DM_HTHUC_LVIEC";
            else if (table == getValue(Table.NS_DM_TDO_HVAN)) return "M.DungChung.Table.NS_DM_TDO_HVAN";
            else if (table == getValue(Table.NS_DM_HTHUC_DTAO)) return "M.DungChung.Table.NS_DM_HTHUC_DTAO";
            else if (table == getValue(Table.NS_DM_TRUONG_DTAO)) return "M.DungChung.Table.NS_DM_TRUONG_DTAO";
            else if (table == getValue(Table.NS_DM_BANG_CAP)) return "M.DungChung.Table.NS_DM_BANG_CAP";
            else if (table == getValue(Table.NS_DM_KHOA_DTAO)) return "M.DungChung.Table.NS_DM_KHOA_DTAO";
            else if (table == getValue(Table.NS_DM_CNGANH_DTAO)) return "M.DungChung.Table.NS_DM_CNGANH_DTAO";
            else if (table == getValue(Table.NS_DM_HOC_HAM)) return "M.DungChung.Table.NS_DM_HOC_HAM";
            else if (table == getValue(Table.NS_DM_HOC_VI)) return "M.DungChung.Table.NS_DM_HOC_VI";
            else if (table == getValue(Table.NS_DM_TDO_TANH)) return "M.DungChung.Table.NS_DM_TDO_TANH";
            else if (table == getValue(Table.NS_DM_TDO_THOC)) return "M.DungChung.Table.NS_DM_TDO_THOC";
            else if (table == getValue(Table.NS_DM_TDO_CTRI)) return "M.DungChung.Table.NS_DM_TDO_CTRI";
            else if (table == getValue(Table.NS_DM_XEP_LOAI)) return "M.DungChung.Table.NS_DM_XEP_LOAI";
            else if (table == getValue(Table.NS_DM_QHE_GDINH)) return "M.DungChung.Table.NS_DM_QHE_GDINH";
            else if (table == getValue(Table.NS_DM_KY_NANG)) return "M.DungChung.Table.NS_DM_KY_NANG";
            else if (table == getValue(Table.NS_DM_NGHE_NGHIEP)) return "M.DungChung.Table.NS_DM_NGHE_NGHIEP";
            else if (table == getValue(Table.NS_DM_CHUC_VU)) return "M.DungChung.Table.NS_DM_CHUC_VU";
            else if (table == getValue(Table.NS_DM_LDO_NPHEP)) return "M.DungChung.Table.NS_DM_LDO_NPHEP";
            else if (table == getValue(Table.NS_DM_LDO_TVIEC)) return "M.DungChung.Table.NS_DM_LDO_TVIEC";
            else if (table == getValue(Table.NS_DM_THAN_HDLD)) return "M.DungChung.Table.NS_DM_THAN_HDLD";
            else if (table == getValue(Table.NS_DM_LOAI_HDLD)) return "M.DungChung.Table.NS_DM_LOAI_HDLD";
            else if (table == getValue(Table.NS_DM_HTHUC_TLUONG)) return "M.DungChung.Table.NS_DM_HTHUC_TLUONG";
            else if (table == getValue(Table.NS_DM_LOAI_TNHAP)) return "M.DungChung.Table.NS_DM_LOAI_TNHAP";
            else if (table == getValue(Table.NS_DM_LOAI_CPHI)) return "M.DungChung.Table.NS_DM_LOAI_CPHI";
            else if (table == getValue(Table.NS_DM_HTHUC_KTHUONG)) return "M.DungChung.Table.NS_DM_HTHUC_KTHUONG";
            else if (table == getValue(Table.NS_DM_HTHUC_KLUAT)) return "M.DungChung.Table.NS_DM_HTHUC_KLUAT";
            else if (table == getValue(Table.NS_DM_KHIEU_CCONG)) return "M.DungChung.Table.NS_DM_KHIEU_CCONG";
            else if (table == getValue(Table.NS_DM_LOAI_HSO)) return "M.DungChung.Table.NS_DM_LOAI_HSO";
            else if (table == getValue(Table.NS_DM_DVI_TGIAN)) return "M.DungChung.Table.NS_DM_DVI_TGIAN";
            else if (table == getValue(Table.NS_DM_DVI_CTAC)) return "M.DungChung.Table.NS_DM_DVI_CTAC";
            else if (table == getValue(Table.NS_DM_LOAI_GTO)) return "M.DungChung.Table.NS_DM_LOAI_GTO";
            else if (table == getValue(Table.NS_DM_BVIEN_KCB)) return "M.DungChung.Table.NS_DM_BVIEN_KCB";
            else if (table == getValue(Table.NS_DM_NHOM_CTV)) return "M.DungChung.Table.NS_DM_NHOM_CTV";
            else if (table == getValue(Table.NS_DM_DU_AN)) return "M.DungChung.Table.NS_DM_DU_AN";
            else if (table == getValue(Table.NS_DM_CHUC_VU_DU_AN)) return "M.DungChung.Table.NS_DM_CHUC_VU_DU_AN";
            else if (table == getValue(Table.NS_DM_CHUC_VU_CTV)) return "M.DungChung.Table.NS_DM_CHUC_VU_CTV";
            else if (table == getValue(Table.NS_DM_PHU_CAP)) return "M.DungChung.Table.NS_DM_PHU_CAP";
            else if (table == getValue(Table.NS_DM_DANH_MUC)) return "M.DungChung.Table.NS_DM_DANH_MUC";

            else if (table == getValue(Table.NS_HO_SO)) return "M.DungChung.Table.NS_HO_SO";
            else if (table == getValue(Table.NS_HO_LSU)) return "M.DungChung.Table.NS_HO_LSU";
            else if (table == getValue(Table.NS_HO_SO_QHE_GDINH)) return "M.DungChung.Table.NS_HO_SO_QHE_GDINH";
            else if (table == getValue(Table.NS_HO_SO_TDO_HVAN)) return "M.DungChung.Table.NS_HO_SO_TDO_HVAN";
            else if (table == getValue(Table.NS_HOP_DONG)) return "M.DungChung.Table.NS_HOP_DONG";
            else if (table == getValue(Table.NS_PHU_CAP)) return "M.DungChung.Table.NS_PHU_CAP";
            else if (table == getValue(Table.NS_QTRINH_CTAC)) return "M.DungChung.Table.NS_QTRINH_CTAC";
            else if (table == getValue(Table.NS_THOI_VIEC)) return "M.DungChung.Table.NS_THOI_VIEC";
            else if (table == getValue(Table.NS_BAC_LUONG)) return "M.DungChung.Table.NS_BAC_LUONG";
            else if (table == getValue(Table.NS_LUONG)) return "M.DungChung.Table.NS_LUONG";
            else if (table == getValue(Table.NS_LUONG_DCHINH)) return "M.DungChung.Table.NS_LUONG_DCHINH";
            else if (table == getValue(Table.NS_TINH_LUONG)) return "M.DungChung.Table.NS_TINH_LUONG";
            else if (table == getValue(Table.NS_PHU_CAP_CDINH_CTV)) return "M.DungChung.Table.NS_PHU_CAP_CDINH_CTV";
            else if (table == getValue(Table.NS_PHU_CAP_BSUNG_CTV)) return "M.DungChung.Table.NS_PHU_CAP_BSUNG_CTV";
            else if (table == getValue(Table.NS_TINH_PHU_CAP_CTV)) return "M.DungChung.Table.NS_TINH_PHU_CAP_CTV";
            else if (table == getValue(Table.NS_TTINH)) return "M.DungChung.Table.NS_TTINH";
            else if (table == getValue(Table.NS_TTINH_GTRI)) return "M.DungChung.Table.NS_TTINH_GTRI";
            else if (table == getValue(Table.NS_TTINH_GTRI_LS)) return "M.DungChung.Table.NS_TTINH_GTRI_LS";
            else if (table == getValue(Table.NS_QLY_DU_AN)) return "M.DungChung.Table.NS_QLY_DU_AN";

            else if (table == getValue(Table.TD_HDTC)) return "M.DungChung.Table.TD_HDTC";
            else if (table == getValue(Table.TD_HDTC_TSDB)) return "M.DungChung.Table.TD_HDTC_TSDB";
            else if (table == getValue(Table.TD_HDTD)) return "M.DungChung.Table.TD_HDTD";
            else if (table == getValue(Table.TD_HDTD_LSU)) return "M.DungChung.Table.TD_HDTD_LSU";
            else if (table == getValue(Table.TD_HDTDVM)) return "M.DungChung.Table.TD_HDTDVM";
            else if (table == getValue(Table.TD_HDTDVM_LSU)) return "M.DungChung.Table.TD_HDTDVM_LSU";
            else if (table == getValue(Table.TD_KHOACH)) return "M.DungChung.Table.TD_KHOACH";
            else if (table == getValue(Table.TD_KHOACH_CT)) return "M.DungChung.Table.TD_KHOACH_CT";
            else if (table == getValue(Table.TD_KHOACHVM)) return "M.DungChung.Table.TD_KHOACHVM";
            else if (table == getValue(Table.TD_KHOACHVM_CT)) return "M.DungChung.Table.TD_KHOACHVM_CT";
            else if (table == getValue(Table.TD_KUOC)) return "M.DungChung.Table.TD_KUOC";
            else if (table == getValue(Table.TD_KUOC_LSU)) return "M.DungChung.Table.TD_KUOC_LSU";
            else if (table == getValue(Table.TD_KUOCVM)) return "M.DungChung.Table.TD_KUOCVM";
            else if (table == getValue(Table.TD_KUOCVM_LSU)) return "M.DungChung.Table.TD_KUOCVM_LSU";
            else if (table == getValue(Table.TD_SAN_PHAM)) return "M.DungChung.Table.TD_SAN_PHAM";
            else if (table == getValue(Table.TD_SAN_PHAMTT)) return "M.DungChung.Table.TD_SAN_PHAMTT";
            else if (table == getValue(Table.TD_TDUNG_HMUC)) return "M.DungChung.Table.TD_TDUNG_HMUC";
            else if (table == getValue(Table.TD_TSAN_LOAI)) return "M.DungChung.Table.TD_TSAN_LOAI";
            else if (table == getValue(Table.TD_TSDB)) return "M.DungChung.Table.TD_TSDB";
            else if (table == getValue(Table.TD_TTINH)) return "M.DungChung.Table.TD_TTINH";
            else if (table == getValue(Table.TD_TTINH_GTRI)) return "M.DungChung.Table.TD_TTINH_GTRI";
            else if (table == getValue(Table.TD_TTINH_GTRI_LSU)) return "M.DungChung.Table.TD_TTINH_GTRI_LSU";
            else if (table == getValue(Table.TD_VONG_VAY)) return "M.DungChung.Table.TD_VONG_VAY";
            else if (table == getValue(Table.TD_VONG_VAY_CTIET)) return "M.DungChung.Table.TD_VONG_VAY_CTIET";
            else if (table == getValue(Table.TD_TDOI_LSUAT)) return "M.DungChung.Table.TD_TDOI_LSUAT";
            else if (table == getValue(Table.TD_DXVVVM)) return "M.DungChung.Table.TD_DXVVVM";
            else if (table == getValue(Table.TD_DXVVVM_LSU)) return "M.DungChung.Table.TD_DXVVVM_LSU";
            else if (table == getValue(Table.TDTD_SAN_PHAM)) return "M.DungChung.Table.TDTD_SAN_PHAM";
            else if (table == getValue(Table.TDTD_HDTD)) return "M.DungChung.Table.TDTD_HDTD";
            else if (table == getValue(Table.TDTD_KUOC)) return "M.DungChung.Table.TDTD_KUOC";
            else if (table == getValue(Table.TD_KIEM_SOAT_RR)) return "M.DungChung.Table.TD_KIEM_SOAT_RR";
            else if (table == getValue(Table.VKT_HDV_DKY_RGOC)) return "M.DungChung.Table.VKT_HDV_DKY_RGOC";

            else if (table == getValue(Table.TDTD_TSDB)) return "M.DungChung.Table.TDTD_TSDB";
            else if (table == getValue(Table.TDTD_HDTC)) return "M.DungChung.Table.TDTD_HDTC";
            else if (table == getValue(Table.TDTD_HDTC_TSDB)) return "M.DungChung.Table.TDTD_HDTC_TSDB";

            else return "";
        }

        public static string layNgonNgu(this Table table)
        {
            switch (table)
            {
                case Table.BC_MAU: return "M.DungChung.Table.BC_MAU";
                case Table.BC_MAU_KHAI_THAC: return "M.DungChung.Table.BC_MAU_KHAI_THAC";
                case Table.BC_MAU_THAM_SO: return "M.DungChung.Table.BC_MAU_THAM_SO";
                case Table.BC_THAM_SO: return "M.DungChung.Table.BC_THAM_SO";
                case Table.BL_SAN_PHAM: return "M.DungChung.Table.BL_SAN_PHAM";
                case Table.BL_SAN_PHAM_CT: return "M.DungChung.Table.BL_SAN_PHAM_CT";
                case Table.BL_TDOI_LSUAT: return "M.DungChung.Table.BL_TDOI_LSUAT";
                case Table.BL_TDOI_LSUAT_CT: return "M.DungChung.Table.BL_TDOI_LSUAT_CT";
                case Table.BL_TIEN_GUI: return "M.DungChung.Table.BL_TIEN_GUI";
                case Table.BL_TIEN_GUI_DCSH: return "M.DungChung.Table.BL_TIEN_GUI_DCSH";
                case Table.BL_TIEN_GUI_KHOACH: return "M.DungChung.Table.BL_TIEN_GUI_KHOACH";
                case Table.BL_TIEN_GUI_KHOACH_LSU: return "M.DungChung.Table.BL_TIEN_GUI_KHOACH_LSU";
                case Table.BL_TIEN_GUI_LSU: return "M.DungChung.Table.BL_TIEN_GUI_LSU";
                case Table.BL_TIEN_GUI_TKHOAN: return "M.DungChung.Table.BL_TIEN_GUI_TKHOAN";
                case Table.BL_TTINH: return "M.DungChung.Table.BL_TTINH";
                case Table.BL_TTINH_GTRI: return "M.DungChung.Table.BL_TTINH_GTRI";
                case Table.BL_TTINH_GTRI_LSU: return "M.DungChung.Table.BL_TTINH_GTRI_LSU";
                case Table.BL_DKY_RGOC: return "M.DungChung.Table.BL_DKY_RGOC";
                case Table.BL_DKY_RGOC_CT: return "M.DungChung.Table.BL_DKY_RGOC_CT";
                case Table.CD_NGUON_VON: return "M.DungChung.Table.CD_NGUON_VON";
                case Table.CD_NSD: return "M.DungChung.Table.CD_NSD";
                case Table.DC_BPHI: return "M.DungChung.Table.DC_BPHI";
                case Table.DC_BPHI_CTIET: return "M.DungChung.Table.DC_BPHI_CTIET";
                case Table.DC_BPHI_GDICH: return "M.DungChung.Table.DC_BPHI_GDICH";
                case Table.DC_CSO_TLAI: return "M.DungChung.Table.DC_CSO_TLAI";
                case Table.DC_CSO_TLAI_CTIET: return "M.DungChung.Table.DC_CSO_TLAI_CTIET";
                case Table.DC_CT_CTIEU: return "M.DungChung.Table.DC_CT_CTIEU";
                case Table.DC_CT_DLIEU: return "M.DungChung.Table.DC_CT_DLIEU";
                case Table.DC_CT_DLIEU_DCHIEU: return "M.DungChung.Table.DC_CT_DLIEU_DCHIEU";
                case Table.DC_CT_GOC: return "M.DungChung.Table.DC_CT_GOC";
                case Table.DC_CT_GOC_CTHUC: return "M.DungChung.Table.DC_CT_GOC_CTHUC";
                case Table.DC_CT_GOC_MATKE: return "M.DungChung.Table.DC_CT_GOC_MATKE";
                case Table.DC_CT_LOAITKE: return "M.DungChung.Table.DC_CT_LOAITKE";
                case Table.DC_CT_MATKE: return "M.DungChung.Table.DC_CT_MATKE";
                case Table.DC_CT_MATKE_MANB: return "M.DungChung.Table.DC_CT_MATKE_MANB";
                case Table.DC_CT_MAUNL: return "M.DungChung.Table.DC_CT_MAUNL";
                case Table.DC_CT_MAUNL_CTIEU: return "M.DungChung.Table.DC_CT_MAUNL_CTIEU";
                case Table.DC_CT_MBIEU: return "M.DungChung.Table.DC_CT_MBIEU";
                case Table.DC_CT_MBIEU_DLIEU: return "M.DungChung.Table.DC_CT_MBIEU_DLIEU";
                case Table.DC_CT_NHOM: return "M.DungChung.Table.DC_CT_NHOM";
                case Table.DC_CTRUC: return "M.DungChung.Table.DC_CTRUC";
                case Table.DC_CTRUC_CTIET: return "M.DungChung.Table.DC_CTRUC_CTIET";
                case Table.DC_HMUC: return "M.DungChung.Table.DC_HMUC";
                case Table.DC_HMUC_DTUONG: return "M.DungChung.Table.DC_HMUC_DTUONG";
                case Table.DC_HMUC_DTUONG_LSU: return "M.DungChung.Table.DC_HMUC_DTUONG_LSU";
                case Table.DC_HMUC_LSU: return "M.DungChung.Table.DC_HMUC_LSU";
                case Table.DC_LSUAT: return "M.DungChung.Table.DC_LSUAT";
                case Table.DC_LSUAT_CTIET: return "M.DungChung.Table.DC_LSUAT_CTIET";
                case Table.DC_NGUOI_QLY: return "M.DungChung.Table.DC_NGUOI_QLY";
                case Table.DC_PPHTOAN: return "M.DungChung.Table.DC_PPHTOAN";
                case Table.DC_PPHTOAN_BTOAN: return "M.DungChung.Table.DC_PPHTOAN_BTOAN";
                case Table.DC_PPHTOAN_BTOAN_KT: return "M.DungChung.Table.DC_PPHTOAN_BTOAN_KT";
                case Table.DC_PPHTOAN_DVI: return "M.DungChung.Table.DC_PPHTOAN_DVI";
                case Table.DC_PTOA: return "M.DungChung.Table.DC_PTOA";
                case Table.DC_PTOA_LSU: return "M.DungChung.Table.DC_PTOA_LSU";
                case Table.DC_TSUAT: return "M.DungChung.Table.DC_TSUAT";
                case Table.DC_TSUAT_CUM: return "M.DungChung.Table.DC_TSUAT_CUM";
                case Table.DC_TTINH: return "M.DungChung.Table.DC_TTINH";
                case Table.DC_TTINH_GTRI: return "M.DungChung.Table.DC_TTINH_GTRI";
                case Table.DC_TY_GIA: return "M.DungChung.Table.DC_TY_GIA";
                case Table.DM_CUM: return "M.DungChung.Table.DM_CUM";
                case Table.DM_DIA_BAN: return "M.DungChung.Table.DM_DIA_BAN";
                case Table.DM_DMUC_GTRI: return "M.DungChung.Table.DM_DMUC_GTRI";
                case Table.DM_DMUC_LOAI: return "M.DungChung.Table.DM_DMUC_LOAI";
                case Table.DM_DON_VI: return "M.DungChung.Table.DM_DON_VI";
                case Table.DM_DON_VI_CCAU: return "M.DungChung.Table.DM_DON_VI_CCAU";
                case Table.DM_KHU_VUC: return "M.DungChung.Table.DM_KHU_VUC";
                case Table.DM_NHOM: return "M.DungChung.Table.DM_NHOM";
                case Table.DM_PHAN_HE: return "M.DungChung.Table.DM_PHAN_HE";
                case Table.DM_PHAN_HE_GD: return "M.DungChung.Table.DM_PHAN_HE_GD";
                case Table.DM_PHAN_HE_GD_CNANG: return "M.DungChung.Table.DM_PHAN_HE_GD_CNANG";
                case Table.DM_QUOC_GIA: return "M.DungChung.Table.DM_QUOC_GIA";
                case Table.DM_TIEN_TE: return "M.DungChung.Table.DM_TIEN_TE";
                case Table.DM_TINH_TP: return "M.DungChung.Table.DM_TINH_TP";
                case Table.DM_TTINH: return "M.DungChung.Table.DM_TTINH";
                case Table.DM_TTINH_GTRI: return "M.DungChung.Table.DM_TTINH_GTRI";
                case Table.DM_VUNG_MIEN: return "M.DungChung.Table.DM_VUNG_MIEN";
                case Table.DM_DTUONG: return "M.DungChung.Table.DM_DTUONG";
                case Table.DM_LOAI_DTUONG: return "M.DungChung.Table.DM_LOAI_DTUONG";
                case Table.DM_TCTD: return "M.DungChung.Table.DM_TCTD";
                case Table.DM_TCTD_TKHOAN: return "M.Dungchung.Table.DM_TCTD_TKHOAN";
                case Table.HM_CTIET: return "M.DungChung.Table.HM_CTIET";
                case Table.HM_CTIET_LOAITS: return "M.DungChung.Table.HM_CTIET_LOAITS";
                case Table.HM_CTIET_TSDB: return "M.DungChung.Table.HM_CTIET_TSDB";
                case Table.HM_TONG: return "M.DungChung.Table.HM_TONG";
                case Table.HM_TONG_NHOMSP: return "M.DungChung.Table.HM_TONG_NHOMSP";
                case Table.HT_CNANG: return "M.DungChung.Table.HT_CNANG";
                case Table.HT_CNANG_HDSD: return "M.DungChung.Table.HT_CNANG_HDSD";
                case Table.HT_CNANG_TNANG: return "M.DungChung.Table.HT_CNANG_TNANG";
                case Table.HT_CNANG_TSO: return "M.DungChung.Table.HT_CNANG_TSO";
                case Table.HT_CSO: return "M.DungChung.Table.HT_CSO";
                case Table.HT_CSO_TSO: return "M.DungChung.Table.HT_CSO_TSO";
                case Table.HT_DATA_CONSTRAINT: return "M.DungChung.Table.HT_DATA_CONSTRAINT";
                case Table.HT_DLIEU_RBUOC: return "M.DungChung.Table.HT_DLIEU_RBUOC";
                case Table.HT_HANG_DOI: return "M.DungChung.Table.HT_HANG_DOI";
                case Table.HT_HANG_DOI_LS: return "M.DungChung.Table.HT_HANG_DOI_LS";
                case Table.HT_LICH: return "M.DungChung.Table.HT_LICH";
                case Table.HT_LICH_LVIEC: return "M.DungChung.Table.HT_LICH_LVIEC";
                case Table.HT_LICH_NGHI: return "M.DungChung.Table.HT_LICH_NGHI";
                case Table.HT_LICH_SU: return "M.DungChung.Table.HT_LICH_SU";
                case Table.HT_LICH_SU_CT: return "M.DungChung.Table.HT_LICH_SU_CT";
                case Table.HT_LSU_GDICH: return "M.DungChung.Table.HT_LSU_GDICH";
                case Table.HT_NGAY_LVIEC: return "M.DungChung.Table.HT_NGAY_LVIEC";
                case Table.HT_NHNSD: return "M.DungChung.Table.HT_NHNSD";
                case Table.HT_NHNSD_HSO: return "M.DungChung.Table.HT_NHNSD_HSO";
                case Table.HT_NHNSD_NSD: return "M.DungChung.Table.HT_NHNSD_NSD";
                case Table.HT_NSD: return "M.DungChung.Table.HT_NSD";
                case Table.HT_NSD_HSO: return "M.DungChung.Table.HT_NSD_HSO";
                case Table.HT_PBAN: return "M.DungChung.Table.HT_PBAN";
                case Table.HT_PBAN_CTIET: return "M.DungChung.Table.HT_PBAN_CTIET";
                case Table.HT_PHIEN_LVIEC: return "M.DungChung.Table.HT_PHIEN_LVIEC";
                case Table.HT_PHIEN_LVIEC_LSU: return "M.DungChung.Table.HT_PHIEN_LVIEC_LSU";
                case Table.HT_TNANG: return "M.DungChung.Table.HT_TNANG";
                case Table.HT_TNGUYEN: return "M.DungChung.Table.HT_TNGUYEN";
                case Table.HT_TNGUYEN_KBAO: return "M.DungChung.Table.HT_TNGUYEN_KBAO";
                case Table.HT_TNGUYEN_KTHAC: return "M.DungChung.Table.HT_TNGUYEN_KTHAC";
                case Table.HT_TNGUYEN_LOAI: return "M.DungChung.Table.HT_TNGUYEN_LOAI";
                case Table.HT_TNGUYEN_LOAI_GTRI: return "M.DungChung.Table.HT_TNGUYEN_LOAI_GTRI";
                case Table.HT_TSO: return "M.DungChung.Table.HT_TSO";
                case Table.HT_TSO_LOAI: return "M.DungChung.Table.HT_TSO_LOAI";
                case Table.HT_TTINH: return "M.DungChung.Table.HT_TTINH";
                case Table.HT_TTINH_GTRI: return "M.DungChung.Table.HT_TTINH_GTRI";
                case Table.HT_TTRINH: return "M.DungChung.Table.HT_TTRINH";
                case Table.HT_TVAN: return "M.DungChung.Table.HT_TVAN";
                case Table.HT_TVAN_CTIET: return "M.DungChung.Table.HT_TVAN_CTIET";
                case Table.KH_CHUYEN_DBAN: return "M.DungChung.Table.KH_CHUYEN_DBAN";
                case Table.KH_CHUYEN_DBAN_CTIET: return "M.DungChung.Table.KH_CHUYEN_DBAN_CTIET";
                case Table.KH_KHANG_HSO: return "M.DungChung.Table.KH_KHANG_HSO";
                case Table.KH_KHANG_HSO_LSU: return "M.DungChung.Table.KH_KHANG_HSO_LSU";
                case Table.KH_TTINH: return "M.DungChung.Table.KH_TTINH";
                case Table.KH_TTINH_GTRI: return "M.DungChung.Table.KH_TTINH_GTRI";
                case Table.KH_TTINH_GTRI_LSU: return "M.DungChung.Table.KH_TTINH_GTRI_LSU";
                case Table.KT_BPHI_PLOAI: return "M.DungChung.Table.KT_BPHI_PLOAI";
                case Table.KT_DOI_CHIEU: return "M.DungChung.Table.KT_DOI_CHIEU";
                case Table.KT_GIAO_DICH: return "M.DungChung.Table.KT_GIAO_DICH";
                case Table.KT_GIAO_DICH_PHI: return "M.DungChung.Table.KT_GIAO_DICH_PHI";
                case Table.KT_NHOM_PLOAI: return "M.DungChung.Table.KT_NHOM_PLOAI";
                case Table.KT_NHOM_PLOAI_LSDU: return "M.DungChung.Table.KT_NHOM_PLOAI_LSDU";
                case Table.KT_PHAT_SINH: return "M.DungChung.Table.KT_PHAT_SINH";
                case Table.KT_PHAT_SINH_CT: return "M.DungChung.Table.KT_PHAT_SINH_CT";
                case Table.KT_PLOAI: return "M.DungChung.Table.KT_PLOAI";
                case Table.KT_SO_CAI: return "M.DungChung.Table.KT_SO_CAI";
                case Table.KT_SO_CAI_CTHUC: return "M.DungChung.Table.KT_SO_CAI_CTHUC";
                case Table.KT_SO_CAI_DNGHIA: return "M.DungChung.Table.KT_SO_CAI_DNGHIA";
                case Table.KT_SO_CAI_LOAI: return "M.DungChung.Table.KT_SO_CAI_LOAI";
                case Table.KT_TCHI_THOP: return "M.DungChung.Table.KT_TCHI_THOP";
                case Table.KT_TKHOAN: return "M.DungChung.Table.KT_TKHOAN";
                case Table.KT_TKHOAN_SDU: return "M.DungChung.Table.KT_TKHOAN_SDU";
                case Table.KT_TTINH: return "M.DungChung.Table.KT_TTINH";
                case Table.KT_TTINH_GTRI: return "M.DungChung.Table.KT_TTINH_GTRI";
                case Table.KT_TTINH_GTRI_LSU: return "M.DungChung.Table.KT_TTINH_GTRI_LSU";
                case Table.KT_HT_TKTH: return "M.DungChung.Table.KT_HT_TKTH";
                case Table.KT_TKTH: return "M.DungChung.Table.KT_TKTH";
                case Table.KT_TKTH_PLOAI: return "M.DungChung.Table.KT_TKTH_PLOAI";
                case Table.NQ_BKE_TMAT: return "M.DungChung.Table.NQ_BKE_TMAT";
                case Table.NQ_BKE_TMAT_CT: return "M.DungChung.Table.NQ_BKE_TMAT_CT";
                case Table.NQ_BKE_TMAT_THOP: return "M.DungChung.Table.NQ_BKE_TMAT_THOP";
                case Table.NQ_MENH_GIA: return "M.DungChung.Table.NQ_MENH_GIA";
                case Table.NS_DM_QUOC_TICH: return "M.DungChung.Table.NS_DM_QUOC_TICH";
                case Table.NS_DM_TINH_TP: return "M.DungChung.Table.NS_DM_TINH_TP";
                case Table.NS_DM_QUAN_HUYEN: return "M.DungChung.Table.NS_DM_QUAN_HUYEN";
                case Table.NS_DM_PHUONG_XA: return "M.DungChung.Table.NS_DM_PHUONG_XA";
                case Table.NS_DM_DAN_TOC: return "M.DungChung.Table.NS_DM_DAN_TOC";
                case Table.NS_DM_TON_GIAO: return "M.DungChung.Table.NS_DM_TON_GIAO";
                case Table.NS_DM_GIOI_TINH: return "M.DungChung.Table.NS_DM_GIOI_TINH";
                case Table.NS_DM_TTRANG_HNHAN: return "M.DungChung.Table.NS_DM_TTRANG_HNHAN";
                case Table.NS_DM_CU_TRU: return "M.DungChung.Table.NS_DM_CU_TRU";
                case Table.NS_DM_HTHUC_LVIEC: return "M.DungChung.Table.NS_DM_HTHUC_LVIEC";
                case Table.NS_DM_TDO_HVAN: return "M.DungChung.Table.NS_DM_TDO_HVAN";
                case Table.NS_DM_HTHUC_DTAO: return "M.DungChung.Table.NS_DM_HTHUC_DTAO";
                case Table.NS_DM_TRUONG_DTAO: return "M.DungChung.Table.NS_DM_TRUONG_DTAO";
                case Table.NS_DM_BANG_CAP: return "M.DungChung.Table.NS_DM_BANG_CAP";
                case Table.NS_DM_KHOA_DTAO: return "M.DungChung.Table.NS_DM_KHOA_DTAO";
                case Table.NS_DM_CNGANH_DTAO: return "M.DungChung.Table.NS_DM_CNGANH_DTAO";
                case Table.NS_DM_HOC_HAM: return "M.DungChung.Table.NS_DM_HOC_HAM";
                case Table.NS_DM_HOC_VI: return "M.DungChung.Table.NS_DM_HOC_VI";
                case Table.NS_DM_TDO_TANH: return "M.DungChung.Table.NS_DM_TDO_TANH";
                case Table.NS_DM_TDO_THOC: return "M.DungChung.Table.NS_DM_TDO_THOC";
                case Table.NS_DM_TDO_CTRI: return "M.DungChung.Table.NS_DM_TDO_CTRI";
                case Table.NS_DM_XEP_LOAI: return "M.DungChung.Table.NS_DM_XEP_LOAI";
                case Table.NS_DM_QHE_GDINH: return "M.DungChung.Table.NS_DM_QHE_GDINH";
                case Table.NS_DM_KY_NANG: return "M.DungChung.Table.NS_DM_KY_NANG";
                case Table.NS_DM_NGHE_NGHIEP: return "M.DungChung.Table.NS_DM_NGHE_NGHIEP";
                case Table.NS_DM_CHUC_VU: return "M.DungChung.Table.NS_DM_CHUC_VU";
                case Table.NS_DM_LDO_NPHEP: return "M.DungChung.Table.NS_DM_LDO_NPHEP";
                case Table.NS_DM_LDO_TVIEC: return "M.DungChung.Table.NS_DM_LDO_TVIEC";
                case Table.NS_DM_THAN_HDLD: return "M.DungChung.Table.NS_DM_THAN_HDLD";
                case Table.NS_DM_LOAI_HDLD: return "M.DungChung.Table.NS_DM_LOAI_HDLD";
                case Table.NS_DM_HTHUC_TLUONG: return "M.DungChung.Table.NS_DM_HTHUC_TLUONG";
                case Table.NS_DM_LOAI_TNHAP: return "M.DungChung.Table.NS_DM_LOAI_TNHAP";
                case Table.NS_DM_LOAI_CPHI: return "M.DungChung.Table.NS_DM_LOAI_CPHI";
                case Table.NS_DM_HTHUC_KTHUONG: return "M.DungChung.Table.NS_DM_HTHUC_KTHUONG";
                case Table.NS_DM_HTHUC_KLUAT: return "M.DungChung.Table.NS_DM_HTHUC_KLUAT";
                case Table.NS_DM_KHIEU_CCONG: return "M.DungChung.Table.NS_DM_KHIEU_CCONG";
                case Table.NS_DM_LOAI_HSO: return "M.DungChung.Table.NS_DM_LOAI_HSO";
                case Table.NS_DM_DVI_TGIAN: return "M.DungChung.Table.NS_DM_DVI_TGIAN";
                case Table.NS_DM_DVI_CTAC: return "M.DungChung.Table.NS_DM_DVI_CTAC";
                case Table.NS_DM_LOAI_GTO: return "M.DungChung.Table.NS_DM_LOAI_GTO";
                case Table.NS_DM_BVIEN_KCB: return "M.DungChung.Table.NS_DM_BVIEN_KCB";
                case Table.NS_DM_NHOM_CTV: return "M.DungChung.Table.NS_DM_NHOM_CTV";
                case Table.NS_DM_DU_AN: return "M.DungChung.Table.NS_DM_DU_AN";
                case Table.NS_DM_CHUC_VU_DU_AN: return "M.DungChung.Table.NS_DM_CHUC_VU_DU_AN";
                case Table.NS_DM_CHUC_VU_CTV: return "M.DungChung.Table.NS_DM_CHUC_VU_CTV";
                case Table.NS_DM_PHU_CAP: return "M.DungChung.Table.NS_DM_PHU_CAP";
                case Table.NS_DM_DANH_MUC: return "M.DungChung.Table.NS_DM_DANH_MUC";

                case Table.NS_HO_SO: return "M.DungChung.Table.NS_HO_SO";
                case Table.NS_HO_LSU: return "M.DungChung.Table.NS_HO_LSU";
                case Table.NS_HO_SO_QHE_GDINH: return "M.DungChung.Table.NS_HO_SO_QHE_GDINH";
                case Table.NS_HO_SO_TDO_HVAN: return "M.DungChung.Table.NS_HO_SO_TDO_HVAN";
                case Table.NS_HOP_DONG: return "M.DungChung.Table.NS_HOP_DONG";
                case Table.NS_PHU_CAP: return "M.DungChung.Table.NS_PHU_CAP";
                case Table.NS_QTRINH_CTAC: return "M.DungChung.Table.NS_QTRINH_CTAC";
                case Table.NS_TCHUYEN_CTAC: return "M.DungChung.Table.NS_TCHUYEN_CTAC";
                case Table.NS_THOI_VIEC: return "M.DungChung.Table.NS_THOI_VIEC";
                case Table.NS_BAC_LUONG: return "M.DungChung.Table.NS_BAC_LUONG";
                case Table.NS_LUONG: return "M.DungChung.Table.NS_LUONG";
                case Table.NS_LUONG_DCHINH: return "M.DungChung.Table.NS_LUONG_DCHINH";
                case Table.NS_TINH_LUONG: return "M.DungChung.Table.NS_TINH_LUONG";
                case Table.NS_PHU_CAP_CDINH_CTV: return "M.DungChung.Table.NS_PHU_CAP_CDINH_CTV";
                case Table.NS_PHU_CAP_BSUNG_CTV: return "M.DungChung.Table.NS_PHU_CAP_BSUNG_CTV";
                case Table.NS_TINH_PHU_CAP_CTV: return "M.DungChung.Table.NS_TINH_PHU_CAP_CTV";
                case Table.NS_TTINH: return "M.DungChung.Table.NS_TTINH";
                case Table.NS_TTINH_GTRI: return "M.DungChung.Table.NS_TTINH_GTRI";
                case Table.NS_TTINH_GTRI_LS: return "M.DungChung.Table.NS_TTINH_GTRI_LS";
                case Table.NS_QLY_DU_AN: return "M.DungChung.Table.NS_QLY_DU_AN";

                case Table.TD_HDTC: return "M.DungChung.Table.TD_HDTC";
                case Table.TD_HDTC_TSDB: return "M.DungChung.Table.TD_HDTC_TSDB";
                case Table.TD_HDTD: return "M.DungChung.Table.TD_HDTD";
                case Table.TD_HDTD_LSU: return "M.DungChung.Table.TD_HDTD_LSU";
                case Table.TD_HDTDVM: return "M.DungChung.Table.TD_HDTDVM";
                case Table.TD_HDTDVM_LSU: return "M.DungChung.Table.TD_HDTDVM_LSU";
                case Table.TD_KHOACH: return "M.DungChung.Table.TD_KHOACH";
                case Table.TD_KHOACH_CT: return "M.DungChung.Table.TD_KHOACH_CT";
                case Table.TD_KHOACHVM: return "M.DungChung.Table.TD_KHOACHVM";
                case Table.TD_KHOACHVM_CT: return "M.DungChung.Table.TD_KHOACHVM_CT";
                case Table.TD_KUOC: return "M.DungChung.Table.TD_KUOC";
                case Table.TD_KUOC_LSU: return "M.DungChung.Table.TD_KUOC_LSU";
                case Table.TD_KUOCVM: return "M.DungChung.Table.TD_KUOCVM";
                case Table.TD_KUOCVM_LSU: return "M.DungChung.Table.TD_KUOCVM_LSU";
                case Table.TD_SAN_PHAM: return "M.DungChung.Table.TD_SAN_PHAM";
                case Table.TD_SAN_PHAMTT: return "M.DungChung.Table.TD_SAN_PHAMTT";                    
                case Table.TD_TDUNG_HMUC: return "M.DungChung.Table.TD_TDUNG_HMUC";
                case Table.TD_TSAN_LOAI: return "M.DungChung.Table.TD_TSAN_LOAI";
                case Table.TD_TSDB: return "M.DungChung.Table.TD_TSDB";
                case Table.TD_TTINH: return "M.DungChung.Table.TD_TTINH";
                case Table.TD_TTINH_GTRI: return "M.DungChung.Table.TD_TTINH_GTRI";
                case Table.TD_TTINH_GTRI_LSU: return "M.DungChung.Table.TD_TTINH_GTRI_LSU";
                case Table.TD_VONG_VAY: return "M.DungChung.Table.TD_VONG_VAY";
                case Table.TD_VONG_VAY_CTIET: return "M.DungChung.Table.TD_VONG_VAY_CTIET";
                case Table.TD_TDOI_LSUAT: return "M.DungChung.Table.TD_TDOI_LSUAT";
                case Table.TD_DXVVVM: return "M.DungChung.Table.TD_DXVVVM";
                case Table.TD_DXVVVM_LSU: return "M.DungChung.Table.TD_DXVVVM_LSU";
                case Table.TDTD_SAN_PHAM: return "M.DungChung.Table.TDTD_SAN_PHAM";
                case Table.TDTD_HDTD: return "M.DungChung.Table.TDTD_HDTD";
                case Table.TDTD_KUOC: return "M.DungChung.Table.TDTD_KUOC";
                case Table.TD_KIEM_SOAT_RR: return "M.DungChung.Table.TD_KIEM_SOAT_RR";

                case Table.VKT_HDV_DKY_RGOC: return "M.DungChung.Table.VKT_HDV_DKY_RGOC";

                case Table.TDTD_TSDB: return "M.DungChung.Table.TDTD_TSDB";
                case Table.TDTD_HDTC: return "M.DungChung.Table.TDTD_HDTC";
                case Table.TDTD_HDTC_TSDB: return "M.DungChung.Table.TDTD_HDTC_TSDB";
                default: return "";
            }
        }

        public static string getValue(this Table item)
        {
            switch (item)
            {
                case Table.BC_MAU: return "BC_MAU";
                case Table.BC_MAU_KHAI_THAC: return "BC_MAU_KHAI_THAC";
                case Table.BC_MAU_THAM_SO: return "BC_MAU_THAM_SO";
                case Table.BC_THAM_SO: return "BC_THAM_SO";
                case Table.BL_SAN_PHAM: return "BL_SAN_PHAM";
                case Table.BL_SAN_PHAM_CT: return "BL_SAN_PHAM_CT";
                case Table.BL_TDOI_LSUAT: return "BL_TDOI_LSUAT";
                case Table.BL_TDOI_LSUAT_CT: return "BL_TDOI_LSUAT_CT";
                case Table.BL_TIEN_GUI: return "BL_TIEN_GUI";
                case Table.BL_TIEN_GUI_DCSH: return "BL_TIEN_GUI_DCSH";
                case Table.BL_TIEN_GUI_KHOACH: return "BL_TIEN_GUI_KHOACH";
                case Table.BL_TIEN_GUI_KHOACH_LSU: return "BL_TIEN_GUI_KHOACH_LSU";
                case Table.BL_TIEN_GUI_LSU: return "BL_TIEN_GUI_LSU";
                case Table.BL_TIEN_GUI_TKHOAN: return "BL_TIEN_GUI_TKHOAN";
                case Table.BL_TTINH: return "BL_TTINH";
                case Table.BL_TTINH_GTRI: return "BL_TTINH_GTRI";
                case Table.BL_TTINH_GTRI_LSU: return "BL_TTINH_GTRI_LSU";
                case Table.BL_DKY_RGOC: return "BL_DKY_RGOC";
                case Table.BL_DKY_RGOC_CT: return "BL_DKY_RGOC_CT";
                case Table.CD_NGUON_VON: return "CD_NGUON_VON";
                case Table.CD_NSD: return "CD_NSD";
                case Table.DC_BPHI: return "DC_BPHI";
                case Table.DC_BPHI_CTIET: return "DC_BPHI_CTIET";
                case Table.DC_BPHI_GDICH: return "DC_BPHI_GDICH";
                case Table.DC_CSO_TLAI: return "DC_CSO_TLAI";
                case Table.DC_CSO_TLAI_CTIET: return "DC_CSO_TLAI_CTIET";
                case Table.DC_CT_CTIEU: return "DC_CT_CTIEU";
                case Table.DC_CT_DLIEU: return "DC_CT_DLIEU";
                case Table.DC_CT_DLIEU_DCHIEU: return "DC_CT_DLIEU_DCHIEU";
                case Table.DC_CT_GOC: return "DC_CT_GOC";
                case Table.DC_CT_GOC_CTHUC: return "DC_CT_GOC_CTHUC";
                case Table.DC_CT_GOC_MATKE: return "DC_CT_GOC_MATKE";
                case Table.DC_CT_LOAITKE: return "DC_CT_LOAITKE";
                case Table.DC_CT_MATKE: return "DC_CT_MATKE";
                case Table.DC_CT_MATKE_MANB: return "DC_CT_MATKE_MANB";
                case Table.DC_CT_MAUNL: return "DC_CT_MAUNL";
                case Table.DC_CT_MAUNL_CTIEU: return "DC_CT_MAUNL_CTIEU";
                case Table.DC_CT_MBIEU: return "DC_CT_MBIEU";
                case Table.DC_CT_MBIEU_DLIEU: return "DC_CT_MBIEU_DLIEU";
                case Table.DC_CT_NHOM: return "DC_CT_NHOM";
                case Table.DC_CTRUC: return "DC_CTRUC";
                case Table.DC_CTRUC_CTIET: return "DC_CTRUC_CTIET";
                case Table.DC_HMUC: return "DC_HMUC";
                case Table.DC_HMUC_DTUONG: return "DC_HMUC_DTUONG";
                case Table.DC_HMUC_DTUONG_LSU: return "DC_HMUC_DTUONG_LSU";
                case Table.DC_HMUC_LSU: return "DC_HMUC_LSU";
                case Table.DC_LSUAT: return "DC_LSUAT";
                case Table.DC_LSUAT_CTIET: return "DC_LSUAT_CTIET";
                case Table.DC_NGUOI_QLY: return "DC_NGUOI_QLY";
                case Table.DC_PPHTOAN: return "DC_PPHTOAN";
                case Table.DC_PPHTOAN_BTOAN: return "DC_PPHTOAN_BTOAN";
                case Table.DC_PPHTOAN_BTOAN_KT: return "DC_PPHTOAN_BTOAN_KT";
                case Table.DC_PPHTOAN_DVI: return "DC_PPHTOAN_DVI";
                case Table.DC_PTOA: return "DC_PTOA";
                case Table.DC_PTOA_LSU: return "DC_PTOA_LSU";
                case Table.DC_TSUAT: return "DC_TSUAT";
                case Table.DC_TSUAT_CUM: return "DC_TSUAT_CUM";
                case Table.DC_TTINH: return "DC_TTINH";
                case Table.DC_TTINH_GTRI: return "DC_TTINH_GTRI";
                case Table.DC_TY_GIA: return "DC_TY_GIA";
                case Table.DM_CUM: return "DM_CUM";
                case Table.DM_DIA_BAN: return "DM_DIA_BAN";
                case Table.DM_DMUC_GTRI: return "DM_DMUC_GTRI";
                case Table.DM_DMUC_LOAI: return "DM_DMUC_LOAI";
                case Table.DM_DON_VI: return "DM_DON_VI";
                case Table.DM_DON_VI_CCAU: return "DM_DON_VI_CCAU";
                case Table.DM_KHU_VUC: return "DM_KHU_VUC";
                case Table.DM_NHOM: return "DM_NHOM";
                case Table.DM_PHAN_HE: return "DM_PHAN_HE";
                case Table.DM_PHAN_HE_GD: return "DM_PHAN_HE_GD";
                case Table.DM_PHAN_HE_GD_CNANG: return "DM_PHAN_HE_GD_CNANG";
                case Table.DM_QUOC_GIA: return "DM_QUOC_GIA";
                case Table.DM_TIEN_TE: return "DM_TIEN_TE";
                case Table.DM_TINH_TP: return "DM_TINH_TP";
                case Table.DM_TTINH: return "DM_TTINH";
                case Table.DM_TTINH_GTRI: return "DM_TTINH_GTRI";
                case Table.DM_VUNG_MIEN: return "DM_VUNG_MIEN";
                case Table.DM_DTUONG: return "DM_DTUONG";
                case Table.DM_LOAI_DTUONG: return "DM_LOAI_DTUONG";
                case Table.DM_TCTD: return "DM_TCTD";
                case Table.DM_TCTD_TKHOAN: return "DM_TCTD_TKHOAN";
                case Table.HM_CTIET: return "HM_CTIET";
                case Table.HM_CTIET_LOAITS: return "HM_CTIET_LOAITS";
                case Table.HM_CTIET_TSDB: return "HM_CTIET_TSDB";
                case Table.HM_TONG: return "HM_TONG";
                case Table.HM_TONG_NHOMSP: return "HM_TONG_NHOMSP";
                case Table.HT_CNANG: return "HT_CNANG";
                case Table.HT_CNANG_HDSD: return "HT_CNANG_HDSD";
                case Table.HT_CNANG_TNANG: return "HT_CNANG_TNANG";
                case Table.HT_CNANG_TSO: return "HT_CNANG_TSO";
                case Table.HT_CSO: return "HT_CSO";
                case Table.HT_CSO_TSO: return "HT_CSO_TSO";
                case Table.HT_DATA_CONSTRAINT: return "HT_DATA_CONSTRAINT";
                case Table.HT_DLIEU_RBUOC: return "HT_DLIEU_RBUOC";
                case Table.HT_HANG_DOI: return "HT_HANG_DOI";
                case Table.HT_HANG_DOI_LS: return "HT_HANG_DOI_LS";
                case Table.HT_LICH: return "HT_LICH";
                case Table.HT_LICH_LVIEC: return "HT_LICH_LVIEC";
                case Table.HT_LICH_NGHI: return "HT_LICH_NGHI";
                case Table.HT_LICH_SU: return "HT_LICH_SU";
                case Table.HT_LICH_SU_CT: return "HT_LICH_SU_CT";
                case Table.HT_LSU_GDICH: return "HT_LSU_GDICH";
                case Table.HT_NGAY_LVIEC: return "HT_NGAY_LVIEC";
                case Table.HT_NHNSD: return "HT_NHNSD";
                case Table.HT_NHNSD_HSO: return "HT_NHNSD_HSO";
                case Table.HT_NHNSD_NSD: return "HT_NHNSD_NSD";
                case Table.HT_NSD: return "HT_NSD";
                case Table.HT_NSD_HSO: return "HT_NSD_HSO";
                case Table.HT_PBAN: return "HT_PBAN";
                case Table.HT_PBAN_CTIET: return "HT_PBAN_CTIET";
                case Table.HT_PHIEN_LVIEC: return "HT_PHIEN_LVIEC";
                case Table.HT_PHIEN_LVIEC_LSU: return "HT_PHIEN_LVIEC_LSU";
                case Table.HT_TNANG: return "HT_TNANG";
                case Table.HT_TNGUYEN: return "HT_TNGUYEN";
                case Table.HT_TNGUYEN_KBAO: return "HT_TNGUYEN_KBAO";
                case Table.HT_TNGUYEN_KTHAC: return "HT_TNGUYEN_KTHAC";
                case Table.HT_TNGUYEN_LOAI: return "HT_TNGUYEN_LOAI";
                case Table.HT_TNGUYEN_LOAI_GTRI: return "HT_TNGUYEN_LOAI_GTRI";
                case Table.HT_TSO: return "HT_TSO";
                case Table.HT_TSO_LOAI: return "HT_TSO_LOAI";
                case Table.HT_TTINH: return "HT_TTINH";
                case Table.HT_TTINH_GTRI: return "HT_TTINH_GTRI";
                case Table.HT_TTRINH: return "HT_TTRINH";
                case Table.HT_TVAN: return "HT_TVAN";
                case Table.HT_TVAN_CTIET: return "HT_TVAN_CTIET";
                case Table.KH_CHUYEN_DBAN: return "KH_CHUYEN_DBAN";
                case Table.KH_CHUYEN_DBAN_CTIET: return "KH_CHUYEN_DBAN_CTIET";
                case Table.KH_KHANG_HSO: return "KH_KHANG_HSO";
                case Table.KH_KHANG_HSO_LSU: return "KH_KHANG_HSO_LSU";
                case Table.KH_TTINH: return "KH_TTINH";
                case Table.KH_TTINH_GTRI: return "KH_TTINH_GTRI";
                case Table.KH_TTINH_GTRI_LSU: return "KH_TTINH_GTRI_LSU";
                case Table.KT_BPHI_PLOAI: return "KT_BPHI_PLOAI";
                case Table.KT_DOI_CHIEU: return "KT_DOI_CHIEU";
                case Table.KT_GIAO_DICH: return "KT_GIAO_DICH";
                case Table.KT_GIAO_DICH_PHI: return "KT_GIAO_DICH_PHI";
                case Table.KT_NHOM_PLOAI: return "KT_NHOM_PLOAI";
                case Table.KT_NHOM_PLOAI_LSDU: return "KT_NHOM_PLOAI_LSDU";
                case Table.KT_PHAT_SINH: return "KT_PHAT_SINH";
                case Table.KT_PHAT_SINH_CT: return "KT_PHAT_SINH_CT";
                case Table.KT_PLOAI: return "KT_PLOAI";
                case Table.KT_SO_CAI: return "KT_SO_CAI";
                case Table.KT_SO_CAI_CTHUC: return "KT_SO_CAI_CTHUC";
                case Table.KT_SO_CAI_DNGHIA: return "KT_SO_CAI_DNGHIA";
                case Table.KT_SO_CAI_LOAI: return "KT_SO_CAI_LOAI";
                case Table.KT_TCHI_THOP: return "KT_TCHI_THOP";
                case Table.KT_TKHOAN: return "KT_TKHOAN";
                case Table.KT_TKHOAN_SDU: return "KT_TKHOAN_SDU";
                case Table.KT_TTINH: return "KT_TTINH";
                case Table.KT_TTINH_GTRI: return "KT_TTINH_GTRI";
                case Table.KT_TTINH_GTRI_LSU: return "KT_TTINH_GTRI_LSU";
                case Table.KT_HT_TKTH: return "KT_HT_TKTH";
                case Table.KT_TKTH: return "KT_TKTH";
                case Table.KT_TKTH_PLOAI: return "KT_TKTH_PLOAI";
                case Table.NQ_BKE_TMAT: return "NQ_BKE_TMAT";
                case Table.NQ_BKE_TMAT_CT: return "NQ_BKE_TMAT_CT";
                case Table.NQ_BKE_TMAT_THOP: return "NQ_BKE_TMAT_THOP";
                case Table.NQ_MENH_GIA: return "NQ_MENH_GIA";
                case Table.NS_DM_QUOC_TICH: return "NS_DM_QUOC_TICH";
                case Table.NS_DM_TINH_TP: return "NS_DM_TINH_TP";
                case Table.NS_DM_QUAN_HUYEN: return "NS_DM_QUAN_HUYEN";
                case Table.NS_DM_PHUONG_XA: return "NS_DM_PHUONG_XA";
                case Table.NS_DM_DAN_TOC: return "NS_DM_DAN_TOC";
                case Table.NS_DM_TON_GIAO: return "NS_DM_TON_GIAO";
                case Table.NS_DM_GIOI_TINH: return "NS_DM_GIOI_TINH";
                case Table.NS_DM_TTRANG_HNHAN: return "NS_DM_TTRANG_HNHAN";
                case Table.NS_DM_CU_TRU: return "NS_DM_CU_TRU";
                case Table.NS_DM_HTHUC_LVIEC: return "NS_DM_HTHUC_LVIEC";
                case Table.NS_DM_TDO_HVAN: return "NS_DM_TDO_HVAN";
                case Table.NS_DM_HTHUC_DTAO: return "NS_DM_HTHUC_DTAO";
                case Table.NS_DM_TRUONG_DTAO: return "NS_DM_TRUONG_DTAO";
                case Table.NS_DM_BANG_CAP: return "NS_DM_BANG_CAP";
                case Table.NS_DM_KHOA_DTAO: return "NS_DM_KHOA_DTAO";
                case Table.NS_DM_CNGANH_DTAO: return "NS_DM_CNGANH_DTAO";
                case Table.NS_DM_HOC_HAM: return "NS_DM_HOC_HAM";
                case Table.NS_DM_HOC_VI: return "NS_DM_HOC_VI";
                case Table.NS_DM_TDO_TANH: return "NS_DM_TDO_TANH";
                case Table.NS_DM_TDO_THOC: return "NS_DM_TDO_THOC";
                case Table.NS_DM_TDO_CTRI: return "NS_DM_TDO_CTRI";
                case Table.NS_DM_XEP_LOAI: return "NS_DM_XEP_LOAI";
                case Table.NS_DM_QHE_GDINH: return "NS_DM_QHE_GDINH";
                case Table.NS_DM_KY_NANG: return "NS_DM_KY_NANG";
                case Table.NS_DM_NGHE_NGHIEP: return "NS_DM_NGHE_NGHIEP";
                case Table.NS_DM_CHUC_VU: return "NS_DM_CHUC_VU";
                case Table.NS_DM_LDO_NPHEP: return "NS_DM_LDO_NPHEP";
                case Table.NS_DM_LDO_TVIEC: return "NS_DM_LDO_TVIEC";
                case Table.NS_DM_THAN_HDLD: return "NS_DM_THAN_HDLD";
                case Table.NS_DM_LOAI_HDLD: return "NS_DM_LOAI_HDLD";
                case Table.NS_DM_HTHUC_TLUONG: return "NS_DM_HTHUC_TLUONG";
                case Table.NS_DM_LOAI_TNHAP: return "NS_DM_LOAI_TNHAP";
                case Table.NS_DM_LOAI_CPHI: return "NS_DM_LOAI_CPHI";
                case Table.NS_DM_HTHUC_KTHUONG: return "NS_DM_HTHUC_KTHUONG";
                case Table.NS_DM_HTHUC_KLUAT: return "NS_DM_HTHUC_KLUAT";
                case Table.NS_DM_KHIEU_CCONG: return "NS_DM_KHIEU_CCONG";
                case Table.NS_DM_LOAI_HSO: return "NS_DM_LOAI_HSO";
                case Table.NS_DM_DVI_TGIAN: return "NS_DM_DVI_TGIAN";
                case Table.NS_DM_DVI_CTAC: return "NS_DM_DVI_CTAC";
                case Table.NS_DM_LOAI_GTO: return "NS_DM_LOAI_GTO";
                case Table.NS_DM_BVIEN_KCB: return "NS_DM_BVIEN_KCB";
                case Table.NS_DM_NHOM_CTV: return "NS_DM_NHOM_CTV";
                case Table.NS_DM_DU_AN: return "NS_DM_DU_AN";
                case Table.NS_DM_CHUC_VU_DU_AN: return "NS_DM_CHUC_VU_DU_AN";
                case Table.NS_DM_CHUC_VU_CTV: return "NS_DM_CHUC_VU_CTV";
                case Table.NS_DM_PHU_CAP: return "NS_DM_PHU_CAP";
                case Table.NS_DM_DANH_MUC: return "NS_DM_DANH_MUC";

                case Table.NS_HO_SO: return "NS_HO_SO";
                case Table.NS_HO_LSU: return "NS_HO_LSU";
                case Table.NS_HO_SO_QHE_GDINH: return "NS_HO_SO_QHE_GDINH";
                case Table.NS_HO_SO_TDO_HVAN: return "NS_HO_SO_TDO_HVAN";
                case Table.NS_HOP_DONG: return "NS_HOP_DONG";
                case Table.NS_PHU_CAP: return "NS_PHU_CAP";
                case Table.NS_QTRINH_CTAC: return "NS_QTRINH_CTAC";
                case Table.NS_TCHUYEN_CTAC: return "NS_TCHUYEN_CTAC";
                case Table.NS_THOI_VIEC: return "NS_THOI_VIEC";
                case Table.NS_BAC_LUONG: return "NS_BAC_LUONG";
                case Table.NS_LUONG: return "NS_LUONG";
                case Table.NS_LUONG_DCHINH: return "NS_LUONG_DCHINH";
                case Table.NS_TINH_LUONG: return "NS_TINH_LUONG";
                case Table.NS_PHU_CAP_CDINH_CTV: return "NS_PHU_CAP_CDINH_CTV";
                case Table.NS_PHU_CAP_BSUNG_CTV: return "NS_PHU_CAP_BSUNG_CTV";
                case Table.NS_TINH_PHU_CAP_CTV: return "NS_TINH_PHU_CAP_CTV";
                case Table.NS_TTINH: return "NS_TTINH";
                case Table.NS_TTINH_GTRI: return "NS_TTINH_GTRI";
                case Table.NS_TTINH_GTRI_LS: return "NS_TTINH_GTRI_LS";
                case Table.NS_QLY_DU_AN: return "NS_QLY_DU_AN";

                case Table.TD_HDTC: return "TD_HDTC";
                case Table.TD_HDTC_TSDB: return "TD_HDTC_TSDB";
                case Table.TD_HDTD: return "TD_HDTD";
                case Table.TD_HDTD_LSU: return "TD_HDTD_LSU";
                case Table.TD_HDTDVM: return "TD_HDTDVM";
                case Table.TD_HDTDVM_LSU: return "TD_HDTDVM_LSU";
                case Table.TD_KHOACH: return "TD_KHOACH";
                case Table.TD_KHOACH_CT: return "TD_KHOACH_CT";
                case Table.TD_KHOACHVM: return "TD_KHOACHVM";
                case Table.TD_KHOACHVM_CT: return "TD_KHOACHVM_CT";
                case Table.TD_KUOC: return "TD_KUOC";
                case Table.TD_KUOC_LSU: return "TD_KUOC_LSU";
                case Table.TD_KUOCVM: return "TD_KUOCVM";
                case Table.TD_KUOCVM_LSU: return "TD_KUOCVM_LSU";
                case Table.TD_SAN_PHAM: return "TD_SAN_PHAM";
                case Table.TD_SAN_PHAMTT: return "TD_SAN_PHAMTT";                    
                case Table.TD_TDUNG_HMUC: return "TD_TDUNG_HMUC";
                case Table.TD_TSAN_LOAI: return "TD_TSAN_LOAI";
                case Table.TD_TSDB: return "TD_TSDB";
                case Table.TD_TTINH: return "TD_TTINH";
                case Table.TD_TTINH_GTRI: return "TD_TTINH_GTRI";
                case Table.TD_TTINH_GTRI_LSU: return "TD_TTINH_GTRI_LSU";
                case Table.TD_VONG_VAY: return "TD_VONG_VAY";
                case Table.TD_VONG_VAY_CTIET: return "TD_VONG_VAY_CTIET";
                case Table.TD_TDOI_LSUAT: return "TD_TDOI_LSUAT";
                case Table.TD_DXVVVM: return "TD_DXVVVM";
                case Table.TD_DXVVVM_LSU: return "TD_DXVVVM_LSU";
                case Table.TDTD_SAN_PHAM: return "TDTD_SAN_PHAM";
                case Table.TDTD_HDTD: return "TDTD_HDTD";
                case Table.TDTD_KUOC: return "TDTD_KUOC";
                case Table.TD_KIEM_SOAT_RR: return "TD_KIEM_SOAT_RR";
                case Table.TS_DM_NHOM_TSCD: return "TS_DM_NHOM_TSCD";
                case Table.TS_TANG: return "TS_TANG";

                case Table.VKT_HDV_DKY_RGOC: return "VKT_HDV_DKY_RGOC";

                case Table.TDTD_TSDB: return "TDTD_TSDB";
                case Table.TDTD_HDTC: return "TDTD_HDTC";
                case Table.TDTD_HDTC_TSDB: return "TDTD_HDTC_TSDB";
                default: return "";
            }
        }

        public enum Action
        {
            DANG_NHAP,
            DANG_XUAT,
            TRUY_VAN,
            XEM,
            XEM_CHI_TIET,
            THEM,
            SUA,
            XOA,
            LUU,
            SAO_CHEP,
            CAT,
            DAN,
            TRINH_DUYET,
            LUU_TAM,
            NHAN_BAN,
            DONG,
            DUYET,
            THOAI_DUYET,
            TU_CHOI_DUYET,
            SU_DUNG,
            KHONG_SU_DUNG,
            LOC,
            TIM_KIEM,
            XEM_TRUOC,
            IN,
            IN_CHUNG,
            DONG_BO,
            TRO_GIUP,
            KET_NOI,
            HUY_KET_NOI,
            LAY_LAI,
            POPUP,
            XUAT_DU_LIEU,
            NHAP_DU_LIEU,
            TONG_HOP,
            TAO_DU_LIEU,
            SINH_DU_LIEU,
            TINH_TOAN,
            TINH_TOAN2,
            TINH_TOAN3,
            TINH_TOAN4,
            TINH_TOAN5,
            TINH_TOAN_LAI_SUAT,
            TINH_TOAN_KY_HAN,
            TINH_TOAN_SO_TIEN_VAY,
            TINH_TOAN_DU_THU_TRONG_HAN,
            TINH_TOAN_DU_THU_QUA_HAN,
            TINH_TOAN_DU_CHI,
            TINH_TOAN_LICH_TRA_NO,
            TINH_TOAN_TRICH_LAP_DU_PHONG,
            TINH_TOAN_TRICH_LAP_DU_PHONG_CHUNG,
            TINH_TOAN_TRICH_LAP_DU_PHONG_CU_THE,
            THUC_HIEN,
            XU_LY,
            CAU_HINH,
            TAI_XUONG,
            TAI_LEN,
            DON_DEP,
            SAO_LUU,
            PHUC_HOI,
            BANG_KE,
            TOAN_QUYEN,
            RESET_PASS,
            LOAD,
            LOAD_DATA,
            GET_BY_ID,
            GET_BY_MA,
            KIEM_TRA
        }
        public static string getValue(this Action action)
        {
            switch (action)
            {
                case Action.DANG_NHAP: return "Login";
                case Action.DANG_XUAT: return "Logout";
                case Action.TRUY_VAN: return "Query";
                case Action.XEM: return "View";
                case Action.XEM_CHI_TIET: return "ViewDetail";
                case Action.THEM: return "Add";
                case Action.SUA: return "Modify";
                case Action.XOA: return "Delete";
                case Action.LUU: return "Save";
                case Action.SAO_CHEP: return "Copy";
                case Action.CAT: return "Cut";
                case Action.DAN: return "Paste";
                case Action.TRINH_DUYET: return "Submit";
                case Action.LUU_TAM: return "Hold";
                case Action.NHAN_BAN: return "Clone";
                case Action.DONG: return "Close";
                case Action.DUYET: return "Approve";
                case Action.THOAI_DUYET: return "Cancel";
                case Action.TU_CHOI_DUYET: return "Refuse";
                case Action.SU_DUNG: return "Using";
                case Action.KHONG_SU_DUNG: return "NotUsing";
                case Action.LOC: return "Filter";
                case Action.TIM_KIEM: return "Search";
                case Action.XEM_TRUOC: return "Preview";
                case Action.IN: return "Print";
                case Action.IN_CHUNG: return "Print";
                case Action.DONG_BO: return "Synch";
                case Action.TRO_GIUP: return "Help";
                case Action.KET_NOI: return "Connect";
                case Action.HUY_KET_NOI: return "Disconnect";
                case Action.LAY_LAI: return "Reload";
                case Action.POPUP: return "Popup";
                case Action.XUAT_DU_LIEU: return "Export";
                case Action.NHAP_DU_LIEU: return "Import";
                case Action.TONG_HOP: return "Collect";
                case Action.TAO_DU_LIEU: return "Make";
                case Action.SINH_DU_LIEU: return "Generate";
                case Action.TINH_TOAN: return "Caculate";
                case Action.THUC_HIEN: return "Execute";
                case Action.XU_LY: return "Process";
                case Action.CAU_HINH: return "Config";
                case Action.TAI_XUONG: return "Download";
                case Action.TAI_LEN: return "Upload";
                case Action.DON_DEP: return "Clean";
                case Action.SAO_LUU: return "Backup";
                case Action.PHUC_HOI: return "Restore";
                case Action.BANG_KE: return "CashStmt";
                case Action.TOAN_QUYEN: return "FullControl";
                case Action.RESET_PASS: return "Resetpass";
                case Action.LOAD: return "Load";
                case Action.LOAD_DATA: return "LoadData";
                case Action.GET_BY_ID: return "GetByID";
                case Action.GET_BY_MA: return "GetByMa";
                case Action.KIEM_TRA: return "KiemTra";

                default: return "";
            }
        }

        public enum DanhMuc
        {
            DON_VI_HACH_TOAN,
            TRANG_THAI_HE_THONG,
            TRANG_THAI_XU_LY_NVU,
            LOAI_HOP_DONG_VAY,
            LOAI_LAI_SUAT,
            PHAN_LOAI_DONG_TIEN,
            PHAM_VI_GIAO_DICH,
            CHAT_LIEU_DONG_TIEN,
            NGUYEN_NHAN_CHAM_TRA,
            NGANH_KINH_TE,
            MUC_DICH_VAY_VON,
            NGHE_NGHIEP,
            TRINH_DO_HOC_VAN,
            TINH_TRANG_HON_NHAN,
            CHUC_VU,
            TON_GIAO,
            DAN_TOC,
            GIOI_TINH,
            PHUONG_TIEN_DI_LAI,
            TINH_CHAT_CU_TRU,
            LOAI_HINH_TO_CHUC,
            MOI_QUAN_HE,
            NGUON_THU_NHAP,
            TAN_SUAT_HOP,
            CAC_KHOAN_CHI_TIEU,
            TAI_SAN_HO_GIA_DINH,
            NGUON_TAO_DU_LIEU,
            TRANG_THAI_BAN_GHI,
            TRANG_THAI_NGHIEP_VU,
            LOAI_DIA_BAN,
            LOAI_THUOC_TINH,
            LOAI_DIEU_KHIEN,
            CO_KHONG,
            TRANG_THAI_TTINH,
            KIEU_DU_LIEU,
            PHAN_LOAI_NGUOI_SU_DUNG,
            TRANG_THAI_DOI_MAT_KHAU,
            PHAM_VI_ANH_HUONG,
            PHAN_LOAI_CHUC_NANG,
            LOAI_DTUONG_KTHAC_TNGUYEN,
            MA_XU_LY_DANG_NHAP,
            HTHUC_BTHANG,
            LOAI_LICH,
            THU_TRONG_TUAN,
            DDANG_NGAY_THANG_NAM,
            LOAI_GIA_TRI,
            DONG_TIEN_CAC_NUOC,
            BCAO_DVI_THOI_GIAN,
            KY_HAN_DVI_TINH,
            THI_TRUONG_TIEN_TE,
            HINH_THUC_NIEM_YET,
            LOAI_HAN_MUC,
            HINH_THUC_HAN_MUC,
            NGUON_HTHANH_HMUC,
            LOAI_BIEU_PHI,
            DTUONG_PHONG_TOA,
            LOAI_THOI_HAN_PTOA,
            LOAI_XLY_HTOAN,
            TTHAI_SDUNG_PPHTOAN,
            LOAI_CHUNG_TU,
            DTUONG_ADUNG_CSO_TLAI,
            DON_VI_THOI_GIAN,
            LHOP_DVI_TINH_TOAN,
            LOAI_DOI_TUONG_CTIEU,
            LOAI_KHACH_HANG,
            CHUC_VU_CUM_NHOM,
            VAO_RA_NHOM_CUM,
            LY_DO_VAO_RA,
            LOAI_SO_DU,
            LOAI_DTUONG_SDUNG_TKHOAN,
            NHOM_SPHAM_TKIEM,
            HINH_THUC_TRA_LAI,
            HINH_THUC_TRA_GOC,
            DVI_TINH_LSUAT,
            PPHAP_TINH_LSUAT,
            PPHAP_TINH_RGOC,
            CSO_TINH_TLE_RGOC,
            PPHAP_TINH_GTIEN,
            CACH_THUC_GTIEN,
            CSO_TINH_TLE_GTIEN,
            PPHAP_TPHI_TGUI_TTOAN,
            NGUYEN_TAC_LAM_TRON,
            DVI_TINH_KY_HAN,
            HINH_THUC_GIAO_DICH,
            CHI_THI_DAO_HAN,
            TINH_CHAT_VONG_VAY,
            TOAN_TU_VONG_VAY,
            NHOM_SPHAM_TDUNG,
            LOAI_SAN_PHAM_TDUNG,
            THOI_HAN_VAY_VON,
            NGUON_VON_VAY,
            PHUONG_THUC_TINH_LAI,
            HINH_THUC_THE_CHAP,
            NGUON_GOC_TAI_SAN,
            LOAI_DTUONG_TDOI_LSUAT,
            PHUONG_THUC_VAY,
            HTHUC_ADUNG_LSUAT,
            HTHUC_LAP_KHOACH,
            TRANG_THAI_GIAI_NGAN,
            NHOM_NO,
            CO_SO_TINH_LAI,
            TAN_SUAT,
            DU_NO_TINH_ST_TOI_THIEU,
            DU_NO_GOC_TINH_ST_GUI_KY,
            HINH_THUC_GD,
            NGAY_GD_ROI_VAO_NGAY_NGHI,
            PHUONG_THUC_PHONG_TOA,
            LOAI_TAI_KHOAN_PVI,
            LOAI_TAI_KHOAN_DTUONG,
            LOAI_TAI_KHOAN_MDSD,
            TINH_CHAT_TK,
            LOAI_CAU_TRUC_TK,
            DANH_MUC_TP_CAU_TRUC,
            CHUNG_TU_KE_TOAN,
            PHUONG_PHAP_DIEU_CHINH,
            DINH_KHOAN,
            DANH_MUC_DOI_TUONG,
            KICH_CO_NHA,
            TUONG_NHA,
            CHAT_LOP_MAI_NHA,
            MUC_DO_BEN_CHAC,
            PHUONG_TIEN_THONG_TIN,
            LOAI_HINH_ANH,
            DU_LIEU_HINH_ANH,
            VAI_TRO_TRONG_GD,
            THOI_GIAN_CONG_TAC,
            LOAI_GIAY_TO,
            TIEU_CHI_XH_TIN_DUNG,
            TIEU_CHI_XH_NGHEO,
            TINH_TRANG_SUC_KHOE,
            LOAI_TSDB,
            NGUYEN_NHAN_THAY_DOI_LTN,
            PHAM_VI_DIEU_CHINH_LS,
            TINH_TRANG_CU_TRU,
            QUOC_GIA,
            DON_VI,
            THUOC_TINH_DON_VI,
            CUM,
            DANH_MUC_PHAN_HE,
            DANH_MUC_CHUC_NANG,
            LOAI_XEP_HANG_TD,
            LOAI_XEP_HANG_NGHEO,
            PHUONG_THUC_TRA_LAI,
            LOAI_TNGUYEN,
            LOAI_GDICH_PTOA,
            LOAI_GD_DONG_MO_TK,
            TRANG_THAI_NGUOI_DUNG,
            HINH_THUC_GIAO_DICH_KT,
            HTHUC_BTHANG_PHI,
            NGHIEP_VU_KHOA_SO,
            NGHIEP_VU_MO_SO,
            NGHIEP_VU_CUOI_NGAY_TW,
            NGON_NGU,
            BCAO_TDUNG_TIEU_CHI_NHOM,
            BCAO_TDUNG_SAP_XEP,
            LOAI_SO_PHU,
            LOAI_SO_DU_TKHOAN,
            DINH_DANG_BAO_CAO,
            HINH_THUC_NOP_KQUY,
            PP_TINH_NOP_KQUY,
            TS_LOAI_TS,
            LOAI_NHOM,
            TINH_TRANG_NGHEO,
            TINH_TRANG_GIA_DINH,
            QUAN_HE_KT,
            TRANG_THAI_CAP_TIN_DUNG,
            PHUONG_PHAP_TINH_LAI,
            CAN_CU_TINH_LAI_QH,
            CACH_TINH_SO_NGAY_TD,
            TRINH_DO_VAN_HOA,
            HINH_THUC_RUT_GOC,
            LOAI_DON_XVV_TDTD,
            LOAI_CONG_TY,
            LOAI_EMAIL
        }

        public static string getValue(this DanhMuc danhMuc)
        {
            switch (danhMuc)
            {
                case DanhMuc.DON_VI_HACH_TOAN: return "DON_VI_HACH_TOAN";
                case DanhMuc.TRANG_THAI_HE_THONG: return "TRANG_THAI_HE_THONG";
                case DanhMuc.TRANG_THAI_XU_LY_NVU: return "TRANG_THAI_XU_LY_NVU";
                case DanhMuc.LOAI_HOP_DONG_VAY: return "LOAI_HOP_DONG_VAY";
                case DanhMuc.LOAI_LAI_SUAT: return "LOAI_LAI_SUAT";
                case DanhMuc.PHAN_LOAI_DONG_TIEN: return "PHAN_LOAI_DONG_TIEN";
                case DanhMuc.PHAM_VI_GIAO_DICH: return "PHAM_VI_GIAO_DICH";
                case DanhMuc.CHAT_LIEU_DONG_TIEN: return "CHAT_LIEU_DONG_TIEN";
                case DanhMuc.NGUYEN_NHAN_CHAM_TRA: return "NGUYEN_NHAN_CHAM_TRA";
                case DanhMuc.NGANH_KINH_TE: return "NGANH_KINH_TE";
                case DanhMuc.MUC_DICH_VAY_VON: return "MUC_DICH_VAY_VON";
                case DanhMuc.NGHE_NGHIEP: return "NGHE_NGHIEP";
                case DanhMuc.TRINH_DO_HOC_VAN: return "TRINH_DO_HOC_VAN";
                case DanhMuc.TINH_TRANG_HON_NHAN: return "TINH_TRANG_HON_NHAN";
                case DanhMuc.CHUC_VU: return "CHUC_VU";
                case DanhMuc.TON_GIAO: return "TON_GIAO";
                case DanhMuc.DAN_TOC: return "DAN_TOC";
                case DanhMuc.GIOI_TINH: return "GIOI_TINH";
                case DanhMuc.PHUONG_TIEN_DI_LAI: return "PHUONG_TIEN_DI_LAI";
                case DanhMuc.TINH_CHAT_CU_TRU: return "TINH_CHAT_CU_TRU";
                case DanhMuc.LOAI_HINH_TO_CHUC: return "LOAI_HINH_TO_CHUC";
                case DanhMuc.MOI_QUAN_HE: return "MOI_QUAN_HE";
                case DanhMuc.NGUON_THU_NHAP: return "NGUON_THU_NHAP";
                case DanhMuc.TAN_SUAT_HOP: return "TAN_SUAT_HOP";
                case DanhMuc.CAC_KHOAN_CHI_TIEU: return "CAC_KHOAN_CHI_TIEU";
                case DanhMuc.TAI_SAN_HO_GIA_DINH: return "TAI_SAN_HO_GIA_DINH";
                case DanhMuc.NGUON_TAO_DU_LIEU: return "NGUON_TAO_DU_LIEU";
                case DanhMuc.TRANG_THAI_BAN_GHI: return "TRANG_THAI_BAN_GHI";
                case DanhMuc.TRANG_THAI_NGHIEP_VU: return "TRANG_THAI_NGHIEP_VU";
                case DanhMuc.LOAI_DIA_BAN: return "LOAI_DIA_BAN";
                case DanhMuc.LOAI_THUOC_TINH: return "LOAI_THUOC_TINH";
                case DanhMuc.LOAI_DIEU_KHIEN: return "LOAI_DIEU_KHIEN";
                case DanhMuc.CO_KHONG: return "CO_KHONG";
                case DanhMuc.TRANG_THAI_TTINH: return "TRANG_THAI_TTINH";
                case DanhMuc.KIEU_DU_LIEU: return "KIEU_DU_LIEU";
                case DanhMuc.PHAN_LOAI_NGUOI_SU_DUNG: return "PHAN_LOAI_NGUOI_SU_DUNG";
                case DanhMuc.TRANG_THAI_DOI_MAT_KHAU: return "TRANG_THAI_DOI_MAT_KHAU";
                case DanhMuc.PHAM_VI_ANH_HUONG: return "PHAM_VI_ANH_HUONG";
                case DanhMuc.PHAN_LOAI_CHUC_NANG: return "PHAN_LOAI_CHUC_NANG";
                case DanhMuc.LOAI_DTUONG_KTHAC_TNGUYEN: return "LOAI_DTUONG_KTHAC_TNGUYEN";
                case DanhMuc.MA_XU_LY_DANG_NHAP: return "MA_XU_LY_DANG_NHAP";
                case DanhMuc.HTHUC_BTHANG: return "HTHUC_BTHANG";
                case DanhMuc.LOAI_LICH: return "LOAI_LICH";
                case DanhMuc.THU_TRONG_TUAN: return "THU_TRONG_TUAN";
                case DanhMuc.DDANG_NGAY_THANG_NAM: return "DDANG_NGAY_THANG_NAM";
                case DanhMuc.LOAI_GIA_TRI: return "LOAI_GIA_TRI";
                case DanhMuc.DONG_TIEN_CAC_NUOC: return "DONG_TIEN_CAC_NUOC";
                case DanhMuc.BCAO_DVI_THOI_GIAN: return "BCAO_DVI_THOI_GIAN";
                case DanhMuc.KY_HAN_DVI_TINH: return "KY_HAN_DVI_TINH";
                case DanhMuc.THI_TRUONG_TIEN_TE: return "THI_TRUONG_TIEN_TE";
                case DanhMuc.HINH_THUC_NIEM_YET: return "HINH_THUC_NIEM_YET";
                case DanhMuc.LOAI_HAN_MUC: return "LOAI_HAN_MUC ";
                case DanhMuc.HINH_THUC_HAN_MUC: return "HINH_THUC_HAN_MUC";
                case DanhMuc.NGUON_HTHANH_HMUC: return "NGUON_HTHANH_HMUC";
                case DanhMuc.LOAI_BIEU_PHI: return "LOAI_BIEU_PHI";
                case DanhMuc.DTUONG_PHONG_TOA: return "DTUONG_PHONG_TOA";
                case DanhMuc.LOAI_THOI_HAN_PTOA: return "LOAI_THOI_HAN_PTOA";
                case DanhMuc.LOAI_XLY_HTOAN: return "LOAI_XLY_HTOAN";
                case DanhMuc.TTHAI_SDUNG_PPHTOAN: return "TTHAI_SDUNG_PPHTOAN";
                case DanhMuc.LOAI_CHUNG_TU: return "LOAI_CHUNG_TU";
                case DanhMuc.DTUONG_ADUNG_CSO_TLAI: return "DTUONG_ADUNG_CSO_TLAI";
                case DanhMuc.DON_VI_THOI_GIAN: return "DON_VI_THOI_GIAN";
                case DanhMuc.LHOP_DVI_TINH_TOAN: return "LHOP_DVI_TINH_TOAN";
                case DanhMuc.LOAI_DOI_TUONG_CTIEU: return "LOAI_DOI_TUONG_CTIEU";
                case DanhMuc.LOAI_KHACH_HANG: return "LOAI_KHACH_HANG";
                case DanhMuc.CHUC_VU_CUM_NHOM: return "CHUC_VU_CUM_NHOM";
                case DanhMuc.VAO_RA_NHOM_CUM: return "VAO_RA_NHOM_CUM";
                case DanhMuc.LY_DO_VAO_RA: return "LY_DO_VAO_RA";
                case DanhMuc.LOAI_SO_DU: return "LOAI_SO_DU";
                case DanhMuc.LOAI_DTUONG_SDUNG_TKHOAN: return "LOAI_DTUONG_SDUNG_TKHOAN";
                case DanhMuc.NHOM_SPHAM_TKIEM: return "NHOM_SPHAM_TKIEM";
                case DanhMuc.HINH_THUC_TRA_LAI: return "HINH_THUC_TRA_LAI";
                case DanhMuc.HINH_THUC_TRA_GOC: return "HINH_THUC_TRA_GOC";
                case DanhMuc.DVI_TINH_LSUAT: return "DVI_TINH_LSUAT";
                case DanhMuc.PPHAP_TINH_LSUAT: return "PPHAP_TINH_LSUAT";
                case DanhMuc.PPHAP_TINH_RGOC: return "PPHAP_TINH _RGOC";
                case DanhMuc.CSO_TINH_TLE_RGOC: return "CSO_TINH_TLE_RGOC";
                case DanhMuc.PPHAP_TINH_GTIEN: return "PPHAP_TINH_GTIEN";
                case DanhMuc.CACH_THUC_GTIEN: return "CACH_THUC_GTIEN";
                case DanhMuc.CSO_TINH_TLE_GTIEN: return "CSO_TINH_TLE_GTIEN";
                case DanhMuc.PPHAP_TPHI_TGUI_TTOAN: return "PPHAP_TPHI_TGUI_TTOAN";
                case DanhMuc.NGUYEN_TAC_LAM_TRON: return "NGUYEN_TAC_LAM_TRON";
                case DanhMuc.DVI_TINH_KY_HAN: return "DVI_TINH_KY_HAN";
                case DanhMuc.HINH_THUC_GIAO_DICH: return "HINH_THUC_GIAO_DICH ";
                case DanhMuc.CHI_THI_DAO_HAN: return "CHI_THI_DAO_HAN";
                case DanhMuc.TINH_CHAT_VONG_VAY: return "TINH_CHAT_VONG_VAY";
                case DanhMuc.TOAN_TU_VONG_VAY: return "TOAN_TU_VONG_VAY";
                case DanhMuc.NHOM_SPHAM_TDUNG: return "NHOM_SPHAM_T DUNG";
                case DanhMuc.LOAI_SAN_PHAM_TDUNG: return "LOAI_SAN_PHAM_TDUNG";
                case DanhMuc.THOI_HAN_VAY_VON: return "THOI_HAN_VAY_VON";
                case DanhMuc.NGUON_VON_VAY: return "NGUON_VON_VAY";
                case DanhMuc.PHUONG_THUC_TINH_LAI: return "PHUONG_THUC_TINH_LAI";
                case DanhMuc.HINH_THUC_THE_CHAP: return "HINH_THUC_THE_CHAP";
                case DanhMuc.NGUON_GOC_TAI_SAN: return "NGUON_GOC_TAI_SAN";
                case DanhMuc.LOAI_DTUONG_TDOI_LSUAT: return "LOAI_DTUONG_TDOI_LSUAT";
                case DanhMuc.PHUONG_THUC_VAY: return "PHUONG_THUC_VAY";
                case DanhMuc.HTHUC_ADUNG_LSUAT: return "HTHUC_ADUNG_LSUAT";
                case DanhMuc.HTHUC_LAP_KHOACH: return "HTHUC_LAP_KHOACH";
                case DanhMuc.TRANG_THAI_GIAI_NGAN: return "TRANG_THAI_GIAI_NGAN";
                case DanhMuc.NHOM_NO: return "NHOM_NO";
                case DanhMuc.CO_SO_TINH_LAI: return "CO_SO_TINH_LAI";
                case DanhMuc.TAN_SUAT: return "TAN_SUAT";
                case DanhMuc.DU_NO_TINH_ST_TOI_THIEU: return "DU_NO_TINH_ST_TOI_THIEU";
                case DanhMuc.DU_NO_GOC_TINH_ST_GUI_KY: return "DU_NO_GOC_TINH_ST_GUI_KY";
                case DanhMuc.HINH_THUC_GD: return "HINH_THUC_GD";
                case DanhMuc.NGAY_GD_ROI_VAO_NGAY_NGHI: return "NGAY_GD_ROI_VAO_NGAY_NGHI";
                case DanhMuc.PHUONG_THUC_PHONG_TOA: return "PHUONG_THUC_PHONG_TOA";
                case DanhMuc.LOAI_TAI_KHOAN_PVI: return "LOAI_TAI_KHOAN_PVI";
                case DanhMuc.LOAI_TAI_KHOAN_DTUONG: return "LOAI_TAI_KHOAN_DTUONG";
                case DanhMuc.LOAI_TAI_KHOAN_MDSD: return "LOAI_TAI_KHOAN_MDSD";
                case DanhMuc.TINH_CHAT_TK: return "TINH_CHAT_TK";
                case DanhMuc.LOAI_CAU_TRUC_TK: return "LOAI_CAU_TRUC_TK";
                case DanhMuc.DANH_MUC_TP_CAU_TRUC: return "DANH_MUC_TP_CAU_TRUC";
                case DanhMuc.CHUNG_TU_KE_TOAN: return "CHUNG_TU_KE_TOAN";
                case DanhMuc.PHUONG_PHAP_DIEU_CHINH: return "PHUONG_PHAP_DIEU_CHINH";
                case DanhMuc.DINH_KHOAN: return "DINH_KHOAN";
                case DanhMuc.DANH_MUC_DOI_TUONG: return "DANH_MUC_DOI_TUONG";
                case DanhMuc.KICH_CO_NHA: return "KICH_CO_NHA";
                case DanhMuc.TUONG_NHA: return "TUONG_NHA";
                case DanhMuc.CHAT_LOP_MAI_NHA: return "CHAT_LOP_MAI_NHA";
                case DanhMuc.MUC_DO_BEN_CHAC: return "MUC_DO_BEN_CHAC";
                case DanhMuc.PHUONG_TIEN_THONG_TIN: return "PHUONG_TIEN_THONG_TIN";
                case DanhMuc.LOAI_HINH_ANH: return "LOAI_HINH_ANH";
                case DanhMuc.DU_LIEU_HINH_ANH: return "DU_LIEU_HINH_ANH";
                case DanhMuc.VAI_TRO_TRONG_GD: return "VAI_TRO_TRONG_GD";
                case DanhMuc.THOI_GIAN_CONG_TAC: return "THOI_GIAN_CONG_TAC";
                case DanhMuc.LOAI_GIAY_TO: return "LOAI_GIAY_TO";
                case DanhMuc.TIEU_CHI_XH_TIN_DUNG: return "TIEU_CHI_XH_TIN_DUNG";
                case DanhMuc.TIEU_CHI_XH_NGHEO: return "TIEU_CHI_XH_NGHEO";
                case DanhMuc.TINH_TRANG_SUC_KHOE: return "TINH_TRANG_SUC_KHOE";
                case DanhMuc.LOAI_TSDB: return "LOAI_TSDB";
                case DanhMuc.NGUYEN_NHAN_THAY_DOI_LTN: return "NGUYEN_NHAN_THAY_DOI_LTN";
                case DanhMuc.PHAM_VI_DIEU_CHINH_LS: return "PHAM_VI_DIEU_CHINH_LS";
                case DanhMuc.TINH_TRANG_CU_TRU: return "TINH_TRANG_CU_TRU";
                case DanhMuc.QUOC_GIA: return "QUOC_GIA";
                case DanhMuc.DON_VI: return "DON_VI";
                case DanhMuc.THUOC_TINH_DON_VI: return "THUOC_TINH_DON_VI";
                case DanhMuc.CUM: return "CUM";
                case DanhMuc.DANH_MUC_PHAN_HE: return "DANH_MUC_PHAN_HE";
                case DanhMuc.DANH_MUC_CHUC_NANG: return "DANH_MUC_CHUC_NANG";
                case DanhMuc.LOAI_XEP_HANG_TD: return "LOAI_XEP_HANG_TD";
                case DanhMuc.LOAI_XEP_HANG_NGHEO: return "LOAI_XEP_HANG_NGHEO";
                case DanhMuc.PHUONG_THUC_TRA_LAI: return "PHUONG_THUC_TRA_LAI";
                case DanhMuc.LOAI_TNGUYEN: return "LOAI_TNGUYEN";
                case DanhMuc.LOAI_GDICH_PTOA: return "LOAI_GDICH_PTOA";
                case DanhMuc.LOAI_GD_DONG_MO_TK: return "LOAI_GD_DONG_MO_TK";
                case DanhMuc.TRANG_THAI_NGUOI_DUNG: return "TRANG_THAI_NGUOI_DUNG";
                case DanhMuc.HINH_THUC_GIAO_DICH_KT: return "HINH_THUC_GIAO_DICH_KT";
                case DanhMuc.HTHUC_BTHANG_PHI: return "HTHUC_BTHANG_PHI";
                case DanhMuc.NGHIEP_VU_KHOA_SO: return "NGHIEP_VU_KHOA_SO";
                case DanhMuc.NGHIEP_VU_MO_SO: return "NGHIEP_VU_MO_SO";
                case DanhMuc.NGHIEP_VU_CUOI_NGAY_TW: return "NGHIEP_VU_CUOI_NGAY_TW";
                case DanhMuc.NGON_NGU: return "NGON_NGU";
                case DanhMuc.BCAO_TDUNG_TIEU_CHI_NHOM: return "BCAO_TDUNG_TIEU_CHI_NHOM";
                case DanhMuc.BCAO_TDUNG_SAP_XEP: return "BCAO_TDUNG_SAP_XEP";
                case DanhMuc.LOAI_SO_PHU: return "LOAI_SO_PHU";
                case DanhMuc.LOAI_SO_DU_TKHOAN: return "LOAI_SO_DU_TKHOAN";
                case DanhMuc.DINH_DANG_BAO_CAO: return "DINH_DANG_BAO_CAO";
                case DanhMuc.HINH_THUC_NOP_KQUY: return "HINH_THUC_NOP_KQUY";
                case DanhMuc.PP_TINH_NOP_KQUY: return "PP_TINH_NOP_KQUY";
                case DanhMuc.TS_LOAI_TS: return "TS_LOAI_TS";
                case DanhMuc.LOAI_NHOM: return "LOAI_NHOM";
                case DanhMuc.TINH_TRANG_NGHEO: return "TINH_TRANG_NGHEO";
                case DanhMuc.TINH_TRANG_GIA_DINH: return "TINH_TRANG_GIA_DINH";
                case DanhMuc.QUAN_HE_KT: return "QUAN_HE_KT";
                case DanhMuc.TRANG_THAI_CAP_TIN_DUNG: return "TRANG_THAI_CAP_TIN_DUNG";
                case DanhMuc.PHUONG_PHAP_TINH_LAI: return "PHUONG_PHAP_TINH_LAI";
                case DanhMuc.CAN_CU_TINH_LAI_QH: return "CAN_CU_TINH_LAI_QH";
                case DanhMuc.CACH_TINH_SO_NGAY_TD: return "CACH_TINH_SO_NGAY_TD";
                case DanhMuc.TRINH_DO_VAN_HOA: return "TRINH_DO_VAN_HOA";
                case DanhMuc.HINH_THUC_RUT_GOC: return "HINH_THUC_RUT_GOC";
                case DanhMuc.LOAI_DON_XVV_TDTD: return "LOAI_DON_XVV_TDTD";
                case DanhMuc.LOAI_CONG_TY: return "LOAI_CONG_TY";
                case DanhMuc.LOAI_EMAIL: return "LOAI_EMAIL";

                default: return "";
            }
        }

        public enum Status
        {

        }

        /// <summary>
        /// Danh sách truy vấn trong hệ thống
        /// Dùng chung cho popup, combobox, danh mục
        /// </summary> 
        public enum DanhSachTruyVan
        {
            // Popup
            POPUP_DS_KHACHHANG,
            POPUP_DS_KHACHHANG_BAOCAO,
            POPUP_DM_DONVI,
            POPUP_DM_TINHTHANH,
            POPUP_DM_DIABAN,
            POPUP_DM_CUM,
            POPUP_DM_LSUAT,
            POPUP_HT_NSD,
            POPUP_HT_NHNSD,
            POPUP_DS_KHACH_HANG,
            POPUP_DS_SAN_PHAM_HDV,
            POPUP_DS_TAI_KHOAN,
            POPUP_HT_TSO_LOAI,
            POPUP_DS_TSDB_KHACHHANG,
            POPUP_DS_SO_TGUI_HDV,
            POPUP_DS_TSDB_KHEUOC,
            POPUP_DS_KHEUOC,
            POPUP_DS_CAN_BO_TUNG,
            POPUP_DS_HDTD,
            POPUP_KT_PLOAI,
            POPUP_TP_CAUTRUC,
            POPUP_CAU_TRUC,
            POPUP_DIEN_GIAI,
            POPUP_DM_PHE_GDICH,
            POPUP_PLOAI_TAO_TK,
            POPUP_LOAIGD_KE_TOAN,
            POPUP_TKHOAN_CTIET,
            POPUP_DS_DOI_TUONG,
            POPUP_DS_TKHOAN_THEO_DAU,
            POPUP_DS_TKHOAN_GTHEM_TIEN,
            POPUP_TAI_KHOAN_KHACH_HANG,
            POPUP_TAI_KHOAN_NOI_BO,
            POPUP_DS_GDICH_KETOAN,
            POPUP_DS_CUM,
            POPUP_KHACH_HANG,
            POPUP_DS_KH_HOA_DON_TK,
            POPUP_DM_LSUAT_HDV,
            POPUP_DS_SO_TGUI_TSDB,
            POPUP_DONVITHEOMACHA,
            POPUP_TKHOAN_CTIET_HOANTIEPQUY,
            POPUP_TKHOAN_CTIET_CONG_NO,
            POPUP_DS_HOSO_NVIEN,
            POPUP_DS_HOSO_NVIEN_DVI,
            POPUP_DS_KHACHHANG_PGD,
            POPUP_DS_KHACHHANG_NHOM,
            POPUP_DS_HOSO_NVIEN_CTIET,
            POPUP_DS_HOSO_NHAN_SU_QLY,
            POPUP_DS_HOSO_NHAN_SU_CTV,
            POPUP_DS_DOI_TUONG_GIAO_DICH,
            POPUP_DS_TAI_SAN,
            POPUP_DS_PHU_CAP,
            POPUP_DS_LUONG_HSO,
            POPUP_DS_LUONG_DIEUCHINH,
            POPUP_DS_PCAP_HSO,
            POPUP_DS_GDICH_TUNG,
            POPUP_DS_TINHLUONG_HSO,
            POPUP_DS_KH_HOA_DON_TK_00,
            POPUP_DS_HMUC_TONG,
            POPUP_DS_SAN_PHAM_HMUC,
            POPUP_DS_NHOM_TSDB,
            POPUP_DS_HM_NHOM_TSDB,
            POPUP_DS_KHE_UOC_KH,
            POPUP_DS_KSKH_DON_XIN_VAY_VON,
            POPUP_DS_TCTD,

            // ComboBox
            COMBOBOX_MIEN,
            COMBOBOX_VUNG,
            COMBOBOX_VUNG_ALL,
            COMBOBOX_TINHTP,
            COMBOBOX_TINHTP_DVGIAODICH,
            COMBOBOX_DIABAN,
            COMBOBOX_DIABAN_CTIET,
            COMBOBOX_KHUVUC,
            COMBOBOX_KHUVUC_ALL,
            COMBOBOX_CUM,
            COMBOBOX_NHOM,
            COMBOBOX_DONVI,
            COMBOBOX_DMUC_LOAI,
            COMBOBOX_DMUC_GTRI,
            COMBOBOX_LOAISANPHAM,
            COMBOBOX_DMUC,
            COMBOBOX_DONVITHEOLOAI,
            COMBOBOX_DONVI_THEOPVI,
            COMBOBOX_LICHHOP,
            COMBOBOX_QUOCGIA,
            COMBOBOX_LOAITIEN,
            COMBOBOX_NHOMVONGVAY,
            COMBOBOX_LOAI_LSUAT,
            COMBOBOX_NSD,
            COMBOBOX_HINHTHUCSD,
            COMBOBOX_LOAITS,
            COMBOBOX_PHANHE,
            COMBOBOX_DONGTRACHNHIEM,
            COMBOBOX_TIENTE,
            COMBOBOX_TINH_CHAT_TK,
            COMBOBOX_CHI_NHANH,
            COMBOBOX_KHOANG_GTRI_TAISAN,
            COMBOBOX_LOAITHAMSO,
            COMBOBOX_LOAICHUNGTU_NGOAIBANG,
            COMBOBOX_CNHANH,
            COMBOBOX_PHONG_GD,
            COMBOBOX_LOAIPHI,
            COMBOBOX_DONVITHEOMACHA,
            COMBOBOX_COSOTINHLAI,
            COMBOBOX_CUM_ALL,
            COMBOBOX_TK_KHONG_KY_HAN,
            COMBOBOX_SAN_PHAM_TK,
            COMBOBOX_KY_HIEU,
            COMBOBOX_PGDLIST,
            COMBOBOX_CNHANHLIST,
            COMBOBOX_CUMLIST,
            COMBOBOX_BAO_CAO_TKE,
            COMBOBOX_NHAN_SU,
            COMBOBOX_KY_THU,
            COMBOBOX_KUOC_KHANG,
            COMBOBOX_NAM_TCHINH,
            COMBOBOX_NHOM_ALL,
            COMBOBOX_NS_QUOC_GIA,
            COMBOBOX_NS_TINH_TP,
            COMBOBOX_NS_QUAN_HUYEN,
            COMBOBOX_NS_PHUONG_XA,
            COMBOBOX_NS_GIOI_TINH,
            COMBOBOX_NS_PHONG_BAN,
            COMBOBOX_NS_CHUC_VU,
            COMBOBOX_NS_HTHUC_LVIEC,
            COMBOBOX_TK_LOAI_DTUONG,
            COMBOBOX_TNANG,
            COMBOBOX_TS_LOAI_TS,
            COMBOBOX_TS_PHAN_LOAI_TS,
            COMBOBOX_TS_NHOM_CHA,
            COMBOBOX_DVI_HTOAN,
            COMBOBOX_HINHTHUC_XULYNO,
            COMBOBOX_DONVI_TKHOAN,
            COMBOBOX_HMUC_VONG_VAY_TD,
            COMBOBOX_SAN_PHAM_TD_DBAN,
            COMBOBOX_DOT_THU_PHAT,
            COMBOBOX_DMUC_NHAN_SU,
            COMBOBOX_NS_NHOM_CTV,
            COMBOBOX_NGUOI_BAN_GIAO,
            COMBOBOX_DVI_SDUNG,
            COMBOBOX_DMUC_NHAN_SU_THEO_BANG,
            COMBOBOX_DMUC_CBO_TIN_DUNG,
            COMBOBOX_KHUVUCLIST,
            COMBOBOX_NHAN_SU_LIST,
            COMBOBOX_SAN_PHAM_TD_LIST,
            COMBOBOX_SAN_PHAM_TDLIST,
            COMBOBOX_SAN_PHAM_TD,
            COMBOBOX_CUM_KVUC_LIST,
            COMBOBOX_QUY_TRONG_NAM,
            COMBOBOX_DU_AN,
            COMBOBOX_NGUON_VON,
            COMBOBOX_NGUON_VON_DVI,
            COMBOBOX_NGAY_PHATVON_CUM,            
            COMBOBOX_HE_THONG_TK,
            COMBOBOX_NGAY_PHATVON_CUM_ALL,
            COMBOBOX_SAN_PHAM_TDTT,
            COMBOBOX_SYS_JOB_BY_CAT,
            COMBOBOX_DS_NSD,
            COMBOBOX_NGUON_VON_CT,

            // Danh sách
            DM_TINHTP,
            DM_DUNGCHUNG,
            DM_DIABAN,
            DM_CUM,
            DM_NHOM,
            DM_PHANHE_GD,
            DM_CUM_TREE,
            DM_CUM_GRID,
            DM_NHOM_TREE,
            DM_NHOM_GRID,
            DM_KHUVUC,
            DM_TAN_SUAT,
            DM_QUOCGIA,
            DM_TIENTE,
            DM_TREE_DONVI
        }

        public static string getValue(this DanhSachTruyVan danhSachTruyVan)
        {
            switch (danhSachTruyVan)
            {
                case DanhSachTruyVan.POPUP_DS_KHACHHANG: return "POPUP_DS_KHACHHANG";
                case DanhSachTruyVan.POPUP_DS_KHACHHANG_BAOCAO: return "POPUP_DS_KHACHHANG_BAOCAO";
                case DanhSachTruyVan.POPUP_DM_DONVI: return "POPUP_DM_DONVI";
                case DanhSachTruyVan.POPUP_DM_TINHTHANH: return "POPUP_DM_TINHTHANH";
                case DanhSachTruyVan.POPUP_DM_DIABAN: return "POPUP_DM_DIABAN";
                case DanhSachTruyVan.POPUP_DM_CUM: return "POPUP_DM_CUM";
                case DanhSachTruyVan.POPUP_DM_LSUAT: return "POPUP_DM_LSUAT";
                case DanhSachTruyVan.POPUP_HT_NSD: return "POPUP_HT_NSD";
                case DanhSachTruyVan.POPUP_HT_NHNSD: return "POPUP_HT_NHNSD";
                case DanhSachTruyVan.POPUP_DS_KHACH_HANG: return "POPUP_DS_KHACH_HANG";
                case DanhSachTruyVan.POPUP_DS_SAN_PHAM_HDV: return "POPUP_DS_SAN_PHAM_HDV";
                case DanhSachTruyVan.POPUP_DS_TAI_KHOAN: return "POPUP_DS_TAI_KHOAN";
                case DanhSachTruyVan.POPUP_HT_TSO_LOAI: return "POPUP_HT_TSO_LOAI";
                case DanhSachTruyVan.POPUP_DS_TSDB_KHACHHANG: return "POPUP_DS_TSDB_KHACHHANG";
                case DanhSachTruyVan.POPUP_DS_SO_TGUI_HDV: return "POPUP_DS_SO_TGUI_HDV";
                case DanhSachTruyVan.POPUP_DS_TSDB_KHEUOC: return "POPUP_DS_TSDB_KHEUOC";
                case DanhSachTruyVan.POPUP_DS_KHEUOC: return "POPUP_DS_KHEUOC";
                case DanhSachTruyVan.POPUP_DS_CAN_BO_TUNG: return "POPUP_DS_CAN_BO_TUNG";
                case DanhSachTruyVan.POPUP_DS_HDTD: return "POPUP_DS_HDTD";
                case DanhSachTruyVan.POPUP_KT_PLOAI: return "POPUP_KT_PLOAI";
                case DanhSachTruyVan.POPUP_TP_CAUTRUC: return "POPUP_TP_CAUTRUC";
                case DanhSachTruyVan.POPUP_CAU_TRUC: return "POPUP_CAU_TRUC";
                case DanhSachTruyVan.POPUP_DIEN_GIAI: return "POPUP_DIEN_GIAI";
                case DanhSachTruyVan.POPUP_DM_PHE_GDICH: return "POPUP_DM_PHE_GDICH";
                case DanhSachTruyVan.POPUP_PLOAI_TAO_TK: return "POPUP_PLOAI_TAO_TK";
                case DanhSachTruyVan.POPUP_LOAIGD_KE_TOAN: return "POPUP_LOAIGD_KE_TOAN";
                case DanhSachTruyVan.POPUP_TKHOAN_CTIET: return "POPUP_TKHOAN_CTIET";
                case DanhSachTruyVan.POPUP_DS_DOI_TUONG: return "POPUP_DS_DOI_TUONG";
                case DanhSachTruyVan.POPUP_DS_TKHOAN_THEO_DAU: return "POPUP_DS_TKHOAN_THEO_DAU";
                case DanhSachTruyVan.POPUP_DS_TKHOAN_GTHEM_TIEN: return "POPUP_DS_TKHOAN_GTHEM_TIEN";
                case DanhSachTruyVan.POPUP_TAI_KHOAN_KHACH_HANG: return "POPUP_TAI_KHOAN_KHACH_HANG";
                case DanhSachTruyVan.POPUP_TAI_KHOAN_NOI_BO: return "POPUP_TAI_KHOAN_NOI_BO";
                case DanhSachTruyVan.POPUP_DS_GDICH_KETOAN: return "POPUP_DS_GDICH_KETOAN";
                case DanhSachTruyVan.POPUP_DS_CUM: return "POPUP_DS_CUM";
                case DanhSachTruyVan.POPUP_KHACH_HANG: return "POPUP_KHACH_HANG";
                case DanhSachTruyVan.POPUP_DS_KH_HOA_DON_TK: return "POPUP_DS_KH_HOA_DON_TK";
                case DanhSachTruyVan.POPUP_DM_LSUAT_HDV: return "POPUP_DM_LSUAT_HDV";
                case DanhSachTruyVan.POPUP_DS_SO_TGUI_TSDB: return "POPUP_DS_SO_TGUI_TSDB";
                case DanhSachTruyVan.POPUP_DONVITHEOMACHA: return "POPUP_DONVITHEOMACHA";
                case DanhSachTruyVan.POPUP_TKHOAN_CTIET_HOANTIEPQUY: return "POPUP_TKHOAN_CTIET_HOANTIEPQUY";
                case DanhSachTruyVan.POPUP_TKHOAN_CTIET_CONG_NO: return "POPUP_TKHOAN_CTIET_CONG_NO";
                case DanhSachTruyVan.POPUP_DS_HOSO_NVIEN: return "POPUP_DS_HOSO_NVIEN";
                case DanhSachTruyVan.POPUP_DS_HOSO_NVIEN_DVI: return "POPUP_DS_HOSO_NVIEN_DVI";
                case DanhSachTruyVan.POPUP_DS_KHACHHANG_PGD: return "POPUP_DS_KHACHHANG_PGD";
                case DanhSachTruyVan.POPUP_DS_KHACHHANG_NHOM: return "POPUP_DS_KHACHHANG_NHOM";
                case DanhSachTruyVan.POPUP_DS_HOSO_NVIEN_CTIET: return "POPUP_DS_HOSO_NVIEN_CTIET";
                case DanhSachTruyVan.POPUP_DS_HOSO_NHAN_SU_QLY: return "POPUP_DS_HOSO_NHAN_SU_QLY";
                case DanhSachTruyVan.POPUP_DS_HOSO_NHAN_SU_CTV: return "POPUP_DS_HOSO_NHAN_SU_CTV";
                case DanhSachTruyVan.POPUP_DS_DOI_TUONG_GIAO_DICH: return "POPUP_DS_DOI_TUONG_GIAO_DICH";
                case DanhSachTruyVan.POPUP_DS_TAI_SAN: return "POPUP_DS_TAI_SAN";
                case DanhSachTruyVan.POPUP_DS_PHU_CAP: return "POPUP_DS_PHU_CAP";
                case DanhSachTruyVan.POPUP_DS_LUONG_HSO: return "POPUP_DS_LUONG_HSO";
                case DanhSachTruyVan.POPUP_DS_LUONG_DIEUCHINH: return "POPUP_DS_LUONG_DIEUCHINH";
                case DanhSachTruyVan.POPUP_DS_PCAP_HSO: return "POPUP_DS_PCAP_HSO";
                case DanhSachTruyVan.POPUP_DS_GDICH_TUNG: return "POPUP_DS_GDICH_TUNG";
                case DanhSachTruyVan.POPUP_DS_TINHLUONG_HSO: return "POPUP_DS_TINHLUONG_HSO";
                case DanhSachTruyVan.POPUP_DS_KH_HOA_DON_TK_00: return "POPUP_DS_KH_HOA_DON_TK_00";
                case DanhSachTruyVan.POPUP_DS_HMUC_TONG: return "POPUP_DS_HMUC_TONG";
                case DanhSachTruyVan.POPUP_DS_SAN_PHAM_HMUC: return "POPUP_DS_SAN_PHAM_HMUC";
                case DanhSachTruyVan.POPUP_DS_NHOM_TSDB: return "POPUP_DS_NHOM_TSDB";
                case DanhSachTruyVan.POPUP_DS_HM_NHOM_TSDB: return "POPUP_DS_HM_NHOM_TSDB";
                case DanhSachTruyVan.POPUP_DS_KHE_UOC_KH: return "POPUP_DS_KHE_UOC_KH";
                case DanhSachTruyVan.POPUP_DS_KSKH_DON_XIN_VAY_VON: return "POPUP_DS_KSKH_DON_XIN_VAY_VON";
                case DanhSachTruyVan.POPUP_DS_TCTD: return "POPUP_DS_TCTD";
                    

                case DanhSachTruyVan.COMBOBOX_MIEN: return "COMBOBOX_MIEN";
                case DanhSachTruyVan.COMBOBOX_VUNG: return "COMBOBOX_VUNG";
                case DanhSachTruyVan.COMBOBOX_VUNG_ALL: return "COMBOBOX_VUNG_ALL";
                case DanhSachTruyVan.COMBOBOX_TINHTP: return "COMBOBOX_TINHTP";
                case DanhSachTruyVan.COMBOBOX_TINHTP_DVGIAODICH: return "COMBOBOX_TINHTP_DVGIAODICH";
                case DanhSachTruyVan.COMBOBOX_DIABAN: return "COMBOBOX_DIABAN";
                case DanhSachTruyVan.COMBOBOX_DIABAN_CTIET: return "COMBOBOX_DIABAN_CTIET";
                case DanhSachTruyVan.COMBOBOX_KHUVUC: return "COMBOBOX_KHUVUC";
                case DanhSachTruyVan.COMBOBOX_KHUVUC_ALL: return "COMBOBOX_KHUVUC_ALL";
                case DanhSachTruyVan.COMBOBOX_CUM: return "COMBOBOX_CUM";
                case DanhSachTruyVan.COMBOBOX_NHOM: return "COMBOBOX_NHOM";
                case DanhSachTruyVan.COMBOBOX_DONVI: return "COMBOBOX_DONVI";
                case DanhSachTruyVan.COMBOBOX_DMUC_LOAI: return "COMBOBOX_DMUC_LOAI";
                case DanhSachTruyVan.COMBOBOX_DMUC_GTRI: return "COMBOBOX_DMUC_GTRI";
                case DanhSachTruyVan.COMBOBOX_LOAISANPHAM: return "COMBOBOX_LOAISANPHAM";
                case DanhSachTruyVan.COMBOBOX_DMUC: return "COMBOBOX_DMUC";
                case DanhSachTruyVan.COMBOBOX_DONVITHEOLOAI: return "COMBOBOX_DONVITHEOLOAI";
                case DanhSachTruyVan.COMBOBOX_DONVI_THEOPVI: return "COMBOBOX_DONVI_THEOPVI";
                case DanhSachTruyVan.COMBOBOX_LICHHOP: return "COMBOBOX_LICHHOP";
                case DanhSachTruyVan.COMBOBOX_QUOCGIA: return "COMBOBOX_QUOCGIA";
                case DanhSachTruyVan.COMBOBOX_LOAITIEN: return "COMBOBOX_LOAITIEN";
                case DanhSachTruyVan.COMBOBOX_NHOMVONGVAY: return "COMBOBOX_NHOMVONGVAY";
                case DanhSachTruyVan.COMBOBOX_LOAI_LSUAT: return "COMBOBOX_LOAI_LSUAT";
                case DanhSachTruyVan.COMBOBOX_NSD: return "COMBOBOX_NSD";
                case DanhSachTruyVan.COMBOBOX_HINHTHUCSD: return "COMBOBOX_HINHTHUCSD";
                case DanhSachTruyVan.COMBOBOX_LOAITS: return "COMBOBOX_LOAITS";
                case DanhSachTruyVan.COMBOBOX_PHANHE: return "COMBOBOX_PHANHE";
                case DanhSachTruyVan.COMBOBOX_DONGTRACHNHIEM: return "COMBOBOX_DONGTRACHNHIEM";
                case DanhSachTruyVan.COMBOBOX_TIENTE: return "COMBOBOX_TIENTE";
                case DanhSachTruyVan.COMBOBOX_TINH_CHAT_TK: return "COMBOBOX_TINH_CHAT_TK";
                case DanhSachTruyVan.COMBOBOX_CHI_NHANH: return "COMBOBOX_CHI_NHANH";
                case DanhSachTruyVan.COMBOBOX_KHOANG_GTRI_TAISAN: return "COMBOBOX_KHOANG_GTRI_TAISAN";
                case DanhSachTruyVan.COMBOBOX_LOAITHAMSO: return "COMBOBOX_LOAITHAMSO";
                case DanhSachTruyVan.COMBOBOX_LOAICHUNGTU_NGOAIBANG: return "COMBOBOX_LOAICHUNGTU_NGOAIBANG";
                case DanhSachTruyVan.COMBOBOX_CNHANH: return "COMBOBOX_CNHANH";
                case DanhSachTruyVan.COMBOBOX_PHONG_GD: return "COMBOBOX_PHONG_GD";
                case DanhSachTruyVan.COMBOBOX_LOAIPHI: return "COMBOBOX_LOAIPHI";
                case DanhSachTruyVan.COMBOBOX_DONVITHEOMACHA: return "COMBOBOX_DONVITHEOMACHA";
                case DanhSachTruyVan.COMBOBOX_COSOTINHLAI: return "COMBOBOX_COSOTINHLAI";
                case DanhSachTruyVan.COMBOBOX_CUM_ALL: return "COMBOBOX_CUM_ALL";
                case DanhSachTruyVan.COMBOBOX_TK_KHONG_KY_HAN: return "COMBOBOX_TK_KHONG_KY_HAN";
                case DanhSachTruyVan.COMBOBOX_SAN_PHAM_TK: return "COMBOBOX_SAN_PHAM_TK";
                case DanhSachTruyVan.COMBOBOX_KY_HIEU: return "COMBOBOX_KY_HIEU";
                case DanhSachTruyVan.COMBOBOX_PGDLIST: return "COMBOBOX_PGDLIST";
                case DanhSachTruyVan.COMBOBOX_CNHANHLIST: return "COMBOBOX_CNHANHLIST";
                case DanhSachTruyVan.COMBOBOX_CUMLIST: return "COMBOBOX_CUMLIST";
                case DanhSachTruyVan.COMBOBOX_BAO_CAO_TKE: return "COMBOBOX_BAO_CAO_TKE";
                case DanhSachTruyVan.COMBOBOX_NHAN_SU: return "COMBOBOX_NHAN_SU";
                case DanhSachTruyVan.COMBOBOX_KY_THU: return "COMBOBOX_KY_THU";
                case DanhSachTruyVan.COMBOBOX_KUOC_KHANG: return "COMBOBOX_KUOC_KHANG";
                case DanhSachTruyVan.COMBOBOX_NAM_TCHINH: return "COMBOBOX_NAM_TCHINH";
                case DanhSachTruyVan.COMBOBOX_NHOM_ALL: return "COMBOBOX_NHOM_ALL";
                case DanhSachTruyVan.COMBOBOX_NS_QUOC_GIA: return "COMBOBOX_NS_QUOC_GIA";
                case DanhSachTruyVan.COMBOBOX_NS_TINH_TP: return "COMBOBOX_NS_TINH_TP";
                case DanhSachTruyVan.COMBOBOX_NS_QUAN_HUYEN: return "COMBOBOX_NS_QUAN_HUYEN";
                case DanhSachTruyVan.COMBOBOX_NS_PHUONG_XA: return "COMBOBOX_NS_PHUONG_XA";
                case DanhSachTruyVan.COMBOBOX_NS_GIOI_TINH: return "COMBOBOX_NS_GIOI_TINH";
                case DanhSachTruyVan.COMBOBOX_NS_PHONG_BAN: return "COMBOBOX_NS_PHONG_BAN";
                case DanhSachTruyVan.COMBOBOX_NS_CHUC_VU: return "COMBOBOX_NS_CHUC_VU";
                case DanhSachTruyVan.COMBOBOX_NS_HTHUC_LVIEC: return "COMBOBOX_NS_HTHUC_LVIEC";
                case DanhSachTruyVan.COMBOBOX_TK_LOAI_DTUONG: return "COMBOBOX_TK_LOAI_DTUONG";
                case DanhSachTruyVan.COMBOBOX_TNANG: return "COMBOBOX_TNANG";
                case DanhSachTruyVan.COMBOBOX_TS_LOAI_TS: return "COMBOBOX_TS_LOAI_TS";
                case DanhSachTruyVan.COMBOBOX_TS_PHAN_LOAI_TS: return "COMBOBOX_TS_PHAN_LOAI_TS";
                case DanhSachTruyVan.COMBOBOX_TS_NHOM_CHA: return "COMBOBOX_TS_NHOM_CHA";
                case DanhSachTruyVan.COMBOBOX_DVI_HTOAN: return "COMBOBOX_DVI_HTOAN";
                case DanhSachTruyVan.COMBOBOX_HINHTHUC_XULYNO: return "COMBOBOX_HINHTHUC_XULYNO";
                case DanhSachTruyVan.COMBOBOX_DONVI_TKHOAN: return "COMBOBOX_DONVI_TKHOAN";
                case DanhSachTruyVan.COMBOBOX_HMUC_VONG_VAY_TD: return "COMBOBOX_HMUC_VONG_VAY_TD";
                case DanhSachTruyVan.COMBOBOX_SAN_PHAM_TD_DBAN: return "COMBOBOX_SAN_PHAM_TD_DBAN";
                case DanhSachTruyVan.COMBOBOX_DOT_THU_PHAT: return "COMBOBOX_DOT_THU_PHAT";
                case DanhSachTruyVan.COMBOBOX_DMUC_NHAN_SU: return "COMBOBOX_DMUC_NHAN_SU";
                case DanhSachTruyVan.COMBOBOX_NS_NHOM_CTV: return "COMBOBOX_NS_NHOM_CTV";
                case DanhSachTruyVan.COMBOBOX_NGUOI_BAN_GIAO: return "COMBOBOX_NGUOI_BAN_GIAO";
                case DanhSachTruyVan.COMBOBOX_DVI_SDUNG: return "COMBOBOX_DVI_SDUNG";
                case DanhSachTruyVan.COMBOBOX_DMUC_NHAN_SU_THEO_BANG: return "COMBOBOX_DMUC_NHAN_SU_THEO_BANG";
                case DanhSachTruyVan.COMBOBOX_DMUC_CBO_TIN_DUNG: return "COMBOBOX_DMUC_CBO_TIN_DUNG";
                case DanhSachTruyVan.COMBOBOX_KHUVUCLIST: return "COMBOBOX_KHUVUCLIST";
                case DanhSachTruyVan.COMBOBOX_NHAN_SU_LIST: return "COMBOBOX_NHAN_SU_LIST";
                case DanhSachTruyVan.COMBOBOX_SAN_PHAM_TD_LIST: return "COMBOBOX_SAN_PHAM_TD_LIST";
                case DanhSachTruyVan.COMBOBOX_SAN_PHAM_TDLIST: return "COMBOBOX_SAN_PHAM_TDLIST";
                case DanhSachTruyVan.COMBOBOX_SAN_PHAM_TD: return "COMBOBOX_SAN_PHAM_TD";
                case DanhSachTruyVan.COMBOBOX_CUM_KVUC_LIST: return "COMBOBOX_CUM_KVUC_LIST";
                case DanhSachTruyVan.COMBOBOX_QUY_TRONG_NAM: return "COMBOBOX_QUY_TRONG_NAM";
                case DanhSachTruyVan.COMBOBOX_DU_AN: return "COMBOBOX_DU_AN";
                case DanhSachTruyVan.COMBOBOX_NGUON_VON: return "COMBOBOX_NGUON_VON";
                case DanhSachTruyVan.COMBOBOX_NGUON_VON_DVI: return "COMBOBOX_NGUON_VON_DVI";
                case DanhSachTruyVan.COMBOBOX_NGAY_PHATVON_CUM: return "COMBOBOX_NGAY_PHATVON_CUM";                
                case DanhSachTruyVan.COMBOBOX_HE_THONG_TK: return "COMBOBOX_HE_THONG_TK";
                case DanhSachTruyVan.COMBOBOX_NGAY_PHATVON_CUM_ALL: return "COMBOBOX_NGAY_PHATVON_CUM_ALL";
                case DanhSachTruyVan.COMBOBOX_SAN_PHAM_TDTT: return "COMBOBOX_SAN_PHAM_TDTT";
                case DanhSachTruyVan.COMBOBOX_SYS_JOB_BY_CAT: return "COMBOBOX_SYS_JOB_BY_CAT";
                case DanhSachTruyVan.COMBOBOX_DS_NSD: return "COMBOBOX_DS_NSD";
                case DanhSachTruyVan.COMBOBOX_NGUON_VON_CT: return "COMBOBOX_NGUON_VON_CT";

                case DanhSachTruyVan.DM_TINHTP: return "DM_TINHTP";
                case DanhSachTruyVan.DM_DUNGCHUNG: return "DM_DUNGCHUNG";
                case DanhSachTruyVan.DM_DIABAN: return "DM_DIABAN";
                case DanhSachTruyVan.DM_CUM: return "DM_CUM";
                case DanhSachTruyVan.DM_NHOM: return "DM_NHOM";
                case DanhSachTruyVan.DM_PHANHE_GD: return "DM_PHANHE_GD";
                case DanhSachTruyVan.DM_CUM_TREE: return "DM_CUM_TREE";
                case DanhSachTruyVan.DM_CUM_GRID: return "DM_CUM_GRID";
                case DanhSachTruyVan.DM_NHOM_TREE: return "DM_NHOM_TREE";
                case DanhSachTruyVan.DM_NHOM_GRID: return "DM_NHOM_GRID";
                case DanhSachTruyVan.DM_KHUVUC: return "DM_KHUVUC";
                case DanhSachTruyVan.DM_TAN_SUAT: return "DM_TAN_SUAT";
                case DanhSachTruyVan.DM_QUOCGIA: return "DM_QUOCGIA";
                case DanhSachTruyVan.DM_TIENTE: return "DM_TIENTE";
                case DanhSachTruyVan.DM_TREE_DONVI: return "DM_TREE_DONVI";
                default: return "";
            }
        }


        public enum DanhSachBaoCaoTheoGiaoDich
        {
            KHTV_PHIEU_KHAOSAT,
            KHTV_PHIEU_DANHGIA,
            KHTV_PHIEU_XEPHANG,
            GDKT_IN_GIAO_DICH,
            GDKT_PHIEU_THU,
            GDKT_PHIEU_CHI,
            GDKT_PHIEU_HACH_TOAN,
            HDVO_SO_TKCKH,
            TDVM_HDTD,
            TDVM_KUOCVM,
            TDVM_KUOCVM_KEHOACH,
            TDVM_DS_KHANG_NHANVON,
            TDVM_HOA_DON_THU_TIEN_KY,
            BHTH_PHIEU_YEU_CAU_BH,
            TDVM_KUOCVM_00,
            NSTL_HOP_DONG_HOC_VIEC,
            NSTL_HOP_DONG_THU_VIEC,
            NSTL_HOP_DONG_LAO_DONG,
            TDVM_KUOCVM_NHAN_NO_00,
            TDVM_KUOCVM_PHAN_KY_00,
            GDKT_PHIEU_UY_NHIEM_CHI

        };
        public static string layMaBaoCao(this DanhSachBaoCaoTheoGiaoDich danhSachBaoCao)
        {
            switch (danhSachBaoCao)
            {
                case DanhSachBaoCaoTheoGiaoDich.KHTV_PHIEU_KHAOSAT: return "MF_RT_KHTV_PHIEU_KHAOSAT";
                case DanhSachBaoCaoTheoGiaoDich.KHTV_PHIEU_DANHGIA: return "MF_RT_KHTV_PHIEU_DANHGIA";
                case DanhSachBaoCaoTheoGiaoDich.KHTV_PHIEU_XEPHANG: return "KHTV_PHIEU_XEP_HANG_TD";
                case DanhSachBaoCaoTheoGiaoDich.GDKT_IN_GIAO_DICH: return "GDKT_IN_GIAO_DICH";
                case DanhSachBaoCaoTheoGiaoDich.GDKT_PHIEU_THU: return "MF_RT_GDKT_PHIEU_THU";
                case DanhSachBaoCaoTheoGiaoDich.GDKT_PHIEU_CHI: return "MF_RT_GDKT_PHIEU_CHI";
                case DanhSachBaoCaoTheoGiaoDich.GDKT_PHIEU_HACH_TOAN: return "MF_RT_GDKT_PHIEU_HACH_TOAN";
                case DanhSachBaoCaoTheoGiaoDich.HDVO_SO_TKCKH: return "MF_RT_HDVO_SO_TKCKH";
                case DanhSachBaoCaoTheoGiaoDich.TDVM_HDTD: return "MF_RT_TDVM_HDTD";
                case DanhSachBaoCaoTheoGiaoDich.TDVM_KUOCVM: return "MF_RT_TDVM_KUOCVM";
                case DanhSachBaoCaoTheoGiaoDich.TDVM_KUOCVM_KEHOACH: return "MF_RT_TDVM_KUOCVM_KEHOACH";
                case DanhSachBaoCaoTheoGiaoDich.TDVM_DS_KHANG_NHANVON: return "MF_RT_TDVM_DS_KHANG_NHANVON";
                case DanhSachBaoCaoTheoGiaoDich.TDVM_HOA_DON_THU_TIEN_KY: return "TDVM_HOA_DON_THU_TIEN_KY";
                case DanhSachBaoCaoTheoGiaoDich.BHTH_PHIEU_YEU_CAU_BH: return "BHTH_PHIEU_YEU_CAU_BAO_VE_VON_VAY";
                case DanhSachBaoCaoTheoGiaoDich.TDVM_KUOCVM_00: return "MF_RT_TDVM_KUOCVM_00";
                case DanhSachBaoCaoTheoGiaoDich.NSTL_HOP_DONG_HOC_VIEC: return "MF_RT_NSTL_HOP_DONG_HOC_VIEC";
                case DanhSachBaoCaoTheoGiaoDich.NSTL_HOP_DONG_THU_VIEC: return "MF_RT_NSTL_HOP_DONG_THU_VIEC";
                case DanhSachBaoCaoTheoGiaoDich.NSTL_HOP_DONG_LAO_DONG: return "MF_RT_NSTL_HOP_DONG_LAO_DONG";
                case DanhSachBaoCaoTheoGiaoDich.TDVM_KUOCVM_NHAN_NO_00: return "MF_RT_TDVM_KUOCVM_NHAN_NO_00";
                case DanhSachBaoCaoTheoGiaoDich.TDVM_KUOCVM_PHAN_KY_00: return "MF_RT_TDVM_KUOCVM_PHAN_KY_00";
                case DanhSachBaoCaoTheoGiaoDich.GDKT_PHIEU_UY_NHIEM_CHI: return "MF_RT_GDKT_PHIEU_UY_NHIEM_CHI";
                default: return "";
            }
        }
        public static int layIdBaoCao(this DanhSachBaoCaoTheoGiaoDich danhSachBaoCao)
        {
            switch (danhSachBaoCao)
            {
                case DanhSachBaoCaoTheoGiaoDich.KHTV_PHIEU_KHAOSAT: return 1200301;
                case DanhSachBaoCaoTheoGiaoDich.KHTV_PHIEU_DANHGIA: return 1200302;
                case DanhSachBaoCaoTheoGiaoDich.KHTV_PHIEU_XEPHANG: return 1100303;
                case DanhSachBaoCaoTheoGiaoDich.GDKT_IN_GIAO_DICH: return 1200400;
                case DanhSachBaoCaoTheoGiaoDich.GDKT_PHIEU_THU: return 1200401;
                case DanhSachBaoCaoTheoGiaoDich.GDKT_PHIEU_CHI: return 1200402;
                case DanhSachBaoCaoTheoGiaoDich.GDKT_PHIEU_HACH_TOAN: return 1200403;
                case DanhSachBaoCaoTheoGiaoDich.HDVO_SO_TKCKH: return 1200501;
                case DanhSachBaoCaoTheoGiaoDich.TDVM_HDTD: return 1200601;
                case DanhSachBaoCaoTheoGiaoDich.TDVM_KUOCVM: return 1200602;
                case DanhSachBaoCaoTheoGiaoDich.TDVM_KUOCVM_KEHOACH: return 1200603;
                case DanhSachBaoCaoTheoGiaoDich.TDVM_DS_KHANG_NHANVON: return 1200604;
                case DanhSachBaoCaoTheoGiaoDich.TDVM_HOA_DON_THU_TIEN_KY: return 1100604;
                case DanhSachBaoCaoTheoGiaoDich.BHTH_PHIEU_YEU_CAU_BH: return 1100709;
                case DanhSachBaoCaoTheoGiaoDich.TDVM_KUOCVM_00: return 1200605;
                case DanhSachBaoCaoTheoGiaoDich.NSTL_HOP_DONG_HOC_VIEC: return 1200606;
                case DanhSachBaoCaoTheoGiaoDich.NSTL_HOP_DONG_THU_VIEC: return 1200607;
                case DanhSachBaoCaoTheoGiaoDich.NSTL_HOP_DONG_LAO_DONG: return 1200608;
                case DanhSachBaoCaoTheoGiaoDich.TDVM_KUOCVM_NHAN_NO_00: return 1200609;
                case DanhSachBaoCaoTheoGiaoDich.TDVM_KUOCVM_PHAN_KY_00: return 1200610;
                case DanhSachBaoCaoTheoGiaoDich.GDKT_PHIEU_UY_NHIEM_CHI: return 1200611;
                default: return 0;
            }
        }


        public enum DanhSachBaoCaoTheoDinhKy
        {
            QTHT_BAO_CAO_KIEM_SOAT,
            KHTV_PHIEU_KHAO_SAT,
            KHTV_PHIEU_DANH_GIA,
            KHTV_PHIEU_XEP_HANG_TD,
            KHTV_DS_KHTV,
            KHTV_DS_KHCN,
            KHTV_DS_KHTC,
            KHTV_DS_KHTV_RA_KHOI_NHOM,
            KHTV_NGUYEN_NHAN_RA_KHOI_NHOM,
            KHTV_DAC_DIEM_HOKHTV,
            KHTV_TANG_TRUONG_KT_KHTV,
            KHTV_THU_NHAP_HOKHTV,
            KHTV_CHI_TIEU_HOKHTV,
            GDKT_NHAT_KY_CHUNG,
            GDKT_NHAT_KY_CHI_TIEN,
            GDKT_NHAT_KY_THU_TIEN,
            GDKT_SO_CAI_TAI_KHOAN,
            GDKT_SO_QUY_TIEN_MAT,
            GDKT_SO_CHI_TIET_TAI_KHOAN,
            GDKT_BANG_KE_TIEN_MAT,
            GDKT_GIAY_GUI_TIEN_TIET_KIEM,
            GDKT_PHIEU_CHI,
            GDKT_PHIEU_HACH_TOAN,
            GDKT_PHIEU_THU,
            GDKT_BANG_CAN_DOI_NOI_BANG,
            GDKT_BANG_CAN_DOI_NGOAI_BANG,
            GDKT_BANG_CAN_DOI_KE_TOAN,
            GDKT_KET_QUA_KINH_DOANH,
            GDKT_LUU_CHUYEN_TIEN_TE,
            GDKT_THUYET_MINH_TAI_CHINH,
            GDKT_SO_TIEN_GUI_NGAN_HANG,
            GDKT_SO_CT_TAI_KHOAN_CONG_NO,
            GDKT_SO_TH_CONG_NO_PHAI_THU,
            GDKT_SO_TH_CONG_NO_PHAI_TRA,
            GDKT_SO_TH_CONG_NO_TAM_UNG,
            GDKT_SO_TH_TKHOAN_THEO_DOI_UNG,
            GDKT_SO_THEO_DOI_CT_NVON_KDOANH,
            GDKT_KET_XUAT_BCDKT_THEO_CAU_TRUC,
            HDVO_BANG_KE_TKQD_TKKKH,
            HDVO_BANG_KE_TKCKH,
            HDVO_SO_PHU_TIEN_GUI,
            HDVO_TONG_HOP_TKCKH,
            HDVO_BANG_XAC_NHAN_SO_DU_TK,
            HDVO_SO_THEO_DOI_TK,
            TDVM_HDTD,
            TDVM_KHE_UOC_NHAN_NO,
            TDVM_DS_KHTV_NHAN_VON,
            TDVM_HOA_DON_THU_TIEN_KY,
            TDVM_SAO_KE_TH,
            TDVM_SAO_KE_VONNH,
            TDVM_SAO_KE_VONTH,
            TDVM_SAO_KE_VONDH,
            TDVM_THONG_KE_TH,
            TDVM_BC_CHAM_TRA,
            TDVM_PHIEU_THAM_DINH_VON_VAY,
            KHTV_KH_THU_CHI_TIEN_MAT_TRONG_THANG,
            KHTV_BC_TIEN_DO,
            TDVM_BC_VON_TIET_KIEM_TVIEN,
            TDVM_BC_VON_TIET_KIEM_CUM,
            TDVM_BC_PHAN_LOAI_NO,
            TDVM_BANG_XAC_NHAN_SO_DU_VAY_VON,
            TDVM_PHIEU_THEO_DOI_HOAN_TRA_CUM,
            TDVM_PHIEU_THEO_DOI_HOAN_TRA_TVIEN,
            TDVM_DS_KHTV_NHAN_VON_00,
            TDVM_KHE_UOC_NHAN_NO_00,
            TDVM_BAO_NO_TONG,
            TDVM_BAO_NO_CUM,
            TDVM_BAO_NO_THANH_VIEN,
            TDVM_BAO_NO_TONG_CHI_TIET,
            BHTH_BAO_VE_VON_VAY,
            BHTH_HO_TRO_MO_DE,
            BHTH_CHI_TRA_NHAN_THO,
            BHTH_CHI_TRA_TU_VONG,
            BHTH_XOA_NO_VON_VAY,
            BHTH_THU_CHI,
            BHTH_TIEN_DO,
            BHTH_DSACH_BAO_VE_VON_VAY,
            BHTH_PHIEU_YEU_CAU_BAO_VE_VON_VAY,
            NSTL_PHU_CAP_CUM_TRUONG,
            BCTH_BC_VAN_HANH,
            BCTH_BANG_CHI_SO_PEAL,
            BCTH_BANG_CHI_TIEU_THEO_NGAN_HANG,
            BCTH_BANG_CHI_SO_BAO_CAO_NHA_TAI_TRO,
            BCTH_THONG_KE_TH,
            BCTH_KH_THU_CHI_TIEN_MAT_TRONG_THANG,
            BCTH_BC_TIEN_DO,
            BCTH_TH_THU_CHI,
            BCTK_DU_NO_TD_THEO_NGANH,
            BCTK_DU_NO_TD_THEO_LH_TOCHUC_CANHAN,
            BCTK_HOAT_DONG_HDV,
            BCTK_BC_PLOAI_NO_DP_RUIRO,
            BCTK_BC_XULY_RUIRO_CHOVAY,
            BCTK_BC_TYLE_DAM_BAO_ANTOAN,
            BCTK_BC_KHANG_CHOVAY_VUOT_QDINH,
            BCTK_BANG_CAN_DOI,
            BCTK_BC_KET_QUA_HD_KD,
            TDVM_BKE_CNO_TVIEN_THEO_CUM,
            TDVM_BKE_CNO_TVIEN_THEO_XA,
            TDVM_BKE_CNO_TVIEN_THEO_PGD,
            TDVM_BKE_CNO_TVIEN_THEO_CN,
            TDVM_BANG_THOP_PHAT_VON_VAY_THEO_PGD,
            TDVM_BANG_THOP_PHAT_VON_VAY_THEO_CN,
            TDVM_BANG_THOP_TINH_LAI_THEO_CN,
            TDVM_BANG_THOP_TINH_LAI_THEO_PGD,
            HDVO_BANG_THOP_DCHI_TRA_TKIEM_CKH_THEO_CN,
            HDVO_BANG_THOP_DCHI_TRA_TKIEM_CKH_THEO_PGD,
            HDVO_BANG_TINH_LAI_TGUI_CKH,
            HDVO_BANG_TINH_LAI_TGUI_CKH_THEO_KH,
            HDVO_BANG_THOP_SDU_GOC_TKIEM_CKH_THEO_XA,
            HDVO_BANG_THOP_CPHI_LAI_TGUI_CKH_THEO_PGD,
            HDVO_BANG_THOP_CPHI_LAI_TGUI_CKH_THEO_CN,
            BCTH_BAO_CAO_VAN_HANH,
            BCTK_THONG_KE_HO_NGHEO_CAN_NGHEO,
            GDKT_BANG_KE_CHUNG_TU,
            KHTV_TKE_HO_NGHEO_CAN_NGHEO,
            BCTH_BC_VAN_HANH_00,
            BCTH_TH_LICH_SU,
            // TaiNM 20161101 Add
            GDKT_TONG_HOP_DU_LIEU_KTOAN
        };

        public static string layMaBaoCao(this DanhSachBaoCaoTheoDinhKy danhSachBaoCao)
        {
            switch (danhSachBaoCao)
            {
                case DanhSachBaoCaoTheoDinhKy.QTHT_BAO_CAO_KIEM_SOAT: return "QTHT_BAO_CAO_KIEM_SOAT";
                case DanhSachBaoCaoTheoDinhKy.KHTV_PHIEU_KHAO_SAT: return "KHTV_PHIEU_KHAO_SAT";
                case DanhSachBaoCaoTheoDinhKy.KHTV_PHIEU_DANH_GIA: return "KHTV_PHIEU_DANH_GIA";
                case DanhSachBaoCaoTheoDinhKy.KHTV_PHIEU_XEP_HANG_TD: return "KHTV_PHIEU_XEP_HANG_TD";
                case DanhSachBaoCaoTheoDinhKy.KHTV_DS_KHTV: return "KHTV_DS_KHTV";
                case DanhSachBaoCaoTheoDinhKy.KHTV_DS_KHCN: return "KHTV_DS_KHCN";
                case DanhSachBaoCaoTheoDinhKy.KHTV_DS_KHTC: return "KHTV_DS_KHTC";
                case DanhSachBaoCaoTheoDinhKy.KHTV_DS_KHTV_RA_KHOI_NHOM: return "KHTV_DS_KHTV_RA_KHOI_NHOM";
                case DanhSachBaoCaoTheoDinhKy.KHTV_NGUYEN_NHAN_RA_KHOI_NHOM: return "KHTV_NGUYEN_NHAN_RA_KHOI_NHOM";
                case DanhSachBaoCaoTheoDinhKy.KHTV_DAC_DIEM_HOKHTV: return "KHTV_DAC_DIEM_HOKHTV";
                case DanhSachBaoCaoTheoDinhKy.KHTV_TANG_TRUONG_KT_KHTV: return "KHTV_TANG_TRUONG_KT_KHTV";
                case DanhSachBaoCaoTheoDinhKy.KHTV_THU_NHAP_HOKHTV: return "KHTV_THU_NHAP_HOKHTV";
                case DanhSachBaoCaoTheoDinhKy.KHTV_CHI_TIEU_HOKHTV: return "KHTV_CHI_TIEU_HOKHTV";
                case DanhSachBaoCaoTheoDinhKy.GDKT_NHAT_KY_CHUNG: return "GDKT_NHAT_KY_CHUNG";
                case DanhSachBaoCaoTheoDinhKy.GDKT_NHAT_KY_CHI_TIEN: return "GDKT_NHAT_KY_CHI_TIEN";
                case DanhSachBaoCaoTheoDinhKy.GDKT_NHAT_KY_THU_TIEN: return "GDKT_NHAT_KY_THU_TIEN";
                case DanhSachBaoCaoTheoDinhKy.GDKT_SO_CAI_TAI_KHOAN: return "GDKT_SO_CAI_TAI_KHOAN";
                case DanhSachBaoCaoTheoDinhKy.GDKT_SO_QUY_TIEN_MAT: return "GDKT_SO_QUY_TIEN_MAT";
                case DanhSachBaoCaoTheoDinhKy.GDKT_SO_CHI_TIET_TAI_KHOAN: return "GDKT_SO_CHI_TIET_TAI_KHOAN";
                case DanhSachBaoCaoTheoDinhKy.GDKT_BANG_KE_TIEN_MAT: return "GDKT_BANG_KE_TIEN_MAT";
                case DanhSachBaoCaoTheoDinhKy.GDKT_GIAY_GUI_TIEN_TIET_KIEM: return "GDKT_GIAY_GUI_TIEN_TIET_KIEM";
                case DanhSachBaoCaoTheoDinhKy.GDKT_PHIEU_CHI: return "GDKT_PHIEU_CHI";
                case DanhSachBaoCaoTheoDinhKy.GDKT_PHIEU_HACH_TOAN: return "GDKT_PHIEU_HACH_TOAN";
                case DanhSachBaoCaoTheoDinhKy.GDKT_PHIEU_THU: return "GDKT_PHIEU_THU";
                case DanhSachBaoCaoTheoDinhKy.GDKT_BANG_CAN_DOI_NOI_BANG: return "GDKT_BANG_CAN_DOI_NOI_BANG";
                case DanhSachBaoCaoTheoDinhKy.GDKT_BANG_CAN_DOI_NGOAI_BANG: return "GDKT_BANG_CAN_DOI_NGOAI_BANG";
                case DanhSachBaoCaoTheoDinhKy.GDKT_BANG_CAN_DOI_KE_TOAN: return "GDKT_BANG_CAN_DOI_KE_TOAN";
                case DanhSachBaoCaoTheoDinhKy.GDKT_KET_QUA_KINH_DOANH: return "GDKT_KET_QUA_KINH_DOANH";
                case DanhSachBaoCaoTheoDinhKy.GDKT_LUU_CHUYEN_TIEN_TE: return "GDKT_LUU_CHUYEN_TIEN_TE";
                case DanhSachBaoCaoTheoDinhKy.GDKT_THUYET_MINH_TAI_CHINH: return "GDKT_THUYET_MINH_TAI_CHINH";
                case DanhSachBaoCaoTheoDinhKy.GDKT_SO_TIEN_GUI_NGAN_HANG: return "GDKT_SO_TIEN_GUI_NGAN_HANG";
                case DanhSachBaoCaoTheoDinhKy.GDKT_SO_CT_TAI_KHOAN_CONG_NO: return "GDKT_SO_CT_TAI_KHOAN_CONG_NO";
                case DanhSachBaoCaoTheoDinhKy.GDKT_SO_TH_CONG_NO_PHAI_THU: return "GDKT_SO_TH_CONG_NO_PHAI_THU";
                case DanhSachBaoCaoTheoDinhKy.GDKT_SO_TH_CONG_NO_PHAI_TRA: return "GDKT_SO_TH_CONG_NO_PHAI_TRA";
                case DanhSachBaoCaoTheoDinhKy.GDKT_SO_TH_CONG_NO_TAM_UNG: return "GDKT_SO_TH_CONG_NO_TAM_UNG";
                case DanhSachBaoCaoTheoDinhKy.GDKT_SO_TH_TKHOAN_THEO_DOI_UNG: return "GDKT_SO_TH_TKHOAN_THEO_DOI_UNG";
                case DanhSachBaoCaoTheoDinhKy.GDKT_SO_THEO_DOI_CT_NVON_KDOANH: return "GDKT_SO_THEO_DOI_CT_NVON_KDOANH";
                case DanhSachBaoCaoTheoDinhKy.GDKT_KET_XUAT_BCDKT_THEO_CAU_TRUC: return "GDKT_KET_XUAT_BCDKT_THEO_CAU_TRUC";
                case DanhSachBaoCaoTheoDinhKy.HDVO_BANG_KE_TKQD_TKKKH: return "HDVO_BANG_KE_TKQD_TKKKH";
                case DanhSachBaoCaoTheoDinhKy.HDVO_BANG_KE_TKCKH: return "HDVO_BANG_KE_TKCKH";
                case DanhSachBaoCaoTheoDinhKy.HDVO_SO_PHU_TIEN_GUI: return "HDVO_SO_PHU_TIEN_GUI";
                case DanhSachBaoCaoTheoDinhKy.HDVO_TONG_HOP_TKCKH: return "HDVO_TONG_HOP_TKCKH";
                case DanhSachBaoCaoTheoDinhKy.HDVO_BANG_XAC_NHAN_SO_DU_TK: return "HDVO_BANG_XAC_NHAN_SO_DU_TK";
                case DanhSachBaoCaoTheoDinhKy.HDVO_SO_THEO_DOI_TK: return "HDVO_SO_THEO_DOI_TK";
                case DanhSachBaoCaoTheoDinhKy.TDVM_HDTD: return "TDVM_HDTD";
                case DanhSachBaoCaoTheoDinhKy.TDVM_KHE_UOC_NHAN_NO: return "TDVM_KHE_UOC_NHAN_NO";
                case DanhSachBaoCaoTheoDinhKy.TDVM_DS_KHTV_NHAN_VON: return "TDVM_DS_KHTV_NHAN_VON";
                case DanhSachBaoCaoTheoDinhKy.TDVM_HOA_DON_THU_TIEN_KY: return "TDVM_HOA_DON_THU_TIEN_KY";
                case DanhSachBaoCaoTheoDinhKy.TDVM_SAO_KE_TH: return "TDVM_SAO_KE_TH";
                case DanhSachBaoCaoTheoDinhKy.TDVM_SAO_KE_VONNH: return "TDVM_SAO_KE_VONNH";
                case DanhSachBaoCaoTheoDinhKy.TDVM_SAO_KE_VONTH: return "TDVM_SAO_KE_VONTH";
                case DanhSachBaoCaoTheoDinhKy.TDVM_SAO_KE_VONDH: return "TDVM_SAO_KE_VONDH";
                case DanhSachBaoCaoTheoDinhKy.TDVM_THONG_KE_TH: return "TDVM_THONG_KE_TH";
                case DanhSachBaoCaoTheoDinhKy.TDVM_BC_CHAM_TRA: return "TDVM_BC_CHAM_TRA";
                case DanhSachBaoCaoTheoDinhKy.TDVM_PHIEU_THAM_DINH_VON_VAY: return "TDVM_PHIEU_THAM_DINH_VON_VAY";
                case DanhSachBaoCaoTheoDinhKy.KHTV_KH_THU_CHI_TIEN_MAT_TRONG_THANG: return "KHTV_KH_THU_CHI_TIEN_MAT_TRONG_THANG";
                case DanhSachBaoCaoTheoDinhKy.KHTV_BC_TIEN_DO: return "KHTV_BC_TIEN_DO";
                case DanhSachBaoCaoTheoDinhKy.TDVM_BC_VON_TIET_KIEM_TVIEN: return "TDVM_BC_VON_TIET_KIEM_TVIEN";
                case DanhSachBaoCaoTheoDinhKy.TDVM_BC_VON_TIET_KIEM_CUM: return "TDVM_BC_VON_TIET_KIEM_CUM";
                case DanhSachBaoCaoTheoDinhKy.TDVM_BC_PHAN_LOAI_NO: return "TDVM_BC_PHAN_LOAI_NO";
                case DanhSachBaoCaoTheoDinhKy.TDVM_BANG_XAC_NHAN_SO_DU_VAY_VON: return "TDVM_BANG_XAC_NHAN_SO_DU_VAY_VON";
                case DanhSachBaoCaoTheoDinhKy.TDVM_PHIEU_THEO_DOI_HOAN_TRA_CUM: return "TDVM_PHIEU_THEO_DOI_HOAN_TRA_CUM";
                case DanhSachBaoCaoTheoDinhKy.TDVM_PHIEU_THEO_DOI_HOAN_TRA_TVIEN: return "TDVM_PHIEU_THEO_DOI_HOAN_TRA_TVIEN";
                case DanhSachBaoCaoTheoDinhKy.TDVM_DS_KHTV_NHAN_VON_00: return "TDVM_DS_KHTV_NHAN_VON_00";
                case DanhSachBaoCaoTheoDinhKy.TDVM_KHE_UOC_NHAN_NO_00: return "TDVM_KHE_UOC_NHAN_NO_00";
                case DanhSachBaoCaoTheoDinhKy.TDVM_BAO_NO_TONG: return "TDVM_BAO_NO_TONG";
                case DanhSachBaoCaoTheoDinhKy.TDVM_BAO_NO_CUM: return "TDVM_BAO_NO_CUM";
                case DanhSachBaoCaoTheoDinhKy.TDVM_BAO_NO_THANH_VIEN: return "TDVM_BAO_NO_THANH_VIEN";
                case DanhSachBaoCaoTheoDinhKy.TDVM_BAO_NO_TONG_CHI_TIET: return "TDVM_BAO_NO_TONG_CHI_TIET";
                case DanhSachBaoCaoTheoDinhKy.BHTH_BAO_VE_VON_VAY: return "BHTH_BAO_VE_VON_VAY";
                case DanhSachBaoCaoTheoDinhKy.BHTH_HO_TRO_MO_DE: return "BHTH_HO_TRO_MO_DE";
                case DanhSachBaoCaoTheoDinhKy.BHTH_CHI_TRA_NHAN_THO: return "BHTH_CHI_TRA_NHAN_THO";
                case DanhSachBaoCaoTheoDinhKy.BHTH_CHI_TRA_TU_VONG: return "BHTH_CHI_TRA_TU_VONG";
                case DanhSachBaoCaoTheoDinhKy.BHTH_XOA_NO_VON_VAY: return "BHTH_XOA_NO_VON_VAY";
                case DanhSachBaoCaoTheoDinhKy.BHTH_THU_CHI: return "BHTH_THU_CHI";
                case DanhSachBaoCaoTheoDinhKy.BHTH_TIEN_DO: return "BHTH_TIEN_DO";
                case DanhSachBaoCaoTheoDinhKy.BHTH_DSACH_BAO_VE_VON_VAY: return "BHTH_DSACH_BAO_VE_VON_VAY";
                case DanhSachBaoCaoTheoDinhKy.BHTH_PHIEU_YEU_CAU_BAO_VE_VON_VAY: return "BHTH_PHIEU_YEU_CAU_BAO_VE_VON_VAY";
                case DanhSachBaoCaoTheoDinhKy.NSTL_PHU_CAP_CUM_TRUONG: return "NSTL_PHU_CAP_CUM_TRUONG";
                case DanhSachBaoCaoTheoDinhKy.BCTH_BC_VAN_HANH: return "BCTH_BC_VAN_HANH";
                case DanhSachBaoCaoTheoDinhKy.BCTH_BANG_CHI_SO_PEAL: return "BCTH_BANG_CHI_SO_PEAL";
                case DanhSachBaoCaoTheoDinhKy.BCTH_BANG_CHI_TIEU_THEO_NGAN_HANG: return "BCTH_BANG_CHI_TIEU_THEO_NGAN_HANG";
                case DanhSachBaoCaoTheoDinhKy.BCTH_BANG_CHI_SO_BAO_CAO_NHA_TAI_TRO: return "BCTH_BANG_CHI_SO_BAO_CAO_NHA_TAI_TRO";
                case DanhSachBaoCaoTheoDinhKy.BCTH_THONG_KE_TH: return "BCTH_THONG_KE_TH";
                case DanhSachBaoCaoTheoDinhKy.BCTH_KH_THU_CHI_TIEN_MAT_TRONG_THANG: return "BCTH_KH_THU_CHI_TIEN_MAT_TRONG_THANG";
                case DanhSachBaoCaoTheoDinhKy.BCTH_BC_TIEN_DO: return "BCTH_BC_TIEN_DO";
                case DanhSachBaoCaoTheoDinhKy.BCTH_TH_THU_CHI: return "BCTH_TH_THU_CHI";
                case DanhSachBaoCaoTheoDinhKy.BCTK_DU_NO_TD_THEO_NGANH: return "BCTK_DU_NO_TD_THEO_NGANH";
                case DanhSachBaoCaoTheoDinhKy.BCTK_DU_NO_TD_THEO_LH_TOCHUC_CANHAN: return "BCTK_DU_NO_TD_THEO_LH_TOCHUC_CANHAN";
                case DanhSachBaoCaoTheoDinhKy.BCTK_HOAT_DONG_HDV: return "BCTK_HOAT_DONG_HDV";
                case DanhSachBaoCaoTheoDinhKy.BCTK_BC_PLOAI_NO_DP_RUIRO: return "BCTK_BC_PLOAI_NO_DP_RUIRO";
                case DanhSachBaoCaoTheoDinhKy.BCTK_BC_XULY_RUIRO_CHOVAY: return "BCTK_BC_XULY_RUIRO_CHOVAY";
                case DanhSachBaoCaoTheoDinhKy.BCTK_BC_TYLE_DAM_BAO_ANTOAN: return "BCTK_BC_TYLE_DAM_BAO_ANTOAN";
                case DanhSachBaoCaoTheoDinhKy.BCTK_BC_KHANG_CHOVAY_VUOT_QDINH: return "BCTK_BC_KHANG_CHOVAY_VUOT_QDINH";
                case DanhSachBaoCaoTheoDinhKy.BCTK_BANG_CAN_DOI: return "BCTK_BANG_CAN_DOI";
                case DanhSachBaoCaoTheoDinhKy.BCTK_BC_KET_QUA_HD_KD: return "BCTK_BC_KET_QUA_HD_KD";
                case DanhSachBaoCaoTheoDinhKy.TDVM_BKE_CNO_TVIEN_THEO_CUM: return "TDVM_BKE_CNO_TVIEN_THEO_CUM";
                case DanhSachBaoCaoTheoDinhKy.TDVM_BKE_CNO_TVIEN_THEO_XA: return "TDVM_BKE_CNO_TVIEN_THEO_XA";
                case DanhSachBaoCaoTheoDinhKy.TDVM_BKE_CNO_TVIEN_THEO_PGD: return "TDVM_BKE_CNO_TVIEN_THEO_PGD";
                case DanhSachBaoCaoTheoDinhKy.TDVM_BKE_CNO_TVIEN_THEO_CN: return "TDVM_BKE_CNO_TVIEN_THEO_CN";
                case DanhSachBaoCaoTheoDinhKy.TDVM_BANG_THOP_PHAT_VON_VAY_THEO_PGD: return  "TDVM_BANG_THOP_PHAT_VON_VAY_THEO_PGD";
                case DanhSachBaoCaoTheoDinhKy.TDVM_BANG_THOP_PHAT_VON_VAY_THEO_CN: return "TDVM_BANG_THOP_PHAT_VON_VAY_THEO_CN";
                case DanhSachBaoCaoTheoDinhKy.TDVM_BANG_THOP_TINH_LAI_THEO_CN: return "TDVM_BANG_THOP_TINH_LAI_THEO_CN";
                case DanhSachBaoCaoTheoDinhKy.TDVM_BANG_THOP_TINH_LAI_THEO_PGD: return "TDVM_BANG_THOP_TINH_LAI_THEO_PGD";
                case DanhSachBaoCaoTheoDinhKy.HDVO_BANG_THOP_DCHI_TRA_TKIEM_CKH_THEO_CN: return "HDVO_BANG_THOP_DCHI_TRA_TKIEM_CKH_THEO_CN";
                case DanhSachBaoCaoTheoDinhKy.HDVO_BANG_THOP_DCHI_TRA_TKIEM_CKH_THEO_PGD: return "HDVO_BANG_THOP_DCHI_TRA_TKIEM_CKH_THEO_PGD";
                case DanhSachBaoCaoTheoDinhKy.HDVO_BANG_TINH_LAI_TGUI_CKH: return "HDVO_BANG_TINH_LAI_TGUI_CKH";
                case DanhSachBaoCaoTheoDinhKy.HDVO_BANG_TINH_LAI_TGUI_CKH_THEO_KH: return "HDVO_BANG_TINH_LAI_TGUI_CKH_THEO_KH";
                case DanhSachBaoCaoTheoDinhKy.HDVO_BANG_THOP_SDU_GOC_TKIEM_CKH_THEO_XA: return "HDVO_BANG_THOP_SDU_GOC_TKIEM_CKH_THEO_XA";
                case DanhSachBaoCaoTheoDinhKy.HDVO_BANG_THOP_CPHI_LAI_TGUI_CKH_THEO_PGD: return "HDVO_BANG_THOP_CPHI_LAI_TGUI_CKH_THEO_PGD";
                case DanhSachBaoCaoTheoDinhKy.HDVO_BANG_THOP_CPHI_LAI_TGUI_CKH_THEO_CN: return "HDVO_BANG_THOP_CPHI_LAI_TGUI_CKH_THEO_CN";
                case DanhSachBaoCaoTheoDinhKy.BCTH_BAO_CAO_VAN_HANH: return "BCTH_BAO_CAO_VAN_HANH";
                case DanhSachBaoCaoTheoDinhKy.BCTK_THONG_KE_HO_NGHEO_CAN_NGHEO: return  "BCTK_THONG_KE_HO_NGHEO_CAN_NGHEO";
                case DanhSachBaoCaoTheoDinhKy.GDKT_BANG_KE_CHUNG_TU: return "GDKT_BANG_KE_CHUNG_TU";
                case DanhSachBaoCaoTheoDinhKy.KHTV_TKE_HO_NGHEO_CAN_NGHEO: return "KHTV_TKE_HO_NGHEO_CAN_NGHEO";
                case DanhSachBaoCaoTheoDinhKy.BCTH_BC_VAN_HANH_00: return "BCTH_BC_VAN_HANH_00";
                case DanhSachBaoCaoTheoDinhKy.BCTH_TH_LICH_SU: return "BCTH_TH_LICH_SU";
                // TaiNM 20161101 Add
                case DanhSachBaoCaoTheoDinhKy.GDKT_TONG_HOP_DU_LIEU_KTOAN: return "BCTH_TH_LICH_SU";
                default: return "";
            }
        }

        public static DanhSachBaoCaoTheoDinhKy layBaoCaoTheoMa(string maBaoCao)
        {
            switch (maBaoCao)
            {
                case "QTHT_BAO_CAO_KIEM_SOAT": return DanhSachBaoCaoTheoDinhKy.QTHT_BAO_CAO_KIEM_SOAT;
                case "KHTV_PHIEU_KHAO_SAT": return DanhSachBaoCaoTheoDinhKy.KHTV_PHIEU_KHAO_SAT;
                case "KHTV_PHIEU_DANH_GIA": return DanhSachBaoCaoTheoDinhKy.KHTV_PHIEU_DANH_GIA;
                case "KHTV_PHIEU_XEP_HANG_TD": return DanhSachBaoCaoTheoDinhKy.KHTV_PHIEU_XEP_HANG_TD;
                case "KHTV_DS_KHTV": return DanhSachBaoCaoTheoDinhKy.KHTV_DS_KHTV;
                case "KHTV_DS_KHCN": return DanhSachBaoCaoTheoDinhKy.KHTV_DS_KHCN;
                case "KHTV_DS_KHTC": return DanhSachBaoCaoTheoDinhKy.KHTV_DS_KHTC;
                case "KHTV_DS_KHTV_RA_KHOI_NHOM": return DanhSachBaoCaoTheoDinhKy.KHTV_DS_KHTV_RA_KHOI_NHOM;
                case "KHTV_NGUYEN_NHAN_RA_KHOI_NHOM": return DanhSachBaoCaoTheoDinhKy.KHTV_NGUYEN_NHAN_RA_KHOI_NHOM;
                case "KHTV_DAC_DIEM_HOKHTV": return DanhSachBaoCaoTheoDinhKy.KHTV_DAC_DIEM_HOKHTV;
                case "KHTV_TANG_TRUONG_KT_KHTV": return DanhSachBaoCaoTheoDinhKy.KHTV_TANG_TRUONG_KT_KHTV;
                case "KHTV_THU_NHAP_HOKHTV": return DanhSachBaoCaoTheoDinhKy.KHTV_THU_NHAP_HOKHTV;
                case "KHTV_CHI_TIEU_HOKHTV": return DanhSachBaoCaoTheoDinhKy.KHTV_CHI_TIEU_HOKHTV;
                case "GDKT_NHAT_KY_CHUNG": return DanhSachBaoCaoTheoDinhKy.GDKT_NHAT_KY_CHUNG;
                case "GDKT_NHAT_KY_CHI_TIEN": return DanhSachBaoCaoTheoDinhKy.GDKT_NHAT_KY_CHI_TIEN;
                case "GDKT_NHAT_KY_THU_TIEN": return DanhSachBaoCaoTheoDinhKy.GDKT_NHAT_KY_THU_TIEN;
                case "GDKT_SO_CAI_TAI_KHOAN": return DanhSachBaoCaoTheoDinhKy.GDKT_SO_CAI_TAI_KHOAN;
                case "GDKT_SO_QUY_TIEN_MAT": return DanhSachBaoCaoTheoDinhKy.GDKT_SO_QUY_TIEN_MAT;
                case "GDKT_SO_CHI_TIET_TAI_KHOAN": return DanhSachBaoCaoTheoDinhKy.GDKT_SO_CHI_TIET_TAI_KHOAN;
                case "GDKT_BANG_KE_TIEN_MAT": return DanhSachBaoCaoTheoDinhKy.GDKT_BANG_KE_TIEN_MAT;
                case "GDKT_GIAY_GUI_TIEN_TIET_KIEM": return DanhSachBaoCaoTheoDinhKy.GDKT_GIAY_GUI_TIEN_TIET_KIEM;
                case "GDKT_PHIEU_CHI": return DanhSachBaoCaoTheoDinhKy.GDKT_PHIEU_CHI;
                case "GDKT_PHIEU_HACH_TOAN": return DanhSachBaoCaoTheoDinhKy.GDKT_PHIEU_HACH_TOAN;
                case "GDKT_PHIEU_THU": return DanhSachBaoCaoTheoDinhKy.GDKT_PHIEU_THU;
                case "GDKT_BANG_CAN_DOI_NOI_BANG": return DanhSachBaoCaoTheoDinhKy.GDKT_BANG_CAN_DOI_NOI_BANG;
                case "GDKT_BANG_CAN_DOI_NGOAI_BANG": return DanhSachBaoCaoTheoDinhKy.GDKT_BANG_CAN_DOI_NGOAI_BANG;
                case "GDKT_BANG_CAN_DOI_KE_TOAN": return DanhSachBaoCaoTheoDinhKy.GDKT_BANG_CAN_DOI_KE_TOAN;
                case "GDKT_KET_QUA_KINH_DOANH": return DanhSachBaoCaoTheoDinhKy.GDKT_KET_QUA_KINH_DOANH;
                case "GDKT_LUU_CHUYEN_TIEN_TE": return DanhSachBaoCaoTheoDinhKy.GDKT_LUU_CHUYEN_TIEN_TE;
                case "GDKT_THUYET_MINH_TAI_CHINH": return DanhSachBaoCaoTheoDinhKy.GDKT_THUYET_MINH_TAI_CHINH;
                case "GDKT_SO_TIEN_GUI_NGAN_HANG": return DanhSachBaoCaoTheoDinhKy.GDKT_SO_TIEN_GUI_NGAN_HANG;
                case "GDKT_SO_CT_TAI_KHOAN_CONG_NO": return DanhSachBaoCaoTheoDinhKy.GDKT_SO_CT_TAI_KHOAN_CONG_NO;
                case "GDKT_SO_TH_CONG_NO_PHAI_THU": return DanhSachBaoCaoTheoDinhKy.GDKT_SO_TH_CONG_NO_PHAI_THU;
                case "GDKT_SO_TH_CONG_NO_PHAI_TRA": return DanhSachBaoCaoTheoDinhKy.GDKT_SO_TH_CONG_NO_PHAI_TRA;
                case "GDKT_SO_TH_CONG_NO_TAM_UNG": return DanhSachBaoCaoTheoDinhKy.GDKT_SO_TH_CONG_NO_TAM_UNG;
                case "GDKT_SO_TH_TKHOAN_THEO_DOI_UNG": return DanhSachBaoCaoTheoDinhKy.GDKT_SO_TH_TKHOAN_THEO_DOI_UNG;
                case "GDKT_SO_THEO_DOI_CT_NVON_KDOANH": return DanhSachBaoCaoTheoDinhKy.GDKT_SO_THEO_DOI_CT_NVON_KDOANH;
                case "GDKT_KET_XUAT_BCDKT_THEO_CAU_TRUC": return DanhSachBaoCaoTheoDinhKy.GDKT_KET_XUAT_BCDKT_THEO_CAU_TRUC;
                case "HDVO_BANG_KE_TKQD_TKKKH": return DanhSachBaoCaoTheoDinhKy.HDVO_BANG_KE_TKQD_TKKKH;
                case "HDVO_BANG_KE_TKCKH": return DanhSachBaoCaoTheoDinhKy.HDVO_BANG_KE_TKCKH;
                case "HDVO_SO_PHU_TIEN_GUI": return DanhSachBaoCaoTheoDinhKy.HDVO_SO_PHU_TIEN_GUI;
                case "HDVO_TONG_HOP_TKCKH": return DanhSachBaoCaoTheoDinhKy.HDVO_TONG_HOP_TKCKH;
                case "HDVO_BANG_XAC_NHAN_SO_DU_TK": return DanhSachBaoCaoTheoDinhKy.HDVO_BANG_XAC_NHAN_SO_DU_TK;
                case "HDVO_SO_THEO_DOI_TK": return DanhSachBaoCaoTheoDinhKy.HDVO_SO_THEO_DOI_TK;
                case "TDVM_HDTD": return DanhSachBaoCaoTheoDinhKy.TDVM_HDTD;
                case "TDVM_KHE_UOC_NHAN_NO": return DanhSachBaoCaoTheoDinhKy.TDVM_KHE_UOC_NHAN_NO;
                case "TDVM_DS_KHTV_NHAN_VON": return DanhSachBaoCaoTheoDinhKy.TDVM_DS_KHTV_NHAN_VON;
                case "TDVM_HOA_DON_THU_TIEN_KY": return DanhSachBaoCaoTheoDinhKy.TDVM_HOA_DON_THU_TIEN_KY;
                case "TDVM_SAO_KE_TH": return DanhSachBaoCaoTheoDinhKy.TDVM_SAO_KE_TH;
                case "TDVM_SAO_KE_VONNH": return DanhSachBaoCaoTheoDinhKy.TDVM_SAO_KE_VONNH;
                case "TDVM_SAO_KE_VONTH": return DanhSachBaoCaoTheoDinhKy.TDVM_SAO_KE_VONTH;
                case "TDVM_SAO_KE_VONDH": return DanhSachBaoCaoTheoDinhKy.TDVM_SAO_KE_VONDH;
                case "TDVM_THONG_KE_TH": return DanhSachBaoCaoTheoDinhKy.TDVM_THONG_KE_TH;
                case "TDVM_BC_CHAM_TRA": return DanhSachBaoCaoTheoDinhKy.TDVM_BC_CHAM_TRA;
                case "TDVM_PHIEU_THAM_DINH_VON_VAY": return DanhSachBaoCaoTheoDinhKy.TDVM_PHIEU_THAM_DINH_VON_VAY;
                case "KHTV_KH_THU_CHI_TIEN_MAT_TRONG_THANG": return DanhSachBaoCaoTheoDinhKy.KHTV_KH_THU_CHI_TIEN_MAT_TRONG_THANG;
                case "KHTV_BC_TIEN_DO": return DanhSachBaoCaoTheoDinhKy.KHTV_BC_TIEN_DO;
                case "TDVM_BC_VON_TIET_KIEM_TVIEN": return DanhSachBaoCaoTheoDinhKy.TDVM_BC_VON_TIET_KIEM_TVIEN;
                case "TDVM_BC_VON_TIET_KIEM_CUM": return DanhSachBaoCaoTheoDinhKy.TDVM_BC_VON_TIET_KIEM_CUM;
                case "TDVM_BC_PHAN_LOAI_NO": return DanhSachBaoCaoTheoDinhKy.TDVM_BC_PHAN_LOAI_NO;
                case "TDVM_BANG_XAC_NHAN_SO_DU_VAY_VON": return DanhSachBaoCaoTheoDinhKy.TDVM_BANG_XAC_NHAN_SO_DU_VAY_VON;
                case "TDVM_PHIEU_THEO_DOI_HOAN_TRA_CUM": return DanhSachBaoCaoTheoDinhKy.TDVM_PHIEU_THEO_DOI_HOAN_TRA_CUM;
                case "TDVM_PHIEU_THEO_DOI_HOAN_TRA_TVIEN": return DanhSachBaoCaoTheoDinhKy.TDVM_PHIEU_THEO_DOI_HOAN_TRA_TVIEN;
                case "TDVM_DS_KHTV_NHAN_VON_00": return DanhSachBaoCaoTheoDinhKy.TDVM_DS_KHTV_NHAN_VON_00;
                case "TDVM_KHE_UOC_NHAN_NO_00": return DanhSachBaoCaoTheoDinhKy.TDVM_KHE_UOC_NHAN_NO_00;
                case "TDVM_BAO_NO_TONG": return DanhSachBaoCaoTheoDinhKy.TDVM_BAO_NO_TONG;
                case "TDVM_BAO_NO_CUM": return DanhSachBaoCaoTheoDinhKy.TDVM_BAO_NO_CUM;
                case "TDVM_BAO_NO_THANH_VIEN": return DanhSachBaoCaoTheoDinhKy.TDVM_BAO_NO_THANH_VIEN;
                case "TDVM_BAO_NO_TONG_CHI_TIET": return DanhSachBaoCaoTheoDinhKy.TDVM_BAO_NO_TONG_CHI_TIET;
                case "BHTH_BAO_VE_VON_VAY": return DanhSachBaoCaoTheoDinhKy.BHTH_BAO_VE_VON_VAY;
                case "BHTH_HO_TRO_MO_DE": return DanhSachBaoCaoTheoDinhKy.BHTH_HO_TRO_MO_DE;
                case "BHTH_CHI_TRA_NHAN_THO": return DanhSachBaoCaoTheoDinhKy.BHTH_CHI_TRA_NHAN_THO;
                case "BHTH_CHI_TRA_TU_VONG": return DanhSachBaoCaoTheoDinhKy.BHTH_CHI_TRA_TU_VONG;
                case "BHTH_XOA_NO_VON_VAY": return DanhSachBaoCaoTheoDinhKy.BHTH_XOA_NO_VON_VAY;
                case "BHTH_THU_CHI": return DanhSachBaoCaoTheoDinhKy.BHTH_THU_CHI;
                case "BHTH_TIEN_DO": return DanhSachBaoCaoTheoDinhKy.BHTH_TIEN_DO;
                case "BHTH_DSACH_BAO_VE_VON_VAY": return DanhSachBaoCaoTheoDinhKy.BHTH_DSACH_BAO_VE_VON_VAY;
                case "BHTH_PHIEU_YEU_CAU_BAO_VE_VON_VAY": return DanhSachBaoCaoTheoDinhKy.BHTH_PHIEU_YEU_CAU_BAO_VE_VON_VAY;
                case "NSTL_PHU_CAP_CUM_TRUONG": return DanhSachBaoCaoTheoDinhKy.NSTL_PHU_CAP_CUM_TRUONG;
                case "BCTH_BC_VAN_HANH": return DanhSachBaoCaoTheoDinhKy.BCTH_BC_VAN_HANH;
                case "BCTH_BANG_CHI_SO_PEAL": return DanhSachBaoCaoTheoDinhKy.BCTH_BANG_CHI_SO_PEAL;
                case "BCTH_BANG_CHI_TIEU_THEO_NGAN_HANG": return DanhSachBaoCaoTheoDinhKy.BCTH_BANG_CHI_TIEU_THEO_NGAN_HANG;
                case "BCTH_BANG_CHI_SO_BAO_CAO_NHA_TAI_TRO": return DanhSachBaoCaoTheoDinhKy.BCTH_BANG_CHI_SO_BAO_CAO_NHA_TAI_TRO;
                case "BCTH_THONG_KE_TH": return DanhSachBaoCaoTheoDinhKy.BCTH_THONG_KE_TH;
                case "BCTH_KH_THU_CHI_TIEN_MAT_TRONG_THANG": return DanhSachBaoCaoTheoDinhKy.BCTH_KH_THU_CHI_TIEN_MAT_TRONG_THANG;
                case "BCTH_BC_TIEN_DO": return DanhSachBaoCaoTheoDinhKy.BCTH_BC_TIEN_DO;
                case "BCTH_TH_THU_CHI": return DanhSachBaoCaoTheoDinhKy.BCTH_TH_THU_CHI;
                case "BCTK_DU_NO_TD_THEO_NGANH": return DanhSachBaoCaoTheoDinhKy.BCTK_DU_NO_TD_THEO_NGANH;
                case "BCTK_DU_NO_TD_THEO_LH_TOCHUC_CANHAN": return DanhSachBaoCaoTheoDinhKy.BCTK_DU_NO_TD_THEO_LH_TOCHUC_CANHAN;
                case "BCTK_HOAT_DONG_HDV": return DanhSachBaoCaoTheoDinhKy.BCTK_HOAT_DONG_HDV;
                case "BCTK_BC_PLOAI_NO_DP_RUIRO": return DanhSachBaoCaoTheoDinhKy.BCTK_BC_PLOAI_NO_DP_RUIRO;
                case "BCTK_BC_XULY_RUIRO_CHOVAY": return DanhSachBaoCaoTheoDinhKy.BCTK_BC_XULY_RUIRO_CHOVAY;
                case "BCTK_BC_TYLE_DAM_BAO_ANTOAN": return DanhSachBaoCaoTheoDinhKy.BCTK_BC_TYLE_DAM_BAO_ANTOAN;
                case "BCTK_BC_KHANG_CHOVAY_VUOT_QDINH": return DanhSachBaoCaoTheoDinhKy.BCTK_BC_KHANG_CHOVAY_VUOT_QDINH;
                case "BCTK_BANG_CAN_DOI": return DanhSachBaoCaoTheoDinhKy.BCTK_BANG_CAN_DOI;
                case "BCTK_BC_KET_QUA_HD_KD": return DanhSachBaoCaoTheoDinhKy.BCTK_BC_KET_QUA_HD_KD;
                case "TDVM_BKE_CNO_TVIEN_THEO_CUM": return DanhSachBaoCaoTheoDinhKy.TDVM_BKE_CNO_TVIEN_THEO_CUM;
                case "TDVM_BKE_CNO_TVIEN_THEO_XA": return DanhSachBaoCaoTheoDinhKy.TDVM_BKE_CNO_TVIEN_THEO_XA;
                case "TDVM_BKE_CNO_TVIEN_THEO_PGD": return DanhSachBaoCaoTheoDinhKy.TDVM_BKE_CNO_TVIEN_THEO_PGD;
                case "TDVM_BKE_CNO_TVIEN_THEO_CN": return DanhSachBaoCaoTheoDinhKy.TDVM_BKE_CNO_TVIEN_THEO_CN;
                case "TDVM_BANG_THOP_PHAT_VON_VAY_THEO_PGD": return DanhSachBaoCaoTheoDinhKy.TDVM_BANG_THOP_PHAT_VON_VAY_THEO_PGD;
                case "TDVM_BANG_THOP_PHAT_VON_VAY_THEO_CN": return DanhSachBaoCaoTheoDinhKy.TDVM_BANG_THOP_PHAT_VON_VAY_THEO_CN;
                case "TDVM_BANG_THOP_TINH_LAI_THEO_CN": return DanhSachBaoCaoTheoDinhKy.TDVM_BANG_THOP_TINH_LAI_THEO_CN;
                case "TDVM_BANG_THOP_TINH_LAI_THEO_PGD": return DanhSachBaoCaoTheoDinhKy.TDVM_BANG_THOP_TINH_LAI_THEO_PGD;
                case "HDVO_BANG_THOP_DCHI_TRA_TKIEM_CKH_THEO_CN": return DanhSachBaoCaoTheoDinhKy.HDVO_BANG_THOP_DCHI_TRA_TKIEM_CKH_THEO_CN;
                case "HDVO_BANG_THOP_DCHI_TRA_TKIEM_CKH_THEO_PGD": return DanhSachBaoCaoTheoDinhKy.HDVO_BANG_THOP_DCHI_TRA_TKIEM_CKH_THEO_PGD;
                case "HDVO_BANG_TINH_LAI_TGUI_CKH": return DanhSachBaoCaoTheoDinhKy.HDVO_BANG_TINH_LAI_TGUI_CKH;
                case "HDVO_BANG_TINH_LAI_TGUI_CKH_THEO_KH": return DanhSachBaoCaoTheoDinhKy.HDVO_BANG_TINH_LAI_TGUI_CKH_THEO_KH;
                case "HDVO_BANG_THOP_SDU_GOC_TKIEM_CKH_THEO_XA": return DanhSachBaoCaoTheoDinhKy.HDVO_BANG_THOP_SDU_GOC_TKIEM_CKH_THEO_XA;
                case "HDVO_BANG_THOP_CPHI_LAI_TGUI_CKH_THEO_PGD": return DanhSachBaoCaoTheoDinhKy.HDVO_BANG_THOP_CPHI_LAI_TGUI_CKH_THEO_PGD;
                case "HDVO_BANG_THOP_CPHI_LAI_TGUI_CKH_THEO_CN": return DanhSachBaoCaoTheoDinhKy.HDVO_BANG_THOP_CPHI_LAI_TGUI_CKH_THEO_CN;
                case "BCTH_BAO_CAO_VAN_HANH": return DanhSachBaoCaoTheoDinhKy.BCTH_BAO_CAO_VAN_HANH;
                case "BCTK_THONG_KE_HO_NGHEO_CAN_NGHEO": return DanhSachBaoCaoTheoDinhKy.BCTK_THONG_KE_HO_NGHEO_CAN_NGHEO;
                case "GDKT_BANG_KE_CHUNG_TU": return DanhSachBaoCaoTheoDinhKy.GDKT_BANG_KE_CHUNG_TU;
                case "KHTV_TKE_HO_NGHEO_CAN_NGHEO": return DanhSachBaoCaoTheoDinhKy.KHTV_TKE_HO_NGHEO_CAN_NGHEO;
                case "BCTH_BC_VAN_HANH_00": return DanhSachBaoCaoTheoDinhKy.BCTH_BC_VAN_HANH_00;
                case "BCTH_TH_LICH_SU": return DanhSachBaoCaoTheoDinhKy.BCTH_TH_LICH_SU;
                // TaiNM 20161101 Add
                case "GDKT_TONG_HOP_DU_LIEU_KTOAN": return DanhSachBaoCaoTheoDinhKy.GDKT_TONG_HOP_DU_LIEU_KTOAN;
                default: return DanhSachBaoCaoTheoDinhKy.TDVM_THONG_KE_TH;
            }
        }

        public static int layIdBaoCao(this DanhSachBaoCaoTheoDinhKy danhSachBaoCao)
        {
            switch (danhSachBaoCao)
            {
                case DanhSachBaoCaoTheoDinhKy.QTHT_BAO_CAO_KIEM_SOAT: return 1100101;
                case DanhSachBaoCaoTheoDinhKy.KHTV_PHIEU_KHAO_SAT: return 1100301;
                case DanhSachBaoCaoTheoDinhKy.KHTV_PHIEU_DANH_GIA: return 1100302;
                case DanhSachBaoCaoTheoDinhKy.KHTV_PHIEU_XEP_HANG_TD: return 1100303;
                case DanhSachBaoCaoTheoDinhKy.KHTV_DS_KHTV: return 1100304;
                case DanhSachBaoCaoTheoDinhKy.KHTV_DS_KHCN: return 1100305;
                case DanhSachBaoCaoTheoDinhKy.KHTV_DS_KHTC: return 1100306;
                case DanhSachBaoCaoTheoDinhKy.KHTV_DS_KHTV_RA_KHOI_NHOM: return 1100307;
                case DanhSachBaoCaoTheoDinhKy.KHTV_NGUYEN_NHAN_RA_KHOI_NHOM: return 1100308;
                case DanhSachBaoCaoTheoDinhKy.KHTV_DAC_DIEM_HOKHTV: return 1100309;
                case DanhSachBaoCaoTheoDinhKy.KHTV_TANG_TRUONG_KT_KHTV: return 1100310;
                case DanhSachBaoCaoTheoDinhKy.KHTV_THU_NHAP_HOKHTV: return 1100311;
                case DanhSachBaoCaoTheoDinhKy.KHTV_CHI_TIEU_HOKHTV: return 1100312;
                case DanhSachBaoCaoTheoDinhKy.GDKT_NHAT_KY_CHUNG: return 1100401;
                case DanhSachBaoCaoTheoDinhKy.GDKT_NHAT_KY_CHI_TIEN: return 1100402;
                case DanhSachBaoCaoTheoDinhKy.GDKT_NHAT_KY_THU_TIEN: return 1100403;
                case DanhSachBaoCaoTheoDinhKy.GDKT_SO_CAI_TAI_KHOAN: return 1100404;
                case DanhSachBaoCaoTheoDinhKy.GDKT_SO_QUY_TIEN_MAT: return 1100405;
                case DanhSachBaoCaoTheoDinhKy.GDKT_SO_CHI_TIET_TAI_KHOAN: return 1100406;
                case DanhSachBaoCaoTheoDinhKy.GDKT_BANG_KE_TIEN_MAT: return 1100407;
                case DanhSachBaoCaoTheoDinhKy.GDKT_GIAY_GUI_TIEN_TIET_KIEM: return 1100408;
                case DanhSachBaoCaoTheoDinhKy.GDKT_PHIEU_CHI: return 1100409;
                case DanhSachBaoCaoTheoDinhKy.GDKT_PHIEU_HACH_TOAN: return 1100410;
                case DanhSachBaoCaoTheoDinhKy.GDKT_PHIEU_THU: return 1100411;
                case DanhSachBaoCaoTheoDinhKy.GDKT_BANG_CAN_DOI_NOI_BANG: return 1100412;
                case DanhSachBaoCaoTheoDinhKy.GDKT_BANG_CAN_DOI_NGOAI_BANG: return 1100413;
                case DanhSachBaoCaoTheoDinhKy.GDKT_BANG_CAN_DOI_KE_TOAN: return 1100414;
                case DanhSachBaoCaoTheoDinhKy.GDKT_KET_QUA_KINH_DOANH: return 1100415;
                case DanhSachBaoCaoTheoDinhKy.GDKT_LUU_CHUYEN_TIEN_TE: return 1100416;
                case DanhSachBaoCaoTheoDinhKy.GDKT_THUYET_MINH_TAI_CHINH: return 1100417;
                case DanhSachBaoCaoTheoDinhKy.GDKT_SO_TIEN_GUI_NGAN_HANG: return 1100418;
                case DanhSachBaoCaoTheoDinhKy.GDKT_SO_CT_TAI_KHOAN_CONG_NO: return 1100419;
                case DanhSachBaoCaoTheoDinhKy.GDKT_SO_TH_CONG_NO_PHAI_THU: return 1100420;
                case DanhSachBaoCaoTheoDinhKy.GDKT_SO_TH_CONG_NO_PHAI_TRA: return 1100421;
                case DanhSachBaoCaoTheoDinhKy.GDKT_SO_TH_CONG_NO_TAM_UNG: return 1100422;
                case DanhSachBaoCaoTheoDinhKy.GDKT_SO_TH_TKHOAN_THEO_DOI_UNG: return 1100423;
                case DanhSachBaoCaoTheoDinhKy.GDKT_SO_THEO_DOI_CT_NVON_KDOANH: return 1100424;
                case DanhSachBaoCaoTheoDinhKy.HDVO_BANG_KE_TKQD_TKKKH: return 1100501;
                case DanhSachBaoCaoTheoDinhKy.HDVO_BANG_KE_TKCKH: return 1100502;
                case DanhSachBaoCaoTheoDinhKy.HDVO_SO_PHU_TIEN_GUI: return 1100503;
                case DanhSachBaoCaoTheoDinhKy.HDVO_TONG_HOP_TKCKH: return 1100504;
                case DanhSachBaoCaoTheoDinhKy.HDVO_BANG_XAC_NHAN_SO_DU_TK: return 1100505;
                case DanhSachBaoCaoTheoDinhKy.HDVO_SO_THEO_DOI_TK: return 1100506;
                case DanhSachBaoCaoTheoDinhKy.TDVM_HDTD: return 1100601;
                case DanhSachBaoCaoTheoDinhKy.TDVM_KHE_UOC_NHAN_NO: return 1100602;
                case DanhSachBaoCaoTheoDinhKy.TDVM_DS_KHTV_NHAN_VON: return 1100603;
                case DanhSachBaoCaoTheoDinhKy.TDVM_HOA_DON_THU_TIEN_KY: return 1100604;
                case DanhSachBaoCaoTheoDinhKy.TDVM_SAO_KE_TH: return 1100605;
                case DanhSachBaoCaoTheoDinhKy.TDVM_SAO_KE_VONNH: return 1100606;
                case DanhSachBaoCaoTheoDinhKy.TDVM_SAO_KE_VONTH: return 1100607;
                case DanhSachBaoCaoTheoDinhKy.TDVM_SAO_KE_VONDH: return 1100608;
                case DanhSachBaoCaoTheoDinhKy.TDVM_THONG_KE_TH: return 1100609;
                case DanhSachBaoCaoTheoDinhKy.TDVM_BC_CHAM_TRA: return 1100610;
                case DanhSachBaoCaoTheoDinhKy.TDVM_PHIEU_THAM_DINH_VON_VAY: return 1100621;
                case DanhSachBaoCaoTheoDinhKy.KHTV_KH_THU_CHI_TIEN_MAT_TRONG_THANG: return 1100611;
                case DanhSachBaoCaoTheoDinhKy.KHTV_BC_TIEN_DO: return 1100612;
                case DanhSachBaoCaoTheoDinhKy.TDVM_BC_VON_TIET_KIEM_TVIEN: return 1100613;
                case DanhSachBaoCaoTheoDinhKy.TDVM_BC_VON_TIET_KIEM_CUM: return 1100614;
                case DanhSachBaoCaoTheoDinhKy.TDVM_BC_PHAN_LOAI_NO: return 1100615;
                case DanhSachBaoCaoTheoDinhKy.TDVM_BANG_XAC_NHAN_SO_DU_VAY_VON: return 1100616;
                case DanhSachBaoCaoTheoDinhKy.TDVM_PHIEU_THEO_DOI_HOAN_TRA_CUM: return 1100617;
                case DanhSachBaoCaoTheoDinhKy.TDVM_PHIEU_THEO_DOI_HOAN_TRA_TVIEN: return 1100618;
                case DanhSachBaoCaoTheoDinhKy.TDVM_DS_KHTV_NHAN_VON_00: return 1100619;
                case DanhSachBaoCaoTheoDinhKy.TDVM_KHE_UOC_NHAN_NO_00: return 1100620;
                case DanhSachBaoCaoTheoDinhKy.BHTH_BAO_VE_VON_VAY: return 1100701;
                case DanhSachBaoCaoTheoDinhKy.BHTH_HO_TRO_MO_DE: return 1100702;
                case DanhSachBaoCaoTheoDinhKy.BHTH_CHI_TRA_NHAN_THO: return 1100703;
                case DanhSachBaoCaoTheoDinhKy.BHTH_CHI_TRA_TU_VONG: return 1100704;
                case DanhSachBaoCaoTheoDinhKy.BHTH_XOA_NO_VON_VAY: return 1100705;
                case DanhSachBaoCaoTheoDinhKy.BHTH_THU_CHI: return 1100706;
                case DanhSachBaoCaoTheoDinhKy.BHTH_TIEN_DO: return 1100707;
                case DanhSachBaoCaoTheoDinhKy.BHTH_DSACH_BAO_VE_VON_VAY: return 1100708;
                case DanhSachBaoCaoTheoDinhKy.BHTH_PHIEU_YEU_CAU_BAO_VE_VON_VAY: return 1100709;
                case DanhSachBaoCaoTheoDinhKy.NSTL_PHU_CAP_CUM_TRUONG: return 1100801;
                case DanhSachBaoCaoTheoDinhKy.BCTH_BC_VAN_HANH: return 1101001;
                case DanhSachBaoCaoTheoDinhKy.BCTH_BANG_CHI_SO_PEAL: return 1101002;
                case DanhSachBaoCaoTheoDinhKy.BCTH_BANG_CHI_TIEU_THEO_NGAN_HANG: return 1101003;
                case DanhSachBaoCaoTheoDinhKy.BCTH_BANG_CHI_SO_BAO_CAO_NHA_TAI_TRO: return 1101004;
                case DanhSachBaoCaoTheoDinhKy.BCTH_THONG_KE_TH: return 1101005;
                case DanhSachBaoCaoTheoDinhKy.BCTH_KH_THU_CHI_TIEN_MAT_TRONG_THANG: return 1101006;
                case DanhSachBaoCaoTheoDinhKy.BCTH_BC_TIEN_DO: return 1101007;
                case DanhSachBaoCaoTheoDinhKy.BCTH_TH_THU_CHI: return 1101008;
                case DanhSachBaoCaoTheoDinhKy.BCTK_DU_NO_TD_THEO_NGANH: return 1101101;
                case DanhSachBaoCaoTheoDinhKy.BCTK_DU_NO_TD_THEO_LH_TOCHUC_CANHAN: return 1101102;
                case DanhSachBaoCaoTheoDinhKy.BCTK_HOAT_DONG_HDV: return 1101103;
                case DanhSachBaoCaoTheoDinhKy.BCTK_BC_PLOAI_NO_DP_RUIRO: return 1101104;
                case DanhSachBaoCaoTheoDinhKy.BCTK_BC_XULY_RUIRO_CHOVAY: return 1101105;
                case DanhSachBaoCaoTheoDinhKy.BCTK_BC_TYLE_DAM_BAO_ANTOAN: return 1101106;
                case DanhSachBaoCaoTheoDinhKy.BCTK_BC_KHANG_CHOVAY_VUOT_QDINH: return 1101107;
                case DanhSachBaoCaoTheoDinhKy.BCTK_BANG_CAN_DOI: return 1101108;
                case DanhSachBaoCaoTheoDinhKy.BCTK_BC_KET_QUA_HD_KD: return 1101109;
                case DanhSachBaoCaoTheoDinhKy.BCTH_TH_LICH_SU: return 1101009;
                default: return 0;
            }
        }

        public static int layTinhTrangGhepBaoCao(this DanhSachBaoCaoTheoDinhKy danhSachBaoCao)
        {
            switch (danhSachBaoCao)
            {
                case DanhSachBaoCaoTheoDinhKy.QTHT_BAO_CAO_KIEM_SOAT: return 0;
                case DanhSachBaoCaoTheoDinhKy.KHTV_PHIEU_KHAO_SAT: return 0;
                case DanhSachBaoCaoTheoDinhKy.KHTV_PHIEU_DANH_GIA: return 0;
                case DanhSachBaoCaoTheoDinhKy.KHTV_PHIEU_XEP_HANG_TD: return 1;
                case DanhSachBaoCaoTheoDinhKy.KHTV_DS_KHTV: return 0;
                case DanhSachBaoCaoTheoDinhKy.KHTV_DS_KHCN: return 0;
                case DanhSachBaoCaoTheoDinhKy.KHTV_DS_KHTC: return 1;
                case DanhSachBaoCaoTheoDinhKy.KHTV_DS_KHTV_RA_KHOI_NHOM: return 0;
                case DanhSachBaoCaoTheoDinhKy.KHTV_NGUYEN_NHAN_RA_KHOI_NHOM: return 0;
                case DanhSachBaoCaoTheoDinhKy.KHTV_DAC_DIEM_HOKHTV: return 0;
                case DanhSachBaoCaoTheoDinhKy.KHTV_TANG_TRUONG_KT_KHTV: return 1;
                case DanhSachBaoCaoTheoDinhKy.KHTV_THU_NHAP_HOKHTV: return 0;
                case DanhSachBaoCaoTheoDinhKy.KHTV_CHI_TIEU_HOKHTV: return 0;
                case DanhSachBaoCaoTheoDinhKy.GDKT_NHAT_KY_CHUNG: return 0;
                case DanhSachBaoCaoTheoDinhKy.GDKT_NHAT_KY_CHI_TIEN: return 0;
                case DanhSachBaoCaoTheoDinhKy.GDKT_NHAT_KY_THU_TIEN: return 0;
                case DanhSachBaoCaoTheoDinhKy.GDKT_SO_CAI_TAI_KHOAN: return 0;
                case DanhSachBaoCaoTheoDinhKy.GDKT_SO_QUY_TIEN_MAT: return 0;
                case DanhSachBaoCaoTheoDinhKy.GDKT_SO_CHI_TIET_TAI_KHOAN: return 0;
                case DanhSachBaoCaoTheoDinhKy.GDKT_BANG_KE_TIEN_MAT: return 0;
                case DanhSachBaoCaoTheoDinhKy.GDKT_GIAY_GUI_TIEN_TIET_KIEM: return 0;
                case DanhSachBaoCaoTheoDinhKy.GDKT_PHIEU_CHI: return 0;
                case DanhSachBaoCaoTheoDinhKy.GDKT_PHIEU_HACH_TOAN: return 0;
                case DanhSachBaoCaoTheoDinhKy.GDKT_PHIEU_THU: return 0;
                case DanhSachBaoCaoTheoDinhKy.GDKT_BANG_CAN_DOI_NOI_BANG: return 0;
                case DanhSachBaoCaoTheoDinhKy.GDKT_BANG_CAN_DOI_NGOAI_BANG: return 1;
                case DanhSachBaoCaoTheoDinhKy.GDKT_BANG_CAN_DOI_KE_TOAN: return 0;
                case DanhSachBaoCaoTheoDinhKy.GDKT_KET_QUA_KINH_DOANH: return 0;
                case DanhSachBaoCaoTheoDinhKy.GDKT_LUU_CHUYEN_TIEN_TE: return 1;
                case DanhSachBaoCaoTheoDinhKy.GDKT_THUYET_MINH_TAI_CHINH: return 0;
                case DanhSachBaoCaoTheoDinhKy.GDKT_SO_TIEN_GUI_NGAN_HANG: return 0;
                case DanhSachBaoCaoTheoDinhKy.GDKT_SO_CT_TAI_KHOAN_CONG_NO: return 1;
                case DanhSachBaoCaoTheoDinhKy.GDKT_SO_TH_CONG_NO_PHAI_THU: return 1;
                case DanhSachBaoCaoTheoDinhKy.GDKT_SO_TH_CONG_NO_PHAI_TRA: return 1;
                case DanhSachBaoCaoTheoDinhKy.GDKT_SO_TH_CONG_NO_TAM_UNG: return 1;
                case DanhSachBaoCaoTheoDinhKy.GDKT_SO_TH_TKHOAN_THEO_DOI_UNG: return 0;
                case DanhSachBaoCaoTheoDinhKy.GDKT_SO_THEO_DOI_CT_NVON_KDOANH: return 1;
                case DanhSachBaoCaoTheoDinhKy.HDVO_BANG_KE_TKQD_TKKKH: return 0;
                case DanhSachBaoCaoTheoDinhKy.HDVO_BANG_KE_TKCKH: return 0;
                case DanhSachBaoCaoTheoDinhKy.HDVO_SO_PHU_TIEN_GUI: return 0;
                case DanhSachBaoCaoTheoDinhKy.HDVO_TONG_HOP_TKCKH: return 0;
                case DanhSachBaoCaoTheoDinhKy.HDVO_BANG_XAC_NHAN_SO_DU_TK: return 1;
                case DanhSachBaoCaoTheoDinhKy.HDVO_SO_THEO_DOI_TK: return 1;
                case DanhSachBaoCaoTheoDinhKy.TDVM_HDTD: return 0;
                case DanhSachBaoCaoTheoDinhKy.TDVM_KHE_UOC_NHAN_NO: return 0;
                case DanhSachBaoCaoTheoDinhKy.TDVM_DS_KHTV_NHAN_VON: return 0;
                case DanhSachBaoCaoTheoDinhKy.TDVM_HOA_DON_THU_TIEN_KY: return 0;
                case DanhSachBaoCaoTheoDinhKy.TDVM_SAO_KE_TH: return 0;
                case DanhSachBaoCaoTheoDinhKy.TDVM_SAO_KE_VONNH: return 0;
                case DanhSachBaoCaoTheoDinhKy.TDVM_SAO_KE_VONTH: return 0;
                case DanhSachBaoCaoTheoDinhKy.TDVM_SAO_KE_VONDH: return 0;
                case DanhSachBaoCaoTheoDinhKy.TDVM_THONG_KE_TH: return 0;
                case DanhSachBaoCaoTheoDinhKy.TDVM_BC_CHAM_TRA: return 0;
                case DanhSachBaoCaoTheoDinhKy.KHTV_KH_THU_CHI_TIEN_MAT_TRONG_THANG: return 0;
                case DanhSachBaoCaoTheoDinhKy.KHTV_BC_TIEN_DO: return 0;
                case DanhSachBaoCaoTheoDinhKy.TDVM_BC_VON_TIET_KIEM_TVIEN: return 0;
                case DanhSachBaoCaoTheoDinhKy.TDVM_BC_VON_TIET_KIEM_CUM: return 0;
                case DanhSachBaoCaoTheoDinhKy.TDVM_BC_PHAN_LOAI_NO: return 0;
                case DanhSachBaoCaoTheoDinhKy.TDVM_BANG_XAC_NHAN_SO_DU_VAY_VON: return 0;
                case DanhSachBaoCaoTheoDinhKy.TDVM_PHIEU_THEO_DOI_HOAN_TRA_CUM: return 1;
                case DanhSachBaoCaoTheoDinhKy.TDVM_PHIEU_THEO_DOI_HOAN_TRA_TVIEN: return 1;
                case DanhSachBaoCaoTheoDinhKy.TDVM_DS_KHTV_NHAN_VON_00: return 1;
                case DanhSachBaoCaoTheoDinhKy.TDVM_KHE_UOC_NHAN_NO_00: return 0;
                case DanhSachBaoCaoTheoDinhKy.BHTH_BAO_VE_VON_VAY: return 0;
                case DanhSachBaoCaoTheoDinhKy.BHTH_HO_TRO_MO_DE: return 0;
                case DanhSachBaoCaoTheoDinhKy.BHTH_CHI_TRA_NHAN_THO: return 0;
                case DanhSachBaoCaoTheoDinhKy.BHTH_CHI_TRA_TU_VONG: return 0;
                case DanhSachBaoCaoTheoDinhKy.BHTH_XOA_NO_VON_VAY: return 0;
                case DanhSachBaoCaoTheoDinhKy.BHTH_THU_CHI: return 0;
                case DanhSachBaoCaoTheoDinhKy.BHTH_TIEN_DO: return 0;
                case DanhSachBaoCaoTheoDinhKy.BHTH_DSACH_BAO_VE_VON_VAY: return 0;
                case DanhSachBaoCaoTheoDinhKy.BHTH_PHIEU_YEU_CAU_BAO_VE_VON_VAY: return 0;
                case DanhSachBaoCaoTheoDinhKy.NSTL_PHU_CAP_CUM_TRUONG: return 0;
                case DanhSachBaoCaoTheoDinhKy.BCTH_BC_VAN_HANH: return 0;
                case DanhSachBaoCaoTheoDinhKy.BCTH_BANG_CHI_SO_PEAL: return 1;
                case DanhSachBaoCaoTheoDinhKy.BCTH_BANG_CHI_TIEU_THEO_NGAN_HANG: return 0;
                case DanhSachBaoCaoTheoDinhKy.BCTH_BANG_CHI_SO_BAO_CAO_NHA_TAI_TRO: return 0;
                case DanhSachBaoCaoTheoDinhKy.BCTH_THONG_KE_TH: return 1;
                case DanhSachBaoCaoTheoDinhKy.BCTH_KH_THU_CHI_TIEN_MAT_TRONG_THANG: return 1;
                case DanhSachBaoCaoTheoDinhKy.BCTH_BC_TIEN_DO: return 0;
                case DanhSachBaoCaoTheoDinhKy.BCTH_TH_THU_CHI: return 1;
                case DanhSachBaoCaoTheoDinhKy.BCTK_DU_NO_TD_THEO_NGANH: return 0;
                case DanhSachBaoCaoTheoDinhKy.BCTK_DU_NO_TD_THEO_LH_TOCHUC_CANHAN: return 0;
                case DanhSachBaoCaoTheoDinhKy.BCTK_HOAT_DONG_HDV: return 0;
                case DanhSachBaoCaoTheoDinhKy.BCTK_BC_PLOAI_NO_DP_RUIRO: return 0;
                case DanhSachBaoCaoTheoDinhKy.BCTK_BC_XULY_RUIRO_CHOVAY: return 0;
                case DanhSachBaoCaoTheoDinhKy.BCTK_BC_TYLE_DAM_BAO_ANTOAN: return 0;
                case DanhSachBaoCaoTheoDinhKy.BCTK_BC_KHANG_CHOVAY_VUOT_QDINH: return 0;
                case DanhSachBaoCaoTheoDinhKy.BCTK_BANG_CAN_DOI: return 0;
                case DanhSachBaoCaoTheoDinhKy.BCTK_BC_KET_QUA_HD_KD: return 0;
                // TaiNM 20161101 Add
                case DanhSachBaoCaoTheoDinhKy.GDKT_TONG_HOP_DU_LIEU_KTOAN: return 0;
                default: return 100;
            }
        }


        public enum DanhSachBaoCaoDungChung
        {
            GDKT_PHIEU_THU,
            GDKT_PHIEU_CHI,
            GDKT_PHIEU_KE_TOAN,
            GDKT_NX_NGOAI_BANG,
            GDKT_SO_CAI_TAI_KHOAN,
            GDKT_SO_CHI_TIET_TAI_KHOAN,
            GDKT_SO_NHAT_KY_CHUNG,
            GDKT_SO_NHAT_KY_CHUNG_TELLER,
            GDKT_SO_QUY_TIEN_MAT,
            GDKT_SO_QUY_TIEN_MAT_TELLER,
            GDKT_SO_TIEN_GUI_NGAN_HANG,
            GDKT_SO_NHAT_KY_CHI,
            GDKT_SO_NHAT_KY_CHI_TELLER,
            GDKT_SO_NHAT_KY_THU,
            GDKT_SO_NHAT_KY_THU_TELLER,
            GDKT_BANG_CAN_DOI_TKKT,
            HDVO_SO_THEO_DOI_TK_KHTV,
            HDVO_BANG_TINH_LAI_KHTV,
            HDVO_GIAY_GUI_TIEN,
            HDVO_BAO_CAO_THEO_DIA_BAN,
            HDVO_BAO_CAO_THEO_CAN_BO,
            HDVO_DANH_SACH_KHTV,
            HDVO_DANH_SACH_HOAN_TK,
            HDVO_BANG_KE_LAI_NHAP_GOC,
            HDVO_SO_PHU_TIEN_GUI,
            HDVO_SAO_KE_TKCKH,
            TDVM_DS_THU_TIEN_PHAT_HANH,
            TDVM_DS_THU_TIEN_VON_TRA_DAN_TK,
            TDVM_SO_THEO_DOI_VON_VAY,
            TDVM_SO_THEO_DOI_KHTV,
            TDVM_SO_THEO_DOI_NO_RUI_RO,
            TDVM_PHAN_LOAI_NO_TLDP,
            TDVM_XU_LY_NO,
            TDVM_SAO_KE_VON_NGAN_HAN,
            TDVM_SAO_KE_VON_TRUNG_HAN,
            TDVM_SAO_KE_VON_DAI_HAN,
            TDVM_SAO_KE_VON_TONG_HOP,
            BCTC_BANG_CAN_DOI_TKKT,
            TDVM_SAO_KE_TH,
            GDKT_SO_NHAP_XUAT_NGOAI_TE,
            // TaiNM 20161101 Add
            GDKT_TONG_HOP_DU_LIEU_KTOAN
        }

        public static string LayMaBaoCaoDungChung(DanhSachBaoCaoDungChung dsbaocao)
        {
            switch (dsbaocao)
            {
                case DanhSachBaoCaoDungChung.GDKT_PHIEU_THU: return "GDKT_PHIEU_THU";
                case DanhSachBaoCaoDungChung.GDKT_PHIEU_CHI: return "GDKT_PHIEU_CHI";
                case DanhSachBaoCaoDungChung.GDKT_PHIEU_KE_TOAN: return "GDKT_PHIEU_KE_TOAN";
                case DanhSachBaoCaoDungChung.GDKT_NX_NGOAI_BANG: return "GDKT_NX_NGOAI_BANG";
                case DanhSachBaoCaoDungChung.GDKT_SO_CAI_TAI_KHOAN: return "GDKT_SO_CAI_TAI_KHOAN";
                case DanhSachBaoCaoDungChung.GDKT_SO_CHI_TIET_TAI_KHOAN: return "GDKT_SO_CHI_TIET_TAI_KHOAN";
                case DanhSachBaoCaoDungChung.GDKT_SO_NHAT_KY_CHUNG: return "GDKT_SO_NHAT_KY_CHUNG";
                case DanhSachBaoCaoDungChung.GDKT_SO_NHAT_KY_CHUNG_TELLER: return "GDKT_SO_NHAT_KY_CHUNG_TELLER";
                case DanhSachBaoCaoDungChung.GDKT_SO_QUY_TIEN_MAT: return "GDKT_SO_QUY_TIEN_MAT";
                case DanhSachBaoCaoDungChung.GDKT_SO_QUY_TIEN_MAT_TELLER: return "GDKT_SO_QUY_TIEN_MAT_TELLER";
                case DanhSachBaoCaoDungChung.GDKT_SO_TIEN_GUI_NGAN_HANG: return "GDKT_SO_TIEN_GUI_NGAN_HANG";
                case DanhSachBaoCaoDungChung.GDKT_SO_NHAT_KY_CHI: return "GDKT_SO_NHAT_KY_CHI";
                case DanhSachBaoCaoDungChung.GDKT_SO_NHAT_KY_THU: return "GDKT_SO_NHAT_KY_THU";
                case DanhSachBaoCaoDungChung.GDKT_SO_NHAT_KY_CHI_TELLER: return "GDKT_SO_NHAT_KY_CHI_TELLER";
                case DanhSachBaoCaoDungChung.GDKT_SO_NHAT_KY_THU_TELLER: return "GDKT_SO_NHAT_KY_THU_TELLER";
                case DanhSachBaoCaoDungChung.GDKT_BANG_CAN_DOI_TKKT: return "GDKT_BANG_CAN_DOI_TKKT";
                case DanhSachBaoCaoDungChung.HDVO_SO_THEO_DOI_TK_KHTV: return "HDVO_SO_THEO_DOI_TK_KHTV";
                case DanhSachBaoCaoDungChung.HDVO_BANG_TINH_LAI_KHTV: return "HDVO_BANG_TINH_LAI_KHTV";
                case DanhSachBaoCaoDungChung.HDVO_GIAY_GUI_TIEN: return "HDVO_GIAY_GUI_TIEN";
                case DanhSachBaoCaoDungChung.HDVO_BAO_CAO_THEO_DIA_BAN: return "HDVO_BAO_CAO_THEO_DIA_BAN";
                case DanhSachBaoCaoDungChung.HDVO_BAO_CAO_THEO_CAN_BO: return "HDVO_BAO_CAO_THEO_CAN_BO";
                case DanhSachBaoCaoDungChung.HDVO_DANH_SACH_KHTV: return "HDVO_DANH_SACH_KHTV";
                case DanhSachBaoCaoDungChung.HDVO_DANH_SACH_HOAN_TK: return "HDVO_DANH_SACH_HOAN_TK";
                case DanhSachBaoCaoDungChung.HDVO_BANG_KE_LAI_NHAP_GOC: return "HDVO_BANG_KE_LAI_NHAP_GOC";
                case DanhSachBaoCaoDungChung.HDVO_SO_PHU_TIEN_GUI: return "HDVO_SO_PHU_TIEN_GUI";
                case DanhSachBaoCaoDungChung.HDVO_SAO_KE_TKCKH: return "HDVO_SAO_KE_TKCKH";  
                case DanhSachBaoCaoDungChung.TDVM_DS_THU_TIEN_PHAT_HANH: return "TDVM_DS_THU_TIEN_PHAT_HANH";
                case DanhSachBaoCaoDungChung.TDVM_DS_THU_TIEN_VON_TRA_DAN_TK: return "TDVM_DS_THU_TIEN_VON_TRA_DAN_TK";
                case DanhSachBaoCaoDungChung.TDVM_SO_THEO_DOI_VON_VAY: return "TDVM_SO_THEO_DOI_VON_VAY";
                case DanhSachBaoCaoDungChung.TDVM_SO_THEO_DOI_KHTV: return "TDVM_SO_THEO_DOI_KHTV";
                case DanhSachBaoCaoDungChung.TDVM_SO_THEO_DOI_NO_RUI_RO: return "TDVM_SO_THEO_DOI_NO_RUI_RO";
                case DanhSachBaoCaoDungChung.TDVM_PHAN_LOAI_NO_TLDP: return "TDVM_PHAN_LOAI_NO_TLDP";
                case DanhSachBaoCaoDungChung.TDVM_XU_LY_NO: return "TDVM_XU_LY_NO";
                case DanhSachBaoCaoDungChung.TDVM_SAO_KE_VON_NGAN_HAN: return "TDVM_SAO_KE_VON_NGAN_HAN";
                case DanhSachBaoCaoDungChung.TDVM_SAO_KE_VON_TRUNG_HAN: return "TDVM_SAO_KE_VON_TRUNG_HAN";
                case DanhSachBaoCaoDungChung.TDVM_SAO_KE_VON_DAI_HAN: return "TDVM_SAO_KE_VON_DAI_HAN";
                case DanhSachBaoCaoDungChung.TDVM_SAO_KE_VON_TONG_HOP: return "TDVM_SAO_KE_VON_TONG_HOP";
                case DanhSachBaoCaoDungChung.BCTC_BANG_CAN_DOI_TKKT: return "BCTC_BANG_CAN_DOI_TKKT";
                case DanhSachBaoCaoDungChung.TDVM_SAO_KE_TH: return "TDVM_SAO_KE_TH";
                case DanhSachBaoCaoDungChung.GDKT_SO_NHAP_XUAT_NGOAI_TE: return "GDKT_SO_NHAP_XUAT_NGOAI_TE";
                // TaiNM 20161101 Add
                case DanhSachBaoCaoDungChung.GDKT_TONG_HOP_DU_LIEU_KTOAN: return "GDKT_TONG_HOP_DU_LIEU_KTOAN";
                default: return "";
            }
        }

        public static DanhSachBaoCaoDungChung layBaoCaoDungChungTheoMa(string maBaoCao)
        {
            switch (maBaoCao)
            {
                case "GDKT_PHIEU_THU": return DanhSachBaoCaoDungChung.GDKT_PHIEU_THU;
                case "GDKT_PHIEU_CHI": return DanhSachBaoCaoDungChung.GDKT_PHIEU_CHI;
                case "GDKT_PHIEU_KE_TOAN": return DanhSachBaoCaoDungChung.GDKT_PHIEU_KE_TOAN;
                case "GDKT_NX_NGOAI_BANG": return DanhSachBaoCaoDungChung.GDKT_NX_NGOAI_BANG;
                case "GDKT_SO_CAI_TAI_KHOAN": return DanhSachBaoCaoDungChung.GDKT_SO_CAI_TAI_KHOAN;
                case "GDKT_SO_CHI_TIET_TAI_KHOAN": return DanhSachBaoCaoDungChung.GDKT_SO_CHI_TIET_TAI_KHOAN;
                case "GDKT_SO_NHAT_KY_CHUNG": return DanhSachBaoCaoDungChung.GDKT_SO_NHAT_KY_CHUNG;
                case "GDKT_SO_NHAT_KY_CHUNG_TELLER": return DanhSachBaoCaoDungChung.GDKT_SO_NHAT_KY_CHUNG_TELLER;
                case "GDKT_SO_QUY_TIEN_MAT": return DanhSachBaoCaoDungChung.GDKT_SO_QUY_TIEN_MAT;
                case "GDKT_SO_QUY_TIEN_MAT_TELLER": return DanhSachBaoCaoDungChung.GDKT_SO_QUY_TIEN_MAT_TELLER;
                case "GDKT_SO_TIEN_GUI_NGAN_HANG": return DanhSachBaoCaoDungChung.GDKT_SO_TIEN_GUI_NGAN_HANG;
                case "GDKT_SO_NHAT_KY_CHI": return DanhSachBaoCaoDungChung.GDKT_SO_NHAT_KY_CHI;
                case "GDKT_SO_NHAT_KY_CHI_TELLER": return DanhSachBaoCaoDungChung.GDKT_SO_NHAT_KY_CHI_TELLER;
                case "GDKT_SO_NHAT_KY_THU": return DanhSachBaoCaoDungChung.GDKT_SO_NHAT_KY_THU;
                case "GDKT_SO_NHAT_KY_THU_TELLER": return DanhSachBaoCaoDungChung.GDKT_SO_NHAT_KY_THU_TELLER;
                case "GDKT_BANG_CAN_DOI_TKKT": return DanhSachBaoCaoDungChung.GDKT_BANG_CAN_DOI_TKKT;
                case "HDVO_SO_THEO_DOI_TK_KHTV": return DanhSachBaoCaoDungChung.HDVO_SO_THEO_DOI_TK_KHTV;
                case "HDVO_BANG_TINH_LAI_KHTV": return DanhSachBaoCaoDungChung.HDVO_BANG_TINH_LAI_KHTV;
                case "HDVO_GIAY_GUI_TIEN": return DanhSachBaoCaoDungChung.HDVO_GIAY_GUI_TIEN;
                case "HDVO_BAO_CAO_THEO_DIA_BAN": return DanhSachBaoCaoDungChung.HDVO_BAO_CAO_THEO_DIA_BAN;
                case "HDVO_BAO_CAO_THEO_CAN_BO": return DanhSachBaoCaoDungChung.HDVO_BAO_CAO_THEO_CAN_BO;
                case "HDVO_DANH_SACH_KHTV": return DanhSachBaoCaoDungChung.HDVO_DANH_SACH_KHTV;
                case "HDVO_DANH_SACH_HOAN_TK": return DanhSachBaoCaoDungChung.HDVO_DANH_SACH_HOAN_TK;
                case "HDVO_BANG_KE_LAI_NHAP_GOC": return DanhSachBaoCaoDungChung.HDVO_BANG_KE_LAI_NHAP_GOC;
                case "HDVO_SO_PHU_TIEN_GUI": return DanhSachBaoCaoDungChung.HDVO_SO_PHU_TIEN_GUI;
                case "HDVO_SAO_KE_TKCKH": return DanhSachBaoCaoDungChung.HDVO_SAO_KE_TKCKH;                    
                case "TDVM_DS_THU_TIEN_PHAT_HANH": return DanhSachBaoCaoDungChung.TDVM_DS_THU_TIEN_PHAT_HANH;
                case "TDVM_DS_THU_TIEN_VON_TRA_DAN_TK": return DanhSachBaoCaoDungChung.TDVM_DS_THU_TIEN_VON_TRA_DAN_TK;
                case "TDVM_SO_THEO_DOI_VON_VAY": return DanhSachBaoCaoDungChung.TDVM_SO_THEO_DOI_VON_VAY;
                case "TDVM_SO_THEO_DOI_KHTV": return DanhSachBaoCaoDungChung.TDVM_SO_THEO_DOI_KHTV;
                case "TDVM_SO_THEO_DOI_NO_RUI_RO": return DanhSachBaoCaoDungChung.TDVM_SO_THEO_DOI_NO_RUI_RO;
                case "TDVM_PHAN_LOAI_NO_TLDP": return DanhSachBaoCaoDungChung.TDVM_PHAN_LOAI_NO_TLDP;
                case "TDVM_XU_LY_NO": return DanhSachBaoCaoDungChung.TDVM_XU_LY_NO;
                case "TDVM_SAO_KE_VON_NGAN_HAN": return DanhSachBaoCaoDungChung.TDVM_SAO_KE_VON_NGAN_HAN;
                case "TDVM_SAO_KE_VON_TRUNG_HAN": return DanhSachBaoCaoDungChung.TDVM_SAO_KE_VON_TRUNG_HAN;
                case "TDVM_SAO_KE_VON_DAI_HAN": return DanhSachBaoCaoDungChung.TDVM_SAO_KE_VON_DAI_HAN;
                case "TDVM_SAO_KE_VON_TONG_HOP": return DanhSachBaoCaoDungChung.TDVM_SAO_KE_VON_TONG_HOP;
                case "BCTC_BANG_CAN_DOI_TKKT": return DanhSachBaoCaoDungChung.BCTC_BANG_CAN_DOI_TKKT;
                case "TDVM_SAO_KE_TH": return DanhSachBaoCaoDungChung.TDVM_SAO_KE_TH;
                case "GDKT_SO_NHAP_XUAT_NGOAI_TE": return DanhSachBaoCaoDungChung.GDKT_SO_NHAP_XUAT_NGOAI_TE;
                // TaiNM 20161101 Add
                case "GDKT_TONG_HOP_DU_LIEU_KTOAN": return DanhSachBaoCaoDungChung.GDKT_TONG_HOP_DU_LIEU_KTOAN;

                default: return DanhSachBaoCaoDungChung.GDKT_PHIEU_THU;
            }
        }

        public static bool LayGiaTriBaoCaoDungChung(string maBaoCao)
        {
            switch (maBaoCao)
            {
                case "GDKT_PHIEU_THU": return true;
                case "GDKT_PHIEU_CHI": return true;
                case "GDKT_PHIEU_KE_TOAN": return true;
                case "GDKT_NX_NGOAI_BANG": return true;
                case "GDKT_SO_CAI_TAI_KHOAN": return true;
                case "GDKT_SO_CHI_TIET_TAI_KHOAN": return true;
                case "GDKT_SO_NHAT_KY_CHUNG": return true;
                case "GDKT_SO_QUY_TIEN_MAT": return true;
                case "GDKT_SO_NHAT_KY_CHUNG_TELLER": return true;
                case "GDKT_SO_QUY_TIEN_MAT_TELLER": return true;
                case "GDKT_SO_TIEN_GUI_NGAN_HANG": return true;
                case "GDKT_SO_NHAT_KY_CHI": return true;
                case "GDKT_SO_NHAT_KY_THU": return true;
                case "GDKT_SO_NHAT_KY_CHI_TELLER": return true;
                case "GDKT_SO_NHAT_KY_THU_TELLER": return true;
                case "GDKT_BANG_CAN_DOI_TKKT": return true;
                case "HDVO_SO_THEO_DOI_TK_KHTV": return true;
                case "HDVO_BANG_TINH_LAI_KHTV": return true;
                case "HDVO_GIAY_GUI_TIEN": return true;
                case "HDVO_BAO_CAO_THEO_DIA_BAN": return true;
                case "HDVO_BAO_CAO_THEO_CAN_BO": return true;
                case "HDVO_DANH_SACH_KHTV": return true;
                case "HDVO_DANH_SACH_HOAN_TK": return true;
                case "HDVO_BANG_KE_LAI_NHAP_GOC": return true;
                case "HDVO_SO_PHU_TIEN_GUI": return true;
                case "HDVO_SAO_KE_TKCKH": return true; 
                case "TDVM_DS_THU_TIEN_PHAT_HANH": return true;
                case "TDVM_DS_THU_TIEN_VON_TRA_DAN_TK": return true;
                case "TDVM_SO_THEO_DOI_VON_VAY": return true;
                case "TDVM_SO_THEO_DOI_KHTV": return true;
                case "TDVM_SO_THEO_DOI_NO_RUI_RO": return true;
                case "TDVM_PHAN_LOAI_NO_TLDP": return true;
                case "TDVM_XU_LY_NO": return true;
                case "TDVM_SAO_KE_VON_NGAN_HAN": return true;
                case "TDVM_SAO_KE_VON_TRUNG_HAN": return true;
                case "TDVM_SAO_KE_VON_DAI_HAN": return true;
                case "TDVM_SAO_KE_VON_TONG_HOP": return true;
                case "BCTC_BANG_CAN_DOI_TKKT": return true;
                case "TDVM_SAO_KE_TH": return true;
                // TaiNM 20161101 Add
                case "GDKT_TONG_HOP_DU_LIEU_KTOAN": return true;

                default: return false;
            }
        }




        public enum DanhSachBaoCaoBTV
        {
            KHTV_QUYET_DINH,
            KHTV_THAM_DINH,
            KHTV_HO_SO,
            GDKT_PHIEU_THU,
            GDKT_PHIEU_CHI,
            GDKT_PHIEU_KE_TOAN,
            GDKT_UY_NHIEM_CHI,
            GDKT_CHUNG_TU_GHI_SO,
            GDKT_SO_CAI_CTGS,
            GDKT_SO_CHI_TIET_TKHOAN,
            GDKT_SO_THEO_DOI_CONG_NO,
            GDKT_SO_THEO_DOI_TAM_UNG,
            GDKT_SO_TONG_HOP_NGHIEP_VU,
            GDKT_SO_QUY_TIEN_MAT,
            GDKT_SO_TIEN_GUI_NGAN_HANG,
            GDKT_NX_NGOAI_BANG,
            GDKT_SO_DANG_KY_CTGS,
            GDKT_KET_XUAT_BCDKT_THEO_CAU_TRUC,
            GDKT_SO_NHAT_KY_CHUNG,
            HDVO_DANH_SACH_HOAN_TK,
            HDVO_SO_PHU_TIEN_GUI,
            HDVO_SAO_KE_TKCKH,
            HDVO_SO_THEO_DOI_TK_KHTV,
            HDVO_BANG_TINH_LAI_KHTV,
            HDVO_SO_TKCKH,
            HDVO_GIAY_GUI_TIEN,
            HDVO_BAO_CAO_THEO_DIA_BAN,
            HDVO_BAO_CAO_THEO_CAN_BO,
            HDVO_DANH_SACH_KHTV,
            TDVM_DON_VAY_VON,
            TDVM_HOP_DONG_VAY_VON,
            TDVM_PHU_LUC_HOP_DONG,
            TDVM_SO_VON_VAY_TIET_KIEM_KHTV,
            TDVM_PHIEU_THAM_KHTV,
            TDVM_DS_THU_TIEN_PHAT_HANH,
            TDVM_DS_DE_NGHI_VAY_VON_CBTD,
            TDVM_DS_DE_NGHI_VAY_VON_TCN,
            TDVM_DS_PHAT_VON,
            TDVM_DS_THU_LAI_VON_MUA_VU,
            TDVM_DS_THU_TIEN_VON_TRA_DAN_TK,
            TDVM_DS_THU_VON_MUA_VU,
            TDVM_BANG_PHAN_BO_LAI_TRA_TRUOC,
            TDVM_SO_THEO_DOI_VON_VAY,
            TDVM_SO_THEO_DOI_KHTV,
            TDVM_LICH_THU_NO,
            TDVM_SO_THEO_DOI_NO_RUI_RO,
            TDVM_PHAN_LOAI_NO_TLDP,
            TDVM_XU_LY_NO,
            TDVM_SAO_KE_TH,
            NSTL_BANG_LUONG,
            NSTL_THE_LUONG,
            NSTL_DANH_SACH_CHUYEN_KHOAN,
            NSTL_CHI_TIEU_THUONG_CB,
            NSTL_CHI_TIEU_THUONG_DB,
            NSTL_THE_THUONG,
            NSTL_PHU_CAP_CTV,
            NSTL_TONG_HOP_CHI_PHI_LUONG,
            NSTL_HDLD_CHINH_THUC,
            NSTL_HDLD_THU_VIEC,
            NSTL_HDLD_HOC_VIEC,
            NSTL_DANH_SACH_NHAN_VIEN,
            QLTS_SO_CHI_TIET_TSCD,
            QLTS_BANG_KIEM_KE_TSCD,
            QLTS_BANG_KHAU_HAO,
            QLTS_BIEN_BAN_BAN_GIAO_TSCD,
            BCTH_HOAT_DONG_TCVM,
            BCTH_HOAT_DONG_CBTD_SP,
            BCTH_HOAT_DONG_DB,
            BCTH_HOAT_DONG_CBTD,
            BCTH_CONG_VIEC_CBTD,
            BCTH_DU_TRU_NGAN_QUY,
            BCTH_HOAT_DONG_KHCN_CBTD,
            BCTH_HOAT_DONG_KHCN_DB,
            BCTH_HOAT_DONG_KHTC_CBTD,
            BCTH_HOAT_DONG_KHTC_DB,
            BCTH_HOAT_DONG,
            BCTH_THEO_DOI_RUI_RO,
            BCTH_CHI_SO_TAI_CHINH,
            BCTC_BANG_CAN_DOI_TKKT,
            BCTC_BANG_CAN_DOI_KE_TOAN,
            BCTC_KET_QUA_HOAT_DONG,
            BCTC_LUU_CHUYEN_TIEN_TE,
            BCTC_THUYET_MINH_TAI_CHINH,
            BCTC_CHI_TIEU_CT_CHEVRON,
            BCTC_CHI_TIEU_TH_CHEVRON,
            BCTC_LUU_CHUYEN_TIEN_TE_CHEVRON,
            BCTC_CHI_TIEU_CT_DISNEY,
            BCTC_CHI_TIEU_TH_DISNEY,
            BCTC_LUU_CHUYEN_TIEN_TE_DISNEY
        }

        public static string LayMaBaoCaoBTV(DanhSachBaoCaoBTV dsbaocao)
        {
            switch (dsbaocao)
            {
                case DanhSachBaoCaoBTV.KHTV_QUYET_DINH: return "KHTV_QUYET_DINH";
                case DanhSachBaoCaoBTV.KHTV_THAM_DINH: return "KHTV_THAM_DINH";
                case DanhSachBaoCaoBTV.KHTV_HO_SO: return "KHTV_HO_SO";
                case DanhSachBaoCaoBTV.GDKT_PHIEU_THU: return "GDKT_PHIEU_THU";
                case DanhSachBaoCaoBTV.GDKT_PHIEU_CHI: return "GDKT_PHIEU_CHI";
                case DanhSachBaoCaoBTV.GDKT_PHIEU_KE_TOAN: return "GDKT_PHIEU_KE_TOAN";
                case DanhSachBaoCaoBTV.GDKT_UY_NHIEM_CHI: return "GDKT_UY_NHIEM_CHI";
                case DanhSachBaoCaoBTV.GDKT_CHUNG_TU_GHI_SO: return "GDKT_CHUNG_TU_GHI_SO";
                case DanhSachBaoCaoBTV.GDKT_SO_CAI_CTGS: return "GDKT_SO_CAI_CTGS";
                case DanhSachBaoCaoBTV.GDKT_SO_CHI_TIET_TKHOAN: return "GDKT_SO_CHI_TIET_TKHOAN";
                case DanhSachBaoCaoBTV.GDKT_SO_THEO_DOI_CONG_NO: return "GDKT_SO_THEO_DOI_CONG_NO";
                case DanhSachBaoCaoBTV.GDKT_SO_THEO_DOI_TAM_UNG: return "GDKT_SO_THEO_DOI_TAM_UNG";
                case DanhSachBaoCaoBTV.GDKT_SO_TONG_HOP_NGHIEP_VU: return "GDKT_SO_TONG_HOP_NGHIEP_VU";
                case DanhSachBaoCaoBTV.GDKT_SO_QUY_TIEN_MAT: return "GDKT_SO_QUY_TIEN_MAT";
                case DanhSachBaoCaoBTV.GDKT_SO_TIEN_GUI_NGAN_HANG: return "GDKT_SO_TIEN_GUI_NGAN_HANG";
                case DanhSachBaoCaoBTV.GDKT_NX_NGOAI_BANG: return "GDKT_NX_NGOAI_BANG";
                case DanhSachBaoCaoBTV.GDKT_SO_DANG_KY_CTGS: return "GDKT_SO_DANG_KY_CTGS";
                case DanhSachBaoCaoBTV.GDKT_KET_XUAT_BCDKT_THEO_CAU_TRUC: return "GDKT_KET_XUAT_BCDKT_THEO_CAU_TRUC";
                case DanhSachBaoCaoBTV.GDKT_SO_NHAT_KY_CHUNG: return "GDKT_SO_NHAT_KY_CHUNG";                    
                case DanhSachBaoCaoBTV.HDVO_DANH_SACH_HOAN_TK: return "HDVO_DANH_SACH_HOAN_TK";
                case DanhSachBaoCaoBTV.HDVO_SO_PHU_TIEN_GUI: return "HDVO_SO_PHU_TIEN_GUI";
                case DanhSachBaoCaoBTV.HDVO_SAO_KE_TKCKH: return "HDVO_SAO_KE_TKCKH";
                case DanhSachBaoCaoBTV.HDVO_SO_THEO_DOI_TK_KHTV: return "HDVO_SO_THEO_DOI_TK_KHTV";
                case DanhSachBaoCaoBTV.HDVO_BANG_TINH_LAI_KHTV: return "HDVO_BANG_TINH_LAI_KHTV";
                case DanhSachBaoCaoBTV.HDVO_SO_TKCKH: return "HDVO_SO_TKCKH";
                case DanhSachBaoCaoBTV.HDVO_GIAY_GUI_TIEN: return "HDVO_GIAY_GUI_TIEN";
                case DanhSachBaoCaoBTV.HDVO_BAO_CAO_THEO_DIA_BAN: return "HDVO_BAO_CAO_THEO_DIA_BAN";
                case DanhSachBaoCaoBTV.HDVO_BAO_CAO_THEO_CAN_BO: return "HDVO_BAO_CAO_THEO_CAN_BO";
                case DanhSachBaoCaoBTV.HDVO_DANH_SACH_KHTV: return "HDVO_DANH_SACH_KHTV";
                case DanhSachBaoCaoBTV.TDVM_DON_VAY_VON: return "TDVM_DON_VAY_VON";
                case DanhSachBaoCaoBTV.TDVM_HOP_DONG_VAY_VON: return "TDVM_HOP_DONG_VAY_VON";
                case DanhSachBaoCaoBTV.TDVM_PHU_LUC_HOP_DONG: return "TDVM_PHU_LUC_HOP_DONG";
                case DanhSachBaoCaoBTV.TDVM_SO_VON_VAY_TIET_KIEM_KHTV: return "TDVM_SO_VON_VAY_TIET_KIEM_KHTV";
                case DanhSachBaoCaoBTV.TDVM_PHIEU_THAM_KHTV: return "TDVM_PHIEU_THAM_KHTV";
                case DanhSachBaoCaoBTV.TDVM_DS_THU_TIEN_PHAT_HANH: return "TDVM_DS_THU_TIEN_PHAT_HANH";
                case DanhSachBaoCaoBTV.TDVM_DS_DE_NGHI_VAY_VON_CBTD: return "TDVM_DS_DE_NGHI_VAY_VON_CBTD";
                case DanhSachBaoCaoBTV.TDVM_DS_DE_NGHI_VAY_VON_TCN: return "TDVM_DS_DE_NGHI_VAY_VON_TCN";
                case DanhSachBaoCaoBTV.TDVM_DS_PHAT_VON: return "TDVM_DS_PHAT_VON";
                case DanhSachBaoCaoBTV.TDVM_DS_THU_LAI_VON_MUA_VU: return "TDVM_DS_THU_LAI_VON_MUA_VU";
                case DanhSachBaoCaoBTV.TDVM_DS_THU_TIEN_VON_TRA_DAN_TK: return "TDVM_DS_THU_TIEN_VON_TRA_DAN_TK";
                case DanhSachBaoCaoBTV.TDVM_DS_THU_VON_MUA_VU: return "TDVM_DS_THU_VON_MUA_VU";
                case DanhSachBaoCaoBTV.TDVM_BANG_PHAN_BO_LAI_TRA_TRUOC: return "TDVM_BANG_PHAN_BO_LAI_TRA_TRUOC";
                case DanhSachBaoCaoBTV.TDVM_SO_THEO_DOI_VON_VAY: return "TDVM_SO_THEO_DOI_VON_VAY";
                case DanhSachBaoCaoBTV.TDVM_SO_THEO_DOI_KHTV: return "TDVM_SO_THEO_DOI_KHTV";
                case DanhSachBaoCaoBTV.TDVM_LICH_THU_NO: return "TDVM_LICH_THU_NO";
                case DanhSachBaoCaoBTV.TDVM_SO_THEO_DOI_NO_RUI_RO: return "TDVM_SO_THEO_DOI_NO_RUI_RO";
                case DanhSachBaoCaoBTV.TDVM_PHAN_LOAI_NO_TLDP: return "TDVM_PHAN_LOAI_NO_TLDP";
                case DanhSachBaoCaoBTV.TDVM_XU_LY_NO: return "TDVM_XU_LY_NO";
                case DanhSachBaoCaoBTV.TDVM_SAO_KE_TH: return "TDVM_SAO_KE_TH";
                case DanhSachBaoCaoBTV.NSTL_BANG_LUONG: return "NSTL_BANG_LUONG";
                case DanhSachBaoCaoBTV.NSTL_THE_LUONG: return "NSTL_THE_LUONG";
                case DanhSachBaoCaoBTV.NSTL_DANH_SACH_CHUYEN_KHOAN: return "NSTL_DANH_SACH_CHUYEN_KHOAN";
                case DanhSachBaoCaoBTV.NSTL_CHI_TIEU_THUONG_CB: return "NSTL_CHI_TIEU_THUONG_CB";
                case DanhSachBaoCaoBTV.NSTL_CHI_TIEU_THUONG_DB: return "NSTL_CHI_TIEU_THUONG_DB";
                case DanhSachBaoCaoBTV.NSTL_THE_THUONG: return "NSTL_THE_THUONG";
                case DanhSachBaoCaoBTV.NSTL_PHU_CAP_CTV: return "NSTL_PHU_CAP_CTV";
                case DanhSachBaoCaoBTV.NSTL_TONG_HOP_CHI_PHI_LUONG: return "NSTL_TONG_HOP_CHI_PHI_LUONG";
                case DanhSachBaoCaoBTV.NSTL_HDLD_CHINH_THUC: return "NSTL_HDLD_CHINH_THUC";
                case DanhSachBaoCaoBTV.NSTL_HDLD_THU_VIEC: return "NSTL_HDLD_THU_VIEC";
                case DanhSachBaoCaoBTV.NSTL_HDLD_HOC_VIEC: return "NSTL_HDLD_HOC_VIEC";
                case DanhSachBaoCaoBTV.NSTL_DANH_SACH_NHAN_VIEN: return "NSTL_DANH_SACH_NHAN_VIEN";
                case DanhSachBaoCaoBTV.QLTS_SO_CHI_TIET_TSCD: return "QLTS_SO_CHI_TIET_TSCD";
                case DanhSachBaoCaoBTV.QLTS_BANG_KIEM_KE_TSCD: return "QLTS_BANG_KIEM_KE_TSCD";
                case DanhSachBaoCaoBTV.QLTS_BANG_KHAU_HAO: return "QLTS_BANG_KHAU_HAO";
                case DanhSachBaoCaoBTV.QLTS_BIEN_BAN_BAN_GIAO_TSCD: return "QLTS_BIEN_BAN_BAN_GIAO_TSCD";
                case DanhSachBaoCaoBTV.BCTH_HOAT_DONG_TCVM: return "BCTH_HOAT_DONG_TCVM";
                case DanhSachBaoCaoBTV.BCTH_HOAT_DONG_CBTD_SP: return "BCTH_HOAT_DONG_CBTD_SP";
                case DanhSachBaoCaoBTV.BCTH_HOAT_DONG_DB: return "BCTH_HOAT_DONG_DB";
                case DanhSachBaoCaoBTV.BCTH_HOAT_DONG_CBTD: return "BCTH_HOAT_DONG_CBTD";
                case DanhSachBaoCaoBTV.BCTH_CONG_VIEC_CBTD: return "BCTH_CONG_VIEC_CBTD";
                case DanhSachBaoCaoBTV.BCTH_DU_TRU_NGAN_QUY: return "BCTH_DU_TRU_NGAN_QUY";
                case DanhSachBaoCaoBTV.BCTH_HOAT_DONG_KHCN_CBTD: return "BCTH_HOAT_DONG_KHCN_CBTD";
                case DanhSachBaoCaoBTV.BCTH_HOAT_DONG_KHCN_DB: return "BCTH_HOAT_DONG_KHCN_DB";
                case DanhSachBaoCaoBTV.BCTH_HOAT_DONG_KHTC_CBTD: return "BCTH_HOAT_DONG_KHTC_CBTD";
                case DanhSachBaoCaoBTV.BCTH_HOAT_DONG_KHTC_DB: return "BCTH_HOAT_DONG_KHTC_DB";
                case DanhSachBaoCaoBTV.BCTH_HOAT_DONG: return "BCTH_HOAT_DONG";
                case DanhSachBaoCaoBTV.BCTH_THEO_DOI_RUI_RO: return "BCTH_THEO_DOI_RUI_RO";
                case DanhSachBaoCaoBTV.BCTH_CHI_SO_TAI_CHINH: return "BCTH_CHI_SO_TAI_CHINH";
                case DanhSachBaoCaoBTV.BCTC_BANG_CAN_DOI_TKKT: return "BCTC_BANG_CAN_DOI_TKKT";
                case DanhSachBaoCaoBTV.BCTC_BANG_CAN_DOI_KE_TOAN: return "BCTC_BANG_CAN_DOI_KE_TOAN";
                case DanhSachBaoCaoBTV.BCTC_KET_QUA_HOAT_DONG: return "BCTC_KET_QUA_HOAT_DONG";
                case DanhSachBaoCaoBTV.BCTC_LUU_CHUYEN_TIEN_TE: return "BCTC_LUU_CHUYEN_TIEN_TE";
                case DanhSachBaoCaoBTV.BCTC_THUYET_MINH_TAI_CHINH: return "BCTC_THUYET_MINH_TAI_CHINH";
                case DanhSachBaoCaoBTV.BCTC_CHI_TIEU_CT_CHEVRON: return "BCTC_CHI_TIEU_CT_CHEVRON";
                case DanhSachBaoCaoBTV.BCTC_CHI_TIEU_TH_CHEVRON: return "BCTC_CHI_TIEU_TH_CHEVRON";
                case DanhSachBaoCaoBTV.BCTC_LUU_CHUYEN_TIEN_TE_CHEVRON: return "BCTC_LUU_CHUYEN_TIEN_TE_CHEVRON";
                case DanhSachBaoCaoBTV.BCTC_CHI_TIEU_CT_DISNEY: return "BCTC_CHI_TIEU_CT_DISNEY";
                case DanhSachBaoCaoBTV.BCTC_CHI_TIEU_TH_DISNEY: return "BCTC_CHI_TIEU_TH_DISNEY";
                case DanhSachBaoCaoBTV.BCTC_LUU_CHUYEN_TIEN_TE_DISNEY: return "BCTC_LUU_CHUYEN_TIEN_TE_DISNEY";
                default: return "";
            }
        }

        public static DanhSachBaoCaoBTV layBaoCaoBTVTheoMa(string maBaoCao)
        {
            switch (maBaoCao)
            {
                case "KHTV_QUYET_DINH": return DanhSachBaoCaoBTV.KHTV_QUYET_DINH;
                case "KHTV_THAM_DINH": return DanhSachBaoCaoBTV.KHTV_THAM_DINH;
                case "KHTV_HO_SO": return DanhSachBaoCaoBTV.KHTV_HO_SO;
                case "GDKT_PHIEU_THU": return DanhSachBaoCaoBTV.GDKT_PHIEU_THU;
                case "GDKT_PHIEU_CHI": return DanhSachBaoCaoBTV.GDKT_PHIEU_CHI;
                case "GDKT_PHIEU_KE_TOAN": return DanhSachBaoCaoBTV.GDKT_PHIEU_KE_TOAN;
                case "GDKT_UY_NHIEM_CHI": return DanhSachBaoCaoBTV.GDKT_UY_NHIEM_CHI;
                case "GDKT_CHUNG_TU_GHI_SO": return DanhSachBaoCaoBTV.GDKT_CHUNG_TU_GHI_SO;
                case "GDKT_SO_CAI_CTGS": return DanhSachBaoCaoBTV.GDKT_SO_CAI_CTGS;
                case "GDKT_SO_CHI_TIET_TKHOAN": return DanhSachBaoCaoBTV.GDKT_SO_CHI_TIET_TKHOAN;
                case "GDKT_SO_THEO_DOI_CONG_NO": return DanhSachBaoCaoBTV.GDKT_SO_THEO_DOI_CONG_NO;
                case "GDKT_SO_THEO_DOI_TAM_UNG": return DanhSachBaoCaoBTV.GDKT_SO_THEO_DOI_TAM_UNG;
                case "GDKT_SO_TONG_HOP_NGHIEP_VU": return DanhSachBaoCaoBTV.GDKT_SO_TONG_HOP_NGHIEP_VU;
                case "GDKT_SO_QUY_TIEN_MAT": return DanhSachBaoCaoBTV.GDKT_SO_QUY_TIEN_MAT;
                case "GDKT_SO_TIEN_GUI_NGAN_HANG": return DanhSachBaoCaoBTV.GDKT_SO_TIEN_GUI_NGAN_HANG;
                case "GDKT_NX_NGOAI_BANG": return DanhSachBaoCaoBTV.GDKT_NX_NGOAI_BANG;
                case "GDKT_SO_DANG_KY_CTGS": return DanhSachBaoCaoBTV.GDKT_SO_DANG_KY_CTGS;
                case "GDKT_KET_XUAT_BCDKT_THEO_CAU_TRUC": return DanhSachBaoCaoBTV.GDKT_KET_XUAT_BCDKT_THEO_CAU_TRUC;
                case "GDKT_SO_NHAT_KY_CHUNG": return DanhSachBaoCaoBTV.GDKT_SO_NHAT_KY_CHUNG;     
                case "HDVO_DANH_SACH_HOAN_TK": return DanhSachBaoCaoBTV.HDVO_DANH_SACH_HOAN_TK;
                case "HDVO_SO_PHU_TIEN_GUI": return DanhSachBaoCaoBTV.HDVO_SO_PHU_TIEN_GUI;
                case "HDVO_SAO_KE_TKCKH": return DanhSachBaoCaoBTV.HDVO_SAO_KE_TKCKH;
                case "HDVO_SO_THEO_DOI_TK_KHTV": return DanhSachBaoCaoBTV.HDVO_SO_THEO_DOI_TK_KHTV;
                case "HDVO_BANG_TINH_LAI_KHTV": return DanhSachBaoCaoBTV.HDVO_BANG_TINH_LAI_KHTV;
                case "HDVO_SO_TKCKH": return DanhSachBaoCaoBTV.HDVO_SO_TKCKH;
                case "HDVO_GIAY_GUI_TIEN": return DanhSachBaoCaoBTV.HDVO_GIAY_GUI_TIEN;
                case "HDVO_BAO_CAO_THEO_DIA_BAN": return DanhSachBaoCaoBTV.HDVO_BAO_CAO_THEO_DIA_BAN;
                case "HDVO_BAO_CAO_THEO_CAN_BO": return DanhSachBaoCaoBTV.HDVO_BAO_CAO_THEO_CAN_BO;
                case "HDVO_DANH_SACH_KHTV": return DanhSachBaoCaoBTV.HDVO_DANH_SACH_KHTV;
                case "TDVM_DON_VAY_VON": return DanhSachBaoCaoBTV.TDVM_DON_VAY_VON;
                case "TDVM_HOP_DONG_VAY_VON": return DanhSachBaoCaoBTV.TDVM_HOP_DONG_VAY_VON;
                case "TDVM_PHU_LUC_HOP_DONG": return DanhSachBaoCaoBTV.TDVM_PHU_LUC_HOP_DONG;
                case "TDVM_SO_VON_VAY_TIET_KIEM_KHTV": return DanhSachBaoCaoBTV.TDVM_SO_VON_VAY_TIET_KIEM_KHTV;
                case "TDVM_PHIEU_THAM_KHTV": return DanhSachBaoCaoBTV.TDVM_PHIEU_THAM_KHTV;
                case "TDVM_DS_THU_TIEN_PHAT_HANH": return DanhSachBaoCaoBTV.TDVM_DS_THU_TIEN_PHAT_HANH;
                case "TDVM_DS_DE_NGHI_VAY_VON_CBTD": return DanhSachBaoCaoBTV.TDVM_DS_DE_NGHI_VAY_VON_CBTD;
                case "TDVM_DS_DE_NGHI_VAY_VON_TCN": return DanhSachBaoCaoBTV.TDVM_DS_DE_NGHI_VAY_VON_TCN;
                case "TDVM_DS_PHAT_VON": return DanhSachBaoCaoBTV.TDVM_DS_PHAT_VON;
                case "TDVM_DS_THU_LAI_VON_MUA_VU": return DanhSachBaoCaoBTV.TDVM_DS_THU_LAI_VON_MUA_VU;
                case "TDVM_DS_THU_TIEN_VON_TRA_DAN_TK": return DanhSachBaoCaoBTV.TDVM_DS_THU_TIEN_VON_TRA_DAN_TK;
                case "TDVM_DS_THU_VON_MUA_VU": return DanhSachBaoCaoBTV.TDVM_DS_THU_VON_MUA_VU;
                case "TDVM_BANG_PHAN_BO_LAI_TRA_TRUOC": return DanhSachBaoCaoBTV.TDVM_BANG_PHAN_BO_LAI_TRA_TRUOC;
                case "TDVM_SO_THEO_DOI_VON_VAY": return DanhSachBaoCaoBTV.TDVM_SO_THEO_DOI_VON_VAY;
                case "TDVM_SO_THEO_DOI_KHTV": return DanhSachBaoCaoBTV.TDVM_SO_THEO_DOI_KHTV;
                case "TDVM_LICH_THU_NO": return DanhSachBaoCaoBTV.TDVM_LICH_THU_NO;
                case "TDVM_SO_THEO_DOI_NO_RUI_RO": return DanhSachBaoCaoBTV.TDVM_SO_THEO_DOI_NO_RUI_RO;
                case "TDVM_PHAN_LOAI_NO_TLDP": return DanhSachBaoCaoBTV.TDVM_PHAN_LOAI_NO_TLDP;
                case "TDVM_XU_LY_NO": return DanhSachBaoCaoBTV.TDVM_XU_LY_NO;
                case "TDVM_SAO_KE_TH": return DanhSachBaoCaoBTV.TDVM_SAO_KE_TH;
                case "NSTL_BANG_LUONG": return DanhSachBaoCaoBTV.NSTL_BANG_LUONG;
                case "NSTL_THE_LUONG": return DanhSachBaoCaoBTV.NSTL_THE_LUONG;
                case "NSTL_DANH_SACH_CHUYEN_KHOAN": return DanhSachBaoCaoBTV.NSTL_DANH_SACH_CHUYEN_KHOAN;
                case "NSTL_CHI_TIEU_THUONG_CB": return DanhSachBaoCaoBTV.NSTL_CHI_TIEU_THUONG_CB;
                case "NSTL_CHI_TIEU_THUONG_DB": return DanhSachBaoCaoBTV.NSTL_CHI_TIEU_THUONG_DB;
                case "NSTL_THE_THUONG": return DanhSachBaoCaoBTV.NSTL_THE_THUONG;
                case "NSTL_PHU_CAP_CTV": return DanhSachBaoCaoBTV.NSTL_PHU_CAP_CTV;
                case "NSTL_TONG_HOP_CHI_PHI_LUONG": return DanhSachBaoCaoBTV.NSTL_TONG_HOP_CHI_PHI_LUONG;
                case "NSTL_HDLD_CHINH_THUC": return DanhSachBaoCaoBTV.NSTL_HDLD_CHINH_THUC;
                case "NSTL_HDLD_THU_VIEC": return DanhSachBaoCaoBTV.NSTL_HDLD_THU_VIEC;
                case "NSTL_HDLD_HOC_VIEC": return DanhSachBaoCaoBTV.NSTL_HDLD_HOC_VIEC;
                case "NSTL_DANH_SACH_NHAN_VIEN": return DanhSachBaoCaoBTV.NSTL_DANH_SACH_NHAN_VIEN;
                case "QLTS_SO_CHI_TIET_TSCD": return DanhSachBaoCaoBTV.QLTS_SO_CHI_TIET_TSCD;
                case "QLTS_BANG_KIEM_KE_TSCD": return DanhSachBaoCaoBTV.QLTS_BANG_KIEM_KE_TSCD;
                case "QLTS_BANG_KHAU_HAO": return DanhSachBaoCaoBTV.QLTS_BANG_KHAU_HAO;
                case "QLTS_BIEN_BAN_BAN_GIAO_TSCD": return DanhSachBaoCaoBTV.QLTS_BIEN_BAN_BAN_GIAO_TSCD;
                case "BCTH_HOAT_DONG_TCVM": return DanhSachBaoCaoBTV.BCTH_HOAT_DONG_TCVM;
                case "BCTH_HOAT_DONG_CBTD_SP": return DanhSachBaoCaoBTV.BCTH_HOAT_DONG_CBTD_SP;
                case "BCTH_HOAT_DONG_DB": return DanhSachBaoCaoBTV.BCTH_HOAT_DONG_DB;
                case "BCTH_HOAT_DONG_CBTD": return DanhSachBaoCaoBTV.BCTH_HOAT_DONG_CBTD;
                case "BCTH_CONG_VIEC_CBTD": return DanhSachBaoCaoBTV.BCTH_CONG_VIEC_CBTD;
                case "BCTH_DU_TRU_NGAN_QUY": return DanhSachBaoCaoBTV.BCTH_DU_TRU_NGAN_QUY;
                case "BCTH_HOAT_DONG_KHCN_CBTD": return DanhSachBaoCaoBTV.BCTH_HOAT_DONG_KHCN_CBTD;
                case "BCTH_HOAT_DONG_KHCN_DB": return DanhSachBaoCaoBTV.BCTH_HOAT_DONG_KHCN_DB;
                case "BCTH_HOAT_DONG_KHTC_CBTD": return DanhSachBaoCaoBTV.BCTH_HOAT_DONG_KHTC_CBTD;
                case "BCTH_HOAT_DONG_KHTC_DB": return DanhSachBaoCaoBTV.BCTH_HOAT_DONG_KHTC_DB;
                case "BCTH_HOAT_DONG": return DanhSachBaoCaoBTV.BCTH_HOAT_DONG;
                case "BCTH_THEO_DOI_RUI_RO": return DanhSachBaoCaoBTV.BCTH_THEO_DOI_RUI_RO;
                case "BCTH_CHI_SO_TAI_CHINH": return DanhSachBaoCaoBTV.BCTH_CHI_SO_TAI_CHINH;
                case "BCTC_BANG_CAN_DOI_TKKT": return DanhSachBaoCaoBTV.BCTC_BANG_CAN_DOI_TKKT;
                case "BCTC_BANG_CAN_DOI_KE_TOAN": return DanhSachBaoCaoBTV.BCTC_BANG_CAN_DOI_KE_TOAN;
                case "BCTC_KET_QUA_HOAT_DONG": return DanhSachBaoCaoBTV.BCTC_KET_QUA_HOAT_DONG;
                case "BCTC_LUU_CHUYEN_TIEN_TE": return DanhSachBaoCaoBTV.BCTC_LUU_CHUYEN_TIEN_TE;
                case "BCTC_THUYET_MINH_TAI_CHINH": return DanhSachBaoCaoBTV.BCTC_THUYET_MINH_TAI_CHINH;
                case "BCTC_CHI_TIEU_CT_CHEVRON": return DanhSachBaoCaoBTV.BCTC_CHI_TIEU_CT_CHEVRON;
                case "BCTC_CHI_TIEU_TH_CHEVRON": return DanhSachBaoCaoBTV.BCTC_CHI_TIEU_TH_CHEVRON;
                case "BCTC_LUU_CHUYEN_TIEN_TE_CHEVRON": return DanhSachBaoCaoBTV.BCTC_LUU_CHUYEN_TIEN_TE_CHEVRON;
                case "BCTC_CHI_TIEU_CT_DISNEY": return DanhSachBaoCaoBTV.BCTC_CHI_TIEU_CT_DISNEY;
                case "BCTC_CHI_TIEU_TH_DISNEY": return DanhSachBaoCaoBTV.BCTC_CHI_TIEU_TH_DISNEY;
                case "BCTC_LUU_CHUYEN_TIEN_TE_DISNEY": return DanhSachBaoCaoBTV.BCTC_LUU_CHUYEN_TIEN_TE_DISNEY;
                default: return DanhSachBaoCaoBTV.KHTV_QUYET_DINH;
            }
        }


        public enum DanhSachBaoCaoBenTre
        {
            KHTV_QUYET_DINH_CONG_NHAN_TVIEN,
            KHTV_BAN_KHAO_SAT_HOAN_CANH_KTXH,
            KHTV_BAN_KHAO_SAT_KH,
            KHTV_PHIEU_DANH_GIA_RUI_RO_TD,
            KHTV_PHIEU_THAM_HO_GD,
            GDKT_PHIEU_TAM_UNG,
            GDKT_GIAY_THANH_TOAN_TAM_UNG,
            GDKT_SO_THEO_DOI_TAM_UNG,
            GDKT_SO_TIEN_GUI_KHO_BAC,
            HDVO_DANH_SACH_HOAN_TK,
            TDVM_DON_XIN_VAY_VON_NHOM_BAO_LANH,
            TDVM_DON_XIN_VAY_VON_NUOC_SACH,
            TDVM_HOP_DONG_VAY_VON_NHOM_BAO_LANH,
            TDVM_HOP_DONG_VAY_VON_NUOC_SACH,
            TDVM_PHIEU_KIEM_TRA_SU_DUNG_VON_VAY,
            TDVM_SO_VON_VAY_TIET_KIEM_TVIEN,
            TDVM_DS_DE_NGHI_VAY_VON,
            TDVM_DS_PHAT_VON,
            TDVM_DS_THU_LAI_VON_MUA_VU,
            TDVM_DS_THU_VON_MUA_VU,
            TDVM_BC_SO_LIEU_TDUNG_XA,
            TDVM_LICH_THU_NO,
            TDVM_TO_KE_THU,
            BCTH_HOAT_DONG,
            BCTC_BANG_CAN_DOI_TKKT_THU,
            BCTC_BANG_CAN_DOI_KE_TOAN,
            BCTC_BANG_CAN_DOI_CHUNG_TU,
            BCTC_KET_QUA_HOAT_DONG,
            BCTC_LUU_CHUYEN_TIEN_TE,
            BCTC_THUYET_MINH_TAI_CHINH,
            BCTH_HOAT_DONG_TCVM,
            BCTH_HOAT_DONG_CBTD_SP,
            BCTH_HOAT_DONG_DB,
            BCTH_HOAT_DONG_CBTD,
            BCTC_BANG_CAN_DOI_TAI_KHOAN_THEO_NGUON,
            BCTC_BAO_CAO_DOANH_THU_THEO_NGUON,
            BCTC_BANG_TONG_KET_TAI_SAN_THEO_NGUON,
            BCTC_BAO_CAO_LUU_CHUYEN_TIEN_TE_THEO_NGUON,
            BCTC_BANG_CAN_DOI_TAI_KHOAN_TOAN_QUY,
            BCTC_BAO_CAO_DOANH_THU_TOAN_QUY,
            BCTC_BANG_TONG_KET_TAI_SAN_TOAN_QUY,
            BCTC_BAO_CAO_LUU_CHUYEN_TIEN_TE_TOAN_QUY
        }

        public static string LayMaBaoCaoBenTre(DanhSachBaoCaoBenTre dsbaocao)
        {
            switch (dsbaocao)
            {
                case DanhSachBaoCaoBenTre.KHTV_QUYET_DINH_CONG_NHAN_TVIEN: return "KHTV_QUYET_DINH_CONG_NHAN_TVIEN";
                case DanhSachBaoCaoBenTre.KHTV_BAN_KHAO_SAT_HOAN_CANH_KTXH: return "KHTV_BAN_KHAO_SAT_HOAN_CANH_KTXH";
                case DanhSachBaoCaoBenTre.KHTV_BAN_KHAO_SAT_KH: return "KHTV_BAN_KHAO_SAT_KH";
                case DanhSachBaoCaoBenTre.KHTV_PHIEU_DANH_GIA_RUI_RO_TD: return "KHTV_PHIEU_DANH_GIA_RUI_RO_TD";
                case DanhSachBaoCaoBenTre.KHTV_PHIEU_THAM_HO_GD: return "KHTV_PHIEU_THAM_HO_GD";
                case DanhSachBaoCaoBenTre.GDKT_PHIEU_TAM_UNG: return "GDKT_PHIEU_TAM_UNG";
                case DanhSachBaoCaoBenTre.GDKT_GIAY_THANH_TOAN_TAM_UNG: return "GDKT_GIAY_THANH_TOAN_TAM_UNG";
                case DanhSachBaoCaoBenTre.GDKT_SO_THEO_DOI_TAM_UNG: return "GDKT_SO_THEO_DOI_TAM_UNG";
                case DanhSachBaoCaoBenTre.GDKT_SO_TIEN_GUI_KHO_BAC: return "GDKT_SO_TIEN_GUI_KHO_BAC";
                case DanhSachBaoCaoBenTre.HDVO_DANH_SACH_HOAN_TK: return "HDVO_DANH_SACH_HOAN_TK";
                case DanhSachBaoCaoBenTre.TDVM_DON_XIN_VAY_VON_NHOM_BAO_LANH: return "TDVM_DON_XIN_VAY_VON_NHOM_BAO_LANH";
                case DanhSachBaoCaoBenTre.TDVM_DON_XIN_VAY_VON_NUOC_SACH: return "TDVM_DON_XIN_VAY_VON_NUOC_SACH";
                case DanhSachBaoCaoBenTre.TDVM_HOP_DONG_VAY_VON_NHOM_BAO_LANH: return "TDVM_HOP_DONG_VAY_VON_NHOM_BAO_LANH";
                case DanhSachBaoCaoBenTre.TDVM_HOP_DONG_VAY_VON_NUOC_SACH: return "TDVM_HOP_DONG_VAY_VON_NUOC_SACH";
                case DanhSachBaoCaoBenTre.TDVM_PHIEU_KIEM_TRA_SU_DUNG_VON_VAY: return "TDVM_PHIEU_KIEM_TRA_SU_DUNG_VON_VAY";
                case DanhSachBaoCaoBenTre.TDVM_SO_VON_VAY_TIET_KIEM_TVIEN: return "TDVM_SO_VON_VAY_TIET_KIEM_TVIEN";
                case DanhSachBaoCaoBenTre.TDVM_DS_DE_NGHI_VAY_VON: return "TDVM_DS_DE_NGHI_VAY_VON";
                case DanhSachBaoCaoBenTre.TDVM_DS_PHAT_VON: return "TDVM_DS_PHAT_VON";
                case DanhSachBaoCaoBenTre.TDVM_DS_THU_LAI_VON_MUA_VU: return "TDVM_DS_THU_LAI_VON_MUA_VU";
                case DanhSachBaoCaoBenTre.TDVM_DS_THU_VON_MUA_VU: return "TDVM_DS_THU_VON_MUA_VU";
                case DanhSachBaoCaoBenTre.TDVM_BC_SO_LIEU_TDUNG_XA: return "TDVM_BC_SO_LIEU_TDUNG_XA";
                case DanhSachBaoCaoBenTre.TDVM_LICH_THU_NO: return "TDVM_LICH_THU_NO";
                case DanhSachBaoCaoBenTre.TDVM_TO_KE_THU: return "TDVM_TO_KE_THU";                    
                case DanhSachBaoCaoBenTre.BCTH_HOAT_DONG: return "BCTH_HOAT_DONG";
                case DanhSachBaoCaoBenTre.BCTC_BANG_CAN_DOI_TKKT_THU: return "BCTC_BANG_CAN_DOI_TKKT_THU";
                case DanhSachBaoCaoBenTre.BCTC_BANG_CAN_DOI_KE_TOAN: return "BCTC_BANG_CAN_DOI_KE_TOAN";
                case DanhSachBaoCaoBenTre.BCTC_BANG_CAN_DOI_CHUNG_TU: return "BCTC_BANG_CAN_DOI_CHUNG_TU";
                case DanhSachBaoCaoBenTre.BCTC_KET_QUA_HOAT_DONG: return "BCTC_KET_QUA_HOAT_DONG";
                case DanhSachBaoCaoBenTre.BCTC_LUU_CHUYEN_TIEN_TE: return "BCTC_LUU_CHUYEN_TIEN_TE";
                case DanhSachBaoCaoBenTre.BCTC_THUYET_MINH_TAI_CHINH: return "BCTC_THUYET_MINH_TAI_CHINH";
                case DanhSachBaoCaoBenTre.BCTH_HOAT_DONG_TCVM: return "BCTH_HOAT_DONG_TCVM";
                case DanhSachBaoCaoBenTre.BCTH_HOAT_DONG_CBTD_SP: return "BCTH_HOAT_DONG_CBTD_SP";
                case DanhSachBaoCaoBenTre.BCTH_HOAT_DONG_DB: return "BCTH_HOAT_DONG_DB";
                case DanhSachBaoCaoBenTre.BCTH_HOAT_DONG_CBTD: return "BCTH_HOAT_DONG_CBTD";
                case DanhSachBaoCaoBenTre.BCTC_BANG_CAN_DOI_TAI_KHOAN_THEO_NGUON: return "BCTC_BANG_CAN_DOI_TAI_KHOAN_THEO_NGUON";
                case DanhSachBaoCaoBenTre.BCTC_BAO_CAO_DOANH_THU_THEO_NGUON: return "BCTC_BAO_CAO_DOANH_THU_THEO_NGUON";
                case DanhSachBaoCaoBenTre.BCTC_BANG_TONG_KET_TAI_SAN_THEO_NGUON: return "BCTC_BANG_TONG_KET_TAI_SAN_THEO_NGUON";
                case DanhSachBaoCaoBenTre.BCTC_BAO_CAO_LUU_CHUYEN_TIEN_TE_THEO_NGUON: return "BCTC_BAO_CAO_LUU_CHUYEN_TIEN_TE_THEO_NGUON";
                case DanhSachBaoCaoBenTre.BCTC_BANG_CAN_DOI_TAI_KHOAN_TOAN_QUY: return "BCTC_BANG_CAN_DOI_TAI_KHOAN_TOAN_QUY";
                case DanhSachBaoCaoBenTre.BCTC_BAO_CAO_DOANH_THU_TOAN_QUY: return "BCTC_BAO_CAO_DOANH_THU_TOAN_QUY";
                case DanhSachBaoCaoBenTre.BCTC_BANG_TONG_KET_TAI_SAN_TOAN_QUY: return "BCTC_BANG_TONG_KET_TAI_SAN_TOAN_QUY";
                case DanhSachBaoCaoBenTre.BCTC_BAO_CAO_LUU_CHUYEN_TIEN_TE_TOAN_QUY: return "BCTC_BAO_CAO_LUU_CHUYEN_TIEN_TE_TOAN_QUY";
                default: return "";
            }
        }

        public static DanhSachBaoCaoBenTre layBaoCaoBenTreTheoMa(string maBaoCao)
        {
            switch (maBaoCao)
            {
                case "KHTV_QUYET_DINH_CONG_NHAN_TVIEN": return DanhSachBaoCaoBenTre.KHTV_QUYET_DINH_CONG_NHAN_TVIEN;
                case "KHTV_BAN_KHAO_SAT_HOAN_CANH_KTXH": return DanhSachBaoCaoBenTre.KHTV_BAN_KHAO_SAT_HOAN_CANH_KTXH;
                case "KHTV_BAN_KHAO_SAT_KH": return DanhSachBaoCaoBenTre.KHTV_BAN_KHAO_SAT_KH;
                case "KHTV_PHIEU_DANH_GIA_RUI_RO_TD": return DanhSachBaoCaoBenTre.KHTV_PHIEU_DANH_GIA_RUI_RO_TD;
                case "KHTV_PHIEU_THAM_HO_GD": return DanhSachBaoCaoBenTre.KHTV_PHIEU_THAM_HO_GD;
                case "GDKT_PHIEU_TAM_UNG": return DanhSachBaoCaoBenTre.GDKT_PHIEU_TAM_UNG;
                case "GDKT_GIAY_THANH_TOAN_TAM_UNG": return DanhSachBaoCaoBenTre.GDKT_GIAY_THANH_TOAN_TAM_UNG;
                case "GDKT_SO_THEO_DOI_TAM_UNG": return DanhSachBaoCaoBenTre.GDKT_SO_THEO_DOI_TAM_UNG;
                case "GDKT_SO_TIEN_GUI_KHO_BAC": return DanhSachBaoCaoBenTre.GDKT_SO_TIEN_GUI_KHO_BAC;
                case "HDVO_DANH_SACH_HOAN_TK": return DanhSachBaoCaoBenTre.HDVO_DANH_SACH_HOAN_TK;
                case "TDVM_DON_XIN_VAY_VON_NHOM_BAO_LANH": return DanhSachBaoCaoBenTre.TDVM_DON_XIN_VAY_VON_NHOM_BAO_LANH;
                case "TDVM_DON_XIN_VAY_VON_NUOC_SACH": return DanhSachBaoCaoBenTre.TDVM_DON_XIN_VAY_VON_NUOC_SACH;
                case "TDVM_HOP_DONG_VAY_VON_NHOM_BAO_LANH": return DanhSachBaoCaoBenTre.TDVM_HOP_DONG_VAY_VON_NHOM_BAO_LANH;
                case "TDVM_HOP_DONG_VAY_VON_NUOC_SACH": return DanhSachBaoCaoBenTre.TDVM_HOP_DONG_VAY_VON_NUOC_SACH;
                case "TDVM_PHIEU_KIEM_TRA_SU_DUNG_VON_VAY": return DanhSachBaoCaoBenTre.TDVM_PHIEU_KIEM_TRA_SU_DUNG_VON_VAY;
                case "TDVM_SO_VON_VAY_TIET_KIEM_TVIEN": return DanhSachBaoCaoBenTre.TDVM_SO_VON_VAY_TIET_KIEM_TVIEN;
                case "TDVM_DS_DE_NGHI_VAY_VON": return DanhSachBaoCaoBenTre.TDVM_DS_DE_NGHI_VAY_VON;
                case "TDVM_DS_PHAT_VON": return DanhSachBaoCaoBenTre.TDVM_DS_PHAT_VON;
                case "TDVM_DS_THU_LAI_VON_MUA_VU": return DanhSachBaoCaoBenTre.TDVM_DS_THU_LAI_VON_MUA_VU;
                case "TDVM_DS_THU_VON_MUA_VU": return DanhSachBaoCaoBenTre.TDVM_DS_THU_VON_MUA_VU;
                case "TDVM_BC_SO_LIEU_TDUNG_XA": return DanhSachBaoCaoBenTre.TDVM_BC_SO_LIEU_TDUNG_XA;
                case "TDVM_LICH_THU_NO": return DanhSachBaoCaoBenTre.TDVM_LICH_THU_NO;
                case "TDVM_TO_KE_THU": return DanhSachBaoCaoBenTre.TDVM_TO_KE_THU;                    
                case "BCTH_HOAT_DONG": return DanhSachBaoCaoBenTre.BCTH_HOAT_DONG;
                case "BCTC_BANG_CAN_DOI_TKKT_THU": return DanhSachBaoCaoBenTre.BCTC_BANG_CAN_DOI_TKKT_THU;
                case "BCTC_BANG_CAN_DOI_KE_TOAN": return DanhSachBaoCaoBenTre.BCTC_BANG_CAN_DOI_KE_TOAN;
                case "BCTC_BANG_CAN_DOI_CHUNG_TU": return DanhSachBaoCaoBenTre.BCTC_BANG_CAN_DOI_CHUNG_TU;
                case "BCTC_KET_QUA_HOAT_DONG": return DanhSachBaoCaoBenTre.BCTC_KET_QUA_HOAT_DONG;
                case "BCTC_LUU_CHUYEN_TIEN_TE": return DanhSachBaoCaoBenTre.BCTC_LUU_CHUYEN_TIEN_TE;
                case "BCTC_THUYET_MINH_TAI_CHINH": return DanhSachBaoCaoBenTre.BCTC_THUYET_MINH_TAI_CHINH;
                case "BCTC_BANG_CAN_DOI_TAI_KHOAN_THEO_NGUON": return DanhSachBaoCaoBenTre.BCTC_BANG_CAN_DOI_TAI_KHOAN_THEO_NGUON;
                case "BCTC_BAO_CAO_DOANH_THU_THEO_NGUON": return DanhSachBaoCaoBenTre.BCTC_BAO_CAO_DOANH_THU_THEO_NGUON;
                case "BCTC_BANG_TONG_KET_TAI_SAN_THEO_NGUON": return DanhSachBaoCaoBenTre.BCTC_BANG_TONG_KET_TAI_SAN_THEO_NGUON;
                case "BCTC_BAO_CAO_LUU_CHUYEN_TIEN_TE_THEO_NGUON": return DanhSachBaoCaoBenTre.BCTC_BAO_CAO_LUU_CHUYEN_TIEN_TE_THEO_NGUON;
                case "BCTC_BANG_CAN_DOI_TAI_KHOAN_TOAN_QUY": return DanhSachBaoCaoBenTre.BCTC_BANG_CAN_DOI_TAI_KHOAN_TOAN_QUY;
                case "BCTC_BAO_CAO_DOANH_THU_TOAN_QUY": return DanhSachBaoCaoBenTre.BCTC_BAO_CAO_DOANH_THU_TOAN_QUY;
                case "BCTC_BANG_TONG_KET_TAI_SAN_TOAN_QUY": return DanhSachBaoCaoBenTre.BCTC_BANG_TONG_KET_TAI_SAN_TOAN_QUY;
                case "BCTC_BAO_CAO_LUU_CHUYEN_TIEN_TE_TOAN_QUY": return DanhSachBaoCaoBenTre.BCTC_BAO_CAO_LUU_CHUYEN_TIEN_TE_TOAN_QUY;
                default: return DanhSachBaoCaoBenTre.KHTV_QUYET_DINH_CONG_NHAN_TVIEN;
            }
        }

        public enum DanhSachBaoCaoPhuTho
        {
            KHTV_QUYET_DINH_CONG_NHAN_TVIEN,
            KHTV_BAN_KHAO_SAT_HOAN_CANH_KTXH,
            KHTV_BAN_KHAO_SAT_KH,
            KHTV_PHIEU_DANH_GIA_RUI_RO_TD,
            KHTV_PHIEU_THAM_HO_GD,
            GDKT_PHIEU_TAM_UNG,
            GDKT_GIAY_THANH_TOAN_TAM_UNG,
            GDKT_SO_THEO_DOI_TAM_UNG,
            GDKT_SO_TIEN_GUI_KHO_BAC,
            GDKT_UY_NHIEM_CHI,
            HDVO_DANH_SACH_HOAN_TK,
            TDVM_DON_VAY_VON,
            TDVM_DON_XIN_VAY_VON_NUOC_SACH,
            TDVM_HOP_DONG_VAY_VON,
            TDVM_HOP_DONG_VAY_VON_NUOC_SACH,
            TDVM_PHU_LUC_HOP_DONG,
            TDVM_PHIEU_KIEM_TRA_SU_DUNG_VON_VAY,
            TDVM_SO_VON_VAY_TIET_KIEM_TVIEN,
            TDVM_DS_DE_NGHI_VAY_VON,
            TDVM_DS_PHAT_VON,
            TDVM_DS_THU_LAI_VON_MUA_VU,
            TDVM_DS_THU_VON_MUA_VU,
            TDVM_BC_SO_LIEU_TDUNG_XA,
            TDVM_LICH_THU_NO,
            TDVM_TO_KE_THU,
            TDVM_DANH_SACH_THANH_VIEN_NHAN_TIEN,
            TDVM_DANH_SACH_THANH_TOAN_TIET_KIEM,
            TDVM_DS_THU_TIEN_VON_TRA_DAN_TK,
            TDVM_DS_XET_DUYET_THANH_VIEN_VAY_VON,
            TDVM_BAO_CAO_NO_QUA_HAN,
            TDVM_LICH_TRA_NO,
            BCTH_HOAT_DONG,
            BCTC_BANG_CAN_DOI_TKKT_THU,
            BCTC_BANG_CAN_DOI_KE_TOAN,
            BCTC_BANG_CAN_DOI_CHUNG_TU,
            BCTC_KET_QUA_HOAT_DONG,
            BCTC_LUU_CHUYEN_TIEN_TE,
            BCTC_THUYET_MINH_TAI_CHINH,
            BCTH_HOAT_DONG_TCVM,
            BCTH_HOAT_DONG_CBTD_SP,
            BCTH_HOAT_DONG_DB,
            BCTH_HOAT_DONG_CBTD,
            BCTH_BAO_CAO_SO_THANH_VIEN,
            BCTH_SO_TONG_HOP_TIET_KIEM,
            BCTH_SO_TONG_HOP_TUONG_TRO,
            BCTH_SO_TONG_HOP_VAY_VON,
            BCTH_BAO_CAO_LAI_VON_VAY
        }

        public static string LayMaBaoCaoPhuTho(DanhSachBaoCaoPhuTho dsbaocao)
        {
            switch (dsbaocao)
            {
                case DanhSachBaoCaoPhuTho.KHTV_QUYET_DINH_CONG_NHAN_TVIEN: return "KHTV_QUYET_DINH_CONG_NHAN_TVIEN";
                case DanhSachBaoCaoPhuTho.KHTV_BAN_KHAO_SAT_HOAN_CANH_KTXH: return "KHTV_BAN_KHAO_SAT_HOAN_CANH_KTXH";
                case DanhSachBaoCaoPhuTho.KHTV_BAN_KHAO_SAT_KH: return "KHTV_BAN_KHAO_SAT_KH";
                case DanhSachBaoCaoPhuTho.KHTV_PHIEU_DANH_GIA_RUI_RO_TD: return "KHTV_PHIEU_DANH_GIA_RUI_RO_TD";
                case DanhSachBaoCaoPhuTho.KHTV_PHIEU_THAM_HO_GD: return "KHTV_PHIEU_THAM_HO_GD";
                case DanhSachBaoCaoPhuTho.GDKT_PHIEU_TAM_UNG: return "GDKT_PHIEU_TAM_UNG";
                case DanhSachBaoCaoPhuTho.GDKT_GIAY_THANH_TOAN_TAM_UNG: return "GDKT_GIAY_THANH_TOAN_TAM_UNG";
                case DanhSachBaoCaoPhuTho.GDKT_SO_THEO_DOI_TAM_UNG: return "GDKT_SO_THEO_DOI_TAM_UNG";
                case DanhSachBaoCaoPhuTho.GDKT_SO_TIEN_GUI_KHO_BAC: return "GDKT_SO_TIEN_GUI_KHO_BAC";
                case DanhSachBaoCaoPhuTho.GDKT_UY_NHIEM_CHI: return "GDKT_UY_NHIEM_CHI";
                case DanhSachBaoCaoPhuTho.HDVO_DANH_SACH_HOAN_TK: return "HDVO_DANH_SACH_HOAN_TK";
                case DanhSachBaoCaoPhuTho.TDVM_DON_VAY_VON: return "TDVM_DON_VAY_VON";
                case DanhSachBaoCaoPhuTho.TDVM_DON_XIN_VAY_VON_NUOC_SACH: return "TDVM_DON_XIN_VAY_VON_NUOC_SACH";
                case DanhSachBaoCaoPhuTho.TDVM_HOP_DONG_VAY_VON: return "TDVM_HOP_DONG_VAY_VON";
                case DanhSachBaoCaoPhuTho.TDVM_PHU_LUC_HOP_DONG: return "TDVM_PHU_LUC_HOP_DONG";
                case DanhSachBaoCaoPhuTho.TDVM_HOP_DONG_VAY_VON_NUOC_SACH: return "TDVM_HOP_DONG_VAY_VON_NUOC_SACH";
                case DanhSachBaoCaoPhuTho.TDVM_PHIEU_KIEM_TRA_SU_DUNG_VON_VAY: return "TDVM_PHIEU_KIEM_TRA_SU_DUNG_VON_VAY";
                case DanhSachBaoCaoPhuTho.TDVM_SO_VON_VAY_TIET_KIEM_TVIEN: return "TDVM_SO_VON_VAY_TIET_KIEM_TVIEN";
                case DanhSachBaoCaoPhuTho.TDVM_DS_DE_NGHI_VAY_VON: return "TDVM_DS_DE_NGHI_VAY_VON";
                case DanhSachBaoCaoPhuTho.TDVM_DS_PHAT_VON: return "TDVM_DS_PHAT_VON";
                case DanhSachBaoCaoPhuTho.TDVM_DS_THU_LAI_VON_MUA_VU: return "TDVM_DS_THU_LAI_VON_MUA_VU";
                case DanhSachBaoCaoPhuTho.TDVM_DS_THU_VON_MUA_VU: return "TDVM_DS_THU_VON_MUA_VU";
                case DanhSachBaoCaoPhuTho.TDVM_BC_SO_LIEU_TDUNG_XA: return "TDVM_BC_SO_LIEU_TDUNG_XA";
                case DanhSachBaoCaoPhuTho.TDVM_LICH_THU_NO: return "TDVM_LICH_THU_NO";
                case DanhSachBaoCaoPhuTho.TDVM_TO_KE_THU: return "TDVM_TO_KE_THU";
                case DanhSachBaoCaoPhuTho.TDVM_DANH_SACH_THANH_VIEN_NHAN_TIEN: return "TDVM_DANH_SACH_THANH_VIEN_NHAN_TIEN";
                case DanhSachBaoCaoPhuTho.TDVM_DANH_SACH_THANH_TOAN_TIET_KIEM: return "TDVM_DANH_SACH_THANH_TOAN_TIET_KIEM";
                case DanhSachBaoCaoPhuTho.TDVM_DS_THU_TIEN_VON_TRA_DAN_TK: return "TDVM_DS_THU_TIEN_VON_TRA_DAN_TK";
                case DanhSachBaoCaoPhuTho.TDVM_DS_XET_DUYET_THANH_VIEN_VAY_VON: return "TDVM_DS_XET_DUYET_THANH_VIEN_VAY_VON";
                case DanhSachBaoCaoPhuTho.TDVM_BAO_CAO_NO_QUA_HAN: return "TDVM_BAO_CAO_NO_QUA_HAN";
                case DanhSachBaoCaoPhuTho.TDVM_LICH_TRA_NO: return "TDVM_LICH_TRA_NO";
                case DanhSachBaoCaoPhuTho.BCTH_HOAT_DONG: return "BCTH_HOAT_DONG";
                case DanhSachBaoCaoPhuTho.BCTC_BANG_CAN_DOI_TKKT_THU: return "BCTC_BANG_CAN_DOI_TKKT_THU";
                case DanhSachBaoCaoPhuTho.BCTC_BANG_CAN_DOI_KE_TOAN: return "BCTC_BANG_CAN_DOI_KE_TOAN";
                case DanhSachBaoCaoPhuTho.BCTC_BANG_CAN_DOI_CHUNG_TU: return "BCTC_BANG_CAN_DOI_CHUNG_TU";
                case DanhSachBaoCaoPhuTho.BCTC_KET_QUA_HOAT_DONG: return "BCTC_KET_QUA_HOAT_DONG";
                case DanhSachBaoCaoPhuTho.BCTC_LUU_CHUYEN_TIEN_TE: return "BCTC_LUU_CHUYEN_TIEN_TE";
                case DanhSachBaoCaoPhuTho.BCTC_THUYET_MINH_TAI_CHINH: return "BCTC_THUYET_MINH_TAI_CHINH";
                case DanhSachBaoCaoPhuTho.BCTH_HOAT_DONG_TCVM: return "BCTH_HOAT_DONG_TCVM";
                case DanhSachBaoCaoPhuTho.BCTH_HOAT_DONG_CBTD_SP: return "BCTH_HOAT_DONG_CBTD_SP";
                case DanhSachBaoCaoPhuTho.BCTH_HOAT_DONG_DB: return "BCTH_HOAT_DONG_DB";
                case DanhSachBaoCaoPhuTho.BCTH_HOAT_DONG_CBTD: return "BCTH_HOAT_DONG_CBTD";
                case DanhSachBaoCaoPhuTho.BCTH_BAO_CAO_SO_THANH_VIEN: return "BCTH_BAO_CAO_SO_THANH_VIEN";
                case DanhSachBaoCaoPhuTho.BCTH_SO_TONG_HOP_TIET_KIEM: return "BCTH_SO_TONG_HOP_TIET_KIEM";
                case DanhSachBaoCaoPhuTho.BCTH_SO_TONG_HOP_TUONG_TRO: return "BCTH_SO_TONG_HOP_TUONG_TRO";
                case DanhSachBaoCaoPhuTho.BCTH_SO_TONG_HOP_VAY_VON: return "BCTH_SO_TONG_HOP_VAY_VON";
                case DanhSachBaoCaoPhuTho.BCTH_BAO_CAO_LAI_VON_VAY: return "BCTH_BAO_CAO_LAI_VON_VAY";

                default: return "";
            }
        }

        public static DanhSachBaoCaoPhuTho layBaoCaoPhuThoTheoMa(string maBaoCao)
        {
            switch (maBaoCao)
            {
                case "KHTV_QUYET_DINH_CONG_NHAN_TVIEN": return DanhSachBaoCaoPhuTho.KHTV_QUYET_DINH_CONG_NHAN_TVIEN;
                case "KHTV_BAN_KHAO_SAT_HOAN_CANH_KTXH": return DanhSachBaoCaoPhuTho.KHTV_BAN_KHAO_SAT_HOAN_CANH_KTXH;
                case "KHTV_BAN_KHAO_SAT_KH": return DanhSachBaoCaoPhuTho.KHTV_BAN_KHAO_SAT_KH;
                case "KHTV_PHIEU_DANH_GIA_RUI_RO_TD": return DanhSachBaoCaoPhuTho.KHTV_PHIEU_DANH_GIA_RUI_RO_TD;
                case "KHTV_PHIEU_THAM_HO_GD": return DanhSachBaoCaoPhuTho.KHTV_PHIEU_THAM_HO_GD;
                case "GDKT_PHIEU_TAM_UNG": return DanhSachBaoCaoPhuTho.GDKT_PHIEU_TAM_UNG;
                case "GDKT_GIAY_THANH_TOAN_TAM_UNG": return DanhSachBaoCaoPhuTho.GDKT_GIAY_THANH_TOAN_TAM_UNG;
                case "GDKT_SO_THEO_DOI_TAM_UNG": return DanhSachBaoCaoPhuTho.GDKT_SO_THEO_DOI_TAM_UNG;
                case "GDKT_SO_TIEN_GUI_KHO_BAC": return DanhSachBaoCaoPhuTho.GDKT_SO_TIEN_GUI_KHO_BAC;
                case "GDKT_UY_NHIEM_CHI": return DanhSachBaoCaoPhuTho.GDKT_UY_NHIEM_CHI;
                case "HDVO_DANH_SACH_HOAN_TK": return DanhSachBaoCaoPhuTho.HDVO_DANH_SACH_HOAN_TK;
                case "TDVM_DON_VAY_VON": return DanhSachBaoCaoPhuTho.TDVM_DON_VAY_VON;
                case "TDVM_DON_XIN_VAY_VON_NUOC_SACH": return DanhSachBaoCaoPhuTho.TDVM_DON_XIN_VAY_VON_NUOC_SACH;
                case "TDVM_HOP_DONG_VAY_VON": return DanhSachBaoCaoPhuTho.TDVM_HOP_DONG_VAY_VON;
                case "TDVM_PHU_LUC_HOP_DONG": return DanhSachBaoCaoPhuTho.TDVM_PHU_LUC_HOP_DONG;
                case "TDVM_HOP_DONG_VAY_VON_NUOC_SACH": return DanhSachBaoCaoPhuTho.TDVM_HOP_DONG_VAY_VON_NUOC_SACH;
                case "TDVM_PHIEU_KIEM_TRA_SU_DUNG_VON_VAY": return DanhSachBaoCaoPhuTho.TDVM_PHIEU_KIEM_TRA_SU_DUNG_VON_VAY;
                case "TDVM_SO_VON_VAY_TIET_KIEM_TVIEN": return DanhSachBaoCaoPhuTho.TDVM_SO_VON_VAY_TIET_KIEM_TVIEN;
                case "TDVM_DS_DE_NGHI_VAY_VON": return DanhSachBaoCaoPhuTho.TDVM_DS_DE_NGHI_VAY_VON;
                case "TDVM_DS_PHAT_VON": return DanhSachBaoCaoPhuTho.TDVM_DS_PHAT_VON;
                case "TDVM_DS_THU_LAI_VON_MUA_VU": return DanhSachBaoCaoPhuTho.TDVM_DS_THU_LAI_VON_MUA_VU;
                case "TDVM_DS_THU_VON_MUA_VU": return DanhSachBaoCaoPhuTho.TDVM_DS_THU_VON_MUA_VU;
                case "TDVM_BC_SO_LIEU_TDUNG_XA": return DanhSachBaoCaoPhuTho.TDVM_BC_SO_LIEU_TDUNG_XA;
                case "TDVM_LICH_THU_NO": return DanhSachBaoCaoPhuTho.TDVM_LICH_THU_NO;
                case "TDVM_TO_KE_THU": return DanhSachBaoCaoPhuTho.TDVM_TO_KE_THU;
                case "TDVM_DANH_SACH_THANH_VIEN_NHAN_TIEN": return DanhSachBaoCaoPhuTho.TDVM_DANH_SACH_THANH_VIEN_NHAN_TIEN;
                case "TDVM_DANH_SACH_THANH_TOAN_TIET_KIEM": return DanhSachBaoCaoPhuTho.TDVM_DANH_SACH_THANH_TOAN_TIET_KIEM;
                case "TDVM_DS_THU_TIEN_VON_TRA_DAN_TK": return DanhSachBaoCaoPhuTho.TDVM_DS_THU_TIEN_VON_TRA_DAN_TK;
                case "TDVM_DS_XET_DUYET_THANH_VIEN_VAY_VON": return DanhSachBaoCaoPhuTho.TDVM_DS_XET_DUYET_THANH_VIEN_VAY_VON;
                case "TDVM_BAO_CAO_NO_QUA_HAN": return DanhSachBaoCaoPhuTho.TDVM_BAO_CAO_NO_QUA_HAN;
                case "TDVM_LICH_TRA_NO": return DanhSachBaoCaoPhuTho.TDVM_LICH_TRA_NO;
                case "BCTH_HOAT_DONG": return DanhSachBaoCaoPhuTho.BCTH_HOAT_DONG;
                case "BCTC_BANG_CAN_DOI_TKKT_THU": return DanhSachBaoCaoPhuTho.BCTC_BANG_CAN_DOI_TKKT_THU;
                case "BCTC_BANG_CAN_DOI_KE_TOAN": return DanhSachBaoCaoPhuTho.BCTC_BANG_CAN_DOI_KE_TOAN;
                case "BCTC_BANG_CAN_DOI_CHUNG_TU": return DanhSachBaoCaoPhuTho.BCTC_BANG_CAN_DOI_CHUNG_TU;
                case "BCTC_KET_QUA_HOAT_DONG": return DanhSachBaoCaoPhuTho.BCTC_KET_QUA_HOAT_DONG;
                case "BCTC_LUU_CHUYEN_TIEN_TE": return DanhSachBaoCaoPhuTho.BCTC_LUU_CHUYEN_TIEN_TE;
                case "BCTC_THUYET_MINH_TAI_CHINH": return DanhSachBaoCaoPhuTho.BCTC_THUYET_MINH_TAI_CHINH;
                case "BCTH_BAO_CAO_SO_THANH_VIEN": return DanhSachBaoCaoPhuTho.BCTH_BAO_CAO_SO_THANH_VIEN;
                case "BCTH_SO_TONG_HOP_TIET_KIEM": return DanhSachBaoCaoPhuTho.BCTH_SO_TONG_HOP_TIET_KIEM;
                case "BCTH_SO_TONG_HOP_TUONG_TRO": return DanhSachBaoCaoPhuTho.BCTH_SO_TONG_HOP_TUONG_TRO;
                case "BCTH_SO_TONG_HOP_VAY_VON": return DanhSachBaoCaoPhuTho.BCTH_SO_TONG_HOP_VAY_VON;
                case "BCTH_BAO_CAO_LAI_VON_VAY": return DanhSachBaoCaoPhuTho.BCTH_BAO_CAO_LAI_VON_VAY;
                default: return DanhSachBaoCaoPhuTho.KHTV_QUYET_DINH_CONG_NHAN_TVIEN;
            }
        }

        public enum DanhSachBaoCaoBIDV
        {
            KHTV_QUYET_DINH_CONG_NHAN_TVIEN,
            KHTV_BIEN_BAN_HOP_NHOM,
            TDVM_DON_VAY_VON,
            TDVM_HOP_DONG_VAY_VON,
            TDVM_KHE_UOC_VAY_VON,
            TDVM_BANG_KHOACH_TRA_NO,
            TDVM_KHE_UOC_NHAN_NO,
            TDVM_PHU_LUC_HOP_DONG,
            TDVM_BANG_KE_RUT_VON,
            TDVM_PHIEU_KHAO_SAT_KHACH_HANG,
            TDVM_BAO_CAO_DE_XUAT_TIN_DUNG,
            TDVM_BAO_CAO_THAM_DINH_RUI_RO_TD,
            TDVM_DS_PHAT_VON,
            TDVM_SO_VON_VAY_TIET_KIEM_KHTV,
            TDVM_DS_THU_TIEN_VON_TRA_DAN_TK,
            TDTT_GIAY_DE_NGHI_VAY_VON_CA_NHAN,
            TDTT_PHIEU_KHAO_SAT_KH_CA_NHAN,
            TDTT_BAO_CAO_DE_XUAT_TIN_DUNG,
            TDTT_BAO_CAO_THAM_DINH_RUI_RO_TD,
            TDTT_HOP_DONG_TIN_DUNG,
            TDTT_PHU_LUC_HOP_DONG,
            TDTT_HOP_DONG_TIN_DUNG_BLANH_BEN_THU_BA,
            TDTT_PHU_LUC_HOP_DONG_BLANH_BEN_THU_BA,
            TDTT_KHE_UOC_RUT_VON,
            TDTT_KHE_UOC_RUT_VON_BLANH_BEN_THU_BA,
            TDTT_SAO_KE_TIN_DUNG_TH,
            TDTT_BANG_KE_RUT_VON,
            TDTT_SO_VON_VAY_TIET_KIEM_KHTV,
            TDTT_DE_XUAT_PHE_DUYET_GIAI_NGAN,
            TDTT_DANH_SACH_THU_NO,
            TDTT_DANH_SACH_SAO_KE_GN_ACTIVE,
            TDTT_DANH_SACH_SAO_KE_GN_PERIOD,
            HDVO_SO_TKCKH,
            GDKT_PHIEU_CHI,
            GDKT_PHIEU_CHI_KH,
            GDKT_PHIEU_THU,
            GDKT_PHIEU_THU_KH,
            GDKT_PHIEU_KE_TOAN,
            GDKT_PHIEU_KE_TOAN_01,
            GDKT_PHIEU_KE_TOAN_GDICH,
            KHTV_DANH_SACH_KHTV,
            BCTK_DAILY_REPORT,
            BCTK_MONTHLY_REPORT,
            BCTK_NGANH_NGHE_MMSC,
            BCTK_THEO_DOANH_SO,
            BCTK_THEO_NGANH_NGHE,
            BCTK_QUAN_LY_NO,
            GDKT_BALANCE_SHEET,
            GDKT_PROFIT_AND_LOSS,
            BCTH_CREDIT_STATEMENT,
            BCTH_BAO_CAO_PHAN_LOAI_NO,
            BCTH_SAO_KE_DU_NO_TIN_DUNG_TKIEM,
            BCTH_BAO_CAO_DONG_TIEN,
            TDTD_BANG_KHOACH_TRA_NO,
            HDVO_GUI_TIEN_DS,
            HDVO_GUI_TIEN_DS_EXCEL,
            HDVO_DS_TIEN_GUI_DEN_HAN,
            TDVM_DANH_SACH_THU_NO,
            BCTH_BLACKLIST,
            TDVM_DANH_SACH_SAO_KE_GN_ACTIVE,
            TDVM_DANH_SACH_SAO_KE_GN_PERIOD,
            TDVM_DANH_SACH_THANH_TOAN_BAT_BUOC
        }

        public static string LayMaBaoCaoBIDV(DanhSachBaoCaoBIDV dsbaocao)
        {
            switch (dsbaocao)
            {
                case DanhSachBaoCaoBIDV.TDVM_DON_VAY_VON: return "TDVM_DON_VAY_VON";
                case DanhSachBaoCaoBIDV.TDVM_HOP_DONG_VAY_VON: return "TDVM_HOP_DONG_VAY_VON";
                case DanhSachBaoCaoBIDV.TDVM_KHE_UOC_VAY_VON: return "TDVM_KHE_UOC_VAY_VON";
                case DanhSachBaoCaoBIDV.TDVM_BANG_KHOACH_TRA_NO: return "TDVM_BANG_KHOACH_TRA_NO";
                case DanhSachBaoCaoBIDV.TDVM_KHE_UOC_NHAN_NO: return "TDVM_KHE_UOC_NHAN_NO";
                case DanhSachBaoCaoBIDV.TDVM_PHU_LUC_HOP_DONG: return "TDVM_PHU_LUC_HOP_DONG";
                case DanhSachBaoCaoBIDV.TDVM_BANG_KE_RUT_VON: return "TDVM_BANG_KE_RUT_VON";
                case DanhSachBaoCaoBIDV.TDVM_PHIEU_KHAO_SAT_KHACH_HANG: return "TDVM_PHIEU_KHAO_SAT_KHACH_HANG";
                case DanhSachBaoCaoBIDV.TDVM_BAO_CAO_DE_XUAT_TIN_DUNG: return "TDVM_BAO_CAO_DE_XUAT_TIN_DUNG";
                case DanhSachBaoCaoBIDV.TDVM_BAO_CAO_THAM_DINH_RUI_RO_TD: return "TDVM_BAO_CAO_THAM_DINH_RUI_RO_TD";
                case DanhSachBaoCaoBIDV.TDVM_DS_PHAT_VON: return "TDVM_DS_PHAT_VON";
                case DanhSachBaoCaoBIDV.TDVM_SO_VON_VAY_TIET_KIEM_KHTV: return "TDVM_SO_VON_VAY_TIET_KIEM_KHTV";
                case DanhSachBaoCaoBIDV.TDVM_DS_THU_TIEN_VON_TRA_DAN_TK: return "TDVM_DS_THU_TIEN_VON_TRA_DAN_TK";
                case DanhSachBaoCaoBIDV.TDTT_GIAY_DE_NGHI_VAY_VON_CA_NHAN: return "TDTT_GIAY_DE_NGHI_VAY_VON_CA_NHAN";
                case DanhSachBaoCaoBIDV.TDTT_PHIEU_KHAO_SAT_KH_CA_NHAN: return "TDTT_PHIEU_KHAO_SAT_KH_CA_NHAN";
                case DanhSachBaoCaoBIDV.TDTT_BAO_CAO_DE_XUAT_TIN_DUNG: return "TDTT_BAO_CAO_DE_XUAT_TIN_DUNG";
                case DanhSachBaoCaoBIDV.TDTT_BAO_CAO_THAM_DINH_RUI_RO_TD: return "TDTT_BAO_CAO_THAM_DINH_RUI_RO_TD";
                case DanhSachBaoCaoBIDV.TDTT_HOP_DONG_TIN_DUNG: return "TDTT_HOP_DONG_TIN_DUNG";
                case DanhSachBaoCaoBIDV.TDTT_PHU_LUC_HOP_DONG: return "TDTT_PHU_LUC_HOP_DONG";
                case DanhSachBaoCaoBIDV.TDTT_HOP_DONG_TIN_DUNG_BLANH_BEN_THU_BA: return "TDTT_HOP_DONG_TIN_DUNG_BLANH_BEN_THU_BA";
                case DanhSachBaoCaoBIDV.TDTT_PHU_LUC_HOP_DONG_BLANH_BEN_THU_BA: return "TDTT_PHU_LUC_HOP_DONG_BLANH_BEN_THU_BA";
                case DanhSachBaoCaoBIDV.TDTT_KHE_UOC_RUT_VON: return "TDTT_KHE_UOC_RUT_VON";
                case DanhSachBaoCaoBIDV.TDTT_KHE_UOC_RUT_VON_BLANH_BEN_THU_BA: return "TDTT_KHE_UOC_RUT_VON_BLANH_BEN_THU_BA";
                case DanhSachBaoCaoBIDV.TDTT_SAO_KE_TIN_DUNG_TH: return "TDTT_SAO_KE_TIN_DUNG_TH";
                case DanhSachBaoCaoBIDV.TDTT_SO_VON_VAY_TIET_KIEM_KHTV: return "TDTT_SO_VON_VAY_TIET_KIEM_KHTV";
                case DanhSachBaoCaoBIDV.TDTT_DE_XUAT_PHE_DUYET_GIAI_NGAN: return "TDTT_DE_XUAT_PHE_DUYET_GIAI_NGAN";
                case DanhSachBaoCaoBIDV.TDTT_DANH_SACH_THU_NO: return "TDTT_DANH_SACH_THU_NO";
                case DanhSachBaoCaoBIDV.TDTT_DANH_SACH_SAO_KE_GN_ACTIVE: return "TDTT_DANH_SACH_SAO_KE_GN_ACTIVE";
                case DanhSachBaoCaoBIDV.TDTT_DANH_SACH_SAO_KE_GN_PERIOD: return "TDTT_DANH_SACH_SAO_KE_GN_PERIOD";
                case DanhSachBaoCaoBIDV.HDVO_SO_TKCKH: return "HDVO_SO_TKCKH";
                case DanhSachBaoCaoBIDV.GDKT_PHIEU_CHI: return "GDKT_PHIEU_CHI";
                case DanhSachBaoCaoBIDV.GDKT_PHIEU_CHI_KH: return "GDKT_PHIEU_CHI_KH";
                case DanhSachBaoCaoBIDV.GDKT_PHIEU_THU: return "GDKT_PHIEU_THU";
                case DanhSachBaoCaoBIDV.GDKT_PHIEU_THU_KH: return "GDKT_PHIEU_THU_KH";
                case DanhSachBaoCaoBIDV.GDKT_PHIEU_KE_TOAN: return "GDKT_PHIEU_KE_TOAN";
                case DanhSachBaoCaoBIDV.GDKT_PHIEU_KE_TOAN_01: return "GDKT_PHIEU_KE_TOAN_01";
                case DanhSachBaoCaoBIDV.GDKT_PHIEU_KE_TOAN_GDICH: return "GDKT_PHIEU_KE_TOAN_GDICH";
                case DanhSachBaoCaoBIDV.KHTV_DANH_SACH_KHTV: return "KHTV_DANH_SACH_KHTV";
                case DanhSachBaoCaoBIDV.KHTV_BIEN_BAN_HOP_NHOM: return "KHTV_BIEN_BAN_HOP_NHOM";
                case DanhSachBaoCaoBIDV.BCTK_DAILY_REPORT: return "BCTK_DAILY_REPORT";
                case DanhSachBaoCaoBIDV.BCTK_MONTHLY_REPORT: return "BCTK_MONTHLY_REPORT";
                case DanhSachBaoCaoBIDV.BCTK_NGANH_NGHE_MMSC: return "BCTK_NGANH_NGHE_MMSC";
                case DanhSachBaoCaoBIDV.BCTK_THEO_DOANH_SO: return "BCTK_THEO_DOANH_SO";
                case DanhSachBaoCaoBIDV.BCTK_THEO_NGANH_NGHE: return "BCTK_THEO_NGANH_NGHE";
                case DanhSachBaoCaoBIDV.BCTK_QUAN_LY_NO: return "BCTK_QUAN_LY_NO";
                case DanhSachBaoCaoBIDV.GDKT_BALANCE_SHEET: return "GDKT_BALANCE_SHEET";
                case DanhSachBaoCaoBIDV.GDKT_PROFIT_AND_LOSS: return "GDKT_PROFIT_AND_LOSS";
                case DanhSachBaoCaoBIDV.BCTH_CREDIT_STATEMENT: return "BCTH_CREDIT_STATEMENT";
                case DanhSachBaoCaoBIDV.BCTH_BAO_CAO_PHAN_LOAI_NO: return "BCTH_BAO_CAO_PHAN_LOAI_NO";
                case DanhSachBaoCaoBIDV.BCTH_SAO_KE_DU_NO_TIN_DUNG_TKIEM: return "BCTH_SAO_KE_DU_NO_TIN_DUNG_TKIEM";
                case DanhSachBaoCaoBIDV.BCTH_BAO_CAO_DONG_TIEN: return "BCTH_BAO_CAO_DONG_TIEN";
                case DanhSachBaoCaoBIDV.TDTD_BANG_KHOACH_TRA_NO: return "TDTD_BANG_KHOACH_TRA_NO";
                case DanhSachBaoCaoBIDV.HDVO_GUI_TIEN_DS: return "HDVO_GUI_TIEN_DS";
                case DanhSachBaoCaoBIDV.HDVO_GUI_TIEN_DS_EXCEL: return "HDVO_GUI_TIEN_DS_EXCEL";
                case DanhSachBaoCaoBIDV.HDVO_DS_TIEN_GUI_DEN_HAN: return "HDVO_DS_TIEN_GUI_DEN_HAN";
                case DanhSachBaoCaoBIDV.TDVM_DANH_SACH_THU_NO: return "TDVM_DANH_SACH_THU_NO";
                case DanhSachBaoCaoBIDV.BCTH_BLACKLIST: return "BCTH_BLACKLIST";
                case DanhSachBaoCaoBIDV.TDVM_DANH_SACH_SAO_KE_GN_ACTIVE: return "TDVM_DANH_SACH_SAO_KE_GN_ACTIVE";
                case DanhSachBaoCaoBIDV.TDVM_DANH_SACH_SAO_KE_GN_PERIOD: return "TDVM_DANH_SACH_SAO_KE_GN_PERIOD";
                case DanhSachBaoCaoBIDV.TDVM_DANH_SACH_THANH_TOAN_BAT_BUOC: return "TDVM_DANH_SACH_THANH_TOAN_BAT_BUOC";
                default: return "";
            }
        }

        public static DanhSachBaoCaoBIDV layBaoCaoBIDVTheoMa(string maBaoCao)
        {
            switch (maBaoCao)
            {
                case "TDVM_DON_VAY_VON": return DanhSachBaoCaoBIDV.TDVM_DON_VAY_VON;
                case "TDVM_HOP_DONG_VAY_VON": return DanhSachBaoCaoBIDV.TDVM_HOP_DONG_VAY_VON;
                case "TDVM_KHE_UOC_VAY_VON": return DanhSachBaoCaoBIDV.TDVM_KHE_UOC_VAY_VON;
                case "TDVM_BANG_KHOACH_TRA_NO": return DanhSachBaoCaoBIDV.TDVM_BANG_KHOACH_TRA_NO;
                case "TDVM_KHE_UOC_NHAN_NO": return DanhSachBaoCaoBIDV.TDVM_KHE_UOC_NHAN_NO;
                case "TDVM_PHU_LUC_HOP_DONG": return DanhSachBaoCaoBIDV.TDVM_PHU_LUC_HOP_DONG;
                case "TDVM_BANG_KE_RUT_VON": return DanhSachBaoCaoBIDV.TDVM_BANG_KE_RUT_VON;
                case "TDVM_PHIEU_KHAO_SAT_KHACH_HANG": return DanhSachBaoCaoBIDV.TDVM_PHIEU_KHAO_SAT_KHACH_HANG;
                case "TDVM_BAO_CAO_DE_XUAT_TIN_DUNG": return DanhSachBaoCaoBIDV.TDVM_BAO_CAO_DE_XUAT_TIN_DUNG;
                case "TDVM_BAO_CAO_THAM_DINH_RUI_RO_TD": return DanhSachBaoCaoBIDV.TDVM_BAO_CAO_THAM_DINH_RUI_RO_TD;
                case "TDVM_DS_PHAT_VON": return DanhSachBaoCaoBIDV.TDVM_DS_PHAT_VON;
                case "TDVM_SO_VON_VAY_TIET_KIEM_KHTV": return DanhSachBaoCaoBIDV.TDVM_SO_VON_VAY_TIET_KIEM_KHTV;
                case "TDVM_DS_THU_TIEN_VON_TRA_DAN_TK": return DanhSachBaoCaoBIDV.TDVM_DS_THU_TIEN_VON_TRA_DAN_TK;
                case "TDTT_GIAY_DE_NGHI_VAY_VON_CA_NHAN": return DanhSachBaoCaoBIDV.TDTT_GIAY_DE_NGHI_VAY_VON_CA_NHAN;
                case "TDTT_PHIEU_KHAO_SAT_KH_CA_NHAN": return DanhSachBaoCaoBIDV.TDTT_PHIEU_KHAO_SAT_KH_CA_NHAN;
                case "TDTT_BAO_CAO_DE_XUAT_TIN_DUNG": return DanhSachBaoCaoBIDV.TDTT_BAO_CAO_DE_XUAT_TIN_DUNG;
                case "TDTT_BAO_CAO_THAM_DINH_RUI_RO_TD": return DanhSachBaoCaoBIDV.TDTT_BAO_CAO_THAM_DINH_RUI_RO_TD;
                case "TDTT_HOP_DONG_TIN_DUNG": return DanhSachBaoCaoBIDV.TDTT_HOP_DONG_TIN_DUNG;
                case "TDTT_PHU_LUC_HOP_DONG": return DanhSachBaoCaoBIDV.TDTT_PHU_LUC_HOP_DONG;
                case "TDTT_HOP_DONG_TIN_DUNG_BLANH_BEN_THU_BA": return DanhSachBaoCaoBIDV.TDTT_HOP_DONG_TIN_DUNG_BLANH_BEN_THU_BA;
                case "TDTT_PHU_LUC_HOP_DONG_BLANH_BEN_THU_BA": return DanhSachBaoCaoBIDV.TDTT_PHU_LUC_HOP_DONG_BLANH_BEN_THU_BA;
                case "TDTT_KHE_UOC_RUT_VON": return DanhSachBaoCaoBIDV.TDTT_KHE_UOC_RUT_VON;
                case "TDTT_KHE_UOC_RUT_VON_BLANH_BEN_THU_BA": return DanhSachBaoCaoBIDV.TDTT_KHE_UOC_RUT_VON_BLANH_BEN_THU_BA;
                case "TDTT_SAO_KE_TIN_DUNG_TH": return DanhSachBaoCaoBIDV.TDTT_SAO_KE_TIN_DUNG_TH;
                case "TDTT_SO_VON_VAY_TIET_KIEM_KHTV": return DanhSachBaoCaoBIDV.TDTT_SO_VON_VAY_TIET_KIEM_KHTV;
                case "TDTT_DE_XUAT_PHE_DUYET_GIAI_NGAN": return DanhSachBaoCaoBIDV.TDTT_DE_XUAT_PHE_DUYET_GIAI_NGAN;
                case "TDTT_DANH_SACH_THU_NO": return DanhSachBaoCaoBIDV.TDTT_DANH_SACH_THU_NO;
                case "TDTT_DANH_SACH_SAO_KE_GN_ACTIVE": return DanhSachBaoCaoBIDV.TDTT_DANH_SACH_SAO_KE_GN_ACTIVE;
                case "TDTT_DANH_SACH_SAO_KE_GN_PERIOD": return DanhSachBaoCaoBIDV.TDTT_DANH_SACH_SAO_KE_GN_PERIOD;
                case "HDVO_SO_TKCKH": return DanhSachBaoCaoBIDV.HDVO_SO_TKCKH;
                case "GDKT_PHIEU_CHI": return DanhSachBaoCaoBIDV.GDKT_PHIEU_CHI;
                case "GDKT_PHIEU_CHI_KH": return DanhSachBaoCaoBIDV.GDKT_PHIEU_CHI_KH;
                case "GDKT_PHIEU_THU": return DanhSachBaoCaoBIDV.GDKT_PHIEU_THU;
                case "GDKT_PHIEU_THU_KH": return DanhSachBaoCaoBIDV.GDKT_PHIEU_THU_KH;
                case "GDKT_PHIEU_KE_TOAN": return DanhSachBaoCaoBIDV.GDKT_PHIEU_KE_TOAN;
                case "GDKT_PHIEU_KE_TOAN_01": return DanhSachBaoCaoBIDV.GDKT_PHIEU_KE_TOAN_01;
                case "GDKT_PHIEU_KE_TOAN_GDICH": return DanhSachBaoCaoBIDV.GDKT_PHIEU_KE_TOAN_GDICH;
                case "KHTV_DANH_SACH_KHTV": return DanhSachBaoCaoBIDV.KHTV_DANH_SACH_KHTV;
                case "KHTV_BIEN_BAN_HOP_NHOM": return DanhSachBaoCaoBIDV.KHTV_BIEN_BAN_HOP_NHOM;
                case "BCTK_DAILY_REPORT": return DanhSachBaoCaoBIDV.BCTK_DAILY_REPORT;
                case "BCTK_MONTHLY_REPORT": return DanhSachBaoCaoBIDV.BCTK_MONTHLY_REPORT;
                case "BCTK_NGANH_NGHE_MMSC": return DanhSachBaoCaoBIDV.BCTK_NGANH_NGHE_MMSC;
                case "BCTK_THEO_DOANH_SO": return DanhSachBaoCaoBIDV.BCTK_THEO_DOANH_SO;
                case "BCTK_THEO_NGANH_NGHE": return DanhSachBaoCaoBIDV.BCTK_THEO_NGANH_NGHE;
                case "BCTK_QUAN_LY_NO": return DanhSachBaoCaoBIDV.BCTK_QUAN_LY_NO;
                case "GDKT_BALANCE_SHEET": return DanhSachBaoCaoBIDV.GDKT_BALANCE_SHEET;
                case "GDKT_PROFIT_AND_LOSS": return DanhSachBaoCaoBIDV.GDKT_PROFIT_AND_LOSS;
                case "BCTH_CREDIT_STATEMENT": return DanhSachBaoCaoBIDV.BCTH_CREDIT_STATEMENT;
                case "BCTH_BAO_CAO_PHAN_LOAI_NO": return DanhSachBaoCaoBIDV.BCTH_BAO_CAO_PHAN_LOAI_NO;
                case "BCTH_SAO_KE_DU_NO_TIN_DUNG_TKIEM": return DanhSachBaoCaoBIDV.BCTH_SAO_KE_DU_NO_TIN_DUNG_TKIEM;
                case "BCTH_BAO_CAO_DONG_TIEN": return DanhSachBaoCaoBIDV.BCTH_BAO_CAO_DONG_TIEN;
                case "TDTD_BANG_KHOACH_TRA_NO": return DanhSachBaoCaoBIDV.TDTD_BANG_KHOACH_TRA_NO;
                case "HDVO_GUI_TIEN_DS": return DanhSachBaoCaoBIDV.HDVO_GUI_TIEN_DS;
                case "HDVO_GUI_TIEN_DS_EXCEL": return DanhSachBaoCaoBIDV.HDVO_GUI_TIEN_DS_EXCEL;
                case "HDVO_DS_TIEN_GUI_DEN_HAN": return DanhSachBaoCaoBIDV.HDVO_DS_TIEN_GUI_DEN_HAN;
                case "TDVM_DANH_SACH_THU_NO": return DanhSachBaoCaoBIDV.TDVM_DANH_SACH_THU_NO;
                case "BCTH_BLACKLIST": return DanhSachBaoCaoBIDV.BCTH_BLACKLIST;
                case "TDVM_DANH_SACH_SAO_KE_GN_ACTIVE": return DanhSachBaoCaoBIDV.TDVM_DANH_SACH_SAO_KE_GN_ACTIVE;
                case "TDVM_DANH_SACH_SAO_KE_GN_PERIOD": return DanhSachBaoCaoBIDV.TDVM_DANH_SACH_SAO_KE_GN_PERIOD;
                case "TDVM_DANH_SACH_THANH_TOAN_BAT_BUOC": return DanhSachBaoCaoBIDV.TDVM_DANH_SACH_THANH_TOAN_BAT_BUOC;
                default: return DanhSachBaoCaoBIDV.KHTV_QUYET_DINH_CONG_NHAN_TVIEN;
            }
        }

        public static bool LayGiaTriBaoCaoBIDV(string maBaoCao)
        {
            switch (maBaoCao)
            {
                case "TDVM_DON_VAY_VON": return true;
                case "TDVM_HOP_DONG_VAY_VON": return true;
                case "TDVM_KHE_UOC_VAY_VON": return true;
                case "TDVM_BANG_KHOACH_TRA_NO": return true;
                case "TDVM_KHE_UOC_NHAN_NO": return true;
                case "TDVM_PHU_LUC_HOP_DONG": return true;
                case "TDVM_BANG_KE_RUT_VON": return true;
                case "TDVM_PHIEU_KHAO_SAT_KHACH_HANG": return true;
                case "TDVM_BAO_CAO_DE_XUAT_TIN_DUNG": return true;
                case "TDVM_BAO_CAO_THAM_DINH_RUI_RO_TD": return true;
                case "TDVM_DS_PHAT_VON": return true;
                case "TDVM_SO_VON_VAY_TIET_KIEM_KHTV": return true;
                case "TDVM_DS_THU_TIEN_VON_TRA_DAN_TK": return true;
                case "TDTT_GIAY_DE_NGHI_VAY_VON_CA_NHAN": return true;
                case "TDTT_PHIEU_KHAO_SAT_KH_CA_NHAN": return true;
                case "TDTT_BAO_CAO_DE_XUAT_TIN_DUNG": return true;
                case "TDTT_BAO_CAO_THAM_DINH_RUI_RO_TD": return true;
                case "TDTT_HOP_DONG_TIN_DUNG": return true;
                case "TDTT_PHU_LUC_HOP_DONG": return true;
                case "TDTT_HOP_DONG_TIN_DUNG_BLANH_BEN_THU_BA": return true;
                case "TDTT_PHU_LUC_HOP_DONG_BLANH_BEN_THU_BA": return true;
                case "TDTT_KHE_UOC_RUT_VON": return true;
                case "TDTT_KHE_UOC_RUT_VON_BLANH_BEN_THU_BA": return true;
                case "TDTT_SAO_KE_TIN_DUNG_TH": return true;
                case "TDTT_SO_VON_VAY_TIET_KIEM_KHTV": return true;
                case "TDTT_DE_XUAT_PHE_DUYET_GIAI_NGAN": return true;
                case "TDTT_DANH_SACH_THU_NO": return true;
                case "TDTT_DANH_SACH_SAO_KE_GN_ACTIVE": return true;
                case "TDTT_DANH_SACH_SAO_KE_GN_PERIOD": return true;
                case "HDVO_SO_TKCKH": return true;
                case "GDKT_PHIEU_CHI": return true;
                case "GDKT_PHIEU_CHI_KH": return true;
                case "GDKT_PHIEU_THU": return true;
                case "GDKT_PHIEU_THU_KH": return true;
                case "GDKT_PHIEU_KE_TOAN": return true;
                case "GDKT_PHIEU_KE_TOAN_01": return true;
                case "GDKT_PHIEU_KE_TOAN_GDICH": return true;
                case "KHTV_DANH_SACH_KHTV": return true;
                case "KHTV_BIEN_BAN_HOP_NHOM": return true;
                case "BCTK_DAILY_REPORT": return true;
                case "BCTK_MONTHLY_REPORT": return true;
                case "BCTK_NGANH_NGHE_MMSC": return true;
                case "BCTK_THEO_DOANH_SO": return true;
                case "BCTK_THEO_NGANH_NGHE": return true;
                case "BCTK_QUAN_LY_NO": return true;
                case "GDKT_BALANCE_SHEET": return true;
                case "GDKT_PROFIT_AND_LOSS": return true;
                case "BCTH_CREDIT_STATEMENT": return true;
                case "BCTH_BAO_CAO_PHAN_LOAI_NO": return true;
                case "BCTH_SAO_KE_DU_NO_TIN_DUNG_TKIEM": return true;
                case "BCTH_BAO_CAO_DONG_TIEN": return true;
                case "TDTD_BANG_KHOACH_TRA_NO": return true;
                case "HDVO_GUI_TIEN_DS": return true;
                case "HDVO_GUI_TIEN_DS_EXCEL": return true;
                case "HDVO_DS_TIEN_GUI_DEN_HAN": return true;
                case "TDVM_DANH_SACH_THU_NO": return true;
                case "BCTH_BLACKLIST": return true;
                case "TDVM_DANH_SACH_SAO_KE_GN_ACTIVE": return true;
                case "TDVM_DANH_SACH_SAO_KE_GN_PERIOD": return true;
                case "TDVM_DANH_SACH_THANH_TOAN_BAT_BUOC": return true;
                default: return false;
            }
        }

        public enum DanhSachBaoCaoBIDV_BLF
        {
            KHTV_QUYET_DINH_CONG_NHAN_TVIEN,
            KHTV_BIEN_BAN_HOP_NHOM,
            TDVM_DON_VAY_VON,
            TDVM_HOP_DONG_VAY_VON,
            TDVM_KHE_UOC_VAY_VON,
            TDVM_BANG_KHOACH_TRA_NO,
            TDVM_KHE_UOC_NHAN_NO,
            TDVM_PHU_LUC_HOP_DONG,
            TDVM_BANG_KE_RUT_VON,
            TDVM_PHIEU_KHAO_SAT_KHACH_HANG,
            TDVM_BAO_CAO_DE_XUAT_TIN_DUNG,
            TDVM_BAO_CAO_THAM_DINH_RUI_RO_TD,
            TDVM_DS_PHAT_VON,
            TDVM_SO_VON_VAY_TIET_KIEM_KHTV,
            TDVM_DS_THU_TIEN_VON_TRA_DAN_TK,
            TDTT_GIAY_DE_NGHI_VAY_VON_CA_NHAN,
            TDTT_PHIEU_KHAO_SAT_KH_CA_NHAN,
            TDTT_BAO_CAO_DE_XUAT_TIN_DUNG,
            TDTT_BAO_CAO_THAM_DINH_RUI_RO_TD,
            TDTT_HOP_DONG_TIN_DUNG,
            TDTT_PHU_LUC_HOP_DONG,
            TDTT_HOP_DONG_TIN_DUNG_BLANH_BEN_THU_BA,
            TDTT_PHU_LUC_HOP_DONG_BLANH_BEN_THU_BA,
            TDTT_KHE_UOC_RUT_VON,
            TDTT_KHE_UOC_RUT_VON_BLANH_BEN_THU_BA,
            TDTT_SAO_KE_TIN_DUNG_TH,
            TDTT_BANG_KE_RUT_VON,
            TDTT_SO_VON_VAY_TIET_KIEM_KHTV,
            TDTT_DE_XUAT_PHE_DUYET_GIAI_NGAN,
            TDTT_DANH_SACH_THU_NO,
            TDTT_DANH_SACH_SAO_KE_GN_ACTIVE,
            TDTT_DANH_SACH_SAO_KE_GN_PERIOD,
            HDVO_SO_TKCKH,
            GDKT_PHIEU_CHI,
            GDKT_PHIEU_CHI_KH,
            GDKT_PHIEU_THU,
            GDKT_PHIEU_THU_KH,
            GDKT_PHIEU_KE_TOAN,
            GDKT_PHIEU_KE_TOAN_01,
            GDKT_PHIEU_KE_TOAN_GDICH,
            KHTV_DANH_SACH_KHTV,
            BCTK_DAILY_REPORT,
            BCTK_MONTHLY_REPORT,
            BCTK_NGANH_NGHE_MMSC,
            BCTK_THEO_DOANH_SO,
            BCTK_THEO_NGANH_NGHE,
            BCTK_QUAN_LY_NO,
            GDKT_BALANCE_SHEET,
            GDKT_PROFIT_AND_LOSS,
            BCTH_CREDIT_STATEMENT,
            BCTH_BAO_CAO_PHAN_LOAI_NO,
            BCTH_SAO_KE_DU_NO_TIN_DUNG_TKIEM,
            BCTH_BAO_CAO_DONG_TIEN,
            TDTD_BANG_KHOACH_TRA_NO,
            HDVO_GUI_TIEN_DS,
            HDVO_GUI_TIEN_DS_EXCEL,
            HDVO_DS_TIEN_GUI_DEN_HAN,
            GDKT_BANG_CAN_DOI_PHAT_SINH,
            BCTC_BANG_CAN_DOI_KE_TOAN,
            BCTC_KET_QUA_HOAT_DONG,
            HDVO_DS_TK_TIEN_GUI_MO_MOI,
            HDVO_DS_TK_TIET_KIEM_DEN_HAN,
            HDVO_DS_TK_TIET_KIEM_TT_TRONG_NGAY,
            HDVO_DS_TK_TIET_KIEM_TT_TRUOC_HAN,
            HDVO_DS_TK_TIET_KIEM_TT_DUNG_HAN,
            HDVO_DS_SO_QUAY_VONG_TRONG_NGAY,
            HDVO_HUY_DONG_VON_BINH_QUAN,
            KHTV_BC_KH_TAO_MOI,
            KHTV_BC_KH_THAY_DOI_TEN,
            KHTV_BC_KH_THAY_DOI_NGANH_KINH_TE,
            TDTD_BC_GIA_TRI_TSDB_HANG_NGAY
        }

        public static string LayMaBaoCaoBIDV_BLF(DanhSachBaoCaoBIDV_BLF dsbaocao)
        {
            switch (dsbaocao)
            {
                case DanhSachBaoCaoBIDV_BLF.TDVM_DON_VAY_VON: return "TDVM_DON_VAY_VON";
                case DanhSachBaoCaoBIDV_BLF.TDVM_HOP_DONG_VAY_VON: return "TDVM_HOP_DONG_VAY_VON";
                case DanhSachBaoCaoBIDV_BLF.TDVM_KHE_UOC_VAY_VON: return "TDVM_KHE_UOC_VAY_VON";
                case DanhSachBaoCaoBIDV_BLF.TDVM_BANG_KHOACH_TRA_NO: return "TDVM_BANG_KHOACH_TRA_NO";
                case DanhSachBaoCaoBIDV_BLF.TDVM_KHE_UOC_NHAN_NO: return "TDVM_KHE_UOC_NHAN_NO";
                case DanhSachBaoCaoBIDV_BLF.TDVM_PHU_LUC_HOP_DONG: return "TDVM_PHU_LUC_HOP_DONG";
                case DanhSachBaoCaoBIDV_BLF.TDVM_BANG_KE_RUT_VON: return "TDVM_BANG_KE_RUT_VON";
                case DanhSachBaoCaoBIDV_BLF.TDVM_PHIEU_KHAO_SAT_KHACH_HANG: return "TDVM_PHIEU_KHAO_SAT_KHACH_HANG";
                case DanhSachBaoCaoBIDV_BLF.TDVM_BAO_CAO_DE_XUAT_TIN_DUNG: return "TDVM_BAO_CAO_DE_XUAT_TIN_DUNG";
                case DanhSachBaoCaoBIDV_BLF.TDVM_BAO_CAO_THAM_DINH_RUI_RO_TD: return "TDVM_BAO_CAO_THAM_DINH_RUI_RO_TD";
                case DanhSachBaoCaoBIDV_BLF.TDVM_DS_PHAT_VON: return "TDVM_DS_PHAT_VON";
                case DanhSachBaoCaoBIDV_BLF.TDVM_SO_VON_VAY_TIET_KIEM_KHTV: return "TDVM_SO_VON_VAY_TIET_KIEM_KHTV";
                case DanhSachBaoCaoBIDV_BLF.TDVM_DS_THU_TIEN_VON_TRA_DAN_TK: return "TDVM_DS_THU_TIEN_VON_TRA_DAN_TK";
                case DanhSachBaoCaoBIDV_BLF.TDTT_GIAY_DE_NGHI_VAY_VON_CA_NHAN: return "TDTT_GIAY_DE_NGHI_VAY_VON_CA_NHAN";
                case DanhSachBaoCaoBIDV_BLF.TDTT_PHIEU_KHAO_SAT_KH_CA_NHAN: return "TDTT_PHIEU_KHAO_SAT_KH_CA_NHAN";
                case DanhSachBaoCaoBIDV_BLF.TDTT_BAO_CAO_DE_XUAT_TIN_DUNG: return "TDTT_BAO_CAO_DE_XUAT_TIN_DUNG";
                case DanhSachBaoCaoBIDV_BLF.TDTT_BAO_CAO_THAM_DINH_RUI_RO_TD: return "TDTT_BAO_CAO_THAM_DINH_RUI_RO_TD";
                case DanhSachBaoCaoBIDV_BLF.TDTT_HOP_DONG_TIN_DUNG: return "TDTT_HOP_DONG_TIN_DUNG";
                case DanhSachBaoCaoBIDV_BLF.TDTT_PHU_LUC_HOP_DONG: return "TDTT_PHU_LUC_HOP_DONG";
                case DanhSachBaoCaoBIDV_BLF.TDTT_HOP_DONG_TIN_DUNG_BLANH_BEN_THU_BA: return "TDTT_HOP_DONG_TIN_DUNG_BLANH_BEN_THU_BA";
                case DanhSachBaoCaoBIDV_BLF.TDTT_PHU_LUC_HOP_DONG_BLANH_BEN_THU_BA: return "TDTT_PHU_LUC_HOP_DONG_BLANH_BEN_THU_BA";
                case DanhSachBaoCaoBIDV_BLF.TDTT_KHE_UOC_RUT_VON: return "TDTT_KHE_UOC_RUT_VON";
                case DanhSachBaoCaoBIDV_BLF.TDTT_KHE_UOC_RUT_VON_BLANH_BEN_THU_BA: return "TDTT_KHE_UOC_RUT_VON_BLANH_BEN_THU_BA";
                case DanhSachBaoCaoBIDV_BLF.TDTT_SAO_KE_TIN_DUNG_TH: return "TDTT_SAO_KE_TIN_DUNG_TH";
                case DanhSachBaoCaoBIDV_BLF.TDTT_SO_VON_VAY_TIET_KIEM_KHTV: return "TDTT_SO_VON_VAY_TIET_KIEM_KHTV";
                case DanhSachBaoCaoBIDV_BLF.TDTT_DE_XUAT_PHE_DUYET_GIAI_NGAN: return "TDTT_DE_XUAT_PHE_DUYET_GIAI_NGAN";
                case DanhSachBaoCaoBIDV_BLF.TDTT_DANH_SACH_THU_NO: return "TDTT_DANH_SACH_THU_NO";
                case DanhSachBaoCaoBIDV_BLF.TDTT_DANH_SACH_SAO_KE_GN_ACTIVE: return "TDTT_DANH_SACH_SAO_KE_GN_ACTIVE";
                case DanhSachBaoCaoBIDV_BLF.TDTT_DANH_SACH_SAO_KE_GN_PERIOD: return "TDTT_DANH_SACH_SAO_KE_GN_PERIOD";
                case DanhSachBaoCaoBIDV_BLF.HDVO_SO_TKCKH: return "HDVO_SO_TKCKH";
                case DanhSachBaoCaoBIDV_BLF.GDKT_PHIEU_CHI: return "GDKT_PHIEU_CHI";
                case DanhSachBaoCaoBIDV_BLF.GDKT_PHIEU_CHI_KH: return "GDKT_PHIEU_CHI_KH";
                case DanhSachBaoCaoBIDV_BLF.GDKT_PHIEU_THU: return "GDKT_PHIEU_THU";
                case DanhSachBaoCaoBIDV_BLF.GDKT_PHIEU_THU_KH: return "GDKT_PHIEU_THU_KH";
                case DanhSachBaoCaoBIDV_BLF.GDKT_PHIEU_KE_TOAN: return "GDKT_PHIEU_KE_TOAN";
                case DanhSachBaoCaoBIDV_BLF.GDKT_PHIEU_KE_TOAN_01: return "GDKT_PHIEU_KE_TOAN_01";
                case DanhSachBaoCaoBIDV_BLF.GDKT_PHIEU_KE_TOAN_GDICH: return "GDKT_PHIEU_KE_TOAN_GDICH";
                case DanhSachBaoCaoBIDV_BLF.KHTV_DANH_SACH_KHTV: return "KHTV_DANH_SACH_KHTV";
                case DanhSachBaoCaoBIDV_BLF.KHTV_BIEN_BAN_HOP_NHOM: return "KHTV_BIEN_BAN_HOP_NHOM";
                case DanhSachBaoCaoBIDV_BLF.BCTK_DAILY_REPORT: return "BCTK_DAILY_REPORT";
                case DanhSachBaoCaoBIDV_BLF.BCTK_MONTHLY_REPORT: return "BCTK_MONTHLY_REPORT";
                case DanhSachBaoCaoBIDV_BLF.BCTK_NGANH_NGHE_MMSC: return "BCTK_NGANH_NGHE_MMSC";
                case DanhSachBaoCaoBIDV_BLF.BCTK_THEO_DOANH_SO: return "BCTK_THEO_DOANH_SO";
                case DanhSachBaoCaoBIDV_BLF.BCTK_THEO_NGANH_NGHE: return "BCTK_THEO_NGANH_NGHE";
                case DanhSachBaoCaoBIDV_BLF.BCTK_QUAN_LY_NO: return "BCTK_QUAN_LY_NO";
                case DanhSachBaoCaoBIDV_BLF.GDKT_BALANCE_SHEET: return "GDKT_BALANCE_SHEET";
                case DanhSachBaoCaoBIDV_BLF.GDKT_PROFIT_AND_LOSS: return "GDKT_PROFIT_AND_LOSS";
                case DanhSachBaoCaoBIDV_BLF.BCTH_CREDIT_STATEMENT: return "BCTH_CREDIT_STATEMENT";
                case DanhSachBaoCaoBIDV_BLF.BCTH_BAO_CAO_PHAN_LOAI_NO: return "BCTH_BAO_CAO_PHAN_LOAI_NO";
                case DanhSachBaoCaoBIDV_BLF.BCTH_SAO_KE_DU_NO_TIN_DUNG_TKIEM: return "BCTH_SAO_KE_DU_NO_TIN_DUNG_TKIEM";
                case DanhSachBaoCaoBIDV_BLF.BCTH_BAO_CAO_DONG_TIEN: return "BCTH_BAO_CAO_DONG_TIEN";
                case DanhSachBaoCaoBIDV_BLF.TDTD_BANG_KHOACH_TRA_NO: return "TDTD_BANG_KHOACH_TRA_NO";
                case DanhSachBaoCaoBIDV_BLF.HDVO_GUI_TIEN_DS: return "HDVO_GUI_TIEN_DS";
                case DanhSachBaoCaoBIDV_BLF.HDVO_GUI_TIEN_DS_EXCEL: return "HDVO_GUI_TIEN_DS_EXCEL";
                case DanhSachBaoCaoBIDV_BLF.HDVO_DS_TIEN_GUI_DEN_HAN: return "HDVO_DS_TIEN_GUI_DEN_HAN";
                case DanhSachBaoCaoBIDV_BLF.GDKT_BANG_CAN_DOI_PHAT_SINH: return "GDKT_BANG_CAN_DOI_PHAT_SINH";
                case DanhSachBaoCaoBIDV_BLF.BCTC_BANG_CAN_DOI_KE_TOAN: return "BCTC_BANG_CAN_DOI_KE_TOAN";
                case DanhSachBaoCaoBIDV_BLF.BCTC_KET_QUA_HOAT_DONG: return "BCTC_KET_QUA_HOAT_DONG";
                case DanhSachBaoCaoBIDV_BLF.HDVO_DS_TK_TIEN_GUI_MO_MOI: return "HDVO_DS_TK_TIEN_GUI_MO_MOI";
                case DanhSachBaoCaoBIDV_BLF.HDVO_DS_TK_TIET_KIEM_DEN_HAN: return "HDVO_DS_TK_TIET_KIEM_DEN_HAN";
                case DanhSachBaoCaoBIDV_BLF.HDVO_DS_TK_TIET_KIEM_TT_TRONG_NGAY: return "HDVO_DS_TK_TIET_KIEM_TT_TRONG_NGAY";
                case DanhSachBaoCaoBIDV_BLF.HDVO_DS_TK_TIET_KIEM_TT_TRUOC_HAN: return "HDVO_DS_TK_TIET_KIEM_TT_TRUOC_HAN";
                case DanhSachBaoCaoBIDV_BLF.HDVO_DS_TK_TIET_KIEM_TT_DUNG_HAN: return "HDVO_DS_TK_TIET_KIEM_TT_DUNG_HAN";
                case DanhSachBaoCaoBIDV_BLF.HDVO_DS_SO_QUAY_VONG_TRONG_NGAY: return "HDVO_DS_SO_QUAY_VONG_TRONG_NGAY";
                case DanhSachBaoCaoBIDV_BLF.HDVO_HUY_DONG_VON_BINH_QUAN: return "HDVO_HUY_DONG_VON_BINH_QUAN";
                case DanhSachBaoCaoBIDV_BLF.KHTV_BC_KH_TAO_MOI: return "KHTV_BC_KH_TAO_MOI";
                case DanhSachBaoCaoBIDV_BLF.KHTV_BC_KH_THAY_DOI_TEN: return "KHTV_BC_KH_THAY_DOI_TEN";
                case DanhSachBaoCaoBIDV_BLF.KHTV_BC_KH_THAY_DOI_NGANH_KINH_TE: return "KHTV_BC_KH_THAY_DOI_NGANH_KINH_TE";
                case DanhSachBaoCaoBIDV_BLF.TDTD_BC_GIA_TRI_TSDB_HANG_NGAY: return "TDTD_BC_GIA_TRI_TSDB_HANG_NGAY";
                default: return "";
            }
        }

        public static DanhSachBaoCaoBIDV_BLF layBaoCaoBIDV_BLFTheoMa(string maBaoCao)
        {
            switch (maBaoCao)
            {
                case "TDVM_DON_VAY_VON": return DanhSachBaoCaoBIDV_BLF.TDVM_DON_VAY_VON;
                case "TDVM_HOP_DONG_VAY_VON": return DanhSachBaoCaoBIDV_BLF.TDVM_HOP_DONG_VAY_VON;
                case "TDVM_KHE_UOC_VAY_VON": return DanhSachBaoCaoBIDV_BLF.TDVM_KHE_UOC_VAY_VON;
                case "TDVM_BANG_KHOACH_TRA_NO": return DanhSachBaoCaoBIDV_BLF.TDVM_BANG_KHOACH_TRA_NO;
                case "TDVM_KHE_UOC_NHAN_NO": return DanhSachBaoCaoBIDV_BLF.TDVM_KHE_UOC_NHAN_NO;
                case "TDVM_PHU_LUC_HOP_DONG": return DanhSachBaoCaoBIDV_BLF.TDVM_PHU_LUC_HOP_DONG;
                case "TDVM_BANG_KE_RUT_VON": return DanhSachBaoCaoBIDV_BLF.TDVM_BANG_KE_RUT_VON;
                case "TDVM_PHIEU_KHAO_SAT_KHACH_HANG": return DanhSachBaoCaoBIDV_BLF.TDVM_PHIEU_KHAO_SAT_KHACH_HANG;
                case "TDVM_BAO_CAO_DE_XUAT_TIN_DUNG": return DanhSachBaoCaoBIDV_BLF.TDVM_BAO_CAO_DE_XUAT_TIN_DUNG;
                case "TDVM_BAO_CAO_THAM_DINH_RUI_RO_TD": return DanhSachBaoCaoBIDV_BLF.TDVM_BAO_CAO_THAM_DINH_RUI_RO_TD;
                case "TDVM_DS_PHAT_VON": return DanhSachBaoCaoBIDV_BLF.TDVM_DS_PHAT_VON;
                case "TDVM_SO_VON_VAY_TIET_KIEM_KHTV": return DanhSachBaoCaoBIDV_BLF.TDVM_SO_VON_VAY_TIET_KIEM_KHTV;
                case "TDVM_DS_THU_TIEN_VON_TRA_DAN_TK": return DanhSachBaoCaoBIDV_BLF.TDVM_DS_THU_TIEN_VON_TRA_DAN_TK;
                case "TDTT_GIAY_DE_NGHI_VAY_VON_CA_NHAN": return DanhSachBaoCaoBIDV_BLF.TDTT_GIAY_DE_NGHI_VAY_VON_CA_NHAN;
                case "TDTT_PHIEU_KHAO_SAT_KH_CA_NHAN": return DanhSachBaoCaoBIDV_BLF.TDTT_PHIEU_KHAO_SAT_KH_CA_NHAN;
                case "TDTT_BAO_CAO_DE_XUAT_TIN_DUNG": return DanhSachBaoCaoBIDV_BLF.TDTT_BAO_CAO_DE_XUAT_TIN_DUNG;
                case "TDTT_BAO_CAO_THAM_DINH_RUI_RO_TD": return DanhSachBaoCaoBIDV_BLF.TDTT_BAO_CAO_THAM_DINH_RUI_RO_TD;
                case "TDTT_HOP_DONG_TIN_DUNG": return DanhSachBaoCaoBIDV_BLF.TDTT_HOP_DONG_TIN_DUNG;
                case "TDTT_PHU_LUC_HOP_DONG": return DanhSachBaoCaoBIDV_BLF.TDTT_PHU_LUC_HOP_DONG;
                case "TDTT_HOP_DONG_TIN_DUNG_BLANH_BEN_THU_BA": return DanhSachBaoCaoBIDV_BLF.TDTT_HOP_DONG_TIN_DUNG_BLANH_BEN_THU_BA;
                case "TDTT_PHU_LUC_HOP_DONG_BLANH_BEN_THU_BA": return DanhSachBaoCaoBIDV_BLF.TDTT_PHU_LUC_HOP_DONG_BLANH_BEN_THU_BA;
                case "TDTT_KHE_UOC_RUT_VON": return DanhSachBaoCaoBIDV_BLF.TDTT_KHE_UOC_RUT_VON;
                case "TDTT_KHE_UOC_RUT_VON_BLANH_BEN_THU_BA": return DanhSachBaoCaoBIDV_BLF.TDTT_KHE_UOC_RUT_VON_BLANH_BEN_THU_BA;
                case "TDTT_SAO_KE_TIN_DUNG_TH": return DanhSachBaoCaoBIDV_BLF.TDTT_SAO_KE_TIN_DUNG_TH;
                case "TDTT_SO_VON_VAY_TIET_KIEM_KHTV": return DanhSachBaoCaoBIDV_BLF.TDTT_SO_VON_VAY_TIET_KIEM_KHTV;
                case "TDTT_DE_XUAT_PHE_DUYET_GIAI_NGAN": return DanhSachBaoCaoBIDV_BLF.TDTT_DE_XUAT_PHE_DUYET_GIAI_NGAN;
                case "TDTT_DANH_SACH_THU_NO": return DanhSachBaoCaoBIDV_BLF.TDTT_DANH_SACH_THU_NO;
                case "TDTT_DANH_SACH_SAO_KE_GN_ACTIVE": return DanhSachBaoCaoBIDV_BLF.TDTT_DANH_SACH_SAO_KE_GN_ACTIVE;
                case "TDTT_DANH_SACH_SAO_KE_GN_PERIOD": return DanhSachBaoCaoBIDV_BLF.TDTT_DANH_SACH_SAO_KE_GN_PERIOD;
                case "HDVO_SO_TKCKH": return DanhSachBaoCaoBIDV_BLF.HDVO_SO_TKCKH;
                case "GDKT_PHIEU_CHI": return DanhSachBaoCaoBIDV_BLF.GDKT_PHIEU_CHI;
                case "GDKT_PHIEU_CHI_KH": return DanhSachBaoCaoBIDV_BLF.GDKT_PHIEU_CHI_KH;
                case "GDKT_PHIEU_THU": return DanhSachBaoCaoBIDV_BLF.GDKT_PHIEU_THU;
                case "GDKT_PHIEU_THU_KH": return DanhSachBaoCaoBIDV_BLF.GDKT_PHIEU_THU_KH;
                case "GDKT_PHIEU_KE_TOAN": return DanhSachBaoCaoBIDV_BLF.GDKT_PHIEU_KE_TOAN;
                case "GDKT_PHIEU_KE_TOAN_01": return DanhSachBaoCaoBIDV_BLF.GDKT_PHIEU_KE_TOAN_01;
                case "GDKT_PHIEU_KE_TOAN_GDICH": return DanhSachBaoCaoBIDV_BLF.GDKT_PHIEU_KE_TOAN_GDICH;
                case "KHTV_DANH_SACH_KHTV": return DanhSachBaoCaoBIDV_BLF.KHTV_DANH_SACH_KHTV;
                case "KHTV_BIEN_BAN_HOP_NHOM": return DanhSachBaoCaoBIDV_BLF.KHTV_BIEN_BAN_HOP_NHOM;
                case "BCTK_DAILY_REPORT": return DanhSachBaoCaoBIDV_BLF.BCTK_DAILY_REPORT;
                case "BCTK_MONTHLY_REPORT": return DanhSachBaoCaoBIDV_BLF.BCTK_MONTHLY_REPORT;
                case "BCTK_NGANH_NGHE_MMSC": return DanhSachBaoCaoBIDV_BLF.BCTK_NGANH_NGHE_MMSC;
                case "BCTK_THEO_DOANH_SO": return DanhSachBaoCaoBIDV_BLF.BCTK_THEO_DOANH_SO;
                case "BCTK_THEO_NGANH_NGHE": return DanhSachBaoCaoBIDV_BLF.BCTK_THEO_NGANH_NGHE;
                case "BCTK_QUAN_LY_NO": return DanhSachBaoCaoBIDV_BLF.BCTK_QUAN_LY_NO;
                case "GDKT_BALANCE_SHEET": return DanhSachBaoCaoBIDV_BLF.GDKT_BALANCE_SHEET;
                case "GDKT_PROFIT_AND_LOSS": return DanhSachBaoCaoBIDV_BLF.GDKT_PROFIT_AND_LOSS;
                case "BCTH_CREDIT_STATEMENT": return DanhSachBaoCaoBIDV_BLF.BCTH_CREDIT_STATEMENT;
                case "BCTH_BAO_CAO_PHAN_LOAI_NO": return DanhSachBaoCaoBIDV_BLF.BCTH_BAO_CAO_PHAN_LOAI_NO;
                case "BCTH_SAO_KE_DU_NO_TIN_DUNG_TKIEM": return DanhSachBaoCaoBIDV_BLF.BCTH_SAO_KE_DU_NO_TIN_DUNG_TKIEM;
                case "BCTH_BAO_CAO_DONG_TIEN": return DanhSachBaoCaoBIDV_BLF.BCTH_BAO_CAO_DONG_TIEN;
                case "TDTD_BANG_KHOACH_TRA_NO": return DanhSachBaoCaoBIDV_BLF.TDTD_BANG_KHOACH_TRA_NO;
                case "HDVO_GUI_TIEN_DS": return DanhSachBaoCaoBIDV_BLF.HDVO_GUI_TIEN_DS;
                case "HDVO_GUI_TIEN_DS_EXCEL": return DanhSachBaoCaoBIDV_BLF.HDVO_GUI_TIEN_DS_EXCEL;
                case "HDVO_DS_TIEN_GUI_DEN_HAN": return DanhSachBaoCaoBIDV_BLF.HDVO_DS_TIEN_GUI_DEN_HAN;
                case "GDKT_BANG_CAN_DOI_PHAT_SINH": return DanhSachBaoCaoBIDV_BLF.GDKT_BANG_CAN_DOI_PHAT_SINH;
                case "BCTC_BANG_CAN_DOI_KE_TOAN": return DanhSachBaoCaoBIDV_BLF.BCTC_BANG_CAN_DOI_KE_TOAN;
                case "BCTC_KET_QUA_HOAT_DONG": return DanhSachBaoCaoBIDV_BLF.BCTC_KET_QUA_HOAT_DONG;
                case "HDVO_DS_TK_TIEN_GUI_MO_MOI": return DanhSachBaoCaoBIDV_BLF.HDVO_DS_TK_TIEN_GUI_MO_MOI;
                case "HDVO_DS_TK_TIET_KIEM_DEN_HAN": return DanhSachBaoCaoBIDV_BLF.HDVO_DS_TK_TIET_KIEM_DEN_HAN;
                case "HDVO_DS_TK_TIET_KIEM_TT_TRONG_NGAY": return DanhSachBaoCaoBIDV_BLF.HDVO_DS_TK_TIET_KIEM_TT_TRONG_NGAY;
                case "HDVO_DS_TK_TIET_KIEM_TT_TRUOC_HAN": return DanhSachBaoCaoBIDV_BLF.HDVO_DS_TK_TIET_KIEM_TT_TRUOC_HAN;
                case "HDVO_DS_TK_TIET_KIEM_TT_DUNG_HAN": return DanhSachBaoCaoBIDV_BLF.HDVO_DS_TK_TIET_KIEM_TT_DUNG_HAN;
                case "HDVO_DS_SO_QUAY_VONG_TRONG_NGAY": return DanhSachBaoCaoBIDV_BLF.HDVO_DS_SO_QUAY_VONG_TRONG_NGAY;
                case "HDVO_HUY_DONG_VON_BINH_QUAN": return DanhSachBaoCaoBIDV_BLF.HDVO_HUY_DONG_VON_BINH_QUAN;
                case "KHTV_BC_KH_TAO_MOI": return DanhSachBaoCaoBIDV_BLF.KHTV_BC_KH_TAO_MOI;
                case "KHTV_BC_KH_THAY_DOI_TEN": return DanhSachBaoCaoBIDV_BLF.KHTV_BC_KH_THAY_DOI_TEN;
                case "KHTV_BC_KH_THAY_DOI_NGANH_KINH_TE": return DanhSachBaoCaoBIDV_BLF.KHTV_BC_KH_THAY_DOI_NGANH_KINH_TE;
                case "TDTD_BC_GIA_TRI_TSDB_HANG_NGAY": return DanhSachBaoCaoBIDV_BLF.TDTD_BC_GIA_TRI_TSDB_HANG_NGAY;
                default: return DanhSachBaoCaoBIDV_BLF.KHTV_QUYET_DINH_CONG_NHAN_TVIEN;
            }
        }

        public static bool LayGiaTriBaoCaoBIDV_BLF(string maBaoCao)
        {
            switch (maBaoCao)
            {
                case "TDVM_DON_VAY_VON": return true;
                case "TDVM_HOP_DONG_VAY_VON": return true;
                case "TDVM_KHE_UOC_VAY_VON": return true;
                case "TDVM_BANG_KHOACH_TRA_NO": return true;
                case "TDVM_KHE_UOC_NHAN_NO": return true;
                case "TDVM_PHU_LUC_HOP_DONG": return true;
                case "TDVM_BANG_KE_RUT_VON": return true;
                case "TDVM_PHIEU_KHAO_SAT_KHACH_HANG": return true;
                case "TDVM_BAO_CAO_DE_XUAT_TIN_DUNG": return true;
                case "TDVM_BAO_CAO_THAM_DINH_RUI_RO_TD": return true;
                case "TDVM_DS_PHAT_VON": return true;
                case "TDVM_SO_VON_VAY_TIET_KIEM_KHTV": return true;
                case "TDVM_DS_THU_TIEN_VON_TRA_DAN_TK": return true;
                case "TDTT_GIAY_DE_NGHI_VAY_VON_CA_NHAN": return true;
                case "TDTT_PHIEU_KHAO_SAT_KH_CA_NHAN": return true;
                case "TDTT_BAO_CAO_DE_XUAT_TIN_DUNG": return true;
                case "TDTT_BAO_CAO_THAM_DINH_RUI_RO_TD": return true;
                case "TDTT_HOP_DONG_TIN_DUNG": return true;
                case "TDTT_PHU_LUC_HOP_DONG": return true;
                case "TDTT_HOP_DONG_TIN_DUNG_BLANH_BEN_THU_BA": return true;
                case "TDTT_PHU_LUC_HOP_DONG_BLANH_BEN_THU_BA": return true;
                case "TDTT_KHE_UOC_RUT_VON": return true;
                case "TDTT_KHE_UOC_RUT_VON_BLANH_BEN_THU_BA": return true;
                case "TDTT_SAO_KE_TIN_DUNG_TH": return true;
                case "TDTT_SO_VON_VAY_TIET_KIEM_KHTV": return true;
                case "TDTT_DE_XUAT_PHE_DUYET_GIAI_NGAN": return true;
                case "TDTT_DANH_SACH_THU_NO": return true;
                case "TDTT_DANH_SACH_SAO_KE_GN_ACTIVE": return true;
                case "TDTT_DANH_SACH_SAO_KE_GN_PERIOD": return true;
                case "HDVO_SO_TKCKH": return true;
                case "GDKT_PHIEU_CHI": return true;
                case "GDKT_PHIEU_CHI_KH": return true;
                case "GDKT_PHIEU_THU": return true;
                case "GDKT_PHIEU_THU_KH": return true;
                case "GDKT_PHIEU_KE_TOAN": return true;
                case "GDKT_PHIEU_KE_TOAN_01": return true;
                case "GDKT_PHIEU_KE_TOAN_GDICH": return true;
                case "KHTV_DANH_SACH_KHTV": return true;
                case "KHTV_BIEN_BAN_HOP_NHOM": return true;
                case "BCTK_DAILY_REPORT": return true;
                case "BCTK_MONTHLY_REPORT": return true;
                case "BCTK_NGANH_NGHE_MMSC": return true;
                case "BCTK_THEO_DOANH_SO": return true;
                case "BCTK_THEO_NGANH_NGHE": return true;
                case "BCTK_QUAN_LY_NO": return true;
                case "GDKT_BALANCE_SHEET": return true;
                case "GDKT_PROFIT_AND_LOSS": return true;
                case "BCTH_CREDIT_STATEMENT": return true;
                case "BCTH_BAO_CAO_PHAN_LOAI_NO": return true;
                case "BCTH_SAO_KE_DU_NO_TIN_DUNG_TKIEM": return true;
                case "BCTH_BAO_CAO_DONG_TIEN": return true;
                case "TDTD_BANG_KHOACH_TRA_NO": return true;
                case "HDVO_GUI_TIEN_DS": return true;
                case "HDVO_GUI_TIEN_DS_EXCEL": return true;
                case "HDVO_DS_TIEN_GUI_DEN_HAN": return true;
                case "GDKT_BANG_CAN_DOI_PHAT_SINH": return true;
                case "BCTC_BANG_CAN_DOI_KE_TOAN": return true;
                case "BCTC_KET_QUA_HOAT_DONG": return true;
                case "HDVO_DS_TK_TIEN_GUI_MO_MOI": return true;
                case "HDVO_DS_TK_TIET_KIEM_DEN_HAN": return true;
                case "HDVO_DS_TK_TIET_KIEM_TT_TRONG_NGAY": return true;
                case "HDVO_DS_TK_TIET_KIEM_TT_TRUOC_HAN": return true;
                case "HDVO_DS_TK_TIET_KIEM_TT_DUNG_HAN": return true;
                case "HDVO_DS_SO_QUAY_VONG_TRONG_NGAY": return true;
                case "HDVO_HUY_DONG_VON_BINH_QUAN": return true;
                case "KHTV_BC_KH_TAO_MOI": return true;
                case "KHTV_BC_KH_THAY_DOI_TEN": return true;
                case "KHTV_BC_KH_THAY_DOI_NGANH_KINH_TE": return true;
                case "TDTD_BC_GIA_TRI_TSDB_HANG_NGAY": return true;
                default: return false;
            }
        }

        public enum DanhSachBaoCaoHVNH
        {
            KHTV_DANH_SACH_KHTV,
            KHTV_DANH_SACH_KHACH_HANG_RA_KHOI_CHUONG_TRINH,
            KHTV_DANH_GIA_XEP_HANG_NGHEO,
            KHTV_QUYET_DINH_CONG_NHAN_THANH_VIEN,
            KHTV_THAM_DINH_KHACH_HANG,
            KHTV_HO_SO_KHACH_HANG_THAM_DINH_THONG_TIN,
            KHTV_CHUYEN_DIA_BAN,
            GDKT_PHIEU_THU,
            GDKT_PHIEU_CHI,
            GDKT_PHIEU_CHUYEN_KHOAN,
            GDKT_NX_NGOAI_BANG,
            GDKT_SO_CHI_TIET_TAI_KHOAN,
            GDKT_SO_NHAT_KY_CHUNG,
            GDKT_SO_QUY_TIEN_MAT,
            GDKT_SO_NHAT_KY_CHI,
            GDKT_SO_NHAT_KY_THU,
            HDVO_SAO_KE_TIEN_GUI_TK,
            HDVO_BANG_XAC_NHAN_SO_DU_TIEN_GUI_TK,
            HDVO_SO_PHU_TIEN_GUI,
            HDVO_SO_TIET_KIEM,
            HDVO_GIAY_GUI_TIEN,
            HDVO_BANG_KE_LAI_NHAP_GOC,
            HDVO_BANG_KE_TINH_LAI_DU_CHI_TIEN_GUI_TK,
            HDVO_LAI_SUAT_BINH_QUAN,
            TDVM_DANH_GIAI_XEP_HANG_KHACH_HANG_CA_NHAN,
            TDVM_SAO_KE_VON_NGAN_HAN,
            TDVM_SAO_KE_VON_TRUNG_HAN,
            TDVM_SAO_KE_VON_DAI_HAN,
            TDVM_BAO_CAO_PHAN_LOAI_NO,
            TDVM_BAO_CAO_CHAM_TRA,
            TDVM_DON_VAY_VON,
            TDVM_HOP_DONG_VAY_VON,
            TDVM_PHU_LUC_HOP_DONG,
            TDVM_DS_PHAT_VON,
            TDVM_DS_DE_NGHI_VAY_VON,
            TDVM_DS_THU_TIEN_VON_TRA_DAN_TK,
            TDVM_DS_THU_TIEN_PHAT_HANH,
            TDVM_MAU_BIEU_XEP_HANG_TIN_DUNG_CA_NHAN,    
            TDVM_THAM_DINH_TIN_DUNG,
            BCTH_BAO_CAO_VAN_HANH,
            BCTH_BAO_CAO_THONG_KE_TONG_HOP,
            BCTH_FACTSHEET,
            BCTK_DU_NO_TD_THEO_NGANH,
            BCTK_DU_NO_TD_THEO_LH_TOCHUC_CANHAN,
            BCTK_HOAT_DONG_HDV,
            BCTK_BC_PLOAI_NO_DP_RUIRO,
            BCTK_BC_XULY_RUIRO_CHOVAY,
            BCTK_BC_TYLE_DAM_BAO_ANTOAN,
            BCTK_BC_KHANG_CHOVAY_VUOT_QDINH,
            BCTC_BANG_CAN_DOI_KE_TOAN,
            BCTC_BANG_CAN_DOI_TKKT,
            BCTC_BAO_CAO_KET_QUA_HOAT_DONG_KINH_DOANH,
            BCTC_LUU_CHUYEN_TIEN_TE,
            BCTC_THUYET_MINH_TAI_CHINH,
            CICR_BAO_CAO_THONG_TIN_TSDB_KH,
            CICR_BAO_CAO_THONG_TIN_TD,
            BHTG_BANG_KE_SO_TIEN_GUI_DTUONG_BH,
            BHTG_PHAN_LOAI_TS_CO_RUI_RO,
            BHTG_BANG_TINH_TOAN_NOP_PHI_BH_TIEN_GUI
        }

        public static string LayMaBaoCaoHVNH(DanhSachBaoCaoHVNH dsbaocao)
        {
            switch (dsbaocao)
            {
                case DanhSachBaoCaoHVNH.KHTV_DANH_SACH_KHTV: return "KHTV_DANH_SACH_KHTV";
                case DanhSachBaoCaoHVNH.KHTV_DANH_SACH_KHACH_HANG_RA_KHOI_CHUONG_TRINH: return "KHTV_DANH_SACH_KHACH_HANG_RA_KHOI_CHUONG_TRINH";
                case DanhSachBaoCaoHVNH.KHTV_DANH_GIA_XEP_HANG_NGHEO: return "KHTV_DANH_GIA_XEP_HANG_NGHEO";
                case DanhSachBaoCaoHVNH.KHTV_QUYET_DINH_CONG_NHAN_THANH_VIEN: return "KHTV_QUYET_DINH_CONG_NHAN_THANH_VIEN";
                case DanhSachBaoCaoHVNH.KHTV_THAM_DINH_KHACH_HANG: return "KHTV_THAM_DINH_KHACH_HANG";
                case DanhSachBaoCaoHVNH.KHTV_HO_SO_KHACH_HANG_THAM_DINH_THONG_TIN: return "KHTV_HO_SO_KHACH_HANG_THAM_DINH_THONG_TIN";
                case DanhSachBaoCaoHVNH.KHTV_CHUYEN_DIA_BAN: return "KHTV_CHUYEN_DIA_BAN";                    
                case DanhSachBaoCaoHVNH.GDKT_PHIEU_THU: return "GDKT_PHIEU_THU";
                case DanhSachBaoCaoHVNH.GDKT_PHIEU_CHI: return "GDKT_PHIEU_CHI";
                case DanhSachBaoCaoHVNH.GDKT_PHIEU_CHUYEN_KHOAN: return "GDKT_PHIEU_CHUYEN_KHOAN";
                case DanhSachBaoCaoHVNH.GDKT_NX_NGOAI_BANG: return "GDKT_NX_NGOAI_BANG";
                case DanhSachBaoCaoHVNH.GDKT_SO_CHI_TIET_TAI_KHOAN: return "GDKT_SO_CHI_TIET_TAI_KHOAN";
                case DanhSachBaoCaoHVNH.GDKT_SO_NHAT_KY_CHUNG: return "GDKT_SO_NHAT_KY_CHUNG";
                case DanhSachBaoCaoHVNH.GDKT_SO_QUY_TIEN_MAT: return "GDKT_SO_QUY_TIEN_MAT";
                case DanhSachBaoCaoHVNH.GDKT_SO_NHAT_KY_CHI: return "GDKT_SO_NHAT_KY_CHI";
                case DanhSachBaoCaoHVNH.GDKT_SO_NHAT_KY_THU: return "GDKT_SO_NHAT_KY_THU";
                case DanhSachBaoCaoHVNH.HDVO_SAO_KE_TIEN_GUI_TK: return "HDVO_SAO_KE_TIEN_GUI_TK";
                case DanhSachBaoCaoHVNH.HDVO_BANG_XAC_NHAN_SO_DU_TIEN_GUI_TK: return "HDVO_BANG_XAC_NHAN_SO_DU_TIEN_GUI_TK";
                case DanhSachBaoCaoHVNH.HDVO_SO_PHU_TIEN_GUI: return "HDVO_SO_PHU_TIEN_GUI";
                case DanhSachBaoCaoHVNH.HDVO_SO_TIET_KIEM: return "HDVO_SO_TIET_KIEM";
                case DanhSachBaoCaoHVNH.HDVO_GIAY_GUI_TIEN: return "HDVO_GIAY_GUI_TIEN";
                case DanhSachBaoCaoHVNH.HDVO_BANG_KE_LAI_NHAP_GOC: return "HDVO_BANG_KE_LAI_NHAP_GOC";
                case DanhSachBaoCaoHVNH.HDVO_BANG_KE_TINH_LAI_DU_CHI_TIEN_GUI_TK: return "HDVO_BANG_KE_TINH_LAI_DU_CHI_TIEN_GUI_TK";
                case DanhSachBaoCaoHVNH.HDVO_LAI_SUAT_BINH_QUAN: return "HDVO_LAI_SUAT_BINH_QUAN";                    
                case DanhSachBaoCaoHVNH.TDVM_DANH_GIAI_XEP_HANG_KHACH_HANG_CA_NHAN: return "TDVM_DANH_GIAI_XEP_HANG_KHACH_HANG_CA_NHAN";
                case DanhSachBaoCaoHVNH.TDVM_SAO_KE_VON_NGAN_HAN: return "TDVM_SAO_KE_VON_NGAN_HAN";
                case DanhSachBaoCaoHVNH.TDVM_SAO_KE_VON_TRUNG_HAN: return "TDVM_SAO_KE_VON_TRUNG_HAN";
                case DanhSachBaoCaoHVNH.TDVM_SAO_KE_VON_DAI_HAN: return "TDVM_SAO_KE_VON_DAI_HAN";
                case DanhSachBaoCaoHVNH.TDVM_BAO_CAO_PHAN_LOAI_NO: return "TDVM_BAO_CAO_PHAN_LOAI_NO";
                case DanhSachBaoCaoHVNH.TDVM_BAO_CAO_CHAM_TRA: return "TDVM_BAO_CAO_CHAM_TRA";
                case DanhSachBaoCaoHVNH.TDVM_DON_VAY_VON: return "TDVM_DON_VAY_VON";
                case DanhSachBaoCaoHVNH.TDVM_HOP_DONG_VAY_VON: return "TDVM_HOP_DONG_VAY_VON";
                case DanhSachBaoCaoHVNH.TDVM_PHU_LUC_HOP_DONG: return "TDVM_PHU_LUC_HOP_DONG";
                case DanhSachBaoCaoHVNH.TDVM_DS_PHAT_VON: return "TDVM_DS_PHAT_VON";
                case DanhSachBaoCaoHVNH.TDVM_DS_DE_NGHI_VAY_VON: return "TDVM_DS_DE_NGHI_VAY_VON";
                case DanhSachBaoCaoHVNH.TDVM_DS_THU_TIEN_VON_TRA_DAN_TK: return "TDVM_DS_THU_TIEN_VON_TRA_DAN_TK";
                case DanhSachBaoCaoHVNH.TDVM_DS_THU_TIEN_PHAT_HANH: return "TDVM_DS_THU_TIEN_PHAT_HANH";
                case DanhSachBaoCaoHVNH.TDVM_MAU_BIEU_XEP_HANG_TIN_DUNG_CA_NHAN: return "TDVM_MAU_BIEU_XEP_HANG_TIN_DUNG_CA_NHAN";
                case DanhSachBaoCaoHVNH.TDVM_THAM_DINH_TIN_DUNG: return "TDVM_THAM_DINH_TIN_DUNG";                                    
                case DanhSachBaoCaoHVNH.BCTH_BAO_CAO_VAN_HANH: return "BCTH_BAO_CAO_VAN_HANH";
                case DanhSachBaoCaoHVNH.BCTH_BAO_CAO_THONG_KE_TONG_HOP: return "BCTH_BAO_CAO_THONG_KE_TONG_HOP";
                case DanhSachBaoCaoHVNH.BCTH_FACTSHEET: return "BCTH_FACTSHEET";
                case DanhSachBaoCaoHVNH.BCTK_DU_NO_TD_THEO_NGANH: return "BCTK_DU_NO_TD_THEO_NGANH";
                case DanhSachBaoCaoHVNH.BCTK_DU_NO_TD_THEO_LH_TOCHUC_CANHAN: return "BCTK_DU_NO_TD_THEO_LH_TOCHUC_CANHAN";
                case DanhSachBaoCaoHVNH.BCTK_HOAT_DONG_HDV: return "BCTK_HOAT_DONG_HDV";
                case DanhSachBaoCaoHVNH.BCTK_BC_PLOAI_NO_DP_RUIRO: return "BCTK_BC_PLOAI_NO_DP_RUIRO";
                case DanhSachBaoCaoHVNH.BCTK_BC_XULY_RUIRO_CHOVAY: return "BCTK_BC_XULY_RUIRO_CHOVAY";
                case DanhSachBaoCaoHVNH.BCTK_BC_TYLE_DAM_BAO_ANTOAN: return "BCTK_BC_TYLE_DAM_BAO_ANTOAN";
                case DanhSachBaoCaoHVNH.BCTK_BC_KHANG_CHOVAY_VUOT_QDINH: return "BCTK_BC_KHANG_CHOVAY_VUOT_QDINH";
                case DanhSachBaoCaoHVNH.BCTC_BANG_CAN_DOI_KE_TOAN: return "BCTC_BANG_CAN_DOI_KE_TOAN";
                case DanhSachBaoCaoHVNH.BCTC_BANG_CAN_DOI_TKKT: return "BCTC_BANG_CAN_DOI_TKKT";
                case DanhSachBaoCaoHVNH.BCTC_BAO_CAO_KET_QUA_HOAT_DONG_KINH_DOANH: return "BCTC_BAO_CAO_KET_QUA_HOAT_DONG_KINH_DOANH";
                case DanhSachBaoCaoHVNH.BCTC_LUU_CHUYEN_TIEN_TE: return "BCTC_LUU_CHUYEN_TIEN_TE";
                case DanhSachBaoCaoHVNH.BCTC_THUYET_MINH_TAI_CHINH: return "BCTC_THUYET_MINH_TAI_CHINH";
                case DanhSachBaoCaoHVNH.CICR_BAO_CAO_THONG_TIN_TSDB_KH: return "CICR_BAO_CAO_THONG_TIN_TSDB_KH";
                case DanhSachBaoCaoHVNH.CICR_BAO_CAO_THONG_TIN_TD: return "CICR_BAO_CAO_THONG_TIN_TD";
                case DanhSachBaoCaoHVNH.BHTG_BANG_KE_SO_TIEN_GUI_DTUONG_BH: return "BHTG_BANG_KE_SO_TIEN_GUI_DTUONG_BH";
                case DanhSachBaoCaoHVNH.BHTG_PHAN_LOAI_TS_CO_RUI_RO: return "BHTG_PHAN_LOAI_TS_CO_RUI_RO";
                case DanhSachBaoCaoHVNH.BHTG_BANG_TINH_TOAN_NOP_PHI_BH_TIEN_GUI: return "BHTG_BANG_TINH_TOAN_NOP_PHI_BH_TIEN_GUI";
                default: return "";
            }
        }


        public static DanhSachBaoCaoHVNH layBaoCaoHVNHTheoMa(string maBaoCao)
        {
            switch (maBaoCao)
            {
                case "KHTV_DANH_SACH_KHTV": return DanhSachBaoCaoHVNH.KHTV_DANH_SACH_KHTV;
                case "KHTV_DANH_SACH_KHACH_HANG_RA_KHOI_CHUONG_TRINH": return DanhSachBaoCaoHVNH.KHTV_DANH_SACH_KHACH_HANG_RA_KHOI_CHUONG_TRINH;
                case "KHTV_DANH_GIA_XEP_HANG_NGHEO": return DanhSachBaoCaoHVNH.KHTV_DANH_GIA_XEP_HANG_NGHEO;
                case "KHTV_QUYET_DINH_CONG_NHAN_THANH_VIEN": return DanhSachBaoCaoHVNH.KHTV_QUYET_DINH_CONG_NHAN_THANH_VIEN;
                case "KHTV_THAM_DINH_KHACH_HANG": return DanhSachBaoCaoHVNH.KHTV_THAM_DINH_KHACH_HANG;
                case "KHTV_HO_SO_KHACH_HANG_THAM_DINH_THONG_TIN": return DanhSachBaoCaoHVNH.KHTV_HO_SO_KHACH_HANG_THAM_DINH_THONG_TIN;
                case "KHTV_CHUYEN_DIA_BAN": return DanhSachBaoCaoHVNH.KHTV_CHUYEN_DIA_BAN;                    
                case "GDKT_PHIEU_THU": return DanhSachBaoCaoHVNH.GDKT_PHIEU_THU;
                case "GDKT_PHIEU_CHI": return DanhSachBaoCaoHVNH.GDKT_PHIEU_CHI;
                case "GDKT_PHIEU_CHUYEN_KHOAN": return DanhSachBaoCaoHVNH.GDKT_PHIEU_CHUYEN_KHOAN;
                case "GDKT_NX_NGOAI_BANG": return DanhSachBaoCaoHVNH.GDKT_NX_NGOAI_BANG;
                case "GDKT_SO_CHI_TIET_TAI_KHOAN": return DanhSachBaoCaoHVNH.GDKT_SO_CHI_TIET_TAI_KHOAN;
                case "GDKT_SO_NHAT_KY_CHUNG": return DanhSachBaoCaoHVNH.GDKT_SO_NHAT_KY_CHUNG;
                case "GDKT_SO_QUY_TIEN_MAT": return DanhSachBaoCaoHVNH.GDKT_SO_QUY_TIEN_MAT;
                case "GDKT_SO_NHAT_KY_CHI": return DanhSachBaoCaoHVNH.GDKT_SO_NHAT_KY_CHI;
                case "GDKT_SO_NHAT_KY_THU": return DanhSachBaoCaoHVNH.GDKT_SO_NHAT_KY_THU;
                case "HDVO_SAO_KE_TIEN_GUI_TK": return DanhSachBaoCaoHVNH.HDVO_SAO_KE_TIEN_GUI_TK;
                case "HDVO_BANG_XAC_NHAN_SO_DU_TIEN_GUI_TK": return DanhSachBaoCaoHVNH.HDVO_BANG_XAC_NHAN_SO_DU_TIEN_GUI_TK;
                case "HDVO_SO_PHU_TIEN_GUI": return DanhSachBaoCaoHVNH.HDVO_SO_PHU_TIEN_GUI;
                case "HDVO_SO_TIET_KIEM": return DanhSachBaoCaoHVNH.HDVO_SO_TIET_KIEM;
                case "HDVO_GIAY_GUI_TIEN": return DanhSachBaoCaoHVNH.HDVO_GIAY_GUI_TIEN;
                case "HDVO_BANG_KE_LAI_NHAP_GOC": return DanhSachBaoCaoHVNH.HDVO_BANG_KE_LAI_NHAP_GOC;
                case "HDVO_BANG_KE_TINH_LAI_DU_CHI_TIEN_GUI_TK": return DanhSachBaoCaoHVNH.HDVO_BANG_KE_TINH_LAI_DU_CHI_TIEN_GUI_TK;
                case "HDVO_LAI_SUAT_BINH_QUAN": return DanhSachBaoCaoHVNH.HDVO_LAI_SUAT_BINH_QUAN;                    
                case "TDVM_DANH_GIAI_XEP_HANG_KHACH_HANG_CA_NHAN": return DanhSachBaoCaoHVNH.TDVM_DANH_GIAI_XEP_HANG_KHACH_HANG_CA_NHAN;
                case "TDVM_SAO_KE_VON_NGAN_HAN": return DanhSachBaoCaoHVNH.TDVM_SAO_KE_VON_NGAN_HAN;
                case "TDVM_SAO_KE_VON_TRUNG_HAN": return DanhSachBaoCaoHVNH.TDVM_SAO_KE_VON_TRUNG_HAN;
                case "TDVM_SAO_KE_VON_DAI_HAN": return DanhSachBaoCaoHVNH.TDVM_SAO_KE_VON_DAI_HAN;
                case "TDVM_BAO_CAO_PHAN_LOAI_NO": return DanhSachBaoCaoHVNH.TDVM_BAO_CAO_PHAN_LOAI_NO;
                case "TDVM_BAO_CAO_CHAM_TRA": return DanhSachBaoCaoHVNH.TDVM_BAO_CAO_CHAM_TRA;
                case "TDVM_DON_VAY_VON": return DanhSachBaoCaoHVNH.TDVM_DON_VAY_VON;
                case "TDVM_HOP_DONG_VAY_VON": return DanhSachBaoCaoHVNH.TDVM_HOP_DONG_VAY_VON;
                case "TDVM_PHU_LUC_HOP_DONG": return DanhSachBaoCaoHVNH.TDVM_PHU_LUC_HOP_DONG;
                case "TDVM_DS_PHAT_VON": return DanhSachBaoCaoHVNH.TDVM_DS_PHAT_VON;
                case "TDVM_DS_DE_NGHI_VAY_VON": return DanhSachBaoCaoHVNH.TDVM_DS_DE_NGHI_VAY_VON;
                case "TDVM_DS_THU_TIEN_VON_TRA_DAN_TK": return DanhSachBaoCaoHVNH.TDVM_DS_THU_TIEN_VON_TRA_DAN_TK;
                case "TDVM_DS_THU_TIEN_PHAT_HANH": return DanhSachBaoCaoHVNH.TDVM_DS_THU_TIEN_PHAT_HANH;
                case "TDVM_MAU_BIEU_XEP_HANG_TIN_DUNG_CA_NHAN": return DanhSachBaoCaoHVNH.TDVM_MAU_BIEU_XEP_HANG_TIN_DUNG_CA_NHAN;
                case "TDVM_THAM_DINH_TIN_DUNG": return DanhSachBaoCaoHVNH.TDVM_THAM_DINH_TIN_DUNG;                                                 
                case "BCTH_BAO_CAO_VAN_HANH": return DanhSachBaoCaoHVNH.BCTH_BAO_CAO_VAN_HANH;
                case "BCTH_BAO_CAO_THONG_KE_TONG_HOP": return DanhSachBaoCaoHVNH.BCTH_BAO_CAO_THONG_KE_TONG_HOP;
                case "BCTH_FACTSHEET": return DanhSachBaoCaoHVNH.BCTH_FACTSHEET;
                case "BCTK_DU_NO_TD_THEO_NGANH": return DanhSachBaoCaoHVNH.BCTK_DU_NO_TD_THEO_NGANH;
                case "BCTK_DU_NO_TD_THEO_LH_TOCHUC_CANHAN": return DanhSachBaoCaoHVNH.BCTK_DU_NO_TD_THEO_LH_TOCHUC_CANHAN;
                case "BCTK_HOAT_DONG_HDV": return DanhSachBaoCaoHVNH.BCTK_HOAT_DONG_HDV;
                case "BCTK_BC_PLOAI_NO_DP_RUIRO": return DanhSachBaoCaoHVNH.BCTK_BC_PLOAI_NO_DP_RUIRO;
                case "BCTK_BC_XULY_RUIRO_CHOVAY": return DanhSachBaoCaoHVNH.BCTK_BC_XULY_RUIRO_CHOVAY;
                case "BCTK_BC_TYLE_DAM_BAO_ANTOAN": return DanhSachBaoCaoHVNH.BCTK_BC_TYLE_DAM_BAO_ANTOAN;
                case "BCTK_BC_KHANG_CHOVAY_VUOT_QDINH": return DanhSachBaoCaoHVNH.BCTK_BC_KHANG_CHOVAY_VUOT_QDINH;
                case "BCTC_BANG_CAN_DOI_KE_TOAN": return DanhSachBaoCaoHVNH.BCTC_BANG_CAN_DOI_KE_TOAN;
                case "BCTC_BANG_CAN_DOI_TKKT": return DanhSachBaoCaoHVNH.BCTC_BANG_CAN_DOI_TKKT;
                case "BCTC_BAO_CAO_KET_QUA_HOAT_DONG_KINH_DOANH": return DanhSachBaoCaoHVNH.BCTC_BAO_CAO_KET_QUA_HOAT_DONG_KINH_DOANH;
                case "BCTC_LUU_CHUYEN_TIEN_TE": return DanhSachBaoCaoHVNH.BCTC_LUU_CHUYEN_TIEN_TE;
                case "BCTC_THUYET_MINH_TAI_CHINH": return DanhSachBaoCaoHVNH.BCTC_THUYET_MINH_TAI_CHINH;
                case "CICR_BAO_CAO_THONG_TIN_TSDB_KH": return DanhSachBaoCaoHVNH.CICR_BAO_CAO_THONG_TIN_TSDB_KH;
                case "CICR_BAO_CAO_THONG_TIN_TD": return DanhSachBaoCaoHVNH.CICR_BAO_CAO_THONG_TIN_TD;
                case "BHTG_BANG_KE_SO_TIEN_GUI_DTUONG_BH": return DanhSachBaoCaoHVNH.BHTG_BANG_KE_SO_TIEN_GUI_DTUONG_BH;
                case "BHTG_PHAN_LOAI_TS_CO_RUI_RO": return DanhSachBaoCaoHVNH.BHTG_PHAN_LOAI_TS_CO_RUI_RO;
                case "BHTG_BANG_TINH_TOAN_NOP_PHI_BH_TIEN_GUI": return DanhSachBaoCaoHVNH.BHTG_BANG_TINH_TOAN_NOP_PHI_BH_TIEN_GUI;
                default: return DanhSachBaoCaoHVNH.KHTV_DANH_SACH_KHTV;
            }
        }

        public enum DanhSachBaoCaoQuangBinh
        {
            KHTV_QUYET_DINH_CONG_NHAN_TVIEN,
            KHTV_BAN_KHAO_SAT_HOAN_CANH_KTXH,
            KHTV_DK_THAM_GIA_THANH_VIEN_QUY,
            KHTV_HOP_DONG_BAO_LANH_NHOM,
            TDVM_BANG_THEO_DOI_NO_QUA_HAN,
            TDVM_DON_XIN_VAY_VON_VAY_KHAC, 
            TDVM_DON_XIN_VAY_VON_TRA_DAN_VA_BO_SUNG,
            TDVM_HOP_DONG_VAY_VON_VAY_KHAC,
            TDVM_HOP_DONG_VAY_VON_TRA_DAN_VA_BO_SUNG,
            TDVM_DS_PHAT_VON,
            TDVM_SO_TIN_DUNG_CAP_CUM,
            TDVM_SO_TIN_DUNG_CAP_XA,
            TDVM_DS_DE_NGHI_VAY_VON,
            TDVM_DS_THU_LAI_VON_MUA_VU,
            TDVM_DS_THU_TIEN_VON_TRA_DAN_TK,
            TDVM_SO_THEO_DOI_KHTV,
            TDVM_SO_THEO_DOI_NO_RUI_RO,
            TDVM_SO_THEO_DOI_VON_VAY,
            TDVM_BAO_CAO_KHACH_HANG_CHAM_TRA,
            HDVO_DANH_SACH_HOAN_TK,
            HDVO_DS_HUY_DONG_VON_CAP_CUM_NHOM,
            HDVO_DS_HUY_DONG_VON_CAP_XA,
            BCTC_BANG_CAN_DOI_TKKT_THU,
            BCTC_BANG_CAN_DOI_KE_TOAN,
            BCTC_LUU_CHUYEN_TIEN_TE,
            BCTC_THUYET_MINH_TAI_CHINH,
            BCTH_HOAT_DONG,
            BCTH_HOAT_DONG_TCVM,
            BCTH_HOAT_DONG_CBTD_SP,
            BCTH_HOAT_DONG_DB,
            BCTH_HOAT_DONG_CBTD,
            BCTC_KET_QUA_HOAT_DONG,
            BCTH_BC_TIEN_DO_THEO_NGUON_TOAN_HE_THONG,
            GDKT_BAO_CAO_CHI_PHI_THEO_NGUON,
            GDKT_BAO_CAO_THU_NHAP_THEO_NGUON,
            TDVM_BAO_CAO_KHACH_HANG_TRA_TRUOC,
            BCTH_BAO_CAO_TIN_DUNG_THEO_XA,
            BCTH_BAO_CAO_TIN_DUNG_THEO_CBTD,
            BCTH_BAO_CAO_TIEN_DO_TOAN_TINH,
            BCTH_BAO_CAO_TIEN_DO_TONG_HOP_TOAN_TINH,
            BCTH_BAO_CAO_THEO_NGUON_CUA_TINH,
            BCTH_BAO_CAO_TIN_DUNG_CUA_CHI_NHANH,
            BCTH_BAO_CAO_TIN_DUNG_THEO_NGUON_CUA_CHI_NHANH
        }

        public static string LayMaBaoCaoQuangBinh(DanhSachBaoCaoQuangBinh dsbaocao)
        {
            switch (dsbaocao)
            {
                case DanhSachBaoCaoQuangBinh.KHTV_QUYET_DINH_CONG_NHAN_TVIEN: return "KHTV_QUYET_DINH_CONG_NHAN_TVIEN";
                case DanhSachBaoCaoQuangBinh.KHTV_BAN_KHAO_SAT_HOAN_CANH_KTXH: return "KHTV_BAN_KHAO_SAT_HOAN_CANH_KTXH";
                case DanhSachBaoCaoQuangBinh.KHTV_DK_THAM_GIA_THANH_VIEN_QUY: return "KHTV_DK_THAM_GIA_THANH_VIEN_QUY";
                case DanhSachBaoCaoQuangBinh.KHTV_HOP_DONG_BAO_LANH_NHOM: return "KHTV_HOP_DONG_BAO_LANH_NHOM";
                case DanhSachBaoCaoQuangBinh.TDVM_BANG_THEO_DOI_NO_QUA_HAN: return "TDVM_BANG_THEO_DOI_NO_QUA_HAN";
                case DanhSachBaoCaoQuangBinh.TDVM_DON_XIN_VAY_VON_VAY_KHAC: return "TDVM_DON_XIN_VAY_VON_VAY_KHAC";
                case DanhSachBaoCaoQuangBinh.TDVM_DON_XIN_VAY_VON_TRA_DAN_VA_BO_SUNG: return "TDVM_DON_XIN_VAY_VON_TRA_DAN_VA_BO_SUNG";
                case DanhSachBaoCaoQuangBinh.TDVM_HOP_DONG_VAY_VON_VAY_KHAC: return "TDVM_HOP_DONG_VAY_VON_VAY_KHAC";
                case DanhSachBaoCaoQuangBinh.TDVM_HOP_DONG_VAY_VON_TRA_DAN_VA_BO_SUNG: return "TDVM_HOP_DONG_VAY_VON_TRA_DAN_VA_BO_SUNG";
                case DanhSachBaoCaoQuangBinh.TDVM_DS_PHAT_VON: return "TDVM_DS_PHAT_VON";
                case DanhSachBaoCaoQuangBinh.TDVM_DS_DE_NGHI_VAY_VON: return "TDVM_DS_DE_NGHI_VAY_VON";
                case DanhSachBaoCaoQuangBinh.TDVM_DS_THU_LAI_VON_MUA_VU: return "TDVM_DS_THU_LAI_VON_MUA_VU";
                case DanhSachBaoCaoQuangBinh.TDVM_DS_THU_TIEN_VON_TRA_DAN_TK: return "TDVM_DS_THU_TIEN_VON_TRA_DAN_TK";
                case DanhSachBaoCaoQuangBinh.TDVM_SO_THEO_DOI_KHTV: return "TDVM_SO_THEO_DOI_KHTV";
                case DanhSachBaoCaoQuangBinh.TDVM_SO_THEO_DOI_NO_RUI_RO: return "TDVM_SO_THEO_DOI_NO_RUI_RO";
                case DanhSachBaoCaoQuangBinh.TDVM_SO_THEO_DOI_VON_VAY: return "TDVM_SO_THEO_DOI_VON_VAY";
                case DanhSachBaoCaoQuangBinh.TDVM_SO_TIN_DUNG_CAP_CUM: return "TDVM_SO_TIN_DUNG_CAP_CUM";
                case DanhSachBaoCaoQuangBinh.TDVM_SO_TIN_DUNG_CAP_XA: return "TDVM_SO_TIN_DUNG_CAP_XA"; 
                case DanhSachBaoCaoQuangBinh.TDVM_BAO_CAO_KHACH_HANG_CHAM_TRA: return "TDVM_BAO_CAO_KHACH_HANG_CHAM_TRA";
                case DanhSachBaoCaoQuangBinh.HDVO_DANH_SACH_HOAN_TK: return "HDVO_DANH_SACH_HOAN_TK";
                case DanhSachBaoCaoQuangBinh.HDVO_DS_HUY_DONG_VON_CAP_CUM_NHOM: return "HDVO_DS_HUY_DONG_VON_CAP_CUM_NHOM";
                case DanhSachBaoCaoQuangBinh.HDVO_DS_HUY_DONG_VON_CAP_XA: return "HDVO_DS_HUY_DONG_VON_CAP_XA";
                case DanhSachBaoCaoQuangBinh.BCTC_BANG_CAN_DOI_TKKT_THU: return "BCTC_BANG_CAN_DOI_TKKT_THU";
                case DanhSachBaoCaoQuangBinh.BCTC_BANG_CAN_DOI_KE_TOAN: return "BCTC_BANG_CAN_DOI_KE_TOAN";
                case DanhSachBaoCaoQuangBinh.BCTC_LUU_CHUYEN_TIEN_TE: return "BCTC_LUU_CHUYEN_TIEN_TE";
                case DanhSachBaoCaoQuangBinh.BCTC_THUYET_MINH_TAI_CHINH: return "BCTC_THUYET_MINH_TAI_CHINH";
                case DanhSachBaoCaoQuangBinh.BCTH_HOAT_DONG: return "BCTH_HOAT_DONG";
                case DanhSachBaoCaoQuangBinh.BCTH_HOAT_DONG_CBTD: return "BCTH_HOAT_DONG_CBTD";
                case DanhSachBaoCaoQuangBinh.BCTH_HOAT_DONG_CBTD_SP: return "BCTH_HOAT_DONG_CBTD_SP";
                case DanhSachBaoCaoQuangBinh.BCTH_HOAT_DONG_TCVM: return "BCTH_HOAT_DONG_TCVM";
                case DanhSachBaoCaoQuangBinh.BCTH_HOAT_DONG_DB: return "BCTH_HOAT_DONG_DB";
                case DanhSachBaoCaoQuangBinh.BCTC_KET_QUA_HOAT_DONG: return "BCTC_KET_QUA_HOAT_DONG";
                case DanhSachBaoCaoQuangBinh.BCTH_BC_TIEN_DO_THEO_NGUON_TOAN_HE_THONG: return "BCTH_BC_TIEN_DO_THEO_NGUON_TOAN_HE_THONG";
                case DanhSachBaoCaoQuangBinh.GDKT_BAO_CAO_CHI_PHI_THEO_NGUON: return "GDKT_BAO_CAO_CHI_PHI_THEO_NGUON";
                case DanhSachBaoCaoQuangBinh.GDKT_BAO_CAO_THU_NHAP_THEO_NGUON: return "GDKT_BAO_CAO_THU_NHAP_THEO_NGUON";
                case DanhSachBaoCaoQuangBinh.TDVM_BAO_CAO_KHACH_HANG_TRA_TRUOC: return "TDVM_BAO_CAO_KHACH_HANG_TRA_TRUOC";
                case DanhSachBaoCaoQuangBinh.BCTH_BAO_CAO_TIN_DUNG_THEO_XA: return "BCTH_BAO_CAO_TIN_DUNG_THEO_XA";
                case DanhSachBaoCaoQuangBinh.BCTH_BAO_CAO_TIN_DUNG_THEO_CBTD: return "BCTH_BAO_CAO_TIN_DUNG_THEO_CBTD";
                case DanhSachBaoCaoQuangBinh.BCTH_BAO_CAO_TIEN_DO_TOAN_TINH: return "BCTH_BAO_CAO_TIEN_DO_TOAN_TINH";
                case DanhSachBaoCaoQuangBinh.BCTH_BAO_CAO_TIEN_DO_TONG_HOP_TOAN_TINH: return "BCTH_BAO_CAO_TIEN_DO_TONG_HOP_TOAN_TINH";
                case DanhSachBaoCaoQuangBinh.BCTH_BAO_CAO_THEO_NGUON_CUA_TINH: return "BCTH_BAO_CAO_THEO_NGUON_CUA_TINH";
                case DanhSachBaoCaoQuangBinh.BCTH_BAO_CAO_TIN_DUNG_CUA_CHI_NHANH: return "BCTH_BAO_CAO_TIN_DUNG_CUA_CHI_NHANH";
                case DanhSachBaoCaoQuangBinh.BCTH_BAO_CAO_TIN_DUNG_THEO_NGUON_CUA_CHI_NHANH: return "BCTH_BAO_CAO_TIN_DUNG_THEO_NGUON_CUA_CHI_NHANH";
                default: return "";
            }
        }

        public static DanhSachBaoCaoQuangBinh layBaoCaoQuangBinhTheoMa(string maBaoCao)
        {
            switch (maBaoCao)
            {
                case "KHTV_QUYET_DINH_CONG_NHAN_TVIEN": return DanhSachBaoCaoQuangBinh.KHTV_QUYET_DINH_CONG_NHAN_TVIEN;
                case "KHTV_BAN_KHAO_SAT_HOAN_CANH_KTXH": return DanhSachBaoCaoQuangBinh.KHTV_BAN_KHAO_SAT_HOAN_CANH_KTXH;
                case "KHTV_DK_THAM_GIA_THANH_VIEN_QUY": return DanhSachBaoCaoQuangBinh.KHTV_DK_THAM_GIA_THANH_VIEN_QUY;
                case "KHTV_HOP_DONG_BAO_LANH_NHOM": return DanhSachBaoCaoQuangBinh.KHTV_HOP_DONG_BAO_LANH_NHOM;
                case "TDVM_BANG_THEO_DOI_NO_QUA_HAN": return DanhSachBaoCaoQuangBinh.TDVM_BANG_THEO_DOI_NO_QUA_HAN;
                case "TDVM_DON_XIN_VAY_VON_VAY_KHAC": return DanhSachBaoCaoQuangBinh.TDVM_DON_XIN_VAY_VON_VAY_KHAC;
                case "TDVM_DON_XIN_VAY_VON_TRA_DAN_VA_BO_SUNG": return DanhSachBaoCaoQuangBinh.TDVM_DON_XIN_VAY_VON_TRA_DAN_VA_BO_SUNG;
                case "TDVM_HOP_DONG_VAY_VON_VAY_KHAC": return DanhSachBaoCaoQuangBinh.TDVM_HOP_DONG_VAY_VON_VAY_KHAC;
                case "TDVM_HOP_DONG_VAY_VON_TRA_DAN_VA_BO_SUNG": return DanhSachBaoCaoQuangBinh.TDVM_HOP_DONG_VAY_VON_TRA_DAN_VA_BO_SUNG;
                case "TDVM_DS_PHAT_VON": return DanhSachBaoCaoQuangBinh.TDVM_DS_PHAT_VON;
                case "TDVM_DS_DE_NGHI_VAY_VON": return DanhSachBaoCaoQuangBinh.TDVM_DS_DE_NGHI_VAY_VON;
                case "TDVM_DS_THU_LAI_VON_MUA_VU": return DanhSachBaoCaoQuangBinh.TDVM_DS_THU_LAI_VON_MUA_VU;
                case "TDVM_DS_THU_TIEN_VON_TRA_DAN_TK": return DanhSachBaoCaoQuangBinh.TDVM_DS_THU_TIEN_VON_TRA_DAN_TK;
                case "TDVM_SO_THEO_DOI_KHTV": return DanhSachBaoCaoQuangBinh.TDVM_SO_THEO_DOI_KHTV;
                case "TDVM_SO_THEO_DOI_NO_RUI_RO": return DanhSachBaoCaoQuangBinh.TDVM_SO_THEO_DOI_NO_RUI_RO;
                case "TDVM_SO_THEO_DOI_VON_VAY": return DanhSachBaoCaoQuangBinh.TDVM_SO_THEO_DOI_VON_VAY;
                case "TDVM_SO_TIN_DUNG_CAP_CUM": return DanhSachBaoCaoQuangBinh.TDVM_SO_TIN_DUNG_CAP_CUM;
                case "TDVM_SO_TIN_DUNG_CAP_XA": return DanhSachBaoCaoQuangBinh.TDVM_SO_TIN_DUNG_CAP_XA;
                case "TDVM_BAO_CAO_KHACH_HANG_CHAM_TRA": return DanhSachBaoCaoQuangBinh.TDVM_BAO_CAO_KHACH_HANG_CHAM_TRA;
                case "HDVO_DANH_SACH_HOAN_TK": return DanhSachBaoCaoQuangBinh.HDVO_DANH_SACH_HOAN_TK;
                case "HDVO_DS_HUY_DONG_VON_CAP_CUM_NHOM": return DanhSachBaoCaoQuangBinh.HDVO_DS_HUY_DONG_VON_CAP_CUM_NHOM;
                case "HDVO_DS_HUY_DONG_VON_CAP_XA": return DanhSachBaoCaoQuangBinh.HDVO_DS_HUY_DONG_VON_CAP_XA;
                case "BCTC_BANG_CAN_DOI_KE_TOAN": return DanhSachBaoCaoQuangBinh.BCTC_BANG_CAN_DOI_KE_TOAN;
                case "BCTC_BANG_CAN_DOI_TKKT_THU": return DanhSachBaoCaoQuangBinh.BCTC_BANG_CAN_DOI_TKKT_THU;
                case "BCTC_LUU_CHUYEN_TIEN_TE": return DanhSachBaoCaoQuangBinh.BCTC_LUU_CHUYEN_TIEN_TE;
                case "BCTC_THUYET_MINH_TAI_CHINH": return DanhSachBaoCaoQuangBinh.BCTC_THUYET_MINH_TAI_CHINH;
                case "BCTH_HOAT_DONG": return DanhSachBaoCaoQuangBinh.BCTH_HOAT_DONG;
                case "BCTH_HOAT_DONG_CBTD": return DanhSachBaoCaoQuangBinh.BCTH_HOAT_DONG_CBTD;
                case "BCTH_HOAT_DONG_CBTD_SP": return DanhSachBaoCaoQuangBinh.BCTH_HOAT_DONG_CBTD_SP;
                case "BCTH_HOAT_DONG_DB": return DanhSachBaoCaoQuangBinh.BCTH_HOAT_DONG_DB;
                case "BCTH_HOAT_DONG_TCVM": return DanhSachBaoCaoQuangBinh.BCTH_HOAT_DONG_TCVM;
                case "BCTC_KET_QUA_HOAT_DONG": return DanhSachBaoCaoQuangBinh.BCTC_KET_QUA_HOAT_DONG;
                case "BCTH_BC_TIEN_DO_THEO_NGUON_TOAN_HE_THONG": return DanhSachBaoCaoQuangBinh.BCTH_BC_TIEN_DO_THEO_NGUON_TOAN_HE_THONG;
                case "GDKT_BAO_CAO_CHI_PHI_THEO_NGUON": return DanhSachBaoCaoQuangBinh.GDKT_BAO_CAO_CHI_PHI_THEO_NGUON;
                case "GDKT_BAO_CAO_THU_NHAP_THEO_NGUON": return DanhSachBaoCaoQuangBinh.GDKT_BAO_CAO_THU_NHAP_THEO_NGUON;
                case "TDVM_BAO_CAO_KHACH_HANG_TRA_TRUOC": return DanhSachBaoCaoQuangBinh.TDVM_BAO_CAO_KHACH_HANG_TRA_TRUOC;
                case "BCTH_BAO_CAO_TIN_DUNG_THEO_XA": return DanhSachBaoCaoQuangBinh.BCTH_BAO_CAO_TIN_DUNG_THEO_XA;
                case "BCTH_BAO_CAO_TIN_DUNG_THEO_CBTD": return DanhSachBaoCaoQuangBinh.BCTH_BAO_CAO_TIN_DUNG_THEO_CBTD;
                case "BCTH_BAO_CAO_TIEN_DO_TOAN_TINH": return DanhSachBaoCaoQuangBinh.BCTH_BAO_CAO_TIEN_DO_TOAN_TINH;
                case "BCTH_BAO_CAO_TIEN_DO_TONG_HOP_TOAN_TINH": return DanhSachBaoCaoQuangBinh.BCTH_BAO_CAO_TIEN_DO_TONG_HOP_TOAN_TINH;
                case "BCTH_BAO_CAO_THEO_NGUON_CUA_TINH": return DanhSachBaoCaoQuangBinh.BCTH_BAO_CAO_THEO_NGUON_CUA_TINH;
                case "BCTH_BAO_CAO_TIN_DUNG_CUA_CHI_NHANH": return DanhSachBaoCaoQuangBinh.BCTH_BAO_CAO_TIN_DUNG_CUA_CHI_NHANH;
                case "BCTH_BAO_CAO_TIN_DUNG_THEO_NGUON_CUA_CHI_NHANH": return DanhSachBaoCaoQuangBinh.BCTH_BAO_CAO_TIN_DUNG_THEO_NGUON_CUA_CHI_NHANH;
                default: return DanhSachBaoCaoQuangBinh.KHTV_QUYET_DINH_CONG_NHAN_TVIEN;
            }
        }

        /*
        public enum DanhSachBaoCaoTheoDinhKy
        {
            NHAT_KY_CHUNG,
            NHAT_KY_CHI_TIEN,
            NHAT_KY_THU_TIEN,
            SO_CAI_TAI_KHOAN,
            SO_QUY_TIEN_MAT,
            SO_CHI_TIET_TAI_KHOAN,
            BC_VON_TIET_KIEM_TVIEN,
            BC_VON_TIET_KIEM_CUM,
            BCTK_DU_NO_TD_THEO_NGANH,
            BCTK_DU_NO_TD_THEO_LH_TOCHUC_CANHAN,
            BCTK_HOAT_DONG_HDV,
            BCTK_BC_PLOAI_NO_DP_RUIRO,
            BCTK_BC_XULY_RUIRO_CHOVAY,
            BCTK_BC_TYLE_DAM_BAO_ANTOAN,
            BCTK_BC_KHANG_CHOVAY_VUOT_QDINH,
            BC_TH_VAN_HANH,
            BC_BHTH_DSACH_BAO_VE_VON_VAY,
            KTDL_GDKT_BANG_CAN_DOI_NOI_BANG

        };
        public static string layMaBaoCao(this DanhSachBaoCaoTheoDinhKy danhSachBaoCao)
        {
            switch (danhSachBaoCao)
            {
                case DanhSachBaoCaoTheoDinhKy.NHAT_KY_CHUNG: return "MF_KTDL_GDKT_NHAT_KY_CHUNG";
                case DanhSachBaoCaoTheoDinhKy.NHAT_KY_CHI_TIEN: return "MF_KTDL_GDKT_NHAT_KY_CHI_TIEN";
                case DanhSachBaoCaoTheoDinhKy.NHAT_KY_THU_TIEN: return "MF_KTDL_GDKT_NHAT_KY_THU_TIEN";
                case DanhSachBaoCaoTheoDinhKy.SO_CAI_TAI_KHOAN: return "MF_KTDL_GDKT_SO_CAI_TAI_KHOAN";
                case DanhSachBaoCaoTheoDinhKy.SO_QUY_TIEN_MAT: return "MF_KTDL_GDKT_SO_QUY_TIEN_MAT";
                case DanhSachBaoCaoTheoDinhKy.SO_CHI_TIET_TAI_KHOAN: return "MF_KTDL_GDKT_SO_CHI_TIET_TAI_KHOAN";
                case DanhSachBaoCaoTheoDinhKy.BC_VON_TIET_KIEM_TVIEN: return "MF_KTDL_TDVM_BC_VON_TIET_KIEM_TVIEN";
                case DanhSachBaoCaoTheoDinhKy.BC_VON_TIET_KIEM_CUM: return "MF_KTDL_TDVM_BC_VON_TIET_KIEM_CUM";
                case DanhSachBaoCaoTheoDinhKy.BCTK_DU_NO_TD_THEO_NGANH: return "MF_KTDL_BCTK_DU_NO_TD_THEO_NGANH";
                case DanhSachBaoCaoTheoDinhKy.BCTK_DU_NO_TD_THEO_LH_TOCHUC_CANHAN: return "MF_KTDL_BCTK_DU_NO_TD_THEO_LH_TOCHUC_CANHAN";
                case DanhSachBaoCaoTheoDinhKy.BCTK_HOAT_DONG_HDV: return "MF_KTDL_BCTK_HOAT_DONG_HDV";
                case DanhSachBaoCaoTheoDinhKy.BCTK_BC_PLOAI_NO_DP_RUIRO: return "MF_KTDL_BCTK_BC_PLOAI_NO_DP_RUIRO";
                case DanhSachBaoCaoTheoDinhKy.BCTK_BC_XULY_RUIRO_CHOVAY: return "MF_KTDL_BCTK_BC_XULY_RUIRO_CHOVAY";
                case DanhSachBaoCaoTheoDinhKy.BCTK_BC_TYLE_DAM_BAO_ANTOAN: return "MF_KTDL_BCTK_BC_TYLE_DAM_BAO_ANTOAN";
                case DanhSachBaoCaoTheoDinhKy.BCTK_BC_KHANG_CHOVAY_VUOT_QDINH: return "MF_KTDL_BCTK_BC_KHANG_CHOVAY_VUOT_QDINH";
                case DanhSachBaoCaoTheoDinhKy.BC_TH_VAN_HANH: return "MF_KTDL_BCTH_BC_VAN_HANH";
                case DanhSachBaoCaoTheoDinhKy.BC_BHTH_DSACH_BAO_VE_VON_VAY: return "MF_KTDL_BHTH_DSACH_BAO_VE_VON_VAY";
                case DanhSachBaoCaoTheoDinhKy.KTDL_GDKT_BANG_CAN_DOI_NOI_BANG: return "MF_KTDL_GDKT_BANG_CAN_DOI_NOI_BANG";
                default: return "";
            }
        }
        public static int layIdBaoCao(this DanhSachBaoCaoTheoDinhKy danhSachBaoCao)
        {
            switch (danhSachBaoCao)
            {
                case DanhSachBaoCaoTheoDinhKy.NHAT_KY_CHUNG: return 1100401;
                case DanhSachBaoCaoTheoDinhKy.NHAT_KY_CHI_TIEN: return 1100402;
                case DanhSachBaoCaoTheoDinhKy.NHAT_KY_THU_TIEN: return 1100403;
                case DanhSachBaoCaoTheoDinhKy.SO_CAI_TAI_KHOAN: return 1100404;
                case DanhSachBaoCaoTheoDinhKy.SO_QUY_TIEN_MAT: return 1100405;
                case DanhSachBaoCaoTheoDinhKy.SO_CHI_TIET_TAI_KHOAN: return 1100406;
                case DanhSachBaoCaoTheoDinhKy.BCTK_DU_NO_TD_THEO_NGANH: return 1101101;
                case DanhSachBaoCaoTheoDinhKy.BCTK_DU_NO_TD_THEO_LH_TOCHUC_CANHAN: return 1101102;
                case DanhSachBaoCaoTheoDinhKy.BCTK_HOAT_DONG_HDV: return 1101103;
                case DanhSachBaoCaoTheoDinhKy.BCTK_BC_PLOAI_NO_DP_RUIRO: return 1101104;
                case DanhSachBaoCaoTheoDinhKy.BCTK_BC_XULY_RUIRO_CHOVAY: return 1101105;
                case DanhSachBaoCaoTheoDinhKy.BCTK_BC_TYLE_DAM_BAO_ANTOAN: return 1101106;
                case DanhSachBaoCaoTheoDinhKy.BCTK_BC_KHANG_CHOVAY_VUOT_QDINH: return 1101107;
                case DanhSachBaoCaoTheoDinhKy.BC_TH_VAN_HANH: return 1101001;
                case DanhSachBaoCaoTheoDinhKy.BC_BHTH_DSACH_BAO_VE_VON_VAY: return 1100708;
                case DanhSachBaoCaoTheoDinhKy.KTDL_GDKT_BANG_CAN_DOI_NOI_BANG: return 1100412;
                default: return 0;
            }
        }
        */

        /*
        public enum DanhSachBaoCaoTheoGiaoDich
        {
            KHTV_PHIEU_KHAOSAT,
            KHTV_PHIEU_DANHGIA,
            GDKT_IN_GIAO_DICH,
            GDKT_PHIEU_THU,
            GDKT_PHIEU_CHI,
            GDKT_PHIEU_HACH_TOAN,
            HDVO_SO_TKCKH,
            TDVM_HDTD,
            TDVM_KUOCVM,
            TDVM_KUOCVM_KEHOACH,
            TDVM_DS_KHANG_NHANVON,
            TDVM_HOA_DON_THU_TIEN_KY
        };
        public static string layMaBaoCao(this DanhSachBaoCaoTheoGiaoDich danhSachBaoCao)
        {
            switch (danhSachBaoCao)
            {
                case DanhSachBaoCaoTheoGiaoDich.KHTV_PHIEU_KHAOSAT: return "KHTV_PHIEU_KHAOSAT";
                case DanhSachBaoCaoTheoGiaoDich.KHTV_PHIEU_DANHGIA: return "KHTV_PHIEU_DANHGIA";
                case DanhSachBaoCaoTheoGiaoDich.GDKT_IN_GIAO_DICH: return "GDKT_IN_GIAO_DICH";
                case DanhSachBaoCaoTheoGiaoDich.GDKT_PHIEU_THU: return "GDKT_PHIEU_THU";
                case DanhSachBaoCaoTheoGiaoDich.GDKT_PHIEU_CHI: return "GDKT_PHIEU_CHI";
                case DanhSachBaoCaoTheoGiaoDich.GDKT_PHIEU_HACH_TOAN: return "GDKT_PHIEU_HACH_TOAN";
                case DanhSachBaoCaoTheoGiaoDich.HDVO_SO_TKCKH: return "HDVO_SO_TKCKH";
                case DanhSachBaoCaoTheoGiaoDich.TDVM_HDTD: return "TDVM_HDTD";
                case DanhSachBaoCaoTheoGiaoDich.TDVM_KUOCVM: return "TDVM_KUOCVM";
                case DanhSachBaoCaoTheoGiaoDich.TDVM_KUOCVM_KEHOACH: return "TDVM_KUOCVM_KEHOACH";
                case DanhSachBaoCaoTheoGiaoDich.TDVM_DS_KHANG_NHANVON: return "TDVM_DS_KHANG_NHANVON";
                case DanhSachBaoCaoTheoGiaoDich.TDVM_HOA_DON_THU_TIEN_KY: return "9254_HOA_DON_THU_TIEN_KY";
                default: return "";
            }
        }
        public static int layIdBaoCao(this DanhSachBaoCaoTheoGiaoDich danhSachBaoCao)
        {
            switch (danhSachBaoCao)
            {
                case DanhSachBaoCaoTheoGiaoDich.KHTV_PHIEU_KHAOSAT: return 2001;
                case DanhSachBaoCaoTheoGiaoDich.KHTV_PHIEU_DANHGIA: return 2002;
                case DanhSachBaoCaoTheoGiaoDich.GDKT_IN_GIAO_DICH: return 3000;
                case DanhSachBaoCaoTheoGiaoDich.GDKT_PHIEU_THU: return 3001;
                case DanhSachBaoCaoTheoGiaoDich.GDKT_PHIEU_CHI: return 3002;
                case DanhSachBaoCaoTheoGiaoDich.GDKT_PHIEU_HACH_TOAN: return 3003;
                case DanhSachBaoCaoTheoGiaoDich.HDVO_SO_TKCKH: return 4001;
                case DanhSachBaoCaoTheoGiaoDich.TDVM_HDTD: return 5001;
                case DanhSachBaoCaoTheoGiaoDich.TDVM_KUOCVM: return 5002;
                case DanhSachBaoCaoTheoGiaoDich.TDVM_KUOCVM_KEHOACH: return 50021;
                case DanhSachBaoCaoTheoGiaoDich.TDVM_DS_KHANG_NHANVON: return 5003;
                case DanhSachBaoCaoTheoGiaoDich.TDVM_HOA_DON_THU_TIEN_KY: return 206;
                default: return 0;
            }
        }
        
        public enum DanhSachBaoCaoTheoDinhKy
        {
            NHAT_KY_CHUNG,
            NHAT_KY_CHI_TIEN,
            NHAT_KY_THU_TIEN,
            SO_CAI_TAI_KHOAN,
            SO_QUY_TIEN_MAT,
            SO_CHI_TIET_TAI_KHOAN
        };
        public static string layMaBaoCao(this DanhSachBaoCaoTheoDinhKy danhSachBaoCao)
        {
            switch (danhSachBaoCao)
            {
                case DanhSachBaoCaoTheoDinhKy.NHAT_KY_CHUNG: return "9231_NHAT_KY_CHUNG";
                case DanhSachBaoCaoTheoDinhKy.NHAT_KY_CHI_TIEN: return "9232_NHAT_KY_CHI_TIEN";
                case DanhSachBaoCaoTheoDinhKy.NHAT_KY_THU_TIEN: return "9233_NHAT_KY_THU_TIEN";
                case DanhSachBaoCaoTheoDinhKy.SO_CAI_TAI_KHOAN: return "9234_SO_CAI_TAI_KHOAN";
                case DanhSachBaoCaoTheoDinhKy.SO_QUY_TIEN_MAT: return "9235_SO_QUY_TIEN_MAT";
                case DanhSachBaoCaoTheoDinhKy.SO_CHI_TIET_TAI_KHOAN: return "9236_SO_CHI_TIET_TAI_KHOAN";
                default: return "";
            }
        }
        public static int layIdBaoCao(this DanhSachBaoCaoTheoDinhKy danhSachBaoCao)
        {
            switch (danhSachBaoCao)
            {
                case DanhSachBaoCaoTheoDinhKy.NHAT_KY_CHUNG: return 180;
                case DanhSachBaoCaoTheoDinhKy.NHAT_KY_CHI_TIEN: return 181;
                case DanhSachBaoCaoTheoDinhKy.NHAT_KY_THU_TIEN: return 182;
                case DanhSachBaoCaoTheoDinhKy.SO_CAI_TAI_KHOAN: return 183;
                case DanhSachBaoCaoTheoDinhKy.SO_QUY_TIEN_MAT: return 184;
                case DanhSachBaoCaoTheoDinhKy.SO_CHI_TIET_TAI_KHOAN: return 185;
                default: return 0;
            }
        }
        */
        public enum DanhSachEFBaoCao
        {
            EF_BAO_CAO,
            EF_CHI_TIEU,
            GIA_TRI_THAM_SO_GIAO_DIEN,  //Giá trị tham số trên giao diện,
            TINH_TRANG_HOAT_DONG,  //Tình trạng hoạt động của đơn vị
            EF_KHTV_100100, //Danh sách khách hàng thành viên
            EF_KHTV_100101, //Chi tiết thành viên xác lập
            EF_KHTV_100102, //Tổng hợp thành viên xác lập
            EF_TCKT_100209, //Nhật ký quỹ
            EF_TCKT_100210, //Nhật ký quỹ 1012
            EF_TCKT_100211, //Bảng cân đối tài khoản kế toán (A01/QTDCS)
            EF_TCKT_100212, //Bảng cân đối tài khoản kế toán (A01/QTDCS) - Ngoại bảng
            EF_TCKT_100213, //Thu nhập - chi phí
            EF_HDVO_100401, //Sao kê tiền gửi
            EF_HDVO_100402, //Sao kê tiền gửi lớn nhất
            EF_HDVO_100403, //Sao kê lãi suất bình quân
            EF_HDVO_100404, //DS sổ tiền gửi đến hạn
            EF_HDVO_100405, //Giao dịch tiết kiệm
            EF_TDTT_100502, //Sao kê theo khu vực
            EF_TDTT_100503, //Sao kê theo loại hợp đồng
            EF_TDTT_100505, //Sao kê theo tài khoản
            EF_TDTT_100506, //Sao kê theo cán bộ tín dụng
            EF_TDTT_100507, //Sao kê đối chiếu dư nợ
            EF_TDTT_100510, //Sao kê tín dụng theo phần trăm vốn tự có
            EF_TDTT_100520, //Báo cáo phát sinh nhập xuất tài sản
            EF_TGUI_100602, //Giao dịch tiền gửi
            EF_TVAY_100702, //Hợp đồng tiền vay đến hạn
            EF_CHI_TIEU_001, //	Tổng số thành viên vay vốn từ đầu năm
            EF_CHI_TIEU_002, //	Tổng số thành viên còn dư nợ
            EF_CHI_TIEU_003, //	Cán bộ chuyên trách
            EF_CHI_TIEU_004, //	Cán bộ có trình độ đại học cao đẳng trở lên (đã có bằng tốt nghiệp)
            EF_CHI_TIEU_005, //	Vốn tự có cấp 1: Vốn điều lệ
            EF_CHI_TIEU_006, //	Vốn tự có cấp 1: Quỹ dự trữ bổ sung vốn điều lệ
            EF_CHI_TIEU_007, //	Vốn cấp 2: (Dự phòng chung)
            EF_CHI_TIEU_008, //	Huy động vốn: Các khoản huy động có kỳ hạn
            EF_CHI_TIEU_009, //	Huy động có kỳ hạn từ 12 tháng trở lên
            EF_CHI_TIEU_010, //	Vay tại QTDTW
            EF_CHI_TIEU_011, //	Vay trung hạn
            EF_CHI_TIEU_012, //	Vốn khác: Doanh số cho vay trong tháng
            EF_CHI_TIEU_013, //	Vốn khác: Doanh số cho vay từ đầu năm
            EF_CHI_TIEU_014, //	Vốn khác: Doanh số thu nợ từ đầu năm
            EF_CHI_TIEU_015, //	Vốn khác: Tổng doanh thu đến cuối kỳ báo cáo
            EF_CHI_TIEU_016, //	Vốn khác: Doanh thu trong tháng
            EF_CHI_TIEU_017, //	Dư nợ trung hạn
            EF_CHI_TIEU_018, //	Nợ nhóm 2
            EF_CHI_TIEU_019, //	Nợ nhóm 3
            EF_CHI_TIEU_020, //	Nợ nhóm 4
            EF_CHI_TIEU_021, //	Nợ nhóm 5
            EF_CHI_TIEU_022, //	Khả năng nguồn vốn trung hạn
            EF_CHI_TIEU_023, //	Giá trị còn lại của tài sản cố định
            EF_CHI_TIEU_024, //	Chênh lệch thu, chi
            EF_CHI_TIEU_025, //	Tiền mặt tồn quỹ
            EF_CHI_TIEU_026, //	Tiền gửi TCTD khác
            EF_CHI_TIEU_027 //	Tiền gửi tại QTDTW
        };

        public static string layEFBaoCao(this DanhSachEFBaoCao danhSachEFBaoCao)
        {
            switch (danhSachEFBaoCao)
            {
                case DanhSachEFBaoCao.EF_BAO_CAO: return "EF_BAO_CAO";
                case DanhSachEFBaoCao.EF_CHI_TIEU: return "EF_CHI_TIEU";
                case DanhSachEFBaoCao.GIA_TRI_THAM_SO_GIAO_DIEN: return "GIA_TRI_THAM_SO_GIAO_DIEN";
                case DanhSachEFBaoCao.TINH_TRANG_HOAT_DONG: return "TINH_TRANG_HOAT_DONG";
                case DanhSachEFBaoCao.EF_KHTV_100100: return "EF_KHTV_100100";
                case DanhSachEFBaoCao.EF_KHTV_100101: return "EF_KHTV_100101";
                case DanhSachEFBaoCao.EF_KHTV_100102: return "EF_KHTV_100102";
                case DanhSachEFBaoCao.EF_TCKT_100209: return "EF_TCKT_100209";
                case DanhSachEFBaoCao.EF_TCKT_100210: return "EF_TCKT_100210";
                case DanhSachEFBaoCao.EF_TCKT_100211: return "EF_TCKT_100211";
                case DanhSachEFBaoCao.EF_TCKT_100212: return "EF_TCKT_100212";
                case DanhSachEFBaoCao.EF_TCKT_100213: return "EF_TCKT_100213";
                case DanhSachEFBaoCao.EF_HDVO_100401: return "EF_HDVO_100401";
                case DanhSachEFBaoCao.EF_HDVO_100402: return "EF_HDVO_100402";
                case DanhSachEFBaoCao.EF_HDVO_100403: return "EF_HDVO_100403";
                case DanhSachEFBaoCao.EF_HDVO_100404: return "EF_HDVO_100404";
                case DanhSachEFBaoCao.EF_HDVO_100405: return "EF_HDVO_100405";
                case DanhSachEFBaoCao.EF_TDTT_100502: return "EF_TDTT_100502";
                case DanhSachEFBaoCao.EF_TDTT_100503: return "EF_TDTT_100503";
                case DanhSachEFBaoCao.EF_TDTT_100505: return "EF_TDTT_100505";
                case DanhSachEFBaoCao.EF_TDTT_100506: return "EF_TDTT_100506";
                case DanhSachEFBaoCao.EF_TDTT_100507: return "EF_TDTT_100507";
                case DanhSachEFBaoCao.EF_TDTT_100510: return "EF_TDTT_100510";
                case DanhSachEFBaoCao.EF_TDTT_100520: return "EF_TDTT_100520";
                case DanhSachEFBaoCao.EF_TGUI_100602: return "EF_TGUI_100602";
                case DanhSachEFBaoCao.EF_TVAY_100702: return "EF_TVAY_100702";
                case DanhSachEFBaoCao.EF_CHI_TIEU_001: return "EF_CHI_TIEU_001";
                case DanhSachEFBaoCao.EF_CHI_TIEU_002: return "EF_CHI_TIEU_002";
                case DanhSachEFBaoCao.EF_CHI_TIEU_003: return "EF_CHI_TIEU_003";
                case DanhSachEFBaoCao.EF_CHI_TIEU_004: return "EF_CHI_TIEU_004";
                case DanhSachEFBaoCao.EF_CHI_TIEU_005: return "EF_CHI_TIEU_005";
                case DanhSachEFBaoCao.EF_CHI_TIEU_006: return "EF_CHI_TIEU_006";
                case DanhSachEFBaoCao.EF_CHI_TIEU_007: return "EF_CHI_TIEU_007";
                case DanhSachEFBaoCao.EF_CHI_TIEU_008: return "EF_CHI_TIEU_008";
                case DanhSachEFBaoCao.EF_CHI_TIEU_009: return "EF_CHI_TIEU_009";
                case DanhSachEFBaoCao.EF_CHI_TIEU_010: return "EF_CHI_TIEU_010";
                case DanhSachEFBaoCao.EF_CHI_TIEU_011: return "EF_CHI_TIEU_011";
                case DanhSachEFBaoCao.EF_CHI_TIEU_012: return "EF_CHI_TIEU_012";
                case DanhSachEFBaoCao.EF_CHI_TIEU_013: return "EF_CHI_TIEU_013";
                case DanhSachEFBaoCao.EF_CHI_TIEU_014: return "EF_CHI_TIEU_014";
                case DanhSachEFBaoCao.EF_CHI_TIEU_015: return "EF_CHI_TIEU_015";
                case DanhSachEFBaoCao.EF_CHI_TIEU_016: return "EF_CHI_TIEU_016";
                case DanhSachEFBaoCao.EF_CHI_TIEU_017: return "EF_CHI_TIEU_017";
                case DanhSachEFBaoCao.EF_CHI_TIEU_018: return "EF_CHI_TIEU_018";
                case DanhSachEFBaoCao.EF_CHI_TIEU_019: return "EF_CHI_TIEU_019";
                case DanhSachEFBaoCao.EF_CHI_TIEU_020: return "EF_CHI_TIEU_020";
                case DanhSachEFBaoCao.EF_CHI_TIEU_021: return "EF_CHI_TIEU_021";
                case DanhSachEFBaoCao.EF_CHI_TIEU_022: return "EF_CHI_TIEU_022";
                case DanhSachEFBaoCao.EF_CHI_TIEU_023: return "EF_CHI_TIEU_023";
                case DanhSachEFBaoCao.EF_CHI_TIEU_024: return "EF_CHI_TIEU_024";
                case DanhSachEFBaoCao.EF_CHI_TIEU_025: return "EF_CHI_TIEU_025";
                case DanhSachEFBaoCao.EF_CHI_TIEU_026: return "EF_CHI_TIEU_026";
                case DanhSachEFBaoCao.EF_CHI_TIEU_027: return "EF_CHI_TIEU_027";

                default: return "";
            }
        }

        public static DanhSachEFBaoCao layEFBaoCao(string danhSachEFBaoCao)
        {
            switch (danhSachEFBaoCao)
            {
                case "EF_BAO_CAO": return DanhSachEFBaoCao.EF_BAO_CAO;
                case "EF_CHI_TIEU": return DanhSachEFBaoCao.EF_CHI_TIEU;
                case "GIA_TRI_THAM_SO_GIAO_DIEN": return DanhSachEFBaoCao.GIA_TRI_THAM_SO_GIAO_DIEN;
                case "TINH_TRANG_HOAT_DONG": return DanhSachEFBaoCao.TINH_TRANG_HOAT_DONG;
                case "EF_KHTV_100100": return DanhSachEFBaoCao.EF_KHTV_100100;
                case "EF_KHTV_100101": return DanhSachEFBaoCao.EF_KHTV_100101;
                case "EF_KHTV_100102": return DanhSachEFBaoCao.EF_KHTV_100102;
                case "EF_TCKT_100209": return DanhSachEFBaoCao.EF_TCKT_100209;
                case "EF_TCKT_100210": return DanhSachEFBaoCao.EF_TCKT_100210;
                case "EF_TCKT_100211": return DanhSachEFBaoCao.EF_TCKT_100211;
                case "EF_TCKT_100212": return DanhSachEFBaoCao.EF_TCKT_100212;
                case "EF_TCKT_100213": return DanhSachEFBaoCao.EF_TCKT_100213;
                case "EF_HDVO_100401": return DanhSachEFBaoCao.EF_HDVO_100401;
                case "EF_HDVO_100402": return DanhSachEFBaoCao.EF_HDVO_100402;
                case "EF_HDVO_100403": return DanhSachEFBaoCao.EF_HDVO_100403;
                case "EF_HDVO_100404": return DanhSachEFBaoCao.EF_HDVO_100404;
                case "EF_HDVO_100405": return DanhSachEFBaoCao.EF_HDVO_100405;
                case "EF_TDTT_100502": return DanhSachEFBaoCao.EF_TDTT_100502;
                case "EF_TDTT_100503": return DanhSachEFBaoCao.EF_TDTT_100503;
                case "EF_TDTT_100505": return DanhSachEFBaoCao.EF_TDTT_100505;
                case "EF_TDTT_100506": return DanhSachEFBaoCao.EF_TDTT_100506;
                case "EF_TDTT_100507": return DanhSachEFBaoCao.EF_TDTT_100507;
                case "EF_TDTT_100510": return DanhSachEFBaoCao.EF_TDTT_100510;
                case "EF_TDTT_100520": return DanhSachEFBaoCao.EF_TDTT_100520;
                case "EF_TGUI_100602": return DanhSachEFBaoCao.EF_TGUI_100602;
                case "EF_TVAY_100702": return DanhSachEFBaoCao.EF_TVAY_100702;
                case "EF_CHI_TIEU_001": return DanhSachEFBaoCao.EF_CHI_TIEU_001;
                case "EF_CHI_TIEU_002": return DanhSachEFBaoCao.EF_CHI_TIEU_002;
                case "EF_CHI_TIEU_003": return DanhSachEFBaoCao.EF_CHI_TIEU_003;
                case "EF_CHI_TIEU_004": return DanhSachEFBaoCao.EF_CHI_TIEU_004;
                case "EF_CHI_TIEU_005": return DanhSachEFBaoCao.EF_CHI_TIEU_005;
                case "EF_CHI_TIEU_006": return DanhSachEFBaoCao.EF_CHI_TIEU_006;
                case "EF_CHI_TIEU_007": return DanhSachEFBaoCao.EF_CHI_TIEU_007;
                case "EF_CHI_TIEU_008": return DanhSachEFBaoCao.EF_CHI_TIEU_008;
                case "EF_CHI_TIEU_009": return DanhSachEFBaoCao.EF_CHI_TIEU_009;
                case "EF_CHI_TIEU_010": return DanhSachEFBaoCao.EF_CHI_TIEU_010;
                case "EF_CHI_TIEU_011": return DanhSachEFBaoCao.EF_CHI_TIEU_011;
                case "EF_CHI_TIEU_012": return DanhSachEFBaoCao.EF_CHI_TIEU_012;
                case "EF_CHI_TIEU_013": return DanhSachEFBaoCao.EF_CHI_TIEU_013;
                case "EF_CHI_TIEU_014": return DanhSachEFBaoCao.EF_CHI_TIEU_014;
                case "EF_CHI_TIEU_015": return DanhSachEFBaoCao.EF_CHI_TIEU_015;
                case "EF_CHI_TIEU_016": return DanhSachEFBaoCao.EF_CHI_TIEU_016;
                case "EF_CHI_TIEU_017": return DanhSachEFBaoCao.EF_CHI_TIEU_017;
                case "EF_CHI_TIEU_018": return DanhSachEFBaoCao.EF_CHI_TIEU_018;
                case "EF_CHI_TIEU_019": return DanhSachEFBaoCao.EF_CHI_TIEU_019;
                case "EF_CHI_TIEU_020": return DanhSachEFBaoCao.EF_CHI_TIEU_020;
                case "EF_CHI_TIEU_021": return DanhSachEFBaoCao.EF_CHI_TIEU_021;
                case "EF_CHI_TIEU_022": return DanhSachEFBaoCao.EF_CHI_TIEU_022;
                case "EF_CHI_TIEU_023": return DanhSachEFBaoCao.EF_CHI_TIEU_023;
                case "EF_CHI_TIEU_024": return DanhSachEFBaoCao.EF_CHI_TIEU_024;
                case "EF_CHI_TIEU_025": return DanhSachEFBaoCao.EF_CHI_TIEU_025;
                case "EF_CHI_TIEU_026": return DanhSachEFBaoCao.EF_CHI_TIEU_026;
                case "EF_CHI_TIEU_027": return DanhSachEFBaoCao.EF_CHI_TIEU_027;

                default: return DanhSachEFBaoCao.GIA_TRI_THAM_SO_GIAO_DIEN;
            }
        }

        public static string layTenEFBaoCao(this DanhSachEFBaoCao danhSachEFBaoCao)
        {
            switch (danhSachEFBaoCao)
            {
                case DanhSachEFBaoCao.EF_KHTV_100101: return "Sao kê thành viên xác lập";
                case DanhSachEFBaoCao.EF_KHTV_100102: return "Tổng hợp thành viên xác lập";
                case DanhSachEFBaoCao.EF_TCKT_100209: return "Nhật ký quỹ";
                case DanhSachEFBaoCao.EF_TCKT_100210: return "Nhật ký quỹ 1012";
                case DanhSachEFBaoCao.EF_TCKT_100211: return "Bảng cân đối tài khoản kế toán (A01/QTDCS) - Nội bảng";
                case DanhSachEFBaoCao.EF_TCKT_100212: return "Bảng cân đối tài khoản kế toán (A01/QTDCS) - Ngoại bảng";
                case DanhSachEFBaoCao.EF_TCKT_100213: return "Thu nhập - chi phí";
                case DanhSachEFBaoCao.EF_HDVO_100401: return "Sao kê tổng hợp tiền gửi theo sản phẩm";
                case DanhSachEFBaoCao.EF_HDVO_100402: return "Sao kê tiền gửi lớn nhất theo sản phẩm, số tiền";
                case DanhSachEFBaoCao.EF_HDVO_100403: return "Sao kê lãi suất bình quân ";
                case DanhSachEFBaoCao.EF_HDVO_100404: return "DS sổ tiền gửi đến hạn theo sản phẩm";
                case DanhSachEFBaoCao.EF_HDVO_100405: return "Giao dịch tiết kiệm theo sản phẩm";
                case DanhSachEFBaoCao.EF_TDTT_100502: return "Sao kê theo khu vực";
                case DanhSachEFBaoCao.EF_TDTT_100503: return "Sao kê theo loại hợp đồng";
                case DanhSachEFBaoCao.EF_TDTT_100505: return "Sao kê theo tài khoản";
                case DanhSachEFBaoCao.EF_TDTT_100506: return "Sao kê theo cán bộ tín dụng";
                case DanhSachEFBaoCao.EF_TDTT_100507: return "Sao kê đối chiếu dư nợ";
                case DanhSachEFBaoCao.EF_TDTT_100510: return "Sao kê tín dụng theo phần trăm vốn tự có";
                case DanhSachEFBaoCao.EF_TDTT_100520: return "Báo cáo phát sinh nhập xuất tài sản";
                case DanhSachEFBaoCao.EF_TGUI_100602: return "Giao dịch tiền gửi";
                case DanhSachEFBaoCao.EF_TVAY_100702: return "Hợp đồng tiền vay đến hạn";
                default: return "";
            }
        }

        /// <summary>
        /// Danh sách truy vấn module tín dụng
        /// Dùng chung cho module tín dụng
        /// </summary> 
        public enum DanhSachTruyVanTinDung
        {
            INQ_DS_TD_SAN_PHAM,
            INQ_CT_TD_SAN_PHAM,
            INQ_DS_TD_VONG_VAY,
            INQ_CT_TD_VONG_VAY,
            INQ_DS_TD_HAN_MUC,
            INQ_CT_TD_HAN_MUC,
            INQ_DS_TD_TSDB,
            INQ_DS_TD_HDTC,
            INQ_DS_TDVM_HDTD,
            INQ_CT_TDVM_HDTD,
            INQ_DS_TDVM_KUOC,
            INQ_DS_TDVM_KUOC_TREE,
            INQ_CT_TDVM_KUOC,
            INQ_CT_TDVM_LAP_KUOC_DS,
            INQ_DS_TDVM_LICH_TRNO,
            INQ_CT_TDVM_LICH_TRNO,
            INQ_DS_TDVM_GIAI_NGAN,
            INQ_CT_TDVM_GIAI_NGAN,
            INQ_DS_TDVM_DU_THU,
            INQ_CT_TDVM_DU_THU,
            INQ_DS_TDVM_TRICH_LAP,
            INQ_CT_TDVM_TRICH_LAP,
            INQ_DS_TDVM_DCHINH_LSUAT,
            INQ_CT_TDVM_DCHINH_LSUAT,
            INQ_DS_TDVM_GIA_HAN,
            INQ_CT_TDVM_GIA_HAN,
            INQ_DS_TDVM_CHUYEN_HOAN,
            INQ_CT_TDVM_CHUYEN_HOAN,
            INQ_DS_TDVM_QUA_HAN,
            INQ_CT_TDVM_QUA_HAN,
            INQ_DS_TDVM_XLY_NO,
            INQ_CT_TDVM_XLY_NO,
            INQ_DS_TDVM_HOA_DON,
            INQ_CT_TDVM_HOA_DON,
            INQ_DS_TDVM_THU_GLAI,
            INQ_CT_TDVM_THU_GLAI,
            INQ_DS_TDVM_PHAN_BO,
            INQ_CT_TDVM_PHAN_BO,
            INQ_TDVM_LAY_GD_XLNO,
            INQ_TDVM_LAY_GD_XLNO_KUOC_TSAN,
            INQ_TDVM_LAY_GD_PBDT,
            INQ_CT_TDVM_GD_TAM_UNG,
            INQ_CT_TDVM_GD_HOAN_UNG,
            INQ_DS_TDVM_DONXINVAY,
            INQ_CT_TDVM_DONXINVAY,
        }

        public static string getValue(this DanhSachTruyVanTinDung danhSachTruyVan)
        {
            switch (danhSachTruyVan)
            {
                case DanhSachTruyVanTinDung.INQ_DS_TD_SAN_PHAM: return "INQ.DS.TD_SAN_PHAM";
                case DanhSachTruyVanTinDung.INQ_CT_TD_SAN_PHAM: return "INQ.CT.TD_SAN_PHAM";
                case DanhSachTruyVanTinDung.INQ_DS_TD_VONG_VAY: return "INQ.DS.TD_VONG_VAY";
                case DanhSachTruyVanTinDung.INQ_CT_TD_VONG_VAY: return "INQ.CT.TD_VONG_VAY";
                case DanhSachTruyVanTinDung.INQ_DS_TD_HAN_MUC: return "INQ.DS.TD_HAN_MUC";
                case DanhSachTruyVanTinDung.INQ_CT_TD_HAN_MUC: return "INQ.CT.TD_HAN_MUC";
                case DanhSachTruyVanTinDung.INQ_DS_TD_TSDB: return "INQ.DS.TD_TSDB";
                case DanhSachTruyVanTinDung.INQ_DS_TD_HDTC: return "INQ.DS.TD_HDTC";
                case DanhSachTruyVanTinDung.INQ_DS_TDVM_HDTD: return "INQ.DS.TDVM_HDTD";
                case DanhSachTruyVanTinDung.INQ_CT_TDVM_HDTD: return "INQ.CT.TDVM_HDTD";
                case DanhSachTruyVanTinDung.INQ_DS_TDVM_KUOC: return "INQ.DS.TDVM_KUOC";
                case DanhSachTruyVanTinDung.INQ_DS_TDVM_KUOC_TREE: return "INQ.DS.TDVM_KUOC_TREE";
                case DanhSachTruyVanTinDung.INQ_CT_TDVM_KUOC: return "INQ.CT.TDVM_KUOC";
                case DanhSachTruyVanTinDung.INQ_CT_TDVM_LAP_KUOC_DS: return "INQ.CT.TDVM_LAP_KUOC_DS";
                case DanhSachTruyVanTinDung.INQ_DS_TDVM_LICH_TRNO: return "INQ.DS.TDVM_LICH_TRNO";
                case DanhSachTruyVanTinDung.INQ_CT_TDVM_LICH_TRNO: return "INQ.CT.TDVM_LICH_TRNO";
                case DanhSachTruyVanTinDung.INQ_DS_TDVM_GIAI_NGAN: return "INQ.DS.TDVM_GIAI_NGAN";
                case DanhSachTruyVanTinDung.INQ_CT_TDVM_GIAI_NGAN: return "INQ.CT.TDVM_GIAI_NGAN";
                case DanhSachTruyVanTinDung.INQ_DS_TDVM_DU_THU: return "INQ.DS.TDVM_DU_THU";
                case DanhSachTruyVanTinDung.INQ_CT_TDVM_DU_THU: return "INQ.CT.TDVM_DU_THU";
                case DanhSachTruyVanTinDung.INQ_DS_TDVM_TRICH_LAP: return "INQ.DS.TDVM_TRICH_LAP";
                case DanhSachTruyVanTinDung.INQ_CT_TDVM_TRICH_LAP: return "INQ.CT.TDVM_TRICH_LAP";
                case DanhSachTruyVanTinDung.INQ_DS_TDVM_DCHINH_LSUAT: return "INQ.DS.TDVM_DCHINH_LSUAT";
                case DanhSachTruyVanTinDung.INQ_CT_TDVM_DCHINH_LSUAT: return "INQ.CT.TDVM_DCHINH_LSUAT";
                case DanhSachTruyVanTinDung.INQ_DS_TDVM_GIA_HAN: return "INQ.DS.TDVM_GIA_HAN";
                case DanhSachTruyVanTinDung.INQ_CT_TDVM_GIA_HAN: return "INQ.CT.TDVM_GIA_HAN";
                case DanhSachTruyVanTinDung.INQ_DS_TDVM_CHUYEN_HOAN: return "INQ.DS.TDVM_CHUYEN_HOAN";
                case DanhSachTruyVanTinDung.INQ_CT_TDVM_CHUYEN_HOAN: return "INQ.CT.TDVM_CHUYEN_HOAN";
                case DanhSachTruyVanTinDung.INQ_DS_TDVM_QUA_HAN: return "INQ.DS.TDVM_QUA_HAN";
                case DanhSachTruyVanTinDung.INQ_CT_TDVM_QUA_HAN: return "INQ.CT.TDVM_QUA_HAN";
                case DanhSachTruyVanTinDung.INQ_DS_TDVM_XLY_NO: return "INQ.DS.TDVM_XLY_NO";
                case DanhSachTruyVanTinDung.INQ_CT_TDVM_XLY_NO: return "INQ.CT.TDVM_XLY_NO";
                case DanhSachTruyVanTinDung.INQ_DS_TDVM_HOA_DON: return "INQ.DS.TDVM_HOA_DON";
                case DanhSachTruyVanTinDung.INQ_CT_TDVM_HOA_DON: return "INQ.CT.TDVM_HOA_DON";
                case DanhSachTruyVanTinDung.INQ_DS_TDVM_THU_GLAI: return "INQ.DS.TDVM_THU_GLAI";
                case DanhSachTruyVanTinDung.INQ_CT_TDVM_THU_GLAI: return "INQ.CT.TDVM_THU_GLAI";
                case DanhSachTruyVanTinDung.INQ_DS_TDVM_PHAN_BO: return "INQ.DS.TDVM_PHAN_BO";
                case DanhSachTruyVanTinDung.INQ_CT_TDVM_PHAN_BO: return "INQ.CT.TDVM_PHAN_BO";
                case DanhSachTruyVanTinDung.INQ_TDVM_LAY_GD_XLNO: return "INQ.TDVM_LAY_GD_XLNO";
                case DanhSachTruyVanTinDung.INQ_TDVM_LAY_GD_XLNO_KUOC_TSAN: return "INQ.TDVM_LAY_GD_XLNO_KUOC_TSAN";
                case DanhSachTruyVanTinDung.INQ_TDVM_LAY_GD_PBDT: return "INQ.TDVM_LAY_GD_PBDT";
                case DanhSachTruyVanTinDung.INQ_CT_TDVM_GD_TAM_UNG: return "INQ.CT.TDVM_GD_TAM_UNG";
                case DanhSachTruyVanTinDung.INQ_CT_TDVM_GD_HOAN_UNG: return "INQ.CT.TDVM_GD_HOAN_UNG";
                case DanhSachTruyVanTinDung.INQ_DS_TDVM_DONXINVAY: return "INQ.DS.TDVM_DONXINVAY";
                case DanhSachTruyVanTinDung.INQ_CT_TDVM_DONXINVAY: return "INQ.CT.TDVM_DONXINVAY";
                default: return "";
            }
        }


        /// <summary>
        /// Danh sách Loại địa bàn
        /// Dùng chung
        /// </summary> 
        public enum DanhSachLoaiDiaBan
        {
            TINH_THANHPHO,
            HUYEN_QUAN,
            XA_PHUONG,
            LANG_TODP

        }

        public static string getValue(this DanhSachLoaiDiaBan danhSachLoaiDiaBan)
        {
            switch (danhSachLoaiDiaBan)
            {
                case DanhSachLoaiDiaBan.TINH_THANHPHO: return "TINH_THANHPHO";
                case DanhSachLoaiDiaBan.HUYEN_QUAN: return "HUYEN_QUAN";
                case DanhSachLoaiDiaBan.XA_PHUONG: return "XA_PHUONG";
                case DanhSachLoaiDiaBan.LANG_TODP: return "LANG_TODP";
                default: return "";
            }
        }

        /// <summary>
        /// Danh sách Loại địa bàn chi tiet
        /// Dùng chung
        /// </summary> 
        public enum DanhSachLoaiDiaBanChiTiet
        {
            TINH,
            THANHPHO,
            HUYEN,
            QUAN,
            XA,
            PHUONG,
            LANG,
            TODP

        }

        public static string getValue(this DanhSachLoaiDiaBanChiTiet danhSachLoaiDiaBanChiTiet)
        {
            switch (danhSachLoaiDiaBanChiTiet)
            {
                case DanhSachLoaiDiaBanChiTiet.TINH: return "TINH";
                case DanhSachLoaiDiaBanChiTiet.THANHPHO: return "THANHPHO";
                case DanhSachLoaiDiaBanChiTiet.HUYEN: return "HUYEN";
                case DanhSachLoaiDiaBanChiTiet.QUAN: return "QUAN";
                case DanhSachLoaiDiaBanChiTiet.XA: return "XA";
                case DanhSachLoaiDiaBanChiTiet.PHUONG: return "PHUONG";
                case DanhSachLoaiDiaBanChiTiet.LANG: return "LANG";
                case DanhSachLoaiDiaBanChiTiet.TODP: return "TODP";
                default: return "";
            }
        }

        public enum LoaiVungMien { VUNG, MIEN };
        public static string getValue(this LoaiVungMien loaiVungMien)
        {
            switch (loaiVungMien)
            {
                case LoaiVungMien.VUNG: return "VUNG";
                case LoaiVungMien.MIEN: return "MIEN";
                default: return "";
            }
        }

        public enum ToChucDonVi { DVI, HSO, CNH, VPGD, PGD, VDGD, DGD };
        public static string getValue(this ToChucDonVi toChucDonVi)
        {
            switch (toChucDonVi)
            {
                case ToChucDonVi.DVI: return "DVI";
                case ToChucDonVi.HSO: return "HSO";
                case ToChucDonVi.CNH: return "CNH";
                case ToChucDonVi.VPGD: return "VPGD";
                case ToChucDonVi.PGD: return "PGD";
                case ToChucDonVi.VDGD: return "VDGD";
                case ToChucDonVi.DGD: return "DGD";
                default: return "";
            }
        }
        public static string layNgonNguLoaiDonVi(string trangthai)
        {
            if (trangthai == getValue(ToChucDonVi.DVI))
            {
                return LLanguage.SearchResourceByKey("U.DMUC_GTRI.THUOC_TINH_DON_VI.DVI");
            }
            else if (trangthai == getValue(ToChucDonVi.HSO))
            {
                return LLanguage.SearchResourceByKey("U.DMUC_GTRI.THUOC_TINH_DON_VI.HSO");
            }
            else if (trangthai == getValue(ToChucDonVi.CNH))
            {
                return LLanguage.SearchResourceByKey("U.DMUC_GTRI.THUOC_TINH_DON_VI.CNH");
            }
            else if (trangthai == getValue(ToChucDonVi.VPGD))
            {
                return LLanguage.SearchResourceByKey("U.DMUC_GTRI.THUOC_TINH_DON_VI.VPGD");
            }
            else if (trangthai == getValue(ToChucDonVi.PGD))
            {
                return LLanguage.SearchResourceByKey("U.DMUC_GTRI.THUOC_TINH_DON_VI.PGD");
            }
            else if (trangthai == getValue(ToChucDonVi.VDGD))
            {
                return LLanguage.SearchResourceByKey("U.DMUC_GTRI.THUOC_TINH_DON_VI.VDGD");
            }
            else if (trangthai == getValue(ToChucDonVi.DGD))
            {
                return LLanguage.SearchResourceByKey("U.DMUC_GTRI.THUOC_TINH_DON_VI.DGD");
            }

            return "";
        }

        public enum ToChucKhachHang { KHUVUC, CUM, NHOM };
        public static string getValue(this ToChucKhachHang toChucKhachHang)
        {
            switch (toChucKhachHang)
            {
                case ToChucKhachHang.KHUVUC: return "KHUVUC";
                case ToChucKhachHang.CUM: return "CUM";
                case ToChucKhachHang.NHOM: return "NHOM";
                default: return "";
            }
        }

        /// <summary>
        /// Nguồn tạo dữ liệu
        /// </summary>
        public enum NguonTaoDuLieu { NSD, HTH };
        public static string layGiaTri(this NguonTaoDuLieu nguonTaoDuLieu)
        {
            switch (nguonTaoDuLieu)
            {
                case NguonTaoDuLieu.NSD: return "NSD";
                case NguonTaoDuLieu.HTH: return "HTH";
                default: return "";
            }
        }

        /// <summary>
        /// Lấy ngôn ngữ cho action
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public static string layNgonNgu(this Action item)
        {
            switch (item)
            {
                case Action.THEM: return "M.DungChung.Action.Them";
                case Action.SUA: return "M.DungChung.Action.Sua";
                case Action.XOA: return "M.DungChung.Action.Xoa";
                case Action.DUYET: return "M.DungChung.Action.Duyet";
                case Action.TU_CHOI_DUYET: return "M.DungChung.Action.TuChoiDuyet";
                case Action.THOAI_DUYET: return "M.DungChung.Action.ThoaiDuyet";
                case Action.TINH_TOAN: return "M.DungChung.Action.TinhToan";
                case Action.TINH_TOAN2: return "M.DungChung.Action.TinhToan";
                case Action.TINH_TOAN3: return "M.DungChung.Action.TinhToan";
                case Action.TINH_TOAN4: return "M.DungChung.Action.TinhToan";
                case Action.TINH_TOAN5: return "M.DungChung.Action.TinhToan";
                case Action.TINH_TOAN_DU_CHI: return "M.DungChung.Action.TinhToanDuChi";
                case Action.TINH_TOAN_DU_THU_TRONG_HAN: return "M.DungChung.Action.TinhToanDuThu";
                case Action.TINH_TOAN_DU_THU_QUA_HAN: return "M.DungChung.Action.TinhToanDuThu";
                case Action.TINH_TOAN_LAI_SUAT: return "M.DungChung.Action.TinhToanLaiSuat";
                case Action.TINH_TOAN_KY_HAN: return "M.DungChung.Action.TinhToanKyHan";
                case Action.TINH_TOAN_LICH_TRA_NO: return "M.DungChung.Action.TinhToanLichTraNo";
                case Action.TINH_TOAN_SO_TIEN_VAY: return "M.DungChung.Action.TinhToanSoTienVay";
                case Action.TINH_TOAN_TRICH_LAP_DU_PHONG: return "M.DungChung.Action.TinhToanTrichLapDuPhong";
                case Action.LOAD: return "M.DungChung.Action.Load";
                case Action.LOAD_DATA: return "M.DungChung.Action.LoadData";
                case Action.GET_BY_ID: return "M.DungChung.Action.GetById";
                case Action.GET_BY_MA: return "M.DungChung.Action.GetByMa";
                case Action.KIEM_TRA: return "M.DungChung.Action.KiemTra";
                default: return "";
            }
        }

        // Đặc thù tổ chức khách hàng: cấp độ cao nhất trong tổ chức khách hàng là gì?
        public static ToChucKhachHang TO_CHUC_KHACH_HANG_ROOT = ToChucKhachHang.CUM;

        // Đặc thù quản lý khách hàng ở cấp độ nào trong tổ chức đơn vị?
        public static ToChucDonVi TO_CHUC_DON_VI_KHACH_HANG = ToChucDonVi.PGD;

        // Mã tổ chức
        public static string MA_TOCHUC = "00";

        // ID tổ chức
        public static int ID_TOCHUC = 1;

        // Đóng form chi tiết sau khi thêm/sửa
        public static bool CLOSE_DETAIL_FORM = false;

        // Mã hội sở chính
        public static string MA_HSO = "0000";

        // ID hội sở chính
        public static int ID_HSO = 2;

        public enum PHUONG_PHAP_HACH_TOAN
        {
            DOC_LAP,
            BAO_SO
        };

        public static string layGiaTri(this PHUONG_PHAP_HACH_TOAN nguonTaoDuLieu)
        {
            switch (nguonTaoDuLieu)
            {
                case PHUONG_PHAP_HACH_TOAN.DOC_LAP: return "DOC_LAP";
                case PHUONG_PHAP_HACH_TOAN.BAO_SO: return "BAO_SO";
                default: return "";
            }
        }

        public enum TINH_NANG
        {
            XEM,
            THEM,
            SUA,
            XOA,
            DUYET,
            HUY_DUYET,
            TU_CHOI,
            NHAN_BAN,
            IN,
            XUAT_DU_LIEU,
            NHAP_DU_LIEU,
            LUU_TAM,
            LUU,
            TIM_KIEM,
            LAY_LAI,
            TRO_GIUP,
            DONG,
            SAO_CHEP,
            CAT,
            DAN,
            LOC,
            XEM_TRUOC,
            DONG_BO,
            KET_NOI,
            HUY_KET_NOI,
            GOI_POPUP,
            TONG_HOP,
            TAO_DU_LIEU,
            SINH_DU_LIEU,
            TINH_TOAN,
            THUC_HIEN,
            XU_LY,
            CAU_HINH,
            TAI_DU_LIEU_XUONG,
            TAI_DU_LIEU_LEN,
            DON_DEP_DU_LIEU,
            SAO_LUU_DU_LIEU,
            PHUC_HOI_DU_LIEU,
            BANG_KE_TIEN_MAT,
            TOAN_QUYEN
        }
        public static string layGiaTri(this TINH_NANG tinhNang)
        {
            switch (tinhNang)
            {
                case TINH_NANG.XEM: return "View";
                case TINH_NANG.THEM: return "Add";
                case TINH_NANG.SUA: return "Modify";
                case TINH_NANG.XOA: return "Delete";
                case TINH_NANG.DUYET: return "Approve";
                case TINH_NANG.HUY_DUYET: return "Cancel";
                case TINH_NANG.TU_CHOI: return "Refuse";
                case TINH_NANG.NHAN_BAN: return "Clone";
                case TINH_NANG.IN: return "Print";
                case TINH_NANG.XUAT_DU_LIEU: return "Export";
                case TINH_NANG.NHAP_DU_LIEU: return "Import";
                case TINH_NANG.LUU_TAM: return "Hold";
                case TINH_NANG.LUU: return "Save";
                case TINH_NANG.TIM_KIEM: return "Search";
                case TINH_NANG.LAY_LAI: return "Reload";
                case TINH_NANG.TRO_GIUP: return "Help";
                case TINH_NANG.DONG: return "Close";
                case TINH_NANG.SAO_CHEP: return "Copy";
                case TINH_NANG.CAT: return "Cut";
                case TINH_NANG.DAN: return "Paste";
                case TINH_NANG.LOC: return "Filter";
                case TINH_NANG.XEM_TRUOC: return "Preview";
                case TINH_NANG.DONG_BO: return "Synch";
                case TINH_NANG.KET_NOI: return "Connect";
                case TINH_NANG.HUY_KET_NOI: return "Disconnect";
                case TINH_NANG.GOI_POPUP: return "Popup";
                case TINH_NANG.TONG_HOP: return "Collect";
                case TINH_NANG.TAO_DU_LIEU: return "Make";
                case TINH_NANG.SINH_DU_LIEU: return "Generate";
                case TINH_NANG.TINH_TOAN: return "Caculate";
                case TINH_NANG.THUC_HIEN: return "Execute";
                case TINH_NANG.XU_LY: return "Process";
                case TINH_NANG.CAU_HINH: return "Config";
                case TINH_NANG.TAI_DU_LIEU_XUONG: return "Download";
                case TINH_NANG.TAI_DU_LIEU_LEN: return "Upload";
                case TINH_NANG.DON_DEP_DU_LIEU: return "Clean";
                case TINH_NANG.SAO_LUU_DU_LIEU: return "Backup";
                case TINH_NANG.PHUC_HOI_DU_LIEU: return "Restore";
                case TINH_NANG.BANG_KE_TIEN_MAT: return "CashStmt";
                case TINH_NANG.TOAN_QUYEN: return "FullControl";

                default: return "";
            }
        }

        public enum LOAI_DMUC_TSAN
        {
            LOAI_TS,
            NHOM_TS,
            NGUON_GOC,
            PP_KHAU_HAO,
            TINH_TRANG,
            HT_BAN_GIAO,
            HT_PHAN_BO,
            NN_GIAM_TS,
            NN_DANH_GIA_LAI,
            LOAI_DANH_GIA,
            PHONG_BAN,
            HINH_THUC_NHAP,
            DON_VI_TINH,
            THONG_TIN_KHAC,
            KHOAN_MUC_NGUYEN_GIA,
            HT_THANH_TOAN
        }
        public static string layGiaTri(this LOAI_DMUC_TSAN loaiDM)
        {
            switch (loaiDM)
            {
                case LOAI_DMUC_TSAN.LOAI_TS: return "LOAI_TS";
                case LOAI_DMUC_TSAN.NHOM_TS: return "NHOM_TS";
                case LOAI_DMUC_TSAN.NGUON_GOC: return "NGUON_GOC";
                case LOAI_DMUC_TSAN.PP_KHAU_HAO: return "PP_KHAU_HAO";
                case LOAI_DMUC_TSAN.TINH_TRANG: return "TINH_TRANG";
                case LOAI_DMUC_TSAN.HT_BAN_GIAO: return "HT_BAN_GIAO";
                case LOAI_DMUC_TSAN.HT_PHAN_BO: return "HT_PHAN_BO";
                case LOAI_DMUC_TSAN.NN_GIAM_TS: return "NN_GIAM_TS";
                case LOAI_DMUC_TSAN.NN_DANH_GIA_LAI: return "NN_DANH_GIA_LAI";
                case LOAI_DMUC_TSAN.LOAI_DANH_GIA: return "LOAI_DANH_GIA";
                case LOAI_DMUC_TSAN.PHONG_BAN: return "PHONG_BAN";
                case LOAI_DMUC_TSAN.HINH_THUC_NHAP: return "HINH_THUC_NHAP";
                case LOAI_DMUC_TSAN.DON_VI_TINH: return "DON_VI_TINH";
                case LOAI_DMUC_TSAN.THONG_TIN_KHAC: return "THONG_TIN_KHAC";
                case LOAI_DMUC_TSAN.KHOAN_MUC_NGUYEN_GIA: return "KHOAN_MUC_NGUYEN_GIA";
                case LOAI_DMUC_TSAN.HT_THANH_TOAN: return "HT_THANH_TOAN";

                default: return "";
            }
        }

        public static LOAI_DMUC_TSAN layLoaiDanhMucTS(string loaiDM)
        {
            switch (loaiDM)
            {
                case "LOAI_TS": return LOAI_DMUC_TSAN.LOAI_TS;
                case "NHOM_TS": return LOAI_DMUC_TSAN.NHOM_TS;
                case "NGUON_GOC": return LOAI_DMUC_TSAN.NGUON_GOC;
                case "PP_KHAU_HAO": return LOAI_DMUC_TSAN.PP_KHAU_HAO;
                case "TINH_TRANG": return LOAI_DMUC_TSAN.TINH_TRANG;
                case "HT_BAN_GIAO": return LOAI_DMUC_TSAN.HT_BAN_GIAO;
                case "HT_PHAN_BO": return LOAI_DMUC_TSAN.HT_PHAN_BO;
                case "NN_GIAM_TS": return LOAI_DMUC_TSAN.NN_GIAM_TS;
                case "NN_DANH_GIA_LAI": return LOAI_DMUC_TSAN.NN_DANH_GIA_LAI;
                case "LOAI_DANH_GIA": return LOAI_DMUC_TSAN.LOAI_DANH_GIA;
                case "PHONG_BAN": return LOAI_DMUC_TSAN.PHONG_BAN;
                case "HINH_THUC_NHAP": return LOAI_DMUC_TSAN.HINH_THUC_NHAP;
                case "DON_VI_TINH": return LOAI_DMUC_TSAN.DON_VI_TINH;
                case "THONG_TIN_KHAC": return LOAI_DMUC_TSAN.THONG_TIN_KHAC;
                case "KHOAN_MUC_NGUYEN_GIA": return LOAI_DMUC_TSAN.KHOAN_MUC_NGUYEN_GIA;
                case "HT_THANH_TOAN": return LOAI_DMUC_TSAN.HT_THANH_TOAN;

                default: return LOAI_DMUC_TSAN.LOAI_TS;
            }
        }


        public enum DanhSachXuatExcel
        {
            KHTV_DANH_SACH_KH,
            HDVO_DANH_SACH_SO_TK,
            TDVM_DANH_SACH_HDTD,
            TDVM_DANH_SACH_KUOC,
            GDKT_KIEM_SOAT
        }
        public static string LayGiaTri(this DanhSachXuatExcel loai)
        {
            switch (loai)
            {
                case DanhSachXuatExcel.KHTV_DANH_SACH_KH: return "KHTV_DANH_SACH_KH";
                case DanhSachXuatExcel.HDVO_DANH_SACH_SO_TK: return "HDVO_DANH_SACH_SO_TK";
                case DanhSachXuatExcel.TDVM_DANH_SACH_HDTD: return "TDVM_DANH_SACH_HDTD";
                case DanhSachXuatExcel.TDVM_DANH_SACH_KUOC: return "TDVM_DANH_SACH_KUOC";
                case DanhSachXuatExcel.GDKT_KIEM_SOAT: return "GDKT_KIEM_SOAT";

                default: return "";
            }
        }

        
    }
}
