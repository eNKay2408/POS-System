using System;
using System.Threading.Tasks;

namespace POSSystem.Services
{
    public class UriLauncher : IUriLauncher
    {
        public Task<bool> LaunchUriAsync(Uri uri) => Windows.System.Launcher.LaunchUriAsync(uri).AsTask();
    }
}
