using Microsoft.UI.Xaml.Controls;
using POSSystem.Helpers;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace POSSystem.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        private readonly ConfigHelper _configHelper;

        protected readonly string ConnectionString;

        public BaseViewModel()
        {
            _configHelper = new ConfigHelper();

            string host = _configHelper.GetDatabaseHost();
            string username = _configHelper.GetDatabaseUsername();
            string password = _configHelper.GetDatabasePassword();
            string database = _configHelper.GetDatabaseName();

            ConnectionString = $"Host={host};Username={username};Password={password};Database={database}";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
