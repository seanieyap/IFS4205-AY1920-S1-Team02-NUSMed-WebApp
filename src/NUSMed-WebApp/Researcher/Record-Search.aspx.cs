﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NUSMed_WebApp.Researcher
{
    public partial class Record_Search : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.LiActiveResearcherRecordSearch();

        }
    }
}