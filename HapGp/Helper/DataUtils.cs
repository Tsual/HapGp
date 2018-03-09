using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HapGp.Helper
{
    public static class DataUtils
    {
        public static double FromStringToDouble(string str)
        {
            if (str == null || str == "") return 0;
            else return Convert.ToDouble(str);
        }
    }
}
