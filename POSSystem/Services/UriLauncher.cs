﻿using System;
using System.Threading.Tasks;
using Windows.System;

namespace POSSystem.Services
{
    public class UriLauncher : IUriLauncher
    {
        public Task<bool> LaunchUriAsync(Uri uri) => Launcher.LaunchUriAsync(uri).AsTask();
    }
}
