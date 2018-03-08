using System;
using System.Collections.Generic;
using HapGp.Models;

namespace HapGp.Core
{
    public partial class FrameCorex
    {
        internal class ServerUtil
        {
            internal static Dictionary<string, object> LocalServerState(ServiceInstance server)
            {
                var db = new AppDbContext();
                db.Database.EnsureCreated();
                return new Dictionary<string, object>(){
                    {"ServerInstance",new Dictionary<string, object>{
                        { "Total", _AvaServiceInstances.Count + _ServiceInstances.Count },
                        { "Avaliable", _AvaServiceInstances.Count  },
                        { "Current", + _ServiceInstances.Count },
                        { "Interupted Container", _IntServiceInstancesInfos.Count  }}
                    },
                    {"DbState",new Dictionary<string, object>{
                        {"User Count", db.M_UserModels.Local.Count},
                        {"User Record Count", db.M_UserRecordModels.Local.Count}
                    }}
                };
            }

        }

    }
}
