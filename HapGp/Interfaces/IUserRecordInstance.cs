namespace HapGp.Interfaces
{
    public interface IUserRecordInstance
    {
        string this[string key] { get; set; }
        void Delete(string key);
    }
}