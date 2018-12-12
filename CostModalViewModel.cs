using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PureSurveyProjectTracker.Models.ViewModels
{
    public class CostModalViewModel
    {
        public string _Proj_Name { get; set; }
        public string _Proj_Client { get; set; }
        public string _Proj_Manager { get; set; }
        public string _Proj_Dev { get; set; }
        public string _Proj_Designer { get; set; }
        public string _Proj_Type { get; set; }
        public string _Proj_Status { get; set; }
        public string _Proj_URl { get; set; }
        public string _Proj_DB { get; set; }
        public string _Proj_Server { get; set; }
        public string _Proj_Backup_Plan { get; set; }
        public double? _Proj_Value { get; set; }
        public DateTime? _Proj_Date_Created { get; set; }
    }
}