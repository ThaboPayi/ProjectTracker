using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PureSurveyProjectTracker.Models.ViewModels
{
    public class UserLoginView
    {
        [EmailAddress]
        [Required(ErrorMessage = "*")]
        [Display(Name = "User Name")]
        public string LoginName { get; set; }
        [Required(ErrorMessage = "*")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}