using System;
using System.Threading.Tasks;

namespace POSSystem.Services
{
    public interface IUriLauncher
    {
        Task LaunchUriAsync(Uri uri);
    }
}
