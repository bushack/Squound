using CommunityToolkit.Mvvm.ComponentModel;


namespace SquoundApp_v1.ViewModels
{
    public partial class BaseViewModel : ObservableObject
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotBusy))]
        bool isBusy = false;

        [ObservableProperty]
        string title = "";

        public BaseViewModel()
        {
            // Constructor logic can go here if needed.
        }

        public bool IsNotBusy => !IsBusy;
    }
}
