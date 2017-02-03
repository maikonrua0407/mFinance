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

namespace Presentation.WebClient.Modules.KHTV.KhachHang
{
    public partial class ucKhachHangDS : ControlBase
    {
        #region khaibao
        private string maDonVi = "%";

        // Danh sách các item được chọn trong treeview
        private List<string> lstCheckedKhuVuc = new List<string>();
        private List<string> lstCheckedSanPham = new List<string>();
        DataTable dt = new DataTable();

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
            tvwDonVi.Attributes.Add("onclick", "OnTreeClick(event)");

            if (!IsPostBack)
            {
                BuildTree();
                tvwDonVi.CollapseAll();
                KhoiTaoControl();
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
                lstDieuKien = new List<string>();
                lstDieuKien.Add(DatabaseConstant.getValue(DatabaseConstant.DanhMuc.LOAI_KHACH_HANG));
                lstPhuongThucTinhLai.Add(new AutoCompleteEntry(Dislay, "%", "%"));
                KhoiTaoGiaTriComboBox(ref lstPhuongThucTinhLai, ref cmbLoaiKHang, maTruyVan, lstDieuKien);

                lstDieuKien = new List<string>();
                lstDieuKien.Add(DatabaseConstant.getValue(DatabaseConstant.DanhMuc.LOAI_HINH_TO_CHUC));
                lstMucDichSuDung.Add(new AutoCompleteEntry(Dislay, "%", "%"));
                KhoiTaoGiaTriComboBox(ref lstMucDichSuDung, ref cmbLoaiHinhTC, maTruyVan, lstDieuKien);

                lstDieuKien = new List<string>();
                lstDieuKien.Add(DatabaseConstant.getValue(DatabaseConstant.DanhMuc.NGHE_NGHIEP));
                lstLoaiSanPham.Add(new AutoCompleteEntry(Dislay, "%", "%"));
                KhoiTaoGiaTriComboBox(ref lstLoaiSanPham, ref cmbNgheNghiep, maTruyVan, lstDieuKien);

                lstDieuKien = new List<string>();
                lstDieuKien.Add(DatabaseConstant.getValue(DatabaseConstant.DanhMuc.NGANH_KINH_TE));
                lstHieuLuc.Add(new AutoCompleteEntry(Dislay, "%", "%"));
                KhoiTaoGiaTriComboBox(ref lstHieuLuc, ref cmbNganhKinhTe, maTruyVan, lstDieuKien);

                lstDieuKien = new List<string>();
                lstDieuKien.Add(DatabaseConstant.getValue(DatabaseConstant.DanhMuc.DAN_TOC));
                lstNguonVon.Add(new AutoCompleteEntry(Dislay, "%", "%"));
                KhoiTaoGiaTriComboBox(ref lstNguonVon, ref cmbDanToc, maTruyVan, lstDieuKien);

                lstDieuKien = new List<string>();
                lstDieuKien.Add(DatabaseConstant.getValue(DatabaseConstant.DanhMuc.GIOI_TINH));
                lstLoaiLaiSuat.Add(new AutoCompleteEntry(Dislay, "%", "%"));
                KhoiTaoGiaTriComboBox(ref lstLoaiLaiSuat, ref cmbGioiTinh, maTruyVan, lstDieuKien);               
                

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
                KhachHangProcess process = new KhachHangProcess();
                DataSet ds = process.getTreeView(AppConfig.LoginedUser.MaDonViQuanLy, AppConfig.LoginedUser.UserName);


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
            try
            {
                TinDungProcess tindunProcess = new TinDungProcess();
                DataSet ds = tindunProcess.getDanhSachDonVi(AppConfig.LoginedUser.MaDonVi, AppConfig.LoginedUser.UserName);
                if (ds != null && ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];

                    DataRow drRoot = dt.Rows[0];
                    tvwDonVi.Nodes.Clear();
                    TreeNode rootItem = new TreeNode();

                    rootItem.Text = drRoot["ten_gdich"].ToString();
                    rootItem.Value = drRoot["ma_dvi"].ToString();
                    //rootItem.Checked = true;
                    tvwDonVi.Nodes.Add(rootItem);
                    BuildSubTree(rootItem);

                }


            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }
        void BuildSubTree(TreeNode Item)
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
                        BuildSubTree(subItem);
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


        protected void grdDangKyKHangDS_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            e.Item.Attributes.Add("onmouseover", "currColor=this.style.backgroundColor;this.style.backgroundColor='#FDDBA5'");
            e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currColor");
        }
        private void TimKiem()
        {
            DataTable dt = null;
            LDatatable.MakeParameterTable(ref dt);

            // Lay du lieu treeview cho tim kiem
            string ma_don_vi = "1";
            string ma_khu_vuc = "-1";
            string ma_cum = "-1";
            string ma_nhom = "-1";
            string userName = AppConfig.LoginedUser.UserName;
            
            if (tvwDonVi.SelectedNode != null)
            {
                TreeNode item = tvwDonVi.SelectedNode as TreeNode;
                string level = item.Value.ToString().Split('#')[0];
                string[] path = item.Value.ToString().Split('#')[1].Split('/');
                string type = item.Value.ToString().Split('#')[2];
                if (type == "DVI")
                {
                    ma_don_vi = path[path.Length - 1];
                }
                else
                {
                    ma_don_vi = path[0];
                    //ma_khu_vuc = path[1];
                    //ma_cum = path[1];
                    if (type == "KVUC")
                    {
                        ma_khu_vuc = path[1];
                    }
                    if (type == "CUM")
                    {
                        if (path.Length > 2)
                            ma_cum = path[2];
                        else
                            ma_cum = path[1];
                    }
                    if (type == "NHOM")
                    {
                        if (path.Length > 3)
                            ma_nhom = path[3];
                        else
                            ma_nhom = path[2];
                    }
                }
            }

            // Thong tin ngay gia nhap
            
                LDatatable.AddParameter(ref dt, "@NgayGiaNhapTu", "STRING", "");
            
                LDatatable.AddParameter(ref dt, "@NgayGiaNhapDen", "STRING", "");
            

            // Lay du lieu tu cac combobox
            
                LDatatable.AddParameter(ref dt, "@NganhKT", "STRING", "");
           
                LDatatable.AddParameter(ref dt, "@LoaiHinhToChuc", "STRING", "");
            
                LDatatable.AddParameter(ref dt, "@LoaiKhachHang", "STRING", "");
            

            // Them du lieu vao tim kiem
         
                LDatatable.AddParameter(ref dt, "@TrangThaiNghiepVu", "STRING", "");
          
           
            LDatatable.AddParameter(ref dt, "@MaKhachHang", "STRING", txtMaKHang.Text);
            LDatatable.AddParameter(ref dt, "@TenKhachHang", "STRING", txtTenKHang.Text);
            LDatatable.AddParameter(ref dt, "@DkienMPA", "STRING", "");
           
                LDatatable.AddParameter(ref dt, "@DanToc", "STRING", "");
          
                LDatatable.AddParameter(ref dt, "@GioiTinh", "STRING", "");
          
                LDatatable.AddParameter(ref dt, "@NgheNghiep", "STRING", "");
            LDatatable.AddParameter(ref dt, "@TuoiTu", "STRING", Convert.ToInt32(0).ToString());
            LDatatable.AddParameter(ref dt, "@TuoiDen", "STRING", Convert.ToInt32(0).ToString());
            LDatatable.AddParameter(ref dt, "@NgayHienTai", "STRING", AppConfig.LoginedUser.NgayLamViecHienTai);
            LDatatable.AddParameter(ref dt, "@SoCMND", "STRING","");
            LDatatable.AddParameter(ref dt, "@SoDkyKDoanh", "STRING", "");
            LDatatable.AddParameter(ref dt, "@TenDangNhap", "STRING", AppConfig.LoginedUser.UserName);
            LDatatable.AddParameter(ref dt, "@DonViQLy", "STRING", AppConfig.LoginedUser.MaDonViQuanLy);
            LDatatable.AddParameter(ref dt, "@DonVi", "INT", ma_don_vi);
            LDatatable.AddParameter(ref dt, "@KhuVuc", "INT", ma_khu_vuc);
            LDatatable.AddParameter(ref dt, "@Cum", "INT", ma_cum);
            LDatatable.AddParameter(ref dt, "@Nhom", "INT", ma_nhom);
            LDatatable.AddParameter(ref dt, "@HetHieuLuc", "STRING", "%");
            LDatatable.AddParameter(ref dt, "@StartRow", "INT","1");
            LDatatable.AddParameter(ref dt, "@EndRow", "INT", "100");

            // Tim kiem
            Presentation.Process.KhachHangProcess process = new Presentation.Process.KhachHangProcess();
            DataSet ds = process.getKetQuaTimKiemNangCao(dt);
            grdDangKyKHangDS.DataSource = ds;
            grdDangKyKHangDS.DataBind();
        }
        protected void btnTimkiem_Click(object sender, EventArgs e)
        {
            TimKiem();
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            string filename = DateTime.Now.Ticks.ToString() + "xls";
            new Business.CustomControl.ExportExcel().ExportToExcel((DataTable)grdDangKyKHangDS.DataSource, Server.MapPath("Modules") + "\\" + filename);
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