using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using PureSurveyProjectTracker.Models.DB;
using PureSurveyProjectTracker.Models.ViewModels;
using PureSurveyProjectTracker.Models.EntityManager;
using PureSurveyProjectTracker.Helpers;

namespace PureSurveyProjectTracker.Models.EntityManager
{
    public class ProjectsManager
    {
        private AuthorisationHelper _authHelper;
        public ProjectsManager()
        {
            _authHelper = new AuthorisationHelper();
        }
        public string AddProject(Guid UserId, string _projectName, string _client, string _projectManager, string _developer, string _designer, string _type, string _status, string _url, string _db, string _server, string _backupPlan, double _cost)
        {
            string _projectCheck = "";
            using (PureSurveyProjectTrackerEntities ptdb = new PureSurveyProjectTrackerEntities())
            {
                var user = ptdb.SYSUsers.FirstOrDefault(m => m.SYSUserID == UserId);
                Project pt = ptdb.Projects.Where(x => x.ProjectName == _projectName).Distinct().FirstOrDefault();
                if (pt != null)
                {
                    _projectCheck = pt.ProjectName.ToString() + "Already exists !";
                }
                else
                {
                    pt = new Project
                    {
                        Project_ID = Guid.NewGuid(),
                        ProjectName = _projectName,
                        ClientName = _client,
                        ProjectManager = _projectManager,
                        Developer = _developer,
                        Designer = _designer,
                        ProjectTypeDescription = _type,
                        Status = _status,
                        URL = _url,
                        ProjectDB = _db,
                        ProjectServer = _server,
                        BackUpPlan = _backupPlan,
                        ProjectCost = _cost,
                        DateCreated = DateTime.Now
                    };

                    pt.SYSUsers.Add(user);

                    ptdb.Projects.AddOrUpdate(pt);
                    ptdb.SaveChanges();

                    _projectCheck = pt.ProjectName.ToString() + "Added Succesfully";
                }
            }
            return _projectCheck;
        }
        public List<Role> GetUserRolesByUserId(Guid userId)
        {
            using (PureSurveyProjectTrackerEntities psdb = new PureSurveyProjectTrackerEntities())
            {
                return psdb.SYSUsers
                    .Where(m => m.SYSUserID == userId)
                    .Select(m => m.Roles)
                    .FirstOrDefault().ToList();
            }
        }
        public List<Project> GetUserProjectsByUserId(Guid userId)
        {
            using (PureSurveyProjectTrackerEntities psdb = new PureSurveyProjectTrackerEntities())
            {
                var allProjects = new List<Project>();
                IQueryable<SYSUser> users = psdb.SYSUsers;
                if (!_authHelper.IsAdministrator(userId))
                {
                    users = users.Where(m => m.SYSUserID == userId);
                }

                users.Select(m => m.Projects).ToList().ForEach(m => allProjects.AddRange(m));

                return allProjects;
            }
        }
        public string SaveEditProject(EditModalView viewModel)
        //string _projectName, string _client, string _projectManager, string _developer, string _designer, string _type, string _status, string _url, string _db, string _server, string _backupPlan, double _cost, Guid _projectId
        {
            string _projectCheck = "";
            using (PureSurveyProjectTrackerEntities ptdb = new PureSurveyProjectTrackerEntities())
            {
                Project pt = ptdb.Projects.Where(x => x.Project_ID == viewModel._projectId).Distinct().FirstOrDefault();
                if (pt != null)
                {

                    pt = new Project
                    {
                        Project_ID = viewModel._projectId,
                        ProjectName = viewModel.ProjectName,
                        ClientName = viewModel.ClientName,
                        ProjectManager = viewModel.ProjectManager,
                        Developer = viewModel.Developer,
                        Designer = viewModel.Designer,
                        ProjectTypeDescription = viewModel.ProjectType,
                        Status = viewModel.Status,
                        URL = viewModel.URL,
                        ProjectDB = viewModel.ProjectDB,
                        ProjectServer = viewModel.ProjectServer,
                        BackUpPlan = viewModel.BackUp,
                        ProjectCost = viewModel.Project_Value,
                        DateCreated = DateTime.Now
                    };

                    ptdb.Projects.AddOrUpdate(pt);
                    ptdb.SaveChanges();
                }
                else
                {
                    _projectCheck = pt.Status.ToString() + "There is no such !";

                }
                
            }
            return _projectCheck;
        }  
        public List<string> getProjectStatusOptions()
        {
            List<string> _projectsStatusOptions;
            using (PureSurveyProjectTrackerEntities psdb = new PureSurveyProjectTrackerEntities())
            {
                _projectsStatusOptions = psdb.Status.Select(e => e.StatusDescription).ToList();
                _projectsStatusOptions.RemoveAll(item => item == null);
            }
            return _projectsStatusOptions;
        }
        public List<string> getProjectTypesOptions()
        {
            List<string> _projectsTypesOptions;
            using (PureSurveyProjectTrackerEntities psdb = new PureSurveyProjectTrackerEntities())
            {
                _projectsTypesOptions = psdb.Types.Select(e => e.TypeDescription).ToList();
                _projectsTypesOptions.RemoveAll(item => item == null);
            }
            return _projectsTypesOptions;
        }
        public List<string> getBackUpPlanOptions()
        {
            List<string> _backUpPlanOptions;
            using (PureSurveyProjectTrackerEntities psdb = new PureSurveyProjectTrackerEntities())
            {
                _backUpPlanOptions = psdb.BackUpPlans.Select(e => e.BackUpDescription).ToList();
                _backUpPlanOptions.RemoveAll(item => item == null);
            }
            return _backUpPlanOptions;
        }
        public List<string> getPMOptions()
        {
            List<string> _PMOptions;
            using (PureSurveyProjectTrackerEntities psdb = new PureSurveyProjectTrackerEntities())
            {
                _PMOptions = psdb.PMs.Select(e => e.PMname).ToList();
                _PMOptions.RemoveAll(item => item == null);
            }
            return _PMOptions;
        }

        public List<string> getDevsOptions()
        {
            List<string> _DevsOptions;
            using (PureSurveyProjectTrackerEntities psdb = new PureSurveyProjectTrackerEntities())
            {
                _DevsOptions = psdb.Developers.Select(e => e.DevName).ToList();
                _DevsOptions.RemoveAll(item => item == null);
            }
            return _DevsOptions;
        }

        public List<string> getDesgnsOptions()
        {
            List<string> _DesgnsOptions;
            using (PureSurveyProjectTrackerEntities psdb = new PureSurveyProjectTrackerEntities())
            {
                _DesgnsOptions = psdb.Designers.Select(e => e.DesignerName).ToList();
                _DesgnsOptions.RemoveAll(item => item == null);
            }
            return _DesgnsOptions;
        }
        public int getProjectNo()
        {
            int _projectsNo;
            using (PureSurveyProjectTrackerEntities psdb = new PureSurveyProjectTrackerEntities())
            {
                _projectsNo = psdb.Projects.Count();
            }
            return _projectsNo;
        }
    }
}