using Presentation.WebClient.Business;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Utilities.Common;
using Presentation.WebClient.Business.CustomControl;
using Presentation.Process;
using System.Data;
using Presentation.Process.Common;
using Presentation.Process.KeToanServiceRef;

namespace Presentation.WebClient.Modules.GDKT.PhanLoai
{
    public partial class ucPhanLoaiDS : ControlBase
    {
        #region khaibao
        private string maDonVi = "%";

        // Danh sách các item được chọn trong treeview
        private List<string> lstCheckedKhuVuc = new List<string>();
        private List<string> lstCheckedPhanLoai = new List<string>();

        List<AutoCompleteEntry> lstSourceDonVi = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstLoaiPhanLoai = new List<AutoCompleteEntry>();
        #endregion
        #region khoitao
        protected void Page_Load(object sender, EventArgs e)
        {
            tvwPhanLoai.Attributes.Add("onclick", "OnTreeClick(event)");

            if (!IsPostBack)
            {
                KhoiTaoControl();
                BuildTree();
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
        void KhoiTaoControl()
        {
            try
            {
                string Dislay = LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.DungChung.TatCa");

                List<string> lstDieuKien = new List<string>();
                string maTruyVan = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();

                // Gán giá trị điều kiện

                AutoComboBox auto = new AutoComboBox();
                lstDieuKien = new List<string>();
                lstDieuKien.Add("'" + DatabaseConstant.ToChucDonVi.HSO.getValue() + "', '" + DatabaseConstant.ToChucDonVi.CNH.getValue() + "'");
                auto.GenAutoComboBox(ref lstSourceDonVi, ref cmbDonVi, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DONVITHEOLOAI.getValue(), lstDieuKien, AppConfig.LoginedUser.MaDonVi);

                if (AppConfig.LoginedUser.LoaiNguoiSuDung.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_NVDV.layGiaTri()) || AppConfig.LoginedUser.LoaiNguoiSuDung.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_QTDV.layGiaTri()))
                    cmbDonVi.Enabled = false;

            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }
        #endregion


        #region Xu ly nghiep vu
        private void LoadDuLieu()
        {
            try
            {
                KeToanProcess ketoanProcess = new KeToanProcess();
                string maDonVi = cmbDonVi.SelectedItem.Value;
                string MaTTNVu = "";

                if (cbChoduyet.Checked)
                    MaTTNVu = MaTTNVu + "''" + cbChoduyet.Value + "'',";
                if (cbDaduyet.Checked)
                    MaTTNVu = MaTTNVu + "''" + cbDaduyet.Value + "'',";
                if (cbThoaiduyet.Checked)
                    MaTTNVu = MaTTNVu + "''" + cbThoaiduyet.Value + "'',";
                if (cbDaduyet.Checked)
                    MaTTNVu = MaTTNVu + "''" + cbDaduyet.Value + "'',";
                if (MaTTNVu != "")
                {
                    MaTTNVu = "(" + MaTTNVu.Substring(0, MaTTNVu.Length - 1) + ")";
                }
                string ListMaPloai = "";
                foreach (TreeNode item in tvwPhanLoai.CheckedNodes)
                {
                    ListMaPloai += item.Value.ToString() + ",";
                }
                if (ListMaPloai.Length > 0)
                    ListMaPloai = "(" + ListMaPloai.Substring(1) + ")";

                grdPhanLoaiDS.DataSource = ketoanProcess.getDanhSachMaPhanLoai(maDonVi, MaTTNVu, BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri(), ListMaPloai);
                grdPhanLoaiDS.DataBind();

            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
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

        private void BuildTree()
        {
            tvwPhanLoai.Nodes.Clear();

            KeToanProcess process = new KeToanProcess();
            AutoComboBox au = new AutoComboBox();
            try
            {
                string maDonVi = cmbDonVi.SelectedItem.Value;
                if (maDonVi != null)
                {
                    //trvPhanLoai.Items.Clear();
                    DataSet ds = process.getDanhSachMaPhanLoai(maDonVi, "(''" + BusinessConstant.TrangThaiNghiepVu.DA_DUYET.layGiaTri() + "'')", BusinessConstant.TrangThaiBanGhi.SU_DUNG.layGiaTri(), "%");

                    tvwPhanLoai.Nodes.Clear();
                    if (ds != null && ds.Tables.Count > 0)
                    {
                        DataView v_dv = ds.Tables[0].DefaultView;
                        v_dv.RowFilter = "ID_PLOAI_CHA IS NULL";
                        if (v_dv.Count > 0)
                        {
                            for (int i = 0; i < v_dv.Count; i++)
                            {
                                TreeNode v_objNode = new TreeNode();
                                v_objNode.Text = v_dv[i]["TEN_PLOAI"].ToString();
                                v_objNode.Value = v_dv[i]["ID"].ToString();
                                if (i == 0)
                                {
                                    v_objNode.Selected = true;
                                }
                                BuidSubNode(v_objNode, v_dv);
                                tvwPhanLoai.Nodes.Add(v_objNode);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
            finally
            {
                au = null;
                process = null;
            }

            tvwPhanLoai.CollapseAll();
            tvwPhanLoai.ShowCheckBoxes = TreeNodeTypes.All;
        }

        private void BuidSubNode(TreeNode pv_objNode, DataView pv_dv)
        {
            string v_strIDParent = pv_objNode.Value;
            string v_strTempFilter = pv_dv.RowFilter;
            pv_dv.RowFilter = "";
            pv_dv.RowFilter = "ID_PLOAI_CHA=" + v_strIDParent;

            if (pv_dv.Count > 0)
            {
                for (int i = 0; i < pv_dv.Count; i++)
                {
                    TreeNode v_objNode = new TreeNode();
                    v_objNode.Text = pv_dv[i]["TEN_PLOAI"].ToString();
                    v_objNode.Value = pv_dv[i]["ID"].ToString();
                    pv_objNode.ChildNodes.Add(v_objNode);
                    BuidSubNode(v_objNode, pv_dv);
                }
            }

            pv_dv.RowFilter = "";
            pv_dv.RowFilter = v_strTempFilter;

        }
        #endregion
        #region giaodien
        protected void cmbLoaiVay_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        #endregion


        protected void grdPhanLoaiDS_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            e.Item.Attributes.Add("onmouseover", "currColor=this.style.backgroundColor;this.style.backgroundColor='#FDDBA5'");
            e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currColor");
        }

        protected void btnTimkiem_Click(object sender, EventArgs e)
        {
            LoadDuLieu();
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            string filename = DateTime.Now.Ticks.ToString() + "xls";
            new ExportExcel().ExportToExcel((DataTable)grdPhanLoaiDS.DataSource, Server.MapPath("Modules") + "\\" + filename);
            Response.Redirect("Modules" + "\\" + filename);

        }

        private List<string> XuLy(string listitem, DatabaseConstant.Action action, string alert)
        {
            List<string> listResult = new List<string>();
            List<ClientResponseDetail> lstResponse = new List<ClientResponseDetail>();
            if (listitem != "")
            {
                List<KT_PLOAI> lstKtPloai = new List<KT_PLOAI>();
                string[] str = listitem.Split(';');
                if (str != null && str.Length > 0)
                    for (int k = 0; k < str.Length; k++)
                    {
                        KT_PLOAI obj = new KT_PLOAI();
                        obj.ID = Convert.ToInt32(str[k]);
                        lstKtPloai.Add(obj);
                    }

                bool bResult = new KeToanProcess().XuLyPhanLoaiTaiKhoanDS(lstKtPloai.ToArray(), ref lstResponse, action);
                if (bResult)
                {
                    foreach (ClientResponseDetail cl in lstResponse)
                    {
                        listResult.Add(LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, cl.Result) + "#" + LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, cl.Detail.Split('#')[0]));
                    }

                }
                else
                {
                    listResult.Add(alert + "#" + LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, "M.DungChung.DuyetKhongThanhCong"));
                }
            }
            return listResult;
        }

        private List<string> Xoa(string listitem)
        {
            return XuLy(listitem, DatabaseConstant.Action.XOA, "M.DungChung.XoaKhongThanhCong");
        }
        private List<string> Duyet(string listitem)
        {
            return XuLy(listitem, DatabaseConstant.Action.DUYET, "M.DungChung.DuyetKhongThanhCong");
        }
        private List<string> ThoaiDuyet(string listitem)
        {
            return XuLy(listitem, DatabaseConstant.Action.THOAI_DUYET, "M.DungChung.ThoaiDuyetKhongThanhCong");
        }
        private List<string> Tuchoi(string listitem)
        {
            return XuLy(listitem, DatabaseConstant.Action.TU_CHOI_DUYET, "M.DungChung.TuchoiKhongThanhCong");
        }
    }
}