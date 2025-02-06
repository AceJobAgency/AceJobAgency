using System.Security.Cryptography;
using System.Text;

public static class EncryptionHelper {
    private static readonly string Key = Environment.GetEnvironmentVariable("SECRET_KEY") ?? "DefaultSecretKey";

    public static string Encrypt(string plainText) {
        using (Aes aes = Aes.Create()) {
            aes.Key = Encoding.UTF8.GetBytes(Key);
            aes.GenerateIV();
            using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV)) {
                byte[] inputBytes = Encoding.UTF8.GetBytes(plainText);
                byte[] encrypted = encryptor.TransformFinalBlock(inputBytes, 0, inputBytes.Length);
                return Convert.ToBase64String(aes.IV) + ":" + Convert.ToBase64String(encrypted);
            }
        }
    }

    public static string Decrypt(string encryptedText) {
        string[] parts = encryptedText.Split(':');
        byte[] iv = Convert.FromBase64String(parts[0]);
        byte[] cipherText = Convert.FromBase64String(parts[1]);

        using (Aes aes = Aes.Create()) {
            aes.Key = Encoding.UTF8.GetBytes(Key);
            aes.IV = iv;
            using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV)) {
                byte[] decryptedBytes = decryptor.TransformFinalBlock(cipherText, 0, cipherText.Length);
                return Encoding.UTF8.GetString(decryptedBytes);
            }
        }
    }
}
