using HapGp.Enums;
using HapGp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HapGp.ModelInstance
{

    public class AppConfigs : IAppConfigs
    {
        private AppConfigs()
        {

        }

        private static AppConfigs _Current = new AppConfigs();
        public static AppConfigs Current { get => _Current; set => _Current = value; }

        public string this[AppConfigEnum key]
        {
            get
            {
                return _QueryConfig(key.ToString());
            }
            set
            {
                _SaveConfig(key.ToString(), value);
            }
        }

        private string _QueryConfig(string key)
        {
            if (key == "" || key == null) return "";
            Models.AppDbContext db = new Models.AppDbContext();
            db.Database.EnsureCreated();
            var items = (from t in db.M_AppConfigModels
                        where t.Key == key
                        select t).ToList();
            return items.Count() > 0 ? items.ElementAt(0).Value : "";
        }

        private void _SaveConfig(string key, string value)
        {
            if (key == "" || key == null) return;
            Models.AppDbContext db = new Models.AppDbContext();
            db.Database.EnsureCreated();
            var items = (from t in db.M_AppConfigModels
                        where t.Key == key
                        select t).ToList();
            if (items.Count() > 0)
            {
                items.ElementAt(0).Value = value;
                db.Entry(items.ElementAt(0)).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            }
            else
            {
                var t = new Models.AppConfigModel()
                {
                    Key = key,
                    Value = value
                };
                db.Entry(t).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            }
            db.SaveChanges();
        }

        public bool ContainsKey(AppConfigEnum Key)
        {
            string key = Key.ToString();
            if (key == "" || key == null) return false;
            Models.AppDbContext db = new Models.AppDbContext();
            db.Database.EnsureCreated();
            var items = (from t in db.M_AppConfigModels
                         where t.Key == key
                         select t).ToList();
            return items.Count() > 0;
        }

    }
}
