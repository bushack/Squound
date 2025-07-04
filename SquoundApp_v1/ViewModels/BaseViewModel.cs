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

        // The following properties and methods are commented out as they are now handled by the ObservableObject base class.
        // Uncomment these if you want to use the old implementation instead of CommunityToolkit.Mvvm.ComponentModel
        //public bool IsBusy
        //{
        //    get => isBusy;
        //    set
        //    {
        //        // If the new value is the same as the current value, do nothing.
        //        if (isBusy == value)
        //            return;

        //        isBusy = value;
        //        OnPropertyChanged();
        //        OnPropertyChanged(nameof(IsNotBusy));
        //    }
        //}

        //public string Title
        //{
        //    get => title;
        //    set
        //    {
        //        // If the new value is the same as the current value, do nothing.
        //        if (title == value)
        //            return;

        //        title = value;
        //        OnPropertyChanged();
        //    }
        //}

        //public bool IsNotBusy => !IsBusy;

        //public event PropertyChangedEventHandler? PropertyChanged;

        //public void OnPropertyChanged([CallerMemberName]string? name = null)
        //{
        //    // Question mark invokes the nullability feature in C# 8.0 and later.
        //    // It will check if PropertyChanged is not null before invoking it.
        //    // In this instance, it ensures that if no subscribers are attached to
        //    // the event, it won't throw a NullReferenceException.
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        //}
    }
}
