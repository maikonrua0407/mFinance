using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Presentation.WebClient.UI.Control
{
    public class CurrentPage : Label
    {
        public string text = "Trang {0}/{1} ({2} bản ghi)";

        protected override void Render(HtmlTextWriter writer)
        {
            Panel panel = (Panel)this.Parent.FindControl("DisplayPager");
            if (panel != null)
            {
                panel.Visible = true;
            }
            this.Text = string.Format(this.text, this.PageIndex, this.TotalPages.ToString("n0"), this.TotalRecords.ToString("n0"));
            base.Render(writer);
        }

        public int PageIndex
        {
            get
            {
                int num = Convert.ToInt32(this.ViewState["PageIndex"]);
                if (num == 0)
                {
                    return 1;
                }
                return num;
            }
            set
            {
                this.ViewState["PageIndex"] = value + 1;
            }
        }

        public string TextFormat
        {
            get
            {
                return this.text;
            }
            set
            {
                this.text = value;
            }
        }

        public int TotalPages
        {
            get
            {
                int num = Convert.ToInt32(this.ViewState["TotalPages"]);
                if (num == 0)
                {
                    return 1;
                }
                return num;
            }
            set
            {
                this.ViewState["TotalPages"] = value;
            }
        }

        public int TotalRecords
        {
            get
            {
                return Convert.ToInt32(this.ViewState["TotalRecords"]);
            }
            set
            {
                this.ViewState["TotalRecords"] = value;
            }
        }
    }
}