using BaseArch.Application.Encryptions.Interfaces;
using BaseArch.Domain.DependencyInjection;
using System.Security.Cryptography;

namespace BaseArch.Application.Encryptions
{
    /// <inheritdoc />
    [DIService(DIServiceLifetime.Singleton)]
    public class AesEncryptionProvider : IEncryptionProvider
    {
        /// <inheritdoc />
        public string Name { get; } = "AES";

        private const int _iterations = 10000;

        /// <inheritdoc />
        public string Encrypt(string plainText, string secrectKey)
        {
            var plaintextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            var saltBytes = System.Text.Encoding.UTF8.GetBytes(secrectKey);

            var passwordBytes = new Rfc2898DeriveBytes(secrectKey, saltBytes, _iterations, HashAlgorithmName.SHA256);

            var encryptor = Aes.Create();
            encryptor.Key = passwordBytes.GetBytes(encryptor.KeySize / 8);
            encryptor.IV = passwordBytes.GetBytes(encryptor.BlockSize / 8);
            using var ms = new MemoryStream();
            using var cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write);

            cs.Write(plaintextBytes, 0, plaintextBytes.Length);
            cs.FlushFinalBlock();
            return Convert.ToBase64String(ms.ToArray());
        }

        /// <inheritdoc />
        public string Decrypt(string encryptedText, string secrectKey)
        {
            var encryptedBytes = Convert.FromBase64String(encryptedText);
            var saltBytes = System.Text.Encoding.UTF8.GetBytes(secrectKey);

            var passwordBytes = new Rfc2898DeriveBytes(secrectKey, saltBytes, _iterations, HashAlgorithmName.SHA256);

            var encryptor = Aes.Create();
            encryptor.Key = passwordBytes.GetBytes(encryptor.KeySize / 8);
            encryptor.IV = passwordBytes.GetBytes(encryptor.BlockSize / 8);
            using var ms = new MemoryStream();
            using var cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write);

            cs.Write(encryptedBytes, 0, encryptedBytes.Length);
            cs.FlushFinalBlock();
            return System.Text.Encoding.UTF8.GetString(ms.ToArray());
        }
    }
}
