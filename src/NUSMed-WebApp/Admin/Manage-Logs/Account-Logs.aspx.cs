using NUSMed_WebApp.Classes.BLL;
using NUSMed_WebApp.Classes.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NUSMed_WebApp.Admin.Manage_Logs
{
    public partial class Account_Logs : Page
    {
        private readonly LogBLL logBLL = new LogBLL();

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.LiActiveAdminManageLogs();
            Master.LiActiveAdminViewAccountLogs();

            if (!IsPostBack)
            {
                foreach (string creatorNRIC in logBLL.GetCreatorNRICs())
                {
                    InputSubjectNRICs.Items.Add(new ListItem(creatorNRIC, creatorNRIC));
                }
                foreach (string action in logBLL.GetActions())
                {
                    inputActions.Items.Add(new ListItem(action, action));
                }

                GridViewLogs.DataBind();
            }
        }

        #region GridViewAccounts Functions
        protected void ButtonSearch_Click(object sender, EventArgs e)
        {
            // subjects
            List<string> subjectNRICs = new List<string>();
            foreach (ListItem item in InputSubjectNRICs.Items)
            {
                if (item.Selected)
                {
                    subjectNRICs.Add(item.Value.Trim());
                }
            }

            // actions
            List<string> actions = new List<string>();
            foreach (ListItem item in inputActions.Items)
            {
                if (item.Selected)
                {
                    actions.Add(item.Value.Trim());
                }
            }

            DateTime? dateTimeFrom = null;
            DateTime? dateTimeTo = null;
            if (!string.IsNullOrEmpty(inputDateTimeFrom.Value))
            {
                dateTimeFrom = DateTime.Parse(inputDateTimeFrom.Value);
            }
            if (!string.IsNullOrEmpty(inputDateTimeTo.Value))
            {
                dateTimeTo = DateTime.Parse(inputDateTimeTo.Value);
            }

            List<LogEvent> logEvents = logBLL.GetAccountLogs(subjectNRICs, actions, dateTimeFrom, dateTimeTo);
            ViewState["GridViewLogs"] = logEvents;
            GridViewLogs.DataSource = logEvents;
            GridViewLogs.DataBind();
        }
        //protected void Bind_GridViewAccounts(string term)
        //{
        //    List<LogEvent> logEvents = logBLL.GetAccountLogs(term);
        //    ViewState["GridViewLogs"] = logEvents;
        //    GridViewLogs.DataSource = logEvents;
        //    GridViewLogs.DataBind();
        //}
        protected void GridViewAccounts_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewLogs.PageIndex = e.NewPageIndex;
            GridViewLogs.DataSource = ViewState["GridViewLogs"];
            GridViewLogs.DataBind();
        }
        #endregion
    }
}