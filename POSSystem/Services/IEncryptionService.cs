using System.Threading.Tasks;

namespace POSSystem.Services
{
    public interface IEncryptionService
    {
        Task<string> EncryptAsync(string plainText);
        Task<string> DecryptAsync(string encryptedText);
    }
}
