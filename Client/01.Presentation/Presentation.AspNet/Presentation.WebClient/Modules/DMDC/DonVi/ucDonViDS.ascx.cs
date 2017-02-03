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
namespace Presentation.WebClient.Modules.DMDC.DonVi
{
    public partial class ucDonViDS : ControlBase
    {
        #region khaibao
        private string maDonVi = "%";

        // Danh sách các item được chọn trong treeview
        private List<string> lstCheckedKhuVuc = new List<string>();
        private List<string> lstCheckedSanPham = new List<string>();
        private static DataTable dt;

        List<AutoCompleteEntry> lstSourceDonVi = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstLoaiSanPham = new List<AutoCompleteEntry>();
        protected List<AutoCompleteEntry> lstNguonVon = new List<AutoCompleteEntry>();
        protected List<AutoCompleteEntry> lstPhuongThucTinhLai = new List<AutoCompleteEntry>();
        protected List<AutoCompleteEntry> lstLoaiLaiSuat = new List<AutoCompleteEntry>();
        protected List<AutoCompleteEntry> lstHieuLuc = new List<AutoCompleteEntry>();
        protected List<AutoCompleteEntry> lstMucDichSuDung = new List<AutoCompleteEntry>();
        protected List<AutoCompleteEntry> lstToChuc = new List<AutoCompleteEntry>();
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            tvwTree.Attributes.Add("onclick", "OnTreeClick(event)");

            if (!IsPostBack)
            {
                LoadDuLieu();
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
        void KhoiTaoGiaTriComboBox(ref List<AutoCompleteEntry> lstAutoComplete, ref DropDownList cmbControl, string sMaTruyVan, List<string> lstDKien = null)
        {
            AutoComboBox autoComboBox = new AutoComboBox();
            autoComboBox.GenAutoComboBox(ref lstAutoComplete, ref cmbControl, sMaTruyVan, lstDKien);
        }
             
        private void LoadDuLieu()
        {
            try
            {
                DanhMucProcess danhmucProcess = new DanhMucProcess();
                DataSet ds = danhmucProcess.getDanhSachDonVi();
                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
 
                    grdDangKyDonViDS.DataSource = ds;
                    grdDangKyDonViDS.DataBind();
                    
                    DataRow drRoot = dt.Rows[0];
                    tvwTree.Nodes.Clear();
                    TreeNode rootItem = new TreeNode();

                    rootItem.Text = drRoot["ten_gdich"].ToString();
                    rootItem.Value = drRoot["ma_dvi"].ToString();
                    tvwTree.Nodes.Add(rootItem);
                    BuildTree(rootItem);

                }


            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void BuildTree(TreeNode item)
        {

            try
            {
                
                List<DataRow> lstDataRow = null;
                lstDataRow = dt.Select().OrderBy(row => row[2]).ToList();

                foreach (DataRow row in lstDataRow)
                {
                    if (row["ma_dvi_cha"].ToString() == item.Value.ToString())
                    {
                        TreeNode subItem = new TreeNode();
                        subItem.Text = row["ten_gdich"].ToString();
                        subItem.Value = row["ma_dvi"].ToString();
                        //subItem.IsExpanded = true;
                        //subItem.IsChecked = false;
                        item.ChildNodes.Add(subItem);
                        BuildTree(subItem);
                    }
                }
            }
            catch
            { }

            tvwTree.ExpandAll();
            tvwTree.ShowCheckBoxes = TreeNodeTypes.All;
        }
        #region giaodien

        #endregion

        private void txtTimKiemNhanh_TextChanged(object sender, EventArgs e)
        {

            txtTimKiemNhanh.Text = "aaaaa";
            //txtTimKiemNhanh.Focus();

        }
        protected void grdDangKyDonViDS_ItemDataBound (object sender, DataGridItemEventArgs e)
        {
            e.Item.Attributes.Add("onmouseover", "currColor=this.style.backgroundColor;this.style.backgroundColor='#FDDBA5'");
            e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currColor");
        }
        protected void btnAdd_Click (object sender, EventArgs e)
        {
            Response.Redirect("~/Default.aspx?TabID=990016");
        }
        protected void btnTimkiem_Click(object sender, EventArgs e)
        {
            LoadDuLieu();
        }
    

        protected void btnExport_Click(object sender, EventArgs e)
        {
            string filename = DateTime.Now.Ticks.ToString() + "xls";
            //new Business.CustomControl.ExportExcel().ExportToExcel((DataTable)grdDangKySanPhamDS.DataSource,Server.MapPath("Modules") + "\\" + filename);
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
                if (process.LockData(DatabaseConstant.Module.DMDC,
                DatabaseConstant.Function.DC_DM_DON_VI,
                DatabaseConstant.Table.DM_DON_VI,
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
                        process.UnlockData(DatabaseConstant.Module.DMDC,
                   DatabaseConstant.Function.DC_DM_DON_VI,
                   DatabaseConstant.Table.DM_DON_VI,
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
                if (process.LockData(DatabaseConstant.Module.DMDC,
                DatabaseConstant.Function.DC_DM_DON_VI,
                DatabaseConstant.Table.DM_DON_VI, DatabaseConstant.Action.DUYET, lstID))
                {

                    bool bResult = new TinDungProcess().DuyetSanPhamTinDung(lstID, ref ResponseDetail);
                    if (bResult)
                    {
                        foreach (ClientResponseDetail cl in ResponseDetail)
                        {
                            listResult.Add(LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, cl.Result) + "#" + LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, cl.Detail.Split('#')[0]));
                        }
                        // Yeu cau Unlook du lieu
                        process.UnlockData(DatabaseConstant.Module.DMDC,
                        DatabaseConstant.Function.DC_DM_DON_VI, DatabaseConstant.Table.DM_DON_VI,
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
                if (process.LockData(DatabaseConstant.Module.DMDC,
                DatabaseConstant.Function.DC_DM_DON_VI,
                DatabaseConstant.Table.DM_DON_VI,
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
                        process.UnlockData(DatabaseConstant.Module.DMDC,
                    DatabaseConstant.Function.DC_DM_DON_VI,
                    DatabaseConstant.Table.DM_DON_VI,
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
                if (process.LockData(DatabaseConstant.Module.DMDC,
                DatabaseConstant.Function.DC_DM_DON_VI,
                DatabaseConstant.Table.DM_DON_VI,
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
                        process.UnlockData(DatabaseConstant.Module.DMDC,
                     DatabaseConstant.Function.DC_DM_DON_VI,
                     DatabaseConstant.Table.DM_DON_VI,
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