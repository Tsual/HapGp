using HapGp.Exceptions;
using HapGp.ModelInstance;
using HapGp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HapGp.BussinessProcessing
{
    public static class Leave
    {
        public static void RequestLeave(this Userx user, int ClassSelectID)
        {
            var db = new AppDbContext();

            if (db.M_ProjectSelectModels.Find(ClassSelectID) == null)
                throw new FPException("请假失败");

            if ((from t in db.M_LeaveModle
                 where t.ClassID == ClassSelectID && t.StudentID == user.Origin.ID
                 select 1).Count() > 0)
                throw new FPException("已经请假");

            var model = new LeaveModel()
            {
                StudentID = user.Origin.ID,
                ClassID = ClassSelectID,
                IsApproved = false
            };
            db.Entry(model).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            db.SaveChanges();
        }

        public static void ApproveLeave(this Userx user, int LeaveID)
        {
            var db = new AppDbContext();
            var models = (from t in db.M_LeaveModle
                          where t.ID == LeaveID && t.IsApproved==false
                          select t).ToList();
            if (models.Count != 1) return;
            models[0].IsApproved = true;
            models[0].ApprovedTeacherID = user.Origin.ID;
            db.Entry(models[0]).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            db.SaveChanges();
        }

        public static void CancelLeave(this Userx user, int LeaveID)
        {
            var db = new AppDbContext();
            var models = (from t in db.M_LeaveModle
                          where t.ID == LeaveID
                          select t).ToList();
            if (models.Count != 1) throw new FPException("撤销失败：未找到请假单");
            db.Entry(models[0]).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
            db.SaveChanges();
        }

        public static IEnumerable<LeaveModel> QueryLeave(this Userx user)
        {
            if (user.Infos.Role == Enums.UserRole.Student)
            {
                return (from t in (new AppDbContext()).M_LeaveModle
                        where t.StudentID == user.Origin.ID && t.IsApproved==false
                        select t).ToList();
            }
            else if (user.Infos.Role == Enums.UserRole.Teacher)
            {
                var db = new AppDbContext();
                var classs = (from t in db.M_ProjectModels
                                    where t.TeacherID == user.Origin.ID
                                    select t.Key).ToList();
                var classselect = (from t in db.M_ProjectSelectModels
                                   where classs.Contains(t.ProjectID)
                                   select t.Key).ToList();
                return (from t in (new AppDbContext()).M_LeaveModle
                        where classselect.Contains(t.ClassID) && t.IsApproved==false
                        select t).ToList();
            }
            throw new FPException("请假单查询失败");
        }

        public static Dictionary<string,object> ConvertLeave(this LeaveModel leave)
        {
            return new Dictionary<string, object>
            {
                {"LeaveID",leave.ID },
                {"StudentID",leave.StudentID },
                {"TeacherID",leave.TeacherID },
                {"ClassID",leave.ClassID }
            };
        }
    }
}
