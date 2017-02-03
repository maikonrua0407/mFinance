using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Presentation.Process.NhanSuServiceRef;
using PresentationWPF.CustomControl;

namespace PresentationWPF.NhanSu.Converts
{
    public static class ConvertNhanSu
    {
        public static List<AutoCompleteEntry> ToAutoCompleteEntry(List<NS_DM_BANG_CAP> lst)
        {
            List<AutoCompleteEntry> lstAutoCompleteEntry = new List<AutoCompleteEntry>();
            foreach (NS_DM_BANG_CAP obj in lst)
            {
                lstAutoCompleteEntry.Add(new AutoCompleteEntry(obj.TEN, obj.MA, obj.ID.ToString()));
            }
            return lstAutoCompleteEntry;
        }

        public static List<AutoCompleteEntry> ToAutoCompleteEntry(List<NS_DM_BVIEN_KCB> lst)
        {
            List<AutoCompleteEntry> lstAutoCompleteEntry = new List<AutoCompleteEntry>();
            foreach (NS_DM_BVIEN_KCB obj in lst)
            {
                lstAutoCompleteEntry.Add(new AutoCompleteEntry(obj.TEN, obj.MA, obj.ID.ToString()));
            }
            return lstAutoCompleteEntry;
        }

        public static List<AutoCompleteEntry> ToAutoCompleteEntry(List<NS_DM_CHUC_VU> lst)
        {
            List<AutoCompleteEntry> lstAutoCompleteEntry = new List<AutoCompleteEntry>();
            foreach (NS_DM_CHUC_VU obj in lst)
            {
                lstAutoCompleteEntry.Add(new AutoCompleteEntry(obj.TEN, obj.MA, obj.ID.ToString()));
            }
            return lstAutoCompleteEntry;
        }

        public static List<AutoCompleteEntry> ToAutoCompleteEntry(List<NS_DM_CNGANH_DTAO> lst)
        {
            List<AutoCompleteEntry> lstAutoCompleteEntry = new List<AutoCompleteEntry>();
            foreach (NS_DM_CNGANH_DTAO obj in lst)
            {
                lstAutoCompleteEntry.Add(new AutoCompleteEntry(obj.TEN, obj.MA, obj.ID.ToString()));
            }
            return lstAutoCompleteEntry;
        }

        public static List<AutoCompleteEntry> ToAutoCompleteEntry(List<NS_DM_CU_TRU> lst)
        {
            List<AutoCompleteEntry> lstAutoCompleteEntry = new List<AutoCompleteEntry>();
            foreach (NS_DM_CU_TRU obj in lst)
            {
                lstAutoCompleteEntry.Add(new AutoCompleteEntry(obj.TEN, obj.MA, obj.ID.ToString()));
            }
            return lstAutoCompleteEntry;
        }

        public static List<AutoCompleteEntry> ToAutoCompleteEntry(List<NS_DM_DAN_TOC> lst)
        {
            List<AutoCompleteEntry> lstAutoCompleteEntry = new List<AutoCompleteEntry>();
            foreach (NS_DM_DAN_TOC obj in lst)
            {
                lstAutoCompleteEntry.Add(new AutoCompleteEntry(obj.TEN, obj.MA, obj.ID.ToString()));
            }
            return lstAutoCompleteEntry;
        }

        public static List<AutoCompleteEntry> ToAutoCompleteEntry(List<NS_DM_DVI_CTAC> lst)
        {
            List<AutoCompleteEntry> lstAutoCompleteEntry = new List<AutoCompleteEntry>();
            foreach (NS_DM_DVI_CTAC obj in lst)
            {
                lstAutoCompleteEntry.Add(new AutoCompleteEntry(obj.TEN, obj.MA, obj.ID.ToString()));
            }
            return lstAutoCompleteEntry;
        }

        public static List<AutoCompleteEntry> ToAutoCompleteEntry(List<NS_DM_DVI_TGIAN> lst)
        {
            List<AutoCompleteEntry> lstAutoCompleteEntry = new List<AutoCompleteEntry>();
            foreach (NS_DM_DVI_TGIAN obj in lst)
            {
                lstAutoCompleteEntry.Add(new AutoCompleteEntry(obj.TEN, obj.MA, obj.ID.ToString()));
            }
            return lstAutoCompleteEntry;
        }

        public static List<AutoCompleteEntry> ToAutoCompleteEntry(List<NS_DM_GIOI_TINH> lst)
        {
            List<AutoCompleteEntry> lstAutoCompleteEntry = new List<AutoCompleteEntry>();
            foreach (NS_DM_GIOI_TINH obj in lst)
            {
                lstAutoCompleteEntry.Add(new AutoCompleteEntry(obj.TEN, obj.MA, obj.ID.ToString()));
            }
            return lstAutoCompleteEntry;
        }

        public static List<AutoCompleteEntry> ToAutoCompleteEntry(List<NS_DM_HOC_HAM> lst)
        {
            List<AutoCompleteEntry> lstAutoCompleteEntry = new List<AutoCompleteEntry>();
            foreach (NS_DM_HOC_HAM obj in lst)
            {
                lstAutoCompleteEntry.Add(new AutoCompleteEntry(obj.TEN, obj.MA, obj.ID.ToString()));
            }
            return lstAutoCompleteEntry;
        }

        public static List<AutoCompleteEntry> ToAutoCompleteEntry(List<NS_DM_HOC_VI> lst)
        {
            List<AutoCompleteEntry> lstAutoCompleteEntry = new List<AutoCompleteEntry>();
            foreach (NS_DM_HOC_VI obj in lst)
            {
                lstAutoCompleteEntry.Add(new AutoCompleteEntry(obj.TEN, obj.MA, obj.ID.ToString()));
            }
            return lstAutoCompleteEntry;
        }

        public static List<AutoCompleteEntry> ToAutoCompleteEntry(List<NS_DM_HTHUC_DTAO> lst)
        {
            List<AutoCompleteEntry> lstAutoCompleteEntry = new List<AutoCompleteEntry>();
            foreach (NS_DM_HTHUC_DTAO obj in lst)
            {
                lstAutoCompleteEntry.Add(new AutoCompleteEntry(obj.TEN, obj.MA, obj.ID.ToString()));
            }
            return lstAutoCompleteEntry;
        }

        public static List<AutoCompleteEntry> ToAutoCompleteEntry(List<NS_DM_HTHUC_KLUAT> lst)
        {
            List<AutoCompleteEntry> lstAutoCompleteEntry = new List<AutoCompleteEntry>();
            foreach (NS_DM_HTHUC_KLUAT obj in lst)
            {
                lstAutoCompleteEntry.Add(new AutoCompleteEntry(obj.TEN, obj.MA, obj.ID.ToString()));
            }
            return lstAutoCompleteEntry;
        }

        public static List<AutoCompleteEntry> ToAutoCompleteEntry(List<NS_DM_HTHUC_KTHUONG> lst)
        {
            List<AutoCompleteEntry> lstAutoCompleteEntry = new List<AutoCompleteEntry>();
            foreach (NS_DM_HTHUC_KTHUONG obj in lst)
            {
                lstAutoCompleteEntry.Add(new AutoCompleteEntry(obj.TEN, obj.MA, obj.ID.ToString()));
            }
            return lstAutoCompleteEntry;
        }

        public static List<AutoCompleteEntry> ToAutoCompleteEntry(List<NS_DM_HTHUC_LVIEC> lst)
        {
            List<AutoCompleteEntry> lstAutoCompleteEntry = new List<AutoCompleteEntry>();
            foreach (NS_DM_HTHUC_LVIEC obj in lst)
            {
                lstAutoCompleteEntry.Add(new AutoCompleteEntry(obj.TEN, obj.MA, obj.ID.ToString()));
            }
            return lstAutoCompleteEntry;
        }

        public static List<AutoCompleteEntry> ToAutoCompleteEntry(List<NS_DM_HTHUC_TLUONG> lst)
        {
            List<AutoCompleteEntry> lstAutoCompleteEntry = new List<AutoCompleteEntry>();
            foreach (NS_DM_HTHUC_TLUONG obj in lst)
            {
                lstAutoCompleteEntry.Add(new AutoCompleteEntry(obj.TEN, obj.MA, obj.ID.ToString()));
            }
            return lstAutoCompleteEntry;
        }

        public static List<AutoCompleteEntry> ToAutoCompleteEntry(List<NS_DM_KHIEU_CCONG> lst)
        {
            List<AutoCompleteEntry> lstAutoCompleteEntry = new List<AutoCompleteEntry>();
            foreach (NS_DM_KHIEU_CCONG obj in lst)
            {
                lstAutoCompleteEntry.Add(new AutoCompleteEntry(obj.TEN, obj.MA, obj.ID.ToString()));
            }
            return lstAutoCompleteEntry;
        }

        public static List<AutoCompleteEntry> ToAutoCompleteEntry(List<NS_DM_KHOA_DTAO> lst)
        {
            List<AutoCompleteEntry> lstAutoCompleteEntry = new List<AutoCompleteEntry>();
            foreach (NS_DM_KHOA_DTAO obj in lst)
            {
                lstAutoCompleteEntry.Add(new AutoCompleteEntry(obj.TEN, obj.MA, obj.ID.ToString()));
            }
            return lstAutoCompleteEntry;
        }

        public static List<AutoCompleteEntry> ToAutoCompleteEntry(List<NS_DM_KY_NANG> lst)
        {
            List<AutoCompleteEntry> lstAutoCompleteEntry = new List<AutoCompleteEntry>();
            foreach (NS_DM_KY_NANG obj in lst)
            {
                lstAutoCompleteEntry.Add(new AutoCompleteEntry(obj.TEN, obj.MA, obj.ID.ToString()));
            }
            return lstAutoCompleteEntry;
        }

        public static List<AutoCompleteEntry> ToAutoCompleteEntry(List<NS_DM_LDO_NPHEP> lst)
        {
            List<AutoCompleteEntry> lstAutoCompleteEntry = new List<AutoCompleteEntry>();
            foreach (NS_DM_LDO_NPHEP obj in lst)
            {
                lstAutoCompleteEntry.Add(new AutoCompleteEntry(obj.TEN, obj.MA, obj.ID.ToString()));
            }
            return lstAutoCompleteEntry;
        }

        public static List<AutoCompleteEntry> ToAutoCompleteEntry(List<NS_DM_LDO_TVIEC> lst)
        {
            List<AutoCompleteEntry> lstAutoCompleteEntry = new List<AutoCompleteEntry>();
            foreach (NS_DM_LDO_TVIEC obj in lst)
            {
                lstAutoCompleteEntry.Add(new AutoCompleteEntry(obj.TEN, obj.MA, obj.ID.ToString()));
            }
            return lstAutoCompleteEntry;
        }

        public static List<AutoCompleteEntry> ToAutoCompleteEntry(List<NS_DM_LOAI_CPHI> lst)
        {
            List<AutoCompleteEntry> lstAutoCompleteEntry = new List<AutoCompleteEntry>();
            foreach (NS_DM_LOAI_CPHI obj in lst)
            {
                lstAutoCompleteEntry.Add(new AutoCompleteEntry(obj.TEN, obj.MA, obj.ID.ToString()));
            }
            return lstAutoCompleteEntry;
        }

        public static List<AutoCompleteEntry> ToAutoCompleteEntry(List<NS_DM_LOAI_GTO> lst)
        {
            List<AutoCompleteEntry> lstAutoCompleteEntry = new List<AutoCompleteEntry>();
            foreach (NS_DM_LOAI_GTO obj in lst)
            {
                lstAutoCompleteEntry.Add(new AutoCompleteEntry(obj.TEN, obj.MA, obj.ID.ToString()));
            }
            return lstAutoCompleteEntry;
        }

        public static List<AutoCompleteEntry> ToAutoCompleteEntry(List<NS_DM_LOAI_HDLD> lst)
        {
            List<AutoCompleteEntry> lstAutoCompleteEntry = new List<AutoCompleteEntry>();
            foreach (NS_DM_LOAI_HDLD obj in lst)
            {
                lstAutoCompleteEntry.Add(new AutoCompleteEntry(obj.TEN, obj.MA, obj.ID.ToString()));
            }
            return lstAutoCompleteEntry;
        }

        public static List<AutoCompleteEntry> ToAutoCompleteEntry(List<NS_DM_LOAI_HSO> lst)
        {
            List<AutoCompleteEntry> lstAutoCompleteEntry = new List<AutoCompleteEntry>();
            foreach (NS_DM_LOAI_HSO obj in lst)
            {
                lstAutoCompleteEntry.Add(new AutoCompleteEntry(obj.TEN, obj.MA, obj.ID.ToString()));
            }
            return lstAutoCompleteEntry;
        }

        public static List<AutoCompleteEntry> ToAutoCompleteEntry(List<NS_DM_LOAI_TNHAP> lst)
        {
            List<AutoCompleteEntry> lstAutoCompleteEntry = new List<AutoCompleteEntry>();
            foreach (NS_DM_LOAI_TNHAP obj in lst)
            {
                lstAutoCompleteEntry.Add(new AutoCompleteEntry(obj.TEN, obj.MA, obj.ID.ToString()));
            }
            return lstAutoCompleteEntry;
        }

        public static List<AutoCompleteEntry> ToAutoCompleteEntry(List<NS_DM_NGHE_NGHIEP> lst)
        {
            List<AutoCompleteEntry> lstAutoCompleteEntry = new List<AutoCompleteEntry>();
            foreach (NS_DM_NGHE_NGHIEP obj in lst)
            {
                lstAutoCompleteEntry.Add(new AutoCompleteEntry(obj.TEN, obj.MA, obj.ID.ToString()));
            }
            return lstAutoCompleteEntry;
        }

        public static List<AutoCompleteEntry> ToAutoCompleteEntry(List<NS_DM_PHUONG_XA> lst)
        {
            List<AutoCompleteEntry> lstAutoCompleteEntry = new List<AutoCompleteEntry>();
            foreach (NS_DM_PHUONG_XA obj in lst)
            {
                lstAutoCompleteEntry.Add(new AutoCompleteEntry(obj.TEN, obj.MA, obj.ID.ToString()));
            }
            return lstAutoCompleteEntry;
        }

        public static List<AutoCompleteEntry> ToAutoCompleteEntry(List<NS_DM_QHE_GDINH> lst)
        {
            List<AutoCompleteEntry> lstAutoCompleteEntry = new List<AutoCompleteEntry>();
            foreach (NS_DM_QHE_GDINH obj in lst)
            {
                lstAutoCompleteEntry.Add(new AutoCompleteEntry(obj.TEN, obj.MA, obj.ID.ToString()));
            }
            return lstAutoCompleteEntry;
        }

        public static List<AutoCompleteEntry> ToAutoCompleteEntry(List<NS_DM_QUAN_HUYEN> lst)
        {
            List<AutoCompleteEntry> lstAutoCompleteEntry = new List<AutoCompleteEntry>();
            foreach (NS_DM_QUAN_HUYEN obj in lst)
            {
                lstAutoCompleteEntry.Add(new AutoCompleteEntry(obj.TEN, obj.MA, obj.ID.ToString()));
            }
            return lstAutoCompleteEntry;
        }

        public static List<AutoCompleteEntry> ToAutoCompleteEntry(List<NS_DM_QUOC_TICH> lst)
        {
            List<AutoCompleteEntry> lstAutoCompleteEntry = new List<AutoCompleteEntry>();
            foreach (NS_DM_QUOC_TICH obj in lst)
            {
                lstAutoCompleteEntry.Add(new AutoCompleteEntry(obj.TEN, obj.MA, obj.ID.ToString()));
            }
            return lstAutoCompleteEntry;
        }

        public static List<AutoCompleteEntry> ToAutoCompleteEntry(List<NS_DM_TDO_CTRI> lst)
        {
            List<AutoCompleteEntry> lstAutoCompleteEntry = new List<AutoCompleteEntry>();
            foreach (NS_DM_TDO_CTRI obj in lst)
            {
                lstAutoCompleteEntry.Add(new AutoCompleteEntry(obj.TEN, obj.MA, obj.ID.ToString()));
            }
            return lstAutoCompleteEntry;
        }

        public static List<AutoCompleteEntry> ToAutoCompleteEntry(List<NS_DM_TDO_HVAN> lst)
        {
            List<AutoCompleteEntry> lstAutoCompleteEntry = new List<AutoCompleteEntry>();
            foreach (NS_DM_TDO_HVAN obj in lst)
            {
                lstAutoCompleteEntry.Add(new AutoCompleteEntry(obj.TEN, obj.MA, obj.ID.ToString()));
            }
            return lstAutoCompleteEntry;
        }

        public static List<AutoCompleteEntry> ToAutoCompleteEntry(List<NS_DM_TDO_TANH> lst)
        {
            List<AutoCompleteEntry> lstAutoCompleteEntry = new List<AutoCompleteEntry>();
            foreach (NS_DM_TDO_TANH obj in lst)
            {
                lstAutoCompleteEntry.Add(new AutoCompleteEntry(obj.TEN, obj.MA, obj.ID.ToString()));
            }
            return lstAutoCompleteEntry;
        }

        public static List<AutoCompleteEntry> ToAutoCompleteEntry(List<NS_DM_TDO_THOC> lst)
        {
            List<AutoCompleteEntry> lstAutoCompleteEntry = new List<AutoCompleteEntry>();
            foreach (NS_DM_TDO_THOC obj in lst)
            {
                lstAutoCompleteEntry.Add(new AutoCompleteEntry(obj.TEN, obj.MA, obj.ID.ToString()));
            }
            return lstAutoCompleteEntry;
        }

        public static List<AutoCompleteEntry> ToAutoCompleteEntry(List<NS_DM_THAN_HDLD> lst)
        {
            List<AutoCompleteEntry> lstAutoCompleteEntry = new List<AutoCompleteEntry>();
            foreach (NS_DM_THAN_HDLD obj in lst)
            {
                lstAutoCompleteEntry.Add(new AutoCompleteEntry(obj.TEN, obj.MA, obj.ID.ToString()));
            }
            return lstAutoCompleteEntry;
        }

        public static List<AutoCompleteEntry> ToAutoCompleteEntry(List<NS_DM_TINH_TP> lst)
        {
            List<AutoCompleteEntry> lstAutoCompleteEntry = new List<AutoCompleteEntry>();
            foreach (NS_DM_TINH_TP obj in lst)
            {
                lstAutoCompleteEntry.Add(new AutoCompleteEntry(obj.TEN, obj.MA, obj.ID.ToString()));
            }
            return lstAutoCompleteEntry;
        }

        public static List<AutoCompleteEntry> ToAutoCompleteEntry(List<NS_DM_TON_GIAO> lst)
        {
            List<AutoCompleteEntry> lstAutoCompleteEntry = new List<AutoCompleteEntry>();
            foreach (NS_DM_TON_GIAO obj in lst)
            {
                lstAutoCompleteEntry.Add(new AutoCompleteEntry(obj.TEN, obj.MA, obj.ID.ToString()));
            }
            return lstAutoCompleteEntry;
        }

        public static List<AutoCompleteEntry> ToAutoCompleteEntry(List<NS_DM_TRUONG_DTAO> lst)
        {
            List<AutoCompleteEntry> lstAutoCompleteEntry = new List<AutoCompleteEntry>();
            foreach (NS_DM_TRUONG_DTAO obj in lst)
            {
                lstAutoCompleteEntry.Add(new AutoCompleteEntry(obj.TEN, obj.MA, obj.ID.ToString()));
            }
            return lstAutoCompleteEntry;
        }

        public static List<AutoCompleteEntry> ToAutoCompleteEntry(List<NS_DM_TTRANG_HNHAN> lst)
        {
            List<AutoCompleteEntry> lstAutoCompleteEntry = new List<AutoCompleteEntry>();
            foreach (NS_DM_TTRANG_HNHAN obj in lst)
            {
                lstAutoCompleteEntry.Add(new AutoCompleteEntry(obj.TEN, obj.MA, obj.ID.ToString()));
            }
            return lstAutoCompleteEntry;
        }

        public static List<AutoCompleteEntry> ToAutoCompleteEntry(List<NS_DM_XEP_LOAI> lst)
        {
            List<AutoCompleteEntry> lstAutoCompleteEntry = new List<AutoCompleteEntry>();
            foreach (NS_DM_XEP_LOAI obj in lst)
            {
                lstAutoCompleteEntry.Add(new AutoCompleteEntry(obj.TEN, obj.MA, obj.ID.ToString()));
            }
            return lstAutoCompleteEntry;
        }

        public static List<AutoCompleteEntry> ToAutoCompleteEntry(List<NS_DM_PHU_CAP> lst)
        {
            List<AutoCompleteEntry> lstAutoCompleteEntry = new List<AutoCompleteEntry>();
            foreach (NS_DM_PHU_CAP obj in lst)
            {
                lstAutoCompleteEntry.Add(new AutoCompleteEntry(obj.TEN, obj.MA, obj.ID.ToString()));
            }
            return lstAutoCompleteEntry;
        }


    }
}
