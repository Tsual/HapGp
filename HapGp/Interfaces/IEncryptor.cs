namespace HapGp.Interfaces
{
    public interface IEncryptor
    {
        string Decrypt(string metaStr);
        string Encrypt(string metaStr);
    }
}