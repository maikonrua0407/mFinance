using Presentation.WebClient.Business;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.Services;
using Presentation.Process;
using Presentation.Process.Common;
using Utilities.Common;
using Presentation.Process.HuyDongVonServiceRef;
namespace Presentation.WebClient.Modules.HDVO.Popup
{
    public partial class ucPopupSoTGui : Page
    {
        #region Khai báo

        private string sSanPham = "";
        private string sIDCum = "";

        DataTable dtTreeSanPham = new DataTable();
        DataTable dtTreeDonVi = new DataTable();

        DataTable dtSoTGui = new DataTable();



        private DatabaseConstant.Function function;
        public DatabaseConstant.Function Function
        {
            get { return function; }
            set { function = value; }
        }

        public  bool isMultiSelect = true;

   
        #endregion
        #region [Properties]

        private static string mv_strRootPath = AppDomain.CurrentDomain.BaseDirectory.Replace("bin\\Debug\\", "").Replace("bin\\Release\\", "").Replace("bin\\", "");
        public  string fn="";
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            tvSearch.Attributes.Add("onclick", "OnTreeClick(event)");
            btnTimkiem.Text = LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.DungChung.Button.TimKiem");
            LoadParams();
            if (!IsPostBack)
            {               
                
                if (HttpContext.Current.Request.QueryString["f"].ToString() != "" && HttpContext.Current.Request.QueryString["f"].ToString()!="")
                {
                    fn=HttpContext.Current.Request.QueryString["f"].ToString();
                    isMultiSelect=bool.Parse(HttpContext.Current.Request.QueryString["m"].ToString());
                    LoadTreeview();
                    LoadGrid(0);
                }
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
                string sIdDonVi =  AppConfig.LoginedUser.IdDonVi.ToString();
                if (sIdDonVi == "0") sIdDonVi = "4";// FIX TAM DE CO DU LIEU
                string sMaDonVi = AppConfig.LoginedUser.MaDonVi;
                DataSet dsTreeSanPham;

                dtTreeDonVi = huyDongVonProcess.GetTreeDonVi(sIdDonVi).Tables[0];

                #region Tree Sản phẩm
                if (fn == "HDV_RUT_BOT_GOC" || fn == "HDV_RUT_GOC_THEO_DANH_SACH")
                {
                    dsTreeSanPham = huyDongVonProcess.GetTreeSanPhamRutGoc(sMaDonVi);
                }
                else if (fn == "HDV_GUI_THEM_TIEN_THEO_SO" || fn == "HDV_GUI_THEM_TIEN_THEO_DANH_SACH")
                {
                    dsTreeSanPham = huyDongVonProcess.GetTreeSanPhamGuiThem(sMaDonVi);
                }
                else if (fn == "HDV_TRA_LAI" || fn == "HDV_TRA_LAI_THEO_DANH_SACH")
                {
                    dsTreeSanPham = huyDongVonProcess.GetTreeSanPhamTraLai(sMaDonVi);
                }
                else if (fn == "HDV_DU_CHI")
                {
                    dsTreeSanPham = huyDongVonProcess.GetTreeSanPhamDuChi(sMaDonVi);
                }
                else if (fn == "HDV_PHAN_BO")
                {
                    dsTreeSanPham = huyDongVonProcess.GetTreeSanPhamPhanBo(sMaDonVi);
                }
                else if (fn == "HDV_DIEU_CHINH_LS")
                {
                    dsTreeSanPham = huyDongVonProcess.GetTreeSanPhamThayDoiLS(sMaDonVi);
                }
                else
                {
                    dsTreeSanPham = huyDongVonProcess.GetTreeSanPham(sMaDonVi);
                }


                if (dsTreeSanPham != null && dsTreeSanPham.Tables[0].Rows.Count > 0)
                {
                    dtTreeSanPham = dsTreeSanPham.Tables[0];
                }
                #endregion

                 tvSearch.Nodes.Clear();
                
                //Cấu trúc của Tag: GiaTri#Level#LoaiTree ( VD: MaSP001#2#SAN_PHAM)
                TreeNode itemSanPham = new TreeNode();
                itemSanPham.Value = "0#0#SAN_PHAM";
                itemSanPham.Text = LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.HuyDongVon.Popup.ucPopupSoTGui.SanPham");
                itemSanPham.Expanded=true;
                tvSearch.Nodes.Add(itemSanPham);
                BuildTree(itemSanPham, dtTreeSanPham, "SAN_PHAM");
                
                TreeNode itemDonVi = new TreeNode();
                //Cấu trúc của Tag: GiaTri#Level#LoaiTree ( VD: MaSP001#2#SAN_PHAM)
                itemDonVi.Value = "0#0#DON_VI";
                itemDonVi.Text = LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.HuyDongVon.MoSo.ucMoSoDS.KhachHang");
                itemDonVi.Expanded = true;
                //BuildTree(itemDonVi, dtTreeDonVi, "DON_VI");
                tvSearch.Nodes.Add(itemDonVi);
                BuildSubTreeKhuVuc(itemDonVi, null, 0);


                tvSearch.ShowCheckBoxes = TreeNodeTypes.All;
                
            }
            catch (Exception ex)
            {
                
            }
        }
        protected void BuildTree(TreeNode item, DataTable dt, string sLoaiTree)
        {
            try
            {
                //Cấu trúc của Tag: GiaTri#Level#LoaiTree  ( VD:  MaSP001#2#SAN_PHAM hoặc CUM001#3#DON_VI)
                string sTag = item.Value.ToString();
                int i1 = sTag.IndexOf("#");
                int i2 = sTag.LastIndexOf("#");

                string sValue = sTag.Substring(0, i1);
                int iLevel = Convert.ToInt32(sTag.Substring(i1 + 1, i2 - i1 - 1));

                List<DataRow> lstDataRow = null;
                if (sLoaiTree.Equals("DON_VI"))
                {
                    lstDataRow = dt.Select().OrderBy(row => row[3]).ToList();
                    //lstDataRow = dt.Select("LEVEL<4").OrderBy(row => row[3]).ToList();
                }
                else
                {
                    lstDataRow = dt.Select().ToList();
                }

                foreach (DataRow row in lstDataRow)
                //foreach (DataRow row in dt.Rows)
                {
                    if (iLevel == Convert.ToInt32(row["LEVEL"]) - 1)
                    {
                        if (row["NODE_PARENT"].ToString() == sValue)
                        {
                            TreeNode subItem = new TreeNode();
                            subItem.Text = row["NODE_NAME"].ToString();
                            subItem.Value = row["NODE"].ToString() + "#" + row["LEVEL"].ToString() + "#" + sLoaiTree;
                            subItem.Expanded = false;
                            item.ChildNodes.Add(subItem);
                            BuildTree(subItem, dt, sLoaiTree);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            // tvwLoaiVay.Nodes.Add(item);
        }

        void BuildSubTreeKhuVuc(TreeNode Item, DataRow dr, int iLevel)
        {
            List<DataRow> lstDataRow = null;
            if (dr != null)
                lstDataRow = dtTreeDonVi.Select("NODE_PARENT='" + dr["NODE"] + "' AND LEVEL=" + iLevel).OrderBy(row => row[3]).ToList();
            //lstDataRow = dtTreeDLy.Select("MA_DVI_CHA='" + dr["MA_DVI"] + "' AND LEVEL=" + iLevel).ToList();
            else
                lstDataRow = dtTreeDonVi.Select("NODE_PARENT=0").OrderBy(row => row[3]).ToList();
            //lstDataRow = dtTreeDLy.Select("MA_DVI_CHA=''").ToList();
            foreach (DataRow row in lstDataRow)
            {
                TreeNode subItem = new TreeNode();
                subItem.Text = row["NODE_NAME"].ToString();
                //subItem.Tag = row["NODE"].ToString();
                subItem.Value = row["NODE"].ToString() + "#" + row["LEVEL"].ToString() + "#" + "DON_VI";
                //subItem.IsExpanded = true;
                //subItem.IsChecked = true;
                subItem.Expanded = false;
                Item.ChildNodes.Add(subItem);
                BuildSubTreeKhuVuc(subItem, row, Convert.ToInt32(row["LEVEL"]) + 1);
            }
        }

        private void LoadGrid(int itype)
        {
            try
            {
                sSanPham = "";
                sIDCum = "";
                if (itype == 1)
                {
                    //string ListPThucVay = "";
                    //foreach (TreeNode item in tvwLoaiVay.CheckedNodes)
                    //{
                    //    ListPThucVay += ",''" + item.Value.ToString() + "''";
                    //}
                    //if (ListPThucVay.Length > 0)
                    //    ListPThucVay = "(" + ListPThucVay.Substring(1) + ")";
                    foreach (TreeNode item in tvSearch.CheckedNodes)
                    {

                        ///Cấu trúc của Tag: GiaTri#Level#LoaiTree  ( VD:  MaSP001#2#SAN_PHAM hoặc CUM001#3#DON_VI)
                        string sTag = item.Value.ToString();
                        int i1 = sTag.IndexOf("#");
                        int i2 = sTag.LastIndexOf("#");

                        string sValue = sTag.Split('#')[0];// sTag.Substring(0, i1);
                        int iLevel = Convert.ToInt32(sTag.Split('#')[1]);
                        string sLoaiTree = sTag.Split('#')[2];// sTag.Substring(i2 + 1);

                        if (sLoaiTree.Equals("SAN_PHAM"))
                        {
                            if (iLevel == 2)
                            {
                                sSanPham = sSanPham + "''" + sValue + "'',";
                            }
                        }

                        if (sLoaiTree.Equals("DON_VI"))
                        {
                            if (iLevel == 3)
                            {
                                sIDCum = sIDCum + "''" + sValue + "'',";
                            }
                        }

                    }

                    if (sSanPham.Length > 0)
                        sSanPham = sSanPham.Substring(0, sSanPham.Length - 1);

                    if (sIDCum.Length > 0)
                        sIDCum = sIDCum.Substring(0, sIDCum.Length - 1);


                    //if (itemDonVi.CheckState == System.Windows.Automation.ToggleState.On)
                    //{
                    //    sIDCum = "%";
                    //}

                    if (sSanPham.Equals(""))
                    {
                        sSanPham = "''''";
                    }

                    if (sIDCum.Equals(""))
                    {
                        sIDCum = "''''";
                    }
                }
                else
                {
                    sSanPham = "''%''";
                    sIDCum = "''%''";
                }

                HuyDongVonProcess huyDongVonProcess = new HuyDongVonProcess();
                DataSet ds = null;
                if (fn == "HDV_LAI_NHAP_GOC_THEO_SO" || fn == "HDV_LAI_NHAP_GOC_THEO_DANH_SACH")
                {
                    ds = huyDongVonProcess.GetDanhSachSoLNG(AppConfig.LoginedUser.MaDonViGiaoDich, sSanPham, sIDCum, AppConfig.LoginedUser.NgayLamViecHienTai);
                }
                else if (fn == "HDV_RUT_BOT_GOC" || fn == "HDV_RUT_GOC_THEO_DANH_SACH")
                {
                    ds = huyDongVonProcess.GetDanhSachSoRutGoc(AppConfig.LoginedUser.MaDonViGiaoDich, sSanPham, sIDCum, AppConfig.LoginedUser.NgayLamViecHienTai);
                }
                else if (fn == "HDV_DU_CHI")
                {
                    ds = huyDongVonProcess.GetDanhSachSoDuChi(AppConfig.LoginedUser.MaDonViGiaoDich, sSanPham, sIDCum, AppConfig.LoginedUser.NgayLamViecHienTai);
                }
                else if (fn == "HDV_TRA_LAI" || fn == "HDV_TRA_LAI_THEO_DANH_SACH")
                {
                    ds = huyDongVonProcess.GetDanhSachSoTraLai(AppConfig.LoginedUser.MaDonViGiaoDich, sSanPham, sIDCum, AppConfig.LoginedUser.NgayLamViecHienTai);
                }
                else if (fn == "HDV_TAT_TOAN" || fn == "HDV_TAT_TOAN_THEO_DANH_SACH")
                {
                    ds = huyDongVonProcess.GetDanhSachSoTatToan(AppConfig.LoginedUser.MaDonViGiaoDich, sSanPham, sIDCum, AppConfig.LoginedUser.NgayLamViecHienTai);
                }
                else if (fn == "HDV_DIEU_CHINH_LS")
                {
                    ds = huyDongVonProcess.GetDanhSachSoThayDoiLS(AppConfig.LoginedUser.MaDonViGiaoDich, sSanPham, sIDCum, AppConfig.LoginedUser.NgayLamViecHienTai);
                }
                else
                {
                    ds = huyDongVonProcess.GetDanhSachSoTGui(AppConfig.LoginedUser.MaDonViGiaoDich, sSanPham, sIDCum);
                }
                
                if (ds != null)
                {
                    dtSoTGui = ds.Tables[0];
                    if (itype == 0)

                    { dtSoTGui = ds.Tables[0].Clone(); }
                    grSoTienGuiDS.DataSource = dtSoTGui;
                    grSoTienGuiDS.DataBind();
                    int soSoTgui = 0;
                    decimal tongSoDu = 0;
                    decimal soDuBinhQuan = 0;
                    if (dtSoTGui.Rows.Count > 0)
                    {
                        soSoTgui = dtSoTGui.Rows.Count;
                        for (int i = 0; i < dtSoTGui.Rows.Count; i++)
                        {
                            tongSoDu += Convert.ToDecimal(dtSoTGui.Rows[i]["SO_DU"]);
                        }
                        soDuBinhQuan = tongSoDu / soSoTgui;
                    }

                    lblSumSoSo.Text = String.Format("{0:#,#}", soSoTgui);
                    lblSumSoDu.Text = String.Format("{0:#,#}", tongSoDu);
                    lblSoDuBQ.Text = String.Format("{0:#,#}", soDuBinhQuan);
                }
                else
                {
                    dtSoTGui.Rows.Clear();
                    grSoTienGuiDS.DataSource = dtSoTGui;
                    grSoTienGuiDS.DataBind();
                }
            }
            catch (Exception ex)
            {
               
            }
        }
        protected void LoadParams()
        {
            if (null != CacheService.Instance().CurrentPortal)
            {
                this.Title = CacheService.Instance().CurrentPortal.Title;
                foreach (Control ctl in this.Header.Controls)
                {
                    if (ctl is HtmlMeta)
                    {
                        if (((HtmlMeta)ctl).Name.ToLower() == "keywords")
                        {
                            ((HtmlMeta)ctl).Content = CacheService.Instance().CurrentPortal.Keywords;
                        }
                        else if (((HtmlMeta)ctl).Name.ToLower() == "description")
                        {
                            ((HtmlMeta)ctl).Content = CacheService.Instance().CurrentPortal.Descriptions;
                        }
                    }
                    else if (ctl is HtmlLink)
                    {
                        if (((HtmlLink)ctl).Attributes["rel"] == "shortcut icon")
                        {
                            ((HtmlLink)ctl).Href = CacheService.Instance().CurrentPortal.ICon.Replace("~", "");
                        }
                    }
                }

                //Them css,javascript theo theme
                string v_strAttach = "";
                string p = Path.GetFullPath(CacheService.Instance().CurrentPortal.TemplateBase.Replace("~", mv_strRootPath)) + "Attach\\";
                string[] v_arrFile = Directory.GetFiles(p);
                if ((null != v_arrFile) && (v_arrFile.GetLength(0) > 0))
                {
                    for (int i = 0; i < v_arrFile.GetLength(0); i++)
                    {
                        switch (Path.GetExtension(v_arrFile[i]).ToUpper().Trim())
                        {
                            case ".CSS":
                                {
                                    v_strAttach += "<link href=\"" + CacheService.Instance().CurrentPortal.TemplateBase.Replace("~/", "/") + "/Attach/" + Path.GetFileName(v_arrFile[i]) + "\" type=\"text/css\" rel=\"Stylesheet\" />\n\r";
                                    break;
                                }
                            case ".JS":
                                {
                                    v_strAttach += "<script language=\"javascript\" type=\"text/javascript\" src=\"" + CacheService.Instance().CurrentPortal.TemplateBase.Replace("~/", "/") + "/Attach/" + Path.GetFileName(v_arrFile[i]) + "\" ></script>\n\r";
                                    break;
                                }
                        }
                    }
                }

                Literal v_el = new Literal();
                v_el.Text = v_strAttach;

                this.Header.Controls.Add(v_el);
            }
        }

        protected void grSoTienGuiDS_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            e.Item.Attributes.Add("onmouseover", "currColor=this.style.backgroundColor;this.style.backgroundColor='#FDDBA5'");
            e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currColor");
            try
            {
                //string tableRowId = grSoTienGuiDS.ClientID + "_" +
                //                    ((VONG_VAY_CTIET)e.Item.DataItem).ID;
                //e.Item.Attributes.Add("id", tableRowId);
            }
            catch { }
        }

        protected void btnTimkiem_Click(object sender, EventArgs e)
        {
            LoadGrid(1);
        }
    }
}