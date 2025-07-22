using CommunityToolkit.Mvvm.ComponentModel;


namespace SquoundApp.ViewModels
{
    public partial class BaseViewModel : ObservableObject
    {
        public bool IsNotBusy => !IsBusy;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotBusy))]
        private bool isBusy = false;

        [ObservableProperty]
        private string title = "";

        public BaseViewModel()
        {
            // Constructor logic can go here if needed.
        }
    }
}
