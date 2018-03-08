using HapGp.Enums;

namespace HapGp.Interfaces
{
    public interface IAppConfigs
    {
        string this[AppConfigEnum key] { get; set; }
        bool ContainsKey(AppConfigEnum Key);
    }
}