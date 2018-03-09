using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HapGp.Helper
{
    public static class ExtendFunctionCollection
    {
        public static string Get(this Dictionary<string, string> dic,string key)
        {
            return dic.ContainsKey(key) ? dic[key] : null;
        }


    }
}
