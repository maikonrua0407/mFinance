using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace PresentationWPF.BaoCao.DungChung
{
    public class ThamSoBaoCao
    {
        public string MaThamSo;
        public string GiaTriThamSo;
        public string LoaiThamSo;
        public List<string> ListGiaTriThamSo;
        public DataTable DtThamSo;
        public DataSet DsThamSo;

        public ThamSoBaoCao(string maThamSo,
            string giaTriThamSo,
            string loaiThamSo)
        {
            this.MaThamSo = maThamSo;
            this.GiaTriThamSo = giaTriThamSo;
            this.LoaiThamSo = loaiThamSo;
        }

        public ThamSoBaoCao(string maThamSo,
            DataTable giaTriThamSo,
            string loaiThamSo)
        {
            this.MaThamSo = maThamSo;
            this.DtThamSo = giaTriThamSo;
            this.LoaiThamSo = loaiThamSo;
        }

        public ThamSoBaoCao(string maThamSo,
            string giaTriThamSo,
            string loaiThamSo,
            List<string> ListGiaTriThamSo)
        {
            this.MaThamSo = maThamSo;
            this.GiaTriThamSo = giaTriThamSo;
            this.LoaiThamSo = loaiThamSo;
            this.ListGiaTriThamSo = ListGiaTriThamSo;
        }

        public ThamSoBaoCao(string maThamSo,
            string giaTriThamSo,
            string loaiThamSo,
            List<string> ListGiaTriThamSo,
            DataSet dsThamSo)
        {
            this.MaThamSo = maThamSo;
            this.GiaTriThamSo = giaTriThamSo;
            this.LoaiThamSo = loaiThamSo;
            this.ListGiaTriThamSo = ListGiaTriThamSo;
            this.DsThamSo = dsThamSo;
        }
    }
}
