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

namespace Presentation.WebClient.Modules.QTHT.NguoiDung
{
    public partial class ucNguoiDungDS : ControlBase
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
        List<HT_NSD> dsNSD = new List<HT_NSD>();
        List<HT_NHNSD> dsNHNSD = new List<HT_NHNSD>();
        QuanTriHeThongProcess qtht = new QuanTriHeThongProcess();

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

                    //grdNhomNSD.DataSource = ds;
                    //grdNhomNSD.DataBind();
                    BuildGridDoiTuong();

                    DataRow drRoot = dt.Rows[0];
                    tvwTree.Nodes.Clear();
                    TreeNode rootItem = new TreeNode();
                    TreeNode subItem = new TreeNode();

                    rootItem.Text = "User and user group";
                    rootItem.Value = "";
                    tvwTree.Nodes.Add(rootItem);
                   
                    List<string> lstDieuKien = new List<string>();
                    string maTruyVan = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();
                    lstDieuKien = new List<string>();
                    lstDieuKien.Add(DatabaseConstant.getValue(DatabaseConstant.DanhMuc.LOAI_DTUONG_KTHAC_TNGUYEN));
                   
                    DropDownList cmb = new DropDownList();
                    KhoiTaoGiaTriComboBox(ref lstNhomNo, ref cmb, maTruyVan, lstDieuKien);
                    foreach(AutoCompleteEntry item in lstNhomNo)
                    {
                        TreeNode chidNode = new TreeNode();
                        chidNode.Text = item.DisplayName;
                        chidNode.Value = item.KeywordStrings.First();
                        rootItem.ChildNodes.Add(chidNode);
                    }
                }


            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }
      
        #region giaodien

        #endregion

        private void txtTimKiemNhanh_TextChanged(object sender, EventArgs e)
        {

            txtTimKiemNhanh.Text = "aaaaa";
            //txtTimKiemNhanh.Focus();

        }
        protected void grdNhomNSD_ItemDataBound(object sender, DataGridItemEventArgs e)
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

        protected void tvwTree_SelectedNodeChanged(object sender, EventArgs e)
        {
            BuildGridDoiTuong();
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
            else if (trangthai=="CAP_QTDV")
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
            else if (trangthai=="SDU")
            {
                return "Sử dụng";
            }
            return "";
        }
        private void BuildGridDoiTuong()
        {
            try
            {
                dsNSD = new List<HT_NSD>();
                dsNHNSD = new List<HT_NHNSD>();
                List<DM_DON_VI> listDonVi = new List<DM_DON_VI>();
                string loaiDoiTuong = "";
               
                loaiDoiTuong = tvwTree.SelectedValue.ToString();

                // lấy dữ liệu sử dụng chung : HT_NSD, HT_NHNSD
                if (loaiDoiTuong.Equals("NSD") || loaiDoiTuong == "")
                {
                    dsNSD = qtht.layNSD("NSD");
                    
                }
                else if (loaiDoiTuong.Equals("NHNSD"))
                {
                    dsNHNSD = qtht.layNhomNSD("NHNSD");
                   
                }

                
                    DM_DON_VI dv = new DM_DON_VI();
                    dv.MA_DVI = AppConfig.LoginedUser.MaDonVi;
                    dv.TEN_GDICH = AppConfig.LoginedUser.TenDonVi;

                    listDonVi.Add(dv);             

                // Tạo source thông tin đối tượng
                dt = new DataTable();
                dt.Columns.Add("ID", typeof(int));
                dt.Columns.Add("STT", typeof(int));
                dt.Columns.Add("MA", typeof(string));
                dt.Columns.Add("TEN", typeof(string));
                dt.Columns.Add("DON_VI", typeof(string));
                dt.Columns.Add("PHAN_LOAI", typeof(string));            
                dt.Columns.Add("TTHAI", typeof(string));
                int stt = 0;
                //Lấy dữ liệu đổ vào source với loại đối tượng tương ứng
                if (loaiDoiTuong.Equals("NSD") || loaiDoiTuong == "")
                {
                    foreach (var item in dsNSD)
                    {
                        DataRow r = dt.NewRow();
                        stt = stt + 1;
                        r[0] = item.ID;
                        r[1] = stt;
                        r[2] = item.MA_NSD;
                        r[3] = item.TEN_DAY_DU;
                        r[4] = layTenDonViTheoDanhSach(item.MA_DVI_QLY, listDonVi);
                        r[5] = layNgonNguLoaiNguoiDung(item.PHAN_LOAI_NSD);
                        r[6] = layNgonNguTrangThai(item.TINH_TRANG);
                        dt.Rows.Add(r);
                    }
                }
                else if (loaiDoiTuong.Equals("NHNSD"))
                {
                    foreach (var item in dsNHNSD)
                    {
                        DataRow r = dt.NewRow();
                        stt = stt + 1;
                        r[0] = item.ID;
                        r[1] = stt;
                        r[2] = item.MA_NHNSD;
                        r[3] = item.TEN_NHNSD;
                        r[4] = layTenDonViTheoDanhSach(item.MA_DVI_QLY, listDonVi);
                        r[5] = item.MO_TA;
                        r[6] = layNgonNguTrangThai(item.TTHAI_BGHI);
                        dt.Rows.Add(r);
                    }
                }
                //// đổ source lên lưới
                grdNhomNSD.DataSource = dt;
                grdNhomNSD.DataBind();
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

   
    }
}