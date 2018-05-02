using HapGp.Exceptions;
using HapGp.ModelInstance;
using HapGp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HapGp.BussinessProcessing
{
    public static class Mission
    {
        public static void CreateMission(this Userx user, int ClassID, string Name)
        {
            if (user.Infos.Role == Enums.UserRole.Student) throw new FPException("需要教师角色");
            var db = new AppDbContext();
            var stds = (from t in db.M_ProjectSelectModels
                        where t.ProjectID == ClassID
                        select t.StudentID).ToList();
            foreach (var t in stds)
            {
                var model = new MissionModel()
                {
                    StudentID = t,
                    TeacherID = user.Origin.ID,
                    Name = Name,
                    IsFinished = false
                };
                db.Entry(model).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            }
            db.SaveChanges();
        }

        public static void FinishMission(this Userx user, int MissionID)
        {
            var db = new AppDbContext();
            var model = db.M_MissionModel.Find(MissionID);
            if (model != null)
            {
                model.IsFinished = true;
            }
            db.Entry(model).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            db.SaveChanges();
        }

        public static IEnumerable<MissionModel> QueryMissions(this Userx user)
        {
            if (user.Infos.Role == Enums.UserRole.Student)
            {
                var db = new AppDbContext();
                return (from t in db.M_MissionModel
                        where t.StudentID == user.Origin.ID && t.IsFinished==false
                        select t).ToList();
            }
            else if (user.Infos.Role == Enums.UserRole.Teacher)
            {
                var db = new AppDbContext();
                return (from t in db.M_MissionModel
                        where t.TeacherID == user.Origin.ID && t.IsFinished==false
                        select t).ToList();
            }
            else
            {
                return null;
            }
        }

        public static Dictionary<string,object> ConvertMission(this MissionModel model)
        {
            return new Dictionary<string, object>
            {
                {"MissionID",model.ID },
                {"TeacherID",model.TeacherID },
                {"StudentID",model.StudentID },
                {"Name",model.Name },
                {"IsFinish",model.IsFinished }
            };
        }
    }
}
