using System;
using System.Runtime.CompilerServices;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Presentation.WebClient.UI.Control
{
    public class Pager : Label, INamingContainer
    {
        private LinkButton firstButton;
        private LinkButton lastButton;
        private LinkButton nextButton;
        private LinkButton[] pagingLinkButtons;
        private LinkButton previousButton;

        public event EventHandler IndexChanged;

        private void AddFirstLastLinkButtons()
        {
            this.firstButton = new LinkButton();
            this.firstButton.ID = "First";
            this.firstButton.Text = "|&lt;";
            this.firstButton.CommandArgument = "0";
            this.firstButton.Click += new EventHandler(this.PageIndex_Click);
            this.Controls.Add(this.firstButton);
            this.lastButton = new LinkButton();
            this.lastButton.ID = "Last";
            this.lastButton.Text = "&gt;|"; 
            this.lastButton.CommandArgument = (this.CalculateTotalPages() - 1).ToString();
            this.lastButton.Click += new EventHandler(this.PageIndex_Click);
            this.Controls.Add(this.lastButton);
        }

        private void AddPageButtons()
        {
            this.pagingLinkButtons = new LinkButton[this.CalculateTotalPages()];
            for (int i = 0; i < this.pagingLinkButtons.Length; i++)
            {
                this.pagingLinkButtons[i] = new LinkButton();
                this.pagingLinkButtons[i].EnableViewState = false;
                this.pagingLinkButtons[i].Text = (i + 1).ToString();
                this.pagingLinkButtons[i].ID = i.ToString();
                this.pagingLinkButtons[i].CommandArgument = i.ToString();
                this.pagingLinkButtons[i].Click += new EventHandler(this.PageIndex_Click);
                this.Controls.Add(this.pagingLinkButtons[i]);
            }
        }

        private void AddPreviousNextLinkButtons()
        {
            this.previousButton = new LinkButton();
            this.previousButton.ID = "Prev";
            this.previousButton.Text = "&lt;&lt;";
            this.previousButton.CommandArgument = (this.PageIndex - 1).ToString();
            this.previousButton.Click += new EventHandler(this.PageIndex_Click);
            this.Controls.Add(this.previousButton);
            this.nextButton = new LinkButton();
            this.nextButton.ID = "Next";
            this.nextButton.Text = "&gt;&gt;";
            this.nextButton.CommandArgument = (this.PageIndex + 1).ToString();
            this.nextButton.Click += new EventHandler(this.PageIndex_Click);
            this.Controls.Add(this.nextButton);
        }

        public int CalculateTotalPages()
        {
            if (this.TotalRecords == 0)
            {
                return 0;
            }
            int num = this.TotalRecords / this.PageSize;
            if ((this.TotalRecords % this.PageSize) > 0)
            {
                num++;
            }
            return num;
        }

        protected override void CreateChildControls()
        {
            this.Controls.Clear();
            this.AddPageButtons();
            this.AddPreviousNextLinkButtons();
            this.AddFirstLastLinkButtons();
        }

        private void PageIndex_Click(object sender, EventArgs e)
        {
            this.PageIndex = Convert.ToInt32(((LinkButton)sender).CommandArgument);
            if (null != this.IndexChanged)
            {
                this.IndexChanged(sender, e);
            }
        }

        private void PageIndexImages_Click(object sender, ImageClickEventArgs e)
        {
            this.PageIndex = Convert.ToInt32(((ImageButton)sender).CommandArgument);
            if (null != this.IndexChanged)
            {
                this.IndexChanged(sender, e);
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (this.CalculateTotalPages() > 1)
            {
                this.AddAttributesToRender(writer);
                writer.AddAttribute(HtmlTextWriterAttribute.Class, this.CssClass, false);
                this.RenderFirst(writer);
                this.RenderPrevious(writer);
                this.RenderPagingButtons(writer);
                this.RenderNext(writer);
                this.RenderLast(writer);
            }
        }

        private void RenderButtonRange(int start, int end, HtmlTextWriter writer)
        {
            for (int i = start; i < end; i++)
            {
                if (this.PageIndex == i)
                {
                    Literal literal = new Literal();
                    literal.Text = "<span class=\"current\">" + ((i + 1)).ToString() + "</span>";
                    literal.RenderControl(writer);
                }
                else
                {
                    this.pagingLinkButtons[i].RenderControl(writer);
                }
                if (i < (end - 1))
                {
                    writer.Write(" ");
                }
            }
        }

        private void RenderFirst(HtmlTextWriter writer)
        {
            int num = this.CalculateTotalPages();
            if ((this.PageIndex >= 3) && (num > 5))
            {
                this.firstButton.RenderControl(writer);
                new LiteralControl("&nbsp;...&nbsp;").RenderControl(writer);
            }
        }

        private void RenderLast(HtmlTextWriter writer)
        {
            int num = this.CalculateTotalPages();
            if (((this.PageIndex + 3) < num) && (num > 5))
            {
                new LiteralControl("&nbsp;...&nbsp;").RenderControl(writer);
                this.lastButton.RenderControl(writer);
            }
        }

        private void RenderNext(HtmlTextWriter writer)
        {
            if (this.HasNext)
            {
                Literal literal = new Literal();
                literal.Text = "&nbsp;";
                literal.RenderControl(writer);
                this.nextButton.RenderControl(writer);
            }
        }

        private void RenderPagingButtons(HtmlTextWriter writer)
        {
            int end = this.CalculateTotalPages();
            if (end <= 5)
            {
                this.RenderButtonRange(0, end, writer);
            }
            else
            {
                int start = this.PageIndex - 2;
                int num3 = this.PageIndex + 3;
                if (start <= 0)
                {
                    start = 0;
                }
                if (this.PageIndex == 0)
                {
                    this.RenderButtonRange(0, 5, writer);
                }
                else if (this.PageIndex == 1)
                {
                    this.RenderButtonRange(0, this.PageIndex + 4, writer);
                }
                else if (this.PageIndex < (end - 2))
                {
                    this.RenderButtonRange(start, this.PageIndex + 3, writer);
                }
                else if (this.PageIndex == (end - 2))
                {
                    this.RenderButtonRange(end - 5, this.PageIndex + 2, writer);
                }
                else if (this.PageIndex == (end - 1))
                {
                    this.RenderButtonRange(end - 5, this.PageIndex + 1, writer);
                }
            }
        }

        private void RenderPrevious(HtmlTextWriter writer)
        {
            if (this.HasPrevious)
            {
                this.previousButton.RenderControl(writer);
                Literal literal = new Literal();
                literal.Text = "&nbsp;";
                literal.RenderControl(writer);
            }
        }

        public override ControlCollection Controls
        {
            get
            {
                this.EnsureChildControls();
                return base.Controls;
            }
        }

        private bool HasNext
        {
            get
            {
                return ((this.PageIndex + 1) < this.CalculateTotalPages());
            }
        }

        private bool HasPrevious
        {
            get
            {
                return (this.PageIndex > 0);
            }
        }

        public int PageIndex
        {
            get
            {
                int num = 0;
                num = Convert.ToInt32(this.ViewState["PageIndex"]);
                if (num < 0)
                {
                    return 0;
                }
                return num;
            }
            set
            {
                this.ViewState["PageIndex"] = value;
            }
        }

        public int PageSize
        {
            get
            {
                return Convert.ToInt32(this.ViewState["PageSize"]);
            }
            set
            {
                this.ViewState["PageSize"] = value;
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