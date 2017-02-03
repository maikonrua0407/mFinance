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
namespace Presentation.WebClient.Modules.TDVM.SanPham
{
    public partial class ucSanPhamDS : ControlBase
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
            tvwLoaiVay.Attributes.Add("onclick", "OnTreeClick(event)");
           
            if (!IsPostBack)
            {
                BuildTree();
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
               List<string> kq= Duyet(chkvalgrid.Value);
               if (kq != null && kq.Count > 0)
               {                  
                   string strkq = "";
                   for (int j=0;j<kq.Count;j++)
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
                
                    
                teldtNgayADungDen.Text = "";
                teldtNgayADungTu.Text = "";
                teldtNgayHetHanDen.Text = "";
                teldtNgayHetHanTu.Text = "";
                List<string> lstDieuKien = new List<string>();
                string maTruyVan = DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue();

                // Gán giá trị điều kiện
                lstDieuKien = new List<string>();
                lstDieuKien.Add(DatabaseConstant.getValue(DatabaseConstant.DanhMuc.PHUONG_THUC_TINH_LAI));
                lstPhuongThucTinhLai.Add(new AutoCompleteEntry(Dislay, "%", "%"));
                KhoiTaoGiaTriComboBox(ref lstPhuongThucTinhLai, ref cmbPhuongThucTinhLai, maTruyVan, lstDieuKien);

                lstDieuKien = new List<string>();
                lstDieuKien.Add(DatabaseConstant.getValue(DatabaseConstant.DanhMuc.MUC_DICH_VAY_VON));
                lstMucDichSuDung.Add(new AutoCompleteEntry(Dislay, "%", "%"));
                KhoiTaoGiaTriComboBox(ref lstMucDichSuDung, ref cmbMucDich, maTruyVan, lstDieuKien);

                lstDieuKien = new List<string>();
                lstDieuKien.Add(DatabaseConstant.getValue(DatabaseConstant.DanhMuc.LOAI_SAN_PHAM_TDUNG));
                lstLoaiSanPham.Add(new AutoCompleteEntry(Dislay, "%", "%"));
                KhoiTaoGiaTriComboBox(ref lstLoaiSanPham, ref cmbLoaiSanPham, maTruyVan, lstDieuKien);

                lstDieuKien = new List<string>();
                lstDieuKien.Add(DatabaseConstant.getValue(DatabaseConstant.DanhMuc.TRANG_THAI_BAN_GHI));
                lstHieuLuc.Add(new AutoCompleteEntry(Dislay, "%", "%"));
                KhoiTaoGiaTriComboBox(ref lstHieuLuc, ref cmbTinhTrang, maTruyVan, lstDieuKien);

                lstDieuKien = new List<string>();
                lstDieuKien.Add(DatabaseConstant.getValue(DatabaseConstant.DanhMuc.NGUON_VON_VAY));
                lstNguonVon.Add(new AutoCompleteEntry(Dislay, "%", "%"));
                KhoiTaoGiaTriComboBox(ref lstNguonVon, ref cmbLoaiVay, maTruyVan, lstDieuKien);

                lstDieuKien = new List<string>();
                lstDieuKien.Add(DatabaseConstant.getValue(DatabaseConstant.DanhMuc.LOAI_LAI_SUAT));
                lstLoaiLaiSuat.Add(new AutoCompleteEntry(Dislay, "%", "%"));
                KhoiTaoGiaTriComboBox(ref lstLoaiLaiSuat, ref cmbLoaiLaiSuat, maTruyVan, lstDieuKien);

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
                TinDungProcess tindungProcess = new TinDungProcess();
                string maDonVi = cmbDonVi.SelectedItem.Value;// "0001";// lstSourceDonVi.ElementAt(cmbDonVi.SelectedIndex).KeywordStrings.ElementAt(0);
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
                string MaLoaiSP = cmbLoaiSanPham.SelectedItem.Value;//lstLoaiSanPham.ElementAt(cmbLoaiSanPham.SelectedIndex).KeywordStrings.FirstOrDefault();
                string MaTinhTrangHLuc = cmbTinhTrang.SelectedItem.Value;// lstHieuLuc.ElementAt(cmbTinhTrang.SelectedIndex).KeywordStrings.FirstOrDefault();
                string MaMucDichVay = cmbMucDich.SelectedItem.Value;//lstMucDichSuDung.ElementAt(cmbMucDich.SelectedIndex).KeywordStrings.FirstOrDefault();
                string MaNguonVon = cmbLoaiVay.SelectedItem.Value;// lstNguonVon.ElementAt(cmbLoaiVay.SelectedIndex).KeywordStrings.FirstOrDefault();
                string MaPhuongThucTinhLai = cmbPhuongThucTinhLai.SelectedItem.Value;// lstPhuongThucTinhLai.ElementAt(cmbPhuongThucTinhLai.SelectedIndex).KeywordStrings.FirstOrDefault();
                string MaLoaiLSuat = cmbLoaiLaiSuat.SelectedItem.Value;//lstLoaiLaiSuat.ElementAt(cmbLoaiLaiSuat.SelectedIndex).KeywordStrings.FirstOrDefault();
                string MaSanPham = txtMaSanPham.Text;
                string TenSanPham = txtTenSanPham.Text;
                string NgayADungTu = "";
                if (teldtNgayADungTu.Text != "")
                    NgayADungTu = LDateTime.DateToString(LDateTime.StringToDate(teldtNgayADungTu.Text, "dd/MM/yyyy"), ApplicationConstant.defaultDateTimeFormat);
                string NgayADungDen = "";
                if (teldtNgayADungDen.Text != "")
                    NgayADungDen = LDateTime.DateToString(LDateTime.StringToDate(teldtNgayADungDen.Text, "dd/MM/yyyy"), ApplicationConstant.defaultDateTimeFormat);
                string NgayHetHanTu = "";
                if (teldtNgayHetHanTu.Text != "")
                    NgayHetHanTu = LDateTime.DateToString(LDateTime.StringToDate(teldtNgayHetHanTu.Text, "dd/MM/yyyy"), ApplicationConstant.defaultDateTimeFormat);
                string NgayHetHanDen = "";
                if (teldtNgayHetHanDen.Text != "")
                    NgayHetHanDen = LDateTime.DateToString(LDateTime.StringToDate(teldtNgayHetHanDen.Text, "dd/MM/yyyy"), ApplicationConstant.defaultDateTimeFormat);
                string UserName = "";
                string ListPThucVay = "";
                foreach (TreeNode item in tvwLoaiVay.CheckedNodes)
                {
                    ListPThucVay += ",''" + item.Value.ToString() + "''";
                }
                if (ListPThucVay.Length > 0)
                    ListPThucVay = "(" + ListPThucVay.Substring(1) + ")";

                grdDangKySanPhamDS.DataSource = tindungProcess.getDanhSachSanPhamTinDung(MaTTNVu, MaLoaiSP, MaTinhTrangHLuc, MaMucDichVay, MaNguonVon, MaPhuongThucTinhLai, MaLoaiLSuat, MaSanPham, TenSanPham, NgayADungTu, NgayADungDen, NgayHetHanTu, NgayHetHanDen, ListPThucVay, UserName, maDonVi).Tables["DANH_SACH"];
                grdDangKySanPhamDS.DataBind();
                // grdDangKySanPhamDS.DataSource = _mdata;
                // TotalRecords = _mdata.Rows.Count;

                // ClonePageData(_mdata, pagenumber, pagesize);
                // //    .DefaultView;
                //// //if (!LObject.IsNullOrEmpty(grdDangKySanPhamDS.SelectedItems))
                // //    grdDangKySanPhamDS.SelectedItems.Clear();
                // grdDangKySanPhamDS.DataSource = _mpageData;
                // pages1.TotalRecords = currentPage1.TotalRecords = TotalRecords;
                // currentPage1.TotalPages = pages1.CalculateTotalPages();
                // currentPage1.PageIndex = pages1.PageIndex;

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
            tvwLoaiVay.Nodes.Clear();

            List<string> lstDieuKien = new List<string>();
            lstDieuKien.Add(DatabaseConstant.DanhMuc.PHUONG_THUC_VAY.getValue());
            DropDownList dr = new DropDownList();
            new AutoComboBox().GenAutoComboBox(ref lstPhuongThucVay, ref dr, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien);
            try
            {
                TreeNode item = new TreeNode();
                item.Checked = true;
                item.Text = LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.DMUC_LOAI.PHUONG_THUC_VAY");
                item.Value = LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.DMUC_LOAI.PHUONG_THUC_VAY");
                tvwLoaiVay.Nodes.Add(item);
                for (int i = 0; i < lstPhuongThucVay.Count; i++)
                {
                    TreeNode ItemSub = new TreeNode();
                    ItemSub.Text = lstPhuongThucVay[i].DisplayName;
                    
                    ItemSub.Checked = true;

                    ItemSub.Value = lstPhuongThucVay[i].KeywordStrings[0];
                    tvwLoaiVay.Nodes[0].ChildNodes.Add(ItemSub);
                }
            }
            catch
            { }

            tvwLoaiVay.ExpandAll();
            tvwLoaiVay.ShowCheckBoxes = TreeNodeTypes.All;
        }
        #endregion
        #region giaodien
        protected void cmbLoaiVay_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        #endregion


        protected void grdDangKySanPhamDS_ItemDataBound(object sender, DataGridItemEventArgs e)
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
            string filename =DateTime.Now.Ticks.ToString()+"xls";
            new Business.CustomControl.ExportExcel().ExportToExcel((DataTable)grdDangKySanPhamDS.DataSource,Server.MapPath("Modules") + "\\" + filename);
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