using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows;
using Presentation.Process;
using Presentation.Process.TruyVanServiceRef;
using Telerik.Windows.Controls;
using System.Data;
using Utilities.Common;
using Presentation.Process.Common;
using System.Xml.Linq;

namespace PresentationWPF.CustomControl
{
    public class AutoComboBox
    {
        private List<AutoCompleteEntry> lstSource;
        private RadComboBox comboBox;

        /// <summary>
        /// Tạo AutocomboBox
        /// </summary>
        /// <param name="lstSource">Source cho Combobox</param>
        /// <param name="comboBox">Tên control Combobox được gen</param>
        /// <param name="maTruyVan">Mã truy vấn để lấy source cho combobox</param>
        /// <param name="lstDieuKien">Giá trị các điều kiện tương ứng với truy vấn</param>
        /// <param name="maChon">Mã Item sẽ được chọn nếu có</param>
        /// <param name="lstMaKhoangChon">Danh sách các Item sẽ được gen vào Combobox nếu có.</param>
        public void GenAutoComboBox(ref List<AutoCompleteEntry> lstSource, ref RadComboBox comboBox, string maTruyVan, List<string> lstDieuKien = null, string maChon = "Default", List<string> lstMaKhoangChon = null)
        {
            try
            {
                if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
                    return;
                if (!string.IsNullOrEmpty(maTruyVan))
                {
                    var process = new TruyVanProcess();
                    DataSet dataSetSource = new DataSet();

                    // Lấy kết quả trả về
                    //if (maTruyVan.Equals(DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue()))
                    //{
                    //    dataSetSource = getDataSetDmDmucGtri(maTruyVan, lstDieuKien);
                    //}
                    //else if (maTruyVan.Equals(DatabaseConstant.DanhSachTruyVan.COMBOBOX_HINHTHUCSD.getValue()))
                    //{
                    //    dataSetSource = getDataSetDmDmucGtriHinhThucSd(maTruyVan, lstDieuKien);
                    //}
                    //else if (maTruyVan.Equals(DatabaseConstant.DanhSachTruyVan.COMBOBOX_KHOANG_GTRI_TAISAN.getValue()))
                    //{
                    //    dataSetSource = getDataSetDmDmucGtriKhoangGiaTriTaiSan(maTruyVan, lstDieuKien);
                    //}
                    //else if (maTruyVan.Equals(DatabaseConstant.DanhSachTruyVan.COMBOBOX_LOAICHUNGTU_NGOAIBANG.getValue()))
                    //{
                    //    dataSetSource = getDataSetDmDmucGtriLoaiChungTuNgoaiBang(maTruyVan, lstDieuKien);
                    //}
                    //else if (maTruyVan.Equals(DatabaseConstant.DanhSachTruyVan.COMBOBOX_QUOCGIA.getValue()))
                    //{
                    //    dataSetSource = getDataSetDmQuocGia(maTruyVan, lstDieuKien);
                    //}
                    //else if (maTruyVan.Equals(DatabaseConstant.DanhSachTruyVan.COMBOBOX_TIENTE.getValue()) ||
                    //    maTruyVan.Equals(DatabaseConstant.DanhSachTruyVan.COMBOBOX_LOAITIEN.getValue()))
                    //{
                    //    dataSetSource = getDataSetDmTienTe(maTruyVan, lstDieuKien);
                    //}
                    //else if (maTruyVan.Equals(DatabaseConstant.DanhSachTruyVan.COMBOBOX_TINHTP.getValue()))
                    //{
                    //    dataSetSource = getDataSetDmTinhTp(maTruyVan, lstDieuKien);
                    //}
                    //else if (maTruyVan.Equals(DatabaseConstant.DanhSachTruyVan.COMBOBOX_COSOTINHLAI.getValue()))
                    //{
                    //    dataSetSource = getDataSetDcCsoTlai(maTruyVan, lstDieuKien);
                    //}
                    //else if (maTruyVan.Equals(DatabaseConstant.DanhSachTruyVan.COMBOBOX_LOAI_LSUAT.getValue()))
                    //{
                    //    dataSetSource = getDataSetDcLoaiLsuat(maTruyVan, lstDieuKien);
                    //}
                    //else if (maTruyVan.Equals(DatabaseConstant.DanhSachTruyVan.COMBOBOX_TINH_CHAT_TK.getValue()))
                    //{
                    //    dataSetSource = getDataSetKtNhomPloai(maTruyVan, lstDieuKien);
                    //}
                    //else if (maTruyVan.Equals(DatabaseConstant.DanhSachTruyVan.COMBOBOX_KY_HIEU.getValue()))
                    //{
                    //    dataSetSource = getDataSetKtKyHieu(maTruyVan, lstDieuKien);
                    //}
                    //else if (maTruyVan.Equals(DatabaseConstant.DanhSachTruyVan.COMBOBOX_NAM_TCHINH.getValue()))
                    //{
                    //    dataSetSource = getDataSetNamTaiChinh(maTruyVan, lstDieuKien);
                    //}
                    //else
                    //{
                        DanhSachResponse response = process.getDanhSachInformation(maTruyVan, lstDieuKien);
                        dataSetSource = response.DataSetSource;
                        
                    //}

                    foreach (DataRow row in dataSetSource.Tables[0].Rows)
                    {

                        if (row.Table.Columns.Count > 3)
                        {
                            List<string> lst = new List<string>();
                            lst.Add(row[1].ToString());
                            lst.Add(row[0].ToString());
                            for (int i = 3; i < dataSetSource.Tables[0].Columns.Count; i++)
                                lst.Add(row[i].ToString());                            
                            lstSource.Add(new AutoCompleteEntry(LLanguage.SearchResourceByKey(row[2].ToString()), lst.ToArray()));
                        }
                        else
                        {
                            lstSource.Add(new AutoCompleteEntry(LLanguage.SearchResourceByKey(row[2].ToString()), row[1].ToString(), row[0].ToString()));
                        }
                    }
                }
                if (lstMaKhoangChon != null)
                {
                    lstSource = lstSource.Where(e => lstMaKhoangChon.Contains(e.KeywordStrings.First())).ToList();
                }
                
                //while (comboBox.Items.Count > 0)
                //    comboBox.Items.RemoveAt(comboBox.Items.Count - 1);

                foreach (AutoCompleteEntry auto in lstSource)
                {
                    comboBox.Items.Add(auto);
                }
                if (!string.IsNullOrEmpty(maChon))
                {
                    if (maChon.Equals("Default"))
                        comboBox.SelectedIndex = 0;
                    else
                        comboBox.SelectedIndex = lstSource.IndexOf(lstSource.FirstOrDefault(e => e.KeywordStrings.First().Equals(maChon)));
                }
            }
            catch (Exception e)
            {
                LLogging.WriteLog("", LLogging.LogType.ERR, e);
            }
        }

        public void GenComboBox_Nam(ref RadComboBox comboBox, string maChon = null)
        {
            try
            {
                for (int year = 2013; year <= Convert.ToInt32(ClientInformation.NgayLamViecHienTai.Substring(0, 4)); year++)
                {
                    AutoCompleteEntry auto = new AutoCompleteEntry(year.ToString(), year.ToString(), year.ToString());
                    comboBox.Items.Add(auto);
                }
                if (maChon.IsNullOrEmptyOrSpace())
                    maChon = Convert.ToInt32(ClientInformation.NgayLamViecHienTai.Substring(0, 4)).ToString();
                foreach (var item in comboBox.Items)
                {
                    AutoCompleteEntry entry = (AutoCompleteEntry)item;
                    if (entry.DisplayName.Equals(maChon))
                        comboBox.SelectedItem = item;
                }
            }
            catch (Exception e)
            {
                LLogging.WriteLog("", LLogging.LogType.ERR, e);
            }
        }

        public void GenComboBox_Quy(ref RadComboBox comboBox, string maChon = null)
        {
            try
            {
                for (int quy = 1; quy <= 4; quy++)
                {
                    AutoCompleteEntry auto = new AutoCompleteEntry(quy.ToString(), quy.ToString(), quy.ToString());
                    comboBox.Items.Add(auto);
                }
                if (maChon.IsNullOrEmptyOrSpace())
                    maChon = ClientInformation.NgayLamViecHienTai.StringToDate("yyyyMMdd").GetQuarter().ToString();
                foreach (var item in comboBox.Items)
                {
                    AutoCompleteEntry entry = (AutoCompleteEntry)item;
                    if (entry.DisplayName.Equals(maChon))
                        comboBox.SelectedItem = item;
                }
            }
            catch (Exception e)
            {
                LLogging.WriteLog("", LLogging.LogType.ERR, e);
            }
        }

        public void GenComboBox_Thang(ref RadComboBox comboBox, string maChon = null)
        {
            try
            {
                for (int month = 1; month <= 12; month++)
                {
                    AutoCompleteEntry auto = new AutoCompleteEntry(month.ToString(), month.ToString(), month.ToString());
                    comboBox.Items.Add(auto);
                }
                if (maChon.IsNullOrEmptyOrSpace())
                    maChon = Convert.ToInt32(ClientInformation.NgayLamViecHienTai.Substring(4, 2)).ToString();
                foreach (var item in comboBox.Items)
                {
                    AutoCompleteEntry entry = (AutoCompleteEntry)item;
                    if (entry.DisplayName.Equals(maChon))
                        comboBox.SelectedItem = item;
                }
            }
            catch (Exception e)
            {
                LLogging.WriteLog("", LLogging.LogType.ERR, e);
            }
        }

        public DataSet getDataSetNamTaiChinh(string maTruyVan, List<string> lstDieuKien = null)
        {
            DataSet ret = null;
            List<CMB_NAM_TCHINH> listCMB_NAM_TCHINH = new List<CMB_NAM_TCHINH>();

            // Lấy năm tài chính theo mã đơn vị (hiện tại fix chung hệ thống = năm làm việc hiện tại của đơn vị quản lý người dùng đang đăng nhập)
            string maDonVi = "";
            if (lstDieuKien != null && lstDieuKien.Count > 0)
            {
                maDonVi = lstDieuKien[0];
            }

            string strNamBatDau = "2013";
            string strNamHienTai = ClientInformation.NgayLamViecHienTai.Substring(0, 4);
            int iNamBatDau = Int32.Parse(strNamBatDau);
            int iNamHienTai = Int32.Parse(strNamHienTai);

            if (iNamHienTai <= iNamBatDau)
            {
                CMB_NAM_TCHINH namTaiChinh = new CMB_NAM_TCHINH
                                             {
                                                 ID = iNamHienTai,
                                                 MA = strNamHienTai,
                                                 TEN = strNamHienTai
                                             };
                listCMB_NAM_TCHINH.Add(namTaiChinh);
            }
            else
            {
                for (int i = iNamBatDau; i <= iNamHienTai; i++)
                {
                    CMB_NAM_TCHINH namTaiChinh = new CMB_NAM_TCHINH
                    {
                        ID = i,
                        MA = i.ToString(),
                        TEN = i.ToString()
                    };
                    listCMB_NAM_TCHINH.Add(namTaiChinh);
                }
            }

            ret = CommonFunction.ToDataSet(listCMB_NAM_TCHINH);
            return ret;
        }

        public DataSet getDataSetDmDmucGtri(string maTruyVan, List<string> lstDieuKien = null)
        {
            DataSet ret = null;
            string xmlFile = ClientInformation.DataDir + "\\" + "DM_DMUC_GTRI.xml";
            XElement xmlBook = XElement.Load(xmlFile);

            string maDmucLoai = lstDieuKien != null ? lstDieuKien[0] : "";
            List<CMB_DM_DMUC_GTRI> listCMB_DM_DMUC_GTRI = (from q in xmlBook.Elements("row")
                                                           where q.Element("MA_DMUC_LOAI").Value == maDmucLoai
                                                           select new CMB_DM_DMUC_GTRI
                                                            {
                                                                ID = Int32.Parse(q.Element("ID").Value),
                                                                MA_DMUC = q.Element("MA_DMUC").Value,
                                                                TEN_DMUC = q.Element("TEN_DMUC").Value
                                                            }).ToList();

            ret = CommonFunction.ToDataSet(listCMB_DM_DMUC_GTRI);

            return ret;
        }

        public DataSet getDataSetDmDmucGtriHinhThucSd(string maTruyVan, List<string> lstDieuKien = null)
        {
            DataSet ret = null;
            string xmlFile = ClientInformation.DataDir + "\\" + "DM_DMUC_GTRI.xml";
            XElement xmlBook = XElement.Load(xmlFile);

            string maDmucLoai = "HINH_THUC_SU_DUNG_TSDB";
            List<CMB_DM_DMUC_GTRI> listCMB_DM_DMUC_GTRI = (from q in xmlBook.Elements("row")
                                                           where q.Element("MA_DMUC_LOAI").Value == maDmucLoai
                                                           select new CMB_DM_DMUC_GTRI
                                                           {
                                                               ID = Int32.Parse(q.Element("ID").Value),
                                                               MA_DMUC = q.Element("MA_DMUC").Value,
                                                               TEN_DMUC = q.Element("TEN_DMUC").Value
                                                           }).ToList();

            ret = CommonFunction.ToDataSet(listCMB_DM_DMUC_GTRI);

            return ret;
        }

        public DataSet getDataSetDmDmucGtriKhoangGiaTriTaiSan(string maTruyVan, List<string> lstDieuKien = null)
        {
            DataSet ret = null;
            string xmlFile = ClientInformation.DataDir + "\\" + "DM_DMUC_GTRI.xml";
            XElement xmlBook = XElement.Load(xmlFile);

            string maDmucLoai = "KHOANG_GIA_TRI_TAI_SAN";
            if (lstDieuKien.Count > 0)
                maDmucLoai = lstDieuKien[0];
            List<CMB_DM_DMUC_GTRI> listCMB_DM_DMUC_GTRI = (from q in xmlBook.Elements("row")
                                                           where q.Element("MA_DMUC_LOAI").Value == maDmucLoai
                                                           select new CMB_DM_DMUC_GTRI
                                                           {
                                                               ID = Int32.Parse(q.Element("ID").Value),
                                                               MA_DMUC = q.Element("MA_DMUC").Value,
                                                               TEN_DMUC = q.Element("TEN_DMUC").Value
                                                           }).ToList();

            ret = CommonFunction.ToDataSet(listCMB_DM_DMUC_GTRI);

            return ret;
        }

        public DataSet getDataSetDmDmucGtriLoaiChungTuNgoaiBang(string maTruyVan, List<string> lstDieuKien = null)
        {
            DataSet ret = null;
            string xmlFile = ClientInformation.DataDir + "\\" + "DM_DMUC_GTRI.xml";
            XElement xmlBook = XElement.Load(xmlFile);

            string maDmucLoai = "LOAI_CHUNG_TU";
            string xnb = "XNB";
            string nnb = "NNB";
            List<CMB_DM_DMUC_GTRI> listCMB_DM_DMUC_GTRI = (from q in xmlBook.Elements("row")
                                                           where q.Element("MA_DMUC_LOAI").Value == maDmucLoai
                                                           && (q.Element("MA_DMUC").Value == xnb || q.Element("MA_DMUC").Value == nnb)
                                                           select new CMB_DM_DMUC_GTRI
                                                           {
                                                               ID = Int32.Parse(q.Element("ID").Value),
                                                               MA_DMUC = q.Element("MA_DMUC").Value,
                                                               TEN_DMUC = q.Element("TEN_DMUC").Value
                                                           }).ToList();

            ret = CommonFunction.ToDataSet(listCMB_DM_DMUC_GTRI);

            return ret;
        }

        public DataSet getDataSetDmQuocGia(string maTruyVan, List<string> lstDieuKien = null)
        {
            DataSet ret = null;
            string xmlFile = ClientInformation.DataDir + "\\" + "DM_QUOC_GIA.xml";
            XElement xmlBook = XElement.Load(xmlFile);

            List<CMB_DM_QUOC_GIA> listCMB_DM_QUOC_GIA = (from q in xmlBook.Elements("row")
                                                         select new CMB_DM_QUOC_GIA
                                                         {
                                                             ID = Int32.Parse(q.Element("ID").Value),
                                                             MA_ISO2 = q.Element("MA_ISO2").Value,
                                                             TEN_QGIA = q.Element("TEN_QGIA").Value,
                                                             MA_ISO3 = q.Element("MA_ISO3").Value
                                                         }).ToList();

            ret = CommonFunction.ToDataSet(listCMB_DM_QUOC_GIA);

            return ret;
        }

        public DataSet getDataSetDmTienTe(string maTruyVan, List<string> lstDieuKien = null)
        {
            DataSet ret = null;
            string xmlFile = ClientInformation.DataDir + "\\" + "DM_TIEN_TE.xml";
            XElement xmlBook = XElement.Load(xmlFile);

            List<CMB_DM_TIEN_TE> listCMB_DM_TIEN_TE = (from q in xmlBook.Elements("row")
                                                       select new CMB_DM_TIEN_TE
                                                       {
                                                           ID = Int32.Parse(q.Element("ID").Value),
                                                           KY_HIEU = q.Element("KY_HIEU").Value,
                                                           TEN_TTE = q.Element("TEN_TTE").Value
                                                       }).ToList();

            ret = CommonFunction.ToDataSet(listCMB_DM_TIEN_TE);

            return ret;
        }

        public DataSet getDataSetDmTinhTp(string maTruyVan, List<string> lstDieuKien = null)
        {
            DataSet ret = null;
            string xmlFile = ClientInformation.DataDir + "\\" + "DM_TINH_TP.xml";
            XElement xmlBook = XElement.Load(xmlFile);

            List<CMB_DM_TINH_TP> listCMB_DM_TINH_TP = (from q in xmlBook.Elements("row")
                                                       select new CMB_DM_TINH_TP
                                                       {
                                                           ID = Int32.Parse(q.Element("ID").Value),
                                                           MA_TINHTP = q.Element("MA_TINHTP").Value,
                                                           TEN_TINHTP = q.Element("TEN_TINHTP").Value
                                                       }).ToList();

            ret = CommonFunction.ToDataSet(listCMB_DM_TINH_TP);

            return ret;
        }

        public DataSet getDataSetDcCsoTlai(string maTruyVan, List<string> lstDieuKien = null)
        {
            DataSet ret = null;
            string xmlFile = ClientInformation.DataDir + "\\" + "DC_CSO_TLAI.xml";
            XElement xmlBook = XElement.Load(xmlFile);

            List<CMB_DC_CSO_TLAI> listCMB_DC_CSO_TLAI = (from q in xmlBook.Elements("row")
                                                         select new CMB_DC_CSO_TLAI
                                                        {
                                                            ID = Int32.Parse(q.Element("ID").Value),
                                                            MA_CSO_TLAI = q.Element("MA_CSO_TLAI").Value,
                                                            TEN_CSO_TLAI = q.Element("TEN_CSO_TLAI").Value
                                                        }).ToList();

            ret = CommonFunction.ToDataSet(listCMB_DC_CSO_TLAI);

            return ret;
        }

        public DataSet getDataSetDcLoaiLsuat(string maTruyVan, List<string> lstDieuKien = null)
        {
            DataSet ret = null;
            string xmlFile = ClientInformation.DataDir + "\\" + "DC_CSO_TLAI.xml";
            XElement xmlBook = XElement.Load(xmlFile);

            string doiTuongApDung = "HTH";
            List<CMB_DC_CSO_TLAI> listCMB_DC_CSO_TLAI = (from q in xmlBook.Elements("row")
                                                         where q.Element("DTUONG_ADUNG").Value == doiTuongApDung
                                                         select new CMB_DC_CSO_TLAI
                                                         {
                                                             ID = Int32.Parse(q.Element("ID").Value),
                                                             MA_CSO_TLAI = q.Element("MA_CSO_TLAI").Value,
                                                             TEN_CSO_TLAI = q.Element("TEN_CSO_TLAI").Value
                                                         }).ToList();

            ret = CommonFunction.ToDataSet(listCMB_DC_CSO_TLAI);

            return ret;
        }

        public DataSet getDataSetKtKyHieu(string maTruyVan, List<string> lstDieuKien = null)
        {
            DataSet ret = null;
            string xmlFile = ClientInformation.DataDir + "\\" + "KT_KY_HIEU.xml";
            XElement xmlBook = XElement.Load(xmlFile);

            List<CMB_KT_KY_HIEU> listCMB_KT_KY_HIEU = (from q in xmlBook.Elements("row")
                                                       select new CMB_KT_KY_HIEU
                                                        {
                                                            ID = Int32.Parse(q.Element("ID").Value),
                                                            MA_KY_HIEU = q.Element("MA_KY_HIEU").Value,
                                                            TEN_KY_HIEU = q.Element("TEN_KY_HIEU").Value
                                                        }).ToList();

            ret = CommonFunction.ToDataSet(listCMB_KT_KY_HIEU);

            return ret;
        }

        public DataSet getDataSetKtNhomPloai(string maTruyVan, List<string> lstDieuKien = null)
        {
            DataSet ret = null;
            string xmlFile = ClientInformation.DataDir + "\\" + "KT_NHOM_PLOAI.xml";
            XElement xmlBook = XElement.Load(xmlFile);

            List<CMB_KT_NHOM_PLOAI> listCMB_KT_NHOM_PLOAI = (from q in xmlBook.Elements("row")
                                                             select new CMB_KT_NHOM_PLOAI
                                                             {
                                                                 ID = Int32.Parse(q.Element("ID").Value),
                                                                 MA_NHOM_PLOAI = q.Element("MA_NHOM_PLOAI").Value,
                                                                 TEN_NHOM_PLOAI = q.Element("TEN_NHOM_PLOAI").Value
                                                             }).ToList();

            ret = CommonFunction.ToDataSet(listCMB_KT_NHOM_PLOAI);

            return ret;
        }

        public class CMB_NAM_TCHINH
        {
            public int ID { get; set; }
            public string MA { get; set; }
            public string TEN { get; set; }
        }

        public class CMB_DM_DMUC_GTRI
        {
            public int ID { get; set; }
            public string MA_DMUC { get; set; }
            public string TEN_DMUC { get; set; }

            public int ID_DMUC_LOAI { get; set; }
            public string MA_DMUC_LOAI { get; set; }
            public string TTHAI_BGHI { get; set; }
            public string TTHAI_NVU { get; set; }
            public string MA_DVI_QLY { get; set; }
            public string MA_DVI_TAO { get; set; }
        }

        public class CMB_DM_QUOC_GIA
        {
            public int ID { get; set; }
            public string MA_ISO2 { get; set; }
            public string TEN_QGIA { get; set; }
            public string MA_ISO3 { get; set; }

            public string MA_QGIA_AN { get; set; }
            public string MA_QGIA_NN { get; set; }
            public string TEN_TIENGANH { get; set; }
            public string TEN_TAT { get; set; }
            public string TTHAI_BGHI { get; set; }
            public string TTHAI_NVU { get; set; }
            public string MA_DVI_QLY { get; set; }
            public string MA_DVI_TAO { get; set; }
        }

        public class CMB_DM_TIEN_TE
        {
            public int ID { get; set; }
            public string KY_HIEU { get; set; }
            public string TEN_TTE { get; set; }

            public string MA_TTE_AN { get; set; }
            public string MA_TTE_NN { get; set; }
            public string MA_SO { get; set; }
            public string TEN_TIENGANH { get; set; }
            public string TEN_TAT { get; set; }
            public string TTHAI_BGHI { get; set; }
            public string TTHAI_NVU { get; set; }
            public string MA_DVI_QLY { get; set; }
            public string MA_DVI_TAO { get; set; }
        }

        public class CMB_DM_TINH_TP
        {
            public int ID { get; set; }
            public string MA_TINHTP { get; set; }
            public string TEN_TINHTP { get; set; }

            public string TEN_TAT { get; set; }
            public string TTHAI_BGHI { get; set; }
            public string TTHAI_NVU { get; set; }
            public string MA_DVI_QLY { get; set; }
            public string MA_DVI_TAO { get; set; }
        }

        public class CMB_DC_CSO_TLAI
        {
            public int ID { get; set; }
            public string MA_CSO_TLAI { get; set; }
            public string TEN_CSO_TLAI { get; set; }

            public string DTUONG_ADUNG { get; set; }
            public string TTHAI_BGHI { get; set; }
            public string TTHAI_NVU { get; set; }
            public string MA_DVI_QLY { get; set; }
            public string MA_DVI_TAO { get; set; }
        }

        public class CMB_KT_KY_HIEU
        {
            public int ID { get; set; }
            public string MA_KY_HIEU { get; set; }
            public string TEN_KY_HIEU { get; set; }

            public string TTHAI_BGHI { get; set; }
            public string TTHAI_NVU { get; set; }
            public string MA_DVI_QLY { get; set; }
            public string MA_DVI_TAO { get; set; }
        }

        public class CMB_KT_NHOM_PLOAI
        {
            public int ID { get; set; }
            public string MA_NHOM_PLOAI { get; set; }
            public string TEN_NHOM_PLOAI { get; set; }

            public string TTHAI_BGHI { get; set; }
            public string TTHAI_NVU { get; set; }
            public string MA_DVI_QLY { get; set; }
            public string MA_DVI_TAO { get; set; }
        }

        public void GenAutoComboBoxDirectly(ref List<AutoCompleteEntry> lstSource, ref RadComboBox comboBox, string maTruyVan, List<string> lstDieuKien = null, string maChon = "Default", List<string> lstMaKhoangChon = null)
        {
            try
            {
                if (DesignerProperties.GetIsInDesignMode(new DependencyObject()))
                    return;
                if (!string.IsNullOrEmpty(maTruyVan))
                {
                    var process = new TruyVanProcess();
                    DataSet dataSetSource = new DataSet();

                    // Lấy kết quả trả về
                    DanhSachResponse response = process.getDanhSachInformation(maTruyVan, lstDieuKien);
                    dataSetSource = response.DataSetSource;

                    foreach (DataRow row in dataSetSource.Tables[0].Rows)
                    {
                        lstSource.Add(new AutoCompleteEntry(LLanguage.SearchResourceByKey(row[2].ToString()), row[1].ToString(), row[0].ToString()));
                    }
                }
                if (lstMaKhoangChon != null)
                {
                    lstSource = lstSource.Where(e => lstMaKhoangChon.Contains(e.KeywordStrings.First())).ToList();
                }
                foreach (AutoCompleteEntry auto in lstSource)
                {
                    comboBox.Items.Add(auto);
                }
                if (!string.IsNullOrEmpty(maChon))
                {
                    if (maChon.Equals("Default"))
                        comboBox.SelectedIndex = 0;
                    else
                        comboBox.SelectedIndex = lstSource.IndexOf(lstSource.FirstOrDefault(e => e.KeywordStrings.First().Equals(maChon)));
                }
            }
            catch (Exception e)
            {
                LLogging.WriteLog("", LLogging.LogType.ERR, e);
            }
        }

        public AutoCompleteEntry checkDisplayName(List<AutoCompleteEntry> lstSource, ref RadComboBox comboBox)
        {
            AutoCompleteEntry result = null;

            if (lstSource == null)
            {
                return result;
            }
            else
            {
                bool chk = false;
                foreach (AutoCompleteEntry auto in lstSource)
                {
                    if (auto.DisplayName.Equals(comboBox.Text))
                    {
                        result = auto;
                        return result;
                    }
                }
                if (!chk)
                {
                    comboBox.Text = "";
                    return result;
                }
            }
            return result;
        }

        public AutoCompleteEntry getEntryByDisplayName(List<AutoCompleteEntry> lstSource, ref RadComboBox comboBox)
        {
            AutoCompleteEntry result = null;

            if (comboBox != null && comboBox.SelectedValue != null)
            {
                result = (AutoCompleteEntry)comboBox.SelectedValue;
            }

            if (comboBox != null && comboBox.SelectedItem != null)
            {
                result = (AutoCompleteEntry)comboBox.SelectedItem;
            }

            return result;
        }

        public void removeEntry(ref List<AutoCompleteEntry> lstSource, ref RadComboBox comboBox, string keyWord, string maChon = "Default")
        {
            foreach (AutoCompleteEntry auto in lstSource)
            {
                if (auto.KeywordStrings.First().Equals(keyWord))
                {
                    lstSource.Remove(auto);
                    comboBox.Items.Remove(auto);

                    if (!string.IsNullOrEmpty(maChon))
                    {
                        if (maChon.Equals("Default"))
                            comboBox.SelectedIndex = 0;
                        else
                            comboBox.SelectedIndex = lstSource.IndexOf(lstSource.FirstOrDefault(e => e.KeywordStrings.First().Equals(maChon)));
                    }

                    return;
                }
            }
        }

        public void GenAutoComboBoxBySource(ref List<AutoCompleteEntry> lstSource, ref RadComboBox comboBox, string maChon = "Default")
        {
            comboBox.Items.Clear();
            foreach (AutoCompleteEntry auto in lstSource)
            {
                comboBox.Items.Add(auto);
            }
            if (!string.IsNullOrEmpty(maChon))
            {
                if (maChon.Equals("Default"))
                    comboBox.SelectedIndex = 0;
                else
                    comboBox.SelectedIndex = lstSource.IndexOf(lstSource.FirstOrDefault(e => e.KeywordStrings.First().Equals(maChon)));
            }
        }

        public List<AutoCompleteEntry> CopyListEntry(List<AutoCompleteEntry> lstSource)
        {
            List<AutoCompleteEntry> lst = new List<AutoCompleteEntry>();
            foreach (AutoCompleteEntry objSource in lstSource)
            {
                AutoCompleteEntry obj = objSource;
                lst.Add(obj);
            }
            return lst;
        }

        public void GenAutoComboBoxTheoList(ref List<COMBOBOX_DTO> lst)
        {
            DataSet ds = GetSourceTheoList(lst);
            if (ds == null || ds.Tables.Count < 1) return;

            for (int i = 0; i < lst.Count; i++)
            {
                if (ds.Tables[i] != null && ds.Tables[i].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[i].Rows)
                    {
                        lst[i].lstSource.Add(new AutoCompleteEntry(LLanguage.SearchResourceByKey(row[2].ToString()), row[1].ToString(), row[0].ToString()));
                    }
                }
            }

            foreach (var item in lst)
            {
                GenAutoComboBoxBySource(ref item.lstSource, ref item.combobox, item.maChon);
            }
        }

        public DataSet GetSourceTheoList(List<COMBOBOX_DTO> lst)
        {
            List<CSO_TSO> lstCSO = new List<CSO_TSO>();
            foreach (var item in lst)
            {
                CSO_TSO obj = new CSO_TSO();
                obj.MA_CSO = item.maCSo;
                if (item.lstDieuKien != null)
                    obj.LIST_DIEU_KIEN = item.lstDieuKien.ToArray();

                lstCSO.Add(obj);
            }

            return new TruyVanProcess().getDanhSachTheoListCSO(lstCSO);
        }
    }
}
