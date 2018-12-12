using System;
using System.Linq;
using PureSurveyProjectTracker.Models.DB;
using PureSurveyProjectTracker.Models.ViewModels;
using PureSurveyProjectTracker.Models.EntityManager;

namespace PureSurveyProjectTracker.Models.EntityManager
{
    public class UserManager
    {
        public void AddUserAccount(UserSignUpView user)
        {

            using (PureSurveyProjectTrackerEntities db = new PureSurveyProjectTrackerEntities())
            {

                SYSUser SU = new SYSUser();           
                SU.SYSUserID = Guid.NewGuid();
                user.SYSUserID = SU.SYSUserID;
                SU.LoginName = user.LoginName;
                SU.PasswordEncryptedText = user.Password;
                //SU.RowCreatedSYSUserID = user.SYSUserID > 0 ? user.SYSUserID : 1;
                SU.RowCreatedSYSUserID = user.SYSUserID;
                // SU.RowModifiedSYSUserID = user.SYSUserID > 0 ? user.SYSUserID : 1; ;
                SU.RowModifiedSYSUserID = user.SYSUserID;
                SU.RowCreatedDateTime = DateTime.Now;
                SU.RowModifiedDateTime = DateTime.Now;

                db.SYSUsers.Add(SU);
                db.SaveChanges();

                SYSUserProfile SUP = new SYSUserProfile();
                SUP.SYSUSERProfileID = Guid.NewGuid();
                SUP.SYSUserID = SU.SYSUserID;
                SUP.FirstName = user.FirstName;
                SUP.LastName = user.LastName;
                SUP.Gender = user.Gender;
                SUP.RowCreatedSYSUserID = user.SYSUserID;
                SUP.RowModifiedSYSUserID = user.SYSUserID;
                SUP.RowCreatedDateTime = DateTime.Now;
                SUP.RowModifiedDateTime = DateTime.Now;

                db.SYSUserProfiles.Add(SUP);
                db.SaveChanges();
            }
        }

        public SYSUser GetUserFromDatabase(string loginName)
        {
            using (PureSurveyProjectTrackerEntities db = new PureSurveyProjectTrackerEntities())
            {
                return db.SYSUsers.FirstOrDefault(o => o.LoginName.Equals(loginName));
            }
        }

        public bool IsLoginNameExist(string loginName)
        {
            using (PureSurveyProjectTrackerEntities db = new PureSurveyProjectTrackerEntities())
            {
                return db.SYSUsers.Where(o => o.LoginName.Equals(loginName)).Any();
            }
        }
        public string GetUserPassword(string loginName)
        {
            using (PureSurveyProjectTrackerEntities db = new PureSurveyProjectTrackerEntities())
            {
                var user = db.SYSUsers.Where(o => o.LoginName.ToLower().Equals(loginName));
                if (user.Any())
                    return user.FirstOrDefault().PasswordEncryptedText;
                else
                    return string.Empty;
            }
        }
    }
}