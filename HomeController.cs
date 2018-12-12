using System;
using System.Web.Security;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity.Migrations;
using System.Web;
using System.Web.Mvc;
using PureSurveyProjectTracker.Models.ViewModels;
using PureSurveyProjectTracker.Models.EntityManager;
using PureSurveyProjectTracker.Models.DB;
using System.Data.Entity.Validation;
using PureSurveyProjectTracker.Helpers;
using System.IO;

namespace PureSurveyProjectTracker.Controllers
{
   // [Authorize]
    public class HomeController : Controller
    {
        private ProjectsManager _manager;
        private AuthorisationHelper _authHelper;

        public HomeController()
        {

            _authHelper = new AuthorisationHelper();
            _manager = new ProjectsManager();
        }

        public ActionResult Index(string Id)
        {
            ProjectsModel pmd = new ProjectsModel();
            if (string.IsNullOrWhiteSpace(Id)) return RedirectToAction("Fault", "Error");
            try
            {
                Guid _Id = new Guid(Id);
                if(_Id == Guid.Empty) return RedirectToAction("Fault", "Error");
                pmd = Refresh(_Id);
            }
            catch (FormatException ex)
            {
                return RedirectToAction("Fault", "Error");
            }           
            return View(pmd);
        }

        [HttpPost]
        public ActionResult AddProject(Guid UserId, string btnAddProject, string btnCancel, string pName, string pClient, string pManager, string pDeveloper, string pDesigner, string pType, string pStatus, string pUrl, string pDb, string pServer, string pBPlan, double pCost)
        {
            //Sending new values to a model
            string ProjectName = pName;
            string ClientName = pClient;
            string ProjectManager = pManager;
            string Developer = pDeveloper;
            string Designer = pDesigner;
            string ProjectType = pType;
            string Status = pStatus;
            string URL = pUrl;
            string ProjectDB = pDb;
            string ProjectServer = pServer;
            string BackUp = pBPlan;
            double Cost = pCost;

            if (!string.IsNullOrEmpty(btnAddProject))
            {
                string _checkMessage = "";

                //storing in DB
                _checkMessage = _manager.AddProject(UserId, ProjectName, ClientName, ProjectManager, Developer, Designer, ProjectType, Status, URL, ProjectDB, ProjectServer, BackUp, Cost);
                ViewBag.Message = _checkMessage;
            }

            ProjectsModel pmd = Refresh(UserId);
            return PartialView("_tablePartial", pmd);
        }

        [HttpPost]
        public ActionResult SaveEditProject(EditModalView viewModel)
        {
            ProjectsManager pm = new ProjectsManager();
            if (!ModelState.IsValid)
            {

                viewModel.Types = pm.getProjectTypesOptions();
                viewModel.Statuses = pm.getProjectStatusOptions();
                viewModel.BackUpOptions = pm.getBackUpPlanOptions();
                viewModel.PMs = pm.getPMOptions();
                viewModel.Devs = pm.getDevsOptions();
                viewModel.Desgns = pm.getDesgnsOptions();

                return Json(new { isValid = false, view = RenderViewAsString("_EditPartial", viewModel) });
            }

            string _checkMessage = "";

            //storing in DB
            _checkMessage = pm.SaveEditProject(viewModel);
            ViewBag.Message = _checkMessage;

            ProjectsModel pmd = Refresh(viewModel.userId);

            return Json(new { isValid = true, view = RenderViewAsString("_tablePartial", pmd) });
        }

        [HttpPost]
        public ActionResult sortTable(Guid userId, SortItems _headerValue, SortDirections sortDirection)
        {
            return GetSortedProjectsModel(userId, sortDirection, _headerValue, 0, 0);
        }

        [HttpPost]
        public ActionResult searchTable(Guid userId, string _searchValue)
        {
            ProjectsModel pmd = new ProjectsModel();
            if (!string.IsNullOrEmpty(_searchValue))
            {
                using (PureSurveyProjectTrackerEntities psdb = new PureSurveyProjectTrackerEntities())
                {
                    ProjectsManager pm = new ProjectsManager();
                    //Obtaining values 
                    List<string> _TypesOptions = pm.getProjectTypesOptions();
                    List<string> _StatusesOptions = pm.getProjectStatusOptions();
                    List<string> _BakupOptions = pm.getBackUpPlanOptions();
                    List<string> _PMOptions = pm.getPMOptions();
                    List<string> _DevOptions = pm.getDevsOptions();
                    List<string> _DesgnsOptions = pm.getDesgnsOptions();

                    string searValue = _searchValue.ToUpper();

                    var prjcts = _manager.GetUserProjectsByUserId(userId).Select(x => new ProjectsModel
                    {
                        _projectId = x.Project_ID,
                        ProjectName = x.ProjectName,
                        ClientName = x.ClientName,
                        ProjectManager = x.ProjectManager,
                        PMs = _PMOptions,
                        Developer = x.Developer,
                        Devs = _DevOptions,
                        Designer = x.Designer,
                        Desgns = _DesgnsOptions,
                        ProjectType = x.ProjectTypeDescription,
                        Types = _TypesOptions,
                        Status = x.Status,
                        Statuses = _StatusesOptions,
                        URL = x.URL,
                        ProjectDB = x.ProjectDB,
                        ProjectServer = x.ProjectServer,
                        BackUp = x.BackUpPlan,
                        BackUpOptions = _BakupOptions,
                        ProjectValue = x.ProjectCost
                    }).Where(x => x.ProjectName.ToUpper().Contains(searValue)).OrderBy(x => x.ProjectName).ToList();

                    pmd = new ProjectsModel
                    {
                        UserId = userId,
                        PMs = _PMOptions,
                        Devs = _DevOptions,
                        Desgns = _DesgnsOptions,
                        Types = _TypesOptions,
                        Statuses = _StatusesOptions,
                        BackUpOptions = _BakupOptions,
                        CampaignItems = prjcts,
                        PageNo = 1,
                        ItemsPerPage = 10,
                        projectNo = prjcts.Count
                    };
                }
            }
            else
            {
                pmd = Refresh(userId);
            }
            return PartialView("_tablePartial", pmd);
        }

        [HttpPost]
        public ActionResult LoadAddViewListboxes(Guid userId)
        {
            using (PureSurveyProjectTrackerEntities psdb = new PureSurveyProjectTrackerEntities())
            {
                AddModalView pmd = new AddModalView();
                if (userId != null)
                {
                    ProjectsManager pm = new ProjectsManager();
                    //Obtaining values 
                    List<string> _TypesOptions = pm.getProjectTypesOptions();
                    List<string> _StatusesOptions = pm.getProjectStatusOptions();
                    List<string> _BakupOptions = pm.getBackUpPlanOptions();
                    List<string> _PMOptions = pm.getPMOptions();
                    List<string> _DevOptions = pm.getDevsOptions();
                    List<string> _DesgnsOptions = pm.getDesgnsOptions();
                
                    pmd = new AddModalView
                    {
                        UserId = userId,
                        Types = _TypesOptions,
                        Statuses = _StatusesOptions,
                        BackUpOptions = _BakupOptions,
                        PMs = _PMOptions,
                        Devs = _DevOptions,
                        Desgns = _DesgnsOptions
                    };
                }                
                return PartialView("_AddPartial", pmd);
            }
        }

        [HttpPost]
        public ActionResult ProjectCostView(Guid _projectId)
        {
            using (PureSurveyProjectTrackerEntities psdb = new PureSurveyProjectTrackerEntities())
            {
                Project pjcs = psdb.Projects.FirstOrDefault(x => x.Project_ID == _projectId);

                CostModalViewModel pmd = new CostModalViewModel();
                pmd = new CostModalViewModel
                {
                    _Proj_Name = pjcs.ProjectName,
                    _Proj_Client = pjcs.ClientName,
                    _Proj_Manager = pjcs.ProjectManager,
                    _Proj_Dev = pjcs.Developer,
                    _Proj_Designer = pjcs.Designer,
                    _Proj_Type = pjcs.ProjectTypeDescription,
                    _Proj_Status = pjcs.Status,
                    _Proj_URl = pjcs.URL,
                    _Proj_DB = pjcs.ProjectDB,
                    _Proj_Server = pjcs.ProjectServer,
                    _Proj_Backup_Plan = pjcs.BackUpPlan,
                    _Proj_Value = pjcs.ProjectCost,
                    _Proj_Date_Created = pjcs.DateCreated
                };
                return PartialView("_CostDetailsPartial", pmd);
            }
        }

        public ActionResult EditProjectView(Guid _projectId, Guid _userId)
        {
            using (PureSurveyProjectTrackerEntities psdb = new PureSurveyProjectTrackerEntities())
            {
                Project pjcs = psdb.Projects.FirstOrDefault(x => x.Project_ID == _projectId);

                ProjectsManager pm = new ProjectsManager();
                //Obtaining values 
                List<string> _TypesOptions = pm.getProjectTypesOptions();
                List<string> _StatusesOptions = pm.getProjectStatusOptions();
                List<string> _BakupOptions = pm.getBackUpPlanOptions();
                List<string> _PMOptions = pm.getPMOptions();
                List<string> _DevOptions = pm.getDevsOptions();
                List<string> _DesgnsOptions = pm.getDesgnsOptions();

                EditModalView pmd = new EditModalView();
                pmd = new EditModalView
                {
                    userId = _userId,
                    _projectId = _projectId,
                    ProjectName = pjcs.ProjectName,
                    ClientName = pjcs.ClientName,
                    ProjectManager = pjcs.ProjectManager,
                    PMs = _PMOptions,
                    Developer = pjcs.Developer,
                    Devs = _DevOptions,
                    Designer = pjcs.Designer,
                    Desgns = _DesgnsOptions,
                    ProjectType = pjcs.ProjectTypeDescription,
                    Types = _TypesOptions,
                    Status = pjcs.Status,
                    Statuses = _StatusesOptions,
                    URL = pjcs.URL,
                    ProjectDB = pjcs.ProjectDB,
                    ProjectServer = pjcs.ProjectServer,
                    BackUp = pjcs.BackUpPlan,
                    BackUpOptions = _BakupOptions,
                    Project_Value = pjcs.ProjectCost
                };
                return PartialView("_EditPartial", pmd);
            }
        }

        public ProjectsModel Refresh(Guid Id)
        {
            ProjectsModel projectsMod = new ProjectsModel();

            return Refresh(Id, null);
        }

        public ProjectsModel Refresh(Guid Id, ProjectsModel projectsMod)
        {
            if (projectsMod == null)
            {
                projectsMod = new ProjectsModel();
            }

            ProjectsManager pm = new ProjectsManager();
            //Obtaining values 
            List<string> _TypesOptions = pm.getProjectTypesOptions();
            List<string> _StatusesOptions = pm.getProjectStatusOptions();
            List<string> _BakupOptions = pm.getBackUpPlanOptions();
            List<string> _PMOptions = pm.getPMOptions();
            List<string> _DevOptions = pm.getDevsOptions();
            List<string> _DesgnsOptions = pm.getDesgnsOptions();
            int _projectNo = pm.getProjectNo();
            projectsMod.ItemsPerPage = projectsMod.ItemsPerPage != 0 ? projectsMod.ItemsPerPage : 10;
            var prjcts = GetProjectsFromDb(Id).OrderBy(x => x.ProjectName).ToList();

            projectsMod = new ProjectsModel
            {
                UserId = Id,
                CampaignItems =
                    prjcts
                    .OrderBy(p => p.ProjectName)
                    .Skip(((projectsMod.PageNo == 0 ? 1 : projectsMod.PageNo) - 1) * (projectsMod.ItemsPerPage == 0 ? 10 : projectsMod.ItemsPerPage))
                    .Take(projectsMod.ItemsPerPage == 0 ? 10 : projectsMod.ItemsPerPage).ToList(),
                Devs = _DevOptions,
                Desgns = _DesgnsOptions,
                PMs = _PMOptions,
                Types = _TypesOptions,
                Statuses = _StatusesOptions,
                BackUpOptions = _BakupOptions,
                projectNo = prjcts.Count,
                PageNo = 1,
                ItemsPerPage = 10,
                NoPages = prjcts.Distinct().Count() / (projectsMod.ItemsPerPage == 0 ? 10 : projectsMod.ItemsPerPage) + (prjcts.Distinct().Count() % (projectsMod.ItemsPerPage == 0 ? 10 : projectsMod.ItemsPerPage) > 0 ? 1 : 0)
            };

            return projectsMod;
        }

        [HttpPost]
        public ActionResult Updatepm(string _SelectedValue, Guid _SelectedIndex, Guid userId)
        {
            try
            {

                ProjectsModel pmd = new ProjectsModel();
                string _projectCheck = "";
                using (PureSurveyProjectTrackerEntities ptdb = new PureSurveyProjectTrackerEntities())
                {
                    Project pjct = ptdb.Projects.Where(x => x.Project_ID == _SelectedIndex).FirstOrDefault();
                    if (pjct != null)
                    {
                        pjct.ProjectManager = _SelectedValue;
                        ptdb.Projects.AddOrUpdate(pjct);
                        ptdb.SaveChanges();

                        _projectCheck = pjct.ProjectName.ToString() + "Updated Succesfully";

                    }
                    else
                    {
                        _projectCheck = pjct.Status.ToString() + "There is no such !";

                    }
                }

                pmd = Refresh(userId);
                //if (_authHelper.IsAdministrator(userId))
                //{
                //    pmd = Refresh(pmd, userId);
                //}
                //else if (_authHelper.IsNormalUser(userId))
                //{
                //    pmd = Refresh(userId);
                //}
                return PartialView("_tablePartial", pmd);
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                return null;
            }
        }

        [HttpPost]
        public ActionResult UpdateDev(string _SelectedValue, Guid _SelectedIndex, Guid userId)
        {
            ProjectsModel pmd = new ProjectsModel();
            string _projectCheck = "";
            using (PureSurveyProjectTrackerEntities ptdb = new PureSurveyProjectTrackerEntities())
            {
                Project pjct = ptdb.Projects.Where(x => x.Project_ID == _SelectedIndex).FirstOrDefault();
                if (pjct != null)
                {
                        pjct.Developer = _SelectedValue;
                        ptdb.Projects.AddOrUpdate(pjct);
                        ptdb.SaveChanges();

                        _projectCheck = pjct.ProjectName.ToString() + "Updated Succesfully";

                }
                else
                {
                    _projectCheck = pjct.Status.ToString() + "There is no such !";

                }
            }

            pmd = Refresh(userId);
            return PartialView("_tablePartial", pmd);
        }

        [HttpPost]
        public ActionResult UpdateDesgn(string _SelectedValue, Guid _SelectedIndex, Guid userId)
        {
            ProjectsModel pmd = new ProjectsModel();
            string _projectCheck = "";
            using (PureSurveyProjectTrackerEntities ptdb = new PureSurveyProjectTrackerEntities())
            {
                Project pjct = ptdb.Projects.Where(x => x.Project_ID == _SelectedIndex).FirstOrDefault();
                if (pjct != null)
                {
                    pjct.Designer = _SelectedValue;
                    ptdb.Projects.AddOrUpdate(pjct);
                    ptdb.SaveChanges();

                    _projectCheck = pjct.ProjectName.ToString() + "Updated Succesfully";

                }
                else
                {
                    _projectCheck = pjct.Status.ToString() + "There is no such !";

                }
            }

            pmd = Refresh(userId);
            return PartialView("_tablePartial", pmd);
        }

        [HttpPost]
        public ActionResult UpdateType(string _SelectedValue, Guid _SelectedIndex, Guid userId)
        {
            ProjectsModel pmd = new ProjectsModel();
            string _projectCheck = "";
            using (PureSurveyProjectTrackerEntities ptdb = new PureSurveyProjectTrackerEntities())
            {
                Project pjct = ptdb.Projects.Where(x => x.Project_ID == _SelectedIndex).FirstOrDefault();
                if (pjct != null)
                {

                    pjct.ProjectTypeDescription = _SelectedValue;
                    ptdb.Projects.AddOrUpdate(pjct);
                    ptdb.SaveChanges();

                    _projectCheck = pjct.ProjectName.ToString() + "Updated Succesfully";

                }
                else
                {
                    _projectCheck = pjct.Status.ToString() + "There is no such !";

                }
            }

            pmd = Refresh(userId);
            return PartialView("_tablePartial", pmd);
        }
        [HttpPost]
        public ActionResult UpdateStatus(string _SelectedValue, Guid _SelectedIndex, Guid userId)
        {
            ProjectsModel pmd = new ProjectsModel();
            string _projectCheck = "";
            using (PureSurveyProjectTrackerEntities ptdb = new PureSurveyProjectTrackerEntities())
            {
                Project pjct = ptdb.Projects.Where(x => x.Project_ID == _SelectedIndex).FirstOrDefault();
                if (pjct != null)
                {
                    pjct.Status = _SelectedValue;
                    ptdb.Projects.AddOrUpdate(pjct);
                    ptdb.SaveChanges();

                    _projectCheck = pjct.ProjectName.ToString() + "Updated Succesfully";

                }
                else
                {
                    _projectCheck = pjct.Status.ToString() + "There is no such !";

                }
            }
            pmd = Refresh(userId);
            return PartialView("_tablePartial", pmd);
        }

        [HttpPost]
        public ActionResult UpdateBackup(string _SelectedValue, Guid _SelectedIndex, Guid userId)
        {
            ProjectsModel pmd = new ProjectsModel();
            string _projectCheck = "";
            using (PureSurveyProjectTrackerEntities ptdb = new PureSurveyProjectTrackerEntities())
            {
                Project pjct = ptdb.Projects.Where(x => x.Project_ID == _SelectedIndex).FirstOrDefault();
                if (pjct != null)
                {

                    pjct.BackUpPlan = _SelectedValue;
                    ptdb.Projects.AddOrUpdate(pjct);
                    ptdb.SaveChanges();

                    _projectCheck = pjct.ProjectName.ToString() + "Updated Succesfully";

                }
                else
                {
                    _projectCheck = pjct.BackUpPlan.ToString() + "There is no such !";

                }
            }
            pmd = Refresh(userId);
            return PartialView("_tablePartial", pmd);
        }

        [HttpPost]
        public ActionResult Paging(int _pageNO, SortDirections _sortValue, SortItems _element, Guid _UserId)
        {
            return GetSortedProjectsModel(_UserId, _sortValue, _element, _pageNO, 0);
        }


        [HttpPost]
        public ActionResult itemsperpage(int newitemsperpage, Guid _UserId)
        {
            ProjectsModel projectsMod = new ProjectsModel();

            using (PureSurveyProjectTrackerEntities psdb = new PureSurveyProjectTrackerEntities())
            {
                ProjectsManager pm = new ProjectsManager();
                //Obtaining values 
                List<string> _TypesOptions = pm.getProjectTypesOptions();
                List<string> _StatusesOptions = pm.getProjectStatusOptions();
                List<string> _BakupOptions = pm.getBackUpPlanOptions();
                List<string> _PMOptions = pm.getPMOptions();
                List<string> _DevOptions = pm.getDevsOptions();
                List<string> _DesgnsOptions = pm.getDesgnsOptions();
                int _projectNo = pm.getProjectNo();
                projectsMod.ItemsPerPage = newitemsperpage;
                var prjcts = _manager.GetUserProjectsByUserId(_UserId).Select(x => new ProjectsModel
                {
                    projectNo = _projectNo,
                    _projectId = x.Project_ID,
                    ProjectName = x.ProjectName,
                    ClientName = x.ClientName,
                    ProjectManager = x.ProjectManager,
                    PMs = _PMOptions,
                    Developer = x.Developer,
                    Devs = _DevOptions,
                    Designer = x.Designer,
                    Desgns = _DesgnsOptions,
                    ProjectType = x.ProjectTypeDescription,
                    Types = _TypesOptions,
                    Status = x.Status,
                    Statuses = _StatusesOptions,
                    URL = x.URL,
                    ProjectDB = x.ProjectDB,
                    ProjectServer = x.ProjectServer,
                    BackUp = x.BackUpPlan,
                    BackUpOptions = _BakupOptions,
                    ProjectValue = x.ProjectCost
                }).Where(x => x.ProjectName != null).OrderBy(x => x.ProjectName).ToList();

                projectsMod = new ProjectsModel
                {
                    CampaignItems =
                        prjcts
                        .OrderBy(p => p.ProjectName)
                        .Skip(((projectsMod.PageNo == 0 ? 1 : projectsMod.PageNo) - 1) * (projectsMod.ItemsPerPage == 0 ? 10 : projectsMod.ItemsPerPage))
                        .Take(projectsMod.ItemsPerPage == 0 ? 10 : projectsMod.ItemsPerPage).ToList(),
                    PMs = _PMOptions,
                    Devs = _DevOptions,
                    Desgns = _DesgnsOptions,
                    Types = _TypesOptions,
                    Statuses = _StatusesOptions,
                    BackUpOptions = _BakupOptions,

                    projectNo = prjcts.Count,
                    PageNo = 1,
                    ItemsPerPage = 10,
                    NoPages = prjcts.Distinct().Count() / (projectsMod.ItemsPerPage == 0 ? 10 : projectsMod.ItemsPerPage) + (prjcts.Distinct().Count() % (projectsMod.ItemsPerPage == 0 ? 10 : projectsMod.ItemsPerPage) > 0 ? 1 : 0)
                };
            }

            return PartialView("_tablePartial", projectsMod);
        }

        #region - Private Methods -

        private static List<ProjectsModel> SortAndOrderProjects(SortItems _headerValue, SortDirections sortDirection, IEnumerable<ProjectsModel> prjcts)
        {
            List<ProjectsModel> OrderedProjects;
            switch (_headerValue)
            {
                default:
                    OrderedProjects = sortDirection ==
                        SortDirections.Ascending ?
                        prjcts.OrderBy(x => x.ProjectName).ToList() :
                        prjcts.OrderByDescending(x => x.ProjectName).ToList();
                    break;
                case SortItems.Client:
                    OrderedProjects = sortDirection ==
                    SortDirections.Ascending ?
                        prjcts.OrderBy(x => x.ClientName).ToList() :
                        prjcts.OrderByDescending(x => x.ClientName).ToList();
                    break;
                //=================================NOT CURRENTLY USED======================================//
                case SortItems.Manager:
                    OrderedProjects = prjcts.OrderByDescending(x => x.ProjectManager).ToList();
                    break;
                case SortItems.Developer:
                    OrderedProjects = prjcts.OrderByDescending(x => x.Developer).ToList();
                    break;
                case SortItems.Designer:
                    OrderedProjects = prjcts.OrderByDescending(x => x.Designer).ToList();
                    break;
                case SortItems.ProjectType:
                    OrderedProjects = prjcts.OrderByDescending(x => x.ProjectType).ToList();
                    break;
                case SortItems.ProjectStatus:
                    OrderedProjects = prjcts.OrderByDescending(x => x.Status).ToList();
                    break;
                case SortItems.BackupPlan:
                    OrderedProjects = prjcts.OrderByDescending(x => x.BackUp).ToList();
                    break;
                case SortItems.Value:
                    OrderedProjects = prjcts.OrderByDescending(x => x.ProjectValue).ToList();
                    break;
            }

            return OrderedProjects;
        }

        private IEnumerable<ProjectsModel> GetProjectsFromDb(Guid userId)
        {
            ProjectsManager pm = new ProjectsManager();
            int _projectNo = pm.getProjectNo();
            var prjcts = _manager.GetUserProjectsByUserId(userId).Select(x => new ProjectsModel
            {
                UserId = userId,
                projectNo = _projectNo,
                _projectId = x.Project_ID,
                ProjectName = x.ProjectName,
                ClientName = x.ClientName,
                ProjectManager = x.ProjectManager,
                Developer = x.Developer,
                Designer = x.Designer,
                ProjectType = x.ProjectTypeDescription,
                Status = x.Status,
                URL = x.URL,
                ProjectDB = x.ProjectDB,
                ProjectServer = x.ProjectServer,
                BackUp = x.BackUpPlan,
                ProjectValue = x.ProjectCost
            }).Where(x => x.ProjectName != null);
            return prjcts;
        }

        private ActionResult GetSortedProjectsModel(Guid _UserId, SortDirections _sortValue, SortItems _element, int pageNumber, int itemsPerPage)
        {
            ProjectsManager pm = new ProjectsManager();
            //Obtaining values 
            List<string> _TypesOptions = pm.getProjectTypesOptions();
            List<string> _StatusesOptions = pm.getProjectStatusOptions();
            List<string> _BakupOptions = pm.getBackUpPlanOptions();
            List<string> _PMOptions = pm.getPMOptions();
            List<string> _DevOptions = pm.getDevsOptions();
            List<string> _DesgnsOptions = pm.getDesgnsOptions();

            var prjcts = GetProjectsFromDb(_UserId);

            List<ProjectsModel> OrderedProjects = SortAndOrderProjects(_element, _sortValue, prjcts);

            var pmd = new ProjectsModel
            {
                SortItem = _element,
                SortDirection = _sortValue,
                UserId = _UserId,
                CampaignItems =
                    OrderedProjects
                    .Skip(((pageNumber == 0 ? 1 : pageNumber) - 1) * (itemsPerPage == 0 ? 10 : itemsPerPage))
                    .Take(itemsPerPage == 0 ? 10 : itemsPerPage).ToList(),
                Devs = _DevOptions,
                Desgns = _DesgnsOptions,
                PMs = _PMOptions,
                Types = _TypesOptions,
                Statuses = _StatusesOptions,
                BackUpOptions = _BakupOptions,
                projectNo = prjcts.Count(),
                PageNo = (pageNumber == 0 ? 1 : pageNumber),
                NoPages = prjcts.Distinct().Count() / (itemsPerPage == 0 ? 10 : itemsPerPage) + (prjcts.Distinct().Count() % (itemsPerPage == 0 ? 10 : itemsPerPage) > 0 ? 1 : 0)
            };
            return PartialView("_tablePartial", pmd);
        }

        private string RenderViewAsString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext,
                    viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }

        #endregion
        [HttpPost]
        public ActionResult LogOut(string id)
        {
            //ProjectsModel pmd = new ProjectsModel();
            if (string.IsNullOrWhiteSpace(id)) return RedirectToAction("Fault", "Error");
            if (id != null)
            {
                try
                {
                    Guid _Id = new Guid(id);
                    if (_Id == Guid.Empty) return RedirectToAction("Fault", "Error");
                    ProjectsModel pmod = new ProjectsModel();
                    pmod.UserId = Guid.Empty;
                    //FormsAuthentication.SignOut();
                    return RedirectToAction("LogIn", "Account");
                }
                catch (FormatException ex)
                {
                    return RedirectToAction("Fault", "Error");
                }
            }
            return View("LogIn");
            //}
            //return RedirectToAction("SignOut", "Account");
        }
    }
}
