﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NUSMed_WebApp.Patient
{
    public partial class My_Therapists : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.LiActivePatientMyTherapists();
        }
    }
}