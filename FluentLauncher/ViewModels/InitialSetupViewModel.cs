using CommunityToolkit.Mvvm.ComponentModel;

namespace FluentLauncher.ViewModels
{
    public class InitialSetupViewModel : ObservableObject
    {
        private int _selectedIndex;
        
        public int SelectedIndex
        {
            get => _selectedIndex;
            set => SetProperty(ref _selectedIndex, value);
        }
    }
}