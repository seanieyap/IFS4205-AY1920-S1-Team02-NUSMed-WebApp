using NUSMed_WebApp.Classes.BLL;
using NUSMed_WebApp.Classes.Entity;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NUSMed_WebApp.Admin.View_Logs
{
    public partial class Record_Logs : Page
    {
        private readonly LogRecordBLL logBLL = new LogRecordBLL();

        protected void Page_Load(object sender, EventArgs e)
        {
            Master.LiActiveAdminViewLogs();
            Master.LiActiveAdminViewRecordLogs();

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

        #region GridViewLogs Functions
        protected void ButtonSearch_Click(object sender, EventArgs e)
        {
            try
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

                DateTime? dateTimeFrom = string.IsNullOrEmpty(inputDateTimeFrom.Value) ? null :
                    (DateTime?)DateTime.ParseExact(inputDateTimeFrom.Value, "MM/dd/yyyy h:mm tt", System.Globalization.CultureInfo.InvariantCulture);
                DateTime? dateTimeTo = string.IsNullOrEmpty(inputDateTimeTo.Value) ? null :
                    (DateTime?)DateTime.ParseExact(inputDateTimeTo.Value, "MM/dd/yyyy h:mm tt", System.Globalization.CultureInfo.InvariantCulture);

                List<LogEvent> logEvents = logBLL.GetLogs(subjectNRICs, actions, dateTimeFrom, dateTimeTo);
                ViewState["GridViewLogs"] = logEvents;
                GridViewLogs.DataSource = logEvents;
                GridViewLogs.DataBind();

                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['success']('Logs successfully displayed.');", true);
            }
            catch
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "alert", "toastr['error']('Error occured when displaying logs.');", true);
            }
        }
        protected void GridViewAccounts_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GridViewLogs.PageIndex = e.NewPageIndex;
            GridViewLogs.DataSource = ViewState["GridViewLogs"];
            GridViewLogs.DataBind();
        }
        #endregion
    }
}