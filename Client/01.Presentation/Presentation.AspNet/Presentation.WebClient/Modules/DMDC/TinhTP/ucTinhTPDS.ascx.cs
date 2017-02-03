using Presentation.WebClient.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utilities.Common;
using Presentation.WebClient.Business.CustomControl;
using Presentation.Process;
using System.Data;
using System.Web.Services;
using System.IO;
using System.Text;
using Presentation.Process.Common;
using Presentation.Process.TruyVanServiceRef;

namespace Presentation.WebClient.Modules.DMDC.TinhTP
{
    public partial class ucTinhTPDS : ControlBase
    {
        #region khaibao
        private string maDonVi = "%";

        // Danh sách các item được chọn trong treeview
        private List<string> lstCheckedKhuVuc = new List<string>();
        private List<string> lstCheckedSanPham = new List<string>();
        static List<HeaderDto> lstHeader;
        DataSet dsData = new DataSet();
        DataTable dtMaster = new DataTable();
        DataTable dtDetail = new DataTable();
        static bool columnsWidthLoad = false;

        List<AutoCompleteEntry> lstSourceDonVi = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstLoaiSanPham = new List<AutoCompleteEntry>();
        protected List<AutoCompleteEntry> lstNguonVon = new List<AutoCompleteEntry>();
        protected List<AutoCompleteEntry> lstPhuongThucTinhLai = new List<AutoCompleteEntry>();
        protected List<AutoCompleteEntry> lstLoaiLaiSuat = new List<AutoCompleteEntry>();
        protected List<AutoCompleteEntry> lstHieuLuc = new List<AutoCompleteEntry>();
        protected List<AutoCompleteEntry> lstMucDichSuDung = new List<AutoCompleteEntry>();
        protected List<AutoCompleteEntry> lstPhuongThucVay = new List<AutoCompleteEntry>();
        #endregion
        #region khoitao
        protected void Page_Load(object sender, EventArgs e)
        {
            tvwVungMien.Attributes.Add("onclick", "OnTreeClick(event)");

            if (!IsPostBack)
            {                
                LoadDuLieu();
                BuildGrid();
            }
            if (cfaction.Value == "delete")
            {
                List<string> kq = Xoa(chkvalgrid.Value);
                if (kq != null && kq.Count > 0)
                {
                    string strkq = "";
                    for (int j = 0; j < kq.Count; j++)
                    {
                        strkq += kq[j].Split('#')[0] + " --    " + kq[j].Split('#')[1] + "\n";
                    }
                    inpshowresult.Value = "1";
                    lblErr.Text = strkq;
                }
                LoadDuLieu();
                cfaction.Value = "0";
            }
            else if (cfaction.Value == "approve")
            {
                List<string> kq = Duyet(chkvalgrid.Value);
                if (kq != null && kq.Count > 0)
                {
                    string strkq = "";
                    for (int j = 0; j < kq.Count; j++)
                    {
                        strkq += kq[j].Split('#')[0] + " --    " + kq[j].Split('#')[1] + "\n";
                    }
                    inpshowresult.Value = "1";
                    lblErr.Text = strkq;
                }

                LoadDuLieu();
                cfaction.Value = "0";
            }
            else if (cfaction.Value == "reject")
            {
                List<string> kq = Tuchoi(chkvalgrid.Value);
                if (kq != null && kq.Count > 0)
                {
                    string strkq = "";
                    for (int j = 0; j < kq.Count; j++)
                    {
                        strkq += kq[j].Split('#')[0] + " --    " + kq[j].Split('#')[1] + "\n";
                    }
                    inpshowresult.Value = "1";
                    lblErr.Text = strkq;
                }
                LoadDuLieu();
                cfaction.Value = "0";
            }
            else if (cfaction.Value == "refuse")
            {
                List<string> kq = ThoaiDuyet(chkvalgrid.Value);
                if (kq != null && kq.Count > 0)
                {
                    string strkq = "";
                    for (int j = 0; j < kq.Count; j++)
                    {
                        strkq += kq[j].Split('#')[0] + " --    " + kq[j].Split('#')[1] + "\n";
                    }
                    inpshowresult.Value = "1";
                    lblErr.Text = strkq;
                }
                LoadDuLieu();
                cfaction.Value = "0";
            }
        }


        private void BuildGrid()
        {
            List<string> lst = new List<string>();
            foreach (TreeNode item in tvwVungMien.CheckedNodes)
            {
                if (item.ChildNodes.Count == 0)
                {
                    lst.Add(item.Value.ToString());
                }
            }
            DataTable grdData = new DataTable();
            foreach (DataColumn col in dtDetail.Columns)
            {
                grdData.Columns.Add(col.ColumnName, typeof(string));
            }
            if (lst.Count > 0)
            {
                int stt = 0;
                foreach (DataRow row in dtDetail.Rows)
                {
                    if (lst.Contains(row[dtDetail.Columns.Count - 1].ToString()))
                    {
                        stt = stt + 1;
                        row[0] = stt.ToString();
                        grdData.ImportRow(row);
                    }
                }
            }
            if (grdData.Rows.Count > 0)
            {
                grdTinhThanhDS.DataSource = grdData;
                grdTinhThanhDS.DataBind();
            }

        }

        private void loadWidthColumn()
        {
            grdTinhThanhDS.DataSource = null;
            grdTinhThanhDS.DataBind();
            if (grdTinhThanhDS.Items.Count > 0)
            {
                int idx = 1;
                foreach (HeaderDto item in lstHeader)
                {
                    double width = (double)item.WidthValue;
                    if (width > 0)
                    {
                        
                      
                        grdTinhThanhDS.Columns[idx].HeaderText = LLanguage.SearchResourceByKey(item.LanguageKey);
                    }
                    else
                        grdTinhThanhDS.Columns[idx].Visible = false;
                    idx = idx + 1;
                }
                columnsWidthLoad = true;
            }
        }


        void KhoiTaoGiaTriComboBox(ref List<AutoCompleteEntry> lstAutoComplete, ref DropDownList cmbControl, string sMaTruyVan, List<string> lstDKien = null)
        {
            AutoComboBox autoComboBox = new AutoComboBox();
            autoComboBox.GenAutoComboBox(ref lstAutoComplete, ref cmbControl, sMaTruyVan, lstDKien);
        }

        #endregion


        #region Xu ly nghiep vu
        private void getData()
        {
            lstHeader = new List<HeaderDto>();
            dsData = new DataSet();
            dtMaster = new DataTable();
            dtDetail = new DataTable();

            var process = new TruyVanProcess();
            List<string> lstDkLoc = new List<string>();
            lstDkLoc.Add("NULL");
            lstDkLoc.Add("NULL");
            DanhSachResponse DanhSachResponse = process.getDanhSachInformation(DatabaseConstant.DanhSachTruyVan.DM_TINHTP.getValue(), lstDkLoc);

            if (DanhSachResponse.DataSetSource != null)
            {
                lstHeader = DanhSachResponse.ListHeader.ToList();
                dsData = DanhSachResponse.DataSetSource;
                dtMaster = DanhSachResponse.DataSetSource.Tables[0];
                dtDetail = DanhSachResponse.DataSetSource.Tables[1];
            }
        }

        /// <summary>
        /// Load dữ liệu lên datagrid
        /// </summary>
        private void LoadDuLieu()
        {
            getData();
            if (dsData != null &&
                dtMaster != null &&
                dtDetail != null)
            {
                while (tvwVungMien.Nodes.Count > 0)
                    tvwVungMien.Nodes.Clear();
                

                DataRow drRoot = dtMaster.Rows[0];
                TreeNode rootItem = new TreeNode();
                rootItem.Text = drRoot["NODE_NAME"].ToString();
                rootItem.Value = drRoot["NODE"].ToString();
                rootItem.Checked = true;          
                tvwVungMien.Nodes.Add(rootItem);
                BuildTree(rootItem);
                //BuildGrid();
                //loadWidthColumn();
            }
        }

        /// <summary>
        /// Build cây thư mục vùng, miền, quốc gia
        /// </summary>
        /// <param name="item"></param>
        protected void BuildTree(TreeNode item)
        {
            foreach (DataRow row in dtMaster.Rows)
            {
                if (row["NODE_PARENT"].ToString() == item.Value.ToString())
                {
                    TreeNode subItem = new TreeNode();
                    subItem.Text = row["NODE_NAME"].ToString();
                    subItem.Value = row["NODE"].ToString();
                    subItem.Checked = true;
                    item.ChildNodes.Add(subItem);
                    BuildTree(subItem);
                }
            }
        }

        private DataTable ClonePageData(DataTable soureData, int page, int size)
        {
            DataTable Desdata = null;
            Desdata = soureData.Clone();
            if (soureData != null && soureData.Rows.Count > 0)
            {

                DataRow[] drow = soureData.Select("STT>" + ((page - 1) * size).ToString() + " and STT<=" + (page * size).ToString());//
                if (drow != null && drow.Length > 0)
                    foreach (DataRow dr in drow)
                    {
                        Desdata.ImportRow(dr);
                    }
            }
            return Desdata;
        }
        
        #endregion
        #region giaodien
        protected void cmbLoaiVay_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        #endregion


        protected void grdTinhThanhDS_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            e.Item.Attributes.Add("onmouseover", "currColor=this.style.backgroundColor;this.style.backgroundColor='#FDDBA5'");
            e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currColor");
        }

        protected void btnTimkiem_Click(object sender, EventArgs e)
        {
            getData();
            BuildGrid();
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            string filename = DateTime.Now.Ticks.ToString() + "xls";
            new Business.CustomControl.ExportExcel().ExportToExcel((DataTable)grdTinhThanhDS.DataSource, Server.MapPath("Modules") + "\\" + filename);
            Response.Redirect("Modules" + "\\" + filename);

        }

        private List<string> Xoa(string listitem)
        {
            List<string> listResult = new List<string>();
            if (listitem != "")
            {
                List<int> lstID = new List<int>();
                string[] str = listitem.Split(';');
                if (str != null && str.Length > 0)
                    for (int k = 0; k < str.Length; k++)
                    {
                        lstID.Add(Convert.ToInt32(str[k]));
                    }
                List<ClientResponseDetail> ResponseDetail = new List<ClientResponseDetail>();
                UtilitiesProcess process = new UtilitiesProcess();
                if (process.LockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_SAN_PHAM_TDVM,
                DatabaseConstant.Table.TD_SAN_PHAM,
                DatabaseConstant.Action.TU_CHOI_DUYET,
                lstID))
                {
                    bool bResult = new TinDungProcess().XoaSanPhamTinDung(lstID, ref ResponseDetail);
                    if (bResult)
                    {
                        foreach (ClientResponseDetail cl in ResponseDetail)
                        {
                            listResult.Add(LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, cl.Result) + "#" + LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, cl.Detail.Split('#')[0]));
                        }
                        // Yeu cau Unlook du lieu
                        process.UnlockData(DatabaseConstant.Module.TDVM,
                   DatabaseConstant.Function.TDVM_SAN_PHAM_TDVM,
                   DatabaseConstant.Table.TD_SAN_PHAM,
                   DatabaseConstant.Action.XOA,
                   lstID);

                    }
                    else
                    {
                        listResult.Add("M.DungChung.DuyetKhongThanhCong#" + LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.DungChung.DuyetKhongThanhCong"));
                    }
                }
                else
                {
                    listResult.Add("M.ResponseMessage.Common.LockDataInvalid#" + LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.ResponseMessage.Common.LockDataInvalid"));
                }
            }
            return listResult;
        }
        private List<string> Duyet(string listitem)
        {
            List<string> listResult = new List<string>();
            if (listitem != "")
            {
                List<int> lstID = new List<int>();
                string[] str = listitem.Split(';');
                if (str != null && str.Length > 0)
                    for (int k = 0; k < str.Length; k++)
                    {
                        lstID.Add(Convert.ToInt32(str[k]));
                    }
                List<ClientResponseDetail> ResponseDetail = new List<ClientResponseDetail>();
                UtilitiesProcess process = new UtilitiesProcess();
                if (process.LockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_SAN_PHAM_TDVM,
                DatabaseConstant.Table.TD_SAN_PHAM, DatabaseConstant.Action.DUYET, lstID))
                {

                    bool bResult = new TinDungProcess().DuyetSanPhamTinDung(lstID, ref ResponseDetail);
                    if (bResult)
                    {
                        foreach (ClientResponseDetail cl in ResponseDetail)
                        {
                            listResult.Add(LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, cl.Result) + "#" + LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, cl.Detail.Split('#')[0]));
                        }
                        // Yeu cau Unlook du lieu
                        process.UnlockData(DatabaseConstant.Module.TDVM,
                        DatabaseConstant.Function.TDVM_SAN_PHAM_TDVM, DatabaseConstant.Table.TD_SAN_PHAM,
                        DatabaseConstant.Action.DUYET, lstID);
                    }
                    else
                    {
                        listResult.Add("M.DungChung.DuyetKhongThanhCong#" + LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.DungChung.DuyetKhongThanhCong"));
                        //  LMessage.ShowMessage("M.DungChung.DuyetKhongThanhCong", LMessage.MessageBoxType.Error);
                    }
                }
                else
                {
                    listResult.Add("M.ResponseMessage.Common.LockDataInvalid#" + LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.ResponseMessage.Common.LockDataInvalid"));
                }
            }
            return listResult;
        }
        private List<string> ThoaiDuyet(string listitem)
        {
            List<string> listResult = new List<string>();
            if (listitem != "")
            {
                List<int> lstID = new List<int>();
                string[] str = listitem.Split(';');
                if (str != null && str.Length > 0)
                    for (int k = 0; k < str.Length; k++)
                    {
                        lstID.Add(Convert.ToInt32(str[k]));
                    }
                List<ClientResponseDetail> ResponseDetail = new List<ClientResponseDetail>();
                UtilitiesProcess process = new UtilitiesProcess();
                if (process.LockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_SAN_PHAM_TDVM,
                DatabaseConstant.Table.TD_SAN_PHAM,
                DatabaseConstant.Action.THOAI_DUYET,
                lstID))
                {
                    bool bResult = new TinDungProcess().HuyDuyetSanPhamTinDung(lstID, ref ResponseDetail);
                    if (bResult)
                    {
                        foreach (ClientResponseDetail cl in ResponseDetail)
                        {
                            listResult.Add(LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, cl.Result) + "#" + LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, cl.Detail.Split('#')[0]));
                        }
                        // Yeu cau Unlook du lieu
                        process.UnlockData(DatabaseConstant.Module.TDVM,
                    DatabaseConstant.Function.TDVM_SAN_PHAM_TDVM,
                    DatabaseConstant.Table.TD_SAN_PHAM,
                    DatabaseConstant.Action.THOAI_DUYET,
                    lstID);

                    }
                    else
                    {
                        listResult.Add("M.DungChung.DuyetKhongThanhCong#" + LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.DungChung.DuyetKhongThanhCong"));
                    }
                }
                else
                {
                    listResult.Add("M.ResponseMessage.Common.LockDataInvalid#" + LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.ResponseMessage.Common.LockDataInvalid"));
                }

            }
            return listResult;
        }
        private List<string> Tuchoi(string listitem)
        {
            List<string> listResult = new List<string>();
            if (listitem != "")
            {
                List<int> lstID = new List<int>();
                string[] str = listitem.Split(';');
                if (str != null && str.Length > 0)
                    for (int k = 0; k < str.Length; k++)
                    {
                        lstID.Add(Convert.ToInt32(str[k]));
                    }
                List<ClientResponseDetail> ResponseDetail = new List<ClientResponseDetail>();
                UtilitiesProcess process = new UtilitiesProcess();
                if (process.LockData(DatabaseConstant.Module.TDVM,
                DatabaseConstant.Function.TDVM_SAN_PHAM_TDVM,
                DatabaseConstant.Table.TD_SAN_PHAM,
                DatabaseConstant.Action.TU_CHOI_DUYET,
                lstID))
                {
                    bool bResult = new TinDungProcess().TuChoiSanPhamTinDung(lstID, ref ResponseDetail);
                    if (bResult)
                    {
                        foreach (ClientResponseDetail cl in ResponseDetail)
                        {
                            listResult.Add(LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, cl.Result) + "#" + LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, cl.Detail.Split('#')[0]));
                        }
                        // Yeu cau Unlook du lieu
                        process.UnlockData(DatabaseConstant.Module.TDVM,
                     DatabaseConstant.Function.TDVM_SAN_PHAM_TDVM,
                     DatabaseConstant.Table.TD_SAN_PHAM,
                     DatabaseConstant.Action.TU_CHOI_DUYET,
                     lstID);
                        LoadDuLieu();
                    }
                    else
                    {
                        listResult.Add("M.DungChung.DuyetKhongThanhCong#" + LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.DungChung.DuyetKhongThanhCong"));
                    }
                }
                else
                {
                    listResult.Add("M.ResponseMessage.Common.LockDataInvalid#" + LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.ResponseMessage.Common.LockDataInvalid"));
                }

            }
            return listResult;
        }

       
    }
}