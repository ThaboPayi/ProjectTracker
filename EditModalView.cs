using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PureSurveyProjectTracker.Models.ViewModels
{
    public class EditModalView
    {
        public Guid userId { get; set; }
        public List<string> Types { get; set; }
        public List<string> Statuses { get; set; }
        public List<string> BackUpOptions { get; set; }
        public List<string> PMs { get; set; }
        public List<string> Devs { get; set; }
        public List<string> Desgns { get; set; }
        //public Guid ProjectID { get; set; }
        public int projectNo { get; set; }
        //Project straight single propeties
        [Required(ErrorMessage = "ProjectName")]
        //[Display(Name = "Project Name")]
        public string ProjectName { get; set; }
        [Required(ErrorMessage = "ClientName")]
        //[Display(Name = "Client Name")]
        public string ClientName { get; set; }
        [Required(ErrorMessage = "ProjectManager")]
        //[Display(Name = "Project Manager")]
        public string ProjectManager { get; set; }
        [Required(ErrorMessage = "Developer")]
        //[Display(Name = "Developer Name")]
        public string Developer { get; set; }
        [Required(ErrorMessage = "Designer")]
        //[Display(Name = "Designer Name")]
        public string Designer { get; set; }
        [Required(ErrorMessage = "ProjectType")]
        //[Display(Name = "Project Type")]
        public string ProjectType { get; set; }

        [Required(ErrorMessage = "Status")]
        //[Display(Name = "Project Status")]
        public string Status { get; set; }
        //[Display(Name = "URL")]
        public string URL { get; set; }
        //[Display(Name = "Project DB")]
        public string ProjectDB { get; set; }
        //[Display(Name = "Project Server")]
        public string ProjectServer { get; set; }
        [Required(ErrorMessage = "BackUpOption")]
        //[Display(Name = "Back Up Plan")]
        public string BackUp { get; set; }
        [Required(ErrorMessage = "Value")]
        //[DisplayFormat(DataFormatString = "{0:C0}", ApplyFormatInEditMode = true)]
        //[DataType(DataType.Currency)]
        public double? Project_Value { get; set; }

        //Hiddenfields
        public Guid _projectId { get; set; }
    }
}