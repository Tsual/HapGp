using HapGp.Enums;
using HapGp.ModelInstance;
using System;

namespace HapGp.Exceptions
{
    public class UserException : FPException
    {
        private Userx _User;

        public Userx User { get => _User; set => _User = value; }
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
