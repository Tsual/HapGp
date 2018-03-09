using HapGp.Models;
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

        //{ "课程ID", tarclass.Key }, { "课程名称", tarclass.ProjectName }, { "课程描述", tarclass.Subtitle }, { "课程开始时间", tarclass.StartTime.Value.ToShortTimeString() }, { "课程结束时间", tarclass.EndTime.Value.ToShortTimeString() }
        public static IEnumerable<Dictionary<string,string>> ConvertAPI(this IEnumerable<ProjectModel> obj)
        {
            List<Dictionary<string, string>> res = new List<Dictionary<string, string>>();
            Dictionary<string, string> temp ;
            foreach (var tarclass in obj)
            {
                temp = new Dictionary<string, string>();
                temp.Add("课程ID", tarclass.Key.ToString());
                temp.Add("课程日程", tarclass.DayofWeek.ToString());
                temp.Add("课程名称", tarclass.ProjectName);
                temp.Add("课程描述", tarclass.Subtitle);
                temp.Add("课程开始时间", tarclass.StartTime.Value.ToShortTimeString());
                temp.Add("课程结束时间", tarclass.EndTime.Value.ToShortTimeString());
                res.Add(temp);
            }
            return res;
        }

    }
}
