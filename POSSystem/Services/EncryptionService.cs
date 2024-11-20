using System.Threading.Tasks;
using Windows.Security.Cryptography.DataProtection;
using Windows.Security.Cryptography;
using Windows.Storage.Streams;
using System;

namespace POSSystem.Services
{
    public class EncryptionService : IEncryptionService
    {
        public async Task<string> EncryptAsync(string text)
        {
            var provider = new DataProtectionProvider("LOCAL=user");
            IBuffer buffer = CryptographicBuffer.ConvertStringToBinary(text, BinaryStringEncoding.Utf8);
            IBuffer protectedBuffer = await provider.ProtectAsync(buffer);
            return CryptographicBuffer.EncodeToBase64String(protectedBuffer);
        }

        public async Task<string> DecryptAsync(string encryptedText)
        {
            var provider = new DataProtectionProvider("LOCAL=user");
            IBuffer protectedBuffer = CryptographicBuffer.DecodeFromBase64String(encryptedText);
            IBuffer unprotectedBuffer = await provider.UnprotectAsync(protectedBuffer);
            return CryptographicBuffer.ConvertBinaryToString(BinaryStringEncoding.Utf8, unprotectedBuffer);
        }
    }
}
