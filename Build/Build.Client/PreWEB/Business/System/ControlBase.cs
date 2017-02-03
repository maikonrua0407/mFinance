using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace Presentation.WebClient.Business
{
    public class ControlBase : UserControl
    {
        #region [Properties]

        private Dictionary<string, string> _ModuleParams;

        public Dictionary<string, string> ModuleParams
        {
            get { return _ModuleParams; }
            set { _ModuleParams = value; }
        }

        private bool mv_blCanAdd = false;

        public bool CanAdd
        {
            get
            {
                return mv_blCanAdd;
            }

            set
            {
                mv_blCanAdd = value;
            }
        }

        private bool mv_blCanUpdate = false;

        public bool CanUpdate
        {
            get
            {
                return mv_blCanUpdate;
            }

            set
            {
                mv_blCanUpdate = value;
            }
        }

        private bool mv_blCanDelete = false;

        public bool CanDelete
        {
            get
            {
                return mv_blCanDelete;
            }

            set
            {
                mv_blCanDelete = value;
            }
        }

        #endregion

        #region [Contructor]
        public ControlBase()
            : base()
        {
            this.Load += new EventHandler(ControlBase_Load);
            this.PreRender += new EventHandler(ControlBase_PreRender);
        }

        protected virtual void ControlBase_Load(object sender, EventArgs args)
        {
            if (!this.IsPostBack)
            {
                //Register Event Hander
                //Track History
                //HistoryTracer.Push(Convert.ToString(Application["AppID"]), Request.RawUrl);
            }

            //Translators.Instance().Translate(AppConfig.Language, this);
        }

        private void ControlBase_PreRender(object sender, EventArgs args)
        {
            //Translate Engine
            //Translators.Instance().Translate(AppConfig.Language, this);
        }

        #endregion

        #region [Can Override]

        protected void Add()
        {

        }

        protected void Update()
        {
        }

        protected void Delete()
        {
        }

        #endregion

        #region [Utils Methods]

        public static Control FindControlRecursive(Control control, string id)
        {
            if (control == null) return null;
            //try to find the control at the current level
            Control ctrl = control.FindControl(id);

            if (ctrl == null)
            {
                //search the children
                foreach (Control child in control.Controls)
                {
                    ctrl = FindControlRecursive(child, id);

                    if (ctrl != null) break;
                }
            }
            return ctrl;
        }

        public string GetKeyParam(string key, string defaultValue)
        {
            string strRet = defaultValue;
            if (this.ModuleParams.ContainsKey(key))
            {
                strRet = this.ModuleParams[key];
            }

            return strRet;
        }

        #endregion
    }
}