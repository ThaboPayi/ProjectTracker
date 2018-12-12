using PureSurveyProjectTracker.Models.EntityManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PureSurveyProjectTracker.Helpers
{
    public class AuthorisationHelper
    {
        private const string adminName = "Administrator";
        

        public bool IsAdministrator(Guid Id)
        {
            return HasPermission(Id, adminName);
        }

        private bool HasPermission(Guid Id, string permissionname)
        {
            ProjectsManager _manager = new ProjectsManager();
            var roles = _manager.GetUserRolesByUserId(Id);

            if (roles.Select(m => m.RoleName).Contains(permissionname))
            {
                return true;
            }
            return false;
        }

    }
}