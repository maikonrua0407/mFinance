using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataModel.EntityFramework;

namespace DataServices.QuanTriHeThong
{
    public class DS_CNANG
    {
        /// <summary>
        /// Lấy thông tin chức năng theo mã
        /// </summary>
        /// <param name="maChucNang">mã của chức năng</param>
        /// <returns>1 đối tượng chức năng</returns>
        public HT_CNANG LayChucNangTheoMa(string maChucNang)
        {
                // Entities entities = new Entities();
                Entities entities = ContextFactory.GetInstance();
                return entities.HT_CNANG.FirstOrDefault(e => e.MA_CNANG.Equals(maChucNang));
        }
        /// <summary>
        /// Lấy thông tin các chức năng theo mã chức năng cha
        /// </summary>
        /// <param name="maChucNangCha">mã của chức năng cha</param>
        /// <returns>danh sách đối tượng chức năng</returns>
        public List<HT_CNANG> LayChucNangTheoMaCha(string maChucNangCha)
        {
                //Entities entities = new Entities();
                Entities entities = ContextFactory.GetInstance();
                int idChucNangCha = entities.HT_CNANG.FirstOrDefault(e => e.MA_CNANG.Equals(maChucNangCha)).ID;
                return entities.HT_CNANG.Where(e => e.ID_CNANG_CHA == idChucNangCha).ToList();

        }

        /// <summary>
        /// Lấy Chức năng theo NSD
        /// </summary>
        /// <param name="maDangNhap">Mã đăng nhập</param>
        /// <param name="maDonVi">Mã đơn vị quản lý</param>
        /// <returns>List chức năng</returns>
        public List<HT_CNANG> LayChucNangTheoNSD(string maDangNhap, string maDonVi)
        {
            // Entities entities = new Entities();
            Entities entities = ContextFactory.GetInstance();
            // Lấy ID NSD
            var id_NSD = entities.HT_NSD.FirstOrDefault(e => e.MA_DANG_NHAP.Equals(maDangNhap) && e.MA_DVI_QLY.Equals(maDonVi)).ID;

            DS_HT_TNGUYEN_KTHAC dsTNGUYENKTHAC = new DS_HT_TNGUYEN_KTHAC();
            var TNKT = dsTNGUYENKTHAC.LayTNguyenKThacNSD(id_NSD, maDonVi).Select(e => e.MA_TNGUYEN);

            // Lấy ds Tài nguyên
            var taiNguyen = entities.HT_TNGUYEN.Where(e => TNKT.Contains(e.MA_TNGUYEN)).Distinct().Select(e => e.GTRI_TNGUYEN);
            return entities.HT_CNANG.Where(e => taiNguyen.Contains(e.MA_CNANG)).ToList();
        }
    }
}
