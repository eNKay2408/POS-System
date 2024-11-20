using POSSystem.Models;
using System.Threading.Tasks;

namespace POSSystem.Services
{
    public interface IGoogleOAuthService
    {
        Task<Employee> AuthenticateAsync();
    }
}
