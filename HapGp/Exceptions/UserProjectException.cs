namespace HapGp.Exceptions
{
    public class UserProjectException: UserException
    {
        private int projectID;

        public int ProjectID { get => projectID; set => projectID = value; }
    }

    public class ProjectAlreadySelectException: UserProjectException
    {
        public override string Message => "您已经选择了该课程";
    }

}
