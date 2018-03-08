using HapGp.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace HapGp.Attribute
{
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    sealed class AppConfigDefaultAttribute : System.Attribute
    {
        // See the attribute guidelines at 
        //  http://go.microsoft.com/fwlink/?LinkId=85236
        readonly string _DefaultValue;

        // This is a positional argument
        public AppConfigDefaultAttribute(string DefaultValue)
        {
            _DefaultValue = DefaultValue;
        }

        public string DefaultValue
        {
            get { return _DefaultValue; }
        }

        //// This is a named argument
        //public APIOperation PostType { get; set; }
    }

    
}
