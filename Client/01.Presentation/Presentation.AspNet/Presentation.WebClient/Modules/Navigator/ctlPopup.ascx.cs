using Presentation.Process;
using Presentation.WebClient.Business;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Presentation.WebClient.Modules.Navigator
{
    public partial class ctlPopup : ControlBase
    {

        #region [Properties]

        public string MaTruyVan
        {
            get
            {
                string v_strMaTruyVan = "";
                if (null != Request.QueryString["m"])
                {
                    v_strMaTruyVan = Request.QueryString["m"];
                }
                return v_strMaTruyVan;
            }
        }

        public string DieuKien
        {
            get
            {
                string v_strDieuKien = "";
                if (null != Request.QueryString["q"])
                {
                    v_strDieuKien = Request.QueryString["q"];
                }
                return v_strDieuKien;
            }
        }

        public string ReturnKey
        {
            get
            {
                string v_strKey = "";
                if (null != Request.QueryString["r"])
                {
                    v_strKey = Request.QueryString["r"];
                }
                return v_strKey;
            }
        }

        protected string mv_strConent = "";

        #endregion

        #region [Custom methods]

        public void InitData()
        {
            string v_strScript = "";
            StringBuilder v_st = new StringBuilder();
            string v_strGridHeader = "", v_strGridBody = "", v_strGridFooter = "";
            int v_iReturnCol = 0;

            if (MaTruyVan.Length > 0)
            {
                List<string> v_lstDieuKien = new List<string>();
                if (DieuKien.Length > 0)
                {
                    string[] v_arr = DieuKien.Split("|".ToCharArray());
                    for (int i = 0; i < v_arr.GetLength(0); i++)
                    {
                        v_lstDieuKien.Add(v_arr[i]);
                    }
                }

                DataTable v_dt = TruyVanController.GetPopupData(MaTruyVan, v_lstDieuKien);
                if (null != v_dt)
                {
                    //Build header
                    v_strGridHeader = "<table id=\"grdPopup\" class=\"display\" cellspacing=\"0\" border=\"0\" width=\"100%\">\r\n";
                    v_strGridHeader += "    <thead>\r\n";
                    v_strGridHeader += "        <tr>\r\n";
                    
                    for (int i = 0; i < v_dt.Columns.Count; i++)
                    {
                        v_strGridHeader += "            <th>" + LanguageEngine.Instance().GetContent(LanguageType.TypeUI,v_dt.Columns[i].Caption) + "</th>\r\n";
                        if (v_dt.Columns[i].ExtendedProperties["width"].ToString() == "0")
                        {
                            v_st.AppendLine("   table.column(" + i.ToString() + ").visible(false);");
                        }
                        if (v_dt.Columns[i].ColumnName.ToUpper().Trim() == ReturnKey.ToUpper().Trim())
                        {
                            v_iReturnCol = i;
                        }
                    }

                    v_strGridHeader += "        </tr>\r\n";
                    v_strGridHeader += "    </thead>\r\n";

                    v_strGridFooter = "</table>";

                    //Build body
                    v_strGridBody = "<tbody>\r\n";
                    for (int i = 0; i < v_dt.Rows.Count; i++)
                    {
                        v_strGridBody += "  <tr>\r\n";
                        for (int j = 0; j < v_dt.Columns.Count; j++)
                        {
                            if (v_dt.Columns[j].DataType == typeof(DateTime))
                            {
                                v_strGridBody += "      <td>" + ((DateTime)v_dt.Rows[i][j]).ToString("dd/MM/yyyy") + "</td>\r\n";
                            }
                            else if ((v_dt.Columns[j].DataType == typeof(Int32)) ||
                                (v_dt.Columns[j].DataType == typeof(Int64)) ||
                                (v_dt.Columns[j].DataType == typeof(Decimal)) ||
                                (v_dt.Columns[j].DataType == typeof(float)))
                            {
                                v_strGridBody += "      <td>" + (Convert.ToDecimal(v_dt.Rows[i][j])).ToString("###,###,##0.##") + "</td>\r\n";
                            }
                            else
                            {
                                if (!v_dt.Columns[j].ColumnName.Contains("KEY"))
                                {
                                    v_strGridBody += "      <td>" + LanguageEngine.Instance().GetContent(LanguageType.TypeUI,v_dt.Rows[i][j].ToString()) + "</td>\r\n";
                                }
                                else
                                {
                                    v_strGridBody += "      <td>" + v_dt.Rows[i][j].ToString() + "</td>\r\n";
                                }
                            }

                        }
                        v_strGridBody += "  </tr>\r\n";
                    }
                    v_strGridBody += "/<tbody>\r\n";

                    //Build script
                    v_strScript = "<script type = \"text/javascript\">\r\n" +
                                    "$(function() {\r\n" +
                                    "   var table = $(\"#grdPopup\").DataTable({\r\n" +
                                    "                   \"bInfo\": false,\r\n" +
                                    "                   \"pageLength\": 10,\r\n" +
                                    "                   \"dom\": '<\"top\"i>rt<\"bottom\"flp><\"clear\">'\r\n" +
                                    "    });\r\n" +
                                    v_st.ToString() + "\r\n" +
                                    "   $('#grdPopup tbody').on('dblclick', 'tr', function() {\r\n" +
                                    "       if ($(this).hasClass('selected')) {\r\n" +
                                    "           $(this).removeClass('selected');\r\n" +
                                    "       }\r\n" +
                                    "       else {\r\n" +
                                    "           table.$('tr.selected').removeClass('selected');\r\n" +
                                    "           $(this).addClass('selected');\r\n" +
                                    "          opener.setSearchResult(targetField,table.row(this).data()[" + v_iReturnCol.ToString() + "]);\r\n" +
                                    "           window.close();\r\n" +
                                    "       }\r\n" +
                                    "   });\r\n" +
                                    "});\r\n" +
                                    " </script>\r\n";


                    mv_strConent = v_strScript + v_strGridHeader + v_strGridBody + v_strGridFooter;
                }
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                InitData();
            }
        }
    }
}