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
        public void SetProject(int? ProjectID, string ProjectName, string Subtitle, DateTime StartTime, DateTime EndTime)
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
                db.Entry(new ProjectModel()
                {
                    StartTime = StartTime,
                    EndTime = EndTime,
                    ProjectName = ProjectName,
                    Subtitle = Subtitle,
                    TeacherID = _Origin.ID
                }).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                db.SaveChanges();
            }
            else
            {
                var obj = db.M_ProjectModels.Find(ProjectID.Value);
                if (obj == null) throw new FPException("课程不存在");
                obj.ProjectName = ProjectName;
                obj.Subtitle = Subtitle;
                obj.StartTime = StartTime;
                obj.EndTime = EndTime;
                db.Entry(obj).State = Microsoft.EntityFrameworkCore.EntityState.Added;
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
                db.SaveChanges();
            }
            catch(Exception)
            {

            }
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

            db.Entry(new ProjectSelectModel() { ProjectID = ProjectID, StudentID = _Origin.ID }).State
                = Microsoft.EntityFrameworkCore.EntityState.Added;
            db.SaveChanges();


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

        public void SaveInfos()
        {
            AppDbContext db = new AppDbContext();
            db.Entry((UserModel)this).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
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
