using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HapGp.Enums;
using HapGp.Interfaces;
using HapGp.Attribute;

namespace HapGp.APIModel
{
    public class PostInparamModel: IAPIModel
    {
        public string LID { get; set; }
        public string PWD { get; set; }
        public string Token { get; set; }
        public APIOperation Operation { get; set; }
        public Dictionary<string,string> Params { get; set; }

        public bool InparamCheck()
        {
            if ((Token == null || Token == "") && (LID == null || LID == "" || PWD == null || PWD == "")) return false;
            if (Operation == APIOperation.None) return false;
            return true;
        }
    }
    public class AdminPostInparamModel : IAPIModel
    {
        public string LID { get; set; }
        public string PWD { get; set; }
        public string Token { get; set; }
        public AdminAPIOperation Operation { get; set; }
        public Dictionary<string, string> Params { get; set; }

        public bool InparamCheck()
        {
            if ((Token == null || Token == "") && (LID == null || LID == "" || PWD == null || PWD == "")) return false;
            if (Operation == AdminAPIOperation.None) return false;
            return true;
        }
    }

}
