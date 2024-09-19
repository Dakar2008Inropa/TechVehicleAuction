using AuctionData.Models.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace AuctionData.Models.UserModels;

public abstract class User : Base, IUser
{
    private static readonly byte[] Key = Encoding.UTF8.GetBytes("tWu2A7CHJ5LUhVxa");
    private static readonly byte[] IV = Encoding.UTF8.GetBytes("qPV4Z6xLYeRkEWp3");

    private string? _encryptedPassword;

    public string? UserName { get; set; }
    public string? Password
    {
        get
        {
            return _encryptedPassword == null ? null : DecryptPassword(_encryptedPassword);
        }
        set
        {
            _encryptedPassword = value == null ? null : EncryptPassword(value);
        }
    }
    public ushort PostalCode { get; set; }
    public string? Discriminator { get; set; }

    public User()
    {
    }
    public override string ToString()
    {
        return $"ID: {Id} " +
            $"Username: {UserName} " +
            $"Postal code: {PostalCode}";
    }

    private static string EncryptPassword(string plainText)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = Key;
            aes.IV = IV;

            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using (var ms = new System.IO.MemoryStream())
            using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
            using (var sw = new StreamWriter(cs))
            {
                sw.Write(plainText);
                sw.Close();
                return Convert.ToBase64String(ms.ToArray());
            }
        }
    }

    private static string DecryptPassword(string cipherText)
    {
        using (Aes aes = Aes.Create())
        {
            aes.Key = Key;
            aes.IV = IV;

            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using (var ms = new System.IO.MemoryStream(Convert.FromBase64String(cipherText)))
            using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
            using (var sr = new StreamReader(cs))
            {
                return sr.ReadToEnd();
            }
        }
    }
}