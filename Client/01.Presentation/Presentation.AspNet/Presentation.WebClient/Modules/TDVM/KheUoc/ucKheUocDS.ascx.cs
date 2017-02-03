using Presentation.WebClient.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
using System.Globalization;

namespace Presentation.WebClient.Modules.TDVM.KheUoc
{
    public partial class ucKheUocDS : ControlBase
    {
        #region khaibao
        private string maDonVi = "%";

        // Danh sách các item được chọn trong treeview
        private List<string> lstCheckedKhuVuc = new List<string>();
        private List<string> lstCheckedSanPham = new List<string>();
        private static DataTable dt;

        List<AutoCompleteEntry> lstSourceDonVi = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstLoaiSanPham = new List<AutoCompleteEntry>();
        protected List<AutoCompleteEntry> lstSanPham = new List<AutoCompleteEntry>();
        protected List<AutoCompleteEntry> lstNhomNo = new List<AutoCompleteEntry>();
        protected List<AutoCompleteEntry> lstThoiHanVayTu = new List<AutoCompleteEntry>();
        protected List<AutoCompleteEntry> lstThoiHanVayDen = new List<AutoCompleteEntry>();
        #endregion
        #region khoitao
        protected void Page_Load(object sender, EventArgs e)
        {
            tvwKhuVuc.Attributes.Add("onclick", "OnTreeClick(event)");
            cball.Attributes.Add("!", "SelectAllChecBoxWithName(this)");

            if (!IsPostBack)
            {
                BuildTreeKhuVuc();
                KhoiTaoControl();
                tvwKhuVuc.CollapseAll();
                //LoadDuLieu();
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


                txtNgayNhanNoTu.Text = "";
                txtNgayNhanNoDen.Text = "";
                txtNgayDaoHanTu.Text = "";
                txtNgayDaoHanDen.Text = "";
                List<string> lstDieuKien = new List<string>();
                string maTruyVan = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();

                // Gán giá trị điều kiện
                lstDieuKien = new List<string>();
                lstDieuKien.Add("''" + AppConfig.LoginedUser.MaDonVi + "''");             
                lstDieuKien.Add("%");
                lstDieuKien.Add("0");
                lstDieuKien.Add("0");
                //lstDieuKien.Add(DatabaseConstant.getValue(DatabaseConstant.DanhMuc.LOAI_SAN_PHAM_TDUNG));
                lstSanPham.Add(new AutoCompleteEntry(Dislay, "%", "%"));
                KhoiTaoGiaTriComboBox(ref lstSanPham, ref cmbSanPham, "COMBOBOX_SAN_PHAM_TD", lstDieuKien);

                lstDieuKien = new List<string>();
                lstDieuKien.Add(DatabaseConstant.getValue(DatabaseConstant.DanhMuc.NHOM_NO));
                lstNhomNo.Add(new AutoCompleteEntry(Dislay, "%", "%"));
                KhoiTaoGiaTriComboBox(ref lstNhomNo, ref cmbNhomNo, maTruyVan, lstDieuKien);

                lstDieuKien = new List<string>();
                lstDieuKien.Add(DatabaseConstant.getValue(DatabaseConstant.DanhMuc.DVI_TINH_KY_HAN));
                lstThoiHanVayTu.Add(new AutoCompleteEntry(Dislay, "%", "%"));
                KhoiTaoGiaTriComboBox(ref lstThoiHanVayTu, ref cmbThoiHanVayTu, maTruyVan, lstDieuKien);

                lstDieuKien = new List<string>();
                lstDieuKien.Add(DatabaseConstant.getValue(DatabaseConstant.DanhMuc.DVI_TINH_KY_HAN));
                lstThoiHanVayDen.Add(new AutoCompleteEntry(Dislay, "%", "%"));
                KhoiTaoGiaTriComboBox(ref lstThoiHanVayDen, ref cmbThoiHanVayDen, maTruyVan, lstDieuKien);
                
                //if (AppConfig.LoginedUser.LoaiNguoiSuDung.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_NVDV.layGiaTri()) || AppConfig.LoginedUser.LoaiNguoiSuDung.Equals(BusinessConstant.LoaiNguoiSuDung.CAP_QTDV.layGiaTri()))
                //    cmbDonVi.Enabled = false;

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
                TinDungProcess tindungProcess = new TinDungProcess();
                string sMaTrangThaiNVu = "";

                if (cbChoduyet.Checked)
                    sMaTrangThaiNVu = sMaTrangThaiNVu + "''" + cbChoduyet.Value + "'',";
                if (cbDaduyet.Checked)
                    sMaTrangThaiNVu = sMaTrangThaiNVu + "''" + cbDaduyet.Value + "'',";
                if (cbThoaiduyet.Checked)
                    sMaTrangThaiNVu = sMaTrangThaiNVu + "''" + cbThoaiduyet.Value + "'',";
                if (cbDaduyet.Checked)
                    sMaTrangThaiNVu = sMaTrangThaiNVu + "''" + cbDaduyet.Value + "'',";
                if (sMaTrangThaiNVu != "")
                {
                    sMaTrangThaiNVu = "(" + sMaTrangThaiNVu.Substring(0, sMaTrangThaiNVu.Length - 1) + ")";
                }
                string NgayNhanNoTu = "";
                if (txtNgayNhanNoTu.Text != "")
                {
                    NgayNhanNoTu = txtNgayNhanNoTu.Text != null ? LDateTime.DateToString(DateTime.ParseExact(txtNgayNhanNoTu.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture), ApplicationConstant.defaultDateTimeFormat) : "";
                }

                string NgayNhanNoDen="";
                if (txtNgayNhanNoTu.Text != "")
                { 
                    NgayNhanNoDen= txtNgayNhanNoTu.Text != null ? LDateTime.DateToString(DateTime.ParseExact(txtNgayNhanNoTu.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture), ApplicationConstant.defaultDateTimeFormat) : "";
                }
                string NgayDaoHanTu = "";
                if (txtNgayNhanNoTu.Text != "")
                {
                    NgayDaoHanTu = txtNgayNhanNoTu.Text != null ? LDateTime.DateToString(DateTime.ParseExact(txtNgayNhanNoTu.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture), ApplicationConstant.defaultDateTimeFormat) : "";
                }
                string NgayDaoHanDen = "";
                if (txtNgayNhanNoTu.Text != "")
                {
                    NgayDaoHanDen = txtNgayNhanNoTu.Text != null ? LDateTime.DateToString(DateTime.ParseExact(txtNgayNhanNoTu.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture), ApplicationConstant.defaultDateTimeFormat) : "";
                }
                
                string SoTienGNTu = txtSoTienGNTu.Text != null ? txtSoTienGNTu.Text.ToString() : "";
                string SoTienGNDen = txtSoTienGNTu.Text != null ? txtSoTienGNTu.Text.ToString() : "";
                string SoDuTu = txtSoTienGNTu.Text != null ? txtSoTienGNTu.Text.ToString() : "";
                string SoDuDen = txtSoTienGNTu.Text != null ? txtSoTienGNTu.Text.ToString() : "";
                string ThoiHanTu = "";
                string ThoiHanDen = "";
                string ThoiHanDViTu = "%";
                string ThoiHanDViDen = "%";
                string LaiSuatTu = "";
                string LaiSuatDen ="";
                string LoaiGiayTo = "%";
                string MaSanPham = "%";
                //if (cmbSanPham.SelectedIndex > -1) MaSanPham = lstSanPhamTinDung.ElementAt(cmbSanPham.SelectedIndex).KeywordStrings.First();
                string SoGiayTo ="%";
                //if (tvwKhuVuc.SelectedNode == null)
                    //tvwKhuVuc.SelectedNode = tvwKhuVuc.Nodes[0];
                string ListKVuc = "CNH0001";
                //if (((TreeNode)tvwKhuVuc.SelectedNode).Value.ToString().Substring(0, 3).Equals("DVI"))
                //{
                //    TreeNode itemDVI = (TreeNode)tvwKhuVuc.SelectedNode;
                //    foreach (TreeNode item in itemDVI.ChildNodes)
                //    {
                //        if (item.Value.ToString().Substring(0, 3).Equals("CNH"))
                //            ListKVuc += ",''" + item.Value.ToString() + "''";
                //    }
                //    ListKVuc = ListKVuc.Substring(3);
                //    ListKVuc = ListKVuc.Substring(0, ListKVuc.Length - 2);
                //}
                //else
                //    ListKVuc = ((TreeNode)tvwKhuVuc.SelectedNode).Value.ToString();

                // Phân trang
                int StartRow = 1;
                int EndRow = ClientInformation.SoLuongBanGhi;

                
                DataSet ds = new TinDungProcess().getDanhSachKUOCVM(sMaTrangThaiNVu, txtSoDonVayVon.Text, txtSoKheUoc.Text, NgayNhanNoTu, NgayNhanNoDen, NgayDaoHanTu, NgayDaoHanDen, SoTienGNTu, SoTienGNDen, SoDuTu, SoDuDen, ThoiHanTu, ThoiHanDen, ThoiHanDViTu, ThoiHanDViDen, LaiSuatTu, LaiSuatDen, txtMaKH.Text, txtTenKH.Text, LoaiGiayTo, SoGiayTo, txtDienThoai.Text, txtEmail.Text, MaSanPham, ListKVuc, AppConfig.LoginedUser.UserName.ToString(), AppConfig.LoginedUser.MaDonViQuanLy.ToString(), StartRow.ToString(), "15");
                grdDangKyKheUocDS.DataSource = ds.Tables[0];
                grdDangKyKheUocDS.DataBind();
                

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
        
        void BuildTreeKhuVuc()
        {

            try
            {
                TinDungProcess tindunProcess = new TinDungProcess();
                DataSet ds = tindunProcess.getDanhSachDonVi(AppConfig.LoginedUser.MaDonVi, AppConfig.LoginedUser.UserName);
                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];

                    DataRow drRoot = dt.Rows[0];
                    tvwKhuVuc.Nodes.Clear();
                    TreeNode rootItem = new TreeNode();

                    rootItem.Text = drRoot["ten_gdich"].ToString();
                    rootItem.Value = drRoot["ma_dvi"].ToString();
                    //rootItem.Checked = true;
                    tvwKhuVuc.Nodes.Add(rootItem);
                    BuildSubTreeKhuVuc(rootItem);

                }


            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }
        void BuildSubTreeKhuVuc(TreeNode Item)
        {
            try
            {

                List<DataRow> lstDataRow = null;
                lstDataRow = dt.Select().OrderBy(row => row[2]).ToList();

                foreach (DataRow row in lstDataRow)
                {
                    if (row["ma_dvi_cha"].ToString() == Item.Value.ToString())
                    {
                        TreeNode subItem = new TreeNode();
                        subItem.Text = row["ten_gdich"].ToString();
                        subItem.Value = row["ma_dvi"].ToString();
                        //subItem.Checked = true;
                        Item.ChildNodes.Add(subItem);
                        BuildSubTreeKhuVuc(subItem);
                    }
                }
            }
            catch
            { }
            
        }
        #endregion
        #region giaodien
        protected void cmbLoaiVay_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        #endregion


        protected void grdDangKyKheUocDS_ItemDataBound(object sender, DataGridItemEventArgs e)
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
            new Business.CustomControl.ExportExcel().ExportToExcel((DataTable)grdDangKyKheUocDS.DataSource, Server.MapPath("Modules") + "\\" + filename);
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

        protected void tvwKhuVuc_TreeNodeCheckChanged(object sender, EventArgs e)
        {

        }

        protected void txtTimKiemNhanh_TextChanged(object sender, EventArgs e)
        {
          
        }
    }
}