using System;
using System.Collections.Generic;
using System.Data;
using System.Xml;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Presentation.WebClient.Business;
using Utilities.Common;

namespace Presentation.WebClient.Modules.KTDL.BaoCao
{
    public partial class ucDanhSach : ControlBase
    {
        #region [Variables]

        protected string MaBC
        {
            set
            {
                ViewState["MaBC"] = value;
            }
            get
            {
                string v_strMaBC = "";

                if (null != ViewState["MaBC"])
                {
                    v_strMaBC = ViewState["MaBC"].ToString();
                }

                return v_strMaBC;
            }
        }

        protected string PhanHe
        {
            get
            {
                string v_strPhanHe = "";
                if (null!=Request.QueryString["p"])
                {
                    v_strPhanHe = Request.QueryString["p"];
                }

                return v_strPhanHe;
            }
        }

        #endregion

        #region [User Functions]

        #region [Danh sach]

        private void BindDMBaoCaoCombo()
        {
            cboDanhMucBC.Items.Clear();

            DataTable v_dt = BaoCaoController.GetDMBaoCao();
            Utils.BuildDropdownlist(ref cboDanhMucBC, v_dt, true);
        }

        private void LoadDSBaocaoGrid()
        {
            DataTable v_dt = BaoCaoController.GetDSBaoCao(cboDanhMucBC.SelectedValue);
            if (null != v_dt)
            {
                rptList.DataSource = v_dt;
                rptList.DataBind();
            }
        }

        private void InitDS()
        {
            dvDS.Visible = true;
            dvParam.Visible = false;
            dvReport.Visible = false;

            BindDMBaoCaoCombo();
            LoadDSBaocaoGrid();
            if (PhanHe.Length > 0)
            {
                cboDanhMucBC.SelectedValue = PhanHe;
                cboDanhMucBC.Enabled = false;
            }
        }

        #endregion

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                InitDS();
            }
        }
    }
}