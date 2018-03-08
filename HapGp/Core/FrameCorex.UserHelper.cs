using HapGp.ModelInstance;
using HapGp.Models;
using System.Linq;
using System.Collections.Generic;

namespace HapGp.Core
{
    public partial class FrameCorex
    {
        public class UserHelper
        {

            /// <summary>
            /// require Admin+
            /// </summary>
            /// <param name="server"></param>
            /// <returns></returns>
            public static Dictionary<string, object> LocalServerState(ServiceInstance server)
            {
                if ((int)ServiceInstanceInfo(server).User.Infos.UserPermission < 2) return null;
                return ServerUtil.LocalServerState(server);
            }


            /// <summary>
            /// 
            /// </summary>
            public static void _CheckCreateDeaultUser()
            {
                AppDbContext db = new AppDbContext();
                var guest = (from t in db.M_UserModels
                             where t.LID == "Guest"
                             select t).ToArray();
                if (guest.Length == 0)
                {
                    string PWD_ori_hash = Userx.HashOripwd("Guest", "Guest");
                    string PWD_ori_hash_aes = CurrnetAppEncryptor.Encrypt(PWD_ori_hash);
                    Userx um = new UserModel()
                    {
                        LID = "Guest",
                        PWD = PWD_ori_hash_aes
                    };
                    um.Infos.UserPermission = Enums.Permission.Guest;
                    db.Entry((UserModel)um).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                }
                else
                {
                    Userx um = guest[0];
                    if (um.Infos.UserPermission != Enums.Permission.Guest)
                    {
                        um.Infos.UserPermission = Enums.Permission.Guest;
                        db.Entry((UserModel)um).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    }
                }

                var root = (from t in db.M_UserModels
                            where t.LID == "Root"
                            select t).ToArray();
                if (root.Length == 0)
                {

                    string PWD_ori_hash = Userx.HashOripwd("Root", "Root");
                    string PWD_ori_hash_aes = CurrnetAppEncryptor.Encrypt(PWD_ori_hash);
                    Userx um = new UserModel()
                    {
                        LID = "Root",
                        PWD = PWD_ori_hash_aes
                    };
                    um.Infos.UserPermission = Enums.Permission.root;
                    db.Entry((UserModel)um).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                }
                else
                {
                    Userx um = root[0];
                    if (um.Infos.UserPermission != Enums.Permission.root)
                    {
                        um.Infos.UserPermission = Enums.Permission.root;
                        db.Entry((UserModel)um).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    }
                }

                var admin = (from t in db.M_UserModels
                             where t.LID == "Admin"
                             select t).ToArray();
                if (admin.Length == 0)
                {

                    string PWD_ori_hash = Userx.HashOripwd("Admin", "Admin");
                    string PWD_ori_hash_aes = CurrnetAppEncryptor.Encrypt(PWD_ori_hash);
                    Userx um = new UserModel()
                    {
                        LID = "Admin",
                        PWD = PWD_ori_hash_aes
                    };
                    um.Infos.UserPermission = Enums.Permission.Administor;
                    db.Entry((UserModel)um).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                }
                else
                {
                    Userx um = admin[0];
                    if (um.Infos.UserPermission != Enums.Permission.Administor)
                    {
                        um.Infos.UserPermission = Enums.Permission.Administor;
                        db.Entry((UserModel)um).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    }
                }

                db.SaveChanges();
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="LID"></param>
            /// <param name="PWD"></param>
            /// <returns></returns>
            internal static bool _CheckLIDPWD(string LID, string PWD)
            {
                AppDbContext db = new AppDbContext();
                string PWD_ori_hash = Userx.HashOripwd(LID, PWD);
                string PWD_ori_hash_aes = CurrnetAppEncryptor.Encrypt(PWD_ori_hash);
                var user = (from t in db.M_UserModels
                            where t.LID == LID && t.PWD == PWD_ori_hash_aes
                            select t).ToArray();
                return user.Length == 1;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="User"></param>
            /// <returns></returns>
            internal static bool _CheckUserLogin(Userx User) => (from t in _ServiceInstances.Values
                                                                where t.User == User
                                                                select t).ToArray().Length > 0;

            /// <summary>
            /// 
            /// </summary>
            /// <param name="User"></param>
            /// <returns></returns>
            internal static string _FindEncryptToken(Userx User) => _CheckUserLogin(User) ? (from t in _ServiceInstances.Values
                                                                                                       where t.User == User
                                                                                                       select t).ToList()[0].EncryptToken : null;




        }

    }
}
