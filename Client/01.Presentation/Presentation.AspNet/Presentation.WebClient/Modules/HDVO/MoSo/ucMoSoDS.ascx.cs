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
using Presentation.Process.DanhMucServiceRef;
using Presentation.Process.HuyDongVonServiceRef;
namespace Presentation.WebClient.Modules.HDVO.MoSo
{
    public partial class ucMoSoDS : ControlBase
    {
        #region Khai bao
        DataTable dtTreeSanPham = new DataTable();
        DataTable dtTreeDonVi = new DataTable();
        DataTable dtTreeCBQL = new DataTable();

        DataTable dtSoTGui = new DataTable();
        List<AutoCompleteEntry> lstSourceDonVi = new List<AutoCompleteEntry>();
        List<AutoCompleteEntry> lstSourceKyHan = new List<AutoCompleteEntry>();
        public static int countbuildtree = 0;
        // Phân trang

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            tvwLoaiVay.Attributes.Add("onclick", "OnTreeClick(event)");
            cmbKyHanTu.Attributes.Add("onchange", "onchangekyhan(this)");
            cmbKyHanDen.Attributes.Add("onchange", "onchangekyhan(this)");

            if (!IsPostBack)
            {              
                LoadDuLieu();
            }
        }

        private void LoadDuLieu()
        {
            LoadCombobox();
            LoadTreeview();
            raddtNgayMoSoTu.Text = LDateTime.StringToDate( AppConfig.LoginedUser.NgayLamViecHienTai,"yyyyMMdd").ToString("dd/MM/yyyy");
            raddtNgayMoSoDen.Text = LDateTime.StringToDate(AppConfig.LoginedUser.NgayLamViecHienTai, "yyyyMMdd").ToString("dd/MM/yyyy");
            LoadGrid(0);
        }

        /// <summary>
        /// Load dữ liệu lên Combobox
        /// </summary>
        private void LoadCombobox()
        {
            try
            {
                AutoComboBox auto = new AutoComboBox();
                List<string> lstDieuKien = new List<string>();

               

                lstDieuKien.Add(AppConfig.LoginedUser.UserName);
                lstDieuKien.Add(AppConfig.LoginedUser.MaDonVi);
                lstDieuKien.Add(DatabaseConstant.ToChucDonVi.PGD.getValue());
                auto.GenAutoComboBox(ref lstSourceDonVi, ref cmbDonVi, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DONVI_THEOPVI.getValue(), lstDieuKien, AppConfig.LoginedUser.MaDonViGiaoDich);

                //Load combobox hình thức trả lãi 
                lstDieuKien.Clear();
               
                lstDieuKien.Add(DatabaseConstant.getValue(DatabaseConstant.DanhMuc.TAN_SUAT));
                auto.GenAutoComboBox(ref lstSourceKyHan, ref cmbKyHanTu, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DMUC.getValue(), lstDieuKien);
                cmbKyHanTu.SelectedIndex = lstSourceKyHan.IndexOf(lstSourceKyHan.FirstOrDefault(i => i.KeywordStrings.First().Equals(BusinessConstant.TAN_SUAT.THANG.layGiaTri())));

                ////Load combobox hình thức trả lãi   
                //while (cmbKyHanDen.Items.Count > 0)
                //{
                //    cmbKyHanDen.Items.RemoveAt(0);
                //}
                auto.GenAutoComboBox(ref lstSourceKyHan, ref cmbKyHanDen, null);
                cmbKyHanDen.SelectedIndex = lstSourceKyHan.IndexOf(lstSourceKyHan.FirstOrDefault(i => i.KeywordStrings.First().Equals(BusinessConstant.TAN_SUAT.THANG.layGiaTri())));
            }
            catch (Exception ex)
            {
                //LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }

        /// <summary>
        /// Load dữ liệu lên Treeview
        /// </summary>
        private void LoadTreeview()
        {
            try
            {
                HuyDongVonProcess huyDongVonProcess = new HuyDongVonProcess();
                string sIdDonVi = "";
                string sMaDonVi = "";
                string sIdDonViQLy = "";
                string sMaDonViQLy = "";
                if (lstSourceDonVi != null)
                {
                    sIdDonVi = lstSourceDonVi.ElementAt(cmbDonVi.SelectedIndex).KeywordStrings.ElementAt(1);
                    sMaDonVi = lstSourceDonVi.ElementAt(cmbDonVi.SelectedIndex).KeywordStrings.ElementAt(0);
                    DM_DON_VI objDonVi = new DanhMucProcess().getDonViById(Convert.ToInt32(sIdDonVi));

                    if (objDonVi.LOAI_DVI.Equals(DatabaseConstant.ToChucDonVi.HSO.getValue()) || objDonVi.LOAI_DVI.Equals(DatabaseConstant.ToChucDonVi.CNH.getValue()))
                    {
                        sIdDonViQLy = objDonVi.ID.ToString();
                        sMaDonViQLy = objDonVi.MA_DVI;
                    }
                    else
                    {
                        sIdDonViQLy = objDonVi.ID_DVI_CHA.ToString();
                        sMaDonViQLy = objDonVi.MA_DVI_CHA;
                    }

                }

                dtTreeSanPham = huyDongVonProcess.GetTreeSanPham(sMaDonViQLy).Tables[0];
                dtTreeDonVi = huyDongVonProcess.GetTreeDonViNhom(AppConfig.LoginedUser.MaDonViQuanLy, AppConfig.LoginedUser.UserName, sMaDonViQLy).Tables[0];
                dtTreeCBQL = huyDongVonProcess.GetTreeCBQL(sMaDonViQLy).Tables[0];

               // BuildTree(dtTreeSanPham, tvwLoaiVay, false);
                DataTable mtree = new DataTable();

                mtree.Columns.Add("LEVEL", Type.GetType("System.String"));
                mtree.Columns.Add("NODE_PARENT", Type.GetType("System.String"));
                mtree.Columns.Add("NODE", Type.GetType("System.String"));
                mtree.Columns.Add("NODE_NAME", Type.GetType("System.String"));
                mtree.Columns.Add("STYPE", Type.GetType("System.String"));
                DataRow row = mtree.NewRow();               
                for (int j = 0; j < dtTreeSanPham.Rows.Count; j++)
                {
                    row = mtree.NewRow();
                    row["LEVEL"] = dtTreeSanPham.Rows[j][0].ToString();
                    row["NODE_PARENT"] = dtTreeSanPham.Rows[j][1].ToString();
                    row["NODE"] = dtTreeSanPham.Rows[j][2].ToString();
                    row["NODE_NAME"] = dtTreeSanPham.Rows[j][3].ToString();
                    row["STYPE"] = "SAN_PHAM";
                    mtree.Rows.Add(row);
                }
               
               
                tvwLoaiVay.Nodes.Clear();
                TreeNode rootItem = new TreeNode();
                rootItem.Text = LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.HuyDongVon.MoSo.ucMoSoDS.NhomSanPham");
                rootItem.Value = "0#SAN_PHAM";
                tvwLoaiVay.Nodes.Add(rootItem);
                BuildTree(rootItem,mtree,"SAN_PHAM");
                mtree.Clear();
                for (int j = 0; j < dtTreeDonVi.Rows.Count; j++)
                {
                    
                   
                        row = mtree.NewRow();
                        row["LEVEL"] = dtTreeDonVi.Rows[j][0].ToString();
                        row["NODE_PARENT"] = dtTreeDonVi.Rows[j][1].ToString();
                        row["NODE"] = dtTreeDonVi.Rows[j][2].ToString();
                        row["NODE_NAME"] = dtTreeDonVi.Rows[j][3].ToString();
                        row["STYPE"] = "DON_VI";
                        mtree.Rows.Add(row);
                    
                }
                TreeNode rootItem_dv = new TreeNode();
                rootItem_dv.Text = LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.HuyDongVon.MoSo.ucMoSoDS.KhachHang");
                rootItem_dv.Value = "0#DON_VI";
                tvwLoaiVay.Nodes.Add(rootItem_dv);
                BuildTree(rootItem, mtree, "DON_VI");
                                               
               
                tvwLoaiVay.ShowCheckBoxes = TreeNodeTypes.All;


            }
            catch (Exception ex)
            {
                //LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }
        }
        private void BuildTree(TreeNode item,DataTable dt, string stype)
        {

            try
            {

                List<DataRow> lstDataRow = null;
                lstDataRow = dt.Select("LEVEL<>'0' AND STYPE='" + stype + "'").OrderBy(row => row[2]).ToList();

                foreach (DataRow row in lstDataRow)
                {
                    if (row["NODE_PARENT"].ToString() == item.Value.ToString().Split('#')[0])
                    {
                        TreeNode subItem = new TreeNode();
                        subItem.Text = row["NODE_NAME"].ToString();
                        subItem.Value = row["NODE"].ToString() + "#" + stype;
                        item.ChildNodes.Add(subItem);
                        BuildTree(subItem,dt,stype);
                    }
                }
            }
            catch
            { }

            //tvwLoaiVay.ExpandAll();
            tvwLoaiVay.ShowCheckBoxes = TreeNodeTypes.All;
        }
        /// <summary>
        /// Load dữ liệu lên Grid
        /// </summary>
        private void LoadGrid(int stype)
        {

            try
            {
                HuyDongVonProcess huyDongVonProcess = new HuyDongVonProcess();

                AutoComboBox auto = new AutoComboBox();
                List<string> lstDieuKien = new List<string>();
                lstDieuKien.Add(AppConfig.LoginedUser.UserName);
                lstDieuKien.Add(AppConfig.LoginedUser.MaDonVi);
                lstDieuKien.Add(DatabaseConstant.ToChucDonVi.PGD.getValue());
                
                auto.GenAutoSoure(ref lstSourceDonVi, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DONVI_THEOPVI.getValue(), lstDieuKien, AppConfig.LoginedUser.MaDonViGiaoDich);
                lstDieuKien.Clear();
                

                #region Điều kiện tìm kiếm
                string sMaDonVi = "";
                sMaDonVi = "(";
                string maChon = cmbDonVi.SelectedItem.Value;// lstSourceDonVi.ElementAt(cmbDonVi.SelectedIndex).KeywordStrings.ElementAt(0);
                string donVi = "";
                foreach (AutoCompleteEntry item in lstSourceDonVi)
                {
                    donVi = item.KeywordStrings.ElementAt(0);
                    if (donVi.Contains(maChon))
                    {
                        sMaDonVi += "''" + donVi + "'',";
                    }
                }
                sMaDonVi = sMaDonVi.Substring(0, sMaDonVi.Length - 1);
                sMaDonVi += ")";

                string sTrangThaiNVu = "";
                string sSoTGui = "%";
                string sTenSoTGui = "%";
                string sTuNgayMoSo = "%";
                string sDenNgayMoSo = "%";
                string sTuNgayDHan = "%";
                string sDenNgayDHan = "%";
                string soDuTu = "%";
                string soDuDen = "%";
                string iKyHanTu = "%";
                string iKyHanDen = "%";
                string sKyHanDonVi = "%";
                string sMaKH = "%";
                string sTenKH = "%";
                string sSoCMND = "%";
                string sSDT = "%";
                string sEmail = "%";
                string sNgayDC = "%";
                string sSanPham = "%", sIDNhom = "%", sCBQL = "%";

                if (cbChoduyet.Checked)
                    sTrangThaiNVu = sTrangThaiNVu + "''" + cbChoduyet.Value + "'',";
                if (cbDaduyet.Checked)
                    sTrangThaiNVu = sTrangThaiNVu + "''" + cbDaduyet.Value + "'',";
                if (cbThoaiduyet.Checked)
                    sTrangThaiNVu = sTrangThaiNVu + "''" + cbThoaiduyet.Value + "'',";
                if (cbTuchoi.Checked)
                    sTrangThaiNVu = sTrangThaiNVu + "''" + cbTuchoi.Value + "'',";
                if (sTrangThaiNVu != "")
                {
                    sTrangThaiNVu = "(" + sTrangThaiNVu.Substring(0, sTrangThaiNVu.Length - 1) + ")";
                }
               
                if (!txtSoSoTG.Text.IsNullOrEmpty())
                    sSoTGui = txtSoSoTG.Text;

                if (!raddtNgayMoSoTu.Text.IsNullOrEmpty())
                    sTuNgayMoSo = LDateTime.StringToDate(raddtNgayMoSoTu.Text,"dd/MM/yyyy").ToString("yyyyMMdd");

                if (!raddtNgayMoSoDen.Text.IsNullOrEmpty())
                    sDenNgayMoSo = LDateTime.StringToDate(raddtNgayMoSoDen.Text, "dd/MM/yyyy").ToString("yyyyMMdd");

                if (!raddtNgayDaoHanTu.Text.IsNullOrEmpty())
                    sTuNgayDHan = LDateTime.StringToDate(raddtNgayDaoHanTu.Text, "dd/MM/yyyy").ToString("yyyyMMdd");

                if (!raddtNgayDaoHanDen.Text.IsNullOrEmpty())
                    sDenNgayDHan = LDateTime.StringToDate(raddtNgayDaoHanDen.Text, "dd/MM/yyyy").ToString("yyyyMMdd");

                if (numSoDuTu.Text!="")
                    soDuTu = numSoDuTu.Text.Replace(",","").ToString();

                if (numSoDuDen.Text != "")
                    soDuDen = numSoDuDen.Text.Replace(",", "").ToString();

                if (numKyHanTu.Text != "")
                    iKyHanTu = numKyHanTu.Text.Replace(",", "").ToString();

                if (numKyHanDen.Text != "")
                    iKyHanDen = numKyHanDen.Text.Replace(",", "").ToString();

                if (cmbKyHanTu.SelectedIndex >= 0)
                    sKyHanDonVi = cmbKyHanTu.SelectedItem.Value;

                if (!txtKhachHang.Text.IsNullOrEmptyOrSpace())
                    sMaKH = txtKhachHang.Text;

                if (!txtTenKhachHang.Text.IsNullOrEmptyOrSpace())
                    sTenKH = txtTenKhachHang.Text;
                //}

                #endregion
                // Phân trang
                int StartRow = 1;
                int EndRow = 999999999 * stype;// ClientInformation.SoLuongBanGhi;
                int CurrentPagging = 1;
                int PaggingSize = 999999999;// ClientInformation.SoLuongBanGhi;

                DataSet ds = huyDongVonProcess.GetDanhSachSoTGuiNhom(sMaDonVi, sSanPham, sIDNhom, sCBQL, sTrangThaiNVu, sSoTGui,
                                         sTenSoTGui, sTuNgayMoSo, sDenNgayMoSo, sTuNgayDHan, sDenNgayDHan,
                                          soDuTu, soDuDen, iKyHanTu, iKyHanDen, sKyHanDonVi,
                                         sMaKH, sTenKH, sSoCMND, sSDT, sEmail, sNgayDC, StartRow.ToString(), EndRow.ToString());
                if (ds != null && ds.Tables.Count > 0)
                {
                    //foreach (DataRow dr in ds.Tables[0].Rows)
                    //{
                    //    dr["TTHAI_NVU"] = BusinessConstant.layNgonNguNghiepVu(dr["TTHAI_NVU"].ToString());
                    //    if (!dr["KY_HAN_DVI_TINH"].IsNullOrEmpty() && !dr["KY_HAN_TGIAN"].IsNullOrEmpty() && !dr["KY_HAN"].IsNullOrEmpty())
                    //    {
                    //        if (!dr["KY_HAN_TGIAN"].ToString().Equals("0") && !dr["KY_HAN_DVI_TINH"].ToString().Equals(""))
                    //            dr["KY_HAN"] = dr["KY_HAN_TGIAN"] + " " + BusinessConstant.layNgonNguTuGiaTri(dr["KY_HAN_DVI_TINH"].ToString(), "TAN_SUAT");
                    //    }
                    //}

                    DataTable serverDataTable = ds.Tables[0];
                    int totalRecord = Int32.Parse(ds.Tables[1].Rows[0][0].ToString());
                    //DataTable clientDataTable = ds.Tables[0].Copy() ;// CommonFunction.BuildClientDataTable(serverDataTable, CurrentPagging, PaggingSize, totalRecord);

                    //dtSoTGui = ds.Tables[0];
                    //grSoTienGuiDS.DataContext = dtSoTGui.DefaultView;
                    dtSoTGui = serverDataTable;
                    grSoTienGuiDS.DataSource = serverDataTable;
                    grSoTienGuiDS.DataBind();

                    int soSoTgui = 0;
                    decimal tongSoDu = 0;
                    decimal soDuBinhQuan = 0;
                    decimal tongSoDuGoc = Decimal.Parse(ds.Tables[2].Rows[0][0].ToString());
                    decimal tongSoDuLai = Decimal.Parse(ds.Tables[2].Rows[0][1].ToString());

                    if (totalRecord > 0)
                    {
                        soSoTgui = totalRecord;
                        for (int i = 0; i < dtSoTGui.Rows.Count; i++)
                        {
                            tongSoDu += Convert.ToDecimal(dtSoTGui.Rows[i]["SO_DU"]);
                        }
                        soDuBinhQuan = tongSoDu / soSoTgui;
                    }

                    lblSumSoSo.Text = String.Format("{0:#,#}", soSoTgui);
                    lblSumSoDu.Text = String.Format("{0:#,#}", tongSoDu);
                    lblSoDuBQ.Text = String.Format("{0:#,#}", soDuBinhQuan);
                    lblSumSoDuGoc.Text = String.Format("{0:#,#}", tongSoDuGoc);
                    lblSumSoDuLai.Text = String.Format("{0:#,#}", tongSoDuLai);
                }
                else
                {
                    dtSoTGui.Rows.Clear();
                    grSoTienGuiDS.DataSource = dtSoTGui.DefaultView;
                    grSoTienGuiDS.DataBind();
                }
            }
            catch (Exception ex)
            {
                // LLogging.WriteLog(System.Reflection.MethodInfo.GetCurrentMethod().ToString(), LLogging.LogType.ERR, ex);
            }           
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {

        }

        protected void btnTimkiem_Click(object sender, EventArgs e)
        {
            LoadGrid(1);
        }
        protected void grSoTienGuiDS_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            e.Item.Attributes.Add("onmouseover", "currColor=this.style.backgroundColor;this.style.backgroundColor='#FDDBA5'");
            e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currColor");
        }
    }
}