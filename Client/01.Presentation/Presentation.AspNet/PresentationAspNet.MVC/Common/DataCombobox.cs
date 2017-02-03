using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Windows;
using Presentation.Process;
using Presentation.Process.TruyVanServiceRef;
using Utilities.Common;

namespace PresentationAspNet.MVC
{
    public class DataCombobox
    {
        public class Page
        {
            public string id;
        }
        public static List<SelectListItem> GetListOptionDatetime()
        {
            var option = new List<SelectListItem>();
            option.Add(new SelectListItem { Value = "1", Text = "Hôm nay" });
            option.Add(new SelectListItem { Value = "2", Text = "Tuần này" });
            option.Add(new SelectListItem { Value = "3", Text = "Tháng này" });
            option.Add(new SelectListItem { Value = "4", Text = "Ngày này trong tháng" });
            option.Add(new SelectListItem { Value = "5", Text = "Quý này" });
            option.Add(new SelectListItem { Value = "6", Text = "Ngày này của quý" });
            option.Add(new SelectListItem { Value = "7", Text = "Năm nay" });
            option.Add(new SelectListItem { Value = "8", Text = "Ngày này của năm" });
            return option;
        }
        public static string GenerateKho(string idControl, int width, int idSelect, string idChiNhanh)
        {
            var data = UserInformation.Session_User.ListPhongGD;
            var sb = new StringBuilder();
            sb.AppendLine("<select id=\"" + idControl + "\" style=\"width:" + width + "%\">");
            if (data.Any())
            {
                foreach (var k in data)
                {
                    if (k.ID == idSelect)
                    {
                        sb.AppendLine("<option value=\"" + k.ID + "\" selected=\"selected\">" + k.TEN_GDICH + "</option>");
                    }
                    else
                    {
                        sb.AppendLine("<option value=\"" + k.ID + "\">" + k.TEN_GDICH + "</option>");
                    }
                }
            }
            sb.AppendLine("</select>");
            return sb.ToString();
        }
        private static bool GetIsSelect(string idOri, string idCompare)
        {
            return idOri == idCompare ? true : false;
        }
        public static List<SelectListItem> LoadKhoTheoChiNhanh(int idChiNhanh, bool ShowAll = true)
        {
            var option = new List<SelectListItem>();
            if (ShowAll) option.Add(new SelectListItem { Text = "-- Tất cả --", Value = "0" });
            var lstData = UserInformation.Session_User.ListPhongGD.Where(p => p.ID_DVI_CHA == idChiNhanh && p.TTHAI_BGHI == BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri()).OrderBy(p => p.TEN_GDICH).ToList();
            if (lstData.Count > 0)
            {
                option.AddRange(lstData.Select(d => new SelectListItem { Text = d.TEN_GDICH, Value = d.ID.ToString(), Selected = d.ID == Common.GetIdDonViGd() }));
            }
            return option;
        }
        public static List<SelectListItem> LoadFontSize(string DefaultSelect = "")
        {
            var option = new List<SelectListItem>();
            for (int i = 0; i < 20; i++)
            {
                option.Add(new SelectListItem
                {
                    Text = i.ToString(),
                    Value = i.ToString(),
                    Selected = !string.IsNullOrEmpty(DefaultSelect) && DefaultSelect.Equals(i.ToString()) ? true : false
                });
            }
            return option;
        }
        public static List<SelectListItem> LoadComboYear()
        {
            var option = new List<SelectListItem>();
            int iYear = DateTime.Now.Year;
            for (int i = 0; i < 20; i++)
            {
                option.Add(new SelectListItem { Text = (iYear - i).ToString(), Value = (iYear - i).ToString() });
            }
            return option;
        }
        public static List<SelectListItem> LoadComboMonth()
        {
            var option = new List<SelectListItem>();
            for (int i = 1; i <= 12; i++)
            {
                option.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString(), Selected = DateTime.Now.Month == i });
            }
            return option;
        }
        /// <summary>
        /// Load datasource Giờ
        /// </summary>
        /// <param name="showAll">hiển thị giá trị mặc định "Giờ"</param>
        /// <returns></returns>
        public static List<SelectListItem> LoadComboGio(bool showAll)
        {
            var option = new List<SelectListItem>();
            if (showAll) option.Add(new SelectListItem { Text = "Giờ", Value = "0" });
            option.Add(new SelectListItem { Text = "6:00 AM", Value = "6" });
            option.Add(new SelectListItem { Text = "7:00 AM", Value = "7" });
            option.Add(new SelectListItem { Text = "8:00 AM", Value = "8" });
            option.Add(new SelectListItem { Text = "9:00 AM", Value = "9" });
            option.Add(new SelectListItem { Text = "10:00 AM", Value = "10" });
            option.Add(new SelectListItem { Text = "11:00 AM", Value = "11" });
            option.Add(new SelectListItem { Text = "12:00 AM", Value = "12" });
            option.Add(new SelectListItem { Text = "1:00 PM", Value = "13" });
            option.Add(new SelectListItem { Text = "2:00 PM", Value = "14" });
            option.Add(new SelectListItem { Text = "3:00 PM", Value = "15" });
            option.Add(new SelectListItem { Text = "4:00 PM", Value = "16" });
            option.Add(new SelectListItem { Text = "5:00 PM", Value = "17" });
            option.Add(new SelectListItem { Text = "6:00 PM", Value = "18" });
            option.Add(new SelectListItem { Text = "7:00 PM", Value = "19" });
            option.Add(new SelectListItem { Text = "8:00 PM", Value = "20" });
            option.Add(new SelectListItem { Text = "9:00 PM", Value = "21" });
            return option;
        }
        public static List<SelectListItem> LoadComboMonthLoadToMoment()
        {
            var option = new List<SelectListItem>();
            for (int i = 1; i <= DateTime.Now.Month; i++)
            {
                option.Add(new SelectListItem { Text = i.ToString(), Value = i.ToString(), Selected = DateTime.Now.Month == i ? true : false });
            }
            return option;
        }
        public static List<SelectListItem> LoadPageSizeCustom()
        {
            var option = new List<SelectListItem>
                             {
                                 //new SelectListItem {Text = "3", Value = "3"},
                                 //new SelectListItem {Text = "5", Value = "5"},
                                 new SelectListItem {Text = "10", Value = "10"},
                                 new SelectListItem {Text = "20", Value = "20"},
                                 new SelectListItem {Text = "30", Value = "30"},
                                 new SelectListItem {Text = "40", Value = "40"},
                                 new SelectListItem {Text = "50", Value = "50"},
                                 new SelectListItem {Text = "100", Value = "100"}
                             };
            return option;
        }
        public static List<Page> initPageShow()
        {
            var lst = new List<Page>
                          {
                              new Page {id = "10"},
                              new Page {id = "20"},
                              new Page {id = "30"},
                              new Page {id = "50"},
                              new Page {id = "100"}
                          };
            return lst;
        }
        public static string LoadPageShow(string current, string act)
        {
            var sb = new StringBuilder();
            sb.AppendLine("<select name=\"cboPage" + act + "\" id=\"cboPage" + act + "\" style=\"width:60%\" class=\"cssCombobox\" onchange=\"loadPage('1','" + act + "');\">");
            foreach (var d in initPageShow())
            {
                if (current == d.id)
                {
                    sb.AppendLine("<option selected  value=\"" + d.id + "\">" + d.id + "</option>");
                }
                else
                {
                    sb.AppendLine("<option value=\"" + d.id + "\">" + d.id + "</option>");
                }
            }
            sb.AppendLine("</select> ");
            return sb.ToString();
        }
        public static string LoadComboKhoByUser()
        {
            var sb = new StringBuilder();
            var data = UserInformation.Session_User.ListPhongGD.OrderBy(p => p.ID).ToList();
            if (data.Count > 0)
            {
                sb.AppendLine("<select name=\"cboKho\" onkeydown=\"ChonPhongGdLamViec(event);\" id=\"cboKho\" class=\"cssCombobox\">");
                foreach (var d in data)
                {
                    sb.AppendLine("<option value=\"" + d.ID + "\">" + d.TEN_GDICH + "</option>");
                }
                sb.AppendLine("</select> ");
            }
            return sb.ToString();
        }
        public static string LoadComboKhoByUser(bool showAll, int selectedId)
        {
            var sb = new StringBuilder();
            var data = UserInformation.Session_User.ListPhongGD.OrderBy(p => p.ID).ToList();
            sb.AppendLine("<select name=\"cboKhoXuatLoadComboKhoByUser\" id=\"cboKhoXuat\" class=\"css-width60\">");
            if (showAll) sb.AppendLine("<option value=\"0\">-- Tất cả --</option>");
            if (data.Count > 0)
            {
                foreach (var d in data)
                {
                    var select = d.ID == selectedId ? "selected" : string.Empty;
                    sb.AppendLine("<option " + select + " value=\"" + d.ID + "\">" + d.TEN_GDICH + "</option>");
                }
            }
            sb.AppendLine("</select> ");
            return sb.ToString();
        }
        public static string LoadComboDiemGdByUser()
        {
            var sb = new StringBuilder();
            var data = UserInformation.Session_User.ListPhongGD.Where(e => e.ID_DVI_CHA == UserInformation.Session_User.IdDonViGiaoDich).ToList();
            if (data != null && data.Count > 0)
            {
                sb.AppendLine("<select name=\"cboDiemGD\" onkeydown=\"ChonDiemLamViec(event);\" id=\"cboDiemGD\" class=\"cssCombobox\">");
                foreach (var d in data)
                {
                    sb.AppendLine("<option value=\"" + d.ID + "\">" + d.TEN_GDICH + "</option>");
                }
                sb.AppendLine("</select> ");
            }
            return sb.ToString();
        }
        public static List<SelectListItem> LoadKho(int selectId = 0, bool isShowAll = false)
        {
            var option = new List<SelectListItem>();
            var data = UserInformation.Session_User.ListPhongGD;
            if (data.Count > 0)
            {
                if (isShowAll)
                {
                    option.Add(new SelectListItem
                    {
                        Text = "-- Tất cả --",
                        Value = "0",
                        Selected = selectId == 0
                    });
                }
                option.AddRange(data.Select(d => new SelectListItem
                                                     {
                                                         Text = d.TEN_GDICH,
                                                         Value = d.ID.ToString(CultureInfo.InvariantCulture),
                                                         Selected = d.ID == selectId
                                                     }));
            }
            return option;
        }
        public static List<SelectListItem> LoadKhoLamViec(int selectId)
        {
            var option = new List<SelectListItem>();
            var data = UserInformation.Session_User.ListPhongGD;
            if (data.Count > 0)
            {
                option.AddRange(data.Select(d => new SelectListItem
                {
                    Text = d.TEN_GDICH,
                    Value = d.ID.ToString(CultureInfo.InvariantCulture),
                    Selected = d.ID == selectId
                }));
            }
            return option;
        }
        public static List<SelectListItem> LoadAllKho(int selectId = 0, bool isShowAll = false)
        {
            var option = new List<SelectListItem>();
            var data = UserInformation.Session_User.ListPhongGD;
            if (data.Count > 0)
            {
                if (isShowAll)
                {
                    option.Add(new SelectListItem
                    {
                        Text = "--- Tất cả ---",
                        Value = "0",
                        Selected = selectId == 0
                    });
                }
                option.AddRange(data.Select(d => new SelectListItem
                {
                    Text = d.TEN_GDICH,
                    Value = d.ID.ToString(),
                    Selected = d.ID == selectId
                }));
            }
            return option;
        }
        public static List<SelectListItem> LoadKho(int selectId, int idChiNhanh = 0)
        {
            var option = new List<SelectListItem>();
            var data = (idChiNhanh > 0
                    ? UserInformation.Session_User.ListPhongGD.Where(p => p.TTHAI_BGHI == BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri() && p.ID_DVI_CHA == idChiNhanh)
                    : UserInformation.Session_User.ListPhongGD.Where(p => p.TTHAI_BGHI == BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri()))
                .OrderBy(p => p.ID).ToList();
            if (data.Count > 0)
            {
                option.AddRange(data.Select(d => new SelectListItem
                {
                    Text = d.TEN_GDICH,
                    Value = d.ID.ToString(),
                    Selected = d.ID == selectId
                }));
            }
            return option;
        }

        /// <summary>
        /// Tạo AutocomboBox
        /// </summary>
        /// <param name="lstSource">Source cho Combobox</param>
        /// <param name="comboBox">Tên control Combobox được gen</param>
        /// <param name="maTruyVan">Mã truy vấn để lấy source cho combobox</param>
        /// <param name="lstDieuKien">Giá trị các điều kiện tương ứng với truy vấn</param>
        /// <param name="maChon">Mã Item sẽ được chọn nếu có</param>
        /// <param name="lstMaKhoangChon">Danh sách các Item sẽ được gen vào Combobox nếu có.</param>
        public static void GenAutoComboBox(ref List<AutoCompleteEntry> lstSource, string maTruyVan, List<string> lstDieuKien = null, string maChon = "Default", List<string> lstMaKhoangChon = null)
        {
            try
            {
                if (!string.IsNullOrEmpty(maTruyVan))
                {
                    var process = new TruyVanProcess();
                    DataSet dataSetSource = new DataSet();
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
                            lstSource.Add(new AutoCompleteEntry(LanguageNode.GetValueUILanguage(row[2].ToString()), lst.ToArray()));
                        }
                        else
                        {
                            lstSource.Add(new AutoCompleteEntry(LanguageNode.GetValueUILanguage(row[2].ToString()), row[1].ToString(), row[0].ToString()));
                        }
                    }
                }
                if (lstMaKhoangChon != null)
                {
                    lstSource = lstSource.Where(e => lstMaKhoangChon.Contains(e.KeywordStrings.First())).ToList();
                }
            }
            catch (Exception e)
            {
                LLogging.WriteLog("", LLogging.LogType.ERR, e);
            }
        }

    }

    public class AutoCompleteEntry
    {
        private string[] keywordStrings;
        private string displayString;

        public string[] KeywordStrings
        {
            get
            {
                if (keywordStrings == null)
                {
                    keywordStrings = new string[] { displayString };
                }
                return keywordStrings;
            }
        }

        public string DisplayName
        {
            get { return displayString; }
            set { displayString = value; }
        }

        public AutoCompleteEntry(string name, params string[] keywords)
        {
            displayString = name;
            keywordStrings = keywords;
        }

        public override string ToString()
        {
            return displayString;
        }
    }
}