﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NUSMed_WebApp.Therapist.My_Medical_Notes
{
    public partial class New_Medical_Note : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Master.LiActiveTherapistMyMedicalNotes();
            Master.LiActiveTherapistMyMedicalNotesNew();

        }
    }
}