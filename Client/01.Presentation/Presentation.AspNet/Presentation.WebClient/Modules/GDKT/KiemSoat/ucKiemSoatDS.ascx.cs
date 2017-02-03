using Presentation.Process;
using Presentation.Process.Common;
using Presentation.Process.KeToanServiceRef;
using Presentation.WebClient.Business;
using Presentation.WebClient.Business.CustomControl;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utilities.Common;
using static Utilities.Common.BusinessConstant;

namespace Presentation.WebClient.Modules.GDKT.KiemSoat
{
    public partial class ucKiemSoatDS : ControlBase
    {
        #region [Variables]

        protected List<AutoCompleteEntry> lstSourceDonVi = new List<AutoCompleteEntry>();

        private DataTable ListGiaoDich
        {
            get
            {
                DataTable v_dtRet = null;

                if (null != ViewState["ListGiaoDich"])
                {
                    v_dtRet = (DataTable)ViewState["ListGiaoDich"];
                }

                return v_dtRet;
            }

            set
            {
                ViewState["ListGiaoDich"] = value;
            }
        }


        #endregion

        #region [User Function]

        private void InitForm()
        {
            BuildTree();
            KhoiTaoControl();
            LoadGDGrid();
        }

        private void KhoiTaoControl()
        {
            string Dislay = LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.DungChung.TatCa");
            try
            {
                BuilCombobox();
                teldtNgayGDTu.Text = DateTime.ParseExact(AppConfig.LoginedUser.NgayLamViecHienTai, "yyyyMMdd", null).ToString("dd/MM/yyyy");
                teldtNgayGDDen.Text = DateTime.ParseExact(AppConfig.LoginedUser.NgayLamViecHienTai, "yyyyMMdd", null).ToString("dd/MM/yyyy");
                tetxSoGiaoDich.Text = "";
                tetxSoPhieu.Text = "";
            }
            catch (Exception ex)
            {
                LLogging.WriteLog(ex.TargetSite.Name, LLogging.LogType.ERR, ex);
            }
        }

        private void LoadGDGrid()
        {
            string v_strMaDonVi = "";
            string v_strMaPhanHe = "", v_strLoaiPhanHe = "", v_strTrangThai = "";
            string v_strTuNgay = teldtNgayGDTu.Text.Trim();
            string v_strDenNgay = teldtNgayGDDen.Text.Trim();
            string v_strSoGD = tetxSoGiaoDich.Text.Trim();
            string v_strSoPhieu = tetxSoPhieu.Text.Trim();

            foreach (ListItem v_objItem in cmbDonVi.Items)
            {
                if (v_objItem.Value.Contains(cmbDonVi.SelectedValue))
                {
                    v_strMaDonVi += "''" + v_objItem.Value + "'',";
                }
            }

            if (v_strMaDonVi.EndsWith(",")) v_strMaDonVi = v_strMaDonVi.Substring(0, v_strMaDonVi.Length - 1);
            v_strMaDonVi = "(" + v_strMaDonVi + ")";

            string v_strTreeVal = "";
            if (null != tvwLoaiGD.SelectedNode)
            {
                v_strTreeVal = tvwLoaiGD.SelectedNode.Value;
                v_strMaPhanHe = v_strTreeVal.Split('|')[0];
                v_strLoaiPhanHe = v_strTreeVal.Split('|')[1];
            }

            if (cbChoduyet.Checked)
                v_strTrangThai = v_strTrangThai + "''" + cbChoduyet.Value + "'',";
            if (cbDaduyet.Checked)
                v_strTrangThai = v_strTrangThai + "''" + cbDaduyet.Value + "'',";
            if (cbThoaiduyet.Checked)
                v_strTrangThai = v_strTrangThai + "''" + cbThoaiduyet.Value + "'',";
            if (cbDaduyet.Checked)
                v_strTrangThai = v_strTrangThai + "''" + cbDaduyet.Value + "'',";
            if (v_strTrangThai != "")
            {
                v_strTrangThai = "(" + v_strTrangThai.Substring(0, v_strTrangThai.Length - 1) + ")";
            }

            if (!Utils.IsDate(v_strTuNgay, "dd/MM/yyyy"))
            {
                v_strTuNgay = AppConfig.LoginedUser.NgayLamViecHienTai;
            }
            else
            {
                v_strTuNgay = DateTime.ParseExact(v_strTuNgay, "dd/MM/yyyy", null).ToString("yyyyMMdd");
            }

            if (!Utils.IsDate(v_strDenNgay, "dd/MM/yyyy"))
            {
                v_strDenNgay = AppConfig.LoginedUser.NgayLamViecHienTai;
            }
            else
            {
                v_strDenNgay = DateTime.ParseExact(v_strDenNgay, "dd/MM/yyyy", null).ToString("yyyyMMdd");
            }

            DataTable dt = null;
            LDatatable.MakeParameterTable(ref dt);
            LDatatable.AddParameter(ref dt, "@MaDonVi", "STRING", v_strMaDonVi);
            LDatatable.AddParameter(ref dt, "@MaPhanHe", "STRING", v_strMaPhanHe);
            LDatatable.AddParameter(ref dt, "@LoaiPhanHe", "STRING", v_strLoaiPhanHe);
            LDatatable.AddParameter(ref dt, "@TrangThai", "STRING", v_strTrangThai);
            LDatatable.AddParameter(ref dt, "@TuNgay", "STRING", v_strTuNgay);
            LDatatable.AddParameter(ref dt, "@DenNgay", "STRING", v_strDenNgay);
            LDatatable.AddParameter(ref dt, "@SoGD", "STRING", v_strSoGD);
            LDatatable.AddParameter(ref dt, "@SoPhieu", "STRING", v_strSoPhieu);
            LDatatable.AddParameter(ref dt, "@StartRow", "INT", "1");
            LDatatable.AddParameter(ref dt, "@EndRow", "INT", "1000000000");
            LDatatable.AddParameter(ref dt, "@UserName", "STRING", AppConfig.LoginedUser.UserName);
            LDatatable.AddParameter(ref dt, "@MaDViQLy", "STRING", AppConfig.LoginedUser.MaDonViQuanLy);
            DataSet ds = new KeToanProcess().getDanhSachGiaoDich(dt);

            if ((null != ds) && (ds.Tables.Count > 0))
            {
                ds.Tables[0].Columns.Add("URL", typeof(string));
                ds.Tables[0].Columns.Add("TRANG_THAI_DT", typeof(string));

                DataTable v_dt = AppConfig.LoginedUser.CayMenu;
                DataView v_dv = v_dt.DefaultView;

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string v_strMa_CNANG = "";
                    switch (dr["MA_LOAI_GDICH"].ToString())
                    {
                        case "GT01":
                            {
                                v_strMa_CNANG = "WEB_522_GUI_THEM_TIEN";
                                break;
                            }
                        case "GT02":
                            {
                                v_strMa_CNANG = "WEB_522_GUI_THEM_TIEN_TUNG_SO";
                                break;
                            }
                    }
                    string v_strID = dr["ID"].ToString();
                    string v_strURL = "";

                    if (dr["SO_GDICH"].ToString() == "GD201610000112")
                    {
                        v_dv.RowFilter = "";
                    }

                    v_dv.RowFilter = "";
                    v_dv.RowFilter = "MA_CNANG='" + v_strMa_CNANG + "'";
                    if (v_dv.Count > 0)
                    {
                        if (DBNull.Value != v_dv[0]["URL"])
                        {
                            v_strURL = v_dv[0]["URL"].ToString();
                            if (v_strURL.Contains("&"))
                            {
                                v_strURL = v_strURL.Substring(0, v_strURL.IndexOf("&"));
                            }
                        }
                    }
                    v_dv.RowFilter = "";
                    dr["URL"] = v_strURL.Replace("~", "");

                    string v_strTT = "";
                    switch (dr["TRANG_THAI"].ToString())
                    {
                        case "DDU":
                            {
                                v_strTT = LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.DungChung.TrangThaiNghiepVu.DaDuyet");
                                break;
                            }
                        case "CDU":
                            {
                                v_strTT = LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.DungChung.TrangThaiNghiepVu.ChoDuyet");
                                break;
                            }
                        case "TCD":
                            {
                                v_strTT = LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.DungChung.TrangThaiNghiepVu.TuChoi");
                                break;
                            }
                        case "THD":
                            {
                                v_strTT = LanguageEngine.Instance().GetContent(LanguageType.TypeUI, "U.DungChung.TrangThaiNghiepVu.ThoaiDuyet");
                                break;
                            }
                    }

                    dr["TRANG_THAI_DT"] =v_strTT; 
                }

                ListGiaoDich = ds.Tables[0];
                grdKiemSoatDS.DataSource = ds.Tables[0];
                grdKiemSoatDS.DataBind();
            }
        }

        private void LoadDetailGrid()
        {
            string v_strMaDonVi = "";
            string v_strIDGiaoDich = hdIDGD.Value;
            foreach (ListItem v_objItem in cmbDonVi.Items)
            {
                if (v_objItem.Value.Contains(cmbDonVi.SelectedValue))
                {
                    v_strMaDonVi += "''" + v_objItem.Value + "'',";
                }
            }

            if (v_strMaDonVi.EndsWith(",")) v_strMaDonVi = v_strMaDonVi.Substring(0, v_strMaDonVi.Length - 1);
            v_strMaDonVi = "(" + v_strMaDonVi + ")";
            if (v_strIDGiaoDich.Length > 0)
            {
                DataSet ds = new KeToanProcess().getDanhSachGiaoDichCT(v_strMaDonVi, v_strIDGiaoDich);
                if ((null != ds) && (ds.Tables.Count > 0))
                {
                    grdChiTietGD.DataSource = ds.Tables[0];
                    grdChiTietGD.DataBind();
                }
            }
        }

        private void BuildTree()
        {
            KeToanProcess process = new KeToanProcess();
            try
            {
                tvwLoaiGD.Nodes.Clear();
                DataSet ds = process.getTreePhanHeGD(AppConfig.LoginedUser.UserName, AppConfig.LoginedUser.MaDonViQuanLy);
                if (ds != null && ds.Tables.Count > 0)
                {
                    DataView v_dv = ds.Tables[0].DefaultView;
                    v_dv.RowFilter = "ID_PHAN_HE_CHA IS NULL";
                    if (v_dv.Count > 0)
                    {
                        for (int i = 0; i < v_dv.Count; i++)
                        {
                            TreeNode v_objNode = new TreeNode();
                            v_objNode.Text = v_dv[i]["TEN_PHAN_HE"].ToString();
                            v_objNode.Value = v_dv[i]["ID_PHAN_HE"].ToString() + "|" + v_dv[i]["TEN_BANG"].ToString();
                            if (i == 0)
                            {
                                v_objNode.Selected = true;
                            }
                            BuidSubNode(v_objNode, v_dv);
                            tvwLoaiGD.Nodes.Add(v_objNode);
                        }
                    }
                }
            }
            catch
            {
            }

            tvwLoaiGD.ExpandAll();

        }

        private void BuidSubNode(TreeNode pv_objNode, DataView pv_dv)
        {
            string v_strIDParent = pv_objNode.Value.Split('|')[0];
            string v_strTempFilter = pv_dv.RowFilter;
            pv_dv.RowFilter = "";
            pv_dv.RowFilter = "ID_PHAN_HE_CHA=" + v_strIDParent;

            if (pv_dv.Count > 0)
            {
                for (int i=0;i<pv_dv.Count;i++)
                {
                    TreeNode v_objNode = new TreeNode();
                    v_objNode.Text = pv_dv[i]["TEN_PHAN_HE"].ToString();
                    v_objNode.Value = pv_dv[i]["ID_PHAN_HE"].ToString() + "|" + pv_dv[i]["TEN_BANG"].ToString();
                    pv_objNode.ChildNodes.Add(v_objNode);
                    BuidSubNode(v_objNode, pv_dv);
                }
            }

            pv_dv.RowFilter = "";
            pv_dv.RowFilter = v_strTempFilter;

        }

        private void BuilCombobox()
        {
            List<string> lstDieuKien = new List<string>();
            AutoComboBox auto = new AutoComboBox();
            
            lstDieuKien.Add(AppConfig.LoginedUser.UserName);
            lstDieuKien.Add(AppConfig.LoginedUser.MaDonVi);
            lstDieuKien.Add(DatabaseConstant.ToChucDonVi.PGD.getValue());
            auto.GenAutoComboBox(ref lstSourceDonVi, ref cmbDonVi, DatabaseConstant.DanhSachTruyVan.COMBOBOX_DONVI_THEOPVI.getValue(), lstDieuKien, AppConfig.LoginedUser.MaDonViGiaoDich);
            cmbDonVi.SelectedIndex = 0;
        }

        private List<int> GetListIdLock(string pv_strIDList)
        {
            List<int> listLockId = new List<int>();
            string[] v_arrID = pv_strIDList.Split(';');

            for (int i = 0; i < v_arrID.GetLength(0); i++)
            {
                listLockId.Add(Convert.ToInt32(v_arrID[i]));
            }

            return listLockId;
        }

        private List<string> Duyet(string pv_strListItem)
        {
            List<string> listResult = new List<string>();
            if (pv_strListItem != "")
            {
                List<GDICH_KSOAT> lstGdich = new List<GDICH_KSOAT>();
                List<int> lstID = GetListIdLock(pv_strListItem);
                string[] str = pv_strListItem.Split(';');
                if (str != null && str.Length > 0)
                {
                    for (int k = 0; k < str.Length; k++)
                    {
                        DataView v_dv = ListGiaoDich.DefaultView;
                        v_dv.RowFilter = "";
                        v_dv.RowFilter = "ID=" + str[k];
                        if (v_dv.Count > 0)
                        {
                            GDICH_KSOAT obj = new GDICH_KSOAT();
                            obj.DIEN_GIAI = v_dv[0]["DIEN_GIAI"].ToString();
                            obj.ID_GDICH = Convert.ToInt32(v_dv[0]["ID"]);
                            obj.LY_DO = v_dv[0]["LY_DO"].ToString();
                            obj.MA_DVI = v_dv[0]["MA_DVI"].ToString();
                            obj.MA_GDICH = v_dv[0]["SO_GDICH"].ToString();
                            obj.MA_LOAI_GDICH = DatabaseConstant.layLoaiGiaoDich(v_dv[0]["MA_LOAI_GDICH"].ToString());
                            obj.MA_PHAN_HE = DatabaseConstant.getModule(v_dv[0]["MA_PHAN_HE"].ToString());
                            obj.NGAY_GDICH = v_dv[0]["NGAY_GIAO_DICH"].ToString();
                            obj.NGAY_CNHAT = AppConfig.LoginedUser.NgayLamViecHienTai;
                            obj.NGUOI_CNHAT = AppConfig.LoginedUser.UserName;
                            obj.NGUOI_NHAP = v_dv[0]["NGUOI_NHAP"].ToString();
                            obj.NGAY_NHAP = v_dv[0]["NGAY_NHAP"].ToString();
                            obj.MA_CNANG = v_dv[0]["MA_CNANG"].ToString();
                            lstGdich.Add(obj);
                        }
                    }

                    List<ClientResponseDetail> lstResponseDetail = new List<ClientResponseDetail>();
                    ApplicationConstant.ResponseStatus status = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                    Presentation.Process.KeToanProcess process = new Presentation.Process.KeToanProcess();
                    // Yêu cầu lock bản ghi cần xử lý
                    UtilitiesProcess Lockprocess = new UtilitiesProcess();
                    bool ret = Lockprocess.LockData(DatabaseConstant.Module.GDKT,
                        DatabaseConstant.Function.KT_GIAO_DICH,
                        DatabaseConstant.Table.KT_GIAO_DICH,
                        DatabaseConstant.Action.DUYET,
                        lstID);
                    bool retUnlockData = Lockprocess.UnlockData(DatabaseConstant.Module.GDKT,
                    DatabaseConstant.Function.KT_GIAO_DICH,
                    DatabaseConstant.Table.KT_GIAO_DICH,
                    DatabaseConstant.Action.DUYET,
                    lstID);

                    if (ret)
                    {
                        status = process.KiemSoatGiaoDich(lstGdich, DatabaseConstant.Action.DUYET, ref lstResponseDetail);
                        if (status == ApplicationConstant.ResponseStatus.THANH_CONG)
                        {
                            foreach (ClientResponseDetail cl in lstResponseDetail)
                            {
                                listResult.Add(LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, cl.Result) + "#" + LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, cl.Detail.Split('#')[0]));
                            }
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
            }

            return listResult;
        }

        private List<string> ThoaiDuyet(string pv_strListItem)
        {
            List<string> listResult = new List<string>();
            if (pv_strListItem != "")
            {
                List<GDICH_KSOAT> lstGdich = new List<GDICH_KSOAT>();
                List<int> lstID = GetListIdLock(pv_strListItem);
                string[] str = pv_strListItem.Split(';');
                if (str != null && str.Length > 0)
                {
                    for (int k = 0; k < str.Length; k++)
                    {
                        DataView v_dv = ListGiaoDich.DefaultView;
                        v_dv.RowFilter = "";
                        v_dv.RowFilter = "ID=" + str[k];
                        if (v_dv.Count > 0)
                        {
                            GDICH_KSOAT obj = new GDICH_KSOAT();
                            obj.DIEN_GIAI = v_dv[0]["DIEN_GIAI"].ToString();
                            obj.ID_GDICH = Convert.ToInt32(v_dv[0]["ID"]);
                            obj.LY_DO = v_dv[0]["LY_DO"].ToString();
                            obj.MA_DVI = v_dv[0]["MA_DVI"].ToString();
                            obj.MA_GDICH = v_dv[0]["SO_GDICH"].ToString();
                            obj.MA_LOAI_GDICH = DatabaseConstant.layLoaiGiaoDich(v_dv[0]["MA_LOAI_GDICH"].ToString());
                            obj.MA_PHAN_HE = DatabaseConstant.getModule(v_dv[0]["MA_PHAN_HE"].ToString());
                            obj.NGAY_GDICH = v_dv[0]["NGAY_GIAO_DICH"].ToString();
                            obj.NGAY_CNHAT = AppConfig.LoginedUser.NgayLamViecHienTai;
                            obj.NGUOI_CNHAT = AppConfig.LoginedUser.UserName;
                            obj.NGUOI_NHAP = v_dv[0]["NGUOI_NHAP"].ToString();
                            obj.NGAY_NHAP = v_dv[0]["NGAY_NHAP"].ToString();
                            obj.MA_CNANG = v_dv[0]["MA_CNANG"].ToString();
                            lstGdich.Add(obj);
                        }
                    }

                    List<ClientResponseDetail> lstResponseDetail = new List<ClientResponseDetail>();
                    ApplicationConstant.ResponseStatus status = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                    Presentation.Process.KeToanProcess process = new Presentation.Process.KeToanProcess();
                    // Yêu cầu lock bản ghi cần xử lý
                    UtilitiesProcess Lockprocess = new UtilitiesProcess();
                    bool ret = Lockprocess.LockData(DatabaseConstant.Module.GDKT,
                        DatabaseConstant.Function.KT_GIAO_DICH,
                        DatabaseConstant.Table.KT_GIAO_DICH,
                        DatabaseConstant.Action.THOAI_DUYET,
                        lstID);
                    bool retUnlockData = Lockprocess.UnlockData(DatabaseConstant.Module.GDKT,
                    DatabaseConstant.Function.KT_GIAO_DICH,
                    DatabaseConstant.Table.KT_GIAO_DICH,
                    DatabaseConstant.Action.THOAI_DUYET,
                    lstID);

                    if (ret)
                    {
                        status = process.KiemSoatGiaoDich(lstGdich, DatabaseConstant.Action.THOAI_DUYET, ref lstResponseDetail);
                        if (status == ApplicationConstant.ResponseStatus.THANH_CONG)
                        {
                            foreach (ClientResponseDetail cl in lstResponseDetail)
                            {
                                listResult.Add(LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, cl.Result) + "#" + LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, cl.Detail.Split('#')[0]));
                            }
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
            }

            return listResult;
        }

        private List<string> TuChoi(string pv_strListItem)
        {
            List<string> listResult = new List<string>();
            if (pv_strListItem != "")
            {
                List<GDICH_KSOAT> lstGdich = new List<GDICH_KSOAT>();
                List<int> lstID = GetListIdLock(pv_strListItem);
                string[] str = pv_strListItem.Split(';');
                if (str != null && str.Length > 0)
                {
                    for (int k = 0; k < str.Length; k++)
                    {
                        DataView v_dv = ListGiaoDich.DefaultView;
                        v_dv.RowFilter = "";
                        v_dv.RowFilter = "ID=" + str[k];
                        if (v_dv.Count > 0)
                        {
                            GDICH_KSOAT obj = new GDICH_KSOAT();
                            obj.DIEN_GIAI = v_dv[0]["DIEN_GIAI"].ToString();
                            obj.ID_GDICH = Convert.ToInt32(v_dv[0]["ID"]);
                            obj.LY_DO = v_dv[0]["LY_DO"].ToString();
                            obj.MA_DVI = v_dv[0]["MA_DVI"].ToString();
                            obj.MA_GDICH = v_dv[0]["SO_GDICH"].ToString();
                            obj.MA_LOAI_GDICH = DatabaseConstant.layLoaiGiaoDich(v_dv[0]["MA_LOAI_GDICH"].ToString());
                            obj.MA_PHAN_HE = DatabaseConstant.getModule(v_dv[0]["MA_PHAN_HE"].ToString());
                            obj.NGAY_GDICH = v_dv[0]["NGAY_GIAO_DICH"].ToString();
                            obj.NGAY_CNHAT = AppConfig.LoginedUser.NgayLamViecHienTai;
                            obj.NGUOI_CNHAT = AppConfig.LoginedUser.UserName;
                            obj.NGUOI_NHAP = v_dv[0]["NGUOI_NHAP"].ToString();
                            obj.NGAY_NHAP = v_dv[0]["NGAY_NHAP"].ToString();
                            obj.MA_CNANG = v_dv[0]["MA_CNANG"].ToString();
                            lstGdich.Add(obj);
                        }
                    }

                    List<ClientResponseDetail> lstResponseDetail = new List<ClientResponseDetail>();
                    ApplicationConstant.ResponseStatus status = ApplicationConstant.ResponseStatus.KHONG_THANH_CONG;
                    Presentation.Process.KeToanProcess process = new Presentation.Process.KeToanProcess();
                    // Yêu cầu lock bản ghi cần xử lý
                    UtilitiesProcess Lockprocess = new UtilitiesProcess();
                    bool ret = Lockprocess.LockData(DatabaseConstant.Module.GDKT,
                        DatabaseConstant.Function.KT_GIAO_DICH,
                        DatabaseConstant.Table.KT_GIAO_DICH,
                        DatabaseConstant.Action.TU_CHOI_DUYET,
                        lstID);
                    bool retUnlockData = Lockprocess.UnlockData(DatabaseConstant.Module.GDKT,
                    DatabaseConstant.Function.KT_GIAO_DICH,
                    DatabaseConstant.Table.KT_GIAO_DICH,
                    DatabaseConstant.Action.TU_CHOI_DUYET,
                    lstID);

                    if (ret)
                    {
                        status = process.KiemSoatGiaoDich(lstGdich, DatabaseConstant.Action.TU_CHOI_DUYET, ref lstResponseDetail);
                        if (status == ApplicationConstant.ResponseStatus.THANH_CONG)
                        {
                            foreach (ClientResponseDetail cl in lstResponseDetail)
                            {
                                listResult.Add(LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, cl.Result) + "#" + LanguageEngine.Instance().GetContent(LanguageType.TypeMessage, cl.Detail.Split('#')[0]));
                            }
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
            }

            return listResult;
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitForm();
            }
            else
            {
                if (cfaction.Value == "approve")
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
                    LoadGDGrid();
                    cfaction.Value = "0";
                }
                else if (cfaction.Value == "reject")
                {
                    List<string> kq = TuChoi(chkvalgrid.Value);
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
                    LoadGDGrid();
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
                    LoadGDGrid();
                    cfaction.Value = "0";
                }
                else if (hdIDGD.Value.Length > 0)
                {
                    //Load detail
                    LoadDetailGrid();
                }
            }
        }

        protected void grdKiemSoatDS_ItemDataBound(Object sender, DataGridItemEventArgs e)
        {
            e.Item.Attributes.Add("onmouseover", "currColor=this.style.backgroundColor;this.style.backgroundColor='#FDDBA5'");
            e.Item.Attributes.Add("onmouseout", "this.style.backgroundColor=currColor");
        }

        protected void btnTimkiem_Click(object sender, EventArgs e)
        {
            LoadGDGrid();
        }
    }
}