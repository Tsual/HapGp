using System.Xml.Serialization;
using System.IO;
using System.Text;
using HapGp.Models;
using HapGp.Helper;
using HapGp.Interfaces;
using System;
using System.Diagnostics;
using System.Linq;
using HapGp.Exceptions;
using System.Collections;
using System.Collections.Generic;

namespace HapGp.ModelInstance
{
    public partial class Userx
    {
        public AppDbContext db = new AppDbContext();
        public static string HashOripwd(string LID, string PWD_ori) => new HashProvider().Hash(LID + PWD_ori);


        public static implicit operator UserModel(Userx obj)
        {
            UserModel res = obj._Origin;
            using (var ms = new MemoryStream())
            {
                var serobj = new XmlSerializer(typeof(Info));
                serobj.Serialize(ms, obj._Infos);
                res.EXT = Encoding.UTF8.GetString(ms.ToArray());
            }
            return res;
        }

        public static implicit operator Userx(UserModel obj)
        {
            Userx res = new Userx();
            if (obj.EXT != "" && obj.EXT != null)
            {
                using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(obj.EXT)))
                {
                    var serobj = new XmlSerializer(typeof(Info));
                    var target = serobj.Deserialize(ms) as Info;
                    res._Infos = target ?? new Info();
                }
            }
            else
                res._Infos = new Info();
            res._Origin = obj;
            Debug.WriteLine("User Instance Active!<<LID : " + obj.LID);
            return res;
        }

        #region 密钥类
        private class UserEncryptor : IEncryptor
        {
            private Userx User;

            private AESProvider AESobj
            {
                get
                {
                    if (_AESobj == null)
                    {
                        var ivhash = new HashProvider(HashProvider.HashAlgorithms.MD5);
                        byte[] _iv = ivhash.Hashbytes(User.Origin.LID);

                        string ranstr = AssetsController.getLocalSequenceString(User.Origin.ID);
                        string kstr1 = ranstr + User.Origin.LID ;

                        var keyhash = new HashProvider();
                        byte[] _key = new byte[32];
                        byte[] btar = keyhash.Hashbytes(kstr1);
                        Array.Copy(btar, 0, _key, 0, 32);
                        _AESobj = new AESProvider(_iv, _key);

                    }


                    return _AESobj;
                }
            }
            AESProvider _AESobj;

            internal UserEncryptor(Userx User)
            {
                this.User = User;
            }

            public string Decrypt(string metaStr)
            {
                return AESobj.Decrypt(metaStr);
            }

            public string Encrypt(string metaStr)
            {
                return AESobj.Encrypt(metaStr); ;
            }
        }

        [XmlIgnore]
        UserEncryptor _Encryptor;
        [XmlIgnore]
        public IEncryptor Encryptor
        {
            get
            {
                if (_Encryptor == null)
                    _Encryptor = new UserEncryptor(this);
                return _Encryptor;
            }
        }
        #endregion
        #region 原始对象
        [XmlIgnore]
        private UserModel _Origin;

        [XmlIgnore]
        public UserModel Origin { get => _Origin; private set => _Origin = value; }

        #endregion
        #region 信息
        [XmlIgnore]
        private Info _Infos;

        public Info Infos { get => _Infos; set => _Infos = value; }
        #endregion
        #region 课程表
        public void SetProject(int? ProjectID, string ProjectName, string Subtitle, DateTime? StartTime, DateTime? EndTime,DayOfWeek dayofWeek,double west,double south,double north,double east)
        {
            if (Infos.Role != Enums.UserRole.Teacher)
            {
                throw new UserRoleException()
                {
                    CurRole = Infos.Role,
                    ReqRole = Enums.UserRole.Teacher,
                    User = this
                };
            }

            if (ProjectName == null || ProjectName == "") throw new FPException("课程名称不能为空 ");

            if (ProjectID == null)
            {
                if ((from t in db.M_ProjectModels
                     where t.ProjectName == ProjectName
                     select t).ToList().Count > 0)
                    throw new FPException("课程名称已经存在");

                db.Entry(new ProjectModel()
                {
                    StartTime = StartTime.Value,
                    EndTime = EndTime.Value,
                    ProjectName = ProjectName,
                    Subtitle = Subtitle,
                    TeacherID = _Origin.ID,
                    DayofWeek=dayofWeek,
                    SiEast=east,
                    SiNorth=north,
                    SiSouth=south,
                    SiWest=west
                }).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                db.Database.EnsureCreated();
                db.SaveChanges();
            }
            else
            {
                var obj = db.M_ProjectModels.Find(ProjectID.Value);
                if (obj == null) throw new FPException("课程不存在");
                obj.ProjectName = ProjectName;
                obj.Subtitle = Subtitle;
                obj.StartTime = StartTime.Value;
                obj.EndTime = EndTime.Value;
                obj.DayofWeek = dayofWeek;
                obj.SiWest = west;
                obj.SiSouth = north;
                obj.SiEast = east;
                obj.SiNorth = north;
                db.Entry(obj).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                db.Database.EnsureCreated();
                db.SaveChanges();
            }

        }
        public void DeleteProject(int ProjectID)
        {
            if (Infos.Role != Enums.UserRole.Teacher)
            {
                throw new UserRoleException()
                {
                    CurRole = Infos.Role,
                    ReqRole = Enums.UserRole.Teacher,
                    User = this
                };
            }
            try
            {
                db.M_ProjectModels.Remove(db.M_ProjectModels.Find(ProjectID));
                db.Database.EnsureCreated();
                db.SaveChanges();
            }
            catch(Exception)
            {

            }
        }

        public void SelectProject(string ProjectName)
        {
            var reslist = (from t in db.M_ProjectModels
                           where t.ProjectName == ProjectName
                           select t).ToList();
            if (reslist.Count > 0)
                SelectProject(reslist[0].Key);
            else
                throw new FPException("制定课程不存在");
        }

        public void SelectProject(int ProjectID)
        {
            if (Infos.Role != Enums.UserRole.Student)
            {
                throw new UserRoleException()
                {
                    CurRole = Infos.Role,
                    ReqRole = Enums.UserRole.Student,
                    User = this
                };
            }

            if ((from t in db.M_ProjectSelectModels
                 where t.ProjectID == ProjectID
                 && t.StudentID == _Origin.ID
                 select t).ToList().Count > 0)
                throw new ProjectAlreadySelectException()
                {
                    ProjectID = ProjectID,
                    User = this
                };

            var tarclass
                = db.M_ProjectModels.Find(ProjectID);

            if (tarclass == null)
                throw new FPException("所选课程不存在");

            var pids = (from t in db.M_ProjectSelectModels
                        where t.StudentID == _Origin.ID
                        select t.ProjectID).ToList();

            foreach (var t in pids)
            {
                var ins = db.M_ProjectModels.Find(t);
                if (ins.DayofWeek == DateTime.Now.DayOfWeek)
                {
                    if (!(DateTime.Parse(ins.StartTime.Value.ToShortTimeString()) > DateTime.Parse(tarclass.EndTime.Value.ToShortTimeString())) ||
                            DateTime.Parse(ins.EndTime.Value.ToShortTimeString()) < DateTime.Parse(tarclass.StartTime.Value.ToShortTimeString()))
                    {
                        throw new FPException("该时间段已经有课(" + ins.ProjectName + ")啦");
                    }
                }
            }

            //if ((from t in db.M_ProjectSelectModels
            //     where db.M_ProjectModels.Find(t.ProjectID).DayofWeek == DateTime.Now.DayOfWeek
            //     && !(DateTime.Parse(db.M_ProjectModels.Find(t.ProjectID).StartTime.Value.ToShortTimeString()) > DateTime.Parse(tarclass.EndTime.Value.ToShortTimeString())) ||
            //     DateTime.Parse(db.M_ProjectModels.Find(t.ProjectID).EndTime.Value.ToShortTimeString()) < DateTime.Parse(tarclass.StartTime.Value.ToShortTimeString())
            //     select t).ToList().Count > 0)
            //    throw new FPException("该时间段已经有课啦");

            db.Entry(new ProjectSelectModel() { ProjectID = ProjectID, StudentID = _Origin.ID }).State
                = Microsoft.EntityFrameworkCore.EntityState.Added;
            db.Database.EnsureCreated();
            db.SaveChanges();


        }

        public ProjectModel GetProjectInfo(string ProjectName)
        {
            var reslis = (from t in db.M_ProjectModels
                          where ProjectName == t.ProjectName
                          select t).ToList();
            if (reslis.Count > 0) return reslis[0];
            return null;
        }

        public ProjectModel GetProjectInfo(int? ProjectID, string ProjectName, string Subtitle, DateTime StartTime, DateTime EndTime)
        {
            var reslis = (from t in db.M_ProjectModels
                          where ProjectID == null ? true : ProjectID.Value == t.Key
                          && ProjectName == null ? true : ProjectName == t.ProjectName
                          && Subtitle == null ? true : Subtitle == t.Subtitle
                          && StartTime == null ? true : DateTime.Now.CompareTo(StartTime) < 0
                           && EndTime == null ? true : DateTime.Now.CompareTo(EndTime) > 0
                          select t).ToList();
            if (reslis.Count > 0) return reslis[0];
            return null;
        }

        public IEnumerable<ProjectModel> QueryClass()
        {
            if (Infos.Role != Enums.UserRole.Student)
            {
                throw new UserRoleException()
                {
                    CurRole = Infos.Role,
                    ReqRole = Enums.UserRole.Student,
                    User = this
                };
            }

            var splist = (from t in db.M_ProjectSelectModels
                          where t.StudentID == _Origin.ID
                          select t.ProjectID).ToList();

            return (
                 from t in db.M_ProjectModels
                 where splist.Contains(t.Key)
                 select t).ToList();


        }
        #endregion
        #region 签到
        public void SignIn(double x,double y,int studentid)
        {

            //时间段
            var cis = (from t in db.M_ProjectSelectModels
                       where t.StudentID == studentid
                       select t.ProjectID).ToList();

            var scis = (from t in db.M_ProjectModels
                        where t.DayofWeek == DateTime.Now.DayOfWeek
                        && cis.Contains(t.Key)
                        && DateTime.Parse(t.StartTime.Value.ToShortTimeString()) < DateTime.Now
                        && DateTime.Parse(t.EndTime.Value.ToShortTimeString()) > DateTime.Now
                        select t).ToList();

            if (scis.Count < 1) throw new FPException("学生在这个时间段并没有课程");

            if (x != 0 || y != 0)
            {
                var ins = scis[0];
                if (ins.SiEast != 0 || ins.SiSouth != 0 || ins.SiNorth != 0 || ins.SiWest != 0)
                {
                    if (!(x < ins.SiEast
                    && x > ins.SiWest
                    && y < ins.SiNorth
                    && y > ins.SiSouth))
                        throw new FPException("签到地点误差较大，请检查");
                }
                else
                {
                    throw new ClassLocationMissing() { User = this, Project1 = ins };
                }
            }

            

            db.Entry(new SigninModel() { ProjectID = scis[0].Key, StudentID = studentid, Time = DateTime.Now }).State =
                Microsoft.EntityFrameworkCore.EntityState.Added;
            db.SaveChanges();



        }

        public Dictionary<string, Dictionary<string,List<string>>> querySignIn(int projectid)
        {
            if (Infos.Role != Enums.UserRole.Teacher)
            {
                throw new UserRoleException()
                {
                    CurRole = Infos.Role,
                    ReqRole = Enums.UserRole.Teacher,
                    User = this
                };
            }
            Dictionary<string, Dictionary<string, List<string>>> res = new Dictionary<string, Dictionary<string, List<string>>>();

            var projs = projectid == 0 ? (from t in db.M_ProjectModels
                                          where t.TeacherID == _Origin.ID
                                          select t).ToList() : new List<ProjectModel>() { db.M_ProjectModels.Find(projectid) };

            foreach (var  proj in projs)
            {
                var plist = (from t in db.M_ProjectSelectModels
                             where t.ProjectID == proj.Key
                             select t.StudentID).ToList();

                var signlist = (from t in db.SigninModel
                                where t.ProjectID == proj.Key
                                &&t.Time.Day==DateTime.Now.Day
                                select t.StudentID).ToList();

                List<string> signinlist = new List<string>();
                foreach (var t in signlist)
                    signinlist.Add(((Userx)db.M_UserModels.Find(t)).Infos.Name);

                List<string> usigninlist = new List<string>();
                foreach(var t in plist)
                    if(!signlist.Contains(t))
                        usigninlist.Add(((Userx)db.M_UserModels.Find(t)).Infos.Name);
                res.Add("课程[" + proj.ProjectName + "]签到情况", new Dictionary<string, List<string>>() { { "已签到", signinlist }, { "未签到", usigninlist } });
            }
            return res;
        }

        public void TeacherSignIn(IEnumerable<string> names)
        {
            if (Infos.Role != Enums.UserRole.Teacher)
            {
                throw new UserRoleException()
                {
                    CurRole = Infos.Role,
                    ReqRole = Enums.UserRole.Teacher,
                    User = this
                };
            }
            var ids = (from t in db.M_UserModels
                       where names.Contains(((Userx)t).Infos.Name)
                       select t.ID);

            foreach(var t in ids)
                this.SignIn(0, 0, t);
        }
        #endregion

        public void SaveInfos()
        {
            AppDbContext db = new AppDbContext();
            db.Entry((UserModel)this).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            db.Database.EnsureCreated();
            db.SaveChanges();
        }

        [XmlIgnore]
        private IUserRecordInstance _Records;

        [XmlIgnore]
        public IUserRecordInstance Records
        {
            get
            {
                if (_Records == null)
                    _Records = new UserRecordInstance(this);
                return _Records;
            }
        }


    }

}
