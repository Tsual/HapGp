using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HapGp.ModelInstance;
using HapGp.Models;


namespace HapGp.BussinessProcessing
{
    public static class SchoolBus
    {
        public static IEnumerable<SchoolBusModel> GetAllSchoolBus(this Userx user)
        {
            return (from t in (new AppDbContext()).M_SchoolBusModel
                    select t).ToList();
        }

        public static void AddSchoolBus(this Userx user)
        {

        }
    }
}
