using HapGp.Core;
using HapGp.Helper;
using HapGp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HapGp.ModelInstance
{
    public class AssetsController
    {
        public static string getLocalSequenceString(int id)
        {
            using (AppDbContext db = new AppDbContext())
            {
                var targ = (from t in db.M_StorageModels
                            where t.Key == "SA" + id
                            select t).ToList();
                if (targ.Count > 0)
                    return targ[0].Value;
                else
                {
                    int _Count = Convert.ToInt32(FrameCorex.Config[Enums.AppConfigEnum.RandomStringCount]);
                    int _Increment = ((id - _Count) / 100 + 1) * 100;
                    var rans = new RandomGenerator();
                    for (int i = _Count; i < _Increment + _Count; i++)
                        db.M_StorageModels.Add(new StorageModel()
                        {
                            Key = "SA" + i,
                            Value = rans.getRandomString(20)
                        });
                    db.SaveChanges();
                    FrameCorex.Config[Enums.AppConfigEnum.RandomStringCount] = (_Count + _Increment).ToString();
                    return getLocalSequenceString(id);
                }
            }
        }
    }
}
