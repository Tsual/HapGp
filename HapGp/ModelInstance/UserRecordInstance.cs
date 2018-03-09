using HapGp.Core;
using HapGp.Exceptions;
using HapGp.Interfaces;
using HapGp.Models;
using System.Linq;
using System;

namespace HapGp.ModelInstance
{
    /// <summary>
    /// 
    /// </summary>
    public class UserRecordInstance : IUserRecordInstance
    {
        private string _LID
        {
            get => userx.Origin.LID;
        }
        private Userx userx;
        private AppDbContext db = new AppDbContext();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Instance"></param>
        internal UserRecordInstance(ServiceInstance Instance)
        {
            userx = FrameCorex.ServiceInstanceInfo(Instance).User;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="User"></param>
        internal UserRecordInstance(Userx User)
        {
            if (!FrameCorex.UserHelper._CheckUserLogin(User))
                throw new UserNotLoginException() { User = User };
            userx = User;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private string _GetRecord(string key)
        {
            var lis = (from t in db.M_UserRecordModels
                       where t.LID == _LID && t.Key == key
                       select t).ToArray();
            if (lis.Length == 1) return lis[0].Value;
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        private void _SetRecord(string key, string value)
        {
            var lis = (from t in db.M_UserRecordModels
                       where t.LID == _LID && t.Key == key
                       select t).ToArray();
            if (lis.Length == 1)
            {
                var ins = lis[0];
                ins.Value = value;
                db.Entry(ins).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            }
            else
            {
                var ins = new UserRecordModel()
                {
                    LID = _LID,
                    Key = key,
                    Value = value
                };
                db.Entry(ins).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            }
            db.SaveChanges();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        public void Delete(string key)
        {
            var lis = (from t in db.M_UserRecordModels
                       where t.LID == _LID && t.Key == key
                       select t).ToArray();
            if (lis.Length == 0) throw new UserRecordNotFindException();
            db.Entry(lis[0]).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
            db.SaveChanges();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string this[string key]
        {
            get
            {
                return _GetRecord(key);
            }
            set
            {
                _SetRecord(key, value);
            }
        }

    }

}
