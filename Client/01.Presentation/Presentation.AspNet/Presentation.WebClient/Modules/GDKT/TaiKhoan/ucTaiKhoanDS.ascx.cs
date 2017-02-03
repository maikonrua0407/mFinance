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

namespace Presentation.WebClient.Modules.GDKT.TaiKhoan
{
    public partial class ucTaiKhoanDS : ControlBase
    {
        #region khaibao
        private string maDonVi = "%";

        // Danh sách các item được chọn trong treeview
        private List<string> lstCheckedKhuVuc = new List<string>();
        private List<string> lstCheckedSanPham = new List<string>();

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


                teldtSoDuNgay.Text = "";
                List<string> lstDieuKien = new List<string>();
                string maTruyVan = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();

                AutoComboBox auto = new AutoComboBox();
                lstDieuKien.Add(AppConfig.LoginedUser.UserName);
                lstDieuKien.Add(AppConfig.LoginedUser.MaDonVi);
                lstDieuKien.Add(DatabaseConstant.ToChucDonVi.PGD.getValue());
                auto.GenAutoComboBox(ref lstSourceDonVi, ref cmbDonVi, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DONVI_THEOPVI.getValue(), lstDieuKien, AppConfig.LoginedUser.MaDonViGiaoDich);

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
                KeToanProcess process = new KeToanProcess();
                string maDonVi = "(''" + cmbDonVi.SelectedItem.Value + "'',";
                maDonVi = maDonVi.Substring(0, maDonVi.Length - 1) + ")";

                string ngayDL = "";
                if (teldtSoDuNgay.Text != "")
                    ngayDL = LDateTime.DateToString(LDateTime.StringToDate(teldtSoDuNgay.Text, "dd/MM/yyyy"), ApplicationConstant.defaultDateTimeFormat);

                string ListMaPloai = "";
                foreach (TreeNode item in tvwPhanLoai.CheckedNodes)
                {
                    ListMaPloai += "''" + item.Value.ToString() + "'',";
                }
                if (ListMaPloai.Length > 0)
                    ListMaPloai = "(" + ListMaPloai.Substring(0, ListMaPloai.Length - 1) + ")";

                DataTable dtThamSo = null;
                LDatatable.MakeParameterTable(ref dtThamSo);
                LDatatable.AddParameter(ref dtThamSo, "@MaDonVi", "STRING", maDonVi);
                LDatatable.AddParameter(ref dtThamSo, "@MaPhanLoai", "STRING", ListMaPloai);
                LDatatable.AddParameter(ref dtThamSo, "@MaTK", "STRING", txtMaPhanLoaiTK.Text);
                LDatatable.AddParameter(ref dtThamSo, "@TenTK", "STRING", "");
                LDatatable.AddParameter(ref dtThamSo, "@NgayDL", "STRING", ngayDL);
                LDatatable.AddParameter(ref dtThamSo, "@SoDuTu", "DECIMAL", "0");
                LDatatable.AddParameter(ref dtThamSo, "@SoDuDen", "DECIMAL", "0");
                LDatatable.AddParameter(ref dtThamSo, "@TinhChat", "STRING", "");

                grdTaiKhoanDS.DataSource = process.getDanhSachTaiKhoanChiTiet(dtThamSo, "DANH_SACH");
                grdTaiKhoanDS.DataBind();

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
                        v_dv.RowFilter = "MA_PLOAI_CHA = ''";
                        if (v_dv.Count > 0)
                        {
                            for (int i = 0; i < v_dv.Count; i++)
                            {
                                TreeNode v_objNode = new TreeNode();
                                v_objNode.Text = v_dv[i]["TEN_PLOAI"].ToString();
                                v_objNode.Value = v_dv[i]["MA_PLOAI"].ToString();
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
            pv_dv.RowFilter = "MA_PLOAI_CHA = '" + v_strIDParent + "'";

            if (pv_dv.Count > 0)
            {
                for (int i = 0; i < pv_dv.Count; i++)
                {
                    TreeNode v_objNode = new TreeNode();
                    v_objNode.Text = pv_dv[i]["TEN_PLOAI"].ToString();
                    v_objNode.Value = pv_dv[i]["MA_PLOAI"].ToString();
                    pv_objNode.ChildNodes.Add(v_objNode);
                    BuidSubNode(v_objNode, pv_dv);
                }
            }

            pv_dv.RowFilter = "";
            pv_dv.RowFilter = v_strTempFilter;

        }
        #endregion
        #region giaodien
        #endregion


        protected void grdTaiKhoanDS_ItemDataBound(object sender, DataGridItemEventArgs e)
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
            new Business.CustomControl.ExportExcel().ExportToExcel((DataTable)grdTaiKhoanDS.DataSource, Server.MapPath("Modules") + "\\" + filename);
            Response.Redirect("Modules" + "\\" + filename);

        }

        private List<string> XuLy(string listitem, DatabaseConstant.Action action, string alert)
        {
            List<string> listResult = new List<string>();
            List<ClientResponseDetail> lstResponse = new List<ClientResponseDetail>();
            if (listitem != "")
            {
                List<KT_TKHOAN> lstKtTkhoan = new List<KT_TKHOAN>();
                string[] str = listitem.Split(';');
                if (str != null && str.Length > 0)
                    for (int k = 0; k < str.Length; k++)
                    {
                        KT_TKHOAN obj = new KT_TKHOAN();
                        obj.ID = Convert.ToInt32(str[k]);
                        lstKtTkhoan.Add(obj);
                    }

                ApplicationConstant.ResponseStatus bResult = new KeToanProcess().TaiKhoanChiTietDS(action, lstKtTkhoan, ref lstResponse);
                if (bResult == ApplicationConstant.ResponseStatus.THANH_CONG)
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