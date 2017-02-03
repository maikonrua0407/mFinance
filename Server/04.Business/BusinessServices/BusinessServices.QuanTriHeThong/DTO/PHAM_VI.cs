using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessServices.QuanTriHeThong.DTO
{
    public class PHAM_VI
    {
        private string _mA_DTUONG_LOAI;
        private int _iD_DTUONG;
        private string _mA_DTUONG;
        private List<string> _phan_LOAI;
        private string _mA_PVI_LOAI;
        private List<int> _iD_PVI;
        private List<string> _mA_PVI;
        private string _tTHAI_BGHI;
        private string _tTHAI_NVU;
        private string _mA_DVI_QLY;
        private string _mA_DVI_TAO;
        private string _nGAY_NHAP;
        private string _nGUOI_NHAP;
        private string _nGAY_CNHAT;
        private string _nGUOI_CNHAT;

        public string MA_DTUONG_LOAI
        {
            get { return _mA_DTUONG_LOAI; }
            set { _mA_DTUONG_LOAI = value; }
        }

        public int ID_DTUONG
        {
            get { return _iD_DTUONG; }
            set { _iD_DTUONG = value; }
        }

        public string MA_DTUONG
        {
            get { return _mA_DTUONG; }
            set { _mA_DTUONG = value; }
        }

        public List<string> PHAN_LOAI
        {
            get { return _phan_LOAI; }
            set { _phan_LOAI = value; }
        }

        public string MA_PVI_LOAI
        {
            get { return _mA_PVI_LOAI; }
            set { _mA_PVI_LOAI = value; }
        }

        public List<int> ID_PVI
        {
            get { return _iD_PVI; }
            set { _iD_PVI = value; }
        }

        public List<string> MA_PVI
        {
            get { return _mA_PVI; }
            set { _mA_PVI = value; }
        }

        public string TTHAI_BGHI
        {
            get { return _tTHAI_BGHI; }
            set { _tTHAI_BGHI = value; }
        }

        public string TTHAI_NVU
        {
            get { return _tTHAI_NVU; }
            set { _tTHAI_NVU = value; }
        }

        public string MA_DVI_QLY
        {
            get { return _mA_DVI_QLY; }
            set { _mA_DVI_QLY = value; }
        }

        public string MA_DVI_TAO
        {
            get { return _mA_DVI_TAO; }
            set { _mA_DVI_TAO = value; }
        }

        public string NGAY_NHAP
        {
            get { return _nGAY_NHAP; }
            set { _nGAY_NHAP = value; }
        }

        public string NGUOI_NHAP
        {
            get { return _nGUOI_NHAP; }
            set { _nGUOI_NHAP = value; }
        }

        public string NGAY_CNHAT
        {
            get { return _nGAY_CNHAT; }
            set { _nGAY_CNHAT = value; }
        }

        public string NGUOI_CNHAT
        {
            get { return _nGUOI_CNHAT; }
            set { _nGUOI_CNHAT = value; }
        }

    }
}
