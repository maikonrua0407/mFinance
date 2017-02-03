using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PresentationAspNet.MVC
{
    public class UserInformation
    {
        public static Presentation.Process.Common.UserInformation Session_User
        {
            get
            {
                return HttpContext.Current.Session["UserInformation"] as Presentation.Process.Common.UserInformation;

            }
            set
            {
                HttpContext.Current.Session["UserInformation"] = value;
            }
        }
    }
    //#region Session

    //public class SessionUser
    //{
    //    public static Session_User Session_User
    //    {
    //        get
    //        {
    //            return HttpContext.Current.Session["Session_User"] as Session_User;

    //        }
    //        set
    //        {
    //            HttpContext.Current.Session["Session_User"] = value;
    //        }
    //    }
    //}
    //public class Session_User
    //{
    //    public string USER_ID { get; set; }
    //    public string USER_NAME { get; set; }
    //    public string FULL_NAME { get; set; }
    //    public string MAT_KHAU { get; set; }
    //    public int PHAI_DOI_MK { get; set; }
    //    public string DON_VI_TAO { get; set; }
    //    public int ID_DON_VI_TAO { get; set; }
    //    public string DON_VI_QLY { get; set; }
    //    public string LOAI_DON_VI { get; set; }
    //    public string TEN_DON_VI { get; set; }
    //    public Presentation.Process.ZAMainAppServiceRef.ChucNangDto[] LIST_CHUC_NANG { get; set; }
    //}


    //public class SessionTemp
    //{
    //    public static Session_SYS_User Session_SYS_User
    //    {
    //        get
    //        {
    //            return HttpContext.Current.Session["Session_SYS_User"] as Session_SYS_User;

    //        }
    //        set
    //        {
    //            HttpContext.Current.Session["Session_SYS_User"] = value;
    //        }
    //    }
    //}
    //public class Session_SYS_User
    //{
    //    public UserBo User { get; set; }
    //    public string IDKho { get; set; }
    //    public int CurrentYear { get; set; }
    //    public string FuncID { get; set; }
    //    public string BussinessId { get; set; }
    //    public string connection { get; set; }
    //    public KhoBo khoInfo { get; set; }
    //    public KhuVucBo KhuVucInfo { get; set; }
    //    public DiemBanBo diemBanInfo { get; set; }
    //    public ChiNhanhBo chiNhanhInfo { get; set; }
    //    public SYS_Bussiness Bussiness { get; set; }
    //    public OrganizationBo organization { get; set; }
    //    public List<KhoBo> lstKhoByUser { get; set; }
    //    public List<KhoBo> lstKhoLamViec { get; set; }
    //    public List<DiemBanBo> lstDiemBan { get; set; }
    //    public List<ConfigSystemBo> ConfigSystem { get; set; }
    //    public List<CauHinhNguoiDungBo> CauHinhUser { get; set; }
    //    public bool IsMobileBrowser { get; set; }
    //    public Session_SYS_User()
    //    {
    //    }

    //}
    
    //#endregion
}