using Presentation.WebClient.Business;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using Utilities.Common;

namespace Presentation.WebClient.Modules.KHTV.Cum
{
    public partial class ucCumDS : ControlBase
    {
        #region [Properties]

        #endregion

        #region [Custom Methods]

        #region [DS]

        private void LoadDonViTree()
        {
            tvKhuVuc.Nodes.Clear();

            DataTable v_dtTreeDonVi = TruyVanController.GetTreeDonVi(AppConfig.LoginedUser.UserName, AppConfig.LoginedUser.MaDonVi);
            if (null != v_dtTreeDonVi)
            {
                DataView v_dv = v_dtTreeDonVi.DefaultView;
                v_dv.RowFilter = "NODE_PARENT IS NULL";
                if (v_dv.Count > 0)
                {
                    for (int i = 0; i < v_dv.Count; i++)
                    {
                        TreeNode v_objNode = new TreeNode();
                        v_objNode.Text = v_dv[i]["NODE_NAME"].ToString();
                        v_objNode.Value = v_dv[i]["NODE"].ToString() + "|" + v_dv[i]["NODE_TYPE"].ToString();
                        if (i == 0)
                        {
                            v_objNode.Selected = true;
                        }
                        BuidSubNode(v_objNode, v_dv);
                        tvKhuVuc.Nodes.Add(v_objNode);
                    }
                }
            }

            tvKhuVuc.ExpandAll();
        }

        private void BuidSubNode(TreeNode pv_objNode, DataView pv_dv)
        {
            string v_strIDParent = pv_objNode.Value.Split('|')[0];
            string v_strTempFilter = pv_dv.RowFilter;
            pv_dv.RowFilter = "";
            pv_dv.RowFilter = "NODE_PARENT=" + v_strIDParent;

            if (pv_dv.Count > 0)
            {
                for (int i = 0; i < pv_dv.Count; i++)
                {
                    TreeNode v_objNode = new TreeNode();
                    v_objNode.Text = pv_dv[i]["NODE_NAME"].ToString();
                    v_objNode.Value = pv_dv[i]["NODE"].ToString() + "|" + pv_dv[i]["NODE_TYPE"].ToString();
                    pv_objNode.ChildNodes.Add(v_objNode);
                    BuidSubNode(v_objNode, pv_dv);
                }
            }

            pv_dv.RowFilter = "";
            pv_dv.RowFilter = v_strTempFilter;
        }


        private void LoadGrid()
        {
            string v_strDonVi = tvKhuVuc.SelectedNode.Value.Replace("#", "").Split('|')[0];
            string v_strLoai = tvKhuVuc.SelectedNode.Value.Replace("#", "").Split('|')[1];
            DataTable v_dt = DanhMucController.GetDSCum(AppConfig.LoginedUser.UserName, AppConfig.LoginedUser.MaDonVi, v_strDonVi, v_strLoai);
            if (null != v_dt)
            {
                DataView v_dvCum = v_dt.DefaultView;
                rptList.DataSource = v_dvCum;
                rptList.DataBind();
            }
        }

        private void InitDS()
        {
            LoadDonViTree();
            LoadGrid();
        }

        #endregion

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InitDS();
            }
        }

        protected void cmdBack_Click(object sender, EventArgs e)
        {
            InitDS();
        }

        protected void tvKhuVuc_SelectedNodeChanged(object sender, EventArgs e)
        {
            LoadGrid();
        }
    }
}