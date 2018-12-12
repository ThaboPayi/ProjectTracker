using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PureSurveyProjectTracker.Models.ViewModels
{
    public class UserSignUpView
    {
        [Key]
        public Guid SYSUserID { get; set; }
        public Guid LOOKUPRoleID { get; set; }
        public string RoleName { get; set; }
        [EmailAddress]
        [Required(ErrorMessage = "*")]
        [Display(Name = "Login ID")]
        public string LoginName { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Password")]
        public string Password { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "*")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public string Gender { get; set; }
    }
}