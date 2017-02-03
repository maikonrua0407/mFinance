using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Presentation.Process;
using Presentation.Process.Common;
using Presentation.Process.QuanTriHeThongServiceRef;
using PresentationAspNet.MVC.Models;
using Utilities.Common;

namespace PresentationAspNet.MVC.Controllers.QuanLyNsd
{
    [Common.SessionAuthorizeAttribute]
    public class NhNsdDsController : Controller
    {
        //
        // GET: /NhNsdDs/

        List<AutoCompleteEntry> lstSourceLoaiDTuong = new List<AutoCompleteEntry>();
        QuanTriHeThongProcess qtht = new QuanTriHeThongProcess();

        public ActionResult Index()
        {
            try
            {
                lstSourceLoaiDTuong = new List<AutoCompleteEntry>();
                // lấy dữ liệu đổ source cho combobox Loại đối tượng
                List<string> lstDieuKien = new List<string>();
                lstDieuKien.Add(DatabaseConstant.DanhMuc.LOAI_DTUONG_KTHAC_TNGUYEN.getValue());
                DataCombobox.GenAutoComboBox(ref lstSourceLoaiDTuong, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien);

                var model = new SysUsers { LstSourceLoaiDTuong = lstSourceLoaiDTuong };
                model.LstNhNsd = qtht.layNhomNSD(ClientInformation.LoaiNguoiSuDung);
                return View(model);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public ActionResult List(string type)
        {
            try
            {
                var model = new SysUsers { LstSourceLoaiDTuong = lstSourceLoaiDTuong };
                if (type.Equals("NHNSD"))
                    model.LstNhNsd = qtht.layNhomNSD(ClientInformation.LoaiNguoiSuDung);
                else if (type.Equals("NSD"))
                    model.LstNsd = qtht.layNSD(ClientInformation.LoaiNguoiSuDung);
                return View(model);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public ActionResult DetailNhNsd(string id)
        {
            try
            {
                var model = qtht.layNhomNSD(ClientInformation.LoaiNguoiSuDung).FirstOrDefault(e => e.ID == id.Trim().StringToInt32());
                return View(model ?? new HT_NHNSD());
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }
        public ActionResult DetailNsd(string id)
        {
            try
            {
                var model = qtht.layNSD(ClientInformation.LoaiNguoiSuDung).FirstOrDefault(e => e.ID == id.Trim().StringToInt32());
                return View(model ?? new HT_NSD());
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public JsonResult Save(string id, string maNhnsd, string tenNhnsd, string moTa, string maDviQly, bool hanCheTruyCap)
        {
            try
            {
                string msg = "";
                var obj = qtht.layNhomNSD(ClientInformation.LoaiNguoiSuDung).FirstOrDefault(e => e.ID == id.Trim().StringToInt32());

                if (obj != null)
                {
                    obj.MA_NHNSD = maNhnsd;
                    obj.TEN_NHNSD = tenNhnsd;
                    obj.MO_TA = moTa;
                    obj.MA_DVI_QLY = maDviQly;
                    obj.HAN_CHE_TRUY_CAP =
                        hanCheTruyCap == true
                            ? BusinessConstant.CoKhong.CO.layGiaTri()
                            : BusinessConstant.CoKhong.KHONG.layGiaTri();
                    obj.NGAY_CNHAT = (DateTime.Today).ToString("yyyyMMdd");
                    if (obj.ID == 0)
                        obj.NGAY_NHAP = (DateTime.Today).ToString("yyyyMMdd");
                    obj.NGUOI_CNHAT = ClientInformation.TenDangNhap;
                    if (obj.ID == 0)
                        obj.NGUOI_NHAP = ClientInformation.TenDangNhap;
                    string strTtnv = string.Empty;
                    var action = DatabaseConstant.Action.SUA;
                    if (obj.ID == 0)
                    {
                        obj.NGUON_TAO_DL = "NSD";
                        obj.PVI_KTHAC = "K";
                        strTtnv = obj.TTHAI_NVU;
                        action = DatabaseConstant.Action.THEM;
                    }
                    obj.TTHAI_NVU = Common.LayTrangThaiBanGhi(action, BusinessConstant.layTrangThaiNghiepVu(strTtnv));
                    obj.TTHAI_BGHI = BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri();

                    // Luôn là đã duyệt (???)
                    obj.TTHAI_NVU = BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri();
                    OnSave(ref msg, obj);
                }

                return Json(msg.IsNullOrEmpty() ? Common.ResultJson.Success.LayMa() : msg, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public JsonResult DeleteNhNsd(string id)
        {
            try
            {
                string msg = "";
                foreach (var item in id.Split(','))
                {
                    BeforeDelete(ref msg, item.StringToInt32());
                }

                return Json(msg.IsNullOrEmpty() ? Common.ResultJson.Success.LayMa() : msg, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }

        public void OnSave(ref string msg, HT_NHNSD obj)
        {
            try
            {
                if (Validate(ref msg, obj))
                {
                    QuanTriHeThongProcess process = new QuanTriHeThongProcess();
                    List<string> lstStrIdNSD = new List<string>(); //(from row in dt.AsEnumerable() select row.Field<string>("ID")).Distinct().ToList();
                    List<int> lstIdNSD = lstStrIdNSD.Select(i => i.StringToInt32()).ToList();
                    ApplicationConstant.ResponseStatus ret = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                    var lstTruyCap = new List<HT_TRUY_CAP>();
                    string responseMessage = null;

                    // Nếu là thêm mới
                    if (obj.ID == 0)
                    {
                        ret = process.ThemNHNSD(ref obj, lstIdNSD, lstTruyCap, ref responseMessage);
                        afterAddNew(ref msg, ret, obj, responseMessage);
                    }
                    else
                    {
                        ret = process.SuaNHNSD(ref obj, lstIdNSD, lstTruyCap, ref responseMessage);
                        afterModify(ref msg, ret, obj, responseMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                msg = LanguageNode.GetValueMessageLanguage(ex.Message);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        private bool Validate(ref string msg, HT_NHNSD obj)
        {
            if (obj.MA_DVI_QLY.IsNullOrEmpty())
            {
                msg = "Chưa chọn Đơn vị";
                return false;
            }

            if (obj.MA_NHNSD.IsNullOrEmptyOrSpace())
            {
                msg = "Chưa nhập Mã";
                return false;
            }

            if (obj.TEN_NHNSD.IsNullOrEmptyOrSpace())
            {
                msg = "Chưa nhập Tên";
                return false;
            }

            return true;
        }

        /// <summary>
        /// Trước khi sửa
        /// </summary>
        public JsonResult BeforeModifyNhNsd(int id)
        {
            string msg = "";
            // Yêu cầu lock bản ghi cần sửa
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(id);

            bool ret = process.LockData(DatabaseConstant.Module.QTHT,
                DatabaseConstant.Function.HT_NHNSD,
                DatabaseConstant.Table.HT_NHNSD,
                DatabaseConstant.Action.SUA,
                listLockId);

            // Nếu lock thành công >> cho phép sửa
            if (!ret)
            {
                msg = LanguageNode.GetValueMessageLanguage("M.ResponseMessage.Common.LockDataInvalid");
            }
            return Json(msg.IsNullOrEmpty() ? Common.ResultJson.Success.LayMa() : msg, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Trước khi sửa
        /// </summary>
        public JsonResult BeforeModifyNsd(int id)
        {
            string msg = "";
            // Yêu cầu lock bản ghi cần sửa
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(id);

            bool ret = process.LockData(DatabaseConstant.Module.QTHT,
                DatabaseConstant.Function.HT_NSD,
                DatabaseConstant.Table.HT_NSD,
                DatabaseConstant.Action.SUA,
                listLockId);

            // Nếu lock thành công >> cho phép sửa
            if (!ret)
            {
                msg = LanguageNode.GetValueMessageLanguage("M.ResponseMessage.Common.LockDataInvalid");
            }
            return Json(msg.IsNullOrEmpty() ? Common.ResultJson.Success.LayMa() : msg, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Trước khi xóa
        /// </summary>
        private void BeforeDelete(ref string msg, int id)
        {
            // Lock dữ liệu nếu người dùng chấp nhận cảnh báo
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(id);
            List<int> listLockedId = new List<int>();

            bool retLockData = process.LockData(DatabaseConstant.Module.QTHT,
                DatabaseConstant.Function.HT_NHNSD,
                DatabaseConstant.Table.HT_NHNSD,
                DatabaseConstant.Action.XOA,
                listLockId);

            // Nếu lock thành công >> cho phép sửa
            if (retLockData)
            {
                // Gọi tới hàm xóa dữ liệu
                OnDelete(ref msg, id);
                return;
            }
            // Nếu lock không thành công >> cảnh báo
            else
            {
                msg = LanguageNode.GetValueMessageLanguage("M.ResponseMessage.Common.LockDataInvalid");
                return;
            }
        }

        /// <summary>
        /// Xóa dữ liệu
        /// </summary>
        private void OnDelete(ref string msg, int id)
        {
            QuanTriHeThongProcess process = new QuanTriHeThongProcess();
            int[] arrayID = new int[0];
            try
            {
                Array.Resize(ref arrayID, arrayID.Length + 1);
                arrayID[arrayID.Length - 1] = id;

                List<ClientResponseDetail> listClientResponseDetail = new List<ClientResponseDetail>();
                bool ret = process.XoaListNHNSD(arrayID, ref listClientResponseDetail);

                AfterDelete(ref msg, id, ret, listClientResponseDetail);
            }
            catch (Exception ex)
            {
                msg = LanguageNode.GetValueMessageLanguage(ex.Message);
                LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        /// <summary>
        /// Sau khi thêm mới
        /// </summary>
        /// <param name="ret"></param>
        private void afterAddNew(ref string msg, ApplicationConstant.ResponseStatus ret, HT_NHNSD obj, string responseMessage, bool multiAdd = false)
        {
            if (ret == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                msg = "S#" + LanguageNode.GetValueMessageLanguage("M.DungChung.ThemThanhCong");

                if (multiAdd)
                {

                }
                else if (!DatabaseConstant.CLOSE_DETAIL_FORM)
                {

                }
                else
                {

                }
            }
            else
            {
                msg = "E#" + LanguageNode.GetValueMessageLanguage(responseMessage);
            }
        }

        /// <summary>
        /// Sau khi sửa
        /// </summary>
        /// <param name="ret"></param>
        private void afterModify(ref string msg, ApplicationConstant.ResponseStatus ret, HT_NHNSD obj, string responseMessage)
        {
            if (ret == ApplicationConstant.ResponseStatus.THANH_CONG)
            {
                msg = "S#" + LanguageNode.GetValueMessageLanguage("M.DungChung.CapNhatThanhCong");

                // Bind lại giao diện
            }
            else
            {
                msg = "E#" + LanguageNode.GetValueMessageLanguage(responseMessage);
            }

            // Yêu cầu Unlock bản ghi cần sửa
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(obj.ID);

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.QTHT,
                DatabaseConstant.Function.HT_NHNSD,
                DatabaseConstant.Table.HT_NHNSD,
                DatabaseConstant.Action.SUA,
                listLockId);
        }

        /// <summary>
        /// Sau khi xóa
        /// </summary>
        /// <param name="ret"></param>
        private void AfterDelete(ref string msg, int id, bool ret, List<ClientResponseDetail> listClientResponseDetail)
        {
            if (ret)
            {
                msg = "S#" + LanguageNode.GetValueMessageLanguage("M.DungChung.XoaThanhCong");
            }
            else
            {
                //msg = LanguageNode.GetValueMessageLanguage("M.DungChung.XoaKhongThanhCong", LMessage.MessageBoxType.Warning);
                //CommonFunction.ThongBaoKetQua(listClientResponseDetail);
                msg = "E#";
            }

            // Yêu cầu unlock dữ liệu
            UtilitiesProcess process = new UtilitiesProcess();
            List<int> listLockId = new List<int>();
            listLockId.Add(id);
            List<int> listUnlockId = new List<int>();

            bool retUnlockData = process.UnlockData(DatabaseConstant.Module.QTHT,
                DatabaseConstant.Function.HT_NHNSD,
                DatabaseConstant.Table.HT_NHNSD,
                DatabaseConstant.Action.XOA,
                listLockId);
        }
    }
}
