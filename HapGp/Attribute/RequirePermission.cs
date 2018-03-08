using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using HapGp.Enums;

namespace HapGp.Attribute
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public class RequirePermissionAttribute : System.Attribute
    {
        private readonly Permission? _Permission = null;

        public RequirePermissionAttribute(Permission? _Permission)
        {
            this._Permission = _Permission;
        }

        public Permission? Permission { get => _Permission; }
    }
}
