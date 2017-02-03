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
using Presentation.Process.QuanTriHeThongServiceRef;

namespace Presentation.WebClient.Modules.QTHT.ThamSo
{
    public partial class ucThamSoDS : ControlBase
    {
        #region khaibao
        private string maDonVi = "%";

        // Danh sách các item được chọn trong treeview
        private List<string> lstCheckedKhuVuc = new List<string>();
        private List<string> lstCheckedSanPham = new List<string>();
        List<HT_TSO> dsThamSo = new List<HT_TSO>();
        List<HT_TSO_LOAI> dsThamSoLoai = new List<HT_TSO_LOAI>();
        DataTable dt = new DataTable();
        QuanTriHeThongProcess qtht = new QuanTriHeThongProcess();
        HT_TSO_LOAI obj = new HT_TSO_LOAI();

        List<AutoCompleteEntry> lstSourceDonVi = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstLoaiSanPham = new List<AutoCompleteEntry>();
        protected List<AutoCompleteEntry> lstNguonVon = new List<AutoCompleteEntry>();
        protected List<AutoCompleteEntry> lstPhuongThucTinhLai = new List<AutoCompleteEntry>();
        protected List<AutoCompleteEntry> lstLoaiLaiSuat = new List<AutoCompleteEntry>();
        protected List<AutoCompleteEntry> lstHieuLuc = new List<AutoCompleteEntry>();
        protected List<AutoCompleteEntry> lstMucDichSuDung = new List<AutoCompleteEntry>();
        protected List<AutoCompleteEntry> lstToChuc = new List<AutoCompleteEntry>();

        private string loaiThamSo = BusinessConstant.LoaiThamSo.TW.layGiaTri();

        protected List<AutoCompleteEntry> lstNhomNo = new List<AutoCompleteEntry>();
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            //tvwTree.Attributes.Add("onclick", "OnTreeClick(event)");

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
                dsThamSoLoai = new List<HT_TSO_LOAI>();
                dsThamSoLoai = qtht.layLoaiThamSo();
                tvwTree.Nodes.Clear();
                    TreeNode rootItem = new TreeNode();
                    TreeNode subItem = new TreeNode();

                    rootItem.Text = "Loại tham số";
                    rootItem.Value = null;
                    tvwTree.Nodes.Add(rootItem);
                
                    foreach (HT_TSO_LOAI item in dsThamSoLoai)
                    {
                        TreeNode chidNode = new TreeNode();
                        chidNode.Text = item.TEN_TSO_LOAI;
                        chidNode.Value = item.MA_TSO_LOAI;
                        rootItem.ChildNodes.Add(chidNode);
                    }                

            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        #region giaodien

        #endregion      
        protected void grdThamSoDS_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            e.Item.Attributes.Add("onmouseover", "currColor=this.style.backgroundColor;this.style.backgroundColor='#FDDBA5'");
            e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currColor");
        }
        protected void btnAdd_Click(object sender, EventArgs e)
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


        protected void tvwTree_SelectedNodeChanged(object sender, EventArgs e)
        {
            BuildGrid();
        }
        private string layTenDonViTheoDanhSach(string maDonVi, List<DM_DON_VI> listDonVi)
        {
            foreach (DM_DON_VI item in listDonVi)
            {
                if (maDonVi.Equals(item.MA_DVI))
                    return item.TEN_GDICH;
            }
            return "";
        }
        private string layNgonNguLoaiNguoiDung(string trangthai)
        {
            if (trangthai == "CAP_NVDV")
            {
                return "Người dùng đơn vị";
            }
            else if (trangthai == "CAP_QTDV")
            {
                return "Quản trị đơn vị";
            }
            return "";
        }

        private string layNgonNguTrangThai(string trangthai)
        {
            if (trangthai == "SU_DUNG")
            {
                return "Sử dụng";
            }
            else if (trangthai == "SDU")
            {
                return "Sử dụng";
            }
            return "";
        }
        private void BuildGrid()
        {
            try
            {
                string maDonVi = AppConfig.LoginedUser.UserName;

                // Lấy loại tham số
                if (tvwTree.SelectedNode != null)
                    if (((TreeNode)tvwTree.SelectedNode).Value != null)
                        loaiThamSo = ((TreeNode)tvwTree.SelectedNode).Value.ToString();

                // Ẩn/hiện thông tin đơn vị tùy theo quyền người dùng
                

                // Lấy danh sách tham số theo loại tham số
                dsThamSo = qtht.layThamSoHeThong(loaiThamSo, maDonVi);

                List<HT_TSO> dsThamSoGrid = new List<HT_TSO>();
                if (loaiThamSo != null && loaiThamSo.Length > 1)
                    dsThamSoGrid = dsThamSo.Where(e => e.MA_TSO_LOAI.Equals(loaiThamSo)).ToList();
                else
                    dsThamSoGrid = dsThamSo;

                dt = new DataTable();
                dt.Columns.Add("ID", typeof(int));
                dt.Columns.Add("MA_THAM_SO", typeof(string));
                dt.Columns.Add("STT", typeof(string));
                dt.Columns.Add("TEN_THAM_SO", typeof(string));
                dt.Columns.Add("MO_TA", typeof(string));
                dt.Columns.Add("GIA_TRI", typeof(string));
                dt.Columns.Add("TRANG_THAI", typeof(string));
                dt.Columns.Add("LOAI_THAM_SO", typeof(string));
                dt.Columns.Add("MA_DVI", typeof(string));
                int stt = 0;
                foreach (var item in dsThamSo)
                {
                    DataRow r = dt.NewRow();
                    stt = stt + 1;
                    r[0] = item.ID;
                    r[1] = item.MA_TSO;
                    r[2] = stt;
                    r[3] = item.TEN_TSO;
                    r[4] = item.MO_TA;
                    r[5] = item.GIA_TRI;
                    r[6] = layNgonNguTrangThai(item.TTHAI_BGHI);
                    r[7] = item.MA_TSO_LOAI;
                    r[8] = item.MA_DVI_QLY;
                    dt.Rows.Add(r);
                }
                // đổ source lên lưới
                grdThamSoDS.DataSource = dt;
                grdThamSoDS.DataBind();
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        protected void tvwTree_Load(Object sender,EventArgs e)
        {
            BuildGrid();
        }
    }
}