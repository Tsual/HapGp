using HapGp.Enums;
using HapGp.ModelInstance;
using HapGp.Models;
using System;

namespace HapGp.Exceptions
{
    public class UserException : FPException
    {
        private Userx _User;

        public Userx User { get => _User; set => _User = value; }
    }

    public class ClassLocationMissing: UserException
    {
        private ProjectModel Project;

        public ProjectModel Project1 { get => Project; set => Project = value; }

        public override string Message => "课程["+Project?.ProjectName+"]的签到地点没有设定" ;
    }

    public class UserRoleException : UserException
    {
        private UserRole curRole;
        private UserRole reqRole;

        public UserRole ReqRole { get => reqRole; set => reqRole = value; }
        public UserRole CurRole { get => curRole; set => curRole = value; }

        public override string Message => "Your role is "+ curRole.ToString() +" ,Require :"+ reqRole.ToString();
    }

    public class UserPermissionException: UserException
    {
        private Permission _RequiredPermission;

        public Permission RequiredPermission { get => _RequiredPermission; set => _RequiredPermission = value; }
    }

    public class UserLIDExsistException : UserException
    {

    }

    public class UserNotLoginException : UserException
    {

    }

    public class UserRecordNotFindException : UserException
    {
        public override string Message => "Record not found";
    }

}
