namespace BaseArch.Application.Encryptions.Interfaces
{
    public interface IEncryptor
    {
        string Name { get; }
        string Encrypt(string plainText, string secrectKey);
        string Decrypt(string encryptedText, string secrectKey);
    }
}
