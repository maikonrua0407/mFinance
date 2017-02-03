using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PresentationWPF.TinDung
{
    public class XuLyNoQuaHan
    {
        public class TSDBXuLy
        {
            int _iD;

            public int ID
            {
                get { return _iD; }
                set { _iD = value; }
            }
            string _mA_TSDB;

            public string MA_TSDB
            {
                get { return _mA_TSDB; }
                set { _mA_TSDB = value; }
            }
            string _tEN_TSDB;

            public string TEN_TSDB
            {
                get { return _tEN_TSDB; }
                set { _tEN_TSDB = value; }
            }
            string _nGAY_PHAT_MAI;

            public string NGAY_PHAT_MAI
            {
                get { return _nGAY_PHAT_MAI; }
                set { _nGAY_PHAT_MAI = value; }
            }
            decimal _gIA_TRI_DA_SDUNG;

            public decimal GIA_TRI_DA_SDUNG
            {
                get { return _gIA_TRI_DA_SDUNG; }
                set { _gIA_TRI_DA_SDUNG = value; }
            }
            decimal _gIA_TRI_SU_DUNG;

            public decimal GIA_TRI_SU_DUNG
            {
                get { return _gIA_TRI_SU_DUNG; }
                set { _gIA_TRI_SU_DUNG = value; }
            }
        }
        public class KheUocXuLyNo
        {
            int _iD;

            public int ID
            {
                get { return _iD; }
                set { _iD = value; }
            }
            string _mA_KUOC;

            public string MA_KUOC
            {
                get { return _mA_KUOC; }
                set { _mA_KUOC = value; }
            }
            int _iD_KHANG;

            public int ID_KHANG
            {
                get { return _iD_KHANG; }
                set { _iD_KHANG = value; }
            }
            string _mA_KHANG;

            public string MA_KHANG
            {
                get { return _mA_KHANG; }
                set { _mA_KHANG = value; }
            }
            string _tEN_KHANG;

            public string TEN_KHANG
            {
                get { return _tEN_KHANG; }
                set { _tEN_KHANG = value; }
            }
            string _nGAY_VAY;

            public string NGAY_VAY
            {
                get { return _nGAY_VAY; }
                set { _nGAY_VAY = value; }
            }
            int _tHOI_HAN_VAY;

            public int THOI_HAN_VAY
            {
                get { return _tHOI_HAN_VAY; }
                set { _tHOI_HAN_VAY = value; }
            }
            string _tHOI_HAN_DVI;

            public string THOI_HAN_DVI
            {
                get { return _tHOI_HAN_DVI; }
                set { _tHOI_HAN_DVI = value; }
            }
            decimal _lSuat;

            public decimal LSuat
            {
                get { return _lSuat; }
                set { _lSuat = value; }
            }
            string _nGAY_DHAN;

            public string NGAY_DHAN
            {
                get { return _nGAY_DHAN; }
                set { _nGAY_DHAN = value; }
            }
            decimal _sO_TIEN_VAY;

            public decimal SO_TIEN_VAY
            {
                get { return _sO_TIEN_VAY; }
                set { _sO_TIEN_VAY = value; }
            }
            decimal _dU_NO_GOC;

            public decimal DU_NO_GOC
            {
                get { return _dU_NO_GOC; }
                set { _dU_NO_GOC = value; }
            }
            decimal _dU_NO_LAI;

            public decimal DU_NO_LAI
            {
                get { return _dU_NO_LAI; }
                set { _dU_NO_LAI = value; }
            }
            string _nHOM_NO_HT;

            public string NHOM_NO_HT
            {
                get { return _nHOM_NO_HT; }
                set { _nHOM_NO_HT = value; }
            }
        }

        List<TSDBXuLy> _dANH_SACH_TSDB;

        public List<TSDBXuLy> DANH_SACH_TSDB
        {
            get { return _dANH_SACH_TSDB; }
            set { _dANH_SACH_TSDB = value; }
        }

        KheUocXuLyNo _KheUocXuLy;

        public KheUocXuLyNo KheUocXuLy
        {
            get { return _KheUocXuLy; }
            set { _KheUocXuLy = value; }
        }
        decimal _xU_LY_NO_GOC;

        public decimal XU_LY_NO_GOC
        {
            get { return _xU_LY_NO_GOC; }
            set { _xU_LY_NO_GOC = value; }
        }
        decimal _xU_LY_NO_LAI;

        public decimal XU_LY_NO_LAI
        {
            get { return _xU_LY_NO_LAI; }
            set { _xU_LY_NO_LAI = value; }
        }
        decimal _dU_PHONG_CU_THE;

        public decimal DU_PHONG_CU_THE
        {
            get { return _dU_PHONG_CU_THE; }
            set { _dU_PHONG_CU_THE = value; }
        }
        decimal _dU_PHONG_CHUNG;

        public decimal DU_PHONG_CHUNG
        {
            get { return _dU_PHONG_CHUNG; }
            set { _dU_PHONG_CHUNG = value; }
        }
        decimal _cHI_PHI;

        public decimal CHI_PHI
        {
            get { return _cHI_PHI; }
            set { _cHI_PHI = value; }
        }
        string _tHEO_DOI_NB;

        public string THEO_DOI_NB
        {
            get { return _tHEO_DOI_NB; }
            set { _tHEO_DOI_NB = value; }
        }
    }
}
