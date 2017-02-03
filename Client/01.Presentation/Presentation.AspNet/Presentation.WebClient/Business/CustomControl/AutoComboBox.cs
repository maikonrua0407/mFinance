using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.ComponentModel;
using Presentation.Process;
using Presentation.Process.TruyVanServiceRef;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Utilities.Common;
using Presentation.Process.Common;
using System.Xml.Linq;

namespace Presentation.WebClient.Business.CustomControl
{
    public class AutoComboBox
    {

        private List<AutoCompleteEntry> lstSource;
        private DropDownList comboBox;
        /// <summary>
        /// Tạo AutocomboBox
        /// </summary>
        /// <param name="lstSource">Source cho Combobox</param>
        /// <param name="comboBox">Tên control Combobox được gen</param>
        /// <param name="maTruyVan">Mã truy vấn để lấy source cho combobox</param>
        /// <param name="lstDieuKien">Giá trị các điều kiện tương ứng với truy vấn</param>
        /// <param name="maChon">Mã Item sẽ được chọn nếu có</param>
        /// <param name="lstMaKhoangChon">Danh sách các Item sẽ được gen vào Combobox nếu có.</param>
        public void GenAutoComboBox(ref List<AutoCompleteEntry> lstSource, ref DropDownList comboBox, string maTruyVan, List<string> lstDieuKien = null, string maChon = "Default", List<string> lstMaKhoangChon = null)
        {
            Func<AutoCompleteEntry, bool> predicate = null;
            Func<AutoCompleteEntry, bool> func2 = null;
            try
            {
                if (!string.IsNullOrEmpty(maTruyVan))
                {
                    TruyVanProcess process = new TruyVanProcess();
                    DataSet dataSetSource = new DataSet();
                    dataSetSource = process.getDanhSachInformation(maTruyVan, lstDieuKien).DataSetSource;
                    foreach (DataRow row in dataSetSource.Tables[0].Rows)
                    {
                        if (row.Table.Columns.Count > 3)
                        {
                            List<string> list = new List<string> {
                            row[1].ToString(),
                            row[0].ToString()
                        };
                            for (int i = 3; i < dataSetSource.Tables[0].Columns.Count; i++)
                            {
                                list.Add(row[i].ToString());
                            }
                            lstSource.Add(new AutoCompleteEntry(LanguageEngine.Instance().GetContent(0, row[2].ToString()), list.ToArray()));
                        }
                        else
                        {
                            string content = LanguageEngine.Instance().GetContent(0, row[2].ToString());
                            if (content == "")
                            {
                                content = LanguageEngine.Instance().GetContent(0, row[1].ToString());
                            }
                            lstSource.Add(new AutoCompleteEntry(content, new string[] { row[1].ToString(), row[0].ToString() }));
                        }
                    }
                }
                if (lstMaKhoangChon != null)
                {
                    if (predicate == null)
                    {
                        predicate = e => lstMaKhoangChon.Contains(e.KeywordStrings.First<string>());
                    }
                    lstSource = lstSource.Where<AutoCompleteEntry>(predicate).ToList<AutoCompleteEntry>();
                }
                foreach (AutoCompleteEntry entry in lstSource)
                {
                    string displayName = entry.DisplayName;
                    string str3 = entry.KeywordStrings[0];
                    if (displayName == "")
                    {
                        try
                        {
                            displayName = entry.KeywordStrings[2];
                        }
                        catch
                        {
                            displayName = entry.KeywordStrings[1];
                        }
                        if (displayName == "")
                        {
                            displayName = entry.KeywordStrings[0];
                        }
                        str3 = entry.KeywordStrings[0];
                    }
                    ListItem item = new ListItem(displayName, str3);
                    comboBox.Items.Add(item);
                }
                if (!string.IsNullOrEmpty(maChon))
                {
                    if (maChon.Equals("Default"))
                    {
                        comboBox.SelectedIndex = 0;
                    }
                    else
                    {
                        if (func2 == null)
                        {
                            func2 = e => e.KeywordStrings.First<string>().Equals(maChon);
                        }
                        comboBox.SelectedIndex = lstSource.IndexOf(lstSource.FirstOrDefault<AutoCompleteEntry>(func2));
                    }
                }
            }
            catch (Exception exception)
            {
                LLogging.WriteLog("", LLogging.LogType.ERR, exception);
            }
        }
        public void GenAutoSoure(ref List<AutoCompleteEntry> lstSource,  string maTruyVan, List<string> lstDieuKien = null, string maChon = "Default", List<string> lstMaKhoangChon = null)
        {
            Func<AutoCompleteEntry, bool> predicate = null;
            Func<AutoCompleteEntry, bool> func2 = null;
            try
            {
                if (!string.IsNullOrEmpty(maTruyVan))
                {
                    TruyVanProcess process = new TruyVanProcess();
                    DataSet dataSetSource = new DataSet();
                    dataSetSource = process.getDanhSachInformation(maTruyVan, lstDieuKien).DataSetSource;
                    foreach (DataRow row in dataSetSource.Tables[0].Rows)
                    {
                        if (row.Table.Columns.Count > 3)
                        {
                            List<string> list = new List<string> {
                            row[1].ToString(),
                            row[0].ToString()
                        };
                            for (int i = 3; i < dataSetSource.Tables[0].Columns.Count; i++)
                            {
                                list.Add(row[i].ToString());
                            }
                            lstSource.Add(new AutoCompleteEntry(LanguageEngine.Instance().GetContent(0, row[2].ToString()), list.ToArray()));
                        }
                        else
                        {
                            string content = LanguageEngine.Instance().GetContent(0, row[2].ToString());
                            if (content == "")
                            {
                                content = LanguageEngine.Instance().GetContent(0, row[1].ToString());
                            }
                            lstSource.Add(new AutoCompleteEntry(content, new string[] { row[1].ToString(), row[0].ToString() }));
                        }
                    }
                }
                if (lstMaKhoangChon != null)
                {
                    if (predicate == null)
                    {
                        predicate = e => lstMaKhoangChon.Contains(e.KeywordStrings.First<string>());
                    }
                    lstSource = lstSource.Where<AutoCompleteEntry>(predicate).ToList<AutoCompleteEntry>();
                }               
            }
            catch (Exception exception)
            {
                LLogging.WriteLog("", LLogging.LogType.ERR, exception);
            }
        }


        public void GenAutoComboBox3C(ref List<AutoCompleteEntry> lstSource, ref DropDownList comboBox, string maTruyVan, List<string> lstDieuKien = null, string maChon = "Default", List<string> lstMaKhoangChon = null)
        {
            Func<AutoCompleteEntry, bool> predicate = null;
            Func<AutoCompleteEntry, bool> func2 = null;
            try
            {
                if (!string.IsNullOrEmpty(maTruyVan))
                {
                    TruyVanProcess process = new TruyVanProcess();
                    DataSet dataSetSource = new DataSet();
                    dataSetSource = process.getDanhSachInformation(maTruyVan, lstDieuKien).DataSetSource;
                    foreach (DataRow row in dataSetSource.Tables[0].Rows)
                    {
                        if (row.Table.Columns.Count >= 3)
                        {
                             List<string> list = new List<string> {
                            row[1].ToString(),
                            row[0].ToString()
                        };

                             lstSource.Add(new AutoCompleteEntry(row[2].ToString(), list.ToArray()));
                        }
                        else
                        {
                            string content = LanguageEngine.Instance().GetContent(0, row[2].ToString());
                            if (content == "")
                            {
                                content = LanguageEngine.Instance().GetContent(0, row[1].ToString());
                            }
                            lstSource.Add(new AutoCompleteEntry(content, new string[] { row[1].ToString(), row[0].ToString() }));
                        }
                    }
                }
                if (lstMaKhoangChon != null)
                {
                    if (predicate == null)
                    {
                        predicate = e => lstMaKhoangChon.Contains(e.KeywordStrings.First<string>());
                    }
                    lstSource = lstSource.Where<AutoCompleteEntry>(predicate).ToList<AutoCompleteEntry>();
                }
                foreach (AutoCompleteEntry entry in lstSource)
                {
                    string displayName = entry.DisplayName;
                    string str3 = "";
                    try
                    {
                        str3 = entry.KeywordStrings[0] + "|" + entry.KeywordStrings[1];
                    }
                    catch
                    {
                        str3 = entry.KeywordStrings[0];
                    }
                    ListItem item = new ListItem(displayName, str3);
                    comboBox.Items.Add(item);
                }
                if (!string.IsNullOrEmpty(maChon))
                {
                    if (maChon.Equals("Default"))
                    {
                        comboBox.SelectedIndex = 0;
                    }
                    else
                    {
                        if (func2 == null)
                        {
                            func2 = e => e.KeywordStrings.First<string>().Equals(maChon);
                        }
                        comboBox.SelectedIndex = lstSource.IndexOf(lstSource.FirstOrDefault<AutoCompleteEntry>(func2));
                    }
                }
            }
            catch (Exception exception)
            {
                LLogging.WriteLog("", LLogging.LogType.ERR, exception);
            }
        }

        public AutoCompleteEntry GetEntryByDisplayName(List<AutoCompleteEntry> lstSource, ref DropDownList comboBox)
        {
            AutoCompleteEntry result = null;

            //if (comboBox != null && comboBox.SelectedValue != null)
            //{
            //    result = (AutoCompleteEntry)comboBox.SelectedValue;
            //}

            //if (comboBox != null && comboBox.SelectedItem != null)
            //{
            //    result = (AutoCompleteEntry)comboBox.SelectedItem;
            //}

            return result;
        }
    }
}