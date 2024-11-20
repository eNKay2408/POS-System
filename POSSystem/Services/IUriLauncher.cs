using System;
using System.Threading.Tasks;

namespace POSSystem.Services
{
    public interface IUriLauncher
    {
        Task<bool> LaunchUriAsync(Uri uri);
    }
}
