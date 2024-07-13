namespace BaseArch.Application.Encryptions.Interfaces
{
    /// <summary>
    /// Encryption provider
    /// </summary>
    public interface IEncryptionProvider
    {
        /// <summary>
        /// Algorithm name to identify the concret class
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Encrypt a plain text
        /// </summary>
        /// <param name="plainText">Plain text</param>
        /// <param name="secrectKey">Secrect key</param>
        /// <returns>Encrypted text</returns>
        string Encrypt(string plainText, string secrectKey);

        /// <summary>
        /// Decrypt a encrypted text
        /// </summary>
        /// <param name="encryptedText">Encrypted text</param>
        /// <param name="secrectKey">Secrect key</param>
        /// <returns>Plain text</returns>
        string Decrypt(string encryptedText, string secrectKey);
    }
}
