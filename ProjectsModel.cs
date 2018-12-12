using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using PureSurveyProjectTracker.Models.DB;
using System.Web.Mvc;
using PureSurveyProjectTracker.Helpers;

namespace PureSurveyProjectTracker.Models.ViewModels
{
    public class ProjectsModel
    {
        public Guid UserId { get; set; }
        public Guid _projectId { get; set; }
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
        //[Required(ErrorMessage = "URL")]
        //[Display(Name = "URL")]
        public string URL { get; set; }    
        //[Required(ErrorMessage = "ProjectDB")]
        //[Display(Name = "Project DB")]
        public string ProjectDB { get; set; }      
        //[Required(ErrorMessage = "ProjectServer")]
        //[Display(Name = "Project Server")]
        public string ProjectServer { get; set; }       
        [Required(ErrorMessage = "BackUpPlan")]
        //[Display(Name = "Back Up Plan")]
        public string BackUp { get; set; }
        [Required(ErrorMessage = "Value")]
        //[DisplayFormat(DataFormatString = "{0:C0}", ApplyFormatInEditMode = true)]
        //[DataType(DataType.Currency)]
        public double? ProjectValue { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime Invoice_Date { get; set; }
        public string Payment_Status { get; set; }
        public string Project_Term { get; set; }
        public string searchValue { get; set; }
        public SortDirections SortDirection { get; set; }
        public SortItems SortItem { get; set; }

        //Pagination     
            public List<ProjectsModel> CampaignItems { get; set; }
            public int ItemsPerPage { get; set; }
            public int PageNo { get; set; }
            public int NoPages { get; set; }

        //Hiddenfields


        public List<string> Types { get; set; }
        public List<string> Statuses { get; set; }
        public List<string> BackUpOptions { get; set; }
        public List<string> PMs { get; set; }
        public List<string> Devs { get; set; }
        public List<string> Desgns { get; set; }

        public ProjectsModel(Guid userId)
        {
            UserId = userId;
        }
        public ProjectsModel()
        {
        }

    }
}